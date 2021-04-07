using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.UI
{
    public partial class TalentPicker : UserControl
    {
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) { character.ClassChanged -= new EventHandler(character_ClassChanged); }
                character = value;
                if (character != null)
                {
                    character.ClassChanged += new EventHandler(character_ClassChanged);
                    character_ClassChanged(this, EventArgs.Empty);
                }
            }
        }

        public void RefreshSpec() { character_ClassChanged(this, EventArgs.Empty); }
        private void character_ClassChanged(object sender, EventArgs e)
        {
            Tree1.Talents = Character.CurrentTalents;
            Glyph.Character = Character;
            
            UpdateSavedSpecs();
        }

        public TalentPicker()
        {
            // Required to initialize variables
            InitializeComponent();

#if SILVERLIGHT
            Scroll1.SetIsMouseWheelScrollingEnabled(true);
            //Scroll2.SetIsMouseWheelScrollingEnabled(true);
            //Scroll3.SetIsMouseWheelScrollingEnabled(true);
#endif

            Tree1.TalentsChanged += new EventHandler(TalentsChanged);
            Glyph.TalentsChanged += new EventHandler(TalentsChanged);
        }

        public void TalentsChanged(object sender, EventArgs e)
        {
            UpdateSavedSpecs();
            Character.OnTalentChange();
            Character.OnCalculationsInvalidated();
        }

        public bool HasCustomSpec { get; private set; }

        private bool updating;
        private void UpdateSavedSpecs()
        {
            SavedTalentSpecList savedSpecs = SavedTalentSpec.SpecsFor(Character.Class);
            SavedTalentSpec current = null;
            updating = true;
            foreach (SavedTalentSpec sts in savedSpecs)
            {
                if (sts.Equals(Character.CurrentTalents))
                {
                    current = sts;
                    break;
                }
            }

            if (current != null)
            {
                HasCustomSpec = false;
                SavedCombo.ItemsSource = savedSpecs;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Delete";
            }
            else
            {
                HasCustomSpec = true;
                current = new SavedTalentSpec("Custom", Character.CurrentTalents, Tree1.Points()/*, Tree2.Points(), Tree3.Points()*/);
                SavedTalentSpecList currentList = new SavedTalentSpecList();
                currentList.AddRange(savedSpecs);
                currentList.Add(current);
                SavedCombo.ItemsSource = null;
                SavedCombo.ItemsSource = currentList;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Save";
            }
            updating = false;
        }

        private void SaveDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SavedTalentSpec currentSpec = SavedCombo.SelectedItem as SavedTalentSpec;
            if (HasCustomSpec)
            {
                SaveTalentSpecDialog dialog = new SaveTalentSpecDialog(currentSpec.TalentSpec(),
                    currentSpec.UsedPoints);
                dialog.Closed += new EventHandler(dialog_Closed);
                dialog.Show();
            }
            else
            {
                SavedTalentSpec.AllSpecs.Remove(currentSpec);
                UpdateSavedSpecs();
            }
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateSavedSpecs();
            }
        }

        private void SavedCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updating)
            {
                SavedTalentSpec newSpec = SavedCombo.SelectedItem as SavedTalentSpec;
                Character.CurrentTalents = newSpec.TalentSpec();
                Character.OnTalentChange();
                character_ClassChanged(this, EventArgs.Empty);
                Character.OnCalculationsInvalidated();
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
             ImportTalentSpecDialog itsdg = new ImportTalentSpecDialog();
             itsdg.Closed += new EventHandler(itsdg_Closed);
             itsdg.Show();
        }

        void itsdg_Closed(object sender, EventArgs e)
        {
            if ((sender as ImportTalentSpecDialog).DialogResult.GetValueOrDefault(false))
            {
                // Now we need to set the current talents to this new imported spec
                string newspec = (sender as ImportTalentSpecDialog).TB_Link.Text;
                if (newspec.Contains("wowhead")) {
                    // Example New Warrior Spec: http://www.wowhead.com/talent#w!\h|krMRmM
                    // Class: w
                    // Spec: !
                    // Talents: \h
                    // Glyphs:  krMRmM
                    // Separator: |
                    // WowHead works now
                    string s = newspec.Split('#')[1];
                    Character.CurrentTalents = parse_talents_wowhead(character.Class, s);
                    TalentsChanged(null, null);
                    character_ClassChanged(null, null);
                    return;
                } else if (newspec.Contains("mmo-champ") || newspec.Contains("wowtal") || newspec.Contains("wowdb")) {
                    // Example Warrior Spec: http://www.wowdb.com/talent-calculator#KmDKmGKmIKmWKmeKmbAbbF
                    // Class/Spec: F
                    // Talents: bb
                    // Glyphs:  KmDKmGKmIKmWKmeKmb
                    // Separator: A
                    // MMO-Champ works now
                    string s = newspec.Split('#')[1];
                    Character.CurrentTalents = parse_talents_mmochamp(character.Class, s);
                    TalentsChanged(null, null);
                    character_ClassChanged(null, null);
                    return;
                } else if (newspec.Contains("battle")) { // battle.net
                    // Example Druid spec: http://us.battle.net/wow/en/tool/talent-calculator#Ua!113221!akNt
                    // Class/Spec: Ua
                    // Talents: 113221
                    // Glyphs: akNt
                    // Separator: !
                    // Battle.net works now
                    string s = newspec.Split('#')[1];
                    Character.CurrentTalents = parse_talents_battlenet(character.Class, s);
                    TalentsChanged(null, null);
                    character_ClassChanged(null, null);
                    return;
                } else {
                    MessageBox.Show("The link you entered is not a valid talent spec, we need the full link of the talent spec for this to work.", "Invalid Talent Spec", MessageBoxButton.OK);
                }
            }
        }

        //*
        const int MAX_TALENT_POINTS = 7;
        const int MAX_TALENT_COL = 3;
        int MAX_TALENT_ROW { get { return MAX_TALENT_POINTS; } }
        int MAX_TALENT_SLOTS { get{ return (MAX_TALENT_ROW * MAX_TALENT_COL); } }

        public class decode_t {
            public decode_t(char? k, int? f, int? s, int? t) {
                key = k;
                first = f;
                second = s;
                third = t;
            }
            public char? key;
            public int? first;
            public int? second;
            public int? third;
        }

        public static decode_t[] wowhead_decoding = new decode_t[] {
            new decode_t(null, null, null, null),
            new decode_t('0', 0, null, null), new decode_t('4', 0, 0, null), new decode_t('8', 0, 1, null), new decode_t('<', 0, 2, null),
            new decode_t('D', 0, 0, 0), new decode_t('T', 0, 0, 1), new decode_t('d', 0, 0, 2),
            new decode_t('H', 0, 1, 0), new decode_t('X', 0, 1, 1), new decode_t('h', 0, 1, 2),
            new decode_t('L', 0, 2, 0), new decode_t('\\', 0, 2, 1), new decode_t('l', 0, 2, 2),
            new decode_t('1', 1, null, null), new decode_t('5', 1, 0, null), new decode_t('9', 1, 1, null), new decode_t('=', 1, 2, null),
            new decode_t('E', 1, 0, 0), new decode_t('U', 1, 0, 1), new decode_t('e', 1, 0, 2),
            new decode_t('I', 1, 1, 0), new decode_t('Y', 1, 1, 1), new decode_t('i', 1, 1, 2),
            new decode_t('M', 1, 2, 0), new decode_t(']', 1, 2, 1), new decode_t('m', 1, 2, 2),
            new decode_t('2', 2, null, null), new decode_t('6', 2, 0, null), new decode_t(':', 1, 1, null), new decode_t('>', 2, 2, null),
            new decode_t('F', 2, 0, 0), new decode_t('V', 2, 0, 1), new decode_t('f', 2, 0, 2),
            new decode_t('J', 2, 1, 0), new decode_t('Z', 2, 1, 1), new decode_t('j', 2, 1, 2),
            new decode_t('N', 2, 2, 0), new decode_t('^', 2, 2, 1), new decode_t('n', 2, 2, 2),
        };

        public static decode_t[] mmochamp_decoding = new decode_t[] {
            new decode_t('A', null, null, null),
            new decode_t('Q', 0, null, null), new decode_t('U', 0, 0, null), new decode_t('Y', 0, 1, null), new decode_t('c', 0, 2, null),
            new decode_t('V', 0, 0, 0), new decode_t('W', 0, 0, 1), new decode_t('X', 0, 0, 2),
            new decode_t('Z', 0, 1, 0), new decode_t('a', 0, 1, 1), new decode_t('b', 0, 1, 2),
            new decode_t('d', 0, 2, 0), new decode_t('e', 0, 2, 1), new decode_t('f', 0, 2, 2),
            new decode_t('g', 1, null, null), new decode_t('k', 1, 0, null), new decode_t('o', 1, 1, null), new decode_t('s', 1, 2, null),
            new decode_t('l', 1, 0, 0), new decode_t('m', 1, 0, 1), new decode_t('n', 1, 0, 2),
            new decode_t('p', 1, 1, 0), new decode_t('q', 1, 1, 1), new decode_t('r', 1, 1, 2),
            new decode_t('t', 1, 2, 0), new decode_t('u', 1, 2, 1), new decode_t('v', 1, 2, 2),
            new decode_t('w', 2, null, null), new decode_t('0', 2, 0, null), new decode_t('4', 1, 1, null), new decode_t('8', 2, 2, null),
            new decode_t('1', 2, 0, 0), new decode_t('2', 2, 0, 1), new decode_t('3', 2, 0, 2),
            new decode_t('5', 2, 1, 0), new decode_t('6', 2, 1, 1), new decode_t('7', 2, 1, 2),
            new decode_t('9', 2, 2, 0), new decode_t('/', 2, 2, 1), new decode_t('_', 2, 2, 2),
            // MMO-Champion changes the encoding for new WoD talents on their WoD calculator
            new decode_t('C', 0, null, null), new decode_t('E', 1, null, null), new decode_t('G', 2, null, null),
        };

        TalentsBase parse_talents_wowhead(CharacterClass characterclass, string talent_string)
        {
            // wowhead format: each letter represents one of the 27 combinations of 3 rows of talents

            string[] splitStrings = talent_string.Split('|');

            // If there is only one character in the talent substring, all it is is the class name, choke
            string talentSubstring = splitStrings[0];
            if (talentSubstring.Length > 1)
                talentSubstring = talentSubstring.Substring(1);
            else
                return null;

            if (wowhead_decoding.Find(dc => dc.key == talentSubstring[0]) == null)
            {
                // Spec is specified (one of !, x, y, or z, none of which show up in the decoding table)
                // If the spec is the only remaining character in the string, no talents are specified
                if (talentSubstring.Length > 1)
                    talentSubstring = talentSubstring.Substring(1);
                else
                    return null;
            }

            int talentCount = 21;

            bool[] encoding = new bool[talentCount];
            int row = 0;

            #region Talents parsing

            for (int i = 0; i < talentSubstring.Length; i++)
            {
                char c = talentSubstring[i];

                decode_t decode = wowhead_decoding.Find(dc => dc.key == c);

                if (decode == null)
                {
                    //sim -> errorf( "Player %s has malformed wowhead talent string. Translation for '%c' unknown.\n", name(), c );
                    return null;
                }

                if (decode.first.HasValue)
                {
                    encoding[row++ * 3 + decode.first.Value] = true;
                }
                else
                    ++row;

                if (decode.second.HasValue)
                {
                    encoding[row++ * 3 + decode.second.Value] = true;
                }
                else
                    ++row;

                if (decode.third.HasValue)
                {
                    encoding[row++ * 3 + decode.third.Value] = true;
                }
                else
                    ++row;
            }
            #endregion

            string newtalentstring = "";
            foreach (bool i in encoding) { newtalentstring += (i ? 1 : 0).ToString(); }

            switch (characterclass)
            {
                case CharacterClass.Warrior: { return new WarriorTalents(newtalentstring, null); }
                case CharacterClass.Paladin: { return new PaladinTalents(newtalentstring, null); }
                case CharacterClass.Hunter: { return new HunterTalents(newtalentstring, null); }
                case CharacterClass.Rogue: { return new RogueTalents(newtalentstring, null); }
                case CharacterClass.Priest: { return new PriestTalents(newtalentstring, null); }
                case CharacterClass.DeathKnight: { return new DeathKnightTalents(newtalentstring, null); }
                case CharacterClass.Shaman: { return new ShamanTalents(newtalentstring, null); }
                case CharacterClass.Mage: { return new MageTalents(newtalentstring, null); }
                case CharacterClass.Warlock: { return new WarlockTalents(newtalentstring, null); }
                case CharacterClass.Druid: { return new DruidTalents(newtalentstring, null); }
            }
            return null;
        }

        TalentsBase parse_talents_mmochamp(CharacterClass characterclass, string talent_string)
        {
            // MMO-Champion format: each letter represents one of the 27 combinations of 3 rows of talents

            // Class only
            if (talent_string.Length == 1)
                return null;

            string[] splitStrings;
            // Do not use Split() because 'A' is also the empty-talents character if glyphs are specified but no talents (for whatever reason)
            // So only use the first instance of A; if there is an A, there are glyphs (unless no talents are specified in the first 3 tiers, but who does that?)
            if (talent_string.Contains("A"))
                splitStrings = new string[] { talent_string.Substring(0, talent_string.IndexOf('A')), talent_string.Substring(talent_string.IndexOf('A') + 1) };
            else
                splitStrings = new string[] { null, talent_string };

            // If there is only one character in the talent substring, all it is is the class/spec name, choke
            // Unlike Wowhead, MMO-Champion combines the class and spec name into the same character at the end of the string
            string talentSubstring = splitStrings[1];
            if (talentSubstring.Length > 1)
                talentSubstring = talentSubstring.Substring(0, talentSubstring.Length - 1);
            else
                return null;

            int talentCount = 21;

            bool[] encoding = new bool[talentCount];
            int row = 0;

            #region Talents parsing

            // MMO-Champion talents go backwards, top level talents are at the left and bottom level talents are at the right
            for (int i = talentSubstring.Length - 1; i >= 0; --i)
            {
                char c = talentSubstring[i];

                decode_t decode = mmochamp_decoding.Find(dc => dc.key == c);

                if (decode == null)
                {
                    //sim -> errorf( "Player %s has malformed wowhead talent string. Translation for '%c' unknown.\n", name(), c );
                    return null;
                }

                if (decode.first.HasValue)
                {
                    encoding[row++ * 3 + decode.first.Value] = true;
                }
                else
                    ++row;

                if (decode.second.HasValue)
                {
                    encoding[row++ * 3 + decode.second.Value] = true;
                }
                else
                    ++row;

                if (decode.third.HasValue)
                {
                    encoding[row++ * 3 + decode.third.Value] = true;
                }
                else
                    ++row;
            }
            #endregion

            string newtalentstring = "";
            foreach (bool i in encoding) { newtalentstring += (i ? 1 : 0).ToString(); }

            switch (characterclass)
            {
                case CharacterClass.Warrior: { return new WarriorTalents(newtalentstring, null); }
                case CharacterClass.Paladin: { return new PaladinTalents(newtalentstring, null); }
                case CharacterClass.Hunter: { return new HunterTalents(newtalentstring, null); }
                case CharacterClass.Rogue: { return new RogueTalents(newtalentstring, null); }
                case CharacterClass.Priest: { return new PriestTalents(newtalentstring, null); }
                case CharacterClass.DeathKnight: { return new DeathKnightTalents(newtalentstring, null); }
                case CharacterClass.Shaman: { return new ShamanTalents(newtalentstring, null); }
                case CharacterClass.Mage: { return new MageTalents(newtalentstring, null); }
                case CharacterClass.Warlock: { return new WarlockTalents(newtalentstring, null); }
                case CharacterClass.Druid: { return new DruidTalents(newtalentstring, null); }
            }
            return null;
        }

        TalentsBase parse_talents_battlenet(CharacterClass characterclass, string talent_string)
        { 
            // Battle.Net format: Each talent is specified as a character [0..2] in a given position.
            // A . character represents a level whose talent is missing.
            // The talent string is bracketed between the class/spec on the left and glyphs on the right, with ! as the separator.

            string[] splitStrings = talent_string.Split('!');
            // No talents specified
            if (splitStrings.Length == 1 || string.IsNullOrEmpty(splitStrings[1]))
                return null;

            // Talent substring at this point is guaranteed to carry talent data
            string talentSubstring = splitStrings[1];

            int talentCount = 21;

            bool[] encoding = new bool[talentCount];

            #region Talents parsing

            // Battle.net talents have one per row
            for (int i = 0; i < talentSubstring.Length; ++i )
            {
                char c = talentSubstring[i];

                if (c == '.')
                    continue;

                encoding[i * 3 + (c - '0')] = true;
            }
            #endregion

            string newtalentstring = "";
            foreach (bool i in encoding) { newtalentstring += (i ? 1 : 0).ToString(); }

            switch (characterclass)
            {
                case CharacterClass.Warrior: { return new WarriorTalents(newtalentstring, null); }
                case CharacterClass.Paladin: { return new PaladinTalents(newtalentstring, null); }
                case CharacterClass.Hunter: { return new HunterTalents(newtalentstring, null); }
                case CharacterClass.Rogue: { return new RogueTalents(newtalentstring, null); }
                case CharacterClass.Priest: { return new PriestTalents(newtalentstring, null); }
                case CharacterClass.DeathKnight: { return new DeathKnightTalents(newtalentstring, null); }
                case CharacterClass.Shaman: { return new ShamanTalents(newtalentstring, null); }
                case CharacterClass.Mage: { return new MageTalents(newtalentstring, null); }
                case CharacterClass.Warlock: { return new WarlockTalents(newtalentstring, null); }
                case CharacterClass.Druid: { return new DruidTalents(newtalentstring, null); }
            }
            return null;
        }

        private void SavedCombo_DropDownOpened(object sender, EventArgs e)
        {
            TheTabControl.IsEnabled = false;
        }

        private void SavedCombo_DropDownClosed(object sender, EventArgs e)
        {
            TheTabControl.IsEnabled = true;
        }
    }
}
