using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ColorData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_COLOR_CODE_READ_ALL_def MID_COLOR_CODE_READ_ALL = new MID_COLOR_CODE_READ_ALL_def();
            public class MID_COLOR_CODE_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_READ_ALL.SQL"

                public MID_COLOR_CODE_READ_ALL_def()
                {
                    base.procedureName = "MID_COLOR_CODE_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_CODE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_COLOR_CODE_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_COLOR_INSERT_def SP_MID_COLOR_INSERT = new SP_MID_COLOR_INSERT_def();
            public class SP_MID_COLOR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_COLOR_INSERT.SQL"

                private stringParameter COLOR_CODE_ID;
                private stringParameter COLOR_CODE_NAME;
                private stringParameter COLOR_CODE_GROUP;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;
                private intParameter COLOR_CODE_RID; //Declare Output Parameter

                public SP_MID_COLOR_INSERT_def()
                {
                    base.procedureName = "SP_MID_COLOR_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_ID = new stringParameter("@COLOR_CODE_ID", base.inputParameterList);
                    COLOR_CODE_NAME = new stringParameter("@COLOR_CODE_NAME", base.inputParameterList);
                    COLOR_CODE_GROUP = new stringParameter("@COLOR_CODE_GROUP", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);

                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string COLOR_CODE_ID,
                                              string COLOR_CODE_NAME,
                                              string COLOR_CODE_GROUP,
                                              char? VIRTUAL_IND,
                                              int? PURPOSE
                                              )
                {
                    lock (typeof(SP_MID_COLOR_INSERT_def))
                    {
                        this.COLOR_CODE_ID.SetValue(COLOR_CODE_ID);
                        this.COLOR_CODE_NAME.SetValue(COLOR_CODE_NAME);
                        this.COLOR_CODE_GROUP.SetValue(COLOR_CODE_GROUP);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        this.COLOR_CODE_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_UPDATE_def MID_COLOR_CODE_UPDATE = new MID_COLOR_CODE_UPDATE_def();
            public class MID_COLOR_CODE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_UPDATE.SQL"

                private intParameter COLOR_CODE_RID;
                private stringParameter COLOR_CODE_ID;
                private stringParameter COLOR_CODE_NAME;
                private stringParameter COLOR_CODE_GROUP;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;

                public MID_COLOR_CODE_UPDATE_def()
                {
                    base.procedureName = "MID_COLOR_CODE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    COLOR_CODE_ID = new stringParameter("@COLOR_CODE_ID", base.inputParameterList);
                    COLOR_CODE_NAME = new stringParameter("@COLOR_CODE_NAME", base.inputParameterList);
                    COLOR_CODE_GROUP = new stringParameter("@COLOR_CODE_GROUP", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? COLOR_CODE_RID,
                                  string COLOR_CODE_ID,
                                  string COLOR_CODE_NAME,
                                  string COLOR_CODE_GROUP,
                                  char? VIRTUAL_IND,
                                  int? PURPOSE
                                  )
                {
                    lock (typeof(MID_COLOR_CODE_UPDATE_def))
                    {
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.COLOR_CODE_ID.SetValue(COLOR_CODE_ID);
                        this.COLOR_CODE_NAME.SetValue(COLOR_CODE_NAME);
                        this.COLOR_CODE_GROUP.SetValue(COLOR_CODE_GROUP);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_DELETE_def MID_COLOR_CODE_DELETE = new MID_COLOR_CODE_DELETE_def();
            public class MID_COLOR_CODE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_DELETE.SQL"

                private intParameter COLOR_CODE_RID;

                public MID_COLOR_CODE_DELETE_def()
                {
                    base.procedureName = "MID_COLOR_CODE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? COLOR_CODE_RID)
                {
                    lock (typeof(MID_COLOR_CODE_DELETE_def))
                    {
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_ID_EXISTS_def MID_COLOR_CODE_ID_EXISTS = new MID_COLOR_CODE_ID_EXISTS_def();
            public class MID_COLOR_CODE_ID_EXISTS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_ID_EXISTS.SQL"

                private stringParameter COLOR_CODE_ID;

                public MID_COLOR_CODE_ID_EXISTS_def()
                {
                    base.procedureName = "MID_COLOR_CODE_ID_EXISTS";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_ID = new stringParameter("@COLOR_CODE_ID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, string COLOR_CODE_ID)
                {
                    lock (typeof(MID_COLOR_CODE_ID_EXISTS_def))
                    {
                        this.COLOR_CODE_ID.SetValue(COLOR_CODE_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_READ_RID_FROM_ID_def MID_COLOR_CODE_READ_RID_FROM_ID = new MID_COLOR_CODE_READ_RID_FROM_ID_def();
            public class MID_COLOR_CODE_READ_RID_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_READ_RID_FROM_ID.SQL"

                private stringParameter COLOR_CODE_ID;

                public MID_COLOR_CODE_READ_RID_FROM_ID_def()
                {
                    base.procedureName = "MID_COLOR_CODE_READ_RID_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_ID = new stringParameter("@COLOR_CODE_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string COLOR_CODE_ID)
                {
                    lock (typeof(MID_COLOR_CODE_READ_RID_FROM_ID_def))
                    {
                        this.COLOR_CODE_ID.SetValue(COLOR_CODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_READ_COLOR_GROUPS_def MID_COLOR_CODE_READ_COLOR_GROUPS = new MID_COLOR_CODE_READ_COLOR_GROUPS_def();
            public class MID_COLOR_CODE_READ_COLOR_GROUPS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_READ_COLOR_GROUPS.SQL"

                public MID_COLOR_CODE_READ_COLOR_GROUPS_def()
                {
                    base.procedureName = "MID_COLOR_CODE_READ_COLOR_GROUPS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_CODE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_COLOR_CODE_READ_COLOR_GROUPS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_CODE_READ_COLORS_FROM_GROUP_def MID_COLOR_CODE_READ_COLORS_FROM_GROUP = new MID_COLOR_CODE_READ_COLORS_FROM_GROUP_def();
            public class MID_COLOR_CODE_READ_COLORS_FROM_GROUP_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_CODE_READ_COLORS_FROM_GROUP.SQL"

                private stringParameter COLOR_CODE_GROUP;

                public MID_COLOR_CODE_READ_COLORS_FROM_GROUP_def()
                {
                    base.procedureName = "MID_COLOR_CODE_READ_COLORS_FROM_GROUP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_CODE");
                    COLOR_CODE_GROUP = new stringParameter("@COLOR_CODE_GROUP", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string COLOR_CODE_GROUP)
                {
                    lock (typeof(MID_COLOR_CODE_READ_COLORS_FROM_GROUP_def))
                    {
                        this.COLOR_CODE_GROUP.SetValue(COLOR_CODE_GROUP);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
