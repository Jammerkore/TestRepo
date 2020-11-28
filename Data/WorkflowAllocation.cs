using System;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for AllocationWorkflowData.
	/// </summary>
	public class AllocationWorkflowData: WorkflowBaseData
	{
		// workflow step values
		private int						_step_Number;
		private eMethodType				_action_Method_Type;
		private int						_method_RID;
		private int						_comp_Crit_RID;
		private double					_pct_Tolerance;
		private bool					_review_Ind;
		private int						_step_Store_Filter_RID;

		// workflow step component criteria values
		private eComponentType			_component_Type;
		private string					_expression;
		private int						_size_Code_RID;
		private int						_color_Code_RID;
		private string					_packName;

		public int Step_Number
		{
			get	{return _step_Number;}
			set	{_step_Number = value;	}
		}

		public int Method_RID
		{
			get	{return _method_RID;}
			set	{_method_RID = value;	}
		}

		public eMethodType Action_Method_Type
		{
			get	{return _action_Method_Type;}
			set	{_action_Method_Type = value;	}
		}

		public int Comp_Crit_RID
		{
			get	{return _comp_Crit_RID;}
			set	{_comp_Crit_RID = value;	}
		}

		public double Pct_Tolerance
		{
			get	{return _pct_Tolerance;}
			set	{_pct_Tolerance = value;	}
		}

		public bool Review_Ind
		{
			get	{return _review_Ind;}
			set	{_review_Ind = value;	}
		}

		public int Step_Store_Filter_RID
		{
			get	{return _step_Store_Filter_RID;}
			set	{_step_Store_Filter_RID = value;	}
		}

		public eComponentType Component_Type
		{
			get	{return _component_Type;}
			set	{_component_Type = value;	}
		}

		public string Expression
		{
			get	{return _expression;}
			set	{_expression = value;	}
		}

		public int Size_Code_RID
		{
			get	{return _size_Code_RID;}
			set	{_size_Code_RID = value;	}
		}

		public int Color_Code_RID
		{
			get	{return _color_Code_RID;}
			set	{_color_Code_RID = value;	}
		}

		public string PackName
		{
			get	{return _packName;}
			set	{_packName = value;	}
		}
		
		public AllocationWorkflowData(): base()
		{
				
		}
		/// <summary>
		/// Creates an instance of the MethodRule class.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		public AllocationWorkflowData(int aWorkflow_RID, eChangeType changeType)
		{
			Workflow_RID = aWorkflow_RID;
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateWorkflow(aWorkflow_RID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the WorkflowBase class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		public AllocationWorkflowData(TransactionData td)
		{
			_dba = td.DBA;
		}
		
		/// <summary>
		/// Returns DataTable of all Workflows steps
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflowSteps()
		{
			try
			{	
				return GetWorkflowSteps(Workflow_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Workflows steps
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflowSteps(int aWorkflow_RID)
		{
			try
			{	
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT ws.WORKFLOW_RID, ws.STEP_NUMBER, ws.ACTION_METHOD_TYPE, "); 
                //SQLCommand.Append("COALESCE(ws.METHOD_RID," + Include.UndefinedMethodRID.ToString(CultureInfo.CurrentUICulture) + ") METHOD_RID, ");
                //SQLCommand.Append("COALESCE(ws.PCT_TOLERANCE," + Include.UseSystemTolerancePercent.ToString(CultureInfo.CurrentUICulture) + ") PCT_TOLERANCE, ");
                //SQLCommand.Append("COALESCE(ws.STORE_FILTER_RID," + Include.UndefinedStoreFilter.ToString(CultureInfo.CurrentUICulture) + ") STORE_FILTER_RID, ");
                //SQLCommand.Append("COALESCE(ws.REVIEW_IND,'0') REVIEW_IND ");
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //SQLCommand.Append("FROM WORKFLOW_STEP_ALLOCATION ws ");
                //// end MID Track # 2354
                //SQLCommand.Append(" WHERE WORKFLOW_RID = " + ((int)aWorkflow_RID).ToString(CultureInfo.CurrentUICulture));
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Workflow_ Steps" );
                return StoredProcedures.MID_WORKFLOW_STEP_ALLOCATION_READ.Read(_dba, WORKFLOW_RID: aWorkflow_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //private StringBuilder GetWorkflowStepBaseSelect()
        //{
        //    StringBuilder SQLCommand = new StringBuilder();
        //    SQLCommand.Append("SELECT WORKFLOW_RID, STEP_NUMBER, ACTION_METHOD_TYPE, WORKFLOW_USER_RID, "); 
        //    SQLCommand.Append("WORKFLOW_DESCRIPTION, COALESCE(STORE_FILTER_RID,-1) STORE_FILTER_RID ");
        //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //    SQLCommand.Append("FROM WORKFLOW");
        //    // end MID Track # 2354

        //    return SQLCommand;
        //}

		/// <summary>
		/// Insert a new row in the COMPONENT_CRITERIA table
		/// </summary>
		public int InsertComponentCriteria(TransactionData td)
		{
			try
			{
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@EXPRESSION", _expression),
                //                            new MIDDbParameter("@SIZE_CODE_RID", _size_Code_RID),
                //                            new MIDDbParameter("@COLOR_CODE_RID", _color_Code_RID),
                //                            new MIDDbParameter("@PACK_NAME", _packName),} ;

                //InParams[0].DbType = eDbType.VarChar;
                //InParams[0].Direction = eParameterDirection.Input;
                //InParams[1].DbType = eDbType.Int;
                //InParams[1].Direction = eParameterDirection.Input;
                //if (_size_Code_RID == Include.NoRID)
                //{
                //    InParams[1].Value = DBNull.Value;
                //}
                //InParams[2].DbType = eDbType.Int;
                //InParams[2].Direction = eParameterDirection.Input;
                //if (_color_Code_RID == Include.NoRID)
                //{
                //    InParams[2].Value = DBNull.Value;
                //}
                //InParams[3].DbType = eDbType.VarChar;
                //InParams[3].Direction = eParameterDirection.Input;
								
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@COMP_CRIT_RID", DBNull.Value) };
                //OutParams[0].DbType = eDbType.Int;
                //OutParams[0].Direction = eParameterDirection.Output;

                //_comp_Crit_RID = td.DBA.ExecuteStoredProcedure("SP_MID_COMP_CRIT_INSERT", InParams, OutParams);

                int? SIZE_CODE_RID_Nullable = null;
                if (_size_Code_RID != Include.NoRID) SIZE_CODE_RID_Nullable = _size_Code_RID;

                int? COLOR_CODE_RID_Nullable = null;
                if (_color_Code_RID != Include.NoRID) COLOR_CODE_RID_Nullable = _color_Code_RID;
                _comp_Crit_RID = StoredProcedures.SP_MID_COMP_CRIT_INSERT.InsertAndReturnRID(td.DBA,
                                                                                             EXPRESSION: _expression,
                                                                                             SIZE_CODE_RID: SIZE_CODE_RID_Nullable,
                                                                                             COLOR_CODE_RID: COLOR_CODE_RID_Nullable,
                                                                                             PACK_NAME: _packName
                                                                                             );


				return _comp_Crit_RID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//        public bool UpdateComponentCriteria(TransactionData td)
//        {
//            bool UpdateSuccessful = true;
//            try
//            {
//                string SQLCommand = "UPDATE COMPONENT_CRITERIA SET "  
//                    + " EXPRESSION = @EXPRESSION," 
//                    + " SIZE_CODE_RID = " + _size_Code_RID.ToString(CultureInfo.CurrentUICulture) + ","
//                    + " COLOR_CODE_RID = " + _color_Code_RID.ToString(CultureInfo.CurrentUICulture) + ","
////					+ " COMP_TYPE_ID = " + ((int)_component_Type).ToString(CultureInfo.CurrentUICulture) + ","
//                    + " PACK_NAME = " + _packName.ToString(CultureInfo.CurrentUICulture)
//                    + " WHERE COMP_CRIT_RID = " + _comp_Crit_RID.ToString(CultureInfo.CurrentUICulture);
//                MIDDbParameter[] InParams  = { new MIDDbParameter("@EXPRESSION", _expression, eDbType.VarChar) } ;

//                td.DBA.ExecuteNonQuery(SQLCommand, InParams);

//                UpdateSuccessful = true;
//            }
//            catch( Exception err )
//            {
//                string exceptionMessage = err.Message;
//                UpdateSuccessful = false;
//                throw;
//            }
//            return UpdateSuccessful;
//        }

		/// <summary>
		/// Delete a row in the COMPONENT_CRITERIA table
		/// </summary>
		public bool DeleteComponentCriteria(TransactionData td, int aComponentCriteriaRID)
		{
			bool UpdateSuccessful = true;
			try
			{
				if (aComponentCriteriaRID != Include.UndefinedComponentCriteria)
				{
                    //string SQLCommand = "DELETE FROM COMPONENT_CRITERIA " 
                    //    + " WHERE COMP_CRIT_RID = " + aComponentCriteriaRID.ToString(CultureInfo.CurrentUICulture);
                    //td.DBA.ExecuteNonQuery(SQLCommand);
                    StoredProcedures.MID_COMPONENT_CRITERIA_DELETE.Delete(td.DBA, COMP_CRIT_RID: aComponentCriteriaRID);
				}

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

		public bool InsertWorkflowStep(int aWorkflowRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
                //string SQLCommand = "INSERT INTO WORKFLOW_STEP_ALLOCATION(WORKFLOW_RID, STEP_NUMBER, ACTION_METHOD_TYPE,"
                //    + "METHOD_RID, REVIEW_IND, PCT_TOLERANCE, STORE_FILTER_RID)"
                //    + " VALUES ("
                //    + aWorkflowRID.ToString(CultureInfo.CurrentUICulture) + ","
                //    + _step_Number.ToString(CultureInfo.CurrentUICulture) + ","
                //    + ((int)_action_Method_Type).ToString(CultureInfo.CurrentUICulture) + ",";

                //if (_method_RID < 0)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += ((int)_method_RID).ToString(CultureInfo.CurrentUICulture) + ",";
                //}
				
                //SQLCommand += Include.ConvertBoolToChar(_review_Ind) + ","
                //    + _pct_Tolerance.ToString(CultureInfo.CurrentUICulture) + ",";
                //if (_step_Store_Filter_RID == Include.NoRID)
                //{
                //    SQLCommand +=  "null";
                //}
                //else
                //{
                //    SQLCommand += _step_Store_Filter_RID.ToString(CultureInfo.CurrentUICulture);
                //}
                //SQLCommand += ")";
                //td.DBA.ExecuteNonQuery(SQLCommand);

                int? METHOD_RID_Nullable = null;
                if (_method_RID >= 0) METHOD_RID_Nullable = _method_RID;

                int? STORE_FILTER_RID_Nullable = null;
                if (_step_Store_Filter_RID != Include.NoRID) STORE_FILTER_RID_Nullable = _step_Store_Filter_RID;

                StoredProcedures.MID_WORKFLOW_STEP_ALLOCATION_INSERT.Insert(td.DBA,
                                                                            WORKFLOW_RID: aWorkflowRID,
                                                                            STEP_NUMBER: _step_Number,
                                                                            ACTION_METHOD_TYPE: (int)_action_Method_Type,
                                                                            METHOD_RID: METHOD_RID_Nullable,
                                                                            REVIEW_IND: Include.ConvertBoolToChar(_review_Ind),
                                                                            PCT_TOLERANCE: _pct_Tolerance,
                                                                            STORE_FILTER_RID: STORE_FILTER_RID_Nullable
                                                                            );

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

//        public bool UpdateWorkflowStep(int aWorkflowRID, TransactionData td)
//        {
//            bool UpdateSuccessful = true;
//            try
//            {
//                string SQLCommand = "UPDATE WORKFLOW_STEP_ALLOCATION SET "  
//                    + " ACTION_METHOD_TYPE = " + ((int)_action_Method_Type).ToString(CultureInfo.CurrentUICulture) + ","
//                    + " METHOD_RID = " + _method_RID.ToString(CultureInfo.CurrentUICulture) + ","
////					+ " COMP_CRIT_RID = " + _comp_Crit_RID.ToString(CultureInfo.CurrentUICulture) + ","
//                    + " PCT_TOLERANCE = " + _pct_Tolerance.ToString(CultureInfo.CurrentUICulture) + ","
//                    + " REVIEW_IND = " + Include.ConvertBoolToChar(_review_Ind) + ","
//                    + " STORE_FILTER_RID = " + _step_Store_Filter_RID.ToString(CultureInfo.CurrentUICulture)
//                    + " WHERE WORKFLOW_RID = " + Workflow_RID.ToString(CultureInfo.CurrentUICulture)
//                    + "   AND STEP_NUMBER = " + _step_Number.ToString(CultureInfo.CurrentUICulture);
				
//                td.DBA.ExecuteNonQuery(SQLCommand);

//                UpdateSuccessful = true;
//            }
//            catch( Exception err )
//            {
//                string exceptionMessage = err.Message;
//                UpdateSuccessful = false;
//                throw;
//            }
//            return UpdateSuccessful;
//        }

        //public bool DeleteWorkflowStep(int aWorkflowRID, TransactionData td)
        //{
        //    bool UpdateSuccessful = true;
        //    try
        //    {
        //        string SQLCommand = "DELETE FROM WORKFLOW_STEP_ALLOCATION " 
        //            + " WHERE WORKFLOW_RID = " + Workflow_RID.ToString(CultureInfo.CurrentUICulture)
        //            + "   AND STEP_NUMBER = " + _step_Number.ToString(CultureInfo.CurrentUICulture);
        //        td.DBA.ExecuteNonQuery(SQLCommand);

        //        UpdateSuccessful = true;
        //    }
        //    catch( Exception err )
        //    {
        //        string exceptionMessage = err.Message;
        //        UpdateSuccessful = false;
        //        throw;
        //    }
        //    return UpdateSuccessful;
        //}

		public bool DeleteAllWorkflowSteps(int aWorkflowRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
				DataTable dtWorkflowSteps = GetWorkflowSteps(aWorkflowRID);
				foreach (DataRow drWorkflowStep in dtWorkflowSteps.Rows)
				{
					int stepNumber = Convert.ToInt32(drWorkflowStep["STEP_NUMBER"], CultureInfo.CurrentUICulture);
					DataTable dtWorkflowStepComponents = GetWorkflowStepComponents(aWorkflowRID, stepNumber);
					foreach (DataRow drWorkflowStepComponents in dtWorkflowStepComponents.Rows)
					{
						eComponentType componentType = (eComponentType)(Convert.ToInt32(drWorkflowStepComponents["COMP_TYPE_ID"], CultureInfo.CurrentUICulture));
						int compCritRID = Convert.ToInt32(drWorkflowStepComponents["COMP_CRIT_RID"], CultureInfo.CurrentUICulture);
						DeleteWorkflowStepComponent(td, aWorkflowRID, stepNumber, componentType, compCritRID);
						if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)componentType))
						{
							DeleteComponentCriteria(td, compCritRID);
						}
					}
                    //string SQLCommand = "DELETE FROM WORKFLOW_STEP_ALLOCATION " 
                    //    + " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
                    //    + "   AND STEP_NUMBER = " + stepNumber.ToString(CultureInfo.CurrentUICulture);
                    //td.DBA.ExecuteNonQuery(SQLCommand);
                    //Begin TT#1238-MD -jsobek -Workflow Error 
                    StoredProcedures.MID_WORKFLOW_STEP_ALLOCATION_DELETE.Delete(td.DBA,
                                                                            WORKFLOW_RID: aWorkflowRID,
                                                                            STEP_NUMBER: stepNumber
                                                                            );
                    //End TT#1238-MD -jsobek -Workflow Error 
				}

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

        // Begin TT#697 - JSmith - Performance
		/// <summary>
		/// Returns DataTable of all Workflows steps
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetWorkflowStepComponents(int aWorkflow_RID)
		{
			try
			{	
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT sc.WORKFLOW_RID, sc.STEP_NUMBER, sc.COMP_TYPE_ID, "); 
                //SQLCommand.Append("COALESCE(sc.COMP_CRIT_RID,-1) COMP_CRIT_RID, ");
                //SQLCommand.Append("COALESCE(cc.SIZE_CODE_RID,-1) SIZE_CODE_RID, ");
                //SQLCommand.Append("COALESCE(cc.COLOR_CODE_RID,-1) COLOR_CODE_RID, ");
                //SQLCommand.Append("COALESCE(cc.PACK_NAME,' ') PACK_NAME ");
                //SQLCommand.Append("FROM ALLOCATION_STEP_COMPONENT sc LEFT OUTER JOIN COMPONENT_CRITERIA cc ON sc.COMP_CRIT_RID = cc.COMP_CRIT_RID");
                //SQLCommand.Append(" WHERE WORKFLOW_RID = " + ((int)aWorkflow_RID).ToString(CultureInfo.CurrentUICulture));
                //SQLCommand.Append("   order by STEP_NUMBER");
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Workflow_ Steps" );
                return StoredProcedures.MID_ALLOCATION_STEP_COMPONENT_READ.Read(_dba, WORKFLOW_RID: aWorkflow_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // End TT#697

        /// <summary>
        /// Returns DataTable of all Workflows steps
        /// </summary>
        /// <returns>DataTable</returns>
		public DataTable GetWorkflowStepComponents(int aWorkflow_RID, int aStepNumber)
		{
			try
			{	
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT sc.WORKFLOW_RID, sc.STEP_NUMBER, sc.COMP_TYPE_ID, "); 
                //SQLCommand.Append("COALESCE(sc.COMP_CRIT_RID,-1) COMP_CRIT_RID, ");
                //SQLCommand.Append("COALESCE(cc.SIZE_CODE_RID,-1) SIZE_CODE_RID, ");
                //SQLCommand.Append("COALESCE(cc.COLOR_CODE_RID,-1) COLOR_CODE_RID, ");
                //SQLCommand.Append("COALESCE(cc.PACK_NAME,' ') PACK_NAME ");
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //SQLCommand.Append("FROM ALLOCATION_STEP_COMPONENT sc LEFT OUTER JOIN COMPONENT_CRITERIA cc ON sc.COMP_CRIT_RID = cc.COMP_CRIT_RID");
                //// end MID Track # 2354
                //SQLCommand.Append(" WHERE WORKFLOW_RID = " + ((int)aWorkflow_RID).ToString(CultureInfo.CurrentUICulture));
                //SQLCommand.Append("   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture));
				
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Workflow_ Steps" );
                return StoredProcedures.MID_ALLOCATION_STEP_COMPONENT_READ_STEP.Read(_dba,
                                                                                     WORKFLOW_RID: aWorkflow_RID,
                                                                                     STEP_NUMBER: aStepNumber
                                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool InsertWorkflowStepComponent(int aWorkflowRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
                //string SQLCommand = "INSERT INTO ALLOCATION_STEP_COMPONENT(WORKFLOW_RID, STEP_NUMBER, COMP_TYPE_ID,"
                //    + "COMP_CRIT_RID)"
                //    + " VALUES ("
                //    + aWorkflowRID.ToString(CultureInfo.CurrentUICulture) + ","
                //    + _step_Number.ToString(CultureInfo.CurrentUICulture) + ","
                //    + ((int)_component_Type).ToString(CultureInfo.CurrentUICulture) + ",";
                //if (_comp_Crit_RID == Include.NoRID)
                //{
                //    SQLCommand +=  Include.UndefinedComponentCriteria.ToString(CultureInfo.CurrentUICulture);
                //}
                //else
                //{
                //    SQLCommand += (_comp_Crit_RID).ToString(CultureInfo.CurrentUICulture);
                //}
                //SQLCommand += ")";
                //td.DBA.ExecuteNonQuery(SQLCommand);

                int COMP_CRIT_RID;
                if (_comp_Crit_RID == Include.NoRID)
                {
                    COMP_CRIT_RID = Include.UndefinedComponentCriteria;
                }
                else
                {
                    COMP_CRIT_RID = _comp_Crit_RID;
                }
                StoredProcedures.MID_ALLOCATION_STEP_COMPONENT_INSERT.Insert(td.DBA,
                                                                             WORKFLOW_RID: aWorkflowRID,
                                                                             STEP_NUMBER: _step_Number,
                                                                             COMP_TYPE_ID: (int)_component_Type,
                                                                             COMP_CRIT_RID: COMP_CRIT_RID
                                                                             );

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

		public bool DeleteWorkflowStepComponent(TransactionData td, int aWorkflowRID, int aStepNumber,
			eComponentType aComponentType, int aComponentCriteriaRID)
		{
			bool UpdateSuccessful = true;
			try
			{
                //string SQLCommand = "DELETE FROM ALLOCATION_STEP_COMPONENT " 
                //    + " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
                //    + "   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture)
                //    + "   AND COMP_TYPE_ID = " + ((int)aComponentType).ToString(CultureInfo.CurrentUICulture)
                //    + "   AND COMP_CRIT_RID = " + aComponentCriteriaRID.ToString(CultureInfo.CurrentUICulture);
                //td.DBA.ExecuteNonQuery(SQLCommand);
                StoredProcedures.MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA.Delete(td.DBA,
                                                                                           WORKFLOW_RID: aWorkflowRID,
                                                                                           STEP_NUMBER: aStepNumber,
                                                                                           COMP_TYPE_ID: (int)aComponentType,
                                                                                           COMP_CRIT_RID: aComponentCriteriaRID
                                                                                           );

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

		public bool DeleteAllWorkflowStepComponents(int aWorkflowRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
                //string SQLCommand = "DELETE FROM ALLOCATION_STEP_COMPONENT " 
                //    + " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
                //    + "   AND STEP_NUMBER = " + _step_Number.ToString(CultureInfo.CurrentUICulture);
                //td.DBA.ExecuteNonQuery(SQLCommand);

                StoredProcedures.MID_ALLOCATION_STEP_COMPONENT_DELETE.Delete(td.DBA,
                                                                             WORKFLOW_RID: aWorkflowRID,
                                                                             STEP_NUMBER: _step_Number
                                                                             );

				UpdateSuccessful = true;
			}
			catch( Exception err )
			{
				string exceptionMessage = err.Message;
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}


	}			
} 
 
