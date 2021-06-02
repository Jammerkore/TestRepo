using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class MIDRetailAPI
    {

        public SessionAddressBlock CreateSessionAddressBlock(string aUser, string sPassword, ref string processDescription, ref eROConnectionStatus connectionStatus)
        {
            SessionAddressBlock sab = null;
            connectionStatus = eROConnectionStatus.Successful;
            try
            {
                IMessageCallback messageCallback = new WebMessageCallback();
                SessionSponsor sponsor = new SessionSponsor();
                sab = new SessionAddressBlock(messageCallback, sponsor);
                MIDEnvironment.isWindows = false;

                processDescription += "Made an SAB..." + System.Environment.NewLine;

                // Register callback channel

                try
                {
                    System.Runtime.Remoting.Channels.IChannel channel = sab.OpenCallbackChannel();
                }
                catch (Exception exception)
                {
                    //UpdateLog("Error opening port #0 - " + exception.Message);
                    processDescription += "Error opening port #0 - " + exception.Message + System.Environment.NewLine;
                    connectionStatus = eROConnectionStatus.Failed;
                    return sab;
                }

                processDescription += "Opened a callback channel..." + System.Environment.NewLine;

                // Create Sessions

                try
                {
                    sab.CreateSessions( (int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | 
                                        (int)eServerType.Application | (int) eServerType.Header | (int)eServerType.Scheduler);
                }
                catch (Exception exception)
                {

                    Exception innerE = exception;
                    while (innerE.InnerException != null)
                    {
                        innerE = innerE.InnerException;
                    }
                    //UpdateLog("Error creating sessions - " + innerE.Message);
                    processDescription += "Error creating sessions - " + innerE.ToString() + System.Environment.NewLine;
                    connectionStatus = eROConnectionStatus.Failed;
                    return null;
                }

                processDescription += "Created sessions..." + System.Environment.NewLine;

                ScheduleData _scheduleData = new ScheduleData();


                eSecurityAuthenticate authentication = sab.ClientServerSession.UserLogin(aUser,
                //MIDConfigurationManager.AppSettings["Password"], eProcesses.StoreBinViewer);
                sPassword, eProcesses.clientApplication);
                if (authentication != eSecurityAuthenticate.UserAuthenticated)
                {
                    //UpdateLog("Unable to log in with user:" + aUser);
                    processDescription += "Unable to log in with user:" + aUser + System.Environment.NewLine;
                    connectionStatus = eROConnectionStatus.FailedInvalidUser;

                    return null;
                }

                bool performSingleInstanceCheck = sab.ClientServerSession.GlobalOptions.ForceSingleClientInstance;
                bool performSingleUserInstanceCheck = sab.ClientServerSession.GlobalOptions.ForceSingleUserInstance;

                string loginDate;
                string loginComputerName;
                if (performSingleUserInstanceCheck
                    && IsUserAlreadySignedOn(aUser, out loginDate, out loginComputerName))
                {
                    processDescription += "This user is already logged onto the system." + System.Environment.NewLine + "Login Date: " + loginDate + System.Environment.NewLine + "Login Computer: " + loginComputerName;
                    connectionStatus = eROConnectionStatus.FailedAlreadyLoggedIn;
                    return null;
                }

                if (sab.IsApplicationInBatchOnlyMode() == true)
                {
                    FunctionSecurityProfile batchOnlyModeSecurity = sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminBatchOnlyMode); //user must be authenticated before security can be read

                    if (batchOnlyModeSecurity.AllowUpdate)
                    {
                        connectionStatus = eROConnectionStatus.SuccessfulBatchOnlyMode;
                    }
                    else
                    {
                        connectionStatus = eROConnectionStatus.FailedBatchOnlyMode;
                        processDescription += "The MID application is running in Batch Only Mode." + System.Environment.NewLine + "Unable to login.  Please retry later.";
                        return null;
                    }
                }

                processDescription += "Initializing..." + System.Environment.NewLine;

                sab.ClientServerSession.Initialize();
                processDescription += "Client initialized..." + System.Environment.NewLine;

                sab.HierarchyServerSession.Initialize();
                processDescription += "Hierarchy Service initialized..." + System.Environment.NewLine;

                sab.ApplicationServerSession.Initialize();
                processDescription += "Application Service initialized..." + System.Environment.NewLine;

                sab.StoreServerSession.Initialize();
                processDescription += "Store Service initialized..." + System.Environment.NewLine;

                return sab;
            }

            catch 
            {
                throw;

            }
        }

        private bool IsUserAlreadySignedOn(string user, out string loginDate, out string loginComputerName)
        {
            MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
            loginDate = string.Empty;
            loginComputerName = string.Empty;

            return secAdmin.IsUserAlreadySignedOn(user, out loginDate, out loginComputerName);
        }
    }
}
