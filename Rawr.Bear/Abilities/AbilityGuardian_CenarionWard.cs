using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_CenarionWard : AbilityGuardian_Base
    {
        private float BaseHealing = 0f;
        private float BaseHealingPowerMultiplier = 0.5699999928f;
        /// <summary>
        /// Protects a friendly target, causing any damage taken to heal the target for 6174 (+ 57% of SpellPower) 
        /// every 2 sec for 6 sec.  Gaining the healing effect consumes the Cenarion Ward.  Useable in all shapeshift forms.  Lasts 30 sec.
        /// </summary>
        public AbilityGuardian_CenarionWard()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_CenarionWard(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        private void baseInfo()
        {
            Name = "Cenarion Ward";
            SpellID = 102352;
            SpellIcon = "ability_druid_naturalperfection";
            druidForm = new DruidForm[] { DruidForm.Bear, DruidForm.Cat, DruidForm.Boomkin, DruidForm.Caster, DruidForm.Tree };

            DamageType = ItemDamageType.Nature;
            BaseDamage = 0f;
            BaseSpellScaleModifier = 5.6399998665f;

            TriggersGCD = true;
            CastTime = 0f;
            Cooldown = 30f;
            Duration = feralDoT.BaseLength = 6f;
            isDoT = true;
            feralDoT.Interval = 2f;
            AbilityIndex = (int)FeralAbility.CenarionWard;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Mana = 0.148f * BaseCombatRating.DruidBaseMana(CState.CharacterLevel);
            BaseHealing = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel);
        }

        private float h_Formula(float healing)
        {
            return healing + (BaseHealingPowerMultiplier * guardianCombatState.MainHand.HealingPower);
        }

        private float h_Formula()
        {
            float crit = h_Formula(BaseHealing) * 2f * CritChance;
            float hit = h_Formula(BaseHealing) * (1 - CritChance);
            return (hit + crit) * feralDoT.TotalTickCount();
        }

        public float HealingFormula
        {
            get
            {
                if (guardianCombatState.Talents.CenarionWard)
                    return h_Formula();
                else
                    return 0f;
            }
        }

        public override float Formula()
        {
            return 0f;
        }

        public override string ToString()
        {
            float critDamMult = guardianCombatState.MainHand.CritDamageMultiplier;

            string hit = string.Format("Tick Hit: {0}", h_Formula(BaseHealing).ToString("n2"));
            string crit = string.Format("Tick Crit: {0}", (h_Formula(BaseHealing) * critDamMult).ToString("n2"));
            string average = string.Format("Tick Average: {0}", (h_Formula() / feralDoT.TotalTickCount()).ToString("n2"));
            return string.Format("{0}*{1}\n{2}\n{3}", h_Formula().ToString("n2"), hit, crit, average);
        }
    }
}