using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class GridViewData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_GRID_VIEW_READ_def MID_GRID_VIEW_READ = new MID_GRID_VIEW_READ_def();
            public class MID_GRID_VIEW_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ.SQL"

                private intParameter VIEW_RID;

                public MID_GRID_VIEW_READ_def()
                {
                    base.procedureName = "MID_GRID_VIEW_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GRID_VIEW");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
                {
                    lock (typeof(MID_GRID_VIEW_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_GRID_VIEW_READ_WITH_GLOBAL_USER_def MID_GRID_VIEW_READ_WITH_GLOBAL_USER = new MID_GRID_VIEW_READ_WITH_GLOBAL_USER_def();
			public class MID_GRID_VIEW_READ_WITH_GLOBAL_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ_WITH_GLOBAL_USER.SQL"

			    private intParameter LAYOUT_ID;
			    private tableParameter USER_RID_LIST;
			
			    public MID_GRID_VIEW_READ_WITH_GLOBAL_USER_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_READ_WITH_GLOBAL_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GRID_VIEW");
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? LAYOUT_ID,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_GRID_VIEW_READ_WITH_GLOBAL_USER_def))
                    {
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GRID_VIEW_READ_FROM_USERS_def MID_GRID_VIEW_READ_FROM_USERS = new MID_GRID_VIEW_READ_FROM_USERS_def();
			public class MID_GRID_VIEW_READ_FROM_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ_FROM_USERS.SQL"

			    private intParameter LAYOUT_ID;
			    private tableParameter USER_RID_LIST;
			
			    public MID_GRID_VIEW_READ_FROM_USERS_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_READ_FROM_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GRID_VIEW");
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? LAYOUT_ID,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_GRID_VIEW_READ_FROM_USERS_def))
                    {
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1313-MD -jsobek -Header Filters
            public static MID_GRID_VIEW_INSERT_def MID_GRID_VIEW_INSERT = new MID_GRID_VIEW_INSERT_def();
            public class MID_GRID_VIEW_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_INSERT.SQL"

			    private intParameter USER_RID;
			    private intParameter LAYOUT_ID;
			    private stringParameter VIEW_ID;
			    private charParameter SHOW_DETAILS;
			    private intParameter GROUP_BY;
			    private intParameter GROUP_BY_SECONDARY;
			    private charParameter IS_SEQUENTIAL;
                private intParameter WORKSPACE_FILTER_RID;
                private intParameter USE_FILTER_SORTING;
			    private intParameter VIEW_RID; //Declare Output Parameter
			
			    public MID_GRID_VIEW_INSERT_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("GRID_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
			        VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
			        SHOW_DETAILS = new charParameter("@SHOW_DETAILS", base.inputParameterList);
			        GROUP_BY = new intParameter("@GROUP_BY", base.inputParameterList);
			        GROUP_BY_SECONDARY = new intParameter("@GROUP_BY_SECONDARY", base.inputParameterList);
			        IS_SEQUENTIAL = new charParameter("@IS_SEQUENTIAL", base.inputParameterList);
                    WORKSPACE_FILTER_RID = new intParameter("@WORKSPACE_FILTER_RID", base.inputParameterList);
                    USE_FILTER_SORTING = new intParameter("@USE_FILTER_SORTING", base.inputParameterList);
			        VIEW_RID = new intParameter("@VIEW_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? USER_RID,
			                                  int? LAYOUT_ID,
			                                  string VIEW_ID,
			                                  char? SHOW_DETAILS,
			                                  int? GROUP_BY,
			                                  int? GROUP_BY_SECONDARY,
			                                  char? IS_SEQUENTIAL,
                                              int? WORKSPACE_FILTER_RID,
                                              int? USE_FILTER_SORTING
			                                  )
			    {
                    lock (typeof(MID_GRID_VIEW_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        this.SHOW_DETAILS.SetValue(SHOW_DETAILS);
                        this.GROUP_BY.SetValue(GROUP_BY);
                        this.GROUP_BY_SECONDARY.SetValue(GROUP_BY_SECONDARY);
                        this.IS_SEQUENTIAL.SetValue(IS_SEQUENTIAL);
                        this.WORKSPACE_FILTER_RID.SetValue(WORKSPACE_FILTER_RID);
                        this.USE_FILTER_SORTING.SetValue(USE_FILTER_SORTING);
                        this.VIEW_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_GRID_VIEW_UPDATE_def MID_GRID_VIEW_UPDATE = new MID_GRID_VIEW_UPDATE_def();
			public class MID_GRID_VIEW_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_UPDATE.SQL"

			    private intParameter VIEW_RID;
			    private charParameter SHOW_DETAILS;
			    private intParameter GROUP_BY;
			    private intParameter GROUP_BY_SECONDARY;
			    private charParameter IS_SEQUENTIAL;
                private intParameter WORKSPACE_FILTER_RID;
                private intParameter USE_FILTER_SORTING;
			  
			    public MID_GRID_VIEW_UPDATE_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("GRID_VIEW");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        SHOW_DETAILS = new charParameter("@SHOW_DETAILS", base.inputParameterList);
			        GROUP_BY = new intParameter("@GROUP_BY", base.inputParameterList);
			        GROUP_BY_SECONDARY = new intParameter("@GROUP_BY_SECONDARY", base.inputParameterList);
			        IS_SEQUENTIAL = new charParameter("@IS_SEQUENTIAL", base.inputParameterList);
                    WORKSPACE_FILTER_RID = new intParameter("@WORKSPACE_FILTER_RID", base.inputParameterList);
                    USE_FILTER_SORTING = new intParameter("@USE_FILTER_SORTING", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      char? SHOW_DETAILS,
			                      int? GROUP_BY,
			                      int? GROUP_BY_SECONDARY,
			                      char? IS_SEQUENTIAL,
                                  int? WORKSPACE_FILTER_RID,
                                  int? USE_FILTER_SORTING
			                      )
			    {
                    lock (typeof(MID_GRID_VIEW_UPDATE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.SHOW_DETAILS.SetValue(SHOW_DETAILS);
                        this.GROUP_BY.SetValue(GROUP_BY);
                        this.GROUP_BY_SECONDARY.SetValue(GROUP_BY_SECONDARY);
                        this.IS_SEQUENTIAL.SetValue(IS_SEQUENTIAL);
                        this.WORKSPACE_FILTER_RID.SetValue(WORKSPACE_FILTER_RID);
                        this.USE_FILTER_SORTING.SetValue(USE_FILTER_SORTING);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}
            //End TT#1313-MD -jsobek -Header Filters

            public static MID_GRID_VIEW_READ_FROM_ID_AND_USER_def MID_GRID_VIEW_READ_FROM_ID_AND_USER = new MID_GRID_VIEW_READ_FROM_ID_AND_USER_def();
            public class MID_GRID_VIEW_READ_FROM_ID_AND_USER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ_FROM_ID_AND_USER.SQL"

                private intParameter USER_RID;
                private intParameter LAYOUT_ID;
                private stringParameter VIEW_ID;

                public MID_GRID_VIEW_READ_FROM_ID_AND_USER_def()
                {
                    base.procedureName = "MID_GRID_VIEW_READ_FROM_ID_AND_USER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GRID_VIEW");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
                    VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? USER_RID,
                                      int? LAYOUT_ID,
                                      string VIEW_ID
                                      )
                {
                    lock (typeof(MID_GRID_VIEW_READ_FROM_ID_AND_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


			public static MID_GRID_VIEW_DETAIL_READ_def MID_GRID_VIEW_DETAIL_READ = new MID_GRID_VIEW_DETAIL_READ_def();
			public class MID_GRID_VIEW_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_DETAIL_READ.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_GRID_VIEW_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GRID_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_GRID_VIEW_DETAIL_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION_def MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION = new MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION_def();
			public class MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GRID_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GRID_VIEW_DETAIL_INSERT_def MID_GRID_VIEW_DETAIL_INSERT = new MID_GRID_VIEW_DETAIL_INSERT_def();
			public class MID_GRID_VIEW_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_DETAIL_INSERT.SQL"

			    private intParameter VIEW_RID;
			    private stringParameter BAND_KEY;
			    private stringParameter COLUMN_KEY;
			    private intParameter VISIBLE_POSITION;
			    private charParameter IS_HIDDEN;
			    private charParameter IS_GROUPBY_COL;
			    private intParameter SORT_DIRECTION;
			    private intParameter SORT_SEQUENCE;
			    private intParameter WIDTH;
			    private stringParameter COLUMN_TYPE;
			    private intParameter HCG_RID;
			
			    public MID_GRID_VIEW_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("GRID_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        BAND_KEY = new stringParameter("@BAND_KEY", base.inputParameterList);
			        COLUMN_KEY = new stringParameter("@COLUMN_KEY", base.inputParameterList);
			        VISIBLE_POSITION = new intParameter("@VISIBLE_POSITION", base.inputParameterList);
			        IS_HIDDEN = new charParameter("@IS_HIDDEN", base.inputParameterList);
			        IS_GROUPBY_COL = new charParameter("@IS_GROUPBY_COL", base.inputParameterList);
			        SORT_DIRECTION = new intParameter("@SORT_DIRECTION", base.inputParameterList);
			        SORT_SEQUENCE = new intParameter("@SORT_SEQUENCE", base.inputParameterList);
			        WIDTH = new intParameter("@WIDTH", base.inputParameterList);
			        COLUMN_TYPE = new stringParameter("@COLUMN_TYPE", base.inputParameterList);
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      string BAND_KEY,
			                      string COLUMN_KEY,
			                      int? VISIBLE_POSITION,
			                      char? IS_HIDDEN,
			                      char? IS_GROUPBY_COL,
			                      int? SORT_DIRECTION,
			                      int? SORT_SEQUENCE,
			                      int? WIDTH,
			                      string COLUMN_TYPE,
			                      int? HCG_RID
			                      )
			    {
                    lock (typeof(MID_GRID_VIEW_DETAIL_INSERT_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.BAND_KEY.SetValue(BAND_KEY);
                        this.COLUMN_KEY.SetValue(COLUMN_KEY);
                        this.VISIBLE_POSITION.SetValue(VISIBLE_POSITION);
                        this.IS_HIDDEN.SetValue(IS_HIDDEN);
                        this.IS_GROUPBY_COL.SetValue(IS_GROUPBY_COL);
                        this.SORT_DIRECTION.SetValue(SORT_DIRECTION);
                        this.SORT_SEQUENCE.SetValue(SORT_SEQUENCE);
                        this.WIDTH.SetValue(WIDTH);
                        this.COLUMN_TYPE.SetValue(COLUMN_TYPE);
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_GRID_VIEW_DETAIL_DELETE_def MID_GRID_VIEW_DETAIL_DELETE = new MID_GRID_VIEW_DETAIL_DELETE_def();
			public class MID_GRID_VIEW_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_DETAIL_DELETE.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_GRID_VIEW_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_GRID_VIEW_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GRID_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_GRID_VIEW_DETAIL_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            //Begin TT#1313-MD -jsobek -Header Filters
            public static MID_GRID_VIEW_DELETE_def MID_GRID_VIEW_DELETE = new MID_GRID_VIEW_DELETE_def();
            public class MID_GRID_VIEW_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_DELETE.SQL"

			    private intParameter VIEW_RID;

                public MID_GRID_VIEW_DELETE_def()
			    {
                    base.procedureName = "MID_GRID_VIEW_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GRID_VIEW");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_GRID_VIEW_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}
            //End TT#1313-MD -jsobek -Header Filters

            //Begin TT#1313-MD -jsobek -Header Filters
            //public static MID_GRID_VIEW_FILTER_READ_COUNT_def MID_GRID_VIEW_FILTER_READ_COUNT = new MID_GRID_VIEW_FILTER_READ_COUNT_def();
            //public class MID_GRID_VIEW_FILTER_READ_COUNT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_FILTER_READ_COUNT.SQL"

            //    public intParameter VIEW_RID;
			
            //    public MID_GRID_VIEW_FILTER_READ_COUNT_def()
            //    {
            //        base.procedureName = "MID_GRID_VIEW_FILTER_READ_COUNT";
            //        base.procedureType = storedProcedureTypes.RecordCount;
            //        base.tableNames.Add("GRID_VIEW_FILTER");
            //        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
            //    }
			
            //    public int ReadRecordCount(DatabaseAccess _dba, int? VIEW_RID)
            //    {
            //        this.VIEW_RID.SetValue(VIEW_RID);
            //        return ExecuteStoredProcedureForRecordCount(_dba);
            //    }
            //}

            public static MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID_def MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID = new MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID_def();
            public class MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID.SQL"

                private intParameter VIEW_RID;

                public MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID_def()
                {
                    base.procedureName = "MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GRID_VIEW");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
                {
                    lock (typeof(MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
           

            //public static SP_MID_GRID_VIEW_FILTER_DELETE_def SP_MID_GRID_VIEW_FILTER_DELETE = new SP_MID_GRID_VIEW_FILTER_DELETE_def();
            //public class SP_MID_GRID_VIEW_FILTER_DELETE_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GRID_VIEW_FILTER_DELETE.SQL"

            //    public intParameter VIEW_RID;
			
            //    public SP_MID_GRID_VIEW_FILTER_DELETE_def()
            //    {
            //        base.procedureName = "SP_MID_GRID_VIEW_FILTER_DELETE";
            //        base.procedureType = storedProcedureTypes.Delete;
            //        base.tableNames.Add("GRID_VIEW_FILTER");
            //        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
            //    }
			
            //    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
            //    {
            //        this.VIEW_RID.SetValue(VIEW_RID);
            //        return ExecuteStoredProcedureForDelete(_dba);
            //    }
            //}
            

            //public static MID_GRID_VIEW_FILTER_INSERT_def MID_GRID_VIEW_FILTER_INSERT = new MID_GRID_VIEW_FILTER_INSERT_def();
            //public class MID_GRID_VIEW_FILTER_INSERT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_FILTER_INSERT.SQL"

            //    public intParameter VIEW_RID;
            //    public intParameter USER_RID;
			
            //    public MID_GRID_VIEW_FILTER_INSERT_def()
            //    {
            //        base.procedureName = "MID_GRID_VIEW_FILTER_INSERT";
            //        base.procedureType = storedProcedureTypes.Insert;
            //        base.tableNames.Add("GRID_VIEW_FILTER");
            //        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
            //    }
			
            //    public int Insert(DatabaseAccess _dba, 
            //                      int? VIEW_RID,
            //                      int? USER_RID
            //                      )
            //    {
            //        this.VIEW_RID.SetValue(VIEW_RID);
            //        this.USER_RID.SetValue(USER_RID);
            //        return ExecuteStoredProcedureForInsert(_dba);
            //    }
            //}

            //public static MID_GRID_VIEW_FILTER_TYPES_INSERT_def MID_GRID_VIEW_FILTER_TYPES_INSERT = new MID_GRID_VIEW_FILTER_TYPES_INSERT_def();
            //public class MID_GRID_VIEW_FILTER_TYPES_INSERT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_FILTER_TYPES_INSERT.SQL"

            //    public intParameter VIEW_RID;
            //    public intParameter USER_RID;
			
            //    public MID_GRID_VIEW_FILTER_TYPES_INSERT_def()
            //    {
            //        base.procedureName = "MID_GRID_VIEW_FILTER_TYPES_INSERT";
            //        base.procedureType = storedProcedureTypes.Insert;
            //        base.tableNames.Add("GRID_VIEW_FILTER_TYPES");
            //        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
            //    }
			
            //    public int Insert(DatabaseAccess _dba, 
            //                      int? VIEW_RID,
            //                      int? USER_RID
            //                      )
            //    {
            //        this.VIEW_RID.SetValue(VIEW_RID);
            //        this.USER_RID.SetValue(USER_RID);
            //        return ExecuteStoredProcedureForInsert(_dba);
            //    }
            //}

            //public static MID_GRID_VIEW_FILTER_STATUS_INSERT_def MID_GRID_VIEW_FILTER_STATUS_INSERT = new MID_GRID_VIEW_FILTER_STATUS_INSERT_def();
            //public class MID_GRID_VIEW_FILTER_STATUS_INSERT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_FILTER_STATUS_INSERT.SQL"

            //    public intParameter VIEW_RID;
            //    public intParameter USER_RID;
			
            //    public MID_GRID_VIEW_FILTER_STATUS_INSERT_def()
            //    {
            //        base.procedureName = "MID_GRID_VIEW_FILTER_STATUS_INSERT";
            //        base.procedureType = storedProcedureTypes.Insert;
            //        base.tableNames.Add("GRID_VIEW_FILTER_STATUS");
            //        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
            //    }
			
            //    public int Insert(DatabaseAccess _dba, 
            //                      int? VIEW_RID,
            //                      int? USER_RID
            //                      )
            //    {
            //        this.VIEW_RID.SetValue(VIEW_RID);
            //        this.USER_RID.SetValue(USER_RID);
            //        return ExecuteStoredProcedureForInsert(_dba);
            //    }
            //}
            //End TT#1313-MD -jsobek -Header Filters

            public static MID_GRID_VIEW_SPLITTER_READ_def MID_GRID_VIEW_SPLITTER_READ = new MID_GRID_VIEW_SPLITTER_READ_def();
            public class MID_GRID_VIEW_SPLITTER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_SPLITTER_READ.SQL"

                private intParameter VIEW_RID;
                private intParameter USER_RID;
                private intParameter LAYOUT_ID;

                public MID_GRID_VIEW_SPLITTER_READ_def()
                {
                    base.procedureName = "MID_GRID_VIEW_SPLITTER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GRID_VIEW_SPLITTER");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
                }

                public DataTable Read(
                    DatabaseAccess _dba,
                    int? VIEW_RID,
                    int? USER_RID,
                    int? LAYOUT_ID
                    )
                {
                    lock (typeof(MID_GRID_VIEW_SPLITTER_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_GRID_VIEW_SPLITTER_INSERT_def MID_GRID_VIEW_SPLITTER_INSERT = new MID_GRID_VIEW_SPLITTER_INSERT_def();
            public class MID_GRID_VIEW_SPLITTER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_SPLITTER_INSERT.SQL"

                private intParameter VIEW_RID;
                private intParameter USER_RID;
                private intParameter LAYOUT_ID;
                private charParameter SPLITTER_TYPE_IND;
                private intParameter SPLITTER_SEQUENCE;
                private floatParameter SPLITTER_PERCENTAGE;


                public MID_GRID_VIEW_SPLITTER_INSERT_def()
                {
                    base.procedureName = "MID_GRID_VIEW_SPLITTER_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("GRID_VIEW_SPLITTER");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
                    SPLITTER_TYPE_IND = new charParameter("@SPLITTER_TYPE_IND", base.inputParameterList);
                    SPLITTER_SEQUENCE = new intParameter("@SPLITTER_SEQUENCE", base.inputParameterList);
                    SPLITTER_PERCENTAGE = new floatParameter("@SPLITTER_PERCENTAGE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? VIEW_RID,
                                  int? USER_RID,
                                  int? LAYOUT_ID,
                                  char SPLITTER_TYPE_IND,
                                  int? SPLITTER_SEQUENCE,
                                  double? SPLITTER_PERCENTAGE
                                  )
                {
                    lock (typeof(MID_GRID_VIEW_SPLITTER_INSERT_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        this.SPLITTER_TYPE_IND.SetValue(SPLITTER_TYPE_IND);
                        this.SPLITTER_SEQUENCE.SetValue(SPLITTER_SEQUENCE);
                        this.SPLITTER_PERCENTAGE.SetValue(SPLITTER_PERCENTAGE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_GRID_VIEW_SPLITTER_DELETE_def MID_GRID_VIEW_SPLITTER_DELETE = new MID_GRID_VIEW_SPLITTER_DELETE_def();
            public class MID_GRID_VIEW_SPLITTER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_SPLITTER_DELETE.SQL"

                private intParameter VIEW_RID;
                private intParameter USER_RID;
                private intParameter LAYOUT_ID;

                public MID_GRID_VIEW_SPLITTER_DELETE_def()
                {
                    base.procedureName = "MID_GRID_VIEW_SPLITTER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("GRID_VIEW_SPLITTER");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    LAYOUT_ID = new intParameter("@LAYOUT_ID", base.inputParameterList);
                }

                public int Delete(
                    DatabaseAccess _dba,
                    int? VIEW_RID,
                    int? USER_RID,
                    int? LAYOUT_ID
                    )
                {
                    lock (typeof(MID_GRID_VIEW_SPLITTER_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.LAYOUT_ID.SetValue(LAYOUT_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
