using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_HealingTouch : AbilityGuardian_Base
    {
        private float BaseHealing = 0f;
        private float BaseHealingPowerMultiplier = 1.8600000143f;
        private float BaseHealingDelta = 0.1659999937f;
        private bool Natures_Swiftness = false;
        private float Natures_Swiftness_Multiplier = 0.50f;
        private float NS_Cooldown = 60f; 
        /// <summary>
        /// Heals a friendly target for 18460 to 21800 (+ 186% of SpellPower).
        /// </summary>
        public AbilityGuardian_HealingTouch()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_HealingTouch(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
        }

        public AbilityGuardian_HealingTouch(GuardianCombatState CState, bool natureSwiftness)
        {
            guardianCombatState = CState;
            baseInfo();
            UpdateCombatState(guardianCombatState);
            Natures_Swiftness = natureSwiftness;
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        private void baseInfo()
        {
            Name = "Healing Touch";
            SpellID = 5185;
            SpellIcon = "spell_nature_healingtouch";
            druidForm = new DruidForm[] { DruidForm.Bear, DruidForm.Cat, DruidForm.Boomkin, DruidForm.Caster, DruidForm.Tree };

            DamageType = ItemDamageType.Nature;
            BaseDamage = 0f;
            BaseSpellScaleModifier = 18.3880004883f;

            TriggersGCD = true;
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
            CastTime = (Natures_Swiftness ? (2.5f / (1 + CState.MainHand.Haste)) : 0);
            Mana = 0.289f * BaseCombatRating.DruidBaseMana(CState.CharacterLevel);
            BaseHealing = BaseSpellScaleModifier * BaseCombatRating.DruidSpellScaling(guardianCombatState.CharacterLevel);
            Cooldown = (Natures_Swiftness ? NS_Cooldown : (2.5f / (1 + CState.MainHand.Haste)));
        }

        private float h_Formula(float healing)
        {
            return (healing + (BaseHealingPowerMultiplier * guardianCombatState.MainHand.HealingPower)) * (1 + (Natures_Swiftness ? Natures_Swiftness_Multiplier : 0));
        }

        private float h_Formula()
        {
            float Crit = (h_Formula(BaseHealing) * 2f) * CritChance;
            float Hit = h_Formula(BaseHealing) * (1 - CritChance);
            return Hit + Crit;
        }

        public float HealingFormula
        {
            get
            {
                if (true)
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

            string hit = string.Format("Hit: {0} - {1}", (h_Formula(BaseHealing) * (1 - (BaseHealingDelta / 2f))).ToString("n2"),
                (h_Formula(BaseHealing) * (1 + (BaseHealingDelta / 2f))).ToString("n2"));
            string crit = string.Format("Crit: {0} - {1}", (h_Formula(BaseHealing) * (1 - (BaseHealingDelta / 2f)) * critDamMult).ToString("n2"),
                (h_Formula(BaseHealing) * (1 + (BaseHealingDelta / 2f)) * critDamMult).ToString("n2"));
            return string.Format("{0}*{1}\n{2}", h_Formula().ToString("n2"), hit, crit);
        }
    }
}