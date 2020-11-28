using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using System.Threading;
using System.Configuration;
using Microsoft.Win32;
//  Begin TT#1547 - GRT - Remove PDB files
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
//  End TT#1547 - GRT - Remove PDB files
using MIDRetail.Encryption;

namespace MIDRetailInstaller
{
    public partial class ucServer : UserControl
    {
        //events
        public event EventHandler ConfigureNext;
        public event EventHandler NotConfigureNext;

        ToolTip tt = new ToolTip();

        string ConfigurationFileName;

        //client object for install
        ucClient client = null;

        //install folder string
        string InstallFolder = "";
        string ConfigurationInstallFolder = "";

        string sInstallerLocation = String.Empty;

        eConfigMachineBy configureBy = eConfigMachineBy.Name;

        //inventory list
        List<string> installedFiles;
        bool UninstallConfigs = false;
        Hashtable htInstalledServices;

        //object to pass frame to
        InstallerFrame frame;
        ucInstallationLog log;

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        Dictionary<string, string> ServiceLocation = new Dictionary<string, string>();
        // End TT#581-MD - JSmith - Configuration Cleanup

        MIDEncryption encryption = new MIDEncryption();
        public ucServer(InstallerFrame p_frame, ucInstallationLog p_log, string installerLocation)
        {
            //pass the frame object to this object
            frame = p_frame;
            frame.help_ID = "server";
            log = p_log;

            ConfigurationFileName = ConfigurationManager.AppSettings["MIDSettings_config"].ToString();

            //initiate the client object
            client = new ucClient(frame, log);

            sInstallerLocation = installerLocation;

            InitializeComponent();

            InstallFolder = ConfigurationManager.AppSettings["ServicesInstallLocation"].ToString();
            txtInstallFolder.Text = InstallFolder;
            ConfigurationInstallFolder = ConfigurationManager.AppSettings["ClientConfigurationLocation"].ToString();
            txtConfigurationInstallFolder.Text = ConfigurationInstallFolder;

            tt.SetToolTip(rdoInstallTypical, frame.GetToolTipText("server_InstallTypical"));
            tt.SetToolTip(rdoInstallServer, frame.GetToolTipText("server_InstallServer"));
            tt.SetToolTip(clstServices, frame.GetToolTipText("server_clstServices"));
            tt.SetToolTip(rdoStartTypeAuto, frame.GetToolTipText("server_autostart"));
            tt.SetToolTip(rdoStartTypeManual, frame.GetToolTipText("server_manualstart"));
            tt.SetToolTip(rdoUseIPAddress, frame.GetToolTipText("server_useIP"));
            tt.SetToolTip(rdoUseMachineName, frame.GetToolTipText("server_usemachine"));
            tt.SetToolTip(lblInstallDir, frame.GetToolTipText("server_installfolder"));
            tt.SetToolTip(txtInstallFolder, frame.GetToolTipText("server_installfolder"));
            tt.SetToolTip(btnInstallFolder, frame.GetToolTipText("server_installfolderButton"));
            tt.SetToolTip(rdoInstallConfiguration, frame.GetToolTipText("server_installconfig"));
            tt.SetToolTip(lblInstallDir2, frame.GetToolTipText("server_installfolder"));
            tt.SetToolTip(txtConfigurationInstallFolder, frame.GetToolTipText("server_installfolder"));
            tt.SetToolTip(btnConfigurationInstallFolder, frame.GetToolTipText("server_installfolderButton"));

            tt.SetToolTip(rdoInstalledServerTasks, frame.GetToolTipText("server_InstallServerTasks"));
            tt.SetToolTip(cboTasks, frame.GetToolTipText("server_ServerTasks"));
            tt.SetToolTip(lstInstalledServices, frame.GetToolTipText("server_ServerComponents"));

            frame.SetStatusMessage(frame.GetText("SelectInstallTypical"));

            rdoInstallTypical.Text = frame.GetText("rdoInstallTypicalServer");
            rdoInstallServer.Text = frame.GetText("rdoInstallServer");
            rdoInstallConfiguration.Text = frame.GetText("rdoInstallConfiguration");
            rdoInstalledServerTasks.Text = frame.GetText("rdoInstalledServerTasks");

            gbxConfigureUsing.Text = frame.GetText("gbxConfigureUsing");
            rdoUseIPAddress.Text = frame.GetText("rdoUseIPAddress");
            rdoUseMachineName.Text = frame.GetText("rdoUseMachineName");
            gbxStartType.Text = frame.GetText("gbxStartType");
            rdoStartTypeManual.Text = frame.GetText("rdoStartTypeManual");
            rdoStartTypeAuto.Text = frame.GetText("rdoStartTypeAuto");
            lblInstallDir.Text = frame.GetText("lblInstallDir");
            btnInstallFolder.Text = frame.GetText("btnChangeDirectory");
            rdoInstallServer.Text = frame.GetText("rdoInstallServer");
            lblTasks.Text = frame.GetText("lblTasks");
            lstInstalledServices.Text = frame.GetText("lstInstalledServices");
            rdoInstalledServerTasks.Text = frame.GetText("rdoInstalledServerTasks");
            grpClientConfiguration.Text = frame.GetText("grpClientConfiguration");
            btnConfigurationInstallFolder.Text = frame.GetText("btnChangeDirectory");
            lblInstallDir2.Text = frame.GetText("lblInstallDir");
            rdoInstallConfiguration.Text = frame.GetText("rdoInstallConfiguration");
        }

        public bool isItemSelected
        {
            get
            {
                if (rdoInstallServer.Checked)
                {
                    return clstServices.CheckedItems.Count > 0;
                }
                else
                {
                    return lstInstalledServices.SelectedItems.Count > 0;
                }
            }
        }

        private List<string> GetInstalledServices()
        {
            //return variable
            List<string> lstReturn = new List<string>();
            htInstalledServices = new Hashtable();

            //drill down into the registry to our stuff
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);
            string[] SubKeyNames = mid_key.GetSubKeyNames();

            //loop thru the subkeys
            foreach (string SubKeyName in SubKeyNames)
            {
                if (SubKeyName.Contains(InstallerConstants.cClientKey) == false)
                {
                    //open the client key
                    RegistryKey sub_key = mid_key.OpenSubKey(SubKeyName);

                    //Begin TT#858 - JSmith - Installer - does not include option to configure MIDSettings.config
                    //if (sub_key.GetValue("Location").ToString().EndsWith(".config") == false)
                    //{
                    //End TT#858
                        //get the location
                        string location = sub_key.GetValue("Location").ToString().Trim();
                        //if (location.Contains(InstallerConstants.cBatchExecutableName))
                        //{
                        //    location = location.Replace(InstallerConstants.cBatchExecutableName, "");
                        //}
                        lstInstalledServices.Items.Add(location);
                        string[] ArrValues = sub_key.Name.Split('\\');
                        htInstalledServices.Add(ArrValues[ArrValues.Length - 1], null);
                        // Begin TT#581-MD - JSmith - Configuration Cleanup
                        ServiceLocation.Add(ArrValues[ArrValues.Length - 1], location);
                        // End TT#581-MD - JSmith - Configuration Cleanup
                    //Begin TT#858 - JSmith - Installer - does not include option to configure MIDSettings.config
                    //}
                    //End TT#858
                }
            }

            //return value
            return lstReturn;
        }

        private void ucServer_Load(object sender, EventArgs e)
        {
            bool isService = true;
             // Begin TT#74 MD - JSmith - One-button Upgrade
            ////get a list of the installed clients
            //List<string> installedServices = GetInstalledServices();

            ////loop thru the clients and add them to the list
            //foreach (string installedService in installedServices)
            //{
            //    lstInstalledServices.Items.Add(installedService);
            //}
            LoadInstalledServerComponents();
            // End TT#74 MD

            gbxStartType.Visible = false;
            ////show 64 bit controls
            //if (frame._64bit == true)
            //{
            //    chkClient64.Visible = true;
            //    pnlClient64.Visible = true;
            //}

            //default to all checked while removing all previously installed items
            string key = string.Empty;
            Stack stkInstalledIndexes = new Stack();
            for (int intItem = 0; intItem < clstServices.Items.Count; intItem++)
            {
                if(clstServices.Items[intItem].ToString().Contains("Client") != true)
                {
                    switch (clstServices.Items[intItem].ToString())
                    {
                        case InstallerConstants.cApplicationServiceName:
                            key = InstallerConstants.cApplicationServiceKey;
                            isService = true;

                            break;
                        case InstallerConstants.cControlServiceName:
                            key = InstallerConstants.cControlServiceKey;
                            isService = true;
                            
                            break;
                        case InstallerConstants.cHierarchyServiceName:
                            key = InstallerConstants.cHierarchyServiceKey;
                            isService = true;
                            
                            break;
                        case InstallerConstants.cSchedulerServiceName:
                            key = InstallerConstants.cSchedulerServiceKey;
                            isService = true;
                            
                            break;
                        case InstallerConstants.cStoreServiceName:
                            key = InstallerConstants.cStoreServiceKey;
                            isService = true;
                            
                            break;
                        case InstallerConstants.cBatchName:
                            // to allow multiple batch folders, comment out the line below
                            key = InstallerConstants.cBatchKey;
                            isService = false;

                            break;

                    }
                    
                    if (htInstalledServices.ContainsKey(key))
                    {
                        stkInstalledIndexes.Push(intItem);
                    }
                    else
                    {
                        clstServices.SetItemChecked(intItem, true);
                        if (isService)
                        {
                            gbxStartType.Visible = true;
                        }
                    }
                }
            }

            int index;
            while (stkInstalledIndexes.Count > 0)
            {
                index = (int)stkInstalledIndexes.Pop();
                clstServices.Items.RemoveAt(index);
            }

            //enable/disable the rdo button as needed
            if (clstServices.Items.Count == 0)
            {
                rdoInstallServer.Enabled = false;
                rdoInstalledServerTasks.Checked = true;
            }
            else
            {
                rdoInstallServer.Enabled = true;
                rdoInstallServer.Checked = true;
            }

            if (lstInstalledServices.Items.Count == 0)
            {
                rdoInstallTypical.Enabled = true;
                rdoInstallTypical.Checked = true;
                rdoInstalledServerTasks.Enabled = false;
                //frame.InstallTask = eInstallTasks.typicalInstall;
            }
            else
            {
                rdoInstallTypical.Enabled = false;
                rdoInstalledServerTasks.Enabled = true;
                //frame.InstallTask = eInstallTasks.upgrade;
            }
            
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        public bool LoadInstalledServerComponents()
        {
            bool successful = true;
            //get a list of the installed components
            List<string> installedServices = GetInstalledServices();

            //loop thru the clients and add them to the list
            foreach (string installedService in installedServices)
            {
                lstInstalledServices.Items.Add(installedService);
            }
            return successful;
        }
        // End TT#74 MD

        private void RemoveOurRegKey(string RegKeyName)
        {
            //drill down into the application installation reg key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey, true);

            //remove the key
            mid_key.DeleteSubKey(RegKeyName);
        }

        private RegistryKey GetRegistryKey(string ApplicationLocation, out string strRegKey)
        {
            //return variable
            RegistryKey appRegKey = null;
            strRegKey = "";

            //drill down into the application installation reg key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);

            //get the sub_key collection
            string[] strSubKeys = mid_key.GetSubKeyNames();

            //loop thru the keys
            foreach (string strSubKey in strSubKeys)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(strSubKey);

                string strLocation = sub_key.GetValue("Location").ToString().Trim();

                if (strLocation == ApplicationLocation)
                {
                    char[] delim = @"\".ToCharArray();
                    appRegKey = sub_key;
                    strRegKey = sub_key.Name;
                    string[] strRegKeyParts = strRegKey.Split(delim);
                    strRegKey = strRegKeyParts[strRegKeyParts.GetUpperBound(0)].ToString().Trim();
                    break;
                }
            }

            //return registry key
            return appRegKey;
        }

