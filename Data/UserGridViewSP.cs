using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class UserGridView : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_USER_CURRENT_GRID_VIEW_DELETE_def MID_USER_CURRENT_GRID_VIEW_DELETE = new MID_USER_CURRENT_GRID_VIEW_DELETE_def();
			public class MID_USER_CURRENT_GRID_VIEW_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_GRID_VIEW_DELETE.SQL"

			    private intParameter USER_RID;
                private intParameter LAYOUT_ID;
			
			    public MID_USER_CURRENT_GRID_VIEW_DELETE_def()
			    {
			        base.procedureName = "MID_USER_CURRENT_GRID_VIEW_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_CURRENT_GRID_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? LAYOUT_ID
			                      )
			    {
                    lock (typeof(MID_USER_CURRENT_GRID_VIEW_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_CURRENT_GRID_VIEW_INSERT_def MID_USER_CURRENT_GRID_VIEW_INSERT = new MID_USER_CURRENT_GRID_VIEW_INSERT_def();
			public class MID_USER_CURRENT_GRID_VIEW_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_GRID_VIEW_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter LAYOUT_ID;
                private intParameter VIEW_RID;
			
			    public MID_USER_CURRENT_GRID_VIEW_INSERT_def()
			    {
			        base.procedureName = "MID_USER_CURRENT_GRID_VIEW_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_CURRENT_GRID_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? LAYOUT_ID,
			                      int? VIEW_RID
			                      )
			    {
                    lock (typeof(MID_USER_CURRENT_GRID_VIEW_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_CURRENT_GRID_VIEW_READ_def MID_USER_CURRENT_GRID_VIEW_READ = new MID_USER_CURRENT_GRID_VIEW_READ_def();
			public class MID_USER_CURRENT_GRID_VIEW_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_GRID_VIEW_READ.SQL"

                private intParameter USER_RID;
                private intParameter LAYOUT_ID;
			
			    public MID_USER_CURRENT_GRID_VIEW_READ_def()
			    {
			        base.procedureName = "MID_USER_CURRENT_GRID_VIEW_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_CURRENT_GRID_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? LAYOUT_ID
			                          )
			    {
                    lock (typeof(MID_USER_CURRENT_GRID_VIEW_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
