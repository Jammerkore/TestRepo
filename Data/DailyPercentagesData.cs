using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class DailyPercentagesCriteriaData : DataLayer
    {
        public DailyPercentagesCriteriaData() : base()
        {

        }

       
        public int DailyPercentagesCriteria_DeleteSP(int aCommitLimit)
        {
            int recordsDeleted;
            int totalDeleted = 0;
            try
            {
                
                recordsDeleted = aCommitLimit + 1;
                while (recordsDeleted >= aCommitLimit)
                {
                    recordsDeleted = StoredProcedures.SP_MID_DAILY_PERCENTAGES_WK_DELETE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                    _dba.CommitData();
                    totalDeleted += recordsDeleted;
                }
                return totalDeleted;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
  
    }
}
