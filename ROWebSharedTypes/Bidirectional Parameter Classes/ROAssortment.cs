using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Logility.ROWebSharedTypes
{
    #region "Set Assortment Properties"

    public class AssortmentPropertiesBasis
    {
        public int HeaderRID { get; set; }
        public int BasicSequence { get; set; }
        public int HN_RID { get; set; }
        public int FV_RID { get; set; }
        public int CDR_RID { get; set; }
        public double Weight { get; set; }
        public string Merchandise { get; set; }
        public string Version { get; set; }
        public string HorizonDateRange { get; set; }
    }

    public class AssortmentPropertiesStoreGrades
    {
        public int HeaderRid { get; set; }
        public int StoreGradeSeq { get; set; }
        public int BoundaryIndex { get; set; }
        public int BoundaryUnits { get; set; }
        public string GradeCode { get; set; }
    }

    public class ROAssortmentProperties
    {
        public eAssortViewType viewType { get; set; }
        public string HeaderID { get; set; }
        public string HeaderDesc { get; set; }
        public int HeaderRid { get; set; }
        public long InstanceId { get; set; }
        public KeyValuePair<int, string> StoreAttributeText { get; set; }
        public int StoreAttributeRid { get; set; }
        public int GroupBy { get; set; }
        public int ViewRid { get; set; }
        public int VariableNumber { get; set; }
        public bool InclOnhand { get; set; }
        public bool InclIntransit { get; set; }
        public bool InclSimStores { get; set; }
        public bool InclCommitted { get; set; }
        public bool PercentReserveType { get; set; }
        public bool UnitsReserveType { get; set; }
        public eStoreAverageBy AverageBy { get; set; }
        public eGradeBoundary GradeBoundary { get; set; }
        public eAssortmentVariableType VariableType { get; set; }
        public string ReserveAmount { get; set; }
        public int AnchorNodeRid { get; set; }
        public string AnchorNodeText { get; set; }
        public int CalendarDateRangeRid { get; set; }
        public string CalendarDateRangeText { get; set; }
        public int BeginDayCalendarDateRangeRid { get; set; }
        public string BeginDayCalendarDateRangeText { get; set; }
        public DateTime AssortLastProcessDt { get; set; }
        public int AssortUserRid { get; set; }
        public eReserveType AssortReserveType { get; set; }
        public List<AssortmentPropertiesBasis> AssortmentPropertiesBasis { get; set; }
        public List<AssortmentPropertiesStoreGrades> AssortmentPropertiesStoreGrades { get; set; }
    }
    #endregion

    #region "Assortment Action"
    [DataContract(Name = "ROAssortmentAction", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentAction
    {
        [DataMember(IsRequired = true)]
        private eAssortmentActionType _assortmentActionType;
        [DataMember(IsRequired = true)]
        private eAssortmentReviewTabType _assortmentTabType; //(Matrix, Content, Characteristics tab)
        [DataMember(IsRequired = true)]
        private ROBaseAssortmentActionDetails _assortmentActionDetails;


        public ROAssortmentAction(eAssortmentActionType assortmentActionType, eAssortmentReviewTabType assortmentTabType, ROBaseAssortmentActionDetails assortmentActionDetails)
        {
            _assortmentActionType = assortmentActionType;
            _assortmentTabType = assortmentTabType;
            _assortmentActionDetails = assortmentActionDetails;
        }

        public eAssortmentActionType AssortmentActionType
        {
            get { return _assortmentActionType; }
        }

        public eAssortmentReviewTabType AssortmentTabType
        {
            get { return _assortmentTabType; }
        }

        public ROBaseAssortmentActionDetails AssortmentActionDetails
        {
            get { return _assortmentActionDetails; }
        }

    }

    [DataContract(Name = "ROAssortmentActionsGradeDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentActionsGradeDetails
    {
        [DataMember(IsRequired = true)]
        private int _gradeBoundaryIndex;
        [DataMember(IsRequired = true)]
        private string _gradeCode;
        [DataMember(IsRequired = true)]
        private double _gradeAverage;

        public ROAssortmentActionsGradeDetails(int gradeBoundaryIndex, string gradeCode, double gradeAverage)
        {
            _gradeBoundaryIndex = gradeBoundaryIndex;
            _gradeCode = gradeCode;
            _gradeAverage = gradeAverage;
        }

        public int GradeBoundaryIndex
        {
            get { return _gradeBoundaryIndex; }
        }

        public string GradeCode
        {
            get { return _gradeCode; }
        }

        public double GradeAverage
        {
            get { return _gradeAverage; }
        }
    }

    [DataContract(Name = "ROBaseAssortmentActionDetails", Namespace = "http://Logility.ROWeb/")]
    public abstract class ROBaseAssortmentActionDetails
    {
        [DataMember(IsRequired = true)]
        private List<ROAssortmentActionsGradeDetails> _gradeDetails;

        public ROBaseAssortmentActionDetails()
        {
            _gradeDetails = new List<ROAssortmentActionsGradeDetails>();
        }

        public List<ROAssortmentActionsGradeDetails> GradeDetails
        {
            get { return _gradeDetails; }
        }
    }

    [DataContract(Name = "ROAssortmentActionsCreateplaceHolders", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentActionsCreateplaceHolders : ROBaseAssortmentActionDetails
    {
        [DataMember(IsRequired = true)]
        private List<int> _averageUnit;

        public ROAssortmentActionsCreateplaceHolders(List<int> averageUnit)
        {
            _averageUnit = averageUnit;
        }

        public List<int> AverageUnit
        {
            get { return _averageUnit; }
        }
    }

    [DataContract(Name = "ROAssortmentActionsSpreadAverage", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentActionsSpreadAverage : ROBaseAssortmentActionDetails
    {
        [DataMember(IsRequired = true)]
        private eSpreadAverage _spreadOption;
        [DataMember(IsRequired = true)]
        private eIndexToAverageReturnType _spreadType;
        [DataMember(IsRequired = true)]
        private double _average;
        [DataMember(IsRequired = true)]
        private List<ROAssortmentActionsGradeDetails> _gradeDetails;
        public ROAssortmentActionsSpreadAverage(eSpreadAverage spreadOption, eIndexToAverageReturnType spreadType, double average, List<ROAssortmentActionsGradeDetails> gradeDetails)
        {
            _spreadOption = spreadOption;
            _spreadType = spreadType;
            _average = average;
            _gradeDetails = gradeDetails;

        }

        public eSpreadAverage SpreadOption
        {
            get { return _spreadOption; }
        }

        public eIndexToAverageReturnType SpreadType
        {
            get { return _spreadType; }
        }
        public double Average
        {
            get { return _average; }
        }

        public List<ROAssortmentActionsGradeDetails> ActionsGradeDetails
        {
            get { return _gradeDetails; }
        }
    }

    #endregion

    #region "Assortment Review Options"

    [DataContract(Name = "ROAssortmentReviewOptions", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentReviewOptions
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _view;

        [DataMember(IsRequired = true)]
		// changing type will cause build issue.  This will be scheduled for a later date
        //private eAllocationAssortmentViewGroupBy _groupBy;
		private int _groupBy;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attribute;

        public KeyValuePair<int, string> View
        {
            get { return _view; }
            set { _view = value; }
        }

        public KeyValuePair<int, string> AttributeSet { get { return _attributeSet; } }
        //public eAllocationAssortmentViewGroupBy GroupBy { get { return _groupBy; } }
		public int GroupBy { get { return _groupBy; } }
        public KeyValuePair<int, string> StoreAttribute { get { return _attribute; } }


        //public ROAssortmentReviewOptions(eAllocationAssortmentViewGroupBy GroupBy = eAllocationAssortmentViewGroupBy.None,
		public ROAssortmentReviewOptions(int GroupBy = 0,
            KeyValuePair<int, string> kvView = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> kvAttributeSet = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> kvAttribute = default(KeyValuePair<int, string>))
        {
            _view = kvView;
            _groupBy = GroupBy;
            _view = kvView;
            _attributeSet = kvAttributeSet;
            _attribute = kvAttribute;
        }
    }

    #endregion

    #region "Assortment Update Content Characteristics"
    [DataContract(Name = "ROAssortmentUpdateContentCharacteristicsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentUpdateContentCharacteristicsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private List<ROAllocationHeaderSummary> _rOAllocationHeaderSummaries;

        [DataMember(IsRequired = true)]
        private List<ROUpdateContent> _contentUpdateRequests = null;

        public ROAssortmentUpdateContentCharacteristicsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, List<ROAllocationHeaderSummary> rOAllocationHeaderSummaries) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _rOAllocationHeaderSummaries = rOAllocationHeaderSummaries;
        }

        /// <summary>
        /// Contains ROAllocationHeaderSummary objects for each header in the Assortment
        /// </summary>
        public List<ROAllocationHeaderSummary> ROAllocationHeaderSummary { get { return _rOAllocationHeaderSummaries; } }

        /// <summary>
        /// Contains ROUpdateContent objects for the entire assortment
        /// </summary>
        public List<ROUpdateContent> ContentUpdateRequests
        {
            get
            {
                if (_contentUpdateRequests == null)
                {
                    _contentUpdateRequests = new List<ROUpdateContent>();
                }
                return _contentUpdateRequests;
            }
        }
    };
    #endregion
}