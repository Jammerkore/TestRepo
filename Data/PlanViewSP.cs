using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class PlanViewData : DataLayer
    {
        protected static class StoredProcedures
        {

			public static MID_PLAN_VIEW_DETAIL_INSERT_def MID_PLAN_VIEW_DETAIL_INSERT = new MID_PLAN_VIEW_DETAIL_INSERT_def();
			public class MID_PLAN_VIEW_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_DETAIL_INSERT.SQL"

			    private intParameter VIEW_RID;
			    private intParameter AXIS;
			    private intParameter AXIS_SEQUENCE;
			    private intParameter PROFILE_TYPE;
			    private intParameter PROFILE_KEY;
			
			    public MID_PLAN_VIEW_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("PLAN_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        AXIS = new intParameter("@AXIS", base.inputParameterList);
			        AXIS_SEQUENCE = new intParameter("@AXIS_SEQUENCE", base.inputParameterList);
			        PROFILE_TYPE = new intParameter("@PROFILE_TYPE", base.inputParameterList);
			        PROFILE_KEY = new intParameter("@PROFILE_KEY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      int? AXIS,
			                      int? AXIS_SEQUENCE,
			                      int? PROFILE_TYPE,
			                      int? PROFILE_KEY
			                      )
			    {
                    lock (typeof(MID_PLAN_VIEW_DETAIL_INSERT_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.AXIS.SetValue(AXIS);
                        this.AXIS_SEQUENCE.SetValue(AXIS_SEQUENCE);
                        this.PROFILE_TYPE.SetValue(PROFILE_TYPE);
                        this.PROFILE_KEY.SetValue(PROFILE_KEY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_PLAN_VIEW_DETAIL_DELETE_def MID_PLAN_VIEW_DETAIL_DELETE = new MID_PLAN_VIEW_DETAIL_DELETE_def();
			public class MID_PLAN_VIEW_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_DETAIL_DELETE.SQL"

                private intParameter VIEW_RID;
			
			    public MID_PLAN_VIEW_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PLAN_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_PLAN_VIEW_DETAIL_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_PLAN_VIEW_READ_FROM_USER_LIST_def MID_PLAN_VIEW_READ_FROM_USER_LIST = new MID_PLAN_VIEW_READ_FROM_USER_LIST_def();
			public class MID_PLAN_VIEW_READ_FROM_USER_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_READ_FROM_USER_LIST.SQL"

                private tableParameter USER_LIST;
			
			    public MID_PLAN_VIEW_READ_FROM_USER_LIST_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_READ_FROM_USER_LIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PLAN_VIEW");
			        USER_LIST = new tableParameter("@USER_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable USER_LIST)
			    {
                    lock (typeof(MID_PLAN_VIEW_READ_FROM_USER_LIST_def))
                    {
                        this.USER_LIST.SetValue(USER_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_PLAN_VIEW_INSERT_def SP_MID_PLAN_VIEW_INSERT = new SP_MID_PLAN_VIEW_INSERT_def();
            public class SP_MID_PLAN_VIEW_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_PLAN_VIEW_INSERT.SQL"

                private intParameter USER_RID;
                private stringParameter VIEW_ID;
                private intParameter GROUPBY_TYPE;
                private intParameter VIEW_RID; //Declare Output Parameter

                public SP_MID_PLAN_VIEW_INSERT_def()
                {
                    base.procedureName = "SP_MID_PLAN_VIEW_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("PLAN_VIEW");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
                    GROUPBY_TYPE = new intParameter("@GROUPBY_TYPE", base.inputParameterList);
                    VIEW_RID = new intParameter("@VIEW_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? USER_RID,
                                              string VIEW_ID,
                                              int? GROUPBY_TYPE
                                              )
                {
                    lock (typeof(SP_MID_PLAN_VIEW_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        this.GROUPBY_TYPE.SetValue(GROUPBY_TYPE);
                        this.VIEW_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

			public static MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION_def MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION = new MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION_def();
			public class MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION.SQL"

                private intParameter VIEW_RID;
			
			    public MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION_def()
			    {
			        base.procedureName = "MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_PLAN");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PLAN_VIEW_DELETE_def MID_PLAN_VIEW_DELETE = new MID_PLAN_VIEW_DELETE_def();
			public class MID_PLAN_VIEW_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_DELETE.SQL"

                private intParameter VIEW_RID;
			
			    public MID_PLAN_VIEW_DELETE_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PLAN_VIEW");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_PLAN_VIEW_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_PLAN_VIEW_READ_KEY_def MID_PLAN_VIEW_READ_KEY = new MID_PLAN_VIEW_READ_KEY_def();
			public class MID_PLAN_VIEW_READ_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_READ_KEY.SQL"

                private intParameter USER_RID;
                private stringParameter VIEW_ID;
			
			    public MID_PLAN_VIEW_READ_KEY_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_READ_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PLAN_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          string VIEW_ID
			                          )
			    {
                    lock (typeof(MID_PLAN_VIEW_READ_KEY_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PLAN_VIEW_DETAIL_READ_def MID_PLAN_VIEW_DETAIL_READ = new MID_PLAN_VIEW_DETAIL_READ_def();
			public class MID_PLAN_VIEW_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_DETAIL_READ.SQL"

                private intParameter VIEW_RID;
			
			    public MID_PLAN_VIEW_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_PLAN_VIEW_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PLAN_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_PLAN_VIEW_DETAIL_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


            public static MID_PLAN_VIEW_FORMAT_READ_def MID_PLAN_VIEW_FORMAT_READ = new MID_PLAN_VIEW_FORMAT_READ_def();
            public class MID_PLAN_VIEW_FORMAT_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_FORMAT_READ.SQL"

                private intParameter VIEW_RID;

                public MID_PLAN_VIEW_FORMAT_READ_def()
                {
                    base.procedureName = "MID_PLAN_VIEW_FORMAT_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PLAN_VIEW_FORMAT");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
                {
                    lock (typeof(MID_PLAN_VIEW_FORMAT_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PLAN_VIEW_FORMAT_INSERT_def MID_PLAN_VIEW_FORMAT_INSERT = new MID_PLAN_VIEW_FORMAT_INSERT_def();
            public class MID_PLAN_VIEW_FORMAT_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_FORMAT_INSERT.SQL"

                private intParameter VIEW_RID;
                private intParameter PLAN_BASIS_TYPE;
                private intParameter VARIABLE_NUMBER;
                private intParameter QUANTITY_VARIABLE_KEY;
                private intParameter TIME_PERIOD_TYPE;
                private intParameter TIME_PERIOD_KEY;
                private intParameter VARIABLE_TOTAL_KEY;
                private intParameter WIDTH;

                public MID_PLAN_VIEW_FORMAT_INSERT_def()
                {
                    base.procedureName = "MID_PLAN_VIEW_FORMAT_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PLAN_VIEW_FORMAT");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    PLAN_BASIS_TYPE = new intParameter("@PLAN_BASIS_TYPE", base.inputParameterList);
                    VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
                    QUANTITY_VARIABLE_KEY = new intParameter("@QUANTITY_VARIABLE_KEY", base.inputParameterList);
                    TIME_PERIOD_TYPE = new intParameter("@TIME_PERIOD_TYPE", base.inputParameterList);
                    TIME_PERIOD_KEY = new intParameter("@TIME_PERIOD_KEY", base.inputParameterList);
                    VARIABLE_TOTAL_KEY = new intParameter("@VARIABLE_TOTAL_KEY", base.inputParameterList);
                    WIDTH = new intParameter("@WIDTH", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? VIEW_RID,
                                  int? PLAN_BASIS_TYPE,
                                  int? VARIABLE_NUMBER,
                                  int? QUANTITY_VARIABLE_KEY,
                                  int? TIME_PERIOD_TYPE,
                                  int? TIME_PERIOD_KEY,
                                  int? VARIABLE_TOTAL_KEY,
                                  int? WIDTH
                                  )
                {
                    lock (typeof(MID_PLAN_VIEW_FORMAT_INSERT_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.PLAN_BASIS_TYPE.SetValue(PLAN_BASIS_TYPE);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.QUANTITY_VARIABLE_KEY.SetValue(QUANTITY_VARIABLE_KEY);
                        this.TIME_PERIOD_TYPE.SetValue(TIME_PERIOD_TYPE);
                        this.TIME_PERIOD_KEY.SetValue(TIME_PERIOD_KEY);
                        this.VARIABLE_TOTAL_KEY.SetValue(VARIABLE_TOTAL_KEY);
                        this.WIDTH.SetValue(WIDTH);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PLAN_VIEW_FORMAT_DELETE_def MID_PLAN_VIEW_FORMAT_DELETE = new MID_PLAN_VIEW_FORMAT_DELETE_def();
            public class MID_PLAN_VIEW_FORMAT_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PLAN_VIEW_FORMAT_DELETE.SQL"

                private intParameter VIEW_RID;

                public MID_PLAN_VIEW_FORMAT_DELETE_def()
                {
                    base.procedureName = "MID_PLAN_VIEW_FORMAT_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PLAN_VIEW_FORMAT");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? VIEW_RID)
                {
                    lock (typeof(MID_PLAN_VIEW_FORMAT_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
