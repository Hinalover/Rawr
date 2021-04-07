using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_SavageDefense : AbilityGuardian_Base
    {
        private float dodgecap = 0.45f;
        /// <summary>
        /// Increases chance to dodge by 45% for 6 sec.
        /// </summary>
        public AbilityGuardian_SavageDefense()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_SavageDefense(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Savage Defense";
            SpellID = 62606;
            SpellIcon = "ability_racial_cannibalize";
            druidForm = new DruidForm[] { DruidForm.Bear };

            DamageType = ItemDamageType.Nature;
            
            isDoT = false;
            Duration = 6f;

            TriggersGCD = true;
            CastTime = 0;
            Duration = 6;
            Cooldown = 9;
            AbilityIndex = (int)FeralAbility.SavageDefense;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Rage = 60 - (guardianCombatState.Stats.Tier_13_2_piece ? 5 : 0 );
        }

        public float getDodgeFromAbility()
        {
            return dodgecap + (guardianCombatState.Stats.Tier_14_4_piece ? 0.05f : 0);
        }

        public float getUptime(float rage, float fightlength)
        {
            // TODO: Get a better uptime, This is close but not exact.
            float freeRage = (rage / Rage) * Duration / fightlength;
            // (FightLength/Cooldown + Charge) * AbilityLength / fightLength
            float Fightlength = (fightlength / Cooldown + 3) * Duration / fightlength;
            return (float)Math.Min(freeRage, Fightlength);
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
