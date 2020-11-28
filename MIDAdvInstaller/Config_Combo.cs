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
    public partial class Config_Combo : InstallerControl
    {
        //public events
        public event EventHandler Config_Combo_Click;
        public event EventHandler Config_Combo_Edited;

        //init flag
        bool blInit = false;

        public Config_Combo()
        {
            InitializeComponent();
        }

        public Config_Combo(InstallerFrame p_frame)
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
        public string Config_Combo_Label
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
        public string Config_Combo_Text
        {
            get
            {
                return cboConfig_Combo.Text.ToString().Trim();
            }
            set
            {
                cboConfig_Combo.Text = value;
            }
        }

        //pick list property
        List<string> lstConfigCbo = new List<string>();
        public List<string> Config_Combo_List
        {
            get
            {
                foreach (string Config_Combo_Item in cboConfig_Combo.Items)
                {
                    lstConfigCbo.Add(Config_Combo_Item);
                }

                return lstConfigCbo;
            }
            set
            {
                lstConfigCbo = value;

                foreach (string cboItem in lstConfigCbo)
                {
                    cboConfig_Combo.Items.Add(cboItem);
                }
            }
        }

        private void lblLabel_Click(object sender, EventArgs e)
        {
            cboConfig_Combo.Select();
            lblLabel.Visible = false;
            lblLabelBold.Visible = true;
            Config_Combo_Click(this, new EventArgs());
        }

        private void cboConfig_ComboClick(object sender, EventArgs e)
        {
            lblLabel.Visible = false;
            lblLabelBold.Visible = true;
            Config_Combo_Click(this, new EventArgs());
        }

        private void Config_Combo_LostFocus(object sender, EventArgs e)
        {
            lblLabel.Visible = true;
            lblLabelBold.Visible = false;
        }

        private void Config_Combo_GotFocus(object sender, EventArgs e)
        {
            //cboConfig_Combo.Select();
        }

        //BEGIN TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        private void lblLabel_Resize(object sender, EventArgs e)
        {
            //if (lblLabel.Width > 165)
            //{
            //    lblLabel.Text = lblLabel.Text.Substring(0, 25) + "...";
            //}
        }

        private void lblLabelBold_Resize(object sender, EventArgs e)
        {
            //if (lblLabelBold.Width > 165)
            //{
            //    lblLabelBold.Text = lblLabelBold.Text.Substring(0, 19) + "...";
            //}
        }
        //END TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011

        private void cboConfig_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_Combo_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }

    }
}
