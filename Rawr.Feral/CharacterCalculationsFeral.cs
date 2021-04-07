using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Feral
{
	public class CharacterCalculationsFeral : CharacterCalculationsBase
	{
        private float overallPoints = 0f;
        public override float OverallPoints { get { return overallPoints; } set { overallPoints = value; } }

        private float[] subPoints = new float[] { 0f, 0f };

        public override float[] SubPoints { get { return subPoints; } set { subPoints = value; } }

        public float PhysicalHit { get; set; }
        public float PhysicalHitCap { get; set; }
        public float Dodge { get; set; }
        public float DodgeCap { get; set; }
        public float Parry { get; set; }
        public float ParryCap { get; set; }
        public float PhysicalCrit { get; set; }
        public float PhysicalHaste { get; set; }
        public float AttackPower { get; set; }
        public float HealingPower { get; set; }
        public float MovementSpeed { get; set; }
        public float Mastery { get; set; }
        public readonly float MasteryPerRating = 0.0313f;
        public FeralRotation MaxRotation { get; set; }
        public StatsFeral BasicStats { get { return baseStats; } set { baseStats = value; } }
        private StatsFeral baseStats;
        public int CharacterLevel { get; set; }
        public int TargetLevel { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            //
            if (baseStats == null) baseStats = new StatsFeral();

            float capMiss = (float)Math.Ceiling(PhysicalHitCap * (BaseCombatRating.MeleeHitRatingMultiplier(CharacterLevel) * 100));
            float capDodge = (float)Math.Ceiling(DodgeCap * (BaseCombatRating.ExpertiseRatingMultiplier(CharacterLevel) * 100));
            
            string tipMiss = string.Empty;
            if (BasicStats.HitRating > capMiss)
                tipMiss = string.Format("Over the hit cap by {0} Hit Rating", BasicStats.HitRating - capMiss);
            else if (BasicStats.HitRating < capMiss)
                tipMiss = string.Format("Under the hit cap by {0} Hit Rating", capMiss - BasicStats.HitRating);
            else
                tipMiss = "Exactly at the hit cap";

            string tipDodgeParry = string.Empty;
            if (BasicStats.ExpertiseRating > capDodge)
                tipDodgeParry = string.Format("Over the dodge cap by {0} Expertise Rating", BasicStats.ExpertiseRating - capDodge);
            else if (BasicStats.ExpertiseRating < capDodge)
                tipDodgeParry = string.Format("Under the dodge cap by {0} Expertise Rating", capDodge - BasicStats.ExpertiseRating);
            else
                tipDodgeParry = "Exactly at the dodge cap";

            retVal.Add("Health", baseStats.Health.ToString("n0"));
            retVal.Add("Attack Power", AttackPower.ToString("n0"));
            retVal.Add("Agility", baseStats.Agility.ToString("n0"));
            retVal.Add("Strength", baseStats.Strength.ToString("n0"));


            float critFromRating = StatConversion.GetPhysicalCritFromRating(baseStats.CritRating, CharacterLevel);
            float critFromAgi = StatConversion.GetPhysicalCritFromAgility(baseStats.Agility, CharacterClass.Druid, CharacterLevel);
            retVal.Add("Crit Rating", string.Format("{0}*Total Crit %: {1}\nCrit % from Rating: {2}\nCrit % from Agility: {3}\nCrit % from Other: {4}", 
                                            baseStats.CritRating.ToString("n0"),
                                            PhysicalCrit.ToString("p"),
                                            critFromRating.ToString("p"),
                                            critFromAgi.ToString("p"),
                                            (PhysicalCrit - critFromRating - critFromAgi).ToString("p")));
            float hasteFromRating = StatConversion.GetPhysicalHasteFromRating(baseStats.HasteRating, CharacterLevel);
            retVal.Add("Haste Rating", string.Format("{0}*Total Haste %: {1}\nHaste % from Rating: {2}\nHaste % from Other: {3}\nEnergy Per Second: {4}",
                                            baseStats.HasteRating.ToString("n0"),
                                            PhysicalHaste.ToString("p"),
                                            hasteFromRating.ToString("p"),
                                            (PhysicalHaste - hasteFromRating).ToString("p"), 
                                            (10 + (10 * PhysicalHaste)).ToString("n")));
            float masteryFromRating = StatConversion.GetMasteryFromRating(baseStats.MasteryRating, CharacterLevel);
            retVal.Add("Mastery Rating", string.Format("{0}*Total Mastery %: {1}\nMastery % from Rating: {2}\nMastery % from Base: {3}\nMastery % from Buff: {4}", 
                                            baseStats.MasteryRating.ToString("n0"),
                                            Mastery.ToString("p"),
                                            (masteryFromRating * MasteryPerRating).ToString("p"),
                                            (8 * MasteryPerRating).ToString("p"),
                                            ((baseStats.Mastery - 8) * MasteryPerRating).ToString("p")));
            retVal.Add("Hit", string.Format("{0}*Hit Rating: {1}\n{2}", PhysicalHit.ToString("p"), baseStats.HitRating.ToString("n0"), tipMiss));
            retVal.Add("Expertise", string.Format("{0}*Expertise Rating: {1}\n{2}", Dodge.ToString("p"), baseStats.ExpertiseRating.ToString("n0"), tipDodgeParry));

            retVal.Add("Total Damage", MaxRotation.totalDamageDone.ToString("n0"));
            retVal.Add("Optimal Rotation", MaxRotation.optimalRotation());
            retVal.Add("Ability Breakdown", MaxRotation.byAbility());

            retVal.Add("Melee", MaxRotation.Melee.feralAbility.ToString());
            retVal.Add("Claw", MaxRotation.Claw.feralAbility.ToString());
            retVal.Add("Mangle", MaxRotation.Mangle.feralAbility.ToString());
            retVal.Add("Shred", MaxRotation.Shred.feralAbility.ToString());
            retVal.Add("Ravage", MaxRotation.Ravage.feralAbility.ToString());
            retVal.Add("Pounce", MaxRotation.Pounce.feralAbility.ToString());
            retVal.Add("Rake", MaxRotation.Rake.feralAbility.ToString());
            retVal.Add("Rip", MaxRotation.Rip.feralAbility.ToString());
            retVal.Add("Ferocious Bite", MaxRotation.FerociousBite.feralAbility.ToString());
            retVal.Add("Swipe", MaxRotation.Swipe.feralAbility.ToString());
            retVal.Add("Thrash", MaxRotation.Thrash.feralAbility.ToString());

			retVal.Add("Overall Points", OverallPoints.ToString("n"));
			retVal.Add("DPS Points", SubPoints[0].ToString("n"));
            retVal.Add("Survivability Points", SubPoints[1].ToString("n3"));

			return retVal;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
                case "Hit Rating": return BasicStats.HitRating;
                case "Hit Rating %": return StatConversion.GetPhysicalHitFromRating(BasicStats.HitRating) * 100;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Expertise": return StatConversion.GetExpertiseFromRating(BasicStats.ExpertiseRating);
                case "Expertise Rating %": return StatConversion.GetExpertiseFromRating(BasicStats.ExpertiseRating) * 0.25f;
			}
			return 0f;
		}
	}
}
