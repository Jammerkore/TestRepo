using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	abstract public class AssortmentComponentPlaceholderTotalSubTotalCube : AssortmentComponentTotalSubTotalCube
	{
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

		public AssortmentComponentPlaceholderTotalSubTotalCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity, eCubeType aCubeType, eCubeType aSubTotalCubeType)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, aCubePriority, aReadOnly, aCheckNodeSecurity, aCubeType, aSubTotalCubeType)
		{
		}

		//===========
		// PROPERTIES
		//===========

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
	}
}
