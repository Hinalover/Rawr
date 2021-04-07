using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class AbilityEnhance_LightningBolt : AbilityEnhance_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public AbilityEnhance_LightningBolt()
        {
            baseInfo();
        }

        /// <summary>
        /// Base contruct of each ability. 
        /// Cut back on coding in constructors
        /// </summary>
        public void baseInfo()
        {
            AbilityIndex = (int)EnhanceAbility2.LightningBolt;
            Name = "Lightning Bolt";
            SpellID = 403;
            SpellIcon = "spell_nature_lightning";

            ReqTalent = false;
            ReqMHWeapon = false;
            ReqOHWeapon = false;
            SwingsOHWeapon = false;
            IsSpell = true;

            Range = 30f;
            Area = 0f;
            AOE = false;
            Targets = 1f;
            Mana = 7.1f;

            DamageType = ItemDamageType.Nature;
            MinDamage = 1099f;
            MaxDamage = 1255f;
            BaseWeaponDamageMultiplier = 0f;
            BaseSpellPowerMultiplier = 68.4f;
            BaseAttackPowerMultiplier = 0f;

            TriggersGCD = true;
            CastTime = 2.5f;
            Cooldown = 0f;
        }
    }
}
