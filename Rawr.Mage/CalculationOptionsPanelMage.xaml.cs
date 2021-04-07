using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls.DataVisualization.Charting;
using System.Threading;

namespace Rawr.Mage
{
    public partial class CalculationOptionsPanelMage : UserControl, ICalculationOptionsPanel
    {
        private int advancedSolverIndex;

        public CalculationOptionsPanelMage()
        {
            InitializeComponent();

            CalculationsMage.AdvancedSolverChanged += new EventHandler(CalculationsMage_AdvancedSolverChanged);
            CalculationsMage.AdvancedSolverLogUpdated += new EventHandler(CalculationsMage_AdvancedSolverLogUpdated);           
            advancedSolverIndex = OptionsTab.Items.CastList<TabItem>().FindIndex(tab => (string)tab.Header == "Solver Log");
        }

        private void OptionsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OptionsTab != null && OptionsTab.SelectedIndex == advancedSolverIndex)
            {
                AdvancedSolverLog.Text = CalculationsMage.AdvancedSolverLog;
            }
        }

        void CalculationsMage_AdvancedSolverLogUpdated(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action<object, EventArgs>)CalculationsMage_AdvancedSolverLogUpdated, sender, e);
            }
            else
            {
                if (OptionsTab.SelectedIndex == advancedSolverIndex)
                {
                    AdvancedSolverLog.Text = CalculationsMage.AdvancedSolverLog;
#if !SILVERLIGHT
                    AdvancedSolverLog.ScrollToEnd();
#endif
                }
            }
        }

        void CalculationsMage_AdvancedSolverChanged(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action<object, EventArgs>)CalculationsMage_AdvancedSolverChanged, sender, e);
            }
            else
            {
                AdvancedSolverCancel.IsEnabled = !CalculationsMage.IsSolverEnabled(null);
            }
        }

        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get
            {
                return character;
            }
            set
            {
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsMage)
                {
                    ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);
                }

                character = value;
                if (character.CalculationOptions == null) { character.CalculationOptions = new CalculationOptionsMage(character); }
                LayoutRoot.DataContext = Character.CalculationOptions;

                ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);

            }
        }

        void CalculationOptionsPanelMage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        private void AdvancedSolverCancel_Click(object sender, RoutedEventArgs e)
        {
            CalculationsMage.CancelAsync();
        }

        private void ScaleRaidBuffsToLevel_Click(object sender, RoutedEventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            Buff.LoadDefaultBuffs(null, calculationOptions.PlayerLevel);
            Character.OnCalculationsInvalidated();
        }

        private void CooldownEditor_Click(object sender, RoutedEventArgs e)
        {
            var cooldownRestrictions = new CooldownRestrictionsDialog();
            cooldownRestrictions.Character = Character;
#if SILVERLIGHT
            cooldownRestrictions.Show();
#else
            ((Window)cooldownRestrictions).Show();
#endif
        }

        private void HotStreakUtilization_Click(object sender, RoutedEventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Solver solver = new Solver(Character, calculationOptions, false, false, false, 0, armor, false, false, true, false, true, false, false);
            solver.Initialize(null);
            CastingState baseState = new CastingState(solver, 0, false, 0);

            FireCycleGenerator generator = new FireCycleGenerator(baseState);

            GenericCycle c1 = new GenericCycle("test", baseState, generator.StateList, true);
            Cycle c2 = baseState.GetCycle(CycleId.FBLBPyro);

            Dictionary<string, SpellContribution> dict1 = new Dictionary<string, SpellContribution>();
            Dictionary<string, SpellContribution> dict2 = new Dictionary<string, SpellContribution>();
            c1.AddDamageContribution(dict1, 1.0f, 0);
            c2.AddDamageContribution(dict2, 1.0f, 0);

            float predicted = dict2["Pyroblast"].Hits / dict2["Fireball"].Hits;
            float actual = dict1["Pyroblast"].Hits / dict1["Fireball"].Hits;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Pyro/Nuke Ratio:");
            sb.AppendLine();
            sb.AppendLine("Approximation Model: " + predicted);
            sb.AppendLine("Exact Model: " + actual);
            sb.AppendLine();
            // predicted = raw * (1 - wastedold)
            // actual = raw * (1 - wasted)
            // wasted = 1 - actual / predicted * (1 - wastedold)
            sb.AppendLine("Predicted Wasted Hot Streaks: " + (1 - actual / predicted));

            MessageBox.Show(sb.ToString());
        }

        private void CalculationTiming_Click(object sender, RoutedEventArgs e)
        {
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Character character = Character;
#if SILVERLIGHT
            DateTime start = DateTime.Now;
#else
            System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
            clock.Start();
#endif
            for (int i = 0; i < 500; i++)
            {
                calculations.GetCharacterCalculations(character);
            }
#if SILVERLIGHT
            MessageBox.Show("Calculating 10000 characters takes " + DateTime.Now.Subtract(start).TotalSeconds + " seconds.");
#else
            clock.Stop();
            MessageBox.Show("Calculating 10000 characters takes " + clock.Elapsed.TotalSeconds + " seconds.");
#endif
        }

        private void CycleAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            CycleAnalyzerDialog analyzer = new CycleAnalyzerDialog(Character);
