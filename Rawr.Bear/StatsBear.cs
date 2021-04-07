using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Bear
{
    /// <summary>
    /// Bear custom implementation of the Stats object to expand it with Bear Specific variables
    /// </summary>
    public class StatsBear : Stats
    {
        /// <summary>
        /// Increases movement speed of the Druid
        /// </summary>
        public float GuardianMovementSpeed { get; set; }
        /// <summary>
        /// Increases the periodic damage done by your Rake and Lacerate abilities by 10%.
        /// </summary>
        public bool Tier_11_2_piece { get; set; }
        /// <summary>
        /// The duration of your Survival Instinct ability is increased by 50%.
        /// </summary>
        public bool Tier_11_4_piece { get; set; }
        /// <summary>
        /// Your attacks with Mangle, Maul, and Shred deal 10% additional damage as Fire damage over 4 sec.
        /// </summary>
        public bool Tier_12_2_piece { get; set; }
        /// <summary>
        /// When your Barkskin ability expires you gain an additional 10% chance to dodge for 12 sec.
        /// </summary>
        public bool Tier_12_4_piece { get; set; }
        /// <summary>
        /// Reduces the rage cost of your Savage Defense by 5.
        /// </summary>
        public bool Tier_13_2_piece { get; set; }
        /// <summary>
        /// Might of Ursoc also affects all raid and party members.
        /// </summary>
        public bool Tier_13_4_piece { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Might of Ursoc ability by 60 sec.
        /// </summary>
        public bool Tier_14_2_piece { get; set; }
        /// <summary>
        /// Increases the healing received from your Frenzied Regeneration by 10% and increases the dodge granted by your Savage Defense by 10%.
        /// </summary>
        public bool Tier_14_4_piece { get; set; }
        /// <summary>
        /// Added support for Physical Damage from Proc.
        /// </summary>
        public float PhysicalDamageProc { get; set; }
        /// <summary>
        /// Bonus Critical Strike Rating Multiplier
        /// </summary>
        public float BonusCritRatingMultiplier { get; set; }
        /// <summary>
        /// Bonus Haste Rating Multiplier
        /// </summary>
        public float BonusHasteRatingMultiplier { get; set; }
    }
}