        private string GetInstalledFileList(string ApplicationLocation)
        {
            //registry key variable
            string strInstalledFiles = "";

            //drill down into the application installation reg key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);

            //get the sub_key collection
            string[] strSubKeys = mid_key.GetSubKeyNames();

            //loop thru the keys
            foreach (string strSubKey in strSubKeys)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(strSubKey);

                string strLocation = sub_key.GetValue("Location").ToString().Trim();

                if (strLocation == ApplicationLocation)
                {
                    if (sub_key.GetValue("InstalledFiles") != null)
                    {
                        strInstalledFiles = sub_key.GetValue("InstalledFiles").ToString().Trim();
                    }
                }
            }

            //return registry key
            return strInstalledFiles;
        }


        private string GetConfigRegKey(string strRegKey)
        {
            //return variable
            string strConfigKey = "";

            //drill down into the application installation reg key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);

            //get the sub_key collection
            string[] strSubKeys = mid_key.GetSubKeyNames();

            //loop thru the keys
            foreach (string strSubKey in strSubKeys)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(strSubKey);

                string strClient = "";
                if (sub_key.Name.Contains("Config")  &&
                    sub_key.GetValue("Client") != null)
                {
                    strClient = sub_key.GetValue("Client").ToString().Trim();
                }

                if (strClient == strRegKey)
                {
                    char[] delim = @"\".ToCharArray();
                    strConfigKey = sub_key.Name;
                    string[] strConfigKeyParts = strConfigKey.Split(delim);
                    strConfigKey = strConfigKeyParts[strConfigKeyParts.GetUpperBound(0)].ToString().Trim();
                    break;
                }
            }

            //return registry key
            return strConfigKey;
        }

        private void rdoInstalledServerTasks_CheckedChanged(object sender, EventArgs e)
        {
            //disable the installed client task controls
            if (rdoInstalledServerTasks.Checked == true &&
                lstInstalledServices.Items.Count > 0)
            {
                frame.SetStatusMessage(frame.GetText("SelectInstalledServer"));

                //enable the needed controls
                grpServer.Enabled = false;
                grpClientConfiguration.Enabled = false;
                grpInstalledServerComponents.Enabled = true;

                //select the default row
                lstInstalledServices.SelectedIndex = 0;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.upgrade;

                //set the uninstall file in the frame (uninstall is really an incorrect name because I use if for all the avialable tasks)
                frame.UninstallFile = lstInstalledServices.SelectedItem.ToString().Trim();

                frame.ProgressBarSetValue(0);
            }
        }

        private void rdoInstallTypical_CheckedChanged(object sender, EventArgs e)
        {
            //disable the installed client task controls
            if (rdoInstallTypical.Checked == true)
            {
                frame.SetStatusMessage(frame.GetText("SelectInstallTypical"));

                //enable the needed controls
                grpInstalledServerComponents.Enabled = false;
                grpClientConfiguration.Enabled = false;
                grpServer.Enabled = false;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.typicalInstall;

                frame.ProgressBarSetValue(0);
            }
        }

        private void rdoInstallServer_CheckedChanged(object sender, EventArgs e)
        {
            //disable the installed client task controls
            if (rdoInstallServer.Checked == true)
            {
                frame.SetStatusMessage(frame.GetText("SelectInstallServer"));

                //enable the needed controls
                grpInstalledServerComponents.Enabled = false;
                grpClientConfiguration.Enabled = false;
                grpServer.Enabled = true;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.install;

                frame.ProgressBarSetValue(0);
            }
        }

        private void rdoInstallConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoInstallConfiguration.Checked == true)
            {
                frame.SetStatusMessage(frame.GetText("SelectInstallConfig"));

                //enable the needed controls
                grpInstalledServerComponents.Enabled = false;
                grpClientConfiguration.Enabled = true;
                grpServer.Enabled = false;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.installConfiguration;

                frame.ProgressBarSetValue(0);
            }

        }

        private void lstInstalledServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //set the uninstall file name on the frame
                if (lstInstalledServices.Items.Count != 0 &&
                    lstInstalledServices.SelectedItem != null)
                {
                    frame.UninstallFile = lstInstalledServices.SelectedItem.ToString().Trim();
                    //frame.ProgressBarSetValue(0);
                }
            }
            catch(Exception err)
            {
                log.AddLogEntry("Installed Service List: " + err, eErrorType.warning);
            }
        }

        public void removeComponentFromList(string TargetLocation)
        {
            lstInstalledServices.Items.Remove(TargetLocation);
        }

        //control the frame as needed 
        private void cboTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButton();
        }

        public void Uninstall()
        {
            // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
            //if (MessageBox.Show("Would you like to save the application configurations?", "Uninstall", 
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    UninstallConfigs = true;
            //}
            UninstallConfigs = true;
            // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers

            try
            {
                ArrayList alServicesToUninstalled = new ArrayList();
                //change to wait cursor
                this.Cursor = Cursors.WaitCursor;

                //set up the progress bar
                frame.ProgressBarSetMinimum(0);
                frame.ProgressBarSetMaximum(lstInstalledServices.SelectedItems.Count + 1);
                frame.ProgressBarIncrementValue(1);
                frame.ProgressBarSetValue(0);

                foreach (string lstInstalledService in lstInstalledServices.SelectedItems)
                {
                    alServicesToUninstalled.Add(lstInstalledService);
                }

                Uninstall(alServicesToUninstalled, true);
                frame.RebuildComponentLists();

                if (lstInstalledServices.Items.Count == 0)
                {
                    rdoInstallTypical.Checked = true;
                    rdoInstallTypical.Enabled = true;
                }
                else
                {
                    this.rdoInstallServer.Checked = true;
                }
            }

            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: Uninstall)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: Uninstall", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }
            finally
            {
                //change cursor back to default
                this.Cursor = Cursors.Default;
            }
        }

        public bool Uninstall(string sInstalledService)
        {
            ArrayList alServicesToUninstalled = new ArrayList();
            alServicesToUninstalled.Add(sInstalledService);
            return Uninstall(alServicesToUninstalled, false);
        }

        public bool Uninstall(ArrayList alServicesToUninstalled, bool blRemoveFromList)
        {
            bool isService = true;
            string key = string.Empty;
            string strExeFile = string.Empty;
            //dissect the uninstall file 
            char[] delim = @"\".ToCharArray();
            string entryType = "";
            RegistryKey regKey;
            try
            {
                foreach (string lstInstalledService in alServicesToUninstalled)
                {
                    //get the rows with the sought location
                    DataRow[] drResults = frame.dtWindowsInstalled.Select("Location = '" + lstInstalledService + "'");
                    string[] strFileParts = lstInstalledService.Split(delim);
                    strExeFile = strFileParts[strFileParts.GetUpperBound(0)].ToString().Trim();

                    //if the file was installed by the windows installer
                    if (drResults.GetUpperBound(0) == 0)
                    {
                        entryType = drResults[0].Field<string>("EntryType").ToString().Trim();
                        //...true, use the windows installer to uninstall
                        string uninstall_key = drResults[0].Field<string>("UninstallKey").ToString().Trim();
                        frame.RemoveWindowsInstalledComponent(uninstall_key);
                    }
                    else
                    {
                        regKey = (RegistryKey)frame.htMIDRegistered[lstInstalledService];
                        entryType = regKey.GetValue("EntryType").ToString();
                        if (entryType == eEntryType.MIDConfig.ToString())
                        {
                            frame.DeleteFile(lstInstalledService);

                            string strRegKey = "";
                            RegistryKey appRegKey = GetRegistryKey(lstInstalledService, out strRegKey);
                            if (strRegKey != null &&
                                strRegKey.Trim().Length > 0)
                            {
                                RemoveOurRegKey(strRegKey);
                            }
                        }
                        else
                        {
                            //get the parent directory from the selected service
                            string strParentDir = null;
                            if (entryType == eEntryType.MIDAPI.ToString())
                            {
                                strParentDir = lstInstalledService;
                            }
                            else
                            {
                                strParentDir = Directory.GetParent(lstInstalledService).ToString().Trim();
                            }


                            //stop and uninstall service
                            if (ServiceUninstall(strExeFile, strParentDir, entryType))
                            {
                                //get the file inventory
                                string strDelimitedInstalledFiles = GetInstalledFileList(lstInstalledService);

                                //delete registry key
                                string strRegKey = "";
                                RegistryKey appRegKey = GetRegistryKey(lstInstalledService, out strRegKey);
                                if (strRegKey != null &&
                                    strRegKey.Trim().Length > 0)
                                {
                                    RemoveOurRegKey(strRegKey);
                                }

                                //  BEGIN TT#1283
                                //      remove the HLKM entries
                                frame.RO.UninstallApplication(strExeFile);
                                //  END TT#1283

                                string[] strFiles = strDelimitedInstalledFiles.Split(';');
                                //loop thru the file and delete them
                                foreach (string strFile in strFiles)
                                {
                                    //delete the files
                                    frame.DeleteFile(strFile);
                                }

                                //loop thru the file and delete them
                                foreach (string strFile in Directory.GetFiles(strParentDir))
                                {
                                    if (strDelimitedInstalledFiles.IndexOf(strFile + ";") >= 0)
                                    {
                                        //delete the files
                                        frame.DeleteFile(strFile);
                                    }
                                }

                                //delete config files if user requests
                                if (UninstallConfigs == true)
                                {
                                    foreach (string strFile in Directory.GetFiles(strParentDir, "*.config"))
                                    {
                                        frame.DeleteFile(strFile);
                                    }
                                }

                                //delete service install log files if user requests
                                foreach (string strFile in Directory.GetFiles(strParentDir, "*.InstallLog"))
                                {
                                    frame.DeleteFile(strFile);
                                }

                                if (strParentDir.Contains("Batch"))
                                {
                                    // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                    frame.CleanupFolders(strParentDir + @"\StoreDelete");
                                    // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                    strParentDir += @"\Transactions";
                                }

                                frame.CleanupFolders(strParentDir);
                            }
                        }
                        
                        //advance the progress bar
                        frame.ProgressBarIncrementValue(1);
                    }

                    if (entryType == eEntryType.MIDApplicationService.ToString())
                    {
                        // set key to empty so will not appear in list
                        //key = InstallerConstants.cApplicationServiceName;
                        key = String.Empty;
                        isService = true;
                    }
                    else if (entryType == eEntryType.MIDControlService.ToString())
                    {
                        key = InstallerConstants.cControlServiceName;
                        isService = true;
                    }
                    else if (entryType == eEntryType.MIDHierarchyService.ToString())
                    {
                        key = InstallerConstants.cHierarchyServiceName;
                        isService = true;
                    }
                    else if (entryType == eEntryType.MIDSchedulerService.ToString())
                    {
                        key = InstallerConstants.cSchedulerServiceName;
                        isService = true;
                    }
                    else if (entryType == eEntryType.MIDStoreService.ToString())
                    {
                        key = InstallerConstants.cStoreServiceName;
                        isService = true;
                    }
                    else if (entryType == eEntryType.MIDAPI.ToString())
                    {
                        key = InstallerConstants.cBatchName;
                        isService = false;
                    }
                    else if (entryType == eEntryType.MIDConfig.ToString())
                    {
                        key = string.Empty;
                        isService = false;
                    }

                    if (key.Trim().Length > 0)
                    {
                        clstServices.Items.Add(key);
                        rdoInstallServer.Enabled = true;
                        if (isService)
                        {
                            gbxStartType.Visible = true;
                        }
                    }

                    if (blRemoveFromList)
                    {
                        removeComponentFromList(lstInstalledService);
                    }

                    frame.CleanupFolders(lstInstalledService);

                    Application.DoEvents();
                    //show the screen long enough to see the removed components from the list
                    Thread.Sleep(2000);
                }

                if (lstInstalledServices.Items.Count == 0)
                {
                    rdoInstalledServerTasks.Enabled = false;
                }

                frame.ProgressBarSetToMaximum();
                Application.DoEvents();
                //user acknowledgement
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = "Requested service(s) has been successfully uninstalled";
                // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                //frame.SetStatusMessage("Requested service(s) has been successfully uninstalled");
                frame.SetStatusMessage("Requested server components have been successfully uninstalled");
                // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
				// End TT#1668
                Application.DoEvents();

                ////clear memory
                //Application.DoEvents();

                //show the screen long enough to see the removed components from the list
                Thread.Sleep(2000);

            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: Uninstall)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: Uninstall", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }
            finally
            {
                //change cursor back to default
                this.Cursor = Cursors.Default;
            }

            return true;
        }

        public bool Upgrade()
        {
            //return variable
            bool blReturn = true;
            bool blSuccessful = true;
            bool blInstallService = false;
            ConfigFiles config;
            ArrayList alServicesToUpgrade = new ArrayList();
            bool blAddExeNameToDisplay = true;
            string strAppFile = "";
            string strServKey = "";
            string strTargetFolder;
            // Begin TT#581-MD - JSmith - Configuration Cleanup
            ArrayList alConfigsToUpgrade = new ArrayList();
            string strParentDir = string.Empty;
            string strAppDir = string.Empty;
            // End TT#581-MD - JSmith - Configuration Cleanup

            try
            {
			    // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                //disable the folder button
                btnInstallFolder.Enabled = false;

                //file variable
                string strFile = "";
                string strExeFile = "";
                string strServName = "";
                //string strServAbbr = "";
                eEntryType intAppIdx = eEntryType.None;

                //set up the progress bar
                if (!frame.blPerformingOneClickUpgrade)
                {
                    frame.ProgressBarSetMinimum(0);
                    frame.ProgressBarSetMaximum(lstInstalledServices.SelectedItems.Count + 2);
                    frame.ProgressBarIncrementValue(1);
                }

                foreach (string lstInstalledService in lstInstalledServices.SelectedItems)
                {
                    if (lstInstalledService.ToUpper().IndexOf(".CONFIG") == -1)
                    {
                        alServicesToUpgrade.Add(lstInstalledService);
                    }
                    // Begin TT#581-MD - JSmith - Configuration Cleanup
                    else
                    {
                        alConfigsToUpgrade.Add(lstInstalledService);
                    }
                    // End TT#581-MD - JSmith - Configuration Cleanup
                }

                // Begin TT#581-MD - JSmith - Configuration Cleanup
                foreach (string lstInstalledConfig in alConfigsToUpgrade)
                {
                    strParentDir = Directory.GetParent(lstInstalledConfig).ToString().Trim();
                    config.MakeConfigBackup(strParentDir, false);

                    strServName = InstallerConstants.cConfigKey;
                    strServKey = InstallerConstants.cConfigKey;
                    blAddExeNameToDisplay = false;

                    InstallUpgradeApp(strParentDir, strServName, strFile, out strExeFile, "Upgrade", blAddExeNameToDisplay, ref blSuccessful);

                    frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
                    config.ReplaceDefaultsInConfigFiles(strParentDir, strParentDir, null, configureBy, Directory.GetDirectoryRoot(strParentDir), false);

                    log.AddLogEntry("(Server: Upgrade) UpgradeConfigFiles", eErrorType.message);
                    config.UpgradeConfigFiles(strParentDir, strAppDir, false, false);
                }
                // End TT#581-MD - JSmith - Configuration Cleanup

                foreach (string lstInstalledService in alServicesToUpgrade)
                {
                    blSuccessful = true;
                    log.AddLogEntry("(Server: Upgrade) " + lstInstalledService, eErrorType.message);

                    char[] delim = @"\".ToCharArray();
                    string[] strExeFileParts = lstInstalledService.Split(delim);
                    strExeFile = strExeFileParts[strExeFileParts.GetUpperBound(0)].ToString().Trim();

                    //get the parent directory
                    // Begin TT#581-MD - JSmith - Configuration Cleanup
                    //string strParentDir = string.Empty;
                    //string strAppDir = string.Empty;
                    // End TT#581-MD - JSmith - Configuration Cleanup
                    log.AddLogEntry("(Server: Upgrade) get the parent directory" , eErrorType.message);
                    if (lstInstalledService.Contains("Batch") ||
                        lstInstalledService.Contains(InstallerConstants.cBatchName))
                    {
                        strParentDir = lstInstalledService.Trim();
                        strAppDir = Directory.GetParent(strParentDir).ToString().Trim();
                        // Begin TT#1132 - JSmith - Upon installing received an installation critical error
                        strExeFile = InstallerConstants.cBatchName;
                        // End TT#1132
                    }
                    else
                    {
                        strParentDir = Directory.GetParent(lstInstalledService).ToString().Trim();
                        strAppDir = Directory.GetParent(strParentDir).ToString().Trim();
                    }

                    //set the install application stuff
                    log.AddLogEntry("(Server: Upgrade) get the application info", eErrorType.message);
                    if (strExeFile == null || strExeFile.Trim().Length == 0 ||
                        ConfigurationManager.AppSettings["APIName"].Contains(strExeFile) == true)
                    {
                        //if (frame._64bit == false)
                        //{
                            strFile = InstallerConstants.cBatchArchive;
                        //}
                        //else
                        //{
                        //    strFile = InstallerConstants.cBatchArchive64;
                        //}

                        strServName = InstallerConstants.cBatchName;
                        strServKey = InstallerConstants.cBatchKey;
                        blAddExeNameToDisplay = false;
                        intAppIdx = eEntryType.MIDAPI;
                    }
                    else if(ConfigurationManager.AppSettings["ApplicationServiceNames"].Contains(strExeFile) == true)
                    {
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cApplicationServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cApplicationServiceArchive64;
                        }

                        strServName = InstallerConstants.cApplicationServiceName;
                        strServKey = InstallerConstants.cApplicationServiceKey;
                        blInstallService = true;
                        intAppIdx = eEntryType.MIDApplicationService;
                    }
                    else if(ConfigurationManager.AppSettings["ControlServiceNames"].Contains(strExeFile) == true)
                    {
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cControlServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cControlServiceArchive64;
                        }

                        strServName = InstallerConstants.cControlServiceName;
                        strServKey = InstallerConstants.cControlServiceKey;
                        blInstallService = true;
                        intAppIdx = eEntryType.MIDControlService;
                    }
                    else if(ConfigurationManager.AppSettings["MerchandiseServiceNames"].Contains(strExeFile) == true)
                    {
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cHierarchyServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cHierarchyServiceArchive64;
                        }

                        strServName = InstallerConstants.cHierarchyServiceName;
                        strServKey = InstallerConstants.cHierarchyServiceKey;
                        blInstallService = true;
                        intAppIdx = eEntryType.MIDHierarchyService;

                    }
                    else if(ConfigurationManager.AppSettings["SchedulerServiceNames"].Contains(strExeFile) == true)
                    {
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cSchedulerServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cSchedulerServiceArchive64;
                        }

                        strServName = InstallerConstants.cSchedulerServiceName;
                        strServKey = InstallerConstants.cSchedulerServiceKey;
                        blInstallService = true;
                        intAppIdx = eEntryType.MIDSchedulerService;

                    }
                    else if(ConfigurationManager.AppSettings["StoreServiceNames"].Contains(strExeFile) == true)
                    {
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cStoreServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cStoreServiceArchive64;
                        }

                        strServName = InstallerConstants.cStoreServiceName;
                        strServKey = InstallerConstants.cStoreServiceKey;
                        blInstallService = true;
                        intAppIdx = eEntryType.MIDStoreService;

                    }

                    log.AddLogEntry("(Server: Upgrade) backup config file", eErrorType.message);
                    // copy all config files to use later
                    config.MakeConfigBackup(strParentDir);

                    //get the existing registry keys
                    log.AddLogEntry("(Server: Upgrade) get registry info", eErrorType.message);
                    string strRegKey = "";
                    RegistryKey appRegKey = GetRegistryKey(lstInstalledService, out strRegKey);
                    if (lstInstalledService.EndsWith(".exe"))
                    {
                        strTargetFolder = Directory.GetParent(lstInstalledService).ToString().Trim();
                    }
                    else
                    {
                        strTargetFolder = lstInstalledService.Trim();
                    }

                    // Begin TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
                    ModuleCleanup mc = new ModuleCleanup();
                    mc.RemoveOldFiles(strTargetFolder, true);
                    // End TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
                    
                    if ((string)appRegKey.GetValue("InstallType") == "Windows")
                    {
                        ArrayList alServicesToUninstalled = new ArrayList();
                        alServicesToUninstalled.Add(lstInstalledService);
                        log.AddLogEntry("(Server: Upgrade) uninstall windows", eErrorType.message);
                        Uninstall(alServicesToUninstalled, false);
                        //Install
                        log.AddLogEntry("(Server: Upgrade) install", eErrorType.message);
                        Install(strTargetFolder, strServName, "Upgrade", out blAddExeNameToDisplay, out strAppFile, out strExeFile);
                    }
                    else
                    {
                        // Begin TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                        bool automaticStart = rdoStartTypeAuto.Checked;
                        // End TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                        //stop and uninstall the service
                        if (blInstallService)
                        {
                            // Begin TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                            string[] ExeFileParts = strExeFile.Split('.');
                            automaticStart = frame.IsServiceAutomaticStart(ExeFileParts[0].ToString().Trim());
                            // End TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                            log.AddLogEntry("(Server: Upgrade) uninstall service", eErrorType.message);
                            blSuccessful = ServiceUninstall(strExeFile, strServName, intAppIdx.ToString());
                        }

                        if (blSuccessful)
                        {
                            //install application
                            log.AddLogEntry("(Server: Upgrade) InstallUpgradeApp", eErrorType.message);
                            InstallUpgradeApp(strTargetFolder, strServName, strFile, out strExeFile, "Upgrade", blAddExeNameToDisplay, ref blSuccessful);

                            if (blSuccessful)
                            {
                                RegisterInstallation(strTargetFolder, strServKey, strExeFile, intAppIdx, true);

                                //install service
                                if (blInstallService &&
                                    blSuccessful)
                                {
                                    log.AddLogEntry("(Server: Upgrade) ServiceInstall", eErrorType.message);
                                    // Begin TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                                    //ServiceInstall(strExeFile, strTargetFolder, false, rdoStartTypeAuto.Checked);
                                    ServiceInstall(strExeFile, strTargetFolder, false, automaticStart);
                                    // End TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
                                }
                            }
                        }
                    }

                    if (!blSuccessful)
                    {
                        blReturn = false;
                        break;
                    }

                    // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                    // Begin TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
                    //ModuleCleanup mc = new ModuleCleanup();
                    if (blSuccessful)
                    {
                        mc.RemoveOldFiles(strTargetFolder, false);
                    }
                    // End TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
					// End TT#1305-MD - JSmith - Change Auto Upgrade

                    if (blSuccessful)
                    {
					    // Begin TT#1668 - JSmith - Install Log
                        frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
						// End TT#1668 
                        // Begin TT#1729 - JSmith - Scheduler Service config overwrites previous configuration settings
                        config.ReplaceDefaultsInConfigFiles(strParentDir, strAppDir, null, configureBy, Directory.GetDirectoryRoot(strAppDir));
                        // End TT#1729

                        log.AddLogEntry("(Server: Upgrade) UpgradeConfigFiles", eErrorType.message);
                        config.UpgradeConfigFiles(strParentDir, strAppDir, false);
                    }

                    //advance progress bar
                    frame.ProgressBarIncrementValue(1);
                }

                // Begin TT#1627-MD - stodd - attribute set filter
