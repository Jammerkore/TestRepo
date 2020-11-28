using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class StockMinMax: DataLayer
	{
		private int	_methodRid;
		private int _sgl_rid;
		private int _hn_rid;
		private int _boundary;
		private int _cdr_rid;
		private int _minStock;
		private int _maxStock;
		
		public int MethodRid
		{
			get { return _methodRid; }
			set { _methodRid = value; }
		}
		public int StoreGroupLevelRid 
		{
			get { return _sgl_rid ; }
			set { _sgl_rid = value; }
		}
		public int HN_RID
		{
			get { return _hn_rid; }
			set { _hn_rid = value; }
		}
		public int Boundary
		{
			get { return _boundary; }
			set { _boundary = value; }
		}
		public int DateRangeRid
		{
			get { return _cdr_rid; }
			set { _cdr_rid = value; }
		} 
		public int MinimumStock
		{
			get { return _minStock; }
			set { _minStock = value; }
		} 
		public int MaximumStock
		{
			get { return _maxStock; }
			set { _maxStock = value; }
		} 
		
		public StockMinMax(): base()
		{
            
		}

		public bool DeleteStockMinMax(int method_RID, int sgl_RID, int boundary, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.MID_STOCK_MIN_MAX_DELETE.Delete(td.DBA,
                                                                 METHOD_RID: method_RID,
                                                                 SGL_RID: sgl_RID,
                                                                 BOUNDARY: boundary
                                                                 );

				DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			return DeleteSuccessfull;
		}

		public bool DeleteStockMinMax(int method_RID, int sgl_RID, int hn_RID, bool isHighLevel, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                if (isHighLevel)
                {
                    StoredProcedures.MID_STOCK_MIN_MAX_DELETE_FOR_HIGH_LEVEL.Delete(td.DBA,
                                                                                    METHOD_RID: method_RID,
                                                                                    SGL_RID: sgl_RID,
                                                                                    HN_RID: hn_RID
                                                                                    );
                }
                else
                {
                    StoredProcedures.MID_STOCK_MIN_MAX_DELETE_FROM_NODE.Delete(td.DBA,
                                                                               METHOD_RID: method_RID,
                                                                               SGL_RID: sgl_RID,
                                                                               HN_RID: hn_RID
                                                                               );
                }

				DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			return DeleteSuccessfull;
		}

		/// <summary>
		/// Save Stock Min Max based on METHOD_RID, SGL_RID, HN_RID, and BOUNDARY
		/// </summary>
		/// <param name="dba">DatabaseAccess connection</param>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool InsertStockMinMax(TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{

                int? MIN_STOCK_Nullable = null;
                if (_minStock != -1) MIN_STOCK_Nullable = _minStock;

                int? MAX_STOCK_Nullable = null;
                if (_maxStock != -1) MAX_STOCK_Nullable = _maxStock;

                StoredProcedures.MID_STOCK_MIN_MAX_INSERT.Insert(td.DBA,
                                                                 METHOD_RID: _methodRid,
                                                                 SGL_RID: _sgl_rid,
                                                                 BOUNDARY: _boundary,
                                                                 HN_RID: _hn_rid,
                                                                 CDR_RID: _cdr_rid,
                                                                 MIN_STOCK: MIN_STOCK_Nullable,
                                                                 MAX_STOCK: MAX_STOCK_Nullable
                                                                 );

				InsertSuccessfull = true;
			}
			catch
			{
				InsertSuccessfull = false;
				throw;
			}
			return InsertSuccessfull;
		}

		/// <summary>
		/// Get Stock Min Max DataTable based on METHOD_RID  
		/// </summary>
		/// <param name="method_RID">method_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetStockMinMax(int method_RID)
		{
			try
			{	
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STOCK_MIN_MAX_READ.Read(_dba, METHOD_RID: method_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Get Trend_Caps DataTable based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sgl_RID"></param>
		/// <returns>DataTable</returns>
		public DataTable GetStockMinMax(int method_RID, int sgl_RID)
		{
			try
			{	
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL.Read(_dba,
                                                                                           METHOD_RID: method_RID,
                                                                                           SGL_RID: sgl_RID
                                                                                           );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetStockMinMax(int method_RID, int sgl_RID, int hn_RID)
		{
			try
			{	
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STOCK_MIN_MAX_READ_FROM_NODE.Read(_dba,
                                                                              METHOD_RID: method_RID,
                                                                              SGL_RID: sgl_RID,
                                                                              HN_RID: hn_RID
                                                                              );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}

}