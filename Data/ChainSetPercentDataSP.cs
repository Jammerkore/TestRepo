using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ChainSetPercentCriteriaData : DataLayer
    {
        protected static class StoredProcedures
        {
            public static SP_MID_CHAIN_SET_PERCENT_SET_INSERT_def SP_MID_CHAIN_SET_PERCENT_SET_INSERT = new SP_MID_CHAIN_SET_PERCENT_SET_INSERT_def();
            public class SP_MID_CHAIN_SET_PERCENT_SET_INSERT_def : baseStoredProcedure
            {
                private intParameter YEAR_WEEK;
                private intParameter HN_RID;
                private stringParameter STORE_ATTRIBUTE;
                private stringParameter STORE_ATTRIBUTE_SET;
                private decimalParameter PERCENT;
                private intParameter RETURN; //Declare Output Parameter
                private intParameter SG_VERSION;  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

                public SP_MID_CHAIN_SET_PERCENT_SET_INSERT_def()
                {
                    base.procedureName = "SP_MID_CHAIN_SET_PERCENT_SET_INSERT";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    YEAR_WEEK = new intParameter("@YEAR_WEEK", base.inputParameterList);
                    HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    STORE_ATTRIBUTE = new stringParameter("@STORE_ATTRIBUTE", base.inputParameterList);
                    STORE_ATTRIBUTE_SET = new stringParameter("@STORE_ATTRIBUTE_SET", base.inputParameterList);
                    PERCENT = new decimalParameter("@PERCENT", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                    RETURN = new intParameter("@RETURN", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                int? YEAR_WEEK,
                                                int? HN_RID,
                                                string STORE_ATTRIBUTE,
                                                string STORE_ATTRIBUTE_SET,
                                                decimal? PERCENT,
                                                int? SG_VERSION  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                                )
                {
                    lock (typeof(SP_MID_CHAIN_SET_PERCENT_SET_INSERT_def))
                    {
                        this.YEAR_WEEK.SetValue(YEAR_WEEK);
                        this.HN_RID.SetValue(HN_RID);
                        this.STORE_ATTRIBUTE.SetValue(STORE_ATTRIBUTE);
                        this.STORE_ATTRIBUTE_SET.SetValue(STORE_ATTRIBUTE_SET);
                        this.PERCENT.SetValue(PERCENT);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        this.RETURN.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)RETURN.Value;
                    }
                }
            }

            public static SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def SP_MID_CHAIN_SET_PCT_SET_WK_DELETE = new SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def();
            public class SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def : baseStoredProcedure
            {
                private intParameter NODE_RID;
                private intParameter YEAR_WEEK;
                private intParameter RETURN; //Declare Output Parameter

                public SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def()
                {
                    base.procedureName = "SP_MID_CHAIN_SET_PCT_SET_WK_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("CHAIN_SET_PERCENT_SET");
                    NODE_RID = new intParameter("@NODE_RID", base.inputParameterList);
                    YEAR_WEEK = new intParameter("@YEAR_WEEK", base.inputParameterList);
                    RETURN = new intParameter("@RETURN", base.outputParameterList); //Add Output Parameter
                }

                public int Delete(DatabaseAccess _dba, 
                                  int? NODE_RID, 
                                  int? YEAR_WEEK
                                  )

                {
                    lock (typeof(SP_MID_CHAIN_SET_PCT_SET_WK_DELETE_def))
                    {
                        this.NODE_RID.SetValue(NODE_RID);
                        this.YEAR_WEEK.SetValue(YEAR_WEEK);
                        this.RETURN.SetValue(0); //Initialize Output Parameter
                        ExecuteStoredProcedureForDelete(_dba);
                        return (int)RETURN.Value;
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
