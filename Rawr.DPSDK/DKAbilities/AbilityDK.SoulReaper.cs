using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Soul Reaper Ability based on the AbilityDK_Base class.
    /// Strikes an enemy for 100% weapon damage and afflicts the target with Soul Reaper. After 5 sec, 
    /// if the target is below 35% health, this effect will deal 46114 to 53590 additional Shadow damage.  
    /// If the enemy dies before this effect triggers, the Death Knight gains 50% haste for 10 sec.
    /// </summary>
    class AbilityDK_SoulReaper : AbilityDK_Base
    {
        public AbilityDK_SoulReaper(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Soul Reaper";
            // 3 versions, 1 for each spec.
            this.AbilityCost[(int)CS.m_Spec] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = (uint)Math.Floor(850 * .8);
            this.fWeaponDamageModifier = 1.0f;
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.SoulReaper;
            // TODO: Implement Buff/Delayed effect.
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                return _DamageMultiplierModifer + base.DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                uint WDam = (uint)((this.wMH.damage) * this.fWeaponDamageModifier);
                // Off-hand damage is only effective if we have Threat of Thassaurian
                // And only for specific strikes as defined by the talent.
                return WDam;
            }
        }
    }
}
