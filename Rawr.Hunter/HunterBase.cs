using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{
    enum subpointINDEX : int
    {
        HUNTERDPS,
        HUNTERSURVIVAL,
        PETDPS,
        PETSURVIVAL,
    }

    public class HunterBase
    {
        public HunterBase (Character charac, StatsHunter stats, HunterTalents talents, Specialization spec, int targetLvL)
        {
            character = charac;
            Stats = stats;
            Talents = talents;
            Tree = spec;
            TargetLevel = targetLvL;
        }

        private Character character { get; set; }
        public StatsHunter Stats { get; set; }
        private HunterTalents Talents { get; set; }
        private Specialization Tree { get; set; }
        private int TargetLevel { get; set; }

        /// <summary>Shows the amount of Stamina the player has.</summary>
        public float Stamina { get { return Stats.Stamina; } }
        /// <summary>Shows the amount of Health the player has.</summary>
        public float Health { get { return Stats.Health; } }
        /// <summary>Shows the amount of Focus the player has. Base is 100 Focus. Beast Mastery players can have up to 110 Focus</summary>
        public float Focus { get { return 100f + (Talents.KindredSpirits * 5f); } }
        /// <summary>Shows the amount of Armor the player has.</summary>
        public float Armor { get { return Stats.Armor; } }
        /// <summary>Shows the amount of Agility the player has.</summary>
        public float Agility { get { return Stats.Agility; } }
        /// <summary>Shows the Ranged Attack Power the Hunter has.</summary>
        public float RangedAttackPower { get { return Stats.RangedAttackPower; } }

        /// <summary>Show base Physical Hit chance</summary>
        public float PhysicalHit { get { return Stats.PhysicalHit; } }
        /// <summary>Show base Hit Rating</summary>
        public float HitRating { get { return Stats.HitRating; } }
        /// <summary>Show Hit Rating Percent from Hit Rating</summary>
        public float HitRatingPercent { get { return StatConversion.GetHitFromRating(this.HitRating); } }
        /// <summary>Show amount of hit from bonuses</summary>
        public float HitFromBonus { get { return this.PhysicalHit - this.HitRatingPercent; } }
        /// <summary>Show amount of hit rating from bonuses</summary>
        public float HitRatingFromBonus { get { return StatConversion.GetRatingFromHit(this.HitFromBonus); } }
        /// <summary>Returns the amount of Hit Rating needed to cap</summary>
        public float HitRatingCap { get { return (float)Math.Ceiling(StatConversion.GetRatingFromHit(StatConversion.WHITE_MISS_CHANCE_CAP[this.TargetLevel - character.Level])); } }
        /// <summary>Returns difference between the hit cap and what the player currently has</summary>
        public float HitCapDifference { get { return (this.HitRatingCap - this.HitRating); } }
        /// <summary>Shows the label for how much hit rating the user needs</summary>
        public string HitNeededLabel { get { return ((this.HitCapDifference < 0) ? "You can free {2:0} Hit Rating" : ((this.HitCapDifference > 0) ? "You need {2:0} more Hit Rating" : "You are exactly at the Hit Rating cap")); } }
        /// <summary>Shows the Hit Rating the user needs to cap</summary>
        public float HitRatingNeeded { get { return (this.HitCapDifference < 0) ? this.HitCapDifference * -1f : this.HitCapDifference; } }


        /// <summary>Show base Expertise Rating</summary>
        public float ExpertiseRating { get { return Stats.ExpertiseRating; } }
        /// <summary>Show Expertise Rating Percent from Hit Rating</summary>
        public float ExpertiseRatingPercent { get { return StatConversion.GetExpertiseFromRating(this.ExpertiseRating); } }
        /// <summary>Show amount of Expertise rating from bonuses</summary>
        public float ExpertiseRatingFromBonus { get { return StatConversion.GetRatingFromHit(this.ExpertiseRatingFromBonus); } }
        /// <summary>Returns the amount of Expertise Rating needed to cap</summary>
        public float ExpertiseRatingCap { get { return (float)Math.Ceiling(StatConversion.GetRatingFromExpertise(StatConversion.WHITE_DODGE_CHANCE_CAP[this.TargetLevel - character.Level])); } }
        /// <summary>Returns difference between the Expertise cap and what the player currently has</summary>
        public float ExpertiseCapDifference { get { return (this.ExpertiseRatingCap - this.ExpertiseRating); } }
        /// <summary>Shows the label for how much Expertise rating the user needs</summary>
        public string ExpertiseNeededLabel { get { return ((this.ExpertiseCapDifference < 0) ? "You can free {2:0} Expertise Rating" : ((this.ExpertiseCapDifference > 0) ? "You need {2:0} more Expertise Rating" : "You are exactly at the Expertise Rating cap")); } }
        /// <summary>Shows the Expertise Rating the user needs to cap</summary>
        public float ExpertiseRatingNeeded { get { return (this.ExpertiseCapDifference < 0) ? this.ExpertiseCapDifference * -1f : this.ExpertiseCapDifference; } }
        

        /// <summary>Show chance to miss on hit</summary>
        public float ChancetoMiss { get { return Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[this.TargetLevel - character.Level] - this.HitRatingPercent); } }
        /// <summary>Show chance to be dodged on hit</summary>
        public float ChancetoBeDodged { get { return Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[this.TargetLevel - character.Level] - this.ExpertiseRatingPercent); } }
        /// <summary>Show chance to hit target</summary>
        public float ChancetoHit { get { return 1f - this.ChancetoMiss - this.ChancetoBeDodged; } }

        /// <summary>Show base Critical Strike Rating</summary>
        public float CritRating { get { return Stats.CritRating; } }
        /// <summary>Show amount of Crit from Critical Strike Rating</summary>
        public float CritfromRating { get { return StatConversion.GetPhysicalCritFromRating(this.CritRating); } }
        /// <summary>Show amount of Crit from Agility</summary>
