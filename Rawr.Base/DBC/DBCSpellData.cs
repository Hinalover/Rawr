using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    public class DBCSpellData
    {
        private string _name;                   // Spell name from Spell.dbc stringblock (enGB)
        private uint _id;                       // Spell ID in dbc
        private uint _flags;                    // Unused for now, 0x00 for all
        private float _prj_speed;               // Projectile Speed
        private ItemDamageType _school;         // School, requires custom thing
        private CharacterClass _class_mask;     // Class mask for spell
        private CharacterRace _race_mask;       // Racial mask for the spell
        private int _scaling_type;              // Array index for gtSpellScaling.dbc. -1 means the first non-class-specific sub array, and so on, 0 disabled
        private uint _max_scaling_level;       // Max scaling level(?), 0 == no restrictions, otherwise min( player_level, max_scaling_level )
        private float _extra_coeff;             // An "extra" coefficient (used for some spells to indicate AP based coefficient)
        // SpellLevels.dbc
        private uint _spell_level;             // Spell learned on level. NOTE: Only accurate for "class abilities"
        private uint _max_level;               // Maximum level for scaling
        // SpellRange.dbc
        private float _min_range;               // Minimum range in yards
        private float _max_range;               // Maximum range in yards
        // SpellCooldown.dbc
        private uint _cooldown;                // Cooldown in milliseconds
        private uint _gcd;                     // GCD in milliseconds
        // SpellCategories.dbc
        private uint _category;                // Spell category (for shared cooldowns, effects?)
        // SpellDuration.dbc
        private float _duration;                // Spell duration in milliseconds
        // SpellRuneCost.dbc
        private DKRune _rune_cost;              // Bitmask of rune cost 0x1, 0x2 = Blood | 0x4, 0x8 = Unholy | 0x10, 0x20 = Frost
        private uint _runic_power_gain;        // Amount of runic power gained ( / 10 )
        // SpellAuraOptions.dbc
        private uint _max_stack;               // Maximum stack size for spell
        private uint _proc_chance;             // Spell proc chance in percent
        private uint _proc_charges;            // Per proc charge amount
        private uint _proc_flags;               // Proc flags, no support for now
        private uint _internal_cooldown;       // ICD in milliseconds
        private float _rppm;                    // Base real procs per minute
        // SpellEquippedItems.dbc
        private uint _equipped_class;
        private uint _equipped_invtype_mask;
        private uint _equipped_subclass_mask;
        // SpellScaling.dbc
        private int _cast_min;                // Minimum casting time in milliseconds
        private int _cast_max;                // Maximum casting time in milliseconds
        private int _cast_div;                // A divisor used in the formula for casting time scaling (20 always?)
        private float _c_scaling;               // A scaling multiplier for level based scaling
        private uint _c_scaling_level;         // A scaling divisor for level based scaling
        // SpecializationSpells.dbc 
        private uint _replace_spell_id;
        // Spell.dbc flags                      
        private List<uint> _attribute;          // Spell.dbc "flags", record field 1..10, note that 12694 added a field here after flags_7
        private string _desc;                   // Spell.dbc description stringblock
        private string _tooltip;                // Spell.dbc tooltip stringblock
        // SpellDescriptionVariables.dbc
        private string _desc_vars;              // Spell description variable stringblock, if present
        // SpellIcon.dbc
        private string _icon;
        private string _active_icon;
        private string _rank_str;
        private string _effect_2;
        private string _effect_3;

        public DBCSpellData()
        {
            _name = "";
            _id = 0;
            _flags = 0x00;
            _prj_speed = 0;
            _school = ItemDamageType.Physical;
            _class_mask = CharacterClass.None;
            _race_mask = CharacterRace.None;
            _scaling_type = 0;
            _max_scaling_level = 0;
            _extra_coeff = 0;
            _spell_level = 0;
            _max_level = 0;
            _min_range = 0;
            _max_range = 0;
            _cooldown = 0;
            _gcd = 0;
            _category = 0;
            _duration = 0;
            _rune_cost = DKRune.None;
            _runic_power_gain = 0;
            _max_stack = 0;
            _proc_chance = 0;
            _proc_charges = 0;
            _proc_flags = 0;
            _internal_cooldown = 0;
            _rppm = 0;
            _equipped_class = 0;
            _equipped_invtype_mask = 0x00;
            _equipped_subclass_mask = 0x00;
            _cast_min = 0;
            _cast_max = 0;
            _cast_div = 0;
            _c_scaling = 0;
            _c_scaling_level = 0;
            _replace_spell_id = 0;
            _attribute.Add(0x00);       // Attribute 1
            _attribute.Add(0x00);       // Attribute 2
            _attribute.Add(0x00);       // Attribute 3
            _attribute.Add(0x00);       // Attribute 4
            _attribute.Add(0x00);       // Attribute 5
            _attribute.Add(0x00);       // Attribute 6
            _attribute.Add(0x00);       // Attribute 7
            _attribute.Add(0x00);       // Attribute 8
            _attribute.Add(0x00);       // Attribute 9
            _attribute.Add(0x00);       // Attribute 10
            _attribute.Add(0x00);       // Attribute 11
            _attribute.Add(0x00);       // Attribute 12
            _desc = "";
            _tooltip = "";
            _desc_vars = "";
            _icon = "";
            _active_icon = "";
            _rank_str = "";
            _effect_2 = "";
            _effect_3 = "";
        }

        public DBCSpellData(string a, uint b, uint c, float d, ItemDamageType e, CharacterClass f, CharacterRace g, int h,
            uint i, float j, uint k, uint l, float m, float n, uint o, uint p, uint q, float r, DKRune s, uint t,
            uint u, uint v, uint w, uint x, uint y, float z, uint aa, uint ab, uint ac, int ad, int ae, int af,
            float ag, uint ah, uint ai, uint aj, uint ak, uint al, uint am, uint an, uint ao, uint ap, uint aq, uint ar, 
            uint at, uint au, uint av, string aw, string ax, string ay, string az, string ba, string bb, string bc, string bd )
        {
            _name = a;
            _id = b;
            _flags = c;
            _prj_speed = d;
            _school = e;
            _class_mask = f;
            _race_mask = g;
            _scaling_type = h;
            _max_scaling_level = i;
            _extra_coeff = j;
            _spell_level = k;
            _max_level = l;
            _min_range = m;
            _max_range = n;
            _cooldown = o;
            _gcd = p;
            _category = q;
            _duration = r;
            _rune_cost = s;
            _runic_power_gain = t;
            _max_stack = u;
            _proc_chance = v;
            _proc_charges = w;
            _proc_flags = x;
            _internal_cooldown = y;
            _rppm = z;
            _equipped_class = aa;
            _equipped_invtype_mask = ab;
            _equipped_subclass_mask = ac;
            _cast_min = ad;
            _cast_max = ae;
            _cast_div = af;
            _c_scaling = ag;
            _c_scaling_level = ah;
            _replace_spell_id = ai;
			_attribute = new List<uint>();
            _attribute.Add(aj);       // Attribute 1
            _attribute.Add(ak);       // Attribute 2
            _attribute.Add(al);       // Attribute 3
            _attribute.Add(am);       // Attribute 4
            _attribute.Add(an);       // Attribute 5
            _attribute.Add(ao);       // Attribute 6
            _attribute.Add(ap);       // Attribute 7
            _attribute.Add(aq);       // Attribute 8
            _attribute.Add(ar);       // Attribute 9
            _attribute.Add(at);       // Attribute 10
            _attribute.Add(au);       // Attribute 11
            _attribute.Add(av);       // Attribute 12
            _desc = aw;
            _tooltip = ax;
            _desc_vars = ay;
            _icon = az;
            _active_icon = ba;
            _rank_str = bb;
            _effect_2 = bc;
            _effect_3 = bd;
        }

        /// <summary>
        /// Spell name from Spell.dbc stringblock (enGB)
        /// </summary>
        public string name { get { return _name; } }

        /// <summary>
        /// Spell ID in dbc
        /// </summary>
        public uint id { get { return _id; } }

        /// <summary>
        /// The projectile speed of the spell in yards per second.
        /// </summary>
        public float missile_speed { get { return _prj_speed; } set { _prj_speed = value; } }

        /// <summary>
        /// School mask of the spell
        /// </summary>
        public ItemDamageType spell_damage_type { get { return _school; } set { _school = value; } }

        /// <summary>
        /// Class mask for spell
        /// </summary>
        public CharacterClass char_class { get { return _class_mask; } }

        /// <summary>
        /// Racial mask for the spell
        /// </summary>
        public CharacterRace char_race { get { return _race_mask; } }

        /// <summary>
        /// Internal value of the scaling class for the spell
        /// </summary>
        public CharacterClass scaling_class
        {
            get
            {
                switch (_scaling_type)
                {
                    case -4:    return CharacterClass.Player_Special_Scale4;
                    case -3:    return CharacterClass.Player_Special_Scale3;
                    case -2:    return CharacterClass.Player_Special_Scale2;
                    case -1:    return CharacterClass.Player_Special_Scale;
                    case 1:     return CharacterClass.Warrior;
                    case 2:     return CharacterClass.Paladin;
                    case 3:     return CharacterClass.Hunter;
                    case 4:     return CharacterClass.Rogue;
                    case 5:     return CharacterClass.Priest;
                    case 6:     return CharacterClass.DeathKnight;
                    case 7:     return CharacterClass.Shaman;
                    case 8:     return CharacterClass.Mage;
                    case 9:     return CharacterClass.Warlock;
                    case 10:    return CharacterClass.Monk;
                    case 11:    return CharacterClass.Druid;
                    default:    return CharacterClass.None;
                }
            }
            set
            {
                switch (value)
                {
                    case CharacterClass.Player_Special_Scale4:  _scaling_type = -4; break;
                    case CharacterClass.Player_Special_Scale3:  _scaling_type = -3; break;
                    case CharacterClass.Player_Special_Scale2:  _scaling_type = -2; break;
                    case CharacterClass.Player_Special_Scale:   _scaling_type = -1; break;
                    case CharacterClass.Warrior:                _scaling_type = 1; break;
                    case CharacterClass.Paladin:                _scaling_type = 2; break;
                    case CharacterClass.Hunter:                 _scaling_type = 3; break;
                    case CharacterClass.Rogue:                  _scaling_type = 4; break;
                    case CharacterClass.Priest:                 _scaling_type = 5; break;
                    case CharacterClass.DeathKnight:            _scaling_type = 6; break;
                    case CharacterClass.Shaman:                 _scaling_type = 7; break;
                    case CharacterClass.Mage:                   _scaling_type = 8; break;
                    case CharacterClass.Warlock:                _scaling_type = 9; break;
                    case CharacterClass.Monk:                   _scaling_type = 10; break;
                    case CharacterClass.Druid:                  _scaling_type = 11; break;
                    default:                                    _scaling_type = 0; break;
                }
            } 
        }

        /// <summary>
        /// Max scaling level(?), 0 == no restrictions, otherwise min( player_level, max_scaling_level )
        /// </summary>
        public uint max_scaling_level { get { return _max_scaling_level; } }

        /// <summary>
        /// An "extra" coefficient (used for some spells to indicate AP based coefficient)
        /// </summary>
        public float extra_coeff { get { return _extra_coeff; } }

        // SpellLevels.dbc

        /// <summary>
        /// Required level of the spell
        /// </summary>
        public uint level { get { return _spell_level; } set { _prj_speed = value; } }

        /// <summary>
        /// Maximum scaling level of the spell
        /// </summary>
        public uint max_level { get { return _max_level; } set { _prj_speed = value; } }

        // SpellRange.dbc

        /// <summary>
        /// Minimum range of the spell (currently unused)
        /// </summary>
        public float min_range { get { return _min_range; } set { _prj_speed = value; } }

        /// <summary>
        /// Maximum range of the spell (currently unused)
        /// </summary>
        public float max_range { get { return _max_range; } set { _prj_speed = value; } }

        // SpellCooldown.dbc

        /// <summary>
        /// The cooldown of the spell in milliseconds
        /// </summary>
        public uint cooldown { get { return _cooldown; } set { _cooldown = value; } }

        /// <summary>
        /// The global cooldown of the spell in milliseconds
        /// </summary>
        public uint gcd { get { return _gcd; } set { _gcd = value; } }

        // SpellCategories.dbc

        /// <summary>
        /// Spell category (for shared cooldowns, effects?)
        /// </summary>
        public uint category { get { return _category; } }

        // SpellDuration.dbc

        /// <summary>
        /// The duration of the aura in milliseconds
        /// </summary>
        public float duration { get { return _duration; } set { _duration = value; } }

        // SpellRuneCost.dbc

        /// <summary>
        ///  The rune cost of the spell. The following values can be combined for the 
        ///  rune cost: 1: blood, 4: unholy, 16: frost, 64: death
        /// </summary>
        public DKRune rune_cost { get { return _rune_cost; } set { _rune_cost = value; } }

        /// <summary>
        /// The runic power gain when the ability is used, multiplied by 10
        /// </summary>
        public float runic_power_gain { get { return (float)(_runic_power_gain * 0.1f); } set { _prj_speed = (uint)(value * 10); } }

        // SpellAuraOptions.dbc

        /// <summary>
        /// The maximum stack of the aura
        /// </summary>
        public uint max_stack { get { return _max_stack; } set { _max_stack = value; } }

        /// <summary>
        /// The percent chance for the spell to trigger
        /// </summary>
        public float proc_chance { get { return (float)(_proc_chance * 0.01f); } set { _proc_chance = (uint)(value * 100); } }

        /// <summary>
        /// The number of stacks per trigger for the aura
        /// </summary>
        public uint initial_stacks { get { return _proc_charges; } set { _proc_charges = value; } }

        /// <summary>
        /// Proc flags, no support for now
        /// </summary>
        public uint proc_flags { get { return _proc_flags; } }

        /// <summary>
        /// The internal cooldown of the spell in milliseconds
        /// </summary>
        public uint internal_cooldown { get { return _internal_cooldown; } set { _internal_cooldown = value; } }

        /// <summary>
        /// Base real procs per minute
        /// </summary>
        public float rppm { get { return _rppm; } }

        // SpellEquippedItems.dbc

        public uint equipped_class { get { return _equipped_class; } }
        public uint equipped_invtype_mask { get { return _equipped_invtype_mask; } }
        public uint equipped_subclass_mask { get { return _equipped_subclass_mask; } }

        // SpellScaling.dbc

        /// <summary>
        /// The minimum cast time of the spell (used only for low level characters)
        /// </summary>
        public int cast_min { get { return _cast_min; } set { _cast_min = value; } }

        /// <summary>
        /// The maximum (normal) cast time of the spell
        /// </summary>
        public int cast_max { get { return _cast_max; } set { _cast_max = value; } }

        /// <summary>
        /// A divisor used in the formula for casting time scaling (20 always?)
        /// </summary>
        public int cast_div { get { return _cast_div; } }

        /// <summary>
        /// A scaling multiplier for level based scaling
        /// </summary>
        public float scaling_multiplier { get { return _c_scaling; } }

        /// <summary>
        /// A scaling divisor for level based scaling
        /// </summary>
        public uint scaling_threshold { get { return _c_scaling_level; } }

        // SpecializationSpells.dbc 

        public uint replace_spell_id { get { return _replace_spell_id; } }

        // Spell.dbc flags

        /// <summary>
        /// Spell.dbc "flags", record field 1..10, note that 12694 added a field here after flags_7
        /// </summary>
        public List<uint> attribute { get { return _attribute; } }

        /// <summary>
        /// Spell.dbc description stringblock
        /// </summary>
        public string desc { get { return _desc; } }

        /// <summary>
        /// Spell.dbc tooltip stringblock
        /// </summary>
        public string tooltip { get { return _tooltip; } }

        // SpellDescriptionVariables.dbc

        /// <summary>
        /// Spell description variable stringblock, if present
        /// </summary>
        public string desc_vars { get { return _desc_vars; } }

        // SpellIcon.dbc

        public string icon { get { return _icon; } }
        public string active_icon { get { return _active_icon; } }
        public string rank_str { get { return _rank_str; } }
        public string effect_2 { get { return _effect_2; } }
        public string effect_3 { get { return _effect_3; } }
    }
}
