using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_ElusiveBrew : AbilityGuardian_Base
    {
        /// <summary>
        /// Increases your chance to dodge melee and ranged attacks by 10% for 8 sec.
        /// </summary>
        public AbilityGuardian_ElusiveBrew()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_ElusiveBrew(GuardianCombatState CState)
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
            Name = "Elusive Brew";
            SpellID = 126453;
            SpellIcon = "ability_monk_elusiveale";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 60f;
            Duration = 8f;
            AbilityIndex = (int)FeralAbility.ElusiveBrew;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public readonly float IncreasesDodge = 0.10f;

        public override float Formula()
        {
            return 0f;
        }
    }
}