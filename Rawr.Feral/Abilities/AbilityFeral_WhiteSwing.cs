using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_WhiteSwing : AbilityFeral_Base
    {
        /// <summary>
        /// Automatically attacks a target in melee with an equipped weapon. This ability can be toggled on or off.
        /// </summary>
        public AbilityFeral_WhiteSwing()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_WhiteSwing(FeralCombatState CState)
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
            Name = "White Swing";
            SpellID = 6603;
            SpellIcon = "ability_druid_catformattack";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            Energy = 0;
            Rage = 10;
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = CombatState.MainHand.WeaponDamage;

            TriggersGCD = false;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            Duration = 0;
            Cooldown = 0;
            AbilityIndex = (int)FeralAbility.WhiteSwing;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            // TODO implement the Talent % for activating Fury Swipe
            //TriggeredAbility[0] = new AbilityFeral_FurySwipe(CState);
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
//                                             * (1 + (CombatState.SavageRoarUptime > 0 ? 0.80f * CombatState.TigersFuryUptime : 0f));
                }
                return _DamageMultiplierModifer;
            }
        }

       public override float Formula()
        {
            return (float)Math.Floor(this.BaseDamage);
        }

       public float GlanceChance
       {
           get
           {
               return CombatState.MainHand.ChanceToGlance;
           }
       }

       override public float HitChance
       {
           get
           {
               // Determine Miss Chance
               float ChanceToHit = 1 - CombatState.MainHand.chanceMissed
                                 - CombatState.MainHand.chanceDodged
                                 - CombatState.MainHand.chanceParried
                                 - CritChance
                                 - GlanceChance; // Ensure that crit is no lower than 0.
               //ChanceToHit -= CombatState.MainHand.chanceDodged;
               return Math.Max(0, Math.Min(1, ChanceToHit));
           }
       }
       
        public override float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float Damage = 0;
            
            // Add the initial Damage from the attack
            Damage = ((Formula() + DamageAdditiveModifer) * DamageMultiplierModifer);
            
            // Factor in Glancing Blows which does 75% normal damage
            float GlanceDamage = Damage * CombatState.MainHand.AverageGlancingPercent * GlanceChance;
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

        public const float Clearcasting = 0.0583333333f;

        public override string ToString()
        {
            float critDamMult = CombatState.MainHand.CritDamageMultiplier;
            float DPE = 0;

            string sDPE = string.Format("DPE: {0}", DPE.ToString("n"));
            string Range = string.Format("Hit Range: {0} - {1}", Math.Floor(CombatState.MainHand.MinDamage).ToString("n"), Math.Floor(CombatState.MainHand.MaxDamage).ToString("n"));
            string CritRange = string.Format("Crit Range: {0} - {1}", Math.Floor(CombatState.MainHand.MinDamage * critDamMult).ToString("n"), Math.Floor(CombatState.MainHand.MaxDamage * critDamMult).ToString("n"));
            string GlanceRange = string.Format("Glance Range: {0} - {1}", Math.Floor(CombatState.MainHand.MinDamage * 0.75f).ToString("n"), Math.Floor(CombatState.MainHand.MaxDamage * 0.75f).ToString("n"));
            string Avoidance = string.Format("Hit %: {0}", (1 - MissChance).ToString("p"));
            string Crit = string.Format("Crit %: {0}", CritChance.ToString("p"));
            string AttackSpeed = string.Format("Attack Speed: {0}", CombatState.MainHand.hastedSpeed.ToString());
            return string.Format("{0}*{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}", TotalDamage.ToString("n"), sDPE, Range, CritRange, GlanceRange, Avoidance, Crit, AttackSpeed);
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            float critCount = (count * CritChance);
            float critAvg = Formula() * DamageMultiplierModifer * CombatState.MainHand.CritDamageMultiplier;
            float glanceCount = (count * StatConversion.WHITE_GLANCE_CHANCE_CAP[Math.Max(0, (Math.Min(3, CombatState.TargetLevel - CombatState.CharacterLevel)))]);
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