using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// The PlanProfile class hold information for a plan.  Information includes the HierarchyNodeProfile, VersionProfile, and HierarchyNodeSecurityProfile.
	/// </summary>

	[Serializable]
	public class PlanProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private HierarchyNodeProfile _nodeProf;
		private VersionProfile _versionProf;
		private eBasisIncludeExclude _includeExclude;	// Issue 4858

		//=============
		// CONSTRUCTORS
		//=============

		public PlanProfile()
			: base(-1)
		{
		}

		public PlanProfile(int aKey)
			: base(aKey)
		{
		}

		public PlanProfile(int aKey, HierarchyNodeProfile aNodeProf, VersionProfile aVersionProf)
			: base(aKey)
		{
			_nodeProf = aNodeProf;
			_versionProf = aVersionProf;
			_includeExclude = eBasisIncludeExclude.Include;		// Issue 4858
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Plan;
			}
		}

		public HierarchyNodeProfile NodeProfile
		{
			get
			{
				return _nodeProf;
			}
			set
			{
				_nodeProf = value;
				if (_nodeProf == null)
				{
					_key = Include.NoRID;
				}
				else
				{
					_key = _nodeProf.Key;
				}
			}
		}

		public VersionProfile VersionProfile
		{
			get
			{
				return _versionProf;
			}
			set
			{
				_versionProf = value;
			}
		}

		// BEGIN Issue 4858 stodd 11.12.2007
		public eBasisIncludeExclude IncludeExclude
		{
			get
			{
				return _includeExclude;
			}
			set
			{
				_includeExclude = value;
			}
		}
		// END Issue 4858 stodd 11.12.2007
		
		override public bool Equals(object obj)
		{
			return (_nodeProf.Key == ((PlanProfile)obj)._nodeProf.Key && _versionProf.Key == ((PlanProfile)obj)._versionProf.Key);
		}

		override public int GetHashCode()
		{
			return (int)(_versionProf.Key & 0xFF) | ((_nodeProf.Key & 0xFF) << 8);
		}
	}
	
	/// <summary>
	/// The PlanOpenParmsData class defines the parameters necessary to open, read, and store the plans required for a
	/// any Plan Maintenance function.
	/// </summary>

	[Serializable]
	abstract public class PlanOpenParmsData
	{
		//=======
		// FIELDS
		//=======

		private FunctionSecurityProfile _functionSecurityProfile;
		private ePlanSessionType _planSessionType;
		private PlanProfile _chainHLPlanProfile;
		private PlanProfile _storeHLPlanProfile;
		private VersionProfile _lowLevelVersionDefault;
		private ProfileList _lowLevelPlanProfileList;
		private ProfileList _basisProfileList;
        private int _eligibilityNodeKey = Include.Undefined;
        private eRequestingApplication _requestingApplication = eRequestingApplication.Forecast;

		private DateRangeProfile _dateRangeProfile;
		private int _StoreGroupRID;
		private bool _similarStores;
		private bool _ineligibleStores;
		private bool _openPeriodAsWeeks;
		private int _filterRID;
		private int _viewRID;
        private string _storeIdNm;    //TT#6-MD-VStuart - Single Store Select
        private string _storeId;    //TT#6-MD-VStuart - Single Store Select
        private eStorePlanSelectedGroupBy _groupBy;
		private eDisplayTimeBy _displayTimeBy;
		private eLowLevelsType _lowLevelsType;
		private int _lowLevelsOffset;
		private int _lowLevelsSequence;
		private string _computationsMode;
		//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		private double _averageDivisor;
		private bool _containsCurrentWeek;
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//Begin Track #3867 -- Low level not sorted on Store Multi view
		//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		//Begin Track #3867 -- Low level not sorted on Store Multi view
		protected SortedList _lowLevelSortedList;
//End Track #3867 -- Low level not sorted on Store Multi view
//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
		protected bool _allowReadOnlyOnConflict;
		protected bool _formatErrorsForMessageBox;
		protected bool _updateAuditHeaderOnError;
//End - Abercrombie & Fitch #4411
		//End Track #3867 -- Low level not sorted on Store Multi view
		private int _overrideLowLevelRid; // Override Low level enhancement stodd
		private int _customOverrideLowLevelRid; // Override Low level enhancement stodd

		// Fields passed for information
		private string _viewName;
		private int _viewUserID;

		private Hashtable _basisProfileListHash;
		private bool _weeksCalculated;
        private bool _setSummaryDateProfile;
		private ePlanDisplayType _displayType;
		private DateRangeProfile _summaryDateProfile;
		private ProfileList _detailDateProfileList;
		private ProfileList _dateProfileList;
		private ProfileList _weekProfileList;
		private ProfileList _periodProfileList;
		private ProfileXRef _dateToWeekXRef;
		private ProfileXRef _dateToPeriodXRef;

        private bool _isLadder; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
        private bool _isMulti; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
        private bool _isTotRT; //// TT#639-MD -agallagher - OTS Forecast Totals Right
        private bool _includeLocks; // TT#TT#739-MD - JSmith - delete stores

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanMaintCubeGroupOpenPamrs.
		/// </summary>

		public PlanOpenParmsData(ePlanSessionType aPlanSessionType, string aComputationMode)
		{
			_planSessionType = aPlanSessionType;
			_computationsMode = aComputationMode;
			_StoreGroupRID = Include.NoRID;
//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
			_allowReadOnlyOnConflict = true;
			_formatErrorsForMessageBox = true; 
			_updateAuditHeaderOnError = true;
//End - Abercrombie & Fitch #4411
            _includeLocks = true; // TT#TT#739-MD - JSmith - delete stores
            _setSummaryDateProfile = false;
            _eligibilityNodeKey = Include.Undefined;
            _requestingApplication = eRequestingApplication.Forecast;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the FunctionSecurityProfile value.
		/// </summary>

		public FunctionSecurityProfile FunctionSecurityProfile
		{
			get
			{
				return _functionSecurityProfile;
			}
			set
			{
				_functionSecurityProfile = value;
			}
		}

		/// <summary>
		/// Gets or sets the ePlanSessionType.
		/// </summary>

		public ePlanSessionType PlanSessionType
		{
			get
			{
				return _planSessionType;
			}
			set
			{
				_planSessionType = value;
			}
		}

		/// <summary>
		/// Gets the high-level Chain PlanProfile
		/// </summary>

		public PlanProfile ChainHLPlanProfile
		{
			get
			{
				try
				{
					if (_chainHLPlanProfile == null)
					{
						_chainHLPlanProfile = new PlanProfile();
					}

					return _chainHLPlanProfile;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the requesting application
		/// </summary>

		public eRequestingApplication RequestingApplication
        {
            get
            {
                return _requestingApplication;
            }
            set
            {
                _requestingApplication = value;
            }
        }

        /// <summary>
        /// Gets or sets the eligibility merchandise key
        /// </summary>

        public int EligibilityNodeKey
        {
            get
            {
                return _eligibilityNodeKey;
            }
            set
            {
                _eligibilityNodeKey = value;
            }
        }

		/// <summary>
		/// Gets the high-level Store PlanProfile
		/// </summary>

		public PlanProfile StoreHLPlanProfile
		{
			get
			{
				try
				{
					if (_storeHLPlanProfile == null)
					{
						_storeHLPlanProfile = new PlanProfile();
					}

					return _storeHLPlanProfile;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the low-level VersionProfile.
		/// </summary>

		public VersionProfile LowLevelVersionDefault
		{
			get
			{
				return _lowLevelVersionDefault;
			}
			set
			{
				_lowLevelVersionDefault = value;
			}
		}

		/// <summary>
		/// Gets the BasisProfile ProfileList.
		/// </summary>

		public ProfileList BasisProfileList
		{
			get
			{
				try
				{
					if (_basisProfileList == null)
					{
						_basisProfileList = new ProfileList(eProfileType.Basis);
					}

					return _basisProfileList;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the low-level Chain PlanProfile ProfileList
		/// </summary>

		public ProfileList LowLevelPlanProfileList
		{
			get
			{
				//Begin Track #3867 -- Low level not sorted on Store Multi view
				IDictionaryEnumerator iEnum;

				//End Track #3867 -- Low level not sorted on Store Multi view
				try
				{
					if (_lowLevelPlanProfileList == null)
					{
						//Begin Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels
						////Begin Track #3867 -- Low level not sorted on Store Multi view
						////						_lowLevelPlanProfileList = new ProfileList(eProfileType.Plan);
						//						_lowLevelPlanProfileList = new ProfileList(eProfileType.Plan);
						//						iEnum = _lowLevelSortedList.GetEnumerator();
						//
						//						while (iEnum.MoveNext())
						//						{
						//							_lowLevelPlanProfileList.Add((Profile)iEnum.Value);
						//						}
						////End Track #3867 -- Low level not sorted on Store Multi view
						_lowLevelPlanProfileList = new ProfileList(eProfileType.Plan);
						if (_lowLevelSortedList != null)
						{
							iEnum = _lowLevelSortedList.GetEnumerator();

							while (iEnum.MoveNext())
							{
								_lowLevelPlanProfileList.Add((Profile)iEnum.Value);
							}
						}
						//Begin Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels
					}

					return _lowLevelPlanProfileList;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the DateRangeProfile value.
		/// </summary>

		public DateRangeProfile DateRangeProfile
		{
			get
			{
				return _dateRangeProfile;
			}
			set
			{
				_dateRangeProfile = value;
				_weeksCalculated = false;
			}
		}

		/// <summary>
		/// Gets or sets the StoreGroupRID value.
		/// </summary>

		public int StoreGroupRID
		{
			get
			{
				return _StoreGroupRID;
			}
			set
			{
				_StoreGroupRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the Similar Store indicator.
		/// </summary>

		public bool SimilarStores
		{
			get
			{
				return _similarStores;
			}
			set
			{
				_similarStores = value;
			}
		}

		/// <summary>
		/// Gets or sets the Ineligible Store indicator.
		/// </summary>

		public bool IneligibleStores
		{
			get
			{
				return _ineligibleStores;
			}
			set
			{
				_ineligibleStores = value;
			}
		}

		/// <summary>
		/// Gets or sets the Open Period As Weeks indicator.
		/// </summary>

		public bool OpenPeriodAsWeeks
		{
			get
			{
				return _openPeriodAsWeeks;
			}
			set
			{
				_openPeriodAsWeeks = value;
			}
		}

		/// <summary>
		/// Gets or sets the record id of the filter.
		/// </summary>
		public int FilterRID
		{
			get	{return _filterRID;}
			set	{_filterRID = value;}
		}

        /// <summary>
        /// Gets or sets the record id of the view.
        /// </summary>
        public int ViewRID
        {
            get { return _viewRID; }
            set { _viewRID = value; }
        }

        //BEGIN TT#6-MD-VStuart - Single Store Select
        /// <summary>
        /// Gets or sets the store id.
        /// </summary>
        public string StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        /// <summary>
        /// Gets or sets the store id and name.
        /// </summary>
        public string StoreIdNm
        {
            get { return _storeIdNm; }
            set { _storeIdNm = value; }
        }

        //END TT#6-MD-VStuart - Single Store Select

        /// <summary>
		/// Gets or sets the eStorePlanSelectedGroupBy by which the data is to be grouped.
		/// </summary>
		public eStorePlanSelectedGroupBy GroupBy
		{
			get	{return _groupBy;}
			set {_groupBy = value;}
		}

		/// <summary>
		/// Gets or sets the eDisplayTimeBy by which the data is to be displayed.
		/// </summary>
		public eDisplayTimeBy DisplayTimeBy
		{
			get	{return _displayTimeBy;}
			set {_displayTimeBy = value;}
		}

		/// <summary>
		/// Gets or sets the type of low levels selected.
		/// </summary>
		public eLowLevelsType LowLevelsType
		{
			get	{return _lowLevelsType;}
			set	{_lowLevelsType = value;}
		}

		/// <summary>
		/// Gets or sets the offset of the low level selected.
		/// </summary>
		/// <remarks>
		/// This field is only to be used if the LowLevelsType is eLowLevelsType.LevelOffset</remarks>
		public int LowLevelsOffset
		{
			get	{return _lowLevelsOffset;}
			set	{_lowLevelsOffset = value;}
		}

		/// <summary>
		/// Gets or sets the sequence of the main hierarchy level used to select the low levels.
		/// </summary>
		/// <remarks>
		/// This field is only to be used if the LowLevelsType is eLowLevelsType.HierarchyLevel</remarks>
		public int LowLevelsSequence
		{
			get	{return _lowLevelsSequence;}
			set	{_lowLevelsSequence = value;}
		}

		/// <summary>
		/// Gets or sets the computation mode to be used by the cube.
		/// </summary>
		public string ComputationsMode
		{
			get	{return _computationsMode;}
			set	{_computationsMode = value;}
		}

		//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		/// <summary>
		/// Gets or sets the Average Divisor.
		/// </summary>
		public double AverageDivisor
		{
			get	{return _averageDivisor;}
			set	{_averageDivisor = value;}
		}

		/// <summary>
		/// Gets or sets the Average Divisor.
		/// </summary>
		public bool ContainsCurrentWeek
		{
			get	{return _containsCurrentWeek;}
			set	{_containsCurrentWeek = value;}
		}

		//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

		// BEGIN Override Low Level Enhancement changes stodd
		/// <summary>
		/// Gets or sets the Override Low Level RID.
		/// </summary>
		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}
		/// <summary>
		/// Gets or sets the Override Low Level RID.
		/// </summary>
		public int CustomOverrideLowLevelRid
		{
			get { return _customOverrideLowLevelRid; }
			set { _customOverrideLowLevelRid = value; }
		}
		// END Override Low Level Enhancement changes stodd

		public string ViewName
		{
			get	{return _viewName;}
			set	{_viewName = value;}
		}

		public int ViewUserID
		{
			get	{return _viewUserID;}
			set	{_viewUserID = value;}
		}

		public Hashtable BasisProfileListHash
		{
			get	{return _basisProfileListHash;}
			set	{_basisProfileListHash = value;}
		}

		public bool WeeksCalculated
		{
			get	{return _weeksCalculated;}
			set	{_weeksCalculated = value;}
		}

        public bool SetSummaryDateProfile
        {
            get { return _setSummaryDateProfile; }
            set { _setSummaryDateProfile = value; }
        }

		public ePlanDisplayType DisplayType
		{
			get	{return _displayType;}
			set	{_displayType = value;}
		}

		public DateRangeProfile SummaryDateProfile
		{
			get	{return _summaryDateProfile;}
			set	{_summaryDateProfile = value;}
		}

		public ProfileList DetailDateProfileList
		{
			get	{return _detailDateProfileList;}
			set	{_detailDateProfileList = value;}
		}

		public ProfileList DateProfileList
		{
			get	{return _dateProfileList;}
			set	{_dateProfileList = value;}
		}

		public ProfileList WeekProfileList
		{
			get	{return _weekProfileList;}
			set	{_weekProfileList = value;}
		}

		public ProfileList PeriodProfileList
		{
			get	{return _periodProfileList;}
			set	{_periodProfileList = value;}
		}

		public ProfileXRef DateToWeekXRef
		{
			get	{return _dateToWeekXRef;}
			set	{_dateToWeekXRef = value;}
		}

		public ProfileXRef DateToPeriodXRef
		{
			get	{return _dateToPeriodXRef;}
			set	{_dateToPeriodXRef = value;}
		}

//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
		public bool AllowReadOnlyOnConflict
		{
			get	{return _allowReadOnlyOnConflict;}
			set	{_allowReadOnlyOnConflict = value;}
		}

		public bool FormatErrorsForMessageBox
		{
			get	{return _formatErrorsForMessageBox;}
			set	{_formatErrorsForMessageBox = value;}
		}

		public bool UpdateAuditHeaderOnError
		{
			get	{return _updateAuditHeaderOnError;}
			set	{_updateAuditHeaderOnError = value;}
		}
//End - Abercrombie & Fitch #4411


        //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
        public bool IsLadder
        {
            get { return _isLadder; }
            set { _isLadder = value; }
        }
        public bool IsMulti
        {
            get { return _isMulti; }
            set { _isMulti = value; }
        }
        //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

        //Begin TT#639-MD -agallagher - OTS Forecast Totals Right
        public bool IsTotRT
        {
            get { return _isTotRT; }
            set { _isTotRT = value; }
        }
        //End TT#639-MD -agallagher - OTS Forecast Totals Right

        // Begin TT#TT#739-MD - JSmith - delete stores
        public bool IncludeLocks
        {
            get { return _includeLocks; }
            set { _includeLocks = value; }
        }
        // End TT#TT#739-MD - JSmith - delete stores

		//Begin Track #4732 - JSmith - No low levels error
		//===========
		// METHODS
		//===========

		public void ClearLowLevelPlanProfileList()
		{
			_lowLevelPlanProfileList = null;
//Begin Track #5220 - JScott - Error when opening multi-level plan
			_lowLevelSortedList = null;
//End Track #5220 - JScott - Error when opening multi-level plan
		}
		//End Track #4732
	}
}
