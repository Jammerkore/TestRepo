using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;

using Logility.ROWebCommon;
using Logility.ROServices;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    /// <summary>
    /// Use to control RO Web Console activity
    /// </summary>
    public class ROWebManager
    {
        //=======
        // FIELDS
        //=======

        private ROWebTools _ROWebTools;
        private Stack<string> _stkAvailablePorts = null;
        private ArrayList _alManagerLock = new ArrayList(); //setup the lock;
        private string ROWebService;
        private List<string> lstSecondaryServers = null;

        //=============
        // CONSTRUCTORS
        //=============

        public ROWebManager (ROWebTools RoWebTools)
        {
            _ROWebTools = RoWebTools;
            ROWebService = ConfigurationManager.AppSettings["ROWebService"];
        }

        //===========
        // PROPERTIES
        //===========

		// BEGIN TT#1156-MD CTeegarden - add app config setting for stale connection timeout
        public int iStaleConnectionTimeoutInSeconds 
        { 
            get 
            { 
                return GetIntAppSetting("StaleConnectionTimeoutSeconds", 30);
            }
        }
		// END TT#1156-MD CTeegarden - add app config setting for stale connection timeout

        private Stack<string> AvailablePorts
        {
            get
            {
                if (_stkAvailablePorts == null)
                {
                    LoadAvailablePorts();
                }
                return _stkAvailablePorts;
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
                return _ROWebTools;
            }
        }

        //========
        // METHODS
        //========

        private void LoadAvailablePorts()
        {
            lock (_alManagerLock.SyncRoot)
            {
                _stkAvailablePorts = new Stack<string>();
                string sPort = ConfigurationManager.AppSettings["Port"];
                if (sPort == null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Warning, "Port parameter not found. Server will not be available to host RO Web Client.");
                    return;
                }
                string[] ports = sPort.Split(';');
                foreach (string port in ports)
                {
                    // handle range
                    if (port.Contains("-"))
                    {
                        string[] portRange = port.Split('-');
                        int iFromPort = Convert.ToInt32(portRange[0].Trim());
                        int iToPort = Convert.ToInt32(portRange[1].Trim());
                        for (int p = iFromPort; p <= iToPort; p++)
                        {
                            ROWebTools.LogMessage(eROMessageLevel.Information, "Adding available port " + p.ToString());
                            _stkAvailablePorts.Push(p.ToString());
                        }
                    }
                    else
                    {
                        ROWebTools.LogMessage(eROMessageLevel.Information, "Adding available port " + port);
                        _stkAvailablePorts.Push(port.Trim());
                    }
                }
            }
        }

        public void LoadSecondaryServers()
        {
            lstSecondaryServers = new List<string>();
            string secondaryServers = ConfigurationManager.AppSettings["SecondaryServers"].ToUpper();
            string[] servers = secondaryServers.Split(';');
            foreach (string server in servers)
            {
                lstSecondaryServers.Add(server);
            }
        }
		
        private int GetIntAppSetting(string sSettingName, int defaultValue)
        {
            string sValue = ConfigurationManager.AppSettings[sSettingName];

            if (string.IsNullOrEmpty(sValue))
            {
                return defaultValue;
            }

            int retVal = defaultValue;

            if (!Int32.TryParse(sValue, out retVal))
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public string GetAvailablePort(bool bPeek = false)
        {
            string sPort = null;
            lock (_alManagerLock.SyncRoot)
            {
                if (AvailablePorts.Count > 0)
                {
                    if (bPeek)  // Get port but do not remove it
                    {
                        sPort = AvailablePorts.Peek();
                    }
                    else
                    {
                        sPort = AvailablePorts.Pop();
                    }
                }
            }

            return sPort;
        }

        public void AddAvailablePort(string sPort)
        {
            lock (_alManagerLock.SyncRoot)
            {
                AvailablePorts.Push(sPort);
                ROWebTools.LogMessage(eROMessageLevel.Information, "Adding available port " + sPort);
            }
        }

        /// <summary>
        /// Starts a client web host console process
        /// </summary>
        /// <param name="sPort">The port for the RO Wev Console</param>
        public bool StartROWebHost(string sPort)
        {
            ROWebTools.LogMessage(eROMessageLevel.Information, "Starting RO Web Clients on port " + sPort, ROWebTools.ROUserID, ROWebTools.ROSessionID);
            if (ROWebService == null)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ROWebService " + ROWebService + " was not found in the configuration file.", ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }

            Process ROWebHost = new Process();

            ROWebHost.StartInfo.FileName = ROWebService;

            ROWebHost.StartInfo.Arguments = sPort;

            ROWebHost.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ROWebHost.Start();

            return true;

        }

        public void KillAllROWebHost(string sReasonMessage)
        {
            try
            {
                if (sReasonMessage != null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Information, sReasonMessage, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                }
                ROWebTools.LogMessage(eROMessageLevel.Information, "Stopping all RO Web Clients.", ROWebTools.ROUserID, ROWebTools.ROSessionID);
                Process[] ROWebHostByName = Process.GetProcessesByName("ROWebHost");
                foreach (Process ROWebHostProces in ROWebHostByName)
                {
                    ROWebHostProces.Kill();
                }
            }
            catch (Exception ex)
            {
                ROWebTools.WriteWindowsEventViewerEntry("Error stopping all RO Web Clients..." + ex.ToString() + ex.StackTrace, EventLogEntryType.Error);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Called by primary server to determine server usage
        /// </summary>
        /// <returns>double representing the availabilty of the server</returns>
        public double GetMachineAvailability(out string sPort)
        {
            double availability = 0;
            sPort = GetAvailablePort(bPeek:true);
            if (sPort == null)
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Port is not available.", ROWebTools.ROUserID, ROWebTools.ROSessionID);
                return 0;
            }

            float cpuUsage; 
            float availableMemory;
            PerformanceCounter theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter theMemCounter = new PerformanceCounter("Memory", "Available MBytes");
            cpuUsage = 0.00F;
            availableMemory = 0.00F;

            cpuUsage = theCPUCounter.NextValue();
            availableMemory = theMemCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            cpuUsage = theCPUCounter.NextValue();
            availableMemory = theMemCounter.NextValue();

            availability = (100 - cpuUsage) * availableMemory;
            ROWebTools.LogMessage(eROMessageLevel.Information, "Server " + Environment.MachineName + " availability is " + availability, ROWebTools.ROUserID, ROWebTools.ROSessionID);
            return availability;
        }

        /// <summary>
        /// Called by primary server to check all secondary servers
        /// </summary>
        /// <param name="sPort">The available port on the server</param>
        /// <returns></returns>
        public string DetermineServer(out string sPort)
        {
            double currentAvailability = 0;
            double availability = 0;
            string availableServer = null;
            sPort = null;

            foreach (string secondaryServer in lstSecondaryServers)
            {
                availability = GetMachineAvailability(secondaryServer, out sPort);
                ROWebTools.LogMessage(eROMessageLevel.Information, "Server " + secondaryServer + " availability is " + availability, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                if (availability > currentAvailability)
                {
                    availableServer = secondaryServer;
                }
            }
            if (availableServer != null)
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Server " + availableServer + " is selected.", ROWebTools.ROUserID, ROWebTools.ROSessionID);
            }

            return availableServer;
        }


        /// <summary>
        /// Calls running job service to determine if another RO Web Console could be executed
        /// </summary>
        /// <param name="sServer">Server Name</param>
        /// <returns></returns>
        public double GetMachineAvailability(string sServer, out string sPort)
        {
            double availability = 0;
            sPort = null;
            try
            {
                if (sServer.ToUpper().Contains(Environment.MachineName.ToUpper())
                    || sServer.ToUpper() == "LOCALHOST")
                {
                    availability = GetMachineAvailability(out sPort);
                }
                else
                {
                    ChannelFactory<IROWebJobService> factory = GetChannelFactory(sServer);

                    IROWebJobService js = factory.CreateChannel();
                    availability = js.GetMachineAvailability(out sPort);
                    factory.Close();
                }
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                _ROWebTools.LogMessage(eROMessageLevel.Severe, "RO Job Service " + sServer + " is not responding.  " + Environment.NewLine + ex.ToString());
            }
            catch (Exception ex)
            {
                _ROWebTools.LogMessage(eROMessageLevel.Severe, "RO Job Service " + sServer + " is not responding.  " + Environment.NewLine + ex.ToString());
            }

            return availability;
        }

        public eROConnectionStatus StartROWebHost(string sServer, string sROUserID, string sPassword, string sROSessionID, string sPort, out string sProcessDescription)
        {
            ROWebTools.LogMessage(eROMessageLevel.Information, "Starting RO Web Client on port " + sPort + " for session " + sROSessionID, ROWebTools.ROUserID, ROWebTools.ROSessionID);
            ChannelFactory<IROWebJobService> factory = GetChannelFactory(sServer);

            IROWebJobService js = factory.CreateChannel();
            eROConnectionStatus connectionStatus = js.StartROWebHost(sROUserID, sPassword, sROSessionID, sPort, out sProcessDescription);
            factory.Close();

            return connectionStatus;
        }

        public bool DisconnectSession(string sServer, string sROUserID, string sROSessionID)
        {
            ROWebTools.LogMessage(eROMessageLevel.Information, "Disconnecting RO Web Client session " + sROSessionID, ROWebTools.ROUserID, ROWebTools.ROSessionID);
            ChannelFactory<IROWebJobService> factory = GetChannelFactory(sServer);

            IROWebJobService js = factory.CreateChannel();
            js.DisconnectSession(sROUserID, sROSessionID);
            factory.Close();

            return true;
        }

        private ChannelFactory<IROWebJobService> GetChannelFactory(string sServer)
        {
            ROWebTools.LogMessage(eROMessageLevel.Information, "Establishing endpoint for server " + sServer, ROWebTools.ROUserID, ROWebTools.ROSessionID);
            EndpointAddress endpoint = new EndpointAddress("net.tcp://" + sServer + "/MIDRetailJobService");
            ChannelFactory<IROWebJobService> factory = new ChannelFactory<IROWebJobService>("Logility.ROServices.ROWebJobService", endpoint);

            return factory;
        }

    }
}
