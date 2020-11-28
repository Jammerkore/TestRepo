using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class WorkflowBaseData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_WORKFLOW_STEP_OTSPLAN_READ_def MID_WORKFLOW_STEP_OTSPLAN_READ = new MID_WORKFLOW_STEP_OTSPLAN_READ_def();
            public class MID_WORKFLOW_STEP_OTSPLAN_READ_def : baseStoredProcedure
            {
                private intParameter WORKFLOW_RID;

                public MID_WORKFLOW_STEP_OTSPLAN_READ_def()
                {
                    base.procedureName = "MID_WORKFLOW_STEP_OTSPLAN_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("WORKFLOW_STEP_OTSPLAN");
                    WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
                {
                    lock (typeof(MID_WORKFLOW_STEP_OTSPLAN_READ_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_WORKFLOW_STEP_OTSPLAN_DELETE_def MID_WORKFLOW_STEP_OTSPLAN_DELETE = new MID_WORKFLOW_STEP_OTSPLAN_DELETE_def();
            public class MID_WORKFLOW_STEP_OTSPLAN_DELETE_def : baseStoredProcedure
            {
                private intParameter WORKFLOW_RID;
                private intParameter STEP_NUMBER;

                public MID_WORKFLOW_STEP_OTSPLAN_DELETE_def()
                {
                    base.procedureName = "MID_WORKFLOW_STEP_OTSPLAN_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("WORKFLOW_STEP_OTSPLAN");
                    WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
                    STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
                }

           
                public int Delete(DatabaseAccess _dba, 
                                  int? WORKFLOW_RID,
                                  int? STEP_NUMBER
                                  )
                {
                    lock (typeof(MID_WORKFLOW_STEP_OTSPLAN_DELETE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);

                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_APPLICATION_USER_READ_NAME_def MID_APPLICATION_USER_READ_NAME = new MID_APPLICATION_USER_READ_NAME_def();
			public class MID_APPLICATION_USER_READ_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_NAME.SQL"

			    private intParameter USER_RID;
			
			    public MID_APPLICATION_USER_READ_NAME_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_NAME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_CHANGE_HISTORY_DELETE_def MID_WORKFLOW_CHANGE_HISTORY_DELETE = new MID_WORKFLOW_CHANGE_HISTORY_DELETE_def();
			public class MID_WORKFLOW_CHANGE_HISTORY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_CHANGE_HISTORY_DELETE.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_CHANGE_HISTORY_DELETE_def()
			    {
			        base.procedureName = "MID_WORKFLOW_CHANGE_HISTORY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("WORKFLOW_CHANGE_HISTORY");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_CHANGE_HISTORY_DELETE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_CHANGE_HISTORY_INSERT_def MID_WORKFLOW_CHANGE_HISTORY_INSERT = new MID_WORKFLOW_CHANGE_HISTORY_INSERT_def();
			public class MID_WORKFLOW_CHANGE_HISTORY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_CHANGE_HISTORY_INSERT.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter USER_RID;
                private datetimeParameter CHANGE_DATE;
                private stringParameter WORKFLOW_COMMENT;
                private stringParameter WINDOWS_USER;
                private stringParameter WINDOWS_MACHINE;
                private stringParameter WINDOWS_REMOTE_MACHINE;
			
			    public MID_WORKFLOW_CHANGE_HISTORY_INSERT_def()
			    {
			        base.procedureName = "MID_WORKFLOW_CHANGE_HISTORY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("WORKFLOW_CHANGE_HISTORY");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        CHANGE_DATE = new datetimeParameter("@CHANGE_DATE", base.inputParameterList);
			        WORKFLOW_COMMENT = new stringParameter("@WORKFLOW_COMMENT", base.inputParameterList);
                    WINDOWS_USER = new stringParameter("@WINDOWS_USER", base.inputParameterList);
                    WINDOWS_MACHINE = new stringParameter("@WINDOWS_MACHINE", base.inputParameterList);
                    WINDOWS_REMOTE_MACHINE = new stringParameter("@WINDOWS_REMOTE_MACHINE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? USER_RID,
			                      DateTime? CHANGE_DATE,
			                      string WORKFLOW_COMMENT,
                                  string WINDOWS_USER,
                                  string WINDOWS_MACHINE,
                                  string WINDOWS_REMOTE_MACHINE
			                      )
			    {
                    lock (typeof(MID_WORKFLOW_CHANGE_HISTORY_INSERT_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.CHANGE_DATE.SetValue(CHANGE_DATE);
                        this.WORKFLOW_COMMENT.SetValue(WORKFLOW_COMMENT);
                    	this.WINDOWS_USER.SetValue(WINDOWS_USER);
                    	this.WINDOWS_MACHINE.SetValue(WINDOWS_MACHINE);
                    	this.WINDOWS_REMOTE_MACHINE.SetValue(WINDOWS_REMOTE_MACHINE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_DELETE_def MID_WORKFLOW_DELETE = new MID_WORKFLOW_DELETE_def();
			public class MID_WORKFLOW_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_DELETE.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_DELETE_def()
			    {
			        base.procedureName = "MID_WORKFLOW_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_DELETE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_METHOD_def MID_WORKFLOW_READ_FROM_METHOD = new MID_WORKFLOW_READ_FROM_METHOD_def();
			public class MID_WORKFLOW_READ_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_METHOD.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_WORKFLOW_READ_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_OTS_METHOD_def MID_WORKFLOW_READ_FROM_OTS_METHOD = new MID_WORKFLOW_READ_FROM_OTS_METHOD_def();
			public class MID_WORKFLOW_READ_FROM_OTS_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_OTS_METHOD.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_WORKFLOW_READ_FROM_OTS_METHOD_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_OTS_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_OTS_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME_def MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME = new MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME_def();
			public class MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME.SQL"

			    private intParameter USER_RID;
                private stringParameter WORKFLOW_NAME;
			
			    public MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          string WORKFLOW_NAME
			                          )
			    {
                    lock (typeof(MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_NAME_def MID_WORKFLOW_READ_NAME = new MID_WORKFLOW_READ_NAME_def();
			public class MID_WORKFLOW_READ_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_NAME.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_NAME_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_NAME_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_RID_FROM_NAME_def MID_WORKFLOW_READ_RID_FROM_NAME = new MID_WORKFLOW_READ_RID_FROM_NAME_def();
			public class MID_WORKFLOW_READ_RID_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_RID_FROM_NAME.SQL"

                private stringParameter WORKFLOW_NAME;
			
			    public MID_WORKFLOW_READ_RID_FROM_NAME_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_RID_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string WORKFLOW_NAME)
			    {
                    lock (typeof(MID_WORKFLOW_READ_RID_FROM_NAME_def))
                    {
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_USER_def MID_WORKFLOW_READ_USER = new MID_WORKFLOW_READ_USER_def();
			public class MID_WORKFLOW_READ_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_USER.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_USER_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_UPDATE_def MID_WORKFLOW_UPDATE = new MID_WORKFLOW_UPDATE_def();
			public class MID_WORKFLOW_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_UPDATE.SQL"

			    private intParameter WORKFLOW_RID;
                private stringParameter WORKFLOW_NAME;
			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter WORKFLOW_USER_RID;
                private stringParameter WORKFLOW_DESCRIPTION;
			    private intParameter STORE_FILTER_RID;
			
			    public MID_WORKFLOW_UPDATE_def()
			    {
			        base.procedureName = "MID_WORKFLOW_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        WORKFLOW_USER_RID = new intParameter("@WORKFLOW_USER_RID", base.inputParameterList);
			        WORKFLOW_DESCRIPTION = new stringParameter("@WORKFLOW_DESCRIPTION", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      string WORKFLOW_NAME,
			                      int? WORKFLOW_TYPE_ID,
			                      int? WORKFLOW_USER_RID,
			                      string WORKFLOW_DESCRIPTION,
			                      int? STORE_FILTER_RID
			                      )
			    {
                    lock (typeof(MID_WORKFLOW_UPDATE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.WORKFLOW_USER_RID.SetValue(WORKFLOW_USER_RID);
                        this.WORKFLOW_DESCRIPTION.SetValue(WORKFLOW_DESCRIPTION);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_USER_def MID_WORKFLOW_READ_FROM_USER = new MID_WORKFLOW_READ_FROM_USER_def();
			public class MID_WORKFLOW_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_USER.SQL"

			    private intParameter USER_RID;
			
			    public MID_WORKFLOW_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_RID_def MID_WORKFLOW_READ_FROM_RID = new MID_WORKFLOW_READ_FROM_RID_def();
			public class MID_WORKFLOW_READ_FROM_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_RID.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_FROM_RID_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_RID_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_TYPE_def MID_WORKFLOW_READ_FROM_TYPE = new MID_WORKFLOW_READ_FROM_TYPE_def();
			public class MID_WORKFLOW_READ_FROM_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_TYPE.SQL"

			    private intParameter WORKFLOW_TYPE_ID;
			
			    public MID_WORKFLOW_READ_FROM_TYPE_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_TYPE_ID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_TYPE_def))
                    {
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_TYPE_AND_USER_def MID_WORKFLOW_READ_FROM_TYPE_AND_USER = new MID_WORKFLOW_READ_FROM_TYPE_AND_USER_def();
			public class MID_WORKFLOW_READ_FROM_TYPE_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_TYPE_AND_USER.SQL"

			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter USER_RID;
			
			    public MID_WORKFLOW_READ_FROM_TYPE_AND_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_TYPE_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? WORKFLOW_TYPE_ID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_TYPE_AND_USER_def))
                    {
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_SHARED_def MID_WORKFLOW_READ_SHARED = new MID_WORKFLOW_READ_SHARED_def();
			public class MID_WORKFLOW_READ_SHARED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_SHARED.SQL"

			    private intParameter USER_RID;
			
			    public MID_WORKFLOW_READ_SHARED_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_SHARED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_WORKFLOW_READ_SHARED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_SHARED_FROM_OWNER_def MID_WORKFLOW_READ_SHARED_FROM_OWNER = new MID_WORKFLOW_READ_SHARED_FROM_OWNER_def();
			public class MID_WORKFLOW_READ_SHARED_FROM_OWNER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_SHARED_FROM_OWNER.SQL"

			    private intParameter USER_RID;
			    private intParameter OWNER_USER_RID;
			
			    public MID_WORKFLOW_READ_SHARED_FROM_OWNER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_SHARED_FROM_OWNER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? OWNER_USER_RID
			                          )
			    {
                    lock (typeof(MID_WORKFLOW_READ_SHARED_FROM_OWNER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_NAME_def MID_WORKFLOW_READ_FROM_NAME = new MID_WORKFLOW_READ_FROM_NAME_def();
			public class MID_WORKFLOW_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_NAME.SQL"

                private stringParameter WORKFLOW_NAME;
			
			    public MID_WORKFLOW_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string WORKFLOW_NAME)
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_NAME_def))
                    {
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_FROM_NAME_AND_USER_def MID_WORKFLOW_READ_FROM_NAME_AND_USER = new MID_WORKFLOW_READ_FROM_NAME_AND_USER_def();
			public class MID_WORKFLOW_READ_FROM_NAME_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_FROM_NAME_AND_USER.SQL"

                private stringParameter WORKFLOW_NAME;
			    private intParameter WORKFLOW_USER_RID;
			
			    public MID_WORKFLOW_READ_FROM_NAME_AND_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_FROM_NAME_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			        WORKFLOW_USER_RID = new intParameter("@WORKFLOW_USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string WORKFLOW_NAME,
			                          int? WORKFLOW_USER_RID
			                          )
			    {
                    lock (typeof(MID_WORKFLOW_READ_FROM_NAME_AND_USER_def))
                    {
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        this.WORKFLOW_USER_RID.SetValue(WORKFLOW_USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_def MID_WORKFLOW_READ = new MID_WORKFLOW_READ_def();
			public class MID_WORKFLOW_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ALLOCATION_STEP_COMPONENT_INSERT_def MID_ALLOCATION_STEP_COMPONENT_INSERT = new MID_ALLOCATION_STEP_COMPONENT_INSERT_def();
			public class MID_ALLOCATION_STEP_COMPONENT_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ALLOCATION_STEP_COMPONENT_INSERT.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			    private intParameter COMP_TYPE_ID;
			    private intParameter COMP_CRIT_RID;
			
			    public MID_ALLOCATION_STEP_COMPONENT_INSERT_def()
			    {
			        base.procedureName = "MID_ALLOCATION_STEP_COMPONENT_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ALLOCATION_STEP_COMPONENT");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			        COMP_TYPE_ID = new intParameter("@COMP_TYPE_ID", base.inputParameterList);
			        COMP_CRIT_RID = new intParameter("@COMP_CRIT_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER,
			                      int? COMP_TYPE_ID,
			                      int? COMP_CRIT_RID
			                      )
			    {
                    lock (typeof(MID_ALLOCATION_STEP_COMPONENT_INSERT_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        this.COMP_TYPE_ID.SetValue(COMP_TYPE_ID);
                        this.COMP_CRIT_RID.SetValue(COMP_CRIT_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_COMPONENT_CRITERIA_DELETE_def MID_COMPONENT_CRITERIA_DELETE = new MID_COMPONENT_CRITERIA_DELETE_def();
			public class MID_COMPONENT_CRITERIA_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPONENT_CRITERIA_DELETE.SQL"

			    private intParameter COMP_CRIT_RID;
			
			    public MID_COMPONENT_CRITERIA_DELETE_def()
			    {
			        base.procedureName = "MID_COMPONENT_CRITERIA_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("COMPONENT_CRITERIA");
			        COMP_CRIT_RID = new intParameter("@COMP_CRIT_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMP_CRIT_RID)
			    {
                    lock (typeof(MID_COMPONENT_CRITERIA_DELETE_def))
                    {
                        this.COMP_CRIT_RID.SetValue(COMP_CRIT_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_STEP_ALLOCATION_DELETE_def MID_WORKFLOW_STEP_ALLOCATION_DELETE = new MID_WORKFLOW_STEP_ALLOCATION_DELETE_def();
			public class MID_WORKFLOW_STEP_ALLOCATION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_STEP_ALLOCATION_DELETE.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			
			    public MID_WORKFLOW_STEP_ALLOCATION_DELETE_def()
			    {
			        base.procedureName = "MID_WORKFLOW_STEP_ALLOCATION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("WORKFLOW_STEP_ALLOCATION");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER
			                      )
			    {
                    lock (typeof(MID_WORKFLOW_STEP_ALLOCATION_DELETE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_STEP_ALLOCATION_INSERT_def MID_WORKFLOW_STEP_ALLOCATION_INSERT = new MID_WORKFLOW_STEP_ALLOCATION_INSERT_def();
			public class MID_WORKFLOW_STEP_ALLOCATION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_STEP_ALLOCATION_INSERT.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			    private intParameter ACTION_METHOD_TYPE;
			    private intParameter METHOD_RID;
                private charParameter REVIEW_IND;
                private floatParameter PCT_TOLERANCE;
			    private intParameter STORE_FILTER_RID;
			
			    public MID_WORKFLOW_STEP_ALLOCATION_INSERT_def()
			    {
			        base.procedureName = "MID_WORKFLOW_STEP_ALLOCATION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("WORKFLOW_STEP_ALLOCATION");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			        ACTION_METHOD_TYPE = new intParameter("@ACTION_METHOD_TYPE", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        REVIEW_IND = new charParameter("@REVIEW_IND", base.inputParameterList);
			        PCT_TOLERANCE = new floatParameter("@PCT_TOLERANCE", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER,
			                      int? ACTION_METHOD_TYPE,
			                      int? METHOD_RID,
			                      char? REVIEW_IND,
			                      double? PCT_TOLERANCE,
			                      int? STORE_FILTER_RID
			                      )
			    {
                    lock (typeof(MID_WORKFLOW_STEP_ALLOCATION_INSERT_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        this.ACTION_METHOD_TYPE.SetValue(ACTION_METHOD_TYPE);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.REVIEW_IND.SetValue(REVIEW_IND);
                        this.PCT_TOLERANCE.SetValue(PCT_TOLERANCE);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_STEP_ALLOCATION_READ_def MID_WORKFLOW_STEP_ALLOCATION_READ = new MID_WORKFLOW_STEP_ALLOCATION_READ_def();
			public class MID_WORKFLOW_STEP_ALLOCATION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_STEP_ALLOCATION_READ.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_STEP_ALLOCATION_READ_def()
			    {
			        base.procedureName = "MID_WORKFLOW_STEP_ALLOCATION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW_STEP_ALLOCATION");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_STEP_ALLOCATION_READ_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ALLOCATION_STEP_COMPONENT_READ_def MID_ALLOCATION_STEP_COMPONENT_READ = new MID_ALLOCATION_STEP_COMPONENT_READ_def();
			public class MID_ALLOCATION_STEP_COMPONENT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ALLOCATION_STEP_COMPONENT_READ.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_ALLOCATION_STEP_COMPONENT_READ_def()
			    {
			        base.procedureName = "MID_ALLOCATION_STEP_COMPONENT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ALLOCATION_STEP_COMPONENT");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_ALLOCATION_STEP_COMPONENT_READ_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ALLOCATION_STEP_COMPONENT_READ_STEP_def MID_ALLOCATION_STEP_COMPONENT_READ_STEP = new MID_ALLOCATION_STEP_COMPONENT_READ_STEP_def();
			public class MID_ALLOCATION_STEP_COMPONENT_READ_STEP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ALLOCATION_STEP_COMPONENT_READ_STEP.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			
			    public MID_ALLOCATION_STEP_COMPONENT_READ_STEP_def()
			    {
			        base.procedureName = "MID_ALLOCATION_STEP_COMPONENT_READ_STEP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ALLOCATION_STEP_COMPONENT");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? WORKFLOW_RID,
			                          int? STEP_NUMBER
			                          )
			    {
                    lock (typeof(MID_ALLOCATION_STEP_COMPONENT_READ_STEP_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA_def MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA = new MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA_def();
			public class MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			    private intParameter COMP_TYPE_ID;
			    private intParameter COMP_CRIT_RID;
			
			    public MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA_def()
			    {
			        base.procedureName = "MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ALLOCATION_STEP_COMPONENT");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			        COMP_TYPE_ID = new intParameter("@COMP_TYPE_ID", base.inputParameterList);
			        COMP_CRIT_RID = new intParameter("@COMP_CRIT_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER,
			                      int? COMP_TYPE_ID,
			                      int? COMP_CRIT_RID
			                      )
			    {
                    lock (typeof(MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        this.COMP_TYPE_ID.SetValue(COMP_TYPE_ID);
                        this.COMP_CRIT_RID.SetValue(COMP_CRIT_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ALLOCATION_STEP_COMPONENT_DELETE_def MID_ALLOCATION_STEP_COMPONENT_DELETE = new MID_ALLOCATION_STEP_COMPONENT_DELETE_def();
			public class MID_ALLOCATION_STEP_COMPONENT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ALLOCATION_STEP_COMPONENT_DELETE.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			
			    public MID_ALLOCATION_STEP_COMPONENT_DELETE_def()
			    {
			        base.procedureName = "MID_ALLOCATION_STEP_COMPONENT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ALLOCATION_STEP_COMPONENT");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER
			                      )
			    {
                    lock (typeof(MID_ALLOCATION_STEP_COMPONENT_DELETE_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_COMP_CRIT_INSERT_def SP_MID_COMP_CRIT_INSERT = new SP_MID_COMP_CRIT_INSERT_def();
			public class SP_MID_COMP_CRIT_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_COMP_CRIT_INSERT.SQL"

                private stringParameter EXPRESSION;
			    private intParameter SIZE_CODE_RID;
			    private intParameter COLOR_CODE_RID;
                private stringParameter PACK_NAME;
			    private intParameter COMP_CRIT_RID; //Declare Output Parameter

                public SP_MID_COMP_CRIT_INSERT_def()
			    {
			        base.procedureName = "SP_MID_COMP_CRIT_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("COMP_CRIT");
			        EXPRESSION = new stringParameter("@EXPRESSION", base.inputParameterList);
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			        COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
			        PACK_NAME = new stringParameter("@PACK_NAME", base.inputParameterList);
			        COMP_CRIT_RID = new intParameter("@COMP_CRIT_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string EXPRESSION,
			                                  int? SIZE_CODE_RID,
			                                  int? COLOR_CODE_RID,
			                                  string PACK_NAME
			                                  )
			    {
                    lock (typeof(SP_MID_COMP_CRIT_INSERT_def))
                    {
                        this.EXPRESSION.SetValue(EXPRESSION);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.PACK_NAME.SetValue(PACK_NAME);
                        this.COMP_CRIT_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_STEP_OTSPLAN_INSERT_def MID_WORKFLOW_STEP_OTSPLAN_INSERT = new MID_WORKFLOW_STEP_OTSPLAN_INSERT_def();
			public class MID_WORKFLOW_STEP_OTSPLAN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_STEP_OTSPLAN_INSERT.SQL"

			    private intParameter WORKFLOW_RID;
			    private intParameter STEP_NUMBER;
			    private intParameter ACTION_METHOD_TYPE;
			    private intParameter METHOD_RID;
                private charParameter REVIEW_IND;
                private floatParameter PCT_TOLERANCE;
			    private intParameter STORE_FILTER_RID;
			    private intParameter VARIABLE_NUMBER;
                private stringParameter CALC_MODE;
                private charParameter BAL_IND;
			
			    public MID_WORKFLOW_STEP_OTSPLAN_INSERT_def()
			    {
			        base.procedureName = "MID_WORKFLOW_STEP_OTSPLAN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("WORKFLOW_STEP_OTSPLAN");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        STEP_NUMBER = new intParameter("@STEP_NUMBER", base.inputParameterList);
			        ACTION_METHOD_TYPE = new intParameter("@ACTION_METHOD_TYPE", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        REVIEW_IND = new charParameter("@REVIEW_IND", base.inputParameterList);
			        PCT_TOLERANCE = new floatParameter("@PCT_TOLERANCE", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        BAL_IND = new charParameter("@BAL_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? WORKFLOW_RID,
			                      int? STEP_NUMBER,
			                      int? ACTION_METHOD_TYPE,
			                      int? METHOD_RID,
			                      char? REVIEW_IND,
			                      double? PCT_TOLERANCE,
			                      int? STORE_FILTER_RID,
			                      int? VARIABLE_NUMBER,
			                      string CALC_MODE,
			                      char? BAL_IND
			                      )
			    {
                    lock (typeof(MID_WORKFLOW_STEP_OTSPLAN_INSERT_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.STEP_NUMBER.SetValue(STEP_NUMBER);
                        this.ACTION_METHOD_TYPE.SetValue(ACTION_METHOD_TYPE);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.REVIEW_IND.SetValue(REVIEW_IND);
                        this.PCT_TOLERANCE.SetValue(PCT_TOLERANCE);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.BAL_IND.SetValue(BAL_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_WORKFLOW_INSERT_def SP_MID_WORKFLOW_INSERT = new SP_MID_WORKFLOW_INSERT_def();
            public class SP_MID_WORKFLOW_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_WORKFLOW_INSERT.SQL"

                private stringParameter WORKFLOW_NAME;
			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter WORKFLOW_USER_RID;
                private stringParameter WORKFLOW_DESCRIPTION;
			    private intParameter STORE_FILTER_RID;
                private charParameter WORKFLOW_OVERRIDE;
			    private intParameter WORKFLOW_RID; //Declare Output Parameter
			
			    public SP_MID_WORKFLOW_INSERT_def()
			    {
                    base.procedureName = "SP_MID_WORKFLOW_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        WORKFLOW_USER_RID = new intParameter("@WORKFLOW_USER_RID", base.inputParameterList);
			        WORKFLOW_DESCRIPTION = new stringParameter("@WORKFLOW_DESCRIPTION", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        WORKFLOW_OVERRIDE = new charParameter("@WORKFLOW_OVERRIDE", base.inputParameterList);
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string WORKFLOW_NAME,
			                                  int? WORKFLOW_TYPE_ID,
			                                  int? WORKFLOW_USER_RID,
			                                  string WORKFLOW_DESCRIPTION,
			                                  int? STORE_FILTER_RID,
			                                  char? WORKFLOW_OVERRIDE
			                                  )
			    {
                    lock (typeof(SP_MID_WORKFLOW_INSERT_def))
                    {
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.WORKFLOW_USER_RID.SetValue(WORKFLOW_USER_RID);
                        this.WORKFLOW_DESCRIPTION.SetValue(WORKFLOW_DESCRIPTION);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.WORKFLOW_OVERRIDE.SetValue(WORKFLOW_OVERRIDE);
                        this.WORKFLOW_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_COUNT_def MID_WORKFLOW_READ_COUNT = new MID_WORKFLOW_READ_COUNT_def();
			public class MID_WORKFLOW_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_COUNT.SQL"

                private stringParameter WORKFLOW_NAME;
			    private intParameter WORKFLOW_TYPE_ID;
			    private intParameter WORKFLOW_USER_RID;
			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_COUNT_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_NAME = new stringParameter("@WORKFLOW_NAME", base.inputParameterList);
			        WORKFLOW_TYPE_ID = new intParameter("@WORKFLOW_TYPE_ID", base.inputParameterList);
			        WORKFLOW_USER_RID = new intParameter("@WORKFLOW_USER_RID", base.inputParameterList);
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               string WORKFLOW_NAME,
			                               int? WORKFLOW_TYPE_ID,
			                               int? WORKFLOW_USER_RID,
			                               int? WORKFLOW_RID
			                               )
			    {
                    lock (typeof(MID_WORKFLOW_READ_COUNT_def))
                    {
                        this.WORKFLOW_NAME.SetValue(WORKFLOW_NAME);
                        this.WORKFLOW_TYPE_ID.SetValue(WORKFLOW_TYPE_ID);
                        this.WORKFLOW_USER_RID.SetValue(WORKFLOW_USER_RID);
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
