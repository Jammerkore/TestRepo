using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class Config_Directory  : InstallerControl
    {
        //public event
        public event EventHandler Config_Dir_Click;
        public event EventHandler Config_Dir_Edited;

        //Directory variable
        string Config_Dir = "";

        //init flag
        bool blInit = false;

        public Config_Directory()
        {
            InitializeComponent();
        }

        public Config_Directory(InstallerFrame p_frame)
            : base(p_frame)
        {
            InitializeComponent();
        }

        //control button click
        private void btnConfig_Dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderConfig = new FolderBrowserDialog();

            folderConfig.SelectedPath = lblConfig_Dir.Text;
            if (folderConfig.ShowDialog() == DialogResult.OK)
            {
                Config_Dir = folderConfig.SelectedPath;
                lblConfig_Dir.Text = Config_Dir;
            }

            Config_Dir_Click(this, new EventArgs());
        }

        //initialized property
        public bool Initialized
        {
            get
            {
                return blInit;
            }
            set
            {
                blInit = value;
            }
        }

        //control label
        public string ConfigDirLabel
        {
            get
            {
                return lblLabel.Text.Trim();
            }
            set
            {
                lblLabel.Text = value;
                lblLabelBold.Text = value;
            }

        }

        //configuration directory property
        public string ConfigurationDirectory
        {
            get
            {
                Config_Dir = lblConfig_Dir.Text.Trim();
                return Config_Dir;
            }
            set
            {
                lblConfig_Dir.Text = value;
                Config_Dir = lblConfig_Dir.Text.Trim();
            }
        }

        //resize reactions
        private void Config_Dir_Resize(object sender, EventArgs e)
        {
            lblConfig_Dir.Width = this.Width - btnConfig_Dir.Width - 5;
        }

        //raise the click event
        private void lblConfig_Dir_Click(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Dir_Click(this, new EventArgs());
        }

        //raise the click event
        private void Config_DirClick(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Dir_Click(this, new EventArgs());
        }

        //click on the label
        private void lblLabel_Click(object sender, EventArgs e)
        {
            lblConfig_Dir.Select();
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Dir_Click(this, new EventArgs());
        }

        private void Config_Directory_LostFocus(object sender, EventArgs e)
        {
            lblLabelBold.Visible = false;
            lblLabel.Visible = true;
        }

        private void Config_Directory_GotFocus(object sender, EventArgs e)
        {
            lblConfig_Dir.Select();
        }

        private void lblConfig_Dir_TextChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_Dir_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text + " (" + aDescription + ")");      //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }
     }
}
