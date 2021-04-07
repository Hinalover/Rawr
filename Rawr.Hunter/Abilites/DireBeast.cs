using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class DireBeast : Ability
    {
        public static float Cooldown = 30f;

        public DireBeast(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Dire Beast";
            shortName = "DB";

            ReqTalent = true;
            
            Talent2ChksValue = c.HunterTalents.DireBeast;

            Cd = Cooldown;
            
            //ReqRangedWeap = true;
            //ReqSkillsRange = true;

            //TODO: Last updated based on data from Lilbitters (ElitistJerks - http://elitistjerks.com/f74/t126894-mists_pandaria_all_specs/p15/) regarding haste and breakpoints
            CastTime = 0f;
            var haste = s.HasteRating;

            

            Duration = 15f;

            int tickCount;

            if (haste < 2834)
                tickCount = 8;
            else if (haste < 3864)
                tickCount = 9;
            else if (haste < 9016)
                tickCount = 10;
            else
                tickCount = 11;
            //else if (haste < 3864)
            //    tickCount = 9;
            //else if (haste < 3864)
            //    tickCount = 9;



            //focus recovery every time it hits (5 focus restored each of X attacks)
            FocusCost = -5f * tickCount;

            DamageType = ItemDamageType.Nature;

            UsesGCD = true;


            DamageBase = (StatS.RangedAttackPower * .2857f + 1246.5f) * tickCount;

            //float DamageBaseTest = SpellEffectProcessor.calculateWeaponDamage(c.MainHand, cf.StatS.RangedAttackPower) * .7f;
            //float DamageBaseTestNorm = SpellEffectProcessor.calculateWeaponDamage(c.MainHand, cf.StatS.RangedAttackPower, true) * .7f;
            
            //SpecialEffect s = new SpecialEffect(Trigger.

            eShot = Shots.DireBeast;

            Initialize();
        }
    }
}
