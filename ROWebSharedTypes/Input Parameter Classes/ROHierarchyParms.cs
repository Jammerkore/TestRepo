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

    //[DataContract(Name = "ROGetNodeDataParms", Namespace = "http://Logility.ROWeb/")]
    //public class ROGetNodeDataParms : ROParms
    //{
    //    [DataMember(IsRequired = true)]
    //    private string _sNodeID;                    // the node id

    //    public ROGetNodeDataParms(string sUserID, string sSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, string sNodeID) :
    //        base (sUserID, sSessionID, ROClass, RORequest, ROInstanceID)
    //    {
    //        _sNodeID = sNodeID;
    //    }

    //    public string NodeID { get { return _sNodeID; } }

    //};

    [DataContract(Name = "ROPagingParms", Namespace = "http://Logility.ROWeb/")]
    public class ROPagingParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private string _sRONodeAddress;
        [DataMember(IsRequired = true)]
        private int _iROStartRecord;
        [DataMember(IsRequired = true)]
        private int _iROMaxRecords;

        public ROPagingParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, string sRONodeAddress, int iROStartRecord, int iROMaxRecords) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _sRONodeAddress = sRONodeAddress;
            _iROStartRecord = iROStartRecord;
            _iROMaxRecords = iROMaxRecords;
        }

        public string RONodeAddress { get { return _sRONodeAddress; } }
        public int ROStartRecord { get { return _iROStartRecord; } }
        public int ROMaxRecords { get { return _iROMaxRecords; } }

    };

    [DataContract(Name = "ROHierarchyPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROHierarchyPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROHierarchyPropertiesProfile _ROHierarchyProperties;
        [DataMember(IsRequired = true)]
        private bool _readOnly;

        public ROHierarchyPropertiesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        ROHierarchyPropertiesProfile ROHierarchyProperties, bool readOnly = false)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _ROHierarchyProperties = ROHierarchyProperties;
            _readOnly = readOnly;
        }

        public ROHierarchyPropertiesProfile ROHierarchyProperties { get { return _ROHierarchyProperties; } }

        public bool ReadOnly { get { return _readOnly; } }
    }

    [DataContract(Name = "RONodePropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private RONodeProperties _RONodeProperties;
        [DataMember(IsRequired = true)]
        private bool _readOnly;

        public RONodePropertiesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        RONodeProperties RONodeProperties, bool readOnly = false)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _RONodeProperties = RONodeProperties;
            _readOnly = readOnly;
        }

        public RONodeProperties RONodeProperties { get { return _RONodeProperties; } }

        public bool ReadOnly { get { return _readOnly; } }
    }

    [DataContract(Name = "ROHierarchyPropertyKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class ROHierarchyPropertyKeyParms : ROProfileKeyParms
    {
        [DataMember(IsRequired = true)]
        private eHierarchyType _hierarchyType;

        [DataMember(IsRequired = true)]
        private int _ownerKey;


        public ROHierarchyPropertyKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        int key = Include.Undefined, bool readOnly = false,
                                        eHierarchyType hierarchyType = eHierarchyType.None, int ownerKey = Include.NoRID )
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, eProfileType.Hierarchy, key, readOnly)
        {
            _hierarchyType = hierarchyType;
            _ownerKey = ownerKey;
        }

        public eHierarchyType HierarchyType { get { return _hierarchyType; } }

        public int OwnerKey { get { return _ownerKey; } }

    }

    [DataContract(Name = "RONodePropertyKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertyKeyParms : ROProfileKeyParms
    {
        [DataMember(IsRequired = true)]
        private int _parentKey;


        public RONodePropertyKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eProfileType profileType, int key = Include.Undefined, bool readOnly = false,
                                        int parentKey = Include.NoRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, profileType, key, readOnly)
        {
            _parentKey = parentKey;
        }

        public int ParentKey { get { return _parentKey; } }

    }

    [DataContract(Name = "RONodePropertyAttributeKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertyAttributeKeyParms : RONodePropertyKeyParms
    {
        [DataMember(IsRequired = true)]
        private int _attributeKey;

        [DataMember(IsRequired = true)]
        private int _attributeSetKey;

        public RONodePropertyAttributeKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eProfileType profileType, int key = Include.Undefined, bool readOnly = false,
                                        int parentNodeKey = Include.NoRID, int attributeKey = Include.NoRID, int attributeSetKey = Include.NoRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, profileType, key, readOnly, parentNodeKey)
        {
            _attributeKey = attributeKey;
            _attributeSetKey = attributeSetKey;
        }

        public int AttributeKey { get { return _attributeKey; } }
        public int AttributeSetKey { get { return _attributeSetKey; } }

    }

    [DataContract(Name = "RONodePropertyAttributeDateKeyParms", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertyAttributeDateKeyParms : RONodePropertyAttributeKeyParms
    {
        [DataMember(IsRequired = true)]
        private int _dateKey;


        public RONodePropertyAttributeDateKeyParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eProfileType profileType, int key = Include.Undefined, bool readOnly = false,
                                        int parentNodeKey = Include.NoRID, int attributeKey = Include.NoRID, int dateKey = Include.NoRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, profileType, key, readOnly, parentNodeKey, attributeKey)
        {
            _dateKey = dateKey;
        }

        public int DateKey { get { return _dateKey; } }

    }

}