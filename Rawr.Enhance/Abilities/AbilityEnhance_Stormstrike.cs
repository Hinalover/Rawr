using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class AbilityEnhance_Stormstrike : AbilityEnhance_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public AbilityEnhance_Stormstrike()
        {
            baseInfo();
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            AbilityIndex = (int)EnhanceAbility2.Stormstrike;
            Name = "Stormstrike";
            SpellID = 17364;
            SpellIcon = "ability_shaman_stormstrike";

            ReqTalent = false;
            ReqMHWeapon = true;
            ReqOHWeapon = false;
            SwingsOHWeapon = true;
            IsSpell = false;

            Range = MELEE_RANGE;
            Area = 0f;
            AOE = false;
            Targets = 1f;
            Mana = 9.4f;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 0f;
            BaseWeaponDamageMultiplier = 3.75f;
            BaseSpellPowerMultiplier = 0f;
            BaseAttackPowerMultiplier = 0f;

            TriggersGCD = true;
            CastTime = INSTANT;
            Cooldown = 8f;
        }
    }
}
