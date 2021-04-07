using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class Powershot : Ability
    {
        public static float Cooldown = 60f;

        public Powershot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            
            //
            Name = "Powershot";
            shortName = "PShot";

            SpellId = 109259;

            ReqTalent = true; // Reqiures MM spec.
            Talent2ChksValue = (c.HunterTalents.Powershot);
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            DamageType = ItemDamageType.Physical;
            MaxRange = 40f;
            CastTime = 3f;
            Cd = Cooldown;    
            FocusCost = 20f;

            //TODO-NICE: Only implemented for primary target, have not implemented other yet
            DamageBase = combatFactors.NormalizedRwWeaponDmg * 8f;
            
            eShot = Shots.Powershot;
            Initialize();
        }
    }
}
