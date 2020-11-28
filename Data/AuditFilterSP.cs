//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;

//namespace MIDRetail.Data
//{
//    public partial class AuditFilterData : DataLayer
//    {
//        protected static class StoredProcedures
//        {

//            //public static MID_AUDIT_FILTER_READ_def MID_AUDIT_FILTER_READ = new MID_AUDIT_FILTER_READ_def();
//            //public class MID_AUDIT_FILTER_READ_def : baseStoredProcedure
//            //{
//            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_FILTER_READ.SQL"

//            //    public intParameter USER_RID;
			
//            //    public MID_AUDIT_FILTER_READ_def()
//            //    {
//            //        base.procedureName = "MID_AUDIT_FILTER_READ";
//            //        base.procedureType = storedProcedureTypes.Read;
//            //        base.tableNames.Add("AUDIT_FILTER");
//            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//            //    }
			
//            //    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
//            //    {
//            //        this.USER_RID.SetValue(USER_RID);
//            //        return ExecuteStoredProcedureForRead(_dba);
//            //    }
//            //}

//            //public static MID_AUDIT_FILTER_INSERT_def MID_AUDIT_FILTER_INSERT = new MID_AUDIT_FILTER_INSERT_def();
//            //public class MID_AUDIT_FILTER_INSERT_def : baseStoredProcedure
//            //{
//            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_FILTER_INSERT.SQL"

//            //    public intParameter USER_RID;
//            //    public intParameter RUN_DATE_TYPE;
//            //    public intParameter RUN_DATE_BETWEEN_FROM;
//            //    public intParameter RUN_DATE_BETWEEN_TO;
//            //    public datetimeParameter RUN_DATE_FROM;
//            //    public datetimeParameter RUN_DATE_TO;
//            //    public intParameter PROCESS_HIGHEST_MESSAGE_LEVEL;
//            //    public intParameter DETAIL_HIGHEST_MESSAGE_LEVEL;
//            //    public intParameter DURATION;
//            //    public charParameter SHOW_RUNNING;
//            //    public charParameter SHOW_COMPLETED;
//            //    public charParameter SHOW_MY_TASKS_ONLY;
			
//            //    public MID_AUDIT_FILTER_INSERT_def()
//            //    {
//            //        base.procedureName = "MID_AUDIT_FILTER_INSERT";
//            //        base.procedureType = storedProcedureTypes.Insert;
//            //        base.tableNames.Add("AUDIT_FILTER");
//            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//            //        RUN_DATE_TYPE = new intParameter("@RUN_DATE_TYPE", base.inputParameterList);
//            //        RUN_DATE_BETWEEN_FROM = new intParameter("@RUN_DATE_BETWEEN_FROM", base.inputParameterList);
//            //        RUN_DATE_BETWEEN_TO = new intParameter("@RUN_DATE_BETWEEN_TO", base.inputParameterList);
//            //        RUN_DATE_FROM = new datetimeParameter("@RUN_DATE_FROM", base.inputParameterList);
//            //        RUN_DATE_TO = new datetimeParameter("@RUN_DATE_TO", base.inputParameterList);
//            //        PROCESS_HIGHEST_MESSAGE_LEVEL = new intParameter("@PROCESS_HIGHEST_MESSAGE_LEVEL", base.inputParameterList);
//            //        DETAIL_HIGHEST_MESSAGE_LEVEL = new intParameter("@DETAIL_HIGHEST_MESSAGE_LEVEL", base.inputParameterList);
//            //        DURATION = new intParameter("@DURATION", base.inputParameterList);
//            //        SHOW_RUNNING = new charParameter("@SHOW_RUNNING", base.inputParameterList);
//            //        SHOW_COMPLETED = new charParameter("@SHOW_COMPLETED", base.inputParameterList);
//            //        SHOW_MY_TASKS_ONLY = new charParameter("@SHOW_MY_TASKS_ONLY", base.inputParameterList);
//            //    }
			
//            //    public int Insert(DatabaseAccess _dba, int? USER_RID,
//            //                      int? RUN_DATE_TYPE,
//            //                      int? RUN_DATE_BETWEEN_FROM,
//            //                      int? RUN_DATE_BETWEEN_TO,
//            //                      DateTime? RUN_DATE_FROM,
//            //                      DateTime? RUN_DATE_TO,
//            //                      int? PROCESS_HIGHEST_MESSAGE_LEVEL,
//            //                      int? DETAIL_HIGHEST_MESSAGE_LEVEL,
//            //                      int? DURATION,
//            //                      char? SHOW_RUNNING,
//            //                      char? SHOW_COMPLETED,
//            //                      char? SHOW_MY_TASKS_ONLY
//            //                      )
//            //    {
//            //        this.USER_RID.SetValue(USER_RID);
//            //        this.RUN_DATE_TYPE.SetValue(RUN_DATE_TYPE);
//            //        this.RUN_DATE_BETWEEN_FROM.SetValue(RUN_DATE_BETWEEN_FROM);
//            //        this.RUN_DATE_BETWEEN_TO.SetValue(RUN_DATE_BETWEEN_TO);
//            //        this.RUN_DATE_FROM.SetValue(RUN_DATE_FROM);
//            //        this.RUN_DATE_TO.SetValue(RUN_DATE_TO);
//            //        this.PROCESS_HIGHEST_MESSAGE_LEVEL.SetValue(PROCESS_HIGHEST_MESSAGE_LEVEL);
//            //        this.DETAIL_HIGHEST_MESSAGE_LEVEL.SetValue(DETAIL_HIGHEST_MESSAGE_LEVEL);
//            //        this.DURATION.SetValue(DURATION);
//            //        this.SHOW_RUNNING.SetValue(SHOW_RUNNING);
//            //        this.SHOW_COMPLETED.SetValue(SHOW_COMPLETED);
//            //        this.SHOW_MY_TASKS_ONLY.SetValue(SHOW_MY_TASKS_ONLY);
//            //        return ExecuteStoredProcedureForInsert(_dba);
//            //    }
//            //}

