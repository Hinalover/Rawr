using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Hunter.Skills;
using Rawr.Base.Algorithms;

namespace Rawr.Hunter
{


    public class HunterStateNew : State<AbilWrapper>
    {
        private int? shotCount = null;

        public Rotation rotation;

        public int[] shotCooldown { get; set; }
        public int RFDuration { get; set; }
        public int LNLAvailable { get; set; }
        
        public int FocusAmt { get; set; }

        //public bool FocusGT50 { get; set; }
        
        
        /// <summary>
        /// Assuming initial state with Talents.
        /// </summary>
        public HunterStateNew(Rotation rot, int focus = 100)
        {
            if(!shotCount.HasValue)
                shotCount = EnumHelper.GetCount(typeof(Shots));

            shotCooldown = new int[shotCount.Value];

            for (int i = 0; i < shotCooldown.Length; i++)
                shotCooldown[i] = 0;

            Name = "";
            rotation = rot;

            //if (rotation != null && rotation.Talents != null && (Specialization)rotation.Talents.Specialization == Specialization.BeastMastery)
            //    FocusAmt = 120;
            //else
            //    FocusAmt = 100;
            
            this.Index = 0;

            //if (rot != null && rot.Talents != null)
            //{
            //    switch ((Specialization)rot.Talents.Specialization)
            //    {
            //        case Specialization.BeastMastery:
            //            {
            //                Name += "BM";
            //                break;
            //            }
            //        case Specialization.Marksmanship:
            //            {
            //                Name += "MM";
            //                break;
            //            }
            //        case Specialization.Survival:
            //            {
            //                Name += "SV";
            //                shotCooldown[(int)Shots.ExplosiveShot] = 0;
            //                shotCooldown[(int)Shots.BlackArrow] = 0;
            //                shotCooldown[(int)Shots.ArcaneShot] = 0;
            //                shotCooldown[(int)Shots.CobraShot] = 0;
            //                shotCooldown[(int)Shots.KillShot] = 0;
            //                //shotCooldown[(int)Shots.RapidFire] = 0;
            //                break;
            //            }
            //    }
            //}
        }

        public override string ToString()
        {
            //string name = "";

            StringBuilder sbState = new StringBuilder();

            //for (int i = 0; i < shotCooldown.Length; i++)
            //{
            //    if (shotCooldown[i] > 0)
            //    {
            //        sbState.Append(Enum.GetName(typeof(Shots), (object)i) + shotCooldown[i]);
            //        //sbState.Append(Enum.GetName(typeof(Shots), (object)i) + shotCooldown[i]);
            //    }
            //}

            sbState.Append("F");
            sbState.Append(FocusAmt / 5);

            if (LNLAvailable > 0)
            {
                sbState.Append("LNL");
                sbState.Append(LNLAvailable);
            }

            if (shotCooldown[(int)Shots.ExplosiveShot] > 0)
            {
                sbState.Append("ES");
                sbState.Append(shotCooldown[(int)Shots.ExplosiveShot]);
            }

            if (shotCooldown[(int)Shots.BlackArrow] > 0)
            {
                sbState.Append("BA");
                sbState.Append(shotCooldown[(int)Shots.BlackArrow]);
            }

            if (shotCooldown[(int)Shots.KillShot] > 0)
            {
                sbState.Append("KS");
                sbState.Append(shotCooldown[(int)Shots.KillShot]);
            }

            if (shotCooldown[(int)Shots.RapidFire] > 0)
            {
                sbState.Append("RF");
                sbState.Append(shotCooldown[(int)Shots.RapidFire]);
            }
            
            //switch ((Specialization)rotation.Talents.Specialization)
            //{
            //    case Specialization.BeastMastery:
            //        {
            //            string szName = "BM";
            //            name = szName;
            //            //name = string.Format("{0}:Foc{1}FF{2}KC{3}??{4}KS{5}RF{6}", szName,
            //            //    (FocusGT50 ? "+" : "-"),
            //            //    (bFF ? "+" : "-"),
            //            //    (bKC ? "+" : "-"),
            //            //    (bUnknown ? "+" : "-"),
            //            //    (bKS ? "+" : "-"),
            //            //    (bRapidFire ? "+" : "-")
            //            //    );
            //            break;
            //        }
            //    case Specialization.Marksmanship:
            //        {
            //            string szName = "MM";
            //            name = szName;
            //            //name = string.Format("{0}:F50{1}MMM{2}ISS{3}CS{4}KS{5}RF{6}RD{7}", szName,
            //            //    (FocusGT50 ? "+" : "-"),
            //            //    (bMMM ? "+" : "-"),
            //            //    (bISS ? "+" : "-"),
            //            //    (bChimeraShot ? "+" : "-"),
            //            //    (bKS ? "+" : "-"),
            //            //    (bRapidFire ? "+" : "-"),
            //            //    (bReadiness ? "+" : "-")
            //            //    );
            //            break;
            //        }
            
            //}
            return sbState.ToString();
        }



