using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Feral
{
	public class StatsFeral : Stats
	{
		public float NonShredBleedDamageMultiplier { get; set; }
		public float MangleDamageMultiplier { get; set; }
		public float ShredDamageMultiplier { get; set; }
		public float FerociousBiteDamageMultiplier { get; set; }
		public float SavageRoarDamageMultiplierIncrease { get; set; }
		public float EnergyOnTigersFury { get; set; }
		public float MaxEnergyOnTigersFuryBerserk { get; set; }
		public float RavageCritChanceOnTargetsAbove80Percent { get; set; }
		public float FurySwipesChance { get; set; }
		public float BonusBerserkDuration { get; set; }
		public float TigersFuryCooldownReduction { get; set; }
		public float FeralChargeCatCooldownReduction { get; set; }
		public float CPOnCrit { get; set; }
		public float FerociousBiteMaxExtraEnergyReduction { get; set; }
		public float FreeRavageOnFeralChargeChance { get; set; }
		public float RipRefreshChanceOnFerociousBiteOnTargetsBelow25Percent { get; set; }

        /// <summary>Increases movement speed of the Druid</summary>
        public float FeralMovementSpeed { get; set; }

        public float MaxEnergy { get; set; }
        /// <summary>Increases the periodic damage done by your Rake and Lacerate abilities by 10%.</summary>
        public bool Tier_11_2pc { get; set; }
        /// <summary>Each time you use Mangle (Cat) you gain a 1% increase to attack power for 30 sec stacking up to 3 times</summary>
        public bool Tier_11_4pc { get; set; }
        /// <summary>Your attacks with Mangle, Maul, and Shred deal 10% additional damage as Fire damage over 4 sec.</summary>
        public bool Tier_12_2pc { get; set; }
        /// <summary>Your finishing moves have a 20% chance per combo point to extend the duration of Berserk by 2 + (20 * ComboPoints) sec</summary>
        public bool Tier_12_4pc { get; set; }
        /// <summary>Ferocious Bite now refreshes the duration of your Rip on targets with 60% or less health.</summary>
        public bool Tier_13_2_piece { get; set; }
        /// <summary>Using Tiger's Fury will cause your next Ravage to be free, not require stealth, and ignore positional requirements.</summary>
        public bool Tier_13_4_piece { get; set; }
        /// <summary>Your Shred and Mangle (Cat) abilities deal 5% additional damage.</summary>
        public bool Tier_14_2_piece { get; set; }
        /// <summary>Increases the duration of your Rip by 4 sec.</summary>
        public bool Tier_14_4_piece { get; set; }
        public float PhysicalDamageProcs { get; set; }
		
		public WeightedStat[] TemporaryCritRatingUptimes { get; set; }
	}
}
