using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class FilterData : DataLayer
	{
        public FilterData() : base()
		{

		}
        public bool DoesFilterNameAlreadyExist(filterTypes filterType, string proposedName, int filterEditRID)
        {
            DataTable dt = StoredProcedures.MID_FILTER_READ_NAME_FOR_DUPLICATE.Read(_dba,
                                                                                    FILTER_TYPE: filterType.dbIndex,
                                                                                    PROPOSED_FILTER_NAME: proposedName,
                                                                                    FILTER_EDIT_RID: filterEditRID);
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

        public int InsertFilter(filterTypes filterType, int aUserRid, int aOwnerUserRID, string aFilterName, bool isLimited, int aResultLimit)
        {
            try
            {


                _dba.OpenUpdateConnection();
                int newFilterRID =  StoredProcedures.MID_FILTER_INSERT.InsertAndReturnRID(_dba,
                                                                        USER_RID: aUserRid,
                                                                        OWNER_USER_RID: aOwnerUserRID,
                                                                        FILTER_TYPE: filterType.dbIndex,
                                                                        FILTER_NAME: aFilterName,
                                                                        IS_LIMITED: Include.ConvertBoolToInt(isLimited),
                                                                        RESULT_LIMIT: aResultLimit);


            	_dba.CommitData();
				_dba.CloseUpdateConnection();



             
    

                return newFilterRID;
			}
			catch (Exception ex)
			{
				_dba.RollBack();
				_dba.CloseUpdateConnection();
				throw;
			}
        }
        public int InsertFilterForAuditDefault(int aUserRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                int newFilterRID = StoredProcedures.MID_FILTER_INSERT_AUDIT_DEFAULT.InsertAndReturnRID(_dba, USER_RID: aUserRid);

                _dba.CommitData();
                _dba.CloseUpdateConnection();

                return newFilterRID;
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }

        // Begin TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
        //public void UpdateFilter(int aFILTER_RID, int aUserRid, string aFilterName, bool isLimited, int aResultLimit)
        public void UpdateFilter(int aFILTER_RID, int aUserRid, int aOwnerUserRid, string aFilterName, bool isLimited, int aResultLimit)
        // End TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_FILTER_UPDATE.Update(_dba,
                                                               FILTER_RID: aFILTER_RID,
                                                               USER_RID: aUserRid,
                                                               OWNER_USER_RID: aOwnerUserRid,   // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                                                               FILTER_NAME: aFilterName,
                                                               IS_LIMITED: Include.ConvertBoolToInt(isLimited),
                                                               RESULT_LIMIT: aResultLimit
                                                               );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
         
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        public void UpdateFilterNameAndUser(int aFILTER_RID, int aUserRid, string aFilterName)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_FILTER_UPDATE_NAME.Update(_dba,
                                                               FILTER_RID: aFILTER_RID,
                                                               USER_RID: aUserRid,
                                                               FILTER_NAME: aFilterName
                                                               );
                _dba.CommitData();
                _dba.CloseUpdateConnection();

            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        public int FilterGetKey(filterTypes filterType, int aUserRID, string aFilterName)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_FILTER_READ_FROM_USER_AND_NAME.Read(_dba,
                                                                                        FILTER_TYPE: filterType.dbIndex,
                                                                                        USER_RID: aUserRID,
                                                                                        FILTER_NAME: aFilterName
                                                                                        );

                if (dt.Rows.Count == 1)
                {
                    return (Convert.ToInt32(dt.Rows[0]["FILTER_RID"], CultureInfo.CurrentUICulture));
                }
                else
                {
                    return -1;
                }
               
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //Begin TT#1313-MD -jsobek -Header Filters
        public DataTable FilterReadAllForUser(filterTypes filterType, int aUserRID)
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ_FROM_USER.Read(_dba,
                                                                        FILTER_TYPE: filterType.dbIndex,
                                                                        USER_RID: aUserRID
                                                                        );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters
        public string FilterGetName(int filterRid)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_FILTER_READ.Read(_dba, FILTER_RID: filterRid);

                if (dt.Rows.Count == 1)
                {
                    return dt.Rows[0]["FILTER_NAME"].ToString();
                }
                else
                {
                    return "unknown";
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        public int FilterGetOwner(int filterRid)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_FILTER_READ.Read(_dba, FILTER_RID: filterRid);

                if (dt.Rows.Count == 1)
                {
                    return Convert.ToInt32(dt.Rows[0]["OWNER_USER_RID"]);
                }
                else
                {
                    return Include.Undefined;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        public DateTime FilterGetUpdateDate(int filterRid)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_FILTER_READ.Read(_dba, FILTER_RID: filterRid);

                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["UPDATE_DATE"] == System.DBNull.Value)
                    {
                        return DateTime.MinValue;
                    }
                    else
                    {
                        return Convert.ToDateTime(dt.Rows[0]["UPDATE_DATE"]);
                    }
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void FilterUpdate(StoreFilterProfile aStoreFilterProfile)
        {
            try
            {
                FilterUpdate(aStoreFilterProfile.UnloadToDataRow(NewStoreFilterRow()));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //Begin TT#1313-MD -jsobek -Header Filters
        public void FilterUpdate(HeaderFilterProfile aFilterProfile)
        {
            try
            {
                FilterUpdate(aFilterProfile.UnloadToDataRow(NewStoreFilterRow()));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void FilterUpdate(AssortmentFilterProfile aFilterProfile)
        {
            try
            {
                FilterUpdate(aFilterProfile.UnloadToDataRow(NewStoreFilterRow()));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void FilterUpdate(StoreGroupFilterProfile aFilterProfile)
        {
            try
            {
                FilterUpdate(aFilterProfile.UnloadToDataRow(NewStoreFilterRow()));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void ExecuteCreateOrAlterFilterProcedure(string sql)
        {
            try
            {
                _dba.OpenUpdateConnection();
                _dba.ExecuteNonQuery(sql);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

        public DataTable ExecuteSqlForResultSet(string sql)
        {
            try
            {
                return _dba.ExecuteSQLQuery(sql, "resultSet");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet ExecuteSqlForAttributeFilters(string sql, int timeout)
        {
            try
            {
                return _dba.ExecuteSQLQueryForUpdateWithResults(sql, timeout);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DataRow NewStoreFilterRow()
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ_DEFAULT.Read(_dba).NewRow();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void FilterUpdate(DataRow aStoreFilterRow)
        {
            try
            {
                int filterRID = (int)aStoreFilterRow["FILTER_RID"];
                int userRID = (int)aStoreFilterRow["USER_RID"];
                // Begin TT#2036-MD - JSmith - Assortment Filters - Cut
                //int ownerUserRID = (int)aStoreFilterRow["OWNER_USER_RID"];  // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                int ownerUserRID = userRID;
                if (aStoreFilterRow["OWNER_USER_RID"] != DBNull.Value)
                {
                    ownerUserRID = (int)aStoreFilterRow["OWNER_USER_RID"];
                }
                // End TT#2036-MD - JSmith - Assortment Filters - Cut
                string filterName = (string)aStoreFilterRow["FILTER_NAME"];
                bool isLimited = false;
                if (aStoreFilterRow["IS_LIMITED"] != DBNull.Value) isLimited = (bool)aStoreFilterRow["IS_LIMITED"];

                int resultLimit = -1;
                if (aStoreFilterRow["RESULT_LIMIT"] != DBNull.Value) resultLimit = (int)aStoreFilterRow["RESULT_LIMIT"];

                StoredProcedures.MID_FILTER_UPDATE.Update(_dba,
                                                                FILTER_RID: filterRID,
                                                                USER_RID: userRID,
                                                                OWNER_USER_RID: ownerUserRID, // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                                                                FILTER_NAME: filterName,
                                                                IS_LIMITED: Include.ConvertBoolToInt(isLimited),
                                                                RESULT_LIMIT: resultLimit
                                                                );


            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
      
        public int InsertCondition(int aFILTER_RID,
                                    int aSEQ,
                                    int aPARENT_SEQ,
                                    int aSIBLING_SEQ,
                                    int aELEMENT_GROUP_TYPE_INDEX,
                                    int aLOGIC_INDEX,
                                    int aFIELD_INDEX,
                                    int aOPERATOR_INDEX,
                                    int aVALUE_TYPE_INDEX,
                                    int aDATE_TYPE_INDEX,
                                    int aNUMERIC_TYPE_INDEX,
                                    string aVALUE_TO_COMPARE,
                                    double? aVALUE_TO_COMPARE_DOUBLE,
                                    double? aVALUE_TO_COMPARE_DOUBLE2,
                                    int? aVALUE_TO_COMPARE_INT,
                                    int? aVALUE_TO_COMPARE_INT2,
                                    bool? aVALUE_TO_COMPARE_BOOL,
                                    DateTime? aVALUE_TO_COMPARE_DATE_FROM,
                                    DateTime? aVALUE_TO_COMPARE_DATE_TO,
                                    int aVALUE_TO_COMPARE_DATE_DAYS_FROM,
                                    int aVALUE_TO_COMPARE_DATE_DAYS_TO,
                                    int aVAR1_VARIABLE_INDEX,
                                    int aVAR1_VERSION_INDEX,
                                    int aVAR1_HN_RID,
                                    int aVAR1_CDR_RID,
                                    int aVAR1_VALUE_TYPE_INDEX,
                                    int aVAR1_TIME_INDEX,
                                    int aVAR_PERCENTAGE_OPERATOR_INDEX,
                                    int aVAR2_VARIABLE_INDEX,
                                    int aVAR2_VERSION_INDEX,
                                    int aVAR2_HN_RID,
                                    int aVAR2_CDR_RID,
                                    int aVAR2_VALUE_TYPE_INDEX,
                                    int aVAR2_TIME_INDEX,
                                    int aHEADER_HN_RID,
                                    //int aHEADER_PH_RID,
                                    int aSORT_BY_TYPE_INDEX,
                                    int aSORT_BY_FIELD_INDEX,
                                    int aLIST_VALUE_CONSTANT_INDEX,
                                    int aDATE_CDR_RID   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                                  )
        {
            try
            {
                _dba.OpenUpdateConnection();


                int? LOGIC_INDEX_Nullable = null;
                if (aLOGIC_INDEX != -1) LOGIC_INDEX_Nullable = aLOGIC_INDEX;

                int? FIELD_INDEX_Nullable = null;
                if (aFIELD_INDEX != -1) FIELD_INDEX_Nullable = aFIELD_INDEX;

                int? OPERATOR_INDEX_Nullable = null;
                if (aOPERATOR_INDEX != -1) OPERATOR_INDEX_Nullable = aOPERATOR_INDEX;

                int? VALUE_TYPE_INDEX_Nullable = null;
                if (aVALUE_TYPE_INDEX != -1) VALUE_TYPE_INDEX_Nullable = aVALUE_TYPE_INDEX;

                int? DATE_TYPE_INDEX_Nullable = null;
                if (aDATE_TYPE_INDEX != -1) DATE_TYPE_INDEX_Nullable = aDATE_TYPE_INDEX;

                int? NUMERIC_TYPE_INDEX_Nullable = null;
                if (aNUMERIC_TYPE_INDEX != -1) NUMERIC_TYPE_INDEX_Nullable = aNUMERIC_TYPE_INDEX;

                string VALUE_TO_COMPARE_Nullable = "null";
                if (aVALUE_TO_COMPARE != string.Empty) VALUE_TO_COMPARE_Nullable = aVALUE_TO_COMPARE;


                int? VALUE_TO_COMPARE_BOOL_Nullable = null;
                if (aVALUE_TO_COMPARE_BOOL != null)
                {
                    if (aVALUE_TO_COMPARE_BOOL == false)
                    {
                        VALUE_TO_COMPARE_BOOL_Nullable = 0;
                    }
                    else
                    {
                        VALUE_TO_COMPARE_BOOL_Nullable = 1;
                    }
                }

                int? VALUE_TO_COMPARE_DATE_DAYS_FROM_Nullable = null;
                if (aVALUE_TO_COMPARE_DATE_DAYS_FROM != -1) VALUE_TO_COMPARE_DATE_DAYS_FROM_Nullable = aVALUE_TO_COMPARE_DATE_DAYS_FROM;

                int? VALUE_TO_COMPARE_DATE_DAYS_TO_Nullable = null;
                if (aVALUE_TO_COMPARE_DATE_DAYS_TO != -1) VALUE_TO_COMPARE_DATE_DAYS_TO_Nullable = aVALUE_TO_COMPARE_DATE_DAYS_TO;

                int? VAR1_VARIABLE_INDEX_Nullable = null;
                if (aVAR1_VARIABLE_INDEX != -1) VAR1_VARIABLE_INDEX_Nullable = aVAR1_VARIABLE_INDEX;

                int? VAR1_VERSION_INDEX_Nullable = null;
                if (aVAR1_VERSION_INDEX != -1) VAR1_VERSION_INDEX_Nullable = aVAR1_VERSION_INDEX;

                int? VAR1_HN_RID_Nullable = null;
                if (aVAR1_HN_RID != -1) VAR1_HN_RID_Nullable = aVAR1_HN_RID;

                int? VAR1_CDR_RID_Nullable = null;
                if (aVAR1_CDR_RID != Include.UndefinedCalendarDateRange) VAR1_CDR_RID_Nullable = aVAR1_CDR_RID;

                int? VAR1_VALUE_TYPE_INDEX_Nullable = null;
                if (aVAR1_VALUE_TYPE_INDEX != -1) VAR1_VALUE_TYPE_INDEX_Nullable = aVAR1_VALUE_TYPE_INDEX;

                int? VAR1_TIME_INDEX_Nullable = null;
                if (aVAR1_TIME_INDEX != -1) VAR1_TIME_INDEX_Nullable = aVAR1_TIME_INDEX;

                //int? VAR1_IS_TIME_TOTAL_Nullable = null;
                //if (aVAR1_IS_TIME_TOTAL != -1) VAR1_IS_TIME_TOTAL_Nullable = aVAR1_IS_TIME_TOTAL;

                int? VAR_PERCENTAGE_OPERATOR_INDEX_Nullable = null;
                if (aVAR_PERCENTAGE_OPERATOR_INDEX != -1) VAR_PERCENTAGE_OPERATOR_INDEX_Nullable = aVAR_PERCENTAGE_OPERATOR_INDEX;

                int? VAR2_VARIABLE_INDEX_Nullable = null;
                if (aVAR2_VARIABLE_INDEX != -1) VAR2_VARIABLE_INDEX_Nullable = aVAR2_VARIABLE_INDEX;

                int? VAR2_VERSION_INDEX_Nullable = null;
                if (aVAR2_VERSION_INDEX != -1) VAR2_VERSION_INDEX_Nullable = aVAR2_VERSION_INDEX;

                int? VAR2_HN_RID_Nullable = null;
                if (aVAR2_HN_RID != -1) VAR2_HN_RID_Nullable = aVAR2_HN_RID;

                int? VAR2_CDR_RID_Nullable = null;
                if (aVAR2_CDR_RID != Include.UndefinedCalendarDateRange) VAR2_CDR_RID_Nullable = aVAR2_CDR_RID;

                int? VAR2_VALUE_TYPE_INDEX_Nullable = null;
                if (aVAR2_VALUE_TYPE_INDEX != -1) VAR2_VALUE_TYPE_INDEX_Nullable = aVAR2_VALUE_TYPE_INDEX;

                int? VAR2_TIME_INDEX_Nullable = null;
                if (aVAR2_TIME_INDEX != -1) VAR2_TIME_INDEX_Nullable = aVAR2_TIME_INDEX;

                //int? VAR2_IS_TIME_TOTAL_Nullable = null;
                //if (aVAR2_IS_TIME_TOTAL != -1) VAR2_IS_TIME_TOTAL_Nullable = aVAR2_IS_TIME_TOTAL;

                int? HEADER_HN_RID_Nullable = null;
                if (aHEADER_HN_RID != -1) HEADER_HN_RID_Nullable = aHEADER_HN_RID;

                //int? HEADER_PH_RID_Nullable = null;
                //if (aHEADER_PH_RID != -1) HEADER_PH_RID_Nullable = aHEADER_PH_RID;

                int? SORT_BY_TYPE_INDEX_Nullable = null;
                if (aSORT_BY_TYPE_INDEX != -1) SORT_BY_TYPE_INDEX_Nullable = aSORT_BY_TYPE_INDEX;

                int? SORT_BY_FIELD_INDEX_Nullable = null;
                if (aSORT_BY_FIELD_INDEX != -1) SORT_BY_FIELD_INDEX_Nullable = aSORT_BY_FIELD_INDEX;

                int? LIST_VALUE_CONSTANT_INDEX_Nullable = null;
                if (aLIST_VALUE_CONSTANT_INDEX != -1 && aLIST_VALUE_CONSTANT_INDEX != 0) LIST_VALUE_CONSTANT_INDEX_Nullable = aLIST_VALUE_CONSTANT_INDEX;

                // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
				int? DATE_CDR_RID_Nullable = null;
                if (aDATE_CDR_RID != Include.UndefinedCalendarDateRange) DATE_CDR_RID_Nullable = aDATE_CDR_RID;
				// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

                int newConditionRID = StoredProcedures.MID_FILTER_CONDITION_INSERT.InsertAndReturnRID(_dba,
                                                                                FILTER_RID: aFILTER_RID,
                                                                                SEQ: aSEQ,
                                                                                PARENT_SEQ: aPARENT_SEQ,
                                                                                SIBLING_SEQ: aSIBLING_SEQ,
                                                                                ELEMENT_GROUP_TYPE_INDEX: aELEMENT_GROUP_TYPE_INDEX,
                                                                                LOGIC_INDEX: LOGIC_INDEX_Nullable,
                                                                                FIELD_INDEX: FIELD_INDEX_Nullable,
                                                                                OPERATOR_INDEX: OPERATOR_INDEX_Nullable,
                                                                                VALUE_TYPE_INDEX: VALUE_TYPE_INDEX_Nullable,
                                                                                DATE_TYPE_INDEX: DATE_TYPE_INDEX_Nullable,
                                                                                NUMERIC_TYPE_INDEX: NUMERIC_TYPE_INDEX_Nullable,
                                                                                VALUE_TO_COMPARE: VALUE_TO_COMPARE_Nullable,
                                                                                VALUE_TO_COMPARE_DOUBLE: aVALUE_TO_COMPARE_DOUBLE,
                                                                                VALUE_TO_COMPARE_DOUBLE2: aVALUE_TO_COMPARE_DOUBLE2,
                                                                                VALUE_TO_COMPARE_INT: aVALUE_TO_COMPARE_INT,
                                                                                VALUE_TO_COMPARE_INT2: aVALUE_TO_COMPARE_INT2,
                                                                                VALUE_TO_COMPARE_BOOL: VALUE_TO_COMPARE_BOOL_Nullable,
                                                                                VALUE_TO_COMPARE_DATE_FROM: aVALUE_TO_COMPARE_DATE_FROM,
                                                                                VALUE_TO_COMPARE_DATE_TO: aVALUE_TO_COMPARE_DATE_TO,
                                                                                VALUE_TO_COMPARE_DATE_DAYS_FROM: VALUE_TO_COMPARE_DATE_DAYS_FROM_Nullable,
                                                                                VALUE_TO_COMPARE_DATE_DAYS_TO: VALUE_TO_COMPARE_DATE_DAYS_TO_Nullable,
                                                                                VAR1_VARIABLE_INDEX: VAR1_VARIABLE_INDEX_Nullable,
                                                                                VAR1_VERSION_INDEX: VAR1_VERSION_INDEX_Nullable,
                                                                                VAR1_HN_RID: VAR1_HN_RID_Nullable,
                                                                                VAR1_CDR_RID: VAR1_CDR_RID_Nullable,
                                                                                VAR1_VALUE_TYPE_INDEX: VAR1_VALUE_TYPE_INDEX_Nullable,
                                                                                VAR1_TIME_INDEX: VAR1_TIME_INDEX_Nullable,
                                                                                VAR_PERCENTAGE_OPERATOR_INDEX: VAR_PERCENTAGE_OPERATOR_INDEX_Nullable,
                                                                                VAR2_VARIABLE_INDEX: VAR2_VARIABLE_INDEX_Nullable,
                                                                                VAR2_VERSION_INDEX: VAR2_VERSION_INDEX_Nullable,
                                                                                VAR2_HN_RID: VAR2_HN_RID_Nullable,
                                                                                VAR2_CDR_RID: VAR2_CDR_RID_Nullable,
                                                                                VAR2_VALUE_TYPE_INDEX: VAR2_VALUE_TYPE_INDEX_Nullable,
                                                                                VAR2_TIME_INDEX: VAR2_TIME_INDEX_Nullable,
                                                                                HEADER_HN_RID: HEADER_HN_RID_Nullable,
                                                                                //HEADER_PH_RID: HEADER_PH_RID_Nullable,
                                                                                SORT_BY_TYPE_INDEX: SORT_BY_TYPE_INDEX_Nullable,
                                                                                SORT_BY_FIELD_INDEX: SORT_BY_FIELD_INDEX_Nullable,
                                                                                LIST_VALUE_CONSTANT_INDEX: LIST_VALUE_CONSTANT_INDEX_Nullable,
                                                                                DATE_CDR_RID: DATE_CDR_RID_Nullable   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                                                                                );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
                return newConditionRID;
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }

        public void InsertConditionForAuditDefault(int aFILTER_RID)
        {
            try
            {
                _dba.OpenUpdateConnection();


                int rowCount = StoredProcedures.MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT.Insert(_dba, FILTER_RID: aFILTER_RID);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }

        public void InsertListValues(DataTable dtListValues)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_FILTER_CONDITION_LIST_VALUES_INSERT.Insert(_dba, dtListValues);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        public void DeleteFilterConditions(int filterRID)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_FILTER_CONDITION_DELETE.Delete(_dba, filterRID);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        public DataTable FilterRead(filterTypes filterType, eProfileType filterProfileType, ArrayList aUserRIDList)
        {
            try
            {
                DataTable dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));
                foreach (int userRID in aUserRIDList)
                {
                    //ensure userRIDs are distinct, and only added to the datatable one time
                    if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = userRID;
                        dtUserList.Rows.Add(dr);
                    }
                }

                return StoredProcedures.MID_FILTER_READ_FROM_USERS.Read(_dba,
                                                                        FILTER_TYPE: filterType.dbIndex,
                                                                        FILTER_PROFILE_TYPE: (int)filterProfileType,
                                                                        USER_RID_LIST: dtUserList
                                                                        );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //Begin TT#1388-MD -jsobek -Product Filter
        public DataTable FilterReadForUser(filterTypes filterType, ArrayList aUserRIDList)
        {
            try
            {
                DataTable dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));
                foreach (int userRID in aUserRIDList)
                {
                    //ensure userRIDs are distinct, and only added to the datatable one time
                    if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = userRID;
                        dtUserList.Rows.Add(dr);
                    }
                }

                return StoredProcedures.MID_FILTER_READ_FOR_USER.Read(_dba,
                                                                        FILTER_TYPE: filterType.dbIndex,
                                                                        USER_RID_LIST: dtUserList
                                                                        );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1388-MD -jsobek -Product Filter

        public DataTable FilterReadConditions(int filterRID)
        {
            try
            {
          

                return StoredProcedures.MID_FILTER_CONDITION_READ.Read(_dba,
                                                                        FILTER_RID: filterRID
                                                                        );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public DataTable FilterReadListValues(int filterRID)
        {
            try
            {


                return StoredProcedures.MID_FILTER_CONDITION_LIST_VALUES_READ.Read(_dba,
                                                                        FILTER_RID: filterRID
                                                                        );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public DataTable FilterReadParent(filterTypes filterType, eProfileType filterProfileType, ArrayList aUserRIDList)
		{
			try
			{
                DataTable dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));
                foreach (int userRID in aUserRIDList)
                {
                    //ensure userRIDs are distinct, and only added to the datatable one time
                    if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = userRID;
                        dtUserList.Rows.Add(dr);
                    }
                }
                return StoredProcedures.MID_FILTER_READ_PARENT_FROM_USERS.Read(_dba,
                                                                       FILTER_TYPE: filterType.dbIndex,
                                                                       FILTER_PROFILE_TYPE: (int)filterProfileType,
                                                                       USER_RID_LIST: dtUserList
                                                                       );
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        public DataTable FilterRead(int filterRID)
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ.Read(_dba, FILTER_RID: filterRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void FilterDelete(int filterRID)
        {
            try
            {
                StoredProcedures.MID_FILTER_DELETE.Delete(_dba, FILTER_RID: filterRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //Begin TT#1313-MD -jsobek -Header Filters


        public bool DoesFilterProcedureExist(int filterRID, filterTypes filterType)
        {
            try
            {
                string procedureName = FilterCommon.BuildFilterProcedureName(filterRID, filterType);
                DataTable dt = StoredProcedures.MID_DOES_WF_PROCEDURE_EXIST.Read(_dba, WF_NAME: procedureName);
                bool exists = false;
                if (dt.Rows.Count > 0)
                {
                    if ((int)dt.Rows[0]["DOES_WF_EXIST"] == 1)
                    {
                        exists = true;
                    }
                }
                return exists;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RemoveFilterProcedure(int filterRID, filterTypes filterType)
        {
            try
            {
                if (DoesFilterProcedureExist(filterRID, filterType))
                {
                    //StoredProcedures.MID_FILTER_DELETE.Delete(_dba, FILTER_RID: filterRID);
                    string procedureName = FilterCommon.BuildFilterProcedureName(filterRID, filterType);
                    string sSQL = "DROP PROCEDURE " + procedureName;

                    try
                    {
                        _dba.OpenUpdateConnection();
                        _dba.ExecuteNonQuery(sSQL);
                        _dba.CommitData();
                        _dba.CloseUpdateConnection();
                    }
			        catch (Exception ex)
			        {
				        _dba.RollBack();
				        _dba.CloseUpdateConnection();
				        throw;
			        }

                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int WorkspaceCurrentFilter_Read(int aUserRID, eWorkspaceType aWorkspaceType)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_USER_CURRENT_WORKSPACE_FILTER_READ.Read(_dba,
                                                                                            USER_RID: aUserRID,
                                                                                            WORKSPACE_TYPE: (int)aWorkspaceType);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["FILTER_RID"]);
                }
                else
                {
                    return Include.NoRID;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void WorkspaceCurrentFilter_Update(int aUserRID, eWorkspaceType aWorkspaceType, int aFilterRID)
        {
            try
            {
                _dba.OpenUpdateConnection();

                try
                {
                    WorkspaceCurrentFilter_Delete(aUserRID, aWorkspaceType);
                    if (aFilterRID != Include.NoRID)
                    {
                        DataTable dtFilter = FilterRead(aFilterRID); // Filter may have been deleted
                        if (dtFilter.Rows.Count > 0)
                        {
                            WorkspaceCurrentFilter_Insert(aUserRID, aWorkspaceType, aFilterRID);
                        }
                    }
                    _dba.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    _dba.RollBack();
                    throw;
                }
                finally
                {
                    _dba.CloseUpdateConnection();
                }

                return;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void WorkspaceCurrentFilter_Delete(int aUserRID, eWorkspaceType aWorkspaceType)
        {
            try
            {
                StoredProcedures.MID_USER_CURRENT_WORKSPACE_FILTER_DELETE.Delete(_dba,
                                                                          USER_RID: aUserRID,
                                                                          WORKSPACE_TYPE: (int)aWorkspaceType
                                                                          );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		// Begin TT#1362-MD - stodd - Header filter InUse is not returning any filters being InUse even though they are
        public void WorkspaceCurrentFilter_DeleteAll(int aFilterRID)
        {
            try
            {
                StoredProcedures.MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL.Delete(_dba,
                                                                          FILTER_RID: aFilterRID
                                                                          );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1362-MD - stodd - Header filter InUse is not returning any filters being InUse even though they are
		
        private void WorkspaceCurrentFilter_Insert(int aUserRID, eWorkspaceType aWorkspaceType, int aFilterRID)
        {
            try
            {
                StoredProcedures.MID_USER_CURRENT_WORKSPACE_FILTER_INSERT.Insert(_dba,
                                                                          USER_RID: aUserRID,
                                                                          WORKSPACE_TYPE: (int)aWorkspaceType,
                                                                          FILTER_RID: aFilterRID
                                                                          );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Reads users that have a certain node in their filter for AWS views - for delete preview
        /// </summary>
        /// <param name="aUserRID"></param>
        /// <param name="aWorkspaceType"></param>
        /// <returns></returns>
        public DataTable WorkspaceFilterNodeRead(int aHnRID, eWorkspaceType aWorkspaceType)
        {
            try
            {
                int filterType = filterTypes.HeaderFilter.dbIndex;
                if (aWorkspaceType == eWorkspaceType.AssortmentWorkspace)
                {
                    filterType = filterTypes.AssortmentFilter.dbIndex;
                }

                return StoredProcedures.MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE.Read(_dba,
                                                                                            HEADER_HN_RID: aHnRID,
                                                                                            FILTER_TYPE: filterType);

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

        //Begin TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
        public DataTable ReadFiltersForType(filterTypes filterType)
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ_FROM_TYPE.Read(_dba, FILTER_TYPE: filterType.dbIndex);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions

        // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
        public DataTable ReadFiltersForTypeAndFieldName(filterTypes filterType, string fieldName)
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME.Read(_dba, FILTER_TYPE: filterType.dbIndex, FIELD_NAME: fieldName);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

        public DataTable ReadFiltersForStoreGroupRefresh(DataTable dtFieldsChanged, DataTable dtCharsChanged)
        {
            try
            {
                return StoredProcedures.MID_FILTER_READ_FOR_REFRESH.Read(_dba, FIELD_CHANGED_LIST: dtFieldsChanged, CHAR_CHANGED_LIST: dtCharsChanged);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public DataTable HeaderCharacteristicsGetValuesForGroup(int aHCG_RID)
        {
            try
            {
                return StoredProcedures.MID_HEADER_CHAR_READ_FOR_GROUP.Read(_dba, HCG_RID: aHCG_RID);            
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
		//public DataTable StoreCharacteristicsGetValuesForGroup(int aSCG_RID)
        public DataTable StoreCharacteristicsGetValuesForGroup(int aSCG_RID, string sortString)
		// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
        {
            try
            {
			    // Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
				//return StoredProcedures.MID_STORE_CHAR_READ_FOR_GROUP.Read(_dba, SCG_RID: aSCG_RID);
                return StoredProcedures.MID_STORE_CHAR_READ_FOR_GROUP.Read(_dba, SCG_RID: aSCG_RID, SORT_STRING: sortString);
				// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        
        /// <summary>
        /// returns the store rids as well as the values for the characteristic
        /// </summary>
        /// <param name="aSCG_RID"></param>
        /// <returns></returns>
        public DataTable StoreCharacteristicsGetValuesForFilter(int aSCG_RID)
        {
            try
            {
                return StoredProcedures.MID_STORE_CHAR_READ_FOR_FILTER.Read(_dba, SCG_RID: aSCG_RID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        public int InsertFilterForDynamicStoreGroup(int scgRid, string scgId, int userRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                int newFilterRID = StoredProcedures.MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE.InsertAndReturnRID(_dba, SCG_RID: scgRid, SCG_ID: scgId, USER_RID: userRid);

                _dba.CommitData();
                _dba.CloseUpdateConnection();

                return newFilterRID;
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }

        public int InsertFilterForAllStoresAttribute()
        {
            try
            {
                _dba.OpenUpdateConnection();
                int newFilterRID = StoredProcedures.MID_FILTER_INSERT_ALL_STORES_SET.InsertAndReturnRID(_dba);

                _dba.CommitData();
                _dba.CloseUpdateConnection();

                return newFilterRID;
            }
            catch (Exception ex)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                throw;
            }
        }
        

        //private static DataTable dtHeaderTypes;
        //public static DataTable HeaderTypesGetDataTable()
        //{
        //    if (dtHeaderTypes == null)
        //    {
        //        dtHeaderTypes = new DataTable();
        //        dtHeaderTypes.Columns.Add("FIELD_NAME");
        //        dtHeaderTypes.Columns.Add("FIELD_INDEX", typeof(int));

        //        DataRow dr1 = dtHeaderTypes.NewRow();
        //        dr1["FIELD_NAME"] = "ASN";
        //        dr1["FIELD_INDEX"] = 800731;
        //        dtHeaderTypes.Rows.Add(dr1);

        //        DataRow dr2 = dtHeaderTypes.NewRow();
        //        dr2["FIELD_NAME"] = "Drop Ship";
        //        dr2["FIELD_INDEX"] = 800733;
        //        dtHeaderTypes.Rows.Add(dr2);

        //        DataRow dr3 = dtHeaderTypes.NewRow();
        //        dr3["FIELD_NAME"] = "Dummy";
        //        dr3["FIELD_INDEX"] = 800732;
        //        dtHeaderTypes.Rows.Add(dr3);

        //        DataRow dr4 = dtHeaderTypes.NewRow();
        //        dr4["FIELD_NAME"] = "Multi-Header";
        //        dr4["FIELD_INDEX"] = 800734;
        //        dtHeaderTypes.Rows.Add(dr4);

        //        DataRow dr5 = dtHeaderTypes.NewRow();
        //        dr5["FIELD_NAME"] = "Purchase Order";
        //        dr5["FIELD_INDEX"] = 800738;
        //        dtHeaderTypes.Rows.Add(dr5);

        //        DataRow dr6 = dtHeaderTypes.NewRow();
        //        dr6["FIELD_NAME"] = "Receipt";
        //        dr6["FIELD_INDEX"] = 800730;
        //        dtHeaderTypes.Rows.Add(dr6);

        //        DataRow dr7 = dtHeaderTypes.NewRow();
        //        dr7["FIELD_NAME"] = "Reserve";
        //        dr7["FIELD_INDEX"] = 800735;
        //        dtHeaderTypes.Rows.Add(dr7);

        //        DataRow dr8 = dtHeaderTypes.NewRow();
        //        dr8["FIELD_NAME"] = "VSW";
        //        dr8["FIELD_INDEX"] = 800741;
        //        dtHeaderTypes.Rows.Add(dr8);

        //        DataRow dr9 = dtHeaderTypes.NewRow();
        //        dr9["FIELD_NAME"] = "Workup Total Buy";
        //        dr9["FIELD_INDEX"] = 800737;
        //        dtHeaderTypes.Rows.Add(dr9);


        //        GlobalOptions opts = new GlobalOptions();
        //        DataTable dt = opts.GetGlobalOptions();
        //        DataRow dr = dt.Rows[0];


        //        // load header type
        //        eHeaderType type;
        //        //string text = string.Empty;
        //        //bool selected = false;
        //        //bool multiChecked = false;
        //        DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));


        //        //bool phRemoved = false, asrtRemoved = false;
        //        for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
        //        {
        //            DataRow dRow = dtTypes.Rows[i];

        //            if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
        //            {
        //                if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
        //                {
        //                    dtTypes.Rows.Remove(dRow);
        //                    //asrtRemoved = true;
        //                }
        //                else if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
        //                {
        //                    dtTypes.Rows.Remove(dRow);
        //                    //phRemoved = true;
        //                }
        //            }

        //            type = (eHeaderType)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
        //            //text = Convert.ToString(dRow["TEXT_VALUE"], CultureInfo.CurrentUICulture);
        //            // if size, use all statuses
        //            if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
        //            {
        //                //lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
        //            }
        //            else
        //            {
        //                // remove all size statuses
        //                if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type, CultureInfo.CurrentUICulture)))
        //                {
        //                    //lstTypes.Items.Add(new MIDListBoxItem(text, type), selected);
        //                }
        //                else
        //                {
        //                    dtTypes.Rows.Remove(dRow);
        //                }
        //            }
        //            //if (asrtRemoved && phRemoved)
        //            //{
        //            //    break;
        //            //}
        //        }
        //    }
        //    return dtHeaderTypes;
        //}

        //private static DataTable dtHeaderStatuses;
        //public static DataTable HeaderStatusesGetDataTable()
        //{
        //    if (dtHeaderStatuses == null)
        //    {
        //        dtHeaderStatuses = new DataTable();
        //        dtHeaderStatuses.Columns.Add("FIELD_NAME");
        //        dtHeaderStatuses.Columns.Add("FIELD_INDEX", typeof(int));

        //        DataRow dr1 = dtHeaderStatuses.NewRow();
        //        dr1["FIELD_NAME"] = "All In balance";
        //        dr1["FIELD_INDEX"] = 802708;
        //        dtHeaderStatuses.Rows.Add(dr1);

        //        DataRow dr2 = dtHeaderStatuses.NewRow();
        //        dr2["FIELD_NAME"] = "Allocated In Balance";
        //        dr2["FIELD_INDEX"] = 802706;
        //        dtHeaderStatuses.Rows.Add(dr2);

        //        DataRow dr3 = dtHeaderStatuses.NewRow();
        //        dr3["FIELD_NAME"] = "Allocated Out of Balance";
        //        dr3["FIELD_INDEX"] = 802705;
        //        dtHeaderStatuses.Rows.Add(dr3);

        //        DataRow dr4 = dtHeaderStatuses.NewRow();
        //        dr4["FIELD_NAME"] = "Allocation Started";
        //        dr4["FIELD_INDEX"] = 802711;
        //        dtHeaderStatuses.Rows.Add(dr4);

        //        DataRow dr5 = dtHeaderStatuses.NewRow();
        //        dr5["FIELD_NAME"] = "In use by Multi-Header";
        //        dr5["FIELD_INDEX"] = 802702;
        //        dtHeaderStatuses.Rows.Add(dr5);

        //        DataRow dr6 = dtHeaderStatuses.NewRow();
        //        dr6["FIELD_NAME"] = "Pre-Size In Balance";
        //        dr6["FIELD_INDEX"] = 802704;
        //        dtHeaderStatuses.Rows.Add(dr6);

        //        DataRow dr7 = dtHeaderStatuses.NewRow();
        //        dr7["FIELD_NAME"] = "Pre-Size Out of Balance";
        //        dr7["FIELD_INDEX"] = 802703;
        //        dtHeaderStatuses.Rows.Add(dr7);

        //        DataRow dr8 = dtHeaderStatuses.NewRow();
        //        dr8["FIELD_NAME"] = "Received In Balance";
        //        dr8["FIELD_INDEX"] = 802701;
        //        dtHeaderStatuses.Rows.Add(dr8);

        //        DataRow dr9 = dtHeaderStatuses.NewRow();
        //        dr9["FIELD_NAME"] = "Received Out of Balance";
        //        dr9["FIELD_INDEX"] = 802700;
        //        dtHeaderStatuses.Rows.Add(dr9);

        //        DataRow dr10 = dtHeaderStatuses.NewRow();
        //        dr10["FIELD_NAME"] = "Released Approved";
        //        dr10["FIELD_INDEX"] = 802710;
        //        dtHeaderStatuses.Rows.Add(dr10);

        //        DataRow dr11 = dtHeaderStatuses.NewRow();
        //        dr11["FIELD_NAME"] = "Released";
        //        dr11["FIELD_INDEX"] = 802709;
        //        dtHeaderStatuses.Rows.Add(dr11);

        //        DataRow dr12 = dtHeaderStatuses.NewRow();
        //        dr12["FIELD_NAME"] = "Sizes Out of Balance";
        //        dr12["FIELD_INDEX"] = 802707;
        //        dtHeaderStatuses.Rows.Add(dr12);

        //    }
        //    return dtHeaderStatuses;
        //}
        //public static string HeaderStatusGetNameFromIndex(int fieldIndex)
        //{
        //    if (dtHeaderStatuses == null)
        //    {
        //        HeaderStatusesGetDataTable();
        //    }
        //    DataRow[] drFind = dtHeaderStatuses.Select("FIELD_INDEX=" + fieldIndex.ToString());
        //    return (string)drFind[0]["FIELD_NAME"];
        //}


	}


}
