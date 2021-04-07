using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class MurderOfCrows : DoT
    {
        public static float Cooldown = 120f;

        public MurderOfCrows(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "A Murder of Crows";
            shortName = "MoC";

            ReqTalent = true;
            
            Talent2ChksValue = c.HunterTalents.MurderOfCrows;

            Cd = Cooldown;

            if (co.BossHPPerc < 20)
                Cd = 60f;
            
            //ReqRangedWeap = true;
            //ReqSkillsRange = true;
            CastTime = 0f;
            FocusCost = 60f;
            DamageType = ItemDamageType.Physical;
            TimeBtwnTicks = 1f;

            UsesGCD = true;
            //TODO: Check, built on elitist jerks observations from Rivkah on 6/23
            Duration = 30f;
            
            DamageBase = (StatS.RangedAttackPower * .206f + 560.834f) * 30f;

            //float DamageBaseTest = SpellEffectProcessor.calculateWeaponDamage(c.MainHand, cf.StatS.RangedAttackPower) * .7f;
            //float DamageBaseTestNorm = SpellEffectProcessor.calculateWeaponDamage(c.MainHand, cf.StatS.RangedAttackPower, true) * .7f;
            
            //SpecialEffect s = new SpecialEffect(Trigger.

            eShot = Shots.AMurderOfCrows;

            Initialize();
        }

        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }
                return DamageBase * (1f + StatS.BonusDamageMultiplier) / NumTicks;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * (acts /*- addMisses - addDodges - addParrys*/);
            float result = GetDmgOverTickingTime(acts) / FightDuration;
            return result;
        }
    }
}
