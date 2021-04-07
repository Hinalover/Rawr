using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Berserk : AbilityGuardian_Base
    {
         /// <summary>
        /// Removes the cooldown from Mangle and causes it to hit up to 3 targets and lasts 10 sec.
        /// </summary>
        public AbilityGuardian_Berserk()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Berserk(GuardianCombatState CState)
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
            Name = "Berserk";
            SpellID = 106952;
            SpellIcon = "ability_druid_berserk";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f);
            Duration = 10f;
            AbilityIndex = (int)FeralAbility.Berserk;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public const float MangleCooldownReduction = 4.5f;

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