using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_MightofUrsoc : AbilityGuardian_Base
    {
        /// <summary>
        /// Increases current and maximum health by 30% for 20 sec.  Activates Bear Form.
        /// </summary>
        public AbilityGuardian_MightofUrsoc()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_MightofUrsoc(GuardianCombatState CState)
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
            Name = "Might of Ursoc";
            SpellID = 106922;
            SpellIcon = "spell_druid_mightofursoc";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Duration = 20f;
            AbilityIndex = (int)FeralAbility.MightofUrsoc;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Cooldown = (60f * (CState.Talents.GlyphofMightofUrsoc ? 5f : 3f)) - (CState.Stats.Tier_14_2_piece ? 60 : 0);
        }

        public float IncreasedMaximumHealth
        {
            get
            {
                return 0.30f + (guardianCombatState.Talents.GlyphofMightofUrsoc ? .20f : 0);
            }
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