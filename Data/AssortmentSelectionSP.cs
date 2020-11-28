using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class AssortmentSelection : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_USER_ASSORTMENT_STORE_GRADE_INSERT_def MID_USER_ASSORTMENT_STORE_GRADE_INSERT = new MID_USER_ASSORTMENT_STORE_GRADE_INSERT_def();
			public class MID_USER_ASSORTMENT_STORE_GRADE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_STORE_GRADE_INSERT.SQL"

			    private intParameter USER_RID;
			    private intParameter STORE_GRADE_SEQ;
			    private intParameter BOUNDARY_UNITS;
			    private intParameter BOUNDARY_INDEX;
			    private stringParameter GRADE_CODE;
			
			    public MID_USER_ASSORTMENT_STORE_GRADE_INSERT_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_STORE_GRADE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_ASSORTMENT_STORE_GRADE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        STORE_GRADE_SEQ = new intParameter("@STORE_GRADE_SEQ", base.inputParameterList);
			        BOUNDARY_UNITS = new intParameter("@BOUNDARY_UNITS", base.inputParameterList);
			        BOUNDARY_INDEX = new intParameter("@BOUNDARY_INDEX", base.inputParameterList);
			        GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? STORE_GRADE_SEQ,
			                      int? BOUNDARY_UNITS,
			                      int? BOUNDARY_INDEX,
			                      string GRADE_CODE
			                      )
			    {
                    lock (typeof(MID_USER_ASSORTMENT_STORE_GRADE_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.STORE_GRADE_SEQ.SetValue(STORE_GRADE_SEQ);
                        this.BOUNDARY_UNITS.SetValue(BOUNDARY_UNITS);
                        this.BOUNDARY_INDEX.SetValue(BOUNDARY_INDEX);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_BASIS_INSERT_def MID_USER_ASSORTMENT_BASIS_INSERT = new MID_USER_ASSORTMENT_BASIS_INSERT_def();
			public class MID_USER_ASSORTMENT_BASIS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_BASIS_INSERT.SQL"

			    private intParameter USER_RID;
			    private intParameter BASIS_SEQ;
			    private intParameter HN_RID;
			    private intParameter FV_RID;
			    private intParameter CDR_RID;
                private decimalParameter WEIGHT;
			
			    public MID_USER_ASSORTMENT_BASIS_INSERT_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_BASIS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_ASSORTMENT_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        BASIS_SEQ = new intParameter("@BASIS_SEQ", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new decimalParameter("@WEIGHT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? BASIS_SEQ,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      decimal? WEIGHT
			                      )
			    {
                    lock (typeof(MID_USER_ASSORTMENT_BASIS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.BASIS_SEQ.SetValue(BASIS_SEQ);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_INSERT_def MID_USER_ASSORTMENT_INSERT = new MID_USER_ASSORTMENT_INSERT_def();
			public class MID_USER_ASSORTMENT_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_INSERT.SQL"

			    private intParameter USER_RID;
			    private intParameter SG_RID;
			    private intParameter GROUP_BY_ID;
			    private intParameter VIEW_RID;
			    private intParameter VARIABLE_TYPE;
			    private intParameter VARIABLE_NUMBER;
			    private charParameter INCL_ONHAND;
			    private charParameter INCL_INTRANSIT;
			    private charParameter INCL_SIMILAR_STORES;
			    private charParameter INCL_COMMITTED;
			    private intParameter AVERAGE_BY;
			    private intParameter GRADE_BOUNDARY_IND;
			
			    public MID_USER_ASSORTMENT_INSERT_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_ASSORTMENT");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        VARIABLE_TYPE = new intParameter("@VARIABLE_TYPE", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        INCL_ONHAND = new charParameter("@INCL_ONHAND", base.inputParameterList);
			        INCL_INTRANSIT = new charParameter("@INCL_INTRANSIT", base.inputParameterList);
			        INCL_SIMILAR_STORES = new charParameter("@INCL_SIMILAR_STORES", base.inputParameterList);
			        INCL_COMMITTED = new charParameter("@INCL_COMMITTED", base.inputParameterList);
			        AVERAGE_BY = new intParameter("@AVERAGE_BY", base.inputParameterList);
			        GRADE_BOUNDARY_IND = new intParameter("@GRADE_BOUNDARY_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? SG_RID,
			                      int? GROUP_BY_ID,
			                      int? VIEW_RID,
			                      int? VARIABLE_TYPE,
			                      int? VARIABLE_NUMBER,
			                      char? INCL_ONHAND,
			                      char? INCL_INTRANSIT,
			                      char? INCL_SIMILAR_STORES,
			                      char? INCL_COMMITTED,
			                      int? AVERAGE_BY,
			                      int? GRADE_BOUNDARY_IND
			                      )
			    {
                    lock (typeof(MID_USER_ASSORTMENT_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.VARIABLE_TYPE.SetValue(VARIABLE_TYPE);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.INCL_ONHAND.SetValue(INCL_ONHAND);
                        this.INCL_INTRANSIT.SetValue(INCL_INTRANSIT);
                        this.INCL_SIMILAR_STORES.SetValue(INCL_SIMILAR_STORES);
                        this.INCL_COMMITTED.SetValue(INCL_COMMITTED);
                        this.AVERAGE_BY.SetValue(AVERAGE_BY);
                        this.GRADE_BOUNDARY_IND.SetValue(GRADE_BOUNDARY_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_STORE_GRADE_DELETE_def MID_USER_ASSORTMENT_STORE_GRADE_DELETE = new MID_USER_ASSORTMENT_STORE_GRADE_DELETE_def();
			public class MID_USER_ASSORTMENT_STORE_GRADE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_STORE_GRADE_DELETE.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_STORE_GRADE_DELETE_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_STORE_GRADE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_ASSORTMENT_STORE_GRADE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_STORE_GRADE_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_BASIS_DELETE_def MID_USER_ASSORTMENT_BASIS_DELETE = new MID_USER_ASSORTMENT_BASIS_DELETE_def();
			public class MID_USER_ASSORTMENT_BASIS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_BASIS_DELETE.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_BASIS_DELETE_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_BASIS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_ASSORTMENT_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_BASIS_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_DELETE_def MID_USER_ASSORTMENT_DELETE = new MID_USER_ASSORTMENT_DELETE_def();
			public class MID_USER_ASSORTMENT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_DELETE.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_DELETE_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_ASSORTMENT");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_STORE_GRADE_READ_def MID_USER_ASSORTMENT_STORE_GRADE_READ = new MID_USER_ASSORTMENT_STORE_GRADE_READ_def();
			public class MID_USER_ASSORTMENT_STORE_GRADE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_STORE_GRADE_READ.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_STORE_GRADE_READ_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_STORE_GRADE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ASSORTMENT_STORE_GRADE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_STORE_GRADE_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_BASIS_READ_def MID_USER_ASSORTMENT_BASIS_READ = new MID_USER_ASSORTMENT_BASIS_READ_def();
			public class MID_USER_ASSORTMENT_BASIS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_BASIS_READ.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_BASIS_READ_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_BASIS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ASSORTMENT_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_BASIS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}



			public static MID_USER_ASSORTMENT_READ_def MID_USER_ASSORTMENT_READ = new MID_USER_ASSORTMENT_READ_def();
			public class MID_USER_ASSORTMENT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_READ.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_READ_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ASSORTMENT");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
