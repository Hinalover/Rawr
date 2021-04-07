using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ModelFramework;

namespace Rawr.Brewmaster {
    public class CharacterCalculationsBrewmaster : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float MitigationPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SurvivabilityPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float RecoveryPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public float ThreatPoints
        {
            get { return _subPoints[3]; }
            set { _subPoints[3] = value; }
        }

        public StatsBrewmaster BasicStats { get; set; }
        public int TargetLevel { get; set; }
        public int CharacterLevel { get; set; }

        public float PhysicalHit { get; set; }
        public float PhysicalHitCap { get; set; }
        public float PhysicalDodge { get; set; }
        public float DodgeCap { get; set; }
        public float PhysicalParry { get; set; }
        public float ParryCap { get; set; }
        public float PhysicalCrit { get; set; }
        public float PhysicalHaste { get; set; }
        public float AttackPower { get; set; }
        public float HealingPower { get; set; }
        public float SpellPower { get; set; }
        public float MovementSpeed { get; set; }
        public float Mastery { get; set; }

        /// <summary>
        /// Elusive Brawler: Increases your Stagger amount by an additional 0.50%.
        /// </summary>
        public float MasteryPerRating = 0.5000000000f;

        public float AvoidedAttacks { get { return PhysicalHit + PhysicalDodge + PhysicalParry; } }
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            int levelDifference = TargetLevel - CharacterLevel;
            float baseMiss = StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - BasicStats.PhysicalHit;
            float baseDodge = StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - StatConversion.GetDodgeParryReducFromExpertise(BasicStats.Expertise);
            float baseParry = StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - StatConversion.GetDodgeParryReducFromExpertise(BasicStats.Expertise);
            float capMiss = (float)Math.Ceiling(StatConversion.GetRatingFromHit(baseMiss, CharacterLevel));
            float capDodge = (float)Math.Ceiling(baseDodge * (BaseCombatRating.ExpertiseRatingMultiplier(CharacterLevel) * 100));
            float capParry = (float)Math.Ceiling(baseParry * (BaseCombatRating.ExpertiseRatingMultiplier(CharacterLevel) * 100)) + capDodge;

            string tipMiss = string.Empty;
            if (BasicStats.HitRating > capMiss)
                tipMiss = string.Format("Over the cap by {0} Hit Rating", BasicStats.HitRating - capMiss);
            else if (BasicStats.HitRating < capMiss)
                tipMiss = string.Format("Under the cap by {0} Hit Rating", capMiss - BasicStats.HitRating);
            else
                tipMiss = "Exactly at the cap";

            string tipDodgeParry = string.Empty;
            if (BasicStats.ExpertiseRating > capDodge)
                tipDodgeParry = string.Format("Over the dodge cap by {0} Expertise Rating\r\n", BasicStats.ExpertiseRating - capDodge);
            else if (BasicStats.ExpertiseRating < capDodge)
                tipDodgeParry = string.Format("Under the dodge cap by {0} Expertise Rating\r\n", capDodge - BasicStats.ExpertiseRating);
            else
                tipDodgeParry = "Exactly at the dodge cap";

            if (BasicStats.ExpertiseRating > capParry)
                tipDodgeParry += string.Format("Over the parry cap by {0} Expertise Rating", BasicStats.ExpertiseRating - capParry);
            else if (BasicStats.ExpertiseRating < capParry)
                tipDodgeParry += string.Format("Under the parry cap by {0} Expertise Rating", capParry - BasicStats.ExpertiseRating);
            else
                tipDodgeParry += "Exactly at the parry cap";

