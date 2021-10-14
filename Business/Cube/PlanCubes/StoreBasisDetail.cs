using System;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the StoreBasisDetail.
	/// </summary>
	/// <remarks>
	/// The StoreBasisDetail defines the values for the basis detail.
	/// </remarks>

	public class StoreBasisDetail : PlanBasisDetailCube
	{
		//=======
		// FIELDS
		//=======

		private bool _similarStore;
		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//private PlanCube _blendedDetailCube;
		//// End Track #5904
		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreBasisDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StoreBasisDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StoreBasisDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this StoreBasisDetail is a part of.
		/// </param>

		public StoreBasisDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			bool aSimilarStore)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Store)
		{
			_similarStore = aSimilarStore;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		override public eCubeType CubeType
		{
			get
			{
				return eCubeType.StoreBasisDetail;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if Similar Store is to be calculated.
		/// </summary>

		override public bool isSimlarStore
		{
			get
			{
				return _similarStore;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that returns the eCubeType of the Chain Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Chain Detail cube.
		/// </returns>

		override public eCubeType GetChainDetailCubeType()
		{
			return eCubeType.ChainBasisDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Detail cube.
		/// </returns>

		override public eCubeType GetLowLevelDetailCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Low-Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Total cube.
		/// </returns>

		override public eCubeType GetLowLevelTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		override public eCubeType GetStoreDetailCubeType()
		{
			return eCubeType.StoreBasisDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		override public eCubeType GetGroupTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.None;
		}

		//Begin Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Abstract method that returns the eCubeType of the Plan cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Plan cube.
		/// </returns>

		override public eCubeType GetPlanCubeType()
		{
			return eCubeType.None;
		}

		//End Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Returns the eCubeType of the Basis cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Basis cube.
		/// </returns>

		override public eCubeType GetBasisCubeType()
		{
			return eCubeType.StoreBasisDetail;
		}

		/// <summary>
		/// Method that returns the store status for the Cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to process.
		/// </param>
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		override public eStoreStatus GetStoreStatus(PlanCellReference aPlanCellRef)
		{
			try
			{
				return _SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is eligible.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		override public bool isStoreIneligible(PlanCellReference aPlanCellRef)
		{
			VariableProfile varProf;
			int nodeRID;

			try
			{
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//// Begin Track #6226 - stodd - mod version + sim store in basis not getting same result as actuals
				//if (aPlanCellRef.isCellActual)
				//{
				//// End Track #6226 - stodd - mod version + sim store in basis not getting same result as actuals
				//    return false;
				//}
				//else
				//{
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				varProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
				nodeRID = GetHierarchyNodeProfile(aPlanCellRef).Key;

                if (PlanCubeGroup.OpenParms.EligibilityNodeKey != Include.Undefined)
                {
                    nodeRID = PlanCubeGroup.OpenParms.EligibilityNodeKey;
                }

				switch (varProf.EligibilityType)
				{
					case eEligibilityType.Sales:
						return !CubeGroup.Transaction.GetStoreEligibilityForSales(
                            PlanCubeGroup.OpenParms.RequestingApplication, 
                            nodeRID, 
                            aPlanCellRef[eProfileType.Store], 
                            aPlanCellRef[eProfileType.Week]
                            );
					case eEligibilityType.Stock:
						return !CubeGroup.Transaction.GetStoreEligibilityForStock(
                            PlanCubeGroup.OpenParms.RequestingApplication,
                            nodeRID, 
                            aPlanCellRef[eProfileType.Store], 
                            aPlanCellRef[eProfileType.Week]
                            );
					case eEligibilityType.Either:
						return !CubeGroup.Transaction.GetStoreEligibilityForSales(
                            PlanCubeGroup.OpenParms.RequestingApplication,
                            nodeRID, 
                            aPlanCellRef[eProfileType.Store], 
                            aPlanCellRef[eProfileType.Week]) &&
							!CubeGroup.Transaction.GetStoreEligibilityForStock(
                                PlanCubeGroup.OpenParms.RequestingApplication, 
                                nodeRID, 
                                aPlanCellRef[eProfileType.Store], 
                                aPlanCellRef[eProfileType.Week]);
					default:
						return false;
				}
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//}
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is closed.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is closed.
		/// </returns>

		override public bool isStoreClosed(PlanCellReference aPlanCellRef)
		{
			try
			{
				return (_SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]) == eStoreStatus.Closed);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		override public bool isDatabaseVariable(ComputationVariableProfile aVarProf, ComputationCellReference aCompCellRef)
		{
			try
			{
				return aVarProf.isDatabaseVariable(eVariableCategory.Store, ((PlanCellReference)aCompCellRef).GetVersionProfile().Key, eCalendarDateType.Week);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		///// <summary>
		///// Reads the PlanCubeCell from the database.
		///// </summary>
		///// <param name="aPlanCellRef">
		///// A PlanCellReference object that identifies the PlanCubeCell to read.
		///// </param>

		//override public void ReadCell(PlanCellReference aPlanCellRef)
		//{
		//    System.Data.DataTable dataTable;
		//    int versionKey;
		//    int hierarchyNodeKey;
		//    int basisKey;
		//    int basisDetailKey;
		//    int weekKey;
		//    BasisReadLogKey readLogKey;
		//    BasisProfile basisProf;
		//    BasisDetailProfile basisDetailProf;
		//    ProfileList weekList;
		//    VersionProfile versionProf;
		//    PlanCellReference planCellRef;
		//    VariableProfile variableProf;
		//    int basisDetailHierarchyNodeKey;
		//    int basisDetailVersionKey;
		//    WeekProfile weekProf = null;

		//    try
		//    {
		//        versionKey = aPlanCellRef[eProfileType.Version];
		//        hierarchyNodeKey = aPlanCellRef[eProfileType.HierarchyNode];
		//        basisKey = aPlanCellRef[eProfileType.Basis];
		//        weekKey = aPlanCellRef[eProfileType.Week];
		//        versionProf = GetVersionProfile(aPlanCellRef);

		//        if (versionProf.BlendType == eForecastBlendType.Month)
		//        {
		//            weekProf = Calendar.GetWeek(weekKey);
		//        }

		//        if (versionProf.BlendType == eForecastBlendType.Week &&
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //((weekKey < Calendar.CurrentWeek.Key && versionProf.Key != versionProf.ActualVersionRID) ||
		//            //(weekKey >= Calendar.CurrentWeek.Key && versionProf.Key != versionProf.ForecastVersionRID)))
		//            _blendedDetailCube != null)
		//            // End Track #5904
		//        {
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //if (weekKey < Calendar.CurrentWeek.Key && versionProf.Key != versionProf.ActualVersionRID)
		//            if (weekKey < Calendar.CurrentWeek.Key)
		//            // End Track #5904
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904
		//                planCellRef[eProfileType.BasisVersion] = versionProf.ActualVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //else if (weekKey == Calendar.CurrentWeek.Key && versionProf.Key != versionProf.ForecastVersionRID)
		//            else if (weekKey == Calendar.CurrentWeek.Key)
		//            // End Track #5904
		//            {
		//                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904

		//                if (variableProf.VariableType == eVariableType.BegStock)
		//                {
		//                    planCellRef[eProfileType.BasisVersion] = versionProf.ActualVersionRID;
		//                }
		//                else
		//                {
		//                    planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                }

		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }
		//            else
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904
		//                planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }

		//            aPlanCellRef.isCellLoadedFromDB = true;
		//        }
		//        else if (versionProf.BlendType == eForecastBlendType.Month &&
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ActualVersionRID) ||
		//            //(weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ActualVersionRID) ||
		//            //(weekProf.Period.FiscalYear > Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ForecastVersionRID) ||
		//            //(weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod >= Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ForecastVersionRID)))
		//            _blendedDetailCube != null)
		//            // End Track #5904
		//        {
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //if ((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ActualVersionRID) ||
		//            //    (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ActualVersionRID))
		//            if ((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear) ||
		//                (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod))
		//            // End Track #5904
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904
		//                planCellRef[eProfileType.BasisVersion] = versionProf.ActualVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }
		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //else if (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod == Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ForecastVersionRID)
		//            else if (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod == Calendar.CurrentWeek.Period.FiscalPeriod)
		//            // End Track #5904
		//            {
		//                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904

		//                if (variableProf.VariableType == eVariableType.BegStock)
		//                {
		//                    if (versionProf.BlendCurrentByMonth)
		//                    {
		//                        if (weekProf.WeekInPeriod <= Calendar.CurrentWeek.WeekInPeriod)
		//                        {
		//                            planCellRef[eProfileType.BasisVersion] = versionProf.ActualVersionRID;
		//                        }
		//                        else
		//                        {
		//                            planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                        }
		//                    }
		//                    else if (weekProf.WeekInPeriod == 1)
		//                    {
		//                        planCellRef[eProfileType.BasisVersion] = versionProf.ActualVersionRID;
		//                    }
		//                    else
		//                    {
		//                        planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                    }
		//                }
		//                else
		//                {
		//                    planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                }

		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }
		//            else
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                // End Track #5904
		//                planCellRef[eProfileType.BasisVersion] = versionProf.ForecastVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//            }

		//            aPlanCellRef.isCellLoadedFromDB = true;
		//        }
		//        else
		//        {
		//            basisProf = (BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(PlanCubeGroup, hierarchyNodeKey, versionKey).FindKey(basisKey);

		//            if (basisProf.areDetailsAligned(_SAB.ApplicationServerSession))
		//            {
		//                basisDetailKey = aPlanCellRef[eProfileType.BasisDetail];
		//                basisDetailHierarchyNodeKey = aPlanCellRef[eProfileType.BasisHierarchyNode];
		//                basisDetailVersionKey = aPlanCellRef[eProfileType.BasisVersion];
		//                weekKey = aPlanCellRef[eProfileType.Week];

		//                readLogKey = new BasisReadLogKey(versionKey, hierarchyNodeKey, basisKey, basisDetailKey,
		//                    basisDetailHierarchyNodeKey, basisDetailVersionKey);

		//                basisDetailProf = (BasisDetailProfile)basisProf.BasisDetailProfileList.FindKey(basisDetailKey);

		//                if (!_planReadLog.Contains(readLogKey, weekKey))
		//                {
		//                    if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)(MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable])), aPlanCellRef) &&
		//                        aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
		//                    {
		//                        weekList = _planReadLog.DetermineWeeksToRead(_SAB, readLogKey, weekKey);

		//                        if (weekList.Count > 0)
		//                        {
		//                            ExpandDimensionSize(eProfileType.Week, weekList);

		//                            // Begin Track #5806 - JSmith - Blended version value wrong 
		//                            //dataTable = _varData.StoreWeek_Read(basisDetailProf.HierarchyNodeProfile.Key, basisDetailProf.VersionProfile.Key, weekList, MasterVariableProfileList);
		//                            dataTable = _varData.StoreWeek_Read(basisDetailHierarchyNodeKey, basisDetailVersionKey, weekList, MasterVariableProfileList);
		//                            // End Track #5806
		//                            intLoadDataTableToCube(dataTable, versionKey, hierarchyNodeKey, basisProf, basisDetailProf, basisDetailHierarchyNodeKey, basisDetailVersionKey);
		//                            intLoadActualWTDSales(versionKey, hierarchyNodeKey, weekList, basisProf, basisDetailProf, basisDetailHierarchyNodeKey, basisDetailVersionKey);
		//                            _planReadLog.Add(readLogKey, weekList);
		//                        }
		//                    }
		//                    else
		//                    {
		//                        aPlanCellRef.isCellLoadedFromDB = true;
		//                    }
		//                }
		//                else
		//                {
		//                    aPlanCellRef.isCellLoadedFromDB = true;
		//                }
		//            }
		//            else
		//            {
		//                aPlanCellRef.isCellLoadedFromDB = true;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		override public void ReadCell(PlanCellReference aPlanCellRef)
		{
			int versionKey;
			int hierarchyNodeKey;
			VersionProfile basisVersionProfile;
			int basisHierarchyNodeKey;
			int basisKey;
			int basisDetailKey;
			int weekKey;
			BasisReadLogKey readLogKey;
			BasisProfile basisProf;
			BasisDetailProfile basisDetailProf;
			ProfileList weekList;

			try
			{
				versionKey = aPlanCellRef[eProfileType.Version];
				hierarchyNodeKey = aPlanCellRef[eProfileType.HierarchyNode];
				basisKey = aPlanCellRef[eProfileType.Basis];
				basisProf = (BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(PlanCubeGroup, hierarchyNodeKey, versionKey).FindKey(basisKey);

				if (basisProf.areDetailsAligned(SAB.ApplicationServerSession))
				{
					basisDetailKey = aPlanCellRef[eProfileType.BasisDetail];
					basisVersionProfile = GetVersionProfile(aPlanCellRef);
					basisHierarchyNodeKey = aPlanCellRef[eProfileType.BasisHierarchyNode];
					weekKey = aPlanCellRef[eProfileType.Week];

					readLogKey = new BasisReadLogKey(versionKey, hierarchyNodeKey, basisKey, basisDetailKey,
						basisHierarchyNodeKey, basisVersionProfile.Key);

					if (!_planReadLog.Contains(readLogKey, weekKey))
					{
						basisDetailProf = (BasisDetailProfile)basisProf.BasisDetailProfileList.FindKey(basisDetailKey);

						if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]), aPlanCellRef) &&
							aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
						{
							weekList = _planReadLog.DetermineWeeksToRead(SAB, readLogKey, weekKey);

							if (weekList.Count > 0)
							{
								ExpandDimensionSize(eProfileType.Week, weekList);
								intReadAndLoadDatabaseToCube(versionKey, hierarchyNodeKey, basisVersionProfile, basisHierarchyNodeKey, weekList, basisProf, basisDetailProf);
								_planReadLog.Add(readLogKey, weekList);
							}
						}
						else
						{
							aPlanCellRef.isCellLoadedFromDB = true;
						}
					}
					else
					{
						aPlanCellRef.isCellLoadedFromDB = true;
					}
				}
				else
				{
					aPlanCellRef.isCellLoadedFromDB = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"

		/// <summary>
		/// Returns true if any cell for the given ProfileList of PlanProfiles has changed.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasAnyPlanChanged(ProfileList aPlanProfileList)
		{
			return false;
		}

		/// <summary>
		/// Returns true if any cell for the given PlanProfile has changed.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasPlanChanged(PlanProfile aPlanProfile)
		{
			return false;
		}

		/// <summary>
		/// Allows a cube to specify custom initializations for a Cell.  Occurs after the standard Cell initialization.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to initialize.
		/// </param>

		//Begin TT#894 - JScott - Locks are not "bolded" or showing held in OTS FC Review 
		//override public void InitCellValue(PlanCellReference aPlanCellRef)
		//{
		//    VersionProfile vp;
		//    ProfileList basisProfList;
		//    BasisProfile basisProf;
		//    BasisDetailProfile basisDetailProf;
		//    SimilarStoreList simStoreList;
		//    PlanCellReference simStoreBasisDetailCellRef;
		//    SimilarStoreProfile simStoreProf;
		//    VariableProfile varProf;
		//    WeekProfile weekProf;

		//    try
		//    {
		//        vp = GetVersionProfile(aPlanCellRef);

		//        if (_similarStore &&
		//            GetVersionProfile(aPlanCellRef).AllowSimilarStore)
		//        {
		//            basisProfList = PlanCubeGroup.OpenParms.GetBasisProfileList(PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]);
		//            basisProf = (BasisProfile)basisProfList.FindKey(aPlanCellRef[eProfileType.Basis]);
		//            basisDetailProf = (BasisDetailProfile)basisProf.BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail]);

		//            simStoreList = Transaction.GetSimilarStoreList(basisDetailProf.HierarchyNodeProfile.Key);

		//            simStoreBasisDetailCellRef = (PlanCellReference)aPlanCellRef.Copy();
		//            simStoreProf = (SimilarStoreProfile)simStoreList.FindKey(aPlanCellRef[eProfileType.Store]);

		//            if (simStoreProf != null)
		//            {
		//                varProf = (VariableProfile)PlanCubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);

		//                if (aPlanCellRef.PlanCube.isDatabaseVariable(varProf, aPlanCellRef))
		//                {
		//                    weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(aPlanCellRef[eProfileType.Week]);

		//                    if (varProf.SimilarStoreDateType != eSimilarStoreDateType.None && intCalcSimilarStore(simStoreProf, weekProf))
		//                    {
		//                        aPlanCellRef.SetCompCellValue(eSetCellMode.Initialize, (double)(decimal)intGetSimilarStoreValue(simStoreBasisDetailCellRef, simStoreList, simStoreProf, weekProf));
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		override public void InitCellValue(ComputationCellReference aCompCellRef)
		{
			PlanCellReference planCellRef;
			VersionProfile vp;
			ProfileList basisProfList;
			BasisProfile basisProf;
			BasisDetailProfile basisDetailProf;
			SimilarStoreList simStoreList;
			PlanCellReference simStoreBasisDetailCellRef;
			SimilarStoreProfile simStoreProf;
			VariableProfile varProf;
			WeekProfile weekProf;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				vp = GetVersionProfile(planCellRef);

				if (_similarStore &&
					GetVersionProfile(planCellRef).AllowSimilarStore)
				{
					basisProfList = PlanCubeGroup.OpenParms.GetBasisProfileList(PlanCubeGroup, aCompCellRef[eProfileType.HierarchyNode], aCompCellRef[eProfileType.Version]);
					basisProf = (BasisProfile)basisProfList.FindKey(aCompCellRef[eProfileType.Basis]);
					basisDetailProf = (BasisDetailProfile)basisProf.BasisDetailProfileList.FindKey(aCompCellRef[eProfileType.BasisDetail]);

					simStoreList = Transaction.GetSimilarStoreList(basisDetailProf.HierarchyNodeProfile.Key);

					simStoreBasisDetailCellRef = (PlanCellReference)aCompCellRef.Copy();
					simStoreProf = (SimilarStoreProfile)simStoreList.FindKey(aCompCellRef[eProfileType.Store]);

					if (simStoreProf != null)
					{
						varProf = (VariableProfile)PlanCubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aCompCellRef[eProfileType.Variable]);

						// BEGIN TT#3054 - stodd - forecasting issue with sales R/P variables and similar stores
						// Removed the need to ask if this is a DB varaible. Instead changed the computationVariables to have the correct SimilarStoreDateType indicator
						//if (planCellRef.PlanCube.isDatabaseVariable(varProf, aCompCellRef))
						//{
							weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(aCompCellRef[eProfileType.Week]);

							if (varProf.SimilarStoreDateType != eSimilarStoreDateType.None && intCalcSimilarStore(simStoreProf, weekProf))
							{
							    // BEGIN TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
								aCompCellRef.SetCompCellValue(eSetCellMode.Initialize, (double)(decimal)intGetSimilarStoreValue(simStoreBasisDetailCellRef, simStoreList, simStoreProf, weekProf), true);
								// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
							}
						//}
						// END TT#3054 - stodd - forecasting issue with sales R/P variables and similar stores

					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End TT#894 - JScott - Locks are not "bolded" or showing held in OTS FC Review 

		///// <summary>
		///// Reads all values into the Cube.
		///// </summary>

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//public void ReadAndLoadCube(VersionProfile aVersionProfile, int aHierarchyNodeKey, ProfileList aBasisProfileList, int aAligntToPlanWeek)
		//{
		//    // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//    //BasisReadLogKey readLogKey;
		//    //ProfileList actualWeekList;
		//    //ProfileList forecastWeekList;
		//    //WeekProfile currentWeek;
		//    //BasisDetailProfile blendBasisDetailProfile;
		//    //ProfileList readWeekList;
		//    //int preWeeks;
		//    //int postWeeks;
		//    // End Track #5904

		//    try
		//    {
		//        foreach (BasisProfile basisProfile in aBasisProfileList)
		//        {
		//            foreach (BasisDetailProfile basisDetailProfile in basisProfile.BasisDetailProfileList)
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                //if (aAligntToPlanWeek > 0 && basisDetailProfile.ForecastingInfo.ShiftWeeksWithPlanWeek == true)
		//                //{
		//                //    basisDetailProfile.RealignBasisWeeks(_SAB.ApplicationServerSession, aAligntToPlanWeek);
		//                //}

		//                //readLogKey = new BasisReadLogKey(aVersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, basisDetailProfile.Key,
		//                //    basisDetailProfile.HierarchyNodeProfile.Key, basisDetailProfile.VersionProfile.Key);

		//                //if (basisProfile.areDetailsAligned(_SAB.ApplicationServerSession))
		//                //{
		//                //    preWeeks = Include.PlanReadPreWeeks;
		//                //    postWeeks = Include.PlanReadPostWeeks;
		//                //}
		//                //else
		//                //{
		//                //    preWeeks = 0;
		//                //    postWeeks = 0;
		//                //}

		//                //readWeekList = _planReadLog.DetermineWeeksToRead(
		//                //    _SAB,
		//                //    readLogKey,
		//                //    basisDetailProfile.GetAlignedBasisWeekProfileList(_SAB.ApplicationServerSession),
		//                //    preWeeks,
		//                //    postWeeks);

		//                //if (basisDetailProfile.VersionProfile.IsBlendedVersion)
		//                //{
		//                //    actualWeekList = new ProfileList(eProfileType.Week);
		//                //    forecastWeekList = new ProfileList(eProfileType.Week);
		//                //    currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

		//                //    foreach (WeekProfile week in readWeekList.ArrayList)
		//                //    {
		//                //        if (basisDetailProfile.VersionProfile.BlendType == eForecastBlendType.Month &&
		//                //            week.Period.FiscalYear == currentWeek.Period.FiscalYear &&
		//                //            week.Period.FiscalPeriod == currentWeek.Period.FiscalPeriod)
		//                //        {
		//                //            actualWeekList.Add(week);
		//                //            forecastWeekList.Add(week);
		//                //        }
		//                //        else if (week.YearWeek == currentWeek.YearWeek)
		//                //        {
		//                //            actualWeekList.Add(week);
		//                //            forecastWeekList.Add(week);
		//                //        }
		//                //        else if (week.YearWeek < currentWeek.YearWeek)
		//                //        {
		//                //            actualWeekList.Add(week);
		//                //        }
		//                //        else
		//                //        {
		//                //            forecastWeekList.Add(week);
		//                //        }
		//                //    }

		//                //    if (actualWeekList.Count > 0)
		//                //    {
		//                //        blendBasisDetailProfile = basisDetailProfile.Copy(_SAB.ApplicationServerSession,true);
		//                //        blendBasisDetailProfile.VersionProfile = (VersionProfile)_SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(basisDetailProfile.VersionProfile.ActualVersionRID);

		//                //        readLogKey = new BasisReadLogKey(blendBasisDetailProfile.VersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, blendBasisDetailProfile.Key,
		//                //            blendBasisDetailProfile.HierarchyNodeProfile.Key, blendBasisDetailProfile.VersionProfile.Key);

		//                //        readWeekList = _planReadLog.DetermineWeeksToRead(
		//                //            _SAB,
		//                //            readLogKey,
		//                //            actualWeekList,
		//                //            0,
		//                //            0);

		//                //        if (readWeekList.Count > 0)
		//                //        {
		//                //            intReadAndLoadDatabaseToCube(aVersionProfile.Key, basisDetailProfile.HierarchyNodeProfile.Key, 
		//                //                readWeekList, basisProfile, blendBasisDetailProfile);
		//                //            _planReadLog.Add(readLogKey, readWeekList);
		//                //        }
		//                //    }

		//                //    if (forecastWeekList.Count > 0)
		//                //    {
		//                //        blendBasisDetailProfile = basisDetailProfile.Copy(_SAB.ApplicationServerSession,true);
		//                //        blendBasisDetailProfile.VersionProfile = (VersionProfile)_SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(basisDetailProfile.VersionProfile.ForecastVersionRID);

		//                //        readLogKey = new BasisReadLogKey(blendBasisDetailProfile.VersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, blendBasisDetailProfile.Key,
		//                //            blendBasisDetailProfile.HierarchyNodeProfile.Key, blendBasisDetailProfile.VersionProfile.Key);

		//                //        readWeekList = _planReadLog.DetermineWeeksToRead(
		//                //            _SAB,
		//                //            readLogKey,
		//                //            forecastWeekList,
		//                //            0,
		//                //            0);

		//                //        if (readWeekList.Count > 0)
		//                //        {
		//                //            intReadAndLoadDatabaseToCube(aVersionProfile.Key, basisDetailProfile.HierarchyNodeProfile.Key, 
		//                //                readWeekList, basisProfile, blendBasisDetailProfile);
		//                //            _planReadLog.Add(readLogKey, readWeekList);
		//                //        }
		//                //    }
		//                //}
		//                //else
		//                //{
		//                //    if (readWeekList.Count > 0)
		//                //    {
		//                //        intReadAndLoadDatabaseToCube(aVersionProfile.Key, aHierarchyNodeKey, readWeekList,
		//                //            basisProfile, basisDetailProfile);
		//                //        _planReadLog.Add(readLogKey, readWeekList);
		//                //    }
		//                //}
		//                ReadAndLoadBasisDetail(aVersionProfile, aHierarchyNodeKey, aBasisProfileList, aAligntToPlanWeek, true, basisProfile, basisDetailProfile);
		//                // End Track #5904
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		///// <summary>
		///// Reads all values into the Cube for a given BasisProfile and BasisDetailProfile.
		///// </summary>

		//private void ReadAndLoadBasisDetail(
		//    VersionProfile aVersionProfile,
		//    int aHierarchyNodeKey,
		//    ProfileList aBasisProfileList,
		//    int aAligntToPlanWeek,
		//    bool aUseBlended,
		//    BasisProfile aBasisProfile,
		//    BasisDetailProfile aBasisDetailProfile)
		//{
		//    BasisReadLogKey readLogKey;
		//    ProfileList actualWeekList;
		//    ProfileList forecastWeekList;
		//    WeekProfile currentWeek;
		//    ProfileList readWeekList;
		//    int preWeeks;
		//    int postWeeks;

		//    try
		//    {
		//        if (aAligntToPlanWeek > 0 && aBasisDetailProfile.ForecastingInfo.ShiftWeeksWithPlanWeek == true)
		//        {
		//            aBasisDetailProfile.RealignBasisWeeks(_SAB.ApplicationServerSession, aAligntToPlanWeek);
		//        }

		//        readLogKey = new BasisReadLogKey(aVersionProfile.Key, aHierarchyNodeKey, aBasisProfile.Key, aBasisDetailProfile.Key,
		//            aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);

		//        if (aBasisProfile.areDetailsAligned(_SAB.ApplicationServerSession))
		//        {
		//            preWeeks = Include.PlanReadPreWeeks;
		//            postWeeks = Include.PlanReadPostWeeks;
		//        }
		//        else
		//        {
		//            preWeeks = 0;
		//            postWeeks = 0;
		//        }

		//        readWeekList = _planReadLog.DetermineWeeksToRead(
		//            _SAB,
		//            readLogKey,
		//            aBasisDetailProfile.GetAlignedBasisWeekProfileList(_SAB.ApplicationServerSession),
		//            preWeeks,
		//            postWeeks);

		//        if (aBasisDetailProfile.VersionProfile.IsBlendedVersion && aUseBlended)
		//        {
		//            if (_blendedDetailCube == null)
		//            {
		//                _blendedDetailCube = new StoreBasisDetail(SAB, Transaction, this.PlanCubeGroup, this.CubeDefinition, this.PlanCubeGroup.OpenParms.SimilarStores);
		//                Transaction.PlanComputations.PlanCubeInitialization.StoreBasisDetail(_blendedDetailCube, this.PlanCubeGroup.OpenParms.GetDisplayType(SAB.ApplicationServerSession));
		//            }

		//            actualWeekList = new ProfileList(eProfileType.Week);
		//            forecastWeekList = new ProfileList(eProfileType.Week);
		//            currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

		//            foreach (WeekProfile week in readWeekList.ArrayList)
		//            {
		//                if (aBasisDetailProfile.VersionProfile.BlendType == eForecastBlendType.Month &&
		//                    week.Period.FiscalYear == currentWeek.Period.FiscalYear &&
		//                    week.Period.FiscalPeriod == currentWeek.Period.FiscalPeriod)
		//                {
		//                    actualWeekList.Add(week);
		//                    forecastWeekList.Add(week);
		//                }
		//                else if (week.YearWeek == currentWeek.YearWeek)
		//                {
		//                    actualWeekList.Add(week);
		//                    forecastWeekList.Add(week);
		//                }
		//                else if (week.YearWeek < currentWeek.YearWeek)
		//                {
		//                    actualWeekList.Add(week);
		//                }
		//                else
		//                {
		//                    forecastWeekList.Add(week);
		//                }
		//            }

		//            if (actualWeekList.Count > 0)
		//            {
		//                ((StoreBasisDetail)_blendedDetailCube).ReadAndLoadBasisDetail(
		//                    (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(aBasisDetailProfile.VersionProfile.ActualVersionRID),
		//                    aHierarchyNodeKey,
		//                    aBasisProfileList,
		//                    aAligntToPlanWeek,
		//                    false,
		//                    aBasisProfile,
		//                    aBasisDetailProfile);
		//            }

		//            if (forecastWeekList.Count > 0)
		//            {
		//                ((StoreBasisDetail)_blendedDetailCube).ReadAndLoadBasisDetail(
		//                    (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(aBasisDetailProfile.VersionProfile.ForecastVersionRID),
		//                    aHierarchyNodeKey,
		//                    aBasisProfileList,
		//                    aAligntToPlanWeek,
		//                    false,
		//                    aBasisProfile,
		//                    aBasisDetailProfile);
		//            }
		//        }
		//        else
		//        {
		//            if (readWeekList.Count > 0)
		//            {
		//                intReadAndLoadDatabaseToCube(aVersionProfile.Key, aHierarchyNodeKey, readWeekList,
		//                    aBasisProfile, aBasisDetailProfile);
		//                _planReadLog.Add(readLogKey, readWeekList);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//// End Track #5904
		public void ReadAndLoadCube(VersionProfile aVersionProfile, int aHierarchyNodeKey, ProfileList aBasisProfileList, int aAligntToPlanWeek)
		{
			BasisReadLogKey readLogKey;
			ProfileList readWeekList;
			int preWeeks;
			int postWeeks;

			try
			{
				foreach (BasisProfile basisProfile in aBasisProfileList)
				{
					foreach (BasisDetailProfile basisDetailProfile in basisProfile.BasisDetailProfileList)
					{
						if (aAligntToPlanWeek > 0 && basisDetailProfile.ForecastingInfo.ShiftWeeksWithPlanWeek == true)
						{
						    basisDetailProfile.RealignBasisWeeks(_SAB.ApplicationServerSession, aAligntToPlanWeek);
						}

						readLogKey = new BasisReadLogKey(aVersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, basisDetailProfile.Key,
						    basisDetailProfile.HierarchyNodeProfile.Key, basisDetailProfile.VersionProfile.Key);

						if (basisProfile.areDetailsAligned(SAB.ApplicationServerSession))
						{
							preWeeks = Include.PlanReadPreWeeks;
							postWeeks = Include.PlanReadPostWeeks;
						}
						else
						{
							preWeeks = 0;
							postWeeks = 0;
						}

						readWeekList = _planReadLog.DetermineWeeksToRead(
							SAB,
							readLogKey,
							basisDetailProfile.GetAlignedBasisWeekProfileList(SAB.ApplicationServerSession),
							preWeeks,
							postWeeks);

						if (readWeekList.Count > 0)
						{
							intReadAndLoadDatabaseToCube(aVersionProfile.Key, aHierarchyNodeKey, basisDetailProfile.VersionProfile, basisDetailProfile.HierarchyNodeProfile.Key, readWeekList, basisProfile, basisDetailProfile);
							_planReadLog.Add(readLogKey, readWeekList);
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
		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"

		private double intGetSimilarStoreValue(
			PlanCellReference aBasisDetailCellRef,
			SimilarStoreList aSimStoreList,
			SimilarStoreProfile aSimStoreProf,
			WeekProfile aWeekProf)
		{
			double totalSimValue = 0;
			SimilarStoreProfile simStoreProf;
			int i;

			try
			{
				i = 0;

				foreach (int strID in aSimStoreProf.SimStores)
				{
					simStoreProf = (SimilarStoreProfile)aSimStoreList.FindKey(strID);

					if (simStoreProf == null || PlanCubeGroup.SimilarStoreModelHash.Contains(strID) || !intCalcSimilarStore(simStoreProf, aWeekProf))
					{
						aBasisDetailCellRef[eProfileType.Store] = strID;
						totalSimValue += aBasisDetailCellRef.CurrentCellValue;
						i++;
					}
				}

				if (i > 0)
				{
					return (totalSimValue / i) * (aSimStoreProf.SimStoreRatio / Include.DefaultSimilarStoreRatio);
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool intCalcSimilarStore(
			SimilarStoreProfile aSimStoreProf,
			WeekProfile aWeekProf)
		{
			ProfileList simStoreWeekList;
			int endWeek;

			try
			{
				simStoreWeekList = aSimStoreProf.SimStoreWeekList;
				endWeek = simStoreWeekList[simStoreWeekList.Count - 1].Key;

				if (aWeekProf.Key > endWeek)
				{
					return false;
				}

				if (simStoreWeekList.Count > 1)
				{
					if (aWeekProf.Key < simStoreWeekList[0].Key)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//private void intReadAndLoadDatabaseToCube(int aVersionKey, int aHierarchyNodeKey, ProfileList aReadWeekList,
		//    BasisProfile aBasisProfile, BasisDetailProfile aBasisDetailProfile)
		//{
		//    System.Data.DataTable dataTable;

		//    try
		//    {
		//        dataTable = _varData.StoreWeek_Read(aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key, aReadWeekList, MasterVariableProfileList);
		//        intLoadDataTableToCube(dataTable, aVersionKey, aHierarchyNodeKey, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
		//        intLoadActualWTDSales(aVersionKey, aHierarchyNodeKey, aReadWeekList, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		private void intReadAndLoadDatabaseToCube(
			int aVersionKey,
			int aHierarchyNodeKey,
			VersionProfile aBasisVersion,
			int aBasisHierarchyNodeKey,
			ProfileList aReadWeekList,
			BasisProfile aBasisProfile,
			BasisDetailProfile aBasisDetailProfile)
		{
			ProfileList actualWeekList;
			ProfileList forecastWeekList;
			WeekProfile currentWeek;
			System.Data.DataTable dataTable;

			try
			{
				if (aBasisVersion.IsBlendedVersion)
				{
					actualWeekList = new ProfileList(eProfileType.Week);
					forecastWeekList = new ProfileList(eProfileType.Week);
					currentWeek = SAB.ApplicationServerSession.Calendar.CurrentWeek;

					foreach (WeekProfile week in aReadWeekList.ArrayList)
					{
						if (aBasisVersion.BlendType == eForecastBlendType.Month &&
							week.Period.FiscalYear == currentWeek.Period.FiscalYear &&
							week.Period.FiscalPeriod == currentWeek.Period.FiscalPeriod)
						{
							actualWeekList.Add(week);
							forecastWeekList.Add(week);
						}
						else if (week.YearWeek == currentWeek.YearWeek)
						{
							actualWeekList.Add(week);
							forecastWeekList.Add(week);
						}
						else if (week.YearWeek < currentWeek.YearWeek)
						{
							actualWeekList.Add(week);
						}
						else
						{
							forecastWeekList.Add(week);
						}
					}

					if (actualWeekList.Count > 0)
					{
						dataTable = _varData.StoreWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.ActualVersionRID, actualWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.ActualVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
						//Begin Track #6203 - JScott - WTD Sales not showing in current week for Actuals
						intLoadActualWTDSales(aVersionKey, aHierarchyNodeKey, actualWeekList, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
						//End Track #6203 - JScott - WTD Sales not showing in current week for Actuals
					}

					if (forecastWeekList.Count > 0)
					{
						dataTable = _varData.StoreWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.ForecastVersionRID, forecastWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.ForecastVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
						//Begin Track #6203 - JScott - WTD Sales not showing in current week for Actuals
						intLoadActualWTDSales(aVersionKey, aHierarchyNodeKey, forecastWeekList, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
						//End Track #6203 - JScott - WTD Sales not showing in current week for Actuals
					}
				}
				else
				{
					if (aReadWeekList.Count > 0)
					{
						dataTable = _varData.StoreWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.Key, aReadWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.Key, false);
						//Begin Track #6203 - JScott - WTD Sales not showing in current week for Actuals
						intLoadActualWTDSales(aVersionKey, aHierarchyNodeKey, aReadWeekList, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
						//End Track #6203 - JScott - WTD Sales not showing in current week for Actuals
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		/// <summary>
		/// Private method that loads the values in a given DataTable to the Cube.
		/// </summary>
		/// <param name="aDataTable">
		/// The DataTable that contains the values to load.
		/// </param>

		private void intLoadDataTableToCube(
			System.Data.DataTable aDataTable,
			//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
			//int aVersionRID,
			//int aHierarchyRID,
			int aHierarchyRID,
			int aVersionRID,
			//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
			BasisProfile aBasisProf,
			BasisDetailProfile aBasisDetailProf,
			int aBasisHierarchyRID,
			//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
			//int aBasisVersionRID)
			int aBasisToVersionRID,
			int aBasisFromVersionRID,
			bool aCurrentWeek)
			//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		{
			PlanCellReference basisDetailCellRef;

			try
			{
				if (aDataTable.Rows.Count > 0)
				{
					basisDetailCellRef = (PlanCellReference)CreateCellReference();
					basisDetailCellRef[eProfileType.Version] = aVersionRID;
					basisDetailCellRef[eProfileType.HierarchyNode] = aHierarchyRID;
					basisDetailCellRef[eProfileType.Basis] = aBasisProf.Key;
					basisDetailCellRef[eProfileType.BasisDetail] = aBasisDetailProf.Key;
					basisDetailCellRef[eProfileType.BasisHierarchyNode] = aBasisHierarchyRID;
					//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
					//basisDetailCellRef[eProfileType.BasisVersion] = aBasisVersionRID;
					basisDetailCellRef[eProfileType.BasisVersion] = aBasisToVersionRID;
					//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
					basisDetailCellRef[eProfileType.QuantityVariable] = CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

					foreach (System.Data.DataRow dataRow in aDataTable.Rows)
					{
						basisDetailCellRef[eProfileType.Week] = Convert.ToInt32(dataRow["TIME_ID"], CultureInfo.CurrentUICulture);
						basisDetailCellRef[eProfileType.Store] = Convert.ToInt32(dataRow["ST_RID"], CultureInfo.CurrentUICulture);

						foreach (VariableProfile varProf in CubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList)
						{
							if (basisDetailCellRef.PlanCube.isDatabaseVariable(varProf, basisDetailCellRef))
							{
								basisDetailCellRef[eProfileType.Variable] = varProf.Key;

								if (!basisDetailCellRef.isCellLoadedFromDB)
								{
									//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
									if (!aCurrentWeek || basisDetailCellRef.GetVersionProfileOfData().Key == aBasisFromVersionRID)
									{
									//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
										basisDetailCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture) * aBasisDetailProf.AdjustedWeight, false);
										basisDetailCellRef.isCellLoadedFromDB = true;
									//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
									}
									//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
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

		private void intLoadActualWTDSales(
			int aVersionRID,
			int aHierachyRID,
			ProfileList aWeekList,
			BasisProfile aBasisProf,
			BasisDetailProfile aBasisDetailProf,
			int aBasisHierarchyRID,
			int aBasisVersionRID)
		{
			PlanCellReference basisDetailCellRef;

			try
			{
				if (aBasisVersionRID == Include.FV_ActualRID || aBasisVersionRID == Include.FV_ModifiedRID)
				{
					foreach (WeekProfile weekProf in aWeekList)
					{
						if (weekProf == _SAB.ApplicationServerSession.Calendar.CurrentWeek)
						{
							basisDetailCellRef = (PlanCellReference)CreateCellReference();
							basisDetailCellRef[eProfileType.Version] = aVersionRID;
							basisDetailCellRef[eProfileType.HierarchyNode] = aHierachyRID;
							basisDetailCellRef[eProfileType.Basis] = aBasisProf.Key;
							basisDetailCellRef[eProfileType.BasisDetail] = aBasisDetailProf.Key;
							basisDetailCellRef[eProfileType.BasisHierarchyNode] = aBasisHierarchyRID;
							basisDetailCellRef[eProfileType.BasisVersion] = aBasisVersionRID;
							basisDetailCellRef[eProfileType.Week] = weekProf.Key;
							basisDetailCellRef[eProfileType.QuantityVariable] = CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

							foreach (VariableProfile varProf in CubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList)
							{
								if (varProf.ActualWTDSalesVariableList != null)
								{
									basisDetailCellRef[eProfileType.Variable] = varProf.Key;

									foreach (StoreProfile storeProf in CubeGroup.GetMasterProfileList(eProfileType.Store))
									{
										basisDetailCellRef[eProfileType.Store] = storeProf.Key;

										basisDetailCellRef.SetLoadCellValue(Convert.ToDouble(Transaction.GetStoreCurrentWeekToDaySales(aBasisHierarchyRID, varProf.ActualWTDSalesVariableList, storeProf.Key), CultureInfo.CurrentUICulture) * aBasisDetailProf.AdjustedWeight, false);
									}
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
	}
}
