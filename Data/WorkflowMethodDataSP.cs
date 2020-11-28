using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class WorkflowMethodData : DataLayer
    {
        protected static class StoredProcedures
        {

            //Begin TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT
            //public static MID_WM_EXP_NODES_STRUCT_READ_def MID_WM_EXP_NODES_STRUCT_READ = new MID_WM_EXP_NODES_STRUCT_READ_def();
            //public class MID_WM_EXP_NODES_STRUCT_READ_def : baseStoredProcedure
            //{

            //    public MID_WM_EXP_NODES_STRUCT_READ_def()
            //    {
            //        base.procedureName = "MID_WM_EXP_NODES_STRUCT_READ";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("WM_EXP_NODES_STRUCT");
            //    }

            //    public DataTable Read(DatabaseAccess _dba)
            //    {
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}
            //End TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT

            public static MID_METHOD_READ_COUNT_def MID_METHOD_READ_COUNT = new MID_METHOD_READ_COUNT_def();
            public class MID_METHOD_READ_COUNT_def : baseStoredProcedure
            {
                private intParameter METHOD_TYPE_ID;
                private intParameter USER_RID;

                public MID_METHOD_READ_COUNT_def()
                {
                    base.procedureName = "MID_METHOD_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("METHOD");
                    METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, 
                                           int? METHOD_TYPE_ID,
                                           int? USER_RID
                                           )
                {
                    lock (typeof(MID_METHOD_READ_COUNT_def))
                    {
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_METHOD_READ_COUNT_FROM_NAME_def MID_METHOD_READ_COUNT_FROM_NAME = new MID_METHOD_READ_COUNT_FROM_NAME_def();
			public class MID_METHOD_READ_COUNT_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_COUNT_FROM_NAME.SQL"

				private intParameter USER_RID;
				private intParameter METHOD_TYPE_ID;
                private stringParameter METHOD_NAME;
				
				public MID_METHOD_READ_COUNT_FROM_NAME_def()
				{
				    base.procedureName = "MID_METHOD_READ_COUNT_FROM_NAME";
				    base.procedureType = storedProcedureTypes.RecordCount;
				    base.tableNames.Add("METHOD");
				    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
				    METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
				    METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
				}

                public int ReadRecordCount(DatabaseAccess _dba, 
                                            int? USER_RID,
				                            int? METHOD_TYPE_ID,
				                            string METHOD_NAME
				                            )
				{
                    lock (typeof(MID_METHOD_READ_COUNT_FROM_NAME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
				}
			}

			public static MID_METHOD_READ_LIST_def MID_METHOD_READ_LIST = new MID_METHOD_READ_LIST_def();
			public class MID_METHOD_READ_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_LIST.SQL"

			
			    public MID_METHOD_READ_LIST_def()
			    {
			        base.procedureName = "MID_METHOD_READ_LIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_METHOD_READ_LIST_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_LIST_FROM_TYPE_def MID_METHOD_READ_LIST_FROM_TYPE = new MID_METHOD_READ_LIST_FROM_TYPE_def();
			public class MID_METHOD_READ_LIST_FROM_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_LIST_FROM_TYPE.SQL"

			    private intParameter METHOD_TYPE_ID;
			
			    public MID_METHOD_READ_LIST_FROM_TYPE_def()
			    {
			        base.procedureName = "MID_METHOD_READ_LIST_FROM_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_TYPE_ID)
			    {
                    lock (typeof(MID_METHOD_READ_LIST_FROM_TYPE_def))
                    {
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_LIST_FROM_TYPE_AND_USER_def MID_METHOD_READ_LIST_FROM_TYPE_AND_USER = new MID_METHOD_READ_LIST_FROM_TYPE_AND_USER_def();
			public class MID_METHOD_READ_LIST_FROM_TYPE_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_LIST_FROM_TYPE_AND_USER.SQL"

			    private intParameter METHOD_TYPE_ID;
			    private intParameter USER_RID;
			
			    public MID_METHOD_READ_LIST_FROM_TYPE_AND_USER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_LIST_FROM_TYPE_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_TYPE_ID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_LIST_FROM_TYPE_AND_USER_def))
                    {
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME_def MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME = new MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME_def();
			public class MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME.SQL"

                private stringParameter METHOD_NAME;
			    private intParameter METHOD_TYPE_ID;
			    private intParameter USER_RID;
			
			    public MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME_def()
			    {
			        base.procedureName = "MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string METHOD_NAME,
			                          int? METHOD_TYPE_ID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_LIST_FROM_TYPE_USER_AND_NAME_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_NAME_def MID_METHOD_READ_NAME = new MID_METHOD_READ_NAME_def();
			public class MID_METHOD_READ_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_NAME.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_METHOD_READ_NAME_def()
			    {
			        base.procedureName = "MID_METHOD_READ_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_READ_NAME_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_TYPE_def MID_METHOD_READ_TYPE = new MID_METHOD_READ_TYPE_def();
			public class MID_METHOD_READ_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_TYPE.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_METHOD_READ_TYPE_def()
			    {
			        base.procedureName = "MID_METHOD_READ_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_READ_TYPE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER_def MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER = new MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER_def();
			public class MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER.SQL"

			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter WORKFLOW_USER_RID;
                private stringParameter WORKFLOW_NAME;
			
			    public MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        WORKFLOW_USER_RID = new intParameter("@WORKFLOW_USER_RID", base.inputParameterList);
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? WORKFLOW_TYPE_ID,
			                               int? WORKFLOW_USER_RID,
			                               string WORKFLOW_NAME
			                               )
			    {
                    lock (typeof(MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER_def))
                    {
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.WORKFLOW_USER_RID.SetValue(WORKFLOW_USER_RID);
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER_def MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER = new MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER_def();
			public class MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER.SQL"

			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter USER_RID;
			
			    public MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? WORKFLOW_TYPE_ID,
			                               int? USER_RID
			                               )
			    {
                    lock (typeof(MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER_def))
                    {
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
