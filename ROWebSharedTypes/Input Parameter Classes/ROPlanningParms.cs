using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROPlanningViewDetailsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningViewDetailsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROPlanningViewDetails _viewDetails;

        public ROPlanningViewDetailsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROPlanningViewDetails ROPlanningViewDetails) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _viewDetails = ROPlanningViewDetails;
        }

        public ROPlanningViewDetails ROPlanningViewDetails { get { return _viewDetails; } }

    }

}
