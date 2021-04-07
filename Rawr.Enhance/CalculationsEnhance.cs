using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Media;

namespace Rawr.Enhance
{
    [Rawr.Calculations.RawrModelInfo("Enhance", "inv_jewelry_talisman_04", CharacterClass.Shaman)]
    public class CalculationsEnhance : CalculationsBase
    {
        #region Gemming Templates
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Meta
                int agile = 76884;
                int fleet = 76887;

                if (_defaultGemmingTemplates == null)
                {
                    Gemming gemming = new Gemming();
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, agile, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, agile, true));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, agile, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Jewelcrafter", 3, agile, false));

                    _defaultGemmingTemplates.AddRange(gemming.addCogwheelTemplates("Engineer", 0, agile, false));
                }
                return _defaultGemmingTemplates;
            }
        }
        #endregion

        #region Display Labels
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[]
                    {
                        #region Summary
                        "Summary:DPS Points*Your total expected DPS with this kit and selected glyphs and buffs",
                        "Summary:Survivability Points*Assumes basic 2% of total health as Survivability",
                        "Summary:Overall Points*This is the sum of Total DPS and Survivability. If you want sort items by DPS only select DPS from the sort dropdown top right",
                        #endregion
                        #region Basic Stats
                        "Base Stats:Health",
                        "Base Stats:Mana",
                        "Base Stats:Strength",
                        "Base Stats:Agility",
                        "Base Stats:Stamina",
                        "Base Stats:Intellect",
                        "Base Stats:Spirit",
                        "Base Stats:Mastery",

                        //"Melee:Damage",
                        //"Melee:DPS",
                        "Melee:Attack Power",
                        //"Melee:Speed",
                        "Melee:Melee Haste",
                        "Melee:Melee Hit",
                        "Melee:Melee Crit",
                        "Melee:Expertise",

                        "Spell:Spell Power",
                        "Spell:Spell Haste",
                        "Spell:Spell Hit",
                        "Spell:Spell Crit",
                        "Spell:Combat Regen",
                        #endregion
                        #region Complex Stats
                        "Combat Stats:Avg Speed",
                        "Combat Stats:Avg Combat Regen*Includes Primal Wisdon regen",

                        "Complex Stats:Avoided Attacks*The percentage of your attacks that fail to land.",
                        "Complex Stats:Armor Mitigation",
                        "Complex Stats:Flurry Uptime",
                        "Complex Stats:ED Uptime*Elemental Devastation Uptime percentage",
                        "Complex Stats:Avg Time to 5 Stack*Average time it takes to get 5 stacks of Maelstrom Weapon.",
                        "Complex Stats:MH Enchant Uptime",
                        "Complex Stats:OH Enchant Uptime",
                        "Complex Stats:Trinket 1 Uptime",
                        "Complex Stats:Trinket 2 Uptime",
                        "Complex Stats:Fire Totem Uptime",
                        #endregion
                        #region Attacks Breakdown
                        "Attacks:White Damage",
                        "Attacks:Windfury Attack",
                        "Attacks:Flametongue Attack",
                        "Attacks:Stormstrike",
                        "Attacks:Lava Lash",
                        "Attacks:Searing/Magma Totem",
                        "Attacks:Earth Shock",
                        "Attacks:Flame Shock",
                        "Attacks:Lightning Bolt",
                        "Attacks:Unleash Wind",
                        "Attacks:Unleash Flame",
                        "Attacks:Lightning Shield",
                        "Attacks:Chain Lightning",
                        "Attacks:Fire Nova",
                        "Attacks:Fire Elemental",
                        "Attacks:Spirit Wolf",
                        "Attacks:Magic Other",
                        "Attacks:Physical Other",
                        "Attacks:Total DPS",
                        #endregion
                    };
                return _characterDisplayCalculationLabels;
            }
        }
        #endregion

        #region Overrides
        public override CharacterClass TargetClass { get { return CharacterClass.Shaman; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationEnhance(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsEnhance(); }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelEnhance();
                }
                return _calculationOptionsPanel;
            }
        }
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsEnhance calcOpts = serializer.Deserialize(reader) as CalculationOptionsEnhance;
            return calcOpts;
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[]
                    {
                        "% Chance to Miss (White)",
                        "% Chance to Miss (Yellow)",
                        "% Chance to Miss (Spell)",
                        "% Chance to be Dodged",
                        "Health"
                    };
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Combat Table (White)",
                    "Combat Table (Yellow)",
                    "Combat Table (Spell)",
                    "Relative Gem Values",
                    "MH Weapon Speeds",
                    "OH Weapon Speeds",
                    };
                return _customChartNames;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 160, 0, 224));  // 0xFFA000E0
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));  // 0xFF408020
                }
                return _subPointNameColors;
            }
        }
        #endregion

        #region Main Calculations
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
            CharacterCalculationsEnhance calc = new CharacterCalculationsEnhance();
            if (character == null) { return calc; }
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            if (calcOpts == null) { return calc; }
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) { bossOpts = new BossOptions(); }
            
            #region Applied Stats
            StatsEnhance stats = GetCharacterStats(character, additionalItem, true);
            calc.BasicStats = stats;
            calc.EnhSimStats = GetCharacterStats(character, additionalItem, false);
            //calc.ActiveBuffs = new List<Buff>(character.ActiveBuffs);

            float windfuryWeaponBonus = 5090f;  //WFAP
            float flametongueMultiplier = 0.00f;
            if (calcOpts.OffhandImbue == "Flametongue" || calcOpts.MainhandImbue == "Flametongue")
            {
                flametongueMultiplier = 0.07f;
            }
            #endregion

            #region Damage Model
            ////////////////////////////
            // Main calculation Block //
            ////////////////////////////

            CombatStats cs = new CombatStats(character, stats, calcOpts, bossOpts); // calculate the combat stats using modified stats

            // assign basic variables for calcs
            float attackPower = stats.AttackPower;
            float spellPower = stats.SpellPower;
            float mastery = 1f + ((stats.Mastery + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.025f); // Check this!!!!
            float wdpsMH = character.MainHand == null ? 46.3f : (stats.WeaponDamage + (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f) / character.MainHand.Speed;
            float wdpsOH = character.OffHand == null ? 46.3f : (stats.WeaponDamage + (character.OffHand.MinDamage + character.OffHand.MaxDamage) / 2f) / character.OffHand.Speed;
            float bonusPhysicalDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusPhysicalDamageMultiplier);
            float bonusFrostDamage    = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFrostDamageMultiplier)  * (1f + flametongueMultiplier);
            float bonusFireDamage     = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFireDamageMultiplier)   * (1f + flametongueMultiplier);
            float bonusNatureDamage   = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier) * (1f + flametongueMultiplier);
            #endregion

            #region Individual DPS
            #region Melee DPS
            float APDPS = (attackPower / 14f);
            float adjustedMHDPS = (wdpsMH + APDPS);
            float adjustedOHDPS = 0f;
            float dpsOHMeleeTotal = 0f;
            float dpsMoteOfAnger = 0f;

            float dpsMHMeleeNormal = adjustedMHDPS * cs.NormalHitModifierMH;
            float dpsMHMeleeCrits = adjustedMHDPS * cs.CritHitModifierMH;
            float dpsMHMeleeGlances = adjustedMHDPS * cs.GlancingHitModifier;
            float meleeMultipliers = cs.DamageReduction * bonusPhysicalDamage * (1f + stats.BonusWhiteDamageMultiplier);

            float dpsMHMeleeTotal = ((dpsMHMeleeNormal + dpsMHMeleeCrits + dpsMHMeleeGlances) * cs.UnhastedMHSpeed / cs.HastedMHSpeed) * meleeMultipliers;

            if (cs.HastedOHSpeed != 0)
            {
                adjustedOHDPS = (wdpsOH + APDPS) * .5f;
                float dpsOHMeleeNormal = adjustedOHDPS * cs.NormalHitModifierOH;
                float dpsOHMeleeCrits = adjustedOHDPS * cs.CritHitModifierOH;
                float dpsOHMeleeGlances = adjustedOHDPS * cs.GlancingHitModifier;
                dpsOHMeleeTotal = ((dpsOHMeleeNormal + dpsOHMeleeCrits + dpsOHMeleeGlances) * cs.UnhastedOHSpeed / cs.HastedOHSpeed) * meleeMultipliers;
            }

            // Generic MH & OH damage values used for SS, LL & WF
            float damageMHSwing = adjustedMHDPS * cs.UnhastedMHSpeed;
            float damageOHSwing = adjustedOHDPS * cs.UnhastedOHSpeed;

            if (cs.HastedOHSpeed != 0)
                dpsMoteOfAnger = (damageMHSwing + damageOHSwing) / 2 * stats.MoteOfAnger;
            else
                dpsMoteOfAnger = damageMHSwing * stats.MoteOfAnger;

            float dpsMelee = dpsMHMeleeTotal + dpsOHMeleeTotal + dpsMoteOfAnger;
            #endregion

            #region Stormstrike DPS
            float dpsSS = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.StormStrike) && character.MainHand != null)
            {
                float swingDPSMH = damageMHSwing * 1.5f * cs.HitsPerSMHSS;
                float swingDPSOH = damageOHSwing * 1.5f * cs.HitsPerSOHSS;
                float SSnormal = (swingDPSMH * cs.YellowHitModifierMH) + (swingDPSOH * cs.YellowHitModifierOH);
                float SScrit = ((swingDPSMH * cs.YellowCritModifierMH) + (swingDPSOH * cs.YellowCritModifierOH)) * cs.CritMultiplierMelee;
                dpsSS = (SSnormal + SScrit) * cs.DamageReduction * (1f + stats.BonusStormstrikeDamageMultiplier);
            }
            #endregion

            #region Lavalash DPS
            /* Taken from EnhSim (thank you ziff)
               Damage = bwd * llb * (1.0 + sfs * (sfb + t12-2p) + ftb) * (1.0 + llg + t11-2p) * fdm

               bwd    - Base weapon damage against a target with no armor
               llb    - Lava Lash Bonus, this is the default lava lash damage bonus of 2.0
               sfs    - # of current stacks of Searing Flames
               sfb    - the searing flames bonus from Improved Lava Lash.  Maxed out it is .20
               t12-2p - the 2 piece bonus from T12, which is currently an extra 0.05 per searing flame stack
               ftb    - the bonus if Flametongue weapon is on your off-hand, which is 0.40
               llg    - the bonus from Lava Lash glyph, which is 0.20
               t11-2p - the 2 piece bonus from T11, which is 0.10
               fdm    - the fire damage multiplier, which includes the mastery bonus, elemental precision,
                        buffs, debuff, etc.  These multipliers are always multiplied to each other, and not
                        done additively.
             */
            float dpsLL = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LavaLash) && character.OffHand != null)
            {
                float searingFlames = 0f;
                float flametongue = 0f;
                float glyphLL = 0f;
                if (calcOpts.PriorityInUse(EnhanceAbility.SearingTotem))
                {
                    searingFlames = (0.2f + stats.BonusSearingFlamesModifier) * 5f; //5f = number of stacks of searing flames (takes app. 8.25s to hit 5 stacks, LL CD is 10s).
                }
                if (calcOpts.OffhandImbue == "Flametongue")
                {
                    flametongue = .4f;
                }
                /*if (character.ShamanTalents.GlyphofLavaLash)
                {
                    glyphLL = .2f;
                }*/
                float lavalashDPS = damageOHSwing * 1.3f * cs.HitsPerSLL;
                float LLnormal = lavalashDPS * cs.YellowHitModifierOH;
                float LLcrit = lavalashDPS * cs.YellowCritModifierOH * cs.CritMultiplierMelee;
                dpsLL = (LLnormal + LLcrit) * (1f + searingFlames + flametongue) * (1f + glyphLL + stats.T11BonusLavaLashDamageMultiplier) * mastery * bonusFireDamage;
            }
            #endregion

            #region Earth Shock DPS
            float dpsES = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.EarthShock))
            {
                float damageESBase = 1030f;
                float coefES = .386f;
                float damageES = (1f + stats.ConcussionMultiplier) * (damageESBase + coefES * spellPower);
                float shockdps = damageES / cs.AbilityCooldown(EnhanceAbility.EarthShock);
                float shockNormal = shockdps * cs.NatureSpellHitModifier;
                float shockCrit = shockdps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsES = (shockNormal + shockCrit) * mastery * bonusNatureDamage;
            }
            #endregion

            #region Flame Shock DPS
            float dpsFS = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.FlameShock))
            {
                float FSBaseNumTick = 18f / 3f;
                float damageFSBase = 586f;
                float damageFSDoTTickBase = 1320f / FSBaseNumTick;
                float FSNumTick = cs.AverageFSDotTime / cs.AverageFSTickTime;
                float coefFS = 1.5f / 3.5f / 2f;
                float coefFSDoT = .6f;
                float damageFS = (damageFSBase + coefFS * spellPower) * (1f + stats.ConcussionMultiplier);
                float damageFTDoT = ((damageFSDoTTickBase * FSNumTick) + coefFSDoT * spellPower) * (1f + stats.ConcussionMultiplier);
                float usesCooldown = cs.AbilityCooldown(EnhanceAbility.FlameShock);
                float flameShockdps = damageFS / usesCooldown;
                float flameShockDoTdps = damageFTDoT / usesCooldown;
                float flameShockNormal = (flameShockdps + flameShockDoTdps) * cs.SpellHitModifier;
                float flameShockCrit = (flameShockdps + flameShockDoTdps) * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFS = (flameShockNormal + flameShockCrit) * mastery * bonusFireDamage;
            }
            #endregion

            #region Lightning Bolt DPS
            float dpsLB = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LightningBolt))
            {
                float damageLBBase = 921f;
                float coefLB = .714f;
                float damageLB = (1f + stats.ConcussionMultiplier) * (damageLBBase + coefLB * spellPower);
                float lbdps = damageLB / cs.AbilityCooldown(EnhanceAbility.LightningBolt);
                float lbNormal = lbdps * cs.NatureSpellHitModifier;
                float lbCrit = lbdps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsLB = (lbNormal + lbCrit) * (1f + stats.BonusLightningBoltDamageMultiplier) * mastery * bonusNatureDamage;
            }
            #endregion

            #region Chain Lightning DPS
            float dpsCL = 0f;
            float coefCL = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.ChainLightning))
            {
                if (character.ShamanTalents.GlyphofChainLightning)
                {
                    coefCL = 0.5714f * 0.9f;
                }
                else
                {
                    coefCL = 0.5714f;
                }
                float damageCLBase = 1230f;
                float damageCL = (1f + stats.ConcussionMultiplier) * (damageCLBase + coefCL * spellPower);
                float cldps = (damageCL) / cs.AbilityCooldown(EnhanceAbility.ChainLightning);
                float clNormal = cldps * cs.NatureSpellHitModifier;
                float clCrit = cldps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsCL = (clNormal + clCrit) * mastery * bonusNatureDamage;
            }
            #endregion

            #region Windfury DPS
            float dpsWF = 0f;
            if (calcOpts.MainhandImbue == "Windfury" && character.MainHand != null)
            {
                float damageWFHit = damageMHSwing + (windfuryWeaponBonus / 14 * cs.UnhastedMHSpeed);
                float WFdps = damageWFHit * cs.HitsPerSWF;
                float WFnormal = WFdps * cs.YellowHitModifierMH;
                float WFcrit = WFdps * cs.YellowCritModifierMH * cs.CritMultiplierMelee;
                dpsWF = (WFnormal + WFcrit) * cs.DamageReduction * bonusPhysicalDamage * (1f + stats.BonusWindfuryDamageMultiplier);
            }
            #endregion

            #region Lightning Shield DPS
            float dpsLS = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LightningShield))
            {
                float damageLSBase = 451f;
                float damageLSCoef = 0.267f; // co-efficient from EnhSim
                float damageLS = (1f + stats.ShieldBonus) * (damageLSBase + damageLSCoef * spellPower);
                float lsdps = damageLS * cs.StaticShockProcsPerS;
                float lsNormal = lsdps * cs.NatureSpellHitModifier;
                float lsCrit = lsdps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsLS = (lsNormal + lsCrit) * mastery * bonusNatureDamage;
            }
            #endregion

            #region Fire Totem DPS
            float dpsFireTotem = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.MagmaTotem))
            {
                float damageFireTotem = (289f + .067f * spellPower) * (1f + stats.CallofFlameBonus);
                float FireTotemdps = damageFireTotem / 2f * cs.FireTotemUptime;
                float FireTotemNormal = FireTotemdps * cs.SpellHitModifier;
                float FireTotemCrit = FireTotemdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireTotem = (FireTotemNormal + FireTotemCrit) * mastery * bonusFireDamage * cs.MultiTargetMultiplier;
            }
            else if (calcOpts.PriorityInUse(EnhanceAbility.SearingTotem))
            {
                float damageFireTotem = (129f + .1669f * spellPower) * (1f + stats.CallofFlameBonus);
                float FireTotemdps = damageFireTotem / 1.65f * cs.FireTotemUptime;
                float FireTotemNormal = FireTotemdps * cs.SpellHitModifier;
                float FireTotemCrit = FireTotemdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireTotem = (FireTotemNormal + FireTotemCrit) * mastery * bonusFireDamage;
            }
            dpsFireTotem *= (1f - cs.FireElementalUptime);
            #endregion

            #region Fire Nova DPS
            float dpsFireNova = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.FireNova) && calcOpts.PriorityInUse(EnhanceAbility.FlameShock))
            {
                float damageFireNova = (844f + 0.143f * spellPower) * stats.CallofFlameBonus;
                float FireNovadps = (damageFireNova / cs.AbilityCooldown(EnhanceAbility.FireNova));
                float FireNovaNormal = FireNovadps * cs.SpellHitModifier;
                float FireNovaCrit = FireNovadps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireNova = (FireNovaNormal + FireNovaCrit) * mastery * bonusFireDamage * cs.MultiTargetMultiplier;
            }
            #endregion

            #region Flametongue Weapon DPS
            float dpsFT = 0f;
            /*if (calcOpts.MainhandImbue == "Flametongue")
            {
                float damageFTBase = 306f;
                float damageFTCoef = 0.1253f;
                float damageFT = (damageFTBase + (damageFTCoef * attackPower)) * cs.UnhastedOHSpeed / 4.0f;
                float FTdps = damageFT * (cs.HitsPerSOH - cs.HitsPerSLL);
                float FTNormal = FTdps * cs.SpellHitModifier;
                float FTCrit = FTdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFT += (FTNormal + FTCrit) * mastery * bonusFireDamage;
            }*/
            if (calcOpts.OffhandImbue == "Flametongue" && character.OffHand != null)
            {
                float damageFTBase = 306f;
                float damageFTCoef = 0.1253f;
                float damageFT = (damageFTBase + (damageFTCoef * attackPower)) * cs.UnhastedOHSpeed / 4.0f;
                float FTdps = damageFT * (cs.HitsPerSOH - cs.HitsPerSLL);
                float FTNormal = FTdps * cs.SpellHitModifier;
                float FTCrit = FTdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFT += (FTNormal + FTCrit) * mastery * bonusFireDamage;
            }
            #endregion

            #region Unleash Elements DPS
            #region Unleash Windfury
            float dpsUW = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.UnleashElements) && calcOpts.MainhandImbue == "Windfury" && character.MainHand != null)
            {
                float damageUWHit = damageMHSwing * 1.75f;
                float UWdps = damageUWHit / cs.AbilityCooldown(EnhanceAbility.UnleashElements);
                float UWnormal = UWdps * cs.YellowCritModifierMH;
                float UWcrit = UWdps * cs.YellowCritModifierMH * cs.CritMultiplierMelee;
                dpsUW = (UWnormal + UWcrit) * cs.DamageReduction * bonusPhysicalDamage;
            }
            #endregion
            #region Unleash Flametongue
            float dpsUF = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.UnleashElements) && calcOpts.OffhandImbue == "Flametongue" && character.OffHand != null)
            {
                float damageUFBase = 1070f;
                float damageUFCoef = 0.43f;
                float damageUF = damageUFBase + damageUFCoef * spellPower;
                float UFdps = damageUF / cs.AbilityCooldown(EnhanceAbility.UnleashElements);
                float UFnormal = UFdps * cs.SpellHitModifier;
                float UFcrit = UFdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsUF = (UFnormal + UFcrit) * mastery * bonusFireDamage;
            }
            #endregion
            #endregion

            #region Other (Damage Procs)
            float dpsMagic = 0f;
            float dpsPhysical = 0f;

            // This section needs major fixing
            // Some procs scale with AP/SP (these have an AP/SP scale factor) and/or Mastery (which is one a per proc basis), while others don't
            dpsMagic += (stats.ArcaneDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.ArcaneDamage * cs.NatureSpellHitModifier);
            dpsMagic += (stats.ShadowDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.ShadowDamage * cs.NatureSpellHitModifier);
            dpsMagic += (stats.HolyDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.HolyDamage * cs.NatureSpellHitModifier);
            dpsMagic += ((stats.FireDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.FireDamage* cs.NatureSpellHitModifier)) * mastery;
            dpsMagic += ((stats.FrostDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.FrostDamage * cs.NatureSpellHitModifier)) * mastery;
            dpsMagic += ((stats.NatureDamage * cs.NatureSpellCritModifier * cs.CritMultiplierSpell) + (stats.NatureDamage * cs.NatureSpellHitModifier)) * mastery;

            dpsPhysical += (stats.PhysicalDamage * cs.CritMultiplierMelee * cs.YellowCritModifierMH) + (stats.PhysicalDamage * cs.YellowHitModifierMH);
            #endregion

            #region Pet calculations
            // needed for pets - spirit wolves and Fire Elemental
            bool critBuff = character.ActiveBuffsContains("Leader of the Pack") ||
                            character.ActiveBuffsContains("Trueshot Aura (Critical Strike)") ||
                            character.ActiveBuffsContains("Arcane Brilliance (Critical Strike)");
            float critbuffs = (critBuff ? 0.05f : 0f);
            float meleeHitBonus = stats.PhysicalHit + StatConversion.GetHitFromRating(stats.HitRating)/* + dualWieldSpecialization*/;
            float petMeleeMissRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[bossOpts.Level - character.Level] - meleeHitBonus) + cs.AverageDodge;
            float petMeleeMultipliers = cs.DamageReduction * bonusPhysicalDamage;
            #endregion

            #region Doggies!
            // TTT article suggests 300-450 dps while the dogs are up plus 30% of AP
            // my analysis reveals they get 31% of shaman AP + 2 * their STR and base 206.17 dps.
            float dpsDogs = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.FeralSpirits))
            {
                float FSglyphAP = character.ShamanTalents.GlyphofFeralSpirit ? attackPower * .3f : 0f;
                float soeBuff = (character.ActiveBuffsContains("Strength of Earth Totem") || character.ActiveBuffsContains("Horn of Winter") || character.ActiveBuffsContains("Roar of Courage") ||
                    character.ActiveBuffsContains("Battle Shout")) ? 594f : 0f;
                float dogsStr = 331f + soeBuff;
                float dogsAgi = 113f + soeBuff;
                float dogsAP = ((dogsStr * 2f - 20f) + .31f * attackPower + FSglyphAP);
                float dogsCrit = (StatConversion.GetCritFromAgility(dogsAgi, CharacterClass.Shaman) + critbuffs) * (1 + stats.BonusCritDamageMultiplier);
                float dogsBaseSpeed = 1.5f;
                float dogsHitsPerS = 1f / (dogsBaseSpeed / (1f + stats.PhysicalHaste));
                float dogsBaseDamage = (490.06f + dogsAP / 14f) * dogsBaseSpeed;

                float dogsMeleeNormal = dogsBaseDamage * (1 - petMeleeMissRate - dogsCrit - cs.GlancingRate);
                float dogsMeleeCrits = dogsBaseDamage * dogsCrit * cs.CritMultiplierMelee;
                float dogsMeleeGlances = dogsBaseDamage * cs.GlancingHitModifier;

                float dogsTotalDamage = dogsMeleeNormal + dogsMeleeCrits + dogsMeleeGlances;

                dpsDogs = 2 * (30f / 120f) * dogsTotalDamage * dogsHitsPerS * petMeleeMultipliers;
                calc.SpiritWolf = new DPSAnalysis(dpsDogs, petMeleeMissRate, cs.AverageDodge, cs.GlancingRate, dogsCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FeralSpirits));
            }
            else
            {
                calc.SpiritWolf = new DPSAnalysis(0, 0, 0, 0, 0, 0);
            }
            #endregion

            #region Fire Elemental
            if (calcOpts.PriorityInUse(EnhanceAbility.FireElemental))
            {
                float spellHitBonus = stats.SpellHit + StatConversion.GetHitFromRating(stats.HitRating);
                float petSpellMissRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[bossOpts.Level - character.Level] - spellHitBonus);
                float petSpellMultipliers = bonusFireDamage * stats.CallofFlameBonus;
                float petCritRate = critbuffs * (1 + stats.BonusCritDamageMultiplier);
                calc.FireElemental = new FireElemental(attackPower, spellPower, stats.Intellect, cs, 
                        petCritRate, petMeleeMissRate, petMeleeMultipliers, petSpellMissRate, petSpellMultipliers);
            }
            else
                calc.FireElemental = new FireElemental(0, 0, 0, cs, 0, 0, 0, 0, 0);
            float dpsFireElemental = calc.FireElemental.getDPS();
            #endregion
            #endregion

            #region Set CalculatedStats
            calc.DPS = dpsMelee + dpsSS + dpsLL + dpsES + dpsFS + dpsLB + dpsCL + dpsWF + dpsLS + dpsFireTotem + dpsFireNova + dpsFT + dpsDogs + dpsFireElemental + dpsMagic + dpsPhysical;
            calc.Survivability = stats.Health * 0.02f;
            calc.OverallPoints = calc.DPS + calc.Survivability;
            calc.DodgedAttacks = cs.AverageDodge * 100f;
            calc.ParriedAttacks = cs.AverageParry * 100f;
            calc.MissedAttacks = (1 - cs.AverageWhiteHitChance) * 100f;
            calc.AvoidedAttacks = calc.MissedAttacks + calc.DodgedAttacks + calc.ParriedAttacks;
            calc.YellowHit = (float)Math.Floor((float)(cs.AverageYellowHitChance * 10000f)) / 100f;
            calc.SpellHit = (float)Math.Floor((float)(cs.ChanceSpellHit * 10000f)) / 100f;
            calc.ElemPrecMod = cs.ElemPrecMod;
            calc.DraeneiHitBonus = character.Race == CharacterRace.Draenei ? 0.01f : 0.00f;
            calc.SpecializationHitBonus = 0.06f;
            calc.OverSpellHitCap = (float)Math.Floor((float)(cs.OverSpellHitCap * 10000f)) / 100f;
            calc.OverMeleeCritCap = (float)Math.Floor((float)(cs.OverMeleeCritCap * 10000f)) / 100f;
            calc.WhiteHit = (float)Math.Floor((float)(cs.AverageWhiteHitChance * 10000f)) / 100f;
            calc.MeleeCrit = (float)Math.Floor((float)((cs.DisplayMeleeCrit)) * 10000f) / 100f;
            calc.YellowCrit = (float)Math.Floor((float)((cs.DisplayYellowCrit)) * 10000f) / 100f;
            calc.SpellCrit = (float)Math.Floor((float)(cs.ChanceSpellCrit * 10000f)) / 100f;
            calc.GlancingBlows = cs.GlancingRate * 100f;
            calc.ArmorMitigation = (1f - cs.DamageReduction) * 100f;
            calc.MasteryRating = stats.MasteryRating;
            calc.AttackPower = attackPower;
            calc.SpellPower = spellPower;
            calc.AvMHSpeed = cs.HastedMHSpeed;
            calc.AvOHSpeed = cs.HastedOHSpeed;
            calc.EDBonusCrit = cs.EDBonusCrit * 100f;
            calc.EDUptime = cs.EDUptime * 100f;
            calc.FlurryUptime = cs.FlurryUptime * 100f;
            calc.SecondsTo5Stack = cs.SecondsToFiveStack;
            calc.MHEnchantUptime = 0f;//se.GetMHUptime() * 100f;
            calc.OHEnchantUptime = 0f;//se.GetOHUptime() * 100f;
            calc.Trinket1Uptime = 0f;// se.GetUptime(character.Trinket1) * 100f;
            calc.Trinket2Uptime = 0f;// se.GetUptime(character.Trinket2) * 100f;
            calc.FireTotemUptime = cs.FireTotemUptime * 100f;
            calc.BaseRegen = cs.BaseRegen;
            calc.ManaRegen = cs.ManaRegen;

            calc.TotalExpertiseMH = (float) Math.Floor(cs.ExpertiseBonusMH * 400f);
            calc.TotalExpertiseOH = (float) Math.Floor(cs.ExpertiseBonusOH * 400f);

            calc.SwingDamage = new DPSAnalysis(dpsMelee, 1 - cs.AverageWhiteHitChance, cs.AverageDodge, cs.GlancingRate, cs.AverageWhiteCritChance, cs.MeleePPM);
            calc.Stormstrike = new DPSAnalysis(dpsSS, 1 - cs.AverageYellowHitChance, cs.AverageDodge, -1, cs.AverageYellowCritChance, 60f / cs.AbilityCooldown(EnhanceAbility.StormStrike));
            calc.LavaLash = new DPSAnalysis(dpsLL, 1 - cs.ChanceYellowHitOH, cs.ChanceDodgeOH, -1, cs.ChanceYellowCritOH, 60f / cs.AbilityCooldown(EnhanceAbility.LavaLash));
            calc.EarthShock = new DPSAnalysis(dpsES, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.EarthShock));
            calc.FlameShock = new DPSAnalysis(dpsFS, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FlameShock));
            calc.LightningBolt = new DPSAnalysis(dpsLB, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceLBSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.LightningBolt));
            calc.WindfuryAttack = new DPSAnalysis(dpsWF, 1 - cs.ChanceYellowHitMH, cs.ChanceDodgeMH, -1, cs.ChanceYellowCritMH, cs.WFPPM);
            calc.LightningShield = new DPSAnalysis(dpsLS, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.LightningShield));
            calc.ChainLightning = new DPSAnalysis(dpsCL, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.ChainLightning));
            calc.SearingMagma = new DPSAnalysis(dpsFireTotem, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit,
                calcOpts.Magma ? 60f / cs.AbilityCooldown(EnhanceAbility.MagmaTotem) : 60f / cs.AbilityCooldown(EnhanceAbility.SearingTotem));
            calc.FlameTongueAttack = new DPSAnalysis(dpsFT, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, cs.FTPPM);
            calc.FireNova = new DPSAnalysis(dpsFireNova, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FireNova));
            calc.UnleashWind = new DPSAnalysis(dpsUW, 1 - cs.ChanceYellowHitMH, cs.ChanceDodgeMH, -1, cs.ChanceYellowCritMH, 60f / cs.AbilityCooldown(EnhanceAbility.UnleashElements));
            calc.UnleashFlame = new DPSAnalysis(dpsUF, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.UnleashElements));
            calc.MagicOther = new DPSAnalysis(dpsMagic, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, cs.MeleePPM/*cs.HitsPerSMH + cs.HitsPerSOH*/);
            calc.PhysicalOther = new DPSAnalysis(dpsPhysical, 1 - cs.AverageYellowHitChance, cs.AverageDodge, -1, cs.AverageYellowCritChance, cs.MeleePPM/*cs.HitsPerSMH + cs.HitsPerSOH*/);
            #endregion
            return calc;
        }
        #endregion

        #region Get Character Stats
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
            Stats statsTotal = GetCharacterStats(character, additionalItem, true) as StatsEnhance;

            return statsTotal;
        }
        private StatsEnhance GetCharacterStats(Character character, Item additionalItem, Boolean buffs)
        {
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance ?? new CalculationOptionsEnhance();
            BossOptions bossOpts = character.BossOptions;
            ShamanTalents talents = character.ShamanTalents;

            StatsEnhance statsRace = new StatsEnhance();
            statsRace.Accumulate(BaseStats.GetBaseStats(character));
            StatsEnhance statsItems = new StatsEnhance();
            AccumulateItemStats(statsItems, character, additionalItem);
            StatsEnhance statsBuffs = new StatsEnhance();
            AccumulateBuffsStats(statsBuffs, character.ActiveBuffs);

            // Talents and Glyphs
            StatsEnhance statsTalents = new StatsEnhance()
            {
                BonusAgilityMultiplier = Character.ValidateArmorSpecialization(character, ItemType.Mail) ? 0.05f : 0f,

                PhysicalHaste = talents.AncestralSwiftness * 0.05f,
                SpellHaste = talents.AncestralSwiftness * 0.05f,

                //BonusLightningBoltDamageMultiplier = talents.GlyphofLightningBolt ? 0.04f : 0f,
            };

            #region Set Bonuses
            StatsEnhance statsSetBonuses = new StatsEnhance();
            int T11Count, T12Count, T13Count, T14Count;
            character.SetBonusCount.TryGetValue("Battlegear of the Raging Elements", out T11Count);
            character.SetBonusCount.TryGetValue("Volcanic Battlegear", out T12Count);
            character.SetBonusCount.TryGetValue("Spiritwalker's Battlegear", out T13Count);
            character.SetBonusCount.TryGetValue("Battlegear of the Firebird", out T14Count);
            if (T11Count >= 2)
            {
                statsSetBonuses.BonusStormstrikeDamageMultiplier = (1f + statsTalents.BonusStormstrikeDamageMultiplier) * (1f + 0.1f) - 1f;
                statsSetBonuses.T11BonusLavaLashDamageMultiplier = 0.1f;
            }
            if (T11Count >= 4)
            {
                // Implemented in CombatStats.cs for now
            }
            if (T12Count >= 2)
            {
                statsSetBonuses.BonusSearingFlamesModifier = 0.05f;
            }
            if (T12Count >= 4)
            {
                statsSetBonuses.BonusFireDamageMultiplier = (1f + statsTalents.BonusFireDamageMultiplier) * (1f + 0.06f) - 1f;
            }
            if (T13Count >= 2)
            {
                statsSetBonuses.BonusLightningBoltDamageMultiplier = (1f + statsTalents.BonusLightningBoltDamageMultiplier) * (1f + 0.2f) - 1f;
            }
            if (T13Count >= 4)
            {
                // Currently not implemented
            }
            if (T14Count >= 2)
            {
                // Increases the damage done by your Lava Lash ability by 15%.
                statsSetBonuses.T14BonusLavaLashDamageMultiplier = 0.15f;
            }
            if (T14Count >= 4)
            {
                // Increases the critical strike chance bonus from your Stormstrike ability by an additional 15%.
                // Implemented in CombatStats.cs for now
            }
            #endregion

            StatsEnhance statsTotal = new StatsEnhance();
            statsTotal.Accumulate(statsRace);
            statsTotal.Accumulate(statsItems);
            statsTotal.Accumulate(statsTalents);
            statsTotal.Accumulate(statsSetBonuses);

            if (buffs)
            {
                statsTotal.Accumulate(statsBuffs);
                StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, calcOpts, bossOpts);
                statsTotal.Accumulate(se.getSpecialEffects());
            }

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1f + statsTotal.BonusIntellectMultiplier));

            statsTotal.Health += (float)Math.Floor(StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            //statsTotal.Mana += (float)Math.Floor(StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));

            statsTotal.AttackPower += statsTotal.Strength + 2f * statsTotal.Agility;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.SpellPower = 0.55f * statsTotal.AttackPower;

            return statsTotal;
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
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Fire Elemental Totem");
                _relevantGlyphs.Add("Glyph of Flame Shock");
                _relevantGlyphs.Add("Glyph of Lava Lash");
            }
            return _relevantGlyphs;
        }
        #endregion

        #region Triggers
        internal static List<Trigger> _relevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                if (_relevantTriggers == null)
                    _relevantTriggers = new List<Trigger>()
                    {
                        Trigger.Use,
                        Trigger.SpellHit,
                        Trigger.SpellCrit,
                        Trigger.SpellCast,
                        Trigger.DamageSpellHit,
                        Trigger.DamageSpellCrit,
                        Trigger.DamageSpellCast,
                        Trigger.MeleeHit,
                        Trigger.MeleeCrit,
                        Trigger.MeleeAttack,
                        Trigger.PhysicalHit,
                        Trigger.PhysicalCrit,
                        Trigger.PhysicalAttack,
                        Trigger.WhiteHit,
                        Trigger.WhiteCrit,
                        Trigger.WhiteAttack,
                        Trigger.CurrentHandHit,
                        Trigger.DamageDone,
                        Trigger.DamageOrHealingDone,
                        Trigger.ShamanLightningBolt,
                        Trigger.ShamanLavaLash,
                        Trigger.ShamanShock,
                        Trigger.ShamanStormStrike,
                        Trigger.ShamanShamanisticRage,
                        Trigger.ShamanFlameShockDoTTick,
                        Trigger.DoTTick,
                    };

                return _relevantTriggers;
            }
        }

        public bool HasRelevantTrigger(Trigger trigger)
        {
            return RelevantTriggers.Contains(trigger);
        }
        #endregion

        #region Items
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
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.FistWeapon,
                        //// Able to be used, but not recommended
                        //ItemType.Dagger,
                        //ItemType.TwoHandAxe,
                        //ItemType.TwoHandMace,
                        //ItemType.Staff,
                        //ItemType.Shield,
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if ((item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Totem && item.Type != ItemType.Relic)))
                return false;
            if (item.Slot == ItemSlot.OffHand && item.Type == ItemType.None)
                return false;
            return base.IsItemRelevant(item);
        }
        #endregion

        #region Buffs
        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            if (buff.AllowedClasses.Contains(CharacterClass.Shaman))
                return base.IsBuffRelevant(buff, character);
            else
                return false;
        }

        public override void SetDefaults(Character character)
        {
            #region Buffs
            // Stats
            character.ActiveBuffsAdd("Mark of the Wild");
            // Stamina
            character.ActiveBuffsAdd("Power Word: Fortitude");
            // Attack Power
            character.ActiveBuffsAdd("Battle Shout");
            // Attack Speed
            character.ActiveBuffsAdd("Unleashed Rage");
            // Critical Strike
            character.ActiveBuffsAdd("Leader of the Pack");
            // Mastery
            character.ActiveBuffsAdd(("Grace of Air"));
            // Temporary Buffs:
            // Bloodlust
            //character.ActiveBuffsAdd("Heroism/Bloodlust");
            #endregion
            
            #region Debuffs
            // Weakened Armor
            character.ActiveBuffsAdd("Faerie Fire");
            // Physical Vulnerability
            character.ActiveBuffsAdd("Judgments of the Bold");
            // Magic Vulnerability
            character.ActiveBuffsAdd("Curse of the Elements");
            #endregion

            #region Consumables
            // Flask
            character.ActiveBuffsAdd("Flask of Spring Blossoms");
            // Potion
            character.ActiveBuffsAdd("Virmen's Bite");
            // Food
            character.ActiveBuffsAdd("Agility Food");
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
                Strength = stats.Strength,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Stamina = stats.Stamina,
                #endregion

                #region Health Related Stats
                Health = stats.Health,
                HealthRestore = stats.HealthRestore,
                #endregion

                #region Mana Related Stats
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,
                #endregion

                #region Offensive Stats
                AttackPower = stats.AttackPower,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                Expertise = stats.Expertise,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                Mastery = stats.Mastery,
                MasteryRating = stats.MasteryRating,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHit = stats.PhysicalHit,
                WeaponDamage = stats.WeaponDamage,
                #endregion

                #region Defensive Stats
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
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                // Secondary
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                //BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                PhysicalHaste = stats.PhysicalHaste,
                SpellHaste = stats.SpellHaste,
                #endregion

                #region Health Related Stats
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                #endregion

                #region Mana Related Stats
                BonusManaMultiplier = stats.BonusManaMultiplier,
                #endregion

                #region Offensive Stats
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                #endregion

                #region Other Stats
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                #endregion
                #endregion

                #region Inverse Multiplicative Stats
                TargetArmorReduction = stats.TargetArmorReduction,
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
                if (HasRelevantTrigger(effect.Trigger))
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                    else
                    {
                        foreach (SpecialEffect subEffect in effect.Stats.SpecialEffects())
                        {
                            if (HasRelevantTrigger(subEffect.Trigger) && HasRelevantStats(subEffect.Stats))
                            {
                                s.AddSpecialEffect(effect);
                            }
                        }
                    }
                }
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            if (relevantStats(stats))
                return true;
            if (irrelevantStats(stats))
                return false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantTrigger(effect.Trigger))
                    foreach (SpecialEffect subEffect in effect.Stats.SpecialEffects())
                    {
                        if (HasRelevantTrigger(subEffect.Trigger) && relevantStats(subEffect.Stats))
                            return true;
                    }
                if (relevantStats(effect.Stats))
                    return true;
            }
            return false;
        }

        private bool relevantStats(Stats stats)
        {
            return (stats.Agility + stats.Intellect + stats.Stamina + stats.Strength + stats.Spirit +
                stats.AttackPower + stats.SpellPower + stats.Mana + stats.WeaponDamage + stats.Health +
                stats.Mastery + stats.MasteryRating + stats.TargetArmorReduction +
                stats.Expertise + stats.ExpertiseRating + stats.HasteRating + stats.CritRating + stats.HitRating +
                stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritDamageMultiplier +
                stats.BonusStrengthMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusIntellectMultiplier +
                stats.BonusSpiritMultiplier + stats.BonusDamageMultiplier + stats.BonusWhiteDamageMultiplier + stats.BonusPhysicalDamageMultiplier +
                stats.BonusNatureDamageMultiplier + stats.BonusFireDamageMultiplier + stats.BonusSpellCritDamageMultiplier +
                stats.BonusHealthMultiplier + stats.BonusManaMultiplier +
                stats.PhysicalCrit + stats.PhysicalHaste + stats.PhysicalHit + stats.Paragon + stats.BonusShadowDamageMultiplier +
                stats.SpellCrit + stats.SpellCritOnTarget + stats.SpellHaste + stats.SpellHit + stats.HighestStat + stats.HighestSecondaryStat +
                stats.MoteOfAnger + stats.PhysicalDamage +
                stats.NatureDamage + stats.FireDamage + stats.FrostDamage + stats.ArcaneDamage + stats.HolyDamage + stats.ShadowDamage +
                stats.Mp5 + stats.ManaRestoreFromMaxManaPerSecond +
                stats.SnareRootDurReduc + stats.FearDurReduc + stats.StunDurReduc + stats.MovementSpeed + stats.NatureSpellsManaCostReduction) != 0;
        }

        private bool irrelevantStats(Stats stats)
        {
            return stats.BonusRageGen != 0;
        }
        #endregion
        #endregion

        #region Custom Chart Data
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Combat Table (White)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceWhite = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissWhite = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeWhite = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritWhite = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcGlanceWhite = new ComparisonCalculationEnhance() { Name = " Glance " };
                    ComparisonCalculationEnhance calcHitWhite = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceWhite != null)
                    {
                        calcMissWhite.SubPoints = new float[2];
                        calcMissWhite.DPS = 0;
                        calcMissWhite.OverallPoints = calcMissWhite.SubPoints[1] = 100 - currentCalculationsEnhanceWhite.WhiteHit;
                        calcDodgeWhite.OverallPoints = calcDodgeWhite.DPS = currentCalculationsEnhanceWhite.DodgedAttacks;
                        calcCritWhite.SubPoints = new float[2];
                        calcCritWhite.SubPoints[1] = currentCalculationsEnhanceWhite.OverMeleeCritCap;
                        calcCritWhite.OverallPoints = currentCalculationsEnhanceWhite.MeleeCrit - 4.8f;
                        calcCritWhite.DPS = calcCritWhite.OverallPoints - calcCritWhite.SubPoints[1];
                        calcGlanceWhite.OverallPoints = calcGlanceWhite.DPS = currentCalculationsEnhanceWhite.GlancingBlows;
                        calcHitWhite.OverallPoints = calcHitWhite.DPS = 100f - calcMissWhite.OverallPoints 
                                                                                   - calcDodgeWhite.OverallPoints 
                                                                                   - calcCritWhite.DPS 
                                                                                   - calcGlanceWhite.OverallPoints;
                    }
                    return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

                case "Combat Table (Yellow)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceYellow = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissYellow = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeYellow = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritYellow = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcHitYellow = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceYellow != null)
                    {
                        calcMissYellow.OverallPoints = calcMissYellow.DPS = 100f - currentCalculationsEnhanceYellow.YellowHit;
                        calcDodgeYellow.OverallPoints = calcDodgeYellow.DPS = currentCalculationsEnhanceYellow.DodgedAttacks;
                        calcCritYellow.OverallPoints = calcCritYellow.DPS = currentCalculationsEnhanceYellow.YellowCrit;
                        calcHitYellow.OverallPoints = calcHitYellow.DPS = (100f - calcMissYellow.OverallPoints -
                        calcDodgeYellow.OverallPoints - calcCritYellow.OverallPoints);
                    }
                    return new ComparisonCalculationBase[] { calcMissYellow, calcDodgeYellow, calcCritYellow, calcHitYellow };

                case "Combat Table (Spell)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceSpell = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissSpell = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcCritSpell = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcHitSpell = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceSpell != null)
                    {
                        calcMissSpell.OverallPoints = calcMissSpell.DPS = 100f - currentCalculationsEnhanceSpell.SpellHit;
                        calcCritSpell.OverallPoints = calcCritSpell.DPS = currentCalculationsEnhanceSpell.SpellCrit;
                        calcHitSpell.OverallPoints = calcHitSpell.DPS = (100f - calcMissSpell.OverallPoints - calcCritSpell.OverallPoints);
                    }
                    return new ComparisonCalculationBase[] { calcMissSpell, calcCritSpell, calcHitSpell };

                case "Relative Gem Values":
                    float dpsBase = GetCharacterCalculations(character).OverallPoints;
                    float dpsStr = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Strength = 40 } }).OverallPoints - dpsBase);
                    float dpsAgi = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Agility = 40 } }).OverallPoints - dpsBase);
                    float dpsInt = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Intellect = 40 } }).OverallPoints - dpsBase);
                    float dpsCrit = (GetCharacterCalculations(character, new Item()  { Stats = new Stats() { CritRating = 40 } }).OverallPoints - dpsBase);
                    float dpsExp = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { ExpertiseRating = 40 } }).OverallPoints - dpsBase);
                    float dpsHaste = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 40 } }).OverallPoints - dpsBase);
                    float dpsHit = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { HitRating = 40 } }).OverallPoints - dpsBase);
                    float dpsDmg = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
                    float dpsMast = (GetCharacterCalculations(character, new Item()  { Stats = new Stats() { MasteryRating = 40 } }).OverallPoints - dpsBase);
                    //float dpsSta = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Stamina = 60 } }).OverallPoints - dpsBase);

                    return new ComparisonCalculationBase[] { 
                        //new ComparisonCalculationEnhance() { Name = "60 Stamina", OverallPoints = dpsSta, DPSPoints = dpsSta },
                        new ComparisonCalculationEnhance() { Name = "40 Agility", OverallPoints = dpsAgi, DPS = dpsAgi },
                        new ComparisonCalculationEnhance() { Name = "40 Strength", OverallPoints = dpsStr, DPS = dpsStr },
                        new ComparisonCalculationEnhance() { Name = "40 Intellect", OverallPoints = dpsInt, DPS = dpsInt },
                        new ComparisonCalculationEnhance() { Name = "40 Crit Rating", OverallPoints = dpsCrit, DPS = dpsCrit },
                        new ComparisonCalculationEnhance() { Name = "40 Expertise Rating", OverallPoints = dpsExp, DPS = dpsExp },
                        new ComparisonCalculationEnhance() { Name = "40 Haste Rating", OverallPoints = dpsHaste, DPS = dpsHaste },
                        new ComparisonCalculationEnhance() { Name = "40 Hit Rating", OverallPoints = dpsHit, DPS = dpsHit },
                        new ComparisonCalculationEnhance() { Name = "40 Mastery Rating", OverallPoints = dpsMast, DPS = dpsMast }
                    };

                case "MH Weapon Speeds":
                    if (character.MainHand == null)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase MHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, true);
                    ComparisonCalculationBase MHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, true);
                    ComparisonCalculationBase MHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, true);
                    ComparisonCalculationBase MHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, true);
                    ComparisonCalculationBase MHonePointEight = CheckWeaponSpeedEffect(character, 1.8f, true);
                    ComparisonCalculationBase MHtwoPointThree = CheckWeaponSpeedEffect(character, 2.3f, true);
                    ComparisonCalculationBase MHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, true);
                    ComparisonCalculationBase MHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, true);
                    ComparisonCalculationBase MHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, true);
                    ComparisonCalculationBase MHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, true);
                    ComparisonCalculationBase MHtwoPointEight = CheckWeaponSpeedEffect(character, 2.8f, true);
                    return new ComparisonCalculationBase[] { MHonePointFour, MHonePointFive, MHonePointSix, MHonePointSeven, MHonePointEight, 
                                                             MHtwoPointThree, MHtwoPointFour, MHtwoPointFive, MHtwoPointSix, MHtwoPointSeven, MHtwoPointEight };

                case "OH Weapon Speeds":
                    if (character.OffHand == null)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase OHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, false);
                    ComparisonCalculationBase OHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, false);
                    ComparisonCalculationBase OHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, false);
                    ComparisonCalculationBase OHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, false);
                    ComparisonCalculationBase OHonePointEight = CheckWeaponSpeedEffect(character, 1.8f, false);
                    ComparisonCalculationBase OHtwoPointThree = CheckWeaponSpeedEffect(character, 2.3f, false);
                    ComparisonCalculationBase OHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, false);
                    ComparisonCalculationBase OHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, false);
                    ComparisonCalculationBase OHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, false);
                    ComparisonCalculationBase OHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, false);
                    ComparisonCalculationBase OHtwoPointEight = CheckWeaponSpeedEffect(character, 2.8f, false);
                    return new ComparisonCalculationBase[] { OHonePointFour, OHonePointFive, OHonePointSix, OHonePointSeven, OHonePointEight, 
                                                             OHtwoPointThree, OHtwoPointFour, OHtwoPointFive, OHtwoPointSix, OHtwoPointSeven, OHtwoPointEight };

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private ComparisonCalculationBase CheckWeaponSpeedEffect(Character character, float newSpeed, bool mainHand)
        {
            float baseSpeed = 0f;
            int minDamage = 0;
            int maxDamage = 0;
            Item newWeapon;
            CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(character);
            Character deltaChar = character.Clone();

            if (mainHand)
            {
                newWeapon = deltaChar.MainHand.Item.Clone();
                baseSpeed = deltaChar.MainHand.Speed;
                minDamage = deltaChar.MainHand.MinDamage;
                maxDamage = deltaChar.MainHand.MaxDamage;
            }
            else
            {
                newWeapon = deltaChar.OffHand.Item.Clone();
                baseSpeed = deltaChar.OffHand.Speed;
                minDamage = deltaChar.OffHand.MinDamage;
                maxDamage = deltaChar.OffHand.MaxDamage;
            }
            newWeapon.MinDamage = (int)Math.Round(minDamage / baseSpeed * newSpeed);
            newWeapon.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * newSpeed);
            newWeapon.Speed = newSpeed;
            String speed = newSpeed.ToString() + " Speed";
            deltaChar.IsLoading = true; // forces item instance to avoid invalidating and reloading from cache
            if (mainHand)
                deltaChar.MainHand = new ItemInstance(newWeapon, character.MainHand.RandomSuffixId, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant, character.MainHand.Reforging, character.MainHand.Tinkering);
            else
                deltaChar.OffHand = new ItemInstance(newWeapon, deltaChar.OffHand.RandomSuffixId, deltaChar.OffHand.Gem1, deltaChar.OffHand.Gem2, deltaChar.OffHand.Gem3, deltaChar.OffHand.Enchant, deltaChar.OffHand.Reforging, deltaChar.OffHand.Tinkering);
            ComparisonCalculationBase result = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, speed, baseSpeed == newWeapon.Speed);
            deltaChar.IsLoading = false;
            result.Item = null;
            return result;
        }

        #endregion
    }
}