        public override bool Equals(object obj)
        {
            
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to HunterStateNew return false.
            HunterStateNew p = obj as HunterStateNew;
            if ((System.Object)p == null)
            {
                return false;
            }

            return this.Equals(p);

        }

        public bool Equals(HunterStateNew p)
        {
            if (this.FocusAmt != p.FocusAmt)
                return false;

            if (this.RFDuration != p.RFDuration)
                return false;

            if (this.LNLAvailable != p.LNLAvailable)
                return false;

            for (int i = 0; i < shotCooldown.Length; i++)
            {
                if (this.shotCooldown[i] != p.shotCooldown[i])
                    return false;
            }

            return true;
            //return base.Equals(obj);
        }
    }

    class HunterStateSpaceGenerator : StateSpaceGenerator<AbilWrapper>
    {
        private Rotation rotation;
        private FightTime ft;

        public bool Under20Percent = false;
        public bool Over90Percent = false;
        
        #region Global state parameters
        public float PhysicalHaste { get; set; }
        public int FocusMax
        {
            get
            {
                //Kindred Spirits is now baked in
                if (rotation != null && rotation.Talents != null && (Specialization)rotation.Talents.Specialization == Specialization.BeastMastery)
                    return 120;
                else
                    return 100;
            }
        }
        
 
        #endregion

        public HunterStateSpaceGenerator(Rotation HT, FightTime FT)
        {
            rotation = HT;
            ft = FT;

            //BuildTransitionTable();
        }

        

        #region StateChange Chances
        /// <summary>
        /// Get the chance that something will happen based on an incremental change.
        /// </summary>
        /// <param name="fCost">Value change</param>
        /// <param name="CrossOver">Cross Over point</param>
        /// <param name="iMaxRange">Max quantity</param>
        /// <returns>Percentage value that the crossover will happen.</returns>
        private double GetChanceBasedOnRange(float fCost, float fCrossOver, float fMaxRange)
        {
            double output = 1;
            // Ensure that fCost & fCrossover is not > fMaxRange
            if (fCost > fMaxRange
                || fCrossOver > fMaxRange)
            {
                throw new ArgumentOutOfRangeException(string.Format("Cost ({0:0.0}) or Cutoff ({1:0.0}) is outside of Range ({2:0.0}).", fCost, fCrossOver, fMaxRange));
            }
            double fCrossoverPerc = (fCrossOver / fMaxRange);
            double fCostRangePerc = (fCost / fMaxRange);
            output = (fCostRangePerc + fCrossoverPerc);
            output = Math.Max(Math.Min(output, 1), 0);
            return output;
        }

        /// <summary>
        /// Percent chance of changing from Above 50 focus to Below
        /// </summary>
        /// <param name="fCost">Normal cost of ability after glyphs/talents but not counting focus regen</param>
        /// <param name="fCastTime">How long to use the ability.</param>
        /// <returns></returns>
        private double SpendFocus(float fCost, float fCastTime)
        {
            float TimeofFR = Math.Max(fCastTime, 1);
            float FRpS = 4 * (1 + PhysicalHaste);
            float FR = FRpS * TimeofFR;
            return GetChanceBasedOnRange(Math.Abs(fCost - FR), 50f, FocusMax);
        }

        /// <summary>
        /// Percent chance of changing a CD
        /// </summary>
        /// <param name="fCastTime">How long to use the ability.</param>
        /// <returns>Percentage chance that duration will switch that CD to true.</returns>
        private double SpendTimeForCD(float fCastTime, float fCDTime)
        {
            float Time = Math.Max(fCastTime, 1);
            return GetChanceBasedOnRange(Time, 0, fCDTime);
        }
        #endregion

