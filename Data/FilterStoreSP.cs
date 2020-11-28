//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;

//namespace MIDRetail.Data
//{
//    public partial class StoreFilterData : DataLayer
//    {
//        protected static class StoredProcedures
//        {

//            public static MID_STORE_FILTER_READ_def MID_STORE_FILTER_READ = new MID_STORE_FILTER_READ_def();
//            public class MID_STORE_FILTER_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ.SQL"

//                public intParameter STORE_FILTER_RID;
			
//                public MID_STORE_FILTER_READ_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, int? STORE_FILTER_RID)
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_READ_FROM_USERS_def MID_STORE_FILTER_READ_FROM_USERS = new MID_STORE_FILTER_READ_FROM_USERS_def();
//            public class MID_STORE_FILTER_READ_FROM_USERS_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ_FROM_USERS.SQL"

//                public tableParameter USER_RID_LIST;
			
//                public MID_STORE_FILTER_READ_FROM_USERS_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ_FROM_USERS";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
//                }

//                public DataTable Read(DatabaseAccess _dba, DataTable USER_RID_LIST)
//                {
//                    this.USER_RID_LIST.SetValue(USER_RID_LIST);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_READ_PARENT_FROM_USERS_def MID_STORE_FILTER_READ_PARENT_FROM_USERS = new MID_STORE_FILTER_READ_PARENT_FROM_USERS_def();
//            public class MID_STORE_FILTER_READ_PARENT_FROM_USERS_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ_PARENT_FROM_USERS.SQL"

//                public tableParameter USER_RID_LIST;
			
//                public MID_STORE_FILTER_READ_PARENT_FROM_USERS_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ_PARENT_FROM_USERS";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE",  base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, DataTable USER_RID_LIST)
//                {
//                    this.USER_RID_LIST.SetValue(USER_RID_LIST);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_READ_ALL_def MID_STORE_FILTER_READ_ALL = new MID_STORE_FILTER_READ_ALL_def();
//            public class MID_STORE_FILTER_READ_ALL_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ_ALL.SQL"

			
//                public MID_STORE_FILTER_READ_ALL_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ_ALL";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                }
			
//                public DataTable Read(DatabaseAccess _dba)
//                {
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_UPDATE_def MID_STORE_FILTER_UPDATE = new MID_STORE_FILTER_UPDATE_def();
//            public class MID_STORE_FILTER_UPDATE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_UPDATE.SQL"

//                public intParameter STORE_FILTER_RID;
//                public intParameter USER_RID;
//                public stringParameter STORE_FILTER_NAME;
			
//                public MID_STORE_FILTER_UPDATE_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_UPDATE";
//                    base.procedureType = storedProcedureTypes.Update;
//                    base.tableNames.Add("STORE_FILTER");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    STORE_FILTER_NAME = new stringParameter("@STORE_FILTER_NAME", base.inputParameterList);
//                }
			
//                public int Update(DatabaseAccess _dba, 
//                                  int? STORE_FILTER_RID,
//                                  int? USER_RID,
//                                  string STORE_FILTER_NAME
//                                  )
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    this.USER_RID.SetValue(USER_RID);
//                    this.STORE_FILTER_NAME.SetValue(STORE_FILTER_NAME);
//                    return ExecuteStoredProcedureForUpdate(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_DELETE_def MID_STORE_FILTER_DELETE = new MID_STORE_FILTER_DELETE_def();
//            public class MID_STORE_FILTER_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_DELETE.SQL"

//                public intParameter STORE_FILTER_RID;
			
//                public MID_STORE_FILTER_DELETE_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("STORE_FILTER");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                }
			
//                public int Delete(DatabaseAccess _dba, int? STORE_FILTER_RID)
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static SP_MID_STORE_FILTER_INSERT_def SP_MID_STORE_FILTER_INSERT = new SP_MID_STORE_FILTER_INSERT_def();
//            public class SP_MID_STORE_FILTER_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_FILTER_INSERT.SQL"

//                public intParameter USER_RID;
//                public stringParameter STORE_FILTER_NAME;
//                public intParameter STORE_FILTER_RID; //Declare Output Parameter

//                public SP_MID_STORE_FILTER_INSERT_def()
//                {
//                    base.procedureName = "SP_MID_STORE_FILTER_INSERT";
//                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
//                    base.tableNames.Add("STORE_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    STORE_FILTER_NAME = new stringParameter("@STORE_FILTER_NAME", base.inputParameterList);
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.outputParameterList); //Add Output Parameter
//                }
			
//                public int InsertAndReturnRID(DatabaseAccess _dba, 
//                                              int? USER_RID,
//                                              string STORE_FILTER_NAME
//                                              )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.STORE_FILTER_NAME.SetValue(STORE_FILTER_NAME);
//                    this.STORE_FILTER_RID.SetValue(null); //Initialize Output Parameter
//                    return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_READ_FROM_USER_AND_NAME_def MID_STORE_FILTER_READ_FROM_USER_AND_NAME = new MID_STORE_FILTER_READ_FROM_USER_AND_NAME_def();
//            public class MID_STORE_FILTER_READ_FROM_USER_AND_NAME_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ_FROM_USER_AND_NAME.SQL"

//                public intParameter USER_RID;
//                public stringParameter STORE_FILTER_NAME;
			
