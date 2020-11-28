using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class BatchComp : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_BATCH_COMP_SELECTION_FILTER_READ_ALL_def MID_BATCH_COMP_SELECTION_FILTER_READ_ALL = new MID_BATCH_COMP_SELECTION_FILTER_READ_ALL_def();
            public class MID_BATCH_COMP_SELECTION_FILTER_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BATCH_COMP_SELECTION_FILTER_READ_ALL.SQL"

                public MID_BATCH_COMP_SELECTION_FILTER_READ_ALL_def()
                {
                    base.procedureName = "MID_BATCH_COMP_SELECTION_FILTER_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_BATCH_COMP_SELECTION_FILTER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_BATCH_COMP_SELECTION_FILTER_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_BATCH_COMP_SELECTION_FILTER_READ_def MID_BATCH_COMP_SELECTION_FILTER_READ = new MID_BATCH_COMP_SELECTION_FILTER_READ_def();
            public class MID_BATCH_COMP_SELECTION_FILTER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PROC_HDR_READ.SQL"

                private intParameter BATCH_COMP_RID;

                public MID_BATCH_COMP_SELECTION_FILTER_READ_def()
                {
                    base.procedureName = "MID_BATCH_COMP_SELECTION_FILTER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("MID_BATCH_COMP_SELECTION_FILTER");
                    BATCH_COMP_RID = new intParameter("@BATCH_COMP_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? BATCH_COMP_RID)
                {
                    lock (typeof(MID_BATCH_COMP_SELECTION_FILTER_READ_def))
                    {
                        this.BATCH_COMP_RID.SetValue(BATCH_COMP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            //INSERT NEW STORED PROCEDURES ABOVE HERE
        } 

    } //End DataLayer
}
