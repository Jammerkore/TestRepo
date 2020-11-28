//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;

//namespace MIDRetail.Data
//{
//    public partial class ProductFilterData : DataLayer
//    {
//        protected static class StoredProcedures
//        {

//            public static MID_PRODUCT_SEARCH_OBJECT_READ_def MID_PRODUCT_SEARCH_OBJECT_READ = new MID_PRODUCT_SEARCH_OBJECT_READ_def();
//            public class MID_PRODUCT_SEARCH_OBJECT_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_OBJECT_READ.SQL"

//                public intParameter USER_RID;
			
//                public MID_PRODUCT_SEARCH_OBJECT_READ_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_OBJECT_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("PRODUCT_SEARCH_OBJECT");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS_def MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS = new MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS_def();
//            public class MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS.SQL"

			
//                public MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_OBJECT_READ_DISTINCT_USERS";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("PRODUCT_SEARCH_OBJECT");
//                }
			
//                public DataTable Read(DatabaseAccess _dba)
//                {
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_PRODUCT_SEARCH_OBJECT_INSERT_def MID_PRODUCT_SEARCH_OBJECT_INSERT = new MID_PRODUCT_SEARCH_OBJECT_INSERT_def();
//            public class MID_PRODUCT_SEARCH_OBJECT_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_OBJECT_INSERT.SQL"

//                public intParameter USER_RID;
//                public intParameter PRODUCT_SEARCH_OBJECT_TYPE;
//                public byteArrayParameter PRODUCT_SEARCH_OBJECT;
			
//                public MID_PRODUCT_SEARCH_OBJECT_INSERT_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_OBJECT_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("PRODUCT_SEARCH_OBJECT");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    PRODUCT_SEARCH_OBJECT_TYPE = new intParameter("@PRODUCT_SEARCH_OBJECT_TYPE", base.inputParameterList);
//                    PRODUCT_SEARCH_OBJECT = new byteArrayParameter("@PRODUCT_SEARCH_OBJECT", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, 
//                                  int? USER_RID,
//                                  int? PRODUCT_SEARCH_OBJECT_TYPE,
//                                  byte[] PRODUCT_SEARCH_OBJECT
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.PRODUCT_SEARCH_OBJECT_TYPE.SetValue(PRODUCT_SEARCH_OBJECT_TYPE);
//                    this.PRODUCT_SEARCH_OBJECT.SetValue(PRODUCT_SEARCH_OBJECT);
//                    return ExecuteStoredProcedureForInsert(_dba);
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

//            public static MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT_def MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT = new MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT_def();
//            public class MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT.SQL"

//                public intParameter FILTER_RID;
			
//                public MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT_def()
//                {
//                    base.procedureName = "MID_IN_USE_FILTER_XREF_DELETE_FOR_PRODUCT";
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

//            public static MID_PRODUCT_SEARCH_OBJECT_DELETE_def MID_PRODUCT_SEARCH_OBJECT_DELETE = new MID_PRODUCT_SEARCH_OBJECT_DELETE_def();
//            public class MID_PRODUCT_SEARCH_OBJECT_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_OBJECT_DELETE.SQL"

//                public intParameter USER_RID;
			
//                public MID_PRODUCT_SEARCH_OBJECT_DELETE_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_OBJECT_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("PRODUCT_SEARCH_OBJECT");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }
			
//                public int Delete(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_PRODUCT_SEARCH_VIEW_READ_def MID_PRODUCT_SEARCH_VIEW_READ = new MID_PRODUCT_SEARCH_VIEW_READ_def();
//            public class MID_PRODUCT_SEARCH_VIEW_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_VIEW_READ.SQL"

//                public intParameter USER_RID;
			
//                public MID_PRODUCT_SEARCH_VIEW_READ_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_VIEW_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("PRODUCT_SEARCH_VIEW");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }
			
//                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_PRODUCT_SEARCH_VIEW_INSERT_def MID_PRODUCT_SEARCH_VIEW_INSERT = new MID_PRODUCT_SEARCH_VIEW_INSERT_def();
//            public class MID_PRODUCT_SEARCH_VIEW_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_VIEW_INSERT.SQL"

//                public intParameter USER_RID;
//                public intParameter VIEW_SEQUENCE;
//                public intParameter DATA_TYPE;
//                public intParameter DATA_KEY;
//                public intParameter COLUMN_WIDTH;
			
//                public MID_PRODUCT_SEARCH_VIEW_INSERT_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_VIEW_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("PRODUCT_SEARCH_VIEW");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    VIEW_SEQUENCE = new intParameter("@VIEW_SEQUENCE", base.inputParameterList);
//                    DATA_TYPE = new intParameter("@DATA_TYPE", base.inputParameterList);
//                    DATA_KEY = new intParameter("@DATA_KEY", base.inputParameterList);
//                    COLUMN_WIDTH = new intParameter("@COLUMN_WIDTH", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, 
//                                  int? USER_RID,
//                                  int? VIEW_SEQUENCE,
//                                  int? DATA_TYPE,
//                                  int? DATA_KEY,
//                                  int? COLUMN_WIDTH
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.VIEW_SEQUENCE.SetValue(VIEW_SEQUENCE);
//                    this.DATA_TYPE.SetValue(DATA_TYPE);
//                    this.DATA_KEY.SetValue(DATA_KEY);
//                    this.COLUMN_WIDTH.SetValue(COLUMN_WIDTH);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_PRODUCT_SEARCH_VIEW_DELETE_def MID_PRODUCT_SEARCH_VIEW_DELETE = new MID_PRODUCT_SEARCH_VIEW_DELETE_def();
//            public class MID_PRODUCT_SEARCH_VIEW_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_SEARCH_VIEW_DELETE.SQL"

//                public intParameter USER_RID;
			
//                public MID_PRODUCT_SEARCH_VIEW_DELETE_def()
//                {
//                    base.procedureName = "MID_PRODUCT_SEARCH_VIEW_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("PRODUCT_SEARCH_VIEW");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }
			
//                public int Delete(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            //INSERT NEW STORED PROCEDURES ABOVE HERE
//        }
//    }  
//}
