using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
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
    public class ROHierarchyPropertiesMaintenance
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private long _ROInstanceID;
        private HierarchyPropertiesBase _HierarchyPropertiesClass;
        private eProfileType _currentDataType = eProfileType.None;
        private object _currentData = null;

        //=============
        // CONSTRUCTORS
        //=============
        public ROHierarchyPropertiesMaintenance(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
        }

        public void CleanUp()
        {
            if (_currentData != null
                && _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus == eLockStatus.Locked)
            {
                _HierarchyPropertiesClass.UnlockHierarchy(key: _HierarchyPropertiesClass.HierarchyProfile.Key);
            }

            if (_HierarchyPropertiesClass != null)
            {
                _HierarchyPropertiesClass.OnClosing();
            }
        }

        /// <summary>
        /// Get node optionally for update
        /// </summary>
        /// <param name="parms">The ROHierarchyPropertiesParms containing the node type and key</param>
        /// <returns>A ROHierarchyPropertiesOut instance with the properties of the node</returns>
        public ROOut HierarchyPropertiesGet(ROHierarchyPropertyKeyParms parms)
        {
            string message;

            ROHierarchyPropertiesOut ROHierarchyPropertiesOut = null;

            if (HierarchyPropertiesInitializeData(parms: parms, message: out message))
            {
                if (_HierarchyPropertiesClass.AllowView)
                {
                    ROHierarchyPropertiesProfile HierarchyProperties = _HierarchyPropertiesClass.HierarchyPropertiesGetData(parms: parms, HierarchyPropertiesData: _currentData, message: ref message);
                    if (_HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus == eLockStatus.Locked)
                    {
                        HierarchyProperties.CanBeDeleted = _HierarchyPropertiesClass.AllowDelete;
                        HierarchyProperties.IsReadOnly = _HierarchyPropertiesClass.IsReadOnly;
                    }
                    ROHierarchyPropertiesOut = new ROHierarchyPropertiesOut(eROReturnCode.Successful, null, _ROInstanceID, HierarchyProperties);
                    _currentDataType = parms.ProfileType;
                }
                else
                {
                    ROHierarchyPropertiesOut = new ROHierarchyPropertiesOut(eROReturnCode.Failure, MIDText.GetText(eMIDTextCode.msg_NotAuthorized), _ROInstanceID, null);
                }
            }
            else
            {
                ROHierarchyPropertiesOut = new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            return ROHierarchyPropertiesOut;
        }

        private bool HierarchyPropertiesInitializeData(ROHierarchyPropertyKeyParms parms, out string message)
        {
            message = null;

            _HierarchyPropertiesClass = GetHierarchyPropertiesClass(profileType: parms.ProfileType);

            if (_HierarchyPropertiesClass == null)
            {
                message = "Invalid profile type";
                return false;
            }

            int securityKey = parms.Key;

            // Unlock previously locked hierarchy
            if (_HierarchyPropertiesClass.HierarchyProfile != null
                && _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus == eLockStatus.Locked
                && (_currentDataType != parms.ProfileType
                    || _HierarchyPropertiesClass.HierarchyProfile.Key != parms.Key))
            {
                _SAB.HierarchyServerSession.DequeueHierarchy(hierarchyRID: _HierarchyPropertiesClass.HierarchyProfile.Key);
                _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus = eLockStatus.Undefined;
            }

            _HierarchyPropertiesClass.SetSecurity(
                key: parms.Key, 
                securityKey: securityKey, 
                setReadOnly: parms.ReadOnly, 
                hierarchyType: parms.HierarchyType, 
                ownerKey: parms.OwnerKey
                );

            if (_currentData == null
                || (_HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus != eLockStatus.Locked && !parms.ReadOnly)
                || (_currentDataType != parms.ProfileType || _HierarchyPropertiesClass.HierarchyProfile.Key != parms.Key)
                )
            {
                _currentData = _HierarchyPropertiesClass.HierarchyPropertiesGetValues(parms: parms);
                _currentDataType = parms.ProfileType;
            }

            if (parms.Key != Include.NoRID
                && _HierarchyPropertiesClass.HierarchyProfile.Key == Include.NoRID)
            {
                _currentData = null;
                _currentDataType = eProfileType.None;
                message = _SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Model) }
                    );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies Node Properties to memory only
        /// </summary>
        /// <returns>The key of the node if the save was successful</returns>
        public ROOut HierarchyPropertiesApply(ROHierarchyPropertiesParms parms)
        {
            string message = null;
            bool successful;
            ROHierarchyPropertyKeyParms HierarchyPropertiesGetParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _HierarchyPropertiesClass = GetHierarchyPropertiesClass(profileType: parms.ROHierarchyProperties.ProfileType);

            int securityKey = parms.ROHierarchyProperties.Hierarchy.Key;

            if (_currentData == null
                || _currentDataType != parms.ROHierarchyProperties.ProfileType
                || _HierarchyPropertiesClass.HierarchyProfile.Key != parms.ROHierarchyProperties.Hierarchy.Key)
            {
                HierarchyPropertiesGetParms = _HierarchyPropertiesClass.HierarchyPropertiesGetParms(parms: parms, profileType: parms.ROHierarchyProperties.ProfileType, key: parms.ROHierarchyProperties.Hierarchy.Key);
                if (!HierarchyPropertiesInitializeData(parms: HierarchyPropertiesGetParms, message: out message))
                {
                    return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _HierarchyPropertiesClass.SetSecurity(
                key: parms.ROHierarchyProperties.Hierarchy.Key, 
                securityKey: securityKey, 
                setReadOnly: parms.ReadOnly, 
                hierarchyType: parms.ROHierarchyProperties.HierarchyType, 
                ownerKey: parms.ROHierarchyProperties.OwnerKey
                );

            if (!_HierarchyPropertiesClass.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.ROHierarchyProperties.Hierarchy.Key != Include.NoRID
                && _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            object data = _HierarchyPropertiesClass.HierarchyPropertiesUpdateData(HierarchyProperties: parms.ROHierarchyProperties, cloneDates: false, message: ref message, successful: out successful, applyOnly: true);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                //MIDEnvironment.Message = message;
            }

            HierarchyPropertiesGetParms = _HierarchyPropertiesClass.HierarchyPropertiesGetParms(parms: parms, profileType: _currentDataType, key: parms.ROHierarchyProperties.Hierarchy.Key);

            if (parms.ROHierarchyProperties.Hierarchy.Key == Include.NoRID)
            {
                // get node properties and lock
                return HierarchyPropertiesGet(parms: HierarchyPropertiesGetParms);
            }
            else
            {
                // replace with update data and get node Properties 
                _currentData = data;
                ROHierarchyPropertiesProfile HierarchyProperties = _HierarchyPropertiesClass.HierarchyPropertiesGetData(parms: HierarchyPropertiesGetParms, HierarchyPropertiesData: _currentData, message: ref message, applyOnly: true);
                if (_HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus == eLockStatus.Locked)
                {
                    HierarchyProperties.CanBeDeleted = _HierarchyPropertiesClass.AllowDelete;
                    HierarchyProperties.IsReadOnly = _HierarchyPropertiesClass.IsReadOnly;
                }
                return new ROHierarchyPropertiesOut(returnCode, message, _ROInstanceID, HierarchyProperties);
            }
        }

        /// <summary>
        /// Save Node Properties to database
        /// </summary>
        /// <returns>The key of the node properties if the save was successful</returns>
        public ROOut HierarchyPropertiesSave(ROHierarchyPropertiesParms parms)
        {
            string message = null;
            bool successful;
            ROHierarchyPropertyKeyParms HierarchyPropertiesGetParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _HierarchyPropertiesClass = GetHierarchyPropertiesClass(profileType: parms.ROHierarchyProperties.ProfileType);

            int securityKey = parms.ROHierarchyProperties.Hierarchy.Key;

            if (_currentData == null
                || _currentDataType != parms.ROHierarchyProperties.ProfileType
                || _HierarchyPropertiesClass.HierarchyProfile.Key != parms.ROHierarchyProperties.Hierarchy.Key)
            {
                HierarchyPropertiesGetParms = _HierarchyPropertiesClass.HierarchyPropertiesGetParms(parms: parms, profileType: parms.ROHierarchyProperties.ProfileType, key: parms.ROHierarchyProperties.Hierarchy.Key);
                if (!HierarchyPropertiesInitializeData(parms: HierarchyPropertiesGetParms, message: out message))
                {
                    return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _HierarchyPropertiesClass.SetSecurity(
                key: parms.ROHierarchyProperties.Hierarchy.Key, 
                securityKey: securityKey, 
                setReadOnly: parms.ReadOnly, 
                hierarchyType: parms.ROHierarchyProperties.HierarchyType, 
                ownerKey: parms.ROHierarchyProperties.OwnerKey
                );

            if (!_HierarchyPropertiesClass.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.ROHierarchyProperties.Hierarchy.Key != Include.NoRID
                && _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            object data = _HierarchyPropertiesClass.HierarchyPropertiesUpdateData(HierarchyProperties: parms.ROHierarchyProperties, cloneDates: false, message: ref message, successful: out successful);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                //MIDEnvironment.Message = message;
            }

            HierarchyPropertiesGetParms = _HierarchyPropertiesClass.HierarchyPropertiesGetParms(parms: parms, profileType: _currentDataType, key: _HierarchyPropertiesClass.HierarchyProfile.Key);


            if (parms.ROHierarchyProperties.Hierarchy.Key == Include.NoRID
                && successful)
            {
                // get node properties and lock
                return HierarchyPropertiesGet(parms: HierarchyPropertiesGetParms);
            }
            else
            {
                // replace with update data and get node Properties 
                _currentData = data;
                ROHierarchyPropertiesProfile HierarchyProperties = _HierarchyPropertiesClass.HierarchyPropertiesGetData(parms: HierarchyPropertiesGetParms, HierarchyPropertiesData: _currentData, message: ref message);
                if (_HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus == eLockStatus.Locked)
                {
                    HierarchyProperties.CanBeDeleted = _HierarchyPropertiesClass.AllowDelete;
                    HierarchyProperties.IsReadOnly = _HierarchyPropertiesClass.IsReadOnly;
                }
                return new ROHierarchyPropertiesOut(returnCode, message, _ROInstanceID, HierarchyProperties);
                //return new ROHierarchyPropertiesOut(eROReturnCode.Successful, null, _ROInstanceID, _HierarchyPropertiesClass.HierarchyPropertiesGetData(parms: HierarchyPropertiesGetParms, HierarchyPropertiesData: _currentData, message: ref message));
            }
        }



        /// <summary>
        /// Delete node properties fora node
        /// </summary>
        /// <param name="parms">A ROHierarchyPropertyKeyParms instance containing the type and key of the node properties to be deleted</param>
        /// <returns>A flag identifying if the delete was successful</returns>
        public ROOut HierarchyPropertiesDelete(ROHierarchyPropertyKeyParms parms)
        {
            string message = null;

            _HierarchyPropertiesClass = GetHierarchyPropertiesClass(profileType: parms.ProfileType);

            int securityKey = parms.Key;

            if (_currentData == null
                || _currentDataType != parms.ProfileType
                || (_HierarchyPropertiesClass.HierarchyProfile != null && _HierarchyPropertiesClass.HierarchyProfile.Key != parms.Key)
                )
            {
                ROHierarchyPropertyKeyParms HierarchyPropertiesGetParms = _HierarchyPropertiesClass.HierarchyPropertiesGetParms(parms: parms, profileType: parms.ProfileType, key: parms.Key);
                if (!HierarchyPropertiesInitializeData(parms: HierarchyPropertiesGetParms, message: out message))
                {
                    return new ROHierarchyPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _HierarchyPropertiesClass.SetSecurity(
                key: parms.Key, 
                securityKey: securityKey, 
                setReadOnly: parms.ReadOnly, 
                hierarchyType: parms.HierarchyType, 
                ownerKey: parms.OwnerKey
                );

            if (!_HierarchyPropertiesClass.AllowDelete
                || _HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus != eLockStatus.Locked
                )
            {
                if (!_HierarchyPropertiesClass.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_HierarchyPropertiesClass.HierarchyProfile.HierarchyLockStatus != eLockStatus.Locked)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                return new ROBoolOut(eROReturnCode.Failure, message, parms.ROInstanceID, false);
            }

            bool success = _HierarchyPropertiesClass.HierarchyPropertiesDelete(key: parms.Key, message: ref message);
            if (success)
            {
                _currentData = _HierarchyPropertiesClass.HierarchyPropertiesGetValues(parms: parms);
            }
            else
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = message;
            }

            return HierarchyPropertiesGet(parms: parms);
        }

        


        private HierarchyPropertiesBase GetHierarchyPropertiesClass(eProfileType profileType)
        {
            if (_HierarchyPropertiesClass != null
                && _HierarchyPropertiesClass.ProfileType == profileType)
            {
                return _HierarchyPropertiesClass;
            }

            return new HierarchyPropertiesProfile(SAB: _SAB, ROWebTools: _ROWebTools);

        }

    }
}
