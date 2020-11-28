using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    abstract public class HierarchyPropertiesBase : HierarchyBase
    {
        //=======
        // FIELDS
        //=======
        protected HierarchyProfile _hierarchyProfile = null;

        //=============
        // CONSTRUCTORS
        //=============
        public HierarchyPropertiesBase(SessionAddressBlock SAB, ROWebTools ROWebTools, eProfileType profileType) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: profileType)
        {

        }

        //===========
        // PROPERTIES
        //===========

        public HierarchyProfile HierarchyProfile
        {
            get
            {
                return _hierarchyProfile;
            }
        }

        public bool AllowView
        {
            get
            {
                return _functionSecurity.AllowView;
            }
        }

        public bool AllowUpdate
        {
            get
            {
                return _functionSecurity.AllowUpdate;
            }
        }

        public bool AllowDelete
        {
            get
            {
                return _functionSecurity.AllowDelete;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _functionSecurity.IsReadOnly;
            }
        }

        //========
        // METHODS
        //========


        abstract public ROHierarchyPropertiesProfile HierarchyPropertiesGetData(ROHierarchyPropertyKeyParms parms, object HierarchyPropertiesData, ref string message, bool applyOnly = false);

        abstract public object HierarchyPropertiesUpdateData(ROHierarchyPropertiesProfile HierarchyProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false);

        abstract public bool HierarchyPropertiesDelete(int key, ref string message);

        abstract public object HierarchyPropertiesGetValues(ROHierarchyPropertyKeyParms parms);

        abstract protected void GetFunctionSecurity(int key);

        abstract public bool OnClosing();

        virtual public ROHierarchyPropertyKeyParms HierarchyPropertiesGetParms(ROHierarchyPropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {

            ROHierarchyPropertyKeyParms profileKeyParms = new ROHierarchyPropertyKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.HierarchyPropertiesGet,
                ROInstanceID: parms.ROInstanceID,
                key: key,
                readOnly: readOnly,
                hierarchyType: parms.ROHierarchyProperties.HierarchyType,
                ownerKey: parms.ROHierarchyProperties.OwnerKey
                );

            return profileKeyParms;
        }

        virtual public ROHierarchyPropertyKeyParms HierarchyPropertiesGetParms(ROHierarchyPropertyKeyParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            ROHierarchyPropertyKeyParms profileKeyParms = new ROHierarchyPropertyKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.HierarchyPropertiesGet,
                ROInstanceID: parms.ROInstanceID,
                key: key,
                readOnly: readOnly,
                hierarchyType: parms.HierarchyType,
                ownerKey: parms.OwnerKey
                );

            return profileKeyParms;
        }

        public void UnlockHierarchy(int key)
        {
            SAB.HierarchyServerSession.DequeueHierarchy(hierarchyRID: key);
        }

        public eLockStatus LockNode(eModelType modelType, int key, string name, bool allowReadOnly, out string message)
        {
            message = null;
            eLockStatus lockStatus = eLockStatus.Undefined;

           

            return lockStatus;
        }

        public void SetSecurity(int key, int securityKey, bool setReadOnly, eHierarchyType hierarchyType, int ownerKey)
        {
            if (_functionSecurity == null
                    || (_hierarchyProfile != null
                        && _hierarchyProfile.Key != securityKey)
                    )
            {
                if (key == Include.NoRID)
                {
                    _hierarchyProfile = new HierarchyProfile(Include.NoRID);
                    _hierarchyProfile.HierarchyLockStatus = eLockStatus.Locked;
                    _hierarchyProfile.HierarchyType = hierarchyType;
                    _hierarchyProfile.Owner = ownerKey;
                }
                else if (setReadOnly)
                {
                    _hierarchyProfile = SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID: key);
                }
                else
                {
                    _hierarchyProfile = SAB.HierarchyServerSession.GetHierarchyDataForUpdate(aHierarchyRID: key, aAllowReadOnly: true);
                }

                GetFunctionSecurity(key: _hierarchyProfile.HierarchyRootNodeRID);
            }
        }

    }
}
