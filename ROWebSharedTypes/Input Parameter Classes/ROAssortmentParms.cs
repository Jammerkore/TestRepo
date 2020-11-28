using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROAssortmentPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROAssortmentProperties _rOAssortmentPropertiesParms;

        public ROAssortmentPropertiesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROAssortmentProperties rOAssortmentPropertiesParms) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _rOAssortmentPropertiesParms = rOAssortmentPropertiesParms;
        }

        public ROAssortmentProperties ROAssortmentProperties { get { return _rOAssortmentPropertiesParms; } }

    };

    [DataContract(Name = "ROAssortmentAllocationActionParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentAllocationActionParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eAssortmentReviewTabType _assortmentTabtype;  

        [DataMember(IsRequired = true)]
        private eAllocationActionType _allocationActionType;

        [DataMember(IsRequired = true)]
        private ArrayList _headerKeys;

        [DataMember(IsRequired = true)]
        private int _workflowKey;

        [DataMember(IsRequired = true)]
        private eMethodType _methodType;

        [DataMember(IsRequired = true)]
        private int _methodKey;


        public ROAssortmentAllocationActionParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            eAssortmentReviewTabType assortmentTabType, eAllocationActionType allocationActionType = eAllocationActionType.None, ArrayList headerKeys = null,
            int workflowKey = Include.Undefined, eMethodType methodType = eMethodType.NotSpecified, int methodKey = Include.Undefined) :
           base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _assortmentTabtype = assortmentTabType;
            _allocationActionType = allocationActionType;
            _headerKeys = headerKeys;
            _workflowKey = workflowKey;
            _methodType = methodType;
            _methodKey = methodKey;
        }

        public eAllocationActionType AllocationActionType { get { return _allocationActionType; } }

        public eAssortmentReviewTabType AssortmentTabType { get { return _assortmentTabtype; } }

        public ArrayList Headerkeys { get { return _headerKeys; } }

        public int WorkflowKey { get { return _workflowKey; } }

        public eMethodType MethodType { get { return _methodType; } }

        public int MethodKey { get { return _methodKey; } }
    }

    [DataContract(Name = "ROAssortmentActionParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentActionParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROAssortmentAction _rOAssortmentActionParms;

        public ROAssortmentActionParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            ROAssortmentAction rOAssortmentActionParms) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _rOAssortmentActionParms = rOAssortmentActionParms;
        }

        public ROAssortmentAction ROAssortmentProperties { get { return _rOAssortmentActionParms; } }

    };

    [DataContract(Name = "ROAssortmentReviewOptionsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentReviewOptionsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROAssortmentReviewOptions _rOAssortmentReviewOptions;

        public ROAssortmentReviewOptionsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            ROAssortmentReviewOptions rOAssortmentReviewOptions) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _rOAssortmentReviewOptions = rOAssortmentReviewOptions;
        }

        public ROAssortmentReviewOptions ROAssortmentReviewOptions { get { return _rOAssortmentReviewOptions; } }

    };

    [DataContract(Name = "ROAssortmentReviewSaveOptionsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentReviewSaveOptionsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _view;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeAttribute;

        [DataMember(IsRequired = true)]
        private bool _saveAssortmentValues;

        public ROAssortmentReviewSaveOptionsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
           KeyValuePair<int, string> kvView = default(KeyValuePair<int, string>),
           KeyValuePair<int, string> kvStoreAttribute = default(KeyValuePair<int, string>),
           bool bSaveAsrtValues = false) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _view = kvView;
            _storeAttribute = kvStoreAttribute;
            _saveAssortmentValues = bSaveAsrtValues;
        }

        public bool SaveAssortmentValues
        {
            get { return _saveAssortmentValues; }

        }

        public KeyValuePair<int, string> StoreAttribute { get { return _storeAttribute; } }
        public KeyValuePair<int, string> View
        {
            get { return _view; }
        }

    }

}