#if (DEBUG)
                // Skip the updating of the database upgrade config
#else
                //======================================================
                // Updating the app.config for the database upgrade
                //======================================================
                string currentLocation = Directory.GetCurrentDirectory();
                DirectoryInfo parentLocationInfo = Directory.GetParent(currentLocation);
                string parentLocation = parentLocationInfo.FullName;
                string databaseUpgradeLocation = parentLocation + "\\Database";

                config.MakeConfigBackup(databaseUpgradeLocation);
                config.UpgradeConfigFiles(databaseUpgradeLocation, strAppDir, false);
#endif
                // End TT#1627-MD - stodd - attribute set filter

                if (blSuccessful)
                {
                    // Begin TT#581-MD - JSmith - Configuration Cleanup
                    RemoveUnneededConfigSettings(config);
                    // End TT#581-MD - JSmith - Configuration Cleanup

                    // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
                    ConvertConfigSettings(config);
                    // End TT#644-MD - JSmith - Modify install values for several configuration settings
                }

                log.AddLogEntry("(Server: Upgrade) out of loop", eErrorType.message);
                //advance progress bar
                if (!frame.blPerformingOneClickUpgrade &&
                    !frame.blPerformingTypicalInstall)
                {
                    frame.ProgressBarSetToMaximum();
                }
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: Upgrade)" + err.ToString(), eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: Upgrade", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }

            //return value
            return blReturn;
        }

        // Begin TT#581-MD - JSmith - Configuration Cleanup
        private void RemoveUnneededConfigSettings(ConfigFiles config)
        {
            string strConfigLocation;
            string strDirectory;
            Dictionary<string, string> diRemovedGlobalConfigValues = null;
            ArrayList alFileLocation = new ArrayList();

            if (ServiceLocation.TryGetValue(InstallerConstants.cConfigKey, out strConfigLocation))
            {
                diRemovedGlobalConfigValues = config.RemoveUnneededConfigSettings(strConfigLocation);
            }

            foreach (KeyValuePair<string, string> serviceKeyPair in ServiceLocation)
            {
                if (serviceKeyPair.Key.Contains(InstallerConstants.cBatchKey))
                {
                    strDirectory = serviceKeyPair.Value;
                }
                else
                {
                    strDirectory = Directory.GetParent(serviceKeyPair.Value).ToString().Trim();
                }

                frame.DirSearch(strDirectory, "*.config", alFileLocation);
                
            }

            foreach (string strConfigFile in alFileLocation)
            {
                config.RemoveUnneededConfigSettings(strConfigFile);
            }

            config.ApplyRemovedGlobalConfigRows(diRemovedGlobalConfigValues, ServiceLocation);

        }
        // End TT#581-MD - JSmith - Configuration Cleanup

        // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
        private void ConvertConfigSettings(ConfigFiles config)
        {
            string strConfigLocation;
            string strDirectory;
            ArrayList alFileLocation = new ArrayList();


            foreach (KeyValuePair<string, string> serviceKeyPair in ServiceLocation)
            {
                if (serviceKeyPair.Key.Contains(InstallerConstants.cBatchKey))
                {
                    strDirectory = serviceKeyPair.Value;
                }
                else
                {
                    strDirectory = Directory.GetParent(serviceKeyPair.Value).ToString().Trim();
                }

                frame.DirSearch(strDirectory, "*.config", alFileLocation);
                
            }

            foreach (string strConfigFile in alFileLocation)
            {
                if (strConfigFile.Contains("RelieveHeaders.exe.config"))
                {
                    config.ConvertRelieveHeadersConfigSettings(strConfigFile);
                }
            }
        }
        // End TT#644-MD - JSmith - Modify install values for several configuration settings

        public List<string> Install(string installFolder)
        {
            //return variable
            string strAppFile = "";
            List<string> lstReturn = new List<string>();
            bool blInstallService = false;
            bool blSuccessful = true;
            bool blAddExeNameToDisplay = true;
            ArrayList alInstalledComponents = new ArrayList();

            ConfigFiles config;

            try
            {
                // Begin TT#195 MD - JSmith - Add environment authentication
                if (!frame.isValidServer())
                {
                    MessageBox.Show(frame.GetText("unsupportedVersion"), "Unsupported Version Error", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                    log.AddLogEntry(frame.GetText("unsupportedVersion"), eErrorType.message);
                    frame.SetStatusMessage(frame.GetText("unsupportedVersion"));
                    return lstReturn;
                }
                // End TT#195 MD

                if (!frame.ServerVersionCheck())
                {
                    log.AddLogEntry(frame.GetText("installCancelled"), eErrorType.message);
                    frame.SetStatusMessage(frame.GetText("installCancelled"));
                    return lstReturn;
                }
                // End TT#195 MD

			    // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668

                //disable the folder button
                btnInstallFolder.Enabled = false;

                //file variable
                //string strFile = "";
                string strExeFile = "";
                //string strServAbbr = "";
                //eEntryType intAppIdx = eEntryType.None;
                string strTargetLocation;
                if (installFolder == null)
                {
                    strTargetLocation = txtInstallFolder.Text;
                }
                else
                {
                    strTargetLocation = installFolder;
                }

                string strTargetDrive = Directory.GetDirectoryRoot(strTargetLocation).ToString().Trim();
                string strTargetParent = Directory.GetParent(strTargetLocation).ToString().Trim();

                //set up the progress bar
                if (!frame.blPerformingOneClickUpgrade &&
                    !frame.blPerformingTypicalInstall)
                {
                    frame.ProgressBarSetMinimum(0);
                    frame.ProgressBarSetMaximum(clstServices.CheckedItems.Count + 1);
                    frame.ProgressBarIncrementValue(1);
                }

                foreach (string CheckedItem in clstServices.CheckedItems)
                {
                    alInstalledComponents.Add(CheckedItem);
                }

                foreach (string CheckedItem in alInstalledComponents)
                {
                    blSuccessful = Install(txtInstallFolder.Text, CheckedItem, "Install", out blAddExeNameToDisplay, out strAppFile, out strExeFile);

                    if (!blSuccessful ||
                        strAppFile == null)
                    {
                        continue;
                    }
                    if (blAddExeNameToDisplay)
                    {
                        lstReturn.Add(strAppFile + @"\" + strExeFile);
                    }
                    else
                    {
                        // Begin TT#904 - JSmith - Uninstalling APIs after install fails
                        lstReturn.Add(strAppFile);
                        //lstReturn.Add(strAppFile + @"\");
                        // End TT#904
                    }

                    //add the exe file to the list of installed components
                    if (blAddExeNameToDisplay)
                    {
                        lstInstalledServices.Items.Add(strAppFile + @"\" + strExeFile);
                    }
                    else
                    {
                        // Begin TT#904 - JSmith - Uninstalling APIs after install fails
                        lstInstalledServices.Items.Add(strAppFile);
                        //lstInstalledServices.Items.Add(strAppFile + @"\");
                        // End TT#904
                    }

                    clstServices.Items.Remove(CheckedItem);

                    if (blSuccessful &&
                        !frame.blPreviousInstalls)
                    {
                        string strParentDir = string.Empty;
                        string strAppDir = string.Empty;
                        strParentDir = txtInstallFolder.Text.Trim();
                        strAppDir = Directory.GetParent(strParentDir).ToString().Trim();
                        config.CopyOldConfigFiles(frame, strAppFile);
                        config.UpgradeConfigFiles(strParentDir, strAppDir, true);
                    }

                    //advance progress bar
                    frame.ProgressBarIncrementValue(1);
                }

                //create the config files needed
				// Begin TT#1668 - JSmith - Install Log
				//ConfigFiles cf = new ConfigFiles(frame.installer_data, log);
                ConfigFiles cf = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                if (!cf.MIDSettings_config_Exists(txtInstallFolder.Text))
                {
                    cf.CopyMIDSettings_config(Application.StartupPath, txtInstallFolder.Text);
                    
                    //register application installation
                    RegisterInstallation(txtInstallFolder.Text, InstallerConstants.cServicesKey, "", eEntryType.MIDConfig, false );

                    lstInstalledServices.Items.Add(txtInstallFolder.Text + @"\" + ConfigurationFileName);

                    lstReturn.Add(txtInstallFolder.Text + @"\" + ConfigurationFileName);
                    frame.ProgressBarIncrementValue(1);
                }

                // Begin TT#1668 - JSmith - Install Log
                frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
				// End TT#1668
                config.ReplaceDefaultsInConfigFiles(strTargetLocation, strTargetLocation, null, configureBy, strTargetDrive);
                // set the values to cause encryption
                config.SetConfigValue(strTargetLocation + @"\" + ConfigurationFileName, "User", "administrator");
                config.SetConfigValue(strTargetLocation + @"\" + ConfigurationFileName, "Password", "administrator");

                frame.RebuildComponentLists();

                if (frame.FileInUse(installFolder))
                {
                    log.AddLogEntry("Found files in use after install completed", eErrorType.error);
                }

                //advance progress bar
                if (!frame.blPerformingOneClickUpgrade &&
                    !frame.blPerformingTypicalInstall)
                {
                    frame.ProgressBarSetToMaximum();
                }

                ////install the clients
                //InstallClient();
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: Install)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: Install", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }
            finally
            {

                /* Begin TT#1192 - Batch executable files blocked after initial install/upgrade - APicchetti - 3/17/2011 */
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.Arguments = "-s -d " + installFolder;
                psi.FileName = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\streams.exe";
                Process.Start(psi);
                /* End TT#1192 - Batch executable files blocked after initial install/upgrade - APicchetti - 3/17/2011 */

                //change cursor back to default
                this.Cursor = Cursors.Default;
            }

            //return value
            return lstReturn;
        }

        public bool Install(string strTargetLocation, string strItem, string Install_Upgrade, out bool blAddExeNameToDisplay, out string strAppFile, out string strExeFile)
        {
            bool blSuccessful = true;
            bool blInstallService = false;
            blAddExeNameToDisplay = true;
            strAppFile = null;
            strExeFile = null;

            try
            {
                //file variable
                string strFile = "";
                strExeFile = "";
                string strServKey = "";
                eEntryType intAppIdx = eEntryType.None;

                switch (strItem)
                {
                    case InstallerConstants.cApplicationServiceName:
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cApplicationServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cApplicationServiceArchive64;
                        }

                        //strExeFile = InstallerConstants.cApplicationServiceExecutableName;
                        strServKey = InstallerConstants.cApplicationServiceKey;
                        intAppIdx = eEntryType.MIDApplicationService;
                        blInstallService = true;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallApplicationServiceFolder"].ToString();

                        break;
                    case InstallerConstants.cControlServiceName:
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cControlServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cControlServiceArchive64;
                        }

                        //strExeFile = InstallerConstants.cControlServiceExecutableName;
                        strServKey = InstallerConstants.cControlServiceKey;
                        intAppIdx = eEntryType.MIDControlService;
                        blInstallService = true;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallControlServiceFolder"].ToString();

                        break;
                    case InstallerConstants.cHierarchyServiceName:
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cHierarchyServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cHierarchyServiceArchive64;
                        }

                        //strExeFile = InstallerConstants.cHierarchyServiceExecutableName;
                        strServKey = InstallerConstants.cHierarchyServiceKey;
                        intAppIdx = eEntryType.MIDHierarchyService;
                        blInstallService = true;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallMerchandiseServiceFolder"].ToString();

                        break;
                    case InstallerConstants.cSchedulerServiceName:
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cSchedulerServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cSchedulerServiceArchive64;
                        }

                        //strExeFile = InstallerConstants.cSchedulerServiceExecutableName;
                        strServKey = InstallerConstants.cSchedulerServiceKey;
                        intAppIdx = eEntryType.MIDSchedulerService;
                        blInstallService = true;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallSchedulerServiceFolder"].ToString();

                        break;
                    case InstallerConstants.cStoreServiceName:
                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cStoreServiceArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cStoreServiceArchive64;
                        }

                        //strExeFile = InstallerConstants.cStoreServiceExecutableName;
                        strServKey = InstallerConstants.cStoreServiceKey;
                        intAppIdx = eEntryType.MIDStoreService;
                        blInstallService = true;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallStoreServiceFolder"].ToString();

                        break;
                    case InstallerConstants.cBatchName:

                        if (frame._64bit == false)
                        {
                            strFile = InstallerConstants.cBatchArchive;
                        }
                        else
                        {
                            strFile = InstallerConstants.cBatchArchive64;
                        }

                        //strExeFile = InstallerConstants.cBatchExecutableName;
                        strServKey = InstallerConstants.cBatchKey;
                        intAppIdx = eEntryType.MIDAPI;
                        blAddExeNameToDisplay = false;
                        strTargetLocation += @"\" + ConfigurationManager.AppSettings["InstallAPIFolder"].ToString();

                        break;

                }

                //install application
                strAppFile = InstallUpgradeApp(strTargetLocation, strItem.Trim(), strFile, out strExeFile, Install_Upgrade, blAddExeNameToDisplay, ref blSuccessful);

                

                if (strAppFile != null)
                {
                    //install service
                    if (blInstallService)
                    {
                        ServiceInstall(strExeFile, strTargetLocation, false, rdoStartTypeAuto.Checked);
                    }

                    //register service
                    RegisterInstallation(strTargetLocation, strServKey, strExeFile, intAppIdx, false);
                }
                else
                {
                    blSuccessful = false;
                }

                // Begin TT#1242 - JSmith - Installer configuration issues
                if (Install_Upgrade == "Install")
                {
                    if (!Directory.Exists(ConfigurationManager.AppSettings["ReleaseLocation"].ToString()))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["ReleaseLocation"].ToString());
                        frame.ShareFolder(ConfigurationManager.AppSettings["ReleaseLocation"].ToString(), ConfigurationManager.AppSettings["ReleaseShareName"].ToString(), ConfigurationManager.AppSettings["ReleaseShareName"].ToString(), true);
                    }
                    //Begin TT#1598-MD - JSmith - Add default Export data directory to the install process
                    if (!Directory.Exists(ConfigurationManager.AppSettings["ExportLocation"].ToString()))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["ExportLocation"].ToString());
                        frame.ShareFolder(ConfigurationManager.AppSettings["ExportLocation"].ToString(), ConfigurationManager.AppSettings["ExportShareName"].ToString(), ConfigurationManager.AppSettings["ExportShareName"].ToString(), true);
                    }
                    //End TT#1598-MD - JSmith - Add default Export data directory to the install process
                }
                // End TT#1242

                //BEGIN TT#3428-M-VStuart-Add Interface Files subdirectories in MIDRetailData-MID
                if (Install_Upgrade == "Install")
                {
                    // Get and parse my directory string.
                    var directories = ConfigurationManager.AppSettings["DataDirectoriesLocation"];
                    var dirs = directories.Split(new[] {'|'}, System.StringSplitOptions.RemoveEmptyEntries);

                    // Loop through the directories.
                    foreach (string item in dirs)
                    {
                        if (!Directory.Exists(item))
                        {
                            Directory.CreateDirectory(item);
                        }
                    }
                }
                //END TT#3428-M-VStuart-Add Interface Files subdirectories in MIDRetailData-MID
            }
            catch
            {
            }

            return blSuccessful;
        }

        public List<string> InstallGlobalConfiguration(string installFolder)
        {
            ConfigFiles config;

            /* Begin TT#1198 - Version 4.0.4073 Installer throws exception during global client settings install - APicchetti - 3/17/2011 */
            if (installFolder == null && rdoInstallConfiguration.Checked == true)
            {
                installFolder = txtConfigurationInstallFolder.Text.ToString();
            }
            /* End TT#1198 - Version 4.0.4073 Installer throws exception during global client settings install */

            string strTargetDrive = Directory.GetDirectoryRoot(installFolder).ToString().Trim();

            string fileName = ConfigurationInstallFolder + @"\" + ConfigurationFileName;
            if (installFolder == null)
            {
                fileName = ConfigurationInstallFolder + @"\" + ConfigurationFileName;
            }
            else
            {
                fileName = installFolder + @"\" + ConfigurationFileName;
            }

            // Begin TT#1668 - JSmith - Install Log
			//config = new ConfigFiles(frame.installer_data, log);
            config = new ConfigFiles(frame, frame.installer_data, log);
			// End TT#1668

            List<string> lstReturn = new List<string>();
            bool registerFile = true;
            try
            {
                if (!Directory.Exists(ConfigurationInstallFolder))
                {
                    //Directory.CreateDirectory(ConfigurationInstallFolder);
                    frame.CreateDirectory(ConfigurationInstallFolder);
                }
                else if (File.Exists(fileName))
                {
                    if (MessageBox.Show("This folder already contains the configuration file.  Would you like to replace the existing file?",
                                "Confirm File Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return lstReturn;
                    }
                    else
                    {
                        // make sure the file is not read only
                        File.SetAttributes(fileName, File.GetAttributes(fileName) & ~(FileAttributes.ReadOnly));

                        frame.DeleteFile(fileName);
                        registerFile = false;
                    }
                }

                //create the config files needed
				// Begin TT#1668 - JSmith - Install Log
				//ConfigFiles cf = new ConfigFiles(frame.installer_data, log);
                ConfigFiles cf = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                if (!cf.MIDSettings_config_Exists(ConfigurationInstallFolder))
                {
                    cf.CopyMIDSettings_config(Application.StartupPath, ConfigurationInstallFolder);

                    // Begin TT#1668 - JSmith - Install Log
                    frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
					// End TT#1668
                    config.ReplaceDefaultsInConfigFiles(ConfigurationInstallFolder, ConfigurationInstallFolder, null, configureBy, strTargetDrive);
                    // set the values to cause encryption
                    config.SetConfigValue(ConfigurationInstallFolder + @"\" + ConfigurationFileName, "User", "administrator");
                    config.SetConfigValue(ConfigurationInstallFolder + @"\" + ConfigurationFileName, "Password", "administrator");

                    if (!frame.blPreviousInstalls)
                    {
                        string strParentDir = string.Empty;
                        string strAppDir = string.Empty;
                        strParentDir = ConfigurationInstallFolder;
                        strAppDir = Directory.GetParent(strParentDir).ToString().Trim();
                        config.CopyOldConfigFiles(frame, ConfigurationInstallFolder);
                        config.UpgradeConfigFiles(strParentDir, ConfigurationInstallFolder, true);
                    }

                    //register application installation
                    if (registerFile)
                    {
                        RegisterInstallation(ConfigurationInstallFolder, InstallerConstants.cServicesKey, "", eEntryType.MIDConfig, false);
                    }

                    lstReturn.Add(fileName);

                    //add the file to the list of installed components
                    lstInstalledServices.Items.Add(fileName);

                    frame.ProgressBarIncrementValue(1);

                    return lstReturn;
                }

                
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: InstallGlobalConfiguration)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: InflateInstallFile", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }
            return lstReturn;
        }

        private bool InflateInstallFile(string FolderName, string ArchiveFile, string TargetLocation, string InstallFile, out string ExeFile, bool blAddExeNameToDisplay)
        {
            ExeFile = null;
            installedFiles = new List<string>();
            try
            {
                if (frame.FileInUse(TargetLocation))
                {
                    // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                   // MessageBox.Show("A module is in use in folder " + TargetLocation + "." + Environment.NewLine +
                   //"All access to modules in this folder must be stopped before the install can continue", "Installation Error", MessageBoxButtons.OK,
                   //MessageBoxIcon.Error);
                    string msg = "A module is in use in folder " + TargetLocation + "." + Environment.NewLine +
                   "All access to modules in this folder must be stopped and the install reprocessed.";
                    log.AddLogEntry(msg, eErrorType.error);
                    MessageBox.Show(msg, "Installation Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error); 
                    // End TT#1822-MD - JSmith - Installer not detecting incomplete install
                    return false;
                }

                frame.CopyFolder(Application.StartupPath + @"\Install Files\" + FolderName, TargetLocation, installedFiles);
                ////copy file
                //System.IO.File.Copy(Application.StartupPath + @"\Install Files\" + ArchiveFile, TargetLocation + @"\" + ArchiveFile, true);

                ////Archive temp file stream
                //ZipInputStream zipStream = new ZipInputStream(System.IO.File.Open(TargetLocation + @"\" + ArchiveFile, FileMode.Open));

                ////the zip entry
                //ZipEntry zippedFile;

                ////loop thru the files in the archive
                //while ((zippedFile = zipStream.GetNextEntry()) != null)
                //{
                //    string strZippedFile = zippedFile.Name;

                //    //update the install message
                //    Application.DoEvents();
                //    frame.SetStatusMessage("Copying file: " + strZippedFile + " to " + TargetLocation;

                //    //fill the file inventory list to
                //    if (TargetLocation.EndsWith(@"\") == true)
                //    {
                //        installedFiles.Add(TargetLocation + strZippedFile);
                //    }
                //    else
                //    {
                //        installedFiles.Add(TargetLocation + @"\" + strZippedFile);
                //    }

                //    if (strZippedFile != String.Empty)
                //    {
                //        FileStream streamWriter = System.IO.File.Create(TargetLocation + @"\" + zippedFile.Name);
                //        Debug.Print(TargetLocation + @"\" + zippedFile.Name);

                //        //write file from stream
                //        int size = 2048;
                //        byte[] data = new byte[2048];
                //        while (true)
                //        {
                //            size = zipStream.Read(data, 0, size);
                //            if (size > 0)
                //            {
                //                streamWriter.Write(data, 0, size);
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }

                //        //close the writer
                //        streamWriter.Close();
                //        streamWriter = null;
                //    }
                //}

                ////get rid of the temp install file
                //zipStream.Close();
                //zippedFile = null;
                //frame.DeleteFile(TargetLocation + @"\" + ArchiveFile);

                DirectoryInfo di = new DirectoryInfo(TargetLocation);
                FileInfo[] rgFiles = di.GetFiles("*.exe");
                foreach (FileInfo fi in rgFiles)
                {
                    ExeFile = fi.Name;
                }
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: InflateInstallFile)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: InflateInstallFile", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }

            return true;
        }

        private string InstallUpgradeApp(string strTargetLocation, string strApplication, string InstallFile, out string ExeFile,
            string Install_Upgrade, bool blAddExeNameToDisplay, ref bool blSuccessful)
        {
            ExeFile = null;
            try
            {

                if (Install_Upgrade == "Install" &&
                    Directory.Exists(strTargetLocation))
                {
                    //give the user feedback
                    if (MessageBox.Show("Installation folder exists." +
                         Environment.NewLine + "Do you want to replace " +
                         "all modules in this folder?",
                         "Server Installation Warning",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return null;
                    }
                    else
                    {
                        Install_Upgrade = "Upgrade";
                    }
                }

                //get into the installer_files table
                DataTable dtInstallFiles = null;

                if (Install_Upgrade == "Install")
                {
                    dtInstallFiles = frame.installer_data.Tables["install_file"];
                }
                else
                {
                    dtInstallFiles = frame.installer_data.Tables["upgrade_file"];
                }

                //get the client data rows
                DataRow[] drClientFiles = null;
                if (frame._64bit == false)
                {
                    drClientFiles = dtInstallFiles.Select("Application = '" + strApplication + "' AND platform = '32-bit'");
                }
                else
                {
                    drClientFiles = dtInstallFiles.Select("Application = '" + strApplication + "' AND platform = '64-bit'");
                }

                //user feedback
                Application.DoEvents();
				// Begin TT#1668 - JSmith - Install Log
                frame.SetStatusMessage("Creating directory structure.");
				// End TT#1668

                //create the sub folders
                if (Install_Upgrade == "Install")
                {
                    //Directory.CreateDirectory(strTargetLocation);
                    frame.CreateDirectory(strTargetLocation);
                }

                for (int intRow = 0; intRow <= drClientFiles.GetUpperBound(0); intRow++)
                {
                    //get values 
                    string strFolder = drClientFiles[intRow].Field<string>("folder");
                    string strZipFile = drClientFiles[intRow].Field<string>("name");


                    if (strFolder == "ConfigFile")
                    {
                        //inflate files to config folder
                        blSuccessful = InflateInstallFile(strFolder, strZipFile, strTargetLocation, InstallFile, out ExeFile, blAddExeNameToDisplay);
                    }
                    else if (strFolder == "Main")
                    {
                        //inflate files to program root
                        blSuccessful = InflateInstallFile(strFolder, strZipFile, strTargetLocation, InstallFile, out ExeFile, blAddExeNameToDisplay);
                    }
                    else
                    {
                        //inflate files to program sub folder
                        blSuccessful = InflateInstallFile(strFolder, strZipFile, strTargetLocation, InstallFile, out ExeFile, blAddExeNameToDisplay);
                    }

                    if (!blSuccessful)
                    {
                        break;
                    }
                }

                // Begin TT#932 - JSmith - Security violation update computations
                if (blSuccessful)
                {
                    frame.UpdateSecurity(strTargetLocation, false);
                }
                // End TT#932

                return strTargetLocation;
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: InstallUpgradeApp)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: InstallUpgradeApp", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();

                return "";
            }
            
        }

        private void RegisterInstallation(string TargetLocation, string strAppAbbr, 
            string strExeFile, eEntryType intAppIdx, bool bUpgrade)
        {
            //get the number of currently installed clients
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey, true);

            if (mid_key != null)
            {
                //counters
                int intClients = 0;
                int intConfigs = 0;

                //get the sub key collection
                string[] SubKeyNames = mid_key.GetSubKeyNames();

                if (!bUpgrade)
                {
                    //loop thru the subkeys and count them to know what to name the new keys
                    foreach (string SubKeyName in SubKeyNames)
                    {
                        if (SubKeyName.Contains(strAppAbbr) == true)
                        {
                            intClients++;
                        }

                        if (SubKeyName.Contains("MIDConfig") == true)
                        {
                            intConfigs++;
                        }
                    }
                }

                //set the key names
                string strClientName = "";
                string strConfigName = "";
                if (intClients == 0)
                {
                    strClientName = strAppAbbr;
                }
                else
                {
                    strClientName = strAppAbbr + intClients.ToString().Trim();
                    
                }

                if (intConfigs == 0)
                {
                    strConfigName = "MIDConfig";
                }
                else
                {
                    strConfigName = "MIDConfig" + intConfigs.ToString().Trim();
                }

                if (intAppIdx == eEntryType.MIDConfig)
                {
                    //set config values
                    RegistryKey config_key = mid_key.CreateSubKey(strConfigName);
                    config_key.SetValue("EntryType", intAppIdx.ToString().Trim());
                    config_key.SetValue("InstallType", "MIDRetail");
                    config_key.SetValue("Location", TargetLocation + @"\" + ConfigurationFileName);
                    config_key.SetValue("Client", strClientName);

                    // Begin TT#1668 - JSmith - Install Log
                    frame.LogRegistryItem(config_key);
					// End TT#1668
                }
                else
                {
                    //set client values
                    RegistryKey client_key = mid_key.CreateSubKey(strClientName);
                    client_key.SetValue("EntryType", intAppIdx.ToString().Trim());
                    client_key.SetValue("InstallType", "MIDRetail");
                    if (intAppIdx == eEntryType.MIDAPI)
                    {
                        client_key.SetValue("Location", TargetLocation);
                    }
                    else
                    {
                        client_key.SetValue("Location", TargetLocation + @"\" + strExeFile);
                    }

                    //set the inventory list in registry
                    string fileRegKey = "";
                    foreach (string installedFile in installedFiles)
                    {
                        fileRegKey += installedFile + ";";
                    }
                    client_key.SetValue("InstalledFiles", fileRegKey);

                    // Begin TT#1668 - JSmith - Install Log
                    frame.LogRegistryItem(client_key);
					// End TT#1668
                }
            }
            else
            {
                //create the mid product key
                mid_key = soft_key.CreateSubKey(InstallerConstants.cRegistryRootKey);

                //create the client key
                RegistryKey client_key = mid_key.CreateSubKey(strAppAbbr);
                //RegistryKey config_key = mid_key.CreateSubKey("MIDConfig");

                //set client values
                client_key.SetValue("EntryType", intAppIdx.ToString().Trim());
                client_key.SetValue("InstallType", "MIDRetail");
                //client_key.SetValue("Location", TargetLocation + @"\" + strExeFile);
                if (intAppIdx == eEntryType.MIDAPI)
                {
                    client_key.SetValue("Location", TargetLocation);
                }
                else
                {
                    client_key.SetValue("Location", TargetLocation + @"\" + strExeFile);
                }

                //set config values
                //config_key.SetValue("EntryType", "8");
                //config_key.SetValue("InstallType", "MIDRetail");
                //config_key.SetValue("Location", TargetLocation + @"\" + strApplication + @"\" + ConfigurationFileName);
                //config_key.SetValue("Client", strAppAbbr);

                //set the inventory list in registry
                string fileRegKey = "";
                foreach (string installedFile in installedFiles)
                {
                    fileRegKey += installedFile + ";";
                }
                client_key.SetValue("InstalledFiles", fileRegKey);

                // Begin TT#1668 - JSmith - Install Log
                frame.LogRegistryItem(client_key);
				// End TT#1668
            }

            // set registry entry
            // BEGIN TT#1283
            frame.RO.InstallFolder.data = "";
            frame.RO.InstallFolder.name = TargetLocation.ToString();

            RegistryOperations.Uninstall _uninstall = new RegistryOperations.Uninstall();
            _uninstall.DisplayName = "MIDRetail, Inc.";
            _uninstall.DisplayVersion = "";
            _uninstall.DisplayIcon = txtInstallFolder.Text + @"\Client\Graphics\MIDRetail.ico,0";
            _uninstall.EstimatedSize = 86018;   //  compute this size
            _uninstall.InstallDate = String.Format("{0: yyyyMMdd}", DateTime.Now.ToString());  // YYYYMMDD
            _uninstall.InstallLocation = txtInstallFolder.Text + "\\";
            _uninstall.InstallSource = Application.StartupPath.ToString() + "\\";  //  MIDAdvInstaller file location
            _uninstall.Language = 1033;
            _uninstall.Publisher = "MIDRetail, Inc.";
            _uninstall.UninstallLocation = txtInstallFolder.Text + "\\";
            _uninstall.UninstallString = "";    //  MDAdvInstaller file location
            _uninstall.Version = frame.ProductVersion;
            _uninstall.VersionMajor = 0;
            _uninstall.VersionMinor = 0;
            _uninstall.WindowsInstaller = false;    //  Windows Installer Used?
            frame.RO.InstallApplication(_uninstall, strAppAbbr);

            // END TT#1283
        }

        private void ServiceInstall(string ExeFileName, string strTargetFolder, bool startService, bool setStartAuto)
        {
            try
            {
                //peel the service name out of the exe file name
                char[] delim = ".".ToCharArray();
                string[] ExeFileParts = ExeFileName.Split(delim);
                string ServiceName = ExeFileParts[0].ToString().Trim();

                if (frame.StopService(ServiceName, true))
                {
                    //user acknowledgement
					// Begin TT#1668 - JSmith - Install Log
					//frame.lblStatus.Text = "Installing: " + ServiceName;
                    frame.SetStatusMessage("Installing: " + ServiceName);
					// End TT#1668
                    Application.DoEvents();

                    //install the service
                    Process servInstall = new Process();
                    //servInstall.StartInfo.FileName = Application.StartupPath + @"\InstallUtil.exe";
                    //servInstall.StartInfo.FileName = sInstallerLocation + @"\InstallUtil.exe";
                    servInstall.StartInfo.FileName = sInstallerLocation;
                    servInstall.StartInfo.Arguments = (char)34 + strTargetFolder + @"\" + ExeFileName + (char)34;
                    servInstall.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    servInstall.Start();
                    servInstall.WaitForExit();
                    int servInstall_Result = servInstall.ExitCode;
                    servInstall.Close();

                    //if (servInstall_Result == -1)
                    //{
                    //    string log_file = (strTargetFolder + @"\" + ExeFileName).Substring(0, (strTargetFolder + @"\" + ExeFileName).Length - 4) + ".InstallLog";
                    //    throw new Exception("Service installation failed. Check " + log_file + "for details.");
                    //}

                    // Begin TT#1668 - JSmith - Install Log
                    string msg = frame.GetText("serviceStartType");
					// End TT#1668
                    //configure the service as an auto startup
                    string computer_name = Environment.MachineName.ToString().Trim();
                    Process servAutostart = new Process();
                    servAutostart.StartInfo.FileName = System.Environment.GetEnvironmentVariable("windir") + @"\system32\sc.exe";
                    if (setStartAuto)
                    {
                        servAutostart.StartInfo.Arguments = @"\\" + computer_name + " config " + ServiceName + " start= auto";
						// Begin TT#1668 - JSmith - Install Log
                        msg = msg.Replace("{0}", "automatic");
						// End TT#1668
                    }
                    else
                    {
                        servAutostart.StartInfo.Arguments = @"\\" + computer_name + " config " + ServiceName + " start= demand";
						// Begin TT#1668 - JSmith - Install Log
                        msg = msg.Replace("{0}", "manual");
						// End TT#1668
                    }
                    // Begin TT#330-MD - JSmith - Add description to MIDRetail client and all services so displays in Task Manager and Services.
                    servAutostart.StartInfo.Arguments += @" displayname= """ + frame.GetText(ServiceName + "Name") + @" "" ";
                    // End TT#330-MD - JSmith - Add description to MIDRetail client and all services so displays in Task Manager and Services.

					// Begin TT#1668 - JSmith - Install Log
                    frame.SetLogMessage(msg, eErrorType.message);
					// End TT#1668
                    servAutostart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    servAutostart.Start();
                    servAutostart.WaitForExit();
                    servAutostart.Close();

                    // Begin TT#330-MD - JSmith - Add description to MIDRetail client and all services so displays in Task Manager and Services.
                    servAutostart = new Process();
                    servAutostart.StartInfo.FileName = System.Environment.GetEnvironmentVariable("windir") + @"\system32\sc.exe";
                    servAutostart.StartInfo.Arguments = @" description """ + ServiceName + @""" " + @" """ + frame.GetText(ServiceName + "Desc") + @" "" ";
                    servAutostart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    servAutostart.Start();
                    servAutostart.WaitForExit();
                    servAutostart.Close();
                    // End TT#330-MD - JSmith - Add description to MIDRetail client and all services so displays in Task Manager and Services.

                    //start the service
                    if (startService)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//frame.lblStatus.Text = "Starting service: " + ServiceName;
                        frame.SetStatusMessage("Starting service: " + ServiceName);
						// End TT#1668
                        Application.DoEvents();
                        frame.StartService(ServiceName);
                    }
                }
                else
                {
                    log.AddLogEntry("Service " + ServiceName + " could not be stopped.  Service was not upgraded", eErrorType.error);
                    MessageBox.Show("Service " + ServiceName + " could not be stopped.  Service was not upgraded", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: ServiceInstall)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: ServiceInstall", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kill
                Application.Exit();
            }
        }

        private bool ServiceUninstall(string ExeFileName, string ApplicationFolder, string entryType)
        {
            try
            {
                // not a service so exit
                if (entryType == eEntryType.MIDAPI.ToString())
                {
                    if (frame.FileInUse(ApplicationFolder))
                    {
                        // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                       //MessageBox.Show("A module is in use in folder " + ApplicationFolder + "." + Environment.NewLine +
                       //"All access to modules in this folder must be stopped before the uninstall can continue", "Installation Error", MessageBoxButtons.OK,
                       //MessageBoxIcon.Error);
                       string msg = "A module is in use in folder " + ApplicationFolder + "." + Environment.NewLine +
                       "All access to modules in this folder must be stopped and the uninstall reprocessed.";
                       log.AddLogEntry(msg, eErrorType.error);
                       MessageBox.Show(msg, "Installation Error", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                       // End TT#1822-MD - JSmith - Installer not detecting incomplete install
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                //peel the service name out of the exe file name
                char[] delim = ".".ToCharArray();
                string[] ExeFileParts = ExeFileName.Split(delim);
                string ServiceName = ExeFileParts[0].ToString().Trim();

                //user acknowledgement
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = "Stopping and uninstalling service: " + ServiceName;
                frame.SetStatusMessage("Stopping and uninstalling service: " + ServiceName);
				// End TT#1668
                Application.DoEvents();

                if (frame.StopService(ServiceName, false))
                {
                    if (frame.FileInUse(ApplicationFolder))
                    {
                        // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                        // MessageBox.Show("A module is in use in folder " + ApplicationFolder + "." + Environment.NewLine +
                        //"All access to modules in this folder must be stopped before the uninstall can continue", "Installation Error", MessageBoxButtons.OK,
                        //MessageBoxIcon.Error);
                        string msg = "A module is in use in folder " + ApplicationFolder + "." + Environment.NewLine +
                       "All access to modules in this folder must be stopped and the uninstall reprocessed.";
                        log.AddLogEntry(msg, eErrorType.error);
                        MessageBox.Show(msg, "Installation Error", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                        // End TT#1822-MD - JSmith - Installer not detecting incomplete install
                        return false;
                    }
                    else
                    {
                        //uninstall the service
                        Process servInstall = new Process();
                        servInstall.StartInfo.FileName = sInstallerLocation;
                        servInstall.StartInfo.Arguments = "/u " + (char)34 + ApplicationFolder + @"\" + ExeFileName + (char)34;
                        servInstall.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        servInstall.Start();
                        servInstall.WaitForExit();
                    }
                }
                else
                {
                    log.AddLogEntry("Service " + ServiceName + " could not be stopped.  Service was not uninstalled", eErrorType.error);
                    MessageBox.Show("Service " + ServiceName + " could not be stopped.  Service was not uninstalled", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch (Exception err)
            {
                //log action
                log.AddLogEntry("(Server: ServiceUninstall)" + err.Message, eErrorType.error);

                //export the log for issue research
                string strLog = log.ExportLog();

                //tell the user about the file
                MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                    "Check the following log for details: " + strLog + Environment.NewLine +
                    "Process: Server Method: ServiceUninstall", "Installation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //hard kil
                Application.Exit();
            }

            return true;
        }

        private void btnInstallFolder_Click(object sender, EventArgs e)
        {
            //set initial directory
            string initial_dir = txtInstallFolder.Text;
            bool initial_dir_exists = true;

            //create the initial directory if it doesn't exist
            if (initial_dir == "")
            {
                initial_dir = "C:";
            }
            if (Directory.Exists(initial_dir) == false)
            {
                //Directory.CreateDirectory(initial_dir);
                frame.CreateDirectory(initial_dir);
                initial_dir_exists = false;
            }

            //select the initial directory
            folderInstallFolder.SelectedPath = initial_dir;

            //set the install directory
            string install_dir = "";
            if (DialogResult.OK == folderInstallFolder.ShowDialog())
            {
                folderInstallFolder.ShowNewFolderButton = true;
                install_dir = folderInstallFolder.SelectedPath;

            }

            //create the directory and clean-up the initial if not used
            if (initial_dir != install_dir)
            {
                if (initial_dir_exists == false)
                {
                    frame.DeleteFolder(initial_dir);
                }

                InstallFolder = install_dir;

                txtInstallFolder.Text = install_dir;
            }
            else
            {
                InstallFolder = initial_dir;
            }
        }

        private void btnConfigurationInstallFolder_Click(object sender, EventArgs e)
        {
            //set initial directory
            string initial_dir = txtInstallFolder.Text;
            bool initial_dir_exists = true;

            //create the initial directory if it doesn't exist
            if (initial_dir == "")
            {
                initial_dir = "C:";
            }
            if (Directory.Exists(initial_dir) == false)
            {
                //Directory.CreateDirectory(initial_dir);
                frame.CreateDirectory(initial_dir);
                initial_dir_exists = false;
            }

            //select the initial directory
            folderInstallFolder.SelectedPath = initial_dir;

            //set the install directory
            string install_dir = "";
            if (DialogResult.OK == folderInstallFolder.ShowDialog())
            {
                folderInstallFolder.ShowNewFolderButton = true;
                install_dir = folderInstallFolder.SelectedPath;

            }

            ConfigurationInstallFolder = install_dir;

            txtConfigurationInstallFolder.Text = install_dir;
        }

        public void SetRadioButtons()
        {
            if (lstInstalledServices.Items.Count > 0)
            {
                rdoInstalledServerTasks.Enabled = true;
                rdoInstallTypical.Enabled = false;
            }
            else
            {
                rdoInstalledServerTasks.Enabled = false;
                rdoInstallTypical.Enabled = true;
            }
        }

        private void SetButton()
        {
            if (cboTasks.Text == "Configure")
            {
                frame.InstallTask = eInstallTasks.configure;
                ConfigureNext(this, new EventArgs());
            }
            else
            {
                frame.InstallTask = eInstallTasks.uninstall;
                NotConfigureNext(this, new EventArgs());
            }
        }

        private void rdoUseMachineName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUseMachineName.Checked == true)
            {
                frame.SetStatusMessage(frame.GetText("SelectConfigureByMachine"));

                configureBy = eConfigMachineBy.Name;
            }
        }

        private void rdoUseIPAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUseIPAddress.Checked == true)
            {
                frame.SetStatusMessage(frame.GetText("SelectConfigureByIP"));

                configureBy = eConfigMachineBy.IP;
            }
        }

        private void ucServer_VisibleChanged(object sender, EventArgs e)
        {

            if (Visible)
            {
                SetRadioButtons();
                SetButton();
                //NotConfigureNext(this, new EventArgs());
                frame.Back_Enabled = true;
            }
        }

        private void lstInstalledServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                // Begin TT#74 MD - JSmith - One-button Upgrade
                //for (int i = 0; i < lstInstalledServices.Items.Count; i++)
                //{
                //    lstInstalledServices.SetSelected(i, true);
                //}
                SelectAll();
                // End TT#74 MD
            }
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        public bool SelectAll()
        {
            bool successful = true;
            for (int i = 0; i < lstInstalledServices.Items.Count; i++)
            {
                lstInstalledServices.SetSelected(i, true);
            }
            return successful;
        }
        // End TT#74 MD
    }
}
