using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public enum CharacterClass
    {
        None = 0,
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        Monk = 10,
        Druid = 11,
        Player_Special_Scale = 12,
        Player_Special_Scale2 = 13,
        Player_Special_Scale3 = 14,
        Player_Special_Scale4 = 15,
        Unknown = 16,
    }

    public enum CharacterFaction
    {
        Alliance = 1,
        Horde = 2,
    }

    public enum CharacterRegion
    {
        US, EU, KR, TW, CN
    }

    public enum CharacterRace
    {
        None             =  0, // Used for the Race chart
        Human            =  1,
        Orc              =  2,
        Dwarf            =  3,
        /// <summary>
        /// Night Elf
        /// </summary>
        NightElf         =  4,
        Undead           =  5,
        Tauren           =  6,
        Gnome            =  7,
        Troll            =  8,
        Goblin           =  9, // Cataclysm
        /// <summary>
        /// Blood Elf
        /// </summary>
        BloodElf         = 10, // Burning Crusade
        Draenei          = 11, // Burning Crusade
        //FelOrc           = 12,
        // Naga            = 13,
        // Broken          = 14,
        // Skeleton        = 15,
        // Vrykul          = 16,
        // Tuskarr         = 17,
        // ForestTroll     = 18,
        // Taunka          = 19,
        // Skeleton        = 20,
        // IceTroll        = 21,
        Worgen           = 22, // Cataclysm
        // Gilnean         = 23,
        Pandaren           = 24,
        /// <summary>
        /// Pandaren - Alliance
        /// </summary>
        PandarenAlliance = 25, // Mists of Pandaria
        /// <summary>
        /// Pandaren - Horde
        /// </summary>
        PandarenHorde    = 26, // Mists of Pandaria
        Unknown = 27,
    }

    public enum CharacterSlot
    {
        None = -1,
        //Projectile = 0,
        Head = 1,
        Neck = 2,
        Shoulders = 3,
        Chest = 4,
        Waist = 5,
        Legs = 6,
        Feet = 7,
        Wrist = 8,
        Hands = 9,
        Finger1 = 10,
        Finger2 = 11,
        Trinket1 = 12,
        Trinket2 = 13,
        Back = 14,
        MainHand = 15,
        OffHand = 16,
        //Ranged = 17,
        //ProjectileBag = 18,
        Tabard = 19,
        Shirt = 20,

        Gems = 100,
        Metas = 101,
        Cogwheels = 102,
        Hydraulics = 103,
        
        AutoSelect = 1000,
    }

    public enum ItemAvailability
    {
        NotAvailable,
        Available,
        AvailableWithEnchantRestrictions,
        RegemmingAllowed,
        RegemmingAllowedWithEnchantRestrictions
    }

    public enum DKRune
    {
        None = 0,
        Blood = 1,
        Unholy = 4,
        Blood_Unholy = 5,
        Frost = 16,
        Blood_Frost = 17,
        Unholy_Frost = 20,
        Blood_Unholy_Frost = 21,
        Unknown_64 = 64,
    }

    public enum ResourceType
    {
        Health = -2,
        Mana = 0,
        Rage = 1,
        Focus = 2,
        Energy = 3,
        MonkEnergy = 4,
        Rune = 5,
        RunicPower = 6,
        SoulShard = 7,
        HolyPower = 9,
        Chi = 12,
        ShadowOrbs = 13,
        BurningEmbers = 14,
        DemonicFury = 15,
        Max = 16,
        None = 10000,
    }

    public enum ItemDamageType
    {
        None = 0x00,
        Physical = 0x01,        // Physical
        Holy = 0x02,            // Holy
        Holystrike = 0x03,      // Holy + Physical
        Fire = 0x04,            // Fire
        Flamestrike = 0x05,     // Fire + Physical
        Holyfire = 0x06,        // Fire + Holy
        Nature = 0x08,          // Nature
        Stormstrike = 0x09,     // Nature + Physical
        Holystorm = 0x0a,       // Nature + Holy
        Firestorm = 0x0c,       // Nature + Fire
        Frost = 0x10,           // Frost
        Froststrike = 0x11,     // Frost + Physical
        Holyfrost = 0x12,       // Frost + Holy
        Frostfire = 0x14,       // Frost + Fire
        Froststorm = 0x18,      // Frost + Nature
        Elemental = 0x1c,       // Frost + Nature + Fire
        Shadow = 0x20,          // Shadow
        Shadowstrike = 0x21,    // Shadow + Physical
        Shadowlight = 0x22,     // Shadow + Holy
        Shadowflame = 0x24,     // Shadow + Fire
        Shadowstorm = 0x28,     // Shadow + Nature
        Shadowfrost = 0x30,     // Shadow + Frost
        Arcane = 0x40,          // Arcane
        Spellstrike = 0x41,     // Arcane + Physical
        Divine = 0x42,          // Arcane + Holy
        Spellfire = 0x44,       // Arcane + Fire
        Spellstorm = 0x48,      // Arcane + Nature
        Spellfrost = 0x50,      // Arcane + Frost
        Spellfrostfire = 0x54,  // Arcane + Frost + Fire
        Spellshadow = 0x60,     // Arcane + Shadow
        Chromatic = 0x7c,       // Fire + Nature + Frost + Shadow + Arcane
        Magic = 0x7e,           // Holy + Fire + Nature + Frost + Shadow + Arcane
        Chaos = 0x7f,           // Physical + Holy + Fire + Nature + Frost + Shadow + Arcane
    }

    [Flags]
    public enum ItemDamageFlags
    {
        None = 0,
        Physical = 1 << ItemDamageType.Physical,
        Holy = 1 << ItemDamageType.Holy,
        Holystrike = 1 << ItemDamageType.Holystrike,
        Fire = 1 << ItemDamageType.Fire,
        Flamestrike = 1 << ItemDamageType.Flamestrike,
        Holyfire = 1 << ItemDamageType.Holyfire,
        Nature = 1 << ItemDamageType.Nature,
        Stormstrike = 1 << ItemDamageType.Stormstrike,
        Holystorm = 1 << ItemDamageType.Holystorm,
        Firestorm = 1 << ItemDamageType.Firestorm,
        Frost = 1 << ItemDamageType.Frost,
        Froststrike = 1 << ItemDamageType.Froststrike,
        Holyfrost = 1 << ItemDamageType.Holyfrost,
        Frostfire = 1 << ItemDamageType.Frostfire,
        Froststorm = 1 << ItemDamageType.Froststorm,
        Elemental = 1 << ItemDamageType.Elemental,
        Shadow = 1 << ItemDamageType.Shadow,
        Shadowstrike = 1 << ItemDamageType.Shadowstrike,
        Shadowlight = 1 << ItemDamageType.Shadowlight,
        Shadowflame = 1 << ItemDamageType.Shadowflame,
        Shadowstorm = 1 << ItemDamageType.Shadowstorm,
        Shadowfrost = 1 << ItemDamageType.Shadowfrost,
        Arcane = 1 << ItemDamageType.Arcane,
        Spellstrike = 1 << ItemDamageType.Spellstrike,
        Divine = 1 << ItemDamageType.Divine,
        Spellfire = 1 << ItemDamageType.Spellfire,
        Spellstorm = 1 << ItemDamageType.Spellstorm,
        Spellfrost = 1 << ItemDamageType.Spellfrost,
        Spellfrostfire = 1 << ItemDamageType.Spellfrostfire,
        Spellshadow = 1 << ItemDamageType.Spellshadow,
        Chromatic = 1 << ItemDamageType.Chromatic,
        Magic = 1 << ItemDamageType.Magic,
        Chaos = 1 << ItemDamageType.Chaos,
    }

    public enum ItemQuality
    {
        Temp = -1,
        Poor = 0,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact,
        Heirloom
    }

    public enum ItemFaction
    {
        Neutral = 0,
        Alliance = 1,
        Horde = 2,
    }

    public enum ItemType
    {
        None,

        Cloth,
        Leather,
        Mail,
        Plate,

        Dagger,
        FistWeapon,
        OneHandAxe,
        TwoHandAxe,
        OneHandMace,
        TwoHandMace,
        OneHandSword,
        TwoHandSword,
        Polearm,
        Staff,
        Shield,

        Bow,
        Crossbow,
        Gun,
        Wand,
        Thrown,

        Idol,
        Libram,
        Totem,
        Sigil,
        Relic, // The 4 above all convert to Relic in Cata

        Arrow,
        Bullet,
        Quiver,
        AmmoPouch
    }

    public enum ItemSlot
    {
        None,

        Head,
        Neck,
        Shoulders,
        Back,
        Chest,
        Shirt,
        Tabard,
        Wrist,
        Hands,
        Waist,
        Legs,
        Feet,
        Finger,
        Trinket,
        OneHand,
        TwoHand,
        MainHand,
        OffHand,
        Ranged,
        Projectile,
        ProjectileBag,

        Meta,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Prismatic,

        // New in Cata for Engineers
        Cogwheel,
        Hydraulic,

        //None = 0,
        //Head = 1,
        //Neck = 2,
        //Shoulders = 3,
        //Shirt = 4,
        //Chest = 5,
        //Waist = 6,
        //Legs = 7,
        //Feet = 8,
        //Wrist = 9,
        //Hands = 10,
        //Finger = 11,
        //Trinket = 12,
        //OneHand = 13,
        //OffHand = 14,
        //Ranged = 15,
        //Back = 16,
        //TwoHand = 17,

        //Tabard = 19,
        //Robe = 20,
        //MainHand = 21,
        //OffHandB = 22,


        //Thrown = 25,
        //Wand = 26,
        //Relic = 28,

        //Weapon = 97,

        //Meta = 101,
        //Red = 102,
        //Orange = 103,
        //Yellow = 104,
        //Green = 105,
        //Prismatic = 106,
        //Purple = 107,
        //Blue = 108
    }

    public enum BuffGroup
    {
        All = 0,
        Current,
        Food,
        ElixirsAndFlasks,
        Scrolls,
        Potions,
        RaidBuffs,
        RaidDeBuffs
    }

    public enum Profession
    { // values are based upon what the armory returns for those profession id's
        None = 0,
        Alchemy = 171,
        Enchanting = 333,
        Engineering = 202,
        Herbalism = 182,
        Inscription = 773,
        Jewelcrafting = 755,
        Leatherworking = 165,
        Skinning = 393,
        Tailoring = 197,
        Blacksmithing = 164,
        Mining = 186,
    }

    public enum AvailableItemIDModifiers {
        Enchants   =       10000,
        Reforges   =     1000000,
        Tinkerings =   100000000,
    }

    public enum Specialization
    {
        SPEC_NONE = 0,
        SPEC_PET = 1,
        PET_FEROCITY = 74,
        PET_TENACITY = 81,
        PET_CUNNING = 79,
        WARRIOR_ARMS = 71,
        WARRIOR_FURY = 72,
        WARRIOR_PROTECTION = 73,
        PALADIN_HOLY = 65,
        PALADIN_PROTECTION = 66,
        PALADIN_RETRIBUTION = 70,
        HUNTER_BEAST_MASTERY = 253,
        HUNTER_MARKSMANSHIP = 254,
        HUNTER_SURVIVAL = 255,
        ROGUE_ASSASSINATION = 259,
        ROGUE_COMBAT = 260,
        ROGUE_SUBTLETY = 261,
        PRIEST_DISCIPLINE = 256,
        PRIEST_HOLY = 257,
        PRIEST_SHADOW = 258,
        DEATH_KNIGHT_BLOOD = 250,
        DEATH_KNIGHT_FROST = 251,
        DEATH_KNIGHT_UNHOLY = 252,
        SHAMAN_ELEMENTAL = 262,
        SHAMAN_ENHANCEMENT = 263,
        SHAMAN_RESTORATION = 264,
        MAGE_ARCANE = 62,
        MAGE_FIRE = 63,
        MAGE_FROST = 64,
        WARLOCK_AFFLICTION = 265,
        WARLOCK_DEMONOLOGY = 266,
        WARLOCK_DESTRUCTION = 267,
        MONK_BREWMASTER = 268,
        MONK_MISTWEAVER = 270,
        MONK_WINDWALKER = 269,
        DRUID_BALANCE = 102,
        DRUID_FERAL = 103,
        DRUID_GUARDIAN = 104,
        DRUID_RESTORATION = 105,
    }

    public enum EffectType
    {
        E_NONE = 0,
        E_INSTAKILL = 1,
        E_SCHOOL_DAMAGE = 2,
        E_DUMMY = 3,
        E_PORTAL_TELEPORT = 4,
        E_TELEPORT_UNITS = 5,
        E_APPLY_AURA = 6,
        E_ENVIRONMENTAL_DAMAGE = 7,
        E_POWER_DRAIN = 8,
        E_HEALTH_LEECH = 9,
        E_HEAL = 10,
        E_BIND = 11,
        E_PORTAL = 12,
        E_RITUAL_BASE = 13,
        E_RITUAL_SPECIALIZE = 14,
        E_RITUAL_ACTIVATE_PORTAL = 15,
        E_QUEST_COMPLETE = 16,
        E_WEAPON_DAMAGE_NOSCHOOL = 17,
        E_RESURRECT = 18,
        E_ADD_EXTRA_ATTACKS = 19,
        E_DODGE = 20,
        E_EVADE = 21,
        E_PARRY = 22,
        E_BLOCK = 23,
        E_CREATE_ITEM = 24,
        E_WEAPON = 25,
        E_DEFENSE = 26,
        E_PERSISTENT_AREA_AURA = 27,
        E_SUMMON = 28,
        E_LEAP = 29,
        E_ENERGIZE = 30,
        E_WEAPON_PERCENT_DAMAGE = 31,
        E_TRIGGER_MISSILE = 32,
        E_OPEN_LOCK = 33,
        E_SUMMON_CHANGE_ITEM = 34,
        E_APPLY_AREA_AURA_PARTY = 35,
        E_LEARN_SPELL = 36,
        E_SPELL_DEFENSE = 37,
        E_DISPEL = 38,
        E_LANGUAGE = 39,
        E_DUAL_WIELD = 40,
        E_JUMP = 41,
        E_JUMP2 = 42,
        E_TELEPORT_UNITS_FACE_CASTER = 43,
        E_SKILL_STEP = 44,
        E_ADD_HONOR = 45,
        E_SPAWN = 46,
        E_TRADE_SKILL = 47,
        E_STEALTH = 48,
        E_DETECT = 49,
        E_TRANS_DOOR = 50,
        E_FORCE_CRITICAL_HIT = 51,
        E_GUARANTEE_HIT = 52,
        E_ENCHANT_ITEM = 53,
        E_ENCHANT_ITEM_TEMPORARY = 54,
        E_TAMECREATURE = 55,
        E_SUMMON_PET = 56,
        E_LEARN_PET_SPELL = 57,
        E_WEAPON_DAMAGE = 58,
        E_CREATE_RANDOM_ITEM = 59,
        E_PROFICIENCY = 60,
        E_SEND_EVENT = 61,
        E_POWER_BURN = 62,
        E_THREAT = 63,
        E_TRIGGER_SPELL = 64,
        E_APPLY_AREA_AURA_RAID = 65,
        E_RESTORE_ITEM_CHARGES = 66,
        E_HEAL_MAX_HEALTH = 67,
        E_INTERRUPT_CAST = 68,
        E_DISTRACT = 69,
        E_PULL = 70,
        E_PICKPOCKET = 71,
        E_ADD_FARSIGHT = 72,
        E_UNTRAIN_TALENTS = 73,
        E_APPLY_GLYPH = 74,
        E_HEAL_MECHANICAL = 75,
        E_SUMMON_OBJECT_WILD = 76,
        E_SCRIPT_EFFECT = 77,
        E_ATTACK = 78,
        E_SANCTUARY = 79,
        E_ADD_COMBO_POINTS = 80,
        E_CREATE_HOUSE = 81,
        E_BIND_SIGHT = 82,
        E_DUEL = 83,
        E_STUCK = 84,
        E_SUMMON_PLAYER = 85,
        E_ACTIVATE_OBJECT = 86,
        E_WMO_DAMAGE = 87,
        E_WMO_REPAIR = 88,
        E_WMO_CHANGE = 89,
        E_KILL_CREDIT = 90,
        E_THREAT_ALL = 91,
        E_ENCHANT_HELD_ITEM = 92,
        E_BREAK_PLAYER_TARGETING = 93,
        E_SELF_RESURRECT = 94,
        E_SKINNING = 95,
        E_CHARGE = 96,
        E_SUMMON_ALL_TOTEMS = 97,
        E_KNOCK_BACK = 98,
        E_DISENCHANT = 99,
        E_INEBRIATE = 100,
        E_FEED_PET = 101,
        E_DISMISS_PET = 102,
        E_REPUTATION = 103,
        E_SUMMON_OBJECT_SLOT1 = 104,
        E_SUMMON_OBJECT_SLOT2 = 105,
        E_SUMMON_OBJECT_SLOT3 = 106,
        E_SUMMON_OBJECT_SLOT4 = 107,
        E_DISPEL_MECHANIC = 108,
        E_SUMMON_DEAD_PET = 109,
        E_DESTROY_ALL_TOTEMS = 110,
        E_DURABILITY_DAMAGE = 111,
        E_112 = 112, // old E_SUMMON_DEMON
        E_RESURRECT_NEW = 113,
        E_ATTACK_ME = 114,
        E_DURABILITY_DAMAGE_PCT = 115,
        E_SKIN_PLAYER_CORPSE = 116,
        E_SPIRIT_HEAL = 117,
        E_SKILL = 118,
        E_APPLY_AREA_AURA_PET = 119,
        E_TELEPORT_GRAVEYARD = 120,
        E_NORMALIZED_WEAPON_DMG = 121,
        E_122 = 122,
        E_SEND_TAXI = 123,
        E_PLAYER_PULL = 124,
        E_MODIFY_THREAT_PERCENT = 125,
        E_STEAL_BENEFICIAL_BUFF = 126,
        E_PROSPECTING = 127,
        E_APPLY_AREA_AURA_FRIEND = 128,
        E_APPLY_AREA_AURA_ENEMY = 129,
        E_REDIRECT_THREAT = 130,
        E_131 = 131,
        E_PLAY_MUSIC = 132,
        E_UNLEARN_SPECIALIZATION = 133,
        E_KILL_CREDIT2 = 134,
        E_CALL_PET = 135,
        E_HEAL_PCT = 136,
        E_ENERGIZE_PCT = 137,
        E_LEAP_BACK = 138,
        E_CLEAR_QUEST = 139,
        E_FORCE_CAST = 140,
        E_141 = 141,
        E_TRIGGER_SPELL_WITH_VALUE = 142,
        E_APPLY_AREA_AURA_OWNER = 143,
        E_144 = 144,
        E_145 = 145,
        E_ACTIVATE_RUNE = 146,
        E_QUEST_FAIL = 147,
        E_148 = 148,
        E_149 = 149,
        E_150 = 150,
        E_TRIGGER_SPELL_2 = 151,
        E_152 = 152,
        E_153 = 153,
        E_TEACH_TAXI_NODE = 154,
        E_TITAN_GRIP = 155,
        E_ENCHANT_ITEM_PRISMATIC = 156,
        E_CREATE_ITEM_2 = 157,
        E_MILLING = 158,
        E_ALLOW_RENAME_PET = 159,
        E_160 = 160,
        E_TALENT_SPEC_COUNT = 161,
        E_TALENT_SPEC_SELECT = 162,
        E_163 = 163,
        E_164 = 164,
        E_165 = 165,
        E_166 = 166,
        E_167 = 167,
        E_168 = 168,
        E_169 = 169,
        E_170 = 170,
        E_171 = 171,
        E_172 = 172,
        E_173 = 173,
        E_174 = 174,
        E_175 = 175,
        E_176 = 176,
        E_179 = 179,
        E_188 = 188,
        E_189 = 189,
        E_196 = 196,
        E_203 = 203,
        E_206 = 206,
		E_223 = 223,
		E_230 = 230,
        E_238 = 238,
		E_243 = 243,
		E_MAX
    }

    public enum EffectSubtype
    {
        A_NONE = 0,
        A_BIND_SIGHT = 1,
        A_MOD_POSSESS = 2,
        A_PERIODIC_DAMAGE = 3,
        A_DUMMY = 4,
        A_MOD_CONFUSE = 5,
        A_MOD_CHARM = 6,
        A_MOD_FEAR = 7,
        A_PERIODIC_HEAL = 8,
        A_MOD_ATTACKSPEED = 9,
        A_MOD_THREAT = 10,
        A_MOD_TAUNT = 11,
        A_MOD_STUN = 12,
        A_MOD_DAMAGE_DONE = 13,
        A_MOD_DAMAGE_TAKEN = 14,
        A_DAMAGE_SHIELD = 15,
        A_MOD_STEALTH = 16,
        A_MOD_STEALTH_DETECT = 17,
        A_MOD_INVISIBILITY = 18,
        A_MOD_INVISIBILITY_DETECTION = 19,
        A_OBS_MOD_HEALTH = 20, //20,21 unofficial
        A_OBS_MOD_MANA = 21,
        A_MOD_RESISTANCE = 22,
        A_PERIODIC_TRIGGER_SPELL = 23,
        A_PERIODIC_ENERGIZE = 24,
        A_MOD_PACIFY = 25,
        A_MOD_ROOT = 26,
        A_MOD_SILENCE = 27,
        A_REFLECT_SPELLS = 28,
        A_MOD_STAT = 29,
        A_MOD_SKILL = 30,
        A_MOD_INCREASE_SPEED = 31,
        A_MOD_INCREASE_MOUNTED_SPEED = 32,
        A_MOD_DECREASE_SPEED = 33,
        A_MOD_INCREASE_HEALTH = 34,
        A_MOD_INCREASE_ENERGY = 35,
        A_MOD_SHAPESHIFT = 36,
        A_EFFECT_IMMUNITY = 37,
        A_STATE_IMMUNITY = 38,
        A_SCHOOL_IMMUNITY = 39,
        A_DAMAGE_IMMUNITY = 40,
        A_DISPEL_IMMUNITY = 41,
        A_PROC_TRIGGER_SPELL = 42,
        A_PROC_TRIGGER_DAMAGE = 43,
        A_TRACK_CREATURES = 44,
        A_TRACK_RESOURCES = 45,
        A_46 = 46, // Ignore all Gear test spells
        A_MOD_PARRY_PERCENT = 47,
        A_48 = 48, // One periodic spell
        A_MOD_DODGE_PERCENT = 49,
        A_MOD_CRITICAL_HEALING_AMOUNT = 50,
        A_MOD_BLOCK_PERCENT = 51,
        A_MOD_CRIT_PERCENT = 52,
        A_PERIODIC_LEECH = 53,
        A_MOD_HIT_CHANCE = 54,
        A_MOD_SPELL_HIT_CHANCE = 55,
        A_TRANSFORM = 56,
        A_MOD_SPELL_CRIT_CHANCE = 57,
        A_MOD_INCREASE_SWIM_SPEED = 58,
        A_MOD_DAMAGE_DONE_CREATURE = 59,
        A_MOD_PACIFY_SILENCE = 60,
        A_MOD_SCALE = 61,
        A_PERIODIC_HEALTH_FUNNEL = 62,
        A_63 = 63, // old A_PERIODIC_MANA_FUNNEL
        A_PERIODIC_MANA_LEECH = 64,
        A_MOD_CASTING_SPEED_NOT_STACK = 65,
        A_FEIGN_DEATH = 66,
        A_MOD_DISARM = 67,
        A_MOD_STALKED = 68,
        A_SCHOOL_ABSORB = 69,
        A_EXTRA_ATTACKS = 70,
        A_MOD_SPELL_CRIT_CHANCE_SCHOOL = 71,
        A_MOD_POWER_COST_SCHOOL_PCT = 72,
        A_MOD_POWER_COST_SCHOOL = 73,
        A_REFLECT_SPELLS_SCHOOL = 74,
        A_MOD_LANGUAGE = 75,
        A_FAR_SIGHT = 76,
        A_MECHANIC_IMMUNITY = 77,
        A_MOUNTED = 78,
        A_MOD_DAMAGE_PERCENT_DONE = 79,
        A_MOD_PERCENT_STAT = 80,
        A_SPLIT_DAMAGE_PCT = 81,
        A_WATER_BREATHING = 82,
        A_MOD_BASE_RESISTANCE = 83,
        A_MOD_REGEN = 84,
        A_MOD_POWER_REGEN = 85,
        A_CHANNEL_DEATH_ITEM = 86,
        A_MOD_DAMAGE_PERCENT_TAKEN = 87,
        A_MOD_HEALTH_REGEN_PERCENT = 88,
        A_PERIODIC_DAMAGE_PERCENT = 89,
        A_90 = 90, // old A_MOD_RESIST_CHANCE
        A_MOD_DETECT_RANGE = 91,
        A_PREVENTS_FLEEING = 92,
        A_MOD_UNATTACKABLE = 93,
        A_INTERRUPT_REGEN = 94,
        A_GHOST = 95,
        A_SPELL_MAGNET = 96,
        A_MANA_SHIELD = 97,
        A_MOD_SKILL_TALENT = 98,
        A_MOD_ATTACK_POWER = 99,
        A_AURAS_VISIBLE = 100,
        A_MOD_RESISTANCE_PCT = 101,
        A_MOD_MELEE_ATTACK_POWER_VERSUS = 102,
        A_MOD_TOTAL_THREAT = 103,
        A_WATER_WALK = 104,
        A_FEATHER_FALL = 105,
        A_HOVER = 106,
        A_ADD_FLAT_MODIFIER = 107,
        A_ADD_PCT_MODIFIER = 108,
        A_ADD_TARGET_TRIGGER = 109,
        A_MOD_POWER_REGEN_PERCENT = 110,
        A_ADD_CASTER_HIT_TRIGGER = 111,
        A_OVERRIDE_CLASS_SCRIPTS = 112,
        A_MOD_RANGED_DAMAGE_TAKEN = 113,
        A_MOD_RANGED_DAMAGE_TAKEN_PCT = 114,
        A_MOD_HEALING = 115,
        A_MOD_REGEN_DURING_COMBAT = 116,
        A_MOD_MECHANIC_RESISTANCE = 117,
        A_MOD_HEALING_PCT = 118,
        A_119 = 119, // old A_SHARE_PET_TRACKING
        A_UNTRACKABLE = 120,
        A_EMPATHY = 121,
        A_MOD_OFFHAND_DAMAGE_PCT = 122,
        A_MOD_TARGET_RESISTANCE = 123,
        A_MOD_RANGED_ATTACK_POWER = 124,
        A_MOD_MELEE_DAMAGE_TAKEN = 125,
        A_MOD_MELEE_DAMAGE_TAKEN_PCT = 126,
        A_RANGED_ATTACK_POWER_ATTACKER_BONUS = 127,
        A_MOD_POSSESS_PET = 128,
        A_MOD_SPEED_ALWAYS = 129,
        A_MOD_MOUNTED_SPEED_ALWAYS = 130,
        A_MOD_RANGED_ATTACK_POWER_VERSUS = 131,
        A_MOD_INCREASE_ENERGY_PERCENT = 132,
        A_MOD_INCREASE_HEALTH_PERCENT = 133,
        A_MOD_MANA_REGEN_INTERRUPT = 134,
        A_MOD_HEALING_DONE = 135,
        A_MOD_HEALING_DONE_PERCENT = 136,
        A_MOD_TOTAL_STAT_PERCENTAGE = 137,
        A_MOD_HASTE = 138,
        A_FORCE_REACTION = 139,
        A_MOD_RANGED_HASTE = 140,
        A_MOD_RANGED_AMMO_HASTE = 141,
        A_MOD_BASE_RESISTANCE_PCT = 142,
        A_MOD_RESISTANCE_EXCLUSIVE = 143,
        A_SAFE_FALL = 144,
        A_MOD_PET_TALENT_POINTS = 145,
        A_ALLOW_TAME_PET_TYPE = 146,
        A_MECHANIC_IMMUNITY_MASK = 147,
        A_RETAIN_COMBO_POINTS = 148,
        A_REDUCE_PUSHBACK = 149, // Reduce Pushback
        A_MOD_SHIELD_BLOCKVALUE_PCT = 150,
        A_TRACK_STEALTHED = 151, // Track Stealthed
        A_MOD_DETECTED_RANGE = 152, // Mod Detected Range
        A_SPLIT_DAMAGE_FLAT = 153, // Split Damage Flat
        A_MOD_STEALTH_LEVEL = 154, // Stealth Level Modifier
        A_MOD_WATER_BREATHING = 155, // Mod Water Breathing
        A_MOD_REPUTATION_GAIN = 156, // Mod Reputation Gain
        A_PET_DAMAGE_MULTI = 157, // Mod Pet Damage
        A_MOD_SHIELD_BLOCKVALUE = 158,
        A_NO_PVP_CREDIT = 159,
        A_MOD_AOE_AVOIDANCE = 160,
        A_MOD_HEALTH_REGEN_IN_COMBAT = 161,
        A_POWER_BURN_MANA = 162,
        A_MOD_CRIT_DAMAGE_BONUS = 163,
        A_164 = 164,
        A_MELEE_ATTACK_POWER_ATTACKER_BONUS = 165,
        A_MOD_ATTACK_POWER_PCT = 166,
        A_MOD_RANGED_ATTACK_POWER_PCT = 167,
        A_MOD_DAMAGE_DONE_VERSUS = 168,
        A_MOD_CRIT_PERCENT_VERSUS = 169,
        A_DETECT_AMORE = 170,
        A_MOD_SPEED_NOT_STACK = 171,
        A_MOD_MOUNTED_SPEED_NOT_STACK = 172,
        A_173 = 173, // old A_ALLOW_CHAMPION_SPELLS
        A_MOD_SPELL_DAMAGE_OF_STAT_PERCENT = 174, // by defeult intelect, dependent from A_MOD_SPELL_HEALING_OF_STAT_PERCENT
        A_MOD_SPELL_HEALING_OF_STAT_PERCENT = 175,
        A_SPIRIT_OF_REDEMPTION = 176,
        A_AOE_CHARM = 177,
        A_MOD_DEBUFF_RESISTANCE = 178,
        A_MOD_ATTACKER_SPELL_CRIT_CHANCE = 179,
        A_MOD_FLAT_SPELL_DAMAGE_VERSUS = 180,
        A_181 = 181, // old A_MOD_FLAT_SPELL_CRIT_DAMAGE_VERSUS - possible flat spell crit damage versus
        A_MOD_RESISTANCE_OF_STAT_PERCENT = 182,
        A_MOD_CRITICAL_THREAT = 183,
        A_MOD_ATTACKER_MELEE_HIT_CHANCE = 184,
        A_MOD_ATTACKER_RANGED_HIT_CHANCE = 185,
        A_MOD_ATTACKER_SPELL_HIT_CHANCE = 186,
        A_MOD_ATTACKER_MELEE_CRIT_CHANCE = 187,
        A_MOD_ATTACKER_RANGED_CRIT_CHANCE = 188,
        A_MOD_RATING = 189,
        A_MOD_FACTION_REPUTATION_GAIN = 190,
        A_USE_NORMAL_MOVEMENT_SPEED = 191,
        A_HASTE_MELEE = 192,
        A_HASTE_ALL = 193,
        A_MOD_IGNORE_ABSORB_SCHOOL = 194,
        A_MOD_IGNORE_ABSORB_FOR_SPELL = 195,
        A_MOD_COOLDOWN = 196, // only 24818 Noxious Breath
        A_MOD_ATTACKER_SPELL_AND_WEAPON_CRIT_CHANCE = 197,
        A_198 = 198, // old A_MOD_ALL_WEAPON_SKILLS
        A_MOD_INCREASES_SPELL_PCT_TO_HIT = 199,
        A_MOD_KILL_XP_PCT = 200,
        A_FLY = 201,
        A_IGNORE_COMBAT_RESULT = 202,
        A_MOD_ATTACKER_MELEE_CRIT_DAMAGE = 203,
        A_MOD_ATTACKER_RANGED_CRIT_DAMAGE = 204,
        A_MOD_ATTACKER_SPELL_CRIT_DAMAGE = 205,
        A_MOD_FLIGHT_SPEED = 206,
        A_MOD_FLIGHT_SPEED_MOUNTED = 207,
        A_MOD_FLIGHT_SPEED_STACKING = 208,
        A_MOD_FLIGHT_SPEED_MOUNTED_STACKING = 209,
        A_MOD_FLIGHT_SPEED_NOT_STACKING = 210,
        A_MOD_FLIGHT_SPEED_MOUNTED_NOT_STACKING = 211,
        A_MOD_RANGED_ATTACK_POWER_OF_STAT_PERCENT = 212,
        A_MOD_RAGE_FROM_DAMAGE_DEALT = 213,
        A_214 = 214,
        A_ARENA_PREPARATION = 215,
        A_HASTE_SPELLS = 216,
        A_217 = 217,
        A_HASTE_RANGED = 218,
        A_MOD_MANA_REGEN_FROM_STAT = 219,
        A_MOD_RATING_FROM_STAT = 220,
        A_221 = 221,
        A_222 = 222,
        A_223 = 223,
        A_224 = 224,
        A_PRAYER_OF_MENDING = 225,
        A_PERIODIC_DUMMY = 226,
        A_PERIODIC_TRIGGER_SPELL_WITH_VALUE = 227,
        A_DETECT_STEALTH = 228,
        A_MOD_AOE_DAMAGE_AVOIDANCE = 229,
        A_230 = 230,
        A_PROC_TRIGGER_SPELL_WITH_VALUE = 231,
        A_MECHANIC_DURATION_MOD = 232,
        A_233 = 233,
        A_MECHANIC_DURATION_MOD_NOT_STACK = 234,
        A_MOD_DISPEL_RESIST = 235,
        A_CONTROL_VEHICLE = 236,
        A_MOD_SPELL_DAMAGE_OF_ATTACK_POWER = 237,
        A_MOD_SPELL_HEALING_OF_ATTACK_POWER = 238,
        A_MOD_SCALE_2 = 239,
        A_MOD_EXPERTISE = 240,
        A_FORCE_MOVE_FORWARD = 241,
        A_MOD_SPELL_DAMAGE_FROM_HEALING = 242,
        A_243 = 243,
        A_COMPREHEND_LANGUAGE = 244,
        A_MOD_DURATION_OF_MAGIC_EFFECTS = 245,
        A_MOD_DURATION_OF_EFFECTS_BY_DISPEL = 246,
        A_247 = 247,
        A_MOD_COMBAT_RESULT_CHANCE = 248,
        A_CONVERT_RUNE = 249,
        A_MOD_INCREASE_HEALTH_2 = 250,
        A_MOD_ENEMY_DODGE = 251,
        A_SLOW_ALL = 252,
        A_MOD_BLOCK_CRIT_CHANCE = 253,
        A_MOD_DISARM_SHIELD = 254,
        A_MOD_MECHANIC_DAMAGE_TAKEN_PERCENT = 255,
        A_NO_REAGENT_USE = 256,
        A_MOD_TARGET_RESIST_BY_SPELL_CLASS = 257,
        A_258 = 258,
        A_259 = 259,
        A_SCREEN_EFFECT = 260,
        A_PHASE = 261,
        A_262 = 262,
        A_ALLOW_ONLY_ABILITY = 263,
        A_264 = 264,
        A_265 = 265,
        A_266 = 266,
        A_MOD_IMMUNE_A_APPLY_SCHOOL = 267,
        A_MOD_ATTACK_POWER_OF_STAT_PERCENT = 268,
        A_MOD_IGNORE_DAMAGE_REDUCTION_SCHOOL = 269,
        A_MOD_IGNORE_TARGET_RESIST = 270, // Possibly need swap vs 195 aura used only in 1 spell Chaos Bolt Passive
        A_MOD_DAMAGE_FROM_CASTER = 271,
        A_MAELSTROM_WEAPON = 272,
        A_X_RAY = 273,
        A_274 = 274,
        A_MOD_IGNORE_SHAPESHIFT = 275,
        A_276 = 276, // Only "Test Mod Damage % Mechanic" spell, possible mod damage done
        A_MOD_MAX_AFFECTED_TARGETS = 277,
        A_MOD_DISARM_RANGED = 278,
        A_279 = 279,
        A_MOD_TARGET_ARMOR_PCT = 280,
        A_MOD_HONOR_GAIN = 281,
        A_MOD_BASE_HEALTH_PCT = 282,
        A_MOD_HEALING_RECEIVED = 283, // Possibly only for some spell family class spells
        A_284,
        A_MOD_ATTACK_POWER_OF_ARMOR = 285,
        A_ABILITY_PERIODIC_CRIT = 286,
        A_DEFLECT_SPELLS = 287,
        A_288 = 288,
        A_289 = 289,
        A_MOD_ALL_CRIT_CHANCE = 290,
        A_MOD_QUEST_XP_PCT = 291,
        A_OPEN_STABLE = 292,
        A_293 = 293,
        A_294 = 294,
        A_295 = 295,
        A_296 = 296,
        A_297 = 297,
        A_298 = 298,
        A_299 = 299,
        A_300 = 300,
        A_301 = 301,
        A_302 = 302,
        A_303 = 303,
        A_304 = 304,
        A_MOD_MINIMUM_SPEED = 305,
        A_306 = 306,
        A_307 = 307,
        A_308 = 308,
        A_309 = 309,
        A_310 = 310,
        A_311 = 311,
        A_312 = 312,
        A_313 = 313,
        A_314 = 314,
        A_315 = 315,
        A_316 = 316,
        A_317 = 317,
        A_318 = 318,
        A_319 = 319,
        A_320 = 320,
        A_321 = 321,
        A_322 = 322,
        A_323 = 323,
        A_324 = 324,
        A_325 = 325,
        A_326 = 326,
        A_327 = 327,
        A_328 = 328,
        A_329 = 329,
        A_330 = 330,
        A_331 = 331,
        A_332 = 332,
        A_333 = 333,
        A_334 = 334,
        A_335 = 335,
        A_336 = 336,
        A_337 = 337,
        A_338 = 338,
        A_339 = 339,
        A_340 = 340,
        A_341 = 341,
        A_342 = 342,
        A_343 = 343,
        A_344 = 344,
        A_345 = 345,
        A_346 = 346,
        A_347 = 347,
        A_348 = 348,
        A_349 = 349,
        A_350 = 350,
        A_351 = 351,
        A_352 = 352,
        A_353 = 353,
        A_354 = 354,
        A_355 = 355,
        A_356 = 356,
        A_357 = 357,
        A_358 = 358,
        A_359 = 359,
        A_360 = 360,
        A_361 = 361,
        A_362 = 362,
        A_363 = 363,
        A_364 = 364,
        A_365 = 365,
        A_366 = 366,
        A_367 = 367,
        A_371 = 371,
        A_373 = 373,
        A_377 = 377,
        A_378 = 378,
        A_379 = 379,
        A_380 = 380,
        A_381 = 381,
        A_382 = 382,
        A_383 = 383,
        A_385 = 385,
        A_389 = 389,
        A_392 = 392,
        A_393 = 393,
        A_395 = 395,
        A_396 = 396,
        A_400 = 400,
        A_402 = 402,
        A_403 = 403,
        A_404 = 404,
        A_405 = 405,
        A_407 = 407,
        A_408 = 408,
        A_409 = 409,
        A_410 = 410,
        A_411 = 411,
        A_412 = 412,
        A_416 = 416,
        A_417 = 417,
        A_418 = 418,
        A_419 = 419,
        A_421 = 421,
        A_MOD_ABSORB_DONE_PERCENT = 422,
        A_423 = 423,
        A_424 = 424,
        A_429 = 429,
        A_440 = 440,
        A_441 = 441,
        A_443 = 443,
        A_446 = 446,
        A_447 = 447,
        A_451 = 451,
        A_453 = 453,
        A_454 = 454,
        A_455 = 455,
        A_458 = 458,
        A_463 = 463,
        A_464 = 464,
        A_465 = 465,
        A_466 = 466,
        A_467 = 467,
		A_468 = 468,
        A_470 = 470,
        A_471 = 471,
		A_478 = 478,
        A_MAX
    }

    public enum SpellAttribute
    {
        SPELL_ATTR_UNK0 = 0x0000, // 0
        SPELL_ATTR_RANGED, // 1 All ranged abilites have this flag
        SPELL_ATTR_ON_NEXT_SWING_1, // 2 on next swing
        SPELL_ATTR_UNK3, // 3 not set in 3.0.3
        SPELL_ATTR_UNK4, // 4 isAbility
        SPELL_ATTR_TRADESPELL, // 5 trade spells, will be added by client to a sublist of profession spell
        SPELL_ATTR_PASSIVE, // 6 Passive spell
        SPELL_ATTR_UNK7, // 7 can't be linked in chat?
        SPELL_ATTR_UNK8, // 8 hide created item in tooltip (for effect=24)
        SPELL_ATTR_UNK9, // 9
        SPELL_ATTR_ON_NEXT_SWING_2, // 10 on next swing 2
        SPELL_ATTR_UNK11, // 11
        SPELL_ATTR_DAYTIME_ONLY, // 12 only useable at daytime, not set in 2.4.2
        SPELL_ATTR_NIGHT_ONLY, // 13 only useable at night, not set in 2.4.2
        SPELL_ATTR_INDOORS_ONLY, // 14 only useable indoors, not set in 2.4.2
        SPELL_ATTR_OUTDOORS_ONLY, // 15 Only useable outdoors.
        SPELL_ATTR_NOT_SHAPESHIFT, // 16 Not while shapeshifted
        SPELL_ATTR_ONLY_STEALTHED, // 17 Must be in stealth
        SPELL_ATTR_UNK18, // 18
        SPELL_ATTR_LEVEL_DAMAGE_CALCULATION, // 19 spelldamage depends on caster level
        SPELL_ATTR_STOP_ATTACK_TARGE, // 20 Stop attack after use this spell (and not begin attack if use)
        SPELL_ATTR_IMPOSSIBLE_DODGE_PARRY_BLOCK, // 21 Cannot be dodged/parried/blocked
        SPELL_ATTR_UNK22, // 22
        SPELL_ATTR_UNK23, // 23 castable while dead?
        SPELL_ATTR_CASTABLE_WHILE_MOUNTED, // 24 castable while mounted
        SPELL_ATTR_DISABLED_WHILE_ACTIVE, // 25 Activate and start cooldown after aura fade or remove summoned creature or go
        SPELL_ATTR_UNK26, // 26
        SPELL_ATTR_CASTABLE_WHILE_SITTING, // 27 castable while sitting
        SPELL_ATTR_CANT_USED_IN_COMBAT, // 28 Cannot be used in combat
        SPELL_ATTR_UNAFFECTED_BY_INVULNERABILITY, // 29 unaffected by invulnerability (hmm possible not...)
        SPELL_ATTR_UNK30, // 30 breakable by damage?
        SPELL_ATTR_CANT_CANCEL, // 31 positive aura can't be canceled

        SPELL_ATTR_EX_UNK0 = 0x0100, // 0
        SPELL_ATTR_EX_DRAIN_ALL_POWER, // 1 use all power (Only paladin Lay of Hands and Bunyanize)
        SPELL_ATTR_EX_CHANNELED_1, // 2 channeled 1
        SPELL_ATTR_EX_UNK3, // 3
        SPELL_ATTR_EX_UNK4, // 4
        SPELL_ATTR_EX_NOT_BREAK_STEALTH, // 5 Not break stealth
        SPELL_ATTR_EX_CHANNELED_2, // 6 channeled 2
        SPELL_ATTR_EX_NEGATIVE, // 7
        SPELL_ATTR_EX_NOT_IN_COMBAT_TARGET, // 8 Spell req target not to be in combat state
        SPELL_ATTR_EX_UNK9, // 9
        SPELL_ATTR_EX_NO_INITIAL_AGGRO, // 10 no generates threat on cast 100%
        SPELL_ATTR_EX_UNK11, // 11
        SPELL_ATTR_EX_UNK12, // 12
        SPELL_ATTR_EX_UNK13, // 13
        SPELL_ATTR_EX_UNK14, // 14
        SPELL_ATTR_EX_DISPEL_AURAS_ON_IMMUNITY, // 15 remove auras on immunity
        SPELL_ATTR_EX_UNAFFECTED_BY_SCHOOL_IMMUNE, // 16 unaffected by school immunity
        SPELL_ATTR_EX_UNK17, // 17 for auras AURA_TRACK_CREATURES, AURA_TRACK_RESOURCES and AURA_TRACK_STEALTHED select non-stacking tracking spells
        SPELL_ATTR_EX_UNK18, // 18
        SPELL_ATTR_EX_UNK19, // 19
        SPELL_ATTR_EX_REQ_COMBO_POINTS1, // 20 Req combo points on target
        SPELL_ATTR_EX_UNK21, // 21
        SPELL_ATTR_EX_REQ_COMBO_POINTS2, // 22 Req combo points on target
        SPELL_ATTR_EX_UNK23, // 23
        SPELL_ATTR_EX_UNK24, // 24 Req fishing pole??
        SPELL_ATTR_EX_UNK25, // 25
        SPELL_ATTR_EX_UNK26, // 26
        SPELL_ATTR_EX_UNK27, // 27
        SPELL_ATTR_EX_UNK28, // 28
        SPELL_ATTR_EX_UNK29, // 29
        SPELL_ATTR_EX_UNK30, // 30 overpower
        SPELL_ATTR_EX_UNK31, // 31

        SPELL_ATTR_EX2_UNK0 = 0x0200, // 0
        SPELL_ATTR_EX2_UNK1, // 1
        SPELL_ATTR_EX2_CANT_REFLECTED, // 2 ? used for detect can or not spell reflected // do not need LOS (e.g. 18220 since 3.3.3)
        SPELL_ATTR_EX2_UNK3, // 3 auto targeting? (e.g. fishing skill enhancement items since 3.3.3)
        SPELL_ATTR_EX2_UNK4, // 4
        SPELL_ATTR_EX2_AUTOREPEAT_FLAG, // 5
        SPELL_ATTR_EX2_UNK6, // 6 only usable on tabbed by yourself
        SPELL_ATTR_EX2_UNK7, // 7
        SPELL_ATTR_EX2_UNK8, // 8 not set in 3.0.3
        SPELL_ATTR_EX2_UNK9, // 9
        SPELL_ATTR_EX2_UNK10, // 10
        SPELL_ATTR_EX2_HEALTH_FUNNEL, // 11
        SPELL_ATTR_EX2_UNK12, // 12
        SPELL_ATTR_EX2_UNK13, // 13
        SPELL_ATTR_EX2_UNK14, // 14
        SPELL_ATTR_EX2_UNK15, // 15 not set in 3.0.3
        SPELL_ATTR_EX2_UNK16, // 16
        SPELL_ATTR_EX2_UNK17, // 17 suspend weapon timer instead of resetting it, (?Hunters Shot and Stings only have this flag?)
        SPELL_ATTR_EX2_UNK18, // 18 Only Revive pet - possible req dead pet
        SPELL_ATTR_EX2_NOT_NEED_SHAPESHIFT, // 19 does not necessarly need shapeshift
        SPELL_ATTR_EX2_UNK20, // 20
        SPELL_ATTR_EX2_DAMAGE_REDUCED_SHIELD, // 21 for ice blocks, pala immunity buffs, priest absorb shields, but used also for other spells -> not sure!
        SPELL_ATTR_EX2_UNK22, // 22
        SPELL_ATTR_EX2_UNK23, // 23 Only mage Arcane Concentration have this flag
        SPELL_ATTR_EX2_UNK24, // 24
        SPELL_ATTR_EX2_UNK25, // 25
        SPELL_ATTR_EX2_UNK26, // 26 unaffected by school immunity
        SPELL_ATTR_EX2_UNK27, // 27
        SPELL_ATTR_EX2_UNK28, // 28 no breaks stealth if it fails??
        SPELL_ATTR_EX2_CANT_CRIT, // 29 Spell can't crit
        SPELL_ATTR_EX2_UNK30, // 30
        SPELL_ATTR_EX2_FOOD_BUFF, // 31 Food or Drink Buff (like Well Fed)

        SPELL_ATTR_EX3_UNK0 = 0x0300, // 0
        SPELL_ATTR_EX3_UNK1, // 1
        SPELL_ATTR_EX3_UNK2, // 2
        SPELL_ATTR_EX3_UNK3, // 3
        SPELL_ATTR_EX3_UNK4, // 4 Druid Rebirth only this spell have this flag
        SPELL_ATTR_EX3_UNK5, // 5
        SPELL_ATTR_EX3_UNK6, // 6
        SPELL_ATTR_EX3_UNK7, // 7 create a separate (de)buff stack for each caster
        SPELL_ATTR_EX3_UNK8, // 8
        SPELL_ATTR_EX3_UNK9, // 9
        SPELL_ATTR_EX3_MAIN_HAND, // 10 Main hand weapon required
        SPELL_ATTR_EX3_BATTLEGROUND, // 11 Can casted only on battleground
        SPELL_ATTR_EX3_CAST_ON_DEAD, // 12 target is a dead player (not every spell has this flag)
        SPELL_ATTR_EX3_UNK13, // 13
        SPELL_ATTR_EX3_UNK14, // 14 "Honorless Target" only this spells have this flag
        SPELL_ATTR_EX3_UNK15, // 15 Auto Shoot, Shoot, Throw, - this is autoshot flag
        SPELL_ATTR_EX3_UNK16, // 16 no triggers effects that trigger on casting a spell??
        SPELL_ATTR_EX3_UNK17, // 17 no triggers effects that trigger on casting a spell??
        SPELL_ATTR_EX3_UNK18, // 18
        SPELL_ATTR_EX3_UNK19, // 19
        SPELL_ATTR_EX3_DEATH_PERSISTENT, // 20 Death persistent spells
        SPELL_ATTR_EX3_UNK21, // 21
        SPELL_ATTR_EX3_REQ_WAND, // 22 Req wand
        SPELL_ATTR_EX3_UNK23, // 23
        SPELL_ATTR_EX3_REQ_OFFHAND, // 24 Req offhand weapon
        SPELL_ATTR_EX3_UNK25, // 25 no cause spell pushback ?
        SPELL_ATTR_EX3_UNK26, // 26
        SPELL_ATTR_EX3_UNK27, // 27
        SPELL_ATTR_EX3_UNK28, // 28
        SPELL_ATTR_EX3_UNK29, // 29
        SPELL_ATTR_EX3_UNK30, // 30
        SPELL_ATTR_EX3_UNK31, // 31

        SPELL_ATTR_EX4_UNK0 = 0x0400, // 0
        SPELL_ATTR_EX4_UNK1, // 1 proc on finishing move?
        SPELL_ATTR_EX4_UNK2, // 2
        SPELL_ATTR_EX4_UNK3, // 3
        SPELL_ATTR_EX4_UNK4, // 4 This will no longer cause guards to attack on use??
        SPELL_ATTR_EX4_UNK5, // 5
        SPELL_ATTR_EX4_NOT_STEALABLE, // 6 although such auras might be dispellable, they cannot be stolen
        SPELL_ATTR_EX4_UNK7, // 7
        SPELL_ATTR_EX4_STACK_DOT_MODIFIER, // 8 no effect on non DoTs?
        SPELL_ATTR_EX4_UNK9, // 9
        SPELL_ATTR_EX4_SPELL_VS_EXTEND_COST, // 10 Rogue Shiv have this flag
        SPELL_ATTR_EX4_UNK11, // 11
        SPELL_ATTR_EX4_UNK12, // 12
        SPELL_ATTR_EX4_UNK13, // 13
        SPELL_ATTR_EX4_UNK14, // 14
        SPELL_ATTR_EX4_UNK15, // 15
        SPELL_ATTR_EX4_NOT_USABLE_IN_ARENA, // 16 not usable in arena
        SPELL_ATTR_EX4_USABLE_IN_ARENA, // 17 usable in arena
        SPELL_ATTR_EX4_UNK18, // 18
        SPELL_ATTR_EX4_UNK19, // 19
        SPELL_ATTR_EX4_UNK20, // 20 do not give "more powerful spell" error message
        SPELL_ATTR_EX4_UNK21, // 21
        SPELL_ATTR_EX4_UNK22, // 22
        SPELL_ATTR_EX4_UNK23, // 23
        SPELL_ATTR_EX4_UNK24, // 24
        SPELL_ATTR_EX4_UNK25, // 25 pet scaling auras
        SPELL_ATTR_EX4_CAST_ONLY_IN_OUTLAND, // 26 Can only be used in Outland.
        SPELL_ATTR_EX4_UNK27, // 27
        SPELL_ATTR_EX4_UNK28, // 28
        SPELL_ATTR_EX4_UNK29, // 29
        SPELL_ATTR_EX4_UNK30, // 30
        SPELL_ATTR_EX4_UNK31, // 31

        SPELL_ATTR_EX5_UNK0 = 0x0500, // 0
        SPELL_ATTR_EX5_NO_REAGENT_WHILE_PREP, // 1 not need reagents if UNIT_FLAG_PREPARATION
        SPELL_ATTR_EX5_UNK2, // 2 removed at enter arena (e.g. 31850 since 3.3.3)
        SPELL_ATTR_EX5_USABLE_WHILE_STUNNED, // 3 usable while stunned
        SPELL_ATTR_EX5_UNK4, // 4
        SPELL_ATTR_EX5_SINGLE_TARGET_SPELL, // 5 Only one target can be apply at a time
        SPELL_ATTR_EX5_UNK6, // 6
        SPELL_ATTR_EX5_UNK7, // 7
        SPELL_ATTR_EX5_UNK8, // 8
        SPELL_ATTR_EX5_START_PERIODIC_AT_APPLY, // 9 begin periodic tick at aura apply
        SPELL_ATTR_EX5_UNK10, // 10
        SPELL_ATTR_EX5_UNK11, // 11
        SPELL_ATTR_EX5_UNK12, // 12
        SPELL_ATTR_EX5_UNK13, // 13 haste affects duration (e.g. 8050 since 3.3.3)
        SPELL_ATTR_EX5_UNK14, // 14
        SPELL_ATTR_EX5_UNK15, // 15
        SPELL_ATTR_EX5_UNK16, // 16
        SPELL_ATTR_EX5_USABLE_WHILE_FEARED, // 17 usable while feared
        SPELL_ATTR_EX5_USABLE_WHILE_CONFUSED, // 18 usable while confused
        SPELL_ATTR_EX5_UNK19, // 19
        SPELL_ATTR_EX5_UNK20, // 20
        SPELL_ATTR_EX5_UNK21, // 21
        SPELL_ATTR_EX5_UNK22, // 22
        SPELL_ATTR_EX5_UNK23, // 23
        SPELL_ATTR_EX5_UNK24, // 24
        SPELL_ATTR_EX5_UNK25, // 25
        SPELL_ATTR_EX5_UNK26, // 26
        SPELL_ATTR_EX5_UNK27, // 27
        SPELL_ATTR_EX5_UNK28, // 28
        SPELL_ATTR_EX5_UNK29, // 29
        SPELL_ATTR_EX5_UNK30, // 30
        SPELL_ATTR_EX5_UNK31, // 31 Forces all nearby enemies to focus attacks caster

        SPELL_ATTR_EX6_UNK0 = 0x0600, // 0 Only Move spell have this flag
        SPELL_ATTR_EX6_ONLY_IN_ARENA, // 1 only usable in arena, not used in 3.2.0a and early
        SPELL_ATTR_EX6_UNK2, // 2
        SPELL_ATTR_EX6_UNK3, // 3
        SPELL_ATTR_EX6_UNK4, // 4
        SPELL_ATTR_EX6_UNK5, // 5
        SPELL_ATTR_EX6_UNK6, // 6
        SPELL_ATTR_EX6_UNK7, // 7
        SPELL_ATTR_EX6_UNK8, // 8
        SPELL_ATTR_EX6_UNK9, // 9
        SPELL_ATTR_EX6_UNK10, // 10
        SPELL_ATTR_EX6_NOT_IN_RAID_INSTANCE, // 11 not usable in raid instance
        SPELL_ATTR_EX6_UNK12, // 12 for auras AURA_TRACK_CREATURES, AURA_TRACK_RESOURCES and AURA_TRACK_STEALTHED select non-stacking tracking spells
        SPELL_ATTR_EX6_UNK13, // 13
        SPELL_ATTR_EX6_UNK14, // 14
        SPELL_ATTR_EX6_UNK15, // 15 not set in 3.0.3
        SPELL_ATTR_EX6_UNK16, // 16
        SPELL_ATTR_EX6_UNK17, // 17
        SPELL_ATTR_EX6_UNK18, // 18
        SPELL_ATTR_EX6_UNK19, // 19
        SPELL_ATTR_EX6_UNK20, // 20
        SPELL_ATTR_EX6_UNK21, // 21
        SPELL_ATTR_EX6_UNK22, // 22
        SPELL_ATTR_EX6_UNK23, // 23 not set in 3.0.3
        SPELL_ATTR_EX6_UNK24, // 24 not set in 3.0.3
        SPELL_ATTR_EX6_UNK25, // 25 not set in 3.0.3
        SPELL_ATTR_EX6_UNK26, // 26 not set in 3.0.3
        SPELL_ATTR_EX6_UNK27, // 27 not set in 3.0.3
        SPELL_ATTR_EX6_UNK28, // 28 not set in 3.0.3
        SPELL_ATTR_EX6_NO_DMG_PERCENT_MODS, // 29 do not apply damage percent mods (usually in cases where it has already been applied)
        SPELL_ATTR_EX6_UNK30, // 30 not set in 3.0.3
        SPELL_ATTR_EX6_UNK31, // 31 not set in 3.0.3

        SPELL_ATTR_EX7_UNK0 = 0x0700, // 0
        SPELL_ATTR_EX7_UNK1, // 1
        SPELL_ATTR_EX7_UNK2, // 2
        SPELL_ATTR_EX7_UNK3, // 3
        SPELL_ATTR_EX7_UNK4, // 4
        SPELL_ATTR_EX7_UNK5, // 5
        SPELL_ATTR_EX7_UNK6, // 6
        SPELL_ATTR_EX7_UNK7, // 7
        SPELL_ATTR_EX7_UNK8, // 8
        SPELL_ATTR_EX7_UNK9, // 9
        SPELL_ATTR_EX7_UNK10, // 10
        SPELL_ATTR_EX7_UNK11, // 11
        SPELL_ATTR_EX7_UNK12, // 12
        SPELL_ATTR_EX7_UNK13, // 13
        SPELL_ATTR_EX7_UNK14, // 14
        SPELL_ATTR_EX7_UNK15, // 15
        SPELL_ATTR_EX7_UNK16, // 16
        SPELL_ATTR_EX7_UNK17, // 17
        SPELL_ATTR_EX7_UNK18, // 18
        SPELL_ATTR_EX7_UNK19, // 19
        SPELL_ATTR_EX7_UNK20, // 20
        SPELL_ATTR_EX7_UNK21, // 21
        SPELL_ATTR_EX7_UNK22, // 22
        SPELL_ATTR_EX7_UNK23, // 23
        SPELL_ATTR_EX7_UNK24, // 24
        SPELL_ATTR_EX7_UNK25, // 25
        SPELL_ATTR_EX7_UNK26, // 26
        SPELL_ATTR_EX7_UNK27, // 27
        SPELL_ATTR_EX7_UNK28, // 28
        SPELL_ATTR_EX7_UNK29, // 29
        SPELL_ATTR_EX7_UNK30, // 30
        SPELL_ATTR_EX7_UNK31, // 31

        SPELL_ATTR_EX8_UNK0 = 0x0800, // 0
        SPELL_ATTR_EX8_UNK1, // 1
        SPELL_ATTR_EX8_UNK2, // 2
        SPELL_ATTR_EX8_UNK3, // 3
        SPELL_ATTR_EX8_UNK4, // 4
        SPELL_ATTR_EX8_UNK5, // 5
        SPELL_ATTR_EX8_UNK6, // 6
        SPELL_ATTR_EX8_UNK7, // 7
        SPELL_ATTR_EX8_UNK8, // 8
        SPELL_ATTR_EX8_UNK9, // 9
        SPELL_ATTR_EX8_UNK10, // 10
        SPELL_ATTR_EX8_UNK11, // 11
        SPELL_ATTR_EX8_UNK12, // 12
        SPELL_ATTR_EX8_UNK13, // 13
        SPELL_ATTR_EX8_UNK14, // 14
        SPELL_ATTR_EX8_UNK15, // 15
        SPELL_ATTR_EX8_UNK16, // 16
        SPELL_ATTR_EX8_UNK17, // 17
        SPELL_ATTR_EX8_UNK18, // 18
        SPELL_ATTR_EX8_UNK19, // 19
        SPELL_ATTR_EX8_UNK20, // 20
        SPELL_ATTR_EX8_UNK21, // 21
        SPELL_ATTR_EX8_UNK22, // 22
        SPELL_ATTR_EX8_UNK23, // 23
        SPELL_ATTR_EX8_UNK24, // 24
        SPELL_ATTR_EX8_UNK25, // 25
        SPELL_ATTR_EX8_UNK26, // 26
        SPELL_ATTR_EX8_UNK27, // 27
        SPELL_ATTR_EX8_UNK28, // 28
        SPELL_ATTR_EX8_UNK29, // 29
        SPELL_ATTR_EX8_UNK30, // 30
        SPELL_ATTR_EX8_UNK31, // 31

        SPELL_ATTR_EX9_UNK0 = 0x0900, // 0
        SPELL_ATTR_EX9_UNK1, // 1
        SPELL_ATTR_EX9_UNK2, // 2
        SPELL_ATTR_EX9_UNK3, // 3
        SPELL_ATTR_EX9_UNK4, // 4
        SPELL_ATTR_EX9_UNK5, // 5
        SPELL_ATTR_EX9_UNK6, // 6
        SPELL_ATTR_EX9_UNK7, // 7
        SPELL_ATTR_EX9_UNK8, // 8
        SPELL_ATTR_EX9_UNK9, // 9
        SPELL_ATTR_EX9_UNK10, // 10
        SPELL_ATTR_EX9_UNK11, // 11
        SPELL_ATTR_EX9_UNK12, // 12
        SPELL_ATTR_EX9_UNK13, // 13
        SPELL_ATTR_EX9_UNK14, // 14
        SPELL_ATTR_EX9_UNK15, // 15
        SPELL_ATTR_EX9_UNK16, // 16
        SPELL_ATTR_EX9_UNK17, // 17
        SPELL_ATTR_EX9_UNK18, // 18
        SPELL_ATTR_EX9_UNK19, // 19
        SPELL_ATTR_EX9_UNK20, // 20
        SPELL_ATTR_EX9_UNK21, // 21
        SPELL_ATTR_EX9_UNK22, // 22
        SPELL_ATTR_EX9_UNK23, // 23
        SPELL_ATTR_EX9_UNK24, // 24
        SPELL_ATTR_EX9_UNK25, // 25
        SPELL_ATTR_EX9_UNK26, // 26
        SPELL_ATTR_EX9_UNK27, // 27
        SPELL_ATTR_EX9_UNK28, // 28
        SPELL_ATTR_EX9_UNK29, // 29
        SPELL_ATTR_EX9_UNK30, // 30
        SPELL_ATTR_EX9_UNK31, // 31
    }    
}
