using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROAssortmentView", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentView
    {
        [DataMember(IsRequired = true)]
        private int _viewRID;
        [DataMember(IsRequired = true)]
        private int _userRID;
        [DataMember(IsRequired = true)]
        private int _layoutID;
        [DataMember(IsRequired = true)]
        private string _viewID;
        [DataMember(IsRequired = true)]
        private string _rawViewID;
        [DataMember(IsRequired = true)]
        private string _showDetails;
        [DataMember(IsRequired = true)]
        private string _groupBy;
        [DataMember(IsRequired = true)]
        private string _groupBySecondary;
        [DataMember(IsRequired = true)]
        private bool _isSequential;

        public int ViewRID { get { return _viewRID; } }

        public int UserRID { get { return _userRID; } }

        public int LayoutID { get { return _layoutID; } }

        public string ViewID { get { return _viewID; } }

        public string RawViewID { get { return _rawViewID; } }

        public string ShowDetails { get { return _showDetails; } }

        public string GroupBy { get { return _groupBy; } }

        public string GroupBySecondary { get { return _groupBySecondary; } }

        public bool IsSequential { get { return _isSequential; } }

        public ROAssortmentView(int viewRID, int userRID, int layoutID, string viewID, string rawViewID, string showDetails, string groupBy, string groupBySecondary, bool isSequential)
        {
            _viewRID = viewRID;
            _userRID = userRID;
            _layoutID = layoutID;
            _viewID = viewID;
            _rawViewID = rawViewID;
            _showDetails = showDetails;
            _groupBy = groupBy;
            _groupBySecondary = groupBySecondary;
            _isSequential = IsSequential;
        }

    }

    [DataContract(Name = "ROAssortmentPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROAssortmentProperties rOAssortmentProperties;

        public ROAssortmentPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAssortmentProperties ROAssortmentProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            rOAssortmentProperties = ROAssortmentProperties;
        }

        public ROAssortmentProperties ROAssortmentProperties { get { return rOAssortmentProperties; } }

    };

    [DataContract(Name = "ROAssortmentActionsOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAssortmentActionsOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROAssortmentAction _rOAssortmentAction;

        public ROAssortmentActionsOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAssortmentAction ROAssortmentAction) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _rOAssortmentAction = ROAssortmentAction;
        }

        public ROAssortmentAction ROAssortmentAction { get { return _rOAssortmentAction; } }

    };

}