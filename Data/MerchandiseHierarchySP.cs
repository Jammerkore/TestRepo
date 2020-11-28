using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class MerchandiseHierarchyData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_POSTING_DATE_READ_ALL_def MID_POSTING_DATE_READ_ALL = new MID_POSTING_DATE_READ_ALL_def();
            public class MID_POSTING_DATE_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_DATE_READ_ALL.SQL"

                public MID_POSTING_DATE_READ_ALL_def()
                {
                    base.procedureName = "MID_POSTING_DATE_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("POSTING_DATE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_POSTING_DATE_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_READ_FROM_OWNER_def MID_PRODUCT_HIERARCHY_READ_FROM_OWNER = new MID_PRODUCT_HIERARCHY_READ_FROM_OWNER_def();
            public class MID_PRODUCT_HIERARCHY_READ_FROM_OWNER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_READ_FROM_OWNER.SQL"

                private intParameter PH_OWNER;

                public MID_PRODUCT_HIERARCHY_READ_FROM_OWNER_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_READ_FROM_OWNER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    PH_OWNER = new intParameter("@PH_OWNER", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PH_OWNER)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_READ_FROM_OWNER_def))
                    {
                        this.PH_OWNER.SetValue(PH_OWNER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //Begin TT#5245-Purge error number 2-BonTon
            public static MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER_def MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER = new MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER_def();
            public class MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER.SQL"

                private intParameter PH_OWNER;

                public MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    PH_OWNER = new intParameter("@PH_OWNER", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PH_OWNER)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_IN_USE_FROM_OWNER_def))
                    {
                        this.PH_OWNER.SetValue(PH_OWNER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#5245-Purge error number 2-BonTon

            public static MID_POSTING_DATE_READ_COUNT_def MID_POSTING_DATE_READ_COUNT = new MID_POSTING_DATE_READ_COUNT_def();
            public class MID_POSTING_DATE_READ_COUNT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_DATE_READ_COUNT.SQL"

                private intParameter PH_RID;

                public MID_POSTING_DATE_READ_COUNT_def()
                {
                    base.procedureName = "MID_POSTING_DATE_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("POSTING_DATE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public int ReadRecordCount(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_POSTING_DATE_READ_COUNT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_POSTING_DATE_INSERT_def MID_POSTING_DATE_INSERT = new MID_POSTING_DATE_INSERT_def();
            public class MID_POSTING_DATE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_DATE_INSERT.SQL"

                private intParameter PH_RID;
                private datetimeParameter POSTINGDATE;
                private intParameter POSTINGDATEYYYYDDD;
                private intParameter POSTINGDATEOFFSET;
                private datetimeParameter CURRENTDATE;
                private intParameter CURRENTDATEYYYYDDD;
                private intParameter CURRENTDATEOFFSET;

                public MID_POSTING_DATE_INSERT_def()
                {
                    base.procedureName = "MID_POSTING_DATE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("POSTING_DATE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    POSTINGDATE = new datetimeParameter("@POSTINGDATE", base.inputParameterList);
                    POSTINGDATEYYYYDDD = new intParameter("@POSTINGDATEYYYYDDD", base.inputParameterList);
                    POSTINGDATEOFFSET = new intParameter("@POSTINGDATEOFFSET", base.inputParameterList);
                    CURRENTDATE = new datetimeParameter("@CURRENTDATE", base.inputParameterList);
                    CURRENTDATEYYYYDDD = new intParameter("@CURRENTDATEYYYYDDD", base.inputParameterList);
                    CURRENTDATEOFFSET = new intParameter("@CURRENTDATEOFFSET", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  DateTime? POSTINGDATE,
                                  int? POSTINGDATEYYYYDDD,
                                  int? POSTINGDATEOFFSET,
                                  DateTime? CURRENTDATE,
                                  int? CURRENTDATEYYYYDDD,
                                  int? CURRENTDATEOFFSET
                                  )
                {
                    lock (typeof(MID_POSTING_DATE_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.POSTINGDATE.SetValue(POSTINGDATE);
                        this.POSTINGDATEYYYYDDD.SetValue(POSTINGDATEYYYYDDD);
                        this.POSTINGDATEOFFSET.SetValue(POSTINGDATEOFFSET);
                        this.CURRENTDATE.SetValue(CURRENTDATE);
                        this.CURRENTDATEYYYYDDD.SetValue(CURRENTDATEYYYYDDD);
                        this.CURRENTDATEOFFSET.SetValue(CURRENTDATEOFFSET);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_POSTING_DATE_UPDATE_def MID_POSTING_DATE_UPDATE = new MID_POSTING_DATE_UPDATE_def();
            public class MID_POSTING_DATE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_DATE_UPDATE.SQL"

                private intParameter PH_RID;
                private datetimeParameter POSTINGDATE;
                private intParameter POSTINGDATEYYYYDDD;
                private intParameter POSTINGDATEOFFSET;
                private datetimeParameter CURRENTDATE;
                private intParameter CURRENTDATEYYYYDDD;
                private intParameter CURRENTDATEOFFSET;

                public MID_POSTING_DATE_UPDATE_def()
                {
                    base.procedureName = "MID_POSTING_DATE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("POSTING_DATE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    POSTINGDATE = new datetimeParameter("@POSTINGDATE", base.inputParameterList);
                    POSTINGDATEYYYYDDD = new intParameter("@POSTINGDATEYYYYDDD", base.inputParameterList);
                    POSTINGDATEOFFSET = new intParameter("@POSTINGDATEOFFSET", base.inputParameterList);
                    CURRENTDATE = new datetimeParameter("@CURRENTDATE", base.inputParameterList);
                    CURRENTDATEYYYYDDD = new intParameter("@CURRENTDATEYYYYDDD", base.inputParameterList);
                    CURRENTDATEOFFSET = new intParameter("@CURRENTDATEOFFSET", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  DateTime? POSTINGDATE,
                                  int? POSTINGDATEYYYYDDD,
                                  int? POSTINGDATEOFFSET,
                                  DateTime? CURRENTDATE,
                                  int? CURRENTDATEYYYYDDD,
                                  int? CURRENTDATEOFFSET
                                  )
                {
                    lock (typeof(MID_POSTING_DATE_UPDATE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.POSTINGDATE.SetValue(POSTINGDATE);
                        this.POSTINGDATEYYYYDDD.SetValue(POSTINGDATEYYYYDDD);
                        this.POSTINGDATEOFFSET.SetValue(POSTINGDATEOFFSET);
                        this.CURRENTDATE.SetValue(CURRENTDATE);
                        this.CURRENTDATEYYYYDDD.SetValue(CURRENTDATEYYYYDDD);
                        this.CURRENTDATEOFFSET.SetValue(CURRENTDATEOFFSET);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_READ_def MID_PRODUCT_HIERARCHY_READ = new MID_PRODUCT_HIERARCHY_READ_def();
            public class MID_PRODUCT_HIERARCHY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_READ.SQL"

                public MID_PRODUCT_HIERARCHY_READ_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_READ_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //Begin TT#1313-MD -jsobek -Header Filters
            public static MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID_def MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID = new MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID_def();
            public class MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID.SQL"

                public MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_READ_ORGANIZATIONAL_RID_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1313-MD -jsobek -Header Filters
            public static SP_MID_HIER_INSERT_def SP_MID_HIER_INSERT = new SP_MID_HIER_INSERT_def();
            public class SP_MID_HIER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIER_INSERT.SQL"

                private stringParameter PH_ID;
                private intParameter PH_TYPE;
                private stringParameter PH_COLOR;
                private intParameter PH_OWNER;
                private intParameter OTS_PLANLEVEL_TYPE;
                private intParameter HISTORY_ROLL_OPTION;
                private intParameter PH_RID; //Declare Output Parameter

                public SP_MID_HIER_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIER_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    PH_ID = new stringParameter("@PH_ID", base.inputParameterList);
                    PH_TYPE = new intParameter("@PH_TYPE", base.inputParameterList);
                    PH_COLOR = new stringParameter("@PH_COLOR", base.inputParameterList);
                    PH_OWNER = new intParameter("@PH_OWNER", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    HISTORY_ROLL_OPTION = new intParameter("@HISTORY_ROLL_OPTION", base.inputParameterList);

                    PH_RID = new intParameter("@PH_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string PH_ID,
                                              int? PH_TYPE,
                                              string PH_COLOR,
                                              int? PH_OWNER,
                                              int? OTS_PLANLEVEL_TYPE,
                                              int? HISTORY_ROLL_OPTION
                                              )
                {
                    lock (typeof(SP_MID_HIER_INSERT_def))
                    {
                        this.PH_ID.SetValue(PH_ID);
                        this.PH_TYPE.SetValue(PH_TYPE);
                        this.PH_COLOR.SetValue(PH_COLOR);
                        this.PH_OWNER.SetValue(PH_OWNER);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.HISTORY_ROLL_OPTION.SetValue(HISTORY_ROLL_OPTION);
                        this.PH_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_UPDATE_def MID_PRODUCT_HIERARCHY_UPDATE = new MID_PRODUCT_HIERARCHY_UPDATE_def();
            public class MID_PRODUCT_HIERARCHY_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_UPDATE.SQL"

                private intParameter PH_RID;
                private stringParameter PH_ID;
                private intParameter PH_TYPE;
                private stringParameter PH_COLOR;
                private intParameter PH_OWNER;
                private intParameter OTS_PLANLEVEL_TYPE;
                private intParameter HISTORY_ROLL_OPTION;

                public MID_PRODUCT_HIERARCHY_UPDATE_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PH_ID = new stringParameter("@PH_ID", base.inputParameterList);
                    PH_TYPE = new intParameter("@PH_TYPE", base.inputParameterList);
                    PH_COLOR = new stringParameter("@PH_COLOR", base.inputParameterList);
                    PH_OWNER = new intParameter("@PH_OWNER", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    HISTORY_ROLL_OPTION = new intParameter("@HISTORY_ROLL_OPTION", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  string PH_ID,
                                  int? PH_TYPE,
                                  string PH_COLOR,
                                  int? PH_OWNER,
                                  int? OTS_PLANLEVEL_TYPE,
                                  int? HISTORY_ROLL_OPTION
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_UPDATE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PH_ID.SetValue(PH_ID);
                        this.PH_TYPE.SetValue(PH_TYPE);
                        this.PH_COLOR.SetValue(PH_COLOR);
                        this.PH_OWNER.SetValue(PH_OWNER);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.HISTORY_ROLL_OPTION.SetValue(HISTORY_ROLL_OPTION);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_DELETE_def MID_PRODUCT_HIERARCHY_DELETE = new MID_PRODUCT_HIERARCHY_DELETE_def();
            public class MID_PRODUCT_HIERARCHY_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_DELETE.SQL"

                private intParameter PH_RID;

                public MID_PRODUCT_HIERARCHY_DELETE_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_DELETE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_READ_def MID_PRODUCT_HIERARCHY_LEVELS_READ = new MID_PRODUCT_HIERARCHY_LEVELS_READ_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_READ.SQL"

                private intParameter PH_RID;

                public MID_PRODUCT_HIERARCHY_LEVELS_READ_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_READ_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ_def MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ = new MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;

                public MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? PH_RID,
                                      int? PHL_SEQUENCE
                                      )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_READ_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_INSERT_def MID_PRODUCT_HIERARCHY_LEVELS_INSERT = new MID_PRODUCT_HIERARCHY_LEVELS_INSERT_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private stringParameter PHL_ID;
                private stringParameter PHL_COLOR;
                private intParameter PHL_TYPE;
                private intParameter LENGTH_TYPE;
                private intParameter REQUIRED_SIZE;
                private intParameter SIZE_RANGE_FROM;
                private intParameter SIZE_RANGE_TO;
                private intParameter OTS_PLANLEVEL_TYPE;
                private intParameter PHL_DISPLAY_OPTION_ID;
                private intParameter PHL_ID_FORMAT;
                private intParameter PURGE_DAILY_HISTORY_TIMEFRAME;
                private intParameter PURGE_DAILY_HISTORY;
                private intParameter PURGE_WEEKLY_HISTORY_TIMEFRAME;
                private intParameter PURGE_WEEKLY_HISTORY;
                private intParameter PURGE_PLANS_TIMEFRAME;
                private intParameter PURGE_PLANS;

                public MID_PRODUCT_HIERARCHY_LEVELS_INSERT_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    PHL_ID = new stringParameter("@PHL_ID", base.inputParameterList);
                    PHL_COLOR = new stringParameter("@PHL_COLOR", base.inputParameterList);
                    PHL_TYPE = new intParameter("@PHL_TYPE", base.inputParameterList);
                    LENGTH_TYPE = new intParameter("@LENGTH_TYPE", base.inputParameterList);
                    REQUIRED_SIZE = new intParameter("@REQUIRED_SIZE", base.inputParameterList);
                    SIZE_RANGE_FROM = new intParameter("@SIZE_RANGE_FROM", base.inputParameterList);
                    SIZE_RANGE_TO = new intParameter("@SIZE_RANGE_TO", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    PHL_DISPLAY_OPTION_ID = new intParameter("@PHL_DISPLAY_OPTION_ID", base.inputParameterList);
                    PHL_ID_FORMAT = new intParameter("@PHL_ID_FORMAT", base.inputParameterList);
                    PURGE_DAILY_HISTORY_TIMEFRAME = new intParameter("@PURGE_DAILY_HISTORY_TIMEFRAME", base.inputParameterList);
                    PURGE_DAILY_HISTORY = new intParameter("@PURGE_DAILY_HISTORY", base.inputParameterList);
                    PURGE_WEEKLY_HISTORY_TIMEFRAME = new intParameter("@PURGE_WEEKLY_HISTORY_TIMEFRAME", base.inputParameterList);
                    PURGE_WEEKLY_HISTORY = new intParameter("@PURGE_WEEKLY_HISTORY", base.inputParameterList);
                    PURGE_PLANS_TIMEFRAME = new intParameter("@PURGE_PLANS_TIMEFRAME", base.inputParameterList);
                    PURGE_PLANS = new intParameter("@PURGE_PLANS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  string PHL_ID,
                                  string PHL_COLOR,
                                  int? PHL_TYPE,
                                  int? LENGTH_TYPE,
                                  int? REQUIRED_SIZE,
                                  int? SIZE_RANGE_FROM,
                                  int? SIZE_RANGE_TO,
                                  int? OTS_PLANLEVEL_TYPE,
                                  int? PHL_DISPLAY_OPTION_ID,
                                  int? PHL_ID_FORMAT,
                                  int? PURGE_DAILY_HISTORY_TIMEFRAME,
                                  int? PURGE_DAILY_HISTORY,
                                  int? PURGE_WEEKLY_HISTORY_TIMEFRAME,
                                  int? PURGE_WEEKLY_HISTORY,
                                  int? PURGE_PLANS_TIMEFRAME,
                                  int? PURGE_PLANS
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.PHL_ID.SetValue(PHL_ID);
                        this.PHL_COLOR.SetValue(PHL_COLOR);
                        this.PHL_TYPE.SetValue(PHL_TYPE);
                        this.LENGTH_TYPE.SetValue(LENGTH_TYPE);
                        this.REQUIRED_SIZE.SetValue(REQUIRED_SIZE);
                        this.SIZE_RANGE_FROM.SetValue(SIZE_RANGE_FROM);
                        this.SIZE_RANGE_TO.SetValue(SIZE_RANGE_TO);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.PHL_DISPLAY_OPTION_ID.SetValue(PHL_DISPLAY_OPTION_ID);
                        this.PHL_ID_FORMAT.SetValue(PHL_ID_FORMAT);
                        this.PURGE_DAILY_HISTORY_TIMEFRAME.SetValue(PURGE_DAILY_HISTORY_TIMEFRAME);
                        this.PURGE_DAILY_HISTORY.SetValue(PURGE_DAILY_HISTORY);
                        this.PURGE_WEEKLY_HISTORY_TIMEFRAME.SetValue(PURGE_WEEKLY_HISTORY_TIMEFRAME);
                        this.PURGE_WEEKLY_HISTORY.SetValue(PURGE_WEEKLY_HISTORY);
                        this.PURGE_PLANS_TIMEFRAME.SetValue(PURGE_PLANS_TIMEFRAME);
                        this.PURGE_PLANS.SetValue(PURGE_PLANS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT_def MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT = new MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter HEADER_TYPE;
                private intParameter PURGE_HEADERS_TIMEFRAME;
                private intParameter PURGE_HEADERS;

                public MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    HEADER_TYPE = new intParameter("@HEADER_TYPE", base.inputParameterList);
                    PURGE_HEADERS_TIMEFRAME = new intParameter("@PURGE_HEADERS_TIMEFRAME", base.inputParameterList);
                    PURGE_HEADERS = new intParameter("@PURGE_HEADERS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  int? HEADER_TYPE,
                                  int? PURGE_HEADERS_TIMEFRAME,
                                  int? PURGE_HEADERS
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.HEADER_TYPE.SetValue(HEADER_TYPE);
                        this.PURGE_HEADERS_TIMEFRAME.SetValue(PURGE_HEADERS_TIMEFRAME);
                        this.PURGE_HEADERS.SetValue(PURGE_HEADERS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_UPDATE_def MID_PRODUCT_HIERARCHY_LEVELS_UPDATE = new MID_PRODUCT_HIERARCHY_LEVELS_UPDATE_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_UPDATE.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private stringParameter PHL_ID;
                private stringParameter PHL_COLOR;
                private intParameter PHL_TYPE;
                private intParameter LENGTH_TYPE;
                private intParameter REQUIRED_SIZE;
                private intParameter SIZE_RANGE_FROM;
                private intParameter SIZE_RANGE_TO;
                private intParameter OTS_PLANLEVEL_TYPE;
                private intParameter PHL_DISPLAY_OPTION_ID;
                private intParameter PHL_ID_FORMAT;
                private intParameter PURGE_DAILY_HISTORY_TIMEFRAME;
                private intParameter PURGE_DAILY_HISTORY;
                private intParameter PURGE_WEEKLY_HISTORY_TIMEFRAME;
                private intParameter PURGE_WEEKLY_HISTORY;
                private intParameter PURGE_PLANS_TIMEFRAME;
                private intParameter PURGE_PLANS;

                public MID_PRODUCT_HIERARCHY_LEVELS_UPDATE_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    PHL_ID = new stringParameter("@PHL_ID", base.inputParameterList);
                    PHL_COLOR = new stringParameter("@PHL_COLOR", base.inputParameterList);
                    PHL_TYPE = new intParameter("@PHL_TYPE", base.inputParameterList);
                    LENGTH_TYPE = new intParameter("@LENGTH_TYPE", base.inputParameterList);
                    REQUIRED_SIZE = new intParameter("@REQUIRED_SIZE", base.inputParameterList);
                    SIZE_RANGE_FROM = new intParameter("@SIZE_RANGE_FROM", base.inputParameterList);
                    SIZE_RANGE_TO = new intParameter("@SIZE_RANGE_TO", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    PHL_DISPLAY_OPTION_ID = new intParameter("@PHL_DISPLAY_OPTION_ID", base.inputParameterList);
                    PHL_ID_FORMAT = new intParameter("@PHL_ID_FORMAT", base.inputParameterList);
                    PURGE_DAILY_HISTORY_TIMEFRAME = new intParameter("@PURGE_DAILY_HISTORY_TIMEFRAME", base.inputParameterList);
                    PURGE_DAILY_HISTORY = new intParameter("@PURGE_DAILY_HISTORY", base.inputParameterList);
                    PURGE_WEEKLY_HISTORY_TIMEFRAME = new intParameter("@PURGE_WEEKLY_HISTORY_TIMEFRAME", base.inputParameterList);
                    PURGE_WEEKLY_HISTORY = new intParameter("@PURGE_WEEKLY_HISTORY", base.inputParameterList);
                    PURGE_PLANS_TIMEFRAME = new intParameter("@PURGE_PLANS_TIMEFRAME", base.inputParameterList);
                    PURGE_PLANS = new intParameter("@PURGE_PLANS", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  string PHL_ID,
                                  string PHL_COLOR,
                                  int? PHL_TYPE,
                                  int? LENGTH_TYPE,
                                  int? REQUIRED_SIZE,
                                  int? SIZE_RANGE_FROM,
                                  int? SIZE_RANGE_TO,
                                  int? OTS_PLANLEVEL_TYPE,
                                  int? PHL_DISPLAY_OPTION_ID,
                                  int? PHL_ID_FORMAT,
                                  int? PURGE_DAILY_HISTORY_TIMEFRAME,
                                  int? PURGE_DAILY_HISTORY,
                                  int? PURGE_WEEKLY_HISTORY_TIMEFRAME,
                                  int? PURGE_WEEKLY_HISTORY,
                                  int? PURGE_PLANS_TIMEFRAME,
                                  int? PURGE_PLANS
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_UPDATE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.PHL_ID.SetValue(PHL_ID);
                        this.PHL_COLOR.SetValue(PHL_COLOR);
                        this.PHL_TYPE.SetValue(PHL_TYPE);
                        this.LENGTH_TYPE.SetValue(LENGTH_TYPE);
                        this.REQUIRED_SIZE.SetValue(REQUIRED_SIZE);
                        this.SIZE_RANGE_FROM.SetValue(SIZE_RANGE_FROM);
                        this.SIZE_RANGE_TO.SetValue(SIZE_RANGE_TO);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.PHL_DISPLAY_OPTION_ID.SetValue(PHL_DISPLAY_OPTION_ID);
                        this.PHL_ID_FORMAT.SetValue(PHL_ID_FORMAT);
                        this.PURGE_DAILY_HISTORY_TIMEFRAME.SetValue(PURGE_DAILY_HISTORY_TIMEFRAME);
                        this.PURGE_DAILY_HISTORY.SetValue(PURGE_DAILY_HISTORY);
                        this.PURGE_WEEKLY_HISTORY_TIMEFRAME.SetValue(PURGE_WEEKLY_HISTORY_TIMEFRAME);
                        this.PURGE_WEEKLY_HISTORY.SetValue(PURGE_WEEKLY_HISTORY);
                        this.PURGE_PLANS_TIMEFRAME.SetValue(PURGE_PLANS_TIMEFRAME);
                        this.PURGE_PLANS.SetValue(PURGE_PLANS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_DELETE_def MID_PRODUCT_HIERARCHY_LEVELS_DELETE = new MID_PRODUCT_HIERARCHY_LEVELS_DELETE_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_DELETE.SQL"

                private intParameter PH_RID;

                public MID_PRODUCT_HIERARCHY_LEVELS_DELETE_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_DELETE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ_def MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ = new MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;

                public MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PHL_SEQUENCE
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_DELETE_WITH_SEQ_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE_def MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE = new MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE_def();
            public class MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE.SQL"

                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;

                public MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE_def()
                {
                    base.procedureName = "MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PHL_SEQUENCE
                                  )
                {
                    lock (typeof(MID_PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE_DELETE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_TYPE_def MID_HIERARCHY_READ_TYPE = new MID_HIERARCHY_READ_TYPE_def();
            public class MID_HIERARCHY_READ_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_TYPE.SQL"

                private intParameter HN_RID;

                public MID_HIERARCHY_READ_TYPE_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIERARCHY_READ_TYPE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_ALL_BASE_def MID_HIERARCHY_READ_ALL_BASE = new MID_HIERARCHY_READ_ALL_BASE_def();
            public class MID_HIERARCHY_READ_ALL_BASE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_ALL_BASE.SQL"

                public MID_HIERARCHY_READ_ALL_BASE_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_ALL_BASE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_HIERARCHY_GET_DATA");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_READ_ALL_BASE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_BASE_FROM_HOME_PH_def MID_HIERARCHY_READ_BASE_FROM_HOME_PH = new MID_HIERARCHY_READ_BASE_FROM_HOME_PH_def();
            public class MID_HIERARCHY_READ_BASE_FROM_HOME_PH_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_BASE_FROM_HOME_PH.SQL"

                private tableParameter HOME_PH_RID_LIST;

                public MID_HIERARCHY_READ_BASE_FROM_HOME_PH_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_BASE_FROM_HOME_PH";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_HIERARCHY_GET_DATA");
                    HOME_PH_RID_LIST = new tableParameter("@HOME_PH_RID_LIST", "HOME_PH_RID_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, DataTable HOME_PH_RID_LIST)
                {
                    lock (typeof(MID_HIERARCHY_READ_BASE_FROM_HOME_PH_def))
                    {
                        this.HOME_PH_RID_LIST.SetValue(HOME_PH_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_INFO_FROM_BASE_NODE_def MID_HIERARCHY_GET_INFO_FROM_BASE_NODE = new MID_HIERARCHY_GET_INFO_FROM_BASE_NODE_def();
            public class MID_HIERARCHY_GET_INFO_FROM_BASE_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_INFO_FROM_BASE_NODE.SQL"

                private intParameter HN_RID;

                public MID_HIERARCHY_GET_INFO_FROM_BASE_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_INFO_FROM_BASE_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_HIERARCHY_GET_DATA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_INFO_FROM_BASE_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_BASE_NODE_READ_FROM_ID_def MID_BASE_NODE_READ_FROM_ID = new MID_BASE_NODE_READ_FROM_ID_def();
            public class MID_BASE_NODE_READ_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BASE_NODE_READ_FROM_ID.SQL"

                private stringParameter BN_ID;

                public MID_BASE_NODE_READ_FROM_ID_def()
                {
                    base.procedureName = "MID_BASE_NODE_READ_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("BASE_NODE");
                    BN_ID = new stringParameter("@BN_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string BN_ID)
                {
                    lock (typeof(MID_BASE_NODE_READ_FROM_ID_def))
                    {
                        this.BN_ID.SetValue(BN_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_BASE_NODE_SEARCH_READ_def MID_BASE_NODE_SEARCH_READ = new MID_BASE_NODE_SEARCH_READ_def();
            public class MID_BASE_NODE_SEARCH_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BASE_NODE_SEARCH_READ.SQL"

                private stringParameter BN_SEARCH_STRING;

                public MID_BASE_NODE_SEARCH_READ_def()
                {
                    base.procedureName = "MID_BASE_NODE_SEARCH_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("BASE_NODE");
                    BN_SEARCH_STRING = new stringParameter("@BN_SEARCH_STRING", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string BN_SEARCH_STRING)
                {
                    lock (typeof(MID_BASE_NODE_SEARCH_READ_def))
                    {
                        this.BN_SEARCH_STRING.SetValue(BN_SEARCH_STRING);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE_def MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE = new MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE_def();
            public class MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE.SQL"

                private intParameter hierarchyRID;
                private intParameter HN_RID;

                public MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("BASE_NODE");
                    hierarchyRID = new intParameter("@hierarchyRID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? hierarchyRID,
                                      int? HN_RID
                                      )
                {
                    lock (typeof(MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE_def))
                    {
                        this.hierarchyRID.SetValue(hierarchyRID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE_def MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE = new MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE_def();
            public class MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE.SQL"

                private intParameter HN_RID;

                public MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_HIERARCHY_GET_DATA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_FROM_HOME_PH_def MID_HIERARCHY_READ_FROM_HOME_PH = new MID_HIERARCHY_READ_FROM_HOME_PH_def();
            public class MID_HIERARCHY_READ_FROM_HOME_PH_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_FROM_HOME_PH.SQL"

                private intParameter HOME_PH_RID;

                public MID_HIERARCHY_READ_FROM_HOME_PH_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_FROM_HOME_PH";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY_NODE");
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HOME_PH_RID)
                {
                    lock (typeof(MID_HIERARCHY_READ_FROM_HOME_PH_def))
                    {
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_UPDATE_def MID_HIERARCHY_UPDATE = new MID_HIERARCHY_UPDATE_def();
            public class MID_HIERARCHY_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_UPDATE.SQL"

                private intParameter HN_RID;
                private intParameter HOME_PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter HN_TYPE;
                private intParameter OTS_PLANLEVEL_TYPE;
                private charParameter USE_BASIC_REPLENISHMENT;
                private intParameter OTS_FORECAST_LEVEL_SELECT_TYPE;
                private intParameter OTS_FORECAST_LEVEL_TYPE;
                private intParameter OTS_FORECAST_LEVEL_PH_RID;
                private intParameter OTS_FORECAST_LEVEL_PHL_SEQUENCE;
                private intParameter OTS_FORECAST_LEVEL_ANCHOR_NODE;
                private intParameter OTS_FORECAST_LEVEL_MASK_FIELD;
                private stringParameter OTS_FORECAST_LEVEL_MASK;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;
                private intParameter DIGITAL_ASSET_KEY;

                public MID_HIERARCHY_UPDATE_def()
                {
                    base.procedureName = "MID_HIERARCHY_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HIERARCHY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                    HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
                    HN_TYPE = new intParameter("@HN_TYPE", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    USE_BASIC_REPLENISHMENT = new charParameter("@USE_BASIC_REPLENISHMENT", base.inputParameterList);
                    OTS_FORECAST_LEVEL_SELECT_TYPE = new intParameter("@OTS_FORECAST_LEVEL_SELECT_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_TYPE = new intParameter("@OTS_FORECAST_LEVEL_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PH_RID = new intParameter("@OTS_FORECAST_LEVEL_PH_RID", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE = new intParameter("@OTS_FORECAST_LEVEL_PHL_SEQUENCE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_ANCHOR_NODE = new intParameter("@OTS_FORECAST_LEVEL_ANCHOR_NODE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK_FIELD = new intParameter("@OTS_FORECAST_LEVEL_MASK_FIELD", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK = new stringParameter("@OTS_FORECAST_LEVEL_MASK", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);
                    DIGITAL_ASSET_KEY = new intParameter("@DIGITAL_ASSET_KEY", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? HOME_PH_RID,
                                  int? HOME_LEVEL,
                                  int? HN_TYPE,
                                  int? OTS_PLANLEVEL_TYPE,
                                  char? USE_BASIC_REPLENISHMENT,
                                  int? OTS_FORECAST_LEVEL_SELECT_TYPE,
                                  int? OTS_FORECAST_LEVEL_TYPE,
                                  int? OTS_FORECAST_LEVEL_PH_RID,
                                  int? OTS_FORECAST_LEVEL_PHL_SEQUENCE,
                                  int? OTS_FORECAST_LEVEL_ANCHOR_NODE,
                                  int? OTS_FORECAST_LEVEL_MASK_FIELD,
                                  string OTS_FORECAST_LEVEL_MASK,
                                  char? VIRTUAL_IND,
                                  int? PURPOSE,
                                  int? DIGITAL_ASSET_KEY
                                  )
                {
                    lock (typeof(MID_HIERARCHY_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.HN_TYPE.SetValue(HN_TYPE);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.USE_BASIC_REPLENISHMENT.SetValue(USE_BASIC_REPLENISHMENT);
                        this.OTS_FORECAST_LEVEL_SELECT_TYPE.SetValue(OTS_FORECAST_LEVEL_SELECT_TYPE);
                        this.OTS_FORECAST_LEVEL_TYPE.SetValue(OTS_FORECAST_LEVEL_TYPE);
                        this.OTS_FORECAST_LEVEL_PH_RID.SetValue(OTS_FORECAST_LEVEL_PH_RID);
                        this.OTS_FORECAST_LEVEL_PHL_SEQUENCE.SetValue(OTS_FORECAST_LEVEL_PHL_SEQUENCE);
                        this.OTS_FORECAST_LEVEL_ANCHOR_NODE.SetValue(OTS_FORECAST_LEVEL_ANCHOR_NODE);
                        this.OTS_FORECAST_LEVEL_MASK_FIELD.SetValue(OTS_FORECAST_LEVEL_MASK_FIELD);
                        this.OTS_FORECAST_LEVEL_MASK.SetValue(OTS_FORECAST_LEVEL_MASK);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        this.DIGITAL_ASSET_KEY.SetValue(DIGITAL_ASSET_KEY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_HIERNODE_DELETE_def SP_MID_HIERNODE_DELETE = new SP_MID_HIERNODE_DELETE_def();
            public class SP_MID_HIERNODE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERNODE_DELETE.SQL"

                private intParameter HN_RID;

                public SP_MID_HIERNODE_DELETE_def()
                {
                    base.procedureName = "SP_MID_HIERNODE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_HIERNODE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HIERNODE_REPLACE_def SP_MID_HIERNODE_REPLACE = new SP_MID_HIERNODE_REPLACE_def();
            public class SP_MID_HIERNODE_REPLACE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERNODE_REPLACE.SQL"

                private intParameter HN_RID;
                private intParameter REPLACE_HN_RID;

                public SP_MID_HIERNODE_REPLACE_def()
                {
                    base.procedureName = "SP_MID_HIERNODE_REPLACE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    REPLACE_HN_RID = new intParameter("@REPLACE_HN_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? REPLACE_HN_RID
                                  )
                {
                    lock (typeof(SP_MID_HIERNODE_REPLACE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.REPLACE_HN_RID.SetValue(REPLACE_HN_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_HIERNODE_BASE_INSERT_def SP_MID_HIERNODE_BASE_INSERT = new SP_MID_HIERNODE_BASE_INSERT_def();
            public class SP_MID_HIERNODE_BASE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERNODE_BASE_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_RID;
                private intParameter HOME_PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter HN_TYPE;
                private intParameter OTS_PLANLEVEL_TYPE;
                private charParameter USE_BASIC_REPLENISHMENT;
                private intParameter OTS_FORECAST_LEVEL_SELECT_TYPE;
                private intParameter OTS_FORECAST_LEVEL_TYPE;
                private intParameter OTS_FORECAST_LEVEL_PH_RID;
                private intParameter OTS_FORECAST_LEVEL_PHL_SEQUENCE;
                private intParameter OTS_FORECAST_LEVEL_ANCHOR_NODE;
                private intParameter OTS_FORECAST_LEVEL_MASK_FIELD;
                private stringParameter OTS_FORECAST_LEVEL_MASK;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;
                private stringParameter NODE_ID;
                private stringParameter NODE_NAME;
                private stringParameter NODE_DESCRIPTION;
                private intParameter PRODUCT_TYPE;
                private intParameter DIGITAL_ASSET_KEY;
                private intParameter HN_RID; //Declare Output Parameter

                public SP_MID_HIERNODE_BASE_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIERNODE_BASE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_RID = new intParameter("@PARENT_RID", base.inputParameterList);
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                    HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
                    HN_TYPE = new intParameter("@HN_TYPE", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    USE_BASIC_REPLENISHMENT = new charParameter("@USE_BASIC_REPLENISHMENT", base.inputParameterList);
                    OTS_FORECAST_LEVEL_SELECT_TYPE = new intParameter("@OTS_FORECAST_LEVEL_SELECT_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_TYPE = new intParameter("@OTS_FORECAST_LEVEL_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PH_RID = new intParameter("@OTS_FORECAST_LEVEL_PH_RID", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE = new intParameter("@OTS_FORECAST_LEVEL_PHL_SEQUENCE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_ANCHOR_NODE = new intParameter("@OTS_FORECAST_LEVEL_ANCHOR_NODE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK_FIELD = new intParameter("@OTS_FORECAST_LEVEL_MASK_FIELD", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK = new stringParameter("@OTS_FORECAST_LEVEL_MASK", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);
                    NODE_ID = new stringParameter("@NODE_ID", base.inputParameterList);
                    NODE_NAME = new stringParameter("@NODE_NAME", base.inputParameterList);
                    NODE_DESCRIPTION = new stringParameter("@NODE_DESCRIPTION", base.inputParameterList);
                    PRODUCT_TYPE = new intParameter("@PRODUCT_TYPE", base.inputParameterList);
                    DIGITAL_ASSET_KEY = new intParameter("@DIGITAL_ASSET_KEY", base.inputParameterList);

                    HN_RID = new intParameter("@HN_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? PH_RID,
                                              int? PARENT_RID,
                                              int? HOME_PH_RID,
                                              int? HOME_LEVEL,
                                              int? HN_TYPE,
                                              int? OTS_PLANLEVEL_TYPE,
                                              char? USE_BASIC_REPLENISHMENT,
                                              int? OTS_FORECAST_LEVEL_SELECT_TYPE,
                                              int? OTS_FORECAST_LEVEL_TYPE,
                                              int? OTS_FORECAST_LEVEL_PH_RID,
                                              int? OTS_FORECAST_LEVEL_PHL_SEQUENCE,
                                              int? OTS_FORECAST_LEVEL_ANCHOR_NODE,
                                              int? OTS_FORECAST_LEVEL_MASK_FIELD,
                                              string OTS_FORECAST_LEVEL_MASK,
                                              char? VIRTUAL_IND,
                                              int? PURPOSE,
                                              string NODE_ID,
                                              string NODE_NAME,
                                              string NODE_DESCRIPTION,
                                              int? PRODUCT_TYPE,
                                              int? DIGITAL_ASSET_KEY
                                              )
                {
                    lock (typeof(SP_MID_HIERNODE_BASE_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_RID.SetValue(PARENT_RID);
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.HN_TYPE.SetValue(HN_TYPE);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.USE_BASIC_REPLENISHMENT.SetValue(USE_BASIC_REPLENISHMENT);
                        this.OTS_FORECAST_LEVEL_SELECT_TYPE.SetValue(OTS_FORECAST_LEVEL_SELECT_TYPE);
                        this.OTS_FORECAST_LEVEL_TYPE.SetValue(OTS_FORECAST_LEVEL_TYPE);
                        this.OTS_FORECAST_LEVEL_PH_RID.SetValue(OTS_FORECAST_LEVEL_PH_RID);
                        this.OTS_FORECAST_LEVEL_PHL_SEQUENCE.SetValue(OTS_FORECAST_LEVEL_PHL_SEQUENCE);
                        this.OTS_FORECAST_LEVEL_ANCHOR_NODE.SetValue(OTS_FORECAST_LEVEL_ANCHOR_NODE);
                        this.OTS_FORECAST_LEVEL_MASK_FIELD.SetValue(OTS_FORECAST_LEVEL_MASK_FIELD);
                        this.OTS_FORECAST_LEVEL_MASK.SetValue(OTS_FORECAST_LEVEL_MASK);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        this.NODE_ID.SetValue(NODE_ID);
                        this.NODE_NAME.SetValue(NODE_NAME);
                        this.NODE_DESCRIPTION.SetValue(NODE_DESCRIPTION);
                        this.PRODUCT_TYPE.SetValue(PRODUCT_TYPE);
                        this.DIGITAL_ASSET_KEY.SetValue(DIGITAL_ASSET_KEY);

                        this.HN_RID.SetValue(null); //Initialize Output Parameter
                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_HIERNODE_COLOR_INSERT_def SP_MID_HIERNODE_COLOR_INSERT = new SP_MID_HIERNODE_COLOR_INSERT_def();
            public class SP_MID_HIERNODE_COLOR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERNODE_COLOR_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_RID;
                private intParameter HOME_PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter HN_TYPE;
                private intParameter OTS_PLANLEVEL_TYPE;
                private charParameter USE_BASIC_REPLENISHMENT;
                private intParameter OTS_FORECAST_LEVEL_SELECT_TYPE;
                private intParameter OTS_FORECAST_LEVEL_TYPE;
                private intParameter OTS_FORECAST_LEVEL_PH_RID;
                private intParameter OTS_FORECAST_LEVEL_PHL_SEQUENCE;
                private intParameter OTS_FORECAST_LEVEL_ANCHOR_NODE;
                private intParameter OTS_FORECAST_LEVEL_MASK_FIELD;
                private stringParameter OTS_FORECAST_LEVEL_MASK;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;
                private intParameter COLOR_CODE_RID;
                private stringParameter COLOR_DESCRIPTION;
                private stringParameter STYLE_NODE_ID;
                private intParameter DIGITAL_ASSET_KEY;
                private intParameter HN_RID; //Declare Output Parameter

                public SP_MID_HIERNODE_COLOR_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIERNODE_COLOR_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_RID = new intParameter("@PARENT_RID", base.inputParameterList);
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                    HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
                    HN_TYPE = new intParameter("@HN_TYPE", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    USE_BASIC_REPLENISHMENT = new charParameter("@USE_BASIC_REPLENISHMENT", base.inputParameterList);
                    OTS_FORECAST_LEVEL_SELECT_TYPE = new intParameter("@OTS_FORECAST_LEVEL_SELECT_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_TYPE = new intParameter("@OTS_FORECAST_LEVEL_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PH_RID = new intParameter("@OTS_FORECAST_LEVEL_PH_RID", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE = new intParameter("@OTS_FORECAST_LEVEL_PHL_SEQUENCE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_ANCHOR_NODE = new intParameter("@OTS_FORECAST_LEVEL_ANCHOR_NODE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK_FIELD = new intParameter("@OTS_FORECAST_LEVEL_MASK_FIELD", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK = new stringParameter("@OTS_FORECAST_LEVEL_MASK", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    COLOR_DESCRIPTION = new stringParameter("@COLOR_DESCRIPTION", base.inputParameterList);
                    STYLE_NODE_ID = new stringParameter("@STYLE_NODE_ID", base.inputParameterList);
                    DIGITAL_ASSET_KEY = new intParameter("@DIGITAL_ASSET_KEY", base.inputParameterList);

                    HN_RID = new intParameter("@HN_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? PH_RID,
                                              int? PARENT_RID,
                                              int? HOME_PH_RID,
                                              int? HOME_LEVEL,
                                              int? HN_TYPE,
                                              int? OTS_PLANLEVEL_TYPE,
                                              char? USE_BASIC_REPLENISHMENT,
                                              int? OTS_FORECAST_LEVEL_SELECT_TYPE,
                                              int? OTS_FORECAST_LEVEL_TYPE,
                                              int? OTS_FORECAST_LEVEL_PH_RID,
                                              int? OTS_FORECAST_LEVEL_PHL_SEQUENCE,
                                              int? OTS_FORECAST_LEVEL_ANCHOR_NODE,
                                              int? OTS_FORECAST_LEVEL_MASK_FIELD,
                                              string OTS_FORECAST_LEVEL_MASK,
                                              char? VIRTUAL_IND,
                                              int? PURPOSE,
                                              int? COLOR_CODE_RID,
                                              string COLOR_DESCRIPTION,
                                              string STYLE_NODE_ID,
                                              int? DIGITAL_ASSET_KEY
                                              )
                {
                    lock (typeof(SP_MID_HIERNODE_COLOR_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_RID.SetValue(PARENT_RID);
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.HN_TYPE.SetValue(HN_TYPE);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.USE_BASIC_REPLENISHMENT.SetValue(USE_BASIC_REPLENISHMENT);
                        this.OTS_FORECAST_LEVEL_SELECT_TYPE.SetValue(OTS_FORECAST_LEVEL_SELECT_TYPE);
                        this.OTS_FORECAST_LEVEL_TYPE.SetValue(OTS_FORECAST_LEVEL_TYPE);
                        this.OTS_FORECAST_LEVEL_PH_RID.SetValue(OTS_FORECAST_LEVEL_PH_RID);
                        this.OTS_FORECAST_LEVEL_PHL_SEQUENCE.SetValue(OTS_FORECAST_LEVEL_PHL_SEQUENCE);
                        this.OTS_FORECAST_LEVEL_ANCHOR_NODE.SetValue(OTS_FORECAST_LEVEL_ANCHOR_NODE);
                        this.OTS_FORECAST_LEVEL_MASK_FIELD.SetValue(OTS_FORECAST_LEVEL_MASK_FIELD);
                        this.OTS_FORECAST_LEVEL_MASK.SetValue(OTS_FORECAST_LEVEL_MASK);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.COLOR_DESCRIPTION.SetValue(COLOR_DESCRIPTION);
                        this.STYLE_NODE_ID.SetValue(STYLE_NODE_ID);
                        this.DIGITAL_ASSET_KEY.SetValue(DIGITAL_ASSET_KEY);

                        this.HN_RID.SetValue(null);  //Initialize Output Parameter
                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_HIERNODE_SIZE_INSERT_def SP_MID_HIERNODE_SIZE_INSERT = new SP_MID_HIERNODE_SIZE_INSERT_def();
            public class SP_MID_HIERNODE_SIZE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERNODE_SIZE_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_RID;
                private intParameter HOME_PH_RID;
                private intParameter HOME_LEVEL;
                private intParameter HN_TYPE;
                private intParameter OTS_PLANLEVEL_TYPE;
                private charParameter USE_BASIC_REPLENISHMENT;
                private intParameter OTS_FORECAST_LEVEL_SELECT_TYPE;
                private intParameter OTS_FORECAST_LEVEL_TYPE;
                private intParameter OTS_FORECAST_LEVEL_PH_RID;
                private intParameter OTS_FORECAST_LEVEL_PHL_SEQUENCE;
                private intParameter OTS_FORECAST_LEVEL_ANCHOR_NODE;
                private intParameter OTS_FORECAST_LEVEL_MASK_FIELD;
                private stringParameter OTS_FORECAST_LEVEL_MASK;
                private charParameter VIRTUAL_IND;
                private intParameter PURPOSE;
                private intParameter SIZE_CODE_RID;
                private stringParameter STYLE_NODE_ID;
                private stringParameter COLOR_NODE_ID;
                private intParameter DIGITAL_ASSET_KEY;
                private intParameter HN_RID; //Declare Output Parameter

                public SP_MID_HIERNODE_SIZE_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIERNODE_SIZE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_RID = new intParameter("@PARENT_RID", base.inputParameterList);
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                    HOME_LEVEL = new intParameter("@HOME_LEVEL", base.inputParameterList);
                    HN_TYPE = new intParameter("@HN_TYPE", base.inputParameterList);
                    OTS_PLANLEVEL_TYPE = new intParameter("@OTS_PLANLEVEL_TYPE", base.inputParameterList);
                    USE_BASIC_REPLENISHMENT = new charParameter("@USE_BASIC_REPLENISHMENT", base.inputParameterList);
                    OTS_FORECAST_LEVEL_SELECT_TYPE = new intParameter("@OTS_FORECAST_LEVEL_SELECT_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_TYPE = new intParameter("@OTS_FORECAST_LEVEL_TYPE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PH_RID = new intParameter("@OTS_FORECAST_LEVEL_PH_RID", base.inputParameterList);
                    OTS_FORECAST_LEVEL_PHL_SEQUENCE = new intParameter("@OTS_FORECAST_LEVEL_PHL_SEQUENCE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_ANCHOR_NODE = new intParameter("@OTS_FORECAST_LEVEL_ANCHOR_NODE", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK_FIELD = new intParameter("@OTS_FORECAST_LEVEL_MASK_FIELD", base.inputParameterList);
                    OTS_FORECAST_LEVEL_MASK = new stringParameter("@OTS_FORECAST_LEVEL_MASK", base.inputParameterList);
                    VIRTUAL_IND = new charParameter("@VIRTUAL_IND", base.inputParameterList);
                    PURPOSE = new intParameter("@PURPOSE", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    STYLE_NODE_ID = new stringParameter("@STYLE_NODE_ID", base.inputParameterList);
                    COLOR_NODE_ID = new stringParameter("@COLOR_NODE_ID", base.inputParameterList);
                    DIGITAL_ASSET_KEY = new intParameter("@DIGITAL_ASSET_KEY", base.inputParameterList);

                    HN_RID = new intParameter("@HN_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? PH_RID,
                                              int? PARENT_RID,
                                              int? HOME_PH_RID,
                                              int? HOME_LEVEL,
                                              int? HN_TYPE,
                                              int? OTS_PLANLEVEL_TYPE,
                                              char? USE_BASIC_REPLENISHMENT,
                                              int? OTS_FORECAST_LEVEL_SELECT_TYPE,
                                              int? OTS_FORECAST_LEVEL_TYPE,
                                              int? OTS_FORECAST_LEVEL_PH_RID,
                                              int? OTS_FORECAST_LEVEL_PHL_SEQUENCE,
                                              int? OTS_FORECAST_LEVEL_ANCHOR_NODE,
                                              int? OTS_FORECAST_LEVEL_MASK_FIELD,
                                              string OTS_FORECAST_LEVEL_MASK,
                                              char? VIRTUAL_IND,
                                              int? PURPOSE,
                                              int? SIZE_CODE_RID,
                                              string STYLE_NODE_ID,
                                              string COLOR_NODE_ID,
                                              int? DIGITAL_ASSET_KEY
                                              )
                {
                    lock (typeof(SP_MID_HIERNODE_SIZE_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_RID.SetValue(PARENT_RID);
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        this.HOME_LEVEL.SetValue(HOME_LEVEL);
                        this.HN_TYPE.SetValue(HN_TYPE);
                        this.OTS_PLANLEVEL_TYPE.SetValue(OTS_PLANLEVEL_TYPE);
                        this.USE_BASIC_REPLENISHMENT.SetValue(USE_BASIC_REPLENISHMENT);
                        this.OTS_FORECAST_LEVEL_SELECT_TYPE.SetValue(OTS_FORECAST_LEVEL_SELECT_TYPE);
                        this.OTS_FORECAST_LEVEL_TYPE.SetValue(OTS_FORECAST_LEVEL_TYPE);
                        this.OTS_FORECAST_LEVEL_PH_RID.SetValue(OTS_FORECAST_LEVEL_PH_RID);
                        this.OTS_FORECAST_LEVEL_PHL_SEQUENCE.SetValue(OTS_FORECAST_LEVEL_PHL_SEQUENCE);
                        this.OTS_FORECAST_LEVEL_ANCHOR_NODE.SetValue(OTS_FORECAST_LEVEL_ANCHOR_NODE);
                        this.OTS_FORECAST_LEVEL_MASK_FIELD.SetValue(OTS_FORECAST_LEVEL_MASK_FIELD);
                        this.OTS_FORECAST_LEVEL_MASK.SetValue(OTS_FORECAST_LEVEL_MASK);
                        this.VIRTUAL_IND.SetValue(VIRTUAL_IND);
                        this.PURPOSE.SetValue(PURPOSE);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.STYLE_NODE_ID.SetValue(STYLE_NODE_ID);
                        this.COLOR_NODE_ID.SetValue(COLOR_NODE_ID);
                        this.DIGITAL_ASSET_KEY.SetValue(DIGITAL_ASSET_KEY);

                        this.HN_RID.SetValue(null); //Initialize Output Parameter
                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_BASE_NODE_UPDATE_def MID_BASE_NODE_UPDATE = new MID_BASE_NODE_UPDATE_def();
            public class MID_BASE_NODE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BASE_NODE_UPDATE.SQL"

                private intParameter HN_RID;
                private stringParameter BN_ID;
                private stringParameter BN_NAME;
                private stringParameter BN_DESCRIPTION;
                private intParameter BN_PRODUCT_TYPE;

                public MID_BASE_NODE_UPDATE_def()
                {
                    base.procedureName = "MID_BASE_NODE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("BASE_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BN_ID = new stringParameter("@BN_ID", base.inputParameterList);
                    BN_NAME = new stringParameter("@BN_NAME", base.inputParameterList);
                    BN_DESCRIPTION = new stringParameter("@BN_DESCRIPTION", base.inputParameterList);
                    BN_PRODUCT_TYPE = new intParameter("@BN_PRODUCT_TYPE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  string BN_ID,
                                  string BN_NAME,
                                  string BN_DESCRIPTION,
                                  int? BN_PRODUCT_TYPE
                                  )
                {
                    lock (typeof(MID_BASE_NODE_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BN_ID.SetValue(BN_ID);
                        this.BN_NAME.SetValue(BN_NAME);
                        this.BN_DESCRIPTION.SetValue(BN_DESCRIPTION);
                        this.BN_PRODUCT_TYPE.SetValue(BN_PRODUCT_TYPE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_BASE_NODE_DELETE_def MID_BASE_NODE_DELETE = new MID_BASE_NODE_DELETE_def();
            public class MID_BASE_NODE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BASE_NODE_DELETE.SQL"

                private intParameter HN_RID;

                public MID_BASE_NODE_DELETE_def()
                {
                    base.procedureName = "MID_BASE_NODE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("BASE_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_BASE_NODE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE_def MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE = new MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE_def();
            public class MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE.SQL"

                private intParameter COLOR_CODE_RID;

                public MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY");
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? COLOR_CODE_RID)
                {
                    lock (typeof(MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE_def))
                    {
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE_def MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE = new MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE_def();
            public class MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE.SQL"

                private intParameter HN_RID;

                public MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_INFO_FROM_COLOR_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE_def MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE = new MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE_def();
            public class MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE.SQL"

                private intParameter hierarchyRID;
                private intParameter HN_RID;

                public MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    hierarchyRID = new intParameter("@hierarchyRID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? hierarchyRID,
                                      int? HN_RID
                                      )
                {
                    lock (typeof(MID_HIERARCHY_GET_CHILD_COLOR_INFO_FROM_NODE_def))
                    {
                        this.hierarchyRID.SetValue(hierarchyRID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_PROPERTIES_DELETE_def MID_HIER_NODE_PROPERTIES_DELETE = new MID_HIER_NODE_PROPERTIES_DELETE_def();
            public class MID_HIER_NODE_PROPERTIES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_PROPERTIES_DELETE.SQL"

                private intParameter HN_RID;

                public MID_HIER_NODE_PROPERTIES_DELETE_def()
                {
                    base.procedureName = "MID_HIER_NODE_PROPERTIES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_NODE_PROPERTIES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIER_NODE_PROPERTIES_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_PROPERTIES_INSERT_def MID_HIER_NODE_PROPERTIES_INSERT = new MID_HIER_NODE_PROPERTIES_INSERT_def();
            public class MID_HIER_NODE_PROPERTIES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_PROPERTIES_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter APPLY_HN_RID_FROM;

                public MID_HIER_NODE_PROPERTIES_INSERT_def()
                {
                    base.procedureName = "MID_HIER_NODE_PROPERTIES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HIER_NODE_PROPERTIES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    APPLY_HN_RID_FROM = new intParameter("@APPLY_HN_RID_FROM", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? APPLY_HN_RID_FROM
                                  )
                {
                    lock (typeof(MID_HIER_NODE_PROPERTIES_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.APPLY_HN_RID_FROM.SetValue(APPLY_HN_RID_FROM);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_UPDATE_def MID_COLOR_NODE_UPDATE = new MID_COLOR_NODE_UPDATE_def();
            public class MID_COLOR_NODE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_NODE_UPDATE.SQL"

                private intParameter HN_RID;
                private intParameter COLOR_CODE_RID;
                private stringParameter COLOR_DESCRIPTION;

                public MID_COLOR_NODE_UPDATE_def()
                {
                    base.procedureName = "MID_COLOR_NODE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("COLOR_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    COLOR_DESCRIPTION = new stringParameter("@COLOR_DESCRIPTION", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? COLOR_CODE_RID,
                                  string COLOR_DESCRIPTION
                                  )
                {
                    lock (typeof(MID_COLOR_NODE_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.COLOR_DESCRIPTION.SetValue(COLOR_DESCRIPTION);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_DELETE_def MID_COLOR_NODE_DELETE = new MID_COLOR_NODE_DELETE_def();
            public class MID_COLOR_NODE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_NODE_DELETE.SQL"
 
                private intParameter HN_RID;
                private intParameter COLOR_CODE_RID;

                public MID_COLOR_NODE_DELETE_def()
                {
                    base.procedureName = "MID_COLOR_NODE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("COLOR_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? COLOR_CODE_RID
                                  )
                {
                    lock (typeof(MID_COLOR_NODE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_READ_FOR_STYLE_def MID_COLOR_NODE_READ_FOR_STYLE = new MID_COLOR_NODE_READ_FOR_STYLE_def();
            public class MID_COLOR_NODE_READ_FOR_STYLE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COLOR_NODE_READ_FOR_STYLE.SQL"
 
                private intParameter PH_RID;
                private intParameter STYLE_HN_RID;
                private stringParameter COLOR_CODE_ID;

                public MID_COLOR_NODE_READ_FOR_STYLE_def()
                {
                    base.procedureName = "MID_COLOR_NODE_READ_FOR_STYLE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_NODE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    STYLE_HN_RID = new intParameter("@STYLE_HN_RID", base.inputParameterList);
                    COLOR_CODE_ID = new stringParameter("@COLOR_CODE_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? PH_RID,
                                      int? STYLE_HN_RID,
                                      string COLOR_CODE_ID
                                      )
                {
                    lock (typeof(MID_COLOR_NODE_READ_FOR_STYLE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.STYLE_HN_RID.SetValue(STYLE_HN_RID);
                        this.COLOR_CODE_ID.SetValue(COLOR_CODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE_def MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE = new MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE_def();
            public class MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE.SQL"
 
                private intParameter HN_RID;

                public MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VW_HIERARCHY_GET_DATA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE_def MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE = new MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE_def();
            public class MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE.SQL"
 
                private intParameter hierarchyRID;
                private intParameter HN_RID;

                public MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    hierarchyRID = new intParameter("@hierarchyRID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? hierarchyRID,
                                      int? HN_RID
                                      )
                {
                    lock (typeof(MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE_def))
                    {
                        this.hierarchyRID.SetValue(hierarchyRID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_NODE_DELETE_def MID_SIZE_NODE_DELETE = new MID_SIZE_NODE_DELETE_def();
            public class MID_SIZE_NODE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_NODE_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter SIZE_CODE_RID;

                public MID_SIZE_NODE_DELETE_def()
                {
                    base.procedureName = "MID_SIZE_NODE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SIZE_CODE_RID
                                  )
                {
                    lock (typeof(MID_SIZE_NODE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SIZE_NODE_READ_RID_FOR_COLOR_def MID_SIZE_NODE_READ_RID_FOR_COLOR = new MID_SIZE_NODE_READ_RID_FOR_COLOR_def();
            public class MID_SIZE_NODE_READ_RID_FOR_COLOR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_NODE_READ_RID_FOR_COLOR.SQL"

                private intParameter PH_RID;
                private intParameter COLOR_HN_RID;
                private stringParameter SIZE_CODE_ID;

                public MID_SIZE_NODE_READ_RID_FOR_COLOR_def()
                {
                    base.procedureName = "MID_SIZE_NODE_READ_RID_FOR_COLOR";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_NODE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    COLOR_HN_RID = new intParameter("@COLOR_HN_RID", base.inputParameterList);
                    SIZE_CODE_ID = new stringParameter("@SIZE_CODE_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? PH_RID,
                                      int? COLOR_HN_RID,
                                      string SIZE_CODE_ID
                                      )
                {
                    lock (typeof(MID_SIZE_NODE_READ_RID_FOR_COLOR_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.COLOR_HN_RID.SetValue(COLOR_HN_RID);
                        this.SIZE_CODE_ID.SetValue(SIZE_CODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_JOIN_READ_def MID_HIER_NODE_JOIN_READ = new MID_HIER_NODE_JOIN_READ_def();
            public class MID_HIER_NODE_JOIN_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_JOIN_READ.SQL"

                private intParameter PH_RID;

                public MID_HIER_NODE_JOIN_READ_def()
                {
                    base.procedureName = "MID_HIER_NODE_JOIN_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_NODE_JOIN");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_HIER_NODE_JOIN_READ_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT_def MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT = new MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT_def();
            public class MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_HN_RID;
                private intParameter HN_RID;

                public MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT_def()
                {
                    base.procedureName = "MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_NODE_JOIN");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_HN_RID = new intParameter("@PARENT_HN_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? PH_RID,
                                      int? PARENT_HN_RID,
                                      int? HN_RID
                                      )
                {
                    lock (typeof(MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_HN_RID.SetValue(PARENT_HN_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_JOIN_INSERT_def MID_HIER_NODE_JOIN_INSERT = new MID_HIER_NODE_JOIN_INSERT_def();
            public class MID_HIER_NODE_JOIN_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_JOIN_INSERT.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_HN_RID;
                private intParameter HN_RID;

                public MID_HIER_NODE_JOIN_INSERT_def()
                {
                    base.procedureName = "MID_HIER_NODE_JOIN_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HIER_NODE_JOIN");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_HN_RID = new intParameter("@PARENT_HN_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PARENT_HN_RID,
                                  int? HN_RID
                                  )
                {
                    lock (typeof(MID_HIER_NODE_JOIN_INSERT_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_HN_RID.SetValue(PARENT_HN_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_HIERJOIN_UPDATE_def SP_MID_HIERJOIN_UPDATE = new SP_MID_HIERJOIN_UPDATE_def();
            public class SP_MID_HIERJOIN_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERJOIN_UPDATE.SQL"

                private intParameter HN_RID;
                private intParameter OLD_HIERARCHY_RID;
                private intParameter OLD_PARENT_RID;
                private intParameter NEW_HIERARCHY_RID;
                private intParameter NEW_PARENT_RID;

                public SP_MID_HIERJOIN_UPDATE_def()
                {
                    base.procedureName = "SP_MID_HIERJOIN_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    OLD_HIERARCHY_RID = new intParameter("@OLD_HIERARCHY_RID", base.inputParameterList);
                    OLD_PARENT_RID = new intParameter("@OLD_PARENT_RID", base.inputParameterList);
                    NEW_HIERARCHY_RID = new intParameter("@NEW_HIERARCHY_RID", base.inputParameterList);
                    NEW_PARENT_RID = new intParameter("@NEW_PARENT_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? OLD_HIERARCHY_RID,
                                  int? OLD_PARENT_RID,
                                  int? NEW_HIERARCHY_RID,
                                  int? NEW_PARENT_RID
                                  )
                {
                    lock (typeof(SP_MID_HIERJOIN_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.OLD_HIERARCHY_RID.SetValue(OLD_HIERARCHY_RID);
                        this.OLD_PARENT_RID.SetValue(OLD_PARENT_RID);
                        this.NEW_HIERARCHY_RID.SetValue(NEW_HIERARCHY_RID);
                        this.NEW_PARENT_RID.SetValue(NEW_PARENT_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_JOIN_DELETE_ALL_def MID_HIER_NODE_JOIN_DELETE_ALL = new MID_HIER_NODE_JOIN_DELETE_ALL_def();
            public class MID_HIER_NODE_JOIN_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_JOIN_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_HIER_NODE_JOIN_DELETE_ALL_def()
                {
                    base.procedureName = "MID_HIER_NODE_JOIN_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_NODE_JOIN");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIER_NODE_JOIN_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_NODE_JOIN_DELETE_def MID_HIER_NODE_JOIN_DELETE = new MID_HIER_NODE_JOIN_DELETE_def();
            public class MID_HIER_NODE_JOIN_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_NODE_JOIN_DELETE.SQL"

                private intParameter PH_RID;
                private intParameter PARENT_HN_RID;
                private intParameter HN_RID;

                public MID_HIER_NODE_JOIN_DELETE_def()
                {
                    base.procedureName = "MID_HIER_NODE_JOIN_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_NODE_JOIN");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PARENT_HN_RID = new intParameter("@PARENT_HN_RID", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? PH_RID,
                                  int? PARENT_HN_RID,
                                  int? HN_RID
                                  )
                {
                    lock (typeof(MID_HIER_NODE_JOIN_DELETE_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.PARENT_HN_RID.SetValue(PARENT_HN_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_GROUP_READ_ALL_def MID_HIER_CHAR_GROUP_READ_ALL = new MID_HIER_CHAR_GROUP_READ_ALL_def();
            public class MID_HIER_CHAR_GROUP_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_GROUP_READ_ALL.SQL"

                public MID_HIER_CHAR_GROUP_READ_ALL_def()
                {
                    base.procedureName = "MID_HIER_CHAR_GROUP_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_CHAR_GROUP");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIER_CHAR_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_GROUP_VALUES_READ_ALL_def MID_HIER_CHAR_GROUP_VALUES_READ_ALL = new MID_HIER_CHAR_GROUP_VALUES_READ_ALL_def();
            public class MID_HIER_CHAR_GROUP_VALUES_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_GROUP_VALUES_READ_ALL.SQL"

                public MID_HIER_CHAR_GROUP_VALUES_READ_ALL_def()
                {
                    base.procedureName = "MID_HIER_CHAR_GROUP_VALUES_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_CHAR_GROUP");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIER_CHAR_GROUP_VALUES_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_HIERCHARGROUP_INSERT_def SP_MID_HIERCHARGROUP_INSERT = new SP_MID_HIERCHARGROUP_INSERT_def();
            public class SP_MID_HIERCHARGROUP_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERCHARGROUP_INSERT.SQL"

                private stringParameter HCG_ID;
                private intParameter HCG_RID; //Declare Output Parameter

                public SP_MID_HIERCHARGROUP_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIERCHARGROUP_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HIER_CHAR_GROUP");
                    HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);

                    HCG_RID = new intParameter("@HCG_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, string HCG_ID)
                {
                    lock (typeof(SP_MID_HIERCHARGROUP_INSERT_def))
                    {
                        this.HCG_ID.SetValue(HCG_ID);
                        this.HCG_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_GROUP_UPDATE_def MID_HIER_CHAR_GROUP_UPDATE = new MID_HIER_CHAR_GROUP_UPDATE_def();
            public class MID_HIER_CHAR_GROUP_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_GROUP_UPDATE.SQL"

                private intParameter HCG_RID;
                private stringParameter HCG_ID;

                public MID_HIER_CHAR_GROUP_UPDATE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_GROUP_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HIER_CHAR_GROUP");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HCG_RID,
                                  string HCG_ID
                                  )
                {
                    lock (typeof(MID_HIER_CHAR_GROUP_UPDATE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HCG_ID.SetValue(HCG_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_GROUP_DELETE_def MID_HIER_CHAR_GROUP_DELETE = new MID_HIER_CHAR_GROUP_DELETE_def();
            public class MID_HIER_CHAR_GROUP_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_GROUP_DELETE.SQL"

                private intParameter HCG_RID;

                public MID_HIER_CHAR_GROUP_DELETE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_GROUP_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_CHAR_GROUP");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HCG_RID)
                {
                    lock (typeof(MID_HIER_CHAR_GROUP_DELETE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_HIERCHAR_INSERT_def SP_MID_HIERCHAR_INSERT = new SP_MID_HIERCHAR_INSERT_def();
            public class SP_MID_HIERCHAR_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIERCHAR_INSERT.SQL"

                private intParameter HCG_RID;
                private stringParameter HC_ID;
                private intParameter HC_RID; //Declare Output Parameter

                public SP_MID_HIERCHAR_INSERT_def()
                {
                    base.procedureName = "SP_MID_HIERCHAR_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("HIER_CHAR");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HC_ID = new stringParameter("@HC_ID", base.inputParameterList);

                    HC_RID = new intParameter("@HC_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? HCG_RID,
                                              string HC_ID
                                              )
                {
                    lock (typeof(SP_MID_HIERCHAR_INSERT_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HC_ID.SetValue(HC_ID);
                        this.HC_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_UPDATE_def MID_HIER_CHAR_UPDATE = new MID_HIER_CHAR_UPDATE_def();
            public class MID_HIER_CHAR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_UPDATE.SQL"

                private intParameter HC_RID;
                private stringParameter HC_ID;

                public MID_HIER_CHAR_UPDATE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HIER_CHAR");
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                    HC_ID = new stringParameter("@HC_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HC_RID,
                                  string HC_ID
                                  )
                {
                    lock (typeof(MID_HIER_CHAR_UPDATE_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        this.HC_ID.SetValue(HC_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_DELETE_def MID_HIER_CHAR_DELETE = new MID_HIER_CHAR_DELETE_def();
            public class MID_HIER_CHAR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_DELETE.SQL"

                private intParameter HC_RID;

                public MID_HIER_CHAR_DELETE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_CHAR");
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HC_RID)
                {
                    lock (typeof(MID_HIER_CHAR_DELETE_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_JOIN_READ_def MID_HIER_CHAR_JOIN_READ = new MID_HIER_CHAR_JOIN_READ_def();
            public class MID_HIER_CHAR_JOIN_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_READ.SQL"

                private intParameter HN_RID;

                public MID_HIER_CHAR_JOIN_READ_def()
                {
                    base.procedureName = "MID_HIER_CHAR_JOIN_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_CHAR_JOIN");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIER_CHAR_JOIN_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_JOIN_READ_FROM_VALUE_def MID_HIER_CHAR_JOIN_READ_FROM_VALUE = new MID_HIER_CHAR_JOIN_READ_FROM_VALUE_def();
            public class MID_HIER_CHAR_JOIN_READ_FROM_VALUE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_READ_FROM_VALUE.SQL"

                private intParameter HC_RID;

                public MID_HIER_CHAR_JOIN_READ_FROM_VALUE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_JOIN_READ_FROM_VALUE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_CHAR_JOIN");
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HC_RID)
                {
                    lock (typeof(MID_HIER_CHAR_JOIN_READ_FROM_VALUE_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_JOIN_INSERT_def MID_HIER_CHAR_JOIN_INSERT = new MID_HIER_CHAR_JOIN_INSERT_def();
            public class MID_HIER_CHAR_JOIN_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter HC_RID;

                public MID_HIER_CHAR_JOIN_INSERT_def()
                {
                    base.procedureName = "MID_HIER_CHAR_JOIN_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("HIER_CHAR_JOIN");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? HC_RID
                                  )
                {
                    lock (typeof(MID_HIER_CHAR_JOIN_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_JOIN_DELETE_def MID_HIER_CHAR_JOIN_DELETE = new MID_HIER_CHAR_JOIN_DELETE_def();
            public class MID_HIER_CHAR_JOIN_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter HC_RID;

                public MID_HIER_CHAR_JOIN_DELETE_def()
                {
                    base.procedureName = "MID_HIER_CHAR_JOIN_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_CHAR_JOIN");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? HC_RID
                                  )
                {
                    lock (typeof(MID_HIER_CHAR_JOIN_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_JOIN_DELETE_ALL_def MID_HIER_CHAR_JOIN_DELETE_ALL = new MID_HIER_CHAR_JOIN_DELETE_ALL_def();
            public class MID_HIER_CHAR_JOIN_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_HIER_CHAR_JOIN_DELETE_ALL_def()
                {
                    base.procedureName = "MID_HIER_CHAR_JOIN_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HIER_CHAR_JOIN");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_HIER_CHAR_JOIN_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIER_CHAR_READ_FOR_GROUP_def MID_HIER_CHAR_READ_FOR_GROUP = new MID_HIER_CHAR_READ_FOR_GROUP_def();
            public class MID_HIER_CHAR_READ_FOR_GROUP_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_READ_FOR_GROUP.SQL"

                private intParameter HCG_RID;

                public MID_HIER_CHAR_READ_FOR_GROUP_def()
                {
                    base.procedureName = "MID_HIER_CHAR_READ_FOR_GROUP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIER_CHAR");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HCG_RID)
                {
                    lock (typeof(MID_HIER_CHAR_READ_FOR_GROUP_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_METHOD_OVERRIDE_IMO_READ_ALL_def MID_METHOD_OVERRIDE_IMO_READ_ALL = new MID_METHOD_OVERRIDE_IMO_READ_ALL_def();
            public class MID_METHOD_OVERRIDE_IMO_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_OVERRIDE_IMO_READ_ALL.SQL"

                public MID_METHOD_OVERRIDE_IMO_READ_ALL_def()
                {
                    base.procedureName = "MID_METHOD_OVERRIDE_IMO_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("METHOD_OVERRIDE_IMO");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_METHOD_OVERRIDE_IMO_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_NODE_IMO_DELETE_def MID_NODE_IMO_DELETE = new MID_NODE_IMO_DELETE_def();
            public class MID_NODE_IMO_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_NODE_IMO_DELETE.SQL"

                private intParameter HN_RID;

                public MID_NODE_IMO_DELETE_def()
                {
                    base.procedureName = "MID_NODE_IMO_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_IMO");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_IMO_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_IMO_DELETE_FROM_STORE_def MID_NODE_IMO_DELETE_FROM_STORE = new MID_NODE_IMO_DELETE_FROM_STORE_def();
            public class MID_NODE_IMO_DELETE_FROM_STORE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_NODE_IMO_DELETE_FROM_STORE.SQL"

                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_NODE_IMO_DELETE_FROM_STORE_def()
                {
                    base.procedureName = "MID_NODE_IMO_DELETE_FROM_STORE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_IMO");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_NODE_IMO_DELETE_FROM_STORE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_IMO_READ_FOR_STORES_def MID_NODE_IMO_READ_FOR_STORES = new MID_NODE_IMO_READ_FOR_STORES_def();
            public class MID_NODE_IMO_READ_FOR_STORES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_NODE_IMO_READ_FOR_STORES.SQL"

                private intParameter HN_RID;

                public MID_NODE_IMO_READ_FOR_STORES_def()
                {
                    base.procedureName = "MID_NODE_IMO_READ_FOR_STORES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_IMO");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_IMO_READ_FOR_STORES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_NODE_IMO_INSERT_def MID_NODE_IMO_INSERT = new MID_NODE_IMO_INSERT_def();
            public class MID_NODE_IMO_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_NODE_IMO_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter IMO_MIN_SHIP_QTY;
                private floatParameter IMO_PCT_PK_THRSHLD;
                private floatParameter IMO_FWOS_MAX;
                private intParameter IMO_MAX_VALUE;
                private intParameter IMO_PSH_BK_STK;
                private intParameter IMO_FWOS_MAX_RID;
                private intParameter IMO_FWOS_MAX_TYPE;

                public MID_NODE_IMO_INSERT_def()
                {
                    base.procedureName = "MID_NODE_IMO_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("NODE_IMO");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    IMO_MIN_SHIP_QTY = new intParameter("@IMO_MIN_SHIP_QTY", base.inputParameterList);
                    IMO_PCT_PK_THRSHLD = new floatParameter("@IMO_PCT_PK_THRSHLD", base.inputParameterList);
                    IMO_FWOS_MAX = new floatParameter("@IMO_FWOS_MAX", base.inputParameterList);
                    IMO_MAX_VALUE = new intParameter("@IMO_MAX_VALUE", base.inputParameterList);
                    IMO_PSH_BK_STK = new intParameter("@IMO_PSH_BK_STK", base.inputParameterList);
                    IMO_FWOS_MAX_RID = new intParameter("@IMO_FWOS_MAX_RID", base.inputParameterList);
                    IMO_FWOS_MAX_TYPE = new intParameter("@IMO_FWOS_MAX_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? IMO_MIN_SHIP_QTY,
                                  double? IMO_PCT_PK_THRSHLD,
                                  double? IMO_FWOS_MAX,
                                  int? IMO_MAX_VALUE,
                                  int? IMO_PSH_BK_STK,
                                  int? IMO_FWOS_MAX_RID,
                                  int? IMO_FWOS_MAX_TYPE
                                  )
                {
                    lock (typeof(MID_NODE_IMO_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.IMO_MIN_SHIP_QTY.SetValue(IMO_MIN_SHIP_QTY);
                        this.IMO_PCT_PK_THRSHLD.SetValue(IMO_PCT_PK_THRSHLD);
                        this.IMO_FWOS_MAX.SetValue(IMO_FWOS_MAX);
                        this.IMO_MAX_VALUE.SetValue(IMO_MAX_VALUE);
                        this.IMO_PSH_BK_STK.SetValue(IMO_PSH_BK_STK);
                        this.IMO_FWOS_MAX_RID.SetValue(IMO_FWOS_MAX_RID);
                        this.IMO_FWOS_MAX_TYPE.SetValue(IMO_FWOS_MAX_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PURGE_CRITERIA_INSERT_def MID_PURGE_CRITERIA_INSERT = new MID_PURGE_CRITERIA_INSERT_def();
            public class MID_PURGE_CRITERIA_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_CRITERIA_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter PURGE_DAILY_HISTORY;
                private intParameter PURGE_WEEKLY_HISTORY;
                private intParameter PURGE_PLANS;

                public MID_PURGE_CRITERIA_INSERT_def()
                {
                    base.procedureName = "MID_PURGE_CRITERIA_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PURGE_CRITERIA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PURGE_DAILY_HISTORY = new intParameter("@PURGE_DAILY_HISTORY", base.inputParameterList);
                    PURGE_WEEKLY_HISTORY = new intParameter("@PURGE_WEEKLY_HISTORY", base.inputParameterList);
                    PURGE_PLANS = new intParameter("@PURGE_PLANS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? PURGE_DAILY_HISTORY,
                                  int? PURGE_WEEKLY_HISTORY,
                                  int? PURGE_PLANS
                                  )
                {
                    lock (typeof(MID_PURGE_CRITERIA_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.PURGE_DAILY_HISTORY.SetValue(PURGE_DAILY_HISTORY);
                        this.PURGE_WEEKLY_HISTORY.SetValue(PURGE_WEEKLY_HISTORY);
                        this.PURGE_PLANS.SetValue(PURGE_PLANS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PURGE_CRITERIA_HEADER_PURGE_INSERT_def MID_PURGE_CRITERIA_HEADER_PURGE_INSERT = new MID_PURGE_CRITERIA_HEADER_PURGE_INSERT_def();
            public class MID_PURGE_CRITERIA_HEADER_PURGE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_CRITERIA_HEADER_PURGE_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter HEADER_TYPE;
                private intParameter PURGE_HEADERS_TIMEFRAME;
                private intParameter PURGE_HEADERS;

                public MID_PURGE_CRITERIA_HEADER_PURGE_INSERT_def()
                {
                    base.procedureName = "MID_PURGE_CRITERIA_HEADER_PURGE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PURGE_CRITERIA_HEADER_PURGE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    HEADER_TYPE = new intParameter("@HEADER_TYPE", base.inputParameterList);
                    PURGE_HEADERS_TIMEFRAME = new intParameter("@PURGE_HEADERS_TIMEFRAME", base.inputParameterList);
                    PURGE_HEADERS = new intParameter("@PURGE_HEADERS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? HEADER_TYPE,
                                  int? PURGE_HEADERS_TIMEFRAME,
                                  int? PURGE_HEADERS
                                  )
                {
                    lock (typeof(MID_PURGE_CRITERIA_HEADER_PURGE_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.HEADER_TYPE.SetValue(HEADER_TYPE);
                        this.PURGE_HEADERS_TIMEFRAME.SetValue(PURGE_HEADERS_TIMEFRAME);
                        this.PURGE_HEADERS.SetValue(PURGE_HEADERS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PURGE_CRITERIA_DELETE_ALL_def MID_PURGE_CRITERIA_DELETE_ALL = new MID_PURGE_CRITERIA_DELETE_ALL_def();
            public class MID_PURGE_CRITERIA_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PURGE_CRITERIA_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_PURGE_CRITERIA_DELETE_ALL_def()
                {
                    base.procedureName = "MID_PURGE_CRITERIA_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PURGE_CRITERIA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_PURGE_CRITERIA_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_READ_def MID_STORE_GRADES_READ = new MID_STORE_GRADES_READ_def();
            public class MID_STORE_GRADES_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_READ.SQL"

                private intParameter HN_RID;

                public MID_STORE_GRADES_READ_def()
                {
                    base.procedureName = "MID_STORE_GRADES_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_GRADES_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_INSERT_def MID_STORE_GRADES_INSERT = new MID_STORE_GRADES_INSERT_def();
            public class MID_STORE_GRADES_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_INSERT.SQL"

                private intParameter HN_RID;
                private stringParameter GRADE_CODE;
                private intParameter BOUNDARY;
                private floatParameter WOS_INDEX;

                public MID_STORE_GRADES_INSERT_def()
                {
                    base.procedureName = "MID_STORE_GRADES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    WOS_INDEX = new floatParameter("@WOS_INDEX", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  string GRADE_CODE,
                                  int? BOUNDARY,
                                  double? WOS_INDEX
                                  )
                {
                    lock (typeof(MID_STORE_GRADES_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.WOS_INDEX.SetValue(WOS_INDEX);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_UPDATE_def MID_STORE_GRADES_UPDATE = new MID_STORE_GRADES_UPDATE_def();
            public class MID_STORE_GRADES_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_UPDATE.SQL"

                private intParameter HN_RID;
                private stringParameter GRADE_CODE;
                private intParameter BOUNDARY;
                private floatParameter WOS_INDEX;

                public MID_STORE_GRADES_UPDATE_def()
                {
                    base.procedureName = "MID_STORE_GRADES_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    WOS_INDEX = new floatParameter("@WOS_INDEX", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  string GRADE_CODE,
                                  int? BOUNDARY,
                                  double? WOS_INDEX
                                  )
                {
                    lock (typeof(MID_STORE_GRADES_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.WOS_INDEX.SetValue(WOS_INDEX);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_DELETE_def MID_STORE_GRADES_DELETE = new MID_STORE_GRADES_DELETE_def();
            public class MID_STORE_GRADES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;

                public MID_STORE_GRADES_DELETE_def()
                {
                    base.procedureName = "MID_STORE_GRADES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY
                                  )
                {
                    lock (typeof(MID_STORE_GRADES_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_DELETE_ALL_def MID_STORE_GRADES_DELETE_ALL = new MID_STORE_GRADES_DELETE_ALL_def();
            public class MID_STORE_GRADES_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_STORE_GRADES_DELETE_ALL_def()
                {
                    base.procedureName = "MID_STORE_GRADES_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_GRADES_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_INSERT_WITH_MINMAX_def MID_STORE_GRADES_INSERT_WITH_MINMAX = new MID_STORE_GRADES_INSERT_WITH_MINMAX_def();
            public class MID_STORE_GRADES_INSERT_WITH_MINMAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_INSERT_WITH_MINMAX.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;
                private intParameter MINIMUM_COLOR;
                private intParameter MAXIMUM_COLOR;
                private intParameter SHIP_UP_TO;

                public MID_STORE_GRADES_INSERT_WITH_MINMAX_def()
                {
                    base.procedureName = "MID_STORE_GRADES_INSERT_WITH_MINMAX";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
                    MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
                    MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
                    MINIMUM_COLOR = new intParameter("@MINIMUM_COLOR", base.inputParameterList);
                    MAXIMUM_COLOR = new intParameter("@MAXIMUM_COLOR", base.inputParameterList);
                    SHIP_UP_TO = new intParameter("@SHIP_UP_TO", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  int? MINIMUM_STOCK,
                                  int? MAXIMUM_STOCK,
                                  int? MINIMUM_AD,
                                  int? MINIMUM_COLOR,
                                  int? MAXIMUM_COLOR,
                                  int? SHIP_UP_TO
                                  )
                {
                    lock (typeof(MID_STORE_GRADES_INSERT_WITH_MINMAX_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        this.MINIMUM_COLOR.SetValue(MINIMUM_COLOR);
                        this.MAXIMUM_COLOR.SetValue(MAXIMUM_COLOR);
                        this.SHIP_UP_TO.SetValue(SHIP_UP_TO);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STORE_GRADES_UPDATE_WITH_MINMAX_def MID_STORE_GRADES_UPDATE_WITH_MINMAX = new MID_STORE_GRADES_UPDATE_WITH_MINMAX_def();
            public class MID_STORE_GRADES_UPDATE_WITH_MINMAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_UPDATE_WITH_MINMAX.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;
                private intParameter MINIMUM_COLOR;
                private intParameter MAXIMUM_COLOR;
                private intParameter SHIP_UP_TO;

                public MID_STORE_GRADES_UPDATE_WITH_MINMAX_def()
                {
                    base.procedureName = "MID_STORE_GRADES_UPDATE_WITH_MINMAX";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_GRADES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
                    MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
                    MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
                    MINIMUM_COLOR = new intParameter("@MINIMUM_COLOR", base.inputParameterList);
                    MAXIMUM_COLOR = new intParameter("@MAXIMUM_COLOR", base.inputParameterList);
                    SHIP_UP_TO = new intParameter("@SHIP_UP_TO", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  int? MINIMUM_STOCK,
                                  int? MAXIMUM_STOCK,
                                  int? MINIMUM_AD,
                                  int? MINIMUM_COLOR,
                                  int? MAXIMUM_COLOR,
                                  int? SHIP_UP_TO
                                  )
                {
                    lock (typeof(MID_STORE_GRADES_UPDATE_WITH_MINMAX_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        this.MINIMUM_COLOR.SetValue(MINIMUM_COLOR);
                        this.MAXIMUM_COLOR.SetValue(MAXIMUM_COLOR);
                        this.SHIP_UP_TO.SetValue(SHIP_UP_TO);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_READ_def MID_VELOCITY_GRADE_READ = new MID_VELOCITY_GRADE_READ_def();
            public class MID_VELOCITY_GRADE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_READ.SQL"

                private intParameter HN_RID;

                public MID_VELOCITY_GRADE_READ_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_VELOCITY_GRADE_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_INSERT_def MID_VELOCITY_GRADE_INSERT = new MID_VELOCITY_GRADE_INSERT_def();
            public class MID_VELOCITY_GRADE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;

                public MID_VELOCITY_GRADE_INSERT_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  string GRADE_CODE
                                  )
                {
                    lock (typeof(MID_VELOCITY_GRADE_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_UPDATE_def MID_VELOCITY_GRADE_UPDATE = new MID_VELOCITY_GRADE_UPDATE_def();
            public class MID_VELOCITY_GRADE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_UPDATE.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private stringParameter GRADE_CODE;

                public MID_VELOCITY_GRADE_UPDATE_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    GRADE_CODE = new stringParameter("@GRADE_CODE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  string GRADE_CODE
                                  )
                {
                    lock (typeof(MID_VELOCITY_GRADE_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.GRADE_CODE.SetValue(GRADE_CODE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_DELETE_def MID_VELOCITY_GRADE_DELETE = new MID_VELOCITY_GRADE_DELETE_def();
            public class MID_VELOCITY_GRADE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_DELETE.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;

                public MID_VELOCITY_GRADE_DELETE_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY
                                  )
                {
                    lock (typeof(MID_VELOCITY_GRADE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_DELETE_ALL_def MID_VELOCITY_GRADE_DELETE_ALL = new MID_VELOCITY_GRADE_DELETE_ALL_def();
            public class MID_VELOCITY_GRADE_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_VELOCITY_GRADE_DELETE_ALL_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_VELOCITY_GRADE_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_INSERT_WITH_MINMAX_def MID_VELOCITY_GRADE_INSERT_WITH_MINMAX = new MID_VELOCITY_GRADE_INSERT_WITH_MINMAX_def();
            public class MID_VELOCITY_GRADE_INSERT_WITH_MINMAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_INSERT_WITH_MINMAX.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;

                public MID_VELOCITY_GRADE_INSERT_WITH_MINMAX_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_INSERT_WITH_MINMAX";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
                    MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
                    MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  int? MINIMUM_STOCK,
                                  int? MAXIMUM_STOCK,
                                  int? MINIMUM_AD
                                  )
                {
                    lock (typeof(MID_VELOCITY_GRADE_INSERT_WITH_MINMAX_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX_def MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX = new MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX_def();
            public class MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX.SQL"

                private intParameter HN_RID;
                private intParameter BOUNDARY;
                private intParameter MINIMUM_STOCK;
                private intParameter MAXIMUM_STOCK;
                private intParameter MINIMUM_AD;

                public MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX_def()
                {
                    base.procedureName = "MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("VELOCITY_GRADE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    MINIMUM_STOCK = new intParameter("@MINIMUM_STOCK", base.inputParameterList);
                    MAXIMUM_STOCK = new intParameter("@MAXIMUM_STOCK", base.inputParameterList);
                    MINIMUM_AD = new intParameter("@MINIMUM_AD", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? BOUNDARY,
                                  int? MINIMUM_STOCK,
                                  int? MAXIMUM_STOCK,
                                  int? MINIMUM_AD
                                  )
                {
                    lock (typeof(MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.MINIMUM_STOCK.SetValue(MINIMUM_STOCK);
                        this.MAXIMUM_STOCK.SetValue(MAXIMUM_STOCK);
                        this.MINIMUM_AD.SetValue(MINIMUM_AD);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SELL_THRU_READ_def MID_SELL_THRU_READ = new MID_SELL_THRU_READ_def();
            public class MID_SELL_THRU_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SELL_THRU_READ.SQL"

                private intParameter HN_RID;

                public MID_SELL_THRU_READ_def()
                {
                    base.procedureName = "MID_SELL_THRU_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SELL_THRU");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_SELL_THRU_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SELL_THRU_INSERT_def MID_SELL_THRU_INSERT = new MID_SELL_THRU_INSERT_def();
            public class MID_SELL_THRU_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SELL_THRU_INSERT.SQL"

                private intParameter HN_RID;
                private intParameter SELL_THRU_PCT;

                public MID_SELL_THRU_INSERT_def()
                {
                    base.procedureName = "MID_SELL_THRU_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SELL_THRU");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SELL_THRU_PCT = new intParameter("@SELL_THRU_PCT", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SELL_THRU_PCT
                                  )
                {
                    lock (typeof(MID_SELL_THRU_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SELL_THRU_PCT.SetValue(SELL_THRU_PCT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SELL_THRU_DELETE_ALL_def MID_SELL_THRU_DELETE_ALL = new MID_SELL_THRU_DELETE_ALL_def();
            public class MID_SELL_THRU_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SELL_THRU_DELETE_ALL.SQL"

                private intParameter HN_RID;

                public MID_SELL_THRU_DELETE_ALL_def()
                {
                    base.procedureName = "MID_SELL_THRU_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SELL_THRU");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_SELL_THRU_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_ELIGIBILITY_MODEL_READ_ALL_def MID_ELIGIBILITY_MODEL_READ_ALL = new MID_ELIGIBILITY_MODEL_READ_ALL_def();
            public class MID_ELIGIBILITY_MODEL_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ELIGIBILITY_MODEL_READ_ALL.SQL"

                public MID_ELIGIBILITY_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_ELIGIBILITY_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("ELIGIBILITY_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_ELIGIBILITY_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_ELIG_MODEL_INSERT_def SP_MID_ELIG_MODEL_INSERT = new SP_MID_ELIG_MODEL_INSERT_def();
            public class SP_MID_ELIG_MODEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ELIG_MODEL_INSERT.SQL"

                private stringParameter EM_ID;
                private intParameter EM_RID; //Declare Output Parameter

                public SP_MID_ELIG_MODEL_INSERT_def()
                {
                    base.procedureName = "SP_MID_ELIG_MODEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("ELIGIBILITY_MODEL");
                    EM_ID = new stringParameter("@EM_ID", base.inputParameterList);

                    EM_RID = new intParameter("@EM_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, string EM_ID)
                {
                    lock (typeof(SP_MID_ELIG_MODEL_INSERT_def))
                    {
                        this.EM_ID.SetValue(EM_ID);
                        this.EM_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_ELIGIBILITY_MODEL_DELETE_def MID_ELIGIBILITY_MODEL_DELETE = new MID_ELIGIBILITY_MODEL_DELETE_def();
            public class MID_ELIGIBILITY_MODEL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ELIGIBILITY_MODEL_DELETE.SQL"

                private intParameter EM_RID;

                public MID_ELIGIBILITY_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_ELIGIBILITY_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("ELIGIBILITY_MODEL");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_ELIGIBILITY_MODEL_DELETE_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SALES_ELIGIBILITY_ENTRY_READ_def MID_SALES_ELIGIBILITY_ENTRY_READ = new MID_SALES_ELIGIBILITY_ENTRY_READ_def();
            public class MID_SALES_ELIGIBILITY_ENTRY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SALES_ELIGIBILITY_ENTRY_READ.SQL"

                private intParameter EM_RID;

                public MID_SALES_ELIGIBILITY_ENTRY_READ_def()
                {
                    base.procedureName = "MID_SALES_ELIGIBILITY_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SALES_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_SALES_ELIGIBILITY_ENTRY_READ_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SALES_ELIGIBILITY_ENTRY_INSERT_def MID_SALES_ELIGIBILITY_ENTRY_INSERT = new MID_SALES_ELIGIBILITY_ENTRY_INSERT_def();
            public class MID_SALES_ELIGIBILITY_ENTRY_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SALES_ELIGIBILITY_ENTRY_INSERT.SQL"

                private intParameter EM_RID;
                private intParameter SALES_EM_SEQUENCE;
                private intParameter CDR_RID;

                public MID_SALES_ELIGIBILITY_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_SALES_ELIGIBILITY_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SALES_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    SALES_EM_SEQUENCE = new intParameter("@SALES_EM_SEQUENCE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? EM_RID,
                                  int? SALES_EM_SEQUENCE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_SALES_ELIGIBILITY_ENTRY_INSERT_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        this.SALES_EM_SEQUENCE.SetValue(SALES_EM_SEQUENCE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SALES_ELIGIBILITY_ENTRY_DELETE_def MID_SALES_ELIGIBILITY_ENTRY_DELETE = new MID_SALES_ELIGIBILITY_ENTRY_DELETE_def();
            public class MID_SALES_ELIGIBILITY_ENTRY_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SALES_ELIGIBILITY_ENTRY_DELETE.SQL"

                private intParameter EM_RID;

                public MID_SALES_ELIGIBILITY_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_SALES_ELIGIBILITY_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SALES_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_SALES_ELIGIBILITY_ENTRY_DELETE_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STOCK_ELIGIBILITY_ENTRY_READ_def MID_STOCK_ELIGIBILITY_ENTRY_READ = new MID_STOCK_ELIGIBILITY_ENTRY_READ_def();
            public class MID_STOCK_ELIGIBILITY_ENTRY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_ELIGIBILITY_ENTRY_READ.SQL"

                private intParameter EM_RID;

                public MID_STOCK_ELIGIBILITY_ENTRY_READ_def()
                {
                    base.procedureName = "MID_STOCK_ELIGIBILITY_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STOCK_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_STOCK_ELIGIBILITY_ENTRY_READ_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STOCK_ELIGIBILITY_ENTRY_INSERT_def MID_STOCK_ELIGIBILITY_ENTRY_INSERT = new MID_STOCK_ELIGIBILITY_ENTRY_INSERT_def();
            public class MID_STOCK_ELIGIBILITY_ENTRY_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_ELIGIBILITY_ENTRY_INSERT.SQL"

                private intParameter EM_RID;
                private intParameter STOCK_EM_SEQUENCE;
                private intParameter CDR_RID;

                public MID_STOCK_ELIGIBILITY_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_STOCK_ELIGIBILITY_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STOCK_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    STOCK_EM_SEQUENCE = new intParameter("@STOCK_EM_SEQUENCE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? EM_RID,
                                  int? STOCK_EM_SEQUENCE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_STOCK_ELIGIBILITY_ENTRY_INSERT_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        this.STOCK_EM_SEQUENCE.SetValue(STOCK_EM_SEQUENCE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STOCK_ELIGIBILITY_ENTRY_DELETE_def MID_STOCK_ELIGIBILITY_ENTRY_DELETE = new MID_STOCK_ELIGIBILITY_ENTRY_DELETE_def();
            public class MID_STOCK_ELIGIBILITY_ENTRY_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_ELIGIBILITY_ENTRY_DELETE.SQL"

                private intParameter EM_RID;

                public MID_STOCK_ELIGIBILITY_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_STOCK_ELIGIBILITY_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STOCK_ELIGIBILITY_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_STOCK_ELIGIBILITY_ENTRY_DELETE_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_PRIORITY_SHIPPING_ENTRY_READ_def MID_PRIORITY_SHIPPING_ENTRY_READ = new MID_PRIORITY_SHIPPING_ENTRY_READ_def();
            public class MID_PRIORITY_SHIPPING_ENTRY_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRIORITY_SHIPPING_ENTRY_READ.SQL"

                private intParameter EM_RID;

                public MID_PRIORITY_SHIPPING_ENTRY_READ_def()
                {
                    base.procedureName = "MID_PRIORITY_SHIPPING_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRIORITY_SHIPPING_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_PRIORITY_SHIPPING_ENTRY_READ_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_PRIORITY_SHIPPING_ENTRY_INSERT_def MID_PRIORITY_SHIPPING_ENTRY_INSERT = new MID_PRIORITY_SHIPPING_ENTRY_INSERT_def();
            public class MID_PRIORITY_SHIPPING_ENTRY_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRIORITY_SHIPPING_ENTRY_INSERT.SQL"

                private intParameter EM_RID;
                private intParameter PRI_SHIP_EM_SEQUENCE;
                private intParameter CDR_RID;

                public MID_PRIORITY_SHIPPING_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_PRIORITY_SHIPPING_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("PRIORITY_SHIPPING_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    PRI_SHIP_EM_SEQUENCE = new intParameter("@PRI_SHIP_EM_SEQUENCE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? EM_RID,
                                  int? PRI_SHIP_EM_SEQUENCE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_PRIORITY_SHIPPING_ENTRY_INSERT_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        this.PRI_SHIP_EM_SEQUENCE.SetValue(PRI_SHIP_EM_SEQUENCE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_PRIORITY_SHIPPING_ENTRY_DELETE_def MID_PRIORITY_SHIPPING_ENTRY_DELETE = new MID_PRIORITY_SHIPPING_ENTRY_DELETE_def();
            public class MID_PRIORITY_SHIPPING_ENTRY_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PRIORITY_SHIPPING_ENTRY_DELETE.SQL"

                private intParameter EM_RID;

                public MID_PRIORITY_SHIPPING_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_PRIORITY_SHIPPING_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRIORITY_SHIPPING_ENTRY");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? EM_RID)
                {
                    lock (typeof(MID_PRIORITY_SHIPPING_ENTRY_DELETE_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            // Begin TT#2131-MD - JSmith - Halo Integration
            public static MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE_def MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE = new MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE_def();
            public class MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE_def : baseStoredProcedure
            {
                private intParameter EM_RID;
                private datetimeParameter UPDATE_DATE;

                public MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE_def()
                {
                    base.procedureName = "MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL");
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    UPDATE_DATE = new datetimeParameter("@UPDATE_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? EM_RID,
                                  DateTime? UPDATE_DATE
                                  )
                {
                    lock (typeof(MID_ELIG_MODEL_UPDATE_ELIG_UPDATE_DATE_def))
                    {
                        this.EM_RID.SetValue(EM_RID);
                        this.UPDATE_DATE.SetValue(UPDATE_DATE);

                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }
            // End TT#2131-MD - JSmith - Halo Integration

            public static MID_STOCK_MODIFIER_MODEL_READ_ALL_def MID_STOCK_MODIFIER_MODEL_READ_ALL = new MID_STOCK_MODIFIER_MODEL_READ_ALL_def();
            public class MID_STOCK_MODIFIER_MODEL_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MODIFIER_MODEL_READ_ALL.SQL"

                public MID_STOCK_MODIFIER_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_STKMOD_MODEL_INSERT_def SP_MID_STKMOD_MODEL_INSERT = new SP_MID_STKMOD_MODEL_INSERT_def();
            public class SP_MID_STKMOD_MODEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_STKMOD_MODEL_INSERT.SQL"

                private stringParameter STKMOD_ID;
                private floatParameter STKMOD_DEFAULT;
                private intParameter STKMOD_RID; //Declare Output Parameter

                public SP_MID_STKMOD_MODEL_INSERT_def()
                {
                    base.procedureName = "SP_MID_STKMOD_MODEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL");
                    STKMOD_ID = new stringParameter("@STKMOD_ID", base.inputParameterList);
                    STKMOD_DEFAULT = new floatParameter("@STKMOD_DEFAULT", base.inputParameterList);

                    STKMOD_RID = new intParameter("@STKMOD_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string STKMOD_ID,
                                              double? STKMOD_DEFAULT
                                             )
                {
                    lock (typeof(SP_MID_STKMOD_MODEL_INSERT_def))
                    {
                        this.STKMOD_ID.SetValue(STKMOD_ID);
                        this.STKMOD_DEFAULT.SetValue(STKMOD_DEFAULT);
                        this.STKMOD_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_FWOSMAX_MODEL_INSERT_def SP_MID_FWOSMAX_MODEL_INSERT = new SP_MID_FWOSMAX_MODEL_INSERT_def();
            public class SP_MID_FWOSMAX_MODEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FWOSMAX_MODEL_INSERT.SQL"

                private stringParameter FWOSMAX_ID;
                private floatParameter FWOSMAX_DEFAULT;
                private intParameter FWOSMAX_RID; //Declare Output Parameter

                public SP_MID_FWOSMAX_MODEL_INSERT_def()
                {
                    base.procedureName = "SP_MID_FWOSMAX_MODEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FWOS_MAX_MODEL");
                    FWOSMAX_ID = new stringParameter("@FWOSMAX_ID", base.inputParameterList);
                    FWOSMAX_DEFAULT = new floatParameter("@FWOSMAX_DEFAULT", base.inputParameterList);

                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string FWOSMAX_ID,
                                              double? FWOSMAX_DEFAULT
                                              )
                {
                    lock (typeof(SP_MID_FWOSMAX_MODEL_INSERT_def))
                    {
                        this.FWOSMAX_ID.SetValue(FWOSMAX_ID);
                        this.FWOSMAX_DEFAULT.SetValue(FWOSMAX_DEFAULT);
                        this.FWOSMAX_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_SLSMOD_MODEL_INSERT_def SP_MID_SLSMOD_MODEL_INSERT = new SP_MID_SLSMOD_MODEL_INSERT_def();
            public class SP_MID_SLSMOD_MODEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SLSMOD_MODEL_INSERT.SQL"

                private stringParameter SLSMOD_ID;
                private floatParameter SLSMOD_DEFAULT;
                private intParameter SLSMOD_RID; //Declare Output Parameter

                public SP_MID_SLSMOD_MODEL_INSERT_def()
                {
                    base.procedureName = "SP_MID_SLSMOD_MODEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("SALES_MODIFIER_MODEL");
                    SLSMOD_ID = new stringParameter("@SLSMOD_ID", base.inputParameterList);
                    SLSMOD_DEFAULT = new floatParameter("@SLSMOD_DEFAULT", base.inputParameterList);

                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string SLSMOD_ID,
                                              double? SLSMOD_DEFAULT
                                              )
                {
                    lock (typeof(SP_MID_SLSMOD_MODEL_INSERT_def))
                    {
                        this.SLSMOD_ID.SetValue(SLSMOD_ID);
                        this.SLSMOD_DEFAULT.SetValue(SLSMOD_DEFAULT);
                        this.SLSMOD_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_FWOSMOD_MODEL_INSERT_def SP_MID_FWOSMOD_MODEL_INSERT = new SP_MID_FWOSMOD_MODEL_INSERT_def();
            public class SP_MID_FWOSMOD_MODEL_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FWOSMOD_MODEL_INSERT.SQL"

                private stringParameter FWOSMOD_ID;
                private floatParameter FWOSMOD_DEFAULT;
                private intParameter FWOSMOD_RID; //Declare Output Parameter

                public SP_MID_FWOSMOD_MODEL_INSERT_def()
                {
                    base.procedureName = "SP_MID_FWOSMOD_MODEL_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL");
                    FWOSMOD_ID = new stringParameter("@FWOSMOD_ID", base.inputParameterList);
                    FWOSMOD_DEFAULT = new floatParameter("@FWOSMOD_DEFAULT", base.inputParameterList);

                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              string FWOSMOD_ID,
                                              double? FWOSMOD_DEFAULT
                                             )
                {
                    lock (typeof(SP_MID_FWOSMOD_MODEL_INSERT_def))
                    {
                        this.FWOSMOD_ID.SetValue(FWOSMOD_ID);
                        this.FWOSMOD_DEFAULT.SetValue(FWOSMOD_DEFAULT);
                        this.FWOSMOD_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_UPDATE_def MID_FWOS_MODIFIER_MODEL_UPDATE = new MID_FWOS_MODIFIER_MODEL_UPDATE_def();
            public class MID_FWOS_MODIFIER_MODEL_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FWOS_MODIFIER_MODEL_UPDATE.SQL"

                private intParameter FWOSMOD_RID;
                private stringParameter FWOSMOD_ID;
                private floatParameter FWOSMOD_DEFAULT;

                public MID_FWOS_MODIFIER_MODEL_UPDATE_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL");
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                    FWOSMOD_ID = new stringParameter("@FWOSMOD_ID", base.inputParameterList);
                    FWOSMOD_DEFAULT = new floatParameter("@FWOSMOD_DEFAULT", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? FWOSMOD_RID,
                                  string FWOSMOD_ID,
                                  double? FWOSMOD_DEFAULT
                                  )
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_UPDATE_def))
                    {
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        this.FWOSMOD_ID.SetValue(FWOSMOD_ID);
                        this.FWOSMOD_DEFAULT.SetValue(FWOSMOD_DEFAULT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_CRIT_INS_def SP_MID_ND_SZ_CRV_CRIT_INS = new SP_MID_ND_SZ_CRV_CRIT_INS_def();
            public class SP_MID_ND_SZ_CRV_CRIT_INS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ND_SZ_CRV_CRIT_INS.SQL"

                private intParameter HN_RID;
                private intParameter PH_OFFSET_IND;
                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter PHL_OFFSET;
                private intParameter CDR_RID;
                private charParameter APPLY_LOST_SALES_IND;
                private intParameter OLL_RID;
                private intParameter CUSTOM_OLL_RID;
                private intParameter SIZE_GROUP_RID;
                private stringParameter CURVE_NAME;
                private intParameter SG_RID;
                private intParameter NSCCD_RID; //Declare Output Parameter

                public SP_MID_ND_SZ_CRV_CRIT_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_CRIT_INS";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("NODE_SIZE_CURVE_CRITERIA_DETAIL");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PH_OFFSET_IND = new intParameter("@PH_OFFSET_IND", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    PHL_OFFSET = new intParameter("@PHL_OFFSET", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    APPLY_LOST_SALES_IND = new charParameter("@APPLY_LOST_SALES_IND", base.inputParameterList);
                    OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                    CURVE_NAME = new stringParameter("@CURVE_NAME", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);

                    NSCCD_RID = new intParameter("@NSCCD_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? HN_RID,
                                              int? PH_OFFSET_IND,
                                              int? PH_RID,
                                              int? PHL_SEQUENCE,
                                              int? PHL_OFFSET,
                                              int? CDR_RID,
                                              char? APPLY_LOST_SALES_IND,
                                              int? OLL_RID,
                                              int? CUSTOM_OLL_RID,
                                              int? SIZE_GROUP_RID,
                                              string CURVE_NAME,
                                              int? SG_RID
                                              )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_CRIT_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_OFFSET_IND.SetValue(PH_OFFSET_IND);
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.PHL_OFFSET.SetValue(PHL_OFFSET);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.APPLY_LOST_SALES_IND.SetValue(APPLY_LOST_SALES_IND);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.CURVE_NAME.SetValue(CURVE_NAME);
                        this.SG_RID.SetValue(SG_RID);
                        this.NSCCD_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_STOCK_MODIFIER_MODEL_UPDATE_def MID_STOCK_MODIFIER_MODEL_UPDATE = new MID_STOCK_MODIFIER_MODEL_UPDATE_def();
            public class MID_STOCK_MODIFIER_MODEL_UPDATE_def : baseStoredProcedure
            {
                private intParameter STKMOD_RID;
                private stringParameter STKMOD_ID;
                private floatParameter STKMOD_DEFAULT;

                public MID_STOCK_MODIFIER_MODEL_UPDATE_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL");
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                    STKMOD_ID = new stringParameter("@STKMOD_ID", base.inputParameterList);
                    STKMOD_DEFAULT = new floatParameter("@STKMOD_DEFAULT", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? STKMOD_RID,
                                  string STKMOD_ID,
                                  double? STKMOD_DEFAULT
                                  )
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_UPDATE_def))
                    {
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        this.STKMOD_ID.SetValue(STKMOD_ID);
                        this.STKMOD_DEFAULT.SetValue(STKMOD_DEFAULT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STOCK_MODIFIER_MODEL_DELETE_def MID_STOCK_MODIFIER_MODEL_DELETE = new MID_STOCK_MODIFIER_MODEL_DELETE_def();
            public class MID_STOCK_MODIFIER_MODEL_DELETE_def : baseStoredProcedure
            {
                private intParameter STKMOD_RID;

                public MID_STOCK_MODIFIER_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL");
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? STKMOD_RID)
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_DELETE_def))
                    {
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STOCK_MODIFIER_MODEL_ENTRY_READ_def MID_STOCK_MODIFIER_MODEL_ENTRY_READ = new MID_STOCK_MODIFIER_MODEL_ENTRY_READ_def();
            public class MID_STOCK_MODIFIER_MODEL_ENTRY_READ_def : baseStoredProcedure
            {
                private intParameter STKMOD_RID;

                public MID_STOCK_MODIFIER_MODEL_ENTRY_READ_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL_ENTRY");
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? STKMOD_RID)
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_ENTRY_READ_def))
                    {
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT_def MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT = new MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT_def();
            public class MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT_def : baseStoredProcedure
            {
                private intParameter STKMOD_RID;
                private intParameter STKMOD_SEQUENCE;
                private floatParameter STKMOD_VALUE;
                private intParameter CDR_RID;

                public MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL_ENTRY");
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                    STKMOD_SEQUENCE = new intParameter("@STKMOD_SEQUENCE", base.inputParameterList);
                    STKMOD_VALUE = new floatParameter("@STKMOD_VALUE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? STKMOD_RID,
                                  int? STKMOD_SEQUENCE,
                                  double? STKMOD_VALUE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT_def))
                    {
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        this.STKMOD_SEQUENCE.SetValue(STKMOD_SEQUENCE);
                        this.STKMOD_VALUE.SetValue(STKMOD_VALUE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE_def MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE = new MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE_def();
            public class MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE_def : baseStoredProcedure
            {
                private intParameter STKMOD_RID;

                public MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STOCK_MODIFIER_MODEL_ENTRY");
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? STKMOD_RID)
                {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_ENTRY_DELETE_def))
                    {
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_READ_ALL_def MID_SALES_MODIFIER_MODEL_READ_ALL = new MID_SALES_MODIFIER_MODEL_READ_ALL_def();
            public class MID_SALES_MODIFIER_MODEL_READ_ALL_def : baseStoredProcedure
            {

                public MID_SALES_MODIFIER_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SALES_MODIFIER_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_UPDATE_def MID_SALES_MODIFIER_MODEL_UPDATE = new MID_SALES_MODIFIER_MODEL_UPDATE_def();
            public class MID_SALES_MODIFIER_MODEL_UPDATE_def : baseStoredProcedure
            {
                private intParameter SLSMOD_RID;
                private stringParameter SLSMOD_ID;
                private floatParameter SLSMOD_DEFAULT;

                public MID_SALES_MODIFIER_MODEL_UPDATE_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SALES_MODIFIER_MODEL");
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                    SLSMOD_ID = new stringParameter("@SLSMOD_ID", base.inputParameterList);
                    SLSMOD_DEFAULT = new floatParameter("@SLSMOD_DEFAULT", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? SLSMOD_RID,
                                  string SLSMOD_ID,
                                  double? SLSMOD_DEFAULT
                                  )
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_UPDATE_def))
                    {
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        this.SLSMOD_ID.SetValue(SLSMOD_ID);
                        this.SLSMOD_DEFAULT.SetValue(SLSMOD_DEFAULT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_DELETE_def MID_SALES_MODIFIER_MODEL_DELETE = new MID_SALES_MODIFIER_MODEL_DELETE_def();
            public class MID_SALES_MODIFIER_MODEL_DELETE_def : baseStoredProcedure
            {
                private intParameter SLSMOD_RID;

                public MID_SALES_MODIFIER_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SALES_MODIFIER_MODEL");
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SLSMOD_RID)
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_DELETE_def))
                    {
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_ENTRY_READ_def MID_SALES_MODIFIER_MODEL_ENTRY_READ = new MID_SALES_MODIFIER_MODEL_ENTRY_READ_def();
            public class MID_SALES_MODIFIER_MODEL_ENTRY_READ_def : baseStoredProcedure
            {
                private intParameter SLSMOD_RID;

                public MID_SALES_MODIFIER_MODEL_ENTRY_READ_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SALES_MODIFIER_MODEL_ENTRY");
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? SLSMOD_RID)
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_ENTRY_READ_def))
                    {
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_ENTRY_INSERT_def MID_SALES_MODIFIER_MODEL_ENTRY_INSERT = new MID_SALES_MODIFIER_MODEL_ENTRY_INSERT_def();
            public class MID_SALES_MODIFIER_MODEL_ENTRY_INSERT_def : baseStoredProcedure
            {
                private intParameter SLSMOD_RID;
                private intParameter SLSMOD_SEQUENCE;
                private floatParameter SLSMOD_VALUE;
                private intParameter CDR_RID;

                public MID_SALES_MODIFIER_MODEL_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SALES_MODIFIER_MODEL_ENTRY");
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                    SLSMOD_SEQUENCE = new intParameter("@SLSMOD_SEQUENCE", base.inputParameterList);
                    SLSMOD_VALUE = new floatParameter("@SLSMOD_VALUE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? SLSMOD_RID,
                                  int? SLSMOD_SEQUENCE,
                                  double? SLSMOD_VALUE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_ENTRY_INSERT_def))
                    {
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        this.SLSMOD_SEQUENCE.SetValue(SLSMOD_SEQUENCE);
                        this.SLSMOD_VALUE.SetValue(SLSMOD_VALUE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SALES_MODIFIER_MODEL_ENTRY_DELETE_def MID_SALES_MODIFIER_MODEL_ENTRY_DELETE = new MID_SALES_MODIFIER_MODEL_ENTRY_DELETE_def();
            public class MID_SALES_MODIFIER_MODEL_ENTRY_DELETE_def : baseStoredProcedure
            {
                private intParameter SLSMOD_RID;

                public MID_SALES_MODIFIER_MODEL_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_SALES_MODIFIER_MODEL_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SALES_MODIFIER_MODEL_ENTRY");
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? SLSMOD_RID)
                {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_ENTRY_DELETE_def))
                    {
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_READ_ALL_def MID_FWOS_MODIFIER_MODEL_READ_ALL = new MID_FWOS_MODIFIER_MODEL_READ_ALL_def();
            public class MID_FWOS_MODIFIER_MODEL_READ_ALL_def : baseStoredProcedure
            {

                public MID_FWOS_MODIFIER_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_DELETE_def MID_FWOS_MODIFIER_MODEL_DELETE = new MID_FWOS_MODIFIER_MODEL_DELETE_def();
            public class MID_FWOS_MODIFIER_MODEL_DELETE_def : baseStoredProcedure
            {
                private intParameter FWOSMOD_RID;

                public MID_FWOS_MODIFIER_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL");
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? FWOSMOD_RID)
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_DELETE_def))
                    {
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_ENTRY_READ_def MID_FWOS_MODIFIER_MODEL_ENTRY_READ = new MID_FWOS_MODIFIER_MODEL_ENTRY_READ_def();
            public class MID_FWOS_MODIFIER_MODEL_ENTRY_READ_def : baseStoredProcedure
            {
                private intParameter FWOSMOD_RID;

                public MID_FWOS_MODIFIER_MODEL_ENTRY_READ_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL_ENTRY");
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? FWOSMOD_RID)
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_ENTRY_READ_def))
                    {
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT_def MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT = new MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT_def();
            public class MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT_def : baseStoredProcedure
            {
                private intParameter FWOSMOD_RID;
                private intParameter FWOSMOD_SEQUENCE;
                private floatParameter FWOSMOD_VALUE;
                private intParameter CDR_RID;

                public MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL_ENTRY");
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                    FWOSMOD_SEQUENCE = new intParameter("@FWOSMOD_SEQUENCE", base.inputParameterList);
                    FWOSMOD_VALUE = new floatParameter("@FWOSMOD_VALUE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? FWOSMOD_RID,
                                  int? FWOSMOD_SEQUENCE,
                                  double? FWOSMOD_VALUE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT_def))
                    {
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        this.FWOSMOD_SEQUENCE.SetValue(FWOSMOD_SEQUENCE);
                        this.FWOSMOD_VALUE.SetValue(FWOSMOD_VALUE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE_def MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE = new MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE_def();
            public class MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE_def : baseStoredProcedure
            {
                private intParameter FWOSMOD_RID;

                public MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FWOS_MODIFIER_MODEL_ENTRY");
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? FWOSMOD_RID)
                {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE_def))
                    {
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_READ_ALL_def MID_FWOS_MAX_MODEL_READ_ALL = new MID_FWOS_MAX_MODEL_READ_ALL_def();
            public class MID_FWOS_MAX_MODEL_READ_ALL_def : baseStoredProcedure
            {

                public MID_FWOS_MAX_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FWOS_MAX_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_UPDATE_def MID_FWOS_MAX_MODEL_UPDATE = new MID_FWOS_MAX_MODEL_UPDATE_def();
            public class MID_FWOS_MAX_MODEL_UPDATE_def : baseStoredProcedure
            {
                private intParameter FWOSMAX_RID;
                private stringParameter FWOSMAX_ID;
                private floatParameter FWOSMAX_DEFAULT;

                public MID_FWOS_MAX_MODEL_UPDATE_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("FWOS_MAX_MODEL");
                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.inputParameterList);
                    FWOSMAX_ID = new stringParameter("@FWOSMAX_ID", base.inputParameterList);
                    FWOSMAX_DEFAULT = new floatParameter("@FWOSMAX_DEFAULT", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? FWOSMAX_RID,
                                  string FWOSMAX_ID,
                                  double? FWOSMAX_DEFAULT
                                  )
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_UPDATE_def))
                    {
                        this.FWOSMAX_RID.SetValue(FWOSMAX_RID);
                        this.FWOSMAX_ID.SetValue(FWOSMAX_ID);
                        this.FWOSMAX_DEFAULT.SetValue(FWOSMAX_DEFAULT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_DELETE_def MID_FWOS_MAX_MODEL_DELETE = new MID_FWOS_MAX_MODEL_DELETE_def();
            public class MID_FWOS_MAX_MODEL_DELETE_def : baseStoredProcedure
            {
                private intParameter FWOSMAX_RID;

                public MID_FWOS_MAX_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FWOS_MAX_MODEL");
                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? FWOSMAX_RID)
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_DELETE_def))
                    {
                        this.FWOSMAX_RID.SetValue(FWOSMAX_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_ENTRY_READ_def MID_FWOS_MAX_MODEL_ENTRY_READ = new MID_FWOS_MAX_MODEL_ENTRY_READ_def();
            public class MID_FWOS_MAX_MODEL_ENTRY_READ_def : baseStoredProcedure
            {
                private intParameter FWOSMAX_RID;

                public MID_FWOS_MAX_MODEL_ENTRY_READ_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_ENTRY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FWOS_MAX_MODEL_ENTRY");
                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? FWOSMAX_RID)
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_ENTRY_READ_def))
                    {
                        this.FWOSMAX_RID.SetValue(FWOSMAX_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_ENTRY_INSERT_def MID_FWOS_MAX_MODEL_ENTRY_INSERT = new MID_FWOS_MAX_MODEL_ENTRY_INSERT_def();
            public class MID_FWOS_MAX_MODEL_ENTRY_INSERT_def : baseStoredProcedure
            {
                private intParameter FWOSMAX_RID;
                private intParameter FWOSMAX_SEQUENCE;
                private floatParameter FWOSMAX_VALUE;
                private intParameter CDR_RID;

                public MID_FWOS_MAX_MODEL_ENTRY_INSERT_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_ENTRY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("FWOS_MAX_MODEL_ENTRY");
                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.inputParameterList);
                    FWOSMAX_SEQUENCE = new intParameter("@FWOSMAX_SEQUENCE", base.inputParameterList);
                    FWOSMAX_VALUE = new floatParameter("@FWOSMAX_VALUE", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? FWOSMAX_RID,
                                  int? FWOSMAX_SEQUENCE,
                                  double? FWOSMAX_VALUE,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_ENTRY_INSERT_def))
                    {
                        this.FWOSMAX_RID.SetValue(FWOSMAX_RID);
                        this.FWOSMAX_SEQUENCE.SetValue(FWOSMAX_SEQUENCE);
                        this.FWOSMAX_VALUE.SetValue(FWOSMAX_VALUE);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_FWOS_MAX_MODEL_ENTRY_DELETE_def MID_FWOS_MAX_MODEL_ENTRY_DELETE = new MID_FWOS_MAX_MODEL_ENTRY_DELETE_def();
            public class MID_FWOS_MAX_MODEL_ENTRY_DELETE_def : baseStoredProcedure
            {
                private intParameter FWOSMAX_RID;

                public MID_FWOS_MAX_MODEL_ENTRY_DELETE_def()
                {
                    base.procedureName = "MID_FWOS_MAX_MODEL_ENTRY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("FWOS_MAX_MODEL_ENTRY");
                    FWOSMAX_RID = new intParameter("@FWOSMAX_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? FWOSMAX_RID)
                {
                    lock (typeof(MID_FWOS_MAX_MODEL_ENTRY_DELETE_def))
                    {
                        this.FWOSMAX_RID.SetValue(FWOSMAX_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DEFAULTS_READ_def MID_DAILY_PERCENTAGES_DEFAULTS_READ = new MID_DAILY_PERCENTAGES_DEFAULTS_READ_def();
            public class MID_DAILY_PERCENTAGES_DEFAULTS_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_DAILY_PERCENTAGES_DEFAULTS_READ_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DEFAULTS_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("DAILY_PERCENTAGES_DEFAULTS");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DEFAULTS_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DEFAULTS_INSERT_def MID_DAILY_PERCENTAGES_DEFAULTS_INSERT = new MID_DAILY_PERCENTAGES_DEFAULTS_INSERT_def();
            public class MID_DAILY_PERCENTAGES_DEFAULTS_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private floatParameter DAY1;
                private floatParameter DAY2;
                private floatParameter DAY3;
                private floatParameter DAY4;
                private floatParameter DAY5;
                private floatParameter DAY6;
                private floatParameter DAY7;

                public MID_DAILY_PERCENTAGES_DEFAULTS_INSERT_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DEFAULTS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("DAILY_PERCENTAGES_DEFAULTS");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    DAY1 = new floatParameter("@DAY1", base.inputParameterList);
                    DAY2 = new floatParameter("@DAY2", base.inputParameterList);
                    DAY3 = new floatParameter("@DAY3", base.inputParameterList);
                    DAY4 = new floatParameter("@DAY4", base.inputParameterList);
                    DAY5 = new floatParameter("@DAY5", base.inputParameterList);
                    DAY6 = new floatParameter("@DAY6", base.inputParameterList);
                    DAY7 = new floatParameter("@DAY7", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  double? DAY1,
                                  double? DAY2,
                                  double? DAY3,
                                  double? DAY4,
                                  double? DAY5,
                                  double? DAY6,
                                  double? DAY7
                                  )
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DEFAULTS_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.DAY1.SetValue(DAY1);
                        this.DAY2.SetValue(DAY2);
                        this.DAY3.SetValue(DAY3);
                        this.DAY4.SetValue(DAY4);
                        this.DAY5.SetValue(DAY5);
                        this.DAY6.SetValue(DAY6);
                        this.DAY7.SetValue(DAY7);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_def MID_DAILY_PERCENTAGES_DEFAULTS_DELETE = new MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_def();
            public class MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DEFAULTS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES_DEFAULTS");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL_def MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL = new MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL_def();
            public class MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES_DEFAULTS");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DEFAULTS_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_READ_def MID_DAILY_PERCENTAGES_READ = new MID_DAILY_PERCENTAGES_READ_def();
            public class MID_DAILY_PERCENTAGES_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_DAILY_PERCENTAGES_READ_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("DAILY_PERCENTAGES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_INSERT_def MID_DAILY_PERCENTAGES_INSERT = new MID_DAILY_PERCENTAGES_INSERT_def();
            public class MID_DAILY_PERCENTAGES_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter CDR_RID;
                private floatParameter DAY1;
                private floatParameter DAY2;
                private floatParameter DAY3;
                private floatParameter DAY4;
                private floatParameter DAY5;
                private floatParameter DAY6;
                private floatParameter DAY7;

                public MID_DAILY_PERCENTAGES_INSERT_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("DAILY_PERCENTAGES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    DAY1 = new floatParameter("@DAY1", base.inputParameterList);
                    DAY2 = new floatParameter("@DAY2", base.inputParameterList);
                    DAY3 = new floatParameter("@DAY3", base.inputParameterList);
                    DAY4 = new floatParameter("@DAY4", base.inputParameterList);
                    DAY5 = new floatParameter("@DAY5", base.inputParameterList);
                    DAY6 = new floatParameter("@DAY6", base.inputParameterList);
                    DAY7 = new floatParameter("@DAY7", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? CDR_RID,
                                  double? DAY1,
                                  double? DAY2,
                                  double? DAY3,
                                  double? DAY4,
                                  double? DAY5,
                                  double? DAY6,
                                  double? DAY7
                                  )
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.DAY1.SetValue(DAY1);
                        this.DAY2.SetValue(DAY2);
                        this.DAY3.SetValue(DAY3);
                        this.DAY4.SetValue(DAY4);
                        this.DAY5.SetValue(DAY5);
                        this.DAY6.SetValue(DAY6);
                        this.DAY7.SetValue(DAY7);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DELETE_def MID_DAILY_PERCENTAGES_DELETE = new MID_DAILY_PERCENTAGES_DELETE_def();
            public class MID_DAILY_PERCENTAGES_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter CDR_RID;

                public MID_DAILY_PERCENTAGES_DELETE_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_DAILY_PERCENTAGES_DELETE_ALL_def MID_DAILY_PERCENTAGES_DELETE_ALL = new MID_DAILY_PERCENTAGES_DELETE_ALL_def();
            public class MID_DAILY_PERCENTAGES_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_DAILY_PERCENTAGES_DELETE_ALL_def()
                {
                    base.procedureName = "MID_DAILY_PERCENTAGES_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_DAILY_PERCENTAGES_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_READ_def MID_STORE_ELIGIBILITY_READ = new MID_STORE_ELIGIBILITY_READ_def();
            public class MID_STORE_ELIGIBILITY_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_STORE_ELIGIBILITY_READ_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_ELIGIBILITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_INSERT_def MID_STORE_ELIGIBILITY_INSERT = new MID_STORE_ELIGIBILITY_INSERT_def();
            public class MID_STORE_ELIGIBILITY_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter EM_RID;
                private charParameter USE_ELIGIBILITY;
                private charParameter INELIGIBLE;
                private intParameter STKMOD_TYPE;
                private intParameter STKMOD_RID;
                private floatParameter STKMOD_PCT;
                private intParameter SLSMOD_TYPE;
                private intParameter SLSMOD_RID;
                private floatParameter SLSMOD_PCT;
                private intParameter FWOSMOD_TYPE;
                private intParameter FWOSMOD_RID;
                private floatParameter FWOSMOD_PCT;
                private intParameter SIMILAR_STORE_TYPE;
                private floatParameter SIMILAR_STORE_RATIO;
                private intParameter UNTIL_DATE;
                private charParameter PRESENTATION_PLUS_SALES_IND;
                private intParameter STOCK_LEAD_WEEKS;
                private datetimeParameter UPDATE_DATE;

                public MID_STORE_ELIGIBILITY_INSERT_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STORE_ELIGIBILITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    USE_ELIGIBILITY = new charParameter("@USE_ELIGIBILITY", base.inputParameterList);
                    INELIGIBLE = new charParameter("@INELIGIBLE", base.inputParameterList);
                    STKMOD_TYPE = new intParameter("@STKMOD_TYPE", base.inputParameterList);
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                    STKMOD_PCT = new floatParameter("@STKMOD_PCT", base.inputParameterList);
                    SLSMOD_TYPE = new intParameter("@SLSMOD_TYPE", base.inputParameterList);
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                    SLSMOD_PCT = new floatParameter("@SLSMOD_PCT", base.inputParameterList);
                    FWOSMOD_TYPE = new intParameter("@FWOSMOD_TYPE", base.inputParameterList);
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                    FWOSMOD_PCT = new floatParameter("@FWOSMOD_PCT", base.inputParameterList);
                    SIMILAR_STORE_TYPE = new intParameter("@SIMILAR_STORE_TYPE", base.inputParameterList);
                    SIMILAR_STORE_RATIO = new floatParameter("@SIMILAR_STORE_RATIO", base.inputParameterList);
                    UNTIL_DATE = new intParameter("@UNTIL_DATE", base.inputParameterList);
                    PRESENTATION_PLUS_SALES_IND = new charParameter("@PRESENTATION_PLUS_SALES_IND", base.inputParameterList);
                    STOCK_LEAD_WEEKS = new intParameter("@STOCK_LEAD_WEEKS", base.inputParameterList);
                    UPDATE_DATE = new datetimeParameter("@UPDATE_DATE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? EM_RID,
                                  char? USE_ELIGIBILITY,
                                  char? INELIGIBLE,
                                  int? STKMOD_TYPE,
                                  int? STKMOD_RID,
                                  double? STKMOD_PCT,
                                  int? SLSMOD_TYPE,
                                  int? SLSMOD_RID,
                                  double? SLSMOD_PCT,
                                  int? FWOSMOD_TYPE,
                                  int? FWOSMOD_RID,
                                  double? FWOSMOD_PCT,
                                  int? SIMILAR_STORE_TYPE,
                                  double? SIMILAR_STORE_RATIO,
                                  int? UNTIL_DATE,
                                  char? PRESENTATION_PLUS_SALES_IND,
                                  int? STOCK_LEAD_WEEKS,
                                  DateTime? UPDATE_DATE
                                  )
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.EM_RID.SetValue(EM_RID);
                        this.USE_ELIGIBILITY.SetValue(USE_ELIGIBILITY);
                        this.INELIGIBLE.SetValue(INELIGIBLE);
                        this.STKMOD_TYPE.SetValue(STKMOD_TYPE);
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        this.STKMOD_PCT.SetValue(STKMOD_PCT);
                        this.SLSMOD_TYPE.SetValue(SLSMOD_TYPE);
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        this.SLSMOD_PCT.SetValue(SLSMOD_PCT);
                        this.FWOSMOD_TYPE.SetValue(FWOSMOD_TYPE);
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        this.FWOSMOD_PCT.SetValue(FWOSMOD_PCT);
                        this.SIMILAR_STORE_TYPE.SetValue(SIMILAR_STORE_TYPE);
                        this.SIMILAR_STORE_RATIO.SetValue(SIMILAR_STORE_RATIO);
                        this.UNTIL_DATE.SetValue(UNTIL_DATE);
                        this.PRESENTATION_PLUS_SALES_IND.SetValue(PRESENTATION_PLUS_SALES_IND);
                        this.STOCK_LEAD_WEEKS.SetValue(STOCK_LEAD_WEEKS);
                        this.UPDATE_DATE.SetValue(UPDATE_DATE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_UPDATE_def MID_STORE_ELIGIBILITY_UPDATE = new MID_STORE_ELIGIBILITY_UPDATE_def();
            public class MID_STORE_ELIGIBILITY_UPDATE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter EM_RID;
                private charParameter USE_ELIGIBILITY;
                private charParameter INELIGIBLE;
                private intParameter STKMOD_TYPE;
                private intParameter STKMOD_RID;
                private floatParameter STKMOD_PCT;
                private intParameter SLSMOD_TYPE;
                private intParameter SLSMOD_RID;
                private floatParameter SLSMOD_PCT;
                private intParameter FWOSMOD_TYPE;
                private intParameter FWOSMOD_RID;
                private floatParameter FWOSMOD_PCT;
                private intParameter SIMILAR_STORE_TYPE;
                private floatParameter SIMILAR_STORE_RATIO;
                private intParameter UNTIL_DATE;
                private charParameter PRESENTATION_PLUS_SALES_IND;
                private intParameter STOCK_LEAD_WEEKS;
                private datetimeParameter UPDATE_DATE;

                public MID_STORE_ELIGIBILITY_UPDATE_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_ELIGIBILITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    EM_RID = new intParameter("@EM_RID", base.inputParameterList);
                    USE_ELIGIBILITY = new charParameter("@USE_ELIGIBILITY", base.inputParameterList);
                    INELIGIBLE = new charParameter("@INELIGIBLE", base.inputParameterList);
                    STKMOD_TYPE = new intParameter("@STKMOD_TYPE", base.inputParameterList);
                    STKMOD_RID = new intParameter("@STKMOD_RID", base.inputParameterList);
                    STKMOD_PCT = new floatParameter("@STKMOD_PCT", base.inputParameterList);
                    SLSMOD_TYPE = new intParameter("@SLSMOD_TYPE", base.inputParameterList);
                    SLSMOD_RID = new intParameter("@SLSMOD_RID", base.inputParameterList);
                    SLSMOD_PCT = new floatParameter("@SLSMOD_PCT", base.inputParameterList);
                    FWOSMOD_TYPE = new intParameter("@FWOSMOD_TYPE", base.inputParameterList);
                    FWOSMOD_RID = new intParameter("@FWOSMOD_RID", base.inputParameterList);
                    FWOSMOD_PCT = new floatParameter("@FWOSMOD_PCT", base.inputParameterList);
                    SIMILAR_STORE_TYPE = new intParameter("@SIMILAR_STORE_TYPE", base.inputParameterList);
                    SIMILAR_STORE_RATIO = new floatParameter("@SIMILAR_STORE_RATIO", base.inputParameterList);
                    UNTIL_DATE = new intParameter("@UNTIL_DATE", base.inputParameterList);
                    PRESENTATION_PLUS_SALES_IND = new charParameter("@PRESENTATION_PLUS_SALES_IND", base.inputParameterList);
                    STOCK_LEAD_WEEKS = new intParameter("@STOCK_LEAD_WEEKS", base.inputParameterList);
                    UPDATE_DATE = new datetimeParameter("@UPDATE_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? EM_RID,
                                  char? USE_ELIGIBILITY,
                                  char? INELIGIBLE,
                                  int? STKMOD_TYPE,
                                  int? STKMOD_RID,
                                  double? STKMOD_PCT,
                                  int? SLSMOD_TYPE,
                                  int? SLSMOD_RID,
                                  double? SLSMOD_PCT,
                                  int? FWOSMOD_TYPE,
                                  int? FWOSMOD_RID,
                                  double? FWOSMOD_PCT,
                                  int? SIMILAR_STORE_TYPE,
                                  double? SIMILAR_STORE_RATIO,
                                  int? UNTIL_DATE,
                                  char? PRESENTATION_PLUS_SALES_IND,
                                  int? STOCK_LEAD_WEEKS,
                                  DateTime? UPDATE_DATE
                                  )
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_UPDATE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.EM_RID.SetValue(EM_RID);
                        this.USE_ELIGIBILITY.SetValue(USE_ELIGIBILITY);
                        this.INELIGIBLE.SetValue(INELIGIBLE);
                        this.STKMOD_TYPE.SetValue(STKMOD_TYPE);
                        this.STKMOD_RID.SetValue(STKMOD_RID);
                        this.STKMOD_PCT.SetValue(STKMOD_PCT);
                        this.SLSMOD_TYPE.SetValue(SLSMOD_TYPE);
                        this.SLSMOD_RID.SetValue(SLSMOD_RID);
                        this.SLSMOD_PCT.SetValue(SLSMOD_PCT);
                        this.FWOSMOD_TYPE.SetValue(FWOSMOD_TYPE);
                        this.FWOSMOD_RID.SetValue(FWOSMOD_RID);
                        this.FWOSMOD_PCT.SetValue(FWOSMOD_PCT);
                        this.SIMILAR_STORE_TYPE.SetValue(SIMILAR_STORE_TYPE);
                        this.SIMILAR_STORE_RATIO.SetValue(SIMILAR_STORE_RATIO);
                        this.UNTIL_DATE.SetValue(UNTIL_DATE);
                        this.PRESENTATION_PLUS_SALES_IND.SetValue(PRESENTATION_PLUS_SALES_IND);
                        this.STOCK_LEAD_WEEKS.SetValue(STOCK_LEAD_WEEKS);
                        this.UPDATE_DATE.SetValue(UPDATE_DATE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_DELETE_ALL_def MID_STORE_ELIGIBILITY_DELETE_ALL = new MID_STORE_ELIGIBILITY_DELETE_ALL_def();
            public class MID_STORE_ELIGIBILITY_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_STORE_ELIGIBILITY_DELETE_ALL_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_ELIGIBILITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_ELIGIBILITY_DELETE_def MID_STORE_ELIGIBILITY_DELETE = new MID_STORE_ELIGIBILITY_DELETE_def();
            public class MID_STORE_ELIGIBILITY_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_STORE_ELIGIBILITY_DELETE_def()
                {
                    base.procedureName = "MID_STORE_ELIGIBILITY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_ELIGIBILITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_STORE_ELIGIBILITY_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_GET_SIMILAR_STORE_RIDS_def MID_GET_SIMILAR_STORE_RIDS = new MID_GET_SIMILAR_STORE_RIDS_def();
            public class MID_GET_SIMILAR_STORE_RIDS_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_GET_SIMILAR_STORE_RIDS_def()
                {
                    base.procedureName = "MID_GET_SIMILAR_STORE_RIDS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_GET_SIMILAR_STORE_RIDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIMILAR_STORES_INSERT_def MID_SIMILAR_STORES_INSERT = new MID_SIMILAR_STORES_INSERT_def();
            public class MID_SIMILAR_STORES_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter SS_RID;

                public MID_SIMILAR_STORES_INSERT_def()
                {
                    base.procedureName = "MID_SIMILAR_STORES_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("SIMILAR_STORES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    SS_RID = new intParameter("@SS_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? SS_RID
                                  )
                {
                    lock (typeof(MID_SIMILAR_STORES_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.SS_RID.SetValue(SS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_SIMILAR_STORES_DELETE_ALL_def MID_SIMILAR_STORES_DELETE_ALL = new MID_SIMILAR_STORES_DELETE_ALL_def();
            public class MID_SIMILAR_STORES_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_SIMILAR_STORES_DELETE_ALL_def()
                {
                    base.procedureName = "MID_SIMILAR_STORES_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIMILAR_STORES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_SIMILAR_STORES_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_SIMILAR_STORES_DELETE_def MID_SIMILAR_STORES_DELETE = new MID_SIMILAR_STORES_DELETE_def();
            public class MID_SIMILAR_STORES_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_SIMILAR_STORES_DELETE_def()
                {
                    base.procedureName = "MID_SIMILAR_STORES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIMILAR_STORES");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_SIMILAR_STORES_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_CRITERIA_READ_def MID_NODE_SIZE_CURVE_CRITERIA_READ = new MID_NODE_SIZE_CURVE_CRITERIA_READ_def();
            public class MID_NODE_SIZE_CURVE_CRITERIA_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_CRITERIA_READ_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_CRITERIA_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_CRITERIA");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_CRITERIA_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ_def MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ = new MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ_def();
            public class MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_CRITERIA_DEFAULT");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_CRITERIA_DEFAULT_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_CRIT_UPD_def SP_MID_ND_SZ_CRV_CRIT_UPD = new SP_MID_ND_SZ_CRV_CRIT_UPD_def();
            public class SP_MID_ND_SZ_CRV_CRIT_UPD_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter NSCCD_RID;
                private intParameter PH_OFFSET_IND;
                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter PHL_OFFSET;
                private intParameter CDR_RID;
                private charParameter APPLY_LOST_SALES_IND;
                private intParameter OLL_RID;
                private intParameter CUSTOM_OLL_RID;
                private intParameter SIZE_GROUP_RID;
                private stringParameter CURVE_NAME;
                private intParameter SG_RID;

                public SP_MID_ND_SZ_CRV_CRIT_UPD_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_CRIT_UPD";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    NSCCD_RID = new intParameter("@NSCCD_RID", base.inputParameterList);
                    PH_OFFSET_IND = new intParameter("@PH_OFFSET_IND", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    PHL_OFFSET = new intParameter("@PHL_OFFSET", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    APPLY_LOST_SALES_IND = new charParameter("@APPLY_LOST_SALES_IND", base.inputParameterList);
                    OLL_RID = new intParameter("@OLL_RID", base.inputParameterList);
                    CUSTOM_OLL_RID = new intParameter("@CUSTOM_OLL_RID", base.inputParameterList);
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                    CURVE_NAME = new stringParameter("@CURVE_NAME", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? NSCCD_RID,
                                  int? PH_OFFSET_IND,
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  int? PHL_OFFSET,
                                  int? CDR_RID,
                                  char? APPLY_LOST_SALES_IND,
                                  int? OLL_RID,
                                  int? CUSTOM_OLL_RID,
                                  int? SIZE_GROUP_RID,
                                  string CURVE_NAME,
                                  int? SG_RID
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_CRIT_UPD_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.NSCCD_RID.SetValue(NSCCD_RID);
                        this.PH_OFFSET_IND.SetValue(PH_OFFSET_IND);
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.PHL_OFFSET.SetValue(PHL_OFFSET);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.APPLY_LOST_SALES_IND.SetValue(APPLY_LOST_SALES_IND);
                        this.OLL_RID.SetValue(OLL_RID);
                        this.CUSTOM_OLL_RID.SetValue(CUSTOM_OLL_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.CURVE_NAME.SetValue(CURVE_NAME);
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_CRIT_DEF_UPD_def SP_MID_ND_SZ_CRV_CRIT_DEF_UPD = new SP_MID_ND_SZ_CRV_CRIT_DEF_UPD_def();
            public class SP_MID_ND_SZ_CRV_CRIT_DEF_UPD_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter NSCCD_RID;

                public SP_MID_ND_SZ_CRV_CRIT_DEF_UPD_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_CRIT_DEF_UPD";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    NSCCD_RID = new intParameter("@NSCCD_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? NSCCD_RID
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_CRIT_DEF_UPD_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.NSCCD_RID.SetValue(NSCCD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_CRIT_DEL_def SP_MID_ND_SZ_CRV_CRIT_DEL = new SP_MID_ND_SZ_CRV_CRIT_DEL_def();
            public class SP_MID_ND_SZ_CRV_CRIT_DEL_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter NSCCD_RID;

                public SP_MID_ND_SZ_CRV_CRIT_DEL_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_CRIT_DEL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    NSCCD_RID = new intParameter("@NSCCD_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? NSCCD_RID
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_CRIT_DEL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.NSCCD_RID.SetValue(NSCCD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_CRIT_DELALL_def SP_MID_ND_SZ_CRV_CRIT_DELALL = new SP_MID_ND_SZ_CRV_CRIT_DELALL_def();
            public class SP_MID_ND_SZ_CRV_CRIT_DELALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter RETURN_ROWCOUNT; //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures

                public SP_MID_ND_SZ_CRV_CRIT_DELALL_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_CRIT_DELALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    RETURN_ROWCOUNT = new intParameter("@RETURN_ROWCOUNT", base.inputParameterList); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_CRIT_DELALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.RETURN_ROWCOUNT.SetValue(1); //TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_TOLERANCE_READ_def MID_NODE_SIZE_CURVE_TOLERANCE_READ = new MID_NODE_SIZE_CURVE_TOLERANCE_READ_def();
            public class MID_NODE_SIZE_CURVE_TOLERANCE_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_TOLERANCE_READ_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_TOLERANCE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_TOLERANCE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_TOLERANCE_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_TOLER_INS_def SP_MID_ND_SZ_CRV_TOLER_INS = new SP_MID_ND_SZ_CRV_TOLER_INS_def();
            public class SP_MID_ND_SZ_CRV_TOLER_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private floatParameter MINIMUM_AVERAGE;
                private intParameter PH_OFFSET_IND;
                private intParameter PH_RID;
                private intParameter PHL_SEQUENCE;
                private intParameter PHL_OFFSET;
                private floatParameter SALES_TOLERANCE;
                private intParameter INDEX_UNITS_TYPE;
                private floatParameter MIN_TOLERANCE;
                private floatParameter MAX_TOLERANCE;
                private charParameter APPLY_MIN_TO_ZERO_TOLERANCE_IND;

                public SP_MID_ND_SZ_CRV_TOLER_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_TOLER_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    MINIMUM_AVERAGE = new floatParameter("@MINIMUM_AVERAGE", base.inputParameterList);
                    PH_OFFSET_IND = new intParameter("@PH_OFFSET_IND", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    PHL_SEQUENCE = new intParameter("@PHL_SEQUENCE", base.inputParameterList);
                    PHL_OFFSET = new intParameter("@PHL_OFFSET", base.inputParameterList);
                    SALES_TOLERANCE = new floatParameter("@SALES_TOLERANCE", base.inputParameterList);
                    INDEX_UNITS_TYPE = new intParameter("@INDEX_UNITS_TYPE", base.inputParameterList);
                    MIN_TOLERANCE = new floatParameter("@MIN_TOLERANCE", base.inputParameterList);
                    MAX_TOLERANCE = new floatParameter("@MAX_TOLERANCE", base.inputParameterList);
                    APPLY_MIN_TO_ZERO_TOLERANCE_IND = new charParameter("@APPLY_MIN_TO_ZERO_TOLERANCE_IND", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  double? MINIMUM_AVERAGE,
                                  int? PH_OFFSET_IND,
                                  int? PH_RID,
                                  int? PHL_SEQUENCE,
                                  int? PHL_OFFSET,
                                  double? SALES_TOLERANCE,
                                  int? INDEX_UNITS_TYPE,
                                  double? MIN_TOLERANCE,
                                  double? MAX_TOLERANCE,
                                  char? APPLY_MIN_TO_ZERO_TOLERANCE_IND
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_TOLER_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.MINIMUM_AVERAGE.SetValue(MINIMUM_AVERAGE);
                        this.PH_OFFSET_IND.SetValue(PH_OFFSET_IND);
                        this.PH_RID.SetValue(PH_RID);
                        this.PHL_SEQUENCE.SetValue(PHL_SEQUENCE);
                        this.PHL_OFFSET.SetValue(PHL_OFFSET);
                        this.SALES_TOLERANCE.SetValue(SALES_TOLERANCE);
                        this.INDEX_UNITS_TYPE.SetValue(INDEX_UNITS_TYPE);
                        this.MIN_TOLERANCE.SetValue(MIN_TOLERANCE);
                        this.MAX_TOLERANCE.SetValue(MAX_TOLERANCE);
                        this.APPLY_MIN_TO_ZERO_TOLERANCE_IND.SetValue(APPLY_MIN_TO_ZERO_TOLERANCE_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL_def MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL = new MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL_def();
            public class MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_SIZE_CURVE_TOLERANCE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ_def MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ = new MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ_def();
            public class MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_SIMILAR_STORE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_SIMSTR_INS_def SP_MID_ND_SZ_CRV_SIMSTR_INS = new SP_MID_ND_SZ_CRV_SIMSTR_INS_def();
            public class SP_MID_ND_SZ_CRV_SIMSTR_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter SS_RID;
                private intParameter UNTIL_DATE;

                public SP_MID_ND_SZ_CRV_SIMSTR_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_SIMSTR_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    SS_RID = new intParameter("@SS_RID", base.inputParameterList);
                    UNTIL_DATE = new intParameter("@UNTIL_DATE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? SS_RID,
                                  int? UNTIL_DATE
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_SIMSTR_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.SS_RID.SetValue(SS_RID);
                        this.UNTIL_DATE.SetValue(UNTIL_DATE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_CRV_SIMSTR_UPD_def SP_MID_ND_SZ_CRV_SIMSTR_UPD = new SP_MID_ND_SZ_CRV_SIMSTR_UPD_def();
            public class SP_MID_ND_SZ_CRV_SIMSTR_UPD_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter SS_RID;
                private intParameter UNTIL_DATE;

                public SP_MID_ND_SZ_CRV_SIMSTR_UPD_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_CRV_SIMSTR_UPD";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    SS_RID = new intParameter("@SS_RID", base.inputParameterList);
                    UNTIL_DATE = new intParameter("@UNTIL_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? SS_RID,
                                  int? UNTIL_DATE
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_CRV_SIMSTR_UPD_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.SS_RID.SetValue(SS_RID);
                        this.UNTIL_DATE.SetValue(UNTIL_DATE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL_def MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL = new MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL_def();
            public class MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_SIZE_CURVE_SIMILAR_STORE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_def MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE = new MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_def();
            public class MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_SIZE_CURVE_SIMILAR_STORE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_DEL_ALL_def SP_MID_ND_SZ_OOS_DEL_ALL = new SP_MID_ND_SZ_OOS_DEL_ALL_def();
            public class SP_MID_ND_SZ_OOS_DEL_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public SP_MID_ND_SZ_OOS_DEL_ALL_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_DEL_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_DEL_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_HEADER_GET_def SP_MID_ND_SZ_OOS_HEADER_GET = new SP_MID_ND_SZ_OOS_HEADER_GET_def();
            public class SP_MID_ND_SZ_OOS_HEADER_GET_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public SP_MID_ND_SZ_OOS_HEADER_GET_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_HEADER_GET";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_HEADER_GET_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_VALUES_GET_def SP_MID_ND_SZ_OOS_VALUES_GET = new SP_MID_ND_SZ_OOS_VALUES_GET_def();
            public class SP_MID_ND_SZ_OOS_VALUES_GET_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SG_RID;
                private intParameter SG_VERSION;  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                private intParameter SIZE_GROUP_RID;
                private intParameter FOR_NODE_PROPERTIES;

                public SP_MID_ND_SZ_OOS_VALUES_GET_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_VALUES_GET";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                    FOR_NODE_PROPERTIES = new intParameter("@FOR_NODE_PROPERTIES", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, 
                                             int? HN_RID,
                                             int? SG_RID,
                                             int? SIZE_GROUP_RID,
                                             int? FOR_NODE_PROPERTIES,
                                             int? SG_VERSION  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                             )
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_VALUES_GET_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.FOR_NODE_PROPERTIES.SetValue(FOR_NODE_PROPERTIES);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        DataSet dsValues = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsValues.Tables[0].TableName = "SetLevelValues";
                        dsValues.Tables[1].TableName = "ColorSizeValues";
                        dsValues.Tables[2].TableName = "ColorCodes";
                        dsValues.Tables[3].TableName = "ColorSizeCodes";
                        return dsValues;
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_INS_def SP_MID_ND_SZ_OOS_INS = new SP_MID_ND_SZ_OOS_INS_def();
            public class SP_MID_ND_SZ_OOS_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SG_RID;
                private intParameter SIZE_GROUP_RID;

                public SP_MID_ND_SZ_OOS_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SG_RID,
                                  int? SIZE_GROUP_RID
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_GRPLVL_INS_def SP_MID_ND_SZ_OOS_GRPLVL_INS = new SP_MID_ND_SZ_OOS_GRPLVL_INS_def();
            public class SP_MID_ND_SZ_OOS_GRPLVL_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SGL_RID;
                private intParameter ROW_TYPE_ID;
                private intParameter OOS_QUANTITY;

                public SP_MID_ND_SZ_OOS_GRPLVL_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_GRPLVL_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    ROW_TYPE_ID = new intParameter("@ROW_TYPE_ID", base.inputParameterList);
                    OOS_QUANTITY = new intParameter("@OOS_QUANTITY", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SGL_RID,
                                  int? ROW_TYPE_ID,
                                  int? OOS_QUANTITY
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_GRPLVL_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.ROW_TYPE_ID.SetValue(ROW_TYPE_ID);
                        this.OOS_QUANTITY.SetValue(OOS_QUANTITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_OOS_QUANTITY_INS_def SP_MID_ND_SZ_OOS_QUANTITY_INS = new SP_MID_ND_SZ_OOS_QUANTITY_INS_def();
            public class SP_MID_ND_SZ_OOS_QUANTITY_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SGL_RID;
                private intParameter COLOR_CODE_RID;
                private intParameter SIZES_RID;
                private intParameter DIMENSIONS_RID;
                private intParameter ROW_TYPE_ID;
                private intParameter SIZE_CODE_RID;
                private intParameter OOS_QUANTITY;

                public SP_MID_ND_SZ_OOS_QUANTITY_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_OOS_QUANTITY_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                    SIZES_RID = new intParameter("@SIZES_RID", base.inputParameterList);
                    DIMENSIONS_RID = new intParameter("@DIMENSIONS_RID", base.inputParameterList);
                    ROW_TYPE_ID = new intParameter("@ROW_TYPE_ID", base.inputParameterList);
                    SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
                    OOS_QUANTITY = new intParameter("@OOS_QUANTITY", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SGL_RID,
                                  int? COLOR_CODE_RID,
                                  int? SIZES_RID,
                                  int? DIMENSIONS_RID,
                                  int? ROW_TYPE_ID,
                                  int? SIZE_CODE_RID,
                                  int? OOS_QUANTITY
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_OOS_QUANTITY_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        this.SIZES_RID.SetValue(SIZES_RID);
                        this.DIMENSIONS_RID.SetValue(DIMENSIONS_RID);
                        this.ROW_TYPE_ID.SetValue(ROW_TYPE_ID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.OOS_QUANTITY.SetValue(OOS_QUANTITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_SELLTHRU_DELETE_ALL_def MID_NODE_SIZE_SELLTHRU_DELETE_ALL = new MID_NODE_SIZE_SELLTHRU_DELETE_ALL_def();
            public class MID_NODE_SIZE_SELLTHRU_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_SELLTHRU_DELETE_ALL_def()
                {
                    base.procedureName = "MID_NODE_SIZE_SELLTHRU_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_SIZE_SELLTHRU");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_SELLTHRU_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_SELLTHRU_READ_def MID_NODE_SIZE_SELLTHRU_READ = new MID_NODE_SIZE_SELLTHRU_READ_def();
            public class MID_NODE_SIZE_SELLTHRU_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_SIZE_SELLTHRU_READ_def()
                {
                    base.procedureName = "MID_NODE_SIZE_SELLTHRU_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_SELLTHRU");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_SIZE_SELLTHRU_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE_def MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE = new MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE_def();
            public class MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE_def : baseStoredProcedure
            {
                private intParameter START_TIME;
                private intParameter END_TIME;
                private intParameter SELECTED_NODE_RID;
                private intParameter STORE_RID;

                public MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE_def()
                {
                    base.procedureName = "MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("SIZE_CURVE");
                    START_TIME = new intParameter("@START_TIME", base.inputParameterList);
                    END_TIME = new intParameter("@END_TIME", base.inputParameterList);
                    SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
                    STORE_RID = new intParameter("@STORE_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, 
                                             int? START_TIME,
                                             int? END_TIME,
                                             int? SELECTED_NODE_RID,
                                             int? STORE_RID
                                             )
                {
                    lock (typeof(MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE_def))
                    {
                        this.START_TIME.SetValue(START_TIME);
                        this.END_TIME.SetValue(END_TIME);
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        this.STORE_RID.SetValue(STORE_RID);
                        //Begin TT#3456 -jsobek -Size Day To Week Failure
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        //End TT#3456 -jsobek -Size Day To Week Failure
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }

            public static SP_MID_ND_SZ_ST_INS_def SP_MID_ND_SZ_ST_INS = new SP_MID_ND_SZ_ST_INS_def();
            public class SP_MID_ND_SZ_ST_INS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private floatParameter SELLTHRU_LIMIT;

                public SP_MID_ND_SZ_ST_INS_def()
                {
                    base.procedureName = "SP_MID_ND_SZ_ST_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SELLTHRU_LIMIT = new floatParameter("@SELLTHRU_LIMIT", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  double? SELLTHRU_LIMIT
                                  )
                {
                    lock (typeof(SP_MID_ND_SZ_ST_INS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SELLTHRU_LIMIT.SetValue(SELLTHRU_LIMIT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES_def MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES = new MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES_def();
            public class MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES_def : baseStoredProcedure
            {

                public MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_CRITERIA_DETAIL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_ALL_NAMES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME_def MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME = new MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME_def();
            public class MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME_def : baseStoredProcedure
            {
                private intParameter NSCCD_RID;

                public MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME_def()
                {
                    base.procedureName = "MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_SIZE_CURVE_CRITERIA_DETAIL");
                    NSCCD_RID = new intParameter("@NSCCD_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? NSCCD_RID)
                {
                    lock (typeof(MID_NODE_SIZE_CURVE_CRITERIA_DETAIL_READ_NAME_def))
                    {
                        this.NSCCD_RID.SetValue(NSCCD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CAPACITY_READ_def MID_STORE_CAPACITY_READ = new MID_STORE_CAPACITY_READ_def();
            public class MID_STORE_CAPACITY_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_STORE_CAPACITY_READ_def()
                {
                    base.procedureName = "MID_STORE_CAPACITY_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CAPACITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_CAPACITY_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_STORE_CAPACITY_INSERT_def MID_STORE_CAPACITY_INSERT = new MID_STORE_CAPACITY_INSERT_def();
            public class MID_STORE_CAPACITY_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;
                private intParameter ST_CAPACITY;

                public MID_STORE_CAPACITY_INSERT_def()
                {
                    base.procedureName = "MID_STORE_CAPACITY_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("STORE_CAPACITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    ST_CAPACITY = new intParameter("@ST_CAPACITY", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID,
                                  int? ST_CAPACITY
                                  )
                {
                    lock (typeof(MID_STORE_CAPACITY_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.ST_CAPACITY.SetValue(ST_CAPACITY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_STORE_CAPACITY_DELETE_ALL_def MID_STORE_CAPACITY_DELETE_ALL = new MID_STORE_CAPACITY_DELETE_ALL_def();
            public class MID_STORE_CAPACITY_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_STORE_CAPACITY_DELETE_ALL_def()
                {
                    base.procedureName = "MID_STORE_CAPACITY_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_CAPACITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_STORE_CAPACITY_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_STORE_CAPACITY_DELETE_def MID_STORE_CAPACITY_DELETE = new MID_STORE_CAPACITY_DELETE_def();
            public class MID_STORE_CAPACITY_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter ST_RID;

                public MID_STORE_CAPACITY_DELETE_def()
                {
                    base.procedureName = "MID_STORE_CAPACITY_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("STORE_CAPACITY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? ST_RID
                                  )
                {
                    lock (typeof(MID_STORE_CAPACITY_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_XREF_def MID_HIERARCHY_READ_XREF = new MID_HIERARCHY_READ_XREF_def();
            public class MID_HIERARCHY_READ_XREF_def : baseStoredProcedure
            {
                private intParameter PH_RID;

                public MID_HIERARCHY_READ_XREF_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_XREF";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(MID_HIERARCHY_READ_XREF_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_READ_TOP_def MID_COLOR_NODE_READ_TOP = new MID_COLOR_NODE_READ_TOP_def();
            public class MID_COLOR_NODE_READ_TOP_def : baseStoredProcedure
            {
                private intParameter ROWS_TO_RETURN;

                public MID_COLOR_NODE_READ_TOP_def()
                {
                    base.procedureName = "MID_COLOR_NODE_READ_TOP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_NODE");
                    ROWS_TO_RETURN = new intParameter("@ROWS_TO_RETURN", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? ROWS_TO_RETURN)
                {
                    lock (typeof(MID_COLOR_NODE_READ_TOP_def))
                    {
                        this.ROWS_TO_RETURN.SetValue(ROWS_TO_RETURN);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_NODE_READ_TOP_def MID_SIZE_NODE_READ_TOP = new MID_SIZE_NODE_READ_TOP_def();
            public class MID_SIZE_NODE_READ_TOP_def : baseStoredProcedure
            {
                private intParameter ROWS_TO_RETURN;

                public MID_SIZE_NODE_READ_TOP_def()
                {
                    base.procedureName = "MID_SIZE_NODE_READ_TOP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_NODE");
                    ROWS_TO_RETURN = new intParameter("@ROWS_TO_RETURN", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? ROWS_TO_RETURN)
                {
                    lock (typeof(MID_SIZE_NODE_READ_TOP_def))
                    {
                        this.ROWS_TO_RETURN.SetValue(ROWS_TO_RETURN);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_READ_FROM_STYLE_def MID_COLOR_NODE_READ_FROM_STYLE = new MID_COLOR_NODE_READ_FROM_STYLE_def();
            public class MID_COLOR_NODE_READ_FROM_STYLE_def : baseStoredProcedure
            {
                private stringParameter STYLE_NODE_ID;

                public MID_COLOR_NODE_READ_FROM_STYLE_def()
                {
                    base.procedureName = "MID_COLOR_NODE_READ_FROM_STYLE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("COLOR_NODE");
                    STYLE_NODE_ID = new stringParameter("@STYLE_NODE_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string STYLE_NODE_ID)
                {
                    lock (typeof(MID_COLOR_NODE_READ_FROM_STYLE_def))
                    {
                        this.STYLE_NODE_ID.SetValue(STYLE_NODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR_def MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR = new MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR_def();
            public class MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR_def : baseStoredProcedure
            {
                private stringParameter STYLE_NODE_ID;
                private stringParameter COLOR_NODE_ID;

                public MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR_def()
                {
                    base.procedureName = "MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SIZE_NODE");
                    STYLE_NODE_ID = new stringParameter("@STYLE_NODE_ID", base.inputParameterList);
                    COLOR_NODE_ID = new stringParameter("@COLOR_NODE_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      string STYLE_NODE_ID,
                                      string COLOR_NODE_ID
                                      )
                {
                    lock (typeof(MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR_def))
                    {
                        this.STYLE_NODE_ID.SetValue(STYLE_NODE_ID);
                        this.COLOR_NODE_ID.SetValue(COLOR_NODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_COLOR_NODE_UPDATE_STYLE_ID_def MID_COLOR_NODE_UPDATE_STYLE_ID = new MID_COLOR_NODE_UPDATE_STYLE_ID_def();
            public class MID_COLOR_NODE_UPDATE_STYLE_ID_def : baseStoredProcedure
            {
                private stringParameter NEW_STYLE_NODE_ID;
                private stringParameter OLD_STYLE_NODE_ID;

                public MID_COLOR_NODE_UPDATE_STYLE_ID_def()
                {
                    base.procedureName = "MID_COLOR_NODE_UPDATE_STYLE_ID";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("COLOR_NODE");
                    NEW_STYLE_NODE_ID = new stringParameter("@NEW_STYLE_NODE_ID", base.inputParameterList);
                    OLD_STYLE_NODE_ID = new stringParameter("@OLD_STYLE_NODE_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  string NEW_STYLE_NODE_ID,
                                  string OLD_STYLE_NODE_ID
                                  )
                {
                    lock (typeof(MID_COLOR_NODE_UPDATE_STYLE_ID_def))
                    {
                        this.NEW_STYLE_NODE_ID.SetValue(NEW_STYLE_NODE_ID);
                        this.OLD_STYLE_NODE_ID.SetValue(OLD_STYLE_NODE_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_SIZE_NODE_UPDATE_STYLE_ID_def MID_SIZE_NODE_UPDATE_STYLE_ID = new MID_SIZE_NODE_UPDATE_STYLE_ID_def();
            public class MID_SIZE_NODE_UPDATE_STYLE_ID_def : baseStoredProcedure
            {
                private stringParameter NEW_STYLE_NODE_ID;
                private stringParameter OLD_STYLE_NODE_ID;

                public MID_SIZE_NODE_UPDATE_STYLE_ID_def()
                {
                    base.procedureName = "MID_SIZE_NODE_UPDATE_STYLE_ID";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("SIZE_NODE");
                    NEW_STYLE_NODE_ID = new stringParameter("@NEW_STYLE_NODE_ID", base.inputParameterList);
                    OLD_STYLE_NODE_ID = new stringParameter("@OLD_STYLE_NODE_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  string NEW_STYLE_NODE_ID,
                                  string OLD_STYLE_NODE_ID
                                  )
                {
                    lock (typeof(MID_SIZE_NODE_UPDATE_STYLE_ID_def))
                    {
                        this.NEW_STYLE_NODE_ID.SetValue(NEW_STYLE_NODE_ID);
                        this.OLD_STYLE_NODE_ID.SetValue(OLD_STYLE_NODE_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_GET_NODE_RIDS_def SP_MID_GET_NODE_RIDS = new SP_MID_GET_NODE_RIDS_def();
            public class SP_MID_GET_NODE_RIDS_def : baseStoredProcedure
            {
                private stringParameter xmlDoc;

                public SP_MID_GET_NODE_RIDS_def()
                {
                    base.procedureName = "SP_MID_GET_NODE_RIDS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    xmlDoc = new stringParameter("@xmlDoc", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string xmlDoc)
                {
                    lock (typeof(SP_MID_GET_NODE_RIDS_def))
                    {
                        this.xmlDoc.SetValue(xmlDoc);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX_def MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX = new MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX_def();
            public class MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX_def : baseStoredProcedure
            {

                public MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX_def()
                {
                    base.procedureName = "MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX";
                    base.procedureType = storedProcedureTypes.Maintenance;
                }

                public void Execute(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX_def))
                    {
                        base.ExecuteStoredProcedureForMaintenance(_dba);
                    }
                }
            }

            public static SP_MID_GET_COLOR_RIDS_def SP_MID_GET_COLOR_RIDS = new SP_MID_GET_COLOR_RIDS_def();
            public class SP_MID_GET_COLOR_RIDS_def : baseStoredProcedure
            {
                private stringParameter xmlDoc;

                public SP_MID_GET_COLOR_RIDS_def()
                {
                    base.procedureName = "SP_MID_GET_COLOR_RIDS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    xmlDoc = new stringParameter("@xmlDoc", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string xmlDoc)
                {
                    lock (typeof(SP_MID_GET_COLOR_RIDS_def))
                    {
                        this.xmlDoc.SetValue(xmlDoc);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_SIZE_RIDS_def SP_MID_GET_SIZE_RIDS = new SP_MID_GET_SIZE_RIDS_def();
            public class SP_MID_GET_SIZE_RIDS_def : baseStoredProcedure
            {
                private stringParameter xmlDoc;

                public SP_MID_GET_SIZE_RIDS_def()
                {
                    base.procedureName = "SP_MID_GET_SIZE_RIDS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    xmlDoc = new stringParameter("@xmlDoc", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string xmlDoc)
                {
                    lock (typeof(SP_MID_GET_SIZE_RIDS_def))
                    {
                        this.xmlDoc.SetValue(xmlDoc);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_ANCESTORS_WITH_APPLY_FROM_def SP_MID_GET_ANCESTORS_WITH_APPLY_FROM = new SP_MID_GET_ANCESTORS_WITH_APPLY_FROM_def();
            public class SP_MID_GET_ANCESTORS_WITH_APPLY_FROM_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter PH_RID;
                private intParameter HierarchySearchType;

                public SP_MID_GET_ANCESTORS_WITH_APPLY_FROM_def()
                {
                    base.procedureName = "SP_MID_GET_ANCESTORS_WITH_APPLY_FROM";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    HierarchySearchType = new intParameter("@HierarchySearchType", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? PH_RID,
                                      int? HierarchySearchType
                                      )
                {
                    lock (typeof(SP_MID_GET_ANCESTORS_WITH_APPLY_FROM_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_RID.SetValue(PH_RID);
                        this.HierarchySearchType.SetValue(HierarchySearchType);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_ANCESTORS_def SP_MID_GET_ANCESTORS = new SP_MID_GET_ANCESTORS_def();
            public class SP_MID_GET_ANCESTORS_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter PH_RID;
                private intParameter HierarchySearchType;

                public SP_MID_GET_ANCESTORS_def()
                {
                    base.procedureName = "SP_MID_GET_ANCESTORS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    HierarchySearchType = new intParameter("@HierarchySearchType", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? PH_RID,
                                      int? HierarchySearchType
                                      )
                {
                    lock (typeof(SP_MID_GET_ANCESTORS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.PH_RID.SetValue(PH_RID);
                        this.HierarchySearchType.SetValue(HierarchySearchType);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_ALL_GUEST_LEVELS_def SP_MID_GET_ALL_GUEST_LEVELS = new SP_MID_GET_ALL_GUEST_LEVELS_def();
            public class SP_MID_GET_ALL_GUEST_LEVELS_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public SP_MID_GET_ALL_GUEST_LEVELS_def()
                {
                    base.procedureName = "SP_MID_GET_ALL_GUEST_LEVELS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_GET_ALL_GUEST_LEVELS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			//BEGIN TT#4650 - DOConnell - Changes do not hold
            public static SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS_def SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS = new SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS_def();
            public class SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS_def()
                {
                    base.procedureName = "SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
			//END TT#4650 - DOConnell - Changes do not hold
			
            public static SP_MID_GET_IMO_READ_NODES_def SP_MID_GET_IMO_READ_NODES = new SP_MID_GET_IMO_READ_NODES_def();
            public class SP_MID_GET_IMO_READ_NODES_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_TYPE;

                public SP_MID_GET_IMO_READ_NODES_def()
                {
                    base.procedureName = "SP_MID_GET_IMO_READ_NODES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_TYPE = new intParameter("@LEVEL_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_TYPE
                                      )
                {
                    lock (typeof(SP_MID_GET_IMO_READ_NODES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_TYPE.SetValue(LEVEL_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_INTRANSIT_READ_NODES_def SP_MID_GET_INTRANSIT_READ_NODES = new SP_MID_GET_INTRANSIT_READ_NODES_def();
            public class SP_MID_GET_INTRANSIT_READ_NODES_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_TYPE;

                public SP_MID_GET_INTRANSIT_READ_NODES_def()
                {
                    base.procedureName = "SP_MID_GET_INTRANSIT_READ_NODES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_TYPE = new intParameter("@LEVEL_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_TYPE
                                      )
                {
                    lock (typeof(SP_MID_GET_INTRANSIT_READ_NODES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_TYPE.SetValue(LEVEL_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_DESCENDANTS_BY_TYPE_def SP_MID_GET_DESCENDANTS_BY_TYPE = new SP_MID_GET_DESCENDANTS_BY_TYPE_def();
            public class SP_MID_GET_DESCENDANTS_BY_TYPE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_TYPE;

                public SP_MID_GET_DESCENDANTS_BY_TYPE_def()
                {
                    base.procedureName = "SP_MID_GET_DESCENDANTS_BY_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_TYPE = new intParameter("@LEVEL_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_TYPE
                                      )
                {
                    lock (typeof(SP_MID_GET_DESCENDANTS_BY_TYPE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_TYPE.SetValue(LEVEL_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_DESCENDANTS_BY_OFFSET_def SP_MID_GET_DESCENDANTS_BY_OFFSET = new SP_MID_GET_DESCENDANTS_BY_OFFSET_def();
            public class SP_MID_GET_DESCENDANTS_BY_OFFSET_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_OFFSET;

                public SP_MID_GET_DESCENDANTS_BY_OFFSET_def()
                {
                    base.procedureName = "SP_MID_GET_DESCENDANTS_BY_OFFSET";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_OFFSET = new intParameter("@LEVEL_OFFSET", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_OFFSET
                                      )
                {
                    lock (typeof(SP_MID_GET_DESCENDANTS_BY_OFFSET_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_OFFSET.SetValue(LEVEL_OFFSET);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_DESCENDANTS_BY_LEVEL_def SP_MID_GET_DESCENDANTS_BY_LEVEL = new SP_MID_GET_DESCENDANTS_BY_LEVEL_def();
            public class SP_MID_GET_DESCENDANTS_BY_LEVEL_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_SEQ;

                public SP_MID_GET_DESCENDANTS_BY_LEVEL_def()
                {
                    base.procedureName = "SP_MID_GET_DESCENDANTS_BY_LEVEL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_SEQ = new intParameter("@LEVEL_SEQ", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_SEQ
                                      )
                {
                    lock (typeof(SP_MID_GET_DESCENDANTS_BY_LEVEL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_SEQ.SetValue(LEVEL_SEQ);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_DESCENDANTS_BY_RANGE_def SP_MID_GET_DESCENDANTS_BY_RANGE = new SP_MID_GET_DESCENDANTS_BY_RANGE_def();
            public class SP_MID_GET_DESCENDANTS_BY_RANGE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter LEVEL_FROM_OFFSET;
                private intParameter LEVEL_TO_OFFSET;

                public SP_MID_GET_DESCENDANTS_BY_RANGE_def()
                {
                    base.procedureName = "SP_MID_GET_DESCENDANTS_BY_RANGE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("<TABLENAME>");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    LEVEL_FROM_OFFSET = new intParameter("@LEVEL_FROM_OFFSET", base.inputParameterList);
                    LEVEL_TO_OFFSET = new intParameter("@LEVEL_TO_OFFSET", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? LEVEL_FROM_OFFSET,
                                      int? LEVEL_TO_OFFSET
                                      )
                {
                    lock (typeof(SP_MID_GET_DESCENDANTS_BY_RANGE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.LEVEL_FROM_OFFSET.SetValue(LEVEL_FROM_OFFSET);
                        this.LEVEL_TO_OFFSET.SetValue(LEVEL_TO_OFFSET);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_NODE_STOCK_MIN_MAX_DELETE_ALL_def MID_NODE_STOCK_MIN_MAX_DELETE_ALL = new MID_NODE_STOCK_MIN_MAX_DELETE_ALL_def();
            public class MID_NODE_STOCK_MIN_MAX_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_STOCK_MIN_MAX_DELETE_ALL_def()
                {
                    base.procedureName = "MID_NODE_STOCK_MIN_MAX_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("NODE_STOCK_MIN_MAX");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_STOCK_MIN_MAX_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_NODE_STOCK_MIN_MAX_INSERT_def MID_NODE_STOCK_MIN_MAX_INSERT = new MID_NODE_STOCK_MIN_MAX_INSERT_def();
            public class MID_NODE_STOCK_MIN_MAX_INSERT_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SGL_RID;
                private intParameter BOUNDARY;
                private intParameter CDR_RID;
                private intParameter MIN_STOCK;
                private intParameter MAX_STOCK;

                public MID_NODE_STOCK_MIN_MAX_INSERT_def()
                {
                    base.procedureName = "MID_NODE_STOCK_MIN_MAX_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("NODE_STOCK_MIN_MAX");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    MIN_STOCK = new intParameter("@MIN_STOCK", base.inputParameterList);
                    MAX_STOCK = new intParameter("@MAX_STOCK", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SGL_RID,
                                  int? BOUNDARY,
                                  int? CDR_RID,
                                  int? MIN_STOCK,
                                  int? MAX_STOCK
                                  )
                {
                    lock (typeof(MID_NODE_STOCK_MIN_MAX_INSERT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.MIN_STOCK.SetValue(MIN_STOCK);
                        this.MAX_STOCK.SetValue(MAX_STOCK);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_BUILD_COLOR_SIZE_IDS_def SP_MID_BUILD_COLOR_SIZE_IDS = new SP_MID_BUILD_COLOR_SIZE_IDS_def();
            public class SP_MID_BUILD_COLOR_SIZE_IDS_def : baseStoredProcedure
            {

                public SP_MID_BUILD_COLOR_SIZE_IDS_def()
                {
                    base.procedureName = "SP_MID_BUILD_COLOR_SIZE_IDS";
                    base.procedureType = storedProcedureTypes.Update;
                }

                public int Update(DatabaseAccess _dba)
                {
                    lock (typeof(SP_MID_BUILD_COLOR_SIZE_IDS_def))
                    {
                        base.SetCommandTimeout(10000);
                        return base.ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX_def MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX = new MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX_def();
            public class MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX_def : baseStoredProcedure
            {

                public MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX_def()
                {
                    base.procedureName = "MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX";
                    base.procedureType = storedProcedureTypes.Maintenance;
                }

                public void Execute(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX_def))
                    {
                        base.SetCommandTimeout(1000);
                        base.ExecuteStoredProcedureForMaintenance(_dba);
                    }
                }
            }

            public static SP_MID_GET_HIGHEST_GUEST_LEVEL_def SP_MID_GET_HIGHEST_GUEST_LEVEL = new SP_MID_GET_HIGHEST_GUEST_LEVEL_def();
            public class SP_MID_GET_HIGHEST_GUEST_LEVEL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                private intParameter GUEST_LEVEL; //Declare Output Parameter
                private intParameter HOME_PATH_LENGTH; //Declare Output Parameter

                public SP_MID_GET_HIGHEST_GUEST_LEVEL_def()
                {
                    base.procedureName = "SP_MID_GET_HIGHEST_GUEST_LEVEL";
                    base.procedureType = storedProcedureTypes.OutputOnly;
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    GUEST_LEVEL = new intParameter("@GUEST_LEVEL", base.outputParameterList); //Add Output Parameter
                    HOME_PATH_LENGTH = new intParameter("@HOME_PATH_LENGTH", base.outputParameterList); //Add Output Parameter
                }

                public void GetOutput(DatabaseAccess _dba, ref int guestLevel, ref int homePathLength, int? HN_RID)
                {
                    lock (typeof(SP_MID_GET_HIGHEST_GUEST_LEVEL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.GUEST_LEVEL.SetValue(null); //Initialize Output Parameter
                        this.HOME_PATH_LENGTH.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForRead(_dba);
                        guestLevel = Convert.ToInt32(this.GUEST_LEVEL.Value);
                        homePathLength = Convert.ToInt32(this.HOME_PATH_LENGTH.Value);
                    }
                }
            }

            public static SP_MID_GET_BRANCH_SIZE_def SP_MID_GET_BRANCH_SIZE = new SP_MID_GET_BRANCH_SIZE_def();
            public class SP_MID_GET_BRANCH_SIZE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private charParameter HOME_HIERARCHY_ONLY;

                private intParameter BRANCH_SIZE; //Declare Output Parameter

                public SP_MID_GET_BRANCH_SIZE_def()
                {
                    base.procedureName = "SP_MID_GET_BRANCH_SIZE";
                    base.procedureType = storedProcedureTypes.OutputOnly;
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    HOME_HIERARCHY_ONLY = new charParameter("@HOME_HIERARCHY_ONLY", base.inputParameterList);
                    BRANCH_SIZE = new intParameter("@BRANCH_SIZE", base.outputParameterList); //Add Output Parameter
                }

                public int GetOutput(DatabaseAccess _dba, int? HN_RID, char? HOME_HIERARCHY_ONLY)
                {
                    lock (typeof(SP_MID_GET_BRANCH_SIZE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.HOME_HIERARCHY_ONLY.SetValue(HOME_HIERARCHY_ONLY);
                        this.BRANCH_SIZE.SetValue(null); //Initialize Output Parameter

                        ExecuteStoredProcedureForRead(_dba);
                        return (int)this.BRANCH_SIZE.Value;
                    }
                }
            }

            public static MID_NODE_STOCK_MIN_MAX_READ_def MID_NODE_STOCK_MIN_MAX_READ = new MID_NODE_STOCK_MIN_MAX_READ_def();
            public class MID_NODE_STOCK_MIN_MAX_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_NODE_STOCK_MIN_MAX_READ_def()
                {
                    base.procedureName = "MID_NODE_STOCK_MIN_MAX_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("NODE_STOCK_MIN_MAX");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_NODE_STOCK_MIN_MAX_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP_def MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP = new MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP_def();
            public class MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SG_RID;

                public MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP_def()
                {
                    base.procedureName = "MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HIERARCHY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SG_RID
                                  )
                {
                    lock (typeof(MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SG_RID.SetValue(SG_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static SP_MID_SET_NODE_ACTIVITY_def SP_MID_SET_NODE_ACTIVITY = new SP_MID_SET_NODE_ACTIVITY_def();
            public class SP_MID_SET_NODE_ACTIVITY_def : baseStoredProcedure
            {
                private intParameter fromOffset;
                private intParameter toOffset;
                private intParameter intransitOffset;
                private intParameter forecastOffset;
                private intParameter table;
                private intParameter Return_Code; //Declare Output Parameter

                public SP_MID_SET_NODE_ACTIVITY_def()
                {
                    base.procedureName = "SP_MID_SET_NODE_ACTIVITY";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("<TABLENAME>");
                    fromOffset = new intParameter("@fromOffset", base.inputParameterList);
                    toOffset = new intParameter("@toOffset", base.inputParameterList);
                    intransitOffset = new intParameter("@intransitOffset", base.inputParameterList);
                    forecastOffset = new intParameter("@forecastOffset", base.inputParameterList);
                    table = new intParameter("@table", base.inputParameterList);
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba, 
                                                int? fromOffset,
                                                int? toOffset,
                                                int? intransitOffset,
                                                int? forecastOffset,
                                                int? table
                                                )
                {
                    lock (typeof(SP_MID_SET_NODE_ACTIVITY_def))
                    {
                        this.fromOffset.SetValue(fromOffset);
                        this.toOffset.SetValue(toOffset);
                        this.intransitOffset.SetValue(intransitOffset);
                        this.forecastOffset.SetValue(forecastOffset);
                        this.table.SetValue(table);
                        this.Return_Code.SetValue(0); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)Return_Code.Value;
                    }
                }
            }

            public static MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR_def MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR = new MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR_def();
            public class MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR_def : baseStoredProcedure
            {

                public MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR_def()
                {
                    base.procedureName = "MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HIERARCHY");
                }

                public int Update(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR_def))
                    {
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_COUNT_def MID_HIERARCHY_READ_COUNT = new MID_HIERARCHY_READ_COUNT_def();
            public class MID_HIERARCHY_READ_COUNT_def : baseStoredProcedure
            {

                public MID_HIERARCHY_READ_COUNT_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HIERARCHY");
                }

                public int ReadRecordCount(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_READ_COUNT_def))
                    {
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_ACTIVE_COUNT_def MID_HIERARCHY_READ_ACTIVE_COUNT = new MID_HIERARCHY_READ_ACTIVE_COUNT_def();
            public class MID_HIERARCHY_READ_ACTIVE_COUNT_def : baseStoredProcedure
            {

                public MID_HIERARCHY_READ_ACTIVE_COUNT_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_ACTIVE_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HIERARCHY");
                }

                public int ReadRecordCount(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_READ_ACTIVE_COUNT_def))
                    {
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_READ_INACTIVE_COUNT_def MID_HIERARCHY_READ_INACTIVE_COUNT = new MID_HIERARCHY_READ_INACTIVE_COUNT_def();
            public class MID_HIERARCHY_READ_INACTIVE_COUNT_def : baseStoredProcedure
            {

                public MID_HIERARCHY_READ_INACTIVE_COUNT_def()
                {
                    base.procedureName = "MID_HIERARCHY_READ_INACTIVE_COUNT";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("HIERARCHY");
                }

                public int ReadRecordCount(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_READ_INACTIVE_COUNT_def))
                    {
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static SP_MID_HIERARCHY_IN_USE_BY_HEADER_def SP_MID_HIERARCHY_IN_USE_BY_HEADER = new SP_MID_HIERARCHY_IN_USE_BY_HEADER_def();
            public class SP_MID_HIERARCHY_IN_USE_BY_HEADER_def : baseStoredProcedure
            {
                private intParameter PH_RID;
                private intParameter ReturnCode; //Declare Output Parameter

                public SP_MID_HIERARCHY_IN_USE_BY_HEADER_def()
                {
                    base.procedureName = "SP_MID_HIERARCHY_IN_USE_BY_HEADER";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("<TABLENAME>");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba, int? PH_RID)
                {
                    lock (typeof(SP_MID_HIERARCHY_IN_USE_BY_HEADER_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.ReturnCode.SetValue(0); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)ReturnCode.Value;
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_SET_READ_def MID_CHAIN_SET_PERCENT_SET_READ = new MID_CHAIN_SET_PERCENT_SET_READ_def();
            public class MID_CHAIN_SET_PERCENT_SET_READ_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter BEGIN_WEEK;
                private intParameter END_WEEK;

                public MID_CHAIN_SET_PERCENT_SET_READ_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_SET_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    BEGIN_WEEK = new intParameter("@BEGIN_WEEK", base.inputParameterList);
                    END_WEEK = new intParameter("@END_WEEK", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? HN_RID,
                                      int? BEGIN_WEEK,
                                      int? END_WEEK
                                      )
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_SET_READ_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BEGIN_WEEK.SetValue(BEGIN_WEEK);
                        this.END_WEEK.SetValue(END_WEEK);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_SET_DELETE_ALL_def MID_CHAIN_SET_PERCENT_SET_DELETE_ALL = new MID_CHAIN_SET_PERCENT_SET_DELETE_ALL_def();
            public class MID_CHAIN_SET_PERCENT_SET_DELETE_ALL_def : baseStoredProcedure
            {
                private intParameter HN_RID;

                public MID_CHAIN_SET_PERCENT_SET_DELETE_ALL_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_SET_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HN_RID)
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_SET_DELETE_ALL_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_SET_DELETE_def MID_CHAIN_SET_PERCENT_SET_DELETE = new MID_CHAIN_SET_PERCENT_SET_DELETE_def();
            public class MID_CHAIN_SET_PERCENT_SET_DELETE_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter FISCAL_WEEK;

                public MID_CHAIN_SET_PERCENT_SET_DELETE_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_SET_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    FISCAL_WEEK = new intParameter("@FISCAL_WEEK", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? FISCAL_WEEK
                                  )
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_SET_DELETE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.FISCAL_WEEK.SetValue(FISCAL_WEEK);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP_def MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP = new MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP_def();
            public class MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP_def : baseStoredProcedure
            {
                private intParameter HN_RID;
                private intParameter SGL_RID;
                private intParameter FISCAL_WEEK;

                public MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
                    FISCAL_WEEK = new intParameter("@FISCAL_WEEK", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? HN_RID,
                                  int? SGL_RID,
                                  int? FISCAL_WEEK
                                  )
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_SET_DELETE_FOR_STORE_GROUP_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.FISCAL_WEEK.SetValue(FISCAL_WEEK);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_USER_UPSERT_def MID_CHAIN_SET_PERCENT_USER_UPSERT = new MID_CHAIN_SET_PERCENT_USER_UPSERT_def();
            public class MID_CHAIN_SET_PERCENT_USER_UPSERT_def : baseStoredProcedure
            {
                private intParameter USER_RID;
                private intParameter SG_RID;
                private intParameter CDR_RID;

                public MID_CHAIN_SET_PERCENT_USER_UPSERT_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_USER_UPSERT";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("<TABLENAME>");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? USER_RID,
                                  int? SG_RID,
                                  int? CDR_RID
                                  )
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_USER_UPSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.SG_RID.SetValue(SG_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_CHAIN_SET_PERCENT_USER_READ_def MID_CHAIN_SET_PERCENT_USER_READ = new MID_CHAIN_SET_PERCENT_USER_READ_def();
            public class MID_CHAIN_SET_PERCENT_USER_READ_def : baseStoredProcedure
            {
                private intParameter USER_RID;

                public MID_CHAIN_SET_PERCENT_USER_READ_def()
                {
                    base.procedureName = "MID_CHAIN_SET_PERCENT_USER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CHAIN_SET_PERCENT_USER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? USER_RID)
                {
                    lock (typeof(MID_CHAIN_SET_PERCENT_USER_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_GET_ALL_DESCENDANTS_def SP_MID_GET_ALL_DESCENDANTS = new SP_MID_GET_ALL_DESCENDANTS_def();
			public class SP_MID_GET_ALL_DESCENDANTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_ALL_DESCENDANTS.SQL"

			    private intParameter HN_RID;

                public SP_MID_GET_ALL_DESCENDANTS_def()
			    {
                    base.procedureName = "SP_MID_GET_ALL_DESCENDANTS";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY_NODE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(SP_MID_GET_ALL_DESCENDANTS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_GET_ALL_AFFECTED_DESCENDANTS_def SP_MID_GET_ALL_AFFECTED_DESCENDANTS = new SP_MID_GET_ALL_AFFECTED_DESCENDANTS_def();
			public class SP_MID_GET_ALL_AFFECTED_DESCENDANTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_ALL_AFFECTED_DESCENDANTS.SQL"

			    private intParameter HN_RID;
			    private intParameter IMO;
			    private intParameter SE;
			    private intParameter CHAR;
			    private intParameter SC;
			    private intParameter DP;
			    private intParameter PC;
			    private intParameter CSP;

                public SP_MID_GET_ALL_AFFECTED_DESCENDANTS_def()
			    {
                    base.procedureName = "SP_MID_GET_ALL_AFFECTED_DESCENDANTS";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY_NODE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        IMO = new intParameter("@IMO", base.inputParameterList);
			        SE = new intParameter("@SE", base.inputParameterList);
			        CHAR = new intParameter("@CHAR", base.inputParameterList);
			        SC = new intParameter("@SC", base.inputParameterList);
			        DP = new intParameter("@DP", base.inputParameterList);
			        PC = new intParameter("@PC", base.inputParameterList);
			        CSP = new intParameter("@CSP", base.inputParameterList);
			    }
			
                /// <summary>
                /// 
                /// </summary>
                /// <param name="_dba"></param>
                /// <param name="HN_RID"></param>
                /// <param name="IMO">IMO</param>
                /// <param name="SE">Store Eligibility</param>
                /// <param name="CHAR">Characteristics</param>
                /// <param name="SC">Store Capacity</param>
                /// <param name="DP">Daily Percentages</param>
                /// <param name="PC">Purge Criteria</param>
                /// <param name="CSP">Chain Set Percentage</param>
                /// <returns></returns>
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HN_RID,
			                          int? IMO, 
                                      int? SE,
			                          int? CHAR,
			                          int? SC,
			                          int? DP,
			                          int? PC,
			                          int? CSP
			                          )
			    {
                    lock (typeof(SP_MID_GET_ALL_AFFECTED_DESCENDANTS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.IMO.SetValue(IMO);
                        this.SE.SetValue(SE);
                        this.CHAR.SetValue(CHAR);
                        this.SC.SetValue(SC);
                        this.DP.SetValue(DP);
                        this.PC.SetValue(PC);
                        this.CSP.SetValue(CSP);
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

            public static SP_MID_UPDATE_COLUMN_BY_TABLE_def SP_MID_UPDATE_COLUMN_BY_TABLE = new SP_MID_UPDATE_COLUMN_BY_TABLE_def();
			public class SP_MID_UPDATE_COLUMN_BY_TABLE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATE_COLUMN_BY_TABLE.SQL"

			    private intParameter HN_RID;
			    private intParameter ST_RID;
			    private stringParameter COLUMNNAME;
			    private stringParameter TABLENAME;
			    private intParameter COLVALUE;
                private intParameter ReturnCode; //Declare Output Parameter

                public SP_MID_UPDATE_COLUMN_BY_TABLE_def()
			    {
			        base.procedureName = "SP_MID_UPDATE_COLUMN_BY_TABLE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("X");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        COLUMNNAME = new stringParameter("@COLUMNNAME", base.inputParameterList);
			        TABLENAME = new stringParameter("@TABLENAME", base.inputParameterList);
			        COLVALUE = new intParameter("@COLVALUE", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int UpdateWithReturnCode(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? ST_RID,
			                      string COLUMNNAME,
			                      string TABLENAME,
			                      int? COLVALUE
			                      )
			    {
                    lock (typeof(SP_MID_UPDATE_COLUMN_BY_TABLE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.COLUMNNAME.SetValue(COLUMNNAME);
                        this.TABLENAME.SetValue(TABLENAME);
                        this.COLVALUE.SetValue(COLVALUE);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)this.ReturnCode.Value;
                    }
			    }
			}

            public static SP_MID_DELETE_HIER_BY_COL_def SP_MID_DELETE_HIER_BY_COL = new SP_MID_DELETE_HIER_BY_COL_def();
			public class SP_MID_DELETE_HIER_BY_COL_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DELETE_HIER_BY_COL.SQL"

			    private intParameter RID;
			    private stringParameter COLUMNAME;
			    private stringParameter TABLENAME;
                private intParameter ReturnCode; //Declare Output Parameter

                public SP_MID_DELETE_HIER_BY_COL_def()
			    {
                    base.procedureName = "SP_MID_DELETE_HIER_BY_COL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("X");
			        RID = new intParameter("@RID", base.inputParameterList);
			        COLUMNAME = new stringParameter("@COLUMNAME", base.inputParameterList);
			        TABLENAME = new stringParameter("@TABLENAME", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int DeleteWithReturnCode(DatabaseAccess _dba, 
			                      int? RID,
			                      string COLUMNAME,
			                      string TABLENAME
			                      )
			    {
                    lock (typeof(SP_MID_DELETE_HIER_BY_COL_def))
                    {
                        this.RID.SetValue(RID);
                        this.COLUMNAME.SetValue(COLUMNAME);
                        this.TABLENAME.SetValue(TABLENAME);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.ReturnCode.Value;
                    }
			    }
			}

            public static SP_MID_DELETE_HIER_FROM_TABLE_def SP_MID_DELETE_HIER_FROM_TABLE = new SP_MID_DELETE_HIER_FROM_TABLE_def();
            public class SP_MID_DELETE_HIER_FROM_TABLE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DELETE_HIER_FROM_TABLE.SQL"

			    private intParameter HN_RID;
			    private intParameter ST_RID;
			    private stringParameter TABLENAME;
                private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_DELETE_HIER_FROM_TABLE_def()
			    {
                    base.procedureName = "SP_MID_DELETE_HIER_FROM_TABLE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("X");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        TABLENAME = new stringParameter("@TABLENAME", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int DeleteWithReturnCode(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? ST_RID,
			                      string TABLENAME
			                      )
			    {
                    lock (typeof(SP_MID_DELETE_HIER_FROM_TABLE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.TABLENAME.SetValue(TABLENAME);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.ReturnCode.Value;
                    }
			    }
			}

            public static SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def SP_MID_CHAIN_SET_PCT_SET_WK_DELETE = new SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def();
			public class SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CHAIN_SET_PCT_SET_WK_DELETE.SQL"

			    private intParameter NODE_RID;
			    private intParameter YEAR_WEEK;
			    private intParameter RETURN; //Declare Output Parameter

                public SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def()
			    {
                    base.procedureName = "SP_MID_CHAIN_SET_PCT_SET_WK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("CHAIN_SET_PERCENT_SET");
			        NODE_RID = new intParameter("@NODE_RID", base.inputParameterList);
			        YEAR_WEEK = new intParameter("@YEAR_WEEK", base.inputParameterList);
			        RETURN = new intParameter("@RETURN", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int DeleteWithReturnCode(DatabaseAccess _dba, 
			                      int? NODE_RID,
			                      int? YEAR_WEEK
			                      )
			    {
                    lock (typeof(SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def))
                    {
                        this.NODE_RID.SetValue(NODE_RID);
                        this.YEAR_WEEK.SetValue(YEAR_WEEK);
                        this.RETURN.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)this.RETURN.Value;
                    }
			    }
			}

            public static SP_MID_GET_STYLE_COLOR_SIZES_def SP_MID_GET_STYLE_COLOR_SIZES = new SP_MID_GET_STYLE_COLOR_SIZES_def();
            public class SP_MID_GET_STYLE_COLOR_SIZES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_STYLE_COLOR_SIZES.SQL"

			    private intParameter HN_RID;
			
			    public SP_MID_GET_STYLE_COLOR_SIZES_def()
			    {
                    base.procedureName = "SP_MID_GET_STYLE_COLOR_SIZES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_NODE");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(SP_MID_GET_STYLE_COLOR_SIZES_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE_def MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE = new MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE_def();
            public class MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE.SQL"

			    private intParameter SELECTED_NODE_RID;
			
			    public MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE_def()
			    {
                    base.procedureName = "MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE");
                    SELECTED_NODE_RID = new intParameter("@SELECTED_NODE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SELECTED_NODE_RID)
			    {
                    lock (typeof(MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE_def))
                    {
                        this.SELECTED_NODE_RID.SetValue(SELECTED_NODE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_GRADES_UPDATE_BOUNDARY_def MID_STORE_GRADES_UPDATE_BOUNDARY = new MID_STORE_GRADES_UPDATE_BOUNDARY_def();
			public class MID_STORE_GRADES_UPDATE_BOUNDARY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_GRADES_UPDATE_BOUNDARY.SQL"

			    private intParameter HN_RID;
			    private intParameter BOUNDARY;
			    private intParameter ORIGINAL_BOUNDARY;
			
			    public MID_STORE_GRADES_UPDATE_BOUNDARY_def()
			    {
			        base.procedureName = "MID_STORE_GRADES_UPDATE_BOUNDARY";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("STORE_GRADES");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        BOUNDARY = new intParameter("@BOUNDARY", base.inputParameterList);
			        ORIGINAL_BOUNDARY = new intParameter("@ORIGINAL_BOUNDARY", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? BOUNDARY,
			                      int? ORIGINAL_BOUNDARY
			                      )
			    {
                    lock (typeof(MID_STORE_GRADES_UPDATE_BOUNDARY_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.BOUNDARY.SetValue(BOUNDARY);
                        this.ORIGINAL_BOUNDARY.SetValue(ORIGINAL_BOUNDARY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_HIER_CHAR_JOIN_READ_ALL_def MID_HIER_CHAR_JOIN_READ_ALL = new MID_HIER_CHAR_JOIN_READ_ALL_def();
			public class MID_HIER_CHAR_JOIN_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIER_CHAR_JOIN_READ_ALL.SQL"

			
			    public MID_HIER_CHAR_JOIN_READ_ALL_def()
			    {
			        base.procedureName = "MID_HIER_CHAR_JOIN_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HIER_CHAR_JOIN");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_HIER_CHAR_JOIN_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ELIGIBILITY_MODEL_READ_FILTERED_def MID_ELIGIBILITY_MODEL_READ_FILTERED = new MID_ELIGIBILITY_MODEL_READ_FILTERED_def();
			public class MID_ELIGIBILITY_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ELIGIBILITY_MODEL_READ_FILTERED.SQL"

			    private stringParameter EM_ID_FILTER;
			
			    public MID_ELIGIBILITY_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_ELIGIBILITY_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ELIGIBILITY_MODEL");
			        EM_ID_FILTER = new stringParameter("@EM_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string EM_ID_FILTER)
			    {
                    lock (typeof(MID_ELIGIBILITY_MODEL_READ_FILTERED_def))
                    {
                        this.EM_ID_FILTER.SetValue(EM_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FWOS_MAX_MODEL_READ_FILTERED_def MID_FWOS_MAX_MODEL_READ_FILTERED = new MID_FWOS_MAX_MODEL_READ_FILTERED_def();
			public class MID_FWOS_MAX_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FWOS_MAX_MODEL_READ_FILTERED.SQL"

			    private stringParameter FWOSMAX_ID_FILTER;
			
			    public MID_FWOS_MAX_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_FWOS_MAX_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FWOS_MAX_MODEL");
			        FWOSMAX_ID_FILTER = new stringParameter("@FWOSMAX_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FWOSMAX_ID_FILTER)
			    {
                    lock (typeof(MID_FWOS_MAX_MODEL_READ_FILTERED_def))
                    {
                        this.FWOSMAX_ID_FILTER.SetValue(FWOSMAX_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FWOS_MODIFIER_MODEL_READ_FILTERED_def MID_FWOS_MODIFIER_MODEL_READ_FILTERED = new MID_FWOS_MODIFIER_MODEL_READ_FILTERED_def();
			public class MID_FWOS_MODIFIER_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FWOS_MODIFIER_MODEL_READ_FILTERED.SQL"

			    private stringParameter FWOSMOD_ID_FILTER;
			
			    public MID_FWOS_MODIFIER_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_FWOS_MODIFIER_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FWOS_MODIFIER_MODEL");
			        FWOSMOD_ID_FILTER = new stringParameter("@FWOSMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FWOSMOD_ID_FILTER)
			    {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_READ_FILTERED_def))
                    {
                        this.FWOSMOD_ID_FILTER.SetValue(FWOSMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_READ_FILTERED_def MID_FORECAST_BAL_MODEL_READ_FILTERED = new MID_FORECAST_BAL_MODEL_READ_FILTERED_def();
			public class MID_FORECAST_BAL_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_READ_FILTERED.SQL"

			    private stringParameter FBMOD_ID_FILTER;
			
			    public MID_FORECAST_BAL_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_ID_FILTER = new stringParameter("@FBMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FBMOD_ID_FILTER)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_READ_FILTERED_def))
                    {
                        this.FBMOD_ID_FILTER.SetValue(FBMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_def MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED = new MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED.SQL"

			    private stringParameter NAME_FILTER;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        NAME_FILTER = new stringParameter("@NAME_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string NAME_FILTER)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_def))
                    {
                        this.NAME_FILTER.SetValue(NAME_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SALES_MODIFIER_MODEL_READ_FILTERED_def MID_SALES_MODIFIER_MODEL_READ_FILTERED = new MID_SALES_MODIFIER_MODEL_READ_FILTERED_def();
			public class MID_SALES_MODIFIER_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SALES_MODIFIER_MODEL_READ_FILTERED.SQL"

			    private stringParameter SLSMOD_ID_FILTER;
			
			    public MID_SALES_MODIFIER_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_SALES_MODIFIER_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SALES_MODIFIER_MODEL");
			        SLSMOD_ID_FILTER = new stringParameter("@SLSMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SLSMOD_ID_FILTER)
			    {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_READ_FILTERED_def))
                    {
                        this.SLSMOD_ID_FILTER.SetValue(SLSMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STOCK_MODIFIER_MODEL_READ_FILTERED_def MID_STOCK_MODIFIER_MODEL_READ_FILTERED = new MID_STOCK_MODIFIER_MODEL_READ_FILTERED_def();
			public class MID_STOCK_MODIFIER_MODEL_READ_FILTERED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MODIFIER_MODEL_READ_FILTERED.SQL"

			    private stringParameter STKMOD_ID_FILTER;
			
			    public MID_STOCK_MODIFIER_MODEL_READ_FILTERED_def()
			    {
			        base.procedureName = "MID_STOCK_MODIFIER_MODEL_READ_FILTERED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STOCK_MODIFIER_MODEL");
			        STKMOD_ID_FILTER = new stringParameter("@STKMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string STKMOD_ID_FILTER)
			    {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_READ_FILTERED_def))
                    {
                        this.STKMOD_ID_FILTER.SetValue(STKMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter EM_ID_FILTER;
			
			    public MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ELIGIBILITY_MODEL");
			        EM_ID_FILTER = new stringParameter("@EM_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string EM_ID_FILTER)
			    {
                    lock (typeof(MID_ELIGIBILITY_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.EM_ID_FILTER.SetValue(EM_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter FBMOD_ID_FILTER;
			
			    public MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_BAL_MODEL");
			        FBMOD_ID_FILTER = new stringParameter("@FBMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FBMOD_ID_FILTER)
			    {
                    lock (typeof(MID_FORECAST_BAL_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.FBMOD_ID_FILTER.SetValue(FBMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE_def MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE = new MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter NAME_FILTER;
			
			    public MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("OVERRIDE_LL_MODEL_HEADER");
			        NAME_FILTER = new stringParameter("@NAME_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string NAME_FILTER)
			    {
                    lock (typeof(MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.NAME_FILTER.SetValue(NAME_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter FWOSMOD_ID_FILTER;
			
			    public MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FWOS_MODIFIER_MODEL");
			        FWOSMOD_ID_FILTER = new stringParameter("@FWOSMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FWOSMOD_ID_FILTER)
			    {
                    lock (typeof(MID_FWOS_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.FWOSMOD_ID_FILTER.SetValue(FWOSMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter SLSMOD_ID_FILTER;
			
			    public MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SALES_MODIFIER_MODEL");
			        SLSMOD_ID_FILTER = new stringParameter("@SLSMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SLSMOD_ID_FILTER)
			    {
                    lock (typeof(MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.SLSMOD_ID_FILTER.SetValue(SLSMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter STKMOD_ID_FILTER;
			
			    public MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STOCK_MODIFIER_MODEL");
			        STKMOD_ID_FILTER = new stringParameter("@STKMOD_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string STKMOD_ID_FILTER)
			    {
                    lock (typeof(MID_STOCK_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.STKMOD_ID_FILTER.SetValue(STKMOD_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE_def MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE = new MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE_def();
			public class MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE.SQL"

			    private stringParameter FWOSMAX_ID_FILTER;
			
			    public MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE_def()
			    {
			        base.procedureName = "MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FWOS_MAX_MODEL");
			        FWOSMAX_ID_FILTER = new stringParameter("@FWOSMAX_ID_FILTER", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string FWOSMAX_ID_FILTER)
			    {
                    lock (typeof(MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE_def))
                    {
                        this.FWOSMAX_ID_FILTER.SetValue(FWOSMAX_ID_FILTER);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_GET_DELETED_HIERARCHY_NODES_def MID_GET_DELETED_HIERARCHY_NODES = new MID_GET_DELETED_HIERARCHY_NODES_def();
            public class MID_GET_DELETED_HIERARCHY_NODES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_DELETED_HIERARCHY_NODES.SQL"

                public MID_GET_DELETED_HIERARCHY_NODES_def()
                {
                    base.procedureName = "MID_GET_DELETED_HIERARCHY_NODES";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_GET_DELETED_HIERARCHY_NODES_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }

            }

            public static MID_MARK_NODE_DELETED_def MID_MARK_NODE_DELETED = new MID_MARK_NODE_DELETED_def();
            public class MID_MARK_NODE_DELETED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MARK_NODE_DELETED.SQL"

                private intParameter HN_RID;
                private charParameter NODE_DELETE_IND;
                private stringParameter NODE_ID;

                public MID_MARK_NODE_DELETED_def()
                {
                    base.procedureName = "MID_MARK_NODE_DELETED";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    NODE_DELETE_IND = new charParameter("@NODE_DELETE_IND", base.inputParameterList);
                    NODE_ID = new stringParameter("@NODE_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? HN_RID,
                                  char? NODE_DELETE_IND,
                                  string NODE_ID
                                  )
                {
                    lock (typeof(MID_MARK_NODE_DELETED_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.NODE_DELETE_IND.SetValue(NODE_DELETE_IND);
                        this.NODE_ID.SetValue(NODE_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL_def MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL = new MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL_def();
            public class MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL.SQL"

                private intParameter HOME_PH_RID;

                public MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL_def()
                {
                    base.procedureName = "MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HIERARCHY_NODE");
                    HOME_PH_RID = new intParameter("@HOME_PH_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? HOME_PH_RID)
                {
                    lock (typeof(MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL_def))
                    {
                        this.HOME_PH_RID.SetValue(HOME_PH_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
            public static MID_HIERARCHY_GET_ROOT_NODE_RID_def MID_HIERARCHY_GET_ROOT_NODE_RID = new MID_HIERARCHY_GET_ROOT_NODE_RID_def();
            public class MID_HIERARCHY_GET_ROOT_NODE_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_GET_ROOT_NODE_RID.SQL"

                private intParameter PH_RID;

                public MID_HIERARCHY_GET_ROOT_NODE_RID_def()
                {
                    base.procedureName = "MID_HIERARCHY_GET_ROOT_NODE_RID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HIERARCHY_NODE");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int PH_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_ROOT_NODE_RID_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);


                        if (dt.Rows.Count > 0)
                        {
                            return dt.Rows[0]["HN_RID"];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                public DataTable Read(DatabaseAccess _dba, int PH_RID)
                {
                    lock (typeof(MID_HIERARCHY_GET_ROOT_NODE_RID_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HIERARCHY_ADD_ORG_LEVEL_def MID_HIERARCHY_ADD_ORG_LEVEL = new MID_HIERARCHY_ADD_ORG_LEVEL_def();
            public class MID_HIERARCHY_ADD_ORG_LEVEL_def : baseStoredProcedure
            {
                private stringParameter levelID;
                private stringParameter afterLevel;
                private intParameter Return_Code; //Declare Output Parameter

                public MID_HIERARCHY_ADD_ORG_LEVEL_def()
                {
                    base.procedureName = "MID_HIERARCHY_ADD_ORG_LEVEL";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HIERARCHY_LEVEL");
                    levelID = new stringParameter("@Level_ID", base.inputParameterList);
                    afterLevel = new stringParameter("@After_Level", base.inputParameterList);
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                string levelID,
                                                string afterLevel
                                                )
                {
                    lock (typeof(MID_HIERARCHY_ADD_ORG_LEVEL_def))
                    {
                        this.levelID.SetValue(levelID);
                        this.afterLevel.SetValue(afterLevel);
                        this.Return_Code.SetValue(0); //Initialize Output Parameter
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)Return_Code.Value;
                    }
                }
            }

            public static MID_HIERARCHY_REMOVE_ORG_LEVEL_def MID_HIERARCHY_REMOVE_ORG_LEVEL = new MID_HIERARCHY_REMOVE_ORG_LEVEL_def();
            public class MID_HIERARCHY_REMOVE_ORG_LEVEL_def : baseStoredProcedure
            {
                private stringParameter levelID;
                private intParameter Return_Code; //Declare Output Parameter

                public MID_HIERARCHY_REMOVE_ORG_LEVEL_def()
                {
                    base.procedureName = "MID_HIERARCHY_REMOVE_ORG_LEVEL";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HIERARCHY_LEVEL");
                    levelID = new stringParameter("@Level_ID", base.inputParameterList);
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                string levelID
                                                )
                {
                    lock (typeof(MID_HIERARCHY_REMOVE_ORG_LEVEL_def))
                    {
                        this.levelID.SetValue(levelID);
                        this.Return_Code.SetValue(0); //Initialize Output Parameter
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)Return_Code.Value;
                    }
                }
            }

            public static MID_HIERARCHY_ADD_ALT_LEVEL_def MID_HIERARCHY_ADD_ALT_LEVEL = new MID_HIERARCHY_ADD_ALT_LEVEL_def();
            public class MID_HIERARCHY_ADD_ALT_LEVEL_def : baseStoredProcedure
            {
                private intParameter PH_RID;
                private intParameter afterLevel;
                private intParameter Return_Code; //Declare Output Parameter

                public MID_HIERARCHY_ADD_ALT_LEVEL_def()
                {
                    base.procedureName = "MID_HIERARCHY_ADD_ALT_LEVEL";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HIERARCHY_LEVEL");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    afterLevel = new intParameter("@After_Level", base.inputParameterList);
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                int PH_RID,
                                                int afterLevel
                                                )
                {
                    lock (typeof(MID_HIERARCHY_ADD_ALT_LEVEL_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.afterLevel.SetValue(afterLevel);
                        this.Return_Code.SetValue(0); //Initialize Output Parameter
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)Return_Code.Value;
                    }
                }
            }

            public static MID_HIERARCHY_REMOVE_ALT_LEVEL_def MID_HIERARCHY_REMOVE_ALT_LEVEL = new MID_HIERARCHY_REMOVE_ALT_LEVEL_def();
            public class MID_HIERARCHY_REMOVE_ALT_LEVEL_def : baseStoredProcedure
            {
                private intParameter PH_RID;
                private intParameter level;
                private intParameter Return_Code; //Declare Output Parameter

                public MID_HIERARCHY_REMOVE_ALT_LEVEL_def()
                {
                    base.procedureName = "MID_HIERARCHY_REMOVE_ALT_LEVEL";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HIERARCHY_LEVEL");
                    PH_RID = new intParameter("@PH_RID", base.inputParameterList);
                    level = new intParameter("@Level", base.inputParameterList);
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                int PH_RID,
                                                int level
                                                )
                {
                    lock (typeof(MID_HIERARCHY_REMOVE_ALT_LEVEL_def))
                    {
                        this.PH_RID.SetValue(PH_RID);
                        this.level.SetValue(level);
                        this.Return_Code.SetValue(0); //Initialize Output Parameter
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)Return_Code.Value;
                    }
                }
            }

            public static MID_HIERARCHY_TEMP_NODES_DELETE_def MID_HIERARCHY_TEMP_NODES_DELETE = new MID_HIERARCHY_TEMP_NODES_DELETE_def();
            public class MID_HIERARCHY_TEMP_NODES_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_TEMP_NODES_DELETE.SQL"

                public MID_HIERARCHY_TEMP_NODES_DELETE_def()
                {
                    base.procedureName = "MID_HIERARCHY_TEMP_NODES_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("PRODUCT_HIERARCHY");
                }

                public int Delete(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HIERARCHY_TEMP_NODES_DELETE_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
            // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
