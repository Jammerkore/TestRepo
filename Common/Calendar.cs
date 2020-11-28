// Begin TT#52 - JSmith - Duplicate key adding to hashtable (Make Calendar thread safe.)
// Wrapped all references to global variables with reader/writer locks
// Removed changed code for readability
// Too many changes to mark.  Use compare tool to see differences
// End TT#52 - JSmith - Duplicate key adding to hashtable (Make Calendar thread safe.)

using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

//    T E S T 
using MIDRetail.Common;
//using MIDRetail.Data;  
//using MIDRetail.DataCommon;  
//using System.Diagnostics;  
//    T E S T 

namespace MIDRetail.Common
{
	/// <summary>
	/// This class provides the client calendar information to other classes and windows.
	/// </summary>
	public class MRSCalendar
	{
		private CalendarData _cd;
		private CalendarModels _calendarModels;
		private ArrayList _calendarModelList;

        // Begin TT#5124 - JSmith - Performance
        //private SortedList _yearList;		// Keyed by (YYYYDDD julian) of first week
        //private SortedList _seasonList;		// Keyed by (YYYYDDD julian) of first week
        //private SortedList _quarterList;	// Keyed by (YYYYDDD julian) of first week
        //private SortedList _monthList;		// Keyed by (YYYYDDD julian) of first week
        //private SortedList _weekList;		// Keyed by key, (YYYYDDD julian)
        //private SortedList _dayList;		// Keyed by YYYYDDD

        //private SortedList _sortedSeasonList;		// Keyed by YYYYS
        //private SortedList _sortedQuarterList;		// Keyed by YYYYQQ

        //private Hashtable _daysByDate;
        //private Hashtable _yearsByYear;

        //private Hashtable _weekKeyByFiscal;		// Keyed by YYYYWW
        //private Hashtable _monthKeyByFiscal;	// Keyed by YYYYMM
        //private Hashtable _quarterKeyByFiscal;	// Keyed by YYYYQQ
        //private Hashtable _seasonKeyByFiscal;	// Keyed by YYYYS

        private SortedList<int, YearProfile> _yearList;		// Keyed by (YYYYDDD julian) of first week
        private SortedList<int, SeasonProfile> _seasonList;		// Keyed by (YYYYDDD julian) of first week
        private SortedList<int, QuarterProfile> _quarterList;	// Keyed by (YYYYDDD julian) of first week
        private SortedList<int, MonthProfile> _monthList;		// Keyed by (YYYYDDD julian) of first week
        private SortedList<int, WeekProfile> _weekList;		// Keyed by key, (YYYYDDD julian)
        private SortedList<int, DayProfile> _dayList;		// Keyed by YYYYDDD

        private SortedList<int, SeasonProfile> _sortedSeasonList;		// Keyed by YYYYS
        private SortedList<int, QuarterProfile> _sortedQuarterList;		// Keyed by YYYYQQ

        private Dictionary<int, DayProfile> _daysByDate;
        private Dictionary<int, YearProfile> _yearsByYear;

        private Dictionary<int, int> _weekKeyByFiscal;		// Keyed by YYYYWW
        private Dictionary<int, int> _monthKeyByFiscal;	// Keyed by YYYYMM
        private Dictionary<int, QuarterProfile> _quarterKeyByFiscal;	// Keyed by YYYYQQ
        private Dictionary<int, SeasonProfile> _seasonKeyByFiscal;	// Keyed by YYYYS
        // End TT#5124 - JSmith - Performance

		private DateTime _createDateTime;
		private DateTime _firstDayOfCalendar;
		private DateTime _lastDayOfCalendar;
		private DateTime _currentDayDate;
		private int _firstCalendarFiscalYear;
		private int _lastCalendarFiscalYear;
		private DateTime _postingDate;

		private DataTable _dtDateSelection;
		private DataTable _dtYears;
		private ArrayList _yearsInDateRangeSelector;
        private DataTable _dtSmallDateSelection = null;  // TT#1953-MD - RO Web

		private DataSet _dsCalendarDisplay;
		private DataTable _dtDisplaySeasons;
		private DataTable _dtDisplayQuarters;
		private DataTable _dtDisplayMonths;
		private DataTable _dtDisplayWeeks;
		private ArrayList _yearsInCalendarDisplay;
		
		private DataTable _dtWeek53;
		private ArrayList _yearsInWeek53;

		private DayProfile _postDate;
		private DayProfile _currentDate;
		private PeriodProfile _currentPeriod;
		private WeekProfile _currentWeek;

		private DataTable _dtDateRangesWithNames;
		//private MIDCache _dateRangeCache = new MIDCache(100);
		//private int _cdrFromCache = 0;
		//private int _cdrFromDb = 0;
		//private int _periodOffset, _weekOffset, _dayOffset;
		private int[] _yearBeginIndex;


//		System.DateTime beginTime;
//		System.DateTime endTime;
		System.DateTime beginTime2;
		System.DateTime endTime2;

		// These are work arrays for the GetPeriodProfileList() method
        // Begin TT#5124 - JSmith - Performance
        //private SortedList _years;
        //private SortedList _seasons;
        //private SortedList _quarters;
        //private SortedList _months;

        private SortedList<int, YearProfile> _years;
        private SortedList<int, SeasonProfile> _seasons;
        private SortedList<int, QuarterProfile> _quarters;
        private SortedList<int, MonthProfile> _months;
        // End TT#5124 - JSmith - Performance

		private const int MAXYEARS = 100; // Issue 4352

