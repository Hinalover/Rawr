using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_TigersFury : AbilityFeral_Base
    {
        /// <summary>
        /// Increases physical damage done by 15% for 6 sec.
        /// </summary>
        public AbilityFeral_TigersFury(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Savage Roar";
            SpellID = 52610;
            SpellIcon = "ability_druid_skinteeth";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = (CombatState.Talents.KingOfTheJungle > 0 ? (-20 * CombatState.Talents.KingOfTheJungle) : 0f);
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0;
            Cooldown = (30f - (CombatState.Talents.GlyphOfTigersFury ? 3f : 0f)) * 1000f;
            Duration = 6f * 1000f;
            AbilityIndex = (int)FeralAbility.TigersFury;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            // Cannot activate Tigers Fury while Berserk is up
            if (!CombatState.BerserkUptime)
            {
                if (CombatState.TigersFuryUptime)
                    CombatState.Stats.MaxEnergy = 100f + (CombatState.Talents.PrimalMadness * 10f);
                else
                    CombatState.Stats.MaxEnergy = 100f;
                CombatState.TigersFuryUptime = true;
            }
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
