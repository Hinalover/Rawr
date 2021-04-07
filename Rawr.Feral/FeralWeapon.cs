using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralWeapon
    {
        private Item item = new Item();
        private StatsFeral feralStats = new StatsFeral();
        private CalculationOptionsFeral calcOpts = new CalculationOptionsFeral();
        private BossOptions bossOpts = new BossOptions();
        private DruidTalents talents = new DruidTalents();
        private float _dodge = 0;
        private float _parry = 0;
        private float _hit = 0;
        private float _physicalCrit = 0;
        private float _physicalHaste = 0;
        private float _mastery = 0;
        private float _attackpower = 0;
        private float _healingpower = 0;
        private float _movementspeed = 0;

        public FeralWeapon() { }
        public FeralWeapon(ItemInstance i, StatsFeral stats, CalculationOptionsFeral calcOpt, BossOptions bossOpt, DruidTalents talent, float Dodge, float Parry, float Hit, float Crit, float Haste, float Mastery, float AttackPower, float HealingPower, float MovementSpeed)
        {
            if (stats == null || calcOpts == null) { return; }

            if (i == null)
            {
                // assume naked
                item = new Item()
                {
                    Type = ItemType.Polearm,
                    Speed = 1.0f,
                    MinDamage = 1,
                    MaxDamage = 1,
                };
            }
            else
                item = i.Item;

            feralStats = stats;
            calcOpts = calcOpt;
            bossOpts = bossOpt;
            talents = talent;
            _dodge = Dodge;
            _parry = Parry;
            _hit = Hit;
            _physicalCrit = Crit;
            _physicalHaste = Haste;
            _mastery = Mastery;
            _attackpower = AttackPower;
            _healingpower = HealingPower;
            _movementspeed = MovementSpeed;
        }

        public StatsFeral Stats
        {
            get
            {
                return feralStats;
            }
        }

        public float Hit
        {
            get
            {
                return _hit;
            }
            set
            {
                _hit = (float)value;
            }
        }

        public float Expertise
        {
            get
            {
                return _dodge;
            }
            set
            {
                _dodge = (float)value;
            }
        }

        public float CriticalStrike
        {
            get
            {
                return _physicalCrit;
            }
            set
            {
                _physicalCrit = (float)value;
            }
        }

        public float Haste
        {
            get
            {
                return _physicalHaste;
            }
            set
            {
                _physicalHaste = (float)value;
            }
        }

        public float Mastery
        {
            get
            {
                return _mastery;
            }
            set
            {
                _mastery = (float)value;
            }
        }

        public float AttackPower
        {
            get
            {
                return _attackpower;
            }
            set
            {
                _attackpower = (float)value;
            }
        }

        public float HealingPower
        {
            get
            {
                return _healingpower;
            }
            set
            {
                _healingpower = (float)value;
            }
        }

        public float MovementSpeed
        {
            get
            {
                return _movementspeed;
            }
            set
            {
                _movementspeed = (float)value;
            }
        }

        public float CritDamageMultiplier
        {
            get
            {
                return 2 + feralStats.CritBonusDamage;
            }
        }

        public int CharacterLevel
        {
            get
            {
                return (calcOpts.UseBossHandler ? 85 : calcOpts.CharacterLevel);
            }
        }

        public int TargetLevel
        {
            get
            {
                return (calcOpts.UseBossHandler ? 88 : calcOpts.TargetLevel);
            }
        }

        #region Base Weapon Damge
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

        public float baseMinDamage
        {
            get
            {
                return (float)(item.MinDamage + feralStats.WeaponDamage);
            }
        }

        public float baseMaxDamage
        {
            get
            {
                return (float)(item.MaxDamage + feralStats.WeaponDamage);
            }
        }

        public float baseDPS
        {
            get
            {
                return item.DPS;
            }
        }
        #endregion

        #region Attack Speed
        public float hastedSpeed
        {
            get
            {
                return 1 / (1 + _physicalHaste);
            }
        }
        #endregion

        #region Dodge
        public float chanceDodged
        {
            get
            {
                return _dodge;
            }
        }
        #endregion

        #region Parry
        public float chanceParried
        {
            get
            {
                return _parry;
            }
        }
        #endregion

        #region Miss
        public float chanceMissed
        {
            get
            {
                return _hit;
            }
        }
        #endregion

        #region Avoidance
        public float fromBehindAvoidance
        {
            get
            {
                return chanceMissed + chanceDodged;
            }
        }

        public float fromInfrontAvoidance
        {
            get
            {
                return chanceMissed + chanceDodged + chanceParried;
            }
        }
        #endregion

        #region Spell Miss
        public float chanceSpellMissed
        {
            get
            {
                float baseSpellMissed = StatConversion.SPELL_MISS_CHANCE_CAP[bossOpts.Level - StatConversion.DEFAULTPLAYERLEVEL];
                float chance = baseSpellMissed - (_hit + _dodge);
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
                return (((baseDamage / baseSpeed) * 2f) + (_attackpower / 14.0f));
            }
        }

        public float MinDamage
        {
            get
            {
                return ((baseMinDamage / baseSpeed) * 2f) + ((_attackpower / 14.0f));
            }
        }

        public float MaxDamage
        {
            get
            {
                return ((baseMaxDamage / baseSpeed) * 2f) + ((_attackpower / 14.0f));
            }
        }

        public float MinimumDPS
        {
            get
            {
                return MinDamage * hastedSpeed;
            }
        }

        public float MaximumDPS
        {
            get
            {
                return MaxDamage * hastedSpeed;
            }
        }

        public float DPS
        {
            get
            {
                return WeaponDamage * hastedSpeed;
            }
        }
        #endregion

        #region Glancing Blows
        public float ChanceToGlance
        {
            get
            {
                return (float)Math.Max(0, (TargetLevel - CharacterLevel) * 0.08f);
            }
        }

        private float _MaxGlancing
        {
            get
            {
                float delta_skill = (float)Math.Max(0, (TargetLevel - CharacterLevel) * 5);
                float delta_max = (float)Math.Max(0.20f, Math.Min((1.3 - (0.03 * delta_skill)), 0.99));
                return delta_max;
            }
        }

        private float _MinGlancing
        {
            get
            {
                float delta_skill = (float)Math.Max(0, (TargetLevel - CharacterLevel) * 5);
                float delta_min = (float)Math.Max(0.01f, Math.Min((1.4 - (0.05 * delta_skill)), 0.91));
                return delta_min;
            }
        }

        public float AverageGlancingPercent
        {
            get
            {
                return (_MinGlancing + _MaxGlancing) / 2f;
            }
        }

        public float MinMinimumGlanceDamage
        {
            get
            {
                return MinDamage * _MinGlancing;
            }
        }

        public float MaxMinimumGlanceDamage
        {
            get
            {
                return MinDamage * _MaxGlancing;
            }
        }

        public float MinimumGlanceDamage
        {
            get
            {
                return (MinMinimumGlanceDamage + MaxMinimumGlanceDamage) / 2f;
            }
        }

        public float MinMaximumGlanceDamage
        {
            get
            {
                return MaxDamage * _MinGlancing;
            }
        }

        public float MaxMaximumGlanceDamage
        {
            get
            {
                return MaxDamage * _MaxGlancing;
            }
        }

        public float MaximumGlanceDamage
        {
            get
            {
                return (MinMaximumGlanceDamage + MaxMaximumGlanceDamage) / 2f;
            }
        }

        public float GlanceDamage
        {
            get
            {
                return (MinimumGlanceDamage + MaximumGlanceDamage) / 2f;
            }
        }
        #endregion
    }
}
