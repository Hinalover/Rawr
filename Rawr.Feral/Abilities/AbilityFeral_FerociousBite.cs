using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_FerociousBite : AbilityFeral_Base
    {
        private float baseEnergy = 25f;
        private float maxFormulaEnergy = 35f;
        /// <summary>
        /// Finishing move that causes damage per combo point and consumes up to 35 additional energy to 
        /// increase damage by up to 100%. Damage is increased by your attack power.
        /// 1 point:  (((230 + 576 * 1) + (0.109 * AP)) * FA) - (((498 + 576 * 1) + (0.109 * AP)) * FA) damage
        /// 2 points: (((230 + 576 * 2) + (0.218 * AP)) * FA) - (((498 + 576 * 2) + (0.218 * AP)) * FA) damage
        /// 3 points: (((230 + 576 * 3) + (0.327 * AP)) * FA) - (((498 + 576 * 3) + (0.327 * AP)) * FA) damage
        /// 4 points: (((230 + 576 * 4) + (0.436 * AP)) * FA) - (((498 + 576 * 4) + (0.436 * AP)) * FA) damage
        /// 5 points: (((230 + 576 * 5) + (0.545 * AP)) * FA) - (((498 + 576 * 5) + (0.545 * AP)) * FA) damage
        /// FA = Feral Agression = 1.10, 1.05, or 1.00
        /// </summary>
        public AbilityFeral_FerociousBite(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Ferocious Bite";
            SpellID = 22568;
            SpellIcon = "ability_druid_ferociousbite";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = baseEnergy * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = -1;

            DamageType = ItemDamageType.Physical;
            MinDamage = 230;
            MaxDamage = 498;

            Formula_CP_Base_Damage_Modifier = 576f;
            Formula_CP_AP_Modifier = 0.109f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.FerociousBite;
            Range = MELEE_RANGE;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            EnergyRefunded();
            if (Formula_CP != 0)
            {
                // Combo Points cannot be over 5 Combo Points
                if (Formula_CP > 5)
                    Formula_CP = 5;
                this.ComboPoint = -Formula_CP;
            }
            this.MainHand = CState.MainHand;
        }

        public override float CritChance
        {
            get
            {
                float crit = 0f;
                float RendandTear = 0f;
                switch (CombatState.Talents.RendAndTear)
                {
                    case 1:
                        RendandTear = .08f;
                        break;
                    case 2:
                        RendandTear = .17f;
                        break;
                    case 3:
                        RendandTear = .25f;
                        break;
                    default:
                        RendandTear = 0f;
                        break;
                }

                if (CombatState != null && CombatState.Stats != null)
                    crit += CombatState.Stats.PhysicalCrit;
                crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                crit *= HitChance;

                if (CombatState.NumberOfBleeds > 0)
                    crit += RendandTear;

                return Math.Max(0, Math.Min(1, crit));
            }
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
                                             * (1 + ((CombatState.Talents.FeralAggression > 0) ? (CombatState.Talents.FeralAggression * 0.05f) : 0))
                                             * (1 + ((Formula_Energy > maxFormulaEnergy) ? 1 : (Formula_Energy / maxFormulaEnergy)));
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
            if (Formula_CP == 0)
                return 0f;
            else
                return ((BaseDamage + (Formula_CP_Base_Damage_Modifier * Formula_CP)) + ((Formula_CP_AP_Modifier * Formula_CP) * CombatState.Stats.AttackPower));
        }

        public void resetBaseEnergy()
        {
            this.Energy = baseEnergy;
        }
    }
}
