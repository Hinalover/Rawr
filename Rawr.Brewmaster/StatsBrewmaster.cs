using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Brewmaster
{
    /// <summary>
    /// Brewmaster custom implementation of the Stats object to expand it with Brewmaster Specific variables
    /// </summary>
    public class StatsBrewmaster : Stats
    {
        /// <summary>
        /// The range of your Fists of Fury is increased by 5 yards.
        /// </summary>
        public bool PvP_2_Piece { get; set; }
        /// <summary>
        /// Your Touch of Death can be used on players with 10% or less health, instantly killing them.
        /// </summary>
        public bool PvP_4_Piece { get; set; }
        /// <summary>
        /// Increases the chance to dodge granted by your Elusive Brew ability by an additional 5%.
        /// </summary>
        public bool Tier_14_2_piece { get; set; }
        /// <summary>
        /// Increases the damage absorbed by your Guard ability by 20%.
        /// </summary>
        public bool Tier_14_4_piece { get; set; }
    }
}
