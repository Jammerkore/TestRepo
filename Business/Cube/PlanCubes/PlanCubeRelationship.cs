using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The TimeTotalCubeRelationshipItem class identifies a total-common-detail relationship between three profile types.
	/// </summary>

	public class TimeTotalPlanCubeRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _detailProfileType;
		private eProfileListType _detailProfileListType;

		//=============
		// CONSTRUCTORS
		//=============

		public TimeTotalPlanCubeRelationshipItem(
			eProfileType aDetailProfileType,
			eProfileListType aDetailProfileListType)
		{
			_detailProfileType = aDetailProfileType;
			_detailProfileListType = aDetailProfileListType;

			if (_detailProfileType == eProfileType.None)
			{
				throw new Exception("Cannot specify eProfileList.None in a TimeTotalPlanCubeRelationshipItem object.");
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eProfileType of the detail profile.
		/// </summary>

		public eProfileType DetailProfileType
		{
			get
			{
				return _detailProfileType;
			}
		}

		/// <summary>
		/// Gets the eProfileListType of the total detail profile.
		/// </summary>

		public eProfileListType DetailProfileListType
		{
			get
			{
				return _detailProfileListType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComputationVariableProfile timeTotVarProf;
			ProfileList timeTotProfileList;
			ProfileXRef commonXRef;
			BaseProfileXRef detailXRef;
			ArrayList commonList = null;
			ArrayList totalList = null;

			try
			{
				aCancel = false;

				timeTotProfileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(eProfileType.TimeTotalVariable);

				commonXRef = (ProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Variable));

				if (((PlanCellReference)aSourceCellRef).PlanCube.isPlanCube)
				{
					detailXRef = aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.TimeTotalVariable, _detailProfileType));
				}
				else
				{
					detailXRef = aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComplexProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Basis, _detailProfileType));
				}

				if (commonXRef != null && detailXRef != null)
				{
					commonList = (ArrayList)commonXRef.GetTotalList(aSourceCellRef[eProfileType.Variable]);

					if (commonList != null)
					{
						foreach (int profileKey in commonList)
						{
							timeTotVarProf = (ComputationVariableProfile)timeTotProfileList.FindKey(profileKey);

							if (((PlanCellReference)aSourceCellRef).PlanCube.isPlanCube)
							{
								totalList = (ArrayList)((ProfileXRef)detailXRef).GetTotalList(aSourceCellRef[_detailProfileType]);
							}
							else
							{
								totalList = (ArrayList)((ComplexProfileXRef)detailXRef).GetTotalList(aSourceCellRef[eProfileType.Basis], aSourceCellRef[_detailProfileType]);
							}

							if (totalList != null && totalList.Count > 0)
							{
								aCellRef[eProfileType.TimeTotalVariable] = profileKey;

								if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
								{
									aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
								}
								else
								{
									aCellSelector.CheckItem(aCellRef, out aCancel);
								}

								if (aCancel)
								{
									return;
								}
							}
						}
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
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComputationVariableProfile sourceVarProf;
			ProfileList detailProfileList;
			BaseProfileXRef totalXRef;
			ArrayList detailList = null;

			try
			{
				aCancel = false;
				sourceVarProf = ((PlanCellReference)aSourceCellRef).GetCalcVariableProfile();

				switch (_detailProfileListType)
				{
					case eProfileListType.Master:
						detailProfileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(_detailProfileType);
						break;
					case eProfileListType.Filtered:
						detailProfileList = aSourceCellRef.Cube.CubeGroup.GetFilteredProfileList(_detailProfileType);
						break;
					default:
						detailProfileList = null;
						break;
				}

				if (((PlanCellReference)aSourceCellRef).PlanCube.isPlanCube)
				{
					totalXRef = aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.TimeTotalVariable, _detailProfileType));

					if (totalXRef != null)
					{
						detailList = (ArrayList)((ProfileXRef)totalXRef).GetDetailList(aSourceCellRef[eProfileType.TimeTotalVariable]);
					}
				}
				else
				{
					totalXRef = aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComplexProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Basis, _detailProfileType));

					if (totalXRef != null)
					{
						detailList = (ArrayList)((ComplexProfileXRef)totalXRef).GetDetailList(aSourceCellRef[eProfileType.TimeTotalVariable], aSourceCellRef[eProfileType.Basis]);
					}
				}

				if (detailList != null)
				{
					foreach (int profileKey in detailList)
					{
						if (detailProfileList == null || detailProfileList.Contains(profileKey))
						{
							aCellRef[_detailProfileType] = profileKey;

							if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
							{
								aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
							}
							else
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
					}
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
	/// The CubeRelationshipItem class identifies a total-detail relationship between two profile types.
	/// </summary>

	public class PeriodWeekPlanCubeRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _totalProfileType;
		private eProfileType _detailProfileType;
		private eProfileListType _profileListType;

		//=============
		// CONSTRUCTORS
		//=============

		public PeriodWeekPlanCubeRelationshipItem(eProfileType aTotalProfileType, eProfileType aDetailProfileType, eProfileListType aProfileListType)
		{
			_totalProfileType = aTotalProfileType;
			_detailProfileType = aDetailProfileType;
			_profileListType = aProfileListType;

			if (_profileListType == eProfileListType.None && (_totalProfileType == eProfileType.None || _detailProfileType == eProfileType.None))
			{
				throw new Exception("Cannot specify eProfileListType.None when either Total or Detail eProfileType = eProfileType.None");
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eProfileType of the total profile.
		/// </summary>

		public eProfileType TotalProfileType
		{
			get
			{
				return _totalProfileType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the detail profile.
		/// </summary>

		public eProfileType DetailProfileType
		{
			get
			{
				return _detailProfileType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the detail profile.
		/// </summary>

		public eProfileListType ProfileListType
		{
			get
			{
				return _profileListType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComputationVariableProfile sourceVarProf;
			ProfileXRef compXRef;
			ArrayList totalList = null;
			ProfileList profileList;

			try
			{
				aCancel = false;
				sourceVarProf = ((PlanCellReference)aSourceCellRef).GetCalcVariableProfile();

				switch (_profileListType)
				{
					case eProfileListType.Master:
						profileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(_totalProfileType);
						break;
					case eProfileListType.Filtered:
						profileList = aSourceCellRef.Cube.CubeGroup.GetFilteredProfileList(_totalProfileType);
						break;
					default:
						profileList = null;
						break;
				}

				if (_totalProfileType != eProfileType.None &&
					_detailProfileType != eProfileType.None)
				{
					compXRef = (ProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(_totalProfileType, _detailProfileType));

					if (compXRef != null)
					{
						switch (sourceVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All:
								totalList = (ArrayList)compXRef.GetTotalList(aSourceCellRef[_detailProfileType]);
								break;

							case eVariableTimeTotalType.AllPlusNext:
								totalList = (ArrayList)compXRef.GetTotalPlusNextList(aSourceCellRef[_detailProfileType]);
								break;

							case eVariableTimeTotalType.First:
								totalList = (ArrayList)compXRef.GetTotalFirst(aSourceCellRef[_detailProfileType]);
								break;

							case eVariableTimeTotalType.Last:
								totalList = (ArrayList)compXRef.GetTotalLast(aSourceCellRef[_detailProfileType]);
								break;

							case eVariableTimeTotalType.FirstAndLast:
								totalList = (ArrayList)compXRef.GetTotalFirstLastList(aSourceCellRef[_detailProfileType]);
								break;

							case eVariableTimeTotalType.Next:
								totalList = (ArrayList)compXRef.GetDetailNext(aSourceCellRef[_detailProfileType]);
								break;
						}

						if (totalList != null)
						{
							foreach (int profileKey in totalList)
							{
								if (profileList == null || profileList.Contains(profileKey))
								{
									intRecurseNextTotal(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, profileKey, out aCancel);

									if (aCancel)
									{
										return;
									}
								}
							}
						}
					}
				}
				else if (profileList != null && _totalProfileType != eProfileType.None)
				{
					foreach (Profile profile in profileList)
					{
						intRecurseNextTotal(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, profile.Key, out aCancel);

						if (aCancel)
						{
							return;
						}
					}
				}
				else
				{
					intRecurseNextTotal(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, -1, out aCancel);

					if (aCancel)
					{
						return;
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
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComputationVariableProfile sourceVarProf;
			ProfileXRef compXRef;
			ArrayList detailList = null;
			ProfileList profileList;

			try
			{
				aCancel = false;
				sourceVarProf = ((PlanCellReference)aSourceCellRef).GetCalcVariableProfile();

				switch (_profileListType)
				{
					case eProfileListType.Master:
						profileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(_detailProfileType);
						break;
					case eProfileListType.Filtered:
						profileList = aSourceCellRef.Cube.CubeGroup.GetFilteredProfileList(_detailProfileType);
						break;
					default:
						profileList = null;
						break;
				}

				if (_totalProfileType != eProfileType.None &&
					_detailProfileType != eProfileType.None)
				{
					compXRef = (ProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(_totalProfileType, _detailProfileType));

					if (compXRef != null)
					{
						switch (sourceVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All:
								detailList = (ArrayList)compXRef.GetDetailList(aSourceCellRef[_totalProfileType]);
								break;

							case eVariableTimeTotalType.AllPlusNext:
								detailList = (ArrayList)compXRef.GetDetailPlusNextList(aSourceCellRef[_totalProfileType]);
								break;

							case eVariableTimeTotalType.First:
								detailList = (ArrayList)compXRef.GetDetailFirst(aSourceCellRef[_totalProfileType]);
								break;

							case eVariableTimeTotalType.Last:
								detailList = (ArrayList)compXRef.GetDetailLast(aSourceCellRef[_totalProfileType]);
								break;

							case eVariableTimeTotalType.FirstAndLast:
								detailList = (ArrayList)compXRef.GetDetailFirstLastList(aSourceCellRef[_totalProfileType]);
								break;

							case eVariableTimeTotalType.Next:
								detailList = (ArrayList)compXRef.GetDetailNext(aSourceCellRef[_totalProfileType]);
								break;
						}

						if (detailList != null)
						{
							foreach (int profKey in detailList)
							{
								if (profileList == null || profileList.Contains(profKey))
								{
									intRecurseNextDetail(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, profKey, out aCancel);

									if (aCancel)
									{
										return;
									}
								}
							}
						}
					}
				}
				else if (profileList != null && _detailProfileType != eProfileType.None)
				{
					foreach (Profile profile in profileList)
					{
						intRecurseNextDetail(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, profile.Key, out aCancel);

						if (aCancel)
						{
							return;
						}
					}
				}
				else
				{
					intRecurseNextDetail(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem, aCellSelector, -1, out aCancel);

					if (aCancel)
					{
						return;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void intRecurseNextTotal(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			int aProfileKey,
			out bool aCancel)
		{
			try
			{
				if (aProfileKey != -1)
				{
					aCellRef[_totalProfileType] = aProfileKey;
				}

				if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
				{
					aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
				}
				else
				{
					aCellSelector.CheckItem(aCellRef, out aCancel);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void intRecurseNextDetail(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			int aProfileKey,
			out bool aCancel)
		{
			try
			{
				if (aProfileKey != -1)
				{
					aCellRef[_detailProfileType] = aProfileKey;
				}

				if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
				{
					aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
				}
				else
				{
					aCellSelector.CheckItem(aCellRef, out aCancel);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
	/// <summary>
	/// The BasisDetailCubeRelationshipItem class identifies a cube and the low-level total relationship.
	/// </summary>

	public class BasisDetailPlanCubeRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public BasisDetailPlanCubeRelationshipItem()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			try
			{
				aCellRef[eProfileType.Week] = ((BasisDetailProfile)((BasisProfile)((PlanCubeGroup)aSourceCellRef.Cube.CubeGroup).OpenParms.BasisProfileList.FindKey(aSourceCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aSourceCellRef[eProfileType.BasisDetail])).GetPlanWeekIdFromBasisWeekId(aSourceCellRef.Cube.CubeGroup.SAB.ApplicationServerSession, aSourceCellRef[eProfileType.Week]);

				aCellSelector.CheckItem(aCellRef, out aCancel);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			TriProfileXRef compXRef;
			int basisRID;
			ArrayList detailList;

			try
			{
				aCancel = false;

				compXRef = (TriProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(
					new TriProfileXRef(
						eProfileType.HierarchyNode, eProfileType.Basis,
						eProfileType.BasisDetail, eProfileType.BasisHierarchyNode, eProfileType.BasisVersion));
					
				if (compXRef != null)
				{
					basisRID = aSourceCellRef[eProfileType.Basis];
					detailList = (ArrayList)compXRef.GetDetailList(aSourceCellRef [eProfileType.HierarchyNode], basisRID);

					if (detailList != null)
					{
						foreach (TriProfileXRefDetailEntry profileKey in detailList)
						{
		
							aCellRef[eProfileType.BasisDetail] = profileKey.PrimaryId;
							aCellRef[eProfileType.BasisHierarchyNode] = profileKey.SecondaryId;
							aCellRef[eProfileType.BasisVersion] = profileKey.TertiaryId;
							aCellRef[eProfileType.Week] = ((BasisDetailProfile)((BasisProfile)((PlanCubeGroup)aSourceCellRef.Cube.CubeGroup).OpenParms.BasisProfileList.FindKey(basisRID)).BasisDetailProfileList.FindKey(profileKey.PrimaryId)).GetBasisWeekIdFromPlanWeekId(aSourceCellRef.Cube.CubeGroup.SAB.ApplicationServerSession, aSourceCellRef[eProfileType.Week]);

							aCellSelector.CheckItem(aCellRef, out aCancel);
									
							if (aCancel)
							{
								return;
							}
						}
					}
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
	/// The LowLevelTotalCubeRelationshipItem class identifies a cube and the low-level total relationship.
	/// </summary>

	public class LowLevelTotalPlanCubeRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _cubeProfileType;

		//=============
		// CONSTRUCTORS
		//=============

		public LowLevelTotalPlanCubeRelationshipItem(eProfileType aCubeProfileType)
		{
			_cubeProfileType = aCubeProfileType;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComplexProfileXRef compXRef;
			ArrayList totalList;

			try
			{
				aCancel = false;

				compXRef = (ComplexProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComplexProfileXRef(_cubeProfileType, eProfileType.HierarchyNode, eProfileType.Version));

				if (compXRef != null)
				{
					totalList = (ArrayList)compXRef.GetTotalList(aSourceCellRef[eProfileType.HierarchyNode], aSourceCellRef[eProfileType.Version]);

					if (totalList != null)
					{
						foreach (int profileKey in totalList)
						{
							aCellRef[eProfileType.LowLevelTotalVersion] = profileKey;

							if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
							{
								aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
							}
							else
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
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
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			ComplexProfileXRef compXRef;
			ArrayList detailList;

			try
			{
				aCancel = false;

				compXRef = (ComplexProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComplexProfileXRef(_cubeProfileType, eProfileType.HierarchyNode, eProfileType.Version));

				if (compXRef != null)
				{
					detailList = (ArrayList)compXRef.GetDetailList(aSourceCellRef[eProfileType.LowLevelTotalVersion]);

					if (detailList != null)
					{
						foreach (ComplexProfileXRefDetailEntry detailEntry in detailList)
						{
							aCellRef[eProfileType.HierarchyNode] = detailEntry.PrimaryId;
							aCellRef[eProfileType.Version] = detailEntry.SecondaryId;

							if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
							{
								aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
							}
							else
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//Begin TT#2 - JScott - Assortment Planning - Phase 2

	/// <summary>
	/// The SetGradeStoreCubeRelationshipItem class identifies a cube and the low-level total relationship.
	/// </summary>

	public class SetGradeStorePlanCubeRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			SetGradeStoreXRef sgsXRef;
			ArrayList totalList;

			try
			{
				aCancel = false;

				sgsXRef = (SetGradeStoreXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new SetGradeStoreXRef());

				if (sgsXRef != null)
				{
					totalList = (ArrayList)sgsXRef.GetTotalList(aSourceCellRef[eProfileType.Store]);

					if (totalList != null)
					{
						foreach (SetGradeStoreXRefTotalEntry totEntry in totalList)
						{
							aCellRef[eProfileType.StoreGroupLevel] = totEntry.SetId;
							aCellRef[eProfileType.StoreGrade] = totEntry.GradeId;

							if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
							{
								aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
							}
							else
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
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
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			SetGradeStoreXRef sgsXRef;
			ArrayList detailList;

			try
			{
				aCancel = false;

				sgsXRef = (SetGradeStoreXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new SetGradeStoreXRef());

				if (sgsXRef != null)
				{
					detailList = (ArrayList)sgsXRef.GetDetailList(aSourceCellRef[eProfileType.StoreGroupLevel], aSourceCellRef[eProfileType.StoreGrade]);

					if (detailList != null)
					{
						foreach (int storeId in detailList)
						{
							aCellRef[eProfileType.Store] = storeId;

							if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
							{
								aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
							}
							else
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//End TT#2 - JScott - Assortment Planning - Phase 2
}
