using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class OTSPlanSelection : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_USER_PLAN_READ_COUNT_def MID_USER_PLAN_READ_COUNT = new MID_USER_PLAN_READ_COUNT_def();
			public class MID_USER_PLAN_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_READ_COUNT.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_PLAN_READ_COUNT_def()
			    {
			        base.procedureName = "MID_USER_PLAN_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("USER_PLAN");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_READ_COUNT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_UPDATE_CUSTOM_OLL_def MID_USER_PLAN_UPDATE_CUSTOM_OLL = new MID_USER_PLAN_UPDATE_CUSTOM_OLL_def();
			public class MID_USER_PLAN_UPDATE_CUSTOM_OLL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_UPDATE_CUSTOM_OLL.SQL"

                private intParameter USER_RID;
                private intParameter CUSTOM_OLL_RID;
			
			    public MID_USER_PLAN_UPDATE_CUSTOM_OLL_def()
			    {
			        base.procedureName = "MID_USER_PLAN_UPDATE_CUSTOM_OLL";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_PLAN");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? CUSTOM_OLL_RID
			                      )
			    {
                    lock (typeof(MID_USER_PLAN_UPDATE_CUSTOM_OLL_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_READ_def MID_USER_PLAN_READ = new MID_USER_PLAN_READ_def();
			public class MID_USER_PLAN_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_READ.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_READ_def()
			    {
			        base.procedureName = "MID_USER_PLAN_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_PLAN");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_READ_def MID_USER_PLAN_BASIS_READ = new MID_USER_PLAN_BASIS_READ_def();
			public class MID_USER_PLAN_BASIS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_READ.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_BASIS_READ_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_PLAN_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_DETAILS_READ_def MID_USER_PLAN_BASIS_DETAILS_READ = new MID_USER_PLAN_BASIS_DETAILS_READ_def();
			public class MID_USER_PLAN_BASIS_DETAILS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_DETAILS_READ.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_BASIS_DETAILS_READ_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_DETAILS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_PLAN_BASIS_DETAILS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_DETAILS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}



			public static MID_USER_PLAN_READ_DISTINCT_STORE_def MID_USER_PLAN_READ_DISTINCT_STORE = new MID_USER_PLAN_READ_DISTINCT_STORE_def();
			public class MID_USER_PLAN_READ_DISTINCT_STORE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_READ_DISTINCT_STORE.SQL"

                private intParameter STORE_HN_RID;
			
			    public MID_USER_PLAN_READ_DISTINCT_STORE_def()
			    {
			        base.procedureName = "MID_USER_PLAN_READ_DISTINCT_STORE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_PLAN");
			        STORE_HN_RID = new intParameter("@STORE_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? STORE_HN_RID)
			    {
                    lock (typeof(MID_USER_PLAN_READ_DISTINCT_STORE_def))
                    {
                        this.STORE_HN_RID.SetValue(STORE_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_READ_DISTINCT_CHAIN_def MID_USER_PLAN_READ_DISTINCT_CHAIN = new MID_USER_PLAN_READ_DISTINCT_CHAIN_def();
			public class MID_USER_PLAN_READ_DISTINCT_CHAIN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_READ_DISTINCT_CHAIN.SQL"

                private intParameter CHAIN_HN_RID;
			
			    public MID_USER_PLAN_READ_DISTINCT_CHAIN_def()
			    {
			        base.procedureName = "MID_USER_PLAN_READ_DISTINCT_CHAIN";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_PLAN");
			        CHAIN_HN_RID = new intParameter("@CHAIN_HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? CHAIN_HN_RID)
			    {
                    lock (typeof(MID_USER_PLAN_READ_DISTINCT_CHAIN_def))
                    {
                        this.CHAIN_HN_RID.SetValue(CHAIN_HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_INSERT_def MID_USER_PLAN_INSERT = new MID_USER_PLAN_INSERT_def();
			public class MID_USER_PLAN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter SG_RID;
                private intParameter FILTER_RID;
                private intParameter GROUP_BY_ID;
                private intParameter VIEW_RID;
                private intParameter STORE_HN_RID;
                private intParameter STORE_FV_RID;
                private intParameter TIME_PERIOD_CDR_RID;
                private intParameter DISPLAY_TIME_BY_ID;
                private intParameter CHAIN_HN_RID;
                private intParameter CHAIN_FV_RID;
                private charParameter INCLUDE_INELIGIBLE_STORES_IND;
                private charParameter INCLUDE_SIMILAR_STORES_IND;
                private intParameter SESSION_TYPE;
                private intParameter LOW_LEVEL_FV_RID;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter LOW_LEVEL_SEQUENCE;
                private stringParameter CALC_MODE;
                private intParameter OLL_RID;
                private intParameter CUSTOM_OLL_RID;
                private intParameter IS_LADDER;
                private intParameter TOT_RIGHT;
                private intParameter IS_MULTI;
			
			    public MID_USER_PLAN_INSERT_def()
			    {
			        base.procedureName = "MID_USER_PLAN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_PLAN");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			        GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        STORE_HN_RID = new intParameter("@STORE_HN_RID", base.inputParameterList);
			        STORE_FV_RID = new intParameter("@STORE_FV_RID", base.inputParameterList);
			        TIME_PERIOD_CDR_RID = new intParameter("@TIME_PERIOD_CDR_RID", base.inputParameterList);
			        DISPLAY_TIME_BY_ID = new intParameter("@DISPLAY_TIME_BY_ID", base.inputParameterList);
			        CHAIN_HN_RID = new intParameter("@CHAIN_HN_RID", base.inputParameterList);
			        CHAIN_FV_RID = new intParameter("@CHAIN_FV_RID", base.inputParameterList);
			        INCLUDE_INELIGIBLE_STORES_IND = new charParameter("@INCLUDE_INELIGIBLE_STORES_IND", base.inputParameterList);
			        INCLUDE_SIMILAR_STORES_IND = new charParameter("@INCLUDE_SIMILAR_STORES_IND", base.inputParameterList);
			        SESSION_TYPE = new intParameter("@SESSION_TYPE", base.inputParameterList);
			        LOW_LEVEL_FV_RID = new intParameter("@LOW_LEVEL_FV_RID", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        LOW_LEVEL_SEQUENCE = new intParameter("@LOW_LEVEL_SEQUENCE", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
			        IS_LADDER = new intParameter("@IS_LADDER", base.inputParameterList);
			        TOT_RIGHT = new intParameter("@TOT_RIGHT", base.inputParameterList);
			        IS_MULTI = new intParameter("@IS_MULTI", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? SG_RID,
			                      int? FILTER_RID,
			                      int? GROUP_BY_ID,
			                      int? VIEW_RID,
			                      int? STORE_HN_RID,
			                      int? STORE_FV_RID,
			                      int? TIME_PERIOD_CDR_RID,
			                      int? DISPLAY_TIME_BY_ID,
			                      int? CHAIN_HN_RID,
			                      int? CHAIN_FV_RID,
			                      char? INCLUDE_INELIGIBLE_STORES_IND,
			                      char? INCLUDE_SIMILAR_STORES_IND,
			                      int? SESSION_TYPE,
			                      int? LOW_LEVEL_FV_RID,
			                      int? LOW_LEVEL_TYPE,
			                      int? LOW_LEVEL_OFFSET,
			                      int? LOW_LEVEL_SEQUENCE,
			                      string CALC_MODE,
			                      int? OLL_RID,
			                      int? CUSTOM_OLL_RID,
			                      int? IS_LADDER,
			                      int? TOT_RIGHT,
			                      int? IS_MULTI
			                      )
			    {
                    lock (typeof(MID_USER_PLAN_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.STORE_HN_RID.SetValue(STORE_HN_RID);
                        this.STORE_FV_RID.SetValue(STORE_FV_RID);
                        this.TIME_PERIOD_CDR_RID.SetValue(TIME_PERIOD_CDR_RID);
                        this.DISPLAY_TIME_BY_ID.SetValue(DISPLAY_TIME_BY_ID);
                        this.CHAIN_HN_RID.SetValue(CHAIN_HN_RID);
                        this.CHAIN_FV_RID.SetValue(CHAIN_FV_RID);
                        this.INCLUDE_INELIGIBLE_STORES_IND.SetValue(INCLUDE_INELIGIBLE_STORES_IND);
                        this.INCLUDE_SIMILAR_STORES_IND.SetValue(INCLUDE_SIMILAR_STORES_IND);
                        this.SESSION_TYPE.SetValue(SESSION_TYPE);
                        this.LOW_LEVEL_FV_RID.SetValue(LOW_LEVEL_FV_RID);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.LOW_LEVEL_SEQUENCE.SetValue(LOW_LEVEL_SEQUENCE);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        this.IS_LADDER.SetValue(IS_LADDER);
                        this.TOT_RIGHT.SetValue(TOT_RIGHT);
                        this.IS_MULTI.SetValue(IS_MULTI);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_INSERT_def MID_USER_PLAN_BASIS_INSERT = new MID_USER_PLAN_BASIS_INSERT_def();
			public class MID_USER_PLAN_BASIS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter BASIS_RID;
			
			    public MID_USER_PLAN_BASIS_INSERT_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_PLAN_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        BASIS_RID = new intParameter("@BASIS_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? BASIS_RID
			                      )
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.BASIS_RID.SetValue(BASIS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_DETAILS_INSERT_def MID_USER_PLAN_BASIS_DETAILS_INSERT = new MID_USER_PLAN_BASIS_DETAILS_INSERT_def();
			public class MID_USER_PLAN_BASIS_DETAILS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_DETAILS_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter BASIS_RID;
                private intParameter SEQ_ID;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter CDR_RID;
                private floatParameter WEIGHT;
                private charParameter IS_INCLUDED_IND;
			
			    public MID_USER_PLAN_BASIS_DETAILS_INSERT_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_DETAILS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("USER_PLAN_BASIS_DETAILS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        BASIS_RID = new intParameter("@BASIS_RID", base.inputParameterList);
			        SEQ_ID = new intParameter("@SEQ_ID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			        IS_INCLUDED_IND = new charParameter("@IS_INCLUDED_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      int? BASIS_RID,
			                      int? SEQ_ID,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      double? WEIGHT,
			                      char? IS_INCLUDED_IND
			                      )
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_DETAILS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.BASIS_RID.SetValue(BASIS_RID);
                        this.SEQ_ID.SetValue(SEQ_ID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        this.IS_INCLUDED_IND.SetValue(IS_INCLUDED_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_DELETE_FORM_USER_def MID_USER_PLAN_DELETE_FORM_USER = new MID_USER_PLAN_DELETE_FORM_USER_def();
			public class MID_USER_PLAN_DELETE_FORM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_DELETE_FORM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_DELETE_FORM_USER_def()
			    {
			        base.procedureName = "MID_USER_PLAN_DELETE_FORM_USER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_PLAN");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_DELETE_FORM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER_def MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER = new MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER_def();
			public class MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_PLAN_BASIS_DETAILS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_BASIS_DELETE_FORM_USER_def MID_USER_PLAN_BASIS_DELETE_FORM_USER = new MID_USER_PLAN_BASIS_DELETE_FORM_USER_def();
			public class MID_USER_PLAN_BASIS_DELETE_FORM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_BASIS_DELETE_FORM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_USER_PLAN_BASIS_DELETE_FORM_USER_def()
			    {
			        base.procedureName = "MID_USER_PLAN_BASIS_DELETE_FORM_USER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("USER_PLAN_BASIS");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_PLAN_BASIS_DELETE_FORM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
