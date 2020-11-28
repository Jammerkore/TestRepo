using System;
using System.Collections;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Class that defines the Component Variables for AllocationSummary.
	/// </summary>

	[Serializable]
	public class AllocationSummaryComponentVariables : AssortmentComponentVariables
	{
		//=======
		// FIELDS
		//=======

		public AssortmentComponentVariableProfile Style = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Style, eProfileType.Style, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Style), "STYLE", true, false);
		public AssortmentComponentVariableProfile PlanLevel = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.PlanLevel, eProfileType.PlanLevel, MIDText.GetTextOnly((int)eAssortmentComponentVariables.PlanLevel), "PLANLEVEL", false, false);
		public AssortmentComponentVariableProfile HeaderID = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.HeaderID, eProfileType.AllocationHeader, MIDText.GetTextOnly((int)eAssortmentComponentVariables.HeaderID), "HEADER", true, true);
		public AssortmentComponentVariableProfile Pack = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Pack, eProfileType.HeaderPack, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Pack), "PACK", true, false);
		public AssortmentComponentVariableProfile Color = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Color, eProfileType.HeaderPackColor, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Color), "COLOR", true, false);
		// Begin TT#1227 - stodd - sort seq
		public AssortmentComponentVariableProfile PlaceholderSeq = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.PlaceholderSeq, eProfileType.PlaceholderSeq, MIDText.GetTextOnly((int)eAssortmentComponentVariables.PlaceholderSeq), "PLACEHOLDERSEQ", false, false);
		public AssortmentComponentVariableProfile HeaderSeq = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.HeaderSeq, eProfileType.HeaderSeq, MIDText.GetTextOnly((int)eAssortmentComponentVariables.HeaderSeq), "HEADERSEQ", false, false);
		// End TT#1227 - stodd - sort seq
		// Begin TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab
        public AssortmentComponentVariableProfile ColorSeq = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.ColorSeq, eProfileType.ColorSeq, MIDText.GetTextOnly((int)eAssortmentComponentVariables.ColorSeq), "COLORSEQ", false, false);
		// End TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationSummaryComponentVariables.
		/// </summary>

		public AllocationSummaryComponentVariables()
			: base(eProfileType.AssortmentComponentVariable)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AllocationSummaryComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(Style);
				_profileList.Add(PlanLevel);
				_profileList.Add(HeaderID);
				_profileList.Add(Pack);
				_profileList.Add(Color);

				// System code -- no modifications beyond this point

				foreach (AssortmentComponentVariableProfile varProf in _profileList)
				{
					_nameHash.Add(varProf.VariableName, varProf);
					_profTypeHash.Add(varProf.ProfileListType, varProf);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns>
		/// A copy of this object.
		/// </returns>

		override public AssortmentComponentVariables Copy()
		{
			try
			{
				return (AssortmentComponentVariables)CopyTo(new AllocationSummaryComponentVariables());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the Summary Variables for AllocationSummary.
	/// </summary>

	[Serializable]
	public class AllocationSummarySummaryVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		public AssortmentSummaryVariableProfile OnHand = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.OnHand, MIDText.GetTextOnly((int)eAllocationWaferVariable.OnHand), "ONHAND", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile Intransit = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.InTransit, MIDText.GetTextOnly((int)eAllocationWaferVariable.InTransit), "INTRANSIT", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
        // Begin TT#1725 - RMatelic - "Hide" Committed
        // Begin TT#1224 - stodd - committed
        //public AssortmentSummaryVariableProfile Committed = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.Committed, MIDText.GetTextOnly((int)eAllocationWaferVariable.Committed), "COMMITTED", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#1224 - stodd - committed
        // End TT#1725
		public AssortmentSummaryVariableProfile OpenToShip = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.OpenToShip, MIDText.GetTextOnly((int)eAllocationWaferVariable.OpenToShip), "OPEN_TO_SHIP", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile QtyAllocated = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.QuantityAllocated, MIDText.GetTextOnly((int)eAllocationWaferVariable.QuantityAllocated), "QTY_ALLOCATED", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile OTSVariance = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.OTSVariance, MIDText.GetTextOnly((int)eAllocationWaferVariable.OTSVariance), "OTS_VARIANCE", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile StoreCount = new AssortmentSummaryVariableProfile((int)eAllocationWaferVariable.StoreCount, MIDText.GetTextOnly((int)eAllocationWaferVariable.StoreCount), "STORE_COUNT", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationSummaryComponentVariables.
		/// </summary>

		public AllocationSummarySummaryVariables()
			: base(eProfileType.AssortmentSummaryVariable)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AllocationSummaryComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(OnHand);
				_profileList.Add(Intransit);
                // Begin TT#1725 - RMatelic - "Hide" Committed
                //_profileList.Add(Committed);	// TT#1224 - stodd - add committed
                // End TT#1725
				_profileList.Add(OpenToShip);
				_profileList.Add(QtyAllocated);
				_profileList.Add(OTSVariance);
				_profileList.Add(StoreCount);

				// System code -- no modifications beyond this point

				foreach (AssortmentSummaryVariableProfile varProf in _profileList)
				{
					_nameHash.Add(varProf.VariableName, varProf);
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
	/// Class that defines the Total Variables for Assortment.
	/// </summary>

	[Serializable]
	public class AllocationSummaryTotalVariables : AssortmentVariables
	{

		//=======
		// FIELDS
		//=======

		public AssortmentTotalVariableProfile TotalPct = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalPct, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile TotalUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile ReserveUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.ReserveUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.ReserveUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile AvgUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.AvgUnits), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile UnitRetail = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.UnitRetail, MIDText.GetTextOnly((int)eAssortmentTotalVariables.UnitRetail), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile UnitCost = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.UnitCost, MIDText.GetTextOnly((int)eAssortmentTotalVariables.UnitCost), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile MUPct = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.MUPct, MIDText.GetTextOnly((int)eAssortmentTotalVariables.MUPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile Balance = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Balance, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Balance), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile OnHand = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.OnHand, MIDText.GetTextOnly((int)eAssortmentTotalVariables.OnHand), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile Intransit = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Intransit, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Intransit), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationSummaryComponentVariables.
		/// </summary>

		public AllocationSummaryTotalVariables()
			: base(eProfileType.AssortmentTotalVariable)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AllocationSummaryComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(TotalPct);
				_profileList.Add(TotalUnits);
				_profileList.Add(ReserveUnits);
				_profileList.Add(AvgUnits);
				_profileList.Add(UnitRetail);
				_profileList.Add(UnitCost);
				_profileList.Add(MUPct);
				_profileList.Add(Balance);
				_profileList.Add(OnHand);
				_profileList.Add(Intransit);

				// System code -- no modifications beyond this point

				foreach (AssortmentTotalVariableProfile varProf in _profileList)
				{
					_nameHash.Add(varProf.VariableName, varProf);
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
	/// Class that defines the Detail Variables for AllocationSummary.
	/// </summary>

	[Serializable]
	public class AllocationSummaryDetailVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		public AssortmentDetailVariableProfile TotalPct = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalPct, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile TotalUnits = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnits, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnits), "UNITS", eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentDetailVariableProfile TotalUnitsPctToSet = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnitsPctToSet, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnitsPctToSet), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile TotalUnitsPctToAll = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnitsPctToAll, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnitsPctToAll), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile AvgUnits = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentDetailVariables.AvgUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile Index = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.Index, MIDText.GetTextOnly((int)eAssortmentDetailVariables.Index), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationSummaryComponentVariables.
		/// </summary>

		public AllocationSummaryDetailVariables()
			: base(eProfileType.AssortmentDetailVariable)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AllocationSummaryComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(TotalPct);
				_profileList.Add(TotalUnits);
				_profileList.Add(TotalUnitsPctToSet);
				_profileList.Add(TotalUnitsPctToAll);
				_profileList.Add(AvgUnits);
				_profileList.Add(Index);

				// System code -- no modifications beyond this point

				foreach (AssortmentDetailVariableProfile varProf in _profileList)
				{
					_nameHash.Add(varProf.VariableName, varProf);
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
	/// Class that defines the Quantity Variables for AllocationSummary.
	/// </summary>

	[Serializable]
	public class AllocationSummaryQuantityVariables : AssortmentQuantityVariables
	{
		//=======
		// FIELDS
		//=======

		public QuantityVariableProfile Value = new QuantityVariableProfile(1, "Value", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		public QuantityVariableProfile StoreAverage = new QuantityVariableProfile(2, "Store Average", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, true, eValueFormatType.GenericNumeric, 2);
		public QuantityVariableProfile Balance = new QuantityVariableProfile(3, "Balance", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		public QuantityVariableProfile Difference = new QuantityVariableProfile(4, "Difference", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		public QuantityVariableProfile Total = new QuantityVariableProfile(5, "Total", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		// END TT#2148 - stodd - Assortment totals do not include header values

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of QuantityVariables.
		/// </summary>

		public AllocationSummaryQuantityVariables()
			: base(eProfileType.AssortmentQuantityVariable)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		override public QuantityVariableProfile ValueVariableProfile
		{
			get
			{
				return Value;
			}
		}

		override public QuantityVariableProfile DifferenceVariableProfile
		{
			get
			{
				return Difference;
			}
		}

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		override public QuantityVariableProfile TotalVariableProfile
		{
			get
			{
				return Total;
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values
		
		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AllocationSummaryComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(Value);
				_profileList.Add(StoreAverage);
				_profileList.Add(Balance);
				_profileList.Add(Difference);
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				_profileList.Add(Total);
				// END TT#2148 - stodd - Assortment totals do not include header values

				// System code -- no modifications beyond this point

				foreach (QuantityVariableProfile varProf in _profileList)
				{
					_nameHash.Add(varProf.VariableName, varProf);
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