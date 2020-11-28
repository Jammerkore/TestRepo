using System;
using System.Collections;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the daily percentages for a range of time
	/// Key is calendar date range record ID
	/// </summary>
	[Serializable()]
	public class DailyPercentagesProfile : Profile
	{
		// Fields

		private eChangeType			_dailyPercentagesChangeType;
		private DateRangeProfile	_dateRange;
		private double				_day1;
		private double				_day2;
		private double				_day3;
		private double				_day4;
		private double				_day5;
		private double				_day6;
		private double				_day7;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public DailyPercentagesProfile(int aKey)
			: base(aKey)
		{
			_dailyPercentagesChangeType		= eChangeType.none;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.DailyPercentages;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a daily percentages information.
		/// </summary>
		public eChangeType DailyPercentagesChangeType 
		{
			get { return _dailyPercentagesChangeType ; }
			set { _dailyPercentagesChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the date range profile of a daily percentages information.
		/// </summary>
		public DateRangeProfile	DateRange
		{
			get { return _dateRange ; }
			set { _dateRange = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 1 of the daily percentages for the range of time.
		/// </summary>
		public double Day1 
		{
			get { return _day1 ; }
			set { _day1 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 2 of the daily percentages for the range of time.
		/// </summary>
		public double Day2 
		{
			get { return _day2 ; }
			set { _day2 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 3 of the daily percentages for the range of time.
		/// </summary>
		public double Day3 
		{
			get { return _day3 ; }
			set { _day3 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 4 of the daily percentages for the range of time.
		/// </summary>
		public double Day4 
		{
			get { return _day4 ; }
			set { _day4 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 5 of the daily percentages for the range of time.
		/// </summary>
		public double Day5 
		{
			get { return _day5 ; }
			set { _day5 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 6 of the daily percentages for the range of time.
		/// </summary>
		public double Day6 
		{
			get { return _day6 ; }
			set { _day6 = value; }
		}
		/// <summary>
		/// Gets or sets the value for day 7 of the daily percentages for the range of time.
		/// </summary>
		public double Day7 
		{
			get { return _day7 ; }
			set { _day7 = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of daily percentages for a range of time
	/// </summary>
	[Serializable()]
	public class DailyPercentagesList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public DailyPercentagesList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}


	/// <summary>
	/// Contains the information about the daily percentages time ranges for a store for a node in a hierarchy
	/// The key is store RID
	/// </summary>
	[Serializable()]
	public class StoreDailyPercentagesProfile : Profile
	{
		// Fields

		private eChangeType				_storeDailyPercentagesDefaultChangeType;
		private bool					_storeDailyPercentagesIsInherited;
		private int						_storeDailyPercentagesInheritedFromNodeRID;
		private bool					_hasDefaultValues;
		private double					_day1Default;
		private double					_day2Default;
		private double					_day3Default;
		private double					_day4Default;
		private double					_day5Default;
		private double					_day6Default;
		private double					_day7Default;
		private DailyPercentagesList	_dailyPercentagesList;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreDailyPercentagesProfile(int aKey)
			: base(aKey)
		{
			_storeDailyPercentagesDefaultChangeType = eChangeType.none;
			_hasDefaultValues = false;
			_storeDailyPercentagesIsInherited			= false;
			_storeDailyPercentagesInheritedFromNodeRID	= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreDailyPercentages;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store's daily percentages information.
		/// </summary>
		public eChangeType StoreDailyPercentagesDefaultChangeType 
		{
			get { return _storeDailyPercentagesDefaultChangeType ; }
			set { _storeDailyPercentagesDefaultChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a store's daily percentages information is inherited.
		/// </summary>
		public bool StoreDailyPercentagesIsInherited 
		{
			get { return _storeDailyPercentagesIsInherited ; }
			set { _storeDailyPercentagesIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store's daily percentages information is inherited from.
		/// </summary>
		public int StoreDailyPercentagesInheritedFromNodeRID 
		{
			get { return _storeDailyPercentagesInheritedFromNodeRID ; }
			set { _storeDailyPercentagesInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the flag that identifies if the node has daily percentages default values.
		/// </summary>
		public bool HasDefaultValues 
		{
			get { return _hasDefaultValues ; }
			set { _hasDefaultValues = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 1 of the daily percentages .
		/// </summary>
		public double Day1Default 
		{
			get { return _day1Default ; }
			set { _day1Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 2 of the daily percentages.
		/// </summary>
		public double Day2Default 
		{
			get { return _day2Default ; }
			set { _day2Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 3 of the daily percentages.
		/// </summary>
		public double Day3Default 
		{
			get { return _day3Default ; }
			set { _day3Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 4 of the daily percentages.
		/// </summary>
		public double Day4Default 
		{
			get { return _day4Default ; }
			set { _day4Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 5 of the daily percentages.
		/// </summary>
		public double Day5Default 
		{
			get { return _day5Default ; }
			set { _day5Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 6 of the daily percentages.
		/// </summary>
		public double Day6Default 
		{
			get { return _day6Default ; }
			set { _day6Default = value; }
		}
		/// <summary>
		/// Gets or sets the default value for day 7 of the daily percentages.
		/// </summary>
		public double Day7Default 
		{
			get { return _day7Default ; }
			set { _day7Default = value; }
		}
		/// <summary>
		/// Gets or sets the list of daily percentages time ranges.
		/// </summary>
		public DailyPercentagesList DailyPercentagesList 
		{
			get { return _dailyPercentagesList ; }
			set { _dailyPercentagesList = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of store daily percentage profiles 
	/// </summary>
	[Serializable()]
	public class StoreDailyPercentagesList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreDailyPercentagesList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	/// <summary>
	/// Contains the information about the eligibility for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreWeekDailyPercentagesProfile : Profile
	{
		// Fields

		private int					_yearWeek;
		private double[]			_dailyPercentages;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreWeekDailyPercentagesProfile(int aKey)
			: base(aKey)
		{
			_dailyPercentages = new double[7];
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.DailyPercentages;
			}
		}

		/// <summary>
		/// Gets or sets the year/week for which eligibility is to be determined.
		/// </summary>
		public int YearWeek 
		{
			get { return _yearWeek ; }
			set { _yearWeek = value; }
		}
		/// <summary>
		/// Gets or sets the array of daily percentage values.
		/// </summary>
		public double[] DailyPercentages 
		{
			get { return _dailyPercentages ; }
			set { _dailyPercentages = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of eligibility information for the store/week combinations
	/// </summary>
	[Serializable()]
	public class StoreWeekDailyPercentagesList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreWeekDailyPercentagesList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
