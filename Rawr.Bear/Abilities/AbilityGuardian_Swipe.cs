using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Swipe : AbilityGuardian_Base
    {
        /// <summary>
        /// Swipe nearby enemies, inflicting 3609 (+ 100% of SpellPower) damage.\n\n
        /// Deals 20% increased damage against bleeding targets.
        /// </summary>
        public AbilityGuardian_Swipe()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Swipe(GuardianCombatState CState)
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
            Name = "Swipe";
            SpellID = 779;
            SpellIcon = "inv_misc_monsterclaw_03";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Physical;
            BaseSpellScaleModifier = 0.2249999940f;
            BaseDamage = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel);
            Formula_AP_Modifier = 0.2250000f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = 3f;
            AbilityIndex = (int)FeralAbility.SwipeBearForm;
            Range = 8;
            AOE = true;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
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
                    _DamageMultiplierModifer = (1 + guardianCombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + guardianCombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(guardianCombatState.Char.Level, 
                                                                                            guardianCombatState.BossArmor, 
                                                                                            guardianCombatState.MainHand.Stats.TargetArmorReduction, 
                                                                                            guardianCombatState.MainHand.Stats.ArmorPenetration))
                                             * (1 + (guardianCombatState.NumberOfBleeds > 0 ? 0.20f : 0));
                }
                return _DamageMultiplierModifer;
            }
        }

        private float p_Formula(float attackPower)
        {
            return BaseDamage + (Formula_AP_Modifier * attackPower);
        }

        public override float Formula()
        {
            return p_Formula(guardianCombatState.MainHand.VengenceAttackPower);
        }

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string Range = string.Format("Hit Range: {0}", ((p_Formula(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"));
            string CritRange = string.Format("Crit Range: {0}", (((p_Formula(guardianCombatState.MainHand.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"));
            string vengRange = string.Format("Vengence Hit Range: {0}", ((p_Formula(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"));
            string vengCritRange = string.Format("Vengence Crit Range: {0}", (((p_Formula(guardianCombatState.MainHand.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", HitChance.ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}", TotalDamage.ToString("n"), Range, CritRange, vengRange, vengCritRange, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance));
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * guardianCombatState.MainHand.CritDamageMultiplier;

            string mangleMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string mangleHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string mangleCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return mangleMain + mangleHit + mangleCrit;
        }
    }
}
