using System;
using System.Data;
using System.Collections;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Reads from the Database the list of user defined calendar models and
	/// holds them for the <see cref="MRSCalendar"/> class.
	/// </summary>
	/// 
	public class CalendarModels
	{
		private ArrayList _models;

		/// <summary>
		/// Gets or sets the array of calendar models defined in the system
		/// </summary>
		public ArrayList Models 
		{
			get { return _models ; }
			set { _models = value; }
		}

		private CalendarData _cd;
		private DataTable _dtCalendarModel;
		private DataTable _dtCalendarModelMonth;
		private DataTable _dtCalendarModelQuarter;
		private DataTable _dtCalendarModelSeason;
		private DataTable _dtCalendarWeek53Year;

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public CalendarModels()
		{
			_models = new ArrayList();
			_cd = new CalendarData();

			this._dtCalendarModel = Read();

			// set up datatable that holds the 53 week years
			Week53_read();
			
			if (_dtCalendarModel.Rows.Count > 0)
				LoadModels();
			else
			{
				// error msg - no models
			}
		}

		/// <summary>
		/// Loads all calendar models from the datavbase into CalendarModeInfo.
		/// </summary>
		private void LoadModels()
		{
			CalendarModel prevModel = null;
			foreach(DataRow dr in _dtCalendarModel.Rows)
			{
			
				CalendarModel cm = new CalendarModel();

				cm.RID = Convert.ToInt32(dr["CM_RID"], CultureInfo.CurrentUICulture);
				cm.ModelName = dr["CM_ID"].ToString();
				cm.StartDate = (DateTime)dr["START_DATE"];
				cm.FiscalYear = Convert.ToInt32(dr["FISCAL_YEAR"], CultureInfo.CurrentUICulture);
				if (prevModel != null)
					prevModel.LastModelYear = cm.FiscalYear - 1;
				cm.LastModelYear = cm.FiscalYear + Include.CalendarStartupYearRange;
				//cm.Offset53rdBaseYear = Convert.ToInt32(dr["WEEK53_OFFSET_BASE_YEAR"]);
				//cm.Offset53rdWeekPeriod = Convert.ToInt32(dr["WEEK53_OFFSET_PERIOD"]);
				//cm.Offset53rdYearInterval = Convert.ToInt32(dr["WEEK53_OFFSET_YEAR_INTERVAL"]);

				//=========
				// MONTHS
				//=========
				this._dtCalendarModelMonth = _cd.CalendarModelPeriods_Read(cm.RID, eCalendarModelPeriodType.Month);
				foreach (DataRow pdr in _dtCalendarModelMonth.Rows)
				{
					CalendarModelPeriod cmp = new CalendarModelPeriod();
					cmp.ModelRID = Convert.ToInt32(pdr["CM_RID"], CultureInfo.CurrentUICulture);
					cmp.Sequence = Convert.ToInt32(pdr["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
					cmp.Name = pdr["CMP_ID"].ToString();
					cmp.Abbreviation = pdr["CMP_ABBREVIATION"].ToString();
					cmp.NoOfTimePeriods = Convert.ToInt32(pdr["NO_OF_TIME_PERIODS"], CultureInfo.CurrentUICulture);
					cmp.ModelPeriodType = eCalendarModelPeriodType.Month;
					cm.Months.Add( cmp );	// add month to model
				}
				//===========
				// QUARTERS
				//===========
				this._dtCalendarModelQuarter = _cd.CalendarModelPeriods_Read(cm.RID, eCalendarModelPeriodType.Quarter);
				foreach (DataRow pdr in _dtCalendarModelQuarter.Rows)
				{
					CalendarModelPeriod cmp = new CalendarModelPeriod();
					cmp.ModelRID = Convert.ToInt32(pdr["CM_RID"], CultureInfo.CurrentUICulture);
					cmp.Sequence = Convert.ToInt32(pdr["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
					cmp.Name = pdr["CMP_ID"].ToString();
					cmp.Abbreviation = pdr["CMP_ABBREVIATION"].ToString();
					cmp.NoOfTimePeriods = Convert.ToInt32(pdr["NO_OF_TIME_PERIODS"], CultureInfo.CurrentUICulture);
					cmp.ModelPeriodType = eCalendarModelPeriodType.Quarter;
					cm.Quarters.Add(cmp);	// add month to model
				}
				//===========
				// SEASONS
				//===========
				this._dtCalendarModelSeason = _cd.CalendarModelPeriods_Read(cm.RID, eCalendarModelPeriodType.Season);
				foreach (DataRow pdr in _dtCalendarModelSeason.Rows)
				{
					CalendarModelPeriod cmp = new CalendarModelPeriod();
					cmp.ModelRID = Convert.ToInt32(pdr["CM_RID"], CultureInfo.CurrentUICulture);
					cmp.Sequence = Convert.ToInt32(pdr["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
					cmp.Name = pdr["CMP_ID"].ToString();
					cmp.Abbreviation = pdr["CMP_ABBREVIATION"].ToString();
					cmp.NoOfTimePeriods = Convert.ToInt32(pdr["NO_OF_TIME_PERIODS"], CultureInfo.CurrentUICulture);
					cmp.ModelPeriodType = eCalendarModelPeriodType.Season;
					cm.Seasons.Add(cmp);	// add month to model
				}
				prevModel = cm;
				this.Models.Add( cm );	// add model to model info
			}
		}

		/// <summary>
		/// reads and returns a datatable of all Calendar Models in start date sequence.
		/// </summary>
		/// <returns>DataTable</returns>
		internal DataTable Read()
		{
			this._dtCalendarModel = _cd.CalendarModel_Read();
			return _dtCalendarModel;
		}

		internal CalendarModel GetModelForYear(int year)
		{
			CalendarModel model = null;
			foreach(CalendarModel cm in _models)
			{
				if (year >= cm.FiscalYear && year <= cm.LastModelYear)
				{
					model = cm;
					break;
				}

			}
			return model;
		}

		public CalendarModel GetCalendarModel(int CalendarModelRID)
		{
			CalendarModel model = null;
			foreach(CalendarModel cm in _models)
			{
				if (CalendarModelRID == cm.RID)
				{
					model = cm;
					break;
				}

			}
			return model;
		}

		public CalendarModel GetPriorCalendarModel(int CalendarModelRID)
		{
			CalendarModel priorModel = null;
			foreach(CalendarModel cm in _models)
			{
				if (CalendarModelRID == cm.RID)
				{
					break;
				}
				priorModel = cm;
			}
			return priorModel;
		}

		/// <summary>
		/// Takes in an array of CalendarModel RIDs and deletes those Models.
		/// </summary>
		/// <param name="deleteKeyList"></param>
		public void Move53WeekToPriorModel(int modelKey)
		{
			OpenUpdateConnection();
			
			CalendarModel priorModel = GetPriorCalendarModel(modelKey);
			DataTable dtWeek53Year = _cd.CalendarWeek53Year_Read(modelKey);
			int week53Cnt = dtWeek53Year.Rows.Count;
			for (int j=0;j<week53Cnt;j++)
			{
				DataRow row53Week = dtWeek53Year.Rows[j];
				int seq = Convert.ToInt32(row53Week["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);
				int year = Convert.ToInt32(row53Week["WEEK53_FISCAL_YEAR"], CultureInfo.CurrentUICulture);
				if (IsSeqOnModel(modelKey, seq))
				{
					eWeek53Offset offset = (eWeek53Offset)Convert.ToInt32(row53Week["OFFSET_ID"], CultureInfo.CurrentUICulture);
					Week53_update(year, priorModel.RID, seq, offset ); 
				}
				else
				{
					this.Week53_delete(year, modelKey, seq);
				}
			}
			
			//_cd.CalendarWeek53Year_Delete(modelRid);
			CommitData();
			CloseUpdateConnection();

		}

		// TEMP until all modules use 3 parm call
		public bool IsSeqOnModel(int modelRid, int seq)
		{
			return IsSeqOnModel(modelRid, seq, eCalendarModelPeriodType.Month);
		}


		public bool IsSeqOnModel(int modelRid, int seq, eCalendarModelPeriodType periodType)
		{
			bool isSeqOnModel = false;
			CalendarModel cm = this.GetCalendarModel(modelRid);

			switch (periodType)
			{
				case eCalendarModelPeriodType.Month:
					foreach (CalendarModelPeriod modelPer in cm.Months)
					{
						if (seq == modelPer.Sequence)
						{
							isSeqOnModel = true;
							break;
						}
					}
					break;
				case eCalendarModelPeriodType.Quarter:
					foreach (CalendarModelPeriod modelPer in cm.Quarters)
					{
						if (seq == modelPer.Sequence)
						{
							isSeqOnModel = true;
							break;
						}
					}
					break;
				case eCalendarModelPeriodType.Season:
					foreach (CalendarModelPeriod modelPer in cm.Seasons)
					{
						if (seq == modelPer.Sequence)
						{
							isSeqOnModel = true;
							break;
						}
					}
					break;
				default:
					break;
			}
				
			return isSeqOnModel;
		}


		/// <summary>
		/// For the year requested, sends back the sequence of the period selected
		/// for the 53rd week.  If this is not a 53 week year, it will return 0.
		/// </summary>
		/// <param name="year">int</param>
		/// <returns>int period sequence</returns>
		internal int GetWeek53Period(int year)
		{
			int periodSeq = 0;
			DataRow dr = _dtCalendarWeek53Year.Rows.Find(year);
			if (dr != null)
				periodSeq = Convert.ToInt32(dr["CMP_SEQUENCE"], CultureInfo.CurrentUICulture);

			return periodSeq;
		}

		internal eWeek53Offset GetWeek53OffsetId(int year)
		{
            //Begin TT#15 - RBeck- Disable the dropdown selection "DropWeek53"
            eWeek53Offset offsetId = eWeek53Offset.Offset1Week;
            //eWeek53Offset offsetId = eWeek53Offset.DropWeek53;
            //End TT#15
			DataRow dr = _dtCalendarWeek53Year.Rows.Find(year);
			if (dr != null)
				offsetId = (eWeek53Offset)Convert.ToInt32(dr["OFFSET_ID"], CultureInfo.CurrentUICulture);
			else
			{
				offsetId = eWeek53Offset.Not53WeekYear;
			}

			return offsetId;
		}

		internal DataTable Week53_read()
		{
			// set up datatable that holds the 53 week years
			this._dtCalendarWeek53Year = _cd.CalendarWeek53Year_Read();
			DataColumn[] PrimaryKeyColumn = new DataColumn[1];
			PrimaryKeyColumn[0] = _dtCalendarWeek53Year.Columns["WEEK53_FISCAL_YEAR"];
			_dtCalendarWeek53Year.PrimaryKey = PrimaryKeyColumn;

			return _dtCalendarWeek53Year;
		}

		internal void Week53_delete(int cm_rid)
		{
			try
			{
				_cd.CalendarWeek53Year_Delete(cm_rid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		internal void Week53_delete(int year, int cm_rid, int sequence)
		{
			try
			{
				_cd.CalendarWeek53Year_Delete(year, cm_rid, sequence);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		internal void Week53_insert(int year, int cm_rid, int sequence, DataCommon.eWeek53Offset offset)
		{
			try
			{
				_cd.CalendarWeek53Year_Insert(year, cm_rid, sequence, offset);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Week53_update(int year, int cm_rid, int sequence, DataCommon.eWeek53Offset offset)
		{
			try
			{
				_cd.CalendarWeek53Year_Update(year, cm_rid, sequence, offset);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// determines where calendar model RID sent is the first model defined
		/// </summary>
		/// <param name="cm_rid">int</param>
		/// <returns>bool</returns>
		internal bool IsFirstModel(int cm_rid)
		{
			bool isFirst = false;

			if (this._models.Count > 0)
			{
				CalendarModel cm = (CalendarModel)_models[0];
				if (cm.RID == cm_rid)
					isFirst = true;
			}
			else
				isFirst = true;

			return isFirst;
		}

		internal bool IsLastModel(int cm_rid)
		{
			bool isLast = false;

			if (this._models.Count > 0)
			{
				CalendarModel cm = (CalendarModel)_models[ _models.Count - 1 ];
				if (cm.RID == cm_rid)
					isLast = true;
			}
			else
				isLast = true;

			return isLast;
		}

		public void OpenUpdateConnection()
		{
			_cd.OpenUpdateConnection();
		}
		public void CommitData()
		{
			_cd.CommitData();
		}
		public void CloseUpdateConnection()
		{
			_cd.CloseUpdateConnection();
		}

	}

	/// <summary>
	/// Holds a single instance of a Calendar Model including an array of the Calendar
	/// Model Periods.
	/// </summary>
	public class CalendarModel : ICloneable
	{
		private int _RID; 
		private string _modelName;
		private DateTime _startDate;
		private int _fiscalYear;
		private int _lastModelYear;
		//private int _offset53rdBaseYear;
		//private int _offset53rdWeekPeriod;
		//private int _offset53rdYearInterval;
		//private int _numberOfPeriods;

		// BEGIN Issue 5121 stodd
		private int _numberOfMonths;
		private int _numberOfQuarters;
		private int _numberOfSeasons;
		// END Issue 5121

		//private ArrayList _periods;
		// BEGIN Issue 5121 stodd
		private ArrayList _months;
		private ArrayList _quarters;
		private ArrayList _seasons;
		// END Issue 5121

		/// <summary>
		/// Gets or sets the Record ID for the Calendar Model.
		/// </summary>
		public int RID 
		{
			get { return _RID ; }
			set { _RID = value; }
		}
		/// <summary>
		/// Gets or sets the calendar model name.
		/// </summary>
		public string ModelName 
		{
			get { return _modelName ; }
			set { _modelName = value; }
		}
		/// <summary>
		/// Gets or sets the gregorian start date for the calendar model.
		/// </summary>
		public DateTime StartDate 
		{
			get { return _startDate ; }
			set { _startDate = value; }
		}
		/// <summary>
		/// Gets or sets the fiscal year that represents the first fiscal year of the
		/// calendar model.
		/// </summary>
		public int FiscalYear 
		{
			get { return _fiscalYear ; }
			set { _fiscalYear = value; }
		}
		/// <summary>
		/// Gets or sets the last fiscal year represented by this
		/// calendar model.
		/// </summary>
		public int LastModelYear 
		{
			get { return _lastModelYear ; }
			set { _lastModelYear = value; }
		}
//		/// <summary>
//		/// Gets of sets the first year that will contain 53 weeks.
//		/// </summary>
//		public int Offset53rdBaseYear 
//		{
//			get { return _offset53rdBaseYear ; }
//			set { _offset53rdBaseYear = value; }
//		}
//		/// <summary>
//		/// Gets or sets the period within the 53 week year where the extra week will go.
//		/// </summary>
//		public int Offset53rdWeekPeriod 
//		{
//			get { return _offset53rdWeekPeriod ; }
//			set { _offset53rdWeekPeriod = value; }
//		}
//		/// <summary>
//		/// Gets or set the interval off of the 53 week base year that the 53rd week will
//		/// appear in.
//		/// </summary>
//		public int Offset53rdYearInterval 
//		{
//			get { return _offset53rdYearInterval ; }
//			set { _offset53rdYearInterval = value; }
//		}
		/// <summary>
		/// Gets or sets the total number of periods within a fiscal year.
		/// </summary>
		//public int NumberOfPeriods 
		//{
		//    get { return _numberOfPeriods ; }
		//    set { _numberOfPeriods = value; }
		//}
		/// <summary>
		/// Gets or sets the array of <see cref="CalendarModelPeriod"/> for this Calendar Model.
		/// </summary>
		//public ArrayList Periods 
		//{
		//    get { return _periods ; }
		//    set { _periods = value; }
		//}
		/// <summary>
		/// Gets or sets the array of <see cref="CalendarModelPeriod"/> for this Calendar Model.
		/// </summary>
		public ArrayList Months
		{
			get { return _months; }
			set { _months = value; }
		}
		/// <summary>
		/// Gets or sets the array of <see cref="CalendarModelPeriod"/> for this Calendar Model.
		/// </summary>
		public ArrayList Quarters
		{
			get { return _quarters; }
			set { _quarters = value; }
		}
		/// <summary>
		/// Gets or sets the array of <see cref="CalendarModelPeriod"/> for this Calendar Model.
		/// </summary>
		public ArrayList Seasons
		{
			get { return _seasons; }
			set { _seasons = value; }
		}
		/// <summary>
		/// Gets or sets the total number of months within a fiscal year.
		/// </summary>
		public int NumberOfMonths
		{
			get 
			{
				if (_months == null)
					return 0;
				else
				{
					return _months.Count;
				}
			}
		}
		/// <summary>
		/// Gets or sets the total number of months within a fiscal year.
		/// </summary>
		public int NumberOfQuarters
		{
			get 
			{
				if (_quarters == null)
					return 0;
				else
				{
					return _quarters.Count;
				}
			}
		}
		/// <summary>
		/// Gets or sets the total number of months within a fiscal year.
		/// </summary>
		public int NumberOfSeasons
		{
			get 
			{
				if (_seasons == null)
					return 0;
				else
				{
					return _seasons.Count;
				}
			}
		}

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public CalendarModel()
		{
			_months = new ArrayList();
			_quarters = new ArrayList();	
			_seasons = new ArrayList();	
		}

		/// <summary>
		/// Creates a clone of the Calendar Model.
		/// </summary>
		/// <returns>An object containing this calendar model.</returns>
		public object Clone() 
		{
			CalendarModel cm = new CalendarModel();
			cm._RID = this._RID;
			cm._startDate = this.StartDate;
			cm._fiscalYear = this.FiscalYear;
			cm._lastModelYear = this._lastModelYear;
			cm._modelName = this._modelName;
			//cm._offset53rdBaseYear = this._offset53rdBaseYear;
			//cm._offset53rdWeekPeriod = this._offset53rdWeekPeriod;
			//cm._offset53rdYearInterval = this._offset53rdYearInterval;
			cm._months = (ArrayList)this._months.Clone();
			cm._quarters = (ArrayList)this._quarters.Clone();
			cm._seasons = (ArrayList)this._seasons.Clone();
			
			return cm;
		}
	}

	/// <summary>
	/// Holds a single instance of a Calendar Model Period
	/// </summary>
	public class CalendarModelPeriod
	{
		private int _modelRID;
		private int _sequence;
		private string _name;
		private string _abbreviation;
		private int _noOfTimePeriods;
		private eCalendarModelPeriodType _modelPeriodType;	// Issue 5121
		/// <summary>
		/// Gets or Sets the Calendar Model Record Id this period belongs to.
		/// </summary>
		public int ModelRID 
		{
			get { return _modelRID ; }
			set { _modelRID = value; }
		}
		/// <summary>
		/// Gets or sets the periods sequence number within the calendar model.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
		/// <summary>
		/// Gets or sets the calendar model period name.
		/// </summary>
		public string Name 
		{
			get { return _name ; }
			set { _name = value; }
		}
		/// <summary>
		/// Gets or sets the calendar model period abbreviation.
		/// </summary>
		public string Abbreviation 
		{
			get { return _abbreviation ; }
			set { _abbreviation = value; }
		}
		/// <summary>
		/// Gets or sets the number of weeks in this period.
		/// </summary>
		public int NoOfTimePeriods 
		{
			get { return _noOfTimePeriods ; }
			set { _noOfTimePeriods = value; }
		}
		// BEGIN Issue 5121 stodd
		/// <summary>
		/// Gets or sets the model period type.
		/// </summary>
		public eCalendarModelPeriodType ModelPeriodType
		{
			get { return _modelPeriodType; }
			set { _modelPeriodType = value; }
		}
		// END Issue 5121

		/// <summary>
		/// Used to construct an instance of the class. 
		/// </summary>
		public CalendarModelPeriod()
		{
			_sequence = 0;
			_name = "Unknown";
			_abbreviation = "Unk";
			_noOfTimePeriods = 0;
			_modelPeriodType = eCalendarModelPeriodType.None;
		}
		/// <summary>
		/// Used to construct an instance of the class
		/// </summary>
		/// <param name="modelPeriodType"></param>
		public CalendarModelPeriod(eCalendarModelPeriodType modelPeriodType)
		{
			_sequence = 0;
			_name = "Unknown";
			_abbreviation = "Unk";
			_noOfTimePeriods = 0;
			_modelPeriodType = modelPeriodType;
		}
		/// <summary>
		/// Used to construct an instance of the class
		/// </summary>
		/// <param name="seq">Sequence</param>
		/// <param name="na">Name</param>
		/// <param name="abbr">Abbreviation</param>
		/// <param name="weeks">WeeksInPeriod</param>
		public CalendarModelPeriod(int seq, string na, string abbr, int timePeriods, eCalendarModelPeriodType modelPeriodType, int noOfChildren)
		{
			_sequence = seq;
			_name = na;
			_abbreviation = abbr;
			_noOfTimePeriods = timePeriods;
			_modelPeriodType = modelPeriodType;
		}
	}
}
