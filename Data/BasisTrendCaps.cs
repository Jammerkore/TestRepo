using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class TrendCaps: DataLayer
	{
		private int _methodRid;
		private int _sglRid;
		private DataCommon.eTrendCapID _trendCapID;
		private double _tolPct;
		private double _highLimit;
		private double _lowLimit;
		
		public int MethodRid 
		{
			get { return _methodRid ; }
			set { _methodRid = value; }
		}
		public int SglRid 
		{
			get { return _sglRid ; }
			set { _sglRid = value; }
		}
		public eTrendCapID TrendCapID 
		{
			get { return _trendCapID ; }
			set { _trendCapID = value; }
		}
		
		public double TolPct
		{
			get { return _tolPct ; }
			set { _tolPct = value; }
		}

		public double HighLimit
		{
			get { return _highLimit ; }
			set { _highLimit = value; }
		}

		public double LowLimit
		{
			get { return _lowLimit ; }
			set { _lowLimit = value; }
		}
//		public eTyLyType TyLyType 
//		{
//			get { return _tyLyType ; }
//			set { _tyLyType = value; }
//		}

		public TrendCaps() : base()
		{
            
		}

		/// <summary>
		/// Delete Trend_Caps based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="method_RID">method_RID - PK of Group Level Function</param>
		/// <param name="sgl_RID">sgl_RID - PK of Group Level Function</param>
		/// <param name="dba">DatabaseAccess connection</param>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool DeleteTrendCaps(int method_RID, int sgl_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.MID_TREND_CAPS_DELETE.Delete(td.DBA,
                                                              METHOD_RID: method_RID,
                                                              SGL_RID: sgl_RID
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

		/// <summary>
		/// Save Trend Caps based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="dba">DatabaseAccess connection</param>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool InsertTrendCaps(TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{
               

                double? TOL_PCT_Nullable = null;
                if ((eTrendCapID)_trendCapID == eTrendCapID.Tolerance) TOL_PCT_Nullable = _tolPct;

                double? HIGH_LIMIT_Nullable = null;
                if ((eTrendCapID)_trendCapID == eTrendCapID.Limits && _highLimit != Include.UndefinedDouble) HIGH_LIMIT_Nullable = _highLimit;

                double? LOW_LIMIT_Nullable = null;
                if ((eTrendCapID)_trendCapID == eTrendCapID.Limits && _lowLimit != Include.UndefinedDouble) LOW_LIMIT_Nullable = _lowLimit;

                StoredProcedures.MID_TREND_CAPS_INSERT.Insert(td.DBA,
                                                              METHOD_RID: _methodRid,
                                                              SGL_RID: _sglRid,
                                                              TREND_CAP_ID: Convert.ToInt32(_trendCapID, CultureInfo.CurrentUICulture),
                                                              TOL_PCT: TOL_PCT_Nullable,
                                                              HIGH_LIMIT: HIGH_LIMIT_Nullable,
                                                              LOW_LIMIT: LOW_LIMIT_Nullable
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
		/// Get Trend_Caps DataTable based on METHOD_RID  
		/// </summary>
		/// <param name="method_RID">method_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetTrendCaps(int method_RID)
		{
			try
			{																											 
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_TREND_CAPS_READ.Read(_dba, METHOD_RID: method_RID);
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
		public DataTable GetTrendCaps(int method_RID, int sgl_RID)
		{
			try
			{																											 
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL.Read(_dba,
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

	}



}