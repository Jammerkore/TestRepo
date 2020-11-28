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
    public class ROCharacteristicMaintenance
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private long _ROInstanceID;
        private CharacteristicBase _characteristicsClass;
        

        //=============
        // CONSTRUCTORS
        //=============
        public ROCharacteristicMaintenance(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
        }

        //===========
        // PROPERTIES
        //===========

        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        protected SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        /// <summary>
        /// Gets the unique function ID
        /// </summary>
        protected long ROInstanceID
        {
            get { return _ROInstanceID; }
        }

        //===========
        // METHODS
        //===========

        public void CleanUp()
        {
            if (_characteristicsClass != null
                && _characteristicsClass.LockStatus == eLockStatus.Locked)
            {
                _characteristicsClass.Unlock();
            }

        }

        /// <summary>
        /// Get Characteristics
        /// </summary>
        /// <returns>Class containing characteristics</returns>
        public ROOut GetCharacteristics(ROProfileKeyParms parms)
        {
            string message = null;

            _characteristicsClass = GetCharacteristicClass(profileType: parms.ProfileType, key: Include.NoRID);

            if (_characteristicsClass == null)
            {
                return new ROCharacteristicsOut(eROReturnCode.Failure, "Invalid characteristic type", parms.ROInstanceID, null);
            }


            if (!_characteristicsClass.FunctionSecurity.AllowView)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROCharacteristicsOut(eROReturnCode.Failure, message, parms.ROInstanceID, null);
            }

            ROCharacteristicsOut ROCharacteristicsOut = new ROCharacteristicsOut(eROReturnCode.Successful, null, _ROInstanceID, _characteristicsClass.CharacteristicsGetData());

            return ROCharacteristicsOut;
        }

        /// <summary>
        /// Get single characteristic optionally for update
        /// </summary>
        /// <returns>Class containing characteristics</returns>
        public ROOut GetCharacteristic(ROProfileKeyParms parms)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            // Unlock previous characteristic if different
            if (_characteristicsClass != null
                && _characteristicsClass.LockStatus == eLockStatus.Locked
                && (_characteristicsClass.ProfileType != parms.ProfileType
                    || _characteristicsClass.Key != parms.Key)
                )
            {
                _characteristicsClass.Unlock();
            }

            _characteristicsClass = GetCharacteristicClass(profileType: parms.ProfileType, key: parms.Key);

            if (_characteristicsClass == null)
            {
                return new ROCharacteristicsOut(eROReturnCode.Failure, "Invalid characteristic type", parms.ROInstanceID, null);
            }

            ROCharacteristicsProperties characteristicsProperties = _characteristicsClass.CharacteristicGetData(parms: parms);

            if (_characteristicsClass.Key == Include.NoRID)
            {
                returnCode = eROReturnCode.Failure;
                message = _SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { _characteristicsClass.GetCharacteristicTypeName(parms.ProfileType) }
                            );
            }
            else if (parms.ReadOnly
                || !_characteristicsClass.FunctionSecurity.AllowUpdate)
            {
                if (!_characteristicsClass.FunctionSecurity.AllowUpdate)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                    MIDEnvironment.isChangedToReadOnly = true;
                    returnCode = eROReturnCode.ChangedToReadOnly;
                }
            }
            else
            {
                if (!_characteristicsClass.CharacteristicLock(profileType: parms.ProfileType, key: parms.Key, name: _characteristicsClass.CharacteristicGroupName))
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                }
            }

            ROCharacteristicsOut ROCharacteristicsOut = new ROCharacteristicsOut(returnCode, message, _ROInstanceID, characteristicsProperties);

            return ROCharacteristicsOut;
        }

        /// <summary>
        /// Save Characteristics
        /// </summary>
        /// <returns>Class containing characteristics</returns>
        public ROOut SaveCharacteristics(ROCharacteristicsPropertiesParms parms)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            if (parms.ROCharacteristicsProperties.Characteristics.Count == 0)
            {
                return new ROCharacteristicsOut(eROReturnCode.Failure, "No data to save", parms.ROInstanceID, null);
            }

            // unlock any previously locked characteristic if not first in list
            if (_characteristicsClass != null
                && _characteristicsClass.LockStatus == eLockStatus.Locked
                && (_characteristicsClass.ProfileType != parms.ROCharacteristicsProperties.ProfileType
                    || _characteristicsClass.Key != parms.ROCharacteristicsProperties.Characteristics[0].Characteristic.Key)
                )
            {
                _characteristicsClass.Unlock();
            }

            _characteristicsClass = GetCharacteristicClass(profileType: parms.ROCharacteristicsProperties.ProfileType, key: parms.ROCharacteristicsProperties.Characteristics[0].Characteristic.Key);

            if (_characteristicsClass == null)
            {
                return new ROCharacteristicsOut(eROReturnCode.Failure, "Invalid characteristic type", parms.ROInstanceID, null);
            }

            if (!_characteristicsClass.FunctionSecurity.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROBoolOut(eROReturnCode.Failure, message, parms.ROInstanceID, false);
            }

            bool success = true;
            bool lockedDuringSave = false;

            foreach (ROCharacteristicDefinition characteristicDefinition in parms.ROCharacteristicsProperties.Characteristics)
            {
                try
                {
                    // unlock any previously locked characteristic
                    if (_characteristicsClass.LockStatus == eLockStatus.Locked
                        && (_characteristicsClass.ProfileType != parms.ROCharacteristicsProperties.ProfileType
                            || _characteristicsClass.Key != characteristicDefinition.Characteristic.Key)
                        )
                    {
                        _characteristicsClass.Unlock();
                    }

                    if (_characteristicsClass.LockStatus != eLockStatus.Locked
                        && characteristicDefinition.Characteristic.Key != Include.NoRID)
                    {
                        if (!_characteristicsClass.CharacteristicLock(profileType: parms.ROCharacteristicsProperties.ProfileType, key: characteristicDefinition.Characteristic.Key, name: characteristicDefinition.Characteristic.Value))
                        {
                            success = false;
                            continue;
                        }
                        else
                        {
                            lockedDuringSave = true;
                        }
                    }

                    if (!_characteristicsClass.CharacteristicSave(characteristicDefinition: characteristicDefinition, message: out message))
                    {
                        success = false;
                    }
                }
                finally
                {
                    if (_characteristicsClass.LockStatus == eLockStatus.Locked
                        && lockedDuringSave)
                    {
                        _characteristicsClass.Unlock();
                    }
                }
            }

            // Refresh cache
            _characteristicsClass.CharacteristicsGetData();

            if (!success
                && returnCode == eROReturnCode.Successful)
            {
                returnCode = eROReturnCode.Failure;
            }

            return new ROBoolOut(returnCode, message, _ROInstanceID, success);
        }

        /// <summary>
        /// Delete characteristic
        /// </summary>
        /// <returns>A flag indicating the success of the delete</returns>
        public ROOut DeleteCharacteristics(ROParms parms)
        {
            if (parms is ROProfileKeyParms)
            {
                return DeleteCharacteristic((ROProfileKeyParms)parms);
            }
            else if (parms is ROListParms)
            {
                return DeleteCharacteristicList((ROListParms)parms);
            }
            else
            {
                return new RONoDataOut(eROReturnCode.Failure, "Invalid input type", ROInstanceID);
            }
        }

        private ROOut DeleteCharacteristic(ROProfileKeyParms parms)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Failure;
            bool success = false;

            try
            {
                _characteristicsClass = GetCharacteristicClass(profileType: parms.ProfileType, key: parms.Key);

                if (_characteristicsClass == null)
                {
                    return new ROBoolOut(eROReturnCode.Failure, "Invalid characteristic type", parms.ROInstanceID, false);
                }

                if (_characteristicsClass.FunctionSecurity.AllowDelete
                    && _characteristicsClass.LockStatus != eLockStatus.Locked)
                {
                    if (!_characteristicsClass.CharacteristicLock(profileType: parms.ProfileType, key: parms.Key, name: null))
                    {
                        MIDEnvironment.isChangedToReadOnly = true;
                    }
                }

                if (!_characteristicsClass.FunctionSecurity.AllowDelete
                    || _characteristicsClass.LockStatus == eLockStatus.ReadOnly
                    || !ApplicationUtilities.AllowDeleteFromInUse(key: parms.Key, profileType: parms.ProfileType, SAB: SAB))
                {
                    message = null;
                    if (!_characteristicsClass.FunctionSecurity.AllowDelete)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                        ROWebTools.LogMessage(eROMessageLevel.Information, message);
                        MIDEnvironment.Message = message;
                    }
                    else if (_characteristicsClass.LockStatus == eLockStatus.ReadOnly)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                        ROWebTools.LogMessage(eROMessageLevel.Information, message);
                        MIDEnvironment.Message = message;
                    }
                }
                else if (_characteristicsClass.LockStatus == eLockStatus.Locked)
                {
                    if (_characteristicsClass.CharacteristicDelete(parms: parms, message: out message))
                    {
                        success = true;
                        returnCode = eROReturnCode.Successful;
                    }
                }

                return new ROBoolOut(returnCode, message, _ROInstanceID, success);
            }
            finally
            {
                if (_characteristicsClass != null)
                {
                    // Refresh cache
                    if (success)
                    {
                        _characteristicsClass.CharacteristicsGetData();
                    }
                    _characteristicsClass.Unlock();
                }
            }
        }

        private ROOut DeleteCharacteristicList(ROListParms parms)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Failure;
            bool success = false;

            foreach (ROProfileKey profileKey in parms.ListValues)
            {
                try
                {
                    _characteristicsClass = GetCharacteristicClass(profileType: profileKey.ProfileType, key: profileKey.Key);

                    if (_characteristicsClass == null)
                    {
                        return new ROBoolOut(eROReturnCode.Failure, "Invalid characteristic type", parms.ROInstanceID, false);
                    }

                    // Unlock previous characteristic if different
                    if (_characteristicsClass != null
                        && _characteristicsClass.LockStatus == eLockStatus.Locked
                        && (_characteristicsClass.ProfileType != profileKey.ProfileType
                            || _characteristicsClass.Key != profileKey.Key)
                        )
                    {
                        _characteristicsClass.Unlock();
                    }

                    if (_characteristicsClass.FunctionSecurity.AllowDelete
                        && _characteristicsClass.LockStatus != eLockStatus.Locked)
                    {
                        if (!_characteristicsClass.CharacteristicLock(profileType: profileKey.ProfileType, key: profileKey.Key, name: null))
                        {
                            MIDEnvironment.isChangedToReadOnly = true;
                        }
                    }

                    if (!_characteristicsClass.FunctionSecurity.AllowDelete
                        || _characteristicsClass.LockStatus == eLockStatus.ReadOnly
                        || !ApplicationUtilities.AllowDeleteFromInUse(key: profileKey.Key, profileType: profileKey.ProfileType, SAB: SAB))
                    {
                        message = null;
                        if (!_characteristicsClass.FunctionSecurity.AllowDelete)
                        {
                            message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                            ROWebTools.LogMessage(eROMessageLevel.Information, message);
                            MIDEnvironment.Message = message;
                        }
                        else if (_characteristicsClass.LockStatus == eLockStatus.ReadOnly)
                        {
                            message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                            ROWebTools.LogMessage(eROMessageLevel.Information, message);
                            MIDEnvironment.Message = message;
                        }
                    }
                    else if (_characteristicsClass.LockStatus == eLockStatus.Locked)
                    {
                        if (_characteristicsClass.CharacteristicDelete(parms: new ROProfileKeyParms(sROUserID: parms.ROUserID,
                                                                                                    sROSessionID: parms.ROSessionID,
                                                                                                    ROClass: parms.ROClass,
                                                                                                    RORequest: parms.RORequest,
                                                                                                    ROInstanceID: parms.ROInstanceID,
                                                                                                    profileType: profileKey.ProfileType,
                                                                                                    key: profileKey.Key),
                                                                       message: out message
                                                                       ))
                        {
                            success = true;
                            returnCode = eROReturnCode.Successful;
                        }
                    }


                    return new ROBoolOut(returnCode, null, _ROInstanceID, success);
                }
                finally
                {
                    _characteristicsClass.Unlock();
                }
            }

            // Refresh cache
            if (success)
            {
                _characteristicsClass.CharacteristicsGetData();
            }

            return new ROBoolOut(returnCode, null, _ROInstanceID, success);
        }

        private CharacteristicBase GetCharacteristicClass(eProfileType profileType, int key)
        {
            // return same object if already have one
            switch (profileType)
            {
                case eProfileType.HeaderCharGroup:
                case eProfileType.HeaderChar:
                    if (_characteristicsClass != null
                        && _characteristicsClass is CharacteristicHeader)
                    {
                        return _characteristicsClass;
                    }
                    return new CharacteristicHeader(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.ProductCharacteristic:
                case eProfileType.ProductCharacteristicValue:
                    if (_characteristicsClass != null
                        && _characteristicsClass is CharacteristicProduct)
                    {
                        return _characteristicsClass;
                    }
                    return new CharacteristicProduct(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.StoreCharacteristics:
                case eProfileType.StoreCharacteristicValues:
                    if (_characteristicsClass != null
                        && _characteristicsClass is CharacteristicStore)
                    {
                        return _characteristicsClass;
                    }
                    return new CharacteristicStore(SAB: _SAB, ROWebTools: _ROWebTools);
            }

            return null;
        }

        
    }
}
