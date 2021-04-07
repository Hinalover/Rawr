using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Strike Ability based on the AbilityDK_Base class.
    /// A deadly attack that deals 150% weapon damage plus (((330  * 150 / 100))), healing you for 15% 
    /// of the damage you have sustained during the preceding 5 sec (minimum of at least 7% of your maximum health).
    /// </summary>
    class AbilityDK_DeathStrike : AbilityDK_Base
    {
        private bool m_bToT = false;
        public AbilityDK_DeathStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Death Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.DeathStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CS.m_Spec == Rotation.Type.Blood)
                this.AbilityCost[(int)DKCostTypes.Death] = -2;
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }
        
        
        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                this.m_bToT = CState.m_Spec == Rotation.Type.Frost;

                uint WDam = (uint)((330 + this.wMH.damage) * this.fWeaponDamageModifier);
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

        private float _DamageMultiplierModifier;
        public override float DamageMultiplierModifer
        {
            get
            {
                _DamageMultiplierModifier = base.DamageMultiplierModifer;
                _DamageMultiplierModifier += CState.m_Stats.BonusDamageDeathStrike;
                _DamageMultiplierModifier += (this.CState.m_Talents.GlyphofDeathStrike ? Math.Max(.02f * CState.m_CurrentRP, .4f) : 0);
                return _DamageMultiplierModifier;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }
    }
}
