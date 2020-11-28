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
    /// Provide Global interfaces to the RO application
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebGlobal
    {
        /// <summary>
        /// Retrieves the global settings for all tabs
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current environment name</param>
        [OperationContract]
        DataSet GetGlobalDefaults(string sUserID, string sEnvironment);
        
        /// <summary>
        /// Updates the global settings in the database.  Ancillary data tables are ignored.
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current environment name</param>
        /// <param name="dsDefaults">The set of global defaults</param>
        [OperationContract]
        void UpdateGlobalDefaults(string sUserID, string sEnvironment, DataSet dsDefaults);
        
    }
}
