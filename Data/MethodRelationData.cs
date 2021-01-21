using System;
using System.Data;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Created to utilize one connection to the DB to save all Method data 
	/// and rollback any inserts on error.
	/// </summary>
	public class MethodRelationData: DataLayer
	{
		public MethodRelationData()
		{

		}

		public int InsertMethod(string Name, eMethodType Method_Type_ID, eProfileType Profile_Type_ID, int User_RID,
                                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                                //string Method_Description, int SG_RID, char Virtual_IND, int methosStatus, int customOLL_RID)
                                string Method_Description, int SG_RID, char Virtual_IND, int methosStatus, int customOLL_RID, int aUpdateUserRID,
                                char template_IND)
                                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			MethodBaseData mb = new MethodBaseData();

			return mb.InsertMethod(Name, Method_Type_ID, Profile_Type_ID, User_RID, Method_Description,
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //SG_RID, Virtual_IND, methosStatus, customOLL_RID, _dba);
                SG_RID, Virtual_IND, methosStatus, customOLL_RID, _dba, aUpdateUserRID, template_IND);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		}


		public bool InsertOTSPlan(Object [] values)
		{
//			MethodOTSPlan OTSPlanData = new MethodOTSPlan();

//			return OTSPlanData.InsertOTSPlan(values,_dba);
			return false;
		}
		
		public bool InsertGLF(Object [] values, int method_RID)
		{
//			GroupLevelFunction GLFData = new GroupLevelFunction();

			return false;
//			return GLFData.InsertGLF(values,method_RID,_dba);
		}
	
		public bool InsertGeneralAllocation(Object [] values)
		{
//			MethodGeneralAllocation GenAllocData = new MethodGeneralAllocation();

//			return GenAllocData.InsertGeneralAllocation(values,_dba);
			return false;
		}

//		/// <summary>
//		/// Insert a new row in the Method table
//		/// </summary>
//		/// <param name="method_Name"></param>
//		/// <param name="method_Type_ID"></param>
//		/// <param name="user_RID"></param>
//		/// <param name="method_Description"></param>
//		/// <param name="sg_RID"></param>
//		/// <returns>
//		/// return eGenericDBError.DuplicateKey if dupe Alt. Key 
//		/// otherwise returns new Method_RID
//		/// </returns>
//		public int InsertMethod(string method_Name, eMethodType method_Type_ID,
//			int user_RID, string method_Description, int sg_RID, char virtual_IND)
//		{
//			int method_RID = -1;
//
//			try
//			{
//				if(GenericRowExists("METHOD", BuildMethodAK(method_Name, method_Type_ID,
//															user_RID)))
//				{
//					return (int)eGenericDBError.DuplicateKey;
//				}
//
//				MIDDbParameter[] InParams  = { new MIDDbParameter("@METHOD_NAME", method_Name),
//											  new MIDDbParameter("@METHOD_TYPE_ID", Convert.ToInt32(method_Type_ID)),
//											  new MIDDbParameter("@USER_RID", user_RID),
//											  new MIDDbParameter("@METHOD_DESCRIPTION", method_Description),
//											  new MIDDbParameter("@SG_RID", sg_RID),
//											  new MIDDbParameter("@VIRTUAL_IND", virtual_IND)} ;
//
//				InParams[0].DbType = eDbType.VarChar;
//				InParams[0].Direction = eParameterDirection.Input;
//				InParams[1].DbType = eDbType.Int;
//				InParams[1].Direction = eParameterDirection.Input;
//				InParams[2].DbType = eDbType.Int;
//				InParams[2].Direction = eParameterDirection.Input;
//				InParams[3].DbType = eDbType.VarChar;
//				InParams[3].Direction = eParameterDirection.Input;
//				InParams[4].DbType = eDbType.Int;
//				InParams[4].Direction = eParameterDirection.Input;
//				InParams[5].DbType = eDbType.Char;
//				InParams[5].Direction = eParameterDirection.Input;
//				
//				MIDDbParameter[] OutParams = { new MIDDbParameter("@METHOD_RID", DBNull.Value) };
//				OutParams[0].DbType = eDbType.Int;
//				OutParams[0].Direction = eParameterDirection.Output;
//
//				method_RID = _dba.ExecuteStoredProcedure("SP_MID_METHOD_INSERT", InParams, OutParams);
//				return method_RID;
//			}
//			catch ( Exception err )
//			{
//				throw;
//			}
//		}

//		/// <summary>
//		/// Insert a row in the OTS_Plan table
//		/// </summary>
//		/// <param name="values"></param>
//		/// <returns>boolean depending if Insert completed or failed</returns>
//		public bool InsertOTSPlan(Object [] values)
//		{
//			try
//			{	
//				StringBuilder SQLCommand = new StringBuilder();
//
//				SQLCommand.Append("INSERT INTO OTS_PLAN(METHOD_RID, PLAN_HN_RID, PLAN_FV_RID, "); 
//				SQLCommand.Append("CDR_RID, CHAIN_FV_RID, BAL_SALES_IND, BAL_STOCK_IND) ");	
//				SQLCommand.Append("VALUES (");
//				SQLCommand.Append(values[0]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[1]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[2]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[3]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[4]);
//				SQLCommand.Append(",'");
//				SQLCommand.Append(values[5]);
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[6]);
//				SQLCommand.Append("')");
//
//				_dba.ExecuteNonQuery(SQLCommand.ToString());
//	
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}

