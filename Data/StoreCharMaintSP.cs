using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StoreCharMaint : DataLayer
    {
        protected static class StoredProcedures
        {
            public static MID_STORE_CHAR_READ_FOR_GROUP_MAINT_def MID_STORE_CHAR_READ_FOR_GROUP_MAINT = new MID_STORE_CHAR_READ_FOR_GROUP_MAINT_def();
            public class MID_STORE_CHAR_READ_FOR_GROUP_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_FOR_GROUP_MAINT.SQL"

                private intParameter SCG_RID;

                public MID_STORE_CHAR_READ_FOR_GROUP_MAINT_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_FOR_GROUP_MAINT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? SCG_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_FOR_GROUP_MAINT_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_GROUP_READ_def MID_STORE_CHAR_GROUP_READ = new MID_STORE_CHAR_GROUP_READ_def();
            public class MID_STORE_CHAR_GROUP_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_READ.SQL"

                private intParameter SCG_RID;

                public MID_STORE_CHAR_GROUP_READ_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? SCG_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_GROUP_READ_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_STORECHARGROUP_INSERT_def SP_MID_STORECHARGROUP_INSERT = new SP_MID_STORECHARGROUP_INSERT_def();
            public class SP_MID_STORECHARGROUP_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORECHARGROUP_INSERT.SQL"

                private stringParameter SCG_ID;
                private intParameter SCG_TYPE;
                private charParameter SCG_LIST_IND;
                private intParameter SCG_RID; //Declare Output Parameter

                public SP_MID_STORECHARGROUP_INSERT_def()
                {
                    base.procedureName = "SP_MID_STORECHARGROUP_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    SCG_ID = new stringParameter("@SCG_ID", base.inputParameterList);
                    SCG_TYPE = new intParameter("@SCG_TYPE", base.inputParameterList);
                    SCG_LIST_IND = new charParameter("@SCG_LIST_IND", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              string SCG_ID,
                                              int? SCG_TYPE,
                                              char? SCG_LIST_IND
                                              )
                {
                    lock (typeof(SP_MID_STORECHARGROUP_INSERT_def))
                    {
                        this.SCG_ID.SetValue(SCG_ID);
                        this.SCG_TYPE.SetValue(SCG_TYPE);
                        this.SCG_LIST_IND.SetValue(SCG_LIST_IND);
                        this.SCG_RID.SetValue(0); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_GROUP_UPDATE_def MID_STORE_CHAR_GROUP_UPDATE = new MID_STORE_CHAR_GROUP_UPDATE_def();
            public class MID_STORE_CHAR_GROUP_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_UPDATE.SQL"

                private intParameter SCG_RID; 
                private stringParameter SCG_ID;
                private intParameter SCG_TYPE;
                private charParameter SCG_LIST_IND;                

                public MID_STORE_CHAR_GROUP_UPDATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SCG_ID = new stringParameter("@SCG_ID", base.inputParameterList);
                    SCG_TYPE = new intParameter("@SCG_TYPE", base.inputParameterList);
                    SCG_LIST_IND = new charParameter("@SCG_LIST_IND", base.inputParameterList);                  
                }

                public int Update(DatabaseAccess _dba,
                                              int? SCG_RID,
                                              string SCG_ID,
                                              int? SCG_TYPE,
                                              char? SCG_LIST_IND
                                              )
                {
                    lock (typeof(MID_STORE_CHAR_GROUP_UPDATE_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SCG_ID.SetValue(SCG_ID);
                        this.SCG_TYPE.SetValue(SCG_TYPE);
                        this.SCG_LIST_IND.SetValue(SCG_LIST_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_GROUP_DELETE_def MID_STORE_CHAR_GROUP_DELETE = new MID_STORE_CHAR_GROUP_DELETE_def();
            public class MID_STORE_CHAR_GROUP_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_DELETE.SQL"

                private intParameter SCG_RID;

                public MID_STORE_CHAR_GROUP_DELETE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SCG_RID)
                {
                    lock (typeof(MID_STORE_CHAR_GROUP_DELETE_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE = new MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def();
            public class MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE.SQL"

                private stringParameter PROPOSED_GROUP_NAME;
                private intParameter SCG_EDIT_RID;

                public MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    PROPOSED_GROUP_NAME = new stringParameter("@PROPOSED_GROUP_NAME", base.inputParameterList);
                    SCG_EDIT_RID = new intParameter("@SCG_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string PROPOSED_GROUP_NAME,
                                      int? SCG_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_GROUP_NAME.SetValue(PROPOSED_GROUP_NAME);
                        this.SCG_EDIT_RID.SetValue(SCG_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_GROUP_READ_FROM_ID_def MID_STORE_CHAR_GROUP_READ_FROM_ID = new MID_STORE_CHAR_GROUP_READ_FROM_ID_def();
            public class MID_STORE_CHAR_GROUP_READ_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_READ_FROM_ID.SQL"

                private stringParameter SCG_ID;                

                public MID_STORE_CHAR_GROUP_READ_FROM_ID_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_READ_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                    SCG_ID = new stringParameter("@SCG_ID", base.inputParameterList);                    
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string SCG_ID                                      
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_GROUP_READ_FROM_ID_def))
                    {
                        this.SCG_ID.SetValue(SCG_ID);                        
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            public static MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE = new MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def();
            public class MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE.SQL"

                private stringParameter PROPOSED_VALUE;
                private intParameter SCG_RID;
                private intParameter SC_EDIT_RID;

                public MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR");
                    PROPOSED_VALUE = new stringParameter("@PROPOSED_VALUE", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SC_EDIT_RID = new intParameter("@SC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string PROPOSED_VALUE,
                                      int? SCG_RID,
                                      int? SC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SC_EDIT_RID.SetValue(SC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE = new MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def();
            public class MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE.SQL"

                private datetimeParameter PROPOSED_VALUE;
                private intParameter SCG_RID;
                private intParameter SC_EDIT_RID;

                public MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR");
                    PROPOSED_VALUE = new datetimeParameter("@PROPOSED_VALUE", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SC_EDIT_RID = new intParameter("@SC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      DateTime? PROPOSED_VALUE,
                                      int? SCG_RID,
                                      int? SC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SC_EDIT_RID.SetValue(SC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE = new MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def();
            public class MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE.SQL"

                private floatParameter PROPOSED_VALUE;
                private intParameter SCG_RID;
                private intParameter SC_EDIT_RID;

                public MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR");
                    PROPOSED_VALUE = new floatParameter("@PROPOSED_VALUE", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SC_EDIT_RID = new intParameter("@SC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      float? PROPOSED_VALUE,
                                      int? SCG_RID,
                                      int? SC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SC_EDIT_RID.SetValue(SC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE = new MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def();
            public class MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE.SQL"

                private floatParameter PROPOSED_VALUE;
                private intParameter SCG_RID;
                private intParameter SC_EDIT_RID;

                public MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR");
                    PROPOSED_VALUE = new floatParameter("@PROPOSED_VALUE", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SC_EDIT_RID = new intParameter("@SC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      float? PROPOSED_VALUE,
                                      int? SCG_RID,
                                      int? SC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SC_EDIT_RID.SetValue(SC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }



            public static SP_MID_STORECHAR_INSERT_def SP_MID_STORECHAR_INSERT = new SP_MID_STORECHAR_INSERT_def();
            public class SP_MID_STORECHAR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORECHAR_INSERT.SQL"

                public intParameter SCG_RID;
                public stringParameter TEXT_VALUE;
                public datetimeParameter DATE_VALUE;
                public floatParameter NUMBER_VALUE;
                public floatParameter DOLLAR_VALUE;
                private intParameter SC_RID; //Declare Output Parameter

                public SP_MID_STORECHAR_INSERT_def()
                {
                    base.procedureName = "SP_MID_STORECHAR_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_CHAR");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
                    DATE_VALUE = new datetimeParameter("@DATE_VALUE", base.inputParameterList);
                    NUMBER_VALUE = new floatParameter("@NUMBER_VALUE", base.inputParameterList);
                    DOLLAR_VALUE = new floatParameter("@DOLLAR_VALUE", base.inputParameterList);
                  

                    SC_RID = new intParameter("@SC_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                    int? SCG_RID,
                                    string TEXT_VALUE,
                                    DateTime? DATE_VALUE,
                                    float? NUMBER_VALUE,
                                    float? DOLLAR_VALUE 
                                    )
                {
                    lock (typeof(SP_MID_STORECHAR_INSERT_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.DATE_VALUE.SetValue(DATE_VALUE);
                        this.NUMBER_VALUE.SetValue(NUMBER_VALUE);
                        this.DOLLAR_VALUE.SetValue(DOLLAR_VALUE);
                        this.SC_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_UPDATE_def MID_STORE_CHAR_UPDATE = new MID_STORE_CHAR_UPDATE_def();
            public class MID_STORE_CHAR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_UPDATE.SQL"

                public intParameter SC_RID;
                public stringParameter TEXT_VALUE;
                public datetimeParameter DATE_VALUE;
                public floatParameter NUMBER_VALUE;
                public floatParameter DOLLAR_VALUE;

                public MID_STORE_CHAR_UPDATE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_CHAR");
                    SC_RID = new intParameter("@SC_RID", base.inputParameterList);
                    TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
                    DATE_VALUE = new datetimeParameter("@DATE_VALUE", base.inputParameterList);
                    NUMBER_VALUE = new floatParameter("@NUMBER_VALUE", base.inputParameterList);
                    DOLLAR_VALUE = new floatParameter("@DOLLAR_VALUE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                    int? SC_RID,
                                    string TEXT_VALUE,
                                    DateTime? DATE_VALUE,
                                    float? NUMBER_VALUE,
                                    float? DOLLAR_VALUE
                                    )
                {
                    lock (typeof(MID_STORE_CHAR_UPDATE_def))
                    {
                        this.SC_RID.SetValue(SC_RID);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.DATE_VALUE.SetValue(DATE_VALUE);
                        this.NUMBER_VALUE.SetValue(NUMBER_VALUE);
                        this.DOLLAR_VALUE.SetValue(DOLLAR_VALUE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_DELETE_def MID_STORE_CHAR_DELETE = new MID_STORE_CHAR_DELETE_def();
            public class MID_STORE_CHAR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_DELETE.SQL"

                private intParameter SC_RID;

                public MID_STORE_CHAR_DELETE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_CHAR");
                    SC_RID = new intParameter("@SC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SC_RID)
                {
                    lock (typeof(MID_STORE_CHAR_DELETE_def))
                    {
                        this.SC_RID.SetValue(SC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }



            public static MID_STORE_CHAR_JOIN_INSERT_def MID_STORE_CHAR_JOIN_INSERT = new MID_STORE_CHAR_JOIN_INSERT_def();
            public class MID_STORE_CHAR_JOIN_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_JOIN_INSERT.SQL"

                public intParameter SCG_RID; 
                public intParameter ST_RID;
                public intParameter SC_RID;
             

                public MID_STORE_CHAR_JOIN_INSERT_def()
                {
                    base.procedureName = "MID_STORE_CHAR_JOIN_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE_CHAR_JOIN");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    SC_RID = new intParameter("@SC_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                    int? SCG_RID, 
                                    int? ST_RID,
                                    int SC_RID
                                    )
                {
                    lock (typeof(MID_STORE_CHAR_JOIN_INSERT_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.SC_RID.SetValue(SC_RID);

                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE_def MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE = new MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE_def();
            public class MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE.SQL"

                public intParameter SCG_RID;
                public intParameter ST_RID;
             

                public MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE_def()
                {
                    base.procedureName = "MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_CHAR_JOIN");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                   
                }

                public int Delete(DatabaseAccess _dba,
                                    int? SCG_RID,
                                    int? ST_RID
                                    )
                {
                    lock (typeof(MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.ST_RID.SetValue(ST_RID);

                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
           
			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
