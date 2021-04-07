using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Feint : AbilityGuardian_Base
    {
        /// <summary>
        /// Performs an evasive maneuver, reducing damage taken from area-of-effect attacks by 10% for 5 sec.
        /// </summary>
        public AbilityGuardian_Feint()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Feint(GuardianCombatState CState)
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
            Name = "Fient";
            SpellID = 122289;
            SpellIcon = "ability_rogue_feint";
            druidForm = new DruidForm[] { DruidForm.Bear, DruidForm.Cat };

            Rage = 20;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 0f;
            Duration = 5f;
            AbilityIndex = (int)FeralAbility.Feint;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public readonly float AoEDamageReduction = 0.10f;

        public override float Formula()
        {
            return 0f;
        }
    }
}