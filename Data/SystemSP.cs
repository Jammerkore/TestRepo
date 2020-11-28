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

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
