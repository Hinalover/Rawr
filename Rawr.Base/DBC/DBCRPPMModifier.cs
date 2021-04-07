using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    public class DBCRPPMModifier
    {
        public readonly uint id;
        public readonly Specialization specialization;
        public readonly float value;

        public DBCRPPMModifier()
        {
            id = 0;
            specialization = Specialization.SPEC_NONE;
            value = 0;
        }

        public DBCRPPMModifier(uint a, Specialization b, float c)
        {
            id = a;
            specialization = b;
            value = c;
        }
    }
}
