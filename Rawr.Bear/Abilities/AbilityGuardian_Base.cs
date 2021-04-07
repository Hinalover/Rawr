using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    abstract public class AbilityGuardian_Base : AbilityFeral_Base
    {
        public GuardianCombatState guardianCombatState { get; set; }

        public virtual void UpdateCombatState(GuardianCombatState CS)
        {
            guardianCombatState = CS;
        }

        /// <summary>
        /// Cooldown in seconds
        /// Default = 1.5 sec == Global Cooldown
        /// GCD min == 1 sec.
        /// </summary>
        override public float Cooldown
        {
            get
            {

                float cd = (float)_AbilityCost[(int)FeralCostTypes.CooldownTime];
                if (guardianCombatState != null)
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
        /// How often does the effect proc for?
        /// Tick rate is millisecs.
        /// Ensure that we don't have a 0 value.  
        /// 1 ms == instant.
        /// </summary>
        override public float TickRate
        {
            get
            {
                // Factor in haste:
                float tr = _TickRate;
                if (CombatState != null && guardianCombatState.Stats != null)
                    tr = (uint)(tr / (1 + guardianCombatState.Stats.PhysicalHaste));
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

        /// <summary>
        /// The Crit Chance for the ability.  
        /// </summary>
        [Percentage]
        override public float CritChance
        {
            get
            {
                float crit = 0f;
                if (guardianCombatState != null && guardianCombatState.Stats != null)
                    crit += guardianCombatState.MainHand.CriticalStrike;
                crit += (float)Math.Min(0, (guardianCombatState.TargetLevel - guardianCombatState.CharacterLevel) * -0.01f);
                return Math.Max(0, Math.Min(1, crit));
            }
        }

        /// <summary>
        /// Chance for the ability to hit the target.  
        /// Includes Expertise
        /// </summary>
        [Percentage]
        override public float HitChance
        {
            get
            {
                // Determine Miss Chance
                float ChanceToHit = 1;
                if (useSpellHit)
                    ChanceToHit = 1 - guardianCombatState.MainHand.chanceMissed - guardianCombatState.MainHand.chanceDodged;
                else
                    ChanceToHit = 1 - guardianCombatState.MainHand.chanceMissed - guardianCombatState.MainHand.chanceDodged - guardianCombatState.MainHand.chanceParried; // Ensure that crit is no lower than 0.
                //ChanceToHit -= CombatState.MainHand.chanceDodged;
                return Math.Max(0, Math.Min(1, ChanceToHit));
            }
        }

        /// <summary>
        /// Chance for the ability to Miss the target.  
        /// Hit only
        /// </summary>
        [Percentage]
        override public float MissChance
        {
            get
            {
                float MissChance = StatConversion.YELLOW_MISS_CHANCE_CAP[3];
                if (guardianCombatState != null && guardianCombatState.Stats != null)
                {
                    if (useSpellHit == true)
                        MissChance = Math.Max(0, (guardianCombatState.MainHand.chanceMissed + guardianCombatState.MainHand.chanceDodged));
                    else
                        MissChance = Math.Max(0, guardianCombatState.MainHand.chanceMissed);
                }
                return MissChance;
            }
        }

        /// <summary>
        /// Get the full effect over the lifetime of the ability.
        /// </summary>
        /// <returns>float that is TickDamage * duration</returns>
        override public float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0f;
            float DamageCount = 1f;
            if (isDoT)
            {
                feralDoT.BaseDamage = Formula();
                Damage = this.GetTickDamage();
                // Assuming full duration, or standard impact.
                // But I want this in whole numbers.
                // Also need to decide if I want this to be whole ticks, or if partial ticks will be allowed.
                DamageCount = feralDoT.TotalTickCount();
            }
            else
            {
                Damage = ((Formula() + DamageAdditiveModifer) * DamageMultiplierModifer);
                if (Partial)
                    Damage *= PartialValue;
            }

            if (AOE == true)
            {
                // Need to ensure this value is reasonable for all abilities.
                DamageCount *= Math.Max(1, this.guardianCombatState.NumberOfTargets);
            }

            Damage *= DamageCount;
            float CritDamage = (guardianCombatState.MainHand.CritDamageMultiplier * Damage) * CritChance;
            Damage = (Damage * (HitChance - CritChance)) + CritDamage;

            return Damage;
        }

        public override float ThreatMultiplier
        {
            get
            {
                return 5f;
            }
        }
    }
}
