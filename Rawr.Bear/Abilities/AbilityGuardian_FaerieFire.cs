using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_FaerieFire : AbilityGuardian_Base
    {
        private float _gcd = 1.5f;
        public float GCD;
        /// <summary>
        /// Faeries surround the target, preventing it from stealthing or turning invisible, 
        /// and causing 3 applications of the Weakened Armor debuff./n/n
        /// Deals 10 damage and has a 25% chance to reset the cooldown on Mangle when cast from Bear Form.
        /// </summary>
        public AbilityGuardian_FaerieFire()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_FaerieFire(GuardianCombatState CState)
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
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Nature;
            Formula_AP_Modifier = 0.3020000f;

            TriggersGCD = true;
            useSpellHit = true;
            TriggeredAbility = new AbilityFeral_Base[1];
            CastTime = 0;
            AbilityIndex = (int)FeralAbility.FaerieFormFeral;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Name = ((CState.Talents.FaerieSwarm > 0) ? "Faerie Swarm" : "Faerie Fire");
            SpellID = (uint)((CState.Talents.FaerieSwarm > 0) ? 102355 : 770);
            SpellIcon = ((CState.Talents.FaerieSwarm > 0) ? "spell_druid_swarm" : "spell_nature_faeriefire");
            Cooldown = 0f + (CState.Talents.GlyphofFaeSilence ? 15f : 0);
            Range = (uint)35 + (uint)(CState.Talents.GlyphOfFaerieFire ? 10 : 0);
            GCD = (float)Math.Max(MIN_GCD_MS, _gcd / (1 + CState.MainHand.Haste));
        }

        /// <summary>
        /// Gain an additional 15 Rage every time you deal a critical strike with autoattack or Mangle
        /// </summary>
        public const float chanceToResetMangleCooldown = 0.25f;

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
                                             * (1 + guardianCombatState.MainHand.Stats.BonusPhysicalDamageMultiplier);
                }
                return _DamageMultiplierModifer;
            }
        }

        private float p_Formula(float attackpower)
        {
            return attackpower * Formula_AP_Modifier;
        }

        public override float Formula()
        {
            return p_Formula(guardianCombatState.VengenceAttackPower);
        }

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string Range = string.Format("Hit: {0}", ((p_Formula(guardianCombatState.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"));
            string CritRange = string.Format("Crit: {0}", (((p_Formula(guardianCombatState.AttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"));
            string vengRange = string.Format("Vengence Hit: {0}", ((p_Formula(guardianCombatState.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer).ToString("n"));
            string vengCritRange = string.Format("Vengence Crit: {0}", (((p_Formula(guardianCombatState.VengenceAttackPower) + DamageAdditiveModifer) * DamageMultiplierModifer) * critDamMult).ToString("n"));
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