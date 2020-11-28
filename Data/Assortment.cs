using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;   // MID Track 3994 Performance
using System.Runtime.Serialization; // MID Track 3994 Performance
using System.Runtime.Serialization.Formatters.Binary; // MID Track 3994 Performance

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for Allocation Header.
	/// </summary>
	public partial class Header : DataLayer
	{
		
		//===============================
		// HEADER ASSORTMENT 
		//===============================

		//========
		// Reads
		//========
		public DataTable GetAssortmentProperties(int headerRid)
		{
			try
			{
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
				// Begin TT32 - stodd - assortment
                //string SQLCommand = "select HDR_RID, "
                //    + "COALESCE(RESERVE,0) as RESERVE, "
                //    + "COALESCE(RESERVE_TYPE_IND,0) as RESERVE_TYPE_IND, "
                //    + "COALESCE(SG_RID,-1) as SG_RID, "
                //    + "COALESCE(VARIABLE_TYPE,0) as VARIABLE_TYPE, "
                //    + "VARIABLE_NUMBER, "
                //    + "INCL_ONHAND, INCL_INTRANSIT, INCL_SIMILAR_STORES, INCL_COMMITTED, AVERAGE_BY, "
                //    + "COALESCE(GRADE_BOUNDARY_IND,0) as GRADE_BOUNDARY_IND, "
                //    + "CDR_RID, "
                //    + "COALESCE(ANCHOR_HN_RID,-1) as ANCHOR_HN_RID, "
                //    + "COALESCE(USER_RID,1) as USER_RID, "
                //    + "COALESCE(ANCHOR_HN_RID,-1) as ANCHOR_HN_RID, "
                //    + "LAST_PROCESS_DATETIME "
                //    + "from ASSORTMENT_PROPERTIES "
                //    + "where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
                //return _dba.ExecuteSQLQuery(SQLCommand, "GET_ASSORTMENT_TABLE");
				// End TT32 - stodd - assortment

                //DataSet dsValues;
                //MIDDbParameter[] InParams = { new MIDDbParameter("@headerRID",  headerRid, eDbType.Int,eParameterDirection.Input) };

                //dsValues = _dba.ExecuteDataSetQuery("MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER", "GET_ASSORTMENT_TABLE", InParams);

                //dsValues.Tables[0].TableName = "GET_ASSORTMENT_TABLE";
                //return dsValues.Tables[0];
                return StoredProcedures.MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER.Read(_dba, headerRID: headerRid);
                //End TT#827-MD -jsobek -Allocation Reviews Performance
			}
			catch
			{
				throw;
			}
		}
        //Begin TT#827-MD -jsobek -Allocation Reviews Performance

        /// <summary>
        /// Gets assortment properties and placeholder assortment data in one dataset
        /// </summary>
        /// <param name="headerRid"></param>
        /// <returns></returns>
        public DataSet GetAssortmentDataForHeader(int headerRid, int placeholderRID, bool getAssortmentProperties, bool getPlaceholderData)
        {
            try
            {


                //DataSet dsValues;
                //MIDDbParameter[] InParams = { new MIDDbParameter("@headerRID", headerRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@placeholderRID", placeholderRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@getAssortmentProperties", getAssortmentProperties, eDbType.Bit, eParameterDirection.Input),
                //                              new MIDDbParameter("@getPlaceholderData", getPlaceholderData, eDbType.Bit, eParameterDirection.Input) 
                //                            };

                //dsValues = _dba.ExecuteDataSetQuery("MID_GET_ASSORTMENT_DATA_FOR_HEADER", "GET_ASSORTMENT_DATA", InParams);

                //dsValues.Tables[0].TableName = "AssortmentProperties";
                //dsValues.Tables[1].TableName = "AssortmentPropertiesForPlaceHolder";
                //dsValues.Tables[2].TableName = "HeadersAttachedToPlaceholder";
                //return dsValues;
                
                return StoredProcedures.MID_GET_ASSORTMENT_DATA_FOR_HEADER.ReadAsDataSet(_dba,
                                                                                       headerRID: headerRid,
                                                                                       placeholderRID: placeholderRID,
                                                                                       getAssortmentProperties: Include.ConvertBoolToInt(getAssortmentProperties),
                                                                                       getPlaceholderData: Include.ConvertBoolToInt(getPlaceholderData)
                                                                                       );
            }
            catch
            {
                throw;
            }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

		// Begin TT#2 - stodd - assortment. Used in assortment explorer
		public DataTable AssortmentProperties_ReadAll()
		{
			//string SQLCommand;

			try
			{
                //SQLCommand = "SELECT * " +
                //    @" FROM ASSORTMENT_PROPERTIES ha, HEADER hd " +
                //    @" WHERE hd.HDR_RID = ha.HDR_RID" +
                //    @" AND hd.DISPLAY_TYPE = " + Convert.ToString((int)eHeaderType.Assortment, CultureInfo.CurrentUICulture);

                //return _dba.ExecuteSQLQuery(SQLCommand, "ASSORTMENT_PROPERTIES");
                return StoredProcedures.MID_ASSORTMENT_PROPERTIES_READ_ALL.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

		public int HeaderAssortment_GetKey(string assortmentName)
		{
			//string SQLCommand;
			//MIDDbParameter[] inParams;
			DataTable dt;

			try
			{
                ////Begin TT#827-MD -jsobek -Allocation Reviews Performance
                ////SQLCommand = "SELECT * FROM HEADER WHERE HDR_ID = @HDR_ID";
                //SQLCommand = "SELECT HDR_RID FROM HEADER WHERE HDR_ID = @HDR_ID";
                ////End TT#827-MD -jsobek -Allocation Reviews Performance

                //inParams = new MIDDbParameter[1];
                //inParams[0] = new MIDDbParameter("@HDR_ID", assortmentName);
                //inParams[0].DbType = eDbType.VarChar;
                //inParams[0].Direction = eParameterDirection.Input;

                //dt = _dba.ExecuteSQLQuery(SQLCommand, "HeaderAssortment", inParams);
                dt = StoredProcedures.MID_HEADER_READ_RID_FROM_ID.Read(_dba, HDR_ID: assortmentName);

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["HDR_RID"], CultureInfo.CurrentUICulture));
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

		// BEGIN TT#219-MD - stodd - Spread Average  - 
		public DataTable GetHeadersAttachedToPlaceholder(int placeholderRid)
		{
			//string SQLCommand;
			//MIDDbParameter[] inParams;

			try
			{
                //SQLCommand = "SELECT HDR_RID FROM HEADER WHERE PLACEHOLDER_RID = @PH_RID";

                //inParams = new MIDDbParameter[1];
                //inParams[0] = new MIDDbParameter("@PH_RID", placeholderRid);
                //inParams[0].DbType = eDbType.VarChar;
                //inParams[0].Direction = eParameterDirection.Input;

                //return _dba.ExecuteSQLQuery(SQLCommand, "GetHeadersAttachedToPlaceholder", inParams);
                return StoredProcedures.MID_HEADER_READ_RID_FROM_PLACEHOLDER.Read(_dba, PLACEHOLDER_RID: placeholderRid);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#219-MD 

		public void AssortmentHeader_Update(int assortmentRID, string assortmentName)
		{
			//string SQLCommand;
			//MIDDbParameter[] inParams;

			try
			{
                //SQLCommand = "UPDATE HEADER SET HDR_ID = @HDR_ID WHERE HDR_RID = @HDR_RID";

                //inParams = new MIDDbParameter[2];
                //inParams[0] = new MIDDbParameter("@HDR_RID", assortmentRID);
                //inParams[0].DbType = eDbType.Int;
                //inParams[0].Direction = eParameterDirection.Input;
                //inParams[1] = new MIDDbParameter("@HDR_ID", assortmentName);
                //inParams[1].DbType = eDbType.VarChar;
                //inParams[1].Direction = eParameterDirection.Input;

                //_dba.ExecuteNonQuery(SQLCommand, inParams);
                StoredProcedures.MID_HEADER_UPDATE_ID.Update(_dba,
                                                             HDR_RID: assortmentRID,
                                                             HDR_ID: assortmentName
                                                             );
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// End TT#2 - stodd - assortment. Used in assortment explorer

		public DataTable GetAssortmentPropertiesStoreGrades(int headerRid)
		{
			try
			{
                //string SQLCommand = "select HDR_RID, STORE_GRADE_SEQ, COALESCE(BOUNDARY_UNITS,0) as BOUNDARY_UNITS," +
                //    " COALESCE(BOUNDARY_INDEX,0) as BOUNDARY_INDEX, GRADE_CODE from ASSORTMENT_PROPERTIES_STORE_GRADE" +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture) +
                //    " order by STORE_GRADE_SEQ";
                //return _dba.ExecuteSQLQuery(SQLCommand, "GET_ASSORTMENT_GRADE_TABLE");
                return StoredProcedures.MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ.Read(_dba, HDR_RID: headerRid);
			}
			catch
			{
				throw;
			}
		}

        public DataTable GetAssortmentPropertiesBasis(int headerRid)
		{
			try
			{
                //string SQLCommand = "select * from ASSORTMENT_PROPERTIES_BASIS" +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture) +
                //    " order by BASIS_SEQ";
                //return _dba.ExecuteSQLQuery(SQLCommand, "GET_ASSORTMENT_PROPERTIES_BASIS_TABLE");
                return StoredProcedures.MID_ASSORTMENT_PROPERTIES_BASIS_READ.Read(_dba, HDR_RID: headerRid);
			}
			catch
			{
				throw;
			}
		}

		

		public DataTable GetAssortmentStoreSummary(int headerRid)
		{
			DataTable dtStoreSummary = MIDEnvironment.CreateDataTable();
			try
			{
                //string SQLCommand = "select * from ASSORTMENT_STORE_SUMMARY" +
                //    " where HDR_RID = " + .ToString(CultureInfo.CurrentUICulture) +
                //    " ORDER BY VARIABLE_NUMBER";
                //dtStoreSummary = _dba.ExecuteSQLQuery(SQLCommand, "GetTotalAssortment");
                dtStoreSummary = StoredProcedures.MID_ASSORTMENT_STORE_SUMMARY_READ_ALL.Read(_dba, HDR_RID: headerRid);
				DataColumn dc = new DataColumn("SGL_RID", typeof(int));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				dc = new DataColumn("STORE_GRADE_INDEX", typeof(float));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				dc = new DataColumn("AVERAGE_STORE", typeof(float));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				dc = new DataColumn("STORE_GRADE_BOUNDARY", typeof(int));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				return dtStoreSummary;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns an empty DataTable of ASSORTMENT_STORE_SUMMARY.
		/// </summary>
		/// <returns></returns>
		public DataTable GetEmptyAssortmentStoreSummaryDataTable()
		{
			try
			{
                //string SQLCommand = "SELECT TOP 1 * FROM ASSORTMENT_STORE_SUMMARY";

                //DataTable dt = _dba.ExecuteSQLQuery(SQLCommand, "GetEmptyAssortmentStoreSummaryDataTable");
                DataTable dt = (StoredProcedures.MID_ASSORTMENT_STORE_SUMMARY_READ.Read(_dba));
				DataTable dtStoreSummary = dt.Clone();
				DataColumn dc = new DataColumn("SGL_RID", typeof(int));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				dc = new DataColumn("STORE_GRADE_INDEX", typeof(float));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				dc = new DataColumn("AVERAGE_STORE", typeof(float));
				dc.DefaultValue = 0;
				dtStoreSummary.Columns.Add(dc);
				return dtStoreSummary;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetAssormentComponents(int headerRid)
		{
			try
			{
                //string SQLCommand = "SELECT * FROM HEADER WHERE ASRT_RID = " + headerRid;
                //return _dba.ExecuteSQLQuery(SQLCommand, "GetAssortmentComponents");
                return StoredProcedures.MID_HEADER_READ_ALL_ASSORTMENT.Read(_dba, ASRT_RID: headerRid);
			}
			catch
			{
				throw;
			}
		}

		//==========
		// Deletes
		//==========
        // Begin TT#1320 - stodd - Cannot Delete Assortment from Assortment Explorer - 
        //public void DeleteEntireAssortment(int headerRID, DataTable aDtPlaceholders)
        //{
        //    try
        //    {
        //        // Remove assortment connections from real headers  
        //        //string SQLCommand = "update HEADER set ASRT_RID = null, PLACEHOLDER_RID = null " +
        //        //                    "where ASRT_RID = " + headerRID.ToString() + " and DISPLAY_TYPE != 800740";
        //        //_dba.ExecuteNonQuery(SQLCommand);
        //        StoredProcedures.MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE.Update(_dba, HDR_RID: headerRID);
        //        // delete placeholder and assortment headers
        //        foreach (DataRow row in aDtPlaceholders.Rows)
        //        {
        //            if (UpdateHeaderBulkColor(Convert.ToInt32(row["HDR_RID"], CultureInfo.CurrentUICulture)))
        //            {
        //                DeleteHeader(Convert.ToInt32(row["HDR_RID"], CultureInfo.CurrentUICulture));
        //            }
        //        }
        //        DeleteHeader(headerRID);    // assortment
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#1320 - stodd - Cannot Delete Assortment from Assortment Explorer - 

        // BEGIN TT#488-MD - Stodd - Group Allocation
        /// <summary>
        /// Deletes assortment or Gropup Allocation and detaches any "real" headers.
        /// Also deletes any placeholders.
        /// </summary>
        /// <param name="headerId"></param>
        public int DeleteGroupAllocation(string headerId)	// T#1091-MD - STodd - Add Header Purge Criteria group allocation
        {
            try
            {
                int headerRid = GetHeaderRID(headerId);
                return DeleteAssortment(headerRid);			// TT#1091-MD - STodd - Add Header Purge Criteria group allocation
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Deletes assortment or Gropup Allocation and detaches any "real" headers.
        /// Also deletes any placeholders.
        /// </summary>
        /// <param name="headerId"></param>
        public int DeleteGroupAllocation(int headerRid)	// TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        {
            try
            {
                return DeleteAssortment(headerRid);			// TT#1091-MD - STodd - Add Header Purge Criteria group allocation
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Deletes assortment or Gropup Allocation and detaches any "real" headers.
        /// Also deletes any placeholders.
        /// </summary>
        /// <param name="headerId"></param>
        public int DeleteAssortment(string headerId)	// TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        {
            try
            {
                int headerRid = GetHeaderRID(headerId);
                return DeleteAssortment(headerRid);		// TT#1091-MD - STodd - Add Header Purge Criteria group allocation
            }
            catch
            {
                throw;
            }

        }

        // BEGIN TT#488-MD - Stodd - Group Allocation
        /// <summary>
        /// Deletes assortment or Gropup Allocation and detaches any "real" headers.
        /// Also deletes any placeholders.
        /// </summary>
        /// <param name="headerRID"></param>
        //Begin TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        public int DeleteAssortment(int headerRID)
        {
            int hdrCnt = 0;
            try
            {
                if (UpdateRuleMethodStatus(headerRID))
                {
                    //Begin TT#1268-MD -jsobek -5.4 Merge
                    //MIDDbParameter[] InParams = { new MIDDbParameter("@ASRT_RID", headerRID, eDbType.Int, eParameterDirection.Input) };

                    //MIDDbParameter[] OutParams = { new MIDDbParameter("@HEADER_DELETE_COUNT ", DBNull.Value, eDbType.Int, eParameterDirection.Output) };

                    //return _dba.ExecuteStoredProcedure("SP_MID_MULTI_HEADER_DELETE", InParams, OutParams);
                    //hdrCnt = _dba.ExecuteStoredProcedure("SP_MID_ASSORTMENT_HEADER_DELETE", InParams, OutParams);

                    hdrCnt = StoredProcedures.MID_ASSORTMENT_HEADER_DELETE.Delete(_dba, ASRT_RID: headerRID); //5.4 Update
                    //End TT#1268-MD -jsobek -5.4 Merge

                }
                return hdrCnt;
            }
            catch
            {
                throw;
            }
        }
        //End TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        // END TT#488-MD - Stodd - Group Allocation

        private bool UpdateHeaderBulkColor(int headerRID)
        {
            try
            {
                ////string updateCommand = "update HEADER_BULK_COLOR set ASRT_BC_RID = null where ASRT_BC_RID in " +
                ////    "(select HDR_BC_RID FROM HEADER_BULK_COLOR where HDR_RID = " + headerRID.ToString(CultureInfo.CurrentUICulture) + ")";
                ////return (_dba.ExecuteNonQuery(updateCommand) >= 0);
                return (StoredProcedures.MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE.Update(_dba, HDR_RID: headerRID) >= 0);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetPlaceholdersForAssortment(int headerRID)
        {
            try
            {
                // get placeholder HDR_RIDs for assortment header 
                // Begin TT#2 - stodd - assortment
                //string SQLCommand = "select HDR_RID, HDR_ID, STYLE_HNRID from HEADER where ASRT_RID = " + headerRID.ToString() + // placedholders
                //    " and DISPLAY_TYPE = 800740";
                //// End TT#2 - stodd - assortment
                //return _dba.ExecuteSQLQuery(SQLCommand, "GetPlaceholdersForAssortment");
                return StoredProcedures.MID_HEADER_READ_ASSORTMENT.Read(_dba, ASRT_RID: headerRID);
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteAllAssortmentPropertiesData(int headerRid)
        {
            try
            {
                if (DeleteAssortmentPropertiesStoreGrades(headerRid) &&
                    DeleteAssortmentPropertiesBasis(headerRid) &&
                    DeleteAssortmentProperties(headerRid))
                {
                    return true;
                }
                else
                {
                    return false;
                }  
            }
            catch
            {
                throw;
            }
        }

		public bool DeleteAssortmentProperties(int headerRid)
		{
			try
			{
                //string deleteCommand = "delete ASSORTMENT_PROPERTIES " +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
                //return (_dba.ExecuteNonQuery(deleteCommand) >= 0);
                return (StoredProcedures.MID_ASSORTMENT_PROPERTIES_DELETE.Delete(_dba, HDR_RID: headerRid) >= 0);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteAssortmentPropertiesStoreGrades(int headerRid)
		{
			try
			{
                //string deleteCommand = "delete ASSORTMENT_PROPERTIES_STORE_GRADE " +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
                //return (_dba.ExecuteNonQuery(deleteCommand) >= 0);
                return (StoredProcedures.MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE.Delete(_dba, HDR_RID: headerRid) >= 0);
			}
			catch
			{
				throw;
			}
		}

        public bool DeleteAssortmentPropertiesBasis(int headerRid)
		{
			try
			{
                //string deleteCommand = "delete ASSORTMENT_PROPERTIES_BASIS " +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
                //return (_dba.ExecuteNonQuery(deleteCommand) >= 0);

                return (StoredProcedures.MID_ASSORTMENT_PROPERTIES_BASIS_DELETE.Delete(_dba, HDR_RID: headerRid) >= 0);
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteAssortmentStoreSummary(int headerRid)
		{
			try
			{
                //string deleteCommand = "delete ASSORTMENT_STORE_SUMMARY " +
                //    " where HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
                //return (_dba.ExecuteNonQuery(deleteCommand) >= 0);
                return (StoredProcedures.MID_ASSORTMENT_STORE_SUMMARY_DELETE.Delete(_dba, HDR_RID: headerRid) >= 0);
			}
			catch
			{
				throw;
			}
		}

		//==========
		// Writes
		//==========

		public bool WriteAssortmentProperties(
			int headerRid,
			double aReserverAmt,
			eReserveType aReserveType,
			int aStoreGroupRid,
			eAssortmentVariableType aVariableType,
			int aVariableNumber,
			bool aInclOnhand,
			bool aInclIntransit,
			bool aInclSimStores,
			bool aInclCommitted,	// TT#2 - stodd - assortment
			eStoreAverageBy aAverageBy,
			int aCdrRid,				// TT#2 - stodd - assortment
			int aAnchorHnRid,		// TT#2 - stodd - assortment
			int aUserRid,
			DateTime aProcessDate,
            int aBeginDayCdrRid)   // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
		{
			try
			{
				//=================================================
				// Only add a record if the Variable Type is valid
				//=================================================
				if (aVariableType != eAssortmentVariableType.None)
				{
                    //string insertCommand = "insert into ASSORTMENT_PROPERTIES "
                    //    + " (HDR_RID, RESERVE, RESERVE_TYPE_IND, SG_RID, VARIABLE_TYPE, VARIABLE_NUMBER, "
                    //    + "INCL_ONHAND, INCL_INTRANSIT, INCL_SIMILAR_STORES, INCL_COMMITTED, AVERAGE_BY, CDR_RID, USER_RID, ANCHOR_HN_RID, LAST_PROCESS_DATETIME) "
                    //    + " values(@HDR_RID, @RESERVE, @RESERVE_TYPE_IND, @SG_RID, @VARIABLE_TYPE, @VARIABLE_NUMBER, "
                    //    + "@INCL_ONHAND, @INCL_INTRANSIT, @INCL_SIMILAR_STORES, @INCL_COMMITTED, @AVERAGE_BY, @CDR_RID, @USER_RID, @ANCHOR_HN_RID, @LAST_PROCESS_DATETIME) ";

                    //MIDDbParameter[] InParams = {   
                    //    new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@RESERVE", aReserverAmt, eDbType.Decimal, eParameterDirection.Input),
                    //    new MIDDbParameter("@RESERVE_TYPE_IND", (int)(aReserveType), eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@SG_RID", aStoreGroupRid, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@VARIABLE_TYPE", (int)aVariableType, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@VARIABLE_NUMBER", aVariableNumber, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@INCL_ONHAND", Include.ConvertBoolToChar(aInclOnhand), eDbType.VarChar, eParameterDirection.Input),
                    //    new MIDDbParameter("@INCL_INTRANSIT", Include.ConvertBoolToChar(aInclIntransit), eDbType.VarChar, eParameterDirection.Input),
                    //    new MIDDbParameter("@INCL_SIMILAR_STORES", Include.ConvertBoolToChar(aInclSimStores), eDbType.VarChar, eParameterDirection.Input),
                    //    new MIDDbParameter("@INCL_COMMITTED", Include.ConvertBoolToChar(aInclCommitted), eDbType.VarChar, eParameterDirection.Input),
                    //    new MIDDbParameter("@AVERAGE_BY", (int)(aAverageBy), eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@CDR_RID", aCdrRid, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@ANCHOR_HN_RID", aAnchorHnRid, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@USER_RID", aUserRid, eDbType.Int, eParameterDirection.Input),
                    //    new MIDDbParameter("@LAST_PROCESS_DATETIME", aProcessDate, eDbType.DateTime, eParameterDirection.Input)
                    //    };

                    int rowsInserted = StoredProcedures.MID_ASSORTMENT_PROPERTIES_INSERT.Insert(_dba,
                                                                         HDR_RID: headerRid,
                                                                         RESERVE: aReserverAmt,
                                                                         RESERVE_TYPE_IND: (int)(aReserveType),
                                                                         SG_RID: aStoreGroupRid,
                                                                         VARIABLE_TYPE: (int)aVariableType,
                                                                         VARIABLE_NUMBER: aVariableNumber,
                                                                         INCL_ONHAND: Include.ConvertBoolToChar(aInclOnhand),
                                                                         INCL_INTRANSIT: Include.ConvertBoolToChar(aInclIntransit),
                                                                         INCL_SIMILAR_STORES: Include.ConvertBoolToChar(aInclSimStores),
                                                                         INCL_COMMITTED: Include.ConvertBoolToChar(aInclCommitted),
                                                                         AVERAGE_BY: (int)(aAverageBy),
                                                                         CDR_RID: aCdrRid,
                                                                         USER_RID: aUserRid,
                                                                         ANCHOR_HN_RID: aAnchorHnRid,
                                                                         LAST_PROCESS_DATETIME: aProcessDate,
                                                                         BEGIN_DAY_CDR_RID: aBeginDayCdrRid);  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

                    //if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
                    if (rowsInserted == 0)  // TT#1307-MD - stodd - GA Matrix incorrect for newly created Group Allocation
					{
						return false;
					}
				}

				return true;
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Deletes any assortment store grades before writing the new ones
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="aGradeCode"></param>
		/// <param name="aBoundary"></param>
		/// <param name="aBoundaryInd"></param>
		/// <returns></returns>
        //public bool WriteAssortmentPropertiesStoreGrades(
        //    int headerRid,
        //    List<string> gradeCodeList,
        //    List<double> boundaryList,
        //    List<double> boundaryUnitsList)		// TT#2 - stodd - assortment
        //{
        //    try
        //    {
        //        int seq = 1;
        //        if (DeleteAssortmentPropertiesStoreGrades(headerRid))
        //        {
        //            // Begin TT#2 - stodd - assortment
        //            string insertCommand = "insert into ASSORTMENT_PROPERTIES_STORE_GRADE "
        //                + " (HDR_RID, STORE_GRADE_SEQ, BOUNDARY_UNITS, BOUNDARY_INDEX, GRADE_CODE) "
        //                + " values(@HDR_RID, @STORE_GRADE_SEQ, @BOUNDARY_UNITS, @BOUNDARY_INDEX, @GRADE_CODE)";
					
        //            for (int i = 0; i < gradeCodeList.Count; i++)
        //            {
        //                MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
        //                new MIDDbParameter("@STORE_GRADE_SEQ", seq++, eDbType.Int, eParameterDirection.Input),
        //                new MIDDbParameter("@BOUNDARY_UNITS", boundaryUnitsList[i], eDbType.Float, eParameterDirection.Input),
        //                new MIDDbParameter("@BOUNDARY_INDEX", boundaryList[i], eDbType.Float, eParameterDirection.Input),
        //                new MIDDbParameter("@GRADE_CODE", gradeCodeList[i], eDbType.VarChar, eParameterDirection.Input)
        //                    };
        //                if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
        //                {
        //                    return false;
        //                }
        //            }
        //            // End TT#2 - stodd - assortment
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		/// <summary>
		/// Writes one assortment store grade at a time
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="aGradeCode"></param>
		/// <param name="aBoundary"></param>
		/// <param name="aBoundaryInd"></param>
		/// <returns></returns>
		public bool WriteAssortmentPropertiesStoreGrade(
			int headerRid,
			int sequence,
			string gradeCode,
			int boundary,
			int boundaryUnits)	// TT#2 - stodd - assortment
		{
			try
			{
                //string insertCommand = "insert into ASSORTMENT_PROPERTIES_STORE_GRADE "
                //    + " (HDR_RID, STORE_GRADE_SEQ, BOUNDARY_UNITS, BOUNDARY_INDEX, GRADE_CODE) "
                //    + " values(@HDR_RID, @STORE_GRADE_SEQ, @BOUNDARY_UNITS, @BOUNDARY_INDEX, @GRADE_CODE)";

                //// Begin TT#2 - stodd - assortment
                //MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@STORE_GRADE_SEQ", sequence, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@BOUNDARY_UNITS", boundaryUnits, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@BOUNDARY_INDEX", boundary, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@GRADE_CODE", gradeCode, eDbType.VarChar, eParameterDirection.Input)
                //            };

                int rowsInserted = StoredProcedures.MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT.Insert(_dba,
                                                                                     HDR_RID: headerRid,
                                                                                     STORE_GRADE_SEQ: sequence,
                                                                                     BOUNDARY_UNITS: boundaryUnits,
                                                                                     BOUNDARY_INDEX: boundary,
                                                                                     GRADE_CODE: gradeCode
                                                                                     );
                //if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
                if (rowsInserted == 0)  // TT#1307-MD - stodd - GA Matrix incorrect for newly created Group Allocation
				{
					return false;
				}
				// End TT#2 - stodd - assortment
				return true;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes any assortment basis before writing the new one
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="aHierNodeRid"></param>
		/// <param name="aVersionRid"></param>
		/// <param name="aDateRangeRid"></param>
		/// <param name="aWeight"></param>
		/// <returns></returns>
		public bool WriteAssortmentPropertiesBasis(
			int headerRid,
			List<int> hierNodeList,
			List<int> versionList,
			List<int> dateRangeList,
			List<double> weightList)
		{
			try
			{
				int seq = 1;
                if (DeleteAssortmentPropertiesBasis(headerRid))
				{
                    //string insertCommand = "insert into ASSORTMENT_PROPERTIES_BASIS "
                    //    + " (HDR_RID, BASIS_SEQ, HN_RID, FV_RID, CDR_RID, WEIGHT) "
                    //    + " values(@HDR_RID, @BASIS_SEQ, @HN_RID, @FV_RID, @CDR_RID, @WEIGHT)";
					for (int i = 0; i < hierNodeList.Count; i++)
					{
                        //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
                        //                            new MIDDbParameter("@BASIS_SEQ", seq++, eDbType.Int, eParameterDirection.Input),
                        //                            new MIDDbParameter("@HN_RID", hierNodeList[i], eDbType.Int, eParameterDirection.Input),
                        //                            new MIDDbParameter("@FV_RID", versionList[i], eDbType.Int, eParameterDirection.Input),
                        //                            new MIDDbParameter("@CDR_RID", dateRangeList[i], eDbType.Int, eParameterDirection.Input),
                        //                            new MIDDbParameter("@WEIGHT", weightList[i], eDbType.Float, eParameterDirection.Input)
                        //                      };

                        int rowsInserted = StoredProcedures.MID_ASSORTMENT_PROPERTIES_BASIS_INSERT.Insert(_dba,
                                                                                                        HDR_RID: headerRid,
                                                                                                        BASIS_SEQ: seq,
                                                                                                        HN_RID: hierNodeList[i],
                                                                                                        FV_RID: versionList[i],
                                                                                                        CDR_RID: dateRangeList[i],
                                                                                                        WEIGHT: weightList[i]
                                                                                                        );

                        //if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
                        if (rowsInserted == 0)  // TT#1307-MD - stodd - GA Matrix incorrect for newly created Group Allocation
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Writes one Assortment basis line at a time
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="seq"></param>
		/// <param name="hierNodeRid"></param>
		/// <param name="versionRid"></param>
		/// <param name="dateRangeRid"></param>
		/// <param name="weight"></param>
		/// <returns></returns>
		public bool WriteAssortmentPropertiesBasis(
			int headerRid,
			int seq,
			int hierNodeRid,
			int versionRid,
			int dateRangeRid,
			float weight)
		{
			try
			{
                //string insertCommand = "insert into ASSORTMENT_PROPERTIES_BASIS "
                //    + " (HDR_RID, BASIS_SEQ, HN_RID, FV_RID, CDR_RID, WEIGHT) "
                //    + " values(@HDR_RID, @BASIS_SEQ, @HN_RID, @FV_RID, @CDR_RID, @WEIGHT)";
                //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@BASIS_SEQ", seq, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@HN_RID", hierNodeRid, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@FV_RID", versionRid, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@CDR_RID", dateRangeRid, eDbType.Int, eParameterDirection.Input),
                //        new MIDDbParameter("@WEIGHT", weight, eDbType.Float, eParameterDirection.Input)
                //                      };
                int rowsInserted = StoredProcedures.MID_ASSORTMENT_PROPERTIES_BASIS_INSERT.Insert(_dba,
                                                                               HDR_RID: headerRid,
                                                                               BASIS_SEQ: seq,
                                                                               HN_RID: hierNodeRid,
                                                                               FV_RID: versionRid,
                                                                               CDR_RID: dateRangeRid,
                                                                               WEIGHT: weight
                                                                               );

                //if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
                if (rowsInserted == 0)  // TT#1307-MD - stodd - GA Matrix incorrect for newly created Group Allocation
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

		

		public bool WriteAssortmentStoreSummary(
			int headerRid,
			DataTable dtTotalAssortment)
		{
			try
			{
                //string insertCommand = "insert into ASSORTMENT_STORE_SUMMARY "
                //    + " (HDR_RID, ST_RID, VARIABLE_NUMBER, UNITS, INTRANSIT, NEED, PCT_NEED, VARIABLE_TYPE) "
                //    + " values(@HDR_RID, @ST_RID, @VARIABLE_NUMBER, @UNITS, @INTRANSIT, @NEED, @PCT_NEED, @VARIABLE_TYPE)";
				foreach (DataRow aRow in dtTotalAssortment.Rows)
				{
					int storeRid = Convert.ToInt32(aRow["ST_RID"], CultureInfo.CurrentUICulture);
					int variableNumber = Convert.ToInt32(aRow["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
					int units = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
					int intransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
					int need = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
					decimal pctNeed = Convert.ToDecimal(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
					int varType = Convert.ToInt32(aRow["VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
                    //Begin TT#1268-MD -jsobek -5.4 Merge
                    int onhand = Convert.ToInt32(aRow["ONHAND"]);	// TT#845-MD - Stodd - add OnHand to Summary
                    int VSWOnhand = Convert.ToInt32(aRow["VSW_ONHAND"]);	// TT#952 - MD - stodd - add matrix to Group Allocation Review
                    //End TT#1268-MD -jsobek -5.4 Merge
                    int planSales = Convert.ToInt32(aRow["PLAN_SALES_UNITS"]); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    int planStock = Convert.ToInt32(aRow["PLAN_STOCK_UNITS"]); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

                    //MIDDbParameter[] InParams = {  new MIDDbParameter("@HDR_RID", headerRid, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@ST_RID", storeRid, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@VARIABLE_NUMBER", variableNumber, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@UNITS", units, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@INTRANSIT", intransit, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@NEED", need, eDbType.Int, eParameterDirection.Input),
                    //                            new MIDDbParameter("@PCT_NEED", pctNeed, eDbType.Float, eParameterDirection.Input),
                    //                            new MIDDbParameter("@VARIABLE_TYPE", varType, eDbType.Int, eParameterDirection.Input)
                    //                            };
                    int rowsInserted = StoredProcedures.MID_ASSORTMENT_STORE_SUMMARY_INSERT.Insert(_dba, 
				                                                                        HDR_RID: headerRid,
				                                                                        ST_RID: storeRid,
				                                                                        VARIABLE_NUMBER: variableNumber,
				                                                                        UNITS: units,
				                                                                        INTRANSIT: intransit,
				                                                                        NEED: need,
				                                                                        PCT_NEED: pctNeed,
				                                                                        VARIABLE_TYPE: varType,
                                                                                        ONHAND: onhand, //TT#1268-MD -jsobek -5.4 Merge
                                                                                        VSW_ONHAND: VSWOnhand, //TT#1268-MD -jsobek -5.4 Merge
                                                                                        PLAN_SALES_UNITS: planSales, // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                                                                                        PLAN_STOCK_UNITS: planStock // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				                                                                        );

                    //if (!(_dba.ExecuteNonQuery(insertCommand, InParams) > 0))
                    if (rowsInserted == 0)  // TT#1307-MD - stodd - GA Matrix incorrect for newly created Group Allocation
					{
						return false;
					}
				}
				return true;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#857 MD - Stodd - assortment not honoring views
        public int GetAssortmentUserView(int hdrRid, int userRid)
        {
            try
            {
                int viewRid = Include.NoRID;

                //Begin TT#1268-MD -jsobek -5.4 Merge
                //string SQLCommand = "SELECT [dbo].[UDF_ASRT_GET_USER_VIEW] (@HDR_RID, @USER_RID) AS VIEW_RID";

                //MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", hdrRid, eDbType.Int),
                //                              new MIDDbParameter("@USER_RID", userRid, eDbType.Int)
                //                            };

                //DataTable dt = _dba.ExecuteSQLQuery(SQLCommand, "ASSORTMENT_USER_VIEW_JOIN", InParams);
                DataTable dt = StoredProcedures.MID_ASSORTMENT_USER_VIEW_READ.Read(_dba, HDR_RID: hdrRid, USER_RID: userRid);
                //End TT#1268-MD -jsobek -5.4 Merge
                if (dt.Rows.Count > 0)
                {
                    DataRow aRow = dt.Rows[0];
                    if (aRow["VIEW_RID"] != DBNull.Value)
                    {
                        viewRid = int.Parse(aRow["VIEW_RID"].ToString());
                    }
                }
                return viewRid;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        ///  Does an upsert. Inserts if needed, otherwise does an update.
        /// </summary>
        /// <param name="hdrRid"></param>
        /// <param name="userRid"></param>
        /// <param name="viewRid"></param>
        /// <returns></returns>
        public void WriteAssortmentUserView(int hdrRid, int userRid, int viewRid)
        {
            try
            {
                _dba.OpenUpdateConnection();

                //Begin TT#1268-MD -jsobek -5.4 Merge
                //MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", hdrRid),
                //                              new MIDDbParameter("@USER_RID", userRid),
                //                              new MIDDbParameter("@VIEW_RID", viewRid),
                //                            };
                //InParams[0].DbType = eDbType.Int;
                //InParams[0].Direction = eParameterDirection.Input;
                //InParams[1].DbType = eDbType.Int;
                //InParams[1].Direction = eParameterDirection.Input;
                //InParams[2].DbType = eDbType.Int;
                //InParams[2].Direction = eParameterDirection.Input;

                ////MIDDbParameter[] OutParams = { new MIDDbParameter("@COLOR_CODE_RID", DBNull.Value) };
                ////OutParams[0].DbType = eDbType.Int;
                ////OutParams[0].Direction = eParameterDirection.Output;

                //_dba.ExecuteStoredProcedure("SP_MID_ASRT_USER_VIEW_UPSERT", InParams, null);

                StoredProcedures.MID_ASSORTMENT_USER_VIEW_UPSERT.Insert(_dba, HDR_RID: hdrRid, USER_RID: userRid, VIEW_RID: viewRid);
                //End TT#1268-MD -jsobek -5.4 Merge

                _dba.CommitData();
                _dba.CloseUpdateConnection();

            }
            catch (Exception err)
            {
                _dba.RollBack();
                _dba.CloseUpdateConnection();
                string message = err.ToString();
                throw;
            }
        }
        // END TT#857 MD - Stodd - assortment not honoring views

        // Begin TT#936 - MD - Prevent the saving of empty Group Allocations
        /// <summary>
        /// Returns the count of actual headers in the assortment (or group allocation). Placeholders are not counted.
        /// </summary>
        /// <param name="asrtHdrRid"></param>
        /// <returns></returns>
        public int GetAssortmentHeaderCount(int asrtHdrRid)
        {
            try
            {
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //string SQLCommand = @"SELECT COUNT(*) MyCount FROM HEADER h WHERE h.ASRT_RID = @asrtHdrRid and h.ASRT_RID <> h.HDR_RID and DISPLAY_TYPE not in (800739, 800740)";
                //MIDDbParameter[] InParams = { new MIDDbParameter("@asrtHdrRid", asrtHdrRid, eDbType.Int) };
                //return _dba.ExecuteRecordCount(SQLCommand, InParams);

                return StoredProcedures.MID_HEADER_READ_ASSORTMENT_COUNT.ReadRecordCount(_dba, ASRT_RID: asrtHdrRid);
                //End TT#1268-MD -jsobek -5.4 Merge
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int GetPlaceholderHeaderCount(int placeholderHdrRid)
        {
            try
            {
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //string SQLCommand = @"SELECT COUNT(*) MyCount FROM HEADER h WHERE h.PLACEHOLDER_RID = @placeholderHdrRid and h.PLACEHOLDER_RID <> h.HDR_RID and DISPLAY_TYPE not in (800739, 800740)";
                //MIDDbParameter[] InParams = { new MIDDbParameter("@placeholderHdrRid", placeholderHdrRid, eDbType.Int) };
                //return _dba.ExecuteRecordCount(SQLCommand, InParams);

                return StoredProcedures.MID_HEADER_READ_PLACEHOLDER_COUNT.ReadRecordCount(_dba, PLACEHOLDER_RID: placeholderHdrRid);
                //End TT#1268-MD -jsobek -5.4 Merge
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#936 - MD - Prevent the saving of empty Group Allocations
		
	}
}
