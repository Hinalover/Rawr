using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum MoonfireRefreshMode { AlwaysRefresh, OnlyOnEclipse };
    // Rotation information for display to the user.
    public class RotationData
    {
        #region Inputs
        public string Name { get; set; }
        public MoonfireRefreshMode MoonfireRefreshMode { get; set; }
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
            float damagePerCrit = damagePerNormalHit * (2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier));
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
            float mfCritDamage = mfDirectDamage * 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
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
            float critTickDamage = baseTickDamage * 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            float averageTickDamage = (1 - spellCrit) * baseTickDamage + spellCrit * critTickDamage;

            dotSpell.DotEffect.DamagePerHit = (float)((baseTicks + ngTicks) * averageTickDamage + (eclipseTicks + ngEclipseTicks) * averageTickDamage * eclipseBonus);
        }

        private float DoMushroomCalcs(CharacterCalculationsMoonkin calcs, float effectiveNatureDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            float critDamageModifier = 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            // 845-1022 damage
            float baseDamage = (845 + 1022) / 2f;
            float damagePerHit = (baseDamage + effectiveNatureDamage * 0.6032f) * hitDamageModifier;
            float damagePerCrit = damagePerHit * critDamageModifier;
            return spellHit * (damagePerHit * (1 - spellCrit) + damagePerCrit * spellCrit);
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(CharacterCalculationsMoonkin calcs, int playerLevel, int bossLevel, float effectiveNatureDamage, float treantLifespan)
        {
            float natureDamageMultiplierBonus = (1f + calcs.BasicStats.BonusDamageMultiplier) * (1f + calcs.BasicStats.BonusNatureDamageMultiplier);
            // Base Wrath cast damage is 2052, 0.375 spell power scaling
            float damagePerHit = 2052 + 0.375f * effectiveNatureDamage;
            // 2.5 s cast time, affected by 100% of druid's haste
            float castTime = Math.Max(0.5f, 2.5f / (1 + calcs.SpellHaste));
            // Inherits 100% of the druid's crit rate
            float critRate = calcs.SpellCrit;
            // Hit and expertise conversion works out to equal the druid's overall spell hit
            float missRate = Math.Max(0, calcs.SpellHitCap - calcs.SpellHit);
            // Apply the nature damage multiplier
            damagePerHit *= natureDamageMultiplierBonus;
            // Damage per cast, including misses
            damagePerHit = (critRate * damagePerHit * 2.0f)  + ((1 - critRate) * damagePerHit) * (1 - missRate);
            // Total damage done in their estimated lifespan
            float damagePerTree = (treantLifespan * (float)Math.Floor(15f / castTime)) * damagePerHit;
            return 3 * damagePerTree;
        }

        // Starfall
        private float DoStarfallCalcs(CharacterCalculationsMoonkin calcs, float effectiveArcaneDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier)
                * (1 + calcs.BasicStats.BonusStarfallDamageModifier);
            // Starfall is affected by Moonfury
            float critDamageModifier = 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            float baseDamagePerStar = (588f + 682f) / 2.0f;
            float mainStarCoefficient = 0.363f;

            float damagePerBigStarHit = (baseDamagePerStar + effectiveArcaneDamage * mainStarCoefficient) * hitDamageModifier;

            float critDamagePerBigStarHit = damagePerBigStarHit * critDamageModifier;

            float averageDamagePerBigStar = spellCrit * critDamagePerBigStarHit + (1 - spellCrit) * damagePerBigStarHit;

            float numberOfStarHits = 10f;

            float avgNumBigStarsHit = spellHit * numberOfStarHits;

            return avgNumBigStarsHit * averageDamagePerBigStar;
        }

        // Celestial Alignment
        public float DoCelestialAlignmentCalcs(CharacterCalculationsMoonkin calcs, DruidTalents talents, float spellPower, float spellHit, float spellCrit, float spellHaste, float masteryPoints, float latency)
        {
            float caDuration = 15f;
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            float eclipseBonus = (1 + MoonkinSolver.ECLIPSE_BASE_PERCENT + masteryPoints * 0.01875f) * (1 + calcs.BasicStats.BonusEclipseDamageMultiplier) * (1 + (talents.NaturesVigil > 0 ? 0.12f : 0f));
            float accumulatedDuration = 0f;

            float sfCastTime = Math.Max(1f, sf.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float ssCastTime = Math.Max(1f, ss.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float wCastTime = Math.Max(1f, w.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float mfCastTime = Math.Max(1f, mf.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            // Add a GCD for activation
            accumulatedDuration += mfCastTime;

            // Moonfire/Sunfire calculations
            float mfTickRate = (float)Math.Round(2f / (1 + spellHaste) / 1.15f, 3);
            float mfTicks = 2 * (15f - mfCastTime) / mfTickRate;
            float mfCasts = 1f;
            accumulatedDuration += mfCasts * mfCastTime;

            // Starsurge and Shooting Stars calculations
            float hardCastStarsurge = 1f;
            float shootingStars = mfTicks * 0.3f * spellCrit;
            accumulatedDuration += hardCastStarsurge * ssCastTime + shootingStars * mfCastTime;

            float starsurgeCritCount = (hardCastStarsurge + shootingStars) * spellCrit;

            // Wrath calculations - only cast one Wrath at the end of CA, to refresh Sunfire
            // Made irrelevant by the changes to dot refreshing, better to continue spamming Starfire
            //float wrathCasts = 1f;
            //accumulatedDuration += wrathCasts * w.CastTime;

            // Starfall calculations
            float starfallCasts = 1f;
            float starfallDamage = DoStarfallCalcs(calcs, spellPower, spellHit, spellCrit);
            accumulatedDuration += starfallCasts * mfCastTime;

            // Starfire calculations - Starfire is the main nuke during CA
            float starfireCasts = (caDuration - accumulatedDuration) / sfCastTime;
            accumulatedDuration += starfireCasts * sfCastTime;

            float starfireCritCount = starfireCasts * spellCrit;

            // Bonus dot ticks after CA expires
            // Each SS crit yields 2 bonus ticks (one of each), each SF crit 1 bonus tick
            float baseMoonfireDuration = 14f + calcs.BasicStats.BonusMoonfireDuration;
            float timeRemaining = Math.Max(0, baseMoonfireDuration - 15f - mfCastTime);
            float bonusMFTicks = 2 * (timeRemaining / mfTickRate) + 2 * starsurgeCritCount + starfireCritCount;

            float retval = (starfireCasts * sf.DamagePerHit +
                mfCasts * mf.DamagePerHit * (1 + calcs.BasicStats.BonusMoonfireDamageMultiplier) +
                (hardCastStarsurge + shootingStars) * ss.DamagePerHit +
                (mfTicks + bonusMFTicks) * (mf.DotEffect.DamagePerHit / mf.DotEffect.NumberOfTicks) * (1 + calcs.BasicStats.BonusMoonfireDamageMultiplier) +
                starfallCasts * starfallDamage) * eclipseBonus;

            return retval;
        }

        /// <summary>
        /// Perform a Lagrangian polynomial interpolation on the specified table at a given value.
        /// </summary>
        /// <param name="x">The value at which to evaluate the polynomial.</param>
        /// <param name="y">The set of Y values that correspond to the nodes in the InterpolationPoints table.</param>
        /// <returns></returns>
        private double LagrangeInterpolation(double x, double[] y)
        {
            double retval = 0;

            for (int i = 0; i < CastDistributions.InterpolationPoints.Length; ++i)
            {
                double numerator = 1;
                double denominator = 1;

                for (int c = 0; c < CastDistributions.InterpolationPoints.Length; ++c)
                {
                    if (c == i) continue;
                    numerator *= x - CastDistributions.InterpolationPoints[c];
                    denominator *= CastDistributions.InterpolationPoints[i] - CastDistributions.InterpolationPoints[c];
                }

                retval += y[i] * (numerator / denominator);
            }

            return retval;
        }

        private double GetInterpolatedRotationLength(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable)
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

            // Derive the Chebyshev node table for crit
            double[] critTable = new double[CastDistributions.InterpolationPoints.Length];
            // Obtain a reference to the correct source table
            double[,] sourceTable = null;
            if (usePTRTable)
            {
                if (useT14Table)
                    if (useSoulOfTheForestTable)
                        sourceTable = PTRCastDistributions.AlwaysRefreshT14SoulRotationDurations;
                    else
                        sourceTable = PTRCastDistributions.AlwaysRefreshT14BaseRotationDurations;
                else
                    if (useT15Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15SoulRotationDurations;
                        else
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15BaseRotationDurations;
                    else
                        if (useT16Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = PTRCastDistributions.AlwaysRefreshT16SoulRotationDurations;
                            else
                                sourceTable = PTRCastDistributions.AlwaysRefreshT16BaseRotationDurations;
                        else
                            if (useSoulOfTheForestTable)
                                sourceTable = PTRCastDistributions.AlwaysRefreshSoulRotationDurations;
                            else
                                sourceTable = PTRCastDistributions.AlwaysRefreshBaseRotationDurations;
            }
            else
            {
                if (useT14Table)
                    if (useSoulOfTheForestTable)
                        sourceTable = CastDistributions.AlwaysRefreshT14SoulRotationDurations;
                    else
                        sourceTable = CastDistributions.AlwaysRefreshT14BaseRotationDurations;
                else
                    if (useT15Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = CastDistributions.AlwaysRefreshT15SoulRotationDurations;
                        else
                            sourceTable = CastDistributions.AlwaysRefreshT15BaseRotationDurations;
                    else
                        if (useT16Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshT16SoulRotationDurations;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshT16BaseRotationDurations;
                        else
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshSoulRotationDurations;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshBaseRotationDurations;
            }

            for (int idx = 0; idx < CastDistributions.InterpolationPoints.Length; ++idx)
            {
                critTable[idx] = sourceTable[i, idx] + r * (sourceTable[i + 1, idx] - sourceTable[i, idx]);
            }

            // Perform a Lagrange polynomial interpolation on the node table
            double retval = LagrangeInterpolation((double)actualCrit, critTable);

            return retval;
        }

        private double[] GetInterpolatedTickDistribution(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable)
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

            double[] retval = new double[CastDistributions.TickDistributionSpells.Length];
            double[] critTable = new double[CastDistributions.InterpolationPoints.Length];
            // Obtain a reference to the correct source table
            double[,,] sourceTable = null;
            if (usePTRTable)
            {
                    if (useT14Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = PTRCastDistributions.AlwaysRefreshT14SoulTickDistribution;
                        else
                            sourceTable = PTRCastDistributions.AlwaysRefreshT14BaseTickDistribution;
                    else
                        if (useT15Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = PTRCastDistributions.AlwaysRefreshT15SoulTickDistribution;
                            else
                                sourceTable = PTRCastDistributions.AlwaysRefreshT15BaseTickDistribution;
                        else
                            if (useT16Table)
                                if (useSoulOfTheForestTable)
                                    sourceTable = PTRCastDistributions.AlwaysRefreshT16SoulTickDistribution;
                                else
                                    sourceTable = PTRCastDistributions.AlwaysRefreshT16BaseTickDistribution;
                            else
                                if (useSoulOfTheForestTable)
                                    sourceTable = PTRCastDistributions.AlwaysRefreshSoulTickDistribution;
                                else
                                    sourceTable = PTRCastDistributions.AlwaysRefreshBaseTickDistribution;
            }
            else
            {
                if (useT14Table)
                    if (useSoulOfTheForestTable)
                        sourceTable = CastDistributions.AlwaysRefreshT14SoulTickDistribution;
                    else
                        sourceTable = CastDistributions.AlwaysRefreshT14BaseTickDistribution;
                else
                    if (useT15Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = CastDistributions.AlwaysRefreshT15SoulTickDistribution;
                        else
                            sourceTable = CastDistributions.AlwaysRefreshT15BaseTickDistribution;
                    else
                        if (useT16Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshT16SoulTickDistribution;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshT16BaseTickDistribution;
                        else
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshSoulTickDistribution;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshBaseTickDistribution;
            }

            // Index the table and interpolate the remainder
            for (int index = 0; index < CastDistributions.TickDistributionSpells.Length; ++index)
            {
                // Derive the Chebyshev node table for crit
                for (int idx = 0; idx < CastDistributions.InterpolationPoints.Length; ++idx)
                {
                    critTable[idx] = sourceTable[i, index, idx] + r * (sourceTable[i + 1, index, idx] - sourceTable[i, index, idx]);
                }

                // Perform a Lagrange polynomial interpolation on the node table
                retval[index] = LagrangeInterpolation((double)actualCrit, critTable);
            }

            return retval;
        }

        private double[] GetInterpolatedTimeTable(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable)
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

            double[] retval = new double[CastDistributions.CastDistributionSpells.Length];
            double[] critTable = new double[CastDistributions.InterpolationPoints.Length];
            // Obtain a reference to the correct source table
            double[, ,] sourceTable = null;
            if (usePTRTable)
            {
                if (useT14Table)
                    if (useSoulOfTheForestTable)
                        sourceTable = PTRCastDistributions.AlwaysRefreshT14SoulTimeDistribution;
                    else
                        sourceTable = PTRCastDistributions.AlwaysRefreshT14BaseTimeDistribution;
                else
                    if (useT15Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15SoulTimeDistribution;
                        else
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15BaseTimeDistribution;
                    else
                        if (useT16Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = PTRCastDistributions.AlwaysRefreshT16SoulTimeDistribution;
                            else
                                sourceTable = PTRCastDistributions.AlwaysRefreshT16BaseTimeDistribution;
                        else
                            if (useSoulOfTheForestTable)
                                sourceTable = PTRCastDistributions.AlwaysRefreshSoulTimeDistribution;
                            else
                                sourceTable = PTRCastDistributions.AlwaysRefreshBaseTimeDistribution;
            }
            else
            {
                if (useT14Table)
                    if (useSoulOfTheForestTable)
                        sourceTable = CastDistributions.AlwaysRefreshT14SoulTimeDistribution;
                    else
                        sourceTable = CastDistributions.AlwaysRefreshT14BaseTimeDistribution;
                else
                    if (useT15Table)
                        if (useSoulOfTheForestTable)
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15SoulTimeDistribution;
                        else
                            sourceTable = PTRCastDistributions.AlwaysRefreshT15BaseTimeDistribution;
                    else
                        if (useT16Table)
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshT16SoulTimeDistribution;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshT16BaseTimeDistribution;
                        else
                            if (useSoulOfTheForestTable)
                                sourceTable = CastDistributions.AlwaysRefreshSoulTimeDistribution;
                            else
                                sourceTable = CastDistributions.AlwaysRefreshBaseTimeDistribution;
            }

            // Index the table and interpolate the remainder
            for (int index = 0; index < CastDistributions.CastDistributionSpells.Length; ++index)
            {
                // Derive the Chebyshev node table for crit
                for (int idx = 0; idx < CastDistributions.InterpolationPoints.Length; ++idx)
                {
                    critTable[idx] = sourceTable[i, index, idx] + r * (sourceTable[i + 1, index, idx] - sourceTable[i, index, idx]);
                }

                // Perform a Lagrange polynomial interpolation on the node table
                retval[index] = LagrangeInterpolation((double)actualCrit, critTable);
            }

            return retval;
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

            // 4.1: The bug causing the Eclipse buff to be rounded down to the nearest percent has been fixed
            float eclipseBonus = 1 + MoonkinSolver.ECLIPSE_BASE_PERCENT + masteryPoints * 0.01875f;

            // Get the cast distribution first
            double[] timeDistribution = GetInterpolatedTimeTable(calcs.SpellHaste, calcs.SpellCrit, talents.SoulOfTheForest > 0, calcs.BasicStats.BonusMoonfireDuration > 0, calcs.BasicStats.BonusStarsurgeCritModifier > 0, calcs.BasicStats.BonusShootingStarsChance > 0, calcs.PTRMode);
            double[] tickDistribution = GetInterpolatedTickDistribution(calcs.SpellHaste, calcs.SpellCrit, talents.SoulOfTheForest > 0, calcs.BasicStats.BonusMoonfireDuration > 0, calcs.BasicStats.BonusStarsurgeCritModifier > 0, calcs.BasicStats.BonusShootingStarsChance > 0, calcs.PTRMode);

            // Do Nature's Grace calculations
            double allWrathPercentage = timeDistribution[0] + timeDistribution[1] + timeDistribution[13] + timeDistribution[14];
            double allStarfirePercentage = timeDistribution[2] + timeDistribution[3] + timeDistribution[15] + timeDistribution[16];
            double allStarsurgePercentage = timeDistribution[4] + timeDistribution[5] + timeDistribution[6] +
                timeDistribution[17] + timeDistribution[18] + timeDistribution[19];
            double allShootingStarsPercentage = timeDistribution[7] + timeDistribution[8] + timeDistribution[9] +
                timeDistribution[20] + timeDistribution[21] + timeDistribution[22];
            double allMoonfirePercentage = timeDistribution[10] + timeDistribution[11] + timeDistribution[12] +
                timeDistribution[23] + timeDistribution[24] + timeDistribution[25];

            double ngWrathPercentage = timeDistribution[13] + timeDistribution[14];
            double ngStarfirePercentage = timeDistribution[15] + timeDistribution[16];
            double ngStarsurgePercentage = timeDistribution[17] + timeDistribution[18] + timeDistribution[19];
            double ngShootingStarsPercentage = timeDistribution[20] + timeDistribution[21] + timeDistribution[22];
            double ngMoonfirePercentage = timeDistribution[23] + timeDistribution[24] + timeDistribution[25];

            float wrNGAverageUptime = (float)Math.Min(1, (ngWrathPercentage / allWrathPercentage));
            float sfNGAverageUptime = (float)Math.Min(1, (ngStarfirePercentage / allStarfirePercentage));
            float ssNGAverageUptime = (float)Math.Min(1, (ngStarsurgePercentage / allStarsurgePercentage));
            float shsNGAverageUptime = (float)Math.Min(1, (ngShootingStarsPercentage / allShootingStarsPercentage));
            float mfNGAverageUptime = (float)Math.Min(1, (ngMoonfirePercentage / allMoonfirePercentage));

            RotationData.NaturesGraceUptime = (float)Math.Min(1, (ngWrathPercentage + ngStarfirePercentage + ngStarsurgePercentage + ngShootingStarsPercentage + ngMoonfirePercentage));

            // Get the duration
            RotationData.Duration = (float)GetInterpolatedRotationLength(calcs.SpellHaste, calcs.SpellCrit, talents.SoulOfTheForest > 0, calcs.BasicStats.BonusMoonfireDuration > 0, calcs.BasicStats.BonusStarsurgeCritModifier > 0, calcs.BasicStats.BonusShootingStarsChance > 0, calcs.PTRMode);

            double gcd = Math.Max(1, 1.5 / (1 + spellHaste)) + 0;
            double ngGcd = Math.Max(1, 1.5 / (1 + spellHaste) / (1.15f)) + 0;

            // Do Lunar/Solar Eclipse uptime calculations
            RotationData.LunarUptime = (float)(timeDistribution[3] + timeDistribution[5] + timeDistribution[8] + timeDistribution[11] + 
                timeDistribution[16] + timeDistribution[18] + timeDistribution[21] + timeDistribution[24]);
            RotationData.SolarUptime = (float)(timeDistribution[1] + timeDistribution[6] + timeDistribution[9] + timeDistribution[12] +
                timeDistribution[14] + timeDistribution[19] + timeDistribution[22] + timeDistribution[25]);

            RotationData.AverageInstantCast = (float)(gcd * (1 - RotationData.NaturesGraceUptime) + ngGcd * RotationData.NaturesGraceUptime) + latency;

            // Calculate the percentage of Incarnation that is wasted
            // The Celestial Alignment figures already factor in Incarnation
            // Multiply in Nature's Vigil, if the druid took it
            float lunarLength = RotationData.Duration * RotationData.LunarUptime;
            float solarLength = RotationData.Duration * RotationData.SolarUptime;
            if (talents.Incarnation > 0)
            {
                CalculationsMoonkin._SE_INCARNATION.Duration = Math.Min(15f, (solarLength + lunarLength) / 2f);
                eclipseBonus *= 1 + ((1 + calcs.BasicStats.BonusEclipseDamageMultiplier) * (1 + (talents.NaturesVigil > 0 ? 0.12f : 0f)) - 1) * 
                    (CalculationsMoonkin._SE_INCARNATION.GetAverageUptime(0, 1) / ((RotationData.LunarUptime + RotationData.SolarUptime) / 2f));
            }
            // Dream of Cenarius
            if (calcs.BasicStats.BonusMoonfireDamageMultiplier > 0)
            {
                eclipseBonus += 0.25f;
            }

            // 5.4: 2T16 set bonus calculations
            float T16Base = (478f + 615f) / 2f;
            float T16SpellpowerScaling = 0.1f;
            float T16TotalScaling = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            float T16ScaledDamage = (T16Base + T16SpellpowerScaling * spellPower) * T16TotalScaling;
            // T16 seems to have a 1.5% crit rate (just guessing)
            float T16DamagePerHit = calcs.BasicStats.T16TwoPieceActive ? 0.985f * T16ScaledDamage + 0.015f * T16ScaledDamage * 2 : 0f;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, sfNGAverageUptime, 0);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit + calcs.BasicStats.BonusStarsurgeCritModifier, spellHaste, ssNGAverageUptime, 0);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, wrNGAverageUptime, 0);

            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, mfNGAverageUptime, 0);

            RotationData.MoonfireAvgCast = mf.CastTime + latency;

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
            double wrathCasts = (timeDistribution[0] + timeDistribution[13]) * RotationData.Duration / w.CastTime;
            double eclipseWrathCasts = (timeDistribution[1] + timeDistribution[14]) * RotationData.Duration / w.CastTime;
            double nonEclipsedWrathPercentage = (timeDistribution[0] + timeDistribution[13]) / allWrathPercentage;
            double eclipsedWrathPercentage = (timeDistribution[1] + timeDistribution[14]) / allWrathPercentage;
            RotationData.WrathAvgHit = (float)(nonEclipsedWrathPercentage * w.DamagePerHit + eclipsedWrathPercentage * (w.DamagePerHit + T16DamagePerHit) * eclipseBonus);
            RotationData.WrathAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.WrathAvgEnergy = w.AverageEnergy;
            RotationData.WrathCount = (float)(wrathCasts + eclipseWrathCasts);
            double starfireCasts = (timeDistribution[2] + timeDistribution[15]) * RotationData.Duration / sf.CastTime;
            double eclipseStarfireCasts = (timeDistribution[3] + timeDistribution[16]) * RotationData.Duration / sf.CastTime;
            double nonEclipsedStarfirePercentage = (timeDistribution[2] + timeDistribution[15]) / allStarfirePercentage;
            double eclipsedStarfirePercentage = (timeDistribution[3] + timeDistribution[16]) / allStarfirePercentage;
            RotationData.StarfireAvgHit = (float)(nonEclipsedStarfirePercentage * sf.DamagePerHit + eclipsedStarfirePercentage * (sf.DamagePerHit + T16DamagePerHit) * eclipseBonus);
            RotationData.StarfireAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;
            RotationData.StarfireCount = (float)(starfireCasts + eclipseStarfireCasts);
            double starsurgeCasts = (timeDistribution[4] + timeDistribution[17]) * RotationData.Duration / ss.CastTime;
            double eclipseStarsurgeCasts = (timeDistribution[5] + timeDistribution[6] + timeDistribution[18] + timeDistribution[19]) * RotationData.Duration / ss.CastTime;
            double shootingStarsProcs = timeDistribution[7] * RotationData.Duration / gcd + timeDistribution[20] * RotationData.Duration / ngGcd;
            double eclipseShootingStarsProcs = (timeDistribution[8] + timeDistribution[9]) * RotationData.Duration / gcd + (timeDistribution[21] + timeDistribution[22]) * RotationData.Duration / ngGcd;
            double nonEclipsedStarsurgePercentage = (timeDistribution[4] + timeDistribution[7] + timeDistribution[17] + timeDistribution[20]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double lunarEclipsedStarsurgePercentage = (timeDistribution[5] + timeDistribution[8] + timeDistribution[18] + timeDistribution[21]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double solarEclipsedStarsurgePercentage = (timeDistribution[6] + timeDistribution[9] + timeDistribution[19] + timeDistribution[22]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double starsurgePercentage = (timeDistribution[4] + timeDistribution[5] + timeDistribution[6] + timeDistribution[17] + timeDistribution[18] + timeDistribution[19]) / (allStarsurgePercentage + allShootingStarsPercentage);
            double shootingStarsPercentage = (timeDistribution[7] + timeDistribution[8] + timeDistribution[9] + timeDistribution[20] + timeDistribution[21] + timeDistribution[22]) / (allStarsurgePercentage + allShootingStarsPercentage);
            RotationData.StarSurgeAvgHit = (float)(nonEclipsedStarsurgePercentage * ss.DamagePerHit + (lunarEclipsedStarsurgePercentage + solarEclipsedStarsurgePercentage) * (ss.DamagePerHit + T16DamagePerHit) * eclipseBonus);
            RotationData.StarSurgeAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeCount = (float)(starsurgeCasts + eclipseStarsurgeCasts + shootingStarsProcs + eclipseShootingStarsProcs);
            double moonfireCasts = (timeDistribution[10] + timeDistribution[23]) * RotationData.Duration / mf.CastTime;
            double lunarEclipsedMoonfireCasts = (timeDistribution[11] + timeDistribution[24]) * RotationData.Duration / mf.CastTime;
            double sunfireCasts = (timeDistribution[12] + timeDistribution[25]) * RotationData.Duration / mf.CastTime;
            double nonEclipsedMoonfirePercentage = (timeDistribution[10] + timeDistribution[23]) / allMoonfirePercentage;
            double eclipsedMoonfirePercentage = (timeDistribution[11] + timeDistribution[24]) / allMoonfirePercentage;
            double sunfirePercentage = (timeDistribution[12] + timeDistribution[25]) / allMoonfirePercentage;
            RotationData.MoonfireCasts = (float)(moonfireCasts + lunarEclipsedMoonfireCasts + sunfireCasts);

            // Dream of Cenarius (5.4 mode)
            if (calcs.BasicStats.BonusMoonfireDamageMultiplier > 0)
            {
                // Add a Healing Touch cast to the rotation duration
                // Added a Nature's Swiftness every 4 HT casts; this may be inaccurate
                float healingTouchBaseCastTime = 2.5f / (1 + spellHaste) + latency;
                float healingTouchNGCastTime = 2.5f / (1 + spellHaste) / 1.15f + latency;
                float healingTouchNSCastTime = 1.5f / (1 + spellHaste) / 1.15f + latency;
                float healingTouchCastTime = (3 * healingTouchNGCastTime + healingTouchNSCastTime) / 4f;
                float healingTouchCount = 2;
                float totalHealingTouchTime = healingTouchCount * healingTouchCastTime;
                RotationData.Duration += totalHealingTouchTime;
            }
            // Dream of Cenarius (legacy)
            float moonfireMultiplier = 1f;
            /*if (!calcOpts.PTRMode && calcs.BasicStats.BonusMoonfireDamageMultiplier > 0)
            {
                // Add a Healing Touch cast to the rotation duration
                float healingTouchBaseCastTime = 2.5f / (1 + spellHaste) + (talents.GlyphofTheMoonbeast ? 0 : (float)gcd) + latency;
                float healingTouchNGCastTime = 2.5f / (1 + spellHaste) / 1.15f + (talents.GlyphofTheMoonbeast ? 0 : (float)ngGcd) + latency;
                float healingTouchCastTime = RotationData.NaturesGraceUptime * healingTouchNGCastTime + (1 - RotationData.NaturesGraceUptime) * healingTouchBaseCastTime;
                float healingTouchCount = (RotationData.MoonfireRefreshMode == MoonfireRefreshMode.AlwaysRefresh ? (RotationData.MoonfireCasts / 2f) : 1);
                float totalHealingTouchTime = healingTouchCount * healingTouchCastTime;
                // Nature's Swiftness makes X% of your Healing Touches instant
                if (talents.NaturesSwiftness > 0)
                {
                    float percentAffected = RotationData.Duration / 60.0f;
                    totalHealingTouchTime = (1 - percentAffected) * healingTouchCastTime + percentAffected * RotationData.AverageInstantCast;
                }
                RotationData.Duration += totalHealingTouchTime;
                moonfireMultiplier = 1 + calcs.BasicStats.BonusMoonfireDamageMultiplier;
            }*/

            DoTickDistribution(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, eclipseBonus, RotationData.Duration, RotationData.MoonfireCasts, tickDistribution, 0, latency);

            // Dot Effect damage numbers already include Eclipsed vs. Non-Eclipsed ticks
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;
            RotationData.MoonfireAvgHit = (float)(nonEclipsedMoonfirePercentage * mf.DamagePerHit +
                (eclipsedMoonfirePercentage + sunfirePercentage) * (mf.DamagePerHit + T16DamagePerHit) * eclipseBonus) * (1 + calcs.BasicStats.MultistrikeProc) + mf.DotEffect.DamagePerHit;

            RotationData.MoonfireAvgHit *= moonfireMultiplier;

            RotationData.StarfireAvgCast = sf.CastTime + latency;
            RotationData.WrathAvgCast = w.CastTime + latency;

            RotationData.StarSurgeAvgCast = (float)(starsurgePercentage * (1 - ngStarsurgePercentage) * (Math.Max(1, ss.BaseCastTime / (1 + spellHaste))) + 
                starsurgePercentage * ngStarsurgePercentage * (Math.Max(1, ss.BaseCastTime / (1 + spellHaste) / (1.15f))) +
                shootingStarsPercentage * (1 - ngShootingStarsPercentage) * gcd +
                shootingStarsPercentage * ngShootingStarsPercentage * ngGcd) + latency;

            RotationData.Duration += (RotationData.MoonfireCasts + RotationData.StarfireCount + RotationData.StarSurgeCount + RotationData.WrathCount) * latency;

            // Starfall calculations
            float starfallBaseDamage = DoStarfallCalcs(calcs, spellPower, spellHit, spellCrit);
            // Dragonwrath
            starfallBaseDamage *= 1 + (calcs.BasicStats.DragonwrathProc > 0 ? MoonkinSolver.DRAGONWRATH_PROC_RATE : 0f);
            float starfallEclipseDamage = starfallBaseDamage * eclipseBonus;
            // Starfall is automatically off cooldown on every Lunar Eclipse
            RotationData.StarfallCasts = 1;
            RotationData.StarfallDamage = starfallEclipseDamage;
            RotationData.StarfallStars = 10;
            // Starfall casts will always be made under Nature's Grace
            RotationData.Duration += RotationData.StarfallCasts * (float)(ngGcd + latency);
            // Treant calculations
            RotationData.TreantDamage = talents.ForceOfNature == 0 ? 0 : DoTreeCalcs(calcs, character.Level, character.BossOptions.Level, spellPower, treantLifespan);
            RotationData.TreantCasts = talents.ForceOfNature == 0 ? 0 : RotationData.Duration / 60f;
            RotationData.Duration += RotationData.TreantCasts * RotationData.AverageInstantCast;

            // Incarnation - 1 full (non-hasted) GCD every 3 minutes
            RotationData.Duration += talents.Incarnation > 0 ? 1.5f / (180f / RotationData.Duration) : 0f;

            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;

            // Calculate total damage done for external cooldowns per rotation
            float starfallDamage = RotationData.StarfallDamage * RotationData.StarfallCasts;
            float treantDamage = RotationData.TreantDamage * RotationData.TreantCasts;

            // Calculate mana cost per cast.
            // Starfall - 32.6% of base mana
            float starfallManaCost = (int)(0.326f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;
            // Force of Nature - 10.3% of base mana
            float treantManaCost = (int)(0.103f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.StarfallCasts + RotationData.TreantCasts;
            RotationData.DotTicks = RotationData.MoonfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.StarfallCasts * starfallManaCost +
                RotationData.TreantCasts * treantManaCost;

            RotationData.ManaGained = 2 * MoonkinSolver.ECLIPSE_MANA_PERCENT * calcs.BasicStats.Mana;

            return (RotationData.WrathAvgHit * RotationData.WrathCount +
                RotationData.StarfireAvgHit * RotationData.StarfireCount +
                RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount +
                moonfireDamage + starfallDamage) + treantDamage;
        }
    }
}
