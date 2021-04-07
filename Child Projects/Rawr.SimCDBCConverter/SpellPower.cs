using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Load_SimC_DBC
{
    class SpellPower
    {
        private uint id;
        private uint spellID;
        private uint stanceID;
        private ResourceType resource;
        private int resourceCost;
        private float manaCost;
        private int _cost_per_second;
        private float _cost_per_second_2;

        public SpellPower()
        {
            id = 0;
            spellID = 0;
            stanceID = 0;
            resource = ResourceType.Mana;
            resourceCost = 0;
            manaCost = 0;
            _cost_per_second = 0;
            _cost_per_second_2 = 0;
        }

        public SpellPower(uint ID, uint SpellID, uint StanceID, float ResourceCode, int ResourceCost, float ManaCost, int DoTCost, float ChannelTickCost)
        {
            id = ID;
            spellID = SpellID;
            stanceID = StanceID;
            resource = (ResourceType)ResourceCode;
            resourceCost = ResourceCost;
            manaCost = ManaCost;
            _cost_per_second = DoTCost;
            _cost_per_second_2 = ChannelTickCost;
        }

        public SpellPower(List<string> list)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            id = Convert.ToUInt32(list[0]);
            spellID = Convert.ToUInt32(list[1]);
            stanceID = Convert.ToUInt32(list[2]);
            resource = (ResourceType)float.Parse(list[3], culture);
            resourceCost = Convert.ToInt32(list[4]);
            manaCost = float.Parse(list[5], culture);
            _cost_per_second = Convert.ToInt32(list[6]);
            _cost_per_second_2 = float.Parse(list[7], culture);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t\t\t\tthis.Add( new DBCSpellPower ( ");
            sb.Append(String.Format("{0}, ", id));
            sb.Append(String.Format("{0}, ", spellID));
            sb.Append(String.Format("{0}, ", stanceID));
            sb.Append(String.Format("ResourceType.{0}, ", resource));
            sb.Append(String.Format("{0}, ", resourceCost));
            sb.Append(String.Format("{0}f, ", manaCost));
            sb.Append(String.Format("{0}, ", _cost_per_second));
            sb.Append(String.Format("{0}f", _cost_per_second_2));
            sb.Append(" ) );");
            return sb.ToString();
        }
    }
}
