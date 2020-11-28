using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

namespace MIDRetail.DataCommon
{
	#region DateProfile
	/// <summary>
	/// DateProfile is a base class for the other date classes: 
	/// <see cref="DayProfile"/>, <see cref="WeekProfile"/>, <see cref="PeriodProfile"/>, and  <see cref="YearProfile"/>. 
	/// </summary>
	[Serializable()]
	public class DateProfile : Profile
	{
		private DateTime _startDate;
		private int _fiscalYear;
		private int _gregorianHash;
		/// <summary>
		/// Gets or sets the beginning date of a week, period or year.
		/// </summary>
		public DateTime ProfileStartDate 
		{
			get { return this.Date; }
			set { this.Date = value; }
//			get { return _startDate ; }
//			set { _startDate = value; }
		}
		/// <summary>
		/// Gets or sets the gregorian date of a day.
		/// </summary>
		public DateTime Date 
		{
			get { return _startDate ; }
			set 
			{
				_startDate = value;
				this._gregorianHash = MIDMath.GregorianHashCode(value);
			}
		}
		/// <summary>
		/// Gets the Gregorian Hash Code associated with this profile.
		/// </summary>
		public int GregorianHashCode
		{
			get { return this._gregorianHash; }
		}

		/// <summary>
		/// Gets or sets the fiscal year.
		/// </summary>
		public int FiscalYear 
		{
			get { return _fiscalYear ; }
			set { _fiscalYear = value; }
		}
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public DateProfile(int aKey)
		: base(aKey)
		{
			
		}
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Date;
			}
		}

		public virtual string Text()
		{
			return "A Date";
		}

		public int CompareTo(DateProfile x)    
		{      
			return this.Key.CompareTo(x.Key);
		}
		public override bool Equals(Object aObject) 
		{
			// Check for null values and compare run-time types.
			if (aObject == null || GetType() != aObject.GetType()) 
				return false;
			if (Key != ((DateProfile)aObject).Key) {	return false; }
			return true;
		}
		public override int GetHashCode() 
		{
			return base.GetHashCode();
		}
		public static bool operator==(DateProfile aDate, DateProfile bDate)
		{ 
			if ((object)aDate == null && (object)bDate == null) { return true; }
		
			if ((object)aDate == null) { return false; }
			if ((object)bDate == null) { return false; }

			return (aDate.Key == bDate.Key);
		}
		public static bool operator!=(DateProfile aDate, DateProfile bDate)
		{ 
			return !(aDate == bDate);
		}
		public static bool operator<(DateProfile aDate, DateProfile bDate)
		{ 
			return ((IComparable)aDate.Key).CompareTo(bDate.Key) < 0;
		}
		public static bool operator<=(DateProfile aDate, DateProfile bDate)
		{ 
			return ((IComparable)aDate.Key).CompareTo(bDate.Key) <= 0;
		}

		public static bool operator>(DateProfile aDate, DateProfile bDate)
		{ 
			return ((IComparable)aDate.Key).CompareTo(bDate.Key) > 0;
		}

		public static bool operator>=(DateProfile aDate, DateProfile bDate)
		{ 
			return ((IComparable)aDate.Key).CompareTo(bDate.Key) >= 0;
		}

	}
	#endregion

	#region DayProfile
	/// <summary>
	/// Holds all the information for a particular day, including a reference to the days
	/// fiscal period and fiscal week.
	/// KEY: Julian Date YYYYDDD
	/// </summary>
	[Serializable()]
	public class DayProfile : DateProfile, ICloneable
	{
		private PeriodProfile _period;
		private WeekProfile _week;
		private int _dayInWeek;
//		private int _dayInYear;
//		private int _yearDay;
		private DayOfWeek _dayOfWeek;

		/// <summary>
		/// Gets or sets the PeriodProfile in which this day resides
		/// </summary>
		public PeriodProfile Period
		{
			get { return _period ; }
			set { _period = value; }
		}
		/// <summary>
		/// Gets or sets the WeekProfile in which this day resides
		/// </summary>
		public WeekProfile Week
		{
			get { return _week ; }
			set { _week = value; }
		}
		/// <summary>
		/// Gets the Julian day in year
		/// </summary>
		public int DayInYear 
		{
			get { return Date.DayOfYear; }
			//get { return _dayInYear ; }
			//set { _dayInYear = value; }
		}
//		/// <summary>
//		/// gets Calendar (Gregorian) day as YYYYDDD
//		/// </summary>
//		public int CalYearDay 
//		{
//			get 
//			{
//				string dayInYear = Date.Year.ToString("####",CultureInfo.CurrentUICulture) + Date.DayOfYear.ToString("###",CultureInfo.CurrentUICulture); 
//				return Convert.ToInt32(dayInYear); ; 
//			}
//		}
		/// <summary>
		///  Gets or sets the fiscal day in week
		/// </summary>
		public int DayInWeek 
		{
			get { return _dayInWeek ; }
			set { _dayInWeek = value; }
		}
		/// <summary>
		/// Gets or sets the year/day which is formatted as YYYYDDD.
		/// </summary>
		public int YearDay 
		{
			get
			{
//				if (_yearDay == 0)
//				{
//					_yearDay = this.FiscalYear * 1000 + this.DayInYear;
//				}
				return Key;
			}
//			get { return _yearDay ; }
//			set { _yearDay = value; }
		}
		/// <summary>
		/// Gets or sets the DayOfWeek enumeration.
		/// </summary>
		public DayOfWeek DayOfWeek 
		{
			get { return _dayOfWeek ; }
			set { _dayOfWeek = value; }
		}

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		/// <param name="aKey"></param>
		public DayProfile(int aKey)
			: base(aKey)
		{
		    
		}
		/// <summary>
		/// Gets a hash code for this class based upon the gregorian YYYYMMDD.
		/// </summary>
		/// <returns> A hash value. </returns>
		public override int GetHashCode()
		{
//			string hash = this.Date.ToString("yyyyMMdd", CultureInfo.CurrentUICulture);
//			int hash = Convert.ToInt32(hash, CultureInfo.CurrentUICulture);
			return this.GregorianHashCode;
//			return Convert.ToInt32(hash, CultureInfo.CurrentUICulture);

		}
		/// <summary>
		/// Gets a hash code for this class based upon fiscal YYYYDDD.
		/// </summary>
		/// <returns> A hash value. </returns>
		public int GetAltHashCode()
		{
			return YearDay;
			//			return Convert.ToInt32( this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture) + this.DayInYear.ToString("000", CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture);

		}

		/// <summary>
		/// returns a text representation of the day.
		/// </summary>
		/// <returns></returns>
		public override string Text()
		{
			return "Day " + this.DayInYear.ToString("000", CultureInfo.CurrentUICulture) + "/" + this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);

		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Day;
			}
		}

		public object Clone()
		{
			DayProfile dayProfile = new DayProfile(_key);
			dayProfile.Date = Date;
			dayProfile.FiscalYear = FiscalYear;
			dayProfile.Week = _week;
			dayProfile.DayInWeek = _dayInWeek;
			dayProfile.Period = _period;
			dayProfile.DayOfWeek = _dayOfWeek;
			return dayProfile;
		}
	}
	#endregion

	#region WeekProfile
	/// <summary>
	/// Holds all of the information for a particular fiscal week
	/// KEY: First Day of Week as a Julian Date YYYYDDD
	/// </summary>
	[Serializable()]
	public class WeekProfile : DateProfile, ICloneable
	{
		private PeriodProfile _period;
		private int _weekInPeriod;
		private int _daysInWeek;
		private int _weekInYear;
		private int _yearWeek;
		private ProfileList _days;

		/// <summary>
		/// Gets or sets the PeriodProfile in which this day resides
		/// </summary>
		public PeriodProfile Period
		{
			get { return _period ; }
			set { _period = value; }
		}
		/// <summary>
		/// Gets or sets the fiscal week in a period.
		/// </summary>
		public int WeekInPeriod 
		{
			get { return _weekInPeriod ; }
			set { _weekInPeriod = value; }
		}
		/// <summary>
		/// Gets or sets the number of days in a week.  Default is 7.
		/// </summary>
		public int DaysInWeek 
		{
			get { return _daysInWeek ; }
			set { _daysInWeek = value; }
		}
		// Gets or sets the fiscal week in the year.
		public int WeekInYear 
		{
			get { return _weekInYear ; }
			set { _weekInYear = value; }
		}
		/// <summary>
		/// Gets or sets the year/week which is formatted as YYYYWW.
		/// </summary>
		public int YearWeek 
		{
			get { return _yearWeek ; }
			set { _yearWeek = value; }
		}
		/// <summary>
		/// Gets a profileList of days contained within the Week.
		/// </summary>
		public ProfileList Days 
		{
			get { return _days ; }
			set { _days = value; }
		}
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public WeekProfile(int aKey)
			: base(aKey)
		{
			 _daysInWeek = 7;
			_days = new ProfileList(eProfileType.Day);

		}

		/// <summary>
		/// Returns the Text() value for this profile.
		/// </summary>
		/// <returns>A string containing the Text() value.</returns>
		public override string ToString()
		{
			return Text();
		}

		/// <summary>
		/// Gets a hash code for the class based upon fiscal YYYYPPWW.
		/// Where PP is the period in the year and WW is the week in the period.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return (Date.Year * 10000) + (Date.Month * 100) + Date.Day;
		}
		/// <summary>
		/// Gets a hash code for the class based upon fiscal YYYYWW.
		/// Where WW is week in year.
		/// </summary>
		/// <returns>A hash code.</returns>
		public int GetAltHashCode()
		{
//			string hash = this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture) + 
//				this.WeekInYear.ToString("00", CultureInfo.CurrentUICulture);
//			return Convert.ToInt32(hash, CultureInfo.CurrentUICulture);
			return Key;

		}
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Week;
			}
		}

		/// <summary>
		/// returns a text representation of the week.
		/// </summary>
		/// <returns></returns>
		public override string Text()
		{
			return "Week " + this.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" + this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);

		}

		public object Clone()
		{
			WeekProfile weekProfile = (WeekProfile)this.MemberwiseClone();
			weekProfile.Date = Date;
			weekProfile.FiscalYear = FiscalYear;
			weekProfile.Period = _period;
			weekProfile.WeekInPeriod = _weekInPeriod;
			weekProfile.DaysInWeek = _daysInWeek;
			weekProfile.WeekInPeriod = _weekInYear;
			weekProfile.YearWeek = _yearWeek;
			weekProfile.Days = (ProfileList)_days.Clone();
			return weekProfile;
		}
	}
	#endregion

	#region PeriodProfile
	/// <summary>
	/// Holds all of the information for a particular fiscal period
	/// KEY: First Day of Week as a Julian Date YYYYDDD
	/// </summary>
	[Serializable()]
	abstract public class PeriodProfile : DateProfile
	{
		private int _fiscalPeriod;
		private string _name;
		private string _abbreviation;
		//private int _weeksInPeriod;
		private int _yearPeriod;
		// BEGIN Issue 5121
		private ProfileList _childPeriodList;
		// END Issue 5121
		private ProfileList _weeks;  // array of weeks within period

		/// <summary>
		/// Gets or sets the fiscal period number which is the fiscal number for the month, quarter, season.
		/// </summary>
		public int FiscalPeriod 
		{
			get { return _fiscalPeriod ; }
			set { _fiscalPeriod = value; }
		}
		/// <summary>
		/// Gets or sets the period's name.
		/// </summary>
		public string Name 
		{
			get { return _name ; }
			set { _name = value; }
		}
		/// <summary>
		/// Gets or sets the period's abbreviation.
		/// </summary>
		public string Abbreviation 
		{
			get { return _abbreviation ; }
			set { _abbreviation = value; }
		}
		///// <summary>
		///// Gets or sets the number of weeks in this period.
		///// </summary>
		//public int WeeksInPeriod 
		//{
		//    get { return _weeksInPeriod ; }
		//    set { _weeksInPeriod = value; }
		//}
		/// <summary>
		/// Gets or sets year/period number where the period number is the fiscal number for the month, quarter, season.
		/// </summary>
		public int YearPeriod 
		{
			get { return _yearPeriod ; }
			set { _yearPeriod = value; }
		}
		/// <summary>
		/// returns a profile list of weeks contained within the period
		/// </summary>
		public ProfileList Weeks 
		{
			get { return _weeks ; }
			set { _weeks = value; }
		}

		public int NoOfWeeks
		{
			get 
			{
				if (_weeks != null)
					return _weeks.Count;
				else 
					return 0;
			}
		}

		// BEGIN Issue 512
		/// <summary>
		/// List of subordinate Periods. (Year will contain seasons, seasons will contain Quarters and so on.)
		/// </summary>
		public ProfileList ChildPeriodList
		{
			get { return _childPeriodList; }
			set { _childPeriodList = value; }
		}

		/// <summary>
		/// First day of period as a julian date, YYYYDDD
		/// </summary>
		public int BeginDateJulian
		{
			get
			{
				if (_weeks != null && _weeks.Count > 0)
					return _weeks[0].Key;
				else
					return 0;
			}
		}

		/// <summary>
		/// The julian date of the last week of the period.
		/// </summary>
		public int LastWeekDateJulian
		{
			get
			{
				if (_weeks != null && _weeks.Count > 0)
				{
					int lastWeek = _weeks.Count;
					lastWeek--;
					return _weeks[lastWeek].Key;
				}
				else
					return 0;
			}
		}

		/// <summary>
		/// Declares which type of Period this is: Year, Season, Quarter, or Month
		/// </summary>
		abstract public eProfileType PeriodProfileType { get; }
		// END Issue 5121

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public PeriodProfile(int aKey)
			: base(aKey)
		{
			_weeks = new ProfileList(eProfileType.Week);
			//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
			_childPeriodList = new ProfileList(eProfileType.Period);
			//End Track #5121 - JScott - Add Year/Season/Quarter totals
		}
		/// <summary>
		/// Returns the Text() value for this profile.
		/// </summary>
		/// <returns>A string containing the Text() value.</returns>
		public override string ToString()
		{
			return Text();
		}

		/// <summary>
		/// Gets a hash code for the class based upon fiscal YYYYPP.
		/// Where PP is the period in the year.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return Key;
		}
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Period;
			}
		}

		/// <summary>
		/// returns a text representation of the period.
		/// </summary>
		/// <returns></returns>
		public override string Text()
		{
			return this.Abbreviation + " " + this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
		}

		//public void CloneBase(PeriodProfile periodClone)
		//{
		//    periodClone.FiscalPeriod = _fiscalPeriod;
		//    periodClone.Name = _name;
		//    periodClone.Abbreviation = _abbreviation;
		//    periodClone.YearPeriod = _yearPeriod;
		//    periodClone.ChildPeriodList.Clear();
		//    foreach (PeriodProfile pp in this.ChildPeriodList.ArrayList)
		//    {
		//        PeriodProfile periodProfile = (PeriodProfile)pp.Clone();
		//        periodClone.ChildPeriodList.Add(periodProfile);
		//    }
		//    periodClone.Weeks.Clear();
		//    foreach (WeekProfile wp in this.Weeks.ArrayList)
		//    {
		//        WeekProfile weekProfile = (WeekProfile)wp.Clone();
		//        periodClone.Weeks.Add(weekProfile);
		//    }
		//}
	}
	#endregion

	//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
	#region MonthProfile
	/// <summary>
	/// KEY: First Day month as a Julian Date YYYYDDD
	/// </summary>
	[Serializable()]
	public class MonthProfile : PeriodProfile, ICloneable
	{
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public MonthProfile(int aKey)
			: base(aKey)
		{
		}

		public override eProfileType PeriodProfileType
		{
			get
			{
				return eProfileType.Month;
			}
		}

		public object Clone()
		{
			MonthProfile monthProfile = new MonthProfile(_key);
			monthProfile.Date = Date;
			monthProfile.FiscalYear = FiscalYear;
			monthProfile.FiscalPeriod = FiscalPeriod;
			monthProfile.Name = Name;
			monthProfile.Abbreviation = Abbreviation;
			monthProfile.YearPeriod = YearPeriod;
			foreach (WeekProfile wp in this.ChildPeriodList.ArrayList)
			{
				WeekProfile weekProfile = (WeekProfile)wp.Clone();
				monthProfile.ChildPeriodList.Add(weekProfile);
			}
			foreach (WeekProfile wp in this.Weeks.ArrayList)
			{
				WeekProfile weekProfile = (WeekProfile)wp.Clone();
				monthProfile.Weeks.Add(weekProfile);
			}
			return monthProfile;
		}
	}
	#endregion

	#region QuarterProfile
	/// <summary>
	/// KEY: YYYYQQ
	/// </summary>
	[Serializable()]
	public class QuarterProfile : PeriodProfile, ICloneable
	{
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public QuarterProfile(int aKey)
			: base(aKey)
		{
		}

		public override eProfileType PeriodProfileType
		{
			get
			{
				return eProfileType.Quarter;
			}
		}

		public object Clone()
		{
			QuarterProfile quarterProfile = new QuarterProfile(_key);
			quarterProfile.Date = Date;
			quarterProfile.FiscalYear = FiscalYear;
			quarterProfile.FiscalPeriod = FiscalPeriod;
			quarterProfile.Name = Name;
			quarterProfile.Abbreviation = Abbreviation;
			quarterProfile.YearPeriod = YearPeriod;
			foreach (MonthProfile mp in this.ChildPeriodList.ArrayList)
			{
				MonthProfile monthProfile = (MonthProfile)mp.Clone();
				quarterProfile.ChildPeriodList.Add(monthProfile);
			}
			foreach (WeekProfile wp in this.Weeks.ArrayList)
			{
				WeekProfile weekProfile = (WeekProfile)wp.Clone();
				quarterProfile.Weeks.Add(weekProfile);
			}
			return quarterProfile;
		}

		//public object Clone()
		//{
		//    QuarterProfile cloneQuarter = new QuarterProfile(Key);
		//    cloneQuarter.FiscalPeriod = FiscalPeriod;
		//    cloneQuarter.Name = Name;
		//    cloneQuarter.Abbreviation = Abbreviation;
		//    cloneQuarter.YearPeriod = YearPeriod;
		//    cloneQuarter.ChildPeriodList = (ProfileList)ChildPeriodList.Clone();
		//    cloneQuarter.Weeks = (ProfileList)Weeks.Clone();
		//    return cloneQuarter;
		//}
	}
	#endregion

	#region SeasonProfile
	/// <summary>
	/// KEY: YYYYS
	/// </summary>
	[Serializable()]
	public class SeasonProfile : PeriodProfile, ICloneable
	{
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public SeasonProfile(int aKey)
			: base(aKey)
		{
		}

		public override eProfileType PeriodProfileType
		{
			get
			{
				return eProfileType.Season;
			}
		}

		public object Clone()
		{
			SeasonProfile seasonProfile = new SeasonProfile(_key);
			seasonProfile.Date = Date;
			seasonProfile.FiscalYear = FiscalYear;
			seasonProfile.FiscalPeriod = FiscalPeriod;
			seasonProfile.Name = Name;
			seasonProfile.Abbreviation = Abbreviation;
			seasonProfile.YearPeriod = YearPeriod;
			foreach (QuarterProfile qp in this.ChildPeriodList.ArrayList)
			{
				QuarterProfile quarterProfile = (QuarterProfile)qp.Clone();
				seasonProfile.ChildPeriodList.Add(quarterProfile);
			}
			foreach (WeekProfile wp in this.Weeks.ArrayList)
			{
				WeekProfile weekProfile = (WeekProfile)wp.Clone();
				seasonProfile.Weeks.Add(weekProfile);
			}
			return seasonProfile;
		}
	}
	#endregion

	//End Track #5121 - JScott - Add Year/Season/Quarter totals
	#region YearProfile
	/// <summary>
	/// Holds the number of weeks in a particular fiscal Year
	/// KEY: YYYY
	/// </summary>
	[Serializable()]
	//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
	//public class YearProfile : DateProfile
	public class YearProfile : PeriodProfile, ICloneable
	//End Track #5121 - JScott - Add Year/Season/Quarter totals
	{
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		//private int _weeksInYear;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		private ProfileList _periods;  // array of periods within year
		private int _CM_RID;
		eWeek53Offset _week53OffsetId;

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		///// <summary>
		///// Gets or sets the number of weeks in a fiscal year.
		///// </summary>
		//public int WeeksInYear 
		//{
		//    get { return _weeksInYear ; }
		//    set { _weeksInYear = value; }
		//}
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Gets or sets the array of periods in a fiscal year.
		/// </summary>
		public ProfileList Periods 
		{
			get { return _periods ; }
			set { _periods = value; }
		}

		/// <summary>
		/// Hold the Calendar Model RID this year belongs to
		/// </summary>
		public int CalendarModelRID 
		{
			get { return _CM_RID ; }
			set { _CM_RID = value; }
		}
		
		public eWeek53Offset Week53OffsetId 
		{
			get { return _week53OffsetId ; }
			set { _week53OffsetId = value; }
		}
		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public YearProfile(int aKey)
			: base(aKey)
		{
			_periods = new ProfileList(eProfileType.Period);
		}
		/// <summary>
		/// Gets a hash code for the class based upon fiscal YYYY.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return this.FiscalYear;
		}

		public override string Text()
		{
			return "Year " + this.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
		}

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		///// <summary>
		///// Returns the eProfileType of this profile.
		///// </summary>
		//override public eProfileType ProfileType
		//{
		//    get
		//    {
		//        return eProfileType.Year;
		//    }
		//}

		public override eProfileType PeriodProfileType
		{
			get
			{
				return eProfileType.Year;
			}
		}
		//End Track #5121 - JScott - Add Year/Season/Quarter totals

		public object Clone()
		{
			YearProfile yearProfile = new YearProfile(_key);
			yearProfile.Date = Date;
			yearProfile.FiscalYear = FiscalYear;
			yearProfile.FiscalPeriod = FiscalPeriod;
			yearProfile.Name = Name;
			yearProfile.Abbreviation = Abbreviation;
			yearProfile.YearPeriod = YearPeriod;
			foreach (SeasonProfile sp in this.ChildPeriodList.ArrayList)
			{
				SeasonProfile seasonProfile = (SeasonProfile)sp.Clone();
				yearProfile.ChildPeriodList.Add(seasonProfile);
			}
			foreach (WeekProfile wp in this.Weeks.ArrayList)
			{
				WeekProfile weekProfile = (WeekProfile)wp.Clone();
				yearProfile.Weeks.Add(weekProfile);
			}
			foreach (MonthProfile mp in this.Periods.ArrayList)
			{
				MonthProfile monthProfile = (MonthProfile)mp.Clone();
				yearProfile.Periods.Add(monthProfile);
			}
			yearProfile.CalendarModelRID = _CM_RID;
			yearProfile.Week53OffsetId = _week53OffsetId;
			return yearProfile;
		}
	}
	#endregion

	#region Week53Profile
	/// <summary>
	/// class to represent a year with 53 week selection
	/// </summary>
	[Serializable()]
	public class Week53Profile : DateProfile
	{
		// Key equal fiscal year
		private int _CM_RID;
		private int _CMP_SEQUENCE;

		/// <summary>
		/// Gets or sets the period chosen
		/// </summary>
		public int PeriodSeq 
		{
			get { return _CMP_SEQUENCE ; }
			set { _CMP_SEQUENCE = value; }
		}
		
		/// <summary>
		/// Gets or Sets the model RID for this year/period
		/// </summary>
		public int CalendarModelRID 
		{
			get { return _CM_RID ; }
			set { _CM_RID = value; }
		}

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public Week53Profile(int aKey)
			: base(aKey)
		{
		}
		/// <summary>
		/// Gets a hash code for the class based upon fiscal YYYY.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return this.Key;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Week53Year;
			}
		}
	}
	#endregion

	#region DateRangeProfile
	/// <summary>
	/// Used to communicate date range information
	/// KEY: 300,000 + DRP offeset (offeset begins at 0)
	/// </summary>
	[Serializable()]
	public class DateRangeProfile : DateProfile
	{
		private int _CDR_START;
		private int _CDR_END;
		private eDateRangeRelativeTo _CDR_RELATIVE_TO;
		private eCalendarDateType _CDT_ID;
		private eCalendarRangeType _CDR_RANGE_TYPE_ID;
		private Profile _anchorDate;
		private string _displayDate;
		private string _name;
		private bool _isDynamicSwitch;
		private int _dynamicSwitchDate; // Issue 5171

		/// <summary>
		/// Gets or sets the start of the date range
		/// </summary>
		public int StartDateKey 
		{
			get { return _CDR_START ; }
			set { _CDR_START = value; }
		}
		/// <summary>
		/// Gets or sets the end of the date range
		/// </summary>
		public int EndDateKey 
		{
			get { return _CDR_END ; }
			set { _CDR_END = value; }
		}
		/// <summary>
		/// Gets or sets for dynasmic dates, what date they are relative to: Current, Plan, or Store Open.
		/// </summary>
		public eDateRangeRelativeTo RelativeTo 
		{
			get { return _CDR_RELATIVE_TO ; }
			set { _CDR_RELATIVE_TO = value; }
		}

		/// <summary>
		/// Gets or sets the date type format that the start and end dates are in
		/// </summary>
		/// <remarks>
		/// day - YYYYJJJ
		/// week - YYYYJJJ
		/// period - YYYYJJJ
		/// month - YYYYJJJ
		/// </remarks>
		public eCalendarDateType SelectedDateType 
		{
			get { return _CDT_ID ; }
			set { _CDT_ID = value; }
		}

		/// <summary>
		/// Gets or Sets whether the date is Static, Dynamic, or Reoccurring.
		/// </summary>
		public eCalendarRangeType DateRangeType 
		{
			get { return _CDR_RANGE_TYPE_ID ; }
			set { _CDR_RANGE_TYPE_ID = value; }
		}

		/// <summary>
		/// Contains the display date string for this date range
		/// </summary>
		public string DisplayDate 
		{
			get { return _displayDate ; }
			set { _displayDate = value; }
		}

		/// <summary>
		/// Optional name for the date range.
		/// </summary>
		public string Name 
		{
			get { return _name ; }
			set { _name = value; }
		}

		/// <summary>
		/// Used internally by the calendar to determine the display date 
		/// when RelativeToPlan or RelativeToStoreOpen is selected.
		/// DO NOT USE this as the Anchor date for a dynamic date range. This is only for the internal use of 
		/// the Calendar class. When needing to set the anchor date in a calendar method, the method should have 
		/// a paramenter to house the anchor date.
		/// </summary>
		public Profile InternalAnchorDate 
		{
			get { return _anchorDate ; }
			set { _anchorDate = value; }
		}

		/// <summary>
		/// Is this date range a Dynamic Switch date?
		/// When the selected beginning week is before or equal to the Current Week, then the date is considered 
		/// static and will not change. Once the selected beginning week has passed the Current Week, 
		/// then the date is considered Dynamic and Relative to Current.
		/// </summary>
		public bool IsDynamicSwitch 
		{
			get { return _isDynamicSwitch ; }
			set { _isDynamicSwitch = value; }
		}

		// BEGIN Issue 5171
		/// <summary>
		/// Contains the weekProfile key (julian date) for the week the switch from static to 
		/// dynamic will take place.
		/// </summary>
		public int DynamicSwitchDate 
		{
			get { return _dynamicSwitchDate ; }
			set { _dynamicSwitchDate = value; }
		}
		// END Issue 5171

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public DateRangeProfile(int aKey)
			: base(aKey)
		{
			Init();  // Issue 5171
		}
		
		public void Init()
		{
			this._CDR_START = 0;
			this._CDR_END = 0;
			this._CDR_RANGE_TYPE_ID = eCalendarRangeType.Static;
			this._CDR_RELATIVE_TO = eDateRangeRelativeTo.Current;
			this._anchorDate = null;
			this.Name = null;
			this.DisplayDate = null;
			this.IsDynamicSwitch = false;
			this.DynamicSwitchDate = Include.UndefinedDynamicSwitchDate;  // Issue 5171
		}

		/// <summary>
		/// Gets a hash code for the class based upon the Key.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return this.Key;
		}

		public DateRangeProfile Clone()
		{
			DateRangeProfile drp = new DateRangeProfile(this.Key);
			drp.InternalAnchorDate = this.InternalAnchorDate;
			drp.Date = this.Date;
			drp.DateRangeType = this.DateRangeType;
			drp.DisplayDate = this.DisplayDate;
			drp.EndDateKey = this.EndDateKey;
			drp.FiscalYear = this.FiscalYear;
			drp.Name = this.Name;
			drp.ProfileStartDate = this.ProfileStartDate;
			drp.RelativeTo = this.RelativeTo;
			drp.SelectedDateType = this.SelectedDateType;
			drp.StartDateKey = this.StartDateKey;
			drp.IsDynamicSwitch = this.IsDynamicSwitch;
			drp.DynamicSwitchDate = this.DynamicSwitchDate;	// Issue 5171
			return drp;
		}

		/// <summary>
		/// Does NOT use Key as part of the compare.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public int CompareTo(DateRangeProfile x)    
		{      
			// Begin Track #5898 stodd
			//int result = this.Key.CompareTo(x.Key);
			//if (result != 0) { return result; }
			// End Track #5898
			int result = this._CDR_START.CompareTo(x._CDR_START);
			if (result != 0) { return result; }
			result = this._CDR_END.CompareTo(x._CDR_END);
			if (result != 0) { return result; }
			result = this._CDR_RELATIVE_TO.CompareTo(x._CDR_RELATIVE_TO);
			if (result != 0) { return result; }
			result = this._CDT_ID.CompareTo(x._CDT_ID);
			if (result != 0) { return result; }
			result = this._CDR_RANGE_TYPE_ID.CompareTo(x._CDR_RANGE_TYPE_ID);
			if (result != 0) { return result; }
			result = this._isDynamicSwitch.CompareTo(x._isDynamicSwitch);
			if (result != 0) { return result; }
			// BEGIN Issue 5171
			result = this._dynamicSwitchDate.CompareTo(x._dynamicSwitchDate);
			if (result != 0) { return result; }
			// END Issue 51717
			return (this._name.CompareTo(x._name));
		}

		/// <summary>
		/// Uses Key as part of the compare.
		/// </summary>
		/// <param name="aObject"></param>
		/// <returns></returns>
		public override bool Equals(Object aObject) 
		{
			// Check for null values and compare run-time types.
			if (aObject == null || GetType() != aObject.GetType()) 
				return false;

			if (Key != ((DateRangeProfile)aObject).Key) {	return false; }
			if (_CDR_START != ((DateRangeProfile)aObject)._CDR_START) {	return false; }
			if (_CDR_END != ((DateRangeProfile)aObject)._CDR_END) {	return false; }
			if (_CDR_RELATIVE_TO != ((DateRangeProfile)aObject)._CDR_RELATIVE_TO) {	return false; }
			if (_CDT_ID != ((DateRangeProfile)aObject)._CDT_ID) {	return false; }
			if (_CDR_RANGE_TYPE_ID != ((DateRangeProfile)aObject)._CDR_RANGE_TYPE_ID) {	return false; }
			if (_isDynamicSwitch != ((DateRangeProfile)aObject)._isDynamicSwitch) {	return false; }
			// BEGIN Issue 5171
			if (_dynamicSwitchDate != ((DateRangeProfile)aObject)._dynamicSwitchDate) {	return false; }
			// END Issue 5171
			if (_name != ((DateRangeProfile)aObject)._name) {	return false; }

			return true;
		}

		/// <summary>
		/// Uses Key as part of the compare.
		/// </summary>
		/// <param name="aDRP"></param>
		/// <param name="bDRP"></param>
		/// <returns></returns>
		public static bool operator==(DateRangeProfile aDRP, DateRangeProfile bDRP)
		{ 
			if ((object)aDRP == null && (object)bDRP == null) { return true; }
		
			if ((object)aDRP == null) { return false; }
			if ((object)bDRP == null) { return false; }

			return (aDRP.Key == bDRP.Key) 
					&& (aDRP._CDR_START == bDRP._CDR_START)
					&& (aDRP._CDR_END == bDRP._CDR_END)
					&& (aDRP._CDR_RELATIVE_TO == bDRP._CDR_RELATIVE_TO)
					&& (aDRP._CDT_ID == bDRP._CDT_ID)
					&& (aDRP._CDR_RANGE_TYPE_ID == bDRP._CDR_RANGE_TYPE_ID)
					&& (aDRP.IsDynamicSwitch == bDRP.IsDynamicSwitch)
					&& (aDRP.DynamicSwitchDate == bDRP.DynamicSwitchDate)	// Issue 5171
					&& (aDRP._name == bDRP._name);
		}

		public static bool operator!=(DateRangeProfile aDRP, DateRangeProfile bDRP)
		{ 
			return !(aDRP == bDRP);
		}

		public void Debug()
		{
			this.Debug(string.Empty);
		}

		public void Debug(string text)
		{
			System.Diagnostics.Debug.WriteLine(text + ":  " +
				this.Key.ToString() + ", " +
				this.DateRangeType.ToString() + ", " +
				this.DisplayDate  + ", " +
				this.Name  + ", " +
				this.ProfileStartDate.ToString() + ", " +
				this.RelativeTo.ToString() + ", " +
				this.SelectedDateType.ToString() + ", " +
				this.StartDateKey.ToString() + ", " +
				this.EndDateKey.ToString() + ", " +
				this.FiscalYear.ToString() + ", " +
				this.IsDynamicSwitch.ToString() + ", " +
				this.DynamicSwitchDate.ToString());
		}


		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.DateRange;
			}
		}
	}
	#endregion
}
