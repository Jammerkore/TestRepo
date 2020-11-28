using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class VariablesData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS_def MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS = new MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS_def();
			public class MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS.SQL"

			    private intParameter HN_RID;
			
			    public MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHAIN_HISTORY_WEEK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS_def MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS = new MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS_def();
			public class MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY0_READ_TIME_IDS_def MID_STORE_HISTORY_DAY0_READ_TIME_IDS = new MID_STORE_HISTORY_DAY0_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY0_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY0_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY0_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY0_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY0_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY1_READ_TIME_IDS_def MID_STORE_HISTORY_DAY1_READ_TIME_IDS = new MID_STORE_HISTORY_DAY1_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY1_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY1_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY1_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY1_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY1_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY2_READ_TIME_IDS_def MID_STORE_HISTORY_DAY2_READ_TIME_IDS = new MID_STORE_HISTORY_DAY2_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY2_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY2_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY2_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY2_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY2_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY3_READ_TIME_IDS_def MID_STORE_HISTORY_DAY3_READ_TIME_IDS = new MID_STORE_HISTORY_DAY3_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY3_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY3_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY3_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY3_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY3_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY4_READ_TIME_IDS_def MID_STORE_HISTORY_DAY4_READ_TIME_IDS = new MID_STORE_HISTORY_DAY4_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY4_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY4_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY4_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY4_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY4_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY5_READ_TIME_IDS_def MID_STORE_HISTORY_DAY5_READ_TIME_IDS = new MID_STORE_HISTORY_DAY5_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY5_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY5_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY5_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY5_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY5_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY6_READ_TIME_IDS_def MID_STORE_HISTORY_DAY6_READ_TIME_IDS = new MID_STORE_HISTORY_DAY6_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY6_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY6_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY6_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY6_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY6_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY7_READ_TIME_IDS_def MID_STORE_HISTORY_DAY7_READ_TIME_IDS = new MID_STORE_HISTORY_DAY7_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY7_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY7_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY7_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY7_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY7_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY8_READ_TIME_IDS_def MID_STORE_HISTORY_DAY8_READ_TIME_IDS = new MID_STORE_HISTORY_DAY8_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY8_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY8_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY8_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY8_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY8_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY9_READ_TIME_IDS_def MID_STORE_HISTORY_DAY9_READ_TIME_IDS = new MID_STORE_HISTORY_DAY9_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_DAY9_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY9_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_DAY9_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY9_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_DAY9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY9_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK0_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK0_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK0_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK0_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK0_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK0_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK0_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK0_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK1_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK1_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK1_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK1_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK1_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK1_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK1_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK1_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK2_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK2_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK2_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK2_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK2_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK2_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK2_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK2_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK3_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK3_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK3_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK3_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK3_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK3_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK3_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK3_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK4_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK4_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK4_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK4_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK4_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK4_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK4_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK4_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK5_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK5_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK5_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK5_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK5_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK5_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK5_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK5_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK6_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK6_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK6_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK6_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK6_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK6_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK6_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK6_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK7_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK7_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK7_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK7_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK7_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK7_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK7_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK7_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK8_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK8_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK8_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK8_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK8_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK8_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK8_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK8_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK9_READ_TIME_IDS_def MID_STORE_HISTORY_WEEK9_READ_TIME_IDS = new MID_STORE_HISTORY_WEEK9_READ_TIME_IDS_def();
			public class MID_STORE_HISTORY_WEEK9_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK9_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
			
			    public MID_STORE_HISTORY_WEEK9_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK9_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_HISTORY_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK9_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK0_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK0_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK0_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK0_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK0_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK0_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK0_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK0_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK1_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK1_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK1_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK1_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK1_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK1_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK1_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK1_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK2_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK2_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK2_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK2_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK2_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK2_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK2_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK2_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK3_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK3_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK3_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK3_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK3_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK3_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK3_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK3_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK4_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK4_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK4_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK4_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK4_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK4_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK4_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK4_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK5_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK5_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK5_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK5_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK5_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK5_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK5_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK5_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK6_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK6_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK6_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK6_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK6_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK6_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK6_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK6_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK7_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK7_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK7_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK7_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK7_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK7_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK7_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK7_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK8_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK8_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK8_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK8_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK8_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK8_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK8_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK8_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK9_READ_TIME_IDS_def MID_STORE_FORECAST_WEEK9_READ_TIME_IDS = new MID_STORE_FORECAST_WEEK9_READ_TIME_IDS_def();
			public class MID_STORE_FORECAST_WEEK9_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK9_READ_TIME_IDS.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_STORE_FORECAST_WEEK9_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK9_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_FORECAST_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? FV_RID
			                          )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK9_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
                private tableParameter NODE_LIST;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        NODE_LIST = new tableParameter("@NODE_LIST", "HN_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID,
			                      DataTable NODE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.NODE_LIST.SetValue(NODE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE = new MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def();
			public class MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
                private tableParameter NODE_LIST;
			
			    public MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        NODE_LIST = new tableParameter("@NODE_LIST", "HN_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID,
			                      DataTable NODE_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.NODE_LIST.SetValue(NODE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE_def MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE = new MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE_def();
			public class MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
                private tableParameter NODE_LIST;
			
			    public MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        NODE_LIST = new tableParameter("@NODE_LIST", "HN_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID,
			                      DataTable NODE_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.NODE_LIST.SetValue(NODE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE_def MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE = new MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE_def();
			public class MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
                private tableParameter NODE_LIST;
			
			    public MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_HISTORY_WEEK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        NODE_LIST = new tableParameter("@NODE_LIST", "HN_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID,
			                      DataTable NODE_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.NODE_LIST.SetValue(NODE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK0_DELETE_def MID_STORE_HISTORY_WEEK0_DELETE = new MID_STORE_HISTORY_WEEK0_DELETE_def();
			public class MID_STORE_HISTORY_WEEK0_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK0_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK0_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK0_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK0_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK1_DELETE_def MID_STORE_HISTORY_WEEK1_DELETE = new MID_STORE_HISTORY_WEEK1_DELETE_def();
			public class MID_STORE_HISTORY_WEEK1_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK1_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK1_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK1_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK1_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK2_DELETE_def MID_STORE_HISTORY_WEEK2_DELETE = new MID_STORE_HISTORY_WEEK2_DELETE_def();
			public class MID_STORE_HISTORY_WEEK2_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK2_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK2_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK2_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK2_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK3_DELETE_def MID_STORE_HISTORY_WEEK3_DELETE = new MID_STORE_HISTORY_WEEK3_DELETE_def();
			public class MID_STORE_HISTORY_WEEK3_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK3_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK3_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK3_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK3_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK4_DELETE_def MID_STORE_HISTORY_WEEK4_DELETE = new MID_STORE_HISTORY_WEEK4_DELETE_def();
			public class MID_STORE_HISTORY_WEEK4_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK4_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK4_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK4_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK4_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK5_DELETE_def MID_STORE_HISTORY_WEEK5_DELETE = new MID_STORE_HISTORY_WEEK5_DELETE_def();
			public class MID_STORE_HISTORY_WEEK5_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK5_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK5_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK5_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK5_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK6_DELETE_def MID_STORE_HISTORY_WEEK6_DELETE = new MID_STORE_HISTORY_WEEK6_DELETE_def();
			public class MID_STORE_HISTORY_WEEK6_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK6_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK6_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK6_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK6_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK7_DELETE_def MID_STORE_HISTORY_WEEK7_DELETE = new MID_STORE_HISTORY_WEEK7_DELETE_def();
			public class MID_STORE_HISTORY_WEEK7_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK7_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK7_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK7_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK7_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK8_DELETE_def MID_STORE_HISTORY_WEEK8_DELETE = new MID_STORE_HISTORY_WEEK8_DELETE_def();
			public class MID_STORE_HISTORY_WEEK8_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK8_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK8_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK8_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK8_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK9_DELETE_def MID_STORE_HISTORY_WEEK9_DELETE = new MID_STORE_HISTORY_WEEK9_DELETE_def();
			public class MID_STORE_HISTORY_WEEK9_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK9_DELETE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_HISTORY_WEEK9_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK9_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK9_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK0_DELETE_def MID_STORE_FORECAST_WEEK0_DELETE = new MID_STORE_FORECAST_WEEK0_DELETE_def();
			public class MID_STORE_FORECAST_WEEK0_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK0_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK0_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK0_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK0_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK1_DELETE_def MID_STORE_FORECAST_WEEK1_DELETE = new MID_STORE_FORECAST_WEEK1_DELETE_def();
			public class MID_STORE_FORECAST_WEEK1_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK1_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK1_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK1_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK1_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK2_DELETE_def MID_STORE_FORECAST_WEEK2_DELETE = new MID_STORE_FORECAST_WEEK2_DELETE_def();
			public class MID_STORE_FORECAST_WEEK2_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK2_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK2_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK2_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK2_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK3_DELETE_def MID_STORE_FORECAST_WEEK3_DELETE = new MID_STORE_FORECAST_WEEK3_DELETE_def();
			public class MID_STORE_FORECAST_WEEK3_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK3_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK3_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK3_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK3_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK4_DELETE_def MID_STORE_FORECAST_WEEK4_DELETE = new MID_STORE_FORECAST_WEEK4_DELETE_def();
			public class MID_STORE_FORECAST_WEEK4_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK4_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK4_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK4_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK4_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK5_DELETE_def MID_STORE_FORECAST_WEEK5_DELETE = new MID_STORE_FORECAST_WEEK5_DELETE_def();
			public class MID_STORE_FORECAST_WEEK5_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK5_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK5_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK5_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK5_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK6_DELETE_def MID_STORE_FORECAST_WEEK6_DELETE = new MID_STORE_FORECAST_WEEK6_DELETE_def();
			public class MID_STORE_FORECAST_WEEK6_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK6_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK6_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK6_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK6_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK7_DELETE_def MID_STORE_FORECAST_WEEK7_DELETE = new MID_STORE_FORECAST_WEEK7_DELETE_def();
			public class MID_STORE_FORECAST_WEEK7_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK7_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK7_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK7_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK7_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK8_DELETE_def MID_STORE_FORECAST_WEEK8_DELETE = new MID_STORE_FORECAST_WEEK8_DELETE_def();
			public class MID_STORE_FORECAST_WEEK8_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK8_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK8_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK8_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK8_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK9_DELETE_def MID_STORE_FORECAST_WEEK9_DELETE = new MID_STORE_FORECAST_WEEK9_DELETE_def();
			public class MID_STORE_FORECAST_WEEK9_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK9_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK9_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK9_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK9_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE_def MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE = new MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE_def();
			public class MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK0");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK1");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK2");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
                    STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK3");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK4");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK5");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK6");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK7");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK8");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE_def MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE = new MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE_def();
			public class MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK9");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST_def MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
                private tableParameter STORE_LIST;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			        STORE_LIST = new tableParameter("@STORE_LIST", "STORE_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST,
			                      DataTable STORE_LIST
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        this.STORE_LIST.SetValue(STORE_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE_def MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE = new MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE_def();
			public class MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private tableParameter TIME_LIST;
			
			    public MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_HISTORY_WEEK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE_def MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE = new MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE_def();
			public class MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE = new MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def();
			public class MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE.SQL"

                private intParameter HN_RID;
                private intParameter FV_RID;
                private tableParameter TIME_LIST;
			
			    public MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK_LOCK");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                    TIME_LIST = new tableParameter("@TIME_LIST", "TIME_ID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? FV_RID,
			                      DataTable TIME_LIST
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_LIST.SetValue(TIME_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_HIS_WK_READ_def SP_MID_CHN_HIS_WK_READ = new SP_MID_CHN_HIS_WK_READ_def();
            public class SP_MID_CHN_HIS_WK_READ_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_HIS_WK_READ.SQL"

                private tableParameter dt;
                private charParameter Rollup;
			
			    public SP_MID_CHN_HIS_WK_READ_def()
			    {
                    base.procedureName = "SP_MID_CHN_HIS_WK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHN_HIS_WK");
			        dt = new tableParameter("@dt", "MID_CHN_HIS_WK_READ_TYPE", base.inputParameterList);
			        Rollup = new charParameter("@Rollup", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable dt,
			                          char? Rollup
			                          )
			    {
                    lock (typeof(SP_MID_CHN_HIS_WK_READ_def))
                    {
                        this.dt.SetValue(dt);
                        this.Rollup.SetValue(Rollup);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_MOD_WK_READ_def SP_MID_CHN_MOD_WK_READ = new SP_MID_CHN_MOD_WK_READ_def();
            public class SP_MID_CHN_MOD_WK_READ_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_MOD_WK_READ.SQL"

                private tableParameter dt;
                private charParameter Rollup;
			
			    public SP_MID_CHN_MOD_WK_READ_def()
			    {
                    base.procedureName = "SP_MID_CHN_MOD_WK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHN_MOD_WK");
			        dt = new tableParameter("@dt", "MID_CHN_MOD_WK_READ_TYPE", base.inputParameterList);
			        Rollup = new charParameter("@Rollup", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable dt,
			                          char? Rollup
			                          )
			    {
                    lock (typeof(SP_MID_CHN_MOD_WK_READ_def))
                    {
                        this.dt.SetValue(dt);
                        this.Rollup.SetValue(Rollup);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_FOR_WK_READ_def SP_MID_CHN_FOR_WK_READ = new SP_MID_CHN_FOR_WK_READ_def();
            public class SP_MID_CHN_FOR_WK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_FOR_WK_READ.SQL"

                private tableParameter dt;
			
			    public SP_MID_CHN_FOR_WK_READ_def()
			    {
                    base.procedureName = "SP_MID_CHN_FOR_WK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHN_FOR_WK");
			        dt = new tableParameter("@dt", "MID_CHN_FOR_WK_READ_TYPE", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public DataTable Read(DatabaseAccess _dba, DataTable dt)
			    {
                    lock (typeof(SP_MID_CHN_FOR_WK_READ_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_FOR_WK_LOCK_READ_def SP_MID_CHN_FOR_WK_LOCK_READ = new SP_MID_CHN_FOR_WK_LOCK_READ_def();
            public class SP_MID_CHN_FOR_WK_LOCK_READ_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_FOR_WK_LOCK_READ.SQL"

                private tableParameter dt;
			
			    public SP_MID_CHN_FOR_WK_LOCK_READ_def()
			    {
			        base.procedureName = "SP_MID_CHN_FOR_WK_LOCK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("CHN_FOR_WK_LOCK");
			        dt = new tableParameter("@dt", "MID_CHN_FOR_WK_READ_LOCK_TYPE", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public DataTable Read(DatabaseAccess _dba, DataTable dt)
			    {
                    lock (typeof(SP_MID_CHN_FOR_WK_LOCK_READ_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_HIS_WK_WRITE_def SP_MID_CHN_HIS_WK_WRITE = new SP_MID_CHN_HIS_WK_WRITE_def();
            public class SP_MID_CHN_HIS_WK_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_HIS_WK_WRITE.SQL"

                private tableParameter dt;
			
			    public SP_MID_CHN_HIS_WK_WRITE_def()
			    {
                    base.procedureName = "SP_MID_CHN_HIS_WK_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("CHN_HIS_WK_WRITE");
			        dt = new tableParameter("@dt", "MID_CHN_HIS_WK_TYPE", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public int Insert(DatabaseAccess _dba, DataTable dt)
			    {
                    lock (typeof(SP_MID_CHN_HIS_WK_WRITE_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_FOR_WK_WRITE_def SP_MID_CHN_FOR_WK_WRITE = new SP_MID_CHN_FOR_WK_WRITE_def();
            public class SP_MID_CHN_FOR_WK_WRITE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_FOR_WK_WRITE.SQL"

                private tableParameter dt;
                private tableParameter dtLock;
                private charParameter SaveLocks;
			
			    public SP_MID_CHN_FOR_WK_WRITE_def()
			    {
                    base.procedureName = "SP_MID_CHN_FOR_WK_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("CHN_FOR_WK_WRITE");
			        dt = new tableParameter("@dt", "MID_CHN_FOR_WK_TYPE", base.inputParameterList);
			        dtLock = new tableParameter("@dtLock", "MID_CHN_FOR_WK_LOCK_TYPE", base.inputParameterList);
			        SaveLocks = new charParameter("@SaveLocks", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable dt,
			                      DataTable dtLock,
			                      char? SaveLocks
			                      )
			    {
                    lock (typeof(SP_MID_CHN_FOR_WK_WRITE_def))
                    {
                        this.dt.SetValue(dt);
                        this.dtLock.SetValue(dtLock);
                        this.SaveLocks.SetValue(SaveLocks);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE_def MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE = new MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE_def();
			public class MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_HISTORY_WEEK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_HISTORY_WEEK_DELETE_def MID_CHAIN_HISTORY_WEEK_DELETE = new MID_CHAIN_HISTORY_WEEK_DELETE_def();
			public class MID_CHAIN_HISTORY_WEEK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_HISTORY_WEEK_DELETE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_CHAIN_HISTORY_WEEK_DELETE_def()
			    {
			        base.procedureName = "MID_CHAIN_HISTORY_WEEK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_HISTORY_WEEK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_CHAIN_HISTORY_WEEK_DELETE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_def MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE = new MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_def();
            public class MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;

                public MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_def()
                {
                    base.procedureName = "MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_FORECAST_WEEK");
                    COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
                {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

			public static MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE = new MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def();
			public class MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE_def MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE = new MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE_def();
			public class MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE_def MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE = new MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE_def();
			public class MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE_def()
			    {
			        base.procedureName = "MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_def MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE = new MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE.SQL"

                private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE.SQL"

                private intParameter COMMIT_LIMIT;
                private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE_def MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE = new MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_DAY9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_ST_FOR_WK_LOCK_READ_def SP_MID_ST_FOR_WK_LOCK_READ = new SP_MID_ST_FOR_WK_LOCK_READ_def();
            public class SP_MID_ST_FOR_WK_LOCK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ST_FOR_WK_LOCK_READ.SQL"

                private tableParameter dt;
			
			    public SP_MID_ST_FOR_WK_LOCK_READ_def()
			    {
                    base.procedureName = "SP_MID_ST_FOR_WK_LOCK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ST_FOR_WK_LOCK");
			        dt = new tableParameter("@dt", "MID_ST_FOR_WK_READ_LOCK_TYPE", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public DataTable Read(DatabaseAccess _dba, DataTable dt)
			    {
                    lock (typeof(SP_MID_ST_FOR_WK_LOCK_READ_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_def MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE = new MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_def();
			public class MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE_def MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE = new MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE_def();
			public class MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE_def()
			    {
			        base.procedureName = "MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_HISTORY_WEEK9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_CHN_FOR_WK_DEL_ZERO_def SP_MID_CHN_FOR_WK_DEL_ZERO = new SP_MID_CHN_FOR_WK_DEL_ZERO_def();
            public class SP_MID_CHN_FOR_WK_DEL_ZERO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHN_FOR_WK_DEL_ZERO.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter RECORDS_DELETED; //Declare Output Parameter
			
			    public SP_MID_CHN_FOR_WK_DEL_ZERO_def()
			    {
                    base.procedureName = "SP_MID_CHN_FOR_WK_DEL_ZERO";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHN_FOR_WK_DEL_ZERO");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }

                [UnitTestMethodAttribute(BypassValidation=true)]
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_CHN_FOR_WK_DEL_ZERO_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RECORDS_DELETED.Value;
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK0");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK1");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK2");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK3");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK4");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK5");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK6");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK7");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK8");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK9");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE_def MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE = new MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE_def();
			public class MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter TIME_ID;
			
			    public MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE_def()
			    {
			        base.procedureName = "MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_FORECAST_WEEK_LOCK");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? COMMIT_LIMIT,
			                      int? TIME_ID
			                      )
			    {
                    lock (typeof(MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.TIME_ID.SetValue(TIME_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
