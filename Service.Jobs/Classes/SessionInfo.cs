using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

using Logility.ROWeb;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROServices
{
    class SessionInfo
    {
        string _sROUserID;
        string _sROSessionID;
        IROWebConsoleService wcfClient = null;
        ROWebTools _ROWebTools;
        string _sMachineName;
        string _sPort;
        string _sServer;
        private DateTime _keepAliveTimestamp = DateTime.Now;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public SessionInfo(string sROUserID, string sROSessionID, string sMachine, string sPort, string sServer, ROWebTools ROWebTools)
        {
            _sROUserID = sROUserID;
            _sROSessionID = sROSessionID;
            _ROWebTools = ROWebTools;
            _sMachineName = sMachine;
            _sPort = sPort;
            _sServer = sServer;
        }

        public string UserID { get { return _sROUserID; } }
        public string SessionID { get { return _sROSessionID; } }
        public string Address { get { return _sMachineName + ":" + _sPort; } }
        public string Server { get { return _sServer; } }
        public string MachineName { get { return _sMachineName; } }
        public string Port { get { return _sPort; } }
        public eROConnectionStatus ConnectToServices(string sPassword, ref string sProcessDescription)
        {
            EndpointAddress myEndpoint = new EndpointAddress("net.tcp://" + _sMachineName + ":" + _sPort + "/ROWebConsoleService");
            ChannelFactory<IROWebConsoleService> myChannelFactory = new ChannelFactory<IROWebConsoleService>("ROWebConsoleServiceClientEndpoint", myEndpoint);

            wcfClient = myChannelFactory.CreateChannel();
            return wcfClient.ConnectToServices(_sROUserID, sPassword, _sROSessionID, out sProcessDescription);
            
        }

        public bool SessionPing()
        {
            bool bConnected = false;

            if (wcfClient != null)
            {
                try
                {
                    bConnected = wcfClient.KeepAlive();
                }
                catch (System.ServiceModel.CommunicationException)
                {
                    bConnected = false;
                    _ROWebTools.LogMessage(eROMessageLevel.Severe, "RO client not responding for user " + UserID + " and session " + SessionID);
                }
            }

            return bConnected;
        }

        public bool KeepAlive()
        {
            bool bConnected = SessionPing();

            _keepAliveTimestamp = DateTime.Now;

            return bConnected;
        }

        public bool ConnectionIsAlive(int timeoutInSeconds)  
        {
            // if StaleConnectionTimeoutSeconds is zero, never timeout the session.
            if (timeoutInSeconds == 0)
            {
                return true;
            }

            long intervalInMillis = (DateTime.Now.Ticks - _keepAliveTimestamp.Ticks) / TimeSpan.TicksPerMillisecond;

            return intervalInMillis < (timeoutInSeconds * 1000);
        }

        public void DisconnectSession()
        {
            if (wcfClient == null)
            {
                return;
            }

            if (KeepAlive())
            {
                wcfClient.DisconnectSession();
            }

            wcfClient = null;
        }

        public ROOut ProcessRequest(ROParms Parms)
        {
            return wcfClient.ProcessRequest(Parms);
        }

        public void GetServiceServerInfo(out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer)
        {
            try
            {
                wcfClient.GetServiceServerInfo(out appSetControlServer, out appSetLocalStoreServer, out appSetLocalHierarchyServer, out appSetLocalApplicationServer);
            }
            catch (Exception ex)
            {
                string faultDescription = "Error getting service server info..." + ex.ToString() + ex.StackTrace;
                throw new FaultException(faultDescription, new FaultCode(_ROWebTools.GetExceptionCode()));
            }
        }
    }
}