//        public float CritfromAgility { get { return StatConversion.GetCritFromAgility(Stats.BaseAgilityforCrit, CharacterClass.Hunter); } }
        public float CritfromAgility { get { return StatConversion.GetCritFromAgility(Stats.Agility, CharacterClass.Hunter); } }
        /// <summary>Shows the amount of Physical Crit</summary>
        public float PhysicalCrit { get { return Stats.PhysicalCrit; } }
        /// <summary>Show Crit modification from attacking a higher target mob.</summary>
        public float CritModifiedfromTarget { get { return StatConversion.NPC_LEVEL_CRIT_MOD[this.TargetLevel - character.Level]; } }
        /// <summary>Show chance to crit target</summary>
        public float ChancetoCrit { get { return ((this.CritfromRating + this.CritfromAgility + Stats.PhysicalCrit + this.CritModifiedfromTarget) * (1f - this.ChancetoMiss)); } }

        /// <summary>Show Haste Rating</summary>
        public float HasteRating { get { return Stats.HasteRating; } }
        /// <summary>Show the Physical Haste Rating</summary>
        public float PhysicalHaste { get { return Stats.PhysicalHaste; } }
        /// <summary>Show the base percentage haste</summary>
        public float BaseHaste { get { return StatConversion.GetPhysicalHasteFromRating(Math.Max(0f,this.HasteRating), CharacterClass.Hunter); } }
        /// <summary>Show Haste Percentage</summary>
        public float Haste { get { return Stats.RangedHaste; } }

        /// <summary>Focus Regen Per Second</summar>
        // TODO: Apply Rapid Fire + Glyph, Hero, and Focused Fire
        /* http://elitistjerks.com/f74/t65904-hunter_dps_analyzer/p25/#post1887407
             * 1) Base focus regen is 4.00.
             * 2) Pathing adds an additional 1% base focus regen per point (4.12 with 3/3 and no gear).
             * 3) WF/IT/HP and ISS don't modify base regen directly.
             * 4) Each 1% gear haste adds 2% base focus regen.
             * 5) Rapid Fire adds 40% base regen (4.00->5.60).
             * 6) Hero adds 30% base regen (4.00->5.20).
         * this line (7) doesn't match the math of the other Base regen modifiers on the other lines.
             * 7) Glyph of Rapid Fire adds 10% base regen (4.00->6.00).
             * 8) Focused Fire adds 15% base regen (4.00->4.60).
         */
        public float FocusRegen { get { return 4f * (1f + (Talents.Pathing * 0.01f)) * (1 + (this.BaseHaste * 2f)); } }

        /// <summary>Returns the Base amount of Mastery value in % per Tree, before Mastery Rating is added</summary>
        [Percentage]
        public float BaseMastery
        {
            get
            {
                switch (this.Tree)
                {
                    case Specialization.BeastMastery:
                        return 0.1336f;
                    case Specialization.Marksmanship:
                        return 0.168f;
                    case Specialization.Survival:
                        return 0.08f;
                    default:
                        return 0f;
                }
            }
        }
        /// <summary>Returns the Incremental Mastery Multiplier, multiplied by the Mastery Rating Conversion.</summary>
        [Percentage]
        public float IncrementalMastery
        {
            get
            {
                switch (this.Tree)
                {
                    case Specialization.BeastMastery:
                        return 0.0167f;
                    case Specialization.Marksmanship:
                        return 0.021f;
                    case Specialization.Survival:
                        return 0.01f;
                    default:
                        return 0f;
                }
            }
        }
        /// <summary>Returns Mastery Rate.</summary>
        public float MasteryRating { get { return Stats.MasteryRating; } }
        /// <summary>Returns Mastery Rate conversion.</summary>
        public float MasteryRateConversion { get { return StatConversion.GetMasteryFromRating(this.MasteryRating); } }
        /// <summary>Returns total amount of Incremental Mastery with the Conversion</summary>
        [Percentage]
        public float IncrementalmasterywithConversion { get { return this.IncrementalMastery * (Stats.Mastery + MasteryRateConversion); } }
        /// <summary>Returns total Mastery Rating Percentage</summary>
        [Percentage]
        public float MasteryRatePercent { get { return this.BaseMastery + this.IncrementalmasterywithConversion; } }
        /// <summary>Show Mastery Label</summary>
        public string MasteryLabel
        {
            get
            {
                switch (this.Tree)
                {
                    case Specialization.BeastMastery:
                        return "\r\nMaster of Beasts: {5:00.00%}";
                    case Specialization.Marksmanship:
                        return "\r\nWild Quiver: {5:00.00%}";
                    case Specialization.Survival:
                        return "\r\nEssence of the Viper: {5:00.00%}";
                    default:
                        return "\r\nNot Specced: {5:00.00%}.";
                }
            }
        }
    }
}
