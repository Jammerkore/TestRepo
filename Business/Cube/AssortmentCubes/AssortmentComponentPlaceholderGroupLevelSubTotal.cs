using System;
using System.Collections;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentComponentPlaceholderGroupLevelSubTotal : AssortmentComponentPlaceholderGroupLevelSubTotalCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentCube.
		/// </param>

		public AssortmentComponentPlaceholderGroupLevelSubTotal(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity, eCubeType aCubeType, eCubeType aSubTotalCubeType)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, aCubePriority, aReadOnly, aCheckNodeSecurity, aCubeType, aSubTotalCubeType)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSubTotalCubeType()
		{
			if (Level > 0)
			{
				return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, Level - 1);
			}
			else
			{
				return eCubeType.None;
			}
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
			return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
		}

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		override public eCubeType GetComponentHeaderCubeType()
		{
			// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
			if (Level == 0)
			{
				return new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, 0);
			}
			else
			{
				return new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, Level);
			}
			// END TT#2150 - stodd - totals not showing in main matrix grid
		}

		override public eCubeType GetComponentPlaceholderCubeType()
		{
			// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
			if (Level == 0)
			{
				return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
			}
			else
			{
				return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, Level);
			}
			// END TT#2150 - stodd - totals not showing in main matrix grid
		}
		// END TT#2148 - stodd - Assortment totals do not include header values

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
			AssortmentCellReference AssortCellRef;
			//HierarchyNodeProfile nodeProf;
			//VersionProfile versProf;
			AssortmentDetailVariableProfile detailVarProf;
			QuantityVariableProfile quantityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				detailVarProf = (AssortmentDetailVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentDetailVariable]);
				quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Component Placeholder Detail Group Level SubTotal" +
					", Store Group Level " + AssortCellRef[eProfileType.StoreGroupLevel] +
					", Detail \"" + detailVarProf.VariableName + "\"" +
					", Quantity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
