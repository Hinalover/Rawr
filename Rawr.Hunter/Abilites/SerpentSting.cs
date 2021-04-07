using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class SerpentSting : DoT
    {
        /// <summary>
        /// <b>Serpent Sting</b>, 25 Focus,5-40yd, Instant, No Cd
        /// <para>Causes (RAP * 0.4 + (460 * 15 sec / 3)) Nature damage over 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Chimera Shot - An instant shot that causes ranged weapon damage plus RAP*0.732+1620, refreshing the duration of  your Serpent Sting and healing you for 5% of your total health.
        /// Noxious Stings - Increases your damage done on targets afflicted by your Serpent Sting by 5/10%.
        /// Serpent Spread - Targets hit by your Multi-Shot are also afflicted by your Serpent Sting equal to 6/9 sec of its total duration.
        /// Toxicology - Increases the periodic critical damage of your Serpent Sting and Black Arrow by 50/100%.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Serpent Sting - Increases the periodic critical strike chance of your Serpent Sting by 6%.</GlyphsAffecting>
        public SerpentSting(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            Name = "Serpent Sting";
            shortName = "Sting";

            ReqRangedWeap = true;
            ReqSkillsRange = true;
            TimeBtwnTicks = 3f; // In Seconds
            Duration = 15f;
            FocusCost = 25f;
            eShot = Shots.SerpentSting;
            DamageType = ItemDamageType.Nature;


            DamageBase = (((StatS.RangedAttackPower * .08f) + 1620.19f) * 5f);

            if (((Specialization)Talents.Specialization == Specialization.Survival))
            {
                // Improved Serpent Sting
                DamageBase = (DamageBase * 1.5f) * 1.3f;
            }
            //TODO: Viper Venom -> Focus Recovery


            //Commented out for MoP.. doubling of value is now pre-Crit (Improved Serpent Sting passive), and crit is on top of that value
            //BonusCritChance = 1f + (Talents.GlyphOfSerpentSting ? 0.06f : 0f) + (Talents.ImprovedSerpentSting * 0.05f);

            MinRange = 0f;
            MaxRange = 40f;
            CanCrit = true;
            StatS.BonusDamageMultiplier += (.05f * Talents.NoxiousStings);
            StatS.BonusCritDamageMultiplier += (.5f * Talents.Toxicology);
            
            // Noxious Stings
            //
            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }
                float fTickSize = (DamageBase * 3 / 15);
                fTickSize *= DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusNatureDamageMultiplier) * (1 + (.15f * Talents.ImprovedSerpentSting));
                fTickSize = fTickSize * (1f + StatS.PhysicalCrit) * BonusCritChance;
                fTickSize *= (1f + StatS.BonusCritDamageMultiplier);
                return fTickSize;
            }
        }
        public override float GetDPS(float acts)
        {
            return getTotalDamage / FightDuration;
        }
        public float getTotalDamage
        {
            get
            {
                if (!Validated) { return 0f; }

                return TickSize * (Duration / TickLength);
            }
        }
    }
}
