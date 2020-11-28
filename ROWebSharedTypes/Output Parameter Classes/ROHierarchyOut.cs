using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROHierarchyPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROHierarchyPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROHierarchyPropertiesProfile _ROHierarchyPropertiesProfile;

        public ROHierarchyPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROHierarchyPropertiesProfile ROHierarchyPropertiesProfile) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROHierarchyPropertiesProfile = ROHierarchyPropertiesProfile;
        }

        public ROHierarchyPropertiesProfile ROHierarchyPropertiesProfile { get { return _ROHierarchyPropertiesProfile; } }

    }

    [DataContract(Name = "RONodePropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private RONodeProperties _RONodeProperties;

        public RONodePropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, RONodeProperties RONodeProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _RONodeProperties = RONodeProperties;
        }

        public RONodeProperties RONodeProperties { get { return _RONodeProperties; } }

    }

    /// <summary>
    /// Contains data specific to hierarchy tree node items
    /// </summary>
    [DataContract(Name = "ROTreeNodeDataHierarchy", Namespace = "http://Logility.ROWeb/")]
    public class ROTreeNodeDataHierarchy : ROTreeNodeData
    {
        [DataMember(IsRequired = true)]
        private int _hierarchyKey;
        [DataMember(IsRequired = true)]
        private int _homeHierarchyParentKey;
        [DataMember(IsRequired = true)]
        private int _homeHierarchyKey;
        [DataMember(IsRequired = true)]
        private int _homeHierarchyLevel;
        [DataMember(IsRequired = true)]
        private eHierarchyType _homeHierarchyType;
        [DataMember(IsRequired = true)]
        private int _homeHierarchyOwner;
        [DataMember(IsRequired = true)]
        private eHierarchyLevelType _nodeType;

        public ROTreeNodeDataHierarchy(
            int key, 
            eProfileType profileType,
            int hierarchyKey,
            int homeHierarchyParentKey,
            int homeHierarchyKey,
            int homeHierarchyLevel,
            eHierarchyType homeHierarchyType,
            int homeHierarchyOwner,
            eHierarchyLevelType nodeType
            ) :
            base(key, profileType)
        {
            _hierarchyKey = hierarchyKey;
            _homeHierarchyParentKey = homeHierarchyParentKey;
            _homeHierarchyKey = homeHierarchyKey;
            _homeHierarchyLevel = homeHierarchyLevel;
            _homeHierarchyType = homeHierarchyType;
            _homeHierarchyOwner = homeHierarchyOwner;
            _nodeType = nodeType;
        }

        /// <summary>
        /// The key of the current hierarchy
        /// </summary>
        public int HierarchyKey { get { return _hierarchyKey; } }

        /// <summary>
        /// The key of the parent in the node's home hierarchy
        /// </summary>
        public int HomeHierarchyParentKey { get { return _homeHierarchyParentKey; } }

        /// <summary>
        /// The key of the hierarchy in the node's home hierarchy
        /// </summary>
        public int HomeHierarchyKey { get { return _homeHierarchyKey; } }

        /// <summary>
        /// The level of the node in its home hierarchy
        /// </summary>
        public int HomeHierarchyLevel { get { return _homeHierarchyLevel; } }

        /// <summary>
        /// The type of hierarchy for the node's home
        /// </summary>
        public eHierarchyType HomeHierarchyType { get { return _homeHierarchyType; } }

        /// <summary>
        /// The key of the owner of the node's home hierarchy
        /// </summary>
        public int HomeHierarchyOwner { get { return _homeHierarchyOwner; } }

        /// <summary>
        /// Identifies if the node is of a specific type style, color, or size
        /// </summary>
        public eHierarchyLevelType NodeType { get { return _nodeType; } }

    }
}
