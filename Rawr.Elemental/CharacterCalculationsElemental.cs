using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public class CharacterCalculationsElemental : CharacterCalculationsBase
    {

        #region Variable Declarations and Definitions
        private Stats basicStats;
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        private float overallPoints = 0f;
        public override float OverallPoints
        {
            get { return overallPoints; }
            set { overallPoints = value; }
        }

        private float[] subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float DPSPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float SurvivabiltyPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            string displayFormat = "";
            int formIter = 1;

            #region Summary
            dictValues.Add("DPS Points", DPSPoints.ToString("F2"));
            dictValues.Add("Survivability Points", SurvivabiltyPoints.ToString("F2"));
            dictValues.Add("Overall Points", OverallPoints.ToString("F2"));
            #endregion
            /*
             * Displayed:
             * Base | Average | Max
             * */
            #region Basic Stats
            #region Base Stats
            dictValues.Add("Health", basicStats.Health.ToString("F0"));
            dictValues.Add("Mana", basicStats.Mana.ToString("F0"));

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            displayFormat += "\r\nIncreases Attack Power by {1:0.#}";
            dictValues.Add("Strength", string.Format(displayFormat,
                basicStats.Strength, basicStats.Strength - 10f));

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            displayFormat += "\r\nIncreases Attack Power by {1:0.#}";
            displayFormat += "\r\nIncreases Critical Hit chance by {2:0.00%}";
            dictValues.Add("Agility", basicStats.Agility.ToString());

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            displayFormat += "\r\nIncreases Health by {1:0.#}";
            dictValues.Add("Stamina", string.Format(displayFormat,
                basicStats.Stamina, StatConversion.GetHealthFromStamina(basicStats.Stamina)));

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            displayFormat += "\r\nIncreases Spell Power by {1:0.#}";
            displayFormat += "\r\nIncreases Spell Critical Hit chance by {2:0.00%}";
            dictValues.Add("Intellect", string.Format(displayFormat,
                basicStats.Intellect, "0", "0"));

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            dictValues.Add("Spirit", basicStats.Spirit.ToString());

            displayFormat = "";
            displayFormat += "{0:0.#}*";
            displayFormat += "Stats Pane shows Averaged Values*";
            displayFormat += "\r\nMastery rating of {1:0.#} adds {2:0.0#} Mastery";
            displayFormat += "\r\n{3:0.00%}.";
            dictValues.Add("Mastery", string.Format(displayFormat,
                basicStats.Mastery,
                basicStats.MasteryRating, StatConversion.GetMasteryFromRating(basicStats.MasteryRating),
                basicStats.Mastery * 1f));
            #endregion
            #region Secondary Stats
            dictValues.Add("Spell Power", basicStats.SpellPower.ToString());
            dictValues.Add("Spell Haste", String.Format("{0:0%}", basicStats.SpellHaste));
            dictValues.Add("Spell Crit", String.Format("{0:0%}", basicStats.SpellCrit));
            dictValues.Add("Combat Regen", basicStats.SpellCombatManaRegeneration.ToString());
            #endregion
            #region Tertiary Stats

            #endregion
            #endregion

            //Elemental Focus uptime
            dictValues.Add("EF Uptime", "0");
            //Lightning shield stacks
            dictValues.Add("Avg Time to 7 Stacks", "0");
            dictValues.Add("MH Enchant Uptime", "0");
            dictValues.Add("OH Enchant Uptime", "0");
            dictValues.Add("Trinket 1 Uptime", "0");
            dictValues.Add("Trinket 2 Uptime", "0");
            dictValues.Add("Fire Totem Uptime", "0");
            // Time to OOM

            /*
             * Displayed:
             *   DPS | Damage per Hit | # Uses | % Total Damage
             * Tooltip:
             *   Cast Time
			 *   Damage per Cast Time (or equiv)
			 *   Non-Crit Dmg
			 *   Crit Dmg
			 *   EoE proc Dmg
			 *   EO proc Dmg
             * */
            dictValues.Add("Description", string.Format("DPS : PerHit : #Uses : %DPS*All values use averaged stats"));

            dictValues.Add("Elemental Blast", "");
            dictValues.Add("Lava Burst", "");
            dictValues.Add("Lightning Bolt", "");
            dictValues.Add("Earth Shock", "");
            dictValues.Add("Flame Shock", "");
            dictValues.Add("Fulmination", "");
            dictValues.Add("Unleash Flame", "");
            dictValues.Add("Searing Totem", "");
            dictValues.Add("Earth Elemental", "");
            dictValues.Add("Fire Elemental", "");
            dictValues.Add("Air Elemental", "");
            dictValues.Add("Chain Lightning", "");
            dictValues.Add("Earthquake", "");
            dictValues.Add("Lava Beam", "");
            dictValues.Add("Magma Totem", "");
            dictValues.Add("Thunderstorm", "");
            dictValues.Add("Magic Other", "");
            dictValues.Add("Physical Other", "");

            dictValues.Add("Total DPS + Damage", "");

            return dictValues;
        }
    }
}
