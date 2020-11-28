using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentComponentTotal is an cube used to store Assortment values.
	/// </summary>

	abstract public class AssortmentComponentTotalCube : AssortmentCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentComponentTotal, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentComponentTotal is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentComponentTotal is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentComponentTotal is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentComponentTotal.
		/// </param>

		public AssortmentComponentTotalCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, aCubeAttributes, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

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

		//========
		// METHODS
		//========

		/// <summary>
		/// Allows a cube to specify custom initializations for the cube.
		/// </summary>
        /// <param name="aCompCellRef">
		/// The PlanCellReference to initialize.
		/// </param>

		override public void InitCellValue(ComputationCellReference aCompCellRef)
		{
			try
			{
				base.InitCellValue(aCompCellRef);

                // Begin TT#1954-MD - JSmith - Assortment
                //((AssortmentCellReference)aCompCellRef).SumDetailCellBlocked();
                //((AssortmentCellReference)aCompCellRef).SumDetailCellReadOnly();
                //((AssortmentCellReference)aCompCellRef).SumDetailCellDisplayOnly();
                //((AssortmentCellReference)aCompCellRef).SumDetailCellFixed();
                ((AssortmentCellReference)aCompCellRef).SumDetailCellFlags();
                // End TT#1954-MD - JSmith - Assortment
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}