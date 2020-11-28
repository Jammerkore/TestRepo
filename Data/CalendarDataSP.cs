using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class CalendarData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_CALENDAR_MODEL_READ_ALL_def MID_CALENDAR_MODEL_READ_ALL = new MID_CALENDAR_MODEL_READ_ALL_def();
            public class MID_CALENDAR_MODEL_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_READ_ALL.SQL"

                public MID_CALENDAR_MODEL_READ_ALL_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_MODEL");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_CALENDAR_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FISCAL_WEEKS_INSERT_def MID_FISCAL_WEEKS_INSERT = new MID_FISCAL_WEEKS_INSERT_def();
            public class MID_FISCAL_WEEKS_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FISCAL_WEEKS_INSERT.SQL"

                private intParameter ROW_SEQUENCE;
                private intParameter FISCAL_WEEK;
                private intParameter FIRST_DAY_OF_WEEK;
                private intParameter LAST_DAY_OF_WEEK;
                private intParameter FIRST_DAY_OF_WEEK_OFFSET;
                private intParameter LAST_DAY_OF_WEEK_OFFSET;

                public MID_FISCAL_WEEKS_INSERT_def()
                {
                    base.procedureName = "MID_FISCAL_WEEKS_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("FISCAL_WEEKS");
                    ROW_SEQUENCE = new intParameter("@ROW_SEQUENCE", base.inputParameterList);
                    FISCAL_WEEK = new intParameter("@FISCAL_WEEK", base.inputParameterList);
                    FIRST_DAY_OF_WEEK = new intParameter("@FIRST_DAY_OF_WEEK", base.inputParameterList);
                    LAST_DAY_OF_WEEK = new intParameter("@LAST_DAY_OF_WEEK", base.inputParameterList);
                    FIRST_DAY_OF_WEEK_OFFSET = new intParameter("@FIRST_DAY_OF_WEEK_OFFSET", base.inputParameterList);
                    LAST_DAY_OF_WEEK_OFFSET = new intParameter("@LAST_DAY_OF_WEEK_OFFSET", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                    int? ROW_SEQUENCE,
                                    int? FISCAL_WEEK,
                                    int? FIRST_DAY_OF_WEEK,
                                    int? LAST_DAY_OF_WEEK,
                                    int? FIRST_DAY_OF_WEEK_OFFSET,
                                    int? LAST_DAY_OF_WEEK_OFFSET
                                 )
                {
                    lock (typeof(MID_FISCAL_WEEKS_INSERT_def))
                    {
                        this.ROW_SEQUENCE.SetValue(ROW_SEQUENCE);
                        this.FISCAL_WEEK.SetValue(FISCAL_WEEK);
                        this.FIRST_DAY_OF_WEEK.SetValue(FIRST_DAY_OF_WEEK);
                        this.LAST_DAY_OF_WEEK.SetValue(LAST_DAY_OF_WEEK);
                        this.FIRST_DAY_OF_WEEK_OFFSET.SetValue(FIRST_DAY_OF_WEEK_OFFSET);
                        this.LAST_DAY_OF_WEEK_OFFSET.SetValue(LAST_DAY_OF_WEEK_OFFSET);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE_def MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE = new MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE_def();
            public class MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE.SQL"

                private intParameter CM_RID;
                private intParameter CMP_TYPE;

                public MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_MODEL_PERIODS");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                    CMP_TYPE = new intParameter("@CMP_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? CM_RID,
                                      int? CMP_TYPE
                                      )
                {
                    lock (typeof(MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        this.CMP_TYPE.SetValue(CMP_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR_def MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR = new MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR_def();
            public class MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR.SQL"

                private stringParameter CM_ID;
                private intParameter FISCAL_YEAR;

                public MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_MODEL");
                    CM_ID = new stringParameter("@CM_ID", base.inputParameterList);
                    FISCAL_YEAR = new intParameter("@FISCAL_YEAR", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      string CM_ID,
                                      int? FISCAL_YEAR
                                     )
                {
                    lock (typeof(MID_CALENDAR_MODEL_READ_RID_FROM_ID_AND_YEAR_def))
                    {
                        this.CM_ID.SetValue(CM_ID);
                        this.FISCAL_YEAR.SetValue(FISCAL_YEAR);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CALENDAR_MODEL_DELETE_def MID_CALENDAR_MODEL_DELETE = new MID_CALENDAR_MODEL_DELETE_def();
            public class MID_CALENDAR_MODEL_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_DELETE.SQL"

                private intParameter CM_RID;

                public MID_CALENDAR_MODEL_DELETE_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_MODEL");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? CM_RID)
                {
                    lock (typeof(MID_CALENDAR_MODEL_DELETE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CALENDAR_MODEL_PERIODS_DELETE_def MID_CALENDAR_MODEL_PERIODS_DELETE = new MID_CALENDAR_MODEL_PERIODS_DELETE_def();
            public class MID_CALENDAR_MODEL_PERIODS_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_PERIODS_DELETE.SQL"

                private intParameter CM_RID;

                public MID_CALENDAR_MODEL_PERIODS_DELETE_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_PERIODS_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_MODEL_PERIODS");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? CM_RID)
                {
                    lock (typeof(MID_CALENDAR_MODEL_PERIODS_DELETE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CALENDAR_DATE_RANGE_READ_def MID_CALENDAR_DATE_RANGE_READ = new MID_CALENDAR_DATE_RANGE_READ_def();
            public class MID_CALENDAR_DATE_RANGE_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_DATE_RANGE_READ.SQL"

                private intParameter CDR_RID;

                public MID_CALENDAR_DATE_RANGE_READ_def()
                {
                    base.procedureName = "MID_CALENDAR_DATE_RANGE_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? CDR_RID)
                {
                    lock (typeof(MID_CALENDAR_DATE_RANGE_READ_def))
                    {
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static SP_MID_CALENDARDTRANGE_INSERT_def SP_MID_CALENDARDTRANGE_INSERT = new SP_MID_CALENDARDTRANGE_INSERT_def();
            public class SP_MID_CALENDARDTRANGE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CALENDARDTRANGE_INSERT.SQL"

                private intParameter CDR_START;
                private intParameter CDR_END;
                private intParameter CDR_RANGE_TYPE_ID;
                private intParameter CDR_DATE_TYPE_ID;
                private intParameter CDR_RELATIVE_TO;
                private stringParameter CDR_NAME;
                private charParameter CDR_DYNAMIC_SWITCH;
                private intParameter CDR_DYNAMIC_SWITCH_DATE;
                private intParameter CDR_RID; //Declare Output Parameter

                public SP_MID_CALENDARDTRANGE_INSERT_def()
                {
                    base.procedureName = "SP_MID_CALENDARDTRANGE_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                    CDR_START = new intParameter("@CDR_START", base.inputParameterList);
                    CDR_END = new intParameter("@CDR_END", base.inputParameterList);
                    CDR_RANGE_TYPE_ID = new intParameter("@CDR_RANGE_TYPE_ID", base.inputParameterList);
                    CDR_DATE_TYPE_ID = new intParameter("@CDR_DATE_TYPE_ID", base.inputParameterList);
                    CDR_RELATIVE_TO = new intParameter("@CDR_RELATIVE_TO", base.inputParameterList);
                    CDR_NAME = new stringParameter("@CDR_NAME", base.inputParameterList);
                    CDR_DYNAMIC_SWITCH = new charParameter("@CDR_DYNAMIC_SWITCH", base.inputParameterList);
                    CDR_DYNAMIC_SWITCH_DATE = new intParameter("@CDR_DYNAMIC_SWITCH_DATE", base.inputParameterList);

                    CDR_RID = new intParameter("@CDR_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
                                              int? CDR_START,
                                              int? CDR_END,
                                              int? CDR_RANGE_TYPE_ID,
                                              int? CDR_DATE_TYPE_ID,
                                              int? CDR_RELATIVE_TO,
                                              string CDR_NAME,
                                              char? CDR_DYNAMIC_SWITCH,
                                              int? CDR_DYNAMIC_SWITCH_DATE
                                              )
                {
                    lock (typeof(SP_MID_CALENDARDTRANGE_INSERT_def))
                    {
                        this.CDR_START.SetValue(CDR_START);
                        this.CDR_END.SetValue(CDR_END);
                        this.CDR_RANGE_TYPE_ID.SetValue(CDR_RANGE_TYPE_ID);
                        this.CDR_DATE_TYPE_ID.SetValue(CDR_DATE_TYPE_ID);
                        this.CDR_RELATIVE_TO.SetValue(CDR_RELATIVE_TO);
                        this.CDR_NAME.SetValue(CDR_NAME);
                        this.CDR_DYNAMIC_SWITCH.SetValue(CDR_DYNAMIC_SWITCH);
                        this.CDR_DYNAMIC_SWITCH_DATE.SetValue(CDR_DYNAMIC_SWITCH_DATE);
                        this.CDR_RID.SetValue(null); //Initialize Output Parameter

                        return base.ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_CALENDAR_DATE_RANGE_UPDATE_def MID_CALENDAR_DATE_RANGE_UPDATE = new MID_CALENDAR_DATE_RANGE_UPDATE_def();
            public class MID_CALENDAR_DATE_RANGE_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_DATE_RANGE_UPDATE.SQL"

                private intParameter CDR_RID;
                private intParameter CDR_START;
                private intParameter CDR_END;
                private intParameter CDR_RANGE_TYPE_ID;
                private intParameter CDR_DATE_TYPE_ID;
                private intParameter CDR_RELATIVE_TO;
                private stringParameter CDR_NAME;
                private charParameter CDR_DYNAMIC_SWITCH;
                private intParameter CDR_DYNAMIC_SWITCH_DATE;

                public MID_CALENDAR_DATE_RANGE_UPDATE_def()
                {
                    base.procedureName = "MID_CALENDAR_DATE_RANGE_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    CDR_START = new intParameter("@CDR_START", base.inputParameterList);
                    CDR_END = new intParameter("@CDR_END", base.inputParameterList);
                    CDR_RANGE_TYPE_ID = new intParameter("@CDR_RANGE_TYPE_ID", base.inputParameterList);
                    CDR_DATE_TYPE_ID = new intParameter("@CDR_DATE_TYPE_ID", base.inputParameterList);
                    CDR_RELATIVE_TO = new intParameter("@CDR_RELATIVE_TO", base.inputParameterList);
                    CDR_NAME = new stringParameter("@CDR_NAME", base.inputParameterList);
                    CDR_DYNAMIC_SWITCH = new charParameter("@CDR_DYNAMIC_SWITCH", base.inputParameterList);
                    CDR_DYNAMIC_SWITCH_DATE = new intParameter("@CDR_DYNAMIC_SWITCH_DATE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? CDR_RID,
                                  int? CDR_START,
                                  int? CDR_END,
                                  int? CDR_RANGE_TYPE_ID,
                                  int? CDR_DATE_TYPE_ID,
                                  int? CDR_RELATIVE_TO,
                                  string CDR_NAME,
                                  char? CDR_DYNAMIC_SWITCH,
                                  int? CDR_DYNAMIC_SWITCH_DATE
                                  )
                {
                    lock (typeof(MID_CALENDAR_DATE_RANGE_UPDATE_def))
                    {
                        this.CDR_RID.SetValue(CDR_RID);
                        this.CDR_START.SetValue(CDR_START);
                        this.CDR_END.SetValue(CDR_END);
                        this.CDR_RANGE_TYPE_ID.SetValue(CDR_RANGE_TYPE_ID);
                        this.CDR_DATE_TYPE_ID.SetValue(CDR_DATE_TYPE_ID);
                        this.CDR_RELATIVE_TO.SetValue(CDR_RELATIVE_TO);
                        this.CDR_NAME.SetValue(CDR_NAME);
                        this.CDR_DYNAMIC_SWITCH.SetValue(CDR_DYNAMIC_SWITCH);
                        this.CDR_DYNAMIC_SWITCH_DATE.SetValue(CDR_DYNAMIC_SWITCH_DATE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_CALENDAR_DATE_RANGE_UPDATE_NAME_def MID_CALENDAR_DATE_RANGE_UPDATE_NAME = new MID_CALENDAR_DATE_RANGE_UPDATE_NAME_def();
            public class MID_CALENDAR_DATE_RANGE_UPDATE_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_DATE_RANGE_UPDATE_NAME.SQL"

                private intParameter CDR_RID;
                private stringParameter CDR_NAME;

                public MID_CALENDAR_DATE_RANGE_UPDATE_NAME_def()
                {
                    base.procedureName = "MID_CALENDAR_DATE_RANGE_UPDATE_NAME";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                    CDR_NAME = new stringParameter("@CDR_NAME", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? CDR_RID,
                                  string CDR_NAME
                                  )
                {
                    lock (typeof(MID_CALENDAR_DATE_RANGE_UPDATE_NAME_def))
                    {
                        this.CDR_RID.SetValue(CDR_RID);
                        this.CDR_NAME.SetValue(CDR_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL_def MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL = new MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL_def();
            public class MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL.SQL"

                public MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL_def()
                {
                    base.procedureName = "MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_DATE_RANGE_NAMES");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CALENDAR_DATE_RANGE_DELETE_def MID_CALENDAR_DATE_RANGE_DELETE = new MID_CALENDAR_DATE_RANGE_DELETE_def();
            public class MID_CALENDAR_DATE_RANGE_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_DATE_RANGE_DELETE.SQL"

                private intParameter CDR_RID;

                public MID_CALENDAR_DATE_RANGE_DELETE_def()
                {
                    base.procedureName = "MID_CALENDAR_DATE_RANGE_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                    CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? CDR_RID)
                {
                    lock (typeof(MID_CALENDAR_DATE_RANGE_DELETE_def))
                    {
                        this.CDR_RID.SetValue(CDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CALENDAR_WEEK53_YEAR_READ_ALL_def MID_CALENDAR_WEEK53_YEAR_READ_ALL = new MID_CALENDAR_WEEK53_YEAR_READ_ALL_def();
            public class MID_CALENDAR_WEEK53_YEAR_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_WEEK53_YEAR_READ_ALL.SQL"

                public MID_CALENDAR_WEEK53_YEAR_READ_ALL_def()
                {
                    base.procedureName = "MID_CALENDAR_WEEK53_YEAR_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_CALENDAR_WEEK53_YEAR_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CALENDAR_WEEK53_YEAR_READ_def MID_CALENDAR_WEEK53_YEAR_READ = new MID_CALENDAR_WEEK53_YEAR_READ_def();
            public class MID_CALENDAR_WEEK53_YEAR_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_WEEK53_YEAR_READ.SQL"

                private intParameter CM_RID;
                private intParameter CMP_TYPE;

                public MID_CALENDAR_WEEK53_YEAR_READ_def()
                {
                    base.procedureName = "MID_CALENDAR_WEEK53_YEAR_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                    CMP_TYPE = new intParameter("@CMP_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, 
                                      int? CM_RID,
                                      int? CMP_TYPE
                                      )
                {
                    lock (typeof(MID_CALENDAR_WEEK53_YEAR_READ_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        this.CMP_TYPE.SetValue(CMP_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_CALENDAR_WEEK53_YEAR_DELETE_def MID_CALENDAR_WEEK53_YEAR_DELETE = new MID_CALENDAR_WEEK53_YEAR_DELETE_def();
            public class MID_CALENDAR_WEEK53_YEAR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_WEEK53_YEAR_DELETE.SQL"

                private intParameter CM_RID;

                public MID_CALENDAR_WEEK53_YEAR_DELETE_def()
                {
                    base.procedureName = "MID_CALENDAR_WEEK53_YEAR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? CM_RID)
                {
                    lock (typeof(MID_CALENDAR_WEEK53_YEAR_DELETE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ_def MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ = new MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ_def();
            public class MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ.SQL"

                private intParameter CM_RID;
                private intParameter WEEK53_FISCAL_YEAR;
                private intParameter CMP_SEQUENCE;

                public MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ_def()
                {
                    base.procedureName = "MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                    WEEK53_FISCAL_YEAR = new intParameter("@WEEK53_FISCAL_YEAR", base.inputParameterList);
                    CMP_SEQUENCE = new intParameter("@CMP_SEQUENCE", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? CM_RID,
                                  int? WEEK53_FISCAL_YEAR,
                                  int? CMP_SEQUENCE
                                  )
                {
                    lock (typeof(MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        this.WEEK53_FISCAL_YEAR.SetValue(WEEK53_FISCAL_YEAR);
                        this.CMP_SEQUENCE.SetValue(CMP_SEQUENCE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static SP_MID_CALENDARWEEK53_INSERT_def SP_MID_CALENDARWEEK53_INSERT = new SP_MID_CALENDARWEEK53_INSERT_def();
            public class SP_MID_CALENDARWEEK53_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_CALENDARWEEK53_INSERT.SQL"

                private intParameter WEEK53_FISCAL_YEAR;
                private intParameter CM_RID;
                private intParameter CMP_SEQUENCE;
                private intParameter OFFSET_ID;
                private intParameter CMP_TYPE;

                public SP_MID_CALENDARWEEK53_INSERT_def()
                {
                    base.procedureName = "SP_MID_CALENDARWEEK53_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                    WEEK53_FISCAL_YEAR = new intParameter("@WEEK53_FISCAL_YEAR", base.inputParameterList);
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                    CMP_SEQUENCE = new intParameter("@CMP_SEQUENCE", base.inputParameterList);
                    OFFSET_ID = new intParameter("@OFFSET_ID", base.inputParameterList);
                    CMP_TYPE = new intParameter("@CMP_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba, 
                                  int? WEEK53_FISCAL_YEAR,
                                  int? CM_RID,
                                  int? CMP_SEQUENCE,
                                  int? OFFSET_ID,
                                  int? CMP_TYPE
                                  )
                {
                    lock (typeof(SP_MID_CALENDARWEEK53_INSERT_def))
                    {
                        this.WEEK53_FISCAL_YEAR.SetValue(WEEK53_FISCAL_YEAR);
                        this.CM_RID.SetValue(CM_RID);
                        this.CMP_SEQUENCE.SetValue(CMP_SEQUENCE);
                        this.OFFSET_ID.SetValue(OFFSET_ID);
                        this.CMP_TYPE.SetValue(CMP_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_CALENDAR_WEEK53_YEAR_UPDATE_def MID_CALENDAR_WEEK53_YEAR_UPDATE = new MID_CALENDAR_WEEK53_YEAR_UPDATE_def();
            public class MID_CALENDAR_WEEK53_YEAR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_WEEK53_YEAR_UPDATE.SQL"

                private intParameter WEEK53_FISCAL_YEAR;
                private intParameter CM_RID;
                private intParameter CMP_SEQUENCE;
                private intParameter OFFSET_ID;

                public MID_CALENDAR_WEEK53_YEAR_UPDATE_def()
                {
                    base.procedureName = "MID_CALENDAR_WEEK53_YEAR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("CALENDAR_WEEK53_YEAR");
                    WEEK53_FISCAL_YEAR = new intParameter("@WEEK53_FISCAL_YEAR", base.inputParameterList);
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                    CMP_SEQUENCE = new intParameter("@CMP_SEQUENCE", base.inputParameterList);
                    OFFSET_ID = new intParameter("@OFFSET_ID", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, 
                                  int? WEEK53_FISCAL_YEAR,
                                  int? CM_RID,
                                  int? CMP_SEQUENCE,
                                  int? OFFSET_ID
                                  )
                {
                    lock (typeof(MID_CALENDAR_WEEK53_YEAR_UPDATE_def))
                    {
                        this.WEEK53_FISCAL_YEAR.SetValue(WEEK53_FISCAL_YEAR);
                        this.CM_RID.SetValue(CM_RID);
                        this.CMP_SEQUENCE.SetValue(CMP_SEQUENCE);
                        this.OFFSET_ID.SetValue(OFFSET_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_POSTING_DATE_RID_READ_ALL_def MID_POSTING_DATE_RID_READ_ALL = new MID_POSTING_DATE_RID_READ_ALL_def();
            public class MID_POSTING_DATE_RID_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_POSTING_DATE_RID_READ_ALL.SQL"

                public MID_POSTING_DATE_RID_READ_ALL_def()
                {
                    base.procedureName = "MID_POSTING_DATE_RID_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("POSTING_DATE");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_POSTING_DATE_RID_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static SP_MID_DELETE_DATE_RANGES_def SP_MID_DELETE_DATE_RANGES = new SP_MID_DELETE_DATE_RANGES_def();
            public class SP_MID_DELETE_DATE_RANGES_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DELETE_DATE_RANGES.SQL"

                public SP_MID_DELETE_DATE_RANGES_def()
                {
                    base.procedureName = "SP_MID_DELETE_DATE_RANGES";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CALENDAR_DATE_RANGE");
                }

                public int Delete(DatabaseAccess _dba)
                {
                    lock (typeof(SP_MID_DELETE_DATE_RANGES_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_CALENDAR_MODEL_READ_def MID_CALENDAR_MODEL_READ = new MID_CALENDAR_MODEL_READ_def();
			public class MID_CALENDAR_MODEL_READ_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_READ.SQL"

			    private intParameter CM_RID;
			
			    public MID_CALENDAR_MODEL_READ_def()
			    {
			        base.procedureName = "MID_CALENDAR_MODEL_READ";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
			        base.tableNames.Add("CALENDAR_MODEL");
			        CM_RID = new intParameter("@CM_RID", base.inputParameterList);
			    }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? CM_RID)
			    {
                    lock (typeof(MID_CALENDAR_MODEL_READ_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        DataSet ds = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        ds.Tables[0].TableName = "CALENDAR_MODEL";
                        return ds;
                    }
			    }
			}

			public static MID_CALENDAR_MODEL_UPDATE_def MID_CALENDAR_MODEL_UPDATE = new MID_CALENDAR_MODEL_UPDATE_def();
			public class MID_CALENDAR_MODEL_UPDATE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_UPDATE.SQL"

			    private intParameter CM_RID;
			    private stringParameter CM_ID;
			    private datetimeParameter START_DATE; 
			    private intParameter FISCAL_YEAR;
			
			    public MID_CALENDAR_MODEL_UPDATE_def()
			    {
			        base.procedureName = "MID_CALENDAR_MODEL_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("CALENDAR_MODEL");
			        CM_RID = new intParameter("@CM_RID", base.inputParameterList);
			        CM_ID = new stringParameter("@CM_ID", base.inputParameterList);
			        START_DATE = new datetimeParameter("@START_DATE", base.inputParameterList);
			        FISCAL_YEAR = new intParameter("@FISCAL_YEAR", base.inputParameterList);
			    }

			    public int Update(DatabaseAccess _dba, 
			                      int? CM_RID,
			                      string CM_ID,
			                      DateTime? START_DATE,
			                      int? FISCAL_YEAR
			                      )
			    {
                    lock (typeof(MID_CALENDAR_MODEL_UPDATE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        this.CM_ID.SetValue(CM_ID);
                        this.START_DATE.SetValue(START_DATE);
                        this.FISCAL_YEAR.SetValue(FISCAL_YEAR);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}



            public static MID_CALENDAR_MODEL_PERIODS_READ_def MID_CALENDAR_MODEL_PERIODS_READ = new MID_CALENDAR_MODEL_PERIODS_READ_def();
            public class MID_CALENDAR_MODEL_PERIODS_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_PERIODS_READ.SQL"

                private intParameter CM_RID;

                public MID_CALENDAR_MODEL_PERIODS_READ_def()
                {
                    base.procedureName = "MID_CALENDAR_MODEL_PERIODS_READ";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("CALENDAR_MODEL_PERIODS");
                    CM_RID = new intParameter("@CM_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? CM_RID)
                {
                    lock (typeof(MID_CALENDAR_MODEL_PERIODS_READ_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        DataSet ds = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        ds.Tables[0].TableName = "CALENDAR_MODEL_PERIODS";
                        return ds;
                    }
                }
            }

			public static MID_CALENDAR_MODEL_PERIODS_UPDATE_def MID_CALENDAR_MODEL_PERIODS_UPDATE = new MID_CALENDAR_MODEL_PERIODS_UPDATE_def();
			public class MID_CALENDAR_MODEL_PERIODS_UPDATE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_CALENDAR_MODEL_PERIODS_UPDATE.SQL"

			    private intParameter CM_RID;
			    private intParameter CMP_SEQUENCE;
			    private intParameter CMP_TYPE;
			    private stringParameter CMP_ID;
			    private stringParameter CMP_ABBREVIATION;
			    private intParameter NO_OF_TIME_PERIODS;
			
			    public MID_CALENDAR_MODEL_PERIODS_UPDATE_def()
			    {
			        base.procedureName = "MID_CALENDAR_MODEL_PERIODS_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("CALENDAR_MODEL_PERIODS");
			        CM_RID = new intParameter("@CM_RID", base.inputParameterList);
			        CMP_SEQUENCE = new intParameter("@CMP_SEQUENCE", base.inputParameterList);
			        CMP_TYPE = new intParameter("@CMP_TYPE", base.inputParameterList);
			        CMP_ID = new stringParameter("@CMP_ID", base.inputParameterList);
			        CMP_ABBREVIATION = new stringParameter("@CMP_ABBREVIATION", base.inputParameterList);
			        NO_OF_TIME_PERIODS = new intParameter("@NO_OF_TIME_PERIODS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? CM_RID,
			                      int? CMP_SEQUENCE,
			                      int? CMP_TYPE,
			                      string CMP_ID,
			                      string CMP_ABBREVIATION,
			                      int? NO_OF_TIME_PERIODS
			                      )
			    {
                    lock (typeof(MID_CALENDAR_MODEL_PERIODS_UPDATE_def))
                    {
                        this.CM_RID.SetValue(CM_RID);
                        this.CMP_SEQUENCE.SetValue(CMP_SEQUENCE);
                        this.CMP_TYPE.SetValue(CMP_TYPE);
                        this.CMP_ID.SetValue(CMP_ID);
                        this.CMP_ABBREVIATION.SetValue(CMP_ABBREVIATION);
                        this.NO_OF_TIME_PERIODS.SetValue(NO_OF_TIME_PERIODS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_FISCAL_WEEKS_DELETE_ALL_def MID_FISCAL_WEEKS_DELETE_ALL = new MID_FISCAL_WEEKS_DELETE_ALL_def();
			public class MID_FISCAL_WEEKS_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FISCAL_WEEKS_DELETE_ALL.SQL"

			
			    public MID_FISCAL_WEEKS_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_FISCAL_WEEKS_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FISCAL_WEEKS");
			    }
			
			    public int Delete(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FISCAL_WEEKS_DELETE_ALL_def))
                    {
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE

        }
    }  
}
