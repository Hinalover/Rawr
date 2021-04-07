using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Feral
{
    /// <summary>
    /// This class will be the base class for abilities.
    /// Each ability will inherit from this class and have to implement their own functions
    /// for the various spells/abilities.
    /// Then we will want to be able to aggregate all the data to have a solid picture of what's
    /// going on when the abilities are used.
    /// </summary>
    abstract public class AbilityFeral_Base
    {
        #region Constants
        public const float MIN_GCD_MS = 1f;
        public const float AVG_GCD_MS = 1.5f;
        public const uint INSTANT = 1;
        public const uint MELEE_RANGE = 5;
        public const uint CRIT_MULTIPLIER = 2;
        #endregion

        /// <summary>
        /// Any Feral ability triggered by this ability.  
        /// Should not be recursive.
        /// This would mean FF when using IT or Glyphed HB.
        /// </summary>
        public AbilityFeral_Base[] TriggeredAbility;

        /// <summary>
        /// Name of the ability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// What is the cost of the ability?
        /// INTs representing the 3 Feral Types: Rage, Energy, and Mana
        /// Use enum (int)FeralCostTypes for placement.
        /// Negative costs mean they grant that item.
        /// </summary>
        public int AbilityIndex;

        /// <summary>
        /// Spell ID of the ability
        /// </summary>
        public uint SpellID;

        /// <summary>
        /// Icon of the Ability
        /// </summary>
        public string SpellIcon;

        /// <summary>
        /// Which Forms the ability is able to be used in
        /// </summary>
        public DruidForm[] druidForm = new DruidForm[EnumHelper.GetCount(typeof(DruidForm))];

        #region Cost Type
        public float[] _AbilityCost = new float[EnumHelper.GetCount(typeof(FeralCostTypes))];

        /// <summary>
        /// How much Cat Energy the Ability requires
        /// </summary>
        public float Energy
        {
            get
            {
                return _AbilityCost[(int)FeralCostTypes.Energy];
            }
            set
            {
                _AbilityCost[(int)FeralCostTypes.Energy] = (float)value;
            }
        }

        /// <summary>
        /// 80% of the energy cost is refunded every time the ability misses
        /// </summary>
        protected void EnergyRefunded()
        {
            Energy = (Energy - ((Energy * 0.2f) * (1 - HitChance))) / HitChance;
        }

        /// <summary>
        /// How much Bear Rage the Ability requires
        /// </summary>
        public float Rage
        {
            get
            {
                return _AbilityCost[(int)FeralCostTypes.Rage];
            }
            set
            {
                _AbilityCost[(int)FeralCostTypes.Rage] = (float)value;
            }
        }

        /// <summary>
        /// How much Caster Mana the Ability requires
        /// </summary>
        public float Mana
        {
            get
            {
                return _AbilityCost[(int)FeralCostTypes.Mana];
            }
            set
            {
                _AbilityCost[(int)FeralCostTypes.Mana] = (float)value;
            }
        }

        /// <summary>
        /// If the Ability is a Cat ability, then does it generate or use up combo points
        /// </summary>
        public float ComboPoint
        {
            get
            {
                return _AbilityCost[(int)FeralCostTypes.ComboPoint];
            }
            set
            {
                float abcost = (float)value;
                if (abcost > 5)
                    abcost = 5;
                _AbilityCost[(int)FeralCostTypes.ComboPoint] = abcost;
            }
        }

        #endregion

        public FeralCombatState CombatState { get; set; }

        public virtual void UpdateCombatState(FeralCombatState CS)
        {
            CombatState = CS;
        }

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
        
        /// <summary>
        /// Some abilitiies require a base % of the weapon damage
        /// </summary>
        private float _WeaponPercentDamage;
        virtual public float BaseWeaponDamageMultiplier
        {
            get
            {
                return _WeaponPercentDamage;
            }
            set
            {
                _WeaponPercentDamage = (float)value;
            }
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        virtual public float BaseDamage
        {
            get
            {
                return (this.MinDamage + this.MaxDamage) / 2;
            }
            set
            {
                // Setup so that we can just set a base damage w/o having to 
                // manually set Min & Max all the time.
                this.MaxDamage = (float)value;
                this.MinDamage = (float)value;
            }
        }

        /// <summary>
        /// Number used to multiply with the Spell Scale Modifier in the BaseCombatRating file to get the BaseDamage
        /// </summary>
        public float BaseSpellScaleModifier { get; set; }
        public uint Range { get; set; }

        /// <summary>
        /// What's the size of the area of a given ability?
        /// The idea is that we want to quantify the area buffing talents.
        /// </summary>
        public uint Area { get; set; }

        /// <summary>
        /// Is this an AOE ability?
        /// </summary>
        public bool AOE { get; set; }

        /// <summary>
        /// If an ability requires the player to be prowling in order to activate the ability
        /// </summary>
        public bool MustBeProwling { get; set; }

        /// <summary>
        /// What type of damage does this ability do?
        /// </summary>
        public ItemDamageType DamageType { get; set; }
        #endregion

        #region Formula Information
        /// <summary>
        /// Attack Power Modifier
        /// </summary>
        virtual public float Formula_AP_Modifier { get; set; }
        /// <summary>
        /// Number of Combo Points used
        /// </summary>
        virtual public float Formula_CP { get; set; }
        /// <summary>
        /// Modifier to the Combo Points that is associated with the Base Damage
        /// </summary>
        virtual public float Formula_CP_Base_Damage_Modifier { get; set; }
        /// <summary>
        /// Modifier to the Combo Points that is associated with the Attack Power
        /// </summary>
        virtual public float Formula_CP_AP_Modifier { get; set; }
        /// <summary>
        /// Amount of Energy used
        /// </summary>
        virtual public float Formula_Energy { get; set; }

        /// <summary>
        /// Function that provides a place where the formula of an ability can be placed
        /// </summary>
        /// <returns>returns the result of the formula</returns>
        virtual public float Formula()
        {
            return BaseDamage;
        }

        /// <summary>
        /// Function that provides a place where the formula of an ability can be placed
        /// </summary>
        /// <value CP>Inputs the number of Combo Points to register</value>
        /// <returns>returns the result of the formula</returns>
        virtual public float Formula(float CP)
        {
            return BaseDamage;
        }

        /// <summary>
        /// Function that provides a place where the formula of an ability can be placed
        /// </summary>
        /// <value CP>Inputs the number of Combo Points to register</value>
        /// <value energy>Inputs the extra energy to register</value>
        /// <returns>returns the result of the formula</returns>
        virtual public float Formula(float CP, float energy)
        {
            return this.Formula(CP) * (1f + (energy / 100f));
        }

        /// <summary>
        /// What's the range of a given ability?
        /// The idea is that we want to quantify the range buffing talents.
        /// </summary>

        #endregion

        #region Time Based Items
        ///////////////////////////////////////////////////////
        // Time based items.  
        // These will all be effected by haste.
        // Haste effects GCD to a max hasted of 1.5 sec to 1 sec.
        /// <summary>
        /// How long does it take to cast in ms?
        /// 1 == instant
        /// </summary>
        public float CastTime
        {
            get
            {
                return Math.Max(INSTANT, (float)_AbilityCost[(int)FeralCostTypes.CastTime]);
            }
            set
            {
                _AbilityCost[(int)FeralCostTypes.CastTime] = (int)value;
            }
        }
        /// <summary>
        /// Cooldown in seconds
        /// Default = 1.5 sec == Global Cooldown
        /// GCD min == 1 sec.
        /// </summary>
        virtual public float Cooldown
        {
            get
            {

                float cd = (float)_AbilityCost[(int)FeralCostTypes.CooldownTime];
                if (CombatState != null)
                {
                    if (this.TriggersGCD)
                        return Math.Max(MIN_GCD_MS, cd);
                }
                return cd;
            }
            set
            {
                _AbilityCost[(int)FeralCostTypes.CooldownTime] = (int)value;
            }
        }

        /// <summary>
        /// Does this ability trigger the GCD?
        /// </summary>
        public bool TriggersGCD { get; set; }
        /// <summary>
        /// Is this Ability a Partial to average out values or left over resources?
        /// </summary>
        public bool Partial { get; set; }
        private float _PartialValue = 1f;
        public float PartialValue
        {
            get
            {
                if (Partial) return _PartialValue;
                else return 1;
            }
            set
            {
                if (value == 1)
                {
                    Partial = false;
                }
                else
                {
                    Partial = true;
                }
                _PartialValue = value;
            }
        }

        /// <summary>
        /// How long does the effect last?
        /// This is in millisecs.
        /// </summary>
        public float Duration
        {
            get
            {
                // Factor in haste:
                float tr = Math.Max(1, (float)_AbilityCost[(int)FeralCostTypes.DurationTime]);
                return Math.Max(INSTANT, tr);
            }
            set
            {
                feralDoT.BaseLength = feralDoT.TotalLength = (float)Math.Max(INSTANT, value);
                _AbilityCost[(int)FeralCostTypes.DurationTime] = (int)Math.Max(INSTANT, value);
            }
        }

        public float _TickRate;
        /// <summary>
        /// How often does the effect proc for?
        /// Tick rate is millisecs.
        /// Ensure that we don't have a 0 value.  
        /// 1 ms == instant.
        /// </summary>
        virtual public float TickRate 
        {
            get
            {
                // Factor in haste:
                float tr = _TickRate;
                    if (CombatState != null && CombatState.Stats != null)
                        tr = (uint)(tr / (1 + CombatState.Stats.PhysicalHaste));
                return Math.Max(INSTANT, tr);
            }
            set
            {
                if (feralDoT == null)
                    feralDoT = new FeralDoT();
                feralDoT.Interval = (float)Math.Max(INSTANT, value);
                _TickRate = Math.Max(INSTANT, value);
            }
        }
        #endregion

        #region Weapon related Items
        /////////////////////////////////////////////////
        // Weapon related items.
        public FeralWeapon MainHand;
        public float WeaponDamageModifier { get; set; }
        public bool useSpellHit { get; set; }
        #endregion

        /// <summary>
        /// The Crit Chance for the ability.  
        /// </summary>
        [Percentage]
        virtual public float CritChance
        {
            get
            {
                    float crit = 0f;
                    if (CombatState != null && CombatState.Stats != null)
                        crit += CombatState.MainHand.CriticalStrike;
                    crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                    return Math.Max(0, Math.Min(1, crit));
            }
        }

        /// <summary>
        /// Chance for the ability to hit the target.  
        /// Includes Expertise
        /// </summary>
        [Percentage]
        virtual public float HitChance
        {
            get
            {
                // Determine Miss Chance
                float ChanceToHit = 1 - CombatState.MainHand.chanceMissed - CombatState.MainHand.chanceDodged; // Ensure that crit is no lower than 0.
                //ChanceToHit -= CombatState.MainHand.chanceDodged;
                return Math.Max(0, Math.Min(1, ChanceToHit));
            }
        }

        /// <summary>
        /// Chance for the ability to Miss the target.  
        /// Hit only
        /// </summary>
        [Percentage]
        virtual public float MissChance
        {
            get
            {
                float MissChance = StatConversion.YELLOW_MISS_CHANCE_CAP[3];
                if (CombatState != null && CombatState.Stats != null)
                {
                    if (useSpellHit == true)
                        MissChance = Math.Max(0, (CombatState.MainHand.chanceMissed + CombatState.MainHand.chanceDodged));
                    else
                        MissChance = Math.Max(0, CombatState.MainHand.chanceMissed);
                }   
                return MissChance;
            }
        }

        #region Damage
        public bool isDoT;
        public FeralDoT feralDoT = new FeralDoT();
        /// <summary>
        /// Get the single instance damage of this ability.
        /// </summary>
        /// <returns>Float that represents a fully buffed single instance of this ability.</returns>
        virtual public float GetTickDamage()
        {
            float _tickDamage = 0;
            // Start w/ getting the base damage values.
            _tickDamage = feralDoT.BaseDamage;
            // Apply modifiers.
            _tickDamage += DamageAdditiveModifer;
            _tickDamage *= DamageMultiplierModifer;
            if (Partial) { _tickDamage *= PartialValue; }
            return _tickDamage;
        }

        virtual public float TotalDamage { get { return GetTotalDamage(); } }
        /// <summary>
        /// Get the full effect over the lifetime of the ability.
        /// </summary>
        /// <returns>float that is TickDamage * duration</returns>
        virtual public float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0f;
            float DamageCount = 1f;
            if (isDoT) {
                feralDoT.BaseDamage = Formula();
                Damage = this.GetTickDamage();
                // Assuming full duration, or standard impact.
                // But I want this in whole numbers.
                // Also need to decide if I want this to be whole ticks, or if partial ticks will be allowed.
                DamageCount = feralDoT.TotalTickCount();
            }
            else {
                Damage = ((Formula() + DamageAdditiveModifer) * DamageMultiplierModifer);
                if (Partial)
                    Damage *= PartialValue;
            }

            if (AOE == true) {
                // Need to ensure this value is reasonable for all abilities.
                DamageCount *= Math.Max(1, this.CombatState.NumberOfTargets);
            }

            Damage *= DamageCount;
            float CritDamage = (CombatState.MainHand.CritDamageMultiplier * Damage) * CritChance;
            Damage = (Damage * (HitChance - CritChance)) + CritDamage;

            return Damage;
        }

        public float DPS { get { return GetDPS(); } }
        virtual public float GetDPS()
        {
            float sub = MIN_GCD_MS;
            if (CombatState != null && TriggersGCD)
            {
                sub = MIN_GCD_MS;
            }
            sub = Math.Max(sub, (Duration + CastTime));
            float dps = (float)(TotalDamage / sub);
            return dps;
        }

        public float TPS { get { return GetTPS(); } }
        virtual public float GetTPS()
        {
            float sub = MIN_GCD_MS;
            if (CombatState != null && TriggersGCD)
            {
                sub = MIN_GCD_MS;
            }
            sub = Math.Max(sub, (Duration + CastTime));
            float dps = (float)(TotalThreat / sub);
            return dps;
        }

        private int _DamageAdditiveModifer;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        virtual public int DamageAdditiveModifer
        {
            get
            {
                return _DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        private float _DamageMultiplierModifer;
        /// <summary>
        /// 0 indexed multiplier.
        /// Setup the modifier formula for a given ability.
        /// </summary>
        virtual public float DamageMultiplierModifer
        {
            get
            {
                float DMM = 0;
                if (CombatState != null && CombatState.Stats != null)
                {
                    switch (DamageType)
                    {
                        case ItemDamageType.Arcane:
                            DMM += CombatState.Stats.BonusArcaneDamageMultiplier;
                            break;
                        case ItemDamageType.Fire:
                            DMM += CombatState.Stats.BonusFireDamageMultiplier;
                            break;
                        case ItemDamageType.Frost:
                            DMM += CombatState.Stats.BonusFrostDamageMultiplier;
                            break;
                        case ItemDamageType.Holy:
                            DMM += CombatState.Stats.BonusHolyDamageMultiplier;
                            break;
                        case ItemDamageType.Nature:
                            DMM += CombatState.Stats.BonusNatureDamageMultiplier;
                            break;
                        case ItemDamageType.Physical:
                            DMM += CombatState.Stats.BonusPhysicalDamageMultiplier;
                            DMM -= Math.Max(0f, StatConversion.GetArmorDamageReduction(85, CombatState.BossArmor, 0, 0));
                            break;
                        case ItemDamageType.Shadow:
                            DMM += CombatState.Stats.BonusShadowDamageMultiplier;
                            break;
                    }
                }
                return _DamageMultiplierModifer + DMM;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }
        #endregion

        #region Threat
        /// <summary>How much to multiply the damage by to generate threat.</summary>
        virtual public float ThreatMultiplier { get { return 1f; } }

        private float _ThreatAdditiveModifier;
        /// <summary>
        /// Get the full effect of threat over the lifetime of the ability.
        /// </summary>
        /// <returns>float that is (GetTotalDamage * ThreatModifiers) * Threat For Rage Presence</returns>
        public float GetTotalThreat() { return TotalThreat; }
        public float TotalThreat
        {
            get
            {
                float Threat = StatConversion.ApplyMultiplier(GetTotalDamage(), ThreatMultiplier) + _ThreatAdditiveModifier;
                if (null != CombatState && null != CombatState.Stats)
                {
                    Threat = StatConversion.ApplyMultiplier(Threat, CombatState.Stats.ThreatIncreaseMultiplier);
                    Threat = StatConversion.ApplyInverseMultiplier(Threat, CombatState.Stats.ThreatReductionMultiplier);
                }
                return Threat;
            }
            set { _ThreatAdditiveModifier = value; }
        }
        #endregion

        #region Comparisons
        /// <summary>
        /// Compare the output of an ability either Threat or Damage.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t">Using ComboPoint Runes will be total Rune Cost.</param>
        /// <param name="bThreat">True if Threat, False if Damage</param>
        /// <returns></returns>
        public static int CompareXPerCost(AbilityFeral_Base a, AbilityFeral_Base b, FeralCostTypes t, bool bThreat)
        {
            int ic = 0;
            float aCostType = 1;
            float bCostType = 1;

            // Sum of cost:
            switch (t)
            {
                case FeralCostTypes.Energy:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Energy];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Energy];
                    break;
                case FeralCostTypes.Rage:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Rage];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Rage];
                    break;
                case FeralCostTypes.Mana:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Mana];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Mana];
                    break;
                case FeralCostTypes.CastTime:
                    aCostType = a.CastTime;
                    bCostType = b.CastTime;
                    break;
                case FeralCostTypes.CooldownTime:
                    aCostType = a.Cooldown;
                    bCostType = b.Cooldown;
                    break;
                case FeralCostTypes.DurationTime:
                    aCostType = a.Duration;
                    foreach (AbilityFeral_Base TriggerByA in a.TriggeredAbility)
                    {
                        aCostType += TriggerByA.Duration;
                    }
                    bCostType = b.Duration;
                    foreach (AbilityFeral_Base TriggerByB in b.TriggeredAbility)
                    {
                        bCostType += TriggerByB.Duration;
                    }
                    break;
                case FeralCostTypes.ComboPoint:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.ComboPoint];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.ComboPoint];
                    break;
                default:
                    aCostType = 1;
                    bCostType = 1;
                    break;

            }
            if (aCostType != 0 || bCostType != 0)
            {
                if (aCostType != 0 && bCostType != 0)
                {
                    float avalue = 0;
                    float bvalue = 0;
                    if (bThreat)
                    {
                        // Let's expand this to include triggered values as well.
                        avalue = a.GetTotalThreat();
                        if (a.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in a.TriggeredAbility)
                            {
                                avalue += TriggerByA.GetTotalThreat();
                            }
                        }
                        avalue /= aCostType;
                        bvalue = b.GetTotalThreat();
                        if (b.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in b.TriggeredAbility)
                            {
                                bvalue += TriggerByA.GetTotalThreat();
                            }
                        }
                        bvalue /= bCostType;
                    }
                    else
                    {
                        // Let's expand this to include triggered values as well.
                        avalue = a.TotalDamage;
                        if (a.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in a.TriggeredAbility)
                            {
                                avalue += TriggerByA.TotalDamage;
                            }
                        }
                        avalue /= aCostType;
                        bvalue = b.TotalDamage;
                        if (b.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in b.TriggeredAbility)
                            {
                                bvalue += TriggerByA.TotalDamage;
                            }
                        }
                        bvalue /= bCostType;
                    }
                    if (avalue != bvalue)
                    {
                        // This is setup where we want a descending order.
                        if (avalue > bvalue)
                            ic = -1;
                        else
                            ic = 1;
                    }
                }
                else // one of them are 0
                {
                    if (aCostType > bCostType)
                        ic = -1;
                    else
                        ic = 1;
                }
            }
            return ic;
        }
        public static int CompareValueDPSPerPower(AbilityFeral_Base a, AbilityFeral_Base b, FeralCostTypes t, bool bThreat)
        {
            int ic = 0;
            float aCostType = 1;
            float bCostType = 1;

            // Sum of cost:
            switch (t)
            {
                case FeralCostTypes.Energy:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Energy];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Energy];
                    break;
                case FeralCostTypes.Rage:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Rage];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Rage];
                    break;
                case FeralCostTypes.Mana:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.Mana];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.Mana];
                    break;
                case FeralCostTypes.ComboPoint:
                    aCostType = a._AbilityCost[(int)FeralCostTypes.ComboPoint];
                    bCostType = b._AbilityCost[(int)FeralCostTypes.ComboPoint];
                    break;
                case FeralCostTypes.CastTime:
                case FeralCostTypes.CooldownTime:
                case FeralCostTypes.DurationTime:
                default:
                    aCostType = 1;
                    bCostType = 1;
                    break;

            }
            if (aCostType != 0 || bCostType != 0)
            {
                if (aCostType != 0 && bCostType != 0)
                {
                    float avalue = 0;
                    float bvalue = 0;
                    if (bThreat)
                    {
                        // Let's expand this to include triggered values as well.
                        avalue = a.GetTPS();
                        if (a.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in a.TriggeredAbility)
                            {
                                avalue += TriggerByA.GetTPS();
                            }
                        }
                        avalue /= aCostType;
                        bvalue = b.GetTPS();
                        if (b.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in b.TriggeredAbility)
                            {
                                bvalue += TriggerByA.GetTPS();
                            }
                        }
                        bvalue /= bCostType;
                    }
                    else
                    {
                        // Let's expand this to include triggered values as well.
                        avalue = a.GetDPS();
                        if (a.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in a.TriggeredAbility)
                            {
                                avalue += TriggerByA.GetDPS();
                            }
                        }
                        avalue /= aCostType;
                        bvalue = b.GetDPS();
                        if (b.TriggeredAbility != null)
                        {
                            foreach (AbilityFeral_Base TriggerByA in b.TriggeredAbility)
                            {
                                bvalue += TriggerByA.GetDPS();
                            }
                        }
                        bvalue /= bCostType;
                    }
                    if (avalue != bvalue)
                    {
                        // This is setup where we want a descending order.
                        if (avalue > bvalue)
                            ic = -1;
                        else
                            ic = 1;
                    }
                }
                else // one of them are 0
                {
                    if (aCostType > bCostType)
                        ic = -1;
                    else
                        ic = 1;
                }
            }
            return ic;
        }

        #region Damage
        public static int CompareByTotalDamage(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.None, false);
        }

        public static int CompareDamageByCooldown(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.CooldownTime, false);
        }

        #region Compare Energy
        public static int CompareDamageByEnergy(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.Energy, false);
        }

        public static int CompareDPSByEnergy(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareValueDPSPerPower(a, b, FeralCostTypes.Energy, false);
        }
        #endregion

        #region Compare Rage
        public static int CompareDamageByRage(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.Energy, false);
        }

        public static int CompareDPSByRage(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareValueDPSPerPower(a, b, FeralCostTypes.Energy, false);
        }
        #endregion

        public static int CompareDamageByCP(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.ComboPoint, false);
        }
        #endregion

        #region Threat
        public static int CompareByTotalThreat(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.None, true);
        }

        public static int CompareThreatByCooldown(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.CooldownTime, true);
        }

        public static int CompareThreatByEnergy(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.Energy, true);
        }
        public static int CompareTPSByEnergy(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareValueDPSPerPower(a, b, FeralCostTypes.Energy, true);
        }

        public static int CompareThreatByRage(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.Rage, true);
        }
        public static int CompareTPSByRage(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareValueDPSPerPower(a, b, FeralCostTypes.Rage, true);
        }
        
        public static int CompareThreatByCP(AbilityFeral_Base a, AbilityFeral_Base b)
        {
            return CompareXPerCost(a, b, FeralCostTypes.ComboPoint, true);
        }
        #endregion
        #endregion

        public override string ToString()
        {
            return string.Format("{0}: {1} {2}", Name, TotalDamage, DamageType.ToString());
        }
    }

    public class FeralDoT
    {
        public float BaseDamage;
        public float BaseLength;
        public float TotalLength;
        public float Interval;

        public FeralDoT()
        {
            BaseDamage = 0f;
            BaseLength = 0f;
            TotalLength = 0f;
            Interval = 0f;
        }

        public FeralDoT(float damage)
        {
            BaseDamage = damage;
            BaseLength = 0f;
            TotalLength = 0f;
            Interval = 0f;
        }

        public FeralDoT (float damage, float baseLength, float totalLength, float interval)
        {
            BaseDamage = damage;
            BaseLength = baseLength;
            TotalLength = totalLength;
            Interval = interval;
        }

        public float BaseTickCount()
        {
            if ((BaseLength > 0) && (Interval > 0))
                return BaseLength / Interval;
            else 
                return 1f;
        }

        public float TotalTickCount()
        {
            if ((TotalLength > 0) && (Interval > 0))
                return TotalLength / Interval;
            else
                return 1f;
        }

        public float TickSize()
        {
            if (BaseLength > 0)
                return BaseDamage / BaseLength;
            else
                return BaseDamage;
        }

        public float TotalDamage()
        {
            return TickSize() * TotalTickCount();
        }
    }
}