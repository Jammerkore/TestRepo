//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;

//namespace MIDRetail.Data
//{
//    public partial class AllocationWorkspaceFilterData : DataLayer
//    {
//        static class StoredProcedures
//        {
//            public static MID_AL_WRKSP_FILTER_READ_def MID_AL_WRKSP_FILTER_READ = new MID_AL_WRKSP_FILTER_READ_def(); 
//            public class MID_AL_WRKSP_FILTER_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_READ.SQL"

//                [UnitTestParameterAttribute(DB = "Dots", DefaultValue = "110")]
//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_READ_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_INSERT_def MID_AL_WRKSP_FILTER_INSERT = new MID_AL_WRKSP_FILTER_INSERT_def();
//            public class MID_AL_WRKSP_FILTER_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_INSERT.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter USER_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HN_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_TYPE;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_BETWEEN_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_BETWEEN_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter HEADER_DATE_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter HEADER_DATE_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_TYPE;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_BETWEEN_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_BETWEEN_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter RELEASE_DATE_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter RELEASE_DATE_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter MAXIMUM_HEADERS;

//                public MID_AL_WRKSP_FILTER_INSERT_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
//                    HEADER_DATE_TYPE = new intParameter("@HEADER_DATE_TYPE", base.inputParameterList);
//                    HEADER_DATE_BETWEEN_FROM = new intParameter("@HEADER_DATE_BETWEEN_FROM", base.inputParameterList);
//                    HEADER_DATE_BETWEEN_TO = new intParameter("@HEADER_DATE_BETWEEN_TO", base.inputParameterList);
//                    HEADER_DATE_FROM = new datetimeParameter("@HEADER_DATE_FROM", base.inputParameterList);
//                    HEADER_DATE_TO = new datetimeParameter("@HEADER_DATE_TO", base.inputParameterList);
//                    RELEASE_DATE_TYPE = new intParameter("@RELEASE_DATE_TYPE", base.inputParameterList);
//                    RELEASE_DATE_BETWEEN_FROM = new intParameter("@RELEASE_DATE_BETWEEN_FROM", base.inputParameterList);
//                    RELEASE_DATE_BETWEEN_TO = new intParameter("@RELEASE_DATE_BETWEEN_TO", base.inputParameterList);
//                    RELEASE_DATE_FROM = new datetimeParameter("@RELEASE_DATE_FROM", base.inputParameterList);
//                    RELEASE_DATE_TO = new datetimeParameter("@RELEASE_DATE_TO", base.inputParameterList);
//                    MAXIMUM_HEADERS = new intParameter("@MAXIMUM_HEADERS", base.inputParameterList);
//                }

//                public int Insert(DatabaseAccess _dba, 
//                                 int? USER_RID,
//                                 int? HN_RID,
//                                 int? HEADER_DATE_TYPE,
//                                 int? HEADER_DATE_BETWEEN_FROM,
//                                 int? HEADER_DATE_BETWEEN_TO,
//                                 DateTime? HEADER_DATE_FROM,
//                                 DateTime? HEADER_DATE_TO,
//                                 int? RELEASE_DATE_TYPE,
//                                 int? RELEASE_DATE_BETWEEN_FROM,
//                                 int? RELEASE_DATE_BETWEEN_TO,
//                                 DateTime? RELEASE_DATE_FROM,
//                                 DateTime? RELEASE_DATE_TO,
//                                 int? MAXIMUM_HEADERS
//                                 )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.HN_RID.SetValue(HN_RID);
//                    this.HEADER_DATE_TYPE.SetValue(HEADER_DATE_TYPE);
//                    this.HEADER_DATE_BETWEEN_FROM.SetValue(HEADER_DATE_BETWEEN_FROM);
//                    this.HEADER_DATE_BETWEEN_TO.SetValue(HEADER_DATE_BETWEEN_TO);
//                    this.HEADER_DATE_FROM.SetValue(HEADER_DATE_FROM);
//                    this.HEADER_DATE_TO.SetValue(HEADER_DATE_TO);
//                    this.RELEASE_DATE_TYPE.SetValue(RELEASE_DATE_TYPE);
//                    this.RELEASE_DATE_BETWEEN_FROM.SetValue(RELEASE_DATE_BETWEEN_FROM);
//                    this.RELEASE_DATE_BETWEEN_TO.SetValue(RELEASE_DATE_BETWEEN_TO);
//                    this.RELEASE_DATE_FROM.SetValue(RELEASE_DATE_FROM);
//                    this.RELEASE_DATE_TO.SetValue(RELEASE_DATE_TO);
//                    this.MAXIMUM_HEADERS.SetValue(MAXIMUM_HEADERS);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_TYPES_READ_def MID_AL_WRKSP_FILTER_TYPES_READ = new MID_AL_WRKSP_FILTER_TYPES_READ_def();
//            public class MID_AL_WRKSP_FILTER_TYPES_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_TYPES_READ.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_TYPES_READ_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_TYPES_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("AL_WRKSP_FILTER_TYPES");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_TYPES_INSERT_def MID_AL_WRKSP_FILTER_TYPES_INSERT = new MID_AL_WRKSP_FILTER_TYPES_INSERT_def();
//            public class MID_AL_WRKSP_FILTER_TYPES_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_TYPES_INSERT.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter USER_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_TYPE;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public charParameter TYPE_SELECTED;

