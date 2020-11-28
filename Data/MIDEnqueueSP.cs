using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class MIDEnqueue : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_ENQUEUE_READ_FOR_HEADER_def MID_ENQUEUE_READ_FOR_HEADER = new MID_ENQUEUE_READ_FOR_HEADER_def();
			public class MID_ENQUEUE_READ_FOR_HEADER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FOR_HEADER.SQL"

			    private intParameter RID;
			
			    public MID_ENQUEUE_READ_FOR_HEADER_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FOR_HEADER";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        RID = new intParameter("@RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? RID)
			    {
                    lock (typeof(MID_ENQUEUE_READ_FOR_HEADER_def))
                    {
                        this.RID.SetValue(RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_READ_FROM_TYPE_def MID_ENQUEUE_READ_FROM_TYPE = new MID_ENQUEUE_READ_FROM_TYPE_def();
			public class MID_ENQUEUE_READ_FROM_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FROM_TYPE.SQL"

                private intParameter RID;
                private intParameter ENQUEUE_TYPE;
			
			    public MID_ENQUEUE_READ_FROM_TYPE_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FROM_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        RID = new intParameter("@RID", base.inputParameterList);
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? RID,
			                          int? ENQUEUE_TYPE
			                          )
			    {
                    lock (typeof(MID_ENQUEUE_READ_FROM_TYPE_def))
                    {
                        this.RID.SetValue(RID);
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_ENQUEUE_READ_FROM_TYPE_ANY_def MID_ENQUEUE_READ_FROM_TYPE_ANY = new MID_ENQUEUE_READ_FROM_TYPE_ANY_def();
            public class MID_ENQUEUE_READ_FROM_TYPE_ANY_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FROM_TYPE_ANY.SQL"

                private intParameter ENQUEUE_TYPE;

                public MID_ENQUEUE_READ_FROM_TYPE_ANY_def()
                {
                    base.procedureName = "MID_ENQUEUE_READ_FROM_TYPE_ANY";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
                    ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? ENQUEUE_TYPE
                                      )
                {
                    lock (typeof(MID_ENQUEUE_READ_FROM_TYPE_ANY_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_ENQUEUE_INSERT_def MID_ENQUEUE_INSERT = new MID_ENQUEUE_INSERT_def();
			public class MID_ENQUEUE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_def MID_ENQUEUE_DELETE = new MID_ENQUEUE_DELETE_def();
			public class MID_ENQUEUE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
                private intParameter USER_RID;
			
			    public MID_ENQUEUE_DELETE_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? RID,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_HEADER_def MID_ENQUEUE_DELETE_FOR_HEADER = new MID_ENQUEUE_DELETE_FOR_HEADER_def();
			public class MID_ENQUEUE_DELETE_FOR_HEADER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_HEADER.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
                private longParameter OWNING_TRANID;
			
			    public MID_ENQUEUE_DELETE_FOR_HEADER_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_HEADER";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			        OWNING_TRANID = new longParameter("@OWNING_TRANID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID,
			                      long? OWNING_TRANID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_HEADER_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        this.OWNING_TRANID.SetValue(OWNING_TRANID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER_def MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER = new MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER_def();
			public class MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter USER_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_TYPE_AND_USER_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_USER_def MID_ENQUEUE_DELETE_FOR_USER = new MID_ENQUEUE_DELETE_FOR_USER_def();
			public class MID_ENQUEUE_DELETE_FOR_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_USER_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_USER";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD_def MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD = new MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD_def();
            public class MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD.SQL"

                private intParameter USER_RID;
                private intParameter OWNING_THREADID;

                public MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD_def()
                {
                    base.procedureName = "MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  int? USER_RID,
                                  int? OWNING_THREADID
                                  )
                {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }


			public static MID_ENQUEUE_DELETE_FOR_PROCESS_ID_def MID_ENQUEUE_DELETE_FOR_PROCESS_ID = new MID_ENQUEUE_DELETE_FOR_PROCESS_ID_def();
			public class MID_ENQUEUE_DELETE_FOR_PROCESS_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_PROCESS_ID.SQL"

                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_DELETE_FOR_PROCESS_ID_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_PROCESS_ID";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? OWNING_THREADID)
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_PROCESS_ID_def))
                    {
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_READ_FOR_PLAN_def MID_ENQUEUE_READ_FOR_PLAN = new MID_ENQUEUE_READ_FOR_PLAN_def();
			public class MID_ENQUEUE_READ_FOR_PLAN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FOR_PLAN.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter START_WEEK;
                private intParameter END_WEEK;
			
			    public MID_ENQUEUE_READ_FOR_PLAN_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FOR_PLAN";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        START_WEEK = new intParameter("@START_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? ENQUEUE_TYPE,
			                          int? HN_RID,
			                          int? FV_RID,
			                          int? START_WEEK,
			                          int? END_WEEK
			                          )
			    {
                    lock (typeof(MID_ENQUEUE_READ_FOR_PLAN_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.START_WEEK.SetValue(START_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_PLAN_def MID_ENQUEUE_INSERT_FOR_PLAN = new MID_ENQUEUE_INSERT_FOR_PLAN_def();
			public class MID_ENQUEUE_INSERT_FOR_PLAN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_PLAN.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter START_WEEK;
                private intParameter END_WEEK;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_FOR_PLAN_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_PLAN";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        START_WEEK = new intParameter("@START_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? START_WEEK,
			                      int? END_WEEK,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_PLAN_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.START_WEEK.SetValue(START_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_PLAN_def MID_ENQUEUE_DELETE_FOR_PLAN = new MID_ENQUEUE_DELETE_FOR_PLAN_def();
			public class MID_ENQUEUE_DELETE_FOR_PLAN_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_PLAN.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter START_WEEK;
                private intParameter END_WEEK;
			
			    public MID_ENQUEUE_DELETE_FOR_PLAN_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_PLAN";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        START_WEEK = new intParameter("@START_WEEK", base.inputParameterList);
			        END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? START_WEEK,
			                      int? END_WEEK
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_PLAN_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.START_WEEK.SetValue(START_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER_def MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER = new MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER_def();
			public class MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter USER_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_READ_FOR_HIERARCHY_def MID_ENQUEUE_READ_FOR_HIERARCHY = new MID_ENQUEUE_READ_FOR_HIERARCHY_def();
			public class MID_ENQUEUE_READ_FOR_HIERARCHY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FOR_HIERARCHY.SQL"

                private intParameter PH_RID;
			
			    public MID_ENQUEUE_READ_FOR_HIERARCHY_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FOR_HIERARCHY";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? PH_RID)
			    {
                    lock (typeof(MID_ENQUEUE_READ_FOR_HIERARCHY_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_HIERARCHY_def MID_ENQUEUE_INSERT_FOR_HIERARCHY = new MID_ENQUEUE_INSERT_FOR_HIERARCHY_def();
			public class MID_ENQUEUE_INSERT_FOR_HIERARCHY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_HIERARCHY.SQL"

                private intParameter PH_RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_FOR_HIERARCHY_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_HIERARCHY";
			        base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("MID_ENQUEUE");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PH_RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_HIERARCHY_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_HIERARCHY_def MID_ENQUEUE_DELETE_FOR_HIERARCHY = new MID_ENQUEUE_DELETE_FOR_HIERARCHY_def();
			public class MID_ENQUEUE_DELETE_FOR_HIERARCHY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_HIERARCHY.SQL"

                private intParameter PH_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_HIERARCHY_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_HIERARCHY";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MID_ENQUEUE");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? PH_RID)
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_HIERARCHY_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_READ_FOR_HIERARCHY_NODES_def MID_ENQUEUE_READ_FOR_HIERARCHY_NODES = new MID_ENQUEUE_READ_FOR_HIERARCHY_NODES_def();
			public class MID_ENQUEUE_READ_FOR_HIERARCHY_NODES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FOR_HIERARCHY_NODES.SQL"

                private intParameter HN_RID;
			
			    public MID_ENQUEUE_READ_FOR_HIERARCHY_NODES_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FOR_HIERARCHY_NODES";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_ENQUEUE_READ_FOR_HIERARCHY_NODES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_ANY_ANCESTORS_LOCKED_def SP_MID_ANY_ANCESTORS_LOCKED = new SP_MID_ANY_ANCESTORS_LOCKED_def();
            public class SP_MID_ANY_ANCESTORS_LOCKED_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ANY_ANCESTORS_LOCKED.SQL"

                private intParameter HN_RID;
			
			    public SP_MID_ANY_ANCESTORS_LOCKED_def()
			    {
                    base.procedureName = "SP_MID_ANY_ANCESTORS_LOCKED";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(SP_MID_ANY_ANCESTORS_LOCKED_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_ANY_DESCENDANTS_LOCKED_def SP_MID_ANY_DESCENDANTS_LOCKED = new SP_MID_ANY_DESCENDANTS_LOCKED_def();
            public class SP_MID_ANY_DESCENDANTS_LOCKED_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ANY_DESCENDANTS_LOCKED.SQL"

                private intParameter HN_RID;
			
			    public SP_MID_ANY_DESCENDANTS_LOCKED_def()
			    {
                    base.procedureName = "SP_MID_ANY_DESCENDANTS_LOCKED";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_ENQUEUE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(SP_MID_ANY_DESCENDANTS_LOCKED_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES_def MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES = new MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES_def();
			public class MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES.SQL"

                private intParameter HN_RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ENQUEUE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES_def MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES = new MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES_def();
			public class MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES.SQL"

                private intParameter HN_RID;
                private intParameter USER_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ENQUEUE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH_def MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH = new MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH_def();
			public class MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH.SQL"

                private intParameter PH_RID;
                private intParameter HN_RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ENQUEUE");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PH_RID,
			                      int? HN_RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH_def MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH = new MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH_def();
			public class MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH.SQL"

                private intParameter PH_RID;
                private intParameter HN_RID;
			
			    public MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ENQUEUE");
			        PH_RID = new intParameter("@PH_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? PH_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_READ_FOR_MODEL_def MID_ENQUEUE_READ_FOR_MODEL = new MID_ENQUEUE_READ_FOR_MODEL_def();
			public class MID_ENQUEUE_READ_FOR_MODEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_READ_FOR_MODEL.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
			
			    public MID_ENQUEUE_READ_FOR_MODEL_def()
			    {
			        base.procedureName = "MID_ENQUEUE_READ_FOR_MODEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? ENQUEUE_TYPE,
			                          int? RID
			                          )
			    {
                    lock (typeof(MID_ENQUEUE_READ_FOR_MODEL_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_MODEL_def MID_ENQUEUE_INSERT_FOR_MODEL = new MID_ENQUEUE_INSERT_FOR_MODEL_def();
			public class MID_ENQUEUE_INSERT_FOR_MODEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_MODEL.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
                private intParameter USER_RID;
                private intParameter OWNING_THREADID;
			
			    public MID_ENQUEUE_INSERT_FOR_MODEL_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_MODEL";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        OWNING_THREADID = new intParameter("@OWNING_THREADID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? RID,
			                      int? USER_RID,
			                      int? OWNING_THREADID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_MODEL_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNING_THREADID.SetValue(OWNING_THREADID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_FOR_MODEL_def MID_ENQUEUE_DELETE_FOR_MODEL = new MID_ENQUEUE_DELETE_FOR_MODEL_def();
			public class MID_ENQUEUE_DELETE_FOR_MODEL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_FOR_MODEL.SQL"

                private intParameter ENQUEUE_TYPE;
                private intParameter RID;
			
			    public MID_ENQUEUE_DELETE_FOR_MODEL_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_FOR_MODEL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ENQUEUE");
			        ENQUEUE_TYPE = new intParameter("@ENQUEUE_TYPE", base.inputParameterList);
			        RID = new intParameter("@RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? ENQUEUE_TYPE,
			                      int? RID
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_FOR_MODEL_def))
                    {
                        this.ENQUEUE_TYPE.SetValue(ENQUEUE_TYPE);
                        this.RID.SetValue(RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_DELETE_ALL_def MID_ENQUEUE_DELETE_ALL = new MID_ENQUEUE_DELETE_ALL_def();
			public class MID_ENQUEUE_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_DELETE_ALL.SQL"

			
			    public MID_ENQUEUE_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_ENQUEUE_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ENQUEUE");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_ENQUEUE_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_VIRTUAL_LOCK_DELETE_ALL_def MID_VIRTUAL_LOCK_DELETE_ALL = new MID_VIRTUAL_LOCK_DELETE_ALL_def();
			public class MID_VIRTUAL_LOCK_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VIRTUAL_LOCK_DELETE_ALL.SQL"

			
			    public MID_VIRTUAL_LOCK_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_VIRTUAL_LOCK_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("VIRTUAL_LOCK");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_VIRTUAL_LOCK_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ENQUEUE_INSERT_FOR_HEADERS_def MID_ENQUEUE_INSERT_FOR_HEADERS = new MID_ENQUEUE_INSERT_FOR_HEADERS_def();
			public class MID_ENQUEUE_INSERT_FOR_HEADERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ENQUEUE_INSERT_FOR_HEADERS.SQL"

                private intParameter EnqType;
                private intParameter UserRID;
                private intParameter ThreadID;
                private longParameter TranID;
                private tableParameter HDR_RID_LIST;
                private intParameter ReturnCode; //Declare Output Parameter
			
			    public MID_ENQUEUE_INSERT_FOR_HEADERS_def()
			    {
			        base.procedureName = "MID_ENQUEUE_INSERT_FOR_HEADERS";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ENQUEUE");
                    EnqType = new intParameter("@EnqType", base.inputParameterList);
                    UserRID = new intParameter("@UserRID", base.inputParameterList);
                    ThreadID = new intParameter("@ThreadID", base.inputParameterList);
                    TranID = new longParameter("@TranID", base.inputParameterList);
			        HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public DataTable InsertAndRead(DatabaseAccess _dba,
                                  ref int returnCode,
                                  int? EnqType,
                                  int? UserRID,
                                  int? ThreadID,
                                  long? TranID,
			                      DataTable HDR_RID_LIST
			                      )
			    {
                    lock (typeof(MID_ENQUEUE_INSERT_FOR_HEADERS_def))
                    {
                        this.EnqType.SetValue(EnqType);
                        this.UserRID.SetValue(UserRID);
                        this.ThreadID.SetValue(ThreadID);
                        this.TranID.SetValue(TranID);
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        this.ReturnCode.SetValue(-200); //Initialize Output Parameter
                        DataTable dt = ExecuteStoredProcedureForInsertAndRead(_dba);
                        returnCode = (int)this.ReturnCode.Value;
                        return dt;
                    }
			    }
			}


            public static MID_IDENTIFY_HDRS_FOR_ENQ_READ_def MID_IDENTIFY_HDRS_FOR_ENQ_READ = new MID_IDENTIFY_HDRS_FOR_ENQ_READ_def();
            public class MID_IDENTIFY_HDRS_FOR_ENQ_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_IDENTIFY_HDRS_FOR_ENQ_READ.SQL"

                private tableParameter HDR_RID_LIST;
                private intParameter ReturnCode; //Declare Output Parameter
			
			    public MID_IDENTIFY_HDRS_FOR_ENQ_READ_def()
			    {
			        base.procedureName = "MID_IDENTIFY_HDRS_FOR_ENQ_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("MASTER_HEADER");
			        HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable HDR_RID_LIST)
			    {
                    lock (typeof(MID_IDENTIFY_HDRS_FOR_ENQ_READ_def))
                    {
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
