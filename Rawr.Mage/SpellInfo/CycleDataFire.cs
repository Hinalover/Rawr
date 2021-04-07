using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public static class CombustionCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = baseCycle.Name;
            cycle.AreaEffect = baseCycle.AreaEffect;

            Spell Combustion = castingState.GetSpell(SpellId.Combustion);

            // 1 combustion in 10 seconds
            // the dot duplication is currently calculated in individual spells
            // consider splitting that out for display purposes

            cycle.AddSpell(needsDisplayCalculations, Combustion, 1);
            cycle.AddCycle(needsDisplayCalculations, baseCycle, (10 - Combustion.CastTime) / baseCycle.CastTime);
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class LivingBombCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            if (needsDisplayCalculations)
            {
                cycle.Name = "Living Bomb+" + baseCycle.Name;
            }
            else
            {
                cycle.Name = baseCycle.Name;
            }

            Spell LB;
            if (baseCycle.AreaEffect)
            {
                LB = castingState.GetSpell(SpellId.LivingBombAOE);
            }
            else
            {
                LB = castingState.GetSpell(SpellId.LivingBomb);
            }

            double wait = 2f / castingState.CastingSpeed * 0.5f;

            // TODO: real aoe calc

            cycle.AreaEffect = baseCycle.AreaEffect;
            cycle.AddSpell(needsDisplayCalculations, LB, 1);
            cycle.AddCycle(needsDisplayCalculations, baseCycle, (LB.DotFullDuration + wait - LB.CastTime) / baseCycle.CastTime);
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class FBIBPyro
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, float effectCrit)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "FBIBPyro";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell IB = castingState.GetSpell(SpellId.InfernoBlast);
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastPOMDotUptime);

            // all direct fire spell crits affect hot streak, including Pyro

            // HSxyHUzw
            // x=real hot streak proc
            // y=known hot streak proc
            // z=real heating up proc
            // w=known heating up proc

            //HS--HU--: S0
            //FB  =>  HS--HU--  (1-FBc)
            //    =>  HS--HU+-  FBc

            //HS--HU+-: S1
            //FB  =>  HS--HU-+  (1-FBc)
            //    =>  HS+-HU-+  FBc

            //HS--HU-+: S2
            //FB  =>  HS--HU--  (1-FBc)*(1-X)
            //    =>  HS--HU+-  FBc*(1-X)
            //IB  =>  HS++HU--  X

            //HS+-HU-+: S3
            //FB  =>  HS++HU--  (1-FBc)*(1-X)
            //    =>  HS++HU+-  FBc*(1-X)
            //IB  =>  HS++HU--  X

            //HS++HU--: S4
            //P!  =>  HS--HU--  (1-Pc)
            //    =>  HS--HU++  Pc

            //HS++HU+-: S5
            //P!  =>  HS--HU--  (1-Pc)
            //    =>  HS++HU--  Pc

            //HS--HU++: S6
            //FB  =>  HS--HU-+  (1-FBc)*(1-X)
            //    =>  HS+-HU-+  FBc*(1-X)
            //IB  =>  HS++HU--  X



            //S0 = S0 * (1-FBc) + S2 * (1-FBc)*(1-X) + S4 * (1-Pc) + S5 * (1-Pc)
            //S1 = S0 * FBc + S2 * FBc*(1-X)
            //S2 = S1 * (1-FBc) + S6 * (1-FBc)*(1-X)
            //S3 = S1 * FBc + S6 * FBc*(1-X)
            //S4 = S2 * X + S3 * (1-FBc)*(1-X) + S3 * X + S5 * Pc + S6 * X
            //S5 = S3 * FBc*(1-X)
            //S6 = S4 * Pc

            //symbolic solve:

            //FB = S0 + S1 + S2 * (1-X) + S3 * (1-X) + S6 * (1-X)
            //IB = S2 * X + S3 * X + S6 * X
            //P! = S4 + S5


            //S0=((FBc^2*Pc^2+(-2*FBc^2+2*FBc-1)*Pc)*X^2+(-2*FBc^2*Pc^2+(3*FBc^2-3*FBc+2)*Pc+FBc^2-FBc)*X+FBc^2*Pc^2+(FBc-FBc^2)*Pc-FBc^2+FBc-1)/   (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S1=((FBc^3*Pc^2+(-FBc^3+FBc^2-FBc)*Pc)*X^2+((2*FBc^3-2*FBc^2+2*FBc)*Pc-2*FBc^3*Pc^2)*X+FBc^3*Pc^2+(FBc^2-FBc^3)*Pc-FBc)/              (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S2=-((FBc^2-FBc)*Pc*X-FBc^2+FBc)/                                                                                                     (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S3=(FBc^2*Pc*X-FBc^2)/                                                                                                                (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S4=((FBc^3*Pc-FBc^3+FBc^2-FBc)*X-FBc^3*Pc+FBc^3-FBc^2)/                                                                               (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S5=-(FBc^3*Pc*X^2+(-FBc^3*Pc-FBc^3)*X+FBc^3)/                                                                                         (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)
            //S6=((FBc^3*Pc^2+(-FBc^3+FBc^2-FBc)*Pc)*X-FBc^3*Pc^2+(FBc^3-FBc^2)*Pc)/                                                                (((FBc^3+FBc^2)*Pc^2+(-2*FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-FBc^3-2*FBc^2)*Pc^2+(3*FBc^3+2*FBc^2-FBc+2)*Pc+2*FBc^2-2*FBc)*X+FBc^2*Pc^2+(-FBc^3-FBc^2+FBc)*Pc-2*FBc^2-FBc-1)


            //S0+S1    = ((FBc^3+FBc^2)*Pc^2+(-FBc^3-FBc^2+FBc-1)*Pc)*X^2+((-2*FBc^3-2*FBc^2)*Pc^2+(2*FBc^3+FBc^2-FBc+2)*Pc+FBc^2-FBc)*X+(FBc^3+FBc^2)*Pc^2+(FBc-FBc^3)*Pc-FBc^2-1
            //S2+S3+S6 = (FBc^3*Pc^2+(FBc^2-FBc^3)*Pc)*X-FBc^3*Pc^2+(FBc^3-FBc^2)*Pc-FBc
            //S4+S5    = -FBc^3*Pc*X^2+(2*FBc^3*Pc+FBc^2-FBc)*X-FBc^3*Pc-FBc^2

            // estimate chance that IB is ready when we get to state S236
            // S236*X = (S236*X*IBcast + (S01+S236*(1-X))*FBcast + S45*Pcast) / interval
            // X=(Pcast*S45+FBcast*S236+FBcast*S01)/((interval-IBcast+FBcast)*S236)


            float FBc = FB.CritRate + effectCrit * FB.CritRateMultiplier;
            float Pc = Pyro.CritRate + effectCrit * Pyro.CritRateMultiplier;
            float FBc2 = FBc * FBc;
            float FBc3 = FBc2 * FBc;
            float Pc2 = Pc * Pc;
            float X = 0.1f;
            float X2 = X * X;
            float S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            float S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            float S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;
            X = (Pyro.CastTime * S45 + FB.CastTime * S236 + FB.CastTime * S01) / ((10f - IB.CastTime + FB.CastTime) * S236);
            X2 = X * X;
            S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;

            // model for Pyro dot uptime
            // TODO update with model including IB, otherwise it might underestimate refreshes

            // S0: no proc, 0 count
            // FB   => S0   1-FBc
            //      => S1   FBc
            // S1: no proc, 1 count
            // FB   => S0   1-FBc
            //      => S2   FBc
            // S2: proc, 0 count
            // Pyro  => S0   1

            // k0 = 1
            // k1 = 0
            // k2 = 0

            // n=>n+1

            // k0' = (k0+k1)*(1-FBc)
            // k1' = k0*FBc
            // k2' = k1*FBc

            float averageDuration = 0f;
            float C = FB.CritRate + effectCrit;
            float k0 = 1;
            float k1 = 0;
            float k2 = 0;
            float totalChance = k2;
            float n = 2; // heuristic filler to account for realistic dot uptime

            //averageDuration += Math.Min((Pyro.CastTime + n * FB.CastTime), Pyro.DotFullDuration) * k2;            

            while ((Pyro.CastTime + n * FB.CastTime) < Pyro.DotFullDuration)
            {
                float tmp = k1;
                k2 = k1 * FBc;
                k1 = k0 * FBc;
                k0 = (k0 + tmp) * (1 - FBc);
                totalChance += k2;
                n++;
                averageDuration += Math.Min((Pyro.CastTime + n * FB.CastTime), Pyro.DotFullDuration) * k2;
            }
            averageDuration += Pyro.DotFullDuration * (1 - totalChance);

            //FB = S0 + S1 + S2 * (1-X) + S3 * (1-X) + S6 * (1-X)
            //IB = S2 * X + S3 * X + S6 * X
            //P! = S4 + S5

            cycle.AddSpell(needsDisplayCalculations, FB, S01 + S236 * (1 - X));
            cycle.AddSpell(needsDisplayCalculations, IB, S236 * X);
            cycle.AddSpell(needsDisplayCalculations, Pyro, S45, averageDuration / Pyro.DotFullDuration);
            cycle.Calculate();
            return cycle;
        }
    }


    public static class FFBIBPyro
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, float effectCrit)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "FFBIBPyro";

            Spell FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Spell IB = castingState.GetSpell(SpellId.InfernoBlast);
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastPOMDotUptime);

            float FBc = FFB.CritRate + effectCrit * FFB.CritRateMultiplier;
            float Pc = Pyro.CritRate + effectCrit * Pyro.CritRateMultiplier;
            float FBc2 = FBc * FBc;
            float FBc3 = FBc2 * FBc;
            float Pc2 = Pc * Pc;
            float X = 0.1f;
            float X2 = X * X;
            float S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            float S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            float S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;
            X = (Pyro.CastTime * S45 + FFB.CastTime * S236 + FFB.CastTime * S01) / ((10f - IB.CastTime + FFB.CastTime) * S236);
            X2 = X * X;
            S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;

            float averageDuration = 0f;
            float C = FFB.CritRate + effectCrit;
            float k0 = 1;
            float k1 = 0;
            float k2 = 0;
            float totalChance = k2;
            float n = 2;

            while ((Pyro.CastTime + n * FFB.CastTime) < Pyro.DotFullDuration)
            {
                float tmp = k1;
                k2 = k1 * FBc;
                k1 = k0 * FBc;
                k0 = (k0 + tmp) * (1 - FBc);
                totalChance += k2;
                n++;
                averageDuration += Math.Min((Pyro.CastTime + n * FFB.CastTime), Pyro.DotFullDuration) * k2;
            }
            averageDuration += Pyro.DotFullDuration * (1 - totalChance);

            cycle.AddSpell(needsDisplayCalculations, FFB, S01 + S236 * (1 - X));
            cycle.AddSpell(needsDisplayCalculations, IB, S236 * X);
            cycle.AddSpell(needsDisplayCalculations, Pyro, S45, averageDuration / Pyro.DotFullDuration);
            cycle.Calculate();
            return cycle;
        }
    }

    public static class ScIBPyro
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, float effectCrit)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ScIBPyro";

            Spell Sc = castingState.GetSpell(SpellId.Scorch);
            Spell IB = castingState.GetSpell(SpellId.InfernoBlast);
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastPOMDotUptime);

            float FBc = Sc.CritRate + effectCrit * Sc.CritRateMultiplier;
            float Pc = Pyro.CritRate + effectCrit * Pyro.CritRateMultiplier;
            float FBc2 = FBc * FBc;
            float FBc3 = FBc2 * FBc;
            float Pc2 = Pc * Pc;
            float X = 0.1f;
            float X2 = X * X;
            float S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            float S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            float S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;
            X = (Pyro.CastTime * S45 + Sc.CastTime * S236 + Sc.CastTime * S01) / ((10f - IB.CastTime + Sc.CastTime) * S236);
            X2 = X * X;
            S01 = ((FBc3 + FBc2) * Pc2 + (-FBc3 - FBc2 + FBc - 1) * Pc) * X2 + ((-2 * FBc3 - 2 * FBc2) * Pc2 + (2 * FBc3 + FBc2 - FBc + 2) * Pc + FBc2 - FBc) * X + (FBc3 + FBc2) * Pc2 + (FBc - FBc3) * Pc - FBc2 - 1;
            S236 = (FBc3 * Pc2 + (FBc2 - FBc3) * Pc) * X - FBc3 * Pc2 + (FBc3 - FBc2) * Pc - FBc;
            S45 = -FBc3 * Pc * X2 + (2 * FBc3 * Pc + FBc2 - FBc) * X - FBc3 * Pc - FBc2;

            float averageDuration = 0f;
            float C = Sc.CritRate + effectCrit;
            float k0 = 1;
            float k1 = 0;
            float k2 = 0;
            float totalChance = k2;
            float n = 2;

            while ((Pyro.CastTime + n * Sc.CastTime) < Pyro.DotFullDuration)
            {
                float tmp = k1;
                k2 = k1 * FBc;
                k1 = k0 * FBc;
                k0 = (k0 + tmp) * (1 - FBc);
                totalChance += k2;
                n++;
                averageDuration += Math.Min((Pyro.CastTime + n * Sc.CastTime), Pyro.DotFullDuration) * k2;
            }
            averageDuration += Pyro.DotFullDuration * (1 - totalChance);

            cycle.AddSpell(needsDisplayCalculations, Sc, S01 + S236 * (1 - X));
            cycle.AddSpell(needsDisplayCalculations, IB, S236 * X);
            cycle.AddSpell(needsDisplayCalculations, Pyro, S45, averageDuration / Pyro.DotFullDuration);
            cycle.Calculate();
            return cycle;
        }
    }

    public class FireCycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public float ScorchDuration { get; set; }
            public float LivingBombDuration { get; set; }
            public int HotStreakCount { get; set; }
            public float PyroDuration { get; set; }
            public bool PyroRegistered { get; set; }
            public float Tier10TwoPieceDuration { get; set; }
        }

        public Spell[] Pyro, FB, LB, Sc;

        //private float HS;
        private float T8;
        private bool T10;

        private bool maintainScorch = false;

        private float LBDotCritRate;

        public FireCycleGenerator(CastingState castingState)
        {
            FB = new Spell[2];
            Sc = new Spell[2];
            LB = new Spell[2];
            Pyro = new Spell[2];

            FB[0] = castingState.GetSpell(SpellId.Fireball);
            Sc[0] = castingState.GetSpell(SpellId.Scorch);
            LB[0] = castingState.GetSpell(SpellId.LivingBomb);
            Pyro[0] = castingState.GetSpell(SpellId.PyroblastPOM); // does not account for dot uptime
            FB[0].Label = "Fireball";
            Sc[0].Label = "Scorch";
            LB[0].Label = "Living Bomb";
            Pyro[0].Label = "Pyroblast";

            FB[1] = castingState.GetSpell(SpellId.Fireball);
            Sc[1] = castingState.GetSpell(SpellId.Scorch);
            LB[1] = castingState.GetSpell(SpellId.LivingBomb);
            Pyro[1] = castingState.GetSpell(SpellId.PyroblastPOM); // does not account for dot uptime
            FB[1].Label = "2T10:Fireball";
            Sc[1].Label = "2T10:Scorch";
            LB[1].Label = "2T10:Living Bomb";
            Pyro[1].Label = "2T10:Pyroblast";

            //HS = 1f;
            T8 = 0;
            //maintainScorch = castingState.CalculationOptions.MaintainScorch;
            LBDotCritRate = castingState.FireCritRate;
            T10 = castingState.Solver.Mage2T10;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(0.0f, 0.0f, 0, 0.0f, false, 0.0f);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell Sc = this.Sc[s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            Spell FB = this.FB[s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            Spell LB = this.LB[s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            Spell Pyro = this.Pyro[s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            if (maintainScorch && s.ScorchDuration < Sc.CastTime)
            {
                // LB explosion and/or ticks can happen during the cast
                // account for all combinations of hot streak interaction
                float castTime = Sc.CastTime;
                int ticksLeft = Math.Max(0, (int)(s.LivingBombDuration / 3.0f));
                int ticksLeftAfter = Math.Max(0, (int)((s.LivingBombDuration - castTime) / 3.0f));
                int hotStreakEvents = ticksLeft - ticksLeftAfter;
                //if (!livingBombGlyph)
                {
                    hotStreakEvents = 0;
                }
                bool explosion = false;
                if (ticksLeft > 0 && ticksLeftAfter == 0)
                {
                    explosion = true;
                    hotStreakEvents++;
                }
                for (int i = 0; i < (1 << (hotStreakEvents + 1)); i++)
                {
                    int k = i;
                    int hsc = s.HotStreakCount;
                    float pd = Math.Max(0f, s.PyroDuration - castTime);
                    bool pr = s.PyroDuration > castTime;
                    float chance = 1.0f;
                    for (int j = 0; j < hotStreakEvents; j++)
                    {
                        float crit = (j == hotStreakEvents - 1 && explosion) ? LB.CritRate : LBDotCritRate;
                        if (k % 2 == 1)
                        {
                            chance *= crit;
                            if (hsc == 1)
                            {
                                pd = 10.0f - (s.LivingBombDuration - ticksLeft * 3);
                                pr = true;
                            }
                            hsc = (hsc + 1) % 2;
                        }
                        else
                        {
                            chance *= (1 - crit);
                            hsc = 0;
                        }
                        k = k >> 1;
                    }
                    if (k % 2 == 1)
                    {
                        chance *= Sc.CritRate;
                        if (hsc == 1)
                        {
                            pd = 10.0f;
                        }
                        hsc = (hsc + 1) % 2;
                    }
                    else
                    {
                        chance *= (1 - Sc.CritRate);
                        hsc = 0;
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = Sc,
                        TargetState = GetState(
                            30.0f + Sc.CastTime,
                            Math.Max(0f, s.LivingBombDuration - castTime),
                            hsc,
                            pd,
                            pr,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - Sc.CastTime)
                        ),
                        TransitionProbability = chance
                    });
                }
            }
            else if (s.LivingBombDuration <= 0f)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = LB,
                    TargetState = GetState(
                        Math.Max(0f, s.ScorchDuration - LB.CastTime),
                        12f - LB.CastTime,
                        s.HotStreakCount,
                        Math.Max(0f, s.PyroDuration - LB.CastTime),
                        s.PyroDuration > LB.CastTime,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - LB.CastTime)
                    ),
                    TransitionProbability = 1
                });
            }
            else if (s.PyroRegistered)
            {
                float castTime = Pyro.CastTime;
                int ticksLeft = Math.Max(0, (int)(s.LivingBombDuration / 3.0f));
                int ticksLeftAfter = Math.Max(0, (int)((s.LivingBombDuration - castTime) / 3.0f));
                int hotStreakEvents = ticksLeft - ticksLeftAfter;
                //if (!livingBombGlyph)
                {
                    hotStreakEvents = 0;
                }
                bool explosion = false;
                if (ticksLeft > 0 && ticksLeftAfter == 0)
                {
                    explosion = true;
                    hotStreakEvents++;
                }
                for (int i = 0; i < (1 << hotStreakEvents); i++)
                {
                    int k = i;
                    int hsc = s.HotStreakCount;
                    float pd1 = Math.Max(0f, s.PyroDuration - castTime);
                    bool pr1 = s.PyroDuration > castTime;
                    float pd2 = 0.0f;
                    bool pr2 = false;
                    float chance = 1.0f;
                    for (int j = 0; j < hotStreakEvents; j++)
                    {
                        float crit = (j == hotStreakEvents - 1 && explosion) ? LB.CritRate : LBDotCritRate;
                        if (k % 2 == 1)
                        {
                            chance *= crit;
                            if (hsc == 1)
                            {
                                pd1 = 10.0f - (s.LivingBombDuration - ticksLeft * 3);
                                pr1 = true;
                                pd2 = 10.0f - (s.LivingBombDuration - ticksLeft * 3);
                                pr2 = true;
                            }
                            hsc = (hsc + 1) % 2;
                        }
                        else
                        {
                            chance *= (1 - crit);
                            hsc = 0;
                        }
                        k = k >> 1;
                    }
                    if (T8 > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = Pyro,
                            TargetState = GetState(
                                Math.Max(0f, s.ScorchDuration - castTime),
                                Math.Max(0f, s.LivingBombDuration - castTime),
                                hsc,
                                pd1,
                                pr1,
                                T10 ? Math.Max(0.0f, 5.0f - Pyro.CastTime) : 0f
                            ),
                            TransitionProbability = chance * T8
                        });
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = Pyro,
                        TargetState = GetState(
                            Math.Max(0f, s.ScorchDuration - castTime),
                            Math.Max(0f, s.LivingBombDuration - castTime),
                            hsc,
                            pd2,
                            pr2,
                            T10 ? Math.Max(0.0f, 5.0f - Pyro.CastTime) : 0f
                        ),
                        TransitionProbability = chance * (1 - T8)
                    });
                }
            }
            else
            {
                float castTime = FB.CastTime;
                int ticksLeft = Math.Max(0, (int)(s.LivingBombDuration / 3.0f));
                int ticksLeftAfter = Math.Max(0, (int)((s.LivingBombDuration - castTime) / 3.0f));
                int hotStreakEvents = ticksLeft - ticksLeftAfter;
                //if (!livingBombGlyph)
                {
                    hotStreakEvents = 0;
                }
                bool explosion = false;
                if (ticksLeft > 0 && ticksLeftAfter == 0)
                {
                    explosion = true;
                    hotStreakEvents++;
                }
                for (int i = 0; i < (1 << (hotStreakEvents + 1)); i++)
                {
                    int k = i;
                    int hsc = s.HotStreakCount;
                    float pd = Math.Max(0f, s.PyroDuration - castTime);
                    bool pr = s.PyroDuration > castTime;
                    float chance = 1.0f;
                    for (int j = 0; j < hotStreakEvents; j++)
                    {
                        float crit = (j == hotStreakEvents - 1 && explosion) ? LB.CritRate : LBDotCritRate;
                        if (k % 2 == 1)
                        {
                            chance *= crit;
                            if (hsc == 1)
                            {
                                pd = 10.0f - (s.LivingBombDuration - ticksLeft * 3);
                                pr = true;
                            }
                            hsc = (hsc + 1) % 2;
                        }
                        else
                        {
                            chance *= (1 - crit);
                            hsc = 0;
                        }
                        k = k >> 1;
                    }
                    if (k % 2 == 1)
                    {
                        chance *= FB.CritRate;
                        if (hsc == 1)
                        {
                            pd = 10.0f;
                        }
                        hsc = (hsc + 1) % 2;
                    }
                    else
                    {
                        chance *= (1 - FB.CritRate);
                        hsc = 0;
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = FB,
                        TargetState = GetState(
                            Math.Max(0f, s.ScorchDuration - castTime),
                            Math.Max(0f, s.LivingBombDuration - castTime),
                            hsc,
                            pd,
                            pr,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - FB.CastTime)
                        ),
                        TransitionProbability = chance
                    });
                }
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(float scorchDuration, float livingBombDuration, int hotStreakCount, float pyroDuration, bool pyroRegistered, float tier10TwoPieceDuration)
        {
            string name = string.Format("Sc{0},LB{1},HS{2},Pyro{3}{4},2T10={5}", scorchDuration, livingBombDuration, hotStreakCount, pyroDuration, pyroRegistered ? "+" : "-", tier10TwoPieceDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, ScorchDuration = scorchDuration, LivingBombDuration = livingBombDuration, HotStreakCount = hotStreakCount, PyroDuration = pyroDuration, PyroRegistered = pyroRegistered, Tier10TwoPieceDuration = tier10TwoPieceDuration };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ScorchDuration != b.ScorchDuration || a.LivingBombDuration != b.LivingBombDuration || a.HotStreakCount != b.HotStreakCount || a.PyroDuration != b.PyroDuration || a.PyroRegistered != b.PyroRegistered || a.Tier10TwoPieceDuration != b.Tier10TwoPieceDuration);
        }
    }
}
