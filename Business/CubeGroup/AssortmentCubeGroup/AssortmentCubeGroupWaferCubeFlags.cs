using System;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentWaferFlags class contains the value flag bit assignment for the flags of the AssortmentWaferInfo class.
	/// </summary>

	public class AssortmentCubeGroupWaferCubeFlags : ComputationCubeGroupWaferCubeFlags
	{
		//==========
		// CONSTANTS
		//==========

		private const ushort cTotal = 0x0001;			// Total Value
		private const ushort cAttribute = 0x0002;		// Attribute Value
		private const ushort cAttributeSet = 0x0004;	// AttributeSet Value
		private const ushort cGrade = 0x0008;			// Grade Value
		private const ushort cSummary = 0x0010;			// Summary Value
		private const ushort cDetail = 0x0020;			// Detail Value
		private const ushort cPlaceholder = 0x0040;		// Placeholder Value
		private const ushort cSubTotal = 0x0080;		// SubTotal Value
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private const ushort cPlanning = 0x0100;		// Planning Value
		//End TT#2 - JScott - Assortment Planning - Phase 2

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
		/// Gets or sets the value of the Total flag.
		/// </summary>

		public bool isTotal
		{
			get
			{
				try
				{
					return ((CubeFlags & cTotal) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cTotal);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cTotal);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Attribute flag.
		/// </summary>

		public bool isAttribute
		{
			get
			{
				try
				{
					return ((CubeFlags & cAttribute) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cAttribute);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cAttribute);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the AttributeSet flag.
		/// </summary>

		public bool isAttributeSet
		{
			get
			{
				try
				{
					return ((CubeFlags & cAttributeSet) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cAttributeSet);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cAttributeSet);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Grade flag.
		/// </summary>

		public bool isGrade
		{
			get
			{
				try
				{
					return ((CubeFlags & cGrade) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cGrade);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cGrade);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Summary flag.
		/// </summary>

		public bool isSummary
		{
			get
			{
				try
				{
					try
					{
						return ((CubeFlags & cSummary) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cSummary);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cSummary);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Detail flag.
		/// </summary>

		public bool isDetail
		{
			get
			{
				try
				{
					try
					{
						return ((CubeFlags & cDetail) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cDetail);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cDetail);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Placeholder flag.
		/// </summary>

		public bool isPlaceholder
		{
			get
			{
				try
				{
					try
					{
						return ((CubeFlags & cPlaceholder) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cPlaceholder);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cPlaceholder);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the SubTotal flag.
		/// </summary>

		public bool isSubTotal
		{
			get
			{
				try
				{
					try
					{
						return ((CubeFlags & cSubTotal) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cSubTotal);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cSubTotal);
				}
			}
		}
		//Begin TT#2 - JScott - Assortment Planning - Phase 2

		/// <summary>
		/// Gets or sets the value of the Planning flag.
		/// </summary>

		public bool isPlanning
		{
			get
			{
				try
				{
					try
					{
						return ((CubeFlags & cPlanning) > 0);
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
					CubeFlags = (ushort)(CubeFlags | cPlanning);
				}
				else
				{
					CubeFlags = (ushort)(CubeFlags & ~cPlanning);
				}
			}
		}
		//End TT#2 - JScott - Assortment Planning - Phase 2
		//========
		// METHODS
		//========
	}
}
