using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace Rawr.Bear
{
    /// <summary>
    /// Core class representing the Rawr.Bear model
    /// </summary>
    [Rawr.Calculations.RawrModelInfo("Bear", "Ability_Racial_BearForm", CharacterClass.Druid)]
    public class CalculationsBear : CalculationsBase
    {
        #region Variables and Properties
        #region Gemming Templates
        private string[] tierNames = { "Uncommon", "Rare", "Epic", "Jewelcrafter" };
        #region Meta Gems
        /// <summary>
        /// Agility + 3% Critical Strike Damage Multiplier Meta Gem
        /// </summary>
        private int agile = 76884; // 216 Agility, 3% Crit Dmg - 3R
        /// <summary>
        /// Stamina + 2% Base Armor
        /// </summary>
        private int Austere = 76895; // 324 Stamina, 2% Base Armor - 2Y
        #endregion

        #region Red Gems
        /// <summary>
        /// Agility Red Gems
        /// </summary>
        private int[] delicate = { 76560, 76692, 76692, 83151 };
        /// <summary>
        /// Expertise Rating Red Gems
        /// </summary>
        private int[] precise = { 76561, 76693, 76693, 83147 };
        #endregion

        #region Blue Gems
        /// <summary>
        /// Hit Rating Blue Gems
        /// </summary>
        private int[] rigid = { 76502, 76636, 76636, 83144 };
        /// <summary>
        /// Stamina Blue Gems
        /// </summary>
        private int[] solid = { 76506, 76639, 76639, 83148 };
        #endregion

        #region Yellow Gems
        /// <summary>
        /// Mastery Rating Yellow Gems
        /// </summary>
        private int[] fractured = { 76568, 76700, 76700, 83143 };
        /// <summary>
        /// Haste Rating Yellow Gems
        /// </summary>
        private int[] quick = { 76567, 76699, 76699, 83142 };
        /// <summary>
        /// Critical Strike Rating Yellow Gems
        /// </summary>
        private int[] smooth = { 76565, 76697, 76697, 83146 };
        /// <summary>
        /// Dodge Rating Yellow Gems
        /// </summary>
        private int[] subtle = { 76566, 76698, 76698, 83145 };
        #endregion

        #region Purple Gems
        /// <summary>
        /// Expertise Rating + Hit Rating Purple Gems
        /// </summary>
        private int[] accurate = { 76549, 76681, 88952, 76681 };
        /// <summary>
        /// Agility + Hit Rating Purple Gems
        /// </summary>
        private int[] glinting = { 76548, 76680, 88955, 76680 };
        /// <summary>
        /// Expertise Rating + Stamina Purple Gems
        /// </summary>
        private int[] guardian = { 76556, 76688, 88956, 76688 };
        /// <summary>
        /// Agility + Stamina Purple Gems
        /// </summary>
        private int[] shifting = { 76555, 76687, 88960, 76687 };
        #endregion

        #region Green Gems
        /// <summary>
        /// Haste Rating + Stamina Green Gems
        /// </summary>
        private int[] forceful = { 76522, 76654, 88914, 76654 };
        /// <summary>
        /// Critical Strike Rating + Stamina Green Gems
        /// </summary>
        private int[] jagged = { 76520, 76652, 88915, 76652 };
        /// <summary>
        /// Haste Rating + Hit Rating Green Gems
        /// </summary>
        private int[] lightning = { 76509, 76642, 88916, 76642 };
        /// <summary>
        /// Dodge Rating + Hit Rating Green Gems
        /// </summary>
        private int[] nimble = { 76523, 76655, 88918, 76655 };
        /// <summary>
        /// Critical Strike Rating + Hit Rating Green Gems
        /// </summary>
        private int[] piercing = { 76508, 76641, 88919, 76641 };
        /// <summary>
        /// Mastery Rating + Stamina Green Gems
        /// </summary>
        private int[] puissant = { 76524, 76656, 88920, 76656 };
        /// <summary>
        /// Dodge Rating + Stamina Green Gems
        /// </summary>
        private int[] regal = { 76521, 76653, 88922, 76653 };
        /// <summary>
        /// Mastery Rating + Hit Rating Green Gems
        /// </summary>
        private int[] sensei = { 76510, 76643, 88923, 76643 };
        #endregion

        #region Orange Gems
        /// <summary>
        /// Agility + Mastery Rating Orange Gems
        /// </summary>
        private int[] adept = { 76538, 76670, 88930, 76670 };
        /// <summary>
        /// Expertise Rating + Critical Strike Rating Orange Gems
        /// </summary>
        private int[] crafty = { 76527, 76659, 88933, 76659 };
        /// <summary>
        /// Agility + Critical Strike Rating Orange Gems
        /// </summary>
        private int[] deadly = { 76526, 76658, 88934, 76658 };
        /// <summary>
        /// Agility + Haste Rating Orange Gems
        /// </summary>
        private int[] deft = { 76534, 76666, 88935, 76666 };
        /// <summary>
        /// Expertise Rating + Mastery Rating Orange Gems
        /// </summary>
        private int[] keen = { 76539, 76671, 88939, 76671 };
        /// <summary>
        /// Agility + Dodge Rating Orange Gems
        /// </summary>
        private int[] polished = { 76530, 76662, 88941, 76662 };
        /// <summary>
        /// Expertise Rating + Dodge Rating Orange Gems
        /// </summary>
        private int[] resolute = { 76531, 76663, 88944, 76663 };
        /// <summary>
        /// Expertise Rating + Haste Rating Orange Gems
        /// </summary>
        private int[] wicked = { 76535, 76667, 88950, 76667 };
        #endregion

        #region Cogwheels
        /// <summary>
        /// Mastery Rating Cogwheel
        /// </summary>
        private int cog_fractured = 77547;
        /// <summary>
        /// Expertise Rating Cogwheel
        /// </summary>
        private int cog_precise = 77543;
        /// <summary>
        /// Haste Rating Cogwheel
        /// </summary>
        private int cog_quick = 77542;
        /// <summary>
        /// Hit Rating Cogwheel
        /// </summary>
        private int cog_rigid = 77545;
        /// <summary>
        /// Critical Strike Rating Cogwheel
        /// </summary>
        private int cog_smooth = 77541;
        /// <summary>
        /// Dodge Rating Cogwheel
        /// </summary>
        private int cog_subtle = 77540;
        #endregion

        #region Hydraulic
        /// <summary>
        /// Agility Hydraulic
        /// </summary>
        private int cryst_dread = 89873;
        /// <summary>
        /// Strength Hydraulic
        /// </summary>
        private int cryst_terror = 89881;
        #endregion

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                List<GemmingTemplate> retval = new List<GemmingTemplate>();
                for (int tier = 0; tier < 4; ++tier)
                {
                    retval.AddRange(GuardianGemmingTemplateBlock(tier, Austere));
                }
                retval.AddRange(new GemmingTemplate[] {
                // Engineering cogwheel templates (meta and 2 cogs each, no repeats)
                CreateGaurdianCogwheelTemplate(Austere, cog_fractured, cog_precise),
                CreateGaurdianCogwheelTemplate(Austere, cog_fractured, cog_quick),
                CreateGaurdianCogwheelTemplate(Austere, cog_fractured, cog_rigid),
                CreateGaurdianCogwheelTemplate(Austere, cog_fractured, cog_smooth),
                CreateGaurdianCogwheelTemplate(Austere, cog_fractured, cog_subtle),
                CreateGaurdianCogwheelTemplate(Austere, cog_quick, cog_precise),
                CreateGaurdianCogwheelTemplate(Austere, cog_quick, cog_rigid),
                CreateGaurdianCogwheelTemplate(Austere, cog_quick, cog_smooth),
                CreateGaurdianCogwheelTemplate(Austere, cog_quick, cog_subtle),
                CreateGaurdianCogwheelTemplate(Austere, cog_rigid, cog_precise),
                CreateGaurdianCogwheelTemplate(Austere, cog_rigid, cog_smooth),
                CreateGaurdianCogwheelTemplate(Austere, cog_rigid, cog_subtle),
                CreateGaurdianCogwheelTemplate(Austere, cog_smooth, cog_precise),
                CreateGaurdianCogwheelTemplate(Austere, cog_smooth, cog_subtle),
                CreateGaurdianCogwheelTemplate(Austere, cog_precise, cog_subtle),
                });
                return retval;
            }
        }
        private List<GemmingTemplate> GuardianGemmingTemplateBlock(int tier, int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[]
            {
                #region Agility Based
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deft, delicate, delicate, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deadly, delicate, delicate, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, adept, delicate, delicate, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deft, glinting, delicate, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deft, lightning, delicate, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deft, shifting, delicate, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deadly, glinting, delicate, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deadly, piercing, delicate, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, deadly, shifting, delicate, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, adept, glinting, delicate, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, adept, sensei, delicate, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, delicate, adept, shifting, delicate, meta), // Agility/Mastery/Stamina
                #endregion
                #region Expertise Based
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, precise, precise, meta), // Expertise/Haste/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, precise, delicate, meta), // Expertise/Haste/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, precise, precise, meta), // Expertise/Crit/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, precise, delicate, meta), // Expertise/Crit/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, precise, precise, meta), // Expertise/Mastery/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, precise, delicate, meta), // Expertise/Mastery/Expertise
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, accurate, precise, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, accurate, delicate, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, lightning, delicate, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, guardian, precise, meta), // Expertise/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, wicked, guardian, delicate, meta), // Expertise/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, accurate, precise, meta), // Expertise/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, accurate, delicate, meta), // Expertise/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, piercing, delicate, meta), // Expertise/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, guardian, precise, meta), // Expertise/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, crafty, guardian, delicate, meta), // Expertise/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, accurate, precise, meta), // Expertise/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, accurate, delicate, meta), // Expertise/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, sensei, delicate, meta), // Expertise/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, guardian, precise, meta), // Expertise/Mastery/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, precise, keen, guardian, delicate, meta), // Expertise/Mastery/Stamina
                #endregion
                #region Agility/Dodge Based
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, polished, polished, polished, meta), // Agility/Dodge/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, subtle, polished, polished, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deft, polished, polished, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deadly, polished, polished, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, adept, polished, polished, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, subtle, nimble, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, subtle, lightning, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, subtle, regal, polished, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deft, nimble, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deft, regal, polished, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deadly, nimble, polished, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deadly, piercing, polished, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, deadly, regal, polished, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, adept, nimble, polished, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, adept, sensei, polished, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, polished, adept, regal, polished, meta), // Agility/Mastery/Stamina
                #endregion
                #region Expertise/Dodge Based
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, resolute, resolute, resolute, meta), // Agility/Dodge/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, subtle, resolute, resolute, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, wicked, resolute, resolute, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, crafty, resolute, resolute, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, keen, resolute, resolute, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, subtle, nimble, resolute, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, subtle, lightning, resolute, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, subtle, regal, resolute, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, wicked, nimble, resolute, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, wicked, regal, resolute, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, crafty, nimble, resolute, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, crafty, piercing, resolute, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, crafty, regal, resolute, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, keen, nimble, resolute, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, keen, sensei, resolute, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, resolute, keen, regal, resolute, meta), // Agility/Mastery/Stamina
                #endregion
                #region Agility/Mastery Based
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, polished, polished, polished, meta), // Agility/Dodge/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, subtle, polished, polished, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deft, polished, polished, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deadly, polished, polished, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, adept, polished, polished, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, subtle, nimble, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, subtle, lightning, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, subtle, regal, polished, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deft, nimble, polished, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deft, regal, polished, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deadly, nimble, polished, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deadly, piercing, piercing, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, deadly, regal, polished, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, adept, nimble, polished, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, adept, sensei, polished, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, adept, adept, regal, polished, meta), // Agility/Mastery/Stamina
                #endregion
                #region Expertise/Mastery Based
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, resolute, resolute, resolute, meta), // Expertise/Dodge/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, subtle, resolute, resolute, meta), // Expertise/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, wicked, resolute, resolute, meta), // Expertise/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, crafty, resolute, resolute, meta), // AgExpertiseility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, keen, resolute, resolute, meta), // Expertise/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, subtle, nimble, resolute, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, subtle, lightning, resolute, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, subtle, regal, resolute, meta), // Expertise/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, wicked, nimble, resolute, meta), // Expertise/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, wicked, regal, resolute, meta), // Expertise/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, crafty, nimble, resolute, meta), // Expertise/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, crafty, piercing, resolute, meta), // Expertise/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, crafty, regal, resolute, meta), // Expertise/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, keen, nimble, resolute, meta), // Expertise/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, keen, sensei, resolute, meta), // Expertise/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, keen, keen, regal, resolute, meta), // Expertise/Mastery/Stamina
                #endregion
                #region Stamina Based
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, shifting, shifting, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, solid, shifting, meta), // Agility/Haste/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, shifting, shifting, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, solid, shifting, meta), // Agility/Crit/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, shifting, shifting, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, solid, shifting, meta), // Agility/Mastery/Agility
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, glinting, shifting, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, lightning, shifting, meta), // Agility/Haste/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, shifting, shifting, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deft, solid, shifting, meta), // Agility/Haste/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, glinting, shifting, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, piercing, shifting, meta), // Agility/Crit/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, shifting, shifting, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, deadly, solid, shifting, meta), // Agility/Crit/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, glinting, shifting, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, sensei, shifting, meta), // Agility/Mastery/Hit
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, shifting, shifting, meta), // Agility/Mastery/Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, shifting, adept, solid, shifting, meta), // Agility/Mastery/Stamina
                #endregion
                #region All Stamina
                CreateGaurdianGemmingTemplate(tier, tierNames, solid, solid, solid, solid, meta), // Agility/Mastery/Stamina
                #endregion
            });
            return retval;
        }

        private const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateGaurdianGemmingTemplate(int tier, string[] tierNames, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate()
            {
                Model = "Bear",
                Group = tierNames[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta,
                HydraulicId = cryst_dread,
            };
        }

        private GemmingTemplate CreateGaurdianCogwheelTemplate(int meta, int cogwheel1, int cogwheel2)
        {
            return new GemmingTemplate
            {
                Model = "Bear",
                Group = "Engineer",
                Enabled = false,
                MetaId = meta,
                CogwheelId = cogwheel1,
                Cogwheel2Id = cogwheel2,
                HydraulicId = cryst_dread,
            };
        }
        #endregion

        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the stats available to the Optimizer 
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Health",
                    "Avoided Attacks",
                    "Hit Rating",
                    "Hit %",
                    "Expertise Rating",
                    "Expertise %",
                    "Critical Strike Rating",
                    "Haste Rating",
                    "Mastery Rating",
                    "Rage Per Second",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>
        /// Names and colors for the SubPoints that Rawr.Bear uses
        /// </summary>
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Mitigation", Colors.Red);
                    _subPointNameColors.Add("Survival", Colors.Blue);
                    _subPointNameColors.Add("Recovery", Color.FromArgb(0xFF, 0xDB, 0x70, 0x93));
                    _subPointNameColors.Add("Threat", Colors.Green);
                }
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// Labels of the stats to display on the Stats tab of the main form
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    @"Summary:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
                    @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
                    @"Summary:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                    @"Summary:Recovery Points*Any and all self-healing abilities, whether they are from talents,
