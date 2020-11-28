using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class AddSetting : Form
    {
        List<string> Settings;
        public AddSetting(List<string> p_settings)
        {
            InitializeComponent();

            Settings = p_settings;
        }

        private void AddSetting_Load(object sender, EventArgs e)
        {
            foreach (string setting in Settings)
            {
                lstSettings.Items.Add(setting);   
            }
        }

        public string Setting
        {
            get
            {
                string strSetting = "";
                if (lstSettings.SelectedItems.Count > 0)
                {
                    strSetting = lstSettings.SelectedItem.ToString().Trim();
                }

                return strSetting;
            }
        }
    }
}
