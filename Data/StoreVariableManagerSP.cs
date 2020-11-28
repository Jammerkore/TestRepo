using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public static partial class MaxStoresHelper
    {
        public static class StoredProcedures
        {
            public static MID_STORES_READ_MAX_def MID_STORES_READ_MAX = new MID_STORES_READ_MAX_def();
            public class MID_STORES_READ_MAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORES_READ_MAX.SQL"


                public MID_STORES_READ_MAX_def()
                {
                    base.procedureName = "MID_STORES_READ_MAX";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORES");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_STORES_READ_MAX_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }


    }
}
