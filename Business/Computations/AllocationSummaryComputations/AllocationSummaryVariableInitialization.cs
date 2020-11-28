using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class AllocationSummaryVariableInitialization : AllocationVariableInitialization
	{
		override public void InitializeVariables(AssortmentCube aAssrtCube)
		{
			AllocationSummaryDetailVariables detVars;
			AllocationSummaryQuantityVariables qtyVars;
			AllocationSummaryFormulasAndSpreads formulas;

			try
			{
				detVars = (AllocationSummaryDetailVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables;
				qtyVars = (AllocationSummaryQuantityVariables)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables;
				formulas = (AllocationSummaryFormulasAndSpreads)aAssrtCube.AssortmentCubeGroup.AssortmentComputations.FormulasAndSpreads;

				if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGradeSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);

					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level != 0)
					{
						aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
					}
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGrade)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToSet, qtyVars.Value, formulas.Init_PctToSet, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevelSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);

					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level > 0)
					{
						aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
					}
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevel)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevel)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalUnitsPctToAll, qtyVars.Value, formulas.Init_PctToAll, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);

					if (((AssortmentComponentSubTotalCube)aAssrtCube).Level > 0)
					{
						aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
					}
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentHeaderTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_SumDetail, null, null, null);
					aAssrtCube.AddRule(detVars.TotalPct, qtyVars.Value, formulas.Init_TotalPct, null, null, null);
				}
				else if (aAssrtCube.CubeType == eCubeType.AssortmentHeaderColorDetail)
				{
					aAssrtCube.AddRule(detVars.TotalUnits, qtyVars.Value, formulas.Init_HeaderDetailUnits, null, null, null);
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
