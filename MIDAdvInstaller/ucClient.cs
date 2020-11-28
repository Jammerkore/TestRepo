using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System.Threading;
//  Begin TT#1547 - GRT - Remove PDB files
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
//  End TT#1547 - GRT - Remove PDB files

using IWshRuntimeLibrary;
using Shell32;


namespace MIDRetailInstaller
{
    public partial class ucClient : UserControl
    {
        // Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        private ShellShortcut m_Shortcut;
        // End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

        //public events
        public event EventHandler ConfigureNext;
        public event EventHandler NotConfigureNext;

        ToolTip tt = new ToolTip();

        //object to pass frame to
        InstallerFrame frame = null;

        //install folder string
        string InstallFolder = "";

        string ConfigurationFileName;
        string ApplicationIcon;
        string globalConfigurationLocation = null;

        //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
        //shell object
        //private WshShellClass WshShell;
        //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010


        //installed file list
        List<string> installedFiles;

        //ShortCut Types
        ShortCutAlert alert;

        ucInstallationLog log;

        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        public string UserDesktop;
        public string UserPrograms;
        public string AllUserDesktop;
        public string AllUserPrograms;
        //End TT#883

        bool bInstallingAutoUpgradeClient = false;

        public bool BInstallingAutoUpgradeClient
        {
            get
            {
                return bInstallingAutoUpgradeClient;
            }
            set
            {
                blAutoUpgrade = value;
                bInstallingAutoUpgradeClient = value;
            }
        }

        public ucClient(InstallerFrame p_frame, ucInstallationLog p_log)
        {
            InitializeComponent();

            StringBuilder sbPath = new StringBuilder(260); // TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //pass the installer frame here
            frame = p_frame;
            frame.help_ID = "client";
            log = p_log;
            ConfigurationFileName = ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
            txtInstallFolder.Text = ConfigurationManager.AppSettings["ClientInstallLocation"].ToString();
            ApplicationIcon = ConfigurationManager.AppSettings["APPLICATION_ICON"].ToString();
            txtShareName.Text = ConfigurationManager.AppSettings["ShareName"].ToString();
            txtShortcutName.Text = ConfigurationManager.AppSettings["ShortcutName"].ToString();

            lblShareName.Text = frame.GetText("lblShareName");
            lblShortcutName.Text = frame.GetText("lblShortcutName");
            lblSelectGlobalConfig.Text = frame.GetText("lblSelectGlobalConfig");
            btnGlobalConfigurationBrowseTypical.Text = frame.GetText("btnBrowse");
            btnGlobalConfigurationBrowse.Text = frame.GetText("btnBrowse");
            chxShareClient.Text = frame.GetText("chxShareClient");
            chkAutoUpgradeClient.Text = frame.GetText("chkAutoUpgradeClient");
            lblAccess.Text = frame.GetText("lblAccess");
            rdoMe.Text = frame.GetText("rdoMe");
            rdoEveryone.Text = frame.GetText("rdoEveryone");
            chkQuickLaunch.Text = frame.GetText("chkQuickLaunch");
            chkDesktop.Text = frame.GetText("chkDesktop");
            lblInstallDir.Text = frame.GetText("lblInstallDir");
            btnInstallFolder.Text = frame.GetText("btnChangeDirectory");
            lblTasks.Text = frame.GetText("lblTasks");
            lblSelectGlobalConfigTypical.Text = frame.GetText("lblSelectGlobalConfigTypical");


            tt.SetToolTip(chkDesktop, frame.GetToolTipText("client_desktop"));
            tt.SetToolTip(chkQuickLaunch, frame.GetToolTipText("client_quicklaunch"));
            tt.SetToolTip(lblInstallDir, frame.GetToolTipText("client_installfolder"));
            tt.SetToolTip(txtInstallFolder, frame.GetToolTipText("client_installfolder"));
            tt.SetToolTip(btnInstallFolder, frame.GetToolTipText("client_installfolderButton"));
            tt.SetToolTip(rdoEveryone, frame.GetToolTipText("client_accesseveryone"));
            tt.SetToolTip(rdoMe, frame.GetToolTipText("client_accessme"));
            tt.SetToolTip(chkAutoUpgradeClient, frame.GetToolTipText("client_autoupgrade"));
            tt.SetToolTip(txtShareName, frame.GetToolTipText("client_ShareName"));
            tt.SetToolTip(txtShortcutName, frame.GetToolTipText("client_ShortcutName"));


            tt.SetToolTip(rdoInstallClientTasks, frame.GetToolTipText("client_InstallClientTasks"));
            tt.SetToolTip(cboTasks, frame.GetToolTipText("client_ClientTasks"));
            tt.SetToolTip(lstInstalledClients, frame.GetToolTipText("client_Clients"));

            rdoInstallTypical.Text = frame.GetText("rdoInstallTypicalClient");
            rdoInstallClient.Text = frame.GetText("rdoInstallClient");
            rdoInstallClientTasks.Text = frame.GetText("rdoInstallClientTasks");

            //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
            //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
            //WshShell shell = new WshShellClass();
            //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
            //switch (frame.OSType)
            //{
            //    case eOSType.Windows7:
            //    case eOSType.WindowsServer2008R2:
            //        chkQuickLaunch.Visible = false;
            //        chkQuickLaunch.Checked = false;
            //        break;
            //    default:
            //        break;
            //}
            // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

            //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
                //object allUsers = "Desktop";
                //UserDesktop = shell.SpecialFolders.Item(ref allUsers).ToString();

            SHGetFolderPath(IntPtr.Zero, (int)ShellShortcut.CSIDL.CSIDL_DESKTOP, IntPtr.Zero, 0, sbPath);
            UserDesktop = sbPath.ToString();

                //allUsers = "AllUsersDesktop";
                //AllUserDesktop = shell.SpecialFolders.Item(ref allUsers).ToString();

            SHGetFolderPath(IntPtr.Zero, (int)ShellShortcut.CSIDL.CSIDL_COMMON_DESKTOPDIRECTORY, IntPtr.Zero, 0, sbPath);
            AllUserDesktop = sbPath.ToString();

                //allUsers = "Programs";
                //UserPrograms = shell.SpecialFolders.Item(ref allUsers).ToString();

            SHGetFolderPath(IntPtr.Zero, (int)ShellShortcut.CSIDL.CSIDL_PROGRAMS, IntPtr.Zero, 0, sbPath);
            UserPrograms = sbPath.ToString();

                //allUsers = "AllUsersPrograms";
                //AllUserPrograms = shell.SpecialFolders.Item(ref allUsers).ToString();

            SHGetFolderPath(IntPtr.Zero, (int)ShellShortcut.CSIDL.CSIDL_COMMON_PROGRAMS, IntPtr.Zero, 0, sbPath);
            AllUserPrograms = sbPath.ToString();

                //End TT#883
            //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
        }

        public bool isItemSelected
        {
            get
            {
                if (rdoInstallClient.Checked)
                {
                    return true;
                }
                else
                {
                    return lstInstalledClients.SelectedItems.Count > 0;
                }
            }
        }

        private void ucClient_Load(object sender, EventArgs e)
        {
            // Begin TT#74 MD - JSmith - One-button Upgrade
            ////get a list of the installed clients
            //List<string> installedClients = GetInstalledClients();

            ////loop thru the clients and add them to the list
            //lstInstalledClients.Items.Clear();
            //foreach (string installedClient in installedClients)
            //{
            //    lstInstalledClients.Items.Add(installedClient);
            //}
            LoadInstalledClients();
            // End TT#74 MD

            //don't allow installed client tasks if none are installed
            if (lstInstalledClients.Items.Count == 0)
            {
                rdoInstallClientTasks.Enabled = false;
                rdoInstallTypical.Checked = true;
                gbxTypical.Enabled = true;
                frame.InstallTask = eInstallTasks.typicalInstall;
            }
            else
            {
                rdoInstallTypical.Enabled = false;
                gbxTypical.Enabled = false;
                rdoInstallClient.Checked = true;
                frame.InstallTask = eInstallTasks.install;
            }

            txtShareName.Enabled = false;
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        public bool LoadInstalledClients()
        {
            bool successful = true;
            //get a list of the installed clients
            List<string> installedClients = GetInstalledClients();

            //loop thru the clients and add them to the list
            lstInstalledClients.Items.Clear();
            foreach (string installedClient in installedClients)
            {
                lstInstalledClients.Items.Add(installedClient);
            }
            return successful;
        }
        // End TT#74 MD

        private List<string> GetInstalledClients()
        {
            //return variable
            List<string> lstReturn = new List<string>();

            //drill down into the registry to our stuff
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);
            if (mid_key != null)
            {
                string[] SubKeyNames = mid_key.GetSubKeyNames();

                //loop thru the subkeys
                foreach (string SubKeyName in SubKeyNames)
                {
                    if (SubKeyName.Contains(InstallerConstants.cClientKey) == true)
                    {
                        //open the client key
                        RegistryKey sub_key = mid_key.OpenSubKey(SubKeyName);

                        //get the location
                        object location = sub_key.GetValue("Location");
                        if (location == null)
                        {
                            continue;
                        }
                        //lstInstalledClients.Items.Add(sub_key.GetValue("Location").ToString().Trim());
                        lstReturn.Add(sub_key.GetValue("Location").ToString().Trim());
                    }
                }
            }

            //return value
            return lstReturn;
        }

        private void RemoveOurRegKey(string RegKeyName)
        {
            //drill down into the application installation reg key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);

            //remove the key
            mid_key.DeleteSubKey(RegKeyName);
        }

        private void rdoInstallClientTasks_CheckedChanged(object sender, EventArgs e)
        {
            //raise event
            switch (cboTasks.Text)
            {
                case "Upgrade":
                case "Configure":
                    NotConfigureNext(this, new EventArgs());
                    break;
                case "Auto Upgrade Client":
                case "Uninstall":
                    NotConfigureNext(this, new EventArgs());
                    break;
            }

            //disable the installed client task controls
            if (rdoInstallClientTasks.Checked == true)
            {
                //enable the needed controls
                grpInstalledClients.Enabled = true;
                grpClient.Enabled = false;
                gbxTypical.Enabled = false;

                //select the default row
                // Begin TT#2788 - JSmith - Installer Error on "Back" button
                //lstInstalledClients.SelectedIndex = 0;
                //frame.UninstallFile = lstInstalledClients.SelectedItem.ToString().Trim();
                if (lstInstalledClients.Items.Count > 0)
                {
                    lstInstalledClients.SelectedIndex = 0;
                    frame.UninstallFile = lstInstalledClients.Items[0].ToString().Trim();
                }
                else
                {
                    frame.UninstallFile = string.Empty;
                }
                // End TT#2788 - JSmith - Installer Error on "Back" button

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.upgrade;
            }
        }

        private void lstInstalledClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the uninstall file name on the frame
            frame.UninstallFile = lstInstalledClients.SelectedItem.ToString().Trim();
        }

