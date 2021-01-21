using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class MethodBaseData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_METHOD_READ_TYPE_ID_def MID_METHOD_READ_TYPE_ID = new MID_METHOD_READ_TYPE_ID_def();
            public class MID_METHOD_READ_TYPE_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_TYPE_ID.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_READ_TYPE_ID_def()
                {
                    base.procedureName = "MID_METHOD_READ_TYPE_ID";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("METHOD");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_READ_TYPE_ID_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

			public static MID_METHOD_READ_def MID_METHOD_READ = new MID_METHOD_READ_def();
			public class MID_METHOD_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_READ_def()
			    {
			        base.procedureName = "MID_METHOD_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_NAME_def MID_METHOD_READ_FROM_NAME = new MID_METHOD_READ_FROM_NAME_def();
			public class MID_METHOD_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_NAME.SQL"

                private stringParameter METHOD_NAME;
			
			    public MID_METHOD_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string METHOD_NAME)
			    {
                    lock (typeof(MID_METHOD_READ_FROM_NAME_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_NAME_AND_TYPE_def MID_METHOD_READ_FROM_NAME_AND_TYPE = new MID_METHOD_READ_FROM_NAME_AND_TYPE_def();
			public class MID_METHOD_READ_FROM_NAME_AND_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_NAME_AND_TYPE.SQL"

                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE_ID;
			
			    public MID_METHOD_READ_FROM_NAME_AND_TYPE_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_NAME_AND_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string METHOD_NAME,
			                          int? METHOD_TYPE_ID
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_FROM_NAME_AND_TYPE_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_TYPE_def MID_METHOD_READ_FROM_TYPE = new MID_METHOD_READ_FROM_TYPE_def();
			public class MID_METHOD_READ_FROM_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_TYPE.SQL"

                private intParameter METHOD_TYPE_ID;
			
			    public MID_METHOD_READ_FROM_TYPE_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_TYPE_ID)
			    {
                    lock (typeof(MID_METHOD_READ_FROM_TYPE_def))
                    {
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_TYPE_AND_USER_def MID_METHOD_READ_FROM_TYPE_AND_USER = new MID_METHOD_READ_FROM_TYPE_AND_USER_def();
			public class MID_METHOD_READ_FROM_TYPE_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_TYPE_AND_USER.SQL"

                private intParameter METHOD_TYPE_ID;
                private intParameter USER_RID;
                private charParameter VIRTUAL_IND;
			
			    public MID_METHOD_READ_FROM_TYPE_AND_USER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_TYPE_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_TYPE_ID,
			                          int? USER_RID,
			                          char? VIRTUAL_IND
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_FROM_TYPE_AND_USER_def))
                    {
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_SHARED_def MID_METHOD_READ_SHARED = new MID_METHOD_READ_SHARED_def();
			public class MID_METHOD_READ_SHARED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_SHARED.SQL"

                private intParameter USER_RID;
                private charParameter VIRTUAL_IND;
			
			    public MID_METHOD_READ_SHARED_def()
			    {
			        base.procedureName = "MID_METHOD_READ_SHARED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          char? VIRTUAL_IND
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_SHARED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_SHARED_FROM_OWNER_def MID_METHOD_READ_SHARED_FROM_OWNER = new MID_METHOD_READ_SHARED_FROM_OWNER_def();
			public class MID_METHOD_READ_SHARED_FROM_OWNER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_SHARED_FROM_OWNER.SQL"

                private intParameter USER_RID;
                private charParameter VIRTUAL_IND;
                private intParameter OWNER_USER_RID;
			
			    public MID_METHOD_READ_SHARED_FROM_OWNER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_SHARED_FROM_OWNER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
			        OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          char? VIRTUAL_IND,
			                          int? OWNER_USER_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_SHARED_FROM_OWNER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER_def MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER = new MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER_def();
			public class MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_ALL_def MID_METHOD_READ_ALL = new MID_METHOD_READ_ALL_def();
			public class MID_METHOD_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_ALL.SQL"

			
			    public MID_METHOD_READ_ALL_def()
			    {
			        base.procedureName = "MID_METHOD_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_METHOD_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_METHOD_INSERT_def SP_MID_METHOD_INSERT = new SP_MID_METHOD_INSERT_def();
            public class SP_MID_METHOD_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_METHOD_INSERT.SQL"

                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE_ID;
                private intParameter PROFILE_TYPE_ID;
                private intParameter USER_RID;
                private stringParameter METHOD_DESCRIPTION;
                private intParameter SG_RID;
                private charParameter VIRTUAL_IND;
                private intParameter METHOD_STATUS;
                private intParameter CUSTOM_OLL_RID;
                private intParameter METHOD_RID; //Declare Output Parameter
                private charParameter TEMPLATE_IND;

                public SP_MID_METHOD_INSERT_def()
			    {
			        base.procedureName = "SP_MID_METHOD_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        PROFILE_TYPE_ID = new intParameter("@PROFILE_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        METHOD_DESCRIPTION = new stringParameter("@METHOD_DESCRIPTION", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    TEMPLATE_IND = new charParameter("@TEMPLATE_IND", base.inputParameterList);
                    METHOD_STATUS = new intParameter("@METHOD_STATUS", base.inputParameterList);
			        CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string METHOD_NAME,
			                                  int? METHOD_TYPE_ID,
			                                  int? PROFILE_TYPE_ID,
			                                  int? USER_RID,
			                                  string METHOD_DESCRIPTION,
			                                  int? SG_RID,
			                                  char? VIRTUAL_IND,
			                                  int? METHOD_STATUS,
			                                  int? CUSTOM_OLL_RID,
                                              char? TEMPLATE_IND
                                              )
			    {
                    lock (typeof(SP_MID_METHOD_INSERT_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.PROFILE_TYPE_ID.SetValue(PROFILE_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.METHOD_DESCRIPTION.SetValue(METHOD_DESCRIPTION);
                        this.SG_RID.SetValue(SG_RID);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.METHOD_STATUS.SetValue(METHOD_STATUS);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        this.TEMPLATE_IND.SetValue(TEMPLATE_IND);
                        this.METHOD_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_COUNT_FOR_WORKFLOW_def MID_METHOD_READ_COUNT_FOR_WORKFLOW = new MID_METHOD_READ_COUNT_FOR_WORKFLOW_def();
			public class MID_METHOD_READ_COUNT_FOR_WORKFLOW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_COUNT_FOR_WORKFLOW.SQL"

                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE_ID;
                private intParameter USER_RID;
                private intParameter METHOD_RID;
			
			    public MID_METHOD_READ_COUNT_FOR_WORKFLOW_def()
			    {
			        base.procedureName = "MID_METHOD_READ_COUNT_FOR_WORKFLOW";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               string METHOD_NAME,
			                               int? METHOD_TYPE_ID,
			                               int? USER_RID,
			                               int? METHOD_RID
			                               )
			    {
                    lock (typeof(MID_METHOD_READ_COUNT_FOR_WORKFLOW_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

            public static SP_MID_METHOD_DELETE_def SP_MID_METHOD_DELETE = new SP_MID_METHOD_DELETE_def();
            public class SP_MID_METHOD_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_METHOD_DELETE.SQL"

                private intParameter METHOD_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
			
			    public SP_MID_METHOD_DELETE_def()
			    {
			        base.procedureName = "SP_MID_METHOD_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(SP_MID_METHOD_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_UPDATE_def MID_METHOD_UPDATE = new MID_METHOD_UPDATE_def();
			public class MID_METHOD_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_UPDATE.SQL"

                private intParameter METHOD_RID;
                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE_ID;
                private intParameter PROFILE_TYPE_ID;
                private intParameter USER_RID;
                private stringParameter METHOD_DESCRIPTION;
                private intParameter SG_RID;
                private charParameter VIRTUAL_IND;
                private intParameter METHOD_STATUS;
                private intParameter CUSTOM_OLL_RID;
                private charParameter TEMPLATE_IND;

                public MID_METHOD_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        METHOD_TYPE_ID = new intParameter("@METHOD_TYPE_ID", base.inputParameterList);
			        PROFILE_TYPE_ID = new intParameter("@PROFILE_TYPE_ID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        METHOD_DESCRIPTION = new stringParameter("@METHOD_DESCRIPTION", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
			        METHOD_STATUS = new intParameter("@METHOD_STATUS", base.inputParameterList);
			        CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
                    TEMPLATE_IND = new charParameter("@TEMPLATE_IND", base.inputParameterList);
                }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      string METHOD_NAME,
			                      int? METHOD_TYPE_ID,
			                      int? PROFILE_TYPE_ID,
			                      int? USER_RID,
			                      string METHOD_DESCRIPTION,
			                      int? SG_RID,
			                      char? VIRTUAL_IND,
			                      int? METHOD_STATUS,
			                      int? CUSTOM_OLL_RID,
                                  char? TEMPLATE_IND
                                  )
			    {
                    lock (typeof(MID_METHOD_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE_ID.SetValue(METHOD_TYPE_ID);
                        this.PROFILE_TYPE_ID.SetValue(PROFILE_TYPE_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.METHOD_DESCRIPTION.SetValue(METHOD_DESCRIPTION);
                        this.SG_RID.SetValue(SG_RID);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.METHOD_STATUS.SetValue(METHOD_STATUS);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        this.TEMPLATE_IND.SetValue(TEMPLATE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_FROM_NAME_AND_USER_def MID_METHOD_READ_FROM_NAME_AND_USER = new MID_METHOD_READ_FROM_NAME_AND_USER_def();
			public class MID_METHOD_READ_FROM_NAME_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_FROM_NAME_AND_USER.SQL"

                private stringParameter METHOD_NAME;
                private intParameter USER_RID;
			
			    public MID_METHOD_READ_FROM_NAME_AND_USER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_FROM_NAME_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string METHOD_NAME,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_READ_FROM_NAME_AND_USER_def))
                    {
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_UPDATE_CUSTOM_OLL_def MID_METHOD_UPDATE_CUSTOM_OLL = new MID_METHOD_UPDATE_CUSTOM_OLL_def();
			public class MID_METHOD_UPDATE_CUSTOM_OLL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_UPDATE_CUSTOM_OLL.SQL"

                private intParameter METHOD_RID;
                private intParameter CUSTOM_OLL_RID;
			
			    public MID_METHOD_UPDATE_CUSTOM_OLL_def()
			    {
			        base.procedureName = "MID_METHOD_UPDATE_CUSTOM_OLL";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? CUSTOM_OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_UPDATE_CUSTOM_OLL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_CHANGE_HISTORY_INSERT_def MID_METHOD_CHANGE_HISTORY_INSERT = new MID_METHOD_CHANGE_HISTORY_INSERT_def();
			public class MID_METHOD_CHANGE_HISTORY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_CHANGE_HISTORY_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter USER_RID;
                private datetimeParameter CHANGE_DATE;
                private stringParameter METHOD_COMMENT;
                private stringParameter WINDOWS_USER;
                private stringParameter WINDOWS_MACHINE;
                private stringParameter WINDOWS_REMOTE_MACHINE;
			
			    public MID_METHOD_CHANGE_HISTORY_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_CHANGE_HISTORY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_CHANGE_HISTORY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        CHANGE_DATE = new datetimeParameter("@CHANGE_DATE", base.inputParameterList);
			        METHOD_COMMENT = new stringParameter("@METHOD_COMMENT", base.inputParameterList);
                    WINDOWS_USER = new stringParameter("@WINDOWS_USER", base.inputParameterList);
                    WINDOWS_MACHINE = new stringParameter("@WINDOWS_MACHINE", base.inputParameterList);
                    WINDOWS_REMOTE_MACHINE = new stringParameter("@WINDOWS_REMOTE_MACHINE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? USER_RID,
			                      DateTime? CHANGE_DATE,
			                      string METHOD_COMMENT,
                                  string WINDOWS_USER,
                                  string WINDOWS_MACHINE,
                                  string WINDOWS_REMOTE_MACHINE
			                      )
			    {
                    lock (typeof(MID_METHOD_CHANGE_HISTORY_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.CHANGE_DATE.SetValue(CHANGE_DATE);
                        this.METHOD_COMMENT.SetValue(METHOD_COMMENT);
                    	this.WINDOWS_USER.SetValue(WINDOWS_USER);
                    	this.WINDOWS_MACHINE.SetValue(WINDOWS_MACHINE);
                    	this.WINDOWS_REMOTE_MACHINE.SetValue(WINDOWS_REMOTE_MACHINE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_BASIS_ALLOCATION_READ_def MID_METHOD_SIZE_BASIS_ALLOCATION_READ = new MID_METHOD_SIZE_BASIS_ALLOCATION_READ_def();
			public class MID_METHOD_SIZE_BASIS_ALLOCATION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_BASIS_ALLOCATION_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_BASIS_ALLOCATION_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_BASIS_ALLOCATION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_BASIS_ALLOCATION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_BASIS_ALLOCATION_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SIZE_BASIS_UPD_INS_def SP_MID_MTH_SIZE_BASIS_UPD_INS = new SP_MID_MTH_SIZE_BASIS_UPD_INS_def();
			public class SP_MID_MTH_SIZE_BASIS_UPD_INS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SIZE_BASIS_UPD_INS.SQL"

                private intParameter METHODRID;
                private intParameter BASISHDRRID;
                private intParameter BASISCLRRID;
                private intParameter RULE;
                private intParameter STOREFILTERRID;
                private intParameter SIZEGROUPRID;
                private intParameter HEADERCOMPONENT;
                private intParameter RULEQUANTITY;
                private intParameter SIZECONSTRAINTRID;
                private intParameter SIZECURVEGROUPRID;
                private intParameter GENCURVE_HCG_RID;
                private intParameter GENCURVE_HN_RID;
                private intParameter GENCURVE_PH_RID;
                private intParameter GENCURVE_PHL_SEQUENCE;
                private charParameter GENCURVE_COLOR_IND;
                private intParameter GENCURVE_MERCH_TYPE;
                private intParameter GENCONSTRAINT_HCG_RID;
                private intParameter GENCONSTRAINT_HN_RID;
                private intParameter GENCONSTRAINT_PH_RID;
                private intParameter GENCONSTRAINT_PHL_SEQUENCE;
                private charParameter GENCONSTRAINT_COLOR_IND;
                private intParameter GENCONSTRAINT_MERCH_TYPE;
                private charParameter USE_DEFAULT_CURVE_IND;
                private intParameter GENCURVE_NSCCD_RID;
                private charParameter APPLY_RULES_ONLY_IND;
                private charParameter INCLUDE_RESERVE_IND;

                public SP_MID_MTH_SIZE_BASIS_UPD_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_SIZE_BASIS_UPD_INS";
			        base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_SIZE_BASIS_ALLOCATION");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        BASISHDRRID = new intParameter("@BASISHDRRID", base.inputParameterList);
			        BASISCLRRID = new intParameter("@BASISCLRRID", base.inputParameterList);
			        RULE = new intParameter("@RULE", base.inputParameterList);
			        STOREFILTERRID = new intParameter("@STOREFILTERRID", base.inputParameterList);
			        SIZEGROUPRID = new intParameter("@SIZEGROUPRID", base.inputParameterList);
			        HEADERCOMPONENT = new intParameter("@HEADERCOMPONENT", base.inputParameterList);
			        RULEQUANTITY = new intParameter("@RULEQUANTITY", base.inputParameterList);
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        SIZECURVEGROUPRID = new intParameter("@SIZECURVEGROUPRID", base.inputParameterList);
			        GENCURVE_HCG_RID = new intParameter("@GENCURVE_HCG_RID", base.inputParameterList);
			        GENCURVE_HN_RID = new intParameter("@GENCURVE_HN_RID", base.inputParameterList);
			        GENCURVE_PH_RID = new intParameter("@GENCURVE_PH_RID", base.inputParameterList);
			        GENCURVE_PHL_SEQUENCE = new intParameter("@GENCURVE_PHL_SEQUENCE", base.inputParameterList);
			        GENCURVE_COLOR_IND = new charParameter("@GENCURVE_COLOR_IND", base.inputParameterList);
			        GENCURVE_MERCH_TYPE = new intParameter("@GENCURVE_MERCH_TYPE", base.inputParameterList);
			        GENCONSTRAINT_HCG_RID = new intParameter("@GENCONSTRAINT_HCG_RID", base.inputParameterList);
			        GENCONSTRAINT_HN_RID = new intParameter("@GENCONSTRAINT_HN_RID", base.inputParameterList);
			        GENCONSTRAINT_PH_RID = new intParameter("@GENCONSTRAINT_PH_RID", base.inputParameterList);
			        GENCONSTRAINT_PHL_SEQUENCE = new intParameter("@GENCONSTRAINT_PHL_SEQUENCE", base.inputParameterList);
			        GENCONSTRAINT_COLOR_IND = new charParameter("@GENCONSTRAINT_COLOR_IND", base.inputParameterList);
			        GENCONSTRAINT_MERCH_TYPE = new intParameter("@GENCONSTRAINT_MERCH_TYPE", base.inputParameterList);
			        USE_DEFAULT_CURVE_IND = new charParameter("@USE_DEFAULT_CURVE_IND", base.inputParameterList);
			        GENCURVE_NSCCD_RID = new intParameter("@GENCURVE_NSCCD_RID", base.inputParameterList);
			        APPLY_RULES_ONLY_IND = new charParameter("@APPLY_RULES_ONLY_IND", base.inputParameterList);
                    INCLUDE_RESERVE_IND = new charParameter("@INCLUDE_RESERVE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHODRID,
			                      int? BASISHDRRID,
			                      int? BASISCLRRID,
			                      int? RULE,
			                      int? STOREFILTERRID,
			                      int? SIZEGROUPRID,
			                      int? HEADERCOMPONENT,
			                      int? RULEQUANTITY,
			                      int? SIZECONSTRAINTRID,
			                      int? SIZECURVEGROUPRID,
			                      int? GENCURVE_HCG_RID,
			                      int? GENCURVE_HN_RID,
			                      int? GENCURVE_PH_RID,
			                      int? GENCURVE_PHL_SEQUENCE,
			                      char? GENCURVE_COLOR_IND,
			                      int? GENCURVE_MERCH_TYPE,
			                      int? GENCONSTRAINT_HCG_RID,
			                      int? GENCONSTRAINT_HN_RID,
			                      int? GENCONSTRAINT_PH_RID,
			                      int? GENCONSTRAINT_PHL_SEQUENCE,
			                      char? GENCONSTRAINT_COLOR_IND,
			                      int? GENCONSTRAINT_MERCH_TYPE,
			                      char? USE_DEFAULT_CURVE_IND,
			                      int? GENCURVE_NSCCD_RID,
			                      char? APPLY_RULES_ONLY_IND,
                                  char? INCLUDE_RESERVE_IND
			                      )
			    {
                    lock (typeof(SP_MID_MTH_SIZE_BASIS_UPD_INS_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.BASISHDRRID.SetValue(BASISHDRRID);
                        this.BASISCLRRID.SetValue(BASISCLRRID);
                        this.RULE.SetValue(RULE);
                        this.STOREFILTERRID.SetValue(STOREFILTERRID);
                        this.SIZEGROUPRID.SetValue(SIZEGROUPRID);
                        this.HEADERCOMPONENT.SetValue(HEADERCOMPONENT);
                        this.RULEQUANTITY.SetValue(RULEQUANTITY);
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.SIZECURVEGROUPRID.SetValue(SIZECURVEGROUPRID);
                        this.GENCURVE_HCG_RID.SetValue(GENCURVE_HCG_RID);
                        this.GENCURVE_HN_RID.SetValue(GENCURVE_HN_RID);
                        this.GENCURVE_PH_RID.SetValue(GENCURVE_PH_RID);
                        this.GENCURVE_PHL_SEQUENCE.SetValue(GENCURVE_PHL_SEQUENCE);
                        this.GENCURVE_COLOR_IND.SetValue(GENCURVE_COLOR_IND);
                        this.GENCURVE_MERCH_TYPE.SetValue(GENCURVE_MERCH_TYPE);
                        this.GENCONSTRAINT_HCG_RID.SetValue(GENCONSTRAINT_HCG_RID);
                        this.GENCONSTRAINT_HN_RID.SetValue(GENCONSTRAINT_HN_RID);
                        this.GENCONSTRAINT_PH_RID.SetValue(GENCONSTRAINT_PH_RID);
                        this.GENCONSTRAINT_PHL_SEQUENCE.SetValue(GENCONSTRAINT_PHL_SEQUENCE);
                        this.GENCONSTRAINT_COLOR_IND.SetValue(GENCONSTRAINT_COLOR_IND);
                        this.GENCONSTRAINT_MERCH_TYPE.SetValue(GENCONSTRAINT_MERCH_TYPE);
                        this.USE_DEFAULT_CURVE_IND.SetValue(USE_DEFAULT_CURVE_IND);
                        this.GENCURVE_NSCCD_RID.SetValue(GENCURVE_NSCCD_RID);
                        this.APPLY_RULES_ONLY_IND.SetValue(APPLY_RULES_ONLY_IND);
                        this.INCLUDE_RESERVE_IND.SetValue(INCLUDE_RESERVE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ_def MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ = new MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ_def();
			public class MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ.SQL"

			    public intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_BASIS_SUBSTITUTES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT_def MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT = new MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT_def();
			public class MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SIZE_TYPE_RID;
                private intParameter SUBSTITUTE_RID;
                private intParameter SIZE_TYPE;
			
			    public MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_SIZE_BASIS_SUBSTITUTES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SIZE_TYPE_RID = new intParameter("@SIZE_TYPE_RID", base.inputParameterList);
			        SUBSTITUTE_RID = new intParameter("@SUBSTITUTE_RID", base.inputParameterList);
			        SIZE_TYPE = new intParameter("@SIZE_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SIZE_TYPE_RID,
			                      int? SUBSTITUTE_RID,
			                      int? SIZE_TYPE
			                      )
			    {
                    lock (typeof(MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SIZE_TYPE_RID.SetValue(SIZE_TYPE_RID);
                        this.SUBSTITUTE_RID.SetValue(SUBSTITUTE_RID);
                        this.SIZE_TYPE.SetValue(SIZE_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE_def MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE = new MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE_def();
			public class MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_SIZE_BASIS_SUBSTITUTES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SIZE_BASIS_DEL_def SP_MID_MTH_SIZE_BASIS_DEL = new SP_MID_MTH_SIZE_BASIS_DEL_def();
            public class SP_MID_MTH_SIZE_BASIS_DEL_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SIZE_BASIS_DEL.SQL"

                private intParameter METHODRID;
			
			    public SP_MID_MTH_SIZE_BASIS_DEL_def()
			    {
                    base.procedureName = "SP_MID_MTH_SIZE_BASIS_DEL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("MTH_SIZE_BASIS");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHODRID)
			    {
                    lock (typeof(SP_MID_MTH_SIZE_BASIS_DEL_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_READ_def MID_METHOD_BLD_PACKS_READ = new MID_METHOD_BLD_PACKS_READ_def();
			public class MID_METHOD_BLD_PACKS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_BLD_PACKS_READ_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_BLD_PACKS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_BPC_SELECT_READ_def MID_METHOD_BLD_PACKS_BPC_SELECT_READ = new MID_METHOD_BLD_PACKS_BPC_SELECT_READ_def();
			public class MID_METHOD_BLD_PACKS_BPC_SELECT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_BPC_SELECT_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_BLD_PACKS_BPC_SELECT_READ_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_BPC_SELECT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_BLD_PACKS_BPC_SELECT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_BPC_SELECT_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BUILD_PACK_CRITERIA_READ_FROM_NAME_def MID_BUILD_PACK_CRITERIA_READ_FROM_NAME = new MID_BUILD_PACK_CRITERIA_READ_FROM_NAME_def();
			public class MID_BUILD_PACK_CRITERIA_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_READ_FROM_NAME.SQL"

                private stringParameter BPC_NAME;
			
			    public MID_BUILD_PACK_CRITERIA_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_BUILD_PACK_CRITERIA_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BUILD_PACK_CRITERIA");
			        BPC_NAME = new stringParameter("@BPC_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string BPC_NAME)
			    {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_READ_FROM_NAME_def))
                    {
                        this.BPC_NAME.SetValue(BPC_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BUILD_PACK_CRITERIA_READ_def MID_BUILD_PACK_CRITERIA_READ = new MID_BUILD_PACK_CRITERIA_READ_def();
			public class MID_BUILD_PACK_CRITERIA_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_READ.SQL"

                private intParameter BPC_RID;
			
			    public MID_BUILD_PACK_CRITERIA_READ_def()
			    {
			        base.procedureName = "MID_BUILD_PACK_CRITERIA_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BUILD_PACK_CRITERIA");
			        BPC_RID = new intParameter("@BPC_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? BPC_RID)
			    {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_READ_def))
                    {
                        this.BPC_RID.SetValue(BPC_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_READ_def MID_METHOD_BLD_PACKS_COMBO_READ = new MID_METHOD_BLD_PACKS_COMBO_READ_def();
			public class MID_METHOD_BLD_PACKS_COMBO_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_BLD_PACKS_COMBO_READ_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_INSERT_def MID_METHOD_BLD_PACKS_INSERT = new MID_METHOD_BLD_PACKS_INSERT_def();
			public class MID_METHOD_BLD_PACKS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BPC_RID;
                private intParameter PATTERN_COMP_MIN;
                private intParameter PATTERN_SIZE_MULTIPLE;
                private intParameter SIZE_GROUP_RID;
                private intParameter SIZE_CURVE_GROUP_RID;
                private floatParameter RESERVE_TOTAL;
                private charParameter RESERVE_TOTAL_PERCENT_IND;
                private floatParameter RESERVE_BULK;
                private charParameter RESERVE_BULK_PERCENT_IND;
                private floatParameter RESERVE_PACKS;
                private charParameter RESERVE_PACKS_PERCENT_IND;
                private floatParameter AVG_PACK_DEV_TOLERANCE;
                private floatParameter MAX_PACK_NEED_TOLERANCE;
                private charParameter DEPLETE_RESERVE_SELECTED;
                private charParameter INCREASE_BUY_SELECTED;
                private floatParameter INCREASE_BUY_PCT;
                private charParameter REMOVE_BULK_IND;
			
			    public MID_METHOD_BLD_PACKS_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_BLD_PACKS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BPC_RID = new intParameter("@BPC_RID", base.inputParameterList);
			        PATTERN_COMP_MIN = new intParameter("@PATTERN_COMP_MIN", base.inputParameterList);
			        PATTERN_SIZE_MULTIPLE = new intParameter("@PATTERN_SIZE_MULTIPLE", base.inputParameterList);
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			        RESERVE_TOTAL = new floatParameter("@RESERVE_TOTAL", base.inputParameterList);
			        RESERVE_TOTAL_PERCENT_IND = new charParameter("@RESERVE_TOTAL_PERCENT_IND", base.inputParameterList);
			        RESERVE_BULK = new floatParameter("@RESERVE_BULK", base.inputParameterList);
			        RESERVE_BULK_PERCENT_IND = new charParameter("@RESERVE_BULK_PERCENT_IND", base.inputParameterList);
			        RESERVE_PACKS = new floatParameter("@RESERVE_PACKS", base.inputParameterList);
			        RESERVE_PACKS_PERCENT_IND = new charParameter("@RESERVE_PACKS_PERCENT_IND", base.inputParameterList);
			        AVG_PACK_DEV_TOLERANCE = new floatParameter("@AVG_PACK_DEV_TOLERANCE", base.inputParameterList);
			        MAX_PACK_NEED_TOLERANCE = new floatParameter("@MAX_PACK_NEED_TOLERANCE", base.inputParameterList);
			        DEPLETE_RESERVE_SELECTED = new charParameter("@DEPLETE_RESERVE_SELECTED", base.inputParameterList);
			        INCREASE_BUY_SELECTED = new charParameter("@INCREASE_BUY_SELECTED", base.inputParameterList);
			        INCREASE_BUY_PCT = new floatParameter("@INCREASE_BUY_PCT", base.inputParameterList);
			        REMOVE_BULK_IND = new charParameter("@REMOVE_BULK_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BPC_RID,
			                      int? PATTERN_COMP_MIN,
			                      int? PATTERN_SIZE_MULTIPLE,
			                      int? SIZE_GROUP_RID,
			                      int? SIZE_CURVE_GROUP_RID,
			                      double? RESERVE_TOTAL,
			                      char? RESERVE_TOTAL_PERCENT_IND,
			                      double? RESERVE_BULK,
			                      char? RESERVE_BULK_PERCENT_IND,
			                      double? RESERVE_PACKS,
			                      char? RESERVE_PACKS_PERCENT_IND,
			                      double? AVG_PACK_DEV_TOLERANCE,
			                      double? MAX_PACK_NEED_TOLERANCE,
			                      char? DEPLETE_RESERVE_SELECTED,
			                      char? INCREASE_BUY_SELECTED,
			                      double? INCREASE_BUY_PCT,
			                      char? REMOVE_BULK_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BPC_RID.SetValue(BPC_RID);
                        this.PATTERN_COMP_MIN.SetValue(PATTERN_COMP_MIN);
                        this.PATTERN_SIZE_MULTIPLE.SetValue(PATTERN_SIZE_MULTIPLE);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.RESERVE_TOTAL.SetValue(RESERVE_TOTAL);
                        this.RESERVE_TOTAL_PERCENT_IND.SetValue(RESERVE_TOTAL_PERCENT_IND);
                        this.RESERVE_BULK.SetValue(RESERVE_BULK);
                        this.RESERVE_BULK_PERCENT_IND.SetValue(RESERVE_BULK_PERCENT_IND);
                        this.RESERVE_PACKS.SetValue(RESERVE_PACKS);
                        this.RESERVE_PACKS_PERCENT_IND.SetValue(RESERVE_PACKS_PERCENT_IND);
                        this.AVG_PACK_DEV_TOLERANCE.SetValue(AVG_PACK_DEV_TOLERANCE);
                        this.MAX_PACK_NEED_TOLERANCE.SetValue(MAX_PACK_NEED_TOLERANCE);
                        this.DEPLETE_RESERVE_SELECTED.SetValue(DEPLETE_RESERVE_SELECTED);
                        this.INCREASE_BUY_SELECTED.SetValue(INCREASE_BUY_SELECTED);
                        this.INCREASE_BUY_PCT.SetValue(INCREASE_BUY_PCT);
                        this.REMOVE_BULK_IND.SetValue(REMOVE_BULK_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_UPDATE_def MID_METHOD_BLD_PACKS_UPDATE = new MID_METHOD_BLD_PACKS_UPDATE_def();
			public class MID_METHOD_BLD_PACKS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter BPC_RID;
                private intParameter PATTERN_COMP_MIN;
                private intParameter PATTERN_SIZE_MULTIPLE;
                private intParameter SIZE_GROUP_RID;
                private intParameter SIZE_CURVE_GROUP_RID;
                private floatParameter RESERVE_TOTAL;
                private charParameter RESERVE_TOTAL_PERCENT_IND;
                private floatParameter RESERVE_BULK;
                private charParameter RESERVE_BULK_PERCENT_IND;
                private floatParameter RESERVE_PACKS;
                private charParameter RESERVE_PACKS_PERCENT_IND;
                private floatParameter AVG_PACK_DEV_TOLERANCE;
                private floatParameter MAX_PACK_NEED_TOLERANCE;
                private charParameter DEPLETE_RESERVE_SELECTED;
                private charParameter INCREASE_BUY_SELECTED;
                private floatParameter INCREASE_BUY_PCT;
                private charParameter REMOVE_BULK_IND;
			
			    public MID_METHOD_BLD_PACKS_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_BLD_PACKS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BPC_RID = new intParameter("@BPC_RID", base.inputParameterList);
			        PATTERN_COMP_MIN = new intParameter("@PATTERN_COMP_MIN", base.inputParameterList);
			        PATTERN_SIZE_MULTIPLE = new intParameter("@PATTERN_SIZE_MULTIPLE", base.inputParameterList);
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			        RESERVE_TOTAL = new floatParameter("@RESERVE_TOTAL", base.inputParameterList);
			        RESERVE_TOTAL_PERCENT_IND = new charParameter("@RESERVE_TOTAL_PERCENT_IND", base.inputParameterList);
			        RESERVE_BULK = new floatParameter("@RESERVE_BULK", base.inputParameterList);
			        RESERVE_BULK_PERCENT_IND = new charParameter("@RESERVE_BULK_PERCENT_IND", base.inputParameterList);
			        RESERVE_PACKS = new floatParameter("@RESERVE_PACKS", base.inputParameterList);
			        RESERVE_PACKS_PERCENT_IND = new charParameter("@RESERVE_PACKS_PERCENT_IND", base.inputParameterList);
			        AVG_PACK_DEV_TOLERANCE = new floatParameter("@AVG_PACK_DEV_TOLERANCE", base.inputParameterList);
			        MAX_PACK_NEED_TOLERANCE = new floatParameter("@MAX_PACK_NEED_TOLERANCE", base.inputParameterList);
			        DEPLETE_RESERVE_SELECTED = new charParameter("@DEPLETE_RESERVE_SELECTED", base.inputParameterList);
			        INCREASE_BUY_SELECTED = new charParameter("@INCREASE_BUY_SELECTED", base.inputParameterList);
			        INCREASE_BUY_PCT = new floatParameter("@INCREASE_BUY_PCT", base.inputParameterList);
			        REMOVE_BULK_IND = new charParameter("@REMOVE_BULK_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BPC_RID,
			                      int? PATTERN_COMP_MIN,
			                      int? PATTERN_SIZE_MULTIPLE,
			                      int? SIZE_GROUP_RID,
			                      int? SIZE_CURVE_GROUP_RID,
			                      double? RESERVE_TOTAL,
			                      char? RESERVE_TOTAL_PERCENT_IND,
			                      double? RESERVE_BULK,
			                      char? RESERVE_BULK_PERCENT_IND,
			                      double? RESERVE_PACKS,
			                      char? RESERVE_PACKS_PERCENT_IND,
			                      double? AVG_PACK_DEV_TOLERANCE,
			                      double? MAX_PACK_NEED_TOLERANCE,
			                      char? DEPLETE_RESERVE_SELECTED,
			                      char? INCREASE_BUY_SELECTED,
			                      double? INCREASE_BUY_PCT,
			                      char? REMOVE_BULK_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BPC_RID.SetValue(BPC_RID);
                        this.PATTERN_COMP_MIN.SetValue(PATTERN_COMP_MIN);
                        this.PATTERN_SIZE_MULTIPLE.SetValue(PATTERN_SIZE_MULTIPLE);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.RESERVE_TOTAL.SetValue(RESERVE_TOTAL);
                        this.RESERVE_TOTAL_PERCENT_IND.SetValue(RESERVE_TOTAL_PERCENT_IND);
                        this.RESERVE_BULK.SetValue(RESERVE_BULK);
                        this.RESERVE_BULK_PERCENT_IND.SetValue(RESERVE_BULK_PERCENT_IND);
                        this.RESERVE_PACKS.SetValue(RESERVE_PACKS);
                        this.RESERVE_PACKS_PERCENT_IND.SetValue(RESERVE_PACKS_PERCENT_IND);
                        this.AVG_PACK_DEV_TOLERANCE.SetValue(AVG_PACK_DEV_TOLERANCE);
                        this.MAX_PACK_NEED_TOLERANCE.SetValue(MAX_PACK_NEED_TOLERANCE);
                        this.DEPLETE_RESERVE_SELECTED.SetValue(DEPLETE_RESERVE_SELECTED);
                        this.INCREASE_BUY_SELECTED.SetValue(INCREASE_BUY_SELECTED);
                        this.INCREASE_BUY_PCT.SetValue(INCREASE_BUY_PCT);
                        this.REMOVE_BULK_IND.SetValue(REMOVE_BULK_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE_def MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE = new MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE_def();
			public class MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE.SQL"

			    public intParameter METHOD_RID;
			
			    public MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_BLD_PACKS_BPC_SELECT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE_def MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE = new MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE_def();
			public class MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_DELETE_def MID_METHOD_BLD_PACKS_COMBO_DELETE = new MID_METHOD_BLD_PACKS_COMBO_DELETE_def();
			public class MID_METHOD_BLD_PACKS_COMBO_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_DELETE.SQL"

                private intParameter METHOD_RID;
                private tableParameter COMBO_RID_LIST;
			
			    public MID_METHOD_BLD_PACKS_COMBO_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COMBO_RID_LIST = new tableParameter("@COMBO_RID_LIST", "COMBO_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      DataTable COMBO_RID_LIST
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_RID_LIST.SetValue(COMBO_RID_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE_def MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE = new MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE_def();
			public class MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE.SQL"

                private intParameter METHOD_RID;
                private tableParameter PATTERN_PACK_RID_LIST;
			
			    public MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO_PATTERN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        PATTERN_PACK_RID_LIST = new tableParameter("@PATTERN_PACK_RID_LIST", "PATTERN_PACK_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      DataTable PATTERN_PACK_RID_LIST
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.PATTERN_PACK_RID_LIST.SetValue(PATTERN_PACK_RID_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT_def MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT = new MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT_def();
			public class MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BPC_COMBO_RID;
			
			    public MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_BLD_PACKS_BPC_SELECT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BPC_COMBO_RID = new intParameter("@BPC_COMBO_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BPC_COMBO_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BPC_COMBO_RID.SetValue(BPC_COMBO_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_MTHD_BPCOMBO_INSERT_def SP_MID_MTHD_BPCOMBO_INSERT = new SP_MID_MTHD_BPCOMBO_INSERT_def();
            public class SP_MID_MTHD_BPCOMBO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTHD_BPCOMBO_INSERT.SQL"

                private intParameter METHOD_RID;
                private stringParameter COMBO_NAME;
                private charParameter COMBO_SELECTED_IND;
                private intParameter COMBO_RID; //Declare Output Parameter

                public SP_MID_MTHD_BPCOMBO_INSERT_def()
                {
                    base.procedureName = "SP_MID_MTHD_BPCOMBO_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("MTHD_BPCOMBO");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    COMBO_NAME = new stringParameter("@COMBO_NAME", base.inputParameterList);
                    COMBO_SELECTED_IND = new charParameter("@COMBO_SELECTED_IND", base.inputParameterList);
                    COMBO_RID = new intParameter("@COMBO_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? METHOD_RID,
                                              string COMBO_NAME,
                                              char? COMBO_SELECTED_IND
                                              )
                {
                    lock (typeof(SP_MID_MTHD_BPCOMBO_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_NAME.SetValue(COMBO_NAME);
                        this.COMBO_SELECTED_IND.SetValue(COMBO_SELECTED_IND);
                        this.COMBO_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }


			public static MID_METHOD_BLD_PACKS_COMBO_UPDATE_def MID_METHOD_BLD_PACKS_COMBO_UPDATE = new MID_METHOD_BLD_PACKS_COMBO_UPDATE_def();
			public class MID_METHOD_BLD_PACKS_COMBO_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter COMBO_RID;
                private stringParameter COMBO_NAME;
                private charParameter COMBO_SELECTED_IND;
			
			    public MID_METHOD_BLD_PACKS_COMBO_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COMBO_RID = new intParameter("@COMBO_RID", base.inputParameterList);
			        COMBO_NAME = new stringParameter("@COMBO_NAME", base.inputParameterList);
			        COMBO_SELECTED_IND = new charParameter("@COMBO_SELECTED_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? COMBO_RID,
			                      string COMBO_NAME,
			                      char? COMBO_SELECTED_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_RID.SetValue(COMBO_RID);
                        this.COMBO_NAME.SetValue(COMBO_NAME);
                        this.COMBO_SELECTED_IND.SetValue(COMBO_SELECTED_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_MTHD_BPCPATTERN_INSERT_def SP_MID_MTHD_BPCPATTERN_INSERT = new SP_MID_MTHD_BPCPATTERN_INSERT_def();
            public class SP_MID_MTHD_BPCPATTERN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTHD_BPCPATTERN_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter COMBO_RID;
                private stringParameter PATTERN_NAME;
                private intParameter PATTERN_PACK_MULT;
                private intParameter COMBO_MAX_PATTERN_PACKS;
                private intParameter PACK_PATTERN_RID; //Declare Output Parameter
			
			    public SP_MID_MTHD_BPCPATTERN_INSERT_def()
			    {
			        base.procedureName = "SP_MID_MTHD_BPCPATTERN_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("MTHD_BPCPATTERN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COMBO_RID = new intParameter("@COMBO_RID", base.inputParameterList);
			        PATTERN_NAME = new stringParameter("@PATTERN_NAME", base.inputParameterList);
			        PATTERN_PACK_MULT = new intParameter("@PATTERN_PACK_MULT", base.inputParameterList);
			        COMBO_MAX_PATTERN_PACKS = new intParameter("@COMBO_MAX_PATTERN_PACKS", base.inputParameterList);
			        PACK_PATTERN_RID = new intParameter("@PACK_PATTERN_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? METHOD_RID,
			                                  int? COMBO_RID,
			                                  string PATTERN_NAME,
			                                  int? PATTERN_PACK_MULT,
			                                  int? COMBO_MAX_PATTERN_PACKS
			                                  )
			    {
                    lock (typeof(SP_MID_MTHD_BPCPATTERN_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_RID.SetValue(COMBO_RID);
                        this.PATTERN_NAME.SetValue(PATTERN_NAME);
                        this.PATTERN_PACK_MULT.SetValue(PATTERN_PACK_MULT);
                        this.COMBO_MAX_PATTERN_PACKS.SetValue(COMBO_MAX_PATTERN_PACKS);
                        this.PACK_PATTERN_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE_def MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE = new MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE_def();
			public class MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter COMBO_RID;
                private intParameter PATTERN_PACK_RID;
                private stringParameter PATTERN_NAME;
                private intParameter PATTERN_PACK_MULT;
                private intParameter COMBO_MAX_PATTERN_PACKS;
			
			    public MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO_PATTERN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COMBO_RID = new intParameter("@COMBO_RID", base.inputParameterList);
			        PATTERN_PACK_RID = new intParameter("@PATTERN_PACK_RID", base.inputParameterList);
			        PATTERN_NAME = new stringParameter("@PATTERN_NAME", base.inputParameterList);
			        PATTERN_PACK_MULT = new intParameter("@PATTERN_PACK_MULT", base.inputParameterList);
			        COMBO_MAX_PATTERN_PACKS = new intParameter("@COMBO_MAX_PATTERN_PACKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? COMBO_RID,
			                      int? PATTERN_PACK_RID,
			                      string PATTERN_NAME,
			                      int? PATTERN_PACK_MULT,
			                      int? COMBO_MAX_PATTERN_PACKS
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_RID.SetValue(COMBO_RID);
                        this.PATTERN_PACK_RID.SetValue(PATTERN_PACK_RID);
                        this.PATTERN_NAME.SetValue(PATTERN_NAME);
                        this.PATTERN_PACK_MULT.SetValue(PATTERN_PACK_MULT);
                        this.COMBO_MAX_PATTERN_PACKS.SetValue(COMBO_MAX_PATTERN_PACKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT_def MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT = new MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT_def();
			public class MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter COMBO_RID;
                private intParameter PATTERN_PACK_RID;
                private intParameter SIZE_CODE_RID;
                private intParameter PATTERN_SIZE_UNITS;
			
			    public MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COMBO_RID = new intParameter("@COMBO_RID", base.inputParameterList);
			        PATTERN_PACK_RID = new intParameter("@PATTERN_PACK_RID", base.inputParameterList);
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			        PATTERN_SIZE_UNITS = new intParameter("@PATTERN_SIZE_UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? COMBO_RID,
			                      int? PATTERN_PACK_RID,
			                      int? SIZE_CODE_RID,
			                      int? PATTERN_SIZE_UNITS
			                      )
			    {
                    lock (typeof(MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COMBO_RID.SetValue(COMBO_RID);
                        this.PATTERN_PACK_RID.SetValue(PATTERN_PACK_RID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.PATTERN_SIZE_UNITS.SetValue(PATTERN_SIZE_UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES_def MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES = new MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES_def();
			public class MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES.SQL"

			
			    public MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES_def()
			    {
			        base.procedureName = "MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BUILD_PACK_CRITERIA");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_FILL_SIZE_HOLES_READ_def MID_METHOD_FILL_SIZE_HOLES_READ = new MID_METHOD_FILL_SIZE_HOLES_READ_def();
			public class MID_METHOD_FILL_SIZE_HOLES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_FILL_SIZE_HOLES_READ.SQL"

			    public intParameter METHOD_RID;
			
			    public MID_METHOD_FILL_SIZE_HOLES_READ_def()
			    {
			        base.procedureName = "MID_METHOD_FILL_SIZE_HOLES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_FILL_SIZE_HOLES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_FILL_SIZE_HOLES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_FSH_UPD_INS_def SP_MID_MTH_FSH_UPD_INS = new SP_MID_MTH_FSH_UPD_INS_def();
            public class SP_MID_MTH_FSH_UPD_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_FSH_UPD_INS.SQL"

                private intParameter METHODRID;
                private floatParameter AVAILABLE;
                private charParameter AVAILABLEIND;
                private intParameter SIZEGROUPRID;
                private intParameter SIZECURVEGROUPRID;
                private intParameter MERCHTYPE;
                private intParameter MERCHHNRID;
                private intParameter MERCHPHRID;
                private intParameter MERCHPHLSEQ;
                private intParameter STOREFILTERRID;
                private intParameter SIZEALTERNATERID;
                private intParameter SIZECONSTRAINTRID;
                private intParameter GENCURVE_HCG_RID;
                private intParameter GENCURVE_HN_RID;
                private intParameter GENCURVE_PH_RID;
                private intParameter GENCURVE_PHL_SEQUENCE;
                private charParameter GENCURVE_COLOR_IND;
                private intParameter GENCURVE_MERCH_TYPE;
                private intParameter GENCONSTRAINT_HCG_RID;
                private intParameter GENCONSTRAINT_HN_RID;
                private intParameter GENCONSTRAINT_PH_RID;
                private intParameter GENCONSTRAINT_PHL_SEQUENCE;
                private charParameter GENCONSTRAINT_COLOR_IND;
                private intParameter GENCONSTRAINT_MERCH_TYPE;
                private charParameter NORMALIZE_SIZE_CURVES_IND;
                private intParameter FILL_SIZES_TO_TYPE;
                private charParameter USE_DEFAULT_CURVE_IND;
                private intParameter GENCURVE_NSCCD_RID;
                private charParameter APPLY_RULES_ONLY_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private charParameter VSW_SIZE_CONSTRAINTS_IND;
                private intParameter VSW_SIZE_CONSTRAINTS;
                private floatParameter AVGPACKDEVIATION; //TT#1636-MD -jsobek -Pre-Pack Fill Size
                private floatParameter MAXPACKNEED; //TT#1636-MD -jsobek -Pre-Pack Fill Size
                private charParameter OVERRIDE_AVG_PACK_DEV_IND; //TT#1636-MD -jsobek -Pre-Pack Fill Size
                private charParameter OVERRIDE_MAX_PACK_NEED_IND; //TT#1636-MD -jsobek -Pre-Pack Fill Size
                private charParameter PACK_TOLERANCE_NO_MAX_STEP_IND; //TT#1636-MD -jsobek -Pre-Pack Fill Size
                private charParameter PACK_TOLERANCE_STEPPED_IND; //TT#1636-MD -jsobek -Pre-Pack Fill Size
			
			    public SP_MID_MTH_FSH_UPD_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_FSH_UPD_INS";
			        base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_FILL_SIZE_HOLES");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        AVAILABLE = new floatParameter("@AVAILABLE", base.inputParameterList);
                    AVAILABLEIND = new charParameter("@AVAILABLEIND", base.inputParameterList);
			        SIZEGROUPRID = new intParameter("@SIZEGROUPRID", base.inputParameterList);
			        SIZECURVEGROUPRID = new intParameter("@SIZECURVEGROUPRID", base.inputParameterList);
			        MERCHTYPE = new intParameter("@MERCHTYPE", base.inputParameterList);
			        MERCHHNRID = new intParameter("@MERCHHNRID", base.inputParameterList);
			        MERCHPHRID = new intParameter("@MERCHPHRID", base.inputParameterList);
			        MERCHPHLSEQ = new intParameter("@MERCHPHLSEQ", base.inputParameterList);
			        STOREFILTERRID = new intParameter("@STOREFILTERRID", base.inputParameterList);
			        SIZEALTERNATERID = new intParameter("@SIZEALTERNATERID", base.inputParameterList);
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        GENCURVE_HCG_RID = new intParameter("@GENCURVE_HCG_RID", base.inputParameterList);
			        GENCURVE_HN_RID = new intParameter("@GENCURVE_HN_RID", base.inputParameterList);
			        GENCURVE_PH_RID = new intParameter("@GENCURVE_PH_RID", base.inputParameterList);
			        GENCURVE_PHL_SEQUENCE = new intParameter("@GENCURVE_PHL_SEQUENCE", base.inputParameterList);
                    GENCURVE_COLOR_IND = new charParameter("@GENCURVE_COLOR_IND", base.inputParameterList);
			        GENCURVE_MERCH_TYPE = new intParameter("@GENCURVE_MERCH_TYPE", base.inputParameterList);
			        GENCONSTRAINT_HCG_RID = new intParameter("@GENCONSTRAINT_HCG_RID", base.inputParameterList);
			        GENCONSTRAINT_HN_RID = new intParameter("@GENCONSTRAINT_HN_RID", base.inputParameterList);
			        GENCONSTRAINT_PH_RID = new intParameter("@GENCONSTRAINT_PH_RID", base.inputParameterList);
			        GENCONSTRAINT_PHL_SEQUENCE = new intParameter("@GENCONSTRAINT_PHL_SEQUENCE", base.inputParameterList);
                    GENCONSTRAINT_COLOR_IND = new charParameter("@GENCONSTRAINT_COLOR_IND", base.inputParameterList);
			        GENCONSTRAINT_MERCH_TYPE = new intParameter("@GENCONSTRAINT_MERCH_TYPE", base.inputParameterList);
                    NORMALIZE_SIZE_CURVES_IND = new charParameter("@NORMALIZE_SIZE_CURVES_IND", base.inputParameterList);
			        FILL_SIZES_TO_TYPE = new intParameter("@FILL_SIZES_TO_TYPE", base.inputParameterList);
                    USE_DEFAULT_CURVE_IND = new charParameter("@USE_DEFAULT_CURVE_IND", base.inputParameterList);
			        GENCURVE_NSCCD_RID = new intParameter("@GENCURVE_NSCCD_RID", base.inputParameterList);
                    APPLY_RULES_ONLY_IND = new charParameter("@APPLY_RULES_ONLY_IND", base.inputParameterList);
			        IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
			        IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
			        IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
			        IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                    VSW_SIZE_CONSTRAINTS_IND = new charParameter("@VSW_SIZE_CONSTRAINTS_IND", base.inputParameterList);
			        VSW_SIZE_CONSTRAINTS = new intParameter("@VSW_SIZE_CONSTRAINTS", base.inputParameterList);
                    AVGPACKDEVIATION = new floatParameter("@AVGPACKDEVIATION", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    MAXPACKNEED = new floatParameter("@MAXPACKNEED", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    OVERRIDE_AVG_PACK_DEV_IND = new charParameter("@OVERRIDE_AVG_PACK_DEV_IND", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    OVERRIDE_MAX_PACK_NEED_IND = new charParameter("@OVERRIDE_MAX_PACK_NEED_IND", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    PACK_TOLERANCE_NO_MAX_STEP_IND = new charParameter("@PACK_TOLERANCE_NO_MAX_STEP_IND", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    PACK_TOLERANCE_STEPPED_IND = new charParameter("@PACK_TOLERANCE_STEPPED_IND", base.inputParameterList); //TT#1636-MD -jsobek -Pre-Pack Fill Size
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHODRID,
			                      double? AVAILABLE,
			                      char? AVAILABLEIND,
			                      int? SIZEGROUPRID,
			                      int? SIZECURVEGROUPRID,
			                      int? MERCHTYPE,
			                      int? MERCHHNRID,
			                      int? MERCHPHRID,
			                      int? MERCHPHLSEQ,
			                      int? STOREFILTERRID,
			                      int? SIZEALTERNATERID,
			                      int? SIZECONSTRAINTRID,
			                      int? GENCURVE_HCG_RID,
			                      int? GENCURVE_HN_RID,
			                      int? GENCURVE_PH_RID,
			                      int? GENCURVE_PHL_SEQUENCE,
			                      char? GENCURVE_COLOR_IND,
			                      int? GENCURVE_MERCH_TYPE,
			                      int? GENCONSTRAINT_HCG_RID,
			                      int? GENCONSTRAINT_HN_RID,
			                      int? GENCONSTRAINT_PH_RID,
			                      int? GENCONSTRAINT_PHL_SEQUENCE,
			                      char? GENCONSTRAINT_COLOR_IND,
			                      int? GENCONSTRAINT_MERCH_TYPE,
			                      char? NORMALIZE_SIZE_CURVES_IND,
			                      int? FILL_SIZES_TO_TYPE,
			                      char? USE_DEFAULT_CURVE_IND,
			                      int? GENCURVE_NSCCD_RID,
			                      char? APPLY_RULES_ONLY_IND,
			                      int? IB_MERCH_TYPE,
			                      int? IB_MERCH_HN_RID,
			                      int? IB_MERCH_PH_RID,
			                      int? IB_MERCH_PHL_SEQUENCE,
			                      char? VSW_SIZE_CONSTRAINTS_IND,
			                      int? VSW_SIZE_CONSTRAINTS,
                                  double? AVGPACKDEVIATION, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                  double? MAXPACKNEED, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                  char? OVERRIDE_AVG_PACK_DEV_IND, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                  char? OVERRIDE_MAX_PACK_NEED_IND, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                  char? PACK_TOLERANCE_NO_MAX_STEP_IND, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                  char? PACK_TOLERANCE_STEPPED_IND //TT#1636-MD -jsobek -Pre-Pack Fill Size
			                      )
			    {
                    lock (typeof(SP_MID_MTH_FSH_UPD_INS_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.AVAILABLE.SetValue(AVAILABLE);
                        this.AVAILABLEIND.SetValue(AVAILABLEIND);
                        this.SIZEGROUPRID.SetValue(SIZEGROUPRID);
                        this.SIZECURVEGROUPRID.SetValue(SIZECURVEGROUPRID);
                        this.MERCHTYPE.SetValue(MERCHTYPE);
                        this.MERCHHNRID.SetValue(MERCHHNRID);
                        this.MERCHPHRID.SetValue(MERCHPHRID);
                        this.MERCHPHLSEQ.SetValue(MERCHPHLSEQ);
                        this.STOREFILTERRID.SetValue(STOREFILTERRID);
                        this.SIZEALTERNATERID.SetValue(SIZEALTERNATERID);
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.GENCURVE_HCG_RID.SetValue(GENCURVE_HCG_RID);
                        this.GENCURVE_HN_RID.SetValue(GENCURVE_HN_RID);
                        this.GENCURVE_PH_RID.SetValue(GENCURVE_PH_RID);
                        this.GENCURVE_PHL_SEQUENCE.SetValue(GENCURVE_PHL_SEQUENCE);
                        this.GENCURVE_COLOR_IND.SetValue(GENCURVE_COLOR_IND);
                        this.GENCURVE_MERCH_TYPE.SetValue(GENCURVE_MERCH_TYPE);
                        this.GENCONSTRAINT_HCG_RID.SetValue(GENCONSTRAINT_HCG_RID);
                        this.GENCONSTRAINT_HN_RID.SetValue(GENCONSTRAINT_HN_RID);
                        this.GENCONSTRAINT_PH_RID.SetValue(GENCONSTRAINT_PH_RID);
                        this.GENCONSTRAINT_PHL_SEQUENCE.SetValue(GENCONSTRAINT_PHL_SEQUENCE);
                        this.GENCONSTRAINT_COLOR_IND.SetValue(GENCONSTRAINT_COLOR_IND);
                        this.GENCONSTRAINT_MERCH_TYPE.SetValue(GENCONSTRAINT_MERCH_TYPE);
                        this.NORMALIZE_SIZE_CURVES_IND.SetValue(NORMALIZE_SIZE_CURVES_IND);
                        this.FILL_SIZES_TO_TYPE.SetValue(FILL_SIZES_TO_TYPE);
                        this.USE_DEFAULT_CURVE_IND.SetValue(USE_DEFAULT_CURVE_IND);
                        this.GENCURVE_NSCCD_RID.SetValue(GENCURVE_NSCCD_RID);
                        this.APPLY_RULES_ONLY_IND.SetValue(APPLY_RULES_ONLY_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.VSW_SIZE_CONSTRAINTS_IND.SetValue(VSW_SIZE_CONSTRAINTS_IND);
                        this.VSW_SIZE_CONSTRAINTS.SetValue(VSW_SIZE_CONSTRAINTS);
                        this.AVGPACKDEVIATION.SetValue(AVGPACKDEVIATION); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        this.MAXPACKNEED.SetValue(MAXPACKNEED); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        this.OVERRIDE_AVG_PACK_DEV_IND.SetValue(OVERRIDE_AVG_PACK_DEV_IND); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        this.OVERRIDE_MAX_PACK_NEED_IND.SetValue(OVERRIDE_MAX_PACK_NEED_IND); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        this.PACK_TOLERANCE_NO_MAX_STEP_IND.SetValue(PACK_TOLERANCE_NO_MAX_STEP_IND); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        this.PACK_TOLERANCE_STEPPED_IND.SetValue(PACK_TOLERANCE_STEPPED_IND); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_FSH_DEL_MTH_def SP_MID_MTH_FSH_DEL_MTH = new SP_MID_MTH_FSH_DEL_MTH_def();
            public class SP_MID_MTH_FSH_DEL_MTH_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_FSH_DEL_MTH.SQL"

                private intParameter METHODRID;
			
			    public SP_MID_MTH_FSH_DEL_MTH_def()
			    {
			        base.procedureName = "SP_MID_MTH_FSH_DEL_MTH";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_FILL_SIZE_HOLES");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHODRID)
			    {
                    lock (typeof(SP_MID_MTH_FSH_DEL_MTH_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_READ_def MID_METHOD_MATRIX_READ = new MID_METHOD_MATRIX_READ_def();
			public class MID_METHOD_MATRIX_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MATRIX_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MATRIX_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_BASIS_DETAILS_READ_def MID_METHOD_MATRIX_BASIS_DETAILS_READ = new MID_METHOD_MATRIX_BASIS_DETAILS_READ_def();
			public class MID_METHOD_MATRIX_BASIS_DETAILS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_BASIS_DETAILS_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MATRIX_BASIS_DETAILS_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_BASIS_DETAILS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MATRIX_BASIS_DETAILS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MATRIX_BASIS_DETAILS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_READ_ALL_def MID_METHOD_MATRIX_READ_ALL = new MID_METHOD_MATRIX_READ_ALL_def();
			public class MID_METHOD_MATRIX_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_READ_ALL.SQL"

			
			    public MID_METHOD_MATRIX_READ_ALL_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MATRIX");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_METHOD_MATRIX_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_INSERT_def MID_METHOD_MATRIX_INSERT = new MID_METHOD_MATRIX_INSERT_def();
			public class MID_METHOD_MATRIX_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter FILTER_RID;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_FV_RID;
                private intParameter CDR_RID;
                private intParameter MODEL_RID;
                private intParameter MATRIX_TYPE;
                private intParameter LOW_LEVEL_FV_RID;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter LOW_LEVEL_SEQUENCE;
                private charParameter INCLUDE_INELIGIBLE_STORES_IND;
                private charParameter INCLUDE_SIMILAR_STORES_IND;
                private intParameter VARIABLE_NUMBER;
                private intParameter ITERATIONS_TYPE;
                private intParameter ITERATIONS_COUNT;
                private intParameter BALANCE_MODE;
                private stringParameter CALC_MODE;
                private intParameter OLL_RID;
			
			    public MID_METHOD_MATRIX_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_FV_RID = new intParameter("@HIGH_LEVEL_FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
			        MATRIX_TYPE = new intParameter("@MATRIX_TYPE", base.inputParameterList);
			        LOW_LEVEL_FV_RID = new intParameter("@LOW_LEVEL_FV_RID", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
			        INCLUDE_INELIGIBLE_STORES_IND = new charParameter("@INCLUDE_INELIGIBLE_STORES_IND", base.inputParameterList);
			        INCLUDE_SIMILAR_STORES_IND = new charParameter("@INCLUDE_SIMILAR_STORES_IND", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        ITERATIONS_TYPE = new intParameter("@ITERATIONS_TYPE", base.inputParameterList);
			        ITERATIONS_COUNT = new intParameter("@ITERATIONS_COUNT", base.inputParameterList);
			        BALANCE_MODE = new intParameter("@BALANCE_MODE", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? FILTER_RID,
			                      int? HN_RID,
			                      int? HIGH_LEVEL_FV_RID,
			                      int? CDR_RID,
			                      int? MODEL_RID,
			                      int? MATRIX_TYPE,
			                      int? LOW_LEVEL_FV_RID,
			                      int? LOW_LEVEL_TYPE,
			                      int? LOW_LEVEL_OFFSET,
			                      int? LOW_LEVEL_SEQUENCE,
			                      char? INCLUDE_INELIGIBLE_STORES_IND,
			                      char? INCLUDE_SIMILAR_STORES_IND,
			                      int? VARIABLE_NUMBER,
			                      int? ITERATIONS_TYPE,
			                      int? ITERATIONS_COUNT,
			                      int? BALANCE_MODE,
			                      string CALC_MODE,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_MATRIX_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_FV_RID.SetValue(HIGH_LEVEL_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.MATRIX_TYPE.SetValue(MATRIX_TYPE);
                        this.LOW_LEVEL_FV_RID.SetValue(LOW_LEVEL_FV_RID);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.INCLUDE_INELIGIBLE_STORES_IND.SetValue(INCLUDE_INELIGIBLE_STORES_IND);
                        this.INCLUDE_SIMILAR_STORES_IND.SetValue(INCLUDE_SIMILAR_STORES_IND);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.ITERATIONS_TYPE.SetValue(ITERATIONS_TYPE);
                        this.ITERATIONS_COUNT.SetValue(ITERATIONS_COUNT);
                        this.BALANCE_MODE.SetValue(BALANCE_MODE);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_BASIS_DETAILS_INSERT_def MID_METHOD_MATRIX_BASIS_DETAILS_INSERT = new MID_METHOD_MATRIX_BASIS_DETAILS_INSERT_def();
			public class MID_METHOD_MATRIX_BASIS_DETAILS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_BASIS_DETAILS_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SEQ_ID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private floatParameter WEIGHT;
                private charParameter IS_INCLUDED_IND;
			
			    public MID_METHOD_MATRIX_BASIS_DETAILS_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_BASIS_DETAILS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MATRIX_BASIS_DETAILS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SEQ_ID = new intParameter("@SEQ_ID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			        IS_INCLUDED_IND = new charParameter("@IS_INCLUDED_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SEQ_ID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      double? WEIGHT,
			                      char? IS_INCLUDED_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_MATRIX_BASIS_DETAILS_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SEQ_ID.SetValue(SEQ_ID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        this.IS_INCLUDED_IND.SetValue(IS_INCLUDED_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_UPDATE_def MID_METHOD_MATRIX_UPDATE = new MID_METHOD_MATRIX_UPDATE_def();
			public class MID_METHOD_MATRIX_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter FILTER_RID;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_FV_RID;
                private intParameter CDR_RID;
                private intParameter MATRIX_TYPE;
                private intParameter MODEL_RID;
                private intParameter LOW_LEVEL_FV_RID;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter LOW_LEVEL_SEQUENCE;
                private charParameter INCLUDE_INELIGIBLE_STORES_IND;
                private charParameter INCLUDE_SIMILAR_STORES_IND;
                private intParameter VARIABLE_NUMBER;
                private intParameter ITERATIONS_TYPE;
                private intParameter ITERATIONS_COUNT;
                private intParameter BALANCE_MODE;
                private stringParameter CALC_MODE;
                private intParameter OLL_RID;
			
			    public MID_METHOD_MATRIX_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_FV_RID = new intParameter("@HIGH_LEVEL_FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MATRIX_TYPE = new intParameter("@MATRIX_TYPE", base.inputParameterList);
			        MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
			        LOW_LEVEL_FV_RID = new intParameter("@LOW_LEVEL_FV_RID", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
			        INCLUDE_INELIGIBLE_STORES_IND = new charParameter("@INCLUDE_INELIGIBLE_STORES_IND", base.inputParameterList);
			        INCLUDE_SIMILAR_STORES_IND = new charParameter("@INCLUDE_SIMILAR_STORES_IND", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        ITERATIONS_TYPE = new intParameter("@ITERATIONS_TYPE", base.inputParameterList);
			        ITERATIONS_COUNT = new intParameter("@ITERATIONS_COUNT", base.inputParameterList);
			        BALANCE_MODE = new intParameter("@BALANCE_MODE", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? FILTER_RID,
			                      int? HN_RID,
			                      int? HIGH_LEVEL_FV_RID,
			                      int? CDR_RID,
			                      int? MATRIX_TYPE,
			                      int? MODEL_RID,
			                      int? LOW_LEVEL_FV_RID,
			                      int? LOW_LEVEL_TYPE,
			                      int? LOW_LEVEL_OFFSET,
			                      int? LOW_LEVEL_SEQUENCE,
			                      char? INCLUDE_INELIGIBLE_STORES_IND,
			                      char? INCLUDE_SIMILAR_STORES_IND,
			                      int? VARIABLE_NUMBER,
			                      int? ITERATIONS_TYPE,
			                      int? ITERATIONS_COUNT,
			                      int? BALANCE_MODE,
			                      string CALC_MODE,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_MATRIX_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_FV_RID.SetValue(HIGH_LEVEL_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MATRIX_TYPE.SetValue(MATRIX_TYPE);
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.LOW_LEVEL_FV_RID.SetValue(LOW_LEVEL_FV_RID);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.INCLUDE_INELIGIBLE_STORES_IND.SetValue(INCLUDE_INELIGIBLE_STORES_IND);
                        this.INCLUDE_SIMILAR_STORES_IND.SetValue(INCLUDE_SIMILAR_STORES_IND);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.ITERATIONS_TYPE.SetValue(ITERATIONS_TYPE);
                        this.ITERATIONS_COUNT.SetValue(ITERATIONS_COUNT);
                        this.BALANCE_MODE.SetValue(BALANCE_MODE);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_DELETE_def MID_METHOD_MATRIX_DELETE = new MID_METHOD_MATRIX_DELETE_def();
			public class MID_METHOD_MATRIX_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MATRIX_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MATRIX_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_BASIS_DETAILS_DELETE_def MID_METHOD_MATRIX_BASIS_DETAILS_DELETE = new MID_METHOD_MATRIX_BASIS_DETAILS_DELETE_def();
			public class MID_METHOD_MATRIX_BASIS_DETAILS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_BASIS_DETAILS_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MATRIX_BASIS_DETAILS_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_BASIS_DETAILS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_MATRIX_BASIS_DETAILS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MATRIX_BASIS_DETAILS_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_MATRIX_READ_FROM_NODE_def MID_METHOD_MATRIX_READ_FROM_NODE = new MID_METHOD_MATRIX_READ_FROM_NODE_def();
			public class MID_METHOD_MATRIX_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MATRIX_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_METHOD_MATRIX_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_MATRIX_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MATRIX");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_METHOD_MATRIX_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_FORECAST_READ_def MID_METHOD_COPY_FORECAST_READ = new MID_METHOD_COPY_FORECAST_READ_def();
			public class MID_METHOD_COPY_FORECAST_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_FORECAST_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_FORECAST_READ_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_FORECAST_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_COPY_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_FORECAST_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_GROUP_LEVEL_READ_def MID_METHOD_COPY_GROUP_LEVEL_READ = new MID_METHOD_COPY_GROUP_LEVEL_READ_def();
			public class MID_METHOD_COPY_GROUP_LEVEL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_GROUP_LEVEL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_GROUP_LEVEL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_GROUP_LEVEL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_COPY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_GROUP_LEVEL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_BASIS_DETAIL_READ_def MID_METHOD_COPY_BASIS_DETAIL_READ = new MID_METHOD_COPY_BASIS_DETAIL_READ_def();
			public class MID_METHOD_COPY_BASIS_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_BASIS_DETAIL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_BASIS_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_BASIS_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_COPY_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_BASIS_DETAIL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_FORECAST_INSERT_def MID_METHOD_COPY_FORECAST_INSERT = new MID_METHOD_COPY_FORECAST_INSERT_def();
			public class MID_METHOD_COPY_FORECAST_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_FORECAST_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter PLAN_TYPE;
                private intParameter STORE_FILTER_RID;
                private intParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private intParameter OLL_RID;
                private charParameter PREINIT_VALUES_IND;
			
			    public MID_METHOD_COPY_FORECAST_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_FORECAST_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_COPY_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        PLAN_TYPE = new intParameter("@PLAN_TYPE", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new intParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        PREINIT_VALUES_IND = new charParameter("@PREINIT_VALUES_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      int? PLAN_TYPE,
			                      int? STORE_FILTER_RID,
			                      int? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      int? OLL_RID,
			                      char? PREINIT_VALUES_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_COPY_FORECAST_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.PLAN_TYPE.SetValue(PLAN_TYPE);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.PREINIT_VALUES_IND.SetValue(PREINIT_VALUES_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_FORECAST_UPDATE_def MID_METHOD_COPY_FORECAST_UPDATE = new MID_METHOD_COPY_FORECAST_UPDATE_def();
			public class MID_METHOD_COPY_FORECAST_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_FORECAST_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter PLAN_TYPE;
                private intParameter STORE_FILTER_RID;
                private intParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private intParameter OLL_RID;
                private charParameter PREINIT_VALUES_IND;
			
			    public MID_METHOD_COPY_FORECAST_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_FORECAST_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_COPY_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        PLAN_TYPE = new intParameter("@PLAN_TYPE", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new intParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        PREINIT_VALUES_IND = new charParameter("@PREINIT_VALUES_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      int? PLAN_TYPE,
			                      int? STORE_FILTER_RID,
			                      int? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      int? OLL_RID,
			                      char? PREINIT_VALUES_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_COPY_FORECAST_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.PLAN_TYPE.SetValue(PLAN_TYPE);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.PREINIT_VALUES_IND.SetValue(PREINIT_VALUES_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_GROUP_LEVEL_INSERT_def MID_METHOD_COPY_GROUP_LEVEL_INSERT = new MID_METHOD_COPY_GROUP_LEVEL_INSERT_def();
			public class MID_METHOD_COPY_GROUP_LEVEL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_GROUP_LEVEL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
			
			    public MID_METHOD_COPY_GROUP_LEVEL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_GROUP_LEVEL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_COPY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_COPY_GROUP_LEVEL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_BASIS_DETAIL_INSERT_def MID_METHOD_COPY_BASIS_DETAIL_INSERT = new MID_METHOD_COPY_BASIS_DETAIL_INSERT_def();
			public class MID_METHOD_COPY_BASIS_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_BASIS_DETAIL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private intParameter DETAIL_SEQ;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private floatParameter WEIGHT;
                private intParameter INCLUDE_EXCLUDE;
			
			    public MID_METHOD_COPY_BASIS_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_BASIS_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_COPY_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        DETAIL_SEQ = new intParameter("@DETAIL_SEQ", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			        INCLUDE_EXCLUDE = new intParameter("@INCLUDE_EXCLUDE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? DETAIL_SEQ,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      double? WEIGHT,
			                      int? INCLUDE_EXCLUDE
			                      )
			    {
                    lock (typeof(MID_METHOD_COPY_BASIS_DETAIL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.DETAIL_SEQ.SetValue(DETAIL_SEQ);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        this.INCLUDE_EXCLUDE.SetValue(INCLUDE_EXCLUDE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_FORECAST_DELETE_def MID_METHOD_COPY_FORECAST_DELETE = new MID_METHOD_COPY_FORECAST_DELETE_def();
			public class MID_METHOD_COPY_FORECAST_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_FORECAST_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_FORECAST_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_FORECAST_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_COPY_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_FORECAST_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_BASIS_DETAIL_DELETE_def MID_METHOD_COPY_BASIS_DETAIL_DELETE = new MID_METHOD_COPY_BASIS_DETAIL_DELETE_def();
			public class MID_METHOD_COPY_BASIS_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_BASIS_DETAIL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_BASIS_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_BASIS_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_COPY_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_BASIS_DETAIL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_GROUP_LEVEL_DELETE_def MID_METHOD_COPY_GROUP_LEVEL_DELETE = new MID_METHOD_COPY_GROUP_LEVEL_DELETE_def();
			public class MID_METHOD_COPY_GROUP_LEVEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_GROUP_LEVEL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_COPY_GROUP_LEVEL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_GROUP_LEVEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_COPY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_GROUP_LEVEL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_FORECAST_READ_FROM_NODE_def MID_METHOD_COPY_FORECAST_READ_FROM_NODE = new MID_METHOD_COPY_FORECAST_READ_FROM_NODE_def();
			public class MID_METHOD_COPY_FORECAST_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_FORECAST_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_METHOD_COPY_FORECAST_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_FORECAST_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_COPY_FORECAST");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_FORECAST_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE_def MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE = new MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE_def();
			public class MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_COPY_BASIS_DETAIL");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_READ_def MID_METHOD_EXPORT_READ = new MID_METHOD_EXPORT_READ_def();
			public class MID_METHOD_EXPORT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_EXPORT_READ_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_EXPORT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_EXPORT_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_VARIABLES_READ_def MID_METHOD_EXPORT_VARIABLES_READ = new MID_METHOD_EXPORT_VARIABLES_READ_def();
			public class MID_METHOD_EXPORT_VARIABLES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_VARIABLES_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_EXPORT_VARIABLES_READ_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_VARIABLES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_EXPORT_VARIABLES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_EXPORT_VARIABLES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_INSERT_def MID_METHOD_EXPORT_INSERT = new MID_METHOD_EXPORT_INSERT_def();
			public class MID_METHOD_EXPORT_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter PLAN_TYPE;
                private charParameter LOW_LEVELS_IND;
                private charParameter LOW_LEVELS_ONLY_IND;
                private intParameter LOW_LEVELS_TYPE;
                private intParameter LOW_LEVEL_SEQUENCE;
                private intParameter LOW_LEVEL_OFFSET;
                private charParameter SHOW_INELIGIBLE_IND;
                private charParameter USE_DEFAULT_SETTINGS_IND;
                private intParameter EXPORT_TYPE;
                private charParameter DELIMETER;
                private stringParameter CSV_FILE_EXTENSION;
                private intParameter DATE_TYPE;
                private charParameter PREINIT_VALUES_IND;
                private charParameter EXCLUDE_ZERO_VALUES_IND;
                private stringParameter FILE_PATH;
                private charParameter ADD_DATE_STAMP_IND;
                private charParameter ADD_TIME_STAMP_IND;
                private intParameter SPLIT_TYPE;
                private intParameter SPLIT_NUM_ENTRIES;
                private intParameter CONCURRENT_PROCESSES;
                private charParameter CREATE_FLAG_FILE_IND;
                private stringParameter FLAG_FILE_EXTENSION;
                private charParameter CREATE_END_FILE_IND;
                private stringParameter END_FILE_EXTENSION;
                private intParameter OLL_RID;
			
			    public MID_METHOD_EXPORT_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_EXPORT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        PLAN_TYPE = new intParameter("@PLAN_TYPE", base.inputParameterList);
			        LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
			        LOW_LEVELS_ONLY_IND = new charParameter("@LOW_LEVELS_ONLY_IND", base.inputParameterList);
			        LOW_LEVELS_TYPE = new intParameter("@LOW_LEVELS_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        SHOW_INELIGIBLE_IND = new charParameter("@SHOW_INELIGIBLE_IND", base.inputParameterList);
			        USE_DEFAULT_SETTINGS_IND = new charParameter("@USE_DEFAULT_SETTINGS_IND", base.inputParameterList);
			        EXPORT_TYPE = new intParameter("@EXPORT_TYPE", base.inputParameterList);
			        DELIMETER = new charParameter("@DELIMETER", base.inputParameterList);
			        CSV_FILE_EXTENSION = new stringParameter("@CSV_FILE_EXTENSION", base.inputParameterList);
			        DATE_TYPE = new intParameter("@DATE_TYPE", base.inputParameterList);
			        PREINIT_VALUES_IND = new charParameter("@PREINIT_VALUES_IND", base.inputParameterList);
			        EXCLUDE_ZERO_VALUES_IND = new charParameter("@EXCLUDE_ZERO_VALUES_IND", base.inputParameterList);
			        FILE_PATH = new stringParameter("@FILE_PATH", base.inputParameterList);
			        ADD_DATE_STAMP_IND = new charParameter("@ADD_DATE_STAMP_IND", base.inputParameterList);
			        ADD_TIME_STAMP_IND = new charParameter("@ADD_TIME_STAMP_IND", base.inputParameterList);
			        SPLIT_TYPE = new intParameter("@SPLIT_TYPE", base.inputParameterList);
			        SPLIT_NUM_ENTRIES = new intParameter("@SPLIT_NUM_ENTRIES", base.inputParameterList);
			        CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
			        CREATE_FLAG_FILE_IND = new charParameter("@CREATE_FLAG_FILE_IND", base.inputParameterList);
			        FLAG_FILE_EXTENSION = new stringParameter("@FLAG_FILE_EXTENSION", base.inputParameterList);
			        CREATE_END_FILE_IND = new charParameter("@CREATE_END_FILE_IND", base.inputParameterList);
			        END_FILE_EXTENSION = new stringParameter("@END_FILE_EXTENSION", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      int? STORE_FILTER_RID,
			                      int? PLAN_TYPE,
			                      char? LOW_LEVELS_IND,
			                      char? LOW_LEVELS_ONLY_IND,
			                      int? LOW_LEVELS_TYPE,
			                      int? LOW_LEVEL_SEQUENCE,
			                      int? LOW_LEVEL_OFFSET,
			                      char? SHOW_INELIGIBLE_IND,
			                      char? USE_DEFAULT_SETTINGS_IND,
			                      int? EXPORT_TYPE,
			                      char? DELIMETER,
			                      string CSV_FILE_EXTENSION,
			                      int? DATE_TYPE,
			                      char? PREINIT_VALUES_IND,
			                      char? EXCLUDE_ZERO_VALUES_IND,
			                      string FILE_PATH,
			                      char? ADD_DATE_STAMP_IND,
			                      char? ADD_TIME_STAMP_IND,
			                      int? SPLIT_TYPE,
			                      int? SPLIT_NUM_ENTRIES,
			                      int? CONCURRENT_PROCESSES,
			                      char? CREATE_FLAG_FILE_IND,
			                      string FLAG_FILE_EXTENSION,
			                      char? CREATE_END_FILE_IND,
			                      string END_FILE_EXTENSION,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_EXPORT_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.PLAN_TYPE.SetValue(PLAN_TYPE);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVELS_ONLY_IND.SetValue(LOW_LEVELS_ONLY_IND);
                        this.LOW_LEVELS_TYPE.SetValue(LOW_LEVELS_TYPE);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.SHOW_INELIGIBLE_IND.SetValue(SHOW_INELIGIBLE_IND);
                        this.USE_DEFAULT_SETTINGS_IND.SetValue(USE_DEFAULT_SETTINGS_IND);
                        this.EXPORT_TYPE.SetValue(EXPORT_TYPE);
                        this.DELIMETER.SetValue(DELIMETER);
                        this.CSV_FILE_EXTENSION.SetValue(CSV_FILE_EXTENSION);
                        this.DATE_TYPE.SetValue(DATE_TYPE);
                        this.PREINIT_VALUES_IND.SetValue(PREINIT_VALUES_IND);
                        this.EXCLUDE_ZERO_VALUES_IND.SetValue(EXCLUDE_ZERO_VALUES_IND);
                        this.FILE_PATH.SetValue(FILE_PATH);
                        this.ADD_DATE_STAMP_IND.SetValue(ADD_DATE_STAMP_IND);
                        this.ADD_TIME_STAMP_IND.SetValue(ADD_TIME_STAMP_IND);
                        this.SPLIT_TYPE.SetValue(SPLIT_TYPE);
                        this.SPLIT_NUM_ENTRIES.SetValue(SPLIT_NUM_ENTRIES);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.CREATE_FLAG_FILE_IND.SetValue(CREATE_FLAG_FILE_IND);
                        this.FLAG_FILE_EXTENSION.SetValue(FLAG_FILE_EXTENSION);
                        this.CREATE_END_FILE_IND.SetValue(CREATE_END_FILE_IND);
                        this.END_FILE_EXTENSION.SetValue(END_FILE_EXTENSION);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_UPDATE_def MID_METHOD_EXPORT_UPDATE = new MID_METHOD_EXPORT_UPDATE_def();
			public class MID_METHOD_EXPORT_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter PLAN_TYPE;
                private charParameter LOW_LEVELS_IND;
                private charParameter LOW_LEVELS_ONLY_IND;
                private intParameter LOW_LEVELS_TYPE;
                private intParameter LOW_LEVEL_SEQUENCE;
                private intParameter LOW_LEVEL_OFFSET;
                private charParameter SHOW_INELIGIBLE_IND;
                private charParameter USE_DEFAULT_SETTINGS_IND;
                private intParameter EXPORT_TYPE;
                private charParameter DELIMETER;
                private stringParameter CSV_FILE_EXTENSION;
                private intParameter DATE_TYPE;
                private charParameter PREINIT_VALUES_IND;
                private charParameter EXCLUDE_ZERO_VALUES_IND;
                private stringParameter FILE_PATH;
                private charParameter ADD_DATE_STAMP_IND;
                private charParameter ADD_TIME_STAMP_IND;
                private intParameter SPLIT_TYPE;
                private intParameter SPLIT_NUM_ENTRIES;
                private intParameter CONCURRENT_PROCESSES;
                private charParameter CREATE_FLAG_FILE_IND;
                private stringParameter FLAG_FILE_EXTENSION;
                private charParameter CREATE_END_FILE_IND;
                private stringParameter END_FILE_EXTENSION;
                private intParameter OLL_RID;
			
			    public MID_METHOD_EXPORT_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_EXPORT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        PLAN_TYPE = new intParameter("@PLAN_TYPE", base.inputParameterList);
			        LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
			        LOW_LEVELS_ONLY_IND = new charParameter("@LOW_LEVELS_ONLY_IND", base.inputParameterList);
			        LOW_LEVELS_TYPE = new intParameter("@LOW_LEVELS_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        SHOW_INELIGIBLE_IND = new charParameter("@SHOW_INELIGIBLE_IND", base.inputParameterList);
			        USE_DEFAULT_SETTINGS_IND = new charParameter("@USE_DEFAULT_SETTINGS_IND", base.inputParameterList);
			        EXPORT_TYPE = new intParameter("@EXPORT_TYPE", base.inputParameterList);
			        DELIMETER = new charParameter("@DELIMETER", base.inputParameterList);
			        CSV_FILE_EXTENSION = new stringParameter("@CSV_FILE_EXTENSION", base.inputParameterList);
			        DATE_TYPE = new intParameter("@DATE_TYPE", base.inputParameterList);
			        PREINIT_VALUES_IND = new charParameter("@PREINIT_VALUES_IND", base.inputParameterList);
			        EXCLUDE_ZERO_VALUES_IND = new charParameter("@EXCLUDE_ZERO_VALUES_IND", base.inputParameterList);
			        FILE_PATH = new stringParameter("@FILE_PATH", base.inputParameterList);
			        ADD_DATE_STAMP_IND = new charParameter("@ADD_DATE_STAMP_IND", base.inputParameterList);
			        ADD_TIME_STAMP_IND = new charParameter("@ADD_TIME_STAMP_IND", base.inputParameterList);
			        SPLIT_TYPE = new intParameter("@SPLIT_TYPE", base.inputParameterList);
			        SPLIT_NUM_ENTRIES = new intParameter("@SPLIT_NUM_ENTRIES", base.inputParameterList);
			        CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
			        CREATE_FLAG_FILE_IND = new charParameter("@CREATE_FLAG_FILE_IND", base.inputParameterList);
			        FLAG_FILE_EXTENSION = new stringParameter("@FLAG_FILE_EXTENSION", base.inputParameterList);
			        CREATE_END_FILE_IND = new charParameter("@CREATE_END_FILE_IND", base.inputParameterList);
			        END_FILE_EXTENSION = new stringParameter("@END_FILE_EXTENSION", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      int? STORE_FILTER_RID,
			                      int? PLAN_TYPE,
			                      char? LOW_LEVELS_IND,
			                      char? LOW_LEVELS_ONLY_IND,
			                      int? LOW_LEVELS_TYPE,
			                      int? LOW_LEVEL_SEQUENCE,
			                      int? LOW_LEVEL_OFFSET,
			                      char? SHOW_INELIGIBLE_IND,
			                      char? USE_DEFAULT_SETTINGS_IND,
			                      int? EXPORT_TYPE,
			                      char? DELIMETER,
			                      string CSV_FILE_EXTENSION,
			                      int? DATE_TYPE,
			                      char? PREINIT_VALUES_IND,
			                      char? EXCLUDE_ZERO_VALUES_IND,
			                      string FILE_PATH,
			                      char? ADD_DATE_STAMP_IND,
			                      char? ADD_TIME_STAMP_IND,
			                      int? SPLIT_TYPE,
			                      int? SPLIT_NUM_ENTRIES,
			                      int? CONCURRENT_PROCESSES,
			                      char? CREATE_FLAG_FILE_IND,
			                      string FLAG_FILE_EXTENSION,
			                      char? CREATE_END_FILE_IND,
			                      string END_FILE_EXTENSION,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_EXPORT_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.PLAN_TYPE.SetValue(PLAN_TYPE);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVELS_ONLY_IND.SetValue(LOW_LEVELS_ONLY_IND);
                        this.LOW_LEVELS_TYPE.SetValue(LOW_LEVELS_TYPE);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.SHOW_INELIGIBLE_IND.SetValue(SHOW_INELIGIBLE_IND);
                        this.USE_DEFAULT_SETTINGS_IND.SetValue(USE_DEFAULT_SETTINGS_IND);
                        this.EXPORT_TYPE.SetValue(EXPORT_TYPE);
                        this.DELIMETER.SetValue(DELIMETER);
                        this.CSV_FILE_EXTENSION.SetValue(CSV_FILE_EXTENSION);
                        this.DATE_TYPE.SetValue(DATE_TYPE);
                        this.PREINIT_VALUES_IND.SetValue(PREINIT_VALUES_IND);
                        this.EXCLUDE_ZERO_VALUES_IND.SetValue(EXCLUDE_ZERO_VALUES_IND);
                        this.FILE_PATH.SetValue(FILE_PATH);
                        this.ADD_DATE_STAMP_IND.SetValue(ADD_DATE_STAMP_IND);
                        this.ADD_TIME_STAMP_IND.SetValue(ADD_TIME_STAMP_IND);
                        this.SPLIT_TYPE.SetValue(SPLIT_TYPE);
                        this.SPLIT_NUM_ENTRIES.SetValue(SPLIT_NUM_ENTRIES);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.CREATE_FLAG_FILE_IND.SetValue(CREATE_FLAG_FILE_IND);
                        this.FLAG_FILE_EXTENSION.SetValue(FLAG_FILE_EXTENSION);
                        this.CREATE_END_FILE_IND.SetValue(CREATE_END_FILE_IND);
                        this.END_FILE_EXTENSION.SetValue(END_FILE_EXTENSION);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_DELETE_def MID_METHOD_EXPORT_DELETE = new MID_METHOD_EXPORT_DELETE_def();
			public class MID_METHOD_EXPORT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_EXPORT_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_EXPORT");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_EXPORT_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_READ_FROM_NODE_def MID_METHOD_EXPORT_READ_FROM_NODE = new MID_METHOD_EXPORT_READ_FROM_NODE_def();
			public class MID_METHOD_EXPORT_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_METHOD_EXPORT_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_EXPORT");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_METHOD_EXPORT_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_VARIABLES_DELETE_def MID_METHOD_EXPORT_VARIABLES_DELETE = new MID_METHOD_EXPORT_VARIABLES_DELETE_def();
			public class MID_METHOD_EXPORT_VARIABLES_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_VARIABLES_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_EXPORT_VARIABLES_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_VARIABLES_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_EXPORT_VARIABLES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_EXPORT_VARIABLES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_EXPORT_VARIABLES_INSERT_def MID_METHOD_EXPORT_VARIABLES_INSERT = new MID_METHOD_EXPORT_VARIABLES_INSERT_def();
			public class MID_METHOD_EXPORT_VARIABLES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_EXPORT_VARIABLES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter VARIABLE_RID;
                private intParameter VARIABLE_SEQ;
			
			    public MID_METHOD_EXPORT_VARIABLES_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_EXPORT_VARIABLES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_EXPORT_VARIABLES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        VARIABLE_RID = new intParameter("@VARIABLE_RID", base.inputParameterList);
			        VARIABLE_SEQ = new intParameter("@VARIABLE_SEQ", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? VARIABLE_RID,
			                      int? VARIABLE_SEQ
			                      )
			    {
                    lock (typeof(MID_METHOD_EXPORT_VARIABLES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.VARIABLE_RID.SetValue(VARIABLE_RID);
                        this.VARIABLE_SEQ.SetValue(VARIABLE_SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            // Begin TT#2131-MD - JSmith - Halo Integration
            public static MID_METHOD_PLANNING_EXTRACT_READ_def MID_METHOD_PLANNING_EXTRACT_READ = new MID_METHOD_PLANNING_EXTRACT_READ_def();
            public class MID_METHOD_PLANNING_EXTRACT_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_PLANNING_EXTRACT_READ_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ_def MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ = new MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ_def();
            public class MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT_VARIABLES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_INSERT_def MID_METHOD_PLANNING_EXTRACT_INSERT = new MID_METHOD_PLANNING_EXTRACT_INSERT_def();
            public class MID_METHOD_PLANNING_EXTRACT_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private charParameter CHAIN_IND;
                private charParameter STORE_IND;
                private charParameter ATTRIBUTE_SET_IND;
                private intParameter ATTRIBUTE_RID;
                private charParameter LOW_LEVELS_IND;
                private charParameter LOW_LEVELS_ONLY_IND;
                private intParameter LOW_LEVELS_TYPE;
                private intParameter LOW_LEVEL_SEQUENCE;
                private intParameter LOW_LEVEL_OFFSET;
                private charParameter SHOW_INELIGIBLE_IND;
                private charParameter EXCLUDE_ZERO_VALUES_IND;
                private intParameter CONCURRENT_PROCESSES;
                private intParameter OLL_RID;
                private datetimeParameter UPDATE_DATE;
                private datetimeParameter EXTRACT_DATE;

                public MID_METHOD_PLANNING_EXTRACT_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
                    CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
                    STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
                    ATTRIBUTE_SET_IND = new charParameter("@ATTRIBUTE_SET_IND", base.inputParameterList);
                    ATTRIBUTE_RID = new intParameter("@ATTRIBUTE_RID", base.inputParameterList);
                    LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
                    LOW_LEVELS_ONLY_IND = new charParameter("@LOW_LEVELS_ONLY_IND", base.inputParameterList);
                    LOW_LEVELS_TYPE = new intParameter("@LOW_LEVELS_TYPE", base.inputParameterList);
                    LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
                    LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
                    SHOW_INELIGIBLE_IND = new charParameter("@SHOW_INELIGIBLE_IND", base.inputParameterList);
                    EXCLUDE_ZERO_VALUES_IND = new charParameter("@EXCLUDE_ZERO_VALUES_IND", base.inputParameterList);
                    CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
                    OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    UPDATE_DATE = new datetimeParameter("@UPDATE_DATE", base.inputParameterList);
                    EXTRACT_DATE = new datetimeParameter("@EXTRACT_DATE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? HN_RID,
                                  int? FV_RID,
                                  int? CDR_RID,
                                  int? STORE_FILTER_RID,
                                  char? CHAIN_IND,
                                  char? STORE_IND,
                                  char? ATTRIBUTE_SET_IND,
                                  int? ATTRIBUTE_RID,
                                  char? LOW_LEVELS_IND,
                                  char? LOW_LEVELS_ONLY_IND,
                                  int? LOW_LEVELS_TYPE,
                                  int? LOW_LEVEL_SEQUENCE,
                                  int? LOW_LEVEL_OFFSET,
                                  char? SHOW_INELIGIBLE_IND,
                                  char? EXCLUDE_ZERO_VALUES_IND,
                                  int? CONCURRENT_PROCESSES,
                                  int? OLL_RID,
                                  DateTime? UPDATE_DATE,
                                  DateTime? EXTRACT_DATE
                                  )
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.ATTRIBUTE_SET_IND.SetValue(ATTRIBUTE_SET_IND);
                        this.ATTRIBUTE_RID.SetValue(ATTRIBUTE_RID);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVELS_ONLY_IND.SetValue(LOW_LEVELS_ONLY_IND);
                        this.LOW_LEVELS_TYPE.SetValue(LOW_LEVELS_TYPE);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.SHOW_INELIGIBLE_IND.SetValue(SHOW_INELIGIBLE_IND);
                        this.EXCLUDE_ZERO_VALUES_IND.SetValue(EXCLUDE_ZERO_VALUES_IND);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.UPDATE_DATE.SetValue(UPDATE_DATE);
                        this.EXTRACT_DATE.SetValue(EXTRACT_DATE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_UPDATE_def MID_METHOD_PLANNING_EXTRACT_UPDATE = new MID_METHOD_PLANNING_EXTRACT_UPDATE_def();
            public class MID_METHOD_PLANNING_EXTRACT_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private charParameter CHAIN_IND;
                private charParameter STORE_IND;
                private charParameter ATTRIBUTE_SET_IND;
                private intParameter ATTRIBUTE_RID;
                private charParameter LOW_LEVELS_IND;
                private charParameter LOW_LEVELS_ONLY_IND;
                private intParameter LOW_LEVELS_TYPE;
                private intParameter LOW_LEVEL_SEQUENCE;
                private intParameter LOW_LEVEL_OFFSET;
                private charParameter SHOW_INELIGIBLE_IND;
                private charParameter EXCLUDE_ZERO_VALUES_IND;
                private intParameter CONCURRENT_PROCESSES;
                private intParameter OLL_RID;
                private datetimeParameter UPDATE_DATE;
                private datetimeParameter EXTRACT_DATE;

                public MID_METHOD_PLANNING_EXTRACT_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
                    CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
                    STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
                    ATTRIBUTE_SET_IND = new charParameter("@ATTRIBUTE_SET_IND", base.inputParameterList);
                    ATTRIBUTE_RID = new intParameter("@ATTRIBUTE_RID", base.inputParameterList);
                    LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
                    LOW_LEVELS_ONLY_IND = new charParameter("@LOW_LEVELS_ONLY_IND", base.inputParameterList);
                    LOW_LEVELS_TYPE = new intParameter("@LOW_LEVELS_TYPE", base.inputParameterList);
                    LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
                    LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
                    SHOW_INELIGIBLE_IND = new charParameter("@SHOW_INELIGIBLE_IND", base.inputParameterList);
                    EXCLUDE_ZERO_VALUES_IND = new charParameter("@EXCLUDE_ZERO_VALUES_IND", base.inputParameterList);
                    CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
                    OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    UPDATE_DATE = new datetimeParameter("@UPDATE_DATE", base.inputParameterList);
                    EXTRACT_DATE = new datetimeParameter("@EXTRACT_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? HN_RID,
                                  int? FV_RID,
                                  int? CDR_RID,
                                  int? STORE_FILTER_RID,
                                  char? CHAIN_IND,
                                  char? STORE_IND,
                                  char? ATTRIBUTE_SET_IND,
                                  int? ATTRIBUTE_RID,
                                  char? LOW_LEVELS_IND,
                                  char? LOW_LEVELS_ONLY_IND,
                                  int? LOW_LEVELS_TYPE,
                                  int? LOW_LEVEL_SEQUENCE,
                                  int? LOW_LEVEL_OFFSET,
                                  char? SHOW_INELIGIBLE_IND,
                                  char? EXCLUDE_ZERO_VALUES_IND,
                                  int? CONCURRENT_PROCESSES,
                                  int? OLL_RID,
                                  DateTime? UPDATE_DATE,
                                  DateTime? EXTRACT_DATE
                                  )
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.ATTRIBUTE_SET_IND.SetValue(ATTRIBUTE_SET_IND);
                        this.ATTRIBUTE_RID.SetValue(ATTRIBUTE_RID);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVELS_ONLY_IND.SetValue(LOW_LEVELS_ONLY_IND);
                        this.LOW_LEVELS_TYPE.SetValue(LOW_LEVELS_TYPE);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.SHOW_INELIGIBLE_IND.SetValue(SHOW_INELIGIBLE_IND);
                        this.EXCLUDE_ZERO_VALUES_IND.SetValue(EXCLUDE_ZERO_VALUES_IND);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.UPDATE_DATE.SetValue(UPDATE_DATE);
                        this.EXTRACT_DATE.SetValue(EXTRACT_DATE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE_def MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE = new MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE_def();
            public class MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_UPDATE.SQL"

                private intParameter METHOD_RID;
                private datetimeParameter EXTRACT_DATE;

                public MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    EXTRACT_DATE = new datetimeParameter("@EXTRACT_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  DateTime? EXTRACT_DATE
                                  )
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.EXTRACT_DATE.SetValue(EXTRACT_DATE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_DELETE_def MID_METHOD_PLANNING_EXTRACT_DELETE = new MID_METHOD_PLANNING_EXTRACT_DELETE_def();
            public class MID_METHOD_PLANNING_EXTRACT_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_PLANNING_EXTRACT_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE_def MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE = new MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE_def();
            public class MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE.SQL"

                private intParameter HN_RID;

                public MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE_def MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE = new MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE_def();
            public class MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT_VARIABLES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT_def MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT = new MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT_def();
            public class MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter VARIABLE_RID;
                private intParameter VARIABLE_TYPE;
                private intParameter VARIABLE_SEQ;

                public MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_PLANNING_EXTRACT_VARIABLES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    VARIABLE_RID = new intParameter("@VARIABLE_RID", base.inputParameterList);
                    VARIABLE_TYPE = new intParameter("@VARIABLE_TYPE", base.inputParameterList);
                    VARIABLE_SEQ = new intParameter("@VARIABLE_SEQ", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? VARIABLE_RID,
                                  int? VARIABLE_TYPE,
                                  int? VARIABLE_SEQ
                                  )
                {
                    lock (typeof(MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.VARIABLE_RID.SetValue(VARIABLE_RID);
                        this.VARIABLE_TYPE.SetValue(VARIABLE_TYPE);
                        this.VARIABLE_SEQ.SetValue(VARIABLE_SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            // End TT#2131-MD - JSmith - Halo Integration

            public static MID_METHOD_MOD_SALES_READ_def MID_METHOD_MOD_SALES_READ = new MID_METHOD_MOD_SALES_READ_def();
			public class MID_METHOD_MOD_SALES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MOD_SALES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_GRADE_READ_def MID_METHOD_MOD_SALES_GRADE_READ = new MID_METHOD_MOD_SALES_GRADE_READ_def();
			public class MID_METHOD_MOD_SALES_GRADE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_GRADE_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_GRADE_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_GRADE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MOD_SALES_GRADE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_GRADE_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_SELL_THRU_READ_def MID_METHOD_MOD_SALES_SELL_THRU_READ = new MID_METHOD_MOD_SALES_SELL_THRU_READ_def();
			public class MID_METHOD_MOD_SALES_SELL_THRU_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_SELL_THRU_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_SELL_THRU_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_SELL_THRU_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MOD_SALES_SELL_THRU");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_SELL_THRU_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_MATRIX_READ_def MID_METHOD_MOD_SALES_MATRIX_READ = new MID_METHOD_MOD_SALES_MATRIX_READ_def();
			public class MID_METHOD_MOD_SALES_MATRIX_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_MATRIX_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_MATRIX_READ_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_MATRIX_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_MOD_SALES_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_MATRIX_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_INSERT_def MID_METHOD_MOD_SALES_INSERT = new MID_METHOD_MOD_SALES_INSERT_def();
			public class MID_METHOD_MOD_SALES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter AVERAGE_STORE;
			
			    public MID_METHOD_MOD_SALES_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MOD_SALES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        AVERAGE_STORE = new intParameter("@AVERAGE_STORE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? CDR_RID,
			                      int? STORE_FILTER_RID,
			                      int? AVERAGE_STORE
			                      )
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.AVERAGE_STORE.SetValue(AVERAGE_STORE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_GRADE_INSERT_def MID_METHOD_MOD_SALES_GRADE_INSERT = new MID_METHOD_MOD_SALES_GRADE_INSERT_def();
			public class MID_METHOD_MOD_SALES_GRADE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_GRADE_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;
			
			    public MID_METHOD_MOD_SALES_GRADE_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_GRADE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MOD_SALES_GRADE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BOUNDARY,
			                      string GRADE_CODE
			                      )
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_GRADE_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_SELL_THRU_INSERT_def MID_METHOD_MOD_SALES_SELL_THRU_INSERT = new MID_METHOD_MOD_SALES_SELL_THRU_INSERT_def();
			public class MID_METHOD_MOD_SALES_SELL_THRU_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_SELL_THRU_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SELL_THRU;
			
			    public MID_METHOD_MOD_SALES_SELL_THRU_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_SELL_THRU_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MOD_SALES_SELL_THRU");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SELL_THRU = new intParameter("@SELL_THRU", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SELL_THRU
			                      )
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_SELL_THRU_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SELL_THRU.SetValue(SELL_THRU);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_MATRIX_INSERT_def MID_METHOD_MOD_SALES_MATRIX_INSERT = new MID_METHOD_MOD_SALES_MATRIX_INSERT_def();
			public class MID_METHOD_MOD_SALES_MATRIX_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_MATRIX_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private intParameter BOUNDARY;
                private intParameter SELL_THRU;
                private intParameter MATRIX_RULE;
                private floatParameter MATRIX_RULE_QUANTITY;
			
			    public MID_METHOD_MOD_SALES_MATRIX_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_MATRIX_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_MOD_SALES_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        SELL_THRU = new intParameter("@SELL_THRU", base.inputParameterList);
			        MATRIX_RULE = new intParameter("@MATRIX_RULE", base.inputParameterList);
			        MATRIX_RULE_QUANTITY = new floatParameter("@MATRIX_RULE_QUANTITY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? BOUNDARY,
			                      int? SELL_THRU,
			                      int? MATRIX_RULE,
			                      double? MATRIX_RULE_QUANTITY
			                      )
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_MATRIX_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.SELL_THRU.SetValue(SELL_THRU);
                        this.MATRIX_RULE.SetValue(MATRIX_RULE);
                        this.MATRIX_RULE_QUANTITY.SetValue(MATRIX_RULE_QUANTITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_UPDATE_def MID_METHOD_MOD_SALES_UPDATE = new MID_METHOD_MOD_SALES_UPDATE_def();
			public class MID_METHOD_MOD_SALES_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter CDR_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter AVERAGE_STORE;
			
			    public MID_METHOD_MOD_SALES_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_MOD_SALES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        AVERAGE_STORE = new intParameter("@AVERAGE_STORE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? CDR_RID,
			                      int? STORE_FILTER_RID,
			                      int? AVERAGE_STORE
			                      )
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.AVERAGE_STORE.SetValue(AVERAGE_STORE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_DELETE_def MID_METHOD_MOD_SALES_DELETE = new MID_METHOD_MOD_SALES_DELETE_def();
			public class MID_METHOD_MOD_SALES_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_MOD_SALES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_MOD_SALES_DELETE_CHILD_DATA_def MID_METHOD_MOD_SALES_DELETE_CHILD_DATA = new MID_METHOD_MOD_SALES_DELETE_CHILD_DATA_def();
			public class MID_METHOD_MOD_SALES_DELETE_CHILD_DATA_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_MOD_SALES_DELETE_CHILD_DATA.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_MOD_SALES_DELETE_CHILD_DATA_def()
			    {
			        base.procedureName = "MID_METHOD_MOD_SALES_DELETE_CHILD_DATA";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_MOD_SALES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_MOD_SALES_DELETE_CHILD_DATA_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_FORECAST_READ_def MID_METHOD_SPREAD_FORECAST_READ = new MID_METHOD_SPREAD_FORECAST_READ_def();
			public class MID_METHOD_SPREAD_FORECAST_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_FORECAST_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SPREAD_FORECAST_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_FORECAST_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SPREAD_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SPREAD_FORECAST_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_BASIS_DETAIL_READ_def MID_METHOD_SPREAD_BASIS_DETAIL_READ = new MID_METHOD_SPREAD_BASIS_DETAIL_READ_def();
			public class MID_METHOD_SPREAD_BASIS_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_BASIS_DETAIL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SPREAD_BASIS_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_BASIS_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SPREAD_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SPREAD_BASIS_DETAIL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_FORECAST_INSERT_def MID_METHOD_SPREAD_FORECAST_INSERT = new MID_METHOD_SPREAD_FORECAST_INSERT_def();
			public class MID_METHOD_SPREAD_FORECAST_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_FORECAST_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter SPREAD_OPTION;
                private charParameter IGNORE_LOCKS;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter EQUALIZE_WEIGHTING;
                private intParameter OLL_RID;
			
			    public MID_METHOD_SPREAD_FORECAST_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_FORECAST_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_SPREAD_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        SPREAD_OPTION = new intParameter("@SPREAD_OPTION", base.inputParameterList);
			        IGNORE_LOCKS = new charParameter("@IGNORE_LOCKS", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        EQUALIZE_WEIGHTING = new charParameter("@EQUALIZE_WEIGHTING", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? HN_RID,
			                          int? FV_RID,
			                          int? CDR_RID,
			                          int? SPREAD_OPTION,
			                          char? IGNORE_LOCKS,
			                          char? MULTI_LEVEL_IND,
			                          int? FROM_LEVEL_TYPE,
			                          int? FROM_LEVEL_SEQ,
			                          int? FROM_LEVEL_OFFSET,
			                          int? TO_LEVEL_TYPE,
			                          int? TO_LEVEL_SEQ,
			                          int? TO_LEVEL_OFFSET,
			                          char? EQUALIZE_WEIGHTING,
			                          int? OLL_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_SPREAD_FORECAST_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.SPREAD_OPTION.SetValue(SPREAD_OPTION);
                        this.IGNORE_LOCKS.SetValue(IGNORE_LOCKS);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.EQUALIZE_WEIGHTING.SetValue(EQUALIZE_WEIGHTING);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_FORECAST_UPDATE_def MID_METHOD_SPREAD_FORECAST_UPDATE = new MID_METHOD_SPREAD_FORECAST_UPDATE_def();
			public class MID_METHOD_SPREAD_FORECAST_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_FORECAST_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private intParameter SPREAD_OPTION;
                private charParameter IGNORE_LOCKS;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter EQUALIZE_WEIGHTING;
                private intParameter OLL_RID;
			
			    public MID_METHOD_SPREAD_FORECAST_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_FORECAST_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_SPREAD_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        SPREAD_OPTION = new intParameter("@SPREAD_OPTION", base.inputParameterList);
			        IGNORE_LOCKS = new charParameter("@IGNORE_LOCKS", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        EQUALIZE_WEIGHTING = new charParameter("@EQUALIZE_WEIGHTING", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? HN_RID,
			                          int? FV_RID,
			                          int? CDR_RID,
			                          int? SPREAD_OPTION,
			                          char? IGNORE_LOCKS,
			                          char? MULTI_LEVEL_IND,
			                          int? FROM_LEVEL_TYPE,
			                          int? FROM_LEVEL_SEQ,
			                          int? FROM_LEVEL_OFFSET,
			                          int? TO_LEVEL_TYPE,
			                          int? TO_LEVEL_SEQ,
			                          int? TO_LEVEL_OFFSET,
			                          char? EQUALIZE_WEIGHTING,
			                          int? OLL_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_SPREAD_FORECAST_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.SPREAD_OPTION.SetValue(SPREAD_OPTION);
                        this.IGNORE_LOCKS.SetValue(IGNORE_LOCKS);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.EQUALIZE_WEIGHTING.SetValue(EQUALIZE_WEIGHTING);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_BASIS_DETAIL_INSERT_def MID_METHOD_SPREAD_BASIS_DETAIL_INSERT = new MID_METHOD_SPREAD_BASIS_DETAIL_INSERT_def();
			public class MID_METHOD_SPREAD_BASIS_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_BASIS_DETAIL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter DETAIL_SEQ;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private floatParameter WEIGHT;
			
			    public MID_METHOD_SPREAD_BASIS_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_BASIS_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_SPREAD_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        DETAIL_SEQ = new intParameter("@DETAIL_SEQ", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? DETAIL_SEQ,
			                          int? FV_RID,
			                          int? CDR_RID,
			                          double? WEIGHT
			                          )
			    {
                    lock (typeof(MID_METHOD_SPREAD_BASIS_DETAIL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.DETAIL_SEQ.SetValue(DETAIL_SEQ);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_FORECAST_DELETE_def MID_METHOD_SPREAD_FORECAST_DELETE = new MID_METHOD_SPREAD_FORECAST_DELETE_def();
			public class MID_METHOD_SPREAD_FORECAST_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_FORECAST_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SPREAD_FORECAST_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_FORECAST_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_SPREAD_FORECAST");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SPREAD_FORECAST_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_SPREAD_BASIS_DETAIL_DELETE_def MID_METHOD_SPREAD_BASIS_DETAIL_DELETE = new MID_METHOD_SPREAD_BASIS_DETAIL_DELETE_def();
			public class MID_METHOD_SPREAD_BASIS_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_BASIS_DETAIL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SPREAD_BASIS_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_BASIS_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_SPREAD_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SPREAD_BASIS_DETAIL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba); 
                    }
			    }
			}

			public static MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE_def MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE = new MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE_def();
			public class MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SPREAD_FORECAST");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba); 
                    }
			    }
			}

			public static MID_METHOD_GENERAL_ALLOCATION_READ_def MID_METHOD_GENERAL_ALLOCATION_READ = new MID_METHOD_GENERAL_ALLOCATION_READ_def();
			public class MID_METHOD_GENERAL_ALLOCATION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GENERAL_ALLOCATION_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GENERAL_ALLOCATION_READ_def()
			    {
			        base.procedureName = "MID_METHOD_GENERAL_ALLOCATION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GENERAL_ALLOCATION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GENERAL_ALLOCATION_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_GENERAL_ALLOCATION_INSERT_def MID_METHOD_GENERAL_ALLOCATION_INSERT = new MID_METHOD_GENERAL_ALLOCATION_INSERT_def();
			public class MID_METHOD_GENERAL_ALLOCATION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GENERAL_ALLOCATION_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BEGIN_CDR_RID;
                private intParameter SHIP_TO_CDR_RID;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter GEN_ALLOC_HDR_RID;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_TYPE;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
			
			    public MID_METHOD_GENERAL_ALLOCATION_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_GENERAL_ALLOCATION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_GENERAL_ALLOCATION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
			        SHIP_TO_CDR_RID = new intParameter("@SHIP_TO_CDR_RID", base.inputParameterList);
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			        MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
			        MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
			        GEN_ALLOC_HDR_RID = new intParameter("@GEN_ALLOC_HDR_RID", base.inputParameterList);
			        RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
			        PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
			        MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
			        RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
			        RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BEGIN_CDR_RID,
			                      int? SHIP_TO_CDR_RID,
			                      int? MERCH_HN_RID,
			                      int? MERCH_PH_RID,
			                      int? MERCH_PHL_SEQUENCE,
			                      int? GEN_ALLOC_HDR_RID,
			                      double? RESERVE,
			                      char? PERCENT_IND,
			                      int? MERCH_TYPE,
			                      double? RESERVE_AS_BULK,
			                      double? RESERVE_AS_PACKS
			                      )
			    {
                    lock (typeof(MID_METHOD_GENERAL_ALLOCATION_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.SHIP_TO_CDR_RID.SetValue(SHIP_TO_CDR_RID);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.GEN_ALLOC_HDR_RID.SetValue(GEN_ALLOC_HDR_RID);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_GENERAL_ALLOCATION_UPDATE_def MID_METHOD_GENERAL_ALLOCATION_UPDATE = new MID_METHOD_GENERAL_ALLOCATION_UPDATE_def();
			public class MID_METHOD_GENERAL_ALLOCATION_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GENERAL_ALLOCATION_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter BEGIN_CDR_RID;
                private intParameter SHIP_TO_CDR_RID;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter GEN_ALLOC_HDR_RID;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_TYPE;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
			
			    public MID_METHOD_GENERAL_ALLOCATION_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_GENERAL_ALLOCATION_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_GENERAL_ALLOCATION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
			        SHIP_TO_CDR_RID = new intParameter("@SHIP_TO_CDR_RID", base.inputParameterList);
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			        MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
			        MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
			        GEN_ALLOC_HDR_RID = new intParameter("@GEN_ALLOC_HDR_RID", base.inputParameterList);
			        RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
			        PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
			        MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
			        RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
			        RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BEGIN_CDR_RID,
			                      int? SHIP_TO_CDR_RID,
			                      int? MERCH_HN_RID,
			                      int? MERCH_PH_RID,
			                      int? MERCH_PHL_SEQUENCE,
			                      int? GEN_ALLOC_HDR_RID,
			                      double? RESERVE,
			                      char? PERCENT_IND,
			                      int? MERCH_TYPE,
			                      double? RESERVE_AS_BULK,
			                      double? RESERVE_AS_PACKS
			                      )
			    {
                    lock (typeof(MID_METHOD_GENERAL_ALLOCATION_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.SHIP_TO_CDR_RID.SetValue(SHIP_TO_CDR_RID);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.GEN_ALLOC_HDR_RID.SetValue(GEN_ALLOC_HDR_RID);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_GENERAL_ALLOCATION_DELETE_def MID_METHOD_GENERAL_ALLOCATION_DELETE = new MID_METHOD_GENERAL_ALLOCATION_DELETE_def();
			public class MID_METHOD_GENERAL_ALLOCATION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GENERAL_ALLOCATION_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GENERAL_ALLOCATION_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_GENERAL_ALLOCATION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GENERAL_ALLOCATION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GENERAL_ALLOCATION_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE_def MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE = new MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE_def();
			public class MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE.SQL"

                private intParameter MERCH_HN_RID;
			
			    public MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GENERAL_ALLOCATION");
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? MERCH_HN_RID)
			    {
                    lock (typeof(MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE_def))
                    {
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_READ_def MID_METHOD_GLOBAL_LOCK_READ = new MID_METHOD_GLOBAL_LOCK_READ_def();
			public class MID_METHOD_GLOBAL_LOCK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_READ_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ_def MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ = new MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ_def();
			public class MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_INSERT_def MID_METHOD_GLOBAL_LOCK_INSERT = new MID_METHOD_GLOBAL_LOCK_INSERT_def();
			public class MID_METHOD_GLOBAL_LOCK_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter STORE_IND;
                private charParameter CHAIN_IND;
                private intParameter STORE_FILTER_RID;
                private intParameter OLL_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      char? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      char? STORE_IND,
			                      char? CHAIN_IND,
			                      int? STORE_FILTER_RID,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_UPDATE_def MID_METHOD_GLOBAL_LOCK_UPDATE = new MID_METHOD_GLOBAL_LOCK_UPDATE_def();
			public class MID_METHOD_GLOBAL_LOCK_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter STORE_IND;
                private charParameter CHAIN_IND;
                private intParameter STORE_FILTER_RID;
                private intParameter OLL_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      char? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      char? STORE_IND,
			                      char? CHAIN_IND,
			                      int? STORE_FILTER_RID,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT_def MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT = new MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT_def();
			public class MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE_def MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE = new MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE_def();
			public class MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_LOCK_DELETE_def MID_METHOD_GLOBAL_LOCK_DELETE = new MID_METHOD_GLOBAL_LOCK_DELETE_def();
			public class MID_METHOD_GLOBAL_LOCK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_LOCK_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_LOCK_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_LOCK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GLOBAL_LOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_LOCK_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_FUNCTION_READ_ALL_def MID_GROUP_LEVEL_FUNCTION_READ_ALL = new MID_GROUP_LEVEL_FUNCTION_READ_ALL_def();
			public class MID_GROUP_LEVEL_FUNCTION_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_FUNCTION_READ_ALL.SQL"

                private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_FUNCTION_READ_ALL_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_FUNCTION_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_GROUP_LEVEL_FUNCTION_READ_ALL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE_def MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE = new MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE_def();
			public class MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE.SQL"

                private intParameter SEASON_HN_RID;
			
			    public MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_FUNCTION");
			        SEASON_HN_RID = new intParameter("@SEASON_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SEASON_HN_RID)
			    {
                    lock (typeof(MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE_def))
                    {
                        this.SEASON_HN_RID.SetValue(SEASON_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_READ_FROM_NODE_def MID_METHOD_OVERRIDE_READ_FROM_NODE = new MID_METHOD_OVERRIDE_READ_FROM_NODE_def();
			public class MID_METHOD_OVERRIDE_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_READ_FROM_NODE.SQL"

                private intParameter MERCH_HN_RID;
			
			    public MID_METHOD_OVERRIDE_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? MERCH_HN_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_READ_FROM_NODE_def))
                    {
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE_def MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE = new MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE_def();
			public class MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE.SQL"

                private intParameter ON_HAND_HN_RID;
			
			    public MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? ON_HAND_HN_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_READ_FROM_ONHAND_NODE_def))
                    {
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE_def MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE = new MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE_def();
			public class MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_COLOR_MINMAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_COLOR_MINMAX_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE_def MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE = new MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_VALUES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE_def MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE = new MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_BOUNDARY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_CAPACITY_DELETE_def MID_METHOD_OVERRIDE_CAPACITY_DELETE = new MID_METHOD_OVERRIDE_CAPACITY_DELETE_def();
			public class MID_METHOD_OVERRIDE_CAPACITY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_CAPACITY_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_CAPACITY_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_CAPACITY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_CAPACITY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_CAPACITY_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE_def MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE = new MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE_def();
			public class MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_PACK_ROUNDING");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_PACK_ROUNDING_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_DELETE_def MID_METHOD_OVERRIDE_DELETE = new MID_METHOD_OVERRIDE_DELETE_def();
			public class MID_METHOD_OVERRIDE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_UPDATE_def MID_METHOD_OVERRIDE_UPDATE = new MID_METHOD_OVERRIDE_UPDATE_def();
			public class MID_METHOD_OVERRIDE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter STORE_GRADE_TIMEFRAME;
                private charParameter EXCEED_MAX_IND;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter ON_HAND_HN_RID;
                private intParameter ON_HAND_PH_RID;
                private intParameter ON_HAND_PHL_SEQUENCE;
                private floatParameter ON_HAND_FACTOR;
                private intParameter COLOR_MULT;
                private intParameter SIZE_MULT;
                private intParameter ALL_COLOR_MIN;
                private intParameter ALL_COLOR_MAX;
                private intParameter SG_RID;
                private charParameter EXCEED_CAPACITY;
                private charParameter MERCH_UNSPECIFIED;
                private charParameter ON_HAND_UNSPECIFIED;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
                private intParameter STORE_GRADES_SG_RID;
                private charParameter INVENTORY_IND;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter IMO_SG_RID;
			
			    public MID_METHOD_OVERRIDE_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        STORE_GRADE_TIMEFRAME = new intParameter("@STORE_GRADE_TIMEFRAME", base.inputParameterList);
			        EXCEED_MAX_IND = new charParameter("@EXCEED_MAX_IND", base.inputParameterList);
			        PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
			        RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
			        PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			        MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
			        MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
			        ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
			        ON_HAND_PH_RID = new intParameter("@ON_HAND_PH_RID", base.inputParameterList);
			        ON_HAND_PHL_SEQUENCE = new intParameter("@ON_HAND_PHL_SEQUENCE", base.inputParameterList);
			        ON_HAND_FACTOR = new floatParameter("@ON_HAND_FACTOR", base.inputParameterList);
			        COLOR_MULT = new intParameter("@COLOR_MULT", base.inputParameterList);
			        SIZE_MULT = new intParameter("@SIZE_MULT", base.inputParameterList);
			        ALL_COLOR_MIN = new intParameter("@ALL_COLOR_MIN", base.inputParameterList);
			        ALL_COLOR_MAX = new intParameter("@ALL_COLOR_MAX", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        EXCEED_CAPACITY = new charParameter("@EXCEED_CAPACITY", base.inputParameterList);
			        MERCH_UNSPECIFIED = new charParameter("@MERCH_UNSPECIFIED", base.inputParameterList);
			        ON_HAND_UNSPECIFIED = new charParameter("@ON_HAND_UNSPECIFIED", base.inputParameterList);
			        RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
			        RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
			        STORE_GRADES_SG_RID = new intParameter("@STORE_GRADES_SG_RID", base.inputParameterList);
			        INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
			        IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
			        IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
			        IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
			        IMO_SG_RID = new intParameter("@IMO_SG_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? STORE_FILTER_RID,
			                      int? STORE_GRADE_TIMEFRAME,
			                      char? EXCEED_MAX_IND,
			                      double? PERCENT_NEED_LIMIT,
			                      double? RESERVE,
			                      char? PERCENT_IND,
			                      int? MERCH_HN_RID,
			                      int? MERCH_PH_RID,
			                      int? MERCH_PHL_SEQUENCE,
			                      int? ON_HAND_HN_RID,
			                      int? ON_HAND_PH_RID,
			                      int? ON_HAND_PHL_SEQUENCE,
			                      double? ON_HAND_FACTOR,
			                      int? COLOR_MULT,
			                      int? SIZE_MULT,
			                      int? ALL_COLOR_MIN,
			                      int? ALL_COLOR_MAX,
			                      int? SG_RID,
			                      char? EXCEED_CAPACITY,
			                      char? MERCH_UNSPECIFIED,
			                      char? ON_HAND_UNSPECIFIED,
			                      double? RESERVE_AS_BULK,
			                      double? RESERVE_AS_PACKS,
			                      int? STORE_GRADES_SG_RID,
			                      char? INVENTORY_IND,
			                      int? IB_MERCH_HN_RID,
			                      int? IB_MERCH_PH_RID,
			                      int? IB_MERCH_PHL_SEQUENCE,
			                      int? IMO_SG_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.STORE_GRADE_TIMEFRAME.SetValue(STORE_GRADE_TIMEFRAME);
                        this.EXCEED_MAX_IND.SetValue(EXCEED_MAX_IND);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        this.ON_HAND_PH_RID.SetValue(ON_HAND_PH_RID);
                        this.ON_HAND_PHL_SEQUENCE.SetValue(ON_HAND_PHL_SEQUENCE);
                        this.ON_HAND_FACTOR.SetValue(ON_HAND_FACTOR);
                        this.COLOR_MULT.SetValue(COLOR_MULT);
                        this.SIZE_MULT.SetValue(SIZE_MULT);
                        this.ALL_COLOR_MIN.SetValue(ALL_COLOR_MIN);
                        this.ALL_COLOR_MAX.SetValue(ALL_COLOR_MAX);
                        this.SG_RID.SetValue(SG_RID);
                        this.EXCEED_CAPACITY.SetValue(EXCEED_CAPACITY);
                        this.MERCH_UNSPECIFIED.SetValue(MERCH_UNSPECIFIED);
                        this.ON_HAND_UNSPECIFIED.SetValue(ON_HAND_UNSPECIFIED);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        this.STORE_GRADES_SG_RID.SetValue(STORE_GRADES_SG_RID);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.IMO_SG_RID.SetValue(IMO_SG_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}


			public static MID_GROUP_LEVEL_FUNCTION_INSERT_def MID_GROUP_LEVEL_FUNCTION_INSERT = new MID_GROUP_LEVEL_FUNCTION_INSERT_def();
			public class MID_GROUP_LEVEL_FUNCTION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_FUNCTION_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private charParameter DEFAULT_IND;
                private charParameter PLAN_IND;
                private charParameter USE_DEFAULT_IND;
                private charParameter CLEAR_IND;
                private charParameter SEASON_IND;
                private intParameter SEASON_HN_RID;
                private intParameter GLFT_ID;
                private intParameter GLSB_ID;
                private charParameter LY_ALT_IND;
                private charParameter TREND_ALT_IND;
                private charParameter TY_EQUALIZE_WEIGHT_IND;
                private charParameter LY_EQUALIZE_WEIGHT_IND;
                private charParameter APPLY_EQUALIZE_WEIGHT_IND;
                private charParameter PROJECT_CURR_WEEK_SALES_IND;
			
			    public MID_GROUP_LEVEL_FUNCTION_INSERT_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_FUNCTION_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("GROUP_LEVEL_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        DEFAULT_IND = new charParameter("@DEFAULT_IND", base.inputParameterList);
			        PLAN_IND = new charParameter("@PLAN_IND", base.inputParameterList);
			        USE_DEFAULT_IND = new charParameter("@USE_DEFAULT_IND", base.inputParameterList);
			        CLEAR_IND = new charParameter("@CLEAR_IND", base.inputParameterList);
			        SEASON_IND = new charParameter("@SEASON_IND", base.inputParameterList);
			        SEASON_HN_RID = new intParameter("@SEASON_HN_RID", base.inputParameterList);
			        GLFT_ID = new intParameter("@GLFT_ID", base.inputParameterList);
			        GLSB_ID = new intParameter("@GLSB_ID", base.inputParameterList);
			        LY_ALT_IND = new charParameter("@LY_ALT_IND", base.inputParameterList);
			        TREND_ALT_IND = new charParameter("@TREND_ALT_IND", base.inputParameterList);
			        TY_EQUALIZE_WEIGHT_IND = new charParameter("@TY_EQUALIZE_WEIGHT_IND", base.inputParameterList);
			        LY_EQUALIZE_WEIGHT_IND = new charParameter("@LY_EQUALIZE_WEIGHT_IND", base.inputParameterList);
			        APPLY_EQUALIZE_WEIGHT_IND = new charParameter("@APPLY_EQUALIZE_WEIGHT_IND", base.inputParameterList);
			        PROJECT_CURR_WEEK_SALES_IND = new charParameter("@PROJECT_CURR_WEEK_SALES_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      char? DEFAULT_IND,
			                      char? PLAN_IND,
			                      char? USE_DEFAULT_IND,
			                      char? CLEAR_IND,
			                      char? SEASON_IND,
			                      int? SEASON_HN_RID,
			                      int? GLFT_ID,
			                      int? GLSB_ID,
			                      char? LY_ALT_IND,
			                      char? TREND_ALT_IND,
			                      char? TY_EQUALIZE_WEIGHT_IND,
			                      char? LY_EQUALIZE_WEIGHT_IND,
			                      char? APPLY_EQUALIZE_WEIGHT_IND,
			                      char? PROJECT_CURR_WEEK_SALES_IND
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_FUNCTION_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.DEFAULT_IND.SetValue(DEFAULT_IND);
                        this.PLAN_IND.SetValue(PLAN_IND);
                        this.USE_DEFAULT_IND.SetValue(USE_DEFAULT_IND);
                        this.CLEAR_IND.SetValue(CLEAR_IND);
                        this.SEASON_IND.SetValue(SEASON_IND);
                        this.SEASON_HN_RID.SetValue(SEASON_HN_RID);
                        this.GLFT_ID.SetValue(GLFT_ID);
                        this.GLSB_ID.SetValue(GLSB_ID);
                        this.LY_ALT_IND.SetValue(LY_ALT_IND);
                        this.TREND_ALT_IND.SetValue(TREND_ALT_IND);
                        this.TY_EQUALIZE_WEIGHT_IND.SetValue(TY_EQUALIZE_WEIGHT_IND);
                        this.LY_EQUALIZE_WEIGHT_IND.SetValue(LY_EQUALIZE_WEIGHT_IND);
                        this.APPLY_EQUALIZE_WEIGHT_IND.SetValue(APPLY_EQUALIZE_WEIGHT_IND);
                        this.PROJECT_CURR_WEEK_SALES_IND.SetValue(PROJECT_CURR_WEEK_SALES_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP_def MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP = new MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP_def();
			public class MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP.SQL"

                private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TREND_CAPS_DELETE_FROM_GROUP_def MID_TREND_CAPS_DELETE_FROM_GROUP = new MID_TREND_CAPS_DELETE_FROM_GROUP_def();
			public class MID_TREND_CAPS_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TREND_CAPS_DELETE_FROM_GROUP.SQL"

                private intParameter METHOD_RID;
			
			    public MID_TREND_CAPS_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_TREND_CAPS_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TREND_CAPS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_TREND_CAPS_DELETE_FROM_GROUP_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_DELETE_FROM_GROUP_def MID_STOCK_MIN_MAX_DELETE_FROM_GROUP = new MID_STOCK_MIN_MAX_DELETE_FROM_GROUP_def();
			public class MID_STOCK_MIN_MAX_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_DELETE_FROM_GROUP.SQL"

                private intParameter METHOD_RID;
			
			    public MID_STOCK_MIN_MAX_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_DELETE_FROM_GROUP_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP_def MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP = new MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP_def();
			public class MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP.SQL"

                private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GROUP_LEVEL_NODE_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP_def MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP = new MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP_def();
			public class MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP.SQL"

                private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GROUP_LEVEL_FUNCTION");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE_def MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE = new MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE_def();
			public class MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE.SQL"

                private intParameter BASIS_HN_RID;
			
			    public MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_BASIS");
			        BASIS_HN_RID = new intParameter("@BASIS_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? BASIS_HN_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE_def))
                    {
                        this.BASIS_HN_RID.SetValue(BASIS_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_READ_FROM_NODE_def MID_METHOD_VELOCITY_READ_FROM_NODE = new MID_METHOD_VELOCITY_READ_FROM_NODE_def();
			public class MID_METHOD_VELOCITY_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_READ_FROM_NODE.SQL"

                private intParameter OTS_PLAN_HN_RID;
			
			    public MID_METHOD_VELOCITY_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY");
			        OTS_PLAN_HN_RID = new intParameter("@OTS_PLAN_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OTS_PLAN_HN_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_READ_FROM_NODE_def))
                    {
                        this.OTS_PLAN_HN_RID.SetValue(OTS_PLAN_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_MATRIX_DELETE_def MID_METHOD_VELOCITY_MATRIX_DELETE = new MID_METHOD_VELOCITY_MATRIX_DELETE_def();
			public class MID_METHOD_VELOCITY_MATRIX_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_MATRIX_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_MATRIX_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_MATRIX_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_VELOCITY_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_MATRIX_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE_def MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE = new MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE_def();
			public class MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_VELOCITY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_SELL_THRU_DELETE_def MID_METHOD_VELOCITY_SELL_THRU_DELETE = new MID_METHOD_VELOCITY_SELL_THRU_DELETE_def();
			public class MID_METHOD_VELOCITY_SELL_THRU_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_SELL_THRU_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_SELL_THRU_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_SELL_THRU_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_VELOCITY_SELL_THRU");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_SELL_THRU_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_GRADE_DELETE_def MID_METHOD_VELOCITY_GRADE_DELETE = new MID_METHOD_VELOCITY_GRADE_DELETE_def();
			public class MID_METHOD_VELOCITY_GRADE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GRADE_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_GRADE_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GRADE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_VELOCITY_GRADE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GRADE_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_BASIS_DELETE_def MID_METHOD_VELOCITY_BASIS_DELETE = new MID_METHOD_VELOCITY_BASIS_DELETE_def();
			public class MID_METHOD_VELOCITY_BASIS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_BASIS_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_BASIS_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_BASIS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_VELOCITY_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_BASIS_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static MID_METHOD_VELOCITY_UPDATE_def MID_METHOD_VELOCITY_UPDATE = new MID_METHOD_VELOCITY_UPDATE_def();
            public class MID_METHOD_VELOCITY_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter SG_RID;
                private intParameter OTS_PLAN_HN_RID;
                private intParameter OTS_PLAN_PH_RID;
                private intParameter OTS_PLAN_PHL_SEQUENCE;
                private charParameter SIM_STORE_IND;
                private charParameter AVG_USING_CHAIN_IND;
                private charParameter SHIP_USING_BASIS_IND;
                private charParameter TREND_PERCENT;
                private intParameter OTS_BEGIN_CDR_RID;
                private charParameter BALANCE_IND;
                private charParameter APPLY_MIN_MAX_IND;
                private charParameter RECONCILE_IND;
                private charParameter INVENTORY_IND;
                private intParameter MERCH_TYPE;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter OTS_SHIP_TO_CDR_RID;
                private intParameter GRADE_VARIABLE_TYPE;
                private charParameter BALANCE_TO_HEADER_IND;

                public MID_METHOD_VELOCITY_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_VELOCITY_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_VELOCITY");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    OTS_PLAN_HN_RID = new intParameter("@OTS_PLAN_HN_RID", base.inputParameterList);
                    OTS_PLAN_PH_RID = new intParameter("@OTS_PLAN_PH_RID", base.inputParameterList);
                    OTS_PLAN_PHL_SEQUENCE = new intParameter("@OTS_PLAN_PHL_SEQUENCE", base.inputParameterList);
                    SIM_STORE_IND = new charParameter("@SIM_STORE_IND", base.inputParameterList);
                    AVG_USING_CHAIN_IND = new charParameter("@AVG_USING_CHAIN_IND", base.inputParameterList);
                    SHIP_USING_BASIS_IND = new charParameter("@SHIP_USING_BASIS_IND", base.inputParameterList);
                    TREND_PERCENT = new charParameter("@TREND_PERCENT", base.inputParameterList);
                    OTS_BEGIN_CDR_RID = new intParameter("@OTS_BEGIN_CDR_RID", base.inputParameterList);
                    BALANCE_IND = new charParameter("@BALANCE_IND", base.inputParameterList);
                    APPLY_MIN_MAX_IND = new charParameter("@APPLY_MIN_MAX_IND", base.inputParameterList);
                    RECONCILE_IND = new charParameter("@RECONCILE_IND", base.inputParameterList);
                    INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
                    MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
                    MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
                    MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
                    MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
                    OTS_SHIP_TO_CDR_RID = new intParameter("@OTS_SHIP_TO_CDR_RID", base.inputParameterList);
                    GRADE_VARIABLE_TYPE = new intParameter("@GRADE_VARIABLE_TYPE", base.inputParameterList);
                    BALANCE_TO_HEADER_IND = new charParameter("@BALANCE_TO_HEADER_IND", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? SG_RID,
                                  int? OTS_PLAN_HN_RID,
                                  int? OTS_PLAN_PH_RID,
                                  int? OTS_PLAN_PHL_SEQUENCE,
                                  char? SIM_STORE_IND,
                                  char? AVG_USING_CHAIN_IND,
                                  char? SHIP_USING_BASIS_IND,
                                  char? TREND_PERCENT,
                                  int? OTS_BEGIN_CDR_RID,
                                  char? BALANCE_IND,
                                  char? APPLY_MIN_MAX_IND,
                                  char? RECONCILE_IND,
                                  char? INVENTORY_IND,
                                  int? MERCH_TYPE,
                                  int? MERCH_HN_RID,
                                  int? MERCH_PH_RID,
                                  int? MERCH_PHL_SEQUENCE,
                                  int? OTS_SHIP_TO_CDR_RID,
                                  int? GRADE_VARIABLE_TYPE,
                                  char? BALANCE_TO_HEADER_IND
                                  )
                {
                    lock (typeof(MID_METHOD_VELOCITY_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.OTS_PLAN_HN_RID.SetValue(OTS_PLAN_HN_RID);
                        this.OTS_PLAN_PH_RID.SetValue(OTS_PLAN_PH_RID);
                        this.OTS_PLAN_PHL_SEQUENCE.SetValue(OTS_PLAN_PHL_SEQUENCE);
                        this.SIM_STORE_IND.SetValue(SIM_STORE_IND);
                        this.AVG_USING_CHAIN_IND.SetValue(AVG_USING_CHAIN_IND);
                        this.SHIP_USING_BASIS_IND.SetValue(SHIP_USING_BASIS_IND);
                        this.TREND_PERCENT.SetValue(TREND_PERCENT);
                        this.OTS_BEGIN_CDR_RID.SetValue(OTS_BEGIN_CDR_RID);
                        this.BALANCE_IND.SetValue(BALANCE_IND);
                        this.APPLY_MIN_MAX_IND.SetValue(APPLY_MIN_MAX_IND);
                        this.RECONCILE_IND.SetValue(RECONCILE_IND);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.OTS_SHIP_TO_CDR_RID.SetValue(OTS_SHIP_TO_CDR_RID);
                        this.GRADE_VARIABLE_TYPE.SetValue(GRADE_VARIABLE_TYPE);
                        this.BALANCE_TO_HEADER_IND.SetValue(BALANCE_TO_HEADER_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }


			public static MID_METHOD_VELOCITY_MATRIX_INSERT_def MID_METHOD_VELOCITY_MATRIX_INSERT = new MID_METHOD_VELOCITY_MATRIX_INSERT_def();
			public class MID_METHOD_VELOCITY_MATRIX_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_MATRIX_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private intParameter BOUNDARY;
                private intParameter VELOCITY_SELL_THRU_INDEX;
                private intParameter VELOCITY_RULE;
                private floatParameter VELOCITY_QUANTITY;
			
			    public MID_METHOD_VELOCITY_MATRIX_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_MATRIX_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_VELOCITY_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        VELOCITY_SELL_THRU_INDEX = new intParameter("@VELOCITY_SELL_THRU_INDEX", base.inputParameterList);
			        VELOCITY_RULE = new intParameter("@VELOCITY_RULE", base.inputParameterList);
			        VELOCITY_QUANTITY = new floatParameter("@VELOCITY_QUANTITY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? BOUNDARY,
			                      int? VELOCITY_SELL_THRU_INDEX,
			                      int? VELOCITY_RULE,
                                  double? VELOCITY_QUANTITY
			                      )
			    {
                    lock (typeof(MID_METHOD_VELOCITY_MATRIX_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.VELOCITY_SELL_THRU_INDEX.SetValue(VELOCITY_SELL_THRU_INDEX);
                        this.VELOCITY_RULE.SetValue(VELOCITY_RULE);
                        this.VELOCITY_QUANTITY.SetValue(VELOCITY_QUANTITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT_def MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT = new MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT_def();
			public class MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private intParameter NO_ONHAND_RULE;
                private floatParameter NO_ONHAND_QUANTITY;
                private charParameter MATRIX_MODE_IND;
                private intParameter MATRIX_MODE_AVERAGE_RULE;
                private floatParameter MATRIX_MODE_AVERAGE_QUANTITY;
                private charParameter MATRIX_SPREAD_IND;
			
			    public MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_VELOCITY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        NO_ONHAND_RULE = new intParameter("@NO_ONHAND_RULE", base.inputParameterList);
			        NO_ONHAND_QUANTITY = new floatParameter("@NO_ONHAND_QUANTITY", base.inputParameterList);
			        MATRIX_MODE_IND = new charParameter("@MATRIX_MODE_IND", base.inputParameterList);
			        MATRIX_MODE_AVERAGE_RULE = new intParameter("@MATRIX_MODE_AVERAGE_RULE", base.inputParameterList);
			        MATRIX_MODE_AVERAGE_QUANTITY = new floatParameter("@MATRIX_MODE_AVERAGE_QUANTITY", base.inputParameterList);
			        MATRIX_SPREAD_IND = new charParameter("@MATRIX_SPREAD_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? NO_ONHAND_RULE,
			                      double? NO_ONHAND_QUANTITY,
			                      char? MATRIX_MODE_IND,
			                      int? MATRIX_MODE_AVERAGE_RULE,
			                      double? MATRIX_MODE_AVERAGE_QUANTITY,
			                      char? MATRIX_SPREAD_IND
			                      )
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GROUP_LEVEL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.NO_ONHAND_RULE.SetValue(NO_ONHAND_RULE);
                        this.NO_ONHAND_QUANTITY.SetValue(NO_ONHAND_QUANTITY);
                        this.MATRIX_MODE_IND.SetValue(MATRIX_MODE_IND);
                        this.MATRIX_MODE_AVERAGE_RULE.SetValue(MATRIX_MODE_AVERAGE_RULE);
                        this.MATRIX_MODE_AVERAGE_QUANTITY.SetValue(MATRIX_MODE_AVERAGE_QUANTITY);
                        this.MATRIX_SPREAD_IND.SetValue(MATRIX_SPREAD_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_MATRIX_READ_def MID_METHOD_VELOCITY_MATRIX_READ = new MID_METHOD_VELOCITY_MATRIX_READ_def();
			public class MID_METHOD_VELOCITY_MATRIX_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_MATRIX_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_MATRIX_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_MATRIX_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_MATRIX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_MATRIX_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_SELL_THRU_INSERT_def MID_METHOD_VELOCITY_SELL_THRU_INSERT = new MID_METHOD_VELOCITY_SELL_THRU_INSERT_def();
			public class MID_METHOD_VELOCITY_SELL_THRU_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_SELL_THRU_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter VELOCITY_SELL_THRU_INDEX;
			
			    public MID_METHOD_VELOCITY_SELL_THRU_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_SELL_THRU_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_VELOCITY_SELL_THRU");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        VELOCITY_SELL_THRU_INDEX = new intParameter("@VELOCITY_SELL_THRU_INDEX", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? VELOCITY_SELL_THRU_INDEX
			                      )
			    {
                    lock (typeof(MID_METHOD_VELOCITY_SELL_THRU_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.VELOCITY_SELL_THRU_INDEX.SetValue(VELOCITY_SELL_THRU_INDEX);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_GRADE_INSERT_def MID_METHOD_VELOCITY_GRADE_INSERT = new MID_METHOD_VELOCITY_GRADE_INSERT_def();
			public class MID_METHOD_VELOCITY_GRADE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GRADE_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;
			
			    public MID_METHOD_VELOCITY_GRADE_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GRADE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_VELOCITY_GRADE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
			        MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
			        MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
			        MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BOUNDARY,
			                      string GRADE_CODE,
			                      int? MINIMUM_STOCK,
			                      int? MAXIMUM_STOCK,
			                      int? MINIMUM_AD
			                      )
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GRADE_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_BASIS_INSERT_def MID_METHOD_VELOCITY_BASIS_INSERT = new MID_METHOD_VELOCITY_BASIS_INSERT_def();
			public class MID_METHOD_VELOCITY_BASIS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_BASIS_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BASIS_SEQUENCE;
                private intParameter BASIS_HN_RID;
                private intParameter BASIS_FV_RID;
                private intParameter CDR_RID;
                private floatParameter SALES_WEIGHT;
                private intParameter BASIS_PH_RID;
                private intParameter BASIS_PHL_SEQUENCE;
			
			    public MID_METHOD_VELOCITY_BASIS_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_BASIS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_VELOCITY_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BASIS_SEQUENCE = new intParameter("@BASIS_SEQUENCE", base.inputParameterList);
			        BASIS_HN_RID = new intParameter("@BASIS_HN_RID", base.inputParameterList);
			        BASIS_FV_RID = new intParameter("@BASIS_FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        SALES_WEIGHT = new floatParameter("@SALES_WEIGHT", base.inputParameterList);
			        BASIS_PH_RID = new intParameter("@BASIS_PH_RID", base.inputParameterList);
			        BASIS_PHL_SEQUENCE = new intParameter("@BASIS_PHL_SEQUENCE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BASIS_SEQUENCE,
			                      int? BASIS_HN_RID,
			                      int? BASIS_FV_RID,
			                      int? CDR_RID,
			                      double? SALES_WEIGHT,
			                      int? BASIS_PH_RID,
			                      int? BASIS_PHL_SEQUENCE
			                      )
			    {
                    lock (typeof(MID_METHOD_VELOCITY_BASIS_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BASIS_SEQUENCE.SetValue(BASIS_SEQUENCE);
                        this.BASIS_HN_RID.SetValue(BASIS_HN_RID);
                        this.BASIS_FV_RID.SetValue(BASIS_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.SALES_WEIGHT.SetValue(SALES_WEIGHT);
                        this.BASIS_PH_RID.SetValue(BASIS_PH_RID);
                        this.BASIS_PHL_SEQUENCE.SetValue(BASIS_PHL_SEQUENCE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static MID_METHOD_VELOCITY_INSERT_def MID_METHOD_VELOCITY_INSERT = new MID_METHOD_VELOCITY_INSERT_def();
            public class MID_METHOD_VELOCITY_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SG_RID;
                private intParameter OTS_PLAN_HN_RID;
                private intParameter OTS_PLAN_PH_RID;
                private intParameter OTS_PLAN_PHL_SEQUENCE;
                private charParameter SIM_STORE_IND;
                private charParameter AVG_USING_CHAIN_IND;
                private charParameter SHIP_USING_BASIS_IND;
                private charParameter TREND_PERCENT;
                private intParameter OTS_BEGIN_CDR_RID;
                private intParameter OTS_SHIP_TO_CDR_RID;
                private charParameter BALANCE_IND;
                private charParameter APPLY_MIN_MAX_IND;
                private charParameter RECONCILE_IND;
                private charParameter INVENTORY_IND;
                private intParameter MERCH_TYPE;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter GRADE_VARIABLE_TYPE;
                private charParameter BALANCE_TO_HEADER_IND;

                public MID_METHOD_VELOCITY_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_VELOCITY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_VELOCITY");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    OTS_PLAN_HN_RID = new intParameter("@OTS_PLAN_HN_RID", base.inputParameterList);
                    OTS_PLAN_PH_RID = new intParameter("@OTS_PLAN_PH_RID", base.inputParameterList);
                    OTS_PLAN_PHL_SEQUENCE = new intParameter("@OTS_PLAN_PHL_SEQUENCE", base.inputParameterList);
                    SIM_STORE_IND = new charParameter("@SIM_STORE_IND", base.inputParameterList);
                    AVG_USING_CHAIN_IND = new charParameter("@AVG_USING_CHAIN_IND", base.inputParameterList);
                    SHIP_USING_BASIS_IND = new charParameter("@SHIP_USING_BASIS_IND", base.inputParameterList);
                    TREND_PERCENT = new charParameter("@TREND_PERCENT", base.inputParameterList);
                    OTS_BEGIN_CDR_RID = new intParameter("@OTS_BEGIN_CDR_RID", base.inputParameterList);
                    OTS_SHIP_TO_CDR_RID = new intParameter("@OTS_SHIP_TO_CDR_RID", base.inputParameterList);
                    BALANCE_IND = new charParameter("@BALANCE_IND", base.inputParameterList);
                    APPLY_MIN_MAX_IND = new charParameter("@APPLY_MIN_MAX_IND", base.inputParameterList);
                    RECONCILE_IND = new charParameter("@RECONCILE_IND", base.inputParameterList);
                    INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
                    MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
                    MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
                    MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
                    MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
                    GRADE_VARIABLE_TYPE = new intParameter("@GRADE_VARIABLE_TYPE", base.inputParameterList);
                    BALANCE_TO_HEADER_IND = new charParameter("@BALANCE_TO_HEADER_IND", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? SG_RID,
                                  int? OTS_PLAN_HN_RID,
                                  int? OTS_PLAN_PH_RID,
                                  int? OTS_PLAN_PHL_SEQUENCE,
                                  char? SIM_STORE_IND,
                                  char? AVG_USING_CHAIN_IND,
                                  char? SHIP_USING_BASIS_IND,
                                  char? TREND_PERCENT,
                                  int? OTS_BEGIN_CDR_RID,
                                  int? OTS_SHIP_TO_CDR_RID,
                                  char? BALANCE_IND,
                                  char? APPLY_MIN_MAX_IND,
                                  char? RECONCILE_IND,
                                  char? INVENTORY_IND,
                                  int? MERCH_TYPE,
                                  int? MERCH_HN_RID,
                                  int? MERCH_PH_RID,
                                  int? MERCH_PHL_SEQUENCE,
                                  int? GRADE_VARIABLE_TYPE,
                                  char? BALANCE_TO_HEADER_IND
                                  )
                {
                    lock (typeof(MID_METHOD_VELOCITY_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.OTS_PLAN_HN_RID.SetValue(OTS_PLAN_HN_RID);
                        this.OTS_PLAN_PH_RID.SetValue(OTS_PLAN_PH_RID);
                        this.OTS_PLAN_PHL_SEQUENCE.SetValue(OTS_PLAN_PHL_SEQUENCE);
                        this.SIM_STORE_IND.SetValue(SIM_STORE_IND);
                        this.AVG_USING_CHAIN_IND.SetValue(AVG_USING_CHAIN_IND);
                        this.SHIP_USING_BASIS_IND.SetValue(SHIP_USING_BASIS_IND);
                        this.TREND_PERCENT.SetValue(TREND_PERCENT);
                        this.OTS_BEGIN_CDR_RID.SetValue(OTS_BEGIN_CDR_RID);
                        this.OTS_SHIP_TO_CDR_RID.SetValue(OTS_SHIP_TO_CDR_RID);
                        this.BALANCE_IND.SetValue(BALANCE_IND);
                        this.APPLY_MIN_MAX_IND.SetValue(APPLY_MIN_MAX_IND);
                        this.RECONCILE_IND.SetValue(RECONCILE_IND);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.GRADE_VARIABLE_TYPE.SetValue(GRADE_VARIABLE_TYPE);
                        this.BALANCE_TO_HEADER_IND.SetValue(BALANCE_TO_HEADER_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }



			public static MID_METHOD_VELOCITY_GROUP_LEVEL_READ_def MID_METHOD_VELOCITY_GROUP_LEVEL_READ = new MID_METHOD_VELOCITY_GROUP_LEVEL_READ_def();
			public class MID_METHOD_VELOCITY_GROUP_LEVEL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GROUP_LEVEL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_GROUP_LEVEL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GROUP_LEVEL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_GROUP_LEVEL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GROUP_LEVEL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_SELL_THRU_READ_def MID_METHOD_VELOCITY_SELL_THRU_READ = new MID_METHOD_VELOCITY_SELL_THRU_READ_def();
			public class MID_METHOD_VELOCITY_SELL_THRU_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_SELL_THRU_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_SELL_THRU_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_SELL_THRU_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_SELL_THRU");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_SELL_THRU_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_GRADE_READ_def MID_METHOD_VELOCITY_GRADE_READ = new MID_METHOD_VELOCITY_GRADE_READ_def();
			public class MID_METHOD_VELOCITY_GRADE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_GRADE_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_GRADE_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_GRADE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_GRADE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_GRADE_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_VELOCITY_BASIS_READ_def MID_METHOD_VELOCITY_BASIS_READ = new MID_METHOD_VELOCITY_BASIS_READ_def();
			public class MID_METHOD_VELOCITY_BASIS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_BASIS_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_BASIS_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_BASIS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_BASIS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_METHOD_VELOCITY_READ_def MID_METHOD_VELOCITY_READ = new MID_METHOD_VELOCITY_READ_def();
			public class MID_METHOD_VELOCITY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_VELOCITY_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_VELOCITY_READ_def()
			    {
			        base.procedureName = "MID_METHOD_VELOCITY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_VELOCITY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_VELOCITY_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK_def MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK = new MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK_def();
			public class MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK.SQL"

                private tableParameter TIME_ID_LIST;
                private intParameter SELECTED_NODE_RID;
                private intParameter OLL_RID;
                private intParameter USE_REG_SALES;
			
			    public MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK_def()
			    {
			        base.procedureName = "MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WEEKLY_HISTORY");
			        TIME_ID_LIST = new tableParameter("@TIME_ID_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USE_REG_SALES = new intParameter("@USE_REG_SALES", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable TIME_ID_LIST,
			                          int? SELECTED_NODE_RID,
			                          int? OLL_RID,
			                          int? USE_REG_SALES
			                          )
			    {
                    lock (typeof(MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK_def))
                    {
                        this.TIME_ID_LIST.SetValue(TIME_ID_LIST);
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USE_REG_SALES.SetValue(USE_REG_SALES);
                        base.SetCommandTimeout(0); //0=Unlimited time out   //TT#3445 -jsobek -Failure of Size Curve Generation
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_def MID_STORE_FORECAST_WEEK_LOCK_DELETE = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter TIME_ID_START;
                private intParameter TIME_ID_END;
                private tableParameter STORE_RID_LIST;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_ID_START = new intParameter("@TIME_ID_START", base.inputParameterList);
			        TIME_ID_END = new intParameter("@TIME_ID_END", base.inputParameterList);
			        STORE_RID_LIST = new tableParameter("@STORE_RID_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? TIME_ID_START,
			                      int? TIME_ID_END,
			                      DataTable STORE_RID_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_ID_START.SetValue(TIME_ID_START);
                        this.TIME_ID_END.SetValue(TIME_ID_END);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_def MID_CHAIN_FORECAST_WEEK_LOCK_DELETE = new MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_def();
			public class MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_LOCK_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter TIME_ID_START;
                private intParameter TIME_ID_END;
			
			    public MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_LOCK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_ID_START = new intParameter("@TIME_ID_START", base.inputParameterList);
			        TIME_ID_END = new intParameter("@TIME_ID_END", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? TIME_ID_START,
			                      int? TIME_ID_END
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_ID_START.SetValue(TIME_ID_START);
                        this.TIME_ID_END.SetValue(TIME_ID_END);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_def MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE = new MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_def();
			public class MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_DELETE_def MID_METHOD_GLOBAL_UNLOCK_DELETE = new MID_METHOD_GLOBAL_UNLOCK_DELETE_def();
			public class MID_METHOD_GLOBAL_UNLOCK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_UPDATE_def MID_METHOD_GLOBAL_UNLOCK_UPDATE = new MID_METHOD_GLOBAL_UNLOCK_UPDATE_def();
			public class MID_METHOD_GLOBAL_UNLOCK_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter STORE_IND;
                private charParameter CHAIN_IND;
                private intParameter STORE_FILTER_RID;
                private intParameter OLL_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      char? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      char? STORE_IND,
			                      char? CHAIN_IND,
			                      int? STORE_FILTER_RID,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_INSERT_def MID_METHOD_GLOBAL_UNLOCK_INSERT = new MID_METHOD_GLOBAL_UNLOCK_INSERT_def();
			public class MID_METHOD_GLOBAL_UNLOCK_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private charParameter MULTI_LEVEL_IND;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private charParameter STORE_IND;
                private charParameter CHAIN_IND;
                private intParameter STORE_FILTER_RID;
                private intParameter OLL_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MULTI_LEVEL_IND = new charParameter("@MULTI_LEVEL_IND", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      char? MULTI_LEVEL_IND,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      char? STORE_IND,
			                      char? CHAIN_IND,
			                      int? STORE_FILTER_RID,
			                      int? OLL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MULTI_LEVEL_IND.SetValue(MULTI_LEVEL_IND);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ_def MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ = new MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ_def();
			public class MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_READ_def MID_METHOD_GLOBAL_UNLOCK_READ = new MID_METHOD_GLOBAL_UNLOCK_READ_def();
			public class MID_METHOD_GLOBAL_UNLOCK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_READ_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_BULK_COLOR_READ_COLOR_CODE_def MID_HEADER_BULK_COLOR_READ_COLOR_CODE = new MID_HEADER_BULK_COLOR_READ_COLOR_CODE_def();
			public class MID_HEADER_BULK_COLOR_READ_COLOR_CODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_READ_COLOR_CODE.SQL"

                private intParameter HDR_BC_RID;
			
			    public MID_HEADER_BULK_COLOR_READ_COLOR_CODE_def()
			    {
			        base.procedureName = "MID_HEADER_BULK_COLOR_READ_COLOR_CODE";
			        base.procedureType = storedProcedureTypes.ScalarValue;
			        base.tableNames.Add("HEADER_BULK_COLOR");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			    }
			
			    public object ReadValue(DatabaseAccess _dba, int? HDR_BC_RID)
			    {
                    lock (typeof(MID_HEADER_BULK_COLOR_READ_COLOR_CODE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
			    }
			}

			public static MID_OTS_PLAN_READ_METHODS_BY_NODE_def MID_OTS_PLAN_READ_METHODS_BY_NODE = new MID_OTS_PLAN_READ_METHODS_BY_NODE_def();
			public class MID_OTS_PLAN_READ_METHODS_BY_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OTS_PLAN_READ_METHODS_BY_NODE.SQL"

                private intParameter PLAN_HN_RID;
			
			    public MID_OTS_PLAN_READ_METHODS_BY_NODE_def()
			    {
			        base.procedureName = "MID_OTS_PLAN_READ_METHODS_BY_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OTS_PLAN");
			        PLAN_HN_RID = new intParameter("@PLAN_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? PLAN_HN_RID)
			    {
                    lock (typeof(MID_OTS_PLAN_READ_METHODS_BY_NODE_def))
                    {
                        this.PLAN_HN_RID.SetValue(PLAN_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_CHAIN_SET_PERCENT_SET_RETURN_def SP_MID_CHAIN_SET_PERCENT_SET_RETURN = new SP_MID_CHAIN_SET_PERCENT_SET_RETURN_def();
            public class SP_MID_CHAIN_SET_PERCENT_SET_RETURN_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHAIN_SET_PERCENT_SET_RETURN.SQL"

			    private intParameter BEG_WEEK;
			    private intParameter END_WEEK;
			    private intParameter HN_RID;
			    private intParameter SG_RID;
                private intParameter SG_VERSION;  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		
			    public SP_MID_CHAIN_SET_PERCENT_SET_RETURN_def()
			    {
                    base.procedureName = "SP_MID_CHAIN_SET_PERCENT_SET_RETURN";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
			        BEG_WEEK = new intParameter("@BEG_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? BEG_WEEK,
			                          int? END_WEEK,
			                          int? HN_RID,
                                      int? SG_RID,
                                      int? SG_VERSION  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			                          )
			    {
                    lock (typeof(SP_MID_CHAIN_SET_PERCENT_SET_RETURN_def))
                    {
                        this.BEG_WEEK.SetValue(BEG_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        this.HN_RID.SetValue(HN_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_READ_def MID_STORE_FORECAST_WEEK_LOCK_READ = new MID_STORE_FORECAST_WEEK_LOCK_READ_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_READ.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter STORE_RID_LIST;
                private intParameter START_WEEK;
                private intParameter END_WEEK;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_READ_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        STORE_RID_LIST = new tableParameter("@STORE_RID_LIST", "STORE_RID_TYPE", base.inputParameterList);
			        START_WEEK = new intParameter("@START_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID,
			                          DataTable STORE_RID_LIST,
			                          int? START_WEEK,
			                          int? END_WEEK
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.STORE_RID_LIST.SetValue(STORE_RID_LIST);
                        this.START_WEEK.SetValue(START_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_NEED_READ_FROM_NODE_def MID_METHOD_SIZE_NEED_READ_FROM_NODE = new MID_METHOD_SIZE_NEED_READ_FROM_NODE_def();
			public class MID_METHOD_SIZE_NEED_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_NEED_READ_FROM_NODE.SQL"

                private intParameter MERCH_HN_RID;
			
			    public MID_METHOD_SIZE_NEED_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_NEED_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_NEED");
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? MERCH_HN_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_NEED_READ_FROM_NODE_def))
                    {
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_COLOR_MINMAX_READ_def MID_METHOD_OVERRIDE_COLOR_MINMAX_READ = new MID_METHOD_OVERRIDE_COLOR_MINMAX_READ_def();
			public class MID_METHOD_OVERRIDE_COLOR_MINMAX_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_COLOR_MINMAX_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_COLOR_MINMAX_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_COLOR_MINMAX_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_COLOR_MINMAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_COLOR_MINMAX_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ_def MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ = new MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_VALUES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_GROUP_LEVEL_READ_FROM_GROUP_def MID_STORE_GROUP_LEVEL_READ_FROM_GROUP = new MID_STORE_GROUP_LEVEL_READ_FROM_GROUP_def();
			public class MID_STORE_GROUP_LEVEL_READ_FROM_GROUP_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_READ_FROM_GROUP.SQL"

                private intParameter SG_RID;
			
			    public MID_STORE_GROUP_LEVEL_READ_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_STORE_GROUP_LEVEL_READ_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_GROUP_LEVEL");
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SG_RID)
			    {
                    lock (typeof(MID_STORE_GROUP_LEVEL_READ_FROM_GROUP_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_IMO_READ_def MID_METHOD_OVERRIDE_IMO_READ = new MID_METHOD_OVERRIDE_IMO_READ_def();
			public class MID_METHOD_OVERRIDE_IMO_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_IMO_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_IMO_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_IMO_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_IMO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_IMO_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ_def MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ = new MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_BOUNDARY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        dt.TableName = "GradeBoundary";
                        return dt;
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_CAPACITY_READ_def MID_METHOD_OVERRIDE_CAPACITY_READ = new MID_METHOD_OVERRIDE_CAPACITY_READ_def();
			public class MID_METHOD_OVERRIDE_CAPACITY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_CAPACITY_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_CAPACITY_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_CAPACITY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_CAPACITY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_CAPACITY_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_PACK_ROUNDING_READ_def MID_METHOD_OVERRIDE_PACK_ROUNDING_READ = new MID_METHOD_OVERRIDE_PACK_ROUNDING_READ_def();
			public class MID_METHOD_OVERRIDE_PACK_ROUNDING_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_PACK_ROUNDING_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_PACK_ROUNDING_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_PACK_ROUNDING_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE_PACK_ROUNDING");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_PACK_ROUNDING_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_INSERT_def MID_METHOD_OVERRIDE_INSERT = new MID_METHOD_OVERRIDE_INSERT_def();
			public class MID_METHOD_OVERRIDE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter STORE_GRADE_TIMEFRAME;
                private charParameter EXCEED_MAX_IND;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter ON_HAND_HN_RID;
                private intParameter ON_HAND_PH_RID;
                private intParameter ON_HAND_PHL_SEQUENCE;
                private floatParameter ON_HAND_FACTOR;
                private intParameter COLOR_MULT;
                private intParameter SIZE_MULT;
                private intParameter ALL_COLOR_MIN;
                private intParameter ALL_COLOR_MAX;
                private intParameter SG_RID;
                private charParameter EXCEED_CAPACITY;
                private charParameter MERCH_UNSPECIFIED;
                private charParameter ON_HAND_UNSPECIFIED;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
                private intParameter STORE_GRADES_SG_RID;
                private charParameter INVENTORY_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter IMO_SG_RID;
			
			    public MID_METHOD_OVERRIDE_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
			        STORE_GRADE_TIMEFRAME = new intParameter("@STORE_GRADE_TIMEFRAME", base.inputParameterList);
			        EXCEED_MAX_IND = new charParameter("@EXCEED_MAX_IND", base.inputParameterList);
			        PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
			        RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
			        PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
			        MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
			        MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
			        MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
			        ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
			        ON_HAND_PH_RID = new intParameter("@ON_HAND_PH_RID", base.inputParameterList);
			        ON_HAND_PHL_SEQUENCE = new intParameter("@ON_HAND_PHL_SEQUENCE", base.inputParameterList);
			        ON_HAND_FACTOR = new floatParameter("@ON_HAND_FACTOR", base.inputParameterList);
			        COLOR_MULT = new intParameter("@COLOR_MULT", base.inputParameterList);
			        SIZE_MULT = new intParameter("@SIZE_MULT", base.inputParameterList);
			        ALL_COLOR_MIN = new intParameter("@ALL_COLOR_MIN", base.inputParameterList);
			        ALL_COLOR_MAX = new intParameter("@ALL_COLOR_MAX", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        EXCEED_CAPACITY = new charParameter("@EXCEED_CAPACITY", base.inputParameterList);
			        MERCH_UNSPECIFIED = new charParameter("@MERCH_UNSPECIFIED", base.inputParameterList);
			        ON_HAND_UNSPECIFIED = new charParameter("@ON_HAND_UNSPECIFIED", base.inputParameterList);
			        RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
			        RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
			        STORE_GRADES_SG_RID = new intParameter("@STORE_GRADES_SG_RID", base.inputParameterList);
			        INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
			        IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
			        IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
			        IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
			        IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
			        IMO_SG_RID = new intParameter("@IMO_SG_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? STORE_FILTER_RID,
			                      int? STORE_GRADE_TIMEFRAME,
			                      char? EXCEED_MAX_IND,
			                      double? PERCENT_NEED_LIMIT,
			                      double? RESERVE,
			                      char? PERCENT_IND,
			                      int? MERCH_HN_RID,
			                      int? MERCH_PH_RID,
			                      int? MERCH_PHL_SEQUENCE,
			                      int? ON_HAND_HN_RID,
			                      int? ON_HAND_PH_RID,
			                      int? ON_HAND_PHL_SEQUENCE,
			                      double? ON_HAND_FACTOR,
			                      int? COLOR_MULT,
			                      int? SIZE_MULT,
			                      int? ALL_COLOR_MIN,
			                      int? ALL_COLOR_MAX,
			                      int? SG_RID,
			                      char? EXCEED_CAPACITY,
			                      char? MERCH_UNSPECIFIED,
			                      char? ON_HAND_UNSPECIFIED,
			                      double? RESERVE_AS_BULK,
			                      double? RESERVE_AS_PACKS,
			                      int? STORE_GRADES_SG_RID,
			                      char? INVENTORY_IND,
			                      int? IB_MERCH_TYPE,
			                      int? IB_MERCH_HN_RID,
			                      int? IB_MERCH_PH_RID,
			                      int? IB_MERCH_PHL_SEQUENCE,
			                      int? IMO_SG_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.STORE_GRADE_TIMEFRAME.SetValue(STORE_GRADE_TIMEFRAME);
                        this.EXCEED_MAX_IND.SetValue(EXCEED_MAX_IND);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        this.ON_HAND_PH_RID.SetValue(ON_HAND_PH_RID);
                        this.ON_HAND_PHL_SEQUENCE.SetValue(ON_HAND_PHL_SEQUENCE);
                        this.ON_HAND_FACTOR.SetValue(ON_HAND_FACTOR);
                        this.COLOR_MULT.SetValue(COLOR_MULT);
                        this.SIZE_MULT.SetValue(SIZE_MULT);
                        this.ALL_COLOR_MIN.SetValue(ALL_COLOR_MIN);
                        this.ALL_COLOR_MAX.SetValue(ALL_COLOR_MAX);
                        this.SG_RID.SetValue(SG_RID);
                        this.EXCEED_CAPACITY.SetValue(EXCEED_CAPACITY);
                        this.MERCH_UNSPECIFIED.SetValue(MERCH_UNSPECIFIED);
                        this.ON_HAND_UNSPECIFIED.SetValue(ON_HAND_UNSPECIFIED);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        this.STORE_GRADES_SG_RID.SetValue(STORE_GRADES_SG_RID);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.IMO_SG_RID.SetValue(IMO_SG_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_IMO_DELETE_def MID_METHOD_OVERRIDE_IMO_DELETE = new MID_METHOD_OVERRIDE_IMO_DELETE_def();
			public class MID_METHOD_OVERRIDE_IMO_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_IMO_DELETE.SQL"

                private intParameter METHOD_RID;
                private intParameter ST_RID;
			
			    public MID_METHOD_OVERRIDE_IMO_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_IMO_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_OVERRIDE_IMO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? ST_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_IMO_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_IMO_INSERT_def MID_METHOD_OVERRIDE_IMO_INSERT = new MID_METHOD_OVERRIDE_IMO_INSERT_def();
			public class MID_METHOD_OVERRIDE_IMO_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_IMO_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter ST_RID;
                private intParameter IMO_MIN_SHIP_QTY;
                private floatParameter IMO_PCT_PK_THRSHLD;
                private intParameter IMO_MAX_VALUE;
			
			    public MID_METHOD_OVERRIDE_IMO_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_IMO_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_IMO");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        IMO_MIN_SHIP_QTY = new intParameter("@IMO_MIN_SHIP_QTY", base.inputParameterList);
			        IMO_PCT_PK_THRSHLD = new floatParameter("@IMO_PCT_PK_THRSHLD", base.inputParameterList);
			        IMO_MAX_VALUE = new intParameter("@IMO_MAX_VALUE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? ST_RID,
			                      int? IMO_MIN_SHIP_QTY,
			                      double? IMO_PCT_PK_THRSHLD,
			                      int? IMO_MAX_VALUE
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_IMO_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.IMO_MIN_SHIP_QTY.SetValue(IMO_MIN_SHIP_QTY);
                        this.IMO_PCT_PK_THRSHLD.SetValue(IMO_PCT_PK_THRSHLD);
                        this.IMO_MAX_VALUE.SetValue(IMO_MAX_VALUE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG_def MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG = new MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG_def();
			public class MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG.SQL"

                private intParameter METHOD_RID;
                private intParameter IMO_APPLY_VSW;
			
			    public MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        IMO_APPLY_VSW = new intParameter("@IMO_APPLY_VSW", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? IMO_APPLY_VSW
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.IMO_APPLY_VSW.SetValue(IMO_APPLY_VSW);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT_def MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT = new MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT_def();
			public class MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter COLOR_CODE_RID;
                private intParameter COLOR_MIN;
                private intParameter COLOR_MAX;
			
			    public MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_COLOR_MINMAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
			        COLOR_MIN = new intParameter("@COLOR_MIN", base.inputParameterList);
			        COLOR_MAX = new intParameter("@COLOR_MAX", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? COLOR_CODE_RID,
			                      int? COLOR_MIN,
			                      int? COLOR_MAX
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.COLOR_MIN.SetValue(COLOR_MIN);
                        this.COLOR_MAX.SetValue(COLOR_MAX);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT_def MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT = new MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_BOUNDARY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BOUNDARY,
			                      string GRADE_CODE
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT_def MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT = new MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT_def();
			public class MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;
                private intParameter MINIMUM_COLOR;
                private intParameter MAXIMUM_COLOR;
                private intParameter SHIP_UP_TO;
                private intParameter SGL_RID;
			
			    public MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_STORE_GRADES_VALUES");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
			        MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
			        MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
			        MINIMUM_COLOR = new intParameter("@MINIMUM_COLOR", base.inputParameterList);
			        MAXIMUM_COLOR = new intParameter("@MAXIMUM_COLOR", base.inputParameterList);
			        SHIP_UP_TO = new intParameter("@SHIP_UP_TO", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BOUNDARY,
			                      int? MINIMUM_STOCK,
			                      int? MAXIMUM_STOCK,
			                      int? MINIMUM_AD,
			                      int? MINIMUM_COLOR,
			                      int? MAXIMUM_COLOR,
			                      int? SHIP_UP_TO,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        this.MINIMUM_COLOR.SetValue(MINIMUM_COLOR);
                        this.MAXIMUM_COLOR.SetValue(MAXIMUM_COLOR);
                        this.SHIP_UP_TO.SetValue(SHIP_UP_TO);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_CAPACITY_INSERT_def MID_METHOD_OVERRIDE_CAPACITY_INSERT = new MID_METHOD_OVERRIDE_CAPACITY_INSERT_def();
			public class MID_METHOD_OVERRIDE_CAPACITY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_CAPACITY_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
                private charParameter EXCEED_CAPACITY;
                private floatParameter EXCEED_BY;
			
			    public MID_METHOD_OVERRIDE_CAPACITY_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_CAPACITY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_CAPACITY");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        EXCEED_CAPACITY = new charParameter("@EXCEED_CAPACITY", base.inputParameterList);
			        EXCEED_BY = new floatParameter("@EXCEED_BY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      char? EXCEED_CAPACITY,
			                      double? EXCEED_BY
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_CAPACITY_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.EXCEED_CAPACITY.SetValue(EXCEED_CAPACITY);
                        this.EXCEED_BY.SetValue(EXCEED_BY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT_def MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT = new MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT_def();
			public class MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter PACK_MULTIPLE_RID;
                private floatParameter PACK_ROUNDING_1ST_PACK_PCT;
                private floatParameter PACK_ROUNDING_NTH_PACK_PCT;
			
			    public MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_OVERRIDE_PACK_ROUNDING");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        PACK_MULTIPLE_RID = new intParameter("@PACK_MULTIPLE_RID", base.inputParameterList);
			        PACK_ROUNDING_1ST_PACK_PCT = new floatParameter("@PACK_ROUNDING_1ST_PACK_PCT", base.inputParameterList);
			        PACK_ROUNDING_NTH_PACK_PCT = new floatParameter("@PACK_ROUNDING_NTH_PACK_PCT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? PACK_MULTIPLE_RID,
			                      double? PACK_ROUNDING_1ST_PACK_PCT,
			                      double? PACK_ROUNDING_NTH_PACK_PCT
			                      )
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.PACK_MULTIPLE_RID.SetValue(PACK_MULTIPLE_RID);
                        this.PACK_ROUNDING_1ST_PACK_PCT.SetValue(PACK_ROUNDING_1ST_PACK_PCT);
                        this.PACK_ROUNDING_NTH_PACK_PCT.SetValue(PACK_ROUNDING_NTH_PACK_PCT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SZ_CURVE_UPD_INS_def SP_MID_MTH_SZ_CURVE_UPD_INS = new SP_MID_MTH_SZ_CURVE_UPD_INS_def();
            public class SP_MID_MTH_SZ_CURVE_UPD_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SZ_CURVE_UPD_INS.SQL"

                private intParameter METHOD_RID;
                private intParameter SIZE_GROUP_RID;
                private intParameter SIZE_CURVES_BY_TYPE;
                private intParameter SIZE_CURVES_BY_SG_RID;
                private intParameter MERCH_BASIS_EQUAL_WEIGHT_IND;
                private intParameter CURVE_BASIS_EQUAL_WEIGHT_IND;
                private intParameter APPLY_LOST_SALES_IND;
                private floatParameter MINIMUM_AVERAGE;
                private floatParameter SALES_TOLERANCE;
                private intParameter INDEX_UNITS_TYPE;
                private floatParameter MIN_TOLERANCE;
                private floatParameter MAX_TOLERANCE;
                private charParameter APPLY_MIN_TO_ZERO_TOLERANCE_IND;
			
			    public SP_MID_MTH_SZ_CURVE_UPD_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_SZ_CURVE_UPD_INS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("MTH_SZ_CURVE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVES_BY_TYPE = new intParameter("@SIZE_CURVES_BY_TYPE", base.inputParameterList);
			        SIZE_CURVES_BY_SG_RID = new intParameter("@SIZE_CURVES_BY_SG_RID", base.inputParameterList);
			        MERCH_BASIS_EQUAL_WEIGHT_IND = new intParameter("@MERCH_BASIS_EQUAL_WEIGHT_IND", base.inputParameterList);
			        CURVE_BASIS_EQUAL_WEIGHT_IND = new intParameter("@CURVE_BASIS_EQUAL_WEIGHT_IND", base.inputParameterList);
			        APPLY_LOST_SALES_IND = new intParameter("@APPLY_LOST_SALES_IND", base.inputParameterList);
			        MINIMUM_AVERAGE = new floatParameter("@MINIMUM_AVERAGE", base.inputParameterList);
			        SALES_TOLERANCE = new floatParameter("@SALES_TOLERANCE", base.inputParameterList);
			        INDEX_UNITS_TYPE = new intParameter("@INDEX_UNITS_TYPE", base.inputParameterList);
			        MIN_TOLERANCE = new floatParameter("@MIN_TOLERANCE", base.inputParameterList);
			        MAX_TOLERANCE = new floatParameter("@MAX_TOLERANCE", base.inputParameterList);
			        APPLY_MIN_TO_ZERO_TOLERANCE_IND = new charParameter("@APPLY_MIN_TO_ZERO_TOLERANCE_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SIZE_GROUP_RID,
			                      int? SIZE_CURVES_BY_TYPE,
			                      int? SIZE_CURVES_BY_SG_RID,
			                      int? MERCH_BASIS_EQUAL_WEIGHT_IND,
			                      int? CURVE_BASIS_EQUAL_WEIGHT_IND,
			                      int? APPLY_LOST_SALES_IND,
			                      double? MINIMUM_AVERAGE,
			                      double? SALES_TOLERANCE,
			                      int? INDEX_UNITS_TYPE,
			                      double? MIN_TOLERANCE,
			                      double? MAX_TOLERANCE,
			                      char? APPLY_MIN_TO_ZERO_TOLERANCE_IND
			                      )
			    {
                    lock (typeof(SP_MID_MTH_SZ_CURVE_UPD_INS_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.SIZE_CURVES_BY_TYPE.SetValue(SIZE_CURVES_BY_TYPE);
                        this.SIZE_CURVES_BY_SG_RID.SetValue(SIZE_CURVES_BY_SG_RID);
                        this.MERCH_BASIS_EQUAL_WEIGHT_IND.SetValue(MERCH_BASIS_EQUAL_WEIGHT_IND);
                        this.CURVE_BASIS_EQUAL_WEIGHT_IND.SetValue(CURVE_BASIS_EQUAL_WEIGHT_IND);
                        this.APPLY_LOST_SALES_IND.SetValue(APPLY_LOST_SALES_IND);
                        this.MINIMUM_AVERAGE.SetValue(MINIMUM_AVERAGE);
                        this.SALES_TOLERANCE.SetValue(SALES_TOLERANCE);
                        this.INDEX_UNITS_TYPE.SetValue(INDEX_UNITS_TYPE);
                        this.MIN_TOLERANCE.SetValue(MIN_TOLERANCE);
                        this.MAX_TOLERANCE.SetValue(MAX_TOLERANCE);
                        this.APPLY_MIN_TO_ZERO_TOLERANCE_IND.SetValue(APPLY_MIN_TO_ZERO_TOLERANCE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE_def MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE = new MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE_def();
			public class MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_SIZE_CURVE_CRVE_BAS_DET");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT_def MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT = new MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT_def();
            public class MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BASIS_SEQ;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private floatParameter WEIGHT;
                private intParameter MERCH_TYPE;
                private intParameter OLL_RID;
                private intParameter CUSTOM_OLL_RID;
            

                public MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_SIZE_CURVE_MRCH_BAS_DET");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    BASIS_SEQ = new intParameter("@BASIS_SEQ", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
                    MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
                    OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
                  
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? BASIS_SEQ,
                                  int? HN_RID,
                                  int? FV_RID,
                                  int? CDR_RID,
                                  double? WEIGHT,
                                  int? MERCH_TYPE,
                                  int? OLL_RID,
                                  int? CUSTOM_OLL_RID
                                  )
                {
                    lock (typeof(MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BASIS_SEQ.SetValue(BASIS_SEQ);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);

                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE_def MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE = new MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE_def();
            public class MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_SIZE_CURVE_MRCH_BAS_DET");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

			public static MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT_def MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT = new MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT_def();
			public class MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BASIS_SEQ;
                private intParameter SIZE_CURVE_GROUP_RID;
                private floatParameter WEIGHT;
			
			    public MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_SIZE_CURVE_CRVE_BAS_DET");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        BASIS_SEQ = new intParameter("@BASIS_SEQ", base.inputParameterList);
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? BASIS_SEQ,
			                      int? SIZE_CURVE_GROUP_RID,
			                      double? WEIGHT
			                      )
			    {
                    lock (typeof(MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BASIS_SEQ.SetValue(BASIS_SEQ);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SZ_CURVE_DEL_MTH_def SP_MID_MTH_SZ_CURVE_DEL_MTH = new SP_MID_MTH_SZ_CURVE_DEL_MTH_def();
            public class SP_MID_MTH_SZ_CURVE_DEL_MTH_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SZ_CURVE_DEL_MTH.SQL"

                private intParameter METHOD_RID;
			
			    public SP_MID_MTH_SZ_CURVE_DEL_MTH_def()
			    {
                    base.procedureName = "SP_MID_MTH_SZ_CURVE_DEL_MTH";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("MTH_SZ_CURVE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(SP_MID_MTH_SZ_CURVE_DEL_MTH_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SZ_NEED_DEL_MTH_def SP_MID_MTH_SZ_NEED_DEL_MTH = new SP_MID_MTH_SZ_NEED_DEL_MTH_def();
            public class SP_MID_MTH_SZ_NEED_DEL_MTH_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SZ_NEED_DEL_MTH.SQL"

                private intParameter METHODRID;
			
			    public SP_MID_MTH_SZ_NEED_DEL_MTH_def()
			    {
                    base.procedureName = "SP_MID_MTH_SZ_NEED_DEL_MTH";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("MTH_SZ_NEED");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHODRID)
			    {
                    lock (typeof(SP_MID_MTH_SZ_NEED_DEL_MTH_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_SZ_NEED_UPD_INS_def SP_MID_MTH_SZ_NEED_UPD_INS = new SP_MID_MTH_SZ_NEED_UPD_INS_def();
            public class SP_MID_MTH_SZ_NEED_UPD_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_SZ_NEED_UPD_INS.SQL"

                private intParameter METHODRID;
                private intParameter SIZEGROUPRID;
                private intParameter SIZECURVEGROUPRID;
                private intParameter MERCHTYPE;
                private intParameter MERCHHNRID;
                private intParameter MERCHPHRID;
                private intParameter MERCHPHLSEQ;
                private floatParameter AVGPACKDEVIATION;
                private floatParameter MAXPACKNEED;
                private intParameter SIZEALTERNATERID;
                private intParameter SIZECONSTRAINTRID;
                private intParameter GENCURVE_HCG_RID;
                private intParameter GENCURVE_HN_RID;
                private intParameter GENCURVE_PH_RID;
                private intParameter GENCURVE_PHL_SEQUENCE;
                private charParameter GENCURVE_COLOR_IND;
                private intParameter GENCURVE_MERCH_TYPE;
                private intParameter GENCONSTRAINT_HCG_RID;
                private intParameter GENCONSTRAINT_HN_RID;
                private intParameter GENCONSTRAINT_PH_RID;
                private intParameter GENCONSTRAINT_PHL_SEQUENCE;
                private charParameter GENCONSTRAINT_COLOR_IND;
                private intParameter GENCONSTRAINT_MERCH_TYPE;
                private charParameter NORMALIZE_SIZE_CURVES_IND;
                private charParameter OVERRIDE_AVG_PACK_DEV_IND;
                private charParameter OVERRIDE_MAX_PACK_NEED_IND;
                private charParameter USE_DEFAULT_CURVE_IND;
                private intParameter GENCURVE_NSCCD_RID;
                private charParameter PACK_TOLERANCE_NO_MAX_STEP_IND;
                private charParameter PACK_TOLERANCE_STEPPED_IND;
                private charParameter APPLY_RULES_ONLY_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private charParameter VSW_SIZE_CONSTRAINTS_IND;
                private intParameter VSW_SIZE_CONSTRAINTS;
			
			    public SP_MID_MTH_SZ_NEED_UPD_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_SZ_NEED_UPD_INS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("MTH_SZ_NEED");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        SIZEGROUPRID = new intParameter("@SIZEGROUPRID", base.inputParameterList);
			        SIZECURVEGROUPRID = new intParameter("@SIZECURVEGROUPRID", base.inputParameterList);
			        MERCHTYPE = new intParameter("@MERCHTYPE", base.inputParameterList);
			        MERCHHNRID = new intParameter("@MERCHHNRID", base.inputParameterList);
			        MERCHPHRID = new intParameter("@MERCHPHRID", base.inputParameterList);
			        MERCHPHLSEQ = new intParameter("@MERCHPHLSEQ", base.inputParameterList);
			        AVGPACKDEVIATION = new floatParameter("@AVGPACKDEVIATION", base.inputParameterList);
			        MAXPACKNEED = new floatParameter("@MAXPACKNEED", base.inputParameterList);
			        SIZEALTERNATERID = new intParameter("@SIZEALTERNATERID", base.inputParameterList);
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        GENCURVE_HCG_RID = new intParameter("@GENCURVE_HCG_RID", base.inputParameterList);
			        GENCURVE_HN_RID = new intParameter("@GENCURVE_HN_RID", base.inputParameterList);
			        GENCURVE_PH_RID = new intParameter("@GENCURVE_PH_RID", base.inputParameterList);
			        GENCURVE_PHL_SEQUENCE = new intParameter("@GENCURVE_PHL_SEQUENCE", base.inputParameterList);
			        GENCURVE_COLOR_IND = new charParameter("@GENCURVE_COLOR_IND", base.inputParameterList);
			        GENCURVE_MERCH_TYPE = new intParameter("@GENCURVE_MERCH_TYPE", base.inputParameterList);
			        GENCONSTRAINT_HCG_RID = new intParameter("@GENCONSTRAINT_HCG_RID", base.inputParameterList);
			        GENCONSTRAINT_HN_RID = new intParameter("@GENCONSTRAINT_HN_RID", base.inputParameterList);
			        GENCONSTRAINT_PH_RID = new intParameter("@GENCONSTRAINT_PH_RID", base.inputParameterList);
			        GENCONSTRAINT_PHL_SEQUENCE = new intParameter("@GENCONSTRAINT_PHL_SEQUENCE", base.inputParameterList);
			        GENCONSTRAINT_COLOR_IND = new charParameter("@GENCONSTRAINT_COLOR_IND", base.inputParameterList);
			        GENCONSTRAINT_MERCH_TYPE = new intParameter("@GENCONSTRAINT_MERCH_TYPE", base.inputParameterList);
			        NORMALIZE_SIZE_CURVES_IND = new charParameter("@NORMALIZE_SIZE_CURVES_IND", base.inputParameterList);
			        OVERRIDE_AVG_PACK_DEV_IND = new charParameter("@OVERRIDE_AVG_PACK_DEV_IND", base.inputParameterList);
			        OVERRIDE_MAX_PACK_NEED_IND = new charParameter("@OVERRIDE_MAX_PACK_NEED_IND", base.inputParameterList);
			        USE_DEFAULT_CURVE_IND = new charParameter("@USE_DEFAULT_CURVE_IND", base.inputParameterList);
			        GENCURVE_NSCCD_RID = new intParameter("@GENCURVE_NSCCD_RID", base.inputParameterList);
			        PACK_TOLERANCE_NO_MAX_STEP_IND = new charParameter("@PACK_TOLERANCE_NO_MAX_STEP_IND", base.inputParameterList);
			        PACK_TOLERANCE_STEPPED_IND = new charParameter("@PACK_TOLERANCE_STEPPED_IND", base.inputParameterList);
			        APPLY_RULES_ONLY_IND = new charParameter("@APPLY_RULES_ONLY_IND", base.inputParameterList);
			        IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
			        IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
			        IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
			        IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
			        VSW_SIZE_CONSTRAINTS_IND = new charParameter("@VSW_SIZE_CONSTRAINTS_IND", base.inputParameterList);
			        VSW_SIZE_CONSTRAINTS = new intParameter("@VSW_SIZE_CONSTRAINTS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHODRID,
			                      int? SIZEGROUPRID,
			                      int? SIZECURVEGROUPRID,
			                      int? MERCHTYPE,
			                      int? MERCHHNRID,
			                      int? MERCHPHRID,
			                      int? MERCHPHLSEQ,
			                      double? AVGPACKDEVIATION,
			                      double? MAXPACKNEED,
			                      int? SIZEALTERNATERID,
			                      int? SIZECONSTRAINTRID,
			                      int? GENCURVE_HCG_RID,
			                      int? GENCURVE_HN_RID,
			                      int? GENCURVE_PH_RID,
			                      int? GENCURVE_PHL_SEQUENCE,
			                      char? GENCURVE_COLOR_IND,
			                      int? GENCURVE_MERCH_TYPE,
			                      int? GENCONSTRAINT_HCG_RID,
			                      int? GENCONSTRAINT_HN_RID,
			                      int? GENCONSTRAINT_PH_RID,
			                      int? GENCONSTRAINT_PHL_SEQUENCE,
			                      char? GENCONSTRAINT_COLOR_IND,
			                      int? GENCONSTRAINT_MERCH_TYPE,
			                      char? NORMALIZE_SIZE_CURVES_IND,
			                      char? OVERRIDE_AVG_PACK_DEV_IND,
			                      char? OVERRIDE_MAX_PACK_NEED_IND,
			                      char? USE_DEFAULT_CURVE_IND,
			                      int? GENCURVE_NSCCD_RID,
			                      char? PACK_TOLERANCE_NO_MAX_STEP_IND,
			                      char? PACK_TOLERANCE_STEPPED_IND,
			                      char? APPLY_RULES_ONLY_IND,
			                      int? IB_MERCH_TYPE,
			                      int? IB_MERCH_HN_RID,
			                      int? IB_MERCH_PH_RID,
			                      int? IB_MERCH_PHL_SEQUENCE,
			                      char? VSW_SIZE_CONSTRAINTS_IND,
			                      int? VSW_SIZE_CONSTRAINTS
			                      )
			    {
                    lock (typeof(SP_MID_MTH_SZ_NEED_UPD_INS_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.SIZEGROUPRID.SetValue(SIZEGROUPRID);
                        this.SIZECURVEGROUPRID.SetValue(SIZECURVEGROUPRID);
                        this.MERCHTYPE.SetValue(MERCHTYPE);
                        this.MERCHHNRID.SetValue(MERCHHNRID);
                        this.MERCHPHRID.SetValue(MERCHPHRID);
                        this.MERCHPHLSEQ.SetValue(MERCHPHLSEQ);
                        this.AVGPACKDEVIATION.SetValue(AVGPACKDEVIATION);
                        this.MAXPACKNEED.SetValue(MAXPACKNEED);
                        this.SIZEALTERNATERID.SetValue(SIZEALTERNATERID);
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.GENCURVE_HCG_RID.SetValue(GENCURVE_HCG_RID);
                        this.GENCURVE_HN_RID.SetValue(GENCURVE_HN_RID);
                        this.GENCURVE_PH_RID.SetValue(GENCURVE_PH_RID);
                        this.GENCURVE_PHL_SEQUENCE.SetValue(GENCURVE_PHL_SEQUENCE);
                        this.GENCURVE_COLOR_IND.SetValue(GENCURVE_COLOR_IND);
                        this.GENCURVE_MERCH_TYPE.SetValue(GENCURVE_MERCH_TYPE);
                        this.GENCONSTRAINT_HCG_RID.SetValue(GENCONSTRAINT_HCG_RID);
                        this.GENCONSTRAINT_HN_RID.SetValue(GENCONSTRAINT_HN_RID);
                        this.GENCONSTRAINT_PH_RID.SetValue(GENCONSTRAINT_PH_RID);
                        this.GENCONSTRAINT_PHL_SEQUENCE.SetValue(GENCONSTRAINT_PHL_SEQUENCE);
                        this.GENCONSTRAINT_COLOR_IND.SetValue(GENCONSTRAINT_COLOR_IND);
                        this.GENCONSTRAINT_MERCH_TYPE.SetValue(GENCONSTRAINT_MERCH_TYPE);
                        this.NORMALIZE_SIZE_CURVES_IND.SetValue(NORMALIZE_SIZE_CURVES_IND);
                        this.OVERRIDE_AVG_PACK_DEV_IND.SetValue(OVERRIDE_AVG_PACK_DEV_IND);
                        this.OVERRIDE_MAX_PACK_NEED_IND.SetValue(OVERRIDE_MAX_PACK_NEED_IND);
                        this.USE_DEFAULT_CURVE_IND.SetValue(USE_DEFAULT_CURVE_IND);
                        this.GENCURVE_NSCCD_RID.SetValue(GENCURVE_NSCCD_RID);
                        this.PACK_TOLERANCE_NO_MAX_STEP_IND.SetValue(PACK_TOLERANCE_NO_MAX_STEP_IND);
                        this.PACK_TOLERANCE_STEPPED_IND.SetValue(PACK_TOLERANCE_STEPPED_IND);
                        this.APPLY_RULES_ONLY_IND.SetValue(APPLY_RULES_ONLY_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.VSW_SIZE_CONSTRAINTS_IND.SetValue(VSW_SIZE_CONSTRAINTS_IND);
                        this.VSW_SIZE_CONSTRAINTS.SetValue(VSW_SIZE_CONSTRAINTS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OTS_PLAN_INSERT_def MID_OTS_PLAN_INSERT = new MID_OTS_PLAN_INSERT_def();
			public class MID_OTS_PLAN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OTS_PLAN_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter PLAN_HN_RID;
                private intParameter PLAN_FV_RID;
                private intParameter CDR_RID;
                private intParameter CHAIN_FV_RID;
                private charParameter BAL_SALES_IND;
                private charParameter BAL_STOCK_IND;
                private intParameter FORECAST_MOD_RID;
                private charParameter HIGH_LEVEL_IND;
                private charParameter LOW_LEVELS_IND;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter OLL_RID;
                private charParameter TREND_OPTIONS_IND;
                private floatParameter TREND_OPTIONS_PLUG_CHAIN_WOS;
			
			    public MID_OTS_PLAN_INSERT_def()
			    {
			        base.procedureName = "MID_OTS_PLAN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OTS_PLAN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        PLAN_HN_RID = new intParameter("@PLAN_HN_RID", base.inputParameterList);
			        PLAN_FV_RID = new intParameter("@PLAN_FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        CHAIN_FV_RID = new intParameter("@CHAIN_FV_RID", base.inputParameterList);
			        BAL_SALES_IND = new charParameter("@BAL_SALES_IND", base.inputParameterList);
			        BAL_STOCK_IND = new charParameter("@BAL_STOCK_IND", base.inputParameterList);
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			        HIGH_LEVEL_IND = new charParameter("@HIGH_LEVEL_IND", base.inputParameterList);
			        LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        TREND_OPTIONS_IND = new charParameter("@TREND_OPTIONS_IND", base.inputParameterList);
			        TREND_OPTIONS_PLUG_CHAIN_WOS = new floatParameter("@TREND_OPTIONS_PLUG_CHAIN_WOS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? PLAN_HN_RID,
			                      int? PLAN_FV_RID,
			                      int? CDR_RID,
			                      int? CHAIN_FV_RID,
			                      char? BAL_SALES_IND,
			                      char? BAL_STOCK_IND,
			                      int? FORECAST_MOD_RID,
			                      char? HIGH_LEVEL_IND,
			                      char? LOW_LEVELS_IND,
			                      int? LOW_LEVEL_TYPE,
			                      int? LOW_LEVEL_SEQ,
			                      int? LOW_LEVEL_OFFSET,
			                      int? OLL_RID,
			                      char? TREND_OPTIONS_IND,
			                      double? TREND_OPTIONS_PLUG_CHAIN_WOS
			                      )
			    {
                    lock (typeof(MID_OTS_PLAN_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.PLAN_HN_RID.SetValue(PLAN_HN_RID);
                        this.PLAN_FV_RID.SetValue(PLAN_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.CHAIN_FV_RID.SetValue(CHAIN_FV_RID);
                        this.BAL_SALES_IND.SetValue(BAL_SALES_IND);
                        this.BAL_STOCK_IND.SetValue(BAL_STOCK_IND);
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        this.HIGH_LEVEL_IND.SetValue(HIGH_LEVEL_IND);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.TREND_OPTIONS_IND.SetValue(TREND_OPTIONS_IND);
                        this.TREND_OPTIONS_PLUG_CHAIN_WOS.SetValue(TREND_OPTIONS_PLUG_CHAIN_WOS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD_def MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD = new MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD_def();
			public class MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT_def MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT = new MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT_def();
			public class MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SGL_RID;
			
			    public MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_GLOBAL_UNLOCK_GRP_LVL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OTS_PLAN_UPDATE_def MID_OTS_PLAN_UPDATE = new MID_OTS_PLAN_UPDATE_def();
			public class MID_OTS_PLAN_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OTS_PLAN_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter PLAN_HN_RID;
                private intParameter PLAN_FV_RID;
                private intParameter CDR_RID;
                private intParameter CHAIN_FV_RID;
                private charParameter BAL_SALES_IND;
                private charParameter BAL_STOCK_IND;
                private intParameter FORECAST_MOD_RID;
                private charParameter HIGH_LEVEL_IND;
                private charParameter LOW_LEVELS_IND;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter OLL_RID;
                private charParameter TREND_OPTIONS_IND;
                private floatParameter TREND_OPTIONS_PLUG_CHAIN_WOS;
			
			    public MID_OTS_PLAN_UPDATE_def()
			    {
			        base.procedureName = "MID_OTS_PLAN_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("OTS_PLAN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        PLAN_HN_RID = new intParameter("@PLAN_HN_RID", base.inputParameterList);
			        PLAN_FV_RID = new intParameter("@PLAN_FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        CHAIN_FV_RID = new intParameter("@CHAIN_FV_RID", base.inputParameterList);
			        BAL_SALES_IND = new charParameter("@BAL_SALES_IND", base.inputParameterList);
			        BAL_STOCK_IND = new charParameter("@BAL_STOCK_IND", base.inputParameterList);
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			        HIGH_LEVEL_IND = new charParameter("@HIGH_LEVEL_IND", base.inputParameterList);
			        LOW_LEVELS_IND = new charParameter("@LOW_LEVELS_IND", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        TREND_OPTIONS_IND = new charParameter("@TREND_OPTIONS_IND", base.inputParameterList);
			        TREND_OPTIONS_PLUG_CHAIN_WOS = new floatParameter("@TREND_OPTIONS_PLUG_CHAIN_WOS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? PLAN_HN_RID,
			                      int? PLAN_FV_RID,
			                      int? CDR_RID,
			                      int? CHAIN_FV_RID,
			                      char? BAL_SALES_IND,
			                      char? BAL_STOCK_IND,
			                      int? FORECAST_MOD_RID,
			                      char? HIGH_LEVEL_IND,
			                      char? LOW_LEVELS_IND,
			                      int? LOW_LEVEL_TYPE,
			                      int? LOW_LEVEL_SEQ,
			                      int? LOW_LEVEL_OFFSET,
			                      int? OLL_RID,
			                      char? TREND_OPTIONS_IND,
			                      double? TREND_OPTIONS_PLUG_CHAIN_WOS
			                      )
			    {
                    lock (typeof(MID_OTS_PLAN_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.PLAN_HN_RID.SetValue(PLAN_HN_RID);
                        this.PLAN_FV_RID.SetValue(PLAN_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.CHAIN_FV_RID.SetValue(CHAIN_FV_RID);
                        this.BAL_SALES_IND.SetValue(BAL_SALES_IND);
                        this.BAL_STOCK_IND.SetValue(BAL_STOCK_IND);
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        this.HIGH_LEVEL_IND.SetValue(HIGH_LEVEL_IND);
                        this.LOW_LEVELS_IND.SetValue(LOW_LEVELS_IND);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.TREND_OPTIONS_IND.SetValue(TREND_OPTIONS_IND);
                        this.TREND_OPTIONS_PLUG_CHAIN_WOS.SetValue(TREND_OPTIONS_PLUG_CHAIN_WOS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OTS_PLAN_DELETE_def MID_OTS_PLAN_DELETE = new MID_OTS_PLAN_DELETE_def();
			public class MID_OTS_PLAN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OTS_PLAN_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_OTS_PLAN_DELETE_def()
			    {
			        base.procedureName = "MID_OTS_PLAN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OTS_PLAN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_OTS_PLAN_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}


			public static MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT_def MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT = new MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT_def();
			public class MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter DETAIL_SEQ;
                private intParameter FROM_LEVEL_HRID;
                private intParameter FROM_LEVEL_TYPE;
                private intParameter FROM_LEVEL_SEQ;
                private intParameter FROM_LEVEL_OFFSET;
                private intParameter TO_LEVEL_HRID;
                private intParameter TO_LEVEL_TYPE;
                private intParameter TO_LEVEL_SEQ;
                private intParameter TO_LEVEL_OFFSET;
                private intParameter STORE_IND;
                private intParameter CHAIN_IND;
                private intParameter STORE_TO_CHAIN_IND;        //TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved
			
			    public MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_ROLLUP_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        DETAIL_SEQ = new intParameter("@DETAIL_SEQ", base.inputParameterList);
			        FROM_LEVEL_HRID = new intParameter("@FROM_LEVEL_HRID", base.inputParameterList);
			        FROM_LEVEL_TYPE = new intParameter("@FROM_LEVEL_TYPE", base.inputParameterList);
			        FROM_LEVEL_SEQ = new intParameter("@FROM_LEVEL_SEQ", base.inputParameterList);
			        FROM_LEVEL_OFFSET = new intParameter("@FROM_LEVEL_OFFSET", base.inputParameterList);
			        TO_LEVEL_HRID = new intParameter("@TO_LEVEL_HRID", base.inputParameterList);
			        TO_LEVEL_TYPE = new intParameter("@TO_LEVEL_TYPE", base.inputParameterList);
			        TO_LEVEL_SEQ = new intParameter("@TO_LEVEL_SEQ", base.inputParameterList);
			        TO_LEVEL_OFFSET = new intParameter("@TO_LEVEL_OFFSET", base.inputParameterList);
			        STORE_IND = new intParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new intParameter("@CHAIN_IND", base.inputParameterList);
                    STORE_TO_CHAIN_IND = new intParameter("@STORE_TO_CHAIN_IND", base.inputParameterList); //TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? DETAIL_SEQ,
			                      int? FROM_LEVEL_HRID,
			                      int? FROM_LEVEL_TYPE,
			                      int? FROM_LEVEL_SEQ,
			                      int? FROM_LEVEL_OFFSET,
			                      int? TO_LEVEL_HRID,
			                      int? TO_LEVEL_TYPE,
			                      int? TO_LEVEL_SEQ,
			                      int? TO_LEVEL_OFFSET,
			                      int? STORE_IND,
			                      int? CHAIN_IND,
                                  int? STORE_TO_CHAIN_IND //TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved
			                      )
			    {
                    lock (typeof(MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.DETAIL_SEQ.SetValue(DETAIL_SEQ);
                        this.FROM_LEVEL_HRID.SetValue(FROM_LEVEL_HRID);
                        this.FROM_LEVEL_TYPE.SetValue(FROM_LEVEL_TYPE);
                        this.FROM_LEVEL_SEQ.SetValue(FROM_LEVEL_SEQ);
                        this.FROM_LEVEL_OFFSET.SetValue(FROM_LEVEL_OFFSET);
                        this.TO_LEVEL_HRID.SetValue(TO_LEVEL_HRID);
                        this.TO_LEVEL_TYPE.SetValue(TO_LEVEL_TYPE);
                        this.TO_LEVEL_SEQ.SetValue(TO_LEVEL_SEQ);
                        this.TO_LEVEL_OFFSET.SetValue(TO_LEVEL_OFFSET);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_TO_CHAIN_IND.SetValue(STORE_TO_CHAIN_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_BASIS_DETAIL_READ_def MID_METHOD_ROLLUP_BASIS_DETAIL_READ = new MID_METHOD_ROLLUP_BASIS_DETAIL_READ_def();
			public class MID_METHOD_ROLLUP_BASIS_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_BASIS_DETAIL_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_ROLLUP_BASIS_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_BASIS_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_ROLLUP_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_ROLLUP_BASIS_DETAIL_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_READ_def MID_METHOD_ROLLUP_READ = new MID_METHOD_ROLLUP_READ_def();
			public class MID_METHOD_ROLLUP_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_READ.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
			
			    public MID_METHOD_ROLLUP_READ_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_ROLLUP");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? HN_RID,
			                          int? FV_RID,
			                          int? CDR_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_ROLLUP_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_RULE_READ_def MID_METHOD_RULE_READ = new MID_METHOD_RULE_READ_def();
			public class MID_METHOD_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_RULE_READ_def()
			    {
			        base.procedureName = "MID_METHOD_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_RULE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_RULE_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_METHOD_RULE_INSERT_def MID_METHOD_RULE_INSERT = new MID_METHOD_RULE_INSERT_def();
            public class  MID_METHOD_RULE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter HDR_RID;
                private intParameter STORE_ORDER;
                private intParameter HEADER_COMPONENT;
                private intParameter HDR_PACK_RID;
                private intParameter HDR_BC_RID;
                private intParameter INCLUDED_STORES;
                private intParameter INCLUDED_QUANTITY;
                private intParameter EXCLUDED_STORES;
                private intParameter EXCLUDED_QUANTITY;
                private intParameter SGL_RID;
                private charParameter IS_HEADER_MASTER;
                private charParameter INCLUDE_RESERVE_IND;

                public MID_METHOD_RULE_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_RULE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_RULE");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    STORE_ORDER = new intParameter("@STORE_ORDER", base.inputParameterList);
                    HEADER_COMPONENT = new intParameter("@HEADER_COMPONENT", base.inputParameterList);
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    INCLUDED_STORES = new intParameter("@INCLUDED_STORES", base.inputParameterList);
                    INCLUDED_QUANTITY = new intParameter("@INCLUDED_QUANTITY", base.inputParameterList);
                    EXCLUDED_STORES = new intParameter("@EXCLUDED_STORES", base.inputParameterList);
                    EXCLUDED_QUANTITY = new intParameter("@EXCLUDED_QUANTITY", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    IS_HEADER_MASTER = new charParameter("@IS_HEADER_MASTER", base.inputParameterList);
                    INCLUDE_RESERVE_IND = new charParameter("@INCLUDE_RESERVE_IND", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? STORE_FILTER_RID,
                                  int? HDR_RID,
                                  int? STORE_ORDER,
                                  int? HEADER_COMPONENT,
                                  int? HDR_PACK_RID,
                                  int? HDR_BC_RID,
                                  int? INCLUDED_STORES,
                                  int? INCLUDED_QUANTITY,
                                  int? EXCLUDED_STORES,
                                  int? EXCLUDED_QUANTITY,
                                  int? SGL_RID,
                                  char? IS_HEADER_MASTER,
                                  char? INCLUDE_RESERVE_IND
                                  )
                {
                    lock (typeof(MID_METHOD_RULE_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.STORE_ORDER.SetValue(STORE_ORDER);
                        this.HEADER_COMPONENT.SetValue(HEADER_COMPONENT);
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.INCLUDED_STORES.SetValue(INCLUDED_STORES);
                        this.INCLUDED_QUANTITY.SetValue(INCLUDED_QUANTITY);
                        this.EXCLUDED_STORES.SetValue(EXCLUDED_STORES);
                        this.EXCLUDED_QUANTITY.SetValue(EXCLUDED_QUANTITY);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.IS_HEADER_MASTER.SetValue(IS_HEADER_MASTER);
                        this.INCLUDE_RESERVE_IND.SetValue(INCLUDE_RESERVE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_RULE_UPDATE_def MID_METHOD_RULE_UPDATE = new MID_METHOD_RULE_UPDATE_def();
            public class MID_METHOD_RULE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_FILTER_RID;
                private intParameter HDR_RID;
                private intParameter STORE_ORDER;
                private intParameter HEADER_COMPONENT;
                private intParameter HDR_PACK_RID;
                private intParameter HDR_BC_RID;
                private intParameter INCLUDED_STORES;
                private intParameter INCLUDED_QUANTITY;
                private intParameter EXCLUDED_STORES;
                private intParameter EXCLUDED_QUANTITY;
                private intParameter SGL_RID;
                private charParameter IS_HEADER_MASTER;
                private charParameter INCLUDE_RESERVE_IND;

                public MID_METHOD_RULE_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_RULE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_RULE");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    STORE_ORDER = new intParameter("@STORE_ORDER", base.inputParameterList);
                    HEADER_COMPONENT = new intParameter("@HEADER_COMPONENT", base.inputParameterList);
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    INCLUDED_STORES = new intParameter("@INCLUDED_STORES", base.inputParameterList);
                    INCLUDED_QUANTITY = new intParameter("@INCLUDED_QUANTITY", base.inputParameterList);
                    EXCLUDED_STORES = new intParameter("@EXCLUDED_STORES", base.inputParameterList);
                    EXCLUDED_QUANTITY = new intParameter("@EXCLUDED_QUANTITY", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    IS_HEADER_MASTER = new charParameter("@IS_HEADER_MASTER", base.inputParameterList);
                    INCLUDE_RESERVE_IND = new charParameter("@INCLUDE_RESERVE_IND", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? STORE_FILTER_RID,
                                  int? HDR_RID,
                                  int? STORE_ORDER,
                                  int? HEADER_COMPONENT,
                                  int? HDR_PACK_RID,
                                  int? HDR_BC_RID,
                                  int? INCLUDED_STORES,
                                  int? INCLUDED_QUANTITY,
                                  int? EXCLUDED_STORES,
                                  int? EXCLUDED_QUANTITY,
                                  int? SGL_RID,
                                  char? IS_HEADER_MASTER,
                                  char? INCLUDE_RESERVE_IND
                                  )
                {
                    lock (typeof(MID_METHOD_RULE_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.STORE_ORDER.SetValue(STORE_ORDER);
                        this.HEADER_COMPONENT.SetValue(HEADER_COMPONENT);
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.INCLUDED_STORES.SetValue(INCLUDED_STORES);
                        this.INCLUDED_QUANTITY.SetValue(INCLUDED_QUANTITY);
                        this.EXCLUDED_STORES.SetValue(EXCLUDED_STORES);
                        this.EXCLUDED_QUANTITY.SetValue(EXCLUDED_QUANTITY);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.IS_HEADER_MASTER.SetValue(IS_HEADER_MASTER);
                        this.INCLUDE_RESERVE_IND.SetValue(INCLUDE_RESERVE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_METHOD_RULE_DELETE_def MID_METHOD_RULE_DELETE = new MID_METHOD_RULE_DELETE_def();
            public class MID_METHOD_RULE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_RULE_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_RULE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_RULE");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_RULE_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

			public static MID_METHOD_OVERRIDE_READ_def MID_METHOD_OVERRIDE_READ = new MID_METHOD_OVERRIDE_READ_def();
			public class MID_METHOD_OVERRIDE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_OVERRIDE_READ_def()
			    {
			        base.procedureName = "MID_METHOD_OVERRIDE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_OVERRIDE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_OVERRIDE_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ_def MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ = new MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ_def();
			public class MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_CURVE_CRVE_BAS_DET");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ_def MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ = new MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ_def();
			public class MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ.SQL"

                private intParameter METHOD_RID;
                private intParameter MERCH_TYPE;
			
			    public MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_CURVE_MRCH_BAS_DET");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? MERCH_TYPE
			                          )
			    {
                    lock (typeof(MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_CURVE_READ_def MID_METHOD_SIZE_CURVE_READ = new MID_METHOD_SIZE_CURVE_READ_def();
			public class MID_METHOD_SIZE_CURVE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_CURVE_READ.SQL"

                private intParameter METHOD_RID;
                private intParameter INDEX_UNITS_TYPE;
			
			    public MID_METHOD_SIZE_CURVE_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_CURVE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_CURVE");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        INDEX_UNITS_TYPE = new intParameter("@INDEX_UNITS_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? INDEX_UNITS_TYPE
			                          )
			    {
                    lock (typeof(MID_METHOD_SIZE_CURVE_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.INDEX_UNITS_TYPE.SetValue(INDEX_UNITS_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_SIZE_NEED_READ_def MID_METHOD_SIZE_NEED_READ = new MID_METHOD_SIZE_NEED_READ_def();
			public class MID_METHOD_SIZE_NEED_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_SIZE_NEED_READ.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_SIZE_NEED_READ_def()
			    {
			        base.procedureName = "MID_METHOD_SIZE_NEED_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_SIZE_NEED");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_SIZE_NEED_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_RULE_GET_COLOR_SZ_def SP_MID_MTH_RULE_GET_COLOR_SZ = new SP_MID_MTH_RULE_GET_COLOR_SZ_def();
            public class SP_MID_MTH_RULE_GET_COLOR_SZ_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_RULE_GET_COLOR_SZ.SQL"

                private intParameter METHODRID;
                private intParameter ROWTYPEID;
                private intParameter SGRID;
                private intParameter SG_VERSION;  // TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
			
			    public SP_MID_MTH_RULE_GET_COLOR_SZ_def()
			    {
                    base.procedureName = "SP_MID_MTH_RULE_GET_COLOR_SZ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("MTH_RULE_GET_COLOR_SZ");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        ROWTYPEID = new intParameter("@ROWTYPEID", base.inputParameterList);
			        SGRID = new intParameter("@SGRID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHODRID,
			                          int? ROWTYPEID,
                                      int? SGRID,
                                      int? SG_VERSION  // TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
			                          )
			    {
                    lock (typeof(SP_MID_MTH_RULE_GET_COLOR_SZ_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.ROWTYPEID.SetValue(ROWTYPEID);
                        this.SGRID.SetValue(SGRID);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_RULE_GET_GRPLVL_def SP_MID_MTH_RULE_GET_GRPLVL = new SP_MID_MTH_RULE_GET_GRPLVL_def();
            public class SP_MID_MTH_RULE_GET_GRPLVL_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_RULE_GET_GRPLVL.SQL"

                private intParameter METHODRID;
                private intParameter SGRID;
                private intParameter SG_VERSION;  // TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
			
			    public SP_MID_MTH_RULE_GET_GRPLVL_def()
			    {
                    base.procedureName = "SP_MID_MTH_RULE_GET_GRPLVL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("MTH_RULE_GET_GRPLVL");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        SGRID = new intParameter("@SGRID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHODRID,
			                          int? SGRID,
                                      int? SG_VERSION  // TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
			                          )
			    {
                    lock (typeof(SP_MID_MTH_RULE_GET_GRPLVL_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.SGRID.SetValue(SGRID);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OTS_PLAN_READ_FROM_METHOD_def MID_OTS_PLAN_READ_FROM_METHOD = new MID_OTS_PLAN_READ_FROM_METHOD_def();
			public class MID_OTS_PLAN_READ_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OTS_PLAN_READ_FROM_METHOD.SQL"

                private intParameter METHOD_RID;
			
			    public MID_OTS_PLAN_READ_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_OTS_PLAN_READ_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OTS_PLAN");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_OTS_PLAN_READ_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_INSERT_def MID_METHOD_ROLLUP_INSERT = new MID_METHOD_ROLLUP_INSERT_def();
			public class MID_METHOD_ROLLUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
			
			    public MID_METHOD_ROLLUP_INSERT_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("METHOD_ROLLUP");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_ROLLUP_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_UPDATE_def MID_METHOD_ROLLUP_UPDATE = new MID_METHOD_ROLLUP_UPDATE_def();
			public class MID_METHOD_ROLLUP_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
			
			    public MID_METHOD_ROLLUP_UPDATE_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_ROLLUP");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID
			                      )
			    {
                    lock (typeof(MID_METHOD_ROLLUP_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_DELETE_def MID_METHOD_ROLLUP_DELETE = new MID_METHOD_ROLLUP_DELETE_def();
			public class MID_METHOD_ROLLUP_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_ROLLUP_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_ROLLUP");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_ROLLUP_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE_def MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE = new MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE_def();
			public class MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("METHOD_ROLLUP_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE_def MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE = new MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE_def();
			public class MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
			
			    public MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_ROLLUP_BASIS_DETAIL");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID,
			                          int? CDR_RID
			                          )
			    {
                    lock (typeof(MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD_def MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD = new MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD_def();
			public class MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD.SQL"

                private intParameter METHOD_RID;
			
			    public MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD_ROLLUP_BASIS_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_RULE_GL_INS_def SP_MID_MTH_RULE_GL_INS = new SP_MID_MTH_RULE_GL_INS_def();
            public class SP_MID_MTH_RULE_GL_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_RULE_GL_INS.SQL"

                private intParameter METHODRID;
                private intParameter SGLRID;
                private intParameter RULE;
                private intParameter QTY;
                private intParameter ROWTYPEID;
			
			    public SP_MID_MTH_RULE_GL_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_RULE_GL_INS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("MTH_RULE_GL");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        SGLRID = new intParameter("@SGLRID", base.inputParameterList);
			        RULE = new intParameter("@RULE", base.inputParameterList);
			        QTY = new intParameter("@QTY", base.inputParameterList);
			        ROWTYPEID = new intParameter("@ROWTYPEID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHODRID,
			                      int? SGLRID,
			                      int? RULE,
			                      int? QTY,
			                      int? ROWTYPEID
			                      )
			    {
                    lock (typeof(SP_MID_MTH_RULE_GL_INS_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.SGLRID.SetValue(SGLRID);
                        this.RULE.SetValue(RULE);
                        this.QTY.SetValue(QTY);
                        this.ROWTYPEID.SetValue(ROWTYPEID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_RULE_CS_INS_def SP_MID_MTH_RULE_CS_INS = new SP_MID_MTH_RULE_CS_INS_def();
            public class SP_MID_MTH_RULE_CS_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_RULE_CS_INS.SQL"

                private intParameter METHODRID;
                private intParameter SGLRID;
                private intParameter COLORCODERID;
                private intParameter SIZESRID;
                private intParameter SIZECODERID;
                private intParameter RULE;
                private intParameter QTY;
                private intParameter ROWTYPEID;
                private intParameter DIMENSIONS_RID;
			
			    public SP_MID_MTH_RULE_CS_INS_def()
			    {
                    base.procedureName = "SP_MID_MTH_RULE_CS_INS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("MTH_RULE_CS");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
			        SGLRID = new intParameter("@SGLRID", base.inputParameterList);
			        COLORCODERID = new intParameter("@COLORCODERID", base.inputParameterList);
			        SIZESRID = new intParameter("@SIZESRID", base.inputParameterList);
			        SIZECODERID = new intParameter("@SIZECODERID", base.inputParameterList);
			        RULE = new intParameter("@RULE", base.inputParameterList);
			        QTY = new intParameter("@QTY", base.inputParameterList);
			        ROWTYPEID = new intParameter("@ROWTYPEID", base.inputParameterList);
			        DIMENSIONS_RID = new intParameter("@DIMENSIONS_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHODRID,
			                      int? SGLRID,
			                      int? COLORCODERID,
			                      int? SIZESRID,
			                      int? SIZECODERID,
			                      int? RULE,
			                      int? QTY,
			                      int? ROWTYPEID,
			                      int? DIMENSIONS_RID
			                      )
			    {
                    lock (typeof(SP_MID_MTH_RULE_CS_INS_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.SGLRID.SetValue(SGLRID);
                        this.COLORCODERID.SetValue(COLORCODERID);
                        this.SIZESRID.SetValue(SIZESRID);
                        this.SIZECODERID.SetValue(SIZECODERID);
                        this.RULE.SetValue(RULE);
                        this.QTY.SetValue(QTY);
                        this.ROWTYPEID.SetValue(ROWTYPEID);
                        this.DIMENSIONS_RID.SetValue(DIMENSIONS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_MTH_RULE_DEL_CHILDREN_def SP_MID_MTH_RULE_DEL_CHILDREN = new SP_MID_MTH_RULE_DEL_CHILDREN_def();
            public class SP_MID_MTH_RULE_DEL_CHILDREN_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MTH_RULE_DEL_CHILDREN.SQL"

                private intParameter METHODRID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
			
			    public SP_MID_MTH_RULE_DEL_CHILDREN_def()
			    {
                    base.procedureName = "SP_MID_MTH_RULE_DEL_CHILDREN";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("MTH_RULE");
			        METHODRID = new intParameter("@METHODRID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
			    }
			
			    public int Delete(DatabaseAccess _dba, int? METHODRID)
			    {
                    lock (typeof(SP_MID_MTH_RULE_DEL_CHILDREN_def))
                    {
                        this.METHODRID.SetValue(METHODRID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_METHOD_GROUP_ALLOCATION_READ_def MID_METHOD_GROUP_ALLOCATION_READ = new MID_METHOD_GROUP_ALLOCATION_READ_def();
            public class MID_METHOD_GROUP_ALLOCATION_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_READ_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_INSERT_def MID_METHOD_GROUP_ALLOCATION_INSERT = new MID_METHOD_GROUP_ALLOCATION_INSERT_def();
            public class MID_METHOD_GROUP_ALLOCATION_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_GRADE_TIMEFRAME;
                private charParameter EXCEED_MAX_IND;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter ON_HAND_HN_RID;
                private intParameter ON_HAND_PH_RID;
                private intParameter ON_HAND_PHL_SEQUENCE;
                private floatParameter ON_HAND_FACTOR;
                private intParameter SG_RID;
                private charParameter MERCH_UNSPECIFIED;
                private charParameter ON_HAND_UNSPECIFIED;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
                private intParameter STORE_GRADES_SG_RID;
                private charParameter INVENTORY_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter BEGIN_CDR_RID;
                private intParameter SHIP_TO_CDR_RID;
                private charParameter LINE_ITEM_MIN_OVERRIDE_IND;
                private intParameter LINE_ITEM_MIN_OVERRIDE;
                private charParameter HDRINVENTORY_IND;
                private intParameter HDRIB_MERCH_TYPE;
                private intParameter HDRIB_MERCH_HN_RID;
                private intParameter HDRIB_MERCH_PH_RID;
                private intParameter HDRIB_MERCH_PHL_SEQUENCE;

                public MID_METHOD_GROUP_ALLOCATION_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    STORE_GRADE_TIMEFRAME = new intParameter("@STORE_GRADE_TIMEFRAME", base.inputParameterList);
                    EXCEED_MAX_IND = new charParameter("@EXCEED_MAX_IND", base.inputParameterList);
                    PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
                    RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
                    PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
                    MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
                    MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
                    MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
                    ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
                    ON_HAND_PH_RID = new intParameter("@ON_HAND_PH_RID", base.inputParameterList);
                    ON_HAND_PHL_SEQUENCE = new intParameter("@ON_HAND_PHL_SEQUENCE", base.inputParameterList);
                    ON_HAND_FACTOR = new floatParameter("@ON_HAND_FACTOR", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    MERCH_UNSPECIFIED = new charParameter("@MERCH_UNSPECIFIED", base.inputParameterList);
                    ON_HAND_UNSPECIFIED = new charParameter("@ON_HAND_UNSPECIFIED", base.inputParameterList);
                    RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
                    RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
                    STORE_GRADES_SG_RID = new intParameter("@STORE_GRADES_SG_RID", base.inputParameterList);
                    INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
                    IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
                    IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
                    IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
                    IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                    BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
                    SHIP_TO_CDR_RID = new intParameter("@SHIP_TO_CDR_RID", base.inputParameterList);
                    LINE_ITEM_MIN_OVERRIDE_IND = new charParameter("@LINE_ITEM_MIN_OVERRIDE_IND", base.inputParameterList);
                    LINE_ITEM_MIN_OVERRIDE = new intParameter("@LINE_ITEM_MIN_OVERRIDE", base.inputParameterList);
                    HDRINVENTORY_IND = new charParameter("@HDRINVENTORY_IND", base.inputParameterList);
                    HDRIB_MERCH_TYPE = new intParameter("@HDRIB_MERCH_TYPE", base.inputParameterList);
                    HDRIB_MERCH_HN_RID = new intParameter("@HDRIB_MERCH_HN_RID", base.inputParameterList);
                    HDRIB_MERCH_PH_RID = new intParameter("@HDRIB_MERCH_PH_RID", base.inputParameterList);
                    HDRIB_MERCH_PHL_SEQUENCE = new intParameter("@HDRIB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? STORE_GRADE_TIMEFRAME,
                                  char? EXCEED_MAX_IND,
                                  double? PERCENT_NEED_LIMIT,
                                  double? RESERVE,
                                  char? PERCENT_IND,
                                  int? MERCH_HN_RID,
                                  int? MERCH_PH_RID,
                                  int? MERCH_PHL_SEQUENCE,
                                  int? ON_HAND_HN_RID,
                                  int? ON_HAND_PH_RID,
                                  int? ON_HAND_PHL_SEQUENCE,
                                  double? ON_HAND_FACTOR,
                                  int? SG_RID,
                                  char? MERCH_UNSPECIFIED,
                                  char? ON_HAND_UNSPECIFIED,
                                  double? RESERVE_AS_BULK,
                                  double? RESERVE_AS_PACKS,
                                  int? STORE_GRADES_SG_RID,
                                  char? INVENTORY_IND,
                                  int? IB_MERCH_TYPE,
                                  int? IB_MERCH_HN_RID,
                                  int? IB_MERCH_PH_RID,
                                  int? IB_MERCH_PHL_SEQUENCE,
                                  int? BEGIN_CDR_RID,
                                  int? SHIP_TO_CDR_RID,
                                  char? LINE_ITEM_MIN_OVERRIDE_IND,
                                  int? LINE_ITEM_MIN_OVERRIDE,
                                  char? HDRINVENTORY_IND,
                                  int? HDRIB_MERCH_TYPE,
                                  int? HDRIB_MERCH_HN_RID,
                                  int? HDRIB_MERCH_PH_RID,
                                  int? HDRIB_MERCH_PHL_SEQUENCE
                                  )
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_GRADE_TIMEFRAME.SetValue(STORE_GRADE_TIMEFRAME);
                        this.EXCEED_MAX_IND.SetValue(EXCEED_MAX_IND);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        this.ON_HAND_PH_RID.SetValue(ON_HAND_PH_RID);
                        this.ON_HAND_PHL_SEQUENCE.SetValue(ON_HAND_PHL_SEQUENCE);
                        this.ON_HAND_FACTOR.SetValue(ON_HAND_FACTOR);
                        this.SG_RID.SetValue(SG_RID);
                        this.MERCH_UNSPECIFIED.SetValue(MERCH_UNSPECIFIED);
                        this.ON_HAND_UNSPECIFIED.SetValue(ON_HAND_UNSPECIFIED);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        this.STORE_GRADES_SG_RID.SetValue(STORE_GRADES_SG_RID);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.SHIP_TO_CDR_RID.SetValue(SHIP_TO_CDR_RID);
                        this.LINE_ITEM_MIN_OVERRIDE_IND.SetValue(LINE_ITEM_MIN_OVERRIDE_IND);
                        this.LINE_ITEM_MIN_OVERRIDE.SetValue(LINE_ITEM_MIN_OVERRIDE);
                        this.HDRINVENTORY_IND.SetValue(HDRINVENTORY_IND);
                        this.HDRIB_MERCH_TYPE.SetValue(HDRIB_MERCH_TYPE);
                        this.HDRIB_MERCH_HN_RID.SetValue(HDRIB_MERCH_HN_RID);
                        this.HDRIB_MERCH_PH_RID.SetValue(HDRIB_MERCH_PH_RID);
                        this.HDRIB_MERCH_PHL_SEQUENCE.SetValue(HDRIB_MERCH_PHL_SEQUENCE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;
               

                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                    
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? BOUNDARY,
                                  string GRADE_CODE 
                                  )
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_GROUP;
                private intParameter MAXIMUM_GROUP;
                private intParameter MINIMUM_HEADER;
                private intParameter MAXIMUM_HEADER;
                private intParameter SGL_RID;


                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_GROUP = new intParameter("@MINIMUM_GROUP", base.inputParameterList);
                    MAXIMUM_GROUP = new intParameter("@MAXIMUM_GROUP", base.inputParameterList);
                    MINIMUM_HEADER = new intParameter("@MINIMUM_HEADER", base.inputParameterList);
                    MAXIMUM_HEADER = new intParameter("@MAXIMUM_HEADER", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? BOUNDARY,
                                  int? MINIMUM_GROUP,
                                  int? MAXIMUM_GROUP,
                                  int? MINIMUM_HEADER,
                                  int? MAXIMUM_HEADER,
                                  int? SGL_RID
                                  )
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_GROUP.SetValue(MINIMUM_GROUP);
                        this.MAXIMUM_GROUP.SetValue(MAXIMUM_GROUP);
                        this.MINIMUM_HEADER.SetValue(MINIMUM_HEADER);
                        this.MAXIMUM_HEADER.SetValue(MAXIMUM_HEADER);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_UPDATE_def MID_METHOD_GROUP_ALLOCATION_UPDATE = new MID_METHOD_GROUP_ALLOCATION_UPDATE_def();
            public class MID_METHOD_GROUP_ALLOCATION_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_UPDATE.SQL"

                private intParameter METHOD_RID;
                private intParameter STORE_GRADE_TIMEFRAME;
                private charParameter EXCEED_MAX_IND;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter RESERVE;
                private charParameter PERCENT_IND;
                private intParameter MERCH_HN_RID;
                private intParameter MERCH_PH_RID;
                private intParameter MERCH_PHL_SEQUENCE;
                private intParameter ON_HAND_HN_RID;
                private intParameter ON_HAND_PH_RID;
                private intParameter ON_HAND_PHL_SEQUENCE;
                private floatParameter ON_HAND_FACTOR;
                private intParameter SG_RID;
                private charParameter MERCH_UNSPECIFIED;
                private charParameter ON_HAND_UNSPECIFIED;
                private floatParameter RESERVE_AS_BULK;
                private floatParameter RESERVE_AS_PACKS;
                private intParameter STORE_GRADES_SG_RID;
                private charParameter INVENTORY_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter BEGIN_CDR_RID;
                private intParameter SHIP_TO_CDR_RID;
                private charParameter LINE_ITEM_MIN_OVERRIDE_IND;
                private intParameter LINE_ITEM_MIN_OVERRIDE;
                private charParameter HDRINVENTORY_IND;
                private intParameter HDRIB_MERCH_TYPE;
                private intParameter HDRIB_MERCH_HN_RID;
                private intParameter HDRIB_MERCH_PH_RID;
                private intParameter HDRIB_MERCH_PHL_SEQUENCE;

                public MID_METHOD_GROUP_ALLOCATION_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    STORE_GRADE_TIMEFRAME = new intParameter("@STORE_GRADE_TIMEFRAME", base.inputParameterList);
                    EXCEED_MAX_IND = new charParameter("@EXCEED_MAX_IND", base.inputParameterList);
                    PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
                    RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
                    PERCENT_IND = new charParameter("@PERCENT_IND", base.inputParameterList);
                    MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
                    MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
                    MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
                    ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
                    ON_HAND_PH_RID = new intParameter("@ON_HAND_PH_RID", base.inputParameterList);
                    ON_HAND_PHL_SEQUENCE = new intParameter("@ON_HAND_PHL_SEQUENCE", base.inputParameterList);
                    ON_HAND_FACTOR = new floatParameter("@ON_HAND_FACTOR", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    MERCH_UNSPECIFIED = new charParameter("@MERCH_UNSPECIFIED", base.inputParameterList);
                    ON_HAND_UNSPECIFIED = new charParameter("@ON_HAND_UNSPECIFIED", base.inputParameterList);
                    RESERVE_AS_BULK = new floatParameter("@RESERVE_AS_BULK", base.inputParameterList);
                    RESERVE_AS_PACKS = new floatParameter("@RESERVE_AS_PACKS", base.inputParameterList);
                    STORE_GRADES_SG_RID = new intParameter("@STORE_GRADES_SG_RID", base.inputParameterList);
                    INVENTORY_IND = new charParameter("@INVENTORY_IND", base.inputParameterList);
                    IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
                    IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
                    IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
                    IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                    BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
                    SHIP_TO_CDR_RID = new intParameter("@SHIP_TO_CDR_RID", base.inputParameterList);
                    LINE_ITEM_MIN_OVERRIDE_IND = new charParameter("@LINE_ITEM_MIN_OVERRIDE_IND", base.inputParameterList);
                    LINE_ITEM_MIN_OVERRIDE = new intParameter("@LINE_ITEM_MIN_OVERRIDE", base.inputParameterList);
                    HDRINVENTORY_IND = new charParameter("@HDRINVENTORY_IND", base.inputParameterList);
                    HDRIB_MERCH_TYPE = new intParameter("@HDRIB_MERCH_TYPE", base.inputParameterList);
                    HDRIB_MERCH_HN_RID = new intParameter("@HDRIB_MERCH_HN_RID", base.inputParameterList);
                    HDRIB_MERCH_PH_RID = new intParameter("@HDRIB_MERCH_PH_RID", base.inputParameterList);
                    HDRIB_MERCH_PHL_SEQUENCE = new intParameter("@HDRIB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? STORE_GRADE_TIMEFRAME,
                                  char? EXCEED_MAX_IND,
                                  double? PERCENT_NEED_LIMIT,
                                  double? RESERVE,
                                  char? PERCENT_IND,
                                  int? MERCH_HN_RID,
                                  int? MERCH_PH_RID,
                                  int? MERCH_PHL_SEQUENCE,
                                  int? ON_HAND_HN_RID,
                                  int? ON_HAND_PH_RID,
                                  int? ON_HAND_PHL_SEQUENCE,
                                  double? ON_HAND_FACTOR,
                                  int? SG_RID,
                                  char? MERCH_UNSPECIFIED,
                                  char? ON_HAND_UNSPECIFIED,
                                  double? RESERVE_AS_BULK,
                                  double? RESERVE_AS_PACKS,
                                  int? STORE_GRADES_SG_RID,
                                  char? INVENTORY_IND,
                                  int? IB_MERCH_TYPE,
                                  int? IB_MERCH_HN_RID,
                                  int? IB_MERCH_PH_RID,
                                  int? IB_MERCH_PHL_SEQUENCE,
                                  int? BEGIN_CDR_RID,
                                  int? SHIP_TO_CDR_RID,
                                  char? LINE_ITEM_MIN_OVERRIDE_IND,
                                  int? LINE_ITEM_MIN_OVERRIDE,
                                  char? HDRINVENTORY_IND,
                                  int? HDRIB_MERCH_TYPE,
                                  int? HDRIB_MERCH_HN_RID,
                                  int? HDRIB_MERCH_PH_RID,
                                  int? HDRIB_MERCH_PHL_SEQUENCE
                                  )
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.STORE_GRADE_TIMEFRAME.SetValue(STORE_GRADE_TIMEFRAME);
                        this.EXCEED_MAX_IND.SetValue(EXCEED_MAX_IND);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.RESERVE.SetValue(RESERVE);
                        this.PERCENT_IND.SetValue(PERCENT_IND);
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        this.ON_HAND_PH_RID.SetValue(ON_HAND_PH_RID);
                        this.ON_HAND_PHL_SEQUENCE.SetValue(ON_HAND_PHL_SEQUENCE);
                        this.ON_HAND_FACTOR.SetValue(ON_HAND_FACTOR);
                        this.SG_RID.SetValue(SG_RID);
                        this.MERCH_UNSPECIFIED.SetValue(MERCH_UNSPECIFIED);
                        this.ON_HAND_UNSPECIFIED.SetValue(ON_HAND_UNSPECIFIED);
                        this.RESERVE_AS_BULK.SetValue(RESERVE_AS_BULK);
                        this.RESERVE_AS_PACKS.SetValue(RESERVE_AS_PACKS);
                        this.STORE_GRADES_SG_RID.SetValue(STORE_GRADES_SG_RID);
                        this.INVENTORY_IND.SetValue(INVENTORY_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.SHIP_TO_CDR_RID.SetValue(SHIP_TO_CDR_RID);
                        this.LINE_ITEM_MIN_OVERRIDE_IND.SetValue(LINE_ITEM_MIN_OVERRIDE_IND);
                        this.LINE_ITEM_MIN_OVERRIDE.SetValue(LINE_ITEM_MIN_OVERRIDE);
                        this.HDRINVENTORY_IND.SetValue(HDRINVENTORY_IND);
                        this.HDRIB_MERCH_TYPE.SetValue(HDRIB_MERCH_TYPE);
                        this.HDRIB_MERCH_HN_RID.SetValue(HDRIB_MERCH_HN_RID);
                        this.HDRIB_MERCH_PH_RID.SetValue(HDRIB_MERCH_PH_RID);
                        this.HDRIB_MERCH_PHL_SEQUENCE.SetValue(HDRIB_MERCH_PHL_SEQUENCE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }


            public static MID_METHOD_GROUP_ALLOCATION_DELETE_def MID_METHOD_GROUP_ALLOCATION_DELETE = new MID_METHOD_GROUP_ALLOCATION_DELETE_def();
            public class MID_METHOD_GROUP_ALLOCATION_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE_def MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE = new MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE_def();
            public class MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE_def MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE = new MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE_def();
            public class MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE.SQL"

                private intParameter MERCH_HN_RID;

                public MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    MERCH_HN_RID = new intParameter("@MERCH_HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? MERCH_HN_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_READ_BY_NODE_def))
                    {
                        this.MERCH_HN_RID.SetValue(MERCH_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE_def MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE = new MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE_def();
            public class MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE.SQL"

                private intParameter ON_HAND_HN_RID;

                public MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE_def()
                {
                    base.procedureName = "MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_GROUP_ALLOCATION");
                    ON_HAND_HN_RID = new intParameter("@ON_HAND_HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? ON_HAND_HN_RID)
                {
                    lock (typeof(MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE_def))
                    {
                        this.ON_HAND_HN_RID.SetValue(ON_HAND_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge

            // Begin TT#1652-MD - RMatelic - DC arton Rounding
            public static MID_METHOD_DC_CARTON_ROUNDING_DELETE_def MID_METHOD_DC_CARTON_ROUNDING_DELETE = new MID_METHOD_DC_CARTON_ROUNDING_DELETE_def();
            public class MID_METHOD_DC_CARTON_ROUNDING_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_CARTON_ROUNDING_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_DC_CARTON_ROUNDING_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_DC_CARTON_ROUNDING_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_DC_CARTON_ROUNDING");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_DC_CARTON_ROUNDING_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_CARTON_ROUNDING_INSERT_def MID_METHOD_DC_CARTON_ROUNDING_INSERT = new MID_METHOD_DC_CARTON_ROUNDING_INSERT_def();
            public class MID_METHOD_DC_CARTON_ROUNDING_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_CARTON_ROUNDING_INSERT.SQL"

                private intParameter METHOD_RID;
                private charParameter APPLY_OVERAGE_TO;
               
                public MID_METHOD_DC_CARTON_ROUNDING_INSERT_def()
                {
                    base.procedureName = "MID_METHOD_DC_CARTON_ROUNDING_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_DC_CARTON_ROUNDING");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    APPLY_OVERAGE_TO = new charParameter("@APPLY_OVERAGE_TO", base.inputParameterList);
               }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  char APPLY_OVERAGE_TO
                                  )
                {
                    lock (typeof(MID_METHOD_DC_CARTON_ROUNDING_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.APPLY_OVERAGE_TO.SetValue(APPLY_OVERAGE_TO);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_CARTON_ROUNDING_READ_def MID_METHOD_DC_CARTON_ROUNDING_READ = new MID_METHOD_DC_CARTON_ROUNDING_READ_def();
            public class MID_METHOD_DC_CARTON_ROUNDING_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_CARTON_ROUNDING_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_DC_CARTON_ROUNDING_READ_def()
                {
                    base.procedureName = "MID_METHOD_DC_CARTON_ROUNDING_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_DC_CARTON_ROUNDING");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_DC_CARTON_ROUNDING_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_CARTON_ROUNDING_UPDATE_def MID_METHOD_DC_CARTON_ROUNDING_UPDATE = new MID_METHOD_DC_CARTON_ROUNDING_UPDATE_def();
            public class MID_METHOD_DC_CARTON_ROUNDING_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_CARTON_ROUNDING_UPDATE.SQL"

                private intParameter METHOD_RID;
                private charParameter APPLY_OVERAGE_TO;

                public MID_METHOD_DC_CARTON_ROUNDING_UPDATE_def()
                {
                    base.procedureName = "MID_METHOD_DC_CARTON_ROUNDING_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_DC_CARTON_ROUNDING");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    APPLY_OVERAGE_TO = new charParameter("@APPLY_OVERAGE_TO", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  char? APPLY_OVERAGE_TO
                                 )
                {
                    lock (typeof(MID_METHOD_DC_CARTON_ROUNDING_UPDATE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.APPLY_OVERAGE_TO.SetValue(APPLY_OVERAGE_TO);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }
            // End TT#1652-MD

            // Begin TT#1966-MD - JSmith- DC Fulfillment
            public static MID_METHOD_CREATE_MASTER_HEADERS_DELETE_def MID_METHOD_CREATE_MASTER_HEADERS_DELETE = new MID_METHOD_CREATE_MASTER_HEADERS_DELETE_def();
            public class MID_METHOD_CREATE_MASTER_HEADERS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_CREATE_MASTER_HEADERS_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_CREATE_MASTER_HEADERS_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_CREATE_MASTER_HEADERS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_CREATE_MASTER_HEADERS");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_CREATE_MASTER_HEADERS_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_CREATE_MASTER_HEADERS_UPSERT_def MID_METHOD_CREATE_MASTER_HEADERS_UPSERT = new MID_METHOD_CREATE_MASTER_HEADERS_UPSERT_def();
            public class MID_METHOD_CREATE_MASTER_HEADERS_UPSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_CREATE_MASTER_HEADERS_UPSERT.SQL"

                private intParameter METHOD_RID;
                private charParameter USE_SELECTED_HEADERS;
                private tableParameter MERCHANDISE_TABLE;
                private tableParameter OVERRIDE_TABLE;

                public MID_METHOD_CREATE_MASTER_HEADERS_UPSERT_def()
                {
                    base.procedureName = "MID_METHOD_CREATE_MASTER_HEADERS_UPSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_CREATE_MASTER_HEADERS");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    USE_SELECTED_HEADERS = new charParameter("@USE_SELECTED_HEADERS", base.inputParameterList);
                    MERCHANDISE_TABLE = new tableParameter("@MERCHANDISE", "CREATE_MASTER_HEADERS_MERCHANDISE_TYPE", base.inputParameterList);
                    OVERRIDE_TABLE = new tableParameter("@OVERRIDE", "CREATE_MASTER_HEADERS_OVERRIDE_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  char USE_SELECTED_HEADERS,
                                  DataTable MERCHANDISE_TABLE,
                                  DataTable OVERRIDE_TABLE
                                  )
                {
                    lock (typeof(MID_METHOD_CREATE_MASTER_HEADERS_UPSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.USE_SELECTED_HEADERS.SetValue(USE_SELECTED_HEADERS);
                        this.MERCHANDISE_TABLE.SetValue(MERCHANDISE_TABLE);
                        this.OVERRIDE_TABLE.SetValue(OVERRIDE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_CREATE_MASTER_HEADERS_READ_def MID_METHOD_CREATE_MASTER_HEADERS_READ = new MID_METHOD_CREATE_MASTER_HEADERS_READ_def();
            public class MID_METHOD_CREATE_MASTER_HEADERS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_CREATE_MASTER_HEADERS_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_CREATE_MASTER_HEADERS_READ_def()
                {
                    base.procedureName = "MID_METHOD_CREATE_MASTER_HEADERS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_CREATE_MASTER_HEADERS");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_CREATE_MASTER_HEADERS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_FULFILLMENT_DELETE_def MID_METHOD_DC_FULFILLMENT_DELETE = new MID_METHOD_DC_FULFILLMENT_DELETE_def();
            public class MID_METHOD_DC_FULFILLMENT_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_FULFILLMENT_DELETE.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_DC_FULFILLMENT_DELETE_def()
                {
                    base.procedureName = "MID_METHOD_DC_FULFILLMENT_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("METHOD_DC_FULFILLMENT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_DC_FULFILLMENT_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_FULFILLMENT_UPSERT_def MID_METHOD_DC_FULFILLMENT_UPSERT = new MID_METHOD_DC_FULFILLMENT_UPSERT_def();
            public class MID_METHOD_DC_FULFILLMENT_UPSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_FULFILLMENT_UPSERT.SQL"

                private intParameter METHOD_RID;
                private intParameter SPLIT_OPTION;
                private charParameter APPLY_MINIMUMS_IND;
                private charParameter PRIORITIZE_TYPE;
                private intParameter HEADER_FIELD;
                private intParameter HCG_RID;
                private intParameter HEADERS_ORDER;
                private intParameter STORES_ORDER;
                private intParameter FIELD_DATA_TYPE;
                private intParameter SPLIT_BY_OPTION;          
                private intParameter SPLIT_BY_RESERVE;         
                private intParameter APPLY_BY;
                private intParameter WITHIN_DC;
                private tableParameter STORE_ORDER_TABLE;

                public MID_METHOD_DC_FULFILLMENT_UPSERT_def()
                {
                    base.procedureName = "MID_METHOD_DC_FULFILLMENT_UPSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("METHOD_DC_FULFILLMENT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    SPLIT_OPTION = new intParameter("@SPLIT_OPTION", base.inputParameterList);
                    APPLY_MINIMUMS_IND = new charParameter("@APPLY_MINIMUMS_IND", base.inputParameterList);
                    PRIORITIZE_TYPE = new charParameter("@PRIORITIZE_TYPE", base.inputParameterList);
                    HEADER_FIELD = new intParameter("@HEADER_FIELD", base.inputParameterList);
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HEADERS_ORDER = new intParameter("@HEADERS_ORDER", base.inputParameterList);
                    STORES_ORDER = new intParameter("@STORES_ORDER", base.inputParameterList);
                    FIELD_DATA_TYPE = new intParameter("@FIELD_DATA_TYPE", base.inputParameterList);
                    SPLIT_BY_OPTION = new intParameter("@SPLIT_BY_OPTION", base.inputParameterList);    
                    SPLIT_BY_RESERVE = new intParameter("@SPLIT_BY_RESERVE", base.inputParameterList);        
                    APPLY_BY = new intParameter("@APPLY_BY", base.inputParameterList);
                    WITHIN_DC = new intParameter("@WITHIN_DC", base.inputParameterList);
                    STORE_ORDER_TABLE = new tableParameter("@STORE_ORDER", "DC_FULFILLMENT_STORE_ORDER_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? METHOD_RID,
                                  int? SPLIT_OPTION,
                                  char APPLY_MINIMUMS_IND,
                                  char PRIORITIZE_TYPE,
                                  int? HEADER_FIELD,
                                  int? HCG_RID,
                                  int? HEADERS_ORDER,
                                  int? STORES_ORDER,
                                  int? FIELD_DATA_TYPE,      
                                  int? SPLIT_BY_OPTION,   
                                  int? SPLIT_BY_RESERVE,  
                                  int? APPLY_BY,
                                  int? WITHIN_DC,
                                  DataTable STORE_ORDER_TABLE
                                  )
                {
                    lock (typeof(MID_METHOD_DC_FULFILLMENT_UPSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SPLIT_OPTION.SetValue(SPLIT_OPTION);
                        this.APPLY_MINIMUMS_IND.SetValue(APPLY_MINIMUMS_IND);
                        this.PRIORITIZE_TYPE.SetValue(PRIORITIZE_TYPE);
                        this.HEADER_FIELD.SetValue(HEADER_FIELD);
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HEADERS_ORDER.SetValue(HEADERS_ORDER);
                        this.STORES_ORDER.SetValue(STORES_ORDER);
                        this.FIELD_DATA_TYPE.SetValue(FIELD_DATA_TYPE);
                        this.SPLIT_BY_OPTION.SetValue(SPLIT_BY_OPTION);       
                        this.SPLIT_BY_RESERVE.SetValue(SPLIT_BY_RESERVE);  
                        this.APPLY_BY.SetValue(APPLY_BY); 
                        this.WITHIN_DC.SetValue(WITHIN_DC);
                        this.STORE_ORDER_TABLE.SetValue(STORE_ORDER_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_METHOD_DC_FULFILLMENT_READ_def MID_METHOD_DC_FULFILLMENT_READ = new MID_METHOD_DC_FULFILLMENT_READ_def();
            public class MID_METHOD_DC_FULFILLMENT_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_DC_FULFILLMENT_READ.SQL"

                private intParameter METHOD_RID;

                public MID_METHOD_DC_FULFILLMENT_READ_def()
                {
                    base.procedureName = "MID_METHOD_DC_FULFILLMENT_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_DC_FULFILLMENT");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_METHOD_DC_FULFILLMENT_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }
            // End TT#1966-MD - JSmith- DC Fulfillment

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
