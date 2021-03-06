using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using Rawr.Mage.SequenceReconstruction;

namespace Rawr.Mage
{
    public enum VariableType
    {
        None = 0,
        IdleRegen,
        Evocation,
        EvocationIV,
        EvocationHero,
        EvocationIVHero,
        ManaPotion,
        ManaGem,
        Drinking,
        TimeExtension,
        AfterFightRegen,
        ManaOverflow,
        Spell,
        SummonWaterElemental,
        SummonMirrorImage,
        ConjureManaGem,
        //Ward
    }

    public class SolutionVariable
    {
        public int Segment;
        public int ManaSegment;
        public CastingState State;
        public Cycle Cycle;
        public VariableType Type;
        public double Dps;
        public double Mps;
        //public double Tps;

        public bool IsZeroTime
        {
            get
            {
                return Type == VariableType.ManaPotion || Type == VariableType.ManaGem || Type == VariableType.ManaOverflow;
            }
        }

        public bool IsMatch(int effects, VariableType cooldownType)
        {
            return ((effects != 0 && State != null && State.EffectsActive(effects) && (cooldownType == VariableType.None || Type == cooldownType)) || (effects == 0 && Type == cooldownType));
        }
    }

    /// <summary>
    /// Lightweight storage for target debuffs, just store the relevant stats
    /// instead of creating a full blown Stats object
    /// </summary>
    public class TargetDebuffStats
    {
        public float SpellCritOnTarget { get; set; }
        public float SpellHaste { get; set; }
        public float BonusDamageMultiplier { get; set; }
        public float BonusFrostDamageMultiplier { get; set; }
        public float BonusFireDamageMultiplier { get; set; }
        public float BonusHolyDamageMultiplier { get; set; }

        public void Accumulate(Stats stats)
        {
            SpellCritOnTarget += stats.SpellCritOnTarget;
            SpellHaste = (1 + SpellHaste) * (1 + stats.SpellHaste) - 1;
            BonusDamageMultiplier = (1 + BonusDamageMultiplier) * (1 + stats.BonusDamageMultiplier) - 1;
            BonusFrostDamageMultiplier = (1 + BonusFrostDamageMultiplier) * (1 + stats.BonusFrostDamageMultiplier) - 1;
            BonusFireDamageMultiplier = (1 + BonusFireDamageMultiplier) * (1 + stats.BonusFireDamageMultiplier) - 1;
            BonusHolyDamageMultiplier = (1 + BonusHolyDamageMultiplier) * (1 + stats.BonusHolyDamageMultiplier) - 1;
        }
    }

    public sealed class DisplayCalculations
    {
        public Specialization Specialization { get; set; }

        //public CalculationsMage Calculations { get; set; }
        public Stats BaseStats { get; set; }
        public CastingState BaseState { get; set; }
        public CalculationOptionsMage CalculationOptions { get; set; }
        public MageTalents MageTalents { get; set; }
        public Character Character { get; set; }

        public List<EffectCooldown> CooldownList { get; set; }
        public Dictionary<int, EffectCooldown> EffectCooldown { get; set; }
        public EffectCooldown[] ItemBasedEffectCooldowns { get; set; }

        public SpecialEffect[] SpellPowerEffects { get; set; }
        public SpecialEffect[] IntellectEffects { get; set; }
        public SpecialEffect[] HasteRatingEffects { get; set; }
        public SpecialEffect[] MasteryRatingEffects { get; set; }
        public SpecialEffect[] CritRatingEffects { get; set; }

        public List<Segment> SegmentList { get; set; }

        public float BaseGlobalCooldown { get; set; }

        public float StartingMana { get; set; }

        public bool ManaGemEffect { get; set; }

        public float ManaAdeptBonus { get; set; }
        public float IgniteBonus { get; set; }
        public float FrostburnBonus { get; set; }

        public float RawArcaneHitRate { get; set; }
        public float RawFireHitRate { get; set; }
        public float RawFrostHitRate { get; set; }

        public float EvocationDuration;
        public float EvocationRegen;
        public float EvocationDurationIV;
        public float EvocationRegenIV;
        public float EvocationDurationHero;
        public float EvocationRegenHero;
        public float EvocationDurationIVHero;
        public float EvocationRegenIVHero;

        public int MaxManaGem;
        public float MaxEvocation;
        public float ManaGemTps;
        public float ManaPotionTps;
        public float ManaGemValue;
        public float MaxManaGemValue;
        public float ManaPotionValue;
        public float MaxManaPotionValue;

        public float CombustionCooldown;
        public float PowerInfusionDuration;
        public float PowerInfusionCooldown;
        public float MirrorImageDuration;
        public float MirrorImageCooldown;
        public float IcyVeinsCooldown;
        public float ColdsnapCooldown;
        public float ArcanePowerCooldown;
        public float ArcanePowerDuration;
        //public float WaterElementalCooldown;
        //public float WaterElementalDuration;
        public float EvocationCooldown;
        public float ManaGemEffectDuration;

        //public float FrBDFFFBIL_KFrB;
        //public float FrBDFFFBIL_KFFB;
        //public float FrBDFFFBIL_KFFBS;
        //public float FrBDFFFBIL_KILS;
        //public float FrBDFFFBIL_KDFS;

        public double[] Solution;
        public List<SolutionVariable> SolutionVariable;
        //public float Tps;
        public double UpperBound = double.PositiveInfinity;
        public double LowerBound = 0;

        public Cycle ConjureManaGem { get; set; }
        public int MaxConjureManaGem { get; set; }
        public Cycle Ward { get; set; }
        public int MaxWards { get; set; }

        public float ChanceToDie { get; set; }
        public float MeanIncomingDps { get; set; }
        public float DamageTakenReduction { get; set; }

        public float MasteryRatingMultiplier { get; set; }
        public float CritRatingMultiplier { get; set; }
        public float MultistrikeRatingMultiplier { get; set; }

