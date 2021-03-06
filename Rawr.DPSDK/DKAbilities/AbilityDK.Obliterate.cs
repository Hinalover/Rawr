using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Obliterate Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_Obliterate : AbilityDK_Base
    {
        public AbilityDK_Obliterate(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Obliterate";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
            // Physical Damage * .125 * # diseases on target may consume the diseases.
            this.AbilityIndex = (int)DKability.Obliterate;
            UpdateCombatState(CS);
        }

        private bool m_bToT = false;
        
        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -20;
            this.wMH = CState.MH;
            this.wOH = CState.OH;
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                m_bToT = CState.m_Spec == Rotation.Type.Frost;
                uint WDam = (uint)((650 + this.wMH.damage) * this.fWeaponDamageModifier);
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

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                float multiplier = 1 + (CState.m_uDiseaseCount * .125f); 
                multiplier *= 1 + _DamageMultiplierModifer;
                multiplier *= 1 + base.DamageMultiplierModifer;
                multiplier *= 1 + (CState.m_Talents.GlyphofObliterate ? .20f : 0);
                return multiplier - 1 ;
            }
        }

        private float _BonusCritChance = 0;
        public override float CritChance
        {
            get
            {
                if (CState.m_Spec == Rotation.Type.Frost)
                    return Math.Max(0, Math.Min(1, _BonusCritChance));
                else
                    return base.CritChance;
            }
        }
        public void SetKMCritChance(float value)
        {
            _BonusCritChance = value;
        }
    }
}
