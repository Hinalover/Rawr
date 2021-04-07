using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr.ServerListGenerator
{
    public class RegionData
    {
        public string Region { get; set; }
        public string Host { get; set; }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonGenerateCode_Click(object sender, EventArgs e)
        {
            textBoxCode.Text = @"        static BNetLoadDialog()
        {
            #region Server Names
            ServerNames = new Dictionary<string, List<string>>();
";

            List<RegionData> regions = new List<RegionData>();
            regions.Add(new RegionData()
            {
                Region = "US",
                Host = "us.battle.net"
            });
            regions.Add(new RegionData()
            {
                Region = "EU",
                Host = "eu.battle.net"
            });
            regions.Add(new RegionData()
            {
                Region = "KR",
                Host = "kr.battle.net"
            });
            regions.Add(new RegionData()
            {
                Region = "TW",
                Host = "tw.battle.net"
            });
            regions.Add(new RegionData()
            {
                Region = "CN",
                Host = "www.battlenet.com.cn"
            });


            foreach (RegionData region in regions)
            {
                string read = "";
                try
                {
                    ProcessRegionDataJSON(read = new StreamReader(System.Net.HttpWebRequest.Create(
                        "http://" + region.Host + "/api/wow/realm/status").GetResponse().GetResponseStream()).ReadToEnd(), region);
                }
                catch (Exception ex)
                {
                    textBoxCode.Text = "FAILED on: " + region.Region + " : " + ex.Message + "\r\n\r\n" + ex.StackTrace + "\r\n\r\n" + read;
                    return;
                }
            }
            textBoxCode.Text += @"            #endregion
        }";
            textBoxCode.SelectAll();
            textBoxCode.Focus();
        }

        private void ProcessRegionDataJSON(string fullResponse, RegionData region)
        {
            StringBuilder code = new StringBuilder();
            var data = JsonParser.Parse(fullResponse, false);

            code.Append("            #region " + region.Region + " Servers\r\n");
            code.Append("            ServerNames[\"" + region.Region + "\"] = new List<string>() {\r\n");
            foreach (Dictionary<string, object> realm in (object[])data["realms"])
            {
                code.Append("                \"" + realm["name"] + "\",\r\n");
            }
            code.Append("            };\r\n");
            code.Append("            ServerNames[\"" + region.Region + "\"].Sort();\r\n");
            code.Append("            #endregion\r\n");

            textBoxCode.Text += code.ToString();
        }
    }
}
