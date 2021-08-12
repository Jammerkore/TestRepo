using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROUserInformationOut", Namespace = "http://Logility.ROWeb/")]
    public class ROUserInformationOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROUserInformation _ROUserInformation;

        public ROUserInformationOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROUserInformation ROUserInformation) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROUserInformation = ROUserInformation;
        }

        public ROUserInformation ROUserInformation { get { return _ROUserInformation; } }

    }

    [DataContract(Name = "ROCharacteristicsOut", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristicsOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROCharacteristicsProperties _ROCharacteristics;

        public ROCharacteristicsOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROCharacteristicsProperties ROCharacteristics) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROCharacteristics = ROCharacteristics;
        }

        public ROCharacteristicsProperties ROCharacteristics { get { return _ROCharacteristics; } }

    }

    [DataContract(Name = "ROModelPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROModelPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROModelProperties _ROModelProperties;

        public ROModelPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROModelProperties ROModelProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROModelProperties = ROModelProperties;
        }

        public ROModelProperties ROModelProperties { get { return _ROModelProperties; } }

    }

    [DataContract(Name = "ROActiveUserOut", Namespace = "http://Logility.ROWeb/")]
    public class ROActiveUserOut
    {

        #region MemberVariables

        [DataMember(IsRequired = true)]
        private string _userName;

        [DataMember(IsRequired = true)]
        private string _sessionID;

        [DataMember(IsRequired = true)]
        private string _server;

        [DataMember(IsRequired = true)]
        private string _machine;

        [DataMember(IsRequired = true)]
        private string _port;

        #endregion

        #region Constructor
        public ROActiveUserOut(string userName, string sessionID, string server, string machine, string port)
        {
            _userName = userName;
            _sessionID = sessionID;
            _server = server;
            _machine = machine;
            _port = port;
        }
        #endregion  

        #region Public Properties
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string SessionID
        {
            get { return _sessionID; }
            set { _sessionID = value; }
        }

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public string Machine
        {
            get { return _machine; }
            set { _machine = value; }
        }

        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        #endregion
    }

    [DataContract(Name = "ROStoreProfileOut", Namespace = "http://Logility.ROWeb/")]
    public class ROStoreProfileOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROStoreProfile _ROStoreProfile;

        public ROStoreProfileOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROStoreProfile ROStoreProfile) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROStoreProfile = ROStoreProfile;
        }

        public ROStoreProfile ROStoreProfile { get { return _ROStoreProfile; } }

    }
    [DataContract(Name = "ROAuditFilterOption", Namespace = "http://Logility.ROWeb/")]
    public class ROAuditFilterOption : ROOut
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _auditFilterOptions;
        public ROAuditFilterOption(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, KeyValuePair<int, string> auditFilterOptions) : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _auditFilterOptions = auditFilterOptions;
        }
    }

    [DataContract(Name = "ROAllStoresProfilesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAllStoresProfilesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private List<ROStoreProfile> _activeStores;

        [DataMember(IsRequired = true)]
        private List<ROStoreProfile> _inactiveStores;

        [DataMember(IsRequired = true)]
        private List<ROStoreProfile> _deletedStores;

        public ROAllStoresProfilesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _activeStores = new List<ROStoreProfile>();
            _inactiveStores = new List<ROStoreProfile>();
            _deletedStores = new List<ROStoreProfile>();
        }

        public List<ROStoreProfile> ActiveStores { get { return _activeStores; } }

        public List<ROStoreProfile> InactiveStores { get { return _inactiveStores; } }

        public List<ROStoreProfile> DeletedStores { get { return _deletedStores; } }




    }


    [DataContract(Name = "ROAuditResult", Namespace = "http://Logility.ROWeb/")]
    public class ROAuditResult : ROOut
    {
        [DataMember(IsRequired = true)]
        public List<AuditResult> _auditResults { get; set; }
        public ROAuditResult(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, List<AuditResult> AuditResults) : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _auditResults = AuditResults;
        }

    }

    public class AuditResult
    {
        public AuditResult()
        {
            AuditSummaryList = new List<AuditSummary>();
            AuditSummaryRowList = new List<AuditSummaryRow>();
            AuditDetailsRowsList = new List<AuditDetailsRow>();
            AuditDetailsList = new List<AuditDetails>();
        }
        public int ProcessRID { get; set; }
        public int ProcessID { get; set; }
        public string Process { get; set; }
        public string User { get; set; }
        public string ExecutionStatus { get; set; }
        public string CompletionStatus { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; } 
        public string Duration { get; set; }
        public string HigestMessageLevel { get; set; }
        public string Time { get; set; }
        public string Module { get; set; } 
        public string MessageLevel { get; set; } 
        public string Message { get; set; }
        public string MessageDetails { get; set; }
        public List<AuditSummary> AuditSummaryList { get; set; }
        public List<AuditSummaryRow> AuditSummaryRowList { get; set; }
        public List<AuditDetailsRow> AuditDetailsRowsList { get; set; }
        public List<AuditDetails> AuditDetailsList { get; set; }
    }
    public class AuditSummaryRow
    {
        public bool NeedsLoaded { get; set; }
        public int ProcessRID { get; set; }
        public int ProcessID { get; set; }
        public string Text { get; set; }
    }

    public class AuditSummary
    {
        public int ProcessRID { get; set; }
        public string  Item { get; set; }
        public string Value { get; set; }
    }
    public class AuditDetailsRow
    {
        public bool NeedsLoaded { get; set; }
        public int ProcessRID { get; set; }
        public int ProcessID { get; set; }
        public string Text { get; set; }
    }
    public class AuditDetails
    {
        public int ProcessRID { get; set; }
        public string Time { get; set; }
        public string Module { get; set; }
        public string MessageLevel { get; set; }
        public string MessageLevelText { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public string Message2 { get; set; }
    }

}
