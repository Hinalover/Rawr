using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    /// <summary>
    /// This class will be the base class for abilities.
    /// Each ability will inherit from this class and have to implement their own functions
    /// for the various spells/abilities.
    /// </summary>
    public abstract class AbilityEnhance_Base
    {
        #region Constants
        public const float MIN_GCD_S = 1.5f;
        public const float MIN_SPELL_GCD_S = 1f;
        public const float INSTANT = 0f;
        public const float MELEE_RANGE = 5f;
        #endregion

        public int AbilityIndex;

        /// <summary>
        /// Name of the ability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Spell ID of the ability
        /// </summary>
        public int SpellID;

        /// <summary>
        /// Icon of the Ability
        /// </summary>
        public string SpellIcon;

        /// <summary>
        /// Does the Ability require a talent?
        /// </summary>
        public bool ReqTalent { get; set; }

        /// <summary>
        /// What talent does the Ability require
        /// </summary>
        public int Talent { get; set; }

        /// <summary>
        /// Does the Ability require a MHWeapon?
        /// </summary>
        public bool ReqMHWeapon { get; set; }

        /// <summary>
        /// Does the Ability require an OHWeapon?
        /// </summary>
        public bool ReqOHWeapon { get; set; }

        /// <summary>
        /// Does the Ability swing the OHWeapon?
        /// </summary>
        public bool SwingsOHWeapon { get; set; }

        /// <summary>
        /// Is the Ability a Spell?
        /// If True, uses spell combat table and spell GCD
        /// if False, uses yellow/white combat table and normal GCD
        /// </summary>
        public bool IsSpell { get; set; }

        /// <summary>
        /// What's the range of a given ability?
        /// The idea is that we want to quantify the range buffing talents.
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// What's the size of the area of a given ability?
        /// The idea is that we want to quantify the area buffing talents.
        /// </summary>
        public float Area { get; set; }

        /// <summary>
        /// Is this an AOE ability?
        /// </summary>
        public bool AOE { get; set; }

        /// <summary>
        /// The maximum number of targets the Ability would normally hit without any other bonues
        /// </summary>
        public float Targets { get; set; }

        /// <summary>
        /// The mana cost of the Ability as a % of Base Mana
        /// </summary>
        public float Mana { get; set; }

        /// <summary>
        /// Any ability triggered by this ability.  
        /// Should not be recursive.
        /// </summary>
        public AbilityEnhance_Base[] TriggeredAbility;

        #region Time Based Items
        ///////////////////////////////////////////////////////
        // Time based items.  
        // These will all be effected by haste.
        // Haste effects Spell GCD to a max hasted of 1.5 sec to 1 sec.
        ///////////////////////////////////////////////////////

        /// <summary>
        /// Does this ability trigger the GCD?
        /// </summary>
        public bool TriggersGCD { get; set; }

        private float _CastTime = -1f;
        /// <summary>
        /// How long does it take to cast in seconds?
        /// 0 == INSTANT
        /// </summary>
        public float CastTime
        {
            get
            {
                return Math.Max(INSTANT, _CastTime);
            }
            set
            {
                _CastTime = value;
            }
        }

        private float _Cooldown = 1.5f;
        /// <summary>
        /// Cooldown in seconds
        /// </summary>
        public float Cooldown
        {
            get
            {
                float cd = _Cooldown;
                if (this.TriggersGCD)
                {
                    if (this.IsSpell)
                    {
                        return Math.Max(MIN_SPELL_GCD_S, cd);
                    }
                    else
                    {
                        return Math.Max(MIN_GCD_S, cd);
                    }
                }
                return cd;
            }
            set
            {
                _Cooldown = (int)value;
            }
        }

        private float _Duration = 0f;
        /// <summary>
        /// How long does the effect last in seconds?
        /// </summary>
        public float Duration
        {
            get
            {
                return Math.Max(INSTANT, _Duration);
            }
            set
            {
                _Duration = value;
            }
        }
        #endregion

        #region Attack Table


        /// <summary>
        /// Percentage Based Crit Chance Bonus (0.5 = 50% Crit Chance, capped between 0%-100%, factoring Boss Level Offsets)
        /// </summary>
        public virtual float BonusCritChance { get; set; }

        /// <summary>
        /// Percentage Based Crit Damage Bonus (1.5 = 150% damage)
        /// </summary>
        public float BonusCritDamage { get; set; }
        #endregion

        #region Base Damage Info
        /// <summary>
        /// What min damage does the ability cause?
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public float MinDamage { get; set; }

        /// <summary>
        /// What max damage does the ability cause?
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public float MaxDamage { get; set; }

        private float _WeaponPercentDamage;
        /// <summary>
        /// Some abilitiies require a base % of the weapon damage
        /// </summary>
        public virtual float BaseWeaponDamageMultiplier
        {
            get
            {
                return _WeaponPercentDamage;
            }
            set
            {
                _WeaponPercentDamage = value;
            }
        }

        private float _SpellPowerPercent;
        /// <summary>
        /// Some abilitiies require a  % of spellpower
        /// </summary>
        public virtual float BaseSpellPowerMultiplier
        {
            get
            {
                return _SpellPowerPercent;
            }
            set
            {
                _SpellPowerPercent = value;
            }
        }

        private float _AttackPowerPercent;
        /// <summary>
        /// Some abilitiies require a  % of spellpower
        /// </summary>
        public virtual float BaseAttackPowerMultiplier
        {
            get
            {
                return _AttackPowerPercent;
            }
            set
            {
                _AttackPowerPercent = value;
            }
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public virtual float BaseDamage
        {
            get
            {
                return (this.MinDamage + this.MaxDamage) / 2;
            }
            set
            {
                // Setup so that we can just set a base damage w/o having to 
                // manually set Min & Max all the time.
                this.MaxDamage = value;
                this.MinDamage = value;
            }
        }

        /// <summary>
        /// What type of damage does this ability do?
        /// </summary>
        public ItemDamageType DamageType { get; set; }
        #endregion

        #region Damage
        /// <summary>
        /// Function that provides a place where the formula of an ability can be placed
        /// </summary>
        /// <returns>returns the result of the formula</returns>
        public virtual float Formula()
        {
            return BaseDamage;
        }

        public virtual float Damage
        {
            get
            {
                return Math.Max(0f, (BaseDamage + DamageAdditiveModifier) * DamageMultiplierModifier);
            }
        }
        public virtual float DamageOnUse
        {
            get
            {
                float dmg = Damage;

                return dmg;
            }
        }

        /// <summary>
        /// Gets the DPS of the ability
        /// </summary>
        public virtual float GetDPS()
        {
            float sub = MIN_GCD_S;
            if (this.TriggersGCD)
            {
                if (this.IsSpell)
                {
                    sub = MIN_SPELL_GCD_S;
                }
                else
                {
                    sub = MIN_GCD_S;
                }
            }
            sub = Math.Max(sub, (Duration + CastTime));
            float dps = (float)(DamageOnUse / sub);
            return dps;
        }

        private float _DamageAdditiveModifier;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public virtual float DamageAdditiveModifier
        {
            get
            {
                return _DamageAdditiveModifier;
            }
            set
            {
                _DamageAdditiveModifier = value;
            }
        }

        private float _DamageMultiplierModifier;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public virtual float DamageMultiplierModifier
        {
            get
            {
                return _DamageMultiplierModifier;
            }
            set
            {
                _DamageMultiplierModifier = value;
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}: {1} {2}", Name, DamageOnUse, DamageType.ToString());
        }
    }

    public class AbilityEnhance_DoT : AbilityEnhance_Base
    {
        public AbilityEnhance_DoT() { }

        public virtual float InitialDamage { get { return 0f; } }
        public virtual float TickSize { get { return 0f; } }
        public virtual float TotalLength { get { return Duration; } }
        public virtual float TickInterval { get { return 0f; } }
        public virtual float NumTicks { get { return TotalLength / TickInterval; } }

        public virtual float GetDmgOverTickingTime() { return TickSize * NumTicks; }
        public override float GetDPS()
        {
            return GetDmgOverTickingTime() / TotalLength;
        }
    }

    public enum EnhanceAbility2
    {
        Melee,
        EarthElemental,
        EarthShock,
        ElementalBlast,
        FeralSpirit,
        FireElemental,
        FlameShock,
        Flametongue,
        LavaLash,
        LightningBolt,
        LightningShield,
        SearingTotem,
        Stormblast,
        Stormstrike,
        UnleashElements,
        Windfury,
        Windlash,
        OtherPhysical,
        OtherHoly,
        OtherArcane,
        OtherFire,
        OtherFrost,
        OtherNature,
        OtherShadow,
    }
}
