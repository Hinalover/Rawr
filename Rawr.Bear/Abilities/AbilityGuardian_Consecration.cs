using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_Consecration : AbilityGuardian_Base
    {
        /// <summary>
        /// Consecrates the land beneath the Druid, doing (90 * 10) Nature damage over 10 sec to enemies who enter the area.
        /// </summary>
        public AbilityGuardian_Consecration()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_Consecration(GuardianCombatState CState)
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
            Name = "Consecration";
            SpellID = 110701;
            SpellIcon = "spell_holy_innerfire";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Nature;
            BaseDamage = guardianCombatState.CharacterLevel;
            Formula_AP_Modifier = 0.04f;

            isDoT = true;
            feralDoT.Interval = 1f;
            feralDoT.BaseLength = 10f;
            Duration = feralDoT.TotalLength = feralDoT.BaseLength;
            useSpellHit = true;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = 30;
            AbilityIndex = (int)FeralAbility.Lacerate;
            Range = MELEE_RANGE;
            AOE = false;
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
                                                                                            guardianCombatState.MainHand.Stats.ArmorPenetration));
                }
                return _DamageMultiplierModifer;
            }
        }

       public override float Formula()
        {
            return (float)Math.Floor(BaseDamage + (Formula_AP_Modifier * guardianCombatState.MainHand.SpellPower) + (Formula_AP_Modifier * guardianCombatState.MainHand.AttackPower));
        }

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;
            float DPE = this.TotalDamage / Energy;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string TickHit = string.Format("DoT Hit: {0}", (Formula() * DamageMultiplierModifer).ToString("n"));
            string TickCrit = string.Format("DoT Crit: {0}", (Formula() * DamageMultiplierModifer * critDamMult).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}", this.TotalDamage.ToString("n"), sDPE, TickHit, TickCrit, Avoidance, Crit);
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
