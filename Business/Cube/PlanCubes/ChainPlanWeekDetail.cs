using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the ChainPlanWeekDetail.
	/// </summary>
	/// <remarks>
	/// The ChainPlanWeekDetail defines the values for the sum of the time periods for a variable.
	/// </remarks>

	public class ChainPlanWeekDetail : PlanPlanWeekDetailCube
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
		/// Creates a new instance of ChainPlanWeekDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ChainPlanWeekDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ChainPlanWeekDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this ChainPlanWeekDetail is a part of.
		/// </param>

		public ChainPlanWeekDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Chain, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		public override eCubeType CubeType
		{
			get
			{
				return eCubeType.ChainPlanWeekDetail;
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
			return eCubeType.ChainPlanWeekDetail;
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
			return eCubeType.ChainPlanLowLevelTotalWeekDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		override public eCubeType GetStoreDetailCubeType()
		{
			return eCubeType.StorePlanWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		override public eCubeType GetGroupTotalCubeType()
		{
			return eCubeType.StorePlanGroupTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.StorePlanStoreTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.ChainPlanDateTotal;
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
			return eCubeType.ChainPlanWeekDetail;
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
			return eCubeType.ChainBasisWeekDetail;
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

		/// <summary>
		/// Reads the PlanCubeCell from the database.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to read.
		/// </param>

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		////override public void ReadCell(PlanCellReference aPlanCellRef)
		////{
		////    System.Data.DataTable dataTable;
		////    int versionKey;
		////    int hierarchyNodeKey;
		////    int weekKey;
		////    PlanReadLogKey readLogKey;
		////    ProfileList weekList;
		////    VersionProfile versionProf;
		////    PlanCellReference planCellRef;
		////    VariableProfile variableProf;
		////    WeekProfile weekProf = null;

		////    try
		////    {
		////        versionKey = aPlanCellRef[eProfileType.Version];
		////        hierarchyNodeKey = aPlanCellRef[eProfileType.HierarchyNode];
		////        weekKey = aPlanCellRef[eProfileType.Week];

		////        versionProf = GetVersionProfile(aPlanCellRef);

		////        if (versionProf.BlendType == eForecastBlendType.Month)
		////        {
		////            weekProf = Calendar.GetWeek(weekKey);
		////        }

		////        if (versionProf.BlendType == eForecastBlendType.Week &&
		////            ((weekKey < Calendar.CurrentWeek.Key && versionKey != versionProf.ActualVersionRID) ||
		////            (weekKey >= Calendar.CurrentWeek.Key && versionKey != versionProf.ForecastVersionRID)))
		////        {
		////            if (weekKey < Calendar.CurrentWeek.Key && versionKey != versionProf.ActualVersionRID)
		////            {
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		////                planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }
		////            else if (weekKey == Calendar.CurrentWeek.Key && versionKey != versionProf.ForecastVersionRID)
		////            {
		////                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();

		////                if (variableProf.VariableType == eVariableType.BegStock)
		////                {
		////                    planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		////                }
		////                else
		////                {
		////                    planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                }

		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }
		////            else
		////            {
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		////                planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }

		////            aPlanCellRef.isCellLoadedFromDB = true;
		////        }
		////        else if (versionProf.BlendType == eForecastBlendType.Month &&
		////            ((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ActualVersionRID) ||
		////            (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ActualVersionRID) ||
		////            (weekProf.Period.FiscalYear > Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ForecastVersionRID) ||
		////            (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod >= Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ForecastVersionRID)))
		////        {
		////            if ((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear && versionKey != versionProf.ActualVersionRID) ||
		////                (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ActualVersionRID))
		////            {
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		////                planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }
		////            else if (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod == Calendar.CurrentWeek.Period.FiscalPeriod && versionKey != versionProf.ForecastVersionRID)
		////            {
		////                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();

		////                if (variableProf.VariableType == eVariableType.BegStock)
		////                {
		////                    if (versionProf.BlendCurrentByMonth)
		////                    {
		////                        if (weekProf.WeekInPeriod <= Calendar.CurrentWeek.WeekInPeriod)
		////                        {
		////                            planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		////                        }
		////                        else
		////                        {
		////                            planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                        }
		////                    }
		////                    else if (weekProf.WeekInPeriod == 1)
		////                    {
		////                        planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		////                    }
		////                    else
		////                    {
		////                        planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                    }
		////                }
		////                else
		////                {
		////                    planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                }

		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }
		////            else
		////            {
		////                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
		////                planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		////                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		////            }

		////            aPlanCellRef.isCellLoadedFromDB = true;
		////        }
		////        else
		////        {
		////            readLogKey = new PlanReadLogKey(versionKey, hierarchyNodeKey);

		////            if (!_planReadLog.Contains(readLogKey, weekKey))
		////            {
		////                if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)(MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable])), aPlanCellRef) &&
		////                    aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
		////                {
		////                    weekList = _planReadLog.DetermineWeeksToRead(_SAB, readLogKey, weekKey);

		////                    if (weekList.Count > 0)
		////                    {
		////                        ExpandDimensionSize(eProfileType.Week, weekList);

		////                        dataTable = PlanCubeGroup.VarData.ChainWeek_Read(hierarchyNodeKey, versionKey, weekList, MasterVariableProfileList);
		////                        intLoadDataTableToCube(dataTable);
		////                        _planReadLog.Add(readLogKey, weekList);
		////                    }
		////                }
		////                else
		////                {
		////                    aPlanCellRef.isCellLoadedFromDB = true;
		////                }
		////            }
		////            else
		////            {
		////                aPlanCellRef.isCellLoadedFromDB = true;
		////            }
		////        }
		////    }
		////    catch (Exception exc)
		////    {
		////        string message = exc.ToString();
		////        throw;
		////    }
		////}

		//override public void ReadCell(PlanCellReference aPlanCellRef)
		//{
		//    System.Data.DataTable dataTable;
		//    int versionKey;
		//    int hierarchyNodeKey;
		//    int weekKey;
		//    PlanReadLogKey readLogKey;
		//    ProfileList weekList;
		//    VersionProfile versionProf;
		//    PlanCellReference planCellRef;
		//    VariableProfile variableProf;
		//    WeekProfile weekProf = null;

		//    try
		//    {
		//        versionKey = aPlanCellRef[eProfileType.Version];
		//        hierarchyNodeKey = aPlanCellRef[eProfileType.HierarchyNode];
		//        weekKey = aPlanCellRef[eProfileType.Week];

		//        // RonM - begin test debug code  
		//        //variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		//        //if (variableProf.Key == 197)   // Init_Flag 
		//        //{
		//        //    int i = 1;
		//        //}
		//        // RonM - end test debug code 

		//        versionProf = GetVersionProfile(aPlanCellRef);

		//        if (versionProf.BlendType == eForecastBlendType.Month)
		//        {
		//            weekProf = Calendar.GetWeek(weekKey);
		//        }

		//        if (versionProf.BlendType == eForecastBlendType.Week &&
		//            _blendedDetailCube != null)
		//        {
		//            if (weekKey < Calendar.CurrentWeek.Key)
		//            {
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }
		//            else if (weekKey == Calendar.CurrentWeek.Key)
		//            {
		//                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);

		//                if (variableProf.VariableType == eVariableType.BegStock)
		//                {
		//                    planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		//                }
		//                else
		//                {
		//                    planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                }

		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }
		//            else
		//            {
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }

		//            aPlanCellRef.isCellLoadedFromDB = true;
		//        }
		//        else if (versionProf.BlendType == eForecastBlendType.Month &&
		//            _blendedDetailCube != null)
		//        {
		//            if ((weekProf.Period.FiscalYear < Calendar.CurrentWeek.Period.FiscalYear) ||
		//                (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod < Calendar.CurrentWeek.Period.FiscalPeriod))
		//            {
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }
		//            else if (weekProf.Period.FiscalYear == Calendar.CurrentWeek.Period.FiscalYear && weekProf.Period.FiscalPeriod == Calendar.CurrentWeek.Period.FiscalPeriod)
		//            {
		//                variableProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);

		//                if (variableProf.VariableType == eVariableType.BegStock)
		//                {
		//                    if (versionProf.BlendCurrentByMonth)
		//                    {
		//                        if (weekProf.WeekInPeriod <= Calendar.CurrentWeek.WeekInPeriod)
		//                        {
		//                            planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		//                        }
		//                        else
		//                        {
		//                            planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                        }
		//                    }
		//                    else if (weekProf.WeekInPeriod == 1)
		//                    {
		//                        planCellRef[eProfileType.Version] = versionProf.ActualVersionRID;
		//                    }
		//                    else
		//                    {
		//                        planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                    }
		//                }
		//                else
		//                {
		//                    planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                }

		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }
		//            else
		//            {
		//                planCellRef = (PlanCellReference)_blendedDetailCube.CreateCellReference(aPlanCellRef);
		//                planCellRef[eProfileType.Version] = versionProf.ForecastVersionRID;
		//                aPlanCellRef.SetLoadCellValue(planCellRef.GetCellValue(eGetCellMode.Current, true), aPlanCellRef.isCellLocked);
		//                // BEGIN MID Track #6037 - Variable values not getting saved to database
		//                if (!aPlanCellRef.isCellProtected)
		//                {
		//                    aPlanCellRef.isCellChanged = planCellRef.isCellChanged;
		//                }
		//                // END MID Track #6037
		//            }

		//            aPlanCellRef.isCellLoadedFromDB = true;
		//        }
		//        else
		//        {
		//            readLogKey = new PlanReadLogKey(versionKey, hierarchyNodeKey);

		//            if (!_planReadLog.Contains(readLogKey, weekKey))
		//            {
		//                if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)(MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable])), aPlanCellRef) &&
		//                    aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
		//                {
		//                    weekList = _planReadLog.DetermineWeeksToRead(_SAB, readLogKey, weekKey);

		//                    if (weekList.Count > 0)
		//                    {
		//                        ExpandDimensionSize(eProfileType.Week, weekList);

		//                        dataTable = PlanCubeGroup.VarData.ChainWeek_Read(hierarchyNodeKey, versionKey, weekList, MasterVariableProfileList);
		//                        intLoadDataTableToCube(dataTable);
		//                        _planReadLog.Add(readLogKey, weekList);
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
		//// End Track #5904
		override public void ReadCell(PlanCellReference aPlanCellRef)
		{
			VersionProfile versionProf;
			int hierarchyNodeKey;
			int weekKey;
			PlanReadLogKey readLogKey;
			ProfileList weekList;

			try
			{
				versionProf = GetVersionProfile(aPlanCellRef);
				hierarchyNodeKey = aPlanCellRef[eProfileType.HierarchyNode];
				weekKey = aPlanCellRef[eProfileType.Week];

				readLogKey = new PlanReadLogKey(versionProf.Key, hierarchyNodeKey);

				if (!_planReadLog.Contains(readLogKey, weekKey))
				{
					if (aPlanCellRef.PlanCube.isDatabaseVariable((VariableProfile)(MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable])), aPlanCellRef) &&
						aPlanCellRef[eProfileType.QuantityVariable] == CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
					{
						weekList = _planReadLog.DetermineWeeksToRead(_SAB, readLogKey, weekKey);

						if (weekList.Count > 0)
						{
							ExpandDimensionSize(eProfileType.Week, weekList);
							intReadAndLoadDatabaseToCube(versionProf, hierarchyNodeKey, weekList);
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
			try
			{
				foreach (PlanProfile planProf in aPlanProfileList)
				{
					if (hasPlanChanged(planProf))
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
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)CreateCellReference();

				planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				planCellRef[eProfileType.Version] = aPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.HierarchyNode] = aPlanProfile.NodeProfile.Key;

				foreach (WeekProfile weekProf in PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession))
				{
					planCellRef[eProfileType.Week] = weekProf.Key;

					foreach (VariableProfile varProf in MasterVariableProfileList)
					{
						if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
						{
							planCellRef[eProfileType.Variable] = varProf.Key;

							//Begin Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more
							//if (planCellRef.PlanCube.doesCellExist(planCellRef) && planCellRef.isCellChanged)
							if (planCellRef.PlanCube.doesCellExist(planCellRef) && planCellRef.isCellInitialized && planCellRef.isCellChanged)
							//End Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more
							{
								return true;
							}
						}
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

		//Begin Track #5950 - JScott - Save Low Level to High may get warning message
		/// <summary>
		/// Returns true if any cell for the given PlanProfile has changed.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		public bool ClearPlanChanges(PlanProfile aPlanProfile)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)CreateCellReference();

				planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				planCellRef[eProfileType.Version] = aPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.HierarchyNode] = aPlanProfile.NodeProfile.Key;

				foreach (WeekProfile weekProf in PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession))
				{
					planCellRef[eProfileType.Week] = weekProf.Key;

					foreach (VariableProfile varProf in MasterVariableProfileList)
					{
						if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
						{
							planCellRef[eProfileType.Variable] = varProf.Key;

							if (planCellRef.PlanCube.doesCellExist(planCellRef) && planCellRef.isCellChanged)
							{
								planCellRef.isCellChanged = false;
							}
						}
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

		//End Track #5950 - JScott - Save Low Level to High may get warning message
		/// <summary>
		/// Returns a string describing the given PlanCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the PlanCellReference.
		/// </returns>

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			PlanCellReference planCellRef;
			HierarchyNodeProfile nodeProf;
			VersionProfile versProf;
			WeekProfile weekProf;
			QuantityVariableProfile qtyVarProf;
			VariableProfile varProf;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				nodeProf = GetHierarchyNodeProfile(planCellRef);
				versProf = GetVersionProfile(planCellRef);
				weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(planCellRef[eProfileType.Week]);
				qtyVarProf = (QuantityVariableProfile)Transaction.PlanComputations.PlanQuantityVariables.QuantityVariableProfileList.FindKey(planCellRef[eProfileType.QuantityVariable]);
				varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(planCellRef[eProfileType.Variable]);

				return "Chain Plan Week Detail" +
					", Node \"" + nodeProf.Text + "\"" +
					", Version \"" + versProf.Description + "\"" +
					", Week \"" + weekProf.Text() + "\"" +
					", Quantity \"" + qtyVarProf.VariableName + "\"" +
					", Variable \"" + varProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// The ProfileList of PlanProfiles to read.
		/// </param>
		/// <param name="aWeekProfileList">
		/// A ProfileList containing all the Week Profiles to read.
		/// </param>

		public void ReadAndLoadCube(ProfileList aPlanProfileList, ProfileList aWeekProfileList)
		{
			try
			{
				foreach (PlanProfile planProf in aPlanProfileList)
				{
					ReadAndLoadCube(planProf, aWeekProfileList);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		///// <summary>
		///// Reads all values into the Cube.
		///// </summary>
		///// <param name="aPlanProfile">
		///// The PlanProfile to read.
		///// </param>
		///// <param name="aWeekProfileList">
		///// A ProfileList containing all the Week Profiles to read.
		///// </param>

		//public void ReadAndLoadCube(PlanProfile aPlanProfile, ProfileList aWeekProfileList)
		//{
		//    try
		//    {
		//        ReadAndLoadCube(aPlanProfile, aWeekProfileList, true);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//// End Track #5904

		///// <summary>
		///// Reads all values into the Cube.
		///// </summary>
		///// <param name="aPlanProfile">
		///// The PlanProfile to read.
		///// </param>
		///// <param name="aWeekProfileList">
		///// A ProfileList containing all the Week Profiles to read.
		///// </param>

		//// Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//public void ReadAndLoadCube(PlanProfile aPlanProfile, ProfileList aWeekProfileList, bool aUseBlended)
		//// End Track #5904
		//{
		//    ProfileList actualWeekList;
		//    ProfileList forecastWeekList;
		//    WeekProfile currentWeek;
		//    PlanReadLogKey readLogKey;
		//    ProfileList readWeekList;

		//    try
		//    {
		//        if (aPlanProfile.VersionProfile != null && aPlanProfile.NodeProfile != null)
		//        {
		//            readLogKey = new PlanReadLogKey(aPlanProfile.VersionProfile.Key, aPlanProfile.NodeProfile.Key);

		//            readWeekList = _planReadLog.DetermineWeeksToRead(
		//                _SAB,
		//                readLogKey,
		//                aWeekProfileList,
		//                Include.PlanReadPreWeeks,
		//                Include.PlanReadPostWeeks);

		//            // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//            //if (aPlanProfile.VersionProfile.IsBlendedVersion)
		//            if (aPlanProfile.VersionProfile.IsBlendedVersion && aUseBlended)
		//            // End Track #5904
		//            {
		//                // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                _blendedDetailCube = new ChainPlanWeekDetail(SAB, Transaction, this.PlanCubeGroup, this.CubeDefinition, 1, true, true);
		//                Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(_blendedDetailCube, this.PlanCubeGroup.OpenParms.GetDisplayType(SAB.ApplicationServerSession));
		//                // End Track #5904

		//                actualWeekList = new ProfileList(eProfileType.Week);
		//                forecastWeekList = new ProfileList(eProfileType.Week);
		//                currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

		//                foreach (WeekProfile week in readWeekList.ArrayList)
		//                {
		//                    if (aPlanProfile.VersionProfile.BlendType == eForecastBlendType.Month &&
		//                        week.Period.FiscalYear == currentWeek.Period.FiscalYear &&
		//                        week.Period.FiscalPeriod == currentWeek.Period.FiscalPeriod)
		//                    {
		//                        actualWeekList.Add(week);
		//                        forecastWeekList.Add(week);
		//                    }
		//                    else if (week.YearWeek == currentWeek.YearWeek)
		//                    {
		//                        actualWeekList.Add(week);
		//                        forecastWeekList.Add(week);
		//                    }
		//                    else if (week.YearWeek < currentWeek.YearWeek)
		//                    {
		//                        actualWeekList.Add(week);
		//                    }
		//                    else
		//                    {
		//                        forecastWeekList.Add(week);
		//                    }
		//                }

		//                if (actualWeekList.Count > 0)
		//                {
		//                    // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                    //readLogKey = new PlanReadLogKey(aPlanProfile.VersionProfile.ActualVersionRID, aPlanProfile.NodeProfile.Key);

		//                    //readWeekList = _planReadLog.DetermineWeeksToRead(
		//                    //    _SAB,
		//                    //    readLogKey,
		//                    //    actualWeekList,
		//                    //    0,
		//                    //    0);

		//                    //if (readWeekList.Count > 0)
		//                    //{
		//                    //    intReadAndLoadDatabaseToCube(aPlanProfile.VersionProfile.ActualVersionRID, aPlanProfile.NodeProfile.Key, readWeekList);
		//                    //    _planReadLog.Add(readLogKey, readWeekList);
		//                    //    intInitDBCells(aPlanProfile.VersionProfile.ActualVersionRID, aPlanProfile.NodeProfile.Key, readWeekList);
		//                    //}
		//                    ((ChainPlanWeekDetail)_blendedDetailCube).ReadAndLoadCube(new PlanProfile(aPlanProfile.Key, aPlanProfile.NodeProfile, (VersionProfile)Transaction.GetProfileList(eProfileType.Version).FindKey(aPlanProfile.VersionProfile.ActualVersionRID)), actualWeekList, false);
		//                    // End Track #5904
		//                }

		//                if (forecastWeekList.Count > 0)
		//                {
		//                    // Begin Track #5904 – JSmith - Some actual missing from completed month in blended version.
		//                    //readLogKey = new PlanReadLogKey(aPlanProfile.VersionProfile.ForecastVersionRID, aPlanProfile.NodeProfile.Key);

		//                    //readWeekList = _planReadLog.DetermineWeeksToRead(
		//                    //    _SAB,
		//                    //    readLogKey,
		//                    //    forecastWeekList,
		//                    //    0,
		//                    //    0);

		//                    //if (readWeekList.Count > 0)
		//                    //{
		//                    //    intReadAndLoadDatabaseToCube(aPlanProfile.VersionProfile.ForecastVersionRID, aPlanProfile.NodeProfile.Key, readWeekList);
		//                    //    _planReadLog.Add(readLogKey, readWeekList);
		//                    //    intInitDBCells(aPlanProfile.VersionProfile.ForecastVersionRID, aPlanProfile.NodeProfile.Key, readWeekList);
		//                    //}
		//                    ((ChainPlanWeekDetail)_blendedDetailCube).ReadAndLoadCube(new PlanProfile(aPlanProfile.Key, aPlanProfile.NodeProfile, (VersionProfile)Transaction.GetProfileList(eProfileType.Version).FindKey(aPlanProfile.VersionProfile.ForecastVersionRID)), forecastWeekList, false);
		//                    // End Track #5904
		//                }
		//            }
		//            else
		//            {
		//                if (readWeekList.Count > 0)
		//                {
		//                    intReadAndLoadDatabaseToCube(aPlanProfile.VersionProfile.Key, aPlanProfile.NodeProfile.Key, readWeekList);
		//                    _planReadLog.Add(readLogKey, readWeekList);
		//                    //Begin Track #5752 - JScott - Calculation Time
		//                    //intInitDBCells(aPlanProfile.VersionProfile.Key, aPlanProfile.NodeProfile.Key, PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession));
		//                    //End Track #5752 - JScott - Calculation Time
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

		//private void intReadAndLoadDatabaseToCube(int aVersionKey, int aHierarchyNodeKey, ProfileList aReadWeekList)
		//{
		//    System.Data.DataTable dataTable;

		//    try
		//    {
		//        dataTable = PlanCubeGroup.VarData.ChainWeek_Read(aHierarchyNodeKey, aVersionKey, aReadWeekList, MasterVariableProfileList);
		//        intLoadDataTableToCube(dataTable);
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
		/// <param name="aPlanProfile">
		/// The PlanProfile to read.
		/// </param>
		/// <param name="aWeekProfileList">
		/// A ProfileList containing all the Week Profiles to read.
		/// </param>

		public void ReadAndLoadCube(PlanProfile aPlanProfile, ProfileList aWeekProfileList)
		{
			PlanReadLogKey readLogKey;
			ProfileList readWeekList;

			try
			{
				if (aPlanProfile.VersionProfile != null && aPlanProfile.NodeProfile != null)
				{
					readLogKey = new PlanReadLogKey(aPlanProfile.VersionProfile.Key, aPlanProfile.NodeProfile.Key);

					readWeekList = _planReadLog.DetermineWeeksToRead(
						_SAB,
						readLogKey,
						aWeekProfileList,
						Include.PlanReadPreWeeks,
						Include.PlanReadPostWeeks);

					if (readWeekList.Count > 0)
					{
						intReadAndLoadDatabaseToCube(aPlanProfile.VersionProfile, aPlanProfile.NodeProfile.Key, readWeekList);
						_planReadLog.Add(readLogKey, readWeekList);
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
		/// Saves values in the Cube.
		/// </summary>
		/// <param name="aHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data to.
		/// </param>
		/// <param name="aVersionId">
		/// The VersionId of the plan to save the data to.
		/// </param>
		/// <param name="aToWeekProfileList">
		/// The ProfileList of weeks to save the data to.
		/// </param>
		/// <param name="aOnlyChanged">
		/// A boolean indicating if only changed data should be saved.
		/// </param>
		/// <param name="aResetChangeFlags">
		/// A boolean indicating if the cell changed flags should be reset to false.
		/// </param>
		/// <param name="aSaveLocks">
		/// A boolean indicating if the lock flags should be saved.
		/// </param>

		public void SaveCube(
			int aHierarchyNodeId,
			int aVersionId,
			//Begin Enhancement - JScott - Add Balance Low Levels functionality
			bool aSaveLowLevelToHigh,
			//End Enhancement - JScott - Add Balance Low Levels functionality
			ProfileList aToWeekProfileList,
			bool aOnlyChanged,
			//Begin Track #5690 - JScott - Can not save low to high
			//bool aResetChangeFlags,
			//End Track #5690 - JScott - Can not save low to high
			bool aSaveLocks)
		{
			try
			{
				intSaveCube(aHierarchyNodeId,
					aVersionId,
					Include.NoRID,
					Include.NoRID,
					aHierarchyNodeId,
					aVersionId,
					aToWeekProfileList,
					aOnlyChanged,
					false,
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					aSaveLowLevelToHigh,
					//End Enhancement - JScott - Add Balance Low Levels functionality
					//Begin Track #5690 - JScott - Can not save low to high
					//aResetChangeFlags,
					//End Track #5690 - JScott - Can not save low to high
					aSaveLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Saves values in the Cube.
		/// </summary>
		/// <param name="aFromHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data from.
		/// </param>
		/// <param name="aFromVersionId">
		/// The VersionId of the plan to save the data from.
		/// </param>
		/// <param name="aToHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data to.
		/// </param>
		/// <param name="aToVersionId">
		/// The VersionId of the plan to save the data to.
		/// </param>
		/// <param name="aToWeekProfileList">
		/// The ProfileList of weeks to save the data to.
		/// </param>
		/// <param name="aOnlyChanged">
		/// A boolean indicating if only changed data should be saved.
		/// </param>
		/// <param name="aResetChangeFlags">
		/// A boolean indicating if the cell changed flags should be reset to false.
		/// </param>
		/// <param name="aSaveLocks">
		/// A boolean indicating if the lock flags should be saved.
		/// </param>

		public void SaveCube(
			int aFromHierarchyNodeId,
			int aFromVersionId,
			int aToHierarchyNodeId,
			int aToVersionId,
			//Begin Enhancement - JScott - Add Balance Low Levels functionality
			bool aSaveLowLevelToHigh,
			//End Enhancement - JScott - Add Balance Low Levels functionality
			ProfileList aToWeekProfileList,
			bool aOnlyChanged,
			//Begin Track #5690 - JScott - Can not save low to high
			//bool aResetChangeFlags,
			//End Track #5690 - JScott - Can not save low to high
			bool aSaveLocks)
		{
			try
			{
				intSaveCube(aFromHierarchyNodeId,
					aFromVersionId,
					Include.NoRID,
					Include.NoRID,
					aToHierarchyNodeId,
					aToVersionId,
					aToWeekProfileList,
					aOnlyChanged,
					false,
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					aSaveLowLevelToHigh,
					//End Enhancement - JScott - Add Balance Low Levels functionality
					//Begin Track #5690 - JScott - Can not save low to high
					//aResetChangeFlags,
					//End Track #5690 - JScott - Can not save low to high
					aSaveLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Saves values in the Cube.
		/// </summary>
		/// <param name="aHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data to.
		/// </param>
		/// <param name="aVersionId">
		/// The VersionId of the plan to save the data to.
		/// </param>
		/// <param name="aStoreHierarchyNodeId">
		/// The HierarchyNodeId of the store plan to save the data from if aSaveAllStoreAsChain is true.
		/// </param>
		/// <param name="aStoreVersionId">
		/// The VersionId of the store plan to save the data from if aSaveAllStoreAsChain is true.
		/// </param>
		/// <param name="aToWeekProfileList">
		/// The ProfileList of weeks to save the data to.
		/// </param>
		/// <param name="aOnlyChanged">
		/// A boolean indicating if only changed data should be saved.
		/// </param>
		/// <param name="aSaveAllStoreAsChain">
		/// A boolean indicating if the All Store values should be save for the chain.
		/// </param>
		/// <param name="aResetChangeFlags">
		/// A boolean indicating if the cell changed flags should be reset to false.
		/// </param>
		/// <param name="aSaveLocks">
		/// A boolean indicating if the lock flags should be saved.
		/// </param>

		public void SaveCube(
			int aHierarchyNodeId,
			int aVersionId,
			int aStoreHierarchyNodeId,
			int aStoreVersionId,
			ProfileList aToWeekProfileList,
			bool aOnlyChanged,
			bool aSaveAllStoreAsChain,
			//Begin Track #5690 - JScott - Can not save low to high
			//bool aResetChangeFlags,
			//End Track #5690 - JScott - Can not save low to high
			bool aSaveLocks)
		{
			try
			{
				intSaveCube(aHierarchyNodeId,
					aVersionId,
					aStoreHierarchyNodeId,
					aStoreVersionId,
					aHierarchyNodeId,
					aVersionId,
					aToWeekProfileList,
					aOnlyChanged,
					aSaveAllStoreAsChain,
					false,
					//Begin Track #5690 - JScott - Can not save low to high
					//aResetChangeFlags,
					//End Track #5690 - JScott - Can not save low to high
					aSaveLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
			
		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		public void SaveCube(
			int aFromHierarchyNodeId,
			int aFromVersionId,
			int aFromStoreHierarchyNodeId,
			int aFromStoreVersionId,
			int aToHierarchyNodeId,
			int aToVersionId,
			ProfileList aToWeekProfileList,
			bool aOnlyChanged,
			bool aSaveAllStoreAsChain,
            //Begin Track #5690 - JScott - Can not save low to high
            //bool aResetChangeFlags,
            //End Track #5690 - JScott - Can not save low to high
			bool aSaveLocks)
		{
			try
			{
				intSaveCube(aFromHierarchyNodeId,
					aFromVersionId,
					aFromStoreHierarchyNodeId,
					aFromStoreVersionId,
					aToHierarchyNodeId,
					aToVersionId,
					aToWeekProfileList,
					aOnlyChanged,
					aSaveAllStoreAsChain,
					false,
                    //Begin Track #5690 - JScott - Can not save low to high
                    //aResetChangeFlags,
                    //End Track #5690 - JScott - Can not save low to high
					aSaveLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Enhancement - JScott - Add Balance Low Levels functionality
		/// <summary>
		/// Saves values in the Cube.
		/// </summary>
		/// <param name="aFromHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data from.
		/// </param>
		/// <param name="aFromVersionId">
		/// The VersionId of the plan to save the data from.
		/// </param>
		/// <param name="aFromStoreHierarchyNodeId">
		/// The HierarchyNodeId of the store plan to save the data from if aSaveAllStoreAsChain is true.
		/// </param>
		/// <param name="aFromStoreVersionId">
		/// The VersionId of the store plan to save the data from if aSaveAllStoreAsChain is true.
		/// </param>
		/// <param name="aToHierarchyNodeId">
		/// The HierarchyNodeId of the plan to save the data to.
		/// </param>
		/// <param name="aToVersionId">
		/// The VersionId of the plan to save the data to.
		/// </param>
		/// <param name="aToWeekProfileList">
		/// The ProfileList of weeks to save the data to.
		/// </param>
		/// <param name="aOnlyChanged">
		/// A boolean indicating if only changed data should be saved.
		/// </param>
		/// <param name="aSaveAllStoreAsChain">
		/// A boolean indicating if the All Store values should be save for the chain.
		/// </param>
		/// <param name="aResetChangeFlags">
		/// A boolean indicating if the cell changed flags should be reset to false.
		/// </param>
		
		private void intSaveCube(
			int aFromHierarchyNodeId,
			int aFromVersionId,
			int aFromStoreHierarchyNodeId,
			int aFromStoreVersionId,
			int aToHierarchyNodeId,
			int aToVersionId,
			ProfileList aToWeekProfileList,
			bool aOnlyChanged,
			bool aSaveAllStoreAsChain,
			//Begin Enhancement - JScott - Add Balance Low Levels functionality
			bool aSaveLowLevelToHigh,
			//End Enhancement - JScott - Add Balance Low Levels functionality
			//Begin Track #5690 - JScott - Can not save low to high
			//bool aResetChangeFlags,
			//End Track #5690 - JScott - Can not save low to high
			bool aSaveLocks)
		{
			PlanCellReference planCellRef;
			int i;
			WeekProfile fromWeekProf;
			WeekProfile toWeekProf;
			Hashtable valueColHash;
			Hashtable lockColHash;
			int writeCount;
			VersionProfile versionProf;
			bool protectSaveAsHistory;
			PlanCellReference saveAsPlanCellRef;
			ProfileList unprotectedWeekProfileList;
			Hashtable protectedVariableHash;
			bool foundProtected;
			bool foundUnprotected;
			int partialProtectWeekKey;

			try
			{
				PlanCubeGroup.VarData.OpenUpdateConnection();

				try
				{
					protectSaveAsHistory = false;
					protectedVariableHash = new Hashtable();
					partialProtectWeekKey = Include.NoRID;

					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					//if (!aOnlyChanged || aSaveAllStoreAsChain)
					if (!aOnlyChanged || aSaveAllStoreAsChain || aSaveLowLevelToHigh)
					//End Enhancement - JScott - Add Balance Low Levels functionality
					{
						if (aSaveAllStoreAsChain || 
							//Begin Enhancement - JScott - Add Balance Low Levels functionality
							aSaveLowLevelToHigh ||
							//End Enhancement - JScott - Add Balance Low Levels functionality
							aFromHierarchyNodeId != aToHierarchyNodeId ||
							aFromVersionId != aToVersionId)
						{
							versionProf = (VersionProfile)CubeGroup.SAB.ApplicationServerSession.GetProfileListVersion().FindKey(aToVersionId); //TT#1517-MD -jsobek -Store Service Optimization

							if (versionProf.ProtectHistory)
							{
								protectSaveAsHistory = true;

								unprotectedWeekProfileList = new ProfileList(eProfileType.Week);

								saveAsPlanCellRef = (PlanCellReference)CreateCellReference();
								saveAsPlanCellRef[eProfileType.HierarchyNode] = aFromHierarchyNodeId;
								saveAsPlanCellRef[eProfileType.Version] = aToVersionId;

								for (i = 0;
									i < PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession).Count &&
									i < aToWeekProfileList.Count; i++)
								{
									foundProtected = false;
									foundUnprotected = false;

									toWeekProf = (WeekProfile)aToWeekProfileList[i];
									saveAsPlanCellRef[eProfileType.Week] = toWeekProf.Key;

									foreach (VariableProfile varProf in MasterVariableProfileList)
									{
										if (saveAsPlanCellRef.PlanCube.isDatabaseVariable(varProf, saveAsPlanCellRef))
										{
											saveAsPlanCellRef[eProfileType.Variable] = varProf.Key;

											if (isVersionProtected(saveAsPlanCellRef))
											{
												protectedVariableHash.Add(new HashKeyObject(toWeekProf.Key, varProf.Key), null);
												foundProtected = true;
											}
											else
											{
												foundUnprotected = true;
											}
										}
									}

									if (!foundProtected)
									{
										unprotectedWeekProfileList.Add(toWeekProf);
									}

									if (foundProtected && foundUnprotected)
									{
										partialProtectWeekKey = toWeekProf.Key;
									}
								}

								if (unprotectedWeekProfileList.Count > 0)
								{
									PlanCubeGroup.VarData.ChainWeek_Delete(aToHierarchyNodeId, aToVersionId, unprotectedWeekProfileList);
								}
							}
							else
							{
								PlanCubeGroup.VarData.ChainWeek_Delete(aToHierarchyNodeId, aToVersionId, aToWeekProfileList);
							}
						}
						else
						{
							PlanCubeGroup.VarData.ChainWeek_Delete(aToHierarchyNodeId, aToVersionId, aToWeekProfileList);
						}
					}

					PlanCubeGroup.VarData.Variable_XMLInit();
					writeCount = 0;

					valueColHash = new Hashtable();
					lockColHash = new Hashtable();

					if (aSaveAllStoreAsChain)
					{
						planCellRef = (PlanCellReference)CubeGroup.GetCube(eCubeType.StorePlanStoreTotalWeekDetail).CreateCellReference();
						planCellRef[eProfileType.Version] = aFromStoreVersionId;
						planCellRef[eProfileType.HierarchyNode] = aFromStoreHierarchyNodeId;
					}
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					else if (aSaveLowLevelToHigh)
					{
						planCellRef = (PlanCellReference)CubeGroup.GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail).CreateCellReference();
						planCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
					}
					//End Enhancement - JScott - Add Balance Low Levels functionality
					else
					{
						planCellRef = (PlanCellReference)CreateCellReference();
						planCellRef[eProfileType.Version] = aFromVersionId;
						planCellRef[eProfileType.HierarchyNode] = aFromHierarchyNodeId;
					}

					planCellRef[eProfileType.QuantityVariable] = CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

					for (i = 0;
						i < PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession).Count &&
						i < aToWeekProfileList.Count; i++)
					{
						fromWeekProf = (WeekProfile)PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession)[i];
						toWeekProf = (WeekProfile)aToWeekProfileList[i];
						planCellRef[eProfileType.Week] = fromWeekProf.Key;

						valueColHash.Clear();
						lockColHash.Clear();

						foreach (VariableProfile varProf in MasterVariableProfileList)
						{
                            // Begin TT#4060 - JSmith - OTS Forecast Review - File>Save As>  Select STore, Select Chain and Save All Store as Chain.  Receive Error Received_
                            if (aSaveAllStoreAsChain && (varProf.VariableCategory != eVariableCategory.Chain && varProf.VariableCategory != eVariableCategory.Both))
                            {
                                continue;
                            }
                            // End TT#4060 - JSmith - OTS Forecast Review - File>Save As>  Select STore, Select Chain and Save All Store as Chain.  Receive Error Received_
							if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
							{
								if (!protectSaveAsHistory || !protectedVariableHash.Contains(new HashKeyObject(toWeekProf.Key, varProf.Key)))
								{
									planCellRef[eProfileType.Variable] = varProf.Key;

                                    // Begin TT#1869 - JSmith  Save as Chain Plan - View affects outcome
                                    //if (planCellRef.PlanCube.doesCellExist(planCellRef))
                                    if (planCellRef.PlanCube.doesCellExist(planCellRef) || aSaveAllStoreAsChain)
                                    // End TT#1869
									{
										if (aOnlyChanged && !aSaveAllStoreAsChain)
										{
											if (planCellRef.isCellChanged)
											{
												valueColHash.Add(varProf, planCellRef.CurrentCellValue);
												lockColHash.Add(varProf, planCellRef.isCellLocked);
											}
										}
										else
										{
											if (toWeekProf.Key == partialProtectWeekKey || planCellRef.CurrentCellValue != 0)
											{
												valueColHash.Add(varProf, planCellRef.CurrentCellValue);
											}
											if (toWeekProf.Key == partialProtectWeekKey || planCellRef.isCellLocked)
											{
												lockColHash.Add(varProf, planCellRef.isCellLocked);
											}
										}

										//Begin Track #5690 - JScott - Can not save low to high
										//if (aResetChangeFlags)
										//{
										//End Track #5690 - JScott - Can not save low to high
										planCellRef.ClearCellChanges();
										//Begin Track #5690 - JScott - Can not save low to high
										//}
										//End Track #5690 - JScott - Can not save low to high
									}
								}
							}
						}

						if (valueColHash.Count > 0 || lockColHash.Count > 0)
						{
							PlanCubeGroup.VarData.ChainWeek_Update_Insert(
								aToHierarchyNodeId,
								toWeekProf.Key,
								aToVersionId,
								valueColHash,
								lockColHash,
								aSaveLocks);

							writeCount += valueColHash.Count + lockColHash.Count;

							if (writeCount > MIDConnectionString.CommitLimit)
							{
								PlanCubeGroup.VarData.ChainWeek_XMLUpdate(aToVersionId, aSaveLocks);
								PlanCubeGroup.VarData.Variable_XMLInit();
								writeCount = 0;
							}
						}
					}

					if (writeCount > 0)
					{
						PlanCubeGroup.VarData.ChainWeek_XMLUpdate(aToVersionId, aSaveLocks);
					}

					PlanCubeGroup.VarData.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					if (PlanCubeGroup.VarData.ConnectionIsOpen)
					{
						PlanCubeGroup.VarData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		///// <summary>
		///// Private method that loads the values in a given DataTable to the Cube.
		///// </summary>
		///// <param name="aDataTable">
		///// The DataTable that contains the values to load.
		///// </param>

		//private void intLoadDataTableToCube(System.Data.DataTable aDataTable)
		/// <summary>
		/// Private method that loads values to the Cube.
		/// </summary>

		private void intReadAndLoadDatabaseToCube(VersionProfile aVersionProf, int aHierarchyNodeKey, ProfileList aReadWeekList)
		{
			ProfileList actualWeekList;
			ProfileList forecastWeekList;
			WeekProfile currentWeek;
			System.Data.DataTable dataTable;

			try
			{
				if (aVersionProf.IsBlendedVersion)
				{
					actualWeekList = new ProfileList(eProfileType.Week);
					forecastWeekList = new ProfileList(eProfileType.Week);
					currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

					foreach (WeekProfile week in aReadWeekList.ArrayList)
					{
						if (aVersionProf.BlendType == eForecastBlendType.Month &&
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
						dataTable = PlanCubeGroup.VarData.ChainWeek_Read(aHierarchyNodeKey, aVersionProf.ActualVersionRID, actualWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aVersionProf.Key, aVersionProf.ActualVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
					}

					if (forecastWeekList.Count > 0)
					{
						dataTable = PlanCubeGroup.VarData.ChainWeek_Read(aHierarchyNodeKey, aVersionProf.ForecastVersionRID, forecastWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aVersionProf.Key, aVersionProf.ForecastVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
                        // Begin TT#TT#739-MD - JSmith - delete stores
                        if (PlanCubeGroup.OpenParms.IncludeLocks)
                        {
                            dataTable = PlanCubeGroup.VarData.ChainWeekLock_Read(aHierarchyNodeKey, aVersionProf.ForecastVersionRID, forecastWeekList, MasterVariableProfileList);
                            if (dataTable.Rows.Count > 0)
                            {
                                intLoadLocksToCube(dataTable, aVersionProf.Key, aVersionProf.ForecastVersionRID, actualWeekList.Count > 0 && forecastWeekList.Count > 0);
                            }
                        }
                        // End TT#TT#739-MD - JSmith - delete stores
					}
				}
				else
				{
					if (aReadWeekList.Count > 0)
					{
						dataTable = PlanCubeGroup.VarData.ChainWeek_Read(aHierarchyNodeKey, aVersionProf.Key, aReadWeekList, MasterVariableProfileList);
						intLoadDataTableToCube(dataTable, aVersionProf.Key, aVersionProf.Key, false);
                        // Begin TT#TT#739-MD - JSmith - delete stores
                        if (PlanCubeGroup.OpenParms.IncludeLocks &&
                            aVersionProf.Key != Include.FV_ActualRID)
                        {
                            dataTable = PlanCubeGroup.VarData.ChainWeekLock_Read(aHierarchyNodeKey, aVersionProf.Key, aReadWeekList, MasterVariableProfileList);
                            if (dataTable.Rows.Count > 0)
                            {
                                intLoadLocksToCube(dataTable, aVersionProf.Key, aVersionProf.Key, false);
                            }
                        }
                        // End TT#TT#739-MD - JSmith - delete stores
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
		/// Private method that loads the values in a given DataTable to the Cube.
		/// </summary>
		/// <param name="aDataTable">
		/// The DataTable that contains the values to load.
		/// </param>

		private void intLoadDataTableToCube(System.Data.DataTable aDataTable, int aToVersionRID, int aFromVersionRID, bool aCurrentWeek)
		//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
		{
			ProfileList varProfList;
			PlanCellReference planCellRef;

			try
			{
				if (aDataTable.Rows.Count > 0)
				{
					varProfList = Transaction.PlanComputations.PlanVariables.VariableProfileList;

					planCellRef = (PlanCellReference)CreateCellReference();
					planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

					foreach (System.Data.DataRow dataRow in aDataTable.Rows)
					{
						//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
						//planCellRef[eProfileType.Version] = Convert.ToInt32(dataRow["FV_RID"], CultureInfo.CurrentUICulture);
						planCellRef[eProfileType.Version] = aToVersionRID;
						//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
						planCellRef[eProfileType.HierarchyNode] = Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture);
						planCellRef[eProfileType.Week] = Convert.ToInt32(dataRow["TIME_ID"], CultureInfo.CurrentUICulture);

						foreach (VariableProfile varProf in varProfList)
						{
							if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
							{
								planCellRef[eProfileType.Variable] = varProf.Key;
								if (!planCellRef.isCellLoadedFromDB)
								{
									//Begin Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
									if (!aCurrentWeek || planCellRef.GetVersionProfileOfData().Key == aFromVersionRID)
									{
									//End Track #6061 - JScott - Incorrect values when exporting combined version with "pre-init"
                                        // Begin TT#TT#739-MD - JSmith - delete stores
                                        planCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), false);
                                        //if (Convert.ToChar(dataRow[varProf.DatabaseColumnName + "_LOCK"], CultureInfo.CurrentUICulture) == '1')
                                        //{
                                        //    planCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), true);
                                        //}
                                        //else
                                        //{
                                            //planCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), false);
                                        //}
                                            // End TT#TT#739-MD - JSmith - delete stores
										planCellRef.isCellLoadedFromDB = true;
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

        // Begin TT#TT#739-MD - JSmith - delete stores
        /// <summary>
        /// Private method that loads the locks in a given DataTable to the Cube.
        /// </summary>
        /// <param name="aDataTable">
        /// The DataTable that contains the values to load.
        /// </param>

        private void intLoadLocksToCube(System.Data.DataTable aDataTable, int aToVersionRID, int aFromVersionRID, bool aCurrentWeek)
        {
            ProfileList varProfList;
            PlanCellReference planCellRef;

            try
            {
                if (aDataTable.Rows.Count > 0)
                {
                    varProfList = Transaction.PlanComputations.PlanVariables.VariableProfileList;

                    planCellRef = (PlanCellReference)CreateCellReference();
                    planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                    foreach (System.Data.DataRow dataRow in aDataTable.Rows)
                    {
                        planCellRef[eProfileType.Version] = aToVersionRID;
                        planCellRef[eProfileType.HierarchyNode] = Convert.ToInt32(dataRow["HN_RID"], CultureInfo.CurrentUICulture);
                        planCellRef[eProfileType.Week] = Convert.ToInt32(dataRow["TIME_ID"], CultureInfo.CurrentUICulture);

                        foreach (VariableProfile varProf in varProfList)
                        {
                            if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
                            {
                                planCellRef[eProfileType.Variable] = varProf.Key;
                                if (!aCurrentWeek || planCellRef.GetVersionProfileOfData().Key == aFromVersionRID)
                                {
                                    if (Convert.ToChar(dataRow[varProf.DatabaseColumnName + "_LOCK"], CultureInfo.CurrentUICulture) == '1')
                                    {
                                        {
                                            //planCellRef.SetCellLock(true);
                                            planCellRef.SetLoadCellLock(true);
                                        }
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
        // End TT#TT#739-MD - JSmith - delete stores

//Begin Track #5752 - JScott - Calculation Time

//        /// <summary>
//        /// Returns true if any cell for the given PlanProfile has changed.
//        /// </summary>
//        /// <param name="aVersionKey">
//        /// The key of the version to initialize.
//        /// </param>
//        /// <param name="aHierarchyNodeKey">
//        /// The key of the hierarchy node to initialize.
//        /// </param>
//        /// <param name="aWeekList">
//        /// The list of weeks to initialize.
//        /// </param>
//        /// <returns>
//        /// A boolean indicating if any cell has changed.
//        /// </returns>

//        //Begin Track #4457 - JSmith - Add forecast versions
////		public void intInitDBCells(PlanProfile aPlanProfile)
//        public void intInitDBCells(int aVersionKey, int aHierarchyNodeKey, ProfileList aWeekList)
//        //End Track #4457
//        {
//            PlanCellReference planCellRef;
//            CompRuleTableEntry compRule;

//            try
//            {
//                foreach (VariableProfile varProf in MasterVariableProfileList)
//                {
//                    //Begin Track #4637 - JSmith - Split variables by type
////					if (varProf.DatabaseColumnName != null)
////					{
//                    planCellRef = (PlanCellReference)CreateCellReference();
//                    planCellRef[eProfileType.Variable] = varProf.Key;
//                    planCellRef[eProfileType.QuantityVariable] = CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
//                    planCellRef[eProfileType.Version] = aVersionKey;
//                    planCellRef[eProfileType.HierarchyNode] = aHierarchyNodeKey;
//                    if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
//                    {
//                    //End Track #4637

//                        compRule = GetRuleTableEntry(planCellRef);

//                        if (compRule != null && compRule.InitFormulaProfile != null)
//                        {
//                            //Begin Track #4457 - JSmith - Add forecast versions
////							planCellRef[eProfileType.Version] = aPlanProfile.VersionProfile.Key;
////							planCellRef[eProfileType.HierarchyNode] = aPlanProfile.NodeProfile.Key;
////
////							foreach (WeekProfile weekProf in PlanCubeGroup.OpenParms.GetWeekProfileList(CubeGroup.SAB.ApplicationServerSession))
//                            //Begin Track #4637 - JSmith - Split variables by type
////							planCellRef[eProfileType.Version] = aVersionKey;
////							planCellRef[eProfileType.HierarchyNode] = aHierarchyNodeKey;
//                            //End Track #4637

//                            foreach (WeekProfile weekProf in aWeekList)
//                            //End Track #4457
//                            {
//                                planCellRef[eProfileType.Week] = weekProf.Key;

//                                planCellRef.InitCellValue();
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//End Track #5752 - JScott - Calculation Time
	}
}
