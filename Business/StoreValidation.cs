using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public static class StoreValidation
    {
        private const string _sourceModule = "StoreValidation.cs";
        public static SessionAddressBlock _SAB;
        public static string _DateNotWithinMerchandiseCalendar; //TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
        private static object _writeLock = new object();
        public static void SetSAB(SessionAddressBlock SAB)
        {
            lock (_writeLock)
            {
                _SAB = SAB;
                _DateNotWithinMerchandiseCalendar = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_DateNotWithinMerchandiseCalendar, false);
            }
        }

        public static eChangeType ProcessStoreData(ref EditMsgs em, StoreProfile storeProfile, DataTable dtCharToAdd, StoresStoreAction storeAction)	// TT#739-MD - STodd - delete stores
        {
            eChangeType _changeType;
            // BEGIN TT#739-MD - STodd - delete stores
            if (storeAction == StoresStoreAction.Delete)
            {
                _changeType = eChangeType.markedForDelete;
                _SAB.StoreServerSession.MarkStoreProfileForDelete(ref em, storeProfile, true);
            }
            else if (storeAction == StoresStoreAction.Recover)
            {
                _changeType = eChangeType.markedForDelete;
                _SAB.StoreServerSession.MarkStoreProfileForDelete(ref em, storeProfile, false);
            }
            else
            // END TT#739-MD - STodd - delete stores
            {
                StoreProfile checkStore = _SAB.StoreServerSession.GetStoreProfile(storeProfile.StoreId);
                if (checkStore.Key == Include.UndefinedStoreRID)
                    _changeType = eChangeType.add;
                else
                    _changeType = eChangeType.update;

                if (StoreValidation.IsStoreProfileValid(ref em, storeProfile))
                {

                    if (_changeType == eChangeType.add)
                    {
                        _SAB.StoreServerSession.AddStoreProfile(ref em, storeProfile, dtCharToAdd);
                    }
                    else if (_changeType == eChangeType.update)
                    {
                        _SAB.StoreServerSession.UpdateStoreProfile(ref em, storeProfile, dtCharToAdd);
                    }
                }
                else
                {
                    _changeType = eChangeType.none;
                }
            }
            return _changeType;
        }


        public static bool IsStoreProfileValid(ref EditMsgs em, StoreProfile sp)
        {
            bool isValid = true;
            // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
            if (sp.SellingOpenDt != Include.UndefinedDate &&
                !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(sp.SellingOpenDt))
            {
                string msgDetails = Convert.ToString(_DateNotWithinMerchandiseCalendar.Clone());
                msgDetails = msgDetails.Replace("{0}", sp.SellingOpenDt.ToString("d", CultureInfo.CurrentUICulture));
                em.AddMsg(eMIDMessageLevel.Edit, msgDetails, _sourceModule);
                sp.SellingOpenDt = Include.UndefinedDate;
            }
            if (sp.SellingCloseDt != Include.UndefinedDate &&
                !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(sp.SellingCloseDt))
            {
                string msgDetails = Convert.ToString(_DateNotWithinMerchandiseCalendar.Clone());
                msgDetails = msgDetails.Replace("{0}", sp.SellingCloseDt.ToString("d", CultureInfo.CurrentUICulture));
                em.AddMsg(eMIDMessageLevel.Edit, msgDetails, _sourceModule);
                sp.SellingCloseDt = Include.UndefinedDate;
            }
            if (sp.StockOpenDt != Include.UndefinedDate &&
                !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(sp.StockOpenDt))
            {
                string msgDetails = Convert.ToString(_DateNotWithinMerchandiseCalendar.Clone());
                msgDetails = msgDetails.Replace("{0}", sp.StockOpenDt.ToString("d", CultureInfo.CurrentUICulture));
                em.AddMsg(eMIDMessageLevel.Edit, msgDetails, _sourceModule);
                sp.StockOpenDt = Include.UndefinedDate;
            }
            if (sp.StockCloseDt != Include.UndefinedDate &&
                !_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(sp.StockCloseDt))
            {
                string msgDetails = Convert.ToString(_DateNotWithinMerchandiseCalendar.Clone());
                msgDetails = msgDetails.Replace("{0}", sp.StockCloseDt.ToString("d", CultureInfo.CurrentUICulture));
                em.AddMsg(eMIDMessageLevel.Edit, msgDetails, _sourceModule);
                sp.StockCloseDt = Include.UndefinedDate;
            }
            // End TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.

            // validate selling dates
            isValid = IsStoreOpenSellingDateValid(ref em, sp.StoreId, sp.SellingOpenDt, sp.SellingCloseDt);

            // validate stock dates         
            isValid = IsStoreOpenSellingDateValid(ref em, sp.StoreId, sp.StockOpenDt, sp.StockCloseDt);

            
            if (sp.ActiveInd != true)
            {
                isValid = IsStoreActiveIndicatorValid(ref em, sp.Key, sp.StoreId);
            }

            return isValid;
        }

        public static bool IsStoreActiveIndicatorValid(ref EditMsgs em, int storeRID, string storeId, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (storeRID != Include.NoRID) //do not do this for new stores
            {
                int allocationCount = _SAB.StoreServerSession.GetAllStoreAllocationCounts(storeRID); //TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit
                int intransitCount = _SAB.StoreServerSession.GetAllStoreIntransitCounts(storeRID);
                if (allocationCount > 0 || intransitCount > 0)
                {
                    string msgDetails = "Store: " + storeId;
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_DoNotAllowInactive);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DoNotAllowInactive, msgDetails, _sourceModule);
                    isValid = false;
                }
            }

            return isValid;
        }
        public static bool IsStoreOpenSellingDateValid(ref EditMsgs em, string StoreId, DateTime SellingOpenDt, DateTime SellingCloseDt, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (SellingOpenDt != Include.UndefinedDate && SellingCloseDt != Include.UndefinedDate)
            {
                if (SellingOpenDt > SellingCloseDt)
                {
                    string msgDetails = "Store: " + StoreId +
                        " Selling Open Date: " + SellingOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                        " Selling Close Date: " + SellingCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreSellingDateError);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreSellingDateError, msgDetails, _sourceModule);
                    isValid = false;
                }
            }
            return isValid;
        }
        public static bool IsStoreClosingSellingDateValid(ref EditMsgs em, string StoreId, DateTime SellingOpenDt, DateTime SellingCloseDt, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (SellingOpenDt != Include.UndefinedDate && SellingCloseDt != Include.UndefinedDate)
            {
                if (SellingOpenDt > SellingCloseDt)
                {
                    string msgDetails = "Store: " + StoreId +
                        " Selling Open Date: " + SellingOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                        " Selling Close Date: " + SellingCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileInvalidSellingCloseDate);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileInvalidSellingCloseDate, msgDetails, _sourceModule);
                    isValid = false;
                }
            }
            return isValid;
        }
        public static bool IsStoreOpenStockDateValid(ref EditMsgs em, string StoreId, DateTime StockOpenDt, DateTime StockCloseDt, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (StockOpenDt != Include.UndefinedDate && StockCloseDt != Include.UndefinedDate)
            {
                if (StockOpenDt > StockCloseDt)
                {
                    string msgDetails = "Store: " + StoreId +
                        " Stock Open Date: " + StockOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                        " Stock Close Date: " + StockCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreStockDateError);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreStockDateError, msgDetails, _sourceModule);
                    isValid = false;
                }
            }
            return isValid;
        }
        public static bool IsStoreClosingStockDateValid(ref EditMsgs em, string StoreId, DateTime StockOpenDt, DateTime StockCloseDt, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (StockOpenDt != Include.UndefinedDate && StockCloseDt != Include.UndefinedDate)
            {
                if (StockOpenDt > StockCloseDt)
                {
                    string msgDetails = "Store: " + StoreId +
                        " Stock Open Date: " + StockOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                        " Stock Close Date: " + StockCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileInvalidStockCloseDate);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileInvalidStockCloseDate, msgDetails, _sourceModule);
                    isValid = false;
                }
            }
            return isValid;
        }


        public static bool IsDateValidWithinMerchandiseCalendar(ref EditMsgs em, string StoreId, DateTime dtValue, bool includeSoftTextMessage = false)
        {
            bool isValid = true;

            if (dtValue != Include.UndefinedDate)
            {
                if (!_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(dtValue))
                {
                    string msgDetails = "Store: " + StoreId +
                        " Date: " + dtValue.ToString("d", CultureInfo.CurrentUICulture);
                    if (includeSoftTextMessage)
                    {
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileDateNotInMerchandiseCalendar);
                    }
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileDateNotInMerchandiseCalendar, msgDetails, _sourceModule);
                    isValid = false;
                }
            }
            return isValid;
        }

        public static int ReserveStoreRID = -1;
        public static bool hasActiveFieldChanged = false;
        public static bool IsStoreFieldValid(validationKinds validationKind, int objectType, int fieldIndex, int storeRID, object proposedValue, ref EditMsgs em, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;

            if (objectType == storeObjectTypes.StoreFields) //Store Fields
            {
                if (fieldIndex == storeFieldTypes.StoreID)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    if (storeRID == ReserveStoreRID && (string)proposedValue != "Reserve") //SAB.StoreServerSession.GlobalOptions.ReserveStoreRID;
                    {
                        isValid = false;
                        em.ErrorFound = true;
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileStoreIdForReserve, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileStoreIdForReserve), _sourceModule);
                    }
                    else
                    {
                        if (proposedValue == DBNull.Value || (string)proposedValue == string.Empty)
                        {
                            isValid = false;
                            em.ErrorFound = true;
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileStoreIdRequired, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileStoreIdRequired), _sourceModule);
                        }
                    }
                    //}
                    if (validationKind == validationKinds.UponSaving) //Need to ensure Store ID is unique - delayed until actual saving of all changes
                    {
                        StoreMaint storeMaintData = new StoreMaint();
                        if (storeMaintData.DoesStoreIdAlreadyExist((string)proposedValue, storeRID))
                        {
                            isValid = false;
                            em.ErrorFound = true;
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileIdNotUnique, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileIdNotUnique), _sourceModule);
                        }
                    }
                }
                else if (fieldIndex == storeFieldTypes.StoreName)
                {

                }
                else if (fieldIndex == storeFieldTypes.ActiveInd)
                {
                    if (validationKind == validationKinds.BeforeExitEditMode || validationKind == validationKinds.UponSaving)
                    {

                        bool proposedActiveInd;
                        bool.TryParse(proposedValue.ToString(), out proposedActiveInd);

                        if (proposedActiveInd == false)
                        {
                            string storeID = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                            isValid = IsStoreActiveIndicatorValid(ref em, storeRID, storeID, includeSoftTextMessage: true);
                            if (isValid)
                            {
                                hasActiveFieldChanged = true; //when editing fields, mark active indicator as changed
                            }
                        }
                        else
                        {
                            hasActiveFieldChanged = true; //when editing fields, mark active indicator as changed
                        }

                    }


                }
                else if (fieldIndex == storeFieldTypes.StoreDeleteInd)
                {
                }
                else if (fieldIndex == storeFieldTypes.SimilarStoreModel)
                {
                }
                else if (fieldIndex == storeFieldTypes.StoreDescription)
                {
                }
                else if (fieldIndex == storeFieldTypes.City)
                {
                }
                else if (fieldIndex == storeFieldTypes.State)
                {
                }
                else if (fieldIndex == storeFieldTypes.SellingSqFt)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    int proposedInt;
                    int.TryParse(proposedValue.ToString(), out proposedInt);

                    if (proposedInt < 0)
                    {
                        isValid = false;
                        em.ErrorFound = true;
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileSellingSqFtPositive, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileSellingSqFtPositive), _sourceModule);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.SellingOpenDate)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    DateTime sellingOpenDt = Include.ConvertObjectToDateTime(proposedValue);
                    DateTime sellingCloseDt = Include.ConvertObjectToDateTime(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.SellingCloseDate));
                    string storeID = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsStoreOpenSellingDateValid(ref em, storeID, sellingOpenDt, sellingCloseDt, includeSoftTextMessage: true);
                    if (isValid)
                    {
                        isValid = IsDateValidWithinMerchandiseCalendar(ref em, storeID, sellingOpenDt, includeSoftTextMessage: true);
                    }
                    if (isValid)
                    {
                        //update the store status
                        WeekProfile currentWeek = _SAB.ClientServerSession.Calendar.CurrentWeek;
                        eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
                        fieldValueSetCurrent(storeRID, storeObjectTypes.StoreFields, storeFieldTypes.StoreStatus, (int)storeStatus);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.SellingCloseDate)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    DateTime sellingOpenDt = Include.ConvertObjectToDateTime(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.SellingOpenDate));
                    DateTime sellingCloseDt = Include.ConvertObjectToDateTime(proposedValue);
                    string storeID = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsStoreClosingSellingDateValid(ref em, storeID, sellingOpenDt, sellingCloseDt, includeSoftTextMessage: true);
                    if (isValid)
                    {
                        isValid = IsDateValidWithinMerchandiseCalendar(ref em, storeID, sellingCloseDt, includeSoftTextMessage: true);
                    }
                    if (isValid)
                    {
                        //update the store status
                        WeekProfile currentWeek = _SAB.ClientServerSession.Calendar.CurrentWeek;
                        eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
                        fieldValueSetCurrent(storeRID, storeObjectTypes.StoreFields, storeFieldTypes.StoreStatus, (int)storeStatus);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.StockOpenDate)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    DateTime stockOpenDt = Include.ConvertObjectToDateTime(proposedValue);
                    DateTime stockCloseDt = Include.ConvertObjectToDateTime(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StockCloseDate));
                    string storeID = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsStoreOpenStockDateValid(ref em, storeID, stockOpenDt, stockCloseDt, includeSoftTextMessage: true);
                    if (isValid)
                    {
                        isValid = IsDateValidWithinMerchandiseCalendar(ref em, storeID, stockOpenDt, includeSoftTextMessage: true);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.StockCloseDate)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    DateTime stockOpenDt = Include.ConvertObjectToDateTime(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StockOpenDate));
                    DateTime stockCloseDt = Include.ConvertObjectToDateTime(proposedValue);
                    string storeID = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsStoreClosingStockDateValid(ref em, storeID, stockOpenDt, stockCloseDt, includeSoftTextMessage: true);
                    if (isValid)
                    {
                        isValid = IsDateValidWithinMerchandiseCalendar(ref em, storeID, stockCloseDt, includeSoftTextMessage: true);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.ShipOnMonday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnTuesday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnWednesday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnThursday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnFriday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnSaturday)
                {
                }
                else if (fieldIndex == storeFieldTypes.ShipOnSunday)
                {
                }
                else if (fieldIndex == storeFieldTypes.LeadTime)
                {
                    //if (validationKind == validationKinds.BeforeExitEditMode)
                    //{
                    int proposedInt;
                    int.TryParse(proposedValue.ToString(), out proposedInt);

                    if (proposedInt < 0)
                    {
                        isValid = false;
                        em.ErrorFound = true;
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileLeadTimePositive, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileLeadTimePositive), _sourceModule);
                    }
                    //}
                }
                else if (fieldIndex == storeFieldTypes.ImoID)
                {
                }
            }
            else if (objectType == storeObjectTypes.StoreCharacteristics) //Store Characteristics
            {
                int scgRID = fieldIndex;

            }
            return isValid;
        }

        public static bool IsStoreObjectValid(int storeRID, ref DataSet dsFields, ref EditMsgs em, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;
            hasActiveFieldChanged = false;

            if (storeRID == Include.NoRID)
            {
                foreach (DataRow cRow in dsFields.Tables[0].Rows)  //here each row is a single field, so we know each of these fields changed
                {
                    int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                    int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                    object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                    object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                    isValid = IsStoreFieldValid(validationKinds.UponSaving, objectType, fieldIndex, storeRID, curVal, ref em, fieldValueGetCurrent, fieldValueSetCurrent);
                    if (isValid == false)
                    {
                        break;
                    }
                }
            }
            else
            {
                DataTable dtChanges = dsFields.Tables[0].GetChanges(DataRowState.Modified);  // Process Rows that were CHANGED
                if (dtChanges != null)
                {
                    foreach (DataRow cRow in dtChanges.Rows)  //here each row is a single field, so we know each of these fields changed
                    {
                        int objectType = Convert.ToInt32(cRow["OBJECT_TYPE"]);
                        int fieldIndex = Convert.ToInt32(cRow["FIELD_INDEX"]);
                        object curVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Current];
                        object origVal = cRow["FIELD_VALUE", System.Data.DataRowVersion.Original];

                        isValid = IsStoreFieldValid(validationKinds.UponSaving, objectType, fieldIndex, storeRID, curVal, ref em, fieldValueGetCurrent, fieldValueSetCurrent);
                        if (isValid == false)
                        {
                            break;
                        }
                    }
                }
            }

            CheckIfActiveFieldChanged(ref em, ref isValid);

            return isValid;
        }
        public static bool IsStoreObjectValidForFieldEditing(ref DataSet dsFields, ref EditMsgs em, List<fieldColumnMap> columnMapList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;
            hasActiveFieldChanged = false;

            DataTable dtChanges = dsFields.Tables[0].GetChanges(DataRowState.Modified); // Process Rows that were CHANGED
            if (dtChanges != null)
            {
                foreach (DataColumn dc in dtChanges.Columns)
                {
                    fieldColumnMap map = columnMapList.Find(x => x.columnIndex == dc.Ordinal);
                    if (map != null)
                    {
                        foreach (DataRow cRow in dtChanges.Rows)
                        {
                            object curVal = cRow[map.columnIndex, System.Data.DataRowVersion.Current];
                            object origVal = cRow[map.columnIndex, System.Data.DataRowVersion.Original];

                            if (curVal.ToString() != origVal.ToString())
                            {
                                int objectRID = (int)cRow["OBJECT_RID", System.Data.DataRowVersion.Original];
                                isValid = IsStoreFieldValid(validationKinds.UponSaving, map.objectType, map.fieldIndex, objectRID, curVal, ref em, fieldValueGetCurrent, fieldValueSetCurrent);
                                if (isValid == false)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            CheckIfActiveFieldChanged(ref em, ref isValid);

            return isValid;
        }
        public static void CheckIfActiveFieldChanged(ref EditMsgs em, ref bool isValid)
        {
            if (hasActiveFieldChanged)
            {
                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreActiveChanged, MIDText.GetTextOnly(eMIDTextCode.msg_StoreActiveChanged), _sourceModule);
                em.DisplayPrompt = true;
                isValid = false;
            }
        }

        public delegate object GetValueFromStoreProfile(StoreProfile prof);

        public sealed class storeObjectTypes
        {
            public static List<storeObjectTypes> valueTypeList = new List<storeObjectTypes>();
            public static readonly storeObjectTypes StoreFields = new storeObjectTypes(1);
            public static readonly storeObjectTypes StoreCharacteristics = new storeObjectTypes(2);
            private storeObjectTypes(int index)
            {
                this.Index = index;
                valueTypeList.Add(this);
            }
            public int Index { get; private set; }
            public static implicit operator int(storeObjectTypes op) { return op.Index; }
            public static storeObjectTypes FromIndex(int index)
            {
                return valueTypeList.Find(x => x.Index == index);
            }
        }

        public sealed class storeFieldTypes : fieldTypeBase
        {
            public static List<storeFieldTypes> storeFieldTypeList = new List<storeFieldTypes>();
            public static readonly storeFieldTypes StoreID = new storeFieldTypes(0, 900000, "ST_ID", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreId; });
            public static readonly storeFieldTypes StoreName = new storeFieldTypes(1, 900001, "STORE_NAME", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreName; });
            public static readonly storeFieldTypes ActiveInd = new storeFieldTypes(2, 900002, "ACTIVE_IND", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ActiveInd; }, defaultValue: true);
            public static readonly storeFieldTypes StoreDeleteInd = new storeFieldTypes(3, 900868, "STORE_DELETE_IND", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.DeleteStore; }, defaultValue: true, allowEdit: false, onDB: true);
            public static readonly storeFieldTypes SimilarStoreModel = new storeFieldTypes(4, 900352, "SIMILAR_STORE_MODEL", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.SimilarStoreModel; }, defaultValue: true);
            public static readonly storeFieldTypes StoreDescription = new storeFieldTypes(5, 900024, "STORE_DESC", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreDescription; });
            public static readonly storeFieldTypes City = new storeFieldTypes(6, 900003, "CITY", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.City; });
            public static readonly storeFieldTypes State = new storeFieldTypes(7, 900004, "STATE", fieldDataTypes.List, 50, delegate(StoreProfile prof) { return prof.State; });
            public static readonly storeFieldTypes SellingSqFt = new storeFieldTypes(8, 900005, "SELLING_SQ_FT", fieldDataTypes.NumericInteger, -1, delegate(StoreProfile prof) { return prof.SellingSqFt; }, defaultValue: 0);
            public static readonly storeFieldTypes SellingOpenDate = new storeFieldTypes(9, 900006, "SELLING_OPEN_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.SellingOpenDt; }, defaultValue: DBNull.Value);
            public static readonly storeFieldTypes SellingCloseDate = new storeFieldTypes(10, 900007, "SELLING_CLOSE_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.SellingCloseDt; }, defaultValue: DBNull.Value);
            public static readonly storeFieldTypes StockOpenDate = new storeFieldTypes(11, 900008, "STOCK_OPEN_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.StockOpenDt; }, defaultValue: DBNull.Value);
            public static readonly storeFieldTypes StockCloseDate = new storeFieldTypes(12, 900009, "STOCK_CLOSE_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.StockCloseDt; }, defaultValue: DBNull.Value);
            public static readonly storeFieldTypes ShipOnMonday = new storeFieldTypes(13, 900011, "SHIP_ON_MONDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnMonday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnTuesday = new storeFieldTypes(14, 900012, "SHIP_ON_TUESDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnTuesday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnWednesday = new storeFieldTypes(15, 900013, "SHIP_ON_WEDNESDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnWednesday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnThursday = new storeFieldTypes(16, 900014, "SHIP_ON_THURSDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnThursday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnFriday = new storeFieldTypes(17, 900015, "SHIP_ON_FRIDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnFriday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnSaturday = new storeFieldTypes(18, 900016, "SHIP_ON_SATURDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnSaturday; }, defaultValue: true);
            public static readonly storeFieldTypes ShipOnSunday = new storeFieldTypes(19, 900017, "SHIP_ON_SUNDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnSunday; }, defaultValue: true);
            public static readonly storeFieldTypes LeadTime = new storeFieldTypes(20, 900010, "LEAD_TIME", fieldDataTypes.NumericInteger, -1, delegate(StoreProfile prof) { return prof.LeadTime; }, defaultValue: 0);
            public static readonly storeFieldTypes ImoID = new storeFieldTypes(21, 900805, "IMO_ID", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { if (prof.IMO_ID == null) return string.Empty; else return prof.IMO_ID; });
            public static readonly storeFieldTypes StoreStatus = new storeFieldTypes(22, 900022, "STATUS", fieldDataTypes.List, -1, delegate(StoreProfile prof) { return prof.Status; }, allowEdit: false, onDB: false);


            private storeFieldTypes(int fieldIndex, int textCode, string dbFieldName, fieldDataTypes dataType, int maxLength, GetValueFromStoreProfile getValueFromProfile, object defaultValue = null, bool allowEdit = true, bool onDB = true)
            {
                string n = MIDRetail.Data.MIDText.GetTextFromCode(textCode);
                base.Name = n;
                base.fieldIndex = fieldIndex;
                base.dbFieldName = dbFieldName;
                base.allowEdit = allowEdit;
                base.maxLength = maxLength;
                base.dataType = dataType;
                if (defaultValue == null)
                {
                    this.defaultValue = string.Empty;
                }
                else
                {
                    this.defaultValue = defaultValue;
                }
                this.getValueFromProfile = getValueFromProfile;
                storeFieldTypeList.Add(this);
            }

            public object defaultValue;
            private GetValueFromStoreProfile getValueFromProfile;

            public static implicit operator int(storeFieldTypes op) { return op.fieldIndex; }


            public object GetStoreValue(StoreProfile prof)
            {
                return getValueFromProfile(prof);
            }



            public static storeFieldTypes FromIndex(int fieldIndex)
            {
                storeFieldTypes result = storeFieldTypeList.Find(
                   delegate(storeFieldTypes ft)
                   {
                       return ft.fieldIndex == fieldIndex;
                   }
                   );
                if (result != null)
                {
                    return result;
                }
                else
                {
                    //storeField type was not found in the list
                    return null;
                }
            }
            public static storeFieldTypes FromString(string storeFieldTypeName)
            {
                storeFieldTypes result = storeFieldTypeList.Find(
                  delegate(storeFieldTypes ft)
                  {
                      return ft.Name == storeFieldTypeName;
                  }
                  );
                if (result != null)
                {
                    return result;
                }
                else
                {
                    //storeField type was not found in the list
                    return null;
                }
            }


            public static string GetNameFromIndex(int fieldIndex)
            {
                storeFieldTypes field = storeFieldTypes.FromIndex(fieldIndex);
                return field.Name;
            }

        }

        private static DataSet dsStoreCharacteristicValueLists;
        public static DataTable GetListValuesForStoreField(int objectType, int fieldIndex, ref string dataField, ref string displayField)
        {
            DataTable dtListValues = FieldTypeUtility.GetDataTableForListDropDowns();
            dataField = "LIST_VALUE_INDEX";
            displayField = "LIST_VALUE";
            if (objectType == storeObjectTypes.StoreFields)
            {
                storeFieldTypes storeField = storeFieldTypes.FromIndex(fieldIndex);
                if (storeField == storeFieldTypes.State)
                {
                    GlobalOptions globalOptionData = new GlobalOptions();
                    string[] states = globalOptionData.GetStateAbbreviationsArray();

                    //Add a blank state to start the list
                    DataRow drBlank = dtListValues.NewRow();
                    drBlank[dataField] = -1;
                    drBlank[displayField] = string.Empty;
                    dtListValues.Rows.Add(drBlank);

                    for (int s = 0; s < states.Length; s++)
                    {
                        DataRow dr = dtListValues.NewRow();
                        dr[dataField] = s;
                        dr[displayField] = states[s];
                        dtListValues.Rows.Add(dr);
                    }
                }
                else if (storeField == storeFieldTypes.StoreStatus)
                {
                    DataTable dtStoreStatus = MIDText.GetLabels((int)eStoreStatus.New, (int)eStoreStatus.Preopen);
                    foreach (DataRow drStoreStatus in dtStoreStatus.Rows)
                    {
                        DataRow dr = dtListValues.NewRow();
                        dr[dataField] = Convert.ToInt32(drStoreStatus["TEXT_CODE"]);
                        dr[displayField] = drStoreStatus["TEXT_VALUE"].ToString();
                        dtListValues.Rows.Add(dr);
                    }
                }
            }
            else //store characteristic lists
            {
                int scgRID = fieldIndex;
                DataRow[] drValueListForGroup = dsStoreCharacteristicValueLists.Tables[0].Select("SCG_RID=" + scgRID.ToString());
                int miniIndex = 0;
                foreach (DataRow drCharListValue in drValueListForGroup)
                {
                    DataRow dr = dtListValues.NewRow();
                    dr[dataField] = miniIndex;
                    dr[displayField] = drCharListValue["CHAR_LIST_VALUE"].ToString();
                    dtListValues.Rows.Add(dr);
                    miniIndex++;
                }
            }

            return dtListValues;
        }

        /// <summary>
        /// Reads in all store characteristic values where the type=list
        /// Used in a delegate call to populate drop down lists for a characteristc
        /// Should be called prior to bind fields to a grid
        /// </summary>
        public static void SetCharListValuesLists()
        {
            StoreMaint storeMaintData = new StoreMaint();
            dsStoreCharacteristicValueLists = storeMaintData.ReadCharacteristicListValuesForMaint();
        }

        /// <summary>
        /// Reads the current field values and characteristic values for a single store.
        /// Places those values into one common datatable
        /// </summary>
        /// <param name="SAB"></param>
        /// <param name="currentStoreRID"></param>
        /// <returns></returns>
        public static DataTable ReadFieldAndCharacteristicValuesForStore(SessionAddressBlock SAB, int currentStoreRID)
        {
            DataTable dtStoreFieldsAndChars = FieldTypeUtility.GetDataTableForFields();

            StoreMaint storeMaintData = new StoreMaint();
            DataSet dsValues = storeMaintData.ReadStoresFieldsForMaint(currentStoreRID);

            //Convert to a store profile so fields are populated in a consistent way (this populates store status, also sets minimum dates (which are then not displayed))
            StoreProfile prof = SAB.StoreServerSession.ConvertToStoreProfile(dsValues.Tables[0].Rows[0], false);

            foreach (storeFieldTypes fieldType in storeFieldTypes.storeFieldTypeList)
            {
                DataRow dr = dtStoreFieldsAndChars.NewRow();
                dr["OBJECT_TYPE"] = storeObjectTypes.StoreFields.Index;
                dr["FIELD_INDEX"] = fieldType.fieldIndex;
                dr["FIELD_TYPE"] = fieldType.dataType.Index;
                dr["ALLOW_EDIT"] = fieldType.allowEdit;
                dr["MAX_LENGTH"] = fieldType.maxLength;
                dr["FIELD_NAME"] = fieldType.Name;
                string val = fieldType.GetStoreValue(prof).ToString();
                dr["FIELD_VALUE"] = val;
                dtStoreFieldsAndChars.Rows.Add(dr);
            }


            foreach (DataRow drChar in dsValues.Tables[1].Rows)
            {
                DataRow dr = dtStoreFieldsAndChars.NewRow();
                dr["OBJECT_TYPE"] = storeObjectTypes.StoreCharacteristics.Index;
                dr["FIELD_INDEX"] = Convert.ToInt32(drChar["SCG_RID"]); ;
                fieldDataTypes dataType = fieldDataTypes.FromChar(Convert.ToInt32(drChar["SCG_TYPE"]), drChar["SCG_LIST_IND"].ToString());
                dr["FIELD_TYPE"] = dataType.Index;
                dr["ALLOW_EDIT"] = true;
                dr["MAX_LENGTH"] = 50;
                dr["FIELD_NAME"] = drChar["SCG_ID"];
                dr["FIELD_VALUE"] = drChar["CHAR_VALUE"];
                dtStoreFieldsAndChars.Rows.Add(dr);
            }

            return dtStoreFieldsAndChars;
        }

        /// <summary>
        /// Gets blank field values and characteristic values for a new store.
        /// Places those values into one common datatable
        /// </summary>
        /// <param name="SAB"></param>
        /// <returns></returns>
        public static DataTable ReadFieldAndCharacteristicValuesForNewStore(SessionAddressBlock SAB)
        {
            DataTable dtStoreFieldsAndChars = FieldTypeUtility.GetDataTableForFields();

            StoreMaint storeMaintData = new StoreMaint();
            DataSet dsBlankCharValues = storeMaintData.ReadStoresFieldsForMaintNewStore();


            foreach (storeFieldTypes fieldType in storeFieldTypes.storeFieldTypeList)
            {
                DataRow dr = dtStoreFieldsAndChars.NewRow();
                dr["OBJECT_TYPE"] = storeObjectTypes.StoreFields.Index;
                dr["FIELD_INDEX"] = fieldType.fieldIndex;
                dr["FIELD_TYPE"] = fieldType.dataType.Index;
                dr["ALLOW_EDIT"] = fieldType.allowEdit;
                dr["MAX_LENGTH"] = fieldType.maxLength;
                dr["FIELD_NAME"] = fieldType.Name;
                dr["FIELD_VALUE"] = fieldType.defaultValue;
                dtStoreFieldsAndChars.Rows.Add(dr);
            }


            foreach (DataRow drChar in dsBlankCharValues.Tables[0].Rows)
            {
                DataRow dr = dtStoreFieldsAndChars.NewRow();
                dr["OBJECT_TYPE"] = storeObjectTypes.StoreCharacteristics.Index;
                dr["FIELD_INDEX"] = Convert.ToInt32(drChar["SCG_RID"]); ;
                fieldDataTypes dataType = fieldDataTypes.FromChar(Convert.ToInt32(drChar["SCG_TYPE"]), drChar["SCG_LIST_IND"].ToString());
                dr["FIELD_TYPE"] = dataType.Index;
                dr["ALLOW_EDIT"] = true;
                dr["MAX_LENGTH"] = 50;
                dr["FIELD_NAME"] = drChar["SCG_ID"];
                dr["FIELD_VALUE"] = DBNull.Value;
                dtStoreFieldsAndChars.Rows.Add(dr);
            }

            return dtStoreFieldsAndChars;
        }



        public static DataTable ReadFieldAndCharacteristicValuesForSelectedFields(SessionAddressBlock SAB, List<DataRow> selectedFieldsAndChars, ref List<fieldColumnMap> columnMapList, ref DataTable dtAllStoreFields)
        {
            List<int> selectedCharGroups = new List<int>();
            foreach (DataRow drField in selectedFieldsAndChars)
            {
                int objectType = (int)drField["OBJECT_TYPE"];
                if (objectType == storeObjectTypes.StoreCharacteristics)
                {
                    int scgRID = (int)drField["FIELD_INDEX"];
                    selectedCharGroups.Add(scgRID);
                }
            }


            StoreMaint storeMaintData = new StoreMaint();
            DataSet dsSelectedFieldValues = storeMaintData.ReadStoreFieldsForMaintByCol(selectedCharGroups);
            dtAllStoreFields = dsSelectedFieldValues.Tables[0].Copy();

            //Set up dynamic columns for the selected fields and characteristics
            columnMapList = new List<fieldColumnMap>();
            DataTable dtStoreFieldsAndChars = FieldTypeUtility.GetDataTableForFieldsByCol();
            int columnIndex = 2; //zero based, 2 columns are already defined so we start with 2
            foreach (DataRow drSelectedField in selectedFieldsAndChars)
            {
                int objectType = (int)drSelectedField["OBJECT_TYPE"];
                if (objectType == storeObjectTypes.StoreFields) //Store Fields
                {
                    int fieldIndex = (int)drSelectedField["FIELD_INDEX"];
                    storeFieldTypes storeField = storeFieldTypes.FromIndex(fieldIndex);
                    if (storeField.allowEdit == true) //Store status cannot be edited
                    {
                        dtStoreFieldsAndChars.Columns.Add(storeField.Name, fieldDataTypes.GetTypeForField(storeField.dataType));

                        //make a column map entry
                        fieldColumnMap map = new fieldColumnMap(objectType, fieldIndex, columnIndex);
                        map.fieldDataType = storeField.dataType;
                        columnMapList.Add(map);
                        columnIndex++;
                    }
                }
                else if (objectType == storeObjectTypes.StoreCharacteristics) //Store Characteristics
                {
                    int scgRID = (int)drSelectedField["FIELD_INDEX"];
                    DataRow[] drCharGroup = dsSelectedFieldValues.Tables[2].Select("SCG_RID=" + scgRID.ToString());
                    if (drCharGroup.Length > 0)
                    {
                        fieldDataTypes fieldDataType = fieldDataTypes.FromChar(Convert.ToInt32(drCharGroup[0]["SCG_TYPE"]), drCharGroup[0]["SCG_LIST_IND"].ToString());
                        string charGroupName = (string)drCharGroup[0]["SCG_ID"];
                        dtStoreFieldsAndChars.Columns.Add(charGroupName, fieldDataTypes.GetTypeForField(fieldDataType));
                        //make a column map entry
                        fieldColumnMap map = new fieldColumnMap(objectType, scgRID, columnIndex);
                        map.fieldDataType = fieldDataType;
                        columnMapList.Add(map);
                        columnIndex++;
                    }
                }
            }
            //Always add Store Status field so it can be updated automatically when open or close dates change  (this keeps all changed data in one place)
            fieldColumnMap storeStatusMap = columnMapList.Find(x => x.fieldIndex == storeFieldTypes.StoreStatus.fieldIndex);
            if (storeStatusMap == null)
            {
                dtStoreFieldsAndChars.Columns.Add(storeFieldTypes.StoreStatus.Name, fieldDataTypes.GetTypeForField(storeFieldTypes.StoreStatus.dataType));

                //make a column map entry
                fieldColumnMap map = new fieldColumnMap(storeObjectTypes.StoreFields.Index, storeFieldTypes.StoreStatus.fieldIndex, columnIndex);
                map.fieldDataType = storeFieldTypes.StoreStatus.dataType;
                map.isVisible = false;
                map.columnName = storeFieldTypes.StoreStatus.Name;
                columnMapList.Add(map);
                columnIndex++;
            }

            //Set the object name caption to store
            dtStoreFieldsAndChars.Columns[1].Caption = "Store";

            //Now that the columns are defined, add the rows by looping through stores (once) and looking up characteristic values along the way
            foreach (DataRow drStoreValue in dsSelectedFieldValues.Tables[0].Rows)
            {
                DataRow drValue = dtStoreFieldsAndChars.NewRow();
                int storeRID = (int)drStoreValue["ST_RID"];
                drValue["OBJECT_RID"] = storeRID;
                drValue["OBJECT_NAME"] = (string)drStoreValue["STORE_TEXT"];
                foreach (fieldColumnMap map in columnMapList)
                {
                    if (map.objectType == storeObjectTypes.StoreFields)
                    {
                        storeFieldTypes storeField = storeFieldTypes.FromIndex(map.fieldIndex);
                        if (storeField.allowEdit == true) //Store status cannot be edited
                        {
                            fieldDataTypes.AssignValueToDataRow(ref drValue, storeField.Name, storeField.dataType.Index, drStoreValue[storeField.dbFieldName]);
                        }
                    }
                    else if (map.objectType == storeObjectTypes.StoreCharacteristics)
                    {
                        int scgRID = map.fieldIndex;
                        DataRow[] drCharValue = dsSelectedFieldValues.Tables[1].Select("ST_RID=" + storeRID.ToString() + " AND SCG_RID=" + scgRID.ToString());
                        if (drCharValue.Length > 0)
                        {
                            fieldDataTypes.AssignValueToDataRow(ref drValue, dtStoreFieldsAndChars.Columns[map.columnIndex].ColumnName, map.fieldDataType.Index, drCharValue[0]["CHAR_VALUE"]);
                        }
                    }
                }
                dtStoreFieldsAndChars.Rows.Add(drValue);
            }


            return dtStoreFieldsAndChars;
        }



    }
}
