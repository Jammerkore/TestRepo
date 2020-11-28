using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;

namespace MIDRetailInstaller
{
    public partial class Config_Sql : InstallerControl
    {
        //public event
        public event EventHandler Config_SqlClick;
        public event EventHandler Config_Sql_Edited;

        //init flag
        bool blInit = false;

        //Directory variable
        string ConfigSql = "";

        public Config_Sql()
        {
            InitializeComponent();
        }

        //frame variable
        InstallerFrame frame = null;
        public Config_Sql(InstallerFrame p_frame)
            : base(p_frame)
        {
            frame = p_frame;
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
        private void btnConfig_Sql_Click(object sender, EventArgs e)
        {
            //invoke the data connection dialog 
            DataConnectionDialog dCon = new DataConnectionDialog();

            //add the datasource to the dialog
            DataSource.AddStandardDataSources(dCon);

            //choose the sql datasource
            dCon.SelectedDataSource = DataSource.SqlDataSource;
            dCon.SelectedDataProvider = DataProvider.SqlDataProvider;
            dCon.ConnectionString = ConfigurationSql;   // TT#1384-MD - stodd - Pre-fill database connection properties when requested from installer config setting

            //enter the datasource text to the text box
            if (DataConnectionDialog.Show(dCon) == DialogResult.OK)
            {
                lblConfig_Sql.Text = dCon.ConnectionString.Trim();
            }

            //Begin TT#1205 - Verify version of sql server during install - APicchetti - 3/18/2011
            //Verify the database version and edition
            string productversion = "";
            string productlevel = "";
            string edition = "";
            string server = "";
            string database = "";
            // Begin TT#74 MD - JSmith - One-button Upgrade
            //if (frame.VerifySQLVersion_Edition(dCon.ConnectionString.Trim(),out productversion, out productlevel, out edition, out server, out database) != true)
            string databaseUser = "user";
            string databasePwd = "pwd";
            // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
            bool isDatabaseCompatible = false;
            CompatibilityLevel compatibilityLevel;
            //if (frame.VerifySQLVersion_Edition(dCon.ConnectionString.Trim(), out productversion, out productlevel, out edition, out server, out database, out databaseUser, out databasePwd) != true)
            if (frame.VerifySQLVersion_Edition(dCon.ConnectionString.Trim(), out productversion, out productlevel, out edition, out server, out database, out databaseUser, out databasePwd, out isDatabaseCompatible, out compatibilityLevel) != true)
            // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
            {
			// End TT#74 MD
                MessageBox.Show(this, "The target database does not meet MID installation requirements: " + Environment.NewLine +
                    "   Server:       " + server + Environment.NewLine +
                    "   Database:     " + database + Environment.NewLine +
                    "   SQL Version:  " + productversion + Environment.NewLine +                        
                    "   SQL Level:    " + productlevel + Environment.NewLine +
                    "   SQL Edition:  " + edition + Environment.NewLine, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //End TT#1205 - Verify version of sql server during install - APicchetti - 3/18/2011

            //raise the event
            Config_SqlClick(this, new EventArgs());
        }

        //configuration directory property
        public string ConfigurationSql
        {
            get
            {
                ConfigSql = lblConfig_Sql.Text.Trim();
                return ConfigSql;
            }
            set
            {
                lblConfig_Sql.Text = value;
                ConfigSql = lblConfig_Sql.Text.Trim();
            }
        }

        //control label
        public string ConfigSqlLabel
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
        private void Config_Sql_Resize(object sender, EventArgs e)
        {
            lblConfig_Sql.Width = this.Width - btnConfig_Sql.Width - 5;
        }

        //raise the click event
        private void lblConfig_Sql_Click(object sender, EventArgs e)
        {
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_SqlClick(this, new EventArgs());
        }

        //raise the click event
        private void Config_Sql_Click(object sender, EventArgs e)
        {
            Config_SqlClick(this, new EventArgs());
        }

        private void lblLabel_Click(object sender, EventArgs e)
        {
            lblConfig_Sql.Select();
            lblLabelBold.Visible = true;
            lblLabel.Visible = false;
            Config_SqlClick(this, new EventArgs());
        }

        private void Config_Sql_LostFocus(object sender, EventArgs e)
        {
            lblLabelBold.Visible = false;
            lblLabel.Visible = true;
        }

        private void Config_Sql_GotFocus(object sender, EventArgs e)
        {
            lblConfig_Sql.Select();
        }

        private void lblConfig_Sql_TextChanged(object sender, EventArgs e)
        {
            if (blInit == true)
            {
                Config_Sql_Edited(this, new EventArgs());
            }
        }

        override public void SetTooltipDescription(string aDescription)
        {
            SetTooltipControl(lblLabel, lblLabel.Text + " (" + aDescription + ")");     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
            SetTooltipControl(lblLabelBold, lblLabelBold + " (" + aDescription + ")");      //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/17/2011
        }
        
     }
}
