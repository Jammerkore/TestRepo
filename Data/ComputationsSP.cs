using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ComputationData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_COMPUTATION_MODEL_INSERT_def SP_MID_COMPUTATION_MODEL_INSERT = new SP_MID_COMPUTATION_MODEL_INSERT_def();
			public class SP_MID_COMPUTATION_MODEL_INSERT_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_COMPUTATION_MODEL_INSERT.SQL"

			    private stringParameter MODEL_ID;
			    private stringParameter CALC_MODE;
			    private intParameter MODEL_RID; //Declare Output Parameter
			
			    public SP_MID_COMPUTATION_MODEL_INSERT_def()
			    {
			        base.procedureName = "SP_MID_COMPUTATION_MODEL_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("COMPUTATION_MODEL");
			        MODEL_ID = new stringParameter("@MODEL_ID", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        MODEL_RID = new intParameter("@MODEL_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string MODEL_ID,
			                                  string CALC_MODE
			                                  )
			    {
                    lock (typeof(SP_MID_COMPUTATION_MODEL_INSERT_def))
                    {
                        this.MODEL_ID.SetValue(MODEL_ID);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.MODEL_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_MODEL_UPDATE_def MID_COMPUTATION_MODEL_UPDATE = new MID_COMPUTATION_MODEL_UPDATE_def();
			public class MID_COMPUTATION_MODEL_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_UPDATE.SQL"

			    private intParameter COMP_MODEL_RID;
			    private stringParameter COMP_MODEL_ID;
			    private stringParameter CALC_MODE;
			
			    public MID_COMPUTATION_MODEL_UPDATE_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("COMPUTATION_MODEL");
			        COMP_MODEL_RID = new intParameter("@COMP_MODEL_RID", base.inputParameterList);
			        COMP_MODEL_ID = new stringParameter("@COMP_MODEL_ID", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? COMP_MODEL_RID,
			                      string COMP_MODEL_ID,
			                      string CALC_MODE
			                      )
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_UPDATE_def))
                    {
                        this.COMP_MODEL_RID.SetValue(COMP_MODEL_RID);
                        this.COMP_MODEL_ID.SetValue(COMP_MODEL_ID);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_MODEL_READ_ALL_def MID_COMPUTATION_MODEL_READ_ALL = new MID_COMPUTATION_MODEL_READ_ALL_def();
			public class MID_COMPUTATION_MODEL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_READ_ALL.SQL"
		
			    public MID_COMPUTATION_MODEL_READ_ALL_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_MODEL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_MODEL_READ_def MID_COMPUTATION_MODEL_READ = new MID_COMPUTATION_MODEL_READ_def();
			public class MID_COMPUTATION_MODEL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_READ.SQL"

			    private intParameter COMP_MODEL_RID;
			
			    public MID_COMPUTATION_MODEL_READ_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_MODEL");
			        COMP_MODEL_RID = new intParameter("@COMP_MODEL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? COMP_MODEL_RID)
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_READ_def))
                    {
                        this.COMP_MODEL_RID.SetValue(COMP_MODEL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_MODEL_READ_FROM_ID_def MID_COMPUTATION_MODEL_READ_FROM_ID = new MID_COMPUTATION_MODEL_READ_FROM_ID_def();
			public class MID_COMPUTATION_MODEL_READ_FROM_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_READ_FROM_ID.SQL"

			    private stringParameter COMP_MODEL_ID;
			
			    public MID_COMPUTATION_MODEL_READ_FROM_ID_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_READ_FROM_ID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_MODEL");
			        COMP_MODEL_ID = new stringParameter("@COMP_MODEL_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string COMP_MODEL_ID)
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_READ_FROM_ID_def))
                    {
                        this.COMP_MODEL_ID.SetValue(COMP_MODEL_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_MODEL_ENTRY_INSERT_def MID_COMPUTATION_MODEL_ENTRY_INSERT = new MID_COMPUTATION_MODEL_ENTRY_INSERT_def();
			public class MID_COMPUTATION_MODEL_ENTRY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_ENTRY_INSERT.SQL"

			    private intParameter COMP_MODEL_RID;
			    private intParameter COMP_MODEL_SEQUENCE;
			    private intParameter COMP_TYPE;
			    private intParameter FV_RID;
			    private intParameter CHANGE_VARIABLE;
			    private intParameter PRODUCT_LEVEL;
			
			    public MID_COMPUTATION_MODEL_ENTRY_INSERT_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_ENTRY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("COMPUTATION_MODEL_ENTRY");
			        COMP_MODEL_RID = new intParameter("@COMP_MODEL_RID", base.inputParameterList);
			        COMP_MODEL_SEQUENCE = new intParameter("@COMP_MODEL_SEQUENCE", base.inputParameterList);
			        COMP_TYPE = new intParameter("@COMP_TYPE", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CHANGE_VARIABLE = new intParameter("@CHANGE_VARIABLE", base.inputParameterList);
			        PRODUCT_LEVEL = new intParameter("@PRODUCT_LEVEL", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? COMP_MODEL_RID,
			                      int? COMP_MODEL_SEQUENCE,
			                      int? COMP_TYPE,
			                      int? FV_RID,
			                      int? CHANGE_VARIABLE,
			                      int? PRODUCT_LEVEL
			                      )
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_ENTRY_INSERT_def))
                    {
                        this.COMP_MODEL_RID.SetValue(COMP_MODEL_RID);
                        this.COMP_MODEL_SEQUENCE.SetValue(COMP_MODEL_SEQUENCE);
                        this.COMP_TYPE.SetValue(COMP_TYPE);
                        this.FV_RID.SetValue(FV_RID);
                        this.CHANGE_VARIABLE.SetValue(CHANGE_VARIABLE);
                        this.PRODUCT_LEVEL.SetValue(PRODUCT_LEVEL);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static MID_COMPUTATION_MODEL_ENTRY_READ_def MID_COMPUTATION_MODEL_ENTRY_READ = new MID_COMPUTATION_MODEL_ENTRY_READ_def();
			public class MID_COMPUTATION_MODEL_ENTRY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_MODEL_ENTRY_READ.SQL"

			    private intParameter COMP_MODEL_RID;
			
			    public MID_COMPUTATION_MODEL_ENTRY_READ_def()
			    {
			        base.procedureName = "MID_COMPUTATION_MODEL_ENTRY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_MODEL_ENTRY");
			        COMP_MODEL_RID = new intParameter("@COMP_MODEL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? COMP_MODEL_RID)
			    {
                    lock (typeof(MID_COMPUTATION_MODEL_ENTRY_READ_def))
                    {
                        this.COMP_MODEL_RID.SetValue(COMP_MODEL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_COMPUTATION_GROUP_INSERT_def SP_MID_COMPUTATION_GROUP_INSERT = new SP_MID_COMPUTATION_GROUP_INSERT_def();
			public class SP_MID_COMPUTATION_GROUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_COMPUTATION_GROUP_INSERT.SQL"

			    private intParameter PROCESS;
			    private intParameter COMP_MODEL_RID;
			    private intParameter COMP_GROUP_RID; //Declare Output Parameter

                public SP_MID_COMPUTATION_GROUP_INSERT_def()
			    {
			        base.procedureName = "SP_MID_COMPUTATION_GROUP_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("COMPUTATION_GROUP");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        COMP_MODEL_RID = new intParameter("@COMP_MODEL_RID", base.inputParameterList);
			        COMP_GROUP_RID = new intParameter("@COMP_GROUP_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? PROCESS,
			                                  int? COMP_MODEL_RID
			                                  )
			    {
                    lock (typeof(SP_MID_COMPUTATION_GROUP_INSERT_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.COMP_MODEL_RID.SetValue(COMP_MODEL_RID);
                        this.COMP_GROUP_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_ITEM_DELETE_ALL_def MID_COMPUTATION_ITEM_DELETE_ALL = new MID_COMPUTATION_ITEM_DELETE_ALL_def();
			public class MID_COMPUTATION_ITEM_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_DELETE_ALL.SQL"

			
			    public MID_COMPUTATION_ITEM_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("COMPUTATION_ITEM");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED_def MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED = new MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED_def();
			public class MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED.SQL"

			
			    public MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("COMPUTATION_ITEM");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_GROUP_READ_ALL_def MID_COMPUTATION_GROUP_READ_ALL = new MID_COMPUTATION_GROUP_READ_ALL_def();
			public class MID_COMPUTATION_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_GROUP_READ_ALL.SQL"

			
			    public MID_COMPUTATION_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_COMPUTATION_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATION_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_GROUP_DELETE_def MID_COMPUTATION_GROUP_DELETE = new MID_COMPUTATION_GROUP_DELETE_def();
			public class MID_COMPUTATION_GROUP_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_GROUP_DELETE.SQL"

			    private intParameter CG_RID;
			
			    public MID_COMPUTATION_GROUP_DELETE_def()
			    {
			        base.procedureName = "MID_COMPUTATION_GROUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("COMPUTATION_GROUP");
			        CG_RID = new intParameter("@CG_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? CG_RID)
			    {
                    lock (typeof(MID_COMPUTATION_GROUP_DELETE_def))
                    {
                        this.CG_RID.SetValue(CG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_XML_COMPUTATION_ITEM_WRITE_def SP_MID_XML_COMPUTATION_ITEM_WRITE = new SP_MID_XML_COMPUTATION_ITEM_WRITE_def();
			public class SP_MID_XML_COMPUTATION_ITEM_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_COMPUTATION_ITEM_WRITE.SQL"

				private textParameter xmlDoc;
				
				public SP_MID_XML_COMPUTATION_ITEM_WRITE_def()
				{
				    base.procedureName = "SP_MID_XML_COMPUTATION_ITEM_WRITE";
				    base.procedureType = storedProcedureTypes.Insert;
				    base.tableNames.Add("<TABLENAME>");
				    xmlDoc = new textParameter("@xmlDoc", base.inputParameterList);
				}
				
				public int Insert (DatabaseAccess _dba, string xmlDoc)
				{
                    lock (typeof(SP_MID_XML_COMPUTATION_ITEM_WRITE_def))
                    {
                        this.xmlDoc.SetValue(xmlDoc);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
				}
			}

			public static MID_COMPUTATION_ITEM_READ_def MID_COMPUTATION_ITEM_READ = new MID_COMPUTATION_ITEM_READ_def();
			public class MID_COMPUTATION_ITEM_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_READ.SQL"

			    private intParameter PROCESS;
			
			    public MID_COMPUTATION_ITEM_READ_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATION_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? PROCESS)
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_READ_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_COMPUTATION_PROCESS_INSERT_def SP_MID_COMPUTATION_PROCESS_INSERT = new SP_MID_COMPUTATION_PROCESS_INSERT_def();
			public class SP_MID_COMPUTATION_PROCESS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_COMPUTATION_PROCESS_INSERT.SQL"

			    private intParameter PROCESS_RID;
			    private intParameter COMPUTATION_TYPE;
			    private intParameter STATUS_CODE;
			    private datetimeParameter START_TIME;
			    private intParameter COMPUTATION_RID; //Declare Output Parameter

                public SP_MID_COMPUTATION_PROCESS_INSERT_def()
			    {
                    base.procedureName = "SP_MID_COMPUTATION_PROCESS_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("COMPUTATION_PROCESS");
			        PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
			        COMPUTATION_TYPE = new intParameter("@COMPUTATION_TYPE", base.inputParameterList);
			        STATUS_CODE = new intParameter("@STATUS_CODE", base.inputParameterList);
			        START_TIME = new datetimeParameter("@START_TIME", base.inputParameterList);
			        COMPUTATION_RID = new intParameter("@COMPUTATION_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? PROCESS_RID,
			                                  int? COMPUTATION_TYPE,
			                                  int? STATUS_CODE,
			                                  DateTime? START_TIME
			                                  )
			    {
                    lock (typeof(SP_MID_COMPUTATION_PROCESS_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.COMPUTATION_TYPE.SetValue(COMPUTATION_TYPE);
                        this.STATUS_CODE.SetValue(STATUS_CODE);
                        this.START_TIME.SetValue(START_TIME);
                        this.COMPUTATION_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_PROCESS_UPDATE_def MID_COMPUTATION_PROCESS_UPDATE = new MID_COMPUTATION_PROCESS_UPDATE_def();
			public class MID_COMPUTATION_PROCESS_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_PROCESS_UPDATE.SQL"

			    private intParameter COMPUTATION_RID;
			    private intParameter STATUS_CODE;
			    private datetimeParameter STOP_TIME;
			
			    public MID_COMPUTATION_PROCESS_UPDATE_def()
			    {
			        base.procedureName = "MID_COMPUTATION_PROCESS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("COMPUTATION_PROCESS");
			        COMPUTATION_RID = new intParameter("@COMPUTATION_RID", base.inputParameterList);
			        STATUS_CODE = new intParameter("@STATUS_CODE", base.inputParameterList);
			        STOP_TIME = new datetimeParameter("@STOP_TIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? COMPUTATION_RID,
			                      int? STATUS_CODE,
			                      DateTime? STOP_TIME
			                      )
			    {
                    lock (typeof(MID_COMPUTATION_PROCESS_UPDATE_def))
                    {
                        this.COMPUTATION_RID.SetValue(COMPUTATION_RID);
                        this.STATUS_CODE.SetValue(STATUS_CODE);
                        this.STOP_TIME.SetValue(STOP_TIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_ITEM_READ_COUNT_def MID_COMPUTATION_ITEM_READ_COUNT = new MID_COMPUTATION_ITEM_READ_COUNT_def();
			public class MID_COMPUTATION_ITEM_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_READ_COUNT.SQL"

			    private intParameter CG_RID;
			
			    public MID_COMPUTATION_ITEM_READ_COUNT_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("COMPUTATION_ITEM");
			        CG_RID = new intParameter("@CG_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? CG_RID)
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_READ_COUNT_def))
                    {
                        this.CG_RID.SetValue(CG_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS_def MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS = new MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS_def();
			public class MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS.SQL"

			    private intParameter PROCESS;
			
			    public MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("COMPUTATION_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? PROCESS)
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED_def MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED = new MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED_def();
			public class MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED.SQL"

			    private intParameter PROCESS;
			    private intParameter CG_RID;
			    private intParameter ITEM_TYPE;
			    private intParameter HN_RID;
			    private intParameter FV_RID;
			    private intParameter FROM_FISCAL_YEAR_WEEK;
			    private intParameter TO_FISCAL_YEAR_WEEK;
			
			    public MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED_def()
			    {
			        base.procedureName = "MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("COMPUTATION_ITEM");
			        PROCESS = new intParameter("@PROCESS", base.inputParameterList);
			        CG_RID = new intParameter("@CG_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        FROM_FISCAL_YEAR_WEEK = new intParameter("@FROM_FISCAL_YEAR_WEEK", base.inputParameterList);
			        TO_FISCAL_YEAR_WEEK = new intParameter("@TO_FISCAL_YEAR_WEEK", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? PROCESS,
			                      int? CG_RID,
			                      int? ITEM_TYPE,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? FROM_FISCAL_YEAR_WEEK,
			                      int? TO_FISCAL_YEAR_WEEK
			                      )
			    {
                    lock (typeof(MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED_def))
                    {
                        this.PROCESS.SetValue(PROCESS);
                        this.CG_RID.SetValue(CG_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.FROM_FISCAL_YEAR_WEEK.SetValue(FROM_FISCAL_YEAR_WEEK);
                        this.TO_FISCAL_YEAR_WEEK.SetValue(TO_FISCAL_YEAR_WEEK);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE

        }
    }  
}
