using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class AbilityFeral_TigersFury : AbilityFeral_Base
    {
        /// <summary>
        /// Increases physical damage done by 15% for 6 sec and instantly restores 60 energy. 
        /// Cannot be activated while Berserk (Cat) is active.
        /// </summary>
        public AbilityFeral_TigersFury()
        {
            CombatState = new FeralCombatState();
            baseInfo();
        }

        public AbilityFeral_TigersFury(FeralCombatState CState)
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
            Name = "Tiger's Fury";
            SpellID = 5217;
            SpellIcon = "ability_mount_jungletiger";
            druidForm = new DruidForm[] { DruidForm.Cat };

            Energy = 60f;
            ComboPoint = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0;
            Cooldown = 30f;
            Duration = 6f;
            AbilityIndex = (int)FeralAbility.TigersFury;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public const float DamageBonus = 0.15f;

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            this.MainHand = CState.MainHand;
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}
