using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rawr.Mistweaver {
    [Rawr.Calculations.RawrModelInfo("Mistweaver", "Ability_Druid_TreeofLife", CharacterClass.Monk)]
    public class CalculationsMistweaver : CalculationsBase
    {
        #region Variables and Properties
        #region Gemming Templates
        private string[] tierNames = { "Uncommon", "Rare", "Epic", "Jewelcrafter" };

        // Red
        private int[] brilliant = { 52173, 52207, 71881, 52257 };

        // Orange
        private int[] reckless = { 52144, 52208, 71850, 52208 };
        private int[] artful = { 52140, 52205, 71854, 52205 };
        private int[] potent = { 52147, 52239, 71842, 52239 };

        // Purple
        private int[] purified = { 52100, 52236, 71868, 52236 };

        // Meta
        private int[] metas = { 68780, 52296, 41333 };

        //Cogwheel
        private int cog_fractured = 59480;  //Mastery
        private int cog_sparkling = 59496;  //Spirit
        private int cog_quick = 59479;  //Haste
        private int cog_smooth = 59478;  //Crit

        /// <summary>
        /// List of gemming templates available to Rawr.
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                List<GemmingTemplate> retval = new List<GemmingTemplate>();
                
                for (int tier = 0; tier < 4; ++tier)
                {
                    foreach (int meta in metas)
                        retval.AddRange(MistweaverGemmingTemplateBlock(tier, meta));
                }

                foreach (int meta in metas)
                    retval.AddRange(MistweaverCogwheelTemplateBlock(meta));

                return retval;
            }
        }

        private List<GemmingTemplate> MistweaverGemmingTemplateBlock(int tier, int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[] {
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, brilliant, brilliant, brilliant, meta), // Straight Intellect
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, reckless, brilliant, brilliant, meta), // Int/Haste/Int
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, potent, brilliant, brilliant, meta), // Int/Crit/Int
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, artful, brilliant, brilliant, meta), // Int/Mastery/Int
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, reckless, purified, brilliant, meta), // Int/Haste/Spirit
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, potent, purified, brilliant, meta), // Int/Crit/Spirit
                CreateMistweaverGemmingTemplate(tier, tierNames, brilliant, artful, purified, brilliant, meta), // Int/Mastery/Spirit
            });
            return retval;
        }

        const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateMistweaverGemmingTemplate(int tier, string[] tierNames, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate
            {
                Model = "Mistweaver",
                Group = tierNames[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta
            };
        }

        private List<GemmingTemplate> MistweaverCogwheelTemplateBlock(int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[] {
                    // Engineering cogwheel templates (meta and 2 cogs each, no repeats)
                    CreateMistweaverCogwheelTemplate(meta, cog_fractured, cog_quick),
                    CreateMistweaverCogwheelTemplate(meta, cog_fractured, cog_smooth),
                    CreateMistweaverCogwheelTemplate(meta, cog_fractured, cog_sparkling),
                    CreateMistweaverCogwheelTemplate(meta, cog_quick, cog_smooth),
                    CreateMistweaverCogwheelTemplate(meta, cog_quick, cog_sparkling),
                    CreateMistweaverCogwheelTemplate(meta, cog_smooth, cog_sparkling),
                });
            return retval;
        }

        private GemmingTemplate CreateMistweaverCogwheelTemplate(int meta, int cogwheel1, int cogwheel2)
        {
            return new GemmingTemplate
            {
                Model = "Mistweaver",
                Group = "Engineer",
                Enabled = false,
                MetaId = meta,
                CogwheelId = cogwheel1,
                Cogwheel2Id = cogwheel2
            };
        }
        #endregion

        /// <summary>Labels of the stats available to the Optimizer</summary>
        public override string[] OptimizableCalculationLabels
        {
            get { return _optimizableCalculationLabels; }
        }
        
        private static string[] _optimizableCalculationLabels = new string[] {
        };

        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("MPS", Colors.Red);
                }
                return _subPointNameColors;
            }
        }
        private Dictionary<string, Color> _subPointNameColors = null;

        private string[] characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (characterDisplayCalculationLabels != null)
                    return characterDisplayCalculationLabels;

                string[] basic = new string[] {
                    };

                List<string> list = new List<string>();
                list.AddRange(basic);

                characterDisplayCalculationLabels = list.ToArray();
                return characterDisplayCalculationLabels;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { if (calculationOptionsPanel == null) { calculationOptionsPanel = new CalculationOptionsPanelMistweaver(); } return calculationOptionsPanel; } }
        private ICalculationOptionsPanel calculationOptionsPanel = null;
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMistweaver(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMistweaver(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMistweaver));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsMistweaver calcOpts = serializer.Deserialize(reader) as CalculationOptionsMistweaver;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        /// <summary>
        /// List of itemtypes that are relevant for Mistweaver
        /// </summary>
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
                {
                            ItemType.None,
                            ItemType.Leather,
                            ItemType.Polearm,
                            ItemType.Staff,
                            ItemType.OneHandAxe,
                            ItemType.FistWeapon,
                            ItemType.OneHandMace,
                            ItemType.OneHandSword,
                }));
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                // -- State Properties --
                // Base Stats
                Health = stats.Health,
                Mana = stats.Mana,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Armor = stats.Armor,
                HasteRating = stats.HasteRating,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                // SpellPenetration = stats.SpellPenetration,
                Mp5 = stats.Mp5,
                BonusArmor = stats.BonusArmor,

                // Buffs / Debuffs
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,

                // Combat Values
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                TargetArmorReduction = stats.TargetArmorReduction,

                // Spell Combat Ratings
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,

                // Equipment Effects
                ManaRestore = stats.ManaRestore,
                SpellsManaCostReduction = stats.SpellsManaCostReduction,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,
                ManaCostReductionMultiplier = stats.ManaCostReductionMultiplier,
                Healed = stats.Healed,
                HealedPerSP = stats.HealedPerSP,

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                SpellHaste = stats.SpellHaste,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusHealingDoneMultiplier = stats.BonusHealingDoneMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,

                // -- NoStackStats
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                BonusManaPotionEffectMultiplier = stats.BonusManaPotionEffectMultiplier,
                HighestStat = stats.HighestStat,
            };

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return true;
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Devotion Aura");
            character.ActiveBuffsAdd("Arcane Brilliance (Mana)");
            character.ActiveBuffsAdd("Arcane Brilliance (SP%)");
            character.ActiveBuffsAdd("Blessing of Might (Mp5)");
            character.ActiveBuffsAdd("Leader of the Pack");
            character.ActiveBuffsAdd("Revitalize");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Mark of the Wild");
            character.ActiveBuffsAdd("Heroism/Bloodlust");
            character.ActiveBuffsAdd("Moonkin Form");
            character.ActiveBuffsAdd("Flask of the Draconic Mind");
            character.ActiveBuffsAdd("Intellect Food");
            character.ActiveBuffsAdd("Mythical Mana Potion");
        }
        #endregion

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMistweaver opts = character.CalculationOptions as CalculationOptionsMistweaver;
            if (opts == null)
                opts = new CalculationOptionsMistweaver();

            Stats stats = new Stats();
            Stats statsBase = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);
            stats.Accumulate(statsBase);
            stats.BaseAgility = statsBase.Agility;

            // Get the gear/enchants/buffs stats loaded in
            stats.Accumulate(GetItemStats(character, additionalItem));
            //stats.Accumulate(GetBuffsStats(character, opts));

            FinalizeStats(stats, stats);

            // Derived stats: Health, mana pool, armor
            stats.Health = (float)Math.Round(stats.Health + StatConversion.GetHealthFromStamina(stats.Stamina));
            stats.Health = (float)Math.Floor(stats.Health * (1f + stats.BonusHealthMultiplier));
            stats.Mana = (float)Math.Round(stats.Mana + StatConversion.GetManaFromIntellect(stats.Intellect));
            stats.Mana = (float)Math.Floor(stats.Mana * (1f + stats.BonusManaMultiplier));

            // Armor
            stats.Armor = stats.Armor * (1f + stats.BaseArmorMultiplier);
            stats.BonusArmor = stats.BonusArmor * (1f + stats.BonusArmorMultiplier);
            stats.Armor += stats.BonusArmor;
            stats.Armor = (float)Math.Round(stats.Armor);

            return stats;
        }

        public static void FinalizeStats(Stats stats, Stats statsMultipliers)
        {
            stats.Intellect += stats.HighestStat;
            stats.Intellect *= (1 + statsMultipliers.BonusIntellectMultiplier);
            stats.Agility *= (1 + statsMultipliers.BonusAgilityMultiplier);
            stats.Stamina *= (1 + statsMultipliers.BonusStaminaMultiplier);
            stats.Spirit *= (1 + statsMultipliers.BonusSpiritMultiplier);
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsMistweaver calc = new CharacterCalculationsMistweaver();
            if (character == null)
                return calc;
            return calc;
        }

        #region Custom Charts

        private string[] customChartNames = new string[] { 
        };

        public override string[] CustomChartNames
        {
            get
            {
                return customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return null;
        }

        #endregion
    }
}
