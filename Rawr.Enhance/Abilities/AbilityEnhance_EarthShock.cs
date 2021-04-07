using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class AbilityEnhance_EarthShock : AbilityEnhance_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public AbilityEnhance_EarthShock()
        {
            baseInfo();
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            AbilityIndex = (int)EnhanceAbility2.EarthShock;
            Name = "Earth Shock";
            SpellID = 8042;
            SpellIcon = "spell_nature_earthshock";

            ReqTalent = false;
            ReqMHWeapon = false;
            ReqOHWeapon = false;
            SwingsOHWeapon = false;
            IsSpell = true;

            Range = 25f;
            Area = 0f;
            AOE = false;
            Targets = 1f;
            Mana = 14.4f;

            DamageType = ItemDamageType.Nature;
            MinDamage = 2035f;
            MaxDamage = 2249f;
            BaseWeaponDamageMultiplier = 0f;
            BaseSpellPowerMultiplier = 58.1f;
            BaseAttackPowerMultiplier = 0f;

            TriggersGCD = true;
            CastTime = INSTANT;
            Cooldown = 6f;
        }
    }
}
