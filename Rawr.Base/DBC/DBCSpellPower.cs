using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    public class DBCSpellPower
    {
        private uint _id;
        private uint _spell_id;
        private uint _aura_id;
        private ResourceType _power_e;
        private int _cost;
        private float _cost_2;
        private int _cost_per_second;
        private float _cost_per_second_2;

        public DBCSpellData spell;
        public SpellCompilation aura;

        private bool _ptr = false;
        public bool PTR
        {
            get { return _ptr; }
            set
            {
                _ptr = value;
                aura = this.getAuraSpell();
            }
        }


        public DBCSpellPower()
        {
            _id = 0;
            _spell_id = 0;
            _aura_id = 0;
            _power_e = ResourceType.Mana;
            _cost = 0;
            _cost_2 = 0;
            _cost_per_second = 0;
            _cost_per_second_2 = 0;
        }

        public DBCSpellPower(uint a, uint b, uint c, ResourceType d, int e, float f, int g, float h)
        {
            _id = a;
            _spell_id = b;
            _aura_id = c;
            _power_e = d;
            _cost = e;
            _cost_2 = f;
            _cost_per_second = g;
            _cost_per_second_2 = h;
        }

        /// <summary>
        /// Returns the Spell Resource ID
        /// </summary>
        public uint id { get { return _id; } }

        /// <summary>
        /// Returns the Spell ID the Resource is associated with
        /// </summary>
        public uint spell_id { get { return _spell_id; } }

        /// <summary>
        /// Returns the Aura ID
        /// </summary>
        public uint aura_id { get { return _aura_id; } }

        /// <summary>
        /// Returns the Resource Type
        /// </summary>
        public ResourceType type { get { return _power_e; } }

        /// <summary>
        /// Returns the Cost divisor based on the type of resource being used
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        private float cost_divisor(bool percentage)
        {
            switch (_power_e)
            {
                case ResourceType.Mana:
                case ResourceType.SoulShard:
                    return 100.0f;
                case ResourceType.Rage:
                case ResourceType.RunicPower:
                case ResourceType.BurningEmbers:
                    return 10.0f;
                case ResourceType.DemonicFury:
                    return percentage ? 0.1f : 1.0f;  // X% of 1000 ("base" demonic fury) is X divided by 0.1
                default:
                    return 1.0f;
            }
        }

        /// <summary>
        /// Returns the base cost of the spell
        /// </summary>
        public float cost
        {
            get
            {
                float spell_cost = ((_cost > 0) ? (float)_cost : _cost_2);

                return spell_cost / cost_divisor( ! ( _cost > 0 ) );
            }
        }

        /// <summary>
        /// Returns the resource cost for each tick of a channeled spell
        /// </summary>
        public float cost_per_second
        {
            get
            {
                float spell_cost = ((_cost_per_second > 0) ? (float)_cost_per_second : _cost_per_second_2);

                return spell_cost / cost_divisor(!(_cost > 0));
            }
        }

        /// <summary>
        /// Returns information on the Aura spell
        /// </summary>
        /// <returns></returns>
        private SpellCompilation getAuraSpell()
        {
            if (_aura_id > 0)
            {
                if (_ptr)
                {
                    SpellCompilation ptr = new DBCPTR().getSpellInfo(_aura_id);
                    ptr.PTR = _ptr;
                    return ptr;
                }
                else
                    return new DBCLive().getSpellInfo(_aura_id);
            }
            else
                return new SpellCompilation();
        }

    }
}
