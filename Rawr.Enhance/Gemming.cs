using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class Gemming
    {
        ////Relevant Gem IDs for Enhancement Shamans
        // { Uncommon, Rare, Epic, Jeweler }

        //Red
        private int[] delicate  = { 76560, 76692, 76692, 83151 };         // Agility
        private int[] precise   = { 76561, 76693, 76693, 83147 };         // Expertise

        //Yellow
        private int[] fractured = { 76568, 76700, 76700, 83143 };         // Mastery
        private int[] smooth    = { 76565, 76697, 76697, 83146 };         // Crit
        private int[] quick     = { 76567, 76699, 76699, 83142 };         // Haste

        //Orange (R&Y)
        private int[] deadly    = { 76526, 76658, 76658, 88934 };         // Agi/Crit
        private int[] deft      = { 76534, 76666, 76666, 88935 };         // Agi/Haste
        private int[] adept     = { 76538, 76670, 76670, 88930 };         // Agi/Mast
        private int[] crafty    = { 76527, 76659, 76659, 88933 };         // Exp/Crit
        private int[] wicked    = { 76535, 76667, 76667, 88950 };         // Exp/Haste
        private int[] keen      = { 76539, 76671, 76671, 88939 };         // Exp/Mast

        //Blue
        private int[] rigid     = { 76502, 76636, 76636, 83144 };         // Hit

        //Purple (B&R)
        private int[] glinting  = { 76548, 76680, 76680, 88955 };         // Agi/Hit
        private int[] accurate  = { 76549, 76681, 76681, 88952 };         // Exp/Hit
        private int[] shifting  = { 76555, 76687, 76687, 88960 };         // Agi/Sta
        private int[] guardian  = { 76556, 76688, 76688, 88956 };         // Exp/Sta

        //Green (B&Y)
        private int[] lightning = { 76509, 76642, 76642, 88916 };         // Haste/Hit
        private int[] senseis   = { 76510, 76643, 76643, 88923 };         // Mast/Hit
        private int[] piercing  = { 76508, 76641, 76641, 88919 };         // Crit/Hit
        private int[] forceful  = { 76522, 76654, 76654, 88914 };         // Haste/Sta
        private int[] puissant  = { 76524, 76656, 76656, 88920 };         // Mast/Sta
        private int[] jagged    = { 76520, 76652, 76652, 88915 };         // Crit/Sta
        private int[] nimble    = { 76523, 76655, 76655, 88918 };         //Hit/Sta

        //Cogwheel
        private int[] cog_fractured = { 77547 };                          // Mastery
        private int[] cog_precise   = { 77543 };                          // Expertise
        private int[] cog_quick     = { 77542 };                          // Haste
        private int[] cog_rigid     = { 77545 };                          // Hit
        private int[] cog_smooth    = { 77541 };                          // Crit

        public List<GemmingTemplate> addTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() 
            {
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Max Expertise - colour match
					RedId = precise[rarity], YellowId = keen[rarity], BlueId = accurate[rarity], PrismaticId = precise[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Max Expertise - no colour match
					RedId = precise[rarity], YellowId = precise[rarity], BlueId = precise[rarity], PrismaticId = precise[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red exp, yellow mast
					RedId = accurate[rarity], YellowId = senseis[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red agi, yellow mast
					RedId = glinting[rarity], YellowId = senseis[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - no colour match
					RedId = rigid[rarity], YellowId = rigid[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow mast
					RedId = delicate[rarity], YellowId = adept[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow haste
					RedId = delicate[rarity], YellowId = deft[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow crit
					RedId = delicate[rarity], YellowId = deadly[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - no colour match
					RedId = delicate[rarity], YellowId = delicate[rarity], BlueId = delicate[rarity], PrismaticId = delicate[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - red agi, blue hit
					RedId = deadly[rarity], YellowId = smooth[rarity], BlueId = piercing[rarity], PrismaticId = smooth[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - no colour match
					RedId = smooth[rarity], YellowId = smooth[rarity], BlueId = smooth[rarity], PrismaticId = smooth[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red agi, blue hit
					RedId = deft[rarity], YellowId = quick[rarity], BlueId = lightning[rarity], PrismaticId = quick[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - no colour match
					RedId = quick[rarity], YellowId = quick[rarity], BlueId = quick[rarity], PrismaticId = quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red agi, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red exp, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - no colour match
					RedId = fractured[rarity], YellowId = fractured[rarity], BlueId = fractured[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
            };
        }

        public List<GemmingTemplate> addCogwheelTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>()
            {
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_rigid[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_smooth[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },
            };
        }
    }
}
