using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Tools class contains routines that are used by the formulas, spreads, initializations, and change rules.
	/// </summary>

	abstract public class BaseToolBox
	{

		//=======
		// FIELDS
		//=======

		protected BaseComputations _computations;
		protected PlanSpread _planSpread;

		//=============
		// CONSTRUCTORS
		//=============

		public BaseToolBox(BaseComputations aComputations)
		{
			_computations = aComputations;
			_planSpread = new PlanSpread();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Scheduled all AutoTotals for a given cell.
        ///// </summary>
        ///// <param name="aCompSchd">
        ///// The ComputationSchedule to insert the Init in.
        ///// </param>
        ///// <param name="aCompCellRef">
        ///// The ComputationCellReference that identifies the cell being computed.
        ///// </param>
        ///// <param name="aVariableProfile">
        ///// The Id of this VariableProfile will be substituted into the CellReference before being added to the
        ///// schedule.
        ///// </param>
		
        //public void ScheduleAutoTotals(
        //    ComputationSchedule aCompSchd,
        //    ComputationCellReference aCompCellRef,
        //    ComputationVariableProfile aVariableProfile)
        //{
        //    ComputationCellReference compCellRef;

        //    try
        //    {
        //        compCellRef = (ComputationCellReference)aCompCellRef.Copy();
        //        compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;

        //        aCompSchd.ScheduleAutoTotals(compCellRef);
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference and FormulaSpreadProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula)
		{
			try
			{
				aCompSchd.InsertFormula(aCompCellRef, (ComputationCellReference)aCompCellRef.Copy(), aFormula);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, and
		/// VariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula,
			ComputationVariableProfile aVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;

				aCompSchd.InsertFormula(aCompCellRef, compCellRef, aFormula);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, VariableProfile, and QuantityVariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;

				aCompSchd.InsertFormula(aCompCellRef, compCellRef, aFormula);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, and QuantityVariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;

				aCompSchd.InsertFormula(aCompCellRef, compCellRef, aFormula);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule and ComputationCellReference.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompSchd.InsertInitFormula(aCompCellRef, (ComputationCellReference)aCompCellRef.Copy());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, ComputationCellReference, and
		/// VariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;

				aCompSchd.InsertInitFormula(aCompCellRef, compCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, ComputationCellReference, VariableProfile, and QuantityVariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;

				aCompSchd.InsertInitFormula(aCompCellRef, compCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, ComputationCellReference, and QuantityVariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;

				aCompSchd.InsertInitFormula(aCompCellRef, compCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an auto Init into the schedule queue for the given ComputationSchedule and ComputationCellReference.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>

		public void InsertAutoTotalFormula(
			ComputationSchedule aCompSchd,
			ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompSchd.InsertAutoTotalFormula(aCompCellRef, (ComputationCellReference)aCompCellRef.Copy());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aCompSchd, aSpreadFromCellRef, aSpread, null);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			ComputationVariableProfile aVariableProfile,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aCompSchd, aSpreadFromCellRef, aVariableProfile, aSpread, null);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
        /// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
        /// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aCompSchd, aSpreadFromCellRef, aQuantityVariableProfile, aSpread, null);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell in cascade spreads.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aSpreadFromCellRef.Copy();
				aCompSchd.InsertSpread(compCellRef, aSpread.GetSpreadToCellReferenceList(compCellRef), aSpread, aCascadeChangeMethodProf);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell in cascade spreads.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			ComputationVariableProfile aVariableProfile,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aSpreadFromCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				aCompSchd.InsertSpread(compCellRef, aSpread.GetSpreadToCellReferenceList(compCellRef), aSpread, aCascadeChangeMethodProf);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The Id of this QuantityVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
        /// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell in cascade spreads.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aSpreadFromCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				aCompSchd.InsertSpread(compCellRef, aSpread.GetSpreadToCellReferenceList(compCellRef), aSpread, aCascadeChangeMethodProf);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given ComputationCellReference to the ArrayList of ComputationCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aSpreadToCellRefs">
		/// An ArrayList of ComputationCellReferences that identify the cells being spread to.
		/// </param>
        /// <param name="aSpread">
		/// The SpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			ComputationCellReference aSpreadFromCellRef,
			ArrayList aSpreadToCellRefs,
			SpreadProfile aSpread)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aSpreadFromCellRef.Copy();
				aCompSchd.InsertSpread(compCellRef, aSpreadToCellRefs, aSpread);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the VariableProfile with the given variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aVariableProfile, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the VariableProfile with the given variable ID and the QuantityVariableProfile with the
		/// given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aVariableProfile, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			bool aCheckPending)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, compCellRef);
				}
				return compCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the VariableProfile with the given variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			bool aCheckPending)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, compCellRef);
				}
				return compCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, compCellRef);
				}
				return compCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the VariableProfile with the given variable ID and the QuantityVariableProfile with the
		/// given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The ComputationCellReference of the PlanCubeCell.
		/// </returns>

		public ComputationCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, compCellRef);
				}
				return compCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the VariableProfile with the given variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the VariableProfile with the given variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value, substituting the QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value, substituting the QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value, substituting the VariableProfile with the given variable ID and the QuantityVariableProfile with the
		/// given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef, aVariableProfile, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value, substituting the VariableProfile with the given variable ID and the QuantityVariableProfile with the
		/// given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aVariableProfile, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a count of all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <returns>
		/// An integer containing the count of detail ComputationCells.
		/// </returns>

		public int GetDetailCellCount(ComputationCellReference aCompCellRef, bool aUseHiddenValues)
		{
			try
			{
				return aCompCellRef.GetComponentDetailCount(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a count of all the detail ComputationCells for a total cell of the given QuantityVariableProfile type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An integer containing the count of detail ComputationCells.
		/// </returns>

		public int GetDetailCellCount(ComputationCellReference aCompCellRef, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				return aCompCellRef.GetComponentDetailCount(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of ComputationCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <returns>
		/// An ArrayList of ComputationCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(ComputationCellReference aCompCellRef, bool aUseHiddenValues)
		{
			try
			{
				return aCompCellRef.GetComponentDetailCellRefArray(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of ComputationCellReference objects that point to all the detail ComputationCells for a total cell of the given QuantityVariableProfile type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An ArrayList of ComputationCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(ComputationCellReference aCompCellRef, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				return compCellRef.GetComponentDetailCellRefArray(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of ComputationCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <returns>
		/// An ArrayList of ComputationCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(ComputationCellReference aCompCellRef, bool aUseHiddenValues)
		{
			try
			{
				return aCompCellRef.GetSpreadDetailCellRefArray(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of ComputationCellReference objects that point to all the detail ComputationCells for a total cell of the given QuantityVariableProfile type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An ArrayList of ComputationCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(ComputationCellReference aCompCellRef, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				return compCellRef.GetSpreadDetailCellRefArray(aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			double aValue)
		{
			try
			{
				SetCellValue(aSetCellMode, aCompCellRef, false, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aIgnoreTempLocks">
		/// Indicates whether the temp-locked flag should be checked before updating.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			bool aIgnoreTempLocks,
			double aValue)
		{
			try
			{
				if (!aIgnoreTempLocks && aSetCellMode == eSetCellMode.Computation && aCompCellRef.isCellCompChanged)
				{
					throw new CellCompChangedException();
				}

				if (double.IsNaN(aValue) || double.IsInfinity(aValue))
				{
					aValue = 0;
				}

				switch (aSetCellMode)
				{
					case eSetCellMode.Entry:

						aCompCellRef.SetEntryCellValue(aValue);
						break;

					//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
					//case eSetCellMode.AutoTotal:
					//case eSetCellMode.ForcedReInit:

					//    if (aCompCellRef.isCellInitialized)
					//    {
					//        aCompCellRef.SetCompCellValue(aSetCellMode, aValue);
					//    }
					//    break;

					//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
					default:

						aCompCellRef.SetCompCellValue(aSetCellMode, aValue);
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before setting.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			double aValue)
		{
			try
			{
				SetCellValue(aSetCellMode, aCompCellRef, aVariableProfile, false, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before setting.
		/// </param>
		/// <param name="aIgnoreTempLocks">
		/// Indicates whether the temp-locked flag should be checked before updating.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			bool aIgnoreTempLocks,
			double aValue)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				SetCellValue(aSetCellMode, compCellRef, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before setting.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			double aValue)
		{
			try
			{
				SetCellValue(aSetCellMode, aCompCellRef, aQuantityVariableProfile, false, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before setting.
		/// </param>
		/// <param name="aIgnoreTempLocks">
		/// Indicates whether the temp-locked flag should be checked before updating.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aIgnoreTempLocks,
			double aValue)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				SetCellValue(aSetCellMode, compCellRef, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before setting.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before setting.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			double aValue)
		{
			try
			{
				SetCellValue(aSetCellMode, aCompCellRef, aVariableProfile, aQuantityVariableProfile, false, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the given value.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before setting.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before setting.
		/// </param>
		/// <param name="aIgnoreTempLocks">
		/// Indicates whether the temp-locked flag should be checked before updating.
		/// </param>
		/// <param name="aValue">
		/// The value to set the PlanCubeCell's value to.
		/// </param>

		public void SetCellValue
			(eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aIgnoreTempLocks,
			double aValue)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.VariableProfileType] = aVariableProfile.Key;
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuantityVariableProfile.Key;
				SetCellValue(aSetCellMode, compCellRef, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Marks the given ComputationCellReference as a null cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to mark as null.
		/// </param>

		public void SetCellNull(ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompCellRef.isCellNull = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Marks the given ComputationCellReference as a null cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to mark as null.
		/// </param>

		public void SetCellCompLocked(ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompCellRef.isCellCompLocked = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Marks the given ComputationCellReference as comp changed.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to mark comp changed.
		/// </param>

		public void SetCellChanged(ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompCellRef.isCellCompChanged = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds up the values of all the detail cells for the given ComputationCellReference.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to sum the detail for.
		/// </param>
		/// <returns>
		/// A double containing the sum of the detail cells.
		/// </returns>

		public double SumDetailComponents(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, ComputationCellReference aCompCellRef)
		{
			try
			{
				return aCompCellRef.GetComponentDetailSum(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds up the values of all the detail cells for the given ComputationCellReference.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to sum the detail for.
		/// </param>
		/// <returns>
		/// A double containing the sum of the detail cells.
		/// </returns>

		public double SumDetailComponents(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, ComputationCellReference aCompCellRef, bool aUseHiddenValues)
		{
			try
			{
				return aCompCellRef.GetComponentDetailSum(aScheduleEntry, aGetCellMode, aSetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <returns>
		/// The average of the ArrayList of cells.
		/// </returns>

		public double CalculateAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef)
		{
			try
			{
				return aCompCellRef.GetComponentDetailAverage(aScheduleEntry, aGetCellMode, aSetCellMode, aCompCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <returns>
		/// The average of the ArrayList of cells.
		/// </returns>

		public double CalculateAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return aCompCellRef.GetComponentDetailAverage(aScheduleEntry, aGetCellMode, aSetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <returns>
		/// The average of the ArrayList of cells.
		/// </returns>

		public double CalculateAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuanVarProf)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuanVarProf.Key;
				return compCellRef.GetComponentDetailAverage(aScheduleEntry, aGetCellMode, aSetCellMode, compCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the sales average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCellRefArray">
		/// The ArrayList of cells.
		/// </param>
		/// <param name="aStoreCount">
		/// An output parameter that contains the count of cells used in the average calculation.
		/// </param>
		/// <returns>
		/// The sales average of the ArrayList of cells.
		/// </returns>

		public double CalculateAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ArrayList aCellRefArray,
			out int aStoreCount)
		{
			try
			{
				return aCompCellRef.GetComponentDetailAverage(aScheduleEntry, aGetCellMode, aSetCellMode, aCellRefArray, out aStoreCount, aCompCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the sales average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aInventoryVar">
		/// A VariableProfile that defines the inventory variable.
		/// </param>
		/// <returns>
		/// The sales average of the ArrayList of cells.
		/// </returns>

		public double CalculateNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			QuantityVariableProfile aQuanVarProf,
			ComputationVariableProfile aInventoryVar)
		{
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCompCellRef.Copy();
				compCellRef[compCellRef.QuantityVariableProfileType] = aQuanVarProf.Key;
				return compCellRef.GetComponentDetailNonZeroAverage(aScheduleEntry, aGetCellMode, aSetCellMode, aInventoryVar, aCompCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Computes the sales average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCellRefArray">
		/// The ArrayList of cells.
		/// </param>
		/// <param name="aInventoryVar">
		/// A VariableProfile that defines the inventory variable.
		/// </param>
		/// <param name="aStoreCount">
		/// An output parameter that contains the count of cells used in the average calculation.
		/// </param>
		/// <returns>
		/// The sales average of the ArrayList of cells.
		/// </returns>

		public double CalculateNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef,
			ArrayList aCellRefArray,
			ComputationVariableProfile aInventoryVar,
			out int aStoreCount)
		{
			try
			{
				return aCompCellRef.GetComponentDetailNonZeroAverage(aScheduleEntry, aGetCellMode, aSetCellMode, aCellRefArray, aInventoryVar, out aStoreCount, aCompCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Spreads a given value to the given ArrayList of cells.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aSpreadCellRefList">
		/// The ArrayList of ComputationCellReferences to spread the value to.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals to round the output values to.
		/// </param>
		/// <param name="aSpreadValue">
		/// The value to spread.
		/// </param>

		public void ExecutePctContributionSpread(
			ComputationScheduleSpreadEntry aSpreadEntry,
			eSetCellMode aSetCellMode,
			ArrayList aSpreadCellRefList,
			int aNumDecimals,
			double aSpreadValue)
		{
			ArrayList compCellValueList;
			ArrayList compCellRefList;
			int i;

			try
			{
				_planSpread.ExecuteSimpleSpread(
					aSpreadValue,
					aSpreadCellRefList,
					aSpreadEntry.ExcludedCellRefHash,
					aNumDecimals,
					out compCellValueList,
					out compCellRefList);

				for (i = 0; i < compCellRefList.Count; i++)
				{
					SetCellValue(aSetCellMode, (ComputationCellReference)compCellRefList[i], (double)compCellValueList[i]);
				}
			}
			catch (NothingToSpreadException)
			{
				throw new CellCompChangedException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Spreads a given value to the given ArrayList of cells.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aSpreadCellRefList">
		/// The ArrayList of ComputationCellReferences to spread the value to.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals to round the output values to.
		/// </param>
        /// <param name="aOldFromValue">
		/// The value to spread.
		/// </param>

		public void ExecutePctChangeSpread(
			ComputationScheduleSpreadEntry aSpreadEntry,
			eSetCellMode aSetCellMode,
			ArrayList aSpreadCellRefList,
			int aNumDecimals,
			double aOldFromValue,
			double aNewFromValue)
		{
			ArrayList compCellValueList;
			ArrayList compCellRefList;
			int i;

			try
			{
				_planSpread.ExecutePctChangeSpread(
					aOldFromValue,
					aNewFromValue,
					aSpreadCellRefList,
					aSpreadEntry.ExcludedCellRefHash,
					aNumDecimals,
					out compCellValueList,
					out compCellRefList);

				for (i = 0; i < compCellRefList.Count; i++)
				{
					SetCellValue(aSetCellMode, (ComputationCellReference)compCellRefList[i], (double)compCellValueList[i]);
				}
			}
			catch (NothingToSpreadException)
			{
				throw new CellCompChangedException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Spreads a given value to the given ArrayList of cells.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aSpreadCellRefList">
		/// The ArrayList of ComputationCellReferences to spread the value to.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals to round the output values to.
		/// </param>
		/// <param name="aSpreadValue">
		/// The value to spread.
		/// </param>

		public void ExecutePlugSpread(
			ComputationScheduleSpreadEntry aSpreadEntry,
			eSetCellMode aSetCellMode,
			ArrayList aSpreadCellRefList,
			int aNumDecimals,
			double aSpreadValue)
		{
			ArrayList compCellValueList;
			ArrayList compCellRefList;
			int i;

			try
			{
				_planSpread.ExecutePlugSpread(
					aSpreadValue,
					aSpreadCellRefList,
					aSpreadEntry.ExcludedCellRefHash,
					aNumDecimals,
					out compCellValueList,
					out compCellRefList);

				for (i = 0; i < compCellRefList.Count; i++)
				{
					SetCellValue(aSetCellMode, (ComputationCellReference)compCellRefList[i], (double)compCellValueList[i]);
				}
			}
			catch (NothingToSpreadException)
			{
				throw new CellCompChangedException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Protected method that checks a given ComputationCellReference to see if it is pending.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being checked.
		/// </param>

		public bool isCellPending(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef)
		{
			try
			{
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //if (aSetCellMode == eSetCellMode.Computation || aSetCellMode == eSetCellMode.AutoTotal || aSetCellMode == eSetCellMode.ForcedReInit)
                if (aSetCellMode == eSetCellMode.Computation || aSetCellMode == eSetCellMode.AutoTotal)
                //End TT#2 - JScott - Assortment Planning - Phase 2
                {
					if (aCompCellRef.isCellFormulaPending(aScheduleEntry) || aCompCellRef.isCellSpreadPending(aScheduleEntry))
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Protected method that checks a given ComputationCellReference to see if it is pending.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to the PlanCubeCell being checked.
		/// </param>

		protected void intCheckOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			ComputationCellReference aCompCellRef)
		{
			try
			{
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //if (aSetCellMode == eSetCellMode.Computation || aSetCellMode == eSetCellMode.AutoTotal || aSetCellMode == eSetCellMode.ForcedReInit)
                if (aSetCellMode == eSetCellMode.Computation || aSetCellMode == eSetCellMode.AutoTotal)
                //End TT#2 - JScott - Assortment Planning - Phase 2
                {
					if (aCompCellRef.isCellFormulaPending(aScheduleEntry) || aCompCellRef.isCellSpreadPending(aScheduleEntry))
					{
					    // Begin TT#1954-MD - JSmith - Assortment Performance
                        aScheduleEntry.LastPendingCell = aCompCellRef;
                        aScheduleEntry.ComputationFormulaReturnType = eComputationFormulaReturnType.Pending;
                        //throw new CellPendingException(aCompCellRef);
						// End TT#1954-MD - JSmith - Assortment Performance
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
