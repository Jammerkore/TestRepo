using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class TrendCaps : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_TREND_CAPS_DELETE_def MID_TREND_CAPS_DELETE = new MID_TREND_CAPS_DELETE_def();
			public class MID_TREND_CAPS_DELETE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TREND_CAPS_DELETE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			
			    public MID_TREND_CAPS_DELETE_def()
			    {
			        base.procedureName = "MID_TREND_CAPS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TREND_CAPS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID
			                      )
			    {
                    lock (typeof(MID_TREND_CAPS_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TREND_CAPS_INSERT_def MID_TREND_CAPS_INSERT = new MID_TREND_CAPS_INSERT_def();
			public class MID_TREND_CAPS_INSERT_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TREND_CAPS_INSERT.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter TREND_CAP_ID;
			    private floatParameter TOL_PCT;
			    private floatParameter HIGH_LIMIT;
			    private floatParameter LOW_LIMIT;
			
			    public MID_TREND_CAPS_INSERT_def()
			    {
			        base.procedureName = "MID_TREND_CAPS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TREND_CAPS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        TREND_CAP_ID = new intParameter("@TREND_CAP_ID", base.inputParameterList);
			        TOL_PCT = new floatParameter("@TOL_PCT", base.inputParameterList);
			        HIGH_LIMIT = new floatParameter("@HIGH_LIMIT", base.inputParameterList);
			        LOW_LIMIT = new floatParameter("@LOW_LIMIT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? TREND_CAP_ID,
			                      double? TOL_PCT,
			                      double? HIGH_LIMIT,
			                      double? LOW_LIMIT
			                      )
			    {
                    lock (typeof(MID_TREND_CAPS_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.TREND_CAP_ID.SetValue(TREND_CAP_ID);
                        this.TOL_PCT.SetValue(TOL_PCT);
                        this.HIGH_LIMIT.SetValue(HIGH_LIMIT);
                        this.LOW_LIMIT.SetValue(LOW_LIMIT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

				public static MID_TREND_CAPS_READ_def MID_TREND_CAPS_READ = new MID_TREND_CAPS_READ_def();
			public class MID_TREND_CAPS_READ_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TREND_CAPS_READ.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_TREND_CAPS_READ_def()
			    {
			        base.procedureName = "MID_TREND_CAPS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TREND_CAPS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_TREND_CAPS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL_def MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL = new MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL_def();
			public class MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			
			    public MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL_def()
			    {
			        base.procedureName = "MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TREND_CAPS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? SGL_RID
			                          )
			    {
                    lock (typeof(MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

				//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
