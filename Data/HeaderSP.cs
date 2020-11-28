using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class Header : DataLayer
    {
        protected static class StoredProcedures
        {
            //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed

            //public static MID_HEADER_READ_ALL_GROUPS_def MID_HEADER_READ_ALL_GROUPS = new MID_HEADER_READ_ALL_GROUPS_def();
            //public class MID_HEADER_READ_ALL_GROUPS_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ALL_GROUPS.SQL"

            //    public MID_HEADER_READ_ALL_GROUPS_def()
            //    {
            //        base.procedureName = "MID_HEADER_READ_ALL_GROUPS";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("HEADER");
            //    }

            //    public DataTable Read(DatabaseAccess _dba)
            //    {
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}
            //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed

            public static MID_HEADER_GROUP_READ_def MID_HEADER_GROUP_READ = new MID_HEADER_GROUP_READ_def();
            public class MID_HEADER_GROUP_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_GROUP_READ.SQL"

                private intParameter HDR_GROUP_RID;

                public MID_HEADER_GROUP_READ_def()
                {
                    base.procedureName = "MID_HEADER_GROUP_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_GROUP");
                    HDR_GROUP_RID = new intParameter("@HDR_GROUP_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_GROUP_RID)
                {
                    lock (typeof(MID_HEADER_GROUP_READ_def))
                    {
                        this.HDR_GROUP_RID.SetValue(HDR_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_GROUP_CHILDREN_def MID_HEADER_READ_GROUP_CHILDREN = new MID_HEADER_READ_GROUP_CHILDREN_def();
            public class MID_HEADER_READ_GROUP_CHILDREN_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_GROUP_CHILDREN.SQL"

                private intParameter HDR_GROUP_RID;

                public MID_HEADER_READ_GROUP_CHILDREN_def()
                {
                    base.procedureName = "MID_HEADER_READ_GROUP_CHILDREN";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_GROUP_RID = new intParameter("@HDR_GROUP_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_GROUP_RID)
                {
                    lock (typeof(MID_HEADER_READ_GROUP_CHILDREN_def))
                    {
                        this.HDR_GROUP_RID.SetValue(HDR_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_ID_EXISTS_def MID_HEADER_ID_EXISTS = new MID_HEADER_ID_EXISTS_def();
            public class MID_HEADER_ID_EXISTS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_ID_EXISTS.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_ID_EXISTS_def()
                {
                    base.procedureName = "MID_HEADER_ID_EXISTS";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_ID_EXISTS_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_ID_UPPERCASE_EXISTS_def MID_HEADER_ID_UPPERCASE_EXISTS = new MID_HEADER_ID_UPPERCASE_EXISTS_def();
            public class MID_HEADER_ID_UPPERCASE_EXISTS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_ID_UPPERCASE_EXISTS.SQL"

                private stringParameter HDR_ID;
                private intParameter HDR_RID;

                public MID_HEADER_ID_UPPERCASE_EXISTS_def()
                {
                    base.procedureName = "MID_HEADER_ID_UPPERCASE_EXISTS";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, 
                                           string HDR_ID,
                                           int? HDR_RID
                                           )
                {
                    lock (typeof(MID_HEADER_ID_UPPERCASE_EXISTS_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_RID_FROM_ID_def MID_HEADER_READ_RID_FROM_ID = new MID_HEADER_READ_RID_FROM_ID_def();
            public class MID_HEADER_READ_RID_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_RID_FROM_ID.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_READ_RID_FROM_ID_def()
                {
                    base.procedureName = "MID_HEADER_READ_RID_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_RID_FROM_ID_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);


                        if (dt.Rows.Count > 0)
                        {
                            return dt.Rows[0]["HDR_RID"];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                public DataTable Read(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_RID_FROM_ID_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //public static MID_HEADER_READ_RID_FROM_ID_AS_TABLE_def MID_HEADER_READ_RID_FROM_ID_AS_TABLE = new MID_HEADER_READ_RID_FROM_ID_AS_TABLE_def();
            //public class MID_HEADER_READ_RID_FROM_ID_AS_TABLE_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_RID_FROM_ID.SQL"

            //    public stringParameter HDR_ID;

            //    public MID_HEADER_READ_RID_FROM_ID_AS_TABLE_def()
            //    {
            //        base.procedureName = "MID_HEADER_READ_RID_FROM_ID";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("HEADER");
            //        HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
            //    }

            //    public DataTable Read(DatabaseAccess _dba, string HDR_ID)
            //    {
            //        this.HDR_ID.SetValue(HDR_ID);
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}

            public static MID_HEADER_READ_ID_def MID_HEADER_READ_ID = new MID_HEADER_READ_ID_def();
            public class MID_HEADER_READ_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ID.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_READ_ID_def()
                {
                    base.procedureName = "MID_HEADER_READ_ID";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_READ_ID_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_MULTI_FLAG_FROM_ID_def MID_HEADER_READ_MULTI_FLAG_FROM_ID = new MID_HEADER_READ_MULTI_FLAG_FROM_ID_def();
            public class MID_HEADER_READ_MULTI_FLAG_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_MULTI_FLAG_FROM_ID.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_READ_MULTI_FLAG_FROM_ID_def()
                {
                    base.procedureName = "MID_HEADER_READ_MULTI_FLAG_FROM_ID";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_MULTI_FLAG_FROM_ID_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_def MID_HEADER_READ = new MID_HEADER_READ_def();
            public class MID_HEADER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_READ_def()
                {
                    base.procedureName = "MID_HEADER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            // Begin TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.
            public static MID_HEADER_READ_FOR_WORKSPACE_def MID_HEADER_READ_FOR_WORKSPACE = new MID_HEADER_READ_FOR_WORKSPACE_def();
            public class MID_HEADER_READ_FOR_WORKSPACE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FOR_WORKSPACE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_READ_FOR_WORKSPACE_def()
                {
                    base.procedureName = "MID_HEADER_READ_FOR_WORKSPACE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_READ_FOR_WORKSPACE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // End TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.


            public static MID_HEADER_READ_FROM_ID_def MID_HEADER_READ_FROM_ID = new MID_HEADER_READ_FROM_ID_def();
            public class MID_HEADER_READ_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_ID.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_READ_FROM_ID_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_ID_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_ALL_def MID_HEADER_READ_ALL = new MID_HEADER_READ_ALL_def();
            public class MID_HEADER_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ALL.SQL"

                public MID_HEADER_READ_ALL_def()
                {
                    base.procedureName = "MID_HEADER_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_NON_RELEASED_def MID_HEADER_READ_NON_RELEASED = new MID_HEADER_READ_NON_RELEASED_def();
            public class MID_HEADER_READ_NON_RELEASED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_NON_RELEASED.SQL"

                public MID_HEADER_READ_NON_RELEASED_def()
                {
                    base.procedureName = "MID_HEADER_READ_NON_RELEASED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_READ_NON_RELEASED_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static MID_HEADER_READ_RECONCILE_NON_RELEASED_def MID_HEADER_READ_RECONCILE_NON_RELEASED = new MID_HEADER_READ_RECONCILE_NON_RELEASED_def();
            public class MID_HEADER_READ_RECONCILE_NON_RELEASED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_RECONCILE_NON_RELEASED.SQL"

                private tableParameter HEADER_TYPE_LIST;

                public MID_HEADER_READ_RECONCILE_NON_RELEASED_def()
                {
                    base.procedureName = "MID_HEADER_READ_RECONCILE_NON_RELEASED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HEADER_TYPE_LIST = new tableParameter("@HEADER_TYPE_LIST", "DISPLAY_TYPE_TYPE", base.inputParameterList);

                }

                public DataTable Read(DatabaseAccess _dba, DataTable HEADER_TYPE_LIST)
                {
                    lock (typeof(MID_HEADER_READ_RECONCILE_NON_RELEASED_def))
                    {
                        this.HEADER_TYPE_LIST.SetValue(HEADER_TYPE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#1313-MD -jsobek -Header Filters
            //public static SP_MID_GET_HEADERS_FOR_TASKLIST_def SP_MID_GET_HEADERS_FOR_TASKLIST = new SP_MID_GET_HEADERS_FOR_TASKLIST_def();
            //public class SP_MID_GET_HEADERS_FOR_TASKLIST_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_HEADERS_FOR_TASKLIST.SQL"

            //    public intParameter TASKLIST_RID;
            //    public intParameter TASK_SEQUENCE;

            //    public SP_MID_GET_HEADERS_FOR_TASKLIST_def()
            //    {
            //        base.procedureName = "SP_MID_GET_HEADERS_FOR_TASKLIST";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("HEADER");
            //        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
            //        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
            //    }

            //    public DataTable Read(DatabaseAccess _dba, 
            //                          int? TASKLIST_RID,
            //                          int? TASK_SEQUENCE
            //                          )
            //    {
            //        this.TASKLIST_RID.SetValue(TASKLIST_RID);
            //        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}
            //End TT#1313-MD -jsobek -Header Filters

            public static MID_HEADER_READ_FROM_STYLE_NODE_def MID_HEADER_READ_FROM_STYLE_NODE = new MID_HEADER_READ_FROM_STYLE_NODE_def();
            public class MID_HEADER_READ_FROM_STYLE_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_STYLE_NODE.SQL"

                private intParameter STYLE_HNRID;

                public MID_HEADER_READ_FROM_STYLE_NODE_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_STYLE_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    STYLE_HNRID = new intParameter("@STYLE_HNRID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? STYLE_HNRID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_STYLE_NODE_def))
                    {
                        this.STYLE_HNRID.SetValue(STYLE_HNRID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_STYL_HDRS_WITH_COLR_def SP_MID_GET_STYL_HDRS_WITH_COLR = new SP_MID_GET_STYL_HDRS_WITH_COLR_def();
            public class SP_MID_GET_STYL_HDRS_WITH_COLR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_STYL_HDRS_WITH_COLR.SQL"

                private intParameter STYLE_HNRID;
                private intParameter COLOR_CODE_RID;

                public SP_MID_GET_STYL_HDRS_WITH_COLR_def()
                {
                    base.procedureName = "SP_MID_GET_STYL_HDRS_WITH_COLR";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    STYLE_HNRID = new intParameter("@STYLE_HNRID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? STYLE_HNRID,
                                      int? COLOR_CODE_RID
                                      )
                {
                    lock (typeof(SP_MID_GET_STYL_HDRS_WITH_COLR_def))
                    {
                        this.STYLE_HNRID.SetValue(STYLE_HNRID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_FROM_ID_LIKE_def MID_HEADER_READ_FROM_ID_LIKE = new MID_HEADER_READ_FROM_ID_LIKE_def();
            public class MID_HEADER_READ_FROM_ID_LIKE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_ID_LIKE.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_READ_FROM_ID_LIKE_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_ID_LIKE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_ID_LIKE_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_FROM_ID_GREATER_THAN_def MID_HEADER_READ_FROM_ID_GREATER_THAN = new MID_HEADER_READ_FROM_ID_GREATER_THAN_def();
            public class MID_HEADER_READ_FROM_ID_GREATER_THAN_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_ID_GREATER_THAN.SQL"

                private stringParameter HDR_ID;

                public MID_HEADER_READ_FROM_ID_GREATER_THAN_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_ID_GREATER_THAN";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string HDR_ID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_ID_GREATER_THAN_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_CHARGED_TO_INTRANSIT_def MID_HEADER_READ_CHARGED_TO_INTRANSIT = new MID_HEADER_READ_CHARGED_TO_INTRANSIT_def();
            public class MID_HEADER_READ_CHARGED_TO_INTRANSIT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_CHARGED_TO_INTRANSIT.SQL"

                private tableParameter HN_RID_LIST;

                public MID_HEADER_READ_CHARGED_TO_INTRANSIT_def()
                {
                    base.procedureName = "MID_HEADER_READ_CHARGED_TO_INTRANSIT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HN_RID_LIST = new tableParameter("@HN_RID_LIST", "HN_RID_TYPE", base.inputParameterList);

                }

                public DataTable Read(DatabaseAccess _dba, DataTable HN_RID_LIST)
                {
                    lock (typeof(MID_HEADER_READ_CHARGED_TO_INTRANSIT_def))
                    {
                        this.HN_RID_LIST.SetValue(HN_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE_def MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE = new MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE_def();
            public class MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE.SQL"

                public MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE_def()
                {
                    base.procedureName = "MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_FROM_PLAN_NODE_def MID_HEADER_READ_FROM_PLAN_NODE = new MID_HEADER_READ_FROM_PLAN_NODE_def();
            public class MID_HEADER_READ_FROM_PLAN_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_PLAN_NODE.SQL"

                private intParameter PLAN_HNRID;

                public MID_HEADER_READ_FROM_PLAN_NODE_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_PLAN_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    PLAN_HNRID = new intParameter("@PLAN_HNRID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PLAN_HNRID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_PLAN_NODE_def))
                    {
                        this.PLAN_HNRID.SetValue(PLAN_HNRID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_FROM_ON_HAND_NODE_def MID_HEADER_READ_FROM_ON_HAND_NODE = new MID_HEADER_READ_FROM_ON_HAND_NODE_def();
            public class MID_HEADER_READ_FROM_ON_HAND_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_ON_HAND_NODE.SQL"

                private intParameter ON_HAND_HNRID;

                public MID_HEADER_READ_FROM_ON_HAND_NODE_def()
                {
                    base.procedureName = "MID_HEADER_READ_FROM_ON_HAND_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    ON_HAND_HNRID = new intParameter("@ON_HAND_HNRID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? ON_HAND_HNRID)
                {
                    lock (typeof(MID_HEADER_READ_FROM_ON_HAND_NODE_def))
                    {
                        this.ON_HAND_HNRID.SetValue(ON_HAND_HNRID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#1313-MD -jsobek -Header Filters
            //public static SP_MID_GET_ASSORTMENTS_FOR_USER_def SP_MID_GET_ASSORTMENTS_FOR_USER = new SP_MID_GET_ASSORTMENTS_FOR_USER_def();
            //public class SP_MID_GET_ASSORTMENTS_FOR_USER_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_ASSORTMENTS_FOR_USER.SQL"

            //    public intParameter USER_RID;

            //    public SP_MID_GET_ASSORTMENTS_FOR_USER_def()
            //    {
            //        base.procedureName = "SP_MID_GET_ASSORTMENTS_FOR_USER";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("HEADER");
            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
            //    }

            //    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
            //    {
            //        this.USER_RID.SetValue(USER_RID);
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}
            //End TT#1313-MD -jsobek -Header Filters

            public static SP_MID_GET_HEADERS_IN_ASSORTMENT_def SP_MID_GET_HEADERS_IN_ASSORTMENT = new SP_MID_GET_HEADERS_IN_ASSORTMENT_def();
            public class SP_MID_GET_HEADERS_IN_ASSORTMENT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_HEADERS_IN_ASSORTMENT.SQL"

                private intParameter ASRT_RID;

                public SP_MID_GET_HEADERS_IN_ASSORTMENT_def()
                {
                    base.procedureName = "SP_MID_GET_HEADERS_IN_ASSORTMENT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? ASRT_RID)
                {
                    lock (typeof(SP_MID_GET_HEADERS_IN_ASSORTMENT_def))
                    {
                        this.ASRT_RID.SetValue(ASRT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#1313-MD -jsobek -Header Filters

            //public static SP_MID_GET_HEADERS_FOR_USER_def SP_MID_GET_HEADERS_FOR_USER = new SP_MID_GET_HEADERS_FOR_USER_def();
            //public class SP_MID_GET_HEADERS_FOR_USER_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_HEADERS_FOR_USER.SQL"

            //    public intParameter USER_RID;
            //    public intParameter HDR_DATE_TYPE;
            //    public datetimeParameter HDR_FROM_DATE;
            //    public datetimeParameter HDR_TO_DATE;
            //    public intParameter RELEASE_DATE_TYPE;
            //    public datetimeParameter RELEASE_FROM_DATE;
            //    public datetimeParameter RELEASE_TO_DATE;

            //    public SP_MID_GET_HEADERS_FOR_USER_def()
            //    {
            //        base.procedureName = "SP_MID_GET_HEADERS_FOR_USER";
            //        base.procedureType = storedProcedureTypes.Read;
            //        base.tableNames.Add("HEADER");
            //        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
            //        HDR_DATE_TYPE = new intParameter("@HDR_DATE_TYPE", base.inputParameterList);
            //        HDR_FROM_DATE = new datetimeParameter("@HDR_FROM_DATE", base.inputParameterList);
            //        HDR_TO_DATE = new datetimeParameter("@HDR_TO_DATE", base.inputParameterList);
            //        RELEASE_DATE_TYPE = new intParameter("@RELEASE_DATE_TYPE", base.inputParameterList);
            //        RELEASE_FROM_DATE = new datetimeParameter("@RELEASE_FROM_DATE", base.inputParameterList);
            //        RELEASE_TO_DATE = new datetimeParameter("@RELEASE_TO_DATE", base.inputParameterList);
            //    }

            //    public DataTable Read(DatabaseAccess _dba, 
            //                          int? USER_RID,
            //                          int? HDR_DATE_TYPE,
            //                          DateTime? HDR_FROM_DATE,
            //                          DateTime? HDR_TO_DATE,
            //                          int? RELEASE_DATE_TYPE,
            //                          DateTime? RELEASE_FROM_DATE,
            //                          DateTime? RELEASE_TO_DATE
            //                          )
            //    {
            //        this.USER_RID.SetValue(USER_RID);
            //        this.HDR_DATE_TYPE.SetValue(HDR_DATE_TYPE);
            //        this.HDR_FROM_DATE.SetValue(HDR_FROM_DATE);
            //        this.HDR_TO_DATE.SetValue(HDR_TO_DATE);
            //        this.RELEASE_DATE_TYPE.SetValue(RELEASE_DATE_TYPE);
            //        this.RELEASE_FROM_DATE.SetValue(RELEASE_FROM_DATE);
            //        this.RELEASE_TO_DATE.SetValue(RELEASE_TO_DATE);
            //        return ExecuteStoredProcedureForRead(_dba);
            //    }
            //}


            public static MID_HEADER_READ_FROM_FILTER_def MID_HEADER_READ_FROM_FILTER = new MID_HEADER_READ_FROM_FILTER_def();
            public class MID_HEADER_READ_FROM_FILTER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FROM_FILTER.SQL"

                //The stored procedure called will be based on the filter RID
                private intParameter HN_RID_OVERRIDE;
                private intParameter USE_WORKSPACE_FIELDS;
       

                public MID_HEADER_READ_FROM_FILTER_def()
                {
                    base.procedureName = "WF_XXX";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HN_RID_OVERRIDE = new intParameter("@HN_RID_OVERRIDE", base.inputParameterList);
                    USE_WORKSPACE_FIELDS = new intParameter("@USE_WORKSPACE_FIELDS", base.inputParameterList);                  
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int FILTER_RID,
                                      int? HN_RID_OVERRIDE,
                                      int? USE_WORKSPACE_FIELDS,
                                      filterTypes filterType
                                      )
                {
                    lock (typeof(MID_HEADER_READ_FROM_FILTER_def))
                    {

                        base.procedureName = FilterCommon.BuildFilterProcedureName(FILTER_RID, filterType);



                        this.HN_RID_OVERRIDE.SetValue(HN_RID_OVERRIDE);
                        this.USE_WORKSPACE_FIELDS.SetValue(USE_WORKSPACE_FIELDS);
                        //this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //End TT#1313-MD -jsobek -Header Filters

            public static SP_MID_GET_HEADERS_TO_DELETE_def SP_MID_GET_HEADERS_TO_DELETE = new SP_MID_GET_HEADERS_TO_DELETE_def();
            public class SP_MID_GET_HEADERS_TO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_HEADERS_TO_DELETE.SQL"

                public SP_MID_GET_HEADERS_TO_DELETE_def()
                {
                    base.procedureName = "SP_MID_GET_HEADERS_TO_DELETE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(SP_MID_GET_HEADERS_TO_DELETE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_MULTI_HEADERS_TO_DELETE_def SP_MID_GET_MULTI_HEADERS_TO_DELETE = new SP_MID_GET_MULTI_HEADERS_TO_DELETE_def();
            public class SP_MID_GET_MULTI_HEADERS_TO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_MULTI_HEADERS_TO_DELETE.SQL"

                public SP_MID_GET_MULTI_HEADERS_TO_DELETE_def()
                {
                    base.procedureName = "SP_MID_GET_MULTI_HEADERS_TO_DELETE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_GET_HEADERS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(SP_MID_GET_MULTI_HEADERS_TO_DELETE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_FOR_RESERVATION_STORES_def MID_HEADER_READ_FOR_RESERVATION_STORES = new MID_HEADER_READ_FOR_RESERVATION_STORES_def();
            public class MID_HEADER_READ_FOR_RESERVATION_STORES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_FOR_RESERVATION_STORES.SQL"

                public MID_HEADER_READ_FOR_RESERVATION_STORES_def()
                {
                    base.procedureName = "MID_HEADER_READ_FOR_RESERVATION_STORES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_READ_FOR_RESERVATION_STORES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_def MID_HEADER_PACK_READ = new MID_HEADER_PACK_READ_def();
            public class MID_HEADER_PACK_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_READ_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            // Begin TT#1966-MD - JSmith - DC Fulfillment
            public static MID_HEADER_PACK_ASSOCIATION_READ_def MID_HEADER_PACK_ASSOCIATION_READ = new MID_HEADER_PACK_ASSOCIATION_READ_def();
            public class MID_HEADER_PACK_ASSOCIATION_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_ASSOCIATION_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_ASSOCIATION_READ_def()
                {
                    base.procedureName = "MID_HEADER_PACK_ASSOCIATION_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK_ASSOCIATION");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_ASSOCIATION_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_ASSOCIATION_INSERT_def MID_HEADER_PACK_ASSOCIATION_INSERT = new MID_HEADER_PACK_ASSOCIATION_INSERT_def();
            public class MID_HEADER_PACK_ASSOCIATION_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_ASSOCIATION_INSERT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter SEQ;
                private intParameter HDR_RID;
                private intParameter ASSOCIATED_PACK_RID;
  
                public MID_HEADER_PACK_ASSOCIATION_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_PACK_ASSOCIATION_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    ASSOCIATED_PACK_RID = new intParameter("@ASSOCIATED_PACK_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? HDR_PACK_RID,
                                  int? SEQ,
                                  int? HDR_RID,
                                  int? ASSOCIATED_PACK_RID
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_ASSOCIATION_INSERT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.SEQ.SetValue(SEQ);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ASSOCIATED_PACK_RID.SetValue(ASSOCIATED_PACK_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_ASSOCIATION_DELETE_def MID_HEADER_PACK_ASSOCIATION_DELETE = new MID_HEADER_PACK_ASSOCIATION_DELETE_def();
            public class MID_HEADER_PACK_ASSOCIATION_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_ASSOCIATION_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_ASSOCIATION_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_PACK_ASSOCIATION_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_TOTAL");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_ASSOCIATION_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            // End TT#1966-MD - JSmith - DC Fulfillment

            public static MID_HEADER_PACK_READ_DISTINCT_NAMES_def MID_HEADER_PACK_READ_DISTINCT_NAMES = new MID_HEADER_PACK_READ_DISTINCT_NAMES_def();
            public class MID_HEADER_PACK_READ_DISTINCT_NAMES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_DISTINCT_NAMES.SQL"

                public MID_HEADER_PACK_READ_DISTINCT_NAMES_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_DISTINCT_NAMES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_PACK_READ_DISTINCT_NAMES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_READ_ALL_def MID_HEADER_PACK_COLOR_READ_ALL = new MID_HEADER_PACK_COLOR_READ_ALL_def();
            public class MID_HEADER_PACK_COLOR_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_READ_ALL.SQL"

                public MID_HEADER_PACK_COLOR_READ_ALL_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_READ_def MID_HEADER_PACK_COLOR_READ = new MID_HEADER_PACK_COLOR_READ_def();
            public class MID_HEADER_PACK_COLOR_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_COLOR_READ_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_COUNT_def MID_HEADER_PACK_READ_COUNT = new MID_HEADER_PACK_READ_COUNT_def();
            public class MID_HEADER_PACK_READ_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_COUNT.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_READ_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_READ_COUNT_def MID_HEADER_BULK_COLOR_READ_COUNT = new MID_HEADER_BULK_COLOR_READ_COUNT_def();
            public class MID_HEADER_BULK_COLOR_READ_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_READ_COUNT.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_READ_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_READ_COUNT_def MID_HEADER_BULK_COLOR_SIZE_READ_COUNT = new MID_HEADER_BULK_COLOR_SIZE_READ_COUNT_def();
            public class MID_HEADER_BULK_COLOR_SIZE_READ_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_READ_COUNT.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_SIZE_READ_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_READ_def MID_HEADER_BULK_COLOR_READ = new MID_HEADER_BULK_COLOR_READ_def();
            public class MID_HEADER_BULK_COLOR_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_READ_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_GET_PACK_DATA_FOR_HEADER_def MID_GET_PACK_DATA_FOR_HEADER = new MID_GET_PACK_DATA_FOR_HEADER_def();
            public class MID_GET_PACK_DATA_FOR_HEADER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_PACK_DATA_FOR_HEADER.SQL"

                private intParameter HDR_RID;

                public MID_GET_PACK_DATA_FOR_HEADER_def()
                {
                    base.procedureName = "MID_GET_PACK_DATA_FOR_HEADER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK");
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    base.tableNames.Add("HEADER_PACK_ROUNDING");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_GET_PACK_DATA_FOR_HEADER_def))
                    {
                        DataSet dsValues;
                        this.HDR_RID.SetValue(HDR_RID);
                        dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsValues.Tables[0].TableName = "PackData";
                        dsValues.Tables[1].TableName = "PackColorData";
                        dsValues.Tables[2].TableName = "PackSizeData";
                        dsValues.Tables[3].TableName = "PackRoundingData";
                        return dsValues;
                    }
                }
            }

            public static MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER_def MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER = new MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER_def();
            public class MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER.SQL"

                private intParameter HDR_RID;

                public MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER_def()
                {
                    base.procedureName = "MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_GET_BULK_COLOR_AND_SIZE_DATA_FOR_HEADER_def))
                    {
                        DataSet dsValues;
                        this.HDR_RID.SetValue(HDR_RID);
                        dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsValues.Tables[0].TableName = "BulkColorData";
                        dsValues.Tables[1].TableName = "BulkColorSizeData";
                        return dsValues;
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_SIZE_READ_def MID_HEADER_PACK_COLOR_SIZE_READ = new MID_HEADER_PACK_COLOR_SIZE_READ_def();
            public class MID_HEADER_PACK_COLOR_SIZE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_SIZE_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_COLOR_SIZE_READ_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_SIZE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_SIZE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_SIZE_READ_COUNT_def MID_HEADER_PACK_COLOR_SIZE_READ_COUNT = new MID_HEADER_PACK_COLOR_SIZE_READ_COUNT_def();
            public class MID_HEADER_PACK_COLOR_SIZE_READ_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_SIZE_READ_COUNT.SQL"

                private intParameter HDR_PACK_RID;

                public MID_HEADER_PACK_COLOR_SIZE_READ_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_SIZE_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? HDR_PACK_RID)
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_SIZE_READ_COUNT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_CHAR_DATA_def SP_MID_HEADER_CHAR_DATA = new SP_MID_HEADER_CHAR_DATA_def();
            public class SP_MID_HEADER_CHAR_DATA_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_CHAR_DATA.SQL"

                private intParameter HDR_RID;
                private charParameter GetUnjoinedChars;

                public SP_MID_HEADER_CHAR_DATA_def()
                {
                    base.procedureName = "SP_MID_HEADER_CHAR_DATA";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    GetUnjoinedChars = new charParameter("@GetUnjoinedChars", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HDR_RID,
                                      char? GetUnjoinedChars
                                      )
                {
                    lock (typeof(SP_MID_HEADER_CHAR_DATA_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.GetUnjoinedChars.SetValue(GetUnjoinedChars);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_READ_def MID_HEADER_BULK_COLOR_SIZE_READ = new MID_HEADER_BULK_COLOR_SIZE_READ_def();
            public class MID_HEADER_BULK_COLOR_SIZE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_SIZE_READ_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_EXISTS_def MID_HEADER_BULK_COLOR_SIZE_EXISTS = new MID_HEADER_BULK_COLOR_SIZE_EXISTS_def();
            public class MID_HEADER_BULK_COLOR_SIZE_EXISTS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_EXISTS.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_SIZE_EXISTS_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_EXISTS";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_EXISTS_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static SP_MID_GET_HEADER_ALLOCATION_def SP_MID_GET_HEADER_ALLOCATION = new SP_MID_GET_HEADER_ALLOCATION_def();
            public class SP_MID_GET_HEADER_ALLOCATION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_HEADER_ALLOCATION.SQL"

                private intParameter HDR_RID;

                public SP_MID_GET_HEADER_ALLOCATION_def()
                {
                    base.procedureName = "SP_MID_GET_HEADER_ALLOCATION";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("TOTAL_ALLOCATION");
                    base.tableNames.Add("DETAIL_ALLOCATION");
                    base.tableNames.Add("BULK_ALLOCATION");
                    base.tableNames.Add("BULK_COLOR_ALLOCATION");
                    base.tableNames.Add("BULK_COLOR_SIZE_ALLOCATION");
                    base.tableNames.Add("PACK_ALLOCATION");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(SP_MID_GET_HEADER_ALLOCATION_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }

            public static MID_HEADER_STORE_GRADE_READ_def MID_HEADER_STORE_GRADE_READ = new MID_HEADER_STORE_GRADE_READ_def();
            public class MID_HEADER_STORE_GRADE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_STORE_GRADE_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_STORE_GRADE_READ_def()
                {
                    base.procedureName = "MID_HEADER_STORE_GRADE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_STORE_GRADE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_STORE_GRADE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_SIZE_NEED_READ_def MID_HEADER_SIZE_NEED_READ = new MID_HEADER_SIZE_NEED_READ_def();
            public class MID_HEADER_SIZE_NEED_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_SIZE_NEED_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_SIZE_NEED_READ_def()
                {
                    base.procedureName = "MID_HEADER_SIZE_NEED_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_SIZE_NEED_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_NEED_READ_def MID_HEADER_BULK_COLOR_SIZE_NEED_READ = new MID_HEADER_BULK_COLOR_SIZE_NEED_READ_def();
            public class MID_HEADER_BULK_COLOR_SIZE_NEED_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_NEED_READ.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_SIZE_NEED_READ_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_NEED_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_NEED_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_RULE_TOTAL_DELETE_def MID_RULE_TOTAL_DELETE = new MID_RULE_TOTAL_DELETE_def();
            public class MID_RULE_TOTAL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_TOTAL_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_TOTAL_DELETE_def()
                {
                    base.procedureName = "MID_RULE_TOTAL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_TOTAL");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_TOTAL_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_LAYER_TOTAL_DELETE_def MID_RULE_LAYER_TOTAL_DELETE = new MID_RULE_LAYER_TOTAL_DELETE_def();
            public class MID_RULE_LAYER_TOTAL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_LAYER_TOTAL_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_LAYER_TOTAL_DELETE_def()
                {
                    base.procedureName = "MID_RULE_LAYER_TOTAL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_LAYER_TOTAL");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_LAYER_TOTAL_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_DETAIL_DELETE_def MID_RULE_DETAIL_DELETE = new MID_RULE_DETAIL_DELETE_def();
            public class MID_RULE_DETAIL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_DETAIL_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_DETAIL_DELETE_def()
                {
                    base.procedureName = "MID_RULE_DETAIL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_DETAIL");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_DETAIL_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_LAYER_DETAIL_DELETE_def MID_RULE_LAYER_DETAIL_DELETE = new MID_RULE_LAYER_DETAIL_DELETE_def();
            public class MID_RULE_LAYER_DETAIL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_LAYER_DETAIL_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_LAYER_DETAIL_DELETE_def()
                {
                    base.procedureName = "MID_RULE_LAYER_DETAIL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_LAYER_DETAIL");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_LAYER_DETAIL_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_BULK_DELETE_def MID_RULE_BULK_DELETE = new MID_RULE_BULK_DELETE_def();
            public class MID_RULE_BULK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_BULK_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_BULK_DELETE_def()
                {
                    base.procedureName = "MID_RULE_BULK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_BULK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_BULK_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_LAYER_BULK_DELETE_def MID_RULE_LAYER_BULK_DELETE = new MID_RULE_LAYER_BULK_DELETE_def();
            public class MID_RULE_LAYER_BULK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_LAYER_BULK_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_RULE_LAYER_BULK_DELETE_def()
                {
                    base.procedureName = "MID_RULE_LAYER_BULK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_LAYER_BULK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_RULE_LAYER_BULK_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_PACK_DELETE_def MID_RULE_PACK_DELETE = new MID_RULE_PACK_DELETE_def();
            public class MID_RULE_PACK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_PACK_DELETE.SQL"

                private intParameter HDR_PACK_RID;

                public MID_RULE_PACK_DELETE_def()
                {
                    base.procedureName = "MID_RULE_PACK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_PACK");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_PACK_RID)
                {
                    lock (typeof(MID_RULE_PACK_DELETE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_LAYER_PACK_DELETE_def MID_RULE_LAYER_PACK_DELETE = new MID_RULE_LAYER_PACK_DELETE_def();
            public class MID_RULE_LAYER_PACK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_LAYER_PACK_DELETE.SQL"

                private intParameter HDR_PACK_RID;

                public MID_RULE_LAYER_PACK_DELETE_def()
                {
                    base.procedureName = "MID_RULE_LAYER_PACK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_LAYER_PACK");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_PACK_RID)
                {
                    lock (typeof(MID_RULE_LAYER_PACK_DELETE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_BULK_COLOR_DELETE_def MID_RULE_BULK_COLOR_DELETE = new MID_RULE_BULK_COLOR_DELETE_def();
            public class MID_RULE_BULK_COLOR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_BULK_COLOR_DELETE.SQL"

                private intParameter HDR_BC_RID;

                public MID_RULE_BULK_COLOR_DELETE_def()
                {
                    base.procedureName = "MID_RULE_BULK_COLOR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_BULK_COLOR");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_BC_RID)
                {
                    lock (typeof(MID_RULE_BULK_COLOR_DELETE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RULE_LAYER_BULK_COLOR_DELETE_def MID_RULE_LAYER_BULK_COLOR_DELETE = new MID_RULE_LAYER_BULK_COLOR_DELETE_def();
            public class MID_RULE_LAYER_BULK_COLOR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RULE_LAYER_BULK_COLOR_DELETE.SQL"

                private intParameter HDR_BC_RID;

                public MID_RULE_LAYER_BULK_COLOR_DELETE_def()
                {
                    base.procedureName = "MID_RULE_LAYER_BULK_COLOR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RULE_LAYER_BULK_COLOR");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_BC_RID)
                {
                    lock (typeof(MID_RULE_LAYER_BULK_COLOR_DELETE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_METHOD_RULE_UPDATE_STATUS_def MID_METHOD_RULE_UPDATE_STATUS = new MID_METHOD_RULE_UPDATE_STATUS_def();
            public class MID_METHOD_RULE_UPDATE_STATUS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_UPDATE_STATUS.SQL"

                private intParameter METHOD_STATUS;
                private intParameter HDR_RID;

                public MID_METHOD_RULE_UPDATE_STATUS_def()
                {
                    base.procedureName = "MID_METHOD_RULE_UPDATE_STATUS";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_RULE");
                    METHOD_STATUS = new intParameter("@METHOD_STATUS", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? METHOD_STATUS,
                                  int? HDR_RID
                                  )
                {
                    lock (typeof(MID_METHOD_RULE_UPDATE_STATUS_def))
                    {
                        this.METHOD_STATUS.SetValue(METHOD_STATUS);
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE_def MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE = new MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE_def();
            public class MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE.SQL"

                private intParameter HDR_RID;

                public MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE_def()
                {
                    base.procedureName = "MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("METHOD_RULE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_MULTI_HEADER_DELETE_def SP_MID_MULTI_HEADER_DELETE = new SP_MID_MULTI_HEADER_DELETE_def();
            public class SP_MID_MULTI_HEADER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MULTI_HEADER_DELETE.SQL"

                private intParameter HDR_RID;
                private intParameter debug;
                private intParameter HEADER_DELETE_COUNT; //Declare Output Parameter

                public SP_MID_MULTI_HEADER_DELETE_def()
                {
                    base.procedureName = "SP_MID_MULTI_HEADER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID ", base.inputParameterList);
                    debug = new intParameter("@debug ", base.inputParameterList);

                    HEADER_DELETE_COUNT = new intParameter("@HEADER_DELETE_COUNT ", base.outputParameterList); //Add Output Parameter
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? debug
                                  )
                {
                    lock (typeof(SP_MID_MULTI_HEADER_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.debug.SetValue(debug);
                        this.HEADER_DELETE_COUNT.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_DELETE_def SP_MID_HEADER_DELETE = new SP_MID_HEADER_DELETE_def();
            public class SP_MID_HEADER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_DELETE.SQL"

                private intParameter HDR_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures

                public SP_MID_HEADER_DELETE_def()
                {
                    base.procedureName = "SP_MID_HEADER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(SP_MID_HEADER_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_INSERT_def SP_MID_HEADER_INSERT = new SP_MID_HEADER_INSERT_def();
            public class SP_MID_HEADER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_INSERT.SQL"

                private stringParameter HDR_ID;
                private stringParameter HDR_DESC;
                private datetimeParameter HDR_DAY;
                private datetimeParameter ORIG_DAY;
                private floatParameter UNIT_RETAIL;
                private floatParameter UNIT_COST;
                private intParameter UNITS_RECEIVED;
                private intParameter STYLE_HNRID;
                private intParameter PLAN_HNRID;
                private intParameter ON_HAND_HNRID;
                private intParameter BULK_MULTIPLE;
                private intParameter ALLOCATION_MULTIPLE;
                private stringParameter VENDOR;
                private stringParameter PURCHASE_ORDER;
                private datetimeParameter BEGIN_DAY;
                private datetimeParameter NEED_DAY;
                private datetimeParameter SHIP_TO_DAY;
                private datetimeParameter RELEASE_DATETIME;
                private datetimeParameter RELEASE_APPROVED_DATETIME;
                private intParameter HDR_GROUP_RID;
                private intParameter SIZE_GROUP_RID;
                private intParameter WORKFLOW_RID;
                private intParameter METHOD_RID;
                private intParameter ALLOCATION_STATUS_FLAGS;
                private intParameter BALANCE_STATUS_FLAGS;
                private intParameter SHIPPING_STATUS_FLAGS;
                private intParameter ALLOCATION_TYPE_FLAGS;
                private intParameter INTRANSIT_STATUS_FLAGS;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter PLAN_PERCENT_FACTOR;
                private intParameter RESERVE_UNITS;
                private intParameter GRADE_WEEK_COUNT;
                private stringParameter DIST_CENTER;
                private stringParameter HEADER_NOTES;
                private charParameter WORKFLOW_TRIGGER;
                private datetimeParameter EARLIEST_SHIP_DAY;
                private intParameter API_WORKFLOW_RID;
                private charParameter API_WORKFLOW_TRIGGER;
                private intParameter ALLOCATED_UNITS;
                private intParameter ORIG_ALLOCATED_UNITS;
                private intParameter RELEASE_COUNT;
                private intParameter RSV_ALLOCATED_UNITS;
                private intParameter DISPLAY_STATUS;
                private intParameter DISPLAY_TYPE;
                private intParameter DISPLAY_INTRANSIT;
                private intParameter DISPLAY_SHIP_STATUS;
                private intParameter ASRT_RID;
                private intParameter PLACEHOLDER_RID;
                private intParameter ASRT_TYPE;
                private intParameter ALLOCATION_MULTIPLE_DEFAULT;
                private intParameter GRADE_SG_RID;
                private intParameter ASRT_PLACEHOLDER_SEQ;
                private intParameter ASRT_HEADER_SEQ;
                private stringParameter IMO_ID;
                private intParameter ITEM_UNITS_ALLOCATED;
                private intParameter ITEM_ORIG_UNITS_ALLOCATED;
                private intParameter UNITS_PER_CARTON;
                private intParameter HDR_RID; //Declare Output Parameter

                public SP_MID_HEADER_INSERT_def()
                {
                    base.procedureName = "SP_MID_HEADER_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HEADER");
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                    HDR_DESC = new stringParameter("@HDR_DESC", base.inputParameterList);
                    HDR_DAY = new datetimeParameter("@HDR_DAY", base.inputParameterList);
                    ORIG_DAY = new datetimeParameter("@ORIG_DAY", base.inputParameterList);
                    UNIT_RETAIL = new floatParameter("@UNIT_RETAIL", base.inputParameterList);
                    UNIT_COST = new floatParameter("@UNIT_COST", base.inputParameterList);
                    UNITS_RECEIVED = new intParameter("@UNITS_RECEIVED", base.inputParameterList);
                    STYLE_HNRID = new intParameter("@STYLE_HNRID", base.inputParameterList);
                    PLAN_HNRID = new intParameter("@PLAN_HNRID", base.inputParameterList);
                    ON_HAND_HNRID = new intParameter("@ON_HAND_HNRID", base.inputParameterList);
                    BULK_MULTIPLE = new intParameter("@BULK_MULTIPLE", base.inputParameterList);
                    ALLOCATION_MULTIPLE = new intParameter("@ALLOCATION_MULTIPLE", base.inputParameterList);
                    VENDOR = new stringParameter("@VENDOR", base.inputParameterList);
                    PURCHASE_ORDER = new stringParameter("@PURCHASE_ORDER", base.inputParameterList);
                    BEGIN_DAY = new datetimeParameter("@BEGIN_DAY", base.inputParameterList);
                    NEED_DAY = new datetimeParameter("@NEED_DAY", base.inputParameterList);
                    SHIP_TO_DAY = new datetimeParameter("@SHIP_TO_DAY", base.inputParameterList);
                    RELEASE_DATETIME = new datetimeParameter("@RELEASE_DATETIME", base.inputParameterList);
                    RELEASE_APPROVED_DATETIME = new datetimeParameter("@RELEASE_APPROVED_DATETIME", base.inputParameterList);
                    HDR_GROUP_RID = new intParameter("@HDR_GROUP_RID", base.inputParameterList);
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                    WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    ALLOCATION_STATUS_FLAGS = new intParameter("@ALLOCATION_STATUS_FLAGS", base.inputParameterList);
                    BALANCE_STATUS_FLAGS = new intParameter("@BALANCE_STATUS_FLAGS", base.inputParameterList);
                    SHIPPING_STATUS_FLAGS = new intParameter("@SHIPPING_STATUS_FLAGS", base.inputParameterList);
                    ALLOCATION_TYPE_FLAGS = new intParameter("@ALLOCATION_TYPE_FLAGS", base.inputParameterList);
                    INTRANSIT_STATUS_FLAGS = new intParameter("@INTRANSIT_STATUS_FLAGS", base.inputParameterList);
                    PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
                    PLAN_PERCENT_FACTOR = new floatParameter("@PLAN_PERCENT_FACTOR", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    GRADE_WEEK_COUNT = new intParameter("@GRADE_WEEK_COUNT", base.inputParameterList);
                    DIST_CENTER = new stringParameter("@DIST_CENTER", base.inputParameterList);
                    HEADER_NOTES = new stringParameter("@HEADER_NOTES", base.inputParameterList);
                    WORKFLOW_TRIGGER = new charParameter("@WORKFLOW_TRIGGER", base.inputParameterList);
                    EARLIEST_SHIP_DAY = new datetimeParameter("@EARLIEST_SHIP_DAY", base.inputParameterList);
                    API_WORKFLOW_RID = new intParameter("@API_WORKFLOW_RID", base.inputParameterList);
                    API_WORKFLOW_TRIGGER = new charParameter("@API_WORKFLOW_TRIGGER", base.inputParameterList);
                    ALLOCATED_UNITS = new intParameter("@ALLOCATED_UNITS", base.inputParameterList);
                    ORIG_ALLOCATED_UNITS = new intParameter("@ORIG_ALLOCATED_UNITS", base.inputParameterList);
                    RELEASE_COUNT = new intParameter("@RELEASE_COUNT", base.inputParameterList);
                    RSV_ALLOCATED_UNITS = new intParameter("@RSV_ALLOCATED_UNITS", base.inputParameterList);
                    DISPLAY_STATUS = new intParameter("@DISPLAY_STATUS", base.inputParameterList);
                    DISPLAY_TYPE = new intParameter("@DISPLAY_TYPE", base.inputParameterList);
                    DISPLAY_INTRANSIT = new intParameter("@DISPLAY_INTRANSIT", base.inputParameterList);
                    DISPLAY_SHIP_STATUS = new intParameter("@DISPLAY_SHIP_STATUS", base.inputParameterList);
                    ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
                    PLACEHOLDER_RID = new intParameter("@PLACEHOLDER_RID", base.inputParameterList);
                    ASRT_TYPE = new intParameter("@ASRT_TYPE", base.inputParameterList);
                    ALLOCATION_MULTIPLE_DEFAULT = new intParameter("@ALLOCATION_MULTIPLE_DEFAULT", base.inputParameterList);
                    GRADE_SG_RID = new intParameter("@GRADE_SG_RID", base.inputParameterList);
                    ASRT_PLACEHOLDER_SEQ = new intParameter("@ASRT_PLACEHOLDER_SEQ", base.inputParameterList);
                    ASRT_HEADER_SEQ = new intParameter("@ASRT_HEADER_SEQ", base.inputParameterList);
                    IMO_ID = new stringParameter("@IMO_ID", base.inputParameterList);
                    ITEM_UNITS_ALLOCATED = new intParameter("@ITEM_UNITS_ALLOCATED", base.inputParameterList);
                    ITEM_ORIG_UNITS_ALLOCATED = new intParameter("@ITEM_ORIG_UNITS_ALLOCATED", base.inputParameterList);
                    UNITS_PER_CARTON = new intParameter("@UNITS_PER_CARTON", base.inputParameterList);      // TT#1652-MD - stodd - DC Carton Rounding

                    HDR_RID = new intParameter("@HDR_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                  string HDR_ID,
                                  string HDR_DESC,
                                  DateTime? HDR_DAY,
                                  DateTime? ORIG_DAY,
                                  double? UNIT_RETAIL,
                                  double? UNIT_COST,
                                  int? UNITS_RECEIVED,
                                  int? STYLE_HNRID,
                                  int? PLAN_HNRID,
                                  int? ON_HAND_HNRID,
                                  int? BULK_MULTIPLE,
                                  int? ALLOCATION_MULTIPLE,
                                  string VENDOR,
                                  string PURCHASE_ORDER,
                                  DateTime? BEGIN_DAY,
                                  DateTime? NEED_DAY,
                                  DateTime? SHIP_TO_DAY,
                                  DateTime? RELEASE_DATETIME,
                                  DateTime? RELEASE_APPROVED_DATETIME,
                                  int? HDR_GROUP_RID,
                                  int? SIZE_GROUP_RID,
                                  int? WORKFLOW_RID,
                                  int? METHOD_RID,
                                  int? ALLOCATION_STATUS_FLAGS,
                                  int? BALANCE_STATUS_FLAGS,
                                  int? SHIPPING_STATUS_FLAGS,
                                  int? ALLOCATION_TYPE_FLAGS,
                                  int? INTRANSIT_STATUS_FLAGS,
                                  double? PERCENT_NEED_LIMIT,
                                  double? PLAN_PERCENT_FACTOR,
                                  int? RESERVE_UNITS,
                                  int? GRADE_WEEK_COUNT,
                                  string DIST_CENTER,
                                  string HEADER_NOTES,
                                  char? WORKFLOW_TRIGGER,
                                  DateTime? EARLIEST_SHIP_DAY,
                                  int? API_WORKFLOW_RID,
                                  char? API_WORKFLOW_TRIGGER,
                                  int? ALLOCATED_UNITS,
                                  int? ORIG_ALLOCATED_UNITS,
                                  int? RELEASE_COUNT,
                                  int? RSV_ALLOCATED_UNITS,
                                  int? DISPLAY_STATUS,
                                  int? DISPLAY_TYPE,
                                  int? DISPLAY_INTRANSIT,
                                  int? DISPLAY_SHIP_STATUS,
                                  int? ASRT_RID,
                                  int? PLACEHOLDER_RID,
                                  int? ASRT_TYPE,
                                  int? ALLOCATION_MULTIPLE_DEFAULT,
                                  int? GRADE_SG_RID,
                                  int? ASRT_PLACEHOLDER_SEQ,
                                  int? ASRT_HEADER_SEQ,
                                  string IMO_ID,
                                  int? ITEM_UNITS_ALLOCATED,
                                  int? ITEM_ORIG_UNITS_ALLOCATED,
                                  int? UNITS_PER_CARTON     // TT#1652-MD - stodd - DC Carton Rounding
                                  )
                {
                    lock (typeof(SP_MID_HEADER_INSERT_def))
                    {
                        this.HDR_ID.SetValue(HDR_ID);
                        this.HDR_DESC.SetValue(HDR_DESC);
                        this.HDR_DAY.SetValue(HDR_DAY);
                        this.ORIG_DAY.SetValue(ORIG_DAY);
                        this.UNIT_RETAIL.SetValue(UNIT_RETAIL);
                        this.UNIT_COST.SetValue(UNIT_COST);
                        this.UNITS_RECEIVED.SetValue(UNITS_RECEIVED);
                        this.STYLE_HNRID.SetValue(STYLE_HNRID);
                        this.PLAN_HNRID.SetValue(PLAN_HNRID);
                        this.ON_HAND_HNRID.SetValue(ON_HAND_HNRID);
                        this.BULK_MULTIPLE.SetValue(BULK_MULTIPLE);
                        this.ALLOCATION_MULTIPLE.SetValue(ALLOCATION_MULTIPLE);
                        this.VENDOR.SetValue(VENDOR);
                        this.PURCHASE_ORDER.SetValue(PURCHASE_ORDER);
                        this.BEGIN_DAY.SetValue(BEGIN_DAY);
                        this.NEED_DAY.SetValue(NEED_DAY);
                        this.SHIP_TO_DAY.SetValue(SHIP_TO_DAY);
                        this.RELEASE_DATETIME.SetValue(RELEASE_DATETIME);
                        this.RELEASE_APPROVED_DATETIME.SetValue(RELEASE_APPROVED_DATETIME);
                        this.HDR_GROUP_RID.SetValue(HDR_GROUP_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.ALLOCATION_STATUS_FLAGS.SetValue(ALLOCATION_STATUS_FLAGS);
                        this.BALANCE_STATUS_FLAGS.SetValue(BALANCE_STATUS_FLAGS);
                        this.SHIPPING_STATUS_FLAGS.SetValue(SHIPPING_STATUS_FLAGS);
                        this.ALLOCATION_TYPE_FLAGS.SetValue(ALLOCATION_TYPE_FLAGS);
                        this.INTRANSIT_STATUS_FLAGS.SetValue(INTRANSIT_STATUS_FLAGS);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.PLAN_PERCENT_FACTOR.SetValue(PLAN_PERCENT_FACTOR);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.GRADE_WEEK_COUNT.SetValue(GRADE_WEEK_COUNT);
                        this.DIST_CENTER.SetValue(DIST_CENTER);
                        this.HEADER_NOTES.SetValue(HEADER_NOTES);
                        this.WORKFLOW_TRIGGER.SetValue(WORKFLOW_TRIGGER);
                        this.EARLIEST_SHIP_DAY.SetValue(EARLIEST_SHIP_DAY);
                        this.API_WORKFLOW_RID.SetValue(API_WORKFLOW_RID);
                        this.API_WORKFLOW_TRIGGER.SetValue(API_WORKFLOW_TRIGGER);
                        this.ALLOCATED_UNITS.SetValue(ALLOCATED_UNITS);
                        this.ORIG_ALLOCATED_UNITS.SetValue(ORIG_ALLOCATED_UNITS);
                        this.RELEASE_COUNT.SetValue(RELEASE_COUNT);
                        this.RSV_ALLOCATED_UNITS.SetValue(RSV_ALLOCATED_UNITS);
                        this.DISPLAY_STATUS.SetValue(DISPLAY_STATUS);
                        this.DISPLAY_TYPE.SetValue(DISPLAY_TYPE);
                        this.DISPLAY_INTRANSIT.SetValue(DISPLAY_INTRANSIT);
                        this.DISPLAY_SHIP_STATUS.SetValue(DISPLAY_SHIP_STATUS);
                        this.ASRT_RID.SetValue(ASRT_RID);
                        this.PLACEHOLDER_RID.SetValue(PLACEHOLDER_RID);
                        this.ASRT_TYPE.SetValue(ASRT_TYPE);
                        this.ALLOCATION_MULTIPLE_DEFAULT.SetValue(ALLOCATION_MULTIPLE_DEFAULT);
                        this.GRADE_SG_RID.SetValue(GRADE_SG_RID);
                        this.ASRT_PLACEHOLDER_SEQ.SetValue(ASRT_PLACEHOLDER_SEQ);
                        this.ASRT_HEADER_SEQ.SetValue(ASRT_HEADER_SEQ);
                        this.IMO_ID.SetValue(IMO_ID);
                        this.ITEM_UNITS_ALLOCATED.SetValue(ITEM_UNITS_ALLOCATED);
                        this.ITEM_ORIG_UNITS_ALLOCATED.SetValue(ITEM_ORIG_UNITS_ALLOCATED);
                        this.HDR_RID.SetValue(null); //Initialize Output Parameter
                        this.UNITS_PER_CARTON.SetValue(UNITS_PER_CARTON);   // TT#1652-MD - stodd - DC Carton Rounding

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HEADER_UPDATE_def MID_HEADER_UPDATE = new MID_HEADER_UPDATE_def();
            public class MID_HEADER_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_UPDATE.SQL"

                private intParameter HDR_RID;
                private stringParameter HDR_ID;
                private stringParameter HDR_DESC;
                private datetimeParameter HDR_DAY;
                private datetimeParameter ORIG_DAY;
                private floatParameter UNIT_RETAIL;
                private floatParameter UNIT_COST;
                private intParameter UNITS_RECEIVED;
                private intParameter STYLE_HNRID;
                private intParameter PLAN_HNRID;
                private intParameter ON_HAND_HNRID;
                private intParameter BULK_MULTIPLE;
                private intParameter ALLOCATION_MULTIPLE;
                private stringParameter VENDOR;
                private stringParameter PURCHASE_ORDER;
                private datetimeParameter BEGIN_DAY;
                private datetimeParameter NEED_DAY;
                private datetimeParameter SHIP_TO_DAY;
                private datetimeParameter RELEASE_DATETIME;
                private datetimeParameter RELEASE_APPROVED_DATETIME;
                private intParameter HDR_GROUP_RID;
                private intParameter SIZE_GROUP_RID;
                private intParameter WORKFLOW_RID;
                private intParameter METHOD_RID;
                private intParameter ALLOCATION_STATUS_FLAGS;
                private intParameter BALANCE_STATUS_FLAGS;
                private intParameter SHIPPING_STATUS_FLAGS;
                private intParameter ALLOCATION_TYPE_FLAGS;
                private intParameter INTRANSIT_STATUS_FLAGS;
                private floatParameter PERCENT_NEED_LIMIT;
                private floatParameter PLAN_PERCENT_FACTOR;
                private intParameter RESERVE_UNITS;
                private intParameter GRADE_WEEK_COUNT;
                private stringParameter DIST_CENTER;
                private stringParameter HEADER_NOTES;
                private charParameter WORKFLOW_TRIGGER;
                private datetimeParameter EARLIEST_SHIP_DAY;
                private intParameter API_WORKFLOW_RID;
                private charParameter API_WORKFLOW_TRIGGER;
                private intParameter ALLOCATED_UNITS;
                private intParameter ORIG_ALLOCATED_UNITS;
                private intParameter RELEASE_COUNT;
                private intParameter RSV_ALLOCATED_UNITS;
                private intParameter StrStylAloctnManualChgCnt;
                private intParameter StrSizeAloctnManualChgCnt;
                private intParameter storeStyleAllocationChangedTotal;
                private intParameter storeSizeAllocationChangedTotal;
                private intParameter storesWithAllocationCount;
                private charParameter horizonOverride;
                private intParameter DISPLAY_STATUS;
                private intParameter DISPLAY_TYPE;
                private intParameter DISPLAY_INTRANSIT;
                private intParameter DISPLAY_SHIP_STATUS;
                private intParameter ASRT_RID;
                private intParameter PLACEHOLDER_RID;
                private intParameter ASRT_TYPE;
                private intParameter ALLOCATION_MULTIPLE_DEFAULT;
                private intParameter GRADE_SG_RID;
                private intParameter ASRT_PLACEHOLDER_SEQ;
                private intParameter ASRT_HEADER_SEQ;
                private charParameter GRADE_INVENTORY_IND;
                private intParameter GRADE_INVENTORY_HNRID;
                private stringParameter IMO_ID;
                private intParameter ITEM_UNITS_ALLOCATED;
                private intParameter ITEM_ORIG_UNITS_ALLOCATED;
                private intParameter UNITS_PER_CARTON;      // TT#1652-MD - stodd - DC Carton Rounding
                private charParameter DC_FULFILLMENT_PROCESSED_IND;  /* TT#1966-MD - JSmith- DC Fulfillment  */

                public MID_HEADER_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                    HDR_DESC = new stringParameter("@HDR_DESC", base.inputParameterList);
                    HDR_DAY = new datetimeParameter("@HDR_DAY", base.inputParameterList);
                    ORIG_DAY = new datetimeParameter("@ORIG_DAY", base.inputParameterList);
                    UNIT_RETAIL = new floatParameter("@UNIT_RETAIL", base.inputParameterList);
                    UNIT_COST = new floatParameter("@UNIT_COST", base.inputParameterList);
                    UNITS_RECEIVED = new intParameter("@UNITS_RECEIVED", base.inputParameterList);
                    STYLE_HNRID = new intParameter("@STYLE_HNRID", base.inputParameterList);
                    PLAN_HNRID = new intParameter("@PLAN_HNRID", base.inputParameterList);
                    ON_HAND_HNRID = new intParameter("@ON_HAND_HNRID", base.inputParameterList);
                    BULK_MULTIPLE = new intParameter("@BULK_MULTIPLE", base.inputParameterList);
                    ALLOCATION_MULTIPLE = new intParameter("@ALLOCATION_MULTIPLE", base.inputParameterList);
                    VENDOR = new stringParameter("@VENDOR", base.inputParameterList);
                    PURCHASE_ORDER = new stringParameter("@PURCHASE_ORDER", base.inputParameterList);
                    BEGIN_DAY = new datetimeParameter("@BEGIN_DAY", base.inputParameterList);
                    NEED_DAY = new datetimeParameter("@NEED_DAY", base.inputParameterList);
                    SHIP_TO_DAY = new datetimeParameter("@SHIP_TO_DAY", base.inputParameterList);
                    RELEASE_DATETIME = new datetimeParameter("@RELEASE_DATETIME", base.inputParameterList);
                    RELEASE_APPROVED_DATETIME = new datetimeParameter("@RELEASE_APPROVED_DATETIME", base.inputParameterList);
                    HDR_GROUP_RID = new intParameter("@HDR_GROUP_RID", base.inputParameterList);
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                    WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
                    METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
                    ALLOCATION_STATUS_FLAGS = new intParameter("@ALLOCATION_STATUS_FLAGS", base.inputParameterList);
                    BALANCE_STATUS_FLAGS = new intParameter("@BALANCE_STATUS_FLAGS", base.inputParameterList);
                    SHIPPING_STATUS_FLAGS = new intParameter("@SHIPPING_STATUS_FLAGS", base.inputParameterList);
                    ALLOCATION_TYPE_FLAGS = new intParameter("@ALLOCATION_TYPE_FLAGS", base.inputParameterList);
                    INTRANSIT_STATUS_FLAGS = new intParameter("@INTRANSIT_STATUS_FLAGS", base.inputParameterList);
                    PERCENT_NEED_LIMIT = new floatParameter("@PERCENT_NEED_LIMIT", base.inputParameterList);
                    PLAN_PERCENT_FACTOR = new floatParameter("@PLAN_PERCENT_FACTOR", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    GRADE_WEEK_COUNT = new intParameter("@GRADE_WEEK_COUNT", base.inputParameterList);
                    DIST_CENTER = new stringParameter("@DIST_CENTER", base.inputParameterList);
                    HEADER_NOTES = new stringParameter("@HEADER_NOTES", base.inputParameterList);
                    WORKFLOW_TRIGGER = new charParameter("@WORKFLOW_TRIGGER", base.inputParameterList);
                    EARLIEST_SHIP_DAY = new datetimeParameter("@EARLIEST_SHIP_DAY", base.inputParameterList);
                    API_WORKFLOW_RID = new intParameter("@API_WORKFLOW_RID", base.inputParameterList);
                    API_WORKFLOW_TRIGGER = new charParameter("@API_WORKFLOW_TRIGGER", base.inputParameterList);
                    ALLOCATED_UNITS = new intParameter("@ALLOCATED_UNITS", base.inputParameterList);
                    ORIG_ALLOCATED_UNITS = new intParameter("@ORIG_ALLOCATED_UNITS", base.inputParameterList);
                    RELEASE_COUNT = new intParameter("@RELEASE_COUNT", base.inputParameterList);
                    RSV_ALLOCATED_UNITS = new intParameter("@RSV_ALLOCATED_UNITS", base.inputParameterList);
                    StrStylAloctnManualChgCnt = new intParameter("@StrStylAloctnManualChgCnt", base.inputParameterList);
                    StrSizeAloctnManualChgCnt = new intParameter("@StrSizeAloctnManualChgCnt", base.inputParameterList);
                    storeStyleAllocationChangedTotal = new intParameter("@storeStyleAllocationChangedTotal", base.inputParameterList);
                    storeSizeAllocationChangedTotal = new intParameter("@storeSizeAllocationChangedTotal", base.inputParameterList);
                    storesWithAllocationCount = new intParameter("@storesWithAllocationCount", base.inputParameterList);
                    horizonOverride = new charParameter("@horizonOverride", base.inputParameterList);
                    DISPLAY_STATUS = new intParameter("@DISPLAY_STATUS", base.inputParameterList);
                    DISPLAY_TYPE = new intParameter("@DISPLAY_TYPE", base.inputParameterList);
                    DISPLAY_INTRANSIT = new intParameter("@DISPLAY_INTRANSIT", base.inputParameterList);
                    DISPLAY_SHIP_STATUS = new intParameter("@DISPLAY_SHIP_STATUS", base.inputParameterList);
                    ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
                    PLACEHOLDER_RID = new intParameter("@PLACEHOLDER_RID", base.inputParameterList);
                    ASRT_TYPE = new intParameter("@ASRT_TYPE", base.inputParameterList);
                    ALLOCATION_MULTIPLE_DEFAULT = new intParameter("@ALLOCATION_MULTIPLE_DEFAULT", base.inputParameterList);
                    GRADE_SG_RID = new intParameter("@GRADE_SG_RID", base.inputParameterList);
                    ASRT_PLACEHOLDER_SEQ = new intParameter("@ASRT_PLACEHOLDER_SEQ", base.inputParameterList);
                    ASRT_HEADER_SEQ = new intParameter("@ASRT_HEADER_SEQ", base.inputParameterList);
                    GRADE_INVENTORY_IND = new charParameter("@GRADE_INVENTORY_IND", base.inputParameterList);
                    GRADE_INVENTORY_HNRID = new intParameter("@GRADE_INVENTORY_HNRID", base.inputParameterList);
                    IMO_ID = new stringParameter("@IMO_ID", base.inputParameterList);
                    ITEM_UNITS_ALLOCATED = new intParameter("@ITEM_UNITS_ALLOCATED", base.inputParameterList);
                    ITEM_ORIG_UNITS_ALLOCATED = new intParameter("@ITEM_ORIG_UNITS_ALLOCATED", base.inputParameterList);
                    UNITS_PER_CARTON = new intParameter("@UNITS_PER_CARTON", base.inputParameterList);  // TT#1652-MD - stodd - DC Carton Rounding
                    DC_FULFILLMENT_PROCESSED_IND = new charParameter("@DC_FULFILLMENT_PROCESSED_IND", base.inputParameterList);   /* TT#1966-MD - JSmith- DC Fulfillment  */
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  string HDR_ID,
                                  string HDR_DESC,
                                  DateTime? HDR_DAY,
                                  DateTime? ORIG_DAY,
                                  double? UNIT_RETAIL,
                                  double? UNIT_COST,
                                  int? UNITS_RECEIVED,
                                  int? STYLE_HNRID,
                                  int? PLAN_HNRID,
                                  int? ON_HAND_HNRID,
                                  int? BULK_MULTIPLE,
                                  int? ALLOCATION_MULTIPLE,
                                  string VENDOR,
                                  string PURCHASE_ORDER,
                                  DateTime? BEGIN_DAY,
                                  DateTime? NEED_DAY,
                                  DateTime? SHIP_TO_DAY,
                                  DateTime? RELEASE_DATETIME,
                                  DateTime? RELEASE_APPROVED_DATETIME,
                                  int? HDR_GROUP_RID,
                                  int? SIZE_GROUP_RID,
                                  int? WORKFLOW_RID,
                                  int? METHOD_RID,
                                  int? ALLOCATION_STATUS_FLAGS,
                                  int? BALANCE_STATUS_FLAGS,
                                  int? SHIPPING_STATUS_FLAGS,
                                  int? ALLOCATION_TYPE_FLAGS,
                                  int? INTRANSIT_STATUS_FLAGS,
                                  double? PERCENT_NEED_LIMIT,
                                  double? PLAN_PERCENT_FACTOR,
                                  int? RESERVE_UNITS,
                                  int? GRADE_WEEK_COUNT,
                                  string DIST_CENTER,
                                  string HEADER_NOTES,
                                  char? WORKFLOW_TRIGGER,
                                  DateTime? EARLIEST_SHIP_DAY,
                                  int? API_WORKFLOW_RID,
                                  char? API_WORKFLOW_TRIGGER,
                                  int? ALLOCATED_UNITS,
                                  int? ORIG_ALLOCATED_UNITS,
                                  int? RELEASE_COUNT,
                                  int? RSV_ALLOCATED_UNITS,
                                  int? StrStylAloctnManualChgCnt,
                                  int? StrSizeAloctnManualChgCnt,
                                  int? storeStyleAllocationChangedTotal,
                                  int? storeSizeAllocationChangedTotal,
                                  int? storesWithAllocationCount,
                                  char? horizonOverride,
                                  int? DISPLAY_STATUS,
                                  int? DISPLAY_TYPE,
                                  int? DISPLAY_INTRANSIT,
                                  int? DISPLAY_SHIP_STATUS,
                                  int? ASRT_RID,
                                  int? PLACEHOLDER_RID,
                                  int? ASRT_TYPE,
                                  int? ALLOCATION_MULTIPLE_DEFAULT,
                                  int? GRADE_SG_RID,
                                  int? ASRT_PLACEHOLDER_SEQ,
                                  int? ASRT_HEADER_SEQ,
                                  char? GRADE_INVENTORY_IND,
                                  int? GRADE_INVENTORY_HNRID,
                                  string IMO_ID,
                                  int? ITEM_UNITS_ALLOCATED,
                                  int? ITEM_ORIG_UNITS_ALLOCATED,
                                  int? UNITS_PER_CARTON, // TT#1652-MD - stodd - DC Carton Rounding
                                  char? DC_FULFILLMENT_PROCESSED_IND   /* TT#1966-MD - JSmith- DC Fulfillment  */
                                  )
                {
                    lock (typeof(MID_HEADER_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_ID.SetValue(HDR_ID);
                        this.HDR_DESC.SetValue(HDR_DESC);
                        this.HDR_DAY.SetValue(HDR_DAY);
                        this.ORIG_DAY.SetValue(ORIG_DAY);
                        this.UNIT_RETAIL.SetValue(UNIT_RETAIL);
                        this.UNIT_COST.SetValue(UNIT_COST);
                        this.UNITS_RECEIVED.SetValue(UNITS_RECEIVED);
                        this.STYLE_HNRID.SetValue(STYLE_HNRID);
                        this.PLAN_HNRID.SetValue(PLAN_HNRID);
                        this.ON_HAND_HNRID.SetValue(ON_HAND_HNRID);
                        this.BULK_MULTIPLE.SetValue(BULK_MULTIPLE);
                        this.ALLOCATION_MULTIPLE.SetValue(ALLOCATION_MULTIPLE);
                        this.VENDOR.SetValue(VENDOR);
                        this.PURCHASE_ORDER.SetValue(PURCHASE_ORDER);
                        this.BEGIN_DAY.SetValue(BEGIN_DAY);
                        this.NEED_DAY.SetValue(NEED_DAY);
                        this.SHIP_TO_DAY.SetValue(SHIP_TO_DAY);
                        this.RELEASE_DATETIME.SetValue(RELEASE_DATETIME);
                        this.RELEASE_APPROVED_DATETIME.SetValue(RELEASE_APPROVED_DATETIME);
                        this.HDR_GROUP_RID.SetValue(HDR_GROUP_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.ALLOCATION_STATUS_FLAGS.SetValue(ALLOCATION_STATUS_FLAGS);
                        this.BALANCE_STATUS_FLAGS.SetValue(BALANCE_STATUS_FLAGS);
                        this.SHIPPING_STATUS_FLAGS.SetValue(SHIPPING_STATUS_FLAGS);
                        this.ALLOCATION_TYPE_FLAGS.SetValue(ALLOCATION_TYPE_FLAGS);
                        this.INTRANSIT_STATUS_FLAGS.SetValue(INTRANSIT_STATUS_FLAGS);
                        this.PERCENT_NEED_LIMIT.SetValue(PERCENT_NEED_LIMIT);
                        this.PLAN_PERCENT_FACTOR.SetValue(PLAN_PERCENT_FACTOR);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.GRADE_WEEK_COUNT.SetValue(GRADE_WEEK_COUNT);
                        this.DIST_CENTER.SetValue(DIST_CENTER);
                        this.HEADER_NOTES.SetValue(HEADER_NOTES);
                        this.WORKFLOW_TRIGGER.SetValue(WORKFLOW_TRIGGER);
                        this.EARLIEST_SHIP_DAY.SetValue(EARLIEST_SHIP_DAY);
                        this.API_WORKFLOW_RID.SetValue(API_WORKFLOW_RID);
                        this.API_WORKFLOW_TRIGGER.SetValue(API_WORKFLOW_TRIGGER);
                        this.ALLOCATED_UNITS.SetValue(ALLOCATED_UNITS);
                        this.ORIG_ALLOCATED_UNITS.SetValue(ORIG_ALLOCATED_UNITS);
                        this.RELEASE_COUNT.SetValue(RELEASE_COUNT);
                        this.RSV_ALLOCATED_UNITS.SetValue(RSV_ALLOCATED_UNITS);
                        this.StrStylAloctnManualChgCnt.SetValue(StrStylAloctnManualChgCnt);
                        this.StrSizeAloctnManualChgCnt.SetValue(StrSizeAloctnManualChgCnt);
                        this.storeStyleAllocationChangedTotal.SetValue(storeStyleAllocationChangedTotal);
                        this.storeSizeAllocationChangedTotal.SetValue(storeSizeAllocationChangedTotal);
                        this.storesWithAllocationCount.SetValue(storesWithAllocationCount);
                        this.horizonOverride.SetValue(horizonOverride);
                        this.DISPLAY_STATUS.SetValue(DISPLAY_STATUS);
                        this.DISPLAY_TYPE.SetValue(DISPLAY_TYPE);
                        this.DISPLAY_INTRANSIT.SetValue(DISPLAY_INTRANSIT);
                        this.DISPLAY_SHIP_STATUS.SetValue(DISPLAY_SHIP_STATUS);
                        this.ASRT_RID.SetValue(ASRT_RID);
                        this.PLACEHOLDER_RID.SetValue(PLACEHOLDER_RID);
                        this.ASRT_TYPE.SetValue(ASRT_TYPE);
                        this.ALLOCATION_MULTIPLE_DEFAULT.SetValue(ALLOCATION_MULTIPLE_DEFAULT);
                        this.GRADE_SG_RID.SetValue(GRADE_SG_RID);
                        this.ASRT_PLACEHOLDER_SEQ.SetValue(ASRT_PLACEHOLDER_SEQ);
                        this.ASRT_HEADER_SEQ.SetValue(ASRT_HEADER_SEQ);
                        this.GRADE_INVENTORY_IND.SetValue(GRADE_INVENTORY_IND);
                        this.GRADE_INVENTORY_HNRID.SetValue(GRADE_INVENTORY_HNRID);
                        this.IMO_ID.SetValue(IMO_ID);
                        this.ITEM_UNITS_ALLOCATED.SetValue(ITEM_UNITS_ALLOCATED);
                        this.ITEM_ORIG_UNITS_ALLOCATED.SetValue(ITEM_ORIG_UNITS_ALLOCATED);
                        this.UNITS_PER_CARTON.SetValue(UNITS_PER_CARTON);   // Begin TT#1652-MD - stodd - DC Carton Rounding
                        this.DC_FULFILLMENT_PROCESSED_IND.SetValue(DC_FULFILLMENT_PROCESSED_IND);    /* TT#1966-MD - JSmith- DC Fulfillment  */
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_STORE_GRADE_INSERT_def MID_HEADER_STORE_GRADE_INSERT = new MID_HEADER_STORE_GRADE_INSERT_def();
            public class MID_HEADER_STORE_GRADE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_STORE_GRADE_INSERT.SQL"

                private intParameter HDR_RID;
                private floatParameter BOUNDARY;
                private stringParameter GRADE_CODE;

                public MID_HEADER_STORE_GRADE_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_STORE_GRADE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_STORE_GRADE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    BOUNDARY = new floatParameter("@BOUNDARY", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  double? BOUNDARY,
                                  string GRADE_CODE
                                  )
                {
                    lock (typeof(MID_HEADER_STORE_GRADE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_STORE_GRADE_VALUES_INSERT_def MID_HEADER_STORE_GRADE_VALUES_INSERT = new MID_HEADER_STORE_GRADE_VALUES_INSERT_def();
            public class MID_HEADER_STORE_GRADE_VALUES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_STORE_GRADE_VALUES_INSERT.SQL"

                private intParameter HDR_RID;
                private floatParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;
                private intParameter MINIMUM_COLOR;
                private intParameter MAXIMUM_COLOR;
                private intParameter SHIP_UP_TO;
                private intParameter SGL_RID;

                public MID_HEADER_STORE_GRADE_VALUES_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_STORE_GRADE_VALUES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_STORE_GRADE_VALUES");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    BOUNDARY = new floatParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
                    MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
                    MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
                    MINIMUM_COLOR = new intParameter("@MINIMUM_COLOR", base.inputParameterList);
                    MAXIMUM_COLOR = new intParameter("@MAXIMUM_COLOR", base.inputParameterList);
                    SHIP_UP_TO = new intParameter("@SHIP_UP_TO", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  double? BOUNDARY,
                                  int? MINIMUM_STOCK,
                                  int? MAXIMUM_STOCK,
                                  int? MINIMUM_AD,
                                  int? MINIMUM_COLOR,
                                  int? MAXIMUM_COLOR,
                                  int? SHIP_UP_TO,
                                  int? SGL_RID
                                  )
                {
                    lock (typeof(MID_HEADER_STORE_GRADE_VALUES_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        this.MINIMUM_COLOR.SetValue(MINIMUM_COLOR);
                        this.MAXIMUM_COLOR.SetValue(MAXIMUM_COLOR);
                        this.SHIP_UP_TO.SetValue(SHIP_UP_TO);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_STORE_GRADE_DELETE_def MID_HEADER_STORE_GRADE_DELETE = new MID_HEADER_STORE_GRADE_DELETE_def();
            public class MID_HEADER_STORE_GRADE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_STORE_GRADE_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_STORE_GRADE_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_STORE_GRADE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_STORE_GRADE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_STORE_GRADE_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_STORE_GRADE_VALUES_DELETE_def MID_HEADER_STORE_GRADE_VALUES_DELETE = new MID_HEADER_STORE_GRADE_VALUES_DELETE_def();
            public class MID_HEADER_STORE_GRADE_VALUES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_STORE_GRADE_VALUES_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_STORE_GRADE_VALUES_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_STORE_GRADE_VALUES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_STORE_GRADE_VALUES");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_STORE_GRADE_VALUES_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_SIZE_NEED_INSERT_def MID_HEADER_SIZE_NEED_INSERT = new MID_HEADER_SIZE_NEED_INSERT_def();
            public class MID_HEADER_SIZE_NEED_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_SIZE_NEED_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter SIZE_CURVE_GROUP_RID;
                private intParameter MERCH_TYPE;
                private intParameter HN_RID;
                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter SIZE_CONSTRAINT_RID;
                private intParameter SIZE_ALTERNATE_RID;
                private charParameter NORMALIZE_SIZE_CURVES_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter VSW_SIZE_CONSTRAINTS;

                public MID_HEADER_SIZE_NEED_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_SIZE_NEED_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
                    MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
                    SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
                    NORMALIZE_SIZE_CURVES_IND = new charParameter("@NORMALIZE_SIZE_CURVES_IND", base.inputParameterList);
                    IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
                    IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
                    IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
                    IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                    VSW_SIZE_CONSTRAINTS = new intParameter("@VSW_SIZE_CONSTRAINTS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? SIZE_CURVE_GROUP_RID,
                                  int? MERCH_TYPE,
                                  int? HN_RID,
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  int? SIZE_CONSTRAINT_RID,
                                  int? SIZE_ALTERNATE_RID,
                                  char? NORMALIZE_SIZE_CURVES_IND,
                                  int? IB_MERCH_TYPE,
                                  int? IB_MERCH_HN_RID,
                                  int? IB_MERCH_PH_RID,
                                  int? IB_MERCH_PHL_SEQUENCE,
                                  int? VSW_SIZE_CONSTRAINTS
                                  )
                {
                    lock (typeof(MID_HEADER_SIZE_NEED_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        this.NORMALIZE_SIZE_CURVES_IND.SetValue(NORMALIZE_SIZE_CURVES_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.VSW_SIZE_CONSTRAINTS.SetValue(VSW_SIZE_CONSTRAINTS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_SIZE_NEED_DELETE_def MID_HEADER_SIZE_NEED_DELETE = new MID_HEADER_SIZE_NEED_DELETE_def();
            public class MID_HEADER_SIZE_NEED_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_SIZE_NEED_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_SIZE_NEED_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_SIZE_NEED_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_SIZE_NEED_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT_def MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT = new MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT_def();
            public class MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter SIZE_CURVE_GROUP_RID;
                private intParameter MERCH_TYPE;
                private intParameter HN_RID;
                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter SIZE_CONSTRAINT_RID;
                private intParameter SIZE_ALTERNATE_RID;
                private charParameter NORMALIZE_SIZE_CURVES_IND;
                private intParameter IB_MERCH_TYPE;
                private intParameter IB_MERCH_HN_RID;
                private intParameter IB_MERCH_PH_RID;
                private intParameter IB_MERCH_PHL_SEQUENCE;
                private intParameter VSW_SIZE_CONSTRAINTS;
                private intParameter FILL_SIZES_TO_TYPE;

                public MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
                    MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
                    SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
                    NORMALIZE_SIZE_CURVES_IND = new charParameter("@NORMALIZE_SIZE_CURVES_IND", base.inputParameterList);
                    IB_MERCH_TYPE = new intParameter("@IB_MERCH_TYPE", base.inputParameterList);
                    IB_MERCH_HN_RID = new intParameter("@IB_MERCH_HN_RID", base.inputParameterList);
                    IB_MERCH_PH_RID = new intParameter("@IB_MERCH_PH_RID", base.inputParameterList);
                    IB_MERCH_PHL_SEQUENCE = new intParameter("@IB_MERCH_PHL_SEQUENCE", base.inputParameterList);
                    VSW_SIZE_CONSTRAINTS = new intParameter("@VSW_SIZE_CONSTRAINTS", base.inputParameterList);
                    FILL_SIZES_TO_TYPE = new intParameter("@FILL_SIZES_TO_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? HDR_BC_RID,
                                  int? SIZE_CURVE_GROUP_RID,
                                  int? MERCH_TYPE,
                                  int? HN_RID,
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  int? SIZE_CONSTRAINT_RID,
                                  int? SIZE_ALTERNATE_RID,
                                  char? NORMALIZE_SIZE_CURVES_IND,
                                  int? IB_MERCH_TYPE,
                                  int? IB_MERCH_HN_RID,
                                  int? IB_MERCH_PH_RID,
                                  int? IB_MERCH_PHL_SEQUENCE,
                                  int? VSW_SIZE_CONSTRAINTS,
                                  int? FILL_SIZES_TO_TYPE
                                  )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        this.NORMALIZE_SIZE_CURVES_IND.SetValue(NORMALIZE_SIZE_CURVES_IND);
                        this.IB_MERCH_TYPE.SetValue(IB_MERCH_TYPE);
                        this.IB_MERCH_HN_RID.SetValue(IB_MERCH_HN_RID);
                        this.IB_MERCH_PH_RID.SetValue(IB_MERCH_PH_RID);
                        this.IB_MERCH_PHL_SEQUENCE.SetValue(IB_MERCH_PHL_SEQUENCE);
                        this.VSW_SIZE_CONSTRAINTS.SetValue(VSW_SIZE_CONSTRAINTS);
                        this.FILL_SIZES_TO_TYPE.SetValue(FILL_SIZES_TO_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_def MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE = new MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_def();
            public class MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE_NEED");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR_def MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR = new MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR_def();
            public class MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR.SQL"

                private intParameter HDR_BC_RID;

                public MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE_NEED");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_BC_RID)
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_NEED_DELETE_BY_COLOR_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_FROM_PACK_def MID_HEADER_PACK_READ_FROM_PACK = new MID_HEADER_PACK_READ_FROM_PACK_def();
            public class MID_HEADER_PACK_READ_FROM_PACK_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_FROM_PACK.SQL"

                private intParameter HDR_PACK_RID;

                public MID_HEADER_PACK_READ_FROM_PACK_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_FROM_PACK";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_PACK_RID)
                {
                    lock (typeof(MID_HEADER_PACK_READ_FROM_PACK_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_RID_FROM_NAME_def MID_HEADER_PACK_READ_RID_FROM_NAME = new MID_HEADER_PACK_READ_RID_FROM_NAME_def();
            public class MID_HEADER_PACK_READ_RID_FROM_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_RID_FROM_NAME.SQL"

                private intParameter HDR_RID;
                private stringParameter HDR_PACK_NAME;

                public MID_HEADER_PACK_READ_RID_FROM_NAME_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_RID_FROM_NAME";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_PACK_NAME = new stringParameter("@HDR_PACK_NAME", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, 
                                         int? HDR_RID,
                                         string HDR_PACK_NAME
                                         )
                {
                    lock (typeof(MID_HEADER_PACK_READ_RID_FROM_NAME_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_PACK_NAME.SetValue(HDR_PACK_NAME);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_ALL_def MID_HEADER_PACK_READ_ALL = new MID_HEADER_PACK_READ_ALL_def();
            public class MID_HEADER_PACK_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_ALL.SQL"

                public MID_HEADER_PACK_READ_ALL_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_PACK");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_PACK_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_HDRPACK_DELETE_def SP_MID_HDRPACK_DELETE = new SP_MID_HDRPACK_DELETE_def();
            public class SP_MID_HDRPACK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HDRPACK_DELETE.SQL"

                private intParameter PACK_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures

                public SP_MID_HDRPACK_DELETE_def()
                {
                    base.procedureName = "SP_MID_HDRPACK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_PACK");
                    PACK_RID = new intParameter("@PACK_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                }

                public int Delete(DatabaseAccess _dba, int? PACK_RID)
                {
                    lock (typeof(SP_MID_HDRPACK_DELETE_def))
                    {
                        this.PACK_RID.SetValue(PACK_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HDRPACKCOLOR_DELETE_def SP_MID_HDRPACKCOLOR_DELETE = new SP_MID_HDRPACKCOLOR_DELETE_def();
            public class SP_MID_HDRPACKCOLOR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HDRPACKCOLOR_DELETE.SQL"

                private intParameter PACK_RID;
                private intParameter COLOR_RID;

                public SP_MID_HDRPACKCOLOR_DELETE_def()
                {
                    base.procedureName = "SP_MID_HDRPACKCOLOR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    PACK_RID = new intParameter("@PACK_RID", base.inputParameterList);
                    COLOR_RID = new intParameter("@COLOR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? PACK_RID,
                                  int? COLOR_RID
                                  )
                {
                    lock (typeof(SP_MID_HDRPACKCOLOR_DELETE_def))
                    {
                        this.PACK_RID.SetValue(PACK_RID);
                        this.COLOR_RID.SetValue(COLOR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HDRPACKCOLORSIZE_DELETE_def SP_MID_HDRPACKCOLORSIZE_DELETE = new SP_MID_HDRPACKCOLORSIZE_DELETE_def();
            public class SP_MID_HDRPACKCOLORSIZE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HDRPACKCOLORSIZE_DELETE.SQL"

                private intParameter HDR_PC_RID;
                private intParameter SIZE_RID;

                public SP_MID_HDRPACKCOLORSIZE_DELETE_def()
                {
                    base.procedureName = "SP_MID_HDRPACKCOLORSIZE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.inputParameterList);
                    SIZE_RID = new intParameter("@SIZE_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HDR_PC_RID,
                                  int? SIZE_RID
                                  )
                {
                    lock (typeof(SP_MID_HDRPACKCOLORSIZE_DELETE_def))
                    {
                        this.HDR_PC_RID.SetValue(HDR_PC_RID);
                        this.SIZE_RID.SetValue(SIZE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_BULK_DELETE_def SP_MID_HEADER_BULK_DELETE = new SP_MID_HEADER_BULK_DELETE_def();
            public class SP_MID_HEADER_BULK_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_BULK_DELETE.SQL"

                private intParameter HDR_BC_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures

                public SP_MID_HEADER_BULK_DELETE_def()
                {
                    base.procedureName = "SP_MID_HEADER_BULK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                }

                public int Delete(DatabaseAccess _dba, int? HDR_BC_RID)
                {
                    lock (typeof(SP_MID_HEADER_BULK_DELETE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HDRBULKCLRSZ_DELETE_def SP_MID_HDRBULKCLRSZ_DELETE = new SP_MID_HDRBULKCLRSZ_DELETE_def();
            public class SP_MID_HDRBULKCLRSZ_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HDRBULKCLRSZ_DELETE.SQL"

                private intParameter HDR_BC_RID;
                private intParameter SIZE_CODE_RID;

                public SP_MID_HDRBULKCLRSZ_DELETE_def()
                {
                    base.procedureName = "SP_MID_HDRBULKCLRSZ_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HDR_BC_RID,
                                  int? SIZE_CODE_RID
                                  )
                {
                    lock (typeof(SP_MID_HDRBULKCLRSZ_DELETE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_PACK_INSERT_def SP_MID_HEADER_PACK_INSERT = new SP_MID_HEADER_PACK_INSERT_def();
            public class SP_MID_HEADER_PACK_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_PACK_INSERT.SQL"

                private intParameter HDR_RID;
                private stringParameter HDR_PACK_NAME;
                private intParameter PACKS;
                private intParameter MULTIPLE;
                private intParameter RESERVE_PACKS;
                private charParameter GENERIC_IND;
                private intParameter ASSOCIATED_PACK_RID;
                private intParameter SEQ;
                //OUPUT
                private intParameter HDR_PACK_RID;

                public SP_MID_HEADER_PACK_INSERT_def()
                {
                    base.procedureName = "SP_MID_HEADER_PACK_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_PACK_NAME = new stringParameter("@HDR_PACK_NAME", base.inputParameterList);
                    PACKS = new intParameter("@PACKS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    RESERVE_PACKS = new intParameter("@RESERVE_PACKS", base.inputParameterList);
                    GENERIC_IND = new charParameter("@GENERIC_IND", base.inputParameterList);
                    ASSOCIATED_PACK_RID = new intParameter("@ASSOCIATED_PACK_RID", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    //OUPUT
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.outputParameterList);
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  string HDR_PACK_NAME,
                                  int? PACKS,
                                  int? MULTIPLE,
                                  int? RESERVE_PACKS,
                                  char? GENERIC_IND,
                                  int? ASSOCIATED_PACK_RID,
                                  int? SEQ
                                  )
                {
                    lock (typeof(SP_MID_HEADER_PACK_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_PACK_NAME.SetValue(HDR_PACK_NAME);
                        this.PACKS.SetValue(PACKS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.RESERVE_PACKS.SetValue(RESERVE_PACKS);
                        this.GENERIC_IND.SetValue(GENERIC_IND);
                        this.ASSOCIATED_PACK_RID.SetValue(ASSOCIATED_PACK_RID);
                        this.SEQ.SetValue(SEQ);
                        //OUPUT
                        this.HDR_PACK_RID.SetValue(null);
                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_UPDATE_def MID_HEADER_PACK_UPDATE = new MID_HEADER_PACK_UPDATE_def();
            public class MID_HEADER_PACK_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_UPDATE.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter HDR_RID;
                private stringParameter HDR_PACK_NAME;
                private intParameter PACKS;
                private intParameter MULTIPLE;
                private intParameter RESERVE_PACKS;
                private charParameter GENERIC_IND;
                private intParameter ASSOCIATED_PACK_RID;
                private intParameter SEQ;

                public MID_HEADER_PACK_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_PACK_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_PACK_NAME = new stringParameter("@HDR_PACK_NAME", base.inputParameterList);
                    PACKS = new intParameter("@PACKS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    RESERVE_PACKS = new intParameter("@RESERVE_PACKS", base.inputParameterList);
                    GENERIC_IND = new charParameter("@GENERIC_IND", base.inputParameterList);
                    ASSOCIATED_PACK_RID = new intParameter("@ASSOCIATED_PACK_RID", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_PACK_RID,
                                  int? HDR_RID,
                                  string HDR_PACK_NAME,
                                  int? PACKS,
                                  int? MULTIPLE,
                                  int? RESERVE_PACKS,
                                  char? GENERIC_IND,
                                  int? ASSOCIATED_PACK_RID,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_UPDATE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_PACK_NAME.SetValue(HDR_PACK_NAME);
                        this.PACKS.SetValue(PACKS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.RESERVE_PACKS.SetValue(RESERVE_PACKS);
                        this.GENERIC_IND.SetValue(GENERIC_IND);
                        this.ASSOCIATED_PACK_RID.SetValue(ASSOCIATED_PACK_RID);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_PACKCOLOR_INSERT_def SP_MID_HEADER_PACKCOLOR_INSERT = new SP_MID_HEADER_PACKCOLOR_INSERT_def();
            public class SP_MID_HEADER_PACKCOLOR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_PACKCOLOR_INSERT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter COLOR_CODE_RID;
                private intParameter UNITS;
                private intParameter SEQ;
                private stringParameter NAME;
                private stringParameter DESCRIPTION;
                private intParameter LAST_PCSZ_KEY_USED;
                private intParameter HDR_PC_RID; //Declare Output Parameter

                public SP_MID_HEADER_PACKCOLOR_INSERT_def()
                {
                    base.procedureName = "SP_MID_HEADER_PACKCOLOR_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    LAST_PCSZ_KEY_USED = new intParameter("@LAST_PCSZ_KEY_USED", base.inputParameterList);

                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? HDR_PACK_RID,
                                              int? COLOR_CODE_RID,
                                              int? UNITS,
                                              int? SEQ,
                                              string NAME,
                                              string DESCRIPTION,
                                              int? LAST_PCSZ_KEY_USED
                                              )
                {
                    lock (typeof(SP_MID_HEADER_PACKCOLOR_INSERT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.SEQ.SetValue(SEQ);
                        this.NAME.SetValue(NAME);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.LAST_PCSZ_KEY_USED.SetValue(LAST_PCSZ_KEY_USED);
                        this.HDR_PC_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_UPDATE_def MID_HEADER_PACK_COLOR_UPDATE = new MID_HEADER_PACK_COLOR_UPDATE_def();
            public class MID_HEADER_PACK_COLOR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_UPDATE.SQL"

                private intParameter HDR_PC_RID;
                private intParameter UNITS;
                private intParameter SEQ;
                private stringParameter NAME;
                private stringParameter DESCRIPTION;
                private intParameter LAST_PCSZ_KEY_USED;

                public MID_HEADER_PACK_COLOR_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    LAST_PCSZ_KEY_USED = new intParameter("@LAST_PCSZ_KEY_USED", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_PC_RID,
                                  int? UNITS,
                                  int? SEQ,
                                  string NAME,
                                  string DESCRIPTION,
                                  int? LAST_PCSZ_KEY_USED
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_UPDATE_def))
                    {
                        this.HDR_PC_RID.SetValue(HDR_PC_RID);
                        this.UNITS.SetValue(UNITS);
                        this.SEQ.SetValue(SEQ);
                        this.NAME.SetValue(NAME);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.LAST_PCSZ_KEY_USED.SetValue(LAST_PCSZ_KEY_USED);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR_def MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR = new MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR_def();
            public class MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR.SQL"

                private intParameter HDR_PC_RID;
                private intParameter COLOR_CODE_RID_NEW;
                private intParameter UNITS;
                private intParameter SEQ;
                private stringParameter NAME;
                private stringParameter DESCRIPTION;
                private intParameter LAST_PCSZ_KEY_USED;

                public MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.inputParameterList);
                    COLOR_CODE_RID_NEW = new intParameter("@COLOR_CODE_RID_NEW", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    LAST_PCSZ_KEY_USED = new intParameter("@LAST_PCSZ_KEY_USED", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_PC_RID,
                                  int? COLOR_CODE_RID_NEW,
                                  int? UNITS,
                                  int? SEQ,
                                  string NAME,
                                  string DESCRIPTION,
                                  int? LAST_PCSZ_KEY_USED
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_UPDATE_WITH_COLOR_def))
                    {
                        this.HDR_PC_RID.SetValue(HDR_PC_RID);
                        this.COLOR_CODE_RID_NEW.SetValue(COLOR_CODE_RID_NEW);
                        this.UNITS.SetValue(UNITS);
                        this.SEQ.SetValue(SEQ);
                        this.NAME.SetValue(NAME);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.LAST_PCSZ_KEY_USED.SetValue(LAST_PCSZ_KEY_USED);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_ROUNDING_DELETE_def MID_HEADER_PACK_ROUNDING_DELETE = new MID_HEADER_PACK_ROUNDING_DELETE_def();
            public class MID_HEADER_PACK_ROUNDING_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_ROUNDING_DELETE.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_PACK_ROUNDING_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_PACK_ROUNDING_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_PACK_ROUNDING");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_PACK_ROUNDING_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_ROUNDING_INSERT_def MID_HEADER_PACK_ROUNDING_INSERT = new MID_HEADER_PACK_ROUNDING_INSERT_def();
            public class MID_HEADER_PACK_ROUNDING_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_ROUNDING_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter PACK_MULTIPLE_RID;
                private floatParameter PACK_ROUNDING_1ST_PACK_PCT;
                private floatParameter PACK_ROUNDING_NTH_PACK_PCT;

                public MID_HEADER_PACK_ROUNDING_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_PACK_ROUNDING_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_PACK_ROUNDING");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    PACK_MULTIPLE_RID = new intParameter("@PACK_MULTIPLE_RID", base.inputParameterList);
                    PACK_ROUNDING_1ST_PACK_PCT = new floatParameter("@PACK_ROUNDING_1ST_PACK_PCT", base.inputParameterList);
                    PACK_ROUNDING_NTH_PACK_PCT = new floatParameter("@PACK_ROUNDING_NTH_PACK_PCT", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? PACK_MULTIPLE_RID,
                                  double? PACK_ROUNDING_1ST_PACK_PCT,
                                  double? PACK_ROUNDING_NTH_PACK_PCT
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_ROUNDING_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.PACK_MULTIPLE_RID.SetValue(PACK_MULTIPLE_RID);
                        this.PACK_ROUNDING_1ST_PACK_PCT.SetValue(PACK_ROUNDING_1ST_PACK_PCT);
                        this.PACK_ROUNDING_NTH_PACK_PCT.SetValue(PACK_ROUNDING_NTH_PACK_PCT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_HEADER_BULK_INSERT_def SP_MID_HEADER_BULK_INSERT = new SP_MID_HEADER_BULK_INSERT_def();
            public class SP_MID_HEADER_BULK_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_BULK_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter COLOR_CODE_RID;
                private intParameter UNITS;
                private intParameter MULTIPLE;
                private intParameter MINIMUM;
                private intParameter MAXIMUM;
                private intParameter RESERVE_UNITS;
                private intParameter SEQ;
                private stringParameter NAME;
                private stringParameter DESCRIPTION;
                private intParameter ASRT_BC_RID;
                private intParameter LAST_BCSZ_KEY_USED;
                private intParameter COLOR_STATUS_FLAGS;
                //OUPUT
                private intParameter HDR_BC_RID;

                public SP_MID_HEADER_BULK_INSERT_def()
                {
                    base.procedureName = "SP_MID_HEADER_BULK_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    MINIMUM = new intParameter("@MINIMUM", base.inputParameterList);
                    MAXIMUM = new intParameter("@MAXIMUM", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    ASRT_BC_RID = new intParameter("@ASRT_BC_RID", base.inputParameterList);
                    LAST_BCSZ_KEY_USED = new intParameter("@LAST_BCSZ_KEY_USED", base.inputParameterList);
                    COLOR_STATUS_FLAGS = new intParameter("@COLOR_STATUS_FLAGS", base.inputParameterList);
                    //OUPUT
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.outputParameterList);
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? COLOR_CODE_RID,
                                  int? UNITS,
                                  int? MULTIPLE,
                                  int? MINIMUM,
                                  int? MAXIMUM,
                                  int? RESERVE_UNITS,
                                  int? SEQ,
                                  string NAME,
                                  string DESCRIPTION,
                                  int? ASRT_BC_RID,
                                  int? LAST_BCSZ_KEY_USED,
                                  int? COLOR_STATUS_FLAGS
                                  )
                {
                    lock (typeof(SP_MID_HEADER_BULK_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.MINIMUM.SetValue(MINIMUM);
                        this.MAXIMUM.SetValue(MAXIMUM);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.SEQ.SetValue(SEQ);
                        this.NAME.SetValue(NAME);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.ASRT_BC_RID.SetValue(ASRT_BC_RID);
                        this.LAST_BCSZ_KEY_USED.SetValue(LAST_BCSZ_KEY_USED);
                        this.COLOR_STATUS_FLAGS.SetValue(COLOR_STATUS_FLAGS);
                        //OUPUT
                        this.HDR_BC_RID.SetValue(null);
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_UPDATE_def MID_HEADER_BULK_COLOR_UPDATE = new MID_HEADER_BULK_COLOR_UPDATE_def();
            public class MID_HEADER_BULK_COLOR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter COLOR_CODE_RID;
                private intParameter UNITS;
                private intParameter MULTIPLE;
                private intParameter MINIMUM;
                private intParameter MAXIMUM;
                private intParameter RESERVE_UNITS;
                private intParameter SEQ;
                private stringParameter NAME;
                private stringParameter DESCRIPTION;
                private intParameter ASRT_BC_RID;
                private intParameter COLOR_STATUS_FLAGS;
                private intParameter LAST_BCSZ_KEY_USED;

                public MID_HEADER_BULK_COLOR_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    MINIMUM = new intParameter("@MINIMUM", base.inputParameterList);
                    MAXIMUM = new intParameter("@MAXIMUM", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    ASRT_BC_RID = new intParameter("@ASRT_BC_RID", base.inputParameterList);
                    COLOR_STATUS_FLAGS = new intParameter("@COLOR_STATUS_FLAGS", base.inputParameterList);
                    LAST_BCSZ_KEY_USED = new intParameter("@LAST_BCSZ_KEY_USED", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? HDR_BC_RID,
                                  int? COLOR_CODE_RID,
                                  int? UNITS,
                                  int? MULTIPLE,
                                  int? MINIMUM,
                                  int? MAXIMUM,
                                  int? RESERVE_UNITS,
                                  int? SEQ,
                                  string NAME,
                                  string DESCRIPTION,
                                  int? ASRT_BC_RID,
                                  int? COLOR_STATUS_FLAGS,
                                  int? LAST_BCSZ_KEY_USED
                                  )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.MINIMUM.SetValue(MINIMUM);
                        this.MAXIMUM.SetValue(MAXIMUM);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.SEQ.SetValue(SEQ);
                        this.NAME.SetValue(NAME);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.ASRT_BC_RID.SetValue(ASRT_BC_RID);
                        this.COLOR_STATUS_FLAGS.SetValue(COLOR_STATUS_FLAGS);
                        this.LAST_BCSZ_KEY_USED.SetValue(LAST_BCSZ_KEY_USED);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_INSERT_def MID_HEADER_BULK_COLOR_SIZE_INSERT = new MID_HEADER_BULK_COLOR_SIZE_INSERT_def();
            public class MID_HEADER_BULK_COLOR_SIZE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter HDR_BCSZ_KEY;
                private intParameter SIZE_CODE_RID;
                private intParameter UNITS;
                private intParameter MULTIPLE;
                private intParameter MINIMUM;
                private intParameter MAXIMUM;
                private intParameter RESERVE_UNITS;
                private intParameter SEQ;

                public MID_HEADER_BULK_COLOR_SIZE_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    HDR_BCSZ_KEY = new intParameter("@HDR_BCSZ_KEY", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    MINIMUM = new intParameter("@MINIMUM", base.inputParameterList);
                    MAXIMUM = new intParameter("@MAXIMUM", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? HDR_BC_RID,
                                  int? HDR_BCSZ_KEY,
                                  int? SIZE_CODE_RID,
                                  int? UNITS,
                                  int? MULTIPLE,
                                  int? MINIMUM,
                                  int? MAXIMUM,
                                  int? RESERVE_UNITS,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.HDR_BCSZ_KEY.SetValue(HDR_BCSZ_KEY);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.MINIMUM.SetValue(MINIMUM);
                        this.MAXIMUM.SetValue(MAXIMUM);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_BULK_COLOR_SIZE_UPDATE_def MID_HEADER_BULK_COLOR_SIZE_UPDATE = new MID_HEADER_BULK_COLOR_SIZE_UPDATE_def();
            public class MID_HEADER_BULK_COLOR_SIZE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_SIZE_UPDATE.SQL"

                private intParameter HDR_BC_RID;
                private intParameter HDR_BCSZ_KEY;
                private intParameter SIZE_CODE_RID;
                private intParameter UNITS;
                private intParameter MULTIPLE;
                private intParameter MINIMUM;
                private intParameter MAXIMUM;
                private intParameter RESERVE_UNITS;
                private intParameter SEQ;

                public MID_HEADER_BULK_COLOR_SIZE_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_SIZE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_BULK_COLOR_SIZE");
                    HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
                    HDR_BCSZ_KEY = new intParameter("@HDR_BCSZ_KEY", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    MULTIPLE = new intParameter("@MULTIPLE", base.inputParameterList);
                    MINIMUM = new intParameter("@MINIMUM", base.inputParameterList);
                    MAXIMUM = new intParameter("@MAXIMUM", base.inputParameterList);
                    RESERVE_UNITS = new intParameter("@RESERVE_UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_BC_RID,
                                  int? HDR_BCSZ_KEY,
                                  int? SIZE_CODE_RID,
                                  int? UNITS,
                                  int? MULTIPLE,
                                  int? MINIMUM,
                                  int? MAXIMUM,
                                  int? RESERVE_UNITS,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_SIZE_UPDATE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.HDR_BCSZ_KEY.SetValue(HDR_BCSZ_KEY);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.MULTIPLE.SetValue(MULTIPLE);
                        this.MINIMUM.SetValue(MINIMUM);
                        this.MAXIMUM.SetValue(MAXIMUM);
                        this.RESERVE_UNITS.SetValue(RESERVE_UNITS);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_SIZE_INSERT_def MID_HEADER_PACK_COLOR_SIZE_INSERT = new MID_HEADER_PACK_COLOR_SIZE_INSERT_def();
            public class MID_HEADER_PACK_COLOR_SIZE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_SIZE_INSERT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter HDR_PC_RID;
                private intParameter HDR_PCSZ_KEY;
                private intParameter SIZE_CODE_RID;
                private intParameter UNITS;
                private intParameter SEQ;

                public MID_HEADER_PACK_COLOR_SIZE_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_SIZE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.inputParameterList);
                    HDR_PCSZ_KEY = new intParameter("@HDR_PCSZ_KEY", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HDR_PACK_RID,
                                  int? HDR_PC_RID,
                                  int? HDR_PCSZ_KEY,
                                  int? SIZE_CODE_RID,
                                  int? UNITS,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_SIZE_INSERT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.HDR_PC_RID.SetValue(HDR_PC_RID);
                        this.HDR_PCSZ_KEY.SetValue(HDR_PCSZ_KEY);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_SIZE_UPDATE_def MID_HEADER_PACK_COLOR_SIZE_UPDATE = new MID_HEADER_PACK_COLOR_SIZE_UPDATE_def();
            public class MID_HEADER_PACK_COLOR_SIZE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_SIZE_UPDATE.SQL"

                private intParameter HDR_PC_RID;
                private intParameter HDR_PCSZ_KEY;
                private intParameter SIZE_CODE_RID;
                private intParameter UNITS;
                private intParameter SEQ;

                public MID_HEADER_PACK_COLOR_SIZE_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_SIZE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_PACK_COLOR_SIZE");
                    HDR_PC_RID = new intParameter("@HDR_PC_RID", base.inputParameterList);
                    HDR_PCSZ_KEY = new intParameter("@HDR_PCSZ_KEY", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    UNITS = new intParameter("@UNITS", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_PC_RID,
                                  int? HDR_PCSZ_KEY,
                                  int? SIZE_CODE_RID,
                                  int? UNITS,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_SIZE_UPDATE_def))
                    {
                        this.HDR_PC_RID.SetValue(HDR_PC_RID);
                        this.HDR_PCSZ_KEY.SetValue(HDR_PCSZ_KEY);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.UNITS.SetValue(UNITS);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_READ_ALL_SUBORD_def MID_MASTER_HEADER_READ_ALL_SUBORD = new MID_MASTER_HEADER_READ_ALL_SUBORD_def();
            public class MID_MASTER_HEADER_READ_ALL_SUBORD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_READ_ALL_SUBORD.SQL"

                public MID_MASTER_HEADER_READ_ALL_SUBORD_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_READ_ALL_SUBORD";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MASTER_HEADER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_MASTER_HEADER_READ_ALL_SUBORD_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_INSERT_def MID_MASTER_HEADER_INSERT = new MID_MASTER_HEADER_INSERT_def();
            public class MID_MASTER_HEADER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_INSERT.SQL"

                private intParameter SUBORD_HDR_RID;
                private intParameter MASTER_HDR_RID;
                private intParameter MASTER_COMPONENT;
                private intParameter MASTER_PACK_RID;
                private intParameter MASTER_BC_RID;
                private intParameter SUBORD_COMPONENT;
                private intParameter SUBORD_PACK_RID;
                private intParameter SUBORD_BC_RID;

                public MID_MASTER_HEADER_INSERT_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("MASTER_HEADER");
                    SUBORD_HDR_RID = new intParameter("@SUBORD_HDR_RID", base.inputParameterList);
                    MASTER_HDR_RID = new intParameter("@MASTER_HDR_RID", base.inputParameterList);
                    MASTER_COMPONENT = new intParameter("@MASTER_COMPONENT", base.inputParameterList);
                    MASTER_PACK_RID = new intParameter("@MASTER_PACK_RID", base.inputParameterList);
                    MASTER_BC_RID = new intParameter("@MASTER_BC_RID", base.inputParameterList);
                    SUBORD_COMPONENT = new intParameter("@SUBORD_COMPONENT", base.inputParameterList);
                    SUBORD_PACK_RID = new intParameter("@SUBORD_PACK_RID", base.inputParameterList);
                    SUBORD_BC_RID = new intParameter("@SUBORD_BC_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? SUBORD_HDR_RID,
                                  int? MASTER_HDR_RID,
                                  int? MASTER_COMPONENT,
                                  int? MASTER_PACK_RID,
                                  int? MASTER_BC_RID,
                                  int? SUBORD_COMPONENT,
                                  int? SUBORD_PACK_RID,
                                  int? SUBORD_BC_RID
                                  )
                {
                    lock (typeof(MID_MASTER_HEADER_INSERT_def))
                    {
                        this.SUBORD_HDR_RID.SetValue(SUBORD_HDR_RID);
                        this.MASTER_HDR_RID.SetValue(MASTER_HDR_RID);
                        this.MASTER_COMPONENT.SetValue(MASTER_COMPONENT);
                        this.MASTER_PACK_RID.SetValue(MASTER_PACK_RID);
                        this.MASTER_BC_RID.SetValue(MASTER_BC_RID);
                        this.SUBORD_COMPONENT.SetValue(SUBORD_COMPONENT);
                        this.SUBORD_PACK_RID.SetValue(SUBORD_PACK_RID);
                        this.SUBORD_BC_RID.SetValue(SUBORD_BC_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_UPDATE_def MID_MASTER_HEADER_UPDATE = new MID_MASTER_HEADER_UPDATE_def();
            public class MID_MASTER_HEADER_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_UPDATE.SQL"

                private intParameter SUBORD_HDR_RID;
                private intParameter MASTER_HDR_RID;
                private intParameter MASTER_COMPONENT;
                private intParameter MASTER_PACK_RID;
                private intParameter MASTER_BC_RID;
                private intParameter SUBORD_COMPONENT;
                private intParameter SUBORD_PACK_RID;
                private intParameter SUBORD_BC_RID;

                public MID_MASTER_HEADER_UPDATE_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("MASTER_HEADER");
                    SUBORD_HDR_RID = new intParameter("@SUBORD_HDR_RID", base.inputParameterList);
                    MASTER_HDR_RID = new intParameter("@MASTER_HDR_RID", base.inputParameterList);
                    MASTER_COMPONENT = new intParameter("@MASTER_COMPONENT", base.inputParameterList);
                    MASTER_PACK_RID = new intParameter("@MASTER_PACK_RID", base.inputParameterList);
                    MASTER_BC_RID = new intParameter("@MASTER_BC_RID", base.inputParameterList);
                    SUBORD_COMPONENT = new intParameter("@SUBORD_COMPONENT", base.inputParameterList);
                    SUBORD_PACK_RID = new intParameter("@SUBORD_PACK_RID", base.inputParameterList);
                    SUBORD_BC_RID = new intParameter("@SUBORD_BC_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? SUBORD_HDR_RID,
                                  int? MASTER_HDR_RID,
                                  int? MASTER_COMPONENT,
                                  int? MASTER_PACK_RID,
                                  int? MASTER_BC_RID,
                                  int? SUBORD_COMPONENT,
                                  int? SUBORD_PACK_RID,
                                  int? SUBORD_BC_RID
                                  )
                {
                    lock (typeof(MID_MASTER_HEADER_UPDATE_def))
                    {
                        this.SUBORD_HDR_RID.SetValue(SUBORD_HDR_RID);
                        this.MASTER_HDR_RID.SetValue(MASTER_HDR_RID);
                        this.MASTER_COMPONENT.SetValue(MASTER_COMPONENT);
                        this.MASTER_PACK_RID.SetValue(MASTER_PACK_RID);
                        this.MASTER_BC_RID.SetValue(MASTER_BC_RID);
                        this.SUBORD_COMPONENT.SetValue(SUBORD_COMPONENT);
                        this.SUBORD_PACK_RID.SetValue(SUBORD_PACK_RID);
                        this.SUBORD_BC_RID.SetValue(SUBORD_BC_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_DELETE_def MID_MASTER_HEADER_DELETE = new MID_MASTER_HEADER_DELETE_def();
            public class MID_MASTER_HEADER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_DELETE.SQL"

                private intParameter SUBORD_HDR_RID;

                public MID_MASTER_HEADER_DELETE_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("MASTER_HEADER");
                    SUBORD_HDR_RID = new intParameter("@SUBORD_HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SUBORD_HDR_RID)
                {
                    lock (typeof(MID_MASTER_HEADER_DELETE_def))
                    {
                        this.SUBORD_HDR_RID.SetValue(SUBORD_HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_READ_SUBORD_def MID_MASTER_HEADER_READ_SUBORD = new MID_MASTER_HEADER_READ_SUBORD_def();
            public class MID_MASTER_HEADER_READ_SUBORD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_READ_SUBORD.SQL"

                private intParameter MASTER_HDR_RID;

                public MID_MASTER_HEADER_READ_SUBORD_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_READ_SUBORD";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("MASTER_HEADER");
                    MASTER_HDR_RID = new intParameter("@MASTER_HDR_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? MASTER_HDR_RID)
                {
                    lock (typeof(MID_MASTER_HEADER_READ_SUBORD_def))
                    {
                        this.MASTER_HDR_RID.SetValue(MASTER_HDR_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_READ_def MID_MASTER_HEADER_READ = new MID_MASTER_HEADER_READ_def();
            public class MID_MASTER_HEADER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_READ.SQL"

                private intParameter SUBORD_HDR_RID;

                public MID_MASTER_HEADER_READ_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_READ";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("MASTER_HEADER");
                    SUBORD_HDR_RID = new intParameter("@SUBORD_HDR_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? SUBORD_HDR_RID)
                {
                    lock (typeof(MID_MASTER_HEADER_READ_def))
                    {
                        this.SUBORD_HDR_RID.SetValue(SUBORD_HDR_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD_def MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD = new MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD_def();
            public class MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD.SQL"

                private intParameter SUBORD_HDR_RID;

                public MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MASTER_HEADER");
                    SUBORD_HDR_RID = new intParameter("@SUBORD_HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SUBORD_HDR_RID)
                {
                    lock (typeof(MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD_def))
                    {
                        this.SUBORD_HDR_RID.SetValue(SUBORD_HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_MASTER_HEADER_READ_COMPONENTS_def MID_MASTER_HEADER_READ_COMPONENTS = new MID_MASTER_HEADER_READ_COMPONENTS_def();
            public class MID_MASTER_HEADER_READ_COMPONENTS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MASTER_HEADER_READ_COMPONENTS.SQL"

                private intParameter MASTER_HDR_RID;

                public MID_MASTER_HEADER_READ_COMPONENTS_def()
                {
                    base.procedureName = "MID_MASTER_HEADER_READ_COMPONENTS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MASTER_HEADER");
                    MASTER_HDR_RID = new intParameter("@MASTER_HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? MASTER_HDR_RID)
                {
                    lock (typeof(MID_MASTER_HEADER_READ_COMPONENTS_def))
                    {
                        this.MASTER_HDR_RID.SetValue(MASTER_HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_DELETE_HEADER_ALLOCATION_def SP_MID_DELETE_HEADER_ALLOCATION = new SP_MID_DELETE_HEADER_ALLOCATION_def();
            public class SP_MID_DELETE_HEADER_ALLOCATION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DELETE_HEADER_ALLOCATION.SQL"

                private intParameter HDR_RID;

                public SP_MID_DELETE_HEADER_ALLOCATION_def()
                {
                    base.procedureName = "SP_MID_DELETE_HEADER_ALLOCATION";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("TOTAL_ALLOCATION");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(SP_MID_DELETE_HEADER_ALLOCATION_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_RECLASS_REJECTED_HEADER_INSERT_def MID_RECLASS_REJECTED_HEADER_INSERT = new MID_RECLASS_REJECTED_HEADER_INSERT_def();
            public class MID_RECLASS_REJECTED_HEADER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_RECLASS_REJECTED_HEADER_INSERT.SQL"

                private intParameter PROCESS_RID;
                private intParameter HDR_RID;
                private stringParameter HDR_ID;
                private stringParameter HDR_STATUS;

                public MID_RECLASS_REJECTED_HEADER_INSERT_def()
                {
                    base.procedureName = "MID_RECLASS_REJECTED_HEADER_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RECLASS_REJECTED_HEADER");
                    PROCESS_RID = new intParameter("@PROCESS_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
                    HDR_STATUS = new stringParameter("@HDR_STATUS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PROCESS_RID,
                                  int? HDR_RID,
                                  string HDR_ID,
                                  string HDR_STATUS
                                  )
                {
                    lock (typeof(MID_RECLASS_REJECTED_HEADER_INSERT_def))
                    {
                        this.PROCESS_RID.SetValue(PROCESS_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_ID.SetValue(HDR_ID);
                        this.HDR_STATUS.SetValue(HDR_STATUS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_LINKED_def MID_HEADER_READ_LINKED = new MID_HEADER_READ_LINKED_def();
            public class MID_HEADER_READ_LINKED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_LINKED.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_READ_LINKED_def()
                {
                    base.procedureName = "MID_HEADER_READ_LINKED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_READ_LINKED_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED_def MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED = new MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED_def();
            public class MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED.SQL"

                private intParameter HDR_RID;

                public MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED_def()
                {
                    base.procedureName = "MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
                {
                    lock (typeof(MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_MULTI_WITH_LINK_CHAR_def MID_HEADER_READ_MULTI_WITH_LINK_CHAR = new MID_HEADER_READ_MULTI_WITH_LINK_CHAR_def();
            public class MID_HEADER_READ_MULTI_WITH_LINK_CHAR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_MULTI_WITH_LINK_CHAR.SQL"

                private intParameter HEADER_LINK_CHARACTERISTIC;

                public MID_HEADER_READ_MULTI_WITH_LINK_CHAR_def()
                {
                    base.procedureName = "MID_HEADER_READ_MULTI_WITH_LINK_CHAR";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER");
                    HEADER_LINK_CHARACTERISTIC = new intParameter("@HEADER_LINK_CHARACTERISTIC", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HEADER_LINK_CHARACTERISTIC)
                {
                    lock (typeof(MID_HEADER_READ_MULTI_WITH_LINK_CHAR_def))
                    {
                        this.HEADER_LINK_CHARACTERISTIC.SetValue(HEADER_LINK_CHARACTERISTIC);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			// Begin TT#1581-MD - stodd - Header Reconcile API
            public static MID_HEADER_SEQUENCE_GET_NEXT_def MID_HEADER_SEQUENCE_GET_NEXT = new MID_HEADER_SEQUENCE_GET_NEXT_def();
            public class MID_HEADER_SEQUENCE_GET_NEXT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_SEQUENCE_GET_NEXT.SQL"

                private intParameter SEQ_LENGTH; 
                private intParameter SEQ_NO; //Declare Output Parameter

                public MID_HEADER_SEQUENCE_GET_NEXT_def()
                {
                    base.procedureName = "MID_HEADER_SEQUENCE_GET_NEXT";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HEADER_SEQUENCE");
                    SEQ_LENGTH = new intParameter("@SEQ_LENGTH", base.inputParameterList);
                    SEQ_NO = new intParameter("@SEQ_NO", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateAndReturnNextSequence(DatabaseAccess _dba,
                                                        int? SEQ_LENGTH
                                                      )
                {
                    lock (typeof(MID_HEADER_SEQUENCE_GET_NEXT_def))
                    {
                        this.SEQ_LENGTH.SetValue(SEQ_LENGTH);
                        base.ExecuteStoredProcedureForUpdate(_dba);
                        return (int)this.SEQ_NO.Value;
                    }
                }
            }
			// End TT#1581-MD - stodd - Header Reconcile API


            public static MID_HEADER_BULK_COLOR_UPDATE_COLOR_def MID_HEADER_BULK_COLOR_UPDATE_COLOR = new MID_HEADER_BULK_COLOR_UPDATE_COLOR_def();
            public class MID_HEADER_BULK_COLOR_UPDATE_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_UPDATE_COLOR.SQL"

                private intParameter HDR_RID;
                private intParameter OLD_COLOR_CODE_RID;
                private intParameter NEW_COLOR_CODE_RID;

                public MID_HEADER_BULK_COLOR_UPDATE_COLOR_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_UPDATE_COLOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    OLD_COLOR_CODE_RID = new intParameter("@OLD_COLOR_CODE_RID", base.inputParameterList);
                    NEW_COLOR_CODE_RID = new intParameter("@NEW_COLOR_CODE_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? OLD_COLOR_CODE_RID,
                                  int? NEW_COLOR_CODE_RID
                                  )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_UPDATE_COLOR_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.OLD_COLOR_CODE_RID.SetValue(OLD_COLOR_CODE_RID);
                        this.NEW_COLOR_CODE_RID.SetValue(NEW_COLOR_CODE_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_COLOR_UPDATE_COLOR_def MID_HEADER_PACK_COLOR_UPDATE_COLOR = new MID_HEADER_PACK_COLOR_UPDATE_COLOR_def();
            public class MID_HEADER_PACK_COLOR_UPDATE_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_COLOR_UPDATE_COLOR.SQL"

                private intParameter HDR_RID;
                private intParameter OLD_COLOR_CODE_RID;
                private intParameter NEW_COLOR_CODE_RID;

                public MID_HEADER_PACK_COLOR_UPDATE_COLOR_def()
                {
                    base.procedureName = "MID_HEADER_PACK_COLOR_UPDATE_COLOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_PACK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    OLD_COLOR_CODE_RID = new intParameter("@OLD_COLOR_CODE_RID", base.inputParameterList);
                    NEW_COLOR_CODE_RID = new intParameter("@NEW_COLOR_CODE_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HDR_RID,
                                  int? OLD_COLOR_CODE_RID,
                                  int? NEW_COLOR_CODE_RID
                                  )
                {
                    lock (typeof(MID_HEADER_PACK_COLOR_UPDATE_COLOR_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.OLD_COLOR_CODE_RID.SetValue(OLD_COLOR_CODE_RID);
                        this.NEW_COLOR_CODE_RID.SetValue(NEW_COLOR_CODE_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_NODE_DISPLAY_def MID_HIERARCHY_READ_NODE_DISPLAY = new MID_HIERARCHY_READ_NODE_DISPLAY_def();
            public class MID_HIERARCHY_READ_NODE_DISPLAY_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_NODE_DISPLAY.SQL"

                private intParameter NODE_RID;

                public MID_HIERARCHY_READ_NODE_DISPLAY_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_NODE_DISPLAY";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HIERARCHY");
                    NODE_RID = new intParameter("@NODE_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? NODE_RID)
                {
                    lock (typeof(MID_HIERARCHY_READ_NODE_DISPLAY_def))
                    {
                        this.NODE_RID.SetValue(NODE_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER_def MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER = new MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER_def();
            public class MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER.SQL"

                private intParameter headerRID;

                public MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER_def()
                {
                    base.procedureName = "MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GET_ASSORTMENT_PROPERTIES_FOR_HEADER");
                    headerRID = new intParameter("@headerRID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? headerRID)
                {
                    lock (typeof(MID_GET_ASSORTMENT_PROPERTIES_FOR_HEADER_def))
                    {
                        this.headerRID.SetValue(headerRID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_ASSORTMENT_HEADER_DELETE_def MID_ASSORTMENT_HEADER_DELETE = new MID_ASSORTMENT_HEADER_DELETE_def();
            public class MID_ASSORTMENT_HEADER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_HEADER_DELETE.SQL"

                private intParameter ASRT_RID;
                private intParameter HEADER_DELETE_COUNT; //Declare Output Parameter

                public MID_ASSORTMENT_HEADER_DELETE_def()
                {
                    base.procedureName = "MID_ASSORTMENT_HEADER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER");
                    ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
                    HEADER_DELETE_COUNT = new intParameter("@HEADER_DELETE_COUNT", base.outputParameterList); //Add Output Parameter
                }

                public int Delete(DatabaseAccess _dba, int? ASRT_RID)
                {
                    lock (typeof(MID_ASSORTMENT_HEADER_DELETE_def))
                    {
                        this.ASRT_RID.SetValue(ASRT_RID);
                        this.HEADER_DELETE_COUNT.SetValue(0); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge


            public static MID_GET_ASSORTMENT_DATA_FOR_HEADER_def MID_GET_ASSORTMENT_DATA_FOR_HEADER = new MID_GET_ASSORTMENT_DATA_FOR_HEADER_def();
            public class MID_GET_ASSORTMENT_DATA_FOR_HEADER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_ASSORTMENT_DATA_FOR_HEADER.SQL"

                private intParameter headerRID;
                private intParameter placeholderRID;
                private intParameter getAssortmentProperties;
                private intParameter getPlaceholderData;
			
			    public MID_GET_ASSORTMENT_DATA_FOR_HEADER_def()
			    {
                    base.procedureName = "MID_GET_ASSORTMENT_DATA_FOR_HEADER";
			        base.procedureType = storedProcedureTypes.ReadAsDataset;
			        base.tableNames.Add("GET_ASSORTMENT_DATA_FOR_HEADER");
                    headerRID = new intParameter("@headerRID", base.inputParameterList);
                    placeholderRID = new intParameter("@placeholderRID", base.inputParameterList);
                    getAssortmentProperties = new intParameter("@getAssortmentProperties", base.inputParameterList);
                    getPlaceholderData = new intParameter("@getPlaceholderData", base.inputParameterList);
			    }
			
			    public DataSet ReadAsDataSet(DatabaseAccess _dba,
                                             int? headerRID,
                                             int? placeholderRID,
                                             int? getAssortmentProperties,
                                             int? getPlaceholderData
			                                 )
			    {
                    lock (typeof(MID_GET_ASSORTMENT_DATA_FOR_HEADER_def))
                    {
                        this.headerRID.SetValue(headerRID);
                        this.placeholderRID.SetValue(placeholderRID);
                        this.getAssortmentProperties.SetValue(getAssortmentProperties);
                        this.getPlaceholderData.SetValue(getPlaceholderData);
                        DataSet dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsValues.Tables[0].TableName = "AssortmentProperties";
                        dsValues.Tables[1].TableName = "AssortmentPropertiesForPlaceHolder";
                        dsValues.Tables[2].TableName = "HeadersAttachedToPlaceholder";
                        return dsValues;
                    }
			    }
			}

            public static SP_MID_HDRPACK_ALLOCATION_DELETE_def SP_MID_HDRPACK_ALLOCATION_DELETE = new SP_MID_HDRPACK_ALLOCATION_DELETE_def();
            public class SP_MID_HDRPACK_ALLOCATION_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HDRPACK_ALLOCATION_DELETE.SQL"

			    private intParameter PACK_RID;
			
			    public SP_MID_HDRPACK_ALLOCATION_DELETE_def()
			    {
                    base.procedureName = "SP_MID_HDRPACK_ALLOCATION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("HDRPACK_ALLOCATION");
			        PACK_RID = new intParameter("@PACK_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? PACK_RID)
			    {
                    lock (typeof(SP_MID_HDRPACK_ALLOCATION_DELETE_def))
                    {
                        this.PACK_RID.SetValue(PACK_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_READ_ALL_def MID_ASSORTMENT_PROPERTIES_READ_ALL = new MID_ASSORTMENT_PROPERTIES_READ_ALL_def();
			public class MID_ASSORTMENT_PROPERTIES_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_READ_ALL.SQL"

			
			    public MID_ASSORTMENT_PROPERTIES_READ_ALL_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_READ_RID_FROM_PLACEHOLDER_def MID_HEADER_READ_RID_FROM_PLACEHOLDER = new MID_HEADER_READ_RID_FROM_PLACEHOLDER_def();
			public class MID_HEADER_READ_RID_FROM_PLACEHOLDER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_RID_FROM_PLACEHOLDER.SQL"

			    private intParameter PLACEHOLDER_RID;
			
			    public MID_HEADER_READ_RID_FROM_PLACEHOLDER_def()
			    {
			        base.procedureName = "MID_HEADER_READ_RID_FROM_PLACEHOLDER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER");
			        PLACEHOLDER_RID = new intParameter("@PLACEHOLDER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? PLACEHOLDER_RID)
			    {
                    lock (typeof(MID_HEADER_READ_RID_FROM_PLACEHOLDER_def))
                    {
                        this.PLACEHOLDER_RID.SetValue(PLACEHOLDER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_UPDATE_ID_def MID_HEADER_UPDATE_ID = new MID_HEADER_UPDATE_ID_def();
			public class MID_HEADER_UPDATE_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_UPDATE_ID.SQL"

			    private intParameter HDR_RID;
			    private stringParameter HDR_ID;
			
			    public MID_HEADER_UPDATE_ID_def()
			    {
			        base.procedureName = "MID_HEADER_UPDATE_ID";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("HEADER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HDR_ID = new stringParameter("@HDR_ID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      string HDR_ID
			                      )
			    {
                    lock (typeof(MID_HEADER_UPDATE_ID_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_ID.SetValue(HDR_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ_def MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ = new MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ_def();
			public class MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_STORE_GRADE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_BASIS_READ_def MID_ASSORTMENT_PROPERTIES_BASIS_READ = new MID_ASSORTMENT_PROPERTIES_BASIS_READ_def();
			public class MID_ASSORTMENT_PROPERTIES_BASIS_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_BASIS_READ.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_PROPERTIES_BASIS_READ_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_BASIS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_BASIS");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_BASIS_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_ASSORTMENT_USER_VIEW_READ_def MID_ASSORTMENT_USER_VIEW_READ = new MID_ASSORTMENT_USER_VIEW_READ_def();
            public class MID_ASSORTMENT_USER_VIEW_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_USER_VIEW_READ.SQL"

                private intParameter HDR_RID;
                private intParameter USER_RID;

                public MID_ASSORTMENT_USER_VIEW_READ_def()
                {
                    base.procedureName = "MID_ASSORTMENT_USER_VIEW_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("UDF_ASRT_GET_USER_VIEW");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HDR_RID, int? USER_RID)
                {
                    lock (typeof(MID_ASSORTMENT_USER_VIEW_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_ASSORTMENT_USER_VIEW_UPSERT_def MID_ASSORTMENT_USER_VIEW_UPSERT = new MID_ASSORTMENT_USER_VIEW_UPSERT_def();
            public class MID_ASSORTMENT_USER_VIEW_UPSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_USER_VIEW_UPSERT.SQL"

                private intParameter HDR_RID;
                private intParameter USER_RID;
                private intParameter VIEW_RID;

                public MID_ASSORTMENT_USER_VIEW_UPSERT_def()
                {
                    base.procedureName = "MID_ASSORTMENT_USER_VIEW_UPSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("ASSORTMENT_USER_VIEW_JOIN");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, int? HDR_RID, int? USER_RID, int? VIEW_RID)
                {
                    lock (typeof(MID_ASSORTMENT_USER_VIEW_UPSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_ASSORTMENT_COUNT_def MID_HEADER_READ_ASSORTMENT_COUNT = new MID_HEADER_READ_ASSORTMENT_COUNT_def();
            public class MID_HEADER_READ_ASSORTMENT_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ASSORTMENT_COUNT.SQL"

                private intParameter ASRT_RID;

                public MID_HEADER_READ_ASSORTMENT_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_READ_ASSORTMENT_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER");
                    ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? ASRT_RID)
                {
                    lock (typeof(MID_HEADER_READ_ASSORTMENT_COUNT_def))
                    {
                        this.ASRT_RID.SetValue(ASRT_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_PLACEHOLDER_COUNT_def MID_HEADER_READ_PLACEHOLDER_COUNT = new MID_HEADER_READ_PLACEHOLDER_COUNT_def();
            public class MID_HEADER_READ_PLACEHOLDER_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_PLACEHOLDER_COUNT.SQL"

                private intParameter PLACEHOLDER_RID;

                public MID_HEADER_READ_PLACEHOLDER_COUNT_def()
                {
                    base.procedureName = "MID_HEADER_READ_PLACEHOLDER_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HEADER");
                    PLACEHOLDER_RID = new intParameter("@PLACEHOLDER_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? PLACEHOLDER_RID)
                {
                    lock (typeof(MID_HEADER_READ_PLACEHOLDER_COUNT_def))
                    {
                        this.PLACEHOLDER_RID.SetValue(PLACEHOLDER_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION_def MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION = new MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION_def();
            public class MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION.SQL"

                public MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION_def()
                {
                    base.procedureName = "MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_GET_HEADERS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge


			public static MID_ASSORTMENT_STORE_SUMMARY_INSERT_def MID_ASSORTMENT_STORE_SUMMARY_INSERT = new MID_ASSORTMENT_STORE_SUMMARY_INSERT_def();
			public class MID_ASSORTMENT_STORE_SUMMARY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_SUMMARY_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter ST_RID;
			    private intParameter VARIABLE_NUMBER;
			    private intParameter UNITS;
			    private intParameter INTRANSIT;
			    private intParameter NEED;
			    private decimalParameter PCT_NEED;
			    private intParameter VARIABLE_TYPE;
                private intParameter ONHAND; //TT#1268-MD -jsobek -5.4 Merge
                private intParameter VSW_ONHAND; //TT#1268-MD -jsobek -5.4 Merge
                private intParameter PLAN_SALES_UNITS; // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                private intParameter PLAN_STOCK_UNITS; // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
			
			    public MID_ASSORTMENT_STORE_SUMMARY_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_SUMMARY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_STORE_SUMMARY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			        INTRANSIT = new intParameter("@INTRANSIT", base.inputParameterList);
			        NEED = new intParameter("@NEED", base.inputParameterList);
                    PCT_NEED = new decimalParameter("@PCT_NEED", base.inputParameterList);
			        VARIABLE_TYPE = new intParameter("@VARIABLE_TYPE", base.inputParameterList);
                    ONHAND = new intParameter("@ONHAND", base.inputParameterList); //TT#1268-MD -jsobek -5.4 Merge
                    VSW_ONHAND = new intParameter("@VSW_ONHAND", base.inputParameterList); //TT#1268-MD -jsobek -5.4 Merge
                    PLAN_SALES_UNITS = new intParameter("@PLAN_SALES_UNITS", base.inputParameterList); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    PLAN_STOCK_UNITS = new intParameter("@PLAN_STOCK_UNITS", base.inputParameterList); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? ST_RID,
			                      int? VARIABLE_NUMBER,
			                      int? UNITS,
			                      int? INTRANSIT,
			                      int? NEED,
			                      decimal? PCT_NEED,
			                      int? VARIABLE_TYPE,
                                  int? ONHAND, //TT#1268-MD -jsobek -5.4 Merge
                                  int? VSW_ONHAND, //TT#1268-MD -jsobek -5.4 Merge
                                  int? PLAN_SALES_UNITS, // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                                  int? PLAN_STOCK_UNITS // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_SUMMARY_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.UNITS.SetValue(UNITS);
                        this.INTRANSIT.SetValue(INTRANSIT);
                        this.NEED.SetValue(NEED);
                        this.PCT_NEED.SetValue(PCT_NEED);
                        this.VARIABLE_TYPE.SetValue(VARIABLE_TYPE);
                        this.ONHAND.SetValue(ONHAND); //TT#1268-MD -jsobek -5.4 Merge
                        this.VSW_ONHAND.SetValue(VSW_ONHAND); //TT#1268-MD -jsobek -5.4 Merge
                        this.PLAN_SALES_UNITS.SetValue(PLAN_SALES_UNITS); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                        this.PLAN_STOCK_UNITS.SetValue(PLAN_STOCK_UNITS); // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_BASIS_INSERT_def MID_ASSORTMENT_PROPERTIES_BASIS_INSERT = new MID_ASSORTMENT_PROPERTIES_BASIS_INSERT_def();
			public class MID_ASSORTMENT_PROPERTIES_BASIS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_BASIS_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter BASIS_SEQ;
			    private intParameter HN_RID;
			    private intParameter FV_RID;
			    private intParameter CDR_RID;
			    private floatParameter WEIGHT;
			
			    public MID_ASSORTMENT_PROPERTIES_BASIS_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_BASIS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_BASIS");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        BASIS_SEQ = new intParameter("@BASIS_SEQ", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? BASIS_SEQ,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      double? WEIGHT
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_BASIS_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.BASIS_SEQ.SetValue(BASIS_SEQ);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT_def MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT = new MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT_def();
			public class MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter STORE_GRADE_SEQ;
			    private intParameter BOUNDARY_UNITS;
			    private intParameter BOUNDARY_INDEX;
			    private stringParameter GRADE_CODE;
			
			    public MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_STORE_GRADE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        STORE_GRADE_SEQ = new intParameter("@STORE_GRADE_SEQ", base.inputParameterList);
			        BOUNDARY_UNITS = new intParameter("@BOUNDARY_UNITS", base.inputParameterList);
			        BOUNDARY_INDEX = new intParameter("@BOUNDARY_INDEX", base.inputParameterList);
			        GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? STORE_GRADE_SEQ,
			                      int? BOUNDARY_UNITS,
			                      int? BOUNDARY_INDEX,
			                      string GRADE_CODE
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.STORE_GRADE_SEQ.SetValue(STORE_GRADE_SEQ);
                        this.BOUNDARY_UNITS.SetValue(BOUNDARY_UNITS);
                        this.BOUNDARY_INDEX.SetValue(BOUNDARY_INDEX);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_INSERT_def MID_ASSORTMENT_PROPERTIES_INSERT = new MID_ASSORTMENT_PROPERTIES_INSERT_def();
			public class MID_ASSORTMENT_PROPERTIES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_INSERT.SQL"

			    private intParameter HDR_RID;
			    private floatParameter RESERVE;
			    private intParameter RESERVE_TYPE_IND;
			    private intParameter SG_RID;
			    private intParameter VARIABLE_TYPE;
			    private intParameter VARIABLE_NUMBER;
			    private charParameter INCL_ONHAND;
			    private charParameter INCL_INTRANSIT;
			    private charParameter INCL_SIMILAR_STORES;
			    private charParameter INCL_COMMITTED;
			    private intParameter AVERAGE_BY;
			    private intParameter CDR_RID;
			    private intParameter USER_RID;
			    private intParameter ANCHOR_HN_RID;
			    private datetimeParameter LAST_PROCESS_DATETIME;
                private intParameter BEGIN_DAY_CDR_RID;  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                private intParameter TARGET_REVENUE;


                public MID_ASSORTMENT_PROPERTIES_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        RESERVE = new floatParameter("@RESERVE", base.inputParameterList);
			        RESERVE_TYPE_IND = new intParameter("@RESERVE_TYPE_IND", base.inputParameterList);
			        SG_RID = new intParameter("@SG_RID", base.inputParameterList);
			        VARIABLE_TYPE = new intParameter("@VARIABLE_TYPE", base.inputParameterList);
			        VARIABLE_NUMBER = new intParameter("@VARIABLE_NUMBER", base.inputParameterList);
			        INCL_ONHAND = new charParameter("@INCL_ONHAND", base.inputParameterList);
			        INCL_INTRANSIT = new charParameter("@INCL_INTRANSIT", base.inputParameterList);
			        INCL_SIMILAR_STORES = new charParameter("@INCL_SIMILAR_STORES", base.inputParameterList);
			        INCL_COMMITTED = new charParameter("@INCL_COMMITTED", base.inputParameterList);
			        AVERAGE_BY = new intParameter("@AVERAGE_BY", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        ANCHOR_HN_RID = new intParameter("@ANCHOR_HN_RID", base.inputParameterList);
			        LAST_PROCESS_DATETIME = new datetimeParameter("@LAST_PROCESS_DATETIME", base.inputParameterList);
                    BEGIN_DAY_CDR_RID = new intParameter("@BEGIN_DAY_CDR_RID", base.inputParameterList);  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                    TARGET_REVENUE = new intParameter("@TARGET_REVENUE", base.inputParameterList);

                }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      double? RESERVE,
			                      int? RESERVE_TYPE_IND,
			                      int? SG_RID,
			                      int? VARIABLE_TYPE,
			                      int? VARIABLE_NUMBER,
			                      char? INCL_ONHAND,
			                      char? INCL_INTRANSIT,
			                      char? INCL_SIMILAR_STORES,
			                      char? INCL_COMMITTED,
			                      int? AVERAGE_BY,
			                      int? CDR_RID,
			                      int? USER_RID,
			                      int? ANCHOR_HN_RID,
			                      DateTime? LAST_PROCESS_DATETIME,
                                  int? BEGIN_DAY_CDR_RID,  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                                  int? TARGET_REVENUE
                                  )
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.RESERVE.SetValue(RESERVE);
                        this.RESERVE_TYPE_IND.SetValue(RESERVE_TYPE_IND);
                        this.SG_RID.SetValue(SG_RID);
                        this.VARIABLE_TYPE.SetValue(VARIABLE_TYPE);
                        this.VARIABLE_NUMBER.SetValue(VARIABLE_NUMBER);
                        this.INCL_ONHAND.SetValue(INCL_ONHAND);
                        this.INCL_INTRANSIT.SetValue(INCL_INTRANSIT);
                        this.INCL_SIMILAR_STORES.SetValue(INCL_SIMILAR_STORES);
                        this.INCL_COMMITTED.SetValue(INCL_COMMITTED);
                        this.AVERAGE_BY.SetValue(AVERAGE_BY);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.ANCHOR_HN_RID.SetValue(ANCHOR_HN_RID);
                        this.LAST_PROCESS_DATETIME.SetValue(LAST_PROCESS_DATETIME);
                        this.BEGIN_DAY_CDR_RID.SetValue(BEGIN_DAY_CDR_RID);  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                        this.TARGET_REVENUE.SetValue(TARGET_REVENUE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_SUMMARY_DELETE_def MID_ASSORTMENT_STORE_SUMMARY_DELETE = new MID_ASSORTMENT_STORE_SUMMARY_DELETE_def();
			public class MID_ASSORTMENT_STORE_SUMMARY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_SUMMARY_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_STORE_SUMMARY_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_SUMMARY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_STORE_SUMMARY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_SUMMARY_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_BASIS_DELETE_def MID_ASSORTMENT_PROPERTIES_BASIS_DELETE = new MID_ASSORTMENT_PROPERTIES_BASIS_DELETE_def();
			public class MID_ASSORTMENT_PROPERTIES_BASIS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_BASIS_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_PROPERTIES_BASIS_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_BASIS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_BASIS");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_BASIS_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE_def MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE = new MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE_def();
			public class MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES_STORE_GRADE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_STORE_GRADE_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_PROPERTIES_DELETE_def MID_ASSORTMENT_PROPERTIES_DELETE = new MID_ASSORTMENT_PROPERTIES_DELETE_def();
			public class MID_ASSORTMENT_PROPERTIES_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_PROPERTIES_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_PROPERTIES_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_PROPERTIES_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_PROPERTIES");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_PROPERTIES_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}


			public static MID_HEADER_READ_ASSORTMENT_def MID_HEADER_READ_ASSORTMENT = new MID_HEADER_READ_ASSORTMENT_def();
			public class MID_HEADER_READ_ASSORTMENT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ASSORTMENT.SQL"

			    private intParameter ASRT_RID;
			
			    public MID_HEADER_READ_ASSORTMENT_def()
			    {
			        base.procedureName = "MID_HEADER_READ_ASSORTMENT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER");
			        ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? ASRT_RID)
			    {
                    lock (typeof(MID_HEADER_READ_ASSORTMENT_def))
                    {
                        this.ASRT_RID.SetValue(ASRT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE_def MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE = new MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE_def();
			public class MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE_def()
			    {
			        base.procedureName = "MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("HEADER_BULK_COLOR");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE_def MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE = new MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE_def();
			public class MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE_def()
			    {
			        base.procedureName = "MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("HEADER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_HEADER_READ_ALL_ASSORTMENT_def MID_HEADER_READ_ALL_ASSORTMENT = new MID_HEADER_READ_ALL_ASSORTMENT_def();
			public class MID_HEADER_READ_ALL_ASSORTMENT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_READ_ALL_ASSORTMENT.SQL"

			    private intParameter ASRT_RID;
			
			    public MID_HEADER_READ_ALL_ASSORTMENT_def()
			    {
			        base.procedureName = "MID_HEADER_READ_ALL_ASSORTMENT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER");
			        ASRT_RID = new intParameter("@ASRT_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? ASRT_RID)
			    {
                    lock (typeof(MID_HEADER_READ_ALL_ASSORTMENT_def))
                    {
                        this.ASRT_RID.SetValue(ASRT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_SUMMARY_READ_def MID_ASSORTMENT_STORE_SUMMARY_READ = new MID_ASSORTMENT_STORE_SUMMARY_READ_def();
			public class MID_ASSORTMENT_STORE_SUMMARY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_SUMMARY_READ.SQL"

			
			    public MID_ASSORTMENT_STORE_SUMMARY_READ_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_SUMMARY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_STORE_SUMMARY");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_SUMMARY_READ_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_SUMMARY_READ_ALL_def MID_ASSORTMENT_STORE_SUMMARY_READ_ALL = new MID_ASSORTMENT_STORE_SUMMARY_READ_ALL_def();
			public class MID_ASSORTMENT_STORE_SUMMARY_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_SUMMARY_READ_ALL.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_STORE_SUMMARY_READ_ALL_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_SUMMARY_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_STORE_SUMMARY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_SUMMARY_READ_ALL_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_VSW_REVERSE_ON_HAND_READ_def MID_VSW_REVERSE_ON_HAND_READ = new MID_VSW_REVERSE_ON_HAND_READ_def();
			public class MID_VSW_REVERSE_ON_HAND_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_REVERSE_ON_HAND_READ.SQL"

			    private intParameter HDR_RID;
			
			    public MID_VSW_REVERSE_ON_HAND_READ_def()
			    {
			        base.procedureName = "MID_VSW_REVERSE_ON_HAND_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("VSW_REVERSE_ON_HAND");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_VSW_REVERSE_ON_HAND_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_VSW_REVERSE_ON_HAND_UPSERT_def MID_VSW_REVERSE_ON_HAND_UPSERT = new MID_VSW_REVERSE_ON_HAND_UPSERT_def();
            public class MID_VSW_REVERSE_ON_HAND_UPSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_REVERSE_ON_HAND_UPSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter HN_RID;
			    private intParameter ST_RID;
			    private intParameter VSW_REVERSE_ON_HAND_UNITS;
			
			    public MID_VSW_REVERSE_ON_HAND_UPSERT_def()
			    {
                    base.procedureName = "MID_VSW_REVERSE_ON_HAND_UPSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("VSW_REVERSE_ON_HAND");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        VSW_REVERSE_ON_HAND_UNITS = new intParameter("@VSW_REVERSE_ON_HAND_UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? HN_RID,
			                      int? ST_RID,
			                      int? VSW_REVERSE_ON_HAND_UNITS
			                      )
			    {
                    lock (typeof(MID_VSW_REVERSE_ON_HAND_UPSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.VSW_REVERSE_ON_HAND_UNITS.SetValue(VSW_REVERSE_ON_HAND_UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCTOTALUDT_def SP_MID_UPDATEALLOCTOTALUDT = new SP_MID_UPDATEALLOCTOTALUDT_def();
            public class SP_MID_UPDATEALLOCTOTALUDT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCTOTALUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCTOTALUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCTOTALUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TOTAL_ALLOCATION");
			        Updt = new tableParameter("@Updt", "TOTAL_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCTOTALUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCDETAILUDT_def SP_MID_UPDATEALLOCDETAILUDT = new SP_MID_UPDATEALLOCDETAILUDT_def();
            public class SP_MID_UPDATEALLOCDETAILUDT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCDETAILUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCDETAILUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCDETAILUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("DETAIL_ALLOCATION");
			        Updt = new tableParameter("@Updt", "DETAIL_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCDETAILUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCBULKUDT_def SP_MID_UPDATEALLOCBULKUDT = new SP_MID_UPDATEALLOCBULKUDT_def();
            public class SP_MID_UPDATEALLOCBULKUDT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCBULKUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCBULKUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCBULKUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_ALLOCATION");
			        Updt = new tableParameter("@Updt", "BULK_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCBULKUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCPACKUDT_def SP_MID_UPDATEALLOCPACKUDT = new SP_MID_UPDATEALLOCPACKUDT_def();
            public class SP_MID_UPDATEALLOCPACKUDT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCPACKUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCPACKUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCPACKUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("PACK_ALLOCATION");
			        Updt = new tableParameter("@Updt", "PACK_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCPACKUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCBULKCOLORUDT_def SP_MID_UPDATEALLOCBULKCOLORUDT = new SP_MID_UPDATEALLOCBULKCOLORUDT_def();
            public class SP_MID_UPDATEALLOCBULKCOLORUDT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCBULKCOLORUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCBULKCOLORUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCBULKCOLORUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_COLOR_ALLOCATION");
			        Updt = new tableParameter("@Updt", "BULK_COLOR_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCBULKCOLORUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_UPDATEALLOCBULKCOLORSIZEUDT_def SP_MID_UPDATEALLOCBULKCOLORSIZEUDT = new SP_MID_UPDATEALLOCBULKCOLORSIZEUDT_def();
            public class SP_MID_UPDATEALLOCBULKCOLORSIZEUDT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATEALLOCBULKCOLORSIZEUDT.SQL"

			    private tableParameter Updt;
			    private intParameter debug;
			    private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_UPDATEALLOCBULKCOLORSIZEUDT_def()
			    {
                    base.procedureName = "SP_MID_UPDATEALLOCBULKCOLORSIZEUDT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_COLOR_SIZE_ALLOCATION");
			        Updt = new tableParameter("@Updt", "BULK_COLOR_SIZE_ALLOCATION_TYPE", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable Updt
			                      )
			    {
                    lock (typeof(SP_MID_UPDATEALLOCBULKCOLORSIZEUDT_def))
                    {
                        this.Updt.SetValue(Updt);
                        this.debug.SetValue(0);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            // Begin TT#1966-MD - JSmith - DC Fulfillment
            public static MID_HEADER_DIST_CENTER_READ_ALL_def MID_HEADER_DIST_CENTER_READ_ALL = new MID_HEADER_DIST_CENTER_READ_ALL_def();
            public class MID_HEADER_DIST_CENTER_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_DIST_CENTER_READ_ALL.SQL"


                public MID_HEADER_DIST_CENTER_READ_ALL_def()
                {
                    base.procedureName = "MID_HEADER_DIST_CENTER_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("DIST_CENTERP");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_DIST_CENTER_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // End TT#1966-MD - JSmith - DC Fulfillment
			//INSERT NEW STORED PROCEDURES ABOVE HERE

        }


    }  
}
