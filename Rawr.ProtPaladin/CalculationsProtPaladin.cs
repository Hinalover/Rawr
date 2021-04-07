using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.Base;

namespace Rawr.ProtPaladin
{
    [Rawr.Calculations.RawrModelInfo("ProtPaladin", "Ability_Paladin_JudgementsOfTheJust", CharacterClass.Paladin)]
    public class CalculationsProtPaladin : CalculationsBase
    {
        #region Variables and Properties
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for ProtPaladin
                // Red
                int[] bold = { 52081, 52176, 52206, 52255 };        // Strength
                //int[] delicate = { 52082, 52175, 52212, 52258 };    // Agility
                int[] flashing = { 52083, 52174, 52216, 52259 };    // Parry
                int[] precise = { 52085, 52172, 52230, 52260 };     // Expertise

                // Purple
                int[] accurate = { 52105, 52152, 52203, 52203 };    // Expertise + Hit
                int[] defender = { 52097, 52160, 52210, 52210 };    // Parry + Stamina
                int[] etched = { 52101, 52156, 52213, 52213 };      // Strength + Hit
                //int[] glinting = { 52102, 52155, 52220, 52220 };    // Agility + Hit
                int[] guardian = { 52099, 52158, 52221, 52221 };    // Expertise + Stamina
                int[] retaliating = { 52103, 52154, 52234, 52234 }; // Parry + Hit
                //int[] shifting = { 52096, 52161, 52238, 52238 };    // Agility + Stamina
                int[] sovereign = { 52095, 52162, 52243, 52243 };   // Strength + Stamina

                // Blue
                int[] rigid = { 52089, 52168, 52235, 52264 };       // Hit
                int[] solid = { 52086, 52171, 52242, 52261 };       // Stamina

                // Green
                int[] nimble = { 52120, 52137, 52227, 52227 };      // Dodge + Hit
                int[] puissant = { 52126, 52131, 52231, 52231 };    // Mastery + Stamina
                int[] regal = { 52119, 52138, 52233, 52233 };       // Dodge + Stamina
                int[] sensei = { 52128, 52129, 52237, 52237 };      // Mastery + Hit

                // Yellow
                int[] fractured = { 52094, 52163, 52219, 52269 };   // Mastery
                int[] subtle = { 52090, 52167, 52247, 52265 };      // Dodge

                // Orange
                //int[] adept = { 52115, 52142, 52204, 52204 };      // Agility + Mastery
                int[] fine = { 52116, 52141, 52215, 52215 };       // Parry + Mastery
                int[] keen = { 52118, 52139, 52224, 52224 };       // Expertise + Mastery
                //int[] polished = { 52106, 52151, 52229, 52229 };   // Agility + Dodge
                int[] resolute = { 52107, 52150, 52249, 52249 };   // Expertise + Dodge
                int[] skillful = { 52114, 52143, 52240, 52240 };   // Strength + Mastery

                // Meta
                int austere = 52294;                               // Stamina + Increased Armor Value
                int eternal = 52293;                               // Stamina + Shield Block Value
                int fleet = 52289;                                 // Mastery + Minor Run Speed

                // Cogwheel
                //int cog_flashing = 59491;                          // Parry
                //int cog_fractured = 59480;                         // Mastery
                //int cog_precise = 59489;                           // Expertise
                //int cog_rigid = 59493;                             // Hit
                //int cog_subtle = 59477;                            // Dodge

                string[] qualityGroupNames = new string[] { "Uncommon", "Perfect Uncommon", "Rare", "Jewelcrafter" };
                string[] typeGroupNames = new string[] { "Survivability", "Mitigation (Dodge)", "Mitigation (Parry)", "Mitigation (Mastery)", "Threat" };
                
                int[] metaTemplates = new int[] { austere, eternal, fleet };

                //    Red           Yellow      Blue        Prismatic
                int[,][] survivabilityTemplates = new int[,][]
                { // Survivability
                    { solid,        solid,      solid,      solid },
                };

