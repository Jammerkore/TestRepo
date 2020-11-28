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
    /// Provide Store interfaces to the RO application
    /// </summary>
   [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IROWebStores
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID">Current User ID</param>
        /// <param name="sEnvironment">Current sEnvironment name</param>
        /// <param name="sStoreID">The store for which information is requested</param>
        /// <returns>A datatable of store properties.</returns>
        [OperationContract]
        DataTable GetStoreProfile(string sUserID, string sEnvironment, string sStoreID);
    }
}