//            //public static MID_AUDIT_FILTER_UPDATE_def MID_AUDIT_FILTER_UPDATE = new MID_AUDIT_FILTER_UPDATE_def();
//            //public class MID_AUDIT_FILTER_UPDATE_def : baseStoredProcedure
//            //{
//            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_AUDIT_FILTER_UPDATE.SQL"

//            //    public intParameter USER_RID;
//            //    public intParameter RUN_DATE_TYPE;
//            //    public intParameter RUN_DATE_BETWEEN_FROM;
//            //    public intParameter RUN_DATE_BETWEEN_TO;
//            //    public datetimeParameter RUN_DATE_FROM;
//            //    public datetimeParameter RUN_DATE_TO;
//            //    public intParameter PROCESS_HIGHEST_MESSAGE_LEVEL;
//            //    public intParameter DETAIL_HIGHEST_MESSAGE_LEVEL;
//            //    public intParameter DURATION;
//            //    public charParameter SHOW_RUNNING;
//            //    public charParameter SHOW_COMPLETED;
//            //    public charParameter SHOW_MY_TASKS_ONLY;
			
//            //    public MID_AUDIT_FILTER_UPDATE_def()
//            //    {
//            //        base.procedureName = "MID_AUDIT_FILTER_UPDATE";
//            //        base.procedureType = storedProcedureTypes.Update;
//            //        base.tableNames.Add("AUDIT_FILTER");
//            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
//            //        RUN_DATE_TYPE = new intParameter("@RUN_DATE_TYPE", base.inputParameterList);
//            //        RUN_DATE_BETWEEN_FROM = new intParameter("@RUN_DATE_BETWEEN_FROM", base.inputParameterList);
//            //        RUN_DATE_BETWEEN_TO = new intParameter("@RUN_DATE_BETWEEN_TO", base.inputParameterList);
//            //        RUN_DATE_FROM = new datetimeParameter("@RUN_DATE_FROM", base.inputParameterList);
//            //        RUN_DATE_TO = new datetimeParameter("@RUN_DATE_TO", base.inputParameterList);
//            //        PROCESS_HIGHEST_MESSAGE_LEVEL = new intParameter("@PROCESS_HIGHEST_MESSAGE_LEVEL", base.inputParameterList);
//            //        DETAIL_HIGHEST_MESSAGE_LEVEL = new intParameter("@DETAIL_HIGHEST_MESSAGE_LEVEL", base.inputParameterList);
//            //        DURATION = new intParameter("@DURATION", base.inputParameterList);
//            //        SHOW_RUNNING = new charParameter("@SHOW_RUNNING", base.inputParameterList);
//            //        SHOW_COMPLETED = new charParameter("@SHOW_COMPLETED", base.inputParameterList);
//            //        SHOW_MY_TASKS_ONLY = new charParameter("@SHOW_MY_TASKS_ONLY", base.inputParameterList);
//            //    }
			
//            //    public int Update(DatabaseAccess _dba, int? USER_RID,
//            //                      int? RUN_DATE_TYPE,
//            //                      int? RUN_DATE_BETWEEN_FROM,
//            //                      int? RUN_DATE_BETWEEN_TO,
//            //                      DateTime? RUN_DATE_FROM,
//            //                      DateTime? RUN_DATE_TO,
//            //                      int? PROCESS_HIGHEST_MESSAGE_LEVEL,
//            //                      int? DETAIL_HIGHEST_MESSAGE_LEVEL,
//            //                      int? DURATION,
//            //                      char? SHOW_RUNNING,
//            //                      char? SHOW_COMPLETED,
//            //                      char? SHOW_MY_TASKS_ONLY
//            //                      )
//            //    {
//            //        this.USER_RID.SetValue(USER_RID);
//            //        this.RUN_DATE_TYPE.SetValue(RUN_DATE_TYPE);
//            //        this.RUN_DATE_BETWEEN_FROM.SetValue(RUN_DATE_BETWEEN_FROM);
//            //        this.RUN_DATE_BETWEEN_TO.SetValue(RUN_DATE_BETWEEN_TO);
//            //        this.RUN_DATE_FROM.SetValue(RUN_DATE_FROM);
//            //        this.RUN_DATE_TO.SetValue(RUN_DATE_TO);
//            //        this.PROCESS_HIGHEST_MESSAGE_LEVEL.SetValue(PROCESS_HIGHEST_MESSAGE_LEVEL);
//            //        this.DETAIL_HIGHEST_MESSAGE_LEVEL.SetValue(DETAIL_HIGHEST_MESSAGE_LEVEL);
//            //        this.DURATION.SetValue(DURATION);
//            //        this.SHOW_RUNNING.SetValue(SHOW_RUNNING);
//            //        this.SHOW_COMPLETED.SetValue(SHOW_COMPLETED);
//            //        this.SHOW_MY_TASKS_ONLY.SetValue(SHOW_MY_TASKS_ONLY);
//            //        return ExecuteStoredProcedureForUpdate(_dba);
//            //    }
//            //}

//            //INSERT NEW STORED PROCEDURES ABOVE HERE

//        }

//    }  
//}