Frenzied Regeneration, Leader of the Pack, or trinket/enchant related procs",
                    @"Summary:Threat Points*The TPS of your Highest TPS Rotation, multiplied by
the Threat Scale defined on the Options tab.",
                    
                    "Basic Stats:Health",
                    "Basic Stats:Stamina",
                    "Basic Stats:Armor",
                    "Basic Stats:Agility",
                    "Basic Stats:Strength",
                    "Basic Stats:Attack Power",
                    "Basic Stats:Average Vengeance AP",
                    "Basic Stats:Dodge Rating",
                    "Basic Stats:Crit Rating",
                    "Basic Stats:Haste Rating",
                    "Basic Stats:Mastery Rating",
                    "Basic Stats:Hit %",
                    "Basic Stats:Expertise %",
                    "Basic Stats:Rage Per Second",

                    @"Mitigation Stats:Savage Defense*Average amount of the dodge that this ability provides over the course of the fight",
                    @"Mitigation Stats:Pre-Dodge DR*Amount of dodge before damage reduction",
                    @"Mitigation Stats:Post-Dodge DR*Amount of dodge after damage reduction; this is what is shown on the character sheet",
                    @"Mitigation Stats:Pre-Armor DR*Amount of damage mitigated verses a target who is of the same level as the character",
                    @"Mitigation Stats:Post-Armor DR*Amount of damage mitigated verses the level of the target",
                    "Mitigation Stats:Avoidance PreDR",
                    "Mitigation Stats:Avoidance PostDR",
                    "Mitigation Stats:Total Damage Reduction",
                    "Mitigation Stats:Total Mitigation",
                    "Mitigation Stats:Damage Taken",

                    //"Survival Stats:Nature Survival",
                    //"Survival Stats:Fire Survival",
                    //"Survival Stats:Frost Survival",
                    //"Survival Stats:Shadow Survival",
                    //"Survival Stats:Arcane Survival",

                    @"Recovery Stats:Frenzied Regen*Self Heal whose value is taken from the higher of two formulas; one based on Attack Power, the other based on Stamina",
                    "Recovery Stats:Leader of the Pack",
                    "Recovery Stats:Healing Touch",
                    "Recovery Stats:Healing Touch + NS",
                    "Recovery Stats:Renewal",
                    "Recovery Stats:Cenarion Ward",
                    "Recovery Stats:Nature's Vigil",

                    "Threat Stats:Damage Done",
                    "Threat Stats:DPS",
                    "Threat Stats:TPS",
                    "Threat Stats:Melee",
                    "Threat Stats:Mangle",
                    "Threat Stats:Lacerate x 1",
                    "Threat Stats:Lacerate x 2",
                    "Threat Stats:Lacerate x 3",
                    "Threat Stats:Thrash",
                    "Threat Stats:Faerie Fire",
                    "Threat Stats:Swipe",
                    "Threat Stats:Maul",
                    "Threat Stats:Force of Nature",
                    "Threat Stats:Avoided Attacks",
                    };
                return _characterDisplayCalculationLabels;
            }
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        /// <summary>
        /// Panel to be placed on the Options tab of the main form
        /// </summary>
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelBear();
                }
                return _calculationOptionsPanel;
            }
        }

        /// <summary>
        /// The class that Rawr.Bear is designed for (Druid)
        /// </summary>
        public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
        /// <summary>
        /// Creates a new ComparisonCalculationBear instance
        /// </summary>
        /// <returns>A new ComparisonCalculationBear instance</returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationBear(); }
        /// <summary>
        /// Creates a new CharacterCalculationsBear instance
        /// </summary>
        /// <returns>A new CharacterCalculationsBear instance</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsBear(); }

        /// <summary>
        /// Deserializes the CalculationOptionsBear object contained in xml
        /// </summary>
        /// <param name="xml">The CalculationOptionsBear object, serialized as xml</param>
        /// <returns>The deserialized CalculationOptionsBear object</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsBear calcOpts = serializer.Deserialize(reader) as CalculationOptionsBear;
            return calcOpts;
        }
        #endregion

        #region Custom Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                        //"Hit Test",
                    };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                /*case "White Combat Table":
                    CharacterCalculationsCat calcs = (CharacterCalculationsCat)GetCharacterCalculations(character);
                    float[] ct = null;//calcs.MeleeStats.CombatTable;
                    return new ComparisonCalculationBase[]
                    {
                        new ComparisonCalculationCat() { Name = "Miss", OverallPoints = ct[0], DPSPoints = ct[0]},
                        new ComparisonCalculationCat() { Name = "Dodge", OverallPoints = ct[1], DPSPoints = ct[1]},
                        new ComparisonCalculationCat() { Name = "Parry", OverallPoints = ct[2], DPSPoints = ct[2]},
                        new ComparisonCalculationCat() { Name = "Glance", OverallPoints = ct[3], DPSPoints = ct[3]},
                        new ComparisonCalculationCat() { Name = "Hit", OverallPoints = ct[4], DPSPoints = ct[4]},
                        new ComparisonCalculationCat() { Name = "Crit", OverallPoints = ct[5], DPSPoints = ct[5]},
                    };
                    */
                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        #endregion

        #region Relevancy
        #region Enchants
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // No ranged enchants allowed
            if (enchant.Slot == ItemSlot.Ranged) return false;
            // No other enchants allowed on our offhands
            if (slot == ItemSlot.OffHand) return false;
            // Otherwise, return the base value
            return base.EnchantFitsInSlot(enchant, character, slot);
        }
        #endregion

        #region Glyphs
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                #region Major
                _relevantGlyphs.Add("Glyph of Cat Form");
                _relevantGlyphs.Add("Glyph of Claw");
                _relevantGlyphs.Add("Glyph of Dash");
                _relevantGlyphs.Add("Glyph of Fae Silence");
                _relevantGlyphs.Add("Glyph of Faerie Fire");
                _relevantGlyphs.Add("Glyph of Frenzied Regeneration");
                _relevantGlyphs.Add("Glyph of Master Shapeshifter");
                _relevantGlyphs.Add("Glyph of Might of Ursoc");
                _relevantGlyphs.Add("Glyph of Maul");
                _relevantGlyphs.Add("Glyph of Rebirth");
                _relevantGlyphs.Add("Glyph of Skull Bash");
                _relevantGlyphs.Add("Glyph of Stampede");
                _relevantGlyphs.Add("Glyph of Stampeding Roar");
                _relevantGlyphs.Add("Glyph of Survival Instincts");
                #endregion
                #region Minor
                _relevantGlyphs.Add("Glyph of Aquatic Form");
                _relevantGlyphs.Add("Glyph of Charm Woodland Creature");
                _relevantGlyphs.Add("Glyph of Grace");
                _relevantGlyphs.Add("Glyph of the Chameleon");
                _relevantGlyphs.Add("Glyph of the Orca");
                _relevantGlyphs.Add("Glyph of the Stag");
                #endregion
            }
            return _relevantGlyphs;
        }
        #endregion

        #region Items
        /// <summary>
        /// List of itemtypes that are relevant for Ferals
        /// </summary>
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Leather,
                        //ItemType.Dagger,
                        //ItemType.FistWeapon,
                        //ItemType.OneHandMace,
                        ItemType.TwoHandMace,
                        ItemType.Polearm,
                        ItemType.Staff,
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand ||
                (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Idol && item.Type != ItemType.Relic) ||
                item.Stats.SpellPower > 0 || item.Stats.Intellect > 0)
                return false;
            foreach (var effect in item.Stats.SpecialEffects(s => s.Stats.SpellPower > 0))
                return false;
            return base.IsItemRelevant(item);
        }
        #endregion

        #region Tiggers
        /// <summary>
        /// List of SpecialEffect Triggers that are relevant for moonkin model
        /// Every trigger listed here needs an implementation in ProcessSpecialEffects()
        /// A trigger not listed here should not appear in ProcessSpecialEffects()
        /// </summary>
        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                            Trigger.Use,
                            Trigger.MeleeAttack,
                            Trigger.MeleeHit,
                            Trigger.PhysicalHit,
                            Trigger.PhysicalAttack,
                            Trigger.MeleeCrit,
                            Trigger.PhysicalCrit,
                            Trigger.DoTTick,
                            Trigger.PhysicalHitorDoTTick,
                            Trigger.MeleeHitorDoTTick,
                            Trigger.WhiteAttack,
                            Trigger.WhiteCrit,
                            Trigger.WhiteHit,
                            Trigger.DamageDone,
                            Trigger.DamageOrHealingDone,
                            Trigger.MangleBearHit,
                            Trigger.MangleCatOrShredOrInfectedWoundsHit,
                            Trigger.DamageTakenPutsMeBelow35PercHealth,
                            Trigger.DamageTakenPutsMeBelow50PercHealth,
                        });
            }
        }
        #endregion

        #region Buffs
        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // Any "Battlegarb" set bonus that applies to Druids is the Feral set
            if (!String.IsNullOrEmpty(buff.SetName) && buff.AllowedClasses.Contains(CharacterClass.Druid))
            {
                if (buff.SetName == "Malorne Harness"                   // Tier 4
                 || buff.SetName == "Nordrassil Harness"                // Tier 5
                 || buff.SetName == "Thunderheart Harness"              // Tier 6
                 || buff.SetName.Contains("Dreamwalker Battlegear")     // Tier 7
                 || buff.SetName.Contains("Nightsong Battlegear")       // Tier 8
                 || buff.SetName.Contains("Runetotem's Battlegear")     // Tier 9 Horde
                 || buff.SetName.Contains("Malfurion's Battlegear")     // Tier 9 Alliance
                 || buff.SetName.Contains("Lasherweave Battlegear")     // Tier 10
                 || buff.SetName == "Stormrider's Battlegarb"           // Tier 11
                 || buff.SetName == "Obsidian Arborweave Battlegarb"    // Tier 12
                 || buff.SetName == "Deep Earth Battlegarb"             // Tier 13
                 || buff.SetName == "Armor of the Eternal Blossom"      // Tier 14
                    )
                    return true;
            }

            // Any "Gladiator's Sanctuary" set bonus that applies to Druids is the Feral set
            if (!String.IsNullOrEmpty(buff.SetName) && buff.AllowedClasses.Contains(CharacterClass.Druid) && buff.SetName.EndsWith("Sanctuary"))
                return true;

            // for buffs that are non-exclusive, allow anything that could be useful even slightly
            if (buff.Group == "Elixirs and Flasks" || buff.Group == "Potion" || buff.Group == "Food" || buff.Group == "Scrolls")
                return base.IsBuffRelevant(buff, character);
            else
            {
                if (character != null && Rawr.Properties.GeneralSettings.Default.HideProfEnchants && !character.HasProfession(buff.Professions))
                    return false;
                // Class Restrictions Enforcement
                else if (character != null && !buff.AllowedClasses.Contains(character.Class))
                    return false;

                return HasPrimaryStats(buff.Stats) || HasSecondaryStats(buff.Stats) || HasExtraStats(buff.Stats);
            }
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsBear calcOpts)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            foreach (Buff b in removedBuffs)
            {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs)
            {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        public override void SetDefaults(Character character)
        {
            #region Buffs
            // Stats
            character.ActiveBuffsAdd(("Mark of the Wild"));
            // Stamina
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            // Attack Power
            character.ActiveBuffsAdd(("Battle Shout"));
            // Attack Speed
            character.ActiveBuffsAdd(("Swiftblade's Cunning"));
            // Critical Strike
            character.ActiveBuffsAdd(("Leader of the Pack"));
            // Mastery
            character.ActiveBuffsAdd(("Blessing of Might"));
            // Temporary Buffs:
            // Bloodlust
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));
            #endregion

            #region Debuffs
            // Weakened Armor
            character.ActiveBuffsAdd(("Faerie Fire"));
            // Physical Vulnerability
            character.ActiveBuffsAdd(("Judgments of the Bold"));
            #endregion

            #region Consumables
            // Flask
            character.ActiveBuffsAdd(("Flask of Spring Blossoms"));
            // Potion
            character.ActiveBuffsAdd(("Virmen's Bite"));
            // Food
            character.ActiveBuffsAdd(("Agility Food"));
            #endregion
        }
        #endregion

        #region Stats
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                #region Additive Stats
                #region Core Stats
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Strength = stats.Strength,
                #endregion

                #region Health Related Stats
                Health = stats.Health,
                HealthRestore = stats.HealthRestore,
                #endregion

                #region Offensive Stats
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                Expertise = stats.Expertise,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHit = stats.PhysicalHit,
                WeaponDamage = stats.WeaponDamage,
                #endregion

                #region Defensive Stats
                Dodge = stats.Dodge,
                DodgeRating = stats.DodgeRating,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                #endregion

                #region Item Proc Stats
                MoteOfAnger = stats.MoteOfAnger,
                Paragon = stats.Paragon,
                #endregion

                #region Other Stats
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                PhysicalDamage = stats.PhysicalDamage,
                ShadowDamage = stats.ShadowDamage,
                #endregion
                #endregion

                #region Multiplicative Stats
                #region Core Stats
                // Primary
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                // Secondary
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                PhysicalHaste = stats.PhysicalHaste,
                #endregion

                #region Health Related Stats
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                DamageAbsorbed = stats.DamageAbsorbed,
//                DamageAbsorbedFromDamageTaken = stats.DamageAbsorbedFromDamageTaken,
                #endregion

                #region Offensive Stats
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                #endregion

                #region Defensive Stats
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                #endregion

                #region Other Stats
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                #endregion

                #region Used by Druids
                BonusDamageMultiplierRakeTick = stats.BonusDamageMultiplierRakeTick,
                #endregion
                #endregion

                #region Inverse Multiplicative Stats
                ArmorPenetration = stats.ArmorPenetration,
                TargetArmorReduction = stats.TargetArmorReduction,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                #endregion

                #region Non-Stacking Stats
                #region Resistances
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                #endregion

                #region Boss Handler
                DisarmDurReduc = stats.DisarmDurReduc,
                FearDurReduc = stats.FearDurReduc,
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                StunDurReduc = stats.StunDurReduc,
                #endregion

                #region Special Procs
                HighestSecondaryStat = stats.HighestSecondaryStat,
                HighestStat = stats.HighestStat,
                #endregion
                #endregion
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                    s.AddSpecialEffect(effect);
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool relevant = (stats.Agility + stats.ArmorPenetration + stats.TargetArmorReduction + stats.AttackPower + stats.PhysicalCrit +
                stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritDamageMultiplier +
                stats.BonusDamageMultiplier + stats.BonusWhiteDamageMultiplier +
                stats.BonusHealthMultiplier + stats.BonusHealingReceived + stats.DamageAbsorbed + stats.BattlemasterHealthProc + 
                stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
                stats.HasteRating + stats.Mastery + stats.MasteryRating + stats.Health + stats.HitRating +
                stats.Strength + stats.WeaponDamage +
                stats.PhysicalHit + stats.MoteOfAnger +
                stats.PhysicalHaste +
                stats.ThreatReductionMultiplier + stats.ArcaneDamage + stats.ShadowDamage + stats.FireDamage + stats.FrostDamage + stats.HolyDamage + stats.NatureDamage + stats.PhysicalDamage +
                stats.Dodge + stats.DodgeRating + 
                stats.Armor + stats.BonusArmor + stats.BaseArmorMultiplier + stats.BonusArmorMultiplier + 
                stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance + stats.BonusBleedDamageMultiplier + stats.Paragon +
                stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff + stats.HighestStat + stats.HighestSecondaryStat +
                stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.BonusPhysicalDamageMultiplier + stats.BonusDamageMultiplierRakeTick +
                stats.SnareRootDurReduc + stats.FearDurReduc + stats.StunDurReduc + stats.MovementSpeed +
                stats.FrostResistanceBuff + stats.ShadowResistanceBuff +
                stats.ArcaneDamageReductionMultiplier + stats.FireDamageReductionMultiplier + stats.FrostDamageReductionMultiplier + stats.ShadowDamageReductionMultiplier + 
                stats.NatureDamageReductionMultiplier + stats.PhysicalDamageTakenReductionMultiplier) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || 
                    effect.Trigger == Trigger.MeleeAttack || 
                    effect.Trigger == Trigger.PhysicalAttack || 
                    effect.Trigger == Trigger.MeleeHit || 
                    effect.Trigger == Trigger.PhysicalHit || 
                    effect.Trigger == Trigger.MeleeCrit || 
                    effect.Trigger == Trigger.PhysicalCrit ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.PhysicalHitorDoTTick ||
                    effect.Trigger == Trigger.MeleeHitorDoTTick ||
                    effect.Trigger == Trigger.WhiteAttack ||
                    effect.Trigger == Trigger.WhiteCrit ||
                    effect.Trigger == Trigger.WhiteHit ||
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone ||
                    effect.Trigger == Trigger.MangleBearHit ||
                    effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || 
                    effect.Trigger == Trigger.DamageTakenPutsMeBelow35PercHealth ||
                    effect.Trigger == Trigger.DamageTakenPutsMeBelow50PercHealth)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        /// <summary>
        /// HasPrimaryStats() should return true if the Stats object has any stats that define the item
        /// as being 'for your class/spec'. For melee classes this is typical melee stats like Strength, 
        /// Agility, AP, Expertise... For casters it would be spellpower, intellect, ...
        /// As soon as an item/enchant/buff has any of the stats listed here, it will be assumed to be 
        /// relevant unless explicitely filtered out.
        /// Stats that could be usefull for both casters and melee such as HitRating, CritRating and Haste
        /// don't belong here, but are SecondaryStats. Specific melee versions of these do belong here 
        /// for melee, spell versions would fit here for casters.
        /// </summary>
        public bool HasPrimaryStats(Stats stats)
        {
            bool PrimaryStats =
                // -- State Properties --
                // Base Stats
                stats.Agility != 0 ||
                stats.AttackPower != 0 ||

                // Combat Values
                stats.Dodge != 0 ||
                stats.DodgeRating != 0 ||
                stats.Armor != 0 ||
                stats.BonusArmor != 0 ||
                stats.PhysicalCrit != 0 ||
                stats.PhysicalHit != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusAgilityMultiplier != 0 ||
                stats.BonusAttackPowerMultiplier != 0 ||
                stats.PhysicalHaste != 0 ||
                stats.BonusCritDamageMultiplier != 0 ||
                stats.BaseArmorMultiplier != 0 ||
                stats.BonusArmorMultiplier != 0 ||
                stats.PhysicalDamageTakenReductionMultiplier != 0;

            if (!PrimaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasPrimaryStats(effect.Stats))
                    {
                        PrimaryStats = true;
                        break;
                    }
                }
            }

            return PrimaryStats;
        }

        /// <summary>
        /// HasSecondaryStats() should return true if the Stats object has any stats that are relevant for the 
        /// model but only to a smaller degree, so small that you wouldn't typically consider the item.
        /// Stats that are usefull to both melee and casters (HitRating, CritRating & Haste) fit in here also.
        /// An item/enchant/buff having these stats would be considered only if it doesn't have any of the 
        /// unwanted stats.  Group/Party buffs are slighly different, they would be considered regardless if 
        /// they have unwanted stats.
        /// Note that a stat may be listed here since it impacts the model, but may also be listed as an unwanted stat.
        /// </summary>
        public bool HasSecondaryStats(Stats stats)
        {
            bool SecondaryStats =
                // -- State Properties --
                // Base Stats
                stats.Expertise != 0 ||
                stats.ExpertiseRating != 0 ||
                stats.HasteRating != 0 ||
                stats.HitRating != 0 ||
                stats.CritRating != 0 ||
                stats.Mastery != 0 ||
                stats.MasteryRating != 0 ||

                // Buffs / Debuffs
                stats.BattlemasterHealthProc != 0 ||

                // Combat Values
                stats.TargetArmorReduction != 0 ||

                // Equipment Effects
                stats.ArcaneDamage != 0 ||
                stats.FireDamage != 0 ||
                stats.FrostDamage != 0 ||
                stats.HolyDamage != 0 ||
                stats.NatureDamage != 0 ||
                stats.ShadowDamage != 0 ||
                stats.WeaponDamage != 0 ||

                stats.MoteOfAnger != 0 ||
                stats.Paragon != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusBleedDamageMultiplier != 0 ||
                stats.BonusDamageMultiplier != 0 ||
                stats.BonusNatureDamageMultiplier != 0 ||
                stats.BonusDamageMultiplierRakeTick != 0 ||
                stats.BonusWhiteDamageMultiplier != 0 ||
                stats.BonusPhysicalDamageMultiplier != 0 ||
                stats.DamageAbsorbed != 0 ||

                // -- InverseMultiplicativeStat
                stats.ArmorPenetration != 0 ||
                stats.TargetArmorReduction != 0 ||
                stats.ThreatReductionMultiplier != 0 ||

                // -- NoStackStats
                stats.DisarmDurReduc != 0 ||
                stats.FearDurReduc != 0 ||
                stats.MovementSpeed != 0 ||
                stats.SnareRootDurReduc != 0 ||
                stats.StunDurReduc != 0 ||

                stats.HighestStat != 0 ||
                stats.HighestSecondaryStat != 0;

            if (!SecondaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasSecondaryStats(effect.Stats))
                    {
                        SecondaryStats = true;
                        break;
                    }
                }
            }

            return SecondaryStats;
        }

        /// <summary>
        /// Return true if the Stats object has any stats that don't influence the model but that you do want 
        /// to display in tooltips and in calculated summary values.
        /// </summary>
        public bool HasExtraStats(Stats stats)
        {
            bool ExtraStats =
                stats.Health != 0 ||
                stats.HealthRestore != 0 ||
                stats.Stamina != 0 ||
                stats.Strength != 0 ||

                stats.BonusHealthMultiplier != 0 ||
                stats.HealingReceivedMultiplier != 0 ||
                stats.BonusStaminaMultiplier != 0 ||
                stats.BonusStrengthMultiplier != 0 ||
                stats.BaseArmorMultiplier != 0 ||
                stats.BonusArmorMultiplier != 0 ||

                stats.ArcaneResistance != 0 ||
                stats.FireResistance != 0 ||
                stats.FrostResistance != 0 ||
                stats.NatureResistance != 0 ||
                stats.ShadowResistance != 0 ||

                stats.ArcaneResistanceBuff != 0 ||
                stats.FireResistanceBuff != 0 ||
                stats.FrostResistanceBuff != 0 ||
                stats.NatureResistanceBuff != 0 ||
                stats.ShadowResistanceBuff != 0 ||
                
                stats.ArcaneDamageReductionMultiplier != 0 ||
                stats.FireDamageReductionMultiplier != 0 ||
                stats.FrostDamageReductionMultiplier != 0 ||
                stats.NatureDamageReductionMultiplier != 0 ||
                stats.ShadowDamageReductionMultiplier != 0;

            if (!ExtraStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasExtraStats(effect.Stats))
                    {
                        ExtraStats = true;
                        break;
                    }
                }
            }

            return ExtraStats;
        }

        /// <summary>
        /// Return true if the Stats object contains any stats that are making the item undesired.
        /// Any item having only Secondary stats would be removed if it also has one of these.
        /// </summary>
        public bool HasUnwantedStats(Stats stats)
        {
            /// List of stats that will filter out some buffs (Flasks, Elixirs & Scrolls), Enchants and Items.
            bool UnwantedStats =
                stats.Intellect > 0 ||
                stats.SpellPower > 0 ||
                stats.ParryRating > 0 ||
                stats.BlockRating > 0;

            if (!UnwantedStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (HasUnwantedStats(effect.Stats))    // An unwanted stat could be behind a trigger we don't model.
                    {
                        UnwantedStats = true;
                        break;
                    }
                }
            }

            return UnwantedStats;
        }
        #endregion
        #endregion

        #region Primary Calculation Methods
        /// <summary>
        /// Gets the results of the Character provided
        /// </summary>
        /// <param name="character">The Character to calculate results for</param>
        /// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
        /// <returns>The CharacterCalculationsBear containing the results of the calculations</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Setup uniform variables from all models
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsBear calc = new CharacterCalculationsBear();
            if (character == null) { return calc; }
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
            if (calcOpts == null) { return calc; }

            BossOptions bossOpts = character.BossOptions;

            // Make sure there is at least one attack in the list.
            // If there's not, add a Default Melee Attack for processing
            if (bossOpts.Attacks.Count < 1) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Make sure there is a default melee attack
            // If the above processed, there will be one so this won't have to process
            // If the above didn't process and there isn't one, add one now
            if (bossOpts.DefaultMeleeAttack == null) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Since the above forced there to be an attack it's safe to do this without a null check
            Attack bossAttack = bossOpts.DefaultMeleeAttack;

            StatsBear stats = GetCharacterStats(character, additionalItem) as StatsBear;
            calc.BasicStats = stats;
            calc.TargetLevel = (calcOpts.UseBossHandler ? character.BossOptions.Level : calcOpts.TargetLevel);
            calc.CharacterLevel = (calcOpts.UseBossHandler ? character.Level : calcOpts.CharacterLevel);
            #endregion

            #region Hit/Dodge/Parry Caps
            int levelDelta = calc.TargetLevel - calc.CharacterLevel;
            float physicalHitCap, dodgeCap, parryCap = 0;

            if (levelDelta < 0)
            {
                physicalHitCap = dodgeCap = (float)Math.Max(0, (0.015f * levelDelta) + 0.03f);
                parryCap = (float)Math.Max(0, (0.015 * levelDelta) + 0.03f);
            }
            else if (levelDelta > 3)
            {
                physicalHitCap = dodgeCap = (0.03f + (0.015f * levelDelta));
                parryCap = (0.03f + (0.015f * levelDelta));
            }
            else
            {
                physicalHitCap = StatConversion.WHITE_MISS_CHANCE_CAP[calc.TargetLevel - calc.CharacterLevel];
                dodgeCap = StatConversion.WHITE_DODGE_CHANCE_CAP[calc.TargetLevel - calc.CharacterLevel];
                parryCap = StatConversion.WHITE_PARRY_CHANCE_CAP[calc.TargetLevel - calc.CharacterLevel];
            }
            #endregion

            calc.AttackPower = (float)Math.Floor((1 + stats.BonusAttackPowerMultiplier) * (stats.AttackPower + (stats.Agility - 10f) * 2f) + (stats.Strength - 10f));
            calc.SpellPower = (float)Math.Floor((stats.Intellect - 10f) * (1 + stats.BonusSpellPowerMultiplier));
            calc.HealingPower = (float)Math.Floor((stats.Agility + (stats.Intellect - 10f)) * (1 + stats.BonusSpellPowerMultiplier));
            calc.PhysicalCrit = StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Druid, calc.CharacterLevel) + StatConversion.GetPhysicalCritFromRating(stats.CritRating, calc.CharacterLevel) + stats.PhysicalCrit;
            calc.PhysicalHit = StatConversion.GetPhysicalHitFromRating(stats.HitRating, calc.CharacterLevel) + stats.PhysicalHit;
            calc.PhysicalHitCap = physicalHitCap;
            float expertise = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, calc.CharacterLevel) + stats.Expertise, CharacterClass.Druid);
            calc.PhysicalDodge = (float)Math.Min(expertise, dodgeCap);
            calc.PhysicalParry = (expertise > dodgeCap ? (expertise > (dodgeCap + parryCap) ? parryCap : expertise - dodgeCap) : 0f);
            calc.DodgeCap = dodgeCap;
            calc.ParryCap = parryCap;
            calc.PhysicalHaste = (1 + StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, calc.CharacterLevel)) * (1 + stats.PhysicalHaste) - 1;
            calc.Mastery = (stats.Mastery + StatConversion.GetMasteryFromRating(stats.MasteryRating, calc.CharacterLevel)) * calc.MasteryPerRating;
            calc.MovementSpeed = stats.GuardianMovementSpeed;

            new GuardianSolver().Solve(character, bossOpts, calcOpts, ref calc, bossAttack);

            calc.OverallPoints = calc.MitigationPoints + calc.SurvivabilityPoints + calc.RecoveryPoints + calc.ThreatPoints;

            return calc;
        }

        /// <summary>
        /// Gets the total Stats of the Character
        /// </summary>
        /// <param name="character">The Character to get the total Stats of</param>
        /// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
        /// <returns>The total stats for the Character</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();

            int characterLevel = (calcOpts.UseBossHandler ? character.Level : calcOpts.CharacterLevel);
            int targetLevel = (calcOpts.UseBossHandler ? character.BossOptions.Level : calcOpts.TargetLevel);

            StatsBear statsTotal = new StatsBear();

            Stats statsBase = BaseStats.GetBaseStats(characterLevel, character.Class, character.Race, BaseStats.DruidForm.Bear);
            statsTotal.Accumulate(statsBase);

            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            statsTotal.GuardianMovementSpeed = 0.15f + (statsTotal.MovementSpeed);
            statsTotal.BonusCritRatingMultiplier = 0.5f;
            statsTotal.BonusHasteRatingMultiplier = 0.5f;

            #region Set Bonuses
            #region PvP
            int PvPCount;
            character.SetBonusCount.TryGetValue("Gladiator's Sanctuary", out PvPCount);
            if (PvPCount >= 2)
            {
                statsTotal.PvPPower += 500f;
                // the 15% movement speed is only outdoors which most dungeons are not
                statsTotal.GuardianMovementSpeed += 0.15f;
            }
            if (PvPCount >= 4)
            {
                statsTotal.PvPResilience += 1000f;
            }
            #endregion

            #region Tier 11
            statsTotal.Tier_11_2_piece = false;
            statsTotal.Tier_11_4_piece = false;
            int T11Count;
            character.SetBonusCount.TryGetValue("Stormrider's Battlegarb", out T11Count);
            if (T11Count >= 2) {
                statsTotal.Tier_11_2_piece = true;
            }
            if (T11Count >= 4)
            {
                statsTotal.Tier_11_4_piece = true;
            }
            #endregion

            #region Tier 12
            statsTotal.Tier_12_2_piece = false;
            statsTotal.Tier_12_4_piece = false;
            int T12Count;
            character.SetBonusCount.TryGetValue("Obsidian Arborweave Battlegarb", out T12Count);
            if (T12Count >= 2)
            {
                statsTotal.Tier_12_2_piece = true;
            }
            if (T12Count >= 4)
            {
                statsTotal.Tier_12_4_piece = true;
            }
            #endregion

            #region Tier 13
            statsTotal.Tier_13_2_piece = false;
            statsTotal.Tier_13_4_piece = false; 
            int T13Count;
            character.SetBonusCount.TryGetValue("Deep Earth Battlegarb", out T13Count);
            if (T13Count >= 2)
            {
                statsTotal.Tier_13_2_piece = true;
            }
            if (T13Count >= 4)
            {
                statsTotal.Tier_13_4_piece = true;
            }
            #endregion

            #region Tier 14
            statsTotal.Tier_14_2_piece = false;
            statsTotal.Tier_14_4_piece = false;
            int T14Count;
            character.SetBonusCount.TryGetValue("Armor of the Eternal Blossom", out T14Count);
            if (T14Count >= 2)
            {
                statsTotal.Tier_14_2_piece = true;
            }
            if (T14Count >= 4)
            {
                statsTotal.Tier_14_4_piece = true;
            }
            #endregion
            #endregion

            // Talented bonus multipliers
            StatsBear statsTalents = new StatsBear()
            {
                BonusAgilityMultiplier = (character.DruidTalents.HeartOfTheWild ? 0.06f : 0),
                BaseArmorMultiplier = 3.30f, // In the end end up with 3.3 when calculated
                BonusIntellectMultiplier = (character.DruidTalents.HeartOfTheWild ? 0.06f : 0),
                BonusAttackPowerMultiplier = 0.25f,
                BonusStaminaMultiplier = ((1 + (Character.ValidateArmorSpecialization(character, ItemType.Leather) ? 0.05f : 0f))
                                            * (1 + (character.DruidTalents.HeartOfTheWild ? 0.06f : 0))
                                            - 1f),
                GuardianMovementSpeed = (character.DruidTalents.FelineSwiftness ? 0.15f : 0),
                CritChanceReduction = 0.06f,
                // Magic Damage reduction
                ArcaneDamageReductionMultiplier = 0.25f,
                NatureDamageReductionMultiplier = 0.25f,
                FrostDamageReductionMultiplier = 0.25f,
                ShadowDamageReductionMultiplier = 0.25f,
                FireDamageReductionMultiplier = 0.25f,
                PhysicalDamageTakenReductionMultiplier = 0.12f,
            };

            statsTotal.Accumulate(statsTalents);
            //statsTotal.GuardianMovementSpeed += statsTalents.GuardianMovementSpeed;

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));

            // Secondary Stats: Critical Strike, Haste, Mastery, Hit, Expertise
            statsTotal.CritRating = (float)Math.Floor(statsTotal.CritRating * (1 + statsTotal.BonusCritRatingMultiplier));
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating * (1 + statsTotal.BonusHasteRatingMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina, characterLevel));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            // Armor
            statsTotal.Armor *= (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));

            return statsTotal;
        }
        #endregion
    }
}
