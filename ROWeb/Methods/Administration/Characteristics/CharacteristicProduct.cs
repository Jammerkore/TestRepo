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
    public class CharacteristicProduct : CharacteristicBase
    {
        //=======
        // FIELDS
        //=======
        private ProductCharProfileList _productCharProfileList;

        //=============
        // CONSTRUCTORS
        //=============
        public CharacteristicProduct(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.ProductCharacteristic)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesCharacteristics);
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        /// <summary>
        /// Get groups and values for all characteristics
        /// </summary>
        /// <returns>A ROCharacteristicsProperties object with all groups and values</returns>
        override public ROCharacteristicsProperties CharacteristicsGetData()
        {
            _characteristicsProperties = new ROCharacteristicsProperties(eProfileType.ProductCharacteristic);

            // populate characteristicsProperties.Characteristics using Windows\ProductCharacteristicMaint.cs as a reference
            // product characteristics are different than store and header is what data is available.  Some will need defaulted
            // so all three types have common data structures.

            _productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();
            foreach (ProductCharProfile pcp in _productCharProfileList)
            {
                int key = pcp.Key;
                string characteristicName = pcp.ProductCharID;
                eCharacteristicValueType charType = eCharacteristicValueType.text;
                bool isList = true;
                bool isProtect = false;
                ROCharacteristicDefinition characteristicDefinition = new ROCharacteristicDefinition(
                    characteristic: new KeyValuePair<int, string>(key, characteristicName),
                    characteristicValueType: charType,
                    isList: isList,
                    isProtect: isProtect
                    );

                GetCharacteristicValues(pcp: pcp, characteristicDefinition: characteristicDefinition);

                _characteristicsProperties.Characteristics.Add(characteristicDefinition);
            }

            _characteristicsProperties.Characteristics.Sort();

            return _characteristicsProperties;
        }

        private void GetCharacteristicValues(ProductCharProfile pcp, ROCharacteristicDefinition characteristicDefinition)
        {
            foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
            {
                int valueKey = pcvp.Key;
                eCharacteristicValueType charType = eCharacteristicValueType.text;
                int maximumLength = 50;
                object fieldValue = pcvp.Text;
                ROCharacteristicValue characteristicValue = new ROCharacteristicValue(
                    characteristicValue: new KeyValuePair<int, object>(valueKey, fieldValue),
                    characteristicValueType: charType,
                    maximumLength: maximumLength
                    );
                characteristicDefinition.CharacteristicsValues.Add(characteristicValue);
            }

            characteristicDefinition.CharacteristicsValues.Sort();
        }

        /// <summary>
        /// Get the characteristic group and values for a single characteristic.
        /// </summary>
        /// <param name="parms">A ROProfileKeyParms containing the type and key of the characteristic</param>
        /// <returns>A ROCharacteristicsProperties object with a single group and values</returns>
        override public ROCharacteristicsProperties CharacteristicGetData(ROProfileKeyParms parms)
        {
            _currentCharacteristic = new KeyValuePair<int, string>(Include.NoRID, null);

            ROCharacteristicsProperties characteristicsProperties = new ROCharacteristicsProperties(eProfileType.ProductCharacteristic);

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            ROCharacteristicDefinition characteristicDefinition = _characteristicsProperties.Characteristics.Find(c => c.Characteristic.Key == parms.Key);

            if (characteristicDefinition != null)
            {
                characteristicsProperties.Characteristics.Add(characteristicDefinition);
                _currentCharacteristic = characteristicDefinition.Characteristic;
            }

            return characteristicsProperties;
        }

        /// <summary>
        /// Saves characteristic changes to the database
        /// </summary>
        /// <param name="characteristicsProperties">The characteristic values</param>
        /// <returns>A flag indicating if the update was successful</returns>
        override public bool CharacteristicSave(ROCharacteristicDefinition characteristicDefinition, out string message)
        {
            bool success = true;
            message = null;
            ROCharacteristicDefinition originalCharacteristicDefinition;
            HierarchyMaintenance hierarchyMaintenance = new HierarchyMaintenance(SAB);

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            bool changePending = false;
            ProductCharProfile pcp = (ProductCharProfile)_productCharProfileList.FindKey(characteristicDefinition.Characteristic.Key);
            if (pcp == null)
            {
                pcp = new ProductCharProfile(characteristicDefinition.Characteristic.Key);
            }
            if (pcp.ProductCharID != characteristicDefinition.Characteristic.Value)
            {
                if (ValidName(hierarchyMaintenance: hierarchyMaintenance, profileType: eProfileType.ProductCharacteristic, groupKey: characteristicDefinition.Characteristic.Key, valueKey: Include.NoRID, newName: characteristicDefinition.Characteristic.Value))
                {
                    pcp.ProductCharID = characteristicDefinition.Characteristic.Value;
                    changePending = true;
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
                    success = false;
                }
            }

            originalCharacteristicDefinition = _characteristicsProperties.Characteristics.Find(c => c.Characteristic.Key == characteristicDefinition.Characteristic.Key);
            if (originalCharacteristicDefinition == null)
            {
                pcp.ProductCharChangeType = eChangeType.add;
            }
            else
            {
                pcp.ProductCharChangeType = eChangeType.update;
            }

            pcp.ProductCharValues.Clear();
            ROCharacteristicValue originalCharacteristicValue = null;
            foreach (ROCharacteristicValue characteristicValue in characteristicDefinition.CharacteristicsValues)
            {
                if (originalCharacteristicDefinition != null)
                {
                    originalCharacteristicValue = originalCharacteristicDefinition.CharacteristicsValues.Find(cv => cv.CharacteristicValue.Key == characteristicValue.CharacteristicValue.Key);
                }
                ProductCharValueProfile pcvp = new ProductCharValueProfile(characteristicValue.CharacteristicValue.Key);
                if (originalCharacteristicValue == null
                    || Convert.ToString(characteristicValue.CharacteristicValue.Value) != Convert.ToString(originalCharacteristicValue.CharacteristicValue.Value))
                {
                    if (ValidName(hierarchyMaintenance: hierarchyMaintenance, profileType: eProfileType.ProductCharacteristicValue, groupKey: characteristicDefinition.Characteristic.Key, valueKey: characteristicValue.CharacteristicValue.Key, newName: Convert.ToString(characteristicValue.CharacteristicValue.Value)))
                    {
                        if (characteristicValue.CharacteristicValue.Key == Include.NoRID
                        || originalCharacteristicValue == null)
                        {
                            pcvp.ProductCharValueChangeType = eChangeType.add;
                        }
                        else
                        {
                            pcvp.ProductCharValueChangeType = eChangeType.update;
                        }
                        changePending = true;
                        pcvp.ProductCharValue = Convert.ToString(characteristicValue.CharacteristicValue.Value);
                    }
                    else
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
                        success = false;
                    }
                }
                pcvp.ProductCharRID = characteristicDefinition.Characteristic.Key;
                pcvp.HasBeenMoved = false;
                pcp.ProductCharValues.Add(pcvp);
            }

            if (changePending)
            {
                pcp = SAB.HierarchyServerSession.ProductCharUpdate(pcp, true);
            }

            return success;
        }

        /// <summary>
        /// Delete the characteristic group and values.
        /// </summary>
        /// <param name="parms">A ROProfileKeyParms containing the type and key of the characteristic</param>
        /// <returns>A flag indicating if the delete was successful</returns>
        override public bool CharacteristicDelete(ROProfileKeyParms parms, out string message)
        {
            message = null;

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            if (parms.ProfileType == eProfileType.ProductCharacteristic)
            {
                return DeleteCharacteristicGroup(parms: parms, message: ref message);
            }
            else if (parms.ProfileType == eProfileType.ProductCharacteristicValue)
            {
                return DeleteCharacteristicValue(parms: parms, message: ref message);
            }

            return false;
        }

        private bool DeleteCharacteristicGroup(ROProfileKeyParms parms, ref string message)
        {
            ProductCharProfile pcp = (ProductCharProfile)_productCharProfileList.FindKey(parms.Key);
            if (pcp != null)
            {
                pcp.ProductCharChangeType = eChangeType.delete;
                pcp = SAB.HierarchyServerSession.ProductCharUpdate(pcp, true);
            }
            else
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharNotFound);
                return false;
            }

            return true;
        }

        private bool DeleteCharacteristicValue(ROProfileKeyParms parms, ref string message)
        {
            bool valueFound = false;
            int groupKey = GetGroupKeyForValueKey(parms.Key);
            ProductCharProfile pcp = (ProductCharProfile)_productCharProfileList.FindKey(groupKey);
            if (pcp != null)
            {
                pcp.ProductCharChangeType = eChangeType.update;
                foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
                {
                    if (pcvp.Key == parms.Key)
                    {
                        pcvp.ProductCharValueChangeType = eChangeType.delete;
                        valueFound = true;
                        break;
                    }
                }
                if (valueFound)
                {
                    pcp = SAB.HierarchyServerSession.ProductCharUpdate(pcp, true);
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharValueNotFound);
                    return false;
                }
            }
            else
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharNotFound);
                return false;
            }
            return true;
        }

        private bool ValidName(HierarchyMaintenance hierarchyMaintenance, eProfileType profileType, int groupKey, int valueKey, string newName)
        {
            try
            {
                
                if (profileType == eProfileType.ProductCharacteristic)
                {
                    return hierarchyMaintenance.IsProductCharNameValid(groupKey, newName);
                }
                else
                {
                    return hierarchyMaintenance.IsProductCharValueValid(valueKey, groupKey, newName);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Locks the characteristic group
        /// </summary>
        /// <param name="characteristicGroupKey">The characteristic group key</param>
        /// <param name="characteristicGroupName">The characteristic group name</param>
        /// <returns>A flag indicating if the lock was successful</returns>
        override public bool CharacteristicLock(eProfileType profileType, int key, string name)
        {
            int lockKey = Include.NoRID;

            if (profileType == eProfileType.ProductCharacteristic)
            {
                lockKey = key;
            }
            else if (profileType == eProfileType.ProductCharacteristicValue)
            {
                lockKey = GetGroupKeyForValueKey(key);
            }

            if (lockKey == Include.NoRID)
            {
                return false;
            }

            GenericEnqueueCharGroup = new GenericEnqueue(eLockType.ProductCharacteristic, lockKey, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            try
            {
                GenericEnqueueCharGroup.EnqueueGeneric();
                _lockStatus = eLockStatus.Locked;
                return true;
            }
            catch (GenericConflictException)
            {
                SetEnqueueConflictMessage(GenericEnqueueCharGroup, GetCharacteristicTypeName(eProfileType.HeaderChar), name);
                _lockStatus = eLockStatus.ReadOnly;
                return false;
            }
            catch (Exception ex)
            {
                MIDEnvironment.Message = ex.Message;
                return false;
            }
        }

    }
}
