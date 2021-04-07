using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_LifeTap : AbilityGuardian_Base
    {
        /// <summary>
        /// Converts 20% of your total health into 30 Rage.
        /// </summary>
        public AbilityGuardian_LifeTap()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_LifeTap(GuardianCombatState CState)
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
            Name = "Life Tap";
            SpellID = 122290;
            SpellIcon = "spell_shadow_burningspirit";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 30;

            DamageType = ItemDamageType.Shadow;
            BaseDamage = 0f;

            TriggersGCD = true;
            CastTime = 0f;
            Cooldown = 15f;
            Duration = 0f;
            AbilityIndex = (int)FeralAbility.LifeTap;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public const float LifeReduction = 0.20f;

        public float getTotalRageGenerated(float count)
        {
            return count * Rage;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}