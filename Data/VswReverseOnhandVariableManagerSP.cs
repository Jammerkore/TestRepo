using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class VswReverseOnhandVariableManager
    {
        protected static class StoredProcedures
        {

            public static MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER_def MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER = new MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER_def();
			public class MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER.SQL"

			    private tableParameter HEADER_LIST;
			
			    public MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER_def()
			    {
			        base.procedureName = "MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("VSW_REVERSE_ON_HAND");
			        HEADER_LIST = new tableParameter("@HEADER_LIST", "HDR_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, DataTable HEADER_LIST)
			    {
                    lock (typeof(MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER_def))
                    {
                        this.HEADER_LIST.SetValue(HEADER_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }

    }
}
