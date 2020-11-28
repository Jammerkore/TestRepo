using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace Logility.ROServices
{
    /// <summary>
    /// Provide Hierarchy Interfaces to the RO application
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebHierarchy
    {

        /// <summary>
        /// Gets the node properties
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="sNodeID">The node id</param>
        /// <returns>A datatable of node properties.</returns>
        [OperationContract]
        DataTable GetNodeProperties(string sUserID, string sEnvironment, string sNodeID);
    }
}
