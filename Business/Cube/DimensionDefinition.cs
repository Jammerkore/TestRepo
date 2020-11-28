using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// A class that defines the format of a dimension of the cube.
	/// </summary>

	public class DimensionDefinition
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _profileType;
		private int _dimensionSize;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of DimensionDefinition using the given eProfileType and dimension size.
		/// </summary>
		/// <param name="aProfileType">
		/// An enum of type eDimentionType that defines the type of this dimension.
		/// </param>
		/// <param name="aDimensionSize">
		/// An int indicating the size of the dimension.
		/// </param>

		public DimensionDefinition(eProfileType aProfileType, int aDimensionSize)
		{
			_profileType = aProfileType;
			_dimensionSize = aDimensionSize;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eProfileType of this dimension.
		/// </summary>

		public eProfileType ProfileType
		{
			get
			{
				return _profileType;
			}
		}

		/// <summary>
		/// Gets the size of this dimension.
		/// </summary>

		public int DimensionSize
		{
			get
			{
				return _dimensionSize;
			}
			set
			{
				_dimensionSize = value;
			}
		}

		//========
		// METHODS
		//========
	}
}
