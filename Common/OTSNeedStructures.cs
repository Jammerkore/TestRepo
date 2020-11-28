using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Structure describing the necessary parameters to request OTS Need for a store
	/// </summary>
	public struct OTS_NeedRequestKey
	{
		private int _hnRID;
		private int _verRID;
		private WeekProfile _beginWeek;
		private WeekProfile _endWeek;
		private long _needHorizonKey;

		/// <summary>
		/// Creates an instance of the structure key to request OTS Need for a store
		/// </summary>
		/// <param name="aHnRID">Hierarchy node RID for which need is to be calculated</param>
		/// <param name="aVerRID">Plan version RID for which need is to be calculated</param>
		/// <param name="aBeginWeek">First week of the planning horizon.  </param>
		/// <param name="aEndWeek">Last week of the planning horizon (if the horizon is a "period" horizon, then this week should be the BOW of the last period).</param>
		public OTS_NeedRequestKey(int aHnRID, int aVerRID, WeekProfile aBeginWeek, WeekProfile aEndWeek)
		{
			_hnRID = aHnRID;
			_verRID = aVerRID;
			_beginWeek = aBeginWeek;
			_endWeek = aEndWeek;
			_needHorizonKey = ((long)_beginWeek.YearWeek << 32) + _endWeek.YearWeek;
		}
		/// <summary>
		/// Gets the Hierarchy RID for which need is required 
		/// </summary>
		public int HnRID
		{
			get { return _hnRID; }
		}
		/// <summary>
		/// Gets the Version RID for which need is required
		/// </summary>
		public int VersionRID
		{
			get { return _verRID; }
		}
		/// <summary>
		/// Gets the Begin Week Profile for which need is required
		/// </summary>
		public WeekProfile BeginWeek
		{
			get { return _beginWeek; }
		}
		/// <summary>
		/// Gets the End Week Profile for which need is required
		/// </summary>
		public WeekProfile EndWeek
		{
			get { return _endWeek; }
		}
		/// <summary>
		/// Gets the Plan Horizon Key associated with this Begin and End week
		/// </summary>
		public long NeedHorizonKey
		{
			get 
			{ 
				return _needHorizonKey; 
			}
		}
	}
	/// <summary>
	/// Structure to describe an OTS Need Request for a store
	/// </summary>
	public struct OTS_NeedRequest
	{
		private OTS_NeedRequestKey _needRequestKey;
		private double _endingStock;
		private double _partTotalSalesPlan;
		private double _currentWeekBOW_Plan;
		private double _currentWeekSales_Plan;

        /// <summary>
        /// Creates an instance of the structure for an OTS Need Request
        /// </summary>
        /// <param name="aOTS_NeedRequestKey">OTS Need Request Key Structure</param>
		/// <param name="aEndingStock">BOW stock plan for aEndWeek</param>
		/// <param name="aPartTotalSalesPlan"></param>IF current selling week is in planning horizon, this is total weekly sales plan from current selling week plus 1 through aEndWeek -1; otherwise, this is the total sales plan from aBeginWeek + 1 through aEndWeek - 1  
		/// <param name="aCurrWeekBOW_Plan">BOW stock plan for the current selling week (ignored when current selling week is not in planning horizon)</param>
		/// <param name="aCurrWeekSalesPlan">IF current selling week is in planning horizon, this is the Sales plan for the current selling week; otherwise, it is the Sales plan for aBeginWeek</param>
		public OTS_NeedRequest 
			(OTS_NeedRequestKey aOTS_NeedRequestKey, 
			double aEndingStock, 
			double aPartTotalSalesPlan, 
			double aCurrWeekBOW_Plan, 
			double aCurrWeekSales_Plan)
		{
			_needRequestKey = aOTS_NeedRequestKey;
			_endingStock = aEndingStock;
			_partTotalSalesPlan = aPartTotalSalesPlan;
			_currentWeekBOW_Plan = aCurrWeekBOW_Plan;
			_currentWeekSales_Plan = aCurrWeekSales_Plan;
		}
		/// <summary>
		/// Creates an instance of the structure for an OTS Need Request
		/// </summary>
		/// <param name="aHnRID">Hierarchy node RID for which need is to be calculated</param>
		/// <param name="aVerRID">Plan version RID for which need is to be calculated</param>
		/// <param name="aBeginWeek">First week of the planning horizon.  </param>
		/// <param name="aEndWeek">Last week of the planning horizon (if the horizon is a "period" horizon, then this week should be the BOW of the last period).</param>
		/// <param name="aEndingStock">BOW stock plan for aEndWeek</param>
		/// <param name="aPartTotalSalesPlan"></param>IF current selling week is in planning horizon, this is total weekly sales plan from current selling week plus 1 through aEndWeek -1; otherwise, this is the total sales plan from aBeginWeek + 1 through aEndWeek - 1  
		/// <param name="aCurrWeekBOW_Plan">BOW stock plan for the current selling week (ignored when current selling week is not in planning horizon)</param>
		/// <param name="aCurrWeekSalesPlan">IF current selling week is in planning horizon, this is the Sales plan for the current selling week; otherwise, it is the Sales plan for aBeginWeek</param>
		public OTS_NeedRequest
			(int aHnRID, 
			int aVerRID, 
			WeekProfile aBeginWeek, 
			WeekProfile aEndWeek,
			double aEndingStock,
			double aPartTotalSalesPlan,
			double aCurrWeekBOW_Plan,
			double aCurrWeekSales_Plan)
		{
			_needRequestKey = new OTS_NeedRequestKey(aHnRID, aVerRID, aBeginWeek, aEndWeek);
			_endingStock = aEndingStock;
			_partTotalSalesPlan = aPartTotalSalesPlan;
			_currentWeekBOW_Plan = aCurrWeekBOW_Plan;
			_currentWeekSales_Plan = aCurrWeekSales_Plan;
		}

		/// <summary>
		/// Gets the OTS Need Request Key
		/// </summary>
		public OTS_NeedRequestKey OTS_NeedRequestKey 
		{
			get { return _needRequestKey; }
		}
		/// <summary>
		/// Gets the OTS Need Request HnRID part of the key
		/// </summary>
		public int HnRID
		{
			get { return _needRequestKey.HnRID; }
		}
		/// <summary>
		/// Gets the OTS Need Request Version of the key
		/// </summary>
		public int VersionRID
		{
			get { return _needRequestKey.VersionRID; }
		}
		/// <summary>
		/// Gets the OTS Need Request Begin Week Profile from the key
		/// </summary>
		public WeekProfile BeginWeek
		{
			get { return _needRequestKey.BeginWeek; }
		}
		/// <summary>
		/// Gets the OTS Need Request End Week Profile from the key
		/// </summary>
		public WeekProfile EndWeek
		{
			get { return _needRequestKey.EndWeek; }
		}
		/// <summary>
		/// Gets the OTS Need Request Ending Stock
		/// </summary>
		public double EndingStock
		{
			get { return _endingStock; }
		}
		/// <summary>
		/// Gets the OTS Need Request Part Total Sales Plan. IF current selling week is in planning horizon, this is total weekly sales plan from current selling week plus 1 through aEndWeek -1; otherwise, this is the total sales plan from aBeginWeek + 1 through aEndWeek - 1 
		/// </summary>
		public double PartTotalSalesPlan
		{
			get { return _partTotalSalesPlan; }
		}
		/// <summary>
		/// Gets the OTS Need Request Current Week BOW Plan. BOW stock plan for the current selling week (ignored when current selling week is not in planning horizon).  This is used for "future" need.
		/// </summary>
		public double CurrentWeekBOW_Plan
		{
			get { return _currentWeekBOW_Plan; }
		}
		/// <summary>
		/// Gets the OTS Need Request Current Week Sales Plan.  IF current selling week is in planning horizon, this is the Sales plan for the current selling week; otherwise, it is the Sales plan for aBeginWeek
		/// </summary>
		public double CurrentWeekSales_Plan
		{
			get { return _currentWeekSales_Plan; }
		}
	}
	/// <summary>
	/// Structure to describe a Percent Need Request for a store.
	/// </summary>
	public struct OTS_PercentNeedRequest
	{
		private OTS_NeedRequest _OTS_NeedRequest;
		private double _unitNeed;
		/// <summary>
		/// Creates the structure that describes a percent need request for a store.
		/// </summary>
		/// <param name="aOTS_NeedRequest">An OTS_NeedRequest that is used to calculate the unit need parameter of this request.</param>
		/// <param name="aUnitNeed">Unit Need calculated using the OTS_NeedRequest parameter.</param>
		OTS_PercentNeedRequest(
			OTS_NeedRequest aOTS_NeedRequest,
			double aUnitNeed)
		{
			_OTS_NeedRequest = aOTS_NeedRequest;
			_unitNeed = aUnitNeed;
		}
		/// <summary>
		/// Gets the OTS_NeedRequest Structure that describes the request
		/// </summary>
		public OTS_NeedRequest OTS_NeedRequest
		{
			get { return _OTS_NeedRequest; }
		}
		/// <summary>
		/// Gets the Unit Need calculated using the OTS_NeedRequest.
		/// </summary>
		public double UnitNeed
		{
			get { return _unitNeed; }
		}
	}
	public struct OTS_NeedHorizon
	{
		private long _otsNeedHorizonKey;
		private WeekProfile _otsBeginWeek;
		private WeekProfile _otsEndWeek;
		private bool _otsBeginWeekIsCurrent;
		public OTS_NeedHorizon(long aOTS_NeedHorizonKey, WeekProfile aBeginWeek, WeekProfile aEndWeek, bool aOTS_BeginWeekIsCurrent)
		{
			_otsNeedHorizonKey = aOTS_NeedHorizonKey;
			_otsBeginWeek = aBeginWeek;
			_otsEndWeek = aEndWeek;
			_otsBeginWeekIsCurrent = aOTS_BeginWeekIsCurrent;
		}
		public long OTS_NeedHorizonKey
		{
			get { return _otsNeedHorizonKey; }
		}
		public WeekProfile OTS_BeginWeek
		{
			get { return _otsBeginWeek; }
		}
		public WeekProfile OTS_EndWeek
		{
			get { return _otsEndWeek; }
		}
		public bool OTS_BeginWeekIsCurrent
		{
			get { return _otsBeginWeekIsCurrent; }
		}
	}
}
