using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    public class DBCSpellEffect
    {
        private uint _id;
        private uint _flags;
        private uint _spell_id;
        private uint _index;
        private EffectType _type;
        private EffectSubtype _subtype;
        // SpellScaling.dbc
        private float _m_avg;
        private float _m_delta;
        private float _m_unk;
        //
        private float _coeff;
        private float _ap_coeff;
        private float _amplitude;
        // SpellRadius.dbc
        private float _radius;
        private float _radius_max;
        //
        private int _base_value;
        private int _misc_value;
        private int _misc_value_2;
        private uint _flags1;
        private uint _flags2;
        private uint _flags3;
        private uint _flags4;
        private uint _trigger_spell_id;
        private float _m_chain;
        private float _pp_combo_points;
        private float _real_ppl;
        private int _die_sides;

        public DBCSpellData spell;
        public SpellCompilation trigger;
        public int player_level = StatConversion.DEFAULTPLAYERLEVEL;

        private bool _ptr = false;
        public bool PTR
        {
            get { return _ptr; }
            set
            {
                _ptr = value;
                if (_trigger_spell_id > 0)
                    trigger = this.getTriggerSpell();
            }
        }

        public DBCSpellEffect()
        {
            _id = 0;
            _flags = 0x00;
            _spell_id = 0;
            _index = 0;
            _type = EffectType.E_NONE;
            _subtype = EffectSubtype.A_NONE;
            _m_avg = 0;
            _m_delta = 0;
            _m_unk = 0;
            _coeff = 0;
            _ap_coeff = 0;
            _amplitude = 0;
            _radius = 0;
            _radius_max = 0;
            _base_value = 0;
            _misc_value = 0;
            _misc_value_2 = 0;
            _flags1 = 0;
            _flags2 = 0;
            _flags3 = 0;
            _flags4 = 0;
            _trigger_spell_id = 0;
            _m_chain = 0;
            _pp_combo_points = 0;
            _real_ppl = 0;
            _die_sides = 0;
        }

        public DBCSpellEffect(uint a, uint b, uint c, uint d, EffectType e, EffectSubtype f, float g, float h,
            float i, float j, float j2, float k, float l, float m, int n, int o, int p, uint p2, uint p3, uint p4, uint p5, uint q, float r, float s,
            float t, int u)
        {
            _id = a;
            _flags = b;
            _spell_id = c;
            _index = d;
            _type = e;
            _subtype = f;
            _m_avg = g;
            _m_delta = h;
            _m_unk = i;
            _coeff = j;
            _ap_coeff = j2;
            _amplitude = k;
            _radius = l;
            _radius_max = m;
            _base_value = n;
            _misc_value = o;
            _misc_value_2 = p;
            _flags1 = p2;
            _flags2 = p3;
            _flags3 = p4;
            _flags4 = p5;
            _trigger_spell_id = q;
            _m_chain = r;
            _pp_combo_points = s;
            _real_ppl = t;
            _die_sides = u;
        }

        /// <summary>
        /// Effect id
        /// </summary>
        public uint id { get { return _id; } }

        /// <summary>
        /// Effect index for the spell
        /// </summary>
        public uint index { get { return _index; } }

        /// <summary>
        /// Spell this effect belongs to
        /// </summary>
        public uint spell_id { get { return _spell_id; } }

        /// <summary>
        /// Effect type
        /// </summary>
        public EffectType type { get { return _type; } }

        /// <summary>
        /// Effect sub-type
        /// </summary>
        public EffectSubtype subtype { get { return _subtype; } }

        /* 
         * The base_value field defines the "base value" of the effect in most cases, where there is 
         * no (player level, or item level) based scaling applied to it. For example with many buffs 
         * that affect the actor with a percentage modifier (This is arguably the most typical use of 
         * the value), this field specifies the percent modifier as an integer. Unfortunately, the 
         * meaning of the field is always dependant on the effect type, and ultimately, what Blizzard 
         * intended it to be. For example, it can also indicate the number of units to summon for a 
         * certain type of spell.
         */
        /// <summary>
        /// The base value of the effect
        /// </summary>
        public int base_value { get { return _base_value; } set { _base_value = value; } }

        /// <summary>
        /// Effect value percentage
        /// </summary>
        public float percent { get { return (float)(_base_value * 0.01); } }

        /// <summary>
        /// Determinds what resource is returned
        /// </summary>
        public ResourceType resource_gain_type
        {
            get
            {
                switch(_misc_value)
                {
                    case -2:    return ResourceType.Health;
                    case 0:     return ResourceType.Mana;
                    case 1:     return ResourceType.Rage;
                    case 2:     return ResourceType.Focus;
                    case 3:     return ResourceType.Energy;
                    case 4:     return ResourceType.MonkEnergy;
                    case 5:     return ResourceType.Rune;
                    case 6:     return ResourceType.RunicPower;
                    case 7:     return ResourceType.SoulShard;
                    case 9:     return ResourceType.HolyPower;
                    case 12:    return ResourceType.Chi;
                    case 13:    return ResourceType.ShadowOrbs;
                    case 14:    return ResourceType.BurningEmbers;
                    case 15:    return ResourceType.DemonicFury;
                    default:    return ResourceType.None;
                }
            }
        }

        /// <summary>
        /// Resource value based on Resource type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float resource(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.RunicPower:
                case ResourceType.Rage:
                    return _base_value * 0.1f;
                case ResourceType.Mana:
                    return _base_value * 0.01f;
                default:
                    return (float)_base_value;
            }
        }

        /// <summary>
        /// Returns the mastery value of the coefficient.
        /// </summary>
        public float mastery_value { get { return _coeff * 0.01f; } }

        /// <summary>
        /// The first "misc" value of the effect (use is effect type, or effect dependant)
        /// </summary>
        public int misc_value1 { get { return _misc_value; } set { _misc_value = value; } }

        /// <summary>
        /// The second "misc" value of the effect (use is effect type, or effect dependant)
        /// </summary>
        public int misc_value2 { get { return _misc_value_2; } set { _misc_value_2 = value; } }

        /// <summary>
        /// Effect triggers this spell id
        /// </summary>
        public uint trigger_spell_id { get { return _trigger_spell_id; } }

        /// <summary>
        /// The multiplier to apply spells when they jump from target to target
        /// </summary>
        public float chain_multiplier { get { return _m_chain; } set { _m_chain = value; } }

        /// <summary>
        /// Effect average spell scaling multiplier
        /// </summary>
        public float m_average { get { return _m_avg; } set { _m_avg = value; } }

        /// <summary>
        /// Effect delta spell scaling multiplier
        /// </summary>
        public float m_delta { get { return _m_delta; } set { _m_delta = value; } }

        /// <summary>
        /// Unused effect scaling multiplier
        /// </summary>
        public float m_unk { get { return _m_unk; } }

        /// <summary>
        /// The (spell power) coefficient for the effect
        /// </summary>
        public float coeff { get { return _coeff; } set { _coeff = value; } }

        /// <summary>
        /// The base tick time of the effect in milliseconds
        /// </summary>
        public float period { get { return _amplitude; } set { _amplitude = value; } }

        /// <summary>
        /// Minimum spell radius
        /// </summary>
        public float radius { get { return _radius; } }

        /// <summary>
        /// Maximum spell radius
        /// </summary>
        public float radius_max { get { return _radius_max; } }

        /// <summary>
        /// "Old style" version of the bonus field. Unused
        /// </summary>
        public float pp_combo_points { get { return _pp_combo_points; } set { _pp_combo_points = value; } }

        /// <summary>
        /// "Old style" version of the average field. Unused
        /// </summary>
        public float real_ppl { get { return _real_ppl; } set { _real_ppl = value; } }

        /// <summary>
        ///  "Old style" version of the delta field. Unused
        /// </summary>
        public int die_sides { get { return _die_sides; } set { _die_sides = value; } }

        /// <summary>
        /// Returns information on the Triggered spell
        /// </summary>
        /// <returns></returns>
        private SpellCompilation getTriggerSpell ()
        {
            if (_trigger_spell_id != 0)
            {
                if (_ptr)
                {
                    SpellCompilation ptr = new DBCPTR().getSpellInfo(_trigger_spell_id);
                    ptr.PTR = _ptr;
                    return ptr;
                }
                else
                    return new DBCLive().getSpellInfo(_trigger_spell_id);
            }
            else
                return new SpellCompilation();
        }

        /// <summary>
        /// Returns the scaling factor for the given class
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private float spell_scaling(int level)
        {
            switch (spell.scaling_class)
            {
                case CharacterClass.Warrior:        return BaseCombatRating.WarriorSpellScaling(level);
                case CharacterClass.Paladin:        return BaseCombatRating.PaladinSpellScaling(level);
                case CharacterClass.Hunter:         return BaseCombatRating.HunterSpellScaling(level);
                case CharacterClass.Rogue:          return BaseCombatRating.RogueSpellScaling(level);
                case CharacterClass.Priest:         return BaseCombatRating.PriestSpellScaling(level);
                case CharacterClass.DeathKnight:    return BaseCombatRating.DeathKnightSpellScaling(level);
                case CharacterClass.Shaman:         return BaseCombatRating.ShamanSpellScaling(level);
                case CharacterClass.Mage:           return BaseCombatRating.MageSpellScaling(level);
                case CharacterClass.Warlock:        return BaseCombatRating.WarlockSpellScaling(level);
                case CharacterClass.Monk:           return BaseCombatRating.MonkSpellScaling(level);
                case CharacterClass.Druid:          return BaseCombatRating.DruidSpellScaling(level);
				case CharacterClass.Player_Special_Scale: return BaseCombatRating.ConstantSpellScaling(level);
				case CharacterClass.Player_Special_Scale2: return BaseCombatRating.ConstantSpellScaling2(level);
				case CharacterClass.Player_Special_Scale3: return BaseCombatRating.ConstantSpellScaling3(level);
				case CharacterClass.Player_Special_Scale4: return BaseCombatRating.ConstantSpellScaling4(level);
                default:                            return 0;
            }
        }

        /// <summary>
        /// Returns the scaled average of the effect
        /// </summary>
        /// <param name="budget"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private float scaled_avarage(float budget, int level)
        {
            if ((_m_avg != 0) && (spell.scaling_class != CharacterClass.None))
                return _m_avg * budget;
            else if (_real_ppl != 0)
            {
                if (spell.max_level > 0)
                    return _base_value + (Math.Min(level, spell.max_level) - spell.level) * _real_ppl;
                else
                    return _base_value + (level - spell.level) * _real_ppl;
            }
            else
                return _base_value;
        }

        /// <summary>
        /// Returns the scaled delta of the effect
        /// </summary>
        /// <param name="budget"></param>
        /// <returns></returns>
        private float scaled_delta(float budget)
        {
            if ((_m_delta != 0) && (budget > 0))
                return _m_avg * _m_delta * budget;
            else if ((_m_avg == 0.0) && (_m_delta == 0.0) && (_die_sides != 0))
                return die_sides;
            else
                return 0;
        }

        /// <summary>
        /// Returns the scaled minimum damage of the effect
        /// </summary>
        /// <param name="avg"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        private float scaled_min(float avg, float delta)
        {
            float result = 0;

            if ((_m_avg != 0) || (_m_delta != 0))
                result = avg - (delta / 2);
            else
                result = avg - delta;

            switch (_type)
            {
                case EffectType.E_WEAPON_PERCENT_DAMAGE:
                    result *= 0.01f;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns the scaled maximum damage of the effect
        /// </summary>
        /// <param name="avg"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        private float scaled_max(float avg, float delta)
        {
            float result = 0;

            if ((_m_avg != 0) || (_m_delta != 0))
                result = avg + (delta / 2);
            else
                result = avg + delta;

            switch (_type)
            {
                case EffectType.E_WEAPON_PERCENT_DAMAGE:
                    result *= 0.01f;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// The average amount scaling coefficient for the effect
        /// </summary>
        /// <returns></returns>
        public float average
        {
            get
            {
                float m_scale = 0;

                if ((_m_avg != 0) && (spell.scaling_class != CharacterClass.None))
                {
                    int scaling_level = player_level;
                    if (spell.max_scaling_level > 0)
                        scaling_level = (int)Math.Min(scaling_level, spell.max_scaling_level);
                    m_scale = spell_scaling(scaling_level);
                }

                return scaled_avarage(m_scale, player_level);
            }
        }

		/// <summary>
		/// The average amount scaling coefficient for the effect on an item
		/// </summary>
		/// <returns></returns>
		public float Average(Item item)
		{
			if (item.IsGem)
			{
				return _base_value;
			}

			float m_scale = 0;

			if ((_m_avg != 0) && (spell.scaling_class != CharacterClass.None))
			{
				m_scale = ItemInstance.GetEffectItemBudget(item, 0, (int)spell.max_scaling_level);
			}

			return scaled_avarage(m_scale, item.ItemLevel);
		}

        /// <summary>
        /// Takes in either Spell Power or Attack Power and returns power modified average
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public float Average(float power)
        {
            if (_coeff > 0)
                return average + (power * _coeff);
            else
                return average;
        }

        /// <summary>
        /// The delta amount scaling coefficient for the effect
        /// </summary>
        /// <returns></returns>
        public float delta
        {
            get
            {
                float m_scale = 0;
                if ( (_m_delta != 0) && (spell.scaling_class != CharacterClass.None))
                {
                    int scaling_level = player_level;
                    if ( spell.max_scaling_level > 0)
                        scaling_level = (int)Math.Min(scaling_level, spell.max_scaling_level);
                    m_scale = spell_scaling(scaling_level);
                }

                return scaled_delta( m_scale);
            }
        }

        /// <summary>
        /// Returns the minimum damage of the effect
        /// </summary>
        /// <returns></returns>
        public float min { get { return scaled_min(average, delta); } }

        /// <summary>
        /// Takes in either Spell Power or Attack Power and returns power modified minimum damge
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public float Min(float power)
        {
            if (_coeff > 0)
                return scaled_min( Average( power ), delta );
            else
                return min;
        }

        /// <summary>
        /// Returns the maximum damage of the effect
        /// </summary>
        /// <returns></returns>
        public float max { get { return scaled_max(average, delta); } }

        /// <summary>
        /// Takes in either Spell Power or Attack Power and returns power modified maximum damge
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public float Max(float power)
        {
            if (_coeff > 0)
                return scaled_max( Average( power ), delta );
            else
                return min;
        }

        /// <summary>
        /// The bonus (in essence, per combo point) amount scaling coefficient for the effect
        /// </summary>
        public float bonus
        {
            get
            {
                if ((player_level > 0) && (player_level <= StatConversion.DEFAULTPLAYERLEVEL))
                {
                    if ((_m_unk != 0) && (spell.scaling_class != CharacterClass.None))
                    {
                        int scaling_level = player_level;
                        if (spell.max_scaling_level > 0)
                            scaling_level = (int)Math.Min(scaling_level, spell.max_scaling_level);
                        float m_scale = spell_scaling(scaling_level);

                        if (m_scale != 0)
                            return _m_unk * m_scale;
                        else
                            return 0;
                    }
                    else
                        return _pp_combo_points;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the base number of ticks from a damage over time spell
        /// </summary>
        public float base_tick_count
        {
            get
            {
                if ((spell.duration != 0) && (_amplitude != 0))
                    return spell.duration / _amplitude;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the number of ticks based on the supplied haste %
        /// </summary>
        /// <param name="haste_percent"></param>
        /// <returns></returns>
        public float Hasted_tick_count(float haste_percent)
        {
            if ((spell.duration != 0) && (_amplitude != 0))
            {
                float _base_tick_count = this.base_tick_count;
                float _adjusted_tick_speed = _amplitude;
                float _last_adjusted_tick_speed = _amplitude;
                float _tickspeed = _amplitude;
                float _num_ticks = _base_tick_count;
                float _hasted_amplitude = (_amplitude / (1 + haste_percent));

                for (int i = 1; i <= (_base_tick_count * 5); i++)
                {
                    _tickspeed = spell.duration / (_base_tick_count + i);

                    // Calculate whole breakpoints
                    if (((float)(spell.duration % Math.Round(_tickspeed, 1)) / _tickspeed) == 0.5f)
                        if (((spell.duration / _tickspeed) % 2) != 0)
                            _adjusted_tick_speed = (float)Math.Floor(_tickspeed - 1) + 0.4999f;
                        else
                            _adjusted_tick_speed = (float)Math.Floor(_tickspeed) + 0.4999f;
                    else
                        _adjusted_tick_speed = (float)Math.Floor(_tickspeed) + 0.4999f;

                    // If the hasted amplitude between the last whole break and this whole break, then we return the number count
                    if ( _hasted_amplitude > _adjusted_tick_speed)
                    {
                        _num_ticks = _base_tick_count + (i - 1);
                        // calculate the partial tick percentage based on where the hasted amplitude
                        // is in comparison to the last break point and this break point.
                        _num_ticks += (_last_adjusted_tick_speed - _hasted_amplitude) / (_last_adjusted_tick_speed - _adjusted_tick_speed);
                        return _num_ticks;
                    }
                    else
                        _last_adjusted_tick_speed = _adjusted_tick_speed;
                }

                return _num_ticks;
            }
            else
                return 0;
        }
    }
}
