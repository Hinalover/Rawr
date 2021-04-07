using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum StarfallMode { Unused, LunarOnly, OnCooldown };
    // Rotation information for display to the user.
    public class RotationData
    {
        #region Inputs
        public string Name { get; set; }
        public StarfallMode StarfallCastMode { get; set; }
        #endregion
        #region Outputs
        public float SustainedDPS = 0.0f;
        public float BurstDPS = 0.0f;
        public float DPM = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
        public float AverageInstantCast { get; set; }
        public float Duration { get; set; }
        public float ManaUsed { get; set; }
        public float ManaGained { get; set; }
        public float CastCount { get; set; }
        public float DotTicks { get; set; }
        public float WrathCount { get; set; }
        public float WrathAvgHit { get; set; }
        public float WrathAvgCast { get; set; }
        public float WrathAvgEnergy { get; set; }
        public float StarfireCount { get; set; }
        public float StarfireAvgHit { get; set; }
        public float StarfireAvgCast { get; set; }
        public float StarfireAvgEnergy { get; set; }
        public float StarSurgeCount { get; set; }
        public float StarSurgeAvgHit { get; set; }
        public float StarSurgeAvgCast { get; set; }
        public float StarSurgeAvgEnergy { get; set; }
        public float InsectSwarmTicks { get; set; }
        public float InsectSwarmCasts { get; set; }
        public float InsectSwarmAvgHit { get; set; }
        public float InsectSwarmAvgCast { get; set; }
        public float InsectSwarmDuration { get; set; }
        public float MoonfireCasts { get; set; }
        public float MoonfireTicks { get; set; }
        public float MoonfireAvgHit { get; set; }
        public float MoonfireAvgCast { get; set; }
        public float MoonfireDuration { get; set; }
        public float StarfallCasts { get; set; }
        public float StarfallDamage { get; set; }
        public float StarfallStars { get; set; }
        public float MushroomCasts { get; set; }
        public float MushroomDamage { get; set; }
        public float TreantCasts { get; set; }
        public float TreantDamage { get; set; }
        public float SolarUptime { get; set; }
        public float LunarUptime { get; set; }
        public float NaturesGraceUptime { get; set; }
        #endregion
    }

    // Our old friend the spell rotation.
    public class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        public RotationData RotationData = new RotationData();

        public override string ToString()
        {
            return RotationData.Name;
        }

        // Calculate damage and casting time for a single, direct-damage spell.
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceUptime, float latency)
        {
            float naturesGraceBonusHaste = 0.15f;

            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
            // Add a check for the higher of the two spell schools, as Starsurge always chooses the higher one
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) :
                (mainNuke.School == SpellSchool.Nature ? (1 + calcs.BasicStats.BonusNatureDamageMultiplier) :
                (1 + (calcs.BasicStats.BonusArcaneDamageMultiplier > calcs.BasicStats.BonusNatureDamageMultiplier ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier)));

            float gcd = 1.5f / (1.0f + spellHaste);
            float ngGcd = gcd / (1 + naturesGraceBonusHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            float instantCastNG = (float)Math.Max(ngGcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float baseCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            float ngCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste) / (1 + naturesGraceBonusHaste), instantCastNG);
            mainNuke.CastTime = (1 - naturesGraceUptime) * baseCastTime + naturesGraceUptime * ngCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        // NOTE: Tick calculations have been moved to a different function due to the way the distribution tables work currently.
        public void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceCastUptime, float latency)
        {
            float naturesGraceBonusHaste = 0.15f;

            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;

            float overallDamageModifier = dotSpell.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

            float gcd = 1.5f / (1.0f + spellHaste);
            float ngGcd = gcd / (1 + naturesGraceBonusHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            float instantCastNG = (float)Math.Max(ngGcd, 1.0f) + latency;
            dotSpell.CastTime = naturesGraceCastUptime * instantCastNG + (1 - naturesGraceCastUptime) * instantCast;

            float mfDirectDamage = (dotSpell.BaseDamage + dotSpell.SpellDamageModifier * spellPower) * overallDamageModifier;
            float mfCritDamage = mfDirectDamage * dotSpell.CriticalDamageModifier;
            float totalCritChance = spellCrit + dotSpell.CriticalChanceModifier;
            dotSpell.DamagePerHit = (totalCritChance * mfCritDamage + (1 - totalCritChance) * mfDirectDamage) * spellHit;
        }

        public void DoTickDistribution(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste, float eclipseBonus, float rotationLength, float dotCastCount, double[] tickDistributions, int baseIndex, float latency)
        {
            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;
            float dotEffectDamageModifier = dotSpell.DotEffect.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

            // Use the exact tick length rather than the rounded version so that the conversion from time% to tick# is smooth
            float tickRate = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste);
            float ngTickRate = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste) / 1.15f;

            // Break down cast distribution table
            double baseTicks = tickDistributions[baseIndex] * rotationLength / tickRate / dotCastCount;
            double ngTicks = tickDistributions[baseIndex + 1] * rotationLength / ngTickRate / dotCastCount;
            double eclipseTicks = tickDistributions[baseIndex + 2] * rotationLength / tickRate / dotCastCount;
            double ngEclipseTicks = tickDistributions[baseIndex + 3] * rotationLength / ngTickRate / dotCastCount;

            // Rounded tick lengths should be used after distribution table is broken down
            float roundedTickRate = (float)Math.Round(tickRate, 3);
            float roundedNGTickRate = (float)Math.Round(ngTickRate, 3);

            dotSpell.DotEffect.NumberOfTicks = (float)(baseTicks + ngTicks + eclipseTicks + ngEclipseTicks);
            dotSpell.DotEffect.Duration = (float)((baseTicks + eclipseTicks) * roundedTickRate + (ngTicks + ngEclipseTicks) * roundedNGTickRate);
            dotSpell.DotEffect.TickLength = (float)((ngTicks + ngEclipseTicks) / dotSpell.DotEffect.NumberOfTicks * roundedNGTickRate +
                (baseTicks + eclipseTicks) / dotSpell.DotEffect.NumberOfTicks * roundedTickRate);

            float baseTickDamage = (dotSpell.DotEffect.TickDamage + spellPower * dotSpell.DotEffect.SpellDamageModifierPerTick) * dotEffectDamageModifier;
            float critTickDamage = baseTickDamage * dotSpell.CriticalDamageModifier;
            float averageTickDamage = (1 - spellCrit) * baseTickDamage + spellCrit * critTickDamage;

            dotSpell.DotEffect.DamagePerHit = (float)((baseTicks + ngTicks) * averageTickDamage + (eclipseTicks + ngEclipseTicks) * averageTickDamage * eclipseBonus);
        }

        private float DoMushroomCalcs(CharacterCalculationsMoonkin calcs, float effectiveNatureDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            float critDamageModifier = 1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            // 845-1022 damage
            float baseDamage = (845 + 1022) / 2f;
            float damagePerHit = (baseDamage + effectiveNatureDamage * 0.6032f) * hitDamageModifier;
            float damagePerCrit = damagePerHit * critDamageModifier;
            return spellHit * (damagePerHit * (1 - spellCrit) + damagePerCrit * spellCrit);
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(CharacterCalculationsMoonkin calcs, int playerLevel, int bossLevel, float effectiveNatureDamage, float treantLifespan)
        {
            float sunderPercent = calcs.BasicStats.TargetArmorReduction;
            float meleeHit = calcs.SpellHit * (StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel] / StatConversion.GetSpellMiss(playerLevel - bossLevel, false));
            float physicalDamageMultiplierBonus = (1f + calcs.BasicStats.BonusDamageMultiplier) * (1f + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            float physicalDamageMultiplierReduc = (1f - calcs.BasicStats.DamageTakenReductionMultiplier) * (1f - calcs.BasicStats.PhysicalDamageTakenReductionMultiplier);
            // 932 = base AP, 57% spell power scaling
            float attackPower = 932.0f + (float)Math.Floor(0.57f * effectiveNatureDamage);
            // 1.65 s base swing speed
            float baseAttackSpeed = 1.65f;
            float attackSpeed = baseAttackSpeed / (1 + calcs.BasicStats.PhysicalHaste);
            // 580 = base DPS
            float damagePerHit = (580f + attackPower / 14.0f) * baseAttackSpeed;
            // 5% base crit rate, inherit crit debuffs
            // Remove crit depression, as it doesn't appear to have an effect (unless it's base ~10% crit rate)
            float critRate = 0.05f;
            // White hit glancing rate
            float glancingRate = StatConversion.WHITE_GLANCE_CHANCE_CAP[bossLevel - playerLevel];
            // Hit rate determined by the amount of melee hit, not by spell hit
            float missRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel] - meleeHit);
            // Since the trees inherit expertise from their hit, scale their hit rate such that when they are hit capped, they are expertise capped
            float dodgeRate = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[bossLevel - playerLevel] * (missRate / StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel]));
            // Armor damage reduction, including Sunder
            float damageReduction = StatConversion.GetArmorDamageReduction(playerLevel, StatConversion.NPC_ARMOR[bossLevel - playerLevel] * (1f - sunderPercent), 0, 0);
            // Final normal damage per swing
            damagePerHit *= 1.0f - damageReduction;
            damagePerHit *= physicalDamageMultiplierReduc;
            damagePerHit *= physicalDamageMultiplierBonus;
            // Damage per swing, including crits/glances/misses
            // This is a cheesy approximation of a true combat table, but because crit/miss/dodge rates will all be fairly low, I don't need to do the whole thing
            damagePerHit = (critRate * damagePerHit * 2.0f) + (glancingRate * damagePerHit * 0.75f) + ((1 - critRate - glancingRate - missRate - dodgeRate) * damagePerHit);
            // Total damage done in their estimated lifespan
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit;
            return 3 * damagePerTree;
        }

        // Starfall
        private float DoStarfallCalcs(CharacterCalculationsMoonkin calcs, float effectiveArcaneDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier);
            // Starfall is affected by Moonfury
            float critDamageModifier = 1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier) + (1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier) - 1);
            float baseDamagePerStar = (370.0f + 428.0f) / 2.0f;
            float mainStarCoefficient = 0.247f;

            float damagePerBigStarHit = (baseDamagePerStar + effectiveArcaneDamage * mainStarCoefficient) * hitDamageModifier;

            float critDamagePerBigStarHit = damagePerBigStarHit * critDamageModifier;

            float averageDamagePerBigStar = spellCrit * critDamagePerBigStarHit + (1 - spellCrit) * damagePerBigStarHit;

            float numberOfStarHits = 10f;

            float avgNumBigStarsHit = spellHit * numberOfStarHits;

            return avgNumBigStarsHit * averageDamagePerBigStar;
        }

        private double GetInterpolatedRotationLength(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.001;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 1001)
            {
                i = 999;
                r = 1;
            }
            if (i < 0)
            {
                i = 0;
                r = 0;
            }

            // Index the table and interpolate the remainder
            if (use4T12Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T12RotationDurationsGoSF[i] + r * (MoonkinSolver.T12RotationDurationsGoSF[i + 1] - MoonkinSolver.T12RotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.T12RotationDurations[i] + r * (MoonkinSolver.T12RotationDurations[i + 1] - MoonkinSolver.T12RotationDurations[i]);
            }
            else if (use4T13Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T13RotationDurationsGoSF[i] + r * (MoonkinSolver.T13RotationDurationsGoSF[i + 1] - MoonkinSolver.T13RotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.T13RotationDurations[i] + r * (MoonkinSolver.T13RotationDurations[i + 1] - MoonkinSolver.T13RotationDurations[i]);
            }
            else
            {
                if (useGoSFTable)
                    return MoonkinSolver.BaseRotationDurationsGoSF[i] + r * (MoonkinSolver.BaseRotationDurationsGoSF[i + 1] - MoonkinSolver.BaseRotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.BaseRotationDurations[i] + r * (MoonkinSolver.BaseRotationDurations[i + 1] - MoonkinSolver.BaseRotationDurations[i]);
            }
        }

        private double[] GetInterpolatedTickDistribution(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.001;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 1001)
            {
                i = 999;
                r = 1;
            }
            if (i < 0)
            {
                i = 0;
                r = 0;
            }

            double[] retval = new double[MoonkinSolver.TickDistributionSpells.Length];

            // Index the table and interpolate the remainder
            for (int index = 0; index < MoonkinSolver.TickDistributionSpells.Length; ++index)
            {
                if (use4T12Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T12TickDistributionGoSF[i, index] + r * (MoonkinSolver.T12TickDistributionGoSF[i + 1, index] - MoonkinSolver.T12TickDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T12TickDistribution[i, index] + r * (MoonkinSolver.T12TickDistribution[i + 1, index] - MoonkinSolver.T12TickDistribution[i, index]);
                }
                else if (use4T13Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T13TickDistributionGoSF[i, index] + r * (MoonkinSolver.T13TickDistributionGoSF[i + 1, index] - MoonkinSolver.T13TickDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T13TickDistribution[i, index] + r * (MoonkinSolver.T13TickDistribution[i + 1, index] - MoonkinSolver.T13TickDistribution[i, index]);
                }
                else
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.BaseTickDistributionGoSF[i, index] + r * (MoonkinSolver.BaseTickDistributionGoSF[i + 1, index] - MoonkinSolver.BaseTickDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.BaseTickDistribution[i, index] + r * (MoonkinSolver.BaseTickDistribution[i + 1, index] - MoonkinSolver.BaseTickDistribution[i, index]);
                }
            }

            return retval;
        }

        private double[] GetInterpolatedCastTable(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.001;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 1001)
            {
                i = 999;
                r = 1;
            }
            if (i < 0)
            {
                i = 0;
                r = 0;
            }

            double[] retval = new double[MoonkinSolver.CastDistributionSpells.Length];

            // Index the table and interpolate the remainder
            for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
            {
                if (use4T12Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T12CastDistributionGoSF[i, index] + r * (MoonkinSolver.T12CastDistributionGoSF[i + 1, index] - MoonkinSolver.T12CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T12CastDistribution[i, index] + r * (MoonkinSolver.T12CastDistribution[i + 1, index] - MoonkinSolver.T12CastDistribution[i, index]);
                }
                else if (use4T13Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T13CastDistributionGoSF[i, index] + r * (MoonkinSolver.T13CastDistributionGoSF[i + 1, index] - MoonkinSolver.T13CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T13CastDistribution[i, index] + r * (MoonkinSolver.T13CastDistribution[i + 1, index] - MoonkinSolver.T13CastDistribution[i, index]);
                }
                else
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.CastDistributionGoSF[i, index] + r * (MoonkinSolver.CastDistributionGoSF[i + 1, index] - MoonkinSolver.CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.CastDistribution[i, index] + r * (MoonkinSolver.CastDistribution[i + 1, index] - MoonkinSolver.CastDistribution[i, index]);
                }
            }

            return retval;
        }

        private double[] ConvertCastDistributionToTimeDistribution(double[] castDistribution, double[] castTimes)
        {
            double[] retval = new double[MoonkinSolver.CastDistributionSpells.Length];
            double timeSum = 0;

            for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
            {
                retval[index] = castDistribution[index] * castTimes[index];
                timeSum += retval[index];
            }
            for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
            {
                retval[index] /= timeSum;
            }

            return retval;
        }

        private double[] GetSpellCastTimes(float spellHaste)
        {
            double wrathBaseCastTime = Math.Max(1, Solver.Wrath.BaseCastTime / (1 + spellHaste));
            double wrathNGCastTime = Math.Max(1, wrathBaseCastTime / 1.15f);

            double starfireBaseCastTime = Math.Max(1, Solver.Starfire.BaseCastTime / (1 + spellHaste));
            double starfireNGCastTime = Math.Max(1, starfireBaseCastTime / 1.15f);

            double starsurgeBaseCastTime = Math.Max(1, Solver.Starsurge.BaseCastTime / (1 + spellHaste));
            double starsurgeNGCastTime = Math.Max(1, starsurgeBaseCastTime / 1.15f);

            double instantBaseCastTime = Math.Max(1, Solver.Moonfire.BaseCastTime / (1 + spellHaste));
            double instantNGCastTime = Math.Max(1, instantBaseCastTime / 1.15f);
            return new double[32]
            {
                wrathBaseCastTime,
                wrathBaseCastTime,
                starfireBaseCastTime,
                starfireBaseCastTime,
                starsurgeBaseCastTime,
                starsurgeBaseCastTime,
                starsurgeBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                instantBaseCastTime,
                wrathNGCastTime,
                wrathNGCastTime,
                starfireNGCastTime,
                starfireNGCastTime,
                starsurgeNGCastTime,
                starsurgeNGCastTime,
                starsurgeNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime,
                instantNGCastTime
            };
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(Character character, CharacterCalculationsMoonkin calcs, float treantLifespan, float spellPower, float spellHit, float spellCrit, float spellHaste, float masteryPoints, float latency)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            Spell iSw = Solver.InsectSwarm;

            // 4.1: The bug causing the Eclipse buff to be rounded down to the nearest percent has been fixed
            float eclipseBonus = 1 + MoonkinSolver.ECLIPSE_BASE + masteryPoints * 0.02f;

            // Get the cast distribution first
            double[] castDistribution = GetInterpolatedCastTable(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);
            double[] timeDistribution = ConvertCastDistributionToTimeDistribution(castDistribution, GetSpellCastTimes(spellHaste));
            double[] tickDistribution = GetInterpolatedTickDistribution(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);

            // Do Nature's Grace calculations
            double allWrathPercentage = timeDistribution[0] + timeDistribution[1] + timeDistribution[16] + timeDistribution[17];
            double allStarfirePercentage = timeDistribution[2] + timeDistribution[3] + timeDistribution[18] + timeDistribution[19];
            double allStarsurgePercentage = timeDistribution[4] + timeDistribution[5] + timeDistribution[6] +
                timeDistribution[20] + timeDistribution[21] + timeDistribution[22];
            double allShootingStarsPercentage = timeDistribution[7] + timeDistribution[8] + timeDistribution[9] +
                timeDistribution[23] + timeDistribution[24] + timeDistribution[25];
            double allMoonfirePercentage = timeDistribution[10] + timeDistribution[11] + timeDistribution[12] +
                timeDistribution[26] + timeDistribution[27] + timeDistribution[28];
            double allInsectSwarmPercentage = timeDistribution[13] + timeDistribution[14] + timeDistribution[15] + 
                timeDistribution[29] + timeDistribution[30] + timeDistribution[31];

            double ngWrathPercentage = timeDistribution[16] + timeDistribution[17];
            double ngStarfirePercentage = timeDistribution[18] + timeDistribution[19];
            double ngStarsurgePercentage = timeDistribution[20] + timeDistribution[21] + timeDistribution[22];
            double ngShootingStarsPercentage = timeDistribution[23] + timeDistribution[24] + timeDistribution[25];
            double ngMoonfirePercentage = timeDistribution[26] + timeDistribution[27] + timeDistribution[28];
            double ngInsectSwarmPercentage = timeDistribution[29] + timeDistribution[30] + timeDistribution[31];

            float wrNGAverageUptime = (float)(ngWrathPercentage / allWrathPercentage);
            float sfNGAverageUptime = (float)(ngStarfirePercentage / allStarfirePercentage);
            float ssNGAverageUptime = (float)(ngStarsurgePercentage / allStarsurgePercentage);
            float shsNGAverageUptime = (float)(ngShootingStarsPercentage / allStarsurgePercentage);
            float mfNGAverageUptime = (float)(ngMoonfirePercentage / allMoonfirePercentage);
            float isNGAverageUptime = (float)(ngInsectSwarmPercentage / allInsectSwarmPercentage);

            RotationData.NaturesGraceUptime = (float)(ngWrathPercentage + ngStarfirePercentage + ngStarsurgePercentage + ngShootingStarsPercentage + ngMoonfirePercentage + ngInsectSwarmPercentage);

            // Get the duration
            RotationData.Duration = (float)GetInterpolatedRotationLength(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);

            // Do Lunar/Solar Eclipse uptime calculations
            RotationData.LunarUptime = (float)(timeDistribution[3] + timeDistribution[5] + timeDistribution[8] + timeDistribution[11] + timeDistribution[14] + 
                timeDistribution[19] + timeDistribution[21] + timeDistribution[24] + timeDistribution[27] + timeDistribution[30]);
            RotationData.SolarUptime = (float)(timeDistribution[1] + timeDistribution[6] + timeDistribution[9] + timeDistribution[12] + timeDistribution[15] +
                timeDistribution[17] + timeDistribution[22] + timeDistribution[25] + timeDistribution[28] + timeDistribution[31]);

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, sfNGAverageUptime, 0);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste, ssNGAverageUptime, 0);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, wrNGAverageUptime, 0);
            double gcd = Math.Max(1, 1.5 / (1 + spellHaste)) + 0;
            double ngGcd = Math.Max(1, 1.5 / (1 + spellHaste) / (1 + 0.05 * talents.NaturesGrace)) + 0;

            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, mfNGAverageUptime, 0);
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, isNGAverageUptime, 0);

            RotationData.MoonfireAvgCast = mf.CastTime + latency;
            RotationData.InsectSwarmAvgCast = iSw.CastTime + latency;

            // 4T12: Modify displayed average energy of Wrath/SF
            if (calcs.BasicStats.BonusWrathEnergy > 0)
            {
                float nonEclipsePercent = 1 - RotationData.LunarUptime - RotationData.SolarUptime;
                w.AverageEnergy = w.BaseEnergy * (1 - nonEclipsePercent) + (w.BaseEnergy + calcs.BasicStats.BonusWrathEnergy) * nonEclipsePercent;
                sf.AverageEnergy = sf.BaseEnergy * (1 - nonEclipsePercent) + (sf.BaseEnergy + calcs.BasicStats.BonusStarfireEnergy) * nonEclipsePercent;
            }
            else
            {
                w.AverageEnergy = w.BaseEnergy;
                sf.AverageEnergy = sf.BaseEnergy;
            }

            // Break the cast distribution down into its component cast counts
            double wrathCasts = (timeDistribution[0] + timeDistribution[16]) * RotationData.Duration / w.CastTime;
            double eclipseWrathCasts = (timeDistribution[1] + timeDistribution[17]) * RotationData.Duration / w.CastTime;
            double nonEclipsedWrathPercentage = (timeDistribution[0] + timeDistribution[16]) / allWrathPercentage;
            double eclipsedWrathPercentage = (timeDistribution[1] + timeDistribution[17]) / allWrathPercentage;
            RotationData.WrathAvgHit = (float)(nonEclipsedWrathPercentage * w.DamagePerHit + eclipsedWrathPercentage * w.DamagePerHit * eclipseBonus);
            RotationData.WrathAvgEnergy = w.AverageEnergy;
            RotationData.WrathCount = (float)(wrathCasts + eclipseWrathCasts);
            double starfireCasts = (timeDistribution[2] + timeDistribution[18]) * RotationData.Duration / sf.CastTime;
            double eclipseStarfireCasts = (timeDistribution[3] + timeDistribution[19]) * RotationData.Duration / sf.CastTime;
            double nonEclipsedStarfirePercentage = (timeDistribution[2] + timeDistribution[18]) / allStarfirePercentage;
            double eclipsedStarfirePercentage = (timeDistribution[3] + timeDistribution[19]) / allStarfirePercentage;
            RotationData.StarfireAvgHit = (float)(nonEclipsedStarfirePercentage * sf.DamagePerHit + eclipsedStarfirePercentage * sf.DamagePerHit * eclipseBonus);
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;
            RotationData.StarfireCount = (float)(starfireCasts + eclipseStarfireCasts);
            double starsurgeCasts = (timeDistribution[4] + timeDistribution[20]) * RotationData.Duration / ss.CastTime;
            double eclipseStarsurgeCasts = (timeDistribution[5] + timeDistribution[6] + timeDistribution[21] + timeDistribution[22]) * RotationData.Duration / ss.CastTime;
            double shootingStarsProcs = timeDistribution[7] * RotationData.Duration / gcd + timeDistribution[23] * RotationData.Duration / ngGcd;
            double eclipseShootingStarsProcs = (timeDistribution[8] + timeDistribution[9]) * RotationData.Duration / gcd + (timeDistribution[24] + timeDistribution[25]) * RotationData.Duration / ngGcd;
            double nonEclipsedStarsurgePercentage = (timeDistribution[4] + timeDistribution[7] + timeDistribution[20] + timeDistribution[23]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double lunarEclipsedStarsurgePercentage = (timeDistribution[5] + timeDistribution[8] + timeDistribution[21] + timeDistribution[24]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double solarEclipsedStarsurgePercentage = (timeDistribution[6] + timeDistribution[9] + timeDistribution[22] + timeDistribution[25]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double starsurgePercentage = (timeDistribution[4] + timeDistribution[5] + timeDistribution[6] + timeDistribution[20] + timeDistribution[21] + timeDistribution[22]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double shootingStarsPercentage = (timeDistribution[7] + timeDistribution[8] + timeDistribution[9] + timeDistribution[23] + timeDistribution[24] + timeDistribution[25]) / (allStarsurgePercentage + allShootingStarsPercentage);
            RotationData.StarSurgeAvgHit = (float)(nonEclipsedStarsurgePercentage * ss.DamagePerHit + (lunarEclipsedStarsurgePercentage + solarEclipsedStarsurgePercentage) * ss.DamagePerHit * eclipseBonus);
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeCount = (float)(starsurgeCasts + eclipseStarsurgeCasts + shootingStarsProcs + eclipseShootingStarsProcs);
            double moonfireCasts = (timeDistribution[10] + timeDistribution[26]) * RotationData.Duration / mf.CastTime;
            double lunarEclipsedMoonfireCasts = (timeDistribution[11] + timeDistribution[27]) * RotationData.Duration / mf.CastTime;
            double sunfireCasts = (timeDistribution[12] + timeDistribution[28]) * RotationData.Duration / mf.CastTime;
            double nonEclipsedMoonfirePercentage = (timeDistribution[10] + timeDistribution[26]) / allMoonfirePercentage;
            double eclipsedMoonfirePercentage = (timeDistribution[11] + timeDistribution[27]) / allMoonfirePercentage;
            double sunfirePercentage = (timeDistribution[12] + timeDistribution[28]) / allMoonfirePercentage;
            RotationData.MoonfireCasts = (float)(moonfireCasts + lunarEclipsedMoonfireCasts + sunfireCasts);
            double insectSwarmCasts = (timeDistribution[13] + timeDistribution[29]) * RotationData.Duration / iSw.CastTime;
            double lunarEclipsedInsectSwarmCasts = (timeDistribution[14] + timeDistribution[30]) * RotationData.Duration / iSw.CastTime;
            double solarEclipsedInsectSwarmCasts = (timeDistribution[15] + timeDistribution[31]) * RotationData.Duration / iSw.CastTime;
            double nonEclipsedInsectSwarmPercentage = (timeDistribution[13] + timeDistribution[14] + timeDistribution[29] + timeDistribution[30]) / allInsectSwarmPercentage;
            double eclipsedInsectSwarmPercentage = (timeDistribution[16] + timeDistribution[31]) / allInsectSwarmPercentage;
            RotationData.InsectSwarmCasts = (float)(insectSwarmCasts + lunarEclipsedInsectSwarmCasts + solarEclipsedInsectSwarmCasts);

            DoTickDistribution(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, eclipseBonus, RotationData.Duration, RotationData.MoonfireCasts, tickDistribution, 0, latency);
            DoTickDistribution(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, eclipseBonus, RotationData.Duration, RotationData.InsectSwarmCasts, tickDistribution, 4, latency);

            // Dot Effect damage numbers already include Eclipsed vs. Non-Eclipsed ticks
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;
            RotationData.MoonfireAvgHit = (float)(nonEclipsedMoonfirePercentage * mf.DamagePerHit +
                (eclipsedMoonfirePercentage + sunfirePercentage) * mf.DamagePerHit * eclipseBonus) + mf.DotEffect.DamagePerHit;

            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;
            RotationData.InsectSwarmAvgHit = iSw.DotEffect.DamagePerHit;

            RotationData.StarfireAvgCast = sf.CastTime + latency;
            RotationData.WrathAvgCast = w.CastTime + latency;

            RotationData.AverageInstantCast = (float)(gcd * (1 - RotationData.NaturesGraceUptime) + ngGcd * RotationData.NaturesGraceUptime) + latency;

            RotationData.StarSurgeAvgCast = (float)(starsurgePercentage * (1 - ngStarsurgePercentage) * (Math.Max(1, ss.BaseCastTime / (1 + spellHaste))) + 
                starsurgePercentage * ngStarsurgePercentage * (Math.Max(1, ss.BaseCastTime / (1 + spellHaste) / (1 + 0.05f * talents.NaturesGrace))) +
                shootingStarsPercentage * (1 - ngShootingStarsPercentage) * gcd +
                shootingStarsPercentage * ngShootingStarsPercentage * ngGcd) + latency;

            // Add latency to rotation duration
            RotationData.Duration += (RotationData.MoonfireCasts + RotationData.InsectSwarmCasts + RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount) * latency;

            // Modify the rotation duration to simulate the energy bonus from Dragonwrath procs
            if (calcs.BasicStats.DragonwrathProc > 0)
            {
                float baselineNukeDuration = RotationData.StarfireCount * RotationData.StarfireAvgCast +
                    RotationData.WrathCount * RotationData.WrathAvgCast +
                    RotationData.StarSurgeCount * RotationData.StarSurgeAvgCast;
                float dragonwrathNukeDuration = baselineNukeDuration / (1 + MoonkinSolver.DRAGONWRATH_PROC_RATE);
                RotationData.Duration -= (baselineNukeDuration - dragonwrathNukeDuration);
            }

            float starfallReduction = (float)(starsurgeCasts + shootingStarsProcs + eclipseStarsurgeCasts + eclipseShootingStarsProcs) * 5f;
            float starfallCooldown = (90f - (talents.GlyphOfStarfall ? 30f : 0f)) - (talents.GlyphOfStarsurge ? starfallReduction : 0);
            float starfallRatio = talents.Starfall == 1 ?
                (RotationData.StarfallCastMode == StarfallMode.OnCooldown ? RotationData.AverageInstantCast / (starfallCooldown + RotationData.AverageInstantCast) : 0f)
                : 0f;
            float starfallTime = RotationData.StarfallCastMode == StarfallMode.LunarOnly ? RotationData.AverageInstantCast : 0f;
            float treantRatio = talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (180f + RotationData.AverageInstantCast) : 0;

            float starfallBaseDamage = (talents.Starfall > 0 && RotationData.StarfallCastMode == StarfallMode.Unused) ? 0 : DoStarfallCalcs(calcs, spellPower, spellHit, spellCrit);
            starfallBaseDamage *= 1 + (talents.GlyphOfFocus ? 0.1f : 0f);
            // Dragonwrath
            starfallBaseDamage *= 1 + (calcs.BasicStats.DragonwrathProc > 0 ? MoonkinSolver.DRAGONWRATH_PROC_RATE : 0f);
            float starfallEclipseDamage = starfallBaseDamage * eclipseBonus;
            RotationData.TreantDamage = talents.ForceOfNature == 0 ? 0 : DoTreeCalcs(calcs, character.Level, character.BossOptions.Level, spellPower, treantLifespan);
            // T12 2-piece: 2-sec cast, 5192-6035 damage, affected by hit, 15-sec duration
            float T122PieceHitDamage = (5192 + 6035) / 2f * spellHit * (1 + calcs.BasicStats.BonusFireDamageMultiplier);
            // I'm going to assume a 150% crit modifier on the 2T12 proc until I'm told otherwise
            float T122PieceCritDamage = T122PieceHitDamage * 1.5f;
            // Use 2.5% crit rate based on EJ testing
            // Hard-code 4.5 casts/proc based on EJ testing
            float T122PieceBaseDamage = (0.975f * T122PieceHitDamage + 0.025f * T122PieceCritDamage) * 4.5f;

            // Without glyph of Starsurge, you cannot fit a Starfall in every Lunar eclipse.
            // The actual result will be better than 1/2, because you will be able to cast SFall later in each Eclipse as the fight goes on,
            // but you will miss a Lunar proc entirely eventually.
            // Note: This is causing a major haste breakpoint, so I am removing the fraction calculation and replacing it with a flat 50%.
            float starfallCooldownOverlap = starfallCooldown - RotationData.Duration;
            float rotationsToMiss = starfallCooldownOverlap > 0 ? RotationData.Duration * RotationData.LunarUptime / starfallCooldownOverlap : 0f;
            float starfallFraction = rotationsToMiss > 0 ? 0.5f : 1f;
            RotationData.StarfallCasts = RotationData.StarfallCastMode == StarfallMode.OnCooldown ? starfallRatio * RotationData.Duration / RotationData.AverageInstantCast
                : (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? starfallFraction : 0f);
            RotationData.TreantCasts = treantRatio * RotationData.Duration / RotationData.AverageInstantCast;
            RotationData.StarfallStars = 10f;
            if (RotationData.StarfallCastMode == StarfallMode.LunarOnly)
                RotationData.LunarUptime += starfallFraction * RotationData.AverageInstantCast / RotationData.Duration;
            else if (RotationData.StarfallCastMode == StarfallMode.OnCooldown)
            {
                RotationData.SolarUptime *= 1 + starfallRatio;
                RotationData.LunarUptime *= 1 + starfallRatio;
            }

            RotationData.Duration += RotationData.StarfallCasts * RotationData.AverageInstantCast + RotationData.TreantCasts * RotationData.AverageInstantCast;

            RotationData.StarfallDamage = RotationData.StarfallCastMode == StarfallMode.OnCooldown ?
                RotationData.LunarUptime * starfallEclipseDamage + (1 - RotationData.LunarUptime) * starfallBaseDamage :
                starfallEclipseDamage;

            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            // Calculate total damage done for external cooldowns per rotation
            float starfallDamage = RotationData.StarfallDamage * RotationData.StarfallCasts;
            float treantDamage = RotationData.TreantDamage * RotationData.TreantCasts;
            float T122PieceDamage = 0f;
            if (calcs.BasicStats.ContainsSpecialEffect(se => se.Trigger == Trigger.MageNukeCast))
            {
                foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.Trigger == Trigger.MageNukeCast))
                {
                    T122PieceDamage = T122PieceBaseDamage * effect.GetAverageUptime(RotationData.Duration / (RotationData.WrathCount + RotationData.StarfireCount), 1f);
                }
            }

            // Calculate mana cost per cast.
            // Starfall - 35% of base mana
            float starfallManaCost = (int)(0.35f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;
            // Force of Nature - 12% of base mana
            float treantManaCost = (int)(0.12f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts + RotationData.StarfallCasts + RotationData.TreantCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost +
                RotationData.StarfallCasts * starfallManaCost +
                RotationData.TreantCasts * treantManaCost;

            float manaSavingsFromOOC = MoonkinSolver.OOC_PROC_CHANCE * (RotationData.MoonfireCasts / RotationData.CastCount * mf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.InsectSwarmCasts / RotationData.CastCount * iSw.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfireCount / RotationData.CastCount * sf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.WrathCount / RotationData.CastCount * w.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarSurgeCount / RotationData.CastCount * ss.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfallCasts / RotationData.CastCount * starfallManaCost);

            RotationData.ManaUsed -= manaSavingsFromOOC;

            RotationData.ManaGained = 2 * MoonkinSolver.EUPHORIA_PERCENT * talents.Euphoria * calcs.BasicStats.Mana;

            return RotationData.WrathAvgHit * RotationData.WrathCount +
                RotationData.StarfireAvgHit * RotationData.StarfireCount +
                RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount +
                moonfireDamage + insectSwarmDamage + treantDamage + starfallDamage + T122PieceDamage;
        }
    }
}
