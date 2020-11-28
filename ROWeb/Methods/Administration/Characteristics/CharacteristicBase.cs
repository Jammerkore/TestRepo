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
    abstract public class CharacteristicBase
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private eProfileType _profileType;
        protected eLockStatus _lockStatus = eLockStatus.Undefined;
        protected KeyValuePair<int, string> _currentCharacteristic = new KeyValuePair<int, string>(Include.NoRID, null);
        private GenericEnqueue _genericEnqueueCharGroup;
        protected FunctionSecurityProfile _functionSecurity;
        protected ROCharacteristicsProperties _characteristicsProperties = null;

        //=============
        // CONSTRUCTORS
        //=============
        public CharacteristicBase(SessionAddressBlock SAB, ROWebTools ROWebTools, eProfileType profileType)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _profileType = profileType;
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets SessionAddressBlock.
        /// </summary>
        public SessionAddressBlock SAB
        {
            get
            {
                return _SAB;
            }
        }
        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        public eProfileType ProfileType
        {
            get
            {
                return _profileType;
            }
        }

        public GenericEnqueue GenericEnqueueCharGroup
        {
            get
            {
                return _genericEnqueueCharGroup;
            }
            set
            {
                _genericEnqueueCharGroup = value;
            }

        }
        public FunctionSecurityProfile FunctionSecurity
        {
            get
            {
                return _functionSecurity;
            }
        }

        /// <summary>
        /// Gets the lock status
        /// </summary>
        public eLockStatus LockStatus
        {
            get { return _lockStatus; }
        }

        /// <summary>
        /// Gets the key of the last characteristic group
        /// </summary>
        public int Key
        {
            get { return _currentCharacteristic.Key; }
        }

        /// <summary>
        /// Gets the name of the last characteristic group
        /// </summary>
        public string CharacteristicGroupName
        {
            get { return _currentCharacteristic.Value; }
        }

        

        //========
        // METHODS
        //========

        abstract public ROCharacteristicsProperties CharacteristicsGetData();

        abstract public ROCharacteristicsProperties CharacteristicGetData(ROProfileKeyParms parms);

        abstract public bool CharacteristicSave(ROCharacteristicDefinition characteristicDefinition, out string message);

        abstract public bool CharacteristicDelete(ROProfileKeyParms parms, out string message);

        abstract public bool CharacteristicLock(eProfileType profileType, int key, string name);

        public string GetCharacteristicTypeName(eProfileType profileType)
        {
            switch (profileType)
            {
                case eProfileType.HeaderCharGroup:
                case eProfileType.HeaderChar:
                    return MIDText.GetTextOnly(eMIDTextCode.frm_HeaderCharacteristicMaint);
                case eProfileType.ProductCharacteristic:
                case eProfileType.ProductCharacteristicValue:
                    return MIDText.GetTextOnly(eMIDTextCode.frm_ProductCharacteristicMaint);
                case eProfileType.StoreCharacteristics:
                case eProfileType.StoreCharacteristicValues:
                    return MIDText.GetTextOnly(eMIDTextCode.frm_StoreCharacteristicMaint);
            }

            return null;
        }
        
        public void Unlock()
        {
            try
            {
                if (_genericEnqueueCharGroup != null)
                {
                    _genericEnqueueCharGroup.DequeueGeneric();
                    _lockStatus = eLockStatus.Undefined;
                }
            }
            catch
            {
                throw;
            }
        }

        protected void SetEnqueueConflictMessage(GenericEnqueue objEnqueue, string objectTypeName, string objectName = "")
        {
            string[] errParms = new string[3];
            errParms.SetValue(objectTypeName, 0);
            errParms.SetValue(objectName, 1);
            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
            string errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
            MIDEnvironment.Message += errMsg;
        }

        protected int GetGroupKeyForValueKey(int key)
        {

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            foreach (ROCharacteristicDefinition characteristicDefinition in _characteristicsProperties.Characteristics)
            {
                ROCharacteristicValue characteristicsValues = characteristicDefinition.CharacteristicsValues.Find(v => v.CharacteristicValue.Key == key);
                if (characteristicsValues != null)
                {
                    return characteristicDefinition.Characteristic.Key;
                }
            }

            return Include.NoRID;
        }

    }
}
