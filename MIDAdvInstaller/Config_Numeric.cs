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
    public partial class Config_Numeric : InstallerControl
    {
        //public event
        public event EventHandler Config_Numeric_Click;
        public event EventHandler Config_Numeric_Edited;

        //init flag
        bool blInit = false;

        public Config_Numeric()
        {
            InitializeComponent();
        }

        public Config_Numeric(InstallerFrame p_frame)
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

        //label property
        public string Config_Numeric_Label
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

        //value property
        public decimal Config_Numeric_Value
        {
            get
            {
                return nmConfig_Numeric.Value;
            }
            set
            {
                nmConfig_Numeric.Value = value;
            }
        }

        //text property
        public string Config_Numeric_Text
        {
            get
            {
                return nmConfig_Numeric.Value.ToString().Trim();
            }
        }

        //increment property
        public decimal Config_Numeric_Increment
        {
            get
            {
                return nmConfig_Numeric.Increment;
            }
            set
            {
                nmConfig_Numeric.Increment = value;
            }
        }

        //maximum property
        public decimal Config_Numeric_Maximum
        {
            get
            {
                return nmConfig_Numeric.Maximum;
            }
            set
            {
                nmConfig_Numeric.Maximum = value;
            }
        }

        //minimum property
        public decimal Config_Numeric_Minimum
        {
            get
            {
                return nmConfig_Numeric.Minimum;
            }
            set
            {
                nmConfig_Numeric.Minimum = value;
            }
        }

        private void Config_NumericClick(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Numeric_Click(this, new EventArgs());
        }

        private void lblLabel_Click(object sender, EventArgs e)
        {
            nmConfig_Numeric.Select();
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Numeric_Click(this, new EventArgs());
        }

        private void nmConfig_Numeric_Click(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_Numeric_Click(this, new EventArgs());
        }

        private void Config_Numeric_LostFocus(object sender, EventArgs e)
        {
            lblLabelBold.Visible = false;
            lblLabel.Visible = true;
        }

        private void Config_Numeric_GotFocus(object sender, EventArgs e)
        {
            nmConfig_Numeric.Select();
        }

        private void nmConfig_Numeric_ValueChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_Numeric_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold + " (" + aDescription + ")");      //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }
    }
}
