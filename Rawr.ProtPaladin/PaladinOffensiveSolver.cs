using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class RotationAbility
    {
        public string Name { get; set; }
        public double BaseDamage { get; set; }
        public double APScaling { get; set; }
        public double SPScaling { get; set; }
        public double WDScaling { get; set; }
        public bool IsPhysicalDamage { get; set; }
        public bool IsSpell { get; set; }
        public bool CanMiss { get; set; }
        public bool CanBeDodged { get; set; }
        public bool CanBeParried { get; set; }
        public bool CanBeBlocked { get; set; }
    }

    public class PaladinOffensiveSolver
    {
        #region Cast Distribution Tables

        #region Normal

        static double[] NormalRotation = new double[] { 0.237056434095435, 0.184377199907373, 0.0989157693017168, 0.109226186614576, 0.0992818310603338, 0, 0.170302596300735, 0.0395093798102173 };

        static double[] NormalRotationHoW = new double[] { 0.237056420170782, 0.184377193799151, 0.098915780877799, 0.0727017609988009, 0.0506448808192891, 0.146491982010121, 0.170302587733336, 0.0395093935907224 };

        #endregion

        #region Sanctified Wrath

        static double[] SWRotation = new double[] { 0.237056434095435, 0.184377199907373, 0.0989157693017168, 0.109226186614576, 0.0992818310603338, 0, 0.170302596300735, 0.0395093798102173 };

        static double[] SWRotationHoW = new double[] { 0.158653869574269, 0.396634673935674, 0.0812775971179659, 0, 0, 0.117039739849871, 0.206730652128654, 0.0396634673935672 };

        #endregion

        #region Holy Avenger

        static double[] HARotation = new double[] { 0.178217832259168, 0.138613864108344, 0.074364361345144, 0.0821157199754296, 0.0746395992280366, 0, 0.376237600152358, 0.0297029676900463 };

        static double[] HARotationHoW = new double[] { 0.178217823982572, 0.138613862402586, 0.0743643803842873, 0.0546568261225276, 0.0380745741380925, 0.110131948767431, 0.376237612619439, 0.0297029715830657 };

        #endregion

        #region Divine Purpose

        static double[] DPRotation = new double[] { 0.233841046579517, 0.18017572392547, 0.0965218970757497, 0.101636074008313, 0.0819774747556974, 0, 0.22116636276832, 0.0394842231085051 };

        static double[] DPRotationHoW = new double[] { 0.233841166509193, 0.180175732916283, 0.0965217612083913, 0.0647412262751709, 0.0287939352320241, 0.135275674938283, 0.221166327944398, 0.0394841749762537 };

        #endregion

        #endregion

        public static string[] RotationAbilities = new string[] { "CS", "J", "AS", "Cons", "HW", "HoW", "SotR", "HotR" };

        #region Ability List

        public RotationAbility[] Abilities = new RotationAbility[]
        {
            new RotationAbility
            {
                Name = "CS",
                BaseDamage = 791,
                WDScaling = 1.25,
                IsPhysicalDamage = true,
                CanMiss = true,
                CanBeBlocked = true,
                CanBeDodged = true,
                CanBeParried = true
            },
            new RotationAbility
            {
                Name = "J",
                BaseDamage = 623,
                APScaling = 0.328,
                SPScaling = 0.546,
                IsPhysicalDamage = false,
                CanMiss = true
            },
            new RotationAbility
            {
                Name = "AS",
                BaseDamage = 6732,
                APScaling = 0.8175,
                SPScaling = 0.315,
                IsPhysicalDamage = false,
                IsSpell = true,
                CanMiss = true
            },
            new RotationAbility
            {
                Name = "Cons",
                BaseDamage = 927,
                APScaling = 0.81,
                IsPhysicalDamage = false,
                IsSpell = true,
                CanMiss = true
            },
            new RotationAbility
            {
                Name = "HW",
                BaseDamage = 4300,
                SPScaling = 0.91,
                IsPhysicalDamage = false,
                IsSpell = true,
                CanMiss = true
            },
            new RotationAbility
            {
                Name = "HoW",
                BaseDamage = 1838,
                SPScaling = 1.61,
                IsPhysicalDamage = false,
                IsSpell = false,
                CanMiss = true,
                CanBeDodged = true
            },
            new RotationAbility
            {
                Name = "SotR",
                BaseDamage = 836,
                APScaling = 0.617,
                IsPhysicalDamage = false,
                IsSpell = false,
                CanMiss = true,
                CanBeDodged = true,
                CanBeParried = true,
                CanBeBlocked = true
            },
            new RotationAbility
            {
                Name = "HotR",
                BaseDamage = 0,
                WDScaling = 0.2,
                IsPhysicalDamage = true,
                IsSpell = false,
                CanMiss = true,
                CanBeDodged = true,
                CanBeParried = true,
                CanBeBlocked = true
            }
        };

        #endregion

        // Normal, Normal+HoW, SW, SW+HoW, DP, DP+HoW, HA, HA+HoW
        public static float[] SotRRotationIntervals = new float[] { 6.9121630859375f, 6.9121630859375f, 6.9121630859375f, 5.490003f, 6.7956650390625f, 6.7956650390625f, 2.21359130859375f, 2.213596435546875f };

        public float SotRInterval = 6.9121630859375f;

        public Dictionary<Trigger, float> TriggerIntervals { get; set; }
        public Dictionary<Trigger, float> TriggerChances { get; set; }

        private SpecialEffect _alabasterShield = new SpecialEffect { Chance = 1, MaxStack = 3, Duration = float.PositiveInfinity, Cooldown = 0, Trigger = Trigger.MeleeHit, Stats = new Stats() };

        public double[] CalculateCastTable(Character character, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts, CharacterCalculationsProtPaladin calcs)
        {
            double[] castDistributionTable = new double[RotationAbilities.Length];
            // Derive the cast table from the current talent selection, boss under 20% percentage, Avenging Wrath uptime
            DeriveCastTable(bossOpts.Under20Perc, calcs.AvengingWrathUptime, calcs.HolyAvengerUptime, character.PaladinTalents.SanctifiedWrath, character.PaladinTalents.HolyAvenger, character.PaladinTalents.DivinePurpose, castDistributionTable);

            return castDistributionTable;
        }

        public void Solve(Character character, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts, ref CharacterCalculationsProtPaladin calcs)
        {
            TriggerIntervals = new Dictionary<Trigger, float>();
            TriggerChances = new Dictionary<Trigger, float>();
            double rotationDPS = 0;
            double normalizedWeaponDamage = calcs.BasicStats.WeaponDamage + (2.4 * (calcs.BasicStats.AttackPower + calcs.AverageVengeanceAP) / 14);
            double[] castDistributionTable = CalculateCastTable(character, calcOpts, bossOpts, calcs);
            int levelDelta = bossOpts.Level - character.Level;
            // SotRInterval set in the DeriveCastTable function because of the interaction with talents
            // Apply hit and expertise as a multiplier to SotR interval
            double grandCrusaderPercentage = 0.2 * (castDistributionTable[0] + castDistributionTable[7]) * castDistributionTable[2];
            double combinedHPAbilityWeight = castDistributionTable[0] + castDistributionTable[1] + castDistributionTable[7] + grandCrusaderPercentage;
            double meleeAbilityPercentage = (castDistributionTable[0] + castDistributionTable[1] + castDistributionTable[7]) / combinedHPAbilityWeight;
            double spellAbilityPercentage = grandCrusaderPercentage / combinedHPAbilityWeight;
            float combinedMiss = (float)(meleeAbilityPercentage * Math.Max(0, StatConversion.YELLOW_MISS_CHANCE_CAP[levelDelta] - calcs.Hit) +
                spellAbilityPercentage * Math.Max(0, StatConversion.SPELL_MISS_CHANCE_CAP[levelDelta] - calcs.SpellHit));
            float combinedAvoid = (float)(meleeAbilityPercentage * (Math.Max(0, StatConversion.YELLOW_DODGE_CHANCE_CAP[levelDelta] - calcs.Expertise) +
                                   Math.Max(0, Math.Min(StatConversion.YELLOW_PARRY_CHANCE_CAP[levelDelta], 2 * StatConversion.YELLOW_PARRY_CHANCE_CAP[levelDelta] - calcs.Expertise))));

            SotRInterval /= (1 + calcs.Haste);
            SotRInterval /= (1 - combinedMiss) * (1 - combinedAvoid);
            // Do yellow attack scaling
            float physicalDamageMultiplier = (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            float spellDamageMultiplier = (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusHolyDamageMultiplier);
            foreach (RotationAbility ability in Abilities)
            {
                // Do not calculate SotR damage if a shield is not equipped
                if (ability.Name == "SotR" && (character.OffHand == null || character.OffHand.Type != ItemType.Shield))
                    continue;

                double abilityScaledDamage = ability.BaseDamage + ((calcs.AverageVengeanceAP + calcs.BasicStats.AttackPower) * ability.APScaling) +
                    calcs.BasicStats.SpellPower * ability.SPScaling + normalizedWeaponDamage * ability.WDScaling;
                if (ability.IsPhysicalDamage)
                    abilityScaledDamage *= (1 - StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Level, bossOpts.Armor, calcs.BasicStats.TargetArmorReduction, 0)) *
                        physicalDamageMultiplier;
                else
                    abilityScaledDamage *= spellDamageMultiplier;
                // Store the raw damage per hit in the output array
                calcs.Abilities[ability.Name] = (float)abilityScaledDamage;
                // Avoidance/crit/block calculations on offensive abilities
                if (ability.IsSpell)
                {
                    if (ability.Name == "Cons")
                        abilityScaledDamage = CalculateConsecrationSpellDamage(abilityScaledDamage, calcs, 9, levelDelta);
                    else
                    {
                        double missedPercentage = Math.Max(0, StatConversion.SPELL_MISS_CHANCE_CAP[levelDelta] - calcs.SpellHit);
                        double hitModifier = 1 - missedPercentage;
                        double critModifier = calcs.SpellCrit;
                        abilityScaledDamage = (abilityScaledDamage * (1 - critModifier) + abilityScaledDamage * 2 * critModifier) * hitModifier;
                    }
                }
                else
                {
                    double missedPercentage = ability.CanMiss ? Math.Max(0, StatConversion.YELLOW_MISS_CHANCE_CAP[levelDelta] - calcs.Hit) : 0;
                    double dodgedPercentage = ability.CanBeDodged ? Math.Max(0, StatConversion.YELLOW_DODGE_CHANCE_CAP[levelDelta] - calcs.Expertise) : 0;
                    double parriedPercentage = ability.CanBeParried ? Math.Max(0, Math.Min(StatConversion.YELLOW_PARRY_CHANCE_CAP[levelDelta], 2 * StatConversion.YELLOW_PARRY_CHANCE_CAP[levelDelta] - calcs.Expertise)) : 0;

                    double unavoidedPercentage = 1 - missedPercentage - dodgedPercentage - parriedPercentage;
                    double critPercentage = calcs.Crit;
                    double blockedPercentage = ability.CanBeBlocked ? StatConversion.YELLOW_BLOCK_CHANCE_CAP[levelDelta] : 0;

                    // 1st roll - hit/avoidance
                    // 2nd roll - crit
                    abilityScaledDamage = (abilityScaledDamage * 2 * critPercentage + abilityScaledDamage * (1 - critPercentage)) * unavoidedPercentage;
                    // 3nd roll - block
                    abilityScaledDamage = abilityScaledDamage * (1 - blockedPercentage) + abilityScaledDamage * 0.7 * blockedPercentage;
                    // HotR nova, 35% normalized weapon damage as Holy, hits when HotR hits
                    if (ability.Name == "HotR")
                    {
                        double newAbilityDamage = 0.35 * normalizedWeaponDamage * spellDamageMultiplier;
                        calcs.Abilities["HotR"] += (float)newAbilityDamage;
                        newAbilityDamage = (newAbilityDamage * 2 * critPercentage + newAbilityDamage * (1 - critPercentage)) * unavoidedPercentage;
                        abilityScaledDamage += newAbilityDamage;
                    }
                    // Seal of Truth, 12% weapon damage as Holy, hits on melee hit except HotR
                    else if (ability.Name != "HoW")
                    {
                        double newAbilityDamage = 0.12 * normalizedWeaponDamage * spellDamageMultiplier;
                        newAbilityDamage = (newAbilityDamage * 2 * critPercentage + newAbilityDamage * (1 - critPercentage)) * unavoidedPercentage;
                        abilityScaledDamage += newAbilityDamage;
                    }
                }
                // Holy Avenger, HP generators used during HA do 30% more damage (18 sec/2 min)
                if (character.PaladinTalents.HolyAvenger &&
                    (ability.Name == "CS" || ability.Name == "J" || ability.Name == "HotR"))
                {
                    abilityScaledDamage *= 1 + (0.3 * calcs.HolyAvengerUptime);
                }
                // Glyph of Final Wrath
                if (character.PaladinTalents.GlyphOfFinalWrath && ability.Name == "HW")
                    abilityScaledDamage *= 1 + (0.5 * bossOpts.Under20Perc);
                // Glyph of Focused Shield
                if (character.PaladinTalents.GlyphOfFocusedShield && ability.Name == "AS")
                    abilityScaledDamage *= 1.3;
                // Glyph of the Alabaster Shield
                if (character.PaladinTalents.GlyphOfTheAlabasterShield && ability.Name == "SotR")
                {
                    float blockInterval = 2f;
                    float avgStack = _alabasterShield.GetAverageStackSize(blockInterval, 1, 3, 0f, SotRInterval);
                    abilityScaledDamage *= 1 + (0.2 * avgStack);
                }

                rotationDPS += abilityScaledDamage * castDistributionTable[Array.IndexOf(RotationAbilities, ability.Name)];
            }

            rotationDPS /= (1.5 / (1 + calcs.Haste));
            // White DPS
            // Base white damage
            double whiteDamage = calcs.BasicStats.WeaponDamage + (calcs.WeaponSpeed * calcs.BasicStats.AttackPower / 14);
            whiteDamage *= 1 - StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Level, bossOpts.Armor, calcs.BasicStats.TargetArmorReduction, 0) * physicalDamageMultiplier;
            calcs.Abilities["Melee"] = (float)whiteDamage;
            // 1st roll: Hit/crit/glancing/dodge/parry
            double avoidedSwings = Math.Max(0, StatConversion.WHITE_MISS_CHANCE_CAP[levelDelta] - calcs.Hit) +
                                   Math.Max(0, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDelta] - calcs.Expertise) +
                                   Math.Max(0, Math.Min(StatConversion.WHITE_PARRY_CHANCE_CAP[levelDelta], 2 * StatConversion.WHITE_PARRY_CHANCE_CAP[levelDelta] - calcs.Expertise));
            double glancingSwings = StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDelta];
            double critSwings = Math.Max(calcs.Crit, 1 - avoidedSwings - glancingSwings);
            double hitSwings = 1 - avoidedSwings - glancingSwings - critSwings;
            whiteDamage = whiteDamage * GlancingReduction(character.Level, bossOpts.Level) * glancingSwings +
                whiteDamage * 2 * critSwings +
                whiteDamage * hitSwings;
            // 2nd roll: block
            double blockedSwings = StatConversion.WHITE_BLOCK_CHANCE_CAP[levelDelta];
            whiteDamage = whiteDamage * (1 - blockedSwings) + whiteDamage * 0.7 * blockedSwings;
            // Seal of Truth procs from white swings
            whiteDamage += (0.12 * normalizedWeaponDamage * spellDamageMultiplier) * (1 - avoidedSwings);
            calcs.Abilities["SoT"] = (float)(0.12 * normalizedWeaponDamage * spellDamageMultiplier);
            rotationDPS += whiteDamage / calcs.WeaponSpeed;
            // Censure
            rotationDPS += ((107 + 0.094 * calcs.BasicStats.SpellPower) * 5 * spellDamageMultiplier) / 3.0;
            calcs.Abilities["Censure"] = (float)((107 + 0.094 * calcs.BasicStats.SpellPower) * 5 * spellDamageMultiplier);
            // Level 90 talent DPS
            if (character.PaladinTalents.LightsHammer)
            {
                double abilityScaledDamage = (3630 + .321 * calcs.BasicStats.SpellPower) * 8 * spellDamageMultiplier;
                calcs.Abilities["LH"] = (float)abilityScaledDamage;
                abilityScaledDamage = CalculateConsecrationSpellDamage(abilityScaledDamage, calcs, 8, levelDelta);
                rotationDPS += abilityScaledDamage / 60.0;
            }
            else if (character.PaladinTalents.HolyPrism)
            {
                double missedPercentage = 0;
                double abilityScaledDamage = (16136 + 1.428 * calcs.BasicStats.SpellPower) * spellDamageMultiplier;
                calcs.Abilities["HrP"] = (float)abilityScaledDamage;
                abilityScaledDamage = (abilityScaledDamage * (1 - calcs.SpellCrit) + abilityScaledDamage * 2 * calcs.SpellCrit) * (1 - missedPercentage);
                rotationDPS += abilityScaledDamage / 20.0;
            }
            else if (character.PaladinTalents.ExecutionSentence)
            {
                double missedPercentage = Math.Max(0, StatConversion.SPELL_MISS_CHANCE_CAP[levelDelta] - calcs.SpellHit);
                double abilityScaledDamage = (12989 + 5.936 * calcs.BasicStats.SpellPower) * spellDamageMultiplier;
                calcs.Abilities["ES"] = (float)abilityScaledDamage;
                abilityScaledDamage = (abilityScaledDamage * (1 - calcs.SpellCrit) + abilityScaledDamage * 2 * calcs.SpellCrit) * (1 - missedPercentage);
                rotationDPS += abilityScaledDamage / 60.0;
            }
            // Send output variables in the calcs object
            calcs.MissedAttacks = Math.Max(0, StatConversion.WHITE_MISS_CHANCE_CAP[levelDelta] - calcs.Hit);
            calcs.DodgedAttacks = Math.Max(0, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDelta] - calcs.Expertise);
            calcs.ParriedAttacks = Math.Max(0, Math.Min(StatConversion.WHITE_PARRY_CHANCE_CAP[levelDelta], 2 * StatConversion.WHITE_PARRY_CHANCE_CAP[levelDelta] - calcs.Expertise));
            calcs.AvoidedAttacks = calcs.MissedAttacks +
                                   calcs.DodgedAttacks +
                                   calcs.ParriedAttacks;
            calcs.BlockedAttacks = (1 - calcs.AvoidedAttacks) * StatConversion.WHITE_BLOCK_CHANCE_CAP[levelDelta];
            calcs.GlancingAttacks = StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDelta];
            calcs.GlancingReduction = GlancingReduction(character.Level, bossOpts.Level);
            calcs.DPS = (float)rotationDPS;
            calcs.TPS = calcs.DPS * 5;
            calcs.ThreatPoints = calcs.TPS;
            // Build triggers
            TriggerIntervals.Add(Trigger.MeleeAttack, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.MeleeHit, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.MeleeCrit, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.MeleeHitorDoTTick, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.PhysicalAttack, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.PhysicalHit, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.PhysicalCrit, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.PhysicalHitorDoTTick, calcs.WeaponSpeed);
            TriggerIntervals.Add(Trigger.SpellCast, 0);
            TriggerIntervals.Add(Trigger.DamageSpellCast, 0);
            TriggerIntervals.Add(Trigger.SpellHit, 0);
            TriggerIntervals.Add(Trigger.DamageSpellHit, 0);
            TriggerIntervals.Add(Trigger.SpellCrit, 0);
            TriggerIntervals.Add(Trigger.DamageSpellCrit, 0);
            TriggerIntervals.Add(Trigger.DamageSpellHitorDoTTick, 0);
            TriggerChances.Add(Trigger.MeleeAttack, 1);
            TriggerChances.Add(Trigger.MeleeHit, 1 - (float)avoidedSwings);
            TriggerChances.Add(Trigger.MeleeCrit, (float)critSwings);
            TriggerChances.Add(Trigger.MeleeHitorDoTTick, 1 - (float)avoidedSwings);
            TriggerChances.Add(Trigger.PhysicalAttack, 1);
            TriggerChances.Add(Trigger.PhysicalHit, 1 - (float)avoidedSwings);
            TriggerChances.Add(Trigger.PhysicalCrit, (float)critSwings);
            TriggerChances.Add(Trigger.PhysicalHitorDoTTick, 1 - (float)avoidedSwings);
            TriggerChances.Add(Trigger.SpellCast, 0);
            TriggerChances.Add(Trigger.DamageSpellCast, 0);
            TriggerChances.Add(Trigger.SpellHit, 0);
            TriggerChances.Add(Trigger.DamageSpellHit, 0);
            TriggerChances.Add(Trigger.SpellCrit, 0);
            TriggerChances.Add(Trigger.DamageSpellCrit, 0);
            TriggerChances.Add(Trigger.DamageSpellHitorDoTTick, 0);
        }

        // Utility function to calculate the total damage per cast of Consecration-style spells
        private double CalculateConsecrationSpellDamage(double scaledDamage, CharacterCalculationsProtPaladin calcs, int numberOfTicks, int levelDelta)
        {
            double tickDamage = scaledDamage / numberOfTicks;
            double critChance = calcs.SpellCrit;
            double missChance = Math.Max(0, StatConversion.SPELL_MISS_CHANCE_CAP[levelDelta] - calcs.SpellHit);
            tickDamage = (tickDamage * (1 - critChance) + tickDamage * 2 * critChance) * (1 - missChance);
            return tickDamage * numberOfTicks;
        }

        private void DeriveCastTable(double under20Percent, double avengingWrathUptime, double holyAvengerUptime, bool hasSanctifiedWrath, bool hasHolyAvenger, bool hasDivinePurpose, double[] castDistributionTable)
        {
            // Divine Purpose
            if (hasDivinePurpose)
            {
                for (int i = 0; i < castDistributionTable.Length; ++i)
                {
                    castDistributionTable[i] = DPRotationHoW[i] * under20Percent + (1 - under20Percent) * DPRotation[i];
                }
                SotRInterval = (float)(SotRRotationIntervals[4] * (1 - under20Percent) + SotRRotationIntervals[5] * under20Percent);
                return;
            }
            // Default case
            for (int i = 0; i < castDistributionTable.Length; ++i)
            {
                castDistributionTable[i] = NormalRotationHoW[i] * under20Percent + (1 - under20Percent) * NormalRotation[i];
            }
            SotRInterval = (float)(SotRRotationIntervals[0] * (1 - under20Percent) + SotRRotationIntervals[1] * under20Percent);
            // Sanctified Wrath
            if (hasSanctifiedWrath)
            {
                for (int i = 0; i < castDistributionTable.Length; ++i)
                {
                    castDistributionTable[i] = castDistributionTable[i] * (1 - avengingWrathUptime) + avengingWrathUptime *
                        (SWRotation[i] * (1 - under20Percent) + SWRotationHoW[i] * under20Percent);
                }
                SotRInterval = (float)(SotRInterval * (1 - avengingWrathUptime) + (SotRRotationIntervals[2] * (1 - under20Percent) + SotRRotationIntervals[3] * under20Percent) * avengingWrathUptime);
            }
            // Holy Avenger
            else if (hasHolyAvenger)
            {
                for (int i = 0; i < castDistributionTable.Length; ++i)
                {
                    castDistributionTable[i] = castDistributionTable[i] * (1 - holyAvengerUptime) + holyAvengerUptime *
                        (HARotation[i] * (1 - under20Percent) + HARotationHoW[i] * under20Percent);
                }
                SotRInterval = (float)(SotRInterval * (1 - holyAvengerUptime) + (SotRRotationIntervals[6] * (1 - under20Percent) + SotRRotationIntervals[7] * under20Percent) * holyAvengerUptime);
            }
        }

        public float GlancingReduction(int attackerLevel, int targetLevel)
        {
            // The character is a melee class, lowEnd is element of [0.01, 0.91]
            float lowEnd = Math.Max(0.01f, Math.Min(0.91f, 1.3f - (0.05f * (float)(targetLevel - attackerLevel) * 5.0f)));
            // The character is a melee class, highEnd is element of [0.20, 0.99]
            float highEnd = Math.Max(0.20f, Math.Min(0.99f, 1.2f - (0.03f * (float)(targetLevel - attackerLevel) * 5.0f)));

            return (lowEnd + highEnd) / 2.0f;
        }
    }
}
