using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCubeGroupWaferInfo class contains information about the parsed CubeWaferCoordinate values.
	/// </summary>

	public class PlanCubeGroupWaferInfo : ComputationCubeGroupWaferInfo
	{
		//=======
		// FIELDS
		//=======

		private int _varRID;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCubeGroupWaferInfo.
		/// </summary>

		public PlanCubeGroupWaferInfo()
			: base(new PlanCubeGroupWaferCubeFlags(), new PlanCubeGroupWaferValueFlags())
		{
			_varRID = Include.NoRID;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the VariableProfile.
		/// </summary>

		public int VariableRID
		{
			get
			{
				return _varRID;
			}
			set
			{
				_varRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the Store flag.
		/// </summary>

		public bool isStore
		{
			get
			{
				try
				{
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isStore;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isStore = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isStoreGroupLevel;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isStoreGroupLevel = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isStoreTotal;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isStoreTotal = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isLowLevelTotal;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isLowLevelTotal = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isChainPlan;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isChainPlan = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isStorePlan;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isStorePlan = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isDate;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isDate = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isPeriod;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isPeriod = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferCubeFlags)CubeFlags).isBasis;
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
					((PlanCubeGroupWaferCubeFlags)CubeFlags).isBasis = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Current flag.
		/// </summary>

		public bool isCurrent
		{
			get
			{
				try
				{
					return ((PlanCubeGroupWaferValueFlags)ValueFlags).isCurrent;
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
					((PlanCubeGroupWaferValueFlags)ValueFlags).isCurrent = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferValueFlags)ValueFlags).isPostInit;
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
					((PlanCubeGroupWaferValueFlags)ValueFlags).isPostInit = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
					return ((PlanCubeGroupWaferValueFlags)ValueFlags).isAdjusted;
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
					((PlanCubeGroupWaferValueFlags)ValueFlags).isAdjusted = value;
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
		/// Private method that examines the contents of a CubeWaferCoordinateList and sets cooresponding cube and value wafer flags.
		/// </summary>
		/// <param name="aWaferCoordinateList">
		/// The CubeWaferCoordinateList that is to be examined.
		/// </param>
		/// <returns>
		/// A ComputationCubeGroupWaferInfo object containing the cube and value wafer flags.
		/// </returns>

		override public void ProcessWaferCoordinates(CubeWaferCoordinateList aWaferCoordinateList)
		{
			try
			{
				foreach (CubeWaferCoordinate waferCoordinate in aWaferCoordinateList)
				{
					switch (waferCoordinate.WaferCoordinateType)
					{
						case eProfileType.Store:
							isStore = true;
							break;

						case eProfileType.StoreGroupLevel:
							isStoreGroupLevel = true;
							break;

						case eProfileType.StoreTotal:
							isStoreTotal = true;
							break;

						case eProfileType.LowLevelTotal:
							isLowLevelTotal = true;
							break;

						case eProfileType.TimeTotalVariable:
						case eProfileType.ChainTimeTotalIndex:
						case eProfileType.StoreTimeTotalIndex:
						case eProfileType.DateRange:
							isDate = true;
							break;

						case eProfileType.Period:
							isPeriod = true;
							break;

						case eProfileType.ChainPlan:
							isChainPlan = true;
							break;

						case eProfileType.StorePlan:
							isStorePlan = true;
							break;

						case eProfileType.Basis:
							isBasis = true;
							break;

						case eProfileType.PlanValue:
							switch (waferCoordinate.Key)
							{
								case (int)ePlanValueType.Current:
									isCurrent = true;
									break;
								case (int)ePlanValueType.PostInit:
									isPostInit = true;
									break;
								case (int)ePlanValueType.Adjusted:
									isAdjusted = true;
									break;
							}
							break;

						case eProfileType.Variable:
							VariableRID = waferCoordinate.Key;
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
