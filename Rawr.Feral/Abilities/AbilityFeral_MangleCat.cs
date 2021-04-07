using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_MangleCat : AbilityFeral_Base
    {
        /// <summary>
        /// Mangle the target for 540% normal damage plus 56 and causes the target to take 30% 
        /// additional damage from bleed effects for 1 min.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_MangleCat(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Mangle (Cat Form)";
            SpellID = 33876;
            SpellIcon = "ability_druid_mangle2";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 40 * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = 1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 56;
            BaseWeaponDamageMultiplier = 5.40f;

            TriggersGCD = true;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.MangleCatForm;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            ComboPoint += PrimalFury();
            EnergyRefunded();
            if (CombatState.Stats.Tier_12_2pc)
            {
                TriggeredAbility[1] = new AbilityFeral_FieryClaws(CState);
                TriggeredAbility[1].BaseDamage = this.TotalDamage * 0.10f;
            }
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
                                             * (1 + (CombatState.Talents.GlyphOfMangle ? 0.1f : 0));
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