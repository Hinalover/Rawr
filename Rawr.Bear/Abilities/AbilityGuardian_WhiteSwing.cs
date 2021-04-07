using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_WhiteSwing : AbilityGuardian_Base
    {
        /// <summary>
        /// Automatically attacks a target in melee with an equipped weapon. This ability can be toggled on or off.
        /// </summary>
        public AbilityGuardian_WhiteSwing()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_WhiteSwing(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "White Swing";
            SpellID = 6603;
            SpellIcon = "ability_druid_catformattack";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            DamageType = ItemDamageType.Physical;
            BaseDamage = guardianCombatState.VengenceWeaponDamage;

            TriggersGCD = false;
            CastTime = 0;
            Duration = 0;
            Cooldown = 0;
            AbilityIndex = (int)FeralAbility.WhiteSwing;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Rage = (1.75f * CState.MainHand.baseAttackSpeed) * rageMultiplier;
        }

        private float rageMultiplier = 2.5f; // 1.43;

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
            return (float)Math.Floor((count * (HitChance + CritChance + GlanceChance)) * Rage);
        }

        public float getRageFromCrit(float count)
        {
            return (float)Math.Floor(CritChance * count) * PrimalFury;
        }

        public float getTotalRageGenerated(float count)
        {
            float rage = 0;
            rage += getRageFromHit(count);
            rage += getRageFromCrit(count);
            return rage;
        }

        public float GlanceChance
        {
            get
            {
                return guardianCombatState.MainHand.ChanceToGlance;
            }
        }

        override public float HitChance
        {
            get
            {
                // Determine Miss Chance
                float ChanceToHit = 1 - guardianCombatState.MainHand.chanceMissed
                                  - guardianCombatState.MainHand.chanceDodged
                                  - guardianCombatState.MainHand.chanceParried
                                  - CritChance
                                  - GlanceChance; // Ensure that crit is no lower than 0.
                //ChanceToHit -= CombatState.MainHand.chanceDodged;
                return Math.Max(0, Math.Min(1, ChanceToHit));
            }
        }

        public override float Formula()
        {
            return (float)Math.Floor(this.BaseDamage);
        }

        public override float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0;
            
            // Add the initial Damage from the attack
            Damage = ((Formula() + DamageAdditiveModifer) * (1 + DamageMultiplierModifer));
            
            // Factor in Glancing Blows which does 75% normal damage
            float GlanceDamage = Damage * guardianCombatState.MainHand.AverageGlancingPercent * GlanceChance;
            // Calculate the crit damage
            float CritDamage = 2 * Damage * CritChance;
            float HitDamage = Damage * HitChance;
            Damage = HitDamage + CritDamage + GlanceDamage;

            return Damage;
        }
        
        public override float GetDPS()
        {
            return TotalDamage / CombatState.MainHand.hastedSpeed;
        }

        /// <summary>
        /// Percent chance to proc Clearcasting
        /// </summary>
        public readonly float Clearcasting = 0.0583333333f;

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string Range = string.Format("Hit Range: {0} - {1}", Math.Floor(guardianCombatState.MinDamage).ToString("n"), Math.Floor(guardianCombatState.MaxDamage).ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", Math.Floor(guardianCombatState.MinDamage * critDamMult).ToString("n"), Math.Floor(guardianCombatState.MaxDamage * critDamMult).ToString("n"));
            string GlanceRange = string.Format("Glance Range: {0} - {1}", Math.Floor(guardianCombatState.MinDamage * 0.75f).ToString("n"), Math.Floor(guardianCombatState.MaxDamage * 0.75f).ToString("n"));
            string vengRange = string.Format("Vengence Hit Range: {0} - {1}", Math.Floor(guardianCombatState.VengenceMinDamage).ToString("n"), Math.Floor(guardianCombatState.VengenceMaxDamage).ToString("n"));
            string vengCritRange = string.Format("Vengence Crit Range: {0} - {1}", Math.Floor(guardianCombatState.VengenceMinDamage * critDamMult).ToString("n"), Math.Floor(guardianCombatState.VengenceMaxDamage * critDamMult).ToString("n"));
            string vengGlanceRange = string.Format("Vengence Glance Range: {0} - {1}", Math.Floor(guardianCombatState.VengenceMinDamage * 0.75f).ToString("n"), Math.Floor(guardianCombatState.VengenceMaxDamage * 0.75f).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", Math.Min(1, (HitChance + CritChance + GlanceChance)).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));

            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}", TotalDamage.ToString("n"), Range, CritRange, GlanceRange, vengRange, vengCritRange, vengGlanceRange, Avoidance, Crit);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * guardianCombatState.MainHand.CritDamageMultiplier;
            float glanceCount = (count * StatConversion.WHITE_GLANCE_CHANCE_CAP[Math.Max(0, (Math.Min(3, guardianCombatState.TargetLevel - guardianCombatState.CharacterLevel)))]);
            float glanceAvg = Formula() * DamageMultiplierModifer * 0.75f;
            float hitCount = (count - critCount - glanceCount);
            float hitAvg = Formula() * DamageMultiplierModifer;

            string whiteMain = string.Format("{0}: {1}, {2}\n", Name, percent.ToString("p"), damageDone.ToString("n0"));
            string whiteHit = string.Format("     Hit(# {0}, Average: {1}, Total: {2})\n", hitCount.ToString("n"), hitAvg.ToString("n0"), (hitCount * hitAvg).ToString("n0"));
            string whiteCrit = string.Format("     Crit(# {0}, Average: {1}, Total: {2})\n", critCount.ToString("n"), critAvg.ToString("n0"), (critCount * critAvg).ToString("n0"));
            string whiteGlance = string.Format("     Glance(# {0}, Average: {1}, Total: {2})", glanceCount.ToString("n"), glanceAvg.ToString("n0"), (glanceCount * glanceAvg).ToString("n0"));

            return whiteMain + whiteHit + whiteCrit + whiteGlance;
        }
    }
}