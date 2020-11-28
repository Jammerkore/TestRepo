using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.IO;

namespace MIDRetailInstaller
{
    public partial class ucConfig : UserControl
    {
        ToolTip tt = new ToolTip();

        //events
        public event EventHandler EditModeInitiated;

        //passed objects
        InstallerFrame frame;
        string installed_component = "";
        eConfigType configType = eConfigType.None;
        ucInstallationLog log;

        //installer dataset
        DataSet dsInstallerInfo = new DataSet();

        //application path
        string strPath = "";

        string ConfigurationFileName;

        //initiate applications to configure array
        //List<string> lstAppsToConfig;
        SortedList lstAppsToConfig;

        //array of configuration panels
        ArrayList alPanels = new ArrayList();
        Panel pnlSettings1;

        //flag to tell other operations that the application in is saving mode
        bool blSaving = false;

        //global edit count
        int intEdits = 0;

        int cPanelWidth = 0;

        public ucConfig(InstallerFrame p_frame, string p_installed_component, eConfigType p_configType, 
            List<string> p_lstAppsToConfig, ucInstallationLog p_log)
        {
            try
            {
                //assign passed objects
                frame = p_frame;
                frame.help_ID = "configure";
                installed_component = p_installed_component;
                configType = p_configType;
                frame.SetStatusMessage("Building Configuration Information");
                Cursor = Cursors.WaitCursor;
                lstAppsToConfig = new SortedList();
                foreach (string AppToConfig in p_lstAppsToConfig)
                {
                    lstAppsToConfig.Add(AppToConfig, AppToConfig);
                }
                //lstAppsToConfig = p_lstAppsToConfig;
                installed_component = (string)lstAppsToConfig.GetKey(0);
                log = p_log;
                ConfigurationFileName = ConfigurationManager.AppSettings["MIDSettings_config"].ToString();

                InitializeComponent();

                //initialize the installer dataset
                dsInstallerInfo.ReadXmlSchema(Application.StartupPath + @"\installer.xsd");
                dsInstallerInfo.ReadXml(Application.StartupPath + @"\installer.xml");

                //initialize panels
                CreatePanels();
                pnlSettings1 = (Panel)alPanels[0];

                //resize the panel
                pnlSettings1.Top = 2;
                pnlSettings1.Left = 2;
                pnlSettings1.Height = splitContainer2.Panel1.Height - 4;
                pnlSettings1.Width = splitContainer2.Panel1.Width - 4;

                cPanelWidth = pnlSettings1.Width;

                tt.SetToolTip(label1, frame.GetToolTipText("config_folder"));
                tt.SetToolTip(cboApplication, frame.GetToolTipText("config_folder"));
                tt.SetToolTip(lstConfig, frame.GetToolTipText("config_configlist"));
            }
            catch
            {
                throw;
            }
            finally
            {
                frame.SetStatusMessage("Modify Configuration Information");
                Cursor = Cursors.Default;
            }
        }

        private void pnlSettings_Resize(object sender, EventArgs e)
        {
            //resize the border rect
            rectangleShape1.Top = 0;
            rectangleShape1.Left = 0;
            rectangleShape1.Width = pnlSettings1.Width + 3;
            rectangleShape1.Height = pnlSettings1.Height + 3;
        }

        private void ucConfig_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                frame.ProgressBarSetValue(0);

                //resize the controls
                splitContainer2.Panel1.BackColor = Color.White;
                rectangleShape1.Top = 0;
                rectangleShape1.Left = 0;
                pnlSettings1.Top = 2;
                pnlSettings1.Left = 2;
                pnlSettings1.Height = splitContainer2.Panel1.Height - 4;
                pnlSettings1.Width = splitContainer2.Panel1.Width - 4;
                rectangleShape1.BringToFront();
                rectangleShape1.Width = pnlSettings1.Width + 3;
                rectangleShape1.Height = pnlSettings1.Height + 3;

                // adjust for larger fonts
                if (label1.Right > cboApplication.Left)
                {
                    label1.Left -= label1.Right - cboApplication.Left + 5;
                }

                //clear the frame messages and status
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = "";
                frame.SetStatusMessage("");
				// End TT#1668
                if (!frame.blPerformingOneClickUpgrade &&
                    !frame.blPerformingTypicalInstall)
                {
                    frame.ProgressBarSetValue(0);
                }

