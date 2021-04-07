using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_FrenziedRegen : AbilityGuardian_Base
    {
        private float maxRage = 60f;
        /// <summary>
        /// Instantly converts up to 60 Rage into up to (AP * 25 / 10) health.
        /// </summary>
        public AbilityGuardian_FrenziedRegen()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_FrenziedRegen(GuardianCombatState CState)
        {
            guardianCombatState = CState;
            baseInfo();
            if (CState.Talents.GlyphofFrenziedRegeneration)
                Rage = maxRage;
            else
                Rage = 0;
            UpdateCombatState(guardianCombatState);
        }

        public AbilityGuardian_FrenziedRegen(GuardianCombatState CState, float rage)
        {
            guardianCombatState = CState;
            baseInfo();
            if (CState.Talents.GlyphOfFrenziedRegeneration)
                Rage = maxRage;
            else
            {
                if (rage > maxRage)
                    Rage = maxRage;
                else
                    Rage = rage;
            }
            UpdateCombatState(guardianCombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            Name = "Frenzied Regeneration";
            SpellID = 22842;
            SpellIcon = "ability_bullrush";
            druidForm = new DruidForm[] { DruidForm.Bear };

            Rage = 0;
            
            DamageType = ItemDamageType.Nature;

            // If the player is using the Glyph of FR, then set the duration of the increased healing

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = 1.5f;
            AbilityIndex = (int)FeralAbility.FrenziedRegeneration;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
            Duration = (CState.Talents.GlyphOfFrenziedRegeneration ? 6f : 0);
        }

        private float getAPFormula
        {
            get
            {
                return (guardianCombatState.VengenceAttackPower - (guardianCombatState.Stats.Agility * 2f)) * 2f * (1 + (guardianCombatState.Stats.Tier_14_4_piece ? 0.10f : 0f));
            }
        }

        private float getStamFormula
        {
            get
            {
                return guardianCombatState.Stats.Stamina * 2.5f * (1 + (guardianCombatState.Stats.Tier_14_4_piece ? 0.10f : 0f));
            }
        }

        /// <summary>
        /// Returns the amount of Health healed by Frenzied Regen based on the Rage used
        /// </summary>
        /// <returns></returns>
        public float getAmountHealed
        {
            get
            {
                // If using the glyph, then the player no longer self heals.
                // Instead they get healed for 40% more from Non-FR abilities
                if (guardianCombatState.Talents.GlyphOfFrenziedRegeneration)
                    return 0;
                else
                    return ((float)Math.Max(getAPFormula, getStamFormula) * (Rage / maxRage));
            }
        }

        private float increasedHealsFromGlyph = 0.40f;
        /// <summary>
        /// If the Player is using the Glyph of Frenzied Regeneration, then return the increased amount of heals 
        /// from non-FR abilities.
        /// </summary>
        /// <returns></returns>
        public float getIncreaseHealed()
        {
            if (guardianCombatState.Talents.GlyphOfFrenziedRegeneration)
                return ((1 + increasedHealsFromGlyph) * (1 + (guardianCombatState.Stats.Tier_14_4_piece ? 0.10f : 0f))) - 1f;
            else
                return 0;
        }

        public override float Formula()
        {
            return 0f;
        }

        public override string ToString()
        {
            return string.Format("{0}*AP Formula: {1}\nStamina Formula: {2}", getAmountHealed.ToString("n"), getAPFormula.ToString("n"), getStamFormula.ToString("n"));
        }
    }
}
