using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rawr.Moonkin
{
    [Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", CharacterClass.Druid)]
    public class CalculationsMoonkin : CalculationsBase
    {
        #region Variables and Properties
        #region Gemming Templates
        private string[] tierNames = { "Uncommon", "Rare", "Legendary Meta", "Jewelcrafter" };

        // Red
        private int[] brilliant = { 76628, 76694, 76694, 83150 };

        // Orange
        private int[] reckless = { 76602, 76668, 76668, 88943 };
        private int[] artful = { 76606, 76672, 76672, 88931 };
        private int[] potent = { 76594, 76660, 76660, 88942 };

        // Yellow
        private int[] smooth = { 76631, 76697, 76697, 83146 };
        private int[] fractured = { 76634, 76700, 76700, 83143 };
        private int[] quick = { 76633, 76699, 76699, 83142 };

        // Purple
        private int[] purified = { 76620, 76686, 76686, 88958 };
        private int[] veiled = { 76616, 76682, 76682, 88963 };

        // Meta
        private int burning = 76885;
        // Legendary meta
        private int sinister = 95347;

        //Cogwheel
        private int cog_fractured = 77547;  //Mastery
        private int cog_sparkling = 77546;  //Spirit
        private int cog_quick = 77542;  //Haste
        private int cog_rigid = 77545;  //Hit
        private int cog_smooth = 77541;  //Crit

        // Hydraulic (Legendary)
        private int hydraulic_int = 89882;

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
                    retval.AddRange(MoonkinGemmingTemplateBlock(tier, tier == 2 ? sinister : burning));
                }
                retval.AddRange(new GemmingTemplate[] {
                // Engineering cogwheel templates (meta and 2 cogs each, no repeats)
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_quick),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_rigid),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_rigid),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_rigid, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_rigid, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_smooth, cog_sparkling),
                });
                return retval;
            }
        }

        private List<GemmingTemplate> MoonkinGemmingTemplateBlock(int tier, int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[]
                {
                // Orange
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, brilliant, brilliant, brilliant, meta), // Straight Intellect
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, brilliant, brilliant, meta), // Int/Haste/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, brilliant, brilliant, meta), // Int/Crit/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, brilliant, brilliant, meta), // Int/Mastery/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, veiled, brilliant, meta), // Int/Haste/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, purified, brilliant, meta), // Int/Haste/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, veiled, brilliant, meta), // Int/Crit/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, purified, brilliant, meta), // Int/Crit/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta), // Int/Mastery/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, purified, brilliant, meta), // Int/Mastery/Spirit
                // Yellow
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, quick, brilliant, brilliant, meta), // Int/Haste/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, smooth, brilliant, brilliant, meta), // Int/Crit/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, fractured, brilliant, brilliant, meta), // Int/Mastery/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, quick, veiled, brilliant, meta), // Int/Haste/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, quick, purified, brilliant, meta), // Int/Haste/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, fractured, veiled, brilliant, meta), // Int/Crit/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, fractured, purified, brilliant, meta), // Int/Crit/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta), // Int/Mastery/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, purified, brilliant, meta), // Int/Mastery/Spirit
                });
            return retval;
        }

        const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateMoonkinGemmingTemplate(int tier, string[] tierNames, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate
            {
                Model = "Moonkin",
                Group = tierNames[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta,
                HydraulicId = hydraulic_int
            };
        }

        private GemmingTemplate CreateMoonkinCogwheelTemplate(int meta, int cogwheel1, int cogwheel2)
        {
            return new GemmingTemplate
            {
                Model = "Moonkin",
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
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Hit Rating",
                    "Haste Rating",
                    "Crit Rating",
                    "Mastery Rating"
                    };
                return _optimizableCalculationLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;

        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Burst DPS", Colors.Red);
                    _subPointNameColors.Add("Sustained DPS", Colors.Blue);
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
                if (characterDisplayCalculationLabels == null)
                {
                    characterDisplayCalculationLabels = new string[] {
                    "Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Agility",
                    "Basic Stats:Stamina",
                    "Basic Stats:Intellect",
                    "Basic Stats:Spirit",
                    "Basic Stats:Armor",
                    "Spell Stats:Spell Power",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Spell Stats:Mastery",
                    "Regen:Mana Regen",
                    "Solution:Total Score",
                    "Solution:Selected Rotation",
                    "Solution:Selected DPS",
                    "Solution:Selected Time To OOM",
                    "Solution:Selected Cycle Length",
                    "Solution:Selected Spell Breakdown",
                    "Solution:Burst Rotation",
                    "Solution:Burst DPS",
                    "Solution:Burst Time To OOM",
                    "Solution:Burst Cycle Length",
                    "Solution:Burst Spell Breakdown",
                    "Solution:Nature's Grace Uptime",
                    "Solution:Solar Eclipse Uptime",
                    "Solution:Lunar Eclipse Uptime",
                    "Spell Info:Starfire",
                    "Spell Info:Wrath",
                    "Spell Info:Starsurge",
                    "Spell Info:Moonfire",
                    "Spell Info:Starfall",
                    "Spell Info:Treants",
                    "Spell Info:Wild Mushroom"
                    };
                }
                return characterDisplayCalculationLabels;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { if (calculationOptionsPanel == null) { calculationOptionsPanel = new CalculationOptionsPanelMoonkin(); } return calculationOptionsPanel; } }
        private ICalculationOptionsPanel calculationOptionsPanel = null;
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMoonkin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMoonkin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsMoonkin calcOpts = serializer.Deserialize(reader) as CalculationOptionsMoonkin;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // No ranged enchants allowed
            if (enchant.Slot == ItemSlot.Ranged) return false;
            // Make an exception for enchant 4091 - Enchant Off-Hand - Superior Intellect
            if (slot == ItemSlot.OffHand) return (enchant.Id == 4091 || enchant.Id == 4434);
            // Otherwise, return the base value
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        public override List<Reforging> GetReforgingOptions(Item baseItem, int randomSuffixId, int upgradeItemLevel)
        {
            List<Reforging> retval = base.GetReforgingOptions(baseItem, randomSuffixId, upgradeItemLevel);

            // If the item has spirit, do not allow reforging spirit -> hit
            if (baseItem.Stats.Spirit > 0)
            {
                // I put this in a sub-if because putting it above messes with the if-else/if-else code flow
                if (!_enableSpiritToHit)
                    retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.Spirit && rf.ReforgeTo == AdditiveStat.HitRating);
            }
            // If it has hit, do not allow reforging hit -> spirit
            else if (baseItem.Stats.HitRating > 0)
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.HitRating && rf.ReforgeTo == AdditiveStat.Spirit);
            }
            // If it has neither, pick one based on the current calculation options.
            else
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeTo == (_reforgePriority == 0 ? AdditiveStat.HitRating : AdditiveStat.Spirit));
            }

            return retval;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of the Moonbeast");
            }
            return _relevantGlyphs;
        }

        /// <summary>
        /// List of itemtypes that are relevant for moonkin
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
                            ItemType.Dagger,
                            ItemType.Staff,
                            ItemType.FistWeapon,
                            ItemType.OneHandMace,
                            ItemType.TwoHandMace,
                            ItemType.Idol,
                            ItemType.Relic,
                }));
            }
        }

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
                            Trigger.DamageSpellCast,
                            Trigger.DamageSpellCrit,        // Black magic enchant ?
                            Trigger.DamageSpellHit,
                            Trigger.SpellCast,
                            Trigger.SpellCrit,        
                            Trigger.SpellHit, 
                            Trigger.SpellMiss,
                            Trigger.DoTTick,
                            Trigger.DamageDone,
                            Trigger.DamageOrHealingDone,    // Darkmoon Card: Greatness
                            Trigger.InsectSwarmTick,
                            Trigger.MoonfireTick,
                            Trigger.MoonfireCast,
                            Trigger.EclipseProc,
                            Trigger.DamageSpellHitorDoTTick,
                            Trigger.DamageSpellOrDoTCrit
                        });
            }
            //set { _RelevantTriggers = value; }
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // Any "Regalia" set bonus that applies to Druids is the Balance set
            if (!String.IsNullOrEmpty(buff.SetName) && buff.AllowedClasses.Contains(CharacterClass.Druid) && buff.SetName.Contains("Regalia"))
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
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                ExpertiseRating = stats.ExpertiseRating,
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

                // Spell Combat Ratings
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
        
                // Equipment Effects
                ManaRestore = stats.ManaRestore,
                ShadowDamage = stats.ShadowDamage,
                NatureDamage = stats.NatureDamage,
                FireDamage = stats.FireDamage,
                HolyDamage = stats.HolyDamage,
                ArcaneDamage = stats.ArcaneDamage,
                HolySummonedDamage = stats.HolySummonedDamage,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,
                HighestSecondaryStat = stats.HighestSecondaryStat,

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                SpellHaste = stats.SpellHaste,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,    // Benefits trees

                // -- NoStackStats
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                BonusManaPotionEffectMultiplier = stats.BonusManaPotionEffectMultiplier,
                HighestStat = stats.HighestStat,
                DragonwrathProc = stats.DragonwrathProc,
                SecondaryStatMultiplier = stats.SecondaryStatMultiplier,
                MultistrikeProc = stats.MultistrikeProc
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
            return HasPrimaryStats(stats) || (HasSecondaryStats(stats) && !HasUnwantedStats(stats));
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

            float ignoreStats = stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.Block + stats.BlockRating + stats.SpellShadowDamageRating + stats.SpellFireDamageRating + stats.SpellFrostDamageRating + stats.Health + stats.Armor + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.BonusHealthMultiplier;

            bool PrimaryStats =
                // -- State Properties --
                // Base Stats
                stats.Intellect != 0 ||
                stats.Spirit != 0 ||
                stats.SpellPower != 0 ||
                // stats.SpellPenetration != 0 ||

                // Combat Values
                stats.SpellCrit != 0 ||
                stats.SpellCritOnTarget != 0 ||
                stats.SpellHit != 0 ||

                // Spell Combat Ratings
                stats.SpellArcaneDamageRating != 0 ||
                stats.SpellNatureDamageRating != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusIntellectMultiplier != 0 ||
                stats.BonusSpiritMultiplier != 0 ||
                stats.SpellHaste != 0 ||
                stats.BonusSpellCritDamageMultiplier != 0 ||
                stats.BonusSpellPowerMultiplier != 0 ||
                stats.BonusArcaneDamageMultiplier != 0 ||
                stats.BonusNatureDamageMultiplier != 0;

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
                stats.Mana != 0 ||
                stats.HasteRating != 0 ||
                stats.HitRating != 0 ||
                stats.CritRating != 0 ||
                stats.Mp5 != 0 ||
                stats.Mastery != 0 ||
                stats.MasteryRating != 0 ||

                // Buffs / Debuffs
                stats.ManaRestoreFromMaxManaPerSecond != 0 ||

                // Combat Values
                stats.SpellCombatManaRegeneration != 0 ||

                // Equipment Effects
                stats.ManaRestore != 0 ||
                stats.ShadowDamage != 0 ||
                stats.NatureDamage != 0 ||
                stats.FireDamage != 0 ||
                stats.HolySummonedDamage != 0 ||
                stats.NatureSpellsManaCostReduction != 0 ||
                stats.HighestSecondaryStat != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusManaMultiplier != 0 ||
                stats.BonusCritDamageMultiplier != 0 ||
                stats.BonusDamageMultiplier != 0 ||
                stats.BonusPhysicalDamageMultiplier != 0 ||

                // -- NoStackStats
                stats.MovementSpeed != 0 ||
                stats.SnareRootDurReduc != 0 ||
                stats.FearDurReduc != 0 ||
                stats.StunDurReduc != 0 ||
                stats.BonusManaPotionEffectMultiplier != 0 ||
                stats.DragonwrathProc != 0 ||
                stats.HighestStat != 0 ||
                stats.SecondaryStatMultiplier != 0 ||
                stats.MultistrikeProc != 0;

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
                stats.Agility != 0 ||
                stats.Stamina != 0 ||
                stats.Armor != 0 ||
                stats.BonusArmor != 0 ||
                stats.BonusHealthMultiplier != 0 ||
                stats.BonusAgilityMultiplier != 0 ||
                stats.BonusStaminaMultiplier != 0  ||
                stats.BaseArmorMultiplier != 0 ||
                stats.BonusArmorMultiplier != 0;

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
                stats.Strength > 0 ||
                stats.Agility > 0 ||
                stats.AttackPower > 0 ||
                stats.DodgeRating > 0 ||
                stats.ParryRating > 0 ||
                stats.BlockRating > 0 ||
                stats.Resilience > 0;

            if (!UnwantedStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (/*RelevantTriggers.Contains(effect.Trigger) && */HasUnwantedStats(effect.Stats))    // An unwanted stat could be behind a trigger we don't model.
                    {
                        UnwantedStats = true;
                        break;
                    }
                }
            }

            return UnwantedStats;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsMoonkin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }
            // Removes the Hunter's Mark Buff and it's Children 'Glyphed', 'Improved' and 'Both' if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
                hasRelevantBuff =  character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        public override void SetDefaults(Character character)
        {
            // Passive buffs:
            // Stats
            character.ActiveBuffsAdd(("Mark of the Wild"));
            // Stam
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            // SP%
            character.ActiveBuffsAdd(("Arcane Brilliance (Spell Power)"));
            // Spell Haste
            character.ActiveBuffsAdd(("Moonkin Form"));
            // Crit
            character.ActiveBuffsAdd(("Arcane Brilliance (Critical Strike)"));
            // Mastery
            character.ActiveBuffsAdd(("Blessing of Might"));
            // Temporary Buffs:
            // Bloodlust
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));
            // Debuffs:
            // Magic vulnerability
            character.ActiveBuffsAdd(("Curse of the Elements"));
            // Consumables:
            // Flask
            character.ActiveBuffsAdd(("Flask of the Warm Sun"));
            // Food
            character.ActiveBuffsAdd(("Intellect Food"));
        }

        #endregion

        #region Custom Charts

        private string[] customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (customChartNames == null) {
                    customChartNames = new string[] { 
                    "Damage per Cast Time",
                    "Mana Gains By Source",
                    "Rotation Comparison",
                    "PTR Buff/Nerf"
                    };
                }
                return customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            
            switch (chartName)
            {
                case "Mana Gains By Source":
                    CharacterCalculationsMoonkin calcsManaBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    RotationData manaGainsRot = calcsManaBase.SelectedRotation;
                    Character c2 = character.Clone();

                    List<ComparisonCalculationMoonkin> manaGainsList = new List<ComparisonCalculationMoonkin>();

                    // Replenishment
                    CalculationOptionsMoonkin calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    calcOpts.Notify = false;
                    float oldReplenishmentUptime = calcOpts.ReplenishmentUptime;
                    calcOpts.ReplenishmentUptime = 0.0f;
                    CharacterCalculationsMoonkin calcsManaReplenishment = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.ReplenishmentUptime = oldReplenishmentUptime;
                    foreach (RotationData rot in calcsManaReplenishment.Rotations)
                    {
                        if (rot.Name == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Replenishment",
                                OverallPoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                ImageSource = "spell_magic_managain"
                            });
                        }
                    }

                    // Innervate
                    bool innervate = calcOpts.Innervate;
                    calcOpts.Innervate = false;
                    CharacterCalculationsMoonkin calcsManaInnervate = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.Innervate = innervate;
                    foreach (RotationData rot in calcsManaInnervate.Rotations)
                    {
                        if (rot.Name == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Innervate",
                                OverallPoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                ImageSource = "spell_nature_lightning"
                            });
                        }
                    }
                    calcOpts.Notify = true;

                    // mp5
                    manaGainsList.Add(new ComparisonCalculationMoonkin()
                    {
                        Name = "MP5",
                        OverallPoints = character.BossOptions.BerserkTimer * 60.0f * calcsManaBase.ManaRegen,
                        BurstDamagePoints = character.BossOptions.BerserkTimer * 60.0f * calcsManaBase.ManaRegen
                    });
                    return manaGainsList.ToArray();

                case "Damage per Cast Time":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    ComparisonCalculationMoonkin sf = new ComparisonCalculationMoonkin() { Name = "Starfire", ImageSource = "spell_arcane_starfire" };
                    ComparisonCalculationMoonkin mf = new ComparisonCalculationMoonkin() { Name = "Moonfire", ImageSource = "spell_nature_starfall" };
                    ComparisonCalculationMoonkin wr = new ComparisonCalculationMoonkin() { Name = "Wrath", ImageSource = "spell_nature_abolishmagic" };
                    ComparisonCalculationMoonkin ss = new ComparisonCalculationMoonkin() { Name = "Starsurge", ImageSource = "spell_arcane_arcane03" };
                    ComparisonCalculationMoonkin ssInst = new ComparisonCalculationMoonkin() { Name = "Shooting Stars Proc", ImageSource = "ability_mage_arcanebarrage" };
                    ComparisonCalculationMoonkin wm = new ComparisonCalculationMoonkin() { Name = "Wild Mushroom", ImageSource = "druid_ability_wildmushroom_b" };
                    ComparisonCalculationMoonkin sfa = new ComparisonCalculationMoonkin() { Name = "Starfall", ImageSource = "ability_druid_starfall" };
                    ComparisonCalculationMoonkin fon = new ComparisonCalculationMoonkin() { Name = "Force of Nature", ImageSource = "ability_druid_forceofnature" };
                    sf.BurstDamagePoints = calcsBase.SelectedRotation.StarfireAvgHit / calcsBase.SelectedRotation.StarfireAvgCast;
                    sf.OverallPoints = sf.BurstDamagePoints;
                    mf.BurstDamagePoints = calcsBase.SelectedRotation.MoonfireAvgHit / calcsBase.SelectedRotation.MoonfireAvgCast;
                    mf.OverallPoints = mf.BurstDamagePoints;
                    wr.BurstDamagePoints = calcsBase.SelectedRotation.WrathAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    wr.OverallPoints = wr.BurstDamagePoints;
                    // Use the Wrath average cast here because the Starsurge average cast is actually the combined weighted average
                    // of Starsurge and Shooting Stars
                    ss.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    ss.OverallPoints = ss.BurstDamagePoints;
                    ssInst.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    ssInst.OverallPoints = ssInst.BurstDamagePoints;
                    wm.BurstDamagePoints = calcsBase.SelectedRotation.MushroomDamage / 3f;
                    wm.OverallPoints = wm.BurstDamagePoints;
                    sfa.BurstDamagePoints = calcsBase.SelectedRotation.StarfallDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    sfa.OverallPoints = sfa.BurstDamagePoints;
                    fon.BurstDamagePoints = calcsBase.SelectedRotation.TreantDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    fon.OverallPoints = fon.BurstDamagePoints;
                    return new ComparisonCalculationMoonkin[] { sf, mf, wr, ss, ssInst, sfa, fon, wm };
                case "Rotation Comparison":
                    List<ComparisonCalculationMoonkin> comparisons = new List<ComparisonCalculationMoonkin>();
                    calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    foreach (RotationData rot in calcsBase.Rotations)
                    {
                        comparisons.Add(new ComparisonCalculationMoonkin
                        {
                            Name = rot.Name,
                            BurstDamagePoints = rot.BurstDPS,
                            SustainedDamagePoints = rot.SustainedDPS,
                            OverallPoints = rot.BurstDPS + rot.SustainedDPS
                        });
                    }
                    return comparisons.ToArray();
                case "PTR Buff/Nerf":
                    calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    c2 = character.Clone();

                    calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    calcOpts.Notify = false;
                    calcOpts.PTRMode = !calcOpts.PTRMode;
                    CharacterCalculationsMoonkin calcsCompare = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;

                    calcOpts.PTRMode = !calcOpts.PTRMode;
                    calcOpts.Notify = true;

                    float burstDpsDelta = calcOpts.PTRMode ? calcsBase.SubPoints[0] - calcsCompare.SubPoints[0] : calcsCompare.SubPoints[0] - calcsBase.SubPoints[0];
                    float sustDpsDelta = calcOpts.PTRMode ? calcsBase.SubPoints[1] - calcsCompare.SubPoints[1] : calcsCompare.SubPoints[1] - calcsBase.SubPoints[1];

                    return new ComparisonCalculationMoonkin[] { new ComparisonCalculationMoonkin { Name = "PTR Mode", BurstDamagePoints = burstDpsDelta, SustainedDamagePoints = sustDpsDelta, OverallPoints = burstDpsDelta + sustDpsDelta } };
            }
            return new ComparisonCalculationBase[0];
        }

        #endregion

        #region Model Specific Variables and Functions
        private static int _reforgePriority = 0;
        private static bool _enableSpiritToHit = false;
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsMoonkin calc = new CharacterCalculationsMoonkin();
            if (character == null) { return calc; }
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            if (calcOpts == null) { return calc; }
            //
            _reforgePriority = calcOpts.ReforgePriority;
            _enableSpiritToHit = calcOpts.AllowReforgingSpiritToHit;
            StatsMoonkin stats = (StatsMoonkin)GetCharacterStats(character, additionalItem);

            calc.SpellPower = (float)Math.Floor((1 + stats.BonusSpellPowerMultiplier) * (stats.SpellPower + stats.Intellect - 10));
            calc.SpellCritPenalty = StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[character.BossOptions.Level - character.Level];
            calc.SpellCrit = StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit + stats.SpellCritOnTarget + calc.SpellCritPenalty;
            calc.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating) + stats.SpellHit;
            calc.SpellHitCap = StatConversion.GetSpellMiss(character.BossOptions.Level - character.Level, false);
            calc.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;
            calc.Mastery = stats.Mastery + StatConversion.GetMasteryFromRating(stats.MasteryRating);
            calc.ManaRegen = stats.Mp5 / 5f;

            // Spec-based modifiers
            foreach (SpecialEffect effect in stats.SpecialEffects(se => se.ModifiedBy != null))
            {
                switch (effect.ModifiedBy)
                {
                    case "Spec/Class":  // Legendary Meta/Cloak
                        if (effect.Stats.SpellHaste > 0)
                            effect.ChanceModifier = 2.142f;
                        else if (effect.Stats.NatureDamage == 1)
                            effect.ChanceModifier = 1.1f;
                        break;
                    case "Spec - Balance":  // UVLS - specific to Moonkin
                        effect.ChanceModifier = 0.65f;
                        break;
                    case "Spell Crit":  // Cha-Ye's
                        effect.ChanceModifier = 1 + calc.SpellCrit;
                        break;
                    default:
                        break;
                }
            }

            calc.BasicStats = stats;

            calc.PTRMode = calcOpts.PTRMode;

            // Run the solver against the generated cycle
            new MoonkinSolver().Solve(character, ref calc);

            return calc;
        }

        // Incarnation (Chosen of Elune form)
        private static SpecialEffect _se_incarnation = null;
        public static SpecialEffect _SE_INCARNATION
        {
            get
            {
                return _se_incarnation ?? (_se_incarnation = new SpecialEffect(Trigger.Use, new Stats() { }, 30f, 180f));
            }
        }

        // Nature's Vigil
        private static SpecialEffect _se_naturesVigil = null;
        public static SpecialEffect _SE_NATURESVIGIL
        {
            get
            {
                return _se_naturesVigil ?? (_se_naturesVigil = new SpecialEffect(Trigger.Use, new Stats { BonusDamageMultiplier = 0.12f }, 30f, 180f));
            }
        }

        // Nature's Vigil, shortened duration for handling case without Incarnation
        private static SpecialEffect _se_naturesVigil_noIncarnation = null;
        public static SpecialEffect _SE_NATURESVIGIL_NOINCARNATION
        {
            get
            {
                return _se_naturesVigil_noIncarnation ?? (_se_naturesVigil_noIncarnation = new SpecialEffect(Trigger.Use, new Stats { BonusDamageMultiplier = 0.12f }, 15f, 180f));
            }
        }

        // 4T15
        // 1000 crit and 1000 mastery while NG is up
        // Trigger on Eclipse proc with 15-second duration is equivalent to NG
        private static SpecialEffect _se_4T15 = null;
        public static SpecialEffect _SE_4T15
        {
            get { return _se_4T15 ?? (_se_4T15 = new SpecialEffect(Trigger.EclipseProc, new Stats { CritRating = 1000f, MasteryRating = 1000f }, 15f, 0f)); }
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;

            StatsMoonkin statsTotal = new StatsMoonkin();
            Stats statsBase = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Moonkin);
            statsTotal.Accumulate(statsBase);
            
            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            #region Set Bonuses
            int T13Count, T14Count, T15Count, T16Count;
            character.SetBonusCount.TryGetValue("Deep Earth Regalia", out T13Count);
            character.SetBonusCount.TryGetValue("Regalia of the Eternal Blossom", out T14Count);
            character.SetBonusCount.TryGetValue("Regalia of the Haunted Forest", out T15Count);
            character.SetBonusCount.TryGetValue("Regalia of the Shattered Vale", out T16Count);
            if (T13Count >= 2)
            {
                // 2 piece: Your nukes deal 3% more damage when Insect Swarm is active on the target.
                statsTotal.BonusNukeDamageModifier = 0.03f;
            }
            if (T13Count >= 4)
            {
                // 4 piece: Starsurge cooldown is reduced by 5 sec and damage increased by 10%.
                statsTotal.T13FourPieceActive = true;
                statsTotal.BonusStarsurgeDamageModifier = 0.1f;
            }
            if (T14Count >= 2)
            {
                // 2 piece: Starfall damage increased by 20%.
                statsTotal.BonusStarfallDamageModifier = 0.2f;
            }
            if (T14Count >= 4)
            {
                // 4 piece: Moonfire and Sunfire duration extended by 2 seconds.
                statsTotal.BonusMoonfireDuration = 2f;
            }
            if (T15Count >= 2)
            {
                // 2 piece: Starsurge has a 10% increased crit chance.
                statsTotal.BonusStarsurgeCritModifier = 0.1f;
            }
            if (T15Count >= 4)
            {
                // 4 piece: You gain 1000 crit rating and 1000 mastery rating when NG is active.
                statsTotal.AddSpecialEffect(_SE_4T15);
            }
            if (T16Count >= 2)
            {
                // 2 piece: Spells cast in their respective eclipse proc a bolt.
                statsTotal.T16TwoPieceActive = true;
            }
            if (T16Count >= 4)
            {
                // 4 piece: Shooting Stars proc chance increased by 8%.
                statsTotal.BonusShootingStarsChance = 0.08f;
            }
            #endregion

            // Talented bonus multipliers
            Stats statsTalents = new StatsMoonkin()
            {
                BonusAgilityMultiplier = 0.06f * character.DruidTalents.HeartOfTheWild,
                BonusStaminaMultiplier = 0.06f * character.DruidTalents.HeartOfTheWild,
                BonusIntellectMultiplier = (1 + (Character.ValidateArmorSpecialization(character, ItemType.Leather) ? 0.05f : 0f)) *
                (1 + (0.06f * character.DruidTalents.HeartOfTheWild)) - 1,
                BonusManaMultiplier = 4f
            };

            // Nature's Vigil is calculated in with Incarnation and CA.
            // The NV special effects added here are to calculate what is left over if Incarnation is not taken, and for 5.2 PTR.
            if (character.DruidTalents.Incarnation > 0)
                statsTotal.BonusEclipseDamageMultiplier = 0.25f;
            else if (character.DruidTalents.NaturesVigil > 0)
                statsTotal.AddSpecialEffect(_SE_NATURESVIGIL_NOINCARNATION);
            if (character.DruidTalents.NaturesVigil > 0)
                statsTotal.AddSpecialEffect(_SE_NATURESVIGIL);
            if (character.DruidTalents.DreamOfCenarius > 0)
                statsTotal.BonusMoonfireDamageMultiplier = 0.5f;
            statsTotal.BonusSpellDamageMultiplier = 0.1f;

            statsTotal.Accumulate(statsTalents);

            // Trinket with % stats/damage multiplier
            statsTotal.BonusCritDamageMultiplier = (1 + statsTotal.BonusCritDamageMultiplier) * (1 + statsTotal.SecondaryStatMultiplier) - 1;
            statsTotal.BonusSpiritMultiplier = (1 + statsTotal.BonusSpiritMultiplier) * (1 + statsTotal.SecondaryStatMultiplier) - 1;
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating * (1 + statsTotal.SecondaryStatMultiplier));
            statsTotal.MasteryRating = (float)Math.Floor(statsTotal.MasteryRating * (1 + statsTotal.SecondaryStatMultiplier));

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            //statsTotal.Mana = (float)Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            statsTotal.Mp5 = (float)Math.Floor(statsTotal.Mp5 * (1f + statsTotal.BonusManaMultiplier));
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            statsTotal.HitRating += statsTotal.Spirit - statsBase.Spirit + statsTotal.ExpertiseRating;

            return statsTotal;
        }
    }
}