//                public MID_AL_WRKSP_FILTER_TYPES_INSERT_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_TYPES_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER_TYPES");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    HEADER_TYPE = new intParameter("@HEADER_TYPE", base.inputParameterList);
//                    TYPE_SELECTED = new charParameter("@TYPE_SELECTED", base.inputParameterList);
//                }

//                public int Insert(DatabaseAccess _dba, 
//                                  int? USER_RID,
//                                  int? HEADER_TYPE,
//                                  char? TYPE_SELECTED
//                                 )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.HEADER_TYPE.SetValue(HEADER_TYPE);
//                    this.TYPE_SELECTED.SetValue(TYPE_SELECTED);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_UPDATE_def MID_AL_WRKSP_FILTER_UPDATE = new MID_AL_WRKSP_FILTER_UPDATE_def();
//            public class MID_AL_WRKSP_FILTER_UPDATE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_UPDATE.SQL"


//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter USER_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HN_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_TYPE;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_BETWEEN_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter HEADER_DATE_BETWEEN_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter HEADER_DATE_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter HEADER_DATE_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_TYPE;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_BETWEEN_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter RELEASE_DATE_BETWEEN_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter RELEASE_DATE_FROM;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public datetimeParameter RELEASE_DATE_TO;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "119")]
//                public intParameter MAXIMUM_HEADERS;

//                public MID_AL_WRKSP_FILTER_UPDATE_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_UPDATE";
//                    base.procedureType = storedProcedureTypes.Update;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
//                    HEADER_DATE_TYPE = new intParameter("@HEADER_DATE_TYPE", base.inputParameterList);
//                    HEADER_DATE_BETWEEN_FROM = new intParameter("@HEADER_DATE_BETWEEN_FROM", base.inputParameterList);
//                    HEADER_DATE_BETWEEN_TO = new intParameter("@HEADER_DATE_BETWEEN_TO", base.inputParameterList);
//                    HEADER_DATE_FROM = new datetimeParameter("@HEADER_DATE_FROM", base.inputParameterList);
//                    HEADER_DATE_TO = new datetimeParameter("@HEADER_DATE_TO", base.inputParameterList);
//                    RELEASE_DATE_TYPE = new intParameter("@RELEASE_DATE_TYPE", base.inputParameterList);
//                    RELEASE_DATE_BETWEEN_FROM = new intParameter("@RELEASE_DATE_BETWEEN_FROM", base.inputParameterList);
//                    RELEASE_DATE_BETWEEN_TO = new intParameter("@RELEASE_DATE_BETWEEN_TO", base.inputParameterList);
//                    RELEASE_DATE_FROM = new datetimeParameter("@RELEASE_DATE_FROM", base.inputParameterList);
//                    RELEASE_DATE_TO = new datetimeParameter("@RELEASE_DATE_TO", base.inputParameterList);
//                    MAXIMUM_HEADERS = new intParameter("@MAXIMUM_HEADERS", base.inputParameterList);
//                }