        public string ReconstructSequence()
        {
            CalculationOptions.SequenceReconstruction = null;
            CalculationOptions.AdviseAdvancedSolver = false;
            if (!CalculationOptions.ReconstructSequence) return "*Disabled";
            if (CalculationOptions.FightDuration > 900) return "*Unavailable";

            StringBuilder timing = new StringBuilder();
            double bestUnexplained = double.PositiveInfinity;
            string bestTiming = "*";

            SequenceItem.Calculations = this;
            double unexplained;
            Sequence sequence = GenerateRawSequence(false);
            if (!sequence.SortGroups(DisplaySolver))
            {
                //sequence = GenerateRawSequence(true);
                //sequence.SortGroups(displaySolver);
            }
            /*foreach (SequenceItem item in sequence.sequence)
            {
                item.MinTime = SegmentList[item.Segment].TimeStart;
                item.MaxTime = SegmentList[item.Segment].TimeEnd - item.Duration;
            }*/


            // mana gem/pot/evo positioning
            if (Specialization != Specialization.Arcane && !CalculationOptions.SimpleStacking && CalculationOptions.CooldownRestrictionList == null && !CalculationOptions.BossHandler)
            {
                sequence.RepositionManaConsumption();
            }

            sequence.RemoveIndex(VariableType.TimeExtension);
            sequence.Compact(true);
            if (DisplaySolver == null || CalculationsMage.IsSolverEnabled(DisplaySolver))
            {
                CalculationOptions.SequenceReconstruction = sequence;
            }

            // evaluate sequence
            unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }

            if (unexplained > 0 && !(CalculationOptions.DisplaySegmentCooldowns && CalculationOptions.DisplayIntegralMana && CalculationOptions.DisplaySegmentMana && CalculationOptions.DisplayAdvancedConstraintsLevel >= 5))
            {
                CalculationOptions.AdviseAdvancedSolver = true;
                bestTiming = "*Sequence Reconstruction was not fully successful, it is recommended that you enable more options in\r\nadvanced solver (segment cooldowns, segment mana, integral mana consumables, advanced constraints options)!\r\n\r\n" + bestTiming.TrimStart('*');
            }

            return bestTiming;
        }

