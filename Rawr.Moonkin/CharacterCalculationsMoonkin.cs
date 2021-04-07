using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    public class CharacterCalculationsMoonkin : CharacterCalculationsBase
    {
        private float overallPoints = 0f;
        public override float OverallPoints { get { return overallPoints; } set { overallPoints = value; } }

        private float[] subPoints = new float[] { 0f, 0f };

        public override float[] SubPoints { get { return subPoints; } set { subPoints = value; } }

        public float SpellCritPenalty { get; set; }
        public float SpellCrit { get; set; }
        public float SpellHaste { get; set; }
        public float SpellPower { get; set; }
        public float Mastery { get; set; }
        public float ManaRegen { get; set; }
        public StatsMoonkin BasicStats { get { return baseStats; } set { baseStats = value; } }
        private StatsMoonkin baseStats;
        public RotationData SelectedRotation { get; set; }
        public RotationData BurstRotation { get; set; }
        public RotationData[] Rotations = new RotationData[1];
        public bool PTRMode { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            //
            if (baseStats == null) baseStats = new StatsMoonkin();
            if (SelectedRotation == null) SelectedRotation = new RotationData();
            if (BurstRotation == null) BurstRotation = new RotationData();
            //
            retVal.Add("Health", baseStats.Health.ToString());
            retVal.Add("Mana", baseStats.Mana.ToString());
            retVal.Add("Armor", baseStats.Armor.ToString());
            retVal.Add("Agility", baseStats.Agility.ToString());
            retVal.Add("Stamina", baseStats.Stamina.ToString());
            retVal.Add("Intellect", baseStats.Intellect.ToString());
            retVal.Add("Spirit", baseStats.Spirit.ToString());
            retVal.Add("Spell Power", SpellPower.ToString());

            retVal.Add("Spell Crit", String.Format("{0:F}%*{1} Crit Rating, {2:F}% Crit From Gear, {3:F}% Crit From Intellect",
                100 * (SpellCrit - SpellCritPenalty),
                baseStats.CritRating,
                100 * StatConversion.GetSpellCritFromRating(baseStats.CritRating),
                100 * StatConversion.GetSpellCritFromIntellect(baseStats.Intellect)));
            retVal.Add("Spell Haste", String.Format("{0:F}%*{1} Haste Rating, {2:F}% Haste From Gear",
                100 * SpellHaste,
                baseStats.HasteRating,
                100 * StatConversion.GetSpellHasteFromRating(baseStats.HasteRating)));
            retVal.Add("Mastery", String.Format("{0:F}%*{1} Mastery Rating, {2:F}% Mastery From Gear",
                Mastery * 1.5f,
                baseStats.MasteryRating,
                StatConversion.GetMasteryFromRating(baseStats.MasteryRating) * 1.5f));
            retVal.Add("Mana Regen", String.Format("{0:F0}", ManaRegen * 5.0f));
            retVal.Add("Total Score", String.Format("{0:F2}", OverallPoints));
            retVal.Add("Selected Rotation", String.Format("*{0}", SelectedRotation.Name));
            retVal.Add("Selected DPS", String.Format("{0:F2}", SelectedRotation.SustainedDPS));
            retVal.Add("Selected Time To OOM", String.Format(SelectedRotation.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", SelectedRotation.TimeToOOM.Minutes, SelectedRotation.TimeToOOM.Seconds));
            retVal.Add("Selected Cycle Length", String.Format("{0:F1} s", SelectedRotation.Duration));

            StringBuilder sb = new StringBuilder("*");
            float rotationDamage = SelectedRotation.SustainedDPS * SelectedRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * SelectedRotation.StarfireAvgHit * SelectedRotation.StarfireCount / rotationDamage,
                SelectedRotation.StarfireAvgHit * SelectedRotation.StarfireCount,
                SelectedRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (SelectedRotation.MoonfireAvgHit) * SelectedRotation.MoonfireCasts / rotationDamage,
                (SelectedRotation.MoonfireAvgHit) * SelectedRotation.MoonfireCasts,
                SelectedRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * SelectedRotation.WrathAvgHit * SelectedRotation.WrathCount / rotationDamage,
                SelectedRotation.WrathAvgHit * SelectedRotation.WrathCount,
                SelectedRotation.WrathCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starsurge", 100 * SelectedRotation.StarSurgeAvgHit * SelectedRotation.StarSurgeCount / rotationDamage,
                SelectedRotation.StarSurgeAvgHit * SelectedRotation.StarSurgeCount,
                SelectedRotation.StarSurgeCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Sunfire", 100 * SelectedRotation.SunfireAvgHit * SelectedRotation.SunfireCasts / rotationDamage,
                SelectedRotation.SunfireAvgHit * SelectedRotation.SunfireCasts,
                SelectedRotation.SunfireCasts));

            retVal.Add("Selected Spell Breakdown", sb.ToString());

            retVal.Add("Starfire", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2}% avg energy",
                SelectedRotation.StarfireAvgHit / (SelectedRotation.StarfireAvgCast > 0 ? SelectedRotation.StarfireAvgCast : 1f),
                SelectedRotation.StarfireAvgCast,
                SelectedRotation.StarfireAvgHit,
                100 * (SelectedRotation.StarfireAvgEnergy - 1)));
            retVal.Add("Wrath", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2}% avg energy",
                SelectedRotation.WrathAvgHit / (SelectedRotation.WrathAvgCast > 0 ? SelectedRotation.WrathAvgCast : 1f),
                SelectedRotation.WrathAvgCast,
                SelectedRotation.WrathAvgHit,
                100 * (SelectedRotation.WrathAvgEnergy - 1)));
            retVal.Add("Starsurge", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2}% avg energy",
                SelectedRotation.StarSurgeAvgHit / (SelectedRotation.StarSurgeAvgCast > 0 ? SelectedRotation.StarSurgeAvgCast : 1f),
                SelectedRotation.StarSurgeAvgCast,
                SelectedRotation.StarSurgeAvgHit,
                100 * (SelectedRotation.StarSurgeAvgEnergy - 1)));
            retVal.Add("Moonfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit\n{3:F2}% avg energy",
                SelectedRotation.MoonfireAvgHit / (SelectedRotation.MoonfireDuration > 0 ? SelectedRotation.MoonfireDuration : 1f),
                SelectedRotation.MoonfireAvgCast,
                SelectedRotation.MoonfireAvgHit,
                100 * (SelectedRotation.MoonfireAvgEnergy - 1)));
            retVal.Add("Sunfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit\n{3:F2}% avg energy",
                SelectedRotation.SunfireAvgHit / (SelectedRotation.SunfireDuration > 0 ? SelectedRotation.SunfireDuration : 1f),
                SelectedRotation.SunfireAvgCast,
                SelectedRotation.SunfireAvgHit,
                100 * (SelectedRotation.SunfireAvgEnergy - 1)));

            retVal.Add("Burst Rotation", String.Format("*{0}", BurstRotation.Name));
            retVal.Add("Burst DPS", String.Format("{0:F2}", BurstRotation.BurstDPS));
            retVal.Add("Burst Time To OOM", String.Format(BurstRotation.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", BurstRotation.TimeToOOM.Minutes, BurstRotation.TimeToOOM.Seconds));
            retVal.Add("Burst Cycle Length", String.Format("{0:F1} s", BurstRotation.Duration));

            sb = new StringBuilder("*");
            rotationDamage = BurstRotation.BurstDPS * BurstRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * BurstRotation.StarfireAvgHit * BurstRotation.StarfireCount / rotationDamage,
                BurstRotation.StarfireAvgHit * BurstRotation.StarfireCount,
                BurstRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (BurstRotation.MoonfireAvgHit) * BurstRotation.MoonfireCasts / rotationDamage,
                (BurstRotation.MoonfireAvgHit) * BurstRotation.MoonfireCasts,
                BurstRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * BurstRotation.WrathAvgHit * BurstRotation.WrathCount / rotationDamage,
                BurstRotation.WrathAvgHit * BurstRotation.WrathCount,
                BurstRotation.WrathCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starsurge", 100 * BurstRotation.StarSurgeAvgHit * BurstRotation.StarSurgeCount / rotationDamage,
                BurstRotation.StarSurgeAvgHit * BurstRotation.StarSurgeCount,
                BurstRotation.StarSurgeCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Sunfire", 100 * BurstRotation.SunfireAvgHit * BurstRotation.SunfireCasts / rotationDamage,
                BurstRotation.SunfireCasts * BurstRotation.SunfireCasts,
                BurstRotation.SunfireCasts));

            retVal.Add("Burst Spell Breakdown", sb.ToString());

            /*retVal.Add("Starfire", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2} avg energy",
                BurstRotation.StarfireAvgHit / (BurstRotation.StarfireAvgCast > 0 ? BurstRotation.StarfireAvgCast : 1f),
                BurstRotation.StarfireAvgCast,
                BurstRotation.StarfireAvgHit,
                BurstRotation.StarfireAvgEnergy - 1));
            retVal.Add("Wrath", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2} avg energy",
                BurstRotation.WrathAvgHit / (BurstRotation.WrathAvgCast > 0 ? BurstRotation.WrathAvgCast : 1f),
                BurstRotation.WrathAvgCast,
                BurstRotation.WrathAvgHit,
                BurstRotation.WrathAvgEnergy - 1));
            retVal.Add("Starsurge", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F2} avg energy",
                BurstRotation.StarSurgeAvgHit / (BurstRotation.StarSurgeAvgCast > 0 ? BurstRotation.StarSurgeAvgCast : 1f),
                BurstRotation.StarSurgeAvgCast,
                BurstRotation.StarSurgeAvgHit,
                BurstRotation.StarSurgeAvgEnergy - 1));
            retVal.Add("Moonfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit\n{3:F2} avg energy",
                BurstRotation.MoonfireAvgHit / (BurstRotation.MoonfireDuration > 0 ? BurstRotation.MoonfireDuration : 1f),
                BurstRotation.MoonfireAvgCast,
                BurstRotation.MoonfireAvgHit,
                BurstRotation.MoonfireAvgEnergy - 1));
            retVal.Add("Insect Swarm", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit{3:F2} avg energy",
                BurstRotation.SunfireAvgHit / (BurstRotation.SunfireDuration > 0 ? BurstRotation.SunfireDuration : 1f),
                BurstRotation.SunfireAvgCast,
                BurstRotation.SunfireAvgHit,
                BurstRotation.SunfireAvgEnergy - 1));*/

            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Haste Rating": return baseStats.HasteRating;
                case "Crit Rating": return baseStats.CritRating;
                case "Mastery Rating": return baseStats.MasteryRating;
            }
            return 0;
        }
    }
}
