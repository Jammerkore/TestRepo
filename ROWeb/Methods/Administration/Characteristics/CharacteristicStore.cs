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
    public class CharacteristicStore : CharacteristicBase
    {
        //=======
        // FIELDS
        //=======
        bool checkForStoreLoadAPI = true;        
        //=============
        // CONSTRUCTORS
        //=============
        public CharacteristicStore(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StoreCharacteristics)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);
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
            _characteristicsProperties = new ROCharacteristicsProperties(eProfileType.StoreCharacteristics);

            // populate characteristicsProperties.Characteristics using Windows\StoreCharacteristics.cs as a reference
            StoreData storeCharMaint = new StoreData();
            DataTable dt = storeCharMaint.StoreCharGroup_Read();
            foreach (DataRow drChar in dt.Rows)
            {

                int key = Convert.ToInt32(drChar["SCG_RID"]);
                string characteristicName = Convert.ToString(drChar["SCG_ID"]);
                eCharacteristicValueType charType = (eCharacteristicValueType)(Convert.ToInt32(drChar["SCG_TYPE"]));
                bool isList = Include.ConvertStringToBool((string)drChar["SCG_LIST_IND"]);
                bool isProtect = false;
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
            DataTable dt = StoreMgmt.StoreCharGroup_ReadValues(key);

            foreach (DataRow drChar in dt.Rows)
            {
                int valueKey = Convert.ToInt32(drChar["SC_RID"]);
                eCharacteristicValueType charType = (eCharacteristicValueType)(Convert.ToInt32(drChar["SCG_TYPE"]));
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

            ROCharacteristicsProperties characteristicsProperties = new ROCharacteristicsProperties(eProfileType.StoreCharacteristics);

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
            message = null;
            ROCharacteristicDefinition originalCharacteristicDefinition = null;
            fieldDataTypes fieldDataType = fieldDataTypes.Text;

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            if (IsStoreLoadRunning())
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnableToSetStoreLock);
                return false;
            }

            try
            {
                int charGroupRID = Include.NoRID;
                int charValueRID = Include.NoRID;
                
                if (characteristicDefinition.Characteristic.Key > 0)
                {
                    originalCharacteristicDefinition = _characteristicsProperties.Characteristics.Find(c => c.Characteristic.Key == characteristicDefinition.Characteristic.Key);
                }
                else
                {
                    originalCharacteristicDefinition = null;
                }

                if (originalCharacteristicDefinition != null)
                {
                    charGroupRID = characteristicDefinition.Characteristic.Key;
                    if (originalCharacteristicDefinition.Characteristic.Value.ToString() == characteristicDefinition.Characteristic.Value.ToString())
                    {
                        if (originalCharacteristicDefinition.IsList != characteristicDefinition.IsList || (eStoreCharType)originalCharacteristicDefinition.CharacteristicValueType != (eStoreCharType)characteristicDefinition.CharacteristicValueType)
                        {
                            //update existing
                            StoreMgmt.StoreCharGroup_Update(characteristicDefinition.Characteristic.Key, characteristicDefinition.Characteristic.Value, (eStoreCharType)characteristicDefinition.CharacteristicValueType, characteristicDefinition.IsList);
                        }
                    }
                    else
                    {
                        //update existing and rename
                        StoreMgmt.StoreCharGroup_Update(characteristicDefinition.Characteristic.Key, characteristicDefinition.Characteristic.Value, (eStoreCharType)characteristicDefinition.CharacteristicValueType, characteristicDefinition.IsList);
                        //update the store group name if the char group name changed and is valid
                        StoreGroupMaint groupMaint = new StoreGroupMaint();
                        int sgRID = groupMaint.StoreGroup_ReadForStoreCharGroupRename(characteristicDefinition.Characteristic.Value.ToString(), Include.GlobalUserRID, characteristicDefinition.Characteristic.Key);
                        if (sgRID != Include.NoRID)
                        {
                            StoreMgmt.StoreGroup_Rename(sgRID, characteristicDefinition.Characteristic.Value.ToString());
                        }
                    }
                }
                else
                {
                    fieldDataType = fieldDataTypes.Text;
                    List<MIDMsg> em = new List<MIDMsg>();
                    bool didInsertNewDynamicStoreGroup = false;
                    int filterRID = Include.NoRID;
                    int charGroupType = (int)characteristicDefinition.CharacteristicValueType; 

                    if (!ValidName(profileType: eProfileType.StoreCharacteristics, charGroupFieldType: fieldDataType, groupKey: characteristicDefinition.Characteristic.Key, valueKey: Include.NoRID, newName: characteristicDefinition.Characteristic.Value))
                    {
                        //insert new
                        charGroupRID = StoreMgmt.StoreCharGroup_Insert(ref em, ref didInsertNewDynamicStoreGroup, ref filterRID, false, SAB.ClientServerSession.UserRID, characteristicDefinition.Characteristic.Value, charTypes.FromIndex(charGroupType).echarGroupType, characteristicDefinition.IsList, string.Empty);
                        if (didInsertNewDynamicStoreGroup) //Filter and its conditions have been saved and committed to the db by now.
                        {
                            filter f = filterDataHelper.LoadExistingFilter(filterRID);
                            StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(f, isNewGroup: true, loadNewResults: true);  //Adds group and levels and results to the db.  Executes the filter.
                        }                                                                                                                                                                                                                                                                                                                                                                            
                    }
                    else
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
                        return false;
                    }
                }
                    
                ROCharacteristicValue originalCharacteristicValue = null;
                foreach (ROCharacteristicValue characteristicValue in characteristicDefinition.CharacteristicsValues)
                {
                    string stringVal = Include.NullForStringValue;
                    DateTime? dateVal = null;
                    float? numericVal = null;
                    float? dollarVal = null;
                    if ((eStoreCharType)characteristicDefinition.CharacteristicValueType == eStoreCharType.date)
                    {
                        dateVal = Convert.ToDateTime(characteristicValue.CharacteristicValue.Value);
                    }
                    else if ((eStoreCharType)characteristicDefinition.CharacteristicValueType == eStoreCharType.number)
                    {
                        numericVal = Convert.ToSingle(characteristicValue.CharacteristicValue.Value);
                    }
                    else if ((eStoreCharType)characteristicDefinition.CharacteristicValueType == eStoreCharType.dollar)
                    {
                        dollarVal = Convert.ToSingle(characteristicValue.CharacteristicValue.Value);
                    }
                    else
                    {
                        stringVal = characteristicValue.CharacteristicValue.Value.ToString();
                    }

                    if (characteristicValue.CharacteristicValue.Key > 0)
                    {
                        originalCharacteristicValue = originalCharacteristicDefinition.CharacteristicsValues.Find(cv => cv.CharacteristicValue.Key == characteristicValue.CharacteristicValue.Key);
                    }
                    else
                    {
                        originalCharacteristicValue = null;
                    }

                    if (originalCharacteristicValue != null)
                    {
                        //check for update
                        int originalValueType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(originalCharacteristicValue.CharacteristicValueType));
                        int newValueType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(characteristicValue.CharacteristicValueType));
                        if (originalCharacteristicValue.CharacteristicValue.Value.ToString() == characteristicValue.CharacteristicValue.Value.ToString())
                        {
                            if (originalValueType != newValueType)
                            {
                                //update existing
                                StoreMgmt.StoreCharValue_Update(characteristicValue.CharacteristicValue.Key, stringVal, dateVal, numericVal, dollarVal);
                            }
                        }
                        else
                        {
                            //update existing and rename
                            StoreMgmt.StoreCharValue_Update(characteristicValue.CharacteristicValue.Key, stringVal, dateVal, numericVal, dollarVal);
                            bool success = RenameGroupLevelsForCharacteristicValue(characteristicValue.CharacteristicValue.Key, originalCharacteristicValue.CharacteristicValue.Value.ToString(), characteristicValue.CharacteristicValue.Value.ToString(), ref message);
                            if (!success)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreCharateristicValue) + message.ToString();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        fieldDataType = fieldDataTypes.Text;
                        if (!ValidName(profileType: eProfileType.StoreCharacteristicValues, charGroupFieldType: fieldDataType, groupKey: characteristicDefinition.Characteristic.Key, valueKey: Include.NoRID, newName: characteristicValue.CharacteristicValue.Value.ToString()))
                        {
                            //insert characteristic value
                            if (charGroupRID > 0)
                            {
                                charValueRID = StoreMgmt.StoreCharValue_Insert(charGroupRID, stringVal, dateVal, numericVal, dollarVal);
                            }
                        }
                        else
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
                            return false;
                        }
                    }
                }
            }
            catch
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreCharateristic);
                return false;
            }
            finally
            {
            }
            
            return true;
        }
        private bool RenameGroupLevelsForCharacteristicValue(int charGroupRid, string originalValue, string currentValue, ref string message)
        {
            try
            {
                StoreGroupProfile sgp;
                string newName;
                DataTable dt = StoreMgmt.StoreGroup_GetFiltersForStoreCharGroup(charGroupRid);
                // Begin TT#1912-MD - JSmith - Str Versioning - Change Value name as user arobinson, go to user pam open the allcocation override method - the set/value name that is change in the method the grid goes blank for that set.  Other sets are fine.
                // Add brackets to name to match attribute set names.
                //originalValue = originalValue.Replace("_", "[_]");
                //currentValue = currentValue.Replace("_", "[_]");
                // End TT#1912-MD - JSmith - Str Versioning - Change Value name as user arobinson, go to user pam open the allcocation override method - the set/value name that is change in the method the grid goes blank for that set.  Other sets are fine.
                foreach (DataRow dr in dt.Rows)
                {
                    sgp = StoreMgmt.StoreGroup_Get(Convert.ToInt32(dr["SG_RID"]));
                    foreach (StoreGroupLevelProfile sglp in sgp.GetGroupLevelList(false))
                    {
                        bool needToRename = false;
                        string[] levelNameSections = sglp.Name.Split(':');
                        for (int i = 0; i < levelNameSections.Length; i++)
                        {
                            if (levelNameSections[i].Trim() == originalValue.Trim())
                            {
                                levelNameSections[i] = currentValue;
                                needToRename = true;
                            }
                        }
                        if (needToRename)
                        {
                            newName = string.Empty;
                            for (int i = 0; i < levelNameSections.Length; i++)
                            {
                                if (i > 0)
                                {
                                    newName = newName + " : ";
                                    needToRename = true;
                                }
                                newName = newName + levelNameSections[i];
                            }
                            StoreMgmt.StoreGroupLevel_Rename(sglp.Key, sglp.LevelVersion, newName, true);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }
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

            if (IsStoreLoadRunning())
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnableToSetStoreLock);
                return false;
            }

            if (parms.ProfileType == eProfileType.StoreCharacteristics)
            {
                return DeleteCharacteristicGroup(parms: parms, message: ref message);
            }
            else if (parms.ProfileType == eProfileType.StoreCharacteristicValues)
            {
                return DeleteCharacteristicValue(parms: parms, message: ref message);
            }

            return false;
        }

        private bool DeleteCharacteristicGroup(ROProfileKeyParms parms, ref string message)
        {
            message = null;
            ROCharacteristicDefinition characteristicDefinition = null;

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }


            characteristicDefinition = _characteristicsProperties.Characteristics.Find(c => c.Characteristic.Key == parms.Key);
            try
            {
                if (characteristicDefinition != null)
                {
                    //delete existing
                    StoreMgmt.StoreCharGroup_Delete(parms.Key);
                    return true;
                }
                else
                {
                    //key does not exist
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreCharateristic);
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            
        }

        private bool DeleteCharacteristicValue(ROProfileKeyParms parms, ref string message)
        {
           
            message = null;
            ROCharacteristicValue characteristicValue = null;

            if (_characteristicsProperties == null)
            {
                CharacteristicsGetData();
            }

            try
            {
                foreach (ROCharacteristicDefinition characteristicDefinition in _characteristicsProperties.Characteristics)
                {
                    characteristicValue = characteristicDefinition.CharacteristicsValues.Find(cv => cv.CharacteristicValue.Key == parms.Key);
                    if (characteristicValue != null)
                    {
                        StoreMgmt.StoreCharValue_Delete(parms.Key);
                        return true;
                    }
                }
                //key does not exist
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreCharateristicValue);
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private bool ValidName( eProfileType profileType, fieldDataTypes charGroupFieldType, int groupKey, int valueKey, string newName)
        {
            int charValueRID = Include.NoRID;
            try
            {

                if (profileType == eProfileType.StoreCharacteristics)
                {
                    return StoreMgmt.DoesStoreCharGroupNameAlreadyExist(newName, Include.NoRID);
                }
                else
                {
                    return StoreMgmt.DoesStoreCharValueAlreadyExist(newName, charGroupFieldType, groupKey, Include.NoRID, ref charValueRID);
                }
            }
            catch
            {
                throw;
            }
        }
        private bool IsStoreLoadRunning()
        {
            try
            {
                //Do not allow deleting adding or editing if Store Load API is running
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    //MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                    return true;
                }
                else
                {
                    //Store Load is Not Running
                    return false;
                }
            }
            catch (Exception ex)
            {
                MIDEnvironment.Message = ex.Message;
                return true;
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
            eLockType lockType = eLockType.StoreCharacteristicGroup;

            if (profileType == eProfileType.StoreCharacteristics)
            {
                lockKey = key;
            }
            else if (profileType == eProfileType.StoreCharacteristicValues)
            {
                lockKey = GetGroupKeyForValueKey(key);
                lockType = eLockType.StoreCharacteristicValue;
            }

            if (lockKey == Include.NoRID)
            {
                return false;
            }

            ArrayList  conflictList = new System.Collections.ArrayList();
            GenericEnqueueCharGroup = new GenericEnqueue(lockType, lockKey, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
            try
            {
                GenericEnqueueCharGroup.EnqueueGeneric();
                _lockStatus = eLockStatus.Locked;
                return true;
            }
            catch (GenericConflictException)
            {
                SetEnqueueConflictMessage(GenericEnqueueCharGroup, GetCharacteristicTypeName(profileType), name);
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
