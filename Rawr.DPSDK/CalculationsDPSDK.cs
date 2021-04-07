using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.DPSDK
{
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", CharacterClass.DeathKnight)]
    public class CalculationsDPSDK : CalculationsBase
    {
        #region DPSDK Gemming Templates
        // Ok... I broke the templates when I was working on replacing them w/ the new Cata gems.
        // Stealing this from DPSwarr, and everything works.  THANK YOU DPSWarr folks.
        // Ideally, Rawr.Base should handle 0's in the template w/o the special work required.
        // Or at least so it doesn't cause a model to break.
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for DPSWarrs
                //               common uncommon rare  jewel |  fills in gaps if it can
                // Red slots
                int[] red_str = { 52081, 52206, 00000, 52255 }; fixArray(red_str);
                int[] red_exp = { 52085, 52230, 00000, 52260 }; fixArray(red_exp);
                int[] red_hit = { 00000, 00000, 00000, 00000 }; fixArray(red_hit);
                int[] red_mst = { 00000, 00000, 00000, 00000 }; fixArray(red_mst);
                int[] red_crt = { 00000, 00000, 00000, 00000 }; fixArray(red_crt);
                int[] red_has = { 00000, 00000, 00000, 00000 }; fixArray(red_has);
                // Orange slots
                int[] org_str = { 52114, 52240, 00000, 00000 }; fixArray(org_str);
                int[] org_exp = { 52118, 52224, 00000, 00000 }; fixArray(org_exp);
                int[] org_hit = { 00000, 00000, 00000, 00000 }; fixArray(org_hit);
                int[] org_mst = { 52114, 52240, 00000, 00000 }; fixArray(org_mst);
                int[] org_crt = { 52108, 52222, 00000, 00000 }; fixArray(org_crt);
                int[] org_has = { 52111, 52214, 00000, 00000 }; fixArray(org_has);
                // Yellow slots
                int[] ylw_str = { 00000, 00000, 00000, 00000 }; fixArray(ylw_str);
                int[] ylw_exp = { 00000, 00000, 00000, 00000 }; fixArray(ylw_exp);
                int[] ylw_hit = { 00000, 00000, 00000, 00000 }; fixArray(ylw_hit);
                int[] ylw_mst = { 52094, 52219, 00000, 52269 }; fixArray(ylw_mst);
                int[] ylw_crt = { 52091, 52241, 00000, 52266 }; fixArray(ylw_crt);
                int[] ylw_has = { 52093, 52232, 00000, 52268 }; fixArray(ylw_has);
                // Green slots
                int[] grn_str = { 00000, 00000, 00000, 00000 }; fixArray(grn_str);
                int[] grn_exp = { 00000, 00000, 00000, 00000 }; fixArray(grn_exp);
                int[] grn_hit = { 52128, 52237, 00000, 00000 }; fixArray(grn_hit);
                int[] grn_mst = { 52126, 52231, 00000, 00000 }; fixArray(grn_mst);
                int[] grn_crt = { 52121, 52223, 00000, 00000 }; fixArray(grn_crt);
                int[] grn_has = { 52124, 52218, 00000, 00000 }; fixArray(grn_has);
                // Blue slots
                int[] blu_str = { 00000, 00000, 00000, 00000 }; fixArray(blu_str);
                int[] blu_exp = { 00000, 00000, 00000, 00000 }; fixArray(blu_exp);
                int[] blu_hit = { 52089, 52235, 00000, 52264 }; fixArray(blu_hit);
                int[] blu_mst = { 00000, 00000, 00000, 00000 }; fixArray(blu_mst);
                int[] blu_crt = { 00000, 00000, 00000, 00000 }; fixArray(blu_crt);
                int[] blu_has = { 00000, 00000, 00000, 00000 }; fixArray(blu_has);
                // Purple slots
                int[] ppl_str = { 52095, 52243, 00000, 00000 }; fixArray(ppl_str);
                int[] ppl_exp = { 52105, 52203, 00000, 00000 }; fixArray(ppl_exp);
                int[] ppl_hit = { 52101, 52213, 00000, 00000 }; fixArray(ppl_hit);
                int[] ppl_mst = { 00000, 00000, 00000, 00000 }; fixArray(ppl_mst);
                int[] ppl_crt = { 00000, 00000, 00000, 00000 }; fixArray(ppl_crt);
                int[] ppl_has = { 00000, 00000, 00000, 00000 }; fixArray(ppl_has);
                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                const int Reverberating = 68779; // Meta

                string group; bool enabled;
                List<GemmingTemplate> templates = new List<GemmingTemplate>()
                    {
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },
                    };

                #region Strength
                enabled = true;
                group = "Strength";
                // Straight
                AddTemplates(templates,
                    red_str, red_str, red_str,
                    red_str, red_str, red_str,
                    red_str, cog_mst, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_str, ylw_str, blu_str,
                    org_str, ppl_str, grn_str,
                    red_str, cog_mst, group, enabled);
                #endregion

                #region Expertise
                group = "Expertise";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    red_exp, red_exp, red_exp,
                    red_exp, red_exp, red_exp,
                    red_exp, cog_exp, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_exp, ylw_exp, blu_exp,
                    org_exp, ppl_exp, grn_exp,
                    red_exp, cog_exp, group, enabled);
                #endregion

                #region Hit
                group = "Hit";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    blu_hit, blu_hit, blu_hit,
                    blu_hit, blu_hit, blu_hit,
                    blu_hit, cog_hit, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_hit, ylw_hit, blu_hit,
                    org_hit, ppl_hit, grn_hit,
                    blu_hit, cog_hit, group, enabled);
                #endregion

                #region Mastery
                enabled = true;
                group = "Mastery";
                // Straight
                AddTemplates(templates,
                    ylw_mst, ylw_mst, ylw_mst,
                    ylw_mst, ylw_mst, ylw_mst,
                    ylw_mst, cog_mst, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_mst, ylw_mst, blu_mst,
                    org_mst, ppl_mst, grn_mst,
                    ylw_mst, cog_mst, group, enabled);
                #endregion

                #region Crit
                group = "Crit";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    ylw_crt, ylw_crt, ylw_crt,
                    ylw_crt, ylw_crt, ylw_crt,
                    ylw_crt, cog_crt, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_crt, ylw_crt, blu_crt,
                    org_crt, ppl_crt, grn_crt,
                    red_crt, cog_crt, group, enabled);
                #endregion

                #region Haste
                group = "Haste";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    ylw_has, ylw_has, ylw_has,
                    ylw_has, ylw_has, ylw_has,
                    ylw_has, cog_has, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_has, ylw_has, blu_has,
                    org_has, ppl_has, grn_has,
                    red_has, cog_has, group, enabled);
                #endregion

                return templates;
            }
        }
        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green, but no Blue
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue (or Green as set above), but no Purple
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple (or Blue/Green as set above), but no Jewel
        }
        private static void AddTemplates(List<GemmingTemplate> templates, int[] red, int[] ylw, int[] blu, int[] org, int[] prp, int[] grn, int[] pris, int[] cog, string group, bool enabled)
        {
            const int chaotic = 52291; // Meta
            const string groupFormat = "{0} {1}";
            string[] quality = new string[] { "Uncommon", "Rare", "Epic", "Jewelcrafter" };
            for (int j = 0; j < 4; j++)
            {
                // Check to make sure we're not adding the same gem template twice due to repeating JC gems
                if (j != 3 || !(red[j] == red[j - 1] && blu[j] == blu[j - 1] && ylw[j] == ylw[j - 1]))
                {
                    string groupStr = String.Format(groupFormat, quality[j], group);
                    templates.Add(new GemmingTemplate()
                    {
                        Model = "DPSDK",
                        Group = groupStr,
                        RedId = red[j] != 0 ? red[j] : org[j] != 0 ? org[j] : prp[j],
                        YellowId = ylw[j] != 0 ? ylw[j] : org[j] != 0 ? org[j] : grn[j],
                        BlueId = blu[j] != 0 ? blu[j] : prp[j] != 0 ? prp[j] : grn[j],
                        PrismaticId = red[j] != 0 ? red[j] : ylw[j] != 0 ? ylw[j] : blu[j],
                        //CogwheelId = cog[j],
                        HydraulicId = 0,
                        MetaId = chaotic,
                        Enabled = (enabled && j == 1)
                    });
                }
            }
        }
        #endregion

        public static float AddStatMultiplierStat(float statMultiplier, float newValue)
        {
            float updatedStatModifier = ((1f + statMultiplier) * (1f + newValue)) - 1f;
            return updatedStatModifier;
        }
        
        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,0,0,255));
                }
                return _subPointNameColors;
            }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>();
                    string szPreBasic = "Basic Stats";
                    string[] szBasicStats = {
                        "Health",
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise",
                        "Haste Rating",
                        "Armor",
                        "Mastery",};
                    labels.AddRange(BuildLabels(szPreBasic, szBasicStats));
                    string szPreAdvanced = "Advanced Stats";
                    string[] szAdvStats = {
                        "Weapon Damage*Damage before misses and mitigation",
                        "Attack Speed",
                        "Crit Chance",
                        "Avoided Attacks",
                        "Enemy Mitigation",
                        "White HitChance",
                        "Yellow HitChance"};
                    labels.AddRange(BuildLabels(szPreAdvanced, szAdvStats));
                    string szPreDPSBreakdown = "DPS Breakdown";
                    string[] szDPSBreakdown = new string[EnumHelper.GetCount(typeof(DKability))];
                    foreach (int i in EnumHelper.GetValues(typeof(DKability)))
                    {
                        szDPSBreakdown[i] = Enum.GetName(typeof(DKability), i);
                    }
                    labels.AddRange(BuildLabels(szPreDPSBreakdown, szDPSBreakdown));
                    string szPreRotation = "Rotation Data";
                    string[] szRotation = {
                        "Rotation Duration*Duration of the total rotation cycle",
                        "Blood*Number of Runes consumed",
                        "Frost*Number of Runes consumed",
                        "Unholy*Number of Runes consumed",
                        "Death*Number of Runes consumed",
                        "Runic Power*Amount of Runic Power left after rotation.\nNegative values mean more RP generated than used.",
                        "RE Runes*Number of Runes Generated by Runic Empowerment.",
                        "Rune Cooldown*Duration for a single Rune to refresh.",};
                    labels.AddRange(BuildLabels(szPreRotation, szRotation));
                    string szPreDPU = "Damage Per Use";
                    string[] szDPU = {
                        "BB",
                        "BP",
                        "BS",
                        "DC",
                        "DnD",
                        "DS",
                        "Fest",
                        "FF",
                        "FS",
                        "HS",
                        "HB",
                        "IT*Not Including FF",
                        "NS",
                        "OB",
                        "PS*Not Including BP",
                        "RS",
                        "SS",};
                    labels.AddRange(BuildLabels(szPreDPU, szDPU));
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] BuildLabels(string szPrefix, string[] szBody)
        {
            List<string> labels = new List<string>();
            if (null != szBody && szBody.Length > 0)
            {
                foreach (string s in szBody)
                {
                    labels.Add(szPrefix + ":" + s);
                }
            }
            return labels.ToArray();
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// List<ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
        /// </summary>
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null) 
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Plate,
                        ItemType.Sigil,
                        ItemType.Relic,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSDK(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSDK(); }
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsDPSDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSDK;
            return calcOpts;
        }

        private static bool HidingBadStuff { get { return HidingBadStuff_Def || HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        internal static bool HidingBadStuff_Def { get; set; }
        internal static bool HidingBadStuff_Spl { get; set; }
        internal static bool HidingBadStuff_PvP { get; set; }

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
            CharacterCalculationsDPSDK calc = new CharacterCalculationsDPSDK();
            if (character == null) { return calc; }
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            if (calcOpts == null) { return calc; }
            //
            StatsDK stats = new StatsDK();
            DeathKnightTalents talents = character.DeathKnightTalents;

            // Setup initial Boss data.
            // Get Boss from BossOptions data.
            BossOptions hBossOptions = character.BossOptions;
            if (hBossOptions == null) hBossOptions = new BossOptions(); 
            int targetLevel = hBossOptions.Level;

            stats = GetCharacterStats(character, additionalItem) as StatsDK;
            calc.BasicStats = stats.Clone() as StatsDK;
            ApplyRatings(calc.BasicStats);

            DKCombatTable combatTable = new DKCombatTable(character, calc.BasicStats, calc, calcOpts, hBossOptions);
            if (needsDisplayCalculations) combatTable.PostAbilitiesSingleUse(false);
            Rotation rot = new Rotation(combatTable);
            Rotation.Type RotT = rot.GetRotationType(character.DeathKnightTalents);

            // TODO: Fix this so we're not using pre-set rotations/priorities.
            if (RotT == Rotation.Type.Frost)
            {
//                if (calcOpts.RotType == RotationType.MasterFrost)
//                  rot.PRE_MasterFrost();
//                else
                    rot.PRE_Frost();
            }
            else if (RotT == Rotation.Type.Unholy)
                rot.PRE_Unholy();
            else if (RotT == Rotation.Type.Blood)
                rot.PRE_BloodDiseased();
            else
                rot.Solver();

            //TODO: This may need to be handled special since it's to update stats.
            AccumulateSpecialEffectStats(stats, character, calcOpts, combatTable, rot); // Now add in the special effects.
            // problem is that Mastery procs (and probably other procs) are not reaching their new final values.
            // Mastery gets updated, but not all what mastery effects.
            ApplyRatings(stats);
            #region Cinderglacier
            if (stats.CinderglacierProc > 0)
            {
                // How many frost & shadow abilities do we have per min.?
                float CGabs = ((rot.m_FrostSpecials + rot.m_ShadowSpecials) / rot.CurRotationDuration) * 60f;
                float effCG = 0;
                if (CGabs > 0)
                    // Since 3 of those abilities get the 20% buff
                    // Get the effective ammount of CinderGlacier that would be applied across each ability.
                    // it is a proc after all.
                    effCG = 3 / CGabs;
                stats.BonusFrostDamageMultiplier += (.2f * effCG);
                stats.BonusShadowDamageMultiplier += (.2f * effCG);
            }
            #endregion

            // refresh w/ updated stats.
            combatTable = new DKCombatTable(character, stats, calc, calcOpts, hBossOptions);
            combatTable.PostAbilitiesSingleUse(false);
            rot = new Rotation(combatTable);
            RotT = rot.GetRotationType(character.DeathKnightTalents);

            // TODO: Fix this so we're not using pre-set rotations.
            if (RotT == Rotation.Type.Frost)
//                if (calcOpts.RotType == RotationType.MasterFrost)
//                    rot.PRE_MasterFrost();
//                else
                    rot.PRE_Frost();
            else if (RotT == Rotation.Type.Unholy)
                rot.PRE_Unholy();
            else if (RotT == Rotation.Type.Blood)
                rot.PRE_BloodDiseased();
            else
                rot.Solver();

            #region Pet Handling 
            // For UH, this is valid.  For Frost/Blood, we need to have this be 1/3 of the value since it has an uptime of 1 min for every 3.
            float ghouluptime = 1f;
            calc.dpsSub[(int)DKability.Gargoyle] = 0;
            calc.damSub[(int)DKability.Gargoyle] = 0; 
            if (RotT != Rotation.Type.Unholy) ghouluptime = 1f / 3f;
            else 
            {
                // Unholy will also have gargoyles.
                if (RotT == Rotation.Type.Unholy)
                {
                    Pet Gar = new Gargoyle(stats, talents, hBossOptions, calcOpts.presence);
                    float garuptime = .5f / 3f;
                    calc.dpsSub[(int)DKability.Gargoyle] = Gar.DPS * garuptime;
                    calc.damSub[(int)DKability.Gargoyle] = Gar.DPS * 30f; // Duration 30 seconds.
                }
            }
            Pet ghoul = new Ghoul(stats, talents, hBossOptions, calcOpts.presence);
            calc.dpsSub[(int)DKability.Ghoul] = ghoul.DPS * ghouluptime;
            calc.damSub[(int)DKability.Ghoul] = ghoul.DPS * 60f; // Duration 1 min.

            #endregion

            // Stats as Fire damage additive value proc.
            // Fire Dam Multiplier.
            if (stats.ArcaneDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherArcane] += stats.ArcaneDamage;
                calc.damSub[(int)DKability.OtherArcane] += stats.ArcaneDamage * rot.CurRotationDuration;
            }
            if (stats.FireDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherFire] += stats.FireDamage;
                calc.damSub[(int)DKability.OtherFire] += stats.FireDamage * rot.CurRotationDuration;
            }
            if (stats.FrostDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherFrost] += stats.FrostDamage;
                calc.damSub[(int)DKability.OtherFrost] += stats.FrostDamage * rot.CurRotationDuration;
            }
            if (stats.HolyDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherHoly] += stats.HolyDamage;
                calc.damSub[(int)DKability.OtherHoly] += stats.HolyDamage * rot.CurRotationDuration;
            }
            if (stats.NatureDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherNature] += stats.NatureDamage;
                calc.damSub[(int)DKability.OtherNature] += stats.NatureDamage * rot.CurRotationDuration;
            }
            if (stats.PhysicalDamage > 1)
            {
                float fpd = (stats.PhysicalDamage + (stats.AttackPower / 14f) * .625f) * (1 + stats.BonusPhysicalDamageMultiplier);
                calc.dpsSub[(int)DKability.OtherPhysical] += fpd;
                calc.damSub[(int)DKability.OtherPhysical] += fpd * rot.CurRotationDuration;
            }
            if (stats.ShadowDamage > 1)
            {
                calc.dpsSub[(int)DKability.OtherShadow] += stats.ShadowDamage;
                calc.damSub[(int)DKability.OtherShadow] += stats.ShadowDamage * rot.CurRotationDuration;
            }
            if (stats.MaxHealthDamageProc > 1)
            {
                calc.dpsSub[(int)DKability.OtherShadow] += stats.MaxHealthDamageProc;
                calc.damSub[(int)DKability.OtherShadow] += stats.MaxHealthDamageProc * rot.CurRotationDuration;
            }
            calc.RotationTime = rot.CurRotationDuration;
            calc.Blood = rot.m_BloodRunes;
            calc.Frost = rot.m_FrostRunes;
            calc.Unholy = rot.m_UnholyRunes;
            calc.Death = rot.m_DeathRunes;
            calc.RP = rot.m_RunicPower;
            calc.FreeRERunes = rot.m_FreeRunesFromRE;

            calc.EffectiveArmor = stats.Armor;

            calc.OverallPoints = calc.DPSPoints = rot.m_DPS 
                // Add in supplemental damage from other sources
                + calc.dpsSub[(int)DKability.Ghoul] 
                + calc.dpsSub[(int)DKability.Gargoyle]
                + calc.dpsSub[(int)DKability.OtherArcane] 
                + calc.dpsSub[(int)DKability.OtherFire] 
                + calc.dpsSub[(int)DKability.OtherFrost] 
                + calc.dpsSub[(int)DKability.OtherHoly] 
                + calc.dpsSub[(int)DKability.OtherNature] 
                + calc.dpsSub[(int)DKability.OtherShadow]
                + calc.dpsSub[(int)DKability.OtherPhysical];
            if (needsDisplayCalculations)
            {
                AbilityDK_Base a = rot.GetAbilityOfType(DKability.White);
                if (rot.ml_Rot.Count > 1)
                {
                    AbilityDK_Base b;
                    b = rot.GetAbilityOfType(DKability.ScourgeStrike);
                    if (b == null) b = rot.GetAbilityOfType(DKability.FrostStrike);
                    if (b == null) b = rot.GetAbilityOfType(DKability.DeathStrike);
                    calc.YellowHitChance = b.HitChance;
                }
                calc.WhiteHitChance = (a == null ? 0 : a.HitChance + .23f); // + glancing
                calc.MHWeaponDPS = (a == null ? 0 : rot.GetAbilityOfType(DKability.White).DPS);
                if (null != combatTable.MH)
                {
                    calc.MHWeaponDamage = combatTable.MH.damage;
                    calc.MHAttackSpeed = combatTable.MH.hastedSpeed;
                    calc.DodgedAttacks = combatTable.MH.chanceDodged;
                    calc.AvoidedAttacks = combatTable.MH.chanceDodged;
                    if (!hBossOptions.InBack)
                        calc.AvoidedAttacks += combatTable.MH.chanceParried;
                    calc.MissedAttacks = combatTable.MH.chanceMissed;
                }
                if (null != combatTable.OH)
                {
                    a = rot.GetAbilityOfType(DKability.WhiteOH);
                    calc.OHWeaponDPS = (a == null ? 0 : rot.GetAbilityOfType(DKability.WhiteOH).DPS);
                    calc.OHWeaponDamage = combatTable.OH.damage;
                    calc.OHAttackSpeed = combatTable.OH.hastedSpeed;
                }
                calcOpts.szRotReport = rot.ReportRotation();
                calc.m_RuneCD = (float)rot.m_SingleRuneCD / 1000;

                calc.DPSBreakdown(rot);
            }  

            return calc;
        }

        private Stats GetRaceStats(Character character) 
        {
            return BaseStats.GetBaseStats(character.Level, CharacterClass.DeathKnight, character.Race);
        }

        public static void AccumulatePresenceStats(StatsDK PresenceStats, Presence p, DeathKnightTalents t)
        {
            Rotation.Type r = (Rotation.Type)(t.Specialization + 1);
            switch(p)
            {
                case Presence.Blood:
                {
                    if (r == Rotation.Type.Blood)
                    {
                        PresenceStats.CritChanceReduction += 0.06f;
                        PresenceStats.BonusRuneRegeneration += .2f;
                    }
                    PresenceStats.BonusStaminaMultiplier += .25f; 
                    PresenceStats.BaseArmorMultiplier += 0.55f;
                    PresenceStats.DamageTakenReductionMultiplier = 1f - (1f - PresenceStats.DamageTakenReductionMultiplier) * (1f - 0.1f);
                    // Threat bonus.
                    PresenceStats.ThreatIncreaseMultiplier += 1f; 
                    break;
                }
                case Presence.Frost:
                {
                    PresenceStats.BonusDamageMultiplier += 0.1f;
                    PresenceStats.BonusRPMultiplier += 0.2f;  
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    PresenceStats.StunDurReduc += .20f;
                    PresenceStats.FearDurReduc += .20f;
                    PresenceStats.SnareRootDurReduc += .20f;
                    PresenceStats.SilenceDurReduc += .20f;
                    break;
                }
                case Presence.Unholy:
                {
                    if (r == Rotation.Type.Unholy)
                        PresenceStats.PhysicalHaste = AddStatMultiplierStat(PresenceStats.PhysicalHaste, (.10f));
                    PresenceStats.PhysicalHaste = AddStatMultiplierStat(PresenceStats.PhysicalHaste, .1f);
                    PresenceStats.MovementSpeed += .15f;
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    break;
                }
            }
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
            StatsDK statsTotal = new StatsDK();
            if (null == character)
            {
#if DEBUG
                throw new Exception("Character is Null");
#else
                return statsTotal;
#endif
            }
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            if (null == calcOpts) { calcOpts = new CalculationOptionsDPSDK(); }
            DeathKnightTalents talents = character.DeathKnightTalents;
            if (null == talents) { return statsTotal; }

            statsTotal.Accumulate(GetRaceStats(character));
            AccumulateItemStats(statsTotal, character, additionalItem);
            statsTotal = GetRelevantStats(statsTotal) as StatsDK; // GetRel removes any stats specific to the StatsDK object.

            statsTotal.bDW = false;
            if (character.MainHand != null && character.OffHand != null) statsTotal.bDW = true;
            RemoveDuplicateRunes(statsTotal, character, statsTotal.bDW);
            AccumulateBuffsStats(statsTotal, character.ActiveBuffs);

            #region Tank
            int tierCount = 0;
            #region T13
            if (character.SetBonusCount.TryGetValue("Necrotic Boneplate Armor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T13_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T13_Tank = true; }
            }
            if (statsTotal.b2T13_Tank)
            {
                // When an attack drops your health below 35%, one of your Blood Runes 
                // will immediately activate and convert into a Death Rune for the next 
                // 20 sec. This effect cannot occur more than once every 45 sec.
            }
            if (statsTotal.b4T13_Tank)
            {
                // Your Vampiric Blood ability also affects all party and raid members 
                // for 50% of the effect it has on you.
            }
            #endregion
            #region T14
            if (character.SetBonusCount.TryGetValue("Plate of the Lost Catacomb", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T14_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T14_Tank = true; }
            }
            if (statsTotal.b2T14_Tank)
            {
                // Reduces the cooldown of your Vampiric Blood ability by 20 sec.
            }
            if (statsTotal.b2T14_Tank)
            {
                // Increases the healing received from your Death Strike by 10%.
            }
            #endregion
            #endregion
            #region DPS
            #region T13
            if (character.SetBonusCount.TryGetValue("Necrotic Boneplate Battlegear", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T13_DPS = true; }
                if (tierCount >= 4) { statsTotal.b4T13_DPS = true; }
            }
            if (statsTotal.b2T13_DPS)
            {
                // Sudden Doom has a 30% chance and Rime has a 60% chance 
                // to grant 2 charges when triggered instead of 1.
            }
            if (statsTotal.b4T13_DPS)
            {
                // Runic Empowerment has a 25% chance and 
                // Runic Corruption has a 40% chance to also 
                // grant 710 mastery rating for 12 sec when activated.
                statsTotal.AddSpecialEffect(_SE_4T13_RC);
                statsTotal.AddSpecialEffect(_SE_4T13_RE);
            }
            #endregion
            #region T14
            if (character.SetBonusCount.TryGetValue("Battlegear of the Lost Catacomb", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T14_DPS = true; }
                if (tierCount >= 4) { statsTotal.b4T14_DPS = true; }
            }
            if (statsTotal.b2T14_DPS)
            {
                // Your Obliterate, Frost Strike, and Scourge Strike deal 10% increased damage.
            }
            if (statsTotal.b4T14_DPS)
            {
                // Your Pillar of Frost ability grants 5% additional Strength, and your Unholy Frenzy ability grants 10% additional haste.
            }
            #endregion
            #endregion

            AccumulateTalents(statsTotal, character);
            AccumulatePresenceStats(statsTotal, calcOpts.presence, talents);

            return (statsTotal);
        }

        private static void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup)
            {
                if (character.ActiveBuffs.Remove(b)) { removedBuffs.Add(b); }
            }
        }

        private void ApplyRatings(StatsDK statsTotal)
        {
            // Apply ratings.
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);

            statsTotal.Strength += statsTotal.HighestStat + statsTotal.Paragon;

            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= 1 + statsTotal.BonusHealthMultiplier;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower + statsTotal.Strength * 2);
            statsTotal.Armor = (float)Math.Floor(StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier) +
                                StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier));
            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            float HighestSecondaryStatValue = statsTotal.HighestSecondaryStat; // how much HighestSecondaryStat to add
            statsTotal.HighestSecondaryStat = 0f; // remove HighestSecondaryStat stat, since it's not needed
            if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating) {
                statsTotal.CritRating += HighestSecondaryStatValue;
            } else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating) {
                statsTotal.HasteRating += HighestSecondaryStatValue;
            } else /*if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating)*/ {
                statsTotal.MasteryRating += HighestSecondaryStatValue;
            }

            statsTotal.MaxHealthDamageProc *= statsTotal.Health;

            statsTotal.PhysicalHit += StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.DeathKnight);
            statsTotal.PhysicalHaste = AddStatMultiplierStat(statsTotal.PhysicalHaste, StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight));

            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);
        }

        private void AccumulateSpecialEffectStats(StatsDK s, Character c, CalculationOptionsDPSDK calcOpts, DKCombatTable t, Rotation rot)
        {
            StatsSpecialEffects se = new StatsSpecialEffects(t, rot, c.BossOptions );
            StatsDK statSE = new StatsDK();

            foreach (SpecialEffect effect in s.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    statSE = se.getSpecialEffects(effect);
                    s.Accumulate(statSE);
                }
            }
        }

        private void AccumulateGlyphStats(Stats s, DeathKnightTalents t)
        {
            if (t.GlyphofBoneShield)
                s.MovementSpeed = (float)Math.Max(s.MovementSpeed, 1.15f);
        }

        /// <summary>
        /// Local version of GetItemStats()
        /// Includes the Armor style bonus.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats stats = base.GetItemStats(character, additionalItem);
            // Add in armor specialty
            if (GetQualifiesForArmorBonus(character, additionalItem))
            {
                stats.BonusStrengthMultiplier += .05f;
                stats.BonusStaminaMultiplier += .05f;
            }
            return stats;
        }


        public static bool GetQualifiesForArmorBonus(Character c, Item additionalItem)
        {
            // Easier to check if there is a DIS qualifying item than 
            // to ensure than every item matches what we expect.
            ItemTypeList list = new ItemTypeList();
            list.Add(ItemType.Cloth);
            list.Add(ItemType.Leather);
            list.Add(ItemType.Mail);
            if (additionalItem != null && list.Contains(additionalItem.Type))
                return false;
            else if ((c.Chest != null && list.Contains(c.Chest.Type))
                || (c.Feet != null && list.Contains(c.Feet.Type))
                || (c.Hands != null && list.Contains(c.Hands.Type))
                || (c.Head != null && list.Contains(c.Head.Type))
                || (c.Legs != null && list.Contains(c.Legs.Type))
                || (c.Neck != null && list.Contains(c.Neck.Type))
                || (c.Shoulders != null && list.Contains(c.Shoulders.Type))
                || (c.Waist != null && list.Contains(c.Waist.Type))
                || (c.Wrist != null && list.Contains(c.Wrist.Type))
                )
                return false;
            else
                return true;
        }
        /// <summary>Build the talent effects.</summary>
        public static void AccumulateTalents(StatsDK FullCharacterStats, Character character)
        {
            // Which talent tree focus?
            // Specs have a whole stack of things that come with them now.
            // Things that used to be in talents.

            DeathKnightTalents t = character.DeathKnightTalents;

            #region Spec Abilities:
            Rotation.Type r = (Rotation.Type)(t.Specialization + 1);
            switch (r)
            {
                case Rotation.Type.Blood:
                    {
                        #region Special abilities for being blood
                        // Blood Rites (Passive)
                        // Whenever you hit w/ DS, Frost & Unholy Runes will become Death Runes. 3 min CD.
                        // TODO: Implement in Rotation

                        // Vengeance (Passive)
                        // Every time you take damage 2% of the unmitigated damage becomes AP for 20 secs.
                        // TODO: Verify in Rotation/TankDK.

                        // Veteran of the Third War (Passive)
                        // Increase Stam by 9%
                        // Increase Dodge by 2%
                        FullCharacterStats.BonusStaminaMultiplier += .09f;
                        FullCharacterStats.Dodge += .02f;

                        // Dark Command
                        // Heart Strike

                        // Scent of Blood (Passive)
                        // Successful Mainhand AutoAttacks have a chance to increase healing & min healing 
                        // done by your next DS w/i 20 sec by 20% & generate 10 RP. Stacks up to 5 times.
                        // TODO: Implement in DS?

                        // Improved Blood Presence (Passive)
                        // Increases Rune Regen by 20%
                        // reduces chance to be crit by 6% 
                        // when in Blood presence
                        // Implemented in AccumulatePresence

                        // Rune Tap
                        FullCharacterStats.AddSpecialEffect(_SE_RuneTap);
                        // Rune Strike

                        // Blood Parasite (Passive)
                        // Melee attacks have a 10% chance to spawn a blood worm which attack until the burst
                        // healing nearby allies.  Lasts for 20 sec.
                        // TODO: Implement

                        // Scarlet Fever (Passive)                        
                        // BB refreshes disease
                        // BP afflicts enemies with Weakened Blows
                        // TODO: Implement in BB/BP/Rotation

                        // Will of the Necropolis (Passive)
                        // When a damaging attack brings you below 30% health:
                        // CD on Rune tap is refreshed
                        // Next Rune Tap has no cost
                        // All damage taken is reduced by 25% for 8 Secs
                        // Cannot occur more than once every 45 secs.
                        FullCharacterStats.AddSpecialEffect(_SE_WillOfTheNecropolis);

                        // Sanguine Fortitude (Passive)
                        // IBF reduces damage by additional 30%
                        // And Costs no RP.
                        FullCharacterStats.AddSpecialEffect(_SE_IBF[1]);

                        // Dancing Rune Weapon
                        if (t.GlyphofDancingRuneWeapon)
                            FullCharacterStats.AddSpecialEffect(_SE_DRW[1]);
                        else
                            FullCharacterStats.AddSpecialEffect(_SE_DRW[0]);

                        // Vampiric Blood
                        // Bone Shield

                        // Mastery: Blood Shield (Passive)
                        // Everytime you heal yourself with DS in Blood Presence
                        // You gain X% ammount healed as a physical damage absorbsion shield 
                        // TODO: Verify in TankDK

                        // Crimson Scourge (Passive)
                        // Increases BB damage by 40%
                        // And when you land a melee attack on a target infected by BP 
                        // there is a 10% chance that your next BB or DnD will cost no runes.
                        // TODO: Implement in BB & Rotation.

                        // Soul Reaper
                        #endregion
                        break;
                    }
                case Rotation.Type.Frost:
                    {
                        // All other specs have normal IBF.
                        FullCharacterStats.AddSpecialEffect(_SE_IBF[0]);

                        #region Special abilities for being Frost
                        // TODO: Frost Spec abilities
                        // Blood of the North (Passive)
                        // Frost Strike
                        // Howling Blast
                        // Icy Talons (Passive)
                        // Obliterate
                        // Unholy Aura (Passive)
                        // Killing Machine (Passive)
                        // Improved Frost Presence (Passive)
                        // Brittle Bones (Passive)
                        // Pillar of Frost
                        // Rime (Passive)
                        // Might of the Frozen Wastes (Passive)
                        // Threat of Thassarian (Passive)
                        // Mastery: Mastery: Frozen Heart (Passive)
                        // Soul Reaper
                        #endregion
                        break;
                    }
                case Rotation.Type.Unholy:
                    {
                        // All other specs have normal IBF.
                        FullCharacterStats.AddSpecialEffect(_SE_IBF[0]);

                        #region Special abilities for being Unholy
                        // TODO: Unholy Spec abilities
                        // Master of Ghouls (Passive)
                        // Reaping (Passive)
                        // Unholy Might (Passive)
                        // Scourge Strike
                        // Shadow Infusion (Passive)
                        // Festering Strike
                        // Sudden Doom (Passive)
                        // Unholy Frenzy
                        // Ebon Plaguebringer (Passive)
                        // Dark Transformation
                        // Summon Gargoyle
                        // Improved Unholy Presence (Passive)
                        // Mastery: Mastery: Dreadblade (Passive)
                        // Soul Reaper
                        #endregion 
                        break;
                    }
            }
            #endregion

            #region Talents
            #region Level 15
            // Roiling Blood
            // Your Blood Boil ability now also triggers Pestilence if it strikes a diseased target.
            // Implemented in BB.

            // Plague Leech
            // Plague Leech
            // 30 yd range
            // Instant	25 sec cooldown
            // Draw forth the infection from an enemy, consuming your Blood Plague and Frost Fever diseases on the target to activate a random fully-depleted rune as a Death Rune.
            // TODO: Implement in Rotation.

            // Unholy Blight
            // Surrounds the Death Knight with a vile swarm of unholy insects for 10 sec, stinging all enemies within 10 yards every 1 sec, infecting them with Blood Plague and Frost Fever.
            // Implemented in as new Ability.
            #endregion
            #region Level 30
            // Lichborne
            // Draw upon unholy energy to become undead for 10 sec.  
            /// While undead, you are immune to Charm, Fear, and Sleep effects, and Death Coil will heal you.
            // TODO: Implement in TankDK only.

            // Anti-Magic Zone
            // Places a large, stationary Anti-Magic Zone that reduces spell damage done to party or raid members 
            // inside it by 75%.  The Anti-Magic Zone lasts for 10 sec or until it absorbs at least 136800+(($STR * 4)) spell damage.
            // TODO: Implement in TankDK only - another CD.

            // Purgatory
            // An unholy pacts grants you the ability to fight on through damage that would kill mere mortals. 
            // When you would sustain fatal damage, you instead are wrapped in a Shroud of Purgatory, absorbing 
            // incoming healing equal to the amount of damage prevented, lasting 3 sec.
            // If any healing absorption remains when Shroud of Purgatory expires, you die. Otherwise, you survive.  
            // This effect may only occur every 3 min.
            // TODO: Implement in TankDK - OMG this is sick!
            #endregion 
            #region Level 45
            // Death's Advance
            // You passively move 10% faster, and movement-impairing effects may not reduce you below 70% of normal movement speed.
            // When activated, you gain 30% movement speed and may not be slowed below 100% of normal movement speed for 6 seconds.
            FullCharacterStats.MovementSpeed += .1f;
            FullCharacterStats.AddSpecialEffect(_SE_DA);

            // Chilblains
            // Victims of your Frost Fever disease are Chilled, reducing movement speed by 50% for 10 sec, and your Chains of Ice immobilizes targets for 3 sec.
            // Implemented in FF & Chains of Ice

            // Asphyxiate
            // Lifts an enemy target off the ground and crushes their throat with dark energy, stunning them for 5 sec.  Functions as a silence if the target is immune to stuns.
            // Replaces Strangulate.
            // Currently no modeling planned.
            #endregion
            #region Level 60
            // Death Pact
            // Drain vitality from an undead minion, healing the Death Knight for 50% of his maximum health and causing the minion to suffer damage equal to 50% of its maximum health.
            // TODO: Implement in TankDK

            // Death Siphon
            // Deal 4612 to 5359 Shadowfrost damage to an enemy, healing the Death Knight for 100% of damage dealt.
            // TODO: Implement new ability.

            // Conversion
            // Continuously converts Runic Power to health, restoring 3% of maximum health every 1 sec. 
            // Only base Runic Power generation from spending runes may occur while Conversion is active. 
            // This effect lasts until canceled, or Runic Power is exhausted.
            // TODO: Implement in TankDK
            #endregion
            #region Level 75
            // Blood Tap
            // Each damaging Death Coil, Frost Strike, or Rune Strike generates 2 Blood Charges, 
            // up to a maximum of 12 charges.  Blood Tap consumes 5 Blood Charges to activate a random fully-depleted rune as a Death Rune.
            // TODO: Implement in Rotation.

            // Runic Empowerment
            // When you land a damaging Death Coil, Frost Strike, or Rune Strike, you have a 45% chance to activate a random fully-depleted rune.
            // Implemented in Rotation
	
            // Runic Corruption
            // When you land a damaging Death Coil, Frost Strike, or Rune Strike, you have a 45% chance to activate Runic Corruption, increasing your rune regeneration rate by 100% for 3 sec.
            // Implemented in Rotation
            #endregion
            #region Level 90
            // Gorefiend's Grasp
            // Shadowy tendrils coil around all enemies within 20 yards of a target (hostile or friendly), pulling them to the target's location.
	        // Will not model currently.

            // Remorseless Winter
            // Surrounds the Death Knight with a swirling tempest of frigid air for 8 sec, chilling enemies within 8 yards every 
            // 1 sec. Each pulse reduces targets' movement speed by 15% for 3 sec, stacking up to 5 times. Upon receiving a 
            // fifth application, an enemy will be stunned for 6 sec.
            // Will not model currently.
	
            // Desecrated Ground
            // Corrupts the ground in a 8 yard radius beneath the Death Knight for 10 sec. While standing in 
            // this corruption, the Death Knight is immune to effects that cause loss of control. This ability instantly 
            // removes such effects when activated.
            // Will not model currently.  
            #endregion
            #endregion
        }

        #region Custom Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] 
                        { 
                            "Stats Graph", 
                            "Scaling vs Strength",
                            "Scaling vs Crit Rating",
                            "Scaling vs Haste Rating",
                            "Scaling vs Mastery Rating",
                            "Presences",
                        };
                }
                return _customChartNames;
            }
        }
        
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSDK baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

            string[] statList = new string[] 
            {
                "Strength",
                "Agility",
                "Attack Power",
                "Crit Rating",
                "Hit Rating",
                "Expertise Rating",
                "Haste Rating",
                "Mastery Rating",
            };

            switch (chartName)
            {
                case "Presences":
                    {
                        string[] listPresence = EnumHelper.GetNames(typeof(Presence));

                        // Set this to have no presence enabled.
                        Character baseCharacter = character.Clone();
                        baseCharacter.IsLoading = true;
                        (baseCharacter.CalculationOptions as CalculationOptionsDPSDK).presence = Presence.None;
                        baseCharacter.IsLoading = false;
                        // replacing pre-factored base calc since this is different than the Item budget lists. 
                        baseCalc = GetCharacterCalculations(baseCharacter, null, true, false, false) as CharacterCalculationsDPSDK;

                        // Set these to have the key presence enabled.
                        for (int index = 0; index < listPresence.Length; index++)
                        {
                            (character.CalculationOptions as CalculationOptionsDPSDK).presence = (Presence)index;
                            
                            calc = GetCharacterCalculations(character, null, false, true, true) as CharacterCalculationsDPSDK;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = listPresence[index];
                            comparison.Equipped = false;
                            comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                        return comparisonList.ToArray();
                    }
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override System.Windows.Controls.Control GetCustomChartControl(string chartName)
        {
            switch (chartName)
            {
                case "Stats Graph":
                case "Scaling vs Strength":
                case "Scaling vs Crit Rating":
                case "Scaling vs Haste Rating":
                case "Scaling vs Mastery Rating":
                    return Graph.Instance;
                default:
                    return null;
            }
        }

        public override void UpdateCustomChartData(Character character, string chartName, System.Windows.Controls.Control control)
        {
            Color[] statColors = new Color[] { 
                Color.FromArgb(0xFF, 0xFF, 0, 0), 
                Color.FromArgb(0xFF, 0xFF, 165, 0), 
                Color.FromArgb(0xFF, 0x80, 0x80, 0x00), 
                Color.FromArgb(0xFF, 154, 205, 50), 
                Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF), 
                Color.FromArgb(0xFF, 0, 0, 0xFF), 
                Color.FromArgb(0xFF, 0x80, 0, 0xFF),
                Color.FromArgb(0xFF, 0, 0x80, 0xFF),
            };

            List<float> X = new List<float>();
            List<ComparisonCalculationBase[]> Y = new List<ComparisonCalculationBase[]>();

            float fMultiplier = 1;
            Stats[] statsList = new Stats[] {
                        new Stats() { Strength = fMultiplier },
                        new Stats() { Agility = fMultiplier },
                        new Stats() { AttackPower = fMultiplier },
                        new Stats() { CritRating = fMultiplier },
                        new Stats() { HitRating = fMultiplier },
                        new Stats() { ExpertiseRating = fMultiplier },
                        new Stats() { HasteRating = fMultiplier },
                        new Stats() { MasteryRating = fMultiplier },
                    };

            switch (chartName)
            {
                case "Stats Graph":
                    Graph.Instance.UpdateStatsGraph(character, statsList, statColors, 200, "", null);
                    break;
                case "Scaling vs Strength":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { Strength = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Crit Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { CritRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Haste Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { HasteRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Mastery Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { MasteryRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
            }
        }

        #endregion

        #region Relevant Stats?
        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand)
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            StatsDK s = new StatsDK()
            {
                // Core stats
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                ExpertiseRating = stats.ExpertiseRating,
                AttackPower = stats.AttackPower,
                MasteryRating = stats.MasteryRating,
                // Other Base Stats
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Resilience = stats.Resilience,

                // Secondary Stats
                Health = stats.Health,
                SpellHaste = stats.SpellHaste,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellPenetration = stats.SpellPenetration,

                // Dam stats
                WeaponDamage = stats.WeaponDamage,
                PhysicalDamage = stats.PhysicalDamage,
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,

                // Bonus to stat
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,

                // Bonus to Dam
                // *Dam
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                // +Dam
                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,
                BonusDamageScourgeStrike = stats.BonusDamageScourgeStrike,
                BonusDamageBloodStrike = stats.BonusDamageBloodStrike,
                BonusDamageDeathCoil = stats.BonusDamageDeathCoil, 
                BonusDamageDeathStrike =  stats.BonusDamageDeathStrike,
                BonusDamageFrostStrike = stats.BonusDamageFrostStrike,
                BonusDamageHeartStrike = stats.BonusDamageHeartStrike,
                BonusDamageIcyTouch = stats.BonusDamageIcyTouch,
                BonusDamageObliterate = stats.BonusDamageObliterate,
                // Crit
                BonusCritChanceDeathCoil = stats.BonusCritChanceDeathCoil,
                BonusCritChanceFrostStrike = stats.BonusCritChanceFrostStrike,
                BonusCritChanceObliterate = stats.BonusCritChanceObliterate,
                // Other
                CinderglacierProc = stats.CinderglacierProc,
                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                TargetArmorReduction = stats.TargetArmorReduction,
                MaxHealthDamageProc = stats.MaxHealthDamageProc,
                // BossHandler
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.MeleeAttack ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.PhysicalAttack ||
                        effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.DeathRuneGained ||
                        effect.Trigger == Trigger.Use)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool bRelevant = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.DeathRuneGained ||
                        effect.Trigger == Trigger.KillingMachine ||
                        effect.Trigger == Trigger.RuneStrikeHit
                    )
                { bRelevant = true; }
                else if (relevantStats(effect.Stats))
                {
                    if (
                        effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeAttack ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.PhysicalAttack ||
                        effect.Trigger == Trigger.Use
                        )
                    {
                        foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                        {
                            if (!bRelevant)
                                bRelevant = HasRelevantStats(e.Stats);
                        }
                        if (!bRelevant)
                            bRelevant = relevantStats(effect.Stats);
                        
                    }
                }
            }
            if (!bRelevant)
                bRelevant = relevantStats(stats);
            return bRelevant;
        }

        private bool relevantStats(Stats stats)
        {
            bool bResults = false;
            // Core stats
            bResults |= (stats.Strength != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.AttackPower != 0);

            // Other Base Stats
            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.MasteryRating != 0);
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.Armor != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.Resilience != 0);

            // Secondary Stats
            bResults |= (stats.Health != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
            bResults |= (stats.SpellCrit != 0);
            bResults |= (stats.SpellCritOnTarget != 0);
            bResults |= (stats.SpellHit != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.SpellPenetration != 0);

            // Dam stats
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.PhysicalDamage != 0);
            bResults |= (stats.ShadowDamage != 0);
            bResults |= (stats.ArcaneDamage != 0);
            bResults |= (stats.FireDamage != 0);
            bResults |= (stats.FrostDamage) != 0;
            bResults |= (stats.HolyDamage) != 0;
            bResults |= (stats.NatureDamage) != 0;

            // Bonus to stat
            bResults |= (stats.BonusHealthMultiplier != 0);
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= ( stats.BonusStaminaMultiplier != 0);
            bResults |= ( stats.BonusAgilityMultiplier != 0);
            bResults |= ( stats.BonusCritDamageMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);

            // Bonus to Dam
            // *Dam
            bResults |= (stats.BonusWhiteDamageMultiplier != 0);
            bResults |= (stats.BonusDamageMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= ( stats.BonusShadowDamageMultiplier != 0);
            bResults |= ( stats.BonusFrostDamageMultiplier != 0);
            bResults |= ( stats.BonusDiseaseDamageMultiplier  != 0);
            // +Dam
            bResults |= (stats.BonusFrostWeaponDamage != 0);
            bResults |= (stats.BonusDamageScourgeStrike != 0);
            bResults |= (stats.BonusDamageBloodStrike != 0);
            bResults |= ( stats.BonusDamageDeathCoil != 0); 
            bResults |= ( stats.BonusDamageDeathStrike != 0);  
            bResults |= ( stats.BonusDamageFrostStrike   != 0); 
            bResults |= ( stats.BonusDamageHeartStrike != 0);  
            bResults |= ( stats.BonusDamageIcyTouch != 0);  
            bResults |= ( stats.BonusDamageObliterate != 0);
            // Crit
            bResults |= (stats.BonusCritDamageMultiplier != 0);
            bResults |= (stats.BonusCritChanceDeathCoil != 0);
            bResults |= ( stats.BonusCritChanceFrostStrike != 0); 
            bResults |= ( stats.BonusCritChanceObliterate != 0); 
            // Other
            bResults |= ( stats.CinderglacierProc != 0); 
            bResults |= ( stats.HighestStat != 0);
            bResults |= ( stats.HighestSecondaryStat != 0); 
            bResults |= ( stats.Paragon != 0); 
            bResults |= (stats.ThreatIncreaseMultiplier != 0);
            bResults |= (stats.ThreatReductionMultiplier != 0);
            bResults |= (stats.TargetArmorReduction != 0);
            bResults |= (stats.MaxHealthDamageProc != 0);
            // BossHandler
            bResults |= (stats.SnareRootDurReduc != 0); 
            bResults |= (stats.FearDurReduc != 0); 
            bResults |= (stats.StunDurReduc != 0); 
            bResults |= (stats.MovementSpeed != 0); 

            bResults |= !(HasIgnoreStats(stats));
            return bResults;
        }

        private bool HasIgnoreStats(Stats stats)
        {
            if (!HidingBadStuff) { return false; }
            bool retVal = false;
            retVal = (
                // Remove Spellcasting Stuff
                (HidingBadStuff_Spl ? stats.Mp5 + stats.SpellPower + stats.Mana + stats.ManaRestore + stats.Spirit + stats.Intellect
                                    + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier
                                    + stats.SpellPenetration + stats.BonusManaMultiplier
                                    : 0f)
                // Remove Defensive Stuff (until we do that special modeling)
                + (HidingBadStuff_Def ? stats.Dodge + stats.Parry
                                      + stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.Block
                                      + stats.SpellReflectChance
                                      : 0f)
                // Remove PvP Items
                + (HidingBadStuff_PvP ? stats.Resilience : 0f)
                ) > 0;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                //if (RelevantTriggers.Contains(effect.Trigger)) 
                //retVal |= !RelevantTriggers.Contains(effect.Trigger);
                retVal |= HasIgnoreStats(effect.Stats);
                if (retVal) break;
                //}
            }

            return retVal;
        }
        #endregion

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Crit Rating",
                        "Expertise Rating",
                        "Hit Rating",
                        "Haste Rating",
                        "Mastery",
                        "Target Miss %",
                        "Target Dodge %",
                        "Resilience",
                        "Spell Penetration"
                    };

                return _optimizableCalculationLabels;
            }
        }
        public override void SetDefaults(Character character)
        {
            // Need to be behind boss
            character.BossOptions.InBack = true;
            character.BossOptions.InBackPerc_Melee = 1.00d;
        }

        public static void RemoveDuplicateRunes(Stats statsBaseGear, Character character, bool bDW)
        {
            if (bDW // Check for DW.
                && (character.MainHandEnchant != null && character.OffHandEnchant != null) // Check that both weapons have enchants.
                && character.MainHandEnchant == character.OffHandEnchant) // check that we have duplicate enchants.
            {
                bool bEffect1Found = false;
                bool bEffect2Found = false;
                switch (character.MainHandEnchant.Id)
                {
                    case 3368: // FC
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_FC1))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (bEffect2Found && se1.Equals(_SE_FC2))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (se1.Equals(_SE_FC1))
                                bEffect1Found = true;
                            else if (se1.Equals(_SE_FC2))
                                bEffect2Found = true;
                        }
                        break;
                    case 3369: // Cinder
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_CG))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (se1.Equals(_SE_CG))
                                bEffect1Found = true;
                        }
                        break;
                    case 3370: // RazorIce
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_RI))
                            {
                                statsBaseGear.BonusFrostWeaponDamage -= .02f;
                                statsBaseGear.RemoveSpecialEffect(se1);
                            }
                            else if (se1.Equals(_SE_RI))
                                bEffect1Found = true;
                        }
                        break;
                }

            }
        }

        #region Static SpecialEffects
        // Enchant: Rune of Fallen Crusader
        public static readonly SpecialEffect _SE_FC1 = new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1, false);
        public static readonly SpecialEffect _SE_FC2 = new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1, false);
        // Enchant: Rune of Razorice
        public static readonly SpecialEffect _SE_RI = new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusFrostDamageMultiplier = 0.02f }, 20f, 0f, 1f, 5);
        // Enchant: Rune of Cinderglacier
        public static readonly SpecialEffect _SE_CG = new SpecialEffect(Trigger.DamageDone, new Stats() { CinderglacierProc = 2f }, 0f, 0f, -1.5f);
        // Icebound Fort
        private static readonly SpecialEffect[] _SE_IBF = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f }, 12, 3 * 60  ), // Default IBF
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f + .30f }, 12, 3 * 60  ), // IBF in Blood Spec.
        };
        public static readonly SpecialEffect[] _SE_VampiricBlood = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() {HealingReceivedMultiplier = .25f, BonusHealthMultiplier = .15f}, 10, 60f), // No Glyph
            new SpecialEffect(Trigger.Use, new Stats() {HealingReceivedMultiplier = .25f + .15f}, 10, 60f) // Glyphed
        };
        // Talent: Rune Tap
        public static readonly SpecialEffect _SE_RuneTap = 
            new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .1f }, 0, 30f);

        /// <summary>
        /// When a damaging attack brings you below 30% of your maximum health, the cooldown on your Rune Tap
        /// ability is refreshed and your next Rune Tap has no cost, and all damage taken is reduced by [25/3*Pts]%
        /// for 8 sec. This effect cannot occur more than once every 45 seconds.
        /// </summary>
        public static readonly SpecialEffect _SE_WillOfTheNecropolis = 
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenReductionMultiplier = 0.25f }, 8, 45, 0.30f);

        public static readonly SpecialEffect[][] _SE_UnbreakableArmor = new SpecialEffect[][] {
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 0 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 0 * 10f),
            },
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 1 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 1 * 10f),
            },
        };
        public static readonly SpecialEffect _SE_AntiMagicZone = new SpecialEffect(Trigger.Use, new Stats() { SpellDamageTakenReductionMultiplier = 0.75f }, 10f, 2f * 60f);

        public static readonly SpecialEffect _SE_PillarOfFrost = new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = .2f }, 20f, 60);
        public static readonly SpecialEffect[] _SE_DRW = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 1f, Parry = .20f }, 12f, 1.5f * 60f), // Normal
            new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.75f, Parry = .20f, ThreatIncreaseMultiplier = 1f }, 12f, 1.5f * 60f), // Glyphed
        };
        public static readonly SpecialEffect _SE_4T13_RC = new SpecialEffect(Trigger.RunicCorruption, new Stats() { MasteryRating = 710 }, 12f, 0, .4f);
        public static readonly SpecialEffect _SE_4T13_RE = new SpecialEffect(Trigger.RunicEmpowerment, new Stats() { MasteryRating = 710 }, 12f, 0, .25f);

        public static readonly SpecialEffect _SE_UnholyFrenzy = new SpecialEffect(Trigger.Use, new Stats() { PhysicalHaste = 0.2f }, 30f, 3 * 60f);

        public static readonly SpecialEffect _SE_DA = new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = .3f }, 6f, 30f);

        #endregion
    }
}