                /*int[,][] agilityTemplates = new int[,][]
                { // Mitigation (Agility)
                    { delicate,     delicate,   delicate,   delicate },
                    { delicate,     polished,   shifting,   delicate },
                    { polished,     subtle,     regal,      delicate },
                    { shifting,     regal,      solid,      delicate },
                };*/

                int[,][] dodgeTemplates = new int[,][]
                { // Mitigation (Dodge)
                    { subtle,       subtle,     subtle,     subtle },
                    { flashing,     resolute,   defender,   subtle },
                    { resolute,     subtle,     regal,      subtle },
                    { defender,     regal,      solid,      subtle },
                };

                int[,][] parryTemplates = new int[,][]
                { // Mitigation (Parry)
                    { flashing,     flashing,   flashing,   flashing },
                    { flashing,     fine,       defender,   flashing },
                    { fine,         subtle,     regal,      flashing },
                    { defender,     regal,      solid,      flashing },
                };

                int[,][] masteryTemplates = new int[,][]
                { // Mitigation (Mastery)
                    { fractured,    fractured,  fractured,  fractured },
                    //{ flashing,     fine,       defender,   fractured },
                    { fine,         fractured,  puissant,   fractured },
                    { defender,     fractured,   puissant,      solid },
                    { fine,     fractured,   puissant,      solid },
                };

                int[,][] threatTemplates = new int[,][]
                { // Threat
                    { bold,         bold,       bold,       bold },
                    { bold,         skillful,   sovereign,  bold },
                    { skillful,     subtle,     regal,      bold },
                    { sovereign,    regal,      solid,      bold },
                };

                int[][,][] gemmingTemplates = new int[][,][]
                //{ survivabilityTemplates, agilityTemplates, dodgeTemplates, parryTemplates, masteryTemplates, threatTemplates };
                { survivabilityTemplates, dodgeTemplates, parryTemplates, masteryTemplates, threatTemplates };

