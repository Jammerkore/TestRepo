using System;
using System.Collections;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Class that defines the Component Variables for AssortmentView.
	/// </summary>

	[Serializable]
	public class AssortmentViewComponentVariables : AssortmentComponentVariables
	{
		//=======
		// FIELDS
		//=======

		public AssortmentComponentVariableProfile Assortment = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Assortment, eProfileType.AssortmentHeader, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Assortment), "ASSORTMENT", false, false);
		public AssortmentComponentVariableProfile Placeholder = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Placeholder, eProfileType.PlaceholderHeader, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Placeholder), "PLACEHOLDER", true, false);
		public AssortmentComponentVariableProfile PlanLevel = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.PlanLevel, eProfileType.PlanLevel, MIDText.GetTextOnly((int)eAssortmentComponentVariables.PlanLevel), "PLANLEVEL", false, false);
		public AssortmentComponentVariableProfile HeaderID = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.HeaderID, eProfileType.AllocationHeader, MIDText.GetTextOnly((int)eAssortmentComponentVariables.HeaderID), "HEADER", true, true);
		public AssortmentComponentVariableProfile Pack = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Pack, eProfileType.HeaderPack, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Pack), "PACK", true, false);
		public AssortmentComponentVariableProfile Color = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.Color, eProfileType.HeaderPackColor, MIDText.GetTextOnly((int)eAssortmentComponentVariables.Color), "COLOR", true, false);
		// Begin TT#1227 - stodd - sort seq *REMOVED for TT#1322*
		//public AssortmentComponentVariableProfile PlaceholderSeq = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.PlaceholderSeq, eProfileType.PlaceholderSeq, MIDText.GetTextOnly((int)eAssortmentComponentVariables.PlaceholderSeq), "PLACEHOLDERSEQ", false, false);
		//public AssortmentComponentVariableProfile HeaderSeq = new AssortmentComponentVariableProfile((int)eAssortmentComponentVariables.HeaderSeq, eProfileType.HeaderSeq, MIDText.GetTextOnly((int)eAssortmentComponentVariables.HeaderSeq), "HEADERSEQ", false, false);
		// End TT#1227 - stodd - sort seq

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentViewComponentVariables.
		/// </summary>

		public AssortmentViewComponentVariables()
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
		/// Initializes this instance of AssortmentViewComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(Assortment);
				_profileList.Add(Placeholder);
				_profileList.Add(PlanLevel);
				_profileList.Add(HeaderID);
				_profileList.Add(Pack);
				_profileList.Add(Color);
				// Begin TT#1227 - stodd - sort seq *REMOVED for TT#1322*
				//_profileList.Add(PlaceholderSeq);
				//_profileList.Add(HeaderSeq);
				// End TT#1227 - stodd - sort seq

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
		/// Method that creates a copy of this object.
		/// </summary>
		/// <returns>
		/// The copy of this object.
		/// </returns>

		override public AssortmentComponentVariables Copy()
		{
			try
			{
				return (AssortmentComponentVariables)CopyTo(new AssortmentViewComponentVariables());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the Summary Variables for AssortmentView.
	/// </summary>

	[Serializable]
	public class AssortmentViewSummaryVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		public AssortmentSummaryVariableProfile Units = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Units, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Units), "UNITS", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile AvgStore = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.AvgStore, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.AvgStore), "AVG_STORE", eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		//public AssortmentSummaryVariableProfile AvgUnits = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.AvgUnits), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentSummaryVariableProfile AvgUnits = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.AvgUnits), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		public AssortmentSummaryVariableProfile Basis = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Basis, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Basis), "BASIS", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile Index = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Index, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Index), "INDEX", eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentSummaryVariableProfile Intransit = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Intransit, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Intransit), "INTRANSIT", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile Need = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Need, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Need), "NEED", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile NumStores = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.NumStores, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.NumStores), "NUM_STORES", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentSummaryVariableProfile PctNeed = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.PctNeed, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.PctNeed), "PCT_NEED", eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
        // Begin TT#1725 - RMatelic - "Hide" Committed
        // Begin TT#1224 - stodd - committed
        //public AssortmentSummaryVariableProfile Committed = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Committed, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Committed), "COMMITTED", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#1224 - stodd - committed
        // End TT#1725
		// BEGIN TT#845-MD - Stodd - add OnHand to Summary
		public AssortmentSummaryVariableProfile OnHand = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.OnHand, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.OnHand), "ONHAND", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// END TT#845-MD - Stodd - add OnHand to Summary
		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public AssortmentSummaryVariableProfile VSWOnHand = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.VSWOnHand, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.VSWOnHand), "VSW_ONHAND", eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review
		// Begin TT#3817 - stodd - Saving View with Balance checked -
		// Needed for Row selection in summary only. Never displayed on summary.
        public AssortmentSummaryVariableProfile Balance = new AssortmentSummaryVariableProfile((int)eAssortmentSummaryVariables.Balance, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Balance), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#3817 - stodd - Saving View with Balance checked -


		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentViewComponentVariables.
		/// </summary>

		public AssortmentViewSummaryVariables()
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
		/// Initializes this instance of AssortmentViewComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(Units);
				_profileList.Add(AvgStore);
				_profileList.Add(AvgUnits);
				_profileList.Add(Basis);
				_profileList.Add(Index);
				_profileList.Add(Intransit);
                // Begin TT#1725 - RMatelic - "Hide" Committed
                //_profileList.Add(Committed);	// TT#1224 - stodd - add committed
                // End TT#1725
				_profileList.Add(Need);
				_profileList.Add(NumStores);
				_profileList.Add(PctNeed);
				_profileList.Add(OnHand);	// TT#845-MD - Stodd - add OnHand to Summary
                _profileList.Add(VSWOnHand);	// TT#952 - MD - stodd - add matrix to Group Allocation Review
                _profileList.Add(Balance);		// TT#3817 - stodd - Saving View with Balance checked -


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
	/// Class that defines the Total Variables for AssortmentView.
	/// </summary>

	[Serializable]
	public class AssortmentViewTotalVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		//Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
		//public AssortmentTotalVariableProfile TotalPct = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalPct, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        public AssortmentTotalVariableProfile TotalPct = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalPct, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
		//End TT#1143 - JScott - Total % change receives Nothing to Spread exception
		public AssortmentTotalVariableProfile TotalUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eValueFormatType.GenericNumeric, 0);
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		//public AssortmentTotalVariableProfile HeaderUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.HeaderUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.HeaderUnits), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// END TT#2148 - stodd - Assortment totals do not include header values
		public AssortmentTotalVariableProfile ReserveUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.ReserveUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.ReserveUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile AvgUnits = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentTotalVariables.AvgUnits), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile TotalRetail = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalRetail, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalRetail), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile TotalCost = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.TotalCost, MIDText.GetTextOnly((int)eAssortmentTotalVariables.TotalCost), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile UnitRetail = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.UnitRetail, MIDText.GetTextOnly((int)eAssortmentTotalVariables.UnitRetail), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile UnitCost = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.UnitCost, MIDText.GetTextOnly((int)eAssortmentTotalVariables.UnitCost), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentTotalVariableProfile MUPct = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.MUPct, MIDText.GetTextOnly((int)eAssortmentTotalVariables.MUPct), null, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		// Begin TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated
		public AssortmentTotalVariableProfile Balance = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Balance, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Balance), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		//public AssortmentTotalVariableProfile Balance = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Balance, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Balance), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// END TT#2148 - stodd - Assortment totals do not include header values
		//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		public AssortmentTotalVariableProfile OnHand = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.OnHand, MIDText.GetTextOnly((int)eAssortmentTotalVariables.OnHand), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		public AssortmentTotalVariableProfile Intransit = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Intransit, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Intransit), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
        // Begin TT#1725 - RMatelic - "Hide" Committed
        // Begin TT#1224 - stodd - add comitted
        //public AssortmentTotalVariableProfile Committed = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Committed, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Committed), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
	    // End TT#1224 - stodd - add comitted
	    // End TT#1725
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
        //public AssortmentTotalVariableProfile Multiple = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Multiple, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Multiple), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// Begin TT#2 - stodd - assortment
		//public AssortmentTotalVariableProfile Minimum = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Minimum, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Minimum), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		//public AssortmentTotalVariableProfile Maximum = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.Maximum, MIDText.GetTextOnly((int)eAssortmentTotalVariables.Maximum), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// END TT#2148 - stodd - Assortment totals do not include header values
		// End TT#2 - stodd - assortment
		// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
        public AssortmentTotalVariableProfile NumStoresAllocated = new AssortmentTotalVariableProfile((int)eAssortmentTotalVariables.NumStoresAllocated, MIDText.GetTextOnly((int)eAssortmentTotalVariables.NumStoresAllocated), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#4294 - stodd - Average Units in Matrix Enahancement

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentViewComponentVariables.
		/// </summary>

		public AssortmentViewTotalVariables()
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
		/// Initializes this instance of AssortmentViewComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(TotalPct);
				_profileList.Add(TotalUnits);
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				// "Hide" headerunits
				//_profileList.Add(HeaderUnits);
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				_profileList.Add(ReserveUnits);
				_profileList.Add(AvgUnits);
				_profileList.Add(TotalRetail);
				_profileList.Add(TotalCost);
				_profileList.Add(UnitRetail);
				_profileList.Add(UnitCost);
				_profileList.Add(MUPct);
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				// "Hide" Balance
				_profileList.Add(Balance);	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated
				// End TT#2148 - stodd - Assortment totals do not include header values
				_profileList.Add(OnHand);
				_profileList.Add(Intransit);
                // Begin TT#1725 - RMatelic - "Hide" Committed
                //_profileList.Add(Committed);	// TT#1224 - stodd
                // End TT#1725
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				// "hide" Multiple, Min and Max
				//_profileList.Add(Multiple);
				// Begin TT#2 - stodd - assortment
				//_profileList.Add(Minimum);
				//_profileList.Add(Maximum);
				// End TT#2 - stodd - assortment
				// END TT#2148 - stodd - Assortment totals do not include header values
				// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                _profileList.Add(NumStoresAllocated);
				// End TT#4294 - stodd - Average Units in Matrix Enahancement

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
	/// Class that defines the Detail Variables for AssortmentView.
	/// </summary>

	[Serializable]
	public class AssortmentViewDetailVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        public AssortmentDetailVariableProfile TotalPct = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalPct, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalPct), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
		public AssortmentDetailVariableProfile TotalUnits = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnits, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnits), "UNITS", eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eValueFormatType.GenericNumeric, 0);
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		//public AssortmentDetailVariableProfile TotalUnitsPctToSet = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnitsPctToSet, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnitsPctToSet), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		// END TT#2148 - stodd - Assortment totals do not include header values
		public AssortmentDetailVariableProfile TotalUnitsPctToAll = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.TotalUnitsPctToAll, MIDText.GetTextOnly((int)eAssortmentDetailVariables.TotalUnitsPctToAll), null, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile AvgUnits = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.AvgUnits, MIDText.GetTextOnly((int)eAssortmentDetailVariables.AvgUnits), null, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile Index = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.Index, MIDText.GetTextOnly((int)eAssortmentDetailVariables.Index), null, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile UnitRetail = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.UnitRetail, MIDText.GetTextOnly((int)eAssortmentDetailVariables.UnitRetail), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		public AssortmentDetailVariableProfile UnitCost = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.UnitCost, MIDText.GetTextOnly((int)eAssortmentDetailVariables.UnitCost), null, eVariableStyle.Dollar, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 2);
		// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
        public AssortmentDetailVariableProfile NumStoresAllocated = new AssortmentDetailVariableProfile((int)eAssortmentDetailVariables.NumStoresAllocated, MIDText.GetTextOnly((int)eAssortmentDetailVariables.NumStoresAllocated), null, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eValueFormatType.GenericNumeric, 0);
		// End TT#4294 - stodd - Average Units in Matrix Enahancement
		
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentViewComponentVariables.
		/// </summary>

		public AssortmentViewDetailVariables()
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
		/// Initializes this instance of AssortmentViewComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(TotalPct);
				_profileList.Add(TotalUnits);
				// BEGIN TT#2148 - stodd - Assortment totals do not include header values
				// "hide" TotalUnitsPctToSet
				//_profileList.Add(TotalUnitsPctToSet);
				// END TT#2148 - stodd - Assortment totals do not include header values
				_profileList.Add(TotalUnitsPctToAll);
				_profileList.Add(AvgUnits);
				_profileList.Add(Index);
				_profileList.Add(UnitRetail);
				_profileList.Add(UnitCost);
                _profileList.Add(NumStoresAllocated);	// TT#4294 - stodd - Average Units in Matrix Enahancement

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
	/// Class that defines the Quantity Variables for AssortmentView.
	/// </summary>

	[Serializable]
	public class AssortmentViewQuantityVariables : AssortmentQuantityVariables
	{
		//=======
		// FIELDS
		//=======

		public QuantityVariableProfile Value = new QuantityVariableProfile(1, "Value", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		//public QuantityVariableProfile Balance = new QuantityVariableProfile(2, "Balance", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		public QuantityVariableProfile Balance = new QuantityVariableProfile(2, "Balance", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		public QuantityVariableProfile Difference = new QuantityVariableProfile(3, "Difference", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		public QuantityVariableProfile Total = new QuantityVariableProfile(4, "Total", eVariableCategory.None, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
		// END TT#2148 - stodd - Assortment totals do not include header values

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of QuantityVariables.
		/// </summary>

		public AssortmentViewQuantityVariables()
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
		/// Initializes this instance of AssortmentViewComponentVariables.
		/// </summary>

		override public void Initialize()
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(Value);
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
