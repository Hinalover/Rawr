using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Rawr.Elemental.Spells;

namespace Rawr.Elemental {
    [Rawr.Calculations.RawrModelInfo("Elemental", "Spell_Nature_Lightning", CharacterClass.Shaman)]
    public class CalculationsElemental : CalculationsBase
    {
        #region Gemming Templates
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Meta
                int burning = 76885;
                int fleet   = 76887;

                if (_defaultGemmingTemplates == null)
                {
                    Gemming gemming = new Gemming();
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, burning, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, burning, true));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, burning, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Jewelcrafter", 3, burning, false));

                    _defaultGemmingTemplates.AddRange(gemming.addCogwheelTemplates("Engineer", 0, burning, false));
                }
                return _defaultGemmingTemplates;
            }
        }
        #endregion
        #region Variables and Properties
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Summary:Overall Points*Sum of burst and sustained points",
                    "Summary:Burst Points*DPS until you go out of mana",
                    "Summary:Sustained Points*Total DPS of the fight",

                    "Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Stamina",
                    "Basic Stats:Spell Power",
                    "Basic Stats:Hit Rating",
                    "Basic Stats:Crit Rating",
                    "Basic Stats:Haste Rating",
                    "Basic Stats:Mastery Rating", 
                    "Basic Stats:Mana Regen",

                    "Combat Stats:Average Health",
                    "Combat Stats:Average Mana",
                    "Combat Stats:Average Stamina",
                    "Combat Stats:Average Spell Power",
                    "Combat Stats:Average Hit Rating",
                    "Combat Stats:Average Crit Rating",
                    "Combat Stats:Average Haste Rating",
                    "Combat Stats:Average Mastery Rating",
                    "Combat Stats:Average Mana Regen",

                    "Attacks:Lightning Bolt",
                    "Attacks:Chain Lightning",
                    "Attacks:Lava Burst",
                    "Attacks:Flame Shock",
                    "Attacks:Earth Shock",
                    "Attacks:Fulmination",
                    "Attacks:Searing Totem",
                    "Attacks:Magma Totem",
                    "Attacks:Unleashed Elements",
                    
                    "Simulation:Simulation",
                    "Simulation:Rotation",
                    "Simulation:Priority List"
                };
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        { 
            get 
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[]
                    {
                        "% Chance to Miss (Spell)",
                        "Health"
                    };
                return _optimizableCalculationLabels;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Burst DPS", Colors.Red);  // 0xFFFF0000 (Red)
                    _subPointNameColors.Add("Sustained DPS", Colors.Blue);  // 0xFF0000FF (Blue)
                    //_subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));  // 0xFF408020
                }
                return _subPointNameColors;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Shaman; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationElemental(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsElemental(); }
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelElemental()); } }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsElemental calcOpts = serializer.Deserialize(reader) as CalculationOptionsElemental;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        #region Glyphs
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            /*
             * Major:
             * Glyph of Capacitor Totem
             * Glyph of Chain Lightning
             * Glyph of Chaining
             * Glyph of Cleansing Waters
             * Glyph of Feral Spirit
             * Glyph of Fire Elemental Totem
             * Glyph of Fire Nova
             * Glyph of Flame Shock
             * Glyph of Frost Shock
             * Glyph of Ghost Wolf
             * Glyph of Grounding Totem
             * Glyph of Healing Storm 
             * Glyph of Healing Stream Totem
             * Glyph of Healing Wave
             * Glyph of Hex
             * Glyph of Purge
             * Glyph of Riptide
             * Glyph of Shamanistic Rage
             * Glyph of Spirit Walk
             * Glyph of Spiritwalker's Grace
             * Glyph of Telluric Currents
             * Glyph of Thunder
             * Glyph of Totemic Recall
             * Glyph of Unleashed Lightning
             * Glyph of Unstable Earth
             * Glyph of Water Shield
             * Glyph of Wind Shear
             * Minor:
             * Glyph of Astral Recall
             * Glyph of Far Sight
             * Glyph of Lava Lash
             * Glyph of the Lakestrider
             * Glyph of the Spectral Wolf
             * Glyph of Thunderstorm
             * Glyph of Totemic Encirclement
             */
            if (_relevantGlyphs == null)
            {
                return base.GetRelevantGlyphs();
                /*_relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("");*/
            }
            return _relevantGlyphs;
            
        }
        #endregion

        #region Triggers
        /// <summary>
        /// List of SpecialEffect Triggers that are relevant for elemental model
        /// </summary>
        internal static List<Trigger> _relevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                if (_relevantTriggers == null)
                    _relevantTriggers = new List<Trigger>()
                    {
                        Trigger.Use,
                        Trigger.SpellCast, 
                        Trigger.SpellHit, 
                        Trigger.SpellCrit, 
                        Trigger.SpellMiss,
                        Trigger.DamageSpellCast, 
                        Trigger.DamageSpellHit, 
                        Trigger.DamageSpellCrit,
                        Trigger.DoTTick,
                        Trigger.DamageDone,
                        Trigger.DamageOrHealingDone,
                        Trigger.ShamanLightningBolt,
                        Trigger.ShamanFlameShockDoTTick,
                        Trigger.ShamanShock,
                    };

                return _relevantTriggers;
            }
        }
        #endregion

        #region Items
        /// <summary>
        /// List of itemtypes that are relevant for elemental
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
                        ItemType.Mail,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.Shield,
                        ItemType.Staff,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if ((item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Totem && item.Type != ItemType.Relic)))
                return false;
            return base.IsItemRelevant(item);
        }
        #endregion

        public override bool IsBuffRelevant(Buff buff, Character character) {
            string name = buff.Name;
            if (!buff.AllowedClasses.Contains(CharacterClass.Shaman)) {
                return false;
            }
            return base.IsBuffRelevant(buff, character);
        }

        //FIXME
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // No ranged enchants allowed
            if (enchant.Slot == ItemSlot.Ranged) return false;
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if (slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
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

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                #region Basic stats
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit= stats.Spirit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellHaste = stats.SpellHaste,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                Mp5 = stats.Mp5,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestore = stats.ManaRestore,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                #endregion
                #region Multipliers
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                #endregion
                #region Sets
                BonusDamageMultiplierLavaBurst = stats.BonusDamageMultiplierLavaBurst,
                #endregion
                #region Misc Damage
                NatureDamage = stats.NatureDamage,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                ShadowDamage = stats.ShadowDamage
                #endregion
            };
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            #endregion
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            #region Basic stats
            if (stats.Mana != 0) return true;
            if (stats.Spirit != 0) return true;
            if (stats.SpellCrit != 0) return true;
            if (stats.SpellCritOnTarget != 0) return true;
            if (stats.SpellHit != 0) return true;
            if (stats.SpellHaste != 0) return true;
            if (stats.SpellPower != 0) return true;
            if (stats.CritRating != 0) return true;
            if (stats.HasteRating != 0) return true;
            if (stats.HitRating != 0) return true;
            if (stats.Mastery != 0) return true;
            if (stats.MasteryRating != 0) return true;
            if (stats.SpellFireDamageRating != 0) return true;
            if (stats.SpellNatureDamageRating != 0) return true;
            if (stats.SpellFrostDamageRating != 0) return true;
            if (stats.Mp5 != 0) return true;
            if (stats.ManaRestoreFromMaxManaPerSecond != 0) return true;
            if (stats.ManaRestore != 0) return true;
            if (stats.NatureSpellsManaCostReduction != 0) return true;
            if (stats.MovementSpeed != 0) return true;
            if (stats.SnareRootDurReduc != 0) return true;
            if (stats.FearDurReduc != 0) return true;
            if (stats.StunDurReduc != 0) return true;
            #endregion
            #region Multipliers
            if (stats.BonusIntellectMultiplier != 0) return true;
            if (stats.BonusSpiritMultiplier != 0) return true;
            if (stats.BonusSpellCritDamageMultiplier != 0) return true;
            if (stats.BonusSpellPowerMultiplier != 0) return true;
            if (stats.BonusFireDamageMultiplier != 0) return true;
            if (stats.BonusFrostDamageMultiplier != 0) return true;
            if (stats.BonusFrostDamageMultiplier != 0) return true;
            if (stats.BonusDamageMultiplier != 0) return true;
            #endregion
            #region Sets
            if (stats.BonusDamageMultiplierLavaBurst != 0) return true;
            #endregion
            #region Misc Damage
            if (stats.NatureDamage != 0) return true;
            if (stats.ArcaneDamage != 0) return true;
            if (stats.FireDamage != 0) return true;
            if (stats.ShadowDamage != 0) return true;
            #endregion
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    if (HasRelevantStats(effect.Stats)) return true;
                }
            }
            #endregion
            return false;
        }

        /*public Stats GetBuffsStats(Character character, CalculationOptionsElemental calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
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
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }*/

        public override void SetDefaults(Character character)
        {
            // Passive buffs:
            // Stats
            character.ActiveBuffsAdd("Mark of the Wild");
            // Stam
            character.ActiveBuffsAdd("Power Word: Fortitude");
            // SP%
            character.ActiveBuffsAdd("Burning Wrath");
            // Spell Haste
            character.ActiveBuffsAdd("Elemental Oath");
            // Crit
            character.ActiveBuffsAdd("Leader of the Pack");
            // Mastery
            character.ActiveBuffsAdd("Grace of Air");
            // Temporary Buffs:
            // Bloodlust
            //character.ActiveBuffsAdd("Heroism/Bloodlust");
            // Debuffs:
            // Magic vulnerability
            character.ActiveBuffsAdd("Curse of the Elements");
            // Consumables:
            // Flask
            character.ActiveBuffsAdd("Flask of the Warm Sun");
            // Food
            character.ActiveBuffsAdd("Intellect Food");
        }
        #endregion

        #region Custom Charts
        private string[] _customChartNames = {};
        public override string[] CustomChartNames { get { return _customChartNames; } }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) { return new ComparisonCalculationBase[0]; }
        #endregion

        #region Model Specific Variables and Functions
        private static int _reforgePriority = 0;
        private static bool _enableSpiritToHit = false;
        #endregion

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsElemental calc = new CharacterCalculationsElemental();
            if (character == null) { return calc; }
            CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
            if (calcOpts == null) { return calc; }
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) { bossOpts = new BossOptions(); }

            _reforgePriority = calcOpts.ReforgePriority;
            _enableSpiritToHit = calcOpts.AllowReforgingSpiritToHit;
            Stats stats = GetCharacterStats(character, additionalItem, true);
            calc.BasicStats = stats;
            //calc.EnhSimStats = GetCharacterStats(character, additionalItem, false);

            calc.LocalCharacter = character;

            Rawr.Elemental.Estimation.solve(calc, calcOpts, bossOpts);

            return calc;
        }

        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsTotal = GetCharacterStats(character, additionalItem, true)/* as StatsElemental*/;

            return statsTotal;
        }

        private Stats GetCharacterStats(Character character, Item additionalItem, Boolean buffs)
        {
            CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
            BossOptions bossOpts = character.BossOptions;
            ShamanTalents talents = character.ShamanTalents;

            Stats statsRace = new Stats();
            statsRace.Accumulate(BaseStats.GetBaseStats(character));
            Stats statsItems = new Stats();
            AccumulateItemStats(statsItems, character, additionalItem);
            Stats statsBuffs = new Stats();
            AccumulateBuffsStats(statsBuffs, character.ActiveBuffs);

            StatsElemental statsTalents = new StatsElemental()
            {
                //// Passive Bonuses
                // Mail Specialization
                BonusIntellectMultiplier = Character.ValidateArmorSpecialization(character, ItemType.Mail) ? 0.05f : 0f,
                // Spiritual Insight
                BonusManaMultiplier = 4f,
                // Elemental Fury
                BonusCritDamageMultiplier = 0.5f,


                //// Talents
                SpellHaste = talents.AncestralSwiftness * 0.05f,

                //// Glyphs

            };

            #region Set Bonuses
            StatsElemental statsSetBonuses = new StatsElemental();
            int T14Count;
            character.SetBonusCount.TryGetValue("Elemental T14", out T14Count);
            if (T14Count >= 2)
            {
                // Increases the damage done by your Lightning Bolt spell by 5%.
                statsSetBonuses.BonusLightningBoltDamageMultiplier = (1f + statsSetBonuses.BonusLightningBoltDamageMultiplier) * (1f + 0.05f) - 1f;
            }
            if (T14Count >= 4)
            {
                // Your Rolling Thunder ability now grants 2 Lightning Shield charges each time it triggers.
            }
            #endregion

            Stats statsTotal = new Stats();
            statsTotal.Accumulate(statsRace);
            statsTotal.Accumulate(statsItems);
            statsTotal.Accumulate(statsTalents);
            statsTotal.Accumulate(statsSetBonuses);

            if (buffs)
            {
                statsTotal.Accumulate(statsBuffs);
            }

            if (statsTotal.HighestStat > 0) {
                if (statsTotal.Spirit > statsTotal.Intellect) {
                    statsTotal.Spirit += (statsTotal.HighestStat * 15f / 50f);
                } else {
                    statsTotal.Intellect += (statsTotal.HighestStat * 15f / 50f);
                }
            }

            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1f + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1f + statsTotal.BonusSpiritMultiplier));

            statsTotal.Health += (float)Math.Floor(StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            //statsTotal.Mana += (float)Math.Floor(StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));

            statsTotal.SpellPower += statsTotal.Intellect * 2f;
            statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower * (1f + statsTotal.BonusSpellPowerMultiplier));

            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating + statsTotal.Spirit);

            return statsTotal;
        }
    }
}
public static class Constants
{
    // Source: http://www.wowwiki.com/Base_mana
    public static float BaseMana = 20000;
}
