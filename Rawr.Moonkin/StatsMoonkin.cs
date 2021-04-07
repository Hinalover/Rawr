using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Moonkin
{
    public class StatsMoonkin : Stats
    {
        public float BonusSpellDamageMultiplier { get; set; }
        public float BonusCritChanceInsectSwarm { get; set; }
        public float BonusCritChanceMoonfire { get; set; }
        public float BonusWrathEnergy { get; set; }
        public float BonusStarfireEnergy { get; set; }
        public float BonusNukeDamageModifier { get; set; }
        public float BonusStarsurgeDamageModifier { get; set; }
        public bool T13FourPieceActive { get; set; }
        public float BonusEclipseDamageMultiplier { get; set; }
        public float BonusStarfallDamageModifier { get; set; }
        public float BonusMoonfireDuration { get; set; }
        public float BonusMoonfireDamageMultiplier { get; set; }
        public float BonusStarsurgeCritModifier { get; set; }
        public bool T16TwoPieceActive { get; set; }
        public float BonusShootingStarsChance { get; set; }
    }
}
