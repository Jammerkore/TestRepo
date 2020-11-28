using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROPlanningView", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningView
    {
        [DataMember(IsRequired = true)]
        private int _viewRID;
        [DataMember(IsRequired = true)]
        private int _userRID;
        [DataMember(IsRequired = true)]
        private string _viewID;
        [DataMember(IsRequired = true)]
        private int _groupByType;
        [DataMember(IsRequired = true)]
        private int _itemType;
        [DataMember(IsRequired = true)]
        private int _ownerUSerRID;
        [DataMember(IsRequired = true)]
        private string _displayID;

        public int ViewRID { get { return _viewRID; } }

        public int UserRID { get { return _userRID; } }

        public string ViewID { get { return _viewID; } }

        public int GroupByType { get { return _groupByType; } }

        public int ItemType { get { return _itemType; } }

        public int OwnerUserRID { get { return _ownerUSerRID; } }

        public string DisplayID { get { return _displayID; } }

        public ROPlanningView(int viewRID, int userRID, string viewID, int groupByType, int itemType, int ownerUserRID, string displayID)
        {
            _viewRID = viewRID;
            _userRID = userRID;
            _viewID = viewID;
            _groupByType = groupByType;
            _itemType = itemType;
            _ownerUSerRID = ownerUserRID;
            _displayID = displayID;
        }
    }

    [DataContract(Name = "ROOTSWorkflowStepOut", Namespace = "http://Logility.ROWeb/")]
    public class ROOTSWorkflowStepOut
    {

        #region Member Variables

        [DataMember(IsRequired = true)]
        private int _key;

        [DataMember(IsRequired = true)]
        private int _specificRID;

        [DataMember(IsRequired = true)]
        private double _tolerancePercent;
        [DataMember(IsRequired = true)]
        private int _rowPosition;

        [DataMember(IsRequired = true)]
        private int _action;

        [DataMember(IsRequired = true)]
        private int _methodRID;

        [DataMember(IsRequired = true)]
        private string _method;

        [DataMember(IsRequired = true)]
        private string _Mode;

        [DataMember(IsRequired = true)]
        private int _filter;

        [DataMember(IsRequired = true)]
        private string _review;

        [DataMember(IsRequired = true)]
        private int _variable;
        #endregion

        #region Constructor

        public ROOTSWorkflowStepOut(int iRowPosition, int iAction, int iMethodRID, string sMethod,
                int iFilter, string sReview, double dTolerancePercent, int variable, string mode)
        {
            _rowPosition = iRowPosition;
            _action = iAction;

            _methodRID = iMethodRID;
            _method = sMethod;

            _filter = iFilter;

            _review = sReview;

            _tolerancePercent = dTolerancePercent;

            _Mode = mode;
            _variable = variable;
        }
        #endregion

        #region Public Properties


        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public int RowPosition
        {
            get { return _rowPosition; }
            set { _rowPosition = value; }
        }

        public int Action
        {
            get { return _action; }
            set { _action = value; }
        }



        public int MethodRID
        {
            get { return _methodRID; }
            set { _methodRID = value; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }



        public int Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }



        public string Review
        {
            get { return _review; }
            set { _review = value; }
        }


        public int SpecificRID
        {
            get { return _specificRID; }
            set { _specificRID = value; }
        }

        public double TolerancePercent
        {
            get { return _tolerancePercent; }
            set { _tolerancePercent = value; }
        }



        public string Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public int Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        #endregion
    };   

    [DataContract(Name = "ROPlanningReviewSelectionPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningReviewSelectionPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROPlanningReviewSelectionProperties _ROPlanningReviewSelectionProperties;

        public ROPlanningReviewSelectionPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROPlanningReviewSelectionProperties ROPlanningReviewSelectionProperties) :
        base(ROReturnCode, sROMessage, ROInstanceID)

        { _ROPlanningReviewSelectionProperties = ROPlanningReviewSelectionProperties; }
        public ROPlanningReviewSelectionProperties ROPlanningReviewSelectionProperties
        {
            get

            { return _ROPlanningReviewSelectionProperties; }
        }
    };

    [DataContract(Name = "ROPlanningViewDetailsOut", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningViewDetailsOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROPlanningViewDetails _viewDetails;

        public ROPlanningViewDetailsOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROPlanningViewDetails ROPlanningViewDetails) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _viewDetails = ROPlanningViewDetails;
        }

        public ROPlanningViewDetails ROPlanningViewDetails { get { return _viewDetails; } }

    }
}
    