//                public int Update(DatabaseAccess _dba, 
//                                  int? USER_RID,
//                                  int? HN_RID,
//                                  int? HEADER_DATE_TYPE,
//                                  int? HEADER_DATE_BETWEEN_FROM,
//                                  int? HEADER_DATE_BETWEEN_TO,
//                                  DateTime? HEADER_DATE_FROM,
//                                  DateTime? HEADER_DATE_TO,
//                                  int? RELEASE_DATE_TYPE,
//                                  int? RELEASE_DATE_BETWEEN_FROM,
//                                  int? RELEASE_DATE_BETWEEN_TO,
//                                  DateTime? RELEASE_DATE_FROM,
//                                  DateTime? RELEASE_DATE_TO,
//                                  int? MAXIMUM_HEADERS
//                                 )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.HN_RID.SetValue(HN_RID);
//                    this.HEADER_DATE_TYPE.SetValue(HEADER_DATE_TYPE);
//                    this.HEADER_DATE_BETWEEN_FROM.SetValue(HEADER_DATE_BETWEEN_FROM);
//                    this.HEADER_DATE_BETWEEN_TO.SetValue(HEADER_DATE_BETWEEN_TO);
//                    this.HEADER_DATE_FROM.SetValue(HEADER_DATE_FROM);
//                    this.HEADER_DATE_TO.SetValue(HEADER_DATE_TO);
//                    this.RELEASE_DATE_TYPE.SetValue(RELEASE_DATE_TYPE);
//                    this.RELEASE_DATE_BETWEEN_FROM.SetValue(RELEASE_DATE_BETWEEN_FROM);
//                    this.RELEASE_DATE_BETWEEN_TO.SetValue(RELEASE_DATE_BETWEEN_TO);
//                    this.RELEASE_DATE_FROM.SetValue(RELEASE_DATE_FROM);
//                    this.RELEASE_DATE_TO.SetValue(RELEASE_DATE_TO);
//                    this.MAXIMUM_HEADERS.SetValue(MAXIMUM_HEADERS);
//                    return ExecuteStoredProcedureForUpdate(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_TYPES_DELETE_def MID_AL_WRKSP_FILTER_TYPES_DELETE = new MID_AL_WRKSP_FILTER_TYPES_DELETE_def();
//            public class MID_AL_WRKSP_FILTER_TYPES_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_TYPES_DELETE.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "104")]
//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_TYPES_DELETE_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_TYPES_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("AL_WRKSP_FILTER_TYPES");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                [UnitTestMethod(SelectStatement = "SELECT * FROM AL_WRKSP_FILTER_TYPES WHERE USER_RID = @USER_RID", Notes = "")]
//                public int Delete(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_STATUS_READ_def MID_AL_WRKSP_FILTER_STATUS_READ = new MID_AL_WRKSP_FILTER_STATUS_READ_def();
//            public class MID_AL_WRKSP_FILTER_STATUS_READ_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_STATUS_READ.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "104")]
//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_STATUS_READ_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_STATUS_READ";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("AL_WRKSP_FILTER_STATUS");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_STATUS_INSERT_def MID_AL_WRKSP_FILTER_STATUS_INSERT = new MID_AL_WRKSP_FILTER_STATUS_INSERT_def();
//            public class MID_AL_WRKSP_FILTER_STATUS_INSERT_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_STATUS_INSERT.SQL"

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "114")]
//                public intParameter USER_RID;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "802800")]
//                public intParameter HEADER_STATUS;

//                [UnitTestParameterAttribute(DB = "TQA", DefaultValue = "1")]
//                public charParameter STATUS_SELECTED;

