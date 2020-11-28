using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class AssortmentViewVariableInitialization : AllocationVariableInitialization
	{
		override public void InitializeVariables(AssortmentCube aAssrtCube)
		{
			AssortmentViewDetailVariables detVars;
			AssortmentViewSummaryVariables sumVars;
			AssortmentViewTotalVariables totVars;
			AssortmentViewQuantityVariables qtyVars;
			AssortmentViewFormulasAndSpreads formulas;
			AssortmentViewChangeMethods chgMethods;
            bool isGroupAllocation = false;		// TT#4294 - stodd - Average Units in Matrix Enahancement

			try
			{
				detVars = (AssortmentViewDetailVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables;
				sumVars = (AssortmentViewSummaryVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentSummaryVariables;
				totVars = (AssortmentViewTotalVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables;
				qtyVars = (AssortmentViewQuantityVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables;
				formulas = (AssortmentViewFormulasAndSpreads)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.FormulasAndSpreads;
				chgMethods = (AssortmentViewChangeMethods)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.ChangeMethods;
				// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                if (aAssrtCube.AssortmentCubeGroup.AssortmentType == eAssortmentType.GroupAllocation)
                {
                    isGroupAllocation = true;
                }
				// End TT#4294 - stodd - Average Units in Matrix Enahancement

				// Detail Cubes

				if (aAssrtCube.CubeType == eCubeType.AssortmentHeaderColorDetail)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_HeaderColorDetail_TotalUnits, chgMethods.Change_Primary_HeaderColorDetail_TotalUnits, null, null);
					// BEGIN TT#1636 - stodd - Index not recalcing
					//aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_HeaderColorDetail_Index, chgMethods.Change_Primary_HeaderColorDetail_Index, null, null);
					// END TT#1636 - stodd - Index not recalcing
					aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
					aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                }
                else if (aAssrtCube.CubeType == eCubeType.AssortmentHeaderTotalDetail)
				{
                    //aAssrtCube.AddRule(totVars.HeaderUnits, qtyVars.Value, formulas.Init_HeaderTotalDetail_HeaderUnits, null, null, null);	// TT#2148 - hide variable
                }
				else if (aAssrtCube.CubeType == eCubeType.AssortmentPlaceholderColorDetail)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, (FormulaProfile)null, chgMethods.Change_Primary_PlaceholderColorDetail_TotalUnits, null, null);
					// BEGIN TT#1636 - stodd - index not re-calcing
					//aAssrtCube.AddRule(detVars.Index, qtyVars.Value, (FormulaProfile)null, chgMethods.Change_Primary_PlaceholderColorDetail_Index, null, null);
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, (FormulaProfile)null, chgMethods.Change_Primary_PlaceholderColorDetail_AvgUnits, null, null);
					// END TT#1636 - stodd - index not re-calcing
					aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
					aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);

                    if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
                    {
                        aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_Difference, null, null, null);
                    }
                }
				else if (aAssrtCube.CubeType == eCubeType.AssortmentPlaceholderGradeTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, chgMethods.Change_Autototal_InitOnly);
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, chgMethods.Change_Autototal_InitOnly);
				}

				// Summary Cubes

				else if (aAssrtCube.CubeType == eCubeType.AssortmentSummaryGrade)
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(sumVars.AvgUnits, qtyVars.Value, formulas.Init_SummaryGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_SummaryGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_InitOnly);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentSummaryGroupLevel)
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(sumVars.AvgUnits, qtyVars.Value, formulas.Init_SummaryGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_SummaryGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_InitOnly);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentSummaryTotal)
				{
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					//aAssrtCube.AddRule(sumVars.AvgUnits, qtyVars.Value, formulas.Init_SummaryTotal_AvgUnits, null, null, chgMethods.Change_Autototal_InitOnly);
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(sumVars.AvgUnits, qtyVars.Value, formulas.Init_SummaryTotal_AvgUnits, chgMethods.Change_Primary_SummaryTotal_AvgUnits, null, chgMethods.Change_Autototal_InitOnly);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
				}

				// Grade Cubes

				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGrade)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGrade_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, chgMethods.Change_Primary_PctToSet, null, null);  // TT#2148 - hide variable
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					// Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                    // End TT#1498-MD
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.NumStoresAllocated, qtyVars.Value, formulas.Init_ComponentGrade_NumStoresAlloc, null, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGrade_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, chgMethods.Change_Primary_PctToSet, null, null); // TT#2148 - hide variable
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException

                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                    // End TT#1498-MD

					if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
					{
                        // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                        //aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, null, null, null);
                        aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, null, null, chgMethods.Change_Autototal_InitOnly);
                        // End TT#2
						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.Index, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Difference, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2150 - stodd - totals not showing in main matrix grid
					}
				}
				//===================================
				// header total row for grade grid
				//===================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGradeSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentSubTotal_TotalUnits(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    //if (!isGroupAllocation || (isGroupAllocation && aAssrtCube.AssortmentCubeGroup.isHeaderDefined))
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentSubTotal_AvgUnits(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentSubTotal_Index(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, chgMethods.Change_Primary_PctToSet, null, null);// TT#2148 - hide variable
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
                    //BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					// END TT#2148 - stodd - Assortment totals do not include header values


					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level != 0)
					{
						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					}
				}
				//======================================
				// placeholder total row for grade grid
				//======================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGradeSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentSubTotal_TotalUnits(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    // stodd note - 
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentSubTotal_AvgUnits(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentSubTotal_Index(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, chgMethods.Change_Primary_PctToSet, null, null);  // TT#2148 - hide variable
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
                    //BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					// END TT#2148 - stodd - Assortment totals do not include header values
					
					//===============================================
					// placeholder + header total row for grade grid
					//================================================
					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level == 0)
					{
						//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						//aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, null, null, chgMethods.Change_Autototal_InitOnly);
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, chgMethods.Change_Primary_ComponentTotal_Balance, null, chgMethods.Change_Autototal_InitOnly);
						//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						// BEGIN TT#2148 - stodd - Assortment totals do not include header values
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                        if (!isGroupAllocation)
                        {
                            aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
                        }
						// End TT#4294 - stodd - Average Units in Matrix Enahancement
						//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
						//aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
						aAssrtCube.AddRule(detVars.Index, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Total, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2148 - stodd - Assortment totals do not include header values

					}
					else
					{
						if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
						{
							aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentSubTotal_Difference(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
							// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
							aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Difference, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
							aAssrtCube.AddRule(detVars.Index, qtyVars.Difference, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
							aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Difference, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
							// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                            aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Difference, formulas.Init_Total_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
							// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
							// END TT#2150 - stodd - totals not showing in main matrix grid
						}

						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						//==== Used when Placeholder and Total values need to be summed together ==//
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.Index, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Total, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Total, formulas.Init_Total_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2150 - stodd - totals not showing in main matrix grid
						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					}
				}

				// Group Level Cubes

				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevel)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGroupLevel_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					// BEGIN TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
					// END TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					// Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                    // End TT#1498-MD
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.NumStoresAllocated, qtyVars.Value, formulas.Init_ComponentGroupLevel_NumStoresAlloc, null, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevel)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGroupLevel_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					// BEGIN TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
                    aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                    // End TT#1498-MD

					if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
					{
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGradeGroupLevel_Difference, null, chgMethods.Change_Autototal_SpreadLock);
						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.Index, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Difference, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Difference, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2150 - stodd - totals not showing in main matrix grid
					}
				}
				//==============================================
				// header total row for store group level grid
				//==============================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevelSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGroupLevel_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    //if (!isGroupAllocation || (isGroupAllocation && aAssrtCube.AssortmentCubeGroup.isHeaderDefined))
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					// BEGIN TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
					// END TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
                    //BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					// END TT#2148 - stodd - Assortment totals do not include header values


					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level != 0)
					{
						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					}
				}
				//==============================================
				// placeholder total row for store group level grid
				//==============================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevelSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGroupLevel_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    // stodd note - 
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_AvgUnits, chgMethods.Change_Primary_ComponentGradeGroupLevel_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					// BEGIN TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.Index, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_Index, chgMethods.Change_Primary_ComponentGradeGroupLevel_Index, null, chgMethods.Change_Autototal_SpreadLock);
					// END TT#1636 - stodd - index not recalcing
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, chgMethods.Change_Primary_PctToAll, null, null);
                    //BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
                    //aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					// END TT#2148 - stodd - Assortment totals do not include header values

					//============================================================
					// placeholder + header total row for store group level grid
					//============================================================
					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level == 0)
					{
						//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						//aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, null, null, chgMethods.Change_Autototal_InitOnly);
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, chgMethods.Change_Primary_ComponentTotal_Balance, null, chgMethods.Change_Autototal_InitOnly);
						//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						// BEGIN TT#2148 - stodd - Assortment totals do not include header values
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                        if (!isGroupAllocation)
                        {
                            aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
                        }
						// End TT#4294 - stodd - Average Units in Matrix Enahancement
						//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
						//aAssrtCube.AddRule(detVars.UnitRetail, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						//aAssrtCube.AddRule(detVars.UnitCost, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
						aAssrtCube.AddRule(detVars.Index, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Total, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2148 - stodd - Assortment totals do not include header values
					}
					else
					{
						if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
						{
							aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentSubTotal_Difference(((AssortmentComponentSubTotalCube)aAssrtCube).Level), null, chgMethods.Change_Autototal_SpreadLock);
							// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
							aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Difference, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
							aAssrtCube.AddRule(detVars.Index, qtyVars.Difference, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
							aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Difference, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
							// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                            aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Difference, formulas.Init_Total_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
							// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
							// END TT#2150 - stodd - totals not showing in main matrix grid
						}

						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						//==== Used when Placeholder and Total values need to be summed together ==//
						aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						aAssrtCube.AddRule(detVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.Index, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Total, formulas.Init_TotalPctToAll, chgMethods.Change_Primary_Total_Spread, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Total, formulas.Init_Total_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// END TT#2150 - stodd - totals not showing in main matrix grid
						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_ComponentGradeGroupLevel_TotalPct, chgMethods.Change_Primary_Detail_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					}
				}

				// Total Cubes

				//========================================
				// header detail rows of total grid
				//========================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderTotal)
				{
					aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(totVars.HeaderUnits, qtyVars.Value, formulas.Init_ComponentHeaderTotal_HeaderUnits, null, null, null);	// TT#2148 - hide variable
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, chgMethods.Change_Primary_ComponentTotal_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (isGroupAllocation)
                    {
                        aAssrtCube.AddRule(totVars.NumStoresAllocated, qtyVars.Value, formulas.Init_ComponentTotal_NumStoresAlloc, null, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
                    aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_Total_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(tottVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, null, null, null);
                    aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, null, null, null);
                    // End TT#1498-MD
					aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_ComponentTotal_TotalCost, null, null, null);
					aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_ComponentTotal_TotalRetail, null, null, null);
					aAssrtCube.AddRule(totVars.MUPct, qtyVars.Value, formulas.Init_ComponentTotal_MUPct, null, null, null);
					aAssrtCube.AddRule(totVars.Balance, qtyVars.Value, formulas.Init_ComponentHeaderTotal_Balance, null, null, null);	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated -  // TT#2148 - hide variable
					aAssrtCube.AddRule(totVars.Intransit, qtyVars.Value, formulas.Init_ComponentHeaderTotal_Intransit, null, null, null);	// TT#1225 - stodd - Intransit
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					aAssrtCube.AddRule(totVars.OnHand, qtyVars.Value, formulas.Init_ComponentHeaderTotal_OnHand, null, null, null);
                    aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Value, formulas.Init_ComponentHeaderTotal_Reserve, chgMethods.Change_Primary_ComponentTotal_Reserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

					// END TT#2148 - stodd - Assortment totals do not include header values
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //aAssrtCube.AddRule(totVars.Committed, qtyVars.Value, formulas.Init_ComponentHeaderTotal_Committed, null, null, null);	// TT#1224 - stodd - Committed
                    // End TT#1725
				}
				//========================================
				// placeholder detail rows of total grid
				//========================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal)
				{
					aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, chgMethods.Change_Primary_ComponentTotal_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
                    aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_Total_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_ColorDetail_UnitCost, chgMethods.Change_Primary_ComponentTotal_UnitCost, null, null);
                    aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_ColorDetail_UnitRetail, chgMethods.Change_Primary_ComponentTotal_UnitRetail, null, null);
                    // End TT#1498-MD
					aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_ComponentTotal_TotalCost, null, null, null);
					aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_ComponentTotal_TotalRetail, null, null, null);
					aAssrtCube.AddRule(totVars.MUPct, qtyVars.Value, formulas.Init_ComponentTotal_MUPct, null, null, null);
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //aAssrtCube.AddRule(totVars.Committed, qtyVars.Value, formulas.Init_ComponentTotal_Committed, null, null, null);	// TT#1224 - stodd - Committed
                    // End TT#1725
					aAssrtCube.AddRule(totVars.Intransit, qtyVars.Value, formulas.Init_ComponentTotal_Intransit, null, null, null);	// TT#1225 - stodd - Intransit
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					aAssrtCube.AddRule(totVars.OnHand, qtyVars.Value, formulas.Init_ComponentTotal_OnHand, null, null, null);
                    aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Value, formulas.Init_ComponentTotal_Reserve, chgMethods.Change_Primary_ComponentTotal_Reserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					// END TT#2148 - stodd - Assortment totals do not include header values
                    aAssrtCube.AddRule(totVars.Balance, qtyVars.Value, formulas.Init_ComponentTotal_Balance, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 


					if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
					{
						// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
						aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentGradeGroupLevel_Difference, null, chgMethods.Change_Autototal_SpreadLock);
						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Difference, formulas.Init_ComponentTotal_AvgUnits, chgMethods.Change_Primary_ComponentTotal_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
                        aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Difference, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_Total_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
						// END TT#2150 - stodd - totals not showing in main matrix grid
						// END TT#2149 - stodd - Difference total does not equal All Store Set
					}
				}
				//============================================================
				// header total row for total grid
				//============================================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
				{
					aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//aAssrtCube.AddRule(totVars.HeaderUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);	// TT#2148 - hide variable
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    //if (!isGroupAllocation || (isGroupAllocation && aAssrtCube.AssortmentCubeGroup.isHeaderDefined))
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, chgMethods.Change_Primary_ComponentTotal_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					//BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
                    //aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);

					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 

					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);

					//aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_ComponentTotal_TotalCost, null, null, null);
					//aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_ComponentTotal_TotalRetail, null, null, null);
					aAssrtCube.AddRule(totVars.MUPct, qtyVars.Value, formulas.Init_ComponentTotal_MUPct, null, null, null);
                    aAssrtCube.AddRule(totVars.Balance, qtyVars.Value, formulas.Init_SumDetail, null, null, null);	 // TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					aAssrtCube.AddRule(totVars.OnHand, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_Reserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					aAssrtCube.AddRule(totVars.Intransit, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					// END TT#2148 - stodd - Assortment totals do not include header values
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //aAssrtCube.AddRule(totVars.Committed, qtyVars.Value, formulas.Init_ComponentHeaderTotal_Committed, null, null, null);	// TT#1224 - stodd - Committed
                    // End TT#1725
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values


                    aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 




					// END TT#2148 - stodd - Assortment totals do not include header values



					if (((AssortmentComponentTotalSubTotalCube)aAssrtCube).Level != 0)
					{
						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
                        aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_TotalTotal_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
					}
				}
				//============================================================
				// placeholder total row for total grid
				//============================================================
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
				{
					aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_TotalUnits, null, chgMethods.Change_Autototal_SpreadLock);
					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					////Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
					////aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, null);
					//aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, null, null, chgMethods.Change_Autototal_SpreadLock);
					////End TT#1143 - JScott - Total % change receives Nothing to Spread exception
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    // stodd Note - 
                    if (!isGroupAllocation)
                    {
                        aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Value, formulas.Init_ComponentTotal_AvgUnits, chgMethods.Change_Primary_ComponentTotal_AvgUnits, null, chgMethods.Change_Autototal_SpreadLock);
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
					//BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
                    //aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);

					//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
					//aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
					//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 

					//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
					//aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_ComponentTotal_TotalCost, null, null, null);
					//aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_ComponentTotal_TotalRetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);

                    //Begin TT#441 - MD - DOConnell - Total Retail and Total Cost are not correct for style and the totals for Placeholder Header and Total.
                    //aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_AvgDetail, null, null, null);
                    // Begin T#1498-MD - RMatelic - ASST-MU% not calcing on the fly for the Detail section.  Total Section does not calc at all >>> remove MU% from lower totals
                    //aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    //aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Value, formulas.Init_SumDetail, null, null, chgMethods.Change_Autototal_SpreadLock);
                    aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Value, formulas.Init_SumDetail, null, null, chgMethods.Change_Autototal_SpreadLock);
                    // End TT#1498-MD
                    //END TT#441 - MD - DOConnell - Total Retail and Total Cost are not correct for style and the totals for Placeholder Header and Total.
                    // Begin T#1498-MD - RMatelic - ASST-MU% not calcing on the fly for the Detail section.  Total Section does not calc at all >>> remove MU% from lower totals
                    //aAssrtCube.AddRule(totVars.MUPct, qtyVars.Value, formulas.Init_ComponentTotal_MUPct, null, null, null);
                    aAssrtCube.AddRule(totVars.MUPct, qtyVars.Value, formulas.Init_ComponentTotal_MUPct, null, null, chgMethods.Change_Autototal_SpreadLock);
                    // End TT#1498-MD
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //aAssrtCube.AddRule(totVars.Committed, qtyVars.Value, formulas.Init_ComponentTotal_Committed, null, null, null);	// TT#1224 - stodd - Committed
                    // End TT#1725
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					aAssrtCube.AddRule(totVars.Intransit, qtyVars.Value, formulas.Init_SumDetail, null, null, null);	// TT#1225 - stodd - Intransit
					aAssrtCube.AddRule(totVars.OnHand, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
                    aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_Reserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					//aAssrtCube.AddRule(totVars.OnHand, qtyVars.Value, formulas.Init_ComponentTotal_OnHand, null, null, null);
					//aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Value, formulas.Init_ComponentTotal_Reserve, null, null, null);	
					// END TT#2148 - stodd - Assortment totals do not include header values
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, chgMethods.Change_Primary_TotalTotal_TotalPct, null, null);
                    aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					// END TT#2148 - stodd - Assortment totals do not include header values
                    aAssrtCube.AddRule(totVars.Balance, qtyVars.Value, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_Balance, null, chgMethods.Change_Autototal_SpreadLock);		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 


					//============================================================
					// placeholder + header total row for total grid
					//============================================================
					if (((AssortmentComponentTotalSubTotalCube)aAssrtCube).Level == 0)
					{
						//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						//aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, null, null, chgMethods.Change_Autototal_InitOnly);
						aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Balance, formulas.Init_Balance, chgMethods.Change_Primary_ComponentTotal_Balance, null, chgMethods.Change_Autototal_InitOnly);
						//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
						//aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						// BEGIN TT#2148 - stodd - Assortment totals do not include header values
						aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);
						// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                        if (!isGroupAllocation)
                        {
                            aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
                        }
						// End TT#4294 - stodd - Average Units in Matrix Enahancement
                        //BEGIN TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
						//aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
                        //aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);

						//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
						// below calcs unit cost as total unit cost / number of details (placeholders or headers)
                        //aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Total, formulas.Init_AvgDetail, null, null, chgMethods.Change_Autototal_SpreadLock);
                        //aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Total, formulas.Init_AvgDetail, null, null, chgMethods.Change_Autototal_SpreadLock);
						// Below calcs unit cost as total cost / total units
						//aAssrtCube.AddRule(totVars.UnitRetail, qtyVars.Total, formulas.Init_ComponentTotal_TotalAvgTotalUnitRetail, null, null, chgMethods.Change_Autototal_SpreadLock);
						//aAssrtCube.AddRule(totVars.UnitCost, qtyVars.Total, formulas.Init_ComponentTotal_TotalAvgTotalUnitCost, null, null, chgMethods.Change_Autototal_SpreadLock);
						//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 

						//END TT#97 - MD - DOConnell - Should not sum unit retail or unit cost when colors are added to the placeholder / style 
						aAssrtCube.AddRule(totVars.TotalRetail, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(totVars.TotalCost, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						//aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);	// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
						aAssrtCube.AddRule(totVars.OnHand, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(totVars.Intransit, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
						aAssrtCube.AddRule(totVars.OnHand, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
                        aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_TotalReserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
                        // Begin T#1498-MD  -ASST-MU% not calcing on the fly for the Detail section.  Total Section does not calc at all >>> remove MU% from lower totals
                        //aAssrtCube.AddRule(totVars.MUPct, qtyVars.Total, formulas.Init_ComponentTotal_MUPct, null, null, null);
                        aAssrtCube.AddRule(totVars.MUPct, qtyVars.Total, formulas.Init_ComponentTotal_MUPct, null, null, chgMethods.Change_Autototal_SpreadLock);
                        // End TT#1498-MD
                        aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
                        aAssrtCube.AddRule(totVars.Balance, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated -  

						// END TT#2148 - stodd - Assortment totals do not include header values
					}
					else
					{
						if (aAssrtCube.AssortmentCubeGroup.isHeaderDefined)
						{
							// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
							aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Difference, formulas.Init_SumDetail, chgMethods.Change_Primary_ComponentTotal_Difference, null, chgMethods.Change_Autototal_SpreadLock);
							// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
							aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Difference, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);

							// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
							//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Difference, formulas.Init_Total_TotalPct, null, null, null);
                            aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Difference, formulas.Init_ComponentTotal_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);	// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
							// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
							aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Difference, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);
							// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
							// END TT#2149 - stodd - Difference total does not equal All Store Set
                            aAssrtCube.AddRule(totVars.Balance, qtyVars.Difference, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 

						}
						// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
						//==== Used when Placeholder and Total values need to be summed together ==//
						aAssrtCube.AddRule(totVars.TotalUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_Total, null, chgMethods.Change_Autototal_InitOnly);

                        aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Total, formulas.Init_Total_TotalPct, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

                        aAssrtCube.AddRule(totVars.ReserveUnits, qtyVars.Total, formulas.Init_Total, chgMethods.Change_Primary_ComponentTotal_TotalReserve, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
						aAssrtCube.AddRule(totVars.AvgUnits, qtyVars.Total, formulas.Init_AvgUnits, chgMethods.Change_Primary_Total_Spread, null, chgMethods.Change_Autototal_SpreadLock);
						// END TT#2150 - stodd - totals not showing in main matrix grid

                        aAssrtCube.AddRule(totVars.Balance, qtyVars.Total, formulas.Init_Total, null, null, chgMethods.Change_Autototal_SpreadLock);		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated -  



						//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
						//aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_TotalPct, null, null);
						// BEGIN TT#2148 - stodd - Assortment totals do not include header values
                        aAssrtCube.AddRule(totVars.TotalPct, qtyVars.Value, formulas.Init_ComponentTotal_TotalPct, chgMethods.Change_Primary_Total_TotalPct, null, chgMethods.Change_Autototal_SpreadLock);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
						// END TT#2148 - stodd - Assortment totals do not include header values
						//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
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
