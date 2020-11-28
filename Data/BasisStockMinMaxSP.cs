using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StockMinMax : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_STOCK_MIN_MAX_DELETE_def MID_STOCK_MIN_MAX_DELETE = new MID_STOCK_MIN_MAX_DELETE_def();
			public class MID_STOCK_MIN_MAX_DELETE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_DELETE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter BOUNDARY;
			
			    public MID_STOCK_MIN_MAX_DELETE_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? BOUNDARY
			                      )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_DELETE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL_def MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL = new MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL_def();
			public class MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter HN_RID;
			
			    public MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_DELETE_FROM_NODE_def MID_STOCK_MIN_MAX_DELETE_FROM_NODE = new MID_STOCK_MIN_MAX_DELETE_FROM_NODE_def();
			public class MID_STOCK_MIN_MAX_DELETE_FROM_NODE_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_DELETE_FROM_NODE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter HN_RID;
			
			    public MID_STOCK_MIN_MAX_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_DELETE_FROM_NODE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_INSERT_def MID_STOCK_MIN_MAX_INSERT = new MID_STOCK_MIN_MAX_INSERT_def();
			public class MID_STOCK_MIN_MAX_INSERT_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_INSERT.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter BOUNDARY;
			    private intParameter HN_RID;
			    private intParameter CDR_RID;
			    private intParameter MIN_STOCK;
			    private intParameter MAX_STOCK;
			
			    public MID_STOCK_MIN_MAX_INSERT_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        MIN_STOCK = new intParameter("@MIN_STOCK", base.inputParameterList);
			        MAX_STOCK = new intParameter("@MAX_STOCK", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? METHOD_RID,
			                      int? SGL_RID,
			                      int? BOUNDARY,
			                      int? HN_RID,
			                      int? CDR_RID,
			                      int? MIN_STOCK,
			                      int? MAX_STOCK
			                      )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.HN_RID.SetValue(HN_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MIN_STOCK.SetValue(MIN_STOCK);
                        this.MAX_STOCK.SetValue(MAX_STOCK);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_READ_def MID_STOCK_MIN_MAX_READ = new MID_STOCK_MIN_MAX_READ_def();
			public class MID_STOCK_MIN_MAX_READ_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_READ.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_STOCK_MIN_MAX_READ_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL_def MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL = new MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL_def();
			public class MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			
			    public MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? SGL_RID
			                          )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STOCK_MIN_MAX_READ_FROM_NODE_def MID_STOCK_MIN_MAX_READ_FROM_NODE = new MID_STOCK_MIN_MAX_READ_FROM_NODE_def();
			public class MID_STOCK_MIN_MAX_READ_FROM_NODE_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MIN_MAX_READ_FROM_NODE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			    private intParameter HN_RID;
			
			    public MID_STOCK_MIN_MAX_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_STOCK_MIN_MAX_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STOCK_MIN_MAX");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? METHOD_RID,
			                          int? SGL_RID,
			                          int? HN_RID
			                          )
			    {
                    lock (typeof(MID_STOCK_MIN_MAX_READ_FROM_NODE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE

        }
    }  
}
