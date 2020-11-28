using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentCubeGroupWaferInfo class contains information about the parsed CubeWaferCoordinate values.
	/// </summary>

	public class AssortmentCubeGroupWaferInfo : ComputationCubeGroupWaferInfo
	{
		//=======
		// FIELDS
		//=======

		private int _subTotalLevel;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCubeGroupWaferInfo.
		/// </summary>

		public AssortmentCubeGroupWaferInfo()
			: base(new AssortmentCubeGroupWaferCubeFlags(), new AssortmentCubeGroupWaferValueFlags())
		{
			_subTotalLevel = Include.NoRID;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the SubTotalLevel value
		/// </summary>

		public int SubTotalLevel
		{
			get
			{
				return _subTotalLevel;
			}
			set
			{
				_subTotalLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the Total flag.
		/// </summary>

		public bool isTotal
		{
			get
			{
				try
				{
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isTotal;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isTotal = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isAttribute;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isAttribute = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isAttributeSet;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isAttributeSet = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isGrade;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isGrade = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isSummary;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isSummary = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isDetail;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isDetail = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isPlaceholder;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isPlaceholder = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isSubTotal;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isSubTotal = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isPlanning;
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
					((AssortmentCubeGroupWaferCubeFlags)CubeFlags).isPlanning = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		//========
		// METHODS
		//========

		/// <summary>
		/// Private method that examines the contents of a AssortmentWaferCoordinateList and sets cooresponding assortment wafer flags.
		/// </summary>
		/// <param name="aWaferCoordinateList">
		/// The AssortmentWaferCoordinateList that is to be examined.
		/// </param>
		/// <returns>
		/// A AssortmentWaferInfo object containing the Assortment wafer flags.
		/// </returns>

		public override void ProcessWaferCoordinates(CubeWaferCoordinateList aWaferCoordinateList)
		{
			try
			{
				foreach (CubeWaferCoordinate waferCoordinate in aWaferCoordinateList)
				{
					switch (waferCoordinate.WaferCoordinateType)
					{
						case eProfileType.AssortmentSummaryVariable:
							isSummary = true;
							isDetail = false;
							break;

						case eProfileType.AssortmentHeader:
						case eProfileType.PlaceholderHeader:
						case eProfileType.PlanLevel:
						case eProfileType.HeaderPack:
						case eProfileType.HeaderPackColor:
						//case eProfileType.PlaceholderSeq:		// TT#1227 - stodd *REMOVED for TT#1322*
							isDetail = true;
							isSummary = false;
							break;

						case eProfileType.AllocationHeader:
						//case eProfileType.HeaderSeq:			// TT#1227 - stodd *REMOVED for TT#1322*
							isDetail = true;
							isSummary = false;
							break;

						case eProfileType.AssortmentTotalVariable:
							isTotal = true;
							isAttribute = false;
							isAttributeSet = false;
							isGrade = false;
							break;

						//Begin TT#2 - JScott - Assortment Planning - Phase 2
						case eProfileType.Variable:
							isPlanning = true;
							break;

						//End TT#2 - JScott - Assortment Planning - Phase 2
						case eProfileType.StoreGroup:
							isTotal = false;
							isAttribute = true;
							isAttributeSet = false;
							isGrade = false;
							break;

						case eProfileType.StoreGroupLevel:
							isTotal = false;
							isAttribute = false;
							isAttributeSet = true;
							isGrade = false;
							break;

						case eProfileType.StoreGrade:
							isTotal = false;
							isAttribute = false;
							isAttributeSet = false;
							isGrade = true;
							break;

						case eProfileType.Placeholder:
							isPlaceholder = true;
							break;

						case eProfileType.AssortmentSubTotal:
							isSubTotal = true;
							SubTotalLevel = waferCoordinate.Key;
							break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
