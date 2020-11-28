using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCubeGroupWaferCubeFlags class contains the flag bit assignment for the ComputationInfo class.
	/// </summary>

	public class ComputationCubeGroupWaferCubeFlags
	{
		//==========
		// CONSTANTS
		//==========

		private const ushort cNone = 0x0000;

		//=======
		// FIELDS
		//=======

		private ushort _cubeFlags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the flags.
		/// </summary>

		public ushort CubeFlags
		{
			get
			{
				return _cubeFlags;
			}
			set
			{
				_cubeFlags = value;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Clears the flags.
		/// </summary>

		public void Clear()
		{
			_cubeFlags = cNone;
		}
	}
}
