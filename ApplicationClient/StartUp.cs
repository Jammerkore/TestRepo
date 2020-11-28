//Begin TT#1283 - JSMith - Win 7 Logo Batch Blocked
// Restructured auto upgrade and moved to separate module
// Too many changes to mark
//End TT#1283
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace MIDRetail.Client
{

	public class StartUp
	{

		private static bool _showMessage = true;

		public StartUp()
		{
		}

		[STAThread()]
		public static void Main(string[] args)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptions);

            // Begin Track #5771 - JSmith - Auto upgrade fails with connection string error
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // End Track #5771

            //Begin Track #4619 - JSmith - Add auto-upgrade
			string parmValue;
			bool autoUpgrade = false;
			DialogResult diagResult;
			FileVersionInfo currentFileVersionInfo;
			FileVersionInfo newFileVersionInfo;
			string clientFolder = "Client";
			string autoUpgradePath = string.Empty;
			string msg;
			string fileName;
            string autoUpgradeFileName = null;
			StreamWriter writer;
			bool upgradeClient = false;
			bool repair = false;
			string testObject = "MIDRetail.Windows.dll";
			Process process;
			bool bypassUpgrade = false;
			// BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
            string user = string.Empty;
            string password = string.Empty;
			// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
			try
			{
                // do not use MIDConfigurationManager so DataCommon can be upgraded
				parmValue = ConfigurationManager.AppSettings["AutoUpgradeClient"];
				if (parmValue != null)
				{
					try
					{
						autoUpgrade = Convert.ToBoolean(parmValue);
					}
					catch
					{
						autoUpgrade = false;
						diagResult = MessageBox.Show("Invalid setting for AutoUpgradeClient. The client will not be upgraded",
							"",
							System.Windows.Forms.MessageBoxButtons.OKCancel, 
							System.Windows.Forms.MessageBoxIcon.Warning);
						if (diagResult == System.Windows.Forms.DialogResult.Cancel)
						{
							return;
						}
					}
				}

				if (autoUpgrade)
				{
					// determine if prior upgrade failed
					fileName = Path.GetTempPath() + "\\MIDUpgrade.txt";
					if (File.Exists(fileName))
					{
						diagResult = MessageBox.Show("A prior upgrade has failed.  Do you want to repair the application?",
							"Upgrade Failure",
							System.Windows.Forms.MessageBoxButtons.YesNo, 
							System.Windows.Forms.MessageBoxIcon.Question);
						if (diagResult == System.Windows.Forms.DialogResult.Yes)
						{
							repair = true;
						}
						else
						{
							bypassUpgrade = true;
						}
					}

					if (!bypassUpgrade)
					{
						if (autoUpgrade || repair)
						{
                            // do not use MIDConfigurationManager so DataCommon can be upgraded
							parmValue = ConfigurationManager.AppSettings["AutoUpgradePath"];
							if (parmValue != null)
							{
								autoUpgradePath = Convert.ToString(parmValue, CultureInfo.CurrentCulture);
							}
							else
							{
								diagResult = MessageBox.Show("Invalid setting for AutoUpgradePath. The client will not be upgraded",
									"Configuration Error",
									System.Windows.Forms.MessageBoxButtons.OKCancel, 
									System.Windows.Forms.MessageBoxIcon.Warning);
								if (diagResult == System.Windows.Forms.DialogResult.Cancel)
								{
									return;
								}
							}

							if (!Directory.Exists(autoUpgradePath))
							{
								diagResult = MessageBox.Show("AutoUpgradePath " + autoUpgradePath + " either does not exist or you do not have access. \n The client will not be upgraded",
									"Configuration Error",
									System.Windows.Forms.MessageBoxButtons.OKCancel, 
									System.Windows.Forms.MessageBoxIcon.Warning);
								if (diagResult == System.Windows.Forms.DialogResult.Cancel)
								{
									return;
								}
								else
								{
									upgradeClient = false;
								}
							}
							else
							{
								upgradeClient = true;
							}

                            // Begin TT#1540-MD - JSmith - Auto Upgrade not displaying different version message
                            autoUpgradeFileName = Path.GetTempPath() + "\\MIDAutoUpgrade.txt";
                            if (upgradeClient)
                            {
                                if (!File.Exists(autoUpgradeFileName))
                                {
                                    if (autoUpgrade &&
                                        !repair)
                                    {
                                        // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                                        //currentFileVersionInfo = GetApplicationVersion(testObject);
                                        currentFileVersionInfo = GetApplicationVersion(Path.GetDirectoryName(Application.ExecutablePath) + @"\" + testObject);
                                        // End TT#924
                                        newFileVersionInfo = GetApplicationVersion(autoUpgradePath + "\\" + clientFolder + "\\" + testObject);
                                        if (currentFileVersionInfo.FileVersion != newFileVersionInfo.FileVersion)
                                        {
                                            msg = "A different version of the application exists: "
                                                + "  \n Current=" + currentFileVersionInfo.FileVersion
                                                + "  \n      New=" + newFileVersionInfo.FileVersion
                                                + " \n\n Do you want to replace with the different version? ";
                                            diagResult = MessageBox.Show(msg,
                                                "Upgrade",
                                                System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                                                System.Windows.Forms.MessageBoxIcon.Question);
                                            if (diagResult == System.Windows.Forms.DialogResult.Cancel)
                                            {
                                                return;
                                            }
                                            else if (diagResult == System.Windows.Forms.DialogResult.No)
                                            {
                                                upgradeClient = false;
                                            }
                                        }
                                        else
                                        {
                                            upgradeClient = false;
                                        }
                                    }
                                }
                            }
                            // End TT#1540-MD - JSmith - Auto Upgrade not displaying different version message

                            // Begin TT#1305-MD - JSmith - Change Auto Upgrade
                            // check for new auto upgrade
                            if (upgradeClient && !repair && !bypassUpgrade)
                            {
                                FileAttributes fileAttributes;
                                currentFileVersionInfo = GetApplicationVersion(Path.GetDirectoryName(Application.ExecutablePath) + @"\MIDAutoUpgrade.exe");
                                newFileVersionInfo = GetApplicationVersion(autoUpgradePath + @"\" + clientFolder + @"\MIDAutoUpgrade.exe");
                                if (currentFileVersionInfo.FileVersion != newFileVersionInfo.FileVersion)
                                {
                                    writer = new StreamWriter(autoUpgradeFileName);
                                    writer.Close();
                                    // Begin TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                                    // kill MIDAutoUpgrade process so can be updated
                                    bool blKillingAutoUpgrade = true;
                                    int iKillAttempts = 0;
                                    while (blKillingAutoUpgrade)
                                    {
                                        try
                                        {
                                            bool blFoundAutoUpgrade = false;
                                            foreach (Process proc in Process.GetProcesses())
                                            {
                                                if (proc.ProcessName.ToUpper().Contains("MIDAUTO"))
                                                {
                                                    blFoundAutoUpgrade = false;
                                                    proc.Kill();
                                                    System.Threading.Thread.Sleep(2000);
                                                    ++iKillAttempts;
                                                }
                                            }
                                            if (!blFoundAutoUpgrade ||
                                                iKillAttempts > 3)
                                            {
                                                blKillingAutoUpgrade = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Kill Process Error:" + ex.Message);
                                            blKillingAutoUpgrade = false;
                                        }
                                    }
                                    // End TT#1459 - MD - JSmith - Client Shortcut name should default to "Logility - RO" instead of "MIDRetail" 
                                    fileAttributes = System.IO.File.GetAttributes(currentFileVersionInfo.FileName);
                                    if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                                    {
                                        System.IO.File.SetAttributes(currentFileVersionInfo.FileName, fileAttributes & ~FileAttributes.ReadOnly);
                                    }
                                    System.IO.File.Copy(newFileVersionInfo.FileName, currentFileVersionInfo.FileName, true);

                                    process = System.Diagnostics.Process.GetCurrentProcess();
                                    writer = new StreamWriter(fileName);
                                    writer.Close();
                                    UpgradeClient(process, autoUpgradePath, fileName, "true");
                                }
                            }
							// End TT#1305-MD - JSmith - Change Auto Upgrade
                            // Begin TT#1540-MD - JSmith - Auto Upgrade not displaying different version message
                            if (File.Exists(autoUpgradeFileName))
                            {
                                File.Delete(autoUpgradeFileName);
                            }
                            //if (upgradeClient)
                            //{
                            //    if (autoUpgrade &&
                            //        !repair)
                            //    {
                            //        // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                            //        //currentFileVersionInfo = GetApplicationVersion(testObject);
                            //        currentFileVersionInfo = GetApplicationVersion(Path.GetDirectoryName(Application.ExecutablePath) + @"\" + testObject);
                            //        // End TT#924
                            //        newFileVersionInfo = GetApplicationVersion(autoUpgradePath + "\\" + clientFolder + "\\" + testObject);
                            //        if (currentFileVersionInfo.FileVersion != newFileVersionInfo.FileVersion)
                            //        {
                            //            msg = "A different version of the application exists: "
                            //                + "  \n Current=" + currentFileVersionInfo.FileVersion 
                            //                + "  \n      New=" + newFileVersionInfo.FileVersion 
                            //                + " \n\n Do you want to replace with the different version? ";
                            //            diagResult = MessageBox.Show(msg,
                            //                "Upgrade",
                            //                System.Windows.Forms.MessageBoxButtons.YesNoCancel, 
                            //                System.Windows.Forms.MessageBoxIcon.Question);
                            //            if (diagResult == System.Windows.Forms.DialogResult.Cancel)
                            //            {
                            //                return;
                            //            }
                            //            else if (diagResult == System.Windows.Forms.DialogResult.No)
                            //            {
                            //                upgradeClient = false;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            upgradeClient = false;
                            //        }
                            //    }
                            //}
                            // End TT#1540-MD - JSmith - Auto Upgrade not displaying different version message
						}

						if (upgradeClient)
						{
							process = System.Diagnostics.Process.GetCurrentProcess();
							writer = new StreamWriter(fileName);
							writer.Close();
							// Begin TT#1305-MD - JSmith - Change Auto Upgrade
							// UpgradeClient(process, autoUpgradePath, fileName);
                            UpgradeClient(process, autoUpgradePath, fileName, "false");
							// End TT#1305-MD - JSmith - Change Auto Upgrade
                            //File.Delete(fileName);
						}
					}
				}
                // BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
				//StartClient();
                ProcessArgs(args, ref user, ref password);
				StartClient(user, password);
				// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
			}
			catch (Exception exc)
			{
				if (_showMessage)
				{
					MessageBox.Show(exc.ToString());
				}
			}
			
		}

        // BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window
		public static bool ProcessArgs(string[] args, ref string user, ref string password)
        {
            bool hasArguments = false;

            try
            {
                if (args.Length == 2)
                {
                    user = args[0].ToString().Trim(); ;
                    password = args[1].ToString().Trim();
                    hasArguments = true;
                }

                return hasArguments;
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }
		// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window

		
		// BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window
		//private static void StartClient()
		private static void StartClient(string user, string password)
		// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window
		{
			MIDRetail.Windows.ClientStartup clientStartup = null;
			try
			{
				clientStartup = new MIDRetail.Windows.ClientStartup();
				// BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window
				//clientStartup.Start();
				clientStartup.Start(user, password);
				// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window
			}
			catch
			{
				throw;
			}
		}

		private static FileVersionInfo GetApplicationVersion(string aModuleName)
		{
			try
			{
				return FileVersionInfo.GetVersionInfo(aModuleName);
			}
			catch
			{
				MessageBox.Show("Unable to retrieve information for " + aModuleName 
					+ ". \n  Please check your configuration information. \n  The application will be terminated",
					"Configuration Error",
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Error);
				_showMessage = false;
				throw;
			}
		}

        // Begin TT#1305-MD - JSmith - Change Auto Upgrade
        // private static void UpgradeClient(Process aProcess, string aAutoUpgradePath, string aLockFile)
        private static void UpgradeClient(Process aProcess, string aAutoUpgradePath, string aLockFile, string aUpgradingAutoUpgrade)
		// End TT#1305-MD - JSmith - Change Auto Upgrade
        {
            // We should trigger a process with the AutoUpgrade.exe  

            ProcessStartInfo upgradeProcess = new ProcessStartInfo("MIDAutoUpgrade.exe");

            // Need to give argument of what the Main programfile is to execute after upgrade, will restart the main app when finished upgrading.  

            // Begin TT#1305-MD - JSmith - Change Auto Upgrade
			// upgradeProcess.Arguments = @"""" + aProcess.MainModule.FileName + @""" """ + aProcess.MainModule.ModuleName + @""" """ + aAutoUpgradePath + @""" """ + aLockFile + @"""";
            upgradeProcess.Arguments = @"""" + aProcess.MainModule.FileName + @""" """ + aProcess.MainModule.ModuleName + @""" """ + aAutoUpgradePath + @""" """ + aLockFile + @""" """ + aUpgradingAutoUpgrade + @"""";
			// End TT#1305-MD - JSmith - Change Auto Upgrade

            upgradeProcess.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            // Start AutoUpgrade.exe  

            Process.Start(upgradeProcess);

            Environment.Exit(0);

        }

		private static void UnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
		{
			string message;
			Exception e = (Exception) args.ExceptionObject;
			message = e.ToString();
			while (e.InnerException != null) 
			{
				message += Environment.NewLine;
				e = e.InnerException;
				message = e.ToString();
			}
			MessageBox.Show(message, "UnhandledExceptions", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
		}

	}
}
