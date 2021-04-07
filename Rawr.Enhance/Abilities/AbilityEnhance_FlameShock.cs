using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class AbilityEnhance_FlameShock : AbilityEnhance_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public AbilityEnhance_FlameShock()
        {
            baseInfo();
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            AbilityIndex = (int)EnhanceAbility2.FlameShock;
            Name = "Flame Shock";
            SpellID = 8050;
            SpellIcon = "spell_fire_flameshock";

            ReqTalent = false;
            ReqMHWeapon = false;
            ReqOHWeapon = false;
            SwingsOHWeapon = false;
            IsSpell = true;

            Range = 25f;
            Area = 0f;
            AOE = false;
            Targets = 1f;
            Mana = 11.9f;

            DamageType = ItemDamageType.Fire;
            BaseDamage = 1086f;
            BaseWeaponDamageMultiplier = 0f;
            BaseSpellPowerMultiplier = 44.9f;
            BaseAttackPowerMultiplier = 0f;

            TriggersGCD = true;
            CastTime = INSTANT;
            Cooldown = 6f;
        }
    }
}