        private Sequence GenerateRawSequence(bool ignoreSegments)
        {
            Sequence sequence = new Sequence();

            double totalTime = 0.0;
            double totalGem = 0.0;
            int columnIdleRegen = 0;
            int columnManaOverflow = -1;
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (SolutionVariable[i].Type == VariableType.IdleRegen)
                {
                    columnIdleRegen = i;
                    break;
                }
            }
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (SolutionVariable[i].Type == VariableType.ManaGem)
                {
                    sequence.ColumnManaGem = i;
                    break;
                }
            }
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (SolutionVariable[i].Type == VariableType.ManaPotion)
                {
                    sequence.ColumnManaPotion = i;
                    break;
                }
            }
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (SolutionVariable[i].Type == VariableType.ManaOverflow)
                {
                    columnManaOverflow = i;
                    break;
                }
            }
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01 && SolutionVariable[i].Type != VariableType.ManaOverflow)
                {
                    SequenceItem item = new SequenceItem(i, Solution[i]);
                    if (ignoreSegments) item.Segment = 0;
                    sequence.Add(item);
                    if (!item.IsManaPotionOrGem) totalTime += item.Duration;
                    if (item.VariableType == VariableType.ManaGem) totalGem += Solution[i];
                }
            }
            if (CalculationOptions.TargetDamage == 0.0 && totalTime < CalculationOptions.FightDuration - 0.00001)
            {
                sequence.Add(new SequenceItem(columnIdleRegen, CalculationOptions.FightDuration - totalTime));
            }

            // evaluate sequence

            /*unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }*/

            /*SequenceGroup heroismGroup = sequence.GroupHeroism();
            if (CalculationOptions.HeroismControl == 3)
            {
                heroismGroup.MinTime = Math.Min(CalculationOptions.FightDuration - CalculationOptions.MoltenFuryPercentage * CalculationOptions.FightDuration, CalculationOptions.FightDuration - 40.0);
            }            
            sequence.GroupCombustion();
            sequence.GroupArcanePower();
            sequence.GroupVolcanicPotion();
            foreach (EffectCooldown cooldown in ItemBasedEffectCooldowns)
            {
                sequence.GroupSpecialEffect(cooldown);
            }
            List<SequenceGroup> list = sequence.GroupManaGemEffect();
            if (list != null && ManaGemEffect && CalculationOptions.DisplaySegmentCooldowns && columnManaOverflow != -1)
            {
                float manaBurn = 0;
                for (int i = 0; i < SolutionVariable.Count; i++)
                {
                    if (Solution[i] > 0.01 && SolutionVariable[i].Segment == 0 && SolutionVariable[i].Type == VariableType.Spell)
                    {
                        CastingState state = SolutionVariable[i].State;
                        if (state != null && !state.EffectsActive((int)StandardEffect.ManaGemEffect))
                        {
                            float burn = (float)SolutionVariable[i].Cycle.ManaPerSecond;
                            if (burn > manaBurn) manaBurn = burn;
                        }
                    }
                }

                double overflow = Solution[columnManaOverflow];
                double tmin = 0;
                if (manaBurn > 0) tmin = (ManaGemValue * (1 + BaseStats.BonusManaGem) - overflow) / manaBurn;

                foreach (SequenceGroup g in list)
                {
                    if (g.Segment == 0) g.MinTime = tmin;
                }
            }
            sequence.GroupIcyVeins(); // should come after trinkets because of coldsnap
            //sequence.GroupWaterElemental();
            sequence.GroupMirrorImage();
            sequence.GroupBerserking();
            sequence.GroupPowerInfusion();
            sequence.GroupEvocation();*/
            return sequence;
        }

        private string GetHitRatingDescription(float hitRate)
        {
            float diff = (hitRate - 1) * 100 * CalculationOptions.HitRatingMultiplier;
            if (diff >= 1)
            {
                return " (" + (int)Math.Floor(diff) + " hit rating over cap)";
            }
            else if (diff <= -1)
            {
                return " (" + (int)Math.Ceiling(-diff) + " hit rating to cap)";
            }
            return string.Empty;
        }

        /*public float MinimumRange
        {
            get
            {
                float minRange = float.PositiveInfinity;
                foreach (SpellContribution contrib in DamageSources.Values)
                {
                    if (contrib.Range > 0 && contrib.Range < minRange)
                    {
                        minRange = contrib.Range;
                    }
                }
                return minRange;
            }
        }*/

        /*public float ThreatReduction
        {
            get
            {
                return 1 - Tps / BaseCalculations.DpsRating;
            }
        }*/

        public Solver DisplaySolver { get; set; }

        public Dictionary<string, string> DisplayCalculationValues { get; private set; }
        public Dictionary<string, SpellContribution> DamageSources { get; private set; }
        public Dictionary<string, float> ManaSources { get; private set; }
        public Dictionary<string, float> ManaUsage { get; private set; }
        public CharacterCalculationsMage BaseCalculations { get; set; }

        public Dictionary<string, string> GetCharacterDisplayCalculationValues(bool computeReconstruction)
        {
            Dictionary<string, string> dictValues = DisplayCalculationValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BaseStats.Stamina.ToString());
            dictValues.Add("Intellect", BaseStats.Intellect.ToString());
			dictValues.Add("Spirit", BaseStats.Spirit.ToString());
			dictValues.Add("Armor", BaseStats.Armor.ToString());
            dictValues.Add("Health", BaseStats.Health.ToString());
            dictValues.Add("Mana", BaseStats.Mana.ToString());
			dictValues.Add("Critical Strike", String.Format("{0:F}%*Crit {1} (+{2:F}% crit chance)", 100 * Math.Max(0, BaseState.CritRate), BaseStats.CritRating, BaseStats.CritRating / CalculationOptions.CritRatingMultiplier));
            if (ManaAdeptBonus > 0)
            {
                dictValues.Add("Mastery", String.Format("{0:F}%*Mastery {1}", ManaAdeptBonus * 100, (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier)));
            }
            else if (IgniteBonus > 0)
            {
                dictValues.Add("Mastery", String.Format("{0:F}*Mastery rating {1} (+{2:F} mastery)\r\nIgnite {3:F}%", BaseState.Mastery, (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier), (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier) / CalculationOptions.MasteryRatingMultiplier, IgniteBonus * 100));
            }
            else if (FrostburnBonus > 0)
            {
                dictValues.Add("Mastery", String.Format("{0:F}*Mastery rating {1} (+{2:F} mastery)\r\nFrostburn {3:F}%", BaseState.Mastery, (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier), (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier) / CalculationOptions.MasteryRatingMultiplier, FrostburnBonus * 100));
            }
            else
            {
                dictValues.Add("Mastery", String.Format("{0:F}*Mastery rating {1} (+{2:F} mastery)", BaseState.Mastery, (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier), (float)Math.Round(BaseStats.MasteryRating * MasteryRatingMultiplier) / CalculationOptions.MasteryRatingMultiplier));
            }
            // hit rating = hitrate * 800 / levelScalingFactor
            dictValues.Add("Hit Chance", String.Format("+{0:F}%", 100 * BaseState.SpellHit));
            dictValues.Add("Haste", String.Format("+{0:F}%*Haste {1} (+{2:F}% haste)", 100 * (BaseState.CastingSpeed - 1f), BaseState.SpellHasteRating, BaseState.SpellHasteRating / CalculationOptions.HasteRatingMultiplier));
            float arcaneSpellPower = BaseState.ArcaneSpellPower * (1 + BaseStats.BonusSpellPowerMultiplier);
            float fireSpellPower = BaseState.FireSpellPower * (1 + BaseStats.BonusSpellPowerMultiplier);
            float frostSpellPower = BaseState.FrostSpellPower * (1 + BaseStats.BonusSpellPowerMultiplier);
            float spellPower = Math.Min(arcaneSpellPower, Math.Min(fireSpellPower, frostSpellPower));
            dictValues.Add("Spell Power", string.Format("{0:F}*Arcane {1:F}\r\nFire {2:F}\r\nFrost {3:F}", spellPower, arcaneSpellPower, fireSpellPower, frostSpellPower));
            dictValues.Add("Mana Regen", String.Format("{0}*Out of combat: {1}", Math.Floor(BaseState.ManaRegen5SR * 5), Math.Floor(BaseState.ManaRegen * 5)));
            dictValues.Add("Physical Mitigation", String.Format("{0:F}%", 100 * BaseState.MeleeMitigation));
            dictValues.Add("PVP Resilience", string.Format("{0:F}%*Resilience {1} (+{2:F}% Resilience)", 100 * DamageTakenReduction, BaseStats.PvPResilience, 100 * (DamageTakenReduction - 0.4f)));
            dictValues.Add("PVP Power", string.Format("{0:F}%*Power {1} (+{2:F}% Power)", BaseStats.PvPPower / CalculationOptions.PvPPowerMultiplier, BaseStats.PvPPower, BaseStats.PvPPower / CalculationOptions.PvPPowerMultiplier));
            //dictValues.Add("Defense", BaseState.Defense.ToString());
            dictValues.Add("Crit Reduction", String.Format("{0:F}%*Spell Crit Reduction: {0:F}%\r\nPhysical Crit Reduction: {1:F}%\r\nCrit Damage Reduction: {2:F}%", BaseState.SpellCritReduction * 100, BaseState.PhysicalCritReduction * 100, BaseState.CritDamageReduction * 100));
            dictValues.Add("Dodge", String.Format("{0:F}%", 100 * BaseState.Dodge));
            dictValues.Add("Chance to Die", String.Format("{0:F}%", 100 * ChanceToDie));
            dictValues.Add("Mean Incoming Dps", String.Format("{0:F}", MeanIncomingDps));
            List<CycleId> cycleList = new List<CycleId>() { CycleId.FBIBPyro, CycleId.ScIBPyro, CycleId.FFBIBPyro, CycleId.AB4AM, CycleId.AB4ABar4AM, CycleId.AB4ABar34AM, CycleId.AB34ABar34AM, CycleId.AB24ABar34AM, CycleId.AB234ABar34AM, CycleId.AB2ABar0AM, CycleId.FFBILFrOFrB, CycleId.ArcaneManaNeutral, CycleId.AE4AB, CycleId.AERampAB };
            List<SpellId> spellList = new List<SpellId>() { SpellId.ArcaneMissiles, SpellId.ArcaneBarrage, SpellId.NetherTempest, SpellId.Scorch, SpellId.Fireball, SpellId.PyroblastPOM, SpellId.Frostbolt, SpellId.FireBlast, SpellId.InfernoBlast, SpellId.ArcaneExplosion0, SpellId.FlamestrikeSingle, SpellId.Blizzard, SpellId.BlastWave, SpellId.DragonsBreath, SpellId.ConeOfCold, SpellId.FrostfireBolt, SpellId.LivingBomb, SpellId.IceLance, SpellId.FrozenOrb, SpellId.Combustion, SpellId.PyroblastSpammed, SpellId.FrostBomb };

            foreach (CycleId cycle in cycleList)
            {
                Cycle s = BaseState.GetCycle(cycle);
                if (s != null)
                {
                    string name = s.Name;
                    name = name.Replace("Nether Tempest+", "");
                    name = name.Replace("Living Bomb+", "");
                    name = name.Replace("Frost Bomb+", "");
                    if (cycle == CycleId.ArcaneManaNeutral)
                    {
                        name = "ArcaneManaNeutral";
                    }
                    if (s.Note != null)
                    {
                        dictValues.Add(name, string.Format("{0:F} Dps*{11}\r\n{1:F} Mps\r\n{8:F} Dps per Spell Power\r\n{9:F} Dps per Mastery\r\n{10:F} Dps per Crit\r\n{3:F} Cast Procs / Sec\r\n{4:F} Hit Procs / Sec\r\n{7:F} Crit Procs / Sec\r\n{5:F} Damage Procs / Sec\r\n{6:F} Dot Procs / Sec\r\n{12:F} Nuke Procs / Sec\r\n{13:F} Nuke Procs 2 / Sec", s.DamagePerSecond, s.ManaPerSecond, 0, s.CastProcs / s.CastTime, s.HitProcs / s.CastTime, s.DamageProcs / s.CastTime, s.DotProcs / s.CastTime, s.CritProcs / s.CastTime, s.DpsPerSpellPower, s.DpsPerMastery, s.DpsPerCrit / 100, s.Note, s.NukeProcs / s.CastTime, s.NukeProcs2 / s.CastTime));
                    }
                    else
                    {
                        dictValues.Add(name, string.Format("{0:F} Dps*{1:F} Mps\r\n{8:F} Dps per Spell Power\r\n{9:F} Dps per Mastery\r\n{10:F} Dps per Crit\r\n{3:F} Cast Procs / Sec\r\n{4:F} Hit Procs / Sec\r\n{7:F} Crit Procs / Sec\r\n{5:F} Damage Procs / Sec\r\n{6:F} Dot Procs / Sec\r\n{11:F} Nuke Procs / Sec\r\n{12:F} Nuke Procs 2 / Sec", s.DamagePerSecond, s.ManaPerSecond, 0, s.CastProcs / s.CastTime, s.HitProcs / s.CastTime, s.DamageProcs / s.CastTime, s.DotProcs / s.CastTime, s.CritProcs / s.CastTime, s.DpsPerSpellPower, s.DpsPerMastery, s.DpsPerCrit / 100, s.NukeProcs / s.CastTime, s.NukeProcs2 / s.CastTime));
                    }
                }
            }
            Spell bs;
            foreach (SpellId spell in spellList)
            {
                bs = BaseState.GetSpell(spell);
                if (bs != null)
                {
                    dictValues.Add(bs.Name, GetSpellTooltip(bs));
                }
            }
            bs = BaseState.GetSpell(SpellId.ArcaneBlast0);
            dictValues.Add("Arcane Blast(0)", GetSpellTooltip(bs, true));
            bs = BaseState.GetSpell(SpellId.ArcaneBlast4);
            dictValues.Add("Arcane Blast(4)", GetSpellTooltip(bs, true));
            bs = BaseState.GetSpell(SpellId.LivingBombAOE);
            dictValues.Add("Living Bomb AOE", GetSpellTooltip(bs));
            bs = BaseState.GetSpell(SpellId.FlamestrikeSpammed);
            dictValues.Add("FlamestrikeSpam", GetSpellTooltip(bs));
            Spell abss = BaseState.GetSpell(SpellId.IncantersWard);
            dictValues.Add("Incanter's Ward", string.Format("{0:F} Absorb*{1:F} Mps\r\nAverage Cast Time: {2:F}\r\n{3:F} Mana", abss.Absorb, ((Cycle)abss).ManaPerSecond, abss.CastTime - abss.Latency, abss.ABCost));
            float totalDamage = (CalculationOptions.TargetDamage > 0.0f) ? CalculationOptions.TargetDamage : BaseCalculations.DpsRating * CalculationOptions.FightDuration;
            dictValues.Add("Total Damage", String.Format("{0:F}*Upper Bound: {1:F}\r\nLower Bound: {2:F}", totalDamage, UpperBound, LowerBound));
            dictValues.Add("Score", String.Format("{0:F}", BaseCalculations.OverallPoints));
            dictValues.Add("Dps", String.Format("{0:F}", BaseCalculations.DpsRating));
            //dictValues.Add("Tps", String.Format("{0:F}", Tps));
            dictValues.Add("Status", String.Format("Score: {0:F}, Dps: {1:F}, Survivability: {2:F}", BaseCalculations.OverallPoints, BaseCalculations.DpsRating, BaseCalculations.SurvivabilityRating));
            dictValues.Add("Sequence", computeReconstruction ? ReconstructSequence() : "...");
            StringBuilder sb = new StringBuilder("*");
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            double idleRegen = 0;
            double evocation = 0;
            double evocationIV = 0;
            double evocationHero = 0;
            double evocationIVHero = 0;
            double manaPotion = 0;
            double manaGem = 0;
            double drums = 0;
            double we = 0;
            double mi = 0;
            double cmg = 0;
            double ward = 0;
            bool segmentedOutput = CharacterCalculationsMage.DebugCooldownSegmentation;
            DamageSources = new Dictionary<string, SpellContribution>();
            ManaSources = new Dictionary<string, float>();
            ManaUsage = new Dictionary<string, float>();
            ManaSources["Initial Mana"] = StartingMana;
            ManaSources["Replenishment"] = 0.0f;
            ManaSources["Mana Gem"] = 0.0f;
            ManaSources["Mana Potion"] = 0.0f;
            ManaSources["MP5"] = 0.0f;
            ManaSources["Intellect/Spirit"] = 0.0f;
            ManaSources["Evocation"] = 0.0f;
            ManaSources["Judgement of Wisdom"] = 0.0f;
            ManaSources["Innervate"] = 0.0f;
            ManaSources["Mana Tide"] = 0.0f;
            ManaSources["Drinking"] = 0.0f;
            ManaSources["Water Elemental"] = 0.0f;
            ManaSources["Other"] = 0.0f;
            ManaUsage["Overflow"] = 0.0f;
            ManaUsage["Summon Water Elemental"] = 0.0f;
            ManaUsage["Summon Mirror Image"] = 0.0f;
            float spiritFactor = 0.003345f;
            CastingState evoBaseState = BaseState;
            /*if (CalculationOptions.Enable2T10Evocation && BaseStats.Mage2T10 > 0)
            {
                evoBaseState = BaseState.Tier10TwoPieceState;
            }*/
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01)
                {
                    switch (SolutionVariable[i].Type)
                    {
                        case VariableType.IdleRegen:
                            idleRegen += Solution[i];
                            // manaRegen = -(calculationResult.BaseState.ManaRegen * (1 - calculationOptions.Fragmentation) + calculationResult.BaseState.ManaRegen5SR * calculationOptions.Fragmentation);
                            // ManaRegen = SpiritRegen + characterStats.Mp5 / 5f + SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
                            // ManaRegen5SR = SpiritRegen * characterStats.SpellCombatManaRegeneration + characterStats.Mp5 / 5f + SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
                            if (!CalculationOptions.EffectDisableManaSources)
                            {
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * (1 - CalculationOptions.Fragmentation) + BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration * CalculationOptions.Fragmentation);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * ((15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration) * (1 - CalculationOptions.Fragmentation) + (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration) * CalculationOptions.Fragmentation);
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            }
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F} sec", "Idle Regen", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        case VariableType.Evocation:
                            evocation += Solution[i];
                            //double evoManaRegen5SR = ((0.001f + BaseStats.Spirit * spiritFactor * (float)Math.Sqrt(BaseStats.Intellect)) * BaseStats.SpellCombatManaRegeneration + BaseStats.Mp5 / 5f + calculationResult.BaseState.SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration);
                            //double evocationRegen = evoManaRegen5SR + 0.15f * BaseStats.Mana / 2f * calculationResult.BaseState.CastingSpeed;
                            //calculationResult.EvocationRegenIV = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2;
                            //calculationResult.EvocationRegenHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3;
                            //calculationResult.EvocationRegenIVHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3;
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + BaseStats.Spirit * spiritFactor * (float)Math.Sqrt(BaseStats.Intellect)) * BaseStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * BaseStats.Mana / 2f * evoBaseState.CastingSpeed;
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Evocation", Solution[i] / EvocationDuration, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        case VariableType.EvocationIV:
                            evocationIV += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + BaseStats.Spirit * spiritFactor * (float)Math.Sqrt(BaseStats.Intellect)) * BaseStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * BaseStats.Mana / 2f * evoBaseState.CastingSpeed * 1.2f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.IcyVeins))
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}", "Icy Veins+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Evocation (Icy Veins)", Solution[i] / EvocationDurationIV, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                            }
                            break;
                        case VariableType.EvocationHero:
                            evocationHero += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + BaseStats.Spirit * spiritFactor * (float)Math.Sqrt(BaseStats.Intellect)) * BaseStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * BaseStats.Mana / 2f * evoBaseState.CastingSpeed * 1.3f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.Heroism))
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}", "Heroism+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Evocation (Heroism)", Solution[i] / EvocationDurationHero, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                            }
                            break;
                        case VariableType.EvocationIVHero:
                            evocationIVHero += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + BaseStats.Spirit * spiritFactor * (float)Math.Sqrt(BaseStats.Intellect)) * BaseStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * BaseStats.Mana / 2f * evoBaseState.CastingSpeed * 1.2f * 1.3f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.IcyVeins | (int)StandardEffect.Heroism))
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}", "Icy Veins+Heroism+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Evocation (Icy Veins+Heroism)", Solution[i] / EvocationDurationIVHero, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                }
                            }
                            break;
                        case VariableType.ManaPotion:
                            manaPotion += Solution[i];
                            // (1 + characterStats.BonusManaPotion) * calculationResult.ManaPotionValue
                            ManaSources["Mana Potion"] += (float)(Solution[i] * (1 + BaseStats.BonusManaPotionEffectMultiplier) * ManaPotionValue);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Mana Potion", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        case VariableType.ManaGem:
                            manaGem += Solution[i];
                            ManaSources["Mana Gem"] += (float)(Solution[i] * (1 + BaseStats.BonusManaGem) * ManaGemValue);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Mana Gem", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        case VariableType.Drinking:
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen);
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            if (CalculationOptions.PlayerLevel < 75)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 240f;
                            }
                            else if (CalculationOptions.PlayerLevel < 80)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 306f;
                            }
                            else
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 432f;
                            }
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking", Solution[i]));
                            break;
                        case VariableType.TimeExtension:
                            break;
                        case VariableType.ManaOverflow:
                            ManaUsage["Overflow"] += (float)Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Mana Overflow", Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        case VariableType.AfterFightRegen:
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen);
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            if (CalculationOptions.PlayerLevel < 75)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 240f;
                            }
                            else if (CalculationOptions.PlayerLevel < 80)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 306f;
                            }
                            else
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 432f;
                            }
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking Regen", Solution[i]));
                            break;
                        case VariableType.SummonWaterElemental:
                            {
                                we += Solution[i];
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                                ManaUsage["Summon Water Elemental"] += (float)Solution[i] * (int)(0.16 * CalculationOptions.BaseMana) / BaseGlobalCooldown;
                                if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Summon Water Elemental", Solution[i] / BaseGlobalCooldown, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                Spell waterbolt = SolutionVariable[i].State.GetSpell(SpellId.Waterbolt);
                                SpellContribution contrib;
                                if (!DamageSources.TryGetValue(waterbolt.Name, out contrib))
                                {
                                    contrib = new SpellContribution() { Name = waterbolt.Name };
                                    DamageSources[waterbolt.Name] = contrib;
                                }
                                contrib.Hits += (float)Solution[i] / waterbolt.CastTime;
                                contrib.Damage += waterbolt.DamagePerSecond * (float)Solution[i];
                            }
                            break;
                        case VariableType.SummonMirrorImage:
                            {
                                mi += Solution[i];
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                                ManaUsage["Summon Mirror Image"] += (float)Solution[i] * (int)(0.10 * CalculationOptions.BaseMana) / BaseGlobalCooldown;
                                if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Summon Mirror Image", Solution[i] / BaseGlobalCooldown, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                                Spell mirrorImage = SolutionVariable[i].State.GetSpell(SpellId.MirrorImage);
                                SpellContribution contrib;
                                if (!DamageSources.TryGetValue("Mirror Image", out contrib))
                                {
                                    contrib = new SpellContribution() { Name = "Mirror Image" };
                                    DamageSources["Mirror Image"] = contrib;
                                }
                                contrib.Hits += 3 * (MageTalents.GlyphOfMirrorImage ? 4 : 3) * (float)Solution[i] / mirrorImage.CastTime;
                                contrib.Damage += mirrorImage.DamagePerSecond * (float)Solution[i];
                            }
                            break;
                        case VariableType.ConjureManaGem:
                            cmg += Solution[i];
                            Cycle smg = SolutionVariable[i].Cycle;
                            smg.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            smg.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F}x", "Conjure Mana Gem", Solution[i] / ConjureManaGem.CastTime, SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                        /*case VariableType.Ward:
                            ward += Solution[i];
                            Cycle sward = SolutionVariable[i].Cycle;
                            sward.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            sward.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", Ward.Name, Solution[i] / Ward.CastTime, SegmentList[SolutionVariable[i].Segment]));
                            break;*/
                        case VariableType.Spell:
                            double value;
                            Cycle s = SolutionVariable[i].Cycle;
                            s.AddDamageContribution(DamageSources, (float)Solution[i], 0);
                            s.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            s.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            string label = ((SolutionVariable[i].State.BuffLabel.Length > 0) ? (SolutionVariable[i].State.BuffLabel + "+") : "") + s.Name;
                            combinedSolution.TryGetValue(label, out value);
                            combinedSolution[label] = value + Solution[i];
                            combinedSolutionData[label] = i;
                            if (segmentedOutput) sb.AppendLine(String.Format("{2}.{3} {0}: {1:F} sec", label, Solution[i], SegmentList[SolutionVariable[i].Segment], SolutionVariable[i].ManaSegment));
                            break;
                    }
                }
            }
            if (Specialization == Specialization.Arcane)
            {
                // we need to special case damage sources because of mana adept
                DamageSources.Clear();
                CalculateArcaneDamageSources();
            }
            if (!segmentedOutput)
            {
                if (idleRegen > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", idleRegen));
                }
                if (evocation > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation", evocation / EvocationDuration, EvocationRegen));
                }
                if (evocationIV > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Icy Veins)", evocationIV / EvocationDurationIV, EvocationRegenIV));
                }
                if (evocationHero > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Heroism)", evocationHero / EvocationDurationHero, EvocationRegenHero));
                }
                if (evocationIVHero > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Icy Veins+Heroism)", evocationIVHero / EvocationDurationIVHero, EvocationRegenIVHero));
                }
                if (manaPotion > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", manaPotion));
                }
                if (manaGem > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", manaGem));
                }
                if (drums > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", drums / BaseGlobalCooldown));
                }
                if (we > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Summon Water Elemental", we / BaseGlobalCooldown));
                }
                if (mi > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Summon Mirror Image", mi / BaseGlobalCooldown));
                }
                if (cmg > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Conjure Mana Gem", cmg / ConjureManaGem.CastTime));
                }
                if (ward > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", Ward.Name, ward / Ward.CastTime));
                }
                foreach (KeyValuePair<string, double> kvp in combinedSolution)
                {
                    Cycle s = SolutionVariable[combinedSolutionData[kvp.Key]].Cycle;
                    if (s != null)
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps)", kvp.Key, kvp.Value, s.DamagePerSecond, s.ManaPerSecond));
                    }
                    else
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                    }
                }
            }
            //if (WaterElemental) sb.AppendLine(String.Format("Water Elemental: {0:F}x", WaterElementalDuration / 45f));
            dictValues.Add("Spell Cycles", sb.ToString());
            sb = new StringBuilder("*");
            List<SpellContribution> contribList = new List<SpellContribution>(DamageSources.Values);
            contribList.Sort();
            foreach (SpellContribution contrib in contribList)
            {
                if (contrib.Name == "Mirror Image")
                {
                    contrib.HitDamage = 0;
                    contrib.CritDamage = 0;
                    contrib.Hits = 0;
                }
                if (contrib.HitDamage > 0)
                {
                    sb.AppendFormat("{0}: {1:F}%, {2:F} Damage\r\n",
                        contrib.Name,
                        100.0 * contrib.Damage / totalDamage,
                        contrib.Damage);
                    sb.AppendFormat("\tHits (#: {0:F}, Avg: {1:F}, Total: {2:F})\r\n",
                        contrib.Hits,
                        contrib.HitDamage / contrib.Hits,
                        contrib.HitDamage);
                }
                else if (contrib.Hits > 0)
                {
                    sb.AppendFormat("{0}: {1:F}%, {2:F} Damage, {3:F} Hits\r\n",
                        contrib.Name,
                        100.0 * contrib.Damage / totalDamage,
                        contrib.Damage,
                        contrib.Hits);
                }
                else
                {
                    sb.AppendFormat("{0}: {1:F}%, {2:F} Damage\r\n",
                        contrib.Name,
                        100.0 * contrib.Damage / totalDamage,
                        contrib.Damage);
                }
                if (contrib.CritDamage > 0)
                {
                    sb.AppendFormat("\tCrits (#: {0:F}, Avg: {1:F}, Total: {2:F})\r\n",
                        contrib.Crits,
                        contrib.CritDamage / contrib.Crits,
                        contrib.CritDamage);
                }
                if (contrib.TickDamage > 0)
                {
                    sb.AppendFormat("\tTicks (#: {0:F}, Avg: {1:F}, Total: {2:F})\r\n",
                        contrib.Ticks,
                        contrib.TickDamage / contrib.Ticks,
                        contrib.TickDamage);
                }
            }
            dictValues.Add("By Spell", sb.ToString());
            //dictValues.Add("Minimum Range", String.Format("{0:F}", MinimumRange));
            //dictValues.Add("Threat Reduction", String.Format("{0:F}%", ThreatReduction * 100));
            CalculationOptions.Calculations = this;
            return dictValues;
        }

        private void CalculateArcaneDamageSources()
        {
            // for now requires solution variables to work
            int[] sort = new int[SolutionVariable.Count];
            for (int j = 0; j < SolutionVariable.Count; j++)
            {
                sort[j] = j;
            }
            Array.Sort(sort, (x, y) =>
            {
                SolutionVariable vx = SolutionVariable[x];
                SolutionVariable vy = SolutionVariable[y];
                int comp = vx.Segment.CompareTo(vy.Segment);
                if (comp != 0) return comp;
                comp = vx.ManaSegment.CompareTo(vy.ManaSegment);
                if (comp != 0) return comp;
                // first instant mana gain, then negative mps
                // then mana overflow, then positive mps
                comp = Solver.GetQuadraticIndex(vx).CompareTo(Solver.GetQuadraticIndex(vy));
                if (comp != 0) return comp;
                return vx.Mps.CompareTo(vy.Mps);
            });

            float mana = StartingMana;
            int gemCount = 0;
            float time = 0;
            float maxMana = BaseStats.Mana;

            for (int j = 0; j < SolutionVariable.Count; j++)
            {
                int i = sort[j];
                if (Solution[i] > 0.01)
                {
                    double mps = SolutionVariable[i].Mps;
                    Cycle cycle = SolutionVariable[i].Cycle;
                    var variableType = SolutionVariable[i].Type;

                    if (variableType == VariableType.ManaGem)
                    {
                        mps = 0.0;
                    }
                    else if (variableType == VariableType.ManaPotion)
                    {
                        mps = 0.0;
                    }
                    else if (variableType == VariableType.Drinking)
                    {
                        mps = -BaseState.ManaRegenDrinking;
                    }
                    else if (variableType == VariableType.AfterFightRegen)
                    {
                        mps = -BaseState.ManaRegenDrinking;
                    }

                    float duration = (float)Solution[i];
                    CastingState state = SolutionVariable[i].State;
                    if (variableType == VariableType.ManaPotion || variableType == VariableType.ManaGem)
                    {
                        float value = duration;
                        duration = 0;
                        if (variableType == VariableType.ManaGem)
                        {
                            mana += (float)((1 + BaseStats.BonusManaGem) * ManaGemValue * value);
                            gemCount++;
                        }
                        else if (variableType == VariableType.ManaPotion)
                        {
                            mana += (float)((1 + BaseStats.BonusManaPotionEffectMultiplier) * ManaPotionValue * value);
                        }
                        if (mana < 0) mana = 0;
                        if (mana > maxMana)
                        {
                            mana = maxMana;
                        }
                    }
                    else
                    {
                        float partTime = duration;
                        if (mana - mps * duration < 0) partTime = (float)(mana / mps);
                        else if (mana - mps * duration > maxMana) partTime = (float)((mana - maxMana) / mps);

                        float startMana = mana;
                        mana -= (float)(mps * duration);
                        if (mana < 0) mana = 0;
                        if (mana > maxMana)
                        {
                            mana = maxMana;
                        }

                        float averageMana = (partTime * (startMana + mana) / 2.0f + (duration - partTime) * mana) / duration;

                        switch (variableType)
                        {
                            case VariableType.SummonWaterElemental:
                                {
                                    Spell waterbolt = SolutionVariable[i].State.GetSpell(SpellId.Waterbolt);
                                    SpellContribution contrib;
                                    if (!DamageSources.TryGetValue(waterbolt.Name, out contrib))
                                    {
                                        contrib = new SpellContribution() { Name = waterbolt.Name };
                                        DamageSources[waterbolt.Name] = contrib;
                                    }
                                    contrib.Hits += (float)Solution[i] / waterbolt.CastTime;
                                    contrib.Damage += waterbolt.DamagePerSecond * (float)Solution[i];
                                }
                                break;
                            case VariableType.SummonMirrorImage:
                                {
                                    Spell mirrorImage = SolutionVariable[i].State.GetSpell(SpellId.MirrorImage);
                                    SpellContribution contrib;
                                    if (!DamageSources.TryGetValue("Mirror Image", out contrib))
                                    {
                                        contrib = new SpellContribution() { Name = "Mirror Image" };
                                        DamageSources["Mirror Image"] = contrib;
                                    }
                                    contrib.Hits += 3 * (MageTalents.GlyphOfMirrorImage ? 4 : 3) * (float)Solution[i] / mirrorImage.CastTime;
                                    contrib.Damage += mirrorImage.DamagePerSecond * (float)Solution[i];
                                }
                                break;
                            case VariableType.Spell:
                                cycle.AddDamageContribution(DamageSources, (float)Solution[i], averageMana);
                                break;
                        }
                    }
                    time += duration;
                }
            }
        }

        private string GetSpellTooltip(Spell bs, bool abcost = false)
        {
            if (bs.CastTime == 0)
            {
                string spellFormatString = "{0:F} Damage*{13:F} Mana\r\n{8:F} - {9:F} Hit\r\n{10:F} - {11:F} Crit\r\n{12}{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier";
                return string.Format(spellFormatString, bs.AverageDamage, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage * (1 + ManaAdeptBonus), bs.MaxHitDamage * (1 + ManaAdeptBonus), bs.MinCritDamage * (1 + ManaAdeptBonus), bs.MaxCritDamage * (1 + ManaAdeptBonus), ((bs.DotTickHitDamage > 0) ? ((bs.DotTickHitDamage * (1 + ManaAdeptBonus)).ToString("F") + " Dot Tick Hit\r\n" + (bs.DotTickCritDamage * (1 + ManaAdeptBonus)).ToString("F") + " Dot Tick Crit\r\n") : ""), abcost ? bs.ABCost : bs.Cost);
            }
            else
            {
                string spellFormatString = "{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\n{3:F} sec\r\n{13:F} Mana\r\n{8:F} - {9:F} Hit\r\n{10:F} - {11:F} Crit\r\n{12}{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier";
                return string.Format(spellFormatString, ((Cycle)bs).DamagePerSecond, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage * (1 + ManaAdeptBonus), bs.MaxHitDamage * (1 + ManaAdeptBonus), bs.MinCritDamage * (1 + ManaAdeptBonus), bs.MaxCritDamage * (1 + ManaAdeptBonus), ((bs.DotTickHitDamage > 0) ? ((bs.DotTickHitDamage * (1 + ManaAdeptBonus)).ToString("F") + " Dot Tick Hit\r\n" + (bs.DotTickCritDamage * (1 + ManaAdeptBonus)).ToString("F") + " Dot Tick Crit\r\n") : ""), abcost ? bs.ABCost : bs.Cost);
            }
        }
    }

    public class OptimizableCalculations
    {
        public float ChanceToDie { get; set; }
        public float Health { get; set; }
        public float NatureResistance { get; set; }
        public float FireResistance { get; set; }
        public float FrostResistance { get; set; }
        public float ShadowResistance { get; set; }
        public float ArcaneResistance { get; set; }
        public float PvPResilience { get; set; }
        public float HasteRating { get; set; }
        public float CritRating { get; set; }
        public float PVPTrinket { get; set; }
        public float MovementSpeed { get; set; }
    }

    public sealed class CharacterCalculationsMage : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[2];
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsRating
        {
            get
            {
                return _subPoints[0];
            }
        }

        public float SurvivabilityRating
        {
            get
            {
                return _subPoints[1];
            }
        }

        public DisplayCalculations DisplayCalculations { get; set; }
        public OptimizableCalculations OptimizableCalculations { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            if (RequiresAsynchronousDisplayCalculation)
            {
                Dictionary<string, string> ret = DisplayCalculations.GetCharacterDisplayCalculationValues(false);
                ret["Dps"] = "...";
                ret["Total Damage"] = "...";
                ret["Score"] = "...";
                ret["Tps"] = "...";
                ret["Spell Cycles"] = "...";
                ret["By Spell"] = "...";
                ret["Status"] = "Score: ..., Dps: ..., Survivability: ...";
                DisplayCalculations.DisplaySolver = new Solver(DisplayCalculations.Character, DisplayCalculations.CalculationOptions, DisplayCalculations.CalculationOptions.DisplaySegmentCooldowns, DisplayCalculations.CalculationOptions.DisplaySegmentMana, DisplayCalculations.CalculationOptions.DisplayIntegralMana, DisplayCalculations.CalculationOptions.DisplayAdvancedConstraintsLevel, false, DisplayCalculations.CalculationOptions.SmartOptimization, true, true, true, DisplayCalculations.CalculationOptions.CombinatorialSolver || DisplayCalculations.CalculationOptions.GeneticSolver, false);
                CalculationsMage.EnableSolver(DisplayCalculations.DisplaySolver);
                DisplayCalculations.CalculationOptions.SequenceReconstruction = null;
                return ret;
            }
            else
            {
                return DisplayCalculations.GetCharacterDisplayCalculationValues(true);
            }
        }

        public override void CancelAsynchronousCharacterDisplayCalculation()
        {
            DisplayCalculations.DisplaySolver.CancelAsync();
        }

        public override bool RequiresAsynchronousDisplayCalculation
        {
            get
            {
                return DisplayCalculations.CalculationOptions.DisplaySegmentCooldowns != DisplayCalculations.CalculationOptions.ComparisonSegmentCooldowns || DisplayCalculations.CalculationOptions.DisplaySegmentMana != DisplayCalculations.CalculationOptions.ComparisonSegmentMana || DisplayCalculations.CalculationOptions.DisplayIntegralMana != DisplayCalculations.CalculationOptions.ComparisonIntegralMana || DisplayCalculations.CalculationOptions.DisplayAdvancedConstraintsLevel != DisplayCalculations.CalculationOptions.ComparisonAdvancedConstraintsLevel || DisplayCalculations.CalculationOptions.CombinatorialSolver || DisplayCalculations.CalculationOptions.GeneticSolver;
            }
        }

        public override Dictionary<string, string> GetAsynchronousCharacterDisplayCalculationValues()
        {
            CharacterCalculationsMage smp = DisplayCalculations.DisplaySolver.GetCharacterCalculations(null);
            smp.DisplayCalculations.DisplaySolver = DisplayCalculations.DisplaySolver;
            Dictionary<string, string> ret = smp.DisplayCalculations.GetCharacterDisplayCalculationValues(true);
            CalculationsMage.DisableSolver(DisplayCalculations.DisplaySolver);
            ret["Dps"] = String.Format("{0:F}*{1:F}% Error margin", smp.DpsRating, Math.Abs(DpsRating - smp.DpsRating) / DpsRating * 100);
            return ret;
        }

        public static bool DebugCooldownSegmentation { get; set; }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return OptimizableCalculations.Health;
                case "Nature Resistance": return OptimizableCalculations.NatureResistance;
                case "Fire Resistance": return OptimizableCalculations.FireResistance;
                case "Frost Resistance": return OptimizableCalculations.FrostResistance;
                case "Shadow Resistance": return OptimizableCalculations.ShadowResistance;
                case "Arcane Resistance": return OptimizableCalculations.ArcaneResistance;
                case "Resilience": return OptimizableCalculations.PvPResilience;
                case "Chance to Live": return 100 * (1 - OptimizableCalculations.ChanceToDie);
                case "Haste Rating": return OptimizableCalculations.HasteRating;
                case "Crit Rating": return OptimizableCalculations.CritRating;
                case "PVP Trinket": return OptimizableCalculations.PVPTrinket;
                case "Movement Speed": return OptimizableCalculations.MovementSpeed * 100f;
            }
            return 0;
        }
    }
}
