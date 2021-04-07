using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_LightningShield : AbilityGuardian_Base
    {
        /// <summary>
        /// The caster is surrounded by 9 balls of lightning. When a spell, melee or 
        /// ranged attack hits the caster, the attacker will be struck for 402 Nature damage. 
        /// This expends one lightning ball. Only one ball will fire every few seconds.  
        /// Lasts 10 min. Only one of your Elemental Shields can be active on a target at any one time.
        /// </summary>
        public AbilityGuardian_LightningShield()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_LightningShield(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        private void baseInfo()
        {
            Name = "Lightning Shield";
            SpellID = 110803;
            SpellIcon = "spell_nature_lightningshield";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Nature;
            BaseSpellScaleModifier = 0.367646542f; // Estimate scaling value get exact value later
            BaseDamage = BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel) * BaseSpellScaleModifier;

            TriggersGCD = true;
            CastTime = 0f;
            Cooldown = 20f; // Estimate; TODO Get more accurate length
            Duration = 0f; 
            AbilityIndex = (int)FeralAbility.LightningShield;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public const float DamageReduction = 0.10f;

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
                    _DamageMultiplierModifer = (1 + guardianCombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + guardianCombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(guardianCombatState.Char.Level,
                                                                                            guardianCombatState.BossArmor,
                                                                                            guardianCombatState.MainHand.Stats.TargetArmorReduction,
                                                                                            guardianCombatState.MainHand.Stats.ArmorPenetration))
                                             * (1 + guardianCombatState.MainHand.Stats.BonusNatureDamageMultiplier);
                }
                return _DamageMultiplierModifer;
            }
        }

        public override float Formula()
        {
            return BaseDamage * 9;
        }

    }
}
