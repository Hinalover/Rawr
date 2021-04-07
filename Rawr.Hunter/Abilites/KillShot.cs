using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class KillShot : Ability
    {
        public static int Cooldown = 10;

        /// <summary>
        /// TODO Zhok: Sniper Training
        /// <b>Kill Shot</b>, 45 Focus, 45yd, Instant, 10 sec Cd
        /// <para>You attempt to finish the wounded target off, firing a long range attack
        /// dealing 150% weapon damage plus RAP*0.30+543. Kill Shot can only be used on
        /// enemies that have 20% or less health.</para>
        /// <para>Kill Shot can only be used on enemies that have 20% or less health.</para>
        /// </summary>
        /// <TalentsAffecting>Sniper Training - Increases the critical strike chance of your Kill Shot ability by 5/10/15%, and after remaining stationary for 6 sec, your Steady Shot and Cobra Shot deal 2/4/6% more damage for 15 sec.</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Kill Shot - If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.</GlyphsAffecting>
        public KillShot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            Name = "Kill Shot";
            shortName = "KS";

            ReqRangedWeap = true;
            ReqSkillsRange = true;
            DamageType = ItemDamageType.Physical;
            // TODO: Implement new CD reset once every 6 seconds
            Cd = Cooldown;

            //Every other CD resets immediately, so add 2 GCDs and divide by 2, net 6 second cooldown average
            Cd = (Cd + 2 * 1)/2;


            FocusCost = 0;

            // 420% weapon damage
            DamageBase = (cf.NormalizedRwWeaponDmg * 4.2f);
            eShot = Shots.KillShot;
            Initialize();
        }
    }
}
