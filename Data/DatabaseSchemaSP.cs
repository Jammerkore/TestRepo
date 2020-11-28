using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace MIDRetail.Data
{
    public partial class DatabaseSchema
    {
        protected static class StoredProcedures
        {

            public static SP_MID_GET_TABLE_FROM_TYPE_def SP_MID_GET_TABLE_FROM_TYPE = new SP_MID_GET_TABLE_FROM_TYPE_def();
            public class SP_MID_GET_TABLE_FROM_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_TABLE_FROM_TYPE.SQL"

                private intParameter TABLE_TYPE;

                public SP_MID_GET_TABLE_FROM_TYPE_def()
                {
                    base.procedureName = "SP_MID_GET_TABLE_FROM_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("");
                    TABLE_TYPE = new intParameter("@TABLE_TYPE", base.inputParameterList);
                }
                
                [UnitTestMethodAttribute(BypassValidation = true)]
                public DataTable Read(DatabaseAccess _dba, int TABLE_TYPE)
                {
                    lock (typeof(SP_MID_GET_TABLE_FROM_TYPE_def))
                    {
                        this.TABLE_TYPE.SetValue(TABLE_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_TABLE_READ_SCHEMA_def MID_TABLE_READ_SCHEMA = new MID_TABLE_READ_SCHEMA_def();
            public class MID_TABLE_READ_SCHEMA_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TABLE_READ_SCHEMA.SQL"

                private stringParameter TABLE_NAME;

                public MID_TABLE_READ_SCHEMA_def()
                {
                    base.procedureName = "MID_TABLE_READ_SCHEMA";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("");
                    TABLE_NAME = new stringParameter("@TABLE_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string TABLE_NAME)
                {
                    lock (typeof(MID_TABLE_READ_SCHEMA_def))
                    {
                        this.TABLE_NAME.SetValue(TABLE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }

    }
}
