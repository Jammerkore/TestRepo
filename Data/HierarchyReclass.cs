using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class HierarchyReclassData : DataLayer
	{
        public HierarchyReclassData() : base()
		{
		}

		public void HierReclassTrans_ClearTrans(int aProcessId)
		{
			try
			{
                StoredProcedures.MID_HIERARCHY_RECLASS_TRANS_DELETE.Delete(_dba, TRANS_PROCESS_ID: aProcessId);
			}
			catch
			{
				throw;
			}
		}

		public void HierReclassTrans_Insert(int aProcessId, int aSequence, string aOriginalLine, string aHierId, string aParentId, string aProductId, string aProductName, string aProductDesc)
		{
			try
			{

                string TRANS_PRODUCT_NAME = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aProductName != string.Empty) TRANS_PRODUCT_NAME = aProductName;

                string TRANS_PRODUCT_DESC = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aProductDesc != string.Empty) TRANS_PRODUCT_DESC = aProductDesc;

                StoredProcedures.MID_HIERARCHY_RECLASS_TRANS_INSERT.Insert(_dba,
                                                                           TRANS_PROCESS_ID: aProcessId,
                                                                           TRANS_SEQUENCE: aSequence,
                                                                           TRANS_ORIGINAL: aOriginalLine,
                                                                           TRANS_HIERARCHY_ID: aHierId,
                                                                           TRANS_PARENT_ID: aParentId,
                                                                           TRANS_PRODUCT_ID: aProductId,
                                                                           TRANS_PRODUCT_NAME: TRANS_PRODUCT_NAME,
                                                                           TRANS_PRODUCT_DESC: TRANS_PRODUCT_DESC
                                                                           );
			}
			catch
			{
				throw;
			}
		}

		public DataSet HierReclassTrans_Process(int aProcessId, string aHierId)
		{
			try
			{
                return StoredProcedures.SP_MID_HIER_RECLASS_PROCESS.ReadAsDataSet(_dba,
                                                                                  ProcessId: aProcessId,
                                                                                  HierarchyId: aHierId
                                                                                  );
			}
			catch
			{
			    throw;
			}
		}
	}
}
