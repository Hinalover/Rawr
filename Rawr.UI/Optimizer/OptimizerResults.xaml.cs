using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rawr.Optimizer;

#if SILVERLIGHT
using System.Windows.Printing;
#endif

namespace Rawr.UI
{
   public partial class OptimizerResults : ChildWindow
   {
      public bool WeWantToStoreIt;

      public CharacterCalculationsBase currentCalc;
      public CharacterCalculationsBase optimizedCalc;

      public OptimizerResults(Character oldCharacter, Character newCharacter, bool cancelOptimization)
      {
         InitializeComponent();

#if !SILVERLIGHT
         WindowStartupLocation = WindowStartupLocation.CenterOwner;
         WindowState = WindowState.Normal;
#endif

         if (cancelOptimization)
         {
            CancelButton.Content = "Cancel";
            BT_StoreIt.Content = "Continue";
         }

         CurrentCharacter = oldCharacter;
         BestCharacter = newCharacter;

         int rows = ItemGrid.RowDefinitions.Count;
         for (int i = 0; i < Character.OptimizableSlotCount; i++)
         {
            CharacterSlot slot = (CharacterSlot) i;
            ItemInstance oldModelItem = oldCharacter[slot];
            ItemInstance newModelItem = newCharacter[slot];

            // Testing if the ring/trinket items were just swapped and not actually different
            if (slot == CharacterSlot.Finger1 || slot == CharacterSlot.Finger2)
            {
               ItemInstance old1 = oldCharacter[CharacterSlot.Finger1];
               ItemInstance old2 = oldCharacter[CharacterSlot.Finger2];
               ItemInstance new1 = newCharacter[CharacterSlot.Finger1];
               ItemInstance new2 = newCharacter[CharacterSlot.Finger2];
               bool crossmatch1 = (old1 == null && new2 == null) ||
                                  (old1 != null && old1.Name == (new2 == null ? null : new2.Name));
               bool crossmatch2 = (old2 == null && new1 == null) ||
                                  (old2 != null && old2.Name == (new1 == null ? null : new1.Name));
               // if both cross match, we'll not print either row
               if (crossmatch1 && crossmatch2) continue;
               // if one cross matches, we'll swap the positions in the new model and let later matching rules decide what to print
               if (crossmatch1 || crossmatch2)
                  newModelItem = (slot == CharacterSlot.Finger1
                                     ? newCharacter[CharacterSlot.Finger2]
                                     : newCharacter[CharacterSlot.Finger1]);
            }
            else if (slot == CharacterSlot.Trinket1 || slot == CharacterSlot.Trinket2)
            {
               ItemInstance old1 = oldCharacter[CharacterSlot.Trinket1];
               ItemInstance old2 = oldCharacter[CharacterSlot.Trinket2];
               ItemInstance new1 = newCharacter[CharacterSlot.Trinket1];
               ItemInstance new2 = newCharacter[CharacterSlot.Trinket2];
               bool crossmatch1 = (old1 == null && new2 == null) ||
                                  (old1 != null && old1.Name == (new2 == null ? null : new2.Name));
               bool crossmatch2 = (old2 == null && new1 == null) ||
                                  (old2 != null && old2.Name == (new1 == null ? null : new1.Name));
               // if both cross match, we'll not print either row
               if (crossmatch1 && crossmatch2) continue;
               // if one cross matches, we'll swap the positions in the new model and let later matching rules decide what to print
               if (crossmatch1 || crossmatch2)
                  newModelItem = (slot == CharacterSlot.Trinket1
                                     ? newCharacter[CharacterSlot.Trinket2]
                                     : newCharacter[CharacterSlot.Trinket1]);
            }
            
            // not 'else if' here, since we may be deciding the final outcome of a swap above
            if (oldModelItem == null && newModelItem == null)
               continue;
            // and here an 'else' is unnecessary after a 'continue'
            if (oldModelItem != null && oldModelItem.Equals(newModelItem))
               continue;
            
            ItemGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            if (oldModelItem != null)
            {
               ItemDisplay oldItem = new ItemDisplay(oldModelItem);
               oldItem.Style = Resources["ItemDisplayStyle"] as Style;
               ItemGrid.Children.Add(oldItem);
               Grid.SetRow(oldItem, rows);
               Grid.SetColumn(oldItem, 0);
            }

            if (newModelItem != null)
            {
               ItemDisplay newItem = new ItemDisplay(newModelItem);
               newItem.Style = Resources["ItemDisplayStyle"] as Style;
               ItemGrid.Children.Add(newItem);
               Grid.SetRow(newItem, rows);
               Grid.SetColumn(newItem, 1);
            }

            rows++;
         }

         currentCalc = oldCharacter.CurrentCalculations.GetCharacterCalculations(oldCharacter, null, false, true, true);
         float oldValue = ItemInstanceOptimizer.GetOptimizationValue(oldCharacter, currentCalc);
         CurrentScoreLabel.Text = string.Format("Current: {0}", oldValue);
         CurrentCalculations.SetCalculations(currentCalc.GetCharacterDisplayCalculationValues());
         CurrentTalents.Character = oldCharacter; //CurrentTalents.IsEnabled = false;
         CurrentBuffs.Character = oldCharacter; //CurrentBuffs.IsEnabled = false;

         optimizedCalc = newCharacter.CurrentCalculations.GetCharacterCalculations(newCharacter, null, false, true, true);
         float newValue = ItemInstanceOptimizer.GetOptimizationValue(newCharacter, optimizedCalc);
         OptimizedScoreLabel.Text = string.Format("Optimized: {0} ({1:P} change)", newValue,
                                                  (newValue - oldValue)/oldValue);
         OptimizedCalculations.SetCalculations(optimizedCalc.GetCharacterDisplayCalculationValues());
         OptimizedTalents.Character = newCharacter; //OptimizedTalents.IsEnabled = false;
         OptimizedBuffs.Character = newCharacter; //OptimizedBuffs.IsEnabled = false;

         CharName2.Text = CharName1.Text = oldCharacter.Name;
         ItemCurrentScoreLabel.Text = CurrentScoreLabel.Text;
         ItemOptimizedScoreLabel.Text = OptimizedScoreLabel.Text;

         //OptimizedScoreLabel.LayoutUpdated += OptimizedScoreLabel_LayoutUpdated;
      }

