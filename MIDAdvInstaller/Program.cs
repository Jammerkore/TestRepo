using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

namespace MIDRetailInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Begin TT#548 - JSmith - Set properties to "run this program as an administrator"
            bool IsElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (!IsElevated)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Application.ExecutablePath;
                proc.Verb = "runas";
                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    // The user refused the elevation.
                    // Do nothing and return directly ...
                    return;
                }
                Application.Exit();  // Quit itself
            }
            else 
            {
            // End TT#548 - JSmith - Set properties to "run this program as an administrator"
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // Begin TT#913 - JSmith - Object Reference Error
                //Application.Run(new InstallerFrame());
                if (ValidSecurity())
                {
                    if (CorrectFramework())
                    {
                        // Begin TT#1668 - JSmith - Install Log
                        //Application.Run(new InstallerFrame());
                        StreamWriter installLog = null;
                        InstallerFrame InstallerFrame = null;
                        string instLogName = @"\MIDRetailInstall_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".log";
                        try
                        {
                            string logFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\MIDRetail";
                            DirectoryInfo dir = new System.IO.DirectoryInfo(logFolder);
                            if (!dir.Exists)
                            {
                                dir.Create();
                            }
                            //if (IsFileInUse(logFolder + @"\MIDRetailInstall.log"))
                            if (IsFileInUse(logFolder + instLogName))
                            {
                                MessageBox.Show("Log file in use by other process.  Installer will close.", "Log File Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                return;
                            }

                            //installLog = new StreamWriter(logFolder + @"\MIDRetailInstall.log", true);
                            installLog = new StreamWriter(logFolder + instLogName, true);
                            InstallerFrame = new InstallerFrame(installLog);
                            Application.Run(InstallerFrame);
                        }
                        finally
                        {
                            if (InstallerFrame != null &&
                                InstallerFrame.InstallLog != null)
                            {
                                string logName = ((FileStream)InstallerFrame.InstallLog.BaseStream).Name;
                                InstallerFrame.InstallLog.Flush();
                                InstallerFrame.InstallLog.Close();

                                if (InstallerFrame != null &&
                                    InstallerFrame.LogLocation != null)
                                {
                                    DirectoryInfo dir = new System.IO.DirectoryInfo(InstallerFrame.LogLocation);
                                    if (dir.Exists)
                                    {
                                        //File.Copy(logName, InstallerFrame.LogLocation + @"\MIDRetailInstall.log", true);
                                        File.Copy(logName, InstallerFrame.LogLocation + instLogName, true);
                                    }
                                }
                            }
                        }
                        // End TT#1668
                    }
                }
                // End TT#913
            // Begin TT#548 - JSmith - Set properties to "run this program as an administrator"
            }
            // End TT#548 - JSmith - Set properties to "run this program as an administrator"
        }

        // Begin TT#913 - JSmith - Object Reference Error
        static private bool ValidSecurity()
        {
            try
            {
                //check to see if can access registry
                RegistryKey reg_key = Registry.LocalMachine;
                RegistryKey soft_key = reg_key.OpenSubKey(InstallerConstants.cRegistrySoftwareKey, true);
            }
            catch (System.Security.SecurityException err)
            {
                MessageBox.Show(err.Message, "Security Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        // End TT#913

        // Begin TT#913 - JSmith - Object Reference Error
        static private bool CorrectFramework()
        {
            //check to see if can access registry
            string supportedFrameworkVersions = ConfigurationManager.AppSettings["SupportedFrameworkVersions"].ToString();
            try
            {
                // Begin TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
                //RegistryKey reg_key = Registry.LocalMachine;
                //RegistryKey framework_key = reg_key.OpenSubKey(InstallerConstants.cRegistryFrameworkKey, false);
                //string[] subkeynames = framework_key.GetSubKeyNames();

                //foreach (string subkeyname in subkeynames)
                //{
                //    if (supportedFrameworkVersions.Contains(subkeyname))
                //    {
                //        return true;
                //    }
                //}

                char[] delim = ";".ToCharArray();
                string[] supportedFrameworkVersionList = supportedFrameworkVersions.Split(delim);
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(InstallerConstants.cRegistryFrameworkKey))
                {
                    if (ndpKey != null)
                    {
                        int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                        string installedFrameworkVersion = GetFrameworkVersion(releaseKey);
                        foreach (string supportedFrameworkVersion in supportedFrameworkVersionList)
                        {
                            if (installedFrameworkVersion == supportedFrameworkVersion)
                            {
                                return true;
                            }
                        }
                    }
                }
                // End TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
            }
            catch (System.Security.SecurityException err)
            {
                MessageBox.Show(err.Message, "Security Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                return false;
            }

            MessageBox.Show("Supported .Net Framework versions " + supportedFrameworkVersions + " not found.  Install will terminate.", "Prerequisite Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            return false;
        }

        // Begin TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
        /// <summary>
        /// With framework 4.5 and later, the release key identifies the version.  
        /// </summary>
        /// <param name="releaseKey"></param>
        /// <returns></returns>
        private static string GetFrameworkVersion(int releaseKey)
        {
            if (releaseKey >= 393295)  // TT#4656 - AGallagher - Support Windows 10 and Framework 4.6
            {
                return "4.6";  // TT#4656 - AGallagher - Support Windows 10 and Framework 4.6
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1";
            }
            //if ((releaseKey >= 378389))
            //{
            //    return "4.5";
            //}
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 
            return "4.5.1, 4.5.2 or 4.6 framework not installed";  // TT#4656 - AGallagher - Support Windows 10 and Framework 4.6
        }
        // End TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
        // End TT#913

        static private bool IsFileInUse(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("'path' cannot be null or empty.", "path");

            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        } 

    }
}
