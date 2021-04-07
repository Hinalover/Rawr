using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_FeralChargeCat : AbilityFeral_Base
    {
        /// <summary>
        /// Causes you to leap behind an enemy, dazing them for 3 sec.
        /// </summary>
        public AbilityFeral_FeralChargeCat()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_FeralChargeCat(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();
            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Feral Charge (Cat Form)";
            SpellID = 49376;
            SpellIcon = "spell_druid_feralchargecat";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 10 * (CombatState.BerserkUptime > 0 ? AbilityFeral_Berserk.CostReduction * CombatState.BerserkUptime : 1f);
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = true;
            TriggeredAbility = new AbilityFeral_Base[1];
            // Assume 1 second to move away from the boss and jump back in; may require 2 second Casttime
            CastTime = 1f;
            Cooldown = 30f;
            AbilityIndex = (int)FeralAbility.FeralChargeCat;
            Range = 8;
            AOE = false;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            /*if (CombatState.Talents.Stampede > 0)
                TriggeredAbility[1] = new AbilityFeral_RavageProc(CState);*/
            //CombatState.SavageRoarUptime = true;
        }

        public override float Formula()
        {
            this.ComboPoint = 0f;
            this.Duration = 0f;
            return 0f;
        }
    }
}

