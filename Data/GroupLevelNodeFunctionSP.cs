using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class GroupLevelNodeFunction
    {
        protected static class StoredProcedures
        {
            public static MID_GROUP_LEVEL_NODE_FUNCTION_READ_def MID_GROUP_LEVEL_NODE_FUNCTION_READ = new MID_GROUP_LEVEL_NODE_FUNCTION_READ_def();
            public class MID_GROUP_LEVEL_NODE_FUNCTION_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_NODE_FUNCTION_READ.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;

                public MID_GROUP_LEVEL_NODE_FUNCTION_READ_def()
                {
                    base.procedureName = "MID_GROUP_LEVEL_NODE_FUNCTION_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GROUP_LEVEL_NODE_FUNCTION");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? METHOD_RID,
                                      int? SGL_RID
                                      )
                {
                    lock (typeof(MID_GROUP_LEVEL_NODE_FUNCTION_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_GROUP_LEVEL_NODE_FUNCTION_INSERT_def MID_GROUP_LEVEL_NODE_FUNCTION_INSERT = new MID_GROUP_LEVEL_NODE_FUNCTION_INSERT_def();
			public class MID_GROUP_LEVEL_NODE_FUNCTION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_NODE_FUNCTION_INSERT.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter HN_RID;
			    private charParameter APPLY_MIN_MAXES_IND;
			    private intParameter MIN_MAXES_INHERIT_TYPE;
			
			    public MID_GROUP_LEVEL_NODE_FUNCTION_INSERT_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_NODE_FUNCTION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("GROUP_LEVEL_NODE_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        APPLY_MIN_MAXES_IND = new charParameter("@APPLY_MIN_MAXES_IND", base.inputParameterList);
			        MIN_MAXES_INHERIT_TYPE = new intParameter("@MIN_MAXES_INHERIT_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? HN_RID,
			                      char? APPLY_MIN_MAXES_IND,
			                      int? MIN_MAXES_INHERIT_TYPE
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_NODE_FUNCTION_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.APPLY_MIN_MAXES_IND.SetValue(APPLY_MIN_MAXES_IND);
                        this.MIN_MAXES_INHERIT_TYPE.SetValue(MIN_MAXES_INHERIT_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_def MID_GROUP_LEVEL_NODE_FUNCTION_DELETE = new MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_def();
			public class MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_NODE_FUNCTION_DELETE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			
			    public MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_NODE_FUNCTION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GROUP_LEVEL_NODE_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }
}
