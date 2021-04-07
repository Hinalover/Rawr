using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    class Gemming
    {
        ////Relevant Gem IDs for Elemental Shamans
        // { Uncommon, Rare, Epic, Jeweler }

        //Red
        private int[] brilliant = { 76562, 76694, 76694, 83150 };         // Intellect

        //Yellow
        private int[] fractured = { 76568, 76700, 76700, 83143 };         // Mastery
        private int[] smooth    = { 76565, 76697, 76697, 83146 };         // Crit
        private int[] quick     = { 76567, 76699, 76699, 83142 };         // Haste

        //Orange (R&Y)
        private int[] potent    = { 76528, 76660, 76660, 88942 };         // Int/Crit
        private int[] reckless  = { 76536, 76668, 76668, 88943 };         // Int/Haste
        private int[] artful    = { 76540, 76672, 76672, 88931 };         // Int/Mast

        //Blue
        private int[] rigid     = { 76502, 76636, 76636, 83144 };         // Hit
        private int[] sparkling = { 76505, 76638, 76638, 83149 };         // Spirit

        //Purple (B&R)
        private int[] veiled    = { 76550, 76682, 76682, 88963 };         // Int/Hit
        private int[] purified  = { 76554, 76686, 76686, 88958 };         // Int/Spi
        private int[] timeless  = { 76557, 76689, 76689, 88962 };         // Int/Sta

        //Green (B&Y)
        private int[] lightning = { 76509, 76642, 76642, 88916 };         // Haste/Hit
        private int[] senseis   = { 76510, 76643, 76643, 88923 };         // Mast/Hit
        private int[] piercing  = { 76508, 76641, 76641, 88919 };         // Crit/Hit
        private int[] energized = { 76519, 76651, 76651, 88913 };         // Haste/Spi
        private int[] zen       = { 76512, 76645, 76645, 88928 };         // Mast/Spi
        private int[] misty     = { 76507, 76640, 76640, 88917 };         // Crit/Spi
        private int[] forceful  = { 76522, 76654, 76654, 88914 };         // Haste/Sta
        private int[] puissant  = { 76524, 76656, 76656, 88920 };         // Mast/Sta
        private int[] jagged    = { 76520, 76652, 76652, 88915 };         // Crit/Sta
        private int[] nimble    = { 76523, 76655, 76655, 88918 };         // Hit/Sta

        //Cogwheel
        private int[] cog_fractured = { 77547 };                          // Mastery
        private int[] cog_sparkling = { 77546 };                          // Spirit
        private int[] cog_quick     = { 77542 };                          // Haste
        private int[] cog_rigid     = { 77545 };                          // Hit
        private int[] cog_smooth    = { 77541 };                          // Crit

        public List<GemmingTemplate> addTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() 
            {

            	new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Hit - no colour match
					RedId = rigid[rarity], YellowId = rigid[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Spirit - no colour match
					RedId = sparkling[rarity], YellowId = sparkling[rarity], BlueId = sparkling[rarity], PrismaticId = sparkling[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Int - no colour match
					RedId = brilliant[rarity], YellowId = brilliant[rarity], BlueId = brilliant[rarity], PrismaticId = brilliant[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Crit - no colour match
					RedId = smooth[rarity], YellowId = smooth[rarity], BlueId = smooth[rarity], PrismaticId = smooth[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Haste - no colour match
					RedId = quick[rarity], YellowId = quick[rarity], BlueId = quick[rarity], PrismaticId = quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,  //Max Mastery - no colour match
					RedId = fractured[rarity], YellowId = fractured[rarity], BlueId = fractured[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
            };
        }

        public List<GemmingTemplate> addCogwheelTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>()
            {
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_sparkling[rarity], Cogwheel2Id = cog_rigid[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_sparkling[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_sparkling[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_sparkling[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Elemental", Group = group, Enabled = enabled,
                    CogwheelId = cog_smooth[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },
            };
        }
    }
}