      public Character CurrentCharacter { get; private set; }
      public Character BestCharacter { get; private set; }

      private void OptimizedScoreLabel_LayoutUpdated(object sender, EventArgs e)
      {
         ItemOptimizedScoreLabel.Text = OptimizedScoreLabel.Text;
      }

      private void OKButton_Click(object sender, RoutedEventArgs e)
      {
         DialogResult = true;
      }

      private void CancelButton_Click(object sender, RoutedEventArgs e)
      {
         DialogResult = false;
      }

      private void BT_StoreIt_Click(object sender, RoutedEventArgs e)
      {
         WeWantToStoreIt = true;
         DialogResult = true;
      }

      private void Print_Click(object sender, RoutedEventArgs e)
      {
         PrintButton.IsEnabled = false;
#if !SILVERLIGHT
         Mouse.OverrideCursor = Cursors.Wait;
#endif
         try
         {
            TabItem item = (TabItem) Tabs.SelectedItem;
            Panel panelToPrint;
            int hiddenRowCount = 1;

            switch (item.Name)
            {
               case "ItemsTab":
                  panelToPrint = ItemGrid;
                  hiddenRowCount = 2;
                  break;
               case "CalculationsTab":
                  panelToPrint = CalcGrid;
                  break;
               case "TalentsTab":
                  panelToPrint = TalentGrid;
                  break;
               default:
                  return;
            }

#if !SILVERLIGHT
   // need print dialog here since, if they choose to cancel, we don't need to create/destroy the header
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
               // add the header
               for (int index = 0; index < hiddenRowCount; index++)
                  ((Grid) panelToPrint).RowDefinitions[index].Height = GridLength.Auto;

               panelToPrint.Print(printDialog);

               // remove the header
               for (int index = 0; index < hiddenRowCount; index++)
                  ((Grid) panelToPrint).RowDefinitions[index].Height = new GridLength(0);
            }
#else
            // Create new a new PrintDocument object
            PrintDocument pd = new PrintDocument();
            pd.BeginPrint += (s, args) =>
                                {
                                   // add the header
                                   for (int index = 0; index < hiddenRowCount; index++)
                                   {
                                      ((Grid) panelToPrint).RowDefinitions[index].Height = GridLength.Auto;
                                   }
                                };
            pd.EndPrint += (s, args) =>
                              {
                                 // remove the header
                                 for (int index = 0; index < hiddenRowCount; index++)
                                 {
                                    ((Grid) panelToPrint).RowDefinitions[index].Height = new GridLength(0);
                                 }
                              };

            // Set the printable area
            pd.PrintPage += (s, args) =>
                               { args.PageVisual = panelToPrint; };

            // Print the document
            pd.Print("Rawr");

#endif
         }
         finally
         {
            PrintButton.IsEnabled = true;
#if !SILVERLIGHT
            Mouse.OverrideCursor = null;
#endif
         }
      }

      private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (Tabs == null || PrintButton == null) return;

#if !SILVERLIGHT
         PrintButton.Visibility = Visibility.Hidden;
#else
         PrintButton.Visibility = Visibility.Collapsed;
#endif
         if (Tabs.SelectedItem != null)
         {
            switch (((TabItem) Tabs.SelectedItem).Name)
            {
               case "ItemsTab":
               case "CalculationsTab":
               case "TalentsTab":
                  PrintButton.Visibility = Visibility.Visible;
                  break;
            }
         }
      }
   }
}