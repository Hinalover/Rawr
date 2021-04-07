using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Shred : AbilityFeral_Base
    {
        private float baseEnergy = 40;
        private float baseCP = 1;
        /// <summary>
        /// Shred the target, causing 480% damage plus 62 to the target. Must be behind the target. Awards 1 combo points.
        ///
        /// Deals 20% additional damage against bleeding targets.
        /// </summary>
        public AbilityFeral_Shred()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_Shred(FeralCombatState CState)
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
            Name = "Shred";
            SpellID = 5221;
            SpellIcon = "spell_shadow_vampiricaura";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = baseEnergy;
            ComboPoint = baseCP;
            
            DamageType = ItemDamageType.Physical;
            BaseSpellScaleModifier = 0.0710000000f;
            BaseDamage = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            BaseWeaponDamageMultiplier = 5.00f;

            TriggersGCD = true;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Shred;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            if (CombatState.Stats.Tier_12_2pc)
            {
                TriggeredAbility[1] = new AbilityFeral_FieryClaws(CState);
                TriggeredAbility[1].BaseDamage = this.TotalDamage * 0.10f;
            } 
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
                
                float RendandTear = 0.20f;
                /*switch (CombatState.Talents.RendAndTear)
                {
                    case 0:
                        RendandTear = 0;
                        break;
                    case 1:
                        RendandTear = .07f;
                        break;
                    case 2:
                        RendandTear = .13f;
                        break;
                    case 3:
                        RendandTear = .20f;
                        break;
                }*/
                if (_DamageMultiplierModifer == 0)
                {
                    _DamageMultiplierModifer = (1 + CombatState.MainHand.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.MainHand.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.MainHand.Stats.TargetArmorReduction, CombatState.MainHand.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime > 0 ? AbilityFeral_TigersFury.DamageBonus * CombatState.TigersFuryUptime : 0f))
                                             * (1 + (CombatState.SavageRoarUptime > 0 ? AbilityFeral_SavageRoar.DamageBonus * CombatState.SavageRoarUptime : 0f))
                                             * (1 + (CombatState.NumberOfBleeds > 0 ? RendandTear : 0f)) // Increases damage by 20% whenever a bleed is on the target
                                             * (1 + (CombatState.MainHand.Stats.Tier_14_2_piece ? 0.05f : 0f))
                                             * (1 + CombatState.MainHand.Stats.BonusBleedDamageMultiplier);
                }
                return _DamageMultiplierModifer;
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
            float DPE = TotalDamage / Energy;

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

            string shredMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string shredHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string shredCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return shredMain + shredHit + shredCrit;
        }
    }
}