using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCubeGroupWaferValueFlags class contains the value flag bit assignment for the valueflags of the PlanCubeGroupWaferInfo class.
	/// </summary>

	public class PlanCubeGroupWaferValueFlags : ComputationCubeGroupWaferValueFlags
	{
		//==========
		// CONSTANTS
		//==========

		private const ushort cCurrent = 0x0001; // Current Value
		private const ushort cPostInit = 0x0002; // PostInit Value
		private const ushort cAdjusted = 0x0004; // Adjusted Value

		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the Current flag.
		/// </summary>

		public bool isCurrent
		{
			get
			{
				try
				{
					return ((ValueFlags & cCurrent) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				if (value)
				{
					ValueFlags = (ushort)(ValueFlags | cCurrent);
				}
				else
				{
					ValueFlags = (ushort)(ValueFlags & ~cCurrent);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the post-init flag.
		/// </summary>

		public bool isPostInit
		{
			get
			{
				try
				{
					return ((ValueFlags & cPostInit) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				if (value)
				{
					ValueFlags = (ushort)(ValueFlags | cPostInit);
				}
				else
				{
					ValueFlags = (ushort)(ValueFlags & ~cPostInit);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Adjusted flag.
		/// </summary>

		public bool isAdjusted
		{
			get
			{
				try
				{
					try
					{
						return ((ValueFlags & cAdjusted) > 0);
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				if (value)
				{
					ValueFlags = (ushort)(ValueFlags | cAdjusted);
				}
				else
				{
					ValueFlags = (ushort)(ValueFlags & ~cAdjusted);
				}
			}
		}

		//========
		// METHODS
		//========
	}
}
