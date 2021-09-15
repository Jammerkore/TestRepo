using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{
    

    [DataContract(Name = "ROCharacteristicsPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristicsPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROCharacteristicsProperties _ROCharacteristicsProperties;

        public ROCharacteristicsPropertiesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROCharacteristicsProperties ROCharacteristicsProperties) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ROCharacteristicsProperties = ROCharacteristicsProperties;
        }

        public ROCharacteristicsProperties ROCharacteristicsProperties { get { return _ROCharacteristicsProperties; } }

    }

    [DataContract(Name = "ROModelParms", Namespace = "http://Logility.ROWeb/")]
    public class ROModelParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eModelType _modelType;
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private bool _readOnly;

        public ROModelParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eModelType modelType, int key = Include.Undefined, bool readOnly = false)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _modelType = modelType;
            _key = key;
            _readOnly = readOnly;
        }

        public eModelType ModelType { get { return _modelType; } }

        public int Key { get { return _key; } }

        public bool ReadOnly { get { return _readOnly; } }
    }

    [DataContract(Name = "ROSizeCurveModelParms", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveModelParms : ROModelParms
    {
        [DataMember(IsRequired = true)]
        private int _attributeKey;
        [DataMember(IsRequired = true)]
        private int _sizeGroupKey;

        public ROSizeCurveModelParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eModelType modelType, int key = Include.Undefined, bool readOnly = false,
                                        int attributeKey = Include.NoRID, int sizeGroupKey = Include.NoRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, modelType, key, readOnly)
        {
            _attributeKey = attributeKey;
            _sizeGroupKey = sizeGroupKey;
        }

        public int AttributeKey { get { return _attributeKey; } }

        public int SizeGroupKey { get { return _sizeGroupKey; } }
    }

    [DataContract(Name = "ROSizeConstraintModelParms", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintModelParms : ROModelParms
    {
        [DataMember(IsRequired = true)]
        private int _attributeKey;

        [DataMember(IsRequired = true)]
        private int _sizeGroupKey;

        [DataMember(IsRequired = true)]
        private int _sizeCurveGroupKey;

        public ROSizeConstraintModelParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eModelType modelType, int key = Include.Undefined, bool readOnly = false,
                                        int attributeKey = Include.NoRID, int sizeGroupKey = Include.NoRID, int sizeCurveGroupKey = Include.NoRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, modelType, key, readOnly)
        {
            _attributeKey = attributeKey;
            _sizeGroupKey = sizeGroupKey;
            _sizeCurveGroupKey = sizeCurveGroupKey;
        }

        public int AttributeKey { get { return _attributeKey; } }

        public int SizeGroupKey { get { return _sizeGroupKey; } }

        public int SizeCurveGroupKey { get { return _sizeCurveGroupKey; } }
    }

    [DataContract(Name = "ROModelPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROModelPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROModelProperties _ROModelProperties;

        public ROModelPropertiesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROModelProperties ROModelProperties) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ROModelProperties = ROModelProperties;
        }

        public ROModelProperties ROModelProperties { get { return _ROModelProperties; } }

    }

    [DataContract(Name = "RUserInformationParms", Namespace = "http://Logility.ROWeb/")]
    public class RUserInformationParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROUserInformation _ROUserInformation;

        public RUserInformationParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROUserInformation ROUserInformation) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ROUserInformation = ROUserInformation;
        }

        public ROUserInformation ROUserInformation { get { return _ROUserInformation; } }

    }

    [DataContract(Name = "ROGlobalOptionsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROGlobalOptionsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROGlobalOptions _ROGlobalOptions;

        public ROGlobalOptionsParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, ROGlobalOptions ROGlobalOptions) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ROGlobalOptions = ROGlobalOptions;
        }

        public ROGlobalOptions ROGlobalOptions { get { return _ROGlobalOptions; } }

    }

}
