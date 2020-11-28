using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class AssortmentDetailData : DataLayer
	{
		private XMLDocument _assrtWriteXML = null;
		private StringBuilder _assrtXML;
        //private bool _endTagAdded;
        //private bool _writeValues;

		public AssortmentDetailData() : base()
		{
            
		}

		public void Variable_XMLInit()
		{
			try
			{
				_assrtWriteXML = new XMLDocument();
				_assrtXML = new StringBuilder();
				_assrtXML.Append("<root> ");
                //_endTagAdded = false;
                //_writeValues = false;
			}
			catch
			{
				throw;
			}
		}

        //public DataTable AssortmentMatrixDetail_Read(ArrayList aAssortmentRIDList)
        //{
        //    string SQLCommand;

        //    try
        //    {
        //        SQLCommand = "SELECT" +
        //            @" COALESCE(hadt.HDR_PACK_RID, " + int.MaxValue + ") HDR_PACK_RID," +
        //            @" COALESCE(hadt.COLOR_CODE_RID, " + int.MaxValue + ") COLOR_CODE_RID," +
        //            @" hadt.HDR_RID, hadt.SGL_RID, hadt.GRADE, hadt.UNITS, hadt.LOCKED, hadt.AVERAGE_UNITS" +
        //            @" FROM ASSORTMENT_MATRIX_DETAIL hadt, HEADER ht " +
        //            @" WHERE ht.DISPLAY_TYPE <> " + Convert.ToString((int)eHeaderType.Placeholder, CultureInfo.CurrentUICulture) +
        //            @" AND " + BuildAssortmentList(aAssortmentRIDList, "ht.") +
        //            @" AND hadt.HDR_RID = ht.HDR_RID";

        //        return _dba.ExecuteSQLQuery(SQLCommand, "AssortmentMatrixDetail");
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public DataTable AssortmentMatrixDetailPlaceholders_Read(ArrayList aAssortmentRIDList)
        //{
        //    string SQLCommand;

        //    try
        //    {
        //        SQLCommand = "SELECT" +
        //            @" COALESCE(hadt.HDR_PACK_RID, " + int.MaxValue + ") HDR_PACK_RID," +
        //            @" COALESCE(hadt.COLOR_CODE_RID, " + int.MaxValue + ") COLOR_CODE_RID," +
        //            @" hadt.HDR_RID, COALESCE(hadt.SGL_RID, " + Include.NoRID + ") SGL_RID, COALESCE(hadt.GRADE, " + Include.NoRID + ") GRADE, hadt.UNITS, hadt.LOCKED, hadt.AVERAGE_UNITS" +
        //            @" FROM ASSORTMENT_MATRIX_DETAIL hadt, HEADER ht " +
        //            @" WHERE ht.DISPLAY_TYPE = " + Convert.ToString((int)eHeaderType.Placeholder, CultureInfo.CurrentUICulture) +
        //            @" AND " + BuildAssortmentList(aAssortmentRIDList, "ht.") +
        //            @" AND hadt.HDR_RID = ht.HDR_RID";

        //        return _dba.ExecuteSQLQuery(SQLCommand, "AssortmentMatrixDetail");

        //        DataTable dtAssortmentList = new DataTable();
        //        dtAssortmentList.Columns.Add("HDR_RID", typeof(int));
        //        for (int i = 0; i < aAssortmentRIDList.Count; i++)
        //        {
        //            //ensure styleHNRids are distinct, and only added to the datatable one time
        //            if (dtAssortmentList.Select("HDR_RID=" + aAssortmentRIDList[i].ToString()).Length == 0)
        //            {
        //                DataRow dr = dtAssortmentList.NewRow();
        //                dr["HDR_RID"] = aAssortmentRIDList[i];
        //                dtAssortmentList.Rows.Add(dr);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        // Begin TT#2 - RMatelic - Assortment Planning-ASSORTMENT_MATRIX_DETAIL database rows not deleted when header removed from assortment
        public void AssortmentMatrixDetail_Delete(ArrayList aHeaderRIDList)
        { 
            try
            {
                DataTable dtHeaderList = new DataTable();
                dtHeaderList.Columns.Add("HDR_RID", typeof(int));
                if (aHeaderRIDList.Count == 0)
                {
                    DataRow dr = dtHeaderList.NewRow();
                    dr["HDR_RID"] = -1;
                    dtHeaderList.Rows.Add(dr);
                }
                else
                {
                    for (int i = 0; i < aHeaderRIDList.Count; i++)
                    {
                        //ensure styleHNRids are distinct, and only added to the datatable one time
                        if (dtHeaderList.Select("HDR_RID=" + aHeaderRIDList[i].ToString()).Length == 0)
                        {
                            DataRow dr = dtHeaderList.NewRow();
                            dr["HDR_RID"] = aHeaderRIDList[i];
                            dtHeaderList.Rows.Add(dr);
                        }
                    }
                }

                StoredProcedures.MID_ASSORTMENT_MATRIX_DETAIL_DELETE.Delete(_dba, HDR_RID_LIST: dtHeaderList);

            }
            catch
            {
                throw;
            }
        }
        // End TT#2

        //BEGIN TT846-MD-DOConnell-New Stored Procedures for Performance - this code was never called
        //public void AssortmentStoreDetail_Delete(int headerRid)
        //{
        //    string SQLCommand;

        //    try
        //    {
        //        SQLCommand = "DELETE FROM ASSORTMENT_STORE_DETAIL WHERE HDR_RID = " + headerRid.ToString(CultureInfo.CurrentUICulture);
        //        _dba.ExecuteNonQuery(SQLCommand);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //END TT846-MD-DOConnell-New Stored Procedures for Performance - this code was never called

		// BEGIN TT#2 - stodd - assortment
		public DataTable AssortmentStoreEligibility_Read(int headerRid)
		{
			try
			{
                return StoredProcedures.MID_ASSORTMENT_STORE_ELIGIBILITY_READ.Read(_dba, HDR_RID: headerRid);
			}
			catch
			{
				throw;
			}
		}

		public bool AssortmentStoreEligibility_Insert(
			int headerRid,
			int stRid,
			bool eligible)
		{
			try
			{
				char cEligible = Include.ConvertBoolToChar(eligible);
        
                int rowsInserted = StoredProcedures.MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT.Insert(_dba, 
				                                                                                   HDR_RID: headerRid,
				                                                                                   ST_RID: stRid,
				                                                                                   ELIGIBLE: cEligible
				                                                                                   );
                if (rowsInserted > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
			}
			catch
			{
				throw;
			}
		}

		public DataTable AssortmentStoreEligibility_Delete(int headerRid)
		{
			try
			{
                DataTable dt = new DataTable();
                dt.Columns.Add("ROWCOUNT", typeof(int));
                DataRow dr = dt.NewRow();
                dr["ROWCOUNT"] = StoredProcedures.MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE.Delete(_dba, HDR_RID: headerRid); //TT#1276-MD -jsobek -Argument Exception trying to create a Group Allocation
                dt.Rows.Add(dr);
                return dt;
			}
			catch
			{
				throw;
			}
		}
		
		// END TT#2 - stodd - assortment



		public void AssortmentStyleClosed_Delete(ArrayList aPlaceholderRIDList)
		{
            //string SQLCommand;

			try
			{
                DataTable dtHeaderList = new DataTable();
                dtHeaderList.Columns.Add("HDR_RID", typeof(int));
                if (aPlaceholderRIDList.Count == 0)
                {
                    DataRow dr = dtHeaderList.NewRow();
                    dr["HDR_RID"] = -1;
                    dtHeaderList.Rows.Add(dr);
                }
                else
                {
                    for (int i = 0; i < aPlaceholderRIDList.Count; i++)
                    {
                        //ensure HDR_RIDs are distinct, and only added to the datatable one time
                        if (dtHeaderList.Select("HDR_RID=" + aPlaceholderRIDList[i].ToString()).Length == 0)
                        {
                            DataRow dr = dtHeaderList.NewRow();
                            dr["HDR_RID"] = aPlaceholderRIDList[i];
                            dtHeaderList.Rows.Add(dr);
                        }
                    }
                }

                StoredProcedures.MID_ASSORTMENT_STYLE_CLOSED_DELETE.Delete(_dba, HDR_RID_LIST: dtHeaderList);
			}
			catch
			{
				throw;
			}
		}

		public DataTable AssortmentStyleClosed_Read(ArrayList aPlaceholderRIDList)
		{
			try
			{
                DataTable dtHeaderList = new DataTable();
                dtHeaderList.Columns.Add("HDR_RID", typeof(int));
                if (aPlaceholderRIDList.Count == 0)
                {
                    DataRow dr = dtHeaderList.NewRow();
                    dr["HDR_RID"] = -1;
                    dtHeaderList.Rows.Add(dr);
                }
                else
                {
                    for (int i = 0; i < aPlaceholderRIDList.Count; i++)
                    {
                        //ensure HDR_RIDs are distinct, and only added to the datatable one time
                        if (dtHeaderList.Select("HDR_RID=" + aPlaceholderRIDList[i].ToString()).Length == 0)
                        {
                            DataRow dr = dtHeaderList.NewRow();
                            dr["HDR_RID"] = aPlaceholderRIDList[i];
                            dtHeaderList.Rows.Add(dr);
                        }
                    }
                }

                return StoredProcedures.MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST.Read(_dba, HDR_RID_LIST: dtHeaderList);
			}
			catch
			{
				throw;
			}
		}

		public void AssortmentStyleClosed_InsertClosed(int aPlaceholderRID, int aStrGrpLvlRID, int aGradeRID)
		{
			try
			{
                StoredProcedures.MID_ASSORTMENT_STYLE_CLOSED_INSERT.Insert(_dba,
                                                                           HDR_RID: aPlaceholderRID,
                                                                           SGL_RID: aStrGrpLvlRID,
                                                                           GRADE: aGradeRID,
                                                                           CLOSED: '1'
                                                                           );
			}
			catch
			{
				throw;
			}
		}

		public void AssortmentStyleClosed_Copy(Hashtable placeholderHash)
		{
			try
			{

				ArrayList fromList = new ArrayList();

				foreach (int rid in placeholderHash.Keys)
				{
					fromList.Add(rid);
				}
				DataTable dtFrom = AssortmentStyleClosed_Read(fromList);

				foreach (DataRow aRow in dtFrom.Rows)
				{
					int sglRid = int.Parse(aRow["SGL_RID"].ToString());
					int grade = int.Parse(aRow["GRADE"].ToString());
					int oldPlaceholder = int.Parse(aRow["HDR_RID"].ToString());
					int newPlaceholder = (int)placeholderHash[oldPlaceholder];
					AssortmentStyleClosed_InsertClosed(newPlaceholder, sglRid, grade);
				}
			}
			catch
			{
				throw;
			}
		}

		public void AssortmentMatrixDetail_XMLInsert(int HDR_RID, int HDR_PACK_RID, int COLOR_CODE_RID, int SGL_RID, int GRADE, double UNITS, bool LOCKED)
		{
			try
			{
				_assrtWriteXML.XMLDoc.Append(" <node HDR_RID=\"");
				_assrtWriteXML.XMLDoc.Append(HDR_RID.ToString());
				_assrtWriteXML.XMLDoc.Append("\"");

				if (HDR_PACK_RID != int.MaxValue)
				{
					_assrtWriteXML.XMLDoc.Append(" HDR_PACK_RID=\"");
					_assrtWriteXML.XMLDoc.Append(HDR_PACK_RID.ToString());
					_assrtWriteXML.XMLDoc.Append("\"");
				}

				if (COLOR_CODE_RID != int.MaxValue)
				{
					_assrtWriteXML.XMLDoc.Append(" COLOR_CODE_RID=\"");
					_assrtWriteXML.XMLDoc.Append(COLOR_CODE_RID.ToString());
					_assrtWriteXML.XMLDoc.Append("\"");
				}

				_assrtWriteXML.XMLDoc.Append(" SGL_RID=\"");
				_assrtWriteXML.XMLDoc.Append(SGL_RID.ToString());
				_assrtWriteXML.XMLDoc.Append("\" GRADE=\"");
				_assrtWriteXML.XMLDoc.Append(GRADE.ToString());
				_assrtWriteXML.XMLDoc.Append("\" AVERAGE_UNITS=\"");
				_assrtWriteXML.XMLDoc.Append(UNITS.ToString());
				_assrtWriteXML.XMLDoc.Append("\" LOCKED=\"");
				if (LOCKED)
				{
					_assrtWriteXML.XMLDoc.Append("1");
				}
				else
				{
					_assrtWriteXML.XMLDoc.Append("0");
				}
				_assrtWriteXML.XMLDoc.Append("\"> ");
				_assrtWriteXML.XMLDoc.Append(" </node>");
				_assrtWriteXML.RowsAdded++;
			}
			catch
			{
				throw;
			}
		}

		public void AssortmentMatrixDetail_XMLUpdate()
		{
			try
			{
				if (_assrtWriteXML.RowsAdded > 0)
				{
					if (!_assrtWriteXML.EndTagAdded)
					{
						_assrtWriteXML.EndTagAdded = true;
						_assrtWriteXML.XMLDoc.Append(" </root>");
					}

                    StoredProcedures.SP_MID_XML_ASSRT_MATRIX_WRITE.Insert(_dba, XMLDOC: _assrtWriteXML.XMLDoc.ToString());
					_assrtWriteXML.ValuesWritten = true;
				}
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT846-MD-DOConnell-New Stored Procedures for Performance
        //private string BuildAssortmentList(ArrayList aAssortmentRIDList, string aFileID)
        //{
        //    string SQLCommand = "";
        //    int assrts;

        //    try
        //    {
        //        //if (aAssortmentRIDList.Count == 1)
        //        {
        //            SQLCommand += aFileID + "ASRT_RID = " + Convert.ToString(aAssortmentRIDList[0], CultureInfo.CurrentUICulture);
        //        }
        //        else
        //        {
        //            SQLCommand += aFileID + "ASRT_RID IN (";

        //            assrts = 0;
        //            foreach (int assrtRID in aAssortmentRIDList)
        //            {
        //                if (assrts > 0)
        //                {
        //                    SQLCommand += ",";
        //                }

        //                SQLCommand += Convert.ToString(assrtRID, CultureInfo.CurrentUICulture);
        //                assrts++;
        //            }
        //            SQLCommand += ")";
        //        }

        //        return SQLCommand;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private string BuildPlaceholderList(ArrayList aPlaceholderRIDList, string aFileID)
        //{
        //    string SQLCommand = "";
        //    int assrts;

        //    try
        //    {
        //        if (aPlaceholderRIDList.Count == 0)
        //        {
        //            SQLCommand += aFileID + "HDR_RID = -1";
        //        }
        //        else if (aPlaceholderRIDList.Count == 1)
        //        {
        //            SQLCommand += aFileID + "HDR_RID = " + Convert.ToString(aPlaceholderRIDList[0], CultureInfo.CurrentUICulture);
        //        }
        //        else
        //        {
        //            SQLCommand += aFileID + "HDR_RID IN (";

        //            assrts = 0;
        //            foreach (int assrtRID in aPlaceholderRIDList)
        //            {
        //                if (assrts > 0)
        //                {
        //                    SQLCommand += ",";
        //                }

        //                SQLCommand += Convert.ToString(assrtRID, CultureInfo.CurrentUICulture);
        //                assrts++;
        //            }
        //            SQLCommand += ")";
        //        }

        //        return SQLCommand;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //END TT846-MD-DOConnell-New Stored Procedures for Performance - this code was never called
	}
}
