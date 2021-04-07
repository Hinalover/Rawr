using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.ProtPaladin
{
    public class ProtPaladinAbility
    {
        public string Name { get; set; }
    }

    public class ProtPaladinState : State<ProtPaladinAbility>
    {
        public int CSCD { get; set; }
        public int JCD { get; set; }
        public int ASCD { get; set; }
        public int ConsCD { get; set; }
        public int HoWCD { get; set; }
        public int HWCD { get; set; }
        public int GCDuration { get; set; }
        public int WBDuration { get; set; }
        public int HPCount { get; set; }
        public bool PrevDPProc { get; set; }
    }

    public class ProtPaladinStateGenerator : StateSpaceGenerator<ProtPaladinAbility>
    {
        private Dictionary<string, ProtPaladinState> _stateSpace = new Dictionary<string, ProtPaladinState>();

        public ProtPaladinAbility CrusaderStrike = new ProtPaladinAbility { Name = "CS" };
        public ProtPaladinAbility Judgment = new ProtPaladinAbility { Name = "J" };
        public ProtPaladinAbility AvengersShield = new ProtPaladinAbility { Name = "AS" };
        public ProtPaladinAbility Consecration = new ProtPaladinAbility { Name = "Cons" };
        public ProtPaladinAbility HolyWrath = new ProtPaladinAbility { Name = "HW" };
        public ProtPaladinAbility HammerOfWrath = new ProtPaladinAbility { Name = "HoW" };
        public ProtPaladinAbility HammerOfTheRighteous = new ProtPaladinAbility { Name = "HotR" };
        public ProtPaladinAbility ShieldOfTheRighteous = new ProtPaladinAbility { Name = "SotR" };
        public ProtPaladinAbility None = new ProtPaladinAbility { Name = "None" };

        public int JudgmentCooldown = 6000;
        public bool HoWActive;
        public bool HolyAvengerActive;
        public bool DivinePurpose;

        protected override State<ProtPaladinAbility> GetInitialState()
        {
            // All spells off CD, GC and WB down, 0 HP, no previous DP proc
            return GetState(0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        }

        protected override List<StateTransition<ProtPaladinAbility>> GetStateTransitions(State<ProtPaladinAbility> state)
        {
            ProtPaladinState theState = state as ProtPaladinState;
            List<StateTransition<ProtPaladinAbility>> retval = new List<StateTransition<ProtPaladinAbility>>();

            if (theState.WBDuration == 0 && theState.CSCD == 0)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = HammerOfTheRighteous,
                    TargetState = GetState(4500 - 1500, theState.JCD - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, 30000 - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 0.8
                });
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = HammerOfTheRighteous,
                    TargetState = GetState(4500 - 1500, theState.JCD - 1500, 0, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, 6000 - 1500, 30000 - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 0.2
                });
            }
            else if (theState.HPCount == 3)
            {
                int transitionTime = theState.PrevDPProc ? 1500 : 0;
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = ShieldOfTheRighteous,
                    TargetState = GetState(theState.CSCD - transitionTime, theState.JCD - transitionTime, theState.ASCD - transitionTime, theState.ConsCD - transitionTime, theState.HoWCD - transitionTime, theState.HWCD - transitionTime, theState.GCDuration - transitionTime, theState.WBDuration - transitionTime, 0, false),
                    TransitionDuration = transitionTime,
                    TransitionProbability = DivinePurpose ? 0.75 : 1
                });
                if (DivinePurpose)
                {
                    retval.Add(new StateTransition<ProtPaladinAbility>
                    {
                        Ability = ShieldOfTheRighteous,
                        TargetState = GetState(theState.CSCD - transitionTime, theState.JCD - transitionTime, theState.ASCD - transitionTime, theState.ConsCD - transitionTime, theState.HoWCD - transitionTime, theState.HWCD - transitionTime, theState.GCDuration - transitionTime, theState.WBDuration - transitionTime, theState.HPCount, true),
                        TransitionDuration = transitionTime,
                        TransitionProbability = 0.25
                    });
                }
            }
            else if (JudgmentCooldown < 4500 && theState.JCD == 0)
            {
                // Use a variable for Judgment cooldown to accommodate reduction from Sanctified Wrath
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = Judgment,
                    TargetState = GetState(theState.CSCD - 1500, JudgmentCooldown - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else if (theState.CSCD == 0)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = CrusaderStrike,
                    TargetState = GetState(4500 - 1500, theState.JCD - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 0.8
                });
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = CrusaderStrike,
                    TargetState = GetState(4500 - 1500, theState.JCD - 1500, 0, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, 6000 - 1500, theState.WBDuration - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 0.2
                });
            }
            else if (theState.JCD == 0)
            {
                // Use a variable for Judgment cooldown to accommodate reduction from Sanctified Wrath
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = Judgment,
                    TargetState = GetState(theState.CSCD - 1500, JudgmentCooldown - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount + (HolyAvengerActive ? 3 : 1), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else if (theState.ASCD == 0)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = AvengersShield,
                    TargetState = GetState(theState.CSCD - 1500, theState.JCD - 1500, 15000 - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount + (theState.GCDuration > 0 ? (HolyAvengerActive ? 3 : 1) : 0), false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else if (theState.HoWCD == 0 && HoWActive)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = HammerOfWrath,
                    TargetState = GetState(theState.CSCD - 1500, theState.JCD - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, 6000 - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount, false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else if (theState.ConsCD == 0)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = Consecration,
                    TargetState = GetState(theState.CSCD - 1500, theState.JCD - 1500, theState.ASCD - 1500, 9000 - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount, false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else if (theState.HWCD == 0)
            {
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = HolyWrath,
                    TargetState = GetState(theState.CSCD - 1500, theState.JCD - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, 9000 - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount, false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }
            else
            {
                // Add a dummy state for empty GCD's
                retval.Add(new StateTransition<ProtPaladinAbility>
                {
                    Ability = None,
                    TargetState = GetState(theState.CSCD - 1500, theState.JCD - 1500, theState.ASCD - 1500, theState.ConsCD - 1500, theState.HoWCD - 1500, theState.HWCD - 1500, theState.GCDuration - 1500, theState.WBDuration - 1500, theState.HPCount, false),
                    TransitionDuration = 1500,
                    TransitionProbability = 1
                });
            }

            return retval;
        }

        private ProtPaladinState GetState(int csCD, int jCD, int asCD, int consCD, int howCD, int hwCD, int gcDuration, int wbDuration, int hpCount, bool prevDPProc)
        {
            ProtPaladinState theState;

            csCD = Math.Max(0, csCD);
            jCD = Math.Max(0, jCD);
            asCD = Math.Max(0, asCD);
            consCD = Math.Max(0, consCD);
            howCD = Math.Max(0, howCD);
            hwCD = Math.Max(0, hwCD);
            gcDuration = Math.Max(0, gcDuration);
            wbDuration = Math.Max(0, wbDuration);
            hpCount = Math.Max(0, Math.Min(3, hpCount));

            string stateKey = String.Format("CS{0}J{1}AS{2}CON{3}HOW{4}HW{5}GC{6}WB{7}HP{8}DP{9}",
                csCD, jCD, asCD, consCD, howCD, hwCD, gcDuration, wbDuration, hpCount, prevDPProc ? 1 : 0);

            if (!_stateSpace.TryGetValue(stateKey, out theState))
            {
                theState = new ProtPaladinState
                {
                    CSCD = csCD,
                    JCD = jCD,
                    ASCD = asCD,
                    ConsCD = consCD,
                    HoWCD = howCD,
                    HWCD = hwCD,
                    GCDuration = gcDuration,
                    WBDuration = wbDuration,
                    HPCount = hpCount,
                    PrevDPProc = prevDPProc,
                    Name = stateKey
                };
                _stateSpace.Add(stateKey, theState);
            }

            return theState;
        }
    }

    public class ProtPaladinMarkovSolver
    {
        private MarkovProcess<ProtPaladinAbility> _process;
        private ProtPaladinStateGenerator _generator;

        public int JudgmentCooldown = 6000;
        public bool HoWActive;
        public bool HolyAvengerActive;
        public bool DivinePurpose;

        public Dictionary<string, double> GetCastDistributionTable()
        {
            _generator = new ProtPaladinStateGenerator { JudgmentCooldown = JudgmentCooldown, HoWActive = HoWActive, HolyAvengerActive = HolyAvengerActive, DivinePurpose = DivinePurpose };
            _process = new MarkovProcess<ProtPaladinAbility>(_generator.GenerateStateSpace());
            Dictionary<string, double> retval = new Dictionary<string, double>();
            foreach (KeyValuePair<ProtPaladinAbility, double> kvp in _process.AbilityWeight)
                retval.Add(kvp.Key.Name, kvp.Value);
            return retval;
        }

        public float GetRotationLength()
        {
            float retval = 0f;
            double totalWeight = 0f;
            double[] transitionTimes = _process.GetAverageTimeToEnd(st => ((ProtPaladinState)st).HPCount == 3);

            foreach (ProtPaladinState st in _process.StateSpace.Where(s => ((ProtPaladinState)s).HPCount == 0))
            {
                int index = _process.StateSpace.IndexOf(st);
                totalWeight += _process.StateWeight[index];
            }

            foreach (ProtPaladinState st in _process.StateSpace.Where(s => ((ProtPaladinState)s).HPCount == 0))
            {
                int index = _process.StateSpace.IndexOf(st);
                retval += (float)(transitionTimes[index] * (_process.StateWeight[index]) / totalWeight);
            }
            return retval;
        }
    }
}
