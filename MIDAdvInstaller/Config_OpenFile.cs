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
    public partial class Config_OpenFile : InstallerControl
    {
        //public event
        public event EventHandler Config_File_Click;
        public event EventHandler Config_File_Edited;

        //init flag
        bool blInit = false;

        //Directory variable
        string Config_File = "";

        public Config_OpenFile()
        {
                InitializeComponent();
        }

        public Config_OpenFile(InstallerFrame p_frame)
            : base(p_frame)
        {
            InitializeComponent();
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

        //control button click
        private void btnConfig_File_Click(object sender, EventArgs e)
        {
            //openConfig_File.FileName = lblConfig_File.Text;
            openConfig_File.InitialDirectory = System.IO.Path.GetDirectoryName(lblConfig_File.Text);
            string strExtension = System.IO.Path.GetExtension(lblConfig_File.Text);
            openConfig_File.Filter = "File|*" + strExtension;
            if (openConfig_File.ShowDialog() == DialogResult.OK)
            {
                Config_File = openConfig_File.FileName;
                lblConfig_File.Text = Config_File;
            }

            Config_File_Click(this, new EventArgs());
        }

        //configuration directory property
        public string ConfigurationFile
        {
            get
            {
                Config_File = lblConfig_File.Text.Trim();
                return Config_File;
            }
            set
            {
                lblConfig_File.Text = value;
                Config_File = lblConfig_File.Text.Trim();
            }
        }

        //control label
        public string ConfigFileLabel
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

        //resize reactions
        private void Config_File_Resize(object sender, EventArgs e)
        {
            lblConfig_File.Width = this.Width - btnConfig_File.Width - 5;
        }

        //raise the click event
        private void lblConfig_File_Click(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_File_Click(this, new EventArgs());
        }

        //raise the click event
        private void Config_FileClick(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_File_Click(this, new EventArgs());
        }

        //handle label click
        private void lblLabel_Click(object sender, EventArgs e)
        {
            lblConfig_File.Select();
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_File_Click(this, new EventArgs());
        }

        private void Config_OpenFile_LostFocus(object sender, EventArgs e)
        {
            lblLabelBold.Visible = false;
            lblLabel.Visible = true;
        }

        private void Config_OpenFile_GotFocus(object sender, EventArgs e)
        {
            lblConfig_File.Select();
        }

        private void lblConfig_File_TextChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_File_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text.Trim() + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold.Text.Trim() + " (" + aDescription + ")");      //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }
      
     }
}
