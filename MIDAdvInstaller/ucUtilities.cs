using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;		// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
using Microsoft.Win32;

namespace MIDRetailInstaller
{
    public partial class ucUtilities : UserControl
    {
        ToolTip tt = new ToolTip();

        //public events
        public event EventHandler ConfigureNext;
        public event EventHandler NotConfigureNext;

        //object to pass frame to
        InstallerFrame frame = null;

        //inventory list
        //List<string> installedFiles;
        //bool UninstallConfigs = false;
        Hashtable htInstalledServices = null;
        Hashtable htServicesLocation;

        string ConfigurationFileName;

        public ucUtilities()
        {
            InitializeComponent();
        }

        ucInstallationLog log;

        public ucUtilities(InstallerFrame p_frame, ucInstallationLog p_log, bool blNeedsScanned, bool blForDisplay)
        {
            InitializeComponent();

            //pass the installer frame here
            frame = p_frame;
            frame.help_ID = "utilities";
            log = p_log;

            tt.SetToolTip(rdoDatabaseMaintenance, frame.GetToolTipText("util_databasemaint"));
            tt.SetToolTip(rdoStartServices, frame.GetToolTipText("util_startservices"));
            tt.SetToolTip(rdoStopServices, frame.GetToolTipText("util_stopservices"));
            tt.SetToolTip(rdoRescan, frame.GetToolTipText("util_rescan"));
            tt.SetToolTip(rdoEventSource, frame.GetToolTipText("util_eventSources"));
            tt.SetToolTip(rdoCrystalReports, frame.GetToolTipText("util_crystalReports"));


            rdoDatabaseMaintenance.Text = frame.GetText("rdoDatabaseMaintenance");
            rdoStartServices.Text = frame.GetText("rdoStartServices");
            rdoStopServices.Text = frame.GetText("rdoStopServices");
            rdoRescan.Text = frame.GetText("rdoRescan");
            rdoEventSource.Text = frame.GetText("rdoEventSource");
            rdoCrystalReports.Text = frame.GetText("rdoCrystalReports");

            ConfigurationFileName = ConfigurationManager.AppSettings["MIDSettings_config"].ToString();
            GetInstalledServices();

            if (blForDisplay)
            {
                if (blNeedsScanned)
                {
                    rdoRescan.Checked = true;
                    rdoDatabaseMaintenance.Enabled = false;
                    rdoStartServices.Enabled = false;
                    rdoStopServices.Enabled = false;
                }
                else
                {
                    rdoDatabaseMaintenance.Checked = true;
                    if (htInstalledServices.Count == 0)
                    {
                        rdoStartServices.Enabled = false;
                        rdoStopServices.Enabled = false;
                    }
                }

                if (IsCrystalReportsInstalled(log))
                {
                    rdoCrystalReports.Enabled = false;
                }
            }
        }

        //database maintenance task
        public bool DatabaseMaintenance
        {
            get
            {
                return rdoDatabaseMaintenance.Checked;
            }
        }

        //start services task
        public bool StartServices
        {
            get
            {
                return rdoStartServices.Checked;
            }
        }

        //stop services task
        public bool StopServices
        {
            get
            {
                return rdoStopServices.Checked;
            }
        }

        //rescan task
        public bool Rescan
        {
            get
            {
                return rdoRescan.Checked;
            }
        }

        public int ServicesCount
        {
            get
            {
                if (htInstalledServices == null)
                {
                    return 0;
                }
                return htInstalledServices.Count;
            }
        }

        private void GetInstalledServices()
        {
            //return variable
            List<string> lstReturn = new List<string>();
            htInstalledServices = new Hashtable();
            htServicesLocation = new Hashtable();

            //drill down into the registry to our stuff
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey);
            RegistryKey mid_key = soft_key.OpenSubKey(InstallerConstants.cRegistryRootKey);
            if (mid_key == null)
            {
                return;
            }
            string[] SubKeyNames = mid_key.GetSubKeyNames();

