using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_Berserk : AbilityFeral_Base
    {
        /// <summary>
        /// Your Lacerate periodic damage has a 50% chance to refresh the cooldown 
        /// of your Mangle (Bear) ability and make it cost no rage.  
        /// 
        /// In addition, when activated this ability causes your Mangle (Bear) ability
        /// to hit up to 3 targets and have no cooldown, and reduces the energy cost of 
        /// all your Cat Form abilities by 50%.  Lasts 15 sec.  You cannot use Tiger's 
        /// Fury while Berserk is active.
        /// </summary>
        public AbilityFeral_Berserk(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Berserk";
            SpellID = 50334;
            SpellIcon = "ability_druid_berserk";
            druidForm = new DruidForm[]{ DruidForm.Cat, DruidForm.Bear };

            Energy = 0f;
            ComboPoint = 0f;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f) * 1000f;
            Duration = (15f + (CombatState.Talents.GlyphOfBerserk ? 10f : 0f)) * 1000f;
            AbilityIndex = (int)FeralAbility.Berserk;
            Range = MELEE_RANGE;
            AOE = false;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            CombatState.BerserkUptime = true;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}