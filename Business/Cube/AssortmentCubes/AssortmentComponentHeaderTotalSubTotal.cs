using System;
using System.Collections;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentComponentHeaderTotalSubTotal : AssortmentComponentHeaderTotalSubTotalCube
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

		public AssortmentComponentHeaderTotalSubTotal(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity, eCubeType aCubeType, eCubeType aSubTotalCubeType)
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
			// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
			if (Level > 1)		//TT#621-MD - stodd - post assortment gets a "Dimension not defined on cube" error
			{
                if (AssortmentCubeGroup.AssortmentType == eAssortmentType.PostReceipt
                    || AssortmentCubeGroup.AssortmentType == eAssortmentType.GroupAllocation)  // TT#1119 - md -stodd - summary calculations wrong 
				{
					return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, Level - 1);
				}
				else
				{
					// BEGIN TT#657-MD - Stodd - Total % displays as zero for Headers on Post-Receipt Assortment
					//return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, Level - 1);
					return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, Level - 1);
					// END TT#657-MD - Stodd - Total % displays as zero for Headers on Post-Receipt Assortment
				}
			}
			else
			{
				//BEGIN TT#621-MD - stodd - post assortment gets a "Dimension not defined on cube" error
				//return eCubeType.None;
				// BEGIN TT#657-MD - Stodd - Total % displays as zero for Headers on Post-Receipt Assortment
                if (AssortmentCubeGroup.AssortmentType == eAssortmentType.PostReceipt
                    || AssortmentCubeGroup.AssortmentType == eAssortmentType.GroupAllocation)  // TT#1119 - md -stodd - summary calculations wrong 
				{
					return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, 0);
				}
				else
				{
					return SubTotalCubeType;
				}
				// END TT#657-MD - Stodd - Total % displays as zero for Headers on Post-Receipt Assortment
				//END TT#621-MD - stodd - post assortment gets a "Dimension not defined on cube" error
			}
			//return SubTotalCubeType;
			// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
			// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
			//BEGIN TT#621-MD - stodd - post assortment gets a "Dimension not defined on cube" error
			//if (Level == 0)
			//{
			//    if (AssortmentCubeGroup.AssortmentType == eAssortmentType.PostReceipt)
			//    {
			//        //return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, 0);
			//        return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, 0);
			//    }
			//    else
			//    {
					return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, 0);
			//    }
			//}
			//else
			//{
			//    if (AssortmentCubeGroup.AssortmentType == eAssortmentType.PostReceipt)
			//    {
			//        return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, Level - 1);
			//    }
			//    else
			//    {
			//        return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, Level - 1);
			//    }
			//}
			//BEGIN TT#621-MD - stodd - post assortment gets a "Dimension not defined on cube" error
			// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
		}

		/// <summary>
		/// Returns a string describing the given PlanCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the PlanCellReference.
		/// </returns>
		// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		override public eCubeType GetComponentHeaderCubeType()
		{
			if (Level == 0)
			{
				return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, 0);
			}
			else
			{
				return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, Level);
			}
		}

		override public eCubeType GetComponentPlaceholderCubeType()
		{
			if (Level == 0)
			{
				return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, 0);
			}
			else
			{
				return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, Level);
			}
		}
		// END TT#2150 - stodd - totals not showing in main matrix grid

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			AssortmentCellReference AssortCellRef;
			//HierarchyNodeProfile nodeProf;
			//VersionProfile versProf;
			AssortmentTotalVariableProfile totalVarProf;
			QuantityVariableProfile quantityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				totalVarProf = (AssortmentTotalVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentTotalVariable]);
				quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Component Header Detail Group Level" +
					", Total Variable \"" + totalVarProf.VariableName + "\"" +
					", Quanitity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
