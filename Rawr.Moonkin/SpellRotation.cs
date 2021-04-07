using System;
using System.Collections.Generic;
using System.Linq;

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
        public float MoonfireAvgEnergy { get; set; }
        public float SunfireCasts { get; set; }
        public float SunfireTicks { get; set; }
        public float SunfireAvgHit { get; set; }
        public float SunfireAvgCast { get; set; }
        public float SunfireDuration { get; set; }
        public float SunfireAvgEnergy { get; set; }
        public float InsectSwarmCasts { get; set; }
        public float InsectSwarmTicks { get; set; }
        public float InsectSwarmAvgHit { get; set; }
        public float InsectSwarmAvgCast { get; set; }
        public float InsectSwarmDuration { get; set; }
        public float StarfallCasts { get; set; }
        public float StarfallDamage { get; set; }
        public float StarfallStars { get; set; }
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
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellCrit, float spellHaste, float multistrike, float versatility, float latency)
        {
            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
            // Add a check for the higher of the two spell schools, as Starsurge always chooses the higher one
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) :
                (mainNuke.School == SpellSchool.Nature ? (1 + calcs.BasicStats.BonusNatureDamageMultiplier) :
                (1 + (calcs.BasicStats.BonusArcaneDamageMultiplier > calcs.BasicStats.BonusNatureDamageMultiplier ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier)));

            overallDamageModifier *= 1 + versatility;

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float baseCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            mainNuke.CastTime = baseCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * (2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier));
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit);
            float damagePerMultistrike = damagePerNormalHit * 0.3f;
            mainNuke.DamagePerHit += (multistrike + (multistrike * multistrike)) * damagePerMultistrike;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
        }

        public void DoTickDistribution(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellCrit, float spellHaste, float versatility, float eclipseBonus, double[] tickDistributions, int baseIndex, float latency)
        {
            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;
            float dotEffectDamageModifier = dotSpell.DotEffect.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier) * (1 + versatility);

            // Use the exact tick length rather than the rounded version so that the conversion from time% to tick# is smooth
            float tickRate = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste);

            // Break down cast distribution table
            double baseTicks = tickDistributions[baseIndex];

            // Rounded tick lengths should be used after distribution table is broken down
            float roundedTickRate = (float)Math.Round(tickRate, 3);

            dotSpell.DotEffect.NumberOfTicks = (float)(baseTicks);
            dotSpell.DotEffect.Duration = dotSpell.DotEffect.BaseDuration;
            dotSpell.DotEffect.TickLength = (float)((baseTicks) / dotSpell.DotEffect.NumberOfTicks * roundedTickRate);

            float baseTickDamage = (dotSpell.DotEffect.TickDamage + spellPower * dotSpell.DotEffect.SpellDamageModifierPerTick) * dotEffectDamageModifier * eclipseBonus;
            float critTickDamage = baseTickDamage * 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            float averageTickDamage = (1 - spellCrit) * baseTickDamage + spellCrit * critTickDamage;

            dotSpell.DotEffect.DamagePerHit = (float)((baseTicks) * averageTickDamage);
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(CharacterCalculationsMoonkin calcs, int playerLevel, int bossLevel, float effectiveNatureDamage, float treantLifespan)
        {
            float natureDamageMultiplierBonus = (1f + calcs.BasicStats.BonusDamageMultiplier) * (1f + calcs.BasicStats.BonusNatureDamageMultiplier);
            // 0.375 spell power scaling
            float damagePerHit = 0.375f * effectiveNatureDamage;
            // 2.5 s cast time, affected by 100% of druid's haste
            float castTime = Math.Max(0.5f, 2.5f / (1 + calcs.SpellHaste));
            // Inherits 100% of the druid's crit rate
            float critRate = calcs.SpellCrit;
            // Apply the nature damage multiplier
            damagePerHit *= natureDamageMultiplierBonus;
            // Damage per cast, including misses
            damagePerHit = (critRate * damagePerHit * 2.0f)  + ((1 - critRate) * damagePerHit);
            // Total damage done in their estimated lifespan
            float damagePerTree = (treantLifespan * (float)Math.Floor(15f / castTime)) * damagePerHit;
            return 3 * damagePerTree;
        }

        // Starfall
        private float DoStarfallCalcs(CharacterCalculationsMoonkin calcs, float effectiveArcaneDamage, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier)
                * (1 + calcs.BasicStats.BonusStarfallDamageModifier);
            // Starfall is affected by Moonfury
            float critDamageModifier = 2 * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            float baseDamagePerStar = 0f;
            float mainStarCoefficient = 0.253f;

            float damagePerBigStarHit = (baseDamagePerStar + effectiveArcaneDamage * mainStarCoefficient) * hitDamageModifier;

            float critDamagePerBigStarHit = damagePerBigStarHit * critDamageModifier;

            float averageDamagePerBigStar = spellCrit * critDamagePerBigStarHit + (1 - spellCrit) * damagePerBigStarHit;

            float numberOfStarHits = 10f;

            float avgNumBigStarsHit = numberOfStarHits;

            return avgNumBigStarsHit * averageDamagePerBigStar;
        }

        // Celestial Alignment
        public float DoCelestialAlignmentCalcs(CharacterCalculationsMoonkin calcs, DruidTalents talents, float spellPower, float spellCrit, float spellHaste, float masteryPoints, float latency)
        {
            float caDuration = 15f;
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            float eclipseBonus = (1 + MoonkinSolver.ECLIPSE_BASE_PERCENT + masteryPoints * 0.01875f) * (1 + calcs.BasicStats.BonusEclipseDamageMultiplier) * (1 + (talents.NaturesVigil ? 0.12f : 0f));
            float accumulatedDuration = 0f;

            float sfCastTime = Math.Max(1f, sf.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float ssCastTime = Math.Max(1f, ss.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float wCastTime = Math.Max(1f, w.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            float mfCastTime = Math.Max(1f, mf.BaseCastTime / (1 + spellHaste) / 1.15f) + latency;
            // Add a GCD for activation
            accumulatedDuration += mfCastTime;

            // Moonfire/Sunfire calculations
            float mfTickRate = calcs.PTRMode ? 2f : (float)Math.Round(2f / (1 + spellHaste) / 1.15f, 3);
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
            float starfallDamage = DoStarfallCalcs(calcs, spellPower, spellCrit);
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

        private double[] DoubleLagrangeInterpolation(double x, double y, double[, ,] z)
        {
            double[] retval = new double[z.GetLength(2)];
            for (int i = 0; i < retval.Length; ++i)
            {
                double currentPoint = 0;
                for (int j = 0; j < CastDistributions.CritInterpolationPoints.Length; ++j)
                {
                    double critNumerator = 1, critDenominator = 1;
                    for (int ca = 0; ca < CastDistributions.CritInterpolationPoints.Length; ++ca)
                    {
                        if (ca == j) continue;
                        critNumerator *= x - CastDistributions.CritInterpolationPoints[ca];
                        critDenominator *= CastDistributions.CritInterpolationPoints[j] - CastDistributions.CritInterpolationPoints[ca];
                    }
                    double currentHastePoint = 0;
                    for (int k = 0; k < CastDistributions.HasteInterpolationPoints.Length; ++k)
                    {
                        double hasteNumerator = 1, hasteDenominator = 1;
                        for (int cb = 0; cb < CastDistributions.HasteInterpolationPoints.Length; ++cb)
                        {
                            if (cb == k) continue;
                            hasteNumerator *= y - CastDistributions.HasteInterpolationPoints[cb];
                            hasteDenominator *= CastDistributions.HasteInterpolationPoints[k] - CastDistributions.HasteInterpolationPoints[cb];
                        }
                        currentHastePoint += z[j, k, i] * (hasteNumerator / hasteDenominator);
                    }
                    currentPoint += currentHastePoint * (critNumerator / critDenominator);
                }
                retval[i] = currentPoint;
            }
            return retval;
        }

        /// <summary>
        /// Perform a Lagrangian polynomial interpolation on the specified table at a given value.
        /// </summary>
        /// <param name="x">The value at which to evaluate the polynomial.</param>
        /// <param name="y">The set of Y values that correspond to the nodes in the InterpolationPoints table.</param>
        /// <returns></returns>
        /*private double LagrangeInterpolation(double x, double[] y)
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
        }*/

        private double GetInterpolatedRotationLength(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable, bool useMightOfMalorneTable, bool useInsectSwarmTable)
        {
            return 30.0;
        }

        private double[] GetInterpolatedTickDistribution(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable, bool useMightOfMalorneTable, bool useInsectSwarmTable)
        {
            return DoubleLagrangeInterpolation(actualCrit, actualHaste, CastDistributions.TickDistribution);
        }

        private double[] GetInterpolatedTimeTable(float actualHaste, float actualCrit, bool useSoulOfTheForestTable, bool useT14Table, bool useT15Table, bool useT16Table, bool usePTRTable, bool useMightOfMalorneTable, bool useInsectSwarmTable)
        {
            return DoubleLagrangeInterpolation(actualCrit, actualHaste, CastDistributions.TimeDistribution);
        }

        private double[] GetInterpolatedEclipseTable(float actualHaste, float actualCrit)
        {
            return DoubleLagrangeInterpolation(actualCrit, actualHaste, CastDistributions.EclipseDistribution);
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(Character character, CharacterCalculationsMoonkin calcs, float treantLifespan, float spellPower, float spellCrit, float spellHaste, float masteryPoints, float multistrike, float versatility, float latency)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            Spell suf = Solver.Sunfire;

            float eclipseBonus = 1 + MoonkinSolver.ECLIPSE_BASE_PERCENT + masteryPoints * 0.015f;

            // Get the cast distribution first
            double[] timeDistribution = GetInterpolatedTimeTable(calcs.SpellHaste, calcs.SpellCrit, talents.SoulOfTheForest, calcs.BasicStats.BonusMoonfireDuration > 0, calcs.BasicStats.BonusStarsurgeCritModifier > 0, calcs.BasicStats.BonusShootingStarsChance > 0, calcs.PTRMode, talents.MightOfMalorne, talents.WillOfMalfurion);
            double[] tickDistribution = GetInterpolatedTickDistribution(calcs.SpellHaste, calcs.SpellCrit, talents.SoulOfTheForest, calcs.BasicStats.BonusMoonfireDuration > 0, calcs.BasicStats.BonusStarsurgeCritModifier > 0, calcs.BasicStats.BonusShootingStarsChance > 0, calcs.PTRMode, talents.MightOfMalorne, talents.WillOfMalfurion);
            double[] eclipseDistribution = GetInterpolatedEclipseTable(calcs.SpellHaste, calcs.SpellCrit).Select(ep => 1 + (ep / MoonkinSolver.ECLIPSE_BASE_PERCENT * (eclipseBonus - 1))).ToArray();

            // Do Nature's Grace calculations
            double allWrathPercentage = timeDistribution[2] + timeDistribution[3];
            double allStarfirePercentage = timeDistribution[0] + timeDistribution[1];
            double allStarsurgePercentage = timeDistribution[4];
            double allMoonfirePercentage = timeDistribution[5];
            double allSunfirePercentage = timeDistribution[6];

            RotationData.NaturesGraceUptime = 0.0f;

            // Get the duration
            RotationData.Duration = 40.0f;

            double gcd = Math.Max(1, 1.5 / (1 + spellHaste)) + 0;

            RotationData.AverageInstantCast = (float)(gcd) + latency;

            // 5.4: 2T16 set bonus calculations
            float T16Base = (478f + 615f) / 2f;
            float T16SpellpowerScaling = 0.1f;
            float T16TotalScaling = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            float T16ScaledDamage = (T16Base + T16SpellpowerScaling * spellPower) * T16TotalScaling;
            // T16 seems to have a 1.5% crit rate (just guessing)
            float T16DamagePerHit = calcs.BasicStats.T16TwoPieceActive ? 0.985f * T16ScaledDamage + 0.015f * T16ScaledDamage * 2 : 0f;

            DoMainNuke(calcs, ref sf, spellPower, spellCrit, spellHaste, multistrike, versatility, 0);
            DoMainNuke(calcs, ref ss, spellPower, spellCrit + calcs.BasicStats.BonusStarsurgeCritModifier, spellHaste, multistrike, versatility, 0);
            DoMainNuke(calcs, ref w, spellPower, spellCrit, spellHaste, multistrike, versatility, 0);

            DoMainNuke(calcs, ref mf, spellPower, spellCrit, spellHaste, multistrike, versatility, 0);
            DoMainNuke(calcs, ref suf, spellPower, spellCrit, spellHaste, multistrike, versatility, 0);

            RotationData.MoonfireAvgCast = mf.CastTime + latency;
            RotationData.SunfireAvgCast = suf.CastTime + latency;

            // Break the cast distribution down into its component cast counts
            double wrathCasts = timeDistribution[2] * RotationData.Duration / w.CastTime;
            double empoweredWrathCasts = timeDistribution[3] * RotationData.Duration / w.CastTime;
            double baseWrathPercentage = wrathCasts / (wrathCasts + empoweredWrathCasts);
            double empoweredWrathPercentage = empoweredWrathCasts / (wrathCasts + empoweredWrathCasts);
            RotationData.WrathAvgHit = (float)(baseWrathPercentage * w.DamagePerHit * eclipseDistribution[2] +
                empoweredWrathPercentage * w.DamagePerHit * eclipseDistribution[3] * 1.3f);
            RotationData.WrathAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.WrathAvgEnergy = (float)(baseWrathPercentage * eclipseDistribution[2] + empoweredWrathPercentage * eclipseDistribution[3]);
            RotationData.WrathCount = (float)(wrathCasts + empoweredWrathCasts);
            double starfireCasts = timeDistribution[0] * RotationData.Duration / sf.CastTime;
            double empoweredStarfireCasts = timeDistribution[1] * RotationData.Duration / sf.CastTime;
            double baseStarfirePercentage = starfireCasts / (starfireCasts + empoweredStarfireCasts);
            double empoweredStarfirePercentage = empoweredStarfireCasts / (starfireCasts + empoweredStarfireCasts);
            RotationData.StarfireAvgHit = (float)(baseStarfirePercentage * sf.DamagePerHit * eclipseDistribution[0] +
                empoweredStarfirePercentage * sf.DamagePerHit * eclipseDistribution[1] * 1.3f);
            RotationData.StarfireAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.StarfireAvgEnergy = (float)(baseStarfirePercentage * eclipseDistribution[0] + empoweredStarfirePercentage * eclipseDistribution[1]);
            RotationData.StarfireCount = (float)(starfireCasts + empoweredStarfireCasts);
            double starsurgeCasts = (timeDistribution[4]) * RotationData.Duration / ss.CastTime;
            RotationData.StarSurgeAvgHit = (float)(ss.DamagePerHit * eclipseDistribution[4]);
            RotationData.StarSurgeAvgHit *= 1 + calcs.BasicStats.MultistrikeProc;
            RotationData.StarSurgeAvgEnergy = (float)eclipseDistribution[4];
            RotationData.StarSurgeCount = (float)(starsurgeCasts);
            double moonfireCasts = (timeDistribution[5]) * RotationData.Duration / mf.CastTime;
            double sunfireCasts = (timeDistribution[6]) * RotationData.Duration / suf.CastTime;
            RotationData.MoonfireCasts = (float)(moonfireCasts);
            RotationData.SunfireCasts = (float)(sunfireCasts);

            DoTickDistribution(calcs, ref mf, spellPower, spellCrit, spellHaste, versatility, (float)eclipseDistribution[5], tickDistribution, 0, latency);
            DoTickDistribution(calcs, ref suf, spellPower, spellCrit, spellHaste, versatility, (float)eclipseDistribution[6], tickDistribution, 1, latency);

            // Dot Effect damage numbers already include Eclipsed vs. Non-Eclipsed ticks
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;
            RotationData.MoonfireAvgHit = (float)((mf.DamagePerHit + T16DamagePerHit) * eclipseDistribution[5]) * (1 + calcs.BasicStats.MultistrikeProc) + mf.DotEffect.DamagePerHit;
            RotationData.MoonfireAvgEnergy = (float)eclipseDistribution[5];
            RotationData.SunfireTicks = RotationData.SunfireCasts * suf.DotEffect.NumberOfTicks;
            RotationData.SunfireDuration = suf.DotEffect.Duration;
            RotationData.SunfireAvgHit = (float)((suf.DamagePerHit + T16DamagePerHit) * eclipseDistribution[6]) * (1 + calcs.BasicStats.MultistrikeProc) + suf.DotEffect.DamagePerHit;
            RotationData.SunfireAvgEnergy = (float)eclipseDistribution[6];

            RotationData.StarfireAvgCast = sf.CastTime + latency;
            RotationData.WrathAvgCast = w.CastTime + latency;

            RotationData.StarSurgeAvgCast = ss.CastTime + latency;

            // Treant calculations
            RotationData.TreantDamage = !talents.ForceOfNature ? 0 : DoTreeCalcs(calcs, character.Level, character.BossOptions.Level, spellPower, treantLifespan);
            RotationData.TreantCasts = !talents.ForceOfNature ? 0 : RotationData.Duration / 60f;
            //RotationData.Duration += RotationData.TreantCasts * RotationData.AverageInstantCast;

            // Incarnation - 1 full (non-hasted) GCD every 3 minutes
            //RotationData.Duration += talents.Incarnation ? 1.5f / (180f / RotationData.Duration) : 0f;

            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            float sunfireDamage = RotationData.SunfireAvgHit * RotationData.SunfireCasts;

            // Calculate total damage done for external cooldowns per rotation
            float treantDamage = RotationData.TreantDamage * RotationData.TreantCasts;

            // Calculate mana cost per cast.
            // Force of Nature - 10.3% of base mana
            float treantManaCost = (int)(0.103f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.StarfallCasts + RotationData.TreantCasts;
            RotationData.DotTicks = RotationData.MoonfireTicks + RotationData.SunfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.SunfireCasts * suf.BaseManaCost +
                RotationData.TreantCasts * treantManaCost;

            RotationData.ManaGained = 2 * MoonkinSolver.ECLIPSE_MANA_PERCENT * calcs.BasicStats.Mana;

            return (RotationData.WrathAvgHit * RotationData.WrathCount +
                RotationData.StarfireAvgHit * RotationData.StarfireCount +
                RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount +
                moonfireDamage + sunfireDamage) + treantDamage;
        }
    }
}
