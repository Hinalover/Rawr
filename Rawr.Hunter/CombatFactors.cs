using System;

namespace Rawr.Hunter {
    public class CombatFactors {
        public CombatFactors(Character character, StatsHunter stats, CalculationOptionsHunter calcOpts, BossOptions bossOpts)
        {
            Char = character;
            if (Char != null)
            {
                //JA:Added criteria to check that Main Hand Weapon is a ranged weapon, otherwise something has gone horribly wrong given the removal of the range slot
                //Original line below read: if (Char.MainHand != null)
                if (Char.MainHand != null && (Char.MainHand.Type == ItemType.Bow || Char.MainHand.Type == ItemType.Gun|| Char.MainHand.Type == ItemType.Crossbow))
                    RW = Char.MainHand.Item;
                else
                {
                    //JA: TODO: Don't know enough about expected functionality to know if when we default RW to Knuckles and not change the UI to show no weapon, do we need to update UI here as well?
                    RW = new Knuckles();
                }

                if (Char.HunterTalents != null)
                    Talents = Char.HunterTalents;
                else
                    Talents = new HunterTalents();
            }
            CalcOpts = (calcOpts == null ? new CalculationOptionsHunter() : calcOpts);
            BossOpts = (bossOpts == null ? new BossOptions() : bossOpts);
            StatS = stats;
            InvalidateCache();
            // Optimizations
            
            //Set_c_values();
        }

        private void Set_c_values()
        {
            _c_rwItemType = RW.Type;
            _c_rwItemSpeed = RW.Speed;
            useRW = _useRW; // public variable gets set once

            _c_ymiss = YwMissChance;
            _c_wmiss = WhMissChance;
            _c_rwdodge = RwDodgeChance;
            _c_rwparry = RwParryChance;
            _c_rwblock = RwBlockChance;
            _c_rwwcrit = RwWhCritChance;
            _c_rwycrit = RwYwCritChance;
            _c_glance = GlanceChance;
        }
        #region Global Variables
        private StatsHunter _Stats;
        public StatsHunter StatS { get { return _Stats; } set { _Stats = value; } }
        private HunterTalents Talents;
        public CalculationOptionsHunter CalcOpts { get; private set; }
        public BossOptions BossOpts { get; private set; }
        public Character Char { get; private set; }
        /// <summary>The character's Ranged Weapon</summary>
        public Item RW { get; private set; }
        // Optimizations
        public float _c_ymiss { get; private set; }
        public float _c_wmiss { get; private set; }
        
        public ItemType _c_rwItemType { get; private set; }

        public float _c_rwItemSpeed { get; private set; }

        public float _c_rwdodge { get; private set; }
        public float _c_rwparry { get; private set; }
        public float _c_rwblock { get; private set; }
        public float _c_rwwcrit { get; private set; }
        public float _c_rwycrit { get; private set; }
        public float _c_glance { get; private set; }
        #endregion

        public bool useRW; private bool _useRW { get { return RW != null && _c_rwItemSpeed > 0; } }

        public void InvalidateCache() {
            _DamageBonus = _DamageReduction = _BonusWhiteCritDmg = _RWSpeed = _TotalHaste = -1f;
            Set_c_values();
        }

        #region Weapon Damage Calcs
        #region Major Damage Factors
        
        private float _DamageBonus = -1f;
        public float DamageBonus {
            get {
                               // General Bonuses
                if (_DamageBonus == -1f) {
                    _DamageBonus = (1f + StatS.BonusDamageMultiplier)
                                 * (1f + StatS.BonusPhysicalDamageMultiplier);
                    // Talents
                }
                return _DamageBonus;
            }
        }

