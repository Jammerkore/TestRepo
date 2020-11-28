using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{

    public enum StoreLoadRecordActions
    {
        processOptions = 0, //processed immediately, as it affects which records get loaded.
        processCharacteristic = 1,
        processMarkStoreForDeletion = 2,
        processRecoverStoreFromDeletion = 3,
        processStore = 4
    }

    //holds all options for processing records
    public class StoreLoadProcessingOptions
    {
        public bool autoAddCharacteristics;
        public string characteristicDelimiter;
        public bool useCharacteristicTransaction;
    }

    public class StoreLoadCharacteristicRecord
    {
        public eStoreCharType storeCharType;
        public string storeCharName;
        public string storeCharValue;
    }

    public class MIDMsg
    {
        public eMIDMessageLevel msgLevel;
        public eMIDTextCode textCode = eMIDTextCode.Unassigned;
        public string msg;
    }

    //holds all information pertaining to a single store load record
    public class StoreLoadRecord
    {
        public StoreLoadRecordActions recordAction;

        public bool hasError = false;
        public bool isNewlyAdded = false; //used to track whether this is a new store
        //public eMIDMessageLevel errorMsgLevel;
        //public eMIDTextCode errorTextCode = eMIDTextCode.Unassigned;
        //public string errorMsg;
        public List<MIDMsg> msgList = new List<MIDMsg>();  
        //public List<MIDMsg> infoMsgList = new List<MIDMsg>(); 

        public string StoreID = string.Empty;
        public string StoreName = null;
        public string StoreDescription = null;
        public string City = null;
        public string State = null;
        public string SellingSqFt = null;
        public string SellingOpenDate = null;
        public string SellingCloseDate = null;
        public string StockOpenDate = null;
        public string StockCloseDate = null;
        public string ActiveIndicator = null;
        public string LeadTime = null;
        public string ShipDate = null;
        public string ImoId = null;

        public List<StoreLoadCharacteristicRecord> characteristicRecordList = new List<StoreLoadCharacteristicRecord>();

        public object FieldValueGetCurrent(int objectRID, int objectType, int fieldIndex)
        {
            //called to get correpsonding date fields and the store ID
            if (objectType == storeObjectTypes.StoreFields) //Store Fields
            {
                if (fieldIndex == storeFieldTypes.StoreID)
                {
                    return this.StoreID;
                }
                else if (fieldIndex == storeFieldTypes.SellingOpenDate)
                {
                    return this.SellingOpenDate;
                }
                else if (fieldIndex == storeFieldTypes.SellingCloseDate)
                {
                    return this.SellingCloseDate;
                }
                else if (fieldIndex == storeFieldTypes.StockOpenDate)
                {
                    return this.StockOpenDate;
                }
                else if (fieldIndex == storeFieldTypes.StockCloseDate)
                {
                    return this.StockCloseDate;
                }
                
            }
            return null;
        }
        public void FieldValueSetCurrent(int objectRID, int objectType, int fieldIndex, object val)
        {
           //called to set the status
           //Store Load will set the status elsewhere 
        }
    }


    public class StoreLoadProcessManager
    {
        private SessionAddressBlock _SAB;
        private GenericEnqueue genericEnqueueStoreLoad;
        public StoreLoadProcessManager(SessionAddressBlock SAB)
		{
            _SAB = SAB;
            StoreValidation.SetSAB(SAB); 
		}

        public bool ProcessDelimitedFile(string delimitedFileName, char[] delimiter, bool autoAddCharacteristics, char[] characteristicDelimiter, bool useCharacteristicTransaction)
        {
            try
            {
                StoreLoadLock();
                StoreLoadProcessingOptions processingOptions;
                List<StoreLoadRecord> recordList = StoreLoadDelimited.GetRecordsFromDelimitedFile(delimitedFileName, delimiter, autoAddCharacteristics, useCharacteristicTransaction, out processingOptions);
                return ProcessStoreLoadRecords(processingOptions, recordList);
            }
            catch
            {
                throw;
            }
            finally
            {
                StoreLoadUnlock();
            }
        }
        public bool ProcessXmlFile(string xmlLoadFileName, bool autoAddCharacteristics)
		{
            try
            {
                StoreLoadLock();
                StoreLoadProcessingOptions processingOptions;
                List<StoreLoadRecord> recordList = StoreLoadXML.GetRecordsFromXmlFile(xmlLoadFileName, autoAddCharacteristics, out processingOptions);
                return ProcessStoreLoadRecords(processingOptions, recordList);
            }
            catch
            {
                throw;
            }
            finally
            {
                StoreLoadUnlock();
            }
        }

        private void StoreLoadLock()
        {
            try
            {
                genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, _SAB.ClientServerSession.UserRID, _SAB.ClientServerSession.ThreadID);
                genericEnqueueStoreLoad.EnqueueGeneric();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void StoreLoadUnlock()
        {
            try
            {
                genericEnqueueStoreLoad.DequeueGeneric();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool ProcessStoreLoadRecords(StoreLoadProcessingOptions processingOptions, List<StoreLoadRecord> recordList)
        {
            bool errorFound = false;

            try
            {
                int recordsAdded = 0;
                int recordsUpdated = 0;
                int recordsDeleted = 0;
                int recordsRecovered = 0;
                bool hasActiveIndicatorChangedOnAnyStore = false;
                List<storeCharInfo> charChangedList = new List<storeCharInfo>();
                List<int> fieldChangedList = new List<int>();

                //process the records in the list
                foreach (StoreLoadRecord storeRecord in recordList)
                {
                    if (storeRecord.hasError)
                    {
                        errorFound = true;
                        AddMsgsToAudit(storeRecord.msgList); //Add messages to the audit
                        continue;
                    }

                    //process the store load record
                    if (storeRecord.recordAction == StoreLoadRecordActions.processOptions)
                    {
                        //options are processed immediately, so no action needed here
                    }
                    else if (storeRecord.recordAction == StoreLoadRecordActions.processMarkStoreForDeletion)
                    {
                        ProcessMarkStoreForDeletion(storeRecord);
                        if (storeRecord.hasError == false)
                        {
                            recordsDeleted++;
                        }
                    }
                    else if (storeRecord.recordAction == StoreLoadRecordActions.processRecoverStoreFromDeletion)
                    {                     
                        ProcessRecoverStoreFromDeletion(storeRecord);
                        if (storeRecord.hasError == false)
                        {
                            recordsRecovered++;
                        }
                    }
                    else if (storeRecord.recordAction == StoreLoadRecordActions.processCharacteristic)
                    {
                        ProcessCharacteristicRecord(processingOptions, storeRecord, charChangedList);
                        if (storeRecord.hasError == false)
                        {
                            recordsUpdated++;
                        }
                    }
                    else if (storeRecord.recordAction == StoreLoadRecordActions.processStore)
                    {
                        ProcessStoreRecord(processingOptions, storeRecord, fieldChangedList, charChangedList, ref hasActiveIndicatorChangedOnAnyStore);
                        if (storeRecord.hasError == false)
                        {
                            if (storeRecord.isNewlyAdded)
                            {
                                recordsAdded++;
                            }
                            else
                            {
                                recordsUpdated++;
                            }
                        }
                    }

                    if (storeRecord.hasError) //check for new errors that occured during processing
                    {
                        errorFound = true;
                    }
                    if (storeRecord.msgList.Count > 0)
                    {
                        List<MIDMsg> msgListForAudit = storeRecord.msgList.FindAll(x => x.msgLevel != eMIDMessageLevel.Ignore);
                        AddMsgsToAudit(msgListForAudit); //Add messages to the audit  (there can be both information messages and error messages)
                    }
                }

                int recordsWithErrors = recordList.FindAll(x => x.hasError).Count;

                _SAB.StoreServerSession.Audit.StoreLoadAuditInfo_Add(recordList.Count, recordsWithErrors, recordsAdded, recordsUpdated, recordsDeleted, recordsRecovered);


                //Now smartly refresh store groups
                StoreMgmt.ProgressBarOptions pOpt = new StoreMgmt.ProgressBarOptions();
                pOpt.useProgressBar = false; //do not show a UI progress bar
                if (recordsAdded > 0 || hasActiveIndicatorChangedOnAnyStore)
                {
                    //If we add a store, or if we activate/deactivate a store, all the store groups must be refreshed
					// Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
					//StoreMgmt.StoreGroups_Refresh(pOpt);
                    StoreMgmt.StoreGroups_Refresh(pOpt, refreshSessions : false);
					// End TT#2078-MD - JSmith - Object Reference error updating Store Group
                }
                else
                {
                    // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
					//StoreMgmt.StoreGroups_RefreshForChangedFieldsAndChars(charChangedList, fieldChangedList, pOpt);
					StoreMgmt.StoreGroups_RefreshForChangedFieldsAndChars(charChangedList, fieldChangedList, pOpt, refreshSessions: false);
					// End TT#2078-MD - JSmith - Object Reference error updating Store Group
                }
            }
            catch
            {
                throw;
            }

            return errorFound;
        }

        private void ProcessStoreRecord(StoreLoadProcessingOptions processingOptions, StoreLoadRecord storeRecord, List<int> fieldChangedList, List<storeCharInfo> charChangedList, ref bool hasActiveIndicatorChanged)
        {
            try
            {
                StoreProfile storeProfile;
        
                ValidateFieldsAndSetOnStoreProfile(storeRecord, out storeProfile);
                if (storeRecord.hasError)
                {
                    return;
                }
             
            

                List<storeCharInfo> charList = new List<storeCharInfo>();
    
                foreach (StoreLoadCharacteristicRecord charRecord in storeRecord.characteristicRecordList)
                {
                     storeCharInfo charInfo = new storeCharInfo();
                     charInfo.stRID = storeProfile.Key;
                     //If autoAddCharacteristic processing option is true, add the characteristic to the database and characteristic value to the database.  Also can create a new dynamic store group for that characteristic.
                     AddStoreCharacteristics(processingOptions, storeRecord, charRecord.storeCharName, charRecord.storeCharValue, charRecord.storeCharType, ref charInfo);
                     charList.Add(charInfo);
                }

                if (storeRecord.hasError)
                {
                    return;
                }

                StoreMgmt.ProgressBarOptions pOpt = new StoreMgmt.ProgressBarOptions();
                pOpt.useProgressBar = false;
                if (storeProfile.Key == Include.UndefinedStoreRID)
                {
                    storeRecord.isNewlyAdded = true;
                    StoreMgmt.StoreProfile_Add(storeProfile, charList, pOpt);
                }
                else 
                {
                    //compare this store profiles fields and char values to the current and denote what has changed.
                    bool hasStoreFieldOrCharChanged = false;

                    StoreMaint storeMaintData = new StoreMaint();
                    DataSet dsValues = storeMaintData.ReadStoresFieldsForMaint(storeProfile.Key);
                    int key = -1;
                    if (dsValues.Tables[0].Rows[0]["ST_RID"] != DBNull.Value)
                    {
                        key = Convert.ToInt32(dsValues.Tables[0].Rows[0]["ST_RID"], CultureInfo.CurrentUICulture);
                    }
                    StoreProfile prof = new StoreProfile(key);
                    prof.LoadFieldsFromDataRow(dsValues.Tables[0].Rows[0]);


                    if (prof.ActiveInd != storeProfile.ActiveInd)
                    {
                        hasActiveIndicatorChanged = true;
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ActiveInd);                    
                    }
                    if (prof.City != storeProfile.City)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.City);                    
                    }
                    if (prof.IMO_ID != storeProfile.IMO_ID)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ImoID);                    
                    }
                    if (prof.LeadTime != storeProfile.LeadTime)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.LeadTime);                    
                    }
                    if (prof.SellingCloseDt != storeProfile.SellingCloseDt)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.SellingCloseDate);                    
                    }
                    if (prof.SellingOpenDt != storeProfile.SellingOpenDt)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.SellingOpenDate);                    
                    }
                    if (prof.SellingSqFt != storeProfile.SellingSqFt)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.SellingSqFt);                    
                    }
                    if (prof.ShipOnFriday != storeProfile.ShipOnFriday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnFriday);                    
                    }
                    if (prof.ShipOnMonday != storeProfile.ShipOnMonday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnMonday);                    
                    }
                    if (prof.ShipOnSaturday != storeProfile.ShipOnSaturday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnSaturday);                    
                    }
                    if (prof.ShipOnSunday != storeProfile.ShipOnSunday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnSunday);                    
                    }
                    if (prof.ShipOnThursday != storeProfile.ShipOnThursday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnThursday);                    
                    }
                    if (prof.ShipOnTuesday != storeProfile.ShipOnTuesday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnTuesday);                    
                    }
                    if (prof.ShipOnWednesday != storeProfile.ShipOnWednesday)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.ShipOnWednesday);                    
                    }
                    if (prof.SimilarStoreModel != storeProfile.SimilarStoreModel)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.SimilarStoreModel);                    
                    }
                    if (prof.State != storeProfile.State)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.State);                    
                    }
                    if (prof.StockCloseDt != storeProfile.StockCloseDt)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.StockCloseDate);
                    }
                    if (prof.StockOpenDt != storeProfile.StockOpenDt)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.StockOpenDate);
                    }
                    if (prof.DeleteStore != storeProfile.DeleteStore)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.StoreDeleteInd);
                    }
                    if (prof.StoreDescription != storeProfile.StoreDescription)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.StoreDescription);
                    }
                    if (prof.StoreName != storeProfile.StoreName)
                    {
                        hasStoreFieldOrCharChanged = true;
                        AddFieldToChangedList(fieldChangedList, storeFieldTypes.StoreName);
                    }
                

                    List<int> currentCharGroups = new List<int>();

                    if (charList.Count > 0)
                    {
                        foreach (DataRow drChar in dsValues.Tables[1].Rows)
                        {

                            int scgRID = Convert.ToInt32(drChar["SCG_RID"]);
                            currentCharGroups.Add(scgRID);

                            string charValue = string.Empty;
                            if (drChar["CHAR_VALUE"] != DBNull.Value)
                            {
                                charValue = Convert.ToString(drChar["CHAR_VALUE"]);
                            }

                            storeCharInfo charInfo = charList.Find(x => x.scgRID == scgRID && x.anyValue == charValue);
                            if (charInfo != null)
                            {
                                //the values match, so we do not include this scgRID
                            }
                            else
                            {
                                hasStoreFieldOrCharChanged = true;
                                AddCharToChangedList(charChangedList, scgRID);
                            }
                        }
                    }
                    //now lets check for new characteristics
                    foreach (storeCharInfo charInfo in charList)
                    {
                        int possibleNewScgRID = charInfo.scgRID;
                        if (currentCharGroups.Exists(x => x == possibleNewScgRID) == false)
                        {
                            //this is a new char group so add to the change list.
                            hasStoreFieldOrCharChanged = true;
                            AddCharToChangedList(charChangedList, possibleNewScgRID);
                        }
                    }
            

                    if (hasStoreFieldOrCharChanged)
                    {
                        bool tempbool = false;
                        StoreMgmt.StoreProfile_Update(storeProfile, false, ref tempbool, pOpt, charList); 
                    }
                }
               
             
            }
            catch(Exception ex)
            {
                throw new Exception("Error processing store record: " + storeRecord.StoreID + System.Environment.NewLine + ex.ToString());
            }
        }

        private void AddFieldToChangedList(List<int> fieldChangedList, storeFieldTypes fieldType)
        {
            try
            {
                filterStoreFieldTypes filterFieldType = filterStoreFieldTypes.FromField(fieldType);
                if (filterFieldType != null)
                {
                    if (fieldChangedList.Exists(x => x == filterFieldType.dbIndex) == false)
                    {
                        fieldChangedList.Add(filterFieldType.dbIndex);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void AddCharToChangedList(List<storeCharInfo> charChangedList, int scgRID)
        {
            try
            {
                if (charChangedList.Exists(x => x.scgRID == scgRID) == false)
                {
                    storeCharInfo changedCharInfo = new storeCharInfo();
                    changedCharInfo.isDirty = true;
                    changedCharInfo.scgRID = scgRID;
                    charChangedList.Add(changedCharInfo);
                }
            }
            catch
            {
                throw;
            }
        }

     
        private void ProcessMarkStoreForDeletion(StoreLoadRecord storeRecord)
        {
            try
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeRecord.StoreID);

                //if (storeProfile.Key == Include.UndefinedStoreRID)
                //{
                //    string msgDetails = "Store: " + storeRecord.StoreID;
                //    storeRecord.hasError = true;
                //    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotFound, msg = msgDetails });
                //    return;
                //}

                //if (storeProfile.ActiveInd == true) // A delete transaction was found, but the store was still active
                //{
                //    string msgDetails = "Store: " + storeRecord.StoreID;
                //    storeRecord.hasError = true;
                //    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotInactiveForDelete, msg = msgDetails });
                //    return;
                //}

                //if (storeProfile.Key == StoreMgmt.ReserveStoreRID)
                //{
                //    string msgDetails = "Store: " + storeRecord.StoreID;
                //    storeRecord.hasError = true;
                //    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_CannotDeleteReserveStore, msg = msgDetails });
                //    return;
                //}

                if (StoreValidation.IsStoreDeleteIndicatorValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, storeProfile.StoreId, storeProfile.ActiveInd, proposedValue: true, originalValue: storeProfile.DeleteStore) == false)
                {
                    storeRecord.hasError = true;
                    return;
                }



                StoreMgmt.StoreProfile_MarkForDelete(storeProfile, true);
            }
            catch
            {
                throw;
            }
        }
        private void ProcessRecoverStoreFromDeletion(StoreLoadRecord storeRecord)
        {
            try
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeRecord.StoreID);
                //if (storeProfile.Key == Include.UndefinedStoreRID)
                //{
                //    string msgDetails = "Store: " + storeRecord.StoreID;
                //    storeRecord.hasError = true;
                //    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotFound, msg = msgDetails });
                //    return;
                //}

                if (StoreValidation.IsStoreDeleteIndicatorValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, storeProfile.StoreId, storeProfile.ActiveInd, proposedValue: false, originalValue: storeProfile.DeleteStore) == false)
                {
                    storeRecord.hasError = true;
                    return;
                }

                StoreMgmt.StoreProfile_MarkForDelete(storeProfile, false);
            }
            catch
            {
                throw;
            }
        }

        private void ProcessCharacteristicRecord(StoreLoadProcessingOptions processingOptions, StoreLoadRecord storeRecord, List<storeCharInfo> charChangedList)
        {
            try
            {
                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeRecord.StoreID); 

                // Check to be sure store exists
                if (storeProfile.Key == Include.UndefinedStoreRID)
                {
                    string msgDetails = "Store: " + storeRecord.StoreID;
                    storeRecord.hasError = true;
                    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_CharacteristicAddedPriorToStore, msg = msgDetails });
                    return;
                }

                StoreMaint storeMaintData = new StoreMaint();
                DataSet dsValues = storeMaintData.ReadStoresFieldsForMaint(storeProfile.Key);

                List<storeCharInfo> charList = null;
                if (storeRecord.characteristicRecordList.Count > 0)
                {
                    charList = new List<storeCharInfo>();
                }
                foreach (StoreLoadCharacteristicRecord charRecord in storeRecord.characteristicRecordList)
                {
                     storeCharInfo charInfo = new storeCharInfo();
                     charInfo.stRID = StoreMgmt.StoreProfile_GetStoreRidFromId(storeRecord.StoreID);
                     //If autoAddCharacteristic processing option is true, add the characteristic to the database and characteristic value to the database.  Also can create a new dynamic store group for that characteristic.
                     AddStoreCharacteristics(processingOptions, storeRecord, charRecord.storeCharName, charRecord.storeCharValue, charRecord.storeCharType, ref charInfo);
                     charList.Add(charInfo);
                }

                if (storeRecord.hasError == false)
                {
                    // Determine changes for attribute rebuild
                    if (charList != null)
                    {
                        List<int> currentCharGroups = new List<int>();

                        if (charList.Count > 0)
                        {
                            foreach (DataRow drChar in dsValues.Tables[1].Rows)
                            {

                                int scgRID = Convert.ToInt32(drChar["SCG_RID"]);
                                currentCharGroups.Add(scgRID);

                                string charValue = string.Empty;
                                if (drChar["CHAR_VALUE"] != DBNull.Value)
                                {
                                    charValue = Convert.ToString(drChar["CHAR_VALUE"]);
                                }

                                if (charList.Exists(x => x.scgRID == scgRID) == true)
                                {
                                    storeCharInfo charInfo = charList.Find(x => x.scgRID == scgRID && x.anyValue == charValue);
                                    if (charInfo != null)
                                    {
                                        //the values match, so we do not include this scgRID
                                    }
                                    else
                                    {
                                        AddCharToChangedList(charChangedList, scgRID);
                                    }
                                }
                            }
                        }
                        //now lets check for new characteristics
                        foreach (storeCharInfo charInfo in charList)
                        {
                            int possibleNewScgRID = charInfo.scgRID;
                            if (currentCharGroups.Exists(x => x == possibleNewScgRID) == false)
                            {
                                //this is a new char group so add to the change list.
                                AddCharToChangedList(charChangedList, possibleNewScgRID);
                            }
                        }
                    }

                    //Set the characteristic values on this store
                    bool tempbool = false;
                    StoreMgmt.ProgressBarOptions pOpt = new StoreMgmt.ProgressBarOptions();
                    pOpt.useProgressBar = false;
                    StoreMgmt.StoreProfile_Update(storeProfile, doRefreshGroups: false, doRefreshNodes: ref tempbool, progessbarOptions: pOpt, charList: charList);
                }
            }
            catch
            {
                throw;
            }
        }
  
      
        public void ValidateFieldsAndSetOnStoreProfile(StoreLoadRecord storeRecord, out StoreProfile storeProfile)
        {
            //delay profile modification until after validation
            bool profileActiveInd = false;
            bool? profileSimilarStoreModelInd = null;  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
            string profileCity = null;
            string profileState = null;
            int? profileLeadTime = null;
            int? profileSellingSqFt = null;
            DateTime? profileSellingCloseDt = null;
            DateTime? profileSellingOpenDt = null;
            DateTime? profileStockCloseDt = null;
            DateTime? profileStockOpenDt = null;
            bool? profileShipOnFriday = null;
            bool? profileShipOnMonday = null;
            bool? profileShipOnSaturday = null;
            bool? profileShipOnSunday = null;
            bool? profileShipOnThursday = null;
            bool? profileShipOnTuesday = null;
            bool? profileShipOnWednesday = null;
            string profileStoreName = null;
            string profileStoreId = null;
            string profileStoreDescription = null;
            string profileIMO_ID = null;



            storeProfile = (StoreProfile)StoreMgmt.StoreProfile_Get(storeRecord.StoreID).Clone();  //Making a copy for existing stores, so we do not immediately edit unintentionally.  New stores will have a store RID of -1


            //Validate the store ID first, since it is used to identify the store later for other validation.
            if (storeRecord.StoreID != null)
            {
                profileStoreId = storeRecord.StoreID.Trim();
            }
            if (StoreValidation.IsStoreIDValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeProfile.Key) == false)
            {
                storeRecord.hasError = true;
                return; // do not proceed if the ID is invalid
            }

            //Validate the store name.
            if (storeRecord.StoreName != null)
            {
                profileStoreName = storeRecord.StoreName.Trim();
            }
            if (StoreValidation.IsStoreNameValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, profileStoreName) == false)
            {
                storeRecord.hasError = true;
            }

            //Validate the description.
            if (storeRecord.StoreDescription != null)
            {
                profileStoreDescription = storeRecord.StoreDescription.Trim();
            }
            else
            {
                if (storeRecord.StoreName != null)
                {
                    profileStoreDescription = storeRecord.StoreName.Trim();
                }
            }
            if (StoreValidation.IsDescriptionValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, profileStoreDescription) == false)
            {
                storeRecord.hasError = true;
            }



            //Validate the active indicator.  This can go to the database if deactiving a store.
            object tempActiveInd;
            if (storeRecord.ActiveIndicator == null)
            {
                tempActiveInd = true;
            }
            else
            {
                tempActiveInd = storeRecord.ActiveIndicator;
            }
            // Begin TT#1860-MD - JSmith - Incorrect message attempting to set store inactive
            //if (StoreValidation.IsStoreActiveIndicatorValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, profileStoreId, storeProfile.DeleteStore, profileActiveInd, storeProfile.ActiveInd) == false)
            if (StoreValidation.IsStoreActiveIndicatorValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, profileStoreId, storeProfile.DeleteStore, storeRecord.ActiveIndicator, storeProfile.ActiveInd) == false)
            // End TT#1860-MD - JSmith - Incorrect message attempting to set store inactive
            {
                storeRecord.hasError = true;
            }
            else
            {
                // Begin TT#5648-MD - AGallagher -Store Load - Active Indicator Flag is defaulting to "false" when loading new stores (Hunkemoller)
                //profileActiveInd = Convert.ToBoolean(storeRecord.ActiveIndicator, CultureInfo.CurrentUICulture);
                profileActiveInd = Convert.ToBoolean(tempActiveInd, CultureInfo.CurrentUICulture);
                // End TT#5648-MD - AGallagher -Store Load - Active Indicator Flag is defaulting to "false" when loading new stores (Hunkemoller)
            }

            //Validate the store delete indicator.  This is currently handled as a different record type.
            //if (StoreValidation.IsStoreDeleteIndicatorValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, profileStoreId, profileActiveInd, profileDeleteInd, storeProfile.DeleteStore) == false)
            //{
            //    storeRecord.hasError = true;
            //}


            //Validate the city.
            if (storeRecord.City != null)
            {
                profileCity = storeRecord.City.Trim();
            }
            if (StoreValidation.IsCityValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, profileCity) == false)
            {
                storeRecord.hasError = true;
            }

            //Validate the state.
            if (storeRecord.State != null)
            {
                profileState = storeRecord.State.Trim();
            }
            if (StoreValidation.IsStateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, profileState) == false)
            {
                storeRecord.hasError = true;
            }

            if (storeRecord.ImoId != null)
            {
                profileIMO_ID = storeRecord.ImoId.Trim();
            }
            // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
            //if (StoreValidation.IsImoIdValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, profileIMO_ID) == false)
            if (StoreValidation.IsImoIdValid(storeRecord.msgList, validationKinds.UponSaving, storeProfile.Key, profileStoreId, profileIMO_ID) == false)
            // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
            {
                storeRecord.hasError = true;
            }

 
            //Validate the lead time.
            if (StoreValidation.IsLeadTimeValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.LeadTime) == false)
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.LeadTime != null && storeRecord.LeadTime != string.Empty)
                {
                    profileLeadTime = Convert.ToInt32(storeRecord.LeadTime, CultureInfo.CurrentUICulture);
                }
            }

            //Validate the selling square footage.
            if (StoreValidation.IsSellingSqFtValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.LeadTime) == false)
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.SellingSqFt != null && storeRecord.SellingSqFt != string.Empty)
                {
                    profileSellingSqFt = Convert.ToInt32(storeRecord.SellingSqFt, CultureInfo.CurrentUICulture);
                }
            }

            //Validate the selling open date.
			// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            //if (StoreValidation.IsSellingOpenDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.SellingCloseDate, storeRecord.SellingOpenDate) == false)
            if (StoreValidation.IsSellingOpenDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.SellingCloseDate, storeRecord.SellingOpenDate, _SAB.StoreServerSession) == false)
			// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.SellingOpenDate != null && storeRecord.SellingOpenDate != string.Empty)
                {
                    // Begin TT#1862 - JSmith - Store Open Date has Store CLose Date value
                    //profileSellingOpenDt = Convert.ToDateTime(storeRecord.SellingCloseDate, CultureInfo.CurrentUICulture);
                    profileSellingOpenDt = Convert.ToDateTime(storeRecord.SellingOpenDate, CultureInfo.CurrentUICulture);
                    // End TT#1862 - JSmith - Store Open Date has Store CLose Date value
                }
            }

            //Validate the selling close date.
			// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            //if (StoreValidation.IsSellingCloseDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.SellingOpenDate, storeRecord.SellingCloseDate) == false)
            if (StoreValidation.IsSellingCloseDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.SellingOpenDate, storeRecord.SellingCloseDate, _SAB.StoreServerSession) == false)
			// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.SellingCloseDate != null && storeRecord.SellingCloseDate != string.Empty)
                {
                    profileSellingCloseDt = Convert.ToDateTime(storeRecord.SellingCloseDate, CultureInfo.CurrentUICulture);
                }
            }

            //Validate the stock open date.
			// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            //if (StoreValidation.IsStockOpenDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.StockCloseDate, storeRecord.StockOpenDate) == false)
            if (StoreValidation.IsStockOpenDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.StockCloseDate, storeRecord.StockOpenDate, _SAB.StoreServerSession) == false)
			// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.StockOpenDate != null && storeRecord.StockOpenDate != string.Empty)
                {
                    profileStockOpenDt = Convert.ToDateTime(storeRecord.StockOpenDate, CultureInfo.CurrentUICulture);
                }
            }

            //Validate the stock close date.
			// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            //if (StoreValidation.IsStockCloseDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.StockOpenDate, storeRecord.StockCloseDate) == false)
            if (StoreValidation.IsStockCloseDateValid(storeRecord.msgList, validationKinds.UponSaving, profileStoreId, storeRecord.StockOpenDate, storeRecord.StockCloseDate, _SAB.StoreServerSession) == false)
			// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
            {
                storeRecord.hasError = true;
            }
            else
            {
                if (storeRecord.StockCloseDate != null && storeRecord.StockCloseDate != string.Empty)
                {
                    profileStockCloseDt = Convert.ToDateTime(storeRecord.StockCloseDate, CultureInfo.CurrentUICulture);
                }
            }
     
            //Validate the ship days allowed
            if (storeRecord.ShipDate != null)
            {
                profileShipOnFriday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("F") > -1);	// So I just check to see if the letter is in there and return that boolean
                profileShipOnMonday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("M") > -1);
                profileShipOnSaturday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SA") > -1);
                profileShipOnSunday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("SU") > -1);
                profileShipOnThursday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TH") > -1);
                profileShipOnTuesday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("TU") > -1);
                profileShipOnWednesday = (storeRecord.ShipDate.ToUpper(CultureInfo.CurrentUICulture).IndexOf("W") > -1);
            }



        

     





            storeProfile.SetFieldsOnProfile(
                profileActiveInd: profileActiveInd,
                profileSimilarStoreModelInd: profileSimilarStoreModelInd,  // TT1842-MD - JSmith - Similar Store Model - After selecting a store in Edit Fields or Stores the setting does not save.
                profileCity: profileCity,
                profileState: profileState,
                profileLeadTime: profileLeadTime,
                profileSellingCloseDt: profileSellingCloseDt,
                profileSellingOpenDt: profileSellingOpenDt,
                profileSellingSqFt: profileSellingSqFt,
                profileShipOnFriday: profileShipOnFriday,
                profileShipOnMonday: profileShipOnMonday,
                profileShipOnSaturday: profileShipOnSaturday,
                profileShipOnSunday: profileShipOnSunday,
                profileShipOnThursday: profileShipOnThursday,
                profileShipOnTuesday: profileShipOnTuesday,
                profileShipOnWednesday: profileShipOnWednesday,
                profileStockCloseDt: profileStockCloseDt,
                profileStockOpenDt: profileStockOpenDt,
                profileStoreName: profileStoreName,
                profileStoreId: profileStoreId,
                profileStoreDescription: profileStoreDescription,
                profileIMO_ID: profileIMO_ID
                );

        }


    
        public void AddStoreCharacteristics(StoreLoadProcessingOptions processingOptions, StoreLoadRecord storeRecord, string aStoreCharacteristic, string Value, eStoreCharType aStoreCharType, ref storeCharInfo charInfo)
        {

            charInfo.isDirty = true;
            charInfo.action = storeCharInfoAction.InsertValue;
            charInfo.dataType = fieldDataTypes.Text;  // TT#1865-MD - JSmith - Duplicates Characteristic Values


            aStoreCharacteristic = aStoreCharacteristic.Trim();
            // Begin Issue 3691 - stodd 2/7/2006
            if (Value != null)
                Value = Value.Trim();
            // End issue 3691

            // Begin TT#1865-MD - JSmith - Duplicates Characteristic Values
            // Begin TT#237 - JSmith - after transaction gets error, subsequent transactions not processed.
            //object val;
            //try
            //{
            //    switch (aStoreCharType)
            //    {
            //        case eStoreCharType.date:
            //            // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
            //            //val = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
            //            DateTime dt = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
            //            if (dt != Include.UndefinedDate && !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(dt))
            //            {
            //                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_DateNotWithinMerchandiseCalendar); 
            //                msgDetails = msgDetails.Replace("{0}", dt.ToString("d", CultureInfo.CurrentUICulture));
            //                storeRecord.hasError = true;  //TO DO - not sure if this should be an error or not
            //                storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DateNotWithinMerchandiseCalendar, msg = msgDetails });
            //                Value = "0001-01-01";
            //            }
            //            // End TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
            //            charInfo.dataType = fieldDataTypes.DateNoTime;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
            //            break;
            //        case eStoreCharType.dollar:
            //            val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
            //            charInfo.dataType = fieldDataTypes.NumericDollar;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
            //            break;
            //        case eStoreCharType.number:
            //            val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
            //            charInfo.dataType = fieldDataTypes.NumericDouble;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
            //            break;
            //    }
            //}
            //catch
            //{
            //    string msgDetails = "Store: " + storeRecord.StoreID +
            //                " Characteristic: " + aStoreCharacteristic +
            //                " Value: " + Value;
            //    storeRecord.hasError = true;
            //    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristicValue, msg = msgDetails });

            //    return;
            //}
            //// End TT#237

            //charInfo.anyValue = Value;
            // End TT#1865-MD - JSmith - Duplicates Characteristic Values

            int charGroupRid = StoreMgmt.StoreCharGroup_Find(aStoreCharacteristic); //TT#722 - stodd - store load and mixed case causing duplicates
            bool characteristicGroupFound;
            if (charGroupRid == Include.NoRID)
            {
                characteristicGroupFound = false;
            }
            else
            {
                characteristicGroupFound = true;
                aStoreCharType = StoreMgmt.StoreCharGroup_GetType(charGroupRid);  // TT#1865-MD - JSmith - Duplicates Characteristic Values
            }

            // Begin TT#1865-MD - JSmith - Duplicates Characteristic Values
            object val;
            try
            {
                switch (aStoreCharType)
                {
                    case eStoreCharType.date:
                        // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
                        //val = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
                        DateTime dt = Convert.ToDateTime(Value, CultureInfo.CurrentUICulture);
                        if (dt != Include.UndefinedDate && !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(dt))
                        {
                            string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_DateNotWithinMerchandiseCalendar);
                            msgDetails = msgDetails.Replace("{0}", dt.ToString("d", CultureInfo.CurrentUICulture));
                            storeRecord.hasError = true;  //TO DO - not sure if this should be an error or not
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DateNotWithinMerchandiseCalendar, msg = msgDetails });
                            Value = "0001-01-01";
                        }
                        // End TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
                        charInfo.dataType = fieldDataTypes.DateNoTime;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
                        break;
                    case eStoreCharType.dollar:
                        val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
                        charInfo.dataType = fieldDataTypes.NumericDollar;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
                        break;
                    case eStoreCharType.number:
                        val = Convert.ToDecimal(Value, CultureInfo.CurrentUICulture);
                        charInfo.dataType = fieldDataTypes.NumericDouble;  // TT#1865-MD - JSmith - Duplicates Characteristic Values
                        break;
                }
            }
            catch
            {
                string msgDetails = "Store: " + storeRecord.StoreID +
                            " Characteristic: " + aStoreCharacteristic +
                            " Value: " + Value;
                storeRecord.hasError = true;
                storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristicValue, msg = msgDetails });

                return;
            }
            // End TT#237

            charInfo.anyValue = Value;
            // End TT#1865-MD - JSmith - Duplicates Characteristic Values

            if (!characteristicGroupFound)
            {
                if (processingOptions.autoAddCharacteristics)
                {
                    bool didInsertNewDynamicStoreGroup = false;
                    int filterRID = Include.NoRID;

                    List<MIDMsg> em = new List<MIDMsg>();
                    charGroupRid = StoreMgmt.StoreCharGroup_Insert(ref em, ref didInsertNewDynamicStoreGroup, ref filterRID, true, Include.GlobalUserRID, aStoreCharacteristic, aStoreCharType, hasList: false, storeId: storeRecord.StoreID);

                    // Begin TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
                    foreach (MIDMsg msg in em)
                    {
                        storeRecord.msgList.Add(msg);
                    }
                    if (charGroupRid != Include.NoRID)
                    {
                    // End TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
                        charInfo.scgRID = charGroupRid;
                        if (didInsertNewDynamicStoreGroup) //Filter and its conditions have been saved and committed to the db by now.
                        {
                            filter f = filterDataHelper.LoadExistingFilter(filterRID);
                            // Begin TT#2078-MD - JSmith - Object Reference error updating Store Group
                            //StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(f, isNewGroup: true, loadNewResults: false);  //Adds group and levels and results to the db.  Executes the filter.
                            StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(f, isNewGroup: true, loadNewResults: false, refreshSessions: false);  //Adds group and levels and results to the db.  Executes the filter.
                            // End TT#2078-MD - JSmith - Object Reference error updating Store Group
                            //((StoreTreeView)(this._EAB.StoreGroupExplorer.TreeView)).AfterSave(e.filterToSave, groupProfile);  //Adds db entry to the FOLDER table

                            //save store group to folder join
                            FolderDataLayer _dlFolder = new FolderDataLayer();

                            DataTable dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);

                            if (dtFolders == null)
                            {
                                string msgDetails = "Store: " + storeRecord.StoreID + " Global Attributes Folder not defined for characteristic: " + aStoreCharacteristic;
                                storeRecord.hasError = true;
                                storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristic, msg = msgDetails });
                            }

                            FolderProfile globalFolderProf = new FolderProfile(dtFolders.Rows[0]);
                            _dlFolder.OpenUpdateConnection();
                            try
                            {
                                _dlFolder.Folder_Item_Insert(globalFolderProf.Key, groupProfile.Key, eProfileType.StoreGroup);
                                _dlFolder.CommitData();
                            }
                            catch (Exception exc)
                            {
                                string message = exc.ToString();
                                throw;
                            }
                            finally
                            {
                                _dlFolder.CloseUpdateConnection();
                            }
                        }


                        characteristicGroupFound = true;
                    // Begin TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
                    }
                    else
                {

                    string msgDetails = "Store: " + storeRecord.StoreID + " Invalid Characteristic Name: " + aStoreCharacteristic;
                    storeRecord.hasError = true;
                    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristic, msg = msgDetails });
                }
                    // End TT#1904-MD - JSmith - Versioning_Test - Interfaced a Store Characteristic in with an existing name.  Questions below
                }
                else
                {

                    string msgDetails = "Store: " + storeRecord.StoreID + " Invalid Characteristic Name: " + aStoreCharacteristic;
                    storeRecord.hasError = true;
                    storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharateristic, msg = msgDetails });
                }
            }
            else
            {
                charInfo.scgRID = charGroupRid;
            }

            if (characteristicGroupFound)
            {
                //=========================================================================================
                // if the value is null or empty
                // the value for this characterisitic will be removed from the store--that is made NULL--
                // by the store session
                //=========================================================================================
                if (Value == null || Value == string.Empty || Value == "0001-01-01")  // Issue 4659 stodd 9.6.2007
                {
                    //isValid = true;
                }
                else
                {
                    //Determine if the current group is a list
                    StoreCharMaint storeCharMaintData = new StoreCharMaint();
                    DataTable dtCharGroup = storeCharMaintData.StoreCharGroup_Read(charGroupRid);

                    bool isList = false;
                    if (dtCharGroup.Rows[0]["SCG_LIST_IND"] != DBNull.Value)
                    {
                        if (dtCharGroup.Rows[0]["SCG_LIST_IND"].ToString() == "1")
                        {
                            isList = true;
                        }                      
                    }

                    fieldDataTypes charGroupFieldType = fieldDataTypes.FromCharIgnoreLists(Convert.ToInt32(dtCharGroup.Rows[0]["SCG_TYPE"]));


                    int scRidDuplicate = Include.NoRID;
                    bool doesCharValueAlreadyExist = StoreMgmt.DoesStoreCharValueAlreadyExist(Value, charGroupFieldType, charGroupRid, Include.NoRID, ref scRidDuplicate);
                    charInfo.scRID = scRidDuplicate;
                    if (isList)
                    {
                        if (doesCharValueAlreadyExist == false)
                        {
                            string msgDetails = "Store: " + storeRecord.StoreID +
                                " Characteristic: " + aStoreCharacteristic +
                                " Value: " + Value;
                            storeRecord.hasError = true;
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreCharListValue, msg = msgDetails });
                        }
                    }
                    else //no validation list
                    {
                        if (Value == null || Value.Length == 0)
                        {
                            string msgDetails = "Store: " + storeRecord.StoreID +
                                ".  Value not found for Characteristic Name: " + aStoreCharacteristic +
                                ".  Characteristic not processed";
                            storeRecord.hasError = true;
                            storeRecord.msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Warning, textCode = eMIDTextCode.msg_InvalidStoreCharateristic, msg = msgDetails });
                        }
                        
                    }

                }
            }

        }




        private void AddMsgsToAudit(List<MIDMsg> msgList)
        {
            try
            {
                string module = "StoreLoadProcessManager.cs";
                foreach (MIDMsg mMsg in msgList)
                {

                    if (mMsg.textCode == eMIDTextCode.Unassigned)
                    {
                        _SAB.StoreServerSession.Audit.Add_Msg(mMsg.msgLevel, mMsg.msg, module);
                    }
                    else
                    {
                        _SAB.StoreServerSession.Audit.Add_Msg(mMsg.msgLevel, mMsg.textCode, mMsg.msg, module);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Local class's exception type.
    /// </summary>
    [Serializable]
    public class XMLStoreLoadProcessException : Exception
    {
        /// <summary>
        /// Used when throwing exceptions in the XML Store Load Class
        /// </summary>
        /// <param name="message">The error message to display</param>
        public XMLStoreLoadProcessException(string message)
            : base(message)
        {
        }
        public XMLStoreLoadProcessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public XMLStoreLoadProcessException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
            : base(aInfo, aContext)
        {
        }
        override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
        {
            base.GetObjectData(aInfo, aContext);
        }
    }
}
