using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class GlaiveToss : Ability
    {
        public static float Cooldown = 15f;
        public GlaiveToss(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            
            //
            Name = "Glaive Toss";
            shortName = "GT";

            SpellId = 117050;

            ReqTalent = true;
            Talent2ChksValue = c.HunterTalents.GlaiveToss;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            DamageType = ItemDamageType.Nature;
            Cd = Cooldown;
            FocusCost = 15f;

            //TODO-NICE: Only implemented for primary target, have not implemented other yet
            DamageBase = ((872f + StatS.RangedAttackPower * .2f) * 4f);
            
            eShot = Shots.GlaiveToss;
            Initialize();
        }
    }
}
