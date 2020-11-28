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

	abstract public class AssortmentComponentSubTotalCube : AssortmentComponentCube
	{
		//=======
		// FIELDS
		//=======

		private eCubeType _cubeType;
		private eCubeType _subTotalCubeType;
		private int _level;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentDetailSubTotalCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentDetailSubTotalCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentDetailSubTotalCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentDetailSubTotalCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentDetailSubTotalCube.
		/// </param>

		public AssortmentComponentSubTotalCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity, int aLevel, eCubeType aCubeType, eCubeType aSubTotalCubeType)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, aCubeAttributes, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			try
			{
				_cubeType = aCubeType;
				_subTotalCubeType = aSubTotalCubeType;
				_level = aLevel;
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

		override public eCubeType CubeType
		{
			get
			{
				return _cubeType;
			}
		}

		public eCubeType SubTotalCubeType
		{
			get
			{
				return _subTotalCubeType;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
		}

		//========
		// METHODS
		//========
	}
}