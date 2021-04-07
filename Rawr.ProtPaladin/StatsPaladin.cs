using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ProtPaladin
{
    public class StatsProtPaladin : Stats
    {
        public float AverageVengeanceAP { get; set; }
        public float BonusDamageMultiplierCrusaderStrike { get; set; }
        public float BonusDurationMultiplierGuardianOfAncientKings { get; set; }
        public float BonusDamageShieldofRighteous { get; set; }
        public float JudgementAbsorbShield { get; set; }
    }
}
