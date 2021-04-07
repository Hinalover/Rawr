using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Maul: AbilityGuardian_Base
    {
        /// <summary>
        /// Maul the target for 110% weapon damage./n/n
        /// Deals 20% additional damage against bleeding targets./n/n
        /// Use when you have more Rage than you can spend.
        /// </summary>
        public AbilityGuardian_Maul()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Maul(GuardianCombatState CState)
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
            Name = "Maul";
            SpellID = 6807;
            SpellIcon = "ability_druid_maul";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 30f;
            
            DamageType = ItemDamageType.Physical;
            BaseWeaponDamageMultiplier = 1.10f;

            TriggersGCD = false;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            Cooldown = 0;
            AbilityIndex = (int)FeralAbility.Maul;
            Range = MELEE_RANGE;
            AOE = guardianCombatState.Talents.GlyphofMaul;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        /// <summary>
        /// Gain an additional 15 Rage every time you deal a critical strike with autoattack or Mangle
        /// </summary>
        public const float PrimalFury = 15f;

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

        public override float Formula()
        {
            return pFormula(guardianCombatState.VengenceWeaponDamage);
        }

        private float pFormula(float damage)
        {
            return (float)Math.Floor(BaseWeaponDamageMultiplier * damage);
        }

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
                DamageCount *= Math.Max(1, Math.Min((guardianCombatState.Talents.GlyphofMaul ? 2 : 1), this.guardianCombatState.NumberOfTargets));
            }

            Damage *= (DamageCount > 1 ? 1.5f : 1);
            float CritDamage = (guardianCombatState.MainHand.CritDamageMultiplier * Damage) * CritChance;
            Damage = (Damage * (HitChance - CritChance)) + CritDamage;

            return Damage;
        }


        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;
            float DPR = TotalDamage / Rage;

            string sDPR = string.Format("DPR: {0}", DPR.ToString("n"));
            string Range = string.Format("Hit Range: {0} - {1}", pFormula(guardianCombatState.MinDamage).ToString("n"), pFormula(guardianCombatState.MaxDamage).ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", (pFormula(guardianCombatState.MinDamage) * critDamMult).ToString("n"), (pFormula(guardianCombatState.MaxDamage) * critDamMult).ToString("n"));
            string vengRange = string.Format("Vengence Hit Range: {0} - {1}", pFormula(guardianCombatState.VengenceMinDamage).ToString("n"), pFormula(guardianCombatState.VengenceMaxDamage).ToString("n"));
            string vengCritRange = string.Format("Vengence Crit Range: {0} - {1}", (pFormula(guardianCombatState.VengenceMinDamage) * critDamMult).ToString("n"), (pFormula(guardianCombatState.VengenceMaxDamage) * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", HitChance.ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}", TotalDamage.ToString("n"), sDPR, Range, CritRange, vengRange, vengCritRange, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance));
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * guardianCombatState.MainHand.CritDamageMultiplier;

            string maulMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string maulHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string maulCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return maulMain + maulHit + maulCrit;
        }
    }
}