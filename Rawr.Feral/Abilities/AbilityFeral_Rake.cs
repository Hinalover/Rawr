using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_Rake : AbilityFeral_Base
    {
        /// <summary>
        /// Rake the target for AP*0.147+56 Bleed damage and an additional (56 * 3 + AP * 0.441) 
        /// Bleed damage over 9 sec.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_Rake(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Rake";
            SpellID = 1822;
            SpellIcon = "ability_druid_disembowel";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 35 * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = 1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 56;
            isDoT = true;
            feralDoT.Interval = 3f * 1000f;
            feralDoT.BaseLength = 9f * 1000f;
            feralDoT.TotalLength = feralDoT.BaseLength + ((CombatState.Talents.EndlessCarnage > 0) ? (CombatState.Talents.EndlessCarnage * 3f * 1000f) : 0);
            Duration = feralDoT.TotalLength;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Rake;
            Range = MELEE_RANGE;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            ComboPoint += PrimalFury();
            EnergyRefunded();
            CombatState.NumberOfBleeds += 1;
            this.MainHand = CState.MainHand;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public override float DamageMultiplierModifer
        {
            get
            {
                if (_DamageMultiplierModifer == 0)
                {
                    _DamageMultiplierModifer = (1 + CombatState.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.Stats.TargetArmorReduction, CombatState.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime ? 0.15f : 0f))
                                             * (1 + CombatState.Stats.NonShredBleedDamageMultiplier) // Mastery Multiplier
                                             * (1 + CombatState.Stats.BonusBleedDamageMultiplier)
                                             * (1 + (CombatState.Stats.Tier_11_2pc ? 0.10f : 0f));  // Tier 11 2-piece Rake tick bonus
                }
                return _DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        /// <summary>
        /// Applies the initial damage of Rake.
        /// Originally set to a different coefficient than the tick damage; changed in Patch 4.2.
        /// Keeping these separated just in case
        /// </summary>
        /// <returns>Damage of the inital application of the attack</returns>
        private float Formula_Initial_Damage()
        {
            return (CombatState.Stats.AttackPower * 0.147f) + BaseDamage;
        }

        public override float Formula()
        {
            return (BaseDamage + (CombatState.Stats.AttackPower * 0.147f));
        }

        public override float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0f;
            float DamageCount = 1f;
            feralDoT.BaseDamage = this.Formula();
            Damage = this.GetTickDamage();
            DamageCount = feralDoT.TotalTickCount();
            if (Partial)
                Damage *= PartialValue;

            // Multiply by the number of ticks
            Damage *= DamageCount;
            
            // Add the initial Damage from the attack
            Damage += ((Formula_Initial_Damage() + DamageAdditiveModifer) * DamageMultiplierModifer);

            // Calculate the crit damage
            float CritDamage = 2 * Damage * CritChance;
            Damage = (Damage * (Math.Min(1, HitChance) - CritChance)) + CritDamage;

            return Damage;
        }
    }
}
