using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationInfoFlags class contains the flag bit assignment for the ComputationInfo class.
	/// </summary>

	[Serializable]
	public struct ComputationInfoFlags
	{
		//=======
		// FIELDS
		//=======

		byte _infoFlags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the flags.
		/// </summary>

		public byte Flags
		{
			get
			{
				return _infoFlags;
			}
			set
			{
				_infoFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the CompChanged flag.
		/// </summary>

		public bool isCompChanged
		{
			get
			{
				try
				{
					return ((_infoFlags & ComputationInfoFlagValues.CompChanged) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_infoFlags = (byte)(_infoFlags | ComputationInfoFlagValues.CompChanged);
					}
					else
					{
						_infoFlags = (byte)(_infoFlags & ~ComputationInfoFlagValues.CompChanged);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the UserChanged flag.
		/// </summary>

		public bool isUserChanged
		{
			get
			{
				try
				{
					return ((_infoFlags & ComputationInfoFlagValues.UserChanged) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_infoFlags = (byte)(_infoFlags | ComputationInfoFlagValues.UserChanged);
					}
					else
					{
						_infoFlags = (byte)(_infoFlags & ~ComputationInfoFlagValues.UserChanged);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the CompLocked flag.
		/// </summary>

		public bool isCompLocked
		{
			get
			{
				try
				{
					return ((_infoFlags & ComputationInfoFlagValues.CompLocked) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_infoFlags = (byte)(_infoFlags | ComputationInfoFlagValues.CompLocked);
					}
					else
					{
						_infoFlags = (byte)(_infoFlags & ~ComputationInfoFlagValues.CompLocked);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//Begin Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Gets or sets the value of the AutoTotalsProcessed flag.
		/// </summary>

		public bool isAutoTotalsProcessed
		{
			get
			{
				try
				{
					return ((_infoFlags & ComputationInfoFlagValues.AutoTotalsProcessed) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_infoFlags = (byte)(_infoFlags | ComputationInfoFlagValues.AutoTotalsProcessed);
					}
					else
					{
						_infoFlags = (byte)(_infoFlags & ~ComputationInfoFlagValues.AutoTotalsProcessed);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #5752 - JScott - Calculation Time
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
				_infoFlags = ComputationInfoFlagValues.None;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
