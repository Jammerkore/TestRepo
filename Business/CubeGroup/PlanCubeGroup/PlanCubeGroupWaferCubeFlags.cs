using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCubeGroupWaferCubeFlags class contains the flag bit assignment for the ComputationInfo class.
	/// </summary>

	public class PlanCubeGroupWaferCubeFlags : ComputationCubeGroupWaferCubeFlags
	{
		//==========
		// CONSTANTS
		//==========

		private const ushort cStore = 0x0001; // Store Cube
		private const ushort cStoreGroupLevel = 0x0002; // Store Group Level Cube
		private const ushort cStoreTotal = 0x0004; // Store Total Cube
		private const ushort cLowLevelTotal = 0x0008; // Low-level Total Cube
		private const ushort cChainPlan = 0x0010; // Chain Detail Cube
		private const ushort cStorePlan = 0x0020; // Store Detail Cube
		private const ushort cDate = 0x0040; // Time Total Cube
		private const ushort cPeriod = 0x0080; // Period Cube
		private const ushort cBasis = 0x0100; // Basis Cube

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
		/// Gets or sets the value of the Store flag.
		/// </summary>

		public bool isStore
		{
			get
			{
				try
				{
					return ((CubeFlags & cStore) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cStore);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cStore);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreGroupLevel flag.
		/// </summary>

		public bool isStoreGroupLevel
		{
			get
			{
				try
				{
					return ((CubeFlags & cStoreGroupLevel) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cStoreGroupLevel);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cStoreGroupLevel);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreTotal flag.
		/// </summary>

		public bool isStoreTotal
		{
			get
			{
				try
				{
					return ((CubeFlags & cStoreTotal) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cStoreTotal);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cStoreTotal);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the LowLevelTotal flag.
		/// </summary>

		public bool isLowLevelTotal
		{
			get
			{
				try
				{
					return ((CubeFlags & cLowLevelTotal) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cLowLevelTotal);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cLowLevelTotal);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the ChainPlan flag.
		/// </summary>

		public bool isChainPlan
		{
			get
			{
				try
				{
					return ((CubeFlags & cChainPlan) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cChainPlan);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cChainPlan);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StorePlan flag.
		/// </summary>

		public bool isStorePlan
		{
			get
			{
				try
				{
					return ((CubeFlags & cStorePlan) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cStorePlan);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cStorePlan);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Date flag.
		/// </summary>

		public bool isDate
		{
			get
			{
				try
				{
					return ((CubeFlags & cDate) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cDate);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cDate);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Period flag.
		/// </summary>

		public bool isPeriod
		{
			get
			{
				try
				{
					return ((CubeFlags & cPeriod) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cPeriod);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cPeriod);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Basis flag.
		/// </summary>

		public bool isBasis
		{
			get
			{
				try
				{
					return ((CubeFlags & cBasis) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cBasis);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cBasis);
				}
			}
		}

		//========
		// METHODS
		//========
	}
}
