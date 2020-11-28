using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Diagnostics;

using System.Security.AccessControl;
using System.Security.Principal;

using Microsoft.Win32;

namespace MIDRetailInstaller
{
    public partial class ucScan : UserControl
    {
        //  Begin TT#1192 - Batch executable files blocked after initial install/upgrade
        public RegistryKey GetSoftwareRoot() { var path = 8 == IntPtr.Size ? @"Software\Wow6432Node" : @"Software"; return Registry.LocalMachine.OpenSubKey(path); }
        //  Is this 64bit context?
        public bool Is64bit
        {
            //  if IntPtr.Size == 8, the context is 64bit
            //get { return (IntPtr.Size == 8); }
            get { return InstallerFrame.is64Bit(); }
        }
        //  End TT#1192

        ToolTip tt = new ToolTip();

        //log object
        ucInstallationLog log = null;

        //frame object
        InstallerFrame frame = null;

        //lists to build
        List<string> configFiles = new List<string>();
        List<string> clientFiles = new List<string>();
        List<string> controlServFiles = new List<string>();
        List<string> storeServFiles = new List<string>();
        List<string> merchServFiles = new List<string>();
        List<string> jobServFiles = new List<string>();
        List<string> schedServFiles = new List<string>();
        List<string> appServFiles = new List<string>();
        List<string> apiFiles = new List<string>();

        // containers with application names
        Hashtable htClientNames;
        Hashtable htControlServiceNames;
        Hashtable htStoreServiceNames;
        Hashtable htMerchandiseServiceNames;
        Hashtable htJobServiceNames;
        Hashtable htSchedulerServiceNames;
        Hashtable htApplicationServiceNames;
        Hashtable htConfigurationFileNames;
        Hashtable htAPINames;

        public ucScan(ucInstallationLog p_log, InstallerFrame p_frame)
        {
            InitializeComponent();

            //pass the install log object
            log = p_log;

            //pass the frame object
            frame = p_frame;
            frame.help_ID = "scan";

            tt.SetToolTip(btnScan, frame.GetToolTipText("scan_scan"));

            GetApplicationNames();
        }

        private void GetApplicationNames()
        {
            htClientNames = GetApplicationNames("ClientNames");
            htControlServiceNames = GetApplicationNames("ControlServiceNames");
            htStoreServiceNames = GetApplicationNames("StoreServiceNames");
            htMerchandiseServiceNames = GetApplicationNames("MerchandiseServiceNames");
            htJobServiceNames = GetApplicationNames("JobServiceNames");
            htSchedulerServiceNames = GetApplicationNames("SchedulerServiceNames");
            htApplicationServiceNames = GetApplicationNames("ApplicationServiceNames");
            htConfigurationFileNames = GetApplicationNames("ConfigurationFileName");
            htAPINames = GetApplicationNames("APIName");
        }

        private Hashtable GetApplicationNames(string componentKey)
        {
            Hashtable hashTable = new Hashtable();
            string[] names = ConfigurationManager.AppSettings[componentKey].ToString().Split(';');
            foreach (string name in names)
            {
                if (name != null &&
                    name.Trim().Length > 0)
                {
                    hashTable.Add(name.Trim(), null);
                }
            }
            return hashTable;
        }