        private float _DamageReduction = -1f;
        public float DamageReduction {
            get {
                if (_DamageReduction == -1f) {
                    float arpenBuffs = 0.0f;
                    _DamageReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, BossOpts.Armor, StatS.TargetArmorReduction, arpenBuffs));
                }
                return _DamageReduction;
            }
        }

        public float HealthBonus { get { return 1f + StatS.BonusHealingReceived; } }
        #endregion
        #region Weapon Damage
        public float NormalizedRwWeaponDmg { get { return useRW ? CalcNormalizedWeaponDamage(RW) : 0f; } }
        private float CalcNormalizedWeaponDamage(Item weapon) {
            return weapon.Speed * weapon.DPS + StatS.RangedAttackPower / 14f * 2.8f + StatS.WeaponDamage;
        }
        public float AvgRwWeaponDmgUnhasted 
        { 
            get 
            { 
                return (useRW ? (StatS.RangedAttackPower / 14f + RW.DPS) * _c_rwItemSpeed + StatS.WeaponDamage : 0f); 
            } 
        }

        /*public float AvgRwWeaponDmg(float speed) {       return (useMH ? (StatS.AttackPower / 14f + MH.DPS) * speed + StatS.WeaponDamage : 0f); }*/
        #endregion
        #region Weapon Crit Damage
        private float _BonusWhiteCritDmg = -1f;
        public float BonusWhiteCritDmg {
            get {
                if (_BonusWhiteCritDmg == -1f) {
                    _BonusWhiteCritDmg = (2f * (1f + StatS.BonusCritDamageMultiplier) - 1f);
                }
                return _BonusWhiteCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg; } }
        #endregion
        #region Weapon Blocked Damage
        public float ReducWhBlockedDmg {
            get {
                return 1.00f;// 100% damage, we don't get blocked
            }
        }
        public float ReducYwBlockedDmg { get { return ReducWhBlockedDmg; } }
        #endregion
        #region Weapon Glanced Damage
        public float ReducWhGlancedDmg {
            get {
                return 1.00f; // 100% damage, we don't get glanced
            }
        }
        #endregion
        #region Speed
        private float _TotalHaste = -1f;
        public float TotalHaste {
            get {
                if (_TotalHaste == -1f)
                    _TotalHaste = 1f + StatS.RangedHaste; // All haste is calc'd into PhysicalHaste in GetCharacterStats
                return _TotalHaste;
            }
        }
        private float _RWSpeed = -1f;
        public float RWSpeed {
            get {
                if (_RWSpeed == -1f) { _RWSpeed = useRW ? _c_rwItemSpeed / TotalHaste : 0f; }
                return _RWSpeed;
            }
        }
        #endregion
        #endregion
        #region Attack Table
        private AttackTable _AttackTableBasicRW;
        public AttackTable AttackTableBasicRW {
            get {
                if (_AttackTableBasicRW == null) {
                    _AttackTableBasicRW = new AttackTable(Char, StatS, this, CalcOpts, Skills.Ability.NULL, false, false);
                }
                return _AttackTableBasicRW;
            }
        }

        

        #region Miss
        private float MissPrevBonuses {
            get
            {
                return 0f;
            }
        }
        private float WhMissCap { get { return StatConversion.WHITE_MISS_CHANCE_CAP[BossOpts.Level - Char.Level]; } }
        private float YwMissCap { get { return StatConversion.YELLOW_MISS_CHANCE_CAP[BossOpts.Level - Char.Level]; } }
        private float WhMissChance { get { return Math.Max(0f, WhMissCap - MissPrevBonuses); } }
        private float YwMissChance { get { return Math.Max(0f, YwMissCap - MissPrevBonuses); } }
        #endregion
        #region Dodge
        private float DodgeChanceCap { get { return 0f; } }//{ return StatConversion.WHITE_DODGE_CHANCE_CAP[Math.Max(BossOpts.Level - Char.Level,0)]; } }
        private float RwDodgeChance { get { return 0f; } }//{ return Math.Max(0f, DodgeChanceCap); } }
        #endregion
        #region Parry
        private float ParryChanceCap { get { return 0f; } }// StatConversion.WHITE_PARRY_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]; } }
        private float RwParryChance {
            get {
                return 0f;

            }
        }
        #endregion
        #region Glance
        private float GlanceChance { get { return 0f; } }//StatConversion.WHITE_GLANCE_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]; } }
        #endregion
        #region Block
        // Hunter Dev Team has decided to remove Block from the Attack Table
        // until evidence can show specific bosses that do block
        private float BlockChanceCap { get { return 0f; } }//StatConversion.WHITE_BLOCK_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]; } }
        private float RwBlockChance { get { return 0f; } }//Math.Max(0f, CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) : BlockChanceCap); } }
        #endregion
        #region Crit
        private float RwWhCritChance {
            get {
                if (!useRW) { return 0f; }
                return StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating) /*+
                 ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f)*/;
                //return crit;
            }
        }
        private float RwYwCritChance {
            get {
                if (!useRW) { return 0f; }
                return ((StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating)) /*+
                       ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f)*/)
                        * (1f - _c_ymiss - _c_rwdodge);
            }
        }
        #endregion
        #region Chance of Hitting
        // White
        private float ProbMhWhiteHit   { get { return         1f - _c_wmiss - _c_rwdodge - _c_rwparry - _c_rwwcrit; } }
        private float ProbMhWhiteLand  { get { return         1f - _c_wmiss - _c_rwdodge - _c_rwparry; } }
        // Yellow (Doesn't Glance and has different MissChance Cap)
        private float ProbMhYellowHit  { get { return         1f - _c_ymiss - _c_rwdodge - _c_rwparry - _c_rwblock - _c_rwycrit; } }
        private float ProbMhYellowLand { get { return         1f - _c_ymiss - _c_rwdodge - _c_rwparry - _c_rwblock; } }
        #endregion
        #endregion
        #region Other
        private class Knuckles : Item
        {
            public Knuckles()
            {
                Speed = 0f;
                MaxDamage = 0;
                MinDamage = 0;
                Slot = ItemSlot.Ranged;
            }
        }
        #endregion
        #region Attackers Stats against you
        private float LevelModifier { get { return (BossOpts.Level - Char.Level) * 0.002f; } }
        private float NPC_CritChance { get { return Math.Max(0f, 0.05f + LevelModifier - StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Crit, BossOpts.Level)); } }
        #endregion
    }
}
