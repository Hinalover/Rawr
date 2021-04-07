using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Rawr.UI
{
    public partial class TalentTree : UserControl
    {
        public TalentTree()
        {
            // Required to initialize variables
            InitializeComponent();
/*#if RAWR4
            // Losing 4 rows
            for (int i=8; i<12; i++) { GridPanel.RowDefinitions[i].Height = new GridLength(0, GridUnitType.Pixel); }
            MinHeight = 465;
#endif*/
            talentAttributes = new Dictionary<int, TalentDataAttribute>();
            belowRow = new Dictionary<int, int>();
        }

        public EventHandler TalentsChanged;

        public CharacterClass Class { get; set; }

        private TalentsBase talents;
        public TalentsBase Talents
        {
            get { return talents; }
            set
            {
                if (value != null)
                {
                    talents = value;
                    for (int r = 1; r <= 7; r++)
                    {
                        for (int c = 1; c <= 3; c++)
                        {
                            this[r, c].TalentData = null;
                        }
                    }
                    talentAttributes.Clear();
                    belowRow.Clear();
                    Class = talents.GetClass();
                    //TreeName = ((string[])Talents.GetType().GetField("TreeNames").GetValue(Talents))[Tree];
                    //BackgroundImage.Source = Icons.TreeBackground(Class, Tree);
                    foreach (PropertyInfo pi in Talents.GetType().GetProperties())
                    {
                        TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                        if (talentDatas.Length > 0)
                        {
                            TalentDataAttribute talentData = talentDatas[0];
                            //if (talentData.Tree != Tree) continue;
                            this[talentData.Row + 1, talentData.Column + 1].TalentData = talentData;
                            talentAttributes[talentData.Index] = talentData;
                        }
                    }
                    for (int r = 1; r <= 7; r++)
                    {
                        for (int c = 1; c <= 3; c++)
                        {
                            this[r, c].Update();
                        }
                    }
                }
            }
        }

        public void RankChanged()
        {
            belowRow.Clear();
            for (int r = 1; r <= 7; r++)
            {
                for (int c = 1; c <= 3; c++)
                {
                    this[r, c].Update();
                }
            }
            if (TalentsChanged != null) TalentsChanged.Invoke(this, EventArgs.Empty);
        }

        private Dictionary<int, int> belowRow;
        public int PointsBelowRow(int row)
        {
            if (belowRow.ContainsKey(row)) return belowRow[row];
            else
            {
                int pts = 0;
                for (int r = 1; r < row; r++)
                {
                    for (int c = 1; c <= 3; c++)
                    {
                        pts += this[r, c].Current;
                    }
                }
                belowRow[row] = pts;
                return pts;
            }
        }
        public int PointsInRow(int row)
        {
            if (row < 1 || row > 7) return 0;
            int pts = 0;
            for (int c = 1; c <= 3; c++)
            {
                pts += this[row, c].Current;
            }
            return pts;
        }
        public int Points() { return PointsBelowRow(8); }

        private Dictionary<int, TalentDataAttribute> talentAttributes;
        public TalentDataAttribute GetAttribute(int index)
        {
            if (talentAttributes.ContainsKey(index)) return talentAttributes[index];
            else return null;
        }

        private TalentItem this[int row, int col]
        {
            get
            {
                if (row == 1)
                {
                    if (col == 1) return Talent_1_1;
                    if (col == 2) return Talent_1_2;
                    if (col == 3) return Talent_1_3;
                    //if (col == 4) return Talent_1_4;
                }
                else if (row == 2)
                {
                    if (col == 1) return Talent_2_1;
                    if (col == 2) return Talent_2_2;
                    if (col == 3) return Talent_2_3;
                    //if (col == 4) return Talent_2_4;
                }
                else if (row == 3)
                {
                    if (col == 1) return Talent_3_1;
                    if (col == 2) return Talent_3_2;
                    if (col == 3) return Talent_3_3;
                    //if (col == 4) return Talent_3_4;
                }
                else if (row == 4)
                {
                    if (col == 1) return Talent_4_1;
                    if (col == 2) return Talent_4_2;
                    if (col == 3) return Talent_4_3;
                    //if (col == 4) return Talent_4_4;
                }
                else if (row == 5)
                {
                    if (col == 1) return Talent_5_1;
                    if (col == 2) return Talent_5_2;
                    if (col == 3) return Talent_5_3;
                    //if (col == 4) return Talent_5_4;
                }
                else if (row == 6)
                {
                    if (col == 1) return Talent_6_1;
                    if (col == 2) return Talent_6_2;
                    if (col == 3) return Talent_6_3;
                    //if (col == 4) return Talent_6_4;
                }
                else if (row == 7)
                {
                    if (col == 1) return Talent_7_1;
                    if (col == 2) return Talent_7_2;
                    if (col == 3) return Talent_7_3;
                    //if (col == 4) return Talent_7_4;
                }
                /*else if (row == 8)
                {
                    if (col == 1) return Talent_8_1;
                    if (col == 2) return Talent_8_2;
                    if (col == 3) return Talent_8_3;
                    if (col == 4) return Talent_8_4;
                }
                else if (row == 9)
                {
                    if (col == 1) return Talent_9_1;
                    if (col == 2) return Talent_9_2;
                    if (col == 3) return Talent_9_3;
                    if (col == 4) return Talent_9_4;
                }
                else if (row == 10)
                {
                    if (col == 1) return Talent_10_1;
                    if (col == 2) return Talent_10_2;
                    if (col == 3) return Talent_10_3;
                    if (col == 4) return Talent_10_4;
                }
                else if (row == 11)
                {
                    if (col == 1) return Talent_11_1;
                    if (col == 2) return Talent_11_2;
                    if (col == 3) return Talent_11_3;
                    if (col == 4) return Talent_11_4;
                }*/
                return null;
            }
        }

    }
}