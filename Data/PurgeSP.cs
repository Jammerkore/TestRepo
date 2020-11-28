using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class PurgeData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_PURGE_DATES_DELETE_ALL_def MID_PURGE_DATES_DELETE_ALL = new MID_PURGE_DATES_DELETE_ALL_def();
			public class MID_PURGE_DATES_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_DELETE_ALL.SQL"

			
			    public MID_PURGE_DATES_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_def MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY = new MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_def();
			public class MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD_def SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD = new SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD_def();
			public class SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD.SQL"

			    private intParameter TIME_ID;

                public SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD_def()
			    {
			        base.procedureName = "SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("ST_HIS_DAY_NONSIZE_PURGE_DATE");
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? TIME_ID)
			    {
                    lock (typeof(SP_MID_ST_HIS_DAY_NONSIZE_PURGE_DATE_UPD_def))
                    {
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_def MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY = new MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_def();
			public class MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_ASN_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_DUMMY_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_DROPSHIP_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_RESERVE_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_WORKUPTOTALBUY_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_PO_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS_def MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS = new MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS_def();
			public class MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_VSW_HEADER_WEEKS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            // Begin TT#5210 - JSmith - Purge Performance
            public static MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES_def MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES = new MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES_def();
            public class MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES.SQL"


                public MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES_def()
                {
                    base.procedureName = "MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PURGE_CRITERIA_HIERARCHIES");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // Begin TT#5210 - JSmith - Purge Performance

            public static MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL_def MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL = new MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL_def();
            public class MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL.SQL"


                public MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL_def()
                {
                    base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PURGE_DATES");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL_def MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL = new MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL_def();
			public class MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL_def MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL = new MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL_def();
			public class MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL.SQL"

			
			    public MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_AUDIT_FORECAST_def MID_PURGE_DATES_READ_AUDIT_FORECAST = new MID_PURGE_DATES_READ_AUDIT_FORECAST_def();
			public class MID_PURGE_DATES_READ_AUDIT_FORECAST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_AUDIT_FORECAST.SQL"

			
			    public MID_PURGE_DATES_READ_AUDIT_FORECAST_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_AUDIT_FORECAST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_AUDIT_FORECAST_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_READ_MODIFIED_SALES_def MID_PURGE_DATES_READ_MODIFIED_SALES = new MID_PURGE_DATES_READ_MODIFIED_SALES_def();
			public class MID_PURGE_DATES_READ_MODIFIED_SALES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_READ_MODIFIED_SALES.SQL"

			
			    public MID_PURGE_DATES_READ_MODIFIED_SALES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_READ_MODIFIED_SALES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PURGE_DATES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_PURGE_DATES_READ_MODIFIED_SALES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_ONETIME_PURGE_def SP_MID_ONETIME_PURGE = new SP_MID_ONETIME_PURGE_def();
			public class SP_MID_ONETIME_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ONETIME_PURGE.SQL"


                public SP_MID_ONETIME_PURGE_def()
			    {
                    base.procedureName = "SP_MID_ONETIME_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ONETIME_PURGE");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(SP_MID_ONETIME_PURGE_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE_def MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE = new MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE_def();
			public class MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE.SQL"

                private charParameter PERFORM_ONETIME_SPECIAL_PURGE_IND;
			
			    public MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE_def()
			    {
			        base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SYSTEM_OPTIONS");
			        PERFORM_ONETIME_SPECIAL_PURGE_IND = new charParameter("@PERFORM_ONETIME_SPECIAL_PURGE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, char? PERFORM_ONETIME_SPECIAL_PURGE_IND)
			    {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE_def))
                    {
                        this.PERFORM_ONETIME_SPECIAL_PURGE_IND.SetValue(PERFORM_ONETIME_SPECIAL_PURGE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_PURGE_AUDIT_def SP_MID_PURGE_AUDIT = new SP_MID_PURGE_AUDIT_def();
            public class SP_MID_PURGE_AUDIT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_PURGE_AUDIT.SQL"

                private intParameter PURGE_DAYS;
                private intParameter COMMIT_LIMIT;
                private intParameter RECORDS_DELETED; //Declare Output Parameter

                public SP_MID_PURGE_AUDIT_def()
			    {
                    base.procedureName = "SP_MID_PURGE_AUDIT";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PURGE_AUDIT");
			        PURGE_DAYS = new intParameter("@PURGE_DAYS", base.inputParameterList);
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? PURGE_DAYS,
			                      int? COMMIT_LIMIT
			                      )
			    {
                    lock (typeof(SP_MID_PURGE_AUDIT_def))
                    {
                        this.PURGE_DAYS.SetValue(PURGE_DAYS);
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RECORDS_DELETED.Value;
                    }
			    }
			}

            public static SP_MID_BUILD_PURGE_DATES_def SP_MID_BUILD_PURGE_DATES = new SP_MID_BUILD_PURGE_DATES_def();
            public class SP_MID_BUILD_PURGE_DATES_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_BUILD_PURGE_DATES.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter debug;
			
			    public SP_MID_BUILD_PURGE_DATES_def()
			    {
                    base.procedureName = "SP_MID_BUILD_PURGE_DATES";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BUILD_PURGE_DATES");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PH_RID,
			                      int? PHL_SEQUENCE
			                      )
			    {
                    lock (typeof(SP_MID_BUILD_PURGE_DATES_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}



			public static MID_PURGE_DATES_UPDATE_DAILY_HISTORY_def MID_PURGE_DATES_UPDATE_DAILY_HISTORY = new MID_PURGE_DATES_UPDATE_DAILY_HISTORY_def();
			public class MID_PURGE_DATES_UPDATE_DAILY_HISTORY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_DAILY_HISTORY.SQL"

                private intParameter PURGE_DAILY_HISTORY;
                private intParameter PURGE_DAILY_HISTORY_WEEKS;
			
			    public MID_PURGE_DATES_UPDATE_DAILY_HISTORY_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_DAILY_HISTORY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_DAILY_HISTORY = new intParameter("@PURGE_DAILY_HISTORY", base.inputParameterList);
			        PURGE_DAILY_HISTORY_WEEKS = new intParameter("@PURGE_DAILY_HISTORY_WEEKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PURGE_DAILY_HISTORY,
			                      int? PURGE_DAILY_HISTORY_WEEKS
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_DAILY_HISTORY_def))
                    {
                        this.PURGE_DAILY_HISTORY.SetValue(PURGE_DAILY_HISTORY);
                        this.PURGE_DAILY_HISTORY_WEEKS.SetValue(PURGE_DAILY_HISTORY_WEEKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY_def MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY = new MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY_def();
			public class MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY.SQL"

                private intParameter PURGE_WEEKLY_HISTORY;
                private intParameter PURGE_WEEKLY_HISTORY_WEEKS;
			
			    public MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_WEEKLY_HISTORY = new intParameter("@PURGE_WEEKLY_HISTORY", base.inputParameterList);
			        PURGE_WEEKLY_HISTORY_WEEKS = new intParameter("@PURGE_WEEKLY_HISTORY_WEEKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PURGE_WEEKLY_HISTORY,
			                      int? PURGE_WEEKLY_HISTORY_WEEKS
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY_def))
                    {
                        this.PURGE_WEEKLY_HISTORY.SetValue(PURGE_WEEKLY_HISTORY);
                        this.PURGE_WEEKLY_HISTORY_WEEKS.SetValue(PURGE_WEEKLY_HISTORY_WEEKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_PLAN_DATES_def MID_PURGE_DATES_UPDATE_PLAN_DATES = new MID_PURGE_DATES_UPDATE_PLAN_DATES_def();
			public class MID_PURGE_DATES_UPDATE_PLAN_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_PLAN_DATES.SQL"

                private intParameter PURGE_PLANS;
                private intParameter PURGE_PLANS_WEEKS;
			
			    public MID_PURGE_DATES_UPDATE_PLAN_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_PLAN_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_PLANS = new intParameter("@PURGE_PLANS", base.inputParameterList);
			        PURGE_PLANS_WEEKS = new intParameter("@PURGE_PLANS_WEEKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PURGE_PLANS,
			                      int? PURGE_PLANS_WEEKS
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_PLAN_DATES_def))
                    {
                        this.PURGE_PLANS.SetValue(PURGE_PLANS);
                        this.PURGE_PLANS_WEEKS.SetValue(PURGE_PLANS_WEEKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_RECEIPT_DATES_def MID_PURGE_DATES_UPDATE_RECEIPT_DATES = new MID_PURGE_DATES_UPDATE_RECEIPT_DATES_def();
			public class MID_PURGE_DATES_UPDATE_RECEIPT_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_RECEIPT_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_RECEIPT_DATETIME;
                private intParameter PURGE_HEADERS_RECEIPT;
                private intParameter PURGE_HEADERS_WEEKS_RECEIPT;
			
			    public MID_PURGE_DATES_UPDATE_RECEIPT_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_RECEIPT_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_RECEIPT_DATETIME = new datetimeParameter("@PURGE_HEADERS_RECEIPT_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_RECEIPT = new intParameter("@PURGE_HEADERS_RECEIPT", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_RECEIPT = new intParameter("@PURGE_HEADERS_WEEKS_RECEIPT", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_RECEIPT_DATETIME,
			                      int? PURGE_HEADERS_RECEIPT,
			                      int? PURGE_HEADERS_WEEKS_RECEIPT
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_RECEIPT_DATES_def))
                    {
                        this.PURGE_HEADERS_RECEIPT_DATETIME.SetValue(PURGE_HEADERS_RECEIPT_DATETIME);
                        this.PURGE_HEADERS_RECEIPT.SetValue(PURGE_HEADERS_RECEIPT);
                        this.PURGE_HEADERS_WEEKS_RECEIPT.SetValue(PURGE_HEADERS_WEEKS_RECEIPT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_ASN_DATES_def MID_PURGE_DATES_UPDATE_ASN_DATES = new MID_PURGE_DATES_UPDATE_ASN_DATES_def();
			public class MID_PURGE_DATES_UPDATE_ASN_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_ASN_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_ASN_DATETIME;
                private intParameter PURGE_HEADERS_ASN;
                private intParameter PURGE_HEADERS_WEEKS_ASN;
			
			    public MID_PURGE_DATES_UPDATE_ASN_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_ASN_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_ASN_DATETIME = new datetimeParameter("@PURGE_HEADERS_ASN_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_ASN = new intParameter("@PURGE_HEADERS_ASN", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_ASN = new intParameter("@PURGE_HEADERS_WEEKS_ASN", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_ASN_DATETIME,
			                      int? PURGE_HEADERS_ASN,
			                      int? PURGE_HEADERS_WEEKS_ASN
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_ASN_DATES_def))
                    {
                        this.PURGE_HEADERS_ASN_DATETIME.SetValue(PURGE_HEADERS_ASN_DATETIME);
                        this.PURGE_HEADERS_ASN.SetValue(PURGE_HEADERS_ASN);
                        this.PURGE_HEADERS_WEEKS_ASN.SetValue(PURGE_HEADERS_WEEKS_ASN);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_DUMMY_DATES_def MID_PURGE_DATES_UPDATE_DUMMY_DATES = new MID_PURGE_DATES_UPDATE_DUMMY_DATES_def();
			public class MID_PURGE_DATES_UPDATE_DUMMY_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_DUMMY_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_DUMMY_DATETIME;
                private intParameter PURGE_HEADERS_DUMMY;
                private intParameter PURGE_HEADERS_WEEKS_DUMMY;
			
			    public MID_PURGE_DATES_UPDATE_DUMMY_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_DUMMY_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_DUMMY_DATETIME = new datetimeParameter("@PURGE_HEADERS_DUMMY_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_DUMMY = new intParameter("@PURGE_HEADERS_DUMMY", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_DUMMY = new intParameter("@PURGE_HEADERS_WEEKS_DUMMY", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_DUMMY_DATETIME,
			                      int? PURGE_HEADERS_DUMMY,
			                      int? PURGE_HEADERS_WEEKS_DUMMY
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_DUMMY_DATES_def))
                    {
                        this.PURGE_HEADERS_DUMMY_DATETIME.SetValue(PURGE_HEADERS_DUMMY_DATETIME);
                        this.PURGE_HEADERS_DUMMY.SetValue(PURGE_HEADERS_DUMMY);
                        this.PURGE_HEADERS_WEEKS_DUMMY.SetValue(PURGE_HEADERS_WEEKS_DUMMY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_DROPSHIP_DATES_def MID_PURGE_DATES_UPDATE_DROPSHIP_DATES = new MID_PURGE_DATES_UPDATE_DROPSHIP_DATES_def();
			public class MID_PURGE_DATES_UPDATE_DROPSHIP_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_DROPSHIP_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_DROPSHIP_DATETIME;
                private intParameter PURGE_HEADERS_DROPSHIP;
                private intParameter PURGE_HEADERS_WEEKS_DROPSHIP;
			
			    public MID_PURGE_DATES_UPDATE_DROPSHIP_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_DROPSHIP_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_DROPSHIP_DATETIME = new datetimeParameter("@PURGE_HEADERS_DROPSHIP_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_DROPSHIP = new intParameter("@PURGE_HEADERS_DROPSHIP", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_DROPSHIP = new intParameter("@PURGE_HEADERS_WEEKS_DROPSHIP", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_DROPSHIP_DATETIME,
			                      int? PURGE_HEADERS_DROPSHIP,
			                      int? PURGE_HEADERS_WEEKS_DROPSHIP
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_DROPSHIP_DATES_def))
                    {
                        this.PURGE_HEADERS_DROPSHIP_DATETIME.SetValue(PURGE_HEADERS_DROPSHIP_DATETIME);
                        this.PURGE_HEADERS_DROPSHIP.SetValue(PURGE_HEADERS_DROPSHIP);
                        this.PURGE_HEADERS_WEEKS_DROPSHIP.SetValue(PURGE_HEADERS_WEEKS_DROPSHIP);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_RESERVE_DATES_def MID_PURGE_DATES_UPDATE_RESERVE_DATES = new MID_PURGE_DATES_UPDATE_RESERVE_DATES_def();
			public class MID_PURGE_DATES_UPDATE_RESERVE_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_RESERVE_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_RESERVE_DATETIME;
                private intParameter PURGE_HEADERS_RESERVE;
                private intParameter PURGE_HEADERS_WEEKS_RESERVE;
			
			    public MID_PURGE_DATES_UPDATE_RESERVE_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_RESERVE_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_RESERVE_DATETIME = new datetimeParameter("@PURGE_HEADERS_RESERVE_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_RESERVE = new intParameter("@PURGE_HEADERS_RESERVE", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_RESERVE = new intParameter("@PURGE_HEADERS_WEEKS_RESERVE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_RESERVE_DATETIME,
			                      int? PURGE_HEADERS_RESERVE,
			                      int? PURGE_HEADERS_WEEKS_RESERVE
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_RESERVE_DATES_def))
                    {
                        this.PURGE_HEADERS_RESERVE_DATETIME.SetValue(PURGE_HEADERS_RESERVE_DATETIME);
                        this.PURGE_HEADERS_RESERVE.SetValue(PURGE_HEADERS_RESERVE);
                        this.PURGE_HEADERS_WEEKS_RESERVE.SetValue(PURGE_HEADERS_WEEKS_RESERVE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES_def MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES = new MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES_def();
			public class MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_WORKUPTOTALBUY_DATETIME;
                private intParameter PURGE_HEADERS_WORKUPTOTALBUY;
                private intParameter PURGE_HEADERS_WEEKS_WORKUPTOTALBUY;
			
			    public MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_WORKUPTOTALBUY_DATETIME = new datetimeParameter("@PURGE_HEADERS_WORKUPTOTALBUY_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_WORKUPTOTALBUY = new intParameter("@PURGE_HEADERS_WORKUPTOTALBUY", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_WORKUPTOTALBUY = new intParameter("@PURGE_HEADERS_WEEKS_WORKUPTOTALBUY", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_WORKUPTOTALBUY_DATETIME,
			                      int? PURGE_HEADERS_WORKUPTOTALBUY,
			                      int? PURGE_HEADERS_WEEKS_WORKUPTOTALBUY
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_WORKUPTOTALBUY_DATES_def))
                    {
                        this.PURGE_HEADERS_WORKUPTOTALBUY_DATETIME.SetValue(PURGE_HEADERS_WORKUPTOTALBUY_DATETIME);
                        this.PURGE_HEADERS_WORKUPTOTALBUY.SetValue(PURGE_HEADERS_WORKUPTOTALBUY);
                        this.PURGE_HEADERS_WEEKS_WORKUPTOTALBUY.SetValue(PURGE_HEADERS_WEEKS_WORKUPTOTALBUY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_PO_DATES_def MID_PURGE_DATES_UPDATE_PO_DATES = new MID_PURGE_DATES_UPDATE_PO_DATES_def();
			public class MID_PURGE_DATES_UPDATE_PO_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_PO_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_PO_DATETIME;
                private intParameter PURGE_HEADERS_PO;
                private intParameter PURGE_HEADERS_WEEKS_PO;
			
			    public MID_PURGE_DATES_UPDATE_PO_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_PO_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_PO_DATETIME = new datetimeParameter("@PURGE_HEADERS_PO_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_PO = new intParameter("@PURGE_HEADERS_PO", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_PO = new intParameter("@PURGE_HEADERS_WEEKS_PO", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_PO_DATETIME,
			                      int? PURGE_HEADERS_PO,
			                      int? PURGE_HEADERS_WEEKS_PO
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_PO_DATES_def))
                    {
                        this.PURGE_HEADERS_PO_DATETIME.SetValue(PURGE_HEADERS_PO_DATETIME);
                        this.PURGE_HEADERS_PO.SetValue(PURGE_HEADERS_PO);
                        this.PURGE_HEADERS_WEEKS_PO.SetValue(PURGE_HEADERS_WEEKS_PO);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PURGE_DATES_UPDATE_VSW_DATES_def MID_PURGE_DATES_UPDATE_VSW_DATES = new MID_PURGE_DATES_UPDATE_VSW_DATES_def();
			public class MID_PURGE_DATES_UPDATE_VSW_DATES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_DATES_UPDATE_VSW_DATES.SQL"

                private datetimeParameter PURGE_HEADERS_VSW_DATETIME;
                private intParameter PURGE_HEADERS_VSW;
                private intParameter PURGE_HEADERS_WEEKS_VSW;
			
			    public MID_PURGE_DATES_UPDATE_VSW_DATES_def()
			    {
			        base.procedureName = "MID_PURGE_DATES_UPDATE_VSW_DATES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PURGE_DATES");
			        PURGE_HEADERS_VSW_DATETIME = new datetimeParameter("@PURGE_HEADERS_VSW_DATETIME", base.inputParameterList);
			        PURGE_HEADERS_VSW = new intParameter("@PURGE_HEADERS_VSW", base.inputParameterList);
			        PURGE_HEADERS_WEEKS_VSW = new intParameter("@PURGE_HEADERS_WEEKS_VSW", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      DateTime? PURGE_HEADERS_VSW_DATETIME,
			                      int? PURGE_HEADERS_VSW,
			                      int? PURGE_HEADERS_WEEKS_VSW
			                      )
			    {
                    lock (typeof(MID_PURGE_DATES_UPDATE_VSW_DATES_def))
                    {
                        this.PURGE_HEADERS_VSW_DATETIME.SetValue(PURGE_HEADERS_VSW_DATETIME);
                        this.PURGE_HEADERS_VSW.SetValue(PURGE_HEADERS_VSW);
                        this.PURGE_HEADERS_WEEKS_VSW.SetValue(PURGE_HEADERS_WEEKS_VSW);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
