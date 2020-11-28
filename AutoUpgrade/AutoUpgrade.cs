using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

using IWshRuntimeLibrary;
using Shell32;

// Begin TT#1305-MD - JSmith - Change Auto Upgrade
using System.Xml;
using System.Data;

// ********************************************************** //
// *                     WARNING                            * //
// *                                                        * //
// * auto upgrade has a static version in AssemblyInfo.cs   * //
// * that must be changed each time the process is changed. * //
// *                                                        * //
// * If the version of Crystal Reports changes, the const   * //
// * CRRuntimeVersion must be changed along with the        * //
// * AssemblyVersion number in AssemblyInfo.cs.             * //
// *                                                        * //
// ********************************************************** //
// Begin TT#1305-MD - JSmith - Change Auto Upgrade

namespace MIDRetail.AutoUpgrade
{
    class AutoUpgrade
    {
        // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization

        //const string CRRuntimeVersion = "13.0.12";
		//const string CRRuntimeVersion = "13.0.21";
        static void Main(string[] args)
        {
            string fileName = null;
            string moduleName = null;
            string autoUpgradePath = null;
            string autoUpgradeLog = null;
            string lockFile = null;
            bool upgradingAutoUpgrade = false;  // TT#1305-MD - JSmith - Change Auto Upgrade

            if (args.Length > 0)
            {
                fileName = args[0];
                moduleName = args[1];
                autoUpgradePath = args[2];
                lockFile = args[3];
                upgradingAutoUpgrade = Convert.ToBoolean(args[4]);  // TT#1305-MD - JSmith - Change Auto Upgrade
                autoUpgradeLog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MIDRetail\AutoUpgrade.log";;

                EventLog.WriteEntry("MIDRetail", 
                    "filename:" + fileName + Environment.NewLine +
                    "moduleName:" + moduleName + Environment.NewLine +
                    "autoUpgradePath:" + autoUpgradePath + Environment.NewLine +
                    "lockFile:" + lockFile + Environment.NewLine +
                    "upgradingAutoUpgrade:" + upgradingAutoUpgrade + Environment.NewLine +
                    "autoUpgradeLog:" + autoUpgradeLog + Environment.NewLine, 
                EventLogEntryType.Information);

				// Begin TT#1305-MD - JSmith - Change Auto Upgrade
				// UpgradeClient(fileName, moduleName, autoUpgradePath, autoUpgradeLog, lockFile);
                UpgradeClient(fileName, moduleName, autoUpgradePath, autoUpgradeLog, lockFile, upgradingAutoUpgrade);
				// End TT#1305-MD - JSmith - Change Auto Upgrade

                StartClient(moduleName);
            }
        }

        private static void StartClient(string aModuleName)
        {

            ProcessStartInfo upgradeProcess = new ProcessStartInfo(aModuleName);

            upgradeProcess.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            Process.Start(upgradeProcess);

            Environment.Exit(0);
        }

