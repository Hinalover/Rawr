using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.DK
{
    public enum StatType { Unbuffed, Buffed, Average, Maximum };
    
    public enum RotationType
    {
        Default,
        MasterFrost,
    }

    public class StatsDK : Stats
    {
        public readonly float BaseMastery = 8;
        public float Mastery { 
            get { return BaseMastery + StatConversion.GetMasteryFromRating(MasteryRating); }
//            set {_Mastery = value;} 
        }

        private float _BaseBonusFrostDamageMultiplierFromMastery = 0;
        public float BonusFrostDamageMultiplierFromMastery
        {
            get { return _BaseBonusFrostDamageMultiplierFromMastery * Mastery; }
            set { _BaseBonusFrostDamageMultiplierFromMastery = value; }
        }
        private float _BonusShadowDamageMultiplierFromMastery = 0;
        public float BonusShadowDamageMultiplierFromMastery
        {
            get { return _BonusShadowDamageMultiplierFromMastery * Mastery; }
            set { _BonusShadowDamageMultiplierFromMastery = value; }
        }

        /// <summary>
        /// Effective value AP of just Vengence.
        /// TotalAP == AP + VengenceAttackPower
        /// </summary>
        public float VengenceAttackPower { get; set; }
        /// <summary>
        /// Effective parry - will be 0 if unarmed.
        /// </summary>
        public float EffectiveParry { get; set; }
        /// <summary>
        /// Increased RP gained from ability usage
        /// Percentage
        /// </summary>
        [Percentage]
        public float BonusRPMultiplier { get; set; }
        /// <summary>
        /// Increased Rune regen rate.  
        /// Percentage - use like haste.
        /// </summary>
        [Percentage]
        public float BonusRuneRegeneration { get; set; }
        /// <summary>
        /// Increased raw max RP.
        /// </summary>
        public float BonusMaxRunicPower { get; set; }
        
        public bool b2T11_Tank { get; set; }
        public bool b4T11_Tank { get; set; }
        public bool b2T11_DPS { get; set; }
        public bool b4T11_DPS { get; set; }

        public bool b2T12_Tank { get; set; }
        public bool b4T12_Tank { get; set; }
        public bool b2T12_DPS { get; set; }
        public bool b4T12_DPS { get; set; }

        public bool b2T13_Tank { get; set; }
        public bool b4T13_Tank { get; set; }
        public bool b2T13_DPS { get; set; }
        public bool b4T13_DPS { get; set; }

        public bool bDW { get; set; }
    }
}
