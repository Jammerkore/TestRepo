//using System;

//using System.Data;
//using System.Collections;
//using System.Globalization;
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{

//    public partial class AuditFilterData : DataLayer
//    {
//        public AuditFilterData() : base()
//        {
            
//        }

//        public DataTable AuditFilter_Read(int aUserRID)
//        {
//            try
//            {
//                return StoredProcedures.MID_AUDIT_FILTER_READ.Read(_dba, USER_RID: aUserRID);
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AuditFilter_Insert(int aUserRID, eFilterDateType aRunDateType, 
//            int aRunDateBetweenFrom, int aRunDateBetweenTo, DateTime aRunDateFrom, DateTime aRunDateTo,
//            int aProcessHighestMessageLevel, int aDetailHighestMessageLevel, 
//            int aDuration, bool aShowRunningProcesses, bool aShowCompletedProcesses,
//            bool aShowMyTasksOnly)
//        {
//            try
//            {
//                StoredProcedures.MID_AUDIT_FILTER_INSERT.Insert(_dba,
//                                                                USER_RID: aUserRID,
//                                                                RUN_DATE_TYPE: Convert.ToInt32(aRunDateType, CultureInfo.CurrentCulture),
//                                                                RUN_DATE_BETWEEN_FROM: aRunDateBetweenFrom,
//                                                                RUN_DATE_BETWEEN_TO: aRunDateBetweenTo,
//                                                                RUN_DATE_FROM: aRunDateFrom,
//                                                                RUN_DATE_TO: aRunDateTo,
//                                                                PROCESS_HIGHEST_MESSAGE_LEVEL: aProcessHighestMessageLevel,
//                                                                DETAIL_HIGHEST_MESSAGE_LEVEL: aDetailHighestMessageLevel,
//                                                                DURATION: aDuration,
//                                                                SHOW_RUNNING: Include.ConvertBoolToChar(aShowRunningProcesses),
//                                                                SHOW_COMPLETED: Include.ConvertBoolToChar(aShowCompletedProcesses),
//                                                                SHOW_MY_TASKS_ONLY: Include.ConvertBoolToChar(aShowMyTasksOnly)
//                                                                );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        public void AuditFilter_Update(int aUserRID, eFilterDateType aRunDateType, 
//            int aRunDateBetweenFrom, int aRunDateBetweenTo, DateTime aRunDateFrom, DateTime aRunDateTo,
//            int aProcessHighestMessageLevel, int aDetailHighestMessageLevel, 
//            int aDuration, bool aShowRunningProcesses, bool aShowCompletedProcesses,
//            bool aShowMyTasksOnly)
//        {
//            try
//            {
//                StoredProcedures.MID_AUDIT_FILTER_UPDATE.Update(_dba,
//                                                                USER_RID: aUserRID,
//                                                                RUN_DATE_TYPE: Convert.ToInt32(aRunDateType, CultureInfo.CurrentCulture),
//                                                                RUN_DATE_BETWEEN_FROM: aRunDateBetweenFrom,
//                                                                RUN_DATE_BETWEEN_TO: aRunDateBetweenTo,
//                                                                RUN_DATE_FROM: aRunDateFrom,
//                                                                RUN_DATE_TO: aRunDateTo,
//                                                                PROCESS_HIGHEST_MESSAGE_LEVEL: aProcessHighestMessageLevel,
//                                                                DETAIL_HIGHEST_MESSAGE_LEVEL: aDetailHighestMessageLevel,
//                                                                DURATION: aDuration,
//                                                                SHOW_RUNNING: Include.ConvertBoolToChar(aShowRunningProcesses),
//                                                                SHOW_COMPLETED: Include.ConvertBoolToChar(aShowCompletedProcesses),
//                                                                SHOW_MY_TASKS_ONLY: Include.ConvertBoolToChar(aShowMyTasksOnly)
//                                                                );
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
//    }
//}
