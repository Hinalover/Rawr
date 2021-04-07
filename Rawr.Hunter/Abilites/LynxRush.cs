using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class LynxRush : DoT
    {
        public static float Cooldown = 90f;
        /// <summary>
        /// TODO Zhok: Thrill of the Hunt, Toxicology, Trap Mastery
        /// <b>Lynx Rush</b>, 100yd, Instant, 1.5 min Cd
        /// <para>Your pet rapidly charges from target to target, attacking 9 times over 4 sec,
        /// dealing 200% of its normal attack damage to each target</para>
        /// </summary>
        /// <TalentsAffecting>Lynx Rush (Requires Talent)
        /// </TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public LynxRush(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;

            Name = "Lynx Rush";
            shortName = "LR";

            SpellId = 120697;
            ReqTalent = true;

            
            Talent2ChksValue = c.HunterTalents.LynxRush;
            
            ReqRangedWeap = false;
            ReqSkillsRange = false;
            DamageType = ItemDamageType.Physical;
            Cd = Cooldown; // in seconds
            Duration = 4f;
            TimeBtwnTicks = 4f/9f; // TODO Haste?
            FocusCost = 0f;
            

            // estimate of pet attack by Rivkah 6/23 on Elitist Jerks
            DamageBase = (StatS.RangedAttackPower*.36f + 329) * 9f;
            
            
            eShot = Shots.LynxRush;

            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }
                return DamageBase * DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusShadowDamageMultiplier) / NumTicks;
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
