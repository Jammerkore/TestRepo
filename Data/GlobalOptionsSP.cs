using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class GlobalOptions : DataLayer
    {
        protected static class StoredProcedures
        {
            public static MID_APPLICATION_VERSION_READ_ALL_def MID_APPLICATION_VERSION_READ_ALL = new MID_APPLICATION_VERSION_READ_ALL_def();
            public class MID_APPLICATION_VERSION_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_APPLICATION_VERSION_READ_ALL.SQL"


                public MID_APPLICATION_VERSION_READ_ALL_def()
                {
                    base.procedureName = "MID_APPLICATION_VERSION_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("APPLICATION_VERSION");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_APPLICATION_VERSION_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
            public static MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ_def MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ = new MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ_def();
            public class MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ.SQL"

                private intParameter SYSTEM_OPTION_RID;

                public MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SYSTEM_OPTION_RID)
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT_def MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT = new MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT_def();
            public class MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter SEQ;
                private stringParameter DIST_CENTER;
                private intParameter SCG_RID;

                public MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                    DIST_CENTER = new stringParameter("@DIST_CENTER", base.inputParameterList);
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? SEQ,
                                  string DIST_CENTER,  
                                  int? SCG_RID
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.SEQ.SetValue(SEQ);
                        this.DIST_CENTER.SetValue(DIST_CENTER);
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_def MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE = new MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_def();
            public class MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter SEQ;

                public MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    SEQ = new intParameter("@SEQ", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? SEQ
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL_def MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL = new MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL_def();
            public class MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL.SQL"

                //private intParameter SYSTEM_OPTION_RID;

                public MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER");
                }

                public int Delete(DatabaseAccess _dba
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            // END TT#1966-MD - AGallagher - DC Fulfillment
            public static MID_SYSTEM_OPTIONS_BASIS_LABELS_READ_def MID_SYSTEM_OPTIONS_BASIS_LABELS_READ = new MID_SYSTEM_OPTIONS_BASIS_LABELS_READ_def();
            public class MID_SYSTEM_OPTIONS_BASIS_LABELS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_BASIS_LABELS_READ.SQL"

                private intParameter SYSTEM_OPTION_RID;

                public MID_SYSTEM_OPTIONS_BASIS_LABELS_READ_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_BASIS_LABELS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SYSTEM_OPTIONS_BASIS_LABELS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SYSTEM_OPTION_RID)
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_BASIS_LABELS_READ_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT_def MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT = new MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT_def();
            public class MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter LABEL_TYPE;
                private intParameter LABEL_SEQ;

                public MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SYSTEM_OPTIONS_BASIS_LABELS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    LABEL_TYPE = new intParameter("@LABEL_TYPE", base.inputParameterList);
                    LABEL_SEQ = new intParameter("@LABEL_SEQ", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? LABEL_TYPE,
                                  int? LABEL_SEQ
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.LABEL_TYPE.SetValue(LABEL_TYPE);
                        this.LABEL_SEQ.SetValue(LABEL_SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE_def MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE = new MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE_def();
            public class MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter LABEL_TYPE;

                public MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SYSTEM_OPTIONS_BASIS_LABELS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    LABEL_TYPE = new intParameter("@LABEL_TYPE", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? LABEL_TYPE
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.LABEL_TYPE.SetValue(LABEL_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_READ_ALL_def MID_SYSTEM_OPTIONS_READ_ALL = new MID_SYSTEM_OPTIONS_READ_ALL_def();
            public class MID_SYSTEM_OPTIONS_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_READ_ALL.SQL"


                public MID_SYSTEM_OPTIONS_READ_ALL_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SYSTEM_EMAIL_READ_ALL_def MID_SYSTEM_EMAIL_READ_ALL = new MID_SYSTEM_EMAIL_READ_ALL_def();
            public class MID_SYSTEM_EMAIL_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_EMAIL_READ_ALL.SQL"


                public MID_SYSTEM_EMAIL_READ_ALL_def()
                {
                    base.procedureName = "MID_SYSTEM_EMAIL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SYSTEM_EMAIL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_SYSTEM_EMAIL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STATE_PROVINCE_READ_ALL_def MID_STATE_PROVINCE_READ_ALL = new MID_STATE_PROVINCE_READ_ALL_def();
            public class MID_STATE_PROVINCE_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STATE_PROVINCE_READ_ALL.SQL"


                public MID_STATE_PROVINCE_READ_ALL_def()
                {
                    base.procedureName = "MID_STATE_PROVINCE_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STATE_PROVINCE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STATE_PROVINCE_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE_def MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE = new MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE_def();
            public class MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter RESERVE_ST_RID;

                public MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    RESERVE_ST_RID = new intParameter("@RESERVE_ST_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? RESERVE_ST_RID
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.RESERVE_ST_RID.SetValue(RESERVE_ST_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS_def MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS = new MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS_def();
            public class MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private charParameter STORE_DELETE_IN_PROGRESS_IND;

                public MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    STORE_DELETE_IN_PROGRESS_IND = new charParameter("@STORE_DELETE_IN_PROGRESS_IND", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  char? STORE_DELETE_IN_PROGRESS_IND
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.STORE_DELETE_IN_PROGRESS_IND.SetValue(STORE_DELETE_IN_PROGRESS_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC_def MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC = new MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC_def();
            public class MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private intParameter HEADER_LINK_CHARACTERISTIC;

                public MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    HEADER_LINK_CHARACTERISTIC = new intParameter("@HEADER_LINK_CHARACTERISTIC", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  int? HEADER_LINK_CHARACTERISTIC
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.HEADER_LINK_CHARACTERISTIC.SetValue(HEADER_LINK_CHARACTERISTIC);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HEADER_TYPE_RELEASE_READ_ALL_def MID_HEADER_TYPE_RELEASE_READ_ALL = new MID_HEADER_TYPE_RELEASE_READ_ALL_def();
            public class MID_HEADER_TYPE_RELEASE_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_TYPE_RELEASE_READ_ALL.SQL"


                public MID_HEADER_TYPE_RELEASE_READ_ALL_def()
                {
                    base.procedureName = "MID_HEADER_TYPE_RELEASE_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_TYPE_RELEASE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_TYPE_RELEASE_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_TYPE_RELEASE_INSERT_def MID_HEADER_TYPE_RELEASE_INSERT = new MID_HEADER_TYPE_RELEASE_INSERT_def();
            public class MID_HEADER_TYPE_RELEASE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_TYPE_RELEASE_INSERT.SQL"

                private intParameter HEADER_TYPE;
                private charParameter RELEASE_HEADER_TYPE_IND;

                public MID_HEADER_TYPE_RELEASE_INSERT_def()
                {
                    base.procedureName = "MID_HEADER_TYPE_RELEASE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HEADER_TYPE_RELEASE");
                    HEADER_TYPE = new intParameter("@HEADER_TYPE", base.inputParameterList);
                    RELEASE_HEADER_TYPE_IND = new charParameter("@RELEASE_HEADER_TYPE_IND", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? HEADER_TYPE,
                                  char? RELEASE_HEADER_TYPE_IND
                                  )
                {
                    lock (typeof(MID_HEADER_TYPE_RELEASE_INSERT_def))
                    {
                        this.HEADER_TYPE.SetValue(HEADER_TYPE);
                        this.RELEASE_HEADER_TYPE_IND.SetValue(RELEASE_HEADER_TYPE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HEADER_TYPE_RELEASE_DELETE_ALL_def MID_HEADER_TYPE_RELEASE_DELETE_ALL = new MID_HEADER_TYPE_RELEASE_DELETE_ALL_def();
            public class MID_HEADER_TYPE_RELEASE_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_TYPE_RELEASE_DELETE_ALL.SQL"


                public MID_HEADER_TYPE_RELEASE_DELETE_ALL_def()
                {
                    base.procedureName = "MID_HEADER_TYPE_RELEASE_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_TYPE_RELEASE");
                }

                public int Delete(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_TYPE_RELEASE_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY_def MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY = new MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY_def();
            public class MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY.SQL"

                private intParameter SYSTEM_OPTION_RID;
                private stringParameter LICENSE_KEY;

                public MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                    SYSTEM_OPTION_RID = new intParameter("@SYSTEM_OPTION_RID", base.inputParameterList);
                    LICENSE_KEY = new stringParameter("@LICENSE_KEY", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SYSTEM_OPTION_RID,
                                  string LICENSE_KEY
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY_def))
                    {
                        this.SYSTEM_OPTION_RID.SetValue(SYSTEM_OPTION_RID);
                        this.LICENSE_KEY.SetValue(LICENSE_KEY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SYSTEM_OPTIONS_UPDATE_def MID_SYSTEM_OPTIONS_UPDATE = new MID_SYSTEM_OPTIONS_UPDATE_def();
            public class MID_SYSTEM_OPTIONS_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SYSTEM_OPTIONS_UPDATE.SQL"

                private intParameter DO_UPDATE_RESERVE_STORE;
                private intParameter RESERVE_ST_RID;
                private stringParameter COMPANY_NAME;
                private stringParameter COMPANY_STREET;
                private stringParameter COMPANY_CITY;
                private stringParameter COMPANY_SP_ABBREVIATION;
                private stringParameter COMPANY_POSTAL_CODE;
                private stringParameter COMPANY_TELEPHONE;
                private stringParameter COMPANY_FAX;
                private stringParameter COMPANY_EMAIL;
                private charParameter PRODUCT_LEVEL_DELIMITER;
                private charParameter SMTP_ENABLED;
                private stringParameter SMTP_SERVER;
                private intParameter SMTP_PORT;
                private charParameter SMTP_USE_SSL;
                private charParameter SMTP_USE_DEFAULT_CREDENTIALS;
                private stringParameter SMTP_USERNAME;
                private stringParameter SMTP_PWD;
                private charParameter SMTP_MESSAGE_FORMAT_IN_HTML;
                private charParameter SMTP_USE_OUTLOOK_CONTACTS;
                private stringParameter SMTP_FROM_ADDRESS;
                private intParameter PURGE_ALLOCATIONS;
                private intParameter STORE_DISPLAY_OPTION_ID;
                private intParameter DEFAULT_OTS_SG_RID;
                private intParameter DEFAULT_ALLOC_SG_RID;
                private intParameter NEW_STORE_TIMEFRAME_BEGIN;
                private intParameter NEW_STORE_TIMEFRAME_END;
                private intParameter NON_COMP_STORE_TIMEFRAME_BEGIN;
                private intParameter NON_COMP_STORE_TIMEFRAME_END;
                private intParameter PRODUCT_LEVEL_DISPLAY_ID;
                private floatParameter DEFAULT_PCT_NEED_LIMIT;
                private floatParameter DEFAULT_BALANCE_TOLERANCE;
                private floatParameter DEFAULT_PACK_SIZE_ERROR_PCT;
                private floatParameter DEFAULT_MAX_SIZE_ERROR_PCT;
                private floatParameter DEFAULT_FILL_SIZE_HOLES_PCT;
                private charParameter SIZE_BREAKOUT_IND;
                private charParameter SIZE_NEED_IND;
                private charParameter BULK_IS_DETAIL_IND;
                private charParameter PROTECT_IF_HDRS_IND;
                private charParameter USE_WINDOWS_LOGIN;
                private intParameter STORE_GRADE_TIMEFRAME;
                private charParameter NORMALIZE_SIZE_CURVES_IND;
                private intParameter FILL_SIZES_TO_TYPE;
                private floatParameter GENERIC_PACK_ROUNDING_1ST_PACK_PCT;
                private floatParameter GENERIC_PACK_ROUNDING_NTH_PACK_PCT;
                private intParameter ACTIVITY_MESSAGE_UPPER_LIMIT;
                private intParameter SHIPPING_HORIZON_WEEKS;
                private intParameter HEADER_LINK_CHARACTERISTIC;
                private stringParameter SIZE_CURVE_CHARMASK;
                private stringParameter SIZE_GROUP_CHARMASK;
                private stringParameter SIZE_ALTERNATE_CHARMASK;
                private stringParameter SIZE_CONSTRAINT_CHARMASK;
                private charParameter ALLOW_RLSE_IF_ALL_IN_RSRV_IND;
                private intParameter GENERIC_SIZE_CURVE_NAME_TYPE;
                private intParameter NUMBER_OF_WEEKS_WITH_ZERO_SALES;
                private intParameter MAXIMUM_CHAIN_WOS;
                private charParameter PRORATE_CHAIN_STOCK;
                private intParameter GEN_SIZE_CURVE_USING;
                private charParameter PACK_TOLERANCE_NO_MAX_STEP_IND;
                private charParameter PACK_TOLERANCE_STEPPED_IND;
                private charParameter RI_EXPAND_IND;
                private intParameter ALLOW_STORE_MAX_VALUE_MODIFICATION;
                private intParameter VSW_SIZE_CONSTRAINTS;
                private charParameter ENABLE_VELOCITY_GRADE_OPTIONS;
                private charParameter FORCE_SINGLE_CLIENT_INSTANCE;
                private charParameter FORCE_SINGLE_USER_INSTANCE;
                private charParameter USE_ACTIVE_DIRECTORY_AUTHENTICATION;
                private charParameter USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN;
                private charParameter USE_BATCH_ONLY_MODE;
                private charParameter CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON;
                private intParameter VSW_ITEM_FWOS_MAX_IND;
                private charParameter PRIOR_HEADER_INCLUDE_RESERVE_IND;
                private intParameter DC_CARTON_ROUNDING_SG_RID;	// TT#1652-MD - stodd - DC Carton Rounding
                private intParameter SPLIT_OPTION;              // TT#1966-MD - AGallagher - DC Fulfillment
                private charParameter APPLY_MINIMUMS_IND;       // TT#1966-MD - AGallagher - DC Fulfillment
                private charParameter PRIORITIZE_TYPE;          // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter HEADER_FIELD;              // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter HCG_RID;                   // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter HEADERS_ORDER;             // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter STORES_ORDER;              // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter SPLIT_BY_OPTION;           // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter SPLIT_BY_RESERVE;          // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter APPLY_BY;                  // TT#1966-MD - AGallagher - DC Fulfillment
                private intParameter WITHIN_DC;                  // TT#1966-MD - AGallagher - DC Fulfillment
       
                public MID_SYSTEM_OPTIONS_UPDATE_def()
                {
                    base.procedureName = "MID_SYSTEM_OPTIONS_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SYSTEM_OPTIONS");
                    DO_UPDATE_RESERVE_STORE = new intParameter("@DO_UPDATE_RESERVE_STORE", base.inputParameterList);
                    RESERVE_ST_RID = new intParameter("@RESERVE_ST_RID", base.inputParameterList);
                    COMPANY_NAME = new stringParameter("@COMPANY_NAME", base.inputParameterList);
                    COMPANY_STREET = new stringParameter("@COMPANY_STREET", base.inputParameterList);
                    COMPANY_CITY = new stringParameter("@COMPANY_CITY", base.inputParameterList);
                    COMPANY_SP_ABBREVIATION = new stringParameter("@COMPANY_SP_ABBREVIATION", base.inputParameterList);
                    COMPANY_POSTAL_CODE = new stringParameter("@COMPANY_POSTAL_CODE", base.inputParameterList);
                    COMPANY_TELEPHONE = new stringParameter("@COMPANY_TELEPHONE", base.inputParameterList);
                    COMPANY_FAX = new stringParameter("@COMPANY_FAX", base.inputParameterList);
                    COMPANY_EMAIL = new stringParameter("@COMPANY_EMAIL", base.inputParameterList);
                    PRODUCT_LEVEL_DELIMITER = new charParameter("@PRODUCT_LEVEL_DELIMITER", base.inputParameterList);
                    SMTP_ENABLED = new charParameter("@SMTP_ENABLED", base.inputParameterList);
                    SMTP_SERVER = new stringParameter("@SMTP_SERVER", base.inputParameterList);
                    SMTP_PORT = new intParameter("@SMTP_PORT", base.inputParameterList);
                    SMTP_USE_SSL = new charParameter("@SMTP_USE_SSL", base.inputParameterList);
                    SMTP_USE_DEFAULT_CREDENTIALS = new charParameter("@SMTP_USE_DEFAULT_CREDENTIALS", base.inputParameterList);
                    SMTP_USERNAME = new stringParameter("@SMTP_USERNAME", base.inputParameterList);
                    SMTP_PWD = new stringParameter("@SMTP_PWD", base.inputParameterList);
                    SMTP_MESSAGE_FORMAT_IN_HTML = new charParameter("@SMTP_MESSAGE_FORMAT_IN_HTML", base.inputParameterList);
                    SMTP_USE_OUTLOOK_CONTACTS = new charParameter("@SMTP_USE_OUTLOOK_CONTACTS", base.inputParameterList);
                    SMTP_FROM_ADDRESS = new stringParameter("@SMTP_FROM_ADDRESS", base.inputParameterList);
                    PURGE_ALLOCATIONS = new intParameter("@PURGE_ALLOCATIONS", base.inputParameterList);
                    STORE_DISPLAY_OPTION_ID = new intParameter("@STORE_DISPLAY_OPTION_ID", base.inputParameterList);
                    DEFAULT_OTS_SG_RID = new intParameter("@DEFAULT_OTS_SG_RID", base.inputParameterList);
                    DEFAULT_ALLOC_SG_RID = new intParameter("@DEFAULT_ALLOC_SG_RID", base.inputParameterList);
                    NEW_STORE_TIMEFRAME_BEGIN = new intParameter("@NEW_STORE_TIMEFRAME_BEGIN", base.inputParameterList);
                    NEW_STORE_TIMEFRAME_END = new intParameter("@NEW_STORE_TIMEFRAME_END", base.inputParameterList);
                    NON_COMP_STORE_TIMEFRAME_BEGIN = new intParameter("@NON_COMP_STORE_TIMEFRAME_BEGIN", base.inputParameterList);
                    NON_COMP_STORE_TIMEFRAME_END = new intParameter("@NON_COMP_STORE_TIMEFRAME_END", base.inputParameterList);
                    PRODUCT_LEVEL_DISPLAY_ID = new intParameter("@PRODUCT_LEVEL_DISPLAY_ID", base.inputParameterList);
                    DEFAULT_PCT_NEED_LIMIT = new floatParameter("@DEFAULT_PCT_NEED_LIMIT", base.inputParameterList);
                    DEFAULT_BALANCE_TOLERANCE = new floatParameter("@DEFAULT_BALANCE_TOLERANCE", base.inputParameterList);
                    DEFAULT_PACK_SIZE_ERROR_PCT = new floatParameter("@DEFAULT_PACK_SIZE_ERROR_PCT", base.inputParameterList);
                    DEFAULT_MAX_SIZE_ERROR_PCT = new floatParameter("@DEFAULT_MAX_SIZE_ERROR_PCT", base.inputParameterList);
                    DEFAULT_FILL_SIZE_HOLES_PCT = new floatParameter("@DEFAULT_FILL_SIZE_HOLES_PCT", base.inputParameterList);
                    SIZE_BREAKOUT_IND = new charParameter("@SIZE_BREAKOUT_IND", base.inputParameterList);
                    SIZE_NEED_IND = new charParameter("@SIZE_NEED_IND", base.inputParameterList);
                    BULK_IS_DETAIL_IND = new charParameter("@BULK_IS_DETAIL_IND", base.inputParameterList);
                    PROTECT_IF_HDRS_IND = new charParameter("@PROTECT_IF_HDRS_IND", base.inputParameterList);
                    USE_WINDOWS_LOGIN = new charParameter("@USE_WINDOWS_LOGIN", base.inputParameterList);
                    STORE_GRADE_TIMEFRAME = new intParameter("@STORE_GRADE_TIMEFRAME", base.inputParameterList);
                    NORMALIZE_SIZE_CURVES_IND = new charParameter("@NORMALIZE_SIZE_CURVES_IND", base.inputParameterList);
                    FILL_SIZES_TO_TYPE = new intParameter("@FILL_SIZES_TO_TYPE", base.inputParameterList);
                    GENERIC_PACK_ROUNDING_1ST_PACK_PCT = new floatParameter("@GENERIC_PACK_ROUNDING_1ST_PACK_PCT", base.inputParameterList);
                    GENERIC_PACK_ROUNDING_NTH_PACK_PCT = new floatParameter("@GENERIC_PACK_ROUNDING_NTH_PACK_PCT", base.inputParameterList);
                    ACTIVITY_MESSAGE_UPPER_LIMIT = new intParameter("@ACTIVITY_MESSAGE_UPPER_LIMIT", base.inputParameterList);
                    SHIPPING_HORIZON_WEEKS = new intParameter("@SHIPPING_HORIZON_WEEKS", base.inputParameterList);
                    HEADER_LINK_CHARACTERISTIC = new intParameter("@HEADER_LINK_CHARACTERISTIC", base.inputParameterList);
                    SIZE_CURVE_CHARMASK = new stringParameter("@SIZE_CURVE_CHARMASK", base.inputParameterList);
                    SIZE_GROUP_CHARMASK = new stringParameter("@SIZE_GROUP_CHARMASK", base.inputParameterList);
                    SIZE_ALTERNATE_CHARMASK = new stringParameter("@SIZE_ALTERNATE_CHARMASK", base.inputParameterList);
                    SIZE_CONSTRAINT_CHARMASK = new stringParameter("@SIZE_CONSTRAINT_CHARMASK", base.inputParameterList);
                    ALLOW_RLSE_IF_ALL_IN_RSRV_IND = new charParameter("@ALLOW_RLSE_IF_ALL_IN_RSRV_IND", base.inputParameterList);
                    GENERIC_SIZE_CURVE_NAME_TYPE = new intParameter("@GENERIC_SIZE_CURVE_NAME_TYPE", base.inputParameterList);
                    NUMBER_OF_WEEKS_WITH_ZERO_SALES = new intParameter("@NUMBER_OF_WEEKS_WITH_ZERO_SALES", base.inputParameterList);
                    MAXIMUM_CHAIN_WOS = new intParameter("@MAXIMUM_CHAIN_WOS", base.inputParameterList);
                    PRORATE_CHAIN_STOCK = new charParameter("@PRORATE_CHAIN_STOCK", base.inputParameterList);
                    GEN_SIZE_CURVE_USING = new intParameter("@GEN_SIZE_CURVE_USING", base.inputParameterList);
                    PACK_TOLERANCE_NO_MAX_STEP_IND = new charParameter("@PACK_TOLERANCE_NO_MAX_STEP_IND", base.inputParameterList);
                    PACK_TOLERANCE_STEPPED_IND = new charParameter("@PACK_TOLERANCE_STEPPED_IND", base.inputParameterList);
                    RI_EXPAND_IND = new charParameter("@RI_EXPAND_IND", base.inputParameterList);
                    ALLOW_STORE_MAX_VALUE_MODIFICATION = new intParameter("@ALLOW_STORE_MAX_VALUE_MODIFICATION", base.inputParameterList);
                    VSW_SIZE_CONSTRAINTS = new intParameter("@VSW_SIZE_CONSTRAINTS", base.inputParameterList);
                    ENABLE_VELOCITY_GRADE_OPTIONS = new charParameter("@ENABLE_VELOCITY_GRADE_OPTIONS", base.inputParameterList);
                    FORCE_SINGLE_CLIENT_INSTANCE = new charParameter("@FORCE_SINGLE_CLIENT_INSTANCE", base.inputParameterList);
                    FORCE_SINGLE_USER_INSTANCE = new charParameter("@FORCE_SINGLE_USER_INSTANCE", base.inputParameterList);
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION = new charParameter("@USE_ACTIVE_DIRECTORY_AUTHENTICATION", base.inputParameterList);
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN = new charParameter("@USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN", base.inputParameterList);
                    USE_BATCH_ONLY_MODE = new charParameter("@USE_BATCH_ONLY_MODE", base.inputParameterList);
                    CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON = new charParameter("@CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON", base.inputParameterList);
                    VSW_ITEM_FWOS_MAX_IND = new intParameter("@VSW_ITEM_FWOS_MAX_IND", base.inputParameterList);
                    PRIOR_HEADER_INCLUDE_RESERVE_IND = new charParameter("@PRIOR_HEADER_INCLUDE_RESERVE_IND", base.inputParameterList);
                    DC_CARTON_ROUNDING_SG_RID = new intParameter("@DC_CARTON_ROUNDING_SG_RID", base.inputParameterList);	// TT#1652-MD - stodd - DC Carton Rounding
                    SPLIT_OPTION = new intParameter("@SPLIT_OPTION", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    APPLY_MINIMUMS_IND = new charParameter("@APPLY_MINIMUMS_IND", base.inputParameterList);     // TT#1966-MD - AGallagher - DC Fulfillment
                    PRIORITIZE_TYPE = new charParameter("@PRIORITIZE_TYPE", base.inputParameterList);           // TT#1966-MD - AGallagher - DC Fulfillment
                    HEADER_FIELD = new intParameter("@HEADER_FIELD", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);                       // TT#1966-MD - AGallagher - DC Fulfillment
                    HEADERS_ORDER = new intParameter("@HEADERS_ORDER", base.inputParameterList);                 // TT#1966-MD - AGallagher - DC Fulfillment
                    STORES_ORDER = new intParameter("@STORES_ORDER", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    SPLIT_BY_OPTION = new intParameter("@SPLIT_BY_OPTION", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    SPLIT_BY_RESERVE = new intParameter("@SPLIT_BY_RESERVE", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    APPLY_BY = new intParameter("@APPLY_BY", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                    WITHIN_DC = new intParameter("@WITHIN_DC", base.inputParameterList);                  // TT#1966-MD - AGallagher - DC Fulfillment
                }

                public int Update(DatabaseAccess _dba,
                                  int? DO_UPDATE_RESERVE_STORE,
                                  int? RESERVE_ST_RID,
                                  string COMPANY_NAME,
                                  string COMPANY_STREET,
                                  string COMPANY_CITY,
                                  string COMPANY_SP_ABBREVIATION,
                                  string COMPANY_POSTAL_CODE,
                                  string COMPANY_TELEPHONE,
                                  string COMPANY_FAX,
                                  string COMPANY_EMAIL,
                                  char? PRODUCT_LEVEL_DELIMITER,
                                  char? SMTP_ENABLED,
                                  string SMTP_SERVER,
                                  int? SMTP_PORT,
                                  char? SMTP_USE_SSL,
                                  char? SMTP_USE_DEFAULT_CREDENTIALS,
                                  string SMTP_USERNAME,
                                  string SMTP_PWD,
                                  char? SMTP_MESSAGE_FORMAT_IN_HTML,
                                  char? SMTP_USE_OUTLOOK_CONTACTS,
                                  string SMTP_FROM_ADDRESS,
                                  int? PURGE_ALLOCATIONS,
                                  int? STORE_DISPLAY_OPTION_ID,
                                  int? DEFAULT_OTS_SG_RID,
                                  int? DEFAULT_ALLOC_SG_RID,
                                  int? NEW_STORE_TIMEFRAME_BEGIN,
                                  int? NEW_STORE_TIMEFRAME_END,
                                  int? NON_COMP_STORE_TIMEFRAME_BEGIN,
                                  int? NON_COMP_STORE_TIMEFRAME_END,
                                  int? PRODUCT_LEVEL_DISPLAY_ID,
                                  double? DEFAULT_PCT_NEED_LIMIT,
                                  double? DEFAULT_BALANCE_TOLERANCE,
                                  double? DEFAULT_PACK_SIZE_ERROR_PCT,
                                  double? DEFAULT_MAX_SIZE_ERROR_PCT,
                                  double? DEFAULT_FILL_SIZE_HOLES_PCT,
                                  char? SIZE_BREAKOUT_IND,
                                  char? SIZE_NEED_IND,
                                  char? BULK_IS_DETAIL_IND,
                                  char? PROTECT_IF_HDRS_IND,
                                  char? USE_WINDOWS_LOGIN,
                                  int? STORE_GRADE_TIMEFRAME,
                                  char? NORMALIZE_SIZE_CURVES_IND,
                                  int? FILL_SIZES_TO_TYPE,
                                  double? GENERIC_PACK_ROUNDING_1ST_PACK_PCT,
                                  double? GENERIC_PACK_ROUNDING_NTH_PACK_PCT,
                                  int? ACTIVITY_MESSAGE_UPPER_LIMIT,
                                  int? SHIPPING_HORIZON_WEEKS,
                                  int? HEADER_LINK_CHARACTERISTIC,
                                  string SIZE_CURVE_CHARMASK,
                                  string SIZE_GROUP_CHARMASK,
                                  string SIZE_ALTERNATE_CHARMASK,
                                  string SIZE_CONSTRAINT_CHARMASK,
                                  char? ALLOW_RLSE_IF_ALL_IN_RSRV_IND,
                                  int? GENERIC_SIZE_CURVE_NAME_TYPE,
                                  int? NUMBER_OF_WEEKS_WITH_ZERO_SALES,
                                  int? MAXIMUM_CHAIN_WOS,
                                  char? PRORATE_CHAIN_STOCK,
                                  int? GEN_SIZE_CURVE_USING,
                                  char? PACK_TOLERANCE_NO_MAX_STEP_IND,
                                  char? PACK_TOLERANCE_STEPPED_IND,
                                  char? RI_EXPAND_IND,
                                  int? ALLOW_STORE_MAX_VALUE_MODIFICATION,
                                  int? VSW_SIZE_CONSTRAINTS,
                                  char? ENABLE_VELOCITY_GRADE_OPTIONS,
                                  char? FORCE_SINGLE_CLIENT_INSTANCE,
                                  char? FORCE_SINGLE_USER_INSTANCE,
                                  char? USE_ACTIVE_DIRECTORY_AUTHENTICATION,
                                  char? USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN,
                                  char? USE_BATCH_ONLY_MODE,
                                  char? CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON,
                                  int? VSW_ITEM_FWOS_MAX_IND,
                                  char? PRIOR_HEADER_INCLUDE_RESERVE_IND,
                                  int? DC_CARTON_ROUNDING_SG_RID,	// TT#1652-MD - stodd - DC Carton Rounding
                                  int? SPLIT_OPTION,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  char? APPLY_MINIMUMS_IND, // TT#1966-MD - AGallagher - DC Fulfillment
                                  char? PRIORITIZE_TYPE,    // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? HEADER_FIELD,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? HCG_RID,             // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? HEADERS_ORDER,       // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? STORES_ORDER,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? SPLIT_BY_OPTION,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? SPLIT_BY_RESERVE,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? APPLY_BY,        // TT#1966-MD - AGallagher - DC Fulfillment
                                  int? WITHIN_DC        // TT#1966-MD - AGallagher - DC Fulfillment
                                  )
                {
                    lock (typeof(MID_SYSTEM_OPTIONS_UPDATE_def))
                    {
                        this.DO_UPDATE_RESERVE_STORE.SetValue(DO_UPDATE_RESERVE_STORE);
                        this.RESERVE_ST_RID.SetValue(RESERVE_ST_RID);
                        this.COMPANY_NAME.SetValue(COMPANY_NAME);
                        this.COMPANY_STREET.SetValue(COMPANY_STREET);
                        this.COMPANY_CITY.SetValue(COMPANY_CITY);
                        this.COMPANY_SP_ABBREVIATION.SetValue(COMPANY_SP_ABBREVIATION);
                        this.COMPANY_POSTAL_CODE.SetValue(COMPANY_POSTAL_CODE);
                        this.COMPANY_TELEPHONE.SetValue(COMPANY_TELEPHONE);
                        this.COMPANY_FAX.SetValue(COMPANY_FAX);
                        this.COMPANY_EMAIL.SetValue(COMPANY_EMAIL);
                        this.PRODUCT_LEVEL_DELIMITER.SetValue(PRODUCT_LEVEL_DELIMITER);
                        this.SMTP_ENABLED.SetValue(SMTP_ENABLED);
                        this.SMTP_SERVER.SetValue(SMTP_SERVER);
                        this.SMTP_PORT.SetValue(SMTP_PORT);
                        this.SMTP_USE_SSL.SetValue(SMTP_USE_SSL);
                        this.SMTP_USE_DEFAULT_CREDENTIALS.SetValue(SMTP_USE_DEFAULT_CREDENTIALS);
                        this.SMTP_USERNAME.SetValue(SMTP_USERNAME);
                        this.SMTP_PWD.SetValue(SMTP_PWD);
                        this.SMTP_MESSAGE_FORMAT_IN_HTML.SetValue(SMTP_MESSAGE_FORMAT_IN_HTML);
                        this.SMTP_USE_OUTLOOK_CONTACTS.SetValue(SMTP_USE_OUTLOOK_CONTACTS);
                        this.SMTP_FROM_ADDRESS.SetValue(SMTP_FROM_ADDRESS);
                        this.PURGE_ALLOCATIONS.SetValue(PURGE_ALLOCATIONS);
                        this.STORE_DISPLAY_OPTION_ID.SetValue(STORE_DISPLAY_OPTION_ID);
                        this.DEFAULT_OTS_SG_RID.SetValue(DEFAULT_OTS_SG_RID);
                        this.DEFAULT_ALLOC_SG_RID.SetValue(DEFAULT_ALLOC_SG_RID);
                        this.NEW_STORE_TIMEFRAME_BEGIN.SetValue(NEW_STORE_TIMEFRAME_BEGIN);
                        this.NEW_STORE_TIMEFRAME_END.SetValue(NEW_STORE_TIMEFRAME_END);
                        this.NON_COMP_STORE_TIMEFRAME_BEGIN.SetValue(NON_COMP_STORE_TIMEFRAME_BEGIN);
                        this.NON_COMP_STORE_TIMEFRAME_END.SetValue(NON_COMP_STORE_TIMEFRAME_END);
                        this.PRODUCT_LEVEL_DISPLAY_ID.SetValue(PRODUCT_LEVEL_DISPLAY_ID);
                        this.DEFAULT_PCT_NEED_LIMIT.SetValue(DEFAULT_PCT_NEED_LIMIT);
                        this.DEFAULT_BALANCE_TOLERANCE.SetValue(DEFAULT_BALANCE_TOLERANCE);
                        this.DEFAULT_PACK_SIZE_ERROR_PCT.SetValue(DEFAULT_PACK_SIZE_ERROR_PCT);
                        this.DEFAULT_MAX_SIZE_ERROR_PCT.SetValue(DEFAULT_MAX_SIZE_ERROR_PCT);
                        this.DEFAULT_FILL_SIZE_HOLES_PCT.SetValue(DEFAULT_FILL_SIZE_HOLES_PCT);
                        this.SIZE_BREAKOUT_IND.SetValue(SIZE_BREAKOUT_IND);
                        this.SIZE_NEED_IND.SetValue(SIZE_NEED_IND);
                        this.BULK_IS_DETAIL_IND.SetValue(BULK_IS_DETAIL_IND);
                        this.PROTECT_IF_HDRS_IND.SetValue(PROTECT_IF_HDRS_IND);
                        this.USE_WINDOWS_LOGIN.SetValue(USE_WINDOWS_LOGIN);
                        this.STORE_GRADE_TIMEFRAME.SetValue(STORE_GRADE_TIMEFRAME);
                        this.NORMALIZE_SIZE_CURVES_IND.SetValue(NORMALIZE_SIZE_CURVES_IND);
                        this.FILL_SIZES_TO_TYPE.SetValue(FILL_SIZES_TO_TYPE);
                        this.GENERIC_PACK_ROUNDING_1ST_PACK_PCT.SetValue(GENERIC_PACK_ROUNDING_1ST_PACK_PCT);
                        this.GENERIC_PACK_ROUNDING_NTH_PACK_PCT.SetValue(GENERIC_PACK_ROUNDING_NTH_PACK_PCT);
                        this.ACTIVITY_MESSAGE_UPPER_LIMIT.SetValue(ACTIVITY_MESSAGE_UPPER_LIMIT);
                        this.SHIPPING_HORIZON_WEEKS.SetValue(SHIPPING_HORIZON_WEEKS);
                        this.HEADER_LINK_CHARACTERISTIC.SetValue(HEADER_LINK_CHARACTERISTIC);
                        this.SIZE_CURVE_CHARMASK.SetValue(SIZE_CURVE_CHARMASK);
                        this.SIZE_GROUP_CHARMASK.SetValue(SIZE_GROUP_CHARMASK);
                        this.SIZE_ALTERNATE_CHARMASK.SetValue(SIZE_ALTERNATE_CHARMASK);
                        this.SIZE_CONSTRAINT_CHARMASK.SetValue(SIZE_CONSTRAINT_CHARMASK);
                        this.ALLOW_RLSE_IF_ALL_IN_RSRV_IND.SetValue(ALLOW_RLSE_IF_ALL_IN_RSRV_IND);
                        this.GENERIC_SIZE_CURVE_NAME_TYPE.SetValue(GENERIC_SIZE_CURVE_NAME_TYPE);
                        this.NUMBER_OF_WEEKS_WITH_ZERO_SALES.SetValue(NUMBER_OF_WEEKS_WITH_ZERO_SALES);
                        this.MAXIMUM_CHAIN_WOS.SetValue(MAXIMUM_CHAIN_WOS);
                        this.PRORATE_CHAIN_STOCK.SetValue(PRORATE_CHAIN_STOCK);
                        this.GEN_SIZE_CURVE_USING.SetValue(GEN_SIZE_CURVE_USING);
                        this.PACK_TOLERANCE_NO_MAX_STEP_IND.SetValue(PACK_TOLERANCE_NO_MAX_STEP_IND);
                        this.PACK_TOLERANCE_STEPPED_IND.SetValue(PACK_TOLERANCE_STEPPED_IND);
                        this.RI_EXPAND_IND.SetValue(RI_EXPAND_IND);
                        this.ALLOW_STORE_MAX_VALUE_MODIFICATION.SetValue(ALLOW_STORE_MAX_VALUE_MODIFICATION);
                        this.VSW_SIZE_CONSTRAINTS.SetValue(VSW_SIZE_CONSTRAINTS);
                        this.ENABLE_VELOCITY_GRADE_OPTIONS.SetValue(ENABLE_VELOCITY_GRADE_OPTIONS);
                        this.FORCE_SINGLE_CLIENT_INSTANCE.SetValue(FORCE_SINGLE_CLIENT_INSTANCE);
                        this.FORCE_SINGLE_USER_INSTANCE.SetValue(FORCE_SINGLE_USER_INSTANCE);
                        this.USE_ACTIVE_DIRECTORY_AUTHENTICATION.SetValue(USE_ACTIVE_DIRECTORY_AUTHENTICATION);
                        this.USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN.SetValue(USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN);
                        this.USE_BATCH_ONLY_MODE.SetValue(USE_BATCH_ONLY_MODE);
                        this.CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON.SetValue(CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON);
                        this.VSW_ITEM_FWOS_MAX_IND.SetValue(VSW_ITEM_FWOS_MAX_IND);
                        this.PRIOR_HEADER_INCLUDE_RESERVE_IND.SetValue(PRIOR_HEADER_INCLUDE_RESERVE_IND);
                        this.DC_CARTON_ROUNDING_SG_RID.SetValue(DC_CARTON_ROUNDING_SG_RID);	// TT#1652-MD - stodd - DC Carton Rounding
                        this.SPLIT_OPTION.SetValue(SPLIT_OPTION);               // TT#1966-MD - AGallagher - DC Fulfillment
                        this.APPLY_MINIMUMS_IND.SetValue(APPLY_MINIMUMS_IND);   // TT#1966-MD - AGallagher - DC Fulfillment
                        this.PRIORITIZE_TYPE.SetValue(PRIORITIZE_TYPE);         // TT#1966-MD - AGallagher - DC Fulfillment
                        this.HEADER_FIELD.SetValue(HEADER_FIELD);               // TT#1966-MD - AGallagher - DC Fulfillment
                        this.HCG_RID.SetValue(HCG_RID);                         // TT#1966-MD - AGallagher - DC Fulfillment
                        this.HEADERS_ORDER.SetValue(HEADERS_ORDER);             // TT#1966-MD - AGallagher - DC Fulfillment
                        this.STORES_ORDER.SetValue(STORES_ORDER);               // TT#1966-MD - AGallagher - DC Fulfillment
                        this.SPLIT_BY_OPTION.SetValue(SPLIT_BY_OPTION);         // TT#1966-MD - AGallagher - DC Fulfillment
                        this.SPLIT_BY_RESERVE.SetValue(SPLIT_BY_RESERVE);       // TT#1966-MD - AGallagher - DC Fulfillment
                        this.APPLY_BY.SetValue(APPLY_BY);                       // TT#1966-MD - AGallagher - DC Fulfillment
                        this.WITHIN_DC.SetValue(WITHIN_DC);                       // TT#1966-MD - AGallagher - DC Fulfillment
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_LICENSE_KEY_READ_def MID_LICENSE_KEY_READ = new MID_LICENSE_KEY_READ_def();
            public class MID_LICENSE_KEY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_LICENSE_KEY_READ.SQL"

                private intParameter SYSTEM_OPTIONS_RID;

                public MID_LICENSE_KEY_READ_def()
                {
                    base.procedureName = "MID_LICENSE_KEY_READ";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("UDF_MID_GET_LICENSE_KEY");
                    SYSTEM_OPTIONS_RID = new intParameter("@SYSTEM_OPTIONS_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? SYSTEM_OPTIONS_RID)
                {
                    lock (typeof(MID_LICENSE_KEY_READ_def))
                    {
                        this.SYSTEM_OPTIONS_RID.SetValue(SYSTEM_OPTIONS_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }
}
