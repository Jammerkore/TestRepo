using System;
using System.Collections;
using System.Diagnostics;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The BasisProfile class identifies the Basis profile.
	/// </summary>

	[Serializable]
	public class BasisProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private PlanOpenParms _planOpenParms;
		private ProfileList _basisDetailProfileList;

//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		private BasisDetailProfile _modelBasisDetailProfile;
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//Begin Track #3879 - JScott - Null Reference Error
		private int _modelBasisDetailProfileIdx;
//End Track #3879 - JScott - Null Reference Error
		private ProfileList _displayablePlanWeekProfileList;
		private ProfileList _displayablePlanPeriodProfileList;
		private ProfileList _displayablePlanDetailProfileList;
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
		private ProfileList _extendedPlanPeriodProfileList;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
		private bool _detailsChecked;
		private bool _detailsAligned;
		// only used by OTS Forecasting
		private eTyLyType _basisType;
		private int _origWeekListCount;
		// BEGIN Issue 5578/5681 stodd 7.18.2008
		private bool _isDynToPlan;
		// END Issue 5578/5681 

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasisProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public BasisProfile(int aKey, string aName, PlanOpenParms aPlanOpenParms)
			: base(aKey)
		{
			_name = aName;
			_planOpenParms = aPlanOpenParms;
			_basisDetailProfileList = new ProfileList(eProfileType.BasisDetail);
			_isDynToPlan = false;	// Issue 
		}
	
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Basis;
			}
		}

		/// <summary>
		/// Gets the name of this profile.
		/// </summary>

		public string Name
		{
			get
			{
				return _name;
			}
            set //Track #5750
            {
                _name = value;
            }
        }

		/// <summary>
		/// Gets the ArrayList containing the BasisDetailProfile objects.
		/// </summary>

		public ProfileList BasisDetailProfileList
		{
			get
			{
				return _basisDetailProfileList;
			}
		}

		/// <summary>
		/// Describes what type of basis this is for TY/LY Trend processing. 
		/// </summary>

		public eTyLyType BasisType
		{
			get
			{
				return _basisType;
			}
			set 
			{ 
				_basisType = value; 
			}
		}

		/// <summary>
		/// Used by the forecast spread 
		/// </summary>
		/// 
		public bool IsDynToPlan
		{
			get
			{
				return _isDynToPlan;
			}
			set
			{
				_isDynToPlan = value;
			}
		}

		//========
		// METHODS
		//========

		public BasisProfile Copy(Session aSession, bool aCloneDateRanges)
		{
			BasisProfile newBasis;

			try
			{
				newBasis = new BasisProfile(_key, _name, _planOpenParms);
				newBasis._basisDetailProfileList = new ProfileList(eProfileType.BasisDetail);

				foreach (BasisDetailProfile detProf in _basisDetailProfileList)
				{
					newBasis._basisDetailProfileList.Add(detProf.Copy(aSession, aCloneDateRanges));
				}

//Begin Track #3879 - JScott - Null Reference Error
				if (newBasis._basisDetailProfileList.Count > _modelBasisDetailProfileIdx)
				{
					newBasis._modelBasisDetailProfile = (BasisDetailProfile)newBasis._basisDetailProfileList[_modelBasisDetailProfileIdx];
				}
				newBasis._modelBasisDetailProfileIdx = _modelBasisDetailProfileIdx;

//End Track #3879 - JScott - Null Reference Error
				if (_displayablePlanWeekProfileList != null)
				{
					newBasis._displayablePlanWeekProfileList = (ProfileList)_displayablePlanWeekProfileList.Clone();
				}

				if (_displayablePlanPeriodProfileList != null)
				{
					newBasis._displayablePlanPeriodProfileList = (ProfileList)_displayablePlanPeriodProfileList.Clone();
				}

				if (_displayablePlanDetailProfileList != null)
				{
					newBasis._displayablePlanDetailProfileList = (ProfileList)_displayablePlanDetailProfileList.Clone();
				}

				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
				if (_extendedPlanPeriodProfileList != null)
				{
					newBasis._extendedPlanPeriodProfileList = (ProfileList)_extendedPlanPeriodProfileList.Clone();
				}

				//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
				newBasis._detailsChecked = _detailsChecked;
				newBasis._detailsAligned = _detailsAligned;
				newBasis._basisType = _basisType;
				newBasis._origWeekListCount = _origWeekListCount;

				return newBasis;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool areDetailsAligned(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _detailsAligned;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ProfileList DisplayablePlanWeekProfileList(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _displayablePlanWeekProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ProfileList DisplayablePlanPeriodProfileList(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _displayablePlanPeriodProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ProfileList DisplayablePlanDetailProfileList(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _displayablePlanDetailProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		public double GetAverageDivisor(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _modelBasisDetailProfile.GetAverageDivisor(aSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool ContainsCurrentWeek(Session aSession)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _modelBasisDetailProfile.GetContainsCurrentWeek(aSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		public bool isPlanWeekDisplayable(Session aSession, int aWeekId)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				return _displayablePlanWeekProfileList.Contains(aWeekId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool isPlanPeriodDisplayable(Session aSession, int aPeriodId)
		{
			try
			{
				if (!_detailsChecked)
				{
					intCombineDetails(aSession);
				}
				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
				//return _displayablePlanPeriodProfileList.Contains(aPeriodId);
				return _displayablePlanPeriodProfileList.Contains(aPeriodId) || _extendedPlanPeriodProfileList.Contains(aPeriodId);
				//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void intCombineDetails(Session aSession)
		{
			int planWeek;
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//			BasisDetailProfile modelBasisDetailProfile = null;
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//Begin Track #3879 - JScott - Null Reference Error
			int i;
			BasisDetailProfile basisDetProf;
//End Track #3879 - JScott - Null Reference Error
			int maxWeeks;
			ProfileList weekList;
			//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
			PeriodProfile perProf;
			//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2

			try
			{
				_detailsChecked = true;
				_detailsAligned = true;
				_displayablePlanWeekProfileList = new ProfileList(eProfileType.Week);
				_displayablePlanPeriodProfileList = new ProfileList(eProfileType.Period);
				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
				_extendedPlanPeriodProfileList = new ProfileList(eProfileType.Period);
				//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2

				maxWeeks = -1;
//Begin Track #3879 - JScott - Null Reference Error
//				foreach (BasisDetailProfile basisDetailProfile in _basisDetailProfileList)
//				{
//					if (basisDetailProfile.GetWeekProfileList(aSession).Count > maxWeeks)
//					{
//						maxWeeks = basisDetailProfile.GetWeekProfileList(aSession).Count;
////Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
////						modelBasisDetailProfile = basisDetailProfile;
//						_modelBasisDetailProfile = basisDetailProfile;
////End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//					}
//				}
				for (i = 0; i < _basisDetailProfileList.Count; i++)
				{
					basisDetProf = (BasisDetailProfile)_basisDetailProfileList[i];

					if (basisDetProf.GetWeekProfileList(aSession).Count > maxWeeks)
					{
						maxWeeks = basisDetProf.GetWeekProfileList(aSession).Count;
						_modelBasisDetailProfile = basisDetProf;
						_modelBasisDetailProfileIdx = i;
					}
				}
//End Track #3879 - JScott - Null Reference Error

				foreach (BasisDetailProfile basisDetailProfile in _basisDetailProfileList)
				{
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//					if (basisDetailProfile.GetAlignedBasisWeekProfileList(aSession).Count != modelBasisDetailProfile.GetAlignedBasisWeekProfileList(aSession).Count)
					if (basisDetailProfile.GetAlignedBasisWeekProfileList(aSession).Count != _modelBasisDetailProfile.GetAlignedBasisWeekProfileList(aSession).Count)
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
					{
						_detailsAligned = false;
						break;
					}

					if (basisDetailProfile.GetPlanWeekIdFromBasisWeekId(aSession, basisDetailProfile.GetAlignedBasisWeekProfileList(aSession)[0].Key) !=
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//						modelBasisDetailProfile.GetPlanWeekIdFromBasisWeekId(aSession, modelBasisDetailProfile.GetAlignedBasisWeekProfileList(aSession)[0].Key))
						_modelBasisDetailProfile.GetPlanWeekIdFromBasisWeekId(aSession, _modelBasisDetailProfile.GetAlignedBasisWeekProfileList(aSession)[0].Key))
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
					{
						_detailsAligned = false;
						break;
					}
				}

//Begin Track #4595 - JScott - Avg In Str Inv not correct
//Begin Track #3796 - JScott - Fix Ending Inventory in basis
				weekList = _planOpenParms.GetWeekProfileList(aSession);
//End Track #3796 - JScott - Fix Ending Inventory in basis
//End Track #4595 - JScott - Avg In Str Inv not correct
			
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//				foreach (WeekProfile weekProfile in modelBasisDetailProfile.GetWeekProfileList(aSession))
				foreach (WeekProfile weekProfile in _modelBasisDetailProfile.GetWeekProfileList(aSession))
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
				{
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//					planWeek = modelBasisDetailProfile.GetPlanWeekIdFromBasisWeekId(aSession, weekProfile.Key);
					planWeek = _modelBasisDetailProfile.GetPlanWeekIdFromBasisWeekId(aSession, weekProfile.Key);
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

//Begin Track #3796 - JScott - Fix Ending Inventory in basis
//Begin Track #4595 - JScott - Avg In Str Inv not correct
//					if (!_displayablePlanWeekProfileList.Contains(planWeek))
					if (weekList.Contains(planWeek) && !_displayablePlanWeekProfileList.Contains(planWeek))
//End Track #3796 - JScott - Fix Ending Inventory in basis
//End Track #4595 - JScott - Avg In Str Inv not correct
					{
						_displayablePlanWeekProfileList.Add(aSession.Calendar.GetWeek(planWeek));
					}
				}

				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2
				//foreach (PeriodProfile periodProfile in _planOpenParms.GetPeriodProfileList(aSession))
				//{
				//    weekList = periodProfile.Weeks;

				//    foreach (WeekProfile weekProfile in weekList)
				//    {
				//        if (_displayablePlanWeekProfileList.Contains(weekProfile.Key))
				//        {
				//            _displayablePlanPeriodProfileList.Add(periodProfile);
				//            break;
				//        }
				//    }
				//}
				foreach (WeekProfile weekProf in _displayablePlanWeekProfileList)
				{
					if (!_displayablePlanPeriodProfileList.Contains(weekProf.Period.Key))
					{
						_displayablePlanPeriodProfileList.Add(weekProf.Period);
					}

					perProf = aSession.Calendar.GetQuarterForWeek(weekProf);

					if (!_extendedPlanPeriodProfileList.Contains(perProf.Key))
					{
						_extendedPlanPeriodProfileList.Add(perProf);
					}

					perProf = aSession.Calendar.GetSeasonForWeek(weekProf);

					if (!_extendedPlanPeriodProfileList.Contains(perProf.Key))
					{
						_extendedPlanPeriodProfileList.Add(perProf);
					}

					perProf = aSession.Calendar.GetYearForWeek(weekProf);

					if (!_extendedPlanPeriodProfileList.Contains(perProf.Key))
					{
						_extendedPlanPeriodProfileList.Add(perProf);
					}
				}
				//End Track #5121 - JScott - Add Year/Season/Quarter totals - Part 2

//Begin Track #4097 - JScott - Invalid Cast during Velocity Method
//				if (_planOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
				if (_planOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week || _planOpenParms.OpenPeriodAsWeeks)
//End Track #4097 - JScott - Invalid Cast during Velocity Method
				{
					_displayablePlanDetailProfileList = (ProfileList)_displayablePlanWeekProfileList.Clone();
				}
				else if (_planOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
				{
					_displayablePlanDetailProfileList = (ProfileList)_displayablePlanPeriodProfileList.Clone();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The BasisDetailProfile class describes a plan of a basis.
	/// </summary>

	[Serializable]
	public class BasisDetailProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private PlanOpenParms _planOpenParms;

		private HierarchyNodeProfile _hierarchyNodeProfile;
		private VersionProfile _versionProfile;
		private DateRangeProfile _dateRangeProfile;
		private eBasisIncludeExclude _includeExclude;
		private float _weight;
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		private double _averageDivisor;
		private bool _containsCurrentWeek;
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

		private bool _weeksCalculated;
		private ProfileList _detailDateProfileList;
		private ProfileList _weekProfileList;
		private ProfileList _alignedBasisWeekProfileList;
		private Hashtable _basisToPlanHash;
		private Hashtable _planToBasisHash;
		// only used by OTS Forecasting
		BasisDetailForecastInfo _forecastingInfo;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasisDetailProfile.
		/// </summary>

		public BasisDetailProfile(int aKey, PlanOpenParms aPlanOpenParms)
			: base(aKey)
		{
			_planOpenParms = aPlanOpenParms;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.BasisDetail;
			}
		}

		/// <summary>
		/// Gets or sets the HierarchyNodeProfile property.
		/// </summary>

		public HierarchyNodeProfile HierarchyNodeProfile
		{
			get
			{
				return _hierarchyNodeProfile;
			}
			set
			{
				_hierarchyNodeProfile = value;
			}
		}

		/// <summary>
		/// Gets or sets the VersionProfile property.
		/// </summary>

		public VersionProfile VersionProfile
		{
			get
			{
				return _versionProfile;
			}
			set
			{
				_versionProfile = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the DataRangeProfile property.
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
		/// Gets or sets the IncludeExclude property.
		/// </summary>

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

		/// <summary>
		/// Gets or sets the Weight property.
		/// </summary>

		public float Weight
		{
			get
			{
				return _weight;
			}
			set
			{
				_weight = value;
			}
		}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
		/// <summary>
		/// Gets the Weight property adjusted with the include/exclude factor.
		/// </summary>

		public float AdjustedWeight
		{
			get
			{
				if (_includeExclude == eBasisIncludeExclude.Exclude)
				{
					return _weight * -1;
				}
				else
				{
					return _weight;
				}
			}
		}

//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
		/// <summary>
		/// Gets or sets the first plan week that the basis aligns to
		/// </summary>
		public BasisDetailForecastInfo ForecastingInfo
		{
			get
			{
				return _forecastingInfo;
			}
			set 
			{ 
				_forecastingInfo = value; 
			}
		}

		//========
		// METHODS
		//========

		public BasisDetailProfile Copy(Session aSession, bool aCloneDateRanges)
		{
			BasisDetailProfile newBasisDetail;

			try
			{
				newBasisDetail = new BasisDetailProfile(_key, _planOpenParms);

				newBasisDetail._hierarchyNodeProfile = _hierarchyNodeProfile;
				newBasisDetail._versionProfile = _versionProfile;
				if (aCloneDateRanges &&
					_dateRangeProfile != null &&
					_dateRangeProfile.Key != Include.UndefinedCalendarDateRange)
				{
					newBasisDetail._dateRangeProfile = aSession.Calendar.GetDateRangeClone(_dateRangeProfile.Key);
				}
				else
				{
					newBasisDetail._dateRangeProfile = _dateRangeProfile;
				}
				newBasisDetail._includeExclude = _includeExclude;
				newBasisDetail._weight = _weight;
//Begin Track #3879 - JScott - Null Reference Error
				newBasisDetail._averageDivisor = _averageDivisor;
				newBasisDetail._containsCurrentWeek = _containsCurrentWeek;
//End Track #3879 - JScott - Null Reference Error

				newBasisDetail._weeksCalculated = _weeksCalculated;
				
				if (_weekProfileList != null)
				{
					newBasisDetail._weekProfileList = (ProfileList)_weekProfileList.Clone();
				}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4
				if (_detailDateProfileList != null)
				{
					newBasisDetail._detailDateProfileList = (ProfileList)_detailDateProfileList.Clone();
				}

//End Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4
//Begin Track #4009 - JScott - Error when opening basis with more weeks that plan
//				if (_weekProfileList != null)
//				{
//					newBasisDetail._alignedBasisWeekProfileList = (ProfileList)_alignedBasisWeekProfileList.Clone();
//				}
//
//				if (_weekProfileList != null)
//				{
//					newBasisDetail._basisToPlanHash = (Hashtable)_basisToPlanHash.Clone();
//				}
//
//				if (_weekProfileList != null)
//				{
//					newBasisDetail._planToBasisHash = (Hashtable)_planToBasisHash.Clone();
//				}
				if (_alignedBasisWeekProfileList != null)
				{
					newBasisDetail._alignedBasisWeekProfileList = (ProfileList)_alignedBasisWeekProfileList.Clone();
				}

				if (_basisToPlanHash != null)
				{
					newBasisDetail._basisToPlanHash = (Hashtable)_basisToPlanHash.Clone();
				}

				if (_planToBasisHash != null)
				{
					newBasisDetail._planToBasisHash = (Hashtable)_planToBasisHash.Clone();
				}
//End Track #4009 - JScott - Error when opening basis with more weeks that plan

				newBasisDetail._forecastingInfo = _forecastingInfo;

				return newBasisDetail;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ProfileList containing the list of detail Profiles.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The ProfileList of detail Profiles.
		/// </returns>

		public ProfileList GetDetailDateProfileList(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _detailDateProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the list of weeks.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The ProfileList of weeks.
		/// </returns>

		public ProfileList GetWeekProfileList(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _weekProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the aligned list of weeks.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The ProfileList of aligned weeks.
		/// </returns>

		public ProfileList GetAlignedBasisWeekProfileList(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _alignedBasisWeekProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4
		/// <summary>
		/// Gets the plan to basis hash list.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The plan to basis hash list.
		/// </returns>

		public Hashtable GetPlanToBasisHash(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _planToBasisHash;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//End Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4
		/// <summary>
		/// Returns the Basis week Id that cooresponds to the given Plan week Id.
		/// </summary>
		/// <param name="aPlanWeekId">
		/// The Plan week Id to search for.
		/// </param>
		/// <returns>
		/// The cooresponding Basis week Id.
		/// </returns>

		public int GetBasisWeekIdFromPlanWeekId(Session aSession, int aPlanWeekId)
		{
			object entry;

			try
			{
//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4
//				entry = _planToBasisHash[aPlanWeekId];
				entry = GetPlanToBasisHash(aSession)[aPlanWeekId];
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis - Part 4

				if (entry != null)
				{
					return (int)entry;
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_ExceededWeekLimit,
						MIDText.GetText(eMIDTextCode.msg_pl_ExceededWeekLimit));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
        
		/// <summary>
		/// Returns the Plan week Id that cooresponds to the given Basis week Id.
		/// </summary>
		/// <param name="aBasisWeekId">
		/// The Basis week Id to search for.
		/// </param>
		/// <returns>
		/// The cooresponding Plan week Id.
		/// </returns>

		public int GetPlanWeekIdFromBasisWeekId(Session aSession, int aBasisWeekId)
		{
			object entry;

			try
			{
				entry = _basisToPlanHash[aBasisWeekId];

				if (entry != null)
				{
					return (int)entry;
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_ExceededWeekLimit,
						MIDText.GetText(eMIDTextCode.msg_pl_ExceededWeekLimit));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		/// <summary>
		/// Returns the Average Divisor for this basis detail.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The Average Divisor for this basis detail.
		/// </returns>

		public double GetAverageDivisor(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _averageDivisor;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if this basis detail contains the current week.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// A boolean indicating if this basis detail contains the current week.
		/// </returns>

		public bool GetContainsCurrentWeek(Session aSession)
		{
			try
			{
				if (!_weeksCalculated)
				{
					intGetBasisWeeks(aSession);
				}

				return _containsCurrentWeek;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
		/// <summary>
		/// Used by forecasting to realign basis weeks to plan after the basis time periods have changed.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>

		public void RealignBasisWeeks(Session aSession, int alignToPlanWeek)
		{
			intGetBasisWeeks(aSession, alignToPlanWeek);
		}

		/// <summary>
		/// This method calls inGetBasisWeeks with 0 in the alignToPlanWeek parameter.
		/// </summary>

		private void intGetBasisWeeks(Session aSession)
		{
			intGetBasisWeeks(aSession, 0);
		}

		/// <summary>
		/// This method analyzes the basis weeks and lines them up with the plan weeks.  After finding a matching WeekInYear, the basis weeks are added to
		/// the WeekList to be read.  Any weeks that do not match a plan week are dropped.  A non-matching 53rd week in either Plan or Basis is skipped.  Hashtable
		/// entries are also added to allow for plan-basis and basis-plan week translations.
		/// </summary>

		private void intGetBasisWeeks(Session aSession, int alignToPlanWeek)
		{
//Begin Track #3972 - JScott - Error opening plan with Current Week in Basis line
			ProfileList periodProfileList;
//End Track #3972 - JScott - Error opening plan with Current Week in Basis line
//Begin Track #4009 - JScott - Error when opening basis with more weeks that plan
			int weekIdx;
			int perWeekCount;
//End Track #4009 - JScott - Error when opening basis with more weeks that plan
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
			WeekProfile currProf;
			double weeks;
			int days;
			double periods;

//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
			try
			{
				if (_planOpenParms.DateRangeProfile != null)
				{
					_weeksCalculated = true;
//Begin Track #4009 - JScott - Error when opening basis with more weeks that plan
////Begin Track #3972 - JScott - Error opening plan with Current Week in Basis line
////					switch (_dateRangeProfile.SelectedDateType)
////					{
////						case eCalendarDateType.Week:
//////Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//////							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//////							_detailDateProfileList = _weekProfileList;
////
////							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
////							_detailDateProfileList = _weekProfileList;
////
////							if (_versionProfile.Key == Include.FV_ActualRID)
////							{
////								currProf = aSession.Calendar.CurrentWeek;
////								weeks = 0;
////								_containsCurrentWeek = false;
////
////								foreach (WeekProfile weekProf in _detailDateProfileList)
////								{
////									if (weekProf == currProf)
////									{
////										weeks += (double)(aSession.Calendar.CurrentDate.DayInWeek - 1) / 7;
////										_containsCurrentWeek = true;
////										break;
////									}
////									else
////									{
////										weeks++;
////									}
////								}
////
////								if (_containsCurrentWeek)
////								{
////									_averageDivisor = weeks;
////								}
////								else
////								{
////									_averageDivisor = 0;
////								}
////							}
////							else
////							{
////								_averageDivisor = 0;
////							}
////	
//////End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
////							break;
////
////						case eCalendarDateType.Period:
//////Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
//////							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//////							_detailDateProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
////
////							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
////							_detailDateProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
////
////							if (_versionProfile.Key == Include.FV_ActualRID)
////							{
////								currProf = aSession.Calendar.CurrentWeek;
////								periods = 0;
////								_containsCurrentWeek = false;
////
////								foreach (PeriodProfile perProf in _detailDateProfileList)
////								{
////									days = 0;
////
////									foreach (WeekProfile weekProf in perProf.Weeks)
////									{
////										if (weekProf == currProf)
////										{
////											days += (aSession.Calendar.CurrentDate.DayInWeek - 1);
////											_containsCurrentWeek = true;
////											break;
////										}
////										else
////										{
////											days += 7;
////										}
////									}
////
////									if (_containsCurrentWeek)
////									{
////										periods += (double)days / ((double)perProf.WeeksInPeriod * 7);
////										break;
////									}
////									else
////									{
////										periods++;
////									}
////								}
////
////								if (_containsCurrentWeek)
////								{
////									_averageDivisor = periods;
////								}
////								else
////								{
////									_averageDivisor = 0;
////								}
////							}
////							else
////							{
////								_averageDivisor = 0;
////							}
////
//////End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
////							break;
////                    
////						default:
////							throw new MIDException (eErrorLevel.severe,
////								(int)eMIDTextCode.msg_pl_InvalidDateType,
////								MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
////					}
//					switch (_dateRangeProfile.SelectedDateType)
//					{
//						case eCalendarDateType.Week:
//							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//							periodProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//							_detailDateProfileList = _weekProfileList;
//							break;
//
//						case eCalendarDateType.Period:
//							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//							periodProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
//							_detailDateProfileList = periodProfileList;
//							break;
//                    
//						default:
//							throw new MIDException (eErrorLevel.severe,
//								(int)eMIDTextCode.msg_pl_InvalidDateType,
//								MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
//					}
//
//					switch (_planOpenParms.DateRangeProfile.SelectedDateType)
//					{
//						case eCalendarDateType.Week:
//
//							if (_versionProfile.Key == Include.FV_ActualRID)
//							{
//								currProf = aSession.Calendar.CurrentWeek;
//								weeks = 0;
//								_containsCurrentWeek = false;
//
//								foreach (WeekProfile weekProf in _weekProfileList)
//								{
//									if (weekProf == currProf)
//									{
//										weeks += (double)(aSession.Calendar.CurrentDate.DayInWeek - 1) / 7;
//										_containsCurrentWeek = true;
//										break;
//									}
//									else
//									{
//										weeks++;
//									}
//								}
//
//								if (_containsCurrentWeek)
//								{
//									_averageDivisor = weeks;
//								}
//								else
//								{
//									_averageDivisor = 0;
//								}
//							}
//							else
//							{
//								_averageDivisor = 0;
//							}
//							break;
//	
//						case eCalendarDateType.Period:
//					
//							if (_versionProfile.Key == Include.FV_ActualRID)
//							{
//								currProf = aSession.Calendar.CurrentWeek;
//								periods = 0;
//								_containsCurrentWeek = false;
//
//								foreach (PeriodProfile perProf in periodProfileList)
//								{
//									days = 0;
//
//									foreach (WeekProfile weekProf in perProf.Weeks)
//									{
//										if (weekProf == currProf)
//										{
//											days += (aSession.Calendar.CurrentDate.DayInWeek - 1);
//											_containsCurrentWeek = true;
//											break;
//										}
//										else
//										{
//											days += 7;
//										}
//									}
//
//									if (_containsCurrentWeek)
//									{
//										periods += (double)days / ((double)perProf.WeeksInPeriod * 7);
//										break;
//									}
//									else
//									{
//										periods++;
//									}
//								}
//
//								if (_containsCurrentWeek)
//								{
//									_averageDivisor = periods;
//								}
//								else
//								{
//									_averageDivisor = 0;
//								}
//							}
//							else
//							{
//								_averageDivisor = 0;
//							}
//
//							break;
//
//						default:
//							throw new MIDException (eErrorLevel.severe,
//								(int)eMIDTextCode.msg_pl_InvalidDateType,
//								MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
//					}
////End Track #3972 - JScott - Error opening plan with Current Week in Basis line
//
//					_alignedBasisWeekProfileList = new ProfileList(eProfileType.Week);
//					_basisToPlanHash = new Hashtable();
//					_planToBasisHash = new Hashtable();
//
////					if (alignByFiscalWeek)
////					{
////						intAlignByFiscalWeek(aSession);
////					}
////					else
////					{
//					intAlignByPlanWeek(aSession, alignToPlanWeek);
////					}
//

					switch (_dateRangeProfile.SelectedDateType)
					{
						case eCalendarDateType.Week:
							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
							periodProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
							_detailDateProfileList = _weekProfileList;
							break;

						case eCalendarDateType.Period:
							_weekProfileList = aSession.Calendar.GetWeekRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
							periodProfileList = aSession.Calendar.GetPeriodRange(_dateRangeProfile, _planOpenParms.DateRangeProfile);
							_detailDateProfileList = periodProfileList;
							break;
                    
						default:
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_InvalidDateType,
								MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
					}

					_alignedBasisWeekProfileList = new ProfileList(eProfileType.Week);
					_basisToPlanHash = new Hashtable();
					_planToBasisHash = new Hashtable();

//					if (alignByFiscalWeek)
//					{
//						intAlignByFiscalWeek(aSession);
//					}
//					else
//					{
					intAlignByPlanWeek(aSession, alignToPlanWeek);
//					}

//Begin Track #4097 - JScott - Invalid Cast during Velocity Method
//					switch (_planOpenParms.DateRangeProfile.SelectedDateType)
//					{
//						case eCalendarDateType.Week:
//
//							if (_versionProfile.Key == Include.FV_ActualRID)
//							{
//								currProf = aSession.Calendar.CurrentWeek;
//								weeks = 0;
//								_containsCurrentWeek = false;
//
//								foreach (WeekProfile weekProf in _alignedBasisWeekProfileList)
//								{
//									if (weekProf == currProf)
//									{
//										weeks += (double)(aSession.Calendar.CurrentDate.DayInWeek - 1) / 7;
//										_containsCurrentWeek = true;
//										break;
//									}
//									else
//									{
//										weeks++;
//									}
//								}
//
//								if (_containsCurrentWeek)
//								{
//									_averageDivisor = weeks;
//								}
//								else
//								{
//									_averageDivisor = 0;
//								}
//							}
//							else
//							{
//								_averageDivisor = 0;
//							}
//							break;
//	
//						case eCalendarDateType.Period:
//					
//							if (_versionProfile.Key == Include.FV_ActualRID)
//							{
//								currProf = aSession.Calendar.CurrentWeek;
//								periods = 0;
//								weekIdx = 0;
//								_containsCurrentWeek = false;
//
//								foreach (PeriodProfile perProf in _planOpenParms.DetailDateProfileList)
//								{
//									days = 0;
//
//									for (perWeekCount = 0; perWeekCount < perProf.WeeksInPeriod && weekIdx < _alignedBasisWeekProfileList.Count; perWeekCount++, weekIdx++)
//									{
//										if ((WeekProfile)_alignedBasisWeekProfileList[weekIdx] == currProf)
//										{
//											days += (aSession.Calendar.CurrentDate.DayInWeek - 1);
//											_containsCurrentWeek = true;
//											break;
//										}
//										else
//										{
//											days += 7;
//										}
//									}
//
//									if (_containsCurrentWeek || perWeekCount < perProf.WeeksInPeriod)
//									{
//										periods += (double)days / ((double)perProf.WeeksInPeriod * 7);
//										break;
//									}
//									else
//									{
//										periods++;
//									}
//								}
//
//								if (_containsCurrentWeek)
//								{
//									_averageDivisor = periods;
//								}
//								else
//								{
//									_averageDivisor = 0;
//								}
//							}
//							else
//							{
//								_averageDivisor = 0;
//							}
//
//							break;
//
//						default:
//							throw new MIDException (eErrorLevel.severe,
//								(int)eMIDTextCode.msg_pl_InvalidDateType,
//								MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
//					}
					if (_planOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week || _planOpenParms.OpenPeriodAsWeeks)
					{
						if (_versionProfile.Key == Include.FV_ActualRID || _versionProfile.Key == Include.FV_ModifiedRID)
						{
							currProf = aSession.Calendar.CurrentWeek;
							weeks = 0;
							_containsCurrentWeek = false;

							foreach (WeekProfile weekProf in _alignedBasisWeekProfileList)
							{
								if (weekProf == currProf)
								{
									weeks += (double)(aSession.Calendar.CurrentDate.DayInWeek - 1) / 7;
									_containsCurrentWeek = true;
									break;
								}
								else
								{
									weeks++;
								}
							}

							if (_containsCurrentWeek)
							{
								_averageDivisor = weeks;
							}
							else
							{
								_averageDivisor = 0;
							}
						}
						else
						{
							_averageDivisor = 0;
						}
					}
					else if (_planOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
					{
						if (_versionProfile.Key == Include.FV_ActualRID || _versionProfile.Key == Include.FV_ModifiedRID)
						{
							currProf = aSession.Calendar.CurrentWeek;
							periods = 0;
							weekIdx = 0;
							_containsCurrentWeek = false;

							foreach (PeriodProfile perProf in _planOpenParms.DetailDateProfileList)
							{
								days = 0;

								for (perWeekCount = 0; perWeekCount < perProf.NoOfWeeks && weekIdx < _alignedBasisWeekProfileList.Count; perWeekCount++, weekIdx++)
								{
									if ((WeekProfile)_alignedBasisWeekProfileList[weekIdx] == currProf)
									{
										days += (aSession.Calendar.CurrentDate.DayInWeek - 1);
										_containsCurrentWeek = true;
										break;
									}
									else
									{
										days += 7;
									}
								}

								if (_containsCurrentWeek || perWeekCount < perProf.NoOfWeeks)	// Issue 5121
								{
									periods += (double)days / ((double)perProf.NoOfWeeks * 7); // Issue 5121
									break;
								}
								else
								{
									periods++;
								}
							}

							if (_containsCurrentWeek)
							{
								_averageDivisor = periods;
							}
							else
							{
								_averageDivisor = 0;
							}
						}
						else
						{
							_averageDivisor = 0;
						}
					}
					else
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_InvalidDateType,
							MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
					}
//End Track #4097 - JScott - Invalid Cast during Velocity Method
//Begin Track #4009 - JScott - Error when opening basis with more weeks that plan
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//		private void intAlignByFiscalWeek(Session aSession)
//		{
//			ProfileList planWeekList;
//			int beginPlanIdx;
//			int beginBasisIdx;
//			int planIdx;
//			int basisIdx;
//			int i;
//			int planKey;
//			int basisKey;
//
//			try
//			{
//				planWeekList = _planOpenParms.GetWeekProfileList(aSession);
//
//				beginPlanIdx = 0;
//				beginBasisIdx = 0;
//
//				while (beginBasisIdx < _weekProfileList.Count &&
//					((WeekProfile)planWeekList[beginPlanIdx]).WeekInYear != ((WeekProfile)_weekProfileList[beginBasisIdx]).WeekInYear)
//				{
//					while (beginPlanIdx < planWeekList.Count &&
//						((WeekProfile)planWeekList[beginPlanIdx]).WeekInYear != ((WeekProfile)_weekProfileList[beginBasisIdx]).WeekInYear)
//					{
//						beginPlanIdx++;
//					}
//
//					if (beginPlanIdx == planWeekList.Count)
//					{
//						beginPlanIdx = 0;
//						beginBasisIdx++;
//					}
//				}
//
//				planIdx = beginPlanIdx;
//				basisIdx = beginBasisIdx;
//
//				while (planIdx < planWeekList.Count && basisIdx < _weekProfileList.Count)
//				{
//					_alignedBasisWeekProfileList.Add(_weekProfileList[basisIdx]);
//					basisIdx++;
//					planIdx++;
//
//					if (((WeekProfile)planWeekList[planIdx]).WeekInYear == 53 && ((WeekProfile)_weekProfileList[basisIdx]).WeekInYear == 1)
//					{
//						planIdx++;
//					}
//					else if (((WeekProfile)planWeekList[planIdx]).WeekInYear == 1 && ((WeekProfile)_weekProfileList[basisIdx]).WeekInYear == 53)
//					{
//						basisIdx++;
//					}
//				}
//
//				// Create Plan-to-Basis and Basis-to-Plan hash tables.  This logic builds the table 1 year in the past and 2 years in the future.  An attempt to reference beyond this range will result
//				// in an error.  Future enhancement -- add code to lookup logic that attempts to calculate the cooresponding index if it does not exist on this table.
//
//				i = 0;
//				planKey = aSession.Calendar.AddWeeks(planWeekList[beginPlanIdx].Key, -53);
//				basisKey = aSession.Calendar.AddWeeks(_weekProfileList[beginBasisIdx].Key, -53);
//
//				while (i < planWeekList.Count + 159)
//				{
//					if (aSession.Calendar.GetWeek(planKey).WeekInYear == 53 && aSession.Calendar.GetWeek(basisKey).WeekInYear == 1)
//					{
//						planKey = aSession.Calendar.AddWeeks(planKey, 1);
//					}
//					else if (aSession.Calendar.GetWeek(planKey).WeekInYear == 1 && aSession.Calendar.GetWeek(basisKey).WeekInYear == 53)
//					{
//						basisKey = aSession.Calendar.AddWeeks(basisKey, 1);
//					}
//
//					_basisToPlanHash.Add(basisKey, planKey);
//					_planToBasisHash.Add(planKey, basisKey);
//
//					i++;
//					planKey = aSession.Calendar.AddWeeks(planKey, 1);
//					basisKey = aSession.Calendar.AddWeeks(basisKey, 1);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}

		private void intAlignByPlanWeek(Session aSession, int alignToPlanWeek)
		{
			ProfileList planWeekList;
			int planIdx;
			int basisIdx;
			int i;
			int planKey;
			int basisKey;

			try
			{
				planWeekList = _planOpenParms.GetWeekProfileList(aSession);

				for (planIdx = 0, basisIdx = 0; planIdx < planWeekList.Count && basisIdx < _weekProfileList.Count; planIdx++, basisIdx++)
				{
					_alignedBasisWeekProfileList.Add(_weekProfileList[basisIdx]);
				}

				_basisToPlanHash.Add( _weekProfileList[0].Key, planWeekList[alignToPlanWeek].Key);
				_planToBasisHash.Add(planWeekList[alignToPlanWeek].Key,  _weekProfileList[0].Key);

				for (i = 1, planKey = aSession.Calendar.AddWeeks(planWeekList[alignToPlanWeek].Key, 1), basisKey = aSession.Calendar.AddWeeks(_weekProfileList[0].Key, 1);
					i < planWeekList.Count + 106;
					i++, planKey = aSession.Calendar.AddWeeks(planKey, 1), basisKey = aSession.Calendar.AddWeeks(basisKey, 1))
				{
					_basisToPlanHash.Add(basisKey, planKey);
					_planToBasisHash.Add(planKey, basisKey);
				}

				for (i = -1, planKey = aSession.Calendar.AddWeeks(planWeekList[alignToPlanWeek].Key, -1), basisKey = aSession.Calendar.AddWeeks(_weekProfileList[0].Key, -1);
					i > -53;
					i--, planKey = aSession.Calendar.AddWeeks(planKey, -1), basisKey = aSession.Calendar.AddWeeks(basisKey, -1))
				{
					_basisToPlanHash.Add(basisKey, planKey);
					_planToBasisHash.Add(planKey, basisKey);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Holds inforamtion by basis detail concerning the forecasting in progress.
	/// </summary>
	[Serializable]
	public class BasisDetailForecastInfo
	{
		//=======
		// FIELDS
		//=======
		private int _origWeekListCount;
		private bool _shiftWeeksWithPlanWeek;
		private WeekProfile _planWeek;
		private ProfileList _basisPeriodList;

		//=============
		// PROPERTIES
		//=============

		/// <summary>
		/// Gets or sets the week list count. 
		/// Forecasting changes the week list, but needs to know the number of weeks in the original week list.
		/// *Used in forecasting only*
		/// </summary>
		public int OrigWeekListCount
		{
			get
			{
				return _origWeekListCount;
			}
			set 
			{ 
				_origWeekListCount = value; 
			}
		}

		/// <summary>
		/// Gets or sets the variable that tracks whether this basis detail is Static or Dynamic to Current; in which case
		/// it needs to shift along with the plan week as the plan week moves.
		/// </summary>
		public bool ShiftWeeksWithPlanWeek
		{
			get
			{
				return _shiftWeeksWithPlanWeek;
			}
			set 
			{ 
				_shiftWeeksWithPlanWeek = value; 
			}
		}

		/// <summary>
		/// Gets or Sets the first Plan week that the dynamic period Basis lines up to.
		/// </summary>
		public WeekProfile PlanWeek
		{
			get
			{
				return _planWeek;
			}
			set 
			{ 
				_planWeek = value; 
			}
		}

		/// <summary>
		/// Gets or sets the current dynamic period basis.
		/// </summary>
		public ProfileList BasisPeriodList
		{
			get
			{
				return _basisPeriodList;
			}
			set 
			{ 
				_basisPeriodList = value; 
			}
		}

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasisDetailForecastInfo.
		/// </summary>
		public BasisDetailForecastInfo()
		{
			_origWeekListCount = 0;
		}

		public WeekProfile ShiftDateRange(MRSCalendar calendar)
		{
			// add next period
			int periodCount = _basisPeriodList.Count;
			PeriodProfile lastPeriod = (PeriodProfile)_basisPeriodList[periodCount - 1];
			int nextPeriodKey = calendar.AddPeriods(lastPeriod.Key, 1);
			PeriodProfile nextPeriod = calendar.GetPeriod(nextPeriodKey);
			_basisPeriodList.Add(nextPeriod);
			// remove first period
			PeriodProfile firstPeriod = (PeriodProfile)_basisPeriodList[0];
			_basisPeriodList.Remove(firstPeriod);

			// update week count
			_origWeekListCount = 0;
			foreach (PeriodProfile pp in _basisPeriodList.ArrayList)
			{
				_origWeekListCount += pp.NoOfWeeks;	// Issue 5121
			}

			// update plan week
            // Begin TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
            int nextPlanWeekKey = calendar.AddWeeks(_planWeek.Key, firstPeriod.NoOfWeeks);	// Issue 5121
            //int nextPlanWeekKey = calendar.AddWeeks(_planWeek.Key, (firstPeriod.NoOfWeeks - _planWeek.WeekInPeriod + 1));
            // End TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
			_planWeek = calendar.GetWeek(nextPlanWeekKey);

			return _planWeek;
		}
	}
}
