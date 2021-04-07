using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_WhiteSwing : AbilityFeral_Base
    {
        /// <summary>
        /// Automatically attacks a target in melee with an equipped weapon. This ability can be toggled on or off.
        /// </summary>
        public AbilityFeral_WhiteSwing(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "White Swing";
            SpellID = 6603;
            SpellIcon = "ability_druid_catformattack";
            druidForm = new DruidForm[]{ DruidForm.Cat, DruidForm.Bear };

            Energy = 0;
            Rage = 10;
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = CombatState.MainHand.WeaponDamage;

            TriggersGCD = false;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            Cooldown = 0;
            AbilityIndex = (int)FeralAbility.WhiteSwing;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            // TODO implement the Talent % for activating Fury Swipe
            TriggeredAbility[1] = new AbilityFeral_FurySwipe(CState);
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
                                             * (1 + (CombatState.SavageRoarUptime ? 0.80f : 0f));
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
            return BaseDamage;
        }
    }
}