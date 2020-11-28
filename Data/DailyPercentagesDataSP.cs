using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class DailyPercentagesCriteriaData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_DAILY_PERCENTAGES_WK_DELETE_def SP_MID_DAILY_PERCENTAGES_WK_DELETE = new SP_MID_DAILY_PERCENTAGES_WK_DELETE_def();
			public class SP_MID_DAILY_PERCENTAGES_WK_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_DAILY_PERCENTAGES_WK_DELETE.SQL"

			    private intParameter COMMIT_LIMIT;
			    private intParameter RECORDS_DELETED; //Declare Output Parameter
			
			    public SP_MID_DAILY_PERCENTAGES_WK_DELETE_def()
			    {
			        base.procedureName = "SP_MID_DAILY_PERCENTAGES_WK_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("DAILY_PERCENTAGES");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			        RECORDS_DELETED = new intParameter("@RECORDS_DELETED", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(SP_MID_DAILY_PERCENTAGES_WK_DELETE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        this.RECORDS_DELETED.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
