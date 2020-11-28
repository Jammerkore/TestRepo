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
    public partial class Config_Text : InstallerControl
    {
        //public event
        public event EventHandler Config_Text_Click;
        public event EventHandler Config_Text_Edited;

        //init flag
        bool blInit = false;

        public Config_Text()
        {
            InitializeComponent();

        }

        public Config_Text(InstallerFrame p_frame)
            : base(p_frame)
        {
            InitializeComponent();

        }

        //initialize property
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

        //label property
        public string Config_Text_Label
        {
            get
            {
                return lblLabel.Text;
            }
            set
            {
                lblLabel.Text = value;
                lblLabelBold.Text = value;
            }
        }

        //text property
        public string Config_Text_Text
        {
            get
            {
                return txtConfig_Text.Text;
            }
            set
            {
                txtConfig_Text.Text = value;
            }
        }

        private void Config_TextClick(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Text_Click(this, new EventArgs());
        }

        private void lblLabel_Click(object sender, EventArgs e)
        {
            txtConfig_Text.Select();
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Text_Click(this, new EventArgs());
        }

        private void txtConfig_Text_Click(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Text_Click(this, new EventArgs());
        }

        private void Config_Text_LostFocus(object sender, EventArgs e)
        {
            lblLabelBold.Visible = false;
            lblLabel.Visible = true;
        }

        private void Config_Text_GotFocus(object sender, EventArgs e)
        {
            txtConfig_Text.Select();
        }

        private void txtConfig_Text_TextChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_Text_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold + " (" + aDescription + ")");      //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }
    }
}
