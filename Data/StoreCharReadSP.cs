using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class StoreCharRead : DataLayer
    {
        protected static class StoredProcedures
        {
            // Begin TT#2131-MD - JSmith - Halo Integration
            public static MID_STORE_CHAR_READ_ALL_def MID_STORE_CHAR_READ_ALL = new MID_STORE_CHAR_READ_ALL_def();
            public class MID_STORE_CHAR_READ_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_ALL.SQL"

                public MID_STORE_CHAR_READ_ALL_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_ALL";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("STORE_CHARACTERISTICS");
                }

                public DataTable Read(DatabaseAccess _dba
                                      )
                {
                    lock (typeof(MID_STORE_CHAR_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // End TT#2131-MD - JSmith - Halo Integration

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
