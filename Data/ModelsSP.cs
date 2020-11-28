using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ModelsData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE.SQL"

			    private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? OLL_RID,
			                               int? HN_RID
			                               )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter VERSION_RID;
                private charParameter EXCLUDE_IND;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        VERSION_RID = new intParameter("@VERSION_RID", base.inputParameterList);
			        EXCLUDE_IND = new charParameter("@EXCLUDE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? VERSION_RID,
			                      char? EXCLUDE_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.VERSION_RID.SetValue(VERSION_RID);
                        this.EXCLUDE_IND.SetValue(EXCLUDE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT_def MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT = new MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter VERSION_RID;
                private charParameter EXCLUDE_IND;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        VERSION_RID = new intParameter("@VERSION_RID", base.inputParameterList);
			        EXCLUDE_IND = new charParameter("@EXCLUDE_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? VERSION_RID,
			                      char? EXCLUDE_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.VERSION_RID.SetValue(VERSION_RID);
                        this.EXCLUDE_IND.SetValue(EXCLUDE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL = new MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL = new MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL.SQL"

                private intParameter OLL_RID;
                private intParameter NEW_OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        NEW_OLL_RID = new intParameter("@NEW_OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? NEW_OLL_RID
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.NEW_OLL_RID.SetValue(NEW_OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE.SQL"

                private stringParameter NAME;
                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_HN_RID;
                private intParameter HIGH_LEVEL_SEQ;
                private intParameter HIGH_LEVEL_OFFEST;
                private intParameter HIGH_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFEST;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter USER_RID;
                private charParameter ACTIVE_ONLY_IND;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
			        HIGH_LEVEL_SEQ = new intParameter("@HIGH_LEVEL_SEQ", base.inputParameterList);
			        HIGH_LEVEL_OFFEST = new intParameter("@HIGH_LEVEL_OFFEST", base.inputParameterList);
			        HIGH_LEVEL_TYPE = new intParameter("@HIGH_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFEST = new intParameter("@LOW_LEVEL_OFFEST", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        ACTIVE_ONLY_IND = new charParameter("@ACTIVE_ONLY_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      string NAME,
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? HIGH_LEVEL_HN_RID,
			                      int? HIGH_LEVEL_SEQ,
			                      int? HIGH_LEVEL_OFFEST,
			                      int? HIGH_LEVEL_TYPE,
			                      int? LOW_LEVEL_SEQ,
			                      int? LOW_LEVEL_OFFEST,
			                      int? LOW_LEVEL_TYPE,
			                      int? USER_RID,
			                      char? ACTIVE_ONLY_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE_def))
                    {
                        this.NAME.SetValue(NAME);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.HIGH_LEVEL_SEQ.SetValue(HIGH_LEVEL_SEQ);
                        this.HIGH_LEVEL_OFFEST.SetValue(HIGH_LEVEL_OFFEST);
                        this.HIGH_LEVEL_TYPE.SetValue(HIGH_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFEST.SetValue(LOW_LEVEL_OFFEST);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        this.ACTIVE_ONLY_IND.SetValue(ACTIVE_ONLY_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT.SQL"

                private stringParameter NAME;
                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_HN_RID;
                private intParameter HIGH_LEVEL_SEQ;
                private intParameter HIGH_LEVEL_OFFEST;
                private intParameter HIGH_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFEST;
                private intParameter LOW_LEVEL_TYPE;
                private intParameter USER_RID;
                private charParameter ACTIVE_ONLY_IND;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
			        HIGH_LEVEL_SEQ = new intParameter("@HIGH_LEVEL_SEQ", base.inputParameterList);
			        HIGH_LEVEL_OFFEST = new intParameter("@HIGH_LEVEL_OFFEST", base.inputParameterList);
			        HIGH_LEVEL_TYPE = new intParameter("@HIGH_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFEST = new intParameter("@LOW_LEVEL_OFFEST", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        ACTIVE_ONLY_IND = new charParameter("@ACTIVE_ONLY_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      string NAME,
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? HIGH_LEVEL_HN_RID,
			                      int? HIGH_LEVEL_SEQ,
			                      int? HIGH_LEVEL_OFFEST,
			                      int? HIGH_LEVEL_TYPE,
			                      int? LOW_LEVEL_SEQ,
			                      int? LOW_LEVEL_OFFEST,
			                      int? LOW_LEVEL_TYPE,
			                      int? USER_RID,
			                      char? ACTIVE_ONLY_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_def))
                    {
                        this.NAME.SetValue(NAME);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.HIGH_LEVEL_SEQ.SetValue(HIGH_LEVEL_SEQ);
                        this.HIGH_LEVEL_OFFEST.SetValue(HIGH_LEVEL_OFFEST);
                        this.HIGH_LEVEL_TYPE.SetValue(HIGH_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFEST.SetValue(LOW_LEVEL_OFFEST);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        this.ACTIVE_ONLY_IND.SetValue(ACTIVE_ONLY_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ.SQL"

                private stringParameter NAME;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string NAME)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_def))
                    {
                        this.NAME.SetValue(NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL.SQL"

                private intParameter OLL_RID;
                private intParameter customOLL_rid;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
                                      int? customOLL_rid
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE.SQL"

                private intParameter OLL_RID;
                private intParameter customOLL_rid;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
                                      int? customOLL_rid
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL.SQL"

                private intParameter OLL_RID;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE_def MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE = new MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE.SQL"

                private intParameter OLL_RID;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? ITEM_TYPE
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL_def MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL = new MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL_WORK");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_def MID_OVERRIDE_LL_MODEL_DETAIL_DELETE = new MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_DELETE.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID_def MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID = new MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID_def();
			public class MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID.SQL"

                private intParameter OLL_RID;
			
			    public MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID_def()
			    {
			        base.procedureName = "MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("METHOD_USER_PLAN");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE_def MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE = new MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT_def MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT = new MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? OLL_RID,
			                               int? HN_RID
			                               )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE_def MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE = new MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter VERSION_RID;
                private charParameter EXCLUDE_IND;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        VERSION_RID = new intParameter("@VERSION_RID", base.inputParameterList);
			        EXCLUDE_IND = new charParameter("@EXCLUDE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? VERSION_RID,
			                      char? EXCLUDE_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.VERSION_RID.SetValue(VERSION_RID);
                        this.EXCLUDE_IND.SetValue(EXCLUDE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_def MID_OVERRIDE_LL_MODEL_DETAIL_INSERT = new MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_INSERT.SQL"

                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter VERSION_RID;
                private charParameter EXCLUDE_IND;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        VERSION_RID = new intParameter("@VERSION_RID", base.inputParameterList);
			        EXCLUDE_IND = new charParameter("@EXCLUDE_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? VERSION_RID,
			                      char? EXCLUDE_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.VERSION_RID.SetValue(VERSION_RID);
                        this.EXCLUDE_IND.SetValue(EXCLUDE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL_def MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL = new MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK_def MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK = new MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK_def();
			public class MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK.SQL"

                private intParameter OLL_RID;
			
			    public MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK_def()
			    {
			        base.procedureName = "MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_PLAN");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_DELETE_def MID_OVERRIDE_LL_MODEL_HEADER_DELETE = new MID_OVERRIDE_LL_MODEL_HEADER_DELETE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_DELETE.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_DELETE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_DELETE_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_UPDATE_def MID_OVERRIDE_LL_MODEL_HEADER_UPDATE = new MID_OVERRIDE_LL_MODEL_HEADER_UPDATE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_UPDATE.SQL"

                private stringParameter NAME;
                private intParameter OLL_RID;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_HN_RID;
                private intParameter HIGH_LEVEL_SEQ;
                private intParameter HIGH_LEVEL_OFFSET;
                private intParameter HIGH_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter LOW_LEVEL_TYPE;
                private charParameter ACTIVE_ONLY_IND;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_UPDATE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
			        HIGH_LEVEL_SEQ = new intParameter("@HIGH_LEVEL_SEQ", base.inputParameterList);
			        HIGH_LEVEL_OFFSET = new intParameter("@HIGH_LEVEL_OFFSET", base.inputParameterList);
			        HIGH_LEVEL_TYPE = new intParameter("@HIGH_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        ACTIVE_ONLY_IND = new charParameter("@ACTIVE_ONLY_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      string NAME,
			                      int? OLL_RID,
			                      int? HN_RID,
			                      int? HIGH_LEVEL_HN_RID,
			                      int? HIGH_LEVEL_SEQ,
			                      int? HIGH_LEVEL_OFFSET,
			                      int? HIGH_LEVEL_TYPE,
			                      int? LOW_LEVEL_SEQ,
			                      int? LOW_LEVEL_OFFSET,
			                      int? LOW_LEVEL_TYPE,
			                      char? ACTIVE_ONLY_IND
			                      )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_UPDATE_def))
                    {
                        this.NAME.SetValue(NAME);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.HIGH_LEVEL_SEQ.SetValue(HIGH_LEVEL_SEQ);
                        this.HIGH_LEVEL_OFFSET.SetValue(HIGH_LEVEL_OFFSET);
                        this.HIGH_LEVEL_TYPE.SetValue(HIGH_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.ACTIVE_ONLY_IND.SetValue(ACTIVE_ONLY_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_OVERRIDE_LL_MODEL_INSERT_def SP_MID_OVERRIDE_LL_MODEL_INSERT = new SP_MID_OVERRIDE_LL_MODEL_INSERT_def();
            public class SP_MID_OVERRIDE_LL_MODEL_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_OVERRIDE_LL_MODEL_INSERT.SQL"

                private stringParameter NAME;
                private intParameter HN_RID;
                private intParameter HIGH_LEVEL_HN_RID;
                private intParameter HIGH_LEVEL_SEQ;
                private intParameter HIGH_LEVEL_OFFSET;
                private intParameter HIGH_LEVEL_TYPE;
                private intParameter LOW_LEVEL_SEQ;
                private intParameter LOW_LEVEL_OFFSET;
                private intParameter LOW_LEVEL_TYPE;
                private charParameter ACTIVE_ONLY_IND;
                private intParameter OLL_RID; //Declare Output Parameter
			
			    public SP_MID_OVERRIDE_LL_MODEL_INSERT_def()
			    {
                    base.procedureName = "SP_MID_OVERRIDE_LL_MODEL_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
			        HIGH_LEVEL_SEQ = new intParameter("@HIGH_LEVEL_SEQ", base.inputParameterList);
			        HIGH_LEVEL_OFFSET = new intParameter("@HIGH_LEVEL_OFFSET", base.inputParameterList);
			        HIGH_LEVEL_TYPE = new intParameter("@HIGH_LEVEL_TYPE", base.inputParameterList);
			        LOW_LEVEL_SEQ = new intParameter("@LOW_LEVEL_SEQ", base.inputParameterList);
			        LOW_LEVEL_OFFSET = new intParameter("@LOW_LEVEL_OFFSET", base.inputParameterList);
			        LOW_LEVEL_TYPE = new intParameter("@LOW_LEVEL_TYPE", base.inputParameterList);
			        ACTIVE_ONLY_IND = new charParameter("@ACTIVE_ONLY_IND", base.inputParameterList);
			        OLL_RID = new intParameter("@OLL_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string NAME,
			                                  int? HN_RID,
			                                  int? HIGH_LEVEL_HN_RID,
			                                  int? HIGH_LEVEL_SEQ,
			                                  int? HIGH_LEVEL_OFFSET,
			                                  int? HIGH_LEVEL_TYPE,
			                                  int? LOW_LEVEL_SEQ,
			                                  int? LOW_LEVEL_OFFSET,
			                                  int? LOW_LEVEL_TYPE,
			                                  char? ACTIVE_ONLY_IND
			                                  )
			    {
                    lock (typeof(SP_MID_OVERRIDE_LL_MODEL_INSERT_def))
                    {
                        this.NAME.SetValue(NAME);
                        this.HN_RID.SetValue(HN_RID);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.HIGH_LEVEL_SEQ.SetValue(HIGH_LEVEL_SEQ);
                        this.HIGH_LEVEL_OFFSET.SetValue(HIGH_LEVEL_OFFSET);
                        this.HIGH_LEVEL_TYPE.SetValue(HIGH_LEVEL_TYPE);
                        this.LOW_LEVEL_SEQ.SetValue(LOW_LEVEL_SEQ);
                        this.LOW_LEVEL_OFFSET.SetValue(LOW_LEVEL_OFFSET);
                        this.LOW_LEVEL_TYPE.SetValue(LOW_LEVEL_TYPE);
                        this.ACTIVE_ONLY_IND.SetValue(ACTIVE_ONLY_IND);
                        this.OLL_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME_def MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME = new MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME_def();
			public class MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME.SQL"

                private stringParameter NAME;
			
			    public MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string NAME)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME_def))
                    {
                        this.NAME.SetValue(NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME_def MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME = new MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME.SQL"

                private intParameter OLL_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? OLL_RID)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW.SQL"

                private intParameter OLL_RID;
                private intParameter USER_RID;
                private intParameter customOLL_rid;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    customOLL_rid = new intParameter("@customOLL_rid", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? USER_RID,
                                      int? customOLL_rid,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.customOLL_rid.SetValue(customOLL_rid);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL.SQL"

                private intParameter OLL_RID;
                private intParameter ITEM_TYPE;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? OLL_RID,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_def))
                    {
                        this.OLL_RID.SetValue(OLL_RID);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT_def MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT = new MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT.SQL"

                private stringParameter NAME;
                private intParameter USER_RID;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               string NAME,
			                               int? USER_RID
			                               )
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT_def))
                    {
                        this.NAME.SetValue(NAME);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_VARIABLE_DELETE_def MID_FORECAST_MODEL_VARIABLE_DELETE = new MID_FORECAST_MODEL_VARIABLE_DELETE_def();
			public class MID_FORECAST_MODEL_VARIABLE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_VARIABLE_DELETE.SQL"

                private intParameter FORECAST_MOD_RID;
			
			    public MID_FORECAST_MODEL_VARIABLE_DELETE_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_VARIABLE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FORECAST_MODEL_VARIABLE");
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FORECAST_MOD_RID)
			    {
                    lock (typeof(MID_FORECAST_MODEL_VARIABLE_DELETE_def))
                    {
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_VARIABLE_INSERT_def MID_FORECAST_MODEL_VARIABLE_INSERT = new MID_FORECAST_MODEL_VARIABLE_INSERT_def();
			public class MID_FORECAST_MODEL_VARIABLE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_VARIABLE_INSERT.SQL"

                private intParameter FORECAST_MOD_RID;
                private intParameter VARIABLE_SEQUENCE;
                private intParameter VARIABLE_NUMBER;
                private intParameter FORECAST_FORMULA;
                private intParameter ASSOC_VARIABLE;
                private charParameter GRADE_WOS_IDX;
                private charParameter STOCK_MODIFIER;
                private charParameter FWOS_OVERRIDE;
                private charParameter STOCK_MIN;
                private charParameter STOCK_MAX;
                private charParameter MIN_PLUS_SALES;
                private charParameter SALES_MODIFIER;
                private charParameter USE_PLAN;
                private charParameter ALLOW_CHAIN_NEGATIVES;
			
			    public MID_FORECAST_MODEL_VARIABLE_INSERT_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_VARIABLE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("FORECAST_MODEL_VARIABLE");
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			        VARIABLE_SEQUENCE = new intParameter("@VARIABLE_SEQUENCE", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        FORECAST_FORMULA = new intParameter("@FORECAST_FORMULA", base.inputParameterList);
			        ASSOC_VARIABLE = new intParameter("@ASSOC_VARIABLE", base.inputParameterList);
			        GRADE_WOS_IDX = new charParameter("@GRADE_WOS_IDX", base.inputParameterList);
			        STOCK_MODIFIER = new charParameter("@STOCK_MODIFIER", base.inputParameterList);
			        FWOS_OVERRIDE = new charParameter("@FWOS_OVERRIDE", base.inputParameterList);
			        STOCK_MIN = new charParameter("@STOCK_MIN", base.inputParameterList);
			        STOCK_MAX = new charParameter("@STOCK_MAX", base.inputParameterList);
			        MIN_PLUS_SALES = new charParameter("@MIN_PLUS_SALES", base.inputParameterList);
			        SALES_MODIFIER = new charParameter("@SALES_MODIFIER", base.inputParameterList);
			        USE_PLAN = new charParameter("@USE_PLAN", base.inputParameterList);
			        ALLOW_CHAIN_NEGATIVES = new charParameter("@ALLOW_CHAIN_NEGATIVES", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? FORECAST_MOD_RID,
			                      int? VARIABLE_SEQUENCE,
			                      int? VARIABLE_NUMBER,
			                      int? FORECAST_FORMULA,
			                      int? ASSOC_VARIABLE,
			                      char? GRADE_WOS_IDX,
			                      char? STOCK_MODIFIER,
			                      char? FWOS_OVERRIDE,
			                      char? STOCK_MIN,
			                      char? STOCK_MAX,
			                      char? MIN_PLUS_SALES,
			                      char? SALES_MODIFIER,
			                      char? USE_PLAN,
			                      char? ALLOW_CHAIN_NEGATIVES
			                      )
			    {
                    lock (typeof(MID_FORECAST_MODEL_VARIABLE_INSERT_def))
                    {
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        this.VARIABLE_SEQUENCE.SetValue(VARIABLE_SEQUENCE);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.FORECAST_FORMULA.SetValue(FORECAST_FORMULA);
                        this.ASSOC_VARIABLE.SetValue(ASSOC_VARIABLE);
                        this.GRADE_WOS_IDX.SetValue(GRADE_WOS_IDX);
                        this.STOCK_MODIFIER.SetValue(STOCK_MODIFIER);
                        this.FWOS_OVERRIDE.SetValue(FWOS_OVERRIDE);
                        this.STOCK_MIN.SetValue(STOCK_MIN);
                        this.STOCK_MAX.SetValue(STOCK_MAX);
                        this.MIN_PLUS_SALES.SetValue(MIN_PLUS_SALES);
                        this.SALES_MODIFIER.SetValue(SALES_MODIFIER);
                        this.USE_PLAN.SetValue(USE_PLAN);
                        this.ALLOW_CHAIN_NEGATIVES.SetValue(ALLOW_CHAIN_NEGATIVES);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_VARIABLE_READ_ALL_def MID_FORECAST_MODEL_VARIABLE_READ_ALL = new MID_FORECAST_MODEL_VARIABLE_READ_ALL_def();
			public class MID_FORECAST_MODEL_VARIABLE_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_VARIABLE_READ_ALL.SQL"

                private intParameter FORECAST_MOD_RID;
			
			    public MID_FORECAST_MODEL_VARIABLE_READ_ALL_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_VARIABLE_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_MODEL_VARIABLE");
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FORECAST_MOD_RID)
			    {
                    lock (typeof(MID_FORECAST_MODEL_VARIABLE_READ_ALL_def))
                    {
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_DELETE_def MID_FORECAST_MODEL_DELETE = new MID_FORECAST_MODEL_DELETE_def();
			public class MID_FORECAST_MODEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_DELETE.SQL"

                private intParameter FORECAST_MOD_RID;
			
			    public MID_FORECAST_MODEL_DELETE_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FORECAST_MODEL");
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FORECAST_MOD_RID)
			    {
                    lock (typeof(MID_FORECAST_MODEL_DELETE_def))
                    {
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_FORECAST_MODEL_INSERT_def SP_MID_FORECAST_MODEL_INSERT = new SP_MID_FORECAST_MODEL_INSERT_def();
			public class SP_MID_FORECAST_MODEL_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FORECAST_MODEL_INSERT.SQL"

                private stringParameter FORECAST_MOD_ID;
                private charParameter DEFAULT_IND;
                private stringParameter CALC_MODE;
                private intParameter FORECAST_MOD_RID; //Declare Output Parameter

                public SP_MID_FORECAST_MODEL_INSERT_def()
			    {
                    base.procedureName = "SP_MID_FORECAST_MODEL_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("FORECAST_MODEL");
			        FORECAST_MOD_ID = new stringParameter("@FORECAST_MOD_ID", base.inputParameterList);
			        DEFAULT_IND = new charParameter("@DEFAULT_IND", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string FORECAST_MOD_ID,
			                                  char? DEFAULT_IND,
			                                  string CALC_MODE
			                                  )
			    {
                    lock (typeof(SP_MID_FORECAST_MODEL_INSERT_def))
                    {
                        this.FORECAST_MOD_ID.SetValue(FORECAST_MOD_ID);
                        this.DEFAULT_IND.SetValue(DEFAULT_IND);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.FORECAST_MOD_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_READ_ALL_WITH_RID_def MID_FORECAST_MODEL_READ_ALL_WITH_RID = new MID_FORECAST_MODEL_READ_ALL_WITH_RID_def();
			public class MID_FORECAST_MODEL_READ_ALL_WITH_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_READ_ALL_WITH_RID.SQL"

                private intParameter MODEL_RID;
			
			    public MID_FORECAST_MODEL_READ_ALL_WITH_RID_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_READ_ALL_WITH_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_MODEL");
			        MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? MODEL_RID)
			    {
                    lock (typeof(MID_FORECAST_MODEL_READ_ALL_WITH_RID_def))
                    {
                        this.MODEL_RID.SetValue(MODEL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_READ_ALL_WITH_ID_def MID_FORECAST_MODEL_READ_ALL_WITH_ID = new MID_FORECAST_MODEL_READ_ALL_WITH_ID_def();
			public class MID_FORECAST_MODEL_READ_ALL_WITH_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_READ_ALL_WITH_ID.SQL"

                private stringParameter MODEL_ID;
			
			    public MID_FORECAST_MODEL_READ_ALL_WITH_ID_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_READ_ALL_WITH_ID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_MODEL");
			        MODEL_ID = new stringParameter("@MODEL_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string MODEL_ID)
			    {
                    lock (typeof(MID_FORECAST_MODEL_READ_ALL_WITH_ID_def))
                    {
                        this.MODEL_ID.SetValue(MODEL_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_READ_ALL_def MID_FORECAST_MODEL_READ_ALL = new MID_FORECAST_MODEL_READ_ALL_def();
			public class MID_FORECAST_MODEL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_READ_ALL.SQL"

			
			    public MID_FORECAST_MODEL_READ_ALL_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_MODEL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FORECAST_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_VARIABLE_DELETE_def MID_FORECAST_BAL_MODEL_VARIABLE_DELETE = new MID_FORECAST_BAL_MODEL_VARIABLE_DELETE_def();
			public class MID_FORECAST_BAL_MODEL_VARIABLE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_VARIABLE_DELETE.SQL"

                private intParameter FBMOD_RID;
			
			    public MID_FORECAST_BAL_MODEL_VARIABLE_DELETE_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_VARIABLE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FORECAST_BAL_MODEL_VARIABLE");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FBMOD_RID)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_VARIABLE_DELETE_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_VARIABLE_INSERT_def MID_FORECAST_BAL_MODEL_VARIABLE_INSERT = new MID_FORECAST_BAL_MODEL_VARIABLE_INSERT_def();
			public class MID_FORECAST_BAL_MODEL_VARIABLE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_VARIABLE_INSERT.SQL"

                private intParameter FBMOD_RID;
                private intParameter VARIABLE_SEQUENCE;
                private intParameter VARIABLE_NUMBER;
			
			    public MID_FORECAST_BAL_MODEL_VARIABLE_INSERT_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_VARIABLE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("FORECAST_BAL_MODEL_VARIABLE");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			        VARIABLE_SEQUENCE = new intParameter("@VARIABLE_SEQUENCE", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? FBMOD_RID,
			                      int? VARIABLE_SEQUENCE,
			                      int? VARIABLE_NUMBER
			                      )
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_VARIABLE_INSERT_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        this.VARIABLE_SEQUENCE.SetValue(VARIABLE_SEQUENCE);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID_def MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID = new MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID_def();
			public class MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID.SQL"

                private intParameter FBMOD_RID;
			
			    public MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL_VARIABLE");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FBMOD_RID)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_DELETE_def MID_FORECAST_BAL_MODEL_DELETE = new MID_FORECAST_BAL_MODEL_DELETE_def();
			public class MID_FORECAST_BAL_MODEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_DELETE.SQL"

                private intParameter FBMOD_RID;
			
			    public MID_FORECAST_BAL_MODEL_DELETE_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FBMOD_RID)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_DELETE_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_UPDATE_def MID_FORECAST_BAL_MODEL_UPDATE = new MID_FORECAST_BAL_MODEL_UPDATE_def();
			public class MID_FORECAST_BAL_MODEL_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_UPDATE.SQL"

                private intParameter FBMOD_RID;
                private stringParameter FBMOD_ID;
                private intParameter MATRIX_TYPE;
                private intParameter ITERATIONS_TYPE;
                private intParameter ITERATIONS_COUNT;
                private intParameter BALANCE_MODE;
                private stringParameter CALC_MODE;
			
			    public MID_FORECAST_BAL_MODEL_UPDATE_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			        FBMOD_ID = new stringParameter("@FBMOD_ID", base.inputParameterList);
			        MATRIX_TYPE = new intParameter("@MATRIX_TYPE", base.inputParameterList);
			        ITERATIONS_TYPE = new intParameter("@ITERATIONS_TYPE", base.inputParameterList);
			        ITERATIONS_COUNT = new intParameter("@ITERATIONS_COUNT", base.inputParameterList);
			        BALANCE_MODE = new intParameter("@BALANCE_MODE", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FBMOD_RID,
			                      string FBMOD_ID,
			                      int? MATRIX_TYPE,
			                      int? ITERATIONS_TYPE,
			                      int? ITERATIONS_COUNT,
			                      int? BALANCE_MODE,
			                      string CALC_MODE
			                      )
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_UPDATE_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        this.FBMOD_ID.SetValue(FBMOD_ID);
                        this.MATRIX_TYPE.SetValue(MATRIX_TYPE);
                        this.ITERATIONS_TYPE.SetValue(ITERATIONS_TYPE);
                        this.ITERATIONS_COUNT.SetValue(ITERATIONS_COUNT);
                        this.BALANCE_MODE.SetValue(BALANCE_MODE);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_FORECAST_MODEL_UPDATE_def MID_FORECAST_MODEL_UPDATE = new MID_FORECAST_MODEL_UPDATE_def();
			public class MID_FORECAST_MODEL_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_MODEL_UPDATE.SQL"

                private intParameter FORECAST_MOD_RID;
                private stringParameter FORECAST_MOD_ID;
                private charParameter DEFAULT_IND;
                private stringParameter CALC_MODE;
			
			    public MID_FORECAST_MODEL_UPDATE_def()
			    {
			        base.procedureName = "MID_FORECAST_MODEL_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FORECAST_MODEL");
			        FORECAST_MOD_RID = new intParameter("@FORECAST_MOD_RID", base.inputParameterList);
			        FORECAST_MOD_ID = new stringParameter("@FORECAST_MOD_ID", base.inputParameterList);
			        DEFAULT_IND = new charParameter("@DEFAULT_IND", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FORECAST_MOD_RID,
			                      string FORECAST_MOD_ID,
			                      char? DEFAULT_IND,
			                      string CALC_MODE
			                      )
			    {
                    lock (typeof(MID_FORECAST_MODEL_UPDATE_def))
                    {
                        this.FORECAST_MOD_RID.SetValue(FORECAST_MOD_RID);
                        this.FORECAST_MOD_ID.SetValue(FORECAST_MOD_ID);
                        this.DEFAULT_IND.SetValue(DEFAULT_IND);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_FOR_BAL_MODEL_INSERT_def SP_MID_FOR_BAL_MODEL_INSERT = new SP_MID_FOR_BAL_MODEL_INSERT_def();
            public class SP_MID_FOR_BAL_MODEL_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FOR_BAL_MODEL_INSERT.SQL"

                private stringParameter FBMOD_ID;
                private intParameter MATRIX_TYPE;
                private intParameter ITERATIONS_TYPE;
                private intParameter ITERATIONS_COUNT;
                private intParameter BALANCE_MODE;
                private stringParameter CALC_MODE;
                private intParameter FBMOD_RID; //Declare Output Parameter
			
			    public SP_MID_FOR_BAL_MODEL_INSERT_def()
			    {
                    base.procedureName = "SP_MID_FOR_BAL_MODEL_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_ID = new stringParameter("@FBMOD_ID", base.inputParameterList);
			        MATRIX_TYPE = new intParameter("@MATRIX_TYPE", base.inputParameterList);
			        ITERATIONS_TYPE = new intParameter("@ITERATIONS_TYPE", base.inputParameterList);
			        ITERATIONS_COUNT = new intParameter("@ITERATIONS_COUNT", base.inputParameterList);
			        BALANCE_MODE = new intParameter("@BALANCE_MODE", base.inputParameterList);
			        CALC_MODE = new stringParameter("@CALC_MODE", base.inputParameterList);
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string FBMOD_ID,
			                                  int? MATRIX_TYPE,
			                                  int? ITERATIONS_TYPE,
			                                  int? ITERATIONS_COUNT,
			                                  int? BALANCE_MODE,
			                                  string CALC_MODE
			                                  )
			    {
                    lock (typeof(SP_MID_FOR_BAL_MODEL_INSERT_def))
                    {
                        this.FBMOD_ID.SetValue(FBMOD_ID);
                        this.MATRIX_TYPE.SetValue(MATRIX_TYPE);
                        this.ITERATIONS_TYPE.SetValue(ITERATIONS_TYPE);
                        this.ITERATIONS_COUNT.SetValue(ITERATIONS_COUNT);
                        this.BALANCE_MODE.SetValue(BALANCE_MODE);
                        this.CALC_MODE.SetValue(CALC_MODE);
                        this.FBMOD_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_READ_WITH_RID_def MID_FORECAST_BAL_MODEL_READ_WITH_RID = new MID_FORECAST_BAL_MODEL_READ_WITH_RID_def();
			public class MID_FORECAST_BAL_MODEL_READ_WITH_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_READ_WITH_RID.SQL"

                private intParameter FBMOD_RID;
			
			    public MID_FORECAST_BAL_MODEL_READ_WITH_RID_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_READ_WITH_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_RID = new intParameter("@FBMOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FBMOD_RID)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_READ_WITH_RID_def))
                    {
                        this.FBMOD_RID.SetValue(FBMOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_READ_WITH_ID_def MID_FORECAST_BAL_MODEL_READ_WITH_ID = new MID_FORECAST_BAL_MODEL_READ_WITH_ID_def();
			public class MID_FORECAST_BAL_MODEL_READ_WITH_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_READ_WITH_ID.SQL"

                private stringParameter FBMOD_ID;
			
			    public MID_FORECAST_BAL_MODEL_READ_WITH_ID_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_READ_WITH_ID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_ID = new stringParameter("@FBMOD_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FBMOD_ID)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_READ_WITH_ID_def))
                    {
                        this.FBMOD_ID.SetValue(FBMOD_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_READ_ALL_def MID_FORECAST_BAL_MODEL_READ_ALL = new MID_FORECAST_BAL_MODEL_READ_ALL_def();
			public class MID_FORECAST_BAL_MODEL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_READ_ALL.SQL"

			
			    public MID_FORECAST_BAL_MODEL_READ_ALL_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static SP_MID_GET_OVERRIDES_BY_OFFSET_def SP_MID_GET_OVERRIDES_BY_OFFSET = new SP_MID_GET_OVERRIDES_BY_OFFSET_def();
			public class SP_MID_GET_OVERRIDES_BY_OFFSET_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_OVERRIDES_BY_OFFSET.SQL"

                private intParameter MODEL_RID;
                private intParameter HN_RID;
                private intParameter LEVEL_OFFSET;
                private intParameter HIGH_LEVEL_HN_RID;
                private charParameter MAINTENANCE;
                private intParameter debug;
			
			    public SP_MID_GET_OVERRIDES_BY_OFFSET_def()
			    {
			        base.procedureName = "SP_MID_GET_OVERRIDES_BY_OFFSET";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE ");
                    MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_OFFSET = new intParameter("@LEVEL_OFFSET", base.inputParameterList);
                    HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
                    MAINTENANCE = new charParameter("@MAINTENANCE", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? MODEL_RID,
			                          int? HN_RID,
			                          int? LEVEL_OFFSET,
			                          int? HIGH_LEVEL_HN_RID,
			                          char MAINTENANCE
			                          )
			    {
                    lock (typeof(SP_MID_GET_OVERRIDES_BY_OFFSET_def))
                    {
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_OFFSET.SetValue(LEVEL_OFFSET);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.MAINTENANCE.SetValue(MAINTENANCE);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static SP_MID_GET_OVERRIDES_BY_LEVEL_def SP_MID_GET_OVERRIDES_BY_LEVEL = new SP_MID_GET_OVERRIDES_BY_LEVEL_def();
			public class SP_MID_GET_OVERRIDES_BY_LEVEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_OVERRIDES_BY_LEVEL.SQL"

                private intParameter MODEL_RID;
                private intParameter HN_RID;
                private intParameter LEVEL_SEQ;
                private intParameter HIGH_LEVEL_HN_RID;
                private charParameter MAINTENANCE;
                private intParameter debug;
			
			    public SP_MID_GET_OVERRIDES_BY_LEVEL_def()
			    {
			        base.procedureName = "SP_MID_GET_OVERRIDES_BY_LEVEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
                    MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_SEQ = new intParameter("@LEVEL_SEQ", base.inputParameterList);
                    HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
			        MAINTENANCE = new charParameter("@MAINTENANCE", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? MODEL_RID,
			                          int? HN_RID,
			                          int? LEVEL_SEQ,
			                          int? HIGH_LEVEL_HN_RID,
			                          char MAINTENANCE
			                          )
			    {
                    lock (typeof(SP_MID_GET_OVERRIDES_BY_LEVEL_def))
                    {
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_SEQ.SetValue(LEVEL_SEQ);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.MAINTENANCE.SetValue(MAINTENANCE);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static SP_MID_DELETE_OVERRIDES_def SP_MID_DELETE_OVERRIDES = new SP_MID_DELETE_OVERRIDES_def();
			public class SP_MID_DELETE_OVERRIDES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DELETE_OVERRIDES.SQL"

                private intParameter MODEL_RID;
                private intParameter HN_RID;
                private intParameter debug;
			
			    public SP_MID_DELETE_OVERRIDES_def()
			    {
			        base.procedureName = "SP_MID_DELETE_OVERRIDES";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("OVERRIDE_LL_MODEL_DETAIL");
			        MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? MODEL_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(SP_MID_DELETE_OVERRIDES_def))
                    {
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static SP_MID_GET_OVERRIDES_BY_TYPE_def SP_MID_GET_OVERRIDES_BY_TYPE = new SP_MID_GET_OVERRIDES_BY_TYPE_def();
			public class SP_MID_GET_OVERRIDES_BY_TYPE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_OVERRIDES_BY_TYPE.SQL"

                private intParameter MODEL_RID;
                private intParameter HN_RID;
                private intParameter LEVEL_TYPE;
                private intParameter HIGH_LEVEL_HN_RID;
                private charParameter MAINTENANCE;
                private intParameter debug;

                public SP_MID_GET_OVERRIDES_BY_TYPE_def()
			    {
                    base.procedureName = "SP_MID_GET_OVERRIDES_BY_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GET_OVERRIDES_BY_TYPE");
                    MODEL_RID = new intParameter("@MODEL_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_TYPE = new intParameter("@LEVEL_TYPE", base.inputParameterList);
                    HIGH_LEVEL_HN_RID = new intParameter("@HIGH_LEVEL_HN_RID", base.inputParameterList);
                    MAINTENANCE = new charParameter("@MAINTENANCE", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? MODEL_RID,
			                          int? HN_RID,
			                          int? LEVEL_TYPE,
			                          int? HIGH_LEVEL_HN_RID,
			                          char? MAINTENANCE
			                          )
			    {
                    lock (typeof(SP_MID_GET_OVERRIDES_BY_TYPE_def))
                    {
                        this.MODEL_RID.SetValue(MODEL_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_TYPE.SetValue(LEVEL_TYPE);
                        this.HIGH_LEVEL_HN_RID.SetValue(HIGH_LEVEL_HN_RID);
                        this.MAINTENANCE.SetValue(MAINTENANCE);
                        this.debug.SetValue(0);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
