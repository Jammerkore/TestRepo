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

    [DataContract(Name = "ROAllocationWorklistLastDataParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistLastDataParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private int _iViewRID;
        [DataMember(IsRequired = true)]
        private int _iFilterRID;

        public ROAllocationWorklistLastDataParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, int iViewRID, int iFilterRID) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)

        {
            _iViewRID = iViewRID;
            _iFilterRID = iFilterRID;

        }

        public int iViewRID { get { return _iViewRID; } }
        public int iFilterRID { get { return _iFilterRID; } }
    }

    [DataContract(Name = "ROAllocationReviewOptionsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewOptionsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ArrayList _ListValues;

        [DataMember(IsRequired = true)]
        private int _groupBy;

        [DataMember(IsRequired = true)]
        private eAllocationActionType _allocationActionType;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _view;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = false)]
        private KeyValuePair<int, string> _filter;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeAttribute;

        [DataMember(IsRequired = true)]
        private bool _viewIsSequential;

        [DataMember(IsRequired = true)]
        private int _secondaryGroupBy;

        [DataMember(IsRequired = true)]
        private eAllocationSelectionViewType _viewType;

        public ROAllocationReviewOptionsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
           ArrayList ListValues, int GroupBy, eAllocationActionType AllocationActionType = eAllocationActionType.None,
           KeyValuePair<int, string> kvView = default(KeyValuePair<int, string>),
           KeyValuePair<int, string> kvAttributeSet = default(KeyValuePair<int, string>),
           KeyValuePair<int, string> kvFilter = default(KeyValuePair<int, string>),
           KeyValuePair<int, string> kvStoreAttribute = default(KeyValuePair<int, string>),
           bool kvViewIsSequential = true,
           int eSecondaryGroupBy = 0,
           eAllocationSelectionViewType viewType = eAllocationSelectionViewType.None) :
          base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ListValues = ListValues;
            _groupBy = GroupBy;
            _allocationActionType = AllocationActionType;
            _view = kvView;
            _attributeSet = kvAttributeSet;
            _filter = kvFilter;
            _storeAttribute = kvStoreAttribute;
            _viewIsSequential = kvViewIsSequential;
            _secondaryGroupBy = eSecondaryGroupBy;
            _viewType = viewType;
        }

        public ArrayList ListValues { get { return _ListValues; } }

        public int GroupBy { get { return _groupBy; } }

        public eAllocationActionType AllocationActionType { get { return _allocationActionType; } }

        public KeyValuePair<int, string> View
        {
            get { return _view; }
            set { _view = value; }
        }

        public KeyValuePair<int, string> AttributeSet { get { return _attributeSet; } set { _attributeSet = value; } }

        public KeyValuePair<int, string> Filter { get { return _filter; } }

        public KeyValuePair<int, string> StoreAttribute { get { return _storeAttribute; } set { _storeAttribute = value; } }

        public bool ViewIsSequential { get { return _viewIsSequential; } }

        public int SecondaryGroupBy { get { return _secondaryGroupBy; } }

        public eAllocationSelectionViewType ViewType { get { return _viewType; } set { _viewType = value; } }

        public bool IsVelocity { get { return _viewType == eAllocationSelectionViewType.Velocity; } }

    }

    [DataContract(Name = "ROAllocationReviewViewDetailsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewViewDetailsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROAllocationReviewViewDetails _viewDetails;

        public ROAllocationReviewViewDetailsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROAllocationReviewViewDetails ROAllocationReviewViewDetails) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _viewDetails = ROAllocationReviewViewDetails;
        }

        public ROAllocationReviewViewDetails ROAllocationReviewViewDetails { get { return _viewDetails; } }

    }

    

    [DataContract(Name = "ROAllocationWorklistViewDetailsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistViewDetailsParms : ROParms
    {

        [DataMember(IsRequired = true)]
        private ROAllocationWorklistViewDetails _viewDetails;

        public ROAllocationWorklistViewDetailsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            ROAllocationWorklistViewDetails ROAllocationWorklistViewDetails) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _viewDetails = ROAllocationWorklistViewDetails;
        }

        public ROAllocationWorklistViewDetails ROAllocationWorklistViewDetails { get { return _viewDetails; } }

    }

    
}
