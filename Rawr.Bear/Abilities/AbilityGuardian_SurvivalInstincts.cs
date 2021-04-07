using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_SurvivalInstincts : AbilityGuardian_Base
    {
        /// <summary>
        /// Reduces all damage taken by 50% for 12 sec.
        /// </summary>
        public AbilityGuardian_SurvivalInstincts()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_SurvivalInstincts(GuardianCombatState CState)
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
            Name = "Survival Instincts";
            SpellID = 61336;
            SpellIcon = "ability_druid_tigersroar";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            AbilityIndex = (int)FeralAbility.SurvivalInstincts;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public readonly float DamageReduction = 0.50f;

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Cooldown = 60f * (CState.Talents.GlyphofSurvivalInstincts ? 2 : 3);
            Duration = 12f * (CState.Talents.GlyphofSurvivalInstincts ? 0.50f : 1f);
        }

        public float PercentUptime(float fightlength)
        {
            return ((fightlength / Cooldown) * Duration) / fightlength;
        }
        
        public override float Formula()
        {
            return 0f;
        }
    }
}
