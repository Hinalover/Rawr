using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_RavageProc : AbilityFeral_Base
    {
        /// <summary>
        /// Ravage the target, causing 950% damage plus 56 to the target.  
        /// Must be prowling and behind the target.  Awards 1 combo point.
        /// </summary>
        public AbilityFeral_RavageProc()
        {
            CombatState = new FeralCombatState() { Stats = new StatsFeral() };
            baseInfo();
        }

        public AbilityFeral_RavageProc(FeralCombatState CState)
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
            Name = "Ravage!";
            SpellID = 81170;
            SpellIcon = "ability_druid_ravage";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 45;
            EnergyRefunded();
            Energy *= (CombatState.BerserkUptime > 0 ? AbilityFeral_Berserk.CostReduction * CombatState.BerserkUptime : 1f);
            ComboPoint = 1;
            ComboPoint += CombatState.MainHand.CriticalStrike;

            DamageType = ItemDamageType.Physical;
            BaseSpellScaleModifier = 0.0570000000f;
            BaseDamage = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            BaseWeaponDamageMultiplier = 7.5f;
            // Assume this is procced from Stampede thus no longer requires Prowling
            MustBeProwling = false;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.RavageProc;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
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
                                             * (1 + (CombatState.SavageRoarUptime > 0 ? AbilityFeral_SavageRoar.DamageBonus * CombatState.SavageRoarUptime : 0f));
                }
                return _DamageMultiplierModifer;
            }
        }

        public override float CritChance
        {
            get
            {
                float crit = 0f;
                if (CombatState != null && CombatState.MainHand != null)
                {
                    crit += (0.25f /*+ CombatState.Talents.PredatoryStrikes)*/ * CombatState.Above80Percent);
                    crit += CombatState.MainHand.CriticalStrike;
                }
                crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                crit *= HitChance;
                return Math.Max(0, Math.Min(1, crit));
            }
        }

        public override float Formula()
        {
            return pFormula(CombatState.MainHand.WeaponDamage);
        }

        private float pFormula(float damage)
        {
            return (float)Math.Floor(BaseDamage + (BaseWeaponDamageMultiplier * damage));
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string Range = string.Format("Hit Range: {0} - {1}", pFormula(CombatState.MainHand.MinDamage).ToString("n"), pFormula(CombatState.MainHand.MaxDamage).ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", (pFormula(CombatState.MainHand.MinDamage) * critDamMult).ToString("n"), (pFormula(CombatState.MainHand.MaxDamage) * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}", TotalDamage.ToString("n"), sDPE, Range, CritRange, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance));
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;

            string ravageProcMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string ravageProcHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string ravageProcCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return ravageProcMain + ravageProcHit + ravageProcCrit;
        }
    }
}
