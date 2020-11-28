using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class EnvironmentData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_APPLICATION_SERVICES_HISTORY_INSERT_def MID_APPLICATION_SERVICES_HISTORY_INSERT = new MID_APPLICATION_SERVICES_HISTORY_INSERT_def();
			public class MID_APPLICATION_SERVICES_HISTORY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_SERVICES_HISTORY_INSERT.SQL"

			    private intParameter SERVICE_EPROCESSES_ID;
			    private datetimeParameter SERVICE_START_DATETIME;
			    private stringParameter SERVICE_VERSION;
			
			    public MID_APPLICATION_SERVICES_HISTORY_INSERT_def()
			    {
			        base.procedureName = "MID_APPLICATION_SERVICES_HISTORY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("APPLICATION_SERVICES_HISTORY");
			        SERVICE_EPROCESSES_ID = new intParameter("@SERVICE_EPROCESSES_ID", base.inputParameterList);
			        SERVICE_START_DATETIME = new datetimeParameter("@SERVICE_START_DATETIME", base.inputParameterList);
			        SERVICE_VERSION = new stringParameter("@SERVICE_VERSION", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SERVICE_EPROCESSES_ID,
			                      DateTime? SERVICE_START_DATETIME,
			                      string SERVICE_VERSION
			                      )
			    {
                    lock (typeof(MID_APPLICATION_SERVICES_HISTORY_INSERT_def))
                    {
                        this.SERVICE_EPROCESSES_ID.SetValue(SERVICE_EPROCESSES_ID);
                        this.SERVICE_START_DATETIME.SetValue(SERVICE_START_DATETIME);
                        this.SERVICE_VERSION.SetValue(SERVICE_VERSION);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_SERVICES_HISTORY_DELETE_def MID_APPLICATION_SERVICES_HISTORY_DELETE = new MID_APPLICATION_SERVICES_HISTORY_DELETE_def();
			public class MID_APPLICATION_SERVICES_HISTORY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_SERVICES_HISTORY_DELETE.SQL"

			    private intParameter SERVICE_EPROCESSES_ID;
			
			    public MID_APPLICATION_SERVICES_HISTORY_DELETE_def()
			    {
			        base.procedureName = "MID_APPLICATION_SERVICES_HISTORY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("APPLICATION_SERVICES_HISTORY");
			        SERVICE_EPROCESSES_ID = new intParameter("@SERVICE_EPROCESSES_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SERVICE_EPROCESSES_ID)
			    {
                    lock (typeof(MID_APPLICATION_SERVICES_HISTORY_DELETE_def))
                    {
                        this.SERVICE_EPROCESSES_ID.SetValue(SERVICE_EPROCESSES_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_SERVICES_HISTORY_READ_ALL_def MID_APPLICATION_SERVICES_HISTORY_READ_ALL = new MID_APPLICATION_SERVICES_HISTORY_READ_ALL_def();
			public class MID_APPLICATION_SERVICES_HISTORY_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_SERVICES_HISTORY_READ_ALL.SQL"

			
			    public MID_APPLICATION_SERVICES_HISTORY_READ_ALL_def()
			    {
			        base.procedureName = "MID_APPLICATION_SERVICES_HISTORY_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_SERVICES_HISTORY");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_SERVICES_HISTORY_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS_def MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS = new MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS_def();
			public class MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS.SQL"

			    private intParameter SERVICE_EPROCESSES_ID;
			
			    public MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS_def()
			    {
			        base.procedureName = "MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_SERVICES_HISTORY");
			        SERVICE_EPROCESSES_ID = new intParameter("@SERVICE_EPROCESSES_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SERVICE_EPROCESSES_ID)
			    {
                    lock (typeof(MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS_def))
                    {
                        this.SERVICE_EPROCESSES_ID.SetValue(SERVICE_EPROCESSES_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE_def MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE = new MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE_def();
			public class MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE.SQL"

			
			    public MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE_def()
			    {
			        base.procedureName = "MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_SERVICES_HISTORY");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
            public static MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE_def MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE = new MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE_def();
            public class MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE.SQL"


                public MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE_def()
                {
                    base.procedureName = "MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_DB_CONVERSION_QUEUE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            public static MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED_def MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED = new MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED_def();
            public class MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED.SQL"


                public MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED_def()
                {
                    base.procedureName = "MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_DB_CONVERSION_QUEUE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION_def MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION = new MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION_def();
            public class MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION.SQL"

                private stringParameter CC_FUNCTION_NAME;

                public MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION_def()
                {
                    base.procedureName = "MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("APPLICATION_DB_CONVERSION_QUEUE");
                    CC_FUNCTION_NAME = new stringParameter("@CC_FUNCTION_NAME", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, string CC_FUNCTION_NAME)
                {
                    lock (typeof(MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION_def))
                    {
                        this.CC_FUNCTION_NAME.SetValue(CC_FUNCTION_NAME);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            //End TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