            int armorCap = (int)Math.Ceiling(6502.5f * TargetLevel - 474502.5f);
            float levelDifferenceAvoidance = 0.002f * levelDifference;
            float targetCritReduction = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];

            // Changed to not just give a resist rating, but a breakdown of the resulting resist values in the tooltip
            string tipResist = string.Empty;

            retVal.Add("Overall Points", OverallPoints.ToString("n2"));
            retVal.Add("Mitigation Points", MitigationPoints.ToString("n2"));
            retVal.Add("Survival Points", SurvivabilityPoints.ToString("n2"));
            retVal.Add("Recovery Points", RecoveryPoints.ToString("n2"));
            retVal.Add("Threat Points", ThreatPoints.ToString("n2"));

            #region Basic Stats
            retVal.Add("Health", BasicStats.Health.ToString("n0"));
            retVal.Add("Stamina", BasicStats.Stamina.ToString("n0"));
            retVal.Add("Armor", BasicStats.Armor.ToString("n0"));
            retVal.Add("Agility", BasicStats.Agility.ToString("n0"));
            retVal.Add("Strength", BasicStats.Strength.ToString("n0"));
            retVal.Add("Attack Power", AttackPower.ToString("n0"));
            //retVal.Add("Average Vengeance AP", Mitigation.Vengence.ToString("n0"));

            float dodgeFromRating = StatConversion.GetDodgeFromRating(BasicStats.DodgeRating, CharacterLevel);
            retVal.Add("Dodge Rating", string.Format("{0}*Pre-DR Dodge: {1}",
                                            BasicStats.DodgeRating.ToString("n0"),
                                            dodgeFromRating.ToString("p")));

            float critFromRating = StatConversion.GetPhysicalCritFromRating(BasicStats.CritRating, CharacterLevel);
            float critFromAgi = StatConversion.GetPhysicalCritFromAgility(BasicStats.Agility, CharacterClass.Druid, CharacterLevel);
            retVal.Add("Crit Rating", string.Format("{0}*Total Crit %: {1}\nCrit % from Rating: {2}\nCrit % from Agility: {3}\nCrit % from Other: {4}",
                                            BasicStats.CritRating.ToString("n0"),
                                            PhysicalCrit.ToString("p"),
                                            critFromRating.ToString("p"),
                                            critFromAgi.ToString("p"),
                                            (PhysicalCrit - critFromRating - critFromAgi).ToString("p")));

            float hasteFromRating = StatConversion.GetPhysicalHasteFromRating(BasicStats.HasteRating, CharacterLevel);
            retVal.Add("Haste Rating", string.Format("{0}*Total Haste %: {1}\nHaste % from Rating: {2}\nHaste % from Other: {3}",
                                            BasicStats.HasteRating.ToString("n0"),
                                            PhysicalHaste.ToString("p"),
                                            hasteFromRating.ToString("p"),
                                            (PhysicalHaste - hasteFromRating).ToString("p")));

            float masteryFromRating = StatConversion.GetMasteryFromRating(BasicStats.MasteryRating, CharacterLevel);
            retVal.Add("Mastery Rating", string.Format("{0}*Total Mastery %: {1}\nMastery % from Rating: {2}\nMastery % from Base: {3}\nMastery % from Buff: {4}",
                                            BasicStats.MasteryRating.ToString("n0"),
                                            Mastery.ToString("p"),
                                            (masteryFromRating * MasteryPerRating).ToString("p"),
                                            (8 * MasteryPerRating).ToString("p"),
                                            ((BasicStats.Mastery - 8) * MasteryPerRating).ToString("p")));

            retVal.Add("Hit %", string.Format("{0}*Hit Rating: {1}\n{2}",
                                            PhysicalHit.ToString("p2"),
                                            BasicStats.HitRating.ToString("n0"),
                                            tipMiss));

            retVal.Add("Expertise %", string.Format("{0}*Expertise Rating: {1}\n{2}",
                                            (PhysicalDodge + PhysicalParry).ToString("p2"),
                                            BasicStats.ExpertiseRating.ToString("n0"),
                                            tipDodgeParry));
            #endregion
            
            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Avoided Attacks": return (1 - PhysicalHit - (PhysicalDodge + PhysicalParry));
                case "Hit Rating": return BasicStats.HitRating;
                case "Hit %": return PhysicalHit;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Expertise %": return (PhysicalDodge + PhysicalParry);
                case "Critical Strike Rating": return BasicStats.CritRating;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Mastery Rating": return BasicStats.MasteryRating;
            }
            return 0;
        }
    }
}
