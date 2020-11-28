using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StoreData : DataLayer
    {
        protected static class StoredProcedures
        {

          

			public static MID_STORE_REMOVAL_ANALYSIS_READ_ALL_def MID_STORE_REMOVAL_ANALYSIS_READ_ALL = new MID_STORE_REMOVAL_ANALYSIS_READ_ALL_def();
			public class MID_STORE_REMOVAL_ANALYSIS_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_REMOVAL_ANALYSIS_READ_ALL.SQL"

			
			    public MID_STORE_REMOVAL_ANALYSIS_READ_ALL_def()
			    {
			        base.procedureName = "MID_STORE_REMOVAL_ANALYSIS_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_REMOVAL_ANALYSIS");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_STORE_REMOVAL_ANALYSIS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORES_READ_ALL_def MID_STORES_READ_ALL = new MID_STORES_READ_ALL_def();
			public class MID_STORES_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_ALL.SQL"

			
			    public MID_STORES_READ_ALL_def()
			    {
			        base.procedureName = "MID_STORES_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_STORES_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORES_READ_FOR_DELETION_def MID_STORES_READ_FOR_DELETION = new MID_STORES_READ_FOR_DELETION_def();
			public class MID_STORES_READ_FOR_DELETION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_FOR_DELETION.SQL"

			
			    public MID_STORES_READ_FOR_DELETION_def()
			    {
			        base.procedureName = "MID_STORES_READ_FOR_DELETION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_STORES_READ_FOR_DELETION_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			


            //public static MID_STORE_GROUP_UPDATE_def MID_STORE_GROUP_UPDATE = new MID_STORE_GROUP_UPDATE_def();
            //public class MID_STORE_GROUP_UPDATE_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_UPDATE.SQL"

            //    private stringParameter SG_ID;
            //    private charParameter SG_DYNAMIC_GROUP_IND;
            //    private intParameter SG_RID;
			
            //    public MID_STORE_GROUP_UPDATE_def()
            //    {
            //        base.procedureName = "MID_STORE_GROUP_UPDATE";
            //        base.procedureType = storedProcedureTypes.Update;
            //        base.tableNames.Add("STORE_GROUP");
            //        SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
            //        SG_DYNAMIC_GROUP_IND = new charParameter("@SG_DYNAMIC_GROUP_IND", base.inputParameterList);
            //        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
            //    }
			
            //    public int Update(DatabaseAccess _dba, 
            //                      string SG_ID,
            //                      char? SG_DYNAMIC_GROUP_IND,
            //                      int? SG_RID
            //                      )
            //    {
            //        lock (typeof(MID_STORE_GROUP_UPDATE_def))
            //        {
            //            this.SG_ID.SetValue(SG_ID);
            //            this.SG_DYNAMIC_GROUP_IND.SetValue(SG_DYNAMIC_GROUP_IND);
            //            this.SG_RID.SetValue(SG_RID);
            //            return ExecuteStoredProcedureForUpdate(_dba);
            //        }
            //    }
            //}

			

			

        

           
            


	



          

		

			public static MID_STORE_CHAR_GROUP_READ_ALL_def MID_STORE_CHAR_GROUP_READ_ALL = new MID_STORE_CHAR_GROUP_READ_ALL_def();
			public class MID_STORE_CHAR_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_READ_ALL.SQL"

			
			    public MID_STORE_CHAR_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_STORE_CHAR_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_CHAR_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_STORE_CHAR_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

           
         



            


            public static MID_STORE_REMOVAL_DO_ANALYSIS_def MID_STORE_REMOVAL_DO_ANALYSIS = new MID_STORE_REMOVAL_DO_ANALYSIS_def();
			public class MID_STORE_REMOVAL_DO_ANALYSIS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_REMOVAL_DO_ANALYSIS.SQL"

			    private intParameter MINIMUM_DELETE_ROW_THRESHOLD;
			    private intParameter MAXIMUM_DELETE_ROW_THRESHOLD;
			    private floatParameter DELETION_PROCESS_PERCENTAGE_THRESHOLD;

                public MID_STORE_REMOVAL_DO_ANALYSIS_def()
			    {
			        base.procedureName = "MID_STORE_REMOVAL_DO_ANALYSIS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("STORE_REMOVAL_ANALYSIS");
			        MINIMUM_DELETE_ROW_THRESHOLD = new intParameter("@MINIMUM_DELETE_ROW_THRESHOLD", base.inputParameterList);
			        MAXIMUM_DELETE_ROW_THRESHOLD = new intParameter("@MAXIMUM_DELETE_ROW_THRESHOLD", base.inputParameterList);
			        DELETION_PROCESS_PERCENTAGE_THRESHOLD = new floatParameter("@DELETION_PROCESS_PERCENTAGE_THRESHOLD", base.inputParameterList);
			    }

                public void Execute(DatabaseAccess _dba, 
			                      int? MINIMUM_DELETE_ROW_THRESHOLD,
			                      int? MAXIMUM_DELETE_ROW_THRESHOLD,
			                      double? DELETION_PROCESS_PERCENTAGE_THRESHOLD
			                      )
			    {
                    lock (typeof(MID_STORE_REMOVAL_DO_ANALYSIS_def))
                    {
                        this.MINIMUM_DELETE_ROW_THRESHOLD.SetValue(MINIMUM_DELETE_ROW_THRESHOLD);
                        this.MAXIMUM_DELETE_ROW_THRESHOLD.SetValue(MAXIMUM_DELETE_ROW_THRESHOLD);
                        this.DELETION_PROCESS_PERCENTAGE_THRESHOLD.SetValue(DELETION_PROCESS_PERCENTAGE_THRESHOLD);
                        base.ExecuteStoredProcedureForMaintenance(_dba);
                    }
			    }
			}

          

            // Begin TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store
            public static MID_STORE_ALLOW_REMOVE_VSWID_def MID_STORE_ALLOW_REMOVE_VSWID = new MID_STORE_ALLOW_REMOVE_VSWID_def();
            public class MID_STORE_ALLOW_REMOVE_VSWID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_ALLOW_REMOVE_VSWID.SQL"

                private intParameter ST_RID;
                private intParameter ReturnCode; //Declare Output Parameter

                public MID_STORE_ALLOW_REMOVE_VSWID_def()
                {
                    base.procedureName = "MID_STORE_ALLOW_REMOVE_VSWID";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("VSW_RECS");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
                }

                public int ReadValue(DatabaseAccess _dba, int? ST_RID)
                {
                    lock (typeof(MID_STORE_ALLOW_REMOVE_VSWID_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForRead(_dba);
                        return (int)this.ReturnCode.Value;
                    }
                }
            }
            // End TT#4685 - JSmith - clear NODE_IMO table when VSW ID is removed from store

			
			public static MID_STORE_REMOVAL_ANALYSIS_UPDATE_def MID_STORE_REMOVAL_ANALYSIS_UPDATE = new MID_STORE_REMOVAL_ANALYSIS_UPDATE_def();
			public class MID_STORE_REMOVAL_ANALYSIS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_REMOVAL_ANALYSIS_UPDATE.SQL"

			    private stringParameter TABLE_NAME;
			    private intParameter COMPLETED;
			    private intParameter IN_PROGRESS;
			
			    public MID_STORE_REMOVAL_ANALYSIS_UPDATE_def()
			    {
			        base.procedureName = "MID_STORE_REMOVAL_ANALYSIS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("STORE_REMOVAL_ANALYSIS");
			        TABLE_NAME = new stringParameter("@TABLE_NAME", base.inputParameterList);
			        COMPLETED = new intParameter("@COMPLETED", base.inputParameterList);
			        IN_PROGRESS = new intParameter("@IN_PROGRESS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      string TABLE_NAME,
			                      int? COMPLETED,
			                      int? IN_PROGRESS
			                      )
			    {
                    lock (typeof(MID_STORE_REMOVAL_ANALYSIS_UPDATE_def))
                    {
                        this.TABLE_NAME.SetValue(TABLE_NAME);
                        this.COMPLETED.SetValue(COMPLETED);
                        this.IN_PROGRESS.SetValue(IN_PROGRESS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

	

            public static SP_MID_STORE_DELETE_SIM_STORE_CLEANUP_def SP_MID_STORE_DELETE_SIM_STORE_CLEANUP = new SP_MID_STORE_DELETE_SIM_STORE_CLEANUP_def();
            public class SP_MID_STORE_DELETE_SIM_STORE_CLEANUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_DELETE_SIM_STORE_CLEANUP.SQL"

			
			    public SP_MID_STORE_DELETE_SIM_STORE_CLEANUP_def()
			    {
			        base.procedureName = "SP_MID_STORE_DELETE_SIM_STORE_CLEANUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORES");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(SP_MID_STORE_DELETE_SIM_STORE_CLEANUP_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}



         
          
            public static SP_MID_STORE_MARK_FOR_DELETE_def SP_MID_STORE_MARK_FOR_DELETE = new SP_MID_STORE_MARK_FOR_DELETE_def();
            public class SP_MID_STORE_MARK_FOR_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_MARK_FOR_DELETE.SQL"

			    private intParameter ST_RID;
			    private charParameter MARK_FOR_DELETE;
			
			    public SP_MID_STORE_MARK_FOR_DELETE_def()
			    {
                    base.procedureName = "SP_MID_STORE_MARK_FOR_DELETE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("STORES");
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        MARK_FOR_DELETE = new charParameter("@MARK_FOR_DELETE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? ST_RID,
			                      char? MARK_FOR_DELETE
			                      )
			    {
                    lock (typeof(SP_MID_STORE_MARK_FOR_DELETE_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.MARK_FOR_DELETE.SetValue(MARK_FOR_DELETE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

           

 

         
            public static MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES_def MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES = new MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES_def();
            public class MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES.SQL"

                private intParameter BATCH_SIZE;
                private intParameter RC; //Declare Output Parameter

                public MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES_def()
                {
                    base.procedureName = "MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FILTER_CONDITION_LIST_VALUES");
                    BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
                    RC = new intParameter("@RC", base.outputParameterList); //Add Output Parameter
                }

                public int DeleteWithReturnCode(DatabaseAccess _dba, int? BATCH_SIZE)
                {
                    lock (typeof(MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES_def))
                    {
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.RC.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RC.Value;
                    }
                }
            }
            //End TT#1342-MD -jsobek -Store Filters - IN_USE_FILTER_XREF

            public static SP_MID_STORE_DELETE_SIMILAR_STORES_def SP_MID_STORE_DELETE_SIMILAR_STORES = new SP_MID_STORE_DELETE_SIMILAR_STORES_def();
            public class SP_MID_STORE_DELETE_SIMILAR_STORES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_DELETE_SIMILAR_STORES.SQL"

			    private intParameter BATCH_SIZE;
			    private intParameter RC; //Declare Output Parameter
			
			    public SP_MID_STORE_DELETE_SIMILAR_STORES_def()
			    {
                    base.procedureName = "SP_MID_STORE_DELETE_SIMILAR_STORES";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIMILAR_STORES");
			        BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
			        RC = new intParameter("@RC", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int DeleteWithReturnCode(DatabaseAccess _dba, int? BATCH_SIZE)
			    {
                    lock (typeof(SP_MID_STORE_DELETE_SIMILAR_STORES_def))
                    {
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.RC.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RC.Value;
                    }
			    }
			}

            public static SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE_def SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE = new SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE_def();
            public class SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE.SQL"

			    private intParameter BATCH_SIZE;
			    private intParameter RC; //Declare Output Parameter
			
			    public SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE_def()
			    {
                    base.procedureName = "SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("NODE_SIZE_CURVE_SIMILAR_STORE");
			        BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
			        RC = new intParameter("@RC", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int DeleteWithReturnCode(DatabaseAccess _dba, int? BATCH_SIZE)
			    {
                    lock (typeof(SP_MID_STORE_DELETE_NODE_SIZE_CURVE_SIMILAR_STORE_def))
                    {
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.RC.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RC.Value;
                    }
			    }
			}

            public static SP_MID_STORE_DELETE_SYSTEM_OPTIONS_def SP_MID_STORE_DELETE_SYSTEM_OPTIONS = new SP_MID_STORE_DELETE_SYSTEM_OPTIONS_def();
            public class SP_MID_STORE_DELETE_SYSTEM_OPTIONS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_DELETE_SYSTEM_OPTIONS.SQL"

			    private intParameter BATCH_SIZE;
			    private intParameter RC; //Declare Output Parameter
			
			    public SP_MID_STORE_DELETE_SYSTEM_OPTIONS_def()
			    {
                    base.procedureName = "SP_MID_STORE_DELETE_SYSTEM_OPTIONS";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SYSTEM_OPTIONS");
			        BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
			        RC = new intParameter("@RC", base.outputParameterList); //Add Output Parameter
			    }

                public int DeleteWithReturnCode(DatabaseAccess _dba, int? BATCH_SIZE)
			    {
                    lock (typeof(SP_MID_STORE_DELETE_SYSTEM_OPTIONS_def))
                    {
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.RC.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RC.Value;
                    }
			    }
			}

            public static SP_MID_STORE_REMOVAL_STORES_def SP_MID_STORE_REMOVAL_STORES = new SP_MID_STORE_REMOVAL_STORES_def();
            public class SP_MID_STORE_REMOVAL_STORES_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_REMOVAL_STORES.SQL"

			    private intParameter BATCH_SIZE;
			    private intParameter RC; //Declare Output Parameter
			
			    public SP_MID_STORE_REMOVAL_STORES_def()
			    {
                    base.procedureName = "SP_MID_STORE_REMOVAL_STORES";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORES");
			        BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
			        RC = new intParameter("@RC", base.outputParameterList); //Add Output Parameter
			    }

                public int DeleteWithReturnCode(DatabaseAccess _dba, int? BATCH_SIZE)
			    {
                    lock (typeof(SP_MID_STORE_REMOVAL_STORES_def))
                    {
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.RC.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RC.Value;
                    }
			    }
			}

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_GET_STORE_ALLOCATION_COUNT_def MID_GET_STORE_ALLOCATION_COUNT = new MID_GET_STORE_ALLOCATION_COUNT_def();
            public class MID_GET_STORE_ALLOCATION_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_STORE_ALLOCATION_COUNT.SQL"

                private intParameter ST_RID;
                private intParameter COUNT; //Declare Output Parameter

                public MID_GET_STORE_ALLOCATION_COUNT_def()
                {
                    base.procedureName = "MID_GET_STORE_ALLOCATION_COUNT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("TOTAL_ALLOCATION");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    COUNT = new intParameter("@COUNT", base.outputParameterList); //Add Output Parameter
                }

                public DataTable Read(DatabaseAccess _dba, ref int count, int? ST_RID)
                {
                    lock (typeof(MID_GET_STORE_ALLOCATION_COUNT_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.COUNT.SetValue(0); //Initialize Output Parameter
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        count = (int)this.COUNT.Value;
                        return dt;
                    }
                }
            }

            public static MID_GET_STORE_INTRANSIT_COUNT_def MID_GET_STORE_INTRANSIT_COUNT = new MID_GET_STORE_INTRANSIT_COUNT_def();
            public class MID_GET_STORE_INTRANSIT_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_STORE_INTRANSIT_COUNT.SQL"

                private intParameter ST_RID;
                private intParameter COUNT; //Declare Output Parameter

                public MID_GET_STORE_INTRANSIT_COUNT_def()
                {
                    base.procedureName = "MID_GET_STORE_INTRANSIT_COUNT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_INTRANSIT");
                    base.tableNames.Add("STORE_EXTERNAL_INTRANSIT");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    COUNT = new intParameter("@COUNT", base.outputParameterList); //Add Output Parameter
                }

                public DataTable Read(DatabaseAccess _dba, ref int count, int? ST_RID)
                {
                    lock (typeof(MID_GET_STORE_INTRANSIT_COUNT_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.COUNT.SetValue(0); //Initialize Output Parameter
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        count = (int)this.COUNT.Value;
                        return dt;
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
