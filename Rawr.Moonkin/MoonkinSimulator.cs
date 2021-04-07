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
        public int ISTickTimer;

        // Tick rate
        public int MFTickRate;
        public int SFTickRate;
        public int ISTickRate;

        // Do the currently active DoTs have NG?
        public bool MFHasNG;
        public bool SFHasNG;
        public bool ISHasNG;

        // Do the currently active DoTs have Eclipse?  (Sunfire always does)
        public bool MFHasEclipse;
        public bool ISHasEclipse;

        // Remaining duration on each DoT
        public int MFDurationTimer;
        public int SFDurationTimer;
        public int ISDurationTimer;

        // Shortcuts to determine ticks remaining
        // Simcraft uses integer representation, we will too
        public int MFDurationTicks { get { return (int)Math.Ceiling(MFDurationTimer / (double)MFTickRate); } }
        public int SFDurationTicks { get { return (int)Math.Ceiling(SFDurationTimer / (double)SFTickRate); } }
        public int ISDurationTicks { get { return (int)Math.Ceiling(ISDurationTimer / (double)ISTickRate); } }

        // NG cooldown and duration
        public int NGCooldown;
        public int NGDurationTimer;

        // Shooting Stars proc timer, Starsurge cooldown
        public int ShSProcTimer;
        public int SSCooldown;

        // Number of activations of the extension on the current MF
        private int _mfExtendedCounter;
        public int MFExtendedCounter
        {
            get { return _mfExtendedCounter; }
            set
            {
                if (value > 3)
                    _mfExtendedCounter = 3;
                else
                    _mfExtendedCounter = value;
            }
        }

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
                // Also clear Nature's Grace cooldown
                if (Math.Abs(_eclipseEnergy) == 100)
                {
                    EclipseDirection = -EclipseDirection;
                    NGCooldown = 0;
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

        // Shortcuts to check for Eclipse state
        public bool InSolarEclipse { get { return _eclipseEnergy > 0 && _eclipseEnergy <= 100 && _eclipseDirection == -1; } }
        public bool InLunarEclipse { get { return _eclipseEnergy < 0 && _eclipseEnergy >= -100 && _eclipseDirection == 1; } }

        // Shortcut to determine if Euphoria is valid
        public bool EuphoriaActive
        {
            get
            {
                return (_eclipseEnergy >= 0 && _eclipseEnergy <= 35 && _eclipseDirection == 1) ||
                       (_eclipseEnergy <= 0 && _eclipseEnergy >= -35 && _eclipseDirection == -1);
            }
        }
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
            }
        }
        public int CurrentDuration;
        public double CompleteRotationTime { get; private set; }
        public double AverageRotationLength { get { return CompleteRotationTime / _rotationCount; } }

        private int[] _rotationCastCounts = new int[MoonkinSimulator.castDistributionCount];
        public int[] RotationCastCounts { get { return _rotationCastCounts; } }
        private int[] _rotationTickCounts = new int[MoonkinSimulator.tickDistributionCount];
        public int[] RotationTickCounts { get { return _rotationTickCounts; } }

        public int[] TotalCastCounts = new int[MoonkinSimulator.castDistributionCount];
        public int[] TotalTickCounts = new int[MoonkinSimulator.tickDistributionCount];
    }

    public class MoonkinSimulator
    {
        public int SimulationCount { get; set; }
        public int FightLength { get; set; }

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
                CurrentISDuration = (int)Math.Round(BaseISDuration / (double)CurrentTickRate) * CurrentTickRate;

                NGStarfireCastTime = Math.Max(1000, (int)Math.Round(BaseStarfireCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGStarsurgeCastTime = Math.Max(1000, (int)Math.Round(BaseStarsurgeCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGWrathCastTime = Math.Max(1000, (int)Math.Round(BaseWrathCastTime / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGGlobalCooldown = Math.Max(1000, (int)Math.Round(BaseGlobalCooldown / (1 + HasteLevel) / (1 + NaturesGraceHaste)));
                NGTickRate = (int)Math.Round(BaseTickRate / (1 + HasteLevel) / (1 + NaturesGraceHaste));
                NGMFDuration = (int)Math.Round(BaseMFDuration / (double)NGTickRate) * NGTickRate;
                NGISDuration = (int)Math.Round(BaseISDuration / (double)NGTickRate) * NGTickRate;
            }
        }

        public bool HasGlyphOfStarfire { get; set; }
        public bool Has4T12 { get; set; }
        public bool Has4T13 { get; set; }

        private double ShootingStarsChance = 0.04;
        private double EuphoriaChance = 0.24;
        private double NaturesGraceHaste = 0.15;

        private int BaseStarfireCastTime = 2700;
        private int BaseWrathCastTime = 2000;
        private int BaseStarsurgeCastTime = 2000;
        private int BaseGlobalCooldown = 1500;
        private int BaseTickRate = 2000;
        private int BaseMFDuration = 18000;
        private int BaseISDuration = 18000;

        private int CurrentStarfireCastTime;
        private int CurrentWrathCastTime;
        private int CurrentStarsurgeCastTime;
        private int CurrentGlobalCooldown;
        private int CurrentTickRate;
        private int CurrentMFDuration;
        private int CurrentISDuration;

        private int NGStarfireCastTime;
        private int NGWrathCastTime;
        private int NGStarsurgeCastTime;
        private int NGGlobalCooldown;
        private int NGTickRate;
        private int NGMFDuration;
        private int NGISDuration;

        private double averageRotationLength;
        private double[] tickResults;

        private Random rng;

        public static int castDistributionCount = 32;
        public static int tickDistributionCount = 8;

        public MoonkinSimulator()
        {
            rng = new Random((int)DateTime.Now.Ticks);
            SimulationCount = 25000;
            FightLength = 450;
        }

        public double[] GenerateCycle()
        {
            tickResults = new double[tickDistributionCount];
            int[] castCounts = new int[castDistributionCount];
            int[] iterationCounts = new int[castDistributionCount];
            int[] iterationTickCounts = new int[tickDistributionCount];
            averageRotationLength = 0;

            for (int iteration = 0; iteration < SimulationCount; ++iteration)
            {
                double totalRotationTime;
                int rotationCount;
                GetIterationValues(iterationCounts, iterationTickCounts, out totalRotationTime, out rotationCount);
                for (int idx = 0; idx < castDistributionCount; ++idx)
                {
                    castCounts[idx] += iterationCounts[idx];
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

        private void GetIterationValues(int[] castCounts, int[] tickCounts, out double totalRotationTime, out int rotationCount)
        {
            SimulatorState state = new SimulatorState { MFTickRate = CurrentTickRate, ISTickRate = CurrentTickRate, SFTickRate = CurrentTickRate };
            IterationResults results = new IterationResults();

            do
            {
                int currentActionTime = 0;
                // If we are not in Solar Eclipse before the action, and we are afterward, we advance the rotation counter
                bool advanceRotationCounter = !state.InSolarEclipse;
                // Priority 1: Insect Swarm
                if ((state.InSolarEclipse || state.InLunarEclipse) &&
                    (state.ISDurationTicks < 2 ||
                    (state.ISDurationTimer < 10000 && state.InSolarEclipse && state.EclipseEnergy < 15)))
                {
                    currentActionTime = CastInsectSwarm(state, results);
                }
                // Priority 2: Sunfire
                else if (state.InSolarEclipse &&
                    ((state.SFDurationTicks < 2 && state.MFDurationTimer == 0) ||
                    (state.SFDurationTimer < 10000 && state.EclipseEnergy < 15)))
                {
                    currentActionTime = CastSunfire(state, results);
                }
                // Priority 3: Moonfire
                else if (state.InLunarEclipse &&
                    ((state.MFDurationTicks < 2 && state.SFDurationTimer == 0) ||
                    (state.MFDurationTimer < 10000 && state.EclipseEnergy > -20)))
                {
                    currentActionTime = CastMoonfire(state, results);
                }
                // Priority 4: Starsurge
                // With the 4T12 set bonus, only cast if we are in Eclipse
                else if (state.SSCooldown == 0 && (!Has4T12 || (state.InSolarEclipse || state.InLunarEclipse)))
                {
                    currentActionTime = CastStarsurge(state, results);
                }
                // Priority 5: Wrath
                else if (state.EclipseDirection == -1)
                {
                    currentActionTime = CastWrath(state, results);
                }
                // Priority 6: Starfire
                else
                {
                    currentActionTime = CastStarfire(state, results);
                }

                results.CurrentDuration += currentActionTime;

                // Advance the rotation counter if we have just hit Solar Eclipse
                // This updates the result set
                if (state.EclipseEnergy == 100 && advanceRotationCounter)
                {
                    ++results.RotationCount;
                }

            } while (results.CurrentDuration <= (FightLength * 1000) - CurrentGlobalCooldown);

            Array.Copy(results.RotationCastCounts, castCounts, castDistributionCount);
            Array.Copy(results.RotationTickCounts, tickCounts, tickDistributionCount);

            totalRotationTime = results.CompleteRotationTime;
            rotationCount = results.RotationCount;
        }

        private int CastInsectSwarm(SimulatorState state, IterationResults results)
        {
            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[31];
                }
                else if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[30];
                }
                else
                {
                    ++results.TotalCastCounts[29];
                }
            }
            else
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[15];
                }
                else if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[14];
                }
                else
                {
                    ++results.TotalCastCounts[13];
                }
            }

            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            // Set IS variables
            state.ISHasNG = state.NGDurationTimer > 0;
            state.ISHasEclipse = state.InSolarEclipse;
            state.ISDurationTimer = (state.ISHasNG ? NGISDuration : CurrentISDuration) + state.ISTickTimer;
            state.ISTickRate = state.ISHasNG ? NGTickRate : CurrentTickRate;

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);
            
            // Update Nature's Grace
            if (state.NGCooldown == 0)
            {
                state.NGCooldown = 60000 - castTime;
                state.NGDurationTimer = 15000 - castTime;
            }
            else
            {
                state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
                state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);
            }

            // Return cast time to caller
            return castTime;
        }

        private int CastSunfire(SimulatorState state, IterationResults results)
        {
            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                ++results.TotalCastCounts[28];
            }
            else
            {
                ++results.TotalCastCounts[12];
            }

            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            // Set SF variables
            state.SFHasNG = state.NGDurationTimer > 0;
            state.SFDurationTimer = (state.SFHasNG ? NGMFDuration : CurrentMFDuration) + state.SFTickTimer;
            state.SFTickRate = state.SFHasNG ? NGTickRate : CurrentTickRate;
            state.MFDurationTimer = state.MFTickTimer = 0;
            state.MFExtendedCounter = 0;

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            if (state.NGCooldown == 0)
            {
                state.NGCooldown = 60000 - castTime;
                state.NGDurationTimer = 15000 - castTime;
            }
            else
            {
                state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
                state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);
            }

            // Return cast time to caller
            return castTime;
        }

        private int CastMoonfire(SimulatorState state, IterationResults results)
        {
            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[27];
                }
                else
                {
                    ++results.TotalCastCounts[26];
                }
            }
            else
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[11];
                }
                else
                {
                    ++results.TotalCastCounts[10];
                }
            }

            // Calculate cast time
            int castTime = (state.NGDurationTimer > 0 ? NGGlobalCooldown : CurrentGlobalCooldown);

            // Set MF variables
            state.MFHasNG = state.NGDurationTimer > 0;
            state.MFHasEclipse = state.InLunarEclipse;
            state.MFDurationTimer = (state.MFHasNG ? NGMFDuration : CurrentMFDuration) + state.MFTickTimer;
            state.MFTickRate = state.MFHasNG ? NGTickRate : CurrentTickRate;
            state.SFDurationTimer = state.SFTickTimer = 0;
            state.MFExtendedCounter = 0;

            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);

            DoTickCalculations(state, results, castTime);

            // Update Nature's Grace
            if (state.NGCooldown == 0)
            {
                state.NGCooldown = 60000 - castTime;
                state.NGDurationTimer = 15000 - castTime;
            }
            else
            {
                state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
                state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);
            }

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
                        ++results.TotalCastCounts[25];
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[24];
                    }
                    else
                    {
                        ++results.TotalCastCounts[23];
                    }
                }
                else
                {
                    castTime = NGStarsurgeCastTime;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[22];
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[21];
                    }
                    else
                    {
                        ++results.TotalCastCounts[20];
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
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[8];
                    }
                    else
                    {
                        ++results.TotalCastCounts[7];
                    }
                }
                else
                {
                    castTime = CurrentStarsurgeCastTime;
                    if (state.InSolarEclipse)
                    {
                        ++results.TotalCastCounts[6];
                    }
                    else if (state.InLunarEclipse)
                    {
                        ++results.TotalCastCounts[5];
                    }
                    else
                    {
                        ++results.TotalCastCounts[4];
                    }
                }
            }

            // Set SS variables
            if (state.ShSProcTimer > 0) state.SSCooldown = (Has4T13 ? 10000 : 15000) - castTime;
            else
            {
                // Adjust the cooldown of Starsurge if the true cast time (equal to current tick rate) is below the 1-second GCD
                int adjustment = 0;
                if (castTime == 1000) adjustment = castTime - (state.NGDurationTimer > 0 ? NGTickRate : CurrentTickRate);
                state.SSCooldown = (Has4T13 ? 10000 : 15000) - adjustment;
            }
            state.ShSProcTimer = 0;

            DoTickCalculations(state, results, castTime);

            state.EclipseEnergy += 15;

            // Update Nature's Grace
            state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Return cast time to caller
            return castTime;
        }

        private int CastWrath(SimulatorState state, IterationResults results)
        {
            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[17];
                }
                else
                {
                    ++results.TotalCastCounts[16];
                }
            }
            else
            {
                if (state.InSolarEclipse)
                {
                    ++results.TotalCastCounts[1];
                }
                else
                {
                    ++results.TotalCastCounts[0];
                }
            }
            // Calculate cast time
            int castTime = state.NGDurationTimer > 0 ? NGWrathCastTime : CurrentWrathCastTime;

            // No variables to set for Wrath

            // Set SS variables before tick calculations in case a Shooting Stars proc occurs
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);
            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);

            DoTickCalculations(state, results, castTime);

            // Set Eclipse energy
            bool euphoriaProcced = state.EuphoriaActive && rng.NextDouble() <= EuphoriaChance;
            state.EclipseEnergy += GetWrathEnergy(Has4T12 && !state.InSolarEclipse, state.WrathCounter++);
            if (euphoriaProcced)
            {
                state.EclipseEnergy += GetWrathEnergy(Has4T12 && !state.InSolarEclipse, state.WrathCounter++);
            }

            // Update Nature's Grace
            state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Return cast time to caller
            return castTime;
        }

        private int CastStarfire(SimulatorState state, IterationResults results)
        {
            // Increment cast tables
            if (state.NGDurationTimer > 0)
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[19];
                }
                else
                {
                    ++results.TotalCastCounts[18];
                }
            }
            else
            {
                if (state.InLunarEclipse)
                {
                    ++results.TotalCastCounts[3];
                }
                else
                {
                    ++results.TotalCastCounts[2];
                }
            }
            // Calculate cast time
            int castTime = state.NGDurationTimer > 0 ? NGStarfireCastTime : CurrentStarfireCastTime;

            // No variables to set for Starfire

            // Set SS variables before tick calculations in case a Shooting Stars proc occurs
            state.ShSProcTimer = Math.Max(0, state.ShSProcTimer - castTime);
            state.SSCooldown = Math.Max(0, state.SSCooldown - castTime);

            DoTickCalculations(state, results, castTime);

            // Set Eclipse energy
            bool euphoriaProcced = state.EuphoriaActive && rng.NextDouble() <= EuphoriaChance;
            state.EclipseEnergy += (Has4T12 && !state.InLunarEclipse ? 25 : 20);
            if (euphoriaProcced)
            {
                state.EclipseEnergy += (Has4T12 && !state.InLunarEclipse ? 25 : 20);
            }

            // Update Nature's Grace
            state.NGCooldown = Math.Max(0, state.NGCooldown - castTime);
            state.NGDurationTimer = Math.Max(0, state.NGDurationTimer - castTime);

            // Perform Glyph of Starfire updates
            if (HasGlyphOfStarfire)
            {
                // Update the NG state regardless of if the Starfire actually extends
                state.MFHasNG = state.NGDurationTimer > 0;
                state.MFTickRate = (state.MFHasNG ? NGTickRate : CurrentTickRate);
                state.SFHasNG = state.NGDurationTimer > 0;
                state.SFTickRate = (state.SFHasNG ? NGTickRate : CurrentTickRate);

                // Extension necessary, calculate the new duration and set it
                if (state.MFExtendedCounter < 3)
                {
                    ++state.MFExtendedCounter;

                    // Handle the case where MF falls off before the SF cast completes - no refresh will take place
                    if (state.MFDurationTimer > 0)
                        state.MFDurationTimer += 3000 + state.MFTickTimer;
                    if (state.SFDurationTimer > 0)
                        state.SFDurationTimer += 3000 + state.SFTickTimer;
                }
            }

            // Return cast time to caller
            return castTime;
        }

        private void DoTickCalculations(SimulatorState state, IterationResults results, int castTime)
        {
            int dotTicks = DoInsectSwarmTicks(state, results, castTime) +
                DoMoonfireTicks(state, results, castTime) +
                DoSunfireTicks(state, results, castTime);

            // Do calculations for Shooting Stars
            if (rng.NextDouble() <= 1 - Math.Pow(1 - ShootingStarsChance, dotTicks))
            {
                state.ShSProcTimer = 12000;
                state.SSCooldown = 0;
            }
        }

        private int DoInsectSwarmTicks(SimulatorState state, IterationResults results, int castTime)
        {
            return DoDotTicks(ref state.ISDurationTimer, state.ISTickRate, ref state.ISTickTimer, castTime, state.ISHasEclipse, state.ISHasNG, 4, results);
        }

        private int DoMoonfireTicks(SimulatorState state, IterationResults results, int castTime)
        {
            return DoDotTicks(ref state.MFDurationTimer, state.MFTickRate, ref state.MFTickTimer, castTime, state.MFHasEclipse, state.MFHasNG, 0, results);
        }

        private int DoSunfireTicks(SimulatorState state, IterationResults results, int castTime)
        {
            return DoDotTicks(ref state.SFDurationTimer, state.SFTickRate, ref state.SFTickTimer, castTime, true, state.SFHasNG, 0, results);
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

        private int GetWrathEnergy(bool t12BonusActive, int currentWrathCounter)
        {
            if (t12BonusActive)
                return currentWrathCounter == 2 ? 16 : 17;
            else
                return currentWrathCounter == 2 ? 14 : 13;
        }
    }
}
