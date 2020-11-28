using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class SystemData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_DETAIL_ACCESS_def SP_MID_DETAIL_ACCESS = new SP_MID_DETAIL_ACCESS_def();
            public class SP_MID_DETAIL_ACCESS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DETAIL_ACCESS.SQL"

                private intParameter inUseType;
                private intParameter inUseRID;
                private intParameter outAllowDelete; //Declare Output Parameter
			
			    public SP_MID_DETAIL_ACCESS_def()
			    {
                    base.procedureName = "SP_MID_DETAIL_ACCESS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DETAIL_INFO");
                    inUseType = new intParameter("@inUseType", base.inputParameterList);
                    inUseRID = new intParameter("@inUseRID", base.inputParameterList);
                    outAllowDelete = new intParameter("@outAllowDelete", base.outputParameterList); //Add Output Parameter
			    }
			
			    public DataTable Read(DatabaseAccess _dba,
                                      ref bool allowDelete, 
                                      int? inUseType,
                                      int? inUseRID
			                          )
			    {
                    lock (typeof(SP_MID_DETAIL_ACCESS_def))
                    {
                        this.inUseType.SetValue(inUseType);
                        this.inUseRID.SetValue(inUseRID);
                        this.outAllowDelete.SetValue(null); //Initialize Output Parameter
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        allowDelete = Include.ConvertIntToBool((int)this.outAllowDelete.Value);
                        return dt;
                    }
			    }
			}

            public static SP_MID_APPLICATION_LABEL_HEADINGS_def SP_MID_APPLICATION_LABEL_HEADINGS = new SP_MID_APPLICATION_LABEL_HEADINGS_def();
			public class SP_MID_APPLICATION_LABEL_HEADINGS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_APPLICATION_LABEL_HEADINGS.SQL"

                private intParameter inUseType;

                public SP_MID_APPLICATION_LABEL_HEADINGS_def()
			    {
			        base.procedureName = "SP_MID_APPLICATION_LABEL_HEADINGS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("APPLICATION_LABELS");
                    inUseType = new intParameter("@inUseType", base.inputParameterList);
			    }

                public DataTable Read(DatabaseAccess _dba, int? inUseType)
			    {
                    lock (typeof(SP_MID_APPLICATION_LABEL_HEADINGS_def))
                    {
                        this.inUseType.SetValue(inUseType);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_DB_READ_TABLE_ROW_COUNTS_def MID_DB_READ_TABLE_ROW_COUNTS = new MID_DB_READ_TABLE_ROW_COUNTS_def();
			public class MID_DB_READ_TABLE_ROW_COUNTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DB_READ_TABLE_ROW_COUNTS.SQL"

			
			    public MID_DB_READ_TABLE_ROW_COUNTS_def()
			    {
			        base.procedureName = "MID_DB_READ_TABLE_ROW_COUNTS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DB");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_DB_READ_TABLE_ROW_COUNTS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			// Begin TT#1581-MD - stodd - API Header Reconcile
            public static MID_API_PROCESS_CONTROL_RULES_READ_def MID_API_PROCESS_CONTROL_RULES_READ = new MID_API_PROCESS_CONTROL_RULES_READ_def();
            public class MID_API_PROCESS_CONTROL_RULES_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_API_PROCESS_CONTROL_RULES_READ.SQL"

                private intParameter API_ID;


                public MID_API_PROCESS_CONTROL_RULES_READ_def()
                {
                    base.procedureName = "MID_API_PROCESS_CONTROL_RULES_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("API_PROCESS_CONTROL_RULES");
                    API_ID = new intParameter("@API_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? API_ID)
                {
                    lock (typeof(MID_API_PROCESS_CONTROL_RULES_READ_def))
                    {
                        this.API_ID.SetValue(API_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
			// End TT#1581-MD - stodd - API Header Reconcile

            public static MID_API_PROCESS_CONTROL_RULES_UPDATE_def MID_API_PROCESS_CONTROL_RULES_UPDATE = new MID_API_PROCESS_CONTROL_RULES_UPDATE_def();
            public class MID_API_PROCESS_CONTROL_RULES_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_API_PROCESS_CONTROL_RULES_UPDATE.SQL"

                private intParameter API_ID;
                private charParameter PROCESS_CANNOT_BE_RUNNING_IND;
                private charParameter PROCESS_MUST_BE_RUNNING_IND;
                private intParameter PROCESS_ID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
                private stringParameter LAST_MODIFIED_BY;

                public MID_API_PROCESS_CONTROL_RULES_UPDATE_def()
                {
                    base.procedureName = "MID_API_PROCESS_CONTROL_RULES_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("API_PROCESS_CONTROL_RULES");
                    API_ID = new intParameter("@API_ID", base.inputParameterList);
                    PROCESS_CANNOT_BE_RUNNING_IND = new charParameter("@PROCESS_CANNOT_BE_RUNNING_IND", base.inputParameterList);
                    PROCESS_MUST_BE_RUNNING_IND = new charParameter("@PROCESS_MUST_BE_RUNNING_IND", base.inputParameterList);
                    PROCESS_ID = new intParameter("@PROCESS_ID", base.inputParameterList);
                    LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
                    LAST_MODIFIED_BY = new stringParameter("@LAST_MODIFIED_BY", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? API_ID,
                                  char? PROCESS_CANNOT_BE_RUNNING_IND,
                                  char? PROCESS_MUST_BE_RUNNING_IND,
                                  int? PROCESS_ID,
                                  DateTime? LAST_MODIFIED_DATETIME,
                                  string LAST_MODIFIED_BY
                                  )
                {
                    lock (typeof(MID_API_PROCESS_CONTROL_RULES_UPDATE_def))
                    {
                        this.API_ID.SetValue(API_ID);
                        this.PROCESS_CANNOT_BE_RUNNING_IND.SetValue(PROCESS_CANNOT_BE_RUNNING_IND);
                        this.PROCESS_MUST_BE_RUNNING_IND.SetValue(PROCESS_MUST_BE_RUNNING_IND);
                        this.PROCESS_ID.SetValue(PROCESS_ID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        this.LAST_MODIFIED_BY.SetValue(LAST_MODIFIED_BY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