#if SILVERLIGHT
            analyzer.Show();
#else
            ((Window)analyzer).Show();
#endif
        }

        private void CustomSpellMix_Click(object sender, RoutedEventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.CustomSpellMix == null) calculationOptions.CustomSpellMix = new List<SpellWeight>();
            CustomSpellMixDialog dialog = new CustomSpellMixDialog(calculationOptions.CustomSpellMix);
            dialog.Show();
        }

        #region Silverlight workaround
        public class ActualSizePropertyProxy : FrameworkElement, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public FrameworkElement Element
            {
                get { return (FrameworkElement)GetValue(ElementProperty); }
                set { SetValue(ElementProperty, value); }
            }

            public double ActualHeightValue
            {
                get { return Element == null ? 0 : Element.ActualHeight; }
            }

            public double ActualWidthValue
            {
                get { return Element == null ? 0 : Element.ActualWidth; }
            }

            public static readonly DependencyProperty ElementProperty =
                DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(ActualSizePropertyProxy),
                                            new PropertyMetadata(null, OnElementPropertyChanged));

            private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                ((ActualSizePropertyProxy)d).OnElementChanged(e);
            }

            private void OnElementChanged(DependencyPropertyChangedEventArgs e)
            {
                FrameworkElement oldElement = (FrameworkElement)e.OldValue;
                FrameworkElement newElement = (FrameworkElement)e.NewValue;

                newElement.SizeChanged += new SizeChangedEventHandler(Element_SizeChanged);
                if (oldElement != null)
                {
                    oldElement.SizeChanged -= new SizeChangedEventHandler(Element_SizeChanged);
                }
                NotifyPropChange();
            }

            private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
            {
                NotifyPropChange();
            }

            private void NotifyPropChange()
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualWidthValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualHeightValue"));
                }
            }
        } 

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Silverlight workaround
            // LayoutRoot
            // MaxHeight="{Binding Path=ActualHeight, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type ScrollViewer}}}"
            DependencyObject obj = this;
            while (obj != null)
            {
                if (obj is ScrollViewer)
                {
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            if (obj != null)
            {
                ActualSizePropertyProxy proxy = new ActualSizePropertyProxy();
                proxy.Element = (FrameworkElement)obj;
                LayoutRoot.SetBinding(Grid.MaxHeightProperty, new Binding()
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath("ActualHeightValue"),
                    Source = proxy,
                });
            }
        }
        #endregion

        private void CopyCharacterStats_Click(object sender, RoutedEventArgs e)
        {
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            string stats = calculations.GetCharacterStatsString(character);
            Clipboard.SetText(stats);
        }

        public struct PlotData
        {
            public double Damage { get; set; }
            public double Probability { get; set; }
        }

        private void CombustionModel_Click(object sender, RoutedEventArgs e)
        {
            CombustionModelWindow window = new CombustionModelWindow();

            CombustionModel model = new CombustionModel(Character, window);
            model.Simulate();

            float max = model.combustionDamageSamples.Keys.Max();
            int binCount = 1000;

            int[] count = new int[binCount];
            int total = 0;

            foreach (var kvp in model.combustionDamageSamples)
            {
                int index = (int)(kvp.Key / max * binCount);
                if (index >= binCount)
                {
                    index = binCount - 1;
                }
                count[index] += kvp.Value;
                total += kvp.Value;
            }

            double maxp = 0;
            PlotData[] plot = new PlotData[binCount];
            int current = 0;
            for (int tick = 0; tick < binCount; tick++)
            {
                if (count[tick] > maxp)
                {
                    maxp = count[tick];
                }
                current += count[tick];
                plot[tick] = new PlotData() { Damage = (tick + 1) * max / binCount, Probability = current / (double)total };
            }

            AreaSeries series;
            window.Chart.Series.Add(series = new AreaSeries()
            {
                Title = "cdf",
                ItemsSource = plot,
                IndependentValuePath = "Damage",
                DependentValuePath = "Probability",
                DependentRangeAxis = new LinearAxis()
                {
                    Minimum = 0,
                    Maximum = 1,
                    Orientation = AxisOrientation.Y,
                    ShowGridLines = true,
                    Location = AxisLocation.Left,
                }
            });

            ThreadPool.QueueUserWorkItem((WaitCallback) ((object state) => {
                var policy = model.OptimizeCombustionPolicy();
                window.Status.Dispatcher.BeginInvoke((Action)(() =>
                {
                    PlotData[] policyplot = new PlotData[CombustionModel.PolicySize];
                    for (int i = 0; i < CombustionModel.PolicySize; i++)
                    {
                        policyplot[i] = new PlotData() { Damage = policy.Policy[i] * max / binCount, Probability = i };
                    }

                    window.Chart.Series.Add(new LineSeries()
                    {
                        Title = "policy",
                        ItemsSource = policyplot,
                        IndependentValuePath = "Damage",
                        DependentValuePath = "Probability",
                        DependentRangeAxis = new LinearAxis()
                        {
                            Minimum = 0,
                            Maximum = 60,
                            Orientation = AxisOrientation.Y,
                            ShowGridLines = false,
                            Location = AxisLocation.Right,
                        }
                    });                    
                }));
            }));

            window.Show();
        }

        private class CombustionModel
        {
            private Solver solver;
            private CastingState baseState;
            private CombustionModelWindow window;

            Spell FB;
            Spell Pyro;
            Spell comb;

            public CombustionModel(Character character, CombustionModelWindow window)
            {
                string armor = "Molten Armor";
                CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
                CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
                solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, true, false, true, false, false);
                solver.Initialize(null);
                baseState = new CastingState(solver, 0, false, solver.Mage2T13 ? 500 : 0);

                FB = baseState.GetSpell(SpellId.Fireball);
                Pyro = baseState.GetSpell(SpellId.PyroblastPOMDotUptime);
                comb = new Spell(solver.CombustionTemplate);
                this.window = window;
            }

            // state
            double time = 0;
            double pyroDotTime = 0;
            float igniteFBRoll = 0; // 1 = one crit ignite over 2 ticks
            float ignitePyroRoll = 0;
            double igniteTime = 0;
            int hotStreakCounter = 0;
            bool hsProc = false;
            bool hsProcVisible = false;
            //double igniteDelay = 0.5;

            float markFrequency = 1;

            public Dictionary<float, int> combustionDamageSamples = new Dictionary<float, int>();
            Random rnd = new Random();

            public double MaxCombustion;
            public const int BinCount = 1000;
            public const int PolicySize = 60;
            public double[] pdf = new double[BinCount];
            public double[] cdf = new double[BinCount];

            public void Simulate()
            {
                float FBc = FB.CritRate;
                float FBHSc = FB.CritRate - FB.NonHSCritRate;
                float FBk = Math.Max(-2.73f * FBHSc + 0.95f, 0f);
                if (baseState.Solver.Mage4T12)
                {
                    FBk = Math.Min(0.3f + FBk, 1f);
                }

                for (int iter = 0; iter < 1000000; iter++)
                {
                    if (hsProcVisible)
                    {
                        // cast Pyro
                        if (rnd.NextDouble() < Pyro.CritRate)
                        {
                            // crit
                            // update ignite
                            if (igniteTime > 0)
                            {
                                int remainingTicks = (int)Math.Ceiling(igniteTime / 2);
                                // refresh
                                igniteTime -= Math.Floor(igniteTime / 2) * 2;
                                igniteTime += 4;
                                ignitePyroRoll = 2f / 3f * (remainingTicks * ignitePyroRoll / 2f + 1);
                                igniteFBRoll = 2f / 3f * (remainingTicks * igniteFBRoll / 2f);
                            }
                            else
                            {
                                igniteTime = 4;
                                ignitePyroRoll = 1;
                            }
                        }
                        // state
                        hsProc = false;
                        hsProcVisible = false;
                        // pyro dot
                        if (pyroDotTime > 0)
                        {
                            // refresh
                            float tickInterval = Pyro.DotTickInterval / Pyro.AverageCastingSpeed;
                            pyroDotTime -= Math.Floor(pyroDotTime / tickInterval) * tickInterval;
                            pyroDotTime += Pyro.DotFullDuration;
                        }
                        else
                        {
                            pyroDotTime = Pyro.DotFullDuration;
                        }

                        MoveTime(Pyro.CastTime);
                    }
                    else
                    {
                        // cast FB

                        MoveTime(FB.CastTime);

                        if (rnd.NextDouble() < FB.CritRate)
                        {
                            // crit
                            hotStreakCounter++;
                            if (hotStreakCounter == 2)
                            {
                                hotStreakCounter = 0;
                                hsProc = true;
                            }
                            else if (rnd.NextDouble() < FBk)
                            {
                                hotStreakCounter = 0;
                                hsProc = true;
                            }
                            // update ignite
                            if (igniteTime > 0)
                            {
                                int remainingTicks = (int)Math.Ceiling(igniteTime / 2);
                                // refresh
                                igniteTime -= Math.Floor(igniteTime / 2) * 2;
                                igniteTime += 4;
                                ignitePyroRoll = 2f / 3f * (remainingTicks * ignitePyroRoll / 2f);
                                igniteFBRoll = 2f / 3f * (remainingTicks * igniteFBRoll / 2f + 1);
                            }
                            else
                            {
                                igniteTime = 4;
                                igniteFBRoll = 1;
                            }
                        }
                        else
                        {
                            hotStreakCounter = 0;
                        }
                    }
                }

                MaxCombustion = combustionDamageSamples.Keys.Max();

                int[] count = new int[BinCount];
                int total = 0;

                foreach (var kvp in combustionDamageSamples)
                {
                    int index = (int)(kvp.Key / MaxCombustion *BinCount);
                    if (index >= BinCount)
                    {
                        index = BinCount - 1;
                    }
                    count[index] += kvp.Value;
                    total += kvp.Value;
                }

                int sum = 0;
                for (int i = 0; i < BinCount; i++)
                {
                    sum += count[i];
                    pdf[i] = count[i] / (double)total;
                    cdf[i] = sum / (double)total;
                }
            }

            private void MoveTime(double timeToMove)
            {
                double nextTime = time + timeToMove;
                double mark = Math.Floor(time / markFrequency) * markFrequency;
                do
                {
                    mark += markFrequency;
                    if (mark < nextTime)
                    {
                        // record combustion state
                        solver.CombustionTemplate.Initialize(solver, pyroDotTime - (mark - time) > 0, igniteTime - (mark - time) > 0 ? ignitePyroRoll : 0, igniteTime - (mark - time) > 0 ? igniteFBRoll : 0);
                        comb.Initialize(solver.CombustionTemplate);
                        comb.Calculate(baseState);
                        comb.CalculateDerivedStats(baseState, false, false, false);
                        float damage = comb.AverageDamage;
                        int count;
                        combustionDamageSamples.TryGetValue(damage, out count);
                        combustionDamageSamples[damage] = count + 1;
                    }
                } while (mark < nextTime);

                // update state                
                time = nextTime;
                pyroDotTime -= timeToMove;
                igniteTime -= timeToMove;
                if (pyroDotTime <= 0)
                {
                    pyroDotTime = 0;
                }
                if (igniteTime <= 0)
                {
                    igniteTime = 0;
                    ignitePyroRoll = 0;
                    igniteFBRoll = 0;
                }
                if (hsProc)
                {
                    hsProcVisible = true;
                }
            }

            public class CombustionPolicy
            {
                public int[] Policy {get;set;}
                public double AverageCombustionDPS { get; set; }
            }

            private class CombustionOptimizer : Optimizer.OptimizerBase<int, CombustionPolicy, double>
            {
                private CombustionModel model;

                public CombustionOptimizer(CombustionModel model)
                {
                    this.model = model;
                    slotCount = PolicySize;
                    _thoroughness = 200;
                    validators = new List<Optimizer.OptimizerRangeValidatorBase<int>>();
                    List<int> items = new List<int>();
                    for (int i = 0; i < BinCount; i++)
                    {
                        items.Add(i);
                    }
                    slotItems = new List<int>[slotCount];
                    for (int slot = 0; slot < slotCount; slot++)
                    {
                        slotItems[slot] = items;
                    }
                }

                protected override CombustionPolicy BuildMutantIndividual(CombustionPolicy parent, CombustionPolicy recycledIndividual)
                {
                    //return base.BuildMutantIndividual(parent, recycledIndividual);
                    Random rand = Rnd;
                    int targetMutations = 2;
                    while (targetMutations < 32 && rand.NextDouble() < 0.75d) targetMutations++;
                    double mutationChance = (double)targetMutations / 32d;

                    return GeneratorBuildIndividual(
                        delegate(int slot, int[] items)
                        {
                            if (Rnd.NextDouble() < mutationChance)
                            {
                                int v = parent.Policy[slot];
                                return rand.Next(Math.Max(v - 100, 0), Math.Min(BinCount, v + 100));
                            }
                            else
                            {
                                return GetItem(parent, slot);
                            }
                        },
                        recycledIndividual);
                }

                protected override void ReportProgress(int progressPercentage, float bestValue)
                {
                    //CalculationsMage.ClearLog(model.solver, progressPercentage + ": " + bestValue);
                    model.window.Status.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        model.window.Status.Text = progressPercentage + ": " + bestValue;
                    }));
                }

                protected override double GetValuation(CombustionPolicy individual)
                {
                    // convert policy to probabilities and expected combustion size

                    double[] P = new double[PolicySize + 1];
                    double[] AvgC = new double[PolicySize + 1];
                    double prior = 1;

                    for (int i = 0; i < PolicySize; i++)
                    {
                        //Pi = integrate_f(i)..inf pdfI(t) dt

                        //sum_t=0..i P0 * 0 +
                        //             P1 * (1 - P0) * 1 +
                        //             P2 * (1 - P0) * (1 - P1) * 2 +
                        //             P3 * (1 - P0) * (1 - P1) * (1 - P2) * 3 +
                        //             ...

                        // Pi = 1 - model.cdf[individual.Policy[i]]
                        P[i] = prior * (1 - model.cdf[individual.Policy[i]]);
                        prior *= model.cdf[individual.Policy[i]];
                        double sum = 0;
                        for (int t = individual.Policy[i] + 1; t < BinCount; t++)
                        {
                            sum += (t + 1) / (double)BinCount * model.MaxCombustion * model.pdf[t];
                        }
                        AvgC[i] = sum / (1 - model.cdf[individual.Policy[i]]);
                    }
                    {
                        P[PolicySize] = prior;
                        double sum = 0;
                        for (int t = 0; t < BinCount; t++)
                        {
                            sum += (t + 1) / (double)BinCount * model.MaxCombustion * model.pdf[t];
                        }
                        AvgC[PolicySize] = sum;
                    }

                    double avgTime = 0;
                    double avgCombustion = 0;
                    for (int i = 0; i <= PolicySize; i++)
                    {
                        avgTime += P[i] * i;
                        avgCombustion += P[i] * AvgC[i];
                    }

                    double dps = avgCombustion / (model.solver.CombustionCooldown + avgTime);
                    individual.AverageCombustionDPS = dps;

                    return dps;
                }

                protected override float GetOptimizationValue(CombustionPolicy individual, double valuation)
                {
                    return (float)valuation;
                }

                protected override int GetItem(CombustionPolicy individual, int slot)
                {
                    return individual.Policy[slot];
                }

                protected override int[] GetItems(CombustionPolicy individual)
                {
                    return individual.Policy;
                }

                protected override CombustionPolicy GenerateIndividual(int[] items, bool canUseArray, CombustionPolicy recycledIndividual)
                {
                    int[] i = items;
                    if (!canUseArray)
                    {
                        i = (int[])items.Clone();
                    }
                    //if (recycledIndividual == null)
                    {
                        recycledIndividual = new CombustionPolicy();
                    }
                    recycledIndividual.Policy = i;
                    return recycledIndividual;
                }

                public CombustionPolicy Optimize()
                {
                    float bestValue;
                    bool inj;
                    double ret;
                    return base.Optimize(null, 0, out bestValue, out ret, out inj);
                }
            }

            public CombustionPolicy OptimizeCombustionPolicy()
            {
                //Ignite Size Distribution = I, assume time independent
                //Combustion Policy = f(t), given time since combustion got off cooldown what is the minimum ignite size where you would combust

                //expected long term combustion dps=

                //expected time till combustion is used=

                //Pi := P(I >= f(i))

                //Ti := 
                //sum_t=0..i P0 * 0 +
                //             P1 * (1 - P0) * 1 +
                //             P2 * (1 - P0) * (1 - P1) * 2 +
                //             P3 * (1 - P0) * (1 - P1) * (1 - P2) * 3 +
                //             ...

                //= (1 - P0) * [1*P1 + (1 - P1) * [2*P2 + (1 - P2) * [3*P3 + (1 - P3) * [...]]]]

                //T0 = 0
                //Ti = Ti-1 + (1 - P0) * (1 - P1) * ... * (1 - Pi-1) * i * Pi


                //Combust size if used at i = 
                //C + integrate_f(i)..inf pdfI(t) * t dt


                //Pi = integrate_f(i)..inf pdfI(t) dt

                CombustionOptimizer optimizer = new CombustionOptimizer(this);               
                return optimizer.Optimize();
            }
        }
    }
}
