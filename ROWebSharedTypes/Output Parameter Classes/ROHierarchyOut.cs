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
}
