using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_Berserk : AbilityFeral_Base
    {
        /// <summary>
        /// When used in Cat Form, reduces the cost of all Cat Form abilities by 50% and lasts 15 sec.
        /// </summary>
        public AbilityFeral_Berserk()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_Berserk(FeralCombatState CState)
        {
            CombatState = CState;
            baseInfo();
            UpdateCombatState(CombatState);
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        private void baseInfo()
        {
            Name = "Berserk";
            SpellID = 106952;
            SpellIcon = "ability_druid_berserk";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear };

            Energy = 0f;
            ComboPoint = 0f;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f);
            Duration = 15f + (CombatState.Stats.Tier_12_4pc ? 3f : 0); // Estimating average for T12 4-piece //+ (CombatState.Talents.GlyphOfBerserk ? 10f : 0f);
            AbilityIndex = (int)FeralAbility.Berserk;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public const float CostReduction = 0.50f;

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
            //CombatState.BerserkUptime = true;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}