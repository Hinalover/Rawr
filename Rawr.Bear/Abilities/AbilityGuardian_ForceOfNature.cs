using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_ForceOfNature : AbilityGuardian_Base
    {
         /// <summary>
        /// Summons 3 treants to protect the summoner and nearby allies for 15 sec.
        /// </summary>
        public AbilityGuardian_ForceOfNature()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_ForceOfNature(GuardianCombatState CState)
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
            Name = "Force of Nature";
            SpellID = 102706;
            SpellIcon = "ability_druid_forceofnature";
            druidForm = new DruidForm[] { DruidForm.Bear };

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = true;
            CastTime = 0f;
            Cooldown = 60f;
            Duration = 15f;
            AbilityIndex = (int)FeralAbility.ForceOfNature;
            Range = 40;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
