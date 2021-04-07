using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_LeaderOfThePack : AbilityGuardian_Base
    {
        /// <summary>
        /// While in Bear Form or Cat Form, increases critical strike chance of all party and raid members within 100 yards by 5%.
        ///
        /// Also causes your melee critical strikes to heal you for 4% of your health and energize you for 8% of your mana.  
        /// This effect cannot occur more than once every 6 sec.
        /// </summary>
        public AbilityGuardian_LeaderOfThePack()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_LeaderOfThePack(GuardianCombatState CState)
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
            Name = "Leader of the Pack";
            SpellID = 17007;
            SpellIcon = "spell_nature_unyeildingstamina";
            druidForm = new DruidForm[] { DruidForm.Bear, DruidForm.Cat };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = 6f;
            Duration = 0f;
            AbilityIndex = (int)FeralAbility.LeaderofthePack;
            Range = 0;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public readonly float percentOfHealthRestored = 0.04f;

        public readonly float percentOfManaRestored = 0.08f;

        private float _baseCritInterval = 0;
        public float BaseCritInterval
        {
            get { return _baseCritInterval; }
            set { _baseCritInterval = value; }
        }

        public float CritInterval
        {
            get
            {
                return (float)Math.Max(_baseCritInterval, Cooldown);
            }
        }

        public float getTotalHealthRestored
        {
            get
            {
                return guardianCombatState.Stats.Health * percentOfHealthRestored;
            }
        }

        public float getTotalManaRestored
        {
            get
            {
                return guardianCombatState.Stats.Mana * percentOfManaRestored;
            }
        }

        public override float Formula()
        {
            return 0f;
        }

        public override string ToString()
        {
            float health = getTotalHealthRestored;
            float mana = getTotalManaRestored;
            float interval = CritInterval;
            string output = string.Format("{0}*Mana Restored: {1}\nInterval: {2}",
                            health.ToString("n0"),
                            mana.ToString("n0"),
                            interval.ToString("n2"));
            return output;
        }
    }
}