        private void GetFolderList(string aFolderName, ArrayList aAlDirectory)
        {
            //clear memory
            string strExclude = "";     //TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/18/2011
            Application.DoEvents();
            ArrayList alExclude = new ArrayList();
            bool skip = false;

            //add parm directory to the folder array
            aAlDirectory.Add(aFolderName);
            try
            {

            /*BEGIN TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/18/2011*/

                //get the folders dir infor
                DirectoryInfo dInfo = new DirectoryInfo(aFolderName);

                // Get the security identifier of the process token
                SecurityIdentifier owner_sid = WindowsIdentity.GetCurrent().Owner;

                // Get the corresponding account.
                NTAccount owner_account = (NTAccount)owner_sid.Translate(typeof(NTAccount));

                //check access rules to the folder for the logged in user
                FileSecurity fs = File.GetAccessControl(dInfo.FullName);
                AuthorizationRuleCollection arc = fs.GetAccessRules(true, true, typeof(NTAccount));
                foreach (FileSystemAccessRule fsar in arc)
                {
                    if (owner_account.Value == fsar.IdentityReference.Value)
                    {
                        if (fsar.AccessControlType.ToString() == "Deny")
                        {
                            strExclude = strExclude + aFolderName + ";";
                            break;
                        }
                    }
                }

                //check for semicolon delimiter before user defined excludes are added
                if(strExclude != "" && strExclude.EndsWith(";") == false) strExclude = strExclude + ";";

            /*END TT#1077 - Installer fixes... updates to Installer.xml file and protect files search - apicchetti - 1/18/2011*/


                //exclude user defined files, windows files, and personal directory files
                strExclude = strExclude + ConfigurationManager.AppSettings["EXCLUDE"].ToString().Trim().ToUpper();
                string[] excludes = strExclude.Split(';');
                foreach (string exclude in excludes)
                {
                    alExclude.Add(exclude.ToUpper());
                }
                string strWindowsInstall = ConfigurationManager.AppSettings["WINDOWS_INSTALL"].ToString().Trim();
                string strPersonalDir = ConfigurationManager.AppSettings["PERSONAL_DIR"].ToString().Trim();

                //program files install location and MID program install location
                string strDefaultInstall = ConfigurationManager.AppSettings["DEFAULT_INSTALL"].ToString().Trim();
                string strProgramsDir = ConfigurationManager.AppSettings["PROGRAMS_DIR"].ToString().Trim();

                //loop thru the sub directorys and build the folder array
                foreach (DirectoryInfo directory in dInfo.GetDirectories())
                {
                    //exclude systems files
                    if ((directory.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        skip = false;
                        foreach (string exclude in alExclude)
                        {
                            if (exclude.Contains(":"))
                            {
                                if (directory.FullName.ToUpper().StartsWith(exclude))
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            else if (exclude.IndexOf(directory.FullName.ToUpper()) >= 0)
                            {
                                skip = true;
                                break;
                            }
                            else if (directory.FullName.ToUpper().IndexOf(exclude) >= 0)
                            {
                                skip = true;
                                break;
                            }
                        }
                        if (skip)
                        {
                            continue;
                        }

                        //if not buid the folder list
                        if (!directory.Name.Contains(strWindowsInstall) &&
                        !directory.Name.Contains(strPersonalDir))
                        {
                            if (directory.FullName.Contains(strProgramsDir))
                            {
                                if (directory.Name.Contains(strProgramsDir) ||
                                    directory.Name.Contains(strPersonalDir))
                                {
                                    GetFolderList(directory.FullName, aAlDirectory);
                                }
                            }
                            else
                            {
                                GetFolderList(directory.FullName, aAlDirectory);
                            }
                        }
                    }
                }
            }
            catch (Exception err_dir)
            {
                //enter error/warning/message into the install log
               log.AddLogEntry(err_dir.Message, eErrorType.warning);
            }
        }

        private void SearchFolder(string aFolderName, int aFolderNumber, int aTotalFolders,
            List<string> configFiles, List<string> clientFiles, List<string> controlServFiles,
            List<string> storeServFiles, List<string> merchServFiles, List<string> schedServFiles,
            List<string> appServFiles, List<string> apiFiles)
        {
            string moduleName;
            //clear memory
            Application.DoEvents();

            char[] delim = @"\".ToCharArray();

            //progress label
			// Begin TT#1668 - JSmith - Install Log
			//frame.lblStatus.Text = "Scanning Folder: " + aFolderName;
            frame.SetStatusMessage("Scanning Folder: " + aFolderName);
			// End TT#1668

            string name = "";
            try
            {
                //get a list of the files in the directory
                string[] files = Directory.GetFiles(aFolderName);

                //build the lists according to the files that are found
                foreach (string file in files)
                {
                    name = Path.GetFileName(file) + ";";


                    string[] strFileParts = file.Split(delim);
                    moduleName = strFileParts[strFileParts.Length - 1];

                    //if (ConfigurationManager.AppSettings["ClientNames"].ToString().Contains(name) == true)
                    //{
                    //    clientFiles.Add(file);
                    //}
                    if (htClientNames.Contains(moduleName) == true)
                    {
                        clientFiles.Add(file);
                    }

                    if (htControlServiceNames.Contains(moduleName) == true)
                    {
                        controlServFiles.Add(file);
                    }

                    if (htStoreServiceNames.Contains(moduleName) == true)
                    {
                        storeServFiles.Add(file);
                    }

                    if (htMerchandiseServiceNames.Contains(moduleName) == true)
                    {
                        merchServFiles.Add(file);
                    }

                    if (htJobServiceNames.Contains(moduleName) == true)
                    {
                        jobServFiles.Add(file);
                    }

                    if (htSchedulerServiceNames.Contains(moduleName) == true)
                    {
                        schedServFiles.Add(file);
                    }

                    if (htApplicationServiceNames.Contains(moduleName) == true)
                    {
                        appServFiles.Add(file);
                    }

                    if (htConfigurationFileNames.Contains(moduleName) == true)
                    {
                        configFiles.Add(file);
                    }

                    if (htAPINames.Contains(moduleName) == true)
                    {
                        string strfile = file.Substring(0, file.LastIndexOf("\\"));
                        apiFiles.Add(strfile);
                    }
                }

            }
            catch (Exception err_files)
            {
                //write any errors/warnings/messages to the installation log
                log.AddLogEntry(err_files.Message, eErrorType.warning);
            }
        }

        private void ScanComputer(out List<string> ConfigFiles, 
                                out List<string> ClientFiles, 
                                out List<string> ControlServiceFiles,
                                out List<string> StoreServiceFiles, 
                                out List<string> MerchServiceFiles, 
                                out List<string> SchedServiceFiles,
                                out List<string> AppServiceFiles, 
                                out List<string> APIFiles)
        {
            //folder counter
            int ctrFolder = 0;

            //directory array
            ArrayList alDirectory = new ArrayList();

            //local drive array
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            //loop thru the drives and build the folder list
            foreach (DriveInfo dirInfo in allDrives)
            {
                if (dirInfo.DriveType == DriveType.Fixed &&
                    dirInfo.IsReady)
                {
                    GetFolderList(dirInfo.RootDirectory.FullName, alDirectory);
                }
            }

            //loop thru all of the found folders and build the application file lists
            foreach (string aDirectory in alDirectory)
            {
                //set up progress bar
                frame.ProgressBarSetMinimum(0);
                frame.ProgressBarSetMaximum(alDirectory.Count);
                frame.ProgressBarSetStep(1);

                //search the folders
                SearchFolder(aDirectory, ctrFolder, alDirectory.Count, configFiles, clientFiles, controlServFiles,
                    storeServFiles, merchServFiles, schedServFiles, appServFiles, apiFiles);
                ctrFolder++;

                //step progress
                frame.ProgressBarPerformStep();
            }

            //return values
            ConfigFiles = configFiles;
            ClientFiles = clientFiles;
            ControlServiceFiles = controlServFiles;
            StoreServiceFiles = storeServFiles;
            MerchServiceFiles = merchServFiles;
            SchedServiceFiles = schedServFiles;
            AppServiceFiles = appServFiles;
            APIFiles = apiFiles;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                //turn to the wait cursor
                this.Cursor = Cursors.WaitCursor;
                frame.ProgressBarSetValue(0);
                tvInstalledComponents.Nodes.Clear();

                //scan the registry for windows installer installed components
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = "Scanning registry for installed components";
                frame.SetStatusMessage("Scanning for installed components");
				// End TT#1668
                Application.DoEvents();
                frame.dtWindowsInstalled = GetWindowsInstalledList();

                //scan the computer for the installed components
                configFiles = new List<string>();
                clientFiles = new List<string>();
                controlServFiles = new List<string>();
                storeServFiles = new List<string>();
                merchServFiles = new List<string>();
                schedServFiles = new List<string>();
                appServFiles = new List<string>();
                apiFiles = new List<string>();
                ScanComputer(out configFiles, out clientFiles, out controlServFiles, out storeServFiles,
                    out merchServFiles, out schedServFiles, out appServFiles, out apiFiles);

                //remove the ones already registered
                //configFiles = RemoveRegisteredComponents(configFiles, false);
                //clientFiles = RemoveRegisteredComponents(clientFiles, false);
                //controlServFiles = RemoveRegisteredComponents(controlServFiles, false);
                //storeServFiles = RemoveRegisteredComponents(storeServFiles, false);
                //merchServFiles = RemoveRegisteredComponents(merchServFiles, false);
                //schedServFiles = RemoveRegisteredComponents(schedServFiles, false);
                //appServFiles = RemoveRegisteredComponents(appServFiles, false);
                //apiFiles = RemoveRegisteredComponents(apiFiles, true);

                //add the client nodes to the tree view
                if (clientFiles.Count > 0)
                {
                    TreeNode tnClient = new TreeNode("Installed Logility RO Clients");
                    tnClient.Checked = true;

                    foreach (string clientFile in clientFiles)
                    {
                        TreeNode tnClient_Node = new TreeNode(clientFile);
                        tnClient_Node.Checked = true;
                        tnClient.Nodes.Add(tnClient_Node);
                    }

                    //add client nodes to the tree view
                    tvInstalledComponents.Nodes.Add(tnClient);
                }

                int intServiceFiles = controlServFiles.Count + storeServFiles.Count + merchServFiles.Count +
                    schedServFiles.Count + appServFiles.Count;

                if(intServiceFiles > 0)
                {
                    //add the service nodes to the tree view
                    TreeNode tnServices = new TreeNode("Installed Logility RO Services");
                    tnServices.Checked = true;

                    foreach (string controlServFile in controlServFiles)
                    {
                        TreeNode tnControlServFiles_Node = new TreeNode(controlServFile);
                        tnControlServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnControlServFiles_Node);
                    }

                    foreach (string storeServFile in storeServFiles)
                    {
                        TreeNode tnStoreServFiles_Node = new TreeNode(storeServFile);
                        tnStoreServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnStoreServFiles_Node);
                    }

                    foreach (string merchServFile in merchServFiles)
                    {
                        TreeNode tnMerchServFiles_Node = new TreeNode(merchServFile);
                        tnMerchServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnMerchServFiles_Node);
                    }

                    foreach (string jobServFile in jobServFiles)
                    {
                        TreeNode tnJobServFiles_Node = new TreeNode(jobServFile);
                        tnJobServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnJobServFiles_Node);
                    }

