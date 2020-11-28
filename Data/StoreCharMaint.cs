using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreCharMaint : DataLayer
	{	


		public StoreCharMaint() : base()
		{

		}

        public StoreCharMaint(string aConnectionString)
			: base(aConnectionString)
		{

		}

        public DataTable StoreCharGroup_ReadValues(int scgRID)
        {
            try
            {
                return StoredProcedures.MID_STORE_CHAR_READ_FOR_GROUP_MAINT.Read(_dba, SCG_RID: scgRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataTable StoreCharGroup_Read(int scgRID)
        {
            try
            {
                return StoredProcedures.MID_STORE_CHAR_GROUP_READ.Read(_dba, SCG_RID: scgRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public int StoreCharGroup_Insert(string charGroupName, eStoreCharType aCharType, bool hasList)
        {
            try
            {
                int scgRid = -1;
                char cHasList = Include.ConvertBoolToChar(hasList);
                _dba.OpenUpdateConnection();
                scgRid = StoredProcedures.SP_MID_STORECHARGROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                          SCG_ID: charGroupName,
                                                                                          SCG_TYPE: Convert.ToInt32(aCharType),
                                                                                          SCG_LIST_IND: cHasList
                                                                                          );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
                return scgRid;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void StoreCharGroup_Update(int scgRID, string charGroupName, eStoreCharType aCharType, bool hasList)
        {
            try
            {
               
                char cHasList = Include.ConvertBoolToChar(hasList);
                _dba.OpenUpdateConnection();
                int rowsUpdated = StoredProcedures.MID_STORE_CHAR_GROUP_UPDATE.Update(_dba,
                                                                                          SCG_RID: scgRID,
                                                                                          SCG_ID: charGroupName,
                                                                                          SCG_TYPE: Convert.ToInt32(aCharType),
                                                                                          SCG_LIST_IND: cHasList
                                                                                          );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool DoesStoreCharGroupNameAlreadyExist(string proposedName, int charGroupEditRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_GROUP_NAME: proposedName,
                                                                                    SCG_EDIT_RID: charGroupEditRID);
            int matchCount = (int)dt.Rows[0]["MYCOUNT"];
            if (matchCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataRow StoreCharGroupReadFromId(string scgID)
        {
            DataRow drStoreCharGroup = null;
            DataTable dt = StoredProcedures.MID_STORE_CHAR_GROUP_READ_FROM_ID.Read(_dba, SCG_ID: scgID);
            
            if (dt.Rows.Count > 0)
            {
                drStoreCharGroup= dt.Rows[0];
            }

            return drStoreCharGroup;
        }

        public bool DoesStoreCharValueTextAlreadyExist(string proposedText, int charGroupRID, int valueEditRID, ref int scRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedText,
                                                                                    SCG_RID: charGroupRID,
                                                                                    SC_EDIT_RID: valueEditRID);
            scRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                scRID = (int)dt.Rows[0]["SC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DoesStoreCharValueDateAlreadyExist(DateTime proposedValue, int charGroupRID, int valueEditRID, ref int scRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    SCG_RID: charGroupRID,
                                                                                    SC_EDIT_RID: valueEditRID);
            scRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                scRID = (int)dt.Rows[0]["SC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesStoreCharValueNumberAlreadyExist(float proposedValue, int charGroupRID, int valueEditRID, ref int scRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    SCG_RID: charGroupRID,
                                                                                    SC_EDIT_RID: valueEditRID);
            scRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                scRID = (int)dt.Rows[0]["SC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesStoreCharValueDollarAlreadyExist(float proposedValue, int charGroupRID, int valueEditRID, ref int scRID)
        {
            DataTable dt = StoredProcedures.MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    SCG_RID: charGroupRID,
                                                                                    SC_EDIT_RID: valueEditRID);
            scRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                scRID = (int)dt.Rows[0]["SC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public int StoreCharValue_Insert(int scgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue) 
        {         
            try
            {
                int scRid = Include.NoRID;
                _dba.OpenUpdateConnection();
                scRid = StoredProcedures.SP_MID_STORECHAR_INSERT.InsertAndReturnRID(_dba,
                                                                            SCG_RID: scgRID,
                                                                            TEXT_VALUE: textValue,
                                                                            DATE_VALUE: dateValue,
                                                                            NUMBER_VALUE: numberValue,
                                                                            DOLLAR_VALUE: dollarValue
                                                                            );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
                return scRid;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void StoreCharValue_Update(int scRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            try
            {
                _dba.OpenUpdateConnection();
                int rowsUpdate = StoredProcedures.MID_STORE_CHAR_UPDATE.Update(_dba,
                                                                                SC_RID: scRID,
                                                                                TEXT_VALUE: textValue,
                                                                                DATE_VALUE: dateValue,
                                                                                NUMBER_VALUE: numberValue,
                                                                                DOLLAR_VALUE: dollarValue
                                                                                );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void StoreCharValue_Delete(int scRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_STORE_CHAR_DELETE.Delete(_dba, SC_RID: scRid);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreCharGroup_Delete(int scgRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_STORE_CHAR_GROUP_DELETE.Delete(_dba, SCG_RID: scgRid);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreCharValueJoin_Insert(int storeRID, int scRID, int scgRID)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_STORE_CHAR_JOIN_INSERT.Insert(_dba,
                                                                    SCG_RID: scgRID,
                                                                    ST_RID: storeRID,
                                                                    SC_RID: scRID
                                                                    );

                _dba.CommitData();
                _dba.CloseUpdateConnection();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void StoreCharValueJoin_DeleteForGroupAndStore(int storeRID, int scgRID)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE.Delete(_dba,
                                                                    SCG_RID: scgRID,
                                                                    ST_RID: storeRID
                                                                    );

                _dba.CommitData();
                _dba.CloseUpdateConnection();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

	}
}
