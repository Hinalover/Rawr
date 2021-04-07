using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Mangle : AbilityGuardian_Base
    {
        /// <summary>
        /// Mangle the target for 470% weapon damage and generate [7][5] Rage.
        /// </summary>
        public AbilityGuardian_Mangle()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Mangle(GuardianCombatState CState)
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
            Name = "Mangle";
            SpellID = 33878;
            SpellIcon = "ability_druid_mangle2";
            druidForm = new DruidForm[] { DruidForm.Bear };

            DamageType = ItemDamageType.Physical;
            BaseWeaponDamageMultiplier = 2.80f;

            TriggersGCD = true;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 6;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.MangleBearForm;
            Range = MELEE_RANGE;
            AOE = false;
            useSpellHit = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Rage = 5 + (CState.Talents.SoulOfTheForest > 0 ? 2 : 0);
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
                                                                                            guardianCombatState.MainHand.Stats.ArmorPenetration));
                }
                return _DamageMultiplierModifer;
            }
        }

        /// <summary>
        /// Gain an additional 15 Rage every time you deal a critical strike with autoattack or Mangle
        /// </summary>
        private const float PrimalFury = 15f;

        public float getRageFromHit(float count)
        {
            return count * Rage;
        }

        public float getRageFromCrit(float count)
        {
            return (CritChance * count) * PrimalFury;
        }

        public float getTotalRageGenerated(float count)
        {
            float rage = 0;
            rage += getRageFromHit(count);
            rage += getRageFromCrit(count);
            return rage;
        }

        public override float Formula()
        {
            return pFormula(guardianCombatState.VengenceWeaponDamage);
        }

        private float pFormula(float damage)
        {
            return (float)Math.Floor(BaseWeaponDamageMultiplier * damage);
        }

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string Range = string.Format("Hit Range: {0} - {1}", pFormula(guardianCombatState.MinDamage).ToString("n"), pFormula(guardianCombatState.MaxDamage).ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", (pFormula(guardianCombatState.MinDamage) * critDamMult).ToString("n"), (pFormula(guardianCombatState.MaxDamage) * critDamMult).ToString("n"));
            string vengRange = string.Format("Vengence Hit Range: {0} - {1}", pFormula(guardianCombatState.VengenceMinDamage).ToString("n"), pFormula(guardianCombatState.VengenceMaxDamage).ToString("n"));
            string vengCritRange = string.Format("Vengence Crit Range: {0} - {1}", (pFormula(guardianCombatState.VengenceMinDamage) * critDamMult).ToString("n"), (pFormula(guardianCombatState.VengenceMaxDamage) * critDamMult).ToString("n"));
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
