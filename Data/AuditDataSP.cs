using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class AuditData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_PROC_HDR_READ_ALL_def MID_PROC_HDR_READ_ALL = new MID_PROC_HDR_READ_ALL_def();
            public class MID_PROC_HDR_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_READ_ALL.SQL"

                public MID_PROC_HDR_READ_ALL_def()
                {
                    base.procedureName = "MID_PROC_HDR_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PROC_HDR");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_PROC_HDR_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PROC_HDR_READ_def MID_PROC_HDR_READ = new MID_PROC_HDR_READ_def();
            public class MID_PROC_HDR_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_PROC_HDR_READ_def()
                {
                    base.procedureName = "MID_PROC_HDR_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PROC_HDR");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PROC_HDR_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_PROC_HDR_INSERT_def SP_MID_PROC_HDR_INSERT = new SP_MID_PROC_HDR_INSERT_def();
            public class SP_MID_PROC_HDR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_PROC_HDR_INSERT.SQL"

                private intParameter PROCESS_ID;
                private intParameter COMPLETION_STATUS_CODE;
                private intParameter EXECUTION_STATUS_CODE;
                private datetimeParameter START_TIME;
                private intParameter USER_RID;
                private intParameter PROCESS_SUMMARY;
                private intParameter PROCESS_RID; //Declare Output Parameter

                public SP_MID_PROC_HDR_INSERT_def()
                {
                    base.procedureName = "SP_MID_PROC_HDR_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("PROC_HDR");
                    PROCESS_ID = new intParameter("@PROCESS_ID", base.inputParameterList);
                    COMPLETION_STATUS_CODE = new intParameter("@COMPLETION_STATUS_CODE", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    START_TIME = new datetimeParameter("@START_TIME", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    PROCESS_SUMMARY = new intParameter("@PROCESS_SUMMARY", base.inputParameterList);
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                  int? PROCESS_ID,
                                  int? COMPLETION_STATUS_CODE,
                                  int? EXECUTION_STATUS_CODE,
                                  DateTime? START_TIME,
                                  int? USER_RID,
                                  int? PROCESS_SUMMARY
                                  )
                {
                    lock (typeof(SP_MID_PROC_HDR_INSERT_def))
                    {
                        this.PROCESS_ID.SetValue(PROCESS_ID);
                        this.COMPLETION_STATUS_CODE.SetValue(COMPLETION_STATUS_CODE);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.START_TIME.SetValue(START_TIME);
                        this.USER_RID.SetValue(USER_RID);
                        this.PROCESS_SUMMARY.SetValue(PROCESS_SUMMARY);
                        this.PROCESS_RID.SetValue(null); //Initialize Output Parameter
                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_PROC_HDR_UPDATE_def MID_PROC_HDR_UPDATE = new MID_PROC_HDR_UPDATE_def();
            public class MID_PROC_HDR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_UPDATE.SQL"

                private intParameter PROCESS_RID;
                private intParameter COMPLETION_STATUS_CODE;
                private intParameter EXECUTION_STATUS_CODE;
                private datetimeParameter STOP_TIME;
                private intParameter SUMMARY_CODE;
                private intParameter HIGHEST_LEVEL;
                private charParameter PROC_DESC;

                public MID_PROC_HDR_UPDATE_def()
                {
                    base.procedureName = "MID_PROC_HDR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PROC_HDR");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    COMPLETION_STATUS_CODE = new intParameter("@COMPLETION_STATUS_CODE", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    STOP_TIME = new datetimeParameter("@STOP_TIME", base.inputParameterList);
                    SUMMARY_CODE = new intParameter("@SUMMARY_CODE", base.inputParameterList);
                    HIGHEST_LEVEL = new intParameter("@HIGHEST_LEVEL", base.inputParameterList);
                    PROC_DESC = new charParameter("@PROC_DESC", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? COMPLETION_STATUS_CODE,
                                  int? EXECUTION_STATUS_CODE,
                                  DateTime? STOP_TIME,
                                  int? SUMMARY_CODE,
                                  int? HIGHEST_LEVEL,
                                  char? PROC_DESC
                                  )
                {
                    lock (typeof(MID_PROC_HDR_UPDATE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.COMPLETION_STATUS_CODE.SetValue(COMPLETION_STATUS_CODE);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.STOP_TIME.SetValue(STOP_TIME);
                        this.SUMMARY_CODE.SetValue(SUMMARY_CODE);
                        this.HIGHEST_LEVEL.SetValue(HIGHEST_LEVEL);
                        this.PROC_DESC.SetValue(PROC_DESC);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PROC_HDR_UPDATE_NON_COMPLETE_def MID_PROC_HDR_UPDATE_NON_COMPLETE = new MID_PROC_HDR_UPDATE_NON_COMPLETE_def();
            public class MID_PROC_HDR_UPDATE_NON_COMPLETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_UPDATE_NON_COMPLETE.SQL"

                private intParameter PROCESS_RID;
                private intParameter EXECUTION_STATUS_CODE;
                private datetimeParameter STOP_TIME;
                private intParameter SUMMARY_CODE;
                private intParameter HIGHEST_LEVEL;
                private charParameter PROC_DESC;

                public MID_PROC_HDR_UPDATE_NON_COMPLETE_def()
                {
                    base.procedureName = "MID_PROC_HDR_UPDATE_NON_COMPLETE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PROC_HDR");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    STOP_TIME = new datetimeParameter("@STOP_TIME", base.inputParameterList);
                    SUMMARY_CODE = new intParameter("@SUMMARY_CODE", base.inputParameterList);
                    HIGHEST_LEVEL = new intParameter("@HIGHEST_LEVEL", base.inputParameterList);
                    PROC_DESC = new charParameter("@PROC_DESC", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? EXECUTION_STATUS_CODE,
                                  DateTime? STOP_TIME,
                                  int? SUMMARY_CODE,
                                  int? HIGHEST_LEVEL,
                                  char? PROC_DESC
                                  )
                {
                    lock (typeof(MID_PROC_HDR_UPDATE_NON_COMPLETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.STOP_TIME.SetValue(STOP_TIME);
                        this.SUMMARY_CODE.SetValue(SUMMARY_CODE);
                        this.HIGHEST_LEVEL.SetValue(HIGHEST_LEVEL);
                        this.PROC_DESC.SetValue(PROC_DESC);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PROC_RPT_UPDATE_IF_UNEXPECTED_def MID_PROC_RPT_UPDATE_IF_UNEXPECTED = new MID_PROC_RPT_UPDATE_IF_UNEXPECTED_def();
            public class MID_PROC_RPT_UPDATE_IF_UNEXPECTED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_UPDATE_IF_UNEXPECTED.SQL"

                private intParameter COMPLETION_STATUS_CODE;
                private intParameter EXECUTION_STATUS_CODE;
                private intParameter SUMMARY_CODE;
                private intParameter PROCESS_RID;

                public MID_PROC_RPT_UPDATE_IF_UNEXPECTED_def()
                {
                    base.procedureName = "MID_PROC_RPT_UPDATE_IF_UNEXPECTED";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PROC_RPT");
                    COMPLETION_STATUS_CODE = new intParameter("@COMPLETION_STATUS_CODE", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    SUMMARY_CODE = new intParameter("@SUMMARY_CODE", base.inputParameterList);
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? COMPLETION_STATUS_CODE,
                                  int? EXECUTION_STATUS_CODE,
                                  int? SUMMARY_CODE,
                                  int? PROCESS_RID
                                  )
                {
                    lock (typeof(MID_PROC_RPT_UPDATE_IF_UNEXPECTED_def))
                    {
                        this.COMPLETION_STATUS_CODE.SetValue(COMPLETION_STATUS_CODE);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.SUMMARY_CODE.SetValue(SUMMARY_CODE);
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION_def MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION = new MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION_def();
            public class MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION.SQL"

                private intParameter COMPLETION_STATUS_CODE;
                private intParameter EXECUTION_STATUS_CODE;
                private intParameter SUMMARY_CODE;

                public MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION_def()
                {
                    base.procedureName = "MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PROC_RPT");
                    COMPLETION_STATUS_CODE = new intParameter("@COMPLETION_STATUS_CODE", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    SUMMARY_CODE = new intParameter("@SUMMARY_CODE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? COMPLETION_STATUS_CODE,
                                  int? EXECUTION_STATUS_CODE,
                                  int? SUMMARY_CODE
                                  )
                {
                    lock (typeof(MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION_def))
                    {
                        this.COMPLETION_STATUS_CODE.SetValue(COMPLETION_STATUS_CODE);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.SUMMARY_CODE.SetValue(SUMMARY_CODE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            // Begin TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
            public static MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION_def MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION = new MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION_def();
            public class MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION.SQL"

                private intParameter COMPLETION_STATUS_CODE;
                private intParameter EXECUTION_STATUS_CODE;
                private intParameter SUMMARY_CODE;
                private intParameter PROCESS_ID;

                public MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION_def()
                {
                    base.procedureName = "MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PROC_RPT");
                    COMPLETION_STATUS_CODE = new intParameter("@COMPLETION_STATUS_CODE", base.inputParameterList);
                    EXECUTION_STATUS_CODE = new intParameter("@EXECUTION_STATUS_CODE", base.inputParameterList);
                    SUMMARY_CODE = new intParameter("@SUMMARY_CODE", base.inputParameterList);
                    PROCESS_ID = new intParameter("@PROCESS_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? PROCESS_ID,
                                  int? COMPLETION_STATUS_CODE,
                                  int? EXECUTION_STATUS_CODE,
                                  int? SUMMARY_CODE
                                  )
                {
                    lock (typeof(MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION_def))
                    {
                        this.COMPLETION_STATUS_CODE.SetValue(COMPLETION_STATUS_CODE);
                        this.EXECUTION_STATUS_CODE.SetValue(EXECUTION_STATUS_CODE);
                        this.SUMMARY_CODE.SetValue(SUMMARY_CODE);
                        this.PROCESS_ID.SetValue(PROCESS_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }
            // End TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination

            public static MID_PROC_HDR_DELETE_def MID_PROC_HDR_DELETE = new MID_PROC_HDR_DELETE_def();
            public class MID_PROC_HDR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_PROC_HDR_DELETE_def()
                {
                    base.procedureName = "MID_PROC_HDR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PROC_HDR");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PROC_HDR_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PROC_RPT_READ_FOR_AUDIT_REPORT_def MID_PROC_RPT_READ_FOR_AUDIT_REPORT = new MID_PROC_RPT_READ_FOR_AUDIT_REPORT_def();
            public class MID_PROC_RPT_READ_FOR_AUDIT_REPORT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_READ_FOR_AUDIT_REPORT.SQL"

                private intParameter PROCESS_RID;

                public MID_PROC_RPT_READ_FOR_AUDIT_REPORT_def()
                {
                    base.procedureName = "MID_PROC_RPT_READ_FOR_AUDIT_REPORT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PROC_RPT");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PROC_RPT_READ_FOR_AUDIT_REPORT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PROC_RPT_INSERT_def MID_PROC_RPT_INSERT = new MID_PROC_RPT_INSERT_def();
            public class MID_PROC_RPT_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_INSERT.SQL"

                private intParameter PROCESS_RID;
                private datetimeParameter TIME_STAMP;
                private stringParameter REPORTING_MODULE;
                private intParameter LINE_NUMBER;
                private intParameter MESSAGE_LEVEL;
                private intParameter MESSAGE_CODE;
                private stringParameter REPORT_MESSAGE;

                public MID_PROC_RPT_INSERT_def()
                {
                    base.procedureName = "MID_PROC_RPT_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PROC_RPT");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    TIME_STAMP = new datetimeParameter("@TIME_STAMP", base.inputParameterList);
                    REPORTING_MODULE = new stringParameter("@REPORTING_MODULE", base.inputParameterList);
                    LINE_NUMBER = new intParameter("@LINE_NUMBER", base.inputParameterList);
                    MESSAGE_LEVEL = new intParameter("@MESSAGE_LEVEL", base.inputParameterList);
                    MESSAGE_CODE = new intParameter("@MESSAGE_CODE", base.inputParameterList);
                    REPORT_MESSAGE = new stringParameter("@REPORT_MESSAGE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  DateTime? TIME_STAMP,
                                  string REPORTING_MODULE,
                                  int? LINE_NUMBER,
                                  int? MESSAGE_LEVEL,
                                  int? MESSAGE_CODE,
                                  string REPORT_MESSAGE
                                  )
                {
                    lock (typeof(MID_PROC_RPT_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.TIME_STAMP.SetValue(TIME_STAMP);
                        this.REPORTING_MODULE.SetValue(REPORTING_MODULE);
                        this.LINE_NUMBER.SetValue(LINE_NUMBER);
                        this.MESSAGE_LEVEL.SetValue(MESSAGE_LEVEL);
                        this.MESSAGE_CODE.SetValue(MESSAGE_CODE);
                        this.REPORT_MESSAGE.SetValue(REPORT_MESSAGE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PROC_RPT_DELETE_def MID_PROC_RPT_DELETE = new MID_PROC_RPT_DELETE_def();
            public class MID_PROC_RPT_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_RPT_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_PROC_RPT_DELETE_def()
                {
                    base.procedureName = "MID_PROC_RPT_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PROC_RPT");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PROC_RPT_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_POSTING_INFO_READ_ALL_def MID_POSTING_INFO_READ_ALL = new MID_POSTING_INFO_READ_ALL_def();
            public class MID_POSTING_INFO_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_INFO_READ_ALL.SQL"

                public MID_POSTING_INFO_READ_ALL_def()
                {
                    base.procedureName = "MID_POSTING_INFO_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("POSTING_INFO");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_POSTING_INFO_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#1267-MD -jsobek -5.22 Audit Summary results do not display
            public static MID_POSTING_INFO_READ_def MID_POSTING_INFO_READ = new MID_POSTING_INFO_READ_def();
            public class MID_POSTING_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_POSTING_INFO_READ_def()
                {
                    base.procedureName = "MID_POSTING_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("POSTING_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_POSTING_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1267-MD -jsobek -5.22 Audit Summary results do not display

            public static MID_POSTING_INFO_INSERT_def MID_POSTING_INFO_INSERT = new MID_POSTING_INFO_INSERT_def();
            public class MID_POSTING_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter CH_DAY_HIS_RECS;
                private intParameter CH_WK_HIS_RECS;
                private intParameter CH_WK_FOR_RECS;
                private intParameter ST_DAY_HIS_RECS;
                private intParameter ST_WK_HIS_RECS;
                private intParameter ST_WK_FOR_RECS;
                private intParameter INTRANSIT_RECS;
                private intParameter RECS_WITH_ERRORS;
                private intParameter NODES_ADDED;

                public MID_POSTING_INFO_INSERT_def()
                {
                    base.procedureName = "MID_POSTING_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("POSTING_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    CH_DAY_HIS_RECS = new intParameter("@CH_DAY_HIS_RECS", base.inputParameterList);
                    CH_WK_HIS_RECS = new intParameter("@CH_WK_HIS_RECS", base.inputParameterList);
                    CH_WK_FOR_RECS = new intParameter("@CH_WK_FOR_RECS", base.inputParameterList);
                    ST_DAY_HIS_RECS = new intParameter("@ST_DAY_HIS_RECS", base.inputParameterList);
                    ST_WK_HIS_RECS = new intParameter("@ST_WK_HIS_RECS", base.inputParameterList);
                    ST_WK_FOR_RECS = new intParameter("@ST_WK_FOR_RECS", base.inputParameterList);
                    INTRANSIT_RECS = new intParameter("@INTRANSIT_RECS", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    NODES_ADDED = new intParameter("@NODES_ADDED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? CH_DAY_HIS_RECS,
                                  int? CH_WK_HIS_RECS,
                                  int? CH_WK_FOR_RECS,
                                  int? ST_DAY_HIS_RECS,
                                  int? ST_WK_HIS_RECS,
                                  int? ST_WK_FOR_RECS,
                                  int? INTRANSIT_RECS,
                                  int? RECS_WITH_ERRORS,
                                  int? NODES_ADDED
                                  )
                {
                    lock (typeof(MID_POSTING_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.CH_DAY_HIS_RECS.SetValue(CH_DAY_HIS_RECS);
                        this.CH_WK_HIS_RECS.SetValue(CH_WK_HIS_RECS);
                        this.CH_WK_FOR_RECS.SetValue(CH_WK_FOR_RECS);
                        this.ST_DAY_HIS_RECS.SetValue(ST_DAY_HIS_RECS);
                        this.ST_WK_HIS_RECS.SetValue(ST_WK_HIS_RECS);
                        this.ST_WK_FOR_RECS.SetValue(ST_WK_FOR_RECS);
                        this.INTRANSIT_RECS.SetValue(INTRANSIT_RECS);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.NODES_ADDED.SetValue(NODES_ADDED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_POSTING_INFO_DELETE_def MID_POSTING_INFO_DELETE = new MID_POSTING_INFO_DELETE_def();
            public class MID_POSTING_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_POSTING_INFO_DELETE_def()
                {
                    base.procedureName = "MID_POSTING_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("POSTING_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_POSTING_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_LOAD_INFO_READ_def MID_HIER_LOAD_INFO_READ = new MID_HIER_LOAD_INFO_READ_def();
            public class MID_HIER_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_HIER_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_HIER_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HIER_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_RECLASS_INFO_READ_def MID_HIER_RECLASS_INFO_READ = new MID_HIER_RECLASS_INFO_READ_def();
            public class MID_HIER_RECLASS_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_RECLASS_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_HIER_RECLASS_INFO_READ_def()
                {
                    base.procedureName = "MID_HIER_RECLASS_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_RECLASS_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HIER_RECLASS_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			// Begin TT#1581-MD - stodd - API Header Reconcile
            public static MID_HEADER_RECONCILE_INFO_READ_def MID_HEADER_RECONCILE_INFO_READ = new MID_HEADER_RECONCILE_INFO_READ_def();
            public class MID_HEADER_RECONCILE_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_RECONCILE_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_HEADER_RECONCILE_INFO_READ_def()
                {
                    base.procedureName = "MID_HEADER_RECONCILE_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_RECONCILE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HIER_RECLASS_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
			// End TT#1581-MD - stodd - API Header Reconcile

            public static MID_HIER_LOAD_INFO_INSERT_def MID_HIER_LOAD_INFO_INSERT = new MID_HIER_LOAD_INFO_INSERT_def();
            public class MID_HIER_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HIER_RECS;
                private intParameter LEVEL_RECS;
                private intParameter MERCH_RECS;
                private intParameter MOVE_RECS;
                private intParameter RENAME_RECS;
                private intParameter DELETE_RECS;
                private intParameter MERCH_ADDED;
                private intParameter MERCH_UPDATED;
                private intParameter RECS_WITH_ERRORS;

                public MID_HIER_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_HIER_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HIER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HIER_RECS = new intParameter("@HIER_RECS", base.inputParameterList);
                    LEVEL_RECS = new intParameter("@LEVEL_RECS", base.inputParameterList);
                    MERCH_RECS = new intParameter("@MERCH_RECS", base.inputParameterList);
                    MOVE_RECS = new intParameter("@MOVE_RECS", base.inputParameterList);
                    RENAME_RECS = new intParameter("@RENAME_RECS", base.inputParameterList);
                    DELETE_RECS = new intParameter("@DELETE_RECS", base.inputParameterList);
                    MERCH_ADDED = new intParameter("@MERCH_ADDED", base.inputParameterList);
                    MERCH_UPDATED = new intParameter("@MERCH_UPDATED", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? HIER_RECS,
                                  int? LEVEL_RECS,
                                  int? MERCH_RECS,
                                  int? MOVE_RECS,
                                  int? RENAME_RECS,
                                  int? DELETE_RECS,
                                  int? MERCH_ADDED,
                                  int? MERCH_UPDATED,
                                  int? RECS_WITH_ERRORS
                                  )
                {
                    lock (typeof(MID_HIER_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HIER_RECS.SetValue(HIER_RECS);
                        this.LEVEL_RECS.SetValue(LEVEL_RECS);
                        this.MERCH_RECS.SetValue(MERCH_RECS);
                        this.MOVE_RECS.SetValue(MOVE_RECS);
                        this.RENAME_RECS.SetValue(RENAME_RECS);
                        this.DELETE_RECS.SetValue(DELETE_RECS);
                        this.MERCH_ADDED.SetValue(MERCH_ADDED);
                        this.MERCH_UPDATED.SetValue(MERCH_UPDATED);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HIER_LOAD_INFO_DELETE_def MID_HIER_LOAD_INFO_DELETE = new MID_HIER_LOAD_INFO_DELETE_def();
            public class MID_HIER_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_HIER_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_HIER_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HIER_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STR_LOAD_INFO_READ_def MID_STR_LOAD_INFO_READ = new MID_STR_LOAD_INFO_READ_def();
            public class MID_STR_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STR_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_STR_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_STR_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STR_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_STR_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STR_LOAD_INFO_INSERT_def MID_STR_LOAD_INFO_INSERT = new MID_STR_LOAD_INFO_INSERT_def();
            public class MID_STR_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STR_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter STORE_RECS;
                private intParameter STORES_CREATED;
                private intParameter STORES_MODIFIED;
                private intParameter STORES_DELETED;
                private intParameter STORES_RECOVERED;
                private intParameter RECS_WITH_ERRORS;

                public MID_STR_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_STR_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STR_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    STORE_RECS = new intParameter("@STORE_RECS", base.inputParameterList);
                    STORES_CREATED = new intParameter("@STORES_CREATED", base.inputParameterList);
                    STORES_MODIFIED = new intParameter("@STORES_MODIFIED", base.inputParameterList);
                    STORES_DELETED = new intParameter("@STORES_DELETED", base.inputParameterList);
                    STORES_RECOVERED = new intParameter("@STORES_RECOVERED", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? STORE_RECS,
                                  int? STORES_CREATED,
                                  int? STORES_MODIFIED,
                                  int? STORES_DELETED,
                                  int? STORES_RECOVERED,
                                  int? RECS_WITH_ERRORS
                                  )
                {
                    lock (typeof(MID_STR_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.STORE_RECS.SetValue(STORE_RECS);
                        this.STORES_CREATED.SetValue(STORES_CREATED);
                        this.STORES_MODIFIED.SetValue(STORES_MODIFIED);
                        this.STORES_DELETED.SetValue(STORES_DELETED);
                        this.STORES_RECOVERED.SetValue(STORES_RECOVERED);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STR_LOAD_INFO_DELETE_def MID_STR_LOAD_INFO_DELETE = new MID_STR_LOAD_INFO_DELETE_def();
            public class MID_STR_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STR_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_STR_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_STR_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STR_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_STR_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SPECIAL_REQUEST_INFO_READ_def MID_SPECIAL_REQUEST_INFO_READ = new MID_SPECIAL_REQUEST_INFO_READ_def();
            public class MID_SPECIAL_REQUEST_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SPECIAL_REQUEST_INFO_READ_def()
                {
                    base.procedureName = "MID_SPECIAL_REQUEST_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SPECIAL_REQUEST_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SPECIAL_REQUEST_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SPECIAL_REQUEST_INFO_INSERT_def MID_SPECIAL_REQUEST_INFO_INSERT = new MID_SPECIAL_REQUEST_INFO_INSERT_def();
            public class MID_SPECIAL_REQUEST_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter TOTAL_JOBS;
                private intParameter JOBS_PROCESSED;
                private intParameter JOBS_WITH_ERRORS;
                private intParameter SUCCESSFUL_JOBS;

                public MID_SPECIAL_REQUEST_INFO_INSERT_def()
                {
                    base.procedureName = "MID_SPECIAL_REQUEST_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SPECIAL_REQUEST_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    TOTAL_JOBS = new intParameter("@TOTAL_JOBS", base.inputParameterList);
                    JOBS_PROCESSED = new intParameter("@JOBS_PROCESSED", base.inputParameterList);
                    JOBS_WITH_ERRORS = new intParameter("@JOBS_WITH_ERRORS", base.inputParameterList);
                    SUCCESSFUL_JOBS = new intParameter("@SUCCESSFUL_JOBS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? TOTAL_JOBS,
                                  int? JOBS_PROCESSED,
                                  int? JOBS_WITH_ERRORS,
                                  int? SUCCESSFUL_JOBS
                                  )
                {
                    lock (typeof(MID_SPECIAL_REQUEST_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.TOTAL_JOBS.SetValue(TOTAL_JOBS);
                        this.JOBS_PROCESSED.SetValue(JOBS_PROCESSED);
                        this.JOBS_WITH_ERRORS.SetValue(JOBS_WITH_ERRORS);
                        this.SUCCESSFUL_JOBS.SetValue(SUCCESSFUL_JOBS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SPECIAL_REQUEST_INFO_DELETE_def MID_SPECIAL_REQUEST_INFO_DELETE = new MID_SPECIAL_REQUEST_INFO_DELETE_def();
            public class MID_SPECIAL_REQUEST_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_SPECIAL_REQUEST_INFO_DELETE_def()
                {
                    base.procedureName = "MID_SPECIAL_REQUEST_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SPECIAL_REQUEST_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SPECIAL_REQUEST_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PURGE_INFO_READ_def MID_PURGE_INFO_READ = new MID_PURGE_INFO_READ_def();
            public class MID_PURGE_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_PURGE_INFO_READ_def()
                {
                    base.procedureName = "MID_PURGE_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PURGE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PURGE_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PURGE_INFO_INSERT_def MID_PURGE_INFO_INSERT = new MID_PURGE_INFO_INSERT_def();
            public class MID_PURGE_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter STORE_DAILY_HISTORY;
                private intParameter CHAIN_WEEKLY_HISTORY;
                private intParameter STORE_WEEKLY_HISTORY;
                private intParameter CHAIN_WEEKLY_FORECAST;
                private intParameter STORE_WEEKLY_FORECAST;
                private intParameter HEADERS;
                private intParameter INTRANSIT;
                private intParameter INTRANSIT_REV;
                private intParameter USERS;
                private intParameter GROUPS;
                private intParameter DAILY_PERCENTAGES;
                private intParameter EMPTY_ATTRIBUTE_SETS;
                private intParameter AUDITS;
                private intParameter IMO_REV; //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge

                public MID_PURGE_INFO_INSERT_def()
                {
                    base.procedureName = "MID_PURGE_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PURGE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    STORE_DAILY_HISTORY = new intParameter("@STORE_DAILY_HISTORY", base.inputParameterList);
                    CHAIN_WEEKLY_HISTORY = new intParameter("@CHAIN_WEEKLY_HISTORY", base.inputParameterList);
                    STORE_WEEKLY_HISTORY = new intParameter("@STORE_WEEKLY_HISTORY", base.inputParameterList);
                    CHAIN_WEEKLY_FORECAST = new intParameter("@CHAIN_WEEKLY_FORECAST", base.inputParameterList);
                    STORE_WEEKLY_FORECAST = new intParameter("@STORE_WEEKLY_FORECAST", base.inputParameterList);
                    HEADERS = new intParameter("@HEADERS", base.inputParameterList);
                    INTRANSIT = new intParameter("@INTRANSIT", base.inputParameterList);
                    INTRANSIT_REV = new intParameter("@INTRANSIT_REV", base.inputParameterList);
                    USERS = new intParameter("@USERS", base.inputParameterList);
                    GROUPS = new intParameter("@GROUPS", base.inputParameterList);
                    DAILY_PERCENTAGES = new intParameter("@DAILY_PERCENTAGES", base.inputParameterList);
                    EMPTY_ATTRIBUTE_SETS = new intParameter("@EMPTY_ATTRIBUTE_SETS", base.inputParameterList);
                    AUDITS = new intParameter("@AUDITS", base.inputParameterList);
                    IMO_REV = new intParameter("@IMO_REV", base.inputParameterList); //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? STORE_DAILY_HISTORY,
                                  int? CHAIN_WEEKLY_HISTORY,
                                  int? STORE_WEEKLY_HISTORY,
                                  int? CHAIN_WEEKLY_FORECAST,
                                  int? STORE_WEEKLY_FORECAST,
                                  int? HEADERS,
                                  int? INTRANSIT,
                                  int? INTRANSIT_REV,
                                  int? USERS,
                                  int? GROUPS,
                                  int? DAILY_PERCENTAGES,
                                  int? EMPTY_ATTRIBUTE_SETS,
                                  int? AUDITS,
                                  int? IMO_REV //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
                                  )
                {
                    lock (typeof(MID_PURGE_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.STORE_DAILY_HISTORY.SetValue(STORE_DAILY_HISTORY);
                        this.CHAIN_WEEKLY_HISTORY.SetValue(CHAIN_WEEKLY_HISTORY);
                        this.STORE_WEEKLY_HISTORY.SetValue(STORE_WEEKLY_HISTORY);
                        this.CHAIN_WEEKLY_FORECAST.SetValue(CHAIN_WEEKLY_FORECAST);
                        this.STORE_WEEKLY_FORECAST.SetValue(STORE_WEEKLY_FORECAST);
                        this.HEADERS.SetValue(HEADERS);
                        this.INTRANSIT.SetValue(INTRANSIT);
                        this.INTRANSIT_REV.SetValue(INTRANSIT_REV);
                        this.USERS.SetValue(USERS);
                        this.GROUPS.SetValue(GROUPS);
                        this.DAILY_PERCENTAGES.SetValue(DAILY_PERCENTAGES);
                        this.EMPTY_ATTRIBUTE_SETS.SetValue(EMPTY_ATTRIBUTE_SETS);
                        this.AUDITS.SetValue(AUDITS);
                    	this.IMO_REV.SetValue(IMO_REV); //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PURGE_INFO_DELETE_def MID_PURGE_INFO_DELETE = new MID_PURGE_INFO_DELETE_def();
            public class MID_PURGE_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_PURGE_INFO_DELETE_def()
                {
                    base.procedureName = "MID_PURGE_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PURGE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PURGE_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_LOAD_INFO_READ_def MID_HEADER_LOAD_INFO_READ = new MID_HEADER_LOAD_INFO_READ_def();
            public class MID_HEADER_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_HEADER_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_HEADER_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HEADER_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_RECLASS_INFO_INSERT_def MID_HIER_RECLASS_INFO_INSERT = new MID_HIER_RECLASS_INFO_INSERT_def();
            public class MID_HIER_RECLASS_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_RECLASS_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HEIR_TRANS_WRITTEN;
                private intParameter ADDCHG_TRANS_WRITTEN;
                private intParameter DELETE_TRANS_WRITTEN;
                private intParameter MOVE_TRANS_WRITTEN;
                private intParameter TRANS_REJECTED;

                public MID_HIER_RECLASS_INFO_INSERT_def()
                {
                    base.procedureName = "MID_HIER_RECLASS_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HIER_RECLASS_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HEIR_TRANS_WRITTEN = new intParameter("@HEIR_TRANS_WRITTEN", base.inputParameterList);
                    ADDCHG_TRANS_WRITTEN = new intParameter("@ADDCHG_TRANS_WRITTEN", base.inputParameterList);
                    DELETE_TRANS_WRITTEN = new intParameter("@DELETE_TRANS_WRITTEN", base.inputParameterList);
                    MOVE_TRANS_WRITTEN = new intParameter("@MOVE_TRANS_WRITTEN", base.inputParameterList);
                    TRANS_REJECTED = new intParameter("@TRANS_REJECTED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? HEIR_TRANS_WRITTEN,
                                  int? ADDCHG_TRANS_WRITTEN,
                                  int? DELETE_TRANS_WRITTEN,
                                  int? MOVE_TRANS_WRITTEN,
                                  int? TRANS_REJECTED
                                  )
                {
                    lock (typeof(MID_HIER_RECLASS_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HEIR_TRANS_WRITTEN.SetValue(HEIR_TRANS_WRITTEN);
                        this.ADDCHG_TRANS_WRITTEN.SetValue(ADDCHG_TRANS_WRITTEN);
                        this.DELETE_TRANS_WRITTEN.SetValue(DELETE_TRANS_WRITTEN);
                        this.MOVE_TRANS_WRITTEN.SetValue(MOVE_TRANS_WRITTEN);
                        this.TRANS_REJECTED.SetValue(TRANS_REJECTED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

			// Begin TT#1581-MD - stodd - API Header Reconcile
            public static MID_HEADER_RECONCILE_INFO_INSERT_def MID_HEADER_RECONCILE_INFO_INSERT = new MID_HEADER_RECONCILE_INFO_INSERT_def();
            public class MID_HEADER_RECONCILE_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_RECONCILE_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HEADER_FILES_READ;
                private intParameter HEADER_TRANS_READ;
                private intParameter HEADER_TRANS_WRITTEN;
                private intParameter HEADER_FILES_WRITTEN;
                private intParameter HEADER_TRANS_DUPLICATES;
                private intParameter HEADER_TRANS_SKIPPED;
                private intParameter REMOVE_TRANS_WRITTEN;
                private intParameter REMOVE_FILES_WRITTEN;


                public MID_HEADER_RECONCILE_INFO_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_RECONCILE_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_RECONCILE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HEADER_FILES_READ = new intParameter("@HEADER_FILES_READ", base.inputParameterList);
                    HEADER_TRANS_READ = new intParameter("@HEADER_TRANS_READ", base.inputParameterList);
                    HEADER_TRANS_WRITTEN = new intParameter("@HEADER_TRANS_WRITTEN", base.inputParameterList);
                    HEADER_FILES_WRITTEN = new intParameter("@HEADER_FILES_WRITTEN", base.inputParameterList);
                    HEADER_TRANS_DUPLICATES = new intParameter("@HEADER_TRANS_DUPLICATES", base.inputParameterList);
                    HEADER_TRANS_SKIPPED = new intParameter("@HEADER_TRANS_SKIPPED", base.inputParameterList);
                    REMOVE_TRANS_WRITTEN = new intParameter("@REMOVE_TRANS_WRITTEN", base.inputParameterList);
                    REMOVE_FILES_WRITTEN = new intParameter("@REMOVE_FILES_WRITTEN", base.inputParameterList);

                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? HEADER_FILES_READ,
                                  int? HEADER_TRANS_READ,
                                  int? HEADER_TRANS_WRITTEN,
                                  int? HEADER_FILES_WRITTEN,
                                  int? HEADER_TRANS_DUPLICATES,
                                  int? HEADER_TRANS_SKIPPED,
                                  int? REMOVE_TRANS_WRITTEN,
                                  int? REMOVE_FILES_WRITTEN
                                  )
                {
                    lock (typeof(MID_HIER_RECLASS_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HEADER_FILES_READ.SetValue(HEADER_FILES_READ);
                        this.HEADER_TRANS_READ.SetValue(HEADER_TRANS_READ);
                        this.HEADER_TRANS_WRITTEN.SetValue(HEADER_TRANS_WRITTEN);
                        this.HEADER_FILES_WRITTEN.SetValue(HEADER_FILES_WRITTEN);
                        this.HEADER_TRANS_DUPLICATES.SetValue(HEADER_TRANS_DUPLICATES);
                        this.HEADER_TRANS_SKIPPED.SetValue(HEADER_TRANS_SKIPPED);
                        this.REMOVE_TRANS_WRITTEN.SetValue(REMOVE_TRANS_WRITTEN);
                        this.REMOVE_FILES_WRITTEN.SetValue(REMOVE_FILES_WRITTEN);

                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }
			// End TT#1581-MD - stodd - API Header Reconcile

            public static MID_HEADER_LOAD_INFO_INSERT_def MID_HEADER_LOAD_INFO_INSERT = new MID_HEADER_LOAD_INFO_INSERT_def();
            public class MID_HEADER_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter HDRS_CREATED;
                private intParameter HDRS_MODIFIED;
                private intParameter HDRS_REMOVED;
                private intParameter HDRS_RESET;

                public MID_HEADER_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    HDRS_CREATED = new intParameter("@HDRS_CREATED", base.inputParameterList);
                    HDRS_MODIFIED = new intParameter("@HDRS_MODIFIED", base.inputParameterList);
                    HDRS_REMOVED = new intParameter("@HDRS_REMOVED", base.inputParameterList);
                    HDRS_RESET = new intParameter("@HDRS_RESET", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? HDRS_CREATED,
                                  int? HDRS_MODIFIED,
                                  int? HDRS_REMOVED,
                                  int? HDRS_RESET
                                  )
                {
                    lock (typeof(MID_HEADER_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.HDRS_CREATED.SetValue(HDRS_CREATED);
                        this.HDRS_MODIFIED.SetValue(HDRS_MODIFIED);
                        this.HDRS_REMOVED.SetValue(HDRS_REMOVED);
                        this.HDRS_RESET.SetValue(HDRS_RESET);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_LOAD_INFO_DELETE_def MID_HEADER_LOAD_INFO_DELETE = new MID_HEADER_LOAD_INFO_DELETE_def();
            public class MID_HEADER_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_HEADER_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_HEADER_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ_def MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ = new MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ_def();
            public class MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("BUILD_PACK_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT_def MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT = new MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT_def();
            public class MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CRITERIA_ADDED_UPDATED;

                public MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("BUILD_PACK_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CRITERIA_ADDED_UPDATED = new intParameter("@CRITERIA_ADDED_UPDATED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CRITERIA_ADDED_UPDATED
                                  )
                {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CRITERIA_ADDED_UPDATED.SetValue(CRITERIA_ADDED_UPDATED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE_def MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE = new MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE_def();
            public class MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("BUILD_PACK_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ_def MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ = new MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ_def();
            public class MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT_def MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT = new MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT_def();
            public class MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CRITERIA_ADDED_UPDATED;

                public MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CRITERIA_ADDED_UPDATED = new intParameter("@CRITERIA_ADDED_UPDATED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CRITERIA_ADDED_UPDATED
                                  )
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CRITERIA_ADDED_UPDATED.SetValue(CRITERIA_ADDED_UPDATED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE_def MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE = new MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE_def();
            public class MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ_def MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ = new MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ_def();
            public class MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("DAILY_PERCENTAGES_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT_def MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT = new MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT_def();
            public class MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CRITERIA_ADDED_UPDATED;

                public MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("DAILY_PERCENTAGES_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CRITERIA_ADDED_UPDATED = new intParameter("@CRITERIA_ADDED_UPDATED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CRITERIA_ADDED_UPDATED
                                  )
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CRITERIA_ADDED_UPDATED.SetValue(CRITERIA_ADDED_UPDATED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE_def MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE = new MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE_def();
            public class MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ_def MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ = new MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ_def();
            public class MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_ELIGIBILITY_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT_def MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT = new MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT_def();
            public class MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CRITERIA_ADDED_UPDATED;

                public MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STORE_ELIGIBILITY_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CRITERIA_ADDED_UPDATED = new intParameter("@CRITERIA_ADDED_UPDATED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CRITERIA_ADDED_UPDATED
                                  )
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CRITERIA_ADDED_UPDATED.SetValue(CRITERIA_ADDED_UPDATED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE_def MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE = new MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE_def();
            public class MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_ELIGIBILITY_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_VSW_CRITERIA_LOAD_INFO_READ_def MID_VSW_CRITERIA_LOAD_INFO_READ = new MID_VSW_CRITERIA_LOAD_INFO_READ_def();
            public class MID_VSW_CRITERIA_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_CRITERIA_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_VSW_CRITERIA_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_VSW_CRITERIA_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VSW_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_VSW_CRITERIA_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_VSW_CRITERIA_LOAD_INFO_INSERT_def MID_VSW_CRITERIA_LOAD_INFO_INSERT = new MID_VSW_CRITERIA_LOAD_INFO_INSERT_def();
            public class MID_VSW_CRITERIA_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_CRITERIA_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CRITERIA_ADDED_UPDATED;

                public MID_VSW_CRITERIA_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_VSW_CRITERIA_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("VSW_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CRITERIA_ADDED_UPDATED = new intParameter("@CRITERIA_ADDED_UPDATED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CRITERIA_ADDED_UPDATED
                                  )
                {
                    lock (typeof(MID_VSW_CRITERIA_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CRITERIA_ADDED_UPDATED.SetValue(CRITERIA_ADDED_UPDATED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_VSW_CRITERIA_LOAD_INFO_DELETE_def MID_VSW_CRITERIA_LOAD_INFO_DELETE = new MID_VSW_CRITERIA_LOAD_INFO_DELETE_def();
            public class MID_VSW_CRITERIA_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_CRITERIA_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_VSW_CRITERIA_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_VSW_CRITERIA_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("VSW_CRITERIA_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_VSW_CRITERIA_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_LOAD_INFO_READ_def MID_COLOR_CODE_LOAD_INFO_READ = new MID_COLOR_CODE_LOAD_INFO_READ_def();
            public class MID_COLOR_CODE_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_COLOR_CODE_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_COLOR_CODE_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_COLOR_CODE_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_LOAD_INFO_INSERT_def MID_COLOR_CODE_LOAD_INFO_INSERT = new MID_COLOR_CODE_LOAD_INFO_INSERT_def();
            public class MID_COLOR_CODE_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CODES_CREATED;
                private intParameter CODES_MODIFIED;

                public MID_COLOR_CODE_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_COLOR_CODE_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("COLOR_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CODES_CREATED = new intParameter("@CODES_CREATED", base.inputParameterList);
                    CODES_MODIFIED = new intParameter("@CODES_MODIFIED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CODES_CREATED,
                                  int? CODES_MODIFIED
                                  )
                {
                    lock (typeof(MID_COLOR_CODE_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CODES_CREATED.SetValue(CODES_CREATED);
                        this.CODES_MODIFIED.SetValue(CODES_MODIFIED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_LOAD_INFO_DELETE_def MID_COLOR_CODE_LOAD_INFO_DELETE = new MID_COLOR_CODE_LOAD_INFO_DELETE_def();
            public class MID_COLOR_CODE_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_COLOR_CODE_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_COLOR_CODE_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("COLOR_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_COLOR_CODE_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SIZE_CONSTRAINT_LOAD_INFO_READ_def MID_SIZE_CONSTRAINT_LOAD_INFO_READ = new MID_SIZE_CONSTRAINT_LOAD_INFO_READ_def();
            public class MID_SIZE_CONSTRAINT_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CONSTRAINT_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_SIZE_CONSTRAINT_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_CONSTRAINT_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CONSTRAINT_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_CODE_LOAD_INFO_READ_def MID_SIZE_CODE_LOAD_INFO_READ = new MID_SIZE_CODE_LOAD_INFO_READ_def();
            public class MID_SIZE_CODE_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CODE_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_SIZE_CODE_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CODE_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT_def MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT = new MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT_def();
            public class MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter MODELS_CREATED;
                private intParameter MODELS_MODIFIED;
                private intParameter MODELS_REMOVED;

                public MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SIZE_CONSTRAINT_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    MODELS_CREATED = new intParameter("@MODELS_CREATED", base.inputParameterList);
                    MODELS_MODIFIED = new intParameter("@MODELS_MODIFIED", base.inputParameterList);
                    MODELS_REMOVED = new intParameter("@MODELS_REMOVED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? MODELS_CREATED,
                                  int? MODELS_MODIFIED,
                                  int? MODELS_REMOVED
                                  )
                {
                    lock (typeof(MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.MODELS_CREATED.SetValue(MODELS_CREATED);
                        this.MODELS_MODIFIED.SetValue(MODELS_MODIFIED);
                        this.MODELS_REMOVED.SetValue(MODELS_REMOVED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIZE_CODE_LOAD_INFO_INSERT_def MID_SIZE_CODE_LOAD_INFO_INSERT = new MID_SIZE_CODE_LOAD_INFO_INSERT_def();
            public class MID_SIZE_CODE_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_WITH_ERRORS;
                private intParameter CODES_CREATED;
                private intParameter CODES_MODIFIED;

                public MID_SIZE_CODE_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_SIZE_CODE_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SIZE_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                    CODES_CREATED = new intParameter("@CODES_CREATED", base.inputParameterList);
                    CODES_MODIFIED = new intParameter("@CODES_MODIFIED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_WITH_ERRORS,
                                  int? CODES_CREATED,
                                  int? CODES_MODIFIED
                                  )
                {
                    lock (typeof(MID_SIZE_CODE_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        this.CODES_CREATED.SetValue(CODES_CREATED);
                        this.CODES_MODIFIED.SetValue(CODES_MODIFIED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE_def MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE = new MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE_def();
            public class MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_CONSTRAINT_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SIZE_CODE_LOAD_INFO_DELETE_def MID_SIZE_CODE_LOAD_INFO_DELETE = new MID_SIZE_CODE_LOAD_INFO_DELETE_def();
            public class MID_SIZE_CODE_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CODE_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_SIZE_CODE_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_CODE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CODE_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_GENERATE_INFO_READ_def MID_SIZE_CURVE_GENERATE_INFO_READ = new MID_SIZE_CURVE_GENERATE_INFO_READ_def();
            public class MID_SIZE_CURVE_GENERATE_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GENERATE_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CURVE_GENERATE_INFO_READ_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_GENERATE_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_CURVE_GENERATE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CURVE_GENERATE_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_GENERATE_INFO_INSERT_def MID_SIZE_CURVE_GENERATE_INFO_INSERT = new MID_SIZE_CURVE_GENERATE_INFO_INSERT_def();
            public class MID_SIZE_CURVE_GENERATE_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GENERATE_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter MTHDS_EXECUTED;
                private intParameter MTHDS_SUCCESSFUL;
                private intParameter MTHDS_FAILED;
                private intParameter MTHDS_NO_ACTION;

                public MID_SIZE_CURVE_GENERATE_INFO_INSERT_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_GENERATE_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SIZE_CURVE_GENERATE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    MTHDS_EXECUTED = new intParameter("@MTHDS_EXECUTED", base.inputParameterList);
                    MTHDS_SUCCESSFUL = new intParameter("@MTHDS_SUCCESSFUL", base.inputParameterList);
                    MTHDS_FAILED = new intParameter("@MTHDS_FAILED", base.inputParameterList);
                    MTHDS_NO_ACTION = new intParameter("@MTHDS_NO_ACTION", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? MTHDS_EXECUTED,
                                  int? MTHDS_SUCCESSFUL,
                                  int? MTHDS_FAILED,
                                  int? MTHDS_NO_ACTION
                                  )
                {
                    lock (typeof(MID_SIZE_CURVE_GENERATE_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.MTHDS_EXECUTED.SetValue(MTHDS_EXECUTED);
                        this.MTHDS_SUCCESSFUL.SetValue(MTHDS_SUCCESSFUL);
                        this.MTHDS_FAILED.SetValue(MTHDS_FAILED);
                        this.MTHDS_NO_ACTION.SetValue(MTHDS_NO_ACTION);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_GENERATE_INFO_DELETE_def MID_SIZE_CURVE_GENERATE_INFO_DELETE = new MID_SIZE_CURVE_GENERATE_INFO_DELETE_def();
            public class MID_SIZE_CURVE_GENERATE_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GENERATE_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CURVE_GENERATE_INFO_DELETE_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_GENERATE_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_CURVE_GENERATE_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CURVE_GENERATE_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_ROLLUP_INFO_READ_def MID_ROLLUP_INFO_READ = new MID_ROLLUP_INFO_READ_def();
            public class MID_ROLLUP_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_ROLLUP_INFO_READ_def()
                {
                    base.procedureName = "MID_ROLLUP_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("ROLLUP_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_ROLLUP_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_ROLLUP_INFO_INSERT_def MID_ROLLUP_INFO_INSERT = new MID_ROLLUP_INFO_INSERT_def();
            public class MID_ROLLUP_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter TOTAL_ITEMS;
                private intParameter BATCH_SIZE;
                private intParameter CONCURRENT_PROCESSES;
                private intParameter TOTAL_BATCHES;
                private intParameter RECS_WITH_ERRORS;

                public MID_ROLLUP_INFO_INSERT_def()
                {
                    base.procedureName = "MID_ROLLUP_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("ROLLUP_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    TOTAL_ITEMS = new intParameter("@TOTAL_ITEMS", base.inputParameterList);
                    BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
                    CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
                    TOTAL_BATCHES = new intParameter("@TOTAL_BATCHES", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? TOTAL_ITEMS,
                                  int? BATCH_SIZE,
                                  int? CONCURRENT_PROCESSES,
                                  int? TOTAL_BATCHES,
                                  int? RECS_WITH_ERRORS
                                  )
                {
                    lock (typeof(MID_ROLLUP_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.TOTAL_ITEMS.SetValue(TOTAL_ITEMS);
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.TOTAL_BATCHES.SetValue(TOTAL_BATCHES);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_ROLLUP_INFO_DELETE_def MID_ROLLUP_INFO_DELETE = new MID_ROLLUP_INFO_DELETE_def();
            public class MID_ROLLUP_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_ROLLUP_INFO_DELETE_def()
                {
                    base.procedureName = "MID_ROLLUP_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("ROLLUP_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_ROLLUP_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_ROLLUP_PROCESS_DELETE_def MID_ROLLUP_PROCESS_DELETE = new MID_ROLLUP_PROCESS_DELETE_def();
            public class MID_ROLLUP_PROCESS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_PROCESS_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_ROLLUP_PROCESS_DELETE_def()
                {
                    base.procedureName = "MID_ROLLUP_PROCESS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("ROLLUP_PROCESS");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_ROLLUP_PROCESS_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_COMPUTATION_DRIVER_INFO_READ_def MID_COMPUTATION_DRIVER_INFO_READ = new MID_COMPUTATION_DRIVER_INFO_READ_def();
            public class MID_COMPUTATION_DRIVER_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_DRIVER_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_COMPUTATION_DRIVER_INFO_READ_def()
                {
                    base.procedureName = "MID_COMPUTATION_DRIVER_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COMPUTATION_DRIVER_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_COMPUTATION_DRIVER_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COMPUTATION_DRIVER_INFO_INSERT_def MID_COMPUTATION_DRIVER_INFO_INSERT = new MID_COMPUTATION_DRIVER_INFO_INSERT_def();
            public class MID_COMPUTATION_DRIVER_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_DRIVER_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter TOTAL_ITEMS;
                private intParameter CONCURRENT_PROCESSES;
                private intParameter RECS_WITH_ERRORS;

                public MID_COMPUTATION_DRIVER_INFO_INSERT_def()
                {
                    base.procedureName = "MID_COMPUTATION_DRIVER_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("COMPUTATION_DRIVER_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    TOTAL_ITEMS = new intParameter("@TOTAL_ITEMS", base.inputParameterList);
                    CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? TOTAL_ITEMS,
                                  int? CONCURRENT_PROCESSES,
                                  int? RECS_WITH_ERRORS
                                  )
                {
                    lock (typeof(MID_COMPUTATION_DRIVER_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.TOTAL_ITEMS.SetValue(TOTAL_ITEMS);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_COMPUTATION_DRIVER_INFO_DELETE_def MID_COMPUTATION_DRIVER_INFO_DELETE = new MID_COMPUTATION_DRIVER_INFO_DELETE_def();
            public class MID_COMPUTATION_DRIVER_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_DRIVER_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_COMPUTATION_DRIVER_INFO_DELETE_def()
                {
                    base.procedureName = "MID_COMPUTATION_DRIVER_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("COMPUTATION_DRIVER_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_COMPUTATION_DRIVER_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_COMPUTATION_PROCESS_DELETE_def MID_COMPUTATION_PROCESS_DELETE = new MID_COMPUTATION_PROCESS_DELETE_def();
            public class MID_COMPUTATION_PROCESS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_PROCESS_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_COMPUTATION_PROCESS_DELETE_def()
                {
                    base.procedureName = "MID_COMPUTATION_PROCESS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("COMPUTATION_PROCESS");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_COMPUTATION_PROCESS_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ_def MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ = new MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ_def();
            public class MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ_def()
                {
                    base.procedureName = "MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("AUDIT_RELIEVE_INTRANSIT_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT_def MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT = new MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT_def();
            public class MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter RECS_READ;
                private intParameter RECS_ACCEPTED;
                private intParameter RECS_WITH_ERRORS;

                public MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT_def()
                {
                    base.procedureName = "MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_RELIEVE_INTRANSIT_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECS_READ = new intParameter("@RECS_READ", base.inputParameterList);
                    RECS_ACCEPTED = new intParameter("@RECS_ACCEPTED", base.inputParameterList);
                    RECS_WITH_ERRORS = new intParameter("@RECS_WITH_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? RECS_READ,
                                  int? RECS_ACCEPTED,
                                  int? RECS_WITH_ERRORS
                                  )
                {
                    lock (typeof(MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECS_READ.SetValue(RECS_READ);
                        this.RECS_ACCEPTED.SetValue(RECS_ACCEPTED);
                        this.RECS_WITH_ERRORS.SetValue(RECS_WITH_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE_def MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE = new MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE_def();
            public class MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE_def()
                {
                    base.procedureName = "MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("AUDIT_RELIEVE_INTRANSIT_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_RECLASS_AUDIT_INSERT_def SP_MID_RECLASS_AUDIT_INSERT = new SP_MID_RECLASS_AUDIT_INSERT_def();
            public class SP_MID_RECLASS_AUDIT_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_RECLASS_AUDIT_INSERT.SQL"

                private intParameter PROCESS_RID;
                private stringParameter RECLASS_ACTION;
                private stringParameter RECLASS_ITEM_TYPE;
                private stringParameter RECLASS_ITEM;
                private stringParameter RECLASS_COMMENT;

                public SP_MID_RECLASS_AUDIT_INSERT_def()
                {
                    base.procedureName = "SP_MID_RECLASS_AUDIT_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_RECLASS");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    RECLASS_ACTION = new stringParameter("@RECLASS_ACTION", base.inputParameterList);
                    RECLASS_ITEM_TYPE = new stringParameter("@RECLASS_ITEM_TYPE", base.inputParameterList);
                    RECLASS_ITEM = new stringParameter("@RECLASS_ITEM", base.inputParameterList);
                    RECLASS_COMMENT = new stringParameter("@RECLASS_COMMENT", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  string RECLASS_ACTION,
                                  string RECLASS_ITEM_TYPE,
                                  string RECLASS_ITEM,
                                  string RECLASS_COMMENT
                                  )
                {
                    lock (typeof(SP_MID_RECLASS_AUDIT_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.RECLASS_ACTION.SetValue(RECLASS_ACTION);
                        this.RECLASS_ITEM_TYPE.SetValue(RECLASS_ITEM_TYPE);
                        this.RECLASS_ITEM.SetValue(RECLASS_ITEM);
                        this.RECLASS_COMMENT.SetValue(RECLASS_COMMENT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_AUDIT_RECLASS_DELETE_def MID_AUDIT_RECLASS_DELETE = new MID_AUDIT_RECLASS_DELETE_def();
            public class MID_AUDIT_RECLASS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_RECLASS_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_AUDIT_RECLASS_DELETE_def()
                {
                    base.procedureName = "MID_AUDIT_RECLASS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("AUDIT_RECLASS");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_AUDIT_RECLASS_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_AUDIT_RECLASS_READ_ALL_def MID_AUDIT_RECLASS_READ_ALL = new MID_AUDIT_RECLASS_READ_ALL_def();
            public class MID_AUDIT_RECLASS_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_RECLASS_READ_ALL.SQL"

                public MID_AUDIT_RECLASS_READ_ALL_def()
                {
                    base.procedureName = "MID_AUDIT_RECLASS_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("AUDIT_RECLASS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_AUDIT_RECLASS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_AUDIT_HEADER_INSERT_def SP_MID_AUDIT_HEADER_INSERT = new SP_MID_AUDIT_HEADER_INSERT_def();
            public class SP_MID_AUDIT_HEADER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_AUDIT_HEADER_INSERT.SQL"

                private datetimeParameter PROCESS_DATE_TIME;
                private intParameter PROCESS_RID;
                private intParameter USER_RID;
                private intParameter HDR_RID;
                private intParameter METHOD_RID;
                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE;
                private intParameter HEADER_COMPONENT_TYPE;
                private intParameter ACTION_TYPE;
                private stringParameter PACK_OR_COLOR_NAME;
                private stringParameter SIZE_NAME;
                private intParameter UNITS_ALLOCATED_BY_PROCESS;
                private intParameter STORE_COUNT;
                private intParameter AUDIT_HEADER_RID; //Declare Output Parameter

                public SP_MID_AUDIT_HEADER_INSERT_def()
                {
                    base.procedureName = "SP_MID_AUDIT_HEADER_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("AUDIT_HEADER");
                    PROCESS_DATE_TIME = new datetimeParameter("@PROCESS_DATE_TIME", base.inputParameterList);
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
                    METHOD_TYPE = new intParameter("@METHOD_TYPE", base.inputParameterList);
                    HEADER_COMPONENT_TYPE = new intParameter("@HEADER_COMPONENT_TYPE", base.inputParameterList);
                    ACTION_TYPE = new intParameter("@ACTION_TYPE", base.inputParameterList);
                    PACK_OR_COLOR_NAME = new stringParameter("@PACK_OR_COLOR_NAME", base.inputParameterList);
                    SIZE_NAME = new stringParameter("@SIZE_NAME", base.inputParameterList);
                    UNITS_ALLOCATED_BY_PROCESS = new intParameter("@UNITS_ALLOCATED_BY_PROCESS", base.inputParameterList);
                    STORE_COUNT = new intParameter("@STORE_COUNT", base.inputParameterList);

                    AUDIT_HEADER_RID = new intParameter("@AUDIT_HEADER_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              DateTime? PROCESS_DATE_TIME,
                                              int? PROCESS_RID,
                                              int? USER_RID,
                                              int? HDR_RID,
                                              int? METHOD_RID,
                                              string METHOD_NAME,
                                              int? METHOD_TYPE,
                                              int? HEADER_COMPONENT_TYPE,
                                              int? ACTION_TYPE,
                                              string PACK_OR_COLOR_NAME,
                                              string SIZE_NAME,
                                              int? UNITS_ALLOCATED_BY_PROCESS,
                                              int? STORE_COUNT
                                              )
                {
                    lock (typeof(SP_MID_AUDIT_HEADER_INSERT_def))
                    {
                        this.PROCESS_DATE_TIME.SetValue(PROCESS_DATE_TIME);
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE.SetValue(METHOD_TYPE);
                        this.HEADER_COMPONENT_TYPE.SetValue(HEADER_COMPONENT_TYPE);
                        this.ACTION_TYPE.SetValue(ACTION_TYPE);
                        this.PACK_OR_COLOR_NAME.SetValue(PACK_OR_COLOR_NAME);
                        this.SIZE_NAME.SetValue(SIZE_NAME);
                        this.UNITS_ALLOCATED_BY_PROCESS.SetValue(UNITS_ALLOCATED_BY_PROCESS);
                        this.STORE_COUNT.SetValue(STORE_COUNT);
                        this.AUDIT_HEADER_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE.SQL"

                private intParameter HDR_RID;
                private intParameter COMPONENT_TYPE1;
                private intParameter COMPONENT_TYPE2;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    COMPONENT_TYPE1 = new intParameter("@COMPONENT_TYPE1", base.inputParameterList);
                    COMPONENT_TYPE2 = new intParameter("@COMPONENT_TYPE2", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  int? COMPONENT_TYPE1,
                                  int? COMPONENT_TYPE2
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.COMPONENT_TYPE1.SetValue(COMPONENT_TYPE1);
                        this.COMPONENT_TYPE2.SetValue(COMPONENT_TYPE2);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR.SQL"

                private intParameter HDR_RID;
                private intParameter HEADER_COMPONENT_TYPE;
                private stringParameter PACK_OR_COLOR_NAME;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HEADER_COMPONENT_TYPE = new intParameter("@HEADER_COMPONENT_TYPE", base.inputParameterList);
                    PACK_OR_COLOR_NAME = new stringParameter("@PACK_OR_COLOR_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  int? HEADER_COMPONENT_TYPE,
                                  string PACK_OR_COLOR_NAME
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HEADER_COMPONENT_TYPE.SetValue(HEADER_COMPONENT_TYPE);
                        this.PACK_OR_COLOR_NAME.SetValue(PACK_OR_COLOR_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE.SQL"

                private intParameter HDR_RID;
                private intParameter HEADER_COMPONENT_TYPE;
                private stringParameter PACK_OR_COLOR_NAME;
                private stringParameter SIZE_NAME;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HEADER_COMPONENT_TYPE = new intParameter("@HEADER_COMPONENT_TYPE", base.inputParameterList);
                    PACK_OR_COLOR_NAME = new stringParameter("@PACK_OR_COLOR_NAME", base.inputParameterList);
                    SIZE_NAME = new stringParameter("@SIZE_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  int? HEADER_COMPONENT_TYPE,
                                  string PACK_OR_COLOR_NAME,
                                  string SIZE_NAME
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HEADER_COMPONENT_TYPE.SetValue(HEADER_COMPONENT_TYPE);
                        this.PACK_OR_COLOR_NAME.SetValue(PACK_OR_COLOR_NAME);
                        this.SIZE_NAME.SetValue(SIZE_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY.SQL"

                private intParameter HDR_RID;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE.SQL"

                private intParameter HDR_RID;
                private tableParameter ACTION_OR_METHOD_LIST;
                private intParameter COMPONENT_TYPE1;
                private intParameter COMPONENT_TYPE2;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    ACTION_OR_METHOD_LIST = new tableParameter("@ACTION_OR_METHOD_LIST", "ACTION_OR_METHOD_TYPE", base.inputParameterList);
                    COMPONENT_TYPE1 = new intParameter("@COMPONENT_TYPE1", base.inputParameterList);
                    COMPONENT_TYPE2 = new intParameter("@COMPONENT_TYPE2", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  DataTable ACTION_OR_METHOD_LIST,
                                  int? COMPONENT_TYPE1,
                                  int? COMPONENT_TYPE2
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ACTION_OR_METHOD_LIST.SetValue(ACTION_OR_METHOD_LIST);
                        this.COMPONENT_TYPE1.SetValue(COMPONENT_TYPE1);
                        this.COMPONENT_TYPE2.SetValue(COMPONENT_TYPE2);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_AUDIT_FORECAST_INSERT_def SP_MID_AUDIT_FORECAST_INSERT = new SP_MID_AUDIT_FORECAST_INSERT_def();
            public class SP_MID_AUDIT_FORECAST_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_AUDIT_FORECAST_INSERT.SQL"

                private datetimeParameter PROCESS_DATE_TIME;
                private intParameter PROCESS_RID;
                private intParameter USER_RID;
                private intParameter HN_RID;
                private intParameter METHOD_RID;
                private stringParameter METHOD_NAME;
                private intParameter METHOD_TYPE;
                private intParameter STORE_FV_RID;
                private intParameter CHAIN_FV_RID;
                private charParameter TIME_RANGE_TYPE;
                private stringParameter TIME_RANGE_NAME;
                private stringParameter TIME_RANGE_DISPLAY;
                private intParameter TIME_RANGE_BEGIN;
                private intParameter TIME_RANGE_END;
                private intParameter SG_RID;
                private stringParameter ATTRIBUTE_NAME;
                private stringParameter HN_TEXT;
                private intParameter AUDIT_FORECAST_RID; //Declare Output Parameter

                public SP_MID_AUDIT_FORECAST_INSERT_def()
                {
                    base.procedureName = "SP_MID_AUDIT_FORECAST_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("AUDIT_FORECAST");
                    PROCESS_DATE_TIME = new datetimeParameter("@PROCESS_DATE_TIME", base.inputParameterList);
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    METHOD_NAME = new stringParameter("@METHOD_NAME", base.inputParameterList);
                    METHOD_TYPE = new intParameter("@METHOD_TYPE", base.inputParameterList);
                    STORE_FV_RID = new intParameter("@STORE_FV_RID", base.inputParameterList);
                    CHAIN_FV_RID = new intParameter("@CHAIN_FV_RID", base.inputParameterList);
                    TIME_RANGE_TYPE = new charParameter("@TIME_RANGE_TYPE", base.inputParameterList);
                    TIME_RANGE_NAME = new stringParameter("@TIME_RANGE_NAME", base.inputParameterList);
                    TIME_RANGE_DISPLAY = new stringParameter("@TIME_RANGE_DISPLAY", base.inputParameterList);
                    TIME_RANGE_BEGIN = new intParameter("@TIME_RANGE_BEGIN", base.inputParameterList);
                    TIME_RANGE_END = new intParameter("@TIME_RANGE_END", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    ATTRIBUTE_NAME = new stringParameter("@ATTRIBUTE_NAME", base.inputParameterList);
                    HN_TEXT = new stringParameter("@HN_TEXT", base.inputParameterList);

                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              DateTime? PROCESS_DATE_TIME,
                                              int? PROCESS_RID,
                                              int? USER_RID,
                                              int? HN_RID,
                                              int? METHOD_RID,
                                              string METHOD_NAME,
                                              int? METHOD_TYPE,
                                              int? STORE_FV_RID,
                                              int? CHAIN_FV_RID,
                                              char? TIME_RANGE_TYPE,
                                              string TIME_RANGE_NAME,
                                              string TIME_RANGE_DISPLAY,
                                              int? TIME_RANGE_BEGIN,
                                              int? TIME_RANGE_END,
                                              int? SG_RID,
                                              string ATTRIBUTE_NAME,
                                              string HN_TEXT
                                              )
                {
                    lock (typeof(SP_MID_AUDIT_FORECAST_INSERT_def))
                    {
                        this.PROCESS_DATE_TIME.SetValue(PROCESS_DATE_TIME);
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.METHOD_NAME.SetValue(METHOD_NAME);
                        this.METHOD_TYPE.SetValue(METHOD_TYPE);
                        this.STORE_FV_RID.SetValue(STORE_FV_RID);
                        this.CHAIN_FV_RID.SetValue(CHAIN_FV_RID);
                        this.TIME_RANGE_TYPE.SetValue(TIME_RANGE_TYPE);
                        this.TIME_RANGE_NAME.SetValue(TIME_RANGE_NAME);
                        this.TIME_RANGE_DISPLAY.SetValue(TIME_RANGE_DISPLAY);
                        this.TIME_RANGE_BEGIN.SetValue(TIME_RANGE_BEGIN);
                        this.TIME_RANGE_END.SetValue(TIME_RANGE_END);
                        this.SG_RID.SetValue(SG_RID);
                        this.ATTRIBUTE_NAME.SetValue(ATTRIBUTE_NAME);
                        this.HN_TEXT.SetValue(HN_TEXT);
                        this.AUDIT_FORECAST_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR.SQL"

                private intParameter HDR_RID;
                private tableParameter ACTION_OR_METHOD_LIST;
                private intParameter COMPONENT_TYPE;
                private stringParameter PACK_OR_COLOR_NAME;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    ACTION_OR_METHOD_LIST = new tableParameter("@ACTION_OR_METHOD_LIST", "ACTION_OR_METHOD_TYPE", base.inputParameterList);
                    COMPONENT_TYPE = new intParameter("@COMPONENT_TYPE", base.inputParameterList);
                    PACK_OR_COLOR_NAME = new stringParameter("@PACK_OR_COLOR_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, int? HDR_RID,
                                  DataTable ACTION_OR_METHOD_LIST,
                                  int? COMPONENT_TYPE,
                                  string PACK_OR_COLOR_NAME
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ACTION_OR_METHOD_LIST.SetValue(ACTION_OR_METHOD_LIST);
                        this.COMPONENT_TYPE.SetValue(COMPONENT_TYPE);
                        this.PACK_OR_COLOR_NAME.SetValue(PACK_OR_COLOR_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE.SQL"

                private intParameter HDR_RID;
                private tableParameter ACTION_OR_METHOD_LIST;
                private intParameter COMPONENT_TYPE;
                private stringParameter PACK_OR_COLOR_NAME;
                private stringParameter SIZE_NAME;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    ACTION_OR_METHOD_LIST = new tableParameter("@ACTION_OR_METHOD_LIST", "ACTION_OR_METHOD_TYPE", base.inputParameterList);
                    COMPONENT_TYPE = new intParameter("@COMPONENT_TYPE", base.inputParameterList);
                    PACK_OR_COLOR_NAME = new stringParameter("@PACK_OR_COLOR_NAME", base.inputParameterList);
                    SIZE_NAME = new stringParameter("@SIZE_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, int? HDR_RID,
                                  DataTable ACTION_OR_METHOD_LIST,
                                  int? COMPONENT_TYPE,
                                  string PACK_OR_COLOR_NAME,
                                  string SIZE_NAME
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ACTION_OR_METHOD_LIST.SetValue(ACTION_OR_METHOD_LIST);
                        this.COMPONENT_TYPE.SetValue(COMPONENT_TYPE);
                        this.PACK_OR_COLOR_NAME.SetValue(PACK_OR_COLOR_NAME);
                        this.SIZE_NAME.SetValue(SIZE_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_def MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION = new MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_def();
            public class MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION.SQL"

                private intParameter HDR_RID;
                private tableParameter ACTION_OR_METHOD_LIST;

                public MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_def()
                {
                    base.procedureName = "MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("AUDIT_HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    ACTION_OR_METHOD_LIST = new tableParameter("@ACTION_OR_METHOD_LIST", "ACTION_OR_METHOD_TYPE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  DataTable ACTION_OR_METHOD_LIST
                                  )
                {
                    lock (typeof(MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ACTION_OR_METHOD_LIST.SetValue(ACTION_OR_METHOD_LIST);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_AUDIT_FORECAST_DELETE_def SP_MID_AUDIT_FORECAST_DELETE = new SP_MID_AUDIT_FORECAST_DELETE_def();
            public class SP_MID_AUDIT_FORECAST_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_AUDIT_FORECAST_DELETE.SQL"

                private intParameter AUDIT_FORECAST_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures

                public SP_MID_AUDIT_FORECAST_DELETE_def()
                {
                    base.procedureName = "SP_MID_AUDIT_FORECAST_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("AUDIT_FORECAST");
                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                }

                public int Delete(DatabaseAccess _dba, int? AUDIT_FORECAST_RID)
                {
                    lock (typeof(SP_MID_AUDIT_FORECAST_DELETE_def))
                    {
                        this.AUDIT_FORECAST_RID.SetValue(AUDIT_FORECAST_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_AUDIT_FORECAST_READ_LAST_PROCESSED_def MID_AUDIT_FORECAST_READ_LAST_PROCESSED = new MID_AUDIT_FORECAST_READ_LAST_PROCESSED_def();
            public class MID_AUDIT_FORECAST_READ_LAST_PROCESSED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_FORECAST_READ_LAST_PROCESSED.SQL"

                private intParameter METHOD_RID;

                public MID_AUDIT_FORECAST_READ_LAST_PROCESSED_def()
                {
                    base.procedureName = "MID_AUDIT_FORECAST_READ_LAST_PROCESSED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("AUDIT_FORECAST");
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
                {
                    lock (typeof(MID_AUDIT_FORECAST_READ_LAST_PROCESSED_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_AUDIT_OTS_FORECAST_SET_INSERT_def MID_AUDIT_OTS_FORECAST_SET_INSERT = new MID_AUDIT_OTS_FORECAST_SET_INSERT_def();
            public class MID_AUDIT_OTS_FORECAST_SET_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_OTS_FORECAST_SET_INSERT.SQL"

                private intParameter AUDIT_FORECAST_RID;
                private intParameter SGL_RID;
                private stringParameter SET_NAME;
                private stringParameter FORECAST_METHOD_TYPE;
                private charParameter STOCK_MIN_MAX;

                public MID_AUDIT_OTS_FORECAST_SET_INSERT_def()
                {
                    base.procedureName = "MID_AUDIT_OTS_FORECAST_SET_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_OTS_FORECAST_SET");
                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    SET_NAME = new stringParameter("@SET_NAME", base.inputParameterList);
                    FORECAST_METHOD_TYPE = new stringParameter("@FORECAST_METHOD_TYPE", base.inputParameterList);
                    STOCK_MIN_MAX = new charParameter("@STOCK_MIN_MAX", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? AUDIT_FORECAST_RID,
                                  int? SGL_RID,
                                  string SET_NAME,
                                  string FORECAST_METHOD_TYPE,
                                  char? STOCK_MIN_MAX
                                  )
                {
                    lock (typeof(MID_AUDIT_OTS_FORECAST_SET_INSERT_def))
                    {
                        this.AUDIT_FORECAST_RID.SetValue(AUDIT_FORECAST_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.SET_NAME.SetValue(SET_NAME);
                        this.FORECAST_METHOD_TYPE.SetValue(FORECAST_METHOD_TYPE);
                        this.STOCK_MIN_MAX.SetValue(STOCK_MIN_MAX);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT_def MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT = new MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT_def();
            public class MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT.SQL"

                private intParameter AUDIT_FORECAST_RID;
                private intParameter SGL_RID;
                private intParameter BASIS_SEQUENCE;
                private intParameter BASIS_HN_RID;
                private stringParameter BASIS_HN_TEXT;
                private intParameter BASIS_FV_RID;
                private stringParameter BASIS_TIME_PERIOD;
                private floatParameter BASIS_WEIGHT;
                private intParameter BASIS_TYPE_SORT_CODE;
                private stringParameter BASIS_TYPE;

                public MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT_def()
                {
                    base.procedureName = "MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_OTS_FORECAST_SET_BASIS");
                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    BASIS_SEQUENCE = new intParameter("@BASIS_SEQUENCE", base.inputParameterList);
                    BASIS_HN_RID = new intParameter("@BASIS_HN_RID", base.inputParameterList);
                    BASIS_HN_TEXT = new stringParameter("@BASIS_HN_TEXT", base.inputParameterList);
                    BASIS_FV_RID = new intParameter("@BASIS_FV_RID", base.inputParameterList);
                    BASIS_TIME_PERIOD = new stringParameter("@BASIS_TIME_PERIOD", base.inputParameterList);
                    BASIS_WEIGHT = new floatParameter("@BASIS_WEIGHT", base.inputParameterList);
                    BASIS_TYPE_SORT_CODE = new intParameter("@BASIS_TYPE_SORT_CODE", base.inputParameterList);
                    BASIS_TYPE = new stringParameter("@BASIS_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? AUDIT_FORECAST_RID,
                                  int? SGL_RID,
                                  int? BASIS_SEQUENCE,
                                  int? BASIS_HN_RID,
                                  string BASIS_HN_TEXT,
                                  int? BASIS_FV_RID,
                                  string BASIS_TIME_PERIOD,
                                  double? BASIS_WEIGHT,
                                  int? BASIS_TYPE_SORT_CODE,
                                  string BASIS_TYPE
                                  )
                {
                    lock (typeof(MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT_def))
                    {
                        this.AUDIT_FORECAST_RID.SetValue(AUDIT_FORECAST_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BASIS_SEQUENCE.SetValue(BASIS_SEQUENCE);
                        this.BASIS_HN_RID.SetValue(BASIS_HN_RID);
                        this.BASIS_HN_TEXT.SetValue(BASIS_HN_TEXT);
                        this.BASIS_FV_RID.SetValue(BASIS_FV_RID);
                        this.BASIS_TIME_PERIOD.SetValue(BASIS_TIME_PERIOD);
                        this.BASIS_WEIGHT.SetValue(BASIS_WEIGHT);
                        this.BASIS_TYPE_SORT_CODE.SetValue(BASIS_TYPE_SORT_CODE);
                        this.BASIS_TYPE.SetValue(BASIS_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_AUDIT_MODIFY_SALES_INSERT_def MID_AUDIT_MODIFY_SALES_INSERT = new MID_AUDIT_MODIFY_SALES_INSERT_def();
            public class MID_AUDIT_MODIFY_SALES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_MODIFY_SALES_INSERT.SQL"

                private intParameter AUDIT_FORECAST_RID;
                private stringParameter FILTER_NAME;
                private stringParameter AVERAGE_BY;

                public MID_AUDIT_MODIFY_SALES_INSERT_def()
                {
                    base.procedureName = "MID_AUDIT_MODIFY_SALES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_MODIFY_SALES");
                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.inputParameterList);
                    FILTER_NAME = new stringParameter("@FILTER_NAME", base.inputParameterList);
                    AVERAGE_BY = new stringParameter("@AVERAGE_BY", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? AUDIT_FORECAST_RID,
                                  string FILTER_NAME,
                                  string AVERAGE_BY
                                  )
                {
                    lock (typeof(MID_AUDIT_MODIFY_SALES_INSERT_def))
                    {
                        this.AUDIT_FORECAST_RID.SetValue(AUDIT_FORECAST_RID);
                        this.FILTER_NAME.SetValue(FILTER_NAME);
                        this.AVERAGE_BY.SetValue(AVERAGE_BY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_AUDIT_MODIFY_SALES_MATRIX_INSERT_def MID_AUDIT_MODIFY_SALES_MATRIX_INSERT = new MID_AUDIT_MODIFY_SALES_MATRIX_INSERT_def();
            public class MID_AUDIT_MODIFY_SALES_MATRIX_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_MODIFY_SALES_MATRIX_INSERT.SQL"

                private intParameter AUDIT_FORECAST_RID;
                private intParameter SGL_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;
                private intParameter SELL_THRU;
                private intParameter NUMBER_OF_STORES;
                private intParameter MATRIX_RULE;
                private floatParameter MATRIX_RULE_QUANTITY;

                public MID_AUDIT_MODIFY_SALES_MATRIX_INSERT_def()
                {
                    base.procedureName = "MID_AUDIT_MODIFY_SALES_MATRIX_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("AUDIT_MODIFY_SALES_MATRIX");
                    AUDIT_FORECAST_RID = new intParameter("@AUDIT_FORECAST_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                    SELL_THRU = new intParameter("@SELL_THRU", base.inputParameterList);
                    NUMBER_OF_STORES = new intParameter("@NUMBER_OF_STORES", base.inputParameterList);
                    MATRIX_RULE = new intParameter("@MATRIX_RULE", base.inputParameterList);
                    MATRIX_RULE_QUANTITY = new floatParameter("@MATRIX_RULE_QUANTITY", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? AUDIT_FORECAST_RID,
                                  int? SGL_RID,
                                  int? BOUNDARY,
                                  string GRADE_CODE,
                                  int? SELL_THRU,
                                  int? NUMBER_OF_STORES,
                                  int? MATRIX_RULE,
                                  double? MATRIX_RULE_QUANTITY
                                  )
                {
                    lock (typeof(MID_AUDIT_MODIFY_SALES_MATRIX_INSERT_def))
                    {
                        this.AUDIT_FORECAST_RID.SetValue(AUDIT_FORECAST_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        this.SELL_THRU.SetValue(SELL_THRU);
                        this.NUMBER_OF_STORES.SetValue(NUMBER_OF_STORES);
                        this.MATRIX_RULE.SetValue(MATRIX_RULE);
                        this.MATRIX_RULE_QUANTITY.SetValue(MATRIX_RULE_QUANTITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ_def MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ = new MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ_def();
            public class MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ_def()
                {
                    base.procedureName = "MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_DAY_TO_WEEK_SUMMARY_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PUSH_TO_BACK_STOCK_INFO_READ_def MID_PUSH_TO_BACK_STOCK_INFO_READ = new MID_PUSH_TO_BACK_STOCK_INFO_READ_def();
            public class MID_PUSH_TO_BACK_STOCK_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PUSH_TO_BACK_STOCK_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_PUSH_TO_BACK_STOCK_INFO_READ_def()
                {
                    base.procedureName = "MID_PUSH_TO_BACK_STOCK_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PUSH_TO_BACK_STOCK_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PUSH_TO_BACK_STOCK_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PUSH_TO_BACK_STOCK_INFO_INSERT_def MID_PUSH_TO_BACK_STOCK_INFO_INSERT = new MID_PUSH_TO_BACK_STOCK_INFO_INSERT_def();
            public class MID_PUSH_TO_BACK_STOCK_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PUSH_TO_BACK_STOCK_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HDRS_READ;
                private intParameter HDRS_WITH_ERRORS;
                private intParameter HDRS_PROCESSED;
                private intParameter HDRS_SKIPPED;

                public MID_PUSH_TO_BACK_STOCK_INFO_INSERT_def()
                {
                    base.procedureName = "MID_PUSH_TO_BACK_STOCK_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PUSH_TO_BACK_STOCK_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HDRS_READ = new intParameter("@HDRS_READ", base.inputParameterList);
                    HDRS_WITH_ERRORS = new intParameter("@HDRS_WITH_ERRORS", base.inputParameterList);
                    HDRS_PROCESSED = new intParameter("@HDRS_PROCESSED", base.inputParameterList);
                    HDRS_SKIPPED = new intParameter("@HDRS_SKIPPED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? HDRS_READ,
                                  int? HDRS_WITH_ERRORS,
                                  int? HDRS_PROCESSED,
                                  int? HDRS_SKIPPED
                                  )
                {
                    lock (typeof(MID_PUSH_TO_BACK_STOCK_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HDRS_READ.SetValue(HDRS_READ);
                        this.HDRS_WITH_ERRORS.SetValue(HDRS_WITH_ERRORS);
                        this.HDRS_PROCESSED.SetValue(HDRS_PROCESSED);
                        this.HDRS_SKIPPED.SetValue(HDRS_SKIPPED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PUSH_TO_BACK_STOCK_INFO_DELETE_def MID_PUSH_TO_BACK_STOCK_INFO_DELETE = new MID_PUSH_TO_BACK_STOCK_INFO_DELETE_def();
            public class MID_PUSH_TO_BACK_STOCK_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PUSH_TO_BACK_STOCK_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_PUSH_TO_BACK_STOCK_INFO_DELETE_def()
                {
                    base.procedureName = "MID_PUSH_TO_BACK_STOCK_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PUSH_TO_BACK_STOCK_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_PUSH_TO_BACK_STOCK_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ_def MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ = new MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ_def();
            public class MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ_def()
                {
                    base.procedureName = "MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GENERATE_RELIEVE_INTRANSIT_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT_def MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT = new MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT_def();
            public class MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HEADERS_TO_RELIEVE;
                private intParameter FILES_GENERATED;
                private intParameter TOTAL_ERRORS;

                public MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT_def()
                {
                    base.procedureName = "MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("GENERATE_RELIEVE_INTRANSIT_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HEADERS_TO_RELIEVE = new intParameter("@HEADERS_TO_RELIEVE", base.inputParameterList);
                    FILES_GENERATED = new intParameter("@FILES_GENERATED", base.inputParameterList);
                    TOTAL_ERRORS = new intParameter("@TOTAL_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? HEADERS_TO_RELIEVE,
                                  int? FILES_GENERATED,
                                  int? TOTAL_ERRORS
                                  )
                {
                    lock (typeof(MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HEADERS_TO_RELIEVE.SetValue(HEADERS_TO_RELIEVE);
                        this.FILES_GENERATED.SetValue(FILES_GENERATED);
                        this.TOTAL_ERRORS.SetValue(TOTAL_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_DETERMINE_NODE_ACTIVITY_INFO_READ_def MID_DETERMINE_NODE_ACTIVITY_INFO_READ = new MID_DETERMINE_NODE_ACTIVITY_INFO_READ_def();
            public class MID_DETERMINE_NODE_ACTIVITY_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETERMINE_NODE_ACTIVITY_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_DETERMINE_NODE_ACTIVITY_INFO_READ_def()
                {
                    base.procedureName = "MID_DETERMINE_NODE_ACTIVITY_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("DETERMINE_NODE_ACTIVITY_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_DETERMINE_NODE_ACTIVITY_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT_def MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT = new MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT_def();
            public class MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter TOTAL_NODES;
                private intParameter ACTIVE_NODES;
                private intParameter INACTIVE_NODES;
                private intParameter TOTAL_ERRORS;

                public MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT_def()
                {
                    base.procedureName = "MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("DETERMINE_NODE_ACTIVITY_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    TOTAL_NODES = new intParameter("@TOTAL_NODES", base.inputParameterList);
                    ACTIVE_NODES = new intParameter("@ACTIVE_NODES", base.inputParameterList);
                    INACTIVE_NODES = new intParameter("@INACTIVE_NODES", base.inputParameterList);
                    TOTAL_ERRORS = new intParameter("@TOTAL_ERRORS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? TOTAL_NODES,
                                  int? ACTIVE_NODES,
                                  int? INACTIVE_NODES,
                                  int? TOTAL_ERRORS
                                  )
                {
                    lock (typeof(MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.TOTAL_NODES.SetValue(TOTAL_NODES);
                        this.ACTIVE_NODES.SetValue(ACTIVE_NODES);
                        this.INACTIVE_NODES.SetValue(INACTIVE_NODES);
                        this.TOTAL_ERRORS.SetValue(TOTAL_ERRORS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            //Begin TT#1413-MD -jsobek -Data Layer Request - SIZE_CURVE_LOAD_INFO
            public static MID_SIZE_CURVE_LOAD_INFO_READ_def MID_SIZE_CURVE_LOAD_INFO_READ = new MID_SIZE_CURVE_LOAD_INFO_READ_def();
            public class MID_SIZE_CURVE_LOAD_INFO_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_LOAD_INFO_READ.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CURVE_LOAD_INFO_READ_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_LOAD_INFO_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_CURVE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CURVE_LOAD_INFO_READ_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_LOAD_INFO_INSERT_def MID_SIZE_CURVE_LOAD_INFO_INSERT = new MID_SIZE_CURVE_LOAD_INFO_INSERT_def();
            public class MID_SIZE_CURVE_LOAD_INFO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_LOAD_INFO_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter CURVES_READ;
                private intParameter CURVES_WITH_ERRORS;
                private intParameter CURVES_CREATED;
                private intParameter CURVES_MODIFIED;
                private intParameter CURVES_REMOVED;
                private intParameter GROUPS_READ;
                private intParameter GROUPS_WITH_ERRORS;
                private intParameter GROUPS_CREATED;
                private intParameter GROUPS_MODIFIED;
                private intParameter GROUPS_REMOVED;

                public MID_SIZE_CURVE_LOAD_INFO_INSERT_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_LOAD_INFO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SIZE_CURVE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    CURVES_READ = new intParameter("@CURVES_READ", base.inputParameterList);
                    CURVES_WITH_ERRORS = new intParameter("@CURVES_WITH_ERRORS", base.inputParameterList);
                    CURVES_CREATED = new intParameter("@CURVES_CREATED", base.inputParameterList);
                    CURVES_MODIFIED = new intParameter("@CURVES_MODIFIED", base.inputParameterList);
                    CURVES_REMOVED = new intParameter("@CURVES_REMOVED", base.inputParameterList);
                    GROUPS_READ = new intParameter("@GROUPS_READ", base.inputParameterList);
                    GROUPS_WITH_ERRORS = new intParameter("@GROUPS_WITH_ERRORS", base.inputParameterList);
                    GROUPS_CREATED = new intParameter("@GROUPS_CREATED", base.inputParameterList);
                    GROUPS_MODIFIED = new intParameter("@GROUPS_MODIFIED", base.inputParameterList);
                    GROUPS_REMOVED = new intParameter("@GROUPS_REMOVED", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? PROCESS_RID,
                                  int? CURVES_READ,
                                  int? CURVES_WITH_ERRORS,
                                  int? CURVES_CREATED,
                                  int? CURVES_MODIFIED,
                                  int? CURVES_REMOVED,
                                  int? GROUPS_READ,
                                  int? GROUPS_WITH_ERRORS,
                                  int? GROUPS_CREATED,
                                  int? GROUPS_MODIFIED,
                                  int? GROUPS_REMOVED
                                  )
                {
                    lock (typeof(MID_SIZE_CURVE_LOAD_INFO_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                    	this.CURVES_READ.SetValue(CURVES_READ);
                    	this.CURVES_WITH_ERRORS.SetValue(CURVES_WITH_ERRORS);
                        this.CURVES_CREATED.SetValue(CURVES_CREATED);
                        this.CURVES_MODIFIED.SetValue(CURVES_MODIFIED);
                    	this.CURVES_REMOVED.SetValue(CURVES_REMOVED);
                    	this.GROUPS_READ.SetValue(GROUPS_READ);
                    	this.GROUPS_WITH_ERRORS.SetValue(GROUPS_WITH_ERRORS);
                    	this.GROUPS_CREATED.SetValue(GROUPS_CREATED);
                    	this.GROUPS_MODIFIED.SetValue(GROUPS_MODIFIED);
                    	this.GROUPS_REMOVED.SetValue(GROUPS_REMOVED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_LOAD_INFO_DELETE_def MID_SIZE_CURVE_LOAD_INFO_DELETE = new MID_SIZE_CURVE_LOAD_INFO_DELETE_def();
            public class MID_SIZE_CURVE_LOAD_INFO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_LOAD_INFO_DELETE.SQL"

                private intParameter PROCESS_RID;

                public MID_SIZE_CURVE_LOAD_INFO_DELETE_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_LOAD_INFO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_CURVE_LOAD_INFO");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PROCESS_RID)
                {
                    lock (typeof(MID_SIZE_CURVE_LOAD_INFO_DELETE_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            //End TT#1413-MD -jsobek -Data Layer Request - SIZE_CURVE_LOAD_INFO

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        } 

    } //End DataLayer
}
