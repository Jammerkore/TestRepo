using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;   // MID Track 3994 Performance
using System.Reflection;
using System.Diagnostics;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Allocation Header
	/// </summary>
	public partial class Header : DataLayer
	{
        // Begin TT#739-MD -JSmith - Delete Stores
        private DataSet _dsHeaderAllocation = null;
        private int _currHeaderRID = Include.NoRID;
        DataTable _dtTotalAllocationSchema = null;
        DataTable _dtDetailAllocationSchema = null;
        DataTable _dtBulkAllocationSchema = null;
        DataTable _dtPackAllocationSchema = null;
        DataTable _dtColorAllocationSchema = null;
        DataTable _dtColorSizeAllocationSchema = null;
        int TotalAllocationWorkRowCount = 0;
        int DetailAllocationWorkRowCount = 0;
        int BulkAllocationWorkRowCount = 0;
        int PackAllocationWorkRowCount = 0;
        int BulkColorAllocationWorkRowCount = 0;
        int BulkColorSizeAllocationWorkRowCount = 0;
        // End TT#739-MD -JSmith - Delete Stores

		public Header()
            : base(false) // TT#1185 - Verify ENQ before update
		{

		}

        // Begin TT#188 - JSmith - Rebrand
        public Header(string aConnectionString)
			: base(aConnectionString, false)  // TT#1185 - Verify ENQ before Update
            // : base(aConnectionString)      // TT#1185 - Verify ENQ before Update
		{

		}
        // End TT#188

        // Begin TT#634 - JSmith - Color rename
        public Header(TransactionData td)
            : base(td.DBA, false) // TT#1185 - Verify ENQ before Update
            // : base(td.DBA) // TT#1185 - Verfiry ENQ before Update
        {

        }
        // End TT#634
     
        // Begin TT#739-MD - JSmith - Delete Stores
        public override void CommitData()
        {
            // Uncomment is using SQLBulkCopy
            //if (TotalAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBTotalAllocation);
            //    TotalAllocationWorkRowCount = 0;
            //}
            //if (DetailAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBDetailAllocation);
            //    DetailAllocationWorkRowCount = 0;
            //}
            //if (BulkAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBBulkAllocation);
            //    BulkAllocationWorkRowCount = 0;
            //}
            //if (PackAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBPackAllocation);
            //    PackAllocationWorkRowCount = 0;
            //}
            //if (BulkColorAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBBulkColorAllocation);
            //    BulkColorAllocationWorkRowCount = 0;
            //}
            //if (BulkColorSizeAllocationWorkRowCount > 0)
            //{
            //    MoveTempTableData(Include.DBBulkColorSizeAllocation);
            //    BulkColorSizeAllocationWorkRowCount = 0;
            //}
            base.CommitData();
        }
        // End TT#739-MD - JSmith - Delete Stores

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed
        //public DataTable GetHeaderGroups()
        //{
        //    try
        //    {
        //        //MID Track # 2354 - removed nolock because it causes concurrency issues
        //        return StoredProcedures.MID_HEADER_READ_ALL_GROUPS.Read(_dba);
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed

        

		public DataTable GetHeaderGroup(int headerGroupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HEADER_GROUP_READ.Read(_dba, HDR_GROUP_RID: headerGroupRID);
			}
			catch 
			{
				throw ;
			}
		}

		/// <summary>
		/// returns a datatable of HEADER records that belong to the header group 
		/// </summary>
		/// <param name="headerGroupRID"></param>
		/// <returns></returns>
		public DataTable GetHeaderGroupChildren(int headerGroupRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HEADER_READ_GROUP_CHILDREN.Read(_dba, HDR_GROUP_RID: headerGroupRID);
			}
			catch 
			{
				throw ;
			}
		}

		public bool HeaderExists(string headerID)
		{
			try
			{       
                return (StoredProcedures.MID_HEADER_ID_EXISTS.ReadRecordCount(_dba, HDR_ID: headerID) > 0);
			}
			catch 
			{
				throw ;
			}
		}

        // BEGIN MID Track #6127 - Case Insensitive ComponentOne issue
        public bool DuplicateHeaderExists(string aHeaderID, int aHeaderRID)
        {
            try
            {
                string upperHeaderID = aHeaderID.ToUpper().Trim();
                return (StoredProcedures.MID_HEADER_ID_UPPERCASE_EXISTS.ReadRecordCount(_dba, HDR_ID: upperHeaderID, HDR_RID: aHeaderRID) > 0);
            }
            catch
            {
                throw;
            }
        }
        // END MID Track #6127

        public int GetHeaderRID(string headerID)
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //TT#1663 - DOConnell - Change Pack Name with quote issue
            return Convert.ToInt32(StoredProcedures.MID_HEADER_READ_RID_FROM_ID.ReadValue(_dba, HDR_ID: headerID), CultureInfo.CurrentUICulture);       
        }

        // Begin MID multi header stodd
        public string GetHeaderID(int headerRid)
        {
            return Convert.ToString(StoredProcedures.MID_HEADER_READ_ID.ReadValue(_dba, HDR_RID: headerRid), CultureInfo.CurrentUICulture);
        }

        public bool IsMultiHeader(string headerID)
        {
            bool isMulti = false;

            //TT#1663 - DOConnell - Change Pack Name with quote issue
            int multiHdr = Convert.ToInt32(StoredProcedures.MID_HEADER_READ_MULTI_FLAG_FROM_ID.ReadValue(_dba, HDR_ID: headerID), CultureInfo.CurrentUICulture);

            if (multiHdr == 1)
                isMulti = true;
            return isMulti;
        }
        // End MID multi header

        // Begin TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.
        public DataTable GetHeaderForWorkspace(int headerRID)
        {
            return StoredProcedures.MID_HEADER_READ_FOR_WORKSPACE.Read(_dba, HDR_RID: headerRID);
        }
        // End TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.

        public DataTable GetHeader(int headerRID)
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //MID Track 4033 allow children of multi to change
            return StoredProcedures.MID_HEADER_READ.Read(_dba, HDR_RID: headerRID);
        }


        public DataTable GetHeader(string headerID)
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //MID Track 4033 allow children of multi to change
            return StoredProcedures.MID_HEADER_READ_FROM_ID.Read(_dba, HDR_ID: headerID);
        }

        public DataTable GetHeaders()
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //MID Track 4033 allow children of multi to change
            return StoredProcedures.MID_HEADER_READ_ALL.Read(_dba);
        }

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        public DataTable GetNonReleasedHeaders()
        {
            return StoredProcedures.MID_HEADER_READ_NON_RELEASED.Read(_dba);
        }
        // End TT#1065

        public DataTable GetNonReleasedHeadersForReconcile(DataTable dtHeaderType)
        {
            return StoredProcedures.MID_HEADER_READ_RECONCILE_NON_RELEASED.Read(_dba, dtHeaderType);
        }


        //Begin TT#1313-MD -jsobek -Header Filters
        // Begin TT#827 - JSmith - Allocation Performance
        //public DataTable GetHeadersForTaskList(int aTaskListRID, int aTaskSeq)
        //{
        //    return StoredProcedures.SP_MID_GET_HEADERS_FOR_TASKLIST.Read(_dba, 
        //                                                                  TASKLIST_RID: aTaskListRID,
        //                                                                  TASK_SEQUENCE: aTaskSeq
        //                                                                  );
        //}
        // End TT#827 - JSmith - Allocation Performance
        //End TT#1313-MD -jsobek -Header Filters

        public DataTable GetHeaders(int nodeRID)
        {
            return StoredProcedures.MID_HEADER_READ_FROM_STYLE_NODE.Read(_dba, STYLE_HNRID: nodeRID);
        }

        // begin TT#1185 Verify ENQ before Update
        /// <summary>
        /// Gets all the headers for the given Style RID containing the given color RID
        /// </summary>
        /// <param name="aSTYLE_HNRID">Style_HNRID</param>
        /// <param name="aColorRID">Color RID</param>
        /// <returns>DataTable containing all headers for the given style containing the given color RID</returns>
        public DataTable GetHeaders(int aSTYLE_HNRID, int aColorCodeRID)
        {
            return StoredProcedures.SP_MID_GET_STYL_HDRS_WITH_COLR.Read(_dba, 
                                                                         STYLE_HNRID: aSTYLE_HNRID,
                                                                         COLOR_CODE_RID: aColorCodeRID
                                                                         );
        }

        private MIDEnqueue midEnq = new MIDEnqueue();
        /// <summary>
        /// Determines whether specified header wass enqueued by the given user on the given thread in the given transaction at the specified enqueue time
        /// </summary>
        /// <param name="aHeaderID">Header ID</param>
        /// <param name="aHeaderRID">Header RID</param>
        /// <param name="aUserRID">User RID</param>
        /// <param name="aThreadID">Thread ID</param>
        /// <param name="aTranID">Transaction ID</param>
        /// <param name="aEnqueuedTimeStamp">Enqueue Time Stamp</param>
        /// <param name="aHeaderConflict">Header Conflict when there is a header enqueue conflict (null when status is not a conflict</param>
        /// <returns>Enqueue Status; Header Conflict is returned whenever the status is a conflict (ie the header is enqueued but not by the specifed user, thread, tran ID at the specified time</returns>
        public eEnqueueStatus HeaderIsEnqueued(string aHeaderID, int aHeaderRID, int aUserRID, int aThreadID, Int64 aTranID, DateTime aEnqueuedTimeStamp, out HeaderConflict aHeaderConflict)
        {
            eEnqueueStatus enqueueStatus;
            int ownedByUserRID;
            int ownedByThread;
            Int64 ownedByTranID;
            DateTime enqTime;
            aHeaderConflict = null;
            enqueueStatus = GetHeaderEnqueueStatus(aHeaderRID, aUserRID, aThreadID, aTranID, out ownedByUserRID, out ownedByThread, out ownedByTranID, out enqTime);
            switch (enqueueStatus)
            {
                case eEnqueueStatus.Enqueued:
                case eEnqueueStatus.TentativelyEnqueued:
                    {
                        if (enqTime != aEnqueuedTimeStamp)
                        {
                            enqueueStatus = eEnqueueStatus.EnqueueConflict;
                            aHeaderConflict =
                                new HeaderConflict(
                                    aHeaderID,
                                    aHeaderRID,
                                    ownedByUserRID,
                                    ownedByThread,
                                    ownedByTranID,
                                    enqTime);
                        }
                        else
                        {
                            enqueueStatus = eEnqueueStatus.Enqueued;
                        }
                        break;
                    }
                case eEnqueueStatus.NotEnqueued:
                    {
                        break;
                    }
                case eEnqueueStatus.EnqueueConflict:
                    {
                        aHeaderConflict =
                            new HeaderConflict(
                                aHeaderID,
                                aHeaderRID,
                                ownedByUserRID,
                                ownedByThread,
                                ownedByTranID,
                                enqTime);
                        break;
                    }
                default:
                    {
                        throw new Exception ("Header.cs:  Unknown Header Enqueue Status");
                    }
            }
            return enqueueStatus;
        }
        /// <summary>
        /// Gets Header Enqueue Status.  If Header is enqueued for the given user on the given thread in the given transaction, the status is returned as "tentatively enqueued".  The time stamp of this enqueue is also returned.  The time stamp can be used to verify that the enqueue persists.  EXAMPLE: when a header is tentatively enqueued at read time, if the enqueue persists to update time (same enqueue with the same date stamp) then it is safe to "assume" that the update is valid--ie no other process has done a controlled update of the header.
        /// </summary>
        /// <param name="aHeaderRID">Header RID</param>
        /// <param name="aUserRID">User RID</param>
        /// <param name="aThreadID">Thread ID</param>
        /// <param name="aTranID">Transaction ID</param>
        /// <param name="aOwnedByUserRID">IF an enqueue exists, the User who requested the enqueue</param>
        /// <param name="aOwnedByThreadID">IF an enqueue exists, the Thread ID where the enqueue occurred</param>
        /// <param name="aOwnedByTranID">IF an enqueue exists, the Transaction ID where the enqueue occurred</param>
        /// <param name="aEnqueuedTimeStamp">IF an enqueue exists, the time when the enqueue occurred</param>
        /// <returns>eEnqueueStatus of the header.  NOTE: IF the header is enqueued by the specified user, thread and transaction, then the returned status is "TentativelyEnqueued"; the time stamp of this enqueue is also returned</returns>
        /// <remarks>The "Enqueued" status will not be returned by this process because that status requires knowing the time stamp of the enqueue.</remarks>
        public eEnqueueStatus GetHeaderEnqueueStatus(int aHeaderRID, int aUserRID, int aThreadID, Int64 aTranID, out int aOwnedByUserRID, out int aOwnedByThreadID, out Int64 aOwnedByTranID, out DateTime aEnqueuedTimeStamp)
        {
            DataTable dt = midEnq.Enqueue_Read(new Resource(eLockType.Header, aHeaderRID, string.Empty));
    
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int headerRID =  Convert.ToInt32(dr["RID"], CultureInfo.CurrentUICulture);
                aOwnedByUserRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                aOwnedByThreadID = Convert.ToInt32(dr["OWNING_THREADID"], CultureInfo.CurrentUICulture);
                aOwnedByTranID = Convert.ToInt64(dr["OWNING_TRANID"], CultureInfo.CurrentUICulture);
                aEnqueuedTimeStamp = Convert.ToDateTime(dr["ENQUEUE_TIMESTAMP"], CultureInfo.CurrentUICulture);
                if (headerRID == aHeaderRID
                    && aOwnedByUserRID == aUserRID
                    && aOwnedByThreadID == aThreadID
                    && aOwnedByTranID == aTranID || aOwnedByTranID == 0)
                {
                    return eEnqueueStatus.TentativelyEnqueued;
                }
                return eEnqueueStatus.EnqueueConflict;
            }
            aOwnedByUserRID = Include.NoRID;
            aOwnedByThreadID = 0;
            aOwnedByTranID = 0;
            aEnqueuedTimeStamp = DateTime.MinValue;
            return eEnqueueStatus.NotEnqueued;
        }
        // end TT#1185 Verify ENQ before Update 

       

        // Begin TT#1705 - JSmith - Reset Header with Piggybacking
        public DataTable GetHeadersWithIdLike(string aHeaderSelectString)
        {
            //aHeaderSelectString = "'" + aHeaderSelectString + "'";
            return StoredProcedures.MID_HEADER_READ_FROM_ID_LIKE.Read(_dba, HDR_ID: aHeaderSelectString);
        }

        public DataTable GetHeadersWithIdGreaterThan(string aHeaderSelectString)
        {
            //aHeaderSelectString = "'" + aHeaderSelectString + "'";
            return StoredProcedures.MID_HEADER_READ_FROM_ID_GREATER_THAN.Read(_dba, HDR_ID: aHeaderSelectString);	
        }
        // End TT#1705

		// begin TT#1137 (MID Track 4351) Rebuild Intransit Utility
		public DataTable GetHeadersChargedToIT(int[] aStyleHnRID)
		{
            DataTable dtStyleList = new DataTable();
            dtStyleList.Columns.Add("HN_RID", typeof(int));
            foreach (int styleHnRID in aStyleHnRID)
            {
                //ensure styleHNRids are distinct, and only added to the datatable one time
                if (dtStyleList.Select("HN_RID=" + styleHnRID.ToString()).Length == 0)
                {
                    DataRow dr = dtStyleList.NewRow();
                    dr["HN_RID"] = styleHnRID;
                    dtStyleList.Rows.Add(dr);
                }
            }

            return StoredProcedures.MID_HEADER_READ_CHARGED_TO_INTRANSIT.Read(_dba, HN_RID_LIST: dtStyleList);			  
		}
		// end TT#1137 (MID Track 4351) Rebuild Intransit Utility
        // begin MID Track 6250 Relieve Intransit Errors
        /// <summary>
        /// Gets headers where all units are allocated to the reserve store
        /// </summary>
        /// <returns>DataTable containing the HDR_ID of the headers allocated to the reserve stores</returns>
        public DataTable GetHeadersAllocatedToReserve()
        {
            return StoredProcedures.MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE.Read(_dba);
        }
        // end MID Track 6250 Relieve Intrtansit Errors

        // Begin Reclass - add methods to retrieve headers
        public DataTable GetPlanNodeHeaders(int nodeRID)
        {
            return StoredProcedures.MID_HEADER_READ_FROM_PLAN_NODE.Read(_dba, PLAN_HNRID: nodeRID);
        }

        public DataTable GetOnHandNodeHeaders(int nodeRID)
        {
            return StoredProcedures.MID_HEADER_READ_FROM_ON_HAND_NODE.Read(_dba, ON_HAND_HNRID: nodeRID);

        }
        // End Reclass - add methods to retrieve headers

        // Begin TT#2 - Ron Matelic - Assortment Planning
        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        //public DataTable GetAssortmentHeadersForUser(int aUserRID)
        //{
        //    return StoredProcedures.SP_MID_GET_ASSORTMENTS_FOR_USER.Read(_dba, USER_RID: aUserRID);
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions

        public DataTable GetHeadersInAssortment(int aAsrtRID)
        {
            return StoredProcedures.SP_MID_GET_HEADERS_IN_ASSORTMENT.Read(_dba, ASRT_RID: aAsrtRID);
        }
        // End TT#2

        //Begin TT#1313-MD -jsobek -Header Filters
        //public DataTable GetHeadersForUser(int aUserRID,
        //    eFilterDateType aHeaderDateType, DateTime aHeaderFromDate, DateTime aHeaderToDate,
        //    eFilterDateType aReleaseDateType, DateTime aReleaseFromDate, DateTime aReleaseToDate)
        //{
        //    return StoredProcedures.SP_MID_GET_HEADERS_FOR_USER.Read(_dba, 
        //                                                              USER_RID: aUserRID,
        //                                                              HDR_DATE_TYPE: (int)aHeaderDateType,
        //                                                              HDR_FROM_DATE: aHeaderFromDate,
        //                                                              HDR_TO_DATE: aHeaderToDate,
        //                                                              RELEASE_DATE_TYPE: (int)aReleaseDateType,
        //                                                              RELEASE_FROM_DATE: aReleaseFromDate,
        //                                                              RELEASE_TO_DATE: aReleaseToDate
        //                                                              );
        //}

        public DataTable GetHeadersFromFilter(int headerFilterRID, FilterHeaderOptions headerFilterOptions)
        {
            int useWorkspaceFields = 0;
            if (headerFilterOptions.USE_WORKSPACE_FIELDS == true) useWorkspaceFields = 1;

            return StoredProcedures.MID_HEADER_READ_FROM_FILTER.Read(_dba, headerFilterRID, headerFilterOptions.HN_RID_OVERRIDE, useWorkspaceFields, headerFilterOptions.filterType);
        }

        //End TT#1313-MD -jsobek -Header Filters
       
        public DataTable GetHeadersToPurge()
        {
            return StoredProcedures.SP_MID_GET_HEADERS_TO_DELETE.Read(_dba);
        }

        public DataTable GetMultiHeadersToPurge()
        {
            return StoredProcedures.SP_MID_GET_MULTI_HEADERS_TO_DELETE.Read(_dba);
        }
      
        //Begin TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        public DataTable GetGroupAllocationHeadersToPurge()
        {
            //Begin TT#1268-MD -jsobek -5.4 Merge
            //return _dba.ExecuteQuery("SP_MID_GET_GROUP_ALLOCATION_HEADERS_TO_DELETE", "GROUP_ALLOCATION_HEADERS");
            return StoredProcedures.MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION.Read(_dba);
            //End TT#1268-MD -jsobek -5.4 Merge
        }
        //End TT#1091-MD - STodd - Add Header Purge Criteria group allocation
     

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		public DataTable GetReservationHeaders()
		{
			try
			{
                return StoredProcedures.MID_HEADER_READ_FOR_RESERVATION_STORES.Read(_dba);
			}
			catch
			{
				throw;
			}
		}
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)


        public DataTable GetPacks(int headerRID)
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            return StoredProcedures.MID_HEADER_PACK_READ.Read(_dba, HDR_RID: headerRID);
        }

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public DataTable GetAssociatedPacks(int headerRID)
        {
            return StoredProcedures.MID_HEADER_PACK_ASSOCIATION_READ.Read(_dba, HDR_RID: headerRID);
        }

        public bool InsertAssociatedPack(int packRID, int seq, int headerRID,  int assocPackRID)
        {
            int rowsInserted = StoredProcedures.MID_HEADER_PACK_ASSOCIATION_INSERT.Insert(_dba,
                                                                             HDR_PACK_RID: packRID,
                                                                             SEQ: seq,
                                                                             HDR_RID: headerRID,
                                                                             ASSOCIATED_PACK_RID: assocPackRID
                                                                             );
            return (rowsInserted > 0);
        }

        public bool DeleteAssociatedPacks(int headerRID)
        {
            try
            {
                StoredProcedures.MID_HEADER_PACK_ASSOCIATION_DELETE.Delete(_dba, HDR_RID: headerRID);
                return true;
            }
            catch
            {
                throw;
            }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment

        public DataTable GetDistinctPacks()
        {
            //begin MID Track # 2354 - removed nolock because it causes concurrency issues   
            return StoredProcedures.MID_HEADER_PACK_READ_DISTINCT_NAMES.Read(_dba);
        }

        public DataTable GetPackColors()
        {
            try
            {
                //MID Track # 2354 - removed nolock because it causes concurrency issues     
                return StoredProcedures.MID_HEADER_PACK_COLOR_READ_ALL.Read(_dba);
            }
            catch
            {
                throw;
            }
        }

      

        // Begin Track #5841 - JSmith - Performance
        public DataTable GetPackColorsForHeader(int aHeaderRID)
        {
            return StoredProcedures.MID_HEADER_PACK_COLOR_READ.Read(_dba, HDR_RID: aHeaderRID);
        }
        // End Track #5841

        // BEGIN Workspace Usability Enhancement - Ron Matelic
        public int GetPackCount(int headerRID)
        {
            return StoredProcedures.MID_HEADER_PACK_READ_COUNT.ReadRecordCount(_dba, HDR_RID: headerRID);
        }

        public int GetBulkColorCount(int headerRID)
        {
            return StoredProcedures.MID_HEADER_BULK_COLOR_READ_COUNT.ReadRecordCount(_dba, HDR_RID: headerRID);
        }

        public int GetBulkColorSizeCount(int headerRID)
        {

            //TT871 - RMatelic - Header Characteristic #Bulk Sizes incorrect.  Header has 2 components each with 6 sizes the #Bulk Sizes Header Characteristic says 12.
            return StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_READ_COUNT.ReadRecordCount(_dba, HDR_RID: headerRID);
        }    
        // END Workspace Usability Enhancement 


		public DataTable GetBulkColors(int headerRID)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            
            return StoredProcedures.MID_HEADER_BULK_COLOR_READ.Read(_dba, HDR_RID: headerRID);
		}

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance

        /// <summary>
        /// Gets all pack data for a header in one dataset for a header
        /// </summary>
        /// <param name="headerRid"></param>
        /// <returns></returns>
        public DataSet GetPackDataForHeader(int headerRid)
        {
            try
            { 
                return StoredProcedures.MID_GET_PACK_DATA_FOR_HEADER.ReadAsDataSet(_dba, HDR_RID: headerRid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all bulk color and bulk size data in one dataset for a header
        /// </summary>
        /// <param name="headerRid"></param>
        /// <returns></returns>
        public DataSet GetBulkColorAndSizeDataForHeader(int headerRid)
        {
            try
            {
                return StoredProcedures.MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER.ReadAsDataSet(_dba, HDR_RID: headerRid);
            }
            catch
            {
                throw;
            }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

       

        public DataTable GetPackColorSizesForHeader(int aHeaderRID)
        {
            return StoredProcedures.MID_HEADER_PACK_COLOR_SIZE_READ.Read(_dba, HDR_RID: aHeaderRID);
        }

        public bool PackSizesExist(int headerRID)
        {
            bool sizesExist = false;
            int packRID;
            try
            {
                DataTable packTable = GetPacks(headerRID);
                foreach (DataRow dr in packTable.Rows)
                {
                    packRID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);

                    if (PackColorSizesExist(packRID))
                    {
                        sizesExist = true;
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return sizesExist;
        }

        public bool PackColorSizesExist(int packRID)
        {    
            int count = StoredProcedures.MID_HEADER_PACK_COLOR_SIZE_READ_COUNT.ReadRecordCount(_dba, HDR_PACK_RID: packRID);

            return count > 0 ? true : false;
        }

        public DataTable GetCharacteristics(int headerRID, bool getUnjoinedChars)
        {
            return StoredProcedures.SP_MID_HEADER_CHAR_DATA.Read(_dba, 
                                                                  HDR_RID: headerRID,
                                                                  GetUnjoinedChars: Include.ConvertBoolToChar(getUnjoinedChars)
                                                                 );
        }
       

        // Begin Track #5841 - JSmith - Performance
        public DataTable GetBulkColorSizesForHeader(int aHeaderRID)
        {
            return StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_READ.Read(_dba, HDR_RID: aHeaderRID);    
        }
        // End Track #5841

        public bool BulkColorSizesExist(int headerRID)
        {
            int count = StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_EXISTS.ReadRecordCount(_dba, HDR_RID: headerRID);

            return count > 0 ? true : false;
        }
        
        // Begin TT#739-MD -JSmith - Delete Stores
        public void GetHeaderAllocation(int aHeaderRID)
        {
            try
            {
                _dsHeaderAllocation = StoredProcedures.SP_MID_GET_HEADER_ALLOCATION.ReadAsDataSet(_dba, HDR_RID: aHeaderRID);


                _dsHeaderAllocation.Tables[0].TableName = "TotalAllocation";
                _dsHeaderAllocation.Tables[1].TableName = "DetailAllocation";
                _dsHeaderAllocation.Tables[2].TableName = "BulkAllocation";
                _dsHeaderAllocation.Tables[3].TableName = "BulkColorAllocation";
                _dsHeaderAllocation.Tables[4].TableName = "BulkColorSizeAllocation";
                _dsHeaderAllocation.Tables[5].TableName = "PackAllocation";
  
                _currHeaderRID = aHeaderRID;
            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                throw;
            }
        }
        // End TT#739-MD -JSmith - Delete Stores

        // Allocation Gets
        // begin MID Track 3994 Performance
        public StoreAllocationQuickRequest GetTotalAllocation(int aHeaderRID)
        {

            StoreAllocationQuickRequest saqr =
               new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.TotalAllocation, aHeaderRID, Include.NoRID, Include.NoRID, Include.NoRID);
            System.Data.DataTable dt;
            {
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["TotalAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue  // TT#2478 - Jellis - Performance // TT#2630 - Intransit Releive Process Locks
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        int storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        if (dr["SHIP_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreShipToDay(
                                eAllocationSqlAccessRequest.TotalAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["SHIP_DAY"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreGradeIndex(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            Convert.ToInt32(dr["STORE_GRADE_INDEX"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreCapacity(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            //Convert.ToInt32(dr["STORE_CAPACITY"], CultureInfo.CurrentUICulture),     // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                             (dr["STORE_CAPACITY"] != DBNull.Value) ? Convert.ToInt32(dr["STORE_CAPACITY"], CultureInfo.CurrentUICulture) : int.MaxValue, // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                            Convert.ToInt32(dr["EXCEED_CAPACITY_PERCENT"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreGeneralAudit(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            Convert.ToInt32(dr["ALLOC_STORE_GEN_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));
                        qtyAllocated = Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            qtyAllocated);                                                             // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));     // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreRule(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //(eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture),
                            (dr["CHOSEN_RULE_TYPE_ID"] != DBNull.Value) ? (eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture) : eRuleType.None,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["CHOSEN_RULE_UNITS"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
                        if (dr["NEED_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreNeedAudit(
                                eAllocationSqlAccessRequest.TotalAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture),
                                Convert.ToInt32(dr["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture),
                                Convert.ToDouble(dr["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                             Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin MID Track 5694 MA Enhancement Relieve IT by Header ID (also fix orphan IT)
                        //saqr.AddStoreShipping(
                        //	eAllocationSqlAccessRequest.TotalAllocation,
                        //	storeRID,
                        //	Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture),
                        //	Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreShipping(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
                        // end MID Track 5694 MA Enhancement Relive IT by Header ID (also fix orphan IT)
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        // begin TT#2478 - Jellis - Performance
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.TotalAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                            // end TT#2478 - JEllis - Performance
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.TotalAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.TotalAllocation,
                                storeRID,
                                0);
                        }
                        saqr.AddStoreImoAllocation(
                            eAllocationSqlAccessRequest.TotalAllocation,
                            storeRID,
                            //imoMaxValue,                                                             // TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue  // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2                                                    
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture),      // TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                            (dr["IMO_MAX_VALUE"] != DBNull.Value) ? Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture) : int.MaxValue,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["IMO_MIN_SHIP_QTY"], CultureInfo.CurrentUICulture),
                            Convert.ToDouble(dr["IMO_PCT_PK_THRSHLD"], CultureInfo.CurrentUICulture));
                    // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                    // end TT#114 MD JEllis - AnF VSW (TT#2440) - After Relieve IT VSW disappears (TT#184)
                    }
                }
                else
                {
                    saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(false);
                }
            }
            return saqr;
        }


        public StoreAllocationQuickRequest GetDetailAllocation(int aHeaderRID)
        {
            StoreAllocationQuickRequest saqr =
               new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.DetailAllocation, aHeaderRID, Include.NoRID, Include.NoRID, Include.NoRID);
            // End Assortment: Color/Size Changes
            System.Data.DataTable dt;
            {
                
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["DetailAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        int storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        qtyAllocated = Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            qtyAllocated);                                                         // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture)); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreRule(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //(eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture),
                            (dr["CHOSEN_RULE_TYPE_ID"] != DBNull.Value) ? (eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture) : eRuleType.None,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["CHOSEN_RULE_UNITS"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
                        if (dr["NEED_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreNeedAudit(
                                eAllocationSqlAccessRequest.DetailAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture),
                                Convert.ToInt32(dr["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture),
                                Convert.ToDouble(dr["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.DetailAllocation,
                            storeRID,
                            Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - Jellis - Performance
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.DetailAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - Jellis - Performance
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.DetailAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.DetailAllocation,
                                storeRID,
                                0);
                        }
                        // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // end TT#114 - MD JEllis - After Release IT VSW disappears (TT#184)
                    }
                }
                else
                {
                    saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(false);
                }
            }
            return saqr;
        }

        public Dictionary<int, StoreAllocationQuickRequest> GetPackAllocation(int aHeaderRID)
        {
            
            Dictionary<int, StoreAllocationQuickRequest> packStoreAllocations = new Dictionary<int, StoreAllocationQuickRequest>();
            StoreAllocationQuickRequest saqr;
            System.Data.DataTable dt;
            int packRID;
            {
                
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["PackAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    int storeRID;
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        packRID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
                        // Begin Performance
                        //if (packStoreAllocations.Contains(packRID))
                        //{
                        //	saqr = (StoreAllocationQuickRequest)packStoreAllocations[packRID];
                        //}
                        //else
                        try
                        {
                            saqr = packStoreAllocations[packRID];
                        }
                        catch (KeyNotFoundException)
                        // End Performance
                        {
                            // Begin Assortment: Color/Size Changes
                            //saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.PackAllocation, aHeaderRID, packRID, Include.NoRID, Include.NoRID, Include.NoRID);
                            saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.PackAllocation, aHeaderRID, packRID, Include.NoRID, Include.NoRID);
                            // End Assortment: Color/Size Changes
                            saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                            packStoreAllocations.Add(packRID, saqr);
                        }
                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        qtyAllocated = Convert.ToInt32(dr["PACKS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            qtyAllocated);                                                              // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["PACKS_ALLOCATED"], CultureInfo.CurrentUICulture));       // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2    
                        saqr.AddStoreRule(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //(eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture),
                            (dr["CHOSEN_RULE_TYPE_ID"] != DBNull.Value) ? (eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture) : eRuleType.None,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["CHOSEN_RULE_PACKS"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            Convert.ToInt32(dr["PACKS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["PACKS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
                        if (dr["NEED_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreNeedAudit(
                                eAllocationSqlAccessRequest.PackAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture),
                                Convert.ToInt32(dr["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture),
                                Convert.ToDouble(dr["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                             Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        saqr.AddStoreGeneralAudit(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            Convert.ToInt32(dr["ALLOC_STORE_GEN_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin MID Track 5694 MA Enhancement Relieve IT by Header ID (Also, fix orphan IT)
                        //saqr.AddStoreShipping(
                        //	eAllocationSqlAccessRequest.PackAllocation,
                        //	storeRID,
                        //	Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture),
                        //	Convert.ToInt32(dr["PACKS_SHIPPED"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreShipping(
                            eAllocationSqlAccessRequest.PackAllocation,
                            storeRID,
                            Convert.ToInt32(dr["PACKS_SHIPPED"], CultureInfo.CurrentUICulture),
                        Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
                        // end MID Track 5694 MA Enhancement Relieve IT by Header ID (Also, fix orphan IT)
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - Jellis - Performance
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_PACKS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.PackAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_PACKS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        if (dr["ITEM_PACKS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - Jellis - Performance
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.PackAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_PACKS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.PackAllocation,
                                storeRID,
                                0);
                        }
                        // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // end TT#114 - MD JEllis - After Release IT VSW disappears (TT#184)
                    }
                }
            }
            return packStoreAllocations;
        }

        public StoreAllocationQuickRequest GetBulkAllocation(int aHeaderRID)
        {
            StoreAllocationQuickRequest saqr =
                new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.BulkAllocation, aHeaderRID, Include.NoRID, Include.NoRID, Include.NoRID);
            System.Data.DataTable dt;
            {
                
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["BulkAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                    int storeRID;
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        qtyAllocated = Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            qtyAllocated);                                                                // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));         // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue  // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreRule(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //(eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture),
                            (dr["CHOSEN_RULE_TYPE_ID"] != DBNull.Value) ? (eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture) : eRuleType.None,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["CHOSEN_RULE_UNITS"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
                        if (dr["NEED_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreNeedAudit(
                                eAllocationSqlAccessRequest.BulkAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture),
                                Convert.ToInt32(dr["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture),
                                Convert.ToDouble(dr["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.BulkAllocation,
                            storeRID,
                             Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - Jellis - Performance
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.BulkAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - Jellis - Performance
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.BulkAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.BulkAllocation,
                                storeRID,
                                0);
                        }
                        // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // end TT#114 - MD JEllis - After Release IT VSW disappears (TT#184)
                    }
                }
            }
            return saqr;
        }

        public Dictionary<int, StoreAllocationQuickRequest> GetBulkColorAllocation(int aHeaderRID)
        {
           
            Dictionary<int, StoreAllocationQuickRequest> colorStoreAllocations = new Dictionary<int, StoreAllocationQuickRequest>();
            StoreAllocationQuickRequest saqr;
            System.Data.DataTable dt;
            int hdr_BC_RID;
            {
                
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["BulkColorAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    int storeRID;
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        hdr_BC_RID = Convert.ToInt32(dr["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                        // begin Assortment: use dictionary for faster response
                        //if (colorStoreAllocations.Contains(hdr_BC_RID))
                        //{
                        //saqr = (StoreAllocationQuickRequest)colorStoreAllocations[hdr_BC_RID]; // Assortment: use generic dictionary
                        //}
                        //else
                        try
                        {
                            saqr = colorStoreAllocations[hdr_BC_RID];
                        }
                        catch (KeyNotFoundException)
                        // end Assortment: use dictionary for faster response
                        {
                            // Begin Assortment: Color/Size Changes
                            //saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.ColorAllocation, aHeaderRID, Include.NoRID, colorCodeRID, Include.NoRID, Include.NoRID);
                            saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.ColorAllocation, aHeaderRID, Include.NoRID, hdr_BC_RID, Include.NoRID);
                            // End Assortment: Color/Size Changes
                            saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                            colorStoreAllocations.Add(hdr_BC_RID, saqr);
                        }
                        qtyAllocated = Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            qtyAllocated);                                                          // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));  // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreRule(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            // Begin TT#739-MD -JSmith - Delete Stores
                            //(eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture),
                            (dr["CHOSEN_RULE_TYPE_ID"] != DBNull.Value) ? (eRuleType)Convert.ToInt32(dr["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture) : eRuleType.None,
                            // End TT#739-MD -JSmith - Delete Stores
                            Convert.ToInt32(dr["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["CHOSEN_RULE_UNITS"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
                        if (dr["NEED_DAY"] != DBNull.Value)
                        {
                            saqr.AddStoreNeedAudit(
                                eAllocationSqlAccessRequest.ColorAllocation,
                                storeRID,
                                Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture),
                                Convert.ToInt32(dr["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture),
                                Convert.ToDouble(dr["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
                        }
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                             Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        saqr.AddStoreGeneralAudit(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            Convert.ToInt32(dr["ALLOC_STORE_GEN_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin mID Track 5694 MA Enhancement Relieve IT by Header ID (Also fix orphan IT)
                        //saqr.AddStoreShipping(
                        //	eAllocationSqlAccessRequest.ColorAllocation,
                        //	storeRID,
                        //	Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture),
                        //	Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreShipping(
                            eAllocationSqlAccessRequest.ColorAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
                        // end mID Track 5694 MA Enhancement Relieve IT by Header ID (Also fix orphan IT)
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - Jellis - Performance
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.ColorAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - Jellis - Performance
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.ColorAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.ColorAllocation,
                                storeRID,
                                0);
                        }
                        // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // end TT#114 - MD JEllis - After Release IT VSW disappears (TT#184)
                    }
                }
            }
            return colorStoreAllocations;
        }

        public Dictionary<long, StoreAllocationQuickRequest> GetBulkColorSizeAllocation(int aHeaderRID) // Assortment: use dictionary for faster reponse
        {
            Dictionary<long, StoreAllocationQuickRequest> colorSizeStoreAllocations = new Dictionary<long, StoreAllocationQuickRequest>();
            StoreAllocationQuickRequest saqr;
            System.Data.DataTable dt;
            {
               
                if (_dsHeaderAllocation == null ||
                    aHeaderRID != _currHeaderRID)
                {
                    GetHeaderAllocation(aHeaderRID);
                    _currHeaderRID = aHeaderRID;
                }
                dt = _dsHeaderAllocation.Tables["BulkColorSizeAllocation"];
                // End TT#739-MD -JSmith - Delete Stores
                if (dt.Rows.Count > 0)
                {
                    int storeRID;
                    //int colorCodeRID;  // Assortment: Color/size change
                    int hdrBCRID;
                    //int sizeCodeRID;   // Assortment: Color/size change
                    //int sequence;      // Assortment: Color/size change
                    int hdrBCSZKey;      // Assortment: Color/size change
                    long colorSizeKey;
                    //int imoMaxValue;    // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    int qtyAllocated;   // TT#2478 - JEllis AnF VSW - Intransit Overstated due to VSW Conversion Issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                        // Begin Assortment: Color/Size change
                        //colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                        //hdrBCRID = Convert.ToInt32(dr["BULK_COLOR_SIZE_ALLOCATION.HDR_BC_RID"], CultureInfo.CurrentUICulture);
                        //sequence = Convert.ToInt32(dr["BULK_COLOR_SIZE_ALLOCATION.SEQ"], CultureInfo.CurrentUICulture);
                        //sizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
                        //colorSizeKey = ((long)colorCodeRID << 32) + (long)sizeCodeRID;
                        hdrBCRID = Convert.ToInt32(dr["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                        hdrBCSZKey = Convert.ToInt32(dr["HDR_BCSZ_KEY"], CultureInfo.CurrentUICulture);
                        colorSizeKey = ((long)hdrBCRID << 32) + (long)hdrBCSZKey;
                        // End Assortment: Color/Size Change
                        // begin Assortment: use dictionary for faster response
                        //if (colorSizeStoreAllocations.Contains(colorSizeKey))
                        //{
                        //	saqr = (StoreAllocationQuickRequest)colorSizeStoreAllocations[colorSizeKey];
                        //}
                        //else
                        try
                        {
                            saqr = colorSizeStoreAllocations[colorSizeKey];
                        }
                        catch (KeyNotFoundException)
                        // end Assortment: use dictionary for faster response
                        {
                            // Begin Assortment: Color/Size Change
                            //saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.SizeAllocation, aHeaderRID, Include.NoRID, colorCodeRID, sizeCodeRID);
                            saqr = new StoreAllocationQuickRequest(eAllocationSqlAccessRequest.SizeAllocation, aHeaderRID, Include.NoRID, hdrBCRID, hdrBCSZKey);
                            // End Assortment: Color/Size Change
                            saqr.AllocationStructureStatus = new StoreAllocationStructureStatus(true);
                            colorSizeStoreAllocations.Add(colorSizeKey, saqr);
                        }
                        qtyAllocated = Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture); // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocation(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            qtyAllocated);                                                           // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //Convert.ToInt32(dr["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));   // TT#2478 - JEllis - AnF VSW - Intransit OVerstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        saqr.AddStoreAllocationAudit(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture),
                            0);
                        saqr.AddStoreMinimum(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreMaximum(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture));
                            (dr["MAXIMUM"] != DBNull.Value) ? Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStorePrimaryMax(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            // Begin TT#3263 - JSmith - Cancelled the allocation on a header
                            //Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
                            (dr["PRIMARY_MAX"] != DBNull.Value) ? Convert.ToInt32(dr["PRIMARY_MAX"], CultureInfo.CurrentUICulture) : int.MaxValue);
                            // End TT#3263 - JSmith - Cancelled the allocation on a header
                        saqr.AddStoreDetailAudit(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                             Convert.ToUInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));  // TT#488 - MD - Jellis - Group Allocation
                        //Convert.ToInt32(dr["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture)); // TT#488 - MD - Jellis - Group Allocation
                        // begin MID Track 5694 MA Enhancment Relieve IT by Header ID (also fix orphan IT)
                        //saqr.AddStoreShipping(
                        //	eAllocationSqlAccessRequest.SizeAllocation,
                        //	storeRID,
                        //	Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture),
                        //	Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture));
                        saqr.AddStoreShipping(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            Convert.ToInt32(dr["UNITS_SHIPPED"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
                        // end MID Track 5694 MA Enhancment Relieve IT by Header ID (also fix orphan IT)
                        // begin TT#114 - MD JEllis -  After Relieve IT VSW disappears (TT#184)
                        // begin TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // begin TT#2478 - Jellis - Performance
                        // begin TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        //imoMaxValue = Convert.ToInt32(dr["IMO_MAX_VALUE"], CultureInfo.CurrentUICulture);
                        ////if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        //if (imoMaxValue == int.MaxValue)
                        //{
                        //    saqr.AddStoreItemAllocation(
                        //        eAllocationSqlAccessRequest.SizeAllocation,
                        //        storeRID,
                        //        qtyAllocated);
                        //}
                        //else if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW Conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                        if (dr["ITEM_UNITS_ALLOCATED"] != DBNull.Value)
                        // end TT#2478 - Jellis - Performance
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.SizeAllocation,
                                storeRID,
                                Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
                        }
                        else
                        {
                            saqr.AddStoreItemAllocation(
                                eAllocationSqlAccessRequest.SizeAllocation,
                                storeRID,
                                0);
                        }
                        // end TT#2454 - Jellis - Relieve Intransit moves allocated to VSW
                        // end TT#114 - MD JEllis - After Release IT VSW disappears (TT#184)
                        // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum
                        saqr.AddStoreItemMinimum(
                            eAllocationSqlAccessRequest.SizeAllocation,
                            storeRID,
                            Convert.ToInt32(dr["ITEM_MINIMUM"], CultureInfo.CurrentUICulture),        // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
                            Convert.ToInt32(dr["ITEM_IDEAL_MINIMUM"], CultureInfo.CurrentUICulture)); // TT#246 - MD - JEllis - AnF VSW In STore Minimum phase 2
                           
                        // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum
                    }
                }
            }
            return colorSizeStoreAllocations;
        }

        public DataTable GetGrades(int headerRID)
        {
            //TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            //MID Track # 2354 - removed nolock because it causes concurrency issues
           
            return StoredProcedures.MID_HEADER_STORE_GRADE_READ.Read(_dba, HDR_RID: headerRID);
        }
        
        // begin MID Track # 2937 - Size OnHand Incorrect
        public DataTable GetDetailCurve(int headerRID)
        {
            return StoredProcedures.MID_HEADER_SIZE_NEED_READ.Read(_dba, HDR_RID: headerRID);
        }

        // Assortment BEGIN
        public DataTable GetBulkColorCurve(int headerRID)
        {   
            return StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_NEED_READ.Read(_dba, HDR_RID: headerRID);
        }
        // end MID Track # 2937 - Size OnHand Incorrect
        // Assortment END

        public bool DeleteSummaryNodeRules(int headerRID, eAllocationSummaryNode aAllocationSummaryNode)
        {
            if (aAllocationSummaryNode == eAllocationSummaryNode.Total)
            {
                try
                {

                    if (StoredProcedures.MID_RULE_TOTAL_DELETE.Delete(_dba, HDR_RID: headerRID) > 0)
                    {
                        return (StoredProcedures.MID_RULE_LAYER_TOTAL_DELETE.Delete(_dba, HDR_RID: headerRID) > 0);
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
             else if (aAllocationSummaryNode == eAllocationSummaryNode.DetailType)
            {
                try
                {
                    if (StoredProcedures.MID_RULE_DETAIL_DELETE.Delete(_dba, HDR_RID: headerRID) > 0)
                    {
                        return (StoredProcedures.MID_RULE_LAYER_DETAIL_DELETE.Delete(_dba, HDR_RID: headerRID) > 0);
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
             else if (aAllocationSummaryNode == eAllocationSummaryNode.Bulk)
            {
                try
                {
                    if (StoredProcedures.MID_RULE_BULK_DELETE.Delete(_dba, HDR_RID: headerRID) > 0)
                    {
                        return (StoredProcedures.MID_RULE_LAYER_BULK_DELETE.Delete(_dba, HDR_RID: headerRID) > 0);
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
            else
            {
                throw new Exception("Unknown allocation summary node");
            }

        }
        public bool DeletePackRules(int aPackRID)
        {
           
            try
            {
                if (StoredProcedures.MID_RULE_PACK_DELETE.Delete(_dba, HDR_PACK_RID: aPackRID) > 0)
                {
                    return (StoredProcedures.MID_RULE_LAYER_PACK_DELETE.Delete(_dba, HDR_PACK_RID: aPackRID) > 0);
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

        public bool DeleteColorRules(int aHDR_RID, int aHdrBCRID)
        {
           
            try
            {
                if (StoredProcedures.MID_RULE_BULK_COLOR_DELETE.Delete(_dba, HDR_BC_RID: aHdrBCRID) > 0)
                {
                    return (StoredProcedures.MID_RULE_LAYER_BULK_COLOR_DELETE.Delete(_dba, HDR_BC_RID: aHdrBCRID) > 0);
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
        /// Removes reference to a header in the RuleMethod
        /// </summary>
        /// <param name="headerRID">Header RID</param>
        /// <returns>True:  successful update; False update unsuccessful</returns>
        private bool UpdateRuleMethodStatus(int headerRID)
        {
            

            int rowsUpdated = StoredProcedures.MID_METHOD_RULE_UPDATE_STATUS.Update(_dba, 
                                                                                    METHOD_STATUS: (int)eMethodStatus.InvalidMethod,
                                                                                    HDR_RID: headerRID
                                                                                   );
            if (rowsUpdated >= 0)
            {
                return (StoredProcedures.MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE.Update(_dba, HDR_RID: headerRID) >= 0);
            }
            return false;
        }


        public int DeleteMultiHeaders(int aHdrRID)
        {
            try
            {
                return StoredProcedures.SP_MID_MULTI_HEADER_DELETE.Delete(_dba, 
                                                                           HDR_RID: aHdrRID,
                                                                           debug: 0
                                                                           );
            }
            catch
            {
                throw;
            }
        }
        //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type

        public void DeleteHeader(int headerRID)
        {
            try
            {
                if (UpdateRuleMethodStatus(headerRID))
                {
                    StoredProcedures.SP_MID_HEADER_DELETE.Delete(_dba, HDR_RID: headerRID);
                }
            }
            catch
            {
                throw;
            }
        }

        // begin TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
        // This is not used
        //// BEGIN TT#1401 - GTaylor - Reservation Stores
        ////  created this override to support code without the updated code.
        //public int CreateHeader(string headerID, string headerDesc, DateTime headerDay, DateTime originalDay,
        //    double unitRetail, double unitCost, int unitsReceived, int styleHnRID, int planHnRID, int onHandHnRID, int bulkMultiple,
        //    int allocationMultiple, string vendor, string purchaseOrder, DateTime beginDay, DateTime needDay, DateTime shipToDay,
        //    DateTime releaseDateTime, DateTime releaseApprovedDateTime, int headerGroupRID, int sizeGroupRID, int workflowRID, int methodRID,
        //    int AllocationStatusFlags, int BalanceStatusFlags, int ShippingStatusFlags, int AllocationTypeFlags, int InTransitStatusFlags,
        //    double percentNeedLimit, double planPercentFactor, int reserveUnits, int gradeWeekCount, 
        //    string distributionCenter, string headerNotes, bool workflowTrigger, DateTime earliestShipDay, int apiWorkflowRID, bool apiWorkflowTrigger,
        //    int allocatedUnits, int origAllocatedUnits, int releaseCount, int rsvAllocatedUnits, int displayStatus, int displayType,
        //    int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, int aAllocationMultipleDefault, int gradeSGRID
        //)
        //    {
        //        return CreateHeader(headerID, headerDesc, headerDay, originalDay, unitRetail, unitCost, unitsReceived, styleHnRID, planHnRID, onHandHnRID, bulkMultiple,
        //            allocationMultiple, vendor, purchaseOrder, beginDay, needDay, shipToDay, releaseDateTime, releaseApprovedDateTime, headerGroupRID, sizeGroupRID, workflowRID, 
        //            methodRID, AllocationStatusFlags, BalanceStatusFlags, ShippingStatusFlags, AllocationTypeFlags, InTransitStatusFlags,
        //            percentNeedLimit, planPercentFactor, reserveUnits, gradeWeekCount, distributionCenter, headerNotes, workflowTrigger, earliestShipDay, 
        //            apiWorkflowRID, apiWorkflowTrigger, allocatedUnits, origAllocatedUnits, releaseCount, rsvAllocatedUnits, displayStatus, displayType,
        //            displayIntransit, displayShipStatus, aAsrtRID, aPlaceholderRID, aAsrtType, aAllocationMultipleDefault, gradeSGRID,                              
        //            Include.UndefinedPlaceholderSeq, Include.UndefinedHeaderSeq, null, 0,0
        //            );
        //    }
        //// END TT#1401 - GTaylor - Reservation Stores
        // end TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error

        public int CreateHeader(string headerID, string headerDesc, DateTime headerDay, DateTime originalDay,
            double unitRetail, double unitCost, int unitsReceived, int aStyleHnRID, int planHnRID, int onHandHnRID, int bulkMultiple, // TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
            int allocationMultiple, string vendor, string purchaseOrder, DateTime beginDay, DateTime needDay, DateTime shipToDay,
            DateTime releaseDateTime, DateTime releaseApprovedDateTime, int headerGroupRID, int sizeGroupRID, int workflowRID, int methodRID,
            int AllocationStatusFlags, int BalanceStatusFlags, int ShippingStatusFlags, int AllocationTypeFlags, int InTransitStatusFlags,
            double percentNeedLimit, double planPercentFactor, int reserveUnits, int gradeWeekCount, //int primarySecondaryHdrRID,
            // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
            string distributionCenter, string headerNotes, bool workflowTrigger, DateTime earliestShipDay, int apiWorkflowRID, bool apiWorkflowTrigger,
            //Begin Assortment - JSmith - Header Service
            //int allocatedUnits, int origAllocatedUnits, int releaseCount, int rsvAllocatedUnits)
            int allocatedUnits, int origAllocatedUnits, int releaseCount, int rsvAllocatedUnits, int displayStatus, int displayType,
            // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            //int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, int aAllocationMultipleDefault) // MID Track 5761 Allocation Multiple not saved to database
        	//End Assortment
            int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, int aAllocationMultipleDefault, int gradeSGRID,
			int aAsrtPlaceholderSeq, int aAsrtHeaderSeq,
            // BEGIN TT#1401 - GTaylor - Reservation Stores
            string imoID, int ITEM_UNITS_ALLOCATED, int ITEM_ORIG_UNITS_ALLOCATED,
            int UNITS_PER_CARTON    // TT#1652-MD - stodd - DC Carton Rounding
            // END TT#1401 - GTaylor - Reservation Stores
            )
            // End TT#618
        // (CSMITH) - END MID Track #3219
        {
            try
            {
                // Assortment BEGIN 
                object asrtRID;

                if (aAsrtRID < 1)
                {
                    asrtRID = null;
                }
                else
                {
                    asrtRID = aAsrtRID;
                }

                object placeholderRID;

                if (aPlaceholderRID < 1)
                {
                    placeholderRID = null;
                }
                else
                {
                    placeholderRID = aPlaceholderRID;
                }
                object asrtType;

                if (aAsrtType < 1)
                {
                    asrtType = null;
                }
                else
                {
                    asrtType = aAsrtType;
                }
				// Begin TT#1227 - stodd
				object asrtPlaceholderSeq;
				if (aAsrtPlaceholderSeq < 1)
				{
					asrtPlaceholderSeq = null;
				}
				else
				{
					asrtPlaceholderSeq = aAsrtPlaceholderSeq;
				}
				
				object asrtHeaderSeq;
				if (aAsrtHeaderSeq < 1)
				{
					asrtHeaderSeq = null;
				}
				else
				{
					asrtHeaderSeq = aAsrtHeaderSeq;
				}
				// End TT#1227 - stodd
                // Assortment END 
                // begin MID Track 5761 Allocation Multiple not saved to database
                object allocationMultipleDefault;
                if (aAllocationMultipleDefault < 1)
                {
                    allocationMultipleDefault = null;
                }
                else
                {
                    allocationMultipleDefault = aAllocationMultipleDefault;
                }
                // end MID Track 5761 Allocation Multiple not saved to database

                // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
                if (gradeSGRID < 1)
                {
                    gradeSGRID = Include.AllStoreGroupRID;
                }
                // End TT#618 
                // BEGIN TT#1401 - GTaylor - Reservation Stores

                // END TT#1401 - GTaylor - Reservation Stores

                // begin TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
                //object styleHnRID;
                //if (aStyleHnRID < 1)
                //{
                //    styleHnRID = null;
                //}
                //else
                //{
                //    styleHnRID = aStyleHnRID;
                //}
                //Begin TT#1268-MD -jsobek -5.4 Merge
                int? styleHnRID_Nullable = null;
                if (aStyleHnRID >= 1) styleHnRID_Nullable = aStyleHnRID;       
                //End TT#1268-MD -jsobek -5.4 Merge
                // end TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
               
                return StoredProcedures.SP_MID_HEADER_INSERT.InsertAndReturnRID(_dba, 
                                                                                 HDR_ID: headerID,
                                                                                 HDR_DESC: headerDesc,
                                                                                 HDR_DAY: headerDay,
                                                                                 ORIG_DAY: originalDay,
                                                                                 UNIT_RETAIL: unitRetail,
                                                                                 UNIT_COST: unitCost,
                                                                                 UNITS_RECEIVED: unitsReceived,
                                                                                 STYLE_HNRID: styleHnRID_Nullable, //TT#1268-MD -jsobek -5.4 Merge
                                                                                 PLAN_HNRID: planHnRID,
                                                                                 ON_HAND_HNRID: onHandHnRID,
                                                                                 BULK_MULTIPLE: bulkMultiple,
                                                                                 ALLOCATION_MULTIPLE: allocationMultiple,
                                                                                 VENDOR: vendor,
                                                                                 PURCHASE_ORDER: purchaseOrder,
                                                                                 BEGIN_DAY: beginDay,
                                                                                 NEED_DAY: needDay,
                                                                                 SHIP_TO_DAY: shipToDay,
                                                                                 RELEASE_DATETIME: releaseDateTime,
                                                                                 RELEASE_APPROVED_DATETIME: releaseApprovedDateTime,
                                                                                 HDR_GROUP_RID: headerGroupRID,
                                                                                 SIZE_GROUP_RID: sizeGroupRID,
                                                                                 WORKFLOW_RID: workflowRID,
                                                                                 METHOD_RID: methodRID,
                                                                                 ALLOCATION_STATUS_FLAGS: AllocationStatusFlags,
                                                                                 BALANCE_STATUS_FLAGS: BalanceStatusFlags,
                                                                                 SHIPPING_STATUS_FLAGS: ShippingStatusFlags,
                                                                                 ALLOCATION_TYPE_FLAGS: AllocationTypeFlags,
                                                                                 INTRANSIT_STATUS_FLAGS: InTransitStatusFlags,
                                                                                 PERCENT_NEED_LIMIT: percentNeedLimit,
                                                                                 PLAN_PERCENT_FACTOR: planPercentFactor,
                                                                                 RESERVE_UNITS: reserveUnits,
                                                                                 GRADE_WEEK_COUNT: gradeWeekCount,
                                                                                 DIST_CENTER: distributionCenter,
                                                                                 HEADER_NOTES: headerNotes,
                                                                                 WORKFLOW_TRIGGER: Include.ConvertBoolToChar(workflowTrigger),
                                                                                 EARLIEST_SHIP_DAY: earliestShipDay,
                                                                                 API_WORKFLOW_RID: apiWorkflowRID,
                                                                                 API_WORKFLOW_TRIGGER: Include.ConvertBoolToChar(apiWorkflowTrigger),
                                                                                 ALLOCATED_UNITS: allocatedUnits,
                                                                                 ORIG_ALLOCATED_UNITS: origAllocatedUnits,
                                                                                 RELEASE_COUNT: releaseCount,
                                                                                 RSV_ALLOCATED_UNITS: rsvAllocatedUnits,
                                                                                 DISPLAY_STATUS: displayStatus,
                                                                                 DISPLAY_TYPE: displayType,
                                                                                 DISPLAY_INTRANSIT: displayIntransit,
                                                                                 DISPLAY_SHIP_STATUS: displayShipStatus,
                                                                                 ASRT_RID: Include.ConvertObjectToNullableInt(asrtRID),
                                                                                 PLACEHOLDER_RID: Include.ConvertObjectToNullableInt(placeholderRID),
                                                                                 ASRT_TYPE: Include.ConvertObjectToNullableInt(asrtType),
                                                                                 ALLOCATION_MULTIPLE_DEFAULT: Include.ConvertObjectToNullableInt(allocationMultipleDefault),
                                                                                 GRADE_SG_RID: gradeSGRID,
                                                                                 ASRT_PLACEHOLDER_SEQ: Include.ConvertObjectToNullableInt(asrtPlaceholderSeq),
                                                                                 ASRT_HEADER_SEQ: Include.ConvertObjectToNullableInt(asrtHeaderSeq),
                                                                                 IMO_ID: imoID,
                                                                                 ITEM_UNITS_ALLOCATED: ITEM_UNITS_ALLOCATED,
                                                                                 ITEM_ORIG_UNITS_ALLOCATED: ITEM_ORIG_UNITS_ALLOCATED,
                                                                                 UNITS_PER_CARTON: UNITS_PER_CARTON     // TT#1652-MD - stodd - DC Carton Rounding
                                                                                 );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // begin TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
        // THIS is not used!
        //// BEGIN TT#1401 - GTaylor - Reservation Stores
        //// created this method to support code that has not been updated
        //public bool UpdateHeader(int headerRID, string headerID, string headerDesc, DateTime headerDay, DateTime originalDay,
        //    double unitRetail, double unitCost, int unitsReceived, int styleHnRID, int planHnRID, int onHandHnRID, int bulkMultiple,
        //    int allocationMultiple, string vendor, string purchaseOrder, DateTime beginDay, DateTime needDay, DateTime shipToDay,
        //    DateTime releaseDateTime, DateTime releaseApprovedDateTime, int headerGroupRID, int sizeGroupRID, int workflowRID, int methodRID,
        //    int AllocationStatusFlags, int BalanceStatusFlags, int ShippingStatusFlags, int AllocationTypeFlags, int InTransitStatusFlags,
        //    double percentNeedLimit, double planPercentFactor, int reserveUnits, int gradeWeekCount, 
        //    string distributionCenter, string headerNotes, bool workflowTrigger, DateTime earliestShipDay, int apiWorkflowRID, bool apiWorkflowTrigger,
        //    int allocatedUnits, int origAllocatedUnits, int releaseCount, int rsvAllocatedUnits, int strStylAloctnManualChgCnt, int strSizeAloctnManualChgCnt, 
        //    int storeStyleAllocationChangedTotal, int storeSizeAllocationChangedTotal, int storesWithAllocationCount, bool horizonOverride, 
        //    int displayStatus, int displayType, int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, 
        //    int aAllocationMultipleDefault, int gradeSGRID, bool aGradeInventoryInd, int aGradeInventoryHnRID
        //    )
        //{ 
        //    return UpdateHeader(headerRID, headerID, headerDesc, headerDay, originalDay, unitRetail, unitCost, unitsReceived, styleHnRID, planHnRID, onHandHnRID, bulkMultiple,
        //    allocationMultiple, vendor, purchaseOrder, beginDay, needDay, shipToDay, releaseDateTime, releaseApprovedDateTime, headerGroupRID, sizeGroupRID, workflowRID, methodRID,
        //    AllocationStatusFlags, BalanceStatusFlags, ShippingStatusFlags, AllocationTypeFlags,  InTransitStatusFlags,
        //    percentNeedLimit, planPercentFactor, reserveUnits, gradeWeekCount, distributionCenter, headerNotes, workflowTrigger, earliestShipDay, 
        //    apiWorkflowRID, apiWorkflowTrigger, allocatedUnits, origAllocatedUnits, releaseCount, rsvAllocatedUnits, strStylAloctnManualChgCnt, strSizeAloctnManualChgCnt, 
        //    storeStyleAllocationChangedTotal, storeSizeAllocationChangedTotal, storesWithAllocationCount, horizonOverride, displayStatus, displayType,
        //    displayIntransit, displayShipStatus, aAsrtRID, aPlaceholderRID, aAsrtType, aAllocationMultipleDefault, gradeSGRID, Include.UndefinedPlaceholderSeq, Include.UndefinedHeaderSeq,
        //    aGradeInventoryInd, aGradeInventoryHnRID, null, 0, 0
        //    );
        //}
        //// END TT#1401 - GTaylor - Reservation Stores
        // end TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error

        public bool UpdateHeader(int headerRID, string headerID, string headerDesc, DateTime headerDay, DateTime originalDay,
            double unitRetail, double unitCost, int unitsReceived, int aStyleHnRID, int planHnRID, int onHandHnRID, int bulkMultiple, // TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
            int allocationMultiple, string vendor, string purchaseOrder, DateTime beginDay, DateTime needDay, DateTime shipToDay,
            DateTime releaseDateTime, DateTime releaseApprovedDateTime, int headerGroupRID, int sizeGroupRID, int workflowRID, int methodRID,
            int AllocationStatusFlags, int BalanceStatusFlags, int ShippingStatusFlags, int AllocationTypeFlags, int InTransitStatusFlags,
            double percentNeedLimit, double planPercentFactor, int reserveUnits, int gradeWeekCount, //int primarySecondaryHdrRID,
            // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
            string distributionCenter, string headerNotes, bool workflowTrigger, DateTime earliestShipDay, int apiWorkflowRID, bool apiWorkflowTrigger,
            int allocatedUnits, int origAllocatedUnits, int releaseCount, int rsvAllocatedUnits, int strStylAloctnManualChgCnt, int strSizeAloctnManualChgCnt, // MID Track 4448 AnF Audit Enhancement
            int storeStyleAllocationChangedTotal, int storeSizeAllocationChangedTotal, int storesWithAllocationCount, bool horizonOverride, // MID Track 4448 ANF Audit Enhancement
            int displayStatus, int displayType,
            // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            //int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, int aAllocationMultipleDefault)  // MID Track 5761 Allocation Multiple not saved to database
             // (CSMITH) - END MID Track #3219
            int displayIntransit, int displayShipStatus, int aAsrtRID, int aPlaceholderRID, int aAsrtType, int aAllocationMultipleDefault, int gradeSGRID, int aAsrtPlaceholderSeq, int aAsrtHeaderSeq, // TT#1227 - stodd - assortment
            bool aGradeInventoryInd, int aGradeInventoryHnRID,                       // TT#1287 - Inventory Minimum and Maximum
            // BEGIN TT#1401 - GTaylor - Reservation Stores
            string imoID, int ITEM_UNITS_ALLOCATED, int ITEM_ORIG_UNITS_ALLOCATED,
            // END TT#1401 - GTaylor - Reservation Stores
            int UNITS_PER_CARTON,    // TT#1652-MD - stodd - DC Carton Rounding
            bool DC_FULFILLMENT_PROCESSED_IND  // TT#1966-MD - JSmith- DC Fulfillment
            )                                                                       // TT#1287 - Inventory Minimum and Maximum
            // End TT#618
        {
            // Assortment BEGIN 
            object asrtRID;

            if (aAsrtRID < 1)
            {
                asrtRID = null;
            }
            else
            {
                asrtRID = aAsrtRID;
            }

            object placeholderRID;

            if (aPlaceholderRID < 1)
            {
                placeholderRID = null;
            }
            else
            {
                placeholderRID = aPlaceholderRID;
            }

            object asrtType;

            if (aAsrtType < 1)
            {
                asrtType = null;
            }
            else
            {
                asrtType = aAsrtType;
            }
            // Assortment END 

			// Begin TT#1227 - stodd - assortment
			object asrtPlaceholderSeq;

			if (aAsrtPlaceholderSeq < 1)
            {
				asrtPlaceholderSeq = null;
            }
            else
            {
				asrtPlaceholderSeq = aAsrtPlaceholderSeq;
            }
			object asrtHeaderSeq;

			if (aAsrtHeaderSeq < 1)
            {
				asrtHeaderSeq = null;
            }
            else
            {
				asrtHeaderSeq = aAsrtHeaderSeq;
            }
			// End TT#1227 - stodd - assortment

            // begin MID Track 5761 Allocation Multiple not saved to database
            object allocationMultipleDefault;
            if (aAllocationMultipleDefault < 1)
            {
                allocationMultipleDefault = null;
            }
            else
            {
                allocationMultipleDefault = aAllocationMultipleDefault;
            }
            // end MID Track 5761 Allocation Multiple not saved to database

            // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            if (gradeSGRID < 1)
            {
                gradeSGRID = Include.AllStoreGroupRID;
            }
            // End TT#618 

            // begin TT#1287 - Inventory Minimum and Maximum
            object gradeInventoryHnRID = null;
            if (aGradeInventoryHnRID > 0)
            {
                gradeInventoryHnRID = aGradeInventoryHnRID;
            }
            // end TT#1287 - Inventory Minimum and Maximum

            // begin TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
            //object styleHnRID;
            //if (aStyleHnRID < 1)
            //{
            //    styleHnRID = null;
            //}
            //else
            //{
            //    styleHnRID = aStyleHnRID;
            //}
            //Begin TT#1268-MD -jsobek -5.4 Merge
            int? styleHnRID_Nullable = null;
            if (aStyleHnRID >= 1) styleHnRID_Nullable = aStyleHnRID;
            //End TT#1268-MD -jsobek -5.4 Merge
            // end TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error
           


            int rowsUpdated = StoredProcedures.MID_HEADER_UPDATE.Update(_dba, 
                                                          HDR_RID: headerRID,
                                                          HDR_ID: headerID,
                                                          HDR_DESC: headerDesc,
                                                          HDR_DAY: headerDay,
                                                          ORIG_DAY: originalDay,
                                                          UNIT_RETAIL: unitRetail,
                                                          UNIT_COST: unitCost,
                                                          UNITS_RECEIVED: unitsReceived,
                                                          STYLE_HNRID: styleHnRID_Nullable, //TT#1268-MD -jsobek -5.4 Merge
                                                          PLAN_HNRID: planHnRID,
                                                          ON_HAND_HNRID: onHandHnRID,
                                                          BULK_MULTIPLE: bulkMultiple,
                                                          ALLOCATION_MULTIPLE: allocationMultiple,
                                                          VENDOR: vendor,
                                                          PURCHASE_ORDER: purchaseOrder,
                                                          BEGIN_DAY: beginDay,
                                                          NEED_DAY: needDay,
                                                          SHIP_TO_DAY: shipToDay,
                                                          RELEASE_DATETIME: releaseDateTime,
                                                          RELEASE_APPROVED_DATETIME: releaseApprovedDateTime,
                                                          HDR_GROUP_RID: headerGroupRID,
                                                          SIZE_GROUP_RID: sizeGroupRID,
                                                          WORKFLOW_RID: workflowRID,
                                                          METHOD_RID: methodRID,
                                                          ALLOCATION_STATUS_FLAGS: AllocationStatusFlags,
                                                          BALANCE_STATUS_FLAGS: BalanceStatusFlags,
                                                          SHIPPING_STATUS_FLAGS: ShippingStatusFlags,
                                                          ALLOCATION_TYPE_FLAGS: AllocationTypeFlags,
                                                          INTRANSIT_STATUS_FLAGS: InTransitStatusFlags,
                                                          PERCENT_NEED_LIMIT: percentNeedLimit,
                                                          PLAN_PERCENT_FACTOR: planPercentFactor,
                                                          RESERVE_UNITS: reserveUnits,
                                                          GRADE_WEEK_COUNT: gradeWeekCount,
                                                          DIST_CENTER: distributionCenter,
                                                          HEADER_NOTES: headerNotes,
                                                          WORKFLOW_TRIGGER: Include.ConvertBoolToChar(workflowTrigger),
                                                          EARLIEST_SHIP_DAY: earliestShipDay,
                                                          API_WORKFLOW_RID: apiWorkflowRID,
                                                          API_WORKFLOW_TRIGGER: Include.ConvertBoolToChar(apiWorkflowTrigger),
                                                          ALLOCATED_UNITS: allocatedUnits,
                                                          ORIG_ALLOCATED_UNITS: origAllocatedUnits,
                                                          RELEASE_COUNT: releaseCount,
                                                          RSV_ALLOCATED_UNITS: rsvAllocatedUnits,
                                                          StrStylAloctnManualChgCnt: strStylAloctnManualChgCnt,
                                                          StrSizeAloctnManualChgCnt: strSizeAloctnManualChgCnt,
                                                          storeStyleAllocationChangedTotal: storeStyleAllocationChangedTotal,
                                                          storeSizeAllocationChangedTotal: storeSizeAllocationChangedTotal,
                                                          storesWithAllocationCount: storesWithAllocationCount,
                                                          horizonOverride: Include.ConvertBoolToChar(horizonOverride),
                                                          DISPLAY_STATUS: displayStatus,
                                                          DISPLAY_TYPE: displayType,
                                                          DISPLAY_INTRANSIT: displayIntransit,
                                                          DISPLAY_SHIP_STATUS: displayShipStatus,
                                                          ASRT_RID: Include.ConvertObjectToNullableInt(asrtRID),
                                                          PLACEHOLDER_RID: Include.ConvertObjectToNullableInt(placeholderRID),
                                                          ASRT_TYPE: Include.ConvertObjectToNullableInt(asrtType),
                                                          ALLOCATION_MULTIPLE_DEFAULT: Include.ConvertObjectToNullableInt(allocationMultipleDefault),
                                                          GRADE_SG_RID: gradeSGRID,
                                                          ASRT_PLACEHOLDER_SEQ: Include.ConvertObjectToNullableInt(asrtPlaceholderSeq),
                                                          ASRT_HEADER_SEQ: Include.ConvertObjectToNullableInt(asrtHeaderSeq),
                                                          GRADE_INVENTORY_IND: Include.ConvertBoolToChar(aGradeInventoryInd),
                                                          GRADE_INVENTORY_HNRID: Include.ConvertObjectToNullableInt(gradeInventoryHnRID),
                                                          IMO_ID: imoID,
                                                          ITEM_UNITS_ALLOCATED: ITEM_UNITS_ALLOCATED,
                                                          ITEM_ORIG_UNITS_ALLOCATED: ITEM_ORIG_UNITS_ALLOCATED,
                                                          UNITS_PER_CARTON: UNITS_PER_CARTON,    // TT#1652-MD - stodd - DC Carton Rounding
                                                          DC_FULFILLMENT_PROCESSED_IND: Include.ConvertBoolToChar(DC_FULFILLMENT_PROCESSED_IND)  /* TT#1966-MD - JSmith- DC Fulfillment  */
                                                          );
            return (rowsUpdated > 0);
        }

        /// <summary>
        /// Writes the grade table for a header to the database
        /// </summary>
        /// <param name="headerRID">RID of the associated header</param>
        /// <param name="agradeCode">Array of grade codes (syncronized with the other arrays)</param>
        /// <param name="aBoundary">Array of boundaries (syncronized witht the other arrays)</param>
        /// <param name="aStockMinimum">Array of Stock Minimums (syncronized with the other arrays)</param>
        /// <param name="aAdMinimum">Array of Ad Minimums (syncronized with the other arrays)</param>
        /// <param name="aStockMaximum">Array of Stock Maximums (syncronized with the other arrays)</param>
        /// <param name="aColorMinimum">Array of Color Minimums (syncronized with the other arrays)</param>
        /// <param name="aColorMaximum">Array of Color Maximums (syncronized with the other arrays)</param>
        /// <returns>True when update is successful</returns>
        public bool WriteGrades(
            int headerRID,
            string[] aGradeCode,
            double[] aBoundary,
            int[] aStockMinimum,
            int[] aAdMinimum,
            int[] aStockMaximum,
            int[] aColorMinimum,
			int[] aColorMaximum,
			int[] aShipUpTo, // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
            int[] sgl_RID)     // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        {
            if (DeleteGrades(headerRID))
            {
                // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
                SortedList boundarySL = new SortedList();
                for (int i = 0; i < aBoundary.Length; i++)
                {
                    if (!boundarySL.Contains(aBoundary[i]))
                    {
                        boundarySL.Add(aBoundary[i], aGradeCode[i]);
                    }
                }
             
                foreach (double boundary in boundarySL.Keys)
                {
                    string grade = boundarySL[boundary].ToString();
                   
                    int rowsInserted = StoredProcedures.MID_HEADER_STORE_GRADE_INSERT.Insert(_dba, 
                                                                                             HDR_RID: headerRID,
                                                                                             BOUNDARY: boundary,
                                                                                             GRADE_CODE: grade
                                                                                             );
                    if (!(rowsInserted > 0))
                    {
                        return false;
                    }
                }

               
                for (int i = 0; i < aGradeCode.Length; i++)
                {
                 
                    int rowsInserted = StoredProcedures.MID_HEADER_STORE_GRADE_VALUES_INSERT.Insert(_dba, 
                                                                                                    HDR_RID: headerRID,
                                                                                                    BOUNDARY: aBoundary[i],
                                                                                                    MINIMUM_STOCK: aStockMinimum[i],
                                                                                                    MAXIMUM_STOCK: aStockMaximum[i],
                                                                                                    MINIMUM_AD: aAdMinimum[i],
                                                                                                    MINIMUM_COLOR: aColorMinimum[i],
                                                                                                    MAXIMUM_COLOR: aColorMaximum[i],
                                                                                                    SHIP_UP_TO: aShipUpTo[i],
                                                                                                    SGL_RID: sgl_RID[i]
                                                                                                    );
                    if (!(rowsInserted > 0))
                    {
                        return false;
                    }
                }
                // End TT#618
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes grade information for a given header RID
        /// </summary>
        /// <param name="headerRID">RID of the header</param>
        /// <returns>True if delete successful</returns>
        public bool DeleteGrades(int headerRID)
        {    
            if (DeleteHeaderGradeValues(headerRID))
            {
                return (StoredProcedures.MID_HEADER_STORE_GRADE_DELETE.Delete(_dba, HDR_RID: headerRID) >= 0);
            }
            else
            {
                return false;
            }
        }   // End TT#618

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        public bool DeleteHeaderGradeValues(int headerRID)
        {
            
            return (StoredProcedures.MID_HEADER_STORE_GRADE_VALUES_DELETE.Delete(_dba, HDR_RID: headerRID) >= 0);
        }
        // End TT#618

        // BEGIN MID Track #2937 Size OnHand Incorrect
        /// <summary>
        /// Writes the header detail curves to the database
        /// </summary>
        /// <param name="headerRID">RID of the associated header</param>
        /// <param name="aSizeCurveGroupRID">Array of grade codes (syncronized with the other arrays)</param>
        /// <param name="aMerchType">Array of boundaries (syncronized witht the other arrays)</param>
        /// <param name="aHnRID">Array of Stock Minimums (syncronized with the other arrays)</param>
        /// <param name="aMerchPhRID">Array of Ad Minimums (syncronized with the other arrays)</param>
        /// <param name="aMerchPhlSeq">Array of Stock Maximums (syncronized with the other arrays)</param>
        /// <param name="aSizeConstraintRID">Array of Color Minimums (syncronized with the other arrays)</param>
        /// <param name="aSizeAlternateRID">Array of Color Maximums (syncronized with the other arrays)</param>
        /// <param name="aNormalizeSizeCurve">True: Size Curves are normalized; False: Size Curves are not normalized</param>
        /// <returns>True when update is successful</returns>
        public bool WriteHeaderDetailCurve(
            int headerRID,
            int aSizeCurveGroupRID,
            int aMerchType,
            int aHnRID,
            int aMerchPhRID,
            int aMerchPhlSeq,
            int aSizeConstraintRID,
            int aSizeAlternateRID,
            // begin TT#41 - MD - JEllis - Size Inventory Min Max pt 1
            bool aNormalizeSizeCurve,
            int aIB_MerchType,
            int aIB_MerchHnRID,
            int aIB_MerchPhRID,
            int aIB_MerchPhlSeq, // TT#246 - MD - Jellis - VSW Size In Store Minimums pt 3
            eVSWSizeConstraints aVswSizeConstraints) // TT#246 - MD - Jellis - VSW Size In Store Minimums pt 3
            //bool aNormalizeSizeCurve) // MID Track 4861 Size Curve Normalization
            // end TT#41 - MD - JEllis - Size Inventory Min Max pt 1
        {
            if (DeleteHeaderDetailCurves(headerRID))
            {
                

                int? aSizeCurveGroupRID_Nullable = null;
                if (aSizeCurveGroupRID >= 0) aSizeCurveGroupRID_Nullable = aSizeCurveGroupRID;

                int? aHnRID_Nullable = null;
                if (aHnRID >= 0) aHnRID_Nullable = aHnRID;

                int? aMerchPhRID_Nullable = null;
                if (aMerchPhRID >= 0) aMerchPhRID_Nullable = aMerchPhRID;
                
                int? aMerchPhlSeq_Nullable = null;
                if (aMerchPhlSeq > 0) aMerchPhlSeq_Nullable = aMerchPhlSeq;
                
                int? aSizeConstraintRID_Nullable = null;
                if (aSizeConstraintRID >= 0) aSizeConstraintRID_Nullable = aSizeConstraintRID;

                int? aSizeAlternateRID_Nullable = null;
                if (aSizeAlternateRID >= 0) aSizeAlternateRID_Nullable = aSizeAlternateRID;

                int? aIB_MerchHnRID_Nullable = null;
                if (aIB_MerchHnRID >= 0) aIB_MerchHnRID_Nullable = aIB_MerchHnRID;

                int? aIB_MerchPhRID_Nullable = null;
                if (aIB_MerchPhRID >= 0) aIB_MerchPhRID_Nullable = aIB_MerchPhRID;

                int? aIB_MerchPhlSeq_Nullable = null;
                if (aIB_MerchPhlSeq > 0) aIB_MerchPhlSeq_Nullable = aIB_MerchPhlSeq;

                int? aVswSizeConstraints_Nullable = null;
                if (aVswSizeConstraints != eVSWSizeConstraints.None) aVswSizeConstraints_Nullable = (int)aVswSizeConstraints;
      
        
                int rowsInserted = StoredProcedures.MID_HEADER_SIZE_NEED_INSERT.Insert(_dba, 
                                                                                        HDR_RID: headerRID,
                                                                                        SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID_Nullable,
                                                                                        MERCH_TYPE: aMerchType,
                                                                                        HN_RID: aHnRID_Nullable,
                                                                                        PH_RID: aMerchPhRID_Nullable,
                                                                                        PHL_SEQUENCE: aMerchPhlSeq_Nullable,
                                                                                        SIZE_CONSTRAINT_RID: aSizeConstraintRID_Nullable,
                                                                                        SIZE_ALTERNATE_RID: aSizeAlternateRID_Nullable,
                                                                                        NORMALIZE_SIZE_CURVES_IND: Include.ConvertBoolToChar(aNormalizeSizeCurve),
                                                                                        IB_MERCH_TYPE: aIB_MerchType,
                                                                                        IB_MERCH_HN_RID: aIB_MerchHnRID_Nullable,
                                                                                        IB_MERCH_PH_RID: aIB_MerchPhRID_Nullable,
                                                                                        IB_MERCH_PHL_SEQUENCE: aIB_MerchPhlSeq_Nullable,
                                                                                        VSW_SIZE_CONSTRAINTS: aVswSizeConstraints_Nullable
                                                                                        );
                if (!(rowsInserted > 0))
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes Header Detail Curves for a give header RID
        /// </summary>
        /// <param name="headerRID">RID of the header</param>
        /// <returns>True if delete successful</returns>
        public bool DeleteHeaderDetailCurves(int headerRID)
        {
            return (StoredProcedures.MID_HEADER_SIZE_NEED_DELETE.Delete(_dba, HDR_RID: headerRID) >= 0);
        }

        /// <summary>
        /// Writes the header detail curves to the database
        /// </summary>
        /// <param name="headerRID">RID of the associated header</param>
        /// <param name="aSizeCurveGroupRID">Array of grade codes (syncronized with the other arrays)</param>
        /// <param name="aMerchType">Array of boundaries (syncronized witht the other arrays)</param>
        /// <param name="aHnRID">Array of Stock Minimums (syncronized with the other arrays)</param>
        /// <param name="aMerchPhRID">Array of Ad Minimums (syncronized with the other arrays)</param>
        /// <param name="aMerchPhlSeq">Array of Stock Maximums (syncronized with the other arrays)</param>
        /// <param name="aSizeConstraintRID">Array of Color Minimums (syncronized with the other arrays)</param>
        /// <param name="aAlternateRID">Array of Color Maximums (syncronized with the other arrays)</param>
        /// <param name="aNormalizeSizeCurve">True: Size Curves are normalized; False: Size Curves are not normalized</param>
        /// <returns>True when update is successful</returns>
        public bool WriteHeaderColorCurve(
            int headerRID,
            //int aColorRID,        // Assortment replace aColorRID with aHdrBCRID 
            int aHdrBCRID,
            int aSizeCurveGroupRID,
            int aMerchType,
            int aHnRID,
            int aMerchPhRID,
            int aMerchPhlSeq,
            int aSizeConstraintRID,
            int aSizeAlternateRID, // MID Track 4861 Size Curve Normalization
            // begin TT#41 - MD - JEllis - Size Inventory Min Max pt 1
            bool aNormalizeSizeCurve,
            int aIB_MerchType,
            int aIB_MerchHnRID,
            int aIB_MerchPhRID,
            int aIB_MerchPhlSeq, // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
            eVSWSizeConstraints aVswSizeConstraints,
            eFillSizesToType aFillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
            ) // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
            //bool aNormalizeSizeCurve) // MID Track 4861 Size Curve Normalization
            // end TT#41 - MD - JEllis - Size Inventory Min Max pt 1
        {
            if (DeleteHeaderColorCurves(headerRID, aHdrBCRID))
            {
               

                int? aSizeCurveGroupRID_Nullable = null;
                if (aSizeCurveGroupRID >= 0) aSizeCurveGroupRID_Nullable = aSizeCurveGroupRID;

                int? aHnRID_Nullable = null;
                if (aHnRID >= 0) aHnRID_Nullable = aHnRID;

                int? aMerchPhRID_Nullable = null;
                if (aMerchPhRID >= 0) aMerchPhRID_Nullable = aMerchPhRID;

                int? aMerchPhlSeq_Nullable = null;
                if (aMerchPhlSeq > 0) aMerchPhlSeq_Nullable = aMerchPhlSeq;

                int? aSizeConstraintRID_Nullable = null;
                if (aSizeConstraintRID >= 0) aSizeConstraintRID_Nullable = aSizeConstraintRID;

                int? aSizeAlternateRID_Nullable = null;
                if (aSizeAlternateRID >= 0) aSizeAlternateRID_Nullable = aSizeAlternateRID;

                int? aIB_MerchHnRID_Nullable = null;
                if (aIB_MerchHnRID >= 0) aIB_MerchHnRID_Nullable = aIB_MerchHnRID;

                int? aIB_MerchPhRID_Nullable = null;
                if (aIB_MerchPhRID >= 0) aIB_MerchPhRID_Nullable = aIB_MerchPhRID;

                int? aIB_MerchPhlSeq_Nullable = null;
                if (aIB_MerchPhlSeq > 0) aIB_MerchPhlSeq_Nullable = aIB_MerchPhlSeq;

                int? aVswSizeConstraints_Nullable = null;
                if (aVswSizeConstraints != eVSWSizeConstraints.None) aVswSizeConstraints_Nullable = (int)aVswSizeConstraints;

              
              
				// BEGIN TT#3199-MD - STodd - Relieve Intransit of 100% VSW headers
				try
				{
                    int rowsInserted = StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT.Insert(_dba, 
                                                                                                       HDR_RID: headerRID,
                                                                                                       HDR_BC_RID: aHdrBCRID,
                                                                                                       SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID_Nullable,
                                                                                                       MERCH_TYPE: aMerchType,
                                                                                                       HN_RID: aHnRID_Nullable,
                                                                                                       PH_RID: aMerchPhRID_Nullable,
                                                                                                       PHL_SEQUENCE: aMerchPhlSeq_Nullable,
                                                                                                       SIZE_CONSTRAINT_RID: aSizeConstraintRID_Nullable,
                                                                                                       SIZE_ALTERNATE_RID: aSizeAlternateRID_Nullable,
                                                                                                       NORMALIZE_SIZE_CURVES_IND: Include.ConvertBoolToChar(aNormalizeSizeCurve),
                                                                                                       IB_MERCH_TYPE: aIB_MerchType,
                                                                                                       IB_MERCH_HN_RID: aIB_MerchHnRID_Nullable,
                                                                                                       IB_MERCH_PH_RID: aIB_MerchPhRID_Nullable,
                                                                                                       IB_MERCH_PHL_SEQUENCE: aIB_MerchPhlSeq_Nullable,
                                                                                                       VSW_SIZE_CONSTRAINTS: aVswSizeConstraints_Nullable,
                                                                                                       FILL_SIZES_TO_TYPE: (int)aFillSizesToType
                                                                                                       );

                    if (!(rowsInserted > 0)) 
					{
						return false;
					}
				}
				catch (DatabaseForeignKeyViolation ex)
				{
					if (ex.Message.Contains("SZ_CV_GR_HDR_BC_SZ_ND_FK1"))
					{
						return true;
					}
					else
					{
						throw;
					}
				}
				// END TT#3199-MD - STodd - Relieve Intransit of 100% VSW headers
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes Header Color Curves for a give header RID
        /// </summary>
        /// <param name="headerRID">RID of the header</param>
        /// <returns>True if delete successful</returns>
        public bool DeleteHeaderColorCurves(int headerRID)
        {
            return (StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE.Delete(_dba, HDR_RID: headerRID) >= 0);
        }
        /// <summary>
        /// Deletes Header Color Curves for a give header RID
        /// </summary>
        /// <param name="headerRID">RID of the header</param>
        /// <returns>True if delete successful</returns>
        public bool DeleteHeaderColorCurves(int headerRID, int aHdrBCRID)
        {
            return (StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR.Delete(_dba, HDR_BC_RID: aHdrBCRID) >= 0);
        }
        // END MID Track #2937 Size OnHand Incorrect
        public DataTable GetPack(int packRID)
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues        
            return StoredProcedures.MID_HEADER_PACK_READ_FROM_PACK.Read(_dba, HDR_PACK_RID: packRID);
        }

        

        public int GetPackRID(int headerRID, string packName)
        {
            int returnHeaderRID = Convert.ToInt32(StoredProcedures.MID_HEADER_PACK_READ_RID_FROM_NAME.ReadValue(_dba, 
                                                                                                             HDR_RID: headerRID,
                                                                                                             HDR_PACK_NAME: packName
                                                                                                             ), CultureInfo.CurrentUICulture);

            return returnHeaderRID;
        }
        //End TT#1663 - DOConnell - Change Pack Name with quote issue

        public DataTable GetPacks()
        {
            //MID Track # 2354 - removed nolock because it causes concurrency issues
 
            return StoredProcedures.MID_HEADER_READ_ALL.Read(_dba);
        }

        public bool DeletePack(int packRID)   // set database for cascade delete
        {
            try
            {
                StoredProcedures.SP_MID_HDRPACK_DELETE.Delete(_dba, PACK_RID: packRID);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public bool DeletePack(int headerRID, string packName)   // set database for cascade delete
        {
            try
            {

                return DeletePack(GetPackRID(headerRID, packName));
            }
            catch
            {
                throw;
            }
        }
        public bool DeletePackColor(int packRID, int colorCodeRID)
        {
            try
            {
                StoredProcedures.SP_MID_HDRPACKCOLOR_DELETE.Delete(_dba, 
                                                                   PACK_RID: packRID,
                                                                   COLOR_RID: colorCodeRID
                                                                   );
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool DeletePackColorSize(int hdrPCRID, int sizeCodeRID)
        {
            try
            {
                StoredProcedures.SP_MID_HDRPACKCOLORSIZE_DELETE.Delete(_dba, 
                                                                       HDR_PC_RID: hdrPCRID,
                                                                       SIZE_RID: sizeCodeRID
                                                                       );
                return true;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#3462 - JSmith - Cancel Allocation Fails on Multi header- DB foreign key error
        public bool DeletePackAllocation(int packRID)   // set database for cascade delete
        {
            try
            {
                StoredProcedures.SP_MID_HDRPACK_ALLOCATION_DELETE.Delete(_dba, PACK_RID: packRID);
                return true;
            }
            catch
            {
                throw;
            }
        }
        // End TT#3462 - JSmith - Cancel Allocation Fails on Multi header- DB foreign key error

        public bool DeleteBulkColor(int hdrBCRID)
        {
            try
            {
                StoredProcedures.SP_MID_HEADER_BULK_DELETE.Delete(_dba, HDR_BC_RID: hdrBCRID);
                return true;
            }
            catch
            {
                throw;
            }
        }
        // Assortment END
        public bool DeleteBulkColorSize(int hdrBCRID, int sizeCodeRID)
        {
            // begin MID Track 4033 allow children of multi to change
            try
            {
                StoredProcedures.SP_MID_HDRBULKCLRSZ_DELETE.Delete(_dba, 
                                                                   HDR_BC_RID: hdrBCRID,
                                                                   SIZE_CODE_RID: sizeCodeRID
                                                                   );
                return true;
                // end MID Track 4033 allow children of multi to change
            }
            catch
            {
                throw;
            }
        }

        public int CreatePack(int headerRID, string packName, int packCount, int packMultiple, int reservePacks, bool packIsGeneric, int assocPackRID, int seq)
        {
            object aPackRID;

            if (assocPackRID < 1)
                aPackRID = null;
            else
                aPackRID = assocPackRID;


            return StoredProcedures.SP_MID_HEADER_PACK_INSERT.InsertAndReturnRID(_dba, 
                                                                                 HDR_RID: headerRID,
                                                                                 HDR_PACK_NAME: packName,
                                                                                 PACKS: packCount,
                                                                                 MULTIPLE: packMultiple,
                                                                                 RESERVE_PACKS: reservePacks,
                                                                                 GENERIC_IND: (packIsGeneric) ? '1' : '0',
                                                                                 ASSOCIATED_PACK_RID: Include.ConvertObjectToNullableInt(aPackRID),
                                                                                 SEQ: seq
                                                                                 );
        }

        public bool UpdatePack(int packRID, int headerRID, string packName, string packDesc, int packCount, int packMultiple, int reservePacks, bool packIsGeneric, int assocPackRID, int seq)
        {
            object aPackRID;

            if (assocPackRID < 1)
                aPackRID = null;
            else
                aPackRID = assocPackRID;

            
            int rowsUpdated = StoredProcedures.MID_HEADER_PACK_UPDATE.Update(_dba, 
                                                                             HDR_PACK_RID: packRID,
                                                                             HDR_RID: headerRID,
                                                                             HDR_PACK_NAME: packName,
                                                                             PACKS: packCount,
                                                                             MULTIPLE: packMultiple,
                                                                             RESERVE_PACKS: reservePacks,
                                                                             GENERIC_IND: (packIsGeneric) ? '1' : '0',
                                                                             ASSOCIATED_PACK_RID: Include.ConvertObjectToNullableInt(aPackRID),
                                                                             SEQ: seq
                                                                             );
            return (rowsUpdated > 0);
        }

        public int CreatePackColor(int packRID, int colorCodeRID, int units, int seq, string name, string description, int last_PCSZ_Key_Used) // Assortment: Color/size changes
        {
            // Assortment BEGIN - change Insert to Stored Procedure, added new columns 
            CheckToSetNull(ref name);
            CheckToSetNull(ref description);
           
            return StoredProcedures.SP_MID_HEADER_PACKCOLOR_INSERT.InsertAndReturnRID(_dba, 
                                                                                       HDR_PACK_RID: packRID,
                                                                                       COLOR_CODE_RID: colorCodeRID,
                                                                                       UNITS: units,
                                                                                       SEQ: seq,
                                                                                       NAME: name,
                                                                                       DESCRIPTION: description,
                                                                                       LAST_PCSZ_KEY_USED: last_PCSZ_Key_Used
                                                                                       );
        }

        public bool UpdatePackColor(int packRID, int hdrPCRID, int colorCodeRID, int units, int seq, string name, string description, int last_PCSZ_Key_Used) // Assortment: Color/size changes
        {
            CheckToSetNull(ref name);
            CheckToSetNull(ref description);
        
            int rowsUpdated = StoredProcedures.MID_HEADER_PACK_COLOR_UPDATE.Update(_dba, 
                                                                                     HDR_PC_RID: hdrPCRID,
                                                                                     UNITS: units,
                                                                                     SEQ: seq,
                                                                                     NAME: name,
                                                                                     DESCRIPTION: description,
                                                                                     LAST_PCSZ_KEY_USED: last_PCSZ_Key_Used
                                                                                     );
            return (rowsUpdated > 0);
        }

        public bool UpdatePackColor(int packRID, int hdrPCRID, int colorCodeRIDOriginal, int colorCodeRIDNew, int units, int seq, string name, string description, int last_PCSZ_Key_Used) // Assortment: Color/size changes
        {
            CheckToSetNull(ref name);
            CheckToSetNull(ref description);

            // assume CASCADE UPDATE constraint on database
          
            int rowsUpdated = StoredProcedures.MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR.Update(_dba, 
                                                                                                HDR_PC_RID: hdrPCRID,
                                                                                                COLOR_CODE_RID_NEW: colorCodeRIDNew,
                                                                                                UNITS: units,
                                                                                                SEQ: seq,
                                                                                                NAME: name,
                                                                                                DESCRIPTION: description,
                                                                                                LAST_PCSZ_KEY_USED: last_PCSZ_Key_Used
                                                                                                );
            return (rowsUpdated > 0);
        }

       

		public bool DeletePackRounding(int headerRID)
		{
            int rowsDeleted = StoredProcedures.MID_HEADER_PACK_ROUNDING_DELETE.Delete(_dba, HDR_RID: headerRID);
            return (rowsDeleted >= 0);
		}

		public bool CreatePackRounding(int headerRID, int packMultipleRid, double PackRounding1stPack, double PackRoundingNthPack)
		{               
            int rowsInserted = StoredProcedures.MID_HEADER_PACK_ROUNDING_INSERT.Insert(_dba, 
                                                                                       HDR_RID: headerRID,
                                                                                       PACK_MULTIPLE_RID: packMultipleRid,
                                                                                       PACK_ROUNDING_1ST_PACK_PCT: PackRounding1stPack,
                                                                                       PACK_ROUNDING_NTH_PACK_PCT: PackRoundingNthPack
                                                                                       );
            return (rowsInserted > 0);
		}
		// END TT#616 - stodd - pack rounding


        public int CreateBulkColor(int headerRID, int colorCodeRID, int units, int multiple, int minimum,
                   int maximum, int reserveUnits, int seq, string name, string description, int aAsrtBCRID, int aLAST_BCSZ_KEY_USED, int aColorStatusFlags) // Assortment: Color/size change // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        {
            object asrtRID;

            if (aAsrtBCRID < 1)
            {
                asrtRID = null;
            }
            else
            {
                asrtRID = aAsrtBCRID;
            }

            CheckToSetNull(ref name);
            CheckToSetNull(ref description);
       
            return StoredProcedures.SP_MID_HEADER_BULK_INSERT.InsertAndReturnRID(_dba, 
                                                                                 HDR_RID: headerRID,
                                                                                 COLOR_CODE_RID: colorCodeRID,
                                                                                 UNITS: units,
                                                                                 MULTIPLE: multiple,
                                                                                 MINIMUM: minimum,
                                                                                 MAXIMUM: maximum,
                                                                                 RESERVE_UNITS: reserveUnits,
                                                                                 SEQ: seq,
                                                                                 NAME: name,
                                                                                 DESCRIPTION: description,
                                                                                 ASRT_BC_RID: Include.ConvertObjectToNullableInt(asrtRID),
                                                                                 LAST_BCSZ_KEY_USED: aLAST_BCSZ_KEY_USED,
                                                                                 COLOR_STATUS_FLAGS: aColorStatusFlags
                                                                                 );
        }


        // Assortment BEGIN - added new columns 
        public bool UpdateBulkColor(int headerRID, int aHDR_BC_RID, int colorCodeRID, int units, int multiple, int minimum, int maximum,
            int reserveUnits, int seq, string name, string description, int aAsrtBCRID, int aLAST_BCSZ_KEY_USED, int aColorStatusFlags) // Assortment: Color/size change // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        {
            object asrtRID;

            if (aAsrtBCRID < 1)
            {
                asrtRID = null;
            }
            else
            {
                asrtRID = aAsrtBCRID;
            }

            CheckToSetNull(ref name);
            CheckToSetNull(ref description);
            
            int rowsUpdated = StoredProcedures.MID_HEADER_BULK_COLOR_UPDATE.Update(_dba, 
                                                                                   HDR_RID: headerRID,
                                                                                   HDR_BC_RID: aHDR_BC_RID,
                                                                                   COLOR_CODE_RID: colorCodeRID,
                                                                                   UNITS: units,
                                                                                   MULTIPLE: multiple,
                                                                                   MINIMUM: minimum,
                                                                                   MAXIMUM: maximum,
                                                                                   RESERVE_UNITS: reserveUnits,
                                                                                   SEQ: seq,
                                                                                   NAME: name,
                                                                                   DESCRIPTION: description,
                                                                                   ASRT_BC_RID: Include.ConvertObjectToNullableInt(asrtRID),
                                                                                   COLOR_STATUS_FLAGS: aColorStatusFlags,
                                                                                   LAST_BCSZ_KEY_USED: aLAST_BCSZ_KEY_USED
                                                                                   );
            return (rowsUpdated > 0);
        }

       
        private void CheckToSetNull(ref string aString)
        {
            if (aString != null && aString.Trim() == string.Empty)
            {
                aString = null;
            }
        }

        
        public bool CreateBulkColorSize(int headerRID, int hdrBCRID, int hdrBCSZ_KEY, int sizeCodeRID, int units, int multiple, int minimum, int maximum, int reserveUnits, int seq) // Assortment: Color/Size change
        {    
            int rowsInserted = StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_INSERT.Insert(_dba, 
                                                                                         HDR_RID: headerRID,
                                                                                         HDR_BC_RID: hdrBCRID,
                                                                                         HDR_BCSZ_KEY: hdrBCSZ_KEY,
                                                                                         SIZE_CODE_RID: sizeCodeRID,
                                                                                         UNITS: units,
                                                                                         MULTIPLE: multiple,
                                                                                         MINIMUM: minimum,
                                                                                         MAXIMUM: maximum,
                                                                                         RESERVE_UNITS: reserveUnits,
                                                                                         SEQ: seq
                                                                                         );
            return (rowsInserted > 0);
        }

        public bool UpdateBulkColorSize(int headerRID, int hdrBCRID, int hdrBCSZKey, int sizeCodeRID, int units, int multiple, int minimum, int maximum, int reserveUnits, int seq)    // Assortment: Color/Size change
        {  
            int rowsUpdated = StoredProcedures.MID_HEADER_BULK_COLOR_SIZE_UPDATE.Update(_dba, 
                                                                                        HDR_BC_RID: hdrBCRID,
                                                                                        HDR_BCSZ_KEY: hdrBCSZKey,
                                                                                        SIZE_CODE_RID: sizeCodeRID,
                                                                                        UNITS: units,
                                                                                        MULTIPLE: multiple,
                                                                                        MINIMUM: minimum,
                                                                                        MAXIMUM: maximum,
                                                                                        RESERVE_UNITS: reserveUnits,
                                                                                        SEQ: seq
                                                                                        );
            return (rowsUpdated > 0);
        }

     
        public bool CreatePackColorSize(int packRID, int hdrPCRID, int aHDR_PCSZ_KEY, int sizeCodeRID, int units, int seq)   // Assortment: Color/Size change
        {
            int rowsInserted = StoredProcedures.MID_HEADER_PACK_COLOR_SIZE_INSERT.Insert(_dba, 
                                                                                         HDR_PACK_RID: packRID,
                                                                                         HDR_PC_RID: hdrPCRID,
                                                                                         HDR_PCSZ_KEY: aHDR_PCSZ_KEY,
                                                                                         SIZE_CODE_RID: sizeCodeRID,
                                                                                         UNITS: units,
                                                                                         SEQ: seq
                                                                                         );
            return (rowsInserted > 0);
        }
        public bool UpdatePackColorSize(int hdrPCRID, int aHDR_PCSZ_KEY, int sizeCodeRID, int units, int seq)
        {
            int rowsUpdated = StoredProcedures.MID_HEADER_PACK_COLOR_SIZE_UPDATE.Update(_dba, 
                                                                                          HDR_PC_RID: hdrPCRID,
                                                                                          HDR_PCSZ_KEY: aHDR_PCSZ_KEY,
                                                                                          SIZE_CODE_RID: sizeCodeRID,
                                                                                          UNITS: units,
                                                                                          SEQ: seq
                                                                                          );
            return (rowsUpdated > 0);
        }

        public DataTable GetMasterSubordinates()
        {
            return StoredProcedures.MID_MASTER_HEADER_READ_ALL_SUBORD.Read(_dba);
        }

        // End Track #5841

        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
        /// <summary>
        /// Creates a Subordinate / Master entry
        /// </summary>
        /// <param name="subordRID">RID of the Subordinate Header</param>
        /// <param name="masterRID">RID of the Master Header</param>
        /// <param name="masterComponent">Key of the Master Component</param>
        /// <param name="masterPackRID">RID of the Master Pack Component</param>
        /// <param name="masterColorRID">RID of the Master Color Component</param>
        /// <param name="subordComponent">Key of the Subordinate Component</param>
        /// <param name="subordPackRID">RID of the Subordinate Pack Component</param>
        /// <param name="subordColorRID">RID of the Subordinate Color Component</param>
        /// <returns>true if successful, false if unsuccessful</returns>
        public bool CreateSubordMaster(int subordRID, int masterRID,
            int aMasterComponent, int aMasterPackRID, int aMasterColorRID,
            int aSubordComponent, int aSubordPackRID, int aSubordColorRID)
        {
            

            int? aMasterPackRID_Nullable = null;
            if (aMasterPackRID != Include.NoRID) aMasterPackRID_Nullable = aMasterPackRID;

            int? aMasterColorRID_Nullable = null;
            if (aMasterColorRID != Include.NoRID) aMasterColorRID_Nullable = aMasterColorRID;

            int? aSubordPackRID_Nullable = null;
            if (aSubordPackRID != Include.NoRID) aSubordPackRID_Nullable = aSubordPackRID;

            int? aSubordColorRID_Nullable = null;
            if (aSubordColorRID != Include.NoRID) aSubordColorRID_Nullable = aSubordColorRID;

            int rowsInserted = StoredProcedures.MID_MASTER_HEADER_INSERT.Insert(_dba, 
                                                                                SUBORD_HDR_RID: subordRID,
                                                                                MASTER_HDR_RID: masterRID,
                                                                                MASTER_COMPONENT: aMasterComponent,
                                                                                MASTER_PACK_RID: aMasterPackRID_Nullable,
                                                                                MASTER_BC_RID: aMasterColorRID_Nullable,
                                                                                SUBORD_COMPONENT: aSubordComponent,
                                                                                SUBORD_PACK_RID: aSubordPackRID_Nullable,
                                                                                SUBORD_BC_RID: aSubordColorRID_Nullable
                                                                                );
            return (rowsInserted > 0);
        }

        /// <summary>
        /// Updates a Subordinate / Master entry
        /// </summary>
        /// <param name="subordRID">RID of the Subordinate Header</param>
        /// <param name="masterRID">RID of the Master Header</param>
        /// <param name="masterComponent">Key of the Master Component</param>
        /// <param name="masterPackRID">RID of the Master Pack Component</param>
        /// <param name="masterColorRID">RID of the Master Color Component</param>
        /// <param name="subordComponent">Key of the Subordinate Component</param>
        /// <param name="subordPackRID">RID of the Subordinate Pack Component</param>
        /// <param name="subordColorRID">RID of the Subordinate Color Component</param>
        /// <returns>true if successful, false if unsuccessful</returns>
        public bool UpdateSubordMaster(int subordRID, int masterRID,
            int aMasterComponent, int aMasterPackRID, int aMasterColorRID,
            int aSubordComponent, int aSubordPackRID, int aSubordColorRID)
        {
            int? aMasterPackRID_Nullable = null;
            if (aMasterPackRID != Include.NoRID) aMasterPackRID_Nullable = aMasterPackRID;

            int? aMasterColorRID_Nullable = null;
            if (aMasterColorRID != Include.NoRID) aMasterColorRID_Nullable = aMasterColorRID;

            int? aSubordPackRID_Nullable = null;
            if (aSubordPackRID != Include.NoRID) aSubordPackRID_Nullable = aSubordPackRID;

            int? aSubordColorRID_Nullable = null;
            if (aSubordColorRID != Include.NoRID) aSubordColorRID_Nullable = aSubordColorRID;

            int rowsUpdated = StoredProcedures.MID_MASTER_HEADER_UPDATE.Update(_dba, 
                                                                               SUBORD_HDR_RID: subordRID,
                                                                               MASTER_HDR_RID: masterRID,
                                                                               MASTER_COMPONENT: aMasterComponent,
                                                                               MASTER_PACK_RID: aMasterPackRID_Nullable,
                                                                               MASTER_BC_RID: aMasterColorRID_Nullable,
                                                                               SUBORD_COMPONENT: aSubordComponent,
                                                                               SUBORD_PACK_RID: aSubordPackRID_Nullable,
                                                                               SUBORD_BC_RID: aSubordColorRID_Nullable
                                                                               );
            return (rowsUpdated > 0);
        }

        /// <summary>
        /// Deletes a Subordinate / Master entry
        /// </summary>
        /// <param name="subordRID">RID of the Subordinate Header</param>
        /// <returns>true if successful, false if unsuccessful</returns>
        public bool DeleteSubordMaster(int subordRID)
        {
            int rowsDeleted = StoredProcedures.MID_MASTER_HEADER_DELETE.Delete(_dba, SUBORD_HDR_RID: subordRID);
            return (rowsDeleted > 0);
        }

        /// <summary>
        /// Gets the Subordinate Header RID for a given Master Header RID
        /// </summary>
        /// <param name="subordRID">RID of the Master Header</param>
        /// <returns>RID of Master Header OR No RID value</returns>
        public int GetSubordForMaster(int masterRID) 
        {
            int subordRID = Include.NoRID;

            try
            {
                subordRID = Convert.ToInt32(StoredProcedures.MID_MASTER_HEADER_READ_SUBORD.ReadValue(_dba, MASTER_HDR_RID: masterRID), CultureInfo.CurrentUICulture); 

                if (subordRID == 0)
                {
                    subordRID = Include.NoRID;
                }
            }
            catch
            {
                subordRID = Include.NoRID;
            }

            return subordRID;
        }

        /// <summary>
        /// Gets the Master Header RID for a given Subordinate Header RID
        /// </summary>
        /// <param name="subordRID">RID of the Subordinate Header</param>
        /// <returns>RID of Master Header OR No RID value</returns>
        public int GetMasterForSubord(int subordRID)
        {
            int masterRID = Include.NoRID;

            try
            {
                masterRID = Convert.ToInt32(StoredProcedures.MID_MASTER_HEADER_READ.ReadValue(_dba, SUBORD_HDR_RID: subordRID), CultureInfo.CurrentUICulture);

                if (masterRID == 0)
                {
                    masterRID = Include.NoRID;
                }
            }
            catch
            {
                masterRID = Include.NoRID;
            }

            return masterRID;
        }

        /// <summary>
        /// Gets the Master Header RID for a given Subordinate Header RID
        /// </summary>
        /// <param name="subordRID">RID of the Subordinate Header</param>
        /// <returns>DataTable containing all component rows for subordinate</returns>
        public DataTable GetComponentsForSubord(int subordRID)
        {
            return StoredProcedures.MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD.Read(_dba, SUBORD_HDR_RID: subordRID);
        }

        // begin MID track 4029 ReWork MasterPO Logic
        /// <summary>
        /// Gets the Subordinate Header RID for a given Master Header RID
        /// </summary>
        /// <param name="aMasterRID">RID of the Master Header</param>
        /// <returns>DataTable containing all component rows for subordinate</returns>
        public DataTable GetComponentsForMaster(int aMasterRID)
        {
            return StoredProcedures.MID_MASTER_HEADER_READ_COMPONENTS.Read(_dba, MASTER_HDR_RID: aMasterRID);
        }
        // end MID Track 4029 ReWork MasterPO Logic

       

        /// <summary>
        /// Gets the Master Header ID for a given Master Header RID
        /// </summary>
        /// <param name="masterRID">RID of the Master Header</param>
        /// <returns>ID of Master Header OR null</returns>
        public string GetMasterID(int masterRID)
        {
            string masterID = null;
            try
            {
                masterID = Convert.ToString(StoredProcedures.MID_HEADER_READ_ID.ReadValue(_dba, HDR_RID: masterRID), CultureInfo.CurrentUICulture);
            }
            catch
            {
                masterID = null;
            }

            return masterID;
        }

        /// <summary>
        /// Gets the Subordinate Header ID for a given Subordinate Header RID
        /// </summary>
        /// <param name="masterRID">RID of the Subordinate Header</param>
        /// <returns>ID of Subordinate Header OR null</returns>
        public string GetSubordinateID(int subordRID)
        {
            string subordID = null;

            try
            {
                subordID = Convert.ToString(StoredProcedures.MID_HEADER_READ_ID.ReadValue(_dba, HDR_RID: subordRID), CultureInfo.CurrentUICulture);
            }
            catch
            {
                subordID = null;
            }

            return subordID;
        }

		// Begin TT#1581-MD - stodd - Header Reconcile API
        public int GetNextHeaderSequenceNumber(int seqLength)
        {
            try
            {
                return StoredProcedures.MID_HEADER_SEQUENCE_GET_NEXT.UpdateAndReturnNextSequence(_dba, seqLength);
            }
            catch
            {
                throw;
            }
        }
    	// End TT#1581-MD - stodd - Header Reconcile API

        /// <summary>
        /// Write total allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteTotalAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqr)
        {
            DateTime dateTimeValue;
            int intValue, qtyAllocated;
            double doubleValue;
            eRuleType eRuleTypeValue;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBTotalAllocation;
                if (_dtTotalAllocationSchema == null)
                {
                    _dtTotalAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtTotalAllocation = _dtTotalAllocationSchema.Copy();
                DataRow drTotalAllocation;
                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqr.IsTotalAllocationAllDefaultValues(storeRID))
                    {
                        drTotalAllocation = dtTotalAllocation.NewRow();
                        dtTotalAllocation.Rows.Add(drTotalAllocation);

                        drTotalAllocation["STORE_GRADE_INDEX"] = saqr.GetStoreGradeIndex(storeRID);
                        qtyAllocated = saqr.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drTotalAllocation["UNITS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqr.GetStoreQtyShipped(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["UNITS_SHIPPED"] = intValue;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["UNITS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["UNITS_ALLOCATED_BY_RULE"] = intValue;
                        }
                        eRuleTypeValue = saqr.GetStoreChosenRule(storeRID);
                        if (eRuleTypeValue != eRuleType.None)
                        {
                            drTotalAllocation["CHOSEN_RULE_TYPE_ID"] = eRuleTypeValue;
                        }
                        intValue = saqr.GetStoreChosenRuleLayerID(storeRID);
                        if (intValue > -1)
                        {
                            drTotalAllocation["CHOSEN_RULE_LAYER_ID"] = saqr.GetStoreChosenRuleLayerID(storeRID);
                        }
                        dateTimeValue = saqr.GetStoreShipToDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drTotalAllocation["SHIP_DAY"] = dateTimeValue;
                        }
                        dateTimeValue = saqr.GetStoreNeedDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drTotalAllocation["NEED_DAY"] = dateTimeValue;
                        }
                        intValue = saqr.GetStoreNeed(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drTotalAllocation["UNIT_NEED_BEFORE"] = intValue;
                        }
                        doubleValue = saqr.GetStorePercentNeed(storeRID);
                        if (doubleValue < double.MaxValue)
                        {
                            drTotalAllocation["PERCENT_NEED_BEFORE"] = doubleValue;
                        }
                        intValue = saqr.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqr.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drTotalAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqr.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drTotalAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqr.GetStoreCapacity(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drTotalAllocation["STORE_CAPACITY"] = intValue;
                        }
                        intValue = saqr.GetStoreDetailAudit(storeRID);
                        if (intValue != 0)
                        {
                            drTotalAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue;            // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drTotalAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue;        // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqr.GetStoreGeneralAudit(storeRID);
                        if (intValue != 0)
                        {
                            drTotalAllocation["ALLOC_STORE_GEN_AUDIT_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqr.GetStoreShippingStatus(storeRID);
                        if (intValue != 0)
                        {
                            drTotalAllocation["SHIPPING_STATUS_FLAGS"] = (byte)intValue;
                        }
                        //intValue = saqr.GetStoreChosenRuleQty(storeRID);
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["CHOSEN_RULE_UNITS"] = intValue;
                        }
                        doubleValue = saqr.GetStoreExceedCapacityByPercent(storeRID);
                        if (doubleValue > 0.0d)
                        {
                            if (doubleValue == double.MaxValue)
                            {
                                drTotalAllocation["EXCEED_CAPACITY_PERCENT"] = -1.0d;
                            }
                            else
                            {
                                drTotalAllocation["EXCEED_CAPACITY_PERCENT"] = doubleValue;
                            }
                        }
                        intValue = saqr.GetStoreImoMaxValue(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drTotalAllocation["IMO_MAX_VALUE"] = intValue;
                        }
                        intValue = saqr.GetStoreImoMinShipQty(storeRID);
                        if (intValue > 0)
                        {
                            drTotalAllocation["IMO_MIN_SHIP_QTY"] = saqr.GetStoreImoMinShipQty(storeRID);
                        }
                        doubleValue = saqr.GetStoreImoPackThreshold(storeRID);
                        if (doubleValue != .5)
                        {
                            drTotalAllocation["IMO_PCT_PK_THRSHLD"] = doubleValue;
                        }
                        intValue = saqr.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drTotalAllocation["ITEM_UNITS_ALLOCATED"] = intValue;
                        }
                        drTotalAllocation["HDR_RID"] = aHeaderRID;
                        drTotalAllocation["ST_RID"] = storeRID;
                    }
                }
                if (dtTotalAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Total, dtTotalAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCTOTALUDT", dtTotalAllocation, "dbo.TOTAL_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtTotalAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        TotalAllocationWorkRowCount += dtTotalAllocation.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write detail allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteDetailAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqr)
        {
            DateTime dateTimeValue;
            int intValue, qtyAllocated;
            double doubleValue;
            eRuleType eRuleTypeValue;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBDetailAllocation;
                if (_dtDetailAllocationSchema == null)
                {
                    _dtDetailAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtDetailAllocation = _dtDetailAllocationSchema.Copy();
                DataRow drDetailAllocation;
                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqr.IsDetailAllocationAllDefaultValues(storeRID))
                    {
                        drDetailAllocation = dtDetailAllocation.NewRow();
                        dtDetailAllocation.Rows.Add(drDetailAllocation);
                        qtyAllocated = saqr.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drDetailAllocation["UNITS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drDetailAllocation["UNITS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drDetailAllocation["UNITS_ALLOCATED_BY_RULE"] = intValue;
                        }
                        intValue = saqr.GetStoreChosenRuleLayerID(storeRID);
                        if (intValue > -1)
                        {
                            drDetailAllocation["CHOSEN_RULE_LAYER_ID"] = intValue;
                        }
                        eRuleTypeValue = saqr.GetStoreChosenRule(storeRID);
                        if (eRuleTypeValue != eRuleType.None)
                        {
                            drDetailAllocation["CHOSEN_RULE_TYPE_ID"] = eRuleTypeValue;
                        }
                        //intValue = saqr.GetStoreChosenRuleQty(storeRID);
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drDetailAllocation["CHOSEN_RULE_UNITS"] = intValue;
                        }
                        dateTimeValue = saqr.GetStoreNeedDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drDetailAllocation["NEED_DAY"] = dateTimeValue;
                        }
                        intValue = saqr.GetStoreNeed(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drDetailAllocation["UNIT_NEED_BEFORE"] = intValue;
                        }
                        doubleValue = saqr.GetStorePercentNeed(storeRID);
                        if (doubleValue < double.MaxValue)
                        {
                            drDetailAllocation["PERCENT_NEED_BEFORE"] = doubleValue;
                        }
                        intValue = saqr.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drDetailAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqr.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drDetailAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqr.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drDetailAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqr.GetStoreDetailAudit(storeRID);
                        if (intValue > 0)
                        {
                            drDetailAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue;        // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drDetailAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue;        // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqr.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drDetailAllocation["ITEM_UNITS_ALLOCATED"] = intValue;
                        }
                        drDetailAllocation["HDR_RID"] = aHeaderRID;
                        drDetailAllocation["ST_RID"] = storeRID;
                    }
                }
                if (dtDetailAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Detail, dtDetailAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCDETAILUDT", dtDetailAllocation, "dbo.DETAIL_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtDetailAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        DetailAllocationWorkRowCount += dtDetailAllocation.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write bulk allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteBulkAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqr)
        {
            DateTime dateTimeValue;
            int intValue, qtyAllocated;
            double doubleValue;
            eRuleType eRuleTypeValue;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBBulkAllocation;
                if (_dtBulkAllocationSchema == null)
                {
                    _dtBulkAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtBulkAllocation = _dtBulkAllocationSchema.Copy();
                DataRow drBulkAllocation;
                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqr.IsBulkAllocationAllDefaultValues(storeRID))
                    {
                        drBulkAllocation = dtBulkAllocation.NewRow();
                        dtBulkAllocation.Rows.Add(drBulkAllocation);

                        qtyAllocated = saqr.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drBulkAllocation["UNITS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drBulkAllocation["UNITS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drBulkAllocation["UNITS_ALLOCATED_BY_RULE"] = intValue;
                        }
                        intValue = saqr.GetStoreChosenRuleLayerID(storeRID);
                        if (intValue > -1)
                        {
                            drBulkAllocation["CHOSEN_RULE_LAYER_ID"] = intValue;
                        }
                        eRuleTypeValue = saqr.GetStoreChosenRule(storeRID);
                        if (eRuleTypeValue != eRuleType.None)
                        {
                            drBulkAllocation["CHOSEN_RULE_TYPE_ID"] = eRuleTypeValue;
                        }
                        //intValue = saqr.GetStoreChosenRuleQty(storeRID);
                        intValue = saqr.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drBulkAllocation["CHOSEN_RULE_UNITS"] = intValue;
                        }
                        dateTimeValue = saqr.GetStoreNeedDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drBulkAllocation["NEED_DAY"] = dateTimeValue;
                        }
                        intValue = saqr.GetStoreNeed(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drBulkAllocation["UNIT_NEED_BEFORE"] = intValue;
                        }
                        doubleValue = saqr.GetStorePercentNeed(storeRID);
                        if (doubleValue < double.MaxValue)
                        {
                            drBulkAllocation["PERCENT_NEED_BEFORE"] = doubleValue;
                        }
                        intValue = saqr.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drBulkAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqr.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drBulkAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqr.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drBulkAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqr.GetStoreDetailAudit(storeRID);
                        if (intValue > 0)
                        {
                            drBulkAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drBulkAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqr.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drBulkAllocation["ITEM_UNITS_ALLOCATED"] = intValue;
                        }

                        drBulkAllocation["HDR_RID"] = aHeaderRID;
                        drBulkAllocation["ST_RID"] = storeRID;
                    }
                }
                if (dtBulkAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Bulk, dtBulkAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCBULKUDT", dtBulkAllocation, "dbo.BULK_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtBulkAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        BulkAllocationWorkRowCount += dtBulkAllocation.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write pack allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WritePackAllocation(int[] aStoreRIDList, int aHeaderRID, Dictionary<int, StoreAllocationQuickRequest> saqrDict)
        {
            bool success = true;

            try
            {
                foreach (StoreAllocationQuickRequest saqrPack in saqrDict.Values)
                {
                    success = WritePackAllocation(aStoreRIDList, aHeaderRID, saqrPack);
                    if (!success)
                    {
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write pack allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WritePackAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqrPack)
        {
            DateTime dateTimeValue;
            int intValue, qtyAllocated;
            double doubleValue;
            eRuleType eRuleTypeValue;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBPackAllocation;
                if (_dtPackAllocationSchema == null)
                {
                    _dtPackAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtPackAllocation = _dtPackAllocationSchema.Copy();
                DataRow drPackAllocation;

                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqrPack.IsPackAllocationAllDefaultValues(storeRID))
                    {
                        drPackAllocation = dtPackAllocation.NewRow();
                        dtPackAllocation.Rows.Add(drPackAllocation);

                        qtyAllocated = saqrPack.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drPackAllocation["PACKS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqrPack.GetStoreQtyShipped(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["PACKS_SHIPPED"] = intValue;
                        }
                        intValue = saqrPack.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["PACKS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqrPack.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["PACKS_ALLOCATED_BY_RULE"] = intValue;
                        }
                        intValue = saqrPack.GetStoreChosenRuleLayerID(storeRID);
                        if (intValue > -1)
                        {
                            drPackAllocation["CHOSEN_RULE_LAYER_ID"] = intValue;
                        }
                        eRuleTypeValue = saqrPack.GetStoreChosenRule(storeRID);
                        if (eRuleTypeValue != eRuleType.None)
                        {
                            drPackAllocation["CHOSEN_RULE_TYPE_ID"] = eRuleTypeValue;
                        }
                        //intValue = saqrPack.GetStoreChosenRuleQty(storeRID);
                        intValue = saqrPack.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["CHOSEN_RULE_PACKS"] = intValue;
                        }
                        dateTimeValue = saqrPack.GetStoreNeedDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drPackAllocation["NEED_DAY"] = dateTimeValue;
                        }
                        intValue = saqrPack.GetStoreNeed(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drPackAllocation["UNIT_NEED_BEFORE"] = intValue;
                        }
                        doubleValue = saqrPack.GetStorePercentNeed(storeRID);
                        if (doubleValue < double.MaxValue)
                        {
                            drPackAllocation["PERCENT_NEED_BEFORE"] = doubleValue;
                        }
                        intValue = saqrPack.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqrPack.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drPackAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqrPack.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drPackAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqrPack.GetStoreDetailAudit(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drPackAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqrPack.GetStoreGeneralAudit(storeRID);
                        if (intValue != 0)
                        {
                            drPackAllocation["ALLOC_STORE_GEN_AUDIT_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqrPack.GetStoreShippingStatus(storeRID);
                        if (intValue > 0)
                        {
                            drPackAllocation["SHIPPING_STATUS_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqrPack.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drPackAllocation["ITEM_PACKS_ALLOCATED"] = intValue;
                        }

                        drPackAllocation["HDR_RID"] = aHeaderRID;
                        drPackAllocation["HDR_PACK_RID"] = saqrPack.PackRID;
                        drPackAllocation["ST_RID"] = storeRID;
                    }
                }
                if (dtPackAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Pack, dtPackAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCPACKUDT", dtPackAllocation, "dbo.PACK_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtPackAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        PackAllocationWorkRowCount += dtPackAllocation.Rows.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write bulk color allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteBulkColorAllocation(int[] aStoreRIDList, int aHeaderRID, Dictionary<int, StoreAllocationQuickRequest> saqrDict)
        {
            bool success = true;

            try
            {
                foreach (StoreAllocationQuickRequest saqrColor in saqrDict.Values)
                {
                    success = WriteBulkColorAllocation(aStoreRIDList, aHeaderRID, saqrColor);
                    if (!success)
                    {
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write bulk color allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteBulkColorAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqrColor)
        {
            DateTime dateTimeValue;
            int intValue, qtyAllocated;
            double doubleValue;
            eRuleType eRuleTypeValue;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBBulkColorAllocation;
                if (_dtColorAllocationSchema == null)
                {
                    _dtColorAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtColorAllocation = _dtColorAllocationSchema.Copy();
                DataRow drColorAllocation;
                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqrColor.IsBulkColorAllocationAllDefaultValues(storeRID))
                    {
                        drColorAllocation = dtColorAllocation.NewRow();
                        dtColorAllocation.Rows.Add(drColorAllocation);

                        qtyAllocated = saqrColor.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drColorAllocation["UNITS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqrColor.GetStoreQtyShipped(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["UNITS_SHIPPED"] = intValue;
                        }
                        intValue = saqrColor.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["UNITS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqrColor.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["UNITS_ALLOCATED_BY_RULE"] = intValue;
                        }
                        intValue = saqrColor.GetStoreChosenRuleLayerID(storeRID);
                        if (intValue > -1)
                        {
                            drColorAllocation["CHOSEN_RULE_LAYER_ID"] = intValue;
                        }
                        eRuleTypeValue = saqrColor.GetStoreChosenRule(storeRID);
                        if (eRuleTypeValue != eRuleType.None)
                        {
                            drColorAllocation["CHOSEN_RULE_TYPE_ID"] = eRuleTypeValue;
                        }
                        //intValue = saqrColor.GetStoreChosenRuleQty(storeRID);
                        intValue = saqrColor.GetStoreQtyAllocatedByRule(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["CHOSEN_RULE_UNITS"] = intValue;
                        }
                        dateTimeValue = saqrColor.GetStoreNeedDay(storeRID);
                        if (dateTimeValue != Include.UndefinedDate)
                        {
                            drColorAllocation["NEED_DAY"] = dateTimeValue;
                        }
                        intValue = saqrColor.GetStoreNeed(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drColorAllocation["UNIT_NEED_BEFORE"] = intValue;
                        }
                        doubleValue = saqrColor.GetStorePercentNeed(storeRID);
                        if (doubleValue < double.MaxValue)
                        {
                            drColorAllocation["PERCENT_NEED_BEFORE"] = doubleValue;
                        }
                        intValue = saqrColor.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqrColor.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drColorAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqrColor.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drColorAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqrColor.GetStoreDetailAudit(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue;   // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drColorAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqrColor.GetStoreGeneralAudit(storeRID);
                        if (intValue != 0)
                        {
                            drColorAllocation["ALLOC_STORE_GEN_AUDIT_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqrColor.GetStoreShippingStatus(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["SHIPPING_STATUS_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqrColor.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drColorAllocation["ITEM_UNITS_ALLOCATED"] = intValue;
                        }
                        intValue = saqrColor.GetStoreItemIdealMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drColorAllocation["ITEM_IDEAL_MINIMUM"] = intValue;
                        }

                        drColorAllocation["HDR_RID"] = aHeaderRID;
                        drColorAllocation["HDR_BC_RID"] = saqrColor.HDR_BC_RID;
                        drColorAllocation["ST_RID"] = storeRID;

                    }
                }
                if (dtColorAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Color_Bulk, dtColorAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCBULKCOLORUDT", dtColorAllocation, "dbo.BULK_COLOR_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtColorAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        BulkColorAllocationWorkRowCount += dtColorAllocation.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write bulk color size allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteBulkColorSizeAllocation(int[] aStoreRIDList, int aHeaderRID, Dictionary<long, StoreAllocationQuickRequest> saqrSizeDict)
        {
            bool success = true;

            try
            {
                foreach (StoreAllocationQuickRequest saqrColorSize in saqrSizeDict.Values)
                {
                    success = WriteBulkColorSizeAllocation(aStoreRIDList, aHeaderRID, saqrColorSize);
                    if (!success)
                    {
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Write bulk color size allocation information to database
        /// </summary>
        /// <param name="aStoreRIDList">List of stores to write</param>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <returns>True: Write was successful</returns>
        public bool WriteBulkColorSizeAllocation(int[] aStoreRIDList, int aHeaderRID, StoreAllocationQuickRequest saqrColorSize)
        {
            int intValue, qtyAllocated;
            string strTableName;
            bool success = true;

            try
            {
                strTableName = Include.DBBulkColorSizeAllocation;
                if (_dtColorSizeAllocationSchema == null)
                {
                    _dtColorSizeAllocationSchema = DatabaseSchema.GetTableSchema("VW_" + strTableName);
                }
                DataTable dtColorSizeAllocation = _dtColorSizeAllocationSchema.Copy();
                DataRow drColorSizeAllocation;
                foreach (int storeRID in aStoreRIDList)
                {
                    if (!saqrColorSize.IsBulkColorSizeAllocationAllDefaultValues(storeRID))
                    {
                        drColorSizeAllocation = dtColorSizeAllocation.NewRow();
                        dtColorSizeAllocation.Rows.Add(drColorSizeAllocation);

                        qtyAllocated = saqrColorSize.GetStoreQtyAllocated(storeRID);
                        if (qtyAllocated > 0)
                        {
                            drColorSizeAllocation["UNITS_ALLOCATED"] = qtyAllocated;
                        }
                        intValue = saqrColorSize.GetStoreQtyShipped(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["UNITS_SHIPPED"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreQtyAllocatedByAuto(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["UNITS_ALLOCATED_BY_AUTO"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["MINIMUM"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreMaximum(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drColorSizeAllocation["MAXIMUM"] = intValue;
                        }
                        intValue = saqrColorSize.GetStorePrimaryMax(storeRID);
                        if (intValue < int.MaxValue)
                        {
                            drColorSizeAllocation["PRIMARY_MAX"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreDetailAudit(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (uint)intValue;   // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                            //drColorSizeAllocation["ALLOC_STORE_DET_AUDIT_FLAGS"] = (ushort)intValue; // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
                        }
                        intValue = saqrColorSize.GetStoreShippingStatus(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["SHIPPING_STATUS_FLAGS"] = (ushort)intValue;
                        }
                        intValue = saqrColorSize.GetStoreItemQtyAllocated(storeRID);
                        if (intValue != qtyAllocated)
                        {
                            drColorSizeAllocation["ITEM_UNITS_ALLOCATED"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreItemMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["ITEM_MINIMUM"] = intValue;
                        }
                        intValue = saqrColorSize.GetStoreItemIdealMinimum(storeRID);
                        if (intValue > 0)
                        {
                            drColorSizeAllocation["ITEM_IDEAL_MINIMUM"] = intValue;
                        }

                        drColorSizeAllocation["HDR_RID"] = aHeaderRID;
                        drColorSizeAllocation["HDR_BC_RID"] = saqrColorSize.HDR_BC_RID;
                        drColorSizeAllocation["HDR_BCSZ_KEY"] = saqrColorSize.HDR_BCSZ_KEY;
                        drColorSizeAllocation["ST_RID"] = storeRID;
                    }
                }
                if (dtColorSizeAllocation.Rows.Count > 0)
                {
                    if (!UpdateStoreAllocation(AllocationUpdateType.Size_BulkColor, dtColorSizeAllocation))
                    //if (!UpdateStoreAllocation("SP_MID_UPDATEALLOCBULKCOLORSIZEUDT", dtColorSizeAllocation, "dbo.BULK_COLOR_SIZE_ALLOCATION_TYPE"))
                    //if (!_dba.SQLBulkCopy(BuildTempTableName(strTableName), dtColorSizeAllocation))
                    {
                        success = false;
                    }
                    else
                    {
                        BulkColorSizeAllocationWorkRowCount += dtColorSizeAllocation.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return success;
        }

        /// <summary>
        /// Deletes the allocation from the expand tables
        /// </summary>
        /// <param name="aHeaderRID">HeaderRID</param>
        /// <remarks>Must open connection prior to call and commit after call</remarks>
        /// <returns>True: Delete action was successful</returns>
        public bool DeleteStoreAllocation(int aHeaderRID)
        {
            bool success = true;
            try
            {
                StoredProcedures.SP_MID_DELETE_HEADER_ALLOCATION.Delete(_dba, HDR_RID: aHeaderRID);
            }
            catch
            {
                throw;
            }
            return success;
        }



        public enum AllocationUpdateType
        {
            Total = 0,
            Detail = 1,
            Bulk = 2,
            Pack = 3,
            Color_Bulk = 4,
            Size_BulkColor = 5
        }
      
        /// <summary>
        /// Update the allocation in the expand tables
        /// </summary>
        /// <param name="aStoredProcedure">The Stored Procedure to execute</param>
        /// <param name="aDataTable">The dataTable containing the values</param>
        /// <remarks>Must open connection prior to call and commit after call</remarks>
        /// <returns>True: Delete action was successful</returns>
        public bool UpdateStoreAllocation(AllocationUpdateType allocationUpdateType, DataTable aDataTable)
        {
            bool success = true;
            try
            {
                if (allocationUpdateType == AllocationUpdateType.Total)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCTOTALUDT.Insert(_dba, Updt: aDataTable);
                }
                else if (allocationUpdateType == AllocationUpdateType.Detail)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCDETAILUDT.Insert(_dba, Updt: aDataTable);
                }
                else if (allocationUpdateType == AllocationUpdateType.Bulk)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCBULKUDT.Insert(_dba, Updt: aDataTable);
                }
                else if (allocationUpdateType == AllocationUpdateType.Pack)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCPACKUDT.Insert(_dba, Updt: aDataTable);
                }
                else if (allocationUpdateType == AllocationUpdateType.Color_Bulk)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCBULKCOLORUDT.Insert(_dba, Updt: aDataTable);
                }
                else if (allocationUpdateType == AllocationUpdateType.Size_BulkColor)
                {
                    StoredProcedures.SP_MID_UPDATEALLOCBULKCOLORSIZEUDT.Insert(_dba, Updt: aDataTable);
                }
            }
            catch
            {
                throw;
            }
            return success;
        }
        // End TT#739-MD -JSmith - Delete Stores

        /// <summary>
        /// Write a header rejected by the reclass process to the database
        /// </summary>
        /// <param name="aProcessRID">The record ID of the reclass process</param>
        /// <param name="aHeaderRID">The record ID of the rejected header</param>
        /// <param name="aHeaderID">The ID of the rejected header</param>
        /// <param name="aHeaderStatus">The status of the rejected header</param>
        /// <returns>True: actions were successful</returns>
        public bool WriteReclassRejectedHeader(int aProcessRID, int aHeaderRID, string aHeaderID, string aHeaderStatus)
        {
            try
            {
                int rowsInserted = StoredProcedures.MID_RECLASS_REJECTED_HEADER_INSERT.Insert(_dba, 
                                                                                              PROCESS_RID: aProcessRID,
                                                                                              HDR_RID: aHeaderRID,
                                                                                              HDR_ID: aHeaderID,
                                                                                              HDR_STATUS: aHeaderStatus
                                                                                              );
                return (rowsInserted > 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all linked headers that are not in use by multi-headers
        /// </summary>
        /// <param name="aHeaderRID">The RID of the header to match the link ID</param>
        /// <returns></returns>
        public DataTable GetLinkedHeaders(int aHeaderRID)
        {
            try
            {
                //TT#1244 -  RMatelic - Getting timeouts on Releases - remove extraneous 'HEADER_CHAR hc,' in 1st select                 
                return StoredProcedures.MID_HEADER_READ_LINKED.Read(_dba, HDR_RID: aHeaderRID);
            }
            catch
            {
                throw;
            }
        }

        public int GetNumberLinkedHeadersNotReleaseApproved(int aHeaderRID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED.Read(_dba, HDR_RID: aHeaderRID);
                return dt.Rows.Count;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all multi-headers
        /// </summary>
        /// <returns>
        /// a DataTable with a headers that participate in a multi-header along with
        /// the link characteristic value.
        /// </returns>
        public DataTable GetMultiHeadersWithLinkCharacteristic(int aLinkCharacteristicValue)
        {
            try
            {
                return StoredProcedures.MID_HEADER_READ_MULTI_WITH_LINK_CHAR.Read(_dba, HEADER_LINK_CHARACTERISTIC: aLinkCharacteristicValue);
            }
            catch
            {
                throw;
            }
        }



        // Begin TT#634 - JSmith - Color rename
        /// <summary>
        /// Update the color code RID on bulk header
        /// </summary>
        public void UpdateBulkColorOnHeader(int aHeaderRID, int aColorCodeRID, int aNewColorCodeRID)
        {
            try
            {
                StoredProcedures.MID_HEADER_BULK_COLOR_UPDATE_COLOR.Update(_dba, 
                                                                           HDR_RID: aHeaderRID,
                                                                           OLD_COLOR_CODE_RID: aColorCodeRID,
                                                                           NEW_COLOR_CODE_RID: aNewColorCodeRID
                                                                           );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Update the color code RID on a header pack
        /// </summary>
        public void UpdatePackColorOnHeader(int aHdrRID, int aColorCodeRID, int aNewColorCodeRID)    // TT#1185 - Verify ENQ after Update
        {
            try
            {
                StoredProcedures.MID_HEADER_PACK_COLOR_UPDATE_COLOR.Update(_dba, 
                                                                           HDR_RID: aHdrRID,
                                                                           OLD_COLOR_CODE_RID: aColorCodeRID,
                                                                           NEW_COLOR_CODE_RID: aNewColorCodeRID
                                                                           );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#634

        // Begin TT#457-MD - RMatelic - Add additional Header information to the Allocation Workspace
        public string GetNodeDisplay(int nodeRID)
        {
            try
            {
                return Convert.ToString(StoredProcedures.MID_HIERARCHY_READ_NODE_DISPLAY.ReadValue(_dba, NODE_RID: nodeRID), CultureInfo.CurrentUICulture);
            }
            catch
            {
                throw;
            }
        }
        // End TT#457-MD

        public DataTable GetVSWStorage(int headerRID)
        {
            return StoredProcedures.MID_VSW_REVERSE_ON_HAND_READ.Read(_dba, HDR_RID: headerRID);
        }
        public void VSW_Upsert(DatabaseAccess dba, int headerRID, int hnRID, int stRID, int reverseOnHandUnits)
        {
            StoredProcedures.MID_VSW_REVERSE_ON_HAND_UPSERT.Insert(dba,
                                                                       HDR_RID: headerRID,
                                                                       HN_RID: hnRID,
                                                                       ST_RID: stRID,
                                                                       VSW_REVERSE_ON_HAND_UNITS: reverseOnHandUnits
                                                                       );
        }

        public DataTable ExecuteCommand(string sqlCommand)
        {
            try
            {
                DataTable dt = _dba.ExecuteSQLQuery(sqlCommand, "ExecuteCommand");
                return dt;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public DataTable GetDistCenters()
        {
            try
            {

                return StoredProcedures.MID_HEADER_DIST_CENTER_READ_ALL.Read(_dba);
            }
            catch
            {
                throw;
            }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment

	}

	/// <summary> 
    /// Identifies the type of store allocation sql access
    /// </summary>
    //public enum eAllocationUpdateRequest
    //{
    //	TotalAllocationUpdate  = 0,
    //	DetailAllocationUpdate = 1,
    //	BulkAllocationUpdate   = 2,
    //	PackAllocationUpdate   = 3, 
    //	ColorAllocationUpdate  = 4,
    //	SizeAllocationUpdate   = 5
    //}
    public enum eAllocationSqlAccessRequest
    {
        TotalAllocation = 0,
        DetailAllocation = 1,
        BulkAllocation = 2,
        PackAllocation = 3,
        ColorAllocation = 4,
        SizeAllocation = 5
    }


    public class StoreAllocationQuickRequest
    {
        #region Fields
        private eAllocationSqlAccessRequest _allocationSqlAccessRequest;
        //private int _version;
        private int _headerRID;
        private int _packRID;
        // Begin Assortment:  Color/size keys changed
        //private int _colorRID;
        //private int _sizeRID;
        //private int _sequence;
        private int _hdr_BC_RID;
        private int _hdr_BCSZ_KEY;
        // End Assortment:  Color/size Keys changed
        private StoreAllocationStructureStatus _sass;
        private Hashtable[] _structureHash;
        #endregion Fields

        #region Constructors
        // Begin Assortment: Changed Color/size keys to be different from ColorRID and SizeRID)
        //public StoreAllocationQuickRequest(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, int aHeaderRID, int aPackRID, int aColorRID, int aSizeRID, int aSequence)
        public StoreAllocationQuickRequest(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, int aHeaderRID, int aPackRID, int aHDR_BC_RID, int aHDR_BCSZ_KEY)
        // End Assortment: Changed Color/size keys to be different from ColorRID and SizeRID)
        {
            //_version = 0;
            _allocationSqlAccessRequest = aAllocationSqlAccessRequest;
            _headerRID = aHeaderRID;
            _sass = new StoreAllocationStructureStatus(false);
            _structureHash = new Hashtable[Include.SQL_StructureTypes.Length];  //NOTE:  the null state of a hash table indicates that no changes have occurred in the data
            switch (_allocationSqlAccessRequest)
            {
                case (eAllocationSqlAccessRequest.BulkAllocation):
                case (eAllocationSqlAccessRequest.DetailAllocation):
                case (eAllocationSqlAccessRequest.TotalAllocation):
                    {
                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Change
                        //_colorRID = Include.NoRID;
                        //_sizeRID =  Include.NoRID;
                        //_sequence = Include.NoRID;  // Not Used
                        _hdr_BC_RID = Include.NoRID;
                        _hdr_BCSZ_KEY = Include.NoRID;
                        // End Assortment: Color/Size Change
                        break;
                    }
                case (eAllocationSqlAccessRequest.PackAllocation):
                    {
                        if (aPackRID <= 0)
                        {
                            throw new Exception("Store Pack Allocation SQL Access Request: Pack RID must be greater than 0");
                        }
                        _packRID = aPackRID;
                        // Begin Assortment: Color/Size Change
                        //_colorRID = Include.NoRID;
                        //_sizeRID =  Include.NoRID;
                        //_sequence = Include.NoRID;  // Not Used
                        _hdr_BC_RID = Include.NoRID;
                        _hdr_BCSZ_KEY = Include.NoRID;
                        // End Assortment: Color/Size Change
                        break;
                    }
                case (eAllocationSqlAccessRequest.ColorAllocation):
                    {
                        //if (aColorRID <=0) // Assortment: Color/Size Change
                        if (aHDR_BC_RID <= 0) // Assortment: Color/Size Change
                        {
                            throw new Exception("Store Color Allocation SQL Access Request: HDR_BC_RID must be greater than 0"); // Assortment: Color/Size Change
                        }
                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Change
                        //_colorRID = aColorRID;
                        //_sizeRID =  Include.NoRID;
                        //_sequence = Include.NoRID;  // Not Used
                        _hdr_BC_RID = aHDR_BC_RID;
                        _hdr_BCSZ_KEY = Include.NoRID;
                        // End Assortment: Color/Size Change
                        break;
                    }
                case (eAllocationSqlAccessRequest.SizeAllocation):
                    {
                        //if (aColorRID <=0) // Assortment: Color/Size Change
                        if (aHDR_BC_RID <= 0) // Assortment: Color/Size Change
                        {
                            throw new Exception("Store Size Allocation SQL Access Request: HDR_BC_RID must be greater than 0"); // Assortment: Color/Size Change
                        }
                        //if (aSizeRID <=0) // Assortment: Color/Size Change
                        if (aHDR_BCSZ_KEY <= 0) // Assortment: Color/Size Change  
                        {
                            throw new Exception("Store Size Allocation SQL Access Request: HDR_BCSZ_Key must be greater than 0"); // Assortment: Color/Size Change
                        }
                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Change
                        //_colorRID = aColorRID;
                        //_sizeRID =  aSizeRID;
                        //_sequence = Include.NoRID;  // Not Used
                        _hdr_BC_RID = aHDR_BC_RID;
                        _hdr_BCSZ_KEY = aHDR_BCSZ_KEY;
                        // End Assortment: Color/Size Change
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown Allocation SQL Access Request: " + ((int)_allocationSqlAccessRequest).ToString());
                    }
            }
        }
        #endregion Constructors

        #region Properties
        public eAllocationSqlAccessRequest SqlAccess
        {
            get { return this._allocationSqlAccessRequest; }
        }
        public int HeaderRID
        {
            get { return _headerRID; }
        }
        public int PackRID
        {
            get { return _packRID; }
        }
        // Begin Assortment: Color/Size Key changes
        //public int ColorRID
        //{
        //	get {return _colorRID;}
        //}
        //public int SizeRID
        //{
        //	get {return _sizeRID;}
        //}
        //public int Sequence
        //{
        //   get { return _sequence; }
        //}
        public int HDR_BC_RID
        {
            get { return _hdr_BC_RID; }
        }
        public int HDR_BCSZ_KEY
        {
            get { return _hdr_BCSZ_KEY; }
        }
        // End Assortment: Color/Size Key Changes
        public StoreAllocationStructureStatus AllocationStructureStatus
        {
            get
            {
                return this._sass;
            }
            set
            {
                _sass = value;
            }
        }
        #endregion Properties

        #region Methods
        //internal void DeserializeHash(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, eSQL_StructureType aSQL_StructureType, byte[] aSerializedHash)
        //{
        //    if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
        //    {
        //        this.ThrowSqlAccessException(aSQL_StructureType, aAllocationSqlAccessRequest);
        //    }
        //    this._structureHash[(int)aSQL_StructureType] = ConvertBinaryObjectToHash(aSerializedHash);
        //}
        //internal byte[] SerializeHash(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, eSQL_StructureType aSQL_StructureType)
        //{
        //    if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
        //    {
        //        this.ThrowSqlAccessException(aSQL_StructureType, aAllocationSqlAccessRequest);
        //    }
        //    return ConvertHashToBinaryObject(_structureHash[(int)aSQL_StructureType]);
        //}
        #region ShipToDayStructure
        /// <summary>
        /// Static method that returns the ship to day structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreShipToDayStructure()
        {
            return eSQL_StructureType.ShipToDayStructure;
        }
        /// <summary>
        /// Adds a store ship to day
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aShipToDay">Store's Ship-To-Day</param>
        public void AddStoreShipToDay(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            DateTime aShipToDay)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.ShipToDayStructure, aAllocationSqlAccessRequest);
            }
            AddStoreShipToDay((short)aStoreRID, aShipToDay);
        }
        /// <summary>
        /// Adds a store ship to day
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aShipToDay">Store's Ship-To-Day</param>
        private void AddStoreShipToDay(
            short aStoreRID,
            DateTime aShipToDay)
        {
            if (_structureHash[(int)eSQL_StructureType.ShipToDayStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.ShipToDayStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.ShipToDayStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.ShipToDayStructure].Remove(aStoreRID);
                }
            }
            if (aShipToDay != Include.UndefinedDate)
            {
                ShipToDayStructure shipToDayStructure =
                    new ShipToDayStructure
                    (
                    aStoreRID,
                    aShipToDay
                    );

                _structureHash[(int)eSQL_StructureType.ShipToDayStructure].Add(aStoreRID, shipToDayStructure);
            }
        }
        /// <summary>
        /// Gets store ship to day
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Ship-To-Day</returns>
        public DateTime GetStoreShipToDay(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ShipToDayStructure] == null)
            {
                return Include.UndefinedDate;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ShipToDayStructure].Contains(storeRID))
            {
                return ((ShipToDayStructure)(_structureHash[(int)eSQL_StructureType.ShipToDayStructure])[storeRID]).ShipToDay;
            }
            return Include.UndefinedDate;
        }
        #endregion ShipToDayStructure

        #region GradeStructure
        /// <summary>
        /// static method that returns the grade structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreGradeStructure()
        {
            return eSQL_StructureType.GradeStructure;
        }
        /// <summary>
        /// Adds a store grade index
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aGradeIndex">Store's Grade Index</param>
        public void AddStoreGradeIndex(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aGradeIndex)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                ThrowSqlAccessException(eSQL_StructureType.GradeStructure, aAllocationSqlAccessRequest);
            }
            AddStoreGradeIndex((short)aStoreRID, aGradeIndex);
        }
        /// <summary>
        /// Adds a store grade index
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aGradeIndex">Store's Grade Index</param>
        private void AddStoreGradeIndex(
            short aStoreRID,
            int aGradeIndex)
        {
            if (_structureHash[(int)eSQL_StructureType.GradeStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.GradeStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.GradeStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.GradeStructure].Remove(aStoreRID);
                }
            }
            if (aGradeIndex != 0)
            {
                GradeStructure gradeStructure =
                    new GradeStructure
                    (
                    aStoreRID,
                    (short)aGradeIndex
                    );

                _structureHash[(int)eSQL_StructureType.GradeStructure].Add(aStoreRID, gradeStructure);
            }
        }
        /// <summary>
        /// Gets store grade index
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Grade Index</returns>
        public int GetStoreGradeIndex(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.GradeStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.GradeStructure].Contains(storeRID))
            {
                return (int)((GradeStructure)(_structureHash[(int)eSQL_StructureType.GradeStructure])[storeRID]).StoreGradeIndex;
            }
            return 0;
        }
        #endregion GradeStructure

        #region CapacityStructure
        /// <summary>
        /// static method that returns the capacity structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreCapacityStructure()
        {
            return eSQL_StructureType.CapacityStructure;
        }
        /// <summary>
        /// static method that returns the exceed capacity pct structure
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreExceedCapacityPctStructure()
        {
            return eSQL_StructureType.CapacityStructure;
        }

        /// <summary>
        /// Adds a store Capacity Structure
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aCapacity">Store's Capacity</param>
        /// <param name="aExceedCapacityPercent">The percent of capacity that a store may exceed</param>
        public void AddStoreCapacity(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aCapacity,
            double aExceedCapacityPercent)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.CapacityStructure, aAllocationSqlAccessRequest);
            }
            AddStoreCapacity((short)aStoreRID, aCapacity, aExceedCapacityPercent);
        }
        /// <summary>
        /// Adds a store Capacity Structure
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aCapacity">Store's Capacity</param>
        /// <param name="aExceedCapacityPercent">The percent of capacity that a store may exceed</param>
        private void AddStoreCapacity(
            short aStoreRID,
            int aCapacity,
            double aExceedCapacityPercent)
        {
            if (_structureHash[(int)eSQL_StructureType.CapacityStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.CapacityStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.CapacityStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.CapacityStructure].Remove(aStoreRID);
                }
            }
            if (aCapacity < int.MaxValue)
            {
                double exceedCapacityPercent = -1;
                if (aExceedCapacityPercent < double.MaxValue)
                {
                    exceedCapacityPercent = aExceedCapacityPercent;
                }
                CapacityStructure capacityStructure =
                    new CapacityStructure
                    (
                    aStoreRID,
                    aCapacity,
                    exceedCapacityPercent
                    );
                _structureHash[(int)eSQL_StructureType.CapacityStructure].Add(aStoreRID, capacityStructure);
            }
        }
        /// <summary>
        /// Gets store capacity
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Capacity</returns>
        public int GetStoreCapacity(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.CapacityStructure] == null)
            {
                return int.MaxValue;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.CapacityStructure].Contains(storeRID))
            {
                return (int)((CapacityStructure)(_structureHash[(int)eSQL_StructureType.CapacityStructure])[storeRID]).StoreCapacity;
            }
            return int.MaxValue;
        }
        /// <summary>
        /// Gets store capacity
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Capacity</returns>
        public double GetStoreExceedCapacityByPercent(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.CapacityStructure] == null)
            {
                return double.MaxValue;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.CapacityStructure].Contains(storeRID))
            {
                double exceedByPct = (double)((CapacityStructure)(_structureHash[(int)eSQL_StructureType.CapacityStructure])[storeRID]).ExceedCapacityPercent;
                if (exceedByPct < 0)
                {
                    return double.MaxValue;
                }
                return exceedByPct; // TT#1342 - FL Capacity not working
            }
            return double.MaxValue;
        }
        #endregion CapacityStructure

        #region GeneralAuditStructure
        /// <summary>
        /// static method that returns the general audit structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreGeneralAuditFlagsStructure()
        {
            return eSQL_StructureType.GeneralAuditStructure;
        }
        /// <summary>
        /// Adds a store's GeneralAuditFlags
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aGeneralAuditFlags">The store's AllocationGeneralAuditFlags cast as an integer</param>
        public void AddStoreGeneralAudit(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aGeneralAuditFlags)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.GeneralAuditStructure, aAllocationSqlAccessRequest);
            }
            AddStoreGeneralAudit((short)aStoreRID, aGeneralAuditFlags);
        }
        /// <summary>
        /// Adds a store's GeneralAuditFlags
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aGeneralAuditFlags">The store's AllocationGeneralAuditFlags cast as an integer</param>
        private void AddStoreGeneralAudit(
            short aStoreRID,
            int aGeneralAuditFlags)
        {
            if (_structureHash[(int)eSQL_StructureType.GeneralAuditStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.GeneralAuditStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.GeneralAuditStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.GeneralAuditStructure].Remove(aStoreRID);
                }
            }
            if (aGeneralAuditFlags != 0)
            {
                GeneralAuditStructure generalAuditStructure =
                    new GeneralAuditStructure
                    (
                    aStoreRID,
                    aGeneralAuditFlags
                    );

                _structureHash[(int)eSQL_StructureType.GeneralAuditStructure].Add(aStoreRID, generalAuditStructure);
            }
        }
        /// <summary>
        /// Gets store's general audit flags
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>General Audit Flags (cast resulting integer as AllocationGeneralAuditFlags)</returns>
        public int GetStoreGeneralAudit(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.GeneralAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.GeneralAuditStructure].Contains(storeRID))
            {
                return (int)((GeneralAuditStructure)(_structureHash[(int)eSQL_StructureType.GeneralAuditStructure])[storeRID]).StoreGeneralAuditFlags;
            }
            return 0;
        }
        #endregion GeneralAuditStructure

        #region AllocatedStructure
        /// <summary>
        /// static method that returns the quantity allocated structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreQtyAllocatedStructure()
        {
            return eSQL_StructureType.AllocatedStructure;
        }
        /// <summary>
        /// Adds a store's allocation
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to store</param>
        public void AddStoreAllocation(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aQtyAllocated)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.AllocatedStructure, aAllocationSqlAccessRequest);
            }
            AddStoreAllocation((short)aStoreRID, aQtyAllocated);
        }
        /// <summary>
        /// Adds a store's allocation
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to store</param>
        private void AddStoreAllocation(
            short aStoreRID,
            int aQtyAllocated)
        {
            if (_structureHash[(int)eSQL_StructureType.AllocatedStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.AllocatedStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.AllocatedStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.AllocatedStructure].Remove(aStoreRID);
                }
            }
            if (aQtyAllocated != 0)
            {
                AllocatedStructure allocatedStructure =
                    new AllocatedStructure
                    (
                    aStoreRID,
                    aQtyAllocated
                    );

                _structureHash[(int)eSQL_StructureType.AllocatedStructure].Add(aStoreRID, allocatedStructure);
            }
        }
        /// <summary>
        /// Gets store's quantity allocated
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Quantity Allocated</returns>
        public int GetStoreQtyAllocated(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.AllocatedStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.AllocatedStructure].Contains(storeRID))
            {
                return (int)((AllocatedStructure)(_structureHash[(int)eSQL_StructureType.AllocatedStructure])[storeRID]).QtyAllocated;
            }
            return 0;
        }
        #endregion AllocatedStructure

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        #region ImoStructure
        /// <summary>
        /// static method that returns the Imo structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreImoQtyAllocatedStructure()
        {
            return eSQL_StructureType.ImoStructure;
        }
        /// <summary>
        /// Adds a store's IMO Criteria
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Imo Quantity Allocated to store</param>
        public void AddStoreImoAllocation(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aImoMaxValue,
            int aImoMinShipQty,
            double aImoPctPackThreshold)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.ImoStructure, aAllocationSqlAccessRequest);
            }
            AddStoreImoAllocation((short)aStoreRID, aImoMaxValue, aImoMinShipQty, aImoPctPackThreshold);
        }
        /// <summary>
        /// Adds a store's IMO Criteria
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aImoMaxValue">Store's Item Max Value</param>
        /// <param name="aImoMinShipQty">Store's Minimum IMO Ship Quantity</param>
        /// <param name="aImoPctPackThreshold">Store's Pack Threshold</param>
        private void AddStoreImoAllocation(
            short aStoreRID,
            int aImoMaxValue,
            int aImoMinShipQty,
            double aImoPctPackThreshold)
        {
            if (_structureHash[(int)eSQL_StructureType.ImoStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.ImoStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.ImoStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.ImoStructure].Remove(aStoreRID);
                }
            }
            //if (aImoMaxValue != 0)   // TT#2225 - JELLIS - AnF VSW FWOS Enhancement pt 6 - Item Max = 0 issue
            if (aImoMaxValue < Include.LargestIntegerMaximum // TT#2225 - JELLIS - AnF VSW FWOS Enhancement pt 6 - Item Max = 0 issue
                && aImoMaxValue > -1) // TT#2225 - JELLIS - AnF VSW FWOS Enhancement pt 6 - Item Max = 0 issue
            {
                ImoStructure imoStructure =
                    new ImoStructure
                    (
                    aStoreRID,
                    aImoMaxValue,
                    aImoMinShipQty,
                    aImoPctPackThreshold
                    );

                _structureHash[(int)eSQL_StructureType.ImoStructure].Add(aStoreRID, imoStructure);
            }
        }
        /// <summary>
        /// Gets store's IMO Max Value
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>IMO Max Value</returns>
        public int GetStoreImoMaxValue(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ImoStructure] == null)
            {
                return int.MaxValue;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ImoStructure].Contains(storeRID))
            {
                return (int)((ImoStructure)(_structureHash[(int)eSQL_StructureType.ImoStructure])[storeRID]).ImoMaxValue;
            }
            return int.MaxValue;
        }
        /// <summary>
        /// Gets store's IMO Minimum Ship Qty
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>IMO Minimum Ship Qty</returns>
        public int GetStoreImoMinShipQty(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ImoStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ImoStructure].Contains(storeRID))
            {
                return (int)((ImoStructure)(_structureHash[(int)eSQL_StructureType.ImoStructure])[storeRID]).ImoMinShipQty;
            }
            return 0;
        }
        /// <summary>
        /// Gets store's IMO Pct Pack Threshold
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>IMO Pct Pack Threshold</returns>
        public double GetStoreImoPackThreshold(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ImoStructure] == null)
            {
                return .5d;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ImoStructure].Contains(storeRID))
            {
                return ((ImoStructure)(_structureHash[(int)eSQL_StructureType.ImoStructure])[storeRID]).ImoPctPackThreshold; // TT#1401 - JEllis - Urban Virtual Store Warehose pt 26A
            }
            return .5d;
        }
        #endregion ImoStructure


        #region ItemAllocatedStructure
        /// <summary>
        /// static method that returns the item quantity allocated structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreItemQtyAllocatedStructure()
        {
            return eSQL_StructureType.ItemAllocatedStructure;
        }
        /// <summary>
        /// Adds a store's item allocation
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Item Quantity Allocated to store</param>
        public void AddStoreItemAllocation(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aItemQtyAllocated)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.ItemAllocatedStructure, aAllocationSqlAccessRequest);
            }
            AddStoreItemAllocation((short)aStoreRID, aItemQtyAllocated);
        }
        /// <summary>
        /// Adds a store's Item allocation
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Item Quantity Allocated to store</param>
        private void AddStoreItemAllocation(
            short aStoreRID,
            int aItemQtyAllocated)
        {
            if (_structureHash[(int)eSQL_StructureType.ItemAllocatedStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.ItemAllocatedStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.ItemAllocatedStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.ItemAllocatedStructure].Remove(aStoreRID);
                }
            }
            if (aItemQtyAllocated != 0)
            {
                ItemAllocatedStructure itemAllocatedStructure =
                    new ItemAllocatedStructure
                    (
                    aStoreRID,
                    aItemQtyAllocated
                    );

                _structureHash[(int)eSQL_StructureType.ItemAllocatedStructure].Add(aStoreRID, itemAllocatedStructure);
            }
        }
        /// <summary>
        /// Gets store's item quantity allocated
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Item Quantity Allocated</returns>
        public int GetStoreItemQtyAllocated(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ItemAllocatedStructure] == null)
            {
                //return 0;                                  // TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
                return GetStoreQtyAllocated(aStoreRID);      // TT#2478 - JEllis - AnF VSW - Intransit Overstated due to VSW conversion issue // TT#196 - MD JEllis - Port Ver 4.0 fix TT#2478 to ver 4.2
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ItemAllocatedStructure].Contains(storeRID))
            {
                return (int)((ItemAllocatedStructure)(_structureHash[(int)eSQL_StructureType.ItemAllocatedStructure])[storeRID]).ItemQtyAllocated;
            }
            return 0;
        }
        #endregion ItemAllocatedStructure

        // begin TT#246 - MD - Jellis - AnF - VSW In Store Minimum
        #region ItemMinimumStructure
        /// <summary>
        /// static method that returns the item Minimum structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreItemMinimumStructure()
        {
            return eSQL_StructureType.ItemMinimumStructure;
        }
        /// <summary>
        /// Adds a store's item Minimum
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aItemMinimum">Store Item Minimum</param>
        /// <param name="aItemIdealMinimum">Store Item Ideal Minimum</param>
        public void AddStoreItemMinimum(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aItemMinimum,      // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
            int aItemIdealMinimum) // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.ItemMinimumStructure, aAllocationSqlAccessRequest);
            }
            AddStoreItemMinimum((short)aStoreRID, aItemMinimum, aItemIdealMinimum);  // TT#246 - MD - JEllis - In Store Minimum phase 2
        }
        /// <summary>
        /// Adds a store's Item Minimum
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aItemMinimum">Store Item Minimum</param>
        /// <param name="aItemIdealMinimum">Store Item Ideal Minimum</param>
        private void AddStoreItemMinimum(
            short aStoreRID,
            int aItemMinimum,       // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
            int aItemIdealMinimum)  // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        {
            if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.ItemMinimumStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.ItemMinimumStructure].Remove(aStoreRID);
                }
            }
            if (aItemMinimum != 0            // TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
                || aItemIdealMinimum != 0)   // TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
            {
                ItemMinimumStructure itemMinimumStructure =
                    new ItemMinimumStructure
                    (
                    aStoreRID,
                    aItemMinimum, // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
                    aItemIdealMinimum // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
                    );

                _structureHash[(int)eSQL_StructureType.ItemMinimumStructure].Add(aStoreRID, itemMinimumStructure);
            }
        }
        /// <summary>
        /// Gets store's item minimum
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Item Minimum</returns>
        public int GetStoreItemMinimum(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure] == null)
            {
                return 0;     
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure].Contains(storeRID))
            {
                return (int)((ItemMinimumStructure)(_structureHash[(int)eSQL_StructureType.ItemMinimumStructure])[storeRID]).ItemMinimum;
            }
            return 0;
        }
        // begin TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
        public int GetStoreItemIdealMinimum(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ItemMinimumStructure].Contains(storeRID))
            {
                return (int)((ItemMinimumStructure)(_structureHash[(int)eSQL_StructureType.ItemMinimumStructure])[storeRID]).ItemIdealMinimum;
            }
            return 0;
        }
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
        #endregion ItemMinimumStructure

        // end TT#246 - MD - Jellis - AnF - VSW In Store Minimum
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        #region AllocatedAuditStructure
        /// <summary>
        /// static method that returns the quantity allocated by auto structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreQtyAllocatedByAutoStructure()
        {
            return eSQL_StructureType.AllocatedAuditStructure;
        }
        /// <summary>
        /// static method that returns the quantity allocated by Rule structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreQtyAllocatedByRuleStructure()
        {
            return eSQL_StructureType.AllocatedAuditStructure;
        }
        /// <summary>
        /// Adds a store's allocation audit
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated by automated actions and methods</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated by Rule</param>
        public void AddStoreAllocationAudit(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.AllocatedAuditStructure, aAllocationSqlAccessRequest);
            }
            AddStoreAllocationAudit((short)aStoreRID, aQtyAllocatedByAuto, aQtyAllocatedByRule);
        }
        /// <summary>
        /// Adds a store's allocation audit
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated by automated actions and methods</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated by Rule</param>
        private void AddStoreAllocationAudit(
            short aStoreRID,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule)
        {
            if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.AllocatedAuditStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.AllocatedAuditStructure].Remove(aStoreRID);
                }
            }
            if (aQtyAllocatedByAuto != 0
                || aQtyAllocatedByRule != 0)
            {
                AllocatedAuditStructure allocatedAuditStructure =
                    new AllocatedAuditStructure
                    (
                    aStoreRID,
                    aQtyAllocatedByAuto,
                    aQtyAllocatedByRule
                    );

                _structureHash[(int)eSQL_StructureType.AllocatedAuditStructure].Add(aStoreRID, allocatedAuditStructure);
            }
        }
        /// <summary>
        /// Gets store's quantity allocated by rule
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Quantity Allocated By Rule</returns>
        public int GetStoreQtyAllocatedByRule(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure].Contains(storeRID))
            {
                return (int)((AllocatedAuditStructure)(_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure])[storeRID]).QtyAllocatedByRule;
            }
            return 0;
        }
        /// <summary>
        /// Gets store's quantity allocated by automated actions and methods
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Quantity Allocated By Auto</returns>
        public int GetStoreQtyAllocatedByAuto(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure].Contains(storeRID))
            {
                return (int)((AllocatedAuditStructure)(_structureHash[(int)eSQL_StructureType.AllocatedAuditStructure])[storeRID]).QtyAllocatedByAuto;
            }
            return 0;
        }
        #endregion AllocatedAuditStructure

        #region RuleStructure
        /// <summary>
        /// static method that returns the chosen rule structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreChosenRuleTypeStructure()
        {
            return eSQL_StructureType.RuleStructure;
        }
        /// <summary>
        /// static method that returns the chosen rule layer structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreChosenRuleLayerStructure()
        {
            return eSQL_StructureType.RuleStructure;
        }
        /// <summary>
        /// static method that returns the chosen rule Quantity structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreChosenRuleQtyStructure()
        {
            return eSQL_StructureType.RuleStructure;
        }
        /// <summary>
        /// Adds a store's chosen rule
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type (eRuleType) cast as an integer</param>
        /// <param name="aChosenRuleLayerID">Rule Layer ID where chosen rule resides</param>
        /// <param name="aChosenRuleQty">Quantity associated with this rule</param>
        public void AddStoreRule(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            eRuleType aChosenRuleType,
            int aChosenRuleLayerID,
            int aChosenRuleQty)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.RuleStructure, aAllocationSqlAccessRequest);
            }
            AddStoreRule((short)aStoreRID, (int)aChosenRuleType, (short)aChosenRuleLayerID, aChosenRuleQty);
        }
        /// <summary>
        /// Adds a store's chosen rule
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type (eRuleType) cast as an integer</param>
        /// <param name="aChosenRuleLayerID">Rule Layer ID where chosen rule resides</param>
        /// <param name="aChosenRuleQty">Quantity associated with this rule</param>
        private void AddStoreRule(
            short aStoreRID,
            int aChosenRuleType,
            short aChosenRuleLayerID,
            int aChosenRuleQty)
        {
            if (_structureHash[(int)eSQL_StructureType.RuleStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.RuleStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.RuleStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.RuleStructure].Remove(aStoreRID);
                }
            }
            if (aChosenRuleType != (int)eRuleType.None)
            {
                RuleStructure ruleStructure =
                    new RuleStructure
                    (
                    aStoreRID,
                    aChosenRuleType,
                    aChosenRuleLayerID,
                    aChosenRuleQty
                    );
                _structureHash[(int)eSQL_StructureType.RuleStructure].Add(aStoreRID, ruleStructure);
            }
        }
        /// <summary>
        /// Gets store chosen rule type
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Chosen Rule Type</returns>
        public eRuleType GetStoreChosenRule(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.RuleStructure] == null)
            {
                return eRuleType.None;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.RuleStructure].Contains(storeRID))
            {
                return (eRuleType)((RuleStructure)(_structureHash[(int)eSQL_StructureType.RuleStructure])[storeRID]).ChosenRuleType;
            }
            return eRuleType.None;
        }
        /// <summary>
        /// Gets store chosen rule layer ID
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Chosen Rule Layer ID</returns>
        public int GetStoreChosenRuleLayerID(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.RuleStructure] == null)
            {
                return Include.NoLayerID;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.RuleStructure].Contains(storeRID))
            {
                return (int)((RuleStructure)(_structureHash[(int)eSQL_StructureType.RuleStructure])[storeRID]).ChosenRuleLayerID;
            }
            return Include.NoLayerID;
        }
        /// <summary>
        /// Gets store chosen rule quantity
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Chosen Rule Qty</returns>
        public int GetStoreChosenRuleQty(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.RuleStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.RuleStructure].Contains(storeRID))
            {
                return (int)((RuleStructure)(_structureHash[(int)eSQL_StructureType.RuleStructure])[storeRID]).ChosenRuleUnits;
            }
            return 0;
        }
        #endregion RuleStructure

        #region NeedAuditStructure
        /// <summary>
        /// static method that returns the last need day structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreNeedDayStructure()
        {
            return eSQL_StructureType.NeedAuditStructure;
        }
        /// <summary>
        /// static method that returns the unit need before structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreNeedBeforeStructure()
        {
            return eSQL_StructureType.NeedAuditStructure;
        }
        /// <summary>
        /// static method that returns the Percent Need Before structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStorePercentNeedBeforeStructure()
        {
            return eSQL_StructureType.NeedAuditStructure;
        }
        /// <summary>
        /// Adds a store's need audit
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aNeedDay">Last Need day used during a need action</param>
        /// <param name="aNeedBefore">Unit Need before need action occurred</param>
        /// <param name="aPercentNeedBefore">Percent Need before need action occurred</param>
        public void AddStoreNeedAudit(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            DateTime aNeedDay,
            int aUnitNeedBefore,
            double aPercentNeedBefore)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.NeedAuditStructure, aAllocationSqlAccessRequest);
            }
            AddStoreNeedAudit((short)aStoreRID, aNeedDay, aUnitNeedBefore, aPercentNeedBefore);
        }
        /// <summary>
        /// Adds a store's need audit
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aNeedDay">Last Need day used during a need action</param>
        /// <param name="aNeedBefore">Unit Need before need action occurred</param>
        /// <param name="aPercentNeedBefore">Percent Need before need action occurred</param>
        private void AddStoreNeedAudit(
            short aStoreRID,
            DateTime aNeedDay,
            int aUnitNeedBefore,
            double aPercentNeedBefore)
        {
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.NeedAuditStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.NeedAuditStructure].Remove(aStoreRID);
                }
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                NeedAuditStructure needAuditStructure =
                    new NeedAuditStructure
                    (
                    aStoreRID,
                    aNeedDay,
                    aUnitNeedBefore,
                    aPercentNeedBefore
                    );

                _structureHash[(int)eSQL_StructureType.NeedAuditStructure].Add(aStoreRID, needAuditStructure);
            }
        }
        /// <summary>
        /// Gets store's last need day
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Last need day used during a need action</returns>
        public DateTime GetStoreNeedDay(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure] == null)
            {
                return Include.UndefinedDate;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure].Contains(storeRID))
            {
                return ((NeedAuditStructure)(_structureHash[(int)eSQL_StructureType.NeedAuditStructure])[storeRID]).NeedDay;
            }
            return Include.UndefinedDate;
        }
        /// <summary>
        /// Gets store's unit need before need action occurred
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Unit Need</returns>
        public int GetStoreNeed(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure].Contains(storeRID))
            {
                return (int)((NeedAuditStructure)(_structureHash[(int)eSQL_StructureType.NeedAuditStructure])[storeRID]).UnitNeedBefore;
            }
            return 0;
        }
        /// <summary>
        /// Gets store's percent need before need action occurred
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Percent Need</returns>
        public double GetStorePercentNeed(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.NeedAuditStructure].Contains(storeRID))
            {
                return ((NeedAuditStructure)(_structureHash[(int)eSQL_StructureType.NeedAuditStructure])[storeRID]).PercentNeedBefore;
            }
            return 0;
        }
        #endregion NeedAuditStructure

        #region MinimumStructure
        /// <summary>
        /// static method that returns the minimum structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreMinimumStructure()
        {
            return eSQL_StructureType.MinimumStructure;
        }
        /// <summary>
        /// Adds a store's minimum constraint
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aMinimum">Minimum Constraint</param>
        public void AddStoreMinimum(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aMinimum)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.MinimumStructure, aAllocationSqlAccessRequest);
            }
            AddStoreMinimum((short)aStoreRID, aMinimum);
        }
        /// <summary>
        /// Adds a store's minimum constraint
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aMinimum">Minimum Constraint</param>
        private void AddStoreMinimum(
            short aStoreRID,
            int aMinimum)
        {
            if (_structureHash[(int)eSQL_StructureType.MinimumStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.MinimumStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.MinimumStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.MinimumStructure].Remove(aStoreRID);
                }
            }
            if (aMinimum != 0)
            {
                MinimumStructure minimumStructure =
                    new MinimumStructure
                    (
                    aStoreRID,
                    aMinimum
                    );

                _structureHash[(int)eSQL_StructureType.MinimumStructure].Add(aStoreRID, minimumStructure);
            }
        }
        /// <summary>
        /// Gets store's minimum constraint
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Minimum</returns>
        public int GetStoreMinimum(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.MinimumStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.MinimumStructure].Contains(storeRID))
            {
                return (int)((MinimumStructure)(_structureHash[(int)eSQL_StructureType.MinimumStructure])[storeRID]).Minimum;
            }
            return 0;
        }
        #endregion MinimumStructure

        #region MaximumStructure
        /// <summary>
        /// static method that returns the maximum structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreMaximumStructure()
        {
            return eSQL_StructureType.MaximumStructure;
        }
        /// <summary>
        /// Adds a store's maximum constraint
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aMaximum">Maximum Constraint</param>
        public void AddStoreMaximum(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aMaximum)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.MaximumStructure, aAllocationSqlAccessRequest);
            }
            AddStoreMaximum((short)aStoreRID, aMaximum);
        }
        /// <summary>
        /// Adds a store's maximum constraint
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aMaximum">maximum Constraint</param>
        private void AddStoreMaximum(
            short aStoreRID,
            int aMaximum)
        {
            if (_structureHash[(int)eSQL_StructureType.MaximumStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.MaximumStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.MaximumStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.MaximumStructure].Remove(aStoreRID);
                }
            }
            if (aMaximum != int.MaxValue)
            {
                MaximumStructure maximumStructure =
                    new MaximumStructure
                    (
                    aStoreRID,
                    aMaximum
                    );

                _structureHash[(int)eSQL_StructureType.MaximumStructure].Add(aStoreRID, maximumStructure);
            }
        }
        /// <summary>
        /// Gets store's maximum constraint
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Maximum</returns>
        public int GetStoreMaximum(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.MaximumStructure] == null)
            {
                return int.MaxValue;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.MaximumStructure].Contains(storeRID))
            {
                return (int)((MaximumStructure)(_structureHash[(int)eSQL_StructureType.MaximumStructure])[storeRID]).Maximum;
            }
            return int.MaxValue;
        }
        #endregion MaximumStructure

        #region PrimaryMaxStructure
        /// <summary>
        /// static method that returns the primary maximum structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStorePrimaryMaxStructure()
        {
            return eSQL_StructureType.PrimaryMaxStructure;
        }
        /// <summary>
        /// Adds a store's minimum constraint
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aPrimaryMax">PrimaryMax Constraint</param>
        public void AddStorePrimaryMax(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aPrimaryMax)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.PrimaryMaxStructure, aAllocationSqlAccessRequest);
            }
            AddStorePrimaryMax((short)aStoreRID, aPrimaryMax);
        }
        /// <summary>
        /// Adds a store's PrimaryMax constraint
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aPrimaryMax">PrimaryMax Constraint</param>
        private void AddStorePrimaryMax(
            short aStoreRID,
            int aPrimaryMax)
        {
            if (_structureHash[(int)eSQL_StructureType.PrimaryMaxStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.PrimaryMaxStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.PrimaryMaxStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.PrimaryMaxStructure].Remove(aStoreRID);
                }
            }
            if (aPrimaryMax != int.MaxValue)
            {
                PrimaryMaxStructure primaryMaxStructure =
                    new PrimaryMaxStructure
                    (
                    aStoreRID,
                    aPrimaryMax
                    );

                _structureHash[(int)eSQL_StructureType.PrimaryMaxStructure].Add(aStoreRID, primaryMaxStructure);
            }
        }
        /// <summary>
        /// Gets store's PrimaryMax constraint
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>PrimaryMax</returns>
        public int GetStorePrimaryMax(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.PrimaryMaxStructure] == null)
            {
                return int.MaxValue;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.PrimaryMaxStructure].Contains(storeRID))
            {
                return (int)((PrimaryMaxStructure)(_structureHash[(int)eSQL_StructureType.PrimaryMaxStructure])[storeRID]).PrimaryMax;
            }
            return int.MaxValue;
        }
        #endregion PrimaryMaxStructure

        #region DetailAuditStructure
        /// <summary>
        /// static method that returns the store detail audit flags structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreDetailAuditFlagsStructure()
        {
            return eSQL_StructureType.DetailAuditStructure;
        }
        /// <summary>
        /// Adds a store's DetailAuditFlags
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aDetailAuditFlags">The store's AllocationDetailAuditFlags cast as an integer</param>
        public void AddStoreDetailAudit(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            uint aDetailAuditFlags) // TT#488 - MD - Jellis - Group Allocation
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.DetailAuditStructure, aAllocationSqlAccessRequest);
            }
            AddStoreDetailAudit((short)aStoreRID, aDetailAuditFlags);
        }
        /// <summary>
        /// Adds a store's DetailAuditFlags
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aDetailAuditFlags">The store's AllocationDetailAuditFlags cast as an integer</param>
        private void AddStoreDetailAudit(
            short aStoreRID,
            uint aDetailAuditFlags)   // TT#488 - Jellis - Group Allocation 
        {
            if (_structureHash[(int)eSQL_StructureType.DetailAuditStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.DetailAuditStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.DetailAuditStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.DetailAuditStructure].Remove(aStoreRID);
                }
            }
            if (aDetailAuditFlags != 0)
            {
                DetailAuditStructure detailAuditStructure =
                    new DetailAuditStructure
                    (
                    aStoreRID,
                    unchecked((int)aDetailAuditFlags) // TT#488 - Jellis - Group Allocation
                    //aDetailAuditFlags)              // TT#488 - Jellis - Group Allocation
                    );

                _structureHash[(int)eSQL_StructureType.DetailAuditStructure].Add(aStoreRID, detailAuditStructure);
            }
        }
        /// <summary>
        /// Gets store's detail audit flags
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Detail Audit Flags (cast resulting integer as AllocationDetailAuditFlags)</returns>
        public int GetStoreDetailAudit(int aStoreRID)
        {
            if (_structureHash[(int)eSQL_StructureType.DetailAuditStructure] == null)
            {
                return 0;
            }
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.DetailAuditStructure].Contains(storeRID))
            {
                return (int)((DetailAuditStructure)(_structureHash[(int)eSQL_StructureType.DetailAuditStructure])[storeRID]).StoreDetailAuditFlags;
            }
            return 0;
        }
        #endregion DetailAuditStructure

        #region ShippingStructure
        /// <summary>
        /// static method that returns the Quantity Shipped structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreQtyShippedStructure()
        {
            return eSQL_StructureType.ShippingStructure;
        }
        /// <summary>
        /// static method that returns the store shipped status flags structure type
        /// </summary>
        /// <returns></returns>
        public static eSQL_StructureType GetStoreShippedStatusFlagsStructure()
        {
            return eSQL_StructureType.ShippingStructure;
        }
        /// <summary>
        /// Adds a store's Shipping Results
        /// </summary>
        /// <param name="aAllocationSqlAccessRequest">eAllocationSqlAccessRequest established at instantiation</param>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyShipped">Quantity shipped to store</param>
        /// <param name="aShippingStatusFlags">Shipped Status Flags</param>
        public void AddStoreShipping(
            eAllocationSqlAccessRequest aAllocationSqlAccessRequest,
            int aStoreRID,
            int aQtyShipped,
            int aShippingStatusFlags)
        {
            if (aAllocationSqlAccessRequest != this._allocationSqlAccessRequest)
            {
                this.ThrowSqlAccessException(eSQL_StructureType.ShippingStructure, aAllocationSqlAccessRequest);
            }
            AddStoreShipping((short)aStoreRID, aQtyShipped, aShippingStatusFlags);
        }
        /// <summary>
        /// Adds a store's Shipping Results
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyShipped">Quantity shipped to store</param>
        /// <param name="aShippingStatusFlags">Shipped Status Flags</param>
        private void AddStoreShipping(
            short aStoreRID,
            int aQtyShipped,
            int aShippingStatusFlags)
        {
            if (_structureHash[(int)eSQL_StructureType.ShippingStructure] == null)
            {
                _structureHash[(int)eSQL_StructureType.ShippingStructure] = new Hashtable();
            }
            else
            {
                if (_structureHash[(int)eSQL_StructureType.ShippingStructure].Contains(aStoreRID))
                {
                    _structureHash[(int)eSQL_StructureType.ShippingStructure].Remove(aStoreRID);
                }
            }
            if (aQtyShipped > 0
                || aShippingStatusFlags != 0)
            {
                ShippingStructure shippingStructure =
                    new ShippingStructure
                    (
                    aStoreRID,
                    aShippingStatusFlags,
                    aQtyShipped
                    );

                _structureHash[(int)eSQL_StructureType.ShippingStructure].Add(aStoreRID, shippingStructure);
            }
        }
        /// <summary>
        /// Gets store's quantity shipped
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>Quantity Shipped</returns>
        public int GetStoreQtyShipped(int aStoreRID)
        {
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ShippingStructure] == null)
            {
                return 0;
            }
            if (_structureHash[(int)eSQL_StructureType.ShippingStructure].Contains(storeRID))
            {
                return (int)((ShippingStructure)(_structureHash[(int)eSQL_StructureType.ShippingStructure])[storeRID]).QtyShipped;
            }
            return 0;
        }
        /// <summary>
        /// Gets store's ship status
        /// </summary>
        /// <param name="aStoreRID">RID that identifies store</param>
        /// <returns>ShippingStatusFlags (cast as ShippingStatusFlags)</returns>
        public int GetStoreShippingStatus(int aStoreRID)
        {
            short storeRID = (short)aStoreRID;
            if (_structureHash[(int)eSQL_StructureType.ShippingStructure] == null)
            {
                return 0;
            }
            if (_structureHash[(int)eSQL_StructureType.ShippingStructure].Contains(storeRID))
            {
                return (int)((ShippingStructure)(_structureHash[(int)eSQL_StructureType.ShippingStructure])[storeRID]).ShippingStatusFlags;
            }
            return 0;
        }
        #endregion ShippingStructure

        #region Convert
        //private byte[] ConvertHashToBinaryObject(Hashtable aHashtable)
        //{
        //    byte[] binaryObject = null;
        //    if (aHashtable != null)
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            BinaryFormatter bf = new BinaryFormatter();
        //            bf.Serialize(ms, aHashtable);
        //            ms.Flush();
        //            binaryObject = ms.ToArray();
        //        }
        //    }
        //    return binaryObject;
        //}
        //private Hashtable ConvertBinaryObjectToHash (byte[] aSerializedHashtable)
        //{
        //    using(MemoryStream ms = new MemoryStream(aSerializedHashtable))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        // Begin TT#188 - JSmith - Rebrand
        //        bf.Binder = new MIDDeserializationBinder();
        //        AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
        //        {
        //            AssemblyName requestedName = new AssemblyName(e.Name);
        //            return Assembly.LoadFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\" + requestedName.Name + ".dll");
        //        };
        //        // End TT#188
        //        return (Hashtable)bf.Deserialize(ms);
        //    }
        //}

        //// Begin TT#188 - JSmith - Rebrand
        //sealed class MIDMRSToMIDRetailDeserializationBinder : SerializationBinder
        //{
        //    //Begin TT#1192 - JSmith - Batch Blocked
        //    string name;
        //    string version;
        //    string currentVersion;
        //    int index;
        //    //End TT#1192

        //    public override Type BindToType(string assemblyName, string typeName)
        //    {
        //        Type typeToDeserialize;

        //        // Begin TT#188 - STodd - Rebrand
        //        assemblyName = assemblyName.Replace("MID.MRS.", "MIDRetail.");
        //        typeName = typeName.Replace("MID.MRS.", "MIDRetail.");
        //        // Begin TT#188 - STodd - Rebrand

        //        //Begin TT#1192 - JSmith - Batch Blocked
        //        //typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
        //        //                typeName, assemblyName));
        //        string shortAssemblyName = assemblyName.Split(',')[0];
        //        typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, shortAssemblyName));
        //        //End TT#1192

        //        return typeToDeserialize;
        //    }

        //}
        //// End TT#188
		#endregion Convert 

        #region ThrowSqlAccessException
        private void ThrowSqlAccessException(eSQL_StructureType aSQL_StructureType, eAllocationSqlAccessRequest aAllocationSqlAccessRequest)
        {
            throw new Exception(
                "Sql Access Structure Type = " + ((int)aSQL_StructureType).ToString()
                + " Allocation SQL Access Request =  "
                + ((int)aAllocationSqlAccessRequest).ToString()
                + " must equal "
                + ((int)_allocationSqlAccessRequest).ToString()
                + " established at instantiation");
        }
        #endregion ThrowSqlAccessException
        #endregion Methods

        // Begin TT#739-MD -JSmith - Delete Stores
        #region InitializeDataChecks
        /// <summary>
        /// Determines of all Total Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsTotalAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (GetStoreGradeIndex(storeRID) != 0 ||
                    qtyAllocated > 0 ||
                    GetStoreQtyShipped(storeRID) > 0 ||
                    GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreChosenRule(storeRID) != eRuleType.None ||
                    GetStoreChosenRuleLayerID(storeRID) > -1 ||
                    GetStoreShipToDay(storeRID) != Include.UndefinedDate ||
                    GetStoreNeedDay(storeRID) != Include.UndefinedDate ||
                    GetStoreNeed(storeRID) < int.MaxValue ||
                    GetStorePercentNeed(storeRID) < double.MaxValue ||
                    GetStoreMinimum(storeRID) > 0 ||
                    GetStoreMaximum(storeRID) < int.MaxValue ||
                    GetStorePrimaryMax(storeRID) < int.MaxValue ||
                    GetStoreCapacity(storeRID) < int.MaxValue ||
                    GetStoreDetailAudit(storeRID) != 0 ||
                    GetStoreGeneralAudit(storeRID) != 0 ||
                    GetStoreShippingStatus(storeRID) != 0 ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreExceedCapacityByPercent(storeRID) > 0.0d ||
                    GetStoreImoMaxValue(storeRID) < int.MaxValue ||
                    GetStoreImoMinShipQty(storeRID) > 0 ||
                    GetStoreImoPackThreshold(storeRID) != .5 ||
                    GetStoreItemQtyAllocated(storeRID) != qtyAllocated)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Determines of all Detail Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsDetailAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (qtyAllocated > 0 ||
                    GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreChosenRuleLayerID(storeRID) > -1 ||
                    GetStoreChosenRule(storeRID) != eRuleType.None ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreNeedDay(storeRID) != Include.UndefinedDate ||
                    GetStoreNeed(storeRID) < int.MaxValue ||
                    GetStorePercentNeed(storeRID) < double.MaxValue ||
                    GetStoreMinimum(storeRID) > 0 ||
                    GetStoreMaximum(storeRID) < int.MaxValue ||
                    GetStorePrimaryMax(storeRID) < int.MaxValue ||
                    GetStoreDetailAudit(storeRID) > 0 ||
                    GetStoreItemQtyAllocated(storeRID) != qtyAllocated)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Determines of all Bulk Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsBulkAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (qtyAllocated > 0 ||
                    GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreChosenRuleLayerID(storeRID) > -1 ||
                    GetStoreChosenRule(storeRID) != eRuleType.None ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreNeedDay(storeRID) != Include.UndefinedDate ||
                    GetStoreNeed(storeRID) < int.MaxValue ||
                    GetStorePercentNeed(storeRID) < double.MaxValue ||
                    GetStoreMinimum(storeRID) > 0 ||
                    GetStoreMaximum(storeRID) < int.MaxValue ||
                    GetStorePrimaryMax(storeRID) < int.MaxValue ||
                    GetStoreDetailAudit(storeRID) > 0 ||
                    GetStoreItemQtyAllocated(storeRID) != qtyAllocated)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Determines of all Pack Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsPackAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (qtyAllocated > 0 ||
                GetStoreQtyShipped(storeRID) > 0 ||
                GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                GetStoreChosenRuleLayerID(storeRID) > -1 ||
                GetStoreChosenRule(storeRID) != eRuleType.None ||
                GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                GetStoreNeedDay(storeRID) != Include.UndefinedDate ||
                GetStoreNeed(storeRID) < int.MaxValue ||
                GetStorePercentNeed(storeRID) < double.MaxValue ||
                GetStoreMinimum(storeRID) > 0 ||
                GetStoreMaximum(storeRID) < int.MaxValue ||
                GetStorePrimaryMax(storeRID) < int.MaxValue ||
                GetStoreDetailAudit(storeRID) > 0 ||
                GetStoreShippingStatus(storeRID) > 0 ||
                GetStoreItemQtyAllocated(storeRID) != qtyAllocated)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Determines of all Bulk Color Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsBulkColorAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (qtyAllocated > 0 ||
                    GetStoreQtyShipped(storeRID) > 0 ||
                    GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreChosenRuleLayerID(storeRID) > -1 ||
                    GetStoreChosenRule(storeRID) != eRuleType.None ||
                    GetStoreQtyAllocatedByRule(storeRID) > 0 ||
                    GetStoreNeedDay(storeRID) != Include.UndefinedDate ||
                    GetStoreNeed(storeRID) < int.MaxValue ||
                    GetStorePercentNeed(storeRID) < double.MaxValue ||
                    GetStoreMinimum(storeRID) > 0 ||
                    GetStoreMaximum(storeRID) < int.MaxValue ||
                    GetStorePrimaryMax(storeRID) < int.MaxValue ||
                    GetStoreDetailAudit(storeRID) > 0 ||
                    GetStoreShippingStatus(storeRID) > 0 ||
                    GetStoreItemQtyAllocated(storeRID) != qtyAllocated ||
                    GetStoreItemIdealMinimum(storeRID) > 0)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Determines of all Bulk Color Size Allocation values are defaule values
        /// </summary>
        /// <returns>True: All values are default</returns>
        public bool IsBulkColorSizeAllocationAllDefaultValues(int storeRID)
        {
            int qtyAllocated;

            try
            {
                qtyAllocated = GetStoreQtyAllocated(storeRID);
                if (qtyAllocated > 0 ||
                    GetStoreQtyShipped(storeRID) > 0 ||
                    GetStoreQtyAllocatedByAuto(storeRID) > 0 ||
                    GetStoreMinimum(storeRID) > 0 ||
                    GetStoreMaximum(storeRID) < int.MaxValue ||
                    GetStorePrimaryMax(storeRID) < int.MaxValue ||
                    GetStoreDetailAudit(storeRID) > 0 ||
                    GetStoreShippingStatus(storeRID) > 0 ||
                    GetStoreItemQtyAllocated(storeRID) != qtyAllocated ||
                    GetStoreItemMinimum(storeRID) > 0 ||
                    GetStoreItemIdealMinimum(storeRID) > 0)
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        #endregion InitializeDataChecks
        // End TT#739-MD -JSmith - Delete Stores
    }
    // end MID Track 3994 Performance
    public class StoreAllocationUpdateRequest
    {
        // Fields
        private eAllocationSqlAccessRequest _allocationUpdateRequest;   // MID Track 3994 Performance
        private int _headerRID;
        private int _packRID;
        // Begin Assortment: Color/Size Key changes
        //private int _colorRID;
        //private int _sizeRID;
        private int _hdr_BC_RID;
        private int _hdr_BCSZ_Key;
        // End Assortment: Color/Size Key changes
        private StringBuilder _XML_Text;
        private bool _requestCompleted;
        private char[] _trimChar;


        // Contructor
        // Begin Assortment: Color/Size Key changes
        //public StoreAllocationUpdateRequest(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, int aHeaderRID, int aPackRID, int aColorRID, int aSizeRID) // MID Track 3994 Performance
        public StoreAllocationUpdateRequest(eAllocationSqlAccessRequest aAllocationSqlAccessRequest, int aHeaderRID, int aPackRID, int aHDR_BC_RID, int aHDR_BCSZ_Key) // MID Track 3994 Performance
        {
            this._trimChar = "0".ToCharArray();
            _XML_Text = new StringBuilder();
            _XML_Text.Append(Include.XML_Version);

            _allocationUpdateRequest = aAllocationSqlAccessRequest; // MID Track 3994 Performance
            if (aHeaderRID <= 0)
            {
                throw new Exception("Store Allocation Update Request: HeaderRID must be greater than 0");
            }
            _headerRID = aHeaderRID;
            switch (aAllocationSqlAccessRequest)  // MID Track 3994 Performance
            {
                case (eAllocationSqlAccessRequest.TotalAllocation): // MID Track 3994 Performance
                case (eAllocationSqlAccessRequest.BulkAllocation): // MID Track 3994 Performance
                case (eAllocationSqlAccessRequest.DetailAllocation): // MID Track 3994 Performance
                    {
                        _XML_Text.Append("<UpdateAllocation> <Update Name=\"Total\" H_RID=\"");
                        _XML_Text.Append(aHeaderRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");

                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Key changes
                        //_colorRID = Include.NoRID;
                        //_sizeRID = Include.NoRID;
                        _hdr_BC_RID = Include.NoRID;
                        _hdr_BCSZ_Key = Include.NoRID;
                        // End Assortment: Color/Size Key Changes
                        break;
                    }

                case (eAllocationSqlAccessRequest.PackAllocation): // MID Track 3994 Performance
                    {
                        if (aPackRID <= 0)
                        {
                            throw new Exception("Store Pack Allocation Update Request: Pack RID must be greater than 0");
                        }
                        _packRID = aPackRID;
                        // Begin Assortment: Color/Size Key changes
                        //_colorRID = Include.NoRID;
                        //_sizeRID = Include.NoRID;
                        _hdr_BC_RID = Include.NoRID;
                        _hdr_BCSZ_Key = Include.NoRID;
                        // End Assortment: Color/Size Key Changes
                        _XML_Text.Append("<UpdateAllocation> <Update Name=\"Pack\" H_RID=\"");
                        _XML_Text.Append(aHeaderRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");

                        _XML_Text.Append("P_RID=\"");
                        _XML_Text.Append(_packRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");
                        break;
                    }
                case (eAllocationSqlAccessRequest.ColorAllocation):  // MID Track 3994 Performance
                    {
                        //if (aColorRID <=0)  // Assortment: Color/Size Key chnages
                        if (aHDR_BC_RID <= 0) // Assortment: Color/Size Key chnages
                        {
                            throw new Exception("Store Color Allocation Update Request: HDR_BC_RID must be greater than 0");
                        }
                        _XML_Text.Append("<UpdateAllocation> <Update Name=\"Color\" H_RID=\"");
                        _XML_Text.Append(aHeaderRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");

                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Key chnages
                        //_colorRID = aColorRID;
                        //_sizeRID = Include.NoRID;
                        _hdr_BC_RID = aHDR_BC_RID;
                        _hdr_BCSZ_Key = Include.NoRID;

                        //_XML_Text.Append("C_RID=\"");
                        _XML_Text.Append("HDR_BC_RID=\"");
                        // End Assortment: Color/Size Key changes
                        _XML_Text.Append(_hdr_BC_RID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");
                        break;
                    }
                case (eAllocationSqlAccessRequest.SizeAllocation): // MID Track 3994 Performance
                    {
                        //if (aColorRID <=0)  // Assortment: Color/Size Key changes
                        if (aHDR_BC_RID <= 0)  // Assortment: Color/Size Key changes
                        {
                            throw new Exception("Store Size Allocation Update Request: HDR_BC_RID must be greater than 0");
                        }
                        //if (aSizeRID <=0)     // Assortment: Color/Size Key Changes
                        if (aHDR_BCSZ_Key <= 0)  // Assortment: Color/Size Key Changes
                        {
                            throw new Exception("Store Size Allocation Update Request: HDR_BCSZ_KEY must be greater than 0");
                        }
                        _XML_Text.Append("<UpdateAllocation> <Update Name=\"Size\" H_RID=\"");
                        _XML_Text.Append(aHeaderRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" ");

                        _packRID = Include.NoRID;
                        // Begin Assortment: Color/Size Key changes
                        //_colorRID = aColorRID;
                        //_sizeRID = aSizeRID;
                        //_XML_Text.Append("C_RID=\"");
                        //_XML_Text.Append(_colorRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        //_XML_Text.Append("\" S_RID=\"");
                        //_XML_Text.Append(_sizeRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _hdr_BC_RID = aHDR_BC_RID;
                        _hdr_BCSZ_Key = aHDR_BCSZ_Key;
                        _XML_Text.Append("HDR_BC_RID=\"");
                        _XML_Text.Append(_hdr_BC_RID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        _XML_Text.Append("\" HDR_BCSZ_KEY=\"");
                        _XML_Text.Append(_hdr_BCSZ_Key.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                        // End Assortment: Color/Size Key Changes

                        _XML_Text.Append("\" ");
                        break;
                    }
            }
            _XML_Text.Append("> ");
            _requestCompleted = false;
        }

        // Properties
        public bool RequestCompleted
        {
            get
            {
                return _requestCompleted;
            }
        }

        /// <summary>
        /// Gets the store allocation data to update.
        /// </summary>
        public StringBuilder AllocationUpdateRequestData
        {
            get
            {
                return this._XML_Text;
            }
        }


    
        /// <summary>
        /// Adds the ending XML parameters to the text and marks the request Completed.
        /// </summary>
        internal void FinalizeRequest()
        {
            _requestCompleted = true;
            _XML_Text.Append("</Update> </UpdateAllocation> ");
        }

        /// <summary>
        /// Adds a store's total allocation data for update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aGradeIdx">Assigned Grade Index for the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyShipped">Quantity Shipped to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated via rules</param>
        /// <param name="aChosenRuleLayerID">Chosen Rule Layer ID for the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type for the store</param>
        /// <param name="aChosenRuleQtyAllocated">Chosen Rule Quantity Allocated</param>
        /// <param name="aShipDay">Ship day for the store</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aCapacity">Capacity for the store</param>
        /// <param name="aCapacityExceedByPct">Allow store to exceed capacity by percent</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aGeneralAuditFlags">General audit flags for the store</param>
        /// <param name="aShipFlags">Ship flags for the store</param>
        /// <param name="aImoMaxValue">IMO Maximum Value (aka VSW Maximum Value)</param>
        /// <param name="aImoMinShipQty">IMO Minimum ship quantity</param>
        /// <param name="aImoPackThresh">IMO Pack Threshold</param>
        /// <param name="aItemUnitsAllocated">Item (VSW) units allocated</param>
        public void AddStoreTotalData(
            int aStoreRID,
            int aGradeIdx,
            int aQtyAllocated,
            int aQtyShipped,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule,
            int aChosenRuleLayerID,
            eRuleType aChosenRuleType,
            int aChosenRuleQtyAllocated,
            DateTime aShipDay,
            DateTime aNeedDay,
            int aUnitNeed,
            double aPercentNeed,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            int aCapacity,
            double aCapacityExceedByPct,
            ushort aDetailAuditFlags,
            //byte aGeneralAuditFlags,  // TT#2464 - JEllis - AnF VSW - unrelated - General Audit flags too small (TT#186)
            // begin TT#2440 - JEllis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //byte aShipFlags)
            ushort aGeneralAuditFlags,  // TT#2464 - JEllis - AnF VSW - unrelated - General Audit flags too small (TT#186)
            byte aShipFlags,
            int aImoMaxValue,
            int aImoMinShipQty,
            double aImoPackThresh,
            int aItemUnitsAllocated)
            // end TT#2440 - JEllis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.TotalAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStoreTotalData method requires the Allocation Update Request be 'TotalAllocation'"); // MID Track 3994 Performance
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store total data after Request processed and complete");
            }
            _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            _XML_Text.Append("\" Gr=\""); _XML_Text.Append(aGradeIdx.ToString(CultureInfo.CurrentUICulture));
            if (aQtyAllocated > 0)
            {
                _XML_Text.Append("\" Qa=\""); _XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyShipped > 0)
            {
                _XML_Text.Append("\" Qs=\""); _XML_Text.Append(aQtyShipped.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                _XML_Text.Append("\" Qa_a=\""); _XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                _XML_Text.Append("\" Qa_r=\""); _XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleLayerID > -1)
            {
                _XML_Text.Append("\" Cr_l=\""); _XML_Text.Append(aChosenRuleLayerID.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleType != eRuleType.None)
            {
                _XML_Text.Append("\" Cr_t=\""); _XML_Text.Append(((int)aChosenRuleType).ToString(CultureInfo.CurrentUICulture));

            }
            if (aQtyAllocatedByRule > 0)
            {
                _XML_Text.Append("\" Cr_q=\""); _XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aShipDay != Include.UndefinedDate)
            {
                _XML_Text.Append("\" S_dy=\""); _XML_Text.Append(aShipDay.ToShortDateString());
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                _XML_Text.Append("\" N_dy=\""); _XML_Text.Append(aNeedDay.ToShortDateString());
            }
            if (aUnitNeed < int.MaxValue)
            {
                _XML_Text.Append("\" U_nd=\""); _XML_Text.Append(aUnitNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPercentNeed < double.MaxValue)
            {
                _XML_Text.Append("\" P_nd=\""); _XML_Text.Append(aPercentNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMinimum > 0)
            {
                _XML_Text.Append("\" Mn=\""); _XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                _XML_Text.Append("\" Mx=\""); _XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                _XML_Text.Append("\" P_Mx=\""); _XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aCapacity < int.MaxValue)
            {
                _XML_Text.Append("\" Cap=\""); _XML_Text.Append(aCapacity.ToString(CultureInfo.CurrentUICulture));
            }
            if (aCapacityExceedByPct > 0.0d)
            {
                if (aCapacityExceedByPct == double.MaxValue)
                {
                    double minusOne = -1.0d;   // These lines added because database rejected the double Max Value as too large
                    _XML_Text.Append("\" E_cap=\""); _XML_Text.Append(minusOne.ToString(CultureInfo.CurrentUICulture));
                }
                else
                {
                    _XML_Text.Append("\" E_cap=\""); _XML_Text.Append(aCapacityExceedByPct.ToString(CultureInfo.CurrentUICulture));
                }
            }
            if ((int)aGeneralAuditFlags != 0) // MID Track 3994 Performance
            {
                _XML_Text.Append("\" Gf=\""); _XML_Text.Append(((int)aGeneralAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags != 0) // MID Track 3994 Performance
            {
                _XML_Text.Append("\" Af=\""); _XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aShipFlags != 0) // MID Track 3994 Performance
            {
                _XML_Text.Append("\" Sf=\""); _XML_Text.Append(((int)aShipFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            if (aImoMaxValue < int.MaxValue)
            {
                _XML_Text.Append("\" I_mx=\""); _XML_Text.Append(aImoMaxValue.ToString(CultureInfo.CurrentUICulture));
            }
            if (aImoMinShipQty > 0)
            {
                _XML_Text.Append("\" I_sq=\""); _XML_Text.Append(aImoMinShipQty.ToString(CultureInfo.CurrentUICulture));
            }
            if (aImoPackThresh != .5)
            {
                _XML_Text.Append("\" I_ppt=\""); _XML_Text.Append(aImoPackThresh.ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemUnitsAllocated > 0)
            if (aItemUnitsAllocated != aQtyAllocated)
                // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                _XML_Text.Append("\" Ia=\""); _XML_Text.Append(aItemUnitsAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            _XML_Text.Append("\" /> ");
        }

        /// <summary>
        /// Adds a store's detail allocation for update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated via rules</param>
        /// <param name="aChosenRuleLayerID">Chosen Rule Layer ID for the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type for the store</param>
        /// <param name="aChosenRuleQtyAllocated">Chosen Rule Quantity Allocated</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aItemUnitsAllocated">Item (VSW) Units allocated</param>
        public void AddStoreDetailData(
            int aStoreRID,
            int aQtyAllocated,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule,
            int aChosenRuleLayerID,
            eRuleType aChosenRuleType,
            int aChosenRuleQtyAllocated,
            DateTime aNeedDay,
            int aUnitNeed,
            double aPercentNeed,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //ushort aDetailAuditFlags)
            ushort aDetailAuditFlags,
            int aItemUnitsAllocated)
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.DetailAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStoreDetailData method requires the Allocation Update Request be 'DetailAllocation'"); // MID track 3994 Performance
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store detail data after Request processed and complete");
            }
            StringBuilder XML_Text = new StringBuilder();
            //			_XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            if (aQtyAllocated > 0)
            {
                XML_Text.Append("\" Qa=\""); XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                XML_Text.Append("\" Qa_a=\""); XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Qa_r=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleLayerID > -1)
            {
                XML_Text.Append("\" Cr_l=\""); XML_Text.Append(aChosenRuleLayerID.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleType != eRuleType.None)
            {
                XML_Text.Append("\" Cr_t=\""); XML_Text.Append(((int)aChosenRuleType).ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Cr_q=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                XML_Text.Append("\" N_dy=\""); XML_Text.Append(aNeedDay.ToShortDateString());
            }
            if (aUnitNeed < int.MaxValue)
            {
                XML_Text.Append("\" U_nd=\""); XML_Text.Append(aUnitNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPercentNeed < double.MaxValue)
            {
                XML_Text.Append("\" P_nd=\""); XML_Text.Append(aPercentNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMinimum > 0)
            {
                XML_Text.Append("\" Mn=\""); XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                XML_Text.Append("\" Mx=\""); XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                XML_Text.Append("\" P_Mx=\""); XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags > 0)
            {
                XML_Text.Append("\" Af=\""); XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemUnitsAllocated > 0)
            if (aItemUnitsAllocated != aQtyAllocated)
            // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                XML_Text.Append("\" Ia=\""); XML_Text.Append(aItemUnitsAllocated.ToString(CultureInfo.CurrentUICulture));  // TT#2454 - Jellis - Anf VSW - VSW disappears after Relieve IT pt 2 // TT#184 MD Jellis port TT#2454 to ver 4.2 
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            if (XML_Text.Length > 0)
            {
                _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                _XML_Text.Append(XML_Text.ToString());
                _XML_Text.Append("\" /> ");
            }
        }

        /// <summary>
        /// Adds a store's bulk allocation data for update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated via rules</param>
        /// <param name="aChosenRuleLayerID">Chosen Rule Layer ID for the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type for the store</param>
        /// <param name="aChosenRuleQtyAllocated">Chosen Rule Quantity Allocated</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aItemUnitsAllocated">Item (VSW) units allocated</param>
        public void AddStoreBulkData(
            int aStoreRID,
            int aQtyAllocated,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule,
            int aChosenRuleLayerID,
            eRuleType aChosenRuleType,
            int aChosenRuleQtyAllocated,
            DateTime aNeedDay,
            int aUnitNeed,
            double aPercentNeed,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //ushort aDetailAuditFlags)
            ushort aDetailAuditFlags,
            int aItemUnitsAllocated)
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.BulkAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStoreBulkData method requires the Allocation Update Request be 'BulkAllocationUpdate'");
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store bulk data after Request processed and complete");
            }
            //			_XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            StringBuilder XML_Text = new StringBuilder();
            if (aQtyAllocated > 0)
            {
                XML_Text.Append("\" Qa=\""); XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                XML_Text.Append("\" Qa_a=\""); XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Qa_r=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleLayerID > -1)
            {
                XML_Text.Append("\" Cr_l=\""); XML_Text.Append(aChosenRuleLayerID.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleType != eRuleType.None)
            {
                XML_Text.Append("\" Cr_t=\""); XML_Text.Append(((int)aChosenRuleType).ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Cr_q=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                XML_Text.Append("\" N_dy=\""); XML_Text.Append(aNeedDay.ToShortDateString());
            }
            if (aUnitNeed < int.MaxValue)
            {
                XML_Text.Append("\" U_nd=\""); XML_Text.Append(aUnitNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPercentNeed < double.MaxValue)
            {
                XML_Text.Append("\" P_nd=\""); XML_Text.Append(aPercentNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMinimum > 0)
            {
                XML_Text.Append("\" Mn=\""); XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                XML_Text.Append("\" Mx=\""); XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                XML_Text.Append("\" P_Mx=\""); XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags > 0)
            {
                XML_Text.Append("\" Af=\""); XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemUnitsAllocated > 0)
            if (aItemUnitsAllocated != aQtyAllocated)
            // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                XML_Text.Append("\" Ia=\""); XML_Text.Append(aItemUnitsAllocated.ToString(CultureInfo.CurrentUICulture));  // TT#2454 - JEllis AnF VSW - VSW Qty disapears after Relieve IT pt 2 // TT#184 MD Jellis port TT#2454 to ver 4.2 
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            if (XML_Text.Length > 0)
            {
                _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                _XML_Text.Append(XML_Text.ToString());
                _XML_Text.Append("\" /> ");
            }
        }

        /// <summary>
        /// Adds the pack allocation for a store to the update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyShipped">Quantity Shipped to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated via rules</param>
        /// <param name="aChosenRuleLayerID">Chosen Rule Layer ID for the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type for the store</param>
        /// <param name="aChosenRuleQtyAllocated">Chosen Rule Quantity Allocated</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aShipFlags">Ship flags for the store</param>
        /// <param name="aItemPacksAllocated">Item (VSW) packs allocated</param>
        public void AddStorePackData(
            int aStoreRID,
            int aQtyAllocated,
            int aQtyShipped,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule,
            int aChosenRuleLayerID,
            eRuleType aChosenRuleType,
            int aChosenRuleQtyAllocated,
            DateTime aNeedDay,
            int aUnitNeed,
            double aPercentNeed,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            ushort aDetailAuditFlags,
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //byte aShipFlags)
            byte aShipFlags,
            int aItemPacksAllocated)
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.PackAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStorePackData method requires the Allocation Update Request be 'PackAllocationUpdate'");
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store pack data after Request processed and complete");
            }
            //			_XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            StringBuilder XML_Text = new StringBuilder();
            if (aQtyAllocated > 0)
            {
                XML_Text.Append("\" Qa=\""); XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyShipped > 0)
            {
                XML_Text.Append("\" Qs=\""); XML_Text.Append(aQtyShipped.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                XML_Text.Append("\" Qa_a=\""); XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Qa_r=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleLayerID > -1)
            {
                XML_Text.Append("\" Cr_l=\""); XML_Text.Append(aChosenRuleLayerID.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleType != eRuleType.None)
            {
                XML_Text.Append("\" Cr_t=\""); XML_Text.Append(((int)aChosenRuleType).ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Cr_q=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                XML_Text.Append("\" N_dy=\""); XML_Text.Append(aNeedDay.ToShortDateString());
            }
            if (aUnitNeed < int.MaxValue)
            {
                XML_Text.Append("\" U_nd=\""); XML_Text.Append(aUnitNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPercentNeed < double.MaxValue)
            {
                XML_Text.Append("\" P_nd=\""); XML_Text.Append(aPercentNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMinimum > 0)
            {
                XML_Text.Append("\" Mn=\""); XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                XML_Text.Append("\" Mx=\""); XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                XML_Text.Append("\" P_Mx=\""); XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags > 0)
            {
                XML_Text.Append("\" Af=\""); XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aShipFlags > 0)
            {
                XML_Text.Append("\" Sf=\""); XML_Text.Append(((int)aShipFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemPacksAllocated > 0)
            if (aItemPacksAllocated != aQtyAllocated)
            // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                XML_Text.Append("\" Ia=\""); XML_Text.Append(aItemPacksAllocated.ToString(CultureInfo.CurrentUICulture));  // TT#2454 - JEllis AnF VSW - VSW Qty disapears after Relieve IT pt 2// TT#184 MD Jellis port TT#2454 to ver 4.2  
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            if (XML_Text.Length > 0)
            {
                _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                _XML_Text.Append(XML_Text.ToString());
                _XML_Text.Append("\" /> ");
            }
            //			_XML_Text.Append("\" /> ");
        }

        /// <summary>
        /// Adds the color allocation for a store to the update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyShipped">Quantity Shipped to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated via rules</param>
        /// <param name="aChosenRuleLayerID">Chosen Rule Layer ID for the store</param>
        /// <param name="aChosenRuleType">Chosen Rule Type for the store</param>
        /// <param name="aChosenRuleQtyAllocated">Chosen Rule Quantity Allocated</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aShipFlags">Ship flags for the store</param>
        /// <param name="aItemUnitsAllocated">Item (VSW) Units allocated</param>
        public void AddStoreColorData(
            int aStoreRID,
            int aQtyAllocated,
            int aQtyShipped,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule,
            int aChosenRuleLayerID,
            eRuleType aChosenRuleType,
            int aChosenRuleQtyAllocated,
            DateTime aNeedDay,
            int aUnitNeed,
            double aPercentNeed,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            ushort aDetailAuditFlags,
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //byte aShipFlags)
            byte aShipFlags,
            int aItemUnitsAllocated)
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.ColorAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStoreColorData method requires the Allocation Update Request be 'ColorAllocation'"); // MID Track 3994 Performance
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store color data after Request processed and complete");
            }
            //			_XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            StringBuilder XML_Text = new StringBuilder();
            if (aQtyAllocated > 0)
            {
                XML_Text.Append("\" Qa=\""); XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyShipped > 0)
            {
                XML_Text.Append("\" Qs=\""); XML_Text.Append(aQtyShipped.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                XML_Text.Append("\" Qa_a=\""); XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Qa_r=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleLayerID > -1)
            {
                XML_Text.Append("\" Cr_l=\""); XML_Text.Append(aChosenRuleLayerID.ToString(CultureInfo.CurrentUICulture));
            }
            if (aChosenRuleType != eRuleType.None)
            {
                XML_Text.Append("\" Cr_t=\""); XML_Text.Append(((int)aChosenRuleType).ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByRule > 0)
            {
                XML_Text.Append("\" Cr_q=\""); XML_Text.Append(aQtyAllocatedByRule.ToString(CultureInfo.CurrentUICulture));
            }
            if (aNeedDay != Include.UndefinedDate)
            {
                XML_Text.Append("\" N_dy=\""); XML_Text.Append(aNeedDay.ToShortDateString());
            }
            if (aUnitNeed < int.MaxValue)
            {
                XML_Text.Append("\" U_nd=\""); XML_Text.Append(aUnitNeed.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPercentNeed < double.MaxValue)
            {
                XML_Text.Append("\" P_nd=\""); XML_Text.Append(aPercentNeed.ToString(CultureInfo.CurrentUICulture));
            }
            //			if (aMinimum < 0)
            if (aMinimum > 0)
            {
                XML_Text.Append("\" Mn=\""); XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                XML_Text.Append("\" Mx=\""); XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                XML_Text.Append("\" P_Mx=\""); XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags > 0)
            {
                XML_Text.Append("\" Af=\""); XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aShipFlags > 0)
            {
                XML_Text.Append("\" Sf=\""); XML_Text.Append(((int)aShipFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemUnitsAllocated > 0)
            if (aItemUnitsAllocated != aQtyAllocated)
            // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                XML_Text.Append("\" Ia=\""); XML_Text.Append(aItemUnitsAllocated.ToString(CultureInfo.CurrentUICulture)); // TT#2454 - JEllis AnF VSW - VSW Qty disapears after Relieve IT pt 2 // TT#184 MD Jellis port TT#2454 to ver 4.2 
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            if (XML_Text.Length > 0)
            {
                _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                _XML_Text.Append(XML_Text.ToString());
                _XML_Text.Append("\" /> ");
            }
            //			_XML_Text.Append("\" /> ");
        }

        /// <summary>
        /// Adds a store's size allocation data for update
        /// </summary>
        /// <param name="aStoreRID">RID of the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        /// <param name="aQtyShipped">Quantity Shipped to the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated via auto allocation</param>
        /// <param name="aNeedDay">Need day for the store</param>
        /// <param name="aUnitNeed">Unit need before allocation</param>
        /// <param name="aPercentNeed">Percent Need before allocation</param>
        /// <param name="aMinimum">Minimum allocation for the store</param>
        /// <param name="aMaximum">Maximum allocation for the store</param>
        /// <param name="aPrimaryMaximum">Primary maximum allocation for the store</param>
        /// <param name="aDetailAuditFlags">Detail audit flags for the store</param>
        /// <param name="aShipFlags">Ship flags for the store</param>
        /// <param name="aItemUnitsAllocated">Item (VSW) units allocated</param>
        /// <param name="aItemMinimum">Item Minimum Allocation</param>
        /// <param name="aItemIdealMinimum">Item Ideal Minimum</param>
        public void AddStoreSizeData(
            int aStoreRID,
            int aQtyAllocated,
            int aQtyShipped,
            int aQtyAllocatedByAuto,
            int aMinimum,
            int aMaximum,
            int aPrimaryMaximum,
            ushort aDetailAuditFlags,
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            //byte aShipFlags)
            byte aShipFlags,
            int aItemUnitsAllocated, // TT#246 - MD - JEllis - AnF VSW In-Store minimum
            int aItemMinimum,        // TT#246 - MD - JEllis - AnF VSW In-Store minimum // TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
            int aItemIdealMinimum)   // TT#246 - MD - JEllis - AnF VSW In Store minimum phase 2
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
        {
            if (this._allocationUpdateRequest != eAllocationSqlAccessRequest.SizeAllocation) // MID Track 3994 Performance
            {
                throw new Exception("AddStoreSizeData method requires the Allocation Update Request be 'SizeAllocationUpdate'");
            }
            if (this._requestCompleted)
            {
                throw new Exception("Attempt to add store size data after Request processed and complete");
            }
            //			_XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
            StringBuilder XML_Text = new StringBuilder();
            if (aQtyAllocated > 0)
            {
                XML_Text.Append("\" Qa=\""); XML_Text.Append(aQtyAllocated.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyShipped > 0)
            {
                XML_Text.Append("\" Qs=\""); XML_Text.Append(aQtyShipped.ToString(CultureInfo.CurrentUICulture));
            }
            if (aQtyAllocatedByAuto > 0)
            {
                XML_Text.Append("\" Qa_a=\""); XML_Text.Append(aQtyAllocatedByAuto.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMinimum > 0)
            {
                XML_Text.Append("\" Mn=\""); XML_Text.Append(aMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aMaximum < int.MaxValue)
            {
                XML_Text.Append("\" Mx=\""); XML_Text.Append(aMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if (aPrimaryMaximum < int.MaxValue)
            {
                XML_Text.Append("\" P_Mx=\""); XML_Text.Append(aPrimaryMaximum.ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aDetailAuditFlags > 0)
            {
                XML_Text.Append("\" Af=\""); XML_Text.Append(((int)aDetailAuditFlags).ToString(CultureInfo.CurrentUICulture));
            }
            if ((int)aShipFlags > 0)
            {
                XML_Text.Append("\" Sf=\""); XML_Text.Append(((int)aShipFlags).ToString(CultureInfo.CurrentUICulture));
            }
            // begin TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            //if (aItemUnitsAllocated > 0)
            if (aItemUnitsAllocated != aQtyAllocated)
            // end TT#2464 - JEllis - AnF VSW - zero Store Qty and non-zero VSW qty disappear after Relieve IT (TT#186)
            {
                XML_Text.Append("\" Ia=\""); XML_Text.Append(aItemUnitsAllocated.ToString(CultureInfo.CurrentUICulture)); // TT#2454 - JEllis AnF VSW - VSW Qty disapears after Relieve IT pt 2 // TT#184 MD Jellis port TT#2454 to ver 4.2 
            }
            // end TT#2440 - Jellis - AnF VSW - After Relieve IT VSW info disappears (TT#185)
            // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum
            if (aItemMinimum > 0)
            {
                XML_Text.Append("\" Imn=\""); XML_Text.Append(aItemMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum
            // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum phase 2
            if (aItemIdealMinimum > 0)
            {
                XML_Text.Append("\" Iim=\""); XML_Text.Append(aItemIdealMinimum.ToString(CultureInfo.CurrentUICulture));
            }
            // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum

            if (XML_Text.Length > 0)
            {
                _XML_Text.Append("<Str S_RID=\""); _XML_Text.Append(aStoreRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                _XML_Text.Append(XML_Text.ToString());
                _XML_Text.Append("\" /> ");
            }
            //			_XML_Text.Append("\" /> ");
        }
    }
}
