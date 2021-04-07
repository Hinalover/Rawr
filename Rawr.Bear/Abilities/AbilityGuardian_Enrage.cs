using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Enrage : AbilityGuardian_Base
    {
        /// <summary>
        /// Generates 20 Rage, and then generates an additional 10 Rage over 10 sec.
        /// </summary>
        public AbilityGuardian_Enrage()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Enrage(GuardianCombatState CState)
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
            Name = "Enrage";
            SpellID = 5229;
            SpellIcon = "ability_druid_enrage";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 30;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 60f;
            Duration = 20f; // Just assume it takes the full 30 rage and not worry about the 20 then 10 over 10 seconds
            AbilityIndex = (int)FeralAbility.Enrage;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

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