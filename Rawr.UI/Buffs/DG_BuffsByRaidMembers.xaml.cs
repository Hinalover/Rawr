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
using System.Windows.Markup;
using System.IO;

namespace Rawr.UI
{
    public partial class DG_BuffsByRaidMembers : ChildWindow
    {
        public DG_BuffsByRaidMembers()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            // Set up the dialog
            CB_RaidSize.SelectedIndex = 1;
            CB_Class2Add.SelectedIndex = 0;
            #region Individual Class Work
            RB_DK_Blood.IsChecked = true;
            RB_Druid_Balance.IsChecked = true;
            RB_Hunter_BM.IsChecked = true;
            RB_Mage_Arcane.IsChecked = true;
            RB_Monk_Brewmaster.IsChecked = true;
            RB_Paladin_Holy.IsChecked = true;
            RB_Priest_D.IsChecked = true;
            RB_Rogue_Assassin.IsChecked = true;
            RB_Shaman_Elemental.IsChecked = true;
            RB_Warlock_Afflic.IsChecked = true;
            RB_Warrior_Arms.IsChecked = true;
            #endregion
        }

        public List<PlayerBuffSet> TheSets = new List<PlayerBuffSet>();

        #region List Editing
        public static Color FromKnownColor(string colorName)
        {
#if SILVERLIGHT
            Line lne = (Line)XamlReader.Load("<Line xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Fill=\"" + colorName + "\" />");
#else
            Line lne;
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("<Line xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Fill=\"" + colorName + "\" />")))
            {
                lne = (Line)XamlReader.Load(stream);
            }
