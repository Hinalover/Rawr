using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Load_SimC_DBC
{
    class RPPMModifier
    {
        private uint id;
        private Specialization specialization;
        private float value;

        public RPPMModifier()
        {
            id = 0;
            specialization = Specialization.SPEC_NONE;
            value = 0;
        }

        public RPPMModifier(uint ID, Specialization spec, float Value)
        {
            id = ID;
            specialization = spec;
            value = Value;
        }

        public RPPMModifier(List<string> list)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            id = Convert.ToUInt32(list[0]);
            specialization = (Specialization)Enum.Parse(typeof(Specialization), list[1]);
            value = float.Parse(list[2], culture);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t\t\t\tthis.Add( new DBCRPPMModifier ( ");
            sb.Append(String.Format("{0}, ", id));
            sb.Append(String.Format("Specialization.{0}, ", specialization));
            sb.Append(String.Format("{0}f", value));
            sb.Append(" ) );");
            return sb.ToString();
        }
    }
}
