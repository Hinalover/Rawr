using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_FerociousBite : AbilityFeral_Base
    {
        private const float baseEnergy = 25f;
        public readonly float maxFormulaEnergy = 25f;
        private const float spellScaleAverage = 0.456999987f;
        private const float spellScaleDelta = 0.74000001f;
        private const float spellScaleBaseDamageAddition = 0.69599998f;
        /// <summary>
        /// Finishing move that causes damage per combo point and consumes up to 35 additional energy to 
        /// increase damage by up to 100%. Damage is increased by your attack power.
        /// 1 point:  ((358 + 866 * 1) + (0.223 * AP)) - ((778 + 866 * 1) + (0.223 * AP)) damage
        /// 2 points: ((358 + 866 * 2) + (0.446 * AP)) - ((778 + 866 * 2) + (0.446 * AP)) damage
        /// 3 points: ((358 + 866 * 3) + (0.669 * AP)) - ((778 + 866 * 3) + (0.669 * AP)) damage
        /// 4 points: ((358 + 866 * 4) + (0.892 * AP)) - ((778 + 866 * 4) + (0.892 * AP)) damage
        /// 5 points: ((358 + 866 * 5) + (1.115 * AP)) - ((778 + 866 * 5) + (1.115 * AP)) damage
        /// FA = Feral Agression = 1.10, 1.05, or 1.00
        /// </summary>
        public AbilityFeral_FerociousBite()
        {
            CombatState = new FeralCombatState();
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;
        }

        public AbilityFeral_FerociousBite(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        public AbilityFeral_FerociousBite(FeralCombatState CState, float CP)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = CP;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        public AbilityFeral_FerociousBite(FeralCombatState CState, float CP, float energy)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = CP;
            Formula_Energy = (float)Math.Max(0, Math.Min(energy, maxFormulaEnergy));

            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Ferocious Bite";
            SpellID = 22568;
            SpellIcon = "ability_druid_ferociousbite";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = baseEnergy;
            EnergyRefunded();
            Energy *= (CombatState.BerserkUptime > 0 ? AbilityFeral_Berserk.CostReduction * CombatState.BerserkUptime : 1f);
            ComboPoint = -1;

            DamageType = ItemDamageType.Physical;
            float druidSpellScaling = BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            float spellScaleBaseDamage = spellScaleAverage * druidSpellScaling;
            float spellScaleDeltaDamage = spellScaleDelta * druidSpellScaling;
            MinDamage = spellScaleBaseDamage - (spellScaleDeltaDamage / 2);
            MaxDamage = spellScaleBaseDamage + (spellScaleDeltaDamage / 2);

            Formula_CP_Base_Damage_Modifier = spellScaleBaseDamageAddition * druidSpellScaling;
            Formula_CP_AP_Modifier = 0.196f; //0.109f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.FerociousBite;
            Range = MELEE_RANGE;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            if (Formula_CP != 0)
            {
                // Combo Points cannot be over 5 Combo Points
                if (Formula_CP > 5)
                    Formula_CP = 5;
                this.ComboPoint = -Formula_CP;
            }
            if (Formula_Energy != 0)
            {
                if (Formula_Energy > maxFormulaEnergy)
                    Formula_Energy = maxFormulaEnergy;
                //this.Energy += Formula_Energy;
            }
            this.MainHand = CState.MainHand;
        }

        public override float CritChance
        {
            get
            {
                float crit = 0f;
                /*float RendandTear = 0f;
                switch (CombatState.Talents.RendAndTear)
                {
                    case 1:
                        RendandTear = .08f;
                        break;
                    case 2:
                        RendandTear = .17f;
                        break;
                    case 3:
                        RendandTear = .25f;
                        break;
                    default:
                        RendandTear = 0f;
                        break;
                }*/

                if (CombatState != null && CombatState.MainHand != null)
                    crit += CombatState.MainHand.CriticalStrike;
                crit += StatConversion.NPC_LEVEL_CRIT_MOD[3];
                crit *= HitChance;

                if (CombatState.NumberOfBleeds > 0)
                    crit += 0.25f; // crit += RendandTear;

                return Math.Max(0, Math.Min(1, crit));
            }
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
//                                             * (1 + ((CombatState.Talents.FeralAggression > 0) ? (CombatState.Talents.FeralAggression * 0.05f) : 0))
                                             * (1 + ((Formula_Energy > 0) ? (Formula_Energy / maxFormulaEnergy) : 0));
                }
                return _DamageMultiplierModifer;
            }
       }

        public override float Formula()
        {
            if (Formula_CP == 0)
                return 0f;
            else
                return (MinFormula(Formula_CP) + MaxFormula(Formula_CP)) / 2f;
        }

        private float MinFormula(float _CP)
        {
            return (float)Math.Floor((MinDamage + (Formula_CP_Base_Damage_Modifier * _CP)) + ((Formula_CP_AP_Modifier * _CP) * CombatState.MainHand.AttackPower));
        }

        private float MaxFormula(float _CP)
        {
            return (float)Math.Floor((MaxDamage + (Formula_CP_Base_Damage_Modifier * _CP)) + ((Formula_CP_AP_Modifier * _CP) * CombatState.MainHand.AttackPower));
        }

        public void resetBaseEnergy()
        {
            this.Energy = baseEnergy;
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage / (Energy + Formula_Energy);

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string cp1 = string.Format("1 CP: {0} - {1}", (MinFormula(1) * DamageMultiplierModifer).ToString("n"), (MaxFormula(1) * DamageMultiplierModifer).ToString("n"));
            string cp1c = string.Format("Crit: {0} - {1}", (MinFormula(1) * DamageMultiplierModifer * critDamMult).ToString("n"), (MaxFormula(1) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string cp2 = string.Format("2 CP: {0} - {1}", (MinFormula(2) * DamageMultiplierModifer).ToString("n"), (MaxFormula(2) * DamageMultiplierModifer).ToString("n"));
            string cp2c = string.Format("Crit: {0} - {1}", (MinFormula(2) * DamageMultiplierModifer * critDamMult).ToString("n"), (MaxFormula(2) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string cp3 = string.Format("3 CP: {0} - {1}", (MinFormula(3) * DamageMultiplierModifer).ToString("n"), (MaxFormula(3) * DamageMultiplierModifer).ToString("n"));
            string cp3c = string.Format("Crit: {0} - {1}", (MinFormula(3) * DamageMultiplierModifer * critDamMult).ToString("n"), (MaxFormula(3) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string cp4 = string.Format("4 CP: {0} - {1}", (MinFormula(4) * DamageMultiplierModifer).ToString("n"), (MaxFormula(4) * DamageMultiplierModifer).ToString("n"));
            string cp4c = string.Format("Crit: {0} - {1}", (MinFormula(4) * DamageMultiplierModifer * critDamMult).ToString("n"), (MaxFormula(4) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string cp5 = string.Format("5 CP: {0} - {1}", (MinFormula(5) * DamageMultiplierModifer).ToString("n"), (MaxFormula(5) * DamageMultiplierModifer).ToString("n"));
            string cp5c = string.Format("Crit: {0} - {1}", (MinFormula(5) * DamageMultiplierModifer * critDamMult).ToString("n"), (MaxFormula(5) * DamageMultiplierModifer * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n\n{4}\n{5}\n\n{6}\n{7}\n\n{8}\n{9}\n\n{10}\n{11}\n{12}\n{13}", TotalDamage.ToString("n"), sDPE, cp1, cp1c, cp2, cp2c, cp3, cp3c, cp4, cp4c, cp5, cp5c, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float hitCount = (count * (1 - CritChance));
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;

            string fbMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string fbHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string fbCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return fbMain + fbHit + fbCrit;
        }
    }
}