#endif
            return (Color)lne.Fill.GetValue(SolidColorBrush.ColorProperty);
        }

        private void RaidSizeCheck()
        {
            // Disable the Add button if we've hit raid size
            if (List_Classes.Items.Count >= (Int32)CB_RaidSize.SelectedItem)
            { BT_Add.IsEnabled = false; } else { BT_Add.IsEnabled = true; }
            // Disable the Delete Button if there is no one in the Raid
            if (List_Classes.Items.Count == 0)
            { BT_Delete.IsEnabled = false; } else { BT_Delete.IsEnabled = true; }
            if (List_Classes.Items.Count == 0)
            { BT_DeleteAll.IsEnabled = false; } else { BT_DeleteAll.IsEnabled = true; }
        }
        private void CB_RaidSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaidSizeCheck();
        }
        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            #region Individual Class Work
            PlayerBuffSet theSet = new PlayerBuffSet();
            theSet.Color = Colors.White;
            switch ((string)CB_Class2Add.SelectedItem)
            {
                #region Death Knight
                case "Death Knight": {
                    theSet.Class = CharacterClass.DeathKnight;
                    theSet.Color = FromKnownColor("#c41e3b");
                    theSet.Spec = (RB_DK_Blood.IsChecked.GetValueOrDefault(false) ? "Blood" : 
                                  RB_DK_Frost.IsChecked.GetValueOrDefault(false) ? "Frost" :
                                  RB_DK_Unholy.IsChecked.GetValueOrDefault(false) ? "Unholy" : "Blood");
                    // All Specs
                    theSet.BuffsToAdd.Add("Horn of Winter", "Buff: Horn of Winter (Attack Power)");
                    // Spec Specific
                    if (theSet.Spec == "Blood") {
                        theSet.BuffsToAdd.Add("Scarlet Fever", "Debuff: Scarlet Fever (Weakened Blows)");
                    } else if (theSet.Spec == "Frost") {
                        theSet.BuffsToAdd.Add("Icy Talons", "Buff: Icy Talons (Attack Speed)");
                        theSet.BuffsToAdd.Add("Brittle Bones", "Debuff: Brittle Bones (Physical Vulnerability)");
                    } else if (theSet.Spec == "Unholy") {
                        theSet.BuffsToAdd.Add("Unholy Frenzy", "Buff: Unholy Fenzy (Temp Haste)");
                        theSet.BuffsToAdd.Add("Ebon Plaguebringer", "Ebon Plaguebringer (Physical Vulnerability)");
                    } 
                    break;
                }
                #endregion
                #region Druid
                case "Druid": {
                    theSet.Class = CharacterClass.Druid;
                    theSet.Color = FromKnownColor("#ff7c0a");
                    theSet.Spec = RB_Druid_Balance.IsChecked.GetValueOrDefault(false) ? "Balance" :
                                  RB_Druid_Feral.IsChecked.GetValueOrDefault(false) ? "Feral" :
                                  RB_Druid_Guardian.IsChecked.GetValueOrDefault(false) ? "Guardian" :
                                  RB_Druid_Restoration.IsChecked.GetValueOrDefault(false) ? "Restoration" : "Balance";
                    // All Specs
                    theSet.BuffsToAdd.Add("Mark of the Wild", "Buff: Mark of the Wild (Stats)");
                    theSet.BuffsToAdd.Add("Fearie Fire", "Debuff: Fearie Fire (Weakened Armor)");
                    // Spec Specific
                    if (theSet.Spec == "Balance") {
                        theSet.BuffsToAdd.Add("Moonkin Form", "Buff: Moonkin Form (Spell Haste)");
                    } else if (theSet.Spec == "Feral") {
                        theSet.BuffsToAdd.Add("Leader of the Pack", "Buff: Leader of the Pack (Critical Strike)");
                        theSet.BuffsToAdd.Add("Thrash", "Debuff: Thrash (Weakened Blows)");
                    } else if (theSet.Spec == "Guardian") {
                        theSet.BuffsToAdd.Add("Leader of the Pack", "Buff: Leader of the Pack (Critical Strike)");
                        theSet.BuffsToAdd.Add("Thrash", "Debuff: Thrash (Weakened Blows)");
                    }  else if (theSet.Spec == "Restoration") { }
                    break;
                }
                #endregion
                #region Hunter
                case "Hunter": {
                    theSet.Class = CharacterClass.Hunter;
                    theSet.Color = FromKnownColor("#aad372");
                    theSet.Spec = RB_Hunter_BM.IsChecked.GetValueOrDefault(false) ? "Beast Mastery" :
                                  RB_Hunter_MM.IsChecked.GetValueOrDefault(false) ? "Marksmanship" :
                                  RB_Hunter_SV.IsChecked.GetValueOrDefault(false) ? "Survival" : "Beast Mastery";
                    theSet.BuffsToAdd.Add("Widow Venom", "Debuff: Widow Venom (Mortal Wounds)");
                    // Spec Specific
                    if (theSet.Spec == "Beast Mastery") { }
                    else if (theSet.Spec == "Marksmanship") {
                        theSet.BuffsToAdd.Add("Trueshot Aura (Attack Power)", "Buff: Trueshot Aura (Attack Power)");
                        theSet.BuffsToAdd.Add("Trueshot Aura (Critical Strike)", "Buff: Trueshot Aura (Critical Strike)");
                    } else if (theSet.Spec == "Survival") {
                        //theSet.BuffsToAdd.Add("Hunting Party", "Buff: Hunting Party (Haste)");
                    }
                    // Pet
                    if (theSet.Spec == "Beast Mastery") {
                        if ((string)CB_Hunter_Pet_BM.SelectedItem != "None" && (string)CB_Hunter_Pet_BM.SelectedItem != "Other") {
                            string buffname = (string)CB_Hunter_Pet_BM.SelectedItem;
                            int colon = buffname.IndexOf(": ") + 2;
                            int perentheses = buffname.IndexOf(" (");
                            theSet.BuffsToAdd.Add(buffname.Substring(colon, (perentheses - colon)), "Buff: " + buffname.Substring(colon));
                        }
                    }
                    else {
                        if ((string)CB_Hunter_Pet.SelectedItem != "None" && (string)CB_Hunter_Pet.SelectedItem != "Other") {
                            string buffname = (string)CB_Hunter_Pet.SelectedItem;
                            int colon = buffname.IndexOf(": ") + 2;
                            int perentheses = buffname.IndexOf(" (");
                            theSet.BuffsToAdd.Add(buffname.Substring(colon, (perentheses - colon)), "Buff: " + buffname.Substring(colon));
                        }
                    }
                    break;
                }
                #endregion
                #region Mage
                case "Mage": {
                    theSet.Class = CharacterClass.Mage;
                    theSet.Color = FromKnownColor("#68ccef");
                    theSet.Spec = RB_Mage_Arcane.IsChecked.GetValueOrDefault(false) ? "Arcane" : 
                                  RB_Mage_Fire.IsChecked.GetValueOrDefault(false) ? "Fire" :
                                  RB_Mage_Frost.IsChecked.GetValueOrDefault(false) ? "Frost" : "Arcane";
                    // All Specs
                    theSet.BuffsToAdd.Add("Arcane Brilliance (Spell Power)", "Buff: Arcane Brilliance (Spell Power)");
                    theSet.BuffsToAdd.Add("Arcane Brilliance (Critical Strike)", "Buff: Arcane Brilliance (Critical Strike)");
                    theSet.BuffsToAdd.Add("Time Warp", "Buff: Time Warp (Temp Haste)");
                    // Spec Specific
                    if (theSet.Spec == "Arcane") { }
                    else if (theSet.Spec == "Fire") { }
                    else if (theSet.Spec == "Frost") { }
                    break;
                }
                #endregion
                #region Monk
                case "Monk":
                    {
                        theSet.Class = CharacterClass.Monk;
                        theSet.Color = FromKnownColor("#02B66C");
                        theSet.Spec = RB_Monk_Brewmaster.IsChecked.GetValueOrDefault(false) ? "Brewmaster" :
                                      RB_Monk_Mistwalker.IsChecked.GetValueOrDefault(false) ? "Mistwalker" :
                                      RB_Monk_Windwalker.IsChecked.GetValueOrDefault(false) ? "Windwalker" : "Windwalker";
                        // All Specs
                        theSet.BuffsToAdd.Add("Legacy of the Emperor", "Buff: Legacy of the Emperor (Stats)");
                        // Spec Specific
                        if (theSet.Spec == "Brewmaster") {
                            theSet.BuffsToAdd.Add("Keg Smash", "Debuff: Keg Smash (Weakened Blows)");
                        }
                        else if (theSet.Spec == "Mistwalker") { }
                        else if (theSet.Spec == "Windwalker") {
                            theSet.BuffsToAdd.Add("Legacy of the White Tiger", "Buff: Legacy of the White Tiger (Mastery)");
                        }
                        break;
                    }
                #endregion
                #region Paladin
                case "Paladin": {
                    theSet.Class = CharacterClass.Paladin;
                    theSet.Color = FromKnownColor("#f48cba");
                    theSet.Spec = RB_Paladin_Holy.IsChecked.GetValueOrDefault(false) ? "Holy" : 
                                  RB_Paladin_Prot.IsChecked.GetValueOrDefault(false) ? "Protection" :
                                  RB_Paladin_Ret.IsChecked.GetValueOrDefault(false) ? "Retribution" : "Holy";
                    // All Specs
                    // Spec Specific
                    if (theSet.Spec == "Holy") { }
                    else if (theSet.Spec == "Protection") {
                        theSet.BuffsToAdd.Add("Hammer of the Righteous", "Debuff: Hammer of the Righteous (Weakened Blows)");
                    } else if (theSet.Spec == "Retribution") {
                        theSet.BuffsToAdd.Add("Judgments of the Bold", "Debuff: Judgments of the Bold (Physical Vulnerability)");
                        theSet.BuffsToAdd.Add("Hammer of the Righteous", "Debuff: Hammer of the Righteous (Weakened Blows)");
                    }
                    // Blessing
                    if ((string)CB_Paladin_Blessing.SelectedItem != "None") {
                        string sString = (string)CB_Paladin_Blessing.SelectedItem;
                        int iString = sString.IndexOf(" (");
                        theSet.BuffsToAdd.Add(sString.Substring(0, iString), "Buff: " + (string)CB_Paladin_Blessing.SelectedItem);
                    }
                    break;
                }
                #endregion
                #region Priest
                case "Priest": {
                    theSet.Class = CharacterClass.Priest;
                    theSet.Color = FromKnownColor("#f0ebe0");
                    theSet.Spec = RB_Priest_D.IsChecked.GetValueOrDefault(false) ? "Disciplin" :
                                  RB_Priest_Holy.IsChecked.GetValueOrDefault(false) ? "Holy" :
                                  RB_Priest_S.IsChecked.GetValueOrDefault(false) ? "Shadow" : "Disciplin";
                    // All Specs
                    theSet.BuffsToAdd.Add("Power Word: Fortitude", "Buff: Power Word: Fortitude (Stamina)");
                    theSet.BuffsToAdd.Add("Power Infusion", "Buff: Power Infusion (Temp Spell Haste)");
                    // Spec Specific
                    if (theSet.Spec == "Disciplin"){
                        theSet.BuffsToAdd.Add("Hymn of Hope", "Buff: Hymn of Hope (Burst Mana Regeneration)");
                    } else if (theSet.Spec == "Holy") { }
                    else if (theSet.Spec == "Shadow") {
                        theSet.BuffsToAdd.Add("Shadowform", "Buff: Shadowform (Spell Haste)");
                    }
                    break;
                }
                #endregion
                #region Rogue
                case "Rogue": {
                    theSet.Class = CharacterClass.Rogue;
                    theSet.Color = FromKnownColor("#fff468");
                    theSet.Spec = RB_Rogue_Assassin.IsChecked.GetValueOrDefault(false) ? "Assassination" :
                                  RB_Rogue_Combat.IsChecked.GetValueOrDefault(false) ? "Combat" :
                                  RB_Rogue_Subtlety.IsChecked.GetValueOrDefault(false) ? "Subtlety" : "Assassination";
                    // All Specs
                    theSet.BuffsToAdd.Add("Swiftblade's Cunning", "Buff: Swiftblade's Cunning (Attack Speed)");
                    theSet.BuffsToAdd.Add("Expose Armor", "Debuff: Expose Armor (Weakened Armor)");
                    theSet.BuffsToAdd.Add("Master Poisoner", "Debuff: Master Poisoner (Magic Vulnerability)");
                    // Spec Specific
                    if (theSet.Spec == "Assassination") { }
                    else if (theSet.Spec == "Combat") { }
                    else if (theSet.Spec == "Subtlety") { }
                    // Poisons
                    if ((string)CB_Rogue_Poisons.SelectedItem != "None")
                    {
                        string sString = (string)CB_Rogue_Poisons.SelectedItem;
                        int iString = sString.IndexOf(" (");
                        theSet.BuffsToAdd.Add(sString.Substring(0, iString), "Debuff: " + (string)CB_Rogue_Poisons.SelectedItem);
                    }
                    // Tricks of the Trade
                    if (CB_Rogue_Tricks.SelectedIndex == 1) {
                        string text = "Tricks of the Trade";
                        theSet.BuffsToAdd.Add(text, "Buff: " + text + " (Temp Dmg %)");
                        if (CK_Rogue_Tricks.IsChecked.GetValueOrDefault(false))
                        {
                            text += " (Glyphed)";
                            theSet.BuffsToAdd.Add(text, "Buff: " + text + " (Temp Dmg %)");
                        }
                    }
                    break;
                }
                #endregion
                #region Shaman
                case "Shaman": {
                    theSet.Class = CharacterClass.Shaman;
                    theSet.Color = FromKnownColor("#2359ff");
                    theSet.Spec = RB_Shaman_Enhance.IsChecked.GetValueOrDefault(false) ? "Elemental" :
                                  RB_Shaman_Elemental.IsChecked.GetValueOrDefault(false) ? "Enhancement" :
                                  RB_Shaman_Restoration.IsChecked.GetValueOrDefault(false) ? "Restoration" : "Elemental";
                    // All Specs
                    theSet.BuffsToAdd.Add("Burning Wrath", "Buff: Burning Wrath (Spell Power)");
                    theSet.BuffsToAdd.Add("Grace of Air", "Buff: Grace of Air (Mastery)");
                    theSet.BuffsToAdd.Add("Heroism/Bloodlust", "Buff: Heroism/Bloodlust (Temp Haste)");
                    theSet.BuffsToAdd.Add("Stormlash Totem", "Buff: Stormlash Totem (Temp Power Boost)");
                    theSet.BuffsToAdd.Add("Earth Shock", "Debuff: Earth Shock (Weakened Blows)");
                    // Spec Specific
                    if (theSet.Spec == "Elemental") {
                        theSet.BuffsToAdd.Add("Elemental Oath", "Elemental Oath (Spell Haste)");
                    }
                    else if (theSet.Spec == "Enhancement") {
                        theSet.BuffsToAdd.Add("Unleashed Rage", "Unleashed Rage (Attack Speed)");
                    }
                    else if (theSet.Spec == "Restoration") {
                        theSet.BuffsToAdd.Add("Mana Tide Totem", "Mana Tide Totem (Burst Mana Regeneration)");
                    }
                    break;
                }
                #endregion
                #region Warlock
                case "Warlock": {
                    theSet.Class = CharacterClass.Warlock;
                    theSet.Color = FromKnownColor("#9382c9");
                    theSet.Spec = RB_Warlock_Afflic.IsChecked.GetValueOrDefault(false) ? "Affliction" : 
                                  RB_Warlock_Demon.IsChecked.GetValueOrDefault(false) ? "Demonology" :
                                  RB_Warlock_Destro.IsChecked.GetValueOrDefault(false) ? "Destruction" : "Affliction";
                    // All Specs
                    theSet.BuffsToAdd.Add("Dark Intent", "Buff: Dark Intent (Spell Power)");
                    // Spec Specific
                    if (theSet.Spec == "Affliction") { }
                    else if (theSet.Spec == "Demonology") { }
                    else if (theSet.Spec == "Destruction") { }
                    // Curse
                    if (CB_Warlock_Curse.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Curse of Elements", "Debuff: Curse of Elements (Magic Vulnerability)");
                    }
                    else if (CB_Warlock_Curse.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Curse of Enfeeblement", "Debuff: Curse of Enfeeblement (Weakened Blows)");
                    }
                    // Pet
                    if (CB_Warlock_Pet.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Blood Pact", "Buff: Blood Pact (Stamina)");
                    }
                    break;
                }
                #endregion
                #region Warrior
                case "Warrior": {
                    theSet.Class = CharacterClass.Warrior;
                    theSet.Color = FromKnownColor("#c69b6d");
                    theSet.Spec = RB_Warrior_Arms.IsChecked.GetValueOrDefault(false) ? "Arms" :
                                  RB_Warrior_Fury.IsChecked.GetValueOrDefault(false) ? "Fury" :
                                  RB_Warrior_Tank.IsChecked.GetValueOrDefault(false) ? "Protection" : "Arms";
                    // All Specs
                    theSet.BuffsToAdd.Add("Skull Banner", "Buff: Skull Banner (Temp Crit Bonus Damage)");
                    theSet.BuffsToAdd.Add("Shattering Throw", "Debuff: Shattering Throw (Temp Armor Reduc)");
                    theSet.BuffsToAdd.Add("Thunder Clap", "Debuff: Thunder Clap (Weakened Blows)");
                    // Spec Specific
                    if (theSet.Spec == "Arms") {
                        theSet.BuffsToAdd.Add("Sunder Armor", "Debuff: Sunder Armor (Weakened Armor)");
                        theSet.BuffsToAdd.Add("Colossus Smash", "Debuff: Colossus Smash (Physical Vulnerability)");
                        theSet.BuffsToAdd.Add("Mortal Strike", "Debuff: Mortal Strike (Mortal Wounds)");
                    } else if (theSet.Spec == "Fury") {
                        theSet.BuffsToAdd.Add("Sunder Armor", "Debuff: Sunder Armor (Weakened Armor)");
                        theSet.BuffsToAdd.Add("Colossus Smash", "Debuff: Colossus Smash (Physical Vulnerability)");
                        theSet.BuffsToAdd.Add("Wild Strike", "Debuff: Wild Strike (Mortal Wounds)");
                    }
                    else if (theSet.Spec == "Protection") {
                        theSet.BuffsToAdd.Add("Devastate", "Debuff: Devastate (Weakened Armor)");
                    }
                    // Buff Shout
                    if (CB_Warrior_BuffShout.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Commanding Shout", "Buff: Commanding Shout (Stamina)");
                    } else if (CB_Warrior_BuffShout.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Battle Shout", "Buff: Battle Shout (Attack Power)");
                    }
                    break;
                }
                #endregion
                default: { break; }
            }
            #endregion
            // Add if not blank
            if (theSet.BuffsToAdd.Keys.Count > 0)
            {
                ListBoxItem newAdd = new ListBoxItem();
                newAdd.Content = theSet.ToString();
                newAdd.Background = new SolidColorBrush(theSet.Color);
                newAdd.Background = new LinearGradientBrush(new GradientStopCollection() {
                    new GradientStop() { Color = Colors.White, Offset = 0 },
                    new GradientStop() { Color = theSet.Color, Offset = 1 }
                }, 0);
                List_Classes.Items.Add(newAdd);
                TheSets.Add(theSet);
            }
            // Verify we can add more people after this
            RaidSizeCheck();
        }
        private void BT_Delete_Click(object sender, RoutedEventArgs e)
        {
            // Make sure there is something to Remove
            if (List_Classes.SelectedItem == null) return;
            int index = List_Classes.SelectedIndex;
            // Remove the Listing
            List_Classes.Items.RemoveAt(index);
            TheSets.RemoveAt(index);
            // Finish off
            RaidSizeCheck();
        }
        private void BT_DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            // Remove all of the Listings
            List_Classes.Items.Clear();
            TheSets.Clear();
            // Finish off
            RaidSizeCheck();
        }
        #endregion

        private void CB_Class2Add_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GB_DeathKnight.Visibility = Visibility.Collapsed;
            GB_Druid.Visibility = Visibility.Collapsed;
            GB_Hunter.Visibility = Visibility.Collapsed;
            GB_Mage.Visibility = Visibility.Collapsed;
            GB_Monk.Visibility = Visibility.Collapsed;
            GB_Priest.Visibility = Visibility.Collapsed;
            GB_Paladin.Visibility = Visibility.Collapsed;
            GB_Rogue.Visibility = Visibility.Collapsed;
            GB_Shaman.Visibility = Visibility.Collapsed;
            GB_Warlock.Visibility = Visibility.Collapsed;
            GB_Warrior.Visibility = Visibility.Collapsed;

            switch ((string)CB_Class2Add.SelectedItem)
            {
                case "Death Knight": { GB_DeathKnight.Visibility = Visibility.Visible; break; }
                case "Druid": { GB_Druid.Visibility = Visibility.Visible; break; }
                case "Hunter": { GB_Hunter.Visibility = Visibility.Visible; break; }
                case "Mage": { GB_Mage.Visibility = Visibility.Visible; break; }
                case "Monk": { GB_Monk.Visibility = Visibility.Visible; break; }
                case "Priest": { GB_Priest.Visibility = Visibility.Visible; break; }
                case "Paladin": { GB_Paladin.Visibility = Visibility.Visible; break; }
                case "Rogue": { GB_Rogue.Visibility = Visibility.Visible; break; }
                case "Shaman": { GB_Shaman.Visibility = Visibility.Visible; break; }
                case "Warlock": { GB_Warlock.Visibility = Visibility.Visible; break; }
                case "Warrior": { GB_Warrior.Visibility = Visibility.Visible; break; }
                default: { break; }
            }
        }

        #region Individual Class Work
        // Druid
        private void CK_DruidSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Death Knight
        private void CK_DeathKnightSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Hunter
        private void CK_HunterSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Hunter_BM.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet_BM.Visibility = Visibility.Visible;
                CB_Hunter_Pet_BM.SelectedIndex = 0;
                CB_Hunter_Pet.Visibility = Visibility.Collapsed;
            } else if (RB_Hunter_MM.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet_BM.Visibility = Visibility.Collapsed;
                CB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet.SelectedIndex = 0;
            } else if (RB_Hunter_SV.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet_BM.Visibility = Visibility.Collapsed;
                CB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet.SelectedIndex = 0;
            }
        }
        // Mage
        private void CK_MageSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Monk
        private void CK_MonkSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Paladin
        private void CK_PaladinSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Paladin_Prot.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Blessing.SelectedIndex = 1;
            } else if (RB_Paladin_Ret.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Blessing.SelectedIndex = 2;
            } else if (RB_Paladin_Holy.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Blessing.SelectedIndex = 1;
            } else {
                // Set them all to None
                CB_Paladin_Blessing.SelectedIndex = 0;
            }
        }
        // Priest
        private void CK_PriestSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Rogue
        private void CK_RogueSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Shaman
        private void CK_ShamanSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Shaman_Enhance.IsChecked.GetValueOrDefault(false)) {
            } else if (RB_Shaman_Elemental.IsChecked.GetValueOrDefault(false)) {
            } else if (RB_Shaman_Restoration.IsChecked.GetValueOrDefault(false)) {
            } else {
                // Set them all to None
            }
        }
        // Warlock
        private void CK_WarlockSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Warrior
        private void CK_WarriorSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Buff Shouts
            LB_Warrior_BuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_BuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_BuffShout.SelectedIndex = 0;

            if (RB_Warrior_Tank.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 1;
                // Thunderclap: Always
                // Sunder: Always
            } else if (RB_Warrior_Arms.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 2;
            } else if (RB_Warrior_Fury.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 2;
            }
        }
        #endregion

        #region Dialog Exiting
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        private void BT_Import_Click(object sender, RoutedEventArgs e)
        {
            DG_ImportMMMORaid DG_ImportMMMORaid = new DG_ImportMMMORaid();
            DG_ImportMMMORaid.Closed += new EventHandler(DG_ImportMMMORaid_Closed);
            DG_ImportMMMORaid.Show();
        }

        void DG_ImportMMMORaid_Closed(object sender, EventArgs e)
        {
            DG_ImportMMMORaid DG_ImportMMMORaid = sender as DG_ImportMMMORaid;
            if (DG_ImportMMMORaid.DialogResult.GetValueOrDefault(false))
            {
                foreach (PlayerBuffSet theSet in DG_ImportMMMORaid.toAdds)
                {
                    if (theSet.BuffsToAdd.Keys.Count > 0)
                    {
                        ListBoxItem newAdd = new ListBoxItem();
                        newAdd.Content = theSet.ToString();
                        newAdd.Background = new SolidColorBrush(theSet.Color);
                        newAdd.Background = new LinearGradientBrush(new GradientStopCollection() {
                            new GradientStop() { Color = Colors.White, Offset = 0 },
                            new GradientStop() { Color = theSet.Color, Offset = 1 }
                        }, 0);
                        List_Classes.Items.Add(newAdd);
                        TheSets.Add(theSet);
                    }
                }
                // Verify we can add more people after this
                RaidSizeCheck();
            }
        }
    }

}
