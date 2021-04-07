using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_FieryClaws : AbilityFeral_Base
    {
        /// <summary>
        /// Your attacks with Mangle, Maul, and Shred cause your target to burn for an 
        /// additional percentage of your attack's damage over 4 sec.
        /// </summary>
        public AbilityFeral_FieryClaws()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_FieryClaws(FeralCombatState CState)
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
            Name = "Fiery Claws";
            SpellID = 99002;
            SpellIcon = "inv_misc_volatilefire";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            Energy = 0;
            ComboPoint = 0;

            DamageType = ItemDamageType.Fire;
            BaseDamage = 0;
            isDoT = true;
            feralDoT.Interval = 2f;
            feralDoT.BaseLength = feralDoT.TotalLength = 4f;
            Duration = feralDoT.TotalLength;

            TriggersGCD = false;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.FieryClaws;
            Range = MELEE_RANGE;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public override float DamageMultiplierModifer
        {
            get
            {
                return _DamageMultiplierModifer;
            }
        }

        public override float Formula()
        {
            return (float)Math.Floor(BaseDamage);
        }
    }
}