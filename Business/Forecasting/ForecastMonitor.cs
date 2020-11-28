using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for ForecastMonitor.
	/// </summary>
	public class ForecastMonitor : MIDLog
	{
		private OTSPlanMethod _forecastMethod;
		private ArrayList _sets;	  //Array of GroupLevelFunctions
		private ArrayList _stores;	  //Array of store names
		private ProfileList _groupSets; //Array of GroupLevelProfiles
		private bool _isDefault;
		private eGroupLevelSmoothBy _smoothBy;
		private eGroupLevelFunctionType _forecastType;
		private double _chainValue;
		private int _yearWeek;
		private ArrayList _storeData;
		private DateTime _startDateTime;
		private ArrayList _miscMsgList;
        //private int _userRid;
        //private string _userName;
        //private string _fileName;
		private eForecastMonitorType _monitorType;
        //private string _homeDirectory;
		private SessionAddressBlock _sab;
		//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
		private CustomBusinessRoutines _customBusinessRoutines;
		//End TT#875 - JScott - Add Base Code to Support A&F Custom Features

		#region Properties
		//************
		// PROPERTIES
		//************
		public string ForecastName 
		{
			get { return _forecastMethod.Name ; }
			//set { _forecastName = value; }
		}
		public int UserRID 
		{
			get { return _forecastMethod.User_RID ; }
			//set { _userRID = value; }
		}
		public int StoreGroup 
		{
			get { return _forecastMethod.SG_RID ; }
			//set { _storeGroup = value; }
		}
		public bool IsDefault 
		{
			get { return _isDefault ; }
			set { _isDefault = value; }
		}
		public eGroupLevelSmoothBy SmoothBy 
		{
			get { return _smoothBy ; }
			set { _smoothBy = value; }
		}
		public eGroupLevelFunctionType ForecastType 
		{
			get { return _forecastType ; }
			set { _forecastType = value; }
		}
		public double ChainValue 
		{
			get { return _chainValue ; }
			set { _chainValue = value; }
		}
		public int YearWeek 
		{
			get { return _yearWeek ; }
			set { _yearWeek = value; }
		}
		public OTSPlanMethod ForecastMethod 
		{
			get { return _forecastMethod ; }
			set { _forecastMethod = value; }
		}
		public ArrayList StoreDataList
		{
			get { return _storeData ; }
			set { _storeData = value; }
		}
		public eForecastMonitorType MonitorType 
		{
			get { return _monitorType ; }
			set { _monitorType = value; }
		}
		public ProfileList GroupSets
		{
			get { return _groupSets ; }
			set { _groupSets = value; }
		}
		#endregion

		//************
		// CONSTRUCTOR
		//************

    //Begin TT#339 - MD - Modify Forecast audit message - RBeck
        //public ForecastMonitor(OTSPlanMethod forecastMethod, SessionAddressBlock aSab, string filePrefix, string filePath, int userRid, string methodName, int methodRid)
        //: base (filePrefix, filePath, userRid, methodName, methodRid)
        public ForecastMonitor(OTSPlanMethod forecastMethod, SessionAddressBlock aSab, 
                               string filePrefix, string filePath, int userRid, string methodName, 
                               string qualifiedNodeID)
            : base(filePrefix, filePath, userRid, methodName,  qualifiedNodeID)    
    //End  TT#339 - MD - Modify Forecast audit message - RBeck
		{
			_forecastMethod = forecastMethod;
			_sab = aSab;

        //TT#753-754 - MD - Log informational message added to audit - RBeck
            string msgText = MIDText.GetText(eMIDTextCode.msg_OTS_Forecast_Method_LogInformation);
            msgText = msgText.Replace("{0}", methodName);
            msgText = msgText.Replace("{1}", LogLocation);
            msgText = msgText.Replace("{2}", userRid.ToString());
            msgText = msgText.Replace("{3}", UserName);
            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, this.ToString());
        //TT#753-754 - MD - Log informational message added to audit - RBeck

			_sets = new ArrayList();
			_stores = new ArrayList();
			_groupSets = new ProfileList(eProfileType.StoreGroupLevel);
			_chainValue = 0;
			_storeData = new ArrayList();
			_startDateTime = DateTime.Now;
			_miscMsgList = new ArrayList();
			//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
            // Begin TT#610-MD - JSmith - Ran a Forecast and Audit shows a system null reference exception for the forecast monitoring log.
            //_customBusinessRoutines = new CustomBusinessRoutines(null, null, null, Include.NoRID);
            _customBusinessRoutines = new CustomBusinessRoutines(aSab, null, null, Include.NoRID);
            // End TT#610-MD - JSmith - Ran a Forecast and Audit shows a system null reference exception for the forecast monitoring log.
			//End TT#875 - JScott - Add Base Code to Support A&F Custom Features
		}

		//*********
		// METHODS
		//*********
		
		public void AddSet(string setName)
		{
			_sets.Add(setName);
		}

		public void AddSets(ArrayList sets)
		{
			_sets.AddRange(sets);
		}

		public void AddStore(string storeName)
		{
			_stores.Add(storeName);
		}

		public ForecastMonitorStoreData CreateStoreData(int storeRID)
		{
			ForecastMonitorStoreData sd = new ForecastMonitorStoreData(storeRID);
			_storeData.Add(sd);
			return sd;
		}

		public ForecastMonitorStoreData GetStoreData(int storeRID)
		{
			ForecastMonitorStoreData sd = null;
			foreach (ForecastMonitorStoreData fmsd in _storeData)
			{
				if (fmsd.StoreRID == storeRID)
				{
					sd = fmsd;
					break;
				}
			}
			return sd;
		}

		/// <summary>
		/// Adds message to Misc Message List
		/// </summary>
		/// <param name="message"></param>
		public void AddMiscMessage(string message)
		{
			_miscMsgList.Add(message);
		}


		public void WriteSetHeader()
		{
			SW.WriteLine(" ");
			SW.WriteLine("Forecast Type: " + _forecastType.ToString());
			SW.WriteLine("Is Default: " + _isDefault.ToString(CultureInfo.CurrentUICulture));
			SW.WriteLine("Smooth By: " + _smoothBy.ToString());
			SW.WriteLine("Chain Value: " + _chainValue.ToString(CultureInfo.CurrentUICulture));
			SW.WriteLine("For Week: " + _yearWeek.ToString(CultureInfo.CurrentUICulture));

			SW.WriteLine("SETS:");
			foreach(GroupLevelFunctionProfile aSet in _sets)
			{

				StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupSets.FindKey(aSet.Key);

				SW.Write("  " + sglp.Name + "(" + aSet.Key.ToString(CultureInfo.CurrentUICulture) + ")" + "\t");
			}
			SW.WriteLine();
			SW.WriteLine("STORES in this SET(S):");
			foreach(string aStore in _stores)
			{
				SW.Write(aStore + "\t");
			}
			SW.WriteLine();
		}

		public void WriteMiscMessageQueue()
		{
			foreach(string msg in this._miscMsgList)
			{
				SW.WriteLine(msg);
			}
			SW.WriteLine();
			_miscMsgList.Clear();

		}

		public void DumpToFile()
		{
			WriteStoreDataLabels();
			foreach(ForecastMonitorStoreData sd in _storeData)
			{

				WriteStoreData(sd, false);
			}
			SW.WriteLine();

//			CloseLogFile();
			ClearSetDataOnly();
		}

		public void Clear()
		{
			ClearSetDataOnly();
			_groupSets.Clear();
			_isDefault = false;
		}

		public void ClearSetDataOnly()
		{
			ClearWeeklyDataOnly();
			_sets.Clear();
			_stores.Clear();
		}


		public void ClearWeeklyDataOnly()
		{
			_chainValue = 0;
			_yearWeek = 0;
			_storeData.Clear();
			_miscMsgList.Clear();
		}


		//**************************************************************************************
		// New methods for on demand printing of log
		//**************************************************************************************
		public void WriteStoreDataLabels()
		{
			String rec = new String(' ',150);
			SW.WriteLine(rec);
			switch (_monitorType)
			{
				case eForecastMonitorType.PercentContribution:
					rec = rec.Insert(0, "StoreName(RID)");
					rec = rec.Insert(26, "isElig");
					rec = rec.Insert(33, "grade");
					rec = rec.Insert(39, "set");
					rec = rec.Insert(46, "isLocked");
					rec = rec.Insert(55, "initValue");
					rec = rec.Insert(67, "salesMod");
					rec = rec.Insert(76, "ResultValue");
					rec = rec.TrimEnd();
					rec += "\r";
					break;

				case eForecastMonitorType.AverageSales:
					rec = rec.Insert(0, "StoreName(RID)");
					rec = rec.Insert(26, "isElig");
					rec = rec.Insert(33, "grade");
					rec = rec.Insert(39, "set");
					rec = rec.Insert(46, "isLocked");
					rec = rec.Insert(55, "initValue");
					rec = rec.Insert(67, "salesMod");
					rec = rec.Insert(76, "ResultValue");
					rec = rec.TrimEnd();
					rec += "\r";
					break;

				case eForecastMonitorType.CurrentTrend:
					rec = rec.Insert(0, "StoreName(RID)");
					rec = rec.Insert(26, "isElig");
					rec = rec.Insert(33, "grade");
					rec = rec.Insert(39, "set");
					rec = rec.Insert(46, "isLocked");
					rec = rec.Insert(55, "initValue");
					rec = rec.Insert(67, "salesMod");
					rec = rec.Insert(76, "ResultValue");
					rec = rec.TrimEnd();
					rec += "\r";
					break;

				case eForecastMonitorType.TyLyTrend:
					rec = rec.Insert(0, "StoreName(RID)");
					rec = rec.Insert(26, "isElig");
					rec = rec.Insert(33, "grade");
					rec = rec.Insert(39, "set");
					rec = rec.Insert(46, "isLocked");
					rec = rec.Insert(55, "LY");
					rec = rec.Insert(67, "TY");
					rec = rec.Insert(79, "Trend");
					rec = rec.Insert(91, "ApplyTo");
					rec = rec.Insert(103, "Result");
					rec = rec.TrimEnd();
					rec += "\r";
					break;
			
				case eForecastMonitorType.Inventory:
					rec = rec.Insert(0, "StoreName(RID)");
					rec = rec.Insert(26, "isElig");
					rec = rec.Insert(33, "grade");
					rec = rec.Insert(39, "set");
					rec = rec.Insert(46, "isLocked");
					rec = rec.Insert(55, "totalSales");
					rec = rec.Insert(67, "avgSales");
					rec = rec.Insert(79, "weeksUsed");
					// BEGIN MID Track #4370 - John Smith - FWOS Models
					rec = rec.Insert(89, "WOS");
					//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
					//rec = rec.Insert(99, "WOSMod");
					rec = rec.Insert(99, _customBusinessRoutines.GetForecastMonitorWOSModLabel());
					//End TT#875 - JScott - Add Base Code to Support A&F Custom Features
					rec = rec.Insert(109, "WOSIndex");
					rec = rec.Insert(118, "stkMod");
					rec = rec.Insert(125, "stkMin");
					rec = rec.Insert(132, "stkMax");
					rec = rec.Insert(139, "inventory");
					//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
					//rec = rec.Insert(149, "Apply-Pre Min");  // Issue 4827
					rec = rec.Insert(149, _customBusinessRoutines.GetForecastMonitorApplyPreMinLabel());
					//End TT#875 - JScott - Add Base Code to Support A&F Custom Features
//					rec = rec.Insert(89, "WOSIndex");
//					rec = rec.Insert(98, "stkMod");
//					rec = rec.Insert(105, "stkMin");
//					rec = rec.Insert(112, "stkMax");
//					rec = rec.Insert(119, "inventory");
					// END MID Track #4370
					rec = rec.TrimEnd();
					rec += "\r";
					break;

			}

			SW.WriteLine(rec);
		}


		public void WriteStoreData(ForecastMonitorStoreData sd, bool showTrendDiff)
		{
			string rec;
			switch (_monitorType)
			{
				case eForecastMonitorType.PercentContribution:
					rec =  BuildPercentContributionOutputString(sd); 
					SW.Write(rec);
					break;

				case eForecastMonitorType.AverageSales:
					rec =  BuildPercentContributionOutputString(sd); 
					SW.Write(rec);
					break;

				case eForecastMonitorType.CurrentTrend:
					rec =  BuildPercentContributionOutputString(sd); 
					SW.Write(rec);
					break;

				case eForecastMonitorType.TyLyTrend:
					rec =  BuildTyLyTrendOutputString(sd, showTrendDiff); 
					SW.Write(rec);
					break;
			
				case eForecastMonitorType.Inventory:
					rec =  BuildInventoryOutputString(sd); 
					SW.Write(rec);
					break;
			}
			SW.Flush();
		}

		// Begin Issue 4124 - stodd
		public void WriteAllStoreData()
		{
			WriteAllStoreData(false);
		}
		// End issue 4124

		public void WriteAllStoreData(bool showTrendDiff)
		{
			WriteStoreDataLabels();
			switch (_monitorType)
			{
				case eForecastMonitorType.PercentContribution:
					foreach(ForecastMonitorStoreData sd in _storeData)
					{
						string rec =  BuildPercentContributionOutputString(sd); 
						SW.Write(rec);
					}
					break;

				case eForecastMonitorType.AverageSales:
					foreach(ForecastMonitorStoreData sd in _storeData)
					{
						string rec =  BuildPercentContributionOutputString(sd); 
						SW.Write(rec);
					}
					break;

				case eForecastMonitorType.CurrentTrend:
					foreach(ForecastMonitorStoreData sd in _storeData)
					{
						string rec =  BuildPercentContributionOutputString(sd); 
						SW.Write(rec);
					}
					break;

				case eForecastMonitorType.TyLyTrend:
					foreach(ForecastMonitorStoreData sd in _storeData)
					{
						string rec =  BuildTyLyTrendOutputString(sd, showTrendDiff); 
						SW.Write(rec);
					}
					break;
			
				case eForecastMonitorType.Inventory:
					foreach(ForecastMonitorStoreData sd in _storeData)
					{
						string rec =  BuildInventoryOutputString(sd); 
						SW.Write(rec);
					}
					break;
			}
			SW.Flush();
		}

		private string BuildInventoryOutputString(ForecastMonitorStoreData sd)
		{
			String rec = new String(' ',150);
			rec = rec.Insert(0,sd.StoreName + "(" + sd.StoreRID.ToString(CultureInfo.CurrentUICulture) + ")");
			rec = rec.Insert(26, sd.IsEligible.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(33, sd.Grade.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(39, sd.Set.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(46, sd.IsLocked.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(55, sd.TotalSales.ToString( CultureInfo.CurrentUICulture));
			rec = rec.Insert(67, sd.AvgSales.ToString("###0.####", CultureInfo.CurrentUICulture));
			rec = rec.Insert(79, sd.WeeksUsed.ToString("###0.####", CultureInfo.CurrentUICulture));
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			rec = rec.Insert(89, sd.WeeksOfSupply.ToString("###0.####", CultureInfo.CurrentUICulture));
			rec = rec.Insert(99, sd.WeeksOfSupplyWasOverridden.ToString(CultureInfo.CurrentUICulture));
			if (sd.WeeksOfSupplyWasOverridden)
			{
				rec = rec.Insert(109, "------");
			}
			else
			{
				rec = rec.Insert(109, sd.WOSIndex.ToString("###0.####", CultureInfo.CurrentUICulture));
			}
			rec = rec.Insert(118, sd.StockModifier.ToString( CultureInfo.CurrentUICulture));
			if (sd.StockMin == 0)
				rec = rec.Insert(125, "------");
			else
				rec = rec.Insert(125, sd.StockMin.ToString( CultureInfo.CurrentUICulture));
			if (sd.StockMax == int.MaxValue || sd.StockMax == 0)
				rec = rec.Insert(132, "------");
			else	
				rec = rec.Insert(132, sd.StockMax.ToString( CultureInfo.CurrentUICulture));
			rec = rec.Insert(139, sd.Inventory.ToString( CultureInfo.CurrentUICulture));
			rec = rec.Insert(149, sd.ApplyPresentationMin.ToString( CultureInfo.CurrentUICulture));  // Issue 4827
			rec = rec.Insert(156, sd.PresentationMin.ToString( CultureInfo.CurrentUICulture));  // Issue 4827
			// END MID Track #4370
			rec = rec.TrimEnd();
			rec += "\r";
			return rec;
		}
		
		private string BuildPercentContributionOutputString(ForecastMonitorStoreData sd)
		{
			String rec = new String(' ',150);
			rec = rec.Insert(0,sd.StoreName + "(" + sd.StoreRID.ToString(CultureInfo.CurrentUICulture) + ")");
			rec = rec.Insert(26, sd.IsEligible.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(33, sd.Grade.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(39, sd.Set.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(46, sd.IsLocked.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(55, sd.InitValue.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(67, sd.SalesModifier.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(79, sd.ResultValue.ToString(CultureInfo.CurrentUICulture));
			rec = rec.TrimEnd();
			rec += "\r";
			return rec;
		}

		private string BuildTyLyTrendOutputString(ForecastMonitorStoreData sd, bool showTrendDiff)
		{
			String rec = new String(' ',150);
			rec = rec.Insert(0,sd.StoreName + "(" + sd.StoreRID.ToString(CultureInfo.CurrentUICulture) + ")");
			rec = rec.Insert(26, sd.IsEligible.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(33, sd.Grade.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(39, sd.Set.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(46, sd.IsLocked.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(55, sd.LyBasisValue.ToString(CultureInfo.CurrentUICulture));
			rec = rec.Insert(67, sd.TyBasisValue.ToString(CultureInfo.CurrentUICulture));
			// Begin Issue 4124 - stodd
			if (showTrendDiff)
			{
				string originalTrend = sd.OriginalTrend.ToString("##.####", CultureInfo.CurrentUICulture);
				string trend = sd.Trend.ToString("##.####", CultureInfo.CurrentUICulture);
				if (trend != originalTrend)
					originalTrend = originalTrend + "*";
				rec = rec.Insert(79, originalTrend);
			}
			else
			{
				string trend = sd.Trend.ToString("##.####", CultureInfo.CurrentUICulture);
				rec = rec.Insert(79, trend);
			}
			// End Issue 4124
			// BEGIN TT#279-MD - stodd - Projected Sales 
			rec = rec.Insert(91, sd.ApplyToValue.ToString("##.########", CultureInfo.CurrentUICulture));
			// END TT#279-MD - stodd - Projected Sales 
			rec = rec.Insert(103, sd.ResultValue.ToString(CultureInfo.CurrentUICulture));
			rec = rec.TrimEnd();
			rec += "\r";
			return rec;
		}
	}

	public class ForecastMonitorStoreData
	{
		private int _storeRID;
		private string _storeName;
		private bool _isLocked;
		private bool _isEligible;
		private int _grade;
		private int _set;
		private double _initValue;
		private double _basisValue;
		private double _resultValue;

		private double _totalSales;
		private double _avgSales;
		private double _weeksUsed;
		private double _WOSIndex;
		private double _inventory;
		private double _salesModifier;
		private double _stockModifier;
		private int _stockMin;
		private int _stockMax;
		private double _tyBasisValue;
		private double _lyBasisValue;
		private double _originalTrend; // Issue 4124 - stodd
		private double _trend;
		private double _applyToValue;
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private double _weeksOfSupply;
		bool _weeksOfSupplyWasOverridden;
		// End MID Track #4370
		private double _presentationMin;			// Issue 4827
		private bool _applyPresentationMin;			// Issue 4827

		//************
		// PROPERTIES
		//************
		#region Properties
		public int StoreRID 
		{
			get { return _storeRID ; }
			set { _storeRID = value; }
		}
		public string StoreName 
		{
			get { return _storeName ; }
			set { _storeName = value; }
		}
		public bool IsLocked 
		{
			get { return _isLocked ; }
			set { _isLocked = value; }
		}
		public bool IsEligible 
		{
			get { return _isEligible ; }
			set { _isEligible = value; }
		}
		public int Grade 
		{
			get { return _grade ; }
			set { _grade = value; }
		}
		public int Set 
		{
			get { return _set ; }
			set { _set = value; }
		}
		public double InitValue 
		{
			get { return _initValue ; }
			set { _initValue = value; }
		}
		public double BasisValue 
		{
			get { return _basisValue ; }
			set { _basisValue = value; }
		}
		public double ResultValue 
		{
			get { return _resultValue ; }
			set { _resultValue = value; }
		}
		public double TotalSales 
		{
			get { return _totalSales ; }
			set { _totalSales = value; }
		}
		public double AvgSales 
		{
			get { return _avgSales ; }
			set { _avgSales = value; }
		}
		public double WeeksUsed 
		{
			get { return _weeksUsed ; }
			set { _weeksUsed = value; }
		}
		public double WOSIndex 
		{
			get { return _WOSIndex ; }
			set { _WOSIndex = value; }
		}
		public double Inventory 
		{
			get { return _inventory ; }
			set { _inventory = value; }
		}
		public double SalesModifier 
		{
			get { return _salesModifier ; }
			set { _salesModifier = value; }
		}
		public double StockModifier 
		{
			get { return _stockModifier ; }
			set { _stockModifier = value; }
		}
		public int StockMin 
		{
			get { return _stockMin ; }
			set { _stockMin = value; }
		}
		public int StockMax 
		{
			get { return _stockMax ; }
			set { _stockMax = value; }
		}
		public double TyBasisValue 
		{
			get { return _tyBasisValue ; }
			set { _tyBasisValue = value; }
		}
		public double LyBasisValue 
		{
			get { return _lyBasisValue ; }
			set { _lyBasisValue = value; }
		}
		public double Trend 
		{
			get { return _trend ; }
			set { _trend = value; }
		}
		// Begin Issue 4124 - stodd
		public double OriginalTrend 
		{
			get { return _originalTrend ; }
			set { _originalTrend = value; }
		}
		// End Issue 4124 - stodd
		public double ApplyToValue 
		{
			get { return _applyToValue ; }
			set { _applyToValue = value; }
		}
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		public double WeeksOfSupply
		{
			get { return _weeksOfSupply; }
			set { _weeksOfSupply = value; }
		}
		public bool WeeksOfSupplyWasOverridden
		{
			get { return _weeksOfSupplyWasOverridden; }
			set { _weeksOfSupplyWasOverridden = value; }
		}
		// End MID Track #4370
		// BEGIN MID Track #4827 - stodd 10.29.2007 presentation minimum
		public double PresentationMin
		{
			get { return _presentationMin; }
			set { _presentationMin = value; }
		}
		public bool ApplyPresentationMin
		{
			get { return _applyPresentationMin; }
			set { _applyPresentationMin = value; }
		}
		// End MID Track #4827
		#endregion	

		//************
		// CONSTRUCTOR
		//************
		public ForecastMonitorStoreData(int storeRID)
		{
			_storeRID = storeRID;
			_isLocked = false;
			this.IsEligible = true;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_weeksOfSupplyWasOverridden = false;
			// End MID Track #4370
		}

		//*********
		// METHODS
		//*********
	}

	public class MonitorDescendingSalesComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is ForecastMonitorStoreData) && !(y is ForecastMonitorStoreData))        
			{          
				throw new ArgumentException("only allows ForecastMonitorStoreData objects");        
			}        
			return (-((ForecastMonitorStoreData)x).TotalSales.CompareTo(((ForecastMonitorStoreData)y).TotalSales));
		}    
	}
}