        protected override State<AbilWrapper> GetInitialState()
        {
            HunterStateNew newState = new HunterStateNew(rotation);
            string name = newState.ToString();

            

            newState.FocusAmt = FocusMax;

            HunterStateNew outState;

            if (!stateDictionary.TryGetValue(name, out outState))
            {
                //newState = new HunterState(rotation) { Name = szName, Flags_CDStates = cdStates };
                stateDictionary.Add(name, newState);
                return newState;
            }

            return outState;
        }

        private Dictionary<string, HunterStateNew> stateDictionary = new Dictionary<string, HunterStateNew>();

        public Dictionary<Type, AbilWrapper> AbilityList;

        public HunterStateNew GetState(HunterStateNew incState, AbilWrapper abilUsed)
        {
            string name = "";
            
            HunterStateNew newState = new HunterStateNew(rotation);

            for (int i = 0; i < incState.shotCooldown.Length; i++)
            {
                if ((int)abilUsed.ability.eShot == i && !(abilUsed.ability is ExplosiveShotLNL))
                    newState.shotCooldown[i] = Math.Max(0, Convert.ToInt32(Math.Floor(abilUsed.ability.Cd * 1000))-1500);
                else
                    newState.shotCooldown[i] = Math.Max(0, incState.shotCooldown[i] - 1500);//Convert.ToInt32((Math.Floor(abilUsed.ability.CastTime*1000f))));
            }

            float FRpS = 4 * (1 + PhysicalHaste);
            float FR = FRpS * Math.Max(1.5f,abilUsed.ability.CastTime);

            

            //Factor focus cost against the maximum for the spec
            newState.FocusAmt = Math.Max(0, incState.FocusAmt - Convert.ToInt32(Math.Floor(abilUsed.FocusCost)));
            //Focus cannot be less than zero
            
            newState.FocusAmt = Math.Min(FocusMax, newState.FocusAmt + Convert.ToInt32(Math.Round(FR)));

            
            newState.LNLAvailable = incState.LNLAvailable;

            if (abilUsed.ability is ExplosiveShotLNL)
            {
                newState.LNLAvailable = Math.Max(0, incState.LNLAvailable - 1);
                newState.shotCooldown[(int)Shots.ExplosiveShot] = 0;
            }

            //TODO: OpOv: Add handling of Rapid Fire/Readiness

            name = newState.ToString();

            HunterStateNew outState = new HunterStateNew(rotation);
            
            if (!stateDictionary.TryGetValue(name, out outState))
            {
                //newState = new HunterState(rotation) { Name = szName, Flags_CDStates = cdStates };
                stateDictionary.Add(name, newState);

                return stateDictionary[name];
            }
            return outState;
        }


        protected override List<StateTransition<AbilWrapper>> GetStateTransitions(State<AbilWrapper> state)
        {
            List<StateTransition<AbilWrapper>> output = new List<StateTransition<AbilWrapper>>();
            if (rotation != null)
            {
                switch ((Specialization)rotation.Talents.Specialization)
                {
                    case Specialization.BeastMastery:
                        {
                            output = GetStateTransitionsBM(state);
                            break;
                        }
                    case Specialization.Marksmanship:
                        {
                            output = GetStateTransitionsMM(state);
                            break;
                        }
                    case Specialization.Survival:
                        {
                            output = GetStateTransitionsSV(state);
                            break;
                        }
                }
            }
            else
            {
                // output = GetStateTransitionsBlank(state);
            }
            return output;
        }

        /// <summary>
        /// BeastMaster cycle.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<StateTransition<AbilWrapper>> GetStateTransitionsBM(State<AbilWrapper> state)
        {
            // This roation assumes Serpent Sting is always up.
            HunterStateNew s = state as HunterStateNew;
            List<StateTransition<AbilWrapper>> output = new List<StateTransition<AbilWrapper>>();

            #region Setup Abilities to use.
            //            AbilWrapper FF = AbilityList[typeof(FocusFire)];
            AbilWrapper KS = AbilityList[typeof(KillShot)];
            //            AbilWrapper KC = AbilityList[typeof(KillCommand)];
            AbilWrapper ArcS = AbilityList[typeof(ArcaneShot)];
            AbilWrapper CS = AbilityList[typeof(CobraShot)];
            #endregion

            //HunterStateNew s2 = s.clone();

            if (s.rotation == null)
                throw new ArgumentNullException();

            #region Shot priority.
            #endregion

            return output;
        }

