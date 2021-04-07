using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load_SimC_DBC
{
    public partial class Form1 : Form
    {
        private List<Spell> spell_data;
        private List<SpellEffect> spelleffect_data;
        private List<SpellPower> spellpower_data;
        private List<RPPMModifier> rppmmodifier_data;
        private bool PTR = false;
        private int buildLevel = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_DBCLive_Click(object sender, EventArgs e)
        {
            Uri url = new Uri("https://simulationcraft.googlecode.com/svn/trunk/engine/dbc/sc_spell_data.inc");
            try
            {
                PTR = false;
                retrieveDBC(url);
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "The file could not be read";
            }
        }

        private void btn_DBCPTR_Click(object sender, EventArgs e)
        {
			Uri url = new Uri("https://simulationcraft.googlecode.com/git-history/wod/engine/dbc/sc_spell_data.inc");
            try
            {
                PTR = true;
                retrieveDBC(url);
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "The file could not be read";
            }
        }

        private void getBuildLevel(string spell)
        {
            char[] delimiterChars = { ' ', ',', '{', '}', ':' };
            List<string> wordList = new List<string>();

            try
            {
                string[] words = spell.Split(delimiterChars);

                foreach (string word in words)
                {
                    if (word != "")
                        wordList.Add(word);
                }
                buildLevel = Convert.ToInt32(wordList[wordList.Count - 1]);
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "Error parsing Spell Effect data";
            }
        }

        private void convertSpell(string spell)
        {
            char[] delimiterChars = { '\"' };
            char[] delimiterCharsTwo = { ' ', ',', '{', '}', ':' };
            char[] delimiterCharsThree = { '*' };
            char[] delimiterCharsFour = { ' ', ',', '{', '}', ':', '/' };
            List<string> wordList = new List<string>();
            List<string> lastsection = null;
            List<string> finalList = new List<string>();

            try
            {
                if (spell.Contains("\\\""))
                    spell = spell.Replace("\\\"", "\'");
                if (spell.Contains("\\\'"))
                    spell = spell.Replace("\\\'", "\'");

                string[] words = spell.Split(delimiterChars);

                foreach (string word in words)
                {
                    if (word != "")
                        wordList.Add(word);
                }

				if (wordList.Count < 2)
				{
					return;
				}

                finalList.Add(wordList[1]);

                words = wordList[2].Split(delimiterCharsTwo);

                foreach (string word in words)
                {
                    if (word != "")
                        finalList.Add(word);
                }

				if(wordList.Count > 3)
					finalList.Add(wordList[3]);

                for (int i = 4; i < wordList.Count; i++)
                {
                    // Checking for commented parts
                    if (wordList[i].Contains("/*"))
                    {
                        lastsection = new List<string>();

                        // First separate the last section from the commented part
                        words = wordList[i].Split(delimiterCharsThree);

                        foreach (string word in words)
                        {
                            if (word != "")
                                lastsection.Add(word);
                        }

                        // take the non commented part and parse that
                        words = lastsection[0].Split(delimiterCharsFour);

                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                if (word == "0")
                                    finalList.Add("");
                                else
                                    finalList.Add(word);
                            }
                        }

                    }
                    // check to see if it has an empty space
                    else if (wordList[i].Contains(" 0,"))
                    {
                        words = wordList[i].Split(delimiterCharsTwo);

                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                if (word == "0")
                                    finalList.Add("");
                                else
                                    finalList.Add(word);
                            }
                        }
                    }
                    else if (wordList[i] != ", ")
                    {
                        finalList.Add(wordList[i]);
                    }
                }

                //spell_data.Add(spell);
                spell_data.Add(new Spell(finalList));
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "Error parsing Spell data";
            }
        }

        private void convertSpellEffect(string spell)
        {
            char[] delimiterChars = { ' ', ',', '{', '}', ':' };
            List<string> wordList = new List<string>();

            try
            {
                string[] words = spell.Split(delimiterChars);

                foreach (string word in words)
                {
                    if (word != "")
                        wordList.Add(word);
                }
                spelleffect_data.Add(new SpellEffect(wordList));
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "Error parsing Spell Effect data";
            }
        }

        private void convertSpellPower(string spell)
        {
            char[] delimiterChars = { ' ', ',', '{', '}', ':' };
            List<string> wordList = new List<string>();

            try
            {
                string[] words = spell.Split(delimiterChars);

                foreach (string word in words)
                {
                    if (word != "")
                        wordList.Add(word);
                }
                spellpower_data.Add(new SpellPower(wordList));
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "Error parsing Spell Power data";
            }
        }

        private void convertRPPMModifier(string rppm)
        {
            char[] delimiterChars = { ' ', ',', '{', '}', ':' };
            List<string> wordList = new List<string>();

            try
            {
                string[] words = rppm.Split(delimiterChars);

                foreach (string word in words)
                {
                    if (word != "")
                        wordList.Add(word);
                }
                rppmmodifier_data.Add(new RPPMModifier(wordList));
            }
            catch (Exception ex)
            {
                txt_DBC.Text = "Error parsing RPPM Modifier data";
            }
        }

        public void retrieveDBC(Uri url)
        {
            spell_data = new List<Spell>();
            spelleffect_data = new List<SpellEffect>();
            spellpower_data = new List<SpellPower>();
            rppmmodifier_data = new List<RPPMModifier>();
            using (WebClient client = new WebClient())
            {
                Stream stream = null;
                StreamReader sr = null;
                string temp = "";
                StringBuilder sb = new StringBuilder();

                bool spellData = false;
                bool spelleffectData = false;
                bool spellpowerData = false;
                bool rppmmodifierData = false;
                try
                {
                    //stream = client.OpenRead(url);
					stream = File.OpenRead("D:\\Theorycrafting\\simulationcraft\\engine\\dbc\\sc_spell_data.inc");
                    sr = new StreamReader(stream);
                    while (sr.EndOfStream != true)
                    {
                        temp = sr.ReadLine();
                        if (temp.Contains("Id,Flags,") == false) // remove all commented out header lines
                        {
                            sb.AppendLine(temp);

                            if (temp.Contains("spells, wow build level"))
                                getBuildLevel(temp);

                            // end data collection
                            else if ((temp.Contains("};")))
                            {
                                spellData = false;
                                spelleffectData = false;
                                spellpowerData = false;
                                rppmmodifierData = false;
                            }

                            // Add Spell data to it's own List
                            else if (spellData)
                                convertSpell(temp);

                            // Add Spell Effect data to it's own List
                            else if (spelleffectData)
                                convertSpellEffect(temp);

                            // Add Spell Power data to it's own List
                            else if (spellpowerData)
                                convertSpellPower(temp);

                            // Add RPPM data to it's own List
                            else if (rppmmodifierData)
                                convertRPPMModifier(temp);

                            // Start Spell Data collection (don't want to add this line to list)
                            else if (temp.Contains("spell_data_t"))
                                spellData = true;

                            // Start Spell Effect Data collection (don't want to add this line to list)
                            else if (temp.Contains("spelleffect_data_t"))
                                spelleffectData = true;

                            // Start Spell Power Data collection (don't want to add this line to list)
                            else if (temp.Contains("spellpower_data_t"))
                                spellpowerData = true;

                            // Start RPPM Modifier Data collection (don't want to add this line to list)
                            else if (temp.Contains("rppm_modifier_t"))
                                rppmmodifierData = true;
                        }
                    }
                    txt_DBC.Text = sb.ToString();
                    convert();
                }
                catch (Exception ex)
                {
                    txt_DBC.Text = "The file could not be read";
                }
            }
        }

        private void convert()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("");
            sb.AppendLine("namespace Rawr");
            sb.AppendLine("{");
            sb.AppendLine(String.Format("\tpublic class {0} : DBCData", (PTR ? "DBCPTR" : "DBCLive" ) ) );
            sb.AppendLine("\t{");
            sb.AppendLine(String.Format("\t\tpublic {0}()", (PTR ? "DBCPTR" : "DBCLive" ) ) );
            sb.AppendLine("\t\t{");
            sb.AppendLine(String.Format("\t\t\tPTR = {0};", (PTR ? "true" : "false") ) );
            sb.AppendLine(String.Format("\t\t\tbuildLevel = {0};", buildLevel));
            sb.AppendLine("\t\t\t{");
            foreach (Spell spell in spell_data)
            {
                sb.AppendLine(spell.ToString());
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\t{");
            foreach (SpellEffect spelleffect in spelleffect_data)
            {
                sb.AppendLine(spelleffect.ToString());
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\t{");
            foreach (SpellPower spellpower in spellpower_data)
            {
                sb.AppendLine(spellpower.ToString());
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\t{");
            foreach (RPPMModifier rppm in rppmmodifier_data)
            {
                sb.AppendLine(rppm.ToString());
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            txt_Converted.Text = sb.ToString();
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            if (txt_Converted.Text != "")
                Clipboard.SetText(txt_Converted.Text);
        }
    }
}
