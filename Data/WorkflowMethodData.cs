using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class WorkflowMethodData: DataLayer
	{
		
		public WorkflowMethodData()
		{
		}

        //Begin TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT
        //public DataTable GetWM_Exp_Static_Nodes()
        //{
        //    try
        //    {
        //        return StoredProcedures.MID_WM_EXP_NODES_STRUCT_READ.Read(_dba);
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT

		public int Method_Row_Count(eMethodTypeUI aMethodType, int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_READ_COUNT.ReadRecordCount(_dba, 
                                                                              METHOD_TYPE_ID: (int)aMethodType,
                                                                              USER_RID: aUserRID
                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Method_Row_Count(eMethodType aMethodType, string aMethodName, int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_READ_COUNT_FROM_NAME.ReadRecordCount(_dba, 
                                                                                         USER_RID: aUserRID,
                                                                                         METHOD_TYPE_ID: (int)aMethodType,
                                                                                         METHOD_NAME: aMethodName
                                                                                         );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Workflow_Row_Count(eWorkflowType aWorkflowType, int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER.ReadRecordCount(_dba,
                                                                                                    WORKFLOW_TYPE_ID: (int)aWorkflowType,
                                                                                                    USER_RID: aUserRID
                                                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Workflow_Row_Count(eWorkflowType aWorkflowType, string aWorkflowName, int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER.ReadRecordCount(_dba,
                                                                                                    WORKFLOW_TYPE_ID: (int)aWorkflowType,
                                                                                                    WORKFLOW_USER_RID: aUserRID,
                                                                                                    WORKFLOW_NAME: aWorkflowName
                                                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public string GetMethodName(int aMethodRID)
		{
			try
			{	
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_METHOD_READ_NAME.Read(_dba, METHOD_RID: aMethodRID);
				if (dt.Rows.Count == 1)
				{
					return Convert.ToString(dt.Rows[0]["METHOD_NAME"], CultureInfo.CurrentUICulture);
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

//Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
		public eMethodType GetMethodType(int aMethodRID)
		{
			try
			{	
                DataTable dt = StoredProcedures.MID_METHOD_READ_TYPE.Read(_dba, METHOD_RID: aMethodRID);
				if (dt.Rows.Count == 1)
				{
					return (eMethodType)Convert.ToInt32(dt.Rows[0]["METHOD_TYPE_ID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return eMethodType.NotSpecified;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
		public DataTable GetMethodList(eMethodTypeUI aMethodType)
		{
			try
			{
                return StoredProcedures.MID_METHOD_READ_LIST_FROM_TYPE.Read(_dba, METHOD_TYPE_ID: (int)aMethodType);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetMethodList(eMethodTypeUI aMethodType, int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_READ_LIST_FROM_TYPE_AND_USER.Read(_dba,
                                                                                     METHOD_TYPE_ID: (int)aMethodType,
                                                                                     USER_RID: aUserRID
                                                                                     );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
        public DataTable GetMethodList(string aMethodName, eMethodType aMethodType, int aUserRID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME.Read(_dba,
                                                                                          METHOD_NAME: aMethodName,
                                                                                          METHOD_TYPE_ID: (int)aMethodType,
                                                                                          USER_RID: aUserRID
                                                                                          );
                return dt;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // END MID Track #6336

		public DataTable GetMethodList()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_METHOD_READ_LIST.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
