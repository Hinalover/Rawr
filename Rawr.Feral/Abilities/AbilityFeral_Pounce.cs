using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Pounce : AbilityFeral_Base
    {
        private float baseEnergy = 50;
        private float baseCP = 1;
        /// <summary>
        /// Pounce, stunning the target for 3 sec and causing 2340 Bleed damage over 18 sec.  
        /// Must be prowling.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_Pounce()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_Pounce(FeralCombatState CState)
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
            Name = "Pounce";
            SpellID = 9005;
            SpellIcon = "ability_druid_supriseattack";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = baseEnergy;
            ComboPoint = baseCP;

            DamageType = ItemDamageType.Physical;
            Formula_AP_Modifier = 0.323f;
            BaseSpellScaleModifier = 0.6970000267f;
            BaseDamage = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);

            isDoT = true;
            feralDoT.Interval = 3f;
            feralDoT.BaseLength = 18f;
            Duration = feralDoT.TotalLength = feralDoT.BaseLength;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Pounce;
            Range = MELEE_RANGE;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            Energy = baseEnergy;
            EnergyRefunded();
            Energy *= (CombatState.BerserkUptime > 0 ? (1 - (AbilityFeral_Berserk.CostReduction * CombatState.BerserkUptime)) : 1f);
            ComboPoint = baseCP;
            ComboPoint += CombatState.MainHand.CriticalStrike;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public override float DamageMultiplierModifer
        {
            get
            {
                if (_DamageMultiplierModifer == 0)
                {
                    _DamageMultiplierModifer = (1 + CombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.MainHand.Stats.TargetArmorReduction, CombatState.MainHand.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime > 0 ? AbilityFeral_TigersFury.DamageBonus * CombatState.TigersFuryUptime : 0f))
                                             * (1 + (CombatState.SavageRoarUptime > 0 ? AbilityFeral_SavageRoar.DamageBonus * CombatState.SavageRoarUptime : 0f))
                                             * (1 + CombatState.MainHand.Mastery); // Mastery Multiplier  // Tier 11 2-piece Rake tick bonus
                }
                return _DamageMultiplierModifer;
            }
        }

        public override float Formula()
        {
            return (float)Math.Floor((CombatState.MainHand.AttackPower * Formula_AP_Modifier) + BaseDamage);
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage / Energy;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string dotDamage = string.Format("DoT Damage: {0} - {1}", (Formula() * DamageMultiplierModifer * (Duration / feralDoT.Interval)).ToString("n"),
                (Formula() * DamageMultiplierModifer * critDamMult * (Duration / feralDoT.Interval)).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}", TotalDamage.ToString("n"), sDPE, dotDamage, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance)) * (Duration / feralDoT.Interval);
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance) * (Duration / feralDoT.Interval);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;

            string rakeMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string rakeCount = string.Format("     Applications(# {0})\n", count.ToString("n"));
            string rakeHit = string.Format("     Tick Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string rakeCrit = string.Format("     Tick Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return rakeMain + rakeCount + rakeHit + rakeCrit;
        }
    }
}
