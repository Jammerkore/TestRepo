using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentDetailCube is an cube used to store Assortment values.
	/// </summary>

	abstract public class AssortmentComponentCube : AssortmentCube
	{
		//=======
		// FIELDS
		//=======

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //private bool _spreadToDetail;

        //End TT#2 - JScott - Assortment Planning - Phase 2
        //=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentDetailCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentDetailCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentDetailCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentDetailCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentDetailCube.
		/// </param>

		public AssortmentComponentCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
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
				return eProfileType.AssortmentDetailVariable;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Gets or sets a flag indicating if this component values are being spread to the detail cubes instead of other sub-total cubes
        ///// </summary>

        //public bool SpreadToDetail
        //{
        //    get
        //    {
        //        return _spreadToDetail;
        //    }
        //    set
        //    {
        //        _spreadToDetail = value;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
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