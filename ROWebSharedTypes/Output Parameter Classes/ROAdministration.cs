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
}
