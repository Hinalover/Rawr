using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralWeapon
    {
        public CharacterSlot hand;
        
        private Item item;
        private StatsFeral feralStats;
        private CalculationOptionsFeral calcOpts;
        private BossOptions bossOpts;
        private DruidTalents talents;
        private float expertise;

        public FeralWeapon(Item i, StatsFeral stats, CalculationOptionsFeral calcOpt, BossOptions bossOpt, DruidTalents talent, float Expertise, CharacterSlot Hand)
        {
            if (stats == null || calcOpts == null || !(hand == CharacterSlot.MainHand)) { return; }

            if (i == null)
            {
                // assume naked
                i = new Item();
                i.Speed = 1.0f;
                i.MinDamage = 1;
                i.MaxDamage = 1;
            }

            item = i;
            feralStats = stats;
            calcOpts = calcOpt;
            bossOpts = bossOpt;
            talents = talent;
            expertise = Expertise;
            hand = Hand;
        }

        public bool twohander
        {
            get
            {
                return (item.Slot == ItemSlot.TwoHand);
            }
        }

        public float effectiveExpertise
        {
            get
            {
                return expertise;
            }
            set
            {
                expertise = (float)value;
            }
        }

        public float baseSpeed
        {
            get
            {
                return item.Speed;
            }
        }

        public float baseDamage
        {
            get
            {
                return (float)(((item.MinDamage + item.MaxDamage) / 2f) + feralStats.WeaponDamage);
            }
        }

        #region Attack Speed
        public float hastedSpeed
        {
            get
            {
                return baseSpeed / (1 + feralStats.PhysicalHaste);
            }
        }
        #endregion

        #region Dodge
        public float chanceDodged
        {
            get
            {
                float baseDodged = StatConversion.WHITE_DODGE_CHANCE_CAP[bossOpts.Level - StatConversion.DEFAULTPLAYERLEVEL];
                float chance = baseDodged - StatConversion.GetDodgeParryReducFromExpertise(effectiveExpertise);
#if DEBUG
                if (Math.Min(Math.Max(chance, 0f), baseDodged) < 0)
                    throw new Exception("Chance to dodge out of range.");
#endif
                return Math.Min(Math.Max(chance, 0f), baseDodged);
            }
        }
        #endregion

        #region Parry
        public float chanceParried
        {
            get
            {
                float baseParried = StatConversion.WHITE_PARRY_CHANCE_CAP[bossOpts.Level - StatConversion.DEFAULTPLAYERLEVEL];
                float chance = baseParried - StatConversion.GetDodgeParryReducFromExpertise(effectiveExpertise);
#if DEBUG
                if (Math.Min(Math.Max(chance, 0f), baseParried) < 0)
                    throw new Exception("Chance to parry out of range.");
#endif
                return Math.Min(Math.Max(chance, 0f), baseParried);
            }
        }
        #endregion

        #region Miss
        public float chanceMissed
        {
            get
            {
                float baseMissed = StatConversion.WHITE_MISS_CHANCE_CAP[bossOpts.Level - StatConversion.DEFAULTPLAYERLEVEL];
                float chance = baseMissed - feralStats.PhysicalHit;
#if DEBUG
                if (Math.Min(Math.Max(chance, 0f), baseMissed) < 0)
                    throw new Exception("Chance to hit out of range.");
#endif
                return Math.Min(Math.Max(chance, 0f), baseMissed);
            }
        }
        #endregion

        #region Spell Miss
        public float chanceSpellMissed
        {
            get
            {
                float baseSpellMissed = StatConversion.SPELL_MISS_CHANCE_CAP[bossOpts.Level - StatConversion.DEFAULTPLAYERLEVEL];
                float chance = baseSpellMissed - feralStats.SpellHit;
#if DEBUG
                if (Math.Min(Math.Max(chance, 0f), baseSpellMissed) < 0)
                    throw new Exception("Chance to hit with spells out of range.");
#endif
                return Math.Min(Math.Max(chance, 0f), baseSpellMissed);
            }
        }
        #endregion

        #region White Damage
        // White damage per hit.  Basic white hits are use elsewhere.
        public float WeaponDamage
        {
            get
            {
                return ((baseDamage / baseSpeed) + (feralStats.AttackPower / 14.0f)) * baseSpeed;
            }
        }

        public float LowerBound
        {
            get
            {
                return WeaponDamage * 0.90f;
            }
        }

        public float UpperBound
        {
            get
            {
                return WeaponDamage * 1.10f;
            }
        }

        public float DPS
        {
            get
            {
                return WeaponDamage / hastedSpeed;
            }
        }
        #endregion
    }
}
