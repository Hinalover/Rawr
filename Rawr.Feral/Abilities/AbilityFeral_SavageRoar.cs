using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_SavageRoar : AbilityFeral_Base
    {
        /// <summary>
        /// Finishing move that consumes combo points on any nearby target to increase autoattack damage done by 80%.  
        /// Only useable while in Cat Form.  Lasts longer per combo point:
        /// 1 point : (18 sec) seconds
        /// 2 points: (24 sec) seconds
        /// 3 points: (30 sec) seconds
        /// 4 points: (36 sec) seconds
        /// 5 points: (42 sec) seconds
        /// </summary>
        public AbilityFeral_SavageRoar()
        {
            CombatState = new FeralCombatState();
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;
        }

        public AbilityFeral_SavageRoar(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        public AbilityFeral_SavageRoar(FeralCombatState CState, float CP)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = CP;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Savage Roar";
            SpellID = 52610;
            SpellIcon = "ability_druid_skinteeth";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 25 * (CombatState.BerserkUptime > 0 ? CombatState.BerserkUptime * 0.5f : 1f);
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.SavageRoar;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public const float DamageBonus = 0.30f;

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.ComboPoint = -Formula_CP;
            //this.Duration = this.getSRLength(Formula_CP);
            this.Duration = getSRLength(Formula_CP);
            this.MainHand = CState.MainHand;
        }

        public override float Formula()
        {
            return 0f;
        }

        public static float getSRLength(float CP)
        {
            // return 17f + (CP * 5f);
            return (12f + (CP * 6f));
        }
    }
}
