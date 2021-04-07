using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Elemental.Spells;

namespace Rawr.Elemental
{
    public class CharacterCalculationsElemental : CharacterCalculationsBase
    {

        #region Variable Declarations and Definitions

        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public int TargetLevel { get; set; }
        public float Mastery { get; set; }
        public Character LocalCharacter { get; set; }

        public Spell LightningBolt;
        public Spell ChainLightning;
        public Spell LavaBurst;
        public Spell FlameShock;
        public Spell EarthShock;
        public Spell FrostShock;
        public Spell FireNova;
        public Spell SearingTotem;
        public Spell MagmaTotem;

        public float ManaRegen;
        public float ReplenishMP5;

        public float TimeToOOM;
        public float CastRegenFraction;
        public float RotationDPS;
        public float TotalDPS;
        public float RotationMPS;
        public float CastsPerSecond;
        public float CritsPerSecond;
        public float MissesPerSecond;

        public float LBPerSecond;
        public float LvBPerSecond;
        public float FSPerSecond;

        public float LatencyPerSecond;

        public float ClearCast_FlameShock;
        public float ClearCast_LavaBurst;
        public float ClearCast_LightningBolt;

        public string Rotation;
        public string RotationDetails;
        public string PriorityDetails;

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float BurstPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SustainedPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Burst Points", BurstPoints.ToString());
            dictValues.Add("Sustained Points", SustainedPoints.ToString());