//                public MID_STORE_FILTER_READ_FROM_USER_AND_NAME_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ_FROM_USER_AND_NAME";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    STORE_FILTER_NAME = new stringParameter("@STORE_FILTER_NAME", base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, 
//                                      int? USER_RID,
//                                      string STORE_FILTER_NAME
//                                      )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.STORE_FILTER_NAME.SetValue(STORE_FILTER_NAME);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_OBJECT_READ_def MID_STORE_FILTER_OBJECT_READ = new MID_STORE_FILTER_OBJECT_READ_def();
//            public class MID_STORE_FILTER_OBJECT_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_OBJECT_READ.SQL"

//                public intParameter STORE_FILTER_RID;
			
//                public MID_STORE_FILTER_OBJECT_READ_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_OBJECT_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER_OBJECT");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, int? STORE_FILTER_RID)
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_OBJECT_INSERT_def MID_STORE_FILTER_OBJECT_INSERT = new MID_STORE_FILTER_OBJECT_INSERT_def();
//            public class MID_STORE_FILTER_OBJECT_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_OBJECT_INSERT.SQL"

//                public intParameter STORE_FILTER_RID;
//                public intParameter STORE_FILTER_OBJECT_TYPE;
//                public byteArrayParameter STORE_FILTER_OBJECT;
			
//                public MID_STORE_FILTER_OBJECT_INSERT_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_OBJECT_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("STORE_FILTER_OBJECT");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                    STORE_FILTER_OBJECT_TYPE = new intParameter("@STORE_FILTER_OBJECT_TYPE", base.inputParameterList);
//                    STORE_FILTER_OBJECT = new byteArrayParameter("@STORE_FILTER_OBJECT", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, 
//                                  int? STORE_FILTER_RID,
//                                  int? STORE_FILTER_OBJECT_TYPE,
//                                  byte[] STORE_FILTER_OBJECT
//                                  )
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    this.STORE_FILTER_OBJECT_TYPE.SetValue(STORE_FILTER_OBJECT_TYPE);
//                    this.STORE_FILTER_OBJECT.SetValue(STORE_FILTER_OBJECT);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_OBJECT_DELETE_def MID_STORE_FILTER_OBJECT_DELETE = new MID_STORE_FILTER_OBJECT_DELETE_def();
//            public class MID_STORE_FILTER_OBJECT_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_OBJECT_DELETE.SQL"

//                public intParameter STORE_FILTER_RID;
			
//                public MID_STORE_FILTER_OBJECT_DELETE_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_OBJECT_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("STORE_FILTER_OBJECT");
//                    STORE_FILTER_RID = new intParameter("@STORE_FILTER_RID", base.inputParameterList);
//                }
			
//                public int Delete(DatabaseAccess _dba, int? STORE_FILTER_RID)
//                {
//                    this.STORE_FILTER_RID.SetValue(STORE_FILTER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_IN_USE_FILTER_XREF_INSERT_def MID_IN_USE_FILTER_XREF_INSERT = new MID_IN_USE_FILTER_XREF_INSERT_def();
//            public class MID_IN_USE_FILTER_XREF_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_IN_USE_FILTER_XREF_INSERT.SQL"

//                public intParameter FILTER_RID;
//                public intParameter FILTER_XREF_TYPE;
//                public intParameter PROFILE_TYPE;
//                public intParameter PROFILE_TYPE_RID;

//                public MID_IN_USE_FILTER_XREF_INSERT_def()
//                {
//                    base.procedureName = "MID_IN_USE_FILTER_XREF_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("IN_USE_FILTER_XREF");
//                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
//                    FILTER_XREF_TYPE = new intParameter("@FILTER_XREF_TYPE", base.inputParameterList);
//                    PROFILE_TYPE = new intParameter("@PROFILE_TYPE", base.inputParameterList);
//                    PROFILE_TYPE_RID = new intParameter("@PROFILE_TYPE_RID", base.inputParameterList);
//                }

//                public int Insert(DatabaseAccess _dba,
//                                  int? FILTER_RID,
//                                  int? FILTER_XREF_TYPE,
//                                  int? PROFILE_TYPE,
//                                  int? PROFILE_TYPE_RID
//                                  )
//                {
//                    this.FILTER_RID.SetValue(FILTER_RID);
//                    this.FILTER_XREF_TYPE.SetValue(FILTER_XREF_TYPE);
//                    this.PROFILE_TYPE.SetValue(PROFILE_TYPE);
//                    this.PROFILE_TYPE_RID.SetValue(PROFILE_TYPE_RID);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE_def MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE = new MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE_def();
//            public class MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE.SQL"

//                public intParameter FILTER_RID;
			
//                public MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE_def()
//                {
//                    base.procedureName = "MID_IN_USE_FILTER_XREF_DELETE_FOR_STORE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("IN_USE_FILTER_XREF");
//                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
//                }
			
//                public int Delete(DatabaseAccess _dba, int? FILTER_RID)
//                {
//                    this.FILTER_RID.SetValue(FILTER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_STORE_FILTER_READ_DEFAULT_def MID_STORE_FILTER_READ_DEFAULT = new MID_STORE_FILTER_READ_DEFAULT_def();
//            public class MID_STORE_FILTER_READ_DEFAULT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FILTER_READ_DEFAULT.SQL"

			
//                public MID_STORE_FILTER_READ_DEFAULT_def()
//                {
//                    base.procedureName = "MID_STORE_FILTER_READ_DEFAULT";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("STORE_FILTER");
//                }
			
//                public DataTable Read(DatabaseAccess _dba)
//                {
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            //INSERT NEW STORED PROCEDURES ABOVE HERE
//        }
//    }  
//}
