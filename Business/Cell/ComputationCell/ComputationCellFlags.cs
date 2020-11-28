using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCellFlags class contains the flag bit assignment for the ComputationCell class.
	/// </summary>

	[Serializable]
	public struct ComputationCellFlags
	{
		//=======
		// FIELDS
		//=======

		private ushort _cellFlags;

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the Flags
		/// </summary>

		public ushort Flags
		{
			get
			{
				return _cellFlags;
			}
			set
			{
				_cellFlags = value;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Clears all flags.
		/// </summary>

		public void Clear()
		{
			try
			{
				_cellFlags = 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
