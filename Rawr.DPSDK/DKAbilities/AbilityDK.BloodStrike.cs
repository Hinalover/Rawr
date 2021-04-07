using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Blood Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodStrike : AbilityDK_Base
    {
        public AbilityDK_BloodStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Blood Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = (uint)Math.Floor(850 * .8);
            this.fWeaponDamageModifier = 0.8f;
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.BloodStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CS.m_Spec == Rotation.Type.Unholy)
                this.AbilityCost[(int)DKCostTypes.Death] = -1; 
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
                return (CState.m_uDiseaseCount * .125f) + _DamageMultiplierModifer + base.DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        private bool m_bToT = false;

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                m_bToT = CState.m_Spec == Rotation.Type.Frost;
                uint WDam = (uint)((850 + this.wMH.damage) * this.fWeaponDamageModifier);
                // Off-hand damage is only effective if we have Threat of Thassaurian
                // And only for specific strikes as defined by the talent.
                float iToTMultiplier = 0;
                if (m_bToT && null != this.wOH) // DW
                {
                    iToTMultiplier = 1f;
                }
                if (this.wOH != null)
                    WDam += (uint)(this.wOH.damage * iToTMultiplier * this.fWeaponDamageModifier);
                return WDam;
            }
        }
    }
}
