using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StoreMaint : DataLayer
    {
        protected static class StoredProcedures
        {
            public static MID_STORES_READ_FOR_MAINT_def MID_STORES_READ_FOR_MAINT = new MID_STORES_READ_FOR_MAINT_def();
            public class MID_STORES_READ_FOR_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_FOR_MAINT.SQL"


                public MID_STORES_READ_FOR_MAINT_def()
                {
                    base.procedureName = "MID_STORES_READ_FOR_MAINT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORES");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    return ExecuteStoredProcedureForRead(_dba);
                }
            }

            public static MID_STORES_READ_FIELDS_FOR_MAINT_def MID_STORES_READ_FIELDS_FOR_MAINT = new MID_STORES_READ_FIELDS_FOR_MAINT_def();
            public class MID_STORES_READ_FIELDS_FOR_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_FIELDS_FOR_MAINT.SQL"

                public intParameter ST_RID;

                public MID_STORES_READ_FIELDS_FOR_MAINT_def()
                {
                    base.procedureName = "MID_STORES_READ_FIELDS_FOR_MAINT";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("STORES");
                    base.tableNames.Add("STORE_CHAR");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }


                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? ST_RID)
                {
                    DataSet dsValues;
                    this.ST_RID.SetValue(ST_RID);
                    dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                    dsValues.Tables[0].TableName = "StoreFieldData";
                    dsValues.Tables[1].TableName = "StoreCharData";
                    return dsValues;
                }
            }

            public static MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL_def MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL = new MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL_def();
            public class MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL.SQL"

                private intParameter INCLUDE_INACTIVE_STORES;

                private tableParameter CHAR_GROUP_RID_LIST;

                public MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL_def()
                {
                    base.procedureName = "MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("STORES");
                    base.tableNames.Add("STORE_CHAR");
                    INCLUDE_INACTIVE_STORES = new intParameter("@INCLUDE_INACTIVE_STORES", base.inputParameterList);
                    CHAR_GROUP_RID_LIST = new tableParameter("@CHAR_GROUP_RID_LIST", "CHAR_GROUP_RID_TYPE", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba,
                                  int? INCLUDE_INACTIVE_STORES,
                                  DataTable CHAR_GROUP_RID_LIST
                                  )
                {
                    lock (typeof(MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL_def))
                    {
                        DataSet dsValues;
                        this.INCLUDE_INACTIVE_STORES.SetValue(INCLUDE_INACTIVE_STORES);
                        this.CHAR_GROUP_RID_LIST.SetValue(CHAR_GROUP_RID_LIST);
                        dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsValues.Tables[0].TableName = "StoreFieldData";
                        dsValues.Tables[1].TableName = "StoreCharData";
                        dsValues.Tables[2].TableName = "CharGroupData";
                        return dsValues;
                    }
                }
            }

            public static MID_STORES_READ_FIELDS_FOR_MAINT_ADD_def MID_STORES_READ_FIELDS_FOR_MAINT_ADD = new MID_STORES_READ_FIELDS_FOR_MAINT_ADD_def();
            public class MID_STORES_READ_FIELDS_FOR_MAINT_ADD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_FIELDS_FOR_MAINT_ADD.SQL"

                public MID_STORES_READ_FIELDS_FOR_MAINT_ADD_def()
                {
                    base.procedureName = "MID_STORES_READ_FIELDS_FOR_MAINT_ADD";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                }


                public DataSet ReadAsDataSet(DatabaseAccess _dba)
                {
                    DataSet dsValues;
                    dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                    dsValues.Tables[0].TableName = "CharData";
                    return dsValues;
                }
            }

            public static MID_STORE_CHAR_GROUP_READ_FOR_MAINT_def MID_STORE_CHAR_GROUP_READ_FOR_MAINT = new MID_STORE_CHAR_GROUP_READ_FOR_MAINT_def();
            public class MID_STORE_CHAR_GROUP_READ_FOR_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_GROUP_READ_FOR_MAINT.SQL"

                public MID_STORE_CHAR_GROUP_READ_FOR_MAINT_def()
                {
                    base.procedureName = "MID_STORE_CHAR_GROUP_READ_FOR_MAINT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHAR_GROUP");
                }


                public DataSet ReadAsDataSet(DatabaseAccess _dba)
                {
                    DataSet dsValues;
                    dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                    dsValues.Tables[0].TableName = "CharListValueData";
                    return dsValues;
                }
            }

            public static MID_STORES_READ_ID_FOR_DUPLICATE_def MID_STORES_READ_ID_FOR_DUPLICATE = new MID_STORES_READ_ID_FOR_DUPLICATE_def();
            public class MID_STORES_READ_ID_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_ID_FOR_DUPLICATE.SQL"

                private stringParameter PROPOSED_STORE_ID;
                private intParameter ST_EDIT_RID;

                public MID_STORES_READ_ID_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_STORES_READ_ID_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORES");
                    PROPOSED_STORE_ID = new stringParameter("@PROPOSED_STORE_ID", base.inputParameterList);
                    ST_EDIT_RID = new intParameter("@ST_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string PROPOSED_STORE_ID,
                                      int? ST_EDIT_RID
                                      )
                {
                    lock (typeof(MID_STORES_READ_ID_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_STORE_ID.SetValue(PROPOSED_STORE_ID);
                        this.ST_EDIT_RID.SetValue(ST_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_STORE_INSERT_def SP_MID_STORE_INSERT = new SP_MID_STORE_INSERT_def();
            public class SP_MID_STORE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_INSERT.SQL"

                private stringParameter ST_ID;
                private stringParameter STORE_NAME;
                private stringParameter STORE_DESC;
                private charParameter ACTIVE_IND;
                private stringParameter CITY;
                private stringParameter STATE;
                private intParameter SELLING_SQ_FT;
                private datetimeParameter SELLING_OPEN_DATE;
                private datetimeParameter SELLING_CLOSE_DATE;
                private datetimeParameter STOCK_OPEN_DATE;
                private datetimeParameter STOCK_CLOSE_DATE;
                private intParameter LEAD_TIME;
                private charParameter SHIP_ON_MONDAY;
                private charParameter SHIP_ON_TUESDAY;
                private charParameter SHIP_ON_WEDNESDAY;
                private charParameter SHIP_ON_THURSDAY;
                private charParameter SHIP_ON_FRIDAY;
                private charParameter SHIP_ON_SATURDAY;
                private charParameter SHIP_ON_SUNDAY;
                private charParameter SIMILAR_STORE_MODEL;
                // Begin TT#1947-MD - JSmith - Store Load API 
                //private charParameter IMO_ID;
                private stringParameter IMO_ID;
                // End TT#1947-MD - JSmith - Store Load API 
                private charParameter STORE_DELETE_IND;
                private intParameter ST_RID; //Declare Output Parameter

                public SP_MID_STORE_INSERT_def()
                {
                    base.procedureName = "SP_MID_STORE_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STORE");
                    ST_ID = new stringParameter("@ST_ID", base.inputParameterList);
                    STORE_NAME = new stringParameter("@STORE_NAME", base.inputParameterList);
                    STORE_DESC = new stringParameter("@STORE_DESC", base.inputParameterList);
                    ACTIVE_IND = new charParameter("@ACTIVE_IND", base.inputParameterList);
                    CITY = new stringParameter("@CITY", base.inputParameterList);
                    STATE = new stringParameter("@STATE", base.inputParameterList);
                    SELLING_SQ_FT = new intParameter("@SELLING_SQ_FT", base.inputParameterList);
                    SELLING_OPEN_DATE = new datetimeParameter("@SELLING_OPEN_DATE", base.inputParameterList);
                    SELLING_CLOSE_DATE = new datetimeParameter("@SELLING_CLOSE_DATE", base.inputParameterList);
                    STOCK_OPEN_DATE = new datetimeParameter("@STOCK_OPEN_DATE", base.inputParameterList);
                    STOCK_CLOSE_DATE = new datetimeParameter("@STOCK_CLOSE_DATE", base.inputParameterList);
                    LEAD_TIME = new intParameter("@LEAD_TIME", base.inputParameterList);
                    SHIP_ON_MONDAY = new charParameter("@SHIP_ON_MONDAY", base.inputParameterList);
                    SHIP_ON_TUESDAY = new charParameter("@SHIP_ON_TUESDAY", base.inputParameterList);
                    SHIP_ON_WEDNESDAY = new charParameter("@SHIP_ON_WEDNESDAY", base.inputParameterList);
                    SHIP_ON_THURSDAY = new charParameter("@SHIP_ON_THURSDAY", base.inputParameterList);
                    SHIP_ON_FRIDAY = new charParameter("@SHIP_ON_FRIDAY", base.inputParameterList);
                    SHIP_ON_SATURDAY = new charParameter("@SHIP_ON_SATURDAY", base.inputParameterList);
                    SHIP_ON_SUNDAY = new charParameter("@SHIP_ON_SUNDAY", base.inputParameterList);
                    SIMILAR_STORE_MODEL = new charParameter("@SIMILAR_STORE_MODEL", base.inputParameterList);
                    // Begin TT#1947-MD - JSmith - Store Load API
                    //IMO_ID = new charParameter("@IMO_ID", base.inputParameterList);
                    IMO_ID = new stringParameter("@IMO_ID", base.inputParameterList);
                    // End TT#1947-MD - JSmith - Store Load API
                    STORE_DELETE_IND = new charParameter("@STORE_DELETE_IND", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              string ST_ID,
                                              string STORE_NAME,
                                              string STORE_DESC,
                                              char? ACTIVE_IND,
                                              string CITY,
                                              string STATE,
                                              int? SELLING_SQ_FT,
                                              DateTime? SELLING_OPEN_DATE,
                                              DateTime? SELLING_CLOSE_DATE,
                                              DateTime? STOCK_OPEN_DATE,
                                              DateTime? STOCK_CLOSE_DATE,
                                              int? LEAD_TIME,
                                              char? SHIP_ON_MONDAY,
                                              char? SHIP_ON_TUESDAY,
                                              char? SHIP_ON_WEDNESDAY,
                                              char? SHIP_ON_THURSDAY,
                                              char? SHIP_ON_FRIDAY,
                                              char? SHIP_ON_SATURDAY,
                                              char? SHIP_ON_SUNDAY,
                                              char? SIMILAR_STORE_MODEL,
                                              // Begin TT#1947-MD - JSmith - Store Load API
                                              //char? IMO_ID,
                                              string IMO_ID,
                                              // End TT#1947-MD - JSmith - Store Load API
                                              char? STORE_DELETE_IND
                                              )
                {
                    lock (typeof(SP_MID_STORE_INSERT_def))
                    {
                        this.ST_ID.SetValue(ST_ID);
                        this.STORE_NAME.SetValue(STORE_NAME);
                        this.STORE_DESC.SetValue(STORE_DESC);
                        this.ACTIVE_IND.SetValue(ACTIVE_IND);
                        this.CITY.SetValue(CITY);
                        this.STATE.SetValue(STATE);
                        this.SELLING_SQ_FT.SetValue(SELLING_SQ_FT);
                        this.SELLING_OPEN_DATE.SetValue(SELLING_OPEN_DATE);
                        this.SELLING_CLOSE_DATE.SetValue(SELLING_CLOSE_DATE);
                        this.STOCK_OPEN_DATE.SetValue(STOCK_OPEN_DATE);
                        this.STOCK_CLOSE_DATE.SetValue(STOCK_CLOSE_DATE);
                        this.LEAD_TIME.SetValue(LEAD_TIME);
                        this.SHIP_ON_MONDAY.SetValue(SHIP_ON_MONDAY);
                        this.SHIP_ON_TUESDAY.SetValue(SHIP_ON_TUESDAY);
                        this.SHIP_ON_WEDNESDAY.SetValue(SHIP_ON_WEDNESDAY);
                        this.SHIP_ON_THURSDAY.SetValue(SHIP_ON_THURSDAY);
                        this.SHIP_ON_FRIDAY.SetValue(SHIP_ON_FRIDAY);
                        this.SHIP_ON_SATURDAY.SetValue(SHIP_ON_SATURDAY);
                        this.SHIP_ON_SUNDAY.SetValue(SHIP_ON_SUNDAY);
                        this.SIMILAR_STORE_MODEL.SetValue(SIMILAR_STORE_MODEL);
                        this.IMO_ID.SetValue(IMO_ID);
                        this.STORE_DELETE_IND.SetValue(STORE_DELETE_IND);
                        this.ST_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_STORE_UPDATE_def SP_MID_STORE_UPDATE = new SP_MID_STORE_UPDATE_def();
            public class SP_MID_STORE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STORE_UPDATE.SQL"

                private intParameter ST_RID;
                private stringParameter ST_ID;
                private stringParameter STORE_NAME;
                private stringParameter STORE_DESC;
                private charParameter ACTIVE_IND;
                private stringParameter CITY;
                private stringParameter STATE;
                private intParameter SELLING_SQ_FT;
                private datetimeParameter SELLING_OPEN_DATE;
                private datetimeParameter SELLING_CLOSE_DATE;
                private datetimeParameter STOCK_OPEN_DATE;
                private datetimeParameter STOCK_CLOSE_DATE;
                private intParameter LEAD_TIME;
                private charParameter SHIP_ON_MONDAY;
                private charParameter SHIP_ON_TUESDAY;
                private charParameter SHIP_ON_WEDNESDAY;
                private charParameter SHIP_ON_THURSDAY;
                private charParameter SHIP_ON_FRIDAY;
                private charParameter SHIP_ON_SATURDAY;
                private charParameter SHIP_ON_SUNDAY;
                private charParameter SIMILAR_STORE_MODEL;
                private stringParameter IMO_ID;
                private charParameter STORE_DELETE_IND;

                public SP_MID_STORE_UPDATE_def()
                {
                    base.procedureName = "SP_MID_STORE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE");
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    ST_ID = new stringParameter("@ST_ID", base.inputParameterList);
                    STORE_NAME = new stringParameter("@STORE_NAME", base.inputParameterList);
                    STORE_DESC = new stringParameter("@STORE_DESC", base.inputParameterList);
                    ACTIVE_IND = new charParameter("@ACTIVE_IND", base.inputParameterList);
                    CITY = new stringParameter("@CITY", base.inputParameterList);
                    STATE = new stringParameter("@STATE", base.inputParameterList);
                    SELLING_SQ_FT = new intParameter("@SELLING_SQ_FT", base.inputParameterList);
                    SELLING_OPEN_DATE = new datetimeParameter("@SELLING_OPEN_DATE", base.inputParameterList);
                    SELLING_CLOSE_DATE = new datetimeParameter("@SELLING_CLOSE_DATE", base.inputParameterList);
                    STOCK_OPEN_DATE = new datetimeParameter("@STOCK_OPEN_DATE", base.inputParameterList);
                    STOCK_CLOSE_DATE = new datetimeParameter("@STOCK_CLOSE_DATE", base.inputParameterList);
                    LEAD_TIME = new intParameter("@LEAD_TIME", base.inputParameterList);
                    SHIP_ON_MONDAY = new charParameter("@SHIP_ON_MONDAY", base.inputParameterList);
                    SHIP_ON_TUESDAY = new charParameter("@SHIP_ON_TUESDAY", base.inputParameterList);
                    SHIP_ON_WEDNESDAY = new charParameter("@SHIP_ON_WEDNESDAY", base.inputParameterList);
                    SHIP_ON_THURSDAY = new charParameter("@SHIP_ON_THURSDAY", base.inputParameterList);
                    SHIP_ON_FRIDAY = new charParameter("@SHIP_ON_FRIDAY", base.inputParameterList);
                    SHIP_ON_SATURDAY = new charParameter("@SHIP_ON_SATURDAY", base.inputParameterList);
                    SHIP_ON_SUNDAY = new charParameter("@SHIP_ON_SUNDAY", base.inputParameterList);
                    SIMILAR_STORE_MODEL = new charParameter("@SIMILAR_STORE_MODEL", base.inputParameterList);
                    IMO_ID = new stringParameter("@IMO_ID", base.inputParameterList);
                    STORE_DELETE_IND = new charParameter("@STORE_DELETE_IND", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? ST_RID,
                                  string ST_ID,
                                  string STORE_NAME,
                                  string STORE_DESC,
                                  char? ACTIVE_IND,
                                  string CITY,
                                  string STATE,
                                  int? SELLING_SQ_FT,
                                  DateTime? SELLING_OPEN_DATE,
                                  DateTime? SELLING_CLOSE_DATE,
                                  DateTime? STOCK_OPEN_DATE,
                                  DateTime? STOCK_CLOSE_DATE,
                                  int? LEAD_TIME,
                                  char? SHIP_ON_MONDAY,
                                  char? SHIP_ON_TUESDAY,
                                  char? SHIP_ON_WEDNESDAY,
                                  char? SHIP_ON_THURSDAY,
                                  char? SHIP_ON_FRIDAY,
                                  char? SHIP_ON_SATURDAY,
                                  char? SHIP_ON_SUNDAY,
                                  char? SIMILAR_STORE_MODEL,
                                  string IMO_ID,
                                  char? STORE_DELETE_IND
                                  )
                {
                    lock (typeof(SP_MID_STORE_UPDATE_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.ST_ID.SetValue(ST_ID);
                        this.STORE_NAME.SetValue(STORE_NAME);
                        this.STORE_DESC.SetValue(STORE_DESC);
                        this.ACTIVE_IND.SetValue(ACTIVE_IND);
                        this.CITY.SetValue(CITY);
                        this.STATE.SetValue(STATE);
                        this.SELLING_SQ_FT.SetValue(SELLING_SQ_FT);
                        this.SELLING_OPEN_DATE.SetValue(SELLING_OPEN_DATE);
                        this.SELLING_CLOSE_DATE.SetValue(SELLING_CLOSE_DATE);
                        this.STOCK_OPEN_DATE.SetValue(STOCK_OPEN_DATE);
                        this.STOCK_CLOSE_DATE.SetValue(STOCK_CLOSE_DATE);
                        this.LEAD_TIME.SetValue(LEAD_TIME);
                        this.SHIP_ON_MONDAY.SetValue(SHIP_ON_MONDAY);
                        this.SHIP_ON_TUESDAY.SetValue(SHIP_ON_TUESDAY);
                        this.SHIP_ON_WEDNESDAY.SetValue(SHIP_ON_WEDNESDAY);
                        this.SHIP_ON_THURSDAY.SetValue(SHIP_ON_THURSDAY);
                        this.SHIP_ON_FRIDAY.SetValue(SHIP_ON_FRIDAY);
                        this.SHIP_ON_SATURDAY.SetValue(SHIP_ON_SATURDAY);
                        this.SHIP_ON_SUNDAY.SetValue(SHIP_ON_SUNDAY);
                        this.SIMILAR_STORE_MODEL.SetValue(SIMILAR_STORE_MODEL);
                        this.IMO_ID.SetValue(IMO_ID);
                        this.STORE_DELETE_IND.SetValue(STORE_DELETE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

       
			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
