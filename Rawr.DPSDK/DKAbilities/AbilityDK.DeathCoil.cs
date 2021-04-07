using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Coil Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_DeathCoil : AbilityDK_Base
    {
        public AbilityDK_DeathCoil(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Death Coil";
            this.uBaseDamage = 985;
            this.bWeaponRequired = false;
            this.bTriggersGCD = true;
            this.uRange = 30;
            this.tDamageType = ItemDamageType.Shadow;
            this.AbilityIndex = (int)DKability.DeathCoil;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            float RPCost = 40 - (CS.m_Talents.RunicCorruption * 3);
            //RPCost /= (1f + (.05f * (float)CS.m_Talents.SuddenDoom));  
            // Moving SD procs to be extra DCs in the rotation rather than reduced cost.
            // This makes it more like Rime.
            this.AbilityCost[(int)DKCostTypes.RunicPower] = (int)Math.Floor(RPCost);
        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                //this.DamageAdditiveModifer = [AP * 0.3]
                return (int)(this.CState.m_Stats.AttackPower * .3) + this._DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        override public float DamageMultiplierModifer
        {
            get
            {
                float DMM = base.DamageMultiplierModifer;
                if (CState.m_Talents.GlyphofDeathCoil)
                    DMM += .15f;
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        public override float CritChance
        {
            get
            {
                return Math.Min(1, base.CritChance + CState.m_Stats.BonusCritChanceDeathCoil);
            }
        }
    }
}