        private List<AbilWrapper> GetAbilityPriorityListMM()
        {
            List<AbilWrapper> abilitiesAvailable = new List<AbilWrapper>();

            abilitiesAvailable.Add(AbilityList[typeof(ChimeraShot)]);

            if (Under20Percent)
                abilitiesAvailable.Add(AbilityList[typeof(KillShot)]);

            //abilitiesAvailable.Add(AbilityList[typeof(Stampede)]);
            abilitiesAvailable.Add(AbilityList[typeof(RapidFire)]);
            abilitiesAvailable.Add(AbilityList[typeof(Readiness)]);
            abilitiesAvailable.Add(AbilityList[typeof(MMMAimedShot)]);
            abilitiesAvailable.Add(AbilityList[typeof(CAAimedShot)]);
            abilitiesAvailable.Add(AbilityList[typeof(AimedShot)]);
            abilitiesAvailable.Add(AbilityList[typeof(ArcaneShot)]);
            abilitiesAvailable.Add(AbilityList[typeof(SteadyShot)]);


            abilitiesAvailable.Add(AbilityList[typeof(Powershot)]);
            abilitiesAvailable.Add(AbilityList[typeof(Barrage)]);
            
            abilitiesAvailable.Add(AbilityList[typeof(DireBeast)]);

            return abilitiesAvailable;
        }

        /// <summary>
        /// Marksman cycle.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<StateTransition<AbilWrapper>> GetStateTransitionsMM(State<AbilWrapper> state)
        {
            // This roation assumes Serpent Sting is always up.
            HunterStateNew s = state as HunterStateNew;
            List<StateTransition<AbilWrapper>> output = new List<StateTransition<AbilWrapper>>();


            List<AbilWrapper> abilityPriorities = GetAbilityPriorityListMM();
            
            if (s == null)
                throw new ArgumentNullException();

            if (s.rotation == null || s.rotation.Talents == null)
                throw new ArgumentNullException();

            //HunterState s2 = s.clone();

            double probabilityRemaining = 1;

            

            for (int loop = 0; loop < abilityPriorities.Count; loop++)
            {
                if (abilityPriorities[loop].ability.Validated != true)
                    continue;

                //if (abilityPriorities[loop].ability is Powershot)
                //    System.Diagnostics.Debugger.Break();

                if (probabilityRemaining == 0)
                    break;

                if (probabilityRemaining > 0 && abilityPriorities[loop].FocusCost < s.FocusAmt && s.shotCooldown[(int)abilityPriorities[loop].ability.eShot] == 0)
                //no special handling required
                {
                    probabilityRemaining = getAbilityTransition(ref output, s, abilityPriorities[loop], probabilityRemaining, probabilityRemaining);
                }
            }
            
            return output;
        }

        /// <summary>
        /// Survival cycle.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// 

        const int transitionDurationDef = 1500;

        private List<AbilWrapper> GetAbilityPriorityListSV()
        {
            List<AbilWrapper> abilitiesAvailable = new List<AbilWrapper>();

            abilitiesAvailable.Add(AbilityList[typeof(ExplosiveShotLNL)]);
            abilitiesAvailable.Add(AbilityList[typeof(GlaiveToss)]);
            abilitiesAvailable.Add(AbilityList[typeof(Powershot)]);
            abilitiesAvailable.Add(AbilityList[typeof(Barrage)]);
            abilitiesAvailable.Add(AbilityList[typeof(BlackArrow)]);

            abilitiesAvailable.Add(AbilityList[typeof(ExplosiveShot)]);

            if(Under20Percent)
                abilitiesAvailable.Add(AbilityList[typeof(KillShot)]);

            abilitiesAvailable.Add(AbilityList[typeof(DireBeast)]);
            
            abilitiesAvailable.Add(AbilityList[typeof(ArcaneShot)]);
            abilitiesAvailable.Add(AbilityList[typeof(CobraShot)]);
            
            return abilitiesAvailable;
        }

