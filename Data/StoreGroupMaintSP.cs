using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StoreGroupMaint : DataLayer
    {
        protected static class StoredProcedures
        {
            public static MID_STORE_GROUP_INSERT_def MID_STORE_GROUP_INSERT = new MID_STORE_GROUP_INSERT_def();
            public class MID_STORE_GROUP_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_INSERT.SQL"

                public stringParameter SG_ID;
                public charParameter SG_DYNAMIC_GROUP_IND;
                public intParameter USER_RID;
                public intParameter FILTER_RID;
                public intParameter SG_VERSION;
                public intParameter SG_RID; //Declare Output Parameter

                public MID_STORE_GROUP_INSERT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP");
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                    SG_DYNAMIC_GROUP_IND = new charParameter("@SG_DYNAMIC_GROUP_IND", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              string SG_ID,
                                              char? SG_DYNAMIC_GROUP_IND,
                                              int? USER_RID,
                                              int? FILTER_RID,
                                              int? SG_VERSION
                                              )
                {
                    this.SG_ID.SetValue(SG_ID);
                    this.SG_DYNAMIC_GROUP_IND.SetValue(SG_DYNAMIC_GROUP_IND);
                    this.USER_RID.SetValue(USER_RID);
                    this.FILTER_RID.SetValue(FILTER_RID);
                    this.SG_VERSION.SetValue(SG_VERSION);
                    this.SG_RID.SetValue(null); //Initialize Output Parameter
                    return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                }
            }

            public static MID_STORE_GROUP_READ_FOR_DUPLICATE_def MID_STORE_GROUP_READ_FOR_DUPLICATE = new MID_STORE_GROUP_READ_FOR_DUPLICATE_def();
            public class MID_STORE_GROUP_READ_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_FOR_DUPLICATE.SQL"

                private stringParameter SG_ID;
                private intParameter USER_RID;

                public MID_STORE_GROUP_READ_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string SG_ID,
                                      int? USER_RID
                                      )
                {
                    lock (typeof(MID_STORE_GROUP_READ_FOR_DUPLICATE_def))
                    {
                        this.SG_ID.SetValue(SG_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            public static MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP_def MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP = new MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP_def();
            public class MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP.SQL"

                private stringParameter SG_ID;
                private intParameter USER_RID;
                private intParameter SCG_RID;

                public MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string SG_ID,
                                      int? USER_RID,
                                      int? SCG_RID
                                      )
                {
                    lock (typeof(MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP_def))
                    {
                        this.SG_ID.SetValue(SG_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_READ_FOR_RENAME_def MID_STORE_GROUP_READ_FOR_RENAME = new MID_STORE_GROUP_READ_FOR_RENAME_def();
            public class MID_STORE_GROUP_READ_FOR_RENAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_FOR_RENAME.SQL"

                private stringParameter SG_ID;
                private intParameter USER_RID;
                private intParameter SCG_RID;

                public MID_STORE_GROUP_READ_FOR_RENAME_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_FOR_RENAME";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string SG_ID,
                                      int? USER_RID,
                                      int? SCG_RID
                                      )
                {
                    lock (typeof(MID_STORE_GROUP_READ_FOR_RENAME_def))
                    {
                        this.SG_ID.SetValue(SG_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static MID_STORE_GROUP_READ_ALL_SORTED_BY_RID_def MID_STORE_GROUP_READ_ALL_SORTED_BY_RID = new MID_STORE_GROUP_READ_ALL_SORTED_BY_RID_def();
            public class MID_STORE_GROUP_READ_ALL_SORTED_BY_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_ALL_SORTED_BY_RID.SQL"

                private charParameter LOAD_INACTIVE_GROUPS_IND;   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

                public MID_STORE_GROUP_READ_ALL_SORTED_BY_RID_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_ALL_SORTED_BY_RID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    LOAD_INACTIVE_GROUPS_IND = new charParameter("@LOAD_INACTIVE_GROUPS_IND", base.inputParameterList);   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                }

                public DataTable Read(DatabaseAccess _dba,
                    char LOAD_INACTIVE_GROUPS_IND   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                    )
                {
                    lock (typeof(MID_STORE_GROUP_READ_ALL_SORTED_BY_RID_def))
                    {
                        this.LOAD_INACTIVE_GROUPS_IND.SetValue(LOAD_INACTIVE_GROUPS_IND);   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_READ_ALL_SORTED_BY_ID_def MID_STORE_GROUP_READ_ALL_SORTED_BY_ID = new MID_STORE_GROUP_READ_ALL_SORTED_BY_ID_def();
            public class MID_STORE_GROUP_READ_ALL_SORTED_BY_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_ALL_SORTED_BY_ID.SQL"

                private charParameter LOAD_INACTIVE_GROUPS_IND;   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

                public MID_STORE_GROUP_READ_ALL_SORTED_BY_ID_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_ALL_SORTED_BY_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    LOAD_INACTIVE_GROUPS_IND = new charParameter("@LOAD_INACTIVE_GROUPS_IND", base.inputParameterList);   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                }

                public DataTable Read(DatabaseAccess _dba,
                    char LOAD_INACTIVE_GROUPS_IND   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                    )
                {
                    lock (typeof(MID_STORE_GROUP_READ_ALL_SORTED_BY_ID_def))
                    {
                        this.LOAD_INACTIVE_GROUPS_IND.SetValue(LOAD_INACTIVE_GROUPS_IND);   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_COUNT_def MID_STORE_GROUP_COUNT = new MID_STORE_GROUP_COUNT_def();
            public class MID_STORE_GROUP_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_COUNT.SQL"

                private charParameter LOAD_INACTIVE_GROUPS_IND;   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

                public MID_STORE_GROUP_COUNT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("STORE_GROUP");
                    LOAD_INACTIVE_GROUPS_IND = new charParameter("@LOAD_INACTIVE_GROUPS_IND", base.inputParameterList);  
                }

                public int ReadRecordCount(DatabaseAccess _dba,
                    char LOAD_INACTIVE_GROUPS_IND   
                    )
                {
                    lock (typeof(MID_STORE_GROUP_COUNT_def))
                    {
                        this.LOAD_INACTIVE_GROUPS_IND.SetValue(LOAD_INACTIVE_GROUPS_IND);   // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_READ_ALL_def MID_STORE_GROUP_LEVEL_READ_ALL = new MID_STORE_GROUP_LEVEL_READ_ALL_def();
            public class MID_STORE_GROUP_LEVEL_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_READ_ALL.SQL"


                public MID_STORE_GROUP_LEVEL_READ_ALL_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP_LEVEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL_def MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL = new MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL_def();
            public class MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL.SQL"


                public MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP_LEVEL_RESULTS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_RESULTS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP_def MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP = new MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP_def();
            public class MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP.SQL"

                private intParameter SG_RID;

                public MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP_LEVEL_RESULTS");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SG_RID)
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_GROUP_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION_def MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION = new MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION_def();
            public class MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION.SQL"

                private intParameter SG_RID;
                private intParameter SG_VERSION;

                public MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP_LEVEL_RESULTS");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SG_RID, int? SG_VERSION)
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_RESULTS_READ_FOR_VERSION_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        this.SG_VERSION.SetValue(SG_VERSION);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_UPDATE_ID_AND_USER_def MID_STORE_GROUP_UPDATE_ID_AND_USER = new MID_STORE_GROUP_UPDATE_ID_AND_USER_def();
            public class MID_STORE_GROUP_UPDATE_ID_AND_USER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_UPDATE_ID_AND_USER.SQL"

                private intParameter SG_RID;
                private stringParameter SG_ID;
                private intParameter USER_RID;

                public MID_STORE_GROUP_UPDATE_ID_AND_USER_def()
                {
                    base.procedureName = "MID_STORE_GROUP_UPDATE_ID_AND_USER";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SG_RID,
                                  string SG_ID,
                                  int? USER_RID
                                  )
                {
                    lock (typeof(MID_STORE_GROUP_UPDATE_ID_AND_USER_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        this.SG_ID.SetValue(SG_ID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

          
            

            public static MID_STORE_GROUP_UPDATE_SET_INACTIVE_def MID_STORE_GROUP_UPDATE_SET_INACTIVE = new MID_STORE_GROUP_UPDATE_SET_INACTIVE_def();
            public class MID_STORE_GROUP_UPDATE_SET_INACTIVE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_UPDATE_SET_INACTIVE.SQL"

                private intParameter SG_RID;

                public MID_STORE_GROUP_UPDATE_SET_INACTIVE_def()
                {
                    base.procedureName = "MID_STORE_GROUP_UPDATE_SET_INACTIVE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP");
                    base.tableNames.Add("USER_PLAN");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, int? SG_RID)
                {
                    lock (typeof(MID_STORE_GROUP_UPDATE_SET_INACTIVE_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
            public static MID_STORE_GROUP_DELETE_def MID_STORE_GROUP_DELETE = new MID_STORE_GROUP_DELETE_def();
            public class MID_STORE_GROUP_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_DELETE.SQL"

                private intParameter SG_RID;

                public MID_STORE_GROUP_DELETE_def()
                {
                    base.procedureName = "MID_STORE_GROUP_DELETE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SG_RID)
                {
                    lock (typeof(MID_STORE_GROUP_DELETE_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

            public static MID_STORE_GROUP_READ_SHARED_def MID_STORE_GROUP_READ_SHARED = new MID_STORE_GROUP_READ_SHARED_def();
            public class MID_STORE_GROUP_READ_SHARED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_READ_SHARED.SQL"

                private intParameter USER_RID;

                public MID_STORE_GROUP_READ_SHARED_def()
                {
                    base.procedureName = "MID_STORE_GROUP_READ_SHARED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GROUP");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_STORE_GROUP_READ_SHARED_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_UPDATE_ID_def MID_STORE_GROUP_UPDATE_ID = new MID_STORE_GROUP_UPDATE_ID_def();
            public class MID_STORE_GROUP_UPDATE_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_UPDATE_ID.SQL"

                private intParameter SG_RID;
                private stringParameter SG_ID;

                public MID_STORE_GROUP_UPDATE_ID_def()
                {
                    base.procedureName = "MID_STORE_GROUP_UPDATE_ID";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_ID = new stringParameter("@SG_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SG_RID,
                                  string SG_ID
                                  )
                {
                    lock (typeof(MID_STORE_GROUP_UPDATE_ID_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        this.SG_ID.SetValue(SG_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_UPDATE_ID_def MID_STORE_GROUP_LEVEL_UPDATE_ID = new MID_STORE_GROUP_LEVEL_UPDATE_ID_def();
            public class MID_STORE_GROUP_LEVEL_UPDATE_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_UPDATE_ID.SQL"

                private intParameter SGL_RID;
                private intParameter SGL_VERSION;
                private stringParameter SGL_ID;
                private charParameter UPDATE_STORE_GROUP_LEVEL_IND;  // TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.

                public MID_STORE_GROUP_LEVEL_UPDATE_ID_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_UPDATE_ID";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP_LEVEL");
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    SGL_VERSION = new intParameter("@SGL_VERSION", base.inputParameterList);
                    SGL_ID = new stringParameter("@SGL_ID", base.inputParameterList);
                    UPDATE_STORE_GROUP_LEVEL_IND = new charParameter("@UPDATE_STORE_GROUP_LEVEL_IND", base.inputParameterList);  // TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                }

                public int Update(DatabaseAccess _dba,
                                  int? SGL_RID,
                                  int? SGL_VERSION,
                                  string SGL_ID,
                                  char UPDATE_STORE_GROUP_LEVEL_IND  // TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                                  )
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_UPDATE_ID_def))
                    {
                        this.SGL_RID.SetValue(SGL_RID);
                        this.SGL_VERSION.SetValue(SGL_VERSION);
                        this.SGL_ID.SetValue(SGL_ID);
                        this.UPDATE_STORE_GROUP_LEVEL_IND.SetValue(UPDATE_STORE_GROUP_LEVEL_IND);  // TT#1870-MD - JSmith - Edit Store Characteristic Value used in a Rule Method.  After the change the Store Attribute Set in the method is BLANK.  It should reflect the changed Value.
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_FILTER_CONDITION_UPDATE_LEVEL_NAME_def MID_FILTER_CONDITION_UPDATE_LEVEL_NAME = new MID_FILTER_CONDITION_UPDATE_LEVEL_NAME_def();
            public class MID_FILTER_CONDITION_UPDATE_LEVEL_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_UPDATE_LEVEL_NAME.SQL"

                private intParameter FILTER_RID;
                private stringParameter OLD_NAME;
                private stringParameter NEW_NAME;

                public MID_FILTER_CONDITION_UPDATE_LEVEL_NAME_def()
                {
                    base.procedureName = "MID_FILTER_CONDITION_UPDATE_LEVEL_NAME";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("FILTER_CONDITION");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    OLD_NAME = new stringParameter("@OLD_NAME", base.inputParameterList);
                    NEW_NAME = new stringParameter("@NEW_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? FILTER_RID,
                                  string OLD_NAME,
                                  string NEW_NAME
                                  )
                {
                    lock (typeof(MID_FILTER_CONDITION_UPDATE_LEVEL_NAME_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.OLD_NAME.SetValue(OLD_NAME);
                        this.NEW_NAME.SetValue(NEW_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_FILTER_UPDATE_NAME_FOR_SET_def MID_FILTER_UPDATE_NAME_FOR_SET = new MID_FILTER_UPDATE_NAME_FOR_SET_def();
            public class MID_FILTER_UPDATE_NAME_FOR_SET_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_UPDATE_NAME_FOR_SET.SQL"

                private intParameter FILTER_RID;
                private stringParameter NEW_NAME;

                public MID_FILTER_UPDATE_NAME_FOR_SET_def()
                {
                    base.procedureName = "MID_FILTER_UPDATE_NAME_FOR_SET";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("FILTER");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    NEW_NAME = new stringParameter("@NEW_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? FILTER_RID,
                                  string NEW_NAME
                                  )
                {
                    lock (typeof(MID_FILTER_UPDATE_NAME_FOR_SET_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.NEW_NAME.SetValue(NEW_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE_def MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE = new MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE_def();
            public class MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE.SQL"

                private intParameter SGL_RID;
                private intParameter SGL_VERSION;
                private intParameter SGL_SEQUENCE;

                public MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GROUP_LEVEL");
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    SGL_VERSION = new intParameter("@SGL_VERSION", base.inputParameterList);
                    SGL_SEQUENCE = new intParameter("@SGL_SEQUENCE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SGL_RID,
                                  int? SGL_VERSION,
                                  int? SGL_SEQUENCE
                                  )
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_UPDATE_SEQUENCE_def))
                    {
                        this.SGL_RID.SetValue(SGL_RID);
                        this.SGL_VERSION.SetValue(SGL_VERSION);
                        this.SGL_SEQUENCE.SetValue(SGL_SEQUENCE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

           

            public static MID_STORE_GROUP_LEVEL_INSERT_def MID_STORE_GROUP_LEVEL_INSERT = new MID_STORE_GROUP_LEVEL_INSERT_def();
            public class MID_STORE_GROUP_LEVEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_LEVEL_INSERT.SQL"

                private intParameter SGL_SEQUENCE;
                private intParameter SG_RID;      
                private stringParameter SGL_ID;
                private intParameter IS_ACTIVE;
                private intParameter CONDITION_RID;
                private intParameter LEVEL_TYPE;
                private intParameter SGL_RID; //Declare Output Parameter

                public MID_STORE_GROUP_LEVEL_INSERT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_LEVEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP_LEVEL");
                    SGL_SEQUENCE = new intParameter("@SGL_SEQUENCE", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList); 
                    SGL_ID = new stringParameter("@SGL_ID", base.inputParameterList);
                    IS_ACTIVE = new intParameter("@IS_ACTIVE", base.inputParameterList);
                    CONDITION_RID = new intParameter("@CONDITION_RID", base.inputParameterList);
                    LEVEL_TYPE = new intParameter("@LEVEL_TYPE", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? SG_RID,
                                              int? SGL_SEQUENCE,
                                              string SGL_ID,
                                              int? IS_ACTIVE,
                                              int? CONDITION_RID,
                                              int? LEVEL_TYPE
                                              )
                {
                    lock (typeof(MID_STORE_GROUP_LEVEL_INSERT_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        this.SGL_SEQUENCE.SetValue(SGL_SEQUENCE);
                        this.SGL_ID.SetValue(SGL_ID);
                        this.IS_ACTIVE.SetValue(IS_ACTIVE);
                        this.CONDITION_RID.SetValue(CONDITION_RID);
                        this.LEVEL_TYPE.SetValue(LEVEL_TYPE);
                        this.SGL_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_JOIN_INSERT_def MID_STORE_GROUP_JOIN_INSERT = new MID_STORE_GROUP_JOIN_INSERT_def();
            public class MID_STORE_GROUP_JOIN_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_JOIN_INSERT.SQL"

                private intParameter SG_RID;
                private intParameter SG_VERSION;
                private intParameter SGL_RID;
                private intParameter SGL_VERSION;
                private intParameter STORE_COUNT;
                private stringParameter SGL_OVERRIDE_ID;
                private intParameter SGL_OVERRIDE_SEQUENCE;
                private intParameter SGJ_RID; //Declare Output Parameter

                public MID_STORE_GROUP_JOIN_INSERT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_JOIN_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP_JOIN");
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    SGL_VERSION = new intParameter("@SGL_VERSION", base.inputParameterList);
                    STORE_COUNT = new intParameter("@STORE_COUNT", base.inputParameterList);
                    SGL_OVERRIDE_ID = new stringParameter("@SGL_OVERRIDE_ID", base.inputParameterList);
                    SGL_OVERRIDE_SEQUENCE = new intParameter("@SGL_OVERRIDE_SEQUENCE", base.inputParameterList);
                    SGJ_RID = new intParameter("@SGJ_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? SG_RID,
                                              int? SG_VERSION,
                                              int? SGL_RID,
                                              int? SGL_VERSION,
                                              int? STORE_COUNT,
                                              string SGL_OVERRIDE_ID,
                                              int? SGL_OVERRIDE_SEQUENCE
                                              )
                {
                    lock (typeof(MID_STORE_GROUP_JOIN_INSERT_def))
                    {
                        this.SG_RID.SetValue(SG_RID);
                        this.SG_VERSION.SetValue(SG_VERSION);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.SGL_VERSION.SetValue(SGL_VERSION);
                        this.STORE_COUNT.SetValue(STORE_COUNT);
                        this.SGL_OVERRIDE_ID.SetValue(SGL_OVERRIDE_ID);
                        this.SGL_OVERRIDE_SEQUENCE.SetValue(SGL_OVERRIDE_SEQUENCE);
                        this.SGJ_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }          
            }


			// Begin TT#1517-MD - stodd - new sets not getting added to database
            public static MID_STORE_GROUP_JOIN_FIRST_TIME_INIT_def MID_STORE_GROUP_JOIN_FIRST_TIME_INIT = new MID_STORE_GROUP_JOIN_FIRST_TIME_INIT_def();
            public class MID_STORE_GROUP_JOIN_FIRST_TIME_INIT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_JOIN_FIRST_TIME_INIT.SQL"

                //private intParameter SGJ_RID; //Declare Output Parameter

                public MID_STORE_GROUP_JOIN_FIRST_TIME_INIT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_JOIN_FIRST_TIME_INIT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP_JOIN");
                   // SGJ_RID = new intParameter("@SGJ_RID", base.outputParameterList); //Add Output Parameter
                }

                public int FirstTimeInit(DatabaseAccess _dba
                                              )
                {
                    lock (typeof(MID_STORE_GROUP_JOIN_FIRST_TIME_INIT_def))
                    {
                        //this.SGJ_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }
			// End TT#1517-MD - stodd - new sets not getting added to database

            public static MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL_def MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL = new MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL_def();
            public class MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL.SQL"


                public MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL_def()
                {
                    base.procedureName = "MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_GROUP_JOIN_HISTORY");
                    base.tableNames.Add("STORE_GROUP_LEVEL_RESULTS_HISTORY");
                    base.tableNames.Add("STORE_GROUP_LEVEL");
                }

                public int Delete(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_RESULTS_INSERT_def MID_STORE_GROUP_RESULTS_INSERT = new MID_STORE_GROUP_RESULTS_INSERT_def();
            public class MID_STORE_GROUP_RESULTS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_RESULTS_INSERT.SQL"

                private intParameter SGL_RID;
                private intParameter SGL_VERSION;
                private intParameter ST_RID;
                private intParameter RESULT_RID; //Declare Output Parameter

                public MID_STORE_GROUP_RESULTS_INSERT_def()
                {
                    base.procedureName = "MID_STORE_GROUP_RESULTS_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP_RESULTS");
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    SGL_VERSION = new intParameter("@SGL_VERSION", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    RESULT_RID = new intParameter("@RESULT_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? SGL_RID,
                                              int? SGL_VERSION,
                                              int? ST_RID
                                              )
                {
                    lock (typeof(MID_STORE_GROUP_RESULTS_INSERT_def))
                    {
                        this.SGL_RID.SetValue(SGL_RID);
                        this.SGL_VERSION.SetValue(SGL_VERSION);
                        this.ST_RID.SetValue(ST_RID);
                        this.RESULT_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_STORE_GROUP_INSERT_ALL_STORES_SET_def MID_STORE_GROUP_INSERT_ALL_STORES_SET = new MID_STORE_GROUP_INSERT_ALL_STORES_SET_def();
            public class MID_STORE_GROUP_INSERT_ALL_STORES_SET_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GROUP_INSERT_ALL_STORES_SET.SQL"

                private intParameter FILTER_RID;

                public MID_STORE_GROUP_INSERT_ALL_STORES_SET_def()
                {
                    base.procedureName = "MID_STORE_GROUP_INSERT_ALL_STORES_SET";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_GROUP");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
            
                }

                public int Insert(DatabaseAccess _dba, int? FILTER_RID)
                {
                    lock (typeof(MID_STORE_GROUP_INSERT_ALL_STORES_SET_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
