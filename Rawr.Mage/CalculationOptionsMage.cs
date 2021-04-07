using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace Rawr.Mage
{
    public class SpellWeight
    {
        public SpellId Spell { get; set; }
        public float Weight { get; set; }
    }

    public class CooldownRestriction
    {
        public double TimeStart { get; set; }
        public double TimeEnd { get; set; }
        public StateDescription.StateDescriptionDelegate IsMatch { get; set; }
    }

    public struct CooldownStackingCacheEntry
    {
        public double Effect1Duration;
        public double Effect1Cooldown;
        public double Effect2Duration;
        public double Effect2Cooldown;
        public double MaximumStackingDuration;
    }

    public class CooldownOffset
    {
        public StandardEffect Effect {get;set;}
        public string Name {get;set;}
        public double Offset {get;set;}
    }

    [GenerateSerializer]
    public sealed class CalculationOptionsMage : ICalculationOptionBase, INotifyPropertyChanged, ICharacterCalculationOptions
    {
        private int playerLevel;
        private float hasteRatingMultiplier;
        private float critRatingMultiplier;
        private float masteryRatingMultiplier;
        private float hitRatingMultiplier;
        private float pvpResilienceMultiplier;
        private float pvpPowerMultiplier;
        private float spellScalingFactor;

        public int PlayerLevel
        {
            get
            {
                return playerLevel;
            }
            set
            {
                playerLevel = value;
                hasteRatingMultiplier = BaseCombatRating.SpellHasteRatingMultiplier(value);
                critRatingMultiplier = BaseCombatRating.SpellCritRatingMultiplier(value);
                masteryRatingMultiplier = BaseCombatRating.MasteryRatingMultiplier(value);
                hitRatingMultiplier = BaseCombatRating.SpellHitRatingMultiplier(value);
                pvpResilienceMultiplier = BaseCombatRating.PvPResilienceRatingRatingMultiplier(value);
                pvpPowerMultiplier = BaseCombatRating.PvPPowerRatingRatingMultiplier(value);
                spellScalingFactor = BaseCombatRating.MageSpellScaling(value);
                OnPropertyChanged("PlayerLevel");
            }
        }

        public float HasteRatingMultiplier
        {
            get
            {
                return hasteRatingMultiplier;
            }
        }

        public float CritRatingMultiplier
        {
            get
            {
                return critRatingMultiplier;
            }
        }

        public float MasteryRatingMultiplier
        {
            get
            {
                return masteryRatingMultiplier;
            }
        }

        public float HitRatingMultiplier
        {
            get
            {
                return hitRatingMultiplier;
            }
        }

        public float PvPResilienceMultiplier
        {
            get
            {
                return pvpResilienceMultiplier;
            }
        }

        public float PvPPowerMultiplier
        {
            get
            {
                return pvpPowerMultiplier;
            }
        }

        public float SpellScalingFactor
        {
            get
            {
                return spellScalingFactor;
            }
        }

        public float BaseMana
        {
            get
            {
                return BaseCombatRating.MageBaseMana(playerLevel);
            }
        }

        public float GetSpellValueRound(float value)
        {
            return (float)Math.Round(spellScalingFactor * value);
        }

        public float GetSpellValue(float value)
        {
            return spellScalingFactor * value;
        }

        public const float SetBonus4T8ProcRate = 0.25f;

        /*private bool _ModePTR;
        public bool ModePTR
        {
            get { return _ModePTR; }
            set { _ModePTR = value; OnPropertyChanged("ModePTR"); }
        }*/

        private bool _BossHandler;
        public bool BossHandler
        {
            get { return _BossHandler; }
            set { _BossHandler = value; OnPropertyChanged("BossHandler"); }
        }

        private float _FrostbiteUtilization;
        public float FrostbiteUtilization
        {
            get { return _FrostbiteUtilization; }
            set { _FrostbiteUtilization = value; OnPropertyChanged("FrostbiteUtilization"); }
        }

        private bool _MaxUseAssumption;
        public bool MaxUseAssumption
        {
            get { return _MaxUseAssumption; }
            set 
            { 
                _MaxUseAssumption = value;
                UpdateCooldownStackingCache();
                OnPropertyChanged("MaxUseAssumption"); 
            }
        }

        private int _TargetLevel;
        [XmlElement("TargetLevel")]
        public int CustomTargetLevel
        {
            get { return _TargetLevel; }
            set { _TargetLevel = value; OnPropertyChanged("CustomTargetLevel"); }
        }

        [XmlIgnore]
        public int TargetLevel
        {
            get
            {
                if (BossHandler)
                {
                    return Character.BossOptions.Level;
                }
                else
                {
                    return CustomTargetLevel;
                }
            }
        }

        private int _AoeTargetLevel;
        public int AoeTargetLevel
        {
            get { return _AoeTargetLevel; }
            set { _AoeTargetLevel = value; OnPropertyChanged("AoeTargetLevel"); }
        }

        private float _LatencyCast;
        public float LatencyCast
        {
            get { return _LatencyCast; }
            set { _LatencyCast = value; OnPropertyChanged("LatencyCast"); }
        }

        private float _LatencyGCD;
        public float LatencyGCD
        {
            get { return _LatencyGCD; }
            set { _LatencyGCD = value; OnPropertyChanged("LatencyGCD"); }
        }

        private float _LatencyChannel;
        public float LatencyChannel
        {
            get { return _LatencyChannel; }
            set { _LatencyChannel = value; OnPropertyChanged("LatencyChannel"); }
        }

        private float _MovementFrequency;
        public float MovementFrequency
        {
            get { return _MovementFrequency; }
            set { _MovementFrequency = value; OnPropertyChanged("MovementFrequency"); }
        }

        private float _MovementDuration;
        public float MovementDuration
        {
            get { return _MovementDuration; }
            set { _MovementDuration = value; OnPropertyChanged("MovementDuration"); }
        }

        private int _AoeTargets;
        public int AoeTargets
        {
            get { return _AoeTargets; }
            set { _AoeTargets = value; OnPropertyChanged("AoeTargets"); }
        }

        private float _ArcaneResist;
        [XmlElement("ArcaneResist")]
        public float CustomArcaneResist
        {
            get { return _ArcaneResist; }
            set { _ArcaneResist = value; OnPropertyChanged("CustomArcaneResist"); }
        }

        [XmlIgnore]
        public float ArcaneResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Arcane;
                }
                else
                {
                    return CustomArcaneResist;
                }
            }
        }

        private float _FireResist;
        [XmlElement("FireResist")]
        public float CustomFireResist
        {
            get { return _FireResist; }
            set { _FireResist = value; OnPropertyChanged("CustomFireResist"); }
        }

        [XmlIgnore]
        public float FireResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Fire;
                }
                else
                {
                    return CustomFireResist;
                }
            }
        }

        private float _FrostResist;
        [XmlElement("FrostResist")]
        public float CustomFrostResist
        {
            get { return _FrostResist; }
            set { _FrostResist = value; OnPropertyChanged("CustomFrostResist"); }
        }

        [XmlIgnore]
        public float FrostResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Frost;
                }
                else
                {
                    return CustomFrostResist;
                }
            }
        }

        private float _NatureResist;
        [XmlElement("NatureResist")]
        public float CustomNatureResist
        {
            get { return _NatureResist; }
            set { _NatureResist = value; OnPropertyChanged("CustomNatureResist"); }
        }

        [XmlIgnore]
        public float NatureResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Nature;
                }
                else
                {
                    return CustomNatureResist;
                }
            }
        }


        private float _ShadowResist;
        [XmlElement("ShadowResist")]
        public float CustomShadowResist
        {
            get { return _ShadowResist; }
            set { _ShadowResist = value; OnPropertyChanged("CustomShadowResist"); }
        }

        [XmlIgnore]
        public float ShadowResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Shadow;
                }
                else
                {
                    return CustomShadowResist;
                }
            }
        }

        private float _HolyResist;
        [XmlElement("HolyResist")]
        public float CustomHolyResist
        {
            get { return _HolyResist; }
            set { _HolyResist = value; OnPropertyChanged("CustomHolyResist"); }
        }

        [XmlIgnore]
        public float HolyResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Holy;
                }
                else
                {
                    return CustomHolyResist;
                }
            }
        }

        private float _FightDuration;
        [XmlElement("FightDuration")]
        public float CustomFightDuration
        {
            get { return _FightDuration; }
            set 
            {
                _FightDuration = value;
                UpdateCooldownStackingCache();
                OnPropertyChanged("CustomFightDuration");
            }
        }

        public float FightDuration
        {
            get
            {
                if (BossHandler && Character != null)
                {
                    return Character.BossOptions.BerserkTimer;
                }
                else
                {
                    return CustomFightDuration;
                }
            }
        }

        public float EffectiveEffectFightDuration(SpecialEffect effect)
        {
            if (effect.LimitedToExecutePhase)
            {
                return FightDuration * MoltenFuryPercentage;
            }
            return FightDuration;
        }

        private static Dictionary<float, List<CooldownStackingCacheEntry>> cooldownStackingCacheMapNoMaxUseAssumption = new Dictionary<float, List<CooldownStackingCacheEntry>>();
        private static Dictionary<float, List<CooldownStackingCacheEntry>> cooldownStackingCacheMap = new Dictionary<float, List<CooldownStackingCacheEntry>>();

        [XmlIgnore]
        public List<CooldownStackingCacheEntry> CooldownStackingCache { get; private set; }

        private void UpdateCooldownStackingCache()
        {
            var map = MaxUseAssumption ? cooldownStackingCacheMap : cooldownStackingCacheMapNoMaxUseAssumption;
            lock(map)
            {
                List<CooldownStackingCacheEntry> cache;
                map.TryGetValue(FightDuration, out cache);
                if (cache == null)
                {
                    cache = new List<CooldownStackingCacheEntry>();
                    map[FightDuration] = cache;
                }
                CooldownStackingCache = cache;
            }
        }

        private bool _HeroismAvailable;
        public bool HeroismAvailable
        {
            get { return _HeroismAvailable; }
            set { _HeroismAvailable = value; OnPropertyChanged("HeroismAvailable"); }
        }

        private bool _PowerInfusionAvailable;
        public bool PowerInfusionAvailable
        {
            get { return _PowerInfusionAvailable; }
            set { _PowerInfusionAvailable = value; OnPropertyChanged("PowerInfusionAvailable"); }
        }

        private bool _VolcanicPotion;
        public bool VolcanicPotion
        {
            get { return _VolcanicPotion; }
            set { _VolcanicPotion = value; OnPropertyChanged("VolcanicPotion"); }
        }

        private bool _ArcaneLight;
        public bool ArcaneLight
        {
            get { return _ArcaneLight; }
            set { _ArcaneLight = value; OnPropertyChanged("ArcaneLight"); }
        }

        private bool _SimpleStacking;
        public bool SimpleStacking
        {
            get { return _SimpleStacking; }
            set { _SimpleStacking = value; OnPropertyChanged("SimpleStacking"); }
        }

        private bool _ProcCombustion;
        public bool ProcCombustion
        {
            get { return _ProcCombustion; }
            set { _ProcCombustion = value; OnPropertyChanged("ProcCombustion"); }
        }

        private float _MoltenFuryPercentage;
        [XmlElement("MoltenFuryPercentage")]
        public float CustomMoltenFuryPercentage
        {
            get { return _MoltenFuryPercentage; }
            set { _MoltenFuryPercentage = value; OnPropertyChanged("CustomMoltenFuryPercentage"); }
        }

        [XmlIgnore]
        public float MoltenFuryPercentage
        {
            get
            {
                if (BossHandler)
                {
                    return (float)(Character.BossOptions.Under20Perc + Character.BossOptions.Under35Perc);
                }
                else
                {
                    return CustomMoltenFuryPercentage;
                }
            }
        }

        private bool _MaintainScorch;
        public bool MaintainScorch
        {
            get { return _MaintainScorch; }
            set { _MaintainScorch = value; OnPropertyChanged("MaintainScorch"); }
        }

        private bool _MaintainSnare;
        public bool MaintainSnare
        {
            get { return _MaintainSnare; }
            set { _MaintainSnare = value; OnPropertyChanged("MaintainSnare"); }
        }

        private float _InterruptFrequency;
        public float InterruptFrequency
        {
            get { return _InterruptFrequency; }
            set { _InterruptFrequency = value; OnPropertyChanged("InterruptFrequency"); }
        }

        private float _AoeDuration;
        public float AoeDuration
        {
            get { return _AoeDuration; }
            set { _AoeDuration = value; OnPropertyChanged("AoeDuration"); }
        }

        private bool _SmartOptimization;
        public bool SmartOptimization
        {
            get { return _SmartOptimization; }
            set { _SmartOptimization = value; OnPropertyChanged("SmartOptimization"); }
        }

        private bool _CombinatorialSolver;
        public bool CombinatorialSolver
        {
            get { return _CombinatorialSolver; }
            set { _CombinatorialSolver = value; OnPropertyChanged("CombinatorialSolver"); }
        }

        private bool _GeneticSolver;
        public bool GeneticSolver
        {
            get { return _GeneticSolver; }
            set { _GeneticSolver = value; OnPropertyChanged("GeneticSolver"); }
        }

        private int _GeneticThoroughness;
        public int GeneticThoroughness
        {
            get { return _GeneticThoroughness; }
            set { _GeneticThoroughness = value; OnPropertyChanged("GeneticThoroughness"); }
        }

        private string _CombinatorialFixedOrdering;
        public string CombinatorialFixedOrdering
        {
            get { return _CombinatorialFixedOrdering; }
            set { _CombinatorialFixedOrdering = value; OnPropertyChanged("CombinatorialFixedOrdering"); }
        }

        private float _DpsTime;
        public float DpsTime
        {
            get { return _DpsTime; }
            set { _DpsTime = value; OnPropertyChanged("DpsTime"); }
        }

        private bool _ForceIncrementalOptimizations;
        public bool ForceIncrementalOptimizations
        {
            get { return _ForceIncrementalOptimizations; }
            set { _ForceIncrementalOptimizations = value; OnPropertyChanged("ForceIncrementalOptimizations"); }
        }

        private bool _IncrementalOptimizations;
        public bool IncrementalOptimizations
        {
            get { return _IncrementalOptimizations; }
            set { _IncrementalOptimizations = value; OnPropertyChanged("IncrementalOptimizations"); }
        }

        private float _SnaredTime;
        public float SnaredTime
        {
            get { return _SnaredTime; }
            set { _SnaredTime = value; OnPropertyChanged("SnaredTime"); }
        }

        [XmlIgnore]
        public int[] IncrementalSetStateIndexes;
        [XmlIgnore]
        public int[] IncrementalSetSortedStates;
        [XmlIgnore]
        public int[] IncrementalSetSegments;
        [XmlIgnore]
        public CycleId[] IncrementalSetSpells;
        [XmlIgnore]
        public VariableType[] IncrementalSetVariableType;
        [XmlIgnore]
        public int[] IncrementalSetManaSegment;
        [XmlIgnore]
        public CycleId[] IncrementalSetManaNeutralMix;
        [XmlIgnore]
        public Rawr.Mage.SequenceReconstruction.Sequence SequenceReconstruction;
        [XmlIgnore]
        public bool AdviseAdvancedSolver;
        [XmlIgnore]
        public DisplayCalculations Calculations; // calculations that are result of the last display in rawr

        // cached cycle solutions
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFrB;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFFB;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFFBS;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KILS;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KDFS;

        [XmlIgnore]
        private Character _character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                value.CalculationsInvalidated += new EventHandler(Character_ItemsChanged);
                value.BossOptions.PropertyChanged += new PropertyChangedEventHandler(BossOptions_PropertyChanged);
                _character = value;
                if (BossHandler)
                {
                    UpdateCooldownStackingCache();
                }
            }
        }

        private void BossOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BossHandler)
            {
                if (e.PropertyName == "BerserkTimer")
                {
                    UpdateCooldownStackingCache();
                }
            }
        }

        private void Character_ItemsChanged(object sender, EventArgs e)
        {
            IncrementalSetStateIndexes = null;
            IncrementalSetSegments = null;
            IncrementalSetSortedStates = null;
            IncrementalSetSpells = null;
            IncrementalSetManaSegment = null;
            IncrementalSetVariableType = null;
            CooldownRestrictionList = null;
            CooldownOffsetList = null;
        }

        private bool _ReconstructSequence;
        public bool ReconstructSequence
        {
            get { return _ReconstructSequence; }
            set { _ReconstructSequence = value; OnPropertyChanged("ReconstructSequence"); }
        }

        private bool _ComparisonSegmentCooldowns;
        public bool ComparisonSegmentCooldowns
        {
            get { return _ComparisonSegmentCooldowns; }
            set { _ComparisonSegmentCooldowns = value; OnPropertyChanged("ComparisonSegmentCooldowns"); }
        }

        private bool _DisplaySegmentCooldowns;
        public bool DisplaySegmentCooldowns
        {
            get { return _DisplaySegmentCooldowns; }
            set { _DisplaySegmentCooldowns = value; OnPropertyChanged("DisplaySegmentCooldowns"); }
        }

        private bool _ComparisonIntegralMana;
        public bool ComparisonIntegralMana
        {
            get { return _ComparisonIntegralMana; }
            set { _ComparisonIntegralMana = value; OnPropertyChanged("ComparisonIntegralMana"); }
        }

        private bool _DisplayIntegralMana;
        public bool DisplayIntegralMana
        {
            get { return _DisplayIntegralMana; }
            set { _DisplayIntegralMana = value; OnPropertyChanged("DisplayIntegralMana"); }
        }

        private bool _ComparisonSegmentMana;
        public bool ComparisonSegmentMana
        {
            get { return _ComparisonSegmentMana; }
            set { _ComparisonSegmentMana = value; OnPropertyChanged("ComparisonSegmentMana"); }
        }

        private bool _DisplaySegmentMana;
        public bool DisplaySegmentMana
        {
            get { return _DisplaySegmentMana; }
            set { _DisplaySegmentMana = value; OnPropertyChanged("DisplaySegmentMana"); }
        }

        private int _ComparisonAdvancedConstraintsLevel;
        public int ComparisonAdvancedConstraintsLevel
        {
            get { return _ComparisonAdvancedConstraintsLevel; }
            set { _ComparisonAdvancedConstraintsLevel = value; OnPropertyChanged("ComparisonAdvancedConstraintsLevel"); }
        }

        private int _DisplayAdvancedConstraintsLevel;
        public int DisplayAdvancedConstraintsLevel
        {
            get { return _DisplayAdvancedConstraintsLevel; }
            set { _DisplayAdvancedConstraintsLevel = value; OnPropertyChanged("DisplayAdvancedConstraintsLevel"); }
        }

        private int _FixedSegmentDuration;
        public int FixedSegmentDuration
        {
            get { return _FixedSegmentDuration; }
            set { _FixedSegmentDuration = value; OnPropertyChanged("FixedSegmentDuration"); }
        }

        private bool _VariableSegmentDuration;
        public bool VariableSegmentDuration
        {
            get { return _VariableSegmentDuration; }
            set { _VariableSegmentDuration = value; OnPropertyChanged("VariableSegmentDuration"); }
        }

        private string _AdditionalSegmentSplits;
        public string AdditionalSegmentSplits
        {
            get { return _AdditionalSegmentSplits; }
            set { _AdditionalSegmentSplits = value; OnPropertyChanged("AdditionalSegmentSplits"); }
        }

        private double _LowerBoundHint;
        public double LowerBoundHint
        {
            get { return _LowerBoundHint; }
            set { _LowerBoundHint = value; OnPropertyChanged("LowerBoundHint"); }
        }

        private string _CooldownRestrictions;
        public string CooldownRestrictions
        {
            get { return _CooldownRestrictions; }
            set { _CooldownRestrictions = value; OnPropertyChanged("CooldownRestrictions"); }
        }

        private float _RuneOfPowerInterval;
        public float RuneOfPowerInterval
        {
            get { return _RuneOfPowerInterval; }
            set { _RuneOfPowerInterval = value; OnPropertyChanged("RuneOfPowerInterval"); }
        }

        private bool _EnableHastedEvocation;
        public bool EnableHastedEvocation
        {
            get { return _EnableHastedEvocation; }
            set { _EnableHastedEvocation = value; OnPropertyChanged("EnableHastedEvocation"); }
        }

        private bool _AdvancedHasteProcs;
        public bool AdvancedHasteProcs
        {
            get { return _AdvancedHasteProcs; }
            set { _AdvancedHasteProcs = value; OnPropertyChanged("AdvancedHasteProcs"); }
        }

        /*private bool _EncounterEnabled;
        public bool EncounterEnabled
        {
            get { return _EncounterEnabled; }
            set { _EncounterEnabled = value; OnPropertyChanged("EncounterEnabled"); }
        }

        public Encounter Encounter { get; set; }*/

        [XmlIgnore]
        public List<CooldownOffset> CooldownOffsetList;

        private string _CooldownOffset;
        public string CooldownOffset
        {
            get { return _CooldownOffset; }
            set { _CooldownOffset = value; OnPropertyChanged("CooldownOffset"); }
        }

        [XmlIgnore]
        public List<CooldownRestriction> CooldownRestrictionList;
        public bool CooldownRestrictionsValid(Segment segment, CastingState state)
        {
            if (CooldownRestrictionList == null) return true;
            foreach (CooldownRestriction restriction in CooldownRestrictionList)
            {
                if (segment.TimeStart >= restriction.TimeStart && segment.TimeEnd <= restriction.TimeEnd)
                {
                    if (!restriction.IsMatch(state.Effects)) return false;
                }
            }
            return true;
        }

        public float GetDamageMultiplier(SequenceReconstruction.SequenceItem variable)
        {
            return GetDamageMultiplier(variable.Timestamp, variable.Timestamp + variable.Duration);
        }

        public float GetDamageMultiplier(Segment segment)
        {
            return GetDamageMultiplier(segment.TimeStart, segment.TimeEnd);
        }

        public float GetDamageMultiplier(double timeStart, double timeEnd)
        {
            float mult = 1.0f;
            if (BossHandler)
            {
                // we can assume that the phase boundaries coincide with segment boundaries
                // that is, a phase is composed of a number of whole segments
                // check if this segment belongs to any damage multiplier phases
                foreach (var buffState in Character.BossOptions.BuffStates)
                {
                    if (buffState.Chance > 0 && buffState.Stats.BonusDamageMultiplier > 0)
                    {
                        foreach (var phase in buffState.PhaseTimes)
                        {
                            if (timeStart >= phase.Value[0] && timeEnd <= phase.Value[1])
                            {
                                // weight by state frequency (duration is in milliseconds)
                                mult *= (1 + buffState.Stats.BonusDamageMultiplier) * buffState.Duration / buffState.Frequency / 1000.0f;
                            }
                        }
                    }
                }
            }
            return mult;
        }

        private MIPMethod _MIPMethod;
        public MIPMethod MIPMethod
        {
            get { return _MIPMethod; }
            set { _MIPMethod = value; OnPropertyChanged("MIPMethod"); }
        }

        private bool _IncludeManaNeutralCycleMix;
        public bool IncludeManaNeutralCycleMix
        {
            get { return _IncludeManaNeutralCycleMix; }
            set { _IncludeManaNeutralCycleMix = value; OnPropertyChanged("IncludeManaNeutralCycleMix"); }
        }

        private bool _DisableManaRegenCycles;
        public bool DisableManaRegenCycles
        {
            get { return _DisableManaRegenCycles; }
            set { _DisableManaRegenCycles = value; OnPropertyChanged("DisableManaRegenCycles"); }
        }

        private float _Innervate;
        public float Innervate
        {
            get { return _Innervate; }
            set { _Innervate = value; OnPropertyChanged("Innervate"); }
        }

        private float _Fragmentation;
        public float Fragmentation
        {
            get { return _Fragmentation; }
            set { _Fragmentation = value; OnPropertyChanged("Fragmentation"); }
        }

        private float _SurvivabilityRating;
        public float SurvivabilityRating
        {
            get { return _SurvivabilityRating; }
            set { _SurvivabilityRating = value; OnPropertyChanged("SurvivabilityRating"); }
        }

        private bool _PVP;
        public bool PVP
        {
            get { return _PVP; }
            set { _PVP = value; OnPropertyChanged("PVP"); }
        }

        private int _HeroismControl;
        // 0 = optimal, 1 = before 20%, 2 = no cooldowns, 3 = after 20%
        public int HeroismControl
        {
            get { return _HeroismControl; }
            set { _HeroismControl = value; OnPropertyChanged("HeroismControl"); }
        }

        [XmlIgnore]
        public string HeroismControlText
        {
            get
            {
                switch (_HeroismControl)
                {
                    case 0:
                    default:
                        return "Optimal";
                    case 1:
                        return "Before 35%";
                    case 2:
                        return "No Cooldowns";
                    case 3:
                        return "After 35%";
                }
            }
            set
            {
                switch (value)
                {
                    case "Optimal":
                    default:
                        _HeroismControl = 0;
                        break;
                    case "Before 35%":
                        _HeroismControl = 1;
                        break;
                    case "No Cooldowns":
                        _HeroismControl = 2;
                        break;
                    case "After 35%":
                        _HeroismControl = 3;
                        break;
                }
                OnPropertyChanged("HeroismControlText");
            }
        }

        private bool _AverageCooldowns;
        public bool AverageCooldowns
        {
            get { return _AverageCooldowns; }
            set { _AverageCooldowns = value; OnPropertyChanged("AverageCooldowns"); }
        }

        private bool _EvocationEnabled;
        public bool EvocationEnabled
        {
            get { return _EvocationEnabled; }
            set { _EvocationEnabled = value; OnPropertyChanged("EvocationEnabled"); }
        }

        private bool _ManaPotionEnabled;
        public bool ManaPotionEnabled
        {
            get { return _ManaPotionEnabled; }
            set { _ManaPotionEnabled = value; OnPropertyChanged("ManaPotionEnabled"); }
        }

        private bool _ManaGemEnabled;
        public bool ManaGemEnabled
        {
            get { return _ManaGemEnabled; }
            set { _ManaGemEnabled = value; OnPropertyChanged("ManaGemEnabled"); }
        }

        private int _MirrorImage; // 0 = disabled, 1 = averaged, 2 = cooldown
        public int MirrorImage
        {
            get { return _MirrorImage; }
            set { _MirrorImage = value; OnPropertyChanged("MirrorImage"); }
        }

        [XmlIgnore]
        public string MirrorImageText
        {
            get
            {
                switch (_MirrorImage)
                {
                    case 0:
                    default:
                        return "Disabled";
                    case 1:
                        return "Averaged";
                    case 2:
                        return "Cooldown";
                }
            }
            set
            {
                switch (value)
                {
                    case "Disabled":
                    default:
                        _MirrorImage = 0;
                        break;
                    case "Averaged":
                        _MirrorImage = 1;
                        break;
                    case "Cooldown":
                        _MirrorImage = 2;
                        break;
                }
                OnPropertyChanged("MirrorImageText");
            }
        }

        private bool _DisableCooldowns;
        public bool DisableCooldowns
        {
            get { return _DisableCooldowns; }
            set { _DisableCooldowns = value; OnPropertyChanged("DisableCooldowns"); }
        }

        private int _MaxHeapLimit;
        public int MaxHeapLimit
        {
            get { return _MaxHeapLimit; }
            set { _MaxHeapLimit = value; OnPropertyChanged("MaxHeapLimit"); }
        }

        private float _DrinkingTime;
        public float DrinkingTime
        {
            get { return _DrinkingTime; }
            set { _DrinkingTime = value; OnPropertyChanged("DrinkingTime"); }
        }

        private float _TargetDamage;
        public float TargetDamage
        {
            get { return _TargetDamage; }
            set { _TargetDamage = value; OnPropertyChanged("TargetDamage"); }
        }

        private bool _FarmingMode;
        public bool FarmingMode
        {
            get { return _FarmingMode; }
            set { _FarmingMode = value; OnPropertyChanged("FarmingMode"); }
        }

        private bool _UnlimitedMana;
        public bool UnlimitedMana
        {
            get { return _UnlimitedMana; }
            set { _UnlimitedMana = value; OnPropertyChanged("UnlimitedMana"); }
        }

        private float _AbsorptionPerSecond;
        public float AbsorptionPerSecond
        {
            get { return _AbsorptionPerSecond; }
            set { _AbsorptionPerSecond = value; OnPropertyChanged("AbsorptionPerSecond"); }
        }


        public List<SpellWeight> CustomSpellMix { get; set; }
        private bool _CustomSpellMixEnabled;
        public bool CustomSpellMixEnabled
        {
            get { return _CustomSpellMixEnabled; }
            set { _CustomSpellMixEnabled = value; OnPropertyChanged("CustomSpellMixEnabled"); }
        }

        private int _GlobalRestarts;
        public int GlobalRestarts
        {
            get { return _GlobalRestarts; }
            set { _GlobalRestarts = value; OnPropertyChanged("GlobalRestarts"); }
        }

        private int _MaxRedecompose;
        public int MaxRedecompose
        {
            get { return _MaxRedecompose; }
            set { _MaxRedecompose = value; OnPropertyChanged("MaxRedecompose"); }
        }

        private bool _CustomSpellMixOnly;
        public bool CustomSpellMixOnly
        {
            get { return _CustomSpellMixOnly; }
            set { _CustomSpellMixOnly = value; OnPropertyChanged("CustomSpellMixOnly"); }
        }


        private float _MeleeDps;
        public float MeleeDps
        {
            get { return _MeleeDps; }
            set { _MeleeDps = value; OnPropertyChanged("MeleeDps"); }
        }

        private float _MeleeCrit;
        public float MeleeCrit
        {
            get { return _MeleeCrit; }
            set { _MeleeCrit = value; OnPropertyChanged("MeleeCrit"); }
        }

        private float _MeleeDot;
        public float MeleeDot
        {
            get { return _MeleeDot; }
            set { _MeleeDot = value; OnPropertyChanged("MeleeDot"); }
        }

        private float _PhysicalDps;
        public float PhysicalDps
        {
            get { return _PhysicalDps; }
            set { _PhysicalDps = value; OnPropertyChanged("PhysicalDps"); }
        }

        private float _PhysicalCrit;
        public float PhysicalCrit
        {
            get { return _PhysicalCrit; }
            set { _PhysicalCrit = value; OnPropertyChanged("PhysicalCrit"); }
        }

        private float _PhysicalDot;
        public float PhysicalDot
        {
            get { return _PhysicalDot; }
            set { _PhysicalDot = value; OnPropertyChanged("PhysicalDot"); }
        }

        private float _FireDps;
        public float FireDps
        {
            get { return _FireDps; }
            set { _FireDps = value; OnPropertyChanged("FireDps"); }
        }

        private float _FireCrit;
        public float FireCrit
        {
            get { return _FireCrit; }
            set { _FireCrit = value; OnPropertyChanged("FireCrit"); }
        }

        private float _FireDot;
        public float FireDot
        {
            get { return _FireDot; }
            set { _FireDot = value; OnPropertyChanged("FireDot"); }
        }

        private float _FrostDps;
        public float FrostDps
        {
            get { return _FrostDps; }
            set { _FrostDps = value; OnPropertyChanged("FrostDps"); }
        }

        private float _FrostCrit;
        public float FrostCrit
        {
            get { return _FrostCrit; }
            set { _FrostCrit = value; OnPropertyChanged("FrostCrit"); }
        }

        private float _FrostDot;
        public float FrostDot
        {
            get { return _FrostDot; }
            set { _FrostDot = value; OnPropertyChanged("FrostDot"); }
        }

        private float _ArcaneDps;
        public float ArcaneDps
        {
            get { return _ArcaneDps; }
            set { _ArcaneDps = value; OnPropertyChanged("ArcaneDps"); }
        }

        private float _ArcaneCrit;
        public float ArcaneCrit
        {
            get { return _ArcaneCrit; }
            set { _ArcaneCrit = value; OnPropertyChanged("ArcaneCrit"); }
        }

        private float _ArcaneDot;
        public float ArcaneDot
        {
            get { return _ArcaneDot; }
            set { _ArcaneDot = value; OnPropertyChanged("ArcaneDot"); }
        }

        private float _ShadowDps;
        public float ShadowDps
        {
            get { return _ShadowDps; }
            set { _ShadowDps = value; OnPropertyChanged("ShadowDps"); }
        }

        private float _ShadowCrit;
        public float ShadowCrit
        {
            get { return _ShadowCrit; }
            set { _ShadowCrit = value; OnPropertyChanged("ShadowCrit"); }
        }

        private float _ShadowDot;
        public float ShadowDot
        {
            get { return _ShadowDot; }
            set { _ShadowDot = value; OnPropertyChanged("ShadowDot"); }
        }

        private float _NatureDps;
        public float NatureDps
        {
            get { return _NatureDps; }
            set { _NatureDps = value; OnPropertyChanged("NatureDps"); }
        }

        private float _NatureCrit;
        public float NatureCrit
        {
            get { return _NatureCrit; }
            set { _NatureCrit = value; OnPropertyChanged("NatureCrit"); }
        }

        private float _NatureDot;
        public float NatureDot
        {
            get { return _NatureDot; }
            set { _NatureDot = value; OnPropertyChanged("NatureDot"); }
        }

        private float _HolyDps;
        public float HolyDps
        {
            get { return _HolyDps; }
            set { _HolyDps = value; OnPropertyChanged("HolyDps"); }
        }

        private float _HolyCrit;
        public float HolyCrit
        {
            get { return _HolyCrit; }
            set { _HolyCrit = value; OnPropertyChanged("HolyCrit"); }
        }

        private float _HolyDot;
        public float HolyDot
        {
            get { return _HolyDot; }
            set { _HolyDot = value; OnPropertyChanged("HolyDot"); }
        }


        private float _BurstWindow;
        public float BurstWindow
        {
            get { return _BurstWindow; }
            set { _BurstWindow = value; OnPropertyChanged("BurstWindow"); }
        }

        private float _BurstImpacts;
        public float BurstImpacts
        {
            get { return _BurstImpacts; }
            set { _BurstImpacts = value; OnPropertyChanged("BurstImpacts"); }
        }

        private float _PassiveHealing;
        public float PassiveHealing
        {
            get { return _PassiveHealing; }
            set { _PassiveHealing = value; OnPropertyChanged("PassiveHealing"); }
        }

        //public float ChanceToLiveLimit { get; set; }
        private float _ChanceToLiveScore;
        public float ChanceToLiveScore
        {
            get { return _ChanceToLiveScore; }
            set { _ChanceToLiveScore = value; OnPropertyChanged("ChanceToLiveScore"); }
        }

        private float _ChanceToLiveAttenuation;
        public float ChanceToLiveAttenuation
        {
            get { return _ChanceToLiveAttenuation; }
            set { _ChanceToLiveAttenuation = value; OnPropertyChanged("ChanceToLiveAttenuation"); }
        }

        private float _EffectSpiritMultiplier;
        public float EffectSpiritMultiplier
        {
            get { return _EffectSpiritMultiplier; }
            set { _EffectSpiritMultiplier = value; OnPropertyChanged("EffectSpiritMultiplier"); }
        }

        private float _EffectCritBonus;
        public float EffectCritBonus
        {
            get { return _EffectCritBonus; }
            set { _EffectCritBonus = value; OnPropertyChanged("EffectCritBonus"); }
        }

        private float _EffectCritDamageBonus;
        public float EffectCritDamageBonus
        {
            get { return _EffectCritDamageBonus; }
            set { _EffectCritDamageBonus = value; OnPropertyChanged("EffectCritDamageBonus"); }
        }

        private float _EffectDamageMultiplier;
        public float EffectDamageMultiplier
        {
            get { return _EffectDamageMultiplier; }
            set { _EffectDamageMultiplier = value; OnPropertyChanged("EffectDamageMultiplier"); }
        }

        private float _EffectHasteMultiplier;
        public float EffectHasteMultiplier
        {
            get { return _EffectHasteMultiplier; }
            set { _EffectHasteMultiplier = value; OnPropertyChanged("EffectHasteMultiplier"); }
        }

        private float _EffectCostMultiplier;
        public float EffectCostMultiplier
        {
            get { return _EffectCostMultiplier; }
            set { _EffectCostMultiplier = value; OnPropertyChanged("EffectCostMultiplier"); }
        }

        private float _EffectRegenMultiplier;
        public float EffectRegenMultiplier
        {
            get { return _EffectRegenMultiplier; }
            set { _EffectRegenMultiplier = value; OnPropertyChanged("EffectRegenMultiplier"); }
        }

        private bool _EffectDisableManaSources;
        public bool EffectDisableManaSources
        {
            get { return _EffectDisableManaSources; }
            set { _EffectDisableManaSources = value; OnPropertyChanged("EffectDisableManaSources"); }
        }

        private float _EffectShadowSilenceFrequency;
        public float EffectShadowSilenceFrequency
        {
            get { return _EffectShadowSilenceFrequency; }
            set { _EffectShadowSilenceFrequency = value; OnPropertyChanged("EffectShadowSilenceFrequency"); }
        }

        private float _EffectShadowSilenceDuration;
        public float EffectShadowSilenceDuration
        {
            get { return _EffectShadowSilenceDuration; }
            set { _EffectShadowSilenceDuration = value; OnPropertyChanged("EffectShadowSilenceDuration"); }
        }

        private float _EffectShadowManaDrainFrequency;
        public float EffectShadowManaDrainFrequency
        {
            get { return _EffectShadowManaDrainFrequency; }
            set { _EffectShadowManaDrainFrequency = value; OnPropertyChanged("EffectShadowManaDrainFrequency"); }
        }

        private float _EffectShadowManaDrain;
        public float EffectShadowManaDrain
        {
            get { return _EffectShadowManaDrain; }
            set { _EffectShadowManaDrain = value; OnPropertyChanged("EffectShadowManaDrain"); }
        }

        private float _EffectArcaneOtherBinary;
        public float EffectArcaneOtherBinary
        {
            get { return _EffectArcaneOtherBinary; }
            set { _EffectArcaneOtherBinary = value; OnPropertyChanged("EffectArcaneOtherBinary"); }
        }

        private float _EffectFireOtherBinary;
        public float EffectFireOtherBinary
        {
            get { return _EffectFireOtherBinary; }
            set { _EffectFireOtherBinary = value; OnPropertyChanged("EffectFireOtherBinary"); }
        }

        private float _EffectFrostOtherBinary;
        public float EffectFrostOtherBinary
        {
            get { return _EffectFrostOtherBinary; }
            set { _EffectFrostOtherBinary = value; OnPropertyChanged("EffectFrostOtherBinary"); }
        }

        private float _EffectShadowOtherBinary;
        public float EffectShadowOtherBinary
        {
            get { return _EffectShadowOtherBinary; }
            set { _EffectShadowOtherBinary = value; OnPropertyChanged("EffectShadowOtherBinary"); }
        }

        private float _EffectNatureOtherBinary;
        public float EffectNatureOtherBinary
        {
            get { return _EffectNatureOtherBinary; }
            set { _EffectNatureOtherBinary = value; OnPropertyChanged("EffectNatureOtherBinary"); }
        }

        private float _EffectHolyOtherBinary;
        public float EffectHolyOtherBinary
        {
            get { return _EffectHolyOtherBinary; }
            set { _EffectHolyOtherBinary = value; OnPropertyChanged("EffectHolyOtherBinary"); }
        }

        private float _EffectArcaneOther;
        public float EffectArcaneOther
        {
            get { return _EffectArcaneOther; }
            set { _EffectArcaneOther = value; OnPropertyChanged("EffectArcaneOther"); }
        }

        private float _EffectFireOther;
        public float EffectFireOther
        {
            get { return _EffectFireOther; }
            set { _EffectFireOther = value; OnPropertyChanged("EffectFireOther"); }
        }

        private float _EffectFrostOther;
        public float EffectFrostOther
        {
            get { return _EffectFrostOther; }
            set { _EffectFrostOther = value; OnPropertyChanged("EffectFrostOther"); }
        }

        private float _EffectShadowOther;
        public float EffectShadowOther
        {
            get { return _EffectShadowOther; }
            set { _EffectShadowOther = value; OnPropertyChanged("EffectShadowOther"); }
        }

        private float _EffectNatureOther;
        public float EffectNatureOther
        {
            get { return _EffectNatureOther; }
            set { _EffectNatureOther = value; OnPropertyChanged("EffectNatureOther"); }
        }

        private float _EffectHolyOther;
        public float EffectHolyOther
        {
            get { return _EffectHolyOther; }
            set { _EffectHolyOther = value; OnPropertyChanged("EffectHolyOther"); }
        }

		private bool _EnhancedArcaneBlast;
		public bool EnhancedArcaneBlast
		{
			get { return _EnhancedArcaneBlast; }
			set { _EnhancedArcaneBlast = value; OnPropertyChanged("EnhancedArcaneBlast"); }
		}

		private bool _ImprovedArcanePower;
		public bool ImprovedArcanePower
		{
			get { return _ImprovedArcanePower; }
			set { _ImprovedArcanePower = value; OnPropertyChanged("ImprovedArcanePower"); }
		}

		private bool _ImprovedBlink;
		public bool ImprovedBlink
		{
			get { return _ImprovedBlink; }
			set { _ImprovedBlink = value; OnPropertyChanged("ImprovedBlink"); }
		}

		private bool _ImprovedEvocation;
		public bool ImprovedEvocation
		{
			get { return _ImprovedEvocation; }
			set { _ImprovedEvocation = value; OnPropertyChanged("ImprovedEvocation"); }
		}

        public CalculationOptionsMage Clone()
        {
            CalculationOptionsMage clone = (CalculationOptionsMage)MemberwiseClone();
            clone.IncrementalSetStateIndexes = null;
            clone.IncrementalSetSegments = null;
            clone.IncrementalSetSpells = null;
            clone.IncrementalSetVariableType = null;
            clone.IncrementalSetManaSegment = null;
            return clone;
        }

        public CalculationOptionsMage()
        {
            CustomTargetLevel = 103;
            AoeTargetLevel = 100;
            LatencyCast = 0.01f;
            LatencyGCD = 0.01f;
            LatencyChannel = 0.2f;
            AoeTargets = 9;
            CustomFightDuration = 300;
            HeroismAvailable = true;
            CustomMoltenFuryPercentage = 0.3f;
            VolcanicPotion = true;
            DpsTime = 1;
            InterruptFrequency = 0;
            AoeDuration = 0;
            SmartOptimization = true;
            //TpsLimit = 0;
            IncrementalOptimizations = true;
            ReconstructSequence = false;
            Innervate = 0;
            Fragmentation = 1;
            SurvivabilityRating = 0.0001f;
            EvocationEnabled = true;
            ManaPotionEnabled = true;
            ManaGemEnabled = true;
            MaxHeapLimit = 300;
            DrinkingTime = 300;
            BurstWindow = 5f;
            BurstImpacts = 5f;
            MirrorImage = 1;
            //ChanceToLiveLimit = 99f;
            PlayerLevel = 100;
            SnaredTime = 1f;
            FixedSegmentDuration = 30;
            EffectSpiritMultiplier = 1.0f;
            EffectDamageMultiplier = 1.0f;
            EffectHasteMultiplier = 1.0f;
            EffectRegenMultiplier = 1.0f;
            EffectCostMultiplier = 1.0f;
            ChanceToLiveAttenuation = 0.1f;
            MaxUseAssumption = true;
            FrostbiteUtilization = 1.0f;
            ComparisonAdvancedConstraintsLevel = 1;
            DisplayAdvancedConstraintsLevel = 1;
            ComparisonSegmentMana = true;
            DisplaySegmentMana = true;
            IncludeManaNeutralCycleMix = true;
            ArcaneLight = false;
            DisableManaRegenCycles = true;
            MaxRedecompose = 50;
            ProcCombustion = true;
            GeneticThoroughness = 200;
            AdvancedHasteProcs = true;
            SimpleStacking = true;
            RuneOfPowerInterval = 45;
			EnhancedArcaneBlast = true;
			ImprovedArcanePower = true;
			ImprovedBlink = true;
			ImprovedEvocation = true;
        }

        public CalculationOptionsMage(Character character)
            : this()
        {
            _character = character;
            character.CalculationsInvalidated += new EventHandler(Character_ItemsChanged);
        }

        string ICalculationOptionBase.GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMage));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
