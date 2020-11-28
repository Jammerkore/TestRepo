using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentPlaceholderTotalDetail : AssortmentCube
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

		public AssortmentPlaceholderTotalDetail(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, 0, aCubePriority, aReadOnly, aCheckNodeSecurity)
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
				return eCubeType.AssortmentPlaceholderTotalDetail;
			}
		}

		/// <summary>
		/// Abstract property returns the eProfileType for the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public eProfileType VariableProfileType
		{
			get
			{
				return eProfileType.AssortmentTotalVariable;
			}
		}

		/// <summary>
		/// Returns the eProfileType of the Header dimension
		/// </summary>

		public override eProfileType HeaderProfileType
		{
			get
			{
				return eProfileType.PlaceholderHeader;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that initializes the Cube.
		/// </summary>

		override public void InitializeCube()
		{
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Abstract method that returns the eCubeType of the corresponding Pack cube for this cube.
        ///// </summary>
        ///// <returns>
        ///// The eCubeType of the Sub-total cube.
        ///// </returns>

        //override public eCubeType GetDetailPackCubeType()
        //{
        //    return eCubeType.AssortmentPlaceholderPackDetail;
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Color cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetDetailColorCubeType()
		{
			return eCubeType.AssortmentPlaceholderColorDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Level Total cube.
		/// </returns>

		override public eCubeType GetComponentGroupLevelCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Total cube.
		/// </returns>

		override public eCubeType GetComponentTotalCubeType()
		{
			return eCubeType.AssortmentComponentPlaceholderTotal;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Placeholder cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentPlaceholderCubeType()
		{
			return eCubeType.AssortmentPlaceholderColorDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentHeaderCubeType()
		{
			return eCubeType.AssortmentHeaderColorDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the summary cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSummaryCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSubTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (ComputationVariableProfile)MasterAssortmentTotalVariableProfileList.FindKey(aCompCellRef[eProfileType.AssortmentTotalVariable]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A AssortmentCellReference object that identifies the AssortmentCubeCell to read.
		/// </param>

		override public void ReadCell(AssortmentCellReference aAssrtCellRef)
		{
		}

		/// <summary>
		/// Allows a cube to specify custom initializations for the Hidden flag.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to initialize.
		/// </param>

		override public void InitCellValue(ComputationCellReference aCompCellRef)
		{
			AssortmentCellReference asrtCellRef;

			try
			{
				base.InitCellValue(aCompCellRef);

				asrtCellRef = (AssortmentCellReference)aCompCellRef;
				asrtCellRef.isCellFixed = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			AssortmentCellReference AssortCellRef;
			//HierarchyNodeProfile nodeProf;
			//VersionProfile versProf;
			AssortmentTotalVariableProfile totalVarProf;
			QuantityVariableProfile quatityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				totalVarProf = (AssortmentTotalVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentTotalVariable]);
				quatityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Placeholder Total Detail" +
					", Header \"" + AssortCellRef[eProfileType.PlaceholderHeader] +
					", HeaderPack \"" + AssortCellRef[eProfileType.HeaderPack] +
					", Header Pack Color " + AssortCellRef[eProfileType.HeaderPackColor] +
					", Total \"" + totalVarProf.VariableName + "\"" +
					", Quantity Variable \"" + quatityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
