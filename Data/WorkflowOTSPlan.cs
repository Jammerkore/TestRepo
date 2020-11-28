using System;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public class OTSPlanWorkflowData: WorkflowBaseData
	{

		public OTSPlanWorkflowData(): base()
		{
		}
       

		/// <summary>
		/// Creates an instance of the MethodRule class.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		public OTSPlanWorkflowData(int aWorkflow_RID, eChangeType changeType) : base()
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
		public OTSPlanWorkflowData(TransactionData td)
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
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT ws.WORKFLOW_RID, ws.STEP_NUMBER, ws.ACTION_METHOD_TYPE, "); 
                //SQLCommand.Append("COALESCE(ws.METHOD_RID," + Include.UndefinedMethodRID.ToString(CultureInfo.CurrentUICulture) + ") METHOD_RID, ");
                //SQLCommand.Append("COALESCE(ws.PCT_TOLERANCE," + Include.UseSystemTolerancePercent.ToString(CultureInfo.CurrentUICulture) + ") PCT_TOLERANCE, ");
                //SQLCommand.Append("COALESCE(ws.STORE_FILTER_RID," + Include.UndefinedStoreFilter.ToString(CultureInfo.CurrentUICulture) + ") STORE_FILTER_RID, ");
                //SQLCommand.Append("COALESCE(ws.REVIEW_IND,'0') REVIEW_IND, ");
                //SQLCommand.Append("COALESCE(ws.VARIABLE_NUMBER,-1) VARIABLE_NUMBER, ");
                //SQLCommand.Append("ws.CALC_MODE, ");
                //SQLCommand.Append("COALESCE(ws.BAL_IND,'0') BAL_IND ");
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //SQLCommand.Append("FROM WORKFLOW_STEP_OTSPLAN ws ");
                //// end MID Track # 2354
                //SQLCommand.Append(" WHERE WORKFLOW_RID = " + ((int)aWorkflow_RID).ToString(CultureInfo.CurrentUICulture));
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Workflow_ Steps" );
                //MIDDbParameter[] InParams = { new MIDDbParameter("@WORKFLOW_RID", aWorkflow_RID, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForRead("MID_WORKFLOW_STEP_OTSPLAN_READ", InParams);
                return StoredProcedures.MID_WORKFLOW_STEP_OTSPLAN_READ.Read(_dba, WORKFLOW_RID: aWorkflow_RID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance - unused function
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
        //End TT#846-MD -jsobek -New Stored Procedures for Performance - unused function

//		/// <summary>
//		/// Insert a new row in the COMPONENT_CRITERIA table
//		/// </summary>
//		public int InsertComponentCriteria(TransactionData td)
//		{
//			try
//			{
//				MIDDbParameter[] InParams  = { new MIDDbParameter("@EXPRESSION", _expression),
//											  new MIDDbParameter("@SIZE_CODE_RID", _size_Code_RID),
//											  new MIDDbParameter("@COLOR_CODE_RID", _color_Code_RID),
//											  new MIDDbParameter("@PACK_NAME", _packName),} ;
//
//				InParams[0].DbType = eDbType.VarChar;
//				InParams[0].Direction = eParameterDirection.Input;
//				InParams[1].DbType = eDbType.Int;
//				InParams[1].Direction = eParameterDirection.Input;
//				if (_size_Code_RID == Include.NoRID)
//				{
//					InParams[1].Value = DBNull.Value;
//				}
//				InParams[2].DbType = eDbType.Int;
//				InParams[2].Direction = eParameterDirection.Input;
//				if (_color_Code_RID == Include.NoRID)
//				{
//					InParams[2].Value = DBNull.Value;
//				}
//				InParams[3].DbType = eDbType.VarChar;
//				InParams[3].Direction = eParameterDirection.Input;
//								
//				MIDDbParameter[] OutParams = { new MIDDbParameter("@COMP_CRIT_RID", DBNull.Value) };
//				OutParams[0].DbType = eDbType.Int;
//				OutParams[0].Direction = eParameterDirection.Output;
//
//				_comp_Crit_RID = td.DBA.ExecuteStoredProcedure("SP_MID_COMP_CRIT_INSERT", InParams, OutParams);
//				return _comp_Crit_RID;
//			}
//			catch ( Exception err )
//			{
//				throw;
//			}
//		}
//
//		public bool UpdateComponentCriteria(TransactionData td)
//		{
//			bool UpdateSuccessful = true;
//			try
//			{
//				string SQLCommand = "UPDATE COMPONENT_CRITERIA SET "  
//					+ " EXPRESSION = @EXPRESSION," 
//					+ " SIZE_CODE_RID = " + _size_Code_RID.ToString(CultureInfo.CurrentUICulture) + ","
//					+ " COLOR_CODE_RID = " + _color_Code_RID.ToString(CultureInfo.CurrentUICulture) + ","
//					//					+ " COMP_TYPE_ID = " + ((int)_component_Type).ToString(CultureInfo.CurrentUICulture) + ","
//					+ " PACK_NAME = " + _packName.ToString(CultureInfo.CurrentUICulture)
//					+ " WHERE COMP_CRIT_RID = " + _comp_Crit_RID.ToString(CultureInfo.CurrentUICulture);
//				MIDDbParameter[] InParams  = { new MIDDbParameter("@EXPRESSION", _expression, eDbType.VarChar) } ;
//
//				td.DBA.ExecuteNonQuery(SQLCommand, InParams);
//
//				UpdateSuccessful = true;
//			}
//			catch( Exception err )
//			{
//				string exceptionMessage = err.Message;
//				UpdateSuccessful = false;
//				throw;
//			}
//			return UpdateSuccessful;
//		}
//
//		/// <summary>
//		/// Delete a row in the COMPONENT_CRITERIA table
//		/// </summary>
//		public bool DeleteComponentCriteria(TransactionData td, int aComponentCriteriaRID)
//		{
//			bool UpdateSuccessful = true;
//			try
//			{
//				if (aComponentCriteriaRID != Include.UndefinedComponentCriteria)
//				{
//					string SQLCommand = "DELETE FROM COMPONENT_CRITERIA " 
//						+ " WHERE COMP_CRIT_RID = " + aComponentCriteriaRID.ToString(CultureInfo.CurrentUICulture);
//					td.DBA.ExecuteNonQuery(SQLCommand);
//				}
//
//				UpdateSuccessful = true;
//			}
//			catch( Exception err )
//			{
//				string exceptionMessage = err.Message;
//				UpdateSuccessful = false;
//				throw;
//			}
//			return UpdateSuccessful;
//		}

        public bool InsertWorkflowStep(int aWorkflowRID, TransactionData td, int aStepNumber, eMethodType aMethodType,
            int aMethodRID, bool aReviewInd, double aPctTolerance, int aStoreFilterRID, int aVariableNumber,
            string aComputationMode, bool aBalanceInd)
        {
            bool UpdateSuccessful = true;
            try
            {
                //string SQLCommand = "INSERT INTO WORKFLOW_STEP_OTSPLAN(WORKFLOW_RID, STEP_NUMBER, ACTION_METHOD_TYPE,"
                //    + "METHOD_RID, REVIEW_IND, PCT_TOLERANCE, STORE_FILTER_RID, VARIABLE_NUMBER, CALC_MODE, BAL_IND)"
                //    + " VALUES ("
                //    + aWorkflowRID.ToString(CultureInfo.CurrentUICulture) + ","
                //    + aStepNumber.ToString(CultureInfo.CurrentUICulture) + ","
                //    + ((int)aMethodType).ToString(CultureInfo.CurrentUICulture) + ",";

                //SQLCommand += aMethodRID.ToString(CultureInfo.CurrentUICulture) + ",";
                //SQLCommand += Include.ConvertBoolToChar(aReviewInd) + ","
                //    + aPctTolerance.ToString(CultureInfo.CurrentUICulture) + ",";
                //if (aStoreFilterRID == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += aStoreFilterRID.ToString(CultureInfo.CurrentUICulture) + ",";
                //}

                //if (aVariableNumber == Include.NoRID)
                //{
                //    SQLCommand +=  "null,";
                //}
                //else
                //{
                //    SQLCommand += aVariableNumber.ToString(CultureInfo.CurrentUICulture) + ",";
                //}
                //SQLCommand += aComputationMode.ToString(CultureInfo.CurrentUICulture) + ",";
                //SQLCommand += Include.ConvertBoolToChar(aBalanceInd).ToString(CultureInfo.CurrentUICulture);
                //SQLCommand += ")";
                //td.DBA.ExecuteNonQuery(SQLCommand);

                int? STORE_FILTER_RID_Nullable = null;
                if (aStoreFilterRID != Include.NoRID) STORE_FILTER_RID_Nullable = aStoreFilterRID;
                int? VARIABLE_NUMBER_Nullable = null;
                if (aVariableNumber != Include.NoRID) VARIABLE_NUMBER_Nullable = aVariableNumber;
                StoredProcedures.MID_WORKFLOW_STEP_OTSPLAN_INSERT.Insert(td.DBA,
                                                                         WORKFLOW_RID: aWorkflowRID,
                                                                         STEP_NUMBER: aStepNumber,
                                                                         ACTION_METHOD_TYPE: (int)aMethodType,
                                                                         METHOD_RID: aMethodRID,
                                                                         REVIEW_IND: Include.ConvertBoolToChar(aReviewInd),
                                                                         PCT_TOLERANCE: aPctTolerance,
                                                                         STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                         VARIABLE_NUMBER: VARIABLE_NUMBER_Nullable,
                                                                         CALC_MODE: aComputationMode,
                                                                         BAL_IND: Include.ConvertBoolToChar(aBalanceInd)
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
        
        //public bool UpdateWorkflowStep(int aWorkflowRID, TransactionData td, int aStepNumber, eMethodType aMethodType,
        //    int aMethodRID, bool aReviewInd, double aPctTolerance, int aStoreFilterRID, int aVariableNumber,
        //    string aComputationMode, bool aBalanceInd)
        //{
        //    bool UpdateSuccessful = true;
        //    try
        //    {
        //        string SQLCommand = "UPDATE WORKFLOW_STEP_OTSPLAN SET "  
        //            + " ACTION_METHOD_TYPE = " + ((int)aMethodType).ToString(CultureInfo.CurrentUICulture) + ","
        //            + " METHOD_RID = " + aMethodRID.ToString(CultureInfo.CurrentUICulture) + ","
        //            + " PCT_TOLERANCE = " + aPctTolerance.ToString(CultureInfo.CurrentUICulture) + ","
        //            + " REVIEW_IND = " + Include.ConvertBoolToChar(aReviewInd) + ","
        //            + " STORE_FILTER_RID = " + aStoreFilterRID.ToString(CultureInfo.CurrentUICulture) + ","
        //            + " VARIABLE_NUMBER = " + aVariableNumber.ToString(CultureInfo.CurrentUICulture) + ","
        //            + " CALC_MODE = " + aComputationMode.ToString(CultureInfo.CurrentUICulture) + ","
        //            + " BAL_IND = " + Include.ConvertBoolToChar(aBalanceInd)
        //            + " WHERE WORKFLOW_RID = " + Workflow_RID.ToString(CultureInfo.CurrentUICulture)
        //            + "   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture);
				
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
        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance - unused function
        //public bool DeleteWorkflowStep(int aWorkflowRID, TransactionData td, int aStepNumber)
        //{
        //    bool UpdateSuccessful = true;
        //    try
        //    {
        //        string SQLCommand = "DELETE FROM WORKFLOW_STEP_OTSPLAN " 
        //            + " WHERE WORKFLOW_RID = " + Workflow_RID.ToString(CultureInfo.CurrentUICulture)
        //            + "   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture);
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
        //End TT#846-MD -jsobek -New Stored Procedures for Performance - unused function
		public bool DeleteAllWorkflowSteps(int aWorkflowRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
				DataTable dtWorkflowSteps = GetWorkflowSteps(aWorkflowRID);
				foreach (DataRow drWorkflowStep in dtWorkflowSteps.Rows)
				{
					int stepNumber = Convert.ToInt32(drWorkflowStep["STEP_NUMBER"], CultureInfo.CurrentUICulture);
//					DataTable dtWorkflowStepComponents = GetWorkflowStepComponents(aWorkflowRID, stepNumber);
//					foreach (DataRow drWorkflowStepComponents in dtWorkflowStepComponents.Rows)
//					{
//						eComponentType componentType = (eComponentType)(Convert.ToInt32(drWorkflowStepComponents["COMP_TYPE_ID"], CultureInfo.CurrentUICulture));
//						int compCritRID = Convert.ToInt32(drWorkflowStepComponents["COMP_CRIT_RID"], CultureInfo.CurrentUICulture);
//						DeleteWorkflowStepComponent(td, aWorkflowRID, stepNumber, componentType, compCritRID);
//						if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)componentType))
//						{
//							DeleteComponentCriteria(td, compCritRID);
//						}
//					}
                    //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                    //string SQLCommand = "DELETE FROM WORKFLOW_STEP_OTSPLAN " 
                    //    + " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
                    //    + "   AND STEP_NUMBER = " + stepNumber.ToString(CultureInfo.CurrentUICulture);
                    //td.DBA.ExecuteNonQuery(SQLCommand);

                    //MIDDbParameter[] InParams = { new MIDDbParameter("@WORKFLOW_RID", aWorkflowRID, eDbType.Int),
                    //                              new MIDDbParameter("@STEP_NUMBER", stepNumber, eDbType.Int)
                    //                            };
                    //_dba.ExecuteStoredProcedureForDelete("MID_WORKFLOW_STEP_OTSPLAN_DELETE", InParams);
    
                    StoredProcedures.MID_WORKFLOW_STEP_OTSPLAN_DELETE.Delete(td.DBA, WORKFLOW_RID: aWorkflowRID,
                                                                             STEP_NUMBER: stepNumber
                                                                            );
                    //End TT#846-MD -jsobek -New Stored Procedures for Performance
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

//		/// <summary>
//		/// Returns DataTable of all Workflows steps
//		/// </summary>
//		/// <returns>DataTable</returns>
//		public DataTable GetWorkflowStepComponents(int aWorkflow_RID, int aStepNumber)
//		{
//			try
//			{	
//				StringBuilder SQLCommand = new StringBuilder();
//				SQLCommand.Append("SELECT sc.WORKFLOW_RID, sc.STEP_NUMBER, sc.COMP_TYPE_ID, "); 
//				SQLCommand.Append("COALESCE(sc.COMP_CRIT_RID,-1) COMP_CRIT_RID, ");
//				SQLCommand.Append("COALESCE(cc.SIZE_CODE_RID,-1) SIZE_CODE_RID, ");
//				SQLCommand.Append("COALESCE(cc.COLOR_CODE_RID,-1) COLOR_CODE_RID, ");
//				SQLCommand.Append("COALESCE(cc.PACK_NAME,' ') PACK_NAME ");
//				// begin MID Track # 2354 - removed nolock because it causes concurrency issues
//				SQLCommand.Append("FROM OTSPlan_STEP_COMPONENT sc LEFT OUTER JOIN COMPONENT_CRITERIA cc ON sc.COMP_CRIT_RID = cc.COMP_CRIT_RID");
//				// end MID Track # 2354
//				SQLCommand.Append(" WHERE WORKFLOW_RID = " + ((int)aWorkflow_RID).ToString(CultureInfo.CurrentUICulture));
//				SQLCommand.Append("   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture));
//				
//				
//				return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Workflow_ Steps" );
//			}
//			catch ( Exception err )
//			{
//				throw;
//			}
//		}
//
//		public bool InsertWorkflowStepComponent(int aWorkflowRID, TransactionData td)
//		{
//			bool UpdateSuccessful = true;
//			try
//			{
//				string SQLCommand = "INSERT INTO OTSPlan_STEP_COMPONENT(WORKFLOW_RID, STEP_NUMBER, COMP_TYPE_ID,"
//					+ "COMP_CRIT_RID)"
//					+ " VALUES ("
//					+ aWorkflowRID.ToString(CultureInfo.CurrentUICulture) + ","
//					+ _step_Number.ToString(CultureInfo.CurrentUICulture) + ","
//					+ ((int)_component_Type).ToString(CultureInfo.CurrentUICulture) + ",";
//				if (_comp_Crit_RID == Include.NoRID)
//				{
//					SQLCommand +=  Include.UndefinedComponentCriteria.ToString(CultureInfo.CurrentUICulture);
//				}
//				else
//				{
//					SQLCommand += (_comp_Crit_RID).ToString(CultureInfo.CurrentUICulture);
//				}
//				SQLCommand += ")";
//				td.DBA.ExecuteNonQuery(SQLCommand);
//
//				UpdateSuccessful = true;
//			}
//			catch( Exception err )
//			{
//				string exceptionMessage = err.Message;
//				UpdateSuccessful = false;
//				throw;
//			}
//			return UpdateSuccessful;
//		}
//
//		public bool DeleteWorkflowStepComponent(TransactionData td, int aWorkflowRID, int aStepNumber,
//			eComponentType aComponentType, int aComponentCriteriaRID)
//		{
//			bool UpdateSuccessful = true;
//			try
//			{
//				string SQLCommand = "DELETE FROM OTSPlan_STEP_COMPONENT " 
//					+ " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
//					+ "   AND STEP_NUMBER = " + aStepNumber.ToString(CultureInfo.CurrentUICulture)
//					+ "   AND COMP_TYPE_ID = " + ((int)aComponentType).ToString(CultureInfo.CurrentUICulture)
//					+ "   AND COMP_CRIT_RID = " + aComponentCriteriaRID.ToString(CultureInfo.CurrentUICulture);
//				td.DBA.ExecuteNonQuery(SQLCommand);
//
//				UpdateSuccessful = true;
//			}
//			catch( Exception err )
//			{
//				string exceptionMessage = err.Message;
//				UpdateSuccessful = false;
//				throw;
//			}
//			return UpdateSuccessful;
//		}
//
//		public bool DeleteAllWorkflowStepComponents(int aWorkflowRID, TransactionData td)
//		{
//			bool UpdateSuccessful = true;
//			try
//			{
//				string SQLCommand = "DELETE FROM OTSPlan_STEP_COMPONENT " 
//					+ " WHERE WORKFLOW_RID = " + aWorkflowRID.ToString(CultureInfo.CurrentUICulture)
//					+ "   AND STEP_NUMBER = " + _step_Number.ToString(CultureInfo.CurrentUICulture);
//				td.DBA.ExecuteNonQuery(SQLCommand);
//
//				UpdateSuccessful = true;
//			}
//			catch( Exception err )
//			{
//				string exceptionMessage = err.Message;
//				UpdateSuccessful = false;
//				throw;
//			}
//			return UpdateSuccessful;
//		}


	}			
} 
 
