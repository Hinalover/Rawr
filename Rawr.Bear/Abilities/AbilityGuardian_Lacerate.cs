using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Lacerate : AbilityGuardian_Base
    {
        private float _stacks = 1f;
        /// <summary>
        /// Lacerates the enemy target, dealing (1 + (AP * 0.822)) damage.  
        /// Each time Lacerate hits, it has a 25% chance to reset the cooldown on Mangle.\n\n
        /// Every 3 sec, the effect will deal (1 + (AP * 0.0682)) bleed damage.\n\n
        /// Stacks up to 3 times and lasts 15 sec.
        /// </summary>
        public AbilityGuardian_Lacerate()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Lacerate(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        public AbilityGuardian_Lacerate(GuardianCombatState CState, float stacks)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
            _stacks = (stacks < 1 ? 1 : (stacks > 3 ? 3 : stacks));
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Lacerate";
            SpellID = 33745;
            SpellIcon = "ability_druid_lacerate";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Physical;
            BaseDamage = 1f;
            Formula_AP_Modifier = 0.616f;

            isDoT = true;
            feralDoT.Interval = 3f;
            feralDoT.BaseLength = 15f;
            Duration = feralDoT.TotalLength = feralDoT.BaseLength;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = 3;
            AbilityIndex = (int)FeralAbility.Lacerate;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public const float chanceToResetMangleCooldown = 0.25f;

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
                    _DamageMultiplierModifer = (1 + guardianCombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + guardianCombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(guardianCombatState.Char.Level, 
                                                                                            guardianCombatState.BossArmor, 
                                                                                            guardianCombatState.MainHand.Stats.TargetArmorReduction, 
                                                                                            guardianCombatState.MainHand.Stats.ArmorPenetration));
                }
                return _DamageMultiplierModifer;
            }
        }

        private float p_Formula_Initial_Damage(float attackPower)
        {
            return (float)Math.Floor((attackPower * Formula_AP_Modifier) + BaseDamage);
        }

        /// <summary>
        /// Applies the initial damage of Lacerate.
        /// Keeping these separated just in case
        /// </summary>
        /// <returns>Damage of the inital application of the attack</returns>
        private float Formula_Initial_Damage()
        {
            return p_Formula_Initial_Damage(guardianCombatState.MainHand.VengenceAttackPower);
        }

        private float p_Formula(float attackPower)
        {
            float DoT_AP_Modifier = 0.0512f;
            return ((float)Math.Floor(((attackPower * DoT_AP_Modifier) + BaseDamage) * _stacks) * (1 + (guardianCombatState.MainHand.Stats.Tier_11_2_piece ? 0.10f : 0)));
        }

        public override float Formula()
        {
            return p_Formula(guardianCombatState.MainHand.VengenceAttackPower);
        }

       public float TotalTickCount()
       {
           if ((feralDoT.TotalLength > 0) && (feralDoT.Interval > 0))
               // We want to refresh before the length expires so assume it is refreshed with one GCD prior it expires
               return ((feralDoT.TotalLength - AVG_GCD_MS) / feralDoT.Interval);
           else
               return 1f;
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
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string initalDamage = string.Format("Initial Hit: {0} - {1}", ((p_Formula_Initial_Damage(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"),
                ((p_Formula_Initial_Damage(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"));
            string initialCrit = string.Format("Initial Crit: {0} - {1}", (((p_Formula_Initial_Damage(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"),
                (((p_Formula_Initial_Damage(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"));
            string dotDamage = string.Format("Zero Veng DoT Damage: {0} - {1}", (((p_Formula(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * feralDoT.TotalTickCount()).ToString("n"),
                (((p_Formula(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult * feralDoT.TotalTickCount()).ToString("n"));
            string vengDotDamage = string.Format("Veng DoT Damage: {0} - {1}", (((p_Formula(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * feralDoT.TotalTickCount()).ToString("n"),
                (((p_Formula(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult * feralDoT.TotalTickCount()).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", HitChance.ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}", TotalDamage.ToString("n"), initalDamage, initialCrit, dotDamage, vengDotDamage, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance)) * (Duration / feralDoT.Interval);
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance) * (Duration / feralDoT.Interval);
            float critAvg = Formula() * DamageMultiplierModifer * guardianCombatState.MainHand.CritDamageMultiplier;

            string rakeMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string rakeCount = string.Format("     Applications(# {0})\n", count.ToString("n"));
            string rakeHit = string.Format("     Tick Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string rakeCrit = string.Format("     Tick Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return rakeMain + rakeCount + rakeHit + rakeCrit;
        }
    }
}
