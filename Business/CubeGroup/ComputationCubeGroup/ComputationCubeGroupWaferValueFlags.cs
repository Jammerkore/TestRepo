using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCubeGroupWaferValueFlags class contains the value flag bit assignment for the valueflags of the PlanCubeGroupWaferInfo class.
	/// </summary>

	public class ComputationCubeGroupWaferValueFlags
	{
		//==========
		// CONSTANTS
		//==========

		private const ushort cNone = 0x0000;

		//=======
		// FIELDS
		//=======

		private ushort _valueFlags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the flags.
		/// </summary>

		public ushort ValueFlags
		{
			get
			{
				return _valueFlags;
			}
			set
			{
				_valueFlags = value;
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
			_valueFlags = cNone;
		}
	}
}
