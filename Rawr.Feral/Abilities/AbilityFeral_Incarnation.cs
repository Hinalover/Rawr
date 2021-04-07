using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Incarnation : AbilityFeral_Base
    {
        /// <summary>
        /// This improved Cat Form allows the use of all abilities which normally require stealth, 
        /// as well as allowing Prowl while in combat.
        /// You may shapeshift in and out of this improved Cat Form for the duration of Incarnation.
        /// </summary>
        public AbilityFeral_Incarnation()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_Incarnation(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();
            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        private void baseInfo()
        {
            Name = "Incarnation: King of the Jungle";
            SpellID = 50334;
            SpellIcon = "spell_druid_incarnation";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 0f;
            ComboPoint = 0f;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f);
            Duration = 30f;
            AbilityIndex = (int)FeralAbility.Incarnation;
            Range = MELEE_RANGE;
            AOE = false;
        }
        
        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
