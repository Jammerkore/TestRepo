using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;



namespace MIDRetail.Data
{
    #region HeaderConflict
    // begin TT#1185 - Verify ENQ before Update
        /// <summary>
    /// The HeaderConflict class stores information regarding a conflict during the enqueue of a header.
    /// </summary>
    [Serializable]
    public class HeaderConflict
    {
        //=======
        // FIELDS
        //=======
        private string _headerID;
        private int _headerRID;
        private int _userRID;
        private int _threadID;
        private long _tranID;
        private DateTime _dateTime;

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of HeaderConflict using the user RID of the conflict.
        /// </summary>
        /// <param name="aUserRID">
        /// The user RID of the conflict.
        /// </param>
        public HeaderConflict(string aHeaderID, int aHeaderRID, int aUserRID, int aThreadID, long aTranID, DateTime aDateTime)
        {
            _headerID = aHeaderID;
            _headerRID = aHeaderRID;
            _userRID = aUserRID;
            _threadID = aThreadID;
            _tranID = aTranID;
            _dateTime = aDateTime;
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets the Header ID in conflict
        /// </summary>
        public string HeaderID
        {
            get
            {
                return _headerID;
            }
        }
        /// <summary>
        /// Gets the header RID of the conflict.
        /// </summary>
        public int HeaderRID
        {
            get
            {
                return _headerRID;
            }
        }
        /// <summary>
        /// Gets the user RID of the conflict.
        /// </summary>
        public int UserRID
        {
            get
            {
                return _userRID;
			}
		}
        /// <summary>
        /// Gets the Thread ID of the conflict
        /// </summary>
        public int ThreadID
        {
            get
            {
                return _threadID;
            }
        }
        /// <summary>
        ///  Gets the Transaction ID of the conflict
        /// </summary>
        public long TransactionID
        {
            get
            {
                return _tranID;
            }
        }
        /// <summary>
        /// Gets the date-time when the enqueue occurred
        /// </summary>
        public DateTime DateTimeEnqueued
        {
            get
            {
                return _dateTime;
            }
        }

        //========
        // METHODS
        //========
    }

    // end TT#1185 - Verify ENQ before update
#endregion HeaderConflict

	public partial class MIDEnqueue : DataLayer
	{
        private char _trimChar; // TT#1185 Verify ENQ before Update
		public MIDEnqueue() : base()
		{
            _trimChar = "0".ToCharArray()[0];  // TT#1185 Verify ENQ before Update

		}
       

        // begin TT#1185 Verify ENQ before Update
        public DataTable Enqueue_Read(Resource aResource)
        {
            return StoredProcedures.MID_ENQUEUE_READ_FOR_HEADER.Read(_dba, RID: aResource.LockRID);
        }
        // end TT#1185 Verify ENQ before Update
		public DataTable Enqueue_Read(eLockType aLockType, int aRID)
		{
            return StoredProcedures.MID_ENQUEUE_READ_FROM_TYPE.Read(_dba,
                                                                    RID: aRID,
                                                                    ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture)
                                                                    );
		}

        public DataTable Enqueue_Read(eLockType aLockType)
        {
            return StoredProcedures.MID_ENQUEUE_READ_FROM_TYPE_ANY.Read(_dba,
                                                                    ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture)
                                                                    );
        }

		public void Enqueue_Insert(eLockType aLockType,
			int aRID,
			int aUserRID,
			int aClientThreadID)
		{
            
            StoredProcedures.MID_ENQUEUE_INSERT.Insert(_dba,
                                                       ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                       RID: aRID,
                                                       USER_RID: aUserRID,
                                                       OWNING_THREADID: aClientThreadID
                                                       );
		}

