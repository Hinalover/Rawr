using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_FurySwipe : AbilityFeral_Base
    {
        /// <summary>
        /// When you autoattack while in Cat Form or Bear Form, you have a chance to cause a Fury Swipe dealing 
        /// 310% weapon damage. This effect cannot occur more than once every 3 sec.
        /// </summary>
        public AbilityFeral_FurySwipe()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_FurySwipe(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();
            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Fury Swipes";
            SpellID = 80861;
            SpellIcon = "ability_druid_rake";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            Energy = 0;
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;
            BaseWeaponDamageMultiplier = 3.1f;

            TriggersGCD = false;
            CastTime = 0;
            Cooldown = 3f;
            AbilityIndex = (int)FeralAbility.FurySwipe;
            Range = MELEE_RANGE;
            AOE = false;
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
                    _DamageMultiplierModifer = (1 + CombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.MainHand.Stats.TargetArmorReduction, CombatState.MainHand.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime > 0 ? AbilityFeral_TigersFury.DamageBonus * CombatState.TigersFuryUptime : 0f))
                                             * (1 + (CombatState.SavageRoarUptime > 0 ? AbilityFeral_SavageRoar.DamageBonus * CombatState.SavageRoarUptime : 0f));
                }
                return _DamageMultiplierModifer;
            }
        }

        public override float Formula()
        {
            return BaseWeaponDamageMultiplier * CombatState.MainHand.WeaponDamage;
        }
    }
}
