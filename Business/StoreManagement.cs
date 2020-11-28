using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public static class StoreMgmt
    {
        public static int StoreCharGroup_Insert(ref EditMsgs em, bool addInfoMsgUponInsert, int userRID, string charGroupName, eStoreCharType aCharType, bool hasList, string storeId = "", string sourceModule = "")
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            int scgRID = charMaint.StoreCharGroup_Insert(charGroupName, aCharType, hasList: false);
            if (addInfoMsgUponInsert)
            {
                string msgDetails = MIDText.GetText(eMIDTextCode.msg_AutoAddedCharacteristic, false);
                msgDetails = msgDetails.Replace("{0}", charGroupName);
                em.AddMsg(eMIDMessageLevel.Information, eMIDTextCode.msg_AutoAddedCharacteristic, msgDetails, sourceModule);
            }

            StoreGroupMaint groupMaint = new StoreGroupMaint();
            if (groupMaint.DoesStoreGroupNameAlreadyExist(charGroupName, userRID) == true)
            {
                string msgDetails = string.Empty;
                if (storeId != string.Empty)
                {
                    msgDetails += "Store: " + storeId;
                }
                msgDetails += " Duplicate Attribute Exits: " + charGroupName;
                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateAttributeToCharacteristic, msgDetails, sourceModule);

            }
            else
            {
                StoreGroupInsertDynamic(scgRID, charGroupName, userRID);
            }
            return scgRID;
        }

        public static void StoreCharGroup_Update(int scgRID, string charGroupName, eStoreCharType aCharType, bool hasList)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            charMaint.StoreCharGroup_Update(scgRID, charGroupName, aCharType, hasList);
        }
        public static void StoreCharGroup_Delete(int scgRid)
        {
            StoreCharMaint storeCharMaint = new StoreCharMaint();
            storeCharMaint.StoreCharGroup_Delete(scgRid);
        }

        public static int StoreCharGroup_Find(string charGroupName)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            DataRow drCharGroup = charMaint.StoreCharGroupReadFromId(charGroupName);

            int scgRID = Include.NoRID;

            if (drCharGroup != null)
            {
                scgRID = (int)drCharGroup["SCG_RID"];
            }

            return scgRID;
        }
        public static bool DoesStoreCharGroupNameAlreadyExist(string proposedName, int charGroupEditRID)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            return charMaint.DoesStoreCharGroupNameAlreadyExist(proposedName, charGroupEditRID);
        }
        public static DataTable StoreCharGroup_ReadValues(int scgRID)
        {
            StoreCharMaint storeCharMaint = new StoreCharMaint();
            return storeCharMaint.StoreCharGroup_ReadValues(scgRID);
        }


        public static int StoreCharValue_Insert(int scgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            int scRID = Include.NoRID;

            StoreCharMaint charMaint = new StoreCharMaint();
            scRID = charMaint.StoreCharValue_Insert(scgRID, textValue, dateValue, numberValue, dollarValue);

            return scRID;
        }
        public static void StoreCharValue_Update(int scRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            StoreCharMaint charMaint = new StoreCharMaint();
            charMaint.StoreCharValue_Update(scRID, textValue, dateValue, numberValue, dollarValue);
        }
        public static void StoreCharValue_Delete(int scRid)
        {
            StoreCharMaint storeCharMaint = new StoreCharMaint();
            storeCharMaint.StoreCharValue_Delete(scRid);
        }
        public static bool DoesStoreCharValueAlreadyExist(object proposedValue, fieldDataTypes charGroupFieldType, int charGroupRID, int valueRID, ref int scRID)
        {
            StoreCharMaint charMaintData = new StoreCharMaint();
            bool doesValueAlreadyExist = false;

            if (charGroupFieldType == fieldDataTypes.Text)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueTextAlreadyExist(Convert.ToString(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.DateNoTime)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueDateAlreadyExist(Convert.ToDateTime(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.NumericDouble)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueNumberAlreadyExist(Convert.ToSingle(proposedValue), charGroupRID, valueRID, ref scRID);
            }
            else if (charGroupFieldType == fieldDataTypes.NumericDollar)
            {
                doesValueAlreadyExist = charMaintData.DoesStoreCharValueDollarAlreadyExist(Convert.ToSingle(proposedValue), charGroupRID, valueRID, ref scRID);
            }

            return doesValueAlreadyExist;
        }


        /// <summary>
        /// Creates a attribute set filter with a dynamic condition
        /// </summary>
        /// <param name="scgRID"></param>
        /// <param name="charGroupName"></param>
        /// <param name="userRID"></param>
        public static void StoreGroupInsertDynamic(int scgRID, string charGroupName, int userRID)
        {

        }

        public static void RefreshStoresInAllGroups()
        {

        }
    }
}
