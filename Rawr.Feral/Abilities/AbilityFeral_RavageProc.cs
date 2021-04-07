using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_RavageProc : AbilityFeral_Base
    {
        /// <summary>
        /// Ravage the target, causing 950% damage plus 56 to the target.  
        /// Must be prowling and behind the target.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_RavageProc(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Ravage!";
            SpellID = 81170;
            SpellIcon = "ability_druid_ravage";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 60 * (1 - (CombatState.Talents.Stampede * .5f));
            ComboPoint = 1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 56;
            BaseWeaponDamageMultiplier = 9.5f;
            // Assume this is procced from Stampede thus no longer requires Prowling
            MustBeProwling = false;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.RavageProc;
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

        public override float CritChance
        {
            get
            {
                float crit = 0f;
                if (CombatState != null && CombatState.Stats != null)
                {
                    if (CombatState.Above80Percent)
                        crit += (0.25f + CombatState.Talents.PredatoryStrikes);
                    crit += CombatState.Stats.PhysicalCrit;
                }
                crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                crit *= HitChance;
                return Math.Max(0, Math.Min(1, crit));
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