        // Begin TT#1305-MD - JSmith - Change Auto Upgrade
		// private static void UpgradeClient(string aFileName, string aModuleName, string aAutoUpgradePath, string aAutoUpgradeLog, string aLockFile)
        private static void UpgradeClient(string aFileName, string aModuleName, string aAutoUpgradePath, string aAutoUpgradeLog, string aLockFile, bool aUpgradingAutoUpgrade)
		// End TT#1305-MD - JSmith - Change Auto Upgrade
        {
            frmUpgradeProgress progress = null;
            ArrayList upgradeList;
            bool upgradeComplete = false;
            string toFileName;
            string toDirectory;
            string currentDirectory;
            int index;
            FileAttributes fileAttributes;
            DirectoryInfo directoryInfo;
            StreamWriter logFile = null;
            string fileName = null;

            try
            {
                try
                {
                    DirectoryInfo parent = Directory.GetParent(aAutoUpgradeLog);
                    string strParent = parent.ToString();
                    if (!Directory.Exists(strParent))
                    {
                        Directory.CreateDirectory(strParent);
                    }

                    logFile = new StreamWriter(aAutoUpgradeLog, true);
                    logFile.WriteLine("*** Upgrade processed on " + DateTime.Now.ToString());
                    logFile.WriteLine("File Name: " + aFileName);
                    logFile.WriteLine("Module Name: " + aModuleName);
                    logFile.WriteLine("Auto Upgrade Path: " + aAutoUpgradePath);
                    logFile.WriteLine("Auto Upgrade Log: " + aAutoUpgradeLog);
                    logFile.WriteLine("Lock File: " + aLockFile);
                    logFile.WriteLine("Upgrading Auto Upgrade: " + aUpgradingAutoUpgrade.ToString());  // TT#1305-MD - JSmith - Change Auto Upgrade
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to create log file for " + aAutoUpgradeLog
                        + ". \n  Please check your configuration information. \n  The application will be terminated",
                            "Configuration Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    throw;
                }
                // END Issue 3619 stodd

                RemoveOldFiles(aFileName, logFile, true);

                progress = new frmUpgradeProgress(0, 0);
                progress.Title = "Upgrading";
				// Begin TT#1305-MD - JSmith - Change Auto Upgrade
				// progress.Show();
                if (!aUpgradingAutoUpgrade)
                {
                    progress.Show();
                }
				// End TT#1305-MD - JSmith - Change Auto Upgrade
                progress.labelText = "Retrieving upgrade information";
                upgradeList = new ArrayList();
                BuildUpgradeList(aModuleName, upgradeList, aAutoUpgradePath, logFile, aAutoUpgradeLog);
                progress.SetMaxValue = upgradeList.Count;
                // strip off last two pieces to get main folder name
                //toDirectory = aProcess.MainModule.FileName;
                toDirectory = aFileName;
                index = toDirectory.LastIndexOf(@"\");
                toDirectory = toDirectory.Substring(0, index);
#if (DEBUG)
                index = toDirectory.LastIndexOf(@"\");
                toDirectory = toDirectory.Substring(0, index);
#else
                index = toDirectory.LastIndexOf(@"\");
                toDirectory = toDirectory.Substring(0, index);
#endif

                int i = 0;
                foreach (FileInfo file in upgradeList)
                {
                    fileName = file.FullName;

                    ++i;
                    progress.SetValue = i;
                    progress.labelText = "Processing " + file.FullName;
                    toFileName = file.FullName.Replace(aAutoUpgradePath, toDirectory);

                    //make sure directory exists
                    index = toFileName.LastIndexOf(@"\");
                    currentDirectory = toFileName.Substring(0, index);
                    directoryInfo = new DirectoryInfo(currentDirectory);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    // override the readonly flag if file exists
                    else if (System.IO.File.Exists(toFileName))
                    {
                        fileAttributes = System.IO.File.GetAttributes(toFileName);
                        if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            System.IO.File.SetAttributes(toFileName, fileAttributes & ~FileAttributes.ReadOnly);
                        }
                    }
                    // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                    // System.IO.File.Copy(file.FullName, toFileName, true);
                    if (file.Name == aModuleName + ".config")
                    {
                        MergeConfigFile(file, toFileName);
                    }
                    else
                    {
                        System.IO.File.Copy(file.FullName, toFileName, true);
                    }
                    // End TT#1305-MD - JSmith - Change Auto Upgrade
                    logFile.WriteLine(file.FullName + " - Updated");
                }

                // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                //if (!IsCrystalReportsInstalled(logFile))
                //{
                //    InstallCrystalReports(true, logFile);
                //}

                RemoveOldFiles(aFileName, logFile, false);
				// End TT#1305-MD - JSmith - Change Auto Upgrade

                // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization 
                try
                {
                    RefreshLinks(aFileName, logFile);
                    RefreshDesktop();
                }
                catch // swallow any error and wait for reboot to refresh icons
                {
                }
                // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization

                upgradeComplete = true;
                System.IO.File.Delete(aLockFile);
            }
            catch (Exception exc)
            {
                string msg = exc.ToString();
                if (logFile != null)
                {
                    logFile.WriteLine("Log File: " + aAutoUpgradeLog);
                    if (fileName != null)
                        logFile.WriteLine(fileName + " - Failed");
                    logFile.WriteLine("Exception: " + exc.ToString());
                }
            }
            finally
            {
                if (logFile != null)
                {
                    logFile.Close();
                }
            }

            if (progress != null)
            {
                progress.CloseForm();
            }

            if (upgradeComplete)
            {
			    // Begin TT#1305-MD - JSmith - Change Auto Upgrade
				// MessageBox.Show("Upgrade complete.",
                //    "Upgrade Complete. Client will be restarted.",
                //    System.Windows.Forms.MessageBoxButtons.OK,
                //    System.Windows.Forms.MessageBoxIcon.Information);
                if (!aUpgradingAutoUpgrade)
                {
                    MessageBox.Show("Upgrade complete.",
                        "Upgrade Complete. Client will be restarted.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
				// End TT#1305-MD - JSmith - Change Auto Upgrade
            }
            else
            {
                MessageBox.Show("Upgrade incomplete. \n  The application may not function correctly.  Review upgrade log at " + aAutoUpgradeLog,
                    "Upgrade Terminated",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private static void BuildUpgradeList(string aModuleName, ArrayList aUpgradeList, string aAutoUpgradePath,
            StreamWriter aLogFile, string aLogFileName)
        {
            DirectoryInfo directoryInfo;
            DirectoryInfo[] directories;
            Process process;

            process = System.Diagnostics.Process.GetCurrentProcess();

            //get files in current directory
            AddDirectoryFiles(process, aModuleName, aUpgradeList, aAutoUpgradePath, aLogFile, aLogFileName);

            //get all directories and files in subdirectories
            directoryInfo = new DirectoryInfo(aAutoUpgradePath);
            directories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                AddDirectoryFiles(process, aModuleName, aUpgradeList, directory.FullName, aLogFile, aLogFileName);
            }
        }

        private static void AddDirectoryFiles(Process aProcess, string aModuleName, ArrayList aUpgradeList, string aDirectoryName,
            StreamWriter aLogFile, string aLogFileName)
        {
            FileInfo[] files;
            DirectoryInfo directoryInfo;

            directoryInfo = new DirectoryInfo(aDirectoryName);
            files = directoryInfo.GetFiles();

            //string logFile = string.Empty;
            //int lastIdx = aLogFileName.LastIndexOf(@"\");
            //if (lastIdx == -1)
            //{
            //    logFile = aLogFileName;
            //}
            //else
            //{
            //    logFile = aLogFileName.Substring(lastIdx + 1);
            //}

            foreach (FileInfo file in files)
            {
                // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                //if (file.Name.IndexOf(aModuleName + ".config") == -1 &&
                //    file.Name.IndexOf(aProcess.MainModule.ModuleName) == -1)	
                if (file.Name.IndexOf(aProcess.MainModule.ModuleName) == -1)
                // End TT#1305-MD - JSmith - Change Auto Upgrade
                {
                    aUpgradeList.Add(file);
                }
                else
                {
                    aLogFile.WriteLine(file.FullName + " - Bypassed");
                }
            }
        }

        // Begin TT#1305-MD - JSmith - Change Auto Upgrade
        private static void MergeConfigFile(FileInfo file, string toFileName)
        {
            string originalFile;
            XmlDocument newDoc, originalDoc;
            try
            {
                originalFile = file.FullName;
                newDoc = GetXmlDocument(originalFile);
                originalDoc = GetXmlDocument(toFileName);

                XmlNode appSettingsNode = originalDoc.SelectSingleNode("configuration/appSettings");

                if (appSettingsNode == null)
                {
                    appSettingsNode = originalDoc.SelectSingleNode("appSettings");
                }

                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        if (!KeyExists(newDoc, childNode.Attributes["key"].Value))
                        {
                            AddKey(toFileName, newDoc, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
                        }
                        else
                        {
                            UpdateKey(toFileName, newDoc, childNode.Attributes["key"].Value.ToString(), childNode.Attributes["value"].Value.ToString());
                        }
                    }
                }

                newDoc.Save(toFileName);
            }
            catch
            {
                throw;
            }
        }

        private static XmlDocument GetXmlDocument(string file)
        {
            if (!System.IO.File.Exists(file))
                return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return doc;
        }

        private static void AddKey(string fileName, XmlDocument xmlDoc, string strKey, string strValue)
        {
            XmlElement newElement;

            XmlNode appSettingsNode = GetSettingNode(xmlDoc);
            try
            {
                if (KeyExists(xmlDoc, strKey))
                {
                    return;
                }
                XmlNode newChild = null;
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        newChild = childNode.Clone();
                        break;
                    }
                }

                if (newChild == null)
                {
                    newElement = xmlDoc.CreateElement("add");
                    newElement.SetAttribute("key", strKey);
                    newElement.SetAttribute("value", strValue);
                    appSettingsNode.AppendChild(newElement);
                }
                else
                {
                    newChild.Attributes["key"].Value = strKey;
                    newChild.Attributes["value"].Value = strValue;
                    appSettingsNode.AppendChild(newChild);
                }
            }
            catch
            {
                throw;
            }
        }

        // Updates a key within the App.config
        private static void UpdateKey(string fileName, XmlDocument xmlDoc, string strKey, string newValue)
        {
            try
            {
                if (!KeyExists(xmlDoc, strKey))
                {
                    return;
                }
                XmlNode appSettingsNode = GetSettingNode(xmlDoc);
                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        if (childNode.Attributes["key"].Value == strKey)
                        {
                            childNode.Attributes["value"].Value = newValue;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static bool KeyExists(XmlDocument xmlDoc, string strKey)
        {
            if (xmlDoc == null)
            {
                return false;
            }

            XmlNode appSettingsNode = GetSettingNode(xmlDoc);
            if (appSettingsNode != null)
            {
                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        if (childNode.Attributes["key"].Value == strKey)
                            return true;
                    }
                }
            }
            return false;
        }

        private static XmlNode GetSettingNode(XmlDocument xmlDoc)
        {
            XmlNode appSettingsNode;
            appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
            if (appSettingsNode == null)
            {
                appSettingsNode = xmlDoc.SelectSingleNode("appSettings");
            }
            return appSettingsNode;
        }

        private static void RemoveOldFiles(string aFileName, StreamWriter aLogFile, bool isPreDelete)
        {
            string toDirectory;
            int index;

            toDirectory = aFileName;
            index = toDirectory.LastIndexOf(@"\");
            toDirectory = toDirectory.Substring(0, index);

            ModuleCleanup mc = new ModuleCleanup();
            mc.RemoveOldFiles(toDirectory, isPreDelete);
        }

        //private static bool IsCrystalReportsInstalled(StreamWriter aLogFile)
        //{
        //    try
        //    {
        //        Dictionary<string, object> keyValuePairs;
        //        using (var settingsRegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\SAP BusinessObjects\\Crystal Reports for .NET Framework 4.0\\Crystal Reports"))
        //        {
        //            if (settingsRegKey != null)
        //            {
        //                var valueNames = settingsRegKey.GetValueNames();
        //                keyValuePairs = valueNames.ToDictionary(name => name, settingsRegKey.GetValue);
        //                foreach (KeyValuePair<string, object> entry in keyValuePairs)
        //                {
        //                    if (entry.Key.Contains("CRRuntime"))
        //                    {
        //                        if (Convert.ToString(entry.Value).Contains(CRRuntimeVersion))
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        aLogFile.WriteLine("Determining if Crystal Reports is installed failed. " + ex.Message);
        //        MessageBox.Show("Unable to determine the version of Crystal Reports." + Environment.NewLine + "Application requires version " + CRRuntimeVersion + " to run properly.");
        //        return true;
        //    }
        //    return false;
        //}

     //   private static bool InstallCrystalReports(bool bHideWindow, StreamWriter aLogFile)
     //   {
     //       bool bSuccessful = true;
     //       bool b64Bit = false;
     //       try
     //       {
     //           if (IntPtr.Size == 8)
     //           {
     //               b64Bit = true;
     //           }

     //           aLogFile.WriteLine("User has requested to install Crystal Reports");
     //           Process servStart = new Process();

     //           if (b64Bit)
     //           {
     //               //servStart.StartInfo.FileName = Application.StartupPath + @"\" + "CRRuntime_64bit_13_0_12.msi";
					//servStart.StartInfo.FileName = Application.StartupPath + @"\" + "CRRuntime_64bit_13_0_21.msi";
     //           }
     //           else
     //           {
     //               //servStart.StartInfo.FileName = Application.StartupPath + @"\" + "CRRuntime_32bit_13_0_12.msi";
					//servStart.StartInfo.FileName = Application.StartupPath + @"\" + "CRRuntime_32bit_13_0_21.msi";
     //           }

     //           servStart.StartInfo.Arguments = "";
     //           if (bHideWindow)
     //           {
     //               servStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
     //           }
     //           else
     //           {
     //               servStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
     //           }
     //           servStart.Start();
     //           servStart.WaitForExit();
     //           if (servStart.ExitCode > 0)
     //           {
     //               aLogFile.WriteLine("Error occurred while installing Crystal Reports.");
     //               bSuccessful = false;
     //           }
     //           else
     //           {
     //               aLogFile.WriteLine("User has completed installing Crystal Reports.");
     //           }
     //           servStart.Close();
     //       }
     //       catch (Exception ex)
     //       {
     //           aLogFile.WriteLine("Installing Crystal Reports failed. " + ex.Message);
     //       }
     //       return bSuccessful;
     //   }
		// End TT#1305-MD - JSmith - Change Auto Upgrade

        // Begin TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
        private static void RefreshLinks(string aPath, StreamWriter aLogFile)
        {
            try
            {
                aLogFile.WriteLine("In RefreshLinks with path=" + aPath);
                RegistryKey appRegKey = GetExistingRegKey(aPath);
                if (appRegKey != null)
                {
                    object desktopLocation = appRegKey.GetValue("Desktop");
                    aLogFile.WriteLine("desktopLocation=" + desktopLocation);
                    object programGroupLocation = appRegKey.GetValue("ProgramGroup");
                    aLogFile.WriteLine("programGroupLocation=" + programGroupLocation);
                    object quickLaunchLocation = appRegKey.GetValue("QuickLaunch");
                    aLogFile.WriteLine("quickLaunchLocation=" + quickLaunchLocation);
					// Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                    //if (desktopLocation != null)
                    //{
                    //    ReplaceLink(Convert.ToString(desktopLocation), aLogFile);
                    //}
                    //if (programGroupLocation != null)
                    //{
                    //    ReplaceLink(Convert.ToString(programGroupLocation), aLogFile);
                    //}
                    //if (quickLaunchLocation != null)
                    //{
                    //    ReplaceLink(Convert.ToString(quickLaunchLocation), aLogFile);
                    //}
                    if (desktopLocation != null)
                    {
                        ReplaceLink(Convert.ToString(desktopLocation), aLogFile, appRegKey, "Desktop");
                    }
                    if (programGroupLocation != null)
                    {
                        ReplaceLink(Convert.ToString(programGroupLocation), aLogFile, appRegKey, "ProgramGroup");
                    }
                    if (quickLaunchLocation != null)
                    {
                        ReplaceLink(Convert.ToString(quickLaunchLocation), aLogFile, appRegKey, "QuickLaunch");
                    }
					// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                }
                else
                {
                    aLogFile.WriteLine("Did not find registry key for application=" + aPath);
                }
            }
            catch (Exception ex)
            {
                aLogFile.WriteLine("RefreshLinks failed. " + ex.Message);
            }
        }

        // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
        //private static void ReplaceLink(string strFile, StreamWriter aLogFile)
        //{
        //    try
        //    {
        //        if (strFile != null && System.IO.File.Exists(strFile))
        //        {
        //            aLogFile.WriteLine("Updating shortcut " + strFile + ".");

        //            WshShell shell = new WshShell();

        //            IWshShortcut link = (IWshShortcut)shell.CreateShortcut(strFile);

        //            link.Save(); // Issue save to see if have authority to update shortcuts.  If not, do not delete

        //            if (System.IO.File.Exists(strFile))
        //            {
        //                System.IO.File.Delete(strFile);
        //                RefreshDesktop();
        //            }

        //            link.Save();
        //            RefreshDesktop();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        aLogFile.WriteLine("Replacing shortcut " + strFile + " failed. " + ex.Message);
        //    }
        //}
        private static void ReplaceLink(string strFile, StreamWriter aLogFile, RegistryKey appRegKey, string aKey)
        {
            try
            {
                if (strFile != null && System.IO.File.Exists(strFile))
                {
                    aLogFile.WriteLine("Updating shortcut " + strFile + ".");

                    WshShell shell = new WshShell();

                    IWshShortcut link = (IWshShortcut)shell.CreateShortcut(strFile);

                    link.Save(); // Issue save to see if have authority to update shortcuts.  If not, do not delete

                    if (System.IO.File.Exists(strFile))
                    {
                        System.IO.File.Delete(strFile);
                        RefreshDesktop();
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

                    linkNew.Save();
                    if (updateRegistry)
                    {
                        Registry.SetValue(appRegKey.Name, aKey, strFile);
                    }
                    RefreshDesktop();
                }
            }
            catch (Exception ex)
            {
                aLogFile.WriteLine("Replacing shortcut " + strFile + " failed. " + ex.Message);
            }
        }
		// End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 

        private static RegistryKey GetExistingRegKey(string strFile)
        {
            //return variable
            string strRegKey = "";
            string strConfigKey = "";
            RegistryKey appRegKey = null;

            //drill into the registry
            RegistryKey local_key = Registry.LocalMachine;
            RegistryKey soft_key = local_key.OpenSubKey("SOFTWARE");
            RegistryKey mid_key = soft_key.OpenSubKey("MIDRetailInc");

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

            ////return keys
            //RegKey = RegKeys[RegKeys.GetUpperBound(0)].ToString().Trim();
            //ConfigKey = ConfigKeys[ConfigKeys.GetUpperBound(0)].ToString().Trim();

            return appRegKey;
        }

        private static void RefreshDesktop()
        {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }
        // End TT#1336-MD - JSmith - Rebrand to Logility Retail Optimization
    }
}
