using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class BatchComp : DataLayer
	{
        public BatchComp()
            : base()
		{
        }

		public DataTable BatchCompSelectionFilter_Read()
		{
			try
			{
                return StoredProcedures.MID_BATCH_COMP_SELECTION_FILTER_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable BatchCompSelectionFilter_Read(int batchCompRid)
		{
			try
			{
                return StoredProcedures.MID_BATCH_COMP_SELECTION_FILTER_READ.Read(_dba, BATCH_COMP_RID: batchCompRid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
