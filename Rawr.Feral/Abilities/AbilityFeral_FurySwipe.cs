using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_FurySwipe : AbilityFeral_Base
    {
        /// <summary>
        /// When you autoattack while in Cat Form or Bear Form, you have a chance to cause a Fury Swipe dealing 
        /// 310% weapon damage. This effect cannot occur more than once every 3 sec.
        /// </summary>
        public AbilityFeral_FurySwipe(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Fury Swipes";
            SpellID = 80861;
            SpellIcon = "ability_druid_rake";
            druidForm = new DruidForm[]{ DruidForm.Cat, DruidForm.Bear };

            Energy = 0;
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;
            BaseWeaponDamageMultiplier = 3.1f;

            TriggersGCD = false;
            CastTime = 0;
            Cooldown = 3f * 1000f;
            AbilityIndex = (int)FeralAbility.FurySwipe;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
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
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.Stats.TargetArmorReduction, CombatState.Stats.ArmorPenetration));
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
            return BaseWeaponDamageMultiplier * CombatState.MainHand.WeaponDamage;
        }
    }
}