                    foreach (string schedServFile in schedServFiles)
                    {
                        TreeNode tnSchedServFiles_Node = new TreeNode(schedServFile);
                        tnSchedServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnSchedServFiles_Node);
                    }

                    foreach (string appServFile in appServFiles)
                    {
                        TreeNode tnAppServFiles_Node = new TreeNode(appServFile);
                        tnAppServFiles_Node.Checked = true;
                        tnServices.Nodes.Add(tnAppServFiles_Node);
                    }

                    //add client nodes to the tree view
                    tvInstalledComponents.Nodes.Add(tnServices);
                }

                if (apiFiles.Count > 0)
                {
                    //add the API nodes to the tree view
                    TreeNode tnAPI = new TreeNode("Installed Logility RO APIs");
                    tnAPI.Checked = true;

                    foreach (string apiFile in apiFiles)
                    {
                        TreeNode tnApiFiles_Node = new TreeNode(apiFile);
                        tnApiFiles_Node.Checked = true;
                        tnAPI.Nodes.Add(tnApiFiles_Node);
                    }

                    //add client nodes to the tree view
                    tvInstalledComponents.Nodes.Add(tnAPI);
                }

                if (configFiles.Count > 0)
                {
                    //add the API nodes to the tree view
                    TreeNode tnConfig = new TreeNode("Installed Logility RO Configuration Files");
                    tnConfig.Checked = true;

                    foreach (string configFile in configFiles)
                    {
                        TreeNode tnConfigFiles_Node = new TreeNode(configFile);
                        tnConfigFiles_Node.Checked = true;
                        tnConfig.Nodes.Add(tnConfigFiles_Node);
                    }

                    //add client nodes to the tree view
                    tvInstalledComponents.Nodes.Add(tnConfig);
                }

                if (tvInstalledComponents.Nodes.Count > 0)
                {
                    //open all the nodes in the tree view
                    tvInstalledComponents.ExpandAll();
                }
            }
            catch (Exception err_scan)
            {
                MessageBox.Show(err_scan.Message);
            }
            finally
            {
			    // Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = "Computer scan complete!";
                frame.SetStatusMessage("Computer scan complete!");
				// End TT#1668
                //btnScan.Enabled = false;
                this.Cursor = Cursors.Default;
            }
        }

        private List<string> RemoveRegisteredComponents(List<string> files, bool isBatch)
        {
            List<string> tmpFiles = new List<string>();
            foreach (string file in files)
            {
                DataRow[] drResults;
                if (isBatch)
                {
                    drResults = frame.dtWindowsInstalled.Select("Location = '" + file + @"\HierarchyLoad.exe'");
                }
                else
                {
                    drResults = frame.dtWindowsInstalled.Select("Location = '" + file + "'");
                }

                if (drResults.Length == 0)
                {
                    tmpFiles.Add(file);
                }
                else
                {
                    if (!frame.htMIDRegistered.ContainsKey(file))
                    {
                        tmpFiles.Add(file);
                    }
                }
            }
            return tmpFiles;
        }
                            
        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //deselect all fo the nodes
            foreach (TreeNode node in tvInstalledComponents.Nodes)
            {
                node.Checked = false;

                foreach (TreeNode sub_node in node.Nodes)
                {
                    sub_node.Checked = false;
                }
            }
        }

        private void cmInstalledCompTree_Opening(object sender, CancelEventArgs e)
        {
            //counters
            int check_ctr = 0;
            int uncheck_ctr = 0;

            //inventory the checked nodes
            foreach (TreeNode node in tvInstalledComponents.Nodes)
            {
                if (node.Checked == true)
                {
                    check_ctr++;
                }
                else
                {
                    uncheck_ctr++;
                }
            }

            //set menu items enabled/disabled as needed
            if (check_ctr > 0)
            {
                deselectAllToolStripMenuItem.Enabled = false;
                selectAllToolStripMenuItem.Enabled = true;
            }
            else if (uncheck_ctr > 0)
            {
                deselectAllToolStripMenuItem.Enabled = true;
                selectAllToolStripMenuItem.Enabled = false;
            }
            else
            {
                deselectAllToolStripMenuItem.Enabled = true;
                selectAllToolStripMenuItem.Enabled = true;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //make a global node selection
            foreach (TreeNode node in tvInstalledComponents.Nodes)
            {
                node.Checked = true;

                foreach (TreeNode sub_node in node.Nodes)
                {
                    sub_node.Checked = true;
                }
            }
        }

        private void tvInstalledComponents_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //do unto the child nodes as the parent node would have done unto it
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Checked == false)
                {
                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        node.Checked = false;
                    }
                }
                else
                {
                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        node.Checked = true;
                    }
                }
            }
        }

        /* read only property to return the list of items from the tree
        view control that need registered */
        List<string> lstItemsToRegister = new List<string>();
        public List<string> ItemsToRegister
        {
            get
            {
                lstItemsToRegister.Clear();
                foreach (TreeNode node in tvInstalledComponents.Nodes)
                {
                    foreach (TreeNode sub_node in node.Nodes)
                    {
                        if (sub_node.Checked == true)
                        {
                            lstItemsToRegister.Add(sub_node.Text);
                        }
                    }
                }

                return lstItemsToRegister;
            }
        }

        private DataTable GetWindowsInstalledList()
        {
            //  Begin TT#1192 - Batch executable files blocked after initial install/upgrade
            string _registrylocation = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\";
            if (Is64bit == false)
            {
                _registrylocation += "UserData\\";
            }
            //  End TT#1192

            //MessageBox.Show("IntPtr Size = " + IntPtr.Size.ToString());
            //MessageBox.Show(_registrylocation);
            //MessageBox.Show("Is64bit = " + Is64bit.ToString());
            //MessageBox.Show(GetSoftwareRoot().ToString());

            //return value variable
            Hashtable htWindowsInstalled = new Hashtable();
            DataTable dtWindowsInstalled = new DataTable();

            //create colunms and add them to the datatable
            DataColumn col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "UninstallKey";
            dtWindowsInstalled.Columns.Add(col);

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "InstallerProductKey";
            dtWindowsInstalled.Columns.Add(col);

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "Location";
            dtWindowsInstalled.Columns.Add(col);


            //drill down to the uninstall keys in the registry
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey subKey1 = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] subKeyNames = subKey1.GetSubKeyNames();

            //list of the uninstall keys
            ArrayList alUninstallKeys = new ArrayList();

            //loop thru the keys
            foreach (string subKeyName in subKeyNames)
            {
                //get a list of the sub keys
                Microsoft.Win32.RegistryKey subKey2 = subKey1.OpenSubKey(subKeyName);

                //fill the uninstall keys list
                if (ValueNameExists(subKey2.GetValueNames(), "DisplayName") &&
                    subKey2.GetValue("DisplayName").ToString().Contains("MID"))
                {
                    if (ValueNameExists(subKey2.GetValueNames(), "UninstallString"))
                    {
                        Debug.Print(subKey2.GetValue("UninstallString").ToString().Trim());
                        int UninstallKeyStart = subKey2.GetValue("UninstallString").ToString().IndexOf("{") + 1;
                        int UninstallKeyEnd = subKey2.GetValue("UninstallString").ToString().IndexOf("}") - 1;
                        int KeyLength = UninstallKeyEnd - UninstallKeyStart + 1;
                        //  TT#1554 - GRT - Scanning for previously installed components gives error message after upgrading
                        //      added this check for KeyLength being less than zero
                        if (!(KeyLength < 0))
                            alUninstallKeys.Add(subKey2.GetValue("UninstallString").ToString().Substring(UninstallKeyStart, KeyLength));
                    }
                }

                //close the subkey
                subKey2.Close();
            }

            //close the sub key
            subKey1.Close();

            //drill down to the installer/products keys in the registry
            Microsoft.Win32.RegistryKey clsRegKey = Microsoft.Win32.Registry.ClassesRoot;
            Microsoft.Win32.RegistryKey clsSubKey = clsRegKey.OpenSubKey("Installer\\Products");
            string[] clsSubKeyNames = clsSubKey.GetSubKeyNames();

            //sub key list
            //ArrayList SubKeyFolderNames = new ArrayList();

            //loop thru the keys and read the product icon value
            foreach (string clsSubKeyName in clsSubKeyNames)
            {
                Microsoft.Win32.RegistryKey clsSubKey2 = clsSubKey.OpenSubKey(clsSubKeyName);

                if (ValueNameExists(clsSubKey2.GetValueNames(), "ProductIcon"))
                {
                    foreach (string UninstallKey in alUninstallKeys)
                    {
                        if (clsSubKey2.GetValue("ProductIcon").ToString().IndexOf(UninstallKey) >= 0)
                        {
                            //uninstall keys hashtable
                            htWindowsInstalled.Add(clsSubKeyName,UninstallKey);
                        }
                    }
                }
            }

            //drill down to the installer/products keys in the registry
            Microsoft.Win32.RegistryKey userDataRegKey = Microsoft.Win32.Registry.LocalMachine;
            //  Begin TT#1192
            //  use registrylocation string for subkey
            Microsoft.Win32.RegistryKey userDataSubKey = userDataRegKey.OpenSubKey(_registrylocation);
            if (userDataSubKey != null)
            {
                //  End TT#1192
                string[] userDataSubKeyNames = userDataSubKey.GetSubKeyNames();

                //list of the user sub keys
                ArrayList userSubKeyFolderName = new ArrayList();

                //loop thru the components and fill the hashtable with the files install by the windows installer
                foreach (string userDataSubKeyName in userDataSubKeyNames)
                {
                    Microsoft.Win32.RegistryKey userRegKey = Microsoft.Win32.Registry.LocalMachine;
                    //  Begin TT#1192
                    //  use registrylocation string for subkey
                    Microsoft.Win32.RegistryKey userSubKey =
                        userRegKey.OpenSubKey(_registrylocation + userDataSubKeyName + "\\Components\\");
                    //  End TT#1192
                    if (userSubKey == null)
                    {
                        continue;
                    }
                    string[] userSubKeyNames = userSubKey.GetSubKeyNames();

                    foreach (string userSubKeyName in userSubKeyNames)
                    {
                        Microsoft.Win32.RegistryKey userSubKey2 = userSubKey.OpenSubKey(userSubKeyName);
                        string[] valueNames = userSubKey2.GetValueNames();

                        foreach (string valueName in valueNames)
                        {
                            if (htWindowsInstalled.Contains(valueName) == true)
                            {
                                //get the uninstall key value
                                string strUninstallKey = htWindowsInstalled[valueName].ToString();

                                if (userSubKey2.GetValue(valueName).ToString().EndsWith(".exe") == true ||
                                    userSubKey2.GetValue(valueName).ToString().EndsWith(".config") == true)
                                {
                                    //create the datatable to pass
                                    DataRow row = dtWindowsInstalled.NewRow();
                                    row["UninstallKey"] = strUninstallKey;
                                    row["InstallerProductKey"] = valueName;
                                    row["Location"] = userSubKey2.GetValue(valueName).ToString().Trim();
                                    dtWindowsInstalled.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }   //  End TT#1192
            return dtWindowsInstalled;
        }

        //look thru a list of values for the value given
        static private bool ValueNameExists(string[] valueNames, string valueName)
        {
            foreach (string s in valueNames)
            {
                if (s.ToLower() == valueName.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        //does the tree view have nodes
        public bool HasNodes
        {
            get
            {
                if (tvInstalledComponents.Nodes.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
