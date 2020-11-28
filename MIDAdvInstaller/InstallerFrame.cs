using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using System.Management;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Data.SqlClient;

using System.Text;
using System.Security;
using System.Text.RegularExpressions;

namespace MIDRetailInstaller
{
    // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
    public enum CompatibilityLevel
    {
        Undefined = 0,
        SQL2005 = 90,
        SQL2008 = 100,
        SQL2012 = 110,
        SQL2014 = 120,
        SQL2016 = 130,
        SQL2017 = 140  // Begin TT#1952-MD - AGallagher - SQL 2017 - Installer issues
    }
    // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install

    public partial class InstallerFrame : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        public const int SM_TABLETPC = 86;
        OSVERSIONINFOEX osVersionInfo;

        // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization

        public enum OSVersion
        {
            TabletPC,
            TabletPCSP2Over,
            NormalXP,
            Normal2000,
            None
        };

        ToolTip tt = new ToolTip();

        //  BEGIN TT#1283
        internal RegistryOperations RO = new RegistryOperations();
        //  END TT#1283

        // Begin TT#74 MD - JSmith - One-button Upgrade
        VersionConfirmation _versionconfirmationform = null;
        System.Security.Principal.WindowsIdentity currentUser;
        // End TT#74 MD

        // Begin TT#195 MD - JSmith - Add environment authentication
        private ArrayList alValidClientVersions = new ArrayList();
        private ArrayList alValidServerVersions = new ArrayList();
        private bool blIsInterpriseServer = false;
        // End TT#195 MD

        // Begin TT#74 MD - JSmith - One-button Upgrade
        //public InstallerFrame()
        //{
        //    InitializeComponent();

        //    //create the log viewer object
        //    help_view = new InstallHelpViewer();

        //    //this.ucInstallationLog1 = new MIDRetailInstaller.ucInstallationLog(this);
        //}

        public InstallerFrame(StreamWriter aInstallLog)
        {
            InitializeComponent();

            //create the log viewer object
            help_view = new InstallHelpViewer();
            currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            installLog = aInstallLog;
            installLogName = ((FileStream)installLog.BaseStream).Name;

            SetLogMessage("********************************************************************", eErrorType.message);
            SetLogMessage("**                                                                **", eErrorType.message);
            SetLogMessage("**                Installation Utility Started                    **", eErrorType.message);
            SetLogMessage("**                                                                **", eErrorType.message);
            SetLogMessage("********************************************************************", eErrorType.message);
        }
        // End TT#74 MD

        private InstallHelpViewer help_view = null;
        public string help_ID = "main";
        // Begin TT#74 MD - JSmith - One-button Upgrade
        StreamWriter installLog;
        public string installLogName;
        // End TT#74 MD

        //loaded control variable
        UserControl ucControl = null;

        //table for windows installed components
        public DataTable dtWindowsInstalled = null;
        
        //table for mid installer components
        public Hashtable htMIDInstalled = null;

        //table for mid registered components
        public Hashtable htMIDRegistered = null;
		// Begin TT#1668 - JSmith - Install Log
        bool blServerComponentsInstalled = false;
        bool blBatchComponentsInstalled = false;
        bool blClientComponentsInstalled = false;
		// End TT#1668
        bool blAPIComponentsInstalled = false;  // TT#3763 - JSmith - One-Click Upgrade fails for batch servers

        //installer data
        public DataSet installer_data;

        DataTable dtText = null;
        DataTable dtHelp = null;
        DataTable dtTooltip = null;
        DataTable dtRebrand = null;

        //installer location
        string sInstallerLocation;

        //installed component counters
        int intClient = 0;
        int intContServ = 0;
        int intStoreServ = 0;
        int intMerchServ = 0;
        int intSchedServ = 0;
        int intAppServ = 0;
        int intConfig = 0;
        int intAPI = 0;

        int cHideHeight = 0;
        int cShowHeight = 0;

        //workflow class
        workflow ProcessControl = new workflow();

        //os platform variable
        public bool _64bit = false;
        public bool _64bitOS = false; // add new one as to not have to change all other code
        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        public eOSType OSType;
        //End TT#883
        public bool blPreviousInstalls;
        public bool blPerformingOneClickUpgrade = false;
        public bool blPerformingTypicalInstall = false;
        public string strInstallClientFolder = ConfigurationManager.AppSettings["InstallClientFolder"].ToString();
        public string strInstallApplicationServiceFolder = ConfigurationManager.AppSettings["InstallApplicationServiceFolder"].ToString();
        public string strInstallControlServiceFolder = ConfigurationManager.AppSettings["InstallControlServiceFolder"].ToString();
        public string strInstallMerchandiseServiceFolder = ConfigurationManager.AppSettings["InstallMerchandiseServiceFolder"].ToString();
        public string strInstallStoreServiceFolder = ConfigurationManager.AppSettings["InstallStoreServiceFolder"].ToString();
        public string strInstallSchedulerServiceFolder = ConfigurationManager.AppSettings["InstallSchedulerServiceFolder"].ToString();
        public string strInstallAPIFolder = ConfigurationManager.AppSettings["InstallAPIFolder"].ToString();
        public string strAutoUpgradeClientLocation = ConfigurationManager.AppSettings["AutoUpgradeClientLocation"].ToString();
        Hashtable htApplicationFolders = new Hashtable();
        Hashtable htApplicationComponentFolders = new Hashtable();
        Hashtable htApplicationRootFolders = new Hashtable();
        Hashtable htSystemFolder = new Hashtable();
        Hashtable htApplicationSubFolders = new Hashtable();

        // Begin TT#195 MD - JSmith - Add environment authentication
        // Begin TT#1267
        //public bool serverversioncheck;
        // End TT#1267
        // End TT#195 MD

        // Begin TT#1668 - JSmith - Install Log
        string _logLocation = null;

        private List<string> _doNotReplaceList = null;	// TT#1656-MD - stodd - New Installation of application is overlaying the HeaderKeys.txt file

        public string LogLocation
        {
            get
            {
                return _logLocation;
            }
        }

        public StreamWriter InstallLog
        {
            get
            {
                return installLog;
            }
        }
		// End TT#1668

        // Begin TT#195 MD - JSmith - Add environment authentication
        public bool isServerEnterprise
        {
            get
            {
                return blIsInterpriseServer;
            }
        }
        // End TT#195 MD

		// Begin TT#1656-MD - stodd - New Installation of application is overlaying the HeaderKeys.txt file
        public List<string> DoNotReplaceList
        {
            get
            {
                if (_doNotReplaceList == null)
                {
                    _doNotReplaceList = new List<string>();
                    DataSet installer_data = new DataSet();
                    installer_data.ReadXmlSchema(Application.StartupPath + @"\installerdonotreplace.xsd");
                    installer_data.ReadXml(Application.StartupPath + @"\installerdonotreplace.xml");
                    DataTable dtDoNoReplace = installer_data.Tables["do_not_replace"];
                    foreach (DataRow aRow in dtDoNoReplace.Rows)
                    {
                        string fileName = aRow["file"].ToString();
                        _doNotReplaceList.Add(fileName);
                    }
                }
                return _doNotReplaceList;
            }
        }
		// End TT#1656-MD - stodd - New Installation of application is overlaying the HeaderKeys.txt file


        // Begin TT#195 MD - JSmith - Add environment authentication
        //// Begin TT#1299 - gtaylor - Verify edition of operating system is enterprise
        private static bool isEnterpriseServer()
        {
            string server_version = "";
            bool isEnterprise = false;
            ObjectQuery wmios = new WqlObjectQuery("Select * from Win32_OperatingSystem");
            ManagementObjectSearcher oss = new ManagementObjectSearcher(wmios);

            foreach (ManagementObject os in oss.Get())
            {
                server_version += os["Caption"];
            }
            //  comment to test
            // Begin TT#4517 - JSmith - Datacenter operating system not recognized
            //isEnterprise = server_version.ToString().ToUpper().Contains("ENTERPRISE");
            isEnterprise = server_version.ToString().ToUpper().Contains("ENTERPRISE") || server_version.ToString().ToUpper().Contains("DATACENTER") || server_version.ToString().ToUpper().Contains("STANDARD");
            // End TT#4517 - JSmith - Datacenter operating system not recognized
            //  uncomment to test
            //isEnterprise = false;

            return isEnterprise;
        }

        public bool isValidServer()
        {
            string ost = OSType.ToString().ToUpper();
            bool isValid = false;

            //  comment to test
            foreach (string version in alValidServerVersions)
            {
                isValid = (ost == version.ToUpper());
                if (isValid)
                {
                    break;
                }
            }
            //  uncomment to test
            //isEnterprise = false;

            return isValid;
        }

        //private static bool isXPProfessional()
        //{
        //    string server_version = "";
        //    bool isXPProfessional = false;
        //    ObjectQuery wmios = new WqlObjectQuery("Select * from Win32_OperatingSystem");
        //    ManagementObjectSearcher oss = new ManagementObjectSearcher(wmios);

        //    foreach (ManagementObject os in oss.Get())
        //    {
        //        server_version += os["Caption"];
        //    }
        //    //  comment to test
        //    isXPProfessional = server_version.ToString().ToUpper().Contains("XP PROFESSIONAL");
        //    //  uncomment to test
        //    //isXPProfessional = false;

        //    return isXPProfessional;
        //}
        //// End TT#1299

        public bool isValidClient()
        {
            //string server_version = "";
            string ost = OSType.ToString().ToUpper();
            bool isValid = false;
            //ObjectQuery wmios = new WqlObjectQuery("Select * from Win32_OperatingSystem");
            //ManagementObjectSearcher oss = new ManagementObjectSearcher(wmios);

            //foreach (ManagementObject os in oss.Get())
            //{
            //    server_version += os["Caption"];
            //}
            //  comment to test
            foreach (string version in alValidClientVersions)
            {
                //isValid = server_version.ToString().ToUpper().Contains(version);
                isValid = (ost == version.ToUpper());
                if (isValid)
                {
                    break;
                }
            }
            //  uncomment to test
            //isEnterprise = false;

            return isValid;
        }
        // End TT#195 MD

        // Begin TT#1192
        public static bool is64Bit()
        {
            // Begin TT#263-MD - JSmith - Install on new 32 bit Windows 7 machine fails
            //bool is64bit = false;
            //string server_description = "";
            //ObjectQuery wmios = new WqlObjectQuery("Select * from Win32_Processor");
            //ManagementObjectSearcher oss = new ManagementObjectSearcher(wmios);

            //foreach (ManagementObject os in oss.Get())
            //{
            //    server_description += os["Architecture"].ToString();
            //}
            ////  comment to test
            //is64bit = server_description.ToString().ToUpper().Contains("9");
            ////  uncomment to test
            ////isEnterprise = false;

            //return is64bit;
            return (IntPtr.Size == 8);
            // End TT#263-MD - JSmith - Install on new 32 bit Windows 7 machine fails
        }
        // End TT#1192

        private void InstallerFrame_Load(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(Path.GetDirectoryName(Application.ExecutablePath));

                //MessageBox.Show(File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\Install Files\Client\" + "MIDRetail.Windows.dll").ToString());

                //7/17/09 - Commented out until the time when and if we do true 64-bit builds
                //set os platform variable
                if (IntPtr.Size == 8)
                {
                    //_64bit = true;
                    _64bitOS = true;
                }
                //IDictionary env = Environment.GetEnvironmentVariables();

                //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
                OSType = GetOS();
                //End TT#883

                // Begin TT#74 MD - JSmith - One-button Upgrade
                _versionconfirmationform = new VersionConfirmation(this);
                // End TT#74 MD

                ucInstallationLog1.Frame = this;

                //add a start entry to the log
                ucInstallationLog1.AddLogEntry("Installation Began:" + DateTime.Now.ToString().Trim(), eErrorType.message);
                if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                {
                    ucInstallationLog1.AddLogEntry("Utility executed by " + System.Environment.UserName + " from remote machine " + GetTerminalServerClientNameWTSAPI() + ".", eErrorType.message);
                }
                else
                {
                    ucInstallationLog1.AddLogEntry("Utility executed by " + System.Environment.UserName, eErrorType.message);
                }

                cShowHeight = pnFrame.Height;
                cHideHeight = pnFrame.Height + this.ucInstallationLog1.Height + 5;
                pnFrame.Height = cHideHeight;

                //get installer data
                installer_data = GetInstallerData();

                dtText = installer_data.Tables["install_text"];

                dtHelp = installer_data.Tables["help_text"];

                dtTooltip = installer_data.Tables["tooltip_text"];

                dtRebrand = installer_data.Tables["rebrand"];
                tt.SetToolTip(picHelp, GetToolTipText("frame_btnHelp"));
                tt.SetToolTip(btnDetail, GetToolTipText("frame_btnDetail"));
                tt.SetToolTip(btnBack, GetToolTipText("frame_btnBack"));
                tt.SetToolTip(btnCancel, GetToolTipText("frame_btnCancel"));
                tt.SetToolTip(btnNext, GetToolTipText("frame_btnNext"));
                tt.SetToolTip(btnProcess, GetToolTipText("frame_btnProcess"));
                tt.SetToolTip(btnSave, GetToolTipText("frame_btnSave"));

                SetLogMessage(GetText("InstallerLocation").Replace("{0}", Application.ExecutablePath), eErrorType.message);
                string assemblyName = Path.GetDirectoryName(Application.ExecutablePath) + @"\Install Files\Client\" + "MIDRetail.Windows.dll";
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);
                SetLogMessage(GetText("ProductVersion").Replace("{0}", fvi.FileVersion), eErrorType.message);
                SetLogMessage(GetText("FrameworkVersion").Replace("{0}", OSInfo.FrameworkVersion), eErrorType.message);
                if (_64bitOS)
                {
                    SetLogMessage(GetText("64BitYes").Replace("{0}", Environment.MachineName), eErrorType.message);
                }
                else
                {
                    SetLogMessage(GetText("64BitNo").Replace("{0}", Environment.MachineName), eErrorType.message);
                }

                sInstallerLocation = GetInstallerLocation();

                SetLogMessage(GetText("FrameworkInstallerLocation").Replace("{0}", sInstallerLocation), eErrorType.message);
                // Begin TT#1298 - gtaylor - new panel to ask if documentaion has been reviewed before install is attempted
                // Begin TT#74 MD - JSmith - One-button Upgrade
                //InstallationDocumentationConfirm _installationdocumentationconfirm = new InstallationDocumentationConfirm();
                InstallationDocumentationConfirm _installationdocumentationconfirm = new InstallationDocumentationConfirm(this);
                // End TT#74 MD
                if (_installationdocumentationconfirm.ShowDialog() == DialogResult.Cancel)
                {
                    this.Visible = false;
                    this.Close();
                    this.Dispose();
                    return;
                }
                // End TT#1298

                // Begin TT#195 MD - JSmith - Add environment authentication
                GetValidVersions();
                // End TT#195 MD

                // Begin TT#1267 - GTaylor - Verify edition of operating system is enterprise
                // Begin TT#195 MD - JSmith - Add environment authentication
                //serverversioncheck = ServerVersionCheck();
                // End TT#195 MD
                //lblNotEnterprise.Text = GetText("lblNotEnterprise");
                ucInstallationLog1.AddLogEntry("OS Name:" + OSInfo.Name, eErrorType.message);
                ucInstallationLog1.AddLogEntry("OS Edition:" + OSInfo.Edition, eErrorType.message);
                ucInstallationLog1.AddLogEntry("OS Version:" + OSInfo.VersionString, eErrorType.message);
                ucInstallationLog1.AddLogEntry("OS Platform Type:" + osVersionInfo.dwPlatformId.ToString(), eErrorType.message);
                ucInstallationLog1.AddLogEntry("OS Product Type:" + osVersionInfo.wProductType.ToString(), eErrorType.message);
                ucInstallationLog1.AddLogEntry("OS Service Pack:" + OSInfo.ServicePack, eErrorType.message);
                //ucInstallationLog1.AddLogEntry("OS Found:" + Enum.GetName(typeof(eOSType), OSType).ToString(), eErrorType.message);
                //lblNotEnterprise.Visible = (!this.serverversioncheck);
                // Begin TT#195 MD - JSmith - Add environment authentication
                //lblNotEnterprise.Visible = (!(isEnterpriseServer()));
                lblNotEnterprise.Visible = false;
                blIsInterpriseServer = isEnterpriseServer();
                //if (serverversioncheck)
                //// End TT#1267
                //{
                // End TT#195 MD
                    // Begin TT#1267
                    ucInstallationLog1.AddLogEntry("Passed Server Version Check.", eErrorType.message);
                    // End TT#1267
                    //disable the back button... no prior panes
                    btnBack.Enabled = false;

                    // for testing, comment out before checked in.
                    //RemoveRegisteredComponents();
                    if (RegistryNeedsConverted())
                    {
                        ucInstallationLog1.AddLogEntry("Converting registry entries.", eErrorType.message);
                        ConvertRegistryEntries();
                    }

                    //check for previous installs
                    blPreviousInstalls = CheckPreviousInstalls();

                    if (blPreviousInstalls == false)
                    {
                        //load the scanning pane
                        ucScan first = new ucScan(ucInstallationLog1, this);
                        ucControl = first;
                        LoadUserControl();

                        htMIDInstalled = new Hashtable();
                        htMIDRegistered = new Hashtable();
                        btnNext_Click(this, new EventArgs());
                    }
                    else
                    {
                        //fill the Windows installed components table
                        dtWindowsInstalled = GetRegisteredComponents();

                        //fill the MID install components array
                        bool blNeedsScanned = false;
                        htMIDInstalled = GetMIDInstalledComponenets(ref blNeedsScanned);
                        // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
                        //htMIDRegistered = GetMIDRegisteredComponents();
                        if (!blNeedsScanned)
                        {
                            htMIDRegistered = GetMIDRegisteredComponents(ref blNeedsScanned);
                        }
                        // End TT#68 MD

                        // Begin TT#1668 - JSmith - Install Log
                        if (blServerComponentsInstalled)
                        {
                            SetLogMessage(GetText("envServer"), eErrorType.message);
                        }
                        else if (blBatchComponentsInstalled)
                        {
                            SetLogMessage(GetText("envBatch"), eErrorType.message);
                        }
                        else if (blClientComponentsInstalled)
                        {
                            SetLogMessage(GetText("envClient"), eErrorType.message);
                        }
                        else
                        {
                            SetLogMessage(GetText("envNewInstall"), eErrorType.message);
                        }
						// End TT#1668

                        if (blNeedsScanned)
                        {
						    // Begin TT#1668 - JSmith - Install Log
							//MessageBox.Show("The installer detected inconsistencies and must rescan the machine.", "Rescan required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MessageBox.Show(GetText("RescanRequired"), "Rescan required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            SetLogMessage(GetText("RescanRequired"), eErrorType.warning);
							// End TT#1668
                            //show user the choice of installs
                            ucUtilities utilities = new ucUtilities(this, ucInstallationLog1, blNeedsScanned, true);
                            ucControl = utilities;
                            LoadUserControl();
                        }
                        else
                        {
                            //show user the choice of installs
                            ucInstallChooser install_chooser = new ucInstallChooser(this);
                            //add event handlers
                            install_chooser.NotConfigureNext += new EventHandler(this.NotConfigureNext);
                            install_chooser.ConfigureNext += new EventHandler(this.ConfigureNext);
                            ucControl = install_chooser;
                            LoadUserControl();
                        }
                    }

                htApplicationFolders.Add(strInstallClientFolder, null);
                htApplicationFolders.Add(strInstallApplicationServiceFolder, null);
                htApplicationFolders.Add(strInstallControlServiceFolder, null);
                htApplicationFolders.Add(strInstallMerchandiseServiceFolder, null);
                htApplicationFolders.Add(strInstallStoreServiceFolder, null);
                htApplicationFolders.Add(strInstallSchedulerServiceFolder, null);
                htApplicationFolders.Add(strInstallAPIFolder, null);
                htApplicationFolders.Add("AutoUpgrade", null);
                htApplicationFolders.Add("GlobalSettings", null);
                htApplicationFolders.Add("Graphics", null);
                htApplicationFolders.Add("StoreDelete", null);
                htApplicationFolders.Add("Transactions", null);

                htApplicationSubFolders.Add("AutoUpgrade", null);
                htApplicationSubFolders.Add("GlobalSettings", null);
                htApplicationSubFolders.Add("Graphics", null);
                htApplicationSubFolders.Add("StoreDelete", null);
                htApplicationSubFolders.Add("Transactions", null);

                htApplicationComponentFolders.Add(strInstallClientFolder, null);
                htApplicationComponentFolders.Add(strInstallApplicationServiceFolder, null);
                htApplicationComponentFolders.Add(strInstallControlServiceFolder, null);
                htApplicationComponentFolders.Add(strInstallMerchandiseServiceFolder, null);
                htApplicationComponentFolders.Add(strInstallStoreServiceFolder, null);
                htApplicationComponentFolders.Add(strInstallSchedulerServiceFolder, null);
                htApplicationComponentFolders.Add(strInstallAPIFolder, null);

                string component;
                DirectoryInfo di;