            //loop thru the subkeys
            foreach (string SubKeyName in SubKeyNames)
            {
                if (SubKeyName.Contains(InstallerConstants.cClientKey) == false)
                {
                    //open the client key
                    RegistryKey sub_key = mid_key.OpenSubKey(SubKeyName);

                    if (sub_key.GetValue("Location").ToString().EndsWith(".config") == false)
                    {
                        //get the location
                        string location = sub_key.GetValue("Location").ToString().Trim();
                        string[] ArrValues = sub_key.Name.Split('\\');
                        if (ArrValues[ArrValues.Length - 1] != InstallerConstants.cBatchKey)
                        {
                            string[] LocationValues = location.Split('\\');
                            string[] ExecValues = LocationValues[LocationValues.Length - 1].Split('.');
                            htInstalledServices.Add(ArrValues[ArrValues.Length - 1], ExecValues[0]);
                            htServicesLocation.Add(ExecValues[0], location);
                        }
                    }
                }
            }
        }

        public bool DoStartServices()
        {
            bool bSuccessful = true;
            ArrayList alServices;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                alServices = new ArrayList();
                string ServiceLabel;
                log.AddLogEntry("User has requested to start the services", eErrorType.message);
                frame.SetStatusMessage("Starting the services ");
                System.Threading.Thread.Sleep(2000);

                // start control service first and scheduler last
                foreach (DictionaryEntry Service in htInstalledServices)
                {
                    ServiceLabel = (string)Service.Key;
                    string ServiceName = (string)Service.Value;
                    if (ServiceLabel.ToUpper().IndexOf("CONTROL") > -1 ||
                        alServices.Count == 0)
                    {
                        alServices.Insert(0, ServiceName);
                    }
                    else if (ServiceLabel.ToUpper().IndexOf("SCHEDULE") > -1)
                    {
                        alServices.Insert(alServices.Count, ServiceName);
                    }
                    else
                    {
                        alServices.Insert(alServices.Count - 1, ServiceName);
                    }
                }

                foreach (string ServiceName in alServices)
                {
                    //ServiceLabel = (string)Service.Key;
                    //string ServiceName = (string)Service.Value;
                    if (!frame.StartService(ServiceName))
                    {
                        bSuccessful = false;
                    }
                    frame.ProgressBarIncrementValue(1);
                }
                frame.SetStatusMessage("Start issued for all services ");
            }
            catch { throw; }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return bSuccessful;
        }

        // Begin TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.
        public bool DoStartServices(ArrayList alServices)
        {
            bool bSuccessful = true;
            try
            {
                this.Cursor = Cursors.WaitCursor;


                foreach (string ServiceName in alServices)
                {

                    if (!frame.StartService(ServiceName))
                    {
                        bSuccessful = false;
                    }
                }
                frame.SetStatusMessage("Start issued for services ");
            }
            catch { throw; }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return bSuccessful;
        }
        // End TT#3633 - JSmith - All Services restart after the One-Click upgrade - does not restart only the services that were originally started/running.

        public bool DoStopServices()
        {
            bool bSuccessful = true;
            ArrayList alServices;
            string ServiceLabel;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                alServices = new ArrayList();
                log.AddLogEntry("User has requested to stop the services", eErrorType.message);
                frame.SetStatusMessage("Stopping the services ");
                System.Threading.Thread.Sleep(2000);

                // stop scheduler first and control service last
                foreach (DictionaryEntry Service in htInstalledServices)
                {
                    ServiceLabel = (string)Service.Key;
                    string ServiceName = (string)Service.Value;
                    if (ServiceLabel.ToUpper().IndexOf("CONTROL") > -1 ||
                        alServices.Count == 0)
                    {
                        alServices.Insert(alServices.Count, ServiceName);
                    }
                    else if (ServiceLabel.ToUpper().IndexOf("SCHEDULE") > -1)
                    {
                        alServices.Insert(0, ServiceName);
                    }
                    else
                    {
                        alServices.Insert(alServices.Count - 1, ServiceName);
                    }
                }

                foreach (string ServiceName in alServices)
                {
                    //string ServiceName = (string)Service.Value;
                    frame.StopService(ServiceName, false);
                    string location = (string)htServicesLocation[ServiceName];
                    int count = 0;
                    while (frame.IsFileLocked(location))
                    {
                        ++count;
                        if (count > 10)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                    frame.ProgressBarIncrementValue(1);
                }
                frame.SetStatusMessage("Stop issued for all services ");
                
            }
            catch { throw; }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return bSuccessful;
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        //public bool DoDatabaseMaintenance()
        public bool DoDatabaseMaintenance()
        {
            string aConnectionString = null;
            // Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
#if (DEBUG)
            aConnectionString = GetMIDSettingsDBConnectionString(Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DataCommon\MIDSettings.config");
#else
            //read the needed values from the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey mid_key = local_key.OpenSubKey(InstallerConstants.cRegistryMIDSoftwareKey);
            if (mid_key != null)
            {
                string[] subkeynames = mid_key.GetSubKeyNames();

                foreach (string subkeyname in subkeynames)
                {
                    if (subkeyname.StartsWith("MIDConfig") == true)
                    {
                        RegistryKey sub_key = mid_key.OpenSubKey(subkeyname);
                        string MIDSettingConfig = (string)sub_key.GetValue("Location");
                        aConnectionString = GetMIDSettingsDBConnectionString(MIDSettingConfig);
                        //===============================================================================================
                        // creates a consistantly formatted connection string and sets it in the DataConnectionDialog
                        //===============================================================================================
                        Microsoft.Data.ConnectionUI.SqlConnectionProperties connProp = new Microsoft.Data.ConnectionUI.SqlConnectionProperties();
                        connProp.ConnectionStringBuilder.ConnectionString = aConnectionString;
                        aConnectionString = connProp.ConnectionStringBuilder.ConnectionString;
                        if (aConnectionString.Trim().Contains("servername"))
                        {
                            //Exception conn_exc = new Exception("The database connection string has not been properly configured.\n Connection String = " + aConnectionString);
                            aConnectionString = null;
                            //HandleException(conn_exc);
                        }
                        break;
                    }
                }
            }
            // End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
#endif
            return DoDatabaseMaintenance(aConnectionString, false);		// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        }

        public bool DoDatabaseMaintenance(string aConnectionString, bool oneClick)	// TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
        // End TT#74 MD
        {
            bool bSuccessful = true;
            log.AddLogEntry("User has requested to perform database maintenance", eErrorType.message);
            // Begin TT#1668 - JSmith - Install Log
            // temporarily close upgrade log so can be updated by database utility
            frame.CloseUpgradeLog();
            // End TT#1688
            Process servStart = new Process();
#if (DEBUG)
            servStart.StartInfo.FileName = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\SQLServerDatabaseUpdate\bin\Debug\" + "DatabaseUtility.exe";
#else
            servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"\Database\DatabaseUtility.exe"; ;
#endif

            // Begin TT#74 MD - JSmith - One-button Upgrade
            //servStart.StartInfo.Arguments = "";
            if (aConnectionString == null)
            {
                servStart.StartInfo.Arguments = "";
            }
            else
            {
				// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
                if (oneClick)
                {
                    servStart.StartInfo.Arguments = @" """ + aConnectionString + @""" " + @" """ + frame.installLogName + @""" ";
                }
                else
                {
                    servStart.StartInfo.Arguments = @" """ + aConnectionString + @""" " + @" """ + frame.installLogName + @""" " + @" """ + "False" + @""" ";
                }
				// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window
            }
            // End TT#74 MD

            servStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            servStart.Start();
            servStart.WaitForExit();
			// Begin TT#1668 - JSmith - Install Log
            frame.OpenUpgradeLog();
			// End TT#1668
            if (servStart.ExitCode > 0)
            {
                log.AddLogEntry("Error occurred while processing database maintenance.", eErrorType.error);
                bSuccessful = false;
            }
            servStart.Close();
            log.AddLogEntry("User has completed database maintenance.", eErrorType.message);
            return bSuccessful;
        }

		// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
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
		// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 

        public bool DoEventSources()
        {
            bool bSuccessful = true;
            try
            {
                log.AddLogEntry("User has requested to add Windows Event Sources", eErrorType.message);
                Process servStart = new Process();
                #if (DEBUG)
                servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"..\..\..\CreateEventSource\bin\Debug\CreateEventSource.exe";
#else
                servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"\Utilities\CreateEventSource\CreateEventSource.exe";
#endif
                servStart.StartInfo.Arguments = "##INSTALLER##";
                servStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                servStart.Start();
                servStart.WaitForExit();
                if (servStart.ExitCode > 0)
                {
                    log.AddLogEntry("Error occurred while processing Windows Event Sources.", eErrorType.error);
                    bSuccessful = false;
                }
                servStart.Close();
                log.AddLogEntry("User has completed processing Windows Event Sources.", eErrorType.message);

                frame.ProgressBarIncrementValue(1);
            }
            catch (Exception ex)
            {
			    // Begin TT#1668 - JSmith - Install Log
				//log.AddLogEntry("Processing Windows Event Sources failed. " + ex.Message, eErrorType.message);
                log.AddLogEntry("Processing Windows Event Sources failed. " + ex.Message, eErrorType.error);
				// End TT#1668
            }
            return bSuccessful;
        }

        // Begin TT#1305-MD - JSmith - Change Auto Upgrade
        //public bool IsCrystalReportsInstalled(ucInstallationLog log)
        //{
        //    bool crystalReportsInstalled = false;
        //    try
        //    {
        //        return SearchRegistryForProduct(InstallerConstants.cBusinessObjectsSoftwareKey, InstallerConstants.cCrystalReportsSoftwareKey, log, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Begin TT#1668 - JSmith - Install Log
        //        //log.AddLogEntry("Determining if Crystal Reports is installed failed. " + ex.Message, eErrorType.message);
        //        log.AddLogEntry("Determining if Crystal Reports is installed failed. " + ex.Message, eErrorType.error);
        //        // End TT#1668
        //    }
        //    return crystalReportsInstalled;
        //}
        public bool IsCrystalReportsInstalled(ucInstallationLog log)
        {
            try
            {
                Dictionary<string, object> keyValuePairs;
                using (var settingsRegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\SAP BusinessObjects\\Crystal Reports for .NET Framework 4.0\\Crystal Reports"))
                {
                    if (settingsRegKey != null)
                    {
                        var valueNames = settingsRegKey.GetValueNames();
                        keyValuePairs = valueNames.ToDictionary(name => name, settingsRegKey.GetValue);
                        foreach (KeyValuePair<string, object> entry in keyValuePairs)
                        {
                            if (entry.Key.Contains("CRRuntime"))
                            {
                                if (Convert.ToString(entry.Value).Contains("13.0.12"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.AddLogEntry("Determining if Crystal Reports is installed failed. " + ex.Message, eErrorType.error);
            }
            return false;
        }
		// End TT#1305-MD - JSmith - Change Auto Upgrade

        private bool SearchRegistryForProduct(string sKey, string sProduct, ucInstallationLog log, bool blStartWith)
        {
            try
            {
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey reg_key = local_key.OpenSubKey(sKey);
                if (reg_key != null)
                {
                    string[] subkeyNames = reg_key.GetSubKeyNames();

                    foreach (string subkeyName in subkeyNames)
                    {
                        if (subkeyName == sProduct)
                        {
                            return true;
                        }
                        else if (blStartWith &&
                            subkeyName.StartsWith(sProduct))
                        {
                            return true;
                        }
                        else
                        {
                            return SearchRegistryForProduct(sKey + @"\" + subkeyName, sProduct, log, blStartWith);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
    			// Begin TT#1668 - JSmith - Install Log
				//log.AddLogEntry("Determining if " + sProduct + " is installed failed. " + ex.Message, eErrorType.message);
                log.AddLogEntry("Determining if " + sProduct + " is installed failed. " + ex.Message, eErrorType.error);
				// End TT#1668
            }
            return false;
        }

        public bool InstallCrystalReports(bool bHideWindow)
        {
            bool bSuccessful = true;
            try
            {
                log.AddLogEntry("User has requested to install Crystal Reports", eErrorType.message);

                //bSuccessful = InstallCrystalReportsViewer(bHideWindow);
                if (bSuccessful)
                {
                    bSuccessful = InstallCrystalReportsApp(bHideWindow);
                }
            }
            catch (Exception ex)
            {
			    // Begin TT#1668 - JSmith - Install Log
				//log.AddLogEntry("Installing Crystal Reports failed. " + ex.Message, eErrorType.message);
                log.AddLogEntry("Installing Crystal Reports failed. " + ex.Message, eErrorType.error);
				// End TT#1668
            }
            return bSuccessful;
        }

//        private bool InstallCrystalReportsViewer(bool bHideWindow)
//        {
//            bool bSuccessful = true;
//            try
//            {
//                log.AddLogEntry("Install Crystal Reports Viewer", eErrorType.message);
//                Process servStart = new Process();
//#if (DEBUG)
//                servStart.StartInfo.FileName = Application.StartupPath + @"\" + InstallerConstants.cCrystalReportsViewer;
//#else
                
//                servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"\Utilities\" + InstallerConstants.cCrystalReportsViewer;
//#endif
//                servStart.StartInfo.Arguments = "";
//                if (bHideWindow)
//                {
//                    servStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//                }
//                else
//                {
//                    servStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
//                }
//                servStart.Start();
//                servStart.WaitForExit();
//                if (servStart.ExitCode > 0)
//                {
//                    log.AddLogEntry("Error occurred while installing Crystal Reports Viewer.", eErrorType.error);
//                    bSuccessful = false;
//                }
//                else
//                {
//                    log.AddLogEntry("User has completed installing Crystal Reports Viewer.", eErrorType.message);
//                }
//                servStart.Close();
//            }
//            catch (Exception ex)
//            {
//                log.AddLogEntry("Installing Crystal Reports Viewer failed. " + ex.Message, eErrorType.message);
//            }
//            return bSuccessful;
//        }

        private bool InstallCrystalReportsApp(bool bHideWindow)
        {
            bool bSuccessful = true;
            try
            {
                log.AddLogEntry("User has requested to install Crystal Reports", eErrorType.message);
                Process servStart = new Process();
#if (DEBUG)
                if (frame._64bitOS)
                {
                    servStart.StartInfo.FileName = Application.StartupPath + @"\" + InstallerConstants.cCrystalReports64;
                }
                else
                {
                    servStart.StartInfo.FileName = Application.StartupPath + @"\" + InstallerConstants.cCrystalReports32;
                }
#else
                if (frame._64bitOS)
                {
                servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"\Utilities\" + InstallerConstants.cCrystalReports64;
                }
                else
                {
                servStart.StartInfo.FileName = Directory.GetParent(Application.StartupPath) + @"\Utilities\" + InstallerConstants.cCrystalReports32;
                }
#endif
                servStart.StartInfo.Arguments = "";
                if (bHideWindow)
                {
                    servStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                else
                {
                    servStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                }
                servStart.Start();
                servStart.WaitForExit();
                if (servStart.ExitCode > 0)
                {
                    log.AddLogEntry("Error occurred while installing Crystal Reports.", eErrorType.error);
                    bSuccessful = false;
                }
                else
                {
                    log.AddLogEntry("User has completed installing Crystal Reports.", eErrorType.message);
                }
                servStart.Close();
            }
            catch (Exception ex)
            {
                log.AddLogEntry("Installing Crystal Reports failed. " + ex.Message, eErrorType.message);
            }
            return bSuccessful;
        }

        private void rdoDatabaseMaintenance_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDatabaseMaintenance.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.databaseMaintenance;
                if (ConfigureNext != null)
                {
                    ConfigureNext(this, new EventArgs());
                }
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesDatabase");
                frame.SetStatusMessage(frame.GetText("UtilitiesDatabase"));
				// End TT#1668
            }
        }

        private void rdoStartServices_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStartServices.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.startServices;
                NotConfigureNext(this, new EventArgs());
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesStartServices");
                frame.SetStatusMessage(frame.GetText("UtilitiesStartServices"));
				// End TT#1668
            }
        }

        private void rdoStopServices_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStopServices.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.stopServices;
                NotConfigureNext(this, new EventArgs());
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesStopServices");
                frame.SetStatusMessage(frame.GetText("UtilitiesStopServices"));
				// End TT#1668
            }
        }

        private void rdoRescan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRescan.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.scan;
                if (ConfigureNext != null)
                {
                    ConfigureNext(this, new EventArgs());
                }
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesRescan");
                frame.SetStatusMessage(frame.GetText("UtilitiesRescan"));
				// End TT#1668
            }
        }

        private void rdoEventSource_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoEventSource.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.eventSources;
                NotConfigureNext(this, new EventArgs());
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesEventSources");
                frame.SetStatusMessage(frame.GetText("UtilitiesEventSources"));
				// End TT#1668
            }
        }

        private void rdoCrystalReports_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCrystalReports.Checked == true)
            {
                //set the install task value on the frame
                frame.InstallTask = eInstallTasks.crystalReports;
                NotConfigureNext(this, new EventArgs());
				// Begin TT#1668 - JSmith - Install Log
				//frame.lblStatus.Text = frame.GetText("UtilitiesCrystalReports");
                frame.SetStatusMessage(frame.GetText("UtilitiesCrystalReports"));
				// End TT#1668
            }
        }
    }
}
