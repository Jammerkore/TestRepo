using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

using Logility.ROWebSharedTypes;

namespace Logility.ROServices
{
    /// <summary>
    /// The BTB interface
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebJobService
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
        /// Returns a value representing the availability to start an RO Web client
        /// </summary>
        /// <param name="sPort">The available port on the machine</param>
        /// <returns>A numeric value representing the availablility of the machine</returns>
        [OperationContract]
        double GetMachineAvailability(out string sPort);

        /// <summary>
        /// Starts the RO Client
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        /// <param name="sROSessionID">Current sROSessionID name</param>
        /// <param name="sPort">The port on which the RO Client is to be started</param>
        /// <returns>A value identifying if able to connect to services.</returns>
        [OperationContract]
        eROConnectionStatus StartROWebHost(string sROUserID, string sPassword, string sROSessionID, string sPort, out string sProcessDescription);

        /// <summary>
        /// Connects to services
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">Current sROSessionID name</param>
        /// <param name="sProcessDescription">A string containing a description of the processing</param>
        /// <returns>True, if successful.  False if unable to connect to services.</returns>
        /// <remarks>Only needs to be called once.</remarks>
        [ObsoleteAttribute("This method will soon be deprecated. Use ConnectToServicesWithStatus instead.")]
        [OperationContract]
        bool ConnectToServices(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription);

        /// <summary>
        /// Connects to services
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">Current sROSessionID name</param>
        /// <param name="sProcessDescription">A string containing a description of the processing</param>
        /// <returns>A value identifying if able to connect to services.</returns>
        /// <remarks>Only needs to be called once.</remarks>
        [OperationContract]
        eROConnectionStatus ConnectToServicesWithStatus(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription);

        // BEGIN TT#1156-MD CTeegarden Add KeepAlive Functionality
        /// <summary>
        /// Notifies the service that the client is still alive. Throws a fault exception on failure
        /// </summary>
        /// 
        [OperationContract]
        bool KeepAlive(string sROUserID, string sROSessionID);
		// END TT#1156-MD CTeegarden Add KeepAlive Functionality

        /// <summary>
        /// Disconnects user from service
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        [OperationContract]
        void DisconnectUser(string sROUserID);

        /// <summary>
        /// Disconnects sROSessionID from user
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        /// <param name="sROSessionID">Current sROSessionID name</param>
        [OperationContract]
        void DisconnectSession(string sROUserID, string sROSessionID);

        /// <summary>
        /// Returns the settings for connecting to the existing services
        /// <para>Used to help when debugging service connection failures.</para>
        /// </summary>
        /// <param name="sROUserID">Current User ID</param>
        /// <param name="sROSessionID">Current sROSessionID name</param>
        /// <param name="appSetControlServer">The name of server hosting the Control Service</param>
        /// <param name="appSetLocalStoreServer">The name of server hosting the Store Service</param>
        /// <param name="appSetLocalHierarchyServer">The name of server hosting the Hierarchy Service</param>
        /// <param name="appSetLocalApplicationServer">The name of server hosting the Application Service</param>
        [OperationContract]
        void GetServiceServerInfo(string sROUserID, string sROSessionID, out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer);

        /// <summary>
        /// Processes the requested function
        /// </summary>
        /// <param name="Parms">parameter that needs to passed into funtion being called</param>
        [OperationContract]
        ROOut ProcessRequest(ROParms Parms);

    }
}