                // get root folders
                if (htMIDRegistered != null)
                {
                    foreach (DictionaryEntry MIDRegistered in htMIDRegistered)
                    {
                        component = (string)MIDRegistered.Key;
                        if (component.Contains(@"\GlobalSettings"))
                        {
                            di = Directory.GetParent(component).Parent.Parent;
                            htApplicationFolders[di.Name] = null;
                            htApplicationRootFolders[di.Name] = null;
                        }
                        else if (component.Contains(@"\Batch"))
                        {
                            di = Directory.GetParent(component);
                            htApplicationRootFolders[di.Name] = null;
                        }
                        else if (component.Contains(@"Service.exe"))
                        {
                            di = Directory.GetParent(component).Parent;
                            htApplicationRootFolders[di.Name] = null;
                        }
                    }
                }
                foreach (Environment.SpecialFolder sp in Enum.GetValues(typeof(Environment.SpecialFolder)))
                {
                    htSystemFolder[Environment.GetFolderPath(sp)] = null;
                }
                // Begin TT#195 MD - JSmith - Add environment authentication
                //} // TT#1267
                //else
                //{
                //    // Begin TT#1267
                //    this.Visible = false;
                //    ucInstallationLog1.AddLogEntry("Failed Server Version Check.", eErrorType.message);
                //    // Begin TT#1668 - JSmith - Install Log
                //    //MessageBox.Show("The Setup has stopped due to user response.", "Server Version Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    SetLogMessage(GetText("SetupStoppedByUser"), eErrorType.error);
                //    MessageBox.Show(GetText("SetupStoppedByUser"), "Server Version Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    // End TT#1668
                //    this.Close();
                //    this.Dispose();
                //    // End TT#1267
                //}
                // End TT#195 MD
            }
			// Begin TT#1668 - JSmith - Install Log
            catch (Exception ex)
            {
                SetLogMessage(ex.ToString(), eErrorType.error);
                throw;
            }
			// End TT#1668
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        private void GetValidVersions()
        {
            alValidClientVersions = new ArrayList();
            string[] versions = ConfigurationManager.AppSettings["SupportedWindowsClientVersions"].ToString().Split(';');
            foreach (string CompatibleVersion in versions)
            {
                if (!string.IsNullOrEmpty(CompatibleVersion))
                {
                    alValidClientVersions.Add(CompatibleVersion.Trim().ToUpper());
                }
            }
            
            alValidServerVersions = new ArrayList();
            versions = ConfigurationManager.AppSettings["SupportedWindowsServerVersions"].ToString().Split(';');
            foreach (string CompatibleVersion in versions)
            {
                if (!string.IsNullOrEmpty(CompatibleVersion))
                {
                    alValidServerVersions.Add(CompatibleVersion.Trim().ToUpper());
                }
            }
        }
        // End TT#195 MD

        // Begin TT#195 MD - JSmith - Add environment authentication
        // Begin TT#1267 - gtaylor - Verify edition of operating system is enterprise
        // Begin TT#74 MD - JSmith - One-button Upgrade
        //static private bool ServerVersionCheck()
        public bool ServerVersionCheck()
        // End TT#74 MD
        {
            bool validserverinstall = false;
            // Begin TT#74 MD - JSmith - One-button Upgrade
            //VersionConfirmation _versionconfirmationform;
            // End TT#74 MD
            bool enterprise_server;
            //bool XP_Professional;

            //  comment to test
            enterprise_server = isEnterpriseServer();
            //XP_Professional = isXPProfessional();

            //  uncomment to test
            //enterprise_server = false;

            //
            //  didn't find ENTERPRISE in the Version String
            //  load the form
            //  wait for user response
            //
            if (enterprise_server == false)
            {
                // Begin TT#74 MD - JSmith - One-button Upgrade
                //_versionconfirmationform = new VersionConfirmation();
                // End TT#74 MD
                switch (_versionconfirmationform.ShowDialog())
                {
                    case DialogResult.OK:
                        //  continue pressed, ok
                        validserverinstall = true;
                        break;
                    default:
                        //  terminate pressed
                        validserverinstall = false;
                        break;
                }
            }
            else
            {
                //  found it, continue
                validserverinstall = true;
            }
            return validserverinstall;
        }
        // End #1267
        // End TT#195 MD

        //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
        private eOSType GetOS()
        {
            OperatingSystem osVersion = Environment.OSVersion;
            osVersionInfo = new OSVERSIONINFOEX();
            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            if (GetVersionEx(ref osVersionInfo))
            {
                int majorVersion = osVersion.Version.Major;
                int minorVersion = osVersion.Version.Minor;

                switch (osVersion.Platform)
                {
                    case PlatformID.Win32Windows:
                        {
                            if (majorVersion == 4)
                            {
                                string csdVersion = osVersionInfo.szCSDVersion;
                                switch (minorVersion)
                                {
                                    case 0:
                                        if (csdVersion == "B" || csdVersion == "C")
                                            return eOSType.Windows95OSR2;
                                        else
                                            return eOSType.Windows95;
                                    case 10:
                                        if (csdVersion == "A")
                                            return eOSType.Windows98SecondEdition;
                                        else
                                            return eOSType.Windows98;
                                    case 90:
                                        return eOSType.WindowsMe;
                                }
                            }
                            break;
                        }

                    case PlatformID.Win32NT:
                        {
                            byte productType = osVersionInfo.wProductType;

                            switch (majorVersion)
                            {
                                case 3:
                                    return eOSType.WindowsNT351;
                                case 4:
                                    switch (productType)
                                    {
                                        case 1:
                                            return eOSType.WindowsNT40;
                                        case 3:
                                            return eOSType.WindowsNT40Server;
                                    }
                                    break;
                                case 5:
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            return eOSType.Windows2000;
                                        case 1:
                                            return eOSType.WindowsXP;
                                        case 2:
                                            return eOSType.WindowsServer2003;
                                    }
                                    break;
                                case 6:
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            switch (productType)
                                            {
                                                case 1:
                                                    return eOSType.WindowsVista;
                                                case 3:
                                                    return eOSType.WindowsServer2008;
                                            }
                                            break;
                                        case 1:
                                            switch (productType)
                                            {
                                                case 1:
                                                    return eOSType.Windows7;
                                                case 3:
                                                    return eOSType.WindowsServer2008R2;
                                            }
                                            break;
                                        case 2:
                                            switch (productType)
                                            {
                                                case 1:
                                                    return eOSType.Windows8;
                                                case 3:
                                                    return eOSType.WindowsServer2012;
                                                // Begin TT#1952-MD - AGallagher - OS 2016 - Installer issues
                                                case 5:
                                                    return eOSType.WindowsServer2016;
                                                // End TT#1952-MD - AGallagher - OS 2016 - Installer issues
                                            }
                                            break;
                                    }

                                    break;
                            }
                            break;
                        }
                }
            }
            return eOSType.Unknown;
        }
        //End TT#883

        /// <summary>
        /// Returns the machine name where the session is located.
        /// </summary>

        public string GetMachineName()
        {
            try
            {
                // Get machine name
                string hostName = System.Net.Dns.GetHostName();
                string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                string fqdn = "";
                if (!hostName.Contains(domainName))
                {
                    if (domainName != null && domainName.Trim().Length > 0)
                    {
                        fqdn = hostName + "." + domainName;
                    }
                    else
                    {
                        fqdn = hostName;
                    }
                }
                else
                {
                    fqdn = hostName;
                }
                return fqdn;
                
                //return Environment.MachineName + "." + Environment.GetEnvironmentVariable("USERDNSDOMAIN");
            }
            catch
            {
                throw;
            }
        }

        // TT#1267 - gtaylor
        //  changed the protection level to internal from private
        internal DataSet GetInstallerData()
        {
            //create dataset from installer file
            DataSet installer_data = new DataSet();
            installer_data.ReadXmlSchema(Application.StartupPath + @"\installer.xsd");
            installer_data.ReadXml(Application.StartupPath + @"\installer.xml");
            //return dataset
            return installer_data;
        }

        public string GetText(string id)
        {
            string text = null;
            DataRow[] drText = null;
            drText = dtText.Select("id = '" + id + "'");
            if (drText.Length > 0)
            {
                text = drText[0].Field<string>("text");
                // BEGIN TT#1267 - gtaylor
                text = text.Replace(@"\n", Environment.NewLine);
                // END TT#1267
            }
            return text;
        }

        public string GetHelp(string id)
        {
            string text = null;
            DataRow[] drText = null;
            drText = dtHelp.Select("id = '" + id + "'");
            if (drText.Length > 0)
            {
                text = drText[0].Field<string>("text");
                text = text.Replace(@"\n", Environment.NewLine);
            }
            return text;
        }

        public string GetToolTipText(string id)
        {
            string text = null;
            DataRow[] drText = null;
            drText = dtTooltip.Select("id = '" + id + "'");
            if (drText.Length > 0)
            {
                text = drText[0].Field<string>("text");
                text = text.Replace(@"\n", Environment.NewLine);
            }
            return text;
        }

        // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
        //public void SetStatusMessage(string message)
        public void SetStatusMessage(string message, eErrorType errorType = eErrorType.message)
        // End TT#1822-MD - JSmith - Installer not detecting incomplete install
        {
            lblStatus.Text = message;
            Application.DoEvents();

            // Begin TT#74 MD - JSmith - One-button Upgrade
            if (message != null &&
                message.Trim().Length > 0)
            {
                // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                //SetLogMessage(message, errorType);
                SetLogMessage(message, eErrorType.message);
                // End TT#1822-MD - JSmith - Installer not detecting incomplete install
            }
            // End TT#74 MD
        }

        // Begin TT#2792 - JSmith - Installer Crash
        public void PopUpMessage(string aMessage)
        {
            MessageBox.Show(aMessage, "Logility - RO Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void PopUpWarning(string aMessage)
        {
            MessageBox.Show(aMessage, "Logility - RO Installer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void PopUpError(string aMessage)
        {
            MessageBox.Show(aMessage, "Logility - RO Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // End TT#2792 - JSmith - Installer Crash

        // Begin TT#1668 - JSmith - Install Log
        public void SetLogMessage(string message, eErrorType aErrorType)
        {
            string msgLevel = null;
            switch (aErrorType)
            {
                case eErrorType.error:
                    msgLevel = "  Error";
                    break;
                case eErrorType.message:
                    msgLevel = "Message";
                    break;
                case eErrorType.warning:
                    msgLevel = "Warning";
                    break;
                case eErrorType.debug:
                    msgLevel = "  Debug";
                    break;

            }

            if (installLog != null &&
                IsOpen(installLog))
            {
                installLog.WriteLine(DateTime.Now.ToString("s") + " - " + currentUser.Name + " - " + msgLevel + " - " + message);
            }
        }

        // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
        public void FlushLog()
        {
            if (installLog != null &&
                            IsOpen(installLog))
            {
                string logName = ((FileStream)InstallLog.BaseStream).Name;
                string[] logNameParts = logName.Split('\\');
                installLog.Flush();
                if (!Directory.Exists(LogLocation))
                {
                    Directory.CreateDirectory(LogLocation);
                }
                File.Copy(logName, LogLocation + @"\" + logNameParts[logNameParts.Length - 1], true);
            }
        }

        private bool VerifyUpgrade(ArrayList installedItems)
        {
            bool success = true;
            string directoryName;
            foreach (string filePath in installedItems)
            {
                FileAttributes attr = File.GetAttributes(filePath);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    directoryName = filePath;
                }
                else
                {
                    directoryName = Path.GetDirectoryName(filePath);
                }
                
                string[] upgradeFiles = Directory.GetFiles(directoryName, "*.upgrade", SearchOption.TopDirectoryOnly);
                if (upgradeFiles.Length > 0)
                {
                    SetLogMessage("upgrade files found in folder " + filePath + ".  Upgrade incomplete.  Reprocess the upgrade.", eErrorType.error);
                    success = false;
                }
            }

            return success;
        }
        // End TT#1822-MD - JSmith - Installer not detecting incomplete install

        public static bool IsOpen(StreamWriter sw)
        {
            try
            {
                if (sw != null &&
                    sw.BaseStream != null)
                {
                    sw.BaseStream.Seek(0, SeekOrigin.Current);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void OpenUpgradeLog()
        {
            if (!IsOpen(installLog))
            {
                installLog = new StreamWriter(installLogName, true);
            }
        }

        public void CloseUpgradeLog()
        {
            try
            {
                if (IsOpen(installLog))
                {
                    installLog.Close();
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#1668

        public void CopyFolder(string sourceFolder, string destFolder, List<string> copiedFiles)
        {
            if (!Directory.Exists(destFolder))
            {
                CreateDirectory(destFolder);
            }
            foreach (string file in Directory.GetFiles(sourceFolder))
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
				// End TT#1656-MD - stodd - New Installation of application is overlaying the HeaderKeys.txt file
                if (DoNotReplaceList.Contains(name))
                {
                    if (File.Exists(dest))
                    {
                        SetStatusMessage("file: " + dest + " already exists and is in the DoNotReplaceList. It will not be copied.");
                        continue;
                    }
                }
				// End TT#1656-MD - stodd - New Installation of application is overlaying the HeaderKeys.txt file
				// Begin TT#1668 - JSmith - Install Log
                SetStatusMessage("Copying file: " + file + " to " + dest);
				// End TT#1668
                File.Copy(file, dest, true);
                copiedFiles.Add(dest);
            }
            foreach (string folder in Directory.GetDirectories(sourceFolder))
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                Application.DoEvents();
				// Begin TT#1668 - JSmith - Install Log
                //lblStatus.Text = "Copying file: " + folder + " to " + dest;
                SetStatusMessage("Copying file: " + folder + " to " + dest);
				// End TT#1668
                CopyFolder(folder, dest, copiedFiles);
            }
        }
		
		public string GetOldRebrandName(string id)
        {
            string name = null;
            DataRow[] drText = null;
            if (dtRebrand != null)
            {
                drText = dtRebrand.Select("new = '" + id + "'");
                if (drText.Length > 0)
                {
                    name = drText[0].Field<string>("old");
                }
            }
            return name;
        }

       // Begin TT#932 - JSmith - Security violation update computations
        public void UpdateSecurity(string sDir, bool isClient)
        {
            try
            {
                ArrayList lstFilesFound = new ArrayList();
                // if not client, only update computations.
                if (!isClient)
                {
                    DirSearch(sDir, ConfigurationManager.AppSettings["ComputationsDll"].ToString(), lstFilesFound);
                }
                // else update all for auto upgrade
                else
                {
                    DirSearch(sDir, "*", lstFilesFound);
                }

                foreach (string file in lstFilesFound)
                {
                    AddFileSecurity(file, @"Users", System.Security.AccessControl.FileSystemRights.Modify, System.Security.AccessControl.AccessControlType.Allow);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        private void AddFileSecurity(string fileName, string account,
           System.Security.AccessControl.FileSystemRights rights, System.Security.AccessControl.AccessControlType controlType)
        {
		    // Begin TT#1668 - JSmith - Install Log
            string msg = GetText("FileSecurity");
            msg = msg.Replace("{0}", fileName);
            msg = msg.Replace("{1}", account);
            msg = msg.Replace("{2}", rights.ToString());
            msg = msg.Replace("{3}", controlType.ToString());
            SetLogMessage(msg, eErrorType.message);
			// End TT#1668
            // Get a FileSecurity object that represents the
            // current security settings.
            System.Security.AccessControl.FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }
        // End TT#932

        public void CreateDirectory(string FolderPath)
        {
		    // Begin TT#1668 - JSmith - Install Log
            string msg = GetText("CreateDirectory");
            msg = msg.Replace("{0}", FolderPath);
            SetLogMessage(msg, eErrorType.message);
			// End TT#1668

            Directory.CreateDirectory(FolderPath);
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);
            dirInfo.Attributes = FileAttributes.Normal;
        }

        public void ShareFolder(string FolderPath, string ShareName, string Description, bool readWriteAccess)
        {
            eCreateShareReturn createShareReturn;
            try
            {
			    // Begin TT#1668 - JSmith - Install Log
                string msg = GetText("ShareFolder");
                msg = msg.Replace("{0}", FolderPath);
                msg = msg.Replace("{1}", ShareName);
                msg = msg.Replace("{2}", readWriteAccess.ToString());
                SetLogMessage(msg, eErrorType.message);
				// End TT#1668

                // Create a ManagementClass object

                ManagementClass managementClass = new ManagementClass("Win32_Share");

                // Create ManagementBaseObjects for in and out parameters

                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");

                ManagementBaseObject outParams;

                // Set the input parameters

                inParams["Description"] = Description;

                inParams["Name"] = ShareName;

                inParams["Path"] = FolderPath;

                inParams["Type"] = 0x0; // Disk Drive

                inParams["Access"] = null;


                //Another Type:

                // DISK_DRIVE = 0x0

                // PRINT_QUEUE = 0x1

                // DEVICE = 0x2

                // IPC = 0x3

                // DISK_DRIVE_ADMIN = 0x80000000

                // PRINT_QUEUE_ADMIN = 0x80000001

                // DEVICE_ADMIN = 0x80000002

                // IPC_ADMIN = 0x8000003

                //inParams["MaximumAllowed"] = int maxConnectionsNum;

                // Invoke the method on the ManagementClass object
                outParams = managementClass.InvokeMethod("Create", inParams, null);
                
                // Check to see if the method invocation was successful

                createShareReturn = (eCreateShareReturn)Convert.ToInt32((outParams.Properties["ReturnValue"].Value));

                if (createShareReturn == eCreateShareReturn.Success)
                {
                    ucInstallationLog1.AddLogEntry(FolderPath + " has been shared.", eErrorType.message);
                }
                else if (createShareReturn == eCreateShareReturn.DuplicateShare)
                {
                    ucInstallationLog1.AddLogEntry("Share to " + FolderPath + " already exists.", eErrorType.message);
                }
                else
                {
                    ucInstallationLog1.AddLogEntry("Unable to share directory " + FolderPath, eErrorType.error);
                }

                //if (readWriteAccess)
                //{
                //    DirectoryInfo dInfo = new DirectoryInfo(FolderPath);
                //    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                //    // Add the FileSystemAccessRule to the security settings.    
                //    dSecurity.AddAccessRule(new FileSystemAccessRule("Everyone",
                //                                                     FileSystemRights.FullControl,
                //                                                     AccessControlType.Allow));
                //    // Set the new access settings.   
                //    dInfo.SetAccessControl(dSecurity);
                //}

            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Unable to share directory " + FolderPath + ". Reason=" + ex.Message, eErrorType.error);
            }
        }

        public void SetFolderPathNotReadOnly(string sDir)
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    SetFileNotReadOnly(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    SetFolderPathNotReadOnly(d);
                }
            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Unable to set Folder " + sDir + " not read only. Reason=" + ex.Message, eErrorType.error);
            }
        }

        public void SetFileNotReadOnly(string FileName)
        {
            try
            {
                System.IO.File.SetAttributes(FileName, System.IO.File.GetAttributes(FileName) & ~(FileAttributes.ReadOnly));
            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Unable to set file " + FileName + " not read only. Reason=" + ex.Message, eErrorType.error);
            }
        }

        public void DeleteFile(string FileName)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(FileName))
                {
                    return;
                }
			    // Begin TT#1668 - JSmith - Install Log
                string msg = GetText("DeleteFile");
                msg = msg.Replace("{0}", FileName);
                SetLogMessage(msg, eErrorType.message);
				// End TT#1668

                if (File.Exists(FileName))
                {
                   File.Delete(FileName);
                }
            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Unable to delete file " + FileName + ". Reason=" + ex.Message, eErrorType.error);
            }
        }

        public void DeleteFolder(string FolderPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FolderPath))
                {
                    return;
                }
			    // Begin TT#1668 - JSmith - Install Log
                string msg = GetText("DeleteFolder");
                msg = msg.Replace("{0}", FolderPath);
                SetLogMessage(msg, eErrorType.message);
				// End TT#1668

                if (Directory.Exists(FolderPath))
                {
                    if (DeleteFolderShares(FolderPath))
                    {
                        Directory.Delete(FolderPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Unable to delete directory " + FolderPath + ". Reason=" + ex.Message, eErrorType.error);
            }
        }

        public bool DeleteFolderShares(string FolderPath)
        {
            try
            {
			    // Begin TT#1668 - JSmith - Install Log
                string msg = GetText("RemoveFolderShare");
                msg = msg.Replace("{0}", FolderPath);
                SetLogMessage(msg, eErrorType.message);
				// End TT#1668

                ArrayList sharedFolders = new ArrayList();

                // Object to query the WMI Win32_Share API for shared files... 

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from win32_share");

                //ManagementBaseObject outParams;

                ManagementClass mc = new ManagementClass("Win32_Share"); //for local shares 

                foreach (ManagementObject share in searcher.Get())
                {

                    string type = share["Type"].ToString();

                    if (type == "0") // 0 = DiskDrive (1 = Print Queue, 2 = Device, 3 = IPH) 
                    {
                        if (share["Path"].ToString() == FolderPath)
                        {
                            sharedFolders.Add(share);
                        }
                        //string name = share["Name"].ToString(); //getting share name 

                        //string path = share["Path"].ToString(); //getting share path 

                        //string caption = share["Caption"].ToString(); //getting share description 
                    }

                }

                foreach (ManagementObject share in sharedFolders)
                {
                    share.Delete();
                }

            }
            catch (ManagementException ex)
            {
                return true;
            }
            catch (Exception ex)
            {
                ucInstallationLog1.AddLogEntry("Error occurred while removing the share to folder " + FolderPath + ". Reason=" + ex.Message, eErrorType.error);
                return false;
            }

            return true;

        }

        private string GetInstallerLocation()
        {
            string sLocation = string.Empty;
            ArrayList lstFilesFound = new ArrayList();
            DirSearch(System.Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework", "InstallUtil.exe", lstFilesFound);

            return Convert.ToString(lstFilesFound[lstFilesFound.Count - 1]);
        }

        // Begin TT#581-MD - JSmith - Configuration Cleanup
		//private void DirSearch(string sDir, string sFile, ArrayList lstFilesFound)
        public void DirSearch(string sDir, string sFile, ArrayList lstFilesFound)
		// End TT#581-MD - JSmith - Configuration Cleanup
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir, sFile))
                {
                    lstFilesFound.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    DirSearch(d, sFile, lstFilesFound);
                }
            }
            catch (Exception err)
            {
                ErrorHandler(err.Message);
            }
        }

        public bool StartService(string ServiceName)
        {
           bool started = false;
            try
            {
                SetStatusMessage("Starting service " + ServiceName);

                // Check whether the service is started.

                ServiceController sc = new ServiceController(ServiceName);

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    // Start the service if the current status is stopped.
                    try
                    {
                        // Start the service, and wait until its status is "Running".
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 10, 0));
                        started = true;

                        // Display the current service status.
                        ucInstallationLog1.AddLogEntry(ServiceName + " successfully started.", eErrorType.message);
                    }
                    catch (System.ServiceProcess.TimeoutException)
                    {
                        ucInstallationLog1.AddLogEntry("Service " + ServiceName + " did not start in a timely manner. Check Event Viewer", eErrorType.error);
                    }
                    catch (InvalidOperationException)
                    {
                        ucInstallationLog1.AddLogEntry("Could not start service " + ServiceName, eErrorType.error);
                    }
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    started = true;
                    ucInstallationLog1.AddLogEntry(ServiceName + " is already running.", eErrorType.message);
                }
            }
            catch (InvalidOperationException)
            {
                started = true;
                ucInstallationLog1.AddLogEntry(ServiceName + " not found.", eErrorType.error);
            }

            return started;
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        public bool isServiceRunning(string ServiceName)
        {
            if (ServiceName == "ROWebHost")
            {
                return false;
            }

            ServiceController sc = new ServiceController(ServiceName);

            return (sc.Status == ServiceControllerStatus.Running);

        }
        // End TT#74 MD

        public bool StopService(string ServiceName, bool newInstall)
        {
            bool stopped = false;
            try
            {
                SetStatusMessage("Stopping service " + ServiceName);
                // Check whether the service is running.

                ServiceController sc = new ServiceController(ServiceName);

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    // Stop the service if the current status is running.
                    try
                    {
                        // Start the service, and wait until its status is "Running".
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 10, 0));
                        stopped = true;
                        Thread.Sleep(5000);

                        // Display the current service status.
                        ucInstallationLog1.AddLogEntry(ServiceName + " successfully stopped.", eErrorType.message);
                    }
                    catch (System.ServiceProcess.TimeoutException)
                    {
                        ucInstallationLog1.AddLogEntry("Service " + ServiceName + " did not stop in a timely manner. Check Event Viewer", eErrorType.error);
                    }
                    catch (InvalidOperationException)
                    {
                        ucInstallationLog1.AddLogEntry("Could not stop service " + ServiceName, eErrorType.error);
                    }
                }
                else if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    stopped = true;
                    ucInstallationLog1.AddLogEntry(ServiceName + " is already stopped.", eErrorType.message);
                }
            }
            catch (InvalidOperationException)
            {
                stopped = true;
                if (!newInstall)
                {
                    ucInstallationLog1.AddLogEntry(ServiceName + " not found.", eErrorType.error);
                }
            }

            return stopped;
        }

        // Begin TT#484-MD - JSmith - Upgrade of Services Overrides Start Type
        public bool IsServiceAutomaticStart(String ServiceName)
        {
            try
            {
                bool isAutomaticStart = true;
                ServiceController sc = new ServiceController(ServiceName);
                try
                {
                    string path = "Win32_Service.Name='" + ServiceName + "'";
                    ManagementPath p = new ManagementPath(path);
                    //construct the management object
                    ManagementObject ManagementObj = new ManagementObject(p);
                    if (ManagementObj["StartMode"].ToString() == "Manual")
                    {
                        isAutomaticStart = false;
                    }
                }
                catch (System.Exception e)
                {
                }

                return isAutomaticStart;
            }
            catch
            {
                return true;
            }
        }
        // End TT#484-MD - JSmith - Upgrade of Services Overrides Start Type

        public bool FileInUse(string folder)
        {
            bool keepTrying = true;
            bool fileInUse = false;
            int attempt = 0;
            int max = 4;

            if (!Directory.Exists(folder))
            {
                return false;
            }

            string currentFileName = string.Empty;  // TT#1822-MD - JSmith - Installer not detecting incomplete install
            while (keepTrying)
            {
                fileInUse = false;
                string[] files = Directory.GetFiles(folder);
                foreach (string fileName in files)
                {
                    currentFileName = fileName;   // TT#1822-MD - JSmith - Installer not detecting incomplete install
                    if (IsFileLocked(fileName))
                    {
                        ucInstallationLog1.AddLogEntry("File in use " + fileName, eErrorType.warning);
                        fileInUse = true;
                        System.Threading.Thread.Sleep(5000);
                        break;
                    }
                }

                ++attempt;
                // Begin TT#5341 - JSmith - Installer incorrectly identifying file in use
                //if (!fileInUse ||
                //    attempt > max)
                //{
                //    ucInstallationLog1.AddLogEntry("File in use " + currentFileName + ". Upgrade will terminate.", eErrorType.error);   // TT#1822-MD - JSmith - Installer not detecting incomplete install
                //    keepTrying = false;
                //}
                if (fileInUse &&
                    attempt > max)
                {
                    ucInstallationLog1.AddLogEntry("File in use " + currentFileName + ". Upgrade will terminate.", eErrorType.error);   // TT#1822-MD - JSmith - Installer not detecting incomplete install
                    keepTrying = false;
                }
                else if (!fileInUse)
                {
                    keepTrying = false;
                }
                // End TT#5341 - JSmith - Installer incorrectly identifying file in use
            }
            return fileInUse;
        }

        public bool IsFileLocked(string fileName)
        {
            FileStream stream = null;
            FileInfo file;

            try
            {
                if (!File.Exists(fileName))
                {
                    return false;
                }

                file = new FileInfo(fileName); 
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is: 
                //still being written to 
                //or being processed by another thread 
                //or does not exist (has already been processed) 
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked 
            return false;
        } 

        private void ConfigEditInit(object sender, EventArgs e)
        {
            //btnBack.Enabled = false;
        }

        public void RebuildComponentLists()
        {
            dtWindowsInstalled = GetRegisteredComponents();
            //fill the MID install components array
            bool blNeedsScanned = false;
            htMIDInstalled = GetMIDInstalledComponenets(ref blNeedsScanned);
            // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
            //htMIDRegistered = GetMIDRegisteredComponents();
            htMIDRegistered = GetMIDRegisteredComponents(ref blNeedsScanned);
            // End TT#68 MD
        }

        private DataTable GetRegisteredComponents()
        {
            //return variable
            DataTable dtResults = new DataTable();

            //create colunms and add them to the datatable
            DataColumn col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "UninstallKey";
            dtResults.Columns.Add(col);

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "InstallerProductKey";
            dtResults.Columns.Add(col);

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "Location";
            dtResults.Columns.Add(col);

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "EntryType";
            dtResults.Columns.Add(col);

            //read the needed values from the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            string[] subkeynames = mid_key.GetSubKeyNames();

            foreach (string subkeyname in subkeynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(subkeyname);
                if((string) sub_key.GetValue("InstallType") == "Windows")
                {
                    DataRow row = dtResults.NewRow();
                    row["UninstallKey"] = sub_key.GetValue("WindowsInstallKey").ToString().Trim();
                    row["Location"] = sub_key.GetValue("Location").ToString().Trim();
                    row["EntryType"] = sub_key.GetValue("EntryType").ToString().Trim();
                    dtResults.Rows.Add(row);
                }
            }

            //return value
            return dtResults;
        }

        private Hashtable GetMIDInstalledComponenets(ref bool blNeedsScanned)
        {
            //return variable
            Hashtable MIDInstalls = new Hashtable();
            string entryType;

            //read the needed values from the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            string[] subkeynames = mid_key.GetSubKeyNames();

            foreach (string subkeyname in subkeynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(subkeyname);
                if ((string)sub_key.GetValue("InstallType") == "MIDRetail" &&
                    (string)sub_key.GetValue("InstallType") != "Manual")
                {
                    entryType = sub_key.GetValue("EntryType").ToString();
                    if (entryType == eEntryType.MIDAPI.ToString())
                    {
                        if (!Directory.Exists(sub_key.GetValue("Location").ToString()))
                        {
                            blNeedsScanned = true;
                        }
                    }
                    else if (!File.Exists(sub_key.GetValue("Location").ToString()))
                    {
                        string value = sub_key.GetValue("Location").ToString();
                        blNeedsScanned = true;
                    }
                    if (!blNeedsScanned)
                    {
                        if (sub_key.GetValue("Location").ToString().EndsWith(".config") == false)
                        {
                            // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
                            //MIDInstalls.Add(sub_key.GetValue("Location").ToString().Trim(), null);
                            if (MIDInstalls.ContainsKey(sub_key.GetValue("Location").ToString().Trim()))
                            {
                                 blNeedsScanned = true;
                            }
                            else
                            {
                                MIDInstalls.Add(sub_key.GetValue("Location").ToString().Trim(), null);
                            }
                            // End TT#68 MD
                        }
                        else
                        {
                            // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
                            //MIDInstalls.Add(sub_key.GetValue("Location").ToString().Trim(),
                            //       sub_key.GetValue("Client").ToString().Trim());
                            if (MIDInstalls.ContainsKey(sub_key.GetValue("Location").ToString().Trim()))
                            {
                                 blNeedsScanned = true;
                            }
                            else
                            {
                                MIDInstalls.Add(sub_key.GetValue("Location").ToString().Trim(),
                                    sub_key.GetValue("Client").ToString().Trim());
                            }
                            // End TT#68 MD
                        }
                    }
                }
            }

            //return value
            return MIDInstalls;
        }

        // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
        //private Hashtable GetMIDRegisteredComponents()
        private Hashtable GetMIDRegisteredComponents(ref bool blNeedsScanned)
        // End TT#68 MD
        {
            //return variable
            Hashtable MIDRegistered = new Hashtable();
            try
            {

                //read the needed values from the registry
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
                string[] subkeynames = mid_key.GetSubKeyNames();

                foreach (string subkeyname in subkeynames)
                {
                    RegistryKey sub_key = mid_key.OpenSubKey(subkeyname);
                    if (sub_key.GetValue("Location").ToString().EndsWith(".config") == false ||
                        (string)sub_key.GetValue("InstallType") != "MIDRetail")
                    {
                        // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
                        //MIDRegistered.Add(sub_key.GetValue("Location").ToString().Trim(), sub_key);
                        if (MIDRegistered.ContainsKey(sub_key.GetValue("Location").ToString().Trim()))
                        {
                            blNeedsScanned = true;
                        }
                        else
                        {
                            MIDRegistered.Add(sub_key.GetValue("Location").ToString().Trim(), sub_key);
                        }
                        // End TT#68 MD
                    }
                    else
                    {
                        // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
                        //MIDRegistered.Add(sub_key.GetValue("Location").ToString().Trim(), sub_key);
                        if (MIDRegistered.ContainsKey(sub_key.GetValue("Location").ToString().Trim()))
                        {
                            blNeedsScanned = true;
                        }
                        else
                        {
                            MIDRegistered.Add(sub_key.GetValue("Location").ToString().Trim(), sub_key);
                        }
                        // End TT#68 MD
                    }
                }
            }
            catch (Exception err)
            {
                ErrorHandler(err.Message);
            }

            // Begin TT#1668 - JSmith - Install Log
            string component;
            RegistryKey regKey;
            string entryType;
            foreach (DictionaryEntry de in MIDRegistered)
            {
                component = (string)de.Key;
                regKey = (RegistryKey)de.Value;
                entryType = regKey.GetValue("EntryType").ToString();
                DirectoryInfo di = new DirectoryInfo(component);
                ArrayList parents = new ArrayList();
                while (di.Root.FullName != di.FullName)
                {
                    parents.Add(di.FullName);
                    di = di.Parent;
                }
                if (_logLocation == null ||
                    component.Contains(InstallerConstants.cControlServiceExecutableName) == true)
                {
                    _logLocation = (string)parents[parents.Count - 1];
                }
                if (component.Contains(InstallerConstants.cClientApp) == true)
                {
                    blClientComponentsInstalled = true;
                }
                else if (component.Contains(InstallerConstants.cBatchName) == true)
                {
                    blBatchComponentsInstalled = true;
                }
                else
                {
                    blServerComponentsInstalled = true;
                }
            }
			// End TT#1668
            //return value
            return MIDRegistered;

        }

        private bool RegistryNeedsConverted()
        {
            bool RegistryNeedsConverted = false;

            if (is64Bit())
            {
                RegistryKey reg_key = Registry.LocalMachine;
                RegistryKey soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, false);
                RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey, true);

                if (mid_key == null)
                {
                    RegistryNeedsConverted = true;
                }
            }

            return RegistryNeedsConverted;
        }

        private void ConvertRegistryEntries()
        {
            FileInfo fi;

            string fileName = Path.GetTempPath() + "RegEntries.txt";

            if (ConvertRegistryProcessExtract(fileName, false))
            {
                fi = new FileInfo(fileName);
                if (fi.Length > 0)
                {
                    if (ConvertRegistryProcessBuild(fileName))
                    {
                        // call extract again, but delete old entries
                        ConvertRegistryProcessExtract(fileName, true);
                    }
                }
            }
        }

        private bool ConvertRegistryProcessExtract(string aFileName, bool blDeleteOldRegistryEntries)
        {
            int exitCode;

            Process extractProcess = new Process();

#if (DEBUG)
            extractProcess.StartInfo.FileName = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\MIDRegExtract\bin\Debug\" + "MIDRegExtract.exe";
#else
            extractProcess.StartInfo.FileName = Application.StartupPath + @"\" + "MIDRegExtract.exe";
#endif

            extractProcess.StartInfo.Arguments = @"""" + InstallerConstants.cRegistryRootKey + @""" """ + aFileName + @""" """ + blDeleteOldRegistryEntries.ToString() + @"""";

            extractProcess.StartInfo.WorkingDirectory = Application.StartupPath;

            // Begin TT#1668 - JSmith - Install Log
            string msg = GetText("ConvertRegistry");
            SetLogMessage(msg, eErrorType.message);
            msg = GetText("ConvertRegistryFileName");
            msg = msg.Replace("{0}", extractProcess.StartInfo.FileName);
            SetLogMessage(msg, eErrorType.message);
            msg = GetText("ConvertRegistryArguments");
            msg = msg.Replace("{0}", extractProcess.StartInfo.Arguments);
            SetLogMessage(msg, eErrorType.message);
            msg = GetText("ConvertRegistryWorkingDirectory");
            msg = msg.Replace("{0}", extractProcess.StartInfo.WorkingDirectory);
            SetLogMessage(msg, eErrorType.message);
			// End TT#1668

            extractProcess.Start();
            extractProcess.WaitForExit();
            exitCode = extractProcess.ExitCode;
            extractProcess.Close();
            return (exitCode==0);
        }

        private bool ConvertRegistryProcessBuild(string aFileName)
        {
            bool successful = true;
            string currentKey = string.Empty;
            RegistryKey sub_key = null;

            RegistryKey reg_key = Registry.LocalMachine;
            RegistryKey soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey, true);

            if (mid_key != null)
            {
                return false;
            }

            mid_key = soft_key.CreateSubKey(InstallerConstants.cRegistryRootKey);

            StreamReader reader = null;
            string line;

            try
            {
                reader = new StreamReader(aFileName);  //opens the file

                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split('|');
                    if (fields.Length != 3)
                    {
                        return false;
                    }
                    if (fields[0] != currentKey)
                    {
                        currentKey = fields[0];
                        sub_key = mid_key.CreateSubKey(currentKey);
                    }
                    if (sub_key != null)
                    {
                        sub_key.SetValue(fields[1], fields[2]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return successful;
        }
       
        private bool CheckPreviousInstalls()
        {
            bool blReturn = true;

            try
            {
                //get the list of software keys on the local machine
                RegistryKey reg_key = Registry.LocalMachine;
                RegistryKey soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, false);

                //key must be add if no previous installs
                if (ValueNameExists(soft_key.GetSubKeyNames(), InstallerConstants.cRegistryRootKey) == false)
                {
                    blReturn = false;
                }
            }
            catch (Exception ex)
            {
                bool stop = true;
            }

            //return value
            return blReturn;
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

        private void RemoveRegisteredComponents()
        {
		    // Begin TT#1668 - JSmith - Install Log
            string msg;

            SetLogMessage(GetText("RegistryRemove"), eErrorType.message);
			// End TT#1668

            //drill down to the install registry entries
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey,true);

            //if the install registy entries exist
            if (mid_key != null)
            {
                //if the install registry has subkeys
                if (mid_key.SubKeyCount > 0)
                {
                    //get a list of sub keys
                    string[] subKeyNames = mid_key.GetSubKeyNames();

                    //delete sub keys
                    foreach (string subKeyName in subKeyNames)
                    {
					    // Begin TT#1668 - JSmith - Install Log
                        msg = GetText("RegistryKey");
                        msg = msg.Replace("{0}", subKeyName);
                        SetLogMessage(msg, eErrorType.message);
						// End TT#1668

                        mid_key.DeleteSubKey(subKeyName);
                    }

                    //delete the mid install key
					// Begin TT#1668 - JSmith - Install Log
                    msg = GetText("RegistryKey");
                    msg = msg.Replace("{0}", InstallerConstants.cRegistryRootKey);
                    SetLogMessage(msg, eErrorType.message);
					// End TT#1668

                    soft_key.DeleteSubKey(InstallerConstants.cRegistryRootKey);
                }
            }
        }

        private void Scan_Next()
        {
            RegistryKey soft_key;
            //register the list of items
            ucScan scan = (ucScan)ucControl;

            // Begin TT#885 - JSmith - Duplicate key on rescan.
            //add the installation flag to the registry
            RegistryKey reg_key = Registry.LocalMachine;
            soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey install_key = reg_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);

            if (install_key != null)
            {
                soft_key.DeleteSubKeyTree(InstallerConstants.cRegistryRootKey);
            }

            install_key = soft_key.CreateSubKey(InstallerConstants.cRegistryRootKey);

            install_key.SetValue("Installs", 1);

            if (scan.HasNodes == true)
            {
                intClient = 0;
                intContServ = 0;
                intStoreServ = 0;
                intMerchServ = 0;
                intSchedServ = 0;
                intAppServ = 0;
                intConfig = 0;
                intAPI = 0;
                RegisterItems(scan.ItemsToRegister);
            }
            // End TT#885

            dtWindowsInstalled = GetRegisteredComponents();

            //fill the MID install components array
            bool blNeedsScanned = false;
            htMIDInstalled = GetMIDInstalledComponenets(ref blNeedsScanned);
            // Begin TT#68 MD - JSmith - Install Log Viewer Pop Up on Install
            //htMIDRegistered = GetMIDRegisteredComponents();
            if (!blNeedsScanned)
            {
                htMIDRegistered = GetMIDRegisteredComponents(ref blNeedsScanned);
            }
            // End TT#68 MD
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //reset the frame status objects
            lblStatus.Text = "";
            ProgressBarSetValue(0);
            List<string> lstFilesToConfig;

            //get the loaded control name
            string loaded_control = pnFrame.Controls[0].Name;
            ucInstallationLog1.AddLogEntry("User clicked next on the " + pnFrame.Controls[0].Name + " panel.",eErrorType.message);

            switch (loaded_control)
            {
                case "ucScan":

                    //scan on next button click
                    ucInstallationLog1.AddLogEntry("Register the components found if scanned", eErrorType.message);
                    Scan_Next();

                    //unload the showing control
                    ProcessControl.ScanPane = (ucScan) ucControl;
                    UnloadControl();

                    //load the next pane
                    ucInstallChooser install_chooser = new ucInstallChooser(this);
                    //add event handlers
                    install_chooser.NotConfigureNext += new EventHandler(this.NotConfigureNext);
                    install_chooser.ConfigureNext += new EventHandler(this.ConfigureNext);

                    ucControl = install_chooser;
                    lblTitle.Text = "Choose the Installation you want to perform";
                    LoadUserControl();
                    ucInstallationLog1.AddLogEntry("Installation Chooser panel loaded", eErrorType.message);

                    break;

                case "ucInstallChooser":
                    
                    //set control
                    install_chooser = (ucInstallChooser)ucControl;

                    //install the client files 
                    if (install_chooser.Client)
                    {
                        //log message
                        ucInstallationLog1.AddLogEntry("User has chosen to install or maintain client components", eErrorType.message);

                        //set workflow type
                        ProcessControl.Workflow = eWorkflowType.client;
                        
                        //unload the previous control
                        ProcessControl.InstallChooserPane = (ucInstallChooser)ucControl;
                        UnloadControl();

                        //set up object
                        ucClient client;
                        if (ProcessControl.ClientPane == null)
                        {
                            client = new ucClient(this, ucInstallationLog1);
                        }
                        else
                        {
                            client = (ucClient)ProcessControl.ClientPane;
                        }

                        //add event handlers
                        client.NotConfigureNext += new EventHandler(this.NotConfigureNext);
                        client.ConfigureNext += new EventHandler(this.ConfigureNext);
                        
                        //new title
                        lblTitle.Text = "Choose the Logility-RO client application task(s) to perform";
                        ucControl = client;
                        LoadUserControl();

                        //show finish button and hide next button
                        btnBack.Enabled = true;
                        btnNext.Visible = false;
                        btnSave.Visible = false;
                        btnProcess.Visible = true;

                        break;
                    }

                    //install the service files
                    if (install_chooser.Server)
                    {
                        //log message
                        ucInstallationLog1.AddLogEntry("User has chosen to install or maintain server components", eErrorType.message);

                        //set workflow type
                        ProcessControl.Workflow = eWorkflowType.server;

                        //unload the previous control
                        ProcessControl.InstallChooserPane = (ucInstallChooser)ucControl;
                        UnloadControl();

                        //new object
                        ucServer server;
                        if (ProcessControl.ServerPane == null)
                        {
                            server = new ucServer(this, ucInstallationLog1, sInstallerLocation);
                        }
                        else
                        {
                            server = (ucServer)ProcessControl.ServerPane;
                        }

                        //event handlers
                        server.ConfigureNext += new EventHandler(this.ConfigureNext);
                        server.NotConfigureNext += new EventHandler(this.NotConfigureNext);

                        //new title
                        lblTitle.Text = "Choose the Logility-RO server application task(s) to perform";

                        //load control
                        ucControl = server;
                        LoadUserControl();

                        //show process button and hide next button
                        btnBack.Enabled = true;
                        btnNext.Visible = false;
                        btnSave.Visible = false;
                        btnProcess.Visible = true;

                        break;
                    }

                    //configure all files
                    if (install_chooser.Configure)
                    {
                        //log message
                        ucInstallationLog1.AddLogEntry("User has chosen to configure components", eErrorType.message);

                        //set workflow type
                        ProcessControl.Workflow = eWorkflowType.configure;

                        //unload the previous control
                        ProcessControl.InstallChooserPane = (ucInstallChooser)ucControl;
                        UnloadControl();

                        lstFilesToConfig = new List<string>();
                        string installed_component = null;
                        eConfigType configType = eConfigType.Server;
                        string component;
                        RegistryKey regKey;
                        string entryType;

                        foreach (DictionaryEntry MIDRegistered in htMIDRegistered)
                        {
                            component = (string)MIDRegistered.Key;
                            regKey = (RegistryKey)MIDRegistered.Value;
                            entryType = regKey.GetValue("EntryType").ToString();
                            if (installed_component == null)
                            {
                                installed_component = component;
                                if (entryType == eEntryType.MIDClient.ToString())
                                {
                                    configType = eConfigType.Client;
                                }
                                else if (entryType == eEntryType.MIDConfig.ToString())
                                {
                                    configType = eConfigType.Config;
                                }
                            }
                            if (component.EndsWith(".exe") ||
                                component.EndsWith(".config"))
                            {
                                lstFilesToConfig.Add(Directory.GetParent(component).ToString().Trim() + @"\");
                            }
                            else
                            {
                                lstFilesToConfig.Add(component.Trim() + @"\");
                                // Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
                                if (component.Trim().EndsWith("Batch"))
                                {
                                    if (Directory.Exists(component.Trim() + @"\HierarchyLevelMaintenance"))
                                    {
                                        lstFilesToConfig.Add(component.Trim() + @"\HierarchyLevelMaintenance\");
                                    }
                                }
                                // End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
                            }
                        }

                        //new object
                        Cursor = Cursors.WaitCursor;
                        ucConfig config1 = new ucConfig(this, installed_component, configType, lstFilesToConfig, ucInstallationLog1);
                        config1.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                        //new title
                        lblTitle.Text = "Configure the client application settings";

                        //load control
                        ucControl = config1;
                        LoadUserControl();
                        Cursor = Cursors.Default;

                        //show finish button and hide next button
                        btnBack.Enabled = true;
                        btnNext.Visible = false;
                        btnProcess.Visible = false;
                        btnSave.Visible = true;

                        break;
                    }

                    //install the database files
                    if (install_chooser.Utilities)
                    {
                        //log message
                        ucInstallationLog1.AddLogEntry("User has chosen to access utilities", eErrorType.message);

                        //set workflow type
                        ProcessControl.Workflow = eWorkflowType.utilities;

                        //unload the previous control
                        ProcessControl.InstallChooserPane = (ucInstallChooser)ucControl;
                        UnloadControl();

                        //set up object
                        ucUtilities utilities;
                        //if (ProcessControl.UtilitiesPane == null)
                        //{
                            utilities = new ucUtilities(this, ucInstallationLog1, false, true);
                        //}
                        //else
                        //{
                        //    utilities = (ucUtilities)ProcessControl.UtilitiesPane;
                        //}

                        //add event handlers
                        utilities.NotConfigureNext += new EventHandler(this.NotConfigureNext);
                        utilities.ConfigureNext += new EventHandler(this.ConfigureNext);

                        //new title
                        lblTitle.Text = "Choose the Logility-RO application task(s) to perform";
                        ucControl = utilities;
                        LoadUserControl();

                        //show process button and hide next button
                        btnBack.Enabled = true;
                        if (utilities.DatabaseMaintenance ||
                            utilities.Rescan)
                        {
                            btnNext.Visible = true;
                            btnProcess.Visible = false;
                        }
                        else
                        {
                            btnNext.Visible = false;
                            btnProcess.Visible = true;
                        }
                        btnSave.Visible = false;

                        break;
                    }

                    break;

                case "ucClient":

                    switch(InstallTask)
                    {
                        case eInstallTasks.configure:

                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to configure client components", eErrorType.message);

                            //unload the previous control
                            ProcessControl.ClientPane = (ucClient)ucControl;
                            UnloadControl();
                            
                            //add application to a list needed by the config control
                            List<string> lstFilesToConfig1 = new List<string>();
                            //lstFilesToConfig1.Add(Directory.GetParent(Directory.GetParent(UninstallFile).ToString().Trim()).ToString().Trim() + @"\");
                            lstFilesToConfig1.Add(Directory.GetParent(UninstallFile).ToString().Trim() + @"\");

                            //new object
                            ucConfig config1 = new ucConfig(this, UninstallFile, eConfigType.Client, lstFilesToConfig1, ucInstallationLog1);
                            config1.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                            //new title
                            lblTitle.Text = "Configure the client application settings";

                            //load control
                            ucControl = config1;
                            LoadUserControl();

                            //show finish button and hide next button
                            btnBack.Enabled = true;
                            btnNext.Visible = false;
                            btnProcess.Visible = false;
                            btnSave.Visible = true;

                            break;

                    }

                    break;

                case "ucServer":

                    switch(InstallTask)
                    {
                        case eInstallTasks.configure:

                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to configure server components", eErrorType.message);
                            
                            //identify the current control
                            ProcessControl.ServerPane = (ucServer)ucControl;
                           
                            //get the list of selected controls
                            List<string> lstFilesToConfig3 = new List<string>();
                            foreach (string selectedItem in ProcessControl.ServerPane.lstInstalledServices.SelectedItems)
                            {
                                if (selectedItem.ToLower().Contains(".exe") ||
                                    selectedItem.ToLower().Contains(".config"))
                                {
                                    lstFilesToConfig3.Add(Directory.GetParent(selectedItem).ToString().Trim() + @"\");
                                }
                                else
                                {
                                    lstFilesToConfig3.Add(selectedItem.Trim() + @"\");
                                }
                            }

                            //unload the previous control
                            UnloadControl();

                            //new object
                            ucConfig config = new ucConfig(this, UninstallFile, eConfigType.Server, lstFilesToConfig3, ucInstallationLog1);
                            config.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                            //new title
                            lblTitle.Text = "Configure the server application settings";

                            //load control
                            ucControl = config;
                            LoadUserControl();

                            //show finish button and hide next button
                            btnBack.Enabled = true;
                            btnNext.Visible = false;
                            btnProcess.Visible = false;
                            btnSave.Visible = true;

                            break;

                        case eInstallTasks.install:

                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to install server components", eErrorType.message);

                            //set control 
                            ProcessControl.ServerPane = (ucServer)ucControl;

                            //hourglass
                            this.Cursor = Cursors.WaitCursor;

                            //perform install
                            List<string> lstResult = ProcessControl.ServerPane.Install(null);

                            //hourglass
                            this.Cursor = Cursors.Default;

                            if (lstResult.Count > 0)
                            {
                                //get the list of selected controls
                                List<string> lstFilesToConfig4 = new List<string>();

                                //if (ProcessControl.ServerPane.blClient == true)
                                //{
                                //    if (ProcessControl.ServerPane.blAutoUpgrade == false)
                                //    {
                                //        lstFilesToConfig4.Add(ProcessControl.ServerPane.txtInstallFolder.Text + @"\MIDRetail_Client\");
                                //    }
                                //    else
                                //    {
                                //        lstFilesToConfig4.Add(ProcessControl.ServerPane.txtInstallFolder.Text + @"\MIDRetail_Client (Auto-Upgrade)\");
                                //    }
                                //}

                                //if(ProcessControl.ServerPane.blClient64 == true)
                                //{
                                //    if (ProcessControl.ServerPane.blAutoUpgrade64 == false)
                                //    {
                                //        lstFilesToConfig4.Add(ProcessControl.ServerPane.txtInstallFolder.Text + @"\MIDRetail_Client64\");
                                //    }
                                //    else
                                //    {
                                //        lstFilesToConfig4.Add(ProcessControl.ServerPane.txtInstallFolder.Text + @"\MIDRetail_Client64 (Auto-Upgrade)\");
                                //    }
                                //}

                                foreach (string Item in lstResult)
                                {
                                    lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                }

                                //unload the previous control
                                UnloadControl();

                                //new object
                                ucConfig config_svr = new ucConfig(this, lstFilesToConfig4[0].ToString().Trim(),
                                    eConfigType.Server, lstFilesToConfig4, ucInstallationLog1);
                                config_svr.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                //new title
                                lblTitle.Text = "Configure the server application settings";

                                //load control
                                ucControl = config_svr;
                                LoadUserControl();


                                //log message
                                ucInstallationLog1.AddLogEntry("Configuring the installed server components", eErrorType.message);


                                //show finish button and hide next button
                                btnBack.Enabled = true;
                                btnNext.Visible = false;
                                btnProcess.Visible = false;
                                btnSave.Visible = true;
                            }

                            break;

                        case eInstallTasks.upgrade:

                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to upgrade server components", eErrorType.message);

                            //set control 
                            ProcessControl.ServerPane = (ucServer)ucControl;

                            //hourglass
                            this.Cursor = Cursors.WaitCursor;

                            //perform install
                            bool blResult1 = ProcessControl.ServerPane.Upgrade();

                            //hourglass
                            this.Cursor = Cursors.Default;

                            if (blResult1 == true)
                            {
                                //get the list of selected controls
                                List<string> lstFilesToConfig5 = new List<string>();
                                foreach (string selectedItem in ProcessControl.ServerPane.lstInstalledServices.SelectedItems)
                                {
                                    lstFilesToConfig5.Add(Directory.GetParent(selectedItem).ToString().Trim() + @"\");
                                }
                                
                                //unload the previous control
                                UnloadControl();

                                //new object
                                ucConfig config1 = new ucConfig(this, UninstallFile, eConfigType.Server, lstFilesToConfig5, ucInstallationLog1);
                                config1.EditModeInitiated += new EventHandler(this.ConfigEditInit);


                                //new title
                                lblTitle.Text = "Configure the server application(s) settings";

                                //load control
                                ucControl = config1;
                                LoadUserControl();

                                //log message
                                ucInstallationLog1.AddLogEntry("Configuring the upgraded server components", eErrorType.message);

                                //show finish button and hide next button
                                btnBack.Enabled = true;
                                btnNext.Visible = false;
                                btnProcess.Visible = false;
                                btnSave.Visible = true;
                            }

                            break;
                    }

                    break;

                case "ucUtilities":

                    switch (InstallTask)
                    {
                        case eInstallTasks.databaseMaintenance:
                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to perform database maintenance", eErrorType.message);

                            //set control 
                            ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                            //perform database maintenance
                            bool blResult1 = ProcessControl.UtilitiesPane.DoDatabaseMaintenance();

                            break;

                        case eInstallTasks.startServices:
                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to start the services", eErrorType.message);

                            //set control 
                            ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                            break;

                        case eInstallTasks.stopServices:
                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to stop the services", eErrorType.message);

                            //set control 
                            ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                            break;

                        case eInstallTasks.eventSources:
                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to add the add Windows Event Sources", eErrorType.message);

                            //set control 
                            ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                            break;

                        //case eInstallTasks.crystalReports:
                        //    //log message
                        //    ucInstallationLog1.AddLogEntry("User has chosen to install Crystal Reports", eErrorType.message);

                        //    //set control 
                        //    ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                        //    break;

                        case eInstallTasks.scan:

                            //log message
                            ucInstallationLog1.AddLogEntry("User has chosen to rescan the computer for installed components", eErrorType.message);

                            //set workflow type
                            ProcessControl.Workflow = eWorkflowType.rescan;

                            //unload the previous control
                            ProcessControl.UtilitiesPane = (ucUtilities)ucControl;
                            UnloadControl();

                            //new title
                            lblTitle.Text = "Rescan your computer for previously installed components";

                            //load the new control
                            ucScan rescan;
                            if (ProcessControl.UtilitiesPane == null)
                            {
                                rescan = new ucScan(ucInstallationLog1, this);
                            }
                            else
                            {
                                if (ProcessControl.ScanPane == null)
                                {
                                    rescan = new ucScan(ucInstallationLog1, this);
                                }
                                else
                                {
                                    rescan = (ucScan)ProcessControl.ScanPane;
                                }
                            }

                            rescan.txtInstruction.Text = "You can scan your local drives for previously " +
                                "installed Logility RO components and register them. After the scan, the " +
                                "components that are left checked will be register on the 'Finish' click. " +
                                "If you don't want to scan and register the previously installed components, " +
                                "just click the 'Finish' button.";
                            ucControl = rescan;
                            LoadUserControl();

                            //show finish button and hide next button
                            btnBack.Enabled = true;
                            btnNext.Visible = true;
                            btnProcess.Visible = false;
                            btnSave.Visible = false;

                            break;
                    }
                    break;
            }
        }

        //config event on the server pane
        private void ConfigureNext(object sender, EventArgs e)
        {
            btnProcess.Visible = false;
            btnNext.Visible = true;
            btnSave.Visible = false;
			// Begin TT#1668 - JSmith - Install Log
            if (!blPerformingOneClickUpgrade &&
                !blPerformingTypicalInstall)
            {
                ProgressBarSetValue(0);
            }
			// End TT#1668
        }

        //config event on the server pane
        private void NotConfigureNext(object sender, EventArgs e)
        {
            btnProcess.Visible = true;
            btnNext.Visible = false;
            btnSave.Visible = false;
			// Begin TT#1668 - JSmith - Install Log
            if (!blPerformingOneClickUpgrade &&
                !blPerformingTypicalInstall)
            {
                ProgressBarSetValue(0);
            }
			// End TT#1668
        }

        //allow the user controls to enable and disable the next button as needed
        public bool Next_Enabled
        {
            get
            {
                return btnNext.Enabled;
            }
            set
            {
                btnNext.Enabled = value;
            }
        }

        //allow the user controls to enable and disable the back button as needed
        public bool Back_Enabled
        {
            get
            {
                return btnBack.Enabled;
            }
            set
            {
                btnBack.Enabled = value;
            }
        }

        private void LoadUserControl()
        {

            //place control
            ucControl.Top = 0;
            ucControl.Left = 0;
            //ucControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            //            | System.Windows.Forms.AnchorStyles.Left)
            //            | System.Windows.Forms.AnchorStyles.Right)));

            ucControl.Dock = DockStyle.Fill;

            // Begin TT#195 MD - JSmith - Add environment authentication
            EnterpriseLabelSetVisible(false);
            // Begin TT#195 MD

            //load control
            pnFrame.Controls.Add(ucControl);

            if (ucControl is ucClient)
            {
                help_ID = "client";
            }
            else if (ucControl is ucConfig)
            {
                help_ID = "configure";
            }
            else if (ucControl is ucInstallationLog)
            {
                help_ID = "log";
            }
            else if (ucControl is ucInstallChooser)
            {
                help_ID = "main";
            }
            else if (ucControl is ucScan)
            {
                help_ID = "scan";
            }
            else if (ucControl is ucServer)
            {
                help_ID = "server";
                // Begin TT#195 MD - JSmith - Add environment authentication
                if (!isServerEnterprise)
                {
                    EnterpriseLabelSetVisible(true);
                }
                // Begin TT#195 MD
            }
            else if (ucControl is ucUtilities)
            {
                help_ID = "utilities";
            }
        }

        private void UnloadControl()
        {
            //remove the loaded user controls
            ucControl.Tag = this.lblTitle.Text;
            pnFrame.Controls.Clear();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            //show the install error/warning/message detail section
            if (btnDetail.Text == "&Show Details >>")
            {
                //this.Height = 615;
                pnFrame.Height = cShowHeight;
                btnDetail.Text = "<< &Hide Details";
                ucInstallationLog1.Visible = true;
            }

            //hide the install error/warning/message detail section
            else
            {
                //this.Height = 555;
                pnFrame.Height = cHideHeight;
                btnDetail.Text = "&Show Details >>";
                ucInstallationLog1.Visible = false;
            }
        }

        private void RegisterItems(List<string> lstItemsToRegister)
        {

            //loop thru the installed components and register them in the windows registry
            foreach (string file in lstItemsToRegister)
            {
                string name = Path.GetFileName(file) + ";";
                //string app_name = "";

                if (ConfigurationManager.AppSettings["ClientNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDClient, intClient);
                    intClient++;
                }

                if (ConfigurationManager.AppSettings["ControlServiceNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDControlService, intContServ);
                    intContServ++;
                }

                if (ConfigurationManager.AppSettings["StoreServiceNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDStoreService, intStoreServ);
                    intStoreServ++;
                }

                if (ConfigurationManager.AppSettings["MerchandiseServiceNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDHierarchyService, intMerchServ);
                    intMerchServ++;
                }

                if (ConfigurationManager.AppSettings["SchedulerServiceNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDSchedulerService, intSchedServ);
                    intSchedServ++;
                }

                if (ConfigurationManager.AppSettings["ApplicationServiceNames"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDApplicationService, intAppServ);
                    intAppServ++;
                }

                if (ConfigurationManager.AppSettings["ConfigurationFileName"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDConfig, intConfig);
                    intConfig++;
                }

                if (ConfigurationManager.AppSettings["APIName"].ToString().Contains(name) == true)
                {
                    ucInstallationLog1.AddLogEntry("Registering " + file, eErrorType.message);

                    RegisterItem(file, eEntryType.MIDAPI, intAPI);
                    intAPI++;
                }
            }
        }

        private void RegisterItem(string ItemToRegister, eEntryType EntryType, int Counter)
        {
            //drill into the registry
            RegistryKey reg_key = Registry.LocalMachine;
            RegistryKey soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            RegistryKey mid_key = soft_key.CreateSubKey(InstallerConstants.cRegistryRootKey);
            RegistryKey sub_key = null;
            string itemToRegister = ItemToRegister;

            switch (EntryType)
            {
                case eEntryType.MIDClient:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cClientKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cClientKey);
                    }
                    break;

                case eEntryType.MIDControlService:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cControlServiceKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cControlServiceKey);
                    }
                    break;

                case eEntryType.MIDStoreService:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cStoreServiceKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cStoreServiceKey);
                    }
                    break;

                case eEntryType.MIDHierarchyService:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cHierarchyServiceKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cHierarchyServiceKey);
                    }
                    break;

                case eEntryType.MIDSchedulerService:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cSchedulerServiceKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cSchedulerServiceKey);
                    }
                    break;

                case eEntryType.MIDApplicationService:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cApplicationServiceKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cApplicationServiceKey);
                    }
                    break;

                case eEntryType.MIDAPI:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cBatchKey + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey(InstallerConstants.cBatchKey);
                    }
                    //itemToRegister += @"\" + InstallerConstants.cBatchExecutableName;
                    break;

                case eEntryType.MIDConfig:

                    //add the key
                    if (Counter > 0)
                    {
                        sub_key = mid_key.CreateSubKey("MIDConfig" + Counter.ToString().Trim());
                    }
                    else
                    {
                        sub_key = mid_key.CreateSubKey("MIDConfig");
                    }
                    break;
            }

            //add the entry type value
            sub_key.SetValue("EntryType", EntryType.ToString().Trim());

            //get the rows with the sought location
            DataRow[] drResults = dtWindowsInstalled.Select("Location = '" + itemToRegister + "'");
            bool blMIDResults = htMIDInstalled.Contains("C" + ItemToRegister.Substring(1));
            
            //get the registry application link for the config file            
            bool blConfig = false;
            string htValue = "";

            if (htMIDInstalled.Count > 0)
            {
                if (ItemToRegister.EndsWith(".config") == true)
                {
                    if (htMIDInstalled.ContainsKey("C" + ItemToRegister.Substring(1)))
                    {
                        htValue = htMIDInstalled["C" + ItemToRegister.Substring(1)].ToString();
                    }
                    blConfig = true;
                }
            }
            else
            {
                if (ItemToRegister.EndsWith(".config") == true)
                {
                    blConfig = true;
                }
            }

            //add the installation type value
            if (drResults.GetUpperBound(0) == 0)
            {
                sub_key.SetValue("InstallType", "Windows");
                sub_key.SetValue("WindowsInstallKey", drResults[0].Field<string>("UninstallKey").Trim());
            }
            else if (blMIDResults == true)
            {
                sub_key.SetValue("InstallType", "MIDRetail");

                if (blConfig == true)
                {
                    sub_key.SetValue("Client", htValue);
                }
            }
            else
            {
                sub_key.SetValue("InstallType", "Manual");

                if (blConfig == true)
                {
                    //sub_key.SetValue("Client",GetConfigClientRegKeyName(ItemToRegister));
                }
            }

            //add the install location value
            sub_key.SetValue("Location", ItemToRegister);

            // Begin TT#1668 - JSmith - Install Log
            LogRegistryItem(sub_key);
			// End TT#1668
        }

        // Begin TT#1668 - JSmith - Install Log
        public void LogRegistryItem(RegistryKey reg_key)
        {
            string msg;
            RegistryKey sub_key;

            SetLogMessage(GetText("RegistryAdd"), eErrorType.message);
            

            foreach (string valueName in reg_key.GetValueNames())
            {
                msg = GetText("RegistryItem");
                msg = msg.Replace("{0}", valueName);
                msg = msg.Replace("{1}", reg_key.GetValue(valueName).ToString());
                SetLogMessage(msg, eErrorType.message);
            }
        }
		// End TT#1668

        private void btnBack_Click(object sender, EventArgs e)
        {
            string pane_name = pnFrame.Controls[0].Name;

            switch (pane_name)
            {
                case "ucClient":

                    //set the process control object
                    ProcessControl.ClientPane = (ucClient)ucControl;

                    //unload the current control
                    UnloadControl();

                    //log back action
                    ucInstallationLog1.AddLogEntry("Returning to the Install Chooser panel", eErrorType.message);

                    //set the control to load
                    ucControl = (ucInstallChooser)ProcessControl.InstallChooserPane;

                    //load the back control
                    LoadUserControl();

                    // Begin TT#785-MD - JSmith - Installer Bug - Next and Process buttons
                    //disable the process button and enable the next button
                    //btnProcess.Visible = false;
                    //btnNext.Visible = true;
                    if (((ucInstallChooser)ucControl).UpgradeAll)
                    {
                        btnProcess.Visible = true;
                        btnNext.Visible = false;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnProcess.Visible = false;
                        btnNext.Visible = true;
                        btnSave.Visible = false;
                    }
                    // End TT#785-MD - JSmith - Installer Bug - Next and Process buttons

                    ProcessControl.ClientPane = null;

                    //set buttons
                    btnBack.Enabled = false;

                    break;

                case "ucServer":

                    //set the process control object
                    ProcessControl.ServerPane = (ucServer)ucControl;

                    //unload the current control
                    UnloadControl();
                    
                    //log back action
                    ucInstallationLog1.AddLogEntry("Returning to the Install Chooser panel", eErrorType.message);

                    //set the control to load
                    ucControl = (ucInstallChooser)ProcessControl.InstallChooserPane;

                    //load the back control
                    LoadUserControl();

                    // Begin TT#785-MD - JSmith - Installer Bug - Next and Process buttons
                    //disable the process button and enable the next button
                    //btnProcess.Visible = false;
                    //btnNext.Visible = true;
                    if (((ucInstallChooser)ucControl).UpgradeAll)
                    {
                        btnProcess.Visible = true;
                        btnNext.Visible = false;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnProcess.Visible = false;
                        btnNext.Visible = true;
                        btnSave.Visible = false;
                    }
                    // End TT#785-MD - JSmith - Installer Bug - Next and Process buttons

                    ProcessControl.ServerPane = null;

                    //set buttons
                    btnBack.Enabled = false;

                    break;

                case "ucScan":

                    //set the process control object
                    ProcessControl.ScanPane = (ucScan)ucControl;

                    //unload the current control
                    UnloadControl();

                    //log back action
                    ucInstallationLog1.AddLogEntry("Returning to the Utilities panel", eErrorType.message);

                    //set the control to load
                    ucControl = (ucUtilities)ProcessControl.UtilitiesPane;

                    //load the back control
                    LoadUserControl();

                    //disable the process button and enable the next button
                    btnProcess.Visible = false;
                    btnNext.Visible = true;

                    //set buttons
                    btnBack.Enabled = true;

                    break;

                case "ucUtilities":

                    //set the process control object
                    ProcessControl.UtilitiesPane = (ucUtilities)ucControl;

                    //unload the current control
                    UnloadControl();

                    //log back action
                    ucInstallationLog1.AddLogEntry("Returning to the Install Chooser panel", eErrorType.message);

                    //set the control to load
                    ucControl = (ucInstallChooser)ProcessControl.InstallChooserPane;

                    //load the back control
                    LoadUserControl();

                    //disable the process button and enable the next button
                    // Begin TT#785-MD - JSmith - Installer Bug - Next and Process buttons
                    //btnProcess.Visible = false;
                    //btnNext.Visible = true;
                    //btnSave.Visible = false;
                    if (((ucInstallChooser)ucControl).UpgradeAll)
                    {
                        btnProcess.Visible = true;
                        btnNext.Visible = false;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnProcess.Visible = false;
                        btnNext.Visible = true;
                        btnSave.Visible = false;
                    }
                    // End TT#785-MD - JSmith - Installer Bug - Next and Process buttons

                    //set buttons
                    btnBack.Enabled = false;

                    break;

                case "ucConfig":

                    //set the process control object
                    ProcessControl.ConfigurationPane = (ucConfig)ucControl;

                    if (ProcessControl.ConfigurationPane.EditCount > 0)
                    {

                        if (MessageBox.Show("Do you want to save the setting changes you've made in this session?",
                            "Save Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //log the action
                            ucInstallationLog1.AddLogEntry("Saving configuration file modifications", eErrorType.message);

                            ProcessControl.ConfigurationPane.SaveConfigChanges();
                        }
                        else
                        {
                            //blClose = false;
                        }
                    }

                    //unload the current control
                    UnloadControl();

                    //set the control to load
                    if (ProcessControl.ClientPane == null && ProcessControl.ServerPane == null)
                    {
                        ucControl = (ucInstallChooser)ProcessControl.InstallChooserPane;
                    }
                    else if (ProcessControl.ClientPane != null && ProcessControl.ServerPane == null)
                    {
                        //begin tt#837 - Unexpected Exception while running the installer - apicchetti - 08/17/2010
                        ProcessControl.ClientPane.rdoInstallClientTasks.Checked = true;
                        ProcessControl.ClientPane.cboTasks.Text = "Configure";
                        //end

                        ucControl = (ucClient)ProcessControl.ClientPane;

                    }
                    else if (ProcessControl.ClientPane == null && ProcessControl.ServerPane != null)
                    {
                        //begin tt#837 - Unexpected Exception while running the installer - apicchetti - 08/17/2010
                        ProcessControl.ServerPane.rdoInstalledServerTasks.Checked = true;
                        ProcessControl.ServerPane.cboTasks.Text = "Configure";
                        //end

                        ucControl = (ucServer)ProcessControl.ServerPane;
                    }
                    else
                    {
                        if (ProcessControl.InstallChooserLoad < ProcessControl.ClientLoad && ProcessControl.InstallChooserLoad < ProcessControl.ServerLoad)
                        {
                            ucControl = (ucInstallChooser)ProcessControl.InstallChooserPane;
                        }
                        else if (ProcessControl.ClientLoad < ProcessControl.InstallChooserLoad && ProcessControl.ClientLoad < ProcessControl.ServerLoad)
                        {
                            //begin tt#837 - Unexpected Exception while running the installer - apicchetti - 08/17/2010
                            ProcessControl.ClientPane.rdoInstallClientTasks.Checked = true;
                            ProcessControl.ClientPane.cboTasks.Text = "Configure";
                            //end

                            ucControl = (ucClient)ProcessControl.ClientPane;

                        }
                        else
                        {
                            //begin tt#837 - Unexpected Exception while running the installer - apicchetti - 08/17/2010
                            ProcessControl.ServerPane.rdoInstalledServerTasks.Checked = true;
                            ProcessControl.ServerPane.cboTasks.Text = "Configure";
                            //end

                            ucControl = (ucServer)ProcessControl.ServerPane;
                        }
                    }

                    //log back action
                    ucInstallationLog1.AddLogEntry("Navigate Back from " + ucControl.Name, eErrorType.message);

                    //load the back control
                    LoadUserControl();

                    //disable the back button
                    //btnBack.Enabled = false;

                    break;
            }

            if (ucControl.Name == "ucInstallChooser")
            {
                lblTitle.Text = "Welcome to the Logility - RO Setup and Configuration Wizard";
            }

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //set the close flag
            //bool blClose = true;
            ucConfig config;
            // Begin TT#74 MD - JSmith - One-button Upgrade
            bool blResult1;
            ucInstallChooser install_chooser;
            ucClient client;
            ucServer server;
            ucUtilities utilities;
            ConfigFiles cf;
            string serverName = "server";
            string databaseName = "database";
            string databaseUser = "user";
            string databasePwd = "pwd";
            string sql_version = "";
            string sql_level = "";
            string sql_edition = "";
            bool isAppServer = false;
            string MIDSettings = null;
            string connectionString = null;
            string ROExtractconnectionString = null;  // TT#2131-MD - JSmith - Halo Integration
            bool successful = true;
            // ENd TT#74 MD
            ProgressBarSetValue(0);

            // Begin TT#74 MD - JSmith - One-button Upgrade
            try
            {
            // End TT#74 MD
                switch (ucControl.Name)
                {
                    // Begin TT#74 MD - JSmith - One-button Upgrade
                    case "ucInstallChooser":
                        //set control
                        install_chooser = (ucInstallChooser)ucControl;

                        //upgrade all 
                        if (install_chooser.UpgradeAll)
                        {

                            this.Cursor = Cursors.WaitCursor;
                            //set workflow type
                            ProcessControl.Workflow = eWorkflowType.upgradeAll;
                            blPerformingOneClickUpgrade = true;

                            ArrayList installedItems = new ArrayList();  // TT#1822-MD - JSmith - Installer not detecting incomplete install

                            // determine registered component types
                            string component;
                            RegistryKey regKey;
                            string entryType;
                            bool blServicesRunning = false;
                            bool blDatabaseValid = false;
                            int intServicesCount = 0; ;
                            ArrayList alRunningServices = new ArrayList();  // TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
							// Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                            bool blDatabaseCompatible = false;
                            CompatibilityLevel compatibilityLevel = CompatibilityLevel.Undefined;
							// End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                            bool blDatabaseCompatibleChecked = false;  // TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            ProgressBarEnabled(true);
                            ProgressBarSetMinimum(0);
                            
                            
                            foreach (DictionaryEntry MIDRegistered in htMIDRegistered)
                            {
                                component = (string)MIDRegistered.Key;
                                regKey = (RegistryKey)MIDRegistered.Value;
                                entryType = regKey.GetValue("EntryType").ToString();

                                installedItems.Add(component);  // TT#1822-MD - JSmith - Installer not detecting incomplete install

                                if (component.Contains(InstallerConstants.cClientApp) == true)
                                {
                                    blClientComponentsInstalled = true;
                                }
                                else
                                {
                                    blServerComponentsInstalled = true;

                                    if (entryType == eEntryType.MIDControlService.ToString())
                                    {
                                        string[] location = component.Split('\\');
                                        string[] ExecValues = location[location.Length - 1].Split('.');
                                        blServicesRunning = isServiceRunning(ExecValues[0]);
                                        ++intServicesCount;
                                        // Begin TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
                                        if (blServicesRunning)
                                        {
                                            // Begin TT#3694 - JSmith - Services marked Unexpected Termination after upgrade
                                            //alRunningServices.Add(ExecValues[0]);
                                            alRunningServices.Insert(0, ExecValues[0]);
                                            // End TT#3694 - JSmith - Services marked Unexpected Termination after upgrade
                                        }
                                        // End TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.

                                        blDatabaseValid = false;
                                        blDatabaseCompatibleChecked = true;  // TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                        cf = new ConfigFiles(this, installer_data, ucInstallationLog1);
                                        XmlDocument doc = cf.GetXmlDocument(component + ".config");
                                        MIDSettings = cf.GetValue(doc, InstallerConstants.cParent_AppSettings, InstallerConstants.cLookupType_Child, "File");
                                        if (!string.IsNullOrEmpty(MIDSettings))
                                        {
                                            connectionString = GetMIDSettingsDBConnectionString(MIDSettings);

                                            if (!string.IsNullOrEmpty(connectionString))
                                            {
                                                // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                                                //blDatabaseValid = VerifySQLVersion_Edition(connectionString, out sql_version, out sql_level, out sql_edition, out serverName, out databaseName, out databaseUser, out databasePwd);
                                                blDatabaseValid = VerifySQLVersion_Edition(connectionString, out sql_version, out sql_level, out sql_edition, out serverName, out databaseName, out databaseUser, out databasePwd, out blDatabaseCompatible, out compatibilityLevel);
                                                // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                                            }

                                            ROExtractconnectionString = GetMIDSettingsROExtractDBConnectionString(MIDSettings);  // TT#2131-MD - JSmith - Halo Integration
                                        }

                                        if (!blDatabaseValid)
                                        {
                                            SetLogMessage(GetText("noDBUpgrade"), eErrorType.warning);
                                        }
                                        isAppServer = true;
                                    }
                                    // Begin TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
                                    else if (entryType.Contains("Service"))
                                    {
                                        string[] location = component.Split('\\');
                                        string[] ExecValues = location[location.Length - 1].Split('.');
                                        blServicesRunning = isServiceRunning(ExecValues[0]);
                                        ++intServicesCount;
                                        if (blServicesRunning
                                            && entryType != InstallerConstants.cJobServiceKey)  // do not restart job service
                                        {
                                            alRunningServices.Add(ExecValues[0]);
                                        }
                                    }
                                    // End TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
                                }
                            }

                            if (isAppServer)
                            {
                                ProgressBarSetMaximum(htMIDRegistered.Count + intServicesCount + 3 + (alRunningServices.Count * 2));
                            }
                            else
                            {
                                ProgressBarSetMaximum(htMIDRegistered.Count + 1);
                            }

                            // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                            // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            //if (!blDatabaseCompatible)
                            if (blDatabaseCompatibleChecked &&
                                !blDatabaseCompatible)
                            // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            {
                                ucInstallationLog1.AddLogEntry("Database Compatibility Level = " + compatibilityLevel, eErrorType.error);
                                SetStatusMessage("Database compatibility level is not valid.  " + GetText("upgradeCancelled"));
                                break;
                            }
                            // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install

                            //log message
                            ucInstallationLog1.AddLogEntry(GetText("upgradeAllComponents"), eErrorType.message);
                            

                            OncClickUpdateConfirmation oncClickUpdateConfirmation = new OncClickUpdateConfirmation(isAppServer, serverName, databaseName, this);
                            if (oncClickUpdateConfirmation.ShowDialog() == DialogResult.Cancel)
                            {
                                SetStatusMessage(GetText("upgradeCancelled"));
                                break;
                            }

                            // Begin TT#195 MD - JSmith - Add environment authentication
                            // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            if (blDatabaseCompatibleChecked)
                            {
                            // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                string databaseConfiguration = "";
                                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                                string assemblyConfiguration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
                                GetDatabaseConfiguration(connectionString, out databaseConfiguration);
                                if (databaseConfiguration != null &&
                                    databaseConfiguration != assemblyConfiguration)
                                {
                                    string msg = "Installer configuration of " + assemblyConfiguration + " does not match database configuration of " + databaseConfiguration + ".";
                                    if (MessageBox.Show(msg + Environment.NewLine + "Do you want to continue with the upgrade?", "",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        break;
                                    }
                                }
                            }  // TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            // End TT#195 MD

                            successful = false;
                            ProgressBarIncrementValue(1);

                            if (blClientComponentsInstalled)
                            {
                                SetStatusMessage(GetText("upgradeAllClients"));
                                //set up object
                                if (ProcessControl.ClientPane == null)
                                {
                                    client = new ucClient(this, ucInstallationLog1);
                                }
                                else
                                {
                                    client = (ucClient)ProcessControl.ClientPane;
                                }

                                if (client.LoadInstalledClients())
                                {
                                    if (client.SelectAll())
                                    {
                                        if (client.Upgrade(false, _64bit))
                                        {
                                            successful = true;
                                        }
                                    }
                                }
                            }
                            // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                            else
                            {
                                successful = true;
                            }
                            // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers

                            if (blServerComponentsInstalled &&
                                successful)
                            {
                                successful = false;
                                SetStatusMessage(GetText("upgradeAllServers"));
                                successful = false;
                                if (ProcessControl.UtilitiesPane == null)
                                {
                                    utilities = new ucUtilities(this, ucInstallationLog1, false, true);
                                }
                                else
                                {
                                    utilities = (ucUtilities)ProcessControl.UtilitiesPane;
                                }

                                // stop the services
                                if (utilities.DoStopServices())
                                {
                                    ProgressBarIncrementValue(1);
                                    if (ProcessControl.ServerPane == null)
                                    {
                                        server = new ucServer(this, ucInstallationLog1, sInstallerLocation);
                                    }
                                    else
                                    {
                                        server = (ucServer)ProcessControl.ServerPane;
                                    }

                                    if (server.LoadInstalledServerComponents())
                                    {
                                        //perform upgrade
                                        if (server.SelectAll())
                                        {
                                            if (server.Upgrade())
                                            {
                                                // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                                //if (blDatabaseValid)
                                                if (isAppServer &&
                                                    blDatabaseValid)
                                                // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                                {
                                                    SetStatusMessage(GetText("databaseMaintenance"));
                                                    ProgressBarIncrementValue(1);
                                                    if (utilities.DoDatabaseMaintenance(connectionString, true))	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
                                                    {
                                                        ProgressBarIncrementValue(1);
                                                        SetStatusMessage(GetText("databaseMaintenanceComplete"));
                                                        successful = true;
                                                        // Begin TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
                                                        //if (blServicesRunning)
                                                        //{
                                                        //    if (!utilities.DoStartServices())
                                                        //    {
                                                        //        successful = false;
                                                        //    }
                                                        //}
                                                        if (alRunningServices.Count > 0)
                                                        {
                                                            if (!utilities.DoStartServices(alRunningServices))
                                                            {
                                                                successful = false;
                                                            }
                                                        }
                                                        ProgressBarIncrementValue(1);
                                                        // End TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
                                                    }
                                                    // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                                                    else
                                                    {
                                                        successful = false;
                                                    }
                                                    // End TT#1822-MD - JSmith - Installer not detecting incomplete install

                                                    // Begin TT#2131-MD - JSmith - Halo Integration
                                                    if (successful
                                                        && !string.IsNullOrEmpty(ROExtractconnectionString))
                                                    {
                                                        SetStatusMessage(GetText("databaseMaintenance"));
                                                        ProgressBarIncrementValue(1);
                                                        if (utilities.DoDatabaseMaintenance(ROExtractconnectionString, true))
                                                        {
                                                            ProgressBarIncrementValue(1);
                                                            SetStatusMessage(GetText("databaseMaintenanceComplete"));
                                                        }
                                                    }
                                                    // End TT#2131-MD - JSmith - Halo Integration
                                                }
                                                // Begin TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                                else
                                                {
                                                    successful = true;
                                                }
                                                // End TT#3763 - JSmith - One-Click Upgrade fails for batch servers
                                            }
											// Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                                            else
                                            {
                                                successful = false;
                                            }
											// End TT#1822-MD - JSmith - Installer not detecting incomplete install
                                        }
                                    }
                                }
                            }

                            // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                            if (successful)
                            {
                                successful = VerifyUpgrade(installedItems); 
                            }
                            // End TT#1822-MD - JSmith - Installer not detecting incomplete install

                            ProgressBarSetToMaximum();

                            //new title
                            lblTitle.Text = "Choose the Logility-RO task to perform";
                            if (successful)
                            {
                                SetStatusMessage(GetText("upgradeCompletedSuccessfully"));
                            }
                            else
                            {
                                SetStatusMessage(GetText("upgradeCompletedWithErrors"), eErrorType.error);
                            }

                            //show finish button and hide next button
                            //btnBack.Enabled = true;
                            //btnNext.Visible = false;
                            //btnSave.Visible = false;
                            //btnProcess.Visible = true;

                        }

                        break;
                    // End TT#74

                    case "ucScan":

                        //log the action
                        ucInstallationLog1.AddLogEntry("Rescanning the computer for installed components", eErrorType.message);

                        //remove registry entries from previous scans
                        RemoveRegisteredComponents();

                        //rescan
                        Scan_Next();

                        //unload the showing control
                        ProcessControl.ScanPane = (ucScan)ucControl;
                        UnloadControl();

                        //load the next pane
                        // Begin TT#74 MD - JSmith - One-button Upgrade
                        //ucInstallChooser install_chooser = new ucInstallChooser(this);
                        install_chooser = new ucInstallChooser(this);
                        // End TT#74 MD
                        //add event handlers
                        install_chooser.NotConfigureNext += new EventHandler(this.NotConfigureNext);
                        install_chooser.ConfigureNext += new EventHandler(this.ConfigureNext);
                        ucControl = install_chooser;
                        lblTitle.Text = "Choose the Installation you want to perform";
                        LoadUserControl();

                        //show buttons
                        btnBack.Enabled = false;
                        btnNext.Visible = true;
                        btnProcess.Visible = false;

                        ucInstallationLog1.AddLogEntry("Installation Chooser panel loaded", eErrorType.message);

                        break;

                    case "ucClient":

                        // Begin TT#74 MD - JSmith - One-button Upgrade
                        //ucClient client = (ucClient)ucControl;
                        client = (ucClient)ucControl;
                        // End TT#74 MD - JSmith - One-button Upgrade
                        string strTarget = "";
                        bool blResult;

                        switch (itTask)
                        {
                            case eInstallTasks.typicalInstall:

                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to install typical client components", eErrorType.message);

                                //set control 
                                ProcessControl.ClientPane = (ucClient)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                strTarget = "";
                                // Begin TT#1249 - stodd - 
                                blResult = ProcessControl.ClientPane.Install(ConfigurationManager.AppSettings["ClientInstallLocation"].ToString(), null, null, null, out strTarget, false);
                                // End TT#1249 - stodd - 

                                // make sure event sources are created
                                if (blResult)
                                {
                                    if (ProcessControl.UtilitiesPane == null)
                                    {
                                        ProcessControl.UtilitiesPane = new ucUtilities(this, ucInstallationLog1, false, true);
                                    }

                                    ProcessControl.UtilitiesPane.DoEventSources();
                                }

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (blResult == true)
                                {
                                    //add application to a list needed by the config control
                                    List<string> lstFilesToConfig = new List<string>();
                                    lstFilesToConfig.Add(Directory.GetParent(strTarget).ToString().Trim() + @"\");


                                    //log message
                                    ucInstallationLog1.AddLogEntry("Configuring the installed client components", eErrorType.message);

                                    //unload the previous control
                                    UnloadControl();

                                    //new object
                                    config = new ucConfig(this, strTarget, eConfigType.Client, lstFilesToConfig, ucInstallationLog1);
                                    config.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                    //new title
                                    lblTitle.Text = "Configure the client application settings";

                                    //load control
                                    ucControl = config;
                                    LoadUserControl();

                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnProcess.Visible = false;
                                    btnSave.Visible = true;
                                }

                                break;

                            case eInstallTasks.install:

                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to install client components", eErrorType.message);

                                //set control 
                                ProcessControl.ClientPane = (ucClient)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                strTarget = "";
                                // Begin TT#1249 - stodd - 
                                blResult = ProcessControl.ClientPane.Install(null, null, null, null, out strTarget, false);
                                // End TT#1249 - stodd - 

                                // make sure event sources are created
                                if (blResult == true)
                                {
                                    if (ProcessControl.UtilitiesPane == null)
                                    {
                                        ProcessControl.UtilitiesPane = new ucUtilities(this, ucInstallationLog1, false, true);
                                    }

                                    ProcessControl.UtilitiesPane.DoEventSources();
                                }

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (blResult == true)
                                {
                                    //add application to a list needed by the config control
                                    List<string> lstFilesToConfig = new List<string>();
                                    lstFilesToConfig.Add(Directory.GetParent(strTarget).ToString().Trim() + @"\");


                                    //log message
                                    ucInstallationLog1.AddLogEntry("Configuring the installed client components", eErrorType.message);

                                    //unload the previous control
                                    UnloadControl();

                                    //new object
                                    config = new ucConfig(this, strTarget, eConfigType.Client, lstFilesToConfig, ucInstallationLog1);
                                    config.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                    //new title
                                    lblTitle.Text = "Configure the client application settings";

                                    //load control
                                    ucControl = config;
                                    LoadUserControl();

                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnProcess.Visible = false;
                                    btnSave.Visible = true;
                                }

                                break;

                            case eInstallTasks.uninstall:
                                if (client.isItemSelected)
                                {
                                    if (MessageBox.Show("Are you sure you want to uninstall this Logility RO Client application?",
                                        "Uninstall Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                    {
                                        client.Uninstall();

                                        client.SetRadioButtonAfterUninstall();
                                    }
                                }

                                break;

                            case eInstallTasks.upgrade:

                                ArrayList installedItems = new ArrayList();  // TT#1822-MD - JSmith - Installer not detecting incomplete install

                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to upgrade client components", eErrorType.message);

                                //set control 
                                ProcessControl.ClientPane = (ucClient)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                // Begin TT#74 MD - JSmith - One-button Upgrade
                                //bool blResult1 = ProcessControl.ClientPane.Upgrade(ProcessControl.ClientPane.AutoUpgrade, _64bit);
                                blResult1 = ProcessControl.ClientPane.Upgrade(ProcessControl.ClientPane.AutoUpgrade, _64bit);
                                // End TT#74 MD

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (blResult1 == true)
                                {
                                    //unload the previous control
                                    UnloadControl();


                                    //add application to a list needed by the config control
                                    List<string> lstFilesToConfig2 = new List<string>();
                                    //lstFilesToConfig2.Add(Directory.GetParent(UninstallFile).ToString().Trim() + @"\");
                                    foreach (string selectedItem in ProcessControl.ClientPane.lstInstalledClients.SelectedItems)
                                    {
                                        installedItems.Add(selectedItem);  // TT#1822-MD - JSmith - Installer not detecting incomplete install

                                        lstFilesToConfig2.Add(Directory.GetParent(selectedItem).ToString().Trim() + @"\");
                                    }

                                    blResult1 = VerifyUpgrade(installedItems);  // TT#1822-MD - JSmith - Installer not detecting incomplete install

                                    if (blResult1 == true)
                                    {
                                        //new object
                                        config = new ucConfig(this, lstFilesToConfig2[0].ToString().Trim(), eConfigType.Client, lstFilesToConfig2, ucInstallationLog1);
                                        config.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                        //new title
                                        lblTitle.Text = "Configure the client application settings";

                                        //load control
                                        ucControl = config;
                                        LoadUserControl();

                                        //log message
                                        ucInstallationLog1.AddLogEntry("Configuring the upgraded client components", eErrorType.message);

                                        //show process button and hide next button
                                        btnBack.Enabled = true;
                                        btnNext.Visible = false;
                                        btnProcess.Visible = false;
                                        btnSave.Visible = true;
                                    }
                                    else
                                    {
                                        SetStatusMessage(GetText("upgradeCompletedWithErrors"), eErrorType.error);
                                    }
                                }

                                break;

                            case eInstallTasks.setasautoupdate:

                                if (MessageBox.Show("Are you sure you want to set this client to recieve automatic upgrades?",
                                    "Automatic Upgrade", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {

                                    //drill into the registry
                                    RegistryKey local_key = Registry.LocalMachine;
                                    RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);
                                    string[] subKeyNames = mid_key.GetSubKeyNames();

                                    //find the sub key for the installed component
                                    foreach (string subKeyName in subKeyNames)
                                    {
                                        RegistryKey sub_key = mid_key.OpenSubKey(subKeyName, true);

                                        if (strUninstallFile == sub_key.GetValue("Location").ToString().Trim())
                                        {
                                            //get the version number form the file
                                            Assembly assem = Assembly.ReflectionOnlyLoadFrom(strUninstallFile);
                                            AssemblyName assemName = assem.GetName();
                                            Version ver = assemName.Version;

                                            //log the action
                                            ucInstallationLog1.AddLogEntry("Set the following client as an auto-upgrade client",
                                                eErrorType.message);

                                            //set the auto upgrade value in the registry key
                                            sub_key.SetValue("AutoUpgrade", ver.ToString());

                                        }
                                    }
                                }

                                //give user feedback
                                MessageBox.Show("The chosen client has been set to automatically upgrade.",
                                    "Logility RO Client Task", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                break;
                        }

                        break;

                    case "ucServer":

                        List<string> lstResult;

                        // Begin TT#74 MD - JSmith - One-button Upgrade
                        //ucServer server = (ucServer)ucControl;
                        //ConfigFiles cf = new ConfigFiles(installer_data, ucInstallationLog1);
                        server = (ucServer)ucControl;
                        cf = new ConfigFiles(this, installer_data, ucInstallationLog1);
                        // End TT#74 MD

                        switch (itTask)
                        {
                            case eInstallTasks.typicalInstall:

                                blPerformingTypicalInstall = true;
                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to install typical server components", eErrorType.message);

                                ProgressBarEnabled(true);
                                ProgressBarSetMinimum(0);
                                ProgressBarSetMaximum(9);

                                //set control 
                                ProcessControl.ServerPane = (ucServer)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                lstResult = ProcessControl.ServerPane.Install(ConfigurationManager.AppSettings["ServicesInstallLocation"].ToString());
                                string strInstallLocation = ProcessControl.ServerPane.strInstallLocation;

                                if (lstResult.Count == 0)
                                {
                                    break;
                                }

                                //get the list of selected controls
                                List<string> lstFilesToConfig4 = new List<string>();

                                if (lstResult.Count > 0)
                                {
                                    foreach (string Item in lstResult)
                                    {
                                        if (Item.Contains("Batch"))
                                        {
                                            lstFilesToConfig4.Add(Item + @"\");
                                        }
                                        else
                                        {
                                            lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                        }
                                    }
                                }

                                // //log the action
                                //ucInstallationLog1.AddLogEntry("Installing global configuration", eErrorType.message);

                                ////run the server InstallGlobalConfiguration
                                //lstResult = server.InstallGlobalConfiguration(ConfigurationManager.AppSettings["ClientConfigurationLocation"].ToString());

                                //if (lstResult.Count > 0)
                                //{
                                //    foreach (string Item in lstResult)
                                //    {
                                //        lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                //    }
                                //}

                                //log the action
                                ucInstallationLog1.AddLogEntry("Installing auto upgrade client global configuration", eErrorType.message);
                                string strClientConfigurationLocation = ConfigurationManager.AppSettings["ClientConfigurationLocation"].ToString();
                                string strPathRoot = Path.GetPathRoot(strInstallLocation);
                                strClientConfigurationLocation = strClientConfigurationLocation.Replace(@"C:\", strPathRoot);

                                //run the server InstallGlobalConfiguration
                                lstResult = server.InstallGlobalConfiguration(strClientConfigurationLocation);

                                if (lstResult.Count > 0)
                                {
                                    string globalSettings = strClientConfigurationLocation + @"\" + ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
                                    cf.SetConfigValue(globalSettings, InstallerConstants.cParent_AppSettings, InstallerConstants.cLookupType_Child,  "AutoUpgradeClient", "True");
                                    cf.SetConfigValue(globalSettings, InstallerConstants.cParent_AppSettings, InstallerConstants.cLookupType_Child, "AutoUpgradePath", ConfigurationManager.AppSettings["AutoUpgradeShareLocation"].ToString().Replace("%machine%", GetMachineName()) + @"\" + ConfigurationManager.AppSettings["InstallClientFolder"].ToString());
                                    //cf.SetConfigValue(globalSettings, "HeaderReleaseFilePath", @"\\" + GetMachineName() + @"\" + ConfigurationManager.AppSettings["ReleaseShareName"].ToString());

                                    foreach (string Item in lstResult)
                                    {
                                        lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                    }
                                }

                                if (!IsClientInstalled)
                                {
                                    // load client to install client
                                    if (ProcessControl.ClientPane == null)
                                    {
                                        ProcessControl.ClientPane = new ucClient(this, ucInstallationLog1);
                                    }

                                    string clientInstallLocation = ConfigurationManager.AppSettings["ClientInstallLocation"].ToString();
                                    if (strInstallLocation != null)
                                    {
                                        clientInstallLocation = strInstallLocation;
                                    }
                                    //perform install client
                                    strTarget = "";
                                    // Begin TT#1249 - stodd - 
                                    blResult = ProcessControl.ClientPane.Install(clientInstallLocation, null, null, null, out strTarget, false);
                                    // Begin TT#1249 - stodd - 

                                    if (blResult == true)
                                    {
                                        string clientSettings = clientInstallLocation
                                            + @"\" + ConfigurationManager.AppSettings["InstallClientFolder"].ToString()
                                            + @"\" + ConfigurationManager.AppSettings["Client_Folder"].ToString()
                                            + @"\" + ConfigurationManager.AppSettings["MIDRetail_config"].ToString();
                                        //C:\Logility\ROClient\Client\GlobalSettings
                                        string configFile = ConfigurationManager.AppSettings["ClientConfigurationShareLocation"].ToString().Replace("%machine%", GetMachineName())
                                            + @"\" + ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
                                        cf.SetConfigValue(clientSettings, InstallerConstants.cParent_AppSettings, InstallerConstants.cLookupType_Child, "file", configFile);
                                        //add application to a list needed by the config control
                                        lstFilesToConfig4.Add(Directory.GetParent(strTarget).ToString().Trim() + @"\");
                                    }

                                    //perform install auto upgrade client
                                    strTarget = "";
                                    ProcessControl.ClientPane.BInstallingAutoUpgradeClient = true;
                                    string strAutoUpgradeClientLocation = ConfigurationManager.AppSettings["AutoUpgradeClientLocation"].ToString();
                                    string strSharePath = ConfigurationManager.AppSettings["SharePath"].ToString();
                                    strAutoUpgradeClientLocation = strAutoUpgradeClientLocation.Replace(@"C:\", strPathRoot);
                                    strSharePath = strSharePath.Replace(@"C:\", strPathRoot);
                                    // Begin TT#1249 - stodd - 
                                    blResult = ProcessControl.ClientPane.Install(strAutoUpgradeClientLocation, strSharePath, ConfigurationManager.AppSettings["ShareName"].ToString(), ConfigurationManager.AppSettings["ShortcutName"].ToString(), out strTarget, true);
                                    // Begin TT#1249 - stodd - 
                                    ProcessControl.ClientPane.BInstallingAutoUpgradeClient = false;

                                    if (blResult == true)
                                    {
                                        //C:\Logility\ROClient\Client\AutoUpgrade\Client\Client
                                        string clientSettings = ConfigurationManager.AppSettings["AutoUpgradeClientLocation"].ToString()
                                            + @"\" + ConfigurationManager.AppSettings["InstallClientFolder"].ToString()
                                            + @"\" + ConfigurationManager.AppSettings["Client_Folder"].ToString()
                                            + @"\" + ConfigurationManager.AppSettings["MIDRetail_config"].ToString();
                                        //C:\Logility\ROClient\Client\GlobalSettings
                                        string configFile = ConfigurationManager.AppSettings["ClientConfigurationShareLocation"].ToString().Replace("%machine%", GetMachineName())
                                            + @"\" + ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
                                        cf.SetConfigValue(clientSettings, InstallerConstants.cParent_AppSettings, InstallerConstants.cLookupType_Child, "file", configFile);
                                        //add application to a list needed by the config control
                                        lstFilesToConfig4.Add(Directory.GetParent(strTarget).ToString().Trim() + @"\");
                                    }
                                }

                                // make sure event sources are created
                                if (ProcessControl.UtilitiesPane == null)
                                {
                                    ProcessControl.UtilitiesPane = new ucUtilities(this, ucInstallationLog1, false, true);
                                }

                                ProcessControl.UtilitiesPane.DoEventSources();

                                // present configuration control
                                if (lstFilesToConfig4.Count > 0)
                                {
                                    //unload the previous control
                                    UnloadControl();

                                    //new object
                                    ucConfig config_svr = new ucConfig(this, lstFilesToConfig4[0].ToString().Trim(),
                                        eConfigType.Server, lstFilesToConfig4, ucInstallationLog1);
                                    config_svr.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                    //new title
                                    lblTitle.Text = "Configure the server application settings";

                                    //load control
                                    ucControl = config_svr;
                                    LoadUserControl();


                                    //log message
                                    ucInstallationLog1.AddLogEntry("Configuring the installed server components", eErrorType.message);


                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnProcess.Visible = false;
                                    btnSave.Visible = true;
                                }

                                ProgressBarSetToMaximum();
                                //hourglass
                                this.Cursor = Cursors.Default;


                                break;

                            case eInstallTasks.install:

                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to install server components", eErrorType.message);

                                //set control 
                                ProcessControl.ServerPane = (ucServer)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                lstResult = ProcessControl.ServerPane.Install(null);

                                if (lstResult.Count == 0)
                                {
                                    break;
                                }

                                // make sure event sources are created
                                if (ProcessControl.UtilitiesPane == null)
                                {
                                    ProcessControl.UtilitiesPane = new ucUtilities(this, ucInstallationLog1, false, true);
                                }

                                ProcessControl.UtilitiesPane.DoEventSources();

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (lstResult.Count > 0)
                                {
                                    //get the list of selected controls
                                    lstFilesToConfig4 = new List<string>();

                                    foreach (string Item in lstResult)
                                    {
                                        if (Item.Contains("Batch"))
                                        {
                                            lstFilesToConfig4.Add(Item + @"\");
                                        }
                                        else
                                        {
                                            lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                        }
                                    }

                                    //unload the previous control
                                    UnloadControl();

                                    //new object
                                    ucConfig config_svr = new ucConfig(this, lstFilesToConfig4[0].ToString().Trim(),
                                        eConfigType.Server, lstFilesToConfig4, ucInstallationLog1);
                                    config_svr.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                    //new title
                                    lblTitle.Text = "Configure the server application settings";

                                    //load control
                                    ucControl = config_svr;
                                    LoadUserControl();


                                    //log message
                                    ucInstallationLog1.AddLogEntry("Configuring the installed server components", eErrorType.message);


                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnProcess.Visible = false;
                                    btnSave.Visible = true;
                                }

                                break;

                            case eInstallTasks.upgrade:

                                //log message
                                ucInstallationLog1.AddLogEntry("User has chosen to upgrade server components", eErrorType.message);

                                //set control 
                                ProcessControl.ServerPane = (ucServer)ucControl;

                                //hourglass
                                this.Cursor = Cursors.WaitCursor;

                                //perform install
                                // Begin TT#74 MD - JSmith - One-button Upgrade
                                //bool blResult1 = ProcessControl.ServerPane.Upgrade();
                                blResult1 = ProcessControl.ServerPane.Upgrade();
                                // End TT#74 MD

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (blResult1 == true)
                                {
                                    //get the list of selected controls
                                    List<string> lstFilesToConfig5 = new List<string>();
                                    foreach (string selectedItem in ProcessControl.ServerPane.lstInstalledServices.SelectedItems)
                                    {
                                        if (selectedItem.Contains("Batch"))
                                        {
                                            lstFilesToConfig5.Add(selectedItem);
                                        }
                                        else
                                        {
                                            lstFilesToConfig5.Add(Directory.GetParent(selectedItem).ToString().Trim() + @"\");
                                        }
                                    }

                                    if (blResult1 == true)
                                    {
                                        //unload the previous control
                                        UnloadControl();

                                        //new object
                                        ucConfig config1 = new ucConfig(this, UninstallFile, eConfigType.Server, lstFilesToConfig5, ucInstallationLog1);
                                        config1.EditModeInitiated += new EventHandler(this.ConfigEditInit);


                                        //new title
                                        lblTitle.Text = "Configure the server application(s) settings";

                                        //load control
                                        ucControl = config1;
                                        LoadUserControl();

                                        //log message
                                        ucInstallationLog1.AddLogEntry("Configuring the upgraded server components", eErrorType.message);
                                    }

                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnProcess.Visible = false;
                                    btnSave.Visible = true;
                                }

                                // Begin TT#1822-MD - JSmith - Installer not detecting incomplete install
                                ArrayList installedItems = new ArrayList();

                                foreach (string selectedItem in ProcessControl.ServerPane.lstInstalledServices.SelectedItems)
                                {
                                    installedItems.Add(selectedItem);  
                                }

                                blResult1 = VerifyUpgrade(installedItems);

                                if (blResult1 == false)
                                {
                                    SetStatusMessage(GetText("upgradeCompletedWithErrors"), eErrorType.error);
                                }
                                // End TT#1822-MD - JSmith - Installer not detecting incomplete install

                                break;

                            case eInstallTasks.uninstall:
                                if (server.isItemSelected)
                                {
                                    if (MessageBox.Show("Are you sure you want to uninstall this Logility RO Service application(s)?",
                                        "Uninstall Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                    {
                                        //log the action
                                        ucInstallationLog1.AddLogEntry("Uninstalling server components", eErrorType.message);

                                        //run the server uninstall
                                        server.Uninstall();

                                        //give user feedback
                                        //MessageBox.Show("The chosen application(s) have been removed.",
                                        //    "MIDRetail Service Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    //blClose = false;
                                }
                                else
                                {
                                    //blClose = true;
                                }

                                break;

                            case eInstallTasks.installConfiguration:

                                //log the action
                                ucInstallationLog1.AddLogEntry("Installing global configuration", eErrorType.message);

                                //run the server InstallGlobalConfiguration
                                lstResult = server.InstallGlobalConfiguration(null);

                                //hourglass
                                this.Cursor = Cursors.Default;

                                if (lstResult.Count > 0)
                                {
                                    //get the list of selected controls
                                    lstFilesToConfig4 = new List<string>();

                                    foreach (string Item in lstResult)
                                    {
                                        lstFilesToConfig4.Add(Directory.GetParent(Item).ToString().Trim() + @"\");
                                    }

                                    //unload the previous control
                                    UnloadControl();

                                    //new object
                                    ucConfig config_svr = new ucConfig(this, lstFilesToConfig4[0].ToString().Trim(),
                                        eConfigType.Server, lstFilesToConfig4, ucInstallationLog1);
                                    config_svr.EditModeInitiated += new EventHandler(this.ConfigEditInit);

                                    //new title
                                    lblTitle.Text = "Configure the server application settings";

                                    //load control
                                    ucControl = config_svr;
                                    LoadUserControl();


                                    //log message
                                    ucInstallationLog1.AddLogEntry("Configuring the installed server components", eErrorType.message);


                                    //show finish button and hide next button
                                    btnBack.Enabled = true;
                                    btnNext.Visible = false;
                                    btnSave.Visible = false;
                                    btnProcess.Visible = true;
                                }

                                break;
                        }

                        break;

                    case "ucUtilities":

                        // Begin TT#74 MD - JSmith - One-button Upgrade
                        //ucUtilities utilities = (ucUtilities)ucControl;
                        utilities = (ucUtilities)ucControl;
                        
                        //bool successful = true;
                        // End TT#74 MD

                        switch (itTask)
                        {
                            case eInstallTasks.databaseMaintenance:
                                successful = utilities.DoDatabaseMaintenance();

                                break;

                            case eInstallTasks.startServices:
                                ProgressBarSetMinimum(0);
                                ProgressBarSetMaximum(utilities.ServicesCount + 1);
                                successful = utilities.DoStartServices();
                                ProgressBarSetToMaximum();
                                if (!successful)
                                {
                                    MessageBox.Show(GetText("UtilitiesServerStartError"), "Service Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                //blClose = false;
                                break;

                            case eInstallTasks.stopServices:
                                ProgressBarSetMinimum(0);
                                ProgressBarSetMaximum(utilities.ServicesCount + 1);
                                successful = utilities.DoStopServices();
                                ProgressBarSetToMaximum();
                                if (!successful)
                                {
                                    MessageBox.Show(GetText("UtilitiesServerStopError"), "Service Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                //blClose = false;

                                break;

                            case eInstallTasks.eventSources:
                                successful = utilities.DoEventSources();

                                break;

                            //case eInstallTasks.crystalReports:
                            //    successful = utilities.InstallCrystalReports(false);

                            //    break;
                        }

                        break;
                }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            }
            finally
            {
                blPerformingOneClickUpgrade = false;
                blPerformingTypicalInstall = false;
                FlushLog();  // TT#1822-MD - JSmith - Installer not detecting incomplete install
                this.Cursor = Cursors.Default;
            }
            // End TT#74 MD
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //set the close flag
            //bool blClose = true;
            ucConfig config;

            switch (ucControl.Name)
            {

                case "ucConfig":

                    config = (ucConfig)ucControl;

                    if (config.EditCount > 0)
                    {

                        if (MessageBox.Show("Are you sure you want to save the setting changes you've made in this session?",
                            "Save Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            //log the action
                            ucInstallationLog1.AddLogEntry("Saving configuration file modifications", eErrorType.message);

                            config.SaveConfigChanges();
                        }
                        else
                        {
                            //blClose = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have no setting changes pending in this session.", "Save Settings", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;

            }
        }

        //uninstall file property
        private string strUninstallFile = "";
        public string UninstallFile
        {
            get
            {
                return strUninstallFile;
            }
            set
            {
                strUninstallFile = value;
            }
        }

        //install task property
        private eInstallTasks itTask;
        public eInstallTasks InstallTask
        {
            get
            {
                return itTask;
            }
            set
            {
                itTask = value;
            }
        }

        public bool IsClientInstalled
        {
            get
            {
                foreach (string key in htMIDInstalled.Keys)
                {
                    if (key.Contains(InstallerConstants.cClientApp))
                    {
                        return true;
                    }
                }
                return false;
            }
            
        }

        public void RemoveRegistryFiles(string UninstallFile)
        {
		    // Begin TT#1668 - JSmith - Install Log
            string msg = GetText("RegistryComponentRemove").Replace("{0}", UninstallFile);
            SetLogMessage(msg, eErrorType.message);
			// End TT#1668

            //remove our key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);
            string[] sub_keynames = mid_key.GetSubKeyNames();
            string exeSubKey = "";

            foreach (string sub_keyname in sub_keynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(sub_keyname, true);

                if ((string)sub_key.GetValue("InstallType") == "Manual" ||
                    (string)sub_key.GetValue("InstallType") == "MIDRetail")
                {
                    if ((string)sub_key.GetValue("Location").ToString().Trim() == UninstallFile)
                    {
					    // Begin TT#1668 - JSmith - Install Log
                        msg = GetText("RegistryKey").Replace("{0}", sub_keyname);
                        SetLogMessage(msg, eErrorType.message);
						// End TT#1668
                        mid_key.DeleteSubKey(sub_keyname);
                        exeSubKey = sub_keyname;
                        break;
                    }
                }
            }

            //remove our config key
            foreach (string sub_keyname in sub_keynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(sub_keyname, true);

                if (sub_keyname.Contains("MIDConfig") == true)
                {
                    if ((string)sub_key.GetValue("Client") == exeSubKey)
                    {
					    // Begin TT#1668 - JSmith - Install Log
                        msg = GetText("RegistryKey").Replace("{0}", sub_keyname);
                        SetLogMessage(msg, eErrorType.message);
						// End TT#1668
                        mid_key.DeleteSubKey(sub_keyname);
                        break;
                    }
                }
            }
        }

        public void RemoveManuallyInstalledComponent(string UninstallFile)
        {
            //get registry key information
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            string[] app_keys = mid_key.GetSubKeyNames();

            //variable for installed files 
            string strDelimInstalledFiles = "";

            //loop thru the installed files and delete them
            foreach (string app_key in app_keys)
            {
                //open the sub key
                RegistryKey sub_key = mid_key.OpenSubKey(app_key);

                //compare location information
                if (sub_key.GetValue("Location").ToString().Trim() == UninstallFile)
                {
                    //get the installed file inventory
                    if (sub_key.GetValue("InstalledFiles") != null)
                    {
                        strDelimInstalledFiles = sub_key.GetValue("InstalledFiles").ToString().Trim();
                    }
                    else
                    {
                        string dir = UninstallFile.ToString().Trim().Substring(0, UninstallFile.LastIndexOf("\\")); ;
                        string[] files = Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            strDelimInstalledFiles += file + ";";
                        }

                        dir = Directory.GetParent(Directory.GetParent(UninstallFile).ToString().Trim()).ToString().Trim() + @"\Graphics";
                        files = Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            strDelimInstalledFiles += file + ";";
                        }
                    }
                    break;
                }
            }

            //split the installed files into an array
            char[] delim = ";".ToCharArray();
            string[] strInstalledFiles = strDelimInstalledFiles.Split(delim);

            //delete the files
            foreach (string strInstalledFile in strInstalledFiles)
            {
                if (strInstalledFile != "")
                {
                    DeleteFile(strInstalledFile);
                }
            }

            //get dir list
            string Parent = Directory.GetParent(Directory.GetParent(UninstallFile).ToString().Trim()).ToString().Trim();

            //delete empty dirs
            if (Directory.Exists(Parent))
            {
                string[] subDirs = Directory.GetDirectories(Parent);
                foreach (string subDir in subDirs)
                {
                    string[] subDir_Files = Directory.GetFiles(subDir);
                    int intFiles = 0;
                    foreach (string subDir_File in subDir_Files)
                    {
                        intFiles++;
                    }

                    //if (intFiles == 0)  delete folder regardless of contents
                    {
                        DeleteFolder(subDir);
                    }
                }
            }
        }

        private string GetConfigClientRegKeyName(string config_path, string app_name)
        {
            //return variable
            string strClientRegKeyName = "";

            //Get the client install parent and application directory
            string strClientPath = Directory.GetParent(config_path).ToString().Trim() + @"\" + app_name;

            //drill into the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            string[] sub_keynames = mid_key.GetSubKeyNames();

            //loop thru the keys and find the client
            foreach (string sub_keyname in sub_keynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(sub_keyname);

                if (sub_key.GetValue("Location").ToString().Trim() == strClientPath)
                {
                    strClientRegKeyName = sub_key.Name;
                    break;
                }
            }

            //return value
            char[] delim = @"\".ToCharArray();
            string[] strFileParts = strClientRegKeyName.Split(delim);
            strClientRegKeyName = strFileParts[strFileParts.GetUpperBound(0)].ToString().Trim();
            return strClientRegKeyName;
        }

        public void RemoveWindowsInstalledComponent(string UninstallKey)
        {
            //prep the uninstall process
            Process uninstall = new Process();
            uninstall.StartInfo.FileName = @"C:\windows\system32\msiexec.exe";
            uninstall.StartInfo.Arguments = "/x{" + UninstallKey + "} /passive";
            uninstall.Start();
            uninstall.WaitForExit();

            //remove our key
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey, true);
            string[] sub_keynames = mid_key.GetSubKeyNames();

            foreach (string sub_keyname in sub_keynames)
            {
                RegistryKey sub_key = mid_key.OpenSubKey(sub_keyname,true);

                if ((string)sub_key.GetValue("InstallType") == "Windows")
                {
                    if((string)sub_key.GetValue("WindowsInstallKey").ToString().Trim() == UninstallKey)
                    {
                        mid_key.DeleteSubKey(sub_keyname);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (pnFrame.Controls[0].Name == "ucConfig" &&
               ((ucConfig)pnFrame.Controls[0]).EditCount > 0)
            {

                if (MessageBox.Show("Do you want to save the setting changes you've made in this session?",
                    "Save Settings", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //log the action
                    ucInstallationLog1.AddLogEntry("Saving configuration file modifications", eErrorType.message);

                    ((ucConfig)pnFrame.Controls[0]).SaveConfigChanges();
                }
                else
                {
                    //blClose = false;
                }
            }

            // BEGIN TT#1284 - AGallagher - Received  a database error, then was stuck in an infiinite loop  of the Database Incompatibility message when clicking the "Exit" button.   The only exit from the installer is through the "X".
            //string IncompatibilityMsg = "";
            //bool blDbCompatible = DBCompatibilityCheck(out IncompatibilityMsg);

            //if (blDbCompatible != true)
            //{
            //    MessageBox.Show("The target database(s) does not meet MID installation requirements." +
            //        "You must correct this before this installation's configuration can be saved:" + Environment.NewLine +
            //        IncompatibilityMsg, "SQL Compatibility", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            // END TT#1284 - AGallagher - Received  a database error, then was stuck in an infiinite loop  of the Database Incompatibility message when clicking the "Exit" button.   The only exit from the installer is through the "X".
            {
                //shutdown the application
                this.Close();
                Application.Exit();
            }
        }

        private void InstallerFrame_Resize(object sender, EventArgs e)
        {
            cShowHeight = pnFrame.Height;
            cHideHeight = pnFrame.Height + this.ucInstallationLog1.Height + 5;
        }

        public void ProgressBarEnabled(bool Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Enabled = Value;
            }
        }

        public void ProgressBarIncrementValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Increment(Value);
            }
        }

        public void ProgressBarSetValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Value = Value;
            }
        }

        public void ProgressBarSetStep(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Step = Value;
            }
        }

        public void ProgressBarPerformStep()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.PerformStep();
            }
        }

        public void ProgressBarSetMinimum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Minimum = Value;
            }
        }

        public void ProgressBarSetMaximum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Maximum = Value;
            }
        }

        public void ProgressBarSetToMaximum()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                ProgressBarSetValue(prgInstall.Maximum);
            }
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void EnterpriseLabelSetVisible(bool Value)
        {

            if (lblNotEnterprise != null &&
                !lblNotEnterprise.IsDisposed)
            {
                lblNotEnterprise.Visible = Value;
            }
        }
        // End TT#195 MD

        public void CleanupFolders(string aFolder)
        {
            DirectoryInfo di;
            DirectoryInfo folder;
            if (Directory.Exists(aFolder))
            {
                DeleteFolder(aFolder);
            }
            return;

            bool deletingFolders = true;
            while (deletingFolders)
            {
                if (Directory.Exists(aFolder))
                {
                    folder = new DirectoryInfo(aFolder);

                    // make sure to not delete Windows folders
                    if (aFolder == @"C:\Program Files" ||
                        aFolder == @"C:\Program Files (x86)" ||
                        aFolder == @"C:\" ||
                        htSystemFolder.ContainsKey(aFolder) ||
                        aFolder.Contains("Program Files") ||
                        Directory.GetParent(aFolder) == null)
                    {
                        deletingFolders = false;
                        break;
                    }
                    else
                    {
                        bool blDeleteFolder = true;
                        if (!htApplicationSubFolders.ContainsKey(folder.Name))
                        {
                            string[] directories = Directory.GetDirectories(aFolder);
                            if (directories.Length > 0)
                            {
                                foreach (string directory in directories)
                                {
                                    di = new DirectoryInfo(directory);
                                    while (di.Parent != null &&
                                        blDeleteFolder)
                                    {
                                        if (htApplicationComponentFolders.ContainsKey(di.Name))
                                        {
                                            if (Directory.Exists(di.FullName))
                                            {
                                                blDeleteFolder = false;
                                            }
                                        }

                                        di = di.Parent;
                                    }
                                    if (!blDeleteFolder)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (blDeleteFolder)
                        {
                            DeleteFolder(aFolder);
                        }
                        else
                        {
                            deletingFolders = false;
                            break;
                        }

                        if (htApplicationRootFolders.ContainsKey(folder.Name))
                        {
                            deletingFolders = false;
                        }
                    }
                }
                aFolder = Directory.GetParent(aFolder).ToString().Trim();
            }
        }

        private void picHelp_MouseClick(object sender, MouseEventArgs e)
        {
            //show help viewer
            help_view = new InstallHelpViewer();
            help_view.SetText(GetHelp(help_ID));
            help_view.ShowDialog();
        }

        public void ErrorHandler(string ErrMessage)
        {
            //log action
            ucInstallationLog1.AddLogEntry("(Client Install) " + ErrMessage, eErrorType.error);

            ucInstallationLog1.ViewLog();

            //hard kill
            
            this.Close();
            Application.Exit();
            throw new Exception("Critical Error");
        }

        //Begin TT#1205 - Verify version of sql during install - apicchetti - 3/22/2011
        private bool DBCompatibilityCheck(out string incompatibleSummary)
        {
            bool blReturn = true;

            //Array list of database info objects
            ArrayList alDBInfo = new ArrayList();

            //read the needed values from the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            string[] subkeynames = mid_key.GetSubKeyNames();

            foreach (string subkeyname in subkeynames)
            {
                if (subkeyname.StartsWith("MIDConfig") == true)
                {
                    RegistryKey sub_key = mid_key.OpenSubKey(subkeyname);
                    string MIDSettingConfig = (string)sub_key.GetValue("Location");
                    string ConnString = GetMIDSettingsDBConnectionString(MIDSettingConfig);

                    Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                    connProp.ConnectionStringBuilder.ConnectionString = ConnString;

                    SQLDatabaseInfo mid_sql = new SQLDatabaseInfo();
                    bool blCompatible = false;
                    try
                    {

                        Cursor.Current = Cursors.WaitCursor;
                        lblStatus.Text = "Checking database compatibility";
                        Application.DoEvents();
                        this.Refresh();

                        connProp.Test();

                        string sql_version = "";
                        string sql_level = "";
                        string sql_edition = "";
                        string server = "";
                        string database = "";
                        // Begin TT#74 MD - JSmith - One-button Upgrade
                        //blCompatible = VerifySQLVersion_Edition(ConnString, out sql_version, out sql_level, out sql_edition, out server, out database);
                        string databaseUser = "user";
                        string databasePwd = "pwd";
                        // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                        bool isDatabaseCompatible = false;
                        CompatibilityLevel compatibilityLevel = CompatibilityLevel.Undefined;
                        //blCompatible = VerifySQLVersion_Edition(ConnString, out sql_version, out sql_level, out sql_edition, out server, out database, out databaseUser, out databasePwd);
                        blCompatible = VerifySQLVersion_Edition(ConnString, out sql_version, out sql_level, out sql_edition, out server, out database, out databaseUser, out databasePwd, out isDatabaseCompatible, out compatibilityLevel);
                        // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                        // End TT#74 MD

                        mid_sql.OriginalConfig = MIDSettingConfig;
                        mid_sql.SQLConnectionString = ConnString;
                        mid_sql.SQLVersion = sql_version;
                        mid_sql.SQLLevel = sql_level;
                        mid_sql.SQLEdition = sql_edition;
                        mid_sql.SQLDatabaseName = database;
                        mid_sql.SQLServerName = server;
                        mid_sql.MIDCompatible = blCompatible;
                    }
                    catch
                    {
                        blCompatible = false;
                        mid_sql.OriginalConfig = MIDSettingConfig;
                        mid_sql.SQLConnectionString = ConnString;
                        mid_sql.SQLVersion = "invalid";
                        mid_sql.SQLLevel = "invalid";
                        mid_sql.SQLEdition = "invalid";
                        mid_sql.SQLDatabaseName = "invalid";
                        mid_sql.SQLServerName = "invalid";
                    }

                    alDBInfo.Add(mid_sql);
                }
                
            }

            incompatibleSummary = "";
            foreach (SQLDatabaseInfo midDB in alDBInfo)
            {
                if (midDB.MIDCompatible == false)
                {
                    blReturn = false;

                    char[] delim = ";".ToCharArray();
                    string[] connStrElements = midDB.SQLConnectionString.Split(delim);

                    string maskedConnStr = "";
                    foreach (string connStrElement in connStrElements)
                    {
                        string new_connStrElement = connStrElement.ToUpper();
                        if (new_connStrElement.StartsWith("PWD=") == true)
                        {
                            maskedConnStr = maskedConnStr + "pwd=*****";
                        }
                        else if (new_connStrElement.StartsWith("PASSWORD=") == true)
                        {
                            maskedConnStr = maskedConnStr + "Password=*****";
                        }
                        else
                        {
                            maskedConnStr = maskedConnStr + connStrElement + ";";
                        }

                    }

                    incompatibleSummary = incompatibleSummary + Environment.NewLine +
                        "Config File: " + midDB.OriginalConfig + Environment.NewLine +
                        "Connection:  " + maskedConnStr + Environment.NewLine +
                        "Server:      " + midDB.SQLServerName + Environment.NewLine +
                        "Database:    " + midDB.SQLDatabaseName + Environment.NewLine +
                        "SQL Version: " + midDB.SQLVersion + Environment.NewLine +
                        "SQL Level:   " + midDB.SQLLevel + Environment.NewLine +
                        "SQL Edition: " + midDB.SQLEdition + Environment.NewLine + Environment.NewLine ;

                }
            }


            Cursor.Current = Cursors.Default;
            lblStatus.Text = "";
            Application.DoEvents();
            this.Refresh();

            return blReturn;
        }


        private string GetMIDSettingsDBConnectionString(string MIDSettings_Location)
        {
            string DBConnString = "";

            XPathDocument doc = new XPathDocument(MIDSettings_Location);

            MIDRetail.Encryption.MIDEncryption crypt = new MIDRetail.Encryption.MIDEncryption();

            foreach (XPathNavigator child in doc.CreateNavigator().Select("appSettings/*"))
            {
                if (child.LocalName == "add")
                {
                    child.MoveToFirstAttribute();           //move to the key attribute
                    if (child.Value == "ConnectionString")
                    {
                        child.MoveToNextAttribute();        //move to the value attribute

                        DBConnString = crypt.Decrypt(child.Value);
                    }
                }
            }

            return DBConnString;
        }

        // Begin TT#2131-MD - JSmith - Halo Integration
        private string GetMIDSettingsROExtractDBConnectionString(string MIDSettings_Location)
        {
            string DBConnString = "";

            XPathDocument doc = new XPathDocument(MIDSettings_Location);

            MIDRetail.Encryption.MIDEncryption crypt = new MIDRetail.Encryption.MIDEncryption();

            foreach (XPathNavigator child in doc.CreateNavigator().Select("appSettings/*"))
            {
                if (child.LocalName == "add")
                {
                    child.MoveToFirstAttribute();           //move to the key attribute
                    if (child.Value == "ROExtractConnectionString")
                    {
                        child.MoveToNextAttribute();        //move to the value attribute

                        DBConnString = crypt.Decrypt(child.Value);
                    }
                }
            }

            return DBConnString;
        }
        // End TT#2131-MD - JSmith - Halo Integration

        // Begin TT#74 MD - JSmith - One-button Upgrade
        // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
        //public bool VerifySQLVersion_Edition(string strConn, out string productversion, out string productlevel, out string edition, out string server, out string database)
        //public bool VerifySQLVersion_Edition(string strConn, out string productversion, out string productlevel, out string edition, out string server, out string database, out string user, out string password)
        public bool VerifySQLVersion_Edition(string strConn, out string productversion, out string productlevel, out string edition, out string server, out string database, out string user, out string password, out bool isDatabaseCompatible, out CompatibilityLevel compatibilityLevel)
        // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
        // End TT#74 MD
        {

            //return value init
            bool blReturn = false;
			// Begin TT#74 MD - JSmith - One-button Upgrade
            productversion = "Unknown";
            productlevel = "Unknown";
            edition = "Unknown";
            server = "Unknown";
            database = "Unknown";
            user = "Unknown";
            password = "Unknown"; 
			// End TT#74 MD
            isDatabaseCompatible = true;  // TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
            compatibilityLevel = CompatibilityLevel.Undefined;    // TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install

            try
            {
                //get the version, level and edition from the target database
                SqlConnection sql = new SqlConnection(strConn);
                sql.Open();
                string sql_query = "SELECT  SERVERPROPERTY('productversion'), SERVERPROPERTY ('productlevel'), SERVERPROPERTY ('edition')";
                SqlCommand cmd = new SqlCommand(sql_query, sql);
                SqlDataReader read = cmd.ExecuteReader();
                read.Read();
                productversion = read[0].ToString().Trim(); //return sql version to user
                productlevel = read[1].ToString().Trim();   //return sql level to user
                edition = read[2].ToString().Trim();        //return sql edition to user
                server = sql.DataSource;
                database = sql.Database;
                // Begin TT#74 MD - JSmith - One-button Upgrade
                user = "user";
                password = "pwd";
                // End TT#74 MD

                // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                cmd.Dispose();
                read.Close();
                sql_query = "SELECT compatibility_level FROM sys.databases WHERE name ='" + database + "'";
                cmd = new SqlCommand(sql_query, sql);
                read = cmd.ExecuteReader();
                read.Read();
                if (read.FieldCount == 0)
                {
                    compatibilityLevel = CompatibilityLevel.Undefined;
                }
                else if (read[0] == DBNull.Value)
                {
                    compatibilityLevel = CompatibilityLevel.Undefined;
                }
                else
                {
                    compatibilityLevel = (CompatibilityLevel)Convert.ToInt32(read[0]);
                }
                // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install

                cmd.Dispose();
                read.Close();
                sql.Close();

                //check value inits
                bool productversion_check = false;
                bool edition_check = false;

                //verify the target database version to the required information (stored in the installer config)
                char[] delim = ";".ToCharArray();
                string[] CompatibleSQLVersions = ConfigurationManager.AppSettings["SupportedSQLVersions"].ToString().Split(delim);
                foreach (string CompatibleSQLVersion in CompatibleSQLVersions)
                {
                    if (productversion.StartsWith(CompatibleSQLVersion) == true)
                    {
                        productversion_check = true;
                    }
                }

                //verify the target database edition to the required information (stored in the installer config)
                string[] CompatibleSQLEditions = ConfigurationManager.AppSettings["SupportedSQLEditions"].ToString().Split(delim);
                foreach (string CompatibleSQLEdition in CompatibleSQLEditions)
                {
                    if (productversion.Contains(CompatibleSQLEdition) == true)
                    {
                        edition_check = true;
                    }
                }

                //if the version and edition check out return true else leave the initial vale or false
                if (productversion_check == true || edition_check == true)
                {
                    // Begin TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                    //blReturn = true;
                    // verify compatibility level of database
                    if (compatibilityLevel < CompatibilityLevel.SQL2008)
                    {
                        isDatabaseCompatible = false;
                        blReturn = false;
                    }
                    else
                    {
                        blReturn = true;
                    }
                    // End TT#3506 - JSmith - Change One Click install to verify SQL Server and Database Compatibility Level before starting install
                }

                // Begin TT#1164 - JSmith - Database compatibility issue when installing 
                string connectionUser = "Unknown";
                Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                try
                {
                    connProp.ConnectionStringBuilder.ConnectionString = strConn;
                    connectionUser = Convert.ToString(connProp.ConnectionStringBuilder["User"]);
                    connProp.ConnectionStringBuilder["Password"] = "********";
                }
                catch
                {
                    connectionUser = "Unknown";
                }
                SetLogMessage(GetText("SQLConnectionString").Replace("{0}", connProp.ConnectionStringBuilder.ConnectionString), eErrorType.message);
                SetLogMessage(GetText("SQLServerSpecifications"), eErrorType.message);
                SetLogMessage(GetText("SQLServerServer").Replace("{0}", server), eErrorType.message);
                SetLogMessage(GetText("SQLServerProductVersion").Replace("{0}", productversion), eErrorType.message);
                SetLogMessage(GetText("SQLServerProductLevel").Replace("{0}", productlevel), eErrorType.message);
                SetLogMessage(GetText("SQLServerEdition").Replace("{0}", edition), eErrorType.message);
                SetLogMessage(GetText("SQLServerCompatibilityLevel").Replace("{0}", compatibilityLevel.ToString()), eErrorType.message);
                SetLogMessage(GetText("SQLServerDatabaseName").Replace("{0}", database), eErrorType.message);
                SetLogMessage(GetText("SQLServerUserName").Replace("{0}", connectionUser), eErrorType.message);
                // End TT#1164 - JSmith - Database compatibility issue when installing
            }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            catch (Exception ex)
            {
                SetLogMessage(ex.Message, eErrorType.error);
                blReturn = false;
            }
			// End TT#74 MD

            //return method value
            return blReturn;
        }

        // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
        public void RefreshDesktop()
        {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }
        // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization

        // Begin TT#195 MD - JSmith - Add environment authentication
        private void GetDatabaseConfiguration(string strConn, out string configuration)
        {

            //return value init
            configuration = null;

            try
            {
                //get the configuration from the target database
                SqlConnection sql = new SqlConnection(strConn);
                sql.Open();
                string sql_query = "if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'APPLICATION_UPGRADE_HISTORY')"
                + " select top 1 UPGRADE_CONFIGURATION from APPLICATION_UPGRADE_HISTORY order by UPGRADE_RID desc";
                SqlCommand cmd = new SqlCommand(sql_query, sql);
                SqlDataReader read = cmd.ExecuteReader();
                read.Read();
                if (read.HasRows)
                {
                    configuration = read[0].ToString().Trim(); //return configuration to user
                }
                cmd.Dispose();
                read.Close();
                sql.Close();
                
            }
            catch (Exception ex)
            {
                SetLogMessage(ex.Message, eErrorType.error);
            }
        }
        // End TT#195 MD

        // Begin TT#1668 - JSmith - Install Log
        private void InstallerFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetLogMessage("********************************************************************", eErrorType.message);
            SetLogMessage("**                                                                **", eErrorType.message);
            SetLogMessage("**                 Installation Utility Ended                     **", eErrorType.message);
            SetLogMessage("**                                                                **", eErrorType.message);
            SetLogMessage("********************************************************************", eErrorType.message);
        }
        // End TT#1668

        [System.Runtime.InteropServices.DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
        }

        private static string GetTerminalServerClientNameWTSAPI()
        {
            const int WTS_CURRENT_SERVER_HANDLE = -1;

            IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;

            string strReturnValue = "";
            try
            {
                WTSQuerySessionInformation(IntPtr.Zero, WTS_CURRENT_SERVER_HANDLE, WTS_INFO_CLASS.WTSClientName, out buffer, out bytesReturned);
                strReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(buffer);
            }

            finally
            {
                buffer = IntPtr.Zero;
            }

            return strReturnValue;
        }
    }

    public class SQLDatabaseInfo
    {
        private string strConfigFile;
        public string OriginalConfig
        {
            get
            {
                return strConfigFile;
            }
            set
            {
                strConfigFile = value;
            }

        }

        private string strConnStr;
        public string SQLConnectionString
        {
            get
            {
                return strConnStr;
            }
            set
            {
                strConnStr = value;
            }
        }

        private string strServer;
        public string SQLServerName
        {
            get
            {
                return strServer;
            }
            set
            {
                strServer = value;
            }
        }

        private string strDBName;
        public string SQLDatabaseName
        {
            get
            {
                return strDBName;
            }
            set
            {
                strDBName = value;
            }
        }
        
        private string strSQLVersion;
        public string SQLVersion
        {
            get
            {
                return strSQLVersion;
            }
            set
            {
                strSQLVersion = value;
            }
        }

        private string strSQLLevel;
        public string SQLLevel
        {
            get
            {
                return strSQLLevel;
            }
            set
            {
                strSQLLevel = value;
            }
        }

        private string strSQLEdition;
        public string SQLEdition
        {
            get
            {
                return strSQLEdition;
            }
            set
            {
                strSQLEdition = value;
            }
        }

        private bool blMIDCompatible;
        public bool MIDCompatible
        {
            get
            {
                return blMIDCompatible;
            }
            set
            {
                blMIDCompatible = value;
            }
        }

        //End TT#1205 - Verify version of sql during install - apicchetti - 3/22/2011
    }

    // Begin TT#1668 - JSmith - Install Log
    /// <summary>
    /// Provides detailed information about the host operating system.
    /// </summary>
    static public class OSInfo
    {
        // Begin TT#1481 - JSmith - Framework version not correct
        private const string FRAMEWORK_PATH = "\\Microsoft.NET\\Framework";
        private const string WINDIR1 = "windir";
        private const string WINDIR2 = "SystemRoot";
        // End TT#1481

        #region BITS
        /// <summary>
        /// Determines if the current application is 32 or 64-bit.
        /// </summary>
        static public int Bits
        {
            get
            {
                return IntPtr.Size * 8;
            }
        }
        #endregion BITS

        #region EDITION
        static private string s_Edition;
        /// <summary>
        /// Gets the edition of the operating system running on this computer.
        /// </summary>
        static public string Edition
        {
            get
            {
                if (s_Edition != null)
                    return s_Edition;  //***** RETURN *****//

                string edition = String.Empty;

                OperatingSystem osVersion = Environment.OSVersion;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;
                    byte productType = osVersionInfo.wProductType;
                    short suiteMask = osVersionInfo.wSuiteMask;

                    #region VERSION 4
                    if (majorVersion == 4)
                    {
                        if (productType == VER_NT_WORKSTATION)
                        {
                            // Windows NT 4.0 Workstation
                            edition = "Workstation";
                        }
                        else if (productType == VER_NT_SERVER)
                        {
                            if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                            {
                                // Windows NT 4.0 Server Enterprise
                                edition = "Enterprise Server";
                            }
                            else
                            {
                                // Windows NT 4.0 Server
                                edition = "Standard Server";
                            }
                        }
                    }
                    #endregion VERSION 4

                    #region VERSION 5
                    else if (majorVersion == 5)
                    {
                        if (productType == VER_NT_WORKSTATION)
                        {
                            if ((suiteMask & VER_SUITE_PERSONAL) != 0)
                            {
                                // Windows XP Home Edition
                                edition = "Home";
                            }
                            else
                            {
                                // Windows XP / Windows 2000 Professional
                                edition = "Professional";
                            }
                        }
                        else if (productType == VER_NT_SERVER)
                        {
                            if (minorVersion == 0)
                            {
                                if ((suiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows 2000 Datacenter Server
                                    edition = "Datacenter Server";
                                }
                                else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows 2000 Advanced Server
                                    edition = "Advanced Server";
                                }
                                else
                                {
                                    // Windows 2000 Server
                                    edition = "Server";
                                }
                            }
                            else
                            {
                                if ((suiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows Server 2003 Datacenter Edition
                                    edition = "Datacenter";
                                }
                                else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows Server 2003 Enterprise Edition
                                    edition = "Enterprise";
                                }
                                else if ((suiteMask & VER_SUITE_BLADE) != 0)
                                {
                                    // Windows Server 2003 Web Edition
                                    edition = "Web Edition";
                                }
                                else
                                {
                                    // Windows Server 2003 Standard Edition
                                    edition = "Standard";
                                }
                            }
                        }
                    }
                    #endregion VERSION 5

                    #region VERSION 6
                    else if (majorVersion == 6)
                    {
                        int ed;
                        if (GetProductInfo(majorVersion, minorVersion,
                            osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor,
                            out ed))
                        {
                            switch (ed)
                            {
                                case PRODUCT_BUSINESS:
                                    edition = "Business";
                                    break;
                                case PRODUCT_BUSINESS_N:
                                    edition = "Business N";
                                    break;
                                case PRODUCT_CLUSTER_SERVER:
                                    edition = "HPC Edition";
                                    break;
                                case PRODUCT_CLUSTER_SERVER_V:
                                    edition = "HPC Edition without Hyper-V";
                                    break;
                                case PRODUCT_DATACENTER_SERVER:
                                    edition = "Datacenter Server";
                                    break;
                                case PRODUCT_DATACENTER_SERVER_CORE:
                                    edition = "Datacenter Server (core installation)";
                                    break;
                                case PRODUCT_DATACENTER_SERVER_V:
                                    edition = "Datacenter Server without Hyper-V";
                                    break;
                                case PRODUCT_DATACENTER_SERVER_CORE_V:
                                    edition = "Datacenter Server without Hyper-V (core installation)";
                                    break;
                                case PRODUCT_EMBEDDED:
                                    edition = "Embedded";
                                    break;
                                case PRODUCT_ENTERPRISE:
                                    edition = "Enterprise";
                                    break;
                                case PRODUCT_ENTERPRISE_N:
                                    edition = "Enterprise N";
                                    break;
                                case PRODUCT_ENTERPRISE_E:
                                    edition = "Enterprise E";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER:
                                    edition = "Enterprise Server";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE:
                                    edition = "Enterprise Server (core installation)";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE_V:
                                    edition = "Enterprise Server without Hyper-V (core installation)";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_IA64:
                                    edition = "Enterprise Server for Itanium-based Systems";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_V:
                                    edition = "Enterprise Server without Hyper-V";
                                    break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT:
                                    edition = "Essential Business Server MGMT";
                                    break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL:
                                    edition = "Essential Business Server ADDL";
                                    break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC:
                                    edition = "Essential Business Server MGMTSVC";
                                    break;
                                case PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC:
                                    edition = "Essential Business Server ADDLSVC";
                                    break;
                                case PRODUCT_HOME_BASIC:
                                    edition = "Home Basic";
                                    break;
                                case PRODUCT_HOME_BASIC_N:
                                    edition = "Home Basic N";
                                    break;
                                case PRODUCT_HOME_BASIC_E:
                                    edition = "Home Basic E";
                                    break;
                                case PRODUCT_HOME_PREMIUM:
                                    edition = "Home Premium";
                                    break;
                                case PRODUCT_HOME_PREMIUM_N:
                                    edition = "Home Premium N";
                                    break;
                                case PRODUCT_HOME_PREMIUM_E:
                                    edition = "Home Premium E";
                                    break;
                                case PRODUCT_HOME_PREMIUM_SERVER:
                                    edition = "Home Premium Server";
                                    break;
                                case PRODUCT_HYPERV:
                                    edition = "Microsoft Hyper-V Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT:
                                    edition = "Windows Essential Business Management Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING:
                                    edition = "Windows Essential Business Messaging Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY:
                                    edition = "Windows Essential Business Security Server";
                                    break;
                                case PRODUCT_PROFESSIONAL:
                                    edition = "Professional";
                                    break;
                                case PRODUCT_PROFESSIONAL_N:
                                    edition = "Professional N";
                                    break;
                                case PRODUCT_PROFESSIONAL_E:
                                    edition = "Professional E";
                                    break;
                                case PRODUCT_SB_SOLUTION_SERVER:
                                    edition = "SB Solution Server";
                                    break;
                                case PRODUCT_SB_SOLUTION_SERVER_EM:
                                    edition = "SB Solution Server EM";
                                    break;
                                case PRODUCT_SERVER_FOR_SB_SOLUTIONS:
                                    edition = "Server for SB Solutions";
                                    break;
                                case PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM:
                                    edition = "Server for SB Solutions EM";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS:
                                    edition = "Windows Essential Server Solutions";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS_V:
                                    edition = "Windows Essential Server Solutions without Hyper-V";
                                    break;
                                case PRODUCT_SERVER_FOUNDATION:
                                    edition = "Server Foundation";
                                    break;
                                case PRODUCT_SMALLBUSINESS_SERVER:
                                    edition = "Windows Small Business Server";
                                    break;
                                case PRODUCT_SMALLBUSINESS_SERVER_PREMIUM:
                                    edition = "Windows Small Business Server Premium";
                                    break;
                                case PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE:
                                    edition = "Windows Small Business Server Premium (core installation)";
                                    break;
                                case PRODUCT_SOLUTION_EMBEDDEDSERVER:
                                    edition = "Solution Embedded Server";
                                    break;
                                case PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE:
                                    edition = "Solution Embedded Server (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER:
                                    edition = "Standard Server";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE:
                                    edition = "Standard Server (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER_SOLUTIONS:
                                    edition = "Standard Server Solutions";
                                    break;
                                case PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE:
                                    edition = "Standard Server Solutions (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE_V:
                                    edition = "Standard Server without Hyper-V (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER_V:
                                    edition = "Standard Server without Hyper-V";
                                    break;
                                case PRODUCT_STARTER:
                                    edition = "Starter";
                                    break;
                                case PRODUCT_STARTER_N:
                                    edition = "Starter N";
                                    break;
                                case PRODUCT_STARTER_E:
                                    edition = "Starter E";
                                    break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER:
                                    edition = "Enterprise Storage Server";
                                    break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE:
                                    edition = "Enterprise Storage Server (core installation)";
                                    break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER:
                                    edition = "Express Storage Server";
                                    break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER_CORE:
                                    edition = "Express Storage Server (core installation)";
                                    break;
                                case PRODUCT_STORAGE_STANDARD_SERVER:
                                    edition = "Standard Storage Server";
                                    break;
                                case PRODUCT_STORAGE_STANDARD_SERVER_CORE:
                                    edition = "Standard Storage Server (core installation)";
                                    break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER:
                                    edition = "Workgroup Storage Server";
                                    break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER_CORE:
                                    edition = "Workgroup Storage Server (core installation)";
                                    break;
                                case PRODUCT_UNDEFINED:
                                    edition = "Unknown product";
                                    break;
                                case PRODUCT_ULTIMATE:
                                    edition = "Ultimate";
                                    break;
                                case PRODUCT_ULTIMATE_N:
                                    edition = "Ultimate N";
                                    break;
                                case PRODUCT_ULTIMATE_E:
                                    edition = "Ultimate E";
                                    break;
                                case PRODUCT_WEB_SERVER:
                                    edition = "Web Server";
                                    break;
                                case PRODUCT_WEB_SERVER_CORE:
                                    edition = "Web Server (core installation)";
                                    break;
                            }
                        }
                    }
                    #endregion VERSION 6
                }

                s_Edition = edition;
                return edition;
            }
        }
        #endregion EDITION

        #region NAME
        static private string s_Name;
        /// <summary>
        /// Gets the name of the operating system running on this computer.
        /// </summary>
        static public string Name
        {
            get
            {
                if (s_Name != null)
                    return s_Name;  //***** RETURN *****//

                string name = "unknown";

                OperatingSystem osVersion = Environment.OSVersion;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;

                    switch (osVersion.Platform)
                    {
                        case PlatformID.Win32Windows:
                            {
                                if (majorVersion == 4)
                                {
                                    string csdVersion = osVersionInfo.szCSDVersion;
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            if (csdVersion == "B" || csdVersion == "C")
                                                name = "Windows 95 OSR2";
                                            else
                                                name = "Windows 95";
                                            break;
                                        case 10:
                                            if (csdVersion == "A")
                                                name = "Windows 98 Second Edition";
                                            else
                                                name = "Windows 98";
                                            break;
                                        case 90:
                                            name = "Windows Me";
                                            break;
                                    }
                                }
                                break;
                            }

                        case PlatformID.Win32NT:
                            {
                                byte productType = osVersionInfo.wProductType;

                                switch (majorVersion)
                                {
                                    case 3:
                                        name = "Windows NT 3.51";
                                        break;
                                    case 4:
                                        switch (productType)
                                        {
                                            case 1:
                                                name = "Windows NT 4.0";
                                                break;
                                            case 3:
                                                name = "Windows NT 4.0 Server";
                                                break;
                                        }
                                        break;
                                    case 5:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                name = "Windows 2000";
                                                break;
                                            case 1:
                                                name = "Windows XP";
                                                break;
                                            case 2:
                                                name = "Windows Server 2003";
                                                break;
                                        }
                                        break;
                                    case 6:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows Vista";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2008";
                                                        break;
                                                }
                                                break;
                                            case 1:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 7";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2008 R2";
                                                        break;
                                                }
                                                break;
                                            // Begin TT#668-MD - JSmith - Windows 8 - Installer issues
                                            case 2:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 8";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2012";
                                                        break;
                                                }
                                                break;
                                            // End TT#668-MD - JSmith - Windows 8 - Installer issues
                                        }

                                        break;
                                }
                                break;
                            }
                    }
                }

                s_Name = name;
                return name;
            }
        }
        #endregion NAME

        #region PINVOKE
        #region GET
        #region PRODUCT INFO
        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(
            int osMajorVersion,
            int osMinorVersion,
            int spMajorVersion,
            int spMinorVersion,
            out int edition);
        #endregion PRODUCT INFO

        #region VERSION
        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);
        #endregion VERSION
        #endregion GET

        #region OSVERSIONINFOEX
        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
        #endregion OSVERSIONINFOEX

        #region PRODUCT
        private const int PRODUCT_UNDEFINED = 0x00000000;
        private const int PRODUCT_ULTIMATE = 0x00000001;
        private const int PRODUCT_HOME_BASIC = 0x00000002;
        private const int PRODUCT_HOME_PREMIUM = 0x00000003;
        private const int PRODUCT_ENTERPRISE = 0x00000004;
        private const int PRODUCT_HOME_BASIC_N = 0x00000005;
        private const int PRODUCT_BUSINESS = 0x00000006;
        private const int PRODUCT_STANDARD_SERVER = 0x00000007;
        private const int PRODUCT_DATACENTER_SERVER = 0x00000008;
        private const int PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        private const int PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        private const int PRODUCT_STARTER = 0x0000000B;
        private const int PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        private const int PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        private const int PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        private const int PRODUCT_BUSINESS_N = 0x00000010;
        private const int PRODUCT_WEB_SERVER = 0x00000011;
        private const int PRODUCT_CLUSTER_SERVER = 0x00000012;
        private const int PRODUCT_HOME_SERVER = 0x00000013;
        private const int PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        private const int PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        private const int PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        private const int PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        private const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        private const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        private const int PRODUCT_ENTERPRISE_N = 0x0000001B;
        private const int PRODUCT_ULTIMATE_N = 0x0000001C;
        private const int PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        private const int PRODUCT_SERVER_FOUNDATION = 0x00000021;
        private const int PRODUCT_HOME_PREMIUM_SERVER = 0x00000022;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        private const int PRODUCT_STANDARD_SERVER_V = 0x00000024;
        private const int PRODUCT_DATACENTER_SERVER_V = 0x00000025;
        private const int PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        private const int PRODUCT_DATACENTER_SERVER_CORE_V = 0x00000027;
        private const int PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        private const int PRODUCT_HYPERV = 0x0000002A;
        private const int PRODUCT_STORAGE_EXPRESS_SERVER_CORE = 0x0000002B;
        private const int PRODUCT_STORAGE_STANDARD_SERVER_CORE = 0x0000002C;
        private const int PRODUCT_STORAGE_WORKGROUP_SERVER_CORE = 0x0000002D;
        private const int PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE = 0x0000002E;
        private const int PRODUCT_STARTER_N = 0x0000002F;
        private const int PRODUCT_PROFESSIONAL = 0x00000030;
        private const int PRODUCT_PROFESSIONAL_N = 0x00000031;
        private const int PRODUCT_SB_SOLUTION_SERVER = 0x00000032;
        private const int PRODUCT_SERVER_FOR_SB_SOLUTIONS = 0x00000033;
        private const int PRODUCT_STANDARD_SERVER_SOLUTIONS = 0x00000034;
        private const int PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE = 0x00000035;
        private const int PRODUCT_SB_SOLUTION_SERVER_EM = 0x00000036;
        private const int PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM = 0x00000037;
        private const int PRODUCT_SOLUTION_EMBEDDEDSERVER = 0x00000038;
        private const int PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE = 0x00000039;
        //private const int ???? = 0x0000003A;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT = 0x0000003B;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL = 0x0000003C;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC = 0x0000003D;
        private const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC = 0x0000003E;
        private const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE = 0x0000003F;
        private const int PRODUCT_CLUSTER_SERVER_V = 0x00000040;
        private const int PRODUCT_EMBEDDED = 0x00000041;
        private const int PRODUCT_STARTER_E = 0x00000042;
        private const int PRODUCT_HOME_BASIC_E = 0x00000043;
        private const int PRODUCT_HOME_PREMIUM_E = 0x00000044;
        private const int PRODUCT_PROFESSIONAL_E = 0x00000045;
        private const int PRODUCT_ENTERPRISE_E = 0x00000046;
        private const int PRODUCT_ULTIMATE_E = 0x00000047;
        #endregion PRODUCT

        #region VERSIONS
        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;
        #endregion VERSIONS
        #endregion PINVOKE

        #region SERVICE PACK
        /// <summary>
        /// Gets the service pack information of the operating system running on this computer.
        /// </summary>
        static public string ServicePack
        {
            get
            {
                string servicePack = String.Empty;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    servicePack = osVersionInfo.szCSDVersion;
                }

                return servicePack;
            }
        }
        #endregion SERVICE PACK

        #region VERSION
        #region BUILD
        /// <summary>
        /// Gets the build version number of the operating system running on this computer.
        /// </summary>
        static public int BuildVersion
        {
            get
            {
                return Environment.OSVersion.Version.Build;
            }
        }
        #endregion BUILD

        #region FULL
        #region STRING
        /// <summary>
        /// Gets the full version string of the operating system running on this computer.
        /// </summary>
        static public string VersionString
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }
        #endregion STRING

        #region VERSION
        /// <summary>
        /// Gets the full version of the operating system running on this computer.
        /// </summary>
        static public Version Version
        {
            get
            {
                return Environment.OSVersion.Version;
            }
        }
        #endregion VERSION
        #endregion FULL

        #region MAJOR
        /// <summary>
        /// Gets the major version number of the operating system running on this computer.
        /// </summary>
        static public int MajorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Major;
            }
        }
        #endregion MAJOR

        #region MINOR
        /// <summary>
        /// Gets the minor version number of the operating system running on this computer.
        /// </summary>
        static public int MinorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Minor;
            }
        }
        #endregion MINOR

        #region REVISION
        /// <summary>
        /// Gets the revision version number of the operating system running on this computer.
        /// </summary>
        static public int RevisionVersion
        {
            get
            {
                return Environment.OSVersion.Version.Revision;
            }
        }
        #endregion REVISION
        #endregion VERSION

        // Begin TT#1481 - JSmith - Framework version not correct
        #region FRAMEWORK
        public static string FrameworkVersion
        {
            get
            {
                try
                {
                    return getHighestVersion(NetFrameworkInstallationPath);
                }
                catch (SecurityException)
                {
                    return "Unknown";
                }
            }
        }

        private static string getHighestVersion(string installationPath)
        {
            string[] versions = Directory.GetDirectories(installationPath, "v*");
            string version = "Unknown";

            for (int i = versions.Length - 1; i >= 0; i--)
            {
                version = extractVersion(versions[i]);
                if (isNumber(version))
                    return version;
            }

            return version;
        }

        private static string extractVersion(string directory)
        {
            int startIndex = directory.LastIndexOf("\\") + 2;
            return directory.Substring(startIndex, directory.Length - startIndex);
        }

        private static bool isNumber(string str)
        {
            //return new Regex(@"^[0-9]+\.?[0-9]*$").IsMatch(str);
            return new Regex(@"^[0-9]+\.?[0-9]*").IsMatch(str);
        }

        public static string NetFrameworkInstallationPath
        {
            get { return WindowsPath + FRAMEWORK_PATH; }
        }

        public static string WindowsPath
        {
            get
            {
                string winDir = Environment.GetEnvironmentVariable(WINDIR1);
                if (String.IsNullOrEmpty(winDir))
                    winDir = Environment.GetEnvironmentVariable(WINDIR2);

                return winDir;
            }
        }

        #endregion FRAMEWORK
        // End TT#1481
    }
	// End TT#1668
}
