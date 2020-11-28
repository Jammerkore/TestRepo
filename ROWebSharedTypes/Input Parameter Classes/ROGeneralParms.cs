using MIDRetail.DataCommon;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "RONoParms", Namespace = "http://Logility.ROWeb/")]
    public class RONoParms : ROParms
    {

        public RONoParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {

        }

    };

    [DataContract(Name = "ROIntParms", Namespace = "http://Logility.ROWeb/")]
    public class ROIntParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private int _nROInt;

        public ROIntParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, int nROInt) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _nROInt = nROInt;
        }

        public int ROInt { get { return _nROInt; } }

    };

    [DataContract(Name = "ROStringParms", Namespace = "http://Logility.ROWeb/")]
    public class ROStringParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private string _sROString;

        public ROStringParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, string sROString) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _sROString = sROString;
        }

        public string ROString { get { return _sROString; } }

    };


    [DataContract(Name = "ROKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class ROKeyParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private int _iKey;                    // the node id

        public ROKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, int iKey) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _iKey = iKey;
        }

        public int Key { get { return _iKey; } }

    };

    [DataContract(Name = "RODataTableParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataTableParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private DataTable _dtValue;                    // the node id

        public RODataTableParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, DataTable dtValue) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _dtValue = dtValue;
        }

        public DataTable dtValue { get { return _dtValue; } }

    };

    [DataContract(Name = "RODataSetParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataSetParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private DataSet _dsValue;                    // the node id

        public RODataSetParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, DataSet dsValue) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _dsValue = dsValue;
        }

        public DataSet dsValue { get { return _dsValue; } }

    };

    [DataContract(Name = "ROListParms", Namespace = "http://Logility.ROWeb/")]
    public class ROListParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ArrayList _ListValues;                    // the node id

        public ROListParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ArrayList ListValues) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ListValues = ListValues;
        }

        public ArrayList ListValues { get { return _ListValues; } }

    };

    [DataContract(Name = "ROPlanningVersionsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningVersionsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eROVersionsType _eROVersionsType;
        [DataMember(IsRequired = true)]
        private eSecuritySelectType _minimumSecurity ;
        

        public ROPlanningVersionsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, eROVersionsType eROVersionsTypeParam, eSecuritySelectType minimumSecurityParam= eSecuritySelectType.View) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _eROVersionsType = eROVersionsTypeParam;
            _minimumSecurity = minimumSecurityParam;
        }

        public eROVersionsType eROVersionsType { get { return _eROVersionsType; } }
        public eSecuritySelectType MinimumSecurity { get { return _minimumSecurity; } }

    };


    [DataContract(Name = "ROLowLevelModelParms", Namespace = "http://Logility.ROWeb/")]
    public class ROLowLevelModelParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private int _modelKey;

        [DataMember(IsRequired = true)]
        private int _customModelKey;
        public ROLowLevelModelParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, int ModelKey = Include.NoRID, int CustomModelKey = Include.NoRID) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _modelKey = ModelKey;
            _customModelKey = CustomModelKey;
        }

        public int ModelKey { get { return _modelKey; } }
        public int CustomModelKey { get { return _customModelKey; } }

    };

    [DataContract(Name = "ROProfileKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class ROProfileKeyParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private bool _readOnly;

        public ROProfileKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eProfileType profileType, int key = Include.Undefined, bool readOnly = false)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _profileType = profileType;
            _key = key;
            _readOnly = readOnly;
        }

        public eProfileType ProfileType { get { return _profileType; } }

        public int Key { get { return _key; } }

        public bool ReadOnly { get { return _readOnly; } }
    }
}
