using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class BuildPackCriteriaData : DataLayer
    {
        protected static class StoredProcedures
        {
            public static SP_MID_BLD_PCK_CRIT_INS_def SP_MID_BLD_PCK_CRIT_INS = new SP_MID_BLD_PCK_CRIT_INS_def();
            public class SP_MID_BLD_PCK_CRIT_INS_def : baseStoredProcedure
            {
                private stringParameter BPC_NAME;
                private intParameter BPC_COMP_MIN;
                private intParameter BPC_SIZE_MULT;
                private intParameter BPC_PACK_MULT;
                private stringParameter BPC_COMBO_NAME;
                private intParameter BPC_COMBO_MAX_PACKS;

                public SP_MID_BLD_PCK_CRIT_INS_def()
                {
                    base.procedureName = "SP_MID_BLD_PCK_CRIT_INS";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("BUILD_PACK_CONFIG");
                    BPC_NAME = new stringParameter("@BPC_NAME", base.inputParameterList);
                    BPC_COMP_MIN = new intParameter("@BPC_COMP_MIN", base.inputParameterList);
                    BPC_SIZE_MULT = new intParameter("@BPC_SIZE_MULT", base.inputParameterList);
                    BPC_PACK_MULT = new intParameter("@BPC_PACK_MULT", base.inputParameterList);
                    BPC_COMBO_NAME = new stringParameter("@BPC_COMBO_NAME", base.inputParameterList);
                    BPC_COMBO_MAX_PACKS = new intParameter("@BPC_COMBO_MAX_PACKS", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  string BPC_NAME,
                                  int? BPC_COMP_MIN,
                                  int? BPC_SIZE_MULT,
                                  int? BPC_PACK_MULT,
                                  string BPC_COMBO_NAME,
                                  int? BPC_COMBO_MAX_PACKS
                                  )
                {
                    lock (typeof(SP_MID_BLD_PCK_CRIT_INS_def))
                    {
                        this.BPC_NAME.SetValue(BPC_NAME);
                        this.BPC_COMP_MIN.SetValue(BPC_COMP_MIN);
                        this.BPC_SIZE_MULT.SetValue(BPC_SIZE_MULT);
                        this.BPC_PACK_MULT.SetValue(BPC_PACK_MULT);
                        this.BPC_COMBO_NAME.SetValue(BPC_COMBO_NAME);
                        this.BPC_COMBO_MAX_PACKS.SetValue(BPC_COMBO_MAX_PACKS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