                if (configType == eConfigType.Client)
                {
                    //clear the list
                    lstConfig.Items.Clear();

                    //get the directories to search for config files
                    string client_dir = Directory.GetParent(installed_component).ToString().Trim();
                    string app_dir = Directory.GetParent(client_dir).ToString().Trim();

                    //show the config file parent dir
                    cboApplication.Text = client_dir.Trim() + @"\";

                    //set the pick list for the applications combo box
                    foreach (string AppToConfig in lstAppsToConfig.Keys)
                    {
                        cboApplication.Items.Add(AppToConfig);
                    }

                    //get the list of application configs
                    SwitchApplications(app_dir, client_dir);
                }
                else
                {
                    //clear the list
                    lstConfig.Items.Clear();

                    //get the directories to search for config files
                    string app_dir;
                    if (installed_component.EndsWith(".exe") ||
                        installed_component.EndsWith(".config"))
                    {
                        app_dir = Directory.GetParent(installed_component).ToString().Trim();
                    }
                    else
                    {
                        app_dir = installed_component.Trim();
                    }

                    //show the config file parent dir
                    if (!app_dir.Trim().EndsWith(@"\"))
                    {
                        cboApplication.Text = app_dir.Trim() + @"\";
                    }
                    else
                    {
                        cboApplication.Text = app_dir.Trim();
                    }

                    //set the pick list for the applications combo box
                    foreach (string AppToConfig in lstAppsToConfig.Keys)
                    {
                        cboApplication.Items.Add(AppToConfig);
                    }

                    //get the list of application configs
                    SwitchApplications(app_dir, false);
                }

                //initialize the description controls
                if (pnlSettings1.Controls.Count > 0)
                {
                    lblKey_Click((object)pnlSettings1.Controls[0], e);
                }

                //put focus on the first setting
                if (pnlSettings1.Controls.Count > 0)
                {
                    pnlSettings1.Controls[0].Focus();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string GetDescription(string ID)
        {
            //variable for description
            string strDescription = "";

            //get the description datatable
            DataTable dtDesc = dsInstallerInfo.Tables[InstallerConstants.cTable_Description];

            //select the correct description
            DataRow[] rows = dtDesc.Select("id = '" + ID + "'");

            //set the description
            if (rows.Length > 0)
            {
                strDescription = rows[0].Field<string>("text").Trim();
            }

            //return the description
            return strDescription;
        }

        private string[] GetList(string ID)
        {
            string[] items = null;

            //get the description datatable
            DataTable dtDesc = dsInstallerInfo.Tables[InstallerConstants.cTable_Description];

            //select the correct description
            DataRow[] rows = dtDesc.Select("id = '" + ID + "'");

            //set the description
            if (rows.Length > 0)
            {
                items = rows[0].Field<string>(InstallerConstants.cControlType_List).Trim().Split('|');
            }

            //return the description
            return items;
        }

        private void lstConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            //remove the old control, to be replaced by the new control w/ changes
            foreach (Control ctrl in splitContainer2.Panel1.Controls)
            {
                if (ctrl.GetType().Name == "Panel")
                {
                    splitContainer2.Panel1.Controls.Remove(ctrl);
                }
            }

            for (int i = 0; i < alPanels.Count; i++)
            {
                //initialize the active object
                Panel pnlSettings = (Panel)alPanels[i];

                /* Begin TT#1193 - Blank config screen for Batch APIs after upgrading server - APicchetti - 3/17/2011 */
                string strApplicationDir = "";
                if (cboApplication.Text.Trim().EndsWith(@"\") != true)
                {
                    strApplicationDir = cboApplication.Text.Trim() + @"\";
                }
                else
                {
                    strApplicationDir = cboApplication.Text.Trim();
                }
                /* End TT#1193 - Blank config screen for Batch APIs after upgrading server - APicchetti - 3/17/2011 */

                if (((InstallerConfigTag)pnlSettings.Tag).FilePath == strApplicationDir + lstConfig.SelectedItem.ToString().Trim())
                {
                    //remove the panel
                    alPanels.RemoveAt(i);

                    //add the panel to the parent panel
                    splitContainer2.Panel1.Controls.Add(pnlSettings);
                    pnlSettings.BringToFront();

                    //initialize the description controls
                    if (pnlSettings.Controls.Count > 0)
                    {
                        lblKey_Click((object)pnlSettings.Controls[0], new EventArgs());
                    }

                    //put focus on the first setting
                    if (pnlSettings.Controls.Count > 0)
                    {
                        pnlSettings.Controls[0].Focus();
                    }

                    //put the panel back
                    alPanels.Add(pnlSettings);
                    pnlSettings1 = pnlSettings;

                    //resize the panel
                    pnlSettings1.Top = 2;
                    pnlSettings1.Left = 2;
                    pnlSettings1.Height = splitContainer2.Panel1.Height - 4;
                    pnlSettings1.Width = splitContainer2.Panel1.Width - 4;

                    break;
                }
            }
        }

        private void CountEdits(object sender, EventArgs e)
        {
            //track the edits
            intEdits++;
            ((InstallerConfigValueTag)((Control)sender).Tag).ValueChangeType = eConfigChangeType.Changed;
            ((InstallerControl)sender).displayFromConfig();
            //initiate edit mode
            EditModeInitiated(this, new EventArgs());
        }

        // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
        private void CountEdits(object sender)
        {
            //track the edits
            intEdits++;
            ((InstallerConfigValueTag)((Control)sender).Tag).ValueChangeType = eConfigChangeType.Changed;
            ((InstallerControl)sender).displayFromConfig();
        }
        // End TT#2261

        //publisize the edit count (read-only)
        public int EditCount
        {
            get
            {
                return intEdits;
            }
        }

        private void CreatePanels()
        {
            Cursor = Cursors.WaitCursor;
			// Begin TT#1668 - JSmith - Install Log
			//ConfigFiles configFiles = new ConfigFiles(dsInstallerInfo, log);
            ConfigFiles configFiles = new ConfigFiles(frame, dsInstallerInfo, log);
			// End TT#1668
            XmlDocument config = null;
            XmlDocument MIDSettings = null;
            bool blErrorCreatingPanels = false;
            List<string> p_list;
            // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
            int value;
            // End TT#2261

            foreach (string application in lstAppsToConfig.Keys)
            {
                //get a list of the config files in the application directory
                List<string> lstConfigs = null;
                if (application.EndsWith(".exe") == false)
                {
                    string[] strConfigs = Directory.GetFiles(application, "*.config");
                    lstConfigs = strConfigs.ToList<string>();
                }
                else
                {
                    lstConfigs = new List<string>();
                    lstConfigs.Add(application + ".config");
                }

                foreach (string strConfig in lstConfigs)
                {
                    //create the panel object
                    Panel pnlSettings = new Panel();
                    //config = configFiles.GetXmlDocument(strConfig);
                    // Begin TT#1819 - JSmith - Installer configuration failing with corrupt file
                    try
                    {
                        config = configFiles.GetXmlDocument(strConfig);
                    }
                    catch (Exception ex)
                    {
                        string msg = "File name: <" + strConfig +
                              "> is corrupt and is excluded because " + ex.Message;
                        MessageBox.Show(msg, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.AddLogEntry(msg, eErrorType.error);
                        continue;
                    }
                    // End TT#1819
                    MIDSettings = configFiles.GetMIDSettings(config);
                    pnlSettings.AutoScroll = true;

                    //clear the description controls
                    lblDescription.Text = "";
                    txtDescription.Text = "";


                    //get the path from the label
                    strPath = strConfig;
                    char[] delim = @"\".ToCharArray();
                    string[] strConfigFileParts = strPath.Split(delim);
                    string strConfigFile = strConfigFileParts[strConfigFileParts.GetUpperBound(0)].ToString().Trim();

                    pnlSettings.Tag = new InstallerConfigTag(strConfig, strConfigFile, config, MIDSettings);

                    #region control_creation
                    //loop thru the table and add controls to the panel
                    //get the configuration table
                    DataTable dtConfiguration = dsInstallerInfo.Tables[InstallerConstants.cTable_Configuration];

                    DataRow[] drConfigRows = dtConfiguration.Select("config_file='" + strConfigFile + "' or config_file='ALL'", "setting ASC");

                    int control_top = 2;
                    eConfigValueFrom from;
                    //for (int intSetting = 0; intSetting < dtAppSettings.Rows.Count; intSetting++)
                    foreach (DataRow dr in drConfigRows)
                    {
                        //label control
                        string strKeyText = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();

                        if (strConfig.Contains("Job")
                            && strKeyText.Contains("value"))
                        {
                            bool stop = true;
                        }

                        //edit control
                        string control_type = "";
                        List<string> pick_list;
                        string description = "";
                        string setting = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                        if (!GetControl(dr, strConfigFile, setting, out control_type, out pick_list, out description))
                        {
                            blErrorCreatingPanels = true;
                        }

                        switch (control_type)
                        {
                            case InstallerConstants.cControlType_Boolean:

                                //create the needed control
                                Config_Combo cboValue = new Config_Combo(frame);
                                cboValue.Config_Combo_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                //cboValue.Config_Combo_Text = dr.Field<string>("value").Trim();
                                cboValue.Config_Combo_Text = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                cboValue.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                cboValue.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                cboValue.Left = 0;
                                cboValue.Top = control_top;
                                cboValue.Config_Combo_Click += new EventHandler(this.lblKey_Click);
                                cboValue.Config_Combo_Edited += new EventHandler(this.CountEdits);
                                //cboValue.Tag = strKeyText + "|" + description;
                                cboValue.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, cboValue.Config_Combo_Text);

                                //fill the pick list
                                p_list = new List<string>();
                                foreach (string pick in pick_list)
                                {
                                    p_list.Add(pick);
                                }
                                cboValue.Config_Combo_List = p_list;

                                //initialize the control
                                cboValue.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(cboValue);

                                break;

                            case InstallerConstants.cControlType_WindowStyle:
                            case InstallerConstants.cControlType_List:

                                //create the needed control
                                Config_Combo cboStyle = new Config_Combo(frame);
                                cboStyle.Config_Combo_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                cboStyle.Config_Combo_Text = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                cboStyle.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                cboStyle.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                cboStyle.Left = 0;
                                cboStyle.Top = control_top;
                                cboStyle.Config_Combo_Click += new EventHandler(this.lblKey_Click);
                                cboStyle.Config_Combo_Edited += new EventHandler(this.CountEdits);
                                //cboStyle.Tag = strKeyText + "|" + description;
                                cboStyle.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, cboStyle.Config_Combo_Text);

                                //fill the pick list
                                p_list = new List<string>();
                                foreach (string pick in pick_list)
                                {
                                    p_list.Add(pick);
                                }
                                cboStyle.Config_Combo_List = p_list;

                                //initialize the control
                                cboStyle.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(cboStyle);

                                break;

                            case InstallerConstants.cControlType_Directory:

                                //create the directory control
                                Config_Directory dir = new Config_Directory(frame);
                                dir.ConfigDirLabel = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                dir.ConfigurationDirectory = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                dir.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                dir.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                //dir.Tag = strKeyText + "|" + description;
                                dir.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, dir.ConfigurationDirectory);
                                dir.Left = 0;
                                dir.Top = control_top;
                                dir.Config_Dir_Click += new EventHandler(this.lblKey_Click);
                                dir.Config_Dir_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                dir.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(dir);

                                break;

                            case InstallerConstants.cControlType_OpenFile:

                                //create the directory control
                                Config_OpenFile cFile = new Config_OpenFile(frame);
                                cFile.ConfigFileLabel = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                cFile.ConfigurationFile = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                cFile.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                cFile.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                //cFile.Tag = strKeyText + "|" + description;
                                cFile.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, cFile.ConfigurationFile);
                                cFile.Left = 0;
                                cFile.Top = control_top;
                                cFile.Config_File_Click += new EventHandler(this.lblKey_Click);
                                cFile.Config_File_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                cFile.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(cFile);

                                break;

                            case InstallerConstants.cControlType_SQLConnect:

                                //create the directory control
                                Config_Sql cSql = new Config_Sql(frame);
                                cSql.ConfigSqlLabel = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                cSql.ConfigurationSql = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                cSql.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                cSql.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                //cSql.Tag = strKeyText + "|" + description;
                                cSql.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, cSql.ConfigurationSql);
                                cSql.Left = 0;
                                cSql.Top = control_top;
                                cSql.Config_SqlClick += new EventHandler(this.lblKey_Click);
                                cSql.Config_Sql_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                cSql.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(cSql);

                                break;

                            case InstallerConstants.cControlType_Thousands:

                                //create the needed control
                                Config_Numeric nmThousands = new Config_Numeric(frame);
                                nmThousands.Config_Numeric_Minimum = 0;
                                nmThousands.Config_Numeric_Maximum = 9000000;
                                nmThousands.Config_Numeric_Increment = 1000;
                                nmThousands.Config_Numeric_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                //nmThousands.Config_Numeric_Value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                nmThousands.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                nmThousands.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                // End TT#2261
                                nmThousands.Left = 0;
                                nmThousands.Top = control_top;
                                //nmThousands.Tag = strKeyText + "|" + description;
                                nmThousands.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, nmThousands.Config_Numeric_Value.ToString());
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                SetNumericValue(strConfig, nmThousands, value);
                                // End TT#2261
                                nmThousands.Config_Numeric_Click += new EventHandler(this.lblKey_Click);
                                nmThousands.Config_Numeric_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                nmThousands.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(nmThousands);

                                break;

                            case InstallerConstants.cControlType_Hundreds:

                                //create the needed control
                                Config_Numeric nmHundreds = new Config_Numeric(frame);
                                nmHundreds.Config_Numeric_Minimum = 0;
                                nmHundreds.Config_Numeric_Maximum = 90000;
                                nmHundreds.Config_Numeric_Increment = 100;
                                nmHundreds.Config_Numeric_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                //nmHundreds.Config_Numeric_Value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                nmHundreds.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                nmHundreds.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                // End TT#2261
                                nmHundreds.Left = 0;
                                nmHundreds.Top = control_top;
                                //nmHundreds.Tag = strKeyText + "|" + description;
                                nmHundreds.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, nmHundreds.Config_Numeric_Value.ToString());
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                SetNumericValue(strConfig, nmHundreds, value);
                                // End TT#2261
                                nmHundreds.Config_Numeric_Click += new EventHandler(this.lblKey_Click);
                                nmHundreds.Config_Numeric_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                nmHundreds.Initialized = true;
                                
                                //add the control to the panel
                                pnlSettings.Controls.Add(nmHundreds);

                                break;

                            case InstallerConstants.cControlType_Tens:

                                //create the needed control
                                Config_Numeric nmTens = new Config_Numeric(frame);
                                nmTens.Config_Numeric_Minimum = 0;
                                nmTens.Config_Numeric_Maximum = 9000;
                                nmTens.Config_Numeric_Increment = 10;
                                nmTens.Config_Numeric_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                //nmTens.Config_Numeric_Value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                nmTens.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                nmTens.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                // End TT#2261
                                nmTens.Left = 0;
                                nmTens.Top = control_top;
                                //nmTens.Tag = strKeyText + "|" + description;
                                nmTens.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, nmTens.Config_Numeric_Value.ToString());
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                SetNumericValue(strConfig, nmTens, value);
                                // End TT#2261
                                nmTens.Config_Numeric_Click += new EventHandler(this.lblKey_Click);
                                nmTens.Config_Numeric_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                nmTens.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(nmTens);

                                break;

                            case InstallerConstants.cControlType_Numeric:

                                //create the needed control
                                Config_Numeric nmNumeric = new Config_Numeric(frame);
                                nmNumeric.Config_Numeric_Minimum = 0;
                                nmNumeric.Config_Numeric_Maximum = 5000;
                                nmNumeric.Config_Numeric_Increment = 1;
                                nmNumeric.Config_Numeric_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                //nmNumeric.Config_Numeric_Value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                value = Convert.ToInt32(configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from));
                                nmNumeric.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                nmNumeric.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                // End TT#2261
                                nmNumeric.Left = 0;
                                nmNumeric.Top = control_top;
                                //nmNumeric.Tag = strKeyText + "|" + description;
                                nmNumeric.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, nmNumeric.Config_Numeric_Value.ToString());
                                // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
                                SetNumericValue(strConfig, nmNumeric, value);
                                // End TT#2261
                                nmNumeric.Config_Numeric_Click += new EventHandler(this.lblKey_Click);
                                nmNumeric.Config_Numeric_Edited += new EventHandler(this.CountEdits);

                                //intialize the control
                                nmNumeric.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(nmNumeric);

                                break;

                            case InstallerConstants.cControlType_Password:

                                //create the needed control
                                Config_Password txtPwd = new Config_Password(frame);
                                txtPwd.Config_Password_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                txtPwd.Config_Password_Text = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                txtPwd.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                txtPwd.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                txtPwd.Left = 0;
                                txtPwd.Top = control_top;
                                //txtPwd.Tag = strKeyText + "|" + description;
                                txtPwd.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, txtPwd.Config_Password_Text);
                                txtPwd.Config_Password_Click += new EventHandler(this.lblKey_Click);
                                txtPwd.Config_Password_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                txtPwd.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(txtPwd);

