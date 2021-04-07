using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class AbilityEnhance_LavaLash : AbilityEnhance_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public AbilityEnhance_LavaLash()
        {
            baseInfo();
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            AbilityIndex = (int)EnhanceAbility2.LavaLash;
            Name = "Lava Lash";
            SpellID = 60103;
            SpellIcon = "ability_shaman_lavalash";

            ReqTalent = false;
            ReqMHWeapon = false;
            ReqOHWeapon = true;
            SwingsOHWeapon = true;
            IsSpell = false;

            Range = MELEE_RANGE;
            Area = 0f;
            AOE = false;
            Targets = 1f;
            Mana = 4.0f;

            DamageType = ItemDamageType.Fire;
            BaseDamage = 0f;
            BaseWeaponDamageMultiplier = 2.50f;
            BaseSpellPowerMultiplier = 0f;
            BaseAttackPowerMultiplier = 0f;

            TriggersGCD = true;
            CastTime = INSTANT;
            Cooldown = 10f;
        }
    }
}