            float BasicUntilGCDCap = (float)Math.Ceiling((1.5f / (1f + BasicStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - BasicStats.HasteRating);
            float BasicUntilLBCap = (float)Math.Ceiling((2f / (1f + BasicStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - BasicStats.HasteRating);

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
            dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString() + "*" + BasicUntilGCDCap + " Haste Rating until GCD cap\n" + BasicUntilLBCap + " Haste Rating until LB cap");
            dictValues.Add("Mastery Rating", BasicStats.MasteryRating.ToString());
            dictValues.Add("Mana Regen", Math.Round(ManaRegen).ToString() + " + " + Math.Round(ReplenishMP5).ToString() + "*All values are Mana per 5 seconds.\nMP5 while casting / MP5 while not casting + Replenishment");

            float CombatUntilGCDCap = (float)Math.Ceiling((1.5f / (1f + CombatStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - CombatStats.HasteRating);
            float CombatUntilLBCap = (float)Math.Ceiling((2f / (1f + CombatStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - CombatStats.HasteRating);

            dictValues.Add("Average Health", CombatStats.Health.ToString());
            dictValues.Add("Average Mana", CombatStats.Mana.ToString());
            dictValues.Add("Average Stamina", CombatStats.Stamina.ToString());
            dictValues.Add("Average Intellect", CombatStats.Intellect.ToString());
            dictValues.Add("Average Spell Power", CombatStats.SpellPower.ToString());
            dictValues.Add("Average Hit Rating", CombatStats.HitRating.ToString());
            dictValues.Add("Average Crit Rating", CombatStats.CritRating.ToString());
            dictValues.Add("Average Haste Rating", CombatStats.HasteRating.ToString() + "*" + CombatUntilGCDCap + " Haste Rating until GCD cap\n" + CombatUntilLBCap + " Haste Rating until LB cap");
            dictValues.Add("Average Mastery Rating", CombatStats.MasteryRating.ToString());
            dictValues.Add("Average Mana Regen", Math.Round(ManaRegen).ToString() + " + " + Math.Round(ReplenishMP5).ToString() + "*All values are Mana per 5 seconds.\nMP5 while casting / MP5 while not casting + Replenishment");

            dictValues.Add("Lightning Bolt", Math.Round(LightningBolt.MinHit).ToString() + "-" + Math.Round(LightningBolt.MaxHit).ToString() + " / " + Math.Round(LightningBolt.MinCrit).ToString() + "-" + Math.Round(LightningBolt.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LightningBolt.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LightningBolt.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LightningBolt.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LightningBolt.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LightningBolt, 2).ToString() + " %");
            dictValues.Add("Chain Lightning", Math.Round(ChainLightning.MinHit).ToString() + "-" + Math.Round(ChainLightning.MaxHit).ToString() + " / " + Math.Round(ChainLightning.MinCrit).ToString() + "-" + Math.Round(ChainLightning.MaxCrit).ToString() + "*Targets: " + (((ChainLightning)ChainLightning).AdditionalTargets + 1) + "\nMana cost: " + Math.Round(ChainLightning.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * ChainLightning.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * ChainLightning.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(ChainLightning.CastTime, 2) + " sec.");
            dictValues.Add("Lava Burst", Math.Round(LavaBurst.MinHit).ToString() + "-" + Math.Round(LavaBurst.MaxHit).ToString() + " / " + Math.Round(LavaBurst.MinCrit).ToString() + "-" + Math.Round(LavaBurst.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LavaBurst.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LavaBurst.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LavaBurst.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LavaBurst.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LavaBurst, 2).ToString() + " %");
            dictValues.Add("Flame Shock", Math.Round(FlameShock.AvgHit).ToString() + " / " + Math.Round(FlameShock.AvgCrit).ToString() + " + " + Math.Round(FlameShock.PeriodicTick).ToString() + " (dot)*Duration: " + FlameShock.Duration + "\nTicks every " + Math.Round(FlameShock.PeriodicTickTime, 2).ToString() + "s\nMana cost: " + Math.Round(FlameShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FlameShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FlameShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FlameShock.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_FlameShock, 2).ToString() + " %");
            dictValues.Add("Earth Shock", Math.Round(EarthShock.MinHit).ToString() + "-" + Math.Round(EarthShock.MaxHit).ToString() + " / " + Math.Round(EarthShock.MinCrit).ToString() + "-" + Math.Round(EarthShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(EarthShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * EarthShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * EarthShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(EarthShock.CastTime, 2) + " sec.\n");
            dictValues.Add("Frost Shock", Math.Round(FrostShock.MinHit).ToString() + "-" + Math.Round(FrostShock.MaxHit).ToString() + " / " + Math.Round(FrostShock.MinCrit).ToString() + "-" + Math.Round(FrostShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(FrostShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FrostShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FrostShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FrostShock.CastTime, 2) + " sec.\n");
            dictValues.Add("Searing Totem", Math.Round(SearingTotem.PeriodicTick).ToString() + "*Mana cost: " + Math.Round(SearingTotem.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * SearingTotem.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * SearingTotem.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(SearingTotem.CastTime, 2).ToString() + " sec.\n");
            dictValues.Add("Magma Totem", Math.Round(MagmaTotem.PeriodicTick).ToString() + "*Targets: " + (((MagmaTotem)MagmaTotem).AdditionalTargets + 1) + "\nMana cost: " + Math.Round(MagmaTotem.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * MagmaTotem.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * MagmaTotem.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(MagmaTotem.CastTime, 2).ToString() + " sec.\n");
            //dictValues.Add("Unleash Elements");

            dictValues.Add("Simulation", Math.Round(RotationDPS).ToString() + ((Math.Abs(RotationDPS - TotalDPS) >= 1) ? (" (" + Math.Round(TotalDPS).ToString() + ")") : "") + " DPS*OOM after " + Math.Round(TimeToOOM).ToString() + " sec.\nDPS until OOM: " + Math.Round(RotationDPS).ToString() + "\nMPS until OOM: " + Math.Round(RotationMPS).ToString() + "\nCast vs regen fraction after OOM: " + Math.Round(CastRegenFraction, 4).ToString() + "\n" + Math.Round(60f * CastsPerSecond, 1).ToString() + " casts per minute\n" + Math.Round(60f * CritsPerSecond, 1).ToString() + " crits per minute\n" + Math.Round(60f * MissesPerSecond, 1).ToString() + " misses per minute\n" + Math.Round(60f * LvBPerSecond, 1).ToString() + " Lava Bursts per minute\n" + Math.Round(60f * FSPerSecond, 1).ToString() + " Flame Shocks per minute\n" + Math.Round(60f * LBPerSecond, 1).ToString() + " Lightning Bolts per minute\n" + Math.Round(60f * LatencyPerSecond, 1).ToString() + " seconds lost to latency per minute\n");
            dictValues.Add("Rotation", Rotation + "*" + RotationDetails);

            return dictValues;
        }
    }
}
