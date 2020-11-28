using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

using Logility.ROWebCommon; 
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IROWebConsoleService" in both code and config file together.
    [ServiceContract]
    public interface IROWebConsoleService
    {
        /// <summary>
        /// Notifies the service that the client is still alive. Throws a fault exception on failure
        /// </summary>
        /// 
        [OperationContract]
        bool KeepAlive();  

        /// <summary>
        /// Connects to services
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="processDescription">A string containing a description of the processing</param>
        /// <returns>A value identifying if able to connect to services.</returns>
        /// <remarks>Only needs to be called once.</remarks>
        [OperationContract]
        eROConnectionStatus ConnectToServices(string sROUserID, string sPassword, string sROSessionID, out string processDescription);

        [OperationContract]
        void DisconnectSession();

        /// <summary>
        /// Returns the settings for connecting to the existing services
        /// <para>Used to help when debugging service connection failures.</para>
        /// </summary>
        /// <param name="appSetControlServer">The name of server hosting the Control Service</param>
        /// <param name="appSetLocalStoreServer">The name of server hosting the Store Service</param>
        /// <param name="appSetLocalHierarchyServer">The name of server hosting the Hierarchy Service</param>
        /// <param name="appSetLocalApplicationServer">The name of server hosting the Application Service</param>
        [OperationContract]
        void GetServiceServerInfo(out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer);

        /// <summary>
        /// Process a request
        /// </summary>
        /// <param name="Parms">parameter that needs to passed into funtion being called</param>
        [OperationContract]
        ROOut ProcessRequest(ROParms Parms);

    }
}
