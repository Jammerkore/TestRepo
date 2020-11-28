using System;
using System.Collections;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanOpenParms class defines the parameters necessary to open, read, and store the plans required for a
	/// any Plan Maintenance function.
	/// </summary>

	[Serializable]
	public class PlanOpenParms : PlanOpenParmsData
	{
		//=======
		// FIELDS
		//=======

		private StoreGroupProfile _storeGroupProfile;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanMaintCubeGroupOpenParms.
		/// </summary>

		public PlanOpenParms(ePlanSessionType aPlanSessionType, string aComputationMode) 
			: base(aPlanSessionType, aComputationMode)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		public void AddLowLevelPlanProfile(PlanProfile aPlanProf)
		{
			try
			{
				if (_lowLevelSortedList == null)
				{
					_lowLevelSortedList = new SortedList();
				}
	
				_lowLevelSortedList.Add(aPlanProf.NodeProfile.Text + _lowLevelSortedList.Count.ToString(), aPlanProf);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the StoreGroupProfile value.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock to use during the lookup.
		/// </param>
		/// <returns>
		/// The StoreGroupProfile.
		/// </returns>

		public StoreGroupProfile GetStoreGroupProfile(SessionAddressBlock aSAB)
		{
			try
			{
				if (_storeGroupProfile == null || _storeGroupProfile.Key != StoreGroupRID)
				{
                    _storeGroupProfile = StoreMgmt.StoreGroup_Get(StoreGroupRID); //aSAB.StoreServerSession.GetStoreGroup(StoreGroupRID);
				}

				return _storeGroupProfile;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Basis ProfileList for the given HiearchyNode and Version key.
		/// </summary>
		/// <param name="aPlanCubeGroup">
		/// The current PlanCubeGroup to lookup HiearchyNode and Version information.
		/// </param>
		/// <param name="aHierarchyNodeKey">
		/// The HiearchyNode key of the plan.
		/// </param>
		/// <param name="aVersionKey">
		/// The Version key of the plan.
		/// </param>
		/// <returns>
		/// The Basis ProfileList for the given HiearchyNode and Version key.
		/// </returns>

		public ProfileList GetBasisProfileList(PlanCubeGroup aPlanCubeGroup, int aHierarchyNodeKey, int aVersionKey)
		{
			HashKeyObject hashKey;
			ProfileList basisProfList;
			BasisProfile newBasisProf;
			ProfileList nodeProfList;
			HierarchyNodeProfile nodeProf;

			try
			{
				if (PlanSessionType == ePlanSessionType.ChainSingleLevel || PlanSessionType == ePlanSessionType.StoreSingleLevel)
				{
					return BasisProfileList;
				}
				else
				{
					hashKey = new HashKeyObject(aHierarchyNodeKey, aVersionKey);

					if (BasisProfileListHash == null)
					{
						BasisProfileListHash = new Hashtable();
					}

					basisProfList = (ProfileList)BasisProfileListHash[hashKey];

					if (basisProfList == null)
					{
						basisProfList = new ProfileList(eProfileType.Basis);

						foreach (BasisProfile basisProf in BasisProfileList)
						{
							newBasisProf = basisProf.Copy(aPlanCubeGroup.SAB.ApplicationServerSession, false);

							foreach (BasisDetailProfile basisDetProf in newBasisProf.BasisDetailProfileList)
							{
								nodeProfList = aPlanCubeGroup.GetMasterProfileList(eProfileType.HierarchyNode);
								nodeProf = (HierarchyNodeProfile)nodeProfList.FindKey(aHierarchyNodeKey);

								if (nodeProf == null)
								{
									nodeProf = aPlanCubeGroup.SAB.HierarchyServerSession.GetNodeData(aHierarchyNodeKey);

									switch (aPlanCubeGroup.OpenParms.PlanSessionType)
									{
										case ePlanSessionType.ChainMultiLevel:
										case ePlanSessionType.ChainSingleLevel:

											nodeProf.ChainSecurityProfile = aPlanCubeGroup.SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aHierarchyNodeKey, (int)eSecurityTypes.Chain);
											break;

										case ePlanSessionType.StoreMultiLevel:
										case ePlanSessionType.StoreSingleLevel:

											nodeProf.ChainSecurityProfile = aPlanCubeGroup.SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aHierarchyNodeKey, (int)eSecurityTypes.Chain);
											nodeProf.StoreSecurityProfile = aPlanCubeGroup.SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aHierarchyNodeKey, (int)eSecurityTypes.Store);
											break;
									}
								}

								basisDetProf.HierarchyNodeProfile = nodeProf;
							}

							basisProfList.Add(newBasisProf);
						}

						BasisProfileListHash.Add(hashKey, basisProfList);
					}

					return basisProfList;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the maximum Basis Detail count for all Basis.
		/// </summary>
		/// <returns>
		/// The maximum Basis Detail count for all Basis.
		/// </returns>

		public int GetMaximumBasisDetailCount()
		{
			int maxCount;

			try
			{
				maxCount = 0;

				foreach (BasisProfile basisProf in BasisProfileList)
				{
					maxCount = Math.Max(basisProf.BasisDetailProfileList.Count, maxCount);
				}

				return maxCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the Chain low-level PlanProfile ProfileList from the given hierarchy level offset and default VersionProfile.
		/// </summary>
		/// <param name="aOffset">
		/// The hierarchy level offset to get the list of nodes from.
		/// </param>
		/// <param name="aVersionProfile">
		/// The default VersionProfile that will be assigned to each PlanProfile.
		/// </param>

		public void BuildLowLevelPlanProfileListFromOffset(HierarchyServerSession aHierarchySession, int aOffset, VersionProfile aVersionProfile)
		{
			HierarchyNodeList nodeList;
			PlanProfile planProf;

			try
			{
				if (ChainHLPlanProfile != null && ChainHLPlanProfile.NodeProfile != null && ChainHLPlanProfile.VersionProfile != null && aOffset > 0)
				{
					nodeList = aHierarchySession.GetDescendantData(ChainHLPlanProfile.NodeProfile.Key, aOffset, true, eNodeSelectType.All);

					foreach (HierarchyNodeProfile nodeProf in nodeList)
					{
						planProf = new PlanProfile();
						planProf.NodeProfile = nodeProf;
						planProf.VersionProfile = aVersionProfile;

						AddLowLevelPlanProfile(planProf);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the LowLevelPlanProfile value.
		/// </summary>
		/// <param name="aNodeKey">
		/// The key of the node to get the profile for.
		/// </param>
		/// <returns>
		/// The PlanProfile for the given key.
		/// </returns>

		public PlanProfile GetLowLevelPlanProfile(int aNodeKey)
		{
			try
			{
				if (LowLevelPlanProfileList != null)
				{
					return (PlanProfile)LowLevelPlanProfileList.FindKey(aNodeKey);
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the Average Divisor for this plan.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The Average Divisor for this plan.
		/// </returns>

		public double GetAverageDivisor(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return AverageDivisor;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the Average Divisor for this plan.
		/// </summary>
		/// <param name="aSession">
		/// The Session that this object exists under.
		/// </param>
		/// <returns>
		/// The Average Divisor for this plan.
		/// </returns>

		public bool GetContainsCurrentWeek(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return ContainsCurrentWeek;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ePlanDisplayType.
		/// </summary>

		public ePlanDisplayType GetDisplayType(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return DisplayType;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the summary DateRangeProfile.
		/// </summary>

		public DateRangeProfile GetSummaryDateProfile(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return SummaryDateProfile;
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

		public ProfileList GetDetailDateProfileList(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return DetailDateProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ProfileList containing the list of dates Profiles (week totals).
		/// </summary>

		public ProfileList GetDateProfileList(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return DateProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ProfileList of weeks.
		/// </summary>

		public ProfileList GetWeekProfileList(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return WeekProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ProfileList of periods.
		/// </summary>

		public ProfileList GetPeriodProfileList(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return PeriodProfileList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the ProfileXRef containing the date-to-week cross-reference.
		/// </summary>

		public ProfileXRef GetDateToWeekXRef(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return DateToWeekXRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Gets the ProfileXRef containing the date-to-profile cross-reference.
		/// </summary>

		public ProfileXRef GetDateToPeriodXRef(Session aSession)
		{
			try
			{
				if (!WeeksCalculated)
				{
					GetPlanWeeks(aSession);
				}

				return DateToPeriodXRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// This method takes the plan's DateRangeProfile and converts it to weeks.  It also builds the summary Profile, detail ProfileList, 
		/// and date ProfileList.
		/// </summary>

		private void GetPlanWeeks(Session aSession)
		{
			//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
			//ProfileList weekProfileList;
			//End Track #5121 - JScott - Add Year/Season/Quarter totals
			WeekProfile currProf;
			double weeks;
			int days;
			double periods;

			try
			{
				DateProfileList = new ProfileList(eProfileType.DateRange);
				PeriodProfileList = new ProfileList(eProfileType.Period);
				DateToWeekXRef = new ProfileXRef(eProfileType.DateRange, eProfileType.Week);
				DateToPeriodXRef = new ProfileXRef(eProfileType.DateRange, eProfileType.Period);

				if (DateRangeProfile.SelectedDateType == eCalendarDateType.Week || OpenPeriodAsWeeks)
				{
					DisplayType = ePlanDisplayType.Week;
					WeekProfileList = aSession.Calendar.GetWeekRange(DateRangeProfile, DateRangeProfile.InternalAnchorDate, DateToWeekXRef);
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					PeriodProfileList = aSession.Calendar.GetPeriodRange(DateRangeProfile, DateRangeProfile);
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
					DetailDateProfileList = WeekProfileList;

                    //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    if (IsLadder)  //allow one week to be selected
                    {
                        SummaryDateProfile = DateRangeProfile;
                        DateProfileList.Add(DateRangeProfile);
                    }
                    else
                    {
                        if (DetailDateProfileList.Count > 1)
                        {
                            SummaryDateProfile = DateRangeProfile;
                            DateProfileList.Add(DateRangeProfile);
                        }
                    }
                    //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

					if ((PlanSessionType == ePlanSessionType.StoreSingleLevel || PlanSessionType == ePlanSessionType.StoreMultiLevel) &&
						(StoreHLPlanProfile.VersionProfile.Key == Include.FV_ActualRID || StoreHLPlanProfile.VersionProfile.Key == Include.FV_ModifiedRID))
					{
						currProf = aSession.Calendar.CurrentWeek;
						weeks = 0;
						ContainsCurrentWeek = false;

						foreach (WeekProfile weekProf in DetailDateProfileList)
						{
							if (weekProf == currProf)
							{
								weeks += (double)(aSession.Calendar.CurrentDate.DayInWeek - 1) / 7;
								ContainsCurrentWeek = true;
								break;
							}
							else
							{
								weeks++;
							}
						}

						if (ContainsCurrentWeek)
						{
							AverageDivisor = weeks;
						}
						else
						{
							AverageDivisor = 0;
						}
					}
					else
					{
						AverageDivisor = 0;
					}
				}
				else if (DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
				{
					DisplayType = ePlanDisplayType.Period;
					WeekProfileList = aSession.Calendar.GetWeekRange(DateRangeProfile, DateRangeProfile.InternalAnchorDate);
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
					//DetailDateProfileList = aSession.Calendar.GetPeriodRange(DateRangeProfile, DateRangeProfile, DateToPeriodXRef);
					//DetailDateProfileList = aSession.Calendar.GetPeriodRange(DateRangeProfile, DateRangeProfile.InternalAnchorDate, DateToPeriodXRef);
					PeriodProfileList = aSession.Calendar.GetPeriodRange(DateRangeProfile, DateRangeProfile.InternalAnchorDate, DateToPeriodXRef);
					DetailDateProfileList = PeriodProfileList;
					//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
					//End Track #5121 - JScott - Add Year/Season/Quarter totals

                   
           
                    //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    if (IsLadder)  //allow one period to be selected
                    {
                        SummaryDateProfile = DateRangeProfile;
                        DateProfileList.Add(DateRangeProfile);
                    }
                    else
                    {
                        if (DetailDateProfileList.Count > 1)
                        {
                            SummaryDateProfile = DateRangeProfile;
                            DateProfileList.Add(DateRangeProfile);
                        }
                    }
                    //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

          

					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//foreach (PeriodProfile periodProfile in DetailDateProfileList)
					//{
					//    PeriodProfileList.Add(periodProfile);
					//    weekProfileList = aSession.Calendar.GetWeekRange(periodProfile);
					//}

					//End Track #5121 - JScott - Add Year/Season/Quarter totals
					if ((PlanSessionType == ePlanSessionType.StoreSingleLevel || PlanSessionType == ePlanSessionType.StoreMultiLevel) &&
						//Begin Track #5823 - JScott - All merchants receive an error message and can not access their plans
						//StoreHLPlanProfile.VersionProfile.Key == Include.FV_ActualRID || StoreHLPlanProfile.VersionProfile.Key == Include.FV_ModifiedRID)
						(StoreHLPlanProfile.VersionProfile.Key == Include.FV_ActualRID || StoreHLPlanProfile.VersionProfile.Key == Include.FV_ModifiedRID))
						//End Track #5823 - JScott - All merchants receive an error message and can not access their plans
					{
						currProf = aSession.Calendar.CurrentWeek;
						periods = 0;
						ContainsCurrentWeek = false;

						foreach (PeriodProfile perProf in DetailDateProfileList)
						{
							days = 0;

							foreach (WeekProfile weekProf in perProf.Weeks)
							{
								if (weekProf == currProf)
								{
									days += (aSession.Calendar.CurrentDate.DayInWeek - 1);
									ContainsCurrentWeek = true;
									break;
								}
								else
								{
									days += 7;
								}
							}

							if (ContainsCurrentWeek)
							{
								periods += (double)days / ((double)perProf.NoOfWeeks * 7);
								break;
							}
							else
							{
								periods++;
							}
						}

						if (ContainsCurrentWeek)
						{
							AverageDivisor = periods;
						}
						else
						{
							AverageDivisor = 0;
						}
					}
					else
					{
						AverageDivisor = 0;
					}
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidDateType,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidDateType));
				}

				WeeksCalculated = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool IsPlanBasisTimeLengthEqual(Session aSession, int aBasisIndex)
		{
			try
			{
				BasisProfile basisProfile = (BasisProfile)BasisProfileList[aBasisIndex];
				if (GetDetailDateProfileList(aSession).Count == basisProfile.DisplayablePlanDetailProfileList(aSession).Count)
				{
					return true;
				}
				else
				{
					return false;
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
	/// The FilterOpenParms class defines the parameters necessary to open, read, and store the plans required for a filter function.
	/// </summary>

	[Serializable]
	public class FilterOpenParms : PlanOpenParms
	{
		//=======
		// FIELDS
		//=======

		//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
		//private ArrayList _storeCubeList;
		//private ArrayList _chainCubeList;
		private Hashtable _storeCubeList;
		private Hashtable _chainCubeList;
		bool _hasStoreCubes;
		bool _hasChainCubes;
		//End Track #6251 - JScott - Get System Null Ref Excp using filter

		//=============
		// CONSTRUCTORS
		//=============

		public FilterOpenParms(ePlanSessionType aPlanSessionType, string aComputationMode)
			: base(aPlanSessionType, aComputationMode)
		{
			//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
			//_storeCubeList = new ArrayList();
			//_chainCubeList = new ArrayList();
			_storeCubeList = new Hashtable();
			_chainCubeList = new Hashtable();
			_hasStoreCubes = false;
			_hasChainCubes = false;
			//End Track #6251 - JScott - Get System Null Ref Excp using filter
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
		//public ArrayList StoreCubeList
		public eCubeType[] StoreCubeList
		//End Track #6251 - JScott - Get System Null Ref Excp using filter
		{
			get
			{
				//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
				//return _storeCubeList;
				eCubeType[] cubeTypeArray;

				try
				{
					cubeTypeArray = new eCubeType[_storeCubeList.Count];
					_storeCubeList.Keys.CopyTo(cubeTypeArray, 0);
					return cubeTypeArray;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				//End Track #6251 - JScott - Get System Null Ref Excp using filter
			}
		}

		//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
		//public ArrayList ChainCubeList
		public eCubeType[] ChainCubeList
		//End Track #6251 - JScott - Get System Null Ref Excp using filter
		{
			get
			{
				//Begin Track #6251 - JScott - Get System Null Ref Excp using filter
				//return _chainCubeList;
				eCubeType[] cubeTypeArray;

				try
				{
					cubeTypeArray = new eCubeType[_chainCubeList.Count];
					_chainCubeList.Keys.CopyTo(cubeTypeArray, 0);
					return cubeTypeArray;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				//End Track #6251 - JScott - Get System Null Ref Excp using filter
			}
		}

		//========
		// METHODS
		//========
		//Begin Track #6251 - JScott - Get System Null Ref Excp using filter

		public void AddStoreCube(eCubeType aCubeType)
		{
			try
			{
				_storeCubeList[aCubeType] = null;
				_hasStoreCubes = true;
				SetGroupType();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddChainCube(eCubeType aCubeType)
		{
			try
			{
				_chainCubeList[aCubeType] = null;
				_hasChainCubes = true;
				SetGroupType();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetGroupType()
		{
			try
			{
				if (_hasStoreCubes)
				{
					PlanSessionType = ePlanSessionType.StoreSingleLevel;
				}
				else
				{
					PlanSessionType = ePlanSessionType.ChainSingleLevel;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6251 - JScott - Get System Null Ref Excp using filter
	}
}
