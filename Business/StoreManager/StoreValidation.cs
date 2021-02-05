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

    public delegate object GetValueFromStoreProfile(StoreProfile prof);
    public sealed class storeFieldTypes : fieldTypeBase
    {
        public static List<storeFieldTypes> storeFieldTypeList = new List<storeFieldTypes>();
		// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        //public static readonly storeFieldTypes StoreID = new storeFieldTypes(0, 900000, "ST_ID", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreId; });
        //public static readonly storeFieldTypes StoreName = new storeFieldTypes(1, 900001, "STORE_NAME", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreName; });
        public static readonly storeFieldTypes StoreID = new storeFieldTypes(0, 900000, "ST_ID", fieldDataTypes.Text, GetDatabaseColumnLength("STORES", "ST_ID"), delegate(StoreProfile prof) { return prof.StoreId; });
        public static readonly storeFieldTypes StoreName = new storeFieldTypes(1, 900001, "STORE_NAME", fieldDataTypes.Text, GetDatabaseColumnLength("STORES", "STORE_NAME"), delegate(StoreProfile prof) { return prof.StoreName; });
		// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        public static readonly storeFieldTypes ActiveInd = new storeFieldTypes(2, 900002, "ACTIVE_IND", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ActiveInd; }, defaultValue: true);
        public static readonly storeFieldTypes StoreDeleteInd = new storeFieldTypes(3, 900868, "STORE_DELETE_IND", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.DeleteStore; }, defaultValue: false, allowEdit: false, onDB: true);
        public static readonly storeFieldTypes SimilarStoreModel = new storeFieldTypes(4, 900352, "SIMILAR_STORE_MODEL", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.SimilarStoreModel; }, defaultValue: false);
		// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        //public static readonly storeFieldTypes StoreDescription = new storeFieldTypes(5, 900024, "STORE_DESC", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.StoreDescription; });
        //public static readonly storeFieldTypes City = new storeFieldTypes(6, 900003, "CITY", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { return prof.City; });
        //public static readonly storeFieldTypes State = new storeFieldTypes(7, 900004, "STATE", fieldDataTypes.List, 50, delegate(StoreProfile prof) { return GetStateValueFromAbbrev(prof.State); });
        public static readonly storeFieldTypes StoreDescription = new storeFieldTypes(5, 900024, "STORE_DESC", fieldDataTypes.Text, GetDatabaseColumnLength("STORES", "STORE_DESC"), delegate(StoreProfile prof) { return prof.StoreDescription; });
        public static readonly storeFieldTypes City = new storeFieldTypes(6, 900003, "CITY", fieldDataTypes.Text, GetDatabaseColumnLength("STORES", "CITY"), delegate(StoreProfile prof) { return prof.City; });
        public static readonly storeFieldTypes State = new storeFieldTypes(7, 900004, "STATE", fieldDataTypes.List, GetDatabaseColumnLength("STORES", "STATE"), delegate(StoreProfile prof) { return GetStateValueFromAbbrev(prof.State); });
		// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        public static readonly storeFieldTypes SellingSqFt = new storeFieldTypes(8, 900005, "SELLING_SQ_FT", fieldDataTypes.NumericInteger, -1, delegate(StoreProfile prof) { return prof.SellingSqFt; }, defaultValue: 0);
        public static readonly storeFieldTypes SellingOpenDate = new storeFieldTypes(9, 900006, "SELLING_OPEN_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.SellingOpenDt; }, defaultValue: DBNull.Value);
        public static readonly storeFieldTypes SellingCloseDate = new storeFieldTypes(10, 900007, "SELLING_CLOSE_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.SellingCloseDt; }, defaultValue: DBNull.Value);
        public static readonly storeFieldTypes StockOpenDate = new storeFieldTypes(11, 900008, "STOCK_OPEN_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.StockOpenDt; }, defaultValue: DBNull.Value);
        public static readonly storeFieldTypes StockCloseDate = new storeFieldTypes(12, 900009, "STOCK_CLOSE_DATE", fieldDataTypes.DateNoTime, -1, delegate(StoreProfile prof) { return prof.StockCloseDt; }, defaultValue: DBNull.Value);
        public static readonly storeFieldTypes ShipOnMonday = new storeFieldTypes(13, 900011, "SHIP_ON_MONDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnMonday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnTuesday = new storeFieldTypes(14, 900012, "SHIP_ON_TUESDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnTuesday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnWednesday = new storeFieldTypes(15, 900013, "SHIP_ON_WEDNESDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnWednesday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnThursday = new storeFieldTypes(16, 900014, "SHIP_ON_THURSDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnThursday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnFriday = new storeFieldTypes(17, 900015, "SHIP_ON_FRIDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnFriday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnSaturday = new storeFieldTypes(18, 900016, "SHIP_ON_SATURDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnSaturday; }, defaultValue: false);
        public static readonly storeFieldTypes ShipOnSunday = new storeFieldTypes(19, 900017, "SHIP_ON_SUNDAY", fieldDataTypes.Boolean, -1, delegate(StoreProfile prof) { return prof.ShipOnSunday; }, defaultValue: false);
        public static readonly storeFieldTypes LeadTime = new storeFieldTypes(20, 900010, "LEAD_TIME", fieldDataTypes.NumericInteger, -1, delegate(StoreProfile prof) { return prof.LeadTime; }, defaultValue: 0);
		// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        //public static readonly storeFieldTypes ImoID = new storeFieldTypes(21, 900805, "IMO_ID", fieldDataTypes.Text, 50, delegate(StoreProfile prof) { if (prof.IMO_ID == null) return string.Empty; else return prof.IMO_ID; });
        public static readonly storeFieldTypes ImoID = new storeFieldTypes(21, 900805, "IMO_ID", fieldDataTypes.Text, GetDatabaseColumnLength("STORES", "IMO_ID"), delegate(StoreProfile prof) { if (prof.IMO_ID == null) return string.Empty; else return prof.IMO_ID; });
		// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        public static readonly storeFieldTypes StoreStatus = new storeFieldTypes(22, 900022, "STATUS", fieldDataTypes.List, -1, delegate(StoreProfile prof) { return prof.Status; }, defaultValue: "Comp", allowEdit: false, onDB: false);


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

        // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
        private static int GetDatabaseColumnLength(string tableName, string columnName)
        {
            StoreData sd = new StoreData();
            return sd.GetColumnSize(tableName, columnName);
        }
		// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.

        public object defaultValue;
        private GetValueFromStoreProfile getValueFromProfile;

        public static implicit operator int(storeFieldTypes op) { return op.fieldIndex; }


        public object GetStoreValue(StoreProfile prof)
        {
            return getValueFromProfile(prof);
        }

        private static DataTable dtListValues = null;
        private static int GetStateValueFromAbbrev(string stateAbbrev)
        {
            if (dtListValues == null)
            {
                dtListValues = FieldTypeUtility.GetDataTableForListDropDowns();
                GlobalOptions globalOptionData = new GlobalOptions();
                string[] states = globalOptionData.GetStateAbbreviationsArray();

                //Add a blank state to start the list
                DataRow drBlank = dtListValues.NewRow();
                drBlank["LIST_VALUE_INDEX"] = -1;
                drBlank["LIST_VALUE"] = " ";
                dtListValues.Rows.Add(drBlank);

                for (int s = 0; s < states.Length; s++)
                {
                    DataRow dr = dtListValues.NewRow();
                    dr["LIST_VALUE_INDEX"] = s;
                    dr["LIST_VALUE"] = states[s];
                    dtListValues.Rows.Add(dr);
                }
            }
            DataRow[] drFound = dtListValues.Select("LIST_VALUE='" + stateAbbrev + "'");
            if (drFound.Length > 0)
            {
                return (int)drFound[0]["LIST_VALUE_INDEX"];
            }
            else
            {
                return -1;
            }
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

    public sealed class storeObjectTypes
    {
        public static List<storeObjectTypes> objectTypeList = new List<storeObjectTypes>();
        public static readonly storeObjectTypes StoreFields = new storeObjectTypes(1);
        public static readonly storeObjectTypes StoreCharacteristics = new storeObjectTypes(2);
        private storeObjectTypes(int index)
        {
            this.Index = index;
            objectTypeList.Add(this);
        }
        public int Index { get; private set; }
        public static implicit operator int(storeObjectTypes op) { return op.Index; }
        public static storeObjectTypes FromIndex(int index)
        {
            return objectTypeList.Find(x => x.Index == index);
        }
    }


    public static class StoreValidation
    {
        private const string _sourceModule = "StoreValidation.cs";
        public static SessionAddressBlock _SAB;
		private static bool storeSellingDateErrorWritten = false;
        
        private static object _writeLock = new object();
        public static void SetSAB(SessionAddressBlock SAB)
        {
            lock (_writeLock)
            {
                _SAB = SAB;
                
            }
        }

        public static void ResetValues()
        {
            storeSellingDateErrorWritten = false;
        }

     


     

  
       





        public static bool IsStoreIDValid(List<MIDMsg> msgList, validationKinds validationKind, object proposedValue, int storeRID)
        {
            if (proposedValue == null || proposedValue == DBNull.Value || proposedValue.ToString() == string.Empty)
            {
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileStoreIdRequired, msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileStoreIdRequired) });
                return false;
            }
            if (storeRID == StoreMgmt.ReserveStoreRID && proposedValue.ToString() != "Reserve")
            {
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileStoreIdForReserve, msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileStoreIdForReserve) });
                return false;
            }
			// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
            //if ((proposedValue.ToString()).Length > 50)
            if ((proposedValue.ToString()).Length > storeFieldTypes.StoreID.maxLength)
			// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
            {
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileStoreIdMaxLengthExceeded, msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileStoreIdMaxLengthExceeded) });
                return false;
            }

            if (validationKind == validationKinds.UponSaving) //Need to ensure Store ID is unique - delayed until actual saving of all changes
            {
                StoreMaint storeMaintData = new StoreMaint();
                if (storeMaintData.DoesStoreIdAlreadyExist(proposedValue.ToString(), storeRID))
                {
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileIdNotUnique, msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileIdNotUnique) });
                    return false;
                }
            }
            return true;
        }
        public static bool IsStoreNameValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
				    // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    //if ((proposedValue.ToString()).Length > 50)
                    if ((proposedValue.ToString()).Length > storeFieldTypes.StoreName.maxLength)
					// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StoreName.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
						// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
						//msgDetails += System.Environment.NewLine + storeFieldTypes.StoreName.Name + " can not exceed 50 characters.";
                        msgDetails += System.Environment.NewLine + storeFieldTypes.StoreName.Name + " can not exceed " + storeFieldTypes.StoreName.maxLength + " characters.";
						// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StoreName.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }
      
            return true;
        }
        public static bool IsStoreActiveIndicatorValid(List<MIDMsg> msgList, validationKinds validationKind, int storeRID, string storeId, bool storeDeleteIndicator, object proposedValue, object originalValue)
        {
            try
            {
                bool proposedActiveInd = Convert.ToBoolean(proposedValue);
                bool currentActiveInd = Convert.ToBoolean(originalValue);

                if (proposedActiveInd && storeDeleteIndicator)
                {
                    string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_StoreNotInactiveForDelete);
                    msgDetails = msgDetails.Replace("{0}", storeId);                 
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotInactiveForDelete, msg = msgDetails });
                    return false;
                }

                if (!proposedActiveInd && StoreMgmt.ReserveStoreRID == storeRID)
                {
                    string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_CannotMarkReserveStoreInactive);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_CannotMarkReserveStoreInactive, msg = msgDetails });
                    return false;
                }



                if (proposedActiveInd != currentActiveInd && proposedActiveInd == false && storeRID != Include.NoRID) //do not do this for new stores
                {
                    StoreData storeData = new StoreData();
                    int allocationCount = storeData.GetStoreAllocationCount(storeRID); //TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit
                    int intransitCount = storeData.GetStoreIntransitCount(storeRID);
                    if (allocationCount > 0 || intransitCount > 0)
                    {
                        string msgDetails = "Store: " + storeId;
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_DoNotAllowInactive);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_DoNotAllowInactive, msg = msgDetails });
                        return false;
                    }
                }
  
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.ActiveInd.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
        public static bool IsStoreDeleteIndicatorValid(List<MIDMsg> msgList, validationKinds validationKind, int storeRID, string storeId, bool storeActiveIndicator, object proposedValue, object originalValue, bool includeSoftTextMessage = false)
        {
            try
            {
                bool proposedDeleteInd = Convert.ToBoolean(proposedValue);
                bool currentDeleteInd = Convert.ToBoolean(originalValue);


                if (proposedDeleteInd && storeRID == Include.UndefinedStoreRID)  //attempting to delete a new store
                {
                    string msgDetails = "Store: " + storeId;
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotFound, msg = msgDetails });
                    return false;
                }

                // Begin TT#1917-MD - JSmith - Store Load API not marking stores as marked for deletion
                //if (proposedDeleteInd && (storeActiveIndicator == false))
                if (proposedDeleteInd && (storeActiveIndicator == true))
                // End TT#1917-MD - JSmith - Store Load API not marking stores as marked for deletion
                {
                    string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_StoreNotInactiveForDelete);
                    msgDetails = msgDetails.Replace("{0}", storeId);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreNotInactiveForDelete, msg = msgDetails });
                    return false;
                }

                if (proposedDeleteInd && StoreMgmt.ReserveStoreRID == storeRID)
                {
                    string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_CannotDeleteReserveStore);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_CannotDeleteReserveStore, msg = msgDetails });
                    return false;
                }

            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StoreDeleteInd.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
        public static bool IsCityValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
				    // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    //if ((proposedValue.ToString()).Length > 50)
                    if ((proposedValue.ToString()).Length > storeFieldTypes.City.maxLength)
					// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.City.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
						// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
						//msgDetails += System.Environment.NewLine + storeFieldTypes.City.Name + " can not exceed 50 characters.";
                        msgDetails += System.Environment.NewLine + storeFieldTypes.City.Name + " can not exceed " + storeFieldTypes.City.maxLength + " characters.";
						// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.City.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
        public static bool IsStateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && (string)proposedValue != string.Empty)
                {
				    // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    //if ((proposedValue.ToString()).Length > 50)
                    if ((proposedValue.ToString()).Length > storeFieldTypes.State.maxLength)
					// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.State.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
						// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
						//msgDetails += System.Environment.NewLine + storeFieldTypes.State.Name + " can not exceed 50 characters.";
                        msgDetails += System.Environment.NewLine + storeFieldTypes.State.Name + " can not exceed " + storeFieldTypes.State.maxLength + " characters.";
						// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.State.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }

        public static bool IsDescriptionValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId,  object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
     				// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    //if ((proposedValue.ToString()).Length > 50)
                    if ((proposedValue.ToString()).Length > storeFieldTypes.StoreDescription.maxLength)
					// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StoreDescription.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
						// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
						//msgDetails += System.Environment.NewLine + storeFieldTypes.StoreDescription.Name + " can not exceed 50 characters.";
                        msgDetails += System.Environment.NewLine + storeFieldTypes.StoreDescription.Name + " can not exceed " + storeFieldTypes.StoreDescription.maxLength + " characters.";
						// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StoreDescription.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }

        // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
        //public static bool IsImoIdValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        public static bool IsImoIdValid(List<MIDMsg> msgList, validationKinds validationKind, int storeRID, string storeId, object proposedValue)
        // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
				    // Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    //if ((proposedValue.ToString()).Length > 50)
                    if ((proposedValue.ToString()).Length > storeFieldTypes.ImoID.maxLength)
					// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.ImoID.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
						// Begin TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
						//msgDetails += System.Environment.NewLine + storeFieldTypes.ImoID.Name + " can not exceed 50 characters.";
                        msgDetails += System.Environment.NewLine + storeFieldTypes.ImoID.Name + " can not exceed " + storeFieldTypes.ImoID.maxLength + " characters.";
						// End TT#1830-MD - JSmith - Store Profiles select Edit Store- type in 50 characters and select OK.  Receive an Errormessage about executing the filte.
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
                // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
                //else if (proposedValue == null || proposedValue == DBNull.Value || proposedValue.ToString() == string.Empty)
                else if (proposedValue != null && proposedValue.ToString() == string.Empty)
                {
                    if (!StoreMgmt.AllowRemoveVSWID(storeRID))
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.ImoID.Name);
                        msgDetails = msgDetails.Replace("{2}", string.Empty);
                        msgDetails += System.Environment.NewLine + MIDText.GetTextFromCode((int)eMIDTextCode.msg_DoNotAllowRemoveVSW);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });

                        return false;
                    }
                }
                // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.ImoID.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }

        public static bool IsLeadTimeValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
                    int proposedInt = Convert.ToInt32(proposedValue, CultureInfo.CurrentUICulture);                    
                    if (proposedInt < 0)
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.LeadTime.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                        msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileLeadTimePositive);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.LeadTime.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }

        public static bool IsSellingSqFtValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object proposedValue)
        {
            try
            {
                if (proposedValue != null && proposedValue != DBNull.Value && proposedValue.ToString() != string.Empty)
                {
                    int proposedInt = Convert.ToInt32(proposedValue, CultureInfo.CurrentUICulture);
                    if (proposedInt < 0)
                    {
                        string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                        msgDetails = msgDetails.Replace("{0}", storeId);
                        msgDetails = msgDetails.Replace("{1}", storeFieldTypes.SellingSqFt.Name);
                        msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                        msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileSellingSqFtPositive);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                        return false;
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.SellingSqFt.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
		// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        //public static bool IsSellingOpenDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object closeDate, object proposedValue)
        public static bool IsSellingOpenDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object closeDate, object proposedValue, Session session)
		// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        {
            try
            {
                if (proposedValue != null )
                {
                    DateTime sellingOpenDt = Include.UndefinedDate;
                    if (proposedValue != null)
                    {
                        sellingOpenDt = Include.ConvertObjectToDateTime(proposedValue);
                    }
                    DateTime sellingCloseDt = Include.UndefinedDate;
                    if (closeDate != null)
                    {
                        sellingCloseDt = Include.ConvertObjectToDateTime(closeDate);
                    }

                    if (sellingOpenDt != Include.UndefinedDate && sellingCloseDt != Include.UndefinedDate)
                    {
                        if (sellingOpenDt > sellingCloseDt
                            && !storeSellingDateErrorWritten)
                        {
                            string msgDetails = "Store: " + storeId +
                                " " + storeFieldTypes.SellingOpenDate.Name + sellingOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                                " " + storeFieldTypes.SellingCloseDate.Name + sellingCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                            msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreSellingDateError);      
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreSellingDateError, msg = msgDetails });
                            storeSellingDateErrorWritten = true;
                            return false;
                        }
                    }
                    if (sellingOpenDt != Include.UndefinedDate)
                    {
                        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                        //return IsDateValidWithinMerchandiseCalendar(msgList, storeId, sellingOpenDt);
                        return IsDateValidWithinMerchandiseCalendar(msgList, storeId, sellingOpenDt, session);
                        // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.SellingOpenDate.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
		// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        //public static bool IsSellingCloseDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object openDate, object proposedValue)
        public static bool IsSellingCloseDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object openDate, object proposedValue, Session session)
		// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        {
            try
            {
                if (proposedValue != null)
                {
                    DateTime sellingCloseDt = Include.UndefinedDate;
                    if (proposedValue != null)
                    {
                        sellingCloseDt = Include.ConvertObjectToDateTime(proposedValue);
                    }
                    DateTime sellingOpenDt = Include.UndefinedDate;
                    if (openDate != null)
                    {
                        sellingOpenDt = Include.ConvertObjectToDateTime(openDate);
                    }

                    if (sellingCloseDt != Include.UndefinedDate && sellingOpenDt != Include.UndefinedDate)
                    {
                        if (sellingOpenDt > sellingCloseDt
                            && !storeSellingDateErrorWritten)
                        {
                            string msgDetails = "Store: " + storeId +
                                " " + storeFieldTypes.SellingOpenDate.Name + sellingOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                                " " + storeFieldTypes.SellingCloseDate.Name + sellingCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                            msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreSellingDateError);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreSellingDateError, msg = msgDetails });
                            storeSellingDateErrorWritten = true;
                            return false;
                        }
                    }
                    if (sellingCloseDt != Include.UndefinedDate)
                    {
                        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                        //return IsDateValidWithinMerchandiseCalendar(msgList, storeId, sellingCloseDt);
                        return IsDateValidWithinMerchandiseCalendar(msgList, storeId, sellingCloseDt, session);
                        // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.SellingCloseDate.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }

        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        //public static bool IsStockOpenDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object closeDate, object proposedValue)
        public static bool IsStockOpenDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object closeDate, object proposedValue, Session session)
		// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        {
            try
            {
                if (proposedValue != null)
                {
                    DateTime stockOpenDt = Include.UndefinedDate;
                    if (proposedValue != null)
                    {
                        stockOpenDt = Include.ConvertObjectToDateTime(proposedValue);
                    }
                    DateTime stockCloseDt = Include.UndefinedDate;
                    if (closeDate != null)
                    {
                        stockCloseDt = Include.ConvertObjectToDateTime(closeDate);
                    }

                    if (stockOpenDt != Include.UndefinedDate && stockCloseDt != Include.UndefinedDate)
                    {
                        if (stockOpenDt > stockCloseDt)
                        {
                            string msgDetails = "Store: " + storeId +
                                " " + storeFieldTypes.StockOpenDate.Name + stockOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                                " " + storeFieldTypes.StockCloseDate.Name + stockCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                            msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreStockDateError);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreStockDateError, msg = msgDetails });
                            return false;
                        }
                    }
                    if (stockOpenDt != Include.UndefinedDate)
                    {
                        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                        //return IsDateValidWithinMerchandiseCalendar(msgList, storeId, stockOpenDt);
                        return IsDateValidWithinMerchandiseCalendar(msgList, storeId, stockOpenDt, session);
                        // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StockOpenDate.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
		// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        //public static bool IsStockCloseDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object openDate, object proposedValue)
        public static bool IsStockCloseDateValid(List<MIDMsg> msgList, validationKinds validationKind, string storeId, object openDate, object proposedValue, Session session)
		// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        {
            try
            {
                if (proposedValue != null)
                {
                    DateTime stockCloseDt = Include.UndefinedDate;
                    if (proposedValue != null)
                    {
                        stockCloseDt = Include.ConvertObjectToDateTime(proposedValue);
                    }
                    DateTime stockOpenDt = Include.UndefinedDate;
                    if (openDate != null)
                    {
                        stockOpenDt = Include.ConvertObjectToDateTime(openDate);
                    }

                    if (stockCloseDt != Include.UndefinedDate && stockOpenDt != Include.UndefinedDate)
                    {
                        if (stockOpenDt > stockCloseDt)
                        {
                            string msgDetails = "Store: " + storeId +
                                " " + storeFieldTypes.StockOpenDate.Name + stockOpenDt.ToString("d", CultureInfo.CurrentUICulture) +
                                " " + storeFieldTypes.StockCloseDate.Name + stockCloseDt.ToString("d", CultureInfo.CurrentUICulture);
                            msgDetails += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_StoreStockDateError);
                            msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreStockDateError, msg = msgDetails });
                            return false;
                        }
                    }
                    if (stockCloseDt != Include.UndefinedDate)
                    {
                        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                        //return IsDateValidWithinMerchandiseCalendar(msgList, storeId, stockCloseDt);
                        return IsDateValidWithinMerchandiseCalendar(msgList, storeId, stockCloseDt, session);
                        // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    }
                }
            }
            catch
            {
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_InvalidStoreFieldDetails);
                msgDetails = msgDetails.Replace("{0}", storeId);
                msgDetails = msgDetails.Replace("{1}", storeFieldTypes.StockCloseDate.Name);
                msgDetails = msgDetails.Replace("{2}", proposedValue.ToString());
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_InvalidStoreField, msg = msgDetails });
                return false;
            }

            return true;
        }
        // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        //public static bool IsDateValidWithinMerchandiseCalendar(List<MIDMsg> msgList, string storeId, DateTime dtValue)
        public static bool IsDateValidWithinMerchandiseCalendar(List<MIDMsg> msgList, string storeId, DateTime dtValue, Session session)
        // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
        {
            try
            {
                bool isValid = true;
                if (dtValue != Include.UndefinedDate)
                {
                    // Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    //if (!_SAB.StoreServerSession.Calendar.IsValidMerchandiseCalendarDate(dtValue))
                    if (!session.Calendar.IsValidMerchandiseCalendarDate(dtValue))
                    // End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    {
                        string msgDetails = "Store: " + storeId +
                            " Date: " + dtValue.ToString("d", CultureInfo.CurrentUICulture);
                        msgDetails += " " + MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileDateNotInMerchandiseCalendar);
                        msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileDateNotInMerchandiseCalendar, msg = msgDetails });
                        isValid = false;
                    }
                }
                return isValid;
            }
            catch
            {
                throw;
            }
        }

        public static bool hasActiveFieldChanged = false;
        public static bool IsStoreFieldValid(validationKinds validationKind, int objectType, int fieldIndex, int storeRID, object originalValue, object proposedValue, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
        {
            bool isValid = true;
            // Since static class, class values must be reset for each store
            ResetValues();

            if (objectType == storeObjectTypes.StoreFields) //Store Fields
            {
                if (fieldIndex == storeFieldTypes.StoreID)
                {
                    isValid = IsStoreIDValid(msgList, validationKind, proposedValue, storeRID);
                }
                else if (fieldIndex == storeFieldTypes.StoreName)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsStoreNameValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.ActiveInd)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    // Begin TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception
                    //bool storeDeleteInd = Convert.ToBoolean(Include.ConvertCharToBool(Convert.ToChar(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreDeleteInd))));
                    bool storeDeleteInd = GetBoolValue(storeRID, objectType, fieldIndex, fieldValueGetCurrent, storeFieldTypes.StoreDeleteInd);
                    // End TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception
                    isValid = IsStoreActiveIndicatorValid(msgList, validationKind, storeRID, storeId, storeDeleteInd, proposedValue, originalValue);
                    if (isValid)
                    {
                        bool proposedActiveInd = Convert.ToBoolean(proposedValue);
                        bool currentActiveInd = Convert.ToBoolean(originalValue);
                        if (proposedActiveInd != currentActiveInd)
                        {
                            hasActiveFieldChanged = true; //when editing fields, mark active indicator as changed   
                        }
                    }
                }
                else if (fieldIndex == storeFieldTypes.StoreDeleteInd)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    // Begin TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception
                    //bool storeActiveInd = Convert.ToBoolean(fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.ActiveInd));
                    bool storeActiveInd = GetBoolValue(storeRID, objectType, fieldIndex, fieldValueGetCurrent, storeFieldTypes.ActiveInd);
                    // End TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception
                    isValid = IsStoreDeleteIndicatorValid(msgList, validationKind, storeRID, storeId, storeActiveInd, proposedValue, originalValue);
                }
       
                else if (fieldIndex == storeFieldTypes.StoreDescription)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsDescriptionValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.City)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsCityValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.State)
                {
                    //string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    //isValid = IsStateValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.ImoID)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
                    //isValid = IsImoIdValid(msgList, validationKind, storeId, proposedValue);
                    isValid = IsImoIdValid(msgList, validationKind, storeRID, storeId, proposedValue);
                    // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
                }
                else if (fieldIndex == storeFieldTypes.LeadTime)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsLeadTimeValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.SellingSqFt)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    isValid = IsSellingSqFtValid(msgList, validationKind, storeId, proposedValue);
                }
                else if (fieldIndex == storeFieldTypes.SellingOpenDate)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    object sellingCloseDate = fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.SellingCloseDate);
					// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    //isValid = IsSellingOpenDateValid(msgList, validationKind, storeId, sellingCloseDate, proposedValue);
                    isValid = IsSellingOpenDateValid(msgList, validationKind, storeId, sellingCloseDate, proposedValue, _SAB.ClientServerSession);
					// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates

                    if (isValid && proposedValue != null && sellingCloseDate != null) //update the store status
                    {
                        DateTime sellingOpenDt = Include.ConvertObjectToDateTime(proposedValue);
                        DateTime sellingCloseDt = Include.ConvertObjectToDateTime(sellingCloseDate);
                        WeekProfile currentWeek = _SAB.ClientServerSession.Calendar.CurrentWeek;
                        eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
                        fieldValueSetCurrent(storeRID, storeObjectTypes.StoreFields, storeFieldTypes.StoreStatus, (int)storeStatus);
                    }

                }
                else if (fieldIndex == storeFieldTypes.SellingCloseDate)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    object sellingOpenDate = fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.SellingOpenDate);
					// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    //isValid = IsSellingCloseDateValid(msgList, validationKind, storeId, sellingOpenDate, proposedValue);
                    isValid = IsSellingCloseDateValid(msgList, validationKind, storeId, sellingOpenDate, proposedValue, _SAB.ClientServerSession);
					// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates

                    if (isValid && proposedValue != null && sellingOpenDate != null) //update the store status
                    {
                        DateTime sellingOpenDt = Include.ConvertObjectToDateTime(sellingOpenDate);
                        DateTime sellingCloseDt = Include.ConvertObjectToDateTime(proposedValue);
                        WeekProfile currentWeek = _SAB.ClientServerSession.Calendar.CurrentWeek;
                        eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
                        fieldValueSetCurrent(storeRID, storeObjectTypes.StoreFields, storeFieldTypes.StoreStatus, (int)storeStatus);
                    }
                }
                else if (fieldIndex == storeFieldTypes.StockOpenDate)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    object stockCloseDate = fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StockCloseDate);
					// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    //isValid = IsStockOpenDateValid(msgList, validationKind, storeId, stockCloseDate, proposedValue);
                    isValid = IsStockOpenDateValid(msgList, validationKind, storeId, stockCloseDate, proposedValue, _SAB.ClientServerSession);
					// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                }
                else if (fieldIndex == storeFieldTypes.StockCloseDate)
                {
                    string storeId = (string)fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StoreID);
                    object stockOpenDate = fieldValueGetCurrent(storeRID, objectType, storeFieldTypes.StockOpenDate);
					// Begin TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
                    //isValid = IsStockCloseDateValid(msgList, validationKind, storeId, stockOpenDate, proposedValue);
                    isValid = IsStockCloseDateValid(msgList, validationKind, storeId, stockOpenDate, proposedValue, _SAB.ClientServerSession);
					// End TT#1843-MD - JSmith - Unable to Change and Save Selling and Stock Open Dates
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
                else if (fieldIndex == storeFieldTypes.SimilarStoreModel)
                {
                }
               
            }
            else if (objectType == storeObjectTypes.StoreCharacteristics) //Store Characteristics
            {
                int scgRID = fieldIndex;

            }
            return isValid;
        }

        // Begin TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception
        private static bool GetBoolValue(int storeRID, int objectType, int fieldIndex, FieldValueGetForCurrentField fieldValueGetCurrent, storeFieldTypes fieldType)
        {
            bool boolValue = false;
            object objectValue = fieldValueGetCurrent(storeRID, objectType, fieldType);
            if (objectValue != null)
            {
                string currentValue = fieldValueGetCurrent(storeRID, objectType, fieldType).ToString().ToUpper();
                if (currentValue == "FALSE")
                {
                    boolValue = false;
                }
                else if (currentValue == "TRUE")
                {
                    boolValue = true;
                }
                else
                {
                    boolValue = Convert.ToBoolean(Include.ConvertCharToBool(Convert.ToChar(currentValue)));
                }
            }
            return boolValue;
        }
        // End TT#1816-MD - JSmith - Str Profile - Select Edit Store - Deselect Active - Click in Selling Sq Ft Value Window - Receive Unhandled Exception

        public static bool IsStoreObjectValid(int storeRID, ref DataSet dsFields, List<MIDMsg> msgList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
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

                    isValid = IsStoreFieldValid(validationKinds.UponSaving, objectType, fieldIndex, storeRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                    if (isValid == false)
                    {
                        break;
                    }

                    if (curVal != origVal)
                    {
                        cRow["IS_DIRTY"] = true;
                    }
                    else
                    {
                        cRow["IS_DIRTY"] = false;
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

                        isValid = IsStoreFieldValid(validationKinds.UponSaving, objectType, fieldIndex, storeRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                        if (isValid == false)
                        {
                            break;
                        }
                        DataRow[] drField = dsFields.Tables[0].Select("OBJECT_TYPE=" + objectType.ToString() + " AND FIELD_INDEX=" + fieldIndex.ToString());
                        
                        if (curVal != origVal)
                        {
                            drField[0]["IS_DIRTY"] = true;
                        }
                        else
                        {
                            drField[0]["IS_DIRTY"] = false;
                        }
                    }
                }
            }

            CheckIfActiveFieldChanged(msgList, ref isValid);

            return isValid;
        }
        public static bool IsStoreObjectValidForFieldEditing(ref DataSet dsFields, List<MIDMsg> msgList, List<fieldColumnMap> columnMapList, FieldValueGetForCurrentField fieldValueGetCurrent, FieldValueSetForCurrentField fieldValueSetCurrent)
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
                                isValid = IsStoreFieldValid(validationKinds.UponSaving, map.objectType, map.fieldIndex, objectRID, origVal, curVal, msgList, fieldValueGetCurrent, fieldValueSetCurrent);
                                if (isValid == false)
                                {
                                    break;
                                }
                                map.isDirty = true;
                            }
                        }
                    }
                }
            }
            CheckIfActiveFieldChanged(msgList, ref isValid);

            return isValid;
        }
        public static void CheckIfActiveFieldChanged(List<MIDMsg> msgList, ref bool isValid)
        {
            if (hasActiveFieldChanged)
            {
                //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreActiveChanged, MIDText.GetTextOnly(eMIDTextCode.msg_StoreActiveChanged), _sourceModule);
                //em.DisplayPrompt = true;
                //isValid = false;
                // Begin TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
                string msgDetails = MIDText.GetTextFromCode((int)eMIDTextCode.msg_StoreActiveChanged);
                msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Warning, textCode = eMIDTextCode.msg_StoreNotInactiveForDelete, msg = msgDetails });
                isValid = false;
                // End TT#1818-MD - JSmith - Str Profiles - Select Edit Field - Str Focus Sim Str model wmns- deselect Active - Selling Sq Ft type in 15000- select OK- expect mssg Msg="Store active flag has been changed.  etc.- no mssg received
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
                    drBlank[displayField] =  " ";
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
                //int miniIndex = 0;
                foreach (DataRow drCharListValue in drValueListForGroup)
                {
                    DataRow dr = dtListValues.NewRow();
                    dr[dataField] = Convert.ToInt32(drCharListValue["SC_RID"]);  //miniIndex;
                    dr[displayField] = drCharListValue["CHAR_LIST_VALUE"].ToString();
                    dtListValues.Rows.Add(dr);
                    //miniIndex++;
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

            int key = -1;

            if (dsValues.Tables[0].Rows[0]["ST_RID"] != DBNull.Value)
                key = Convert.ToInt32(dsValues.Tables[0].Rows[0]["ST_RID"], CultureInfo.CurrentUICulture);

            StoreProfile prof = new StoreProfile(key);
            prof.LoadFieldsFromDataRow(dsValues.Tables[0].Rows[0]);
            StoreMgmt.StoreProfile_UpdateStatusAndText(ref prof);
            //StoreProfile prof = StoreMgmt.ConvertToStoreProfile(, false); //SAB.StoreServerSession.ConvertToStoreProfile(dsValues.Tables[0].Rows[0], false);

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
                if (dataType == fieldDataTypes.List)
                {
                    dr["FIELD_VALUE"] = drChar["SC_RID"];
                }
                else
                {
                    dr["FIELD_VALUE"] = drChar["CHAR_VALUE"];
                }
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
                    //if (storeField.allowEdit == true) //Store status cannot be edited  // TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
                    {
                        dtStoreFieldsAndChars.Columns.Add(storeField.Name, fieldDataTypes.GetTypeForField(storeField.dataType));

                        //make a column map entry
                        fieldColumnMap map = new fieldColumnMap(objectType, fieldIndex, columnIndex);
                        map.fieldDataType = storeField.dataType;
                        // Begin TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
                        map.fieldActualDataType = storeField.dataType;
                        // End TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
                        columnMapList.Add(map);
                        // Begin TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
                        if (storeField.allowEdit == false)
                        {
                            dtStoreFieldsAndChars.Columns[columnIndex].ReadOnly = true;
                        }
                        // End TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
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
                        // Begin TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
                        map.fieldActualDataType = fieldDataTypes.FromChar(Convert.ToInt32(drCharGroup[0]["SCG_TYPE"]), "0");
                        // End TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
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
                        // Begin TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
                        else
                        {
                            fieldDataTypes.AssignValueToDataRow(ref drValue, storeField.Name, storeField.dataType.Index, storeField.GetStoreValue(StoreMgmt.StoreProfile_Get(storeRID)).ToString());
                        }
                        // End TT#1804-MD - JSmith - Store Profiles - Highligt Store Status- Select Edit Field - Store Status does not appear in the Store Profiles Fields (Edit)
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

        // Begin TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
        public static bool ClearStoreCharValue(int scgRID, int scRID)
        {
            bool emptyStoreCharValue = false;
            string dataField = string.Empty;
            string displayField = string.Empty;
            DataTable dtCharList = GetListValuesForStoreField(storeObjectTypes.StoreCharacteristics.Index, scgRID, ref dataField, ref displayField);
            DataRow[] drCharTemp = dtCharList.Select("LIST_VALUE_INDEX ='" + scRID + "'");
            if (drCharTemp.Length > 0)
            {
                string value = Convert.ToString(drCharTemp[0]["LIST_VALUE"]);
                if (value == "None")
                {
                    emptyStoreCharValue = true;
                }
            }
            return emptyStoreCharValue;
        }
        // End TT#1881-MD - JSmith - Store Value set to None in different format.  When str set with value did not go in to Available store Set
    }
}
