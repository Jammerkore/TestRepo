using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCubeGroupWaferInfo class contains information about the parsed CubeWaferCoordinate values.
	/// </summary>

	abstract public class ComputationCubeGroupWaferInfo
	{
		//=======
		// FIELDS
		//=======

		private ComputationCubeGroupWaferCubeFlags _cubeFlags;
		private ComputationCubeGroupWaferValueFlags _valueFlags;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCubeGroupWaferInfo, using the given ComputationCubeGroupWaferCubeFlags and ComputationCubeGroupWaferValueFlags.
		/// </summary>

		public ComputationCubeGroupWaferInfo(ComputationCubeGroupWaferCubeFlags aCubeFlags, ComputationCubeGroupWaferValueFlags aValueFlags)
		{
			_cubeFlags = aCubeFlags;
			_valueFlags = aValueFlags;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets CubeFlags.  CubeFlags contain ComputationCubeGroupWaferCubeFlags that describe the cube being referenced by the coordinate.
		/// </summary>

		protected ComputationCubeGroupWaferCubeFlags CubeFlags
		{
			get
			{
				try
				{
					return _cubeFlags;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets ValueFlags.  CubeFlags contain ComputationCubeGroupWaferValueFlags that describe the value type being referenced by the coordinate.
		/// </summary>

		protected ComputationCubeGroupWaferValueFlags ValueFlags
		{
			get
			{
				try
				{
					return _valueFlags;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the CubeFlags values.
		/// </summary>

		public ushort CubeFlagValues
		{
			get
			{
				try
				{
					return _cubeFlags.CubeFlags;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ValueFlags values.
		/// </summary>

		public ushort ValueFlagValues
		{
			get
			{
				try
				{
					return _valueFlags.ValueFlags;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that examines the contents of a CubeWaferCoordinateList and sets cooresponding cube and value wafer flags.
		/// </summary>
		/// <param name="aWaferCoordinateList">
		/// The CubeWaferCoordinateList that is to be examined.
		/// </param>
		/// <returns>
		/// A ComputationCubeGroupWaferInfo object containing the cube and value wafer flags.
		/// </returns>

		abstract public void ProcessWaferCoordinates(CubeWaferCoordinateList aWaferCoordinateList);
	}
}
