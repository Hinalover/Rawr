using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_Claw : AbilityFeral_Base
    {
        /// <summary>
        /// Claw the enemy, causing 155% of normal damage plus 710.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_Claw(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Claw";
            SpellID = 1082;
            SpellIcon = "ability_druid_rake";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 40 * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = 1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 710;
            BaseWeaponDamageMultiplier = 1.55f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Claw;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            ComboPoint += PrimalFury();
            EnergyRefunded();
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
                                             * (1 + (CombatState.TigersFuryUptime ? 0.15f : 0));
                }
                return _DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        public override float Formula()
        {
            return BaseDamage + (BaseWeaponDamageMultiplier * CombatState.MainHand.WeaponDamage);
        }
    }
}
