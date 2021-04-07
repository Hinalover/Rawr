using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_SwipeCat : AbilityFeral_Base
    {
        /// <summary>
        /// Swipe nearby enemies, inflicting 525% weapon damage.
        /// </summary>
        public AbilityFeral_SwipeCat(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Swipe (Cat Form)";
            SpellID = 62078;
            SpellIcon = "inv_misc_monsterclaw_03";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 45 * (CombatState.BerserkUptime ? 0.5f : 1f);

            DamageType = ItemDamageType.Physical;
            BaseWeaponDamageMultiplier = 5.25f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.SwipeCatForm;
            Range = MELEE_RANGE;
            AOE = true;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
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
                                             * (1 + (CombatState.TigersFuryUptime ? 0.15f : 0f));
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
            return (BaseWeaponDamageMultiplier * CombatState.MainHand.WeaponDamage);
        }
    }
}