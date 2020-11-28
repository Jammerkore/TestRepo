using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class SecurityAdmin : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_APPLICATION_USER_READ_FROM_NAME_def MID_APPLICATION_USER_READ_FROM_NAME = new MID_APPLICATION_USER_READ_FROM_NAME_def();
            public class MID_APPLICATION_USER_READ_FROM_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_FROM_NAME.SQL"

                private stringParameter USER_NAME;

                public MID_APPLICATION_USER_READ_FROM_NAME_def()
                {
                    base.procedureName = "MID_APPLICATION_USER_READ_FROM_NAME";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_USER");
                    USER_NAME = new stringParameter("@USER_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string USER_NAME)
                {
                    lock (typeof(MID_APPLICATION_USER_READ_FROM_NAME_def))
                    {
                        this.USER_NAME.SetValue(USER_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_USER_SESSION_READ_def MID_USER_SESSION_READ = new MID_USER_SESSION_READ_def();
			public class MID_USER_SESSION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_READ.SQL"

                private intParameter USER_RID;
                private intParameter SESSION_STATUS;
			
			    public MID_USER_SESSION_READ_def()
			    {
			        base.procedureName = "MID_USER_SESSION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_SESSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SESSION_STATUS = new intParameter("@SESSION_STATUS", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? SESSION_STATUS
			                          )
			    {
                    lock (typeof(MID_USER_SESSION_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SESSION_STATUS.SetValue(SESSION_STATUS);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME_def MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME = new MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME_def();
			public class MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME.SQL"

                private stringParameter USER_NAME;
			
			    public MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME_def()
			    {
			        base.procedureName = "MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_SESSION");
			        USER_NAME = new stringParameter("@USER_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string USER_NAME)
			    {
                    lock (typeof(MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME_def))
                    {
                        this.USER_NAME.SetValue(USER_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS_def MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS = new MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS_def();
            public class MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS.SQL"
			
			    public MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS_def()
			    {
                    base.procedureName = "MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SYSTEM_OPTIONS");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_USER_GROUP_DELETE_def SP_MID_USER_GROUP_DELETE = new SP_MID_USER_GROUP_DELETE_def();
            public class SP_MID_USER_GROUP_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_GROUP_DELETE.SQL"

                private intParameter GroupRID;
                private intParameter debug;
			
			    public SP_MID_USER_GROUP_DELETE_def()
			    {
                    base.procedureName = "SP_MID_USER_GROUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_GROUP");
                    GroupRID = new intParameter("@GroupRID", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba,
                                  int? GroupRID
			                      )
			    {
                    lock (typeof(SP_MID_USER_GROUP_DELETE_def))
                    {
                        this.GroupRID.SetValue(GroupRID);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR_def MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR = new MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR_def();
			public class MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR.SQL"

                private intParameter GROUP_RID;
                private charParameter GROUP_ACTIVE_IND;
			
			    public MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        GROUP_ACTIVE_IND = new charParameter("@GROUP_ACTIVE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      char? GROUP_ACTIVE_IND
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.GROUP_ACTIVE_IND.SetValue(GROUP_ACTIVE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_FOR_DELETION_def MID_USER_GROUP_UPDATE_FOR_DELETION = new MID_USER_GROUP_UPDATE_FOR_DELETION_def();
			public class MID_USER_GROUP_UPDATE_FOR_DELETION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_FOR_DELETION.SQL"

                private intParameter GROUP_RID;
                private datetimeParameter GROUP_DELETE_DATETIME;
			
			    public MID_USER_GROUP_UPDATE_FOR_DELETION_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_FOR_DELETION";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        GROUP_DELETE_DATETIME = new datetimeParameter("@GROUP_DELETE_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      DateTime? GROUP_DELETE_DATETIME
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_FOR_DELETION_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.GROUP_DELETE_DATETIME.SetValue(GROUP_DELETE_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_FOR_RECOVERY_def MID_USER_GROUP_UPDATE_FOR_RECOVERY = new MID_USER_GROUP_UPDATE_FOR_RECOVERY_def();
			public class MID_USER_GROUP_UPDATE_FOR_RECOVERY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_FOR_RECOVERY.SQL"

                private intParameter GROUP_RID;
			
			    public MID_USER_GROUP_UPDATE_FOR_RECOVERY_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_FOR_RECOVERY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_FOR_RECOVERY_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_DELETED_USERS_def MID_APPLICATION_USER_READ_DELETED_USERS = new MID_APPLICATION_USER_READ_DELETED_USERS_def();
			public class MID_APPLICATION_USER_READ_DELETED_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_DELETED_USERS.SQL"

			
			    public MID_APPLICATION_USER_READ_DELETED_USERS_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_DELETED_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_DELETED_USERS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_DELETED_GROUPS_def MID_USER_GROUP_READ_DELETED_GROUPS = new MID_USER_GROUP_READ_DELETED_GROUPS_def();
			public class MID_USER_GROUP_READ_DELETED_GROUPS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_DELETED_GROUPS.SQL"

			
			    public MID_USER_GROUP_READ_DELETED_GROUPS_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_DELETED_GROUPS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_USER_GROUP_READ_DELETED_GROUPS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_JOIN_INSERT_def MID_USER_GROUP_JOIN_INSERT = new MID_USER_GROUP_JOIN_INSERT_def();
			public class MID_USER_GROUP_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_JOIN_INSERT.SQL"

                private intParameter GROUP_RID;
                private intParameter USER_RID;
			
			    public MID_USER_GROUP_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_USER_GROUP_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_GROUP_JOIN");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_JOIN_INSERT_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_JOIN_DELETE_def MID_USER_GROUP_JOIN_DELETE = new MID_USER_GROUP_JOIN_DELETE_def();
			public class MID_USER_GROUP_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_JOIN_DELETE.SQL"

                private intParameter GROUP_RID;
                private intParameter USER_RID;
			
			    public MID_USER_GROUP_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_USER_GROUP_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_GROUP_JOIN");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_JOIN_DELETE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_NAMES_def MID_APPLICATION_USER_READ_NAMES = new MID_APPLICATION_USER_READ_NAMES_def();
			public class MID_APPLICATION_USER_READ_NAMES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_NAMES.SQL"

			    public MID_APPLICATION_USER_READ_NAMES_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_NAMES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_NAMES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_APPLICATION_USER_READ_NAME_FROM_RID_def MID_APPLICATION_USER_READ_NAME_FROM_RID = new MID_APPLICATION_USER_READ_NAME_FROM_RID_def();
            public class MID_APPLICATION_USER_READ_NAME_FROM_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_NAME_FROM_RID.SQL"

                private intParameter USER_RID;

                public MID_APPLICATION_USER_READ_NAME_FROM_RID_def()
                {
                    base.procedureName = "MID_APPLICATION_USER_READ_NAME_FROM_RID";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("APPLICATION_USER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_APPLICATION_USER_READ_NAME_FROM_RID_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

			public static MID_APPLICATION_USER_READ_def MID_APPLICATION_USER_READ = new MID_APPLICATION_USER_READ_def();
			public class MID_APPLICATION_USER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ.SQL"

                private intParameter USER_RID;
			
			    public MID_APPLICATION_USER_READ_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE_def MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE = new MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE_def();
			public class MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE.SQL"


                public MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE_def()
			    {
                    base.procedureName = "MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_ALL_FOR_MAINTENANCE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_ALL_ACTIVE_def MID_APPLICATION_USER_READ_ALL_ACTIVE = new MID_APPLICATION_USER_READ_ALL_ACTIVE_def();
			public class MID_APPLICATION_USER_READ_ALL_ACTIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_ALL_ACTIVE.SQL"

			
			    public MID_APPLICATION_USER_READ_ALL_ACTIVE_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_ALL_ACTIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_ALL_ACTIVE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP_def MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP = new MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP_def();
			public class MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP.SQL"

			
			    public MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_ACTIVE_IN_GROUP_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_FROM_GROUP_def MID_APPLICATION_USER_READ_FROM_GROUP = new MID_APPLICATION_USER_READ_FROM_GROUP_def();
			public class MID_APPLICATION_USER_READ_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_FROM_GROUP.SQL"

                private intParameter GROUP_RID;
			
			    public MID_APPLICATION_USER_READ_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_FROM_GROUP_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_ALL_def MID_APPLICATION_USER_READ_ALL = new MID_APPLICATION_USER_READ_ALL_def();
			public class MID_APPLICATION_USER_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_ALL.SQL"

			
			    public MID_APPLICATION_USER_READ_ALL_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_READ_FROM_RID_def MID_APPLICATION_USER_READ_FROM_RID = new MID_APPLICATION_USER_READ_FROM_RID_def();
			public class MID_APPLICATION_USER_READ_FROM_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_READ_FROM_RID.SQL"

                private intParameter USER_RID;
			
			    public MID_APPLICATION_USER_READ_FROM_RID_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_READ_FROM_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_APPLICATION_USER_READ_FROM_RID_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME_def MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME = new MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME_def();
			public class MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME.SQL"

                private stringParameter USER_NAME;
                private stringParameter USER_PASSWORD;
			
			    public MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_NAME = new stringParameter("@USER_NAME", base.inputParameterList);
			        USER_PASSWORD = new stringParameter("@USER_PASSWORD", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      string USER_NAME,
			                      string USER_PASSWORD
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME_def))
                    {
                        this.USER_NAME.SetValue(USER_NAME);
                        this.USER_PASSWORD.SetValue(USER_PASSWORD);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_USER_INSERT_def SP_MID_USER_INSERT = new SP_MID_USER_INSERT_def();
            public class SP_MID_USER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_INSERT.SQL"

                private stringParameter USER_NAME;
                private stringParameter USER_PASSWORD;
                private stringParameter USER_FULLNAME;
                private stringParameter USER_DESCRIPTION;
                private charParameter USER_ACTIVE_IND;
                private intParameter USER_RID; //Declare Output Parameter
			
			    public SP_MID_USER_INSERT_def()
			    {
                    base.procedureName = "SP_MID_USER_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("APPLICATION_USER");
			        USER_NAME = new stringParameter("@USER_NAME", base.inputParameterList);
			        USER_PASSWORD = new stringParameter("@USER_PASSWORD", base.inputParameterList);
			        USER_FULLNAME = new stringParameter("@USER_FULLNAME", base.inputParameterList);
			        USER_DESCRIPTION = new stringParameter("@USER_DESCRIPTION", base.inputParameterList);
			        USER_ACTIVE_IND = new charParameter("@USER_ACTIVE_IND", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string USER_NAME,
			                                  string USER_PASSWORD,
			                                  string USER_FULLNAME,
			                                  string USER_DESCRIPTION,
			                                  char? USER_ACTIVE_IND
			                                  )
			    {
                    lock (typeof(SP_MID_USER_INSERT_def))
                    {
                        this.USER_NAME.SetValue(USER_NAME);
                        this.USER_PASSWORD.SetValue(USER_PASSWORD);
                        this.USER_FULLNAME.SetValue(USER_FULLNAME);
                        this.USER_DESCRIPTION.SetValue(USER_DESCRIPTION);
                        this.USER_ACTIVE_IND.SetValue(USER_ACTIVE_IND);
                        this.USER_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static SP_MID_USER_SESSION_INSERT_def SP_MID_USER_SESSION_INSERT = new SP_MID_USER_SESSION_INSERT_def();
            public class SP_MID_USER_SESSION_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_SESSION_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter SESSION_STATUS;
                private datetimeParameter LOGIN_DATETIME;
                private stringParameter COMPUTER_NAME;
                private intParameter SESSION_RID; //Declare Output Parameter
			
			    public SP_MID_USER_SESSION_INSERT_def()
			    {
                    base.procedureName = "SP_MID_USER_SESSION_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("USER_SESSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SESSION_STATUS = new intParameter("@SESSION_STATUS", base.inputParameterList);
			        LOGIN_DATETIME = new datetimeParameter("@LOGIN_DATETIME", base.inputParameterList);
			        COMPUTER_NAME = new stringParameter("@COMPUTER_NAME", base.inputParameterList);
			        SESSION_RID = new intParameter("@SESSION_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? USER_RID,
			                                  int? SESSION_STATUS,
			                                  DateTime? LOGIN_DATETIME,
			                                  string COMPUTER_NAME
			                                  )
			    {
                    lock (typeof(SP_MID_USER_SESSION_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SESSION_STATUS.SetValue(SESSION_STATUS);
                        this.LOGIN_DATETIME.SetValue(LOGIN_DATETIME);
                        this.COMPUTER_NAME.SetValue(COMPUTER_NAME);
                        this.SESSION_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_USER_SESSION_UPDATE_STATUS_def MID_USER_SESSION_UPDATE_STATUS = new MID_USER_SESSION_UPDATE_STATUS_def();
			public class MID_USER_SESSION_UPDATE_STATUS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_UPDATE_STATUS.SQL"

                private intParameter SESSION_RID;
                private intParameter SESSION_STATUS;
			
			    public MID_USER_SESSION_UPDATE_STATUS_def()
			    {
			        base.procedureName = "MID_USER_SESSION_UPDATE_STATUS";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_SESSION");
			        SESSION_RID = new intParameter("@SESSION_RID", base.inputParameterList);
			        SESSION_STATUS = new intParameter("@SESSION_STATUS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SESSION_RID,
			                      int? SESSION_STATUS
			                      )
			    {
                    lock (typeof(MID_USER_SESSION_UPDATE_STATUS_def))
                    {
                        this.SESSION_RID.SetValue(SESSION_RID);
                        this.SESSION_STATUS.SetValue(SESSION_STATUS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_SESSION_UPDATE_FOR_CLOSE_def MID_USER_SESSION_UPDATE_FOR_CLOSE = new MID_USER_SESSION_UPDATE_FOR_CLOSE_def();
			public class MID_USER_SESSION_UPDATE_FOR_CLOSE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_UPDATE_FOR_CLOSE.SQL"

                private intParameter SESSION_RID;
                private intParameter SESSION_STATUS;
                private datetimeParameter LOGOUT_DATETIME;
			
			    public MID_USER_SESSION_UPDATE_FOR_CLOSE_def()
			    {
			        base.procedureName = "MID_USER_SESSION_UPDATE_FOR_CLOSE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_SESSION");
			        SESSION_RID = new intParameter("@SESSION_RID", base.inputParameterList);
			        SESSION_STATUS = new intParameter("@SESSION_STATUS", base.inputParameterList);
			        LOGOUT_DATETIME = new datetimeParameter("@LOGOUT_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SESSION_RID,
			                      int? SESSION_STATUS,
			                      DateTime? LOGOUT_DATETIME
			                      )
			    {
                    lock (typeof(MID_USER_SESSION_UPDATE_FOR_CLOSE_def))
                    {
                        this.SESSION_RID.SetValue(SESSION_RID);
                        this.SESSION_STATUS.SetValue(SESSION_STATUS);
                        this.LOGOUT_DATETIME.SetValue(LOGOUT_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE_def MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE = new MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE_def();
			public class MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE.SQL"

                private datetimeParameter LOGOUT_DATETIME;
			
			    public MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE_def()
			    {
			        base.procedureName = "MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_SESSION");
			        LOGOUT_DATETIME = new datetimeParameter("@LOGOUT_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, DateTime? LOGOUT_DATETIME)
			    {
                    lock (typeof(MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE_def))
                    {
                        this.LOGOUT_DATETIME.SetValue(LOGOUT_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_SESSION_DELETE_FROM_USER_def MID_USER_SESSION_DELETE_FROM_USER = new MID_USER_SESSION_DELETE_FROM_USER_def();
			public class MID_USER_SESSION_DELETE_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_SESSION_DELETE_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_SESSION_DELETE_FROM_USER_def()
			    {
			        base.procedureName = "MID_USER_SESSION_DELETE_FROM_USER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_SESSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_SESSION_DELETE_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static SP_MID_GROUP_INSERT_def SP_MID_GROUP_INSERT = new SP_MID_GROUP_INSERT_def();
			public class SP_MID_GROUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GROUP_INSERT.SQL"

                private stringParameter GROUP_NAME;
                private stringParameter GROUP_DESCRIPTION;
                private charParameter GROUP_ACTIVE_IND;
                private intParameter GROUP_RID; //Declare Output Parameter

                public SP_MID_GROUP_INSERT_def()
			    {
                    base.procedureName = "SP_MID_GROUP_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("GROUP");
			        GROUP_NAME = new stringParameter("@GROUP_NAME", base.inputParameterList);
			        GROUP_DESCRIPTION = new stringParameter("@GROUP_DESCRIPTION", base.inputParameterList);
			        GROUP_ACTIVE_IND = new charParameter("@GROUP_ACTIVE_IND", base.inputParameterList);
			        GROUP_RID = new intParameter("@GROUP_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string GROUP_NAME,
			                                  string GROUP_DESCRIPTION,
			                                  char? GROUP_ACTIVE_IND
			                                  )
			    {
                    lock (typeof(SP_MID_GROUP_INSERT_def))
                    {
                        this.GROUP_NAME.SetValue(GROUP_NAME);
                        this.GROUP_DESCRIPTION.SetValue(GROUP_DESCRIPTION);
                        this.GROUP_ACTIVE_IND.SetValue(GROUP_ACTIVE_IND);
                        this.GROUP_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_DESCRIPTION_def MID_APPLICATION_USER_UPDATE_DESCRIPTION = new MID_APPLICATION_USER_UPDATE_DESCRIPTION_def();
			public class MID_APPLICATION_USER_UPDATE_DESCRIPTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_DESCRIPTION.SQL"

                private intParameter USER_RID;
                private stringParameter USER_DESCRIPTION;
			
			    public MID_APPLICATION_USER_UPDATE_DESCRIPTION_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_DESCRIPTION";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_DESCRIPTION = new stringParameter("@USER_DESCRIPTION", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string USER_DESCRIPTION
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_DESCRIPTION_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_DESCRIPTION.SetValue(USER_DESCRIPTION);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_NAME_def MID_APPLICATION_USER_UPDATE_NAME = new MID_APPLICATION_USER_UPDATE_NAME_def();
			public class MID_APPLICATION_USER_UPDATE_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_NAME.SQL"

                private intParameter USER_RID;
                private stringParameter USER_NAME;
			
			    public MID_APPLICATION_USER_UPDATE_NAME_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_NAME = new stringParameter("@USER_NAME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string USER_NAME
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_NAME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_NAME.SetValue(USER_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_PWD_def MID_APPLICATION_USER_UPDATE_PWD = new MID_APPLICATION_USER_UPDATE_PWD_def();
			public class MID_APPLICATION_USER_UPDATE_PWD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_PWD.SQL"

                private intParameter USER_RID;
                private stringParameter USER_PASSWORD;
			
			    public MID_APPLICATION_USER_UPDATE_PWD_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_PWD";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_PASSWORD = new stringParameter("@USER_PASSWORD", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string USER_PASSWORD
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_PWD_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_PASSWORD.SetValue(USER_PASSWORD);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_FULL_NAME_def MID_APPLICATION_USER_UPDATE_FULL_NAME = new MID_APPLICATION_USER_UPDATE_FULL_NAME_def();
			public class MID_APPLICATION_USER_UPDATE_FULL_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_FULL_NAME.SQL"

                private intParameter USER_RID;
                private stringParameter USER_FULLNAME;
			
			    public MID_APPLICATION_USER_UPDATE_FULL_NAME_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_FULL_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_FULLNAME = new stringParameter("@USER_FULLNAME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string USER_FULLNAME
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_FULL_NAME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_FULLNAME.SetValue(USER_FULLNAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_ACTIVE_IND_def MID_APPLICATION_USER_UPDATE_ACTIVE_IND = new MID_APPLICATION_USER_UPDATE_ACTIVE_IND_def();
			public class MID_APPLICATION_USER_UPDATE_ACTIVE_IND_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_ACTIVE_IND.SQL"

                private intParameter USER_RID;
                private charParameter USER_ACTIVE_IND;
			
			    public MID_APPLICATION_USER_UPDATE_ACTIVE_IND_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_ACTIVE_IND";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_ACTIVE_IND = new charParameter("@USER_ACTIVE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      char? USER_ACTIVE_IND
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_ACTIVE_IND_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_ACTIVE_IND.SetValue(USER_ACTIVE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_NAME_def MID_USER_GROUP_UPDATE_NAME = new MID_USER_GROUP_UPDATE_NAME_def();
			public class MID_USER_GROUP_UPDATE_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_NAME.SQL"

                private intParameter GROUP_RID;
                private stringParameter GROUP_NAME;
			
			    public MID_USER_GROUP_UPDATE_NAME_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        GROUP_NAME = new stringParameter("@GROUP_NAME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      string GROUP_NAME
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_NAME_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.GROUP_NAME.SetValue(GROUP_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_DESCRIPTION_def MID_USER_GROUP_UPDATE_DESCRIPTION = new MID_USER_GROUP_UPDATE_DESCRIPTION_def();
			public class MID_USER_GROUP_UPDATE_DESCRIPTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_DESCRIPTION.SQL"

                private intParameter GROUP_RID;
                private stringParameter GROUP_DESCRIPTION;
			
			    public MID_USER_GROUP_UPDATE_DESCRIPTION_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_DESCRIPTION";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        GROUP_DESCRIPTION = new stringParameter("@GROUP_DESCRIPTION", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      string GROUP_DESCRIPTION
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_DESCRIPTION_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.GROUP_DESCRIPTION.SetValue(GROUP_DESCRIPTION);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_UPDATE_ACTIVE_IND_def MID_USER_GROUP_UPDATE_ACTIVE_IND = new MID_USER_GROUP_UPDATE_ACTIVE_IND_def();
			public class MID_USER_GROUP_UPDATE_ACTIVE_IND_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_UPDATE_ACTIVE_IND.SQL"

                private intParameter GROUP_RID;
                private charParameter GROUP_ACTIVE_IND;
			
			    public MID_USER_GROUP_UPDATE_ACTIVE_IND_def()
			    {
			        base.procedureName = "MID_USER_GROUP_UPDATE_ACTIVE_IND";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        GROUP_ACTIVE_IND = new charParameter("@GROUP_ACTIVE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      char? GROUP_ACTIVE_IND
			                      )
			    {
                    lock (typeof(MID_USER_GROUP_UPDATE_ACTIVE_IND_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.GROUP_ACTIVE_IND.SetValue(GROUP_ACTIVE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_USER_FILTERS_DELETE_def SP_MID_USER_FILTERS_DELETE = new SP_MID_USER_FILTERS_DELETE_def();
            public class SP_MID_USER_FILTERS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_FILTERS_DELETE.SQL"

                private intParameter UserRID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                private intParameter debug;
                private intParameter COMMIT_LIMIT;
                private intParameter RECORDS_DELETED; //Declare Output Parameter
			
			    public SP_MID_USER_FILTERS_DELETE_def()
			    {
			        base.procedureName = "SP_MID_USER_FILTERS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FILTER");    //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                    debug = new intParameter("@debug", base.inputParameterList);
                    COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
                    RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }

                public int Delete(DatabaseAccess _dba, int? UserRID, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_USER_FILTERS_DELETE_def))
                    {
                        this.UserRID.SetValue(UserRID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        this.debug.SetValue(0);
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_USER_METHODS_DELETE_def SP_MID_USER_METHODS_DELETE = new SP_MID_USER_METHODS_DELETE_def();
            public class SP_MID_USER_METHODS_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_METHODS_DELETE.SQL"

                private intParameter UserRID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                private intParameter debug;
                private intParameter COMMIT_LIMIT;
                private intParameter RECORDS_DELETED; //Declare Output Parameter
			
			    public SP_MID_USER_METHODS_DELETE_def()
			    {
			        base.procedureName = "SP_MID_USER_METHODS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD");
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                    debug = new intParameter("@debug", base.inputParameterList);
                    COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
                    RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }

                public int Delete(DatabaseAccess _dba, int? UserRID, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_USER_METHODS_DELETE_def))
                    {
                        this.UserRID.SetValue(UserRID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        this.debug.SetValue(0);
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_USER_WORKFLOWS_DELETE_def SP_MID_USER_WORKFLOWS_DELETE = new SP_MID_USER_WORKFLOWS_DELETE_def();
            public class SP_MID_USER_WORKFLOWS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_WORKFLOWS_DELETE.SQL"

                private intParameter UserRID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                private intParameter debug;
                private intParameter COMMIT_LIMIT;
                private intParameter RECORDS_DELETED; //Declare Output Parameter
			

			    public SP_MID_USER_WORKFLOWS_DELETE_def()
			    {
			        base.procedureName = "SP_MID_USER_WORKFLOWS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("WORKFLOW");
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                    debug = new intParameter("@debug", base.inputParameterList);
                    COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
                    RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }

                public int Delete(DatabaseAccess _dba, int? UserRID, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_USER_WORKFLOWS_DELETE_def))
                    {
                        this.UserRID.SetValue(UserRID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        this.debug.SetValue(0);
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_USER_TASKLISTS_DELETE_def SP_MID_USER_TASKLISTS_DELETE = new SP_MID_USER_TASKLISTS_DELETE_def();
            public class SP_MID_USER_TASKLISTS_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_TASKLISTS_DELETE.SQL"

                private intParameter UserRID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                private intParameter debug;
                private intParameter COMMIT_LIMIT;
                private intParameter RECORDS_DELETED; //Declare Output Parameter
			
			    public SP_MID_USER_TASKLISTS_DELETE_def()
			    {
			        base.procedureName = "SP_MID_USER_TASKLISTS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASKLIST");
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                    debug = new intParameter("@debug", base.inputParameterList);
                    COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
                    RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }

                public int Delete(DatabaseAccess _dba, int? UserRID, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_USER_TASKLISTS_DELETE_def))
                    {
                        this.UserRID.SetValue(UserRID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        this.debug.SetValue(0);
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_USER_DELETE_def SP_MID_USER_DELETE = new SP_MID_USER_DELETE_def();
            public class SP_MID_USER_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_USER_DELETE.SQL"

                private intParameter UserRID;
                private intParameter debug;
			
			    public SP_MID_USER_DELETE_def()
			    {
			        base.procedureName = "SP_MID_USER_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("APPLICATION_USER");
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? UserRID)
			    {
                    lock (typeof(SP_MID_USER_DELETE_def))
                    {
                        this.UserRID.SetValue(UserRID);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_FOR_DELETION_def MID_APPLICATION_USER_UPDATE_FOR_DELETION = new MID_APPLICATION_USER_UPDATE_FOR_DELETION_def();
			public class MID_APPLICATION_USER_UPDATE_FOR_DELETION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_FOR_DELETION.SQL"

                private intParameter USER_RID;
                private datetimeParameter USER_DELETE_DATETIME;
			
			    public MID_APPLICATION_USER_UPDATE_FOR_DELETION_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_FOR_DELETION";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        USER_DELETE_DATETIME = new datetimeParameter("@USER_DELETE_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      DateTime? USER_DELETE_DATETIME
			                      )
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_FOR_DELETION_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.USER_DELETE_DATETIME.SetValue(USER_DELETE_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_APPLICATION_USER_UPDATE_FOR_RECOVERY_def MID_APPLICATION_USER_UPDATE_FOR_RECOVERY = new MID_APPLICATION_USER_UPDATE_FOR_RECOVERY_def();
			public class MID_APPLICATION_USER_UPDATE_FOR_RECOVERY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_USER_UPDATE_FOR_RECOVERY.SQL"

                private intParameter USER_RID;
			
			    public MID_APPLICATION_USER_UPDATE_FOR_RECOVERY_def()
			    {
			        base.procedureName = "MID_APPLICATION_USER_UPDATE_FOR_RECOVERY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("APPLICATION_USER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_APPLICATION_USER_UPDATE_FOR_RECOVERY_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_READ_FROM_OWNER_def MID_USER_ITEM_READ_FROM_OWNER = new MID_USER_ITEM_READ_FROM_OWNER_def();
			public class MID_USER_ITEM_READ_FROM_OWNER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_READ_FROM_OWNER.SQL"

                private intParameter OWNER_USER_RID;
			
			    public MID_USER_ITEM_READ_FROM_OWNER_def()
			    {
			        base.procedureName = "MID_USER_ITEM_READ_FROM_OWNER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ITEM");
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OWNER_USER_RID)
			    {
                    lock (typeof(MID_USER_ITEM_READ_FROM_OWNER_def))
                    {
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_READ_def MID_USER_ITEM_READ = new MID_USER_ITEM_READ_def();
			public class MID_USER_ITEM_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_READ.SQL"

                private intParameter USER_RID;
                private intParameter OWNER_USER_RID;
			
			    public MID_USER_ITEM_READ_def()
			    {
			        base.procedureName = "MID_USER_ITEM_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ITEM");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? OWNER_USER_RID
			                          )
			    {
                    lock (typeof(MID_USER_ITEM_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_INSERT_def MID_USER_ITEM_INSERT = new MID_USER_ITEM_INSERT_def();
			public class MID_USER_ITEM_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter ITEM_TYPE;
                private intParameter ITEM_RID;
                private intParameter OWNER_USER_RID;
			
			    public MID_USER_ITEM_INSERT_def()
			    {
			        base.procedureName = "MID_USER_ITEM_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_ITEM");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        ITEM_RID = new intParameter("@ITEM_RID", base.inputParameterList);
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? ITEM_TYPE,
			                      int? ITEM_RID,
			                      int? OWNER_USER_RID
			                      )
			    {
                    lock (typeof(MID_USER_ITEM_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.ITEM_RID.SetValue(ITEM_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM_def MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM = new MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM_def();
			public class MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM.SQL"

                private intParameter ITEM_TYPE;
                private intParameter ITEM_RID;
			
			    public MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM_def()
			    {
			        base.procedureName = "MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_ITEM");
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        ITEM_RID = new intParameter("@ITEM_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ITEM_TYPE,
			                      int? ITEM_RID
			                      )
			    {
                    lock (typeof(MID_USER_ITEM_DELETE_FROM_TYPE_AND_ITEM_def))
                    {
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.ITEM_RID.SetValue(ITEM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_DELETE_def MID_USER_ITEM_DELETE = new MID_USER_ITEM_DELETE_def();
			public class MID_USER_ITEM_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_DELETE.SQL"

                private intParameter USER_RID;
                private intParameter ITEM_TYPE;
                private intParameter ITEM_RID;
                private intParameter OWNER_USER_RID;
			
			    public MID_USER_ITEM_DELETE_def()
			    {
			        base.procedureName = "MID_USER_ITEM_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_ITEM");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        ITEM_RID = new intParameter("@ITEM_RID", base.inputParameterList);
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? ITEM_TYPE,
			                      int? ITEM_RID,
			                      int? OWNER_USER_RID
			                      )
			    {
                    lock (typeof(MID_USER_ITEM_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.ITEM_RID.SetValue(ITEM_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_READ_ASSIGNED_def MID_USER_ITEM_READ_ASSIGNED = new MID_USER_ITEM_READ_ASSIGNED_def();
			public class MID_USER_ITEM_READ_ASSIGNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_READ_ASSIGNED.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_ITEM_READ_ASSIGNED_def()
			    {
			        base.procedureName = "MID_USER_ITEM_READ_ASSIGNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ITEM");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ITEM_READ_ASSIGNED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ITEM_READ_ASSIGNED_TO_ME_def MID_USER_ITEM_READ_ASSIGNED_TO_ME = new MID_USER_ITEM_READ_ASSIGNED_TO_ME_def();
			public class MID_USER_ITEM_READ_ASSIGNED_TO_ME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ITEM_READ_ASSIGNED_TO_ME.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_ITEM_READ_ASSIGNED_TO_ME_def()
			    {
			        base.procedureName = "MID_USER_ITEM_READ_ASSIGNED_TO_ME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ITEM");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ITEM_READ_ASSIGNED_TO_ME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_def MID_USER_GROUP_READ = new MID_USER_GROUP_READ_def();
			public class MID_USER_GROUP_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ.SQL"

                private intParameter GROUP_RID;
			
			    public MID_USER_GROUP_READ_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_USER_GROUP_READ_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_FROM_NAME_def MID_USER_GROUP_READ_FROM_NAME = new MID_USER_GROUP_READ_FROM_NAME_def();
			public class MID_USER_GROUP_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_FROM_NAME.SQL"

                private stringParameter GROUP_NAME;
			
			    public MID_USER_GROUP_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			        GROUP_NAME = new stringParameter("@GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string GROUP_NAME)
			    {
                    lock (typeof(MID_USER_GROUP_READ_FROM_NAME_def))
                    {
                        this.GROUP_NAME.SetValue(GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_ALL_def MID_USER_GROUP_READ_ALL = new MID_USER_GROUP_READ_ALL_def();
			public class MID_USER_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_ALL.SQL"

			
			    public MID_USER_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_USER_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_ALL_ACTIVE_def MID_USER_GROUP_READ_ALL_ACTIVE = new MID_USER_GROUP_READ_ALL_ACTIVE_def();
			public class MID_USER_GROUP_READ_ALL_ACTIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_ALL_ACTIVE.SQL"

			
			    public MID_USER_GROUP_READ_ALL_ACTIVE_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_ALL_ACTIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_USER_GROUP_READ_ALL_ACTIVE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST_def MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST = new MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST_def();
			public class MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST.SQL"

			
			    public MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_GROUP_READ_FROM_USER_def MID_USER_GROUP_READ_FROM_USER = new MID_USER_GROUP_READ_FROM_USER_def();
			public class MID_USER_GROUP_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_GROUP_READ_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_GROUP_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_USER_GROUP_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_GROUP");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_GROUP_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP.SQL"

                private intParameter GROUP_RID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP.SQL"

                private intParameter GROUP_RID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP_def MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP = new MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP_def();
			public class MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP.SQL"

                private intParameter GROUP_RID;
			
			    public MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_def MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP = new MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_def();
			public class MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP.SQL"

                private intParameter GROUP_RID;
			
			    public MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? GROUP_RID)
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_VERSION_DELETE_def MID_SECURITY_USER_VERSION_DELETE = new MID_SECURITY_USER_VERSION_DELETE_def();
			public class MID_SECURITY_USER_VERSION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_VERSION_DELETE.SQL"

                private intParameter USER_RID;
                private intParameter FV_RID;
			
			    public MID_SECURITY_USER_VERSION_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_VERSION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_VERSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FV_RID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_VERSION_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE_def MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE = new MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE_def();
			public class MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE.SQL"

                private intParameter USER_RID;
                private intParameter FV_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_VERSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FV_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_VERSION_INSERT_def MID_SECURITY_USER_VERSION_INSERT = new MID_SECURITY_USER_VERSION_INSERT_def();
			public class MID_SECURITY_USER_VERSION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_VERSION_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter FV_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_USER_VERSION_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_VERSION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_USER_VERSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FV_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_VERSION_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_DELETE_def MID_SECURITY_USER_FUNCTION_DELETE = new MID_SECURITY_USER_FUNCTION_DELETE_def();
			public class MID_SECURITY_USER_FUNCTION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_DELETE.SQL"

                private intParameter USER_RID;
                private intParameter FUNC_ID;
			
			    public MID_SECURITY_USER_FUNCTION_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FUNC_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION_def MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION = new MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION_def();
			public class MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION.SQL"

                private intParameter USER_RID;
                private intParameter FUNC_ID;
                private intParameter ACTION_ID;
			
			    public MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FUNC_ID,
			                      int? ACTION_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_DELETE_FROM_ACTION_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_INSERT_def MID_SECURITY_USER_FUNCTION_INSERT = new MID_SECURITY_USER_FUNCTION_INSERT_def();
			public class MID_SECURITY_USER_FUNCTION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter FUNC_ID;
                private intParameter ACTION_ID;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_USER_FUNCTION_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? FUNC_ID,
			                      int? ACTION_ID,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT_def MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT = new MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT.SQL"

                private intParameter USER_RID;
                private intParameter HN_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? USER_RID,
			                               int? HN_RID
			                               )
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER_def MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER = new MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_def MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER = new MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE_def MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE = new MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE.SQL"

                private intParameter USER_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP_def MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP = new MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP.SQL"

                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE_def MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE = new MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT.SQL"

                private intParameter USER_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE.SQL"

                private intParameter GROUP_RID;
                private intParameter HN_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? GROUP_RID,
			                          int? HN_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT_def MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT = new MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT.SQL"

                private intParameter GROUP_RID;
                private intParameter HN_RID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? GROUP_RID,
			                               int? HN_RID
			                               )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_READ_def MID_USER_OPTIONS_READ = new MID_USER_OPTIONS_READ_def();
			public class MID_USER_OPTIONS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_READ.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_OPTIONS_READ_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_OPTIONS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_INSERT_MY_HIERARCHY_def MID_USER_OPTIONS_INSERT_MY_HIERARCHY = new MID_USER_OPTIONS_INSERT_MY_HIERARCHY_def();
			public class MID_USER_OPTIONS_INSERT_MY_HIERARCHY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_INSERT_MY_HIERARCHY.SQL"

                private intParameter USER_RID;
                private stringParameter MY_HIERARCHY;
                private stringParameter MY_HIERARCHY_COLOR;
			
			    public MID_USER_OPTIONS_INSERT_MY_HIERARCHY_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_INSERT_MY_HIERARCHY";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_HIERARCHY = new stringParameter("@MY_HIERARCHY", base.inputParameterList);
			        MY_HIERARCHY_COLOR = new stringParameter("@MY_HIERARCHY_COLOR", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_HIERARCHY,
			                      string MY_HIERARCHY_COLOR
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_INSERT_MY_HIERARCHY_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_HIERARCHY.SetValue(MY_HIERARCHY);
                        this.MY_HIERARCHY_COLOR.SetValue(MY_HIERARCHY_COLOR);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_INSERT_def MID_USER_OPTIONS_INSERT = new MID_USER_OPTIONS_INSERT_def();
			public class MID_USER_OPTIONS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_INSERT.SQL"
                // TT#4243 - RMatelic - Add SHOW_SIGNOFF_PROMPT column
                private intParameter USER_RID;
                private stringParameter MY_HIERARCHY;
                private stringParameter MY_HIERARCHY_COLOR;
                private charParameter FORECAST_MONITOR_ACTIVE;
                private stringParameter FORECAST_MONITOR_DIRECTORY;
                private charParameter MODIFY_SALES_MONITOR_ACTIVE;
                private stringParameter MODIFY_SALES_MONITOR_DIRECTORY;
                private intParameter AUDIT_LOGGING_LEVEL;
                private charParameter SHOW_LOGIN;
                private charParameter SHOW_SIGNOFF_PROMPT;
                private charParameter DCFULFILLMENT_MONITOR_ACTIVE;
                private stringParameter DCFULFILLMENT_MONITOR_DIRECTORY;
			
			    public MID_USER_OPTIONS_INSERT_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_HIERARCHY = new stringParameter("@MY_HIERARCHY", base.inputParameterList);
			        MY_HIERARCHY_COLOR = new stringParameter("@MY_HIERARCHY_COLOR", base.inputParameterList);
			        FORECAST_MONITOR_ACTIVE = new charParameter("@FORECAST_MONITOR_ACTIVE", base.inputParameterList);
			        FORECAST_MONITOR_DIRECTORY = new stringParameter("@FORECAST_MONITOR_DIRECTORY", base.inputParameterList);
			        MODIFY_SALES_MONITOR_ACTIVE = new charParameter("@MODIFY_SALES_MONITOR_ACTIVE", base.inputParameterList);
			        MODIFY_SALES_MONITOR_DIRECTORY = new stringParameter("@MODIFY_SALES_MONITOR_DIRECTORY", base.inputParameterList);
			        AUDIT_LOGGING_LEVEL = new intParameter("@AUDIT_LOGGING_LEVEL", base.inputParameterList);
			        SHOW_LOGIN = new charParameter("@SHOW_LOGIN", base.inputParameterList);
                    SHOW_SIGNOFF_PROMPT = new charParameter("@SHOW_SIGNOFF_PROMPT", base.inputParameterList);
                    DCFULFILLMENT_MONITOR_ACTIVE = new charParameter("@DCFULFILLMENT_MONITOR_ACTIVE", base.inputParameterList);
                    DCFULFILLMENT_MONITOR_DIRECTORY = new stringParameter("@DCFULFILLMENT_MONITOR_DIRECTORY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_HIERARCHY,
			                      string MY_HIERARCHY_COLOR,
			                      char? FORECAST_MONITOR_ACTIVE,
			                      string FORECAST_MONITOR_DIRECTORY,
			                      char? MODIFY_SALES_MONITOR_ACTIVE,
			                      string MODIFY_SALES_MONITOR_DIRECTORY,
			                      int? AUDIT_LOGGING_LEVEL,
			                      char? SHOW_LOGIN,
                                  char? SHOW_SIGNOFF_PROMPT,
                                  char? DCFULFILLMENT_MONITOR_ACTIVE,
			                      string DCFULFILLMENT_MONITOR_DIRECTORY
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_HIERARCHY.SetValue(MY_HIERARCHY);
                        this.MY_HIERARCHY_COLOR.SetValue(MY_HIERARCHY_COLOR);
                        this.FORECAST_MONITOR_ACTIVE.SetValue(FORECAST_MONITOR_ACTIVE);
                        this.FORECAST_MONITOR_DIRECTORY.SetValue(FORECAST_MONITOR_DIRECTORY);
                        this.MODIFY_SALES_MONITOR_ACTIVE.SetValue(MODIFY_SALES_MONITOR_ACTIVE);
                        this.MODIFY_SALES_MONITOR_DIRECTORY.SetValue(MODIFY_SALES_MONITOR_DIRECTORY);
                        this.AUDIT_LOGGING_LEVEL.SetValue(AUDIT_LOGGING_LEVEL);
                        this.SHOW_LOGIN.SetValue(SHOW_LOGIN);
                        this.SHOW_SIGNOFF_PROMPT.SetValue(SHOW_SIGNOFF_PROMPT);
                        this.DCFULFILLMENT_MONITOR_ACTIVE.SetValue(DCFULFILLMENT_MONITOR_ACTIVE);
                        this.DCFULFILLMENT_MONITOR_DIRECTORY.SetValue(DCFULFILLMENT_MONITOR_DIRECTORY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS_def MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS = new MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS_def();
			public class MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS.SQL"

                private intParameter USER_RID;
                private stringParameter MY_WORKFLOWMETHODS;
			
			    public MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_WORKFLOWMETHODS = new stringParameter("@MY_WORKFLOWMETHODS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_WORKFLOWMETHODS
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_INSERT_MY_WORKFLOW_METHODS_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_WORKFLOWMETHODS.SetValue(MY_WORKFLOWMETHODS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_INSERT_THEME_def MID_USER_OPTIONS_INSERT_THEME = new MID_USER_OPTIONS_INSERT_THEME_def();
			public class MID_USER_OPTIONS_INSERT_THEME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_INSERT_THEME.SQL"

                private intParameter USER_RID;
                private intParameter THEME_RID;
			
			    public MID_USER_OPTIONS_INSERT_THEME_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_INSERT_THEME";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        THEME_RID = new intParameter("@THEME_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? THEME_RID
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_INSERT_THEME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.THEME_RID.SetValue(THEME_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_UPDATE_MY_HIERARCHY_def MID_USER_OPTIONS_UPDATE_MY_HIERARCHY = new MID_USER_OPTIONS_UPDATE_MY_HIERARCHY_def();
			public class MID_USER_OPTIONS_UPDATE_MY_HIERARCHY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_UPDATE_MY_HIERARCHY.SQL"

                private intParameter USER_RID;
                private stringParameter MY_HIERARCHY;
                private stringParameter MY_HIERARCHY_COLOR;
			
			    public MID_USER_OPTIONS_UPDATE_MY_HIERARCHY_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_UPDATE_MY_HIERARCHY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_HIERARCHY = new stringParameter("@MY_HIERARCHY", base.inputParameterList);
			        MY_HIERARCHY_COLOR = new stringParameter("@MY_HIERARCHY_COLOR", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_HIERARCHY,
			                      string MY_HIERARCHY_COLOR
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_UPDATE_MY_HIERARCHY_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_HIERARCHY.SetValue(MY_HIERARCHY);
                        this.MY_HIERARCHY_COLOR.SetValue(MY_HIERARCHY_COLOR);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_UPDATE_def MID_USER_OPTIONS_UPDATE = new MID_USER_OPTIONS_UPDATE_def();
			public class MID_USER_OPTIONS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_UPDATE.SQL"
                
                private intParameter USER_RID;
                private stringParameter MY_HIERARCHY;
                private stringParameter MY_HIERARCHY_COLOR;
                private charParameter FORECAST_MONITOR_ACTIVE;
                private stringParameter FORECAST_MONITOR_DIRECTORY;
                private charParameter MODIFY_SALES_MONITOR_ACTIVE;
                private stringParameter MODIFY_SALES_MONITOR_DIRECTORY;
                private intParameter AUDIT_LOGGING_LEVEL;
                private charParameter SHOW_LOGIN;
                private charParameter SHOW_SIGNOFF_PROMPT; // TT#4243 - RMatelic - Add SHOW_SIGNOFF_PROMPT column
                private charParameter DCFULFILLMENT_MONITOR_ACTIVE;
                private stringParameter DCFULFILLMENT_MONITOR_DIRECTORY;
			
			    public MID_USER_OPTIONS_UPDATE_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_HIERARCHY = new stringParameter("@MY_HIERARCHY", base.inputParameterList);
			        MY_HIERARCHY_COLOR = new stringParameter("@MY_HIERARCHY_COLOR", base.inputParameterList);
			        FORECAST_MONITOR_ACTIVE = new charParameter("@FORECAST_MONITOR_ACTIVE", base.inputParameterList);
			        FORECAST_MONITOR_DIRECTORY = new stringParameter("@FORECAST_MONITOR_DIRECTORY", base.inputParameterList);
			        MODIFY_SALES_MONITOR_ACTIVE = new charParameter("@MODIFY_SALES_MONITOR_ACTIVE", base.inputParameterList);
			        MODIFY_SALES_MONITOR_DIRECTORY = new stringParameter("@MODIFY_SALES_MONITOR_DIRECTORY", base.inputParameterList);
			        AUDIT_LOGGING_LEVEL = new intParameter("@AUDIT_LOGGING_LEVEL", base.inputParameterList);
			        SHOW_LOGIN = new charParameter("@SHOW_LOGIN", base.inputParameterList);
                    SHOW_SIGNOFF_PROMPT = new charParameter("@SHOW_SIGNOFF_PROMPT", base.inputParameterList);
                    DCFULFILLMENT_MONITOR_ACTIVE = new charParameter("@DCFULFILLMENT_MONITOR_ACTIVE", base.inputParameterList);
                    DCFULFILLMENT_MONITOR_DIRECTORY = new stringParameter("@DCFULFILLMENT_MONITOR_DIRECTORY", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_HIERARCHY,
			                      string MY_HIERARCHY_COLOR,
			                      char? FORECAST_MONITOR_ACTIVE,
			                      string FORECAST_MONITOR_DIRECTORY,
			                      char? MODIFY_SALES_MONITOR_ACTIVE,
			                      string MODIFY_SALES_MONITOR_DIRECTORY,
			                      int? AUDIT_LOGGING_LEVEL,
			                      char? SHOW_LOGIN,
                                  char? SHOW_SIGNOFF_PROMPT,
                                  char? DCFULFILLMENT_MONITOR_ACTIVE,
			                      string DCFULFILLMENT_MONITOR_DIRECTORY
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_UPDATE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_HIERARCHY.SetValue(MY_HIERARCHY);
                        this.MY_HIERARCHY_COLOR.SetValue(MY_HIERARCHY_COLOR);
                        this.FORECAST_MONITOR_ACTIVE.SetValue(FORECAST_MONITOR_ACTIVE);
                        this.FORECAST_MONITOR_DIRECTORY.SetValue(FORECAST_MONITOR_DIRECTORY);
                        this.MODIFY_SALES_MONITOR_ACTIVE.SetValue(MODIFY_SALES_MONITOR_ACTIVE);
                        this.MODIFY_SALES_MONITOR_DIRECTORY.SetValue(MODIFY_SALES_MONITOR_DIRECTORY);
                        this.AUDIT_LOGGING_LEVEL.SetValue(AUDIT_LOGGING_LEVEL);
                        this.SHOW_LOGIN.SetValue(SHOW_LOGIN);
                        this.SHOW_SIGNOFF_PROMPT.SetValue(SHOW_SIGNOFF_PROMPT);
                        this.DCFULFILLMENT_MONITOR_ACTIVE.SetValue(DCFULFILLMENT_MONITOR_ACTIVE);
                        this.DCFULFILLMENT_MONITOR_DIRECTORY.SetValue(DCFULFILLMENT_MONITOR_DIRECTORY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS_def MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS = new MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS_def();
			public class MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS.SQL"

			    private intParameter USER_RID;
                private stringParameter MY_WORKFLOWMETHODS;
			
			    public MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        MY_WORKFLOWMETHODS = new stringParameter("@MY_WORKFLOWMETHODS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string MY_WORKFLOWMETHODS
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_UPDATE_MY_WORKFLOW_METHODS_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.MY_WORKFLOWMETHODS.SetValue(MY_WORKFLOWMETHODS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_UPDATE_SHOW_LOGIN_def MID_USER_OPTIONS_UPDATE_SHOW_LOGIN = new MID_USER_OPTIONS_UPDATE_SHOW_LOGIN_def();
			public class MID_USER_OPTIONS_UPDATE_SHOW_LOGIN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_UPDATE_SHOW_LOGIN.SQL"

                private intParameter USER_RID;
                private charParameter SHOW_LOGIN;
			
			    public MID_USER_OPTIONS_UPDATE_SHOW_LOGIN_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_UPDATE_SHOW_LOGIN";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SHOW_LOGIN = new charParameter("@SHOW_LOGIN", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      char? SHOW_LOGIN
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_UPDATE_SHOW_LOGIN_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SHOW_LOGIN.SetValue(SHOW_LOGIN);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_UPDATE_THEME_def MID_USER_OPTIONS_UPDATE_THEME = new MID_USER_OPTIONS_UPDATE_THEME_def();
			public class MID_USER_OPTIONS_UPDATE_THEME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_UPDATE_THEME.SQL"

                private intParameter USER_RID;
                private intParameter THEME_RID;
			
			    public MID_USER_OPTIONS_UPDATE_THEME_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_UPDATE_THEME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        THEME_RID = new intParameter("@THEME_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? THEME_RID
			                      )
			    {
                    lock (typeof(MID_USER_OPTIONS_UPDATE_THEME_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.THEME_RID.SetValue(THEME_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_OPTIONS_READ_COUNT_def MID_USER_OPTIONS_READ_COUNT = new MID_USER_OPTIONS_READ_COUNT_def();
			public class MID_USER_OPTIONS_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_OPTIONS_READ_COUNT.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_OPTIONS_READ_COUNT_def()
			    {
			        base.procedureName = "MID_USER_OPTIONS_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("USER_OPTIONS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_OPTIONS_READ_COUNT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION_def MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION = new MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION_def();
			public class MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION.SQL"

                private intParameter GROUP_RID;
                private intParameter FUNC_ID;
			
			    public MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? GROUP_RID,
			                          int? FUNC_ID
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


            public static MID_SECURITY_MERCH_ACTIONS_READ_ALL_def MID_SECURITY_MERCH_ACTIONS_READ_ALL = new MID_SECURITY_MERCH_ACTIONS_READ_ALL_def();
            public class MID_SECURITY_MERCH_ACTIONS_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_MERCH_ACTIONS_READ_ALL.SQL"


                public MID_SECURITY_MERCH_ACTIONS_READ_ALL_def()
                {
                    base.procedureName = "MID_SECURITY_MERCH_ACTIONS_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SECURITY_MERCH_ACTIONS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_SECURITY_MERCH_ACTIONS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


			public static MID_SECURITY_VERSION_ACTIONS_READ_ALL_def MID_SECURITY_VERSION_ACTIONS_READ_ALL = new MID_SECURITY_VERSION_ACTIONS_READ_ALL_def();
			public class MID_SECURITY_VERSION_ACTIONS_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_VERSION_ACTIONS_READ_ALL.SQL"

			
			    public MID_SECURITY_VERSION_ACTIONS_READ_ALL_def()
			    {
			        base.procedureName = "MID_SECURITY_VERSION_ACTIONS_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_VERSION_ACTIONS");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SECURITY_VERSION_ACTIONS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION_def MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION = new MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION_def();
			public class MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION.SQL"

                private intParameter FUNC_ID;
			
			    public MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION_def()
			    {
			        base.procedureName = "MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_FUNCTION_ACTIONS");
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FUNC_ID)
			    {
                    lock (typeof(MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION_def))
                    {
                        this.FUNC_ID.SetValue(FUNC_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN_def MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN = new MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN_def();
			public class MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN.SQL"

                private intParameter FUNC_PARENT;
			
			    public MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN_def()
			    {
			        base.procedureName = "MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_FUNCTION_JOIN");
			        FUNC_PARENT = new intParameter("@FUNC_PARENT", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FUNC_PARENT)
			    {
                    lock (typeof(MID_SECURITY_FUNCTION_JOIN_READ_CHILDREN_def))
                    {
                        this.FUNC_PARENT.SetValue(FUNC_PARENT);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_FUNCTION_JOIN_READ_def MID_SECURITY_FUNCTION_JOIN_READ = new MID_SECURITY_FUNCTION_JOIN_READ_def();
			public class MID_SECURITY_FUNCTION_JOIN_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_FUNCTION_JOIN_READ.SQL"

                private intParameter FUNC_ID;
			
			    public MID_SECURITY_FUNCTION_JOIN_READ_def()
			    {
			        base.procedureName = "MID_SECURITY_FUNCTION_JOIN_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_FUNCTION_JOIN");
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FUNC_ID)
			    {
                    lock (typeof(MID_SECURITY_FUNCTION_JOIN_READ_def))
                    {
                        this.FUNC_ID.SetValue(FUNC_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_FUNCTION_JOIN_READ_ALL_def MID_SECURITY_FUNCTION_JOIN_READ_ALL = new MID_SECURITY_FUNCTION_JOIN_READ_ALL_def();
			public class MID_SECURITY_FUNCTION_JOIN_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_FUNCTION_JOIN_READ_ALL.SQL"

			
			    public MID_SECURITY_FUNCTION_JOIN_READ_ALL_def()
			    {
			        base.procedureName = "MID_SECURITY_FUNCTION_JOIN_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_FUNCTION_JOIN");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SECURITY_FUNCTION_JOIN_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS_def MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS = new MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS_def();
			public class MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS.SQL"

                private intParameter FUNC_ID;
                private intParameter USER_RID;
			
			    public MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FUNC_ID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS_def))
                    {
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_READ_def MID_SECURITY_USER_FUNCTION_READ = new MID_SECURITY_USER_FUNCTION_READ_def();
			public class MID_SECURITY_USER_FUNCTION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_READ.SQL"

                private intParameter FUNC_ID;
                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_FUNCTION_READ_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FUNC_ID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_READ_def))
                    {
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_READ_FROM_USER_def MID_SECURITY_USER_FUNCTION_READ_FROM_USER = new MID_SECURITY_USER_FUNCTION_READ_FROM_USER_def();
			public class MID_SECURITY_USER_FUNCTION_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_READ_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_FUNCTION_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION_def MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION = new MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION_def();
			public class MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION.SQL"

                private intParameter USER_RID;
			
			    public MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_FUNCTION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_READ_def MID_SECURITY_GROUP_VERSION_READ = new MID_SECURITY_GROUP_VERSION_READ_def();
			public class MID_SECURITY_GROUP_VERSION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_READ.SQL"

                private intParameter GROUP_RID;
                private intParameter FV_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_VERSION_READ_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? GROUP_RID,
			                          int? FV_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_READ_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_READ_FROM_USER_def MID_SECURITY_GROUP_VERSION_READ_FROM_USER = new MID_SECURITY_GROUP_VERSION_READ_FROM_USER_def();
			public class MID_SECURITY_GROUP_VERSION_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_READ_FROM_USER.SQL"

                private intParameter USER_RID;
                private intParameter FV_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_VERSION_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? FV_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_VERSION_READ_def MID_SECURITY_USER_VERSION_READ = new MID_SECURITY_USER_VERSION_READ_def();
			public class MID_SECURITY_USER_VERSION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_VERSION_READ.SQL"

                private intParameter USER_RID;
                private intParameter FV_RID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_USER_VERSION_READ_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_VERSION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SECURITY_USER_VERSION");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          int? FV_RID,
			                          int? SEC_TYPE
			                          )
			    {
                    lock (typeof(MID_SECURITY_USER_VERSION_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT_def MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT = new MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT.SQL"

                private intParameter GROUP_RID;
                private intParameter HN_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? HN_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_def MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE = new MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE.SQL"

                private intParameter GROUP_RID;
                private intParameter HN_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? HN_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE_def MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE = new MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE_def();
			public class MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE.SQL"

                private intParameter GROUP_RID;
                private intParameter HN_RID;
			
			    public MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_HIERARCHY_NODE");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_INSERT_def MID_SECURITY_GROUP_FUNCTION_INSERT = new MID_SECURITY_GROUP_FUNCTION_INSERT_def();
			public class MID_SECURITY_GROUP_FUNCTION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_INSERT.SQL"

                private intParameter GROUP_RID;
                private intParameter FUNC_ID;
                private intParameter ACTION_ID;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_GROUP_FUNCTION_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FUNC_ID,
			                      int? ACTION_ID,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_INSERT_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_DELETE_def MID_SECURITY_GROUP_FUNCTION_DELETE = new MID_SECURITY_GROUP_FUNCTION_DELETE_def();
			public class MID_SECURITY_GROUP_FUNCTION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_DELETE.SQL"

                private intParameter GROUP_RID;
                private intParameter FUNC_ID;
                private intParameter ACTION_ID;
			
			    public MID_SECURITY_GROUP_FUNCTION_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FUNC_ID,
			                      int? ACTION_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_DELETE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION_def MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION = new MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION_def();
			public class MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION.SQL"

                private intParameter GROUP_RID;
                private intParameter FUNC_ID;
			
			    public MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_FUNCTION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FUNC_ID = new intParameter("@FUNC_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FUNC_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FUNC_ID.SetValue(FUNC_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_INSERT_def MID_SECURITY_GROUP_VERSION_INSERT = new MID_SECURITY_GROUP_VERSION_INSERT_def();
			public class MID_SECURITY_GROUP_VERSION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_INSERT.SQL"

                private intParameter GROUP_RID;
                private intParameter FV_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_GROUP_VERSION_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FV_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_INSERT_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_DELETE_def MID_SECURITY_GROUP_VERSION_DELETE = new MID_SECURITY_GROUP_VERSION_DELETE_def();
			public class MID_SECURITY_GROUP_VERSION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_DELETE.SQL"

                private intParameter GROUP_RID;
                private intParameter FV_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_GROUP_VERSION_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FV_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_DELETE_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION_def MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION = new MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION_def();
			public class MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION.SQL"

                private intParameter GROUP_RID;
                private intParameter FV_RID;
			
			    public MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION_def()
			    {
			        base.procedureName = "MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_GROUP_VERSION");
			        GROUP_RID = new intParameter("@GROUP_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? GROUP_RID,
			                      int? FV_RID
			                      )
			    {
                    lock (typeof(MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION_def))
                    {
                        this.GROUP_RID.SetValue(GROUP_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_INSERT_def MID_SECURITY_USER_HIERARCHY_NODE_INSERT = new MID_SECURITY_USER_HIERARCHY_NODE_INSERT_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter HN_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
                private intParameter SEC_LVL_ID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_INSERT_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			        SEC_LVL_ID = new intParameter("@SEC_LVL_ID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? HN_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE,
			                      int? SEC_LVL_ID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        this.SEC_LVL_ID.SetValue(SEC_LVL_ID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_DELETE_def MID_SECURITY_USER_HIERARCHY_NODE_DELETE = new MID_SECURITY_USER_HIERARCHY_NODE_DELETE_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_DELETE.SQL"

                private intParameter USER_RID;
                private intParameter HN_RID;
                private intParameter ACTION_ID;
                private intParameter SEC_TYPE;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_DELETE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ACTION_ID = new intParameter("@ACTION_ID", base.inputParameterList);
			        SEC_TYPE = new intParameter("@SEC_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? HN_RID,
			                      int? ACTION_ID,
			                      int? SEC_TYPE
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.ACTION_ID.SetValue(ACTION_ID);
                        this.SEC_TYPE.SetValue(SEC_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE_def MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE = new MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE_def();
			public class MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE.SQL"

                private intParameter USER_RID;
                private intParameter HN_RID;
			
			    public MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE_def()
			    {
			        base.procedureName = "MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SECURITY_USER_HIERARCHY_NODE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
