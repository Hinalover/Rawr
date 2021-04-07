using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GaurdianWeapon
    {
        private ItemInstance instance = new ItemInstance();
        private Item item = new Item();
        private StatsBear feralStats = new StatsBear();
        private CalculationOptionsBear calcOpts = new CalculationOptionsBear();
        private BossOptions bossOpts = new BossOptions();
        private DruidTalents talents = new DruidTalents();
        private float _dodge = 0;
        private float _parry = 0;
        private float _hit = 0;
        private float _physicalCrit = 0;
        private float _physicalHaste = 0;
        private float _mastery = 0;
        private float _attackpower = 0;
        private float _vengenceattackpower = 0;
        private float _spellpower = 0;
        private float _healingpower = 0;
        private float _movementspeed = 0;

        public GaurdianWeapon() { }
        public GaurdianWeapon(ItemInstance i, StatsBear stats, CalculationOptionsBear calcOpt, BossOptions bossOpt, DruidTalents talent, float Dodge, float Parry, float Hit, float Crit, float Haste, float Mastery, float AttackPower, float VengenceAP, float SpellPower, float HealingPower, float MovementSpeed)
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
            {
                instance = i;
                item = i.Item;
            }

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
            _vengenceattackpower = VengenceAP;
            _spellpower = SpellPower;
            _healingpower = HealingPower;
            _movementspeed = MovementSpeed;
        }

        public StatsBear Stats
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

        public float VengenceAttackPower
        {
            get
            {
                return _vengenceattackpower;
            }
            set
            {
                _vengenceattackpower = (float)value;
            }
        }

        public float SpellPower
        {
            get
            {
                return _spellpower;
            }
            set
            {
                _spellpower = (float)value;
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

        public float BossAttackSpeed
        {
            get
            {
                if (bossOpts.DefaultMeleeAttack != null)
                    return bossOpts.DefaultMeleeAttack.AttackSpeed;
                else
                    return 0;
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
        public float baseAttackSpeed
        {
            get
            {
                return 2.5f;
            }
        }
        public float hastedSpeed
        {
            get
            {
                return baseAttackSpeed / (1 + _physicalHaste);
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
            set
            {
                _dodge = (float)value;
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
            set
            {
                _parry = (float)value;
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
                return ((baseDamage / baseSpeed) + (_attackpower / 14.0f));
            }
        }

        public float MinDamage
        {
            get
            {
                return ((baseMinDamage / baseSpeed) + (_attackpower / 14.0f));
            }
        }

        public float MaxDamage
        {
            get
            {
                return ((baseMaxDamage / baseSpeed) + (_attackpower / 14.0f));
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

        public float VengenceWeaponDamage
        {
            get
            {
                return ((baseDamage / baseSpeed) + (_vengenceattackpower / 14.0f));
            }
        }

        public float VengenceMinDamage
        {
            get
            {
                return ((baseMinDamage / baseSpeed) + (_vengenceattackpower / 14.0f));
            }
        }

        public float VengenceMaxDamage
        {
            get
            {
                return ((baseMaxDamage / baseSpeed) + (_vengenceattackpower / 14.0f));
            }
        }

        public float VengenceMinimumDPS
        {
            get
            {
                return VengenceMinDamage * hastedSpeed;
            }
        }

        public float VengenceMaximumDPS
        {
            get
            {
                return VengenceMaxDamage * hastedSpeed;
            }
        }

        public float VengenceDPS
        {
            get
            {
                return VengenceWeaponDamage * hastedSpeed;
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

        #region Non-Vengence Calcs
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

        #region Vengence Calcs
        public float MinMinimumVengenceGlanceDamage
        {
            get
            {
                return VengenceMinDamage * _MinGlancing;
            }
        }

        public float MaxMinimumVengenceGlanceDamage
        {
            get
            {
                return VengenceMinDamage * _MaxGlancing;
            }
        }

        public float MinimumVengenceGlanceDamage
        {
            get
            {
                return (MinMinimumVengenceGlanceDamage + MaxMinimumVengenceGlanceDamage) / 2f;
            }
        }

        public float MinMaximumVengenceGlanceDamage
        {
            get
            {
                return VengenceMaxDamage * _MinGlancing;
            }
        }

        public float MaxMaximumVengenceGlanceDamage
        {
            get
            {
                return VengenceMaxDamage * _MaxGlancing;
            }
        }

        public float MaximumVengenceGlanceDamage
        {
            get
            {
                return (MinMaximumVengenceGlanceDamage + MaxMaximumVengenceGlanceDamage) / 2f;
            }
        }

        public float VengenceGlanceDamage
        {
            get
            {
                return (MinimumVengenceGlanceDamage + MaximumVengenceGlanceDamage) / 2f;
            }
        }
        #endregion
        #endregion

        public float SpellPowerFromWeapon
        {
            get
            {
                return (float)Math.Round((baseDPS / 2f) * 3.135670813f);
            }
        }

        /// <summary>
        /// Converts a Guardian Weapon to a Feral Weapon
        /// </summary>
        /// <returns></returns>
        public FeralWeapon toFeralWeapn()
        {
            CalculationOptionsFeral calcOptionFeral = new CalculationOptionsFeral();
            Stats statsconverter = (Stats)Stats;
            StatsFeral statsFeral = (StatsFeral)statsconverter;
            FeralWeapon feralWeapon = new FeralWeapon(instance, statsFeral, calcOptionFeral, bossOpts, talents, _dodge, _parry, _hit, _physicalCrit, _physicalHaste, _mastery, _attackpower, _healingpower, _movementspeed);

            return feralWeapon;
        }
    }
}
