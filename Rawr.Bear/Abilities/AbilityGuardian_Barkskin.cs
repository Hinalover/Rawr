using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Barkskin : AbilityGuardian_Base
    {
        /// <summary>
        /// The Druid's skin becomes as tough as bark.  All damage taken is reduced by 20%.\n\n
        /// While protected, damaging attacks will not cause spellcasting delays.\n\n
        /// This spell is usable while stunned, frozen, incapacitated, feared or asleep.  Usable in all forms.  Lasts 12 sec.
        /// </summary>
        public AbilityGuardian_Barkskin()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Barkskin(GuardianCombatState CState)
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
            Name = "Barkskin";
            SpellID = 22812;
            SpellIcon = "spell_nature_stoneclawtotem";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;

            DamageType = ItemDamageType.Nature;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 60f;
            Duration = 12f;
            AbilityIndex = (int)FeralAbility.SurvivalInstincts;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public readonly float DamageReduction = 0.20f;

        public float CriticallyHit
        {
            get
            {
                return (guardianCombatState.Talents.GlyphOfBarkskin ? 0.25f : 0);
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