                // Generate List of Gemming Templates
                List<GemmingTemplate> gemmingTemplate = new List<GemmingTemplate>();
                for (int qualityId = 0; qualityId <= qualityGroupNames.GetUpperBound(0); qualityId++)
                {
                    for (int typeId = 0; typeId <= typeGroupNames.GetUpperBound(0); typeId++)
                    {
                        for (int templateId = 0; templateId <= gemmingTemplates[typeId].GetUpperBound(0); templateId++)
                        {
                            for (int metaId = 0; metaId <= metaTemplates.GetUpperBound(0); metaId++)
                            {
                                gemmingTemplate.Add(new GemmingTemplate()
                                {
                                    Model       = "ProtPaladin",
                                    Group       = string.Format("{0} - {1}", qualityGroupNames[qualityId], typeGroupNames[typeId]),
                                    RedId       = gemmingTemplates[typeId][templateId, 0][qualityId],
                                    YellowId    = gemmingTemplates[typeId][templateId, 1][qualityId],
                                    BlueId      = gemmingTemplates[typeId][templateId, 2][qualityId],
                                    PrismaticId = gemmingTemplates[typeId][templateId, 3][qualityId],
                                    MetaId      = metaTemplates[metaId],
                                    Enabled     = qualityGroupNames[qualityId] == "Rare" && typeGroupNames[typeId] != "Threat",
                                });
                            }
                        }
                    }
                }
                return gemmingTemplate;
            }
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null) {
                    _calculationOptionsPanel = new CalculationOptionsPanelProtPaladin();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Base Stats:Health",
                    "Base Stats:Mana",
                    "Base Stats:Strength",
                    "Base Stats:Agility",
                    "Base Stats:Stamina",
                    "Base Stats:Intellect",

                    "Defensive Stats:Armor",
                    "Defensive Stats:Miss",
                    "Defensive Stats:Dodge",
                    "Defensive Stats:Parry",
                    "Defensive Stats:Block",
                    "Defensive Stats:Mastery",
                    "Defensive Stats:Attacker Speed",
                    "Defensive Stats:Damage Taken",
                    "Defensive Stats:Guaranteed Reduction",
                    "Defensive Stats:Total Mitigation",

                    "Offensive Stats:Weapon Speed",
                    "Offensive Stats:Weapon Damage",
                    "Offensive Stats:Attack Power",
                    "Offensive Stats:Average Vengeance AP",
                    "Offensive Stats:Spell Power",
                    "Offensive Stats:Hit",
                    "Offensive Stats:Spell Hit",
                    "Offensive Stats:Expertise",
                    "Offensive Stats:Crit",
                    "Offensive Stats:Spell Crit",
                    "Offensive Stats:Haste",
                    "Offensive Stats:Spell Haste",
                    "Offensive Stats:Avoided Attacks",
                    "Offensive Stats:DPS",
                    "Offensive Stats:TPS",

                    @"Complex Stats:Overall Points*Overall Points are a sum of Mitigation, Threat, Survival and Recovery Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.
Remember to set your threat scale as needed.",
                    @"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
                    @"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-avoidance/block) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers.
If you find that you are being killed by burst damage,
focus on Survival Points.",
                    @"Complex Stats:Threat Points*Threat Points represents the average threat per second accumulated scaled by the threat scale.",
                    @"Complex Stats:Recovery Points*Recovery Points represents the average healing per second accumulated scaled by the recovery scale."
                    };
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Health"
                    };
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Ability Damage",
                    "Ability Threat",
                    "Combat Table: Defensive Stats",
                    "Combat Table: Offensive Stats",
                    "Item Budget",
                    };
                return _customChartNames;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Survival", Colors.Blue);
                    _subPointNameColors.Add("Mitigation", Colors.Red);
                    _subPointNameColors.Add("Threat", Colors.Green);
                    _subPointNameColors.Add("Recovery", Color.FromArgb(0xFF, 0xDB, 0x70, 0x93));
                }
                return _subPointNameColors;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Paladin; } }
        #endregion

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtPaladin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtPaladin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsProtPaladin));
            StringReader reader = new StringReader(xml);
            CalculationOptionsProtPaladin calcOpts = serializer.Deserialize(reader) as CalculationOptionsProtPaladin;
            return calcOpts;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsProtPaladin calc = new CharacterCalculationsProtPaladin();
            if (character == null) { return calc; }
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
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

            Attack bossAttack = bossOpts.DefaultMeleeAttack;

            StatsProtPaladin stats = GetCharacterStats(character, additionalItem, calcOpts, bossOpts);

            calc.BasicStats = stats;

            calc.TargetLevel = bossOpts.Level;
            
            // Transfer derived stats to calculations
            calc.Hit = StatConversion.GetHitFromRating(stats.HitRating) + stats.PhysicalHit;
            calc.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating) + stats.Expertise;
            calc.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating + stats.ExpertiseRating) + stats.SpellHit;
            calc.Crit = Math.Max(0, StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Paladin) + StatConversion.GetCritFromRating(stats.CritRating) + StatConversion.NPC_LEVEL_CRIT_MOD[bossOpts.Level - character.Level]) + stats.PhysicalCrit;
            calc.SpellCrit = Math.Max(0, StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[bossOpts.Level - character.Level]) + stats.SpellCrit;
            calc.Haste = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Paladin);
            calc.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;
            calc.Miss = 0.05f - 0.002f * (bossOpts.Level - character.Level);
            calc.Dodge = StatConversion.GetDRAvoidanceChance(character, stats, HitResult.Dodge, bossOpts.Level);
            calc.Parry = StatConversion.GetDRAvoidanceChance(character, stats, HitResult.Parry, bossOpts.Level);
            calc.Mastery = stats.Mastery + StatConversion.GetMasteryFromRating(stats.MasteryRating);
            calc.Block = stats.Block + calc.Mastery * 0.01f - 0.002f * (bossOpts.Level - character.Level);
            // Physical haste stat affects swing speed, not actual melee haste
            calc.WeaponSpeed = (character.MainHand == null ? 2.4f : character.MainHand.Speed) / (1 + calc.Haste) / (1 + stats.PhysicalHaste);

            calc.AvengingWrathUptime = AvengingWrath.GetAverageUptime(0, 1);

            calc.HolyAvengerUptime = HolyAvenger.GetAverageUptime(0, 1);

            ProtPaladinSolver solver = new ProtPaladinSolver();
            solver.Solve(character, calcOpts, bossOpts, ref calc, bossAttack);
            // Get averaged special effect stats and apply them to the base stats
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                Stats effectStats = effect.GetAverageStats(solver.TriggerIntervals, solver.TriggerChances, calc.WeaponSpeed, calc.Haste, bossOpts.BerserkTimer, 1f);
                // Handle effects for highest stat/highest secondary stat
                if (effectStats.HighestStat > 0)
                    effectStats.Strength = effectStats.HighestStat;
                else if (effectStats.HighestSecondaryStat > 0)
                {
                    if (calc.BasicStats.MasteryRating > calc.BasicStats.HasteRating)
                        effectStats.MasteryRating = effectStats.HighestSecondaryStat;
                    else
                        effectStats.HasteRating = effectStats.HighestSecondaryStat;
                }
                calc.BasicStats.Accumulate(effectStats);
            }

            // Transfer derived stats to calculations
            calc.Hit = StatConversion.GetHitFromRating(stats.HitRating) + stats.PhysicalHit;
            calc.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating) + stats.Expertise;
            calc.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating + stats.ExpertiseRating) + stats.SpellHit;
            calc.Crit = Math.Max(0, StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Paladin) + StatConversion.GetCritFromRating(stats.CritRating) + StatConversion.NPC_LEVEL_CRIT_MOD[bossOpts.Level - character.Level]) + stats.PhysicalCrit;
            calc.SpellCrit = Math.Max(0, StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[bossOpts.Level - character.Level]) + stats.SpellCrit;
            calc.Haste = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Paladin);
            calc.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;
            calc.Miss = 0.05f - 0.002f * (bossOpts.Level - character.Level);
            calc.Dodge = StatConversion.GetDRAvoidanceChance(character, stats, HitResult.Dodge, bossOpts.Level);
            calc.Parry = StatConversion.GetDRAvoidanceChance(character, stats, HitResult.Parry, bossOpts.Level);
            calc.Mastery = stats.Mastery + StatConversion.GetMasteryFromRating(stats.MasteryRating);
            calc.Block = stats.Block + calc.Mastery * 0.01f - 0.002f * (bossOpts.Level - character.Level);
            // Physical haste stat affects swing speed, not actual melee haste
            calc.WeaponSpeed = (character.MainHand == null ? 2.4f : character.MainHand.Speed) / (1 + calc.Haste) / (1 + stats.PhysicalHaste);

            solver.Solve(character, calcOpts, bossOpts, ref calc, bossAttack);
            
            calc.OverallPoints = calc.MitigationPoints + calc.SurvivabilityPoints + calc.ThreatPoints + calc.RecoveryPoints;

            return calc;
        }

        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return GetItemStats(character, additionalItem, calcOpts);
        }

        public Stats GetItemStats(Character character, Item additionalItem, CalculationOptionsProtPaladin options)
        {
            Stats statsItems = base.GetItemStats(character, additionalItem);
            return statsItems;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            if (calcOpts == null) calcOpts = new CalculationOptionsProtPaladin();

            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();

            // Make sure there is at least one attack in the list.
            // If there's not, add a Default Melee Attack for processing
            if (bossOpts.Attacks.Count < 1)
            {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Make sure there is a default melee attack
            // If the above processed, there will be one so this won't have to process
            // If the above didn't process and there isn't one, add one now
            if (bossOpts.DefaultMeleeAttack == null)
            {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }

            return GetCharacterStats(character, additionalItem, calcOpts, bossOpts);
        }

        public SpecialEffect DivineProtection = new SpecialEffect(Trigger.Use, new Stats { DamageTakenReductionMultiplier = 0.2f }, 10, 60);
        public SpecialEffect ArdentDefender = new SpecialEffect(Trigger.Use, new Stats { DamageTakenReductionMultiplier = 0.2f }, 10, 180);
        public SpecialEffect GuardianOfAncientKings = new SpecialEffect(Trigger.Use, new Stats { DamageTakenReductionMultiplier = 0.5f }, 12f, 180f);
        public SpecialEffect AvengingWrath = new SpecialEffect(Trigger.Use, new Stats { BonusDamageMultiplier = 0.2f, BonusHealingDoneMultiplier = 0.2f }, 20, 180f);
        public SpecialEffect HolyAvenger = new SpecialEffect(Trigger.Use, new Stats {  }, 18, 120f);

        public StatsProtPaladin GetCharacterStats(Character character, Item additionalItem, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            PaladinTalents talents = character.PaladinTalents;

            Stats statsBase = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            statsBase.Expertise += BaseStats.GetRacialExpertise(character, ItemSlot.MainHand);

            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsItems = GetItemStats(character, additionalItem, calcOpts);
            Stats statsTalents = new Stats()
            {
                // Guarded by the Light
                BonusStaminaMultiplier = 0.15f,
                Block = 0.1f,
                // Sanctuary
                BaseArmorMultiplier = 0.1f,
                DamageTakenReductionMultiplier = 0.15f,
                Dodge = 0.02f
            };
            StatsProtPaladin statsTotal = new StatsProtPaladin();

            // Avenging Wrath lasts 50% longer with Sanctified Wrath
            if (character.PaladinTalents.SanctifiedWrath > 0)
                AvengingWrath.Duration = 30;
            else
                AvengingWrath.Duration = 20;

            // Add paladin-specific tank cooldowns
            statsBase.AddSpecialEffect(GuardianOfAncientKings);
            statsBase.AddSpecialEffect(AvengingWrath);
            if (character.PaladinTalents.GlyphOfDivineProtection)
                statsTalents.AddSpecialEffect(DivineProtection);
            if (character.PaladinTalents.ArdentDefender > 0)
                statsTalents.AddSpecialEffect(ArdentDefender);

            statsTotal.Accumulate(statsBase);
            statsTotal.Accumulate(statsItems);
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsTalents);

            // Base stats: Intellect, stamina, strength, agility
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1.0f + statsTotal.BonusIntellectMultiplier));

            statsTotal.BaseAgility = statsBase.Agility + statsTalents.Agility;

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina
                                    * (1.0f + statsTotal.BonusStaminaMultiplier)
                                    * (Character.ValidateArmorSpecialization(character, ItemType.Plate) ? 1.05f : 1f)); // Plate specialization

            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1.0f + statsTotal.BonusStrengthMultiplier));

            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1.0f + statsTotal.BonusAgilityMultiplier));

            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            /// Derived stats: Health, mana, spell power, attack power
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            //statsTotal.Mana = (float)Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            //statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            statsTotal.MaxHealthDamageProc = statsTotal.Health * statsTotal.MaxHealthDamageProc;

            statsTotal.AttackPower += statsTotal.Strength * 2f;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.SpellPower = (float)Math.Floor(statsTotal.AttackPower / 2.0f);

            // Weapon damage
            statsTotal.WeaponDamage = (character.MainHand == null ? 0f : (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsProtPaladin calculations = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;

            switch (chartName)
            {
                #region Ability Damage/Threat
                case "Ability Damage":
                case "Ability Threat":
                    {
                        ComparisonCalculationBase[] comparisons = new ComparisonCalculationBase[calculations.Abilities.Count];
                        int j = 0;
                        foreach (var abilities in calculations.Abilities)
                        {
                            string abilityName = abilities.Key;
                            float abilityDamage = abilities.Value;
                            ComparisonCalculationProtPaladin comparison = new ComparisonCalculationProtPaladin();

                            comparison.Name = abilityName;
                            if (chartName == "Ability Damage")
                            {
                                comparison.MitigationPoints = abilityDamage;
                                comparison.SurvivalPoints = (abilityName == "CS" || abilityName == "J" || abilityName == "SotR") ? calculations.Abilities["SoT"] : 0;
                            }
                            if (chartName == "Ability Threat")
                            {
                                comparison.ThreatPoints = abilityDamage * 5;
                                comparison.RecoveryPoints = (abilityName == "CS" || abilityName == "J" || abilityName == "SotR") ? calculations.Abilities["SoT"] * 5 : 0;
                            }

                            comparison.OverallPoints = comparison.SurvivalPoints + comparison.ThreatPoints + comparison.MitigationPoints + comparison.RecoveryPoints;
                            comparisons[j] = comparison;
                            j++;
                        }
                        return comparisons;
                    }
                #endregion
                #region Combat Table: Defensive Stats
                case "Combat Table: Defensive Stats":
                    {
                        ComparisonCalculationProtPaladin calcMiss = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcDodge = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcParry = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcBlock = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcHit = new ComparisonCalculationProtPaladin();
                        if (calculations != null)
                        {
                            float chanceToBeHit = 1.0f - (calculations.Dodge + calculations.Miss + calculations.Parry);
                            calcMiss.Name  = "1 Miss";
                            calcDodge.Name = "2 Dodge";
                            calcParry.Name = "3 Parry";
                            calcHit.Name = "4 Hit";
                            calcBlock.Name = "5 Block";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.Miss * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.Dodge * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.Parry * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = chanceToBeHit * (1 - calculations.Block) * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = chanceToBeHit * calculations.Block * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcBlock, calcHit };
                    }
                #endregion
                #region Combat Table: Offensive Stats
                case "Combat Table: Offensive Stats":
                    {
                        ComparisonCalculationProtPaladin calcMiss = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcDodge = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcParry = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcGlance = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcBlock = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrit = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcHit = new ComparisonCalculationProtPaladin();
                        if (calculations != null)
                        {
                            calcMiss.Name   = "1 Miss";
                            calcDodge.Name  = "2 Dodge";
                            calcParry.Name  = "3 Parry";
                            calcGlance.Name = "4 Glancing";
                            calcCrit.Name   = "5 Crit";
                            calcHit.Name = "6 Hit";
                            calcBlock.Name = "7 Block";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.MissedAttacks * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.DodgedAttacks * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.ParriedAttacks * 100.0f;
                            calcGlance.OverallPoints = calcGlance.MitigationPoints = calculations.GlancingAttacks * 100.0f;
                            calcCrit.OverallPoints = calcCrit.SurvivalPoints = calculations.Crit * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = (1.0f - (calculations.AvoidedAttacks + calculations.GlancingAttacks + calculations.BlockedAttacks + calculations.Crit)) * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = calculations.BlockedAttacks * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit };
                    }
                #endregion
                #region Item Budget
                case "Item Budget":
                    CharacterCalculationsProtPaladin calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = (10f * 10f) / 0.667f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcMasteryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { MasteryRating = 10f } }) as CharacterCalculationsProtPaladin;

                    //Differential Calculations for Agi
                    CharacterCalculationsProtPaladin calcAtAdd = calcBaseValue;
                    float agiToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
                    {
                        agiToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    CharacterCalculationsProtPaladin calcAtSubtract = calcBaseValue;
                    float agiToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
                    {
                        agiToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    agiToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAgi = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Agility",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints) / (agiToAdd - agiToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)
                    };

                    //Differential Calculations for Str
                    calcAtAdd = calcBaseValue;
                    float strToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd <= 22)
                    {
                        strToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float strToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract >= -22)
                    {
                        strToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    strToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonStr = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Strength",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints) / (strToAdd - strToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
                    };


                    //Differential Calculations for AC
                    calcAtAdd = calcBaseValue;
                    float acToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
                    {
                        acToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float acToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
                    {
                        acToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    acToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAC = new ComparisonCalculationProtPaladin()
                    {
                        Name = "100 Armor",
                        OverallPoints = 100f * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
                        MitigationPoints = 100f * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract),
                        SurvivalPoints = 100f * (calcAtAdd.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints) / (acToAdd - acToSubtract),
                        ThreatPoints = 100f * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract),
                    };


                    //Differential Calculations for Sta
                    calcAtAdd = calcBaseValue;
                    float staToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
                    {
                        staToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float staToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
                    {
                        staToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    staToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonSta = new ComparisonCalculationProtPaladin()
                    {
                        Name = "15 Stamina",
                        OverallPoints = (10f / 0.667f) * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
                        MitigationPoints = (10f / 0.667f) * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract),
                        SurvivalPoints = (10f / 0.667f) * (calcAtAdd.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints) / (staToAdd - staToSubtract),
                        ThreatPoints = (10f / 0.667f) * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)
                    };

                    return new ComparisonCalculationBase[] { 
                        comparisonStr,
                        comparisonAgi,
                        comparisonAC,
                        comparisonSta,
                        new ComparisonCalculationProtPaladin() { Name = "10 Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Block Rating",
                            OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcBlockValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Haste Rating",
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHasteValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Expertise Rating",
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcExpertiseValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Hit Rating",
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHitValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "150 Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Resilience",
                            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcResilValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Mastery Rating",
                            OverallPoints = (calcMasteryValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcMasteryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcMasteryValue.SurvivabilityPoints - calcBaseValue.SurvivabilityPoints),
                            ThreatPoints = (calcMasteryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                    };
                #endregion 
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        #region Relevancy Methods
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new ItemType[] {
                        ItemType.Plate,
                        ItemType.None,
                        ItemType.Shield,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword,
                    });
                }
                return _relevantItemTypes;
            }
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of the Alabaster Shield");
                _relevantGlyphs.Add("Glyph of Final Wrath");
                _relevantGlyphs.Add("Glyph of Focused Shield");
                _relevantGlyphs.Add("Glyph of Avenging Wrath");
                _relevantGlyphs.Add("Glyph of Hammer of the Righteous");
                _relevantGlyphs.Add("Glyph of Word of Glory");
            }
            return _relevantGlyphs;
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            // No ranged enchants allowed
            if (enchant.Slot == ItemSlot.Ranged) return false;
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if (slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) return false;
            // Filters out Death Knight and Two-Hander Enchants
            if (enchant.Name.StartsWith("Rune of the") || enchant.Slot == ItemSlot.TwoHand) return false;

            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            if (slot == CharacterSlot.OffHand && item.Type != ItemType.Shield) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private bool IsTriggerRelevant(Trigger trigger)
        {
            return (
                    trigger == Trigger.Use                   || trigger == Trigger.MeleeCrit            ||
                    trigger == Trigger.MeleeHit              || trigger == Trigger.PhysicalCrit         ||
                    trigger == Trigger.PhysicalAttack        || trigger == Trigger.MeleeAttack          ||
                    trigger == Trigger.PhysicalHit           || trigger == Trigger.DoTTick              ||
                    trigger == Trigger.DamageDone            || trigger == Trigger.DamageOrHealingDone  ||
                    trigger == Trigger.JudgementHit          || trigger == Trigger.DamageParried        ||
                    trigger == Trigger.SpellCast             || trigger == Trigger.SpellHit             ||
                    trigger == Trigger.DamageSpellHit        || trigger == Trigger.DamageTaken          ||
                    trigger == Trigger.DamageTakenPhysical   || trigger == Trigger.DivineProtection     ||
                    trigger == Trigger.DamageTakenPutsMeBelow35PercHealth || trigger == Trigger.DamageTakenPutsMeBelow50PercHealth ||
                    trigger == Trigger.MainHandHit           || trigger == Trigger.CurrentHandHit
            );
        }

        public override Stats GetRelevantStats(Stats stats) {
            Stats s = new Stats() {
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                DodgeRating = stats.DodgeRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                BattlemasterHealthProc = stats.BattlemasterHealthProc,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                Miss = stats.Miss,
                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
                MaxHealthDamageProc = stats.MaxHealthDamageProc,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                PhysicalCrit = stats.PhysicalCrit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                HitRating = stats.HitRating,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,
                BossPhysicalDamageDealtReductionMultiplier = stats.BossPhysicalDamageDealtReductionMultiplier,
                BossAttackSpeedReductionMultiplier = stats.BossAttackSpeedReductionMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,

                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (IsTriggerRelevant(effect.Trigger) && HasRelevantStats(effect.Stats)) {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        public override bool IsItemRelevant(Item item) {
            return base.IsItemRelevant(item);
        }

        public override bool HasRelevantStats(Stats stats) {
            bool relevant = (
                // Basic Stats
                stats.Stamina +
                stats.Health +
                stats.BattlemasterHealthProc + 
                stats.Strength +
                stats.Agility +

                // Tanking Stats
                stats.Armor +
                stats.BonusArmor +
                stats.Block +
                stats.BlockRating +
                stats.Mastery + 
                stats.MasteryRating +
                stats.Dodge +
                stats.DodgeRating +
                stats.Miss +
                stats.Parry +
                stats.ParryRating +

                stats.BaseArmorMultiplier +
                stats.BonusArmorMultiplier +
                stats.BonusBlockValueMultiplier +

                // Threat Stats
                stats.AttackPower +
                stats.SpellPower +
                stats.CritRating +
                stats.PhysicalCrit +
                stats.SpellCrit +
                stats.SpellCritOnTarget +
                stats.HasteRating +
                stats.PhysicalHaste +
                stats.SpellHaste +
                stats.HitRating +
                stats.PhysicalHit +
                stats.SpellHit +
                stats.Expertise +
                stats.ExpertiseRating +

                // Special Stats
                stats.HighestStat +
                stats.HighestSecondaryStat +
                stats.Paragon +
                stats.ArcaneDamage +
                stats.ShadowDamage +
                stats.Healed +
                stats.HealthRestoreFromMaxHealth +
                stats.MaxHealthDamageProc +

                // Multiplier Buffs/Debuffs
                stats.BonusStrengthMultiplier +
                stats.BonusAgilityMultiplier +
                stats.BonusStaminaMultiplier +
                stats.BonusHealthMultiplier +
                stats.BossAttackSpeedReductionMultiplier +
                stats.ThreatIncreaseMultiplier +
                stats.BaseArmorMultiplier +
                stats.BonusArmorMultiplier +
                stats.BonusAttackPowerMultiplier +
                stats.BonusDamageMultiplier +
                stats.BonusWhiteDamageMultiplier +
                stats.BonusPhysicalDamageMultiplier +
                stats.BonusHolyDamageMultiplier +
                stats.DamageTakenReductionMultiplier +
                stats.BossPhysicalDamageDealtReductionMultiplier +

                // BossHandler
                stats.SnareRootDurReduc +
                stats.FearDurReduc +
                stats.StunDurReduc +
                stats.MovementSpeed
                ) != 0;

            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (IsTriggerRelevant(effect.Trigger)) {
                    relevant |= HasRelevantStats(effect.Stats);
                }
            }
            return relevant;
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // Any "Armor" set bonus that applies to Paladins is the protection set bonus
            // Some are "Battlearmor", but the T13 is "Armor", hence the call to ToLower().Contains() instead of BeginsWith()
            if (!String.IsNullOrEmpty(buff.SetName) && buff.AllowedClasses.Contains(character.Class) && buff.SetName.ToLower().Contains("armor"))
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
            }
            return HasRelevantStats(buff.Stats);
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsProtPaladin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Passive Ability Auto-Fixing
            // NOTE: THIS CODE IS FROM DPSWARR, PROTPALADIN MAY MAKE USE OF IT EVENTUALLY TO HANDLE CONFLICTS LIKE CONCENTRATION AURA
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
                hasRelevantBuff = character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
                }
            }*/
            #endregion

            Stats statsBuffs = base.GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs)
            {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }
        public override void SetDefaults(Character character) {
            #region Boss Options
            // Never in back of the Boss
            character.BossOptions.InBack = false;

            int avg = character.AvgWornItemLevel;
            int[] points = new int[] { 350, 358, 365 };
            #region Need a Boss Attack
            character.BossOptions.DamagingTargs = true;
            if (character.BossOptions.DefaultMeleeAttack == null) {
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
            }
            if        (avg <= points[0]) {
                character.BossOptions.Health = 20000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10];
            } else if (avg <= points[1]) {
                character.BossOptions.Health = 35000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25];
            } else if (avg <= points[2]) {
                character.BossOptions.Health = 50000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10H];
            } else if (avg >  points[2]) {
                character.BossOptions.Health = 65000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25H];
            }
            #endregion
            #endregion
        }
        #endregion
    }
}
