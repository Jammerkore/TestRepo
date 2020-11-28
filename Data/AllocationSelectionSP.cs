using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class AllocationSelection : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_USER_ALLOCATION_READ_def MID_USER_ALLOCATION_READ = new MID_USER_ALLOCATION_READ_def();
            public class MID_USER_ALLOCATION_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_READ.SQL"

                private intParameter USER_RID;

                public MID_USER_ALLOCATION_READ_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_ALLOCATION");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_USER_ALLOCATION_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_HEADERS_READ_def MID_USER_ALLOCATION_HEADERS_READ = new MID_USER_ALLOCATION_HEADERS_READ_def();
            public class MID_USER_ALLOCATION_HEADERS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_HEADERS_READ.SQL"

                private intParameter USER_RID;

                public MID_USER_ALLOCATION_HEADERS_READ_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_HEADERS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_ALLOCATION_HEADERS");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_USER_ALLOCATION_HEADERS_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_UPDATE_def MID_USER_ALLOCATION_UPDATE = new MID_USER_ALLOCATION_UPDATE_def();
            public class MID_USER_ALLOCATION_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_UPDATE.SQL"

                private intParameter USER_RID;
                private intParameter VIEW_TYPE;
                private intParameter SG_RID;
                private intParameter FILTER_RID;
                private intParameter GROUP_BY_ID;
                private intParameter VIEW_RID;
                private charParameter INCLUDE_INELIGIBLE_STORES;
                private intParameter BEGIN_CDR_RID;
                private intParameter END_CDR_RID;
                private intParameter HN_RID;

                public MID_USER_ALLOCATION_UPDATE_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("USER_ALLOCATION");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    VIEW_TYPE = new intParameter("@VIEW_TYPE", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    INCLUDE_INELIGIBLE_STORES = new charParameter("@INCLUDE_INELIGIBLE_STORES", base.inputParameterList);
                    BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
                    END_CDR_RID = new intParameter("@END_CDR_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? USER_RID,
                                  int? VIEW_TYPE,
                                  int? SG_RID,
                                  int? FILTER_RID,
                                  int? GROUP_BY_ID,
                                  int? VIEW_RID,
                                  char? INCLUDE_INELIGIBLE_STORES,
                                  int? BEGIN_CDR_RID,
                                  int? END_CDR_RID,
                                  int? HN_RID
                                  )
                {
                    lock (typeof(MID_USER_ALLOCATION_UPDATE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_TYPE.SetValue(VIEW_TYPE);
                        this.SG_RID.SetValue(SG_RID);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.INCLUDE_INELIGIBLE_STORES.SetValue(INCLUDE_INELIGIBLE_STORES);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.END_CDR_RID.SetValue(END_CDR_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_DELETE_AND_INSERT_def MID_USER_ALLOCATION_DELETE_AND_INSERT = new MID_USER_ALLOCATION_DELETE_AND_INSERT_def();
            public class MID_USER_ALLOCATION_DELETE_AND_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_DELETE_AND_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter VIEW_TYPE;
                private intParameter SG_RID;
                private intParameter FILTER_RID;
                private intParameter GROUP_BY_ID;
                private intParameter VIEW_RID;
                private charParameter INCLUDE_INELIGIBLE_STORES;
                private intParameter BEGIN_CDR_RID;
                private intParameter END_CDR_RID;
                private intParameter HN_RID;

                public MID_USER_ALLOCATION_DELETE_AND_INSERT_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_DELETE_AND_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("USER_ALLOCATION");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    VIEW_TYPE = new intParameter("@VIEW_TYPE", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    INCLUDE_INELIGIBLE_STORES = new charParameter("@INCLUDE_INELIGIBLE_STORES", base.inputParameterList);
                    BEGIN_CDR_RID = new intParameter("@BEGIN_CDR_RID", base.inputParameterList);
                    END_CDR_RID = new intParameter("@END_CDR_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? USER_RID,
                                  int? VIEW_TYPE,
                                  int? SG_RID,
                                  int? FILTER_RID,
                                  int? GROUP_BY_ID,
                                  int? VIEW_RID,
                                  char? INCLUDE_INELIGIBLE_STORES,
                                  int? BEGIN_CDR_RID,
                                  int? END_CDR_RID,
                                  int? HN_RID
                                  )
                {
                    lock (typeof(MID_USER_ALLOCATION_DELETE_AND_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_TYPE.SetValue(VIEW_TYPE);
                        this.SG_RID.SetValue(SG_RID);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.INCLUDE_INELIGIBLE_STORES.SetValue(INCLUDE_INELIGIBLE_STORES);
                        this.BEGIN_CDR_RID.SetValue(BEGIN_CDR_RID);
                        this.END_CDR_RID.SetValue(END_CDR_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_HEADERS_INSERT_def MID_USER_ALLOCATION_HEADERS_INSERT = new MID_USER_ALLOCATION_HEADERS_INSERT_def();
            public class MID_USER_ALLOCATION_HEADERS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_HEADERS_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter HDR_RID;

                public MID_USER_ALLOCATION_HEADERS_INSERT_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_HEADERS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("USER_ALLOCATION_HEADERS");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? USER_RID,
                                  int? HDR_RID
                                  )
                {
                    lock (typeof(MID_USER_ALLOCATION_HEADERS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_BASIS_INSERT_def MID_USER_ALLOCATION_BASIS_INSERT = new MID_USER_ALLOCATION_BASIS_INSERT_def();
            public class MID_USER_ALLOCATION_BASIS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_BASIS_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter BASIS_SEQUENCE;
                private intParameter BASIS_HN_RID;
                private intParameter BASIS_PH_RID;
                private intParameter BASIS_PHL_SEQUENCE;
                private intParameter BASIS_FV_RID;
                private intParameter CDR_RID;
                private floatParameter SALES_WEIGHT;

                public MID_USER_ALLOCATION_BASIS_INSERT_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_BASIS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("USER_ALLOCATION_BASIS");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    BASIS_SEQUENCE = new intParameter("@BASIS_SEQUENCE", base.inputParameterList);
                    BASIS_HN_RID = new intParameter("@BASIS_HN_RID", base.inputParameterList);
                    BASIS_PH_RID = new intParameter("@BASIS_PH_RID", base.inputParameterList);
                    BASIS_PHL_SEQUENCE = new intParameter("@BASIS_PHL_SEQUENCE", base.inputParameterList);
                    BASIS_FV_RID = new intParameter("@BASIS_FV_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    SALES_WEIGHT = new floatParameter("@SALES_WEIGHT", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? USER_RID,
                                  int? BASIS_SEQUENCE,
                                  int? BASIS_HN_RID,
                                  int? BASIS_PH_RID,
                                  int? BASIS_PHL_SEQUENCE,
                                  int? BASIS_FV_RID,
                                  int? CDR_RID,
                                  double? SALES_WEIGHT
                                  )
                {
                    lock (typeof(MID_USER_ALLOCATION_BASIS_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.BASIS_SEQUENCE.SetValue(BASIS_SEQUENCE);
                        this.BASIS_HN_RID.SetValue(BASIS_HN_RID);
                        this.BASIS_PH_RID.SetValue(BASIS_PH_RID);
                        this.BASIS_PHL_SEQUENCE.SetValue(BASIS_PHL_SEQUENCE);
                        this.BASIS_FV_RID.SetValue(BASIS_FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.SALES_WEIGHT.SetValue(SALES_WEIGHT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_READ_CURRENT_COLUMNS_def MID_USER_ALLOCATION_READ_CURRENT_COLUMNS = new MID_USER_ALLOCATION_READ_CURRENT_COLUMNS_def();
            public class MID_USER_ALLOCATION_READ_CURRENT_COLUMNS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_READ_CURRENT_COLUMNS.SQL"

                private intParameter USER_RID;

                public MID_USER_ALLOCATION_READ_CURRENT_COLUMNS_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_READ_CURRENT_COLUMNS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_ALLOCATION");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_USER_ALLOCATION_READ_CURRENT_COLUMNS_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE_def MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE = new MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE_def();
            public class MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE.SQL"

                private intParameter HN_RID;

                public MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_ALLOCATION");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_USER_ALLOCATION_BASIS_READ_def MID_USER_ALLOCATION_BASIS_READ = new MID_USER_ALLOCATION_BASIS_READ_def();
            public class MID_USER_ALLOCATION_BASIS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ALLOCATION_BASIS_READ.SQL"

                private intParameter USER_RID;

                public MID_USER_ALLOCATION_BASIS_READ_def()
                {
                    base.procedureName = "MID_USER_ALLOCATION_BASIS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_ALLOCATION_BASIS");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_USER_ALLOCATION_BASIS_READ_def))
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
