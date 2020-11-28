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
    public class CharacteristicHeader : CharacteristicBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public CharacteristicHeader(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.HeaderChar)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHeadersCharacteristics);
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
            _characteristicsProperties = new ROCharacteristicsProperties(eProfileType.HeaderChar);

            // populate characteristicsProperties.Characteristics using Windows\HeaderCharacteristics.cs as a reference
            HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
            DataTable dt = charData.HeaderCharGroup_Read();
            foreach (DataRow drChar in dt.Rows)
            {
                
                int key  = Convert.ToInt32(drChar["HCG_RID"]);
                string characteristicName = Convert.ToString(drChar["HCG_ID"]);
                eCharacteristicValueType charType = (eCharacteristicValueType)(Convert.ToInt32(drChar["HCG_TYPE"]));
                bool isList = Include.ConvertStringToBool((string)drChar["HCG_LIST_IND"]);
                bool isProtect = Include.ConvertStringToBool((string)drChar["HCG_PROTECT_IND"]);
                ROCharacteristicDefinition characteristicDefinition = new ROCharacteristicDefinition(
                    characteristic: new KeyValuePair<int, string>(key, characteristicName),
                    characteristicValueType: charType,
                    isList: isList,
                    isProtect: isProtect
                    );

                GetCharacteristicValues(key: key, characteristicDefinition: characteristicDefinition);

                _characteristicsProperties.Characteristics.Add(characteristicDefinition);
            }

            return _characteristicsProperties;
        }

        private void GetCharacteristicValues(int key, ROCharacteristicDefinition characteristicDefinition)
        {
            HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
            DataTable dt = charData.HeaderCharGroup_ReadValues(key);

            foreach (DataRow drChar in dt.Rows)
            {
                int valueKey = Convert.ToInt32(drChar["HC_RID"]);
                eCharacteristicValueType charType = (eCharacteristicValueType)(Convert.ToInt32(drChar["HCG_TYPE"]));
                int maximumLength = 50;
                object fieldValue = drChar["CHAR_VALUE"];
                ROCharacteristicValue characteristicValue = new ROCharacteristicValue(
                    characteristicValue: new KeyValuePair<int, object>(valueKey, fieldValue),
                    characteristicValueType: charType,
                    maximumLength: maximumLength
                    );
                characteristicDefinition.CharacteristicsValues.Add(characteristicValue);
            }
        }

        /// <summary>
        /// Get the characteristic group and values for a single characteristic.
        /// </summary>
        /// <param name="parms">A ROProfileKeyParms containing the type and key of the characteristic</param>
        /// <returns>A ROCharacteristicsProperties object with a single group and values</returns>
        override public ROCharacteristicsProperties CharacteristicGetData(ROProfileKeyParms parms)
        {
            _currentCharacteristic = new KeyValuePair<int, string>(Include.NoRID, null);

            ROCharacteristicsProperties characteristicsProperties = new ROCharacteristicsProperties(eProfileType.HeaderChar);

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
            int characteristicGroupKey = characteristicDefinition.Characteristic.Key;
            string textValue;
            DateTime? dateValue;
            float? numberValue;
            float? dollarValue;

            ROCharacteristicDefinition originalCharacteristicDefinition;
            HeaderCharacteristicsData charMaint = new HeaderCharacteristicsData();

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            originalCharacteristicDefinition = _characteristicsProperties.Characteristics.Find(c => c.Characteristic.Key == characteristicDefinition.Characteristic.Key);
            if (originalCharacteristicDefinition == null)
            {
                if (ValidGroupName(groupKey: characteristicDefinition.Characteristic.Key,
                                   proposedValue: characteristicDefinition.Characteristic.Value))
                {
                    characteristicGroupKey = charMaint.HeaderCharGroup_Insert(charGroupName: characteristicDefinition.Characteristic.Value,
                                                                  aCharType: (eHeaderCharType)characteristicDefinition.CharacteristicValueType,
                                                                  hasList: characteristicDefinition.IsList,
                                                                  isProtected: characteristicDefinition.IsProtect
                                                                  );
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
                    return false;
                }
            }
            else if (characteristicDefinition.Characteristic.Value != originalCharacteristicDefinition.Characteristic.Value
                || characteristicDefinition.CharacteristicValueType != originalCharacteristicDefinition.CharacteristicValueType
                || characteristicDefinition.IsList != originalCharacteristicDefinition.IsList
                || characteristicDefinition.IsProtect != originalCharacteristicDefinition.IsProtect
                )
            {
                charMaint.HeaderCharGroup_Update(scgRID: characteristicDefinition.Characteristic.Key,
                                                 charGroupName: characteristicDefinition.Characteristic.Value,
                                                 aCharType: (eHeaderCharType)characteristicDefinition.CharacteristicValueType, 
                                                 hasList: characteristicDefinition.IsList,
                                                 isProtected: characteristicDefinition.IsProtect
                                                 );
            }

            ROCharacteristicValue originalCharacteristicValue = null;
            int hcRID = Include.NoRID;
            foreach (ROCharacteristicValue characteristicValue in characteristicDefinition.CharacteristicsValues)
            {
                if (originalCharacteristicDefinition != null)
                {
                    originalCharacteristicValue = originalCharacteristicDefinition.CharacteristicsValues.Find(cv => cv.CharacteristicValue.Key == characteristicValue.CharacteristicValue.Key);
                }


                if (ValidValueName(groupKey: characteristicDefinition.Characteristic.Key,
                    valueKey: characteristicValue.CharacteristicValue.Key,
                    charGroupFieldType: characteristicDefinition.CharacteristicValueType,
                    proposedValue: characteristicValue.CharacteristicValue.Value
                    ))
                {
                    dateValue = null;
                    numberValue = null;
                    dollarValue = null;
                    textValue = null;

                    try
                    {
                        if (characteristicDefinition.CharacteristicValueType == eCharacteristicValueType.date)
                        {
                            dateValue = Convert.ToDateTime(characteristicValue.CharacteristicValue.Value);
                        }
                        else if (characteristicDefinition.CharacteristicValueType == eCharacteristicValueType.number)
                        {
                            numberValue = Convert.ToSingle(characteristicValue.CharacteristicValue.Value);
                        }
                        else if (characteristicDefinition.CharacteristicValueType == eCharacteristicValueType.dollar)
                        {
                            dollarValue = Convert.ToSingle(characteristicValue.CharacteristicValue.Value);
                        }
                        else
                        {
                            textValue = Convert.ToString(characteristicValue.CharacteristicValue.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        message += "Value: " + characteristicValue.CharacteristicValue.Value + " " + ex.Message + Environment.NewLine;
                        continue;
                    }

                    if (characteristicValue.CharacteristicValue.Key == Include.NoRID
                        || originalCharacteristicValue == null
                        || characteristicDefinition.Characteristic.Key == Include.NoRID)
                    {
                        hcRID = charMaint.HeaderCharValue_Insert(hcgRID: characteristicGroupKey,
                                                                 textValue: textValue,
                                                                 dateValue: dateValue,
                                                                 numberValue: numberValue,
                                                                 dollarValue: dollarValue
                                                                 );
                    }
                    else if (ValueChanged(originalCharacteristicValue: originalCharacteristicValue,
                                           charGroupFieldType: characteristicDefinition.CharacteristicValueType,
                                           proposedValue: characteristicValue.CharacteristicValue.Value
                                           )
                            )
                    {
                        charMaint.HeaderCharValue_Update(hcRID: characteristicValue.CharacteristicValue.Key,
                                                         textValue: textValue,
                                                         dateValue: Convert.ToDateTime(characteristicValue.CharacteristicValue.Value),
                                                         numberValue: numberValue,
                                                         dollarValue: dollarValue
                                                         );
                    }

                }
                else if (characteristicDefinition.Characteristic.Key == Include.NoRID)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
                    success = false;
                }
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
            if (parms.ProfileType == eProfileType.HeaderCharGroup)
            {
                return DeleteCharacteristicGroup(key: parms.Key, message: ref message);
            }
            else if (parms.ProfileType == eProfileType.HeaderChar)
            {
                return DeleteCharacteristicValue(key: parms.Key, message: ref message);
            }

            return false;
        }

        private bool DeleteCharacteristicGroup(int key, ref string message)
        {
            HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
            charData.HeaderCharGroup_Delete(key);

            return true;
        }

        private bool DeleteCharacteristicValue(int key, ref string message)
        {
            HeaderCharacteristicsData charData = new HeaderCharacteristicsData();
            charData.HeaderCharValue_Delete(key); 

            return true;
        }

        private bool ValueChanged(ROCharacteristicValue originalCharacteristicValue, eCharacteristicValueType charGroupFieldType, object proposedValue)
        {
            try
            {
                bool valueChanged = false;

                if (charGroupFieldType == eCharacteristicValueType.text)
                {
                    valueChanged = Convert.ToString(originalCharacteristicValue.CharacteristicValue.Value) != Convert.ToString(proposedValue);
                }
                else if (charGroupFieldType == eCharacteristicValueType.date)
                {
                    valueChanged = Convert.ToDateTime(originalCharacteristicValue.CharacteristicValue.Value) != Convert.ToDateTime(proposedValue);
                }
                else if (charGroupFieldType == eCharacteristicValueType.number)
                {
                    valueChanged = Convert.ToSingle(originalCharacteristicValue.CharacteristicValue.Value) != Convert.ToSingle(proposedValue);
                }
                else if (charGroupFieldType == eCharacteristicValueType.dollar)
                {
                    valueChanged = Convert.ToSingle(originalCharacteristicValue.CharacteristicValue.Value) != Convert.ToSingle(proposedValue);
                }

                if (valueChanged)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        private bool ValidGroupName(int groupKey, object proposedValue)
        {
            bool doesGroupAlreadyExist = false;
            HeaderCharacteristicsData charMaintData = new HeaderCharacteristicsData();
            if (charMaintData.DoesHeaderCharGroupNameAlreadyExist((string)proposedValue, groupKey))
            {
                doesGroupAlreadyExist = true;
            }

            if (doesGroupAlreadyExist)
            {
                return false;
            }

            return true;
        }

        private bool ValidValueName(int groupKey, int valueKey, eCharacteristicValueType charGroupFieldType, object proposedValue)
        {
            try
            {

                HeaderCharacteristicsData charMaintData = new HeaderCharacteristicsData();
                bool doesValueAlreadyExist = false;
                int hcRidDuplicate = Include.NoRID;
                if (charGroupFieldType == eCharacteristicValueType.text)
                {
                    doesValueAlreadyExist = charMaintData.DoesHeaderCharValueTextAlreadyExist(Convert.ToString(proposedValue), groupKey, valueKey, ref hcRidDuplicate);
                }
                else if (charGroupFieldType == eCharacteristicValueType.date)
                {
                    doesValueAlreadyExist = charMaintData.DoesHeaderCharValueDateAlreadyExist(Convert.ToDateTime(proposedValue), groupKey, valueKey, ref hcRidDuplicate);
                }
                else if (charGroupFieldType == eCharacteristicValueType.number)
                {
                    doesValueAlreadyExist = charMaintData.DoesHeaderCharValueNumberAlreadyExist(Convert.ToSingle(proposedValue), groupKey, valueKey, ref hcRidDuplicate);
                }
                else if (charGroupFieldType == eCharacteristicValueType.dollar)
                {
                    doesValueAlreadyExist = charMaintData.DoesHeaderCharValueDollarAlreadyExist(Convert.ToSingle(proposedValue), groupKey, valueKey, ref hcRidDuplicate);
                }

                if (doesValueAlreadyExist)
                {
                    return false;
                }

                return true;
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

            if (profileType == eProfileType.HeaderCharGroup)
            {
                lockKey = key;
            }
            else if (profileType == eProfileType.HeaderChar)
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
