using System;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the ChainBasisDetail.
	/// </summary>
	/// <remarks>
	/// The ChainBasisDetail defines the values for the basis detail.
	/// </remarks>

	public class ChainBasisDetail : PlanBasisDetailCube
	{
		//=======
		// FIELDS
		//=======

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//private PlanCube _blendedDetailCube;
		//// End Track #5904

		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ChainBasisDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ChainBasisDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ChainBasisDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this ChainBasisDetail is a part of.
		/// </param>

		public ChainBasisDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Chain)
		{
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
				return eCubeType.ChainBasisDetail;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if Similar Store is to be calculated.
		/// </summary>

		override public bool isSimlarStore
		{
			get
			{
				return false;
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
            // Begin TT#4277 - RMatelic - JAB - PCF Custom Calcs - Variable: UnitAvailability Init formula requires this base change when used as a plan basis
            //return eCubeType.None;
            return eCubeType.ChainBasisDateTotal;
            // End TT#4277
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
			return eCubeType.None;
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
			return eStoreStatus.None;
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is eligible.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that point to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		override public bool isStoreIneligible(PlanCellReference aPlanCellRef)
		{
			return false;
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
			return false;
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
				return aVarProf.isDatabaseVariable(eVariableCategory.Chain, ((PlanCellReference)aCompCellRef).GetVersionProfile().Key, eCalendarDateType.Week);
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
		//    PlanCellReference planCellRef;
		//    VersionProfile versionProf;
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

		//            if (basisProf.areDetailsAligned(SAB.ApplicationServerSession))
		//            {
		//                basisDetailKey = aPlanCellRef[eProfileType.BasisDetail];
		//                basisDetailHierarchyNodeKey = aPlanCellRef[eProfileType.BasisHierarchyNode];
		//                basisDetailVersionKey = aPlanCellRef[eProfileType.BasisVersion];
		//                weekKey = aPlanCellRef[eProfileType.Week];

		//                readLogKey = new BasisReadLogKey(versionKey, hierarchyNodeKey, basisKey, basisDetailKey,
		//                    basisDetailHierarchyNodeKey, basisDetailVersionKey);

		//                if (!_planReadLog.Contains(readLogKey, weekKey))
		//                {
		//                    basisDetailProf = (BasisDetailProfile)basisProf.BasisDetailProfileList.FindKey(basisDetailKey);

		//                    if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]), aPlanCellRef)&&
		//                        aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
		//                    {
		//                        weekList = _planReadLog.DetermineWeeksToRead(SAB, readLogKey, weekKey);

		//                        if (weekList.Count > 0)
		//                        {
		//                            ExpandDimensionSize(eProfileType.Week, weekList);

		//                            dataTable = _varData.ChainWeek_Read(basisDetailHierarchyNodeKey, basisDetailVersionKey, weekList, MasterVariableProfileList);
		//                            intLoadDataTableToCube(dataTable, hierarchyNodeKey, versionKey, basisProf, basisDetailProf, basisDetailHierarchyNodeKey, basisDetailVersionKey);
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
		//}

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

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		///// <summary>
		///// Reads all values into the Cube.
		///// </summary>

		//public void ReadAndLoadCube(VersionProfile aVersionProfile, int aHierarchyNodeKey, ProfileList aBasisProfileList)
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
		//                //readLogKey = new BasisReadLogKey(aVersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, basisDetailProfile.Key,
		//                //    basisDetailProfile.HierarchyNodeProfile.Key, basisDetailProfile.VersionProfile.Key);

		//                //if (basisProfile.areDetailsAligned(SAB.ApplicationServerSession))
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
		//                //    SAB,
		//                //    readLogKey,
		//                //    basisDetailProfile.GetAlignedBasisWeekProfileList(SAB.ApplicationServerSession),
		//                //    preWeeks,
		//                //    postWeeks);

		//                //if (basisDetailProfile.VersionProfile.IsBlendedVersion)
		//                //{
		//                //    actualWeekList = new ProfileList(eProfileType.Week);
		//                //    forecastWeekList = new ProfileList(eProfileType.Week);
		//                //    currentWeek = SAB.ApplicationServerSession.Calendar.CurrentWeek;

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
		//                //        blendBasisDetailProfile = basisDetailProfile.Copy(SAB.ApplicationServerSession,true);
		//                //        blendBasisDetailProfile.VersionProfile = (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(basisDetailProfile.VersionProfile.ActualVersionRID);

		//                //        readLogKey = new BasisReadLogKey(blendBasisDetailProfile.VersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, blendBasisDetailProfile.Key,
		//                //            blendBasisDetailProfile.HierarchyNodeProfile.Key, blendBasisDetailProfile.VersionProfile.Key);

		//                //        readWeekList = _planReadLog.DetermineWeeksToRead(
		//                //            SAB,
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
		//                //        blendBasisDetailProfile = basisDetailProfile.Copy(SAB.ApplicationServerSession,true);
		//                //        blendBasisDetailProfile.VersionProfile = (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(basisDetailProfile.VersionProfile.ForecastVersionRID);

		//                //        readLogKey = new BasisReadLogKey(blendBasisDetailProfile.VersionProfile.Key, aHierarchyNodeKey, basisProfile.Key, blendBasisDetailProfile.Key,
		//                //            blendBasisDetailProfile.HierarchyNodeProfile.Key, blendBasisDetailProfile.VersionProfile.Key);

		//                //        readWeekList = _planReadLog.DetermineWeeksToRead(
		//                //            SAB,
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
		//                ReadAndLoadBasisDetail(aVersionProfile, aHierarchyNodeKey, aBasisProfileList, true, basisProfile, basisDetailProfile);
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
		//        readLogKey = new BasisReadLogKey(aVersionProfile.Key, aHierarchyNodeKey, aBasisProfile.Key, aBasisDetailProfile.Key,
		//            aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);

		//        if (aBasisProfile.areDetailsAligned(SAB.ApplicationServerSession))
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
		//            SAB,
		//            readLogKey,
		//            aBasisDetailProfile.GetAlignedBasisWeekProfileList(SAB.ApplicationServerSession),
		//            preWeeks,
		//            postWeeks);

		//        if (aBasisDetailProfile.VersionProfile.IsBlendedVersion && aUseBlended)
		//        {
		//            if (_blendedDetailCube == null)
		//            {
		//                _blendedDetailCube = new ChainBasisDetail(SAB, Transaction, this.PlanCubeGroup, this.CubeDefinition);
		//                Transaction.PlanComputations.PlanCubeInitialization.ChainBasisDetail(_blendedDetailCube, this.PlanCubeGroup.OpenParms.GetDisplayType(SAB.ApplicationServerSession));
		//            }

		//            actualWeekList = new ProfileList(eProfileType.Week);
		//            forecastWeekList = new ProfileList(eProfileType.Week);
		//            currentWeek = SAB.ApplicationServerSession.Calendar.CurrentWeek;

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
		//                ((ChainBasisDetail)_blendedDetailCube).ReadAndLoadBasisDetail(
		//                    (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(aBasisDetailProfile.VersionProfile.ActualVersionRID),
		//                    aHierarchyNodeKey,
		//                    aBasisProfileList,
		//                    false,
		//                    aBasisProfile,
		//                    aBasisDetailProfile);
		//            }

		//            if (forecastWeekList.Count > 0)
		//            {
		//                ((ChainBasisDetail)_blendedDetailCube).ReadAndLoadBasisDetail(
		//                    (VersionProfile)SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(aBasisDetailProfile.VersionProfile.ForecastVersionRID),
		//                    aHierarchyNodeKey,
		//                    aBasisProfileList,
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

		//private void intReadAndLoadDatabaseToCube(int aVersionKey, int aHierarchyNodeKey, ProfileList aReadWeekList,
		//    BasisProfile aBasisProfile, BasisDetailProfile aBasisDetailProfile)
		//{
		//    System.Data.DataTable dataTable;

		//    try
		//    {
		//        dataTable = _varData.ChainWeek_Read(aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key, aReadWeekList, MasterVariableProfileList);
		//        intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisDetailProfile.HierarchyNodeProfile.Key, aBasisDetailProfile.VersionProfile.Key);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>

		public void ReadAndLoadCube(VersionProfile aVersionProfile, int aHierarchyNodeKey, ProfileList aBasisProfileList)
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
						dataTable = _varData.ChainWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.ActualVersionRID, actualWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.ActualVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
					}

					if (forecastWeekList.Count > 0)
					{
						dataTable = _varData.ChainWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.ForecastVersionRID, forecastWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.ForecastVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
					}
				}
				else
				{
					if (aReadWeekList.Count > 0)
					{
						dataTable = _varData.ChainWeek_Read(aBasisHierarchyNodeKey, aBasisVersion.Key, aReadWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aHierarchyNodeKey, aVersionKey, aBasisProfile, aBasisDetailProfile, aBasisHierarchyNodeKey, aBasisVersion.Key, aBasisVersion.Key, false);
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
			int aHierarchyRID,
			int aVersionRID,
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
	}
}
