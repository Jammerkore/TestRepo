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
    class UserInfo
    {
        private string _sROUserID;
        private Dictionary<string, SessionInfo> _dictSessions;
        private ROWebTools _ROWebTools;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public UserInfo(string sROUserID, ROWebTools ROWebTools)
        {
            _sROUserID = sROUserID;
            _dictSessions = new Dictionary<string, SessionInfo>();
            _ROWebTools = ROWebTools;
        }

        public string sROUserID { get { return _sROUserID; } }

        public Dictionary<string, SessionInfo> Sessions
        {
            get { return _dictSessions; }
        }

        public SessionInfo GetSession(string sROUserID, string sROSessionID, ref string sProcessDescription)
        {
            SessionInfo si = null;
            if (!_dictSessions.TryGetValue(sROSessionID, out si))
            {

            }

            return si;
        }

        public SessionInfo AddSession(string sROUserID, string sPassword, string sROSessionID, string sMachineName, string sPort, bool bIsSecondaryServer, string sServer, ref string sProcessDescription, ref eROConnectionStatus connectionStatus)
        {
            SessionInfo si;
            if (!_dictSessions.TryGetValue(sROSessionID, out si))
            {
                try
                {
                    si = new SessionInfo(sROUserID, sROSessionID, sMachineName, sPort, sServer, _ROWebTools); 

                    if (!bIsSecondaryServer)
                    {
                        connectionStatus = si.ConnectToServices(sPassword, ref sProcessDescription);
                    }
                    _dictSessions.Add(sROSessionID, si);
                }

                catch (Exception ex)
                {
                    _ROWebTools.LogMessage(eROMessageLevel.Error, "Error creating SessionInfo " + ex.ToString() + ex.StackTrace);
                    throw new FaultException(_ROWebTools.GetExceptionReason(), new FaultCode(_ROWebTools.GetExceptionCode()));

                }
            }

            return si;
        }

        public void RemoveSessionInfo(string sROSessionID)
        {
            SessionInfo si;
            if (_dictSessions.TryGetValue(sROSessionID, out si))
            {
                try
                {
                    _dictSessions.Remove(sROSessionID);
                }

                catch (Exception ex)
                {
                    string faultDescription = "Error removing SessionInfo" + ex.ToString() + ex.StackTrace;
                    throw new FaultException(faultDescription, new FaultCode(_ROWebTools.GetExceptionCode()));

                }
            }
        }
    }
}