                                break;

                            default:

                                //create the needed control
                                Config_Text txtValue = new Config_Text(frame);
                                txtValue.Config_Text_Label = dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim();
                                txtValue.Config_Text_Text = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), config, MIDSettings, dr, out from);
                                txtValue.Parent = dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim();
                                txtValue.LookupType = dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim();
                                txtValue.Left = 0;
                                txtValue.Top = control_top;
                                //txtValue.Tag = strKeyText + "|" + description;
                                txtValue.Tag = new InstallerConfigValueTag(strKeyText + "|" + description, from, txtValue.Config_Text_Text);
                                txtValue.Config_Text_Click += new EventHandler(this.lblKey_Click);
                                txtValue.Config_Text_Edited += new EventHandler(this.CountEdits);

                                //initialize the control
                                txtValue.Initialized = true;

                                //add the control to the panel
                                pnlSettings.Controls.Add(txtValue);

                                break;
                        }
                        //iterate the control's top placement
                        control_top += 25;
                    }
                    #endregion

                    // must set anchor of controls after added to panel because setting them during creating causes resize problems.
                    foreach (Control control in pnlSettings.Controls)
                    {
                        control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                                                    | System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));

                        switch (((InstallerConfigValueTag)control.Tag).ValueFrom)
                        {
                            case eConfigValueFrom.MIDSettings:
                                ((InstallerControl)control).displayFromMIDSettings();
                                break;
                            case eConfigValueFrom.Default:
                                ((InstallerControl)control).displayFromDefault();
                                break;
                            default:
                                ((InstallerControl)control).displayFromConfig();
                                break;
                        }

                        string description = ((InstallerConfigValueTag)control.Tag).Description;
                        description = description.Substring(description.IndexOf('|') + 1);
                        ((InstallerControl)control).SetTooltipDescription(description);

                    }

                    pnlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                    //add the panel to the control
                    alPanels.Add(pnlSettings);
                }
            }

            if (blErrorCreatingPanels)
            {
                MessageBox.Show("Error loading configuration information.  Review log messages.", "Configuration file error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            Cursor = Cursors.Default;
        }

        // Begin TT#2261 - JSmith - Installer needs to override values not within range of data
        private void SetNumericValue(string strConfig, Config_Numeric aConfig_Numeric, int aValue)
        {

            if (aValue < aConfig_Numeric.Config_Numeric_Minimum)
            {
                string msg = frame.GetText("minimumOverride");
                msg = msg.Replace("{0}", aValue.ToString());
                msg = msg.Replace("{1}", aConfig_Numeric.Config_Numeric_Label);
                msg = msg.Replace("{2}", strConfig);
                msg = msg.Replace("{3}", aConfig_Numeric.Config_Numeric_Minimum.ToString());
                frame.SetLogMessage(msg, eErrorType.warning);
                aConfig_Numeric.Config_Numeric_Value = aConfig_Numeric.Config_Numeric_Minimum;
                CountEdits(aConfig_Numeric);
                MessageBox.Show(msg, "Value Override", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (aValue > aConfig_Numeric.Config_Numeric_Maximum)
            {
                string msg = frame.GetText("maximumOverride");
                msg = msg.Replace("{0}", aValue.ToString());
                msg = msg.Replace("{1}", aConfig_Numeric.Config_Numeric_Label);
                msg = msg.Replace("{2}", strConfig);
                msg = msg.Replace("{3}", aConfig_Numeric.Config_Numeric_Maximum.ToString());
                frame.SetLogMessage(msg, eErrorType.warning);
                aConfig_Numeric.Config_Numeric_Value = aConfig_Numeric.Config_Numeric_Maximum;
                CountEdits(aConfig_Numeric);
                MessageBox.Show(msg, "Value Override", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                aConfig_Numeric.Config_Numeric_Value = aValue;
            }

        }
        // End TT#2261

        private bool GetControl(DataRow drConfigRow, string configfile, string setting, out string control_type, 
            out List<string> pick_list, out string description)
        {
            //initialize the out parms
            control_type = "";
            description = "";
            pick_list = new List<string>();

            try
            {
                //get the configuration table
                //DataTable dtConfiguration = dsInstallerInfo.Tables[InstallerConstants.cTable_Configuration];

                //get the application name from the path
                char[] delim = @"\".ToCharArray();
                string[] strAppFileParts = strPath.Split(delim);
                string strApplication = strAppFileParts[strAppFileParts.GetUpperBound(0) - 1].ToString().Trim();

                if (strApplication == "MIDRetail_Client (Auto-Upgrade)" || 
                    strApplication == "MIDRetail_Client64" || 
                    strApplication == "MIDRetail_Client64 (Auto-Upgrade)" ||
                    strApplication == "Client")
                {
                    strApplication = "MIDRetail_Client";

                    if (strAppFileParts[strAppFileParts.GetUpperBound(0) - 1].ToString().Trim() == "Client")
                    {
                        configfile = @"Client\" + configfile;
                    }
                }

                //get the control type to load
                control_type = drConfigRow.Field<string>(InstallerConstants.cSettingValue_Type).Trim();

                //get the id of the control to load
                string control_id = drConfigRow.Field<string>(InstallerConstants.cConfigurationField_ID).Trim();
                description = GetDescription(control_id);

                //define the pick list (if needed) for the control to load
                pick_list = new List<string>();
                if (control_type == InstallerConstants.cControlType_Boolean)
                {
                    //get the setting values
                    DataTable dtBoolean = dsInstallerInfo.Tables[InstallerConstants.cTable_Boolean];

                    //loop thru the settings and add them to the list
                    for (int intSetting = 0; intSetting < dtBoolean.Rows.Count; intSetting++)
                    {
                        pick_list.Add(dtBoolean.Rows[intSetting].Field<string>(InstallerConstants.cConfigurationField_Setting));
                    }
                }

                if (control_type == InstallerConstants.cControlType_WindowStyle)
                {
                    //get the setting values
                    DataTable dtStyle = dsInstallerInfo.Tables[InstallerConstants.cTable_WindowStyle];

                    //loop thru the settings and add them to the list
                    for (int intSetting = 0; intSetting < dtStyle.Rows.Count; intSetting++)
                    {
                        pick_list.Add(dtStyle.Rows[intSetting].Field<string>(InstallerConstants.cConfigurationField_Setting));
                    }
                }

                if (control_type == InstallerConstants.cControlType_List)
                {
                    string[] items = GetList(control_id);

                    //loop thru the settings and add them to the list
                    foreach (string item in items)
                    {
                        pick_list.Add(item);
                    }
                }
            }
            catch (Exception err)
            {
                //log the error message
                log.AddLogEntry("Error: " + err.Message, eErrorType.error);
                return false;

                ////give the user a message
                //MessageBox.Show(err.Message, "Configuration file error", MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);
            }

            return true;
        }

        private void GetControl(DataRow drConfigRow, string configfile, string setting, out string control_type,
            out List<string> pick_list, out string description, out string strDefault)
        {
            //initialize the out parms
            control_type = "";
            description = "";
            pick_list = new List<string>();
            strDefault = "";

            try
            {
                //get the configuration table
                DataTable dtConfiguration = dsInstallerInfo.Tables[InstallerConstants.cTable_Configuration];

                //get the application name from the path
                char[] delim = @"\".ToCharArray();
                string[] strAppFileParts = strPath.Split(delim);
                string strApplication = strAppFileParts[strAppFileParts.GetUpperBound(0) - 1].ToString().Trim();

                if (strApplication == "MIDRetail_Client (Auto-Upgrade)" ||
                    strApplication == "MIDRetail_Client64" ||
                    strApplication == "MIDRetail_Client64 (Auto-Upgrade)" ||
                    strApplication == "Client")
                {
                    strApplication = "MIDRetail_Client";
                }

                //get the control type to load
                control_type = drConfigRow.Field<string>(InstallerConstants.cSettingValue_Type).Trim();

                //get the default value
                strDefault = drConfigRow.Field<string>(InstallerConstants.cSettingValue_DefaultValue).Trim();

                //get the id of the control to load
                string control_id = drConfigRow.Field<string>(InstallerConstants.cConfigurationField_ID).Trim();
                description = GetDescription(control_id);

                //define the pick list (if needed) for the control to load
                pick_list = new List<string>();
                if (control_type == InstallerConstants.cControlType_Boolean)
                {
                    //get the setting values
                    DataTable dtBoolean = dsInstallerInfo.Tables[InstallerConstants.cTable_Boolean];

                    //loop thru the settings and add them to the list
                    for (int intSetting = 0; intSetting < dtBoolean.Rows.Count; intSetting++)
                    {
                        pick_list.Add(dtBoolean.Rows[intSetting].Field<string>(InstallerConstants.cConfigurationField_Setting));
                    }
                }

                if (control_type == InstallerConstants.cControlType_WindowStyle)
                {
                    //get the setting values
                    DataTable dtStyle = dsInstallerInfo.Tables[InstallerConstants.cTable_WindowStyle];

                    //loop thru the settings and add them to the list
                    for (int intSetting = 0; intSetting < dtStyle.Rows.Count; intSetting++)
                    {
                        pick_list.Add(dtStyle.Rows[intSetting].Field<string>(InstallerConstants.cConfigurationField_Setting));
                    }
                }
            }
            catch (Exception err)
            {
                //log the error message
                log.AddLogEntry("Error: " + err.Message, eErrorType.error);

                //give the user a message
                MessageBox.Show(err.Message, "Configuration file error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void lblKey_Click(object sender, EventArgs e)
        {
            //cast the control
            Control clicked_control = (Control)sender;

            //split the 2 parts of the control tag
            char[] delim = "|".ToCharArray();
            string[] Tag_Parts = ((InstallerConfigValueTag)clicked_control.Tag).Description.Split(delim);

            //set the text of the description controls
            lblDescription.Text = Tag_Parts[0].ToString().Trim();
            //txtDescription.Text = Environment.NewLine + Tag_Parts[1].ToString().Trim();
            txtDescription.Text = Tag_Parts[1].ToString().Trim();

            // Begin TT#1972 - JSmith - Need to display the currently configured database connection information in the Installer Configure area
            if (clicked_control is MIDRetailInstaller.Config_Sql)
            {
                Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                try
                {
                    connProp.ConnectionStringBuilder.ConnectionString = ((Config_Sql)clicked_control).ConfigurationSql;
                    connProp.ConnectionStringBuilder[InstallerConstants.cControlType_Password] = "********";
                    txtDescription.Text += Environment.NewLine + "Value:" + connProp.ConnectionStringBuilder.ConnectionString;
                }
                catch
                {
                    txtDescription.Text += Environment.NewLine + "Value: Connection properties are invalid!" ;
                }
            }
            // End TT#1972
        }

        private void cboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////if the config file is the client...
            //if (installed_component.Contains(@"\Client\") == true)
            //{
            //    //switch to the client application
            //    SwitchApplications(cboApplication.Text.Trim(),cboApplication.Text.Trim() + @"\Client\");
            //}

            ////if the config file in a service...
            //else
            //{
            //    //switch to the server application
            //    SwitchApplications(cboApplication.SelectedItem.ToString().Trim(), false);
            //}

            //switch to the server application
            SwitchApplications(cboApplication.SelectedItem.ToString().Trim(), false);
        }

        private void SwitchApplications(string ApplicationDirectory, string ClientDirectory)
        {
            //clear the list
            lstConfig.Items.Clear();

            //get the files in the directories
            string[] client_files = Directory.GetFiles(ClientDirectory);
            string[] application_files = Directory.GetFiles(ApplicationDirectory);

            //loop thru the files and search for configs
            foreach (string client_file in client_files)
            {
                if (client_file.EndsWith(".config") == true)
                {
                    //add the config file to the list
                    //lstConfig.Items.Add(@"Client\" + Path.GetFileName(client_file));
                    lstConfig.Items.Add(Path.GetFileName(client_file));
                }
            }

            //loop thru the files in the app dir and search for configs
            foreach (string application_file in application_files)
            {
                if (application_file.EndsWith(".config") == true)
                {
                    lstConfig.Items.Add(Path.GetFileName(application_file));
                }
            }

            //select the first config file in the list
            lstConfig.SelectedIndex = 0;

            //re-assign active panel
            foreach (Panel pnlSettings in alPanels)
            {
                System.Diagnostics.Debug.Print(((InstallerConfigTag)pnlSettings.Tag).FilePath);

                if (cboApplication.Text.Trim() + lstConfig.SelectedItem.ToString().Trim() == ((InstallerConfigTag)pnlSettings.Tag).FilePath)
                {
                    pnlSettings1 = pnlSettings;
                    break;
                }
            }

            //initialize the description controls
            if (blSaving == false)
            {
                pnlSettings1.Controls[0].Select();
                char[] delim = "|".ToCharArray();
                string[] strDescripParts = ((InstallerConfigValueTag)pnlSettings1.Controls[0].Tag).Description.Split(delim);
                lblDescription.Text = strDescripParts[0].Trim();
                txtDescription.Text = Environment.NewLine + strDescripParts[1].Trim();
            }
        }

        private void SwitchApplications(string ApplicationDirectory, bool isClientfolder)
        {
            //clear the list
            lstConfig.Items.Clear();

            //get the files in the directories
            string[] app_files = Directory.GetFiles(ApplicationDirectory);

            //loop thru the files and search for configs
            foreach (string app_file in app_files)
            {
                if (app_file.EndsWith(".config") == true)
                {
                    //add the config file to the list
                    lstConfig.Items.Add(Path.GetFileName(app_file));
                }
            }

            //if the client is the app dir drill into the client folder
            if (isClientfolder)
            {
                app_files = Directory.GetFiles(ApplicationDirectory + @"\Client");

                //loop thru the files and search for configs
                foreach (string app_file in app_files)
                {
                    if (app_file.EndsWith(".config") == true)
                    {
                        //add the config file to the list
                        lstConfig.Items.Add(@"Client\" + Path.GetFileName(app_file));
                    }
                }
            }

            //select the first config file in the list
            lstConfig.SelectedIndex = 0;

            //re-assign active panel
            foreach (Panel pnlSettings in alPanels)
            {
                System.Diagnostics.Debug.Print(((InstallerConfigTag)pnlSettings.Tag).FilePath);
                if (cboApplication.Text.Trim() + lstConfig.SelectedItem.ToString().Trim() == ((InstallerConfigTag)pnlSettings.Tag).FilePath)
                {
                    pnlSettings1 = pnlSettings;
                    break;
                }
            }

            //initialize the description controls
            if (blSaving == false)
            {
                pnlSettings1.Controls[0].Select();
                char[] delim = "|".ToCharArray();
                string[] strDescripParts = ((InstallerConfigValueTag)pnlSettings1.Controls[0].Tag).Description.Split(delim);
                lblDescription.Text = strDescripParts[0].Trim();
                txtDescription.Text = Environment.NewLine + strDescripParts[1].Trim();
            }
        }
        
        public void SaveConfigChanges()
        {
            InstallerConfigValueTag valueTag;
            //set the saving mode flag to true
            bool blSave = false;

            // Begin TT#1668 - JSmith - Install Log
			//ConfigFiles configFiles = new ConfigFiles(dsInstallerInfo, log);
            ConfigFiles configFiles = new ConfigFiles(frame, dsInstallerInfo, log);
			// End TT#1668

            foreach(Panel configApp in alPanels)
            {
                //get the config file full path name
                string configFile = ((InstallerConfigTag)configApp.Tag).FilePath.Trim();
                
                //get the xml document
                XmlDocument doc = configFiles.GetXmlDocument(configFile);


                foreach (Control asControl in configApp.Controls)
                {
                    valueTag = (InstallerConfigValueTag)asControl.Tag;
                    //get key name from control tag
                    char[] delim = "|".ToCharArray();
                    string[] strTagParts = ((InstallerConfigValueTag)asControl.Tag).Description.Split(delim);
                    string strKey = strTagParts[0].ToString().Trim();
                    string strParent = ((InstallerControl)asControl).Parent;
                    string strLookupType = ((InstallerControl)asControl).LookupType;

                    if (valueTag.ValueChangeType == eConfigChangeType.Changed)
                    {
                        blSave = true;
                        
                        string value = string.Empty;

                        switch (asControl.Name)
                        {
                            case "Config_Combo":
                                Config_Combo cboConfig = (Config_Combo)asControl;
                                value = cboConfig.Config_Combo_Text;
                                break;
                            case "Config_Directory":
                                Config_Directory dirConfig = (Config_Directory)asControl;
                                value = dirConfig.ConfigurationDirectory;
                                break;
                            case "Config_OpenFile":
                                Config_OpenFile fileConfig = (Config_OpenFile)asControl;
                                value = fileConfig.ConfigurationFile;
                                break;
                            case "Config_Sql":
                                Config_Sql sqlConfig = (Config_Sql)asControl;
                                value = sqlConfig.ConfigurationSql;
                                break;
                            case "Config_Numeric":
                                Config_Numeric nmConfig = (Config_Numeric)asControl;
                                value = nmConfig.Config_Numeric_Text;
                                break;
                            case "Config_Password":
                                Config_Password pwdConfig = (Config_Password)asControl;
                                value = pwdConfig.Config_Password_Text;
                                break;
                            case "Config_Text":
                                Config_Text txtConfig = (Config_Text)asControl;
                                value = txtConfig.Config_Text_Text;
                                break;
                        }
                        //Begin TT#1821 - DOConnell - Installer Config values cannot be removed or emptied
                        //if (value != null &&
                        //    value.Trim().Length > 0)
                        //{  
						// Begin TT#1668 - JSmith - Install Log
						//configFiles.SetConfigValue(doc, strKey, value);
                        string msg = frame.GetText("ConfigChange");
                        msg = msg.Replace("{0}", strKey);
                        msg = msg.Replace("{1}", configFile);
                        msg = msg.Replace("{2}", valueTag.OriginalValue);
                        msg = msg.Replace("{3}", value);
                        frame.SetLogMessage(msg, eErrorType.message);
                        configFiles.SetConfigValue(null, doc, strParent, strLookupType, strKey, value);
						// End TT#1668
                        //}
                        //End TT#1821 - DOConnell - Installer Config values cannot be removed or emptied
                    }
                    else if (valueTag.ValueChangeType == eConfigChangeType.Remove)
                    {
                        blSave = true;
                        configFiles.DeleteKey(doc, strParent, strLookupType, strKey);
                    }

                }

                //Save the document
                if (blSave)
                {
                    // make sure the file is not read only
                    File.SetAttributes(configFile, File.GetAttributes(configFile) & ~(FileAttributes.ReadOnly));
                    doc.Save(configFile);
                }
            }

            // reset the edit count to zero
            intEdits = 0;
            // Begin TT#4525 - JSmith - Changes in MIDSettings.config not reflected in process specific config files until installer restarted
            // Rebuild panels to incorporate changes from MIDSettings.
            alPanels.Clear();
            CreatePanels();
            // End TT#4525 - JSmith - Changes in MIDSettings.config not reflected in process specific config files until installer restarted
        }

        private void mnuSetting_Undo_Click(object sender, EventArgs e)
        {
            InstallerConfigValueTag valueTag;
            //confirm the removal with the user
            if (MessageBox.Show("Are you sure you would like to undo the selected setting from this configuration?",
                "Configuration Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //formulate the tag value
                string strTagText = lblDescription.Text.Trim() + "|" + txtDescription.Text.Substring(1).Trim();

                //search the controls and undo the one with the matching tag
                foreach (Control setting in pnlSettings1.Controls)
                {
                    valueTag = (InstallerConfigValueTag)setting.Tag;
                    if (valueTag.Description.ToString().Trim() == strTagText)
                    {
                        if (setting is MIDRetailInstaller.Config_Combo)
                        {
                            ((Config_Combo)setting).Config_Combo_Text = valueTag.OriginalValue;
                        }
                        else if (setting is MIDRetailInstaller.Config_Directory)
                        {
                            ((Config_Directory)setting).ConfigurationDirectory = valueTag.OriginalValue;
                        }
                        else if (setting is MIDRetailInstaller.Config_OpenFile)
                        {
                            ((Config_OpenFile)setting).ConfigurationFile = valueTag.OriginalValue;
                        }
                        else if (setting is MIDRetailInstaller.Config_Sql)
                        {
                            ((Config_Sql)setting).ConfigurationSql = valueTag.OriginalValue;
                        }
                        else if (setting is MIDRetailInstaller.Config_Numeric)
                        {
                            ((Config_Numeric)setting).Config_Numeric_Value = Convert.ToInt32(valueTag.OriginalValue);
                        }
                        else if (setting is MIDRetailInstaller.Config_Password)
                        {
                            ((Config_Password)setting).Config_Password_Text = valueTag.OriginalValue;
                        }
                        else if (setting is MIDRetailInstaller.Config_Text)
                        {
                            ((Config_Text)setting).Config_Text_Text = valueTag.OriginalValue;
                        }
                        valueTag.ValueChangeType = eConfigChangeType.None;
                        break;
                    }
                }

                //refresh the panel
                pnlSettings1.Refresh();
            }
        }

        private void mnuSetting_Reset_Click(object sender, EventArgs e)
        {
            InstallerConfigValueTag valueTag;
            InstallerConfigTag configTag;
            string value;
            eConfigValueFrom from;
            DataRow dr = null;
            //confirm the removal with the user
            if (MessageBox.Show("Are you sure you would like to reset the selected setting from this configuration?",
                "Configuration Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
			    // Begin TT#1668 - JSmith - Install Log
				//ConfigFiles configFiles = new ConfigFiles(dsInstallerInfo, log);
                ConfigFiles configFiles = new ConfigFiles(frame, dsInstallerInfo, log);
				// End TT#1668
                configTag = (InstallerConfigTag)pnlSettings1.Tag;

                //formulate the tag value
                // Begin TT#4524 - JSmith - Cannot remove setting from configuration file
                //string strTagText = lblDescription.Text.Trim() + "|" + txtDescription.Text.Substring(1).Trim();
                string strTagText = lblDescription.Text.Trim() + "|" + txtDescription.Text.Trim();
                // End TT#4524 - JSmith - Cannot remove setting from configuration file

                DataTable dtConfiguration = dsInstallerInfo.Tables[InstallerConstants.cTable_Configuration];
                DataRow[] drConfigRows = dtConfiguration.Select("(config_file='" + configTag.FileName + "' or config_file='ALL') and setting = '" + lblDescription.Text.Trim() + "'");
                dr = drConfigRows[0];

                //search the controls and remove the one with the matching tag
                foreach (Control setting in pnlSettings1.Controls)
                {
                    valueTag = (InstallerConfigValueTag)setting.Tag;
                    if (valueTag.Description.ToString().Trim() == strTagText)
                    {
                        value = configFiles.GetConfigValue(dr.Field<string>(InstallerConstants.cConfigurationField_Parent).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_LookupType).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_Setting).Trim(), dr.Field<string>(InstallerConstants.cConfigurationField_ID).Trim(), null, configTag.MIDSettings, dr, out from);
                        if (setting is MIDRetailInstaller.Config_Combo)
                        {
                            ((Config_Combo)setting).Config_Combo_Text = value;
                        }
                        else if (setting is MIDRetailInstaller.Config_Directory)
                        {
                            ((Config_Directory)setting).ConfigurationDirectory = value;
                        }
                        else if (setting is MIDRetailInstaller.Config_OpenFile)
                        {
                            ((Config_OpenFile)setting).ConfigurationFile = value;
                        }
                        else if (setting is MIDRetailInstaller.Config_Sql)
                        {
                            ((Config_Sql)setting).ConfigurationSql = value;
                        }
                        else if (setting is MIDRetailInstaller.Config_Numeric)
                        {
                            ((Config_Numeric)setting).Config_Numeric_Value = Convert.ToInt32(value);
                        }
                        else if (setting is MIDRetailInstaller.Config_Password)
                        {
                            ((Config_Password)setting).Config_Password_Text = value;
                        }
                        else if (setting is MIDRetailInstaller.Config_Text)
                        {
                            ((Config_Text)setting).Config_Text_Text = value;
                        }
                        valueTag.ValueChangeType = eConfigChangeType.Remove;
                        //Begin TT#1822 - DOConnell - when configuring the config settings in the installer, the context menu item "Reset to Global" abends.
                        switch (((InstallerConfigValueTag)setting.Tag).ValueFrom)
                        {
                            case eConfigValueFrom.MIDSettings:
                                ((InstallerControl)setting).displayFromMIDSettings();
                                break;
                            case eConfigValueFrom.Default:
                                ((InstallerControl)setting).displayFromDefault();
                                break;
                            default:
                                ((InstallerControl)setting).displayFromConfig();
                                break;
                        }
                        
                        //switch (((InstallerConfigValueTag)((Control)sender).Tag).ValueFrom)
                        //{
                        //    case eConfigValueFrom.Default:
                        //        ((InstallerControl)sender).displayFromDefault();
                        //        break;
                        //    case eConfigValueFrom.MIDSettings:
                        //        ((InstallerControl)sender).displayFromMIDSettings();
                        //        break;
                        //    default:
                        //        ((InstallerControl)sender).displayFromConfig();
                        //        break;
                        //}
                        //break;
                        //End TT#1822 - DOConnell - when configuring the config settings in the installer, the context menu item "Reset to Global" abends.

                        ++intEdits;  // TT#4524 - JSmith - Cannot remove setting from configuration file
                    }
                }

                //refresh the panel
                pnlSettings1.Refresh();
            }
        }
    }

    public class InstallerConfigTag
    {
        private string _filePath;
        private string _fileName;
        private XmlDocument _config;
        private XmlDocument _MIDSettings;

        public InstallerConfigTag(string filePath, string fileName, XmlDocument config, XmlDocument MIDSettings)
        {
            _filePath = filePath;
            _fileName = fileName;
            _config = config;
            _MIDSettings = MIDSettings;
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public XmlDocument Config
        {
            get
            {
                return _config;
            }
        }

        public XmlDocument MIDSettings
        {
            get
            {
                return _MIDSettings;
            }
        }
    }

    public class InstallerConfigValueTag
    {
        private string _description;
        private eConfigValueFrom _valueFrom;
        private eConfigChangeType _valueChangeType;
        private string _originalValue;

        public InstallerConfigValueTag(string description, eConfigValueFrom valueFrom, string originalValue)
        {
            _description = description;
            _valueFrom = valueFrom;
            _valueChangeType = eConfigChangeType.None;
            _originalValue = originalValue;
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public eConfigValueFrom ValueFrom
        {
            get
            {
                return _valueFrom;
            }
        }

        public string OriginalValue
        {
            get
            {
                return _originalValue;
            }
        }

        public eConfigChangeType ValueChangeType
        {
            get
            {
                return _valueChangeType;
            }
            set
            {
                _valueChangeType = value;
            }
        }
    }
}
