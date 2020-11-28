using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;
 
namespace MIDRetail.Data
{
	/// <summary>
	/// Base Workflow class containing all properties for a Workflow.
	/// </summary>
	public partial class WorkflowBaseData: DataLayer
	{
		// workflow values
		private int						_workflow_RID;
		private string					_workflow_Name;
		private eWorkflowType			_workflow_Type_ID;
		private int						_user_RID;
		private string					_workflow_Description;
		private int						_store_Filter_RID;
		private bool					_manualOverride;
		private string					_workflow_Comment = null;

		
		public int Workflow_RID
		{
			get	{return _workflow_RID;}
			set	{_workflow_RID = value;	}
		}
		public string Workflow_Name
		{
			get	{return _workflow_Name;}
			set	{_workflow_Name = value;	}
		}
		public int User_RID
		{
			get	{return _user_RID;}
			set	{_user_RID = value;	}
		}

		public eWorkflowType Workflow_Type_ID
		{
			get	{return _workflow_Type_ID;}
			set	{_workflow_Type_ID = value;	}
		}
		public int int_workflow_Type_ID
		{
			get	{return (int)_workflow_Type_ID;}
		}
		public string Workflow_Description
		{
			get	{return _workflow_Description;}
			set	{_workflow_Description = value;	}
		}

		public int Store_Filter_RID
		{
			get	{return _store_Filter_RID;}
			set	{_store_Filter_RID = value;	}
		}

		public string Workflow_Comment
		{
			get	{return _workflow_Comment;}
			set	{_workflow_Comment = value;	}
		}

		/// <summary>
		/// Gets or sets manual override
		/// </summary>
		/// <remarks>
		/// True indicates the user had the opportunity to issue overrides prior to the execution of the workflow.
		/// False indicates the user did not make any manual adjustments to the override Workflow prior to execution.
		/// </remarks>
		public bool ManualOverride
		{
			get
			{
				return	_manualOverride;
			}
			set
			{
				_manualOverride = value;
			}
		}
		
		public WorkflowBaseData(): base()
		{

		}

		/// <summary>
		/// Creates an instance of the WorkflowBase class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		public WorkflowBaseData(TransactionData td): base(td.DBA)
		{

		}

