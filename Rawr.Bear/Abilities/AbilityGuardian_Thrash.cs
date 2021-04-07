using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Thrash : AbilityGuardian_Base
    {
        /// <summary>
        /// Mangle the target for 470% weapon damage and generate [7][5] Rage.
        /// </summary>
        public AbilityGuardian_Thrash()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Thrash(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Thrash";
            SpellID = 77758;
            SpellIcon = "spell_druid_thrash";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Physical;
            BaseDamage = Math.Max(1, (BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel)));

            isDoT = true;
            feralDoT.Interval = 2f;
            feralDoT.BaseLength = 16f;
            Duration = feralDoT.TotalLength = feralDoT.BaseLength;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = 6;
            AbilityIndex = (int)FeralAbility.ThrashBearForm;
            Range = 8;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Formula_AP_Modifier = (CState.PTR ? 0.191f : 0.162f);
            BaseSpellScaleModifier = (CState.PTR ? 1.1250000000f : 0f);
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

        /// <summary>
        /// Applies the initial damage of Lacerate.
        /// Keeping these separated just in case
        /// </summary>
        /// <returns>Damage of the inital application of the attack</returns>
        private float Formula_Initial_Damage(float attackpower)
        {
            return (float)Math.Floor((attackpower * Formula_AP_Modifier) + BaseDamage);
        }

        private float p_Formula(float attackpower)
        {
            float DoT_AP_Modifier = (guardianCombatState.PTR ? 0.141f : 0.11984f); //0.0749f * 1.6f;
            float BaseBleedSpellScaleModifier = 0.6269999743f;
            float BaseBleedDamage = (guardianCombatState.PTR ? (BaseBleedSpellScaleModifier * BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel)) : 1f);
            return (float)Math.Floor((attackpower * DoT_AP_Modifier) + BaseBleedDamage);
        }

        public override float Formula()
        {
            return p_Formula(guardianCombatState.VengenceAttackPower);
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
            Damage += ((Formula_Initial_Damage(guardianCombatState.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer);

            // Calculate the crit damage
            float CritDamage = 2 * Damage * CritChance;
            Damage = (Damage * (HitChance - CritChance)) + CritDamage;

            return Damage;
        }
        
        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string initalDamage = string.Format("Initial Hit: {0} - {1}", (Formula_Initial_Damage(guardianCombatState.AttackPower) * DamageMultiplierModifer).ToString("n"),
                (Formula_Initial_Damage(guardianCombatState.VengenceAttackPower) * DamageMultiplierModifer).ToString("n"));
            string initialCrit = string.Format("Initial Crit: {0} - {1}", (Formula_Initial_Damage(guardianCombatState.AttackPower) * DamageMultiplierModifer * critDamMult).ToString("n"),
                (Formula_Initial_Damage(guardianCombatState.VengenceAttackPower) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string zerovengdotDamage = string.Format("Zero Veng DoT Damage: {0} - {1}", (p_Formula(guardianCombatState.AttackPower) * DamageMultiplierModifer * feralDoT.TotalTickCount()).ToString("n"),
                (p_Formula(guardianCombatState.AttackPower) * DamageMultiplierModifer * critDamMult * feralDoT.TotalTickCount()).ToString("n"));
            string avgvengdotDamage = string.Format("Veng DoT Damage: {0} - {1}", (p_Formula(guardianCombatState.VengenceAttackPower) * DamageMultiplierModifer * feralDoT.TotalTickCount()).ToString("n"),
                (p_Formula(guardianCombatState.VengenceAttackPower) * DamageMultiplierModifer * critDamMult * feralDoT.TotalTickCount()).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", HitChance.ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}", TotalDamage.ToString("n"), initalDamage, initialCrit, zerovengdotDamage, avgvengdotDamage, Avoidance, Crit);
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
