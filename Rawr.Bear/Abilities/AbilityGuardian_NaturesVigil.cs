using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class AbilityGuardian_NaturesVigil : AbilityGuardian_Base
    {
         /// <summary>
        /// Increases all damage and healing done by 20% for 30 sec.  While active, all single-target 
        /// healing spells also damage a nearby enemy target for 25% of the healing done, and all 
        /// single-target damage spells and abilities also heal a nearby friendly target for 25% of the damage done.
        /// </summary>
        public AbilityGuardian_NaturesVigil()
        {
            guardianCombatState = new GuardianCombatState();
            baseInfo();
        }

        public AbilityGuardian_NaturesVigil(GuardianCombatState CState)
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
            Name = "Nature's Vigil";
            SpellID = 124974;
            SpellIcon = "achievement_zone_feralas";
            druidForm = new DruidForm[] { DruidForm.Cat, DruidForm.Bear, DruidForm.Boomkin, DruidForm.Caster };

            Rage = 0;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;

            TriggersGCD = false;
            CastTime = 0f;
            Cooldown = (3f * 60f);
            Duration = 30f;
            AbilityIndex = (int)FeralAbility.NaturesVigil;
            Range = MELEE_RANGE;
            AOE = false;
        }

        public readonly float DamageBonus = 0.20f;
        public readonly float HealingBonus = 0.25f;

        public override void UpdateCombatState(GuardianCombatState CState)
        {
            base.UpdateCombatState(CState);
        }

        public override float Formula()
        {
            return 0f;
        }
    }
}