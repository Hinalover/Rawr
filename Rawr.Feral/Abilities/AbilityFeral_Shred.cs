using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_Shred : AbilityFeral_Base
    {
        /// <summary>
        /// Shred the target, causing 540% damage plus 56 to the target.  Must be behind the target.  
        /// Awards 1 combo point.  Effects which increase Bleed damage also increase Shred damage.
        /// </summary>
        public AbilityFeral_Shred(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Shred";
            SpellID = 5221;
            SpellIcon = "spell_shadow_vampiricaura";
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
            AbilityIndex = (int)FeralAbility.Shred;
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
                float RendandTear = 0;
                switch (CombatState.Talents.RendAndTear)
                {
                    case 0:
                        RendandTear = 0;
                        break;
                    case 1:
                        RendandTear = .07f;
                        break;
                    case 2:
                        RendandTear = .13f;
                        break;
                    case 3:
                        RendandTear = .20f;
                        break;
                }
                if (_DamageMultiplierModifer == 0)
                {
                    _DamageMultiplierModifer = (1 + CombatState.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.Stats.TargetArmorReduction, CombatState.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime ? 0.15f : 0f))
                                             * (1 + RendandTear)
                                             * (1 + CombatState.Stats.BonusBleedDamageMultiplier);
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
            if (CombatState.AttackingFromBehind)
                return BaseDamage + (BaseWeaponDamageMultiplier * CombatState.MainHand.WeaponDamage);
            else
                return 0f;
        }
    }
}