        private void cboTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the installed component task
            SetButton();
        }

        private void rdoInstallClient_CheckedChanged(object sender, EventArgs e)
        {
            //raise event
            NotConfigureNext(this, new EventArgs());

            //disable the installed client task controls
            if (rdoInstallClient.Checked == true)
            {
                //enable the needed controls
                grpInstalledClients.Enabled = false;
                grpClient.Enabled = true;
                gbxTypical.Enabled = false;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.install;
            }
        }

        private void rdoInstallTypical_CheckedChanged(object sender, EventArgs e)
        {
            //raise event
            NotConfigureNext(this, new EventArgs());

            //disable the installed client task controls
            if (rdoInstallTypical.Checked == true)
            {
                //enable the needed controls
                gbxTypical.Enabled = true;
                grpInstalledClients.Enabled = false;
                grpClient.Enabled = false;

                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.typicalInstall;
            }
        }

        private void InflateInstallFile(string FolderName, string ArchiveFile, string TargetLocation, out string ApplicationFile)
        {
            //installedFiles = new List<string>();

            //initialize the out parm
            ApplicationFile = "";
            DirectoryInfo directoryInfo;
            FileInfo[] files;

            try
            {
                frame.CopyFolder(Application.StartupPath + @"\Install Files\" + FolderName, TargetLocation, installedFiles);
                directoryInfo = new DirectoryInfo(TargetLocation);
                //files = directoryInfo.GetFiles("*.exe");
                files = directoryInfo.GetFiles(InstallerConstants.cClientApp);
                if (files.Length > 0)
                {
                    ApplicationFile = files[0].Name;
                }

                foreach (FileInfo file in files)
                {
                    //  Begin TT#1547 - DOConnell - Remove PDB files
                    if (file.Extension.Equals(".pdb", StringComparison.CurrentCultureIgnoreCase))
                        file.Delete();
                    //  End TT#1547 - DOConnell - Remove PDB files
                }

               // //copy file
               // System.IO.File.Copy(Application.StartupPath + @"\Install Files\" +
               //     ArchiveFile, TargetLocation + @"\" + ArchiveFile, true);

               // //Archive temp file stream
               // ZipInputStream zipStream = new ZipInputStream(System.IO.File.Open(TargetLocation +
               //     @"\" + ArchiveFile, FileMode.Open));

               // //the zip entry
               // ZipEntry zippedFile;

               // //loop thru the files in the archive and make the directories
               // while ((zippedFile = zipStream.GetNextEntry()) != null)
               // {
               //     if (zippedFile.IsDirectory == true)
               //     {
               //         //Directory.CreateDirectory(TargetLocation + @"\" + zippedFile.Name);
               //         frame.CreateDirectory(TargetLocation + @"\" + zippedFile.Name);
               //     }
               // }

               // //Archive temp file stream
               // zipStream = new ZipInputStream(System.IO.File.Open(TargetLocation +
               //     @"\" + ArchiveFile, FileMode.Open));

               // //the zip entry
               // zippedFile = null;

               // //loop thru the files in the archive
               // while ((zippedFile = zipStream.GetNextEntry()) != null)
               // {
               //     if (zippedFile.IsFile == true)
               //     {
               //         string strZippedFile = zippedFile.Name.Replace("/", @"\");

               //         log.AddLogEntry("Decompressing file: " + strZippedFile, eErrorType.message);

               //         //check for executable to return for use in registering client
               //         if (strZippedFile.EndsWith(".exe") == true)
               //         {
               //             ApplicationFile = strZippedFile;
               //         }

               //         //update the install message
               //         Application.DoEvents();
               //         frame.SetStatusMessage("Copying file: " + strZippedFile + " to " + TargetLocation;

               //         //fill the file inventory list to
               //         if (TargetLocation.EndsWith(@"\") == true)
               //         {
               //             installedFiles.Add(TargetLocation + strZippedFile);
               //         }
               //         else
               //         {
               //             installedFiles.Add(TargetLocation + @"\" + strZippedFile);
               //         }

               //         if (strZippedFile != String.Empty)
               //         {
               //             if (System.IO.File.Exists(TargetLocation + @"\" + zippedFile.Name))
               //             {
               //                 System.IO.File.SetAttributes(TargetLocation + @"\" + zippedFile.Name, System.IO.File.GetAttributes(TargetLocation + @"\" + zippedFile.Name) & ~(FileAttributes.ReadOnly));
               //                 frame.DeleteFile(TargetLocation + @"\" + zippedFile.Name);
               //             }

               //             FileStream streamWriter = System.IO.File.Create(TargetLocation +
               //                 @"\" + zippedFile.Name);
               //             Debug.Print(TargetLocation + @"\" + zippedFile.Name);

               //             //write file from stream
               //             int size = 2048;
               //             byte[] data = new byte[2048];
               //             while (true)
               //             {
               //                 size = zipStream.Read(data, 0, size);
               //                 if (size > 0)
               //                 {
               //                     streamWriter.Write(data, 0, size);
               //                 }
               //                 else
               //                 {
               //                     break;
               //                 }
               //             }

               //             //close the writer
               //             streamWriter.Close();
               //             streamWriter = null;
               //         }
               //     }
               // }

               // //get rid of the temp install file
               // zipStream.Close();
               // zippedFile = null;
               //frame.DeleteFile(TargetLocation + @"\" + ArchiveFile);
            }
            catch (Exception err)
            {
                frame.ErrorHandler(err.Message);
            }

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

        private void btnGlobalConfigurationBrowse_Click(object sender, EventArgs e)
        {
            ofdBrowser.FileName = ConfigurationFileName;
            if (ofdBrowser.ShowDialog() == DialogResult.OK)
            {
                txtGlobalConfigurationFile.Text = ofdBrowser.FileName;
                globalConfigurationLocation = txtGlobalConfigurationFile.Text;
            }
        }

        private void btnGlobalConfigurationBrowseTypical_Click(object sender, EventArgs e)
        {
            ofdBrowser.FileName = ConfigurationFileName;
            if (ofdBrowser.ShowDialog() == DialogResult.OK)
            {
                txtGlobalConfigurationFileTypical.Text = ofdBrowser.FileName;
                globalConfigurationLocation = txtGlobalConfigurationFileTypical.Text;
            }
        }

        public bool Upgrade(bool AutoUpgradeClient, bool bl64bit)
        {
            //return variable
            bool blreturn = true;
            ConfigFiles config;

            try
            {
                // check for prerequisites
                ucUtilities utilities = new ucUtilities(frame, log, false, false);
                if (!utilities.IsCrystalReportsInstalled(log))
                {
                    log.AddLogEntry("Crystal Reports must be installed.", eErrorType.message);
                    utilities.InstallCrystalReports(true);
                }

                // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                ArrayList slClientsToUpgrade = new ArrayList();

                foreach (string lstInstalledClient in lstInstalledClients.SelectedItems)
                {
                    slClientsToUpgrade.Add(lstInstalledClient);
                }

                //set up the progress bar
                if (!frame.blPerformingOneClickUpgrade &&
                    !frame.blPerformingTypicalInstall)
                {
                    frame.ProgressBarSetMinimum(0);
                    frame.ProgressBarSetMaximum(slClientsToUpgrade.Count + 2);
                    frame.ProgressBarIncrementValue(1);
                }

                foreach (string lstInstalledClient in slClientsToUpgrade)
                {
                    //enable progress controls
                    frame.ProgressBarEnabled(true);
                    frame.lblStatus.Enabled = true;

                    //get install target location and create
                    string strTargetDrive = Directory.GetDirectoryRoot(lstInstalledClient);
                    string strTargetLocation = Directory.GetParent(lstInstalledClient).ToString().Trim();
                    string strTargetParent = Directory.GetParent(strTargetLocation).ToString().Trim();
                    string strAppDir = Directory.GetParent(strTargetParent).ToString().Trim();

                    // Perform delete of any unneeded files
                    ModuleCleanup mc = new ModuleCleanup();
                    mc.RemoveOldFiles(strTargetLocation, true);    // TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder


                    // copy all config files to use later
                    config.MakeConfigBackup(strTargetLocation);

                    //get the existing registry keys
                    string strRegKey = "";
                    string strConfigKey = "";
                    RegistryKey appRegKey = GetExistingRegKey(lstInstalledClient, out strRegKey, out strConfigKey);

                    if ((string)appRegKey.GetValue("InstallType") == "Windows")
                    {
                        Uninstall((string)appRegKey.GetValue("Location"), false);
                        //do the install
                        alert = new ShortCutAlert();
                        DoInstall(strTargetLocation, "", true, frame._64bit, false, false, strTargetDrive[0].ToString());
                    }
                    else
                    {
                        //inflate the upgrade file
                        string ApplicationFile = "";
                        if (InstallUpgradeApp(strTargetParent, strTargetLocation, "Upgrade", out ApplicationFile))
                        {
                            //drill into the registry
                            RegistryKey local_key = Registry.LocalMachine;
                            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);

                            //register client
                            RegClient(strRegKey, strConfigKey, mid_key, strTargetLocation, "Upgrade", ApplicationFile, AutoUpgradeClient, bl64bit);
                        }
                    }

                    // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
                    UpdateLinks(appRegKey);
                    frame.RefreshDesktop();
                    // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization

                    // Begin TT#1668 - JSmith - Install Log
                    frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
					// End TT#1668
                    // Begin TT#1729 - JSmith - Scheduler Service config overwrites previous configuration settings
                    config.ReplaceDefaultsInConfigFiles(strTargetLocation, strAppDir, globalConfigurationLocation, eConfigMachineBy.Name, strTargetDrive);
                    // End TT#1729

                    config.UpgradeConfigFiles(strTargetLocation, strAppDir, false);

                    // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                    // Begin TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
                    //ModuleCleanup mc = new ModuleCleanup();
                    mc.RemoveOldFiles(strTargetLocation, false);
                    // End TT#1392-MD - stodd - During installalation remove MIDRetail.Windows.dll from batch folder
					// End TT#1305-MD - JSmith - Change Auto Upgrade

                    if (!frame.blPerformingOneClickUpgrade)
                    {
                        frame.ProgressBarSetToMaximum();
                    }
                }
            }

            catch (Exception err)
            {

                frame.ErrorHandler(err.Message);
            //    //log action
            //    log.AddLogEntry("(Client: Upgrade)" + err.Message, eErrorType.error);

            //    //export the log for issue research
            //    string strLog = log.ExportLog();

            //    //tell the user about the file
            //    MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
            //        "Check the following log for details: " + strLog + Environment.NewLine +
            //        "Process: Client Method: Upgrade", "Installation Error", MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    blreturn = false;

            //    //hard kill
            //    Application.Exit();
            }

            //return value
            return blreturn;
        }

        //auto upgrade property
        bool blAutoUpgrade = false;
        public bool AutoUpgrade
        {
            get
            {
                return blAutoUpgrade;
            }
        }

        private RegistryKey GetExistingRegKey(string strFile, out string RegKey, out string ConfigKey)
        {
            //return variable
            string strRegKey = "";
            string strConfigKey = "";
            RegistryKey appRegKey = null;

            //drill into the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);

            if (mid_key != null)
            {
                //open the sub key
                string[] MID_Keys = mid_key.GetSubKeyNames();

                //loop thru the sub keys in the installation
                foreach (string MID_Key in MID_Keys)
                {
                    //open the sub key
                    RegistryKey sub_key = mid_key.OpenSubKey(MID_Key);

                    //check the location key and compare the file names
                    if ((string)sub_key.GetValue("Location") == strFile)
                    {
                        //set the return value
                        strRegKey = sub_key.Name;
                        appRegKey = sub_key;
                    }
                }

                //loop thru the sub keys in the installation
                foreach (string MID_Key in MID_Keys)
                {
                    //open the sub key
                    RegistryKey sub_key = mid_key.OpenSubKey(MID_Key);

                    //check the client key and compare the file names
                    if ((string)sub_key.GetValue("Client") == strRegKey)
                    {
                        //set the return value
                        strConfigKey = sub_key.Name;
                    }
                }
            }

            //get key names
            char[] delim = @"\".ToCharArray();
            string[] RegKeys = strRegKey.Split(delim);
            string[] ConfigKeys = strConfigKey.Split(delim);

            //return keys
            RegKey = RegKeys[RegKeys.GetUpperBound(0)].ToString().Trim();
            ConfigKey = ConfigKeys[ConfigKeys.GetUpperBound(0)].ToString().Trim();

            return appRegKey;
        }

        //Client name
        string strClientName = "";
        public string RegClientName
        {
            get
            {
                return strClientName;
            }
            set
            {
                strClientName = value;
            }
        }

        //config name
        string strConfigName = "";
        public string RegConfigName
        {
            get
            {
                return strConfigName;
            }
            set
            {
                strConfigName = value;
            }
        }

        private void DoInstall(string strTargetLocation, string strShortcutName, bool WantFeedback, bool bl64bit, bool blAutoUpgrade, bool Everyone, string strTargetDrive)
        {
            ConfigFiles config;

            try
            {
                // Begin TT#1120 - JSmith - Custom Client Install doesn't remember global settings file location
                if (globalConfigurationLocation == null)
                {
                    if (rdoInstallTypical.Checked &&
                        txtGlobalConfigurationFileTypical.Text != null &&
                        txtGlobalConfigurationFileTypical.Text.Trim().Length > 0)
                    {
                        globalConfigurationLocation = txtGlobalConfigurationFileTypical.Text.Trim();
                    }
                    else if (rdoInstallClient.Checked &&
                        txtGlobalConfigurationFile.Text != null &&
                        txtGlobalConfigurationFile.Text.Trim().Length > 0)
                    {
                        globalConfigurationLocation = txtGlobalConfigurationFile.Text.Trim();
                    }

                    if (!System.IO.File.Exists(globalConfigurationLocation))
                    {
                        if (MessageBox.Show("The location specified for the global configuration does not exist.  Do you want to continue?", this.Text,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                // End TT#1120

                // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                //disable the ability to pick another location
                btnInstallFolder.Enabled = false;

                string strTargetParent = Directory.GetParent(strTargetLocation).ToString().Trim();
                string strAppDir = Directory.GetParent(strTargetParent).ToString().Trim();

                //Install application
                string ApplicationFile = "";
                if (InstallUpgradeApp(strTargetParent, strTargetLocation, "Install", out ApplicationFile))
                {
                    //create the config files needed
					// Begin TT#1668 - JSmith - Install Log
					//ConfigFiles cf = new ConfigFiles(frame.installer_data, log);
                    ConfigFiles cf = new ConfigFiles(frame, frame.installer_data, log);
					// End TT#1668
                    cf.CopyMIDSettings_config(Application.StartupPath, strTargetLocation);

                    //register application installation
                    RegisterInstallation(strTargetLocation, ApplicationFile, blAutoUpgrade, bl64bit, out strClientName, out strConfigName);

                    //create desktop short cuts
                    if (chkDesktop.Checked == true)
                    {
                        DesktopShortCut(strTargetLocation, ApplicationFile, strShortcutName, false, Everyone, strClientName);
                    }

                    //create quick launch short cut
                    //if (chkQuickLaunch.Checked == true)
                    //{
                    //    QuickLaunchShortCut(strTargetLocation, ApplicationFile, strShortcutName, false, strClientName);
                    //}

                    //Create start menu program group
                    ProgramGroup(strTargetLocation, ApplicationFile, strShortcutName, false, Everyone, strClientName);

                    // Begin TT#1668 - JSmith - Install Log
                    frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
					// End TT#1668
                    config.ReplaceDefaultsInConfigFiles(strTargetLocation, strAppDir, globalConfigurationLocation, eConfigMachineBy.Name, strTargetDrive);

                    //add the exe to the list of available uninstalls
                    lstInstalledClients.Items.Add(strTargetLocation + @"\Client\" + ApplicationFile);

                    //update the install message
                    if (WantFeedback == true)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//frame.lblStatus.Text = "Installation Complete";
                        frame.SetStatusMessage("Client Installation Complete");
						// End TT#1668
                    }
                }
                else
                {
                    if (WantFeedback == true)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//frame.lblStatus.Text = "Installation Terminated";
                        frame.SetStatusMessage("Client Installation Terminated");
						// End TT#1668
                    }
                }
            }

            catch (Exception err)
            {
                frame.ErrorHandler(err.Message);

                ////log action
                //log.AddLogEntry("(Client: DoInstall)" + err.Message, eErrorType.error);

                ////export the log for issue research
                //string strLog = log.ExportLog();

                ////tell the user about the file
                //MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
                //    "Check the following log for details: " + strLog + Environment.NewLine +
                //    "Process: Client Method: DoInstall", "Installation Error", MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);

                ////hard kill
                //Application.Exit();
            }
        }

        private string DoInstall(string strTargetLocation, string strShortcutName, bool WantFeedback, bool bl64bit, bool blAddToList, string strTargetDrive)
        {
            ConfigFiles config;


            try
            {
			    // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668
                //target location
                if (bl64bit == true)
                {
                    strTargetLocation += "64";
                }

                string strTargetParent = Directory.GetParent(strTargetLocation).ToString().Trim();

                //disable the ability to pick another location
                btnInstallFolder.Enabled = false;

                //Install application
                string ApplicationFile = "";
                if (InstallUpgradeApp(strTargetParent, strTargetLocation, "Install", out ApplicationFile))
                {
                    // Begin TT#1268 - JSmith - Folders are not removed during uninstall
                    installedFiles.Add(strTargetLocation + @"\Upgrade.log");
                    // End TT#1268
                    //register application installation
                    RegisterInstallation(strTargetLocation, ApplicationFile, blAutoUpgrade, bl64bit, out strClientName, out strConfigName);

                    if (!bInstallingAutoUpgradeClient)
                    {
                        //create desktop short cuts
                        if (chkDesktop.Checked == true)
                        {
                            DesktopShortCut(strTargetLocation, ApplicationFile, strShortcutName, false);
                        }

                        //create quick launch short cut
                        //if (chkQuickLaunch.Checked == true)
                        //{
                        //    QuickLaunchShortCut(strTargetLocation, ApplicationFile, strShortcutName, false, strClientName);
                        //}

                        //Create start menu program group
                        ProgramGroup(strTargetLocation, ApplicationFile, strShortcutName, false);
                    }

                    // Begin TT#1668 - JSmith - Install Log
                    frame.SetStatusMessage(frame.GetText("applyingSubstitutions"));
					// End TT#1668
                    config.ReplaceDefaultsInConfigFiles(strTargetLocation, strTargetParent, globalConfigurationLocation, eConfigMachineBy.Name, strTargetDrive);

                    //add the exe to the list of available uninstalls
                    if (blAddToList)
                    {
                        lstInstalledClients.Items.Add(strTargetLocation + @"\" + ApplicationFile);
                    }

                    //update the install message
                    if (WantFeedback == true)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//frame.lblStatus.Text = "Installation Complete";
                        frame.SetStatusMessage("Client Installation Complete");
						// End TT#1668
                    }
                }
                else
                {
                    if (WantFeedback == true)
                    {
					    // Begin TT#1668 - JSmith - Install Log
						//frame.lblStatus.Text = "Installation Terminated";
                        frame.SetStatusMessage("Client Installation Terminated");
						// End TT#1668
                    }
                }

                return strTargetLocation + @"\" + ApplicationFile;
            }
            catch (Exception err)
            {
                frame.ErrorHandler(err.Message);
            //    //log action
            //    log.AddLogEntry("(Client: DoInstall)" + err.Message, eErrorType.error);

            //    //export the log for issue research
            //    string strLog = log.ExportLog();

            //    //tell the user about the file
            //    MessageBox.Show("Installation has hit a critical error and will close!" + Environment.NewLine +
            //        "Check the following log for details: " + strLog + Environment.NewLine +
            //        "Process: Client Method: DoInstall", "Installation Error", MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    //hard kill
            //    Application.Exit();

                return "";
            }


        }

        public bool Install(string installFolder, string sharePath, string shareName, string shortcutName, out string strTarget, bool AutoUpgradeClient)
        {
            //initialize the out parameter
            strTarget = "";

            //return variable
            bool blreturn = true;
            bool blInstallExists = false;
            bool bShareClient = false;
            ConfigFiles config;

            try
            {

                // Begin TT#195 MD - JSmith - Add environment authentication
                if (!frame.isValidClient())
                {
                    MessageBox.Show(frame.GetText("unsupportedVersion"), "Unsupported Version Error", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                    log.AddLogEntry(frame.GetText("unsupportedVersion"), eErrorType.message);
                    frame.SetStatusMessage(frame.GetText("unsupportedVersion"));
                    blreturn = false;
                    return blreturn;
                }
                // End TT#195 MD

			    // Begin TT#1668 - JSmith - Install Log
				//config = new ConfigFiles(frame.installer_data, log);
                config = new ConfigFiles(frame, frame.installer_data, log);
				// End TT#1668

                //// check for prerequisites
                //ucUtilities utilities = new ucUtilities(frame, log, false);
                //if (!utilities.IsCrystalReportsInstalled(log))
                //{
                //    log.AddLogEntry("Crystal Reports must be installed.", eErrorType.message);
                //    utilities.InstallCrystalReports(true);
                //}

                //get install target location and create
                string strTargetLocation = "";
                string strGraphicsLocation = "";
                if (installFolder == null)
                {
                    if (frame._64bit == true)
                    {
                        strTargetLocation = txtInstallFolder.Text.Trim() + @"\" + InstallerConstants.cClientRootFolder64 + @"\" + InstallerConstants.cClientFolder;
                        strGraphicsLocation = txtInstallFolder.Text.Trim() + @"\" + InstallerConstants.cClientRootFolder64 + @"\" + InstallerConstants.cGraphicsFolder;
                    }
                    else
                    {
                        strTargetLocation = txtInstallFolder.Text.Trim() + @"\" + InstallerConstants.cClientRootFolder + @"\" + InstallerConstants.cClientFolder;
                        strGraphicsLocation = txtInstallFolder.Text.Trim() + @"\" + InstallerConstants.cClientRootFolder + @"\" + InstallerConstants.cGraphicsFolder;
                    }
                    installFolder = txtInstallFolder.Text.Trim();
                }
                else
                {
                    strTargetLocation = installFolder + @"\" + InstallerConstants.cClientRootFolder + @"\" + InstallerConstants.cClientFolder;
                    strGraphicsLocation = installFolder + @"\" + InstallerConstants.cClientRootFolder + @"\" + InstallerConstants.cGraphicsFolder;
                }

                if (shareName == null)
                {
                    shareName = txtShareName.Text;
                }

                if (sharePath == null)
                {
                    sharePath = installFolder;
                }

                if (BInstallingAutoUpgradeClient ||
                    chxShareClient.Checked)
                {
                    bShareClient = true;
                }

                if (shortcutName == null)
                {
                    shortcutName = txtShortcutName.Text;
                }

                if (Directory.Exists(strTargetLocation) == true)
                {
                    //give the user feedback
                    if (chkAutoUpgradeClient.Checked == false)
                    {
                        //MessageBox.Show("Installation folder is not empty." +
                        //    Environment.NewLine + "You need to run uninstall " +
                        //    "before you can install at this location.",
                        //    "Client Installation Warning",
                        //    MessageBoxButtons.OK,
                        //    MessageBoxIcon.Warning);
                        //give the user feedback
                        if (MessageBox.Show("Installation folder exists." +
                             Environment.NewLine + "Do you want to replace " +
                             "all modules in this folder?",
                             "Client Installation Warning",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            blreturn = false;
                        }
                        else
                        {
                            blreturn = true;
                            frame.InstallTask = eInstallTasks.upgrade;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Installation folder is not empty." +
                            Environment.NewLine + "If you are trying to make this client" +
                            Environment.NewLine + "your auto-upgrade client please do" +
                            Environment.NewLine + "that in Install Client Tasks below." +
                            Environment.NewLine + "If you are trying to install a new" +
                            Environment.NewLine + "client and make it your auto-upgrade" +
                            Environment.NewLine + "client, please change the location" +
                            Environment.NewLine + "of the installation.",
                            "Client Installation Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        blreturn = false;
                    }

                    //blreturn = false;

                }

                if (blreturn)
                {
                    // Begin TT#1120 - JSmith - Custom Client Install doesn't remember global settings file location
                    if (globalConfigurationLocation == null)
                    {
                        if (rdoInstallTypical.Checked &&
                            txtGlobalConfigurationFileTypical.Text != null &&
                            txtGlobalConfigurationFileTypical.Text.Trim().Length > 0)
                        {
                            globalConfigurationLocation = txtGlobalConfigurationFileTypical.Text.Trim();
                        }
                        else if (rdoInstallClient.Checked &&
                            txtGlobalConfigurationFile.Text != null &&
                            txtGlobalConfigurationFile.Text.Trim().Length > 0)
                        {
                            globalConfigurationLocation = txtGlobalConfigurationFile.Text.Trim();
                        }

                        if (globalConfigurationLocation != null &&
                            !System.IO.File.Exists(globalConfigurationLocation))
                        {
                            if (MessageBox.Show("The location specified for the global configuration does not exist.  Do you want to continue?", this.Text,
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                        == DialogResult.No)
                            {
                                return false;
                            }
                        }
                    }
                    // End TT#1120

                    // check for prerequisites
                    ucUtilities utilities = new ucUtilities(frame, log, false, false);
                    if (!utilities.IsCrystalReportsInstalled(log))
                    {
                        log.AddLogEntry("Crystal Reports must be installed.", eErrorType.message);
                        utilities.InstallCrystalReports(true);
                    }

                    //short cut alert object
                    alert = new ShortCutAlert();

                    //create the directory
                    //Directory.CreateDirectory(strTargetLocation);
                    frame.CreateDirectory(strTargetLocation);
                    //Directory.CreateDirectory(strGraphicsLocation);
                    frame.CreateDirectory(strGraphicsLocation);

                    string strInstallDrive = Directory.GetDirectoryRoot(installFolder);

                    //do the install
                    strTarget = DoInstall(strTargetLocation, shortcutName, true, frame._64bit, true, strInstallDrive);


                    /*Begin TT#1157 - In a Windows 7 environment, the first auto 
                     * upgrade failed because the logged in user did not have 
                     * authority to create the auto upgrade log file.*/

                    // Begin TT#1249 - stodd - 
                    //create a generic upgrade log to be used in the future
                    //if (!AutoUpgradeClient)
                    //{
                    //    System.IO.File.Create(strTargetLocation + @"\Upgrade.log");
                    //}
                    // End TT#1249 - stodd - 
                    /*End TT#1157*/

                    if (strTarget == null)
                    {
                        blreturn = false;
                    }
                    else
                    {
                        frame.SetFolderPathNotReadOnly(Directory.GetParent(strTargetLocation).ToString());

                        if (bShareClient)
                        {
                            frame.ShareFolder(sharePath, shareName, shareName, false);
                        }
                    }
                }

                if (blreturn)
                {
                    if (!frame.blPreviousInstalls)
                    {
                        string strParentDir = string.Empty;
                        string strAppDir = string.Empty;
                        strParentDir = txtInstallFolder.Text.Trim();
                        strAppDir = Directory.GetParent(strParentDir).ToString().Trim();
                        config.CopyOldConfigFiles(frame, strTargetLocation);
                        config.UpgradeConfigFiles(strParentDir, strAppDir, true);
                    }

                    frame.RebuildComponentLists();

                    if (frame.FileInUse(installFolder))
                    {
                        log.AddLogEntry("Found files in use after install completed", eErrorType.error);
                    }

                    if (frame.FileInUse(sharePath))
                    {
                        log.AddLogEntry("Found files in use in share path after install completed", eErrorType.error);
                    }
                }

                //refresh stuff            
                this.Refresh();
                Application.DoEvents();
                Thread.Sleep(2000);

                ////reset the progress bar
                //if (!frame.blPerformingOneClickUpgrade &&
                //    !frame.blPerformingTypicalInstall)
                //{
                //    frame.ProgressBarSetValue(0);
                //}

            }
            catch (Exception err)
            {
                frame.ErrorHandler(err.Message);
            }
            finally
            {
                /* Begin TT#1192 - Batch executable files blocked after initial install/upgrade - APicchetti - 3/17/2011 */
                if (blreturn)
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.Arguments = "-s -d " + installFolder;
                    psi.FileName = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\streams.exe";
                    Process.Start(psi);
                }
                /* End TT#1192 - Batch executable files blocked after initial install/upgrade - APicchetti - 3/17/2011 */
            }

            //return the flag
            return blreturn;

        }

        public void Uninstall()
        {
            ArrayList slClientsToUninstalled = new ArrayList();

            foreach (string lstInstalledClient in lstInstalledClients.SelectedItems)
            {
                slClientsToUninstalled.Add(lstInstalledClient);
            }

            frame.ProgressBarSetMinimum(0);
            frame.ProgressBarSetMaximum((slClientsToUninstalled.Count * 5) + 1);
            foreach (string lstInstalledClient in slClientsToUninstalled)
            {
                Uninstall(lstInstalledClient, true);
            }

            frame.RebuildComponentLists();

            frame.ProgressBarSetToMaximum();
            //give user feedback
			// Begin TT#1668 - JSmith - Install Log
            //frame.lblStatus.Text = "Requested client(s) has been successfully uninstalled";
            frame.SetStatusMessage("Requested client(s) has been successfully uninstalled");
			// End TT#1668
        }

        public bool Uninstall(string sInstalledClient)
        {
            return Uninstall(sInstalledClient, false);
        }

        private bool Uninstall(string sInstalledClient, bool blRemoveFromList)
        {
		    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
			//string strShortcutName = "MIDRetail";
            string strShortcutName = "Logility - RO";
			// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
            string strRegKey = "";
            string strConfigKey = "";

            frame.ProgressBarIncrementValue(1);

            RegistryKey appRegKey = GetExistingRegKey(sInstalledClient, out strRegKey, out strConfigKey);

            if (appRegKey == null)
            {
                return false;
            }

            //get the rows with the sought location
            DataRow[] drResults = frame.dtWindowsInstalled.Select("Location = '" + sInstalledClient + "'");

            //log the action
            log.AddLogEntry("Uninstalling client components: " + sInstalledClient, eErrorType.message);
            frame.SetStatusMessage("Uninstalling client components: " + sInstalledClient);

            //if the file was installed by the windows installer
            if (drResults.GetUpperBound(0) == 0)
            {
                //...true, use the windows installer to uninstall
                string uninstall_key = drResults[0].Field<string>("UninstallKey").ToString().Trim();
                frame.RemoveWindowsInstalledComponent(uninstall_key);

                if (blRemoveFromList)
                {
                    removeClientFromList(drResults[0].Field<string>("Location").ToString().Trim());
                }

                ////give user feedback
                //MessageBox.Show("The chosen application has been removed.",
                //    "MIDRetail Client Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string strApplicationDir = Directory.GetParent(
                        sInstalledClient).ToString().Trim();
                string strApplicationFile = Path.GetFileName(sInstalledClient);
                if (frame.FileInUse(strApplicationDir))
                {
                    // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                   // MessageBox.Show("A module is in use in folder " + strApplicationDir + "." + Environment.NewLine +
                   //"All access to modules in this folder must be stopped before the uninstall can continue", "Installation Error", MessageBoxButtons.OK,
                   //MessageBoxIcon.Error);
                    string msg = "A module is in use in folder " + strApplicationDir + "." + Environment.NewLine +
                   "All access to modules in this folder must be stopped and the uninstall reprocessed.";
                    log.AddLogEntry(msg, eErrorType.error);
                    MessageBox.Show(msg, "Installation Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error); 
                    // End TT#1822-MD - JSmith - Installer not detecting incomplete install
                }
                else
                {
				    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    //strShortcutName = (string)appRegKey.GetValue("QuickLaunch");
                    //if (System.IO.File.Exists(strShortcutName) == true)
                    //{
                    //    QuickLaunchShortCut(strApplicationDir, strApplicationFile, strShortcutName, true, "");
                    //}
                    //else
                    //{
                    //    PinUnpinTaskBar(strApplicationDir + @"\" + strApplicationFile, false);
                    //}
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

                    frame.ProgressBarIncrementValue(1);

                    //...false, delete all the files in the parent directory
                    frame.RemoveManuallyInstalledComponent(sInstalledClient);

                    frame.ProgressBarIncrementValue(1);

                    //log the action
                    log.AddLogEntry("Removing client shortcuts", eErrorType.message);
                    frame.SetStatusMessage("Removing client shortcuts");

                    //remove shortcuts
                    //string strApplicationDir = Directory.GetParent(Directory.GetParent(
                    //    sInstalledClient).ToString().Trim()).ToString().Trim();
                    
                    //ucClient client = (ucClient)ucControl;

                    //begin tt#839-Uninstall of Client - Unexpected Error - apicchetti
                    strShortcutName = (string)appRegKey.GetValue("Desktop");
                    //if (System.IO.File.Exists(AllUserDesktop + @"\" + strShortcutName + ".lnk") == true)
                    if (System.IO.File.Exists(strShortcutName) == true)
                    {
                        DesktopShortCut(strApplicationDir, strApplicationFile, strShortcutName, true);
                    }

                    frame.ProgressBarIncrementValue(1);

                    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    //if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    //    "\\Microsoft\\Internet Explorer\\Quick Launch\\" + strShortcutName + ".lnk") == true)
                    //{
                    //    QuickLaunchShortCut(strApplicationDir, strApplicationFile, strShortcutName, true, "");
                    //}
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

                    strShortcutName = (string)appRegKey.GetValue("ProgramGroup");
                    //if (System.IO.Directory.Exists(AllUserPrograms + @"\" + strShortcutName + "") == true)
                    if (System.IO.File.Exists(strShortcutName) == true)
                    {
                        ProgramGroup(strApplicationDir, strApplicationFile, strShortcutName, true);
                    }
                    //end

                    frame.ProgressBarIncrementValue(1);

                    //log the action
                    log.AddLogEntry("Removing client registry entries", eErrorType.message);
                    frame.SetStatusMessage("Removing client registry entries");

                    //remove registry files
                    frame.RemoveRegistryFiles(sInstalledClient);

                    frame.CleanupFolders(Directory.GetParent(sInstalledClient).ToString().Trim());

                    if (blRemoveFromList)
                    {
                        removeClientFromList(strApplicationDir, strApplicationFile);
                    }

                    //  BEGIN TT#1283
                    //      remove the HLKM entries
                    //log the action
                    log.AddLogEntry("Removing registry entries", eErrorType.message);
                    frame.SetStatusMessage("Removing registry entries");
                    frame.RO.UninstallApplication(strApplicationFile);
                    //  END TT#1283
                }
            }
            return true;
        }

        private bool UpdateLinks(RegistryKey appRegKey)
        {
            try
            {
                // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail"
                //string desktopLocation = appRegKey.GetValue("Desktop").ToString().Trim();
                //string programGroupLocation = appRegKey.GetValue("ProgramGroup").ToString().Trim();
                //string quickLaunchLocation = appRegKey.GetValue("QuickLaunch").ToString().Trim();
                //ReplaceLink(desktopLocation);
                //ReplaceLink(programGroupLocation);
                //ReplaceLink(quickLaunchLocation);

                log.AddLogEntry("Updating Links", eErrorType.message);
                object desktopLocation = appRegKey.GetValue("Desktop");
                object programGroupLocation = appRegKey.GetValue("ProgramGroup");
                object quickLaunchLocation = appRegKey.GetValue("QuickLaunch");

                log.AddLogEntry("desktopLocation=" + desktopLocation, eErrorType.message);
                log.AddLogEntry("programGroupLocation=" + programGroupLocation, eErrorType.message);
                log.AddLogEntry("quickLaunchLocation=" + quickLaunchLocation, eErrorType.message);

                if (desktopLocation != null)
                {
                    ReplaceLink(desktopLocation.ToString().Trim(), appRegKey, "Desktop");
                }
                if (programGroupLocation != null)
                {
                    ReplaceLink(programGroupLocation.ToString().Trim(), appRegKey, "ProgramGroup");
                }
                if (quickLaunchLocation != null)
                {
                    ReplaceLink(quickLaunchLocation.ToString().Trim(), appRegKey, "QuickLaunch");
                }
				// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail"
               
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //private void ReplaceLink(string strFile)
        //{
        //    if (strFile != null && System.IO.File.Exists(strFile))
        //    {
        //        WshShell shell = new WshShell();

        //        IWshShortcut link = (IWshShortcut)shell.CreateShortcut(strFile);

        //        if (System.IO.File.Exists(strFile))
        //        {
        //            System.IO.File.Delete(strFile);
        //        }

        //        link.Save();
        //    }
        //}
        private void ReplaceLink(string strFile, RegistryKey appRegKey, string aKey)
        {
            if (strFile != null && System.IO.File.Exists(strFile))
            {
                try
                {
                    log.AddLogEntry("Updating shortcut " + strFile + ".", eErrorType.message);

                    WshShell shell = new WshShell();

                    IWshShortcut link = (IWshShortcut)shell.CreateShortcut(strFile);

                    log.AddLogEntry("link.FullName: " + link.FullName + ".", eErrorType.message);

                    if (System.IO.File.Exists(strFile))
                    {
                        System.IO.File.Delete(strFile);
                        frame.RefreshDesktop();
                    }

                    // If on start menu, replace MIDRetail folder with Logility - RO folder
                    if (aKey == "ProgramGroup" &&
                        link.FullName.Contains("MIDRetail"))
                    {
                        string path = Directory.GetParent(strFile).ToString().Trim();
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                            path = Directory.GetParent(path).ToString().Trim();
                            Directory.CreateDirectory(path + @"\Logility - RO");
                        }
                    }

                    bool updateRegistry = false;
                    if (link.FullName.Contains("MIDRetail"))
                    {
                        strFile = strFile.Replace("MIDRetail", "Logility - RO");
                        updateRegistry = true;
                    }

                    IWshShortcut linkNew = (IWshShortcut)shell.CreateShortcut(strFile);
                    linkNew.Description = link.Description.Replace("MIDRetail", "Logility - RO");
                    linkNew.Arguments = link.Arguments;
                    linkNew.IconLocation = link.IconLocation;
                    linkNew.TargetPath = link.TargetPath;

                    log.AddLogEntry("New shortcut name: " + strFile + ".", eErrorType.message);

                    linkNew.Save();
                    if (updateRegistry)
                    {
                        Registry.SetValue(appRegKey.Name, aKey, strFile);
                    }
                    frame.RefreshDesktop();
                }
                catch (Exception err)
                {
                    frame.ErrorHandler(err.Message);
                }
            }
            else if (strFile != null)
            {
                log.AddLogEntry("Shortcut file " + strFile + " not found.", eErrorType.error);
            }
        }
		// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

        private bool InstallUpgradeApp(string strParentLocation, string strTargetLocation, string Install_Upgrade, out string ApplicationFile)
        {
            //initialize the out parm
            ApplicationFile = "";


            try
            {
                if (frame.FileInUse(strTargetLocation))
                {
                    // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                   // MessageBox.Show("A module is in use in folder " + strTargetLocation + "." + Environment.NewLine +
                   //"All access to modules in this folder must be stopped before the " + Install_Upgrade + " can continue", "Installation Error", MessageBoxButtons.OK,
                   //MessageBoxIcon.Error);
                    string msg = "A module is in use in folder " + strTargetLocation + "." + Environment.NewLine +
                   "All access to modules in this folder must be stopped and the " + Install_Upgrade + " reprocessed.";
                    log.AddLogEntry(msg, eErrorType.error);
                    MessageBox.Show(msg, "Installation Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                    // End TT#1822-MD - JSmith - Installer not detecting incomplete install
                    return false;
                }
                //get into the installer_files table
                DataTable dtFiles = null;
                if (Install_Upgrade == "Install")
                {
                    dtFiles = frame.installer_data.Tables["install_file"];
                }
                else
                {
                    dtFiles = frame.installer_data.Tables["upgrade_file"];
                }

                //get the client data rows
                DataRow[] drClientFiles = null;
                if (frame._64bit == false)
                {
                    drClientFiles = dtFiles.Select("Application = '" + ConfigurationManager.AppSettings["Client_Folder"].ToString().Trim()
                        + "' AND platform = '32-bit'");
                }
                else
                {
                    drClientFiles = dtFiles.Select("Application = '" + ConfigurationManager.AppSettings["Client_Folder"].ToString().Trim()
                        + "' AND platform = '64-bit'");
                }

                //get the graphics data rows
                DataRow[] drGraphicFiles = null;
                if (frame._64bit == false)
                {
                    drGraphicFiles = dtFiles.Select("Application = '" + ConfigurationManager.AppSettings["Client_Graphics_Folder"].ToString().Trim()
                        + "' AND platform = '32-bit'");
                }
                else
                {
                    drGraphicFiles = dtFiles.Select("Application = '" + ConfigurationManager.AppSettings["Client_Graphics_Folder"].ToString().Trim()
                        + "' AND platform = '64-bit'");
                }

                //prep the progress bar
                //if (!frame.blPerformingOneClickUpgrade)
                //{
                //    frame.ProgressBarSetMinimum(0);
                //    frame.ProgressBarSetMaximum(drClientFiles.GetUpperBound(0) + 1);
                //}

                installedFiles = new List<string>();

                for (int intRow = 0; intRow <= drClientFiles.GetUpperBound(0); intRow++)
                {
                    //process graphics 
                    string strFolder = drGraphicFiles[intRow].Field<string>("folder");
                    string strZipFile = drGraphicFiles[intRow].Field<string>("name");

                    InflateInstallFile(strFolder, strZipFile, strParentLocation + @"\" + InstallerConstants.cGraphicsFolder, out ApplicationFile);

                    // process application
                    strFolder = drClientFiles[intRow].Field<string>("folder");
                    strZipFile = drClientFiles[intRow].Field<string>("name");

                    InflateInstallFile(strFolder, strZipFile, strTargetLocation, out ApplicationFile);

                    ////increment progress bar
                    //frame.ProgressBarIncrementValue(1);
                }

                //increment progress bar
                frame.ProgressBarIncrementValue(1);

                // Begin TT#932 - JSmith - Security violation update computations
                frame.UpdateSecurity(strTargetLocation, true);
                // End TT#932

                // Begin TT#1456 - JSmith - auto upgrade fails
                frame.UpdateSecurity(Directory.GetParent(strTargetLocation).ToString().Trim() + @"\" + InstallerConstants.cGraphicsFolder, true);
                // End TT#1456
                //if (!frame.blPerformingOneClickUpgrade)
                //{
                //    frame.ProgressBarSetToMaximum();
                //}
            }
            catch(Exception err)
            {
                frame.ErrorHandler(err.Message);
            }
            return true;
        }

        private void RegClient(string strClientName, string strConfigName, RegistryKey mid_key,
            string TargetLocation, string Install_Upgrade, string ApplicationFile, bool AutoUpgrade,
            bool bl64bit)
        {
            //create the client key
            RegistryKey client_key = null;
            //RegistryKey config_key = null;
            if (Install_Upgrade == "Install")
            {
                client_key = mid_key.CreateSubKey(strClientName);
                //config_key = mid_key.CreateSubKey(strConfigName);
            }
            else
            {
                client_key = mid_key.OpenSubKey(strClientName, true);
                //config_key = mid_key.OpenSubKey(strConfigName,true);
            }

            //set client values
            client_key.SetValue("EntryType", eEntryType.MIDClient.ToString());
            client_key.SetValue("InstallType", "MIDRetail");
            //client_key.SetValue("Location", TargetLocation +
            //    @"\Client\" + ApplicationFile);
            client_key.SetValue("Location", TargetLocation +
                  @"\" + ApplicationFile);

            if (AutoUpgrade == true)
            {
                //get the version number form the file
                //Assembly assem = Assembly.ReflectionOnlyLoadFrom(TargetLocation +
                //    @"\Client\" + ApplicationFile);
                Assembly assem = Assembly.ReflectionOnlyLoadFrom(TargetLocation +
                     @"\" + ApplicationFile);
                AssemblyName assemName = assem.GetName();
                Version ver = assemName.Version;

                //set the auto upgrade value
                client_key.SetValue("AutoUpgrade", ver.ToString());
            }

            ////set config values
            //config_key.SetValue("EntryType", eEntryType.MIDConfig.GetHashCode().ToString());
            //config_key.SetValue("InstallType", "MIDRetail");
            //config_key.SetValue("Location", TargetLocation + @"\" + ConfigurationFileName);
            //config_key.SetValue("Client", strClientName);

            //register the file inventory
            string strDelimInstalledFiles = "";
            foreach (string installedFile in installedFiles)
            {
                strDelimInstalledFiles = strDelimInstalledFiles + installedFile + ";";
            }
            client_key.SetValue("InstalledFiles", strDelimInstalledFiles);

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
            _uninstall.UninstallString = "";    //  MDAdvInstaller file location
            _uninstall.UninstallLocation = txtInstallFolder.Text + "\\";
            _uninstall.Version = frame.ProductVersion;
            _uninstall.VersionMajor = 0;
            _uninstall.VersionMinor = 0;
            _uninstall.WindowsInstaller = false;    //  Windows Installer Used?
            frame.RO.InstallApplication(_uninstall, ApplicationFile);

            // END TT#1283
            // Begin TT#1668 - JSmith - Install Log
            frame.LogRegistryItem(client_key);
			// End TT#1668
        }

        private void RegisterInstallation(string TargetLocation, string ApplicationFile, bool blAutoUpgrade, bool bl64bit,
            out string strRegClientName, out string strRegConfigName)
        {
            //get the number of currently installed clients
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey, true);
            bool alreadyRegistered = false;

            if (mid_key != null)
            {
                //counters
                int intClients = 0;
                int intConfigs = 0;

                //get the sub key collection
                string[] SubKeyNames = mid_key.GetSubKeyNames();

                //loop thru the subkeys and count them to know what to name the new keys
                foreach (string SubKeyName in SubKeyNames)
                {
                    RegistryKey sub_key = mid_key.OpenSubKey(SubKeyName);

                    

                    if (SubKeyName.Contains(InstallerConstants.cClientKey) == true)
                    {
                        if (sub_key.GetValue("Location").ToString().Trim() == TargetLocation + @"\" + ApplicationFile)
                        {
                            alreadyRegistered = true;
                            strClientName = SubKeyName;
                        }
                        else
                        {
                            intClients++;
                        }
                    }

                    if (SubKeyName.Contains("MIDConfig") == true)
                    {
                        if (sub_key.GetValue("Location").ToString().Trim() == TargetLocation + @"\" + ApplicationFile)
                        {
                            alreadyRegistered = true;
                            strConfigName = SubKeyName;
                        }
                        else
                        {
                            intConfigs++;
                        }
                    }
                }

                if (!alreadyRegistered)
                {
                    //set the key names
                    if (intClients != 0)
                    {
                        strClientName = InstallerConstants.cClientKey + intClients.ToString().Trim();
                    }
                    else
                    {
                        strClientName = InstallerConstants.cClientKey;
                    }

                    if (intConfigs != 0)
                    {
                        strConfigName = "MIDConfig" + intConfigs.ToString().Trim();
                    }
                    else
                    {
                        strConfigName = "MIDConfig";
                    }

                    //register the client

                    RegClient(strClientName, strConfigName, mid_key, TargetLocation, "Install", ApplicationFile, blAutoUpgrade, bl64bit);
                }

            }
            else
            {
                //create the mid product key
                mid_key = soft_key.CreateSubKey(InstallerConstants.cRegistryRootKey);

                //register client
                RegClient(InstallerConstants.cClientKey, "MIDConfig", mid_key, TargetLocation, "Install", ApplicationFile, blAutoUpgrade, bl64bit);
            }

            //set out parms values
            strRegClientName = strClientName;
            strRegConfigName = strConfigName;
        }

        private void RemoveShortCut(string ExeLocation, eShortcutType sType)
        {
            //iterate the link value
            string strLink = "";

            //program group flag
            bool blProgramGroup = false;

            //short cut flags
            bool blQuicklaunch = false;
            bool blDesktop = false;
            bool blProgramGroupExists = false;

            //drill into the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);
            string[] sub_keysnames = mid_key.GetSubKeyNames();

            //loop thru the registry sub keys
            foreach (string sub_keyname in sub_keysnames)
            {
                if (sub_keyname.Contains(InstallerConstants.cClientKey) == true)
                {
                    RegistryKey sub_key = mid_key.OpenSubKey(sub_keyname);

                    if (sub_key.GetValue("Location").ToString().Trim() == ExeLocation)
                    {
                        //loop thru key value and set flags as needed
                        string[] strValueNames = sub_key.GetValueNames();
                        foreach (string strValueName in strValueNames)
                        {
                            if (strValueName == "QuickLaunch")
                            {
                                blQuicklaunch = true;
                            }

                            if (strValueName == "Desktop")
                            {
                                blDesktop = true;
                            }

                            if (strValueName == "ProgramGroup")
                            {
                                blProgramGroupExists = true;
                            }
                        }

                        //get the link path
                        switch (sType)
                        {
                            case eShortcutType.quicklaunch:
                                if (blQuicklaunch == true)
                                {
                                    strLink = sub_key.GetValue("QuickLaunch").ToString().Trim();
                                }
                                break;
                            case eShortcutType.desktop:
                                if (blDesktop == true)
                                {
                                    strLink = sub_key.GetValue("DeskTop").ToString().Trim();
                                }
                                break;

                            //get the link path and delete them at the same time
                            case eShortcutType.programgroup:
                                if (blProgramGroupExists == true)
                                {
                                    blProgramGroup = true;
                                    strLink = sub_key.GetValue("ProgramGroup").ToString().Trim();

                                    frame.DeleteFile(strLink);

                                    string[] strLinkFiles = Directory.GetFiles(Directory.GetParent(strLink).ToString().Trim());

                                    if (strLinkFiles.GetUpperBound(0) == -1)
                                    {
                                        string strParentDir = Directory.GetParent(strLink).ToString().Trim();
                                        frame.DeleteFolder(strParentDir);
                                    }
                                }

                                break;
                        }
                    }
                }
            }

            //delete the link
            if (blProgramGroup == false)
            {
                if (blQuicklaunch == true || blDesktop == true || blProgramGroupExists == true)
                {
                    frame.DeleteFile(strLink);
                }
            }

            // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
            if (blDesktop)
            {
                frame.RefreshDesktop();
            }
            // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        }

        //public void QuickLaunchShortCut(string TargetLocation, string ApplicationFile, string strLinkName, bool Remove, string strClientName)
        //{
        //    //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

        //    //string strLinkName = "MIDRetail";
        //    // Create a new instance of WshShellClass
        //    //WshShell = new WshShellClass();

        //    // Create the shortcut
        //    //IWshRuntimeLibrary.IWshShortcut AllocationShortcut;

        //    //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

        //    // Choose the path for the shortcut
        //    string strAllocShortCut = "";
        //    if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
        //        "\\Microsoft\\Internet Explorer\\Quick Launch\\" + strLinkName + ".lnk") == true && Remove == false)
        //    {
        //        //short cut alert dialog box
        //        if (alert.LinkName == "")
        //        {
        //            if (alert.ShowDialog() == DialogResult.OK)
        //            {
        //                //short cut location
        //                strAllocShortCut = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
        //                    "\\Microsoft\\Internet Explorer\\Quick Launch\\" + alert.LinkName + ".lnk";
        //            }
        //        }
        //        else
        //        {
        //            //short cut location
        //            strAllocShortCut = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
        //                    "\\Microsoft\\Internet Explorer\\Quick Launch\\" + alert.LinkName + ".lnk";
        //        }
        //    }
        //    else if (Remove)
        //    {
        //        //short cut location
        //        strAllocShortCut = strLinkName;
        //    }
        //    else
        //    {
        //        //short cut location
        //        strAllocShortCut = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
        //            "\\Microsoft\\Internet Explorer\\Quick Launch\\" + strLinkName + ".lnk";
        //    }

        //    if (Remove == false)
        //    {
        //        //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
        //        //set the short cut path
        //        //AllocationShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(strAllocShortCut);

        //        // Where the shortcut should point to
        //        //AllocationShortcut.TargetPath = TargetLocation + @"\" + ApplicationFile;
        //        //AllocationShortcut.TargetPath = TargetLocation + @"\Client\" + ApplicationFile;

        //        // Description for the shortcut
        //        //AllocationShortcut.Description = "Shortcut to MIDRetail";

        //        // Location for the shortcut's icon
        //        //AllocationShortcut.IconLocation = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon + @", 0";

        //        // Create the shortcut at the given path
        //        //AllocationShortcut.Save();

        //        try
        //        {
        //            m_Shortcut = new ShellShortcut(strAllocShortCut);
        //            m_Shortcut.Path = TargetLocation + @"\" + ApplicationFile;
        //            // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //            //m_Shortcut.Description = "Shortcut to MIDRetail";
        //            m_Shortcut.Description = "Shortcut to Logility - RO";
        //            // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //            m_Shortcut.IconPath = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon;
        //            m_Shortcut.IconIndex = 0;

        //            // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //            PinUnpinTaskBar(m_Shortcut.Path, true);
        //            // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

        //            m_Shortcut.Save();
        //        }
        //        catch (Exception ex)
        //        { throw ex; }
        //        //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

        //        //register the shortcut
        //        RegistryKey local_key = Registry.LocalMachine;
        //        RegistryKey client_key = local_key.OpenSubKey("SOFTWARE\\MIDRetailInc\\" + strClientName, true);
        //        client_key.SetValue("QuickLaunch", strAllocShortCut);
        //    }
        //    else
        //    {
        //        //remove the quicklaunch shortcut
        //        string strExeFile = TargetLocation + @"\" + ApplicationFile;
        //        // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //        PinUnpinTaskBar(strExeFile, false);
        //        // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //        RemoveShortCut(strExeFile, eShortcutType.quicklaunch);
        //    }

        //    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //    frame.RefreshDesktop();
        //    // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //}

        //// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //private static void PinUnpinTaskBar(string filePath, bool pin)
        //{
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return;
        //    }

        //    // create the shell application object
        //    Shell shellApplication = new ShellClass();

        //    string path = Path.GetDirectoryName(filePath);
        //    string fileName = Path.GetFileName(filePath);

        //    Shell32.Folder directory = shellApplication.NameSpace(path);
        //    FolderItem link = directory.ParseName(fileName);

        //    FolderItemVerbs verbs = link.Verbs();
        //    for (int i = 0; i < verbs.Count; i++)
        //    {
        //        FolderItemVerb verb = verbs.Item(i);
        //        string verbName = verb.Name.Replace(@"&", string.Empty).ToLower();

        //        if ((pin && verbName.Equals("pin to taskbar")) || (!pin && verbName.Equals("unpin from taskbar")))
        //        {

        //            verb.DoIt();
        //        }
        //    }

        //    shellApplication = null;
        //}
		// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        //too many changes to individually mark
        //replaced hard-coded references with variables set by environment
        //End TT#883
        public void DesktopShortCut(string TargetLocation, string ApplicationFile, string strLinkName, bool Remove, bool Everyone, string strRegClientName)
        {
            // Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //string strLinkName = "MIDRetail";
            // Create a new instance of WshShellClass
            //WshShell = new WshShellClass();

            // Create the shortcut
            //IWshRuntimeLibrary.IWshShortcut AllocationShortcut = null;

            // End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //short cut variable
            string strLinkFile = "";

            // Choose the path for the shortcut
            if (Everyone == true)
            {
                if (!Directory.Exists(AllUserDesktop))
                {
                    //Directory.CreateDirectory(AllUserDesktop);
                    frame.CreateDirectory(AllUserDesktop);
                }
                if (Remove == false)
                {
                    if (System.IO.File.Exists(AllUserDesktop + @"\" + strLinkName + ".lnk") == true)
                    {
                        //short cut alert dialog box
                        if (alert.LinkName == "")
                        {
                            if (alert.ShowDialog() == DialogResult.OK)
                            {
                                //short cut location
                                strLinkFile = AllUserDesktop + @"\" + alert.LinkName + ".lnk";
                            }
                        }
                        else
                        {
                            //short cut location
                            strLinkFile = AllUserDesktop + @"\" + alert.LinkName + ".lnk";
                        }
                    }
                    else
                    {
                        //short cut location
                        strLinkFile = AllUserDesktop + @"\" + strLinkName + ".lnk";
                    }
                }

            }
            else if (Everyone == false)
            {
                //logged in user name
                string strUser = Environment.UserName;

                //loop thru the document settings folder to find shortcut locations
                foreach (string strDir in Directory.GetDirectories(UserDesktop))
                {
                    if (Remove == false)
                    {
                        if (System.IO.File.Exists(strDir + @"\" + strLinkName + ".lnk") == true)
                        {
                            //short cut alert dialog box
                            if (alert.LinkName == "")
                            {
                                if (alert.ShowDialog() == DialogResult.OK)
                                {
                                    //short cut location
                                    strLinkFile = strDir + @"\" + alert.LinkName + ".lnk";
                                }
                            }

                        }
                        else
                        {
                            if (strDir.Contains(strUser))
                            {
                                //create shortcut
                                strLinkFile = strDir + @"\" + strLinkName + ".lnk";
                            }
                        }
                    }
                }
            }

            if (Remove == false)
            {

                //create the shortcut
                //AllocationShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(strLinkFile);

                // Where the shortcut should point to
                //AllocationShortcut.TargetPath = TargetLocation + @"\" + ApplicationFile;

                // Description for the shortcut
                //AllocationShortcut.Description = "Shortcut to MIDRetail";

                // Location for the shortcut's icon
                //AllocationShortcut.IconLocation = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon + @", 0";

                // Create the shortcut at the given path
                //AllocationShortcut.Save();

                //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
                try
                {
                    m_Shortcut = new ShellShortcut(strLinkFile);
                    m_Shortcut.Path = TargetLocation + @"\" + ApplicationFile;
					// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
					//m_Shortcut.Description = "Shortcut to MIDRetail";
                    m_Shortcut.Description = "Shortcut to Logility - RO";
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    m_Shortcut.IconPath = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon;
                    m_Shortcut.IconIndex = 0;
                    m_Shortcut.Save();
                }
                catch (Exception ex)
                { throw ex; }
                //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
                //register the shortcut
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey client_key = local_key.OpenSubKey("SOFTWARE\\MIDRetailInc\\" + strRegClientName, true);
                client_key.SetValue("Desktop", strLinkFile);

            }
            else
            {
                //remove the desktop shortcut
                string strExeFile = TargetLocation + @"\" + ApplicationFile;
                RemoveShortCut(strExeFile, eShortcutType.desktop);
            }
        }

        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        //too many changes to individually mark
        //replaced hard-coded references with variables set by environment
        //End TT#883
        public void DesktopShortCut(string TargetLocation, string ApplicationFile, string strLinkName, bool Remove)
        {
            //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
            //string strLinkName = "MIDRetail";
            // Create a new instance of WshShellClass
            //WshShell = new WshShellClass();

            // Create the shortcut
            //IWshRuntimeLibrary.IWshShortcut AllocationShortcut = null;

            //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //link file variable
            string strLinkFile = "";
            bool shortcutExists = false;

            // Choose the path for the shortcut
            if (rdoEveryone.Checked == true)
            {
                if (!Directory.Exists(AllUserDesktop))
                {
                    //Directory.CreateDirectory(AllUserDesktop);
                    frame.CreateDirectory(AllUserDesktop);
                }
                if (Remove == false)
                {
                    if (System.IO.File.Exists(AllUserDesktop + @"\" + strLinkName + ".lnk") == true &&
                        frame.InstallTask == eInstallTasks.install)
                    {
                        //short cut alert dialog box
                        if (alert.ShowDialog() == DialogResult.OK)
                        {
                            //short cut location
                            strLinkFile = AllUserDesktop + @"\" + alert.LinkName + ".lnk";
                        }
                    }
                    else
                    {
                        //short cut location
                        strLinkFile = AllUserDesktop + @"\" + strLinkName + ".lnk";
                    }
                }

            }
            else if (rdoMe.Checked == true)
            {
                //logged in user name
                string strUser = Environment.UserName;

                //loop thru the document settings folder to find shortcut locations
                foreach (string strDir in Directory.GetDirectories(UserDesktop))
                {
                    if (Remove == false)
                    {
                        if (System.IO.File.Exists(strDir + @"\" + strLinkName + ".lnk") == true &&
                        frame.InstallTask == eInstallTasks.install)
                        {
                            //short cut alert dialog box
                            if (alert.ShowDialog() == DialogResult.OK)
                            {
                                //short cut location
                                strLinkFile = strDir + @"\" + alert.LinkName + ".lnk";
                            }
                        }
                        else
                        {
                            if (strDir.Contains(strUser))
                            {
                                //short cut location
                                strLinkFile = strDir + @"\" + strLinkName + ".lnk";
                            }
                        }
                    }
                }
            }

            if (Remove == false)
            {
                //create shortcut
                //AllocationShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(strLinkFile);

                // Where the shortcut should point to
                //AllocationShortcut.TargetPath = TargetLocation + @"\" + ApplicationFile;
                //AllocationShortcut.TargetPath = TargetLocation + @"\Client\" + ApplicationFile;

                // Description for the shortcut
                //AllocationShortcut.Description = "Shortcut to MIDRetail";

                // Location for the shortcut's icon
                //AllocationShortcut.IconLocation = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon + @", 0";

                // Create the shortcut at the given path
                //AllocationShortcut.Save();

                //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
                try
                {
                    if (frame.InstallTask == eInstallTasks.upgrade)
                    {
                        //remove the desktop shortcut
                        string strExeFile = TargetLocation + @"\" + ApplicationFile;
                        RemoveShortCut(strExeFile, eShortcutType.desktop);
                    }

                    m_Shortcut = new ShellShortcut(strLinkFile);
                    m_Shortcut.Path = TargetLocation + @"\" + ApplicationFile;
					// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
					//m_Shortcut.Description = "Shortcut to MIDRetail";
                    m_Shortcut.Description = "Shortcut to Logility - RO";
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    m_Shortcut.IconPath = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon;
                    m_Shortcut.IconIndex = 0;
                    m_Shortcut.Save();
                }
                catch (Exception ex)
                { throw ex; }
                //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

                //register the shortcut
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey client_key = local_key.OpenSubKey("SOFTWARE\\MIDRetailInc\\" + strClientName, true);
                client_key.SetValue("Desktop", strLinkFile);
            }
            else
            {
                //remove the desktop shortcut
                string strExeFile = TargetLocation + @"\" + ApplicationFile;
                RemoveShortCut(strExeFile, eShortcutType.desktop);
            }
        }

        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        //too many changes to individually mark
        //replaced hard-coded references with variables set by environment
        //End TT#883
        public void ProgramGroup(string TargetLocation, string ApplicationFile, string strLinkName, bool Remove, bool Everyone, string strRegClientName)
        {
            //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
            //string strLinkName = "MIDRetail";
            // Create a new instance of WshShellClass
            //WshShell = new WshShellClass();

            // Create the shortcut
            //IWshRuntimeLibrary.IWshShortcut AllocationShortcut = null;

            //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //get user name
            string strUserName = Environment.UserName;

            //file variables
			// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
			//string strShortcutDir = AllUserPrograms + @"\MIDRetail";
            string strShortcutDir = AllUserPrograms + @"\Logility - RO";
			// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
            string strShortcutFile = strShortcutDir + @"\" + strLinkName + ".lnk";

            //create the program group folder
            if (Everyone == true)
            {
                if (Remove == false)
                {
                    //create the parent folder if it does not exist
                    if (Directory.Exists(strShortcutDir) == false)
                    {
                        //Directory.CreateDirectory(strShortcutDir);
                        frame.CreateDirectory(strShortcutDir);
                    }

                    //create a different shortcut if one already exists
                    if (System.IO.File.Exists(strShortcutFile) == true &&
                        frame.InstallTask == eInstallTasks.install)
                    {
                        //short cut alert dialog box
                        if (alert.LinkName == "")
                        {
                            //get link file name
                            if (alert.ShowDialog() == DialogResult.OK)
                            {
                                strShortcutFile = strShortcutDir + @"\" + alert.LinkName + ".lnk";
                            }

                        }
                        else
                        {
                            strShortcutFile = strShortcutDir + @"\" + alert.LinkName + ".lnk";
                        }

                    }
                }
            }
            else if (Everyone == false)
            {
                if (Remove == false)
                {
                    foreach (string strDir in Directory.GetDirectories(UserPrograms))
                    {
                        if (strDir.Contains(strUserName))
                        {
						    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                            ////create the dir
                            //if (Directory.Exists(strDir + @"\MIDRetail") == false)
                            //{
                            //    //Directory.CreateDirectory(strDir + @"\MIDRetail");
                            //    frame.CreateDirectory(strDir + @"\MIDRetail");
                            //}

                            //// Choose the path for the shortcut
                            //if (System.IO.File.Exists(strDir + @"\MIDRetail\" + strLinkName + ".lnk") == true &&
                            //    frame.InstallTask == eInstallTasks.install)
                            //{
                            //    //short cut alert dialog box
                            //    if (alert.LinkName == "")
                            //    {
                            //        //get link file name
                            //        if (alert.ShowDialog() == DialogResult.OK)
                            //        {
                            //            strShortcutFile = strDir + @"\MIDRetail\" + alert.LinkName;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        strShortcutFile = strDir + @"\MIDRetail\" + alert.LinkName;
                            //    }
                            //}
                            //else
                            //{
                            //    strShortcutFile = strDir + @"\MIDRetail\" + strLinkName + ".lnk";
                            //}
                            //create the dir
                            if (Directory.Exists(strDir + @"\Logility - RO") == false)
                            {
                                //Directory.CreateDirectory(strDir + @"\MIDRetail");
                                frame.CreateDirectory(strDir + @"\Logility - RO");
                            }

                            // Choose the path for the shortcut
                            if (System.IO.File.Exists(strDir + @"\Logility - RO\" + strLinkName + ".lnk") == true &&
                                frame.InstallTask == eInstallTasks.install)
                            {
                                //short cut alert dialog box
                                if (alert.LinkName == "")
                                {
                                    //get link file name
                                    if (alert.ShowDialog() == DialogResult.OK)
                                    {
                                        strShortcutFile = strDir + @"\Logility - RO\" + alert.LinkName;
                                    }
                                }
                                else
                                {
                                    strShortcutFile = strDir + @"\Logility - RO\" + alert.LinkName;
                                }
                            }
                            else
                            {
                                strShortcutFile = strDir + @"\Logility - RO\" + strLinkName + ".lnk";
                            }
							// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                        }
                    }
                }
            }

            if (Remove == false)
            {
                //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

                // Choose the path for the shortcut
                //AllocationShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(strShortcutFile + ".lnk");

                // Where the shortcut should point to
                //AllocationShortcut.TargetPath = TargetLocation + @"\" + ApplicationFile;

                // Description for the shortcut
                //AllocationShortcut.Description = "MIDRetail";

                // Location for the shortcut's icon
                //AllocationShortcut.IconLocation = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon + @", 0";

                // Create the shortcut at the given path
                //AllocationShortcut.Save();

                try
                {
                    if (frame.InstallTask == eInstallTasks.upgrade)
                    {
                        //remove the desktop shortcut
                        string strExeFile = TargetLocation + @"\" + ApplicationFile;
                        RemoveShortCut(strExeFile, eShortcutType.programgroup);
                    }

                    m_Shortcut = new ShellShortcut(strShortcutFile + ".lnk");
                    m_Shortcut.Path = TargetLocation + @"\" + ApplicationFile;
					// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
					//m_Shortcut.Description = "MIDRetail";
                    m_Shortcut.Description = "Logility - RO";
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    m_Shortcut.IconPath = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon;
                    m_Shortcut.IconIndex = 0;
                    m_Shortcut.Save();
                }
                catch (Exception ex)
                { throw ex; }
                //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

                //register the shortcut
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey client_key = local_key.OpenSubKey("SOFTWARE\\MIDRetailInc\\" + strRegClientName, true);
                client_key.SetValue("ProgramGroup", strShortcutFile);
            }
            else
            {
                //remove the desktop shortcut
                string strExeFile = TargetLocation + @"\" + ApplicationFile;
                RemoveShortCut(strExeFile, eShortcutType.programgroup);

            }
        }

        public void ProgramGroup(string TargetLocation, string ApplicationFile, string strLinkName, bool Remove)
        {
            //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
            //string strLinkName = "MIDRetail";
            // Create a new instance of WshShellClass
            //WshShell = new WshShellClass();

            // Create the shortcut
            //IWshRuntimeLibrary.IWshShortcut AllocationShortcut = null;
            //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

            //get user name
            string strUserName = Environment.UserName;

            //shortcut location variable
            string strShortcutFile = "";

            //create the program group folder
            if (rdoEveryone.Checked == true)
            {
                if (Remove == false)
                {
				    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    //if (Directory.Exists(AllUserPrograms + @"\MIDRetail") == false)
                    //{
                    //    //Directory.CreateDirectory(AllUserPrograms + @"\MIDRetail");
                    //    frame.CreateDirectory(AllUserPrograms + @"\MIDRetail");
                    //}

                    //// Choose the path for the shortcut
                    //strShortcutFile = AllUserPrograms + @"\MIDRetail\" + strLinkName + ".lnk";
                    if (Directory.Exists(AllUserPrograms + @"\Logility - RO") == false)
                    {
                        //Directory.CreateDirectory(AllUserPrograms + @"\MIDRetail");
                        frame.CreateDirectory(AllUserPrograms + @"\Logility - RO");
                    }

                    // Choose the path for the shortcut
                    strShortcutFile = AllUserPrograms + @"\Logility - RO\" + strLinkName + ".lnk";
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                }

            }
            else if (rdoMe.Checked == true)
            {
                if (Remove == false)
                {
                    foreach (string strDir in Directory.GetDirectories(UserPrograms))
                    {
                        if (strDir.Contains(strUserName))
                        {
                            //create the dir
                            //Directory.CreateDirectory(strDir + @"\MIDRetail");
							// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                            //frame.CreateDirectory(strDir + @"\MIDRetail");

                            //// Choose the path for the shortcut
                            //strShortcutFile = strDir + @"\MIDRetail\" + strLinkName + ".lnk";
                            frame.CreateDirectory(strDir + @"\Logility - RO");

                            // Choose the path for the shortcut
                            strShortcutFile = strDir + @"\Logility - RO\" + strLinkName + ".lnk";
							// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                        }
                    }
                }
            }

            if (Remove == false)
            {
                //Begin TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010
                //make shortcut
                //AllocationShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(strShortcutFile);

                // Where the shortcut should point to
                //AllocationShortcut.TargetPath = TargetLocation + @"\Client\" + ApplicationFile;
                //AllocationShortcut.TargetPath = TargetLocation + @"\" + ApplicationFile;

                // Description for the shortcut
                //AllocationShortcut.Description = "MIDRetail";

                // Location for the shortcut's icon
                //AllocationShortcut.IconLocation = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon + @", 0";

                // Create the shortcut at the given path
                //AllocationShortcut.Save();

                try
                {
                    m_Shortcut = new ShellShortcut(strShortcutFile);
                    m_Shortcut.Path = TargetLocation + @"\" + ApplicationFile;
					// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
					//m_Shortcut.Description = "MIDRetail";
                    m_Shortcut.Description = "Logility - RO";
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    m_Shortcut.IconPath = Directory.GetParent(TargetLocation) + @"\" + ApplicationIcon;
                    m_Shortcut.IconIndex = 0;
                    m_Shortcut.Save();
                }
                catch (Exception ex)
                { throw ex; }
                //End TT#1653 - gtaylor - Create new class to replace shortcut code in the installer to allow for VS2010

                //register the shortcut
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey client_key = local_key.OpenSubKey("SOFTWARE\\MIDRetailInc\\" + strClientName, true);
                client_key.SetValue("ProgramGroup", strShortcutFile);
            }
            else
            {
                //remove personal program group
                string strExeFile = TargetLocation + @"\" + ApplicationFile;
                RemoveShortCut(strExeFile, eShortcutType.programgroup);
            }
        }

        public void SetRadioButtons()
        {
            if (lstInstalledClients.Items.Count > 0)
            {
                rdoInstallClientTasks.Enabled = true;
                rdoInstallTypical.Enabled = false;
                gbxTypical.Enabled = false;
            }
            else
            {
                rdoInstallClientTasks.Enabled = false;
                rdoInstallTypical.Enabled = true;
                gbxTypical.Enabled = true;
            }
        }

        private void SetButton()
        {
            switch (cboTasks.Text)
            {
                case "Uninstall":
                    frame.InstallTask = eInstallTasks.uninstall;
                    NotConfigureNext(this, new EventArgs());
                    break;
                case "Auto Upgrade Client":
                    frame.InstallTask = eInstallTasks.setasautoupdate;
                    NotConfigureNext(this, new EventArgs());
                    break;
                case "Configure":
                    frame.InstallTask = eInstallTasks.configure;
                    ConfigureNext(this, new EventArgs());
                    break;
                case "Upgrade":
                    frame.InstallTask = eInstallTasks.upgrade;
                    NotConfigureNext(this, new EventArgs());
                    break;
            }
        }

        public void removeClientFromList(string TargetLocation)
        {
            lstInstalledClients.Items.Remove(TargetLocation);
        }

        public void removeClientFromList(string TargetLocation, string ApplicationFile)
        {
            lstInstalledClients.Items.Remove(TargetLocation + @"\" + ApplicationFile);
        }

        public void SetRadioButtonAfterUninstall()
        {
            if (lstInstalledClients.Items.Count == 0)
            {
                rdoInstallTypical.Enabled = true;
                rdoInstallTypical.Checked = true;
                rdoInstallClientTasks.Enabled = false;
            }
        }

        private void chkAutoUpgradeClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoUpgradeClient.Checked)
            {
                blAutoUpgrade = true;
                chxShareClient.Checked = true;
            }
            else
            {
                blAutoUpgrade = false;
                chxShareClient.Checked = false;
            }
        }

        private void ucClient_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                SetRadioButtons();
                SetButton();
                //NotConfigureNext(this, new EventArgs());
                frame.Back_Enabled = true;
            }
        }

        private void chxShareClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chxShareClient.Checked)
            {
                txtShareName.Enabled = true;
            }
            else
            {
                txtShareName.Enabled = false;
            }
        }

        private void lstInstalledClients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                // Begin TT#74 MD - JSmith - One-button Upgrade
                //for (int i = 0; i < lstInstalledClients.Items.Count; i++)
                //{
                //    lstInstalledClients.SetSelected(i, true);
                //}
                SelectAll();
                // End TT#74 MD
            }
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        public bool SelectAll()
        {
            bool successful = true;
            for (int i = 0; i < lstInstalledClients.Items.Count; i++)
            {
                lstInstalledClients.SetSelected(i, true);
            }
            return successful;
        }
        // End TT#74 MD

        private void chkDesktop_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDesktop.Checked ||
                chkQuickLaunch.Checked)
            {
                txtShortcutName.Enabled = true;
            }
            else
            {
                txtShortcutName.Enabled = false;
            }
        }

        private void chkQuickLaunch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDesktop.Checked ||
                chkQuickLaunch.Checked)
            {
                txtShortcutName.Enabled = true;
            }
            else
            {
                txtShortcutName.Enabled = false;
            }
        }
    }
}
