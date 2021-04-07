using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Renewal : AbilityGuardian_Base
    {
        /// <summary>
        /// Instantly heals the Druid for 30% of maximum health.  Useable in all shapeshift forms.
        /// </summary>
        public AbilityGuardian_Renewal()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Renewal(GuardianCombatState CState)
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
            Name = "Renewal";
            SpellID = 108238;
            SpellIcon = "spell_nature_natureblessing";
            druidForm = new DruidForm[] { DruidForm.Bear, DruidForm.Cat, DruidForm.Boomkin, DruidForm.Caster, DruidForm.Tree };

            DamageType = ItemDamageType.Nature;
            BaseDamage = 0f;

            TriggersGCD = true;
            CastTime = 0f;
            Cooldown = 60f * 2f;
            Duration = 0;
            AbilityIndex = (int)FeralAbility.Renewal;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public readonly float PercentOfHealthHealed = 0.30f;

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        private float h_Formula()
        {
            return guardianCombatState.Stats.Health * PercentOfHealthHealed;
        }

        public float HealingFormula
        {
            get
            {
                if (guardianCombatState.Talents.Renewal > 0)
                    return h_Formula();
                else
                    return 0f;
            }
        }

        public override float Formula()
        {
            return 0f;
        }

        public override string ToString()
        {
            return string.Format("{0}", h_Formula().ToString("n"));
        }
    }
}