//                public MID_AL_WRKSP_FILTER_STATUS_INSERT_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_STATUS_INSERT";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER_STATUS");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    HEADER_STATUS = new intParameter("@HEADER_STATUS", base.inputParameterList);
//                    STATUS_SELECTED = new charParameter("@STATUS_SELECTED", base.inputParameterList);
//                }

//                public int Insert(DatabaseAccess _dba, 
//                                  int? USER_RID,
//                                  int? HEADER_STATUS,
//                                  char? STATUS_SELECTED
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.HEADER_STATUS.SetValue(HEADER_STATUS);
//                    this.STATUS_SELECTED.SetValue(STATUS_SELECTED);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_STATUS_DELETE_def MID_AL_WRKSP_FILTER_STATUS_DELETE = new MID_AL_WRKSP_FILTER_STATUS_DELETE_def();
//            public class MID_AL_WRKSP_FILTER_STATUS_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_STATUS_DELETE.SQL"

//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_STATUS_DELETE_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_STATUS_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("AL_WRKSP_FILTER_STATUS");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                public int Delete(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_READ_FROM_NODE_def MID_AL_WRKSP_FILTER_READ_FROM_NODE = new MID_AL_WRKSP_FILTER_READ_FROM_NODE_def();
//            public class MID_AL_WRKSP_FILTER_READ_FROM_NODE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_READ_FROM_NODE.SQL"

//                public intParameter HN_RID;

//                public MID_AL_WRKSP_FILTER_READ_FROM_NODE_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_READ_FROM_NODE";
//                    base.procedureType = storedProcedureTypes.Read;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
//                }

//                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
//                {
//                    this.HN_RID.SetValue(HN_RID);
//                    return ExecuteStoredProcedureForRead(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_DELETE_def MID_AL_WRKSP_FILTER_DELETE = new MID_AL_WRKSP_FILTER_DELETE_def();
//            public class MID_AL_WRKSP_FILTER_DELETE_def : baseStoredProcedure
//            {
//                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_DELETE.SQL"

//                public intParameter USER_RID;

//                public MID_AL_WRKSP_FILTER_DELETE_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_DELETE";
//                    base.procedureType = storedProcedureTypes.Delete;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                }

//                public int Delete(DatabaseAccess _dba, int? USER_RID)
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    return ExecuteStoredProcedureForDelete(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW_def MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW = new MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW_def();
//            public class MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW_def : baseStoredProcedure
//            {
//                    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW.SQL"

//                public intParameter USER_RID;
//                public intParameter VIEW_RID;
			
//                public MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_INSERT_FROM_GRID_VIEW";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, int? USER_RID,
//                                  int? VIEW_RID
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.VIEW_RID.SetValue(VIEW_RID);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW_def MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW = new MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW_def();
//            public class MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW_def : baseStoredProcedure
//            {
//                    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW.SQL"

//                public intParameter USER_RID;
//                public intParameter VIEW_RID;
			
//                public MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_TYPES_INSERT_FROM_GRID_VIEW";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER_TYPES");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, int? USER_RID,
//                                  int? VIEW_RID
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.VIEW_RID.SetValue(VIEW_RID);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//            public static MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW_def MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW = new MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW_def();
//            public class MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW_def : baseStoredProcedure
//            {
//                    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW.SQL"

//                public intParameter USER_RID;
//                public intParameter VIEW_RID;
			
//                public MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW_def()
//                {
//                    base.procedureName = "MID_AL_WRKSP_FILTER_STATUS_INSERT_FROM_GRID_VIEW";
//                    base.procedureType = storedProcedureTypes.Insert;
//                    base.tableNames.Add("AL_WRKSP_FILTER_STATUS");
//                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
//                }
			
//                public int Insert(DatabaseAccess _dba, int? USER_RID,
//                                  int? VIEW_RID
//                                  )
//                {
//                    this.USER_RID.SetValue(USER_RID);
//                    this.VIEW_RID.SetValue(VIEW_RID);
//                    return ExecuteStoredProcedureForInsert(_dba);
//                }
//            }

//                //INSERT NEW STORED PROCEDURES ABOVE HERE

//        }
//    }  
//}
