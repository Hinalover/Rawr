using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_BoneShield : AbilityGuardian_Base
    {
        /// <summary>
        /// Surrounds you with a barrier of whirling bones.  The shield begins with 3 charges, 
        /// and each damaging attack consumes a charge.  While at least 1 charge remains, 
        /// you take 10% less damage from all sources.  Lasts 5 min.
        /// </summary>
        public AbilityGuardian_BoneShield()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_BoneShield(GuardianCombatState CState)
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
            Name = "Bone Shield";
            SpellID = 122285;
            SpellIcon = "ability_deathknight_boneshield";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Shadow;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 60f;
            Duration = 8f; // Estimate; TODO Get more accurate duration
            AbilityIndex = (int)FeralAbility.BoneShield;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public readonly float DamageReduction = 0.10f;

        public override float Formula()
        {
            return 0f;
        }
    }
}