using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
    // Helper class to contain the complete state of the simulator at any given time.
    class SimulatorState
    {
        // Time until next DoT tick for all 3 DoT spells
        public int MFTickTimer;
        public int SFTickTimer;

        // Tick rate
        public int MFTickRate;
        public int SFTickRate;

        // Do the currently active DoTs have NG?
        public bool MFHasNG;
        public bool SFHasNG;

        // Do the currently active DoTs have Eclipse?
        public bool MFHasEclipse;
        public bool SFHasEclipse;

        // Remaining duration on each DoT
        public int MFDurationTimer;
        public int SFDurationTimer;

        // Shortcuts to determine ticks remaining
        // Simcraft uses integer representation, we will too
        public int MFDurationTicks { get { return (int)Math.Ceiling(MFDurationTimer / (double)MFTickRate); } }
        public int SFDurationTicks { get { return (int)Math.Ceiling(SFDurationTimer / (double)SFTickRate); } }
        // NG duration
        public int NGDurationTimer = 15000;

        // Shooting Stars proc timer, Starsurge cooldown
        public int ShSProcTimer;
        public int SSCooldown;

        // Eclipse energy bar
        private int _eclipseEnergy = 100;
        public int EclipseEnergy
        {
            get
            {
                return _eclipseEnergy;
            }
            set
            {
                // Get the difference between the new value and the old
                // Because we are always adding a positive number to energy, the incoming value will always be bigger
                int incomingEnergy = value - _eclipseEnergy;

                // Apply the appropriate directional multiplier and cap it at -100 <= x <= 100
                _eclipseEnergy = Math.Max(-100, Math.Min(100, _eclipseEnergy + incomingEnergy * _eclipseDirection));

                // If we hit an Eclipse, reverse the direction
                // Also reset Nature's Grace duration
                if (Math.Abs(_eclipseEnergy) == 100)
                {
                    EclipseDirection = -EclipseDirection;
                    NGDurationTimer = 15000;
                }
            }
        }

        // Direction on the Eclipse bar
        private int _eclipseDirection = -1;
        public int EclipseDirection
        {
            get { return _eclipseDirection; }
            set
            {
                if (value == -1 || value == 1)
                    _eclipseDirection = value;
            }
        }

        // Wrath counter - used to track the amount of energy each Wrath generates
        private int _wrathCounter;
        public int WrathCounter
        {
            get
            {
                return _wrathCounter;
            }
            set
            {
                _wrathCounter = value % 3;
            }
        }

        public bool WantToCastStarsurge
        {
            get
            {
                if (InSolarEclipse)
                {
                    return !(
                        _eclipseEnergy == 80 ||
                        _eclipseEnergy == 65 ||
                        _eclipseEnergy == 50 ||
                        _eclipseEnergy == 35 ||
                        _eclipseEnergy == 20);
                }
                return true;
            }
        }

        // Shortcuts to check for Eclipse state
        public bool InSolarEclipse { get { return _eclipseEnergy > 0 && _eclipseEnergy <= 100 && _eclipseDirection == -1; } }
        public bool InLunarEclipse { get { return _eclipseEnergy < 0 && _eclipseEnergy >= -100 && _eclipseDirection == 1; } }

        // Shortcut function to evaluate Euphoria validity
        public bool CanEuphoriaProc(int incomingEnergy) { return !InSolarEclipse && !InLunarEclipse && Math.Abs(_eclipseEnergy + _eclipseDirection * incomingEnergy) < 100; }

        // Has SotF procced?
        public bool SotfActive;
    }

    class IterationResults
    {
        private int _rotationCount;
        public int RotationCount
        {
            get { return _rotationCount; }
            set
            {
                _rotationCount = value;
                CompleteRotationTime = CurrentDuration / 1000.0;
                Array.Copy(TotalCastCounts, _rotationCastCounts, TotalCastCounts.Length);
                Array.Copy(TotalTickCounts, _rotationTickCounts, TotalTickCounts.Length);
                Array.Copy(TotalTimeCounts, _rotationTimeCounts, TotalTimeCounts.Length);
            }
        }
        public int CurrentDuration;
        public double CompleteRotationTime { get; private set; }
        public double AverageRotationLength { get { return CompleteRotationTime / _rotationCount; } }

        private int[] _rotationCastCounts = new int[MoonkinSimulator.castDistributionCount];
        public int[] RotationCastCounts { get { return _rotationCastCounts; } }
        private int[] _rotationTickCounts = new int[MoonkinSimulator.tickDistributionCount];
        public int[] RotationTickCounts { get { return _rotationTickCounts; } }
        private int[] _rotationTimeCounts = new int[MoonkinSimulator.castDistributionCount];
        public int[] RotationTimeCounts { get { return _rotationTimeCounts; } }

        public int[] TotalCastCounts = new int[MoonkinSimulator.castDistributionCount];
        public int[] TotalTickCounts = new int[MoonkinSimulator.tickDistributionCount];
        public int[] TotalTimeCounts = new int[MoonkinSimulator.castDistributionCount];
    }

    public class MoonkinSimulator
    {
        public int SimulationCount { get; set; }
        public int FightLength { get; set; }
        public bool HasSoulOfTheForest { get; set; }
        public bool AlwaysRefresh { get; set; }
        private bool _has4T14 = false;
        public bool Has4T14
        {
            get { return _has4T14; }
            set
            {
                _has4T14 = value;
                BaseMFDuration = value ? 16000 : 14000;
                HasteLevel = HasteLevel;
            }
        }
        public bool Has2T15 { get; set; }
        private bool _has4T16 = false;
        public bool Has4T16
        {
            get { return _has4T16; }
            set
            {
                _has4T16 = value;
                ShootingStarsChance = value ? 0.38 : 0.30;
            }
        }

        private double _hasteLevel;
        public double HasteLevel
        {
            get { return _hasteLevel; }
            set
            {
                _hasteLevel = value;
                CurrentStarfireCastTime = Math.Max(1000, (int)Math.Round(BaseStarfireCastTime / (1 + HasteLevel)));
                CurrentStarsurgeCastTime = Math.Max(1000, (int)Math.Round(BaseStarsurgeCastTime / (1 + HasteLevel)));
                CurrentWrathCastTime = Math.Max(1000, (int)Math.Round(BaseWrathCastTime / (1 + HasteLevel)));
                CurrentGlobalCooldown = Math.Max(1000, (int)Math.Round(BaseGlobalCooldown / (1 + HasteLevel)));
                CurrentTickRate = (int)Math.Round(BaseTickRate / (1 + HasteLevel));
                CurrentMFDuration = (int)Math.Round(BaseMFDuration / (double)CurrentTickRate) * CurrentTickRate;

                NGStarfireCastTime = Math.Max(1000, (int)Math.Round(BaseStarfireCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGStarsurgeCastTime = Math.Max(1000, (int)Math.Round(BaseStarsurgeCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGWrathCastTime = Math.Max(1000, (int)Math.Round(BaseWrathCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGGlobalCooldown = Math.Max(1000, (int)Math.Round(BaseGlobalCooldown / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGTickRate = (int)Math.Round(BaseTickRate / (1 + HasteLevel) / (1 + NaturesGraceHaste));
                NGMFDuration = (int)Math.Round(BaseMFDuration / (double)NGTickRate) * NGTickRate;
            }
        }
        public double CritChance { get; set; }

        private double SotfChance = 0.08;
        private double ShootingStarsChance = 0.3;
        private double NaturesGraceHaste = 0.15;

        private int BaseStarfireCastTime = 2700;
        private int BaseWrathCastTime = 2000;
        private int BaseStarsurgeCastTime = 2000;
        private int BaseGlobalCooldown = 1500;
        private int BaseTickRate = 2000;
        private int BaseMFDuration = 14000;

        private int CurrentStarfireCastTime;
        private int CurrentWrathCastTime;
        private int CurrentStarsurgeCastTime;
        private int CurrentGlobalCooldown;
        private int CurrentTickRate;
        private int CurrentMFDuration;

        private int NGStarfireCastTime;
        private int NGWrathCastTime;
        private int NGStarsurgeCastTime;
        private int NGGlobalCooldown;
        private int NGTickRate;
        private int NGMFDuration;

        private double averageRotationLength;
        private double[] tickResults;
        private double[] timeResults;

        private Random rng;

        public static int castDistributionCount = 28;
        public static int tickDistributionCount = 4;

        public MoonkinSimulator()
        {
            rng = new Random((int)DateTime.Now.Ticks);
            SimulationCount = 10000;
            FightLength = 300;
            CritChance = 0.2;
            HasteLevel = 0;
        }

        public double[] GenerateCycle()
        {
            tickResults = new double[tickDistributionCount];
            int[] castCounts = new int[castDistributionCount];
            timeResults = new double[castDistributionCount];
            int[] iterationCounts = new int[castDistributionCount];
            int[] iterationTickCounts = new int[tickDistributionCount];
            int[] iterationTimeCounts = new int[castDistributionCount];
            averageRotationLength = 0;

            for (int iteration = 0; iteration < SimulationCount; ++iteration)
            {
                double totalRotationTime;
                int rotationCount;
                GetIterationValues(iterationCounts, iterationTickCounts, iterationTimeCounts, out totalRotationTime, out rotationCount);
                for (int idx = 0; idx < castDistributionCount; ++idx)
                {
                    castCounts[idx] += iterationCounts[idx];
                    timeResults[idx] += iterationTimeCounts[idx] / (totalRotationTime * 1000.0);
                }
                for (int idx = 0; idx < tickDistributionCount; ++idx)
                {
                    // Use un-rounded tick rate to make the conversion function smooth, i.e. not stepwise
                    double modifier = BaseTickRate / 1000.0 / (1 + HasteLevel) / (1 + (idx % 2) * 0.15);
                    tickResults[idx] += iterationTickCounts[idx] * modifier / totalRotationTime;
                }

                averageRotationLength += totalRotationTime / rotationCount;
            }

            averageRotationLength /= SimulationCount;

            for (int idx = 0; idx < tickDistributionCount; ++idx)
            {
                tickResults[idx] /= (double)SimulationCount;
            }

            int castCount = castCounts.Sum();

            double[] retval = new double[castDistributionCount];

            for (int idx = 0; idx < castDistributionCount; ++idx)
            {
                retval[idx] = castCounts[idx] / (double)castCount;
                timeResults[idx] /= (double)SimulationCount;
            }

            return retval;
        }

        public double GetRotationLength()
        {
            return averageRotationLength;
        }

        public double[] GetTickResults()
        {
            return tickResults;
        }

        public double[] GetTimeResults()
        {
            return timeResults;
        }

        private void GetIterationValues(int[] castCounts, int[] tickCounts, int[] timeCounts, out double totalRotationTime, out int rotationCount)
        {
            SimulatorState state = new SimulatorState { MFTickRate = NGTickRate, SFTickRate = NGTickRate };
            IterationResults results = new IterationResults();

            bool recastSunfire = true, recastMoonfire = false;
            bool sunfireCast = false, moonfireCast = false;

            do
            {
                int currentActionTime = 0;
                // If we are not in Solar Eclipse before the action, and we are afterward, we advance the rotation counter
                bool advanceRotationCounter = !state.InSolarEclipse;
                bool currentlyInLunar = state.InLunarEclipse;

                // Soul of the Forest: When you exit Eclipse, you gain 40 energy in the appropriate direction.
                // New SotF: When you are not in any Eclipse, cast Astral Communion to immediately proceed to the next Eclipse.
                if (HasSoulOfTheForest && state.SotfActive && !state.InSolarEclipse && !state.InLunarEclipse)
                {
                    currentActionTime = CastAstralCommunion(state, results);
                    state.EclipseEnergy += 100;
                    state.SotfActive = false;
                }
                // Priority 1: Sunfire
                else if (recastSunfire)
                {
                    currentActionTime = CastSunfire(state, results);
                    sunfireCast = true;
                }
                // Priority 2: Moonfire
                else if (recastMoonfire)
                {
                    currentActionTime = CastMoonfire(state, results);
                    moonfireCast = true;
                }
                // Priority 3: Starsurge
                // From Hamlet at EJ: If Eclipse energy is in certain positions on the bar, hold Starsurge
                else if (state.SSCooldown == 0 && state.WantToCastStarsurge)
                {
                    currentActionTime = CastStarsurge(state, results);
                    moonfireCast = sunfireCast = false;
                }
                // Priority 4: Wrath
                else if (state.EclipseDirection == -1)
                {
                    currentActionTime = CastWrath(state, results);
                    moonfireCast = sunfireCast = false;
                }
                // Priority 5: Starfire
                else
                {
                    currentActionTime = CastStarfire(state, results);
                    moonfireCast = sunfireCast = false;
                }

                results.CurrentDuration += currentActionTime;

                // Advance the rotation counter if we have just hit Solar Eclipse
                // This updates the result set
                if (state.EclipseEnergy == 100 && advanceRotationCounter)
                {
                    ++results.RotationCount;
                }

                // Recast the appropriate DoT if it's about to expire (tick timer set to run out before next nuke can go off) and we are in the appropriate Eclipse
                // Modified for experimental purposes
                if (AlwaysRefresh)
                {
                    recastMoonfire = (state.InLunarEclipse && state.EclipseEnergy > -20 && !moonfireCast) ||
                        (state.MFDurationTimer <= NGStarsurgeCastTime &&
                                (state.InLunarEclipse ||
                                (!state.InLunarEclipse && GetTimeToLunarEclipse(state) >= 5 * state.MFTickRate)));
                    recastSunfire = (state.InSolarEclipse && state.EclipseEnergy < 15 && !sunfireCast) ||
                        (state.SFDurationTimer <= NGStarsurgeCastTime &&
                                (state.InSolarEclipse ||
                                (!state.InSolarEclipse && GetTimeToSolarEclipse(state) >= 6 * state.SFTickRate)));
                }
                else
                {
                    recastMoonfire = (state.InLunarEclipse && state.MFDurationTimer <= NGStarsurgeCastTime) ||
                        (state.InLunarEclipse && state.EclipseEnergy > -20 && !moonfireCast);
                    recastSunfire = (state.InSolarEclipse && state.SFDurationTimer <= NGStarsurgeCastTime) ||
                        (state.InSolarEclipse && state.EclipseEnergy < 15 && !sunfireCast);
                }

            } while (results.CurrentDuration <= (FightLength * 1000) - CurrentGlobalCooldown);

            Array.Copy(results.RotationCastCounts, castCounts, castDistributionCount);
            Array.Copy(results.RotationTickCounts, tickCounts, tickDistributionCount);
            Array.Copy(results.RotationTimeCounts, timeCounts, castDistributionCount);

            totalRotationTime = results.CompleteRotationTime;
            rotationCount = results.RotationCount;
        }

        private int GetTimeToLunarEclipse(SimulatorState state)
        {
            // In Lunar, no time remaining
            if (state.InLunarEclipse)
            {
                return 0;
            }
            // In Solar, get time remaining in Solar and add casts to Lunar (4 for no SotF, 2 with)
            else if (state.InSolarEclipse)
            {
                int eclipseRemaining = state.EclipseEnergy;
                int castsToExitEclipse = (int)Math.Ceiling(eclipseRemaining / 15.0);
                return (castsToExitEclipse + 4) * CurrentWrathCastTime;
            }
            // Moving in Lunar direction
            else if (state.EclipseDirection < 0)
            {
                int eclipseRemaining = Math.Abs(state.EclipseEnergy);
                return (int)Math.Ceiling(eclipseRemaining / 30.0) * CurrentWrathCastTime;
            }
            // Moving in Solar direction
            else// if (state.EclipseDirection > 0)
            {
                // Get time to Solar
                int eclipseToSolar = 100 - state.EclipseEnergy;
                int timeToSolar = (int)Math.Ceiling(eclipseToSolar / 20.0) * CurrentStarfireCastTime;
                // Add time from Solar to Lunar
                return timeToSolar + 11 * CurrentWrathCastTime;
            }
        }

        private int GetTimeToSolarEclipse(SimulatorState state)
        {
            // In Lunar
            if (state.InLunarEclipse)
            {
                int eclipseRemaining = -state.EclipseEnergy;
                return ((int)Math.Ceiling(eclipseRemaining / 15.0) + 3) * CurrentStarfireCastTime;
            }
            // In Solar, no time remaining
            else if (state.InSolarEclipse)
            {
                return 0;
            }
            // Moving in Lunar direction
            else if (state.EclipseDirection < 0)
            {
                // Get time to Lunar
                int eclipseToLunar = 100 + state.EclipseEnergy;
                int timeToLunar = (int)Math.Ceiling(eclipseToLunar / 30.0) * CurrentWrathCastTime;
                // Add time from Lunar to Solar
                return timeToLunar + 8 * CurrentStarfireCastTime;
            }
            // Moving in Solar direction
            else// if (state.EclipseDirection > 0)
            {
                int eclipseRemaining = Math.Abs(state.EclipseEnergy);
                return (int)Math.Ceiling(eclipseRemaining / 40.0) * CurrentStarfireCastTime;
            }
        }

        private int CastAstralCommunion(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Return cast time to caller
            return castTime;
        }

        private int CastSunfire(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[27];
                    results.TotalTimeCounts[27] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[26];
                    results.TotalTimeCounts[26] += castTime;
                }
            }
            else
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[13];
                    results.TotalTimeCounts[13] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[12];
                    results.TotalTimeCounts[12] += castTime;
                }
            }

            // Set SF variables
            state.SFHasNG = state.NGDurationTimer > 0;
            state.SFDurationTimer = (state.SFHasNG ? NGMFDuration : CurrentMFDuration) + state.SFTickTimer;
            state.SFTickRate = state.SFHasNG ? NGTickRate : CurrentTickRate;

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Return cast time to caller
            return castTime;
        }

        private int CastMoonfire(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[25];
                    results.TotalTimeCounts[25] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[24];
                    results.TotalTimeCounts[24] += castTime;
                }
            }
            else
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[11];
                    results.TotalTimeCounts[11] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[10];
                    results.TotalTimeCounts[10] += castTime;
                }
            }

            // Set MF variables
            state.MFHasNG = state.NGDurationTimer > 0;
            state.MFHasEclipse = state.InLunarEclipse;
            state.MFDurationTimer = (state.MFHasNG ? NGMFDuration : CurrentMFDuration) + state.MFTickTimer;
            state.MFTickRate = state.MFHasNG ? NGTickRate : CurrentTickRate;

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Return cast time to caller
            return castTime;
        }

        private int CastStarsurge(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = 0;

            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.ShSProcTimer > 0)
                {
                    castTime = NGGlobalCooldown;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[23];
                        results.TotalTimeCounts[23] += castTime;
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[22];
                        results.TotalTimeCounts[22] += castTime;
                    }
                    else
                    {
                        ++results.TotalCastCounts[21];
                        results.TotalTimeCounts[21] += castTime;
                    }
                }
                else
                {
                    castTime = NGStarsurgeCastTime;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[20];
                        results.TotalTimeCounts[20] += castTime;
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[19];
                        results.TotalTimeCounts[19] += castTime;
                    }
                    else
                    {
                        ++results.TotalCastCounts[18];
                        results.TotalTimeCounts[18] += castTime;
                    }
                }
            }
            else
            {
                if (state.ShSProcTimer > 0)
                {
                    castTime = CurrentGlobalCooldown;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[9];
                        results.TotalTimeCounts[9] += castTime;
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[8];
                        results.TotalTimeCounts[8] += castTime;
                    }
                    else
                    {
                        ++results.TotalCastCounts[7];
                        results.TotalTimeCounts[7] += castTime;
                    }
                }
                else
                {
                    castTime = CurrentStarsurgeCastTime;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[6];
                        results.TotalTimeCounts[6] += castTime;
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[5];
                        results.TotalTimeCounts[5] += castTime;
                    }
                    else
                    {
                        ++results.TotalCastCounts[4];
                        results.TotalTimeCounts[4] += castTime;
                    }
                }
            }

            // Set SS variables
            if (state.ShSProcTimer > 0) state.SSCooldown = 15000 - castTime;
            else
            {
                // Adjust the cooldown of Starsurge if the true cast time (equal to current tick rate) is below the 1-second GCD
                int adjustment = 0;
                if (castTime == 1000) adjustment = castTime - (state.NGDurationTimer > 0 ? NGTickRate : CurrentTickRate);
                state.SSCooldown = 15000 - adjustment;
            }
            state.ShSProcTimer = 0;

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Updated: Euphoria now gives double energy whenever not in Eclipse
            int baseEnergy = 20;
            state.EclipseEnergy += baseEnergy + (!(state.InLunarEclipse || state.InSolarEclipse) ? baseEnergy : 0);

            // Perform Sunfire updates
            if (state.SFDurationTimer > 0 && rng.NextDouble() <= CritChance + (Has2T15 ? 0.1 : 0))
            {
                // Reset the SF duration timer
                state.SFDurationTimer += state.SFTickRate;
            }

            // Perform Moonfire updates
            if (state.MFDurationTimer > 0 && rng.NextDouble() <= CritChance + (Has2T15 ? 0.1 : 0))
            {
                // Reset the MF duration timer
                // Nuke crits now only add a tick
                state.MFDurationTimer += state.MFTickRate;
            }

            // Soul of the Forest
            if (HasSoulOfTheForest && rng.NextDouble() <= SotfChance)
                state.SotfActive = true;

            // Return cast time to caller
            return castTime;
        }

        private int CastWrath(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = state.NGDurationTimer > 0 ? NGWrathCastTime : CurrentWrathCastTime;

            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[15];
                    results.TotalTimeCounts[15] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[14];
                    results.TotalTimeCounts[14] += castTime;
                }
            }
            else
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[1];
                    results.TotalTimeCounts[1] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[0];
                    results.TotalTimeCounts[0] += castTime;
                }
            }

            // No variables to set for Wrath

            // Set SS variables before tick calculations in case a Shooting Stars proc occurs
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);
            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Set Eclipse energy
            int baseEnergy = 15;
            //bool euphoriaProcced = state.CanEuphoriaProc(2 * baseEnergy) && rng.NextDouble() <= EuphoriaChance;
            state.EclipseEnergy += baseEnergy + (!state.InSolarEclipse ? baseEnergy : 0);
            /*if (euphoriaProcced)
            {
                state.EclipseEnergy += baseEnergy;
            }*/

            // Perform Sunfire updates
            if (state.SFDurationTimer > 0 && rng.NextDouble() <= CritChance)
            {
                // Reset the SF duration timer
                state.SFDurationTimer += state.SFTickRate;
            }

            // Soul of the Forest
            if (HasSoulOfTheForest && rng.NextDouble() <= SotfChance)
                state.SotfActive = true;

            // Return cast time to caller
            return castTime;
        }

        private int CastStarfire(SimulatorState state, IterationResults results)
        {
            // Calculate cast time
            int castTime = state.NGDurationTimer > 0 ? NGStarfireCastTime : CurrentStarfireCastTime;

            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[17];
                    results.TotalTimeCounts[17] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[16];
                    results.TotalTimeCounts[16] += castTime;
                }
            }
            else
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[3];
                    results.TotalTimeCounts[3] += castTime;
                }
                else
                {
                    ++results.TotalCastCounts[2];
                    results.TotalTimeCounts[2] += castTime;
                }
            }

            // No variables to set for Starfire

            // Set SS variables before tick calculations in case a Shooting Stars proc occurs
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);
            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Set Eclipse energy
            //int baseEnergy = HasChosenOfElune ? (state.InLunarEclipse ? 10 : 40) : 20;
            int baseEnergy = 20;
            //bool euphoriaProcced = state.CanEuphoriaProc(2 * baseEnergy) && rng.NextDouble() <= EuphoriaChance;
            state.EclipseEnergy += baseEnergy + (!state.InLunarEclipse ? baseEnergy : 0);
            /*if (euphoriaProcced)
            {
                state.EclipseEnergy += baseEnergy;
            }*/

            // Perform Moonfire updates
            if (state.MFDurationTimer > 0 && rng.NextDouble() <= CritChance)
            {
                // Reset the MF duration timer
                // Nuke crits now only add a tick
                state.MFDurationTimer += state.MFTickRate;
            }

            // Soul of the Forest
            if (HasSoulOfTheForest && rng.NextDouble() <= SotfChance)
                state.SotfActive = true;

            // Return cast time to caller
            return castTime;
        }

        private void DoTickCalculations(SimulatorState state, IterationResults results, int castTime)
        {
            int dotTicks = DoMoonfireTicks(state, results, castTime) +
                DoSunfireTicks(state, results, castTime);

            // Do calculations for Shooting Stars
            if (rng.NextDouble() <= 1 - Math.Pow(1 - CritChance, dotTicks) && rng.NextDouble() <= 1 - Math.Pow(1 - ShootingStarsChance, dotTicks))
            {
                state.ShSProcTimer = 12000;
                state.SSCooldown = 0;
            }
        }

        private int DoMoonfireTicks(SimulatorState state, IterationResults results, int castTime)
        {
            return DoDotTicks(ref state.MFDurationTimer, state.MFTickRate, ref state.MFTickTimer, castTime, state.MFHasEclipse, state.MFHasNG, 0, results);
        }

        private int DoSunfireTicks(SimulatorState state, IterationResults results, int castTime)
        {
            return DoDotTicks(ref state.SFDurationTimer, state.SFTickRate, ref state.SFTickTimer, castTime, state.SFHasEclipse, state.SFHasNG, 0, results);
        }

        private int DoDotTicks(ref int dotDuration, int tickRate, ref int tickTimer, int castTime, bool dotHasEclipse, bool dotHasNG, int baseIndex, IterationResults results)
        {
            // Dot inactive - no ticks occurred
            if (dotDuration == 0) return 0;

            // Find current number of ticks remaining
            int currentTicksLeft = dotDuration / tickRate;
            if (tickTimer > 0) ++currentTicksLeft;

            // Calculate the duration of the dot spell following the cast
            int newDuration = Math.Max(0, dotDuration - castTime);

            // Transform the new duration into a tick count
            int newTicksLeft = newDuration / tickRate;
            // Set the tick timer while we're here
            tickTimer = newDuration - tickRate * newTicksLeft;
            if (tickTimer > 0) ++newTicksLeft;

            // The tick count difference is the number of ticks that occurred
            int ticksOccurred = currentTicksLeft - newTicksLeft;

            // Set the new dot duration
            dotDuration = newDuration;

            // Increment the tick result tables
            if (dotHasNG)
            {
                if (dotHasEclipse)
                {
                    results.TotalTickCounts[baseIndex+3] += ticksOccurred;
                }
                else
                {
                    results.TotalTickCounts[baseIndex+1] += ticksOccurred;
                }
            }
            else
            {
                if (dotHasEclipse)
                {
                    results.TotalTickCounts[baseIndex+2] += ticksOccurred;
                }
                else
                {
                    results.TotalTickCounts[baseIndex] += ticksOccurred;
                }
            }

            // Return the tick total to the caller
            return ticksOccurred;
        }
    }
}
