using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// A class that defines the format of a cube.
	/// </summary>

	public class CubeDefinition
	{
		//=======
		// FIELDS
		//=======

		private int _numDimensions;
		private DimensionDefinition[] _dimensionDefinitions;
		private System.Collections.Hashtable _profileTypeXRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeDefinition with the given array of DimensionDefinitions.
		/// </summary>
		/// <param name="aDimensionDefinitions">
		/// An array of DimensionDefinition entries that defines the dimensions of the cube.
		/// </param>

		public CubeDefinition(params DimensionDefinition[] aDimensionDefinitions)
		{
			int i;

			try
			{
				_numDimensions = aDimensionDefinitions.Length;
				_dimensionDefinitions = new DimensionDefinition[_numDimensions];
				System.Array.Copy(aDimensionDefinitions, _dimensionDefinitions, aDimensionDefinitions.Length);

				_profileTypeXRef = new System.Collections.Hashtable();
				for (i = 0; i < _numDimensions; i++)
				{
					_profileTypeXRef.Add(aDimensionDefinitions[i].ProfileType, i);
				}
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
		/// Indexer that gets the DimensionDefinition for the given dimension index.
		/// </summary>

		public DimensionDefinition this[int aIndex]
		{
			get
			{
				try
				{
					return _dimensionDefinitions[aIndex];
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the number of dimension defined in this cube.
		/// </summary>

		public int NumDimensions
		{
			get
			{
				return _numDimensions;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the dimension index for a given eProfileType.
		/// </summary>
		/// <remarks>
		/// This method can handle a lookup error in two ways: 1) Returning -1 if aThrowError is false; 2) Throw an exception if
		/// aThrowError is true.
		/// </remarks>
		/// <param name="aProfileType">
		/// The eProfileType to find the index for.
		/// </param>
		/// <returns>
		/// The dimension index of the eProfileType.
		/// </returns>

		public int GetDimensionProfileTypeIndex(eProfileType aProfileType)
		{
			object obj;

			try
			{
				obj = _profileTypeXRef[aProfileType];

				if (obj == null)
				{
					return -1;
				}

				return (int)obj;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a value indicating whether an instance of CubeDefinition is equal to a specified object.
		/// </summary>
		/// <remarks>
		/// In order for two CubeDefinition instances to be equal, they must have the same number of dimensions, and
		/// the dimension types of each dimension must be the same.
		/// </remarks>
		/// <param name="aObject">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of CubeDefinition and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		override public bool Equals(object aObject)
		{
			int i;

			try
			{
				if (((CubeDefinition)aObject)._numDimensions != _numDimensions)
				{
					return false;
				}

				for (i = 0; i < _numDimensions; i++)
				{
					if (((CubeDefinition)aObject)._dimensionDefinitions[i].ProfileType != _dimensionDefinitions[i].ProfileType)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
		override public int GetHashCode()
		{
			try
			{
				return base.GetHashCode();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
