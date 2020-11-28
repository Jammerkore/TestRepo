using System;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Security.Principal;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Windows;


using System.Net;
using System.Net.Sockets;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ClientStartup.
	/// </summary>
	public class ClientStartup
	{
		public ClientStartup()
		{
            // BEGIN TT#5777 - AGallagher - force the RO client cluture to allows operate as en-US
            var culture = new CultureInfo("en-US");  
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            // END TT#5777 - AGallagher - force the RO client cluture to allows operate as en-US
		}
        // BEGIN TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
		//public void Start()
		public void Start(string user, string password)
		// END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
		{
			bool loggedIn = false;
			bool clientSessionInitialized = false;
			SessionAddressBlock SAB = null;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			IChannel channel;
			//Begin Track #4619 - JSmith - Add auto-upgrade
			string clientVersion;
			string controlServerVersion;
			int index;
			string msg;
			//End Track #4619
            // Begin TT#1269 - gtaylor
            bool status_done = false;
         
            // End TT#1269

            

			try
			{
                // Begin Track #5771 - JSmith - Auto upgrade fails with connection string error
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                // End Track #5771

                // Begin TT#1269 - gtaylor
                status_done = false;
                // End TT#1269

                // check environment
                string message = string.Empty;
                if (!MIDEnvironment.ValidEnvironment(out message))
                {
                    //System.Windows.Forms.MessageBox.Show(message, "Application Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    //return;

                    frmLoginMessageDisplay frmDisplay = new frmLoginMessageDisplay("Application Error" + System.Environment.NewLine + message);
                    frmDisplay.ShowDialog();
                    return;   // say goodnight Gracie, the application will exit
                }

				// Create objects

				messageCallback = new OnlineMessageCallback();
				sponsor = new SessionSponsor();
				SAB = new SessionAddressBlock(messageCallback, sponsor);

				// Register callback channel

                try
                {
                    channel = SAB.OpenCallbackChannel();
                }
                catch (Exception e)
                {
                    //System.Windows.Forms.MessageBox.Show("Error opening port #0 - " + e.Message, "Application Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    //throw;
                    frmLoginMessageDisplay frmDisplay = new frmLoginMessageDisplay("Application Error" + System.Environment.NewLine + "Error opening port #0 - " + e.Message);
                    frmDisplay.ShowDialog();
                    return;   // say goodnight Gracie, the application will exit
                }



                bool isAppInDebugMode = false;
                #if (DEBUG)
                isAppInDebugMode = true;
                #endif


                // Create Sessions
                //Begin TT#899-MD -jsobek -Services Unavailable Login Message
                System.Net.Sockets.SocketException sessionSocketException = null;
                Exception sessionException = null;
                try
                {
                    SAB.CreateSessions((int)eServerType.All);

                    if (!SAB.RemoteServices)
                    {
                        ChannelServices.UnregisterChannel(channel);
                    }

                    //Begin Track #4619 - JSmith - Add auto-upgrade
                    // compares the first 3 nodes of version to make sure client is compatable
                    clientVersion = SAB.ClientServerSession.GetProductVersion();
                    index = clientVersion.LastIndexOf(".");
                    clientVersion = clientVersion.Substring(0, index);
                    controlServerVersion = SAB.ControlServerSession.GetProductVersion();
                    index = controlServerVersion.LastIndexOf(".");
                    controlServerVersion = controlServerVersion.Substring(0, index);
                    if (isAppInDebugMode == false)
                    {
                        if (clientVersion != controlServerVersion)
                        {
                            msg = MIDText.GetTextOnly(eMIDTextCode.msg_IncompatibleClient);
                            msg = msg.Replace("{0}", clientVersion);
                            msg = msg.Replace("{1}", controlServerVersion);
                            System.Windows.Forms.MessageBox.Show(
                                msg,
                                MIDText.GetTextOnly(eMIDTextCode.lbl_IncompatibleClient),
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    sessionSocketException = e;
                }
                catch (Exception e)
                {
                    sessionException = e;
                }
                //End TT#899-MD -jsobek -Services Unavailable Login Message
				
                //End Track #4619





   


                if (SAB.IsServicesAvailable == false) //the application will exit if this condition is true
                {

                    if (isAppInDebugMode)
                    {
                        if (sessionSocketException != null)
                        {
                            throw sessionSocketException; // say goodnight Gracie, the application will exit
                        }
                        else
                        {
                            if (sessionException != null)
                                throw sessionException; // say goodnight Gracie, the application will exit
                        }
                    }
                    else
                    {
                        WriteSessionErrorsToEventLog(sessionSocketException, sessionException);
                        frmLoginMessageDisplay frmDisplay = new frmLoginMessageDisplay("MID Services are not currently available." + System.Environment.NewLine + "Unable to login.  Please retry later.");
                        frmDisplay.ShowDialog();
					    return;   // say goodnight Gracie, the application will exit
                    }
                }

                bool performBatchOnlyModeCheck = true;
                bool performSingleInstanceCheck = SAB.ClientServerSession.GlobalOptions.ForceSingleClientInstance;
                bool performSingleUserInstanceCheck = SAB.ClientServerSession.GlobalOptions.ForceSingleUserInstance;
                bool useWindowsAuthentication = SAB.ClientServerSession.GlobalOptions.UseWindowsLogin;
                bool useActiveDirectoryAuthentication = SAB.ClientServerSession.GlobalOptions.UseActiveDirectoryAuthentication;
                bool useActiveDirectoryAuthenticationWithDomain = SAB.ClientServerSession.GlobalOptions.UseActiveDirectoryAuthenticationWithDomain;
                bool usePreSalesAuthentication = false; //TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
                bool signInAsAdministrator = false;
                string environmentInfo = "Environment: " + EnvironmentBusinessInfo.GetSessionEnviroment(SAB);
                MIDUserInfo userInfo = new MIDUserInfo(); //reads the last user name logged in from isolated storage
                SecurityAdmin secAdmin = new SecurityAdmin();
                string adminUserName = secAdmin.GetUserNameFromRID(Include.AdministratorUserRID);



                if (user == "autoadmin")
                {
                    signInAsAdministrator = true;
                    user = adminUserName;
                }
                else
                {
                    if (user != string.Empty)
                    {
                        usePreSalesAuthentication = true;
                    }
                }

                //Determine if we should bypass the login screen that obtains credentials
                bool obtainCredentials = true;
                if (signInAsAdministrator == false && (usePreSalesAuthentication || useActiveDirectoryAuthentication || useActiveDirectoryAuthenticationWithDomain || useWindowsAuthentication))
                {
                    obtainCredentials = false;
                }

                //Set credentials if necessary
                if (signInAsAdministrator == false && (useActiveDirectoryAuthentication || useActiveDirectoryAuthenticationWithDomain || useWindowsAuthentication))
                {
                    System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                    user = currentUser.Name;
                    password = string.Empty;

                    if (useActiveDirectoryAuthentication)
                    {
                        user = StripOutDomain(user);
                    }

                }

                //Set authentication rules
                if (usePreSalesAuthentication)
                {
                    performBatchOnlyModeCheck = false;
                    performSingleInstanceCheck = false;
                    performSingleUserInstanceCheck = false;
                }

                bool haveIssues = true;
                bool closeDown = false;

                while (haveIssues == true)
                {
                    haveIssues = false;
                    string messageToDisplay = string.Empty;
                    bool hasNewPassword = false;
                    string newPassword = null;
                    bool haveObtainedCredentials = false;

                    if (obtainCredentials)
                    {
                        //Show the login screen to obtain credentials
                        formLogin frmLogin = new formLogin();
                        frmLogin.lblEnvironment.Text = environmentInfo;
                        //automatically set the login screen to administrator if needed
                        if (signInAsAdministrator)
                        {
                            frmLogin.txtUser.Text = user;
                            frmLogin.txtUser.Enabled = false;
                        }
                        else
                        {
                            frmLogin.txtUser.Text = userInfo.User; //keep the user name if they typed an incorrect pwd
                        }
                        frmLogin.ShowDialog();
                        if (frmLogin.Cancelled)
                        {
                            return;   // say goodnight Gracie, the application will exit
                        }

                        user = frmLogin.txtUser.Text;
                        userInfo.User = user;
                        password = frmLogin.txtPassword.Text;
                        newPassword = frmLogin.NewPassword;
                        haveObtainedCredentials = true;


                        if (newPassword != null)
                        {
                            hasNewPassword = true;
                        }

                        if (user.Trim() == string.Empty)
                        {
                            haveIssues = true;
                            messageToDisplay = "User required.";
                        }
                    
                    }

                    //Authenticate the credentials against the retail application
                    if (haveIssues == false)
                    {
                        //bool isUserValid;
                        eSecurityAuthenticate retVal = SAB.ClientServerSession.UserLogin(user, password, eProcesses.clientApplication);
                        if (retVal == eSecurityAuthenticate.UserAuthenticated)
                        {
                            //isUserValid = true;
                            if (hasNewPassword == true)
                            {
                                //show message that the password has been changed, and then keep going
                                eSecurityAuthenticate retValForPasswordChange = SAB.ClientServerSession.SetUserPassword(user, password, newPassword);
                                frmLoginMessageDisplay frmDisplay = new frmLoginMessageDisplay(MIDText.GetTextOnly((eMIDTextCode)retValForPasswordChange));
                                frmDisplay.ShowDialog();
                            }

                            if (useWindowsAuthentication)
                            {
                                bool obtainCredentialsWithWindowsAuthentication = SAB.ClientServerSession.UserOptions.ShowLogin; //when active, always obtain credentials, even when Windows Authentication is on - this allows users to sign on as someone else

                                if (obtainCredentialsWithWindowsAuthentication && haveObtainedCredentials == false)
                                {
                                    obtainCredentials = true;
                                    haveIssues = true; //loop back and get credentials
                                }
                            }
                        }
                        else
                        {
                            //isUserValid = false;

                            haveIssues = true;
                            messageToDisplay = MIDText.GetTextOnly((eMIDTextCode)retVal);

                            if (obtainCredentials == false) //Presales, Windows & Active Directory Authentication gets one and only one chance to authenticate
                            {
                                closeDown = true;
                            }
                        }
                    }

                    //Authenticate the credentials against Active Directory, if necessary
                    if (haveIssues == false && (useActiveDirectoryAuthentication || useActiveDirectoryAuthenticationWithDomain))
                    {
                        if (ActiveDirectoryCheck.IsCurrentUserValidInActiveDirectory() == false)
                        {
                            haveIssues = true;
                            messageToDisplay = "This user is not valid in Active Directory.";
                            closeDown = true;
                        }
                    }

                    //Perform Single User Instance Check, if necessary
                    if (haveIssues == false && performSingleUserInstanceCheck && user != adminUserName)
                    {
                        string loginDate;
                        string loginComputerName;
                        if (IsUserAlreadySignedOn(user, out loginDate, out loginComputerName))
                        {
                            haveIssues = true;
                            messageToDisplay = "This user is already logged onto the system." + System.Environment.NewLine + "Login Date: " + loginDate + System.Environment.NewLine + "Login Computer: " + loginComputerName;
                            closeDown = true;
                        }
                    }

                    //Perform Single Instance Check, if necessary
                    if (haveIssues == false && performSingleInstanceCheck)
                    {
                        if (IsThisInstanceASecondIntance())
                        {
                            haveIssues = true;
                            messageToDisplay = "Another client instance of the application is already running.";
                            closeDown = true;
                        }
                    }


                    //Perform Batch Only Mode Check, if necessary
                    if (haveIssues == false && performBatchOnlyModeCheck)
                    {
                        if (SAB.IsApplicationInBatchOnlyMode() == true)
                        {
                            bool isUserBatchOnlyAdmin;
                            FunctionSecurityProfile batchOnlyModeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminBatchOnlyMode); //user must be authenticated before security can be read

                            if (batchOnlyModeSecurity.AllowUpdate)
                            {
                                isUserBatchOnlyAdmin = true;
                            }
                            else
                            {
                                isUserBatchOnlyAdmin = false;
                            }

                            if (isUserBatchOnlyAdmin == false)
                            {
                                haveIssues = true;
                                messageToDisplay = "The MID application is running in Batch Only Mode." + System.Environment.NewLine + "Unable to login.  Please retry later.";
                                closeDown = true;
                            }
                        }
                    }


                    if (haveIssues && messageToDisplay != string.Empty)
                    {
                        frmLoginMessageDisplay frmDisplay = new frmLoginMessageDisplay(messageToDisplay);
                        frmDisplay.ShowDialog();
                    }

                    if (closeDown)
                    {
                        return;   // say goodnight Gracie, the application will exit
                    }


                } //while (haveIssues == true)


                //formLogin frmLogin = new formLogin(SAB);
                //bool firstTime = true;
               

                ////frmLogin.ShowLogin = userInfo.ShowLogin;
                //MIDUserInfo userInfo = new MIDUserInfo();
                //frmLogin.ShowLogin = true;
                //// End Track #5755

                //while (!loggedIn)
                //{
                //    if (firstTime)
                //    {
					     
                //        if (user != string.Empty)
                //        {
                //            frmLogin.User = user;
                //            frmLogin.Password = password;
                //            frmLogin.ShowLogin = false;

                //            eSecurityAuthenticate retVal = SAB.ClientServerSession.UserLogin(user, password, eProcesses.clientApplication);
    
                //            if (retVal == eSecurityAuthenticate.UserAuthenticated)
                //            {
                //                //// Begin Track #5755 - JSmith - Windows login changes
                //                //frmLogin.ShowLogin = SAB.ClientServerSession.UserOptions.ShowLogin;
                //                //// End Track #5755
                //                //if (!frmLogin.ShowLogin)
                //                //{
                //                    loggedIn = true;
                //                //}
                //            }

                //        }
                //        else if (SAB.ClientServerSession.GlobalOptions.UseWindowsLogin)
                //         // END TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
                //        {
                //            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                //            frmLogin.User = currentUser.Name;
                //            eSecurityAuthenticate retVal = SAB.ClientServerSession.UserLogin(currentUser.Name, "", eProcesses.clientApplication);
                //            if (retVal == eSecurityAuthenticate.UserAuthenticated)
                //            {
                //                // Begin Track #5755 - JSmith - Windows login changes
                //                frmLogin.ShowLogin = SAB.ClientServerSession.UserOptions.ShowLogin;
                //                // End Track #5755
                //                if (!frmLogin.ShowLogin)
                //                {
                //                    loggedIn = true;
                //                }
                //            }
                //        }
                //        else if (SAB.ClientServerSession.GlobalOptions.UseActiveDirectoryAuthentication)
                //        {
                //            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                //            frmLogin.User = currentUser.Name;
                //            eSecurityAuthenticate retVal = SAB.ClientServerSession.UserLogin(currentUser.Name, "", eProcesses.clientApplication);
                //            if (retVal == eSecurityAuthenticate.UserAuthenticated)
                //            {
                //                bool isUserValidInActiveDirectory = ActiveDirectoryCheck.IsCurrentUserValidInActiveDirectory();
                //                // Begin Track #5755 - JSmith - Windows login changes
                //                frmLogin.ShowLogin = SAB.ClientServerSession.UserOptions.ShowLogin;
                //                // End Track #5755
                //                if (!frmLogin.ShowLogin && isUserValidInActiveDirectory)
                //                {
                //                    loggedIn = true;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            frmLogin.User = userInfo.User;
                //        }
                //        frmLogin.EnvironmentMsg = "Environment: " + EnvironmentBusinessInfo.GetSessionEnviroment(SAB);
                //    }
                //    firstTime = false;

                //    // Begin Track #5755 - JSmith - Windows login changes
                //    //if (!loggedIn || userInfo.ShowLogin)
                //    if (!loggedIn || frmLogin.ShowLogin)
                //    // End Track #5755
                //    {
                //        frmLogin.ShowDialog();
                //        if (frmLogin.Cancelled)
                //        {
                //            return;   // say goodnight Gracie
                //        }

                //        //eSecurityAuthenticate retVal = SAB.ClientServerSession.UserLogin(frmLogin.User, frmLogin.Password, eProcesses.clientApplication);
                //        eSecurityAuthenticate retVal = frmLogin.authenticationResult;
                //        if (retVal == eSecurityAuthenticate.UserAuthenticated)
                //        {
                //            if (frmLogin.NewPassword != null)
                //            {
                //                retVal = SAB.ClientServerSession.SetUserPassword(frmLogin.User, frmLogin.Password, frmLogin.NewPassword);
                //                MessageBox.Show(MIDText.GetTextOnly((eMIDTextCode)retVal), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            }
                //            loggedIn = true;
                //        }
                //        else
                //        {
                //            MessageBox.Show(MIDText.GetTextOnly((eMIDTextCode)retVal), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //    }
                //}





                // Begin TT#1269 - gtaylor
                StatusBarWindow clientstartup_splash = new StatusBarWindow();
                clientstartup_splash.BringToFront();  // TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window 
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    using (clientstartup_splash)
                    {
                        clientstartup_splash.Show();
                        Application.DoEvents();
                        while (!status_done)
                            Application.DoEvents();
                        clientstartup_splash.Close();
                    }
                });
                clientstartup_splash.StatusText = "Initializing ...";
                clientstartup_splash.StatusBarValue = 10;
                Application.DoEvents();
                Thread.Sleep(500);  // TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window  
                // End TT#1269 - gtaylor

         
                userInfo.User = user; //frmLogin.User;

                // Begin Track #5755 - JSmith - Windows login changes
                //userInfo.ShowLogin = frmLogin.ShowLogin;
                // End Track #5755
				userInfo.WriteUserInfo();
                // Begin Track #5755 - JSmith - Windows login changes
                //if (SAB.ClientServerSession.GlobalOptions.UseWindowsLogin)
                //{
                //    SAB.ClientServerSession.UserOptions.ShowLogin = frmLogin.ShowLogin;
                //    SAB.ClientServerSession.UserOptions.UpdateShowLogin(SAB.ClientServerSession.UserOptions.ShowLogin);
                //}
                // End Track #5755

                // Begin TT#1269 - gtaylor
                clientstartup_splash.StatusText = "Establishing Sessions ...";
                clientstartup_splash.StatusBarValue = 20;
                Application.DoEvents();
                Thread.Sleep(500);  // TT#1249-MD - AGallagher - Add option to recieve user ID and Password and bypass Login window  
                // End TT#1269 - gtaylor



            


				//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
				SAB.ControlServerSession.Initialize(SAB.ClientServerSession.UserRID, System.Environment.MachineName);
				//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
				SAB.ClientServerSession.Initialize();
				clientSessionInitialized = true;
				SAB.ApplicationServerSession.Initialize();
                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
				SAB.StoreServerSession.Initialize();
                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                // StoreServerSession must be initialized before HierarchyServerSession 
                SAB.HierarchyServerSession.Initialize();
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

                // Begin TT#267-MD - JSmith - Audit logging level in User Options not taking effect
                if (SAB.ApplicationServerSession.isSessionRunningLocal)
                {
                    SAB.ApplicationServerSession.Audit.LoggingLevel = SAB.ClientServerSession.Audit.LoggingLevel;
                } 
                if (SAB.HierarchyServerSession.isSessionRunningLocal)
                {
                    SAB.HierarchyServerSession.Audit.LoggingLevel = SAB.ClientServerSession.Audit.LoggingLevel;
                }
                if (SAB.StoreServerSession.isSessionRunningLocal)
                {
                    SAB.StoreServerSession.Audit.LoggingLevel = SAB.ClientServerSession.Audit.LoggingLevel;
                }
                // End TT#267-MD - JSmith - Audit logging level in User Options not taking effect


                //Begin TT#901-MD -jsobek -Batch Only Mode
               
                if (SAB.IsRemoteServices() == true)
                {
                    SecurityAdmin securityAdmin = new SecurityAdmin();
                   
                    if (securityAdmin.UseBatchOnlyMode() == true)
                    {
                        string controlServerName;
                        int controlServerPort;
                        double clientTimerIntervalInMilliseconds;
                        double tmpInterval;

                        SAB.GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out clientTimerIntervalInMilliseconds, out tmpInterval);
                       
                        SAB.clientSocketManager = new SocketClientManager();
                        SAB.clientSocketManager.StartClient(controlServerName, controlServerPort, clientTimerIntervalInMilliseconds, SAB.PerformClientCommandFromControlService);
                        if (SAB.clientSocketManager.ableToConnect == false)
                        {
                            //show a message and quit
                            MessageBox.Show("Unable to connect to the control service. Please try again later.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;   // say goodnight Gracie
                        }
                    }
                }
                //End TT#901-MD -jsobek -Batch Only Mode


                // Begin TT#1269 - gtaylor
                status_done = true;
                // End TT#1269
                // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View
                Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + @"\Windows7Copy.isl", true);
                Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + @"\StyleChainLadder.isl", true, "ChainLadderLibraryName1");
                // End TT#609-MD
                // Begin TT#2972 - RMatelic - Select Date Range highlights months at a time
                Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + @"\Windows7NoHotTrack.isl", true, "Windows7NoHotTrackLibraryName");
                // End TT#2972 
                // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                clientstartup_splash.Close();
                clientstartup_splash = null;
                // Begin TT#3513 - JSmith - Clean Up Memory Leaks
				System.Windows.Forms.Application.Run(new MIDRetail.Windows.Explorer(SAB));

				SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());

			}
			catch (System.Net.Sockets.SocketException e)
			{
                // Begin TT#87 MD - JSmith - The type initializer for MIDText fails if control server not found
                //System.Windows.Forms.MessageBox.Show(MIDText.GetText(eMIDTextCode.cannotAccessAppServer) + e.Message, "MRS System Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                msg = "Can not access application services.";
                if (MIDConnectionString.ConnectionString != null &&
                    MIDConnectionString.ConnectionString.Trim().Length > 0)
                {
                    msg = MIDText.GetText(eMIDTextCode.cannotAccessAppServer);
                }
                System.Windows.Forms.MessageBox.Show(msg + Environment.NewLine + e.Message, "MIDRetail System Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                // End TT#87 MD 
			}
			catch(Exception e)
			{
				if (clientSessionInitialized)
				{
					SAB.ClientServerSession.Audit.Log_Exception(e);
				}
				Exception innerE = e;
				while (innerE.InnerException != null) 
				{
					innerE = innerE.InnerException;
				}

				string message = innerE.Message + System.Environment.NewLine + innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
                // Begin Track #5828 - JSmith - ConnectionString not set
                //System.Windows.Forms.MessageBox.Show(message, MIDText.GetText(eMIDTextCode.systemError), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                string title = "System Error";
                if (MIDConnectionString.ConnectionString != null &&
                    MIDConnectionString.ConnectionString.Trim().Length > 0)
                {
                    title = MIDText.GetText(eMIDTextCode.systemError);
                }
                System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                // End Track #5828

			}
			finally
			{
				// make sure all locks are cleaned up.
				if (loggedIn)
				{
					MIDEnqueue midNQ = new MIDEnqueue();
					try
					{
						midNQ.OpenUpdateConnection();
						midNQ.Enqueue_DeleteAll(SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
						midNQ.CommitData();
					}
					catch 
					{
					
					}
					finally
					{
						midNQ.CloseUpdateConnection();
					}
				}

                //Begin TT#901-MD -jsobek -Batch Only Mode
                if (SAB.clientSocketManager != null)
                {  
                    SAB.clientSocketManager.StopClient();
                }
                //End TT#901-MD -jsobek -Batch Only Mode


				if (SAB != null)
				{
					SAB.CloseSessions();
				}
			}

		}
        private bool IsUserAlreadySignedOn(string user, out string loginDate, out string loginComputerName)
        {
            MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
            loginDate = string.Empty;
            loginComputerName = string.Empty;

            return secAdmin.IsUserAlreadySignedOn(user, out loginDate, out loginComputerName);
        }
        private static Mutex _m;
        private static bool IsThisInstanceASecondIntance()
        {
            try
            {
                // Try to open existing mutex.
                Mutex.OpenExisting("MIDRetailClientApp");
            }
            catch
            {
                // If exception occurred, there is no such mutex.
                _m = new Mutex(true, "MIDRetailClientApp");

                // Only one instance.
                return false;
            }
            // More than one instance.
            return true;
        }
        private static string StripOutDomain(string domainAndUser)
        {
            string userName = domainAndUser;

            int backslashPosition = domainAndUser.IndexOf("\\");
            if (backslashPosition > 0)
            {
                userName = domainAndUser.Substring(backslashPosition + 1);
            }

            return userName;
        }
        


        private void WriteSessionErrorsToEventLog(System.Net.Sockets.SocketException sessionSocketException, Exception sessionException)
        {
            try
            {
                string msg = string.Empty;
                if (sessionSocketException != null)
                {
                    msg = sessionSocketException.ToString();
                }
                else if (sessionException != null)
                {
                    msg = sessionException.ToString();
                }

                if (msg != string.Empty)
                {
                    System.Diagnostics.EventLog.WriteEntry("MIDRetail", msg, System.Diagnostics.EventLogEntryType.Error);
                }
            }
            catch
            {
                //Do nothing if we cannot write to the event log.
            }
        }

        //TT#901-MD -jsobek -Batch Only Mode -unused bad code below -should be all OR statements - it does not return true for "Yes" and "t" - see TT#906-MD
        //private bool ConvertLocalAppSetting(string aLocalAppSetting)
        //{
        //    if (aLocalAppSetting != null)
        //    {
        //        if (aLocalAppSetting.ToLower() == "true" || aLocalAppSetting.ToLower() == "yes" &&
        //            aLocalAppSetting.ToLower() == "t" || aLocalAppSetting.ToLower() == "y")
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
			
        //}
	}
}
