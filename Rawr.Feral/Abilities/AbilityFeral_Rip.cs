using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Rip : AbilityFeral_Base
    {
        private const float spellScaleAverage = 0.1030000001f;
        private const float spellScaleBaseDamageAddition = 0.2919999957f;
        /// <summary>
        /// Finishing move that causes Bleed damage over time.  Damage increases per combo point and by your attack power:
        /// 1 point:  (((128 + ((363 * 1) * 1.3)) + (((0.055 * 1) * AP) * 1.3f)) * 8)  damage over 16 sec.
        /// 2 points: (((128 + ((363 * 2) * 1.3)) + (((0.055 * 2) * AP) * 1.3f)) * 8)  damage over 16 sec.
        /// 3 points: (((128 + ((363 * 3) * 1.3)) + (((0.055 * 3) * AP) * 1.3f)) * 8)  damage over 16 sec.
        /// 4 points: (((128 + ((363 * 4) * 1.3)) + (((0.055 * 4) * AP) * 1.3f)) * 8)  damage over 16 sec.
        /// 5 points: (((128 + ((363 * 5) * 1.3)) + (((0.055 * 5) * AP) * 1.3f)) * 8)  damage over 16 sec.
        /// </summary>
        public AbilityFeral_Rip()
        {
            CombatState = new FeralCombatState();
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;
        }

        public AbilityFeral_Rip(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = 0;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        public AbilityFeral_Rip(FeralCombatState CState, float CP)
        {
            CombatState = CState;
            baseInfo();

            Formula_CP = CP;
            Formula_Energy = 0;

            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Rip";
            SpellID = 1079;
            SpellIcon = "ability_ghoulfrenzy";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 30;
            EnergyRefunded();
            Energy *= (CombatState.BerserkUptime > 0 ? 0.5f * CombatState.BerserkUptime : 1f);
            ComboPoint = -1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = spellScaleAverage * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);

            isDoT = true;
            feralDoT.Interval = 2f;
            feralDoT.BaseLength = 16f;
            // Assume the Glyph of Bloodletting adds the six extra seconds
            feralDoT.TotalLength = feralDoT.BaseLength + 6 + (CombatState.MainHand.Stats.Tier_14_4_piece ? 4 : 0);//(CombatState.Talents.GlyphOfBloodletting ? (6f) : 0);
            Duration = feralDoT.TotalLength;

            BaseSpellScaleModifier = spellScaleBaseDamageAddition;
            Formula_CP_Base_Damage_Modifier = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(CombatState.CharacterLevel);
            Formula_CP_AP_Modifier = 0.0484f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Rip;
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
                this.ComboPoint = Formula_CP;
            }
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
                                             * (1 + (CombatState.SavageRoarUptime > 0 ? AbilityFeral_SavageRoar.DamageBonus * CombatState.SavageRoarUptime : 0f)); // Mastery Multiplier
                }
                return _DamageMultiplierModifer;
            }
        }

        public override float Formula()
        {
            if (Formula_CP == 0)
                return 0f;
            else
                return BaseFormula(Formula_CP);
        }

        private float BaseFormula(float _CP)
        {
            return (float)Math.Floor(BaseDamage + (Formula_CP_Base_Damage_Modifier * _CP * (1 + CombatState.MainHand.Mastery)) + (Formula_CP_AP_Modifier * _CP * CombatState.MainHand.AttackPower * (1 + CombatState.MainHand.Mastery)));
        }

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = TotalDamage / Energy;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string cp1 = string.Format("1 CP: {0} - {1}", ((BaseFormula(1) * DamageMultiplierModifer) * (Duration / feralDoT.Interval)).ToString("n"),
                ((BaseFormula(1) * DamageMultiplierModifer) * (Duration / feralDoT.Interval) * critDamMult).ToString("n"));
            string cp2 = string.Format("2 CP: {0} - {1}", ((BaseFormula(2) * DamageMultiplierModifer) * (Duration / feralDoT.Interval)).ToString("n"),
                ((BaseFormula(2) * DamageMultiplierModifer) * (Duration / feralDoT.Interval) * critDamMult).ToString("n"));
            string cp3 = string.Format("3 CP: {0} - {1}", ((BaseFormula(3) * DamageMultiplierModifer) * (Duration / feralDoT.Interval)).ToString("n"),
                ((BaseFormula(3) * DamageMultiplierModifer) * (Duration / feralDoT.Interval) * critDamMult).ToString("n"));
            string cp4 = string.Format("4 CP: {0} - {1}", ((BaseFormula(4) * DamageMultiplierModifer) * (Duration / feralDoT.Interval)).ToString("n"),
                ((BaseFormula(4) * DamageMultiplierModifer) * (Duration / feralDoT.Interval) * critDamMult).ToString("n"));
            string cp5 = string.Format("5 CP: {0} - {1}", ((BaseFormula(5) * DamageMultiplierModifer) * (Duration / feralDoT.Interval)).ToString("n"),
                ((BaseFormula(5) * DamageMultiplierModifer) * (Duration / feralDoT.Interval) * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}", TotalDamage.ToString("n"), sDPE, cp1, cp2, cp3, cp4, cp5, Avoidance, Crit);
        }

        public string byAbility(float count, float bloodintheWater, float percent, float total, float damageDone)
        {
            float hitCount = ((count + bloodintheWater) * (1 - CritChance)) * (Duration / feralDoT.Interval);
            float hitAvg = Formula() * DamageMultiplierModifer;
            float critCount = ((count + bloodintheWater) * CritChance) * (Duration / feralDoT.Interval);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;

            string ripMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string ripCount = string.Format("     Applications(# {0}, Refresh: {1})\n", count.ToString("n"), bloodintheWater.ToString("n"));
            string ripHit = string.Format("     Tick Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string ripCrit = string.Format("     Tick Crit(# {0}, Average: {1}, Total: {2})", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));

            return ripMain + ripCount + ripHit + ripCrit;
        }
    }
}
