using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Rake : AbilityFeral_Base
    {
        private float baseEnergy = 35;
        private float baseCP = 1;
        /// <summary>
        /// Rake the target for AP*0.147+56 Bleed damage and an additional (56 * 3 + AP * 0.441) 
        /// Bleed damage over 9 sec.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_Rake()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_Rake(FeralCombatState CState)
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
            Name = "Rake";
            SpellID = 1822;
            SpellIcon = "ability_druid_disembowel";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = baseEnergy;
            ComboPoint = baseCP;

            DamageType = ItemDamageType.Physical;
            BaseSpellScaleModifier = 0.0900000036f;
            BaseDamage = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            Formula_AP_Modifier = 0.3000000f;

            isDoT = true;
            feralDoT.Interval = 3f;
            feralDoT.BaseLength = 15f; //9f;
            Duration = feralDoT.TotalLength = feralDoT.BaseLength; /*+ ((CombatState.Talents.EndlessCarnage > 0) ? (CombatState.Talents.EndlessCarnage * 3f * 1000f) : 0);*/

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Rake;
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
                                             * (1 + CombatState.MainHand.Stats.BonusBleedDamageMultiplier);
                }
                return _DamageMultiplierModifer;
            }
        }

        /// <summary>
        /// Applies the initial damage of Rake.
        /// Originally set to a different coefficient than the tick damage; changed in Patch 4.2.
        /// Keeping these separated just in case
        /// </summary>
        /// <returns>Damage of the inital application of the attack</returns>
        private float Formula_Initial_Damage()
        {
            return (float)Math.Floor(((CombatState.MainHand.AttackPower * Formula_AP_Modifier) * (1 + CombatState.MainHand.Mastery)) + BaseDamage);
        }

        public override float Formula()
        {
            return (float)Math.Floor(((CombatState.MainHand.AttackPower * Formula_AP_Modifier) * (1 + CombatState.MainHand.Mastery)) + BaseDamage) * (1 + (CombatState.MainHand.Stats.Tier_11_2pc ? 0.10f : 0f));
        }

        public override float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0f;
            float DamageCount = 1f;
            feralDoT.BaseDamage = this.Formula();
            Damage = this.GetTickDamage();
            DamageCount = feralDoT.TotalTickCount();
            
            // Multiply by the number of ticks
            Damage *= DamageCount;
            
            // Add the initial Damage from the attack
            Damage += ((Formula_Initial_Damage() + DamageAdditiveModifer) * DamageMultiplierModifer);

            // Calculate the crit damage
            float CritDamage = 2 * Damage * CritChance;
            Damage = (Damage * (HitChance - CritChance)) + CritDamage;

            return Damage;
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage / Energy;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string initalDamage = string.Format("Initial Hit: {0}", (Formula_Initial_Damage() * DamageMultiplierModifer).ToString("n")); 
            string initialCrit = string.Format("Initial Crit: {0}", (Formula_Initial_Damage() * DamageMultiplierModifer * critDamMult).ToString("n"));
            string dotDamage = string.Format("DoT Damage: {0} - {1}", (Formula() * DamageMultiplierModifer * (Duration / feralDoT.Interval)).ToString("n"),
                (Formula() * DamageMultiplierModifer * critDamMult * (Duration / feralDoT.Interval)).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}", TotalDamage.ToString("n"), sDPE, initalDamage, initialCrit, dotDamage, Avoidance, Crit);
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
