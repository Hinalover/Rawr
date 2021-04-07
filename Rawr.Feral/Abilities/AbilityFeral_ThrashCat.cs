
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_ThrashCat : AbilityFeral_Base
    {
        private float baseEnergy = 50;
        /// <summary>
        /// Strikes all enemy targets within 8 yards, dealing ${AP*0.0982+1035} to ${AP*0.0982+1277} damage and 
        /// an additional ${(AP*0.167+645)} damage every 3 sec for the next 15 sec, and also causing the 
        /// Weakened Blows effect.
        /// </summary>
        public AbilityFeral_ThrashCat()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_ThrashCat(FeralCombatState CState)
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
            Name = "Thrash (Cat Form)";
            SpellID = 106830;
            SpellIcon = "spell_druid_thrash";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = baseEnergy;

            DamageType = ItemDamageType.Physical;
            BaseSpellScaleModifier = 1.1250000000f;
            float delta = 0.2099999934f;
            MinDamage = (BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel)) * (1 - (delta / 2f));
            MaxDamage = (BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel)) * (1 + (delta / 2f));

            isDoT = true;
            feralDoT.Interval = 3f;
            feralDoT.BaseLength = 15f;
            feralDoT.TotalLength = feralDoT.BaseLength;
            Duration = feralDoT.TotalLength;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.ThrashCatForm;
            Range = 8;
            AOE = true;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            Energy = baseEnergy;
            Formula_AP_Modifier = (CState.PTR ? 0.191f : 0.203f);
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
        /// Applies the initial damage of Thrash.
        /// </summary>
        /// <returns>Damage of the inital application of the attack</returns>
        private float Formula_Initial_Damage()
        {
            return ((Formula_Initial_Damage(MinDamage) + Formula_Initial_Damage(MaxDamage)) / 2f) * (1 + CombatState.MainHand.Mastery);
        }

        private float Formula_Initial_Damage(float damage)
        {
            return (float)Math.Floor(((CombatState.MainHand.AttackPower * Formula_AP_Modifier) * (1 + CombatState.MainHand.Mastery)) + damage);
        }

        /// <summary>
        /// Applies the forumla for the dot port
        /// </summary>
        /// <returns></returns>
        public override float Formula()
        {
            float dotSpellScaleModifier = 0.6269999743f;
            float dotBaseDamage = dotSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            float dot_AP_Modifier = (CombatState.PTR ? 0.141f : 0.0936f);
            // 
            return (dotBaseDamage + ((CombatState.MainHand.AttackPower * dot_AP_Modifier) * (1 + CombatState.MainHand.Mastery))) / (feralDoT.BaseLength / feralDoT.Interval);
        }

        public override float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0f;
            float DamageCount = 1f;
            feralDoT.BaseDamage = this.Formula();
            Damage = this.GetTickDamage();
            DamageCount = feralDoT.TotalTickCount();
            if (Partial)
                Damage *= PartialValue;

            // Multiply by the number of ticks
            Damage *= DamageCount;

            // Add the initial Damage from the attack
            Damage += ((Formula_Initial_Damage() + DamageAdditiveModifer) * DamageMultiplierModifer);

            // Calculate the crit damage
            float CritDamage = (2 * Damage) * CritChance;
            Damage = (Damage * (1 - CritChance)) + CritDamage;

            return Damage;
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage / Energy;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            float minDamage = (Formula_Initial_Damage(MinDamage) + (Formula() * (Duration / feralDoT.Interval))) * DamageMultiplierModifer;
            float maxDamage = (Formula_Initial_Damage(MaxDamage) + (Formula() * (Duration / feralDoT.Interval))) * DamageMultiplierModifer;
            string Range = string.Format("Hit Range: {0} - {1}", minDamage.ToString("n"), maxDamage.ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", (minDamage * critDamMult).ToString("n"), (maxDamage * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}", TotalDamage.ToString("n"), sDPE, Range, CritRange, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance)) * (Duration / feralDoT.Interval);
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance) * (Duration / feralDoT.Interval);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;

            string thrashMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string thrashCount = string.Format("     Applications(# {0})\n", count.ToString("n"));
            string thrashHit = string.Format("     Tick Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string thrashCrit = string.Format("     Tick Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return thrashMain + thrashHit + thrashCrit;
        }
    }
}