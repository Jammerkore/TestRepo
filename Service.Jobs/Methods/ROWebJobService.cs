using Microsoft.Win32.SafeHandles;
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Configuration;
using System.Text;
// using System.Threading;
using System.Timers;
using System.Data;

using Logility.ROWeb;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
//using Logility.Foundation.Core.Discovery;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROServices
{
    /// <inheritdoc />
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] //sets all calls to go through a single instance of this class, since we do not want to initialize the hierarchy service every time
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class ROWebJobService : IROWebJobService, IDisposable
    {
        //=======
        // FIELDS
        //=======

        private int iJobNumber = 0;
        private Dictionary<string, UserInfo> _dictUsers;
        private ArrayList _alLock; //setup the lock;
        private ROWebManager _ROWebManager;
        private ROWebTools _ROWebTools;
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        Timer _Timer;
        private string ROServerType;
        //private ServiceLocator _serviceLocator;
        private string _uniqueMicroserviceID = string.Empty;

        private bool _remoteServicesEnabled = false;
        private SocketClientManager _clientSocketManager = null;
        private string _socketMessage;


        /// <summary>
        /// Constructor
        /// </summary>
        public ROWebJobService()
        { 
		    _dictUsers = new Dictionary<string, UserInfo>(); 
			_alLock = new ArrayList();          
            _ROWebTools = new ROWebCommon.ROWebTools();
            _ROWebManager = new ROWeb.ROWebManager(_ROWebTools);
            
            // Get server information for load balancing
            ROServerType = ConfigurationManager.AppSettings["ROServerType"];
            if (ROServerType != null)
            {
                ROServerType = ROServerType.ToUpper();
            }
            else
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "ROServerType parameter not found. Single used for ROServerType.");
                ROServerType = "SINGLE";
            }
            if (IsPrimaryServer)
            {
                ROWebManager.LoadSecondaryServers();
            }

            if (!IsSecondaryServer)
            {
                // Add microservices service entry for this job service
                RegisterWithMicroservices();

                StartStaleConnectionsTimer();

                string ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

                if (ConnectionString == null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Severe, "Control Server not found - processing terminated");
                }
                else
                {
                    MIDConnectionString.ConnectionString = ConnectionString;
					ConnectToControlService();
                }
            }
            
        }

        /// <summary>
        /// Register the job service WCF endpoint with the microservices registry with the environent as the service name 
        /// </summary>
        private void RegisterWithMicroservices()
        {
            try
            {
                //ROWebTools.LogMessage(eROMessageLevel.Debug, "Registering with microservices");
                //_serviceLocator = new ServiceLocator();

                //string environmentName = ConfigurationManager.AppSettings["MIDEnvironment"];
                //string hostName = System.Net.Dns.GetHostName();

                //_uniqueMicroserviceID = hostName + "_" + Guid.NewGuid().ToString();

                //Uri serviceURI = GetJobServiceURI(hostName);
                //ServiceInfo toBeRegistered = new ServiceInfo(environmentName, _uniqueMicroserviceID, serviceURI);

                //ROWebTools.LogMessage(eROMessageLevel.Debug,
                //                      string.Format("Registering job service with name {0}, ID {1} with microservices at uri {2}",
                //                                    environmentName, _uniqueMicroserviceID, serviceURI.ToString()));
                //_serviceLocator.RegisterServiceAsync(toBeRegistered).Wait();
                //ROWebTools.LogMessage(eROMessageLevel.Debug, "registration succesful");
            }
            catch (Exception exc)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Failed to register Job Service with microservices:\n" + exc.ToString());
            }
        }

        /// <summary>
        /// Get the WCF URI for this job service 
        /// </summary>
        private Uri GetJobServiceURI(string hostName)
        {
            ServicesSection servicesSection =
                ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;

            foreach (ServiceElement service in servicesSection.Services)
            {
                if (service.Name == "Logility.ROServices.ROWebJobService")
                {
                    foreach(BaseAddressElement address in service.Host.BaseAddresses)
                    {
                        Uri serviceURI = new Uri(address.BaseAddress);
                        UriBuilder uriBuilder = new UriBuilder(serviceURI);
                        uriBuilder.Host = hostName;

                        return uriBuilder.Uri;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Deregister the job service WCF endpoint with the microservices registry 
        /// </summary>
        private void UnregisterWithMicroservices()
        {
            //if (!string.IsNullOrEmpty(_uniqueMicroserviceID))
            //{
            //    _serviceLocator.DeregisterServiceAsync(_uniqueMicroserviceID);
            //    _uniqueMicroserviceID = string.Empty;
            //}
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Flag identifying if dispose is in progress</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                CleanUp();
            }

            disposed = true;
        }

        //===========
        // PROPERTIES
        //===========

        /// <summary>
        /// Return a flag identifying if this is the primary Job service
        /// </summary>
        private bool IsPrimaryServer
        {
            get
            {
                return ROServerType == "PRIMARY";
            }
        }

        /// <summary>
        /// Return a flag identifying if this is the primary Job service
        /// </summary>
        private bool IsSecondaryServer
        {
            get
            {
                return ROServerType == "SECONDARY";
            }
        }

        /// <summary>
        /// Return the instance of the ROWebManager
        /// </summary>
        private ROWebManager ROWebManager
        {
            get
            {
                lock (_alLock.SyncRoot)
                {
                    if (_ROWebManager == null)
                    {
                        _ROWebManager = new ROWebManager(ROWebTools);
                    }
                }
                return _ROWebManager;
            }
        }

        /// <summary>
        /// Returns the instance of the ROWebTools
        /// </summary>
        /// <remarks>Creates an instance of the log in the tools</remarks>
        private ROWebTools ROWebTools
        {
            get 
            {
                lock (_alLock.SyncRoot)
                {
                    if (_ROWebTools == null)
                    {
                        _ROWebTools = new ROWebTools();
                        _ROWebTools.CreateLog();
                    }
                }
                return _ROWebTools; 
            }
        }

        //========
        // METHODS
        //========

        /// <summary>
        /// Free all managed objects
        /// </summary>
        private void CleanUp()
        {
            try
            {
                // Remove microservices service entry for this job service
                UnregisterWithMicroservices();

                // remove all users from the service
                // copy users to other list so do not get iterator error when remove from dictionary
                ArrayList alUsers = new ArrayList();
                foreach (KeyValuePair<string, UserInfo> ui in _dictUsers)
                {
                    alUsers.Add(ui.Key);
                }
                foreach (string sROUserID in alUsers)
                {
                    DisconnectUser(sROUserID);
                }

                if (_clientSocketManager != null
                    && _remoteServicesEnabled)
                {
                    _clientSocketManager.StopClient();
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error during CleanUp..." + ex.ToString() + ex.StackTrace);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Returns the Job ID
        /// </summary>
        /// <returns>The job id</returns>
        public string TestEventLog()
        {
            try
            {
                ROWebTools.WriteWindowsEventViewerEntry("Testing Event Log");
                return "OK";
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error updating event log..." + ex.ToString() + ex.StackTrace);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <inheritdoc />
        public void GetServiceServerInfo(string sROUserID, string sROSessionID, out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer)
        {
            try
            {
                SessionInfo si = GetUserSession(sROUserID, sROSessionID);
                if (si == null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Unable to find SessionInfo in GetServiceServerInfo", sROUserID, sROSessionID);
                    throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                }
                si.GetServiceServerInfo(out appSetControlServer, out appSetLocalStoreServer, out appSetLocalHierarchyServer, out appSetLocalApplicationServer);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error getting service server info..." + ex.ToString() + ex.StackTrace, sROUserID, sROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
		/// Notifies the service that the client is still alive.
        /// </summary>
        /// <returns>True, if successful.  False if the connection is already gone.</returns>
        public bool KeepAlive(string sROUserID, string sROSessionID)
        {
            UserInfo ui = GetUser(sROUserID);
            SessionInfo si = null;
            bool bConnected = false;

            if (ui != null)
            {
                string sIgnore = string.Empty;

                si = ui.GetSession(sROUserID, sROSessionID, ref sIgnore);
            }

            if (si != null)
            {
                try
                {
                    bConnected = si.KeepAlive();
                }
                catch (Exception ex)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Error in KeepAlive:\n" + ex.ToString() + ex.StackTrace, sROUserID, sROSessionID);
                    throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                }
            }

            return bConnected;
        }

        /// <summary>
        /// Returns a value representing the availability to start an RO Web client
        /// </summary>
        /// <returns>A numeric value representing the availability of the machine along with an available port</returns>
        /// <remarks>The should only be called by the primary Job Service</remarks>
        public double GetMachineAvailability(out string sPort)
        {
            return ROWebManager.GetMachineAvailability(out sPort);
        }

        /// <summary>
        /// Returns a value representing the availability to start an RO Web client
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="sPort">The port on which the RO Web Host is to be started</param>
        /// <param name="sProcessDescription">The output of the processing flow</param>
        /// <returns>A value indicating if the RO Web Host was successfully started</returns>
        /// <remarks>The should only be called by the primary Job Service</remarks>
        public eROConnectionStatus StartROWebHost(string sROUserID, string sPassword, string sROSessionID, string sPort, out string sProcessDescription)
        {
            sProcessDescription = string.Empty;
            eROConnectionStatus connectionStatus = eROConnectionStatus.Failed;

            sPort = ROWebManager.GetAvailablePort();
            if (ROWebManager.StartROWebHost(sPort))
            {
                UserInfo ui = GetUser(sROUserID);
                if (ui == null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Could not find User Info.", sROUserID, sROSessionID);
                    throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                }
                else
                {
                    ui.AddSession(sROUserID, sPassword, sROSessionID, Environment.MachineName, sPort, IsSecondaryServer, Environment.MachineName + ":" + sPort, ref sProcessDescription, ref connectionStatus);
                }
            }

            return connectionStatus;
        }

        /// <summary>
        /// Connect the user to an Session
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="sProcessDescription">The output of the processing flow</param>
        /// <returns>A value indicating if a session was successfully created</returns>
        [ObsoleteAttribute("This method will soon be deprecated. Use ConnectToServicesWithStatus instead.")]
        public bool ConnectToServices(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription)
        {
            eROConnectionStatus connectionStatus = ConnectToServicesWithStatus(sROUserID, sPassword, sROSessionID, out sProcessDescription);
            switch (connectionStatus)
            {
                case eROConnectionStatus.Successful:
                case eROConnectionStatus.SuccessfulBatchOnlyMode:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Connect the user to an Session
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="sProcessDescription">The output of the processing flow</param>
        /// <returns>A value indicating if a session was successfully created</returns>
        public eROConnectionStatus ConnectToServicesWithStatus(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription)
        {
            eROConnectionStatus connectionStatus = eROConnectionStatus.Failed;
            string[] serverParts = null;
            string sMachineName = null;
            string sPort = null;
            sProcessDescription = string.Empty;

            if (GetExistingConnection(sROUserID, sROSessionID) != null)
            {
                return eROConnectionStatus.Successful;
            }

            try
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Connecting Session.", sROUserID, sROSessionID);

                string sServer = null;
                lock (_alLock.SyncRoot)
                {
                    if (IsPrimaryServer)
                    {
                        sServer = ROWebManager.DetermineServer(out sPort);
                        if (sServer == null)
                        {
                            ROWebTools.LogMessage(eROMessageLevel.Error, "ConnectToServices failed.  Server or port not available.", sROUserID, sROSessionID);
                            throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                        }
                        connectionStatus = ROWebManager.StartROWebHost(sServer, sROUserID, sPassword, sROSessionID, sPort, out sProcessDescription);
                        serverParts = sServer.Split(':');
                        sMachineName = serverParts[0].Trim();
                    }
                    else if (IsSecondaryServer)
                    {
                        ROWebTools.LogMessage(eROMessageLevel.Error, "ConnectToServices should not be called on a secondary server.", sROUserID, sROSessionID);
                        throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                    }
                    else
                    {
                        sPort = ROWebManager.GetAvailablePort();
                        if (sPort == null)
                        {
                            ROWebTools.LogMessage(eROMessageLevel.Error, "ConnectToServices failed.  Port not available.", sROUserID, sROSessionID);
                            throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                        }
                        sServer = Environment.MachineName + ":" + sPort;
                        ROWebManager.StartROWebHost(sPort);
                        sMachineName = Environment.MachineName;
                    }

                    UserInfo ui = GetUser(sROUserID);
                    if (ui == null)
                    {
                        ROWebTools.LogMessage(eROMessageLevel.Error, "Could not find User Info.", sROUserID, sROSessionID);
                        throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                    }
                    else
                    {
                        ui.AddSession(sROUserID, sPassword, sROSessionID, sMachineName, sPort, IsSecondaryServer, sServer, ref sProcessDescription, ref connectionStatus);
                    }

                    ROWebTools.LogMessage(eROMessageLevel.Information, "Connection established to services on " + sMachineName + ":" + sPort, sROUserID, sROSessionID);
                    sProcessDescription += "Completed connecting to services on " + sMachineName + ":" + sPort + System.Environment.NewLine;
                }
                return connectionStatus;
            }
            catch (System.ServiceModel.FaultException fe)
            {
                throw fe;
            }
            catch
            {
                throw;
            }
        }

        private SessionInfo GetExistingConnection(string sROUserID, string sROSessionID)
        {
            UserInfo userInfo;
            SessionInfo si = null;
            bool bConnected = false;

            if (_dictUsers.TryGetValue(sROUserID, out userInfo))
            {
                userInfo.Sessions.TryGetValue(sROSessionID, out si);
                if (si != null)
                {
                    bConnected = si.KeepAlive();
                    if (!bConnected)
                    {
                        ROWebTools.LogMessage(eROMessageLevel.Severe, "RO client not responding.  A new RO client will be started.", sROUserID, sROSessionID);
                        DisconnectSession(sROUserID, sROSessionID);
                        si = null;
                    }
                }
            }

            return si;
        }

        private void StartStaleConnectionsTimer()
        {
            _Timer = new Timer();

            _Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _Timer.Interval = 30 * 1000; 

            _Timer.Enabled = true;
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            DisconnectStaleConnections();
        }

        private void DisconnectStaleConnections()
        {
            //ROWebTools.LogMessage(eROMessageLevel.Information, "DisconnectStaleConnections called...");  // TT#1156-MD remove SAB keep alive code
            lock (_alLock.SyncRoot)
            {
                List<SessionInfo> lStaleConnections = GetStaleConnections();

                foreach (SessionInfo si in lStaleConnections)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Information, "Closing stale connection.", si.UserID, si.SessionID); 
                    DisconnectSession(si.UserID, si.SessionID);
                }
            }
            //ROWebTools.LogMessage(eROMessageLevel.Debug, "DisconnectStaleConnections ending");  TT#1156-MD remove SAB keep alive code
        }

        private List<SessionInfo> GetStaleConnections()
        {
            List<SessionInfo> lStaleConnections = new List<SessionInfo>();

            foreach (UserInfo ui in _dictUsers.Values)
            {
                foreach (SessionInfo si in ui.Sessions.Values)
                {
                    if (!si.ConnectionIsAlive(ROWebManager.iStaleConnectionTimeoutInSeconds)) 
                    {
                        lStaleConnections.Add(si);
                    }
                    else
                    {
                        si.SessionPing();
                    }
                }
            }

            return lStaleConnections;
        }

        /// <summary>
        /// Remove the user from the service
        /// </summary>
        /// <returns></returns>
        public void DisconnectUser(string sROUserID)
        {
            string sROSessionID = null;
            try
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Disconnecting user.", sROUserID, sROSessionID);
                UserInfo ui = GetUser(sROUserID);
                // close all Sessions for the user
                // copy Sessions to other list so do not get iterator error when remove from dictionary
                ArrayList alSessions = new ArrayList();
                foreach (KeyValuePair<string, SessionInfo> ei in ui.Sessions)
                {
                    alSessions.Add(ei.Key);
                }
                foreach (string sEnv in alSessions) 
                {
                    sROSessionID = sEnv;
                    DisconnectSession(sROUserID, sROSessionID);
                }
                RemoveUser(sROUserID);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error in DisconnectUser." + ex.ToString() + ex.StackTrace, sROUserID, sROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Remove the Session from the user
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sROSessionID">The name of the Session</param>
        public void DisconnectSession(string sROUserID, string sROSessionID)
        {
            SessionInfo si = GetUserSession(sROUserID, sROSessionID, true);
            // remove session information on remote secondary Job Service
            if (!si.Server.ToUpper().Contains(Environment.MachineName.ToUpper())
                && !si.Server.ToUpper().Contains("LOCALHOST"))
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Disconnecting remote Session on " + si.Address, sROUserID, sROSessionID);
                ROWebManager.DisconnectSession(si.Server, sROUserID, sROSessionID);
            }

            ROWebTools.LogMessage(eROMessageLevel.Information, "Disconnecting Session on " + si.Address, sROUserID, sROSessionID);

            try
            {
                UserInfo ui = GetUser(sROUserID);
                ui.RemoveSessionInfo(sROSessionID);
                try
                {
                    _ROWebManager.AddAvailablePort(si.Port);
                    si.DisconnectSession();
                }
                catch
                {
                    // Swallow error when RO Web Console is terminated
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error in DisconnectSession." + ex.ToString() + ex.StackTrace, sROUserID, sROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Processes a request based on the parms
        /// </summary>
        /// <param name="Parms">parameter that needs to passed into funtion being called</param>
        /// <returns>An ROOut abstract object containing the status of the request</returns>
        public ROOut ProcessRequest(ROParms Parms)
        {
            if (Parms.RORequest == eRORequest.GetActiveUsers)
            {
                if (_remoteServicesEnabled)
                {
                    return GetActiveUsers();
                }
                else
                {
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Remote System Options are not enabled.  Request cannot be completed.", Parms.ROUserID, Parms.ROSessionID);
                    throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                }
            }

            SessionInfo si = GetUserSession(Parms.ROUserID, Parms.ROSessionID);

            try
            {
                ROOut outParms = si.ProcessRequest(Parms);
                return outParms;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error in ProcessRequest..." + ex.ToString() + ex.StackTrace, Parms.ROUserID, Parms.ROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        private ROOut GetActiveUsers()
        {
            return new ROIListOut(eROReturnCode.Successful, null, 0, GetActiveUsersList());
        }

        private List<ROActiveUserOut> GetActiveUsersList()
        {
            List<ROActiveUserOut> users = new List<ROActiveUserOut>();
            lock (_alLock.SyncRoot)
            {
                foreach (UserInfo ui in _dictUsers.Values)
                {
                    foreach (SessionInfo si in ui.Sessions.Values)
                    {
                        users.Add(new ROActiveUserOut(userName: ui.sROUserID, sessionID: si.SessionID, server: si.Server, machine: si.MachineName, port: si.Port));
                    }
                }
            }

            return users;
        }

        private void ConnectToControlService()
        {
            string controlServerName;
            int controlServerPort;
            double clientTimerIntervalInMilliseconds;
            double tmpInterval;

            ROWebTools.LogMessage(eROMessageLevel.Information, "In ConnectToControlService");

            try
            {
                SecurityAdmin securityAdmin = new SecurityAdmin();

                if (securityAdmin.UseBatchOnlyMode() == true)
                {
                    _remoteServicesEnabled = true;
                }
                else
                {
                    ROWebTools.LogMessage(eROMessageLevel.Warning, "Remote System Options not enabled.  Current user information will not be available.");
                    return;
                }

                GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out clientTimerIntervalInMilliseconds, out tmpInterval);
                
                _clientSocketManager = new SocketClientManager();
                _clientSocketManager.StartClient(controlServerName, controlServerPort, clientTimerIntervalInMilliseconds, PerformClientCommandFromControlService);
                if (_clientSocketManager.ableToConnect == false)
                {
                    //log a message and quit
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Unable to connect to the Control Service. Please try again later.");
                    return;
                }
                else
                {
                    _clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.SetAsJobServiceClient, "");
                    ROWebTools.LogMessage(eROMessageLevel.Information, "Connection established to the Control Service.");
                }
            }
            catch (Exception)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Unable to connect to the Control Service. Current User Information cannot be determined.  Verify Remote System Options has been enabled in System Options.");
            }
        }

        private bool GetSocketSettingsFromConfigFile(out string controlServerName, out int controlServerPort, out double clientTimerIntervalInMilliseconds, out double serverTimerIntervalInMilliseconds)
        {
            controlServerName = string.Empty;
            controlServerPort = -1;
            clientTimerIntervalInMilliseconds = -1;
            serverTimerIntervalInMilliseconds = -1;
            try
            {
                controlServerName = MIDConfigurationManager.AppSettings["RemoteOptions_ServerName"];
                string tempPort = MIDConfigurationManager.AppSettings["RemoteOptions_ServerPort"];
                string tempIntervalClient = MIDConfigurationManager.AppSettings["RemoteOptions_ClientInterval"];
                string tempIntervalServer = MIDConfigurationManager.AppSettings["RemoteOptions_ServerInterval"];

                int.TryParse(tempPort, out controlServerPort);
                double.TryParse(tempIntervalClient, out clientTimerIntervalInMilliseconds);
                double.TryParse(tempIntervalServer, out serverTimerIntervalInMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error encountered while attempting to read configuration settings: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tagInfo"></param>
        public void PerformClientCommandFromControlService(string command, string tagInfo)
        {
            if (command == SocketSharedRoutines.SocketClientCommands.ShutDown.commandName)
            {
                string[] tagParts = tagInfo.Split('|');
                if (tagParts != null
                    && tagParts.Length > 1)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Information, "Web client for " + tagParts[1].Trim() + " was forced closed by " + tagParts[0].Trim());
                    DisconnectUser(tagParts[1].Trim());
                }
            }
            if (command == SocketSharedRoutines.SocketClientCommands.ShowMessage.commandName)
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Title:" + RemoteSystemOptions.Messages.MessageForClientTitle + "; Message:" + tagInfo);
                _socketMessage = "Title:" + RemoteSystemOptions.Messages.MessageForClientTitle + "; Message:" + tagInfo;
            }
            if (command == SocketSharedRoutines.SocketClientCommands.GiveUserInfo.commandName)
            {
                List<ROActiveUserOut> users = GetActiveUsersList();
                foreach (ROActiveUserOut user in users)
                {
                    string userInfo = tagInfo;
                    userInfo += SocketSharedRoutines.Tags.rowStart;
                    userInfo += SocketSharedRoutines.Tags.userNameStart + user.UserName + SocketSharedRoutines.Tags.userNameEnd;
                    userInfo += SocketSharedRoutines.Tags.clientTypeStart + MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType_Web) + SocketSharedRoutines.Tags.clientTypeEnd;
                    userInfo += SocketSharedRoutines.Tags.machineNameStart + user.Machine + SocketSharedRoutines.Tags.machineNameEnd;
                    userInfo += SocketSharedRoutines.Tags.appStatusStart + "Logged In" + SocketSharedRoutines.Tags.appStatusEnd;
                    userInfo += SocketSharedRoutines.Tags.rowEnd;
                    _clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.TakeUserInfo, userInfo);
                }
            }
        }

        /// <summary>
        /// Retrieves information about the user
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <returns>UserInfo object containing information about the current user</returns>
        private UserInfo GetUser(string sROUserID)
        {
            lock (_alLock.SyncRoot)
            {
                UserInfo ui = null;

                if (!_dictUsers.TryGetValue(sROUserID, out ui))
                {
                    try
                    {
                        ui = new UserInfo(sROUserID, ROWebTools);

                        _dictUsers.Add(sROUserID, ui);
                    }

                    catch (Exception ex)
                    {
                        ROWebTools.LogMessage(eROMessageLevel.Error, "Error connecting to existing services..." + ex.ToString() + ex.StackTrace, sROUserID);
                        throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));

                    }
                }

                return ui;
            }
        }

        /// <summary>
        /// Removes the user from the collection
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        private void RemoveUser(string sROUserID)
        {
            lock (_alLock.SyncRoot)
            {
                _dictUsers.Remove(sROUserID);
            }
        }

        /// <summary>
        /// Retrieves an instance of the SessionInfo class for the user and Session
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="bIgnoreConnected">Identifies if the existance of a valid connection to the RO Client is to be ignored</param>
        /// <returns>SessionInfo object containing information about the RO Client session</returns>
        private SessionInfo GetUserSession(string sROUserID, string sROSessionID, bool bIgnoreConnected = false)
        {
            string sProcessDescription = null;
            bool bConnected = false;
            SessionInfo si = null;
            UserInfo ui = GetUser(sROUserID);
            if (ui == null)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Unable to find Error UserInfo in GetUserSession", sROUserID, sROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
            else
            {
                si = ui.GetSession(sROUserID, sROSessionID, ref sProcessDescription);
                if (si != null)
                {
                    bConnected = si.KeepAlive();
                }
            }

            if (si == null
                || (!bConnected && !bIgnoreConnected))
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error in GetUserSession", sROUserID, sROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
            return si;
        }

        /// <summary>
        /// Returns the Job ID
        /// </summary>
        /// <returns>The job id</returns>
        public string GetJobID()
        {
            iJobNumber++;
            return string.Format("Job ID: {0}", iJobNumber);
        }
    }
  
}

