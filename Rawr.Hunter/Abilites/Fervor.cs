using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class Fervor : Ability
    {
        public static float Cooldown = 30f;

        public Fervor(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Fervor";
            shortName = "Ferv";
            Cd = Cooldown; // In Seconds
            Duration = 0f;
            FocusCost = -50f;

            //TODO: Add pet focus component

            ReqTalent = true;
            Talent2ChksValue = c.HunterTalents.Fervor;

            UseHitTable = false;
            eShot = Shots.Fervor;
            Initialize();
        }

    }
}