        private List<StateTransition<AbilWrapper>> GetStateTransitionsSV(State<AbilWrapper> state)
        {
            

            // This rotation assumes Serpent Sting is always up.
            // This rotation also assumes that Hunter's Mark is always being applied by Marked for Death.. need to confirm
            HunterStateNew s = state as HunterStateNew;
            List<StateTransition<AbilWrapper>> output = new List<StateTransition<AbilWrapper>>();

            List<AbilWrapper> abilityPriorities = GetAbilityPriorityListSV();
            
            if (s.rotation == null || s.rotation.Talents == null)
                throw new ArgumentNullException();

            //HunterState s2 = s.clone();

            double probabilityRemaining = 1;

            #region Shot priority.

            for (int loop = 0; loop < abilityPriorities.Count; loop++)
            {
                if (abilityPriorities[loop].ability.Validated != true)
                    continue;

                //if (abilityPriorities[loop].ability is Powershot)
                //    System.Diagnostics.Debugger.Break();

                if (probabilityRemaining == 0)
                    break;

                if (abilityPriorities[loop].ability is ExplosiveShotLNL)
                {
                    if (s.LNLAvailable > 0 && probabilityRemaining > 0)
                        probabilityRemaining = getAbilityTransition(ref output, s, abilityPriorities[loop], probabilityRemaining, probabilityRemaining);
                    else
                        continue;
                }
                else if (abilityPriorities[loop].ability is BlackArrow)
                {
                    if (probabilityRemaining > 0 && abilityPriorities[loop].FocusCost < s.FocusAmt && s.shotCooldown[(int)Shots.BlackArrow] == 0)
                    {
                        double probOfLNLProcsZero = .1073741824;
                        double probOfLNLProcsTwo = .1808;
                        double probOfLNLProcsOne = 1 - probOfLNLProcsZero - probOfLNLProcsTwo;

                        probabilityRemaining = getAbilityTransition(ref output, s, abilityPriorities[loop], probabilityRemaining, probOfLNLProcsZero);

                        int LNLPrevious = s.LNLAvailable;

                        //LNL Procs once
                        s.LNLAvailable += 2;
                        probabilityRemaining = getAbilityTransition(ref output, s, abilityPriorities[loop], probabilityRemaining, probabilityRemaining);

                        //LNL Procs twice
                        //s.LNLAvailable += 2;
                        //output.Add(new StateTransition<AbilWrapper>
                        //{
                        //    Ability = BA,
                        //    TargetState = GetState(s, BA),
                        //    TransitionDuration = Math.Max(transitionDurationDef, BA.CastTime),
                        //    TransitionProbability = probOfLNLProcsTwo
                        //});

                        s.LNLAvailable = LNLPrevious;

                        probabilityRemaining = 0;
                    }
                    else
                        continue;
                }
                else if (probabilityRemaining > 0 && abilityPriorities[loop].FocusCost < s.FocusAmt && s.shotCooldown[(int)abilityPriorities[loop].ability.eShot] == 0)
                //no special handling required
                {
                    probabilityRemaining = getAbilityTransition(ref output, s, abilityPriorities[loop], probabilityRemaining, probabilityRemaining);
                }
            }

            return output;


            #endregion
        }

        private double getAbilityTransition(ref List<StateTransition<AbilWrapper>> output, HunterStateNew state, AbilWrapper shot, double probRemaining, double probToUse)
        {
            if (probRemaining <= 0)
                return 0;

            output.Add(new StateTransition<AbilWrapper>
            {
                Ability = shot,
                TargetState = GetState(state, shot),
                TransitionDuration = Math.Max(transitionDurationDef, shot.CastTime),
                TransitionProbability = probToUse
            });

            return probRemaining-probToUse;
        }

        private void addTalentShots(ref List<AbilWrapper> abilitiesAvailable, Rotation rotation)
        {
            if (rotation.Talents.Fervor > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.Fervor));
            else if (rotation.Talents.DireBeast > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.DireBeast));
            //else if (rotation.Talents.ThrillOfTheHunt > 0)
              //IMPLEMENT  


            if (rotation.Talents.LynxRush > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.LynxRush));
            else if (rotation.Talents.MurderOfCrows > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.MurderOfCrows));
            
            
            if (rotation.Talents.Barrage > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.Barrage));
            else if (rotation.Talents.GlaiveToss > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.GlaiveToss));
            else if (rotation.Talents.Powershot > 0)
                abilitiesAvailable.Add(new AbilWrapper(rotation.Powershot));

            

            return;
        }

                


        
        
    }
}