		/// <summary>
		/// Populates the WorkflowBase class based on PK input param.
		/// </summary>
		/// <param name="aWorkflow_RID">Record ID of the workflow</param>
		/// <returns>Returns boolean value: true = populated</returns>
		public bool PopulateWorkflow(int aWorkflow_RID)
		{
			try
			{				
                DataTable dtWorkflow = MIDEnvironment.CreateDataTable();
                dtWorkflow = StoredProcedures.MID_WORKFLOW_READ.Read(_dba, WORKFLOW_RID: aWorkflow_RID);

				if(dtWorkflow != null && dtWorkflow.Rows.Count != 0)
				{
					DataRow dr = dtWorkflow.Rows[0];
					_workflow_RID = aWorkflow_RID;
					_workflow_Name = Convert.ToString(dr["WORKFLOW_NAME"], CultureInfo.CurrentUICulture);
					_workflow_Type_ID = (eWorkflowType)Convert.ToInt32(dr["WORKFLOW_TYPE_ID"], CultureInfo.CurrentUICulture);
					_user_RID = Convert.ToInt32(dr["WORKFLOW_USER_RID"], CultureInfo.CurrentUICulture);
					_workflow_Description = Convert.ToString(dr["WORKFLOW_DESCRIPTION"], CultureInfo.CurrentUICulture);
					_store_Filter_RID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
					_manualOverride = Include.ConvertCharToBool(Convert.ToChar(dr["WORKFLOW_OVERRIDE"], CultureInfo.CurrentUICulture));
					return true;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		/// <summary>
		/// Returns DataTable containing a single row with the data for the workflow
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the workflow</param>
		/// <returns>DataTable</returns>
		public string GetWorkflowName(int aWorkflowRID)
		{
			try
			{	
                DataTable dt = StoredProcedures.MID_WORKFLOW_READ_NAME.Read(_dba, WORKFLOW_RID: aWorkflowRID);
				if (dt.Rows.Count == 1)
				{
					return Convert.ToString(dt.Rows[0]["WORKFLOW_NAME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return string.Empty;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1218 - JSmith - API - Header Load Performance
        /// <summary>
        /// Returns RID for the workflow
        /// </summary>
        /// <param name="aWorkflowName">The name of the workflow</param>
        /// <returns>int</returns>
        public int GetWorkflowRID(string aWorkflowName)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_WORKFLOW_READ_RID_FROM_NAME.Read(_dba, WORKFLOW_NAME: aWorkflowName);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["WORKFLOW_RID"]);
                }
                else
                {
                    return Include.NoRID;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1218

		/// <summary>
		/// Returns DataTable containing a single row with the data for the workflow
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the workflow</param>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflow(int aWorkflowRID)
		{
			try
			{	
                return StoredProcedures.MID_WORKFLOW_READ_FROM_RID.Read(_dba, WORKFLOW_RID: aWorkflowRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		/// <summary>
		/// Returns DataTable containing a single row with the data for the workflow
		/// </summary>
		/// <param name="aWorkflowName">The name of the workflow</param>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflow(string aWorkflowName)
		{
			try
			{	
                return StoredProcedures.MID_WORKFLOW_READ_FROM_NAME.Read(_dba, WORKFLOW_NAME: aWorkflowName);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
        //Begin TT#1764 - DOConnell - Duplicate Workflow Naming Convention
        /// <summary>
        /// Returns DataTable containing a single row with the data for the workflow and User ID
        /// </summary>
        /// <param name="aWorkflowName">The name of the workflow</param>
        /// <param name="UserID">The owners UserId </param>
        /// <returns>DataTable</returns>
        public DataTable GetWorkflow(string aWorkflowName, int UserID)
        {
            try
            {
                return StoredProcedures.MID_WORKFLOW_READ_FROM_NAME_AND_USER.Read(_dba,
                                                                                  WORKFLOW_NAME: aWorkflowName,
                                                                                  WORKFLOW_USER_RID: UserID
                                                                                  );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1764 - DOConnell - Duplicate Workflow Naming Convention

		/// <summary>
		/// Returns DataTable of all Workflows by Workflow ID
		/// </summary>
		/// <param name="WorkflowTypeID">eWorkflowType enum</param>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflows(eWorkflowType WorkflowTypeID)
		{
			try
			{	
                return StoredProcedures.MID_WORKFLOW_READ_FROM_TYPE.Read(_dba, WORKFLOW_TYPE_ID: (int)WorkflowTypeID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Workflows by Workflow ID and User ID
		/// </summary>
		/// <param name="WorkflowTypeID">eWorkflowType enu</param>
		/// <param name="UserID">USER_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflows(eWorkflowType WorkflowTypeID, int UserID, bool isVirtual)
		{
			try
			{	
                return StoredProcedures.MID_WORKFLOW_READ_FROM_TYPE_AND_USER.Read(_dba,
                                                                                  WORKFLOW_TYPE_ID: (int)WorkflowTypeID,
                                                                                  USER_RID: UserID
                                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#63 - JSmith - Shared folder showing when nothing shared
        /// <summary>
        /// Returns DataTable of all Workflows by Workflow ID and User ID
        /// </summary>
        /// <param name="aUserID">USER_RID</param>
        /// <returns>DataTable</returns>
        public DataTable GetSharedWorkflows(int UserID)
        {
            try
            {
                return StoredProcedures.MID_WORKFLOW_READ_SHARED.Read(_dba, USER_RID: UserID );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns DataTable of all Workflows by Workflow ID and User ID
        /// </summary>
        /// <param name="aUserID">USER_RID</param>
        /// <param name="aOwnerUserID">Owner USER_RID</param>
        /// <returns>DataTable</returns>
        public DataTable GetSharedWorkflows(int aUserID, int aOwnerUserID)
        {
            try
            {
                return StoredProcedures.MID_WORKFLOW_READ_SHARED_FROM_OWNER.Read(_dba,
                                                                                 USER_RID: aUserID,
                                                                                 OWNER_USER_RID: aOwnerUserID
                                                                                 );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#63

		/// <summary>
		/// Returns DataTable of all Workflows per USER_RID or GLOBAL_USER_IND
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflows(int UserID)
		{
			try
			{
                return StoredProcedures.MID_WORKFLOW_READ_FROM_USER.Read(_dba, USER_RID: UserID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		

		/// <summary>
		/// Check if a row exists in param tableName where param whereClause.
		/// </summary>
		/// <param name="tableName">Name of table to query</param>
		/// <param name="whereClause">WHERE clause</param>
		/// <returns>bool if row(s) exists</returns>
        public bool GenericRowExists(string WORKFLOW_NAME, int WORKFLOW_TYPE_ID, int WORKFLOW_USER_RID, int WORKFLOW_RID)
		{
            // MID Track # 2354 - removed nolock because it causes concurrency issues
            int recordCount = StoredProcedures.MID_WORKFLOW_READ_COUNT.ReadRecordCount(_dba,
                                                                         WORKFLOW_NAME: WORKFLOW_NAME,
                                                                         WORKFLOW_TYPE_ID: WORKFLOW_TYPE_ID,
                                                                         WORKFLOW_USER_RID: WORKFLOW_USER_RID,
                                                                         WORKFLOW_RID: WORKFLOW_RID
                                                                         );
				
			if (recordCount == 0)
				return false;

			return true;
		}

        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //public int CreateWorkflow()
        public int CreateWorkflow(int aUpdateUserRID)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			int aWorkflow_RID = Include.NoRID;

			try
			{
                aWorkflow_RID = StoredProcedures.SP_MID_WORKFLOW_INSERT.InsertAndReturnRID(_dba,
                                                                                            WORKFLOW_NAME: Workflow_Name,
                                                                                            WORKFLOW_TYPE_ID: int_workflow_Type_ID,
                                                                                            WORKFLOW_USER_RID: User_RID,
                                                                                            WORKFLOW_DESCRIPTION: Workflow_Description,
                                                                                            STORE_FILTER_RID: Store_Filter_RID,
                                                                                            WORKFLOW_OVERRIDE: Include.ConvertBoolToChar(ManualOverride)
                                                                                            );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //AddChangeHistory(_dba, aWorkflow_RID, "Created");
                AddChangeHistory(_dba, aWorkflow_RID, "Created", aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(User_RID, (int)eProfileType.Workflow, aWorkflow_RID, User_RID);
                }

                ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.AddUserItem(User_RID, (int)eSharedDataType.Workflow, aWorkflow_RID, User_RID);
                //    sa.AddUserItem(User_RID, (int)eProfileType.Workflow, aWorkflow_RID, User_RID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                ////End Track #4815
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login


				return aWorkflow_RID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //public bool UpdateWorkflow()
        public bool UpdateWorkflow(int aUpdateUserRID)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			bool UpdateSuccessful = true;
			try
			{
                StoredProcedures.MID_WORKFLOW_UPDATE.Update(_dba,
                                                            WORKFLOW_RID: _workflow_RID,
                                                            WORKFLOW_NAME: _workflow_Name,
                                                            WORKFLOW_TYPE_ID: (int)_workflow_Type_ID,
                                                            WORKFLOW_USER_RID: _user_RID,
                                                            WORKFLOW_DESCRIPTION: _workflow_Description,
                                                            STORE_FILTER_RID: _store_Filter_RID
                                                            );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //AddChangeHistory(_dba, _workflow_RID, _workflow_Comment);
                AddChangeHistory(_dba, _workflow_RID, _workflow_Comment, aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine

				UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

		public bool DeleteWorkflow()
		{
			bool UpdateSuccessful = true;
			try
			{
				DeleteChangeHistory();

                //Ensure to delete child tables first
                StoredProcedures.MID_WORKFLOW_DELETE.Delete(_dba, WORKFLOW_RID: _workflow_RID);

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.DeleteUserItemByTypeAndRID((int)eProfileType.Workflow, _workflow_RID);
                }

                ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.Workflow, _workflow_RID);
                //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.Workflow, _workflow_RID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                ////End Track #4815
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

        public int Workflow_GetKey(int aUserRID, string aWorkflowName)
        {
            try
            {

                DataTable dt = StoredProcedures.MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME.Read(_dba,
                                                                                      USER_RID: aUserRID,
                                                                                      WORKFLOW_NAME: aWorkflowName
                                                                                      );

                if (dt.Rows.Count == 1)
                {
                    return (Convert.ToInt32(dt.Rows[0]["WORKFLOW_RID"], CultureInfo.CurrentUICulture));
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

        private void AddChangeHistory(DatabaseAccess aDBA, int aWorkflowRID, string aComment, int aUpdateUserRID, string windowsUser, string windowsMachine, string windowsRemoteMachine) //TT#1511-MD -jsobek  -Data Layer Request - Changes for Method and Workflow change history)
		{
			try
			{
                StoredProcedures.MID_WORKFLOW_CHANGE_HISTORY_INSERT.Insert(_dba,
                                                                           WORKFLOW_RID: aWorkflowRID,
                                                                           USER_RID: aUpdateUserRID,   // TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                                                                           CHANGE_DATE: DateTime.Now,
                                                                           WORKFLOW_COMMENT: aComment,
                                                                           WINDOWS_USER: windowsUser,
                                                                           WINDOWS_MACHINE: windowsMachine,
                                                                           WINDOWS_REMOTE_MACHINE: windowsRemoteMachine
                                                                           );
			}
			catch
			{
				throw;
			}
		}

		private void DeleteChangeHistory()
		{
			try
			{
                StoredProcedures.MID_WORKFLOW_CHANGE_HISTORY_DELETE.Delete(_dba, WORKFLOW_RID: _workflow_RID);
			}
			catch
			{
				throw;
			}
		}

		public DataRow GetLoadedWorkflowBaseNewRow(DataTable dt)
		{
			DataRow newDataRow = dt.NewRow();
			
			newDataRow["WORKFLOW_RID"]			= _workflow_RID;
			newDataRow["WORKFLOW_NAME"]			= _workflow_Name;
			newDataRow["WORKFLOW_TYPE_ID"]		= (int)_workflow_Type_ID;
			newDataRow["WORKFLOW_USER_RID"]		= _user_RID;
			newDataRow["WORKFLOW_DESCRIPTION"]	= _workflow_Description;
			newDataRow["STORE_FILTER_RID"]		= _store_Filter_RID;
			
			return newDataRow;
		}

		/// <summary>
		/// Used for Allocation Property tabs
		/// </summary>
		/// <param name="method_RID"></param>
		/// <returns></returns>
		public DataTable GetAllocMethodPropertiesUIWorkflows(int method_RID)
		{
			try
			{
                return StoredProcedures.MID_WORKFLOW_READ_FROM_METHOD.Read(_dba, METHOD_RID: method_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

       

		// Begin MID ISssue #3501 - stodd
		/// <summary>
		/// Used for OTS Planning Property tabs
		/// </summary>
		/// <param name="method_RID"></param>
		/// <returns></returns>
		public DataTable GetOTSMethodPropertiesUIWorkflows(int method_RID)
		{
			try
			{
                return StoredProcedures.MID_WORKFLOW_READ_FROM_OTS_METHOD.Read(_dba, METHOD_RID: method_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// End MID ISssue #3501 - stodd



//Begin  TT#283 - MD - User name not designated in Task List - RBeck
        /// <summary>
        /// Returns string containing the USER NAME for the workflow
        /// </summary>
        /// <param name="aUserRID">The user RID of the workflow</param>
        /// <returns>String</returns>
        public string GetWorkflowUserName(int aUserRID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_APPLICATION_USER_READ_NAME.Read(_dba, USER_RID: aUserRID);

                if (dt.Rows.Count == 1)
                {
                    return Convert.ToString(dt.Rows[0]["USER_NAME"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        /// <summary>
        /// Returns the integer user ID of the workflow creater 
        /// </summary>
        /// <param name="aWorkflowRID">The user ID of the workflow creater</param>
        /// <returns>Integer</returns>
        public int GetWorkflowUserId(int aWorkflowRID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_WORKFLOW_READ_USER.Read(_dba, WORKFLOW_RID: aWorkflowRID);
                if (dt.Rows.Count == 1)
                {
                    return Convert.ToInt32(dt.Rows[0]["WORKFLOW_USER_RID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
//End    TT#283 - MD - User name not designated in Task List - RBeck

	}			
} 
 