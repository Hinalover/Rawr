using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Incarnation : AbilityGuardian_Base
    {
         /// <summary>
        /// This improved Bear Form reduces the cooldown on all melee damage abilities and Growl to 1.5 sec.
        /// </summary>
        public AbilityGuardian_Incarnation()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Incarnation(GuardianCombatState CState)
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
            Name = "Incarnation: Son of Ursoc";
            SpellID = 102558;
            SpellIcon = "spell_druid_incarnation";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f);
            Duration = 30f;
            AbilityIndex = (int)FeralAbility.Incarnation;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public const float MangleCooldownReduction = 4.5f;

        public override float Formula()
        {
            return 0f;
        }
    }
}