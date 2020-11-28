using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class RollupData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL_def MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL = new MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL_def();
			public class MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL.SQL"

			    private intParameter PROCESS;
			    private intParameter PH_RID;
			    private intParameter ITEM_TYPE;
			    private intParameter FV_RID;
			
			    public MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL";
			        base.procedureType = storedProcedureTypes.ScalarValue;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public object ReadValue(DatabaseAccess _dba, 
			                             int? PROCESS,
			                             int? PH_RID,
			                             int? ITEM_TYPE,
			                             int? FV_RID
			                             )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL_def MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL = new MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL_def();
			public class MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter ITEM_TYPE;
                private intParameter FV_RID;
			
			    public MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL";
			        base.procedureType = storedProcedureTypes.ScalarValue;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public object ReadValue(DatabaseAccess _dba, 
			                             int? PROCESS,
			                             int? PH_RID,
			                             int? ITEM_TYPE,
			                             int? FV_RID
			                             )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED_def MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED = new MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED_def();
			public class MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
			
			    public MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? PROCESS,
			                               int? PH_RID
			                               )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_READ_COUNT_def MID_ROLLUP_ITEM_READ_COUNT = new MID_ROLLUP_ITEM_READ_COUNT_def();
			public class MID_ROLLUP_ITEM_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_COUNT.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter FV_RID;
                private intParameter ITEM_TYPE;
                private intParameter HOME_LEVEL;
			
			    public MID_ROLLUP_ITEM_READ_COUNT_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? PROCESS,
			                               int? PH_RID,
			                               int? FV_RID,
			                               int? ITEM_TYPE,
			                               int? HOME_LEVEL
			                               )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_COUNT_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_READ_BATCH_NUMBERS_def MID_ROLLUP_ITEM_READ_BATCH_NUMBERS = new MID_ROLLUP_ITEM_READ_BATCH_NUMBERS_def();
			public class MID_ROLLUP_ITEM_READ_BATCH_NUMBERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_BATCH_NUMBERS.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter FV_RID;
                private intParameter ITEM_TYPE;
                private intParameter HOME_LEVEL;
			
			    public MID_ROLLUP_ITEM_READ_BATCH_NUMBERS_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_BATCH_NUMBERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? PROCESS,
			                          int? PH_RID,
			                          int? FV_RID,
			                          int? ITEM_TYPE,
			                          int? HOME_LEVEL
			                          )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_BATCH_NUMBERS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_BUILD_ROLLUP_DCLR_ITEMS_def SP_MID_BUILD_ROLLUP_DCLR_ITEMS = new SP_MID_BUILD_ROLLUP_DCLR_ITEMS_def();
            public class SP_MID_BUILD_ROLLUP_DCLR_ITEMS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_BUILD_ROLLUP_DCLR_ITEMS.SQL"

                private intParameter PROCESS;
                private intParameter debug;
			
			    public SP_MID_BUILD_ROLLUP_DCLR_ITEMS_def()
			    {
                    base.procedureName = "SP_MID_BUILD_ROLLUP_DCLR_ITEMS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BUILD_ROLLUP_DCLR_ITEMS");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, int? PROCESS)
			    {
                    lock (typeof(SP_MID_BUILD_ROLLUP_DCLR_ITEMS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_ROLLUP_PROCESS_INSERT_def SP_MID_ROLLUP_PROCESS_INSERT = new SP_MID_ROLLUP_PROCESS_INSERT_def();
            public class SP_MID_ROLLUP_PROCESS_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ROLLUP_PROCESS_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter ROLLUP_TYPE;
                private intParameter STATUS_CODE;
                private intParameter BATCH_NUMBER;
                private datetimeParameter START_TIME;
                private intParameter ROLLUP_RID; //Declare Output Parameter
			
			    public SP_MID_ROLLUP_PROCESS_INSERT_def()
			    {
                    base.procedureName = "SP_MID_ROLLUP_PROCESS_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("ROLLUP_PROCESS");
			        PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
			        ROLLUP_TYPE = new intParameter("@ROLLUP_TYPE", base.inputParameterList);
			        STATUS_CODE = new intParameter("@STATUS_CODE", base.inputParameterList);
			        BATCH_NUMBER = new intParameter("@BATCH_NUMBER", base.inputParameterList);
			        START_TIME = new datetimeParameter("@START_TIME", base.inputParameterList);
			        ROLLUP_RID = new intParameter("@ROLLUP_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? PROCESS_RID,
			                                  int? ROLLUP_TYPE,
			                                  int? STATUS_CODE,
			                                  int? BATCH_NUMBER,
			                                  DateTime? START_TIME
			                                  )
			    {
                    lock (typeof(SP_MID_ROLLUP_PROCESS_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.ROLLUP_TYPE.SetValue(ROLLUP_TYPE);
                        this.STATUS_CODE.SetValue(STATUS_CODE);
                        this.BATCH_NUMBER.SetValue(BATCH_NUMBER);
                        this.START_TIME.SetValue(START_TIME);
                        this.ROLLUP_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_PROCESS_UPDATE_def MID_ROLLUP_PROCESS_UPDATE = new MID_ROLLUP_PROCESS_UPDATE_def();
			public class MID_ROLLUP_PROCESS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_PROCESS_UPDATE.SQL"

                private intParameter ROLLUP_RID;
                private intParameter STATUS_CODE;
                private datetimeParameter STOP_TIME;
			
			    public MID_ROLLUP_PROCESS_UPDATE_def()
			    {
			        base.procedureName = "MID_ROLLUP_PROCESS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("ROLLUP_PROCESS");
			        ROLLUP_RID = new intParameter("@ROLLUP_RID", base.inputParameterList);
			        STATUS_CODE = new intParameter("@STATUS_CODE", base.inputParameterList);
			        STOP_TIME = new datetimeParameter("@STOP_TIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? ROLLUP_RID,
			                      int? STATUS_CODE,
                                  DateTime? STOP_TIME
			                      )
			    {
                    lock (typeof(MID_ROLLUP_PROCESS_UPDATE_def))
                    {
                        this.ROLLUP_RID.SetValue(ROLLUP_RID);
                        this.STATUS_CODE.SetValue(STATUS_CODE);
                        this.STOP_TIME.SetValue(STOP_TIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_SET_ROLLUP_BATCHES_def SP_MID_SET_ROLLUP_BATCHES = new SP_MID_SET_ROLLUP_BATCHES_def();
            public class SP_MID_SET_ROLLUP_BATCHES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SET_ROLLUP_BATCHES.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter ITEM_TYPE;
                private intParameter FV_RID;
                private intParameter BATCH_SIZE;
                private intParameter TABLE_NUMBER;
                private intParameter BATCH_NUMBER;
                private intParameter debug;
                private intParameter BATCH_COUNT; //Declare Output Parameter
			
			    public SP_MID_SET_ROLLUP_BATCHES_def()
			    {
                    base.procedureName = "SP_MID_SET_ROLLUP_BATCHES";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SET_ROLLUP_BATCHES");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        BATCH_SIZE = new intParameter("@BATCH_SIZE", base.inputParameterList);
			        TABLE_NUMBER = new intParameter("@TABLE_NUMBER", base.inputParameterList);
			        BATCH_NUMBER = new intParameter("@BATCH_NUMBER", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        BATCH_COUNT = new intParameter("@BATCH_COUNT", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? PH_RID,
			                      int? HOME_LEVEL,
			                      int? ITEM_TYPE,
			                      int? FV_RID,
			                      int? BATCH_SIZE,
			                      int? TABLE_NUMBER,
			                      int? BATCH_NUMBER
			                      )
			    {
                    lock (typeof(SP_MID_SET_ROLLUP_BATCHES_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FV_RID.SetValue(FV_RID);
                        this.BATCH_SIZE.SetValue(BATCH_SIZE);
                        this.TABLE_NUMBER.SetValue(TABLE_NUMBER);
                        this.BATCH_NUMBER.SetValue(BATCH_NUMBER);
                        this.debug.SetValue(0);
                        this.BATCH_COUNT.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)this.BATCH_COUNT.Value;
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR_def MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR = new MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR_def();
			public class MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR.SQL"

                private intParameter PROCESS;
                private intParameter FV_RID;
                private intParameter ITEM_TYPE;
                private intParameter PH_RID;
                private intParameter HOME_LEVEL;
			
			    public MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? FV_RID,
			                      int? ITEM_TYPE,
			                      int? PH_RID,
			                      int? HOME_LEVEL
			                      )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.FV_RID.SetValue(FV_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.PH_RID.SetValue(PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS_def MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS = new MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS_def();
			public class MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS.SQL"

			
			    public MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("ROLLUP_ITEM");
			    }
			
			    public int Update(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS_def))
                    {
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}


			public static MID_ROLLUP_ITEM_READ_VERSIONS_def MID_ROLLUP_ITEM_READ_VERSIONS = new MID_ROLLUP_ITEM_READ_VERSIONS_def();
			public class MID_ROLLUP_ITEM_READ_VERSIONS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_VERSIONS.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter ITEM_TYPE;
			
			    public MID_ROLLUP_ITEM_READ_VERSIONS_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_VERSIONS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? PROCESS,
			                          int? PH_RID,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_VERSIONS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_READ_TYPES_def MID_ROLLUP_ITEM_READ_TYPES = new MID_ROLLUP_ITEM_READ_TYPES_def();
			public class MID_ROLLUP_ITEM_READ_TYPES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_TYPES.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
			
			    public MID_ROLLUP_ITEM_READ_TYPES_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_READ_TYPES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? PROCESS,
			                          int? PH_RID
			                          )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_READ_TYPES_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            // Begin TT#2131-MD - JSmith - Halo Integration
            public static MID_ROLLUP_ITEM_READ_PROCESSED_def MID_ROLLUP_ITEM_READ_PROCESSED = new MID_ROLLUP_ITEM_READ_PROCESSED_def();
            public class MID_ROLLUP_ITEM_READ_PROCESSED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_READ_PROCESSED.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;

                public MID_ROLLUP_ITEM_READ_PROCESSED_def()
                {
                    base.procedureName = "MID_ROLLUP_ITEM_READ_PROCESSED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("ROLLUP_ITEM");
                    PROCESS = new intParameter("@PROCESS", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? PROCESS,
                                      int? PH_RID
                                      )
                {
                    lock (typeof(MID_ROLLUP_ITEM_READ_PROCESSED_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // End TT#2131-MD - JSmith - Halo Integration

            public static MID_ROLLUP_ITEM_INSERT_def MID_ROLLUP_ITEM_INSERT = new MID_ROLLUP_ITEM_INSERT_def();
			public class MID_ROLLUP_ITEM_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_INSERT.SQL"

                private intParameter PROCESS;
                private intParameter HN_RID;
                private intParameter PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter TIME_ID;
                private intParameter ITEM_TYPE;
                private intParameter FV_RID;
			
			    public MID_ROLLUP_ITEM_INSERT_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? HN_RID,
			                      int? PH_RID,
			                      int? HOME_LEVEL,
			                      int? TIME_ID,
			                      int? ITEM_TYPE,
			                      int? FV_RID
			                      )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_INSERT_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_RID.SetValue(PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_XML_ROLLUP_ITEM_WRITE_def SP_MID_XML_ROLLUP_ITEM_WRITE = new SP_MID_XML_ROLLUP_ITEM_WRITE_def();
            public class SP_MID_XML_ROLLUP_ITEM_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_ROLLUP_ITEM_WRITE.SQL"

                private stringParameter xmlDoc;
			
			    public SP_MID_XML_ROLLUP_ITEM_WRITE_def()
			    {
                    base.procedureName = "SP_MID_XML_ROLLUP_ITEM_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("ROLLUP_ITEM");
			        xmlDoc = new stringParameter("@xmlDoc", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, string xmlDoc)
			    {
                    lock (typeof(SP_MID_XML_ROLLUP_ITEM_WRITE_def))
                    {
                        this.xmlDoc.SetValue(xmlDoc);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_DELETE_def MID_ROLLUP_ITEM_DELETE = new MID_ROLLUP_ITEM_DELETE_def();
			public class MID_ROLLUP_ITEM_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_DELETE.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
			
			    public MID_ROLLUP_ITEM_DELETE_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? PH_RID
			                      )
			    {
                    lock (typeof(MID_ROLLUP_ITEM_DELETE_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_DELETE_FROM_PROCESS_def MID_ROLLUP_ITEM_DELETE_FROM_PROCESS = new MID_ROLLUP_ITEM_DELETE_FROM_PROCESS_def();
			public class MID_ROLLUP_ITEM_DELETE_FROM_PROCESS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_DELETE_FROM_PROCESS.SQL"

                private intParameter PROCESS;
			
			    public MID_ROLLUP_ITEM_DELETE_FROM_PROCESS_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_DELETE_FROM_PROCESS";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ROLLUP_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? PROCESS)
			    {
                    lock (typeof(MID_ROLLUP_ITEM_DELETE_FROM_PROCESS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS_def MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS = new MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS_def();
			public class MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS.SQL"

                private charParameter ALTERNATES_ONLY;
			
			    public MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS_def()
			    {
			        base.procedureName = "MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ROLLUP_ITEM");
			        ALTERNATES_ONLY = new charParameter("@ALTERNATES_ONLY", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, char? ALTERNATES_ONLY)
			    {
                    lock (typeof(MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS_def))
                    {
                        this.ALTERNATES_ONLY.SetValue(ALTERNATES_ONLY);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_BUILD_ROLLUP_HIER_ITEMS_def SP_MID_BUILD_ROLLUP_HIER_ITEMS = new SP_MID_BUILD_ROLLUP_HIER_ITEMS_def();
            public class SP_MID_BUILD_ROLLUP_HIER_ITEMS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_BUILD_ROLLUP_HIER_ITEMS.SQL"

                private intParameter PROCESS;
                private charParameter AlternatesOnly;
                private intParameter debug;
			
			    public SP_MID_BUILD_ROLLUP_HIER_ITEMS_def()
			    {
                    base.procedureName = "SP_MID_BUILD_ROLLUP_HIER_ITEMS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BUILD_ROLLUP_HIER_ITEMS");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        AlternatesOnly = new charParameter("@AlternatesOnly", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      char? AlternatesOnly
			                      )
			    {
                    lock (typeof(SP_MID_BUILD_ROLLUP_HIER_ITEMS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.AlternatesOnly.SetValue(AlternatesOnly);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_BUILD_ALT_ROLLUP_ITEMS_def SP_MID_BUILD_ALT_ROLLUP_ITEMS = new SP_MID_BUILD_ALT_ROLLUP_ITEMS_def();
            public class SP_MID_BUILD_ALT_ROLLUP_ITEMS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_BUILD_ALT_ROLLUP_ITEMS.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
				// Begin TT#5485 - JSmith - Performance
                //private intParameter TIME_ID;
                private stringParameter TIME_ID;
				// End TT#5485 - JSmith - Performance
                private intParameter ITEM_TYPE;
                private intParameter FROM_LEVEL;
                private intParameter TO_LEVEL;
                private intParameter FIRST_DAY_OF_WEEK;
                private intParameter LAST_DAY_OF_WEEK;
                private intParameter FIRST_DAY_OF_NEXT_WEEK;
			
			    public SP_MID_BUILD_ALT_ROLLUP_ITEMS_def()
			    {
                    base.procedureName = "SP_MID_BUILD_ALT_ROLLUP_ITEMS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BUILD_ALT_ROLLUP_ITEMS");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
					// Begin TT#5485 - JSmith - Performance
                    //TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
                    TIME_ID = new stringParameter("@TIME_ID", base.inputParameterList);
					// End TT#5485 - JSmith - Performance
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FROM_LEVEL = new intParameter("@FROM_LEVEL", base.inputParameterList);
			        TO_LEVEL = new intParameter("@TO_LEVEL", base.inputParameterList);
			        FIRST_DAY_OF_WEEK = new intParameter("@FIRST_DAY_OF_WEEK", base.inputParameterList);
			        LAST_DAY_OF_WEEK = new intParameter("@LAST_DAY_OF_WEEK", base.inputParameterList);
			        FIRST_DAY_OF_NEXT_WEEK = new intParameter("@FIRST_DAY_OF_NEXT_WEEK", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? PH_RID,
			                      int? HN_RID,
			                      int? FV_RID,
								  // Begin TT#5485 - JSmith - Performance
                                  //int? TIME_ID,
                                  string TIME_ID,
								  // End TT#5485 - JSmith - Performance
			                      int? ITEM_TYPE,
			                      int? FROM_LEVEL,
			                      int? TO_LEVEL,
			                      int? FIRST_DAY_OF_WEEK,
			                      int? LAST_DAY_OF_WEEK,
			                      int? FIRST_DAY_OF_NEXT_WEEK
			                      )
			    {
                    lock (typeof(SP_MID_BUILD_ALT_ROLLUP_ITEMS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FROM_LEVEL.SetValue(FROM_LEVEL);
                        this.TO_LEVEL.SetValue(TO_LEVEL);
                        this.FIRST_DAY_OF_WEEK.SetValue(FIRST_DAY_OF_WEEK);
                        this.LAST_DAY_OF_WEEK.SetValue(LAST_DAY_OF_WEEK);
                        this.FIRST_DAY_OF_NEXT_WEEK.SetValue(FIRST_DAY_OF_NEXT_WEEK);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_BUILD_ROLLUP_ITEMS_def SP_MID_BUILD_ROLLUP_ITEMS = new SP_MID_BUILD_ROLLUP_ITEMS_def();
            public class SP_MID_BUILD_ROLLUP_ITEMS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_BUILD_ROLLUP_ITEMS.SQL"

                private intParameter PROCESS;
                private intParameter PH_RID;
                private intParameter HN_RID;
                private intParameter FV_RID;
				// Begin TT#5485 - JSmith - Performance
                //private intParameter TIME_ID;
                private stringParameter TIME_ID;
				// End TT#5485 - JSmith - Performance
                private intParameter ITEM_TYPE;
                private intParameter FROM_LEVEL;
                private intParameter TO_LEVEL;
                private intParameter FIRST_DAY_OF_WEEK;
                private intParameter LAST_DAY_OF_WEEK;
                private intParameter FIRST_DAY_OF_NEXT_WEEK;
			
			    public SP_MID_BUILD_ROLLUP_ITEMS_def()
			    {
                    base.procedureName = "SP_MID_BUILD_ROLLUP_ITEMS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BUILD_ROLLUP_ITEMS");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
					// Begin TT#5485 - JSmith - Performance
                    //TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
                    TIME_ID = new stringParameter("@TIME_ID", base.inputParameterList);
					// End TT#5485 - JSmith - Performance
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        FROM_LEVEL = new intParameter("@FROM_LEVEL", base.inputParameterList);
			        TO_LEVEL = new intParameter("@TO_LEVEL", base.inputParameterList);
			        FIRST_DAY_OF_WEEK = new intParameter("@FIRST_DAY_OF_WEEK", base.inputParameterList);
			        LAST_DAY_OF_WEEK = new intParameter("@LAST_DAY_OF_WEEK", base.inputParameterList);
			        FIRST_DAY_OF_NEXT_WEEK = new intParameter("@FIRST_DAY_OF_NEXT_WEEK", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? PH_RID,
			                      int? HN_RID,
			                      int? FV_RID,
								  // Begin TT#5485 - JSmith - Performance
                                  //int? TIME_ID,
                                  string TIME_ID,
								  // End TT#5485 - JSmith - Performance
			                      int? ITEM_TYPE,
			                      int? FROM_LEVEL,
			                      int? TO_LEVEL,
			                      int? FIRST_DAY_OF_WEEK,
			                      int? LAST_DAY_OF_WEEK,
			                      int? FIRST_DAY_OF_NEXT_WEEK
			                      )
			    {
                    lock (typeof(SP_MID_BUILD_ROLLUP_ITEMS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.PH_RID.SetValue(PH_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.FROM_LEVEL.SetValue(FROM_LEVEL);
                        this.TO_LEVEL.SetValue(TO_LEVEL);
                        this.FIRST_DAY_OF_WEEK.SetValue(FIRST_DAY_OF_WEEK);
                        this.LAST_DAY_OF_WEEK.SetValue(LAST_DAY_OF_WEEK);
                        this.FIRST_DAY_OF_NEXT_WEEK.SetValue(FIRST_DAY_OF_NEXT_WEEK);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
