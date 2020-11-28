using System;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class ChainSetPercentCriteriaData : DataLayer
    {
        public ChainSetPercentCriteriaData() : base()
        {
            
        }

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public int ChainSetPercentCriteria_Insert(int aYearWeek, int aNodeRId, string aStoreAttribute, string aStoreAttributeSet, decimal aPercent)
        public int ChainSetPercentCriteria_Insert(int aYearWeek, int aNodeRId, string aStoreAttribute, string aStoreAttributeSet, decimal aPercent, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        {  
            try
            {
               return StoredProcedures.SP_MID_CHAIN_SET_PERCENT_SET_INSERT.UpdateWithReturnCode(_dba,
                                                                                                 YEAR_WEEK: aYearWeek,
                                                                                                 HN_RID: aNodeRId,
                                                                                                 STORE_ATTRIBUTE: aStoreAttribute,
                                                                                                 STORE_ATTRIBUTE_SET: aStoreAttributeSet,
                                                                                                 PERCENT: aPercent,
                                                                                                 SG_VERSION: sg_Version  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                                                                                 );
            }
            catch
            {
                throw;
            }
        }

        public int ChainSetPercentCriteria_Delete(int aNodeRId, int aYearWeek)
        {
            try
            {
                return StoredProcedures.SP_MID_CHAIN_SET_PCT_SET_WK_DELETE.Delete(_dba,
                                                                           NODE_RID: aNodeRId,
                                                                           YEAR_WEEK: aYearWeek
                                                                           );
                //return (int)StoredProcedures.SP_MID_CHAIN_SET_PCT_SET_WK_DELETE.RETURN.Value;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

    }
}
