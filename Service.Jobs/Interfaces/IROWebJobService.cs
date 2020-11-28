using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace Logility.ROServices
{
    /// <summary>
    /// The BTB interface
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebJobService : IROWebHierarchy, IROWebStores, IROWebPlanning, IROWebGlobal
    {
        /// <summary>
        /// Gets a new job ID
        /// </summary>
        /// <returns>A string containing the new job ID.</returns>
        [OperationContract]
        string GetJobID();

        /// <summary>
        /// Tests the event log
        /// </summary>
        /// <returns>OK</returns>
        [OperationContract]
        string TestEventLog();

        /// <summary>
        /// Connects to services
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="sProcessDescription">A string containing a description of the processing</param>
        /// <returns>True, if successful.  False if unable to connect to services.</returns>
        /// <remarks>Only needs to be called once.</remarks>
        [OperationContract]
        bool ConnectToServices(string sUserID, string sEnvironment, out string sProcessDescription);

        // BEGIN TT#1156-MD CTeegarden Add KeepAlive Functionality
        /// <summary>
        /// Notifies the service that the client is still alive. Throws a fault exception on failure
        /// </summary>
        /// 
        [OperationContract]
        bool KeepAlive(string sUserID, string sEnvironment);
		// END TT#1156-MD CTeegarden Add KeepAlive Functionality

        /// <summary>
        /// Disconnects user from service
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        [OperationContract]
        void DisconnectUser(string sUserID);

        /// <summary>
        /// Disconnects sEnvironment from user
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        [OperationContract]
        void DisconnectEnvironment(string sUserID, string sEnvironment);

        /// <summary>
        /// Returns the settings for connecting to the existing services
        /// <para>Used to help when debugging service connection failures.</para>
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="appSetControlServer">The name of server hosting the Control Service</param>
        /// <param name="appSetLocalStoreServer">The name of server hosting the Store Service</param>
        /// <param name="appSetLocalHierarchyServer">The name of server hosting the Hierarchy Service</param>
        /// <param name="appSetLocalApplicationServer">The name of server hosting the Application Service</param>
        [OperationContract]
        void GetServiceServerInfo(string sUserID, string sEnvironment, out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer);
        
    }
}