        ReaderWriterLockSlim rw = Locks.GetLockInstance();
		/// <summary>
		/// Returns the Post Date as a DateProfile.
		/// </summary>
		public DayProfile PostDate 
		{
			get 
			{ 
				if (_postDate != null)
					return _postDate ; 
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PostingDateNotSet,
						MIDText.GetText(eMIDTextCode.msg_PostingDateNotSet));
				}
			}
		}
		/// <summary>
		/// Returns the Created Date Time.
		/// </summary>
		public DateTime CreateDateTime
		{
			get { return _createDateTime ; }
			//set { _createDateTime = value; }
		}
		/// <summary>
		/// Returns the Current Date (date after posting date) as a DayProfile.
		/// </summary>
		public DayProfile CurrentDate 
		{
			get 
			{ 
				if (_currentDate != null)
					return _currentDate ; 
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PostingDateNotSet,
						MIDText.GetText(eMIDTextCode.msg_PostingDateNotSet));
				} 
			}
		}
		/// <summary>
		/// Returns the Current Period as a PeriodProfile.
		/// </summary>
		public PeriodProfile CurrentPeriod 
		{
			get 
			{ 
				if (_currentPeriod != null)
					return _currentPeriod ; 
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PostingDateNotSet,
						MIDText.GetText(eMIDTextCode.msg_PostingDateNotSet));
				} 
			}
		}
		/// <summary>
		/// Returns the Current Week as a WeekProfile.
		/// </summary>
		public WeekProfile CurrentWeek 
		{
			get 
			{ 
				if (_currentWeek != null)
					return _currentWeek ; 
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PostingDateNotSet,
						MIDText.GetText(eMIDTextCode.msg_PostingDateNotSet));
				} 
			}
		}
		/// <summary>
		/// Returns the Date Range Selector Data Table.  This is used only 
		/// in conjunction with the date range selector.
		/// </summary>
		public DataTable DateSelectionDataTable
		{
			get 
			{ 
				if (_dtDateSelection.Rows.Count == 0)
					DateRangeSelector_Populate();

				return _dtDateSelection; 
			}
		}

        // Begin TT#1953-MD - RO Web
        /// <summary>
        /// Returns the Date Range Selector Data Table.  This is used only 
        /// in conjunction with the date range selector.
        /// </summary>
        public DataTable DateSmallSelectionDataTable
        {
            get
            {
                if (_dtSmallDateSelection == null
                    || _dtSmallDateSelection.Rows.Count == 0)
                {
                    string dateSelect = "Year IN (";
                    int selectYear = _currentWeek.FiscalYear - 5;
                    dateSelect += selectYear.ToString();
                    for (int i = 0; i < 9; i++)
                    {
                        ++selectYear;
                        dateSelect += "," + selectYear.ToString();
                    }
                    dateSelect += ")";
                    DataRow[] drows = DateSelectionDataTable.Select(dateSelect);
                    _dtSmallDateSelection = DateSelectionDataTable.Clone();
                    for (int i = 0; i < drows.Length; i++)
                    {
                        _dtSmallDateSelection.ImportRow(drows[i]);
                    }
                }

                return _dtSmallDateSelection.Copy();
            }
        }
        // Begin TT#1953-MD - RO Web

		/// <summary>
		/// Returns the Calendar Display Data Set.  This is used only 
		/// in conjunction with the calendar display.
		/// </summary>
		public DataSet CalendarDisplayDataSet
		{
			get 
			{
				if (_dsCalendarDisplay.Tables[0].Rows.Count == 0)
					CalendarDisplay_Populate();
				return _dsCalendarDisplay; 
			}
		}

		public DataTable CalendarWeek53DataTable
		{
			get { return _dtWeek53; }
		}

		/// <summary>
		/// Returns the first fiscal year defined within the calendar.
		/// </summary>
		public int FirstCalendarFiscalYear
		{
			get { return _firstCalendarFiscalYear; }
		}

		/// <summary>
		/// Returns the last fiscal year defined within the calendar.
		/// </summary>
		public int LastCalendarFiscalYear
		{
			get { return _lastCalendarFiscalYear; }
		}

		/// <summary>
		/// Returns the first fiscal week defined within the calendar.
		/// </summary>
		public int FirstCalendarFiscalWeek
		{
			get 
			{
				if (_weekList == null || _weekList.Count == 0)
				{
					return 0;
				}
				else
				{
                    // Begin TT#5124 - JSmith - Performance
                    //return((WeekProfile)_weekList.GetByIndex(0)).YearWeek;
                    return _weekList.Values[0].YearWeek;
                    // End TT#5124 - JSmith - Performance
				}
			}
		}

		/// <summary>
		/// Returns the last fiscal week defined within the calendar.
		/// </summary>
		public int LastCalendarFiscalWeek
		{
			get 
			{
				if (_weekList == null || _weekList.Count == 0)
				{
					return 0;
				}
				else
				{
                    // Begin TT#5124 - JSmith - Performance
                    //return((WeekProfile)_weekList.GetByIndex(_weekList.Count - 1)).YearWeek;
                    return _weekList.Values[_weekList.Count - 1].YearWeek;
                    // End TT#5124 - JSmith - Performance
				}
			}
		}
		// END MID Track #4373

		/// <summary>
		/// Returns a REPRESENTATIVE first Day of the week as defined in the calendar.
		/// Used to get day of week.
		/// </summary>
		public DayProfile FirstDayOfWeek
		{
			get { return GetDay(_firstDayOfCalendar); }
		}

		/// <summary>
		/// Returns a REPRESENTATIVE last Day of the week as defined in the calendar.
		/// Used to get day of week.
		/// </summary>
		public DayProfile LastDayOfWeek
		{
			get 
			{ 
				return Add(FirstDayOfWeek, 6); 
			}
		}

		/// <summary>
		/// Returns the Calendaer's calendar data object
		/// </summary>
		public CalendarData CalendarData
		{
			get { return _cd; }
		}

		//StreamWriter _sw

		/// <summary>
		/// Base constructor for the class.
		/// </summary>
		public MRSCalendar()
		{
			try
			{
				_cd = new CalendarData();
				// BEGIN Issue 5121 stodd 2.21.2008
                // Begin TT#5124 - JSmith - Performance
                //_yearList = new SortedList();
                //_seasonList = new SortedList();
                //_quarterList = new SortedList();
                //_monthList = new SortedList();
				// END Issue 5121 stodd 2.21.2008
				//_periodList = new SortedList();
                //_weekList = new SortedList();
                //_dayList = new SortedList();
                //_daysByDate = new Hashtable();
                //_yearsByYear = new Hashtable();
                //_weekKeyByFiscal = new Hashtable();
				// BEGIN Issue 5121 stodd 2.21.2008
                //_monthKeyByFiscal = new Hashtable();
                //_quarterKeyByFiscal = new Hashtable();
                //_seasonKeyByFiscal = new Hashtable();
				// END Issue 5121 stodd 2.21.2008

				// Begin Track 5984 stodd
                //_sortedSeasonList = new SortedList();
                //_sortedQuarterList = new SortedList();

                _yearList = new SortedList<int, YearProfile>();
                _seasonList = new SortedList<int, SeasonProfile>();
                _quarterList = new SortedList<int, QuarterProfile>();
                _monthList = new SortedList<int, MonthProfile>();
                _weekList = new SortedList<int, WeekProfile>();
                _dayList = new SortedList<int, DayProfile>();
                _daysByDate = new Dictionary<int,DayProfile>();
                _yearsByYear = new Dictionary<int,YearProfile>();
                _weekKeyByFiscal = new Dictionary<int, int>();
                _monthKeyByFiscal = new Dictionary<int,int>();
                _quarterKeyByFiscal = new Dictionary<int, QuarterProfile>();
                _seasonKeyByFiscal = new Dictionary<int, SeasonProfile>();

                _sortedSeasonList = new SortedList<int, SeasonProfile>();
                _sortedQuarterList = new SortedList<int, QuarterProfile>();
                // End TT#5124 - JSmith - Performance
				// End Track 5984 stodd
				_calendarModels = new CalendarModels();
				_calendarModelList = _calendarModels.Models;

				InitializePostingDate();

				if (_calendarModelList.Count > 0)
				{
					Build();
					SetPostingDate(_postingDate);
				}

				_createDateTime = DateTime.Now;

				//DumpYearsToFile();
				//DumpSeasonsToFile();
				//DumpQuartersToFile();
				//DumpMonthsToFile();
				//DumpWeeksToFile();
				//DumpDaysToFile();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Main function that controls the building of the calendar and all of it's information.
		/// </summary>
		private void Build()
		{
			try
			{
                using (new WriteLock(rw))
                {

                    beginTime2 = System.DateTime.Now;

                    BuildMainCalendar();

                    DateRangeSelector_BuildTables();
                    //DateRangeSelector_Populate();

                    CalendarDisplay_BuildTables();
                    //CalendarDisplay_Populate();

                    Week53_BuildTable();
                    //Week53_Populate();  //now done in 53rd week selector

                    PopulateDateRangesWithNames();
                    endTime2 = System.DateTime.Now;
                    Debug.WriteLine("CALENDAR " + System.Convert.ToString(endTime2.Subtract(beginTime2), CultureInfo.CurrentUICulture));
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		private void InitializePostingDate()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    // Init to today's date
                    _postDate = new DayProfile(0);
                    _postDate.Date = DateTime.Now;
                    _postingDate = DateTime.Now;

                    // if only one posting date on file, us it.
                    DataTable dtPostingDate = _cd.PostingDate_Read();
                    if (dtPostingDate.Rows.Count == 1)
                    {
                        DataRow dr = dtPostingDate.Rows[0];
                        if (dr["POSTING_DATE"] == DBNull.Value)
                        {
                        }
                        else
                        {
                            _postDate.Date = (DateTime)dr["POSTING_DATE"];
                            _postingDate = (DateTime)dr["POSTING_DATE"];
                        }
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void Refresh()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    DateTime holdCurrDate = _currentDate.Date;
                    Clear();

                    _calendarModels = new CalendarModels();
                    _calendarModelList = _calendarModels.Models;

                    if (_calendarModelList.Count > 0)
                    {
                        Build();

                        SetPostingDate(holdCurrDate);
                    }
                    else
                    {
                        _postDate = new DayProfile(0);
                        _postDate.Date = DateTime.Now;
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
	
		/// <summary>
		/// this is to help with memory management prior to the calendar being refreshed by the 
		/// services.
		/// </summary>
		public void Clear()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _yearList.Clear();
                    _seasonList.Clear();
                    _quarterList.Clear();
                    _monthList.Clear();
                    _weekList.Clear();
                    _dayList.Clear();
                    _daysByDate.Clear();
                    _yearsByYear.Clear();
                    _weekKeyByFiscal.Clear();
                    _monthKeyByFiscal.Clear();
                    _quarterKeyByFiscal.Clear();
                    _seasonKeyByFiscal.Clear();
                    _sortedQuarterList.Clear();
                    _sortedSeasonList.Clear();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// This loops through each calendar model building a plethera of hashtables 
		/// and a few arraylists that are used for fast accessing.  
		/// BUT FIRST it uses the first model encountered to go back in time about 100 years, using that
		/// first model as a pattern.  Once it finds this 'first year', it creates a calendar model for it
		/// (just here in the calendar class).  Then it begins looping through each model Starting with 
		/// the 'first year' model (it just made) building about 200 years of calendar information.
		/// </summary>
		/// <remarks>
		/// A note about Date Profile Keys:
		/// Year Profile Key is year (YYYY).
		/// Period Profile Key is a 0-based index + 10000.
		/// Week Profile Key is yearWeek (YYYYWW).
		/// Day Profile Key begins at 0 and increases by one for each day.
		/// </remarks>
		private void BuildMainCalendar()
		{
			try
			{
				int year;
				CalendarModel currModel;

                using (new WriteLock(rw))
                {
                    //  Figure out beginning time frame, make it a model, and add it to the front of the collection
                    if (_calendarModelList.Count > 0)
                    {
                        CalendarModel firstModel = CreateBeginningModel((CalendarModel)_calendarModelList[0]);
                        _calendarModelList.Insert(0, firstModel);
                    }

                    //			_periodOffset = 0;
                    //			_weekOffset = 0;
                    //			_dayOffset = 0;

                    IEnumerator modelEnumerator = _calendarModelList.GetEnumerator();
                    // Loops through each calendar model defined
                    while (modelEnumerator.MoveNext())
                    {
                        currModel = (CalendarModel)modelEnumerator.Current;
                        _currentDayDate = currModel.StartDate;
                        for (year = currModel.FiscalYear; year <= currModel.LastModelYear; year++)
                        {
                            AddYear(year, currModel);
                        }
                        _lastCalendarFiscalYear = year - 1;
                    }
                    _lastDayOfCalendar = (DateTime)_currentDayDate.AddDays(-1);

                    // remove the artificial first calendar model
                    _calendarModelList.RemoveAt(0);
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void AddYear(int newYear)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    //================================================================================
                    // test year to be sure its not going too far in the past or too far in the future.
                    //================================================================================
                    int pastBoundary = this.CurrentDate.FiscalYear - MAXYEARS;
                    int futureBoundary = this.CurrentDate.FiscalYear + MAXYEARS;
                    if (newYear < pastBoundary || newYear > futureBoundary)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_InvalidCalendarDate,
                            MIDText.GetText(eMIDTextCode.msg_InvalidCalendarDate));
                    }

                    int beginYear, endYear, year;
                    DateTime startDate = Include.UndefinedDate;
                    CalendarModel cm = null;

                    if (newYear > _lastCalendarFiscalYear)
                    {
                        // get last model
                        cm = (CalendarModel)_calendarModelList[_calendarModelList.Count - 1];
                        beginYear = _lastCalendarFiscalYear + 1;
                        _currentDayDate = this._lastDayOfCalendar.AddDays(1);
                        endYear = newYear;
                        // Add needed years to Calendar
                        for (year = beginYear; year <= endYear; year++)
                        {
                            AddYear(year, cm);
                        }
                        _lastCalendarFiscalYear = year - 1;
                        _lastDayOfCalendar = (DateTime)_currentDayDate.AddDays(-1);
                    }
                    else if (newYear < _firstCalendarFiscalYear)
                    {
                        // get first model
                        cm = (CalendarModel)_calendarModelList[0];
                        beginYear = newYear;
                        endYear = _firstCalendarFiscalYear - 1;
                        // Add needed years to Calendar -- in REVERSE order
                        for (year = endYear; year >= beginYear - 1; year--)
                        {
                            int week53Period = this._calendarModels.GetWeek53Period(year);
                            if (week53Period > 0)
                                _currentDayDate = _firstDayOfCalendar.AddDays(-371);
                            else
                                _currentDayDate = _firstDayOfCalendar.AddDays(-364);
                            _firstDayOfCalendar = _currentDayDate;
                            _firstCalendarFiscalYear = year;
                            AddYear(year, cm);
                        }
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}		


		private void AddYear(int year, CalendarModel currModel)
		{
            try
            {
                using (new WriteLock(rw))
                {
                    int SeaIdx, QtrIdx, monIdx, weekIdx, dayIdx;
                    int noOfWeeks;
                    CalendarModelPeriod modelSeason;
                    CalendarModelPeriod modelQuarter;
                    CalendarModelPeriod modelMonth;
                    int week53Period = 0;
                    bool week53Year = false;

                    // return if another process already added the year
                    if (_yearsByYear.ContainsKey(year))
                    {
                        return;
                    }
                    YearProfile newYear = new YearProfile(year);
                    newYear.ProfileStartDate = _currentDayDate;
                    newYear.FiscalYear = year;
                    newYear.CalendarModelRID = currModel.RID;
                    newYear.Week53OffsetId = _calendarModels.GetWeek53OffsetId(year);

                    week53Period = _calendarModels.GetWeek53Period(year);

                    if (week53Period > 0)
                        week53Year = true;
                    else
                        week53Year = false;
                    // week has to wait until all periods within the year are process to know
                    // how many weeks are in the year before we can add this year to the hash table.
                    // the adding to the year hash table is at the end of the period loop 

                    int seasonInYear = 1;
                    int quarterInYear = 1;
                    int weekInYear = 1;
                    int monthInYear = 1;
                    for (SeaIdx = 0; SeaIdx < currModel.NumberOfSeasons; SeaIdx++)
                    {
                        // * Get current season from calendar model
                        modelSeason = (CalendarModelPeriod)currModel.Seasons[seasonInYear - 1];
                        SeasonProfile newSeason = new SeasonProfile(year * 10 + seasonInYear);
                        // * Update newSeason with season model information
                        newSeason.ProfileStartDate = _currentDayDate;
                        newSeason.FiscalYear = year;
                        newSeason.FiscalPeriod = seasonInYear;
                        newSeason.YearPeriod = newSeason.FiscalYear * 10 + seasonInYear;
                        newSeason.Name = modelSeason.Name;
                        newSeason.Abbreviation = modelSeason.Abbreviation;
                        // Add new month to current season
                        newYear.ChildPeriodList.Add(newSeason);

                        _seasonKeyByFiscal.Add(newSeason.YearPeriod, newSeason);
                        _sortedSeasonList.Add(newSeason.YearPeriod, newSeason);	// Track #5984 stodd

                        for (QtrIdx = 0; QtrIdx < modelSeason.NoOfTimePeriods; QtrIdx++)
                        {
                            // * Get current quarter from calendar model
                            modelQuarter = (CalendarModelPeriod)currModel.Quarters[quarterInYear - 1];
                            QuarterProfile newQuarter = new QuarterProfile(year * 100 + quarterInYear);
                            // * Update newQuarter with quarter model information
                            newQuarter.ProfileStartDate = _currentDayDate;
                            newQuarter.FiscalYear = year;
                            newQuarter.FiscalPeriod = quarterInYear;
                            newQuarter.YearPeriod = newQuarter.FiscalYear * 100 + quarterInYear;
                            newQuarter.Name = modelQuarter.Name;
                            newQuarter.Abbreviation = modelQuarter.Abbreviation;
                            // Add new month to current season
                            newSeason.ChildPeriodList.Add(newQuarter);

                            _quarterKeyByFiscal.Add(newQuarter.YearPeriod, newQuarter);
                            _sortedQuarterList.Add(newQuarter.YearPeriod, newQuarter);	// Track #5984 stodd

                            for (monIdx = 0; monIdx < modelQuarter.NoOfTimePeriods; monIdx++)
                            {
                                modelMonth = (CalendarModelPeriod)currModel.Months[monthInYear - 1];

                                if (week53Year && monthInYear == week53Period)
                                {
                                    noOfWeeks = modelMonth.NoOfTimePeriods + 1;
                                }
                                else
                                    noOfWeeks = modelMonth.NoOfTimePeriods;
                                // build Period
                                // Begin TT#5124 - JSmith - Performance
                                //PeriodProfile newMonth = new MonthProfile(_currentDayDate.Year * 1000 + _currentDayDate.DayOfYear);
                                MonthProfile newMonth = new MonthProfile(_currentDayDate.Year * 1000 + _currentDayDate.DayOfYear);
                                // End TT#5124 - JSmith - Performance

                                newMonth.ProfileStartDate = _currentDayDate;
                                newMonth.FiscalYear = year;
                                newMonth.FiscalPeriod = monthInYear; ;
                                newMonth.YearPeriod = newMonth.FiscalYear * 100 + newMonth.FiscalPeriod;
                                newMonth.Name = modelMonth.Name;
                                newMonth.Abbreviation = modelMonth.Abbreviation;

                                // Add new month to current season
                                newQuarter.ChildPeriodList.Add(newMonth);
                                _monthKeyByFiscal.Add(newMonth.YearPeriod, newMonth.Key);

                                for (weekIdx = 0; weekIdx < noOfWeeks; weekIdx++)
                                {
                                    // Build Week
                                    WeekProfile newWeek = new WeekProfile(_currentDayDate.Year * 1000 + _currentDayDate.DayOfYear);

                                    newWeek.Period = newMonth;
                                    newWeek.FiscalYear = year;
                                    newWeek.ProfileStartDate = _currentDayDate;
                                    newWeek.Period.FiscalPeriod = monthInYear;
                                    newWeek.WeekInPeriod = weekIdx + 1;
                                    newWeek.WeekInYear = weekInYear++;
                                    newWeek.ProfileStartDate = _currentDayDate;
                                    newWeek.YearWeek = (newWeek.FiscalYear * 100) + newWeek.WeekInYear;
                                    // Add weeks to all week lists
                                    newMonth.Weeks.Add(newWeek);
                                    newQuarter.Weeks.Add(newWeek);
                                    newSeason.Weeks.Add(newWeek);
                                    newYear.Weeks.Add(newWeek);
                                    // Add week to current Month
                                    _weekList.Add(newWeek.Key, newWeek);
                                    _weekKeyByFiscal.Add(newWeek.YearWeek, newWeek.Key);

                                    for (dayIdx = 0; dayIdx < newWeek.DaysInWeek; dayIdx++)
                                    {
                                        // Build Day
                                        DayProfile newDay = new DayProfile(_currentDayDate.Year * 1000 + _currentDayDate.DayOfYear);

                                        newDay.Period = newMonth;
                                        newDay.Week = newWeek;
                                        newDay.FiscalYear = year;
                                        newDay.ProfileStartDate = _currentDayDate;
                                        newDay.DayOfWeek = newDay.ProfileStartDate.DayOfWeek;
                                        newDay.DayInWeek = dayIdx + 1;
                                        newWeek.Days.Add(newDay);
                                        _daysByDate.Add(newDay.GetHashCode(), newDay);
                                        _dayList.Add(newDay.GetAltHashCode(), newDay);
                                        _currentDayDate = _currentDayDate.AddDays(1);
                                    }
                                } // END Week
                                ((ProfileList)newYear.Periods).Add(newMonth);
                                _monthList.Add(newMonth.BeginDateJulian, newMonth);
                                monthInYear++;
                            } // END Month
                            _quarterList.Add(newQuarter.BeginDateJulian, newQuarter);
                            quarterInYear++;
                        } // END Quarter
                        _seasonList.Add(newSeason.BeginDateJulian, newSeason);
                        seasonInYear++;
                    } // END Season

                    // Now that we know the number of weeks in the year, add the year
                    //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
                    //newYear.WeeksInYear = weekInYear - 1;
                    //newYear.WeeksInPeriod = weekInYear - 1;
                    //End Track #5121 - JScott - Add Year/Season/Quarter totals
                    this._yearsByYear.Add(newYear.GetHashCode(), newYear);
                    _yearList.Add(newYear.BeginDateJulian, newYear);

                    // Rebuild year to week index
                    BuildYearWeekIndexList();
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
		}

		private void BuildYearWeekIndexList()
		{
            using (new WriteLock(rw))
            {
                _yearBeginIndex = new int[_yearsByYear.Count];
                int currYear = 0;
                int idx = 0;
                int w = 0;
                for (w = 0; w < this._weekList.Count; w++)
                {
                    // Begin TT#5124 - JSmith - Performance
                    //WeekProfile wp = (WeekProfile)_weekList.GetByIndex(w);
                    WeekProfile wp = _weekList.Values[w];
                    // End TT#5124 - JSmith - Performance
                    if (wp.FiscalYear != currYear)
                    {
                        int yrIdx = w - 1;
                        _yearBeginIndex[idx++] = w - 1;
                        currYear = wp.FiscalYear;
                        //Debug.WriteLine(wp.FiscalYear.ToString() + " " + yrIdx.ToString());
                    }
                }
            }

            // Debug code
//			Debug.WriteLine("UNLOAD");
            //			for (w=0;w<this._yearBeginIndex.Length;w++)
//			{
            //				int wkIdx = _yearBeginIndex[w];
            //				WeekProfile wp = (WeekProfile)_weekList.GetByIndex(w);
//				Debug.WriteLine(wp.FiscalYear.ToString() + " " + wkIdx.ToString());
//			}

		}

		/// <summary>
		/// his does all of the work of backtracking from the first defined model to get a starting 
		/// point for the calendar build process.  Once it finds the 'first year' on the calendar, it
		/// loads a temporary calendar model, so processing can continue.
		/// </summary>
		/// <param name="currModel">Calendar Model</param>
		/// <returns>First Calendar Model</returns>
		private CalendarModel CreateBeginningModel(CalendarModel currModel)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    CalendarModel firstModel = (CalendarModel)currModel.Clone();
                    int beginYear = currModel.FiscalYear - Include.CalendarStartupYearRange;
                    DateTime baseDate = new DateTime(beginYear, 1, 1);
                    DateTime currDate = currModel.StartDate;

                    int startYear = currModel.FiscalYear;
                    int year = 0;

                    int week53Period = 0;
                    bool week53Year = false;
                    for (year = currModel.FiscalYear - 1; currDate > baseDate; year--)
                    {
                        week53Period = this._calendarModels.GetWeek53Period(year);
                        if (week53Period > 0)
                            week53Year = true;
                        else
                            week53Year = false;
                        if (week53Year)
                        {
                            currDate = currDate.AddDays(-371);
                        }
                        else
                        {
                            currDate = currDate.AddDays(-364);
                        }
                    }

                    firstModel.ModelName = "First Model";
                    firstModel.LastModelYear = currModel.FiscalYear - 1;
                    firstModel.FiscalYear = year + 1;
                    _firstCalendarFiscalYear = firstModel.FiscalYear;
                    firstModel.StartDate = currDate;
                    _firstDayOfCalendar = currDate;

                    return firstModel;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		public void SetPostingDate(DateTime postingDate)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    if (postingDate == Include.UndefinedDate)
                        _postingDate = DateTime.Now;
                    else
                        _postingDate = postingDate;
                    PopulateStaticDates();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		/// <summary>
		/// Populates the static dates in the calendar: post date and current date information.
		/// </summary>
		private void PopulateStaticDates()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    // Set Post Date
                    this._postDate = GetDay(_postingDate);

                    // Set Current Day
                    DateTime dt = _postingDate.AddDays(1);
                    this._currentDate = GetDay(dt);

                    // Set Current Week
                    this._currentWeek = this.GetWeek(_currentDate.Date);

                    // Set Current Period
                    this._currentPeriod = _currentWeek.Period;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

        public ArrayList GetCalendarModels()
        {
            try
            {
                using (new ReadLock(rw))
                {
                    return _calendarModelList;
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
        }

		/// <summary>
		/// Constructs the data table used by the date range selector.
		/// </summary>
		private void DateRangeSelector_BuildTables()
		{
            try
            {
                using (new WriteLock(rw))
                {
                    _dtDateSelection = MIDEnvironment.CreateDataTable("Date Selection");

                    DataColumn myDataColumn;

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Period";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Name";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week1";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week2";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week3";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week4";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week5";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDateSelection.Columns.Add(myDataColumn);


                    // build DataTable of just years to use as a parent table
                    // for the date selection table
                    _dtYears = MIDEnvironment.CreateDataTable("Years");

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtYears.Columns.Add(myDataColumn);
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
		}

		/// <summary>
		/// Populates the date range selector data table and year array list.
		/// </summary>
		private void DateRangeSelector_Populate()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _yearsInDateRangeSelector = new ArrayList();
                    for (int year = FirstCalendarFiscalYear; year < LastCalendarFiscalYear; year++)
                    {
                        DateRangeSelector_AddYear(year);
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if the date range selector contains a specific year.  If it doesn't, it addes it.
		/// </summary>
		/// <param name="year">int year.</param>
		public void DateRangeSelector_CheckYear(int year)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    if (_yearsInDateRangeSelector == null)
                        DateRangeSelector_Populate();

                    if (!_yearsInDateRangeSelector.Contains(year))
                    {
                        this.AddYear(year);
                        DateRangeSelector_AddYear(year);
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a specific year of values to the date range selector data table.
		/// </summary>
		/// <param name="year"></param>
		private void DateRangeSelector_AddYear(int year)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    DataRow dr;
                    // Begin TT#5124 - JSmith - Performance
                    //YearProfile cYear = (YearProfile)_yearsByYear[year];
                    YearProfile cYear = _yearsByYear[year];
                    // End TT#5124 - JSmith - Performance
                    _yearsInDateRangeSelector.Add(year);

                    // add year 'header' row
                    dr = _dtDateSelection.NewRow();
                    dr["Year"] = year;
                    dr["Name"] = "Year: " + year.ToString("0000", CultureInfo.CurrentUICulture);
                    dr["Period"] = DBNull.Value; ;
                    dr["Week1"] = DBNull.Value;
                    dr["Week2"] = DBNull.Value;
                    dr["Week3"] = DBNull.Value;
                    dr["Week4"] = DBNull.Value;
                    dr["Week5"] = DBNull.Value;
                    _dtDateSelection.Rows.Add(dr);

                    foreach (PeriodProfile period in cYear.Periods.ArrayList)
                    {
                        int weekNo = 0;
                        dr = _dtDateSelection.NewRow();
                        dr["Year"] = year;
                        dr["Name"] = period.Abbreviation + " " + year.ToString("0000", CultureInfo.CurrentUICulture);
                        dr["Period"] = period.FiscalPeriod;
                        for (int wk = 1; wk <= period.NoOfWeeks; wk++)  // Issue 5121
                        {
                            WeekProfile week = GetWeek(year, period.FiscalPeriod, wk);

                            weekNo = wk % 5;

                            switch (weekNo)
                            {
                                case 1:
                                    dr["Week1"] = week.WeekInYear;
                                    break;
                                case 2:
                                    dr["Week2"] = week.WeekInYear;
                                    break;
                                case 3:
                                    dr["Week3"] = week.WeekInYear;
                                    break;
                                case 4:
                                    dr["Week4"] = week.WeekInYear;
                                    break;
                                case 0:
                                    dr["Week5"] = week.WeekInYear;
                                    break;
                            }

                            // are we on week 5 and still have more weeks to go? 
                            if (weekNo == 0 && period.NoOfWeeks > wk)
                            {
                                // add current row
                                _dtDateSelection.Rows.Add(dr);
                                // get a new row ready
                                dr = _dtDateSelection.NewRow();
                                dr["Year"] = year;
                                dr["Name"] = period.Abbreviation + " (cont)";
                                dr["Period"] = period.FiscalPeriod;
                            }
                        }
                        _dtDateSelection.Rows.Add(dr);
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if the date range name sent already exists
		/// </summary>
		/// <param name="year">int year.</param>
		public bool DateRangeSelector_NameExists(string newName)
		{
            try
            {
                bool nameExists = false;
                using (new ReadLock(rw))
                {
                    RefreshDateRangesWithNames();
                    foreach (DataRow aRow in _dtDateRangesWithNames.Rows)
                    {
                        string aName = aRow["CDR_NAME"].ToString();
                        if (newName.ToUpper(CultureInfo.CurrentUICulture) == aName.ToUpper(CultureInfo.CurrentUICulture))
                        {
                            nameExists = true;
                            break;
                        }
                    }
                }
                return nameExists;
            }
            catch
            {
                throw;
            }
		}

		private void CalendarDisplay_BuildTables()
		{
			DataColumn myDataColumn;

			try
			{
                using (new WriteLock(rw))
                {
                    _dtDisplaySeasons = MIDEnvironment.CreateDataTable("Seasons");

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplaySeasons.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Season";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplaySeasons.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Name";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplaySeasons.Columns.Add(myDataColumn);


                    _dtDisplayQuarters = MIDEnvironment.CreateDataTable("Quarters");

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayQuarters.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Season";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayQuarters.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Quarter";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayQuarters.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Name";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayQuarters.Columns.Add(myDataColumn);


                    _dtDisplayMonths = MIDEnvironment.CreateDataTable("Months");

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayMonths.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Quarter";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayMonths.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Month";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayMonths.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Name";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayMonths.Columns.Add(myDataColumn);

                    // Build weeks table
                    _dtDisplayWeeks = MIDEnvironment.CreateDataTable("Weeks");

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Year";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayWeeks.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Month";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayWeeks.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "Week";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayWeeks.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Date";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayWeeks.Columns.Add(myDataColumn);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = "JulianDate";
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtDisplayWeeks.Columns.Add(myDataColumn);

                    // add tables to dataset
                    _dsCalendarDisplay = MIDEnvironment.CreateDataSet("Calendar Display DS");
                    _dsCalendarDisplay.Tables.Add(_dtDisplaySeasons);
                    _dsCalendarDisplay.Tables.Add(_dtDisplayQuarters);
                    _dsCalendarDisplay.Tables.Add(_dtDisplayMonths);
                    _dsCalendarDisplay.Tables.Add(_dtDisplayWeeks);
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void CalendarDisplay_Populate()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _yearsInCalendarDisplay = new ArrayList();
                    int startYear = _currentPeriod.FiscalYear - 3;
                    int endYear = _currentPeriod.FiscalYear + 3;
                    for (int year = startYear; year < endYear; year++)
                    {
                        CalendarDisplay_AddYear(year);
                    }

                    DataColumn[] mCols = new DataColumn[2], wCols = new DataColumn[2];
                    DataColumn[] sCols = new DataColumn[2], qCols = new DataColumn[2];
                    DataColumn[] m2Cols = new DataColumn[2], q2Cols = new DataColumn[2];
                    sCols[0] = _dsCalendarDisplay.Tables["Seasons"].Columns["Year"];
                    sCols[1] = _dsCalendarDisplay.Tables["Seasons"].Columns["Season"];

                    qCols[0] = _dsCalendarDisplay.Tables["Quarters"].Columns["Year"];
                    qCols[1] = _dsCalendarDisplay.Tables["Quarters"].Columns["Season"];
                    q2Cols[0] = _dsCalendarDisplay.Tables["Quarters"].Columns["Year"];
                    q2Cols[1] = _dsCalendarDisplay.Tables["Quarters"].Columns["Quarter"];

                    mCols[0] = _dsCalendarDisplay.Tables["Months"].Columns["Year"];
                    mCols[1] = _dsCalendarDisplay.Tables["Months"].Columns["Quarter"];
                    m2Cols[0] = _dsCalendarDisplay.Tables["Months"].Columns["Year"];
                    m2Cols[1] = _dsCalendarDisplay.Tables["Months"].Columns["Month"];

                    wCols[0] = _dsCalendarDisplay.Tables["Weeks"].Columns["Year"];
                    wCols[1] = _dsCalendarDisplay.Tables["Weeks"].Columns["Month"];


                    DataRelation monthRel = _dsCalendarDisplay.Relations.Add("MonthWeeks", m2Cols, wCols);
                    DataRelation quarterRel = _dsCalendarDisplay.Relations.Add("QuarterMonth", q2Cols, mCols);
                    DataRelation seasonsRel = _dsCalendarDisplay.Relations.Add("SeasonQuarter", sCols, qCols);
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void CalendarDisplay_CheckYear(int year)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    if (_yearsInCalendarDisplay == null)
                        CalendarDisplay_Populate();

                    if (!_yearsInCalendarDisplay.Contains(year))
                    {
                        AddYear(year);
                        CalendarDisplay_AddYear(year);
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void CalendarDisplay_AddYear(int year)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    DataRow dr;
                    // Begin TT#5124 - JSmith - Performance
                    //YearProfile cYear = (YearProfile)_yearsByYear[year];
                    YearProfile cYear = _yearsByYear[year];
                    // End TT#5124 - JSmith - Performance
                    _yearsInCalendarDisplay.Add(year);
                    DataTable dtSeasons = (DataTable)_dsCalendarDisplay.Tables["Seasons"];
                    DataTable dtQuarters = (DataTable)_dsCalendarDisplay.Tables["Quarters"];
                    DataTable dtMonths = (DataTable)_dsCalendarDisplay.Tables["Months"];
                    DataTable dtWeeks = (DataTable)_dsCalendarDisplay.Tables["Weeks"];

                    string aName;
                    foreach (SeasonProfile season in cYear.ChildPeriodList.ArrayList)
                    {
                        dr = dtSeasons.NewRow();
                        dr["Year"] = year;
                        aName = season.Name;
                        aName += " (" + season.NoOfWeeks.ToString(CultureInfo.CurrentUICulture) + ")"; // Issue 5121
                        dr["Name"] = aName;
                        dr["Season"] = season.FiscalPeriod;
                        dtSeasons.Rows.Add(dr);

                        foreach (QuarterProfile quarter in season.ChildPeriodList.ArrayList)
                        {
                            dr = dtQuarters.NewRow();
                            dr["Year"] = year;
                            aName = quarter.Name;
                            aName += " (" + quarter.NoOfWeeks.ToString(CultureInfo.CurrentUICulture) + ")"; // Issue 5121
                            dr["Name"] = aName;
                            dr["Quarter"] = quarter.FiscalPeriod;
                            dr["Season"] = season.FiscalPeriod;
                            dtQuarters.Rows.Add(dr);

                            foreach (MonthProfile month in quarter.ChildPeriodList.ArrayList)
                            {
                                dr = dtMonths.NewRow();
                                dr["Year"] = year;
                                aName = month.Name;
                                aName += " (" + month.NoOfWeeks.ToString(CultureInfo.CurrentUICulture) + ")"; // Issue 5121
                                dr["Name"] = aName;
                                dr["Month"] = month.FiscalPeriod;
                                dr["Quarter"] = quarter.FiscalPeriod;
                                dtMonths.Rows.Add(dr);

                                for (int wk = 1; wk <= month.NoOfWeeks; wk++) // Issue 5121
                                {
                                    WeekProfile week = GetWeek(year, month.FiscalPeriod, wk);
                                    dr = dtWeeks.NewRow();
                                    dr["Year"] = year;
                                    dr["Week"] = week.WeekInYear;
                                    dr["Date"] = week.Date.ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
                                    dr["Month"] = month.FiscalPeriod;
                                    dr["JulianDate"] = week.Key;
                                    dtWeeks.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void Week53_BuildTable()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _dtWeek53 = MIDEnvironment.CreateDataTable("Week53");
                }

			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void Week53_Populate(int calendarModelRID)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    CalendarModel cm = _calendarModels.GetCalendarModel(calendarModelRID);

                    if (_yearsInWeek53 == null)
                        _yearsInWeek53 = new ArrayList();
                    else
                        _yearsInWeek53.Clear();

                    _dtWeek53 = MIDEnvironment.CreateDataTable("week53");

                    // The first and the last model's beginning year and ending year, respectively, 
                    // are really infinite.  Here we are placing a logical limit on what is displayed
                    // in the control.
                    int beginYear = 0;
                    int endYear = 0;
                    if (_calendarModels.IsFirstModel(calendarModelRID))
                        beginYear = 1901;
                    else
                        beginYear = cm.FiscalYear;
                    if (_calendarModels.IsLastModel(calendarModelRID))
                        endYear = 2100;
                    else
                        endYear = cm.LastModelYear;

                    //for (int year=_firstCalendarFiscalYear;year<=_lastCalendarFiscalYear;year++)
                    for (int year = beginYear; year <= endYear; year++)
                    {
                        Week53_AddYear(year, cm.RID);
                    }

                    _dtWeek53.AcceptChanges();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void Week53_AddYear(int year, int calendarModelRID)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    DataColumn myDataColumn;
                    string columnName = year.ToString(CultureInfo.CurrentUICulture);

                    // Create next column
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.Int32");
                    myDataColumn.ColumnName = columnName;
                    myDataColumn.AutoIncrement = false;
                    myDataColumn.ReadOnly = false;
                    myDataColumn.Unique = false;
                    // Add the Column to the DataColumnCollection.
                    _dtWeek53.Columns.Add(myDataColumn);
                    // Add the year to an arraylist for easy checking
                    _yearsInWeek53.Add(year);

                    // Add two rows to DT if they are not there already
                    if (_dtWeek53.Rows.Count == 0)
                    {
                        // 1 - for Model RID - hidden
                        DataRow dr = _dtWeek53.NewRow(); ;
                        _dtWeek53.Rows.Add(dr);
                        // 2 - Period sequence
                        dr = _dtWeek53.NewRow(); ;
                        _dtWeek53.Rows.Add(dr);
                    }

                    _dtWeek53.Rows[0][columnName] = calendarModelRID;
                    _dtWeek53.Rows[1][columnName] = _calendarModels.GetWeek53Period(year);
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Delete all calendar week53 records
		/// </summary>
		public void Week53_Delete(int cm_rid)
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _cd.CalendarWeek53Year_Delete(cm_rid);
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// reads all week53 records for a particular calendar model
		/// </summary>
		public DataTable Week53_Read(int cm_rid)
		{
			try
			{
                DataTable dt = _cd.CalendarWeek53Year_Read(cm_rid);
				dt.PrimaryKey = new DataColumn[] {dt.Columns["WEEK53_FISCAL_YEAR"] };

				return dt;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// inserts a week 53 record
		/// </summary>
		/// <param name="year">int</param>
		/// <param name="cm_rid">int</param>
		/// <param name="sequence">int</param>
		public void Week53_Insert(int year, int cm_rid, int sequence, DataCommon.eWeek53Offset offset)
		{
			try
			{
                //_cd.OpenUpdateConnection();
                _cd.CalendarWeek53Year_Insert(year, cm_rid, sequence, offset);
                //_cd.CommitData();
                //_cd.CloseUpdateConnection();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Week53_Update(int year, int cm_rid, int sequence, DataCommon.eWeek53Offset offset)
		{
			try
			{
                //				_cd.OpenUpdateConnection();
                _cd.CalendarWeek53Year_Update(year, cm_rid, sequence, offset);
                //				_cd.CommitData();
                //				_cd.CloseUpdateConnection();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// forces Calendar List class to re-read week 53 selections
		/// </summary>
		public void Week53_Refresh()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _calendarModels.Week53_read();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// When a Model is added or deleted, it can cause certain years that may have 
		/// been in one model definiation to now be in a different model.  If any of these years
		/// where defined with 53 weeks, those model RIDs are now incorrect.  This function checks for  
		/// 53 week selections containing the wrong model RID and corrects them.  This method is
		/// called immediately after adding or removing a calendar model definition.
		/// </summary>
		public void Realign53WeekSelections()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _calendarModels.OpenUpdateConnection();

                    DataTable dtModels = _calendarModels.Read();
                    int[] startYear = new int[dtModels.Rows.Count];
                    int[] endYear = new int[dtModels.Rows.Count];
                    int[] modelRID = new int[dtModels.Rows.Count];


                    int mIdx = 0;
                    foreach (DataRow row in dtModels.Rows)
                    {
                        if (mIdx == 0)  // first model
                        {
                            startYear[mIdx] = 0;
                        }
                        else
                        {
                            int year = Convert.ToInt32(row["FISCAL_YEAR"], CultureInfo.CurrentUICulture);
                            endYear[mIdx - 1] = year - 1;
                            startYear[mIdx] = year;
                        }
                        modelRID[mIdx] = Convert.ToInt32(row["CM_RID"], CultureInfo.CurrentUICulture);

                        mIdx++;
                    }
                    endYear[mIdx - 1] = 9999; // get index to point to last model and set it's end year to 9999

                    DataTable dt53WeekYears = _calendarModels.Week53_read();

                    foreach (DataRow row in dt53WeekYears.Rows)
                    {
                        int year = Convert.ToInt32(row["WEEK53_FISCAL_YEAR"], CultureInfo.CurrentUICulture);
                        for (int m = 0; m < startYear.Length; m++)
                        {
                            if (year >= startYear[m] && year <= endYear[m])  //year belongs in this model
                            {
                                int seq = Convert.ToInt32(row["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
                                int w53ModelRID = Convert.ToInt32(row["CM_RID"], CultureInfo.CurrentUICulture);
                                eWeek53Offset offset = (eWeek53Offset)Convert.ToInt32(row["OFFSET_ID"], CultureInfo.CurrentUICulture);
                                if (w53ModelRID != modelRID[m])
                                {
                                    CalendarModels cModels = new CalendarModels();
                                    if (cModels.IsSeqOnModel(modelRID[m], seq))
                                        _calendarModels.Week53_update(year, modelRID[m], seq, offset);
                                    else
                                        _calendarModels.Week53_delete(year, w53ModelRID, seq);
                                }
                            }
                        }
                    }

                    _calendarModels.CommitData();
                    _calendarModels.CloseUpdateConnection();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if requested model is first model defined 
		/// </summary>
		/// <param name="cm_rid">int</param>
		/// <returns>bool</returns>
		public bool IsFirstModel(int cm_rid)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    return _calendarModels.IsFirstModel(cm_rid);
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if today's date is within the date range sent.
		/// WILL NOT WORK FOR DYNAMIC TO PLAN DATE RANGES.
		/// </summary>
		/// <param name="dateRangeProfileRid"></param>
		/// <returns></returns>
		public bool IsCurrentDateWithinRange(int dateRangeProfileRid)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    bool inRange = false;

                    //Begin TT#237 - MD - Date range value incorrect - rbeck
                    //DateTime today = DateTime.Now;
                    // int todayYYYYDDD = (today.Year * 1000) +  today.DayOfYear;
                    int todayYYYYDDD = _currentDate.YearDay;
                    //End   TT#237 - MD - Date range value incorrect - rbeck

                    DateRangeProfile drp = this.GetDateRange(dateRangeProfileRid);

                    ProfileList dayList = this.GetDateRangeDays(drp, null);

                    if (todayYYYYDDD >= dayList.MinValue && todayYYYYDDD <= dayList.MaxValue)
                        inRange = true;

                    return inRange;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void PopulateDateRangesWithNames()
		{
			try
			{
                using (new WriteLock(rw))
                {
                    _dtDateRangesWithNames = _cd.CalendarDateRange_ReadForNames();
                    DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                    PrimaryKeyColumn[0] = _dtDateRangesWithNames.Columns["CDR_NAME"];
                    _dtDateRangesWithNames.PrimaryKey = PrimaryKeyColumn;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Gets and rebuilds from the DB the Date Ranges with names
		/// </summary>
		/// <returns></returns>
		public void RefreshDateRangesWithNames()
		{
			try
			{
				PopulateDateRangesWithNames();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the date ranges with names data view
		/// </summary>
		/// <param name="aRelativeTo"></param>
		/// <returns></returns>
		public DataView GetDateRangesWithNames(bool allowDynToCurrent, bool allowDynToPlan, bool allowDynToStore,
				bool allowReoccurring, bool restrictToWeeks, bool restrictToPeriods, bool allowDynamicSwitch)
		{
            using (new ReadLock(rw))
            {
                // refresh Datatable
                PopulateDateRangesWithNames();

                // Begin Issue 3998 - stodd, only return the appropiate date ranges with names
                //==========================
                // Static - always selected
                //==========================
                string filter = "(CDR_RANGE_TYPE_ID = " + ((int)eCalendarRangeType.Static).ToString(CultureInfo.CurrentUICulture);
                //=========	
                // Dynamic
                //=========
                if (allowDynToCurrent)
                {
                    filter += " OR (CDR_RANGE_TYPE_ID = " + ((int)eCalendarRangeType.Dynamic).ToString(CultureInfo.CurrentUICulture) +
                        " AND CDR_RELATIVE_TO = " + ((int)eDateRangeRelativeTo.Current).ToString(CultureInfo.CurrentUICulture) + ")";
                }
                if (allowDynToPlan)
                {
                    filter += " OR (CDR_RANGE_TYPE_ID = " + ((int)eCalendarRangeType.Dynamic).ToString(CultureInfo.CurrentUICulture) +
                        " AND CDR_RELATIVE_TO = " + ((int)eDateRangeRelativeTo.Plan).ToString(CultureInfo.CurrentUICulture) + ")";
                }
                if (allowDynToStore)
                {
                    filter += " OR (CDR_RANGE_TYPE_ID = " + ((int)eCalendarRangeType.Dynamic).ToString(CultureInfo.CurrentUICulture) +
                        " AND CDR_RELATIVE_TO = " + ((int)eDateRangeRelativeTo.StoreOpen).ToString(CultureInfo.CurrentUICulture) + ")";
                }
                //=============
                // reoccurring
                //=============
                if (allowReoccurring)
                {
                    filter += " OR CDR_RANGE_TYPE_ID = " + ((int)eCalendarRangeType.Reoccurring).ToString(CultureInfo.CurrentUICulture);
                }

                filter += ")";

                //==============
                // Restrictions
                //==============
                if (restrictToWeeks)
                    filter += " AND CDR_DATE_TYPE_ID = " + ((int)eCalendarDateType.Week).ToString(CultureInfo.CurrentUICulture);
                else if (restrictToPeriods)
                    filter += " AND CDR_DATE_TYPE_ID = " + ((int)eCalendarDateType.Period).ToString(CultureInfo.CurrentUICulture);

                //string filter = "CDR_RELATIVE_TO = 0 OR CDR_RELATIVE_TO = " + ((int)aRelativeTo).ToString(CultureInfo.CurrentUICulture);
                // End Issue 3998
                DataTable dtFiltered = _dtDateRangesWithNames.Clone();


                foreach (DataRow dr in _dtDateRangesWithNames.Select(filter))
                {
                    //	object[] drVals = {dr[0], dr[1], dr[2]};
                    object[] drVals = { dr["CDR_RID"], dr["CDR_START"], dr["CDR_END"], dr["CDR_RANGE_TYPE_ID"], dr["CDR_DATE_TYPE_ID"], dr["CDR_RELATIVE_TO"], dr["CDR_NAME"] };
                    dtFiltered.LoadDataRow(drVals, false);
                }

                DataView dvFiltered = new DataView(dtFiltered, "", "CDR_NAME", DataViewRowState.CurrentRows);
                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                PrimaryKeyColumn[0] = dvFiltered.Table.Columns["CDR_NAME"];
                dvFiltered.Table.PrimaryKey = PrimaryKeyColumn;

                return dvFiltered;
            }
		}
		/// <summary>
		/// Returns the date ranges with names data view
		/// </summary>
		/// <param name="aRelativeTo"></param>
		/// <returns></returns>
		public DataView GetDateRangesWithNames(eDateRangeRelativeTo aRelativeTo, bool allowDynamicSwitch)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    // refresh Datatable
                    PopulateDateRangesWithNames();

                    string filter = string.Empty;
                    if (allowDynamicSwitch)
                    {
                        filter = "CDR_RELATIVE_TO = 0 OR CDR_RELATIVE_TO = " + ((int)aRelativeTo).ToString(CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        filter = "CDR_DYNAMIC_SWITCH <> '1' and (CDR_RELATIVE_TO = 0 OR CDR_RELATIVE_TO = " + ((int)aRelativeTo).ToString(CultureInfo.CurrentUICulture) + ")";
                    }
                    DataTable dtFiltered = _dtDateRangesWithNames.Clone();


                    foreach (DataRow dr in _dtDateRangesWithNames.Select(filter))
                    {
                        //	object[] drVals = {dr[0], dr[1], dr[2]};
                        object[] drVals = { dr["CDR_RID"], dr["CDR_START"], dr["CDR_END"], dr["CDR_RANGE_TYPE_ID"], dr["CDR_DATE_TYPE_ID"], dr["CDR_RELATIVE_TO"], dr["CDR_NAME"] };
                        dtFiltered.LoadDataRow(drVals, false);
                    }

                    DataView dvFiltered = new DataView(dtFiltered, "", "CDR_NAME", DataViewRowState.CurrentRows);
                    DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                    PrimaryKeyColumn[0] = dvFiltered.Table.Columns["CDR_NAME"];
                    dvFiltered.Table.PrimaryKey = PrimaryKeyColumn;

                    return dvFiltered;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public ProfileList GetDateRangeDays(DateRangeProfile dateRange, Profile anchorDate)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    DayProfile anchorDay = null;
                    WeekProfile anchorWeek = null;
                    PeriodProfile anchorPeriod = null;

                    if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
                            anchorDate = this.CurrentWeek;
                    }


                    if (anchorDate != null)
                    {
                        switch (anchorDate.ProfileType)
                        {
                            case eProfileType.Day:
                                anchorDay = (DayProfile)anchorDate;
                                anchorWeek = GetWeek(((DayProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((DayProfile)anchorDate).Date);
                                break;
                            case eProfileType.Week:
                                anchorWeek = (WeekProfile)anchorDate;
                                anchorDay = GetDay(((WeekProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((WeekProfile)anchorDate).Date);
                                break;
                            case eProfileType.Period:
                                anchorPeriod = (PeriodProfile)anchorDate;
                                anchorDay = GetDay(((PeriodProfile)anchorDate).Date);
                                anchorWeek = GetWeek(((PeriodProfile)anchorDate).Date);
                                break;
                        }

                    }

                    DayProfile startDay = null;
                    DayProfile endDay = null;

                    if (dateRange.SelectedDateType == eCalendarDateType.Day)
                    {
                        DayProfile startDt;
                        DayProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticDay(dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticDay(anchorDay, dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(anchorDay, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetDay(dateRange.StartDateKey);
                            endDt = GetDay(dateRange.EndDateKey);
                        }
                        startDay = startDt;
                        endDay = endDt;
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                    {
                        WeekProfile startDt;
                        WeekProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticWeek(dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticWeek(anchorWeek, dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(anchorWeek, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetWeek(dateRange.StartDateKey);
                            endDt = GetWeek(dateRange.EndDateKey);
                        }
                        startDay = GetDay(startDt.ProfileStartDate);
                        endDay = GetDay(endDt.ProfileStartDate);
                        endDay = Add(endDay, 6);
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                    {
                        PeriodProfile startDt;
                        PeriodProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticPeriod(dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticPeriod(anchorPeriod, dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(anchorPeriod, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetPeriod(dateRange.StartDateKey);
                            endDt = GetPeriod(dateRange.EndDateKey);
                        }
                        startDay = GetDay(startDt.ProfileStartDate);
                        endDay = GetDay(endDt.ProfileStartDate);
                        endDay = Add(endDay, 6);
                    }

                    ProfileList dayList = GetDayRange(startDay, endDay);

                    return dayList;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList of WeekProfiles for the requested date range.
		/// Dynamic Dates will be figured off of Current Date unless the 
		/// anchorDate is set to something other than null.
		/// </summary>
		/// <param name="dateRange">DateRangeProfile</param>
		/// <returns></returns>
		public ProfileList GetDateRangeWeeks(DateRangeProfile dateRange, Profile anchorDate)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    DayProfile anchorDay = null;
                    WeekProfile anchorWeek = null;
                    PeriodProfile anchorPeriod = null;

                    if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
                            anchorDate = this.CurrentWeek;
                    }

                    if (anchorDate != null)
                    {
                        switch (anchorDate.ProfileType)
                        {
                            case eProfileType.Day:
                                anchorDay = (DayProfile)anchorDate;
                                anchorWeek = GetWeek(((DayProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((DayProfile)anchorDate).Date);
                                break;
                            case eProfileType.Week:
                                anchorWeek = (WeekProfile)anchorDate;
                                anchorDay = GetDay(((WeekProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((WeekProfile)anchorDate).Date);
                                break;
                            case eProfileType.Period:
                                anchorPeriod = (PeriodProfile)anchorDate;
                                anchorDay = GetDay(((PeriodProfile)anchorDate).Date);
                                anchorWeek = GetWeek(((PeriodProfile)anchorDate).Date);
                                break;
                        }

                    }

                    WeekProfile startWeek = null;
                    WeekProfile endWeek = null;

                    if (dateRange.SelectedDateType == eCalendarDateType.Day)
                    {
                        DayProfile startDt;
                        DayProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticDay(dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticDay(anchorDay, dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(anchorDay, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetDay(dateRange.StartDateKey);
                            endDt = GetDay(dateRange.EndDateKey);
                        }
                        startWeek = GetWeek(startDt.Date);
                        endWeek = GetWeek(endDt.Date);
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                    {
                        WeekProfile startDt;
                        WeekProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticWeek(dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticWeek(anchorWeek, dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(anchorWeek, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetWeek(dateRange.StartDateKey);
                            endDt = GetWeek(dateRange.EndDateKey);
                            // BEGIN TT#1571 - stodd - recurring end date is proir to start date
                            if (dateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                if (dateRange.EndDateKey < dateRange.StartDateKey)
                                {
                                    int year = endDt.YearWeek / 100;
                                    int week = endDt.YearWeek - (year * 100);
                                    year++;
                                    endDt = GetWeek(year, week);
                                }
                            }
                            // END TT#1571 - stodd - recurring end date is proir to start date
                        }
                        startWeek = startDt;
                        endWeek = endDt;
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                    {
                        PeriodProfile startDt;
                        PeriodProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticPeriod(dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticPeriod(anchorPeriod, dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(anchorPeriod, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetPeriod(dateRange.StartDateKey);
                            endDt = GetPeriod(dateRange.EndDateKey);
                            // BEGIN TT#1571 - stodd - recurring end date is proir to start date
                            if (dateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                if (dateRange.EndDateKey < dateRange.StartDateKey)
                                {
                                    try
                                    {
                                        int year = 0;
                                        int month = 0;
                                        switch (endDt.PeriodProfileType)
                                        {
                                            //case eProfileType.Year:
                                            //    year = endDt.YearPeriod;
                                            //    year++;
                                            //    endDt = GetPeriod(year);
                                            //    break;
                                            //case eProfileType.Season:
                                            //    year = endDt.YearPeriod / 10;
                                            //    period = endDt.YearPeriod - (year * 10);
                                            //    year++;
                                            //    newPeriod = int.Parse(year.ToString("0000") + period.ToString());
                                            //    endDt = GetPeriod(newPeriod);
                                            //    break;
                                            //case eProfileType.Quarter:
                                            //    year = endDt.YearPeriod / 100;
                                            //    period = endDt.YearPeriod - (year * 100);
                                            //    year++;
                                            //    newPeriod = int.Parse(year.ToString("0000") + period.ToString("00"));
                                            //    endDt = GetPeriod(newPeriod);
                                            //    break;
                                            case eProfileType.Month:
                                                year = endDt.YearPeriod / 100;
                                                month = endDt.YearPeriod - (year * 100);
                                                year++;
                                                endDt = GetPeriod(year, month);
                                                break;
                                            default:
                                                throw new Exception("Invalid PeriodProfileType in GetDateRangeWeeks()");
                                            //break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string message = ex.ToString();
                                        throw;
                                    }
                                }
                            }
                            // END TT#1571 - stodd - recurring end date is proir to start date
                        }
                        startWeek = (WeekProfile)startDt.Weeks[0];
                        endWeek = (WeekProfile)endDt.Weeks[endDt.NoOfWeeks - 1]; // Issue 5121
                    }

                    ProfileList weekList = GetWeekRange(startWeek, endWeek);

                    return weekList;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList of PeriodProfiles for the requested date range.
		/// Dynamic Dates will be figured off of Current Date unless the 
		/// anchorDate is set to something other than null.
		/// </summary>
		/// <param name="dateRange"></param>
		/// <param name="anchorDate"></param>
		/// <returns></returns>
		public ProfileList GetDateRangePeriods(DateRangeProfile dateRange, Profile anchorDate)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    DayProfile anchorDay = null;
                    WeekProfile anchorWeek = null;
                    PeriodProfile anchorPeriod = null;

                    if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
                            anchorDate = this.CurrentWeek;
                    }

                    if (anchorDate != null)
                    {
                        switch (anchorDate.ProfileType)
                        {
                            case eProfileType.Day:
                                anchorDay = (DayProfile)anchorDate;
                                anchorWeek = GetWeek(((DayProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((DayProfile)anchorDate).Date);
                                break;
                            case eProfileType.Week:
                                anchorWeek = (WeekProfile)anchorDate;
                                anchorDay = GetDay(((WeekProfile)anchorDate).Date);
                                anchorPeriod = GetPeriod(((WeekProfile)anchorDate).Date);
                                break;
                            case eProfileType.Period:
                                anchorPeriod = (PeriodProfile)anchorDate;
                                anchorDay = GetDay(((PeriodProfile)anchorDate).Date);
                                anchorWeek = GetWeek(((PeriodProfile)anchorDate).Date);
                                break;
                        }

                    }

                    PeriodProfile startPeriod = null;
                    PeriodProfile endPeriod = null;

                    if (dateRange.SelectedDateType == eCalendarDateType.Day)
                    {
                        DayProfile startDt;
                        DayProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticDay(dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticDay(anchorDay, dateRange.StartDateKey);
                                endDt = ConvertToStaticDay(anchorDay, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetDay(dateRange.StartDateKey);
                            endDt = GetDay(dateRange.EndDateKey);
                        }
                        startPeriod = GetPeriod(startDt.Date);
                        endPeriod = GetPeriod(endDt.Date);
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                    {
                        WeekProfile startDt;
                        WeekProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticWeek(dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticWeek(anchorWeek, dateRange.StartDateKey);
                                endDt = ConvertToStaticWeek(anchorWeek, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetWeek(dateRange.StartDateKey);
                            endDt = GetWeek(dateRange.EndDateKey);
                        }
                        startPeriod = GetPeriod(startDt.Date);
                        endPeriod = GetPeriod(endDt.Date);
                    }
                    else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                    {
                        PeriodProfile startDt;
                        PeriodProfile endDt;
                        if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (anchorDay == null)
                            {
                                startDt = ConvertToStaticPeriod(dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(dateRange.EndDateKey);
                            }
                            else
                            {
                                startDt = ConvertToStaticPeriod(anchorPeriod, dateRange.StartDateKey);
                                endDt = ConvertToStaticPeriod(anchorPeriod, dateRange.EndDateKey);
                            }
                        }
                        else
                        {
                            startDt = GetPeriod(dateRange.StartDateKey);
                            endDt = GetPeriod(dateRange.EndDateKey);
                        }
                        startPeriod = startDt;
                        endPeriod = endDt;
                    }

                    ProfileList periodList = GetPeriodRange(startPeriod, endPeriod);

                    return periodList;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a specific year as a YearProfile.
		/// </summary>
		/// <param name="year">int</param>
		/// <returns>YearProfile.</returns>
		public YearProfile GetYear(int year)
		{
			YearProfile mrsYear= null;
			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //if (_yearsByYear.Contains(year))
                    //{
                    //    mrsYear = (YearProfile)_yearsByYear[year];
                    //}
                    //else
                    if (!_yearsByYear.TryGetValue(year, out mrsYear))
                    // End TT#5124 - JSmith - Performance
                    {
                        if (_yearsByYear.ContainsKey(year))
                        {
                            // Begin TT#5124 - JSmith - Performance
                            //mrsYear = (YearProfile)_yearsByYear[year];
                            mrsYear = _yearsByYear[year];
                            // End TT#5124 - JSmith - Performance
                        }
                        else
                        {
                            AddYear(year);
                            // Begin TT#5124 - JSmith - Performance
                            //mrsYear = (YearProfile)_yearsByYear[year];
                            mrsYear = _yearsByYear[year];
                            // End TT#5124 - JSmith - Performance
                        }
                    }
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsYear;
		}

		/// <summary>
		/// Returns a specifi period as a PeriodProfile.
		/// </summary>
		/// <param name="year">int year.</param>
		/// <param name="period">int fiscal period</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile GetPeriod(int year, int month)
		{
			PeriodProfile monthProf = null;
			try
			{
				if (IsValidMonth(month))	
				{
					int hash = year * 100 + month;
					monthProf = GetMonth(hash);
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return (MonthProfile)monthProf;
		}

		/// <summary>
		/// Returns a specific Month as a PeriodProfile.
		/// </summary>
		/// <param name="year">int year.</param>
		/// <param name="period">int fiscal month</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile GetMonth(int year, int month)
		{
			PeriodProfile monthProf = null;
			try
			{
				if (IsValidMonth(month))
				{
					int hash = year * 100 + month;
					monthProf = GetMonth(hash);
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}

			return (MonthProfile)monthProf;
		}

		/// <summary>
		/// Returns a specifi period as a PeriodProfile.
		/// </summary>
		/// <param name="yearPeriod">int YYYYPP or int YYYYDDD Julian</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile GetMonth(int key)
		{
			try
			{

                // Begin TT#5124 - JSmith - Performance
                //PeriodProfile mrsPeriod = null;
                MonthProfile mrsPeriod = null;
                // End TT#5124 - JSmith - Performance
                using (new ReadLock(rw))
                {
                    // for reoccuring dates, the YearPeriod
                    // could just be the period.  If so we
                    // add the current year to it
                    if (key > 0 && key < 54)
                    {
                        key = this.CurrentDate.FiscalYear * 100 + key;
                    }
                    // Key is Fiscal.  Convert to Julian Key; 
                    if (key < 1000000)
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //key = (int)_monthKeyByFiscal[key];
                        key = _monthKeyByFiscal[key];
                        // End TT#5124 - JSmith - Performance
                    }
                    try
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //if (_monthList.ContainsKey(key))
                        //{
                        //    mrsPeriod = (PeriodProfile)_monthList[key];
                        //}
                        //else
                        if (!_monthList.TryGetValue(key, out mrsPeriod))
                        // End TT#5124 - JSmith - Performance
                        {
                            int year = key / 1000;

                            //=====================================================================================
                            // Many times julian dates fall outside of the same fiscal year.
                            // to catch this we either go back and extra year or go forward and extra year
                            // and add those years to the calendar.
                            //=====================================================================================
                            if (year < DateTime.Now.Year)
                                AddYear(--year);
                            else
                                AddYear(++year);
                            // Begin TT#5124 - JSmith - Performance
                            //mrsPeriod = (PeriodProfile)_monthList[key];
                            //// find the period if not found
                            //if (mrsPeriod == null)
                            if (!_monthList.TryGetValue(key, out mrsPeriod))
                            // End TT#5124 - JSmith - Performance
                            {
                                // Begin TT#5124 - JSmith - Performance
                                //foreach (DictionaryEntry entry in _monthList)
                                //{
                                //    int periodKey = (int)entry.Key;
                                //    PeriodProfile period = (PeriodProfile)entry.Value;
                                //    if (key <= periodKey)
                                //    {
                                //        mrsPeriod = period;
                                //        break;
                                //    }
                                //}
                                foreach (KeyValuePair<int, MonthProfile> entry in _monthList)
                                {
                                    int periodKey = entry.Key;
                                    MonthProfile period = entry.Value;
                                    if (key <= periodKey)
                                    {
                                        // Begin TT#5124 - JSmith - Performance
                                        //mrsPeriod = (PeriodProfile)period;
                                        mrsPeriod = period;
                                        // End TT#5124 - JSmith - Performance
                                        break;
                                    }
                                }
                                // End TT#5124 - JSmith - Performance
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }

				return mrsPeriod;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a specific period as a PeriodProfile.
		/// </summary>
		/// <param name="lookupPeriod">PeriodProfile containing fiscal year and fiscal period.</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile GetPeriod(PeriodProfile lookupPeriod)
		{
            // Begin TT#5124 - JSmith - Performance
            //PeriodProfile mrsPeriod = null;
            MonthProfile mrsPeriod = null;
            // End TT#5124 - JSmith - Performance
			try
			{
                using (new ReadLock(rw))
                {
                    int hash = lookupPeriod.FiscalYear * 100 + lookupPeriod.FiscalPeriod;
                    // Begin TT#5124 - JSmith - Performance
                    //if (_monthList.ContainsKey(hash))
                    //{
                    //    mrsPeriod = (PeriodProfile)_monthList[hash];
                    //}
                    //else
                    //{
                    //    AddYear(lookupPeriod.FiscalYear);
                    //    mrsPeriod = (PeriodProfile)_monthList[hash];
                    //}
                    if (!_monthList.TryGetValue(hash, out mrsPeriod))
                    {
                        AddYear(lookupPeriod.FiscalYear);
                        mrsPeriod = _monthList[hash];
                    }
                    // End TT#5124 - JSmith - Performance
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsPeriod;
		}

		/// <summary>
		/// Returns a specific period as a PeriodProfile.
		/// </summary>
		/// <param name="date">DateTime date</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile GetPeriod(DateTime date)
		{
			PeriodProfile mrsPeriod = null;
			DayProfile DayProfile= null;
			try
			{
                using (new ReadLock(rw))
                {
                    //				int hash = date.Year * 10000 + date.Month * 100 + date.Day;
                    int hash = MIDMath.GregorianHashCode(date);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    DayProfile = (DayProfile)_daysByDate[hash];
                    //    mrsPeriod = DayProfile.Period;
                    //}
                    if (_daysByDate.TryGetValue(hash, out DayProfile))
                    {
                        mrsPeriod = DayProfile.Period;
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        //=====================================================================================
                        // Many times julian dates fall outside of the same fiscal year.
                        // to catch this we either go back and extra year or go forward and extra year
                        // and add those years to the calendar.
                        //=====================================================================================
                        int year = date.Year;
                        if (year < DateTime.Now.Year)
                            AddYear(--year);
                        else
                            AddYear(++year);
                        DayProfile = (DayProfile)_daysByDate[hash];
                        mrsPeriod = DayProfile.Period;
                    }
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsPeriod;
		}

        public ProfileList GetPeriodProfileList(int dateRangeProfileRid)
        {
            ProfileList periodList = new ProfileList(eProfileType.Period);
            ProfileList deletePeriodList = new ProfileList(eProfileType.Period);

            using (new WriteLock(rw))
            {
                // Begin TT#5124 - JSmith - Performance
                //_years = new SortedList();
                //_seasons = new SortedList();
                //_quarters = new SortedList();
                //_months = new SortedList();
                _years = new SortedList<int, YearProfile>();
                _seasons = new SortedList<int, SeasonProfile>();
                _quarters = new SortedList<int, QuarterProfile>();
                _months = new SortedList<int, MonthProfile>();
                // Begin TT#5124 - JSmith - Performance
                try
                {
                    DateRangeProfile dateRange = GetDateRange(dateRangeProfileRid);
                    ProfileList weekRange = GetWeekRange(dateRange, null);
                    foreach (WeekProfile week in weekRange)
                    {
                        BuildPeriodsForWeek(week);
                    }

                    //=============================================================================================
                    // Remove unwanted Seasons
                    // Loops through the years and removes any seasons that are not found in the _seasons array.
                    // The seasons to be deleted are placed in the deletePeriodList. Then this list is processed 
                    // and the seasons are removed from the year.
                    //=============================================================================================
                    for (int y = 0; y < _years.Count; y++)
                    {
                        deletePeriodList.Clear();
                        // Begin TT#5124 - JSmith - Performance
                        //YearProfile aYear = (YearProfile)_years.GetByIndex(y);
                        YearProfile aYear = _years.Values[y];
                        // End TT#5124 - JSmith - Performance
                        foreach (SeasonProfile sp in aYear.ChildPeriodList.ArrayList)
                        {
                            if (!_seasons.ContainsKey(sp.BeginDateJulian))
                                deletePeriodList.Add(sp);
                        }
                        foreach (SeasonProfile sp in deletePeriodList.ArrayList)
                        {
                            aYear.ChildPeriodList.Remove(sp);
                        }
                    }
                    //=============================================================================================
                    // Remove unwanted Quarters
                    //=============================================================================================
                    foreach (YearProfile yp in _years.Values)
                    {
                        foreach (SeasonProfile sp in yp.ChildPeriodList.ArrayList)
                        {
                            deletePeriodList.Clear();
                            foreach (QuarterProfile qp in sp.ChildPeriodList.ArrayList)
                            {
                                if (!_quarters.ContainsKey(qp.BeginDateJulian))
                                    deletePeriodList.Add(qp);
                            }
                            foreach (QuarterProfile qp in deletePeriodList.ArrayList)
                            {
                                sp.ChildPeriodList.Remove(qp);
                            }
                        }
                    }
                    //=============================================================================================
                    // Remove unwanted Months
                    //=============================================================================================
                    foreach (YearProfile yp in _years.Values)
                    {
                        foreach (SeasonProfile sp in yp.ChildPeriodList.ArrayList)
                        {
                            foreach (QuarterProfile qp in sp.ChildPeriodList.ArrayList)
                            {
                                deletePeriodList.Clear();
                                foreach (MonthProfile mp in qp.ChildPeriodList.ArrayList)
                                {
                                    if (!_months.ContainsKey(mp.BeginDateJulian))
                                        deletePeriodList.Add(mp);
                                }
                                foreach (MonthProfile mp in deletePeriodList.ArrayList)
                                {
                                    qp.ChildPeriodList.Remove(mp);
                                }
                            }
                        }
                    }

                    //=============================================================================================
                    // Remove unwanted weeks from the months only
                    //=============================================================================================
                    ProfileList deleteWeekList = new ProfileList(eProfileType.Week);
                    foreach (YearProfile yp in _years.Values)
                    {
                        foreach (SeasonProfile sp in yp.ChildPeriodList.ArrayList)
                        {
                            foreach (QuarterProfile qp in sp.ChildPeriodList.ArrayList)
                            {
                                foreach (MonthProfile mp in qp.ChildPeriodList.ArrayList)
                                {
                                    deletePeriodList.Clear();
                                    foreach (WeekProfile wp in mp.Weeks.ArrayList)
                                    {
                                        WeekProfile wpFromRange = (WeekProfile)weekRange.FindKey(wp.Key);
                                        if (wpFromRange == null)
                                            deleteWeekList.Add(wp);
                                    }
                                    foreach (WeekProfile wp in deleteWeekList.ArrayList)
                                    {
                                        mp.Weeks.Remove(mp);
                                    }
                                }
                            }
                        }
                    }

                    //===============================================
                    // Add the year records to the list we send back
                    //===============================================
                    foreach (YearProfile yp in _years.Values)
                    {
                        periodList.Add(yp);
                    }

                    return periodList;
                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();
                    throw;
                }
            }
        }

		public PeriodProfile BuildPeriodsForWeek(WeekProfile week)
		{
			PeriodProfile period = null;
			YearProfile year = null;
			SeasonProfile season = null;
			QuarterProfile quarter = null;
			MonthProfile month = null;

            try
            {
                using (new WriteLock(rw))
                {
                    month = GetMonthForWeek(week);
                    if (!_months.ContainsKey(month.BeginDateJulian))
                        _months.Add(month.BeginDateJulian, month);

                    quarter = GetQuarterForWeek(week);
                    if (!_quarters.ContainsKey(quarter.BeginDateJulian))
                        _quarters.Add(quarter.BeginDateJulian, quarter);

                    season = GetSeasonForWeek(week);
                    if (!_seasons.ContainsKey(season.BeginDateJulian))
                        _seasons.Add(season.BeginDateJulian, season);

                    year = GetYearForWeek(week);
                    if (!_years.ContainsKey(year.BeginDateJulian))
                        _years.Add(year.BeginDateJulian, year);

                }

                return period;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
		}

		public MonthProfile GetMonthForWeek(WeekProfile week)
		{
			MonthProfile selectedMonth = null;
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //using (new ReadLock(rw))
                //{
                //    if (_monthList.ContainsKey(week.Key))
                //    {
                //        int idx = _monthList.IndexOfKey(week.Key);
                //        MonthProfile tempMonth = (MonthProfile)_monthList.GetByIndex(idx); ;
                //        selectedMonth = (MonthProfile)tempMonth.Clone();
                //    }
                //    else
                //    {
                //        for (int i = 0; i < _monthList.Count; i++)
                //        {
                //            MonthProfile aMonth = (MonthProfile)_monthList.GetByIndex(i);
                //            if (aMonth.BeginDateJulian <= week.Key)
                //                selectedMonth = (MonthProfile)aMonth.Clone();
                //            else
                //                break;
                //        }
                //    }
                //}
                //return selectedMonth;
                using (new ReadLock(rw))
                {
                    if (!_monthList.TryGetValue(week.Key, out selectedMonth))
                    {
                        for (int i = 0; i < _monthList.Count; i++)
                        {
                            if (_monthList.Values[i].BeginDateJulian <= week.Key)
                                selectedMonth = _monthList.Values[i];
                            else
                                break;
                        }
                    }
                }
                return (MonthProfile)selectedMonth.Clone();
                // End TT#5124 - JSmith - Performance
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public QuarterProfile GetQuarterForWeek(WeekProfile week)
		{
			QuarterProfile selectedQuarter = null;
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //using (new ReadLock(rw))
                //{
                //    if (_quarterList.ContainsKey(week.Key))
                //    {
                //        int idx = _quarterList.IndexOfKey(week.Key);
                //        QuarterProfile tempQuater = (QuarterProfile)_quarterList.GetByIndex(idx);
                //        selectedQuarter = (QuarterProfile)tempQuater.Clone();
                //    }
                //    else
                //    {
                //        for (int i = 0; i < _quarterList.Count; i++)
                //        {
                //            QuarterProfile aQuarter = (QuarterProfile)_quarterList.GetByIndex(i);
                //            if (aQuarter.BeginDateJulian <= week.Key)
                //                selectedQuarter = (QuarterProfile)aQuarter.Clone();
                //            else
                //                break;
                //        }
                //    }
                //}
                //return selectedQuarter;
                using (new ReadLock(rw))
                {
                    if (!_quarterList.TryGetValue(week.Key, out selectedQuarter))
                    {
                        for (int i = 0; i < _quarterList.Count; i++)
                        {
                            if (_quarterList.Values[i].BeginDateJulian <= week.Key)
                                selectedQuarter = _quarterList.Values[i];
                            else
                                break;
                        }
                    }
                }
                return (QuarterProfile)selectedQuarter.Clone();
                // End TT#5124 - JSmith - Performance
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public SeasonProfile GetSeasonForWeek(WeekProfile week)
		{
			SeasonProfile selectedSeason = null;
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //using (new ReadLock(rw))
                //{
                //    if (_seasonList.ContainsKey(week.Key))
                //    {
                //        int idx = _seasonList.IndexOfKey(week.Key);
                //        SeasonProfile tempSeason = (SeasonProfile)_seasonList.GetByIndex(idx);
                //        selectedSeason = (SeasonProfile)tempSeason.Clone();
                //    }
                //    else
                //    {
                //        for (int i = 0; i < _seasonList.Count; i++)
                //        {
                //            SeasonProfile aSeason = (SeasonProfile)_seasonList.GetByIndex(i);
                //            if (aSeason.BeginDateJulian <= week.Key)
                //                selectedSeason = (SeasonProfile)aSeason.Clone();
                //            else
                //                break;
                //        }
                //    }
                //}
                //return selectedSeason;
                using (new ReadLock(rw))
                {
                    if (!_seasonList.TryGetValue(week.Key, out selectedSeason))
                    {
                        for (int i = 0; i < _seasonList.Count; i++)
                        {
                            if (_seasonList.Values[i].BeginDateJulian <= week.Key)
                                selectedSeason = _seasonList.Values[i];
                            else
                                break;
                        }
                    }
                }
                return (SeasonProfile)selectedSeason.Clone();
                // End TT#5124 - JSmith - Performance
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public YearProfile GetYearForWeek(WeekProfile week)
		{
			YearProfile selectedYear = null;
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //using (new ReadLock(rw))
                //{
                //    if (_yearList.ContainsKey(week.Key))
                //    {
                //        int idx = _yearList.IndexOfKey(week.Key);
                //        YearProfile tempYear = (YearProfile)_yearList.GetByIndex(idx);
                //        selectedYear = (YearProfile)tempYear.Clone();
                //    }
                //    else
                //    {
                //        for (int i = 0; i < _yearList.Count; i++)
                //        {
                //            YearProfile aYear = (YearProfile)_yearList.GetByIndex(i);
                //            if (aYear.BeginDateJulian <= week.Key)
                //                selectedYear = (YearProfile)aYear.Clone();
                //            else
                //                break;
                //        }
                //    }
                //}
                //return selectedYear;
                using (new ReadLock(rw))
                {
                    if (!_yearList.TryGetValue(week.Key, out selectedYear))
                    {
                        for (int i = 0; i < _yearList.Count; i++)
                        {
                            if (_yearList.Values[i].BeginDateJulian <= week.Key)
                                selectedYear = _yearList.Values[i];
                            else
                                break;
                        }
                    }
                }
                return (YearProfile)selectedYear.Clone();
                // End TT#5124 - JSmith - Performance
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Returns a specific week as a WeekProfile.
		/// </summary>
		/// <param name="year">int Fiscal Year</param>
		/// <param name="period">int Fiscal Period</param>
		/// <param name="weekInPeriod">int week in period</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile GetWeek(int year, int period, int weekInPeriod)
		{
			WeekProfile mrsWeek = null;
			try
			{
				// Get period containing week
				PeriodProfile pp = GetPeriod(year, period);
				// get week in period
				mrsWeek = (WeekProfile)pp.Weeks[weekInPeriod - 1];
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsWeek;
		}

		/// <summary>
		/// Returns a specific week as a WeekProfile.
		/// </summary>
		/// <param name="year">int Fiscal Year</param>
		/// <param name="weekInYear">int Week in year</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile GetWeek(int year, int weekInYear)
		{
			WeekProfile mrsWeek = null;
			int altHash = year * 100 + weekInYear;

			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //if (_weekKeyByFiscal.ContainsKey(altHash))
                    //{
                    //    int weekKey = (int)_weekKeyByFiscal[altHash];
                    //    mrsWeek = (WeekProfile)_weekList[weekKey];
                    //}
                    int weekKey;
                    if (_weekKeyByFiscal.TryGetValue(altHash, out weekKey))
                    {
                        mrsWeek = _weekList[weekKey];
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        AddYear(year);
                        // BEGIN MID Issue # 3582 jsmith - return week 52 if week 53 does not exist
                        //					int weekKey = (int)_weekKeyByFiscal[altHash];
                        // Begin TT#5124 - JSmith - Performance
                        //int weekKey = -1;
                        weekKey = -1;
                        // End TT#5124 - JSmith - Performance
						// Begin TT#5677 - JSmith - Error Message due to Week 53
                        //object weekObject = _weekKeyByFiscal[altHash];
                        //if (weekObject != null)
                        //{
                        //    weekKey = (int)weekObject;
                        //}
                        //else
                        //{
                        //    string weekStr = altHash.ToString();
                        //    if (weekStr.EndsWith("53"))
                        //    {
                        //        // Begin TT#5124 - JSmith - Performance
                        //        //weekKey = (int)_weekKeyByFiscal[altHash - 1];
                        //        weekKey = _weekKeyByFiscal[altHash - 1];
                        //        // End TT#5124 - JSmith - Performance
                        //    }
                        //}
                        if (!_weekKeyByFiscal.TryGetValue(altHash, out weekKey))
                        {
                            if (weekInYear == 53)
                            {
                                altHash = (year + 1) * 100 + 1;
                                weekKey = _weekKeyByFiscal[altHash];
                            }
                        }
						// End TT#5677 - JSmith - Error Message due to Week 53
                        // END MID Issue # 3582
                        // Begin TT#5124 - JSmith - Performance
                        //mrsWeek = (WeekProfile)_weekList[weekKey];
                        mrsWeek = _weekList[weekKey];
                        // End TT#5124 - JSmith - Performance
                    }
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsWeek;
		}


		/// <summary>
		/// returns Week Key (julian YYYYDDD) from fiscal week
		/// </summary>
		/// <param name="fiscalYearWeek"></param>
		/// <returns></returns>
		public int GetWeekKey(int fiscalYearWeek)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    int weekKey = (int)this._weekKeyByFiscal[fiscalYearWeek];
                    return weekKey;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// returns Period Key (julian YYYYDDD) from fiscal period
		/// </summary>
		/// <param name="fiscalYearPeriod"></param>
		/// <returns></returns>
		public int GetPeriodKey(int fiscalYearPeriod)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    int periodKey = (int)this._monthKeyByFiscal[fiscalYearPeriod];
                    return periodKey;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a WeekProfile for the YearWeek given (YYYYWW)
		/// </summary>
		/// <param name="yearWeek"></param>
		/// <returns></returns>
		public WeekProfile GetFiscalWeek(int yearWeek)
		{
			try
			{
				int year = yearWeek / 100;
				int week = yearWeek - (year * 100);

				return GetWeek(year, week);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		int _lastGetWeekKey = int.MinValue;
		WeekProfile _lastGetWeekProfile;

		/// <summary>
		/// Returns a specific week as a WeekProfile.
		/// </summary>
		/// <param name="yearWeek">int YYYYWW</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile GetWeek(int key)
		{
            try
            {
                WeekProfile mrsWeek = null;
                using (new ReadLock(rw))
                {
                    // for reoccuring dates, the YearWeek
                    // could just be the week.  If so we
                    // add the current year to it
                    if (key > 0 && key < 54)
                    {
                        if (key == 53)
                        {
                            int period53 = this._calendarModels.GetWeek53Period(CurrentDate.FiscalYear);
                            if (period53 == 0)
                                key = 52;
                        }
                        key = CurrentDate.FiscalYear * 100 + key;
                    }

                    // Key is Fiscal.  Convert to Julian Key; 
                    if (key < 1000000)
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //key = (int)_weekKeyByFiscal[key];
                        key = _weekKeyByFiscal[key];
                        // End TT#5124 - JSmith - Performance
                    }

                    try
                    {
                        // begin MID Track 6001 Header Load Runs Slow
                        //if (_weekList.ContainsKey(key))
                        //{
                        //	mrsWeek = (WeekProfile)_weekList[key];
                        //}
                        //else
                        if (key == _lastGetWeekKey)
                        {
                            return _lastGetWeekProfile;
                        }
                        // Begin TT#5124 - JSmith - Performance
                        //mrsWeek = (WeekProfile)_weekList[key];
                        mrsWeek = _weekList[key];
                        // End TT#5124 - JSmith - Performance
                        if (mrsWeek == null)
                        // end MID Track 6001 Header Load Runs Slow
                        {
                            int year = key / 1000;

                            //=====================================================================================
                            // Many times julian dates fall outside of the same fiscal year.
                            // to catch this we either go back and extra year or go forward and extra year
                            // and add those years to the calendar.
                            //=====================================================================================
                            if (year < DateTime.Now.Year)
                                AddYear(--year);
                            else
                                AddYear(++year);
                            // Begin TT#5124 - JSmith - Performance
                            //mrsWeek = (WeekProfile)_weekList[key];
                            mrsWeek = _weekList[key];
                            // End TT#5124 - JSmith - Performance
                        }
                        // begin MID Track 6001 Header Load Runs Slow
                        using (new WriteLock(rw))
                        {
                            _lastGetWeekKey = key;
                            _lastGetWeekProfile = mrsWeek;
                        }
                        // end MID Track 6001 Header Load Runs Slow
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }

                return mrsWeek;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
		}

		public WeekProfile GetWeekFromJulianDate(int julianDate)
		{
			WeekProfile mrsWeek = null;

			try
			{
                using (new ReadLock(rw))
                {
                    try
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //if (_weekList.ContainsKey(julianDate))
                        //{
                        //    mrsWeek = (WeekProfile)_weekList[julianDate];
                        //}
                        //else
                        if (!_weekList.TryGetValue(julianDate, out mrsWeek))
                        // End TT#5124 - JSmith - Performance
                        {
                            int year = julianDate / 1000;

                            //=====================================================================================
                            // Many times julian dates fall outside of the same fiscal year.
                            // to catch this we either go back and extra year or go forward and extra year
                            // and add those years to the calendar.
                            //=====================================================================================
                            if (year < DateTime.Now.Year)
                                AddYear(--year);
                            else
                                AddYear(++year);
                            // Begin TT#5124 - JSmith - Performance
                            //mrsWeek = (WeekProfile)_weekList[julianDate];
                            mrsWeek = _weekList[julianDate];
                            // End TT#5124 - JSmith - Performance
                        }
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }

				return mrsWeek;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public WeekProfile GetSameWeekForLastYear(int julianDate)
		{
			WeekProfile tyWeek = GetWeekFromJulianDate(julianDate);
			WeekProfile lyWeek = null;
			try
			{
				lyWeek = this.GetWeek(tyWeek.FiscalYear - 1, tyWeek.WeekInYear);
				if (tyWeek.FiscalYear != lyWeek.FiscalYear)
				{
					YearProfile lyYear = GetYear(lyWeek.FiscalYear);
					if (lyYear.Week53OffsetId == eWeek53Offset.Offset1Week)
					{
						int weekKey = this.AddWeeks(lyWeek.Key, 1);
						lyWeek = GetWeek(weekKey);
					}
				}
				return lyWeek;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}

		}
		// END Added for the Bon Ton calcs stodd 10.22.2007

		// BEGIN Track #6018 stodd - bonton calcs 
		/// <summary>
		/// Returns the same period (Month, Quarter, Season, or Year) for the previous year.
		/// </summary>
		/// <param name="periodRid"></param>
		/// <returns></returns>
		public PeriodProfile GetSamePeriodForLastYear(int periodRid)
		{
			MonthProfile mp = new MonthProfile(Include.NoRID);

			PeriodProfile period = GetPeriod(periodRid);
			eProfileType pType = GetPeriodType(periodRid);
			switch (period.PeriodProfileType)
			{
				case eProfileType.Month:
					PeriodProfile lyMonth = this.GetMonth(period.FiscalYear - 1, period.FiscalPeriod);
					return lyMonth;
                    //break;
				case eProfileType.Quarter:
					int lq = period.FiscalYear - 1;
					int newQKey = (lq * 100) + period.FiscalPeriod;
					PeriodProfile qp = this.GetPeriod(newQKey);
					return qp;
                    //break;
				case eProfileType.Season:
					int ls = period.FiscalYear - 1;
					int newSKey = (ls * 10) + period.FiscalPeriod;
					PeriodProfile sp = this.GetPeriod(newSKey);
					return sp;
                    //break;
				case eProfileType.Year:
					PeriodProfile ly = this.GetPeriod(period.FiscalYear - 1);
					return ly;
                    //break;

				default:
					break;
			}

			return mp;
		}
		// End Track #6018 stodd - bonton calcs 

		/// <summary>
		/// returns the number of weeks that would be added to the fromWeekProfile to
		/// get the toWeekprofile.
		/// </summary>
		/// <param name="fromWeekProfile"></param>
		/// <param name="toWeekProfile"></param>
		/// <returns></returns>
		public int GetWeekOffset(WeekProfile fromWeekProfile, WeekProfile toWeekProfile)
		{
			try
			{
				int offset = 0;
                using (new ReadLock(rw))
                {
                    int fromOffset = _weekList.IndexOfKey(fromWeekProfile.Key);
                    int toOffset = _weekList.IndexOfKey(toWeekProfile.Key);

                    offset = (toOffset - fromOffset);
                }

				return offset;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		///returns the number of weeks that would be added to the fromWeekProfile to
		/// get the toWeekprofile.
		/// </summary>
		/// <param name="fromPeriodProfile"></param>
		/// <param name="toPeriodProfile"></param>
		/// <returns></returns>
		public int GetPeriodOffset(PeriodProfile fromPeriodProfile, PeriodProfile toPeriodProfile)
		{
			try
			{
				int offset = 0;
                using (new ReadLock(rw))
                {
                    int fromOffset = _monthList.IndexOfKey(fromPeriodProfile.Key);
                    int toOffset = _monthList.IndexOfKey(toPeriodProfile.Key);

                    offset = (toOffset - fromOffset);
                }

				return offset;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a specific week as a WeekProfile.
		/// </summary>
		/// <param name="date">DateTime date</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile GetWeek(DateTime date)
		{
			WeekProfile mrsWeek = null;
			DayProfile mrsDay= null;
			try
			{
                using (new ReadLock(rw))
                {
                    int hash = MIDMath.GregorianHashCode(date);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //    mrsWeek = mrsDay.Week;
                    //}
                    // End TT#5124 - JSmith - Performance
                    if (_daysByDate.TryGetValue(hash, out mrsDay))
                    {
                        mrsWeek = mrsDay.Week;
                    }
                    else
                    {
                        //=====================================================================================
                        // Many times julian dates fall outside of the same fiscal year.
                        // to catch this we either go back and extra year or go forward and extra year
                        // and add those years to the calendar.
                        //=====================================================================================
                        int year = date.Year;
                        if (year < DateTime.Now.Year)
                            AddYear(--year);
                        else
                            AddYear(++year);
                        mrsDay = (DayProfile)_daysByDate[hash];
                        mrsWeek = mrsDay.Week;
                    }
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsWeek;
		}

		/// <summary>
		/// Returns an array list of MRSPeriods which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromDate">DateTime</param>
		/// <param name="toDate">DateTime</param>
		/// <returns>ArrayList of MRSPeriods</returns>
		public ProfileList GetPeriodRange(DateTime fromDate, DateTime toDate)
		{
			PeriodProfile fromPeriod = null;
			PeriodProfile toPeriod = null;
			DayProfile mrsDay= null;
			int hash = 0;

			try
			{
                using (new ReadLock(rw))
                {
                    hash = MIDMath.GregorianHashCode(fromDate);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //    fromPeriod = mrsDay.Period;
                    //}
                    if (_daysByDate.TryGetValue(hash, out mrsDay))
                    {
                        fromPeriod = mrsDay.Period;
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CalendarPeriodNotFound,
                            MIDText.GetText(eMIDTextCode.msg_CalendarPeriodNotFound));
                    }
                    //				hash = toDate.Year * 10000 + toDate.Month * 100 + toDate.Day;
                    hash = MIDMath.GregorianHashCode(toDate);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //    toPeriod = mrsDay.Period;
                    //}
                    if (_daysByDate.TryGetValue(hash, out mrsDay))
                    {
                        toPeriod = mrsDay.Period;
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CalendarPeriodNotFound,
                            MIDText.GetText(eMIDTextCode.msg_CalendarPeriodNotFound));
                    }
                    ProfileList periodList = GetPeriodRange(fromPeriod, toPeriod);

                    return periodList;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSPeriods which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromPeriod">PeriodProfile from period</param>
		/// <param name="toPeriod">PeriodProfile to period</param>
		/// <returns>ArrayList of MRSPeriods</returns>
		public ProfileList GetPeriodRange(PeriodProfile fromPeriod, PeriodProfile toPeriod)
		{
            try
            {
                ProfileList periodList = new ProfileList(eProfileType.Period);
                using (new ReadLock(rw))
                {
                    int beginIndex = _monthList.IndexOfKey(fromPeriod.Key);
                    int endIndex = _monthList.IndexOfKey(toPeriod.Key);

                    try
                    {
                        for (int offset = beginIndex; offset <= endIndex; offset++)
                        {
                            // Begin TT#5124 - JSmith - Performance
                            //periodList.Add((PeriodProfile)_monthList.GetByIndex(offset));
                            periodList.Add((PeriodProfile)_monthList.Values[offset]);
                            // End TT#5124 - JSmith - Performance
                        }

                        return periodList;
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
		}

		/// <summary>
		/// Returns an array list of MRSPeriods which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromYear">int fiscal from year</param>
		/// <param name="fromPeriod">int from fiscal period</param>
		/// <param name="toYear">int to fiscal year</param>
		/// <param name="toPeriod">int to fiscal period</param>
		/// <returns>ArrayList of MRSPeriods</returns>
		public ProfileList GetPeriodRange(int fromYear, int fromPeriod, int toYear, int toPeriod)
		{
			try
			{
				PeriodProfile fromMrsPeriod = GetPeriod(fromYear,fromPeriod);
				PeriodProfile toMrsPeriod = GetPeriod(toYear,toPeriod);

				ProfileList periodList = GetPeriodRange(fromMrsPeriod, toMrsPeriod);
				
				return periodList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a profile list of Period Profiles described in the drp.
		/// </summary>
		/// <param name="drp">Date Range Profile you want the period list for.</param>
		/// <param name="pdrp">Plan Date Range Profile (needed if drp is dynamic to plan)</param>
		/// <returns>Period Profile List</returns>
		public ProfileList GetPeriodRange(DateRangeProfile drp, DateRangeProfile pdrp)
		{
			try
			{
				ProfileList periodList = null;

//Begin Track #3972 - JScott - Error opening plan with Current Week in Basis line
//				// if the drp is NOT a specified in PERIODS, throw an error!
//				if (drp.SelectedDateType != eCalendarDateType.Period)
//				{
//					throw new MIDException (eErrorLevel.severe,
//						(int)eMIDTextCode.msg_NoRelativeDateRangeProfile,
//						MIDText.GetText(eMIDTextCode.msg_NoRelativeDateRangeProfile));
//				}
//
//End Track #3972 - JScott - Error opening plan with Current Week in Basis line
				if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
				{
					// get range of periods for PLAN
					ProfileList planPeriodList = GetDateRangePeriods(pdrp, null);
					// Use first week of PLAN as anchor date for figuring out week range
					periodList = GetDateRangePeriods(drp, planPeriodList[0]);
				}
				else
				{
					periodList = GetDateRangePeriods(drp, null);
				}

				return periodList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// This function expects the drp to be a date range profile specifying periods.
		/// </summary>
		/// <param name="drp"></param>
		/// <param name="aDateProfile"></param>
		/// <returns></returns>
		public ProfileList GetPeriodRange(DateRangeProfile drp, Profile aDateProfile)
		{
			try
			{
				ProfileList periodList = null;
				periodList = this.GetDateRangePeriods(drp, aDateProfile);

				return periodList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a profile list of Period Profiles described in the drp.
		/// Also adds to the xRef the drp key/period key for each period
		/// </summary>
		/// <param name="drp">Date Range Profile you want the period list for.</param>
		/// <param name="pdrp">Plan Date Range Profile (needed if drp is dynamic to plan)</param>
		/// <param name="weekXRef"></param>
		/// <returns>Period Profile List</returns>
		//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
		//public ProfileList GetPeriodRange(DateRangeProfile drp, DateRangeProfile pdrp, ProfileXRef periodXRef)
		public ProfileList GetPeriodRange(DateRangeProfile drp, Profile pdrp, ProfileXRef periodXRef)
		//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
		{
			try
			{
				ProfileList periodList = null;
				periodList = GetPeriodRange(drp, pdrp);

				// take weeklist and add each to the weekXRef
				foreach(PeriodProfile pp in periodList)
				{
					periodXRef.AddXRefIdEntry(drp.Key, pp.Key);
				}

				return periodList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSWeeks which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromWeek">WeekProfile from week</param>
		/// <param name="toWeek">WeekProfile to week</param>
		/// <returns>ArrayList of MRSWeeks</returns>
		public ProfileList GetWeekRange(WeekProfile fromWeek, WeekProfile toWeek)
		{
			try
			{
				ProfileList weekList = new ProfileList(eProfileType.Week);
                using (new ReadLock(rw))
                {
                    int beginIndex = _weekList.IndexOfKey(fromWeek.Key);
                    int endIndex = _weekList.IndexOfKey(toWeek.Key);

                    try
                    {
                        for (int offset = beginIndex; offset <= endIndex; offset++)
                        {
                            // Begin TT#5124 - JSmith - Performance
                            //weekList.Add((WeekProfile)_weekList.GetByIndex(offset));
                            weekList.Add(_weekList.Values[offset]);
                            // End TT#5124 - JSmith - Performance
                        }

                        return weekList;
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSWeeks which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromDate">DateTime from date</param>
		/// <param name="toDate">DateTime to date</param>
		/// <returns>ArrayList of MRSWeeks</returns>
		public ProfileList GetWeekRange(DateTime fromDate, DateTime toDate)
		{
			WeekProfile fromWeek = null;
			WeekProfile toWeek = null;
			DayProfile mrsDay= null;
			int hash = 0;

			try
			{
                using (new ReadLock(rw))
                {
                    hash = MIDMath.GregorianHashCode(fromDate);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //    fromWeek = mrsDay.Week;
                    //}
                    if (_daysByDate.TryGetValue(hash, out mrsDay))
                    {
                        fromWeek = mrsDay.Week;
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CalendarWeekNotFound,
                            MIDText.GetText(eMIDTextCode.msg_CalendarWeekNotFound));
                    }
                    //				hash = toDate.Year * 10000 + toDate.Month * 100 + toDate.Day;
                    hash = MIDMath.GregorianHashCode(toDate);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //    toWeek = mrsDay.Week;
                    //}
                    if (_daysByDate.TryGetValue(hash, out mrsDay))
                    {
                        toWeek = mrsDay.Week;
                    }
                    // End TT#5124 - JSmith - Performance
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CalendarWeekNotFound,
                            MIDText.GetText(eMIDTextCode.msg_CalendarWeekNotFound));
                    }
                    ProfileList weekList = GetWeekRange(fromWeek, toWeek);

                    return weekList;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSWeeks which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromYear">int from fiscal year</param>
		/// <param name="fromPeriod">int from fiscal period</param>
		/// <param name="fromWeek">int from fiscal week in period</param>
		/// <param name="toYear">int to fiscal year</param>
		/// <param name="toPeriod">int to fiscdal period</param>
		/// <param name="toWeek">int to fiscal week in period</param>
		/// <returns>ArrayList of WeekProfile</returns>
		public ProfileList GetWeekRange(int fromYear, int fromPeriod, int fromWeek, int toYear, int toPeriod, int toWeek)
		{
			try
			{
				WeekProfile fromMrsWeek = GetWeek(fromYear,fromPeriod, fromWeek);
				WeekProfile toMrsWeek = GetWeek(toYear,toPeriod, toWeek);

				ProfileList weekList = GetWeekRange(fromMrsWeek, toMrsWeek);
				
				return weekList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSWeeks which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromYear">int from  fiscal Year</param>
		/// <param name="fromWeekInYear">int from week in year</param>
		/// <param name="toYear">int to fiscal year</param>
		/// <param name="toWeekInYear">int to week in year</param>
		/// <returns>ArrayList of WeekProfile</returns>
		public ProfileList GetWeekRange(int fromYear, int fromWeekInYear, int toYear, int toWeekInYear)
		{
			try
			{
				WeekProfile fromMrsWeek = GetWeek(fromYear,fromWeekInYear);
				WeekProfile toMrsWeek = GetWeek(toYear,toWeekInYear);

				ProfileList weekList = GetWeekRange(fromMrsWeek, toMrsWeek);
				
				return weekList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSWeeks which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromYearWeek">int from yearWeek YYYYWW</param>
		/// <param name="toYearWeek">int to yearWeek YYYYWW</param>
		/// <returns>ArrayList of WeekProfile</returns>
		public ProfileList GetWeekRange(int fromYearWeek, int toYearWeek)
		{
			try
			{
				WeekProfile fromMrsWeek = GetWeek(fromYearWeek);
				WeekProfile toMrsWeek = GetWeek(toYearWeek);

				ProfileList weekList = GetWeekRange(fromMrsWeek, toMrsWeek);
				
				return weekList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		/// <summary>
		/// reoccurringYear
		/// </summary>
		/// <remarks>
		/// This function expects the drp to be a date range profile specifying weeks.
		/// pdrp is optional.
		/// reoccurringYear is optional.
		/// </remarks>
		/// <param name="drp">Date range profile you want the week list for</param>
		/// <param name="pdrp">DateRangeProfile for the RelativeTo date.  Send null if none needed.</param>
		/// <param name="reoccurringYear">YEar to be used for reoccurring dates.</param>
		/// <returns></returns>
		public ProfileList GetWeekRange(DateRangeProfile drp, DateRangeProfile pdrp, int reoccurringYear)
		{
			try
			{
				ProfileList weekList = null;

                using (new ReadLock(rw))
                {
                    // for reoccuring dates, the YearWeek could just be the week.  If so we
                    // add the reoccurringYear to it to make a YYYYWW..unfortunately that's not the right KEy
                    // we need...
                    if (reoccurringYear > 0 && drp.DateRangeType == eCalendarRangeType.Reoccurring)
                    {
                        if (!_yearsByYear.ContainsKey(reoccurringYear))
                            AddYear(reoccurringYear);

                        int tempKey = 0;
                        if (drp.StartDateKey > 0 && drp.StartDateKey < 54)
                        {
                            tempKey = reoccurringYear * 100 + drp.StartDateKey;
                            // Begin TT#5124 - JSmith - Performance
                            //drp.StartDateKey = (int)_weekKeyByFiscal[tempKey];
                            drp.StartDateKey = _weekKeyByFiscal[tempKey];
                            // End TT#5124 - JSmith - Performance

                        }
                        if (drp.EndDateKey > 0 && drp.EndDateKey < 54)
                        {
                            tempKey = reoccurringYear * 100 + drp.EndDateKey;
                            // Begin TT#5124 - JSmith - Performance
                            //drp.EndDateKey = (int)_weekKeyByFiscal[tempKey];
                            drp.EndDateKey = _weekKeyByFiscal[tempKey];
                            // End TT#5124 - JSmith - Performance
                        }
                    }

                    weekList = GetWeekRange(drp, pdrp);
                }

				return weekList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// This function expects the drp to be a date range profile specifying weeks.
		/// </summary>
		/// <param name="drp"></param>
		/// <param name="aDateProfile"></param>
		/// <returns></returns>
		public ProfileList GetWeekRange(DateRangeProfile drp, Profile aDateProfile)
		{
			try
			{
				ProfileList weekList = null;
				weekList = GetDateRangeWeeks(drp, aDateProfile);

				return weekList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

//Begin Track #4030 - JScott - Filter using Relative Date Range not working correctly
		/// <summary>
		/// This function expects the drp to be a date range profile specifying weeks.
		/// </summary>
		/// <param name="drp"></param>
		/// <param name="aDateProfile"></param>
		/// <param name="weekXRef"></param>
		/// <returns></returns>
		public ProfileList GetWeekRange(DateRangeProfile drp, Profile aDateProfile, ProfileXRef weekXRef)
		{
			try
			{
				ProfileList weekList = null;
				weekList = GetWeekRange(drp, aDateProfile);

				// take weeklist and add each to the weekXRef
				foreach(WeekProfile wp in weekList)
				{
					weekXRef.AddXRefIdEntry(drp.Key, wp.Key);
				}

				return weekList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

//End Track #4030 - JScott - Filter using Relative Date Range not working correctly
		/// <summary>
		/// reoccurringYear.
		/// </summary>
		/// <remarks>
		/// This function expects the drp to be a date range profile specifying weeks
		/// </remarks>
		/// <param name="drp">Date range profile you want the week list for</param>
		/// <param name="pdrp"></param>DateRangeProfile for the RelativeTo date.  Send null if none needed.
		/// <returns></returns>
		public ProfileList GetWeekRange(DateRangeProfile drp, DateRangeProfile pdrp)
		{
			try
			{
				ProfileList weekList = null;
				if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
				{
					if (pdrp == null)
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_NoRelativeDateRangeProfile,
							MIDText.GetText(eMIDTextCode.msg_NoRelativeDateRangeProfile));
					}
					// get range of weeks for PLAN
					ProfileList planWeekList = GetDateRangeWeeks(pdrp, null);
					// Use first week of PLAN as anchor date for figuring out week range
					weekList = GetDateRangeWeeks(drp, planWeekList[0]);
				}
				else
				{
					weekList = GetDateRangeWeeks(drp, null);
				}

				return weekList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList of WeekProfiles designated by the drp.
		/// Also adds to the xRef the drp key/week key for each week
		/// </summary>
		/// <remarks>
		/// This function expects the drp to be a date range profile specifying weeks
		/// </remarks>
		/// <param name="drp">Date range profile you want the week list for</param>
		/// <param name="pdrp"></param>
		/// <param name="weekXRef"></param>
		/// <returns></returns>
		public ProfileList GetWeekRange(DateRangeProfile drp, DateRangeProfile pdrp, ProfileXRef weekXRef)
		{
			try
			{
				ProfileList weekList = null;
				weekList = GetWeekRange(drp, pdrp);

				// take weeklist and add each to the weekXRef
				foreach(WeekProfile wp in weekList)
				{
					weekXRef.AddXRefIdEntry(drp.Key, wp.Key);
				}

				return weekList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a list of weeks in the period
		/// </summary>
		/// <param name="period"></param>
		/// <returns></returns>
		public ProfileList GetWeekRange(PeriodProfile period)
		{
			try
			{
				ProfileList pl = new ProfileList(eProfileType.Week);
		
				foreach (WeekProfile week in period.Weeks.ArrayList)
				{
					pl.Add(week);
				}

				return pl;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Returns a ProfileList of Day profiles
		/// </summary>
		/// <param name="fromDay"></param>
		/// <param name="toDay"></param>
		/// <returns></returns>
		public ProfileList GetDayRange(DayProfile fromDay, DayProfile toDay)
		{
			try
			{
				ProfileList dayList = new ProfileList(eProfileType.Day);
				DayProfile day = fromDay;
			
				try
				{
					while (day <= toDay)
					{
						dayList.Add( day );
						day = Add(day, 1);
					}

					//				dayList.Add( fromDay ); // add first day
					//				for (int offset=fromDay.DayOffset + 1;offset<toDay.DayOffset;offset++)
					//				{
					//					day = Add(day, 1);
					//					dayList.Add( day );
					//				}
					//				dayList.Add( toDay ); // add last day

					return dayList;
				}
				catch ( Exception err )
				{
					string message = err.ToString();
					throw;
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


//		public ProfileList GetReoccurringWeekRange(DateRangeProfile drp, DateRangeProfile pdrp)
//		{
//			ProfileList weekList = null;
//			if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
//			{
//				if (pdrp == null)
//				{
//					throw new System.Exception("Calendar: GetWeekRange failed.  No relative date range profile.");
//				}
//				// get range of weeks for PLAN
//				ProfileList planWeekList = GetDateRangeWeeks(pdrp, null);
//				// Use first week of PLAN as anchor date for figuring out week range
//				weekList = GetDateRangeWeeks(drp, planWeekList[0]);
//			}
//			else
//			{
//				weekList = GetDateRangeWeeks(drp, null);
//			}
//
//			return weekList;
//		}



//		/// <summary>
//		/// Takes the XRef sent and adds to XRef of the Period to the Periods weeks
//		/// </summary>
//		/// <param name="period">period you want the weeks for</param>
//		/// <param name="periodXRef">XRef that it will add the period/week XRef to</param>
//		public void GetWeekXRef(PeriodProfile period, ProfileXRef periodXRef)
//		{
//			
//			//ArrayList wl = (ArrayList)_periodWeekXRef.GetDetailList(period.Key);
//			// adds XRefs for all weeks to the period
//			periodXRef.AddXRefIdEntry(period, period.Weeks);
//		}


		/// <summary>
		/// Returns an array list of MRSDays which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromDate">DateTime from date</param>
		/// <param name="toDate">DateTime to date</param>
		/// <returns>ArrayList of DayProfile</returns>
		public ProfileList DayRange(DateTime fromDate, DateTime toDate)
		{
			try
			{
				ProfileList dayList = new ProfileList(eProfileType.Day);

				try
				{
					for ( DateTime date=fromDate;date<=toDate;date=date.AddDays(1) )
					{
						DayProfile aDay = GetDay(date);
						dayList.Add(aDay);
					}				
					return dayList;
				}
				catch ( Exception err )
				{
					string message = err.ToString();
					throw;
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an array list of MRSDays which includes the From and To dates sent.
		/// </summary>
		/// <param name="fromYearDay">int YYYYDDD</param>
		/// <param name="toYearDay">int YYYYDDD</param>
		/// <returns>ArrayList of DayProfile</returns>
		public ProfileList DayRange(int fromYearDay, int toYearDay)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //DayProfile fromMrsDay = (DayProfile)_dayList[fromYearDay];
                    DayProfile fromMrsDay = _dayList[fromYearDay];
                    // End TT#5124 - JSmith - Performance
                    DateTime fromDate = fromMrsDay.Date;
                    // Begin TT#5124 - JSmith - Performance
                    //DayProfile toMrsDay = (DayProfile)_dayList[toYearDay];
                    DayProfile toMrsDay = _dayList[toYearDay];
                    // End TT#5124 - JSmith - Performance
                    DateTime toDate = toMrsDay.Date;

                    ProfileList dayList = DayRange(fromDate, toDate);

                    return dayList;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a DayProfile for the date requested.
		/// </summary>
		/// <param name="date">DateTime date</param>
		/// <returns>DayProfile</returns>
		public DayProfile GetDay(DateTime date)
		{
			DayProfile mrsDay= null;
			try
			{
                using (new ReadLock(rw))
                {
                    int hash = MIDMath.GregorianHashCode(date);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    //{
                    //    mrsDay = (DayProfile)_daysByDate[hash];
                    //}
                    //else
                    if (!_daysByDate.TryGetValue(hash, out mrsDay))
                    // End TT#5124 - JSmith - Performance
                    {
                        //=====================================================================================
                        // Many times julian dates fall outside of the same fiscal year.
                        // to catch this we either go back and extra year or go forward and extra year
                        // and add those years to the calendar.
                        //=====================================================================================
                        int year = date.Year;
                        if (year < DateTime.Now.Year)
                            AddYear(--year);
                        else
                            AddYear(++year);
                        mrsDay = (DayProfile)_daysByDate[hash];
                    }
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return mrsDay;
		}

		/// <summary>
		/// Returns a DayProfile for the date requested.
		/// </summary>
		/// <param name="yearDay">int YYYYDDD</param>
		/// <returns>DayProfile</returns>
		public DayProfile GetDay(int yearDay)
		{
			try
			{
				DayProfile mrsDay= null;
                using (new ReadLock(rw))
                {
                    // for reoccuring dates, the YearDay
                    // could just be the day.  If so we
                    // add the current year to it
                    if (yearDay > 1 && yearDay < 366)
                    {
                        yearDay = CurrentDate.FiscalYear * 1000 + yearDay;
                    }

                    try
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //if (_dayList.ContainsKey(yearDay))
                        //{
                        //    mrsDay = (DayProfile)_dayList[yearDay];
                        //}
                        //else
                        if (!_dayList.TryGetValue(yearDay, out mrsDay))
                        // End TT#5124 - JSmith - Performance
                        {
                            int year = yearDay / 1000;

                            //=====================================================================================
                            // Many times julian dates fall outside of the same fiscal year.
                            // to catch this we either go back and extra year or go forward and extra year
                            // and add those years to the calendar.
                            //=====================================================================================
                            if (year < DateTime.Now.Year)
                                AddYear(--year);
                            else
                                AddYear(++year);
                            // Begin TT#5124 - JSmith - Performance
                            //mrsDay = (DayProfile)_dayList[yearDay];
                            mrsDay = _dayList[yearDay];
                            // End TT#5124 - JSmith - Performance
                        }
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }

				return mrsDay;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		// BEGIN MID Issue # 3845 John Smith
		/// <summary>
		/// Returns a DayProfile for the date requested.
		/// </summary>
		/// <param name="yearDay">int YYYYDDD</param>
		/// <returns>DayProfile</returns>
		public DayProfile GetFiscalDay(int yearDay)
		{
			try
			{
                using (new ReadLock(rw))
                {
                    // for reoccuring dates, the YearDay
                    // could just be the day.  If so we
                    // add the current year to it
                    if (yearDay > 1 && yearDay < 366)
                    {
                        yearDay = CurrentDate.FiscalYear * 1000 + yearDay;
                    }

                    try
                    {
                        string yyyyddd = yearDay.ToString();
                        YearProfile year = GetYear(Convert.ToInt32(yyyyddd.Substring(0, 4)));
                        DayProfile firstDay = GetDay(year.ProfileStartDate);
                        int day = Convert.ToInt32(yyyyddd.Substring(4, 3));
                        return Add(firstDay, day - 1);
                    }
                    catch (Exception err)
                    {
                        string message = err.ToString();
                        throw;
                    }
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		// END MID Issue # 3845

		/// <summary>
		/// Returns the number of fiscal weeks in a year.
		/// </summary>
		/// <param name="year">int year</param>
		/// <returns>int number of weeks</returns>
		public int GetNumWeeks(int year)
		{
			int numWeeks = 0;
			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //if (!_yearsByYear.Contains(year))
                    //{
                    //    AddYear(year);
                    //}
                    //YearProfile mrsYear = (YearProfile)_yearsByYear[year];
                    YearProfile mrsYear;
                    if (!_yearsByYear.TryGetValue(year, out mrsYear))
                    // End TT#5124 - JSmith - Performance
                    {
                        AddYear(year);
                        mrsYear = _yearsByYear[year];
                    }
                    // End TT#5124 - JSmith - Performance
                    numWeeks = mrsYear.NoOfWeeks;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return numWeeks;
		}

		public int GetNumSeasons(int year)
		{
			int numSeasons = 0;
			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //if (!_yearsByYear.Contains(year))
                    //{
                    //    AddYear(year);
                    //}
                    //YearProfile mrsYear = (YearProfile)_yearsByYear[year];
                    YearProfile mrsYear;
                    if (!_yearsByYear.TryGetValue(year, out mrsYear))
                    {
                        AddYear(year);
                        mrsYear = _yearsByYear[year];
                    }
                    // End TT#5124 - JSmith - Performance
                    numSeasons = mrsYear.ChildPeriodList.Count;
                }
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}

			return numSeasons;
		}

		public int GetNumQuarters(int year)
		{
			int numQuarters = 0;
			try
			{
                using (new ReadLock(rw))
                {
                    // Begin TT#5124 - JSmith - Performance
                    //if (!_yearsByYear.Contains(year))
                    //{
                    //    AddYear(year);
                    //}
                    //YearProfile mrsYear = (YearProfile)_yearsByYear[year];
                    YearProfile mrsYear;
                    if (!_yearsByYear.TryGetValue(year, out mrsYear))
                    {
                        AddYear(year);
                        mrsYear = _yearsByYear[year];
                    }
                    // End TT#5124 - JSmith - Performance
                    foreach (SeasonProfile sp in mrsYear.ChildPeriodList.ArrayList)
                    {
                        numQuarters += sp.ChildPeriodList.Count;
                    }
                }
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}

			return numQuarters;
		}


		/// <summary>
		/// Returns the first day of the DateRangeProfile
		/// </summary>
		/// <param name="CDR_RID"></param>
		/// <returns></returns>
		public DayProfile GetFirstDayOfRange(int CDR_RID)
		{
			try
			{
				DateRangeProfile drp = this.GetDateRange(CDR_RID);
				ProfileList days = this.GetDateRangeDays(drp, null);
				return (DayProfile)days.ArrayList[0];
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		/// <summary>
		/// Returns the first week of the DateRangeProfile
		/// </summary>
		/// <param name="CDR_RID"></param>
		/// <returns></returns>
		public WeekProfile GetFirstWeekOfRange(int CDR_RID)
		{
			try
			{
				DateRangeProfile drp = this.GetDateRange(CDR_RID);
				return GetFirstWeekOfRange(drp);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public WeekProfile GetFirstWeekOfRange(DateRangeProfile drp)
		{
			try
			{
				ProfileList weeks = this.GetDateRangeWeeks(drp, null);
				return (WeekProfile)weeks.ArrayList[0];
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the first period of the DateRangeProfile
		/// </summary>
		/// <param name="CDR_RID"></param>
		/// <returns></returns>
		public PeriodProfile GetFirstPeriodOfRange(int CDR_RID)
		{
			try
			{
				DateRangeProfile drp = this.GetDateRange(CDR_RID);
				ProfileList periods = this.GetDateRangePeriods(drp, null);
				return (PeriodProfile)periods.ArrayList[0];
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the first day of a DateRangeProfile
		/// </summary>
		/// <param name="drp"></param>
		/// <returns></returns>
		public DayProfile GetFirstDayOfRange(DateRangeProfile drp)
		{
			try
			{
				ProfileList days = this.GetDateRangeDays(drp, null);
				return (DayProfile)days.ArrayList[0];
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Takes the dateRangeProfile provided and returns another DRP that
		/// references only the first week of the DRP sent.
		/// Used primarily by OTS Method Screen for TY/LY Trend.
		/// </summary>
		/// <param name="drp"></param>
		/// <returns></returns>
		public DateRangeProfile GetRangeAsFirstWeekOfRange(DateRangeProfile drp)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    DateRangeProfile drp1st = drp.Clone();
                    drp1st.Key = -1;

                    if (drp1st.SelectedDateType == eCalendarDateType.Week)
                    {
                        drp1st.EndDateKey = drp1st.StartDateKey;
                    }
                    else
                    {
                        WeekProfile wp = GetFirstWeekOfRange(drp);
                        if (drp1st.DateRangeType == eCalendarRangeType.Static)
                        {
                            drp1st.StartDateKey = wp.Key;
                            drp1st.EndDateKey = wp.Key;
                            drp1st.SelectedDateType = eCalendarDateType.Week;
                        }
                        else
                        {
                            int offset = 0;
                            if (drp1st.RelativeTo == eDateRangeRelativeTo.Current)
                            {
                                offset = this.ConvertToDynamicWeek(wp.Key);
                            }
                            else
                            {
                                offset = this.ConvertToDynamicWeek((WeekProfile)drp1st.InternalAnchorDate, wp.Key);
                            }

                            drp1st.StartDateKey = offset;
                            drp1st.EndDateKey = offset;
                            drp1st.SelectedDateType = eCalendarDateType.Week;
                        }
                    }

                    drp1st.DisplayDate = this.GetDisplayDate(drp1st);

                    CalendarData.OpenUpdateConnection();
                    int key = CalendarData.CalendarDateRange_Insert(drp1st.StartDateKey, drp1st.EndDateKey, (int)drp1st.DateRangeType,
                        (int)drp1st.SelectedDateType, (int)drp1st.RelativeTo, string.Empty, false, drp1st.DynamicSwitchDate);  //Issue 5171
                    CalendarData.CommitData();
                    CalendarData.CloseUpdateConnection();
                    drp1st.Key = key;

                    return drp1st;
                }   // TT#4481 - JSmith - Object reference error in calendar
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public DateRangeProfile GetSameRangeForLastYear(DateRangeProfile drp)
		{
			try
			{
				drp = GetSameRangeForLastYear(drp, null);

				return drp;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		/// <summary>
		/// Takes the dateRangeProfile provided and returns another DRP that
		/// references the same range of weeks for the previous year.
		/// Used primarily by OTS Method Screen for TY/LY Trend.
		/// </summary>
		/// <param name="drp"></param>
		/// <returns></returns>
		public DateRangeProfile GetSameRangeForLastYear(DateRangeProfile drp, WeekProfile planWeek)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                //using (new ReadLock(rw))
                using (new WriteLock(rw))  
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    DateRangeProfile drpLy = drp.Clone();
                    drpLy.Key = -1;

                    WeekProfile anchorDate = null;
                    if (drpLy.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        if (drpLy.RelativeTo == eDateRangeRelativeTo.Current)
                            anchorDate = this.CurrentWeek;
                        else
                            anchorDate = planWeek;
                    }

                    ProfileList weekList = this.GetDateRangeWeeks(drp, planWeek);
                    WeekProfile firstWeek = (WeekProfile)weekList[0];
                    WeekProfile lastWeek = (WeekProfile)weekList[weekList.Count - 1];
                    WeekProfile lyFirstWeek = this.GetWeek(firstWeek.FiscalYear - 1, firstWeek.WeekInYear);
                    WeekProfile lyLastWeek = this.GetWeek(lastWeek.FiscalYear - 1, lastWeek.WeekInYear);
                    if (firstWeek.FiscalYear != lyFirstWeek.FiscalYear)
                    {
                        YearProfile lyYear = GetYear(lyFirstWeek.FiscalYear);
                        if (lyYear.Week53OffsetId == eWeek53Offset.Offset1Week)
                        {
                            int weekKey = this.AddWeeks(lyFirstWeek.Key, 1);
                            lyFirstWeek = GetWeek(weekKey);
                        }
                    }
                    if (lastWeek.FiscalYear != lyLastWeek.FiscalYear)
                    {
                        YearProfile lyYear = GetYear(lyLastWeek.FiscalYear);
                        if (lyYear.Week53OffsetId == eWeek53Offset.Offset1Week)
                        {
                            int weekKey = this.AddWeeks(lyLastWeek.Key, 1);
                            lyLastWeek = GetWeek(weekKey);
                        }
                    }

                    if (drpLy.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        if (drpLy.SelectedDateType == eCalendarDateType.Week)
                        {
                            if (anchorDate == null)
                                drpLy.StartDateKey = ConvertToDynamicWeek(lyFirstWeek.YearWeek);
                            else
                                drpLy.StartDateKey = ConvertToDynamicWeek(anchorDate, lyFirstWeek.YearWeek);

                            if (anchorDate == null)
                                drpLy.EndDateKey = ConvertToDynamicWeek(lyLastWeek.YearWeek);
                            else
                                drpLy.EndDateKey = ConvertToDynamicWeek(anchorDate, lyLastWeek.YearWeek);
                        }
                        else //Period
                        {
                            if (anchorDate == null)
                                drpLy.StartDateKey = ConvertToDynamicPeriod(lyFirstWeek.Period.YearPeriod);
                            else
                                drpLy.StartDateKey = ConvertToDynamicPeriod(anchorDate, lyFirstWeek.Period.YearPeriod);

                            if (anchorDate == null)
                                drpLy.EndDateKey = ConvertToDynamicPeriod(lyLastWeek.Period.YearPeriod);
                            else
                                drpLy.EndDateKey = ConvertToDynamicPeriod(anchorDate, lyLastWeek.Period.YearPeriod);
                        }
                    }
                    else if (drpLy.DateRangeType == eCalendarRangeType.Static)
                    {
                        if (drpLy.SelectedDateType == eCalendarDateType.Week)
                        {
                            drpLy.StartDateKey = lyFirstWeek.Key;
                            drpLy.EndDateKey = lyLastWeek.Key;
                        }
                        else  //Period
                        {
                            drpLy.StartDateKey = lyFirstWeek.Period.Key;
                            drpLy.EndDateKey = lyLastWeek.Period.Key;
                        }

                        //				// Start Date
                        //				WeekProfile wp = (WeekProfile)this._weekList[drpLy.StartDateKey];
                        //				string aStringDate = wp.YearWeek.ToString(CultureInfo.CurrentUICulture);
                        //				string aStringYear = aStringDate.Substring(0,4);
                        //				string aStringEnd = aStringDate.Substring(4);
                        //				int aYear = Convert.ToInt32(aStringYear);
                        //
                        //				aYear -= 1;
                        //				YearProfile yp = GetYear(aYear);
                        //				//if (yp.Week53OffsetId == eWeek53Offset.Offset1Week)
                        //				aStringDate = aYear.ToString(CultureInfo.CurrentUICulture) + aStringEnd;
                        //				int newYearWeek = Convert.ToInt32(aStringDate, CultureInfo.CurrentUICulture);
                        //				drpLy.StartDateKey = (int)this._weekKeyByFiscal[newYearWeek];
                        //
                        //
                        //				// End Date
                        //				wp = (WeekProfile)this._weekList[drpLy.EndDateKey];
                        //				aStringDate = wp.YearWeek.ToString(CultureInfo.CurrentUICulture);
                        //				aStringYear = aStringDate.Substring(0,4);
                        //				aStringEnd = aStringDate.Substring(4);
                        //				aYear = Convert.ToInt32(aStringYear, CultureInfo.CurrentUICulture);
                        //				aYear -= 1;
                        //				aStringDate = aYear.ToString(CultureInfo.CurrentUICulture) + aStringEnd;
                        //				newYearWeek = Convert.ToInt32(aStringDate, CultureInfo.CurrentUICulture);
                        //				drpLy.EndDateKey = (int)this._weekKeyByFiscal[newYearWeek];
                    }
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_InvalidCalDateRangeProperty,
                            MIDText.GetText(eMIDTextCode.msg_InvalidCalDateRangeProperty));
                    }

                    drpLy.DisplayDate = this.GetDisplayDate(drpLy);

                    CalendarData.OpenUpdateConnection();
                    int key = CalendarData.CalendarDateRange_Insert(drpLy.StartDateKey, drpLy.EndDateKey,
                        (int)drpLy.DateRangeType, (int)drpLy.SelectedDateType, (int)drpLy.RelativeTo, string.Empty, false, drpLy.DynamicSwitchDate);  // Issue 5171
                    CalendarData.CommitData();
                    CalendarData.CloseUpdateConnection();
                    drpLy.Key = key;


                    return drpLy;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		// BEGIN MID Issue # 2790 stodd
		/// <summary>
		/// takes a DateRaangeProfile and converts it to be Dynamic to plan.
		/// </summary>
		/// <param name="drp"></param>
		/// <param name="planDrp"></param>
		/// <returns></returns>
		public DateRangeProfile ConvertToDynamicToPlan(DateRangeProfile drp, DateRangeProfile planDrp)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    if (drp.DateRangeType == eCalendarRangeType.Dynamic
                        && drp.RelativeTo == eDateRangeRelativeTo.Plan)
                    {
                        return drp;
                    }
                    else
                    {
                        DateRangeProfile newDrp = drp.Clone();
                        newDrp.Key = -1;

                        WeekProfile planWeek = GetFirstWeekOfRange(planDrp);
                        newDrp.InternalAnchorDate = planWeek;

                        ProfileList weekList = this.GetDateRangeWeeks(drp, planWeek);
                        WeekProfile firstWeek = (WeekProfile)weekList[0];
                        WeekProfile lastWeek = (WeekProfile)weekList[weekList.Count - 1];

                        newDrp.StartDateKey = ConvertToDynamicWeek(planWeek, firstWeek.YearWeek);
                        newDrp.EndDateKey = ConvertToDynamicWeek(planWeek, lastWeek.YearWeek);
                        newDrp.DateRangeType = eCalendarRangeType.Dynamic;
                        newDrp.RelativeTo = eDateRangeRelativeTo.Plan;

                        newDrp.DisplayDate = this.GetDisplayDate(newDrp);

                        CalendarData.OpenUpdateConnection();
                        int key = CalendarData.CalendarDateRange_Insert(newDrp.StartDateKey, newDrp.EndDateKey, (int)newDrp.DateRangeType,
                            (int)newDrp.SelectedDateType, (int)newDrp.RelativeTo, string.Empty, false, Include.UndefinedDynamicSwitchDate);	// Issue 5171
                        CalendarData.CommitData();
                        CalendarData.CloseUpdateConnection();
                        newDrp.Key = key;

                        return newDrp;
                    }
                }  // TT#4481 - JSmith - Object reference error in calendar
			}
			catch
			{
				throw;
			}
		}
		// END MID Issue # 2790 stodd


		/// <summary>
		/// Returns the first period of the DateRangeProfile
		/// </summary>
		/// <param name="drp"></param>
		/// <returns></returns>
		public PeriodProfile GetFirstPeriodOfRange(DateRangeProfile drp)
		{
			try
			{
				ProfileList periods = this.GetDateRangePeriods(drp, null);
				return (PeriodProfile)periods.ArrayList[0];
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        public string GetDisplayDate(DateRangeProfile dateRange)
        {
            return GetDisplayDate(dateRange, false);
        }
        // End Track #5833

		/// <summary>
		/// from the date range pasted constructs the required display for a user control
		/// </summary>
		/// <param name="dateRange">DateRangeProfile</param>
		/// <returns>String display date</returns>
        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        //public string GetDisplayDate(DateRangeProfile dateRange)
        public string GetDisplayDate(DateRangeProfile dateRange, bool aAnchorDateWasOverridden)
        // End Track #5833
		{
			try
			{
				string displayDate = string.Empty;			
				int startDate = 0, endDate = 0;
                // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                string startDisplayDate = string.Empty;
                string endDisplayDate = string.Empty;
                // End Track #5833

				// if this date range has a name, use it as the display and return
				if (dateRange.Name != null)
				{
					if (dateRange.Name != string.Empty)
					{
						if (dateRange.Name.Length != 0)
						{
							dateRange.DisplayDate = dateRange.Name;
							return dateRange.DisplayDate;
						}
					}
				}

                // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                if (dateRange.DateRangeType == eCalendarRangeType.Dynamic &&
                    aAnchorDateWasOverridden)
                {
                    switch (dateRange.RelativeTo)
                    {
                        case eDateRangeRelativeTo.Current:
                            if (dateRange.SelectedDateType == eCalendarDateType.Day)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_DaysDynamicToCurrentDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_WeeksDynamicToCurrentDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_PeriodsDynamicToCurrentDate);
                            }
                            break;
                        case eDateRangeRelativeTo.Plan:
                            if (dateRange.SelectedDateType == eCalendarDateType.Day)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_DaysDynamicToPlanDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_WeeksDynamicToPlanDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_PeriodsDynamicToPlanDate);
                            }
                            break;
                        case eDateRangeRelativeTo.StoreOpen:
                            if (dateRange.SelectedDateType == eCalendarDateType.Day)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_DaysDynamicToStoreOpenDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Week)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_WeeksDynamicToStoreOpenDate);
                            }
                            else if (dateRange.SelectedDateType == eCalendarDateType.Period)
                            {
                                displayDate = MIDText.GetTextOnly(eMIDTextCode.msg_PeriodsDynamicToStoreOpenDate);
                            }
                            break;
                    }

                    if (dateRange.StartDateKey > 0)
                    {
                        startDisplayDate = "+" + dateRange.StartDateKey.ToString();
                    }
                    else
                    {
                        startDisplayDate = dateRange.StartDateKey.ToString();
                    }

                    if (dateRange.EndDateKey > 0)
                    {
                        endDisplayDate = "+" + dateRange.EndDateKey.ToString();
                    }
                    else
                    {
                        endDisplayDate = dateRange.EndDateKey.ToString();
                    }
                    if (dateRange.StartDateKey == dateRange.EndDateKey)
                    {
                        displayDate = displayDate.Replace("{0}", startDisplayDate);
                    }
                    else
                    {
                        displayDate = displayDate.Replace("{0}", startDisplayDate + " : " + endDisplayDate);
                    }
                    dateRange.DisplayDate = displayDate;
                    return dateRange.DisplayDate;
                }
                // End Track #5833

				if (dateRange.SelectedDateType == eCalendarDateType.Period)
				{
					if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
					{
						if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
						{
                            startDate = ConvertToStaticDate(_currentPeriod, dateRange.StartDateKey, dateRange.SelectedDateType);
                            endDate = ConvertToStaticDate(_currentPeriod, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
						else
						{
							if (dateRange.InternalAnchorDate == null)
								return "Invalid Parameters";
							startDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.StartDateKey, dateRange.SelectedDateType);
							endDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Static)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Reoccurring)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
						displayDate = "Reoccurs Period(s) " + startDate.ToString("00", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate += " - " + endDate.ToString("00", CultureInfo.CurrentUICulture);
						}
					}
			
					if (startDate == 0 && endDate == 0)
						return "Invalid Parameters";

					// only if it's not a reoccurring date do we do this...
					if (dateRange.DateRangeType != eCalendarRangeType.Reoccurring)
					{
						PeriodProfile startPeriod = GetPeriod(startDate);
						PeriodProfile endPeriod = GetPeriod(endDate);
						displayDate = startPeriod.Abbreviation + " " + startPeriod.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate += " - " + endPeriod.Abbreviation + " " + endPeriod.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
						}
					}
				}
				else if (dateRange.SelectedDateType == eCalendarDateType.Week)
				{
					if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
					{
						if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
						{
                            startDate = ConvertToStaticDate(_currentWeek, dateRange.StartDateKey, dateRange.SelectedDateType);
                            endDate = ConvertToStaticDate(_currentWeek, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
						else
						{
							if (dateRange.InternalAnchorDate == null)
								return "Invalid Parameters";
							startDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.StartDateKey, dateRange.SelectedDateType);
							endDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Static)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Reoccurring)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
						displayDate = "RecurringWeek(s) " + startDate.ToString("00", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate += " - " + endDate.ToString("00", CultureInfo.CurrentUICulture);
						}
					}

					if (startDate == 0 && endDate == 0)
						return "Invalid Parameters";

					// do only of its NOT reoccurring
					if (dateRange.DateRangeType != eCalendarRangeType.Reoccurring)
					{
						WeekProfile startWeek = GetWeek(startDate);
						WeekProfile endWeek = GetWeek(endDate);
						displayDate = "Week " + startWeek.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" + startWeek.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate = displayDate.Insert(4,"s");  //change Week to Weeks
							displayDate += " - " + endWeek.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" + endWeek.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
						}
					}
				}
				else if (dateRange.SelectedDateType == eCalendarDateType.Day)
				{
					if (dateRange.DateRangeType == eCalendarRangeType.Dynamic)
					{
						if (dateRange.RelativeTo == eDateRangeRelativeTo.Current)
						{
                            startDate = ConvertToStaticDate(_currentDate, dateRange.StartDateKey, dateRange.SelectedDateType);
                            endDate = ConvertToStaticDate(_currentDate, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
						else
						{
							if (dateRange.InternalAnchorDate == null)
								return "Invalid Parameters";
							startDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.StartDateKey, dateRange.SelectedDateType);
							endDate = ConvertToStaticDate(dateRange.InternalAnchorDate, dateRange.EndDateKey, dateRange.SelectedDateType);
						}
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Static)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
					}
					else if (dateRange.DateRangeType == eCalendarRangeType.Reoccurring)
					{
						startDate = dateRange.StartDateKey;
						endDate = dateRange.EndDateKey;
						displayDate = "RecurringDay(s) " + startDate.ToString("000", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate += " - " + endDate.ToString("000", CultureInfo.CurrentUICulture);
						}
					}

					if (startDate == 0 && endDate == 0)
						return "Invalid Parameters";

					// do only if NOT reoccurring
					if (dateRange.DateRangeType != eCalendarRangeType.Reoccurring)
					{
						DayProfile startDay = GetDay(startDate);
						DayProfile endDay = GetDay(endDate);
						displayDate = startDay.Date.ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
						if (startDate != endDate)
						{
							displayDate += " - " + endDay.Date.ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
						}
					}
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_InvalidCalendarDateType,
						MIDText.GetText(eMIDTextCode.msg_InvalidCalendarDateType));
				}

				dateRange.DisplayDate = displayDate;

				return displayDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}	

		/// <summary>
		/// Converts a dynamic date to it's static equivalent and returns it as a DayProfile
		/// </summary>
		/// <param name="dynamicDate"></param>
		/// <returns>DayProfile</returns>
		public DayProfile ConvertToStaticDay(int dynamicDate)
		{
			try
			{
                //this._staticDayAnchor = this._currentDate;
                int yyyyddd = ConvertToStaticDate(_currentDate, dynamicDate, eCalendarDateType.Day);
				return GetDay(yyyyddd);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a dynamic date to it's static equivalent revative to the anchor date and returns it as a DayProfile
		/// </summary>
		/// <param name="anchorDay">DayProfile</param>
		/// <param name="dynamicDate">int</param>
		/// <returns>DayProfile</returns>
		public DayProfile ConvertToStaticDay(DayProfile anchorDay, int dynamicDate)
		{
			try
			{
				//this._staticDayAnchor = anchorDay;
				int yyyyddd = ConvertToStaticDate(anchorDay, dynamicDate, eCalendarDateType.Day);
				return GetDay(yyyyddd);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a dynamic date to it's static equivalent and returns it as a WeekProfile
		/// </summary>
		/// <param name="dynamicDate"></param>
		/// <returns>WeekProfile</returns>
		public WeekProfile ConvertToStaticWeek(int dynamicDate)
		{
			try
			{
                //this._staticWeekAnchor = this._currentWeek;
                int yyyyww = ConvertToStaticDate(_currentWeek, dynamicDate, eCalendarDateType.Week);
				return GetWeek(yyyyww);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a dynamic date to it's static equivalent revative to the anchor date and returns it as a WeekProfile
		/// </summary>
		/// <param name="anchorWeek">WeekProfile</param>
		/// <param name="dynamicDate">int</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile ConvertToStaticWeek(WeekProfile anchorWeek, int dynamicDate)
		{
			try
			{
				//this._staticWeekAnchor = anchorWeek;
				int yyyyww = ConvertToStaticDate(anchorWeek, dynamicDate, eCalendarDateType.Week);
				return GetWeek(yyyyww);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a dynamic date to it's static equivalent and returns it as a PeriodProfile
		/// </summary>
		/// <param name="dynamicDate"></param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile ConvertToStaticPeriod(int dynamicDate)
		{
			try
			{
                //this._staticPeriodAnchor = this._currentPeriod;
                int yyyypp = ConvertToStaticDate(_currentPeriod, dynamicDate, eCalendarDateType.Period);
				return GetMonth(yyyypp);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a dynamic date to it's static equivalent revative to the anchor date and returns it as a PeriodProfile
		/// </summary>
		/// <param name="anchorPeriod">PeriodProfile</param>
		/// <param name="dynamicDate">int</param>
		/// <returns>PeriodProfile</returns>
		public PeriodProfile ConvertToStaticPeriod(PeriodProfile anchorPeriod, int dynamicDate)
		{
			try
			{
				//this._staticPeriodAnchor = anchorPeriod;
				int yyyymm = ConvertToStaticDate(anchorPeriod, dynamicDate, eCalendarDateType.Period);
				return GetMonth(yyyymm);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		/// <summary>
		/// Takes a profile date of any type (Day, Week, Period or Year and converts it to the 
		/// type indicated by the 2nd parameter. 
		/// </summary>
		/// <param name="dateProfile"></param>
		/// <param name="dateType"></param>
		/// <returns></returns>
		public Profile ConvertDateProfile(Profile dateProfile, eCalendarDateType dateType)
		{
			try
			{
				Profile returnProfile = null;
				WeekProfile wp = null;
				PeriodProfile pp = null;

				switch (dateProfile.ProfileType)
				{
					case eProfileType.Day:
						DayProfile dp = (DayProfile)dateProfile;
						if (dateType == eCalendarDateType.Day)
							returnProfile = dateProfile;
						else if (dateType == eCalendarDateType.Week)
							returnProfile = GetWeek(dp.Date);
						else if (dateType == eCalendarDateType.Period)
							returnProfile = GetPeriod(dp.Date);
						else if (dateType == eCalendarDateType.Year)
							returnProfile = GetYear(dp.FiscalYear);
						break;

					case eProfileType.Week:
						wp = (WeekProfile)dateProfile;
						if (dateType == eCalendarDateType.Day)
							returnProfile = (Profile)wp.Days.ArrayList[0];
						else if (dateType == eCalendarDateType.Week)
							returnProfile = dateProfile;
						else if (dateType == eCalendarDateType.Period)
							returnProfile = wp.Period;
						else if (dateType == eCalendarDateType.Year)
							returnProfile = GetYear(wp.FiscalYear);
						break;

					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//case eProfileType.Period:
					//    pp = (PeriodProfile)dateProfile;
					//    if (dateType == eCalendarDateType.Day)
					//    {
					//        wp = (WeekProfile)pp.Weeks.ArrayList[0];
					//        returnProfile = (Profile)wp.Days.ArrayList[0];
					//    }
					//    else if (dateType == eCalendarDateType.Week)
					//        returnProfile = (Profile)pp.Weeks.ArrayList[0];
					//    else if (dateType == eCalendarDateType.Period)
					//        returnProfile = dateProfile;
					//    else if (dateType == eCalendarDateType.Year)
					//        returnProfile = GetYear(pp.FiscalYear);
					//    break;

					//case eProfileType.Year:
					//    YearProfile yp = (YearProfile)dateProfile;
					//    if (dateType == eCalendarDateType.Day)
					//    {
					//        pp = (PeriodProfile)yp.Periods.ArrayList[0];
					//        wp = (WeekProfile)pp.Weeks.ArrayList[0];
					//        returnProfile = (Profile)wp.Days.ArrayList[0];
					//    }
					//    else if (dateType == eCalendarDateType.Week)
					//    {
					//        pp = (PeriodProfile)yp.Periods.ArrayList[0];
					//        returnProfile = (Profile)pp.Weeks.ArrayList[0];
					//    }					
					//    else if (dateType == eCalendarDateType.Period)
					//        returnProfile = (Profile)yp.Periods.ArrayList[0];
					//    else if (dateType == eCalendarDateType.Year)
					//        returnProfile = dateProfile;
					//    break;
					case eProfileType.Period:
						pp = (PeriodProfile)dateProfile;
						if (pp.PeriodProfileType == eProfileType.Year)
						{
							YearProfile yp = (YearProfile)dateProfile;
							if (dateType == eCalendarDateType.Day)
							{
								pp = (PeriodProfile)yp.Periods.ArrayList[0];
								wp = (WeekProfile)pp.Weeks.ArrayList[0];
								returnProfile = (Profile)wp.Days.ArrayList[0];
							}
							else if (dateType == eCalendarDateType.Week)
							{
								pp = (PeriodProfile)yp.Periods.ArrayList[0];
								returnProfile = (Profile)pp.Weeks.ArrayList[0];
							}
							else if (dateType == eCalendarDateType.Period)
								returnProfile = (Profile)yp.Periods.ArrayList[0];
							else if (dateType == eCalendarDateType.Year)
								returnProfile = dateProfile;
						}
						else
						{
							if (dateType == eCalendarDateType.Day)
							{
								wp = (WeekProfile)pp.Weeks.ArrayList[0];
								returnProfile = (Profile)wp.Days.ArrayList[0];
							}
							else if (dateType == eCalendarDateType.Week)
								returnProfile = (Profile)pp.Weeks.ArrayList[0];
							else if (dateType == eCalendarDateType.Period)
								returnProfile = dateProfile;
							else if (dateType == eCalendarDateType.Year)
								returnProfile = GetYear(pp.FiscalYear);
						}
						break;
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals

					default:
						break;
				}

				return returnProfile;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		// BEGIN Issue 4352 stodd 9.21.2007
		/// <summary>
		/// This was added to be able to call the ConvertToStaticDate until a good date is found.
		/// Since we are just working with offsets, it's possble to get an offset that's too far 
		/// in the future or past and not yet contained in the calendar. If this happends the method
		/// will add a year to the calendar and try again until it contains the date needed.
		/// </summary>
		/// <param name="anchorDate"></param>
		/// <param name="dynamicDate"></param>
		/// <param name="dateType"></param>
		/// <returns></returns>
		private int ConvertToStaticDate(Profile anchorDate, int dynamicDate, eCalendarDateType dateType)
		{
			try
			{
				int returnDate = 0;
				bool successful = false;

                using (new ReadLock(rw))
                {
                    do
                    {
                        successful = ConvertToStaticDate(anchorDate, dynamicDate, dateType, ref returnDate);
                        if (!successful)
                        {
                            if (dynamicDate > 0)
                            {
                                AddYear(_lastCalendarFiscalYear + 1);
                            }
                            else
                            {
                                AddYear(_firstCalendarFiscalYear - 1);
                            }
                        }
                    } while (!successful);

                    return returnDate;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// END Issue 4352 stodd 9.21.2007

		/// <summary>
		/// Converts a dynamic date into it's static date equivalent .
		/// returns the KEY of the date found
		/// </summary>
		/// <param name="dynamicDate">int.</param>
		/// <param name="dateType">eCalendarDateType.</param>
		/// <returns></returns>
		/// <remarks>
		/// </remarks>
		private bool ConvertToStaticDate(Profile anchorDate, int dynamicDate, eCalendarDateType dateType, ref int returnDate) // Issue 4352
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string sDate = string.Empty;
                    if (dateType == DataCommon.eCalendarDateType.Period)
                    {
                        PeriodProfile pp = (PeriodProfile)ConvertDateProfile(anchorDate, eCalendarDateType.Period);
                        int index = _monthList.IndexOfKey(pp.Key);
                        int dateOffset = index + dynamicDate;
                        // Begin TT#5124 - JSmith - Performance
                        //PeriodProfile period = (PeriodProfile)_monthList.GetByIndex(dateOffset);
                        PeriodProfile period = (PeriodProfile)_monthList.Values[dateOffset];
                        // End TT#5124 - JSmith - Performance
                        returnDate = period.Key;
                    }
                    else if (dateType == DataCommon.eCalendarDateType.Week)
                    {
                        WeekProfile wp = (WeekProfile)ConvertDateProfile(anchorDate, eCalendarDateType.Week);
                        int index = _weekList.IndexOfKey(wp.Key);
                        int dateOffset = index + dynamicDate;
                        // Begin TT#5124 - JSmith - Performance
                        //WeekProfile week = (WeekProfile)_weekList.GetByIndex(dateOffset);
                        WeekProfile week = _weekList.Values[dateOffset];
                        // End TT#5124 - JSmith - Performance
                        returnDate = week.Key;

                    }
                    else if (dateType == DataCommon.eCalendarDateType.Day)
                    {
                        DayProfile dp = (DayProfile)ConvertDateProfile(anchorDate, eCalendarDateType.Day);
                        TimeSpan ts = new TimeSpan(dynamicDate, 0, 0, 0, 0);
                        DateTime dDate = dp.Date.Add(ts);
                        DayProfile day = GetDay(dDate);
                        returnDate = day.Key;

                    }
                    else
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_InvalidCalendarDateType,
                            MIDText.GetText(eMIDTextCode.msg_InvalidCalendarDateType));
                    }
                }
				
				return true;
			}
			catch(ArgumentOutOfRangeException) // Issue 4352
			{
				return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		/// <summary>
		/// Converts a static date to it's dynamic equivalent and returns it.
		/// </summary>
		/// <param name="yearPeriod">int YYYYPP</param>
		/// <returns>int dynamic date</returns>
		public int ConvertToDynamicPeriod(int yearPeriod)
		{
			try
			{
				int dynamicDate = 0;
                using (new ReadLock(rw))
                {
                    PeriodProfile per = GetMonth(yearPeriod);
                    int perIndex = _monthList.IndexOfKey(per.Key);
                    int CurrIndex = _monthList.IndexOfKey(_currentPeriod.Key);
                    dynamicDate = perIndex - CurrIndex;
                }
				return dynamicDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a static date to it's dynamic equivalent relative to the anchor period and returns it.
		/// </summary>
		/// <param name="anchorPeriod">PeriodProfile</param>
		/// <param name="yearPeriod">int YYYYPP</param>
		/// <returns>int</returns>
		public int ConvertToDynamicPeriod(PeriodProfile anchorPeriod, int yearPeriod)
		{
			try
			{
				int dynamicDate = 0;
                using (new ReadLock(rw))
                {
                    PeriodProfile per = GetMonth(yearPeriod);
                    int perIndex = _monthList.IndexOfKey(per.Key);
                    int anchorIndex = _monthList.IndexOfKey(anchorPeriod.Key);
                    dynamicDate = perIndex - anchorIndex;
                }
				return dynamicDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public int ConvertToDynamicPeriod(WeekProfile anchorWeek, int yearPeriod)
		{
			try
			{
				int dynamicDate = 0;
                using (new ReadLock(rw))
                {
                    PeriodProfile per = GetMonth(yearPeriod);
                    int perIndex = _monthList.IndexOfKey(per.Key);
                    int anchorIndex = _monthList.IndexOfKey(anchorWeek.Period.Key);
                    dynamicDate = perIndex - anchorIndex;
                }
				return dynamicDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a static date to it's dynamic equivalent and returns it.
		/// </summary>
		/// <param name="yearWeek">int YYYYWW</param>
		/// <returns>int dynamic date</returns>
		public int ConvertToDynamicWeek(int yearWeek)
		{
			try
			{
				WeekProfile wk = null;

                using (new ReadLock(rw))
                {
                    if (yearWeek < 1000000)  // YYYYWW format
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //int key = (int)_weekKeyByFiscal[yearWeek];
                        int key = _weekKeyByFiscal[yearWeek];
                        // End TT#5124 - JSmith - Performance
                        wk = GetWeek(key);  // YYYYDDD
                    }
                    else
                    {
                        wk = GetWeek(yearWeek);  // YYYYDDD format (Key)
                    }
                    int dynamicDate = 0;
                    int weekIndex = _weekList.IndexOfKey(wk.Key);
                    int currIndex = _weekList.IndexOfKey(_currentWeek.Key);
                    dynamicDate = weekIndex - currIndex;
                    return dynamicDate;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a static date to it's dynamic equivalent relevant to the anchor week and returns it.
		/// </summary>
		/// <param name="anchorWeek">WeekProfile</param>
		/// <param name="yearWeek">int YYYYWW</param>
		/// <returns>int</returns>
		public int ConvertToDynamicWeek(WeekProfile anchorWeek, int yearWeek)
		{
			try
			{
                
				WeekProfile wk = null;
                using (new ReadLock(rw))
                {
                    if (yearWeek < 1000000)  // YYYYWW format
                    {
                        // Begin TT#5124 - JSmith - Performance
                        //int key = (int)_weekKeyByFiscal[yearWeek];
                        int key = _weekKeyByFiscal[yearWeek];
                        // End TT#5124 - JSmith - Performance
                        wk = GetWeek(key);  // YYYYDDD
                    }
                    else
                    {
                        wk = GetWeek(yearWeek);  // YYYYDDD format (Key)
                    }
                    int dynamicDate = 0;
                    int weekIndex = _weekList.IndexOfKey(wk.Key);
                    int anchorIndex = _weekList.IndexOfKey(anchorWeek.Key);
                    dynamicDate = weekIndex - anchorIndex;
                    return dynamicDate;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a static date to it's dynamic equivalent and returns it.
		/// </summary>
		/// <param name="yearDay">int YYYYDDD</param>
		/// <returns>int dynamic date</returns>
		public int ConvertToDynamicDay(int yearDay)
		{
			try
			{
				int dynamicDate = 0;
                using (new ReadLock(rw))
                {
                    DayProfile day = GetDay(yearDay);
                    int dayIndex = _dayList.IndexOfKey(day.YearDay);
                    int currIndex = _dayList.IndexOfKey(_currentDate.YearDay);
                    dynamicDate = dayIndex - currIndex;
                }
				return dynamicDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Converts a static date to it's dynamic equivalent relevant to the anchor week and returns it.
		/// </summary>
		/// <param name="anchorDay">int DayProfile</param>
		/// <param name="yearDay">int YYYYDDD</param>
		/// <returns>int</returns>
		public int ConvertToDynamicDay(DayProfile anchorDay, int yearDay)
		{
			try
			{
				int dynamicDate = 0;
                using (new ReadLock(rw))
                {
                    DayProfile day = GetDay(yearDay);
                    int dayIndex = _dayList.IndexOfKey(day.YearDay);
                    int anchIndex = _dayList.IndexOfKey(anchorDay.YearDay);
                    dynamicDate = dayIndex - anchIndex;
                }
				return dynamicDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		/// <summary>
		/// Returns a Date Range Profile with the Display Date filled in.
		/// </summary>
		/// <remarks>
		/// if you do not have an anchor date cdr_rid, set the anchor_cdr_rid = Include.UndefinedCalendarDateRange.
		/// </remarks>
		/// <param name="cdr_rid"></param>
		/// <param name="anchor_cdr_rid"></param>
		/// <returns></returns>
		public DateRangeProfile GetDateRange(int cdr_rid, int anchor_cdr_rid)
		{
			try
			{
				DateRangeProfile drp = null;
                using (new ReadLock(rw))
                {
                    if (anchor_cdr_rid == Include.UndefinedCalendarDateRange)
                    {
                        drp = GetDateRange(cdr_rid);
                    }
                    else
                    {
                        Profile anchorDate = null;
                        DateRangeProfile anchorDrp = GetDateRange(anchor_cdr_rid);
                        switch (anchorDrp.SelectedDateType)
                        {
                            case eCalendarDateType.Day:
                                anchorDate = GetFirstDayOfRange(anchor_cdr_rid);
                                break;
                            case eCalendarDateType.Week:
                                anchorDate = GetFirstWeekOfRange(anchor_cdr_rid);
                                break;
                            case eCalendarDateType.Period:
                                anchorDate = GetFirstPeriodOfRange(anchor_cdr_rid);
                                break;
                        }

                        drp = GetDateRange(cdr_rid, anchorDate);
                    }
                }

				return drp;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public DateRangeProfile GetDateRange(int cdr_rid)
		{
			try
			{
				return GetDateRange(cdr_rid, null, true);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public DateRangeProfile GetDateRange(int cdr_rid, bool resolveDynamicSwitchDate)
		{
			try
			{
				return GetDateRange(cdr_rid, null, resolveDynamicSwitchDate);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public DateRangeProfile GetDateRange(int cdr_rid, Profile anchorDate)
		{
			return GetDateRange(cdr_rid, anchorDate, true);
		}

		/// <summary>
		/// reads a specific calendar date range record from the data base and loads it into a
		/// DateRangeProfile class.
		/// </summary>
		/// <param name="cdr_rid">int.</param>
		/// <returns>DateRangeProfile.</returns>
		public DateRangeProfile GetDateRange(int cdr_rid, Profile anchorDate, bool resolveDynamicSwitchDate)
		{
			try
			{
                //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
                //DateRangeProfile dr = new DateRangeProfile(0);
                DateRangeProfile dr = new DateRangeProfile(Include.UndefinedCalendarDateRange);
                //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed


				// We removed Caching because each server has it;s own global which have thier own calendar--so caching won't work.
				//			if (_dateRangeCache.Contains(cdr_rid))
				//			{
				//				dr = (DateRangeProfile)_dateRangeCache[cdr_rid];
				//
				//				_cdrFromCache++;
				////				Debug.WriteLine("From CACHE " + cdr_rid.ToString() + " CA: " + _cdrFromCache.ToString() +
				////					" DB: " + _cdrFromDb.ToString());
				//			}
				//			else
				//			{

                DataTable dtDateRange = _cd.CalendarDateRange_Read(cdr_rid);
				if (dtDateRange.Rows.Count == 1)
				{
					dr.Key = Convert.ToInt32(dtDateRange.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
					dr.StartDateKey = Convert.ToInt32(dtDateRange.Rows[0]["CDR_START"], CultureInfo.CurrentUICulture);
					dr.EndDateKey = Convert.ToInt32(dtDateRange.Rows[0]["CDR_END"], CultureInfo.CurrentUICulture);
					dr.DateRangeType = (eCalendarRangeType)Convert.ToInt32(dtDateRange.Rows[0]["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture);
					dr.SelectedDateType = (eCalendarDateType)Convert.ToInt32(dtDateRange.Rows[0]["CDR_DATE_TYPE_ID"], CultureInfo.CurrentUICulture);
					dr.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(dtDateRange.Rows[0]["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);
				
					if (anchorDate != null)
						dr.InternalAnchorDate = anchorDate;

					if (dtDateRange.Rows[0]["CDR_NAME"] != DBNull.Value)
						dr.Name	= (string)dtDateRange.Rows[0]["CDR_NAME"];
					dr.IsDynamicSwitch = Include.ConvertCharToBool(Convert.ToChar(dtDateRange.Rows[0]["CDR_DYNAMIC_SWITCH"], CultureInfo.CurrentUICulture));
					dr.DynamicSwitchDate = Convert.ToInt32(dtDateRange.Rows[0]["CDR_DYNAMIC_SWITCH_DATE"], CultureInfo.CurrentUICulture);  // Issue 5171

					//================================================
					// Changes for Dynamic Switch Date (Set date)
					//================================================
					if (resolveDynamicSwitchDate & dr.IsDynamicSwitch)
						ResolveDynamicSwitchDate(dr);

                    // Begin TT#5252 - JSmith - Date Range Removed from Min Max tab in Frcst Method - Fatal Error
                    try
                    {
                        GetDisplayDate(dr);
                    }
                    catch
                    {
                        dr.Key = Include.UndefinedCalendarDateRange;
                        dr.DisplayDate = "Invalid date range";
                    }
					// End TT#5252 - JSmith - Date Range Removed from Min Max tab in Frcst Method - Fatal Error

					//					_dateRangeCache.Add(dr.Key, dr);
					//
					//					_cdrFromDb++;
					//					Debug.WriteLine("From DB    " + cdr_rid.ToString() + " CA: " + _cdrFromCache.ToString() +
					//						" DB: " + _cdrFromDb.ToString());
				}
				//			}
				return dr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		
		
		/// <summary>
		/// Dynamic Switch is a combination of a static date and a dynamic date.  When the selected beginning week is 
		/// before or equal to the Current Week, then the date is considered static and will not change.  Once the 
		/// selected beginning week has passed the Current Week, then the date is considered Dynamic and Relative to Current.
		/// </summary>
		/// <param name="dr"></param>
		public void ResolveDynamicSwitchDate(DateRangeProfile dr)
		{
			// BEGIN Issue 5171
			WeekProfile wp;
			if (dr.DynamicSwitchDate == Include.UndefinedDynamicSwitchDate)
			{
				wp = this.GetFirstWeekOfRange(dr);
			}
			else
			{
				wp = this.GetWeek(dr.DynamicSwitchDate);
			}
			// END Issue 5171

			if (wp >= this.CurrentWeek)
			{
				dr.DateRangeType = eCalendarRangeType.Static;
				// BEGIN issue 5255
				if (dr.StartDateKey < 999999)
				{
					switch (dr.SelectedDateType)
					{
						case eCalendarDateType.Day:
							dr.StartDateKey = (ConvertToStaticDay(dr.StartDateKey)).Key;
							dr.EndDateKey = (ConvertToStaticDay(dr.EndDateKey)).Key;
							break;
						case eCalendarDateType.Week:
							dr.StartDateKey = (ConvertToStaticWeek(dr.StartDateKey)).Key;
							dr.EndDateKey = (ConvertToStaticWeek(dr.EndDateKey)).Key;
							break;
						case eCalendarDateType.Period:
							dr.StartDateKey = (ConvertToStaticPeriod(dr.StartDateKey)).Key;
							dr.EndDateKey = (ConvertToStaticPeriod(dr.EndDateKey)).Key;
							break;
					}
				}
				// END Issue 5255
			}
			else
			{
				switch (dr.SelectedDateType)
				{
					case eCalendarDateType.Day:
						ProfileList dayList = this.GetDateRangeDays(dr, null);
						dr.StartDateKey = 0;
						dr.EndDateKey = dayList.ArrayList.Count - 1;
						break;
					case eCalendarDateType.Week:
						ProfileList weekList = this.GetDateRangeWeeks(dr, null);
						// BEGIN Issue 5171
						// This figures out the offset from the dynamic switch day start week
						// so the range moves along when it should.
						WeekProfile wp1 = (WeekProfile)weekList[0];
                        // Begin TT#3442 - JSmith - Dynamic Switch Date Jumped 15 wks ahead
                        //int offsetDays = wp1.Key - wp.Key;
                        TimeSpan span = ((DayProfile)wp1.Days[0]).Date - ((DayProfile)wp.Days[0]).Date;
                        int offsetDays = span.Days;
                        // End TT#3442 - JSmith - Dynamic Switch Date Jumped 15 wks ahead
						int offsetWeeks = offsetDays/7;
						dr.StartDateKey = offsetWeeks;
						// END Issue 5171
						dr.EndDateKey = (weekList.ArrayList.Count - 1) + offsetWeeks;
						break;
					case eCalendarDateType.Period:
						ProfileList periodList = this.GetDateRangePeriods(dr, null);
						dr.StartDateKey = 0;
						dr.EndDateKey = periodList.ArrayList.Count - 1;
						break;
				}
				dr.RelativeTo = eDateRangeRelativeTo.Current;
				dr.DateRangeType = eCalendarRangeType.Dynamic;
			}
		}

		public void ResolveDynamicSwitchDateForSelector(DateRangeProfile dr)
		{
			// BEGIN Issue 5171
			WeekProfile wp;
			if (dr.DynamicSwitchDate == Include.UndefinedDynamicSwitchDate)
			{
				wp = this.GetFirstWeekOfRange(dr);
			}
			else
			{
				wp = this.GetWeek(dr.DynamicSwitchDate);
			}
			// END Issue 5171
			if (wp >= this.CurrentWeek)
			{
				// No Changes in display date needed
			}
			else
			{
				switch (dr.SelectedDateType)
				{
					case eCalendarDateType.Day:
						ProfileList dayList = this.GetDateRangeDays(dr, null);
						//dr.StartDateKey = 0;
						//dr.EndDateKey = dayList.ArrayList.Count - 1;
						dr.StartDateKey = (this.ConvertToStaticDay(0)).Key;
						dr.EndDateKey = (this.ConvertToStaticDay( dayList.ArrayList.Count - 1 )).Key;
						break;
					case eCalendarDateType.Week:
						ProfileList weekList = this.GetDateRangeWeeks(dr, null);
						//dr.StartDateKey = 0;
						//dr.EndDateKey = weekList.ArrayList.Count - 1;
						// BEgin Track #5886 - stodd
						// BEGIN Issue 5171
						// This figures out the offset from the dynamic switch day start week
						// so the range moves along when it should.
						WeekProfile wp1 = (WeekProfile)weekList[0];
						int offsetDays = wp1.Key - wp.Key;
						int remainder = offsetDays % 7;
						int weeks = offsetDays / 7;
						WeekProfile startWeek = this.ConvertToStaticWeek(0);
						WeekProfile endWeek = this.ConvertToStaticWeek(weekList.ArrayList.Count - 1);
						startWeek = this.Add(startWeek, weeks);
						endWeek = this.Add(endWeek, weeks);
						dr.StartDateKey = startWeek.Key;
						dr.EndDateKey = endWeek.Key;
						//dr.StartDateKey = (this.ConvertToStaticWeek(0)).Key + offsetDays;
						//dr.EndDateKey = (this.ConvertToStaticWeek( weekList.ArrayList.Count - 1 )).Key + offsetDays;
						// END Issue 5171
						// END Issue 5886
						break;
					case eCalendarDateType.Period:
						ProfileList periodList = this.GetDateRangePeriods(dr, null);
						//dr.StartDateKey = 0;
						//dr.EndDateKey = periodList.ArrayList.Count - 1;
						dr.StartDateKey = (this.ConvertToStaticPeriod(0)).Key;
						dr.EndDateKey = (this.ConvertToStaticPeriod( periodList.ArrayList.Count - 1 )).Key;
						break;
				}
				//dr.RelativeTo = eDateRangeRelativeTo.Current;
				//dr.DateRangeType = eCalendarRangeType.Dynamic;
			}
		}

		/// <summary>
		/// Caution: this creates and adds a new date range profile to the DB every time it's called.
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public DateRangeProfile GetDateRange(WeekProfile aWeek)
		{
			try
			{
				DateRangeProfile dr = new DateRangeProfile(0);

				dr.Key = 
				dr.StartDateKey = aWeek.Key;
				dr.EndDateKey = aWeek.Key;
				dr.DateRangeType = eCalendarRangeType.Static;
				dr.SelectedDateType = eCalendarDateType.Week;
				dr.RelativeTo = eDateRangeRelativeTo.Current;
				GetDisplayDate(dr);
				this.AddDateRange(dr);
				return dr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Accepts a DateRangeProfile RID, resolves it into a DatRangeProfile (DRP) and then returns a 
		/// clone of that DateRangeProfile as a NEW DRP with a different RID.
		/// Used where you have many references to the same DRP and you find you need one of them changed.
		/// </summary>
		/// <param name="cdr_rid"></param>
		/// <returns></returns>
		public DateRangeProfile GetDateRangeClone(int cdr_rid)
		{
			try
			{
				// Begin Issue 3990 - stodd, check for invalid key
				if (cdr_rid == Include.UndefinedCalendarDateRange
					|| cdr_rid == Include.NoRID)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_UndefinedCalendarDateRange,
						MIDText.GetText(eMIDTextCode.msg_UndefinedCalendarDateRange));
				}
				else
				// end issue 3990
				{
					DateRangeProfile drp = null;
					drp = GetDateRange(cdr_rid, false);	// Issue 5255 stodd
					// Begin Issue 4285 - jsmith, do not clone named date ranges
					if (drp.Name == null ||
						drp.Name.Trim().Length > 0)
					{
						return drp;
					}
					// end issue 4285
					int cloneKey = this.AddDateRange(drp);
					DateRangeProfile cloneDrp = drp.Clone();
					cloneDrp.Key = cloneKey;
					return cloneDrp;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Used by Gen Assortment method to generate a Date Range
		/// that includes the current week and for a specific number
		/// of weeks.
		/// </summary>
		/// <param name="noOfWeeks"></param>
		/// <returns></returns>
		public DateRangeProfile AddDateRangeWithCurrent(int noOfWeeks)
		{
			try
			{
				DateRangeProfile dr = new DateRangeProfile(Include.NoRID);
				dr.DateRangeType = eCalendarRangeType.Dynamic;
				dr.StartDateKey = 0;
				dr.EndDateKey = (0 + (noOfWeeks - 1));
				dr.RelativeTo = eDateRangeRelativeTo.Current;
				dr.SelectedDateType = eCalendarDateType.Week;
				AddDateRange(dr);
				return dr;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds a new calendar date range record to the database using the info found in the DateRangeProfile class.
		/// </summary>
		/// <param name="dr">DateRangeProfile</param>
		/// <returns>int.  CDR_RID.</returns>
		public int AddDateRange(DateRangeProfile dr)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    _cd.OpenUpdateConnection();
                    int RID = 0;

                    int cdt_id = (int)dr.SelectedDateType;
                    // Begin Issue 3998 - stodd, force static dates to have 'none' for relative to.
                    if (dr.DateRangeType == eCalendarRangeType.Static)
                        dr.RelativeTo = eDateRangeRelativeTo.None;
                    // End 3998
                    int cdr_relative_to = (int)dr.RelativeTo;
                    int cdr_range_type = (int)dr.DateRangeType;
                    RID = _cd.CalendarDateRange_Insert(dr.StartDateKey, dr.EndDateKey, cdr_range_type, cdt_id,
                        cdr_relative_to, dr.Name, dr.IsDynamicSwitch, dr.DynamicSwitchDate);
                    _cd.CommitData();
                    _cd.CloseUpdateConnection();
                    dr.Key = RID;

                    RefreshDateRangesWithNames();

                    return RID;
                }  // TT#4481 - JSmith - Object reference error in calendar
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a DateRangeProfile from a range of weeks (YYYYWW, YYYYWW).
		/// </summary>
		/// <param name="fromWeekYear"></param>
		/// <param name="toWeekYear"></param>
		/// <returns></returns>
		public DateRangeProfile AddDateRangeFromWeeks(int fromWeekYear, int toWeekYear)
		{
			//===================
			// Check Parameters
			//===================
			if (fromWeekYear.ToString(CultureInfo.CurrentUICulture).Length != 6
				|| toWeekYear.ToString(CultureInfo.CurrentUICulture).Length != 6)
			{
				string msg = MIDText.GetText(eMIDTextCode.msg_InvalidMethod2Parameters);
				msg = msg.Replace("{0}","AddDateRangeFromWeeks");
				msg = msg.Replace("{1}",fromWeekYear.ToString(CultureInfo.CurrentUICulture));
				msg = msg.Replace("{2}",toWeekYear.ToString(CultureInfo.CurrentUICulture));
				throw new MIDException(eErrorLevel.severe,(int)eMIDTextCode.msg_InvalidMethod2Parameters,msg);
			}

			try
			{
				//=========================================
				// Get FROM Week Profile & TO week profile
				//=========================================
				int fromYear = Convert.ToInt32(fromWeekYear.ToString(CultureInfo.CurrentUICulture).Substring(0,4));
				int fromWeek = Convert.ToInt32(fromWeekYear.ToString(CultureInfo.CurrentUICulture).Substring(4,2));
				WeekProfile fromWeekProfile = this.GetWeek(fromYear, fromWeek);
				int toYear = Convert.ToInt32(toWeekYear.ToString(CultureInfo.CurrentUICulture).Substring(0,4));
				int toWeek = Convert.ToInt32(toWeekYear.ToString(CultureInfo.CurrentUICulture).Substring(4,2));
				WeekProfile toWeekProfile = this.GetWeek(toYear, toWeek);

				//=================================================
				// Build Date Range Profile and add it to Database
				//=================================================
				DateRangeProfile drp = new DateRangeProfile(Include.UndefinedCalendarDateRange);
				drp.StartDateKey = fromWeekProfile.Key;
				drp.EndDateKey = toWeekProfile.Key;
				drp.DateRangeType = eCalendarRangeType.Static;
				drp.SelectedDateType = eCalendarDateType.Week;
				AddDateRange(drp);

				//===============================================
				// Fills in the Text Display Field for the DRP.
				//===============================================
				GetDisplayDate(drp);

				return drp;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a new calendar date range record to the database using the parameters passed in.
		/// </summary>
		/// <param name="cdr_start">int.</param>
		/// <param name="cdr_end">int.</param>
		/// <param name="cdr_range_type">eCalendarRangeType.</param>
		/// <param name="cdt_id">eCalendarDateType.</param>
		/// <param name="cdr_relative_to">bool.</param>
		/// <returns></returns>
		public int AddDateRange(int cdr_start, int cdr_end, eCalendarRangeType cdr_range_type, eCalendarDateType cdt_id, eDateRangeRelativeTo cdr_relative_to, 
			string name, bool isDynamicSwitchDate, int dynamicSwitchDate)	// Issue 51717
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    _cd.OpenUpdateConnection();
                    int RID = 0;

                    // Begin Issue 3998 - stodd, force static dates to have 'none' for relative to.
                    if (cdr_range_type == eCalendarRangeType.Static)
                        cdr_relative_to = eDateRangeRelativeTo.None;
                    // End 3998

                    RID = _cd.CalendarDateRange_Insert(cdr_start, cdr_end, (int)cdr_range_type, (int)cdt_id,
                            (int)cdr_relative_to, name, isDynamicSwitchDate, dynamicSwitchDate);
                    _cd.CommitData();
                    _cd.CloseUpdateConnection();

                    RefreshDateRangesWithNames();

                    return RID;
                }  // TT#4481 - JSmith - Object reference error in calendar
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// deletes a calendar date range record.
		/// </summary>
		/// <param name="cdr_rid">int.</param>
		public void DeleteDateRange(int cdr_rid)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    _cd.OpenUpdateConnection();
                    _cd.CalendarDateRange_Delete(cdr_rid);
                    _cd.CommitData();
                    _cd.CloseUpdateConnection();

                    RefreshDateRangesWithNames();
                }  // TT#4481 - JSmith - Object reference error in calendar
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Updates a date range db record
		/// </summary>
		/// <param name="dr">DateRangeProfile</param>
		public void UpdateDateRange(DateRangeProfile dr)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    _cd.OpenUpdateConnection();

                    int cdt_id = (int)dr.SelectedDateType;
                    int cdr_range_type = (int)dr.DateRangeType;
                    _cd.CalendarDateRange_Update(dr.Key, dr.StartDateKey, dr.EndDateKey, cdr_range_type, cdt_id,
                        (int)dr.RelativeTo, dr.Name, dr.IsDynamicSwitch, dr.DynamicSwitchDate);	// Issue 5171
                    _cd.CommitData();
                    _cd.CloseUpdateConnection();
                }  // TT#4481 - JSmith - Object reference error in calendar

				//			//======================================================
				//			// If CDR has been cached, update the cached value too
				//			//======================================================
				//			if (_dateRangeCache.Contains(dr.Key))
				//			{
				//				_dateRangeCache.Add(dr.Key, dr);
				//			}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}



		//=======================================================================================================
		// This was rememvoed because it wasn't being used and leaving it in would cause difficulties for the 
		// CDR caching.
		//=======================================================================================================
//		/// <summary>
//		/// Update the calendar date range record
//		/// </summary>
//		/// <param name="cdr_rid">int</param>
//		/// <param name="cdr_start">int</param>
//		/// <param name="cdr_end">int</param>
//		/// <param name="cdr_range_type">eCalendarRangeType</param>
//		/// <param name="cdt_id">eCalendarDateType</param>
//		/// <param name="cdr_relative_to">bool</param>
//		public void UpdateDateRange(int cdr_rid, int cdr_start, int cdr_end, eCalendarRangeType cdr_range_type, eCalendarDateType cdt_id, eDateRangeRelativeTo cdr_relative_to, string name)
//		{
        //			_cd.OpenUpdateConnection();
//			
        //			_cd.CalendarDateRange_Update(cdr_rid, cdr_start, cdr_end, (int)cdr_range_type, (int)cdt_id, (int)cdr_relative_to, name);
        //			_cd.CommitData();
        //			_cd.CloseUpdateConnection();
//			//======================================================
//			// If CDR has been cached, update the cached value too
//			//======================================================
//			if (_dateRangeCache.Contains(cdr_rid))
//			{
//				DateRangeProfile dr = this.GetDateRange(
//				_dateRangeCache.Add(cdr_rid, dr);
//			}
//		}

		/// <summary>
		/// Renames the Calendar Date Range
		/// </summary>
		/// <param name="cdr_rid"></param>
		/// <param name="name"></param>
		public void UpdateDateRange(int cdr_rid, string name)
		{
			try
			{
                // Begin TT#4481 - JSmith - Object reference error in calendar
                using (new WriteLock(rw))
                {
                // End TT#4481 - JSmith - Object reference error in calendar
                    _cd.OpenUpdateConnection();
                    _cd.CalendarDateRange_UpdateName(cdr_rid, name);
                    _cd.CommitData();
                    _cd.CloseUpdateConnection();
                }  // TT#4481 - JSmith - Object reference error in calendar

				//			//======================================================
				//			// If CDR has been cached, update the cached value too
				//			//======================================================
				//			if (_dateRangeCache.Contains(cdr_rid))
				//			{
				//				DateRangeProfile dr = (DateRangeProfile)_dateRangeCache[cdr_rid];
				//				dr.Name = name;
				//			}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

        ///// <summary>
        ///// Adds posting date
        ///// </summary>
        ///// <param name="postingDate">DateTime</param>
        ///// <returns>int RID</returns>
        //public int AddPostingDate(DateTime postingDate)
        //{
        //    try
        //    {
        //        _cd.OpenUpdateConnection();
        //        int RID = 0;

        //        RID = _cd.PostingDate_Insert(postingDate);
        //        _cd.CommitData();
        //        _cd.CloseUpdateConnection();
        //        return RID;
        //    }
        //    catch ( Exception ex )
        //    {
        //        string message = ex.ToString();
        //        throw;
        //    }
        //}

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        ///// <summary>
        ///// updates current posting date
        ///// </summary>
        ///// <param name="ph_rid">int</param>
        ///// <param name="postingDate">DateTime</param>
        //public void UpdatePostingDate(int ph_rid, DateTime postingDate)
        //{
        //    try
        //    {
        //        _cd.OpenUpdateConnection();

        //        _cd.PostingDate_Update(ph_rid, postingDate);
        //        _cd.CommitData();
        //        _cd.CloseUpdateConnection();
        //    }
        //    catch ( Exception ex )
        //    {
        //        string message = ex.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function

		/// <summary>
		/// Returns a DayProfile after adding the number of days to the Day Profile sent.
		/// days CAN be negative.
		/// </summary>
		/// <param name="profile">DayProfile</param>
		/// <param name="days">int</param>
		/// <returns>DayProfile</returns>
		public DayProfile Add(DayProfile profile, int days)
		{
			try
			{
				DateTime newDate = profile.Date.AddDays(days);
				DayProfile newDay = this.GetDay(newDate);
				return newDay;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a WeekProfile after adding the number of weeks to the WeekProfile sent.
		/// Weeks CAN be negative.		/// </summary>
		/// <param name="profile">WeekProfile</param>
		/// <param name="weeks">int</param>
		/// <returns>WeekProfile</returns>
		public WeekProfile Add(WeekProfile profile, int weeks)
		{
			try
			{
				int newOffset = 0;
				bool indexNotFound = true;
                using (new ReadLock(rw))
                {
                    // sometimes a request will 
                    while (indexNotFound)
                    {
                        // ANF Performance change
                        //int index = _weekList.IndexOfKey(profile.Key);
                        int index = this._yearBeginIndex[profile.FiscalYear - this.FirstCalendarFiscalYear] + profile.WeekInYear;

                        newOffset = index + weeks;
                        if (newOffset < 0)
                            this.AddYear(this.FirstCalendarFiscalYear - 1);
                        else
                        {
                            // Begin Issue 3922 - stodd
                            if (newOffset >= _weekList.Count)
                            {
                                AddYear(this.LastCalendarFiscalYear + 1);
                            }
                            //End Issue 3922
                            indexNotFound = false;
                        }
                    }
                    // Begin TT#5124 - JSmith - Performance
                    //WeekProfile newWeek = (WeekProfile)_weekList.GetByIndex(newOffset);
                    WeekProfile newWeek = _weekList.Values[newOffset];
                    // End TT#5124 - JSmith - Performance
                    return newWeek;
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a WeekProfile after adding the number of weeks to the WeekProfile KEY sent.
		/// "weeks" can be negative. 
		/// </summary>
		/// <param name="weekProfile_rid"></param>
		/// <param name="weeks"></param>
		/// <returns></returns>
		public int AddWeeks(int weekProfileKey, int weeks)
		{
			try
			{
				WeekProfile wp = GetWeek(weekProfileKey);
				WeekProfile newWeek = Add(wp, weeks);
				return newWeek.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		// BEGIN Track #5984 stodd
		/// <summary>
		/// Adds the indicated number of periods and returns the key to period found.
		/// </summary>
		/// <param name="periodProfileKey"></param>
		/// <param name="periods"></param>
		/// <returns></returns>
		public int AddPeriods(int periodProfileKey, int periods)
		{
            //int newKey = Include.NoRID;
			PeriodProfile newPeriod = null;
			try
			{
				PeriodProfile pp = GetPeriod(periodProfileKey);

				eProfileType profileType = pp.PeriodProfileType;
				switch (profileType)
				{
					case eProfileType.Month:
						newPeriod = Add((MonthProfile)pp, periods);
						break;
					case eProfileType.Quarter:
						newPeriod = Add((QuarterProfile)pp, periods);
						break;
					case eProfileType.Season:
						newPeriod = Add((SeasonProfile)pp, periods);
						break;
					case eProfileType.Year:
						newPeriod = Add((YearProfile)pp, periods);
						break;

					default:
						break;
				}

				return newPeriod.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public int AddMonths(int monthProfileKey, int months)
		{
			try
			{
				MonthProfile mp = (MonthProfile)GetPeriod(monthProfileKey);
				MonthProfile newMonth = Add(mp, months);
				return newMonth.Key;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the Period Profile for the Key. Year=YYYY. Season=YYYYS. Quarter=YYYYQQ. Month=YYYYDDD. 
		/// </summary>
		/// <param name="periodProfileKey"></param>
		/// <returns></returns>
		public PeriodProfile GetPeriod(int periodKey)
		{
			try
			{
				int keyLength = periodKey.ToString().Length;

                using (new ReadLock(rw))
                {
                    switch (keyLength)
                    {
                        // BEGIN TT#1571 - stodd - recurring end date is proir to start date
                        case 1:
                        case 2:
                            int year = CurrentWeek.YearWeek / 100;
                            return (PeriodProfile)GetMonth(year, periodKey);
                        //break; 
                        // END TT#1571 - stodd - recurring end date is proir to start date
                        case 4:
                            return (PeriodProfile)_yearsByYear[periodKey];
                        //break; 
                        case 5:
                            return (PeriodProfile)_seasonKeyByFiscal[periodKey];
                        //break; 
                        case 6:
                            return (PeriodProfile)_quarterKeyByFiscal[periodKey];
                        //break; 
                        case 7:
                            return (PeriodProfile)_monthList[periodKey];
                        //break; 
                        default:
                            break;
                    }
                }

				return new MonthProfile(Include.NoRID);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		// BEGIN Track #6018 stodd
		/// <summary>
		/// Returns the Period type for the Key. Year=YYYY. Season=YYYYS. Quarter=YYYYQQ. Month=YYYYDDD. 
		/// </summary>
		/// <param name="periodProfileKey"></param>
		/// <returns></returns>
		public eProfileType GetPeriodType(int periodKey)
		{
			eProfileType profileType = new eProfileType();
			try
			{
				int keyLength = periodKey.ToString().Length;

				switch (keyLength)
				{
					case 4:
						profileType = eProfileType.Year;
						break;
					case 5:
						profileType = eProfileType.Season;
						break;
					case 6:
						profileType = eProfileType.Quarter;
						break;
					case 7:
						profileType = eProfileType.Month;
						break;
					default:
						string msg = MIDText.GetText(eMIDTextCode.msg_InvalidPeriodProfileKey);
						msg = msg.Replace("{0}", periodKey.ToString()); 
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_InvalidPeriodProfileKey, msg);
                        //break;
				}

				return profileType;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}
		// END Track #6018


		/// <summary>
		/// Returns a PeriodProfile after adding the number of periods to the PeriodProfile sent.
		/// Periods CAN be negative.		/// </summary>
		/// </summary>
		/// <param name="profile">PeriodProfile</param>
		/// <param name="periods">int</param>
		/// <returns>PeriodProfile</returns>
		//public PeriodProfile Add(PeriodProfile profile, int periods)
		//{
		//    try
		//    {
		//        int newOffset = 0;
		//        bool indexNotFound = true;
		//        // sometimes a request will 
		//        while (indexNotFound)
		//        {
		//            int index = _monthList.IndexOfKey(profile.Key);
		//            newOffset = index + periods;
		//            if (newOffset < 0)
		//                this.AddYear(this._firstCalendarFiscalYear-1);
		//            else
		//            {
		//                // Begin Issue 3922 - stodd
		//                if (newOffset >= _monthList.Count)
		//                {
		//                    AddYear(this._lastCalendarFiscalYear + 1);
		//                }
		//                //End Issue 3922
		//                indexNotFound = false;
		//            }
		//        }
		//        PeriodProfile newPeriod = (PeriodProfile)_monthList.GetByIndex(newOffset);
		//        return newPeriod;
		//    }
		//    catch ( Exception ex )
		//    {
		//        string message = ex.ToString();
		//        throw;
		//    }
		//}
		// BEGIN Track #5984 stodd
		/// <summary>
		/// Returns a MonthProfile after adding the number of months to the MonthProfile sent.
		/// months CAN be negative.		/// </summary>
		/// </summary>
		/// <param name="profile">MonthProfile</param>
		/// <param name="periods">int</param>
		/// <returns>MonthProfile</returns>
		public MonthProfile Add(MonthProfile profile, int months)
		{
			try
			{
				int newOffset = 0;
				bool indexNotFound = true;
                using (new ReadLock(rw))
                {
                    // sometimes a request will 
                    while (indexNotFound)
                    {
                        int index = _monthList.IndexOfKey(profile.Key);
                        newOffset = index + months;
                        if (newOffset < 0)
                            this.AddYear(this.FirstCalendarFiscalYear - 1);
                        else
                        {
                            // Begin Issue 3922 - stodd
                            if (newOffset >= _monthList.Count)
                            {
                                AddYear(this.LastCalendarFiscalYear + 1);
                            }
                            //End Issue 3922
                            indexNotFound = false;
                        }
                    }
                    // End TT#5124 - JSmith - Performance
                    //MonthProfile aMonth = (MonthProfile)_monthList.GetByIndex(newOffset);
                    MonthProfile aMonth = _monthList.Values[newOffset];
                    // End TT#5124 - JSmith - Performance
                    return aMonth;
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a QuarterProfile after adding the number of quarters to the QuarterProfile sent.
		/// quarters CAN be negative.		/// </summary>
		/// </summary>
		/// <param name="profile">QuarterProfile</param>
		/// <param name="periods">int</param>
		/// <returns>QuarterProfile</returns>
		public QuarterProfile Add(QuarterProfile profile, int quarters)
		{
			try
			{
				int newOffset = 0;
				bool indexNotFound = true;
                using (new ReadLock(rw))
                {
                    // sometimes a request will 
                    while (indexNotFound)
                    {
                        int index = _sortedQuarterList.IndexOfKey(profile.Key);
                        newOffset = index + quarters;
                        if (newOffset < 0)
                            this.AddYear(this.FirstCalendarFiscalYear - 1);
                        else
                        {
                            // Begin Issue 3922 - stodd
                            if (newOffset >= _sortedQuarterList.Count)
                            {
                                AddYear(this.LastCalendarFiscalYear + 1);
                            }
                            //End Issue 3922
                            indexNotFound = false;
                        }
                    }
                    // Begin TT#5124 - JSmith - Performance
                    //QuarterProfile aQuarter = (QuarterProfile)_sortedQuarterList.GetByIndex(newOffset);
                    QuarterProfile aQuarter = _sortedQuarterList.Values[newOffset];
                    // End TT#5124 - JSmith - Performance
                    return aQuarter;
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a SeasonProfile after adding the number of season to the SeasonProfile sent.
		/// seasons CAN be negative.		/// </summary>
		/// </summary>
		/// <param name="profile">SeasonProfile</param>
		/// <param name="periods">int</param>
		/// <returns>SeasonProfile</returns>
		public SeasonProfile Add(SeasonProfile profile, int seasons)
		{
			try
			{
				int newOffset = 0;
				bool indexNotFound = true;
                using (new ReadLock(rw))
                {
                    // sometimes a request will 
                    while (indexNotFound)
                    {
                        int index = _sortedSeasonList.IndexOfKey(profile.Key);
                        newOffset = index + seasons;
                        if (newOffset < 0)
                            this.AddYear(this.FirstCalendarFiscalYear - 1);
                        else
                        {
                            // Begin Issue 3922 - stodd
                            if (newOffset >= _sortedSeasonList.Count)
                            {
                                AddYear(this.LastCalendarFiscalYear + 1);
                            }
                            //End Issue 3922
                            indexNotFound = false;
                        }
                    }
                    // Begin TT#5124 - JSmith - Performance
                    //SeasonProfile aSeason = (SeasonProfile)_sortedSeasonList.GetByIndex(newOffset);
                    SeasonProfile aSeason = _sortedSeasonList.Values[newOffset];
                    // End TT#5124 - JSmith - Performance
                    return aSeason;
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a YearProfile after adding the number of year to the YearProfile sent.
		/// years CAN be negative.		/// </summary>
		/// </summary>
		/// <param name="profile">YearProfile</param>
		/// <param name="years">int</param>
		/// <returns>YearProfile</returns>
		public YearProfile Add(YearProfile profile, int years)
		{
			try
			{
				int newYear = 0;
				bool yearNotFound = true;
                using (new ReadLock(rw))
                {
                    // sometimes a request will 
                    while (yearNotFound)
                    {
                        newYear = profile.Key + years;
                        if (newYear < FirstCalendarFiscalYear)
                            this.AddYear(this.FirstCalendarFiscalYear - 1);
                        else
                        {
                            // Begin Issue 3922 - stodd
                            if (newYear >= LastCalendarFiscalYear)
                            {
                                AddYear(this.LastCalendarFiscalYear + 1);
                            }
                            //End Issue 3922
                            yearNotFound = false;
                        }
                    }
                    // Begin TT#5124 - JSmith - Performance
                    //YearProfile aYear = (YearProfile)_yearsByYear[newYear];
                    YearProfile aYear = _yearsByYear[newYear];
                    // End TT#5124 - JSmith - Performance
                    return aYear;
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		// END Track #5984 stodd

		public void OpenUpdateConnection()
		{
			try
			{
                _cd.OpenUpdateConnection();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void CommitData()
		{
			try
			{
                _cd.CommitData();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void CloseUpdateConnection()
		{
			try
			{
                _cd.CloseUpdateConnection();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}


		// PARAMETER EDITING
		private bool IsValidMonth(int month)
		{
			try
			{
				if (month < 0 || month > 13)
					return false;
				else
					return true;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		private bool IsValidWeekInYear(int year, int week)
		{
			try
			{
				int maxWeeks = GetNumWeeks(year);
				if (week < 0 || week > maxWeeks)
					return false;
				else
					return true;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
		private bool IsValidDayInYear(int year, int day)
		{
			try
			{
				int maxWeeks = GetNumWeeks(year);
				int maxDays = 7 * maxWeeks;
				if (day < 0 || day > maxDays)
					return false;
				else
					return true;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

        // Begin TT#615-MD - JSmith - Store Load is allowing the adding of older dates, outside of the calendar boundaries.
        /// <summary>
        /// Determines if date is within the loaded Merchandise calendar dates
        /// </summary>
        /// <param name="aDate">The date to look up</param>
        /// <returns></returns>
        public bool IsValidMerchandiseCalendarDate(DateTime aDate)
        {
            try
            {
                using (new ReadLock(rw))
                {
                    int hash = MIDMath.GregorianHashCode(aDate);

                    // Begin TT#5124 - JSmith - Performance
                    //if (_daysByDate.Contains(hash))
                    if (_daysByDate.ContainsKey(hash))
                    // End TT#5124 - JSmith - Performance
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		// PRINT UTILITIES
		public void DumpYearsToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalYears.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.AutoFlush = true;

                    for (int year = _firstCalendarFiscalYear; year <= _lastCalendarFiscalYear; year++)
                    {
                        YearProfile aYear = GetYear(year);

                        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
                        //rec = aYear.FiscalYear + "\t" + aYear.WeeksInYear + "\t" +
                        rec = aYear.FiscalYear + "\t" + aYear.NoOfWeeks + "\t" +		// Issue 5121
                            //End Track #5121 - JScott - Add Year/Season/Quarter totals
                            aYear.GetHashCode() + " " + aYear.ProfileStartDate.ToString("D", CultureInfo.CurrentUICulture) + "\n";
                        sw.Write(rec);
                    }


                    sw.Close();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void DumpSeasonsToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalSeasons.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.AutoFlush = true;

                    foreach (SeasonProfile cmp in _seasonList.Values)
                    {
                        rec = cmp.Key.ToString(CultureInfo.CurrentUICulture) + " -- " + cmp.FiscalYear + "/" + cmp.FiscalPeriod + "\t" +
                            cmp.Name.PadRight(9, ' ') + "\t" + cmp.NoOfWeeks + "\t" + " " +	// Issue 5121
                            cmp.YearPeriod + " " +
                            cmp.GetHashCode() + " " + cmp.ProfileStartDate.ToString("D", CultureInfo.CurrentUICulture) + "\n";
                        sw.Write(rec);
                        foreach (QuarterProfile qtr in cmp.ChildPeriodList.ArrayList)
                        {
                            rec = "\t" + qtr.Name + " (" + qtr.Key.ToString() + ") " +
                                qtr.YearPeriod.ToString(CultureInfo.CurrentUICulture) + "\n";
                            sw.Write(rec);
                        }
                    }

                    sw.Close();
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void DumpQuartersToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalQuarters.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.AutoFlush = true;

                    foreach (QuarterProfile cmp in _quarterList.Values)
                    {
                        rec = cmp.Key.ToString(CultureInfo.CurrentUICulture) + " -- " + cmp.FiscalYear + "/" + cmp.FiscalPeriod + "\t" +
                            cmp.Name.PadRight(9, ' ') + "\t" + cmp.NoOfWeeks + "\t" + " " +	// Issue 5121
                            cmp.YearPeriod + " " +
                            cmp.GetHashCode() + " " + cmp.ProfileStartDate.ToString("D", CultureInfo.CurrentUICulture) + "\n";
                        sw.Write(rec);
                        foreach (MonthProfile month in cmp.ChildPeriodList.ArrayList)
                        {
                            rec = "\t" + month.Name + " (" + month.Key.ToString() + ") " +
                                month.YearPeriod.ToString(CultureInfo.CurrentUICulture) + "\n";
                            sw.Write(rec);
                        }
                    }

                    sw.Close();
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void DumpMonthsToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalMonths.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    foreach (MonthProfile cmp in _monthList.Values)
                    {
                        rec = cmp.Key.ToString(CultureInfo.CurrentUICulture) + " -- " + cmp.FiscalYear + "/" + cmp.FiscalPeriod + "\t" +
                            cmp.Name.PadRight(9, ' ') + "\t" + cmp.NoOfWeeks + "\t" + " " +	// Issue 5121
                            cmp.YearPeriod + " " +
                            cmp.GetHashCode() + " " + cmp.ProfileStartDate.ToString("D", CultureInfo.CurrentUICulture) + "\n";
                        sw.Write(rec);
                        foreach (WeekProfile wk in cmp.Weeks.ArrayList)
                        {
                            rec = "\t" + wk.FiscalYear.ToString(CultureInfo.CurrentUICulture) +
                                wk.WeekInYear.ToString(CultureInfo.CurrentUICulture) + "\n";
                            sw.Write(rec);
                        }
                    }
                    sw.Close();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void DumpWeeksToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalWeeks.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    foreach (WeekProfile wk in _weekList.Values)
                    {
                        rec = wk.Key.ToString(CultureInfo.CurrentUICulture) + " -- " + wk.FiscalYear + "/" + wk.Period.FiscalPeriod + " " + wk.WeekInPeriod + "\t" +
                            wk.WeekInYear + " " + wk.YearWeek + " " + " " +
                            wk.GetHashCode() + " " + wk.GetAltHashCode() + " " + wk.ProfileStartDate.ToString("D", CultureInfo.CurrentUICulture) + "\n";
                        sw.Write(rec);
                        foreach (DayProfile day in wk.Days.ArrayList)
                        {
                            rec = "\t" + day.YearDay.ToString(CultureInfo.CurrentUICulture) + "\n";
                            sw.Write(rec);
                        }
                    }

                    sw.Close();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void DumpDaysToFile()
		{
			try
			{
                using (new ReadLock(rw))
                {
                    string rec = "";
                    FileStream fs = new FileStream("CalDays.txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.AutoFlush = true;

                    foreach (DayProfile aDay in _dayList.Values)
                    {
                        rec = aDay.FiscalYear + "/" + aDay.Period.FiscalPeriod + " " + aDay.Week.WeekInPeriod + "\t" +
                            aDay.DayInWeek + " " + aDay.DayInYear + " " + aDay.YearDay + " " +
                            aDay.GetHashCode() + " " + aDay.Date.ToString("D", CultureInfo.CurrentUICulture) +
                            " enum " + aDay.DayOfWeek + "\n";
                        sw.Write(rec);
                    }

                    sw.Close();
                }
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}
	}
}
