using System;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class BuildPackCriteriaData : DataLayer
	{
		public BuildPackCriteriaData() : base()
		{
           
        }

		public void BuildPackCriteria_Insert(string aCriteriaName, int aCompMinQuant, int aSizeMult, int aPackMult, string aComboName, int aComboNumPacks)
		{
			try
			{
                StoredProcedures.SP_MID_BLD_PCK_CRIT_INS.Insert(_dba,
                                                                BPC_NAME: aCriteriaName,
                                                                BPC_COMP_MIN: aCompMinQuant,
                                                                BPC_SIZE_MULT: aSizeMult,
                                                                BPC_PACK_MULT: aPackMult,
                                                                BPC_COMBO_NAME: aComboName,
                                                                BPC_COMBO_MAX_PACKS: aComboNumPacks
                                                                );
			}
			catch
			{
				throw;
			}
		}
	}
}
