using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class InfragisticsLayoutData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_INFRAGISTICS_LAYOUTS_READ_def MID_INFRAGISTICS_LAYOUTS_READ = new MID_INFRAGISTICS_LAYOUTS_READ_def();
			public class MID_INFRAGISTICS_LAYOUTS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_READ.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			
			    public MID_INFRAGISTICS_LAYOUTS_READ_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? LAYOUT_ID
			                          )
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_READ_COUNT_def MID_INFRAGISTICS_LAYOUTS_READ_COUNT = new MID_INFRAGISTICS_LAYOUTS_READ_COUNT_def();
			public class MID_INFRAGISTICS_LAYOUTS_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_READ_COUNT.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			
			    public MID_INFRAGISTICS_LAYOUTS_READ_COUNT_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? USER_RID,
			                               int? LAYOUT_ID
			                               )
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_READ_COUNT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_INSERT_def MID_INFRAGISTICS_LAYOUTS_INSERT = new MID_INFRAGISTICS_LAYOUTS_INSERT_def();
			public class MID_INFRAGISTICS_LAYOUTS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_INSERT.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			    private intParameter LAYOUT_SIZE;
			    private byteArrayParameter LAYOUT_CONTENT;
			
			    public MID_INFRAGISTICS_LAYOUTS_INSERT_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        LAYOUT_SIZE = new intParameter("@LAYOUT_SIZE", base.inputParameterList);
			        LAYOUT_CONTENT = new byteArrayParameter("@LAYOUT_CONTENT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? LAYOUT_ID,
			                      int? LAYOUT_SIZE,
			                      byte[] LAYOUT_CONTENT
			                      )
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.LAYOUT_SIZE.SetValue(LAYOUT_SIZE);
                        this.LAYOUT_CONTENT.SetValue(LAYOUT_CONTENT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_UPDATE_def MID_INFRAGISTICS_LAYOUTS_UPDATE = new MID_INFRAGISTICS_LAYOUTS_UPDATE_def();
			public class MID_INFRAGISTICS_LAYOUTS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_UPDATE.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			    private intParameter LAYOUT_SIZE;
			    private byteArrayParameter LAYOUT_CONTENT;
			
			    public MID_INFRAGISTICS_LAYOUTS_UPDATE_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        LAYOUT_SIZE = new intParameter("@LAYOUT_SIZE", base.inputParameterList);
			        LAYOUT_CONTENT = new byteArrayParameter("@LAYOUT_CONTENT", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? LAYOUT_ID,
			                      int? LAYOUT_SIZE,
			                      byte[] LAYOUT_CONTENT
			                      )
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_UPDATE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.LAYOUT_SIZE.SetValue(LAYOUT_SIZE);
                        this.LAYOUT_CONTENT.SetValue(LAYOUT_CONTENT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_DELETE_def MID_INFRAGISTICS_LAYOUTS_DELETE = new MID_INFRAGISTICS_LAYOUTS_DELETE_def();
			public class MID_INFRAGISTICS_LAYOUTS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_DELETE.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			
			    public MID_INFRAGISTICS_LAYOUTS_DELETE_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? LAYOUT_ID
			                      )
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT_def MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT = new MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT_def();
			public class MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT.SQL"

			    private intParameter LAYOUT_ID;
			
			    public MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? LAYOUT_ID)
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT_def))
                    {
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER_def MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER = new MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER_def();
			public class MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER.SQL"

			    private intParameter USER_RID;
			
			    public MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER_def MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER = new MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER_def();
			public class MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER.SQL"

			    private intParameter USER_RID;
			
			    public MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER_def()
			    {
			        base.procedureName = "MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("INFRAGISTICS_LAYOUTS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
