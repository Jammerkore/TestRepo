using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class HeaderAllocationLoadData : DataLayer
	{
		public HeaderAllocationLoadData()
			: base()
		{

		}

   
		/// <summary>
		/// Reads pack allocation records for a specific header.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="headerId"></param>
		/// <returns></returns>
		public DataTable PackAlloc_Read(string tableName, string headerId)
		{
			try
			{
				string SQLCommand = "select ap.*, s.ST_RID from " + tableName + " ap" +
					" LEFT OUTER JOIN STORES s ON ap.STORE_ID = s.ST_ID" +
					" where ap.HEADER_ID = '" + headerId + "' order by HEADER_ID, PACK_ID, STORE_ID";
				DataTable dt = _dba.ExecuteSQLQuery(SQLCommand, "Alloc Pack");
				//dt.PrimaryKey = new DataColumn[] { dt.Columns["HEADER_ID"] };
				return dt;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

     
		/// <summary>
		/// Reads bulk allocation records for a specific header.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="headerId"></param>
		/// <returns></returns>
		public DataTable BulkAlloc_Read(string tableName, string headerId)
		{
			try
			{
				string SQLCommand = "select ab.*, s.ST_RID, sc.SIZE_CODE_RID from " + tableName + " ab" +
					" LEFT OUTER JOIN SIZE_CODE sc ON ab.SIZE_CODE = sc.SIZE_CODE_ID" +
					" LEFT OUTER JOIN STORES s ON ab.STORE_ID = s.ST_ID" + 
					" where ab.HEADER_ID = '" + headerId + "' order by HEADER_ID, SIZE_CODE, STORE_ID";
				DataTable dt = _dba.ExecuteSQLQuery(SQLCommand, "Alloc Bulk");
				//dt.PrimaryKey = new DataColumn[] { dt.Columns["HEADER_ID"] };
				return dt;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Read all bulk allocation records
		/// </summary>
		/// <param name="bulkTableName"></param>
		/// <param name="packTableName"></param>
		/// <returns></returns>
		public DataTable DistinctHeader_Read(string bulkTableName, string packTableName)
		{
			try
			{
				string SQLCommand = "select distinct HEADER_ID from " + bulkTableName + " UNION select HEADER_ID from " + packTableName;

				return _dba.ExecuteSQLQuery(SQLCommand, "Distinct Header");
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
