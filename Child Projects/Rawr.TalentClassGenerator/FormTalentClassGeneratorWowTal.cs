using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Globalization;


// AS OF 04/10/2011:
// - THE LATEST 4.1.0 BUILD (UNRELEASED PATCH) IS 13726
// - THE LATEST LIVE BUILD IS 13623 according to wow client


namespace Rawr.TalentClassGenerator
{
	public partial class FormTalentClassGeneratorWowTal : Form
	{
		public FormTalentClassGeneratorWowTal()
		{
			InitializeComponent();
		}

		private void buttonGenerateCode_Click(object sender, EventArgs e)
		{
            textBoxCode.Text = @"using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rawr
{
	
";
			List<ClassData> classes = new List<ClassData>();
			classes.Add(new ClassData()
			{
				ID = 1,
				Name = "Warrior",
				TreeNames = new string[] { "Arms", "Fury", "Protection" },
				TreeIDs = new string[] { "746", "815", "845" }
			});
			classes.Add(new ClassData()
			{
				ID = 2,
				Name = "Paladin",
				TreeNames = new string[] { "Holy", "Protection", "Retribution" },
				TreeIDs = new string[] { "831", "839", "855" }
			});
			classes.Add(new ClassData()
			{
				ID = 3,
				Name = "Hunter",
				TreeNames = new string[] { "Beast Mastery", "Marksmanship", "Survival" },
				TreeIDs = new string[] { "811", "807", "809" }
			});
			classes.Add(new ClassData()
			{
				ID = 4,
				Name = "Rogue",
				TreeNames = new string[] { "Assassination", "Combat", "Subtlety" },
				TreeIDs = new string[] { "182", "181", "183" }
			});
			classes.Add(new ClassData()
			{
				ID = 5,
				Name = "Priest",
				TreeNames = new string[] { "Discipline", "Holy", "Shadow" },
				TreeIDs = new string[] { "760", "813", "795" }
			});
			classes.Add(new ClassData()
			{
				ID = 6,
				Name = "DeathKnight",
				TreeNames = new string[] { "Blood", "Frost", "Unholy" },
				TreeIDs = new string[] { "398", "399", "400" }
			});
			classes.Add(new ClassData()
			{
				ID = 7,
				Name = "Shaman",
				TreeNames = new string[] { "Elemental", "Enhancement", "Restoration" },
				TreeIDs = new string[] { "261", "263", "262" }
			});
			classes.Add(new ClassData()
			{
				ID = 8,
				Name = "Mage",
				TreeNames = new string[] { "Arcane", "Fire", "Frost" },
				TreeIDs = new string[] { "799", "851", "823" }
			});
			classes.Add(new ClassData()
			{
				ID = 9,
				Name = "Warlock",
				TreeNames = new string[] { "Affliction", "Demonology", "Destruction" },
				TreeIDs = new string[] { "871", "867", "865" }
			});
			classes.Add(new ClassData()
			{
				ID = 11,
				Name = "Druid",
				TreeNames = new string[] { "Balance", "Feral Combat", "Restoration" },
				TreeIDs = new string[] { "752", "750", "748" }
			});



			//for (int i = 0; i < classId.Length; i++)
			foreach (ClassData classData in classes)
			{
                string read = "";
                try {
                    ProcessTalentDataJSON(read = new StreamReader(System.Net.HttpWebRequest.Create(
                        string.Format("http://static.mmo-champion.com/db/js/talents/{0}-{1}.js",
                        textBoxUrl.Text, classData.Name.ToUpper())).GetResponse().GetResponseStream()).ReadToEnd(), classData);
                } catch (Exception ex) {
                    textBoxCode.Text = "FAILED on: " + classData.Name + " : " + ex.Message + "\r\n\r\n" + ex.StackTrace + "\r\n\r\n" + read;
                    textBoxUrl.Text = (int.Parse(textBoxUrl.Text) - 1).ToString();
                    return;
                }
			}
            textBoxCode.Text += "}\r\n";
			textBoxCode.SelectAll();
			textBoxCode.Focus();
		}

		private void ProcessTalentDataJSON(string fullResponse, ClassData classData)
		{
			//Clean formatting out of the Descriptions
			fullResponse = fullResponse
				.Replace("</div></div>", "\r\n")
				.Replace("\\r\\n", "\r\n")
				.Replace(" <NNF>", "")
				.Replace(" <NYI>", "")
				.Replace("&#39;", "'")
				.Replace("</div>", " - ")
				.Replace("<span style=\"color:#FFFFFF;\">", "")
                .Replace("<span style=\"color: #FFFFFF;\">", "")
				.Replace("</span>", "")
				.Replace("<div class=\"tt-spell sigrie-tooltip\">", "")
				.Replace("<div class=\"tt-name tts-name\">", "")
				.Replace("<div class=\"tts-rank\">", "")
				.Replace("</a>", "")
				.Replace("<div class=\"tt-spell-range_power\">", "")
				.Replace("<div class=\"tts-power_cost\">", "")
				.Replace("<div class=\"tt-spell-cast_cooldown\">", "")
				.Replace("<div class=\"tts-cooldown tt-spell-right\">", "")
				.Replace("<div class=\"tts-cast_time\">", "")
				.Replace("<div class=\"tts-description\">", "")
				.Replace("<div class=\"tts-range\">", "")
				.Replace("<div class=\"tts-required_tools\">", "")
				.Replace("<div class=\"tts-rune_cost tts-power_cost\">", "")
				.Replace("<div class=\"tts-required_stances\">", "")
				.Replace("<div class=\"tts-range tt-spell-right\">", "")
				.Replace("<div class=\"tts-power_cost tts-power_percent\">", "")
				.Replace("<div class=\"tts-required_item_subclasses\">", "");
			while (fullResponse.Contains("<a href=\""))
			{
				string href = fullResponse.Between("<a href=\"", "\">");
				fullResponse = fullResponse.Replace("<a href=\"" + href + "\">", "");
			}


			List<TalentData> talents = new List<TalentData>();
			string[] allTalents = fullResponse.Between("[\"talents\"] = {", "}};").Split(new string[] { "}," }, StringSplitOptions.None);
			string[] allDescriptions = fullResponse.Between("[\"tooltips\"] = {", "}};").Split(new string[] { "}," }, StringSplitOptions.None);

			foreach (string strTalent in allTalents)
			{
				//10496: {'depends_count': 0, 'depends': 10486, 'max_ranks': 3, 'name': 'Sword and Board', 'tab': 845, 'column': 2, 'icon': 'ability_warrior_swordandboard', 'active': false, 'row': 5

				TalentData talent = new TalentData();
				talent.ID = strTalent.Before(":").Trim();
				talent.Name = strTalent.Between("'name': ", ", 'tab'").Trim('\'','"');
				talent.Icon = strTalent.Between("'icon': '", "',"); ;
				talent.MaxPoints = int.Parse(strTalent.Between("'max_ranks': ", ","));
				talent.Prerequisite = int.Parse(strTalent.Between("'depends': ", ",")); ; //Set this to the ID for now, will update it to be the Index in a moment
				if (string.IsNullOrWhiteSpace(talent.Name) || talent.MaxPoints == 0) continue;
				string treeID = strTalent.Between("'tab': ", ",");
				talent.Tree = treeID == classData.TreeIDs[0] ? 0 : (treeID == classData.TreeIDs[1] ? 1 : 2);
				talent.Row = int.Parse(strTalent.After("'row': ")) + 1;
				talent.Column = int.Parse(strTalent.Between("'column': ", ",")) + 1;
				string strDescription = allDescriptions.First(str => str.Trim().StartsWith(talent.ID + ":")).After("{");
				if (strDescription.After(": ")[0]=='\'')
				{ //It's quoted using '
					talent.Description = strDescription.TrimEnd('\'')
						.Split(new string[] { "'," }, StringSplitOptions.None).Select(str => str.After("'").Trim()).ToArray();
				}
				else
				{ //It's quoted using "
					talent.Description = strDescription.TrimEnd('"')
						.Split(new string[] { "\"," }, StringSplitOptions.None).Select(str => str.After("\"").Trim()).ToArray();
				}
				// 1: "Your critical strikes cause the opponent to bleed, dealing 16% of your melee weapon's average damage over 6 sec.", 2: "Your critical strikes cause the opponent to bleed, dealing 32% of your melee weapon's average damage over 6 sec.", 3: "Your critical strikes cause the opponent to bleed, dealing 48% of your melee weapon's average damage over 6 sec."
				talents.Add(talent);
			}

			talents = talents.OrderBy(talent => talent.Tree * 1000 + talent.Row * 10 + talent.Column * 1).ToList();
			int index = 0;
			foreach (var talent in talents)
				talent.Index = index++;
			foreach (var talent in talents.Where(t => t.Prerequisite > 0))
				talent.Prerequisite = talents.First(t => t.ID == talent.Prerequisite.ToString()).Index;
			
			//Generaete the code
			string className = classData.Name + "Talents";
			StringBuilder code = new StringBuilder();
			code.AppendFormat(
@"	public partial class {0} : TalentsBase
	{{
", className);
            int lasttree = -1;
			foreach (TalentData talent in talents)
			{
                if (lasttree == -1) {
                    lasttree = 0;
                    code.Append(string.Format("		#region {0}\r\n", classData.TreeNames[lasttree]));
                } else if (lasttree == 0 && talent.Tree == 1) {
                    lasttree = 1;
                    code.Append("		#endregion\r\n");
                    code.Append(string.Format("		#region {0}\r\n", classData.TreeNames[lasttree]));
                } else if (lasttree == 1 && talent.Tree == 2) {
                    lasttree = 2;
                    code.Append("		#endregion\r\n");
                    code.Append(string.Format("		#region {0}\r\n", classData.TreeNames[lasttree]));
                }
                code.Append(GenerateComment(talent));
                code.Append("\r\n");
				code.AppendFormat("public int {0} {{ get; set; }}\r\n",
					PropertyFromName(talent.Name));
			}
            code.Append("		#endregion\r\n");
            code.Append("	}\r\n\r\n");

			textBoxCode.Text += code.ToString();
		}

		/// <summary>
		/// Generate a comment for the talent field, based on it's description( Replaceed changed value by [BaseNumber * Pts])
		/// </summary>
		/// <param name="Talent">Given talent</param>
		/// <returns>The comment</returns>
		private string GenerateComment(TalentData Talent)
		{
			string Comment = Talent.Description[Talent.Description.Length - 1];

			if (Talent.Description.Length > 1)
			{
				char[] SplitCharacter = new char[] { ' ', '%' };
				string[] FirstRank = Talent.Description[0].Split(SplitCharacter, StringSplitOptions.RemoveEmptyEntries);
				string[] LastRank = Talent.Description[Talent.Description.Length - 1].Split(SplitCharacter, StringSplitOptions.RemoveEmptyEntries);

				int ReplacePos = 0;

				//Description contains the same count of words for all ranks, diference only in some values
				for (int i = 0; i < Math.Min(FirstRank.Length, LastRank.Length); i++)
				{
					if (FirstRank[i] != LastRank[i])
					{
						//To avoid string like "... increase by 5."
						if (FirstRank[i].Contains(".") == false) FirstRank[i] = FirstRank[i] + ".0";
						else FirstRank[i] = FirstRank[i] + "0";

						float BaseNumber = 0;
						if (float.TryParse(FirstRank[i], NumberStyles.Any, new NumberFormatInfo(), out BaseNumber))
						{
							float MaxNumber = 0;
							string Replaced = "[{0} * Pts]";
							if (float.TryParse(LastRank[i], NumberStyles.Any, new NumberFormatInfo(), out MaxNumber))
							{
								int Base = Convert.ToInt32(BaseNumber);
								int Max = Convert.ToInt32(MaxNumber);

								//Number like: BaseNumber - 7, MaxNumber - 20
								if ((Base > 0) && ((Max / Base) * Base) != Max)
								{
									Replaced = "[" + MaxNumber + " / " + Talent.Description.Length + " * Pts]";
								}
							}

							Comment = Replace(Comment, LastRank[i], String.Format(Replaced, BaseNumber), ref ReplacePos);
						}
					}
				}
			}

			Comment = @"/// <summary>" + Environment.NewLine +
					  @"/// " + Comment.Replace("\r\n", "\r\n/// ") + Environment.NewLine +
					  @"/// </summary>";
			return Comment;
		}

		/// <summary>
		/// Returns a new string in which first occurrences of a specified string after position in this input string are replaced with another specified string.
		/// </summary>
		/// <param name="Text">Input string</param>
		/// <param name="OldValue">A string to be replaced</param>
		/// <param name="NewValue">A string to replace first occurrences of OldValue</param>
		/// <param name="Position">The starting character position, after wich, first occurences be replaced. 
		/// When this method returns, contains the 32-bit signed integer value, indicates the position, after replaced string  </param>
		/// <returns>A String equivalent to the input string but with first instance of OldValue replaced with NewValue</returns>
		private string Replace(string Text, string OldValue, string NewValue, ref int Position)
		{
			int NewPosition = Text.IndexOf(OldValue, Position);
			string Res = Text.Substring(0, NewPosition) + NewValue + Text.Substring(NewPosition + OldValue.Length);

			Position = NewPosition + NewValue.Length;
			return Res;
		}

		private string PropertyFromName(string name)
		{
			name = name.Replace("'", ""); // don't camel word after apostrophe
			string[] arr = name.Split(new char[] { ' ', ',', ':', '(', ')', '.', '-','!' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = Char.ToUpperInvariant(arr[i][0]) + arr[i].Substring(1);
			}
			return string.Join("", arr);
		}

		private class TalentData
		{
			public string ID { get; set; }
			public int Index { get; set; }
			public string Name { get; set; }
			public int MaxPoints { get; set; }
			public int Tree { get; set; }
			public int Column { get; set; }
			public int Row { get; set; }
			public string Icon { get; set; }
			public int Prerequisite { get; set; }
			public string[] Description { get; set; }
		}

		private class ClassData
		{
			public int ID { get; set; }
			public string Name { get; set; }
			public string[] TreeNames { get; set; }
			public string[] TreeIDs { get; set; }
		}
	}
}