        // begin TT#1185 Verify ENQ before Update
        public List<int> GetHeadersToENQ(List<int> aHdrRID)
        {
            //int returnCode = -200;
            //StringBuilder hdrRidList = new StringBuilder();
            DataTable dtHeaderList = new DataTable();
            DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "HDR_RID";
            dtHeaderList.Columns.Add(dataColumn);

            string[] connector = new string[2];
            connector[0] = string.Empty;
            connector[1] = ",";
            //int connectWith = 0;
            foreach (int hdrRID in aHdrRID)
            {
                DataRow dr = dtHeaderList.NewRow();
                dr["HDR_RID"] = hdrRID;
                dtHeaderList.Rows.Add(dr);
                //hdrRidList.Append(connector[connectWith] + hdrRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                //connectWith = 1;
            }
            //MIDDbParameter[] InParams = new MIDDbParameter[1];
            //MIDDbParameter[] OutParams = new MIDDbParameter[1];
            //InParams[0] = new MIDDbParameter("@HdrRID_List", hdrRidList, eDbType.Text, eParameterDirection.Input);
            //OutParams[0] = new MIDDbParameter("@ReturnCode", returnCode, eDbType.Int, eParameterDirection.Output);
            //_dba.OpenReadConnection();
            try
            {
                //_dba.ReadOnlyStoredProcedure("SP_MID_IDENTIFY_HDRS_FOR_ENQ", InParams, OutParams);
                DataTable dt = StoredProcedures.MID_IDENTIFY_HDRS_FOR_ENQ_READ.Read(_dba, HDR_RID_LIST: dtHeaderList);
                List<int> hdrToEnq = new List<int>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    hdrToEnq.Add(Convert.ToInt32(dt.Rows[i]["HDR_RID"], CultureInfo.CurrentUICulture));
                }	

                //List<int> hdrToEnq = new List<int>();
                //int hdrRID_Ordinal = (int)_dba.GetReadOrdinal("HDR_RID");
                //do
                //{
                //    while (_dba.Read())
                //    {
                //        hdrToEnq.Add((int)_dba.ReadResultRow(hdrRID_Ordinal));
                //    }
                //} while (_dba.NextResult());
                return hdrToEnq;
            }
            finally
            {
                //_dba.CloseReadConnection();
            }
        }
        public bool Header_Enqueue(eLockType aLockType, int aUserRID, int aClientThreadID, long aTransactionID, List<int> aHdrRID, out List<HeaderConflict> aHeaderConflictList)
        {
            try
            {
                OpenUpdateConnection(eLockType.Header);
                //int returnCode = -200;
                aHeaderConflictList = new List<HeaderConflict>();
                //StringBuilder hdrRidList = new StringBuilder();
                //string[] connector = new string[2];
                //connector[0] = string.Empty;
                //connector[1] = ",";
                //int connectWith = 0;
                //foreach (int hdrRID in aHdrRID)
                //{
                //    hdrRidList.Append(connector[connectWith] + hdrRID.ToString(CultureInfo.CurrentUICulture).TrimStart(_trimChar));
                //    connectWith = 1;
                //}
                //MIDDbParameter[] InParams = new MIDDbParameter[5];
                //InParams[0] = new MIDDbParameter("@EnqType", (int)aLockType, eDbType.Int, eParameterDirection.Input);
                //InParams[1] = new MIDDbParameter("@UserRID", aUserRID, eDbType.Int, eParameterDirection.Input);
                //InParams[2] = new MIDDbParameter("@ThreadID", aClientThreadID, eDbType.Int, eParameterDirection.Input);
                //InParams[3] = new MIDDbParameter("@TranID", aTransactionID, eDbType.Int64, eParameterDirection.Input);
                //InParams[4] = new MIDDbParameter("@HdrRID_List", hdrRidList, eDbType.Text, eParameterDirection.Input);
                //MIDDbParameter[] OutParams = new MIDDbParameter[1];
                //OutParams[0] = new MIDDbParameter("@ReturnCode", returnCode, eDbType.Int, eParameterDirection.Output);

                //int returnValue = _dba.UpdateStoredProcedure("SP_MID_ENQUEUE_HEADERS", InParams, OutParams);

                DataTable dtHeaderList = new DataTable();
                dtHeaderList.Columns.Add("HDR_RID", typeof(int));
                foreach (int hdrRID in aHdrRID)
                {
                    //ensure userRIDs are distinct, and only added to the datatable one time
                    if (dtHeaderList.Select("HDR_RID=" + hdrRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtHeaderList.NewRow();
                        dr["HDR_RID"] = hdrRID;
                        dtHeaderList.Rows.Add(dr);
                    }
                }
                int returnValue = -1;
                DataTable dtConflicts = StoredProcedures.MID_ENQUEUE_INSERT_FOR_HEADERS.InsertAndRead(_dba,
                                                                                                    ref returnValue,
                                                                                                    EnqType: (int)aLockType,
                                                                                                    UserRID: aUserRID,
                                                                                                    ThreadID: aClientThreadID,
                                                                                                    TranID: aTransactionID,
                                                                                                    HDR_RID_LIST: dtHeaderList
                                                                                                    );
                //int returnValue = (int)StoredProcedures.MID_ENQUEUE_INSERT_FOR_HEADERS.ReturnCode.Value;
                if (returnValue > 0)
                {
                    CommitData();
                    return true;
                }
                //_dba.OpenReader(true);
                //int hdrRID_Ordinal = (int)_dba.GetReadOrdinal("HDR_RID");
                //int hdrID_Ordinal = (int)_dba.GetReadOrdinal("HDR_ID");
                //int userRID_Ordinal = (int)_dba.GetReadOrdinal("USER_RID");
                //int threadID_Ordinal = (int)_dba.GetReadOrdinal("OWNING_THREADID");
                //int tranID_Ordinal = (int)_dba.GetReadOrdinal("OWNING_TRANID");
                //int timeStamp_Ordinal = (int)_dba.GetReadOrdinal("ENQUEUE_TIMESTAMP");
                //do
                //{
                //    while (_dba.Read())
                //    {
                //        aHeaderConflictList.Add(
                //            new HeaderConflict(
                //                (string)_dba.ReadResultRow(hdrID_Ordinal),
                //                (int)_dba.ReadResultRow(hdrRID_Ordinal),
                //                (int)_dba.ReadResultRow(userRID_Ordinal),
                //                (int)_dba.ReadResultRow(threadID_Ordinal),
                //                (long)_dba.ReadResultRow(tranID_Ordinal),
                //                (DateTime)_dba.ReadResultRow(timeStamp_Ordinal)));
                //    }
                //} while (_dba.NextResult());
                //_dba.CloseReader();
                foreach (DataRow dr in dtConflicts.Rows)
                {
                    string hdrID_Ordinal = (string)dr["HDR_ID"];
                    int hdrRID_Ordinal = (int)dr["HDR_RID"];
                   
                    int userRID_Ordinal = (int)dr["USER_RID"];
                    int threadID_Ordinal = (int)dr["OWNING_THREADID"];
                    long tranID_Ordinal = (long)dr["OWNING_TRANID"];
                    DateTime timeStamp_Ordinal = (DateTime)dr["ENQUEUE_TIMESTAMP"];
                    aHeaderConflictList.Add(new HeaderConflict(hdrID_Ordinal, hdrRID_Ordinal, userRID_Ordinal, threadID_Ordinal, tranID_Ordinal, timeStamp_Ordinal));
                          
                }
   
                return false;
            }
            finally
            {
                CloseUpdateConnection();
            }
        }

  
       

       

		public void Enqueue_Delete(eLockType aLockType,
			int aRID,
			int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE.Delete(_dba,
                                                       ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                       RID: aRID,
                                                       USER_RID: aUserRID
                                                       );
		}

        // begin TT#1185 - Verify ENQ before Update
        /// <summary>
        /// Deletes ENQ on Lock Type for the given RID, User and Thread.
        /// </summary>
        /// <param name="aLockType">Lock Type</param>
        /// <param name="aRID">RID of the resouce that is locked</param>
        /// <param name="aUserRID">User RID of the owner of the lock</param>
        /// <param name="aThreadID">Thread ID of the owner of the lock</param>
        public void Enqueue_Delete(
            eLockType aLockType,
            int aRID,
            int aUserRID,
            int aThreadID,
            long aTransactionID)  
        {
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_HEADER.Delete(_dba,
                                                                  ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                                  RID: aRID,
                                                                  USER_RID: aUserRID,
                                                                  OWNING_THREADID: aThreadID,
                                                                  OWNING_TRANID: aTransactionID
                                                                  );
        }
        // end TT#1185 - Verify ENQ before Update

		public void Enqueue_Delete(eLockType aLockType, int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER.Delete(_dba,
                                                                         ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                                         USER_RID: aUserRID
				                                                         );
		}

		/// <summary>
		/// Delete all enqueues for user across all sessions
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		public void Enqueue_DeleteAll(int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_USER.Delete(_dba, USER_RID: aUserRID);
		}

		/// <summary>
		/// Delete all enqueues for a user in a given session
		/// </summary>
		/// <param name="aUserRID">The record ID of the user</param>
		/// <param name="aClientThreadID">The thread ID of the client session</param>
		public void Enqueue_DeleteAll(int aUserRID, int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD.Delete(_dba,
                                                                           USER_RID: aUserRID,
                                                                           OWNING_THREADID: aClientThreadID
                                                                           );
		}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		/// <summary>
		/// Delete all enqueues for a Process Id (Unique from SCHEDULE_PROCESS table)
		/// </summary>
		/// <param name="aUserRID">The Process Id of the process</param>
		public void Enqueue_DeleteAll_ByProcess(int aProcessId)
		{
            
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_PROCESS_ID.Delete(_dba, OWNING_THREADID: aProcessId);
		}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

		public DataTable StoreWeekEnqueue_Read(int HierarchyNodeRID, int VersionRID, int StartWeek, int EndWeek)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            return StoredProcedures.MID_ENQUEUE_READ_FOR_PLAN.Read(_dba,
                                                                       ENQUEUE_TYPE: Convert.ToInt32(eLockType.StoreWeek, CultureInfo.CurrentUICulture),
                                                                       HN_RID: HierarchyNodeRID,
                                                                       FV_RID: VersionRID,
                                                                       START_WEEK: StartWeek,
                                                                       END_WEEK: EndWeek
                                                                       );
		}

		public void StoreWeekEnqueue_Insert(
			int HierarchyNodeRID,
			int VersionRID,
			int StartWeek,
			int EndWeek,
			int UserRID,
			int aClientThreadID)
		{
         
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_PLAN.Insert(_dba,
                                                                ENQUEUE_TYPE: Convert.ToInt32(eLockType.StoreWeek, CultureInfo.CurrentUICulture),
                                                                HN_RID: HierarchyNodeRID,
                                                                FV_RID: VersionRID,
                                                                START_WEEK: StartWeek,
                                                                END_WEEK: EndWeek,
                                                                USER_RID: UserRID,
                                                                OWNING_THREADID: aClientThreadID
                                                                );
		}

		public void StoreWeekEnqueue_Delete(
			int HierarchyNodeRID,
			int VersionRID,
			int StartWeek,
			int EndWeek,
			int UserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_PLAN.Delete(_dba,
                                                                    ENQUEUE_TYPE: Convert.ToInt32(eLockType.StoreWeek, CultureInfo.CurrentUICulture),
                                                                    HN_RID: HierarchyNodeRID,
                                                                    FV_RID: VersionRID,
                                                                    START_WEEK: StartWeek,
                                                                    END_WEEK: EndWeek
                                                                    );
		}

		public void StoreWeekEnqueue_Delete(int UserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER.Delete(_dba,
                                                                        ENQUEUE_TYPE: Convert.ToInt32(eLockType.StoreWeek, CultureInfo.CurrentUICulture),
                                                                        USER_RID: UserRID
                                                                        );
		}

		public DataTable ChainWeekEnqueue_Read(int HierarchyNodeRID, int VersionRID, int StartWeek, int EndWeek)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues

            return StoredProcedures.MID_ENQUEUE_READ_FOR_PLAN.Read(_dba,
                                                                   ENQUEUE_TYPE: Convert.ToInt32(eLockType.ChainWeek, CultureInfo.CurrentUICulture),
                                                                   HN_RID: HierarchyNodeRID,
                                                                   FV_RID: VersionRID,
                                                                   START_WEEK: StartWeek,
                                                                   END_WEEK: EndWeek
                                                                   );
		}

		public void ChainWeekEnqueue_Insert(
			int HierarchyNodeRID,
			int VersionRID,
			int StartWeek,
			int EndWeek,
			int UserRID,
			int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_PLAN.Insert(_dba,
                                                                    ENQUEUE_TYPE: Convert.ToInt32(eLockType.ChainWeek, CultureInfo.CurrentUICulture),
                                                                    HN_RID: HierarchyNodeRID,
                                                                    FV_RID: VersionRID,
                                                                    START_WEEK: StartWeek,
                                                                    END_WEEK: EndWeek,
                                                                    USER_RID: UserRID,
                                                                    OWNING_THREADID: aClientThreadID
                                                                    );
		}

		public void ChainWeekEnqueue_Delete(
			int HierarchyNodeRID,
			int VersionRID,
			int StartWeek,
			int EndWeek,
			int UserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_PLAN.Delete(_dba,
                                                                ENQUEUE_TYPE: Convert.ToInt32(eLockType.ChainWeek, CultureInfo.CurrentUICulture),
                                                                HN_RID: HierarchyNodeRID,
                                                                FV_RID: VersionRID,
                                                                START_WEEK: StartWeek,
                                                                END_WEEK: EndWeek
                                                                );
		}

		public void ChainWeekEnqueue_Delete(int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER.Delete(_dba,
                                                                        ENQUEUE_TYPE: Convert.ToInt32(eLockType.ChainWeek, CultureInfo.CurrentUICulture),
                                                                        USER_RID: aUserRID
                                                                        );
		}

		public DataTable Hierarchy_Read(int aHierarchyRID)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //MID Track #4477 - JSmith - No user name on lock message
            return StoredProcedures.MID_ENQUEUE_READ_FOR_HIERARCHY.Read(_dba, PH_RID: aHierarchyRID);
		}

		public void Hierarchy_Insert(
			int aHierarchyRID,
			int aUserRID,
			int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_HIERARCHY.Insert(_dba,
                                                                         PH_RID: aHierarchyRID,
                                                                         USER_RID: aUserRID,
                                                                         OWNING_THREADID: aClientThreadID
                                                                         );
		}

		public void Hierarchy_Delete(
			int aHierarchyRID,
			int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_HIERARCHY.Delete(_dba, PH_RID: aHierarchyRID);
		}

      
		public DataTable HierarchyNode_Read(int aNodeRID)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            //MID Track #4477 - JSmith - No user name on lock message
            return StoredProcedures.MID_ENQUEUE_READ_FOR_HIERARCHY_NODES.Read(_dba, HN_RID: aNodeRID); 
		}

        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
        public DataTable AnyAncestorsLocked(int aHN_RID)
        {
            return StoredProcedures.SP_MID_ANY_ANCESTORS_LOCKED.Read(_dba, HN_RID: aHN_RID);
        }

        public DataTable AnyDescendantsLocked(int aHN_RID)
        {
            return StoredProcedures.SP_MID_ANY_DESCENDANTS_LOCKED.Read(_dba, HN_RID: aHN_RID);
        }
        // End TT#2015

		public void HierarchyNode_Insert(
			int aNodeRID,
			int aUserRID,
			int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES.Insert(_dba,
                                                                           HN_RID: aNodeRID,
                                                                           USER_RID: aUserRID,
                                                                           OWNING_THREADID: aClientThreadID
                                                                           );
		}

		public void HierarchyNode_Delete(
			int aNodeRID,
			int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES.Delete(_dba,
                                                                           HN_RID: aNodeRID,
                                                                           USER_RID: aUserRID
                                                                           );
		}

  
        

        


		public void HierarchyBranch_Insert(
			int aHierarchyRID, 
			int aNodeRID,
			int aUserRID,
			int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH.Insert(_dba,
                                                                            PH_RID: aHierarchyRID,
                                                                            HN_RID: aNodeRID,
                                                                            USER_RID: aUserRID,
                                                                            OWNING_THREADID: aClientThreadID
                                                                            );
		}

		public void HierarchyBranch_Delete(
			int aHierarchyRID,
			int aNodeRID,
			int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH.Delete(_dba,
                                                                            PH_RID: aHierarchyRID,
                                                                            HN_RID: aNodeRID
                                                                            );
		}

		public DataTable Model_Read(eLockType aLockType,int aModelRID)
		{ 
			//MID Track # 2354 - removed nolock because it causes concurrency issues
			//MID Track #4477 - JSmith - No user name on lock message
            return StoredProcedures.MID_ENQUEUE_READ_FOR_MODEL.Read(_dba,
                                                                    ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                                    RID: aModelRID
                                                                    );
		}

		public void Model_Insert(
			eLockType aLockType,
			int aModelRID,
			int aUserRID,
			int aClientThreadID)
		{
            StoredProcedures.MID_ENQUEUE_INSERT_FOR_MODEL.Insert(_dba,
                                                                 ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                                 RID: aModelRID,
                                                                 USER_RID: aUserRID,
                                                                 OWNING_THREADID: aClientThreadID
                                                                 );
		}

		public void Model_Delete(
			eLockType aLockType,
			int aModelRID,
			int aUserRID)
		{
            StoredProcedures.MID_ENQUEUE_DELETE_FOR_MODEL.Delete(_dba,
                                                                 ENQUEUE_TYPE: Convert.ToInt32(aLockType, CultureInfo.CurrentUICulture),
                                                                 RID: aModelRID
                                                                 );
		}

		// BEGIN MID Track #4572 - John Smith - clear enqueues on global start
		public void DeleteAllEnqueues()
		{
            StoredProcedures.MID_ENQUEUE_DELETE_ALL.Delete(_dba);
		}
		// END MID Track #4572

    //BEGIN TT#187 - Truncate Virtual Locks RBeck
        /// <summary>
        /// Deletes all virtual locks
        /// </summary>
        /// <returns></returns>
        public void DeleteAllVirtualLocks()
        {
            StoredProcedures.MID_VIRTUAL_LOCK_DELETE_ALL.Delete(_dba);
        }
    //END  TT#187 - Truncate Virtual Locks RBeck

	}
}
