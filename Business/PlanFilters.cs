using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	public delegate bool EvaluteRoutineSimpleDelegate(
		double[] aFirstValues,
		double[] aSecondValues,
		eFilterTimeModifyer aFirstTimeMod,
		eFilterTimeModifyer aSecondTimeMod,
		eFilterComparisonType aComparisonType,
		bool aNegateComparison,
		int aFirstIndex,
		double aFirstValue,
		double aSecondValue);

	public delegate bool EvaluteRoutineComplexDelegate(
		double[] aFirstValues,
		double[] aPctValues,
		double[] aSecondValues,
		eFilterTimeModifyer aFirstTimeMod,
		eFilterTimeModifyer aPctTimeMod,
		eFilterTimeModifyer aSecondTimeMod,
		eFilterComparisonType aComparisonType,
		bool aNegateComparison,
		bool aPctOf,
		bool aPctChange,
		int aFirstIndex,
		double aFirstValue,
		double aPctValue,
		double aSecondValue);

	/// <summary>
	/// This StoreVariableFilter filters out variables that are not store variables.
	/// </summary>

	[Serializable]
	public class StoreVariableFilter : Filter
	{
		//=======
		// FIELDS
		//=======

		PlanCubeGroup _planCubeGroup;

		//=============
		// CONSTRUCTORS
		//=============

		public StoreVariableFilter(PlanCubeGroup aPlanCubeGroup)
		{
			_planCubeGroup = aPlanCubeGroup;
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Variable;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Applies this Filter to a given ProfileList.
		/// </summary>
		/// <remarks>
		/// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
		/// </remarks>
		/// <param name="aProfileList">
		/// The ArrayList to apply the Filter to.
		/// </param>
		/// <returns>
		/// An ArrayList containing the selected Profiles.
		/// </returns>

		override public ProfileList ApplyFilter(ProfileList aProfileList)
		{
			ProfileList selectedVariableProfList;

			try
			{
				selectedVariableProfList = new ProfileList(aProfileList.ProfileType);

				foreach (ComputationVariableProfile varProf in aProfileList)
				{
					if (varProf.VariableCategory == eVariableCategory.Both || varProf.VariableCategory == eVariableCategory.Store)
					{
						selectedVariableProfList.Add(varProf);
					}
				}

				return selectedVariableProfList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This ChainVariableFilter filters out variables that are not chain variables.
	/// </summary>

	[Serializable]
	public class ChainVariableFilter : Filter
	{
		//=======
		// FIELDS
		//=======

		PlanCubeGroup _planCubeGroup;

		//=============
		// CONSTRUCTORS
		//=============

		public ChainVariableFilter(PlanCubeGroup aPlanCubeGroup)
		{
			_planCubeGroup = aPlanCubeGroup;
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Variable;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Applies this Filter to a given ProfileList.
		/// </summary>
		/// <remarks>
		/// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
		/// </remarks>
		/// <param name="aProfileList">
		/// The ArrayList to apply the Filter to.
		/// </param>
		/// <returns>
		/// An ArrayList containing the selected Profiles.
		/// </returns>

		override public ProfileList ApplyFilter(ProfileList aProfileList)
		{
			ProfileList selectedVariableProfList;

			try
			{
				selectedVariableProfList = new ProfileList(aProfileList.ProfileType);

				foreach (ComputationVariableProfile varProf in aProfileList)
				{
					if (varProf.VariableCategory == eVariableCategory.Both || varProf.VariableCategory == eVariableCategory.Chain)
					{
						selectedVariableProfList.Add(varProf);
					}
				}

				return selectedVariableProfList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This EligibilityFilter filters out ineligible stores.
	/// </summary>

	[Serializable]
	public class EligibilityFilter : Filter
	{
		//=======
		// FIELDS
		//=======

		PlanCubeGroup _planCubeGroup;

		//=============
		// CONSTRUCTORS
		//=============

		public EligibilityFilter(PlanCubeGroup aPlanCubeGroup)
		{
			_planCubeGroup = aPlanCubeGroup;
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Store;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Applies this Filter to a given ProfileList.
		/// </summary>
		/// <remarks>
		/// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
		/// </remarks>
		/// <param name="aProfileList">
		/// The ArrayList to apply the Filter to.
		/// </param>
		/// <returns>
		/// An ArrayList containing the selected Profiles.
		/// </returns>

		override public ProfileList ApplyFilter(ProfileList aProfileList)
		{
			ProfileList selectedStoreProfList;

			try
			{
				selectedStoreProfList = new ProfileList(aProfileList.ProfileType);

				foreach (StoreProfile storeProf in aProfileList)
				{
					if (isStoreEligible(storeProf))
					{
						selectedStoreProfList.Add(storeProf);
					}
				}

				return selectedStoreProfList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if a given StoreProfile is eligible or ineligible.
		/// </summary>
		/// <param name="aStoreProf">
		/// The StoreProfile of the store to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store is eligible.
		/// </returns>

		private bool isStoreEligible(StoreProfile aStoreProf)
		{
			try
			{
				int nodeRID = _planCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key;

                if (_planCubeGroup.OpenParms.EligibilityNodeKey != Include.Undefined)
                {
                    nodeRID = _planCubeGroup.OpenParms.EligibilityNodeKey;
                }

				foreach (WeekProfile weekProf in _planCubeGroup.OpenParms.GetWeekProfileList(_planCubeGroup.SAB.ApplicationServerSession))
				{
					if (_planCubeGroup.Transaction.GetStoreEligibilityForSales(
                        _planCubeGroup.OpenParms.RequestingApplication,
                        nodeRID, 
                        aStoreProf.Key, 
                        weekProf.Key) ||
						_planCubeGroup.Transaction.GetStoreEligibilityForStock(
                            _planCubeGroup.OpenParms.RequestingApplication,
                            nodeRID, 
                            aStoreProf.Key, 
                            weekProf.Key))
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This CustomStoreFilter filters out stores based upon a custom User definition.
	/// </summary>

    //[Serializable]
    //public class CustomStoreFilter : Filter, IDisposable
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    private SessionAddressBlock _SAB;
    //    private ApplicationSessionTransaction _transaction;
    //    private Session _currentSession;
    //    private PlanCubeGroup _currentPlanCubeGroup;
    //    private PlanOpenParms _currentPlanOpenParms;
    //    private int _filterID;
    //    private StoreFilterData _filterDL;
    //    private StoreFilterDefinition _filterDef;
    //    private Hashtable _filterCubeGroupHash;
    //    private IDictionaryEnumerator _enumerator;
    //    private DateTime _nullDate = new DateTime(1, 1, 1);

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public CustomStoreFilter(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        Session aCurrentSession,
    //        PlanCubeGroup aCurrentPlanCubeGroup,
    //        int aFilterID)
    //    {
    //        try
    //        {
    //            _SAB = aSAB;
    //            _transaction = aTransaction;
    //            _currentSession = aCurrentSession;
    //            _currentPlanCubeGroup = aCurrentPlanCubeGroup;
    //            _filterID = aFilterID;

    //            if (_currentPlanCubeGroup != null)
    //            {
    //                _currentPlanOpenParms = _currentPlanCubeGroup.OpenParms;
    //            }
    //            else
    //            {
    //                _currentPlanOpenParms = null;
    //            }

    //            _filterDL = new StoreFilterData();
    //            _filterCubeGroupHash = new Hashtable();

    //            _filterDef = new StoreFilterDefinition(
    //                _SAB,
    //                _currentSession,
    //                _filterDL,
    //                null,
    //                _transaction.GetProfileList(eProfileType.Version),
    //                _transaction.GetProfileList(eProfileType.Variable),
    //                _transaction.GetProfileList(eProfileType.TimeTotalVariable),
    //                _filterID);

    //            GetCubeInfo();

    //            //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
    //            if (!_filterDef.FilterOutdatedInformation)
    //            {
    //                _enumerator = _filterCubeGroupHash.GetEnumerator();

    //                while (_enumerator.MoveNext())
    //                {
    //                    ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
    //                }
    //            }
    //            //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed

    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    public CustomStoreFilter(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        Session aCurrentSession,
    //        int aFilterID,
    //        PlanOpenParms aCurrentPlanOpenParms)
    //    {
    //        try
    //        {
    //            _SAB = aSAB;
    //            _transaction = aTransaction;
    //            _currentSession = aCurrentSession;
    //            _currentPlanOpenParms = aCurrentPlanOpenParms;
    //            _filterID = aFilterID;

    //            _currentPlanCubeGroup = null;

    //            _filterDL = new StoreFilterData();
    //            _filterCubeGroupHash = new Hashtable();

    //            _filterDef = new StoreFilterDefinition(
    //                _SAB,
    //                _currentSession,
    //                _filterDL,
    //                null,
    //                _transaction.GetProfileList(eProfileType.Version),
    //                _transaction.GetProfileList(eProfileType.Variable),
    //                _transaction.GetProfileList(eProfileType.TimeTotalVariable),
    //                _filterID);

    //            GetCubeInfo();

    //            _enumerator = _filterCubeGroupHash.GetEnumerator();

    //            while (_enumerator.MoveNext())
    //            {
    //                ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    override protected void Dispose(bool disposing)
    //    {
    //        try
    //        {
    //            if (disposing)
    //            {
    //                foreach (CubeGroupHashEntry cghe in _filterCubeGroupHash.Values)
    //                {
    //                    cghe.FilterCubeGroup.Dispose();
    //                }
    //            }

    //            base.Dispose(disposing);
    //        }
    //        catch (Exception)
    //        {
    //        }
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    override public eProfileType ProfileType
    //    {
    //        get
    //        {
    //            return eProfileType.Store;
    //        }
    //    }

    //    // BEGIN Issue 5727 stodd 
    //    public bool FilterOutdatedInformation
    //    {
    //        get
    //        {
    //            if (_filterDef == null)
    //                return false;
    //            else
    //                return _filterDef.FilterOutdatedInformation;
    //        }
    //    }
    //    // END Issue 5727 stodd 

    //    //========
    //    // METHODS
    //    //========

    //    /// <summary>
    //    /// Applies this Filter to a given ProfileList.
    //    /// </summary>
    //    /// <remarks>
    //    /// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
    //    /// </remarks>
    //    /// <param name="aProfileList">
    //    /// The ArrayList to apply the Filter to.
    //    /// </param>
    //    /// <returns>
    //    /// An ArrayList containing the selected Profiles.
    //    /// </returns>

    //    override public ProfileList ApplyFilter(ProfileList aProfileList)
    //    {
    //        IEnumerator enumerator;
    //        QueryOperand operand = null;
    //        ProfileList profileList;
    //        ProfileList newProfileList;
    //        ProfileList dateProfileList;
    //        ArrayList modDataOperandList;

    //        try
    //        {
    //            newProfileList = aProfileList;

    //            if (_filterDef.AttrOperandList.Count > 0)
    //            {
    //                profileList = newProfileList;
    //                newProfileList = new ProfileList(eProfileType.Store);

    //                foreach (StoreProfile storeProf in profileList)
    //                {
    //                    enumerator = _filterDef.AttrOperandList.GetEnumerator();
    //                    if (ProcessAttr(enumerator, ref operand, storeProf))
    //                    {
    //                        newProfileList.Add(storeProf);
    //                    }
    //                }
    //            }

    //            if (_filterDef.DataOperandList.Count > 0)
    //            {
    //                profileList = newProfileList;
    //                newProfileList = new ProfileList(eProfileType.Store);

    //                modDataOperandList = PreProcessDataOperandList(_filterDef.DataOperandList);

    //                if (_currentPlanOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
    //                {
    //                    dateProfileList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(_currentPlanOpenParms.DateRangeProfile, _currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
    //                }
    //                else
    //                {
    //                    dateProfileList = _SAB.ApplicationServerSession.Calendar.GetPeriodRange(_currentPlanOpenParms.DateRangeProfile, _currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
    //                }

    //                foreach (StoreProfile storeProf in profileList)
    //                {
    //                    foreach (DateProfile dateProf in dateProfileList)
    //                    {
    //                        enumerator = modDataOperandList.GetEnumerator();
    //                        if (ProcessData(enumerator, ref operand, storeProf, dateProf))
    //                        {
    //                            newProfileList.Add(storeProf);
    //                            break;
    //                        }
    //                    }
    //                }
    //            }

    //            return newProfileList;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private void GetCubeInfo()
    //    {
    //        IEnumerator enumerator;
    //        QueryOperand operand;
    //        DataQueryVariableOperand variableOperand = null;
    //        //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //        //bool currentCubeGroup;
    //        bool chainCurrentCubeGroup;
    //        bool storeCurrentCubeGroup;
    //        //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //        bool buildGroup;
    //        PlanCubeGroupWaferCubeFlags cumulatedFlags;
    //        CubeGroupHashEntry cubeGroupHashEntry;

    //        try
    //        {
    //            cumulatedFlags = new PlanCubeGroupWaferCubeFlags();

    //            enumerator = _filterDef.DataOperandList.GetEnumerator();
    //            operand = GetNextOperand(enumerator);

    //            while (true)
    //            {
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (operand == null)
    //                {
    //                    return;
    //                }
    //                // End TT#189 MD

    //                if (operand.GetType().IsSubclassOf(typeof(DataQueryVariableOperand)))
    //                {
    //                    //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    //currentCubeGroup = false;
    //                    //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    variableOperand = (DataQueryVariableOperand)operand;

    //                    if (variableOperand.NodeProfile == null || variableOperand.VersionProfile == null || variableOperand.DateRangeProfile == null)
    //                    {
    //                        if (_currentPlanOpenParms == null)
    //                        {
    //                            throw new FilterUsesCurrentPlanException();
    //                        }
    //                    }

    //                    //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    //if (variableOperand.NodeProfile == null && variableOperand.VersionProfile == null && variableOperand.DateRangeProfile == null)
    //                    //{
    //                    //    currentCubeGroup = true;
    //                    //}

    //                    //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    switch (variableOperand.CubeModifyer)
    //                    {
    //                        case eFilterCubeModifyer.StoreDetail:
    //                        case eFilterCubeModifyer.None:
    //                            cumulatedFlags.isStorePlan = true;
    //                            cumulatedFlags.isStore = true;
    //                            break;
    //                        case eFilterCubeModifyer.StoreTotal:
    //                            cumulatedFlags.isStorePlan = true;
    //                            cumulatedFlags.isStoreTotal = true;
    //                            break;
    //                        case eFilterCubeModifyer.StoreAverage:
    //                            cumulatedFlags.isStorePlan = true;
    //                            cumulatedFlags.isStoreTotal = true;
    //                            break;
    //                        case eFilterCubeModifyer.ChainDetail:
    //                            cumulatedFlags.isChainPlan = true;
    //                            break;
    //                    }

    //                    if (variableOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
    //                    {
    //                        cumulatedFlags.isDate = true;
    //                    }

    //                    //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    if (_currentPlanCubeGroup != null)
    //                    {
    //                        chainCurrentCubeGroup = true;
    //                        storeCurrentCubeGroup = true;

    //                        if (variableOperand.DateRangeProfile != null)
    //                        {
    //                            if (variableOperand.DateRangeProfile.CompareTo(_currentPlanOpenParms.DateRangeProfile) != 0)
    //                            {
    //                                chainCurrentCubeGroup = false;
    //                                storeCurrentCubeGroup = false;
    //                            }
    //                        }

    //                        if (cumulatedFlags.isChainPlan)
    //                        {
    //                            if (variableOperand.NodeProfile != null)
    //                            {
    //                                if (variableOperand.NodeProfile.Key != _currentPlanOpenParms.ChainHLPlanProfile.NodeProfile.Key)
    //                                {
    //                                    chainCurrentCubeGroup = false;
    //                                }
    //                            }

    //                            if (variableOperand.VersionProfile != null)
    //                            {
    //                                if (variableOperand.VersionProfile.Key != _currentPlanOpenParms.ChainHLPlanProfile.VersionProfile.Key)
    //                                {
    //                                    chainCurrentCubeGroup = false;
    //                                }
    //                            }
    //                        }

    //                        if (cumulatedFlags.isStorePlan)
    //                        {
    //                            if (variableOperand.NodeProfile != null)
    //                            {
    //                                if (variableOperand.NodeProfile.Key != _currentPlanOpenParms.StoreHLPlanProfile.NodeProfile.Key)
    //                                {
    //                                    storeCurrentCubeGroup = false;
    //                                }
    //                            }

    //                            if (variableOperand.VersionProfile != null)
    //                            {
    //                                if (variableOperand.VersionProfile.Key != _currentPlanOpenParms.StoreHLPlanProfile.VersionProfile.Key)
    //                                {
    //                                    storeCurrentCubeGroup = false;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        chainCurrentCubeGroup = false;
    //                        storeCurrentCubeGroup = false;
    //                    }

    //                    //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                    if (variableOperand.DateRangeProfile == null)
    //                    {
    //                        variableOperand.DateRangeProfile = _currentPlanOpenParms.DateRangeProfile;
    //                    }
    //                    else
    //                    {
    //                        if (variableOperand.DateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan)
    //                        {
    //                            if (_currentPlanOpenParms == null)
    //                            {
    //                                throw new FilterUsesCurrentPlanException();
    //                            }

    //                            variableOperand.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(variableOperand.DateRangeProfile.Key, _currentPlanOpenParms.DateRangeProfile.Key);
    //                        }
    //                    }
    //                    //Begin Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                    //// Begin Track #6097 stodd - A&F HOL Null Ref resolving filter
    //                    ////if (cumulatedFlags.isChainPlan)
    //                    ////{
    //                    //// End Track #6097
    //                    if (cumulatedFlags.isChainPlan)
    //                    {
    //                        //End Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                        buildGroup = false;

    //                        //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                        //if (currentCubeGroup && _currentPlanCubeGroup != null)
    //                        if (chainCurrentCubeGroup && _currentPlanCubeGroup != null)
    //                        //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                        {
    //                            if (_currentPlanCubeGroup.GetCube(eCubeType.ChainPlanWeekDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.ChainPlanPeriodDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.ChainPlanDateTotal) == null)
    //                            {
    //                                buildGroup = true;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            buildGroup = true;
    //                        }

    //                        if (variableOperand.NodeProfile == null)
    //                        {
    //                            variableOperand.NodeProfile = _currentPlanOpenParms.ChainHLPlanProfile.NodeProfile;
    //                        }

    //                        if (variableOperand.VersionProfile == null)
    //                        {
    //                            variableOperand.VersionProfile = _currentPlanOpenParms.ChainHLPlanProfile.VersionProfile;
    //                        }

    //                        if (buildGroup)
    //                        {
    //                            cubeGroupHashEntry = new CubeGroupHashEntry(variableOperand.NodeProfile.Key, variableOperand.VersionProfile.Key, variableOperand.DateRangeProfile.Key);

    //                            if (!_filterCubeGroupHash.Contains(cubeGroupHashEntry))
    //                            {
    //                                InitializeCubeGroupHashEntry(cubeGroupHashEntry, variableOperand.DateRangeProfile);
    //                                _filterCubeGroupHash.Add(cubeGroupHashEntry, cubeGroupHashEntry);
    //                            }
    //                            else
    //                            {
    //                                cubeGroupHashEntry = (CubeGroupHashEntry)_filterCubeGroupHash[cubeGroupHashEntry];
    //                            }

    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = variableOperand.NodeProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = variableOperand.VersionProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();

    //                            //Begin Track #6251 - JScott - Get System Null Ref Excp using filter
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanWeekDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanPeriodDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanDateTotal);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanWeekDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanPeriodDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanDateTotal);
    //                            //End Track #6251 - JScott - Get System Null Ref Excp using filter

    //                            variableOperand.PlanCubeGroup = cubeGroupHashEntry.FilterCubeGroup;
    //                        }
    //                        else
    //                        {
    //                            variableOperand.PlanCubeGroup = _currentPlanCubeGroup;
    //                        }
    //                        //Begin Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                        //// Begin Track #6097 stodd - A&F HOL Null Ref resolving filter
    //                        ////}
    //                        ////else if (cumulatedFlags.isStorePlan)
    //                        //// End Track #6097
    //                        //if (cumulatedFlags.isStorePlan)
    //                    }
    //                    else if (cumulatedFlags.isStorePlan)
    //                    //End Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                    {
    //                        buildGroup = false;

    //                        //Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                        //if (currentCubeGroup && _currentPlanCubeGroup != null)
    //                        if (storeCurrentCubeGroup && _currentPlanCubeGroup != null)
    //                        //End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
    //                        {
    //                            //Begin Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                            //if (_currentPlanCubeGroup.GetCube(eCubeType.StorePlanWeekDetail) == null ||
    //                            if (_currentPlanCubeGroup.GetCube(eCubeType.ChainPlanWeekDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.ChainPlanPeriodDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.ChainPlanDateTotal) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanWeekDetail) == null ||
    //                                //End Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanPeriodDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanDateTotal) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalWeekDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalPeriodDetail) == null ||
    //                                _currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalDateTotal) == null)
    //                            {
    //                                buildGroup = true;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            buildGroup = true;
    //                        }

    //                        if (variableOperand.NodeProfile == null)
    //                        {
    //                            variableOperand.NodeProfile = _currentPlanOpenParms.StoreHLPlanProfile.NodeProfile;
    //                        }

    //                        if (variableOperand.VersionProfile == null)
    //                        {
    //                            variableOperand.VersionProfile = _currentPlanOpenParms.StoreHLPlanProfile.VersionProfile;
    //                        }

    //                        if (buildGroup)
    //                        {
    //                            cubeGroupHashEntry = new CubeGroupHashEntry(variableOperand.NodeProfile.Key, variableOperand.VersionProfile.Key, variableOperand.DateRangeProfile.Key);

    //                            if (!_filterCubeGroupHash.Contains(cubeGroupHashEntry))
    //                            {
    //                                InitializeCubeGroupHashEntry(cubeGroupHashEntry, variableOperand.DateRangeProfile);
    //                                _filterCubeGroupHash.Add(cubeGroupHashEntry, cubeGroupHashEntry);
    //                            }
    //                            else
    //                            {
    //                                cubeGroupHashEntry = (CubeGroupHashEntry)_filterCubeGroupHash[cubeGroupHashEntry];
    //                            }

    //                            //Begin Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = variableOperand.NodeProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = variableOperand.VersionProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
    //                            //End Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                            cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile = variableOperand.NodeProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.VersionProfile = variableOperand.VersionProfile;
    //                            cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();

    //                            //Begin Track #6251 - JScott - Get System Null Ref Excp using filter
    //                            ////Begin Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanWeekDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanPeriodDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.ChainCubeList.Add(eCubeType.ChainPlanDateTotal);
    //                            ////End Track #6194 - JScott - Filter does not work if chain and store versions do not match
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanWeekDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanPeriodDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanDateTotal);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanGroupTotalWeekDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanGroupTotalPeriodDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanGroupTotalDateTotal);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanStoreTotalWeekDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanStoreTotalPeriodDetail);
    //                            //cubeGroupHashEntry.FilterOpenParms.StoreCubeList.Add(eCubeType.StorePlanStoreTotalDateTotal);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanWeekDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanPeriodDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanDateTotal);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanWeekDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanPeriodDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanDateTotal);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalWeekDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalPeriodDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalDateTotal);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalWeekDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalPeriodDetail);
    //                            cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalDateTotal);
    //                            //End Track #6251 - JScott - Get System Null Ref Excp using filter

    //                            variableOperand.PlanCubeGroup = cubeGroupHashEntry.FilterCubeGroup;
    //                        }
    //                        else
    //                        {
    //                            variableOperand.PlanCubeGroup = _currentPlanCubeGroup;
    //                        }
    //                    }
    //                }

    //                operand = GetNextOperand(enumerator);
    //            }
    //        }
    //        // Begin TT#189 MD - JSmith - Filter Performance
    //        //catch (EndOfOperandsException)
    //        //{
    //        //}
    //        // End TT#189 MD
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private void InitializeCubeGroupHashEntry(CubeGroupHashEntry aCubeGroupHashEntry, DateRangeProfile aDateRangeProf)
    //    {
    //        try
    //        {
    //            aCubeGroupHashEntry.FilterCubeGroup = new FilterCubeGroup(_SAB, _transaction);
    //            //Begin Track #6251 - JScott - Get System Null Ref Excp using filter
    //            //aCubeGroupHashEntry.FilterOpenParms = new FilterOpenParms(ePlanSessionType.StoreSingleLevel, _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
    //            aCubeGroupHashEntry.FilterOpenParms = new FilterOpenParms(ePlanSessionType.None, _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
    //            //End Track #6251 - JScott - Get System Null Ref Excp using filter
    //            aCubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = null;
    //            aCubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = null;
    //            aCubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile = null;
    //            aCubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.VersionProfile = null;
    //            aCubeGroupHashEntry.FilterOpenParms.DateRangeProfile = aDateRangeProf;
    //            aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = Include.NoRID;
    //            aCubeGroupHashEntry.FilterOpenParms.IneligibleStores = true;
    //            aCubeGroupHashEntry.FilterOpenParms.SimilarStores = false;

    //            if (_currentPlanOpenParms.StoreGroupRID == Include.NoRID)
    //            {
    //                aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = Include.AllStoreGroupRID;
    //            }
    //            else
    //            {
    //                aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = _currentPlanOpenParms.StoreGroupRID;
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool ProcessAttr(IEnumerator aEnumerator, ref QueryOperand aOperand, StoreProfile aStoreProf)
    //    {
    //        bool currentReturn = false;

    //        try
    //        {
    //            aOperand = GetNextOperand(aEnumerator);
    //            // Begin TT#189 MD - JSmith - Filter Performance
    //            if (aOperand == null)
    //            {
    //                return currentReturn;
    //            }
    //            // End TT#189 MD

    //            if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
    //            {
    //                currentReturn = ProcessAttr(aEnumerator, ref aOperand, aStoreProf);
    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }
    //            else
    //            {
    //                if (aOperand.GetType() == typeof(AttrQueryAttributeMainOperand))
    //                {
    //                    foreach (StoreGroupLevelProfile sglp in ((AttrQueryAttributeMainOperand)aOperand).AttributeSetProfList)
    //                    {
    //                        if (sglp.Stores.Contains(aStoreProf.Key))
    //                        {
    //                            currentReturn = true;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (((AttrQueryStoreMainOperand)aOperand).StoreProfileList.Contains(aStoreProf))
    //                    {
    //                        currentReturn = true;
    //                    }
    //                }

    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }

    //            if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
    //            {
    //                return currentReturn;
    //            }
    //            else if (aOperand.GetType() == typeof(GenericQueryAndOperand))
    //            {
    //                return ProcessAttr(aEnumerator, ref aOperand, aStoreProf) && currentReturn;
    //            }
    //            else
    //            {
    //                return ProcessAttr(aEnumerator, ref aOperand, aStoreProf) || currentReturn;
    //            }
    //        }
    //        // Begin TT#189 MD - JSmith - Filter Performance
    //        //catch (EndOfOperandsException)
    //        //{
    //        //    return currentReturn;
    //        //}
    //        // End TT#189 MD
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private ArrayList PreProcessDataOperandList(ArrayList aDataOperandList)
    //    {
    //        ArrayList outList;
    //        IEnumerator enumerator;
    //        QueryOperand operand;
    //        DataQueryVariableOperand firstVariableOperand;
    //        DataQueryVariableOperand pctVariableOperand = null;
    //        bool pctOf = false;
    //        bool pctChange = false;
    //        bool negateComparison = false;
    //        eFilterComparisonType comparisonType;
    //        QueryOperand compareToOperand;
    //        OperandProcessor operandProc;

    //        try
    //        {
    //            outList = new ArrayList();
    //            enumerator = aDataOperandList.GetEnumerator();
    //            operand = GetNextOperand(enumerator);
    //            // Begin TT#189 MD - JSmith - Filter Performance
    //            if (operand == null)
    //            {
    //                return outList;
    //            }
    //            // End TT#189 MD

    //            try
    //            {
    //                while (true)
    //                {
    //                    if (operand.GetType() == typeof(GenericQueryLeftParenOperand) ||
    //                        operand.GetType() == typeof(GenericQueryRightParenOperand) ||
    //                        operand.GetType() == typeof(GenericQueryAndOperand) ||
    //                        operand.GetType() == typeof(GenericQueryOrOperand))
    //                    {
    //                        outList.Add(operand);
    //                    }
    //                    else
    //                    {
    //                        firstVariableOperand = (DataQueryVariableOperand)operand;

    //                        operand = GetNextOperand(enumerator);
    //                        // Begin TT#189 MD - JSmith - Filter Performance
    //                        if (operand == null)
    //                        {
    //                            return outList;
    //                        }
    //                        // End TT#189 MD

    //                        if (operand.GetType() == typeof(DataQueryPctChangeOperand) || operand.GetType() == typeof(DataQueryPctOfOperand))
    //                        {
    //                            if (operand.GetType() == typeof(DataQueryPctOfOperand))
    //                            {
    //                                pctOf = true;
    //                            }
    //                            else
    //                            {
    //                                pctChange = true;
    //                            }

    //                            operand = GetNextOperand(enumerator);
    //                            // Begin TT#189 MD - JSmith - Filter Performance
    //                            if (operand == null)
    //                            {
    //                                return outList;
    //                            }
    //                            // End TT#189 MD

    //                            pctVariableOperand = (DataQueryVariableOperand)operand;

    //                            operand = GetNextOperand(enumerator);
    //                            // Begin TT#189 MD - JSmith - Filter Performance
    //                            if (operand == null)
    //                            {
    //                                return outList;
    //                            }
    //                            // End TT#189 MD
    //                        }

    //                        if (operand.GetType() == typeof(DataQueryNotOperand))
    //                        {
    //                            negateComparison = true;
    //                            operand = GetNextOperand(enumerator);
    //                            // Begin TT#189 MD - JSmith - Filter Performance
    //                            if (operand == null)
    //                            {
    //                                return outList;
    //                            }
    //                            // End TT#189 MD
    //                        }
    //                        else
    //                        {
    //                            negateComparison = false;
    //                        }

    //                        if (operand.GetType() == typeof(DataQueryEqualOperand))
    //                        {
    //                            comparisonType = eFilterComparisonType.Equal;
    //                        }
    //                        else if (operand.GetType() == typeof(DataQueryLessOperand))
    //                        {
    //                            comparisonType = eFilterComparisonType.Less;
    //                        }
    //                        else if (operand.GetType() == typeof(DataQueryGreaterOperand))
    //                        {
    //                            comparisonType = eFilterComparisonType.Greater;
    //                        }
    //                        else if (operand.GetType() == typeof(DataQueryLessEqualOperand))
    //                        {
    //                            comparisonType = eFilterComparisonType.LessEqual;
    //                        }
    //                        else
    //                        {
    //                            comparisonType = eFilterComparisonType.GreaterEqual;
    //                        }

    //                        compareToOperand = GetNextOperand(enumerator);
    //                        // Begin TT#189 MD - JSmith - Filter Performance
    //                        if (compareToOperand == null)
    //                        {
    //                            return outList;
    //                        }
    //                        // End TT#189 MD

    //                        if (pctOf)
    //                        {
    //                            operandProc = new PctOfOperandProcessor(_SAB, _transaction, firstVariableOperand, pctVariableOperand, negateComparison, comparisonType, compareToOperand);
    //                        }
    //                        else if (pctChange)
    //                        {
    //                            operandProc = new PctChangeOperandProcessor(_SAB, _transaction, firstVariableOperand, pctVariableOperand, negateComparison, comparisonType, compareToOperand);
    //                        }
    //                        else
    //                        {
    //                            operandProc = new ComparisonOperandProcessor(_SAB, _transaction, firstVariableOperand, negateComparison, comparisonType, compareToOperand);
    //                        }

    //                        outList.Add(operandProc);
    //                    }

    //                    operand = GetNextOperand(enumerator);
    //                    // Begin TT#189 MD - JSmith - Filter Performance
    //                    if (operand == null)
    //                    {
    //                        return outList;
    //                    }
    //                    // End TT#189 MD
    //                }
    //            }
    //            // Begin TT#189 MD - JSmith - Filter Performance
    //            //catch (EndOfOperandsException)
    //            //{
    //            //    return outList;
    //            //}
    //            // End TT#189 MD
    //            catch (Exception exc)
    //            {
    //                string message = exc.ToString();
    //                throw;
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool ProcessData(IEnumerator aEnumerator, ref QueryOperand aOperand, StoreProfile aStoreProf, DateProfile aDateProf)
    //    {
    //        bool currentReturn = false;
    //        OperandProcessor operandProc;

    //        try
    //        {
    //            aOperand = GetNextOperand(aEnumerator);
    //            // Begin TT#189 MD - JSmith - Filter Performance
    //            if (aOperand == null)
    //            {
    //                return currentReturn;
    //            }
    //            // End TT#189 MD

    //            if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
    //            {
    //                currentReturn = ProcessData(aEnumerator, ref aOperand, aStoreProf, aDateProf);
    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }
    //            else
    //            {
    //                operandProc = (OperandProcessor)aOperand;
    //                currentReturn = operandProc.AnalyzeOperand(aStoreProf, aDateProf);
    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }

    //            if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
    //            {
    //                return currentReturn;
    //            }
    //            else if (aOperand.GetType() == typeof(GenericQueryAndOperand))
    //            {
    //                return ProcessData(aEnumerator, ref aOperand, aStoreProf, aDateProf) && currentReturn;
    //            }
    //            else
    //            {
    //                return ProcessData(aEnumerator, ref aOperand, aStoreProf, aDateProf) || currentReturn;
    //            }
    //        }
    //        // Begin TT#189 MD - JSmith - Filter Performance
    //        //catch (EndOfOperandsException)
    //        //{
    //        //    return currentReturn;
    //        //}
    //        // End TT#189 MD
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private QueryOperand GetNextOperand(IEnumerator aEnumerator)
    //    {
    //        bool rc;

    //        try
    //        {
    //            while ((rc = aEnumerator.MoveNext()) &&
    //                (aEnumerator.Current.GetType() == typeof(AttrQuerySpacerOperand) ||
    //                aEnumerator.Current.GetType() == typeof(DataQuerySpacerOperand) ||
    //                !((QueryOperand)aEnumerator.Current).isMainOperand))
    //            {
    //            }

    //            if (rc)
    //            {
    //                return (QueryOperand)aEnumerator.Current;
    //            }
    //            else
    //            {
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                //throw new EndOfOperandsException();
    //                return null;
    //                // End TT#189 MD
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //abstract public class OperandProcessor : QueryOperand
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    private DateTime _nullDate = new DateTime(1,1,1);

    //    protected SessionAddressBlock _SAB;
    //    protected ApplicationSessionTransaction _transaction;
    //    protected DataQueryVariableOperand _firstVariableOperand;
    //    protected DataQueryVariableOperand _secondVariableOperand;
    //    protected bool _negateComparison;
    //    protected eFilterComparisonType _comparisonType;
    //    protected QueryOperand _compareToOperand;

    //    private bool _analyzed;
    //    private bool _result;
    //    private int _lastStoreKey;
    //    private int _lastDateKey;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public OperandProcessor(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        DataQueryVariableOperand aFirstVariableOperand,
    //        bool aNegateComparison,
    //        eFilterComparisonType aComparisonType,
    //        QueryOperand aCompareToOperand)

    //        : base(null)
    //    {
    //        _SAB = aSAB;
    //        _transaction = aTransaction;
    //        _firstVariableOperand = aFirstVariableOperand;
    //        _negateComparison = aNegateComparison;
    //        _comparisonType = aComparisonType;
    //        _compareToOperand = aCompareToOperand;
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    //========
    //    // METHODS
    //    //========

    //    abstract protected bool ProcessOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProfile);

    //    protected override System.Windows.Forms.Label CreateLabel()
    //    {
    //        return null;
    //    }

    //    public bool AnalyzeOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProfile)
    //    {
    //        try
    //        {
    //            if (!_analyzed ||
    //                ((_firstVariableOperand.TimeModifyer == eFilterTimeModifyer.Join && aCurrentDateProfile.Key != _lastDateKey) ||
    //                aStoreProf.Key != _lastStoreKey))
    //            {
    //                _result = ProcessOperand(aStoreProf, aCurrentDateProfile);
    //            }

    //            _lastDateKey = aCurrentDateProfile.Key;
    //            _lastStoreKey = aStoreProf.Key;
    //            _analyzed = true;

    //            return _result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    protected ProfileList GetDateRangeProfile(DataQueryVariableOperand aVariableOperand, StoreProfile aStoreProf, DateProfile aCurrentDateProf)
    //    {
    //        DateRangeProfile dateRangeProf;
    //        ProfileList detailDateProfList;

    //        try
    //        {
    //            if (aVariableOperand.TimeModifyer != eFilterTimeModifyer.Join)
    //            {
    //                if (aVariableOperand.DateRangeProfile.RelativeTo == eDateRangeRelativeTo.StoreOpen && aStoreProf.SellingOpenDt != _nullDate)
    //                {
    //                    dateRangeProf = _SAB.ApplicationServerSession.Calendar.GetDateRange(aVariableOperand.DateRangeProfile.Key, _SAB.ApplicationServerSession.Calendar.GetDay(aStoreProf.SellingOpenDt));
    //                }
    //                else
    //                {
    //                    dateRangeProf = aVariableOperand.DateRangeProfile;
    //                }

    //                if (dateRangeProf.SelectedDateType == eCalendarDateType.Week)
    //                {
    //                    detailDateProfList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf.InternalAnchorDate);
    //                }
    //                else
    //                {
    //                    detailDateProfList = _SAB.ApplicationServerSession.Calendar.GetPeriodRange(dateRangeProf, dateRangeProf.InternalAnchorDate);
    //                }
    //            }
    //            else
    //            {
    //                detailDateProfList = new ProfileList(aCurrentDateProf.ProfileType);
    //                detailDateProfList.Add(aCurrentDateProf);
    //            }

    //            return detailDateProfList;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    protected double[] GetValues(DataQueryVariableOperand aVariableOperand, StoreProfile aStoreProf, ProfileList aDetailDateProfList)
    //    {
    //        double[] valueList = null;
    //        eCubeType cubeType = eCubeType.None;
    //        PlanCellReference planCellRef;
    //        double totalValue;
    //        int count;
    //        VariableProfile varProf;

    //        try
    //        {
    //            switch (aVariableOperand.CubeModifyer)
    //            {
    //                case eFilterCubeModifyer.None:
    //                case eFilterCubeModifyer.StoreDetail:

    //                    if (aVariableOperand.GetType() == typeof(DataQueryPlanVariableOperand))
    //                    {
    //                        if (aVariableOperand.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
    //                        {
    //                            cubeType = eCubeType.StorePlanWeekDetail;
    //                        }
    //                        else
    //                        {
    //                            cubeType = eCubeType.StorePlanPeriodDetail;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        cubeType = eCubeType.StorePlanDateTotal;
    //                    }

    //                    break;

    //                case eFilterCubeModifyer.StoreAverage:
    //                case eFilterCubeModifyer.StoreTotal:

    //                    if (aVariableOperand.GetType() == typeof(DataQueryPlanVariableOperand))
    //                    {
    //                        if (aVariableOperand.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
    //                        {
    //                            cubeType = eCubeType.StorePlanStoreTotalWeekDetail;
    //                        }
    //                        else
    //                        {
    //                            cubeType = eCubeType.StorePlanStoreTotalPeriodDetail;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        cubeType = eCubeType.StorePlanStoreTotalDateTotal;
    //                    }

    //                    break;

    //                case eFilterCubeModifyer.ChainDetail:

    //                    if (aVariableOperand.GetType() == typeof(DataQueryPlanVariableOperand))
    //                    {
    //                        if (aVariableOperand.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
    //                        {
    //                            cubeType = eCubeType.ChainPlanWeekDetail;
    //                        }
    //                        else
    //                        {
    //                            cubeType = eCubeType.ChainPlanPeriodDetail;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        cubeType = eCubeType.ChainPlanDateTotal;
    //                    }

    //                    break;
    //            }

    //            planCellRef = (PlanCellReference)aVariableOperand.PlanCubeGroup.GetCube(cubeType).CreateCellReference();
    //            planCellRef[eProfileType.Version] = aVariableOperand.VersionProfile.Key;
    //            planCellRef[eProfileType.HierarchyNode] = aVariableOperand.NodeProfile.Key;

    //            if (aVariableOperand.CubeModifyer == eFilterCubeModifyer.StoreAverage)
    //            {
    //                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.StoreAverageQuantity.Key;
    //            }
    //            else
    //            {
    //                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
    //            }

    //            switch (cubeType.Id)
    //            {
    //                case eCubeType.cStorePlanWeekDetail:
    //                case eCubeType.cStorePlanPeriodDetail:
    //                case eCubeType.cStorePlanStoreTotalWeekDetail:
    //                case eCubeType.cStorePlanStoreTotalPeriodDetail:
    //                case eCubeType.cChainPlanWeekDetail:
    //                case eCubeType.cChainPlanPeriodDetail:

    //                    planCellRef[eProfileType.Variable] = ((DataQueryPlanVariableOperand)aVariableOperand).VariableProfile.Key;
    //                    break;

    //                case eCubeType.cStorePlanDateTotal:
    //                case eCubeType.cStorePlanStoreTotalDateTotal:
    //                case eCubeType.cChainPlanDateTotal:

    //                    varProf = ((DataQueryTimeTotalVariableOperand)aVariableOperand).VariableProfile;
    //                    planCellRef[eProfileType.Variable] = varProf.Key;
    //                    planCellRef[eProfileType.TimeTotalVariable] = varProf.GetTimeTotalVariable(((DataQueryTimeTotalVariableOperand)aVariableOperand).TimeTotalIndex).Key;
    //                    break;
    //            }

    //            switch (cubeType.Id)
    //            {
    //                case eCubeType.cStorePlanWeekDetail:
    //                case eCubeType.cStorePlanPeriodDetail:
    //                case eCubeType.cStorePlanDateTotal:

    //                    planCellRef[eProfileType.Store] = aStoreProf.Key;
    //                    break;
    //            }

    //            switch (cubeType.Id)
    //            {
    //                case eCubeType.cStorePlanWeekDetail:
    //                case eCubeType.cStorePlanPeriodDetail:
    //                case eCubeType.cStorePlanStoreTotalWeekDetail:
    //                case eCubeType.cStorePlanStoreTotalPeriodDetail:
    //                case eCubeType.cChainPlanWeekDetail:
    //                case eCubeType.cChainPlanPeriodDetail:

    //                switch (aVariableOperand.TimeModifyer)
    //                {
    //                    case eFilterTimeModifyer.Average:
    //                    case eFilterTimeModifyer.Total:

    //                        valueList = new double[1];
    //                        break;

    //                    default:

    //                        valueList = new double[aDetailDateProfList.Count];
    //                        break;
    //                }

    //                    totalValue = 0;
    //                    count = 0;

    //                    foreach (DateProfile dateProf in aDetailDateProfList)
    //                    {
    //                        switch (cubeType.Id)
    //                        {
    //                            case eCubeType.cStorePlanWeekDetail:
    //                            case eCubeType.cStorePlanStoreTotalWeekDetail:
    //                            case eCubeType.cChainPlanWeekDetail:

    //                                planCellRef[eProfileType.Week] = dateProf.Key;
    //                                break;

    //                            case eCubeType.cStorePlanPeriodDetail:
    //                            case eCubeType.cStorePlanStoreTotalPeriodDetail:
    //                            case eCubeType.cChainPlanPeriodDetail:

    //                                planCellRef[eProfileType.Period] = dateProf.Key;
    //                                break;
    //                        }

    //                        switch (aVariableOperand.TimeModifyer)
    //                        {
    //                            case eFilterTimeModifyer.Average:
    //                            case eFilterTimeModifyer.Total:

    //                                totalValue += planCellRef.CurrentCellValue;
    //                                break;

    //                            default:

    //                                valueList[count] = planCellRef.CurrentCellValue;
    //                                break;
    //                        }

    //                        count++;
    //                    }

    //                switch (aVariableOperand.TimeModifyer)
    //                {
    //                    case eFilterTimeModifyer.Average:

    //                        valueList[0] = totalValue / count;
    //                        break;

    //                    case eFilterTimeModifyer.Total:

    //                        valueList[0] = totalValue;
    //                        break;
    //                }

    //                    break;

    //                case eCubeType.cStorePlanDateTotal:
    //                case eCubeType.cStorePlanStoreTotalDateTotal:
    //                case eCubeType.cChainPlanDateTotal:

    //                    valueList = new double[1];
    //                    valueList[0] = planCellRef.CurrentCellValue;
    //                    break;
    //            }

    //            return valueList;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //abstract public class PctOperandProcessor : OperandProcessor
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    protected DataQueryVariableOperand _pctVariableOperand;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public PctOperandProcessor(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        DataQueryVariableOperand aFirstVariableOperand,
    //        DataQueryVariableOperand aPctVariableOperand,
    //        bool aNegateComparison,
    //        eFilterComparisonType aComparisonType,
    //        QueryOperand aCompareToOperand)

    //        : base(aSAB, aTransaction, aFirstVariableOperand, aNegateComparison, aComparisonType, aCompareToOperand)
    //    {
    //        _pctVariableOperand = aPctVariableOperand;
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    //========
    //    // METHODS
    //    //========

    //    protected bool ProcessPctOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProf, bool aPctOf, bool aPctChange)
    //    {
    //        ProfileList detailDateProfList;
    //        DataQueryVariableOperand secondVariableOperand;
    //        double[] firstValues = null;
    //        double[] pctValues = null;
    //        double[] secondValues = null;

    //        try
    //        {
    //            //Begin Track #5843 - JScott - Filters and Filter Wizard using % change receive no results when there are stores that fit the criteria.
    //            //detailDateProfList = GetDateRangeProfile(_firstVariableOperand, aStoreProf, aCurrentDateProf);

    //            //firstValues = GetValues(_firstVariableOperand, aStoreProf, detailDateProfList);
    //            //pctValues = GetValues(_pctVariableOperand, aStoreProf, detailDateProfList);
    //            detailDateProfList = GetDateRangeProfile(_firstVariableOperand, aStoreProf, aCurrentDateProf);
    //            firstValues = GetValues(_firstVariableOperand, aStoreProf, detailDateProfList);

    //            detailDateProfList = GetDateRangeProfile(_pctVariableOperand, aStoreProf, aCurrentDateProf);
    //            pctValues = GetValues(_pctVariableOperand, aStoreProf, detailDateProfList);
    //            //End Track #5843 - JScott - Filters and Filter Wizard using % change receive no results when there are stores that fit the criteria.

    //            secondVariableOperand = null;

    //            if (_compareToOperand.GetType() == typeof(DataQueryLiteralOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = ((DataQueryLiteralOperand)_compareToOperand).LiteralValue;
    //            }
    //            else if (_compareToOperand.GetType() == typeof(DataQueryGradeOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = (double)_firstVariableOperand.PlanCubeGroup.Transaction.GetStoreGradeList(_firstVariableOperand.NodeProfile.Key).GetStoreGradeKey(((DataQueryGradeOperand)_compareToOperand).GradeValue);
    //            }
    //            else if (_compareToOperand.GetType() == typeof(DataQueryStatusOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = (double)((DataQueryStatusOperand)_compareToOperand).StatusValue;
    //            }
    //            else
    //            {
    //                secondVariableOperand = (DataQueryVariableOperand)_compareToOperand;
    //                detailDateProfList = GetDateRangeProfile(secondVariableOperand, aStoreProf, aCurrentDateProf);
    //                secondValues = GetValues(secondVariableOperand, aStoreProf, detailDateProfList);
    //            }

    //            return EvaluateCondition(
    //                firstValues,
    //                pctValues,
    //                secondValues,
    //                (_firstVariableOperand == null || _firstVariableOperand.TimeModifyer == eFilterTimeModifyer.None) ? eFilterTimeModifyer.Any : _firstVariableOperand.TimeModifyer,
    //                (_pctVariableOperand == null || _pctVariableOperand.TimeModifyer == eFilterTimeModifyer.None) ? eFilterTimeModifyer.Any : _pctVariableOperand.TimeModifyer,
    //                (secondVariableOperand == null || secondVariableOperand.TimeModifyer == eFilterTimeModifyer.None) ? eFilterTimeModifyer.Any : secondVariableOperand.TimeModifyer,
    //                _comparisonType,
    //                _negateComparison,
    //                aPctOf,
    //                aPctChange);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    protected bool EvaluateCondition(
    //        double[] aFirstValues,
    //        double[] aPctValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aPctTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        bool aPctOf,
    //        bool aPctChange)
    //    {
    //        bool result;
    //        int i;

    //        try
    //        {
    //            result = false;

    //            for (i = 0; i < aFirstValues.Length; i++)
    //            {
    //                result = EvaluatePctValue(
    //                    aFirstValues,
    //                    aPctValues,
    //                    aSecondValues,
    //                    aFirstTimeMod,
    //                    aPctTimeMod,
    //                    aSecondTimeMod,
    //                    aComparisonType,
    //                    aNegateComparison,
    //                    aPctOf,
    //                    aPctChange,
    //                    i,
    //                    aFirstValues[i]);

    //                if (result != (aFirstTimeMod == eFilterTimeModifyer.All))
    //                {
    //                    break;
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool EvaluatePctValue(
    //        double[] aFirstValues,
    //        double[] aPctValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aPctTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        bool aPctOf,
    //        bool aPctChange,
    //        int aFirstIndex,
    //        double aFirstValue)
    //    {
    //        bool result;
    //        int i;

    //        try
    //        {
    //            result = (aFirstTimeMod == eFilterTimeModifyer.All);

    //            if (aPctTimeMod == eFilterTimeModifyer.Corresponding)
    //            {
    //                if (aFirstIndex < aPctValues.Length)
    //                {
    //                    result = EvaluateSecondValue(
    //                        aFirstValues,
    //                        aPctValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aPctTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aPctOf,
    //                        aPctChange,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aPctValues[aFirstIndex]);
    //                }
    //            }
    //            else
    //            {
    //                for (i = 0; i < aPctValues.Length; i++)
    //                {
    //                    result = EvaluateSecondValue(
    //                        aFirstValues,
    //                        aPctValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aPctTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aPctOf,
    //                        aPctChange,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aPctValues[i]);

    //                    if (result != (aPctTimeMod == eFilterTimeModifyer.All))
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool EvaluateSecondValue(
    //        double[] aFirstValues,
    //        double[] aPctValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aPctTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        bool aPctOf,
    //        bool aPctChange,
    //        int aFirstIndex,
    //        double aFirstValue,
    //        double aPctValue)
    //    {
    //        bool result;
    //        int i;

    //        try
    //        {
    //            result = (aPctTimeMod == eFilterTimeModifyer.All);

    //            if (aSecondTimeMod == eFilterTimeModifyer.Corresponding)
    //            {
    //                if (aFirstIndex < aSecondValues.Length)
    //                {
    //                    result = CompareValues(
    //                        aFirstValues,
    //                        aPctValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aPctTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aPctOf,
    //                        aPctChange,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aPctValue,
    //                        aSecondValues[aFirstIndex]);
    //                }
    //            }
    //            else
    //            {
    //                for (i = 0; i < aSecondValues.Length; i++)
    //                {
    //                    result = CompareValues(
    //                        aFirstValues,
    //                        aPctValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aPctTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aPctOf,
    //                        aPctChange,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aPctValue,
    //                        aSecondValues[i]);

    //                    if (result != (aSecondTimeMod == eFilterTimeModifyer.All))
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool CompareValues(
    //        double[] aFirstValues,
    //        double[] aPctValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aPctTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        bool aPctOf,
    //        bool aPctChange,
    //        int aFirstIndex,
    //        double aFirstValue,
    //        double aPctValue,
    //        double aSecondValue)
    //    {
    //        double compareAmt;

    //        try
    //        {
    //            if (aPctOf)
    //            {
    //                compareAmt = (aFirstValue / aPctValue) * 100;	
    //            }
    //            else
    //            {
    //                compareAmt = ((aFirstValue - aPctValue) / aPctValue) * 100;
    //            }

    //            switch (aComparisonType)
    //            {
    //                case eFilterComparisonType.Equal:
    //                    return (compareAmt == aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.Less:
    //                    return (compareAmt < aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.Greater:
    //                    return (compareAmt > aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.LessEqual:
    //                    return (compareAmt <= aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.GreaterEqual:
    //                    return (compareAmt >= aSecondValue) ^ aNegateComparison;
    //                default:
    //                    return false;
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //public class PctOfOperandProcessor : PctOperandProcessor
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public PctOfOperandProcessor(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        DataQueryVariableOperand aFirstVariableOperand,
    //        DataQueryVariableOperand aPctVariableOperand,
    //        bool aNegateComparison,
    //        eFilterComparisonType aComparisonType,
    //        QueryOperand aCompareToOperand)

    //        : base(aSAB, aTransaction, aFirstVariableOperand, aPctVariableOperand, aNegateComparison, aComparisonType, aCompareToOperand)
    //    {
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    //========
    //    // METHODS
    //    //========

    //    override protected bool ProcessOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProf)
    //    {
    //        try
    //        {
    //            return ProcessPctOperand(aStoreProf, aCurrentDateProf, true, false);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //public class PctChangeOperandProcessor : PctOperandProcessor
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public PctChangeOperandProcessor(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        DataQueryVariableOperand aFirstVariableOperand,
    //        DataQueryVariableOperand aPctVariableOperand,
    //        bool aNegateComparison,
    //        eFilterComparisonType aComparisonType,
    //        QueryOperand aCompareToOperand)

    //        : base(aSAB, aTransaction, aFirstVariableOperand, aPctVariableOperand, aNegateComparison, aComparisonType, aCompareToOperand)
    //    {
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    //========
    //    // METHODS
    //    //========

    //    override protected bool ProcessOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProf)
    //    {
    //        try
    //        {
    //            return ProcessPctOperand(aStoreProf, aCurrentDateProf, false, true);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //public class ComparisonOperandProcessor : OperandProcessor
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public ComparisonOperandProcessor(
    //        SessionAddressBlock aSAB,
    //        ApplicationSessionTransaction aTransaction,
    //        DataQueryVariableOperand aFirstVariableOperand,
    //        bool aNegateComparison,
    //        eFilterComparisonType aComparisonType,
    //        QueryOperand aCompareToOperand)

    //        : base(aSAB, aTransaction, aFirstVariableOperand, aNegateComparison, aComparisonType, aCompareToOperand)
    //    {
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    //========
    //    // METHODS
    //    //========

    //    override protected bool ProcessOperand(StoreProfile aStoreProf, DateProfile aCurrentDateProf)
    //    {
    //        ProfileList detailDateProfList;
    //        DataQueryVariableOperand secondVariableOperand;
    //        double[] firstValues = null;
    //        double[] secondValues = null;

    //        try
    //        {
    //            detailDateProfList = GetDateRangeProfile(_firstVariableOperand, aStoreProf, aCurrentDateProf);

    //            firstValues = GetValues(_firstVariableOperand, aStoreProf, detailDateProfList);

    //            secondVariableOperand = null;

    //            if (_compareToOperand.GetType() == typeof(DataQueryLiteralOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = ((DataQueryLiteralOperand)_compareToOperand).LiteralValue;
    //            }
    //            else if (_compareToOperand.GetType() == typeof(DataQueryGradeOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = (double)_firstVariableOperand.PlanCubeGroup.Transaction.GetStoreGradeList(_firstVariableOperand.NodeProfile.Key).GetStoreGradeKey(((DataQueryGradeOperand)_compareToOperand).GradeValue);
    //            }
    //            else if (_compareToOperand.GetType() == typeof(DataQueryStatusOperand))
    //            {
    //                secondValues = new double[1];
    //                secondValues[0] = (double)((DataQueryStatusOperand)_compareToOperand).StatusValue;
    //            }
    //            else
    //            {
    //                secondVariableOperand = (DataQueryVariableOperand)_compareToOperand;
    //                detailDateProfList = GetDateRangeProfile(secondVariableOperand, aStoreProf, aCurrentDateProf);
    //                secondValues = GetValues(secondVariableOperand, aStoreProf, detailDateProfList);
    //            }

    //            return EvaluateCondition(
    //                firstValues,
    //                secondValues,
    //                (_firstVariableOperand == null || _firstVariableOperand.TimeModifyer == eFilterTimeModifyer.None) ? eFilterTimeModifyer.Any : _firstVariableOperand.TimeModifyer,
    //                (secondVariableOperand == null || secondVariableOperand.TimeModifyer == eFilterTimeModifyer.None) ? eFilterTimeModifyer.Any : secondVariableOperand.TimeModifyer,
    //                _comparisonType,
    //                _negateComparison);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool EvaluateCondition(
    //        double[] aFirstValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison)
    //    {
    //        bool result;
    //        int i;

    //        try
    //        {
    //            result = false;

    //            for (i = 0; i < aFirstValues.Length; i++)
    //            {
    //                result = EvaluateSecondValue(
    //                    aFirstValues,
    //                    aSecondValues,
    //                    aFirstTimeMod,
    //                    aSecondTimeMod,
    //                    aComparisonType,
    //                    aNegateComparison,
    //                    i,
    //                    aFirstValues[i]);

    //                if (result != (aFirstTimeMod == eFilterTimeModifyer.All))
    //                {
    //                    break;
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool EvaluateSecondValue(
    //        double[] aFirstValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        int aFirstIndex,
    //        double aFirstValue)
    //    {
    //        bool result;
    //        int i;

    //        try
    //        {
    //            result = (aFirstTimeMod == eFilterTimeModifyer.All);

    //            if (aSecondTimeMod == eFilterTimeModifyer.Corresponding)
    //            {
    //                if (aFirstIndex < aSecondValues.Length)
    //                {
    //                    result = CompareValues(
    //                        aFirstValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aSecondValues[aFirstIndex]);
    //                }
    //            }
    //            else
    //            {
    //                for (i = 0; i < aSecondValues.Length; i++)
    //                {
    //                    result = CompareValues(
    //                        aFirstValues,
    //                        aSecondValues,
    //                        aFirstTimeMod,
    //                        aSecondTimeMod,
    //                        aComparisonType,
    //                        aNegateComparison,
    //                        aFirstIndex,
    //                        aFirstValue,
    //                        aSecondValues[i]);

    //                    if (result != (aSecondTimeMod == eFilterTimeModifyer.All))
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool CompareValues(
    //        double[] aFirstValues,
    //        double[] aSecondValues,
    //        eFilterTimeModifyer aFirstTimeMod,
    //        eFilterTimeModifyer aSecondTimeMod,
    //        eFilterComparisonType aComparisonType,
    //        bool aNegateComparison,
    //        int aFirstIndex,
    //        double aFirstValue,
    //        double aSecondValue)
    //    {
    //        try
    //        {
    //            switch (aComparisonType)
    //            {
    //                case eFilterComparisonType.Equal:
    //                    return (aFirstValue == aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.Less:
    //                    return (aFirstValue < aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.Greater:
    //                    return (aFirstValue > aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.LessEqual:
    //                    return (aFirstValue <= aSecondValue) ^ aNegateComparison;
    //                case eFilterComparisonType.GreaterEqual:
    //                    return (aFirstValue >= aSecondValue) ^ aNegateComparison;
    //                default:
    //                    return false;
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

	/// <summary>
	/// This class defines the CubeGroup entry in the CubeGroup Hashtable.
	/// </summary>

	public class CubeGroupHashEntry
	{
		//=======
		// FIELDS
		//=======

		private int _nodeKey;
		private int _versionKey;
		private int _dateKey;
		private FilterCubeGroup _filterCubeGroup;
		private FilterOpenParms _filterOpenParms;

		//=============
		// CONSTRUCTORS
		//=============

		public CubeGroupHashEntry(int aNodeKey, int aVersionKey, int aDateKey)
		{
			_nodeKey = aNodeKey;
			_versionKey = aVersionKey;
			_dateKey = aDateKey;
		}

		public CubeGroupHashEntry(int aNodeKey, int aVersionKey, int aDateKey, FilterCubeGroup aFilterCubeGroup, FilterOpenParms aFilterOpenParms)
		{
			_nodeKey = aNodeKey;
			_versionKey = aVersionKey;
			_dateKey = aDateKey;
			_filterCubeGroup = aFilterCubeGroup;
			_filterOpenParms = aFilterOpenParms;
		}

		//===========
		// PROPERTIES
		//===========

		public FilterCubeGroup FilterCubeGroup
		{
			get
			{
				return _filterCubeGroup;
			}
			set
			{
				_filterCubeGroup = value;
			}
		}

		public FilterOpenParms FilterOpenParms
		{
			get
			{
				return _filterOpenParms;
			}
			set
			{
				_filterOpenParms = value;
			}
		}

		//========
		// METHODS
		//========

		override public int GetHashCode()
		{
			return (int)(_nodeKey & 0x3FF << 20) | (_versionKey & 0x03FF << 10) | (_dateKey & 0x03FF);
		}

		override public bool Equals(object obj)
		{
			return (_nodeKey == ((CubeGroupHashEntry)obj)._nodeKey && _versionKey == ((CubeGroupHashEntry)obj)._versionKey && _dateKey == ((CubeGroupHashEntry)obj)._dateKey);
		}
	}
}
