using System;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for ForecastStockMinMax.
	/// </summary>
	public class ForecastStockMinMax
	{
		Hashtable _setHash;  // contains ForecastStockMinMaxSet by sglRid
		StockMinMax _stockMinMaxData;
		MRSCalendar _calendar;
		int _methodRid;
		bool _hasDynamicToPlanDates = false;
		bool _built = false;
        OTSPlanMethod _OTSMethod;  // TT#5028 - JSmith - Node Property - Sku Level Min/Max setup

        // Begin TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
        //public ForecastStockMinMax(MRSCalendar aCalendar, int methodRid)
        public ForecastStockMinMax(MRSCalendar aCalendar, int methodRid, OTSPlanMethod aOTSMethod)
        // End TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
		{
			_setHash = new Hashtable();
			_calendar = aCalendar;
			_methodRid = methodRid;
			_stockMinMaxData = new StockMinMax();
            _OTSMethod = aOTSMethod;  // TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
		}

		/// <summary>
		/// forces a rebuild of the object
		/// </summary>
		/// <param name="methodRid"></param>
		/// <param name="planWeek"></param>
        // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
        //public void Rebuild(int methodRid, WeekProfile planWeek, int aNodeRid, ArrayList aGroupLevelFunctionList)
        //{
        //    _setHash.Clear();
        //    Build(methodRid, planWeek, aNodeRid, aGroupLevelFunctionList);
        //}
        // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.

		/// <summary>
		/// Builds StockMinMax object for a particular planWeek.
		/// If the object is already built and no dynamic to plan dates occur,
		/// object remains the same.
		/// </summary>
		/// <param name="planWeek"></param>
        // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
        //public void Build(WeekProfile planWeek, int aNodeRid, ArrayList aGroupLevelFunctionList)
        public void Build(WeekProfile planWeek, int aNodeRid, ArrayList aGroupLevelFunctionList, int aHighLevelRID)
        // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
		{
			// Only if there are dynamic to plan date ranges do we need to rebuild
			if (_hasDynamicToPlanDates || _built == false)
			{
				_setHash.Clear();
                // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                //Build(_methodRid, planWeek, aNodeRid, aGroupLevelFunctionList);
                Build(_methodRid, planWeek, aNodeRid, aGroupLevelFunctionList, aHighLevelRID);
                // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
			}
		}

        // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
        //private void Build(int methodRid, WeekProfile planWeek, int aNodeRid, ArrayList aGroupLevelFunctionList)
        private void Build(int methodRid, WeekProfile planWeek, int aNodeRid, ArrayList aGroupLevelFunctionList, int aHighLevelRID)
        // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
		{

//			DataTable dt = _stockMinMaxData.GetStockMinMax(methodRid);

			int boundary, sglRid;
			int cdrRid;
			int min, max;
			const int INIT_VALUE = -22;
			int currentBoundary = INIT_VALUE;
			int currentSglRid = INIT_VALUE;

			ArrayList theDefaultList = new ArrayList();
			ArrayList aBoundaryList = new ArrayList();
			ArrayList aList = new ArrayList();

			ForecastStockMinMaxSet aMinMaxSet = null;

			foreach (GroupLevelFunctionProfile glf in aGroupLevelFunctionList)
			{
				GroupLevelNodeFunction glfn = (GroupLevelNodeFunction)glf.Group_Level_Nodes[aNodeRid];

                // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                if (glfn == null &&
                    aHighLevelRID != aNodeRid)
                {
                    glfn = (GroupLevelNodeFunction)glf.Group_Level_Nodes[aHighLevelRID];
                    if (glfn != null &&
                        glfn.MinMaxInheritType == eMinMaxInheritType.None)
                    {
                        glfn = null;
                    }
                    // Begin TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
                    else if (glfn.MinMaxInheritType == eMinMaxInheritType.Hierarchy)
                    {
                        glfn.Stock_MinMax.Clear();
                        _OTSMethod.LoadStockMinMaxProfileListFromHierarchy(glfn, aNodeRid);
                    }
                    // End TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
                }
                // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.

				if (glfn != null &&
					glfn.ApplyMinMaxesInd)
				{
					foreach (StockMinMaxProfile smmp in glfn.Stock_MinMax.ArrayList)
					{
						sglRid = glf.Key;
						boundary = smmp.Boundary;
						cdrRid = smmp.DateRangeRid;
						min = smmp.MinimumStock;
						max = smmp.MaximumStock;
						if (sglRid != currentSglRid && currentSglRid != INIT_VALUE)
						{
							aList.Clear();					// Clear List
							aList.AddRange(theDefaultList);	// insert any Default data first
							aList.AddRange(aBoundaryList);	// then insert the boundary info
							ForecastStockMinMaxBoundary aMinMaxBoundary = CreateBoundary(currentBoundary, aList);
							aMinMaxSet.AddBoundary(aMinMaxBoundary);
							_setHash.Add(currentSglRid, aMinMaxSet);
					
							//ForecastStockMinMaxBoundary minMaxBoundary = CreateBoundary(aBoundaryList);
							aBoundaryList.Clear();				// clear all entries for prior grade
							theDefaultList.Clear();	// clear default entries for Set
				
							currentBoundary = boundary;
							currentSglRid = sglRid;
							aMinMaxSet = new ForecastStockMinMaxSet(currentSglRid);

							// build a new minMaxInfo 
							StockMinMaxInfo minMaxInfo = null;
							if (cdrRid == Include.UndefinedCalendarDateRange)
							{
								minMaxInfo = new StockMinMaxInfo(boundary, cdrRid, min, max, null);
							}
							else
							{
								DateRangeProfile drp = _calendar.GetDateRange(cdrRid, planWeek);
								if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
									_hasDynamicToPlanDates = true;
								ProfileList weekList = _calendar.GetDateRangeWeeks(drp, planWeek);
								minMaxInfo = new StockMinMaxInfo(boundary, cdrRid, min, max, weekList);
							}

							// Add it to the right list
							if (boundary == Include.Undefined)
							{
								theDefaultList.Add(minMaxInfo);
							}
							else
							{
								aBoundaryList.Add(minMaxInfo);
							}
						}
						else
						{
							if (boundary != currentBoundary && currentBoundary != INIT_VALUE)
							{
								aList.Clear();					// Clear List
								aList.AddRange(theDefaultList);	// insert any Default data first
								aList.AddRange(aBoundaryList);	// then insert the boundary info
								ForecastStockMinMaxBoundary aMinMaxBoundary = CreateBoundary(currentBoundary, aList);
								aMinMaxSet.AddBoundary(aMinMaxBoundary);
								aBoundaryList.Clear();

								currentBoundary = boundary;
							}
					
							// to catch setting the current values the first time through
							if (currentBoundary == INIT_VALUE)
							{
								currentBoundary = boundary;
								currentSglRid = sglRid;
								aMinMaxSet = new ForecastStockMinMaxSet(currentSglRid);
							}

							StockMinMaxInfo minMaxInfo = null;
							if (cdrRid == Include.UndefinedCalendarDateRange)
							{
								minMaxInfo = new StockMinMaxInfo(boundary, cdrRid, min, max, null);
							}
							else
							{
								DateRangeProfile drp = _calendar.GetDateRange(cdrRid, planWeek);
								if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
									_hasDynamicToPlanDates = true;
								ProfileList weekList = _calendar.GetDateRangeWeeks(drp, planWeek);
								minMaxInfo = new StockMinMaxInfo(boundary, cdrRid, min, max, weekList);
							}

							// Add it to the right list
							if (boundary == Include.Undefined)
							{
								theDefaultList.Add(minMaxInfo);
							}
							else
							{
								aBoundaryList.Add(minMaxInfo);
							}
				
						}
					}
				}
			}

			// catch the last set
			if (aMinMaxSet != null &&
				(theDefaultList.Count > 0 ||
				aBoundaryList.Count > 0 ))
			{
				aList.Clear();					// Clear List
				aList.AddRange(theDefaultList);	// insert any Default data first
				aList.AddRange(aBoundaryList);	// then insert the boundary info
				ForecastStockMinMaxBoundary aMinMaxBoundary = CreateBoundary(currentBoundary, aList);
				aMinMaxSet.AddBoundary(aMinMaxBoundary);
				_setHash.Add(currentSglRid, aMinMaxSet);
			}
					

			_built = true;
		}

		private ForecastStockMinMaxBoundary CreateBoundary(int boundary, ArrayList aBoundaryList)
		{
			ForecastStockMinMaxBoundary newBoundary = new ForecastStockMinMaxBoundary(boundary);

			foreach (StockMinMaxInfo smm in aBoundaryList)
			{
				if (smm.CdrRid == Include.UndefinedCalendarDateRange)  // is this a default record
				{
					if (smm.Boundary == Include.Undefined)  // default record for default grade
					{
						newBoundary.DefaultMinimum = smm.Minimum;
						newBoundary.DefaultMaximum = smm.Maximum;
					}
					else
					{
						if (smm.Minimum != (int)Include.UndefinedDouble)
							newBoundary.DefaultMinimum = smm.Minimum;
						if (smm.Maximum != (int)Include.UndefinedDouble)
							newBoundary.DefaultMaximum = smm.Maximum;
					}
				}	
				else
				{
					foreach (WeekProfile wp in smm.WeekList.ArrayList)
					{
						newBoundary.AddMinimumForWeek(wp, smm.Minimum);
						newBoundary.AddMaximumForWeek(wp, smm.Maximum);
					}
				}
			}

			return newBoundary;
		}

		public int GetMinimumForWeek(WeekProfile aWeek, int sglRid, int aBoundaryKey)
		{
			int min = Include.Undefined;
			ForecastStockMinMaxSet aSet = (ForecastStockMinMaxSet)_setHash[sglRid];
			if (aSet != null)
			{
				ForecastStockMinMaxBoundary aBoundary = aSet.Getboundary(aBoundaryKey);
				if (aBoundary != null)
				{
					min = aBoundary.GetMimimumForWeek(aWeek);
				}
				else //  check for default
				{
					aBoundary = aSet.Getboundary(Include.NoRID);
					if (aBoundary != null)
					{
						min = aBoundary.GetMimimumForWeek(aWeek);
					}
				}
			}

			return min;
		}

		public int GetMaximumForWeek(WeekProfile aWeek, int sglRid, int aBoundaryKey)
		{
			int max = Include.Undefined;
			ForecastStockMinMaxSet aSet = (ForecastStockMinMaxSet)_setHash[sglRid];
			if (aSet != null)
			{
				ForecastStockMinMaxBoundary aBoundary = aSet.Getboundary(aBoundaryKey);
				if (aBoundary != null)
				{
					max = aBoundary.GetMaximumForWeek(aWeek);
				}
				else //  check for default
				{
					aBoundary = aSet.Getboundary(Include.NoRID);
					if (aBoundary != null)
					{
						max = aBoundary.GetMaximumForWeek(aWeek);
					}
				}
			}

			return max;
		}


		public void DebugThis()
		{
			IDictionaryEnumerator setEnumerator = _setHash.GetEnumerator();
			while ( setEnumerator.MoveNext() )
			{
				Debug.WriteLine("Set Key " + setEnumerator.Key.ToString());
				ForecastStockMinMaxSet mmSet = (ForecastStockMinMaxSet)setEnumerator.Value;

				IDictionaryEnumerator boundaryEnumerator = mmSet.Boundaries.GetEnumerator();
				while ( boundaryEnumerator.MoveNext() )
				{
					ForecastStockMinMaxBoundary boundary = (ForecastStockMinMaxBoundary) boundaryEnumerator.Value;
					boundary.DebugThis();
				}
			}
		}

		/// <summary>
		/// holds 1 instance of temporary stock min/max information before it's 
		/// processed into a boundary record.
		/// </summary>
		private class StockMinMaxInfo
		{
			int _boundary;
			int _cdrRid;
			int _minimum;
			int _maximum;
			ProfileList _weekList;

			public int Boundary 
			{
				get { return _boundary ; }
				set { _boundary = value; }
			}
			public int CdrRid 
			{
				get { return _cdrRid ; }
				set { _cdrRid = value; }
			}
			public int Minimum 
			{
				get { return _minimum ; }
				set { _minimum = value; }
			}
			public int Maximum 
			{
				get { return _maximum ; }
				set { _maximum = value; }
			}
			public ProfileList WeekList 
			{
				get { return _weekList ; }
				set { _weekList = value; }
			}

			public StockMinMaxInfo(int boundary, int cdrRid, int min, int max, ProfileList weekList)
			{
				_boundary = boundary;
				_cdrRid = cdrRid;
				_minimum = min;
				_maximum = max;
				_weekList = weekList;
			}

		}
	}

	/// <summary>
	/// holds a list of ForecastStockMinMaxBoundary information for the Set
	/// </summary>
	public class ForecastStockMinMaxSet
	{
		// key is sgl rid (set rid)
		// object is a hash table keyed by boundary for the set
		Hashtable _boundaryHash; 
		int		  _sglRid;

		public int SglRid 
		{
			get { return _sglRid ; }
			set { _sglRid = value; }
		}

		public Hashtable Boundaries 
		{
			get { return _boundaryHash ; }
			set { _boundaryHash = value; }
		}
		
		public ForecastStockMinMaxSet(int sglRid)
		{
			_boundaryHash = new Hashtable();
		}

		public ForecastStockMinMaxBoundary Getboundary(int aBoundaryKey)
		{
			ForecastStockMinMaxBoundary aBoundary = null;
			aBoundary = (ForecastStockMinMaxBoundary)_boundaryHash[aBoundaryKey];

			return aBoundary;
		}

		public void AddBoundary(ForecastStockMinMaxBoundary aBoundary)
		{
			_boundaryHash.Add(aBoundary.Boundary, aBoundary);
		}
	}



	/// <summary>
	/// Holds the Stock Min Max information for a particular boundary key
	/// </summary>
	public class ForecastStockMinMaxBoundary
	{
		Hashtable _weekMinimumHash;  // key is YearWeek
		Hashtable _weekMaximumHash;  // key is YearWeek

		int _boundary;
		private int _defaultMinimum = (int)Include.UndefinedDouble;
		private int _defaultMaximum = (int)Include.UndefinedDouble;
		bool _filled = false;

		public int Boundary 
		{
			get { return _boundary ; }
			set 
			{ 
				_boundary = value;
				_filled = true;
			}
		}
		public int DefaultMinimum 
		{
			get { return _defaultMinimum ; }
			set { _defaultMinimum = value; }
		}
		public int DefaultMaximum 
		{
			get { return _defaultMaximum ; }
			set { _defaultMaximum = value; }
		}
		public bool Filled 
		{
			get { return _filled ; }
		}

		public ForecastStockMinMaxBoundary(int boundary)
		{
			_boundary = boundary;
			_weekMinimumHash = new Hashtable();
			_weekMaximumHash = new Hashtable();
		}

		public void DebugThis()
		{
			Debug.WriteLine(" ");
			Debug.WriteLine("  Boundary " + _boundary.ToString());
			Debug.WriteLine("  Default Min " + _defaultMinimum);
			Debug.WriteLine("  Default Max " + _defaultMaximum);
			IDictionaryEnumerator minEnumerator = _weekMinimumHash.GetEnumerator();
			while ( minEnumerator.MoveNext() )
			{
				Debug.WriteLine("  MIN " + minEnumerator.Key.ToString() + " " + minEnumerator.Value.ToString());
			}
			IDictionaryEnumerator maxEnumerator = _weekMaximumHash.GetEnumerator();
			while ( maxEnumerator.MoveNext() )
			{
				Debug.WriteLine("  MAX " + maxEnumerator.Key.ToString() + " " + maxEnumerator.Value.ToString());
			}
		}

		public int GetMimimumForWeek(WeekProfile aWeek)
		{
			return GetMimimumForWeek(aWeek.YearWeek);
		}
		public int GetMimimumForWeek(int yearWeek)
		{
			if (_weekMinimumHash.ContainsKey(yearWeek))
				return (int)_weekMinimumHash[yearWeek];
			else
				return _defaultMinimum;
		}

		public int GetMaximumForWeek(WeekProfile aWeek)
		{
			return GetMaximumForWeek(aWeek.YearWeek);
		}
		public int GetMaximumForWeek(int yearWeek)
		{
			if (_weekMaximumHash.ContainsKey(yearWeek))
				return (int)_weekMaximumHash[yearWeek];
			else
				return _defaultMaximum;
		}

		public void AddMinimumForWeek(int yearWeek, int min)
		{
			if (_weekMinimumHash.ContainsKey(yearWeek))
			{
				_weekMinimumHash.Remove(yearWeek);
				_weekMinimumHash.Add(yearWeek, min);
			}
			else
				_weekMinimumHash.Add(yearWeek, min);
		}
		public void AddMinimumForWeek(WeekProfile aWeek, int min)
		{
			AddMinimumForWeek(aWeek.YearWeek, min);
		}

		public void AddMaximumForWeek(int yearWeek, int max)
		{
			if (_weekMinimumHash.ContainsKey(yearWeek))
			{
				_weekMaximumHash.Remove(yearWeek);
				_weekMaximumHash.Add(yearWeek, max);
			}
			else
				_weekMaximumHash.Add(yearWeek, max);
		}
		public void AddMaximumForWeek(WeekProfile aWeek, int max)
		{
			AddMaximumForWeek(aWeek.YearWeek, max);
		}
	}
}
