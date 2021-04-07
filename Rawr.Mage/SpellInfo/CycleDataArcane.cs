using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public static class IncantersWardCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            if (needsDisplayCalculations)
            {
                cycle.Name = "Incanter's Ward+" + baseCycle.Name;
            }
            else
            {
                cycle.Name = baseCycle.Name;
            }

            Spell IncantersWard = castingState.GetSpell(SpellId.IncantersWard);

            // 1 ward every 25 seconds

            cycle.AreaEffect = baseCycle.AreaEffect;
            cycle.AddSpell(needsDisplayCalculations, IncantersWard, 1);
            cycle.AddCycle(needsDisplayCalculations, baseCycle, (25 - IncantersWard.CastTime) / baseCycle.CastTime);
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class MirrorImageCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = baseCycle.Name;
            cycle.AreaEffect = baseCycle.AreaEffect;

            // uptime
            float fightDuration = castingState.CalculationOptions.FightDuration;
            float effectDuration = Solver.MirrorImageDuration;
            float effectCooldown = Solver.MirrorImageCooldown;
            int activations = 0;
            float total;
            if (fightDuration < effectDuration)
            {
                total = fightDuration;
                activations = 1;
            }
            else
            {
                total = effectDuration;
                activations = 1;
                fightDuration -= effectDuration;
                int count = (int)(fightDuration / effectCooldown);
                total += effectDuration * count;
                activations += count;
                fightDuration -= effectCooldown * count;
                fightDuration -= effectCooldown - effectDuration;
                if (fightDuration > 0) 
                {
                    total += fightDuration;
                    activations++;
                }
            }          

            Spell mirrorImage = castingState.GetSpell(SpellId.MirrorImage);
           
            // activations * gcd in fightDuration
            float gcd = castingState.Solver.BaseGlobalCooldown + castingState.CalculationOptions.LatencyGCD;

            cycle.AddCycle(needsDisplayCalculations, baseCycle, (castingState.CalculationOptions.FightDuration - activations * gcd) / baseCycle.CastTime);
            cycle.CastTime += activations * gcd;
            cycle.costPerSecond += activations * (int)(0.02 * castingState.CalculationOptions.BaseMana);
            //effectDamagePerSecond += (mirrorImage.AverageDamage + spellPower * mirrorImage.DamagePerSpellPower) / mirrorImage.CastTime;
            cycle.damagePerSecond += total * mirrorImage.AverageDamage / mirrorImage.CastTime;
            cycle.DpsPerSpellPower += total * mirrorImage.DamagePerSpellPower / mirrorImage.CastTime;
            if (needsDisplayCalculations)
            {
                cycle.Spell.Add(new Cycle.SpellData() { Spell = mirrorImage, Weight = (total / mirrorImage.CastTime) });
            }
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class RuneOfPowerCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = baseCycle.Name;
            cycle.AreaEffect = baseCycle.AreaEffect;

            float fightDuration = castingState.CalculationOptions.FightDuration;
            float effectDuration = castingState.CalculationOptions.RuneOfPowerInterval;
            int activations = 0;
            if (fightDuration < effectDuration)
            {
                activations = 0;
            }
            else
            {
                activations = 0;
                fightDuration -= effectDuration;
                int count = (int)(fightDuration / effectDuration);
                activations += count;
                fightDuration -= effectDuration * count;
                if (fightDuration > 0)
                {
                    activations++;
                }
            }          

            float gcd = castingState.Solver.BaseGlobalCooldown + castingState.CalculationOptions.LatencyGCD;
            // 1 gcd lost per minute except at start

            cycle.AddCycle(needsDisplayCalculations, baseCycle, (castingState.CalculationOptions.FightDuration - activations * gcd) / baseCycle.CastTime);
            cycle.CastTime += activations * gcd;
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class NetherTempestCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            if (needsDisplayCalculations)
            {
                cycle.Name = "Nether Tempest+" + baseCycle.Name;
            }
            else
            {
                cycle.Name = baseCycle.Name;
            }

            Spell NT;
            if (baseCycle.AreaEffect)
            {
                NT = castingState.GetSpell(SpellId.NetherTempestDOTAOE);
            }
            else
            {
                NT = castingState.GetSpell(SpellId.NetherTempestDOT);
            }

            //|------|------|...X-------|----
            //---X--------------......X------


            //|--X---|------|----
            //---......X---------


            //t = dot tick interval
            //o = offset
            //N = number of ticks
            //d = tick damage
            //dps = average filler dps

            //(N*d+(N*t+o-1.5*t)*dps)/(N*t+o)               : delay
            //(N*d-d+(N*t-t-1.5*t)*dps)/(N*t-t)       : refresh early


            //(N*d+(N*t-1.5*t)*dps +o*dps)/(N*t+o)
            //(N*d+(N*t-1.5*t)*dps -d-t*dps)/(N*t-t)

            //B := N*d+(N*t-1.5*t)*dps

            //(B+o*dps)/(N*t+o)
            //(B-d-t*dps)/(N*t-t)

            //when is delaying better:

            //(B+o*dps)/(N*t+o) >= (B-d-t*dps)/(N*t-t)

            //(B+o*dps)*(N*t-t) >= (B-d-t*dps)*(N*t+o)

            //B*(N-1)*t + o*dps*(N-1)*t >= (B-d-t*dps)*N*t + (B-d-t*dps)*o

            //o * [dps*N*t - B + d] >= (B - (d+t*dps)*N) * t

            // assuming 2 sec base cast for filler purposes, expand on this
            // assuming dot perfectly out of sync with base cycle
            // assuming offset is uniformly distributed on 0..2 interval

            // values 1..2 allow for perfect refresh
            // values 0..1 require determination of early refresh vs delay

            double N = NT.DotDuration / NT.DotTickInterval + NT.DotExtraTicks;
            double t = NT.DotFullDuration / N;
            double d = NT.DotAverageDamage / N;
            double dps = baseCycle.damagePerSecond;

            double B = N * d + (N * t - 1.5 * t) * dps;
            double V1 = (B - (d + t * dps) * N) * t;
            double V2 = dps * N * t - B + d;

            double o = V1 / V2;

            double delay;

            if (V2 >= 0)
            {
                // delay when >= o
                if (o < 0)
                {
                    o = 0;
                }
                if (o < 1)
                {
                    delay = 1.0 - o;
                    o = (o + 1.0) / 2.0;
                }
                else
                {
                    delay = 0;
                }
            }
            else
            {
                // delay when <= o
                if (o > 1)
                {
                    o = 1;
                }
                if (o > 0)
                {
                    delay = o;
                    o = 0.5 * o;
                }
                else
                {
                    delay = 0;
                }
            }

            double w, wnt;

            // TODO: real aoe calc

            cycle.AreaEffect = baseCycle.AreaEffect;
            // values 1..2 allow for perfect refresh
            //cycle.AddSpell(needsDisplayCalculations, NT, 1, 1);
            //cycle.AddCycle(needsDisplayCalculations, baseCycle, (NT.DotFullDuration - NT.CastTime) / baseCycle.CastTime);
            wnt = 1;
            w = (NT.DotFullDuration - NT.CastTime) / baseCycle.CastTime;
            // values 0..1 require determination of early refresh vs delay
            //cycle.AddSpell(needsDisplayCalculations, NT, delay, 1);
            //cycle.AddCycle(needsDisplayCalculations, baseCycle, delay * (NT.DotFullDuration + o - NT.CastTime) / baseCycle.CastTime);
            wnt += delay;
            w += delay * (NT.DotFullDuration + o - NT.CastTime) / baseCycle.CastTime;
            if (delay < 1)
            {
                cycle.AddSpell(needsDisplayCalculations, NT, (1 - delay), (N - 1) / N);
                //cycle.AddCycle(needsDisplayCalculations, baseCycle, (1 - delay) * (NT.DotFullDuration * (N - 1) / N - NT.CastTime) / baseCycle.CastTime);
                w += (1 - delay) * (NT.DotFullDuration * (N - 1) / N - NT.CastTime) / baseCycle.CastTime;
            }
            cycle.AddSpell(needsDisplayCalculations, NT, wnt, 1);
            cycle.AddCycle(needsDisplayCalculations, baseCycle, w);
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class AB4AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4AM";

            Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);

			cycle.AddSpell(needsDisplayCalculations, AB4, 0.714285932538934);
			cycle.AddSpell(needsDisplayCalculations, AM4, 0.285714373308031);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB4ABar4AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4ABar4AM";

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            Spell ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);

			cycle.AddSpell(needsDisplayCalculations, AB3, 0.136477806159648);
			cycle.AddSpell(needsDisplayCalculations, AB2, 0.145641419423834);
			cycle.AddSpell(needsDisplayCalculations, AB1, 0.146799011716437);
			cycle.AddSpell(needsDisplayCalculations, AB0, 0.146799007341485);
			cycle.AddSpell(needsDisplayCalculations, AM4, 0.266004963327073);
			cycle.AddSpell(needsDisplayCalculations, AM3, 0.010321213964596);
			cycle.AddSpell(needsDisplayCalculations, AM2, 0.00115759663305476);
			cycle.AddSpell(needsDisplayCalculations, ABar4, 0.146799002966534);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB4ABar34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4ABar34AM";

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);

            cycle.AddSpell(needsDisplayCalculations, AB3, 0.0363780938249129);
            cycle.AddSpell(needsDisplayCalculations, AB2, 0.177147834414143);
            cycle.AddSpell(needsDisplayCalculations, AB1, 0.173655531988763);
            cycle.AddSpell(needsDisplayCalculations, AB0, 0.177147823959388);
            cycle.AddSpell(needsDisplayCalculations, AM4, 0.114260880411217);
            cycle.AddSpell(needsDisplayCalculations, AM3, 0.140769741673381);
            cycle.AddSpell(needsDisplayCalculations, AM1, 0.003492297145963);
            cycle.AddSpell(needsDisplayCalculations, ABar4, 0.177147840777711);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB34ABar34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB34ABar34AM";

            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);

            cycle.AddSpell(needsDisplayCalculations, AB2, 0.161307174011725);
            cycle.AddSpell(needsDisplayCalculations, AB1, 0.187342471038971);
            cycle.AddSpell(needsDisplayCalculations, AB0, 0.193497922875122);
            cycle.AddSpell(needsDisplayCalculations, AM4, 0.070981994675296);
            cycle.AddSpell(needsDisplayCalculations, AM3, 0.155026325541037);
            cycle.AddSpell(needsDisplayCalculations, AM2, 0.032190759253966);
            cycle.AddSpell(needsDisplayCalculations, AM1, 0.00615545741939155);
            cycle.AddSpell(needsDisplayCalculations, ABar4, 0.155026330161182);
            cycle.AddSpell(needsDisplayCalculations, ABar3, 0.0384716088711972);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB24ABar34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB24ABar34AM";

            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            cycle.AddSpell(needsDisplayCalculations, AB2, 0.109900082717677);
            cycle.AddSpell(needsDisplayCalculations, AB1, 0.202557420603404);
            cycle.AddSpell(needsDisplayCalculations, AB0, 0.213296836427447);
            cycle.AddSpell(needsDisplayCalculations, AM4, 0.0677585908426607);
            cycle.AddSpell(needsDisplayCalculations, AM3, 0.146175461110249);
            cycle.AddSpell(needsDisplayCalculations, AM2, 0.0362753783925723);
            cycle.AddSpell(needsDisplayCalculations, AM1, 0.0107394218607248);
            cycle.AddSpell(needsDisplayCalculations, ABar4, 0.146175465466617);
            cycle.AddSpell(needsDisplayCalculations, ABar2, 0.0671213866295299);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB234ABar34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB234ABar34AM";

            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            Spell ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            cycle.AddSpell(needsDisplayCalculations, AB1, 0.244397664841874);
            cycle.AddSpell(needsDisplayCalculations, AB0, 0.244397657558256);
            cycle.AddSpell(needsDisplayCalculations, AM3, 0.099317741017749);
            cycle.AddSpell(needsDisplayCalculations, AM2, 0.167489308158335);
            cycle.AddSpell(needsDisplayCalculations, ABar4, 0.0993177439776483);
            cycle.AddSpell(needsDisplayCalculations, ABar3, 0.0681715691722566);
            cycle.AddSpell(needsDisplayCalculations, ABar2, 0.0769083589755872);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AB2ABar0AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2ABar0AM";

            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell AM0 = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            cycle.AddSpell(needsDisplayCalculations, AB1, 0.0842591434855811);
            cycle.AddSpell(needsDisplayCalculations, AB0, 0.267756840894371);
            cycle.AddSpell(needsDisplayCalculations, AM1, 0.18349769995104);
            cycle.AddSpell(needsDisplayCalculations, AM0, 0.0983647521042338);
            cycle.AddSpell(needsDisplayCalculations, ABar2, 0.267756851333377);
            cycle.AddSpell(needsDisplayCalculations, ABar1, 0.0983647550357318);
            cycle.Calculate();

            cycle.AreaEffect = false;

            return cycle;
        }
    }

    public static class AE4AB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AE4AB";

            Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            Spell AE4 = castingState.GetSpell(SpellId.ArcaneExplosion4);

            // AEx4-AB

            // 6 seconds on AB debuff - time to refresh AB
            int aeCount = (int)((6.0f - AB4.CastTime) / AE4.CastTime);

            cycle.AddSpell(needsDisplayCalculations, AE4, aeCount);
            cycle.AddSpell(needsDisplayCalculations, AB4, 1);
            cycle.Calculate();

            cycle.AreaEffect = true;

            return cycle;
        }
    }

    public static class AERampAB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AERampAB";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AE1 = castingState.GetSpell(SpellId.ArcaneExplosion1);
            Spell AE2 = castingState.GetSpell(SpellId.ArcaneExplosion2);
            Spell AE3 = castingState.GetSpell(SpellId.ArcaneExplosion3);
            Spell AE4 = castingState.GetSpell(SpellId.ArcaneExplosion4);

            // ABx2-AEx4-AB-AEx4-AB-AEx6

            // 6 seconds on AB debuff - time to refresh AB
            int ae1Count = (int)((6.0f - AB1.CastTime) / AE1.CastTime);
            int ae2Count = (int)((6.0f - AB2.CastTime) / AE2.CastTime);
            int ae3Count = (int)((6.0f - AB3.CastTime) / AE3.CastTime);
            int ae4Count = (int)Math.Ceiling(6.0f / AE4.CastTime);

            cycle.AddSpell(needsDisplayCalculations, AB0, 1);
            cycle.AddSpell(needsDisplayCalculations, AE1, ae1Count);
            cycle.AddSpell(needsDisplayCalculations, AB1, 1);
            cycle.AddSpell(needsDisplayCalculations, AE2, ae2Count);
            cycle.AddSpell(needsDisplayCalculations, AB2, 1);
            cycle.AddSpell(needsDisplayCalculations, AE3, ae3Count);
            cycle.AddSpell(needsDisplayCalculations, AB3, 1);
            cycle.AddSpell(needsDisplayCalculations, AE4, ae4Count);
            cycle.Calculate();

            cycle.AreaEffect = true;

            return cycle;
        }
    }

    public static class ArcaneManaNeutral
    {
        private static readonly CycleId[] globalCycleIds = new CycleId[] { CycleId.AB4AM, CycleId.AB4ABar4AM, CycleId.AB4ABar34AM, CycleId.AB34ABar34AM, CycleId.AB24ABar34AM, CycleId.AB234ABar34AM, CycleId.AB2ABar0AM };

        [ThreadStatic]
        private static List<Cycle> Cycles; // reuse to minimize memory thrashing

        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            CycleId[] cycleIds = castingState.Solver.UseIncrementalOptimizations ? castingState.CalculationOptions.IncrementalSetManaNeutralMix : globalCycleIds;
            List<Cycle> cycles = Cycles;
            if (cycles == null)
            {
                cycles = Cycles = new List<Cycle>(16);
            }
            else
            {
                cycles.Clear();
            }
            foreach (var cid in cycleIds)
            {
                Cycle c = castingState.GetCycle(cid);
                cycles.Add(c);
                /*if (c.ManaPerSecond < 0)
                {
                    break; // we have all we need
                }*/
            }

            cycles.Sort((c1, c2) => c1.ManaPerSecond.CompareTo(c2.ManaPerSecond));

            if (cycles[0].ManaPerSecond > 0)
            {
                //return cycles[0];
                Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
                cycle.Name = "ArcaneManaNeutral";
                if (needsDisplayCalculations)
                {
                    cycle.Note = string.Format("Mix {0:F}% {1}", 100, cycles[0].Name);
                }
                cycle.Mix1 = cycles[0].CycleId;
                cycle.Mix2 = CycleId.None;
                cycle.AddCycle(needsDisplayCalculations, cycles[0], 1);
                cycle.DpmConversion = 0;
                cycle.Calculate();
                return cycle;
            }

            int i = 0;
            while (i < cycles.Count)
            {
                double maxDpm = 0;
                int maxj = -1;
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    double dpm = (cycles[j].DamagePerSecond - cycles[i].DamagePerSecond) / (cycles[j].ManaPerSecond - cycles[i].ManaPerSecond);
                    if (dpm > maxDpm)
                    {
                        maxDpm = dpm;
                        maxj = j;
                    }
                }
                if (maxj != -1)
                {
                    if (cycles[maxj].ManaPerSecond >= 0)
                    {
                        // mps1 + k * (mps2 - mps1)
                        double k = -cycles[i].ManaPerSecond / (cycles[maxj].ManaPerSecond - cycles[i].ManaPerSecond);
                        Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
                        cycle.Name = "ArcaneManaNeutral";
                        if (needsDisplayCalculations)
                        {
                            cycle.Note = string.Format("Mix {0:F}% {1} and {2:F}% {3}", 100 * (1 - k), cycles[i].Name, 100 * k, cycles[maxj].Name);
                        }
                        cycle.Mix1 = cycles[i].CycleId;
                        cycle.Mix2 = cycles[maxj].CycleId;
                        cycle.AddCycle(needsDisplayCalculations, cycles[i], (1 - k) / cycles[i].CastTime);
                        cycle.AddCycle(needsDisplayCalculations, cycles[maxj], k / cycles[maxj].CastTime);
                        cycle.DpmConversion = maxDpm;
                        cycle.Calculate();
                        return cycle;
                    }
                    i = maxj;
                }
                else
                {
                    // we've run out of cycles
                    //return cycles[i];
                    Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
                    cycle.Name = "ArcaneManaNeutral";
                    if (needsDisplayCalculations)
                    {
                        cycle.Note = string.Format("Mix {0:F}% {1}", 100, cycles[i].Name);
                    }
                    cycle.Mix1 = cycles[i].CycleId;
                    cycle.Mix2 = CycleId.None;
                    cycle.AddCycle(needsDisplayCalculations, cycles[i], 1);
                    cycle.DpmConversion = 0;
                    cycle.Calculate();
                    return cycle;
                }
            }
            return null;
        }
    }

    public class ArcaneCycleGeneratorWOD : CycleGenerator
    {
        private class State : CycleState
        {
            public int ArcaneMissilesRegistered { get; set; }
            public int ArcaneMissilesProcced { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneChargeStack { get; set; }
        }

        public Spell[] AB;
        public Spell[] ABar, AM;
        public Spell Bomb;

        private float AMProc;
        private int maxStack;
		private int maxAMStack;
        private float channelLatency;

        private bool ABarOnCooldownOnly;
        private bool ABarCooldownCollapsed;

        public override Cycle WrapCycle(Cycle baseCycle, CastingState castingState)
        {
            //return NetherTempestCycle.GetCycle(true, castingState, baseCycle);
			return baseCycle;
        }

        public ArcaneCycleGeneratorWOD(CastingState castingState, bool ABarOnCooldownOnly, bool ABarCooldownCollapsed, bool aoe)
        {
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;

            var calc = castingState.Solver;
            maxStack = 4;
			maxAMStack = 3;

            AB = new Spell[maxStack + 1];
            ABar = new Spell[maxStack + 1];
            AM = new Spell[maxStack + 1];

            for (int stack = 0; stack <= maxStack; stack++)
            {
                AB[stack] = calc.ArcaneBlastTemplate.GetSpell(castingState, stack);
                AB[stack].Label = "ArcaneBlast" + stack;
                ABar[stack] = calc.ArcaneBarrageTemplate.GetSpell(castingState, stack, aoe);
                ABar[stack].Label = "ArcaneBarrage" + stack;
                AM[stack] = calc.ArcaneMissilesTemplate.GetSpell(castingState, false, stack);
                AM[stack].Label = "ArcaneMissiles" + stack;
            }

            channelLatency = castingState.CalculationOptions.LatencyChannel;
            AMProc = 0.4f;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(0.0f, 0, 0, 0);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            AB = this.AB[s.ArcaneChargeStack];
            ABar = this.ABar[s.ArcaneChargeStack];
            if (s.ArcaneMissilesRegistered > 0)
            {
                AM = this.AM[s.ArcaneChargeStack];
            }
            if (AMProc > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneChargeStack + 1),
                        s.ArcaneMissilesProcced,
                        Math.Min(maxAMStack, s.ArcaneMissilesProcced + 1)),
                    TransitionProbability = AMProc
                });
            }
            list.Add(new CycleControlledStateTransition()
            {
                Spell = AB,
                TargetState = GetState(
                    Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                    Math.Min(maxStack, s.ArcaneChargeStack + 1),
                    s.ArcaneMissilesProcced,
                    s.ArcaneMissilesProcced),
                TransitionProbability = (1 - AMProc)
            });
            if (s.ArcaneMissilesRegistered > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        Math.Min(maxStack, s.ArcaneChargeStack + 1),
                        s.ArcaneMissilesProcced - 1,
                        s.ArcaneMissilesProcced - 1),
                    TransitionProbability = 1.0f
                });
            }
            if ((!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0.0) && s.ArcaneChargeStack == maxStack)
            {
                if (AMProc > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            ABar.Cooldown - ABar.CastTime,
                            0,
                            Math.Min(maxAMStack, s.ArcaneMissilesProcced + 1),
                            Math.Min(maxAMStack, s.ArcaneMissilesProcced + 1)),
                        TransitionProbability = AMProc
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(
                        ABar.Cooldown - ABar.CastTime,
                        0,
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced),
                    TransitionProbability = (1 - AMProc)
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(float arcaneBarrageCooldown, int arcaneBlastStack, int arcaneMissilesRegistered, int arcaneMissilesProcced)
        {
            string name;
            name = string.Format("AB{0},ABar{1},AM{2}{3}", arcaneBlastStack, arcaneBarrageCooldown, arcaneMissilesProcced, arcaneMissilesRegistered);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneChargeStack = arcaneBlastStack, ArcaneMissilesProcced = arcaneMissilesProcced, ArcaneMissilesRegistered = arcaneMissilesRegistered };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneChargeStack != b.ArcaneChargeStack ||
                (!ABarCooldownCollapsed && a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown) ||
                ((a.ArcaneBarrageCooldown > 0) != (b.ArcaneBarrageCooldown > 0)) ||
                a.ArcaneMissilesRegistered != b.ArcaneMissilesRegistered);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend:
State Descriptions: ABx,ABary,AM+-
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
+ = Arcane Missiles proc visible
- = Arcane Missiles proc not visible";
            }
        }
    }
}
