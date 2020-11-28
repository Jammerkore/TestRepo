using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.Data
{
    public partial class ResourceLock
    {
        protected static class StoredProcedures
        {
            //public static MID_VIRTUAL_LOCK_INSERT_def MID_VIRTUAL_LOCK_INSERT = new MID_VIRTUAL_LOCK_INSERT_def();
            //public class MID_VIRTUAL_LOCK_INSERT_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VIRTUAL_LOCK_INSERT.SQL"

            //    private intParameter LOCK_TYPE;
            //    private stringParameter LOCK_ID;

            //    public MID_VIRTUAL_LOCK_INSERT_def()
            //    {
            //        base.procedureName = "MID_VIRTUAL_LOCK_INSERT";
            //        base.procedureType = storedProcedureTypes.Insert;
            //        base.tableNames.Add("VIRTUAL_LOCK");
            //        LOCK_TYPE = new intParameter("@LOCK_TYPE", base.inputParameterList);
            //        LOCK_ID = new stringParameter("@LOCK_ID", base.inputParameterList);
            //    }

            //    public int Insert(DatabaseAccess _dba,
            //                      int? LOCK_TYPE,
            //                      string LOCK_ID
            //                      )
            //    {
            //        lock (typeof(MID_VIRTUAL_LOCK_INSERT_def))
            //        {
            //            this.LOCK_TYPE.SetValue(LOCK_TYPE);
            //            this.LOCK_ID.SetValue(LOCK_ID);
            //            return ExecuteStoredProcedureForInsert(_dba);
            //        }
            //    }
            //}

            //public static MID_VIRTUAL_LOCK_DELETE_def MID_VIRTUAL_LOCK_DELETE = new MID_VIRTUAL_LOCK_DELETE_def();
            //public class MID_VIRTUAL_LOCK_DELETE_def : baseStoredProcedure
            //{
            //    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VIRTUAL_LOCK_DELETE.SQL"

            //    private intParameter LOCK_TYPE;
            //    private stringParameter LOCK_ID;

            //    public MID_VIRTUAL_LOCK_DELETE_def()
            //    {
            //        base.procedureName = "MID_VIRTUAL_LOCK_DELETE";
            //        base.procedureType = storedProcedureTypes.Delete;
            //        base.tableNames.Add("VIRTUAL_LOCK");
            //        LOCK_TYPE = new intParameter("@LOCK_TYPE", base.inputParameterList);
            //        LOCK_ID = new stringParameter("@LOCK_ID", base.inputParameterList);
            //    }

            //    public int Delete(DatabaseAccess _dba,
            //                      int? LOCK_TYPE,
            //                      string LOCK_ID
            //                      )
            //    {
            //        lock (typeof(MID_VIRTUAL_LOCK_DELETE_def))
            //        {
            //            this.LOCK_TYPE.SetValue(LOCK_TYPE);
            //            this.LOCK_ID.SetValue(LOCK_ID);
            //            return ExecuteStoredProcedureForDelete(_dba);
            //        }
            //    }
            //}
            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }
}
