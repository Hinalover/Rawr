using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_SavageRoar : AbilityFeral_Base
    {
        /// <summary>
        /// Finishing move that consumes combo points on any nearby target to increase autoattack damage done by 80%.  
        /// Only useable while in Cat Form.  Lasts longer per combo point:
        /// 1 point : (9 sec) seconds
        /// 2 points: (18 sec) seconds
        /// 3 points: (27 sec) seconds
        /// 4 points: (36 sec) seconds
        /// 5 points: (45 sec) seconds
        /// </summary>
        public AbilityFeral_SavageRoar(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Savage Roar";
            SpellID = 52610;
            SpellIcon = "ability_druid_skinteeth";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 25 * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.SavageRoar;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.ComboPoint = -Formula_CP;
            this.Duration = 9f * Formula_CP * 1000f;
            this.MainHand = CState.MainHand;
            CombatState.SavageRoarUptime = true;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