//		/// <summary>
//		/// Insert a row in the Group_Level_Function table
//		/// </summary>
//		/// <param name="values"></param>
//		/// <param name="method_RID"></param>
//		/// <returns>boolean depending if Insert completed or failed</returns>
//		public bool InsertGLF(Object [] values, int method_RID)
//		{
//			try
//			{	
//				StringBuilder SQLCommand = new StringBuilder();
//
//				SQLCommand.Append("INSERT INTO GROUP_LEVEL_FUNCTION(METHOD_RID, ");
//				SQLCommand.Append("SGL_RID, DEFAULT_IND, PLAN_IND, "); 
//				SQLCommand.Append("USE_DEFAULT_IND, CLEAR_IND, SEASON_IND, SEASON_HN_RID, GLFT_ID, GLSB_ID) ");
//				SQLCommand.Append("VALUES (");
//				SQLCommand.Append(method_RID);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[0]);
//				SQLCommand.Append(",'");
//				SQLCommand.Append(values[1]);
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[2]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[3]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[4]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[5]);//
//				SQLCommand.Append("',");
//				SQLCommand.Append(values[6]);//
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[7]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[8]);
//				SQLCommand.Append(")");
//
//				_dba.ExecuteNonQuery(SQLCommand.ToString());
//	
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}
		
//		public bool InsertGenAlloc(Object [] values)
//		{
//			try
//			{	
//				// The following array values can be nulled, so the "NoRid" needs
//				// to be replaced with null.
//				//ar[3] =  Merch_HN_RID;
//				//ar[4] =  Merch_PH_RID;
//				//ar[5] =  Merch_PHL_Sequence;
//				//ar[6] =  Gen_Alloc_HDR_RID;
//				//ar[7] =  Reserve;
//				//ar[8] =  Percent_Ind;	
//				StringBuilder SQLCommand = new StringBuilder();
//				
//				SQLCommand.Append("INSERT INTO METHOD_GENERAL_ALLOCATION ");
//				SQLCommand.Append("(METHOD_RID, BEGIN_CDR_RID, SHIP_TO_CDR_RID, "); 
//				SQLCommand.Append("MERCH_HN_RID, MERCH_PH_RID, MERCH_PHL_SEQUENCE, ");	
//				SQLCommand.Append("GEN_ALLOC_HDR_RID, RESERVE, PERCENT_IND)");
//				SQLCommand.Append(" VALUES (");
//				SQLCommand.Append(values[0]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[1]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[2]);
//				SQLCommand.Append(",");
//				//Merch_HN_RID
//				if ((int)values[3] == Include.NoRID)
//					values[3] = "null" ;
//				SQLCommand.Append(values[3]);
//				SQLCommand.Append(",");
//				//Merch_PH_RID & Merch_PHL_Sequence
//				if ((int)values[4] == Include.NoRID)
//				{
//					values[4] = "null";
//					values[5] = "null";
//				}
//				SQLCommand.Append(values[4]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[5]);
//				SQLCommand.Append(",");
//				// Gen_Alloc_HDR_RID
//				if ((int)values[6] == Include.NoRID)
//					values[6] = "null";
//				SQLCommand.Append(values[6]);
//				SQLCommand.Append(",");
//				// Reserve amt
//				if ((double)values[7] == Include.UndefinedReserve)
//				{
//					values[7] = "null";
//					values[8] = "null";
//					SQLCommand.Append(values[7]);
//					SQLCommand.Append(",");
//					SQLCommand.Append(values[8]);
//					SQLCommand.Append(" )");
//				}	
//				else
//				{
//					SQLCommand.Append(values[7]);
//					SQLCommand.Append(",'");
//					SQLCommand.Append(values[8]);
//					SQLCommand.Append("')");
//				}
//
//				_dba.ExecuteNonQuery(SQLCommand.ToString());
//	
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}

//		/// <summary>
//		/// Builds WHERE statement based on Method AK.
//		/// </summary>
//		/// <param name="method_Name"></param>
//		/// <param name="method_Type_ID"></param>
//		/// <param name="user_RID"></param>
//		/// <returns>returns string WHERE statement</returns>
//		private string BuildMethodAK(string method_Name, eMethodType method_Type_ID,
//			int user_RID)
//		{
//			StringBuilder sb = new StringBuilder();
//			sb.Append("WHERE METHOD_NAME = '");
//			sb.Append(method_Name.Replace("'","''"));
//			sb.Append("' AND METHOD_TYPE_ID = ");
//			sb.Append((int)method_Type_ID);
//			sb.Append(" AND USER_RID = ");
//			sb.Append(user_RID);
//
//			return sb.ToString();
//		}

//		/// <summary>
//		/// Check if a row exists in param tableName where param whereClause.
//		/// </summary>
//		/// <param name="tableName">Name of table to query</param>
//		/// <param name="whereClause">WHERE clause</param>
//		/// <returns>bool if row(s) exists</returns>
//		private bool GenericRowExists(string tableName, string whereClause)
//		{
//			string SQLCommand = "SELECT COUNT(*) AS MyCount FROM " + tableName + " " + whereClause;
//				
//			int recordCount = _dba.ExecuteRecordCount( SQLCommand);
//				
//			if (recordCount == 0)
//				return false;
//
//			return true;
//		}
	}
}
