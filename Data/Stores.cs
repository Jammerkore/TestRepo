using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for Stores.
	/// </summary>
	public partial class StoreData : DataLayer
	{	
		

		public StoreData() : base()
		{

		}

        // Begin TT#188 - JSmith - Rebrand
        public StoreData(string aConnectionString)
			: base(aConnectionString)
		{

		}
        // End TT#188



       
			  

		public DataTable StoreProfile_Read()
		{
			try
			{
                return StoredProcedures.MID_STORES_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads stores marked for delete where the STORE_DELETE_IND = '1'.
		/// </summary>
		/// <returns></returns>
		public DataTable StoreProfile_ReadForStoreDelete()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STORES_READ_FOR_DELETION.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		

	

		// BEGIN TT#739-MD - STodd - delete stores
		public void StoreProfile_MarkForDelete(int storeRid, bool markForDelete)
		{
			try
			{
				char forDelete = Include.ConvertBoolToChar(markForDelete);
                StoredProcedures.SP_MID_STORE_MARK_FOR_DELETE.Update(_dba,
                                                                         ST_RID: storeRid,
                                                                         MARK_FOR_DELETE: forDelete
                                                                         );
				return;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN TT#739-MD - STodd - delete stores
		public void StoreProfile_DoStoreRemovalAnalysis(double pctThreshold, int minRowCount, int maxRowCount)
		{
			try
			{
                StoredProcedures.MID_STORE_REMOVAL_DO_ANALYSIS.Execute(_dba,
                                                                      MINIMUM_DELETE_ROW_THRESHOLD: minRowCount,
                                                                      MAXIMUM_DELETE_ROW_THRESHOLD: maxRowCount,
                                                                      DELETION_PROCESS_PERCENTAGE_THRESHOLD: pctThreshold
                                                                      );

				return;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable StoreProfile_ReadRemovalAnalysis()
		{
			try
			{
                return StoredProcedures.MID_STORE_REMOVAL_ANALYSIS_READ_ALL.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateStoreDeleteInProgress(eStoreDeleteTableStatus status, string tableName)
		{
			try
			{
                int completed = -1;
                int inProgress = -1;

				if (status == eStoreDeleteTableStatus.NotStarted)
				{
                    completed = 0;
                    inProgress = 0;
				}
				if (status == eStoreDeleteTableStatus.InProgress)
				{
                    completed = 0;
                    inProgress = 1;
				}
				if (status == eStoreDeleteTableStatus.Completed)
				{
                    completed = 1;
                    inProgress = 0;
				}

                if (completed != -1)
                {
                    StoredProcedures.MID_STORE_REMOVAL_ANALYSIS_UPDATE.Update(_dba,
                                                                              TABLE_NAME: tableName,
                                                                              COMPLETED: completed,
                                                                              IN_PROGRESS: inProgress
                                                                              );
                }
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#739-MD - STodd - delete stores

	

        //TODO: Skipped during the Convert SQL
		// BEGIN TT#739-MD - STodd - delete stores
		public void StoreProfile_ExecCommand(string sqlCommand)
		{
			try
			{
				_dba.ExecuteSQLQuery(sqlCommand, "StoreProfile_ExecCommand");
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
        //TODO: Skipped during the Convert SQL
		public int StoreProfile_ExecProc(string procName)
		{
			try
			{
				//MIDDbParameter[] InParams = { new MIDDbParameter("@ST_RID", storeRid, eDbType.Int) };
				//InParams[0].Direction = eParameterDirection.Input;

				MIDDbParameter[] OutParams = { new MIDDbParameter("@RC", DBNull.Value, eDbType.Int) };
				OutParams[0].Direction = eParameterDirection.Output;

				return _dba.ExecuteStoredProcedure(procName, null, OutParams);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#739-MD - STodd - delete stores

		// BEGIN TT#739-MD - STodd - delete stores
		public int StoreProfile_Delete(string tableName, int batchSize)
		{
			try
			{
               
                if (tableName == "FILTER_CONDITION_LIST_VALUES") //else if (tableName == "IN_USE_FILTER_XREF") //TT#1342-MD -jsobek -Store Filters - IN_USE_FILTER_XREF
				{
                    //Begin TT#1342-MD -jsobek -Store Filters - IN_USE_FILTER_XREF
                    //StoredProcedures.SP_MID_STORE_DELETE_IN_USE_FILTER_XREF.Delete(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.SP_MID_STORE_DELETE_IN_USE_FILTER_XREF.RC.Value;
                    return StoredProcedures.MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES.DeleteWithReturnCode(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES.RC.Value;
                    //End TT#1342-MD -jsobek -Store Filters - IN_USE_FILTER_XREF
				}
				else if (tableName == "SIMILAR_STORES")
				{
                    return StoredProcedures.SP_MID_STORE_DELETE_SIMILAR_STORES.DeleteWithReturnCode(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.SP_MID_STORE_DELETE_SIMILAR_STORES.RC.Value;
				}
				else if (tableName == "NODE_SIZE_CURVE_SIMILAR_STORE")
				{
                    return StoredProcedures.SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE.DeleteWithReturnCode(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE.RC.Value;
				}
				else if (tableName == "SYSTEM_OPTIONS")
				{
                    return StoredProcedures.SP_MID_STORE_DELETE_SYSTEM_OPTIONS.DeleteWithReturnCode(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.SP_MID_STORE_DELETE_SYSTEM_OPTIONS.RC.Value;
				}
				else if (tableName == "STORES")
				{
                    return StoredProcedures.SP_MID_STORE_REMOVAL_STORES.DeleteWithReturnCode(_dba, BATCH_SIZE: batchSize);
                    //return (int)StoredProcedures.SP_MID_STORE_REMOVAL_STORES.RC.Value;
				}
				else
				{
					return 0;
				}
				//else if (isIndexed)
				//{
				//    return _dba.ExecuteStoredProcedure("SP_MID_STORE_DELETE_FROM_TABLE_INDEXED", InParams, OutParams);
				//}
				//else
				//{
				//    return _dba.ExecuteStoredProcedure("SP_MID_STORE_DELETE_FROM_TABLE", InParams, OutParams);
				//}
			}
			catch
			{
				throw;

			}

		}

  

		/// <summary>
		/// The procedure will read through the STORE_ELIGIBILITY table looking for SIMILAR_STORE_TYPE = 1. 
		/// For each, it will read the SIMILAR_STORE table looking for similar stores. 
		/// If none are found, the SIMILAR_STORE_TYPE & SIMIAR_STORE_RATIO will be set to 0. The UNTIL_DATE will be set to NULL.
		/// </summary>
		public void StoreProfile_CleanupSimilarStoreEligibility()
		{
			try
			{
				//_dba.ExecuteStoredProcedure("SP_MID_STORE_DELETE_SIM_STORE_CLEANUP");
                StoredProcedures.SP_MID_STORE_DELETE_SIM_STORE_CLEANUP.Delete(_dba);

				return;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

      



		// BEGIN TT#739-MD - STodd - delete stores
		public void TruncateTable(string tableName)
		{
			StringBuilder sbCommand = new StringBuilder();
			sbCommand.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)" + System.Environment.NewLine);
			sbCommand.Append("BEGIN" + System.Environment.NewLine);
			sbCommand.Append("TRUNCATE TABLE dbo." + tableName + System.Environment.NewLine);
			sbCommand.Append("END" + System.Environment.NewLine);
			string SQLCommand = sbCommand.ToString();
			_dba.ExecuteNonQuery(SQLCommand);
		}

		public void DropTable(string tableName)
		{
			StringBuilder sbCommand = new StringBuilder();
			sbCommand.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)" + System.Environment.NewLine);
			sbCommand.Append("BEGIN" + System.Environment.NewLine);
			sbCommand.Append("DROP TABLE dbo." + tableName + System.Environment.NewLine);
			sbCommand.Append("END" + System.Environment.NewLine);
			string SQLCommand = sbCommand.ToString();
			_dba.ExecuteNonQuery(SQLCommand);
		}

		public void DisableTrigger(string triggerName, string tableName)
		{
			string SQLCommand = "disable trigger dbo." + triggerName + " on dbo." + tableName;
			_dba.ExecuteNonQuery(SQLCommand);

		}

		public void EnableTrigger(string triggerName, string tableName)
		{
			string SQLCommand = "enable trigger dbo." + triggerName + " on dbo." + tableName;
			_dba.ExecuteNonQuery(SQLCommand);

		}
		// END TT#739-MD - STodd - delete stores



		public DataTable StoreCharGroup_Read()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STORE_CHAR_GROUP_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		
		
		





        //BEGIN TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit
        public int GetStoreAllocationCount(int storeRID)
        {
            int SACount = 0;
            //_dba.OpenReadConnection();  // Begin TT#1268-MD - RMatelic - 5.4 Merge
            try
            {
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //MIDDbParameter[] inParams = { new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input) };

                //MIDDbParameter[] outParams = { new MIDDbParameter("@COUNT", 0, eDbType.Int) };
                //outParams[0].Direction = eParameterDirection.Output;

                //_dba.ReadOnlyStoredProcedure("MID_GET_STORE_ALLOCATION_COUNT", inParams, outParams);
                //if (outParams[0].Value != DBNull.Value)
                //{
                //    SACount = Convert.ToInt32(outParams[0].Value);
                //}
                //else
                //{
                //    SACount = 0;
                //}
                DataTable dt = StoredProcedures.MID_GET_STORE_ALLOCATION_COUNT.Read(_dba, ref SACount, ST_RID: storeRID);

                //SACount = (int)StoredProcedures.MID_GET_STORE_ALLOCATION_COUNT.COUNT.Value;

                //End TT#1268-MD -jsobek -5.4 Merge
                return SACount;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            // Begin TT#1268-MD - RMatelic - 5.4 Merge
            //finally
            //{
            //    _dba.CloseReadConnection();
            //}
            // End TT#1268-MD 
        }

        public int GetStoreIntransitCount(int storeRID)
        {
            int SICount = 0;
            //_dba.OpenReadConnection();  // Begin TT#1268-MD - RMatelic - 5.4 Merge
            try
            {
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //MIDDbParameter[] inParams = { new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input) };

                //MIDDbParameter[] outParams = { new MIDDbParameter("@COUNT", 0, eDbType.Int) };
                //outParams[0].Direction = eParameterDirection.Output;

                //_dba.ReadOnlyStoredProcedure("MID_GET_STORE_INTRANSIT_COUNT", inParams, outParams);
                //if (outParams[0].Value != DBNull.Value)
                //{
                //    SICount = Convert.ToInt32(outParams[0].Value);
                //}
                //else
                //{
                //    SICount = 0;
                //}
                DataTable dt = StoredProcedures.MID_GET_STORE_INTRANSIT_COUNT.Read(_dba, ref SICount, ST_RID: storeRID);

                //SICount = (int)StoredProcedures.MID_GET_STORE_INTRANSIT_COUNT.COUNT.Value;

                //End TT#1268-MD -jsobek -5.4 Merge

                return SICount;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            // Begin TT#1268-MD - RMatelic - 5.4 Merge
            //finally
            //{
            //    _dba.CloseReadConnection();
            //}
            // End TT#1268-MD 
        }
        //END TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit

        // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
        public bool AllowRemoveVSWID(int storeRID)
        {
            bool allowRemove = false;
            try
            {

                int returnCode = StoredProcedures.MID_STORE_ALLOW_REMOVE_VSWID.ReadValue(_dba, ST_RID: storeRID);

                allowRemove = Include.ConvertIntToBool(returnCode);

                return allowRemove;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store



	}